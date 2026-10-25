import { expect, Page, test } from '@playwright/test'
import { execFileSync } from 'node:child_process'

test.beforeEach(() => {
  execFileSync(
    'dotnet',
    [
      'run',
      '--project',
      '../src/ItemOrganizer.Database',
      '--no-restore',
      '--',
      'reset-and-seed',
    ],
    { stdio: 'inherit' },
  )
})

async function createPhoto(page: Page, width = 512, height = 512) {
  const fixturePage = await page.context().newPage()
  await fixturePage.setViewportSize({ width, height })
  await fixturePage.setContent(`
    <style>
      html, body { margin: 0; width: ${width}px; height: ${height}px; }
      body { background: linear-gradient(135deg, #234f3d, #dce7cf); }
    </style>
  `)
  const buffer = await fixturePage.screenshot({ type: 'png' })
  await fixturePage.close()
  return {
    name: 'inventory.png',
    mimeType: 'image/png',
    buffer,
  }
}

async function uploadPhoto(page: Page, width = 512, height = 512) {
  await page.getByLabel('Photo').setInputFiles(await createPhoto(page, width, height))
  await expect(page.getByAltText('Selected source')).toBeVisible()
  await page.getByRole('button', { name: 'Upload and analyze' }).click()
  await expect(page.getByText('Analysis completed')).toBeVisible({
    timeout: 30_000,
  })
}

test('reviews geometry, confirms a crop, and displays it in inventory', async ({
  page,
}) => {
  await page.goto('/')
  await expect(
    page.getByRole('heading', { name: 'Know where everything lives.' }),
  ).toBeVisible()

  await uploadPhoto(page)

  const results = page.locator('.analysis-results')
  const cable = results.locator('.detection-card').filter({
    hasText: 'Confidence 92%',
  })
  const notes = results.locator('.detection-card').filter({
    hasText: 'Confidence 73%',
  })
  await expect(cable).toContainText('Confidence 92%')
  await expect(cable).toContainText('Suggested container: Office supplies')
  await expect(page.getByLabel('Bounding box for USB-C cable')).toBeVisible()
  const box = cable.getByRole('button', { name: 'Move crop for USB-C cable' })
  await box.scrollIntoViewIfNeeded()
  const beforeMove = await box.boundingBox()
  if (!beforeMove) throw new Error('Expected a visible crop box')
  await page.mouse.move(beforeMove.x + beforeMove.width / 2, beforeMove.y + beforeMove.height / 2)
  await page.mouse.down()
  await page.mouse.move(beforeMove.x + beforeMove.width / 2 + 20, beforeMove.y + beforeMove.height / 2 + 10)
  await page.mouse.up()
  await expect.poll(async () => (await box.boundingBox())?.x).toBeCloseTo(beforeMove.x + 20, 0)
  const resize = cable.getByRole('button', { name: 'Resize crop for USB-C cable' })
  const handle = await resize.boundingBox()
  const beforeResize = await box.boundingBox()
  if (!handle || !beforeResize) throw new Error('Expected a visible resize handle')
  await page.mouse.move(handle.x + handle.width / 2, handle.y + handle.height / 2)
  await page.mouse.down()
  await page.mouse.move(handle.x + handle.width / 2 + 15, handle.y + handle.height / 2 + 10)
  await page.mouse.up()
  await expect.poll(async () => (await box.boundingBox())?.width).toBeCloseTo(beforeResize.width + 15, 0)
  const reviewedBox = await box.evaluate((element) => ({
    x: parseFloat(element.style.left) / 100,
    y: parseFloat(element.style.top) / 100,
    width: parseFloat(element.style.width) / 100,
    height: parseFloat(element.style.height) / 100,
  }))
  await notes.getByRole('checkbox', { name: 'Add to inventory' }).uncheck()

  const confirmation = page.waitForRequest((request) =>
    request.url().endsWith('/confirm') && request.method() === 'POST',
  )
  await page.getByRole('button', { name: 'Confirm inventory' }).click()
  const payload = (await confirmation).postDataJSON()
  expect(payload.detections[0].boundingBox).toEqual({
    x: expect.closeTo(reviewedBox.x, 5),
    y: expect.closeTo(reviewedBox.y, 5),
    width: expect.closeTo(reviewedBox.width, 5),
    height: expect.closeTo(reviewedBox.height, 5),
  })

  await expect(
    page.getByRole('status').filter({
      hasText: 'Confirmed detections were added to inventory.',
    }),
  ).toBeVisible()
  const inventory = page.locator('.inventory-table')
  await expect(inventory.getByAltText('USB-C cable crop')).toBeVisible()
  await expect(
    inventory.getByAltText('Sticky notes crop'),
  ).toHaveCount(0)
  await expect(box).toBeDisabled()
})

test('aligns boxes to a portrait photo and supports adding and removing crops', async ({ page }) => {
  await page.setViewportSize({ width: 390, height: 844 })
  await page.goto('/')
  await uploadPhoto(page, 512, 1536)
  const cable = page.locator('.detection-card').filter({ hasText: 'Confidence 92%' })
  const canvas = cable.locator('.photo-canvas')
  const imageBounds = await canvas.locator('img').boundingBox()
  const canvasBounds = await canvas.boundingBox()
  expect(imageBounds).toEqual(canvasBounds)
  const box = cable.getByRole('button', { name: 'Move crop for USB-C cable' })
  const normalized = await box.evaluate((element) => ({
    x: parseFloat(element.style.left) / 100,
    width: parseFloat(element.style.width) / 100,
  }))
  const bounds = await box.boundingBox()
  if (!bounds || !imageBounds) throw new Error('Expected a photo and crop box')
  expect(bounds.x).toBeCloseTo(imageBounds.x + normalized.x * imageBounds.width, 0)
  expect(bounds.width).toBeCloseTo(normalized.width * imageBounds.width, 0)
  await cable.getByRole('button', { name: 'Remove crop' }).click()
  await expect(box).toHaveCount(0)
  await cable.getByRole('button', { name: 'Add crop' }).click()
  await expect(box).toBeVisible()
})

test('container-targeted review still requires confirmation', async ({
  page,
}) => {
  await page.goto('/')
  await page
    .getByLabel('Preselect a container for review (optional)')
    .selectOption({ label: 'Office supplies' })

  await uploadPhoto(page)

  const results = page.locator('.analysis-results')
  const cards = results.locator('.detection-card')
  await expect(cards).toHaveCount(2)
  await expect(
    results.getByRole('combobox', { name: 'Container for USB-C cable' }),
  ).toHaveValue('30000000-0000-0000-0000-000000000001')
  await expect(
    results.getByRole('combobox', { name: 'Container for Sticky notes' }),
  ).toHaveValue('30000000-0000-0000-0000-000000000001')

  await page.getByRole('button', { name: 'Confirm inventory' }).click()

  await expect(
    page.locator('.inventory-table img[alt$=" crop"]'),
  ).toHaveCount(2)
})
