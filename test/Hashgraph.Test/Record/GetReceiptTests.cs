﻿using Hashgraph.Implementation;
using Hashgraph.Test.Fixtures;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Hashgraph.Test.Record
{
    [Collection(nameof(NetworkCredentials))]
    public class GetReceiptTests
    {
        private readonly NetworkCredentials _network;
        public GetReceiptTests(NetworkCredentials network, ITestOutputHelper output)
        {
            _network = network;
            _network.Output = output;
        }
        [Fact(DisplayName = "Get Receipt: Can get Create Topic Receipt")]
        public async Task CanGetCreateTopicReceipt()
        {
            await using var fx = await TestTopic.CreateAsync(_network);
            var receipt = await fx.Client.GetReceiptAsync(fx.Record.Id);            
            var topicReceipt = Assert.IsType<CreateTopicReceipt>(receipt);
            Assert.Equal(fx.Record.Id, topicReceipt.Id);
            Assert.Equal(fx.Record.Status, topicReceipt.Status);
            Assert.Equal(fx.Record.CurrentExchangeRate, topicReceipt.CurrentExchangeRate);
            Assert.Equal(fx.Record.NextExchangeRate, topicReceipt.NextExchangeRate);
            Assert.Equal(fx.Record.Topic, topicReceipt.Topic);
        }
        [Fact(DisplayName = "Get Receipt: Can get Submit Message Receipt")]
        public async Task CanGetSubmitMessageReceipt()
        {
            await using var fx = await TestTopic.CreateAsync(_network);
            var message = Encoding.ASCII.GetBytes(Generator.String(10, 100));
            var receipt1 = await fx.Client.SubmitMessageAsync(fx.Record.Topic, message, fx.ParticipantPrivateKey);
            var receipt2 = await fx.Client.GetReceiptAsync(receipt1.Id);
            var sendReceipt = Assert.IsType<SubmitMessageReceipt>(receipt2);
            Assert.Equal(receipt1.Id, sendReceipt.Id);
            Assert.Equal(receipt1.Status, sendReceipt.Status);
            Assert.Equal(receipt1.CurrentExchangeRate, sendReceipt.CurrentExchangeRate);
            Assert.Equal(receipt1.NextExchangeRate, sendReceipt.NextExchangeRate);
            Assert.Equal(receipt1.SequenceNumber, sendReceipt.SequenceNumber);
            Assert.Equal(receipt1.RunningHash.ToArray(), sendReceipt.RunningHash.ToArray());
            Assert.Equal(receipt1.RunningHashVersion, sendReceipt.RunningHashVersion);
        }

        [Fact(DisplayName = "Get Receipt: Can get Create Contract Receipt")]
        public async Task CanGetCreateContractRecipt()
        {
            await using var fx = await StatefulContract.CreateAsync(_network);
            var receipt = await fx.Client.GetReceiptAsync(fx.ContractRecord.Id);
            var createRecipt = Assert.IsType<CreateContractReceipt>(receipt);

            Assert.Equal(fx.ContractRecord.Id, createRecipt.Id);
            Assert.Equal(fx.ContractRecord.Status, createRecipt.Status);
            Assert.Equal(fx.ContractRecord.CurrentExchangeRate, createRecipt.CurrentExchangeRate);
            Assert.Equal(fx.ContractRecord.NextExchangeRate, createRecipt.NextExchangeRate);
            Assert.Equal(fx.ContractRecord.Contract, createRecipt.Contract);
        }
        [Fact(DisplayName = "Get Receipt: Can get Create Account Receipt")]
        public async Task CanGetCreateAccountReceipt()
        {
            await using var fx = await TestAccount.CreateAsync(_network);
            var receipt = await fx.Client.GetReceiptAsync(fx.Record.Id);
            var accountReceipt = Assert.IsType<CreateAccountReceipt>(receipt);
            Assert.Equal(fx.Record.Id, accountReceipt.Id);
            Assert.Equal(fx.Record.Status, accountReceipt.Status);
            Assert.Equal(fx.Record.CurrentExchangeRate, accountReceipt.CurrentExchangeRate);
            Assert.Equal(fx.Record.NextExchangeRate, accountReceipt.NextExchangeRate);
            Assert.Equal(fx.Record.Address, accountReceipt.Address);
        }
        [Fact(DisplayName = "Get Receipt: Can get Create File Receipt")]
        public async Task CanGetCreateFileReceipt()
        {
            await using var fx = await TestFile.CreateAsync(_network);
            var receipt = await fx.Client.GetReceiptAsync(fx.Record.Id);
            var fileReceipt = Assert.IsType<FileReceipt>(receipt);
            Assert.Equal(fx.Record.Id, fileReceipt.Id);
            Assert.Equal(fx.Record.Status, fileReceipt.Status);
            Assert.Equal(fx.Record.CurrentExchangeRate, fileReceipt.CurrentExchangeRate);
            Assert.Equal(fx.Record.NextExchangeRate, fileReceipt.NextExchangeRate);
            Assert.Equal(fx.Record.File, fileReceipt.File);
        }
    }
}