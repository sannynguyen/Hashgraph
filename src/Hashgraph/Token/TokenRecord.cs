﻿#pragma warning disable CS8618 // Non-nullable field is uninitialized.

namespace Hashgraph
{
    /// <summary>
    /// A transaction record containing information regarding
    /// new token coin balance, typically returned from methods
    /// that can affect a change on the total circulation supply.
    /// </summary>
    public sealed class TokenRecord : TransactionRecord
    {
        /// <summary>
        /// The current (new) total balance of tokens 
        /// in all accounts (the whole denomination).
        /// </summary>
        public ulong Circulation { get; internal set; }
    }
}