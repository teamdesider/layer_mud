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

let k_id = getQueryString("k_id");
k_id = k_id.replace("Symbol(", "").replace(")", "");
let dis = 5;
console.log("k_id11", k_id);

let x = 0;
let y = 0;
console.log("log", x, y, 1, dis);
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

let k = Symbol(k_id);
for (const [key, value] of xlist) {
    if(key.toString() == k.toString()) {
        x = Number(value);
    }
}
for (const [key, value] of components.Chunks.values.y) {
    if(key.toString() == k.toString()) {
        y = Number(value);
    }
}

const tx1 = await worldSend("collectChunks", [x, y, 1, "0xa85233C63b9Ee964Add6F2cffe00Fd84eb32338f"]);
console.log("generate tx", tx1);
console.log("generate result", await tx1.wait());

//   console.log("klist", JSON.stringify(klist));
document.write('<div id="datalist">finish</div>');

// mountDevTools();
