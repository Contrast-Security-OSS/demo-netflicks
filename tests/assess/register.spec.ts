import { test, expect } from '@playwright/test';

test.describe('unauthenticated tests', () => {
  test('register to use netflicks', async ({ page }) => {
    await page.goto('/');

    await page.locator('text=Register').click();
    await expect(page).toHaveURL('/Account/Register');

    var randomId = Math.random().toString(36).substring(2, 10) + Math.random().toString(36).substring(2, 10);
    await page.locator('input[name="Email"]').fill(randomId + '@contrast.co.uk');
    await page.locator('input[name="Password"]').fill('P@ssw0rd');
    await page.locator('input[name="ConfirmPassword"]').fill('P@ssw0rd');

    await page.locator('button:has-text("Register")').click();
    await expect(page.locator("text=Log Out")).toHaveCount(1)
  });

  test('verify error page', async ({ page }) => {
    await page.goto('/Home/Error');
    await expect(page).toHaveTitle('Error - DotNetFlicks');
  });

  test('reset password', async ({ page }) => {
    await page.goto('/Account/ForgotPassword');

    await page.locator('input[name="Email"]').fill('admin@dotnetflicks.com');
    await page.locator('button:has-text("Submit")').click();

    await expect(page).toHaveTitle('Forgot password confirmation - DotNetFlicks');
  })
})