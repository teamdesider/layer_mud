import { mount as mountDevTools } from "@latticexyz/dev-tools";
import { setup } from "./mud/setup";

const { components, 
    network: {worldSend}, 
} = await setup();

function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null)return decodeURI(r[2]);
    return null;
}

let x = parseInt(getQueryString("x"));
let y = parseInt(getQueryString("y"));
console.log("x, y", x, y);

let dis = 5;
let startNotPos = 10000000;
x = x + startNotPos;
y = y + startNotPos;
console.log("log", x, y, 1, dis);

const tx1 = await worldSend("mulGenerate", [x, y, 1, dis]);
console.log("generate tx", tx1);
console.log("generate result", await tx1.wait());

//   console.log(components.Chunks.values);
let xlist = components.Chunks.values.x;
const sleep = (delay) => new Promise((resolve) => setTimeout(resolve, delay));
for(let i = 0; i < 100; i ++) {
  xlist = components.Chunks.values.x;
  console.log(i, xlist.size, components.Chunks.values);
  if (xlist.size >= 1) {
    break;
  }
  await sleep(1000);
}
console.log(xlist.size);
let klist = [];
for (const [key, value] of xlist) {
    // console.log("abd", Math.abs(Number(value) - x));
  if(Math.abs(Number(value) - x) <= 8) {
      klist.push(key);
  }
  // console.log(key, key.toString(), Number(value) - 1000);
  // klist.push(key);
  // console.log("Key", key, "Value", value);
}
let endlist = [];
for (const element of klist) {
    // console.log("aby", Number(components.Chunks.values.y.get(element)), y, Math.abs(Number(components.Chunks.values.y.get(element)) - y));
  if(Math.abs(Number(components.Chunks.values.y.get(element)) - y) <= 30) {
      endlist.push(element);
  }
}
let result = [];
for (const element of endlist) {
  let ty = Number(components.Chunks.values.t.get(element));
  if (ty > 0) {
      result.push({
          "id" : element.toString(),
          "type": ty,
          "pos": [
              Number(xlist.get(element)) - x,
              Number(components.Chunks.values.y.get(element)) - y,
          ],
      });
  }
}
console.log("endlist", result);
//   console.log("klist", JSON.stringify(klist));
document.write('<div id="datalist">' + JSON.stringify(result) + '</div>');

// mountDevTools();
