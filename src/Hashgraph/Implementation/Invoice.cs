﻿using Google.Protobuf;
using Proto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hashgraph.Implementation
{
    internal sealed class Invoice : IInvoice
    {
        private readonly TxId _txId;
        private readonly string _memo;
        private readonly ReadOnlyMemory<byte> _txBytes;
        private readonly Dictionary<ByteString, SignaturePair> _signatures;

        TxId IInvoice.TxId { get { return _txId; } }
        string IInvoice.Memo { get { return _memo; } }
        ReadOnlyMemory<byte> IInvoice.TxBytes { get { return _txBytes; } }
        internal Invoice(TransactionBody transactionBody)
        {
            _txId = transactionBody.TransactionID.ToTxId();
            _memo = transactionBody.Memo;
            _txBytes = transactionBody.ToByteArray();
            _signatures = new Dictionary<ByteString, SignaturePair>();
        }
        void IInvoice.AddSignature(KeyType type, ReadOnlyMemory<byte> publicPrefix, ReadOnlyMemory<byte> signature)
        {
            var key = ByteString.CopyFrom(publicPrefix.Span);
            var value = ByteString.CopyFrom(signature.Span);
            var pair = new Proto.SignaturePair();
            switch (type)
            {
                case KeyType.Ed25519:
                    pair.Ed25519 = value;
                    break;
                case KeyType.ECDSA384:
                    pair.ECDSA384 = value;
                    break;
                case KeyType.RSA3072:
                    pair.RSA3072 = value;
                    break;
                case KeyType.Contract:
                    pair.Contract = value;
                    break;
            }
            if (_signatures.TryGetValue(key, out Proto.SignaturePair? existing))
            {
                if (!pair.Equals(existing))
                {
                    throw new ArgumentException("Signature with Duplicate Prefix Identifier was provided, but did not have an Identical Signature.");
                }
            }
            else
            {
                _signatures.Add(key, pair);
            }
        }
        internal SignedTransaction GetSignedTransaction(int prefixTrimLimit)
        {
            var count = _signatures.Count;
            if (count == 0)
            {
                throw new InvalidOperationException("A transaction or query requires at least one signature, sometimes more.  None were found, did you forget to assign a Signatory to the context, transaction or query?");
            }
            var signatures = new SignatureMap();
            if (count == 1 && prefixTrimLimit < 1)
            {
                signatures.SigPair.Add(_signatures.Values.First());
            }
            else
            {
                var list = _signatures.ToArray();
                var keys = new byte[count][];
                for (var length = Math.Max(1, prefixTrimLimit); true; length++)
                {
                    var unique = true;
                    for (var i = 0; unique && i < count; i++)
                    {
                        var key = keys[i] = list[i].Key.Memory.Slice(0, Math.Min(list[i].Key.Length, length)).ToArray();
                        for (var j = 0; j < i; j++)
                        {
                            if (Enumerable.SequenceEqual(key, keys[j]))
                            {
                                unique = false;
                                break;
                            }
                        }
                    }
                    if (unique)
                    {
                        break;
                    }
                }
                for (var i = 0; i < count; i++)
                {
                    var sig = list[i].Value;
                    sig.PubKeyPrefix = ByteString.CopyFrom(keys[i]);
                    signatures.SigPair.Add(sig);
                }
            }
            return new SignedTransaction
            {
                BodyBytes = ByteString.CopyFrom(_txBytes.Span),
                SigMap = signatures
            };
        }
    }
}

