﻿using Hashgraph.Test.Fixtures;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Hashgraph.Test.Token
{
    [Collection(nameof(NetworkCredentials))]
    public class SuspendTokenTests
    {
        private readonly NetworkCredentials _network;
        public SuspendTokenTests(NetworkCredentials network, ITestOutputHelper output)
        {
            _network = network;
            _network.Output = output;
        }
        [Fact(DisplayName = "Suspend Tokens: Can Suspend Token Coin Trading")]
        public async Task CanSuspendTokenCoinTrading()
        {
            await using var fxAccount = await TestAccount.CreateAsync(_network);
            await using var fxToken = await TestToken.CreateAsync(_network, fx =>
            {
                fx.Params.GrantKycEndorsement = null;
                fx.Params.InitializeSuspended = false;
            }, fxAccount);
            var circulation = fxToken.Params.Circulation;
            var xferAmount = circulation / 3;

            await AssertHg.TokenStatusAsync(fxToken, fxAccount, TokenTradableStatus.Tradable);

            await fxToken.Client.SuspendTokenAsync(fxToken.Record.Token, fxAccount, fxToken.SuspendPrivateKey);

            await AssertHg.TokenStatusAsync(fxToken, fxAccount, TokenTradableStatus.Suspended);

            var tex = await Assert.ThrowsAsync<TransactionException>(async () =>
            {
                await fxToken.Client.TransferTokensAsync(fxToken, fxToken.TreasuryAccount, fxAccount, (long)xferAmount, fxToken.TreasuryAccount);
            });
            Assert.Equal(ResponseCode.AccountFrozenForToken, tex.Status);
            Assert.StartsWith("Unable to execute transfers, status: AccountFrozenForToken", tex.Message);

            await AssertHg.TokenStatusAsync(fxToken, fxAccount, TokenTradableStatus.Suspended);
        }
        [Fact(DisplayName = "Suspend Tokens: Can Suspend Token Coin Trading and get Record")]
        public async Task CanSuspendTokenCoinTradingAndGetRecord()
        {
            await using var fxAccount = await TestAccount.CreateAsync(_network);
            await using var fxToken = await TestToken.CreateAsync(_network, fx =>
            {
                fx.Params.GrantKycEndorsement = null;
                fx.Params.InitializeSuspended = false;
            }, fxAccount);
            var circulation = fxToken.Params.Circulation;
            var xferAmount = circulation / 3;

            await AssertHg.TokenStatusAsync(fxToken, fxAccount, TokenTradableStatus.Tradable);

            var record = await fxToken.Client.SuspendTokenWithRecordAsync(fxToken.Record.Token, fxAccount, fxToken.SuspendPrivateKey);
            Assert.Equal(ResponseCode.Success, record.Status);
            Assert.False(record.Hash.IsEmpty);
            Assert.NotNull(record.Concensus);
            Assert.NotNull(record.CurrentExchangeRate);
            Assert.NotNull(record.NextExchangeRate);
            Assert.NotEmpty(record.Hash.ToArray());
            Assert.Empty(record.Memo);
            Assert.InRange(record.Fee, 0UL, ulong.MaxValue);
            Assert.Equal(_network.Payer, record.Id.Address);

            await AssertHg.TokenStatusAsync(fxToken, fxAccount, TokenTradableStatus.Suspended);

            var tex = await Assert.ThrowsAsync<TransactionException>(async () =>
            {
                await fxToken.Client.TransferTokensAsync(fxToken, fxToken.TreasuryAccount, fxAccount, (long)xferAmount, fxToken.TreasuryAccount);
            });
            Assert.Equal(ResponseCode.AccountFrozenForToken, tex.Status);
            Assert.StartsWith("Unable to execute transfers, status: AccountFrozenForToken", tex.Message);

            await AssertHg.TokenStatusAsync(fxToken, fxAccount, TokenTradableStatus.Suspended);
        }
        [Fact(DisplayName = "Suspend Tokens: Can Suspend Token Coin Trading and get Record (No Extra Signatory)")]
        public async Task CanSuspendTokenCoinTradingAndGetRecordNoExtraSignatory()
        {
            await using var fxAccount = await TestAccount.CreateAsync(_network);
            await using var fxToken = await TestToken.CreateAsync(_network, fx =>
            {
                fx.Params.GrantKycEndorsement = null;
                fx.Params.InitializeSuspended = false;
            }, fxAccount);
            var circulation = fxToken.Params.Circulation;
            var xferAmount = circulation / 3;

            await AssertHg.TokenStatusAsync(fxToken, fxAccount, TokenTradableStatus.Tradable);

            var record = await fxToken.Client.SuspendTokenWithRecordAsync(fxToken.Record.Token, fxAccount, ctx => ctx.Signatory = new Signatory(_network.Signatory, fxToken.SuspendPrivateKey));
            Assert.Equal(ResponseCode.Success, record.Status);
            Assert.False(record.Hash.IsEmpty);
            Assert.NotNull(record.Concensus);
            Assert.NotNull(record.CurrentExchangeRate);
            Assert.NotNull(record.NextExchangeRate);
            Assert.NotEmpty(record.Hash.ToArray());
            Assert.Empty(record.Memo);
            Assert.InRange(record.Fee, 0UL, ulong.MaxValue);
            Assert.Equal(_network.Payer, record.Id.Address);

            await AssertHg.TokenStatusAsync(fxToken, fxAccount, TokenTradableStatus.Suspended);

            var tex = await Assert.ThrowsAsync<TransactionException>(async () =>
            {
                await fxToken.Client.TransferTokensAsync(fxToken, fxToken.TreasuryAccount, fxAccount, (long)xferAmount, fxToken.TreasuryAccount);
            });
            Assert.Equal(ResponseCode.AccountFrozenForToken, tex.Status);
            Assert.StartsWith("Unable to execute transfers, status: AccountFrozenForToken", tex.Message);

            await AssertHg.TokenStatusAsync(fxToken, fxAccount, TokenTradableStatus.Suspended);
        }
        [Fact(DisplayName = "Suspend Tokens: Can Suspend Token Coin Trading from Any Account with Suspend Key")]
        public async Task CanSuspendTokenCoinTradingFromAnyAccountWithSuspendKey()
        {
            await using var fxOther = await TestAccount.CreateAsync(_network, fx => fx.CreateParams.InitialBalance = 120_00_000_000);
            await using var fxAccount = await TestAccount.CreateAsync(_network);
            await using var fxToken = await TestToken.CreateAsync(_network, fx =>
            {
                fx.Params.GrantKycEndorsement = null;
                fx.Params.InitializeSuspended = false;
            }, fxAccount);
            var circulation = fxToken.Params.Circulation;
            var xferAmount = circulation / 3;

            await AssertHg.TokenStatusAsync(fxToken, fxAccount, TokenTradableStatus.Tradable);

            await fxToken.Client.SuspendTokenAsync(fxToken.Record.Token, fxAccount, fxToken.SuspendPrivateKey, ctx =>
            {
                ctx.Payer = fxOther.Record.Address;
                ctx.Signatory = fxOther.PrivateKey;
            });

            await AssertHg.TokenStatusAsync(fxToken, fxAccount, TokenTradableStatus.Suspended);

            var tex = await Assert.ThrowsAsync<TransactionException>(async () =>
            {
                await fxToken.Client.TransferTokensAsync(fxToken, fxToken.TreasuryAccount, fxAccount, (long)xferAmount, fxToken.TreasuryAccount);
            });
            Assert.Equal(ResponseCode.AccountFrozenForToken, tex.Status);
            Assert.StartsWith("Unable to execute transfers, status: AccountFrozenForToken", tex.Message);

            await AssertHg.TokenStatusAsync(fxToken, fxAccount, TokenTradableStatus.Suspended);
        }
        [Fact(DisplayName = "Suspend Tokens: Suspending a Frozen Account is a Noop")]
        public async Task SuspendingAFrozenAccountIsANoop()
        {
            await using var fxAccount = await TestAccount.CreateAsync(_network);
            await using var fxToken = await TestToken.CreateAsync(_network, fx =>
            {
                fx.Params.GrantKycEndorsement = null;
                fx.Params.InitializeSuspended = true;
            }, fxAccount);
            var circulation = fxToken.Params.Circulation;
            var xferAmount = circulation / 3;

            await AssertHg.TokenStatusAsync(fxToken, fxAccount, TokenTradableStatus.Suspended);

            await fxToken.Client.SuspendTokenAsync(fxToken.Record.Token, fxAccount, fxToken.SuspendPrivateKey);

            var tex = await Assert.ThrowsAsync<TransactionException>(async () =>
            {
                await fxToken.Client.TransferTokensAsync(fxToken, fxToken.TreasuryAccount, fxAccount, (long)xferAmount, fxToken.TreasuryAccount);
            });
            Assert.Equal(ResponseCode.AccountFrozenForToken, tex.Status);
            Assert.StartsWith("Unable to execute transfers, status: AccountFrozenForToken", tex.Message);
        }
        [Fact(DisplayName = "Suspend Tokens: Can Suspend a Resumed Account")]
        public async Task CanSuspendAResumedAccount()
        {
            await using var fxAccount = await TestAccount.CreateAsync(_network);
            await using var fxToken = await TestToken.CreateAsync(_network, fx =>
            {
                fx.Params.GrantKycEndorsement = null;
                fx.Params.InitializeSuspended = true;
            }, fxAccount);
            var circulation = fxToken.Params.Circulation;
            var xferAmount = circulation / 3;

            await AssertHg.TokenStatusAsync(fxToken, fxAccount, TokenTradableStatus.Suspended);

            await fxToken.Client.ResumeTokenAsync(fxToken.Record.Token, fxAccount, fxToken.SuspendPrivateKey);

            await AssertHg.TokenStatusAsync(fxToken, fxAccount, TokenTradableStatus.Tradable);

            await fxToken.Client.SuspendTokenAsync(fxToken.Record.Token, fxAccount, fxToken.SuspendPrivateKey);

            await AssertHg.TokenStatusAsync(fxToken, fxAccount, TokenTradableStatus.Suspended);

            var tex = await Assert.ThrowsAsync<TransactionException>(async () =>
            {
                await fxToken.Client.TransferTokensAsync(fxToken, fxToken.TreasuryAccount, fxAccount, (long)xferAmount, fxToken.TreasuryAccount);
            });
            Assert.Equal(ResponseCode.AccountFrozenForToken, tex.Status);
            Assert.StartsWith("Unable to execute transfers, status: AccountFrozenForToken", tex.Message);

            await AssertHg.TokenStatusAsync(fxToken, fxAccount, TokenTradableStatus.Suspended);
        }
        [Fact(DisplayName = "Suspend Tokens: Suspend Token Requires Suspend Key to Sign Transaciton")]
        public async Task SuspendTokenRequiresSuspendKeyToSignTransaciton()
        {
            await using var fxAccount = await TestAccount.CreateAsync(_network);
            await using var fxToken = await TestToken.CreateAsync(_network, fx =>
            {
                fx.Params.GrantKycEndorsement = null;
                fx.Params.InitializeSuspended = false;
            });
            var circulation = fxToken.Params.Circulation;
            var xferAmount = circulation / 3;

            await AssertHg.TokenNotAssociatedAsync(fxToken, fxAccount);

            var tex = await Assert.ThrowsAsync<TransactionException>(async () =>
            {
                await fxToken.Client.SuspendTokenAsync(fxToken.Record.Token, fxAccount);
            });
            Assert.Equal(ResponseCode.InvalidSignature, tex.Status);
            Assert.StartsWith("Unable to Suspend Token, status: InvalidSignature", tex.Message);

            await AssertHg.TokenNotAssociatedAsync(fxToken, fxAccount);
        }
        [Fact(DisplayName = "Suspend Tokens: Cannot Suspend Token when Freeze Not Enabled")]
        public async Task CannotSuspendTokenWhenFreezeNotEnabled()
        {
            await using var fxAccount = await TestAccount.CreateAsync(_network);
            await using var fxToken = await TestToken.CreateAsync(_network, fx =>
            {
                fx.Params.GrantKycEndorsement = null;
                fx.Params.SuspendEndorsement = null;
                fx.Params.InitializeSuspended = false;
            }, fxAccount);
            var circulation = fxToken.Params.Circulation;
            var xferAmount = circulation / 3;

            await AssertHg.TokenStatusAsync(fxToken, fxAccount, TokenTradableStatus.NotApplicable);

            var tex = await Assert.ThrowsAsync<TransactionException>(async () =>
            {
                await fxToken.Client.SuspendTokenAsync(fxToken.Record.Token, fxAccount, fxToken.SuspendPrivateKey);
            });
            Assert.Equal(ResponseCode.TokenHasNoFreezeKey, tex.Status);
            Assert.StartsWith("Unable to Suspend Token, status: TokenHasNoFreezeKey", tex.Message);

            await AssertHg.TokenStatusAsync(fxToken, fxAccount, TokenTradableStatus.NotApplicable);

            await fxToken.Client.TransferTokensAsync(fxToken, fxToken.TreasuryAccount, fxAccount, (long)xferAmount, fxToken.TreasuryAccount);

            await AssertHg.TokenStatusAsync(fxToken, fxAccount, TokenTradableStatus.NotApplicable);
        }
    }
}
