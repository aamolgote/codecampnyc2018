using BAAS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAAS.Models;
using Nethereum.RPC.Eth.DTOs;

namespace BAAS.PlatformWrapper
{
    public class BlockManager : IBlockManager
    {
        private ISmartContractDb smartContractDb;

        public BlockManager(ISmartContractDb smartContractDb)
        {
            this.smartContractDb = smartContractDb;
        }

        private async Task<ulong> GetLatestBlockNumber()
        {
            var web3 = new Nethereum.Geth.Web3Geth();
            var maxBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            ulong blockNumber = 0;
            ulong.TryParse(maxBlockNumber.Value.ToString(), out blockNumber);
            return blockNumber;
        }

        public async Task<DltBlock> GetLatestBlock()
        {
            var web3 = new Nethereum.Geth.Web3Geth();
            ulong blockNumber = await this.GetLatestBlockNumber();
            var blockParameter = new BlockParameter(blockNumber);
            var blockEth = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(blockParameter);
            var trans = blockEth.Transactions;
            int txCount = trans.Length;
            DltBlock block = null;
            DateTime blockDatetime = Helper.UnixTimeStampToDateTime((double)blockEth.Timestamp.Value);
            block = new DltBlock
            {
                BlockNumber = blockNumber,
                TransactionCount = txCount,
                BlockDatetime = blockDatetime,
                BlockHash = blockEth.BlockHash,
                Difficulty = (ulong)blockEth.Difficulty.Value,
                GasLimit = (ulong)blockEth.GasLimit.Value,
                GasUsed = (ulong)blockEth.GasUsed.Value,
                Size = (ulong)blockEth.Size.Value
            };
            if (block.TransactionCount > 0)
            {
                block.DltTransactions = new List<DltBlockTransaction>();
                foreach (var transaction in blockEth.Transactions)
                {
                    DltBlockTransaction dltTransaction = new DltBlockTransaction()
                    {
                        From = transaction.From,
                        To = transaction.To,
                        Gas = (ulong)transaction.Gas.Value,
                        GasPrice = (ulong)transaction.GasPrice.Value,
                        Nonce = (ulong)transaction.Nonce.Value,
                        Value = (ulong)transaction.Value.Value,
                        TransactionHash = transaction.TransactionHash

                    };
                    block.DltTransactions.Add(dltTransaction);
                }
            }
            return block;
        }

        public async Task<List<DltBlock>> GetRecentBlocks(int numberOfBlocks)
        {
            ulong blockCount = (ulong)numberOfBlocks;
            var web3 = new Nethereum.Geth.Web3Geth();
            ulong startBlockNumber = 0;
            ulong endBlockNumber = startBlockNumber + blockCount;
            var maxBlockNumber = await this.GetLatestBlockNumber();
            startBlockNumber = maxBlockNumber < blockCount ? 1 : maxBlockNumber - blockCount;
            endBlockNumber = maxBlockNumber;

            List<DltBlock> dltBlocks = new List<DltBlock>();
            Dictionary<string, SmartContractTransaction> databaseDataDict = new Dictionary<string, SmartContractTransaction>();
            List<string> transactionHashesList = new List<string>();
            for (ulong blockNumber = startBlockNumber; blockNumber <= endBlockNumber; blockNumber++)
            {
                var blockParameter = new BlockParameter(blockNumber);
                var blockEth = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(blockParameter);
                var trans = blockEth.Transactions;
                int txCount = trans.Length;
                DltBlock block = null;
                DateTime blockDatetime = Helper.UnixTimeStampToDateTime((double)blockEth.Timestamp.Value);
                block = new DltBlock
                {
                    BlockNumber = blockNumber,
                    TransactionCount = txCount,
                    BlockDatetime = blockDatetime,
                    BlockHash = blockEth.BlockHash,
                    Difficulty = (ulong)blockEth.Difficulty.Value,
                    GasLimit = (ulong)blockEth.GasLimit.Value,
                    GasUsed = (ulong)blockEth.GasUsed.Value,
                    Size = (ulong)blockEth.Size.Value
                };
                if (block.TransactionCount > 0)
                {
                    block.DltTransactions = new List<DltBlockTransaction>();
                    foreach (var transaction in blockEth.Transactions)
                    {
                        DltBlockTransaction dltTransaction = new DltBlockTransaction()
                        {
                            From = transaction.From,
                            To = transaction.To,
                            Gas = (ulong)transaction.Gas.Value,
                            GasPrice = (ulong)transaction.GasPrice.Value,
                            Nonce = (ulong)transaction.Nonce.Value,
                            Value = (ulong)transaction.Value.Value,
                            TransactionHash = transaction.TransactionHash

                        };
                        block.DltTransactions.Add(dltTransaction);
                        transactionHashesList.Add(transaction.TransactionHash);
                    }
                }
                dltBlocks.Add(block);
            }
            dltBlocks = await AddDatabaseDataToDltBlock(dltBlocks, transactionHashesList);
            return dltBlocks.OrderByDescending( block => block.BlockNumber).ToList();
        }

        private async Task<List<DltBlock>> AddDatabaseDataToDltBlock(List<DltBlock> dltBlocksParams, List<string> transactionHashesList)
        {
            List<DltBlock> dltBlocks = dltBlocksParams;
            Dictionary<string, SmartContractTransaction> databaseDataDict;
            if (transactionHashesList.Count > 0)
            {
                databaseDataDict = await this.smartContractDb.GetSmartContractTransactionsInfoWithList(transactionHashesList);

                foreach(var dltBlock in dltBlocks)
                {
                    if (dltBlock.DltTransactions != null)
                    {
                        foreach(var dltTransaction in dltBlock.DltTransactions)
                        {
                            string trHash = dltTransaction.TransactionHash;
                            SmartContractTransaction smartContractTranaction;
                            if (databaseDataDict.ContainsKey(trHash))
                            {
                                smartContractTranaction = databaseDataDict[trHash];
                                dltTransaction.SmartContractTransactionId = smartContractTranaction.SmartContractTransactionId;
                                dltTransaction.SmartContractDeployedInstanceId = smartContractTranaction.SmartContractDeployedInstanceId;
                                dltTransaction.TransactionUser = smartContractTranaction.TransactionUser;
                                dltTransaction.SmartContractFunction = smartContractTranaction.SmartContractFunction;
                                dltTransaction.SmartContractFunctionParamtersList = smartContractTranaction.SmartContractFunctionParametersList;
                                dltTransaction.SmartContractFunctionParamterNames = smartContractTranaction.SmartContractFunctionParamterNames;
                                dltTransaction.CreatedDatetime = smartContractTranaction.CreatedDatetime;
                                dltTransaction.UpdatedDatetime = smartContractTranaction.UpdatedDatetime;
                                dltTransaction.Name = smartContractTranaction.Name;
                            }
                        }
                    }
                }

            }
            return dltBlocks;
        }
    }

    public static class Helper
    {
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
