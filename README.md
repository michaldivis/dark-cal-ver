<img src="assets/icon.png?raw=true" width="200">

# Dark CalVer

C# simple implementation of the [CalVer](https://calver.org/) versioning scheme.

The version is calculated based on a timestamp, reference date and accuracy.

The `CalVer` type contained in this library has the following properties:
- VersionString (`string`): `yy.MM.ddhh` format (when using `Accuracy.Hours`).
- Version (`System.Version`): same as *VersionString*, but parsed as a `System.Version`.
- VersionNumber (`long`): total number of hours since the reference date (when using `Accuracy.Hours`).

## Nuget

[![Nuget](https://img.shields.io/nuget/v/Divis.DarkCalVer?label=Divis.DarkCalVer)](https://www.nuget.org/packages/Divis.DarkCalVer/)

DarkCalVer is available using [nuget](https://www.nuget.org/packages/Divis.DarkCalVer/). To install DarkCalVer, run the following command in the [Package Manager Console](http://docs.nuget.org/docs/start-here/using-the-package-manager-console)

```Powershell
PM> Install-Package Divis.DarkCalVer
```

## Usage

**Basic usage**

```csharp
using DarkCalVer;

var timestamp = new DateTime(2023, 1, 1, 08, 15, 30);
var calVer = CalVer.Create(timestamp);

// Version string: 23.01.0108
// Version: 23.1.108
// Version number: 26 312
```

**Preventing leading zeros**

Will prevent leading zeros by adding 50 to the minor and revision components of the version.

```csharp
using DarkCalVer;

var timestamp = new DateTime(2023, 1, 1, 08, 15, 30);

var calVer = CalVer.Create(new CalVerOptions
{
    Timestamp = timestamp,
    PreventLeadingZeros = true,
});

// Version string: 23.51.5108
// Version: 23.51.5108
// Version number: 26 312
```

**Custom reference date**

A reference date is used to calculate the version number. When using `Accuracy.Hours`, the version number is the total number of hours since the reference date. This number can become very large, especially when using `Accuracy.Minutes` or `Accuracy.Seconds`. To mitigate this, you can use a custom reference date that's as far in the future as your use case allows.

```csharp
using DarkCalVer;

var timestamp = new DateTime(2023, 1, 1, 08, 15, 30);

var calVer = CalVer.Create(new CalVerOptions
{
    Timestamp = timestamp,
    ReferenceDate = new DateTime(2023, 1, 1),
});

// Version string: 23.01.0108
// Version: 23.1.108
// Version number: 8
```

**Accuracy**

The generated version will be unique per a given unit of accuracy. For example, if you use `Accuracy.Hours`, the version will be unique per hour, and so on.

```csharp
using DarkCalVer;

var timestamp = new DateTime(2023, 1, 1, 08, 15, 30);

var calVer = CalVer.Create(new CalVerOptions
{
    Timestamp = timestamp,
    Accuracy = Accuracy.Minutes
});

// Version string: 23.01.010815
// Version: 23.1.10815
// Version number: 1 578 735
```