﻿using Grpc.Core;
using Hashgraph.Implementation;
using Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hashgraph
{
    public partial class Client
    {
        #region DEPRICATED_REMOVE_WHEN_ACCOUNTS_REMOVED
        /// <summary>
        /// Transfer tinybars from one account to another.
        /// </summary>
        /// <param name="fromAccount">
        /// The account to transfer the tinybars from.  It will sign the 
        /// transaction, but may not necessarily be the account 
        /// <see cref="IContext.Payer">paying</see> the transaction fee.
        /// </param>
        /// <param name="toAddress">
        /// The address receiving the tinybars.
        /// </param>
        /// <param name="amount">
        /// The amount of tinybars to transfer.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transfer receipt indicating success of the operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        [Obsolete("Please use Address objects for identifying accounts and Signatory objects to hold private keys. The Account object will be removed in a future release.")]
        public Task<TransactionReceipt> TransferAsync(Account fromAccount, Address toAddress, long amount, Action<IContext>? configure = null)
        {
            return TransferImplementationDeprecatedAsync<TransactionReceipt>(fromAccount, toAddress, amount, configure);
        }
        /// <summary>
        /// Transfer tinybars from one account to another.
        /// </summary>
        /// <param name="fromAccount">
        /// The account to transfer the tinybars from.  It will sign the 
        /// transaction, but may not necessarily be the account 
        /// <see cref="IContext.Payer">paying</see> the transaction fee.
        /// </param>
        /// <param name="toAddress">
        /// The address receiving the tinybars.
        /// </param>
        /// <param name="amount">
        /// The amount of tinybars to transfer.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transfer record describing the details of the concensus transaction.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        [Obsolete("Please use Address objects for identifying accounts and Signatory objects to hold private keys. The Account object will be removed in a future release.")]
        public Task<TransactionRecord> TransferWithRecordAsync(Account fromAccount, Address toAddress, long amount, Action<IContext>? configure = null)
        {
            return TransferImplementationDeprecatedAsync<TransactionRecord>(fromAccount, toAddress, amount, configure);
        }
        /// <summary>
        /// Transfer tinybars from one account to another.
        /// </summary>
        /// <param name="sendAccounts">
        /// A dictionary mapping how much crypto to send
        /// from each account in the dictionary.  Amount values
        /// must be positive.  The send accounts must contain
        /// the necessary signing keys to authroize the transaction
        /// (unless the <see cref="IContext.Payer"/> holds the
        /// necessary keys instead).
        /// </param>
        /// <param name="receiveAddresses">
        /// A dictionary mapping how much crypto will be received
        /// by each address in the dictionary.  Amount values
        /// must be positive.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transfer receipt indicating success of the operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        [Obsolete("Please use Address objects for identifying accounts and Signatory objects to hold private keys. The Account object will be removed in a future release.")]
        public Task<TransactionReceipt> TransferAsync(Dictionary<Account, long> sendAccounts, Dictionary<Address, long> receiveAddresses, Action<IContext>? configure = null)
        {
            return TransferImplementationDeprecatedAsync<TransactionReceipt>(sendAccounts, receiveAddresses, configure);
        }
        /// <summary>
        /// Transfer tinybars from one account to another.
        /// </summary>
        /// <param name="sendAccounts">
        /// A dictionary mapping how much crypto to send
        /// from each account in the dictionary.  Amount values
        /// must be positive.  The send accounts must contain
        /// the necessary signing keys to authroize the transaction
        /// (unless the <see cref="IContext.Payer"/> holds the
        /// necessary keys instead).
        /// </param>
        /// <param name="receiveAddresses">
        /// A dictionary mapping how much crypto will be received
        /// by each address in the dictionary.  Amount values
        /// must be positive.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transfer record describing the details of the concensus transaction.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        [Obsolete("Please use Address objects for identifying accounts and Signatory objects to hold private keys. The Account object will be removed in a future release.")]
        public Task<TransactionRecord> TransferWithRecordAsync(Dictionary<Account, long> sendAccounts, Dictionary<Address, long> receiveAddresses, Action<IContext>? configure = null)
        {
            return TransferImplementationDeprecatedAsync<TransactionRecord>(sendAccounts, receiveAddresses, configure);
        }
        /// <summary>
        /// Internal implementation for Transfer Crypto.
        /// Returns either a receipt or record or throws
        /// an exception.
        /// </summary>
        private async Task<TResult> TransferImplementationDeprecatedAsync<TResult>(Account fromAccount, Address toAddress, long amount, Action<IContext>? configure) where TResult : new()
        {
            fromAccount = RequireInputParameter.FromAccount(fromAccount);
            toAddress = RequireInputParameter.ToAddress(toAddress);
            amount = RequireInputParameter.Amount(amount);
            var signatory = new Signatory(fromAccount);
            var transferList = new[] { (fromAccount, -amount), (toAddress, amount) };
            return await TransferImplementationAsync<TResult>(transferList, signatory, configure);
        }
        /// <summary>
        /// Internal implementation for Transfer Crypto.
        /// Returns either a receipt or record or throws
        /// an exception.
        /// </summary>
        private Task<TResult> TransferImplementationDeprecatedAsync<TResult>(Dictionary<Account, long> sendAccounts, Dictionary<Address, long> receiveAddresses, Action<IContext>? configure) where TResult : new()
        {
            var transferList = RequireInputParameter.MultiTransfers(sendAccounts, receiveAddresses);
            var signatory = new Signatory(sendAccounts.Keys.ToArray<ISignatory>().Select(s => new Signatory(s)).ToArray());
            return TransferImplementationAsync<TResult>(transferList, signatory, configure);
        }
        #endregion DEPRICATED_REMOVE_WHEN_ACCOUNTS_REMOVED
        /// <summary>
        /// Transfer tinybars from one account to another.
        /// </summary>
        /// <param name="fromAddress">
        /// The address to transfer the tinybars from.  Ensure that
        /// a signatory either in the context or passed with this
        /// call can fulfill the signing requrements to transfer 
        /// crypto out of the account identified by this address.
        /// </param>
        /// <param name="toAddress">
        /// The address receiving the tinybars.
        /// </param>
        /// <param name="amount">
        /// The amount of tinybars to transfer.
        /// </param>
        /// <param name="signatory">
        /// The signatory containing any additional private keys or callbacks
        /// to meet the requirements for the sending and receiving accounts.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transfer receipt indicating success of the operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public Task<TransactionReceipt> TransferAsync(Address fromAddress, Address toAddress, long amount, Action<IContext>? configure = null)
        {
            return TransferImplementationAsync<TransactionReceipt>(fromAddress, toAddress, amount, null, configure);
        }
        /// <summary>
        /// Transfer tinybars from one account to another.
        /// </summary>
        /// <param name="fromAddress">
        /// The address to transfer the tinybars from.  Ensure that
        /// a signatory either in the context or passed with this
        /// call can fulfill the signing requrements to transfer 
        /// crypto out of the account identified by this address.
        /// </param>
        /// <param name="toAddress">
        /// The address receiving the tinybars.
        /// </param>
        /// <param name="amount">
        /// The amount of tinybars to transfer.
        /// </param>
        /// <param name="signatory">
        /// The signatory containing any additional private keys or callbacks
        /// to meet the requirements for the sending and receiving accounts.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transfer receipt indicating success of the operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public Task<TransactionReceipt> TransferAsync(Address fromAddress, Address toAddress, long amount, Signatory signatory, Action<IContext>? configure = null)
        {
            return TransferImplementationAsync<TransactionReceipt>(fromAddress, toAddress, amount, signatory, configure);
        }
        /// <summary>
        /// Transfer tinybars from one account to another.
        /// </summary>
        /// <param name="fromAddress">
        /// The address to transfer the tinybars from.  Ensure that
        /// a signatory either in the context can fulfill the signing 
        /// requrements to transfer crypto out of the account identified 
        /// by this address.
        /// </param>
        /// <param name="toAddress">
        /// The address receiving the tinybars.
        /// </param>
        /// <param name="amount">
        /// The amount of tinybars to transfer.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transfer record describing the details of the concensus transaction.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public Task<TransactionRecord> TransferWithRecordAsync(Address fromAddress, Address toAddress, long amount, Action<IContext>? configure = null)
        {
            return TransferImplementationAsync<TransactionRecord>(fromAddress, toAddress, amount, null, configure);
        }
        /// <summary>
        /// Transfer tinybars from one account to another.
        /// </summary>
        /// <param name="fromAddress">
        /// The address to transfer the tinybars from.  Ensure that
        /// a signatory either in the context or passed with this
        /// call can fulfill the signing requrements to transfer 
        /// crypto out of the account identified by this address.
        /// </param>
        /// <param name="toAddress">
        /// The address receiving the tinybars.
        /// </param>
        /// <param name="amount">
        /// The amount of tinybars to transfer.
        /// </param>
        /// <param name="signatory">
        /// The signatory containing any additional private keys or callbacks
        /// to meet the requirements for the sending and receiving accounts.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transfer record describing the details of the concensus transaction.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public Task<TransactionRecord> TransferWithRecordAsync(Address fromAddress, Address toAddress, long amount, Signatory signatory, Action<IContext>? configure = null)
        {
            return TransferImplementationAsync<TransactionRecord>(fromAddress, toAddress, amount, signatory, configure);
        }
        /// <summary>
        /// Transfer tinybars from an arbitray set of accounts to
        /// another arbitrary set of accounts.
        /// </summary>
        /// <param name="transfers">
        /// A dictionary mapping how much crypto to transfer
        /// from and to each address.  Negative values send 
        /// crypto out of the account, positive values receive
        /// crypto into the account.  The value of all the 
        /// transfer values in the dictionary must sum to zero.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transfer receipt indicating success of the operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public Task<TransactionReceipt> TransferAsync(Dictionary<Address, long> transfers, Action<IContext>? configure = null)
        {
            return TransferImplementationAsync<TransactionReceipt>(transfers, null, configure);
        }
        /// <summary>
        /// Transfer tinybars from an arbitray set of accounts to
        /// another arbitrary set of accounts.
        /// </summary>
        /// <param name="transfers">
        /// A dictionary mapping how much crypto to transfer
        /// from and to each address.  Negative values send 
        /// crypto out of the account, positive values receive
        /// crypto into the account.  The value of all the 
        /// transfer values in the dictionary must sum to zero.
        /// </param>
        /// <param name="signatory">
        /// The signatory containing any additional private keys or callbacks
        /// to meet the requirements for the sending and receiving accounts.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transfer receipt indicating success of the operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public Task<TransactionReceipt> TransferAsync(Dictionary<Address, long> transfers, Signatory signatory, Action<IContext>? configure = null)
        {
            return TransferImplementationAsync<TransactionReceipt>(transfers, signatory, configure);
        }
        /// <summary>
        /// Transfer tinybars from an arbitray set of accounts to
        /// another arbitrary set of accounts.
        /// </summary>
        /// <param name="transfers">
        /// A dictionary mapping how much crypto to transfer
        /// from and to each address.  Negative values send 
        /// crypto out of the account, positive values receive
        /// crypto into the account.  The value of all the 
        /// transfer values in the dictionary must sum to zero.
        /// </param>
        /// <param name="signatory">
        /// The signatory containing any additional private keys or callbacks
        /// to meet the requirements for the sending and receiving accounts.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transfer record describing the details of the concensus transaction.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public Task<TransactionRecord> TransferWithRecordAsync(Dictionary<Address, long> transfers, Action<IContext>? configure = null)
        {
            return TransferImplementationAsync<TransactionRecord>(transfers, null, configure);
        }
        /// <summary>
        /// Transfer tinybars from an arbitray set of accounts to
        /// another arbitrary set of accounts.
        /// </summary>
        /// <param name="transfers">
        /// A dictionary mapping how much crypto to transfer
        /// from and to each address.  Negative values send 
        /// crypto out of the account, positive values receive
        /// crypto into the account.  The value of all the 
        /// transfer values in the dictionary must sum to zero.
        /// </param>
        /// <param name="signatory">
        /// The signatory containing any additional private keys or callbacks
        /// to meet the requirements for the sending and receiving accounts.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transfer record describing the details of the concensus transaction.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public Task<TransactionRecord> TransferWithRecordAsync(Dictionary<Address, long> transfers, Signatory signatory, Action<IContext>? configure = null)
        {
            return TransferImplementationAsync<TransactionRecord>(transfers, signatory, configure);
        }
        /// <summary>
        /// Internal implementation for Transfer Crypto.
        /// Returns either a receipt or record or throws
        /// an exception.
        /// </summary>
        private async Task<TResult> TransferImplementationAsync<TResult>(Address fromAddress, Address toAddress, long amount, Signatory? signatory, Action<IContext>? configure) where TResult : new()
        {
            fromAddress = RequireInputParameter.FromAddress(fromAddress);
            toAddress = RequireInputParameter.ToAddress(toAddress);
            amount = RequireInputParameter.Amount(amount);
            var transferList = new[] { (fromAddress, -amount), (toAddress, amount) };
            return await TransferImplementationAsync<TResult>(transferList, signatory, configure);
        }
        /// <summary>
        /// Internal implementation for Transfer Crypto.
        /// Returns either a receipt or record or throws
        /// an exception.
        /// </summary>
        private async Task<TResult> TransferImplementationAsync<TResult>(Dictionary<Address, long> transfers, Signatory? signatory, Action<IContext>? configure = null) where TResult : new()
        {
            var transferList = RequireInputParameter.TransferList(transfers);
            return await TransferImplementationAsync<TResult>(transferList, signatory, configure);
        }
        /// <summary>
        /// Internal implementation for Multi Account Transfer Crypto.
        /// Returns either a receipt or record or throws an exception.
        /// </summary>
        private async Task<TResult> TransferImplementationAsync<TResult>((Address address, long amount)[] transferList, Signatory? signatory, Action<IContext>? configure) where TResult : new()
        {
            var context = CreateChildContext(configure);
            RequireInContext.Gateway(context);
            var signatories = signatory is null ?
                Transactions.GatherSignatories(context) :
                Transactions.GatherSignatories(context, signatory);
            var transfers = Transactions.CreateCryptoTransferList(transferList);
            var transactionId = Transactions.GetOrCreateTransactionID(context);
            var transactionBody = Transactions.CreateTransactionBody(context, transactionId, "Transfer Crypto");
            transactionBody.CryptoTransfer = new CryptoTransferTransactionBody { Transfers = transfers };
            var request = await Transactions.SignTransactionAsync(transactionBody, signatories);
            var precheck = await Transactions.ExecuteSignedRequestWithRetryAsync(context, request, getRequestMethod, getResponseCode);
            ValidateResult.PreCheck(transactionId, precheck.NodeTransactionPrecheckCode);
            var receipt = await GetReceiptAsync(context, transactionId);
            if (receipt.Status != ResponseCodeEnum.Success)
            {
                throw new TransactionException($"Unable to execute crypto transfer, status: {receipt.Status}", Protobuf.FromTransactionId(transactionId), (ResponseCode)receipt.Status);
            }
            var result = new TResult();
            if (result is TransactionRecord rec)
            {
                var record = await GetTransactionRecordAsync(context, transactionId);
                Protobuf.FillRecordProperties(record, rec);
            }
            else if (result is TransactionReceipt rcpt)
            {
                Protobuf.FillReceiptProperties(transactionId, receipt, rcpt);
            }
            return result;

            static Func<Transaction, Task<TransactionResponse>> getRequestMethod(Channel channel)
            {
                var client = new CryptoService.CryptoServiceClient(channel);
                return async (Transaction request) => await client.cryptoTransferAsync(request);
            }

            static ResponseCodeEnum getResponseCode(TransactionResponse response)
            {
                return response.NodeTransactionPrecheckCode;
            }
        }
    }
}
