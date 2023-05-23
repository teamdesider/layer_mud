const express = require('express');
const puppeteer = require('puppeteer');
var bodyParser = require('body-parser');
var fs = require('fs');

const app = express();
const port = 9000;


app.use(bodyParser());

app.get('/list', (req, res) => {
  // const respon = request('GET', 'http://127.0.0.1:3000');
  // if (respon.statusCode === 200) {
  //   const content = respon.getBody('utf8');
  //   console.log(content);
  // } else {
  //   console.error('请求失败，状态码：', respon.statusCode);
  // }

  let lat = Number(req.query.lat);
  let log = Number(req.query.log);
  console.log(lat, log, isNaN(lat), isNaN(log));

  (async () => {
    const browser = await puppeteer.launch({headless: true, args: ['--no-sandbox']});
    let inner_html = "";
    
    try {
      const page = await browser.newPage();
      // Configure the navigation timeout
      await page.setDefaultNavigationTimeout(0);

      let url = 'http://localhost:3099/list.html?x='+lat+'&y='+log;
      console.log(url);
      await page.goto(url, {
          waitUntil: 'load', // Remove the timeout
          timeout: 0
      });
      await page.waitForSelector("#datalist");
      inner_html = await page.$eval('#datalist', element => element.textContent);
      console.log('inner', inner_html);
      // const htmlContent = await page.content();
      // console.log(htmlContent);
    
    }catch(e) {
      inner_html = fs.readFileSync('./backup.txt');
    }
    console.log("done");
    await browser.close();

    res.send(jsondata(0, JSON.parse(inner_html)));
  })();
});

app.get('/checkcollect', (req, res) => {
    (async () => {
      const browser = await puppeteer.launch({headless: true, args: ['--no-sandbox']});
    
      const page = await browser.newPage();
      let k_id = req.query.id;
      console.log(k_id);
  
      let url = 'http://localhost:3099/checkcollect.html?k_id=' + k_id;
      console.log(url);
      await page.goto(url);
      await page.waitForSelector("#datalist");
  
      const inner_html = await page.$eval('#datalist', element => element.textContent);
      console.log('inner', inner_html);
      // const htmlContent = await page.content();
      // console.log(htmlContent);
    
      await browser.close();
      console.log("done");
      if(Number(inner_html) == 1) {
        res.send(jsondata(1, ""))
      } else {
        res.send(jsondata(0, ""))
      }
    })();
});

function jsondata(code, data) {
    let d = {
        code: code,
        data: data,
    }
    return JSON.stringify(d);
}

app.post('/collect', (req, res) => {
    let k_id = req.body.id;
    console.log("req body", k_id);

    (async () => {
      const browser = await puppeteer.launch({headless: true, args: ['--no-sandbox']});
    
      const page = await browser.newPage();
  
      let url = 'http://localhost:3099/collect.html?k_id=' + k_id;
      console.log(url);
      await page.goto(url);
      await page.waitForSelector("#datalist", {timeout:0});
  
      const inner_html = await page.$eval('#datalist', element => element.textContent);
      // console.log('inner', inner_html);
      // const htmlContent = await page.content();
      // console.log(htmlContent);
    
      await browser.close();
      console.log("done");
      if(inner_html == "finish") {
        res.send(jsondata(0, ""));
      } else {
        res.send(jsondata(-1, ""));
      }
    })();
});

// 启动服务器
app.listen(port, () => {
  console.log(`Server is listening on port ${port}`);
});