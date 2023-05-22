import { mount as mountDevTools } from "@latticexyz/dev-tools";
import { setup } from "./mud/setup";

const { components, 
    network: {worldSend}, 
} = await setup();

// Components expose a stream that triggers when the component is updated.
// components.Counter.update$.subscribe((update) => {
//   const [nextValue, prevValue] = update.value;
//   console.log("Counter updated", update, { nextValue, prevValue });
//   document.getElementById("counter")!.innerHTML = String(nextValue?.value ?? "unset");
// });

// components.Chunks.update$.subscribe((update) => {
//   const [nextValue, prevValue] = update.value;
//   console.log("Chunks updated", update, { nextValue, prevValue });
// });

// Just for demonstration purposes: we create a global function that can be
// called to invoke the Increment system contract via the world. (See IncrementSystem.sol.)
(window as any).increment = async () => {
  // console.log(components.Counter.values);
  // const tx = await worldSend("increment", []);
  // console.log("increment tx", tx);
  // console.log("increment result", await tx.wait());
  let x = 5000;
  let y = 10000;

  let dis = 5;
  let startNotPos = 10000000;
  x = x + startNotPos;
  y = y + startNotPos;
  console.log("log", x, y, 1, dis);
  
  const tx1 = await worldSend("mulGenerate", [x, y, 1, dis]);
  console.log("increment tx", tx1);
  console.log("increment result", await tx1.wait());

  // const d = await worldSend("getChunk", [1]);
  console.log(components.Chunks.values);
  console.log(components.ChunksTimes.values);
};

(window as any).showchunks = async () => {
  console.log(components.Chunks.values);
  let xlist = components.Chunks.values.x;
//   console.log(xlist);
//   let klist = [];
//   for (const [key, value] of xlist) {
//     klist.push(key);
//     console.log("Key", key, "Value", value);
//   }
//   for (const element of klist) {
//     console.log(components.Chunks.values.y.get(element));
//   }
  // console.log(klist);
};

// mountDevTools();
