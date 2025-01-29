import { test, expect } from '@playwright/test';

test.use({ storageState: './tmp/admin.json' })
test.describe('manage movies', () => {
  test('search for a movie', async ({ page }) => {
    await page.goto('/Movie');

    await page.locator('[placeholder="Search"]').fill('Tropic Thunder');
    await page.locator('.input-group-append > .btn').click();

    await page.locator('text=Tropic Thunder').click();
    await expect(page).toHaveTitle('Tropic Thunder - DotNetFlicks');
  })

  test('add a movie', async ({ page }) => {
    await page.goto('/Movie');

    await page.locator('text=New').click();
    await expect(page).toHaveTitle('New Movie - DotNetFlicks');

    await page.locator('input[name="Name"]').fill('Starwars');
    await page.locator('textarea[name="Description"]').fill('A classic');
    await page.locator('input[name="ReleaseDate"]').fill('2022-01-06');
    await page.locator('input[name="Runtime"]').fill('00:01:00');
    await page.locator('input[name="PurchaseCost"]').fill('10.00');
    await page.locator('input[name="RentCost"]').fill('5.00');
    await page.locator('input[name="ImageUrl"]').fill('https://image.tmdb.org/t/p/w600_and_h900_bestv2/eKi8dIrr8voobbaGzDpe8w0PVbC.jpg');

    await page.locator('text=Save').click();
    await expect(page).toHaveTitle('Movies - DotNetFlicks');
  })

  test('delete a movie', async ({ page }) => {
    await page.goto('Movie?sortOrder=Date_Desc&pageIndex=1&pageSize=10');

    await expect(page).toHaveTitle('Movies - DotNetFlicks');

    await page.locator('a:nth-child(2) > .svg-inline--fa > path').first().click();
    await page.locator('input:has-text("Delete")').click();

    await expect(page).toHaveTitle('Movies - DotNetFlicks');
  });
});

test.describe('manage people', () => {
  test('search for a person', async ({ page }) => {
    await page.goto('/Person');

    await page.locator('[placeholder="Search"]').fill('Woody Harrelson');
    await page.locator('.input-group-append > .btn').click();

    await page.locator('text=Woody Harrelson').click();
    await expect(page).toHaveTitle('Woody Harrelson - DotNetFlicks');
  })

  test('add a person', async ({ page }) => {
    await page.goto('/Person');

    await page.locator('text=New').click();
    await expect(page).toHaveTitle('New Person - DotNetFlicks');

    await page.locator('input[name="Name"]').fill('Arnold ');
    await page.locator('textarea[name="Biography"]').fill('I\`ll be back');
    await page.locator('input[name="BirthDate"]').fill('1960-01-01');

    await page.locator('text=Save').click();
    await expect(page).toHaveTitle('People - DotNetFlicks');
  })

  test('delete a person', async ({ page }) => {
    await page.goto('/Person');

    await page.locator('text=New').click();
    await expect(page).toHaveTitle('New Person - DotNetFlicks');

    await page.locator('input[name="Name"]').fill('Aabavaanan');
    await page.locator('textarea[name="Biography"]').fill('Won\`t last long');
    await page.locator('input[name="BirthDate"]').fill('1960-01-01');

    await page.locator('text=Save').click();
    await expect(page).toHaveTitle('People - DotNetFlicks');

    await page.locator('text=Aabavaanan 0 >> path').nth(1).click();
    await page.locator('input:has-text("Delete")').click();

    await expect(page).toHaveTitle('People - DotNetFlicks');
  });
});

test.describe('manage departments', () => {
  test('search for a department', async ({ page }) => {
    await page.goto('/Department');

    await page.locator('[placeholder="Search"]').fill('Writing');
    await page.locator('.input-group-append > .btn').click();

    await expect(page.locator("text=Writing")).toBeVisible();
  })

  test('add a department', async ({ page }) => {
    await page.goto('/Department');

    await page.locator('text=New').click();
    await expect(page).toHaveTitle('New Department - DotNetFlicks');

    await page.locator('input[name="Name"]').fill('Atmosphere');

    await page.locator('text=Save').click();
    await expect(page).toHaveTitle('Departments - DotNetFlicks');
  })

  test('delete a department', async ({ page }) => {
    await page.goto('/Department');

    await page.locator('text=New').click();
    await expect(page).toHaveTitle('New Department - DotNetFlicks');

    await page.locator('input[name="Name"]').fill('Atmosphere');

    await page.locator('text=Save').click();

    await expect(page).toHaveTitle('Departments - DotNetFlicks');

    await page.locator('text=Atmosphere 0 >> path').nth(1).click();

    await page.locator('input:has-text("Delete")').click();
    await expect(page).toHaveTitle('Departments - DotNetFlicks');
  });
})

test.describe('manage genres', () => {
  test('search for a genre', async ({ page }) => {
    await page.goto('/Genre');

    await page.locator('[placeholder="Search"]').fill('Action');
    await page.locator('.input-group-append > .btn').click();

    await page.locator('text=Action').click();
    await expect(page).toHaveTitle('Action - DotNetFlicks');
  })

  test('add a genre', async ({ page }) => {
    await page.goto('/Genre');

    await page.locator('text=New').click();
    await expect(page).toHaveTitle('New Genre - DotNetFlicks');

    await page.locator('input[name="Name"]').fill('Snuff');

    await page.locator('text=Save').click();
    await expect(page).toHaveTitle('Genres - DotNetFlicks');
  })

  test('delete a genre', async ({ page }) => {
    await page.goto('/Genre');

    //Add
    await page.locator('text=New').click();
    await expect(page).toHaveTitle('New Genre - DotNetFlicks');
    await page.locator('input[name="Name"]').fill('Aadvarks');
    await page.locator('text=Save').click();

    //Delete
    await expect(page).toHaveTitle('Genres - DotNetFlicks');
    await page.locator('text=Aadvarks 0 >> a').nth(2).click();
    await expect(page).toHaveTitle('Delete Aadvarks - DotNetFlicks');
    await page.locator('input:has-text("Delete")').click();

    await expect(page).toHaveTitle('Genres - DotNetFlicks');
  });
});
