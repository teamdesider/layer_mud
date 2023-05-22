cd ~/Project/layer/

#config
cp ~/Project/layer_mud/mudserver/packages/contracts/mud.config.ts packages/contracts/
cp ~/Project/layer_mud/mudserver/packages/client/vite.config.ts packages/client/

#client
cp ~/Project/layer_mud_temp/packages/client/index.html packages/client/
cp ~/Project/layer_mud_temp/packages/client/list.html packages/client/
cp ~/Project/layer_mud_temp/packages/client/collect.html packages/client/
cp ~/Project/layer_mud_temp/packages/client/checkcollect.html packages/client/
cp ~/Project/layer_mud_temp/packages/client/src/index.ts packages/client/src/
cp ~/Project/layer_mud_temp/packages/client/src/list.ts packages/client/src/
cp ~/Project/layer_mud_temp/packages/client/src/collect.ts packages/client/src/
cp ~/Project/layer_mud_temp/packages/client/src/checkcollect.ts packages/client/src/

#contract
cp ~/Project/layer_mud/mudserver/packages/contracts/src/systems/ChunksSystem.sol packages/contracts/src/systems/
cp ~/Project/layer_mud_temp/packages/contracts/src/systems/Layer1155.sol packages/contracts/src/systems/