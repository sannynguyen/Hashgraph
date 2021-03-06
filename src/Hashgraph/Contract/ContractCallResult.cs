﻿#pragma warning disable CS8618 // Non-nullable field is uninitialized.
using System;

namespace Hashgraph
{
    /// <summary>
    /// The results returned from a contract call.
    /// </summary>
    public sealed record ContractCallResult
    {
        /// <summary>
        /// The values returned from the contract call.
        /// </summary>
        public ContractCallResultData Result { get; internal init; }
        /// <summary>
        /// An error string returned from the system if there was a problem.
        /// </summary>
        public string Error { get; internal init; }
        /// <summary>
        /// Bloom filter for record
        /// </summary>
        public ReadOnlyMemory<byte> Bloom { get; internal init; }
        /// <summary>
        /// The amount of gas that was used.
        /// </summary>
        public ulong Gas { get; internal init; }
        /// <summary>
        /// Log events returned by the function.
        /// </summary>
        public ContractEvent[] Events { get; internal init; }
        /// <summary>
        /// Addresses of any contracts created as a side effect of
        /// this contract call.
        /// </summary>
        public Address[] CreatedContracts { get; internal init; }
    }
    /// <summary>
    /// Represents the log events returned by a contract function call.
    /// </summary>
    public sealed class ContractEvent
    {
        /// <summary>
        /// Address of a contract that emitted the event.
        /// </summary>
        public Address Contract { get; internal init; }
        /// <summary>
        /// Bloom filter for this log record.
        /// </summary>
        public ReadOnlyMemory<byte> Bloom { get; internal init; }
        /// <summary>
        /// Associated Event Topics
        /// </summary>
        public ReadOnlyMemory<byte>[] Topic { get; internal init; }
        /// <summary>
        /// The event data returned.
        /// </summary>
        public ContractCallResultData Data { get; internal init; }
    }
}