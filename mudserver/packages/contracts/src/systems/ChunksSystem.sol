// SPDX-License-Identifier: MIT
pragma solidity >=0.8.0;

import { System } from "@latticexyz/world/src/System.sol";
import { Chunks, ChunksData, ChunksTableId } from "../codegen/Tables.sol";
import { ChunksTimes, ChunksTimesTableId } from "../codegen/Tables.sol";

contract ChunksSystem is System {
  uint256 private nonce = 0;
  uint256 private timediff = 600;

  function mulGenerate(uint256 lan, uint256 log, uint256 uid, uint128 dis) public {
    uint256 startx = lan - dis;
    uint256 starty = log;
    uint256 length = dis*2;
    for (uint256 i = 0; i < 16; i = i + 2) {
      for (uint256 j = 0; j < 30; j = j + 2) {
        generate(startx+i, starty+j, uid);
        // uint256 k_id = generateUniqueId(startx+i, starty+j, uid);
        // doGenerate(startx+i, starty+j, uid, k_id, 1);
        // ChunksTimes.set(k_id, 1);
      }
    }
  }

  function generate(uint256 x, uint256 y, uint256 uid) internal {
    uint256 tm = block.timestamp;
    uint256 k_id = generateUniqueId(x, y, uid);
    uint256 times = 0;
    times = ChunksTimes.get(k_id);
    if (times == 0) {
        doGenerate(x, y, uid, k_id, 1);
        ChunksTimes.set(k_id, 1);
    } else {
        ChunksData memory cdata = Chunks.get(k_id, times);
        if(cdata.state == 1 && (cdata.tm - tm) > timediff) {
            doGenerate(x, y, uid, k_id, times + 1);
            ChunksTimes.set(k_id, times + 1);
        }
    }
    //location that hasn't spawned before or it's time to spawn again
  }

  function doGenerate(uint256 x, uint256 y, uint256 uid, uint256 k_id, uint256 times) internal {
      uint256 tm = block.timestamp;
      uint128 t = 0;
      uint256 randnum = getRandomNumber(x, y);

      if(randnum > 95) {
          t = 1;
      } else if (randnum > 90) {
          t = 2;
      } else if (randnum > 85) {
          t = 3;
      } else if (randnum > 80) {
          t = 4;
      } else if (randnum > 75) {
          t = 5;
      } else if (randnum > 70) {
          t = 6;
      } else if (randnum > 65) {
          t = 7;
      }

      Chunks.set(k_id, times, x, y, t, uid, tm, times, 0);
  }

  function collectChunks(uint256 x, uint256 y, uint256 uid) public {
    uint256 k_id = generateUniqueId(x, y, uid);
    uint256 times = ChunksTimes.get(k_id);
    Chunks.setState(k_id, times, 1);
  }

  function generateUniqueId(
        uint256 a,
        uint256 b,
        uint256 c
    ) internal pure returns (uint256) {
        uint256 uniqueId;

        uniqueId = (a << 192) | (b << 128) | c;

        return uniqueId;
    }

  function getRandomNumber(uint256 x, uint256 y) public returns (uint256) {
      uint256 randomNumber = uint256(keccak256(abi.encodePacked(block.timestamp, msg.sender, nonce, x, y))) % 101;
      nonce++;
      return randomNumber;
  }
}