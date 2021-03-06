﻿using Google.Protobuf;
using Hashgraph.Implementation;
using Proto;
using System;
using System.Threading.Tasks;

namespace Hashgraph
{
    public partial class Client
    {
        /// <summary>
        /// Creates a new contract instance with the given create parameters.
        /// </summary>
        /// <param name="createParameters">
        /// Details regarding the contract to instantiate.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transaction receipt with a description of the newly created contract.
        /// and receipt information.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public Task<CreateContractReceipt> CreateContractAsync(CreateContractParams createParameters, Action<IContext>? configure = null)
        {
            return CreateContractImplementationAsync<CreateContractReceipt>(createParameters, configure);
        }
        /// <summary>
        /// Creates a new contract instance with the given create parameters 
        /// returning a detailed record.
        /// </summary>
        /// <param name="createParameters">
        /// Details regarding the contract to instantiate.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transaction record with a description of the newly created contract.
        /// and receipt information.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public Task<CreateContractRecord> CreateContractWithRecordAsync(CreateContractParams createParameters, Action<IContext>? configure = null)
        {
            return CreateContractImplementationAsync<CreateContractRecord>(createParameters, configure);
        }
        /// <summary>
        /// Internal Create Contract Implementation
        /// </summary>
        public async Task<TResult> CreateContractImplementationAsync<TResult>(CreateContractParams createParameters, Action<IContext>? configure) where TResult : new()
        {
            createParameters = RequireInputParameter.CreateParameters(createParameters);
            await using var context = CreateChildContext(configure);
            var gateway = RequireInContext.Gateway(context);
            var payer = RequireInContext.Payer(context);
            var signatory = Transactions.GatherSignatories(context, createParameters.Signatory);
            var transactionId = Transactions.GetOrCreateTransactionID(context);
            var transactionBody = new TransactionBody(context, transactionId);
            transactionBody.ContractCreateInstance = new ContractCreateTransactionBody
            {
                FileID = new FileID(createParameters.File),
                AdminKey = createParameters.Administrator is null ? null : new Key(createParameters.Administrator),
                Gas = createParameters.Gas,
                InitialBalance = createParameters.InitialBalance,
                AutoRenewPeriod = new Duration(createParameters.RenewPeriod),
                ConstructorParameters = ByteString.CopyFrom(Abi.EncodeArguments(createParameters.Arguments).ToArray()),
                Memo = context.Memo ?? ""
            };
            var receipt = await transactionBody.SignAndExecuteWithRetryAsync(signatory, context);
            if (receipt.Status != ResponseCodeEnum.Success)
            {
                throw new TransactionException($"Unable to create contract, status: {receipt.Status}", transactionId.ToTxId(), (ResponseCode)receipt.Status);
            }
            var result = new TResult();
            if (result is CreateContractRecord rec)
            {
                var record = await GetTransactionRecordAsync(context, transactionId);
                record.FillProperties(rec);
            }
            else if (result is CreateContractReceipt rcpt)
            {
                receipt.FillProperties(transactionId, rcpt);
            }
            return result;
        }
    }
}
