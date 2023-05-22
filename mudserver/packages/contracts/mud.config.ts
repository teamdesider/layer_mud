import { mudConfig } from "@latticexyz/world/register";

export default mudConfig({
  tables: {
    Counter: {
      keySchema: {},
      schema: "uint32",
    },
    Chunks: {
      keySchema: {
        k_id : "uint256",
        k_times : "uint256",
      },
      schema: {
        x: "uint256",
        y: "uint256",
        t: "uint256",
        uid: "uint256",
        tm: "uint256",
        times: "uint256",
        state: "uint256",
      },
    },
    ChunksTimes: {
      keySchema: {
        k_id : "uint256",
      },
      schema: {
        times: "uint256",
      },
    }
  },
});
