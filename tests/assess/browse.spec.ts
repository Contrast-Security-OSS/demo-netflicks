import { test, expect } from '@playwright/test';

test.use({ storageState: './tmp/admin.json' })
test.describe('browse movies', () => {
  test('view all movies', async ({ page }) => {
    await page.goto('/');
    await page.click('text=Browse available movies');

    await expect(page).toHaveURL('/Movie/ViewAll');
  })

  test('view movie', async ({ page, context }) => {
    await page.goto('/Movie/View/399055');

    await expect(page).toHaveTitle('The Shape of Water - DotNetFlicks');
  })

  test('view genre', async ({ page, context }) => {
    await page.goto('/Genre/View/28');

    await expect(page).toHaveTitle('Action - DotNetFlicks');
  })

});

test.describe('purchase', () => {
  test('rent movie', async ({ page }) => {
    await page.goto('/Movie/View/122');

    const count = await page.locator('text=Watch').count();
    if (count == 0) {
      await page.locator('text=Rent ($1.99)').click();
      await page.locator('#rent-modal >> text=I promise to pay').click();
    }
    await expect(page).toHaveTitle('The Lord of the Rings: The Return of the King - DotNetFlicks');
  })

  test('buy movie', async ({ page }) => {
    await page.goto('/Movie/View/433808');

    const count = await page.locator('text=Watch').count();
    if (count == 0) {    
      await page.locator('text=Buy ($13.99)').click();
      await page.locator('#buy-modal >> text=I promise to pay').click();
    }
    await expect(page).toHaveTitle('The Ritual - DotNetFlicks');
  })
});

test.describe('log out', () => {
  test('log out', async ({ page }) => {
    await page.goto('/');
    await page.locator('button:has-text("Log out")').click();

    await expect(page.locator("text=Log In")).toHaveCount(1)
  })
});


