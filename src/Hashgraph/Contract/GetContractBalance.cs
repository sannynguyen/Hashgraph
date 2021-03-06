﻿using Hashgraph.Implementation;
using Proto;
using System;
using System.Threading.Tasks;

namespace Hashgraph
{
    public partial class Client
    {
        /// <summary>
        /// Retrieves the crypto and token blances from the network for a given contract.
        /// </summary>
        /// <param name="address">
        /// The hedera network address of the contract to retrieve the balance of.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// An object containing the crypto balance associated with the
        /// contract in addition to a list of all tokens held by the contract
        /// with their balances.
        /// </returns>
        public Task<AccountBalances> GetContractBalancesAsync(Address address, Action<IContext>? configure = null)
        {
            return GetContractBalancesImplementationAsync(address, configure);
        }
        /// <summary>
        /// Retrieves the balance in tinybars from the network for a given contract.
        /// </summary>
        /// <param name="contract">
        /// The hedera network contract address to retrieve the balance of.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// The balance of the associated contract.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        public async Task<ulong> GetContractBalanceAsync(Address contract, Action<IContext>? configure = null)
        {
            return (await GetContractBalancesImplementationAsync(contract, configure)).Crypto;
        }
        /// <summary>
        /// Internal Implementation of the Get Account Balances.
        /// </summary>
        private async Task<AccountBalances> GetContractBalancesImplementationAsync(Address contract, Action<IContext>? configure)
        {
            contract = RequireInputParameter.Contract(contract);
            await using var context = CreateChildContext(configure);
            var query = new Query
            {
                CryptogetAccountBalance = new CryptoGetAccountBalanceQuery
                {
                    ContractID = new ContractID(contract)
                }
            };
            var response = await query.SignAndExecuteWithRetryAsync(context);
            return response.CryptogetAccountBalance.ToAccountBalances();
        }
    }
}
