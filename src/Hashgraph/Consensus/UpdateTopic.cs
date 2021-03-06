﻿using Hashgraph.Implementation;
using Proto;
using System;
using System.Threading.Tasks;

namespace Hashgraph
{
    public partial class Client
    {
        /// <summary>
        /// Updates the changeable properties of a Hedera Network Topic.
        /// </summary>
        /// <param name="updateParameters">
        /// The Topic update parameters, includes a required 
        /// <see cref="Address"/> reference to the Topic to update plus
        /// a number of changeable properties of the Topic.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transaction receipt indicating success of the operation.
        /// of the request.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public Task<TransactionReceipt> UpdateTopicAsync(UpdateTopicParams updateParameters, Action<IContext>? configure = null)
        {
            return UpdateTopicImplementationAsync<TransactionReceipt>(updateParameters, configure);
        }
        /// <summary>
        /// Updates the changeable properties of a hedera network Topic.
        /// </summary>
        /// <param name="updateParameters">
        /// The Topic update parameters, includes a required 
        /// <see cref="Address"/> reference to the Topic to update plus
        /// a number of changeable properties of the Topic.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transaction record containing the details of the results.
        /// of the request.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public Task<TransactionRecord> UpdateTopicWithRecordAsync(UpdateTopicParams updateParameters, Action<IContext>? configure = null)
        {
            return UpdateTopicImplementationAsync<TransactionRecord>(updateParameters, configure);
        }
        /// <summary>
        /// Internal implementation of the update Topic functionality.
        /// </summary>
        private async Task<TResult> UpdateTopicImplementationAsync<TResult>(UpdateTopicParams updateParameters, Action<IContext>? configure) where TResult : new()
        {
            updateParameters = RequireInputParameter.UpdateParameters(updateParameters);
            await using var context = CreateChildContext(configure);
            RequireInContext.Gateway(context);
            var payer = RequireInContext.Payer(context);
            var signatory = Transactions.GatherSignatories(context, updateParameters.Signatory);
            var updateTopicBody = new ConsensusUpdateTopicTransactionBody
            {
                TopicID = new TopicID(updateParameters.Topic)
            };
            if (updateParameters.Memo != null)
            {
                updateTopicBody.Memo = updateParameters.Memo;
            }
            if (!(updateParameters.Administrator is null))
            {
                updateTopicBody.AdminKey = new Key(updateParameters.Administrator);
            }
            if (!(updateParameters.Participant is null))
            {
                updateTopicBody.SubmitKey = new Key(updateParameters.Participant);
            }
            if (updateParameters.RenewPeriod.HasValue)
            {
                updateTopicBody.AutoRenewPeriod = new Duration(updateParameters.RenewPeriod.Value);
            }
            if (!(updateParameters.RenewAccount is null))
            {
                updateTopicBody.AutoRenewAccount = new AccountID(updateParameters.RenewAccount);
            }
            var transactionId = Transactions.GetOrCreateTransactionID(context);
            var transactionBody = new TransactionBody(context, transactionId);
            transactionBody.ConsensusUpdateTopic = updateTopicBody;
            var receipt = await transactionBody.SignAndExecuteWithRetryAsync(signatory, context);
            if (receipt.Status != ResponseCodeEnum.Success)
            {
                throw new TransactionException($"Unable to update Topic, status: {receipt.Status}", transactionId.ToTxId(), (ResponseCode)receipt.Status);
            }
            var result = new TResult();
            if (result is TransactionRecord rec)
            {
                var record = await GetTransactionRecordAsync(context, transactionId);
                record.FillProperties(rec);
            }
            else if (result is TransactionReceipt rcpt)
            {
                receipt.FillProperties(transactionId, rcpt);
            }
            return result;
        }
    }
}
