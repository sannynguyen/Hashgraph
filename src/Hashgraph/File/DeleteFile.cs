﻿using Hashgraph.Implementation;
using Proto;
using System;
using System.Threading.Tasks;

namespace Hashgraph
{
    public partial class Client
    {
        /// <summary>
        /// Removes a file from the network.
        /// </summary>
        /// <param name="fileToDelete">
        /// The address of the file to delete.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transaction receipt indicating success of the file deletion.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public Task<TransactionReceipt> DeleteFileAsync(Address fileToDelete, Action<IContext>? configure = null)
        {
            return DeleteFileImplementationAsync<TransactionReceipt>(fileToDelete, null, configure);
        }
        /// <summary>
        /// Removes a file from the network.
        /// </summary>
        /// <param name="fileToDelete">
        /// The address of the file to delete.
        /// </param>
        /// <param name="signatory">
        /// Typically private key, keys or signing callback method that 
        /// are needed to delete the file as per the requirements in the
        /// associated Endorsement.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transaction receipt indicating success of the file deletion.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public Task<TransactionReceipt> DeleteFileAsync(Address fileToDelete, Signatory signatory, Action<IContext>? configure = null)
        {
            return DeleteFileImplementationAsync<TransactionReceipt>(fileToDelete, signatory, configure);
        }
        /// <summary>
        /// Removes a file from the network.
        /// </summary>
        /// <param name="fileToDelete">
        /// The address of the file to delete.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transaction record indicating success of the file deletion,
        /// fees & other transaction details.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public Task<TransactionRecord> DeleteFileWithRecordAsync(Address fileToDelete, Action<IContext>? configure = null)
        {
            return DeleteFileImplementationAsync<TransactionRecord>(fileToDelete, null, configure);
        }
        /// <summary>
        /// Removes a file from the network.
        /// </summary>
        /// <param name="fileToDelete">
        /// The address of the file to delete.
        /// </param>
        /// <param name="signatory">
        /// Typically private key, keys or signing callback method that 
        /// are needed to delete the file as per the requirements in the
        /// associated Endorsement.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transaction record indicating success of the file deletion,
        /// fees & other transaction details.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public Task<TransactionRecord> DeleteFileWithRecordAsync(Address fileToDelete, Signatory signatory, Action<IContext>? configure = null)
        {
            return DeleteFileImplementationAsync<TransactionRecord>(fileToDelete, signatory, configure);
        }
        /// <summary>
        /// Internal helper function implementing the file delete functionality.
        /// </summary>
        public async Task<TResult> DeleteFileImplementationAsync<TResult>(Address fileToDelete, Signatory? signatory, Action<IContext>? configure = null) where TResult : new()
        {
            fileToDelete = RequireInputParameter.FileToDelete(fileToDelete);
            await using var context = CreateChildContext(configure);
            RequireInContext.Gateway(context);
            var payer = RequireInContext.Payer(context);
            var signatories = Transactions.GatherSignatories(context, signatory);
            var transactionId = Transactions.GetOrCreateTransactionID(context);
            var transactionBody = new TransactionBody(context, transactionId);
            transactionBody.FileDelete = new FileDeleteTransactionBody
            {
                FileID = new FileID(fileToDelete)
            };
            var receipt = await transactionBody.SignAndExecuteWithRetryAsync(signatories, context);
            if (receipt.Status != ResponseCodeEnum.Success)
            {
                throw new TransactionException($"Unable to delete file, status: {receipt.Status}", transactionId.ToTxId(), (ResponseCode)receipt.Status);
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
