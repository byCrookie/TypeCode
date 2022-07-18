# TypeCode
Develop c# code faster by generating .NET specific boilerplate code using reflection on assemblies. Available a console or wpf application.

## Features

* Generate specflow tables for classes
* Generate fake constructor for unit-test class
* Generate composer for strategies by interface
* Generate mapper for two classes
* Generate builder for class using builder pattern

Further documentation can be found here: [Wiki](https://github.com/byCrookie/TypeCode/wiki)

## Dependencies & Acknowledgements
* Framework: https://github.com/byCrookie/Framework
* Icons: https://fontawesome.com/
* AsyncAwaitBestPractices.MVVM: https://github.com/brminnick/AsyncAwaitBestPractices
* Jab: https://github.com/pakrym/jab
* Spectre.Console: https://github.com/spectreconsole/spectre.console
* Serilog: https://github.com/serilog/serilog
* Nito.AsyncEx.Context: https://github.com/StephenCleary/AsyncEx
* Humanizer.Core https://github.com/Humanizr/Humanizer


## How to use

* Make sure that you have installed ".NET 6.0 Desktop Runtime". If not, download it from Microsoft.

> :warning: **Information: Windows Defender** <br />
> Windows Defender blocks execution of the exe.
> The application needs permission.
> Right-click the exe and select properties.
> Then enable/allow execution.

### Zip

* Download wpf or console application zip from releases
* Extract zip
* Start the .exe
* Change configuration in settings

### Installer (Wpf)

* Download wpf msi installer from releases
* Click the msi installer
* Walk through the installation pages
* Search with window search for TypeCode
* Start the app
* Change configuration in settings

### Dev

* Clone the git repository
* Change the "localPackages" path in the nuget.config
* {Token}: Z2hwX1hybmFLaVIyTm1zaGVWRVpqMjVLbHZsNTBjdldKYjMzQ2hPeQ== -> Convert Base64 back to Text First
* Execute: dotnet nuget add source --username byCrookie --password {Token} --name byCrookie_Github --store-password-in-clear-text https://nuget.pkg.github.com/byCrookie/index.json

## Contributing / Issues
All contributions are welcome! If you have any issues or feature requests, either implement it yourself or create an issue, thank you.

## Donation
If you like this project, feel free to donate and support further development. Thank you.

* Bitcoin (BTC) Donations using Bitcoin (BTC) Network -> bc1qygqya2w3hgpvy8hupctfkv5x06l69ydq4su2e2
* Ethereum (ETH) Donations using Ethereum (ETH) Network -> 0x1C0416cC1DDaAEEb3017D4b8Dcd3f0B82f4d94C1







