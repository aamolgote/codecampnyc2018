set curr_dir=%cd%
chdir /D C:\TFS\BAASBlockchain
geth --datadir=Node1 --keystore "C:\TFS\BAASBlockchain\Keystore" --networkid 444444444500 --port 30301 --nodiscover --rpc --rpccorsdomain "*" --rpcapi "eth,web3,personal,net,miner,admin,debug" --syncmode "fast" --cache 1024 console