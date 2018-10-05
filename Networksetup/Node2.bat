set curr_dir=%cd%
chdir /D C:\TFS\BAASBlockchain
geth --datadir=Node2 --keystore "C:\TFS\BAASBlockchain\Keystore" --networkid 444444444500 --port 30302 --nodiscover --ipcdisable --syncmode "fast" --cache 1024 console