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

let k_id = getQueryString("k_id")
k_id = k_id.replace("Symbol(", "").replace(")", "");
let dis = 5;
console.log("k_id", k_id);

//   console.log(components.Chunks.values);
let slist = components.Chunks.values.state;
const sleep = (delay) => new Promise((resolve) => setTimeout(resolve, delay));
for(let i = 0; i < 100; i ++) {
    slist = components.Chunks.values.state;
  console.log(i, slist.size, components.Chunks.values);
  if (slist.size >= 1) {
    break;
  }
  await sleep(1000);
}
console.log(slist.size);

let state = 0;
let k = Symbol(k_id);
for (const [key, value] of slist) {
    if(key.toString() == k.toString()) {
        state = Number(value);
    }
    // console.log("Key", key, "Value", value);
  }
  console.log("state", state);

document.write('<div id="datalist">' + state + '</div>');

// mountDevTools();
