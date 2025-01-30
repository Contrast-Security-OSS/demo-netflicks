import { test, expect } from '@playwright/test';

test.use({ storageState: './tmp/admin.json' })
test.describe('manage account', () => {
    test('edit phone number', async ({ page }) => {
      await page.goto('/');
      await page.locator('text=Hello admin@dotnetflicks.com!').click();
      await expect(page).toHaveTitle('Profile - DotNetFlicks');
  
      await page.locator('input[name="PhoneNumber"]').fill('07976 263876');
  
      await page.locator('text=Save').click();
      await expect(page).toHaveTitle('Profile - DotNetFlicks');
    })
  
    test('change password', async ({ page }) => {
      await page.goto('/Manage/ChangePassword');
  
      await page.locator('input[name="OldPassword"]').fill('p@ssWORD471');
      await page.locator('input[name="NewPassword"]').fill('p@ssWORD471');
      await page.locator('input[name="ConfirmPassword"]').fill('p@ssWORD471');
  
      await page.locator('text=Update password').click();
  
      await expect(page).toHaveTitle('Change password - DotNetFlicks');
    })

    test('manage external logins', async ({ page }) => {
      await page.goto('/Manage/ExternalLogins');
  
      await expect(page).toHaveTitle('Manage your external logins - DotNetFlicks');
    })

    test('manage 2fa', async ({ page }) => {
      await page.goto('/Manage/TwoFactorAuthentication');
  
      await expect(page).toHaveTitle('Two-factor authentication - DotNetFlicks');
    })

    test('enable authenticator', async ({ page }) => {
      await page.goto('/Manage/EnableAuthenticator');

      await page.locator('input[name="Code"]').fill('123456');
      await page.locator('text=Verify').click();
  
      await expect(page).toHaveTitle('Enable authenticator - DotNetFlicks');
    })

    test('access denied', async ({ page }) => {
      await page.goto('/Account/AccessDenied');
  
      await expect(page).toHaveTitle('Access Denied - DotNetFlicks');
    })

    test('lockout', async ({ page }) => {
      await page.goto('/Account/Lockout');
  
      await expect(page).toHaveTitle('Locked Out - DotNetFlicks');
    })

    test('reset password', async ({ page }) => {
      await page.goto('/Account/ResetPassword?code=123456');
  
      await expect(page).toHaveTitle('Reset password - DotNetFlicks');
    })

    test('reset password confirmation', async ({ page }) => {
      await page.goto('/Account/ResetPasswordConfirmation');
  
      await expect(page).toHaveTitle('Reset password confirmation - DotNetFlicks');
    })
  });