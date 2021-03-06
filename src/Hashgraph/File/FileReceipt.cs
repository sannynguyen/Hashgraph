﻿#pragma warning disable CS8618 // Non-nullable field is uninitialized.

namespace Hashgraph
{
    /// <summary>
    /// A transaction receipt containing information concerning the newly created file.
    /// </summary>
    public sealed class FileReceipt : TransactionReceipt
    {
        /// <summary>
        /// The address of the newly created file.
        /// </summary>
        public Address File { get; internal set; }
    }
}