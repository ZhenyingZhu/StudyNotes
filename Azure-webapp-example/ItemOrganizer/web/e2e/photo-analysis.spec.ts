import { expect, Page, test } from '@playwright/test'

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

test('uploads, reviews, and assigns detected inventory', async ({ page }) => {
  await page.goto('/')
  await expect(
    page.getByRole('heading', { name: 'Know where everything lives.' }),
  ).toBeVisible()

  await uploadPhoto(page)

  const results = page.locator('.analysis-results')
  const cable = results.locator('.detection-card').filter({
    hasText: 'USB-C cable',
  })
  const notes = results.locator('.detection-card').filter({
    hasText: 'Sticky notes',
  })
  await expect(cable).toContainText('Confidence 92%')
  await expect(cable).toContainText('Suggested container: Office supplies')
  await expect(notes).toContainText('Assignment: unassigned')

  await cable.getByRole('button', { name: 'Accept suggestion' }).click()
  await expect(cable).toContainText('Assignment: confirmed')

  await notes
    .getByRole('combobox', { name: 'Change container for Sticky notes' })
    .selectOption({ label: 'Office supplies' })
  await expect(notes).toContainText('Assignment: confirmed')

  await page.getByRole('button', { name: 'Refresh' }).click()
  await expect(cable).toContainText('Assignment: confirmed')
  await expect(notes).toContainText('Assignment: confirmed')
})

test('convenience workflow confirms all detected items', async ({ page }) => {
  await page.goto('/')
  await page
    .getByLabel('Assign all detected items to a container (optional)')
    .selectOption({ label: 'Office supplies' })

  await uploadPhoto(page)

  const results = page.locator('.analysis-results')
  const cards = results.locator('.detection-card')
  await expect(cards).toHaveCount(2)
  await expect(cards.nth(0)).toContainText('Assignment: confirmed')
  await expect(cards.nth(1)).toContainText('Assignment: confirmed')
  await expect(
    results.getByRole('button', { name: 'Accept suggestion' }),
  ).toHaveCount(0)
})
