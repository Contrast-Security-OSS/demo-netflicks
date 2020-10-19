const puppeteer = require('puppeteer');

(async () => {
  if (!process.env.BASEURL) {
      console.log('Please specify a base url. E.g. `BASEURL=http://example.org node exercise.js`');
  } else {
    var browser;

    if (process.env.DEBUG) {
      browser = await puppeteer.launch({
          headless: false,
          executablePath: '/Applications/Google Chrome.app/Contents/MacOS/Google Chrome',
      });
    } else {
      browser = await puppeteer.launch();
    }

    function delay(time) {
      return new Promise(function(resolve) { 
          setTimeout(resolve, time)
      });
    }

    const page = await browser.newPage()
    await page.setViewport({ width: 1366, height: 768});
    page.setDefaultNavigationTimeout(120000) //2 mins

    const navigationPromise = page.waitForNavigation()

    const pageOptions = {waitUntil: 'domcontentloaded'}
    const selectorOptions = {"timeout": 120000} //2 mins

    try {
      //Add error handling here in case the endpoint is not ready.
      await page.goto(process.env.BASEURL, pageOptions);
    } catch (error) {
      console.log(error);
      browser.close();
    }

    //Log in
    console.log('Logging in');
    await page.goto(process.env.BASEURL + '/Movie/ViewAll', pageOptions);

    await page.waitForSelector('input#Email', selectorOptions);
    await page.type('input#Email', 'admin@dotnetflicks.com');

    await page.waitForSelector('input#Password', selectorOptions)
    await page.type('input#Password', 'p@ssWORD471')

    await page.waitForSelector('form button', selectorOptions)
    await page.click('form button')

    await navigationPromise

    await delay(5000);

    //Browse Movies
    console.log('Browse movies');
    await page.goto(process.env.BASEURL + '/Movie/ViewAll', pageOptions);

    //Movies for user
    console.log('Movies for user');
    await page.goto(process.env.BASEURL + '/Movie/ViewAllForUser', pageOptions);

    //Movies admin
    console.log('Movies search');
    await page.goto(process.env.BASEURL + '/Movie', pageOptions);

    await page.waitForSelector('input[name="Search"]', selectorOptions);
    await page.type('input[name="Search"]', 'Coco');
    await (await page.$('input[name="Search"]')).press('Enter');

    //Edit movie
    console.log('Edit movie');
    await page.goto(process.env.BASEURL + '/Movie/Edit/185', pageOptions);
    await page.waitForSelector('.body-content form button[type="submit"]');
    await page.click('.body-content form button[type="submit"]');

    await navigationPromise

    //People
    console.log('People');
    await page.goto(process.env.BASEURL + '/Person', pageOptions);

    //Edit person
    console.log('Edit person');
    await page.goto(process.env.BASEURL + '/Person/Edit/1215657', pageOptions);
    await page.waitForSelector('#Biography', selectorOptions);
    await page.type('#Biography', 'Bloody nice chap');

    await page.waitForSelector('.body-content form button[type="submit"]');
    await page.click('.body-content form button[type="submit"]');

    await navigationPromise

    //Genre
    console.log('Genres');
    await page.goto(process.env.BASEURL + '/Genre', pageOptions);

    //Add genre
    console.log('Add genre');
    await page.goto(process.env.BASEURL + '/Genre/Edit', pageOptions);
    await page.waitForSelector('#Name', selectorOptions);
    await page.type('#Name', 'Snuff');

    await page.waitForSelector('.body-content form button[type="submit"]');
    await page.click('.body-content form button[type="submit"]');

    //Department
    console.log('Departments');
    await page.goto(process.env.BASEURL + '/Department', pageOptions);

    //Add Department
    console.log('Add Department');
    await page.goto(process.env.BASEURL + '/Department/Edit', pageOptions);
    await page.waitForSelector('#Name', selectorOptions);
    await page.type('#Name', 'Rider');

    await page.waitForSelector('.body-content form button[type="submit"]');
    await page.click('.body-content form button[type="submit"]');

    await navigationPromise

    //Profile
    console.log('Profile');
    await page.goto(process.env.BASEURL + '/Manage/Index', pageOptions);
    await page.waitForSelector('#PhoneNumber', selectorOptions);
    await page.type('#PhoneNumber', '0870277277');

    await page.waitForSelector('.body-content form button[type="submit"]');
    await page.click('.body-content form button[type="submit"]');

    await navigationPromise

    //Change Password
    console.log('Change Password');
    await page.goto(process.env.BASEURL + '/Manage/ChangePassword', pageOptions);
    await page.waitForSelector('#OldPassword', selectorOptions);
    await page.type('#OldPassword', 'p@ssWORD471');

    await page.waitForSelector('#NewPassword', selectorOptions);
    await page.type('#NewPassword', 'p@ssWORD471');

    await page.waitForSelector('#ConfirmPassword', selectorOptions);
    await page.type('#ConfirmPassword', 'p@ssWORD471');

    await page.waitForSelector('.body-content form button[type="submit"]');
    await page.click('.body-content form button[type="submit"]');

    await navigationPromise

    //Forgot Password
    console.log('Forgot Password');
    await page.goto(process.env.BASEURL + '/Account/ForgotPassword', pageOptions);
    await page.waitForSelector('#Email', selectorOptions);
    await page.type('#Email', 'admin@dotnetflicks.com');

    await page.waitForSelector('.body-content form button[type="submit"]');
    await page.click('.body-content form button[type="submit"]');

    await navigationPromise

    //Register User
    console.log('Register User');
    await page.goto(process.env.BASEURL + '/Account/Register', pageOptions);
    await page.waitForSelector('#Email', selectorOptions);
    await page.type('#Email', Math.random().toString(36).substring(7) + '@dotnetflicks.com');


    await page.waitForSelector('#Password', selectorOptions);
    await page.type('#Password', 'p@ssWORD471');

    await page.waitForSelector('#ConfirmPassword', selectorOptions);
    await page.type('#ConfirmPassword', 'p@ssWORD471');

    await page.waitForSelector('.body-content form button[type="submit"]');
    await page.click('.body-content form button[type="submit"]');

    await navigationPromise

    //Quit
    if (!process.env.DEBUG) {await browser.close()}

  }
})()
