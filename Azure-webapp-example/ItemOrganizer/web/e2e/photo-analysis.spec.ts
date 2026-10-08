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

async function createPhoto(page: Page) {
  const fixturePage = await page.context().newPage()
  await fixturePage.setViewportSize({ width: 512, height: 512 })
  await fixturePage.setContent(`
    <style>
      html, body { margin: 0; width: 512px; height: 512px; }
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

async function uploadPhoto(page: Page) {
  await page.getByLabel('Photo').setInputFiles(await createPhoto(page))
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
  await cable.getByRole('spinbutton', { name: 'x for USB-C cable' }).fill('0.12')
  await notes.getByRole('checkbox', { name: 'Add to inventory' }).uncheck()

  await page.getByRole('button', { name: 'Confirm inventory' }).click()

  await expect(
    page.getByRole('status').filter({
      hasText: 'Confirmed detections were added to inventory.',
    }),
  ).toBeVisible()
  const inventory = page.locator('.inventory-table')
  const cableRow = inventory.locator('.inventory-row').filter({
    hasText: 'USB-C cable',
  })
  await expect(cableRow).toBeVisible()
  await expect(cableRow.getByAltText('USB-C cable crop')).toBeVisible()
  await expect(
    inventory.getByAltText('Sticky notes crop'),
  ).toHaveCount(0)
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
