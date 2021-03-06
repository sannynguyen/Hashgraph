# .NET Client Library for Hedera Hashgraph

Hashgraph provides access to the [Hedera Hashgraph](https://www.hedera.com/) Network for the .NET platform.  It manages the communication details with participating [network nodes](https://docs.hedera.com/guides/mainnet/mainnet-nodes) and provides an efficient set asynchronous interface methods for consumption by .NET programs.

## Documentation

For an [introduction](https://bugbytesinc.github.io/Hashgraph/tutorials/index.html) on how to use this library to connect with the Hedera Network, please visit our documentation [website](https://bugbytesinc.github.io/Hashgraph/).

## Cloning

The latest version of this project can be cloned with the following command:

```
$ git clone https://github.com/bugbytesinc/Hashgraph.git
```

**Please note:** this project no longer references the [Hedera Protobuf](https://github.com/hashgraph/hedera-protobuf) project, which has been abandoned by Hedera in favor of the [hedera-services](https://github.com/hashgraph/hedera-services) repository.  The [hedera-services](https://github.com/hashgraph/hedera-services) repository is now the "source of truth" for the Hedera API Potobuf (HAPI).  Since the [hedera-services](https://github.com/hashgraph/hedera-services) project is too large to be included in this project as a git submodule; we have resorted, for now, to copying the protobuf files from that project into the reference subdirectory of this project.  Going forward, the naming convention for that directory will be "hapi-" followed by the tag information corresponding to the version of protobuf files copied over (as generated by `git describe`).  By following this convention, it will be possible to cross-reference the protobuf consumed by this project with a specific version found in the git history of the [hedera-services](https://github.com/hashgraph/hedera-services) project.

## Contributing
While we are in the process of building the preliminary infrastructure for this project, please direct any feedback, requests or questions to  [Hedera’s Discord](https://discordapp.com/invite/FFb9YFX) channel.

## Build Status

| Main Branch | vNext (Preview Network)
| - | -
| [![Build Status](https://bugbytes.visualstudio.com/Hashgraph/_apis/build/status/Hashgraph%20TESTNET%20Continuous%20&%20Nightly%20Build?branchName=master)](https://bugbytes.visualstudio.com/Hashgraph/_apis/build/status/Hashgraph%20TESTNET%20Continuous%20&%20Nightly%20Build?branchName=master) | [![Build Status](https://bugbytes.visualstudio.com/Hashgraph/_apis/build/status/Hashgraph%20PREVIEWNET%20Continuous%20&%20Nightly%20Build?branchName=previewnet)](https://bugbytes.visualstudio.com/Hashgraph/_apis/build/status/Hashgraph%20PREVIEWNET%20Continuous%20&%20Nightly%20Build?branchName=previewnet)

## Packages

| Nuget
| - 
[![NuGet](https://img.shields.io/nuget/v/hashgraph.svg)](http://www.nuget.org/packages/hashgraph/)


## Build Requirements
This project relies protobuf support found in .net core 5, 
previous versions of the .net core framework will not work.
(At the time of this writing we are in [5.0.100](https://dotnet.microsoft.com/download/dotnet-core/5.0))

Visual Studio is not required to build the library, however the project
references the [NSec.Cryptography](https://nsec.rocks/) library, which 
loads the libsodium.dll library which relies upon the VC++ runtime. In
order to execute tests, the [Microsoft Visual C++ Redistributable](https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads)
must be installed on the build agent if Visual Studio is not.

## License
Hashgraph is licensed under the [Apache 2.0 license](https://licenses.nuget.org/Apache-2.0).