# Contributing

WebSharper is a Free Software project, and we welcome your contributions!

* [What to contribute](#what-to-contribute)
* [How to contribute](#how-to-contribute)
  * [Required software](#requirements)
  * [Building from the command line](#build-cli)
  * [Setting up your development environment](#devenv)
  * [Running the tests](#tests)
  * [Project structure](#structure)

<a name="what-to-contribute"></a>
## What to contribute?

We welcome all types of contributions, particularly:

* Bug fixes in [the issue tracker](https://github.com/dotnet-websharper/ui/issues)
* Standard library improvements (JavaScript APIs, etc.)
* Compiler optimizations and improvements
* Feature suggestions are welcome on [the Gitter chat](https://gitter.im/intellifactory/websharper) and [the issue tracker](https://github.com/dotnet-websharper/ui/issues); we suggest that you discuss new features with the rest of the team on these channels before getting started on implementation.

<a name="how-to-contribute"></a>
## How to contribute

<a name="requirements"></a>
### Required software

It is possible to work on WebSharper on Windows, Linux and OSX.

To compile this project, you need the following installed:

* The .NET Core SDK 2.1.300 or newer. You can download it [here](https://www.microsoft.com/net/download).
* The .NET Framework 4.6.1.
  * For Windows, it is preinstalled, or you can download it [here](https://www.microsoft.com/en-us/download/details.aspx?id=49981).
  * For Linux and OSX, this is provided by Mono. You can follow the install instructions [here](https://www.mono-project.com/download/stable/). For Linux, you should also follow the instructions to install `xsp` in order to run the test project.

<a name="build-cli"></a>
### Building from the command line

This project can be built using the script `build.cmd` on Windows, or `build.sh` on Linux andr OSX.
In the following shell snippets, a command line starting with `build` means `.\build.cmd` on Windows and `./build.sh` on Linux and OSX.

Simply running `build` compiles the UI libraries and tests in debug mode. The following targets are available:

* `build ws-builddebug`

    Equivalent to simple `build`: compiles the libraries and tests in debug mode.

* `build ws-buildrelease`

    Compiles the libraries and tests in release mode.

* `build ws-package`

    Compiles the libraries and tests in release mode, then creates NuGet packages in the `build` folder.

* `build ws-clean`

    Deletes temporary and output directories.

The following options are available:

* `build [TARGET] -ef verbose`

    Makes compilation more verbose. Equivalently, set the `verbose` environment variable to `true`.

<a name="devenv"></a>
### Setting up your development environment

We recommend that you use one of the following development environments:

* On Windows: [Visual Studio 2017](https://visualstudio.microsoft.com/vs/).
* On all platforms: [Visual Studio Code](https://code.visualstudio.com/) with the following extensions:
  * `ionide-fsharp` for F# support
  * `ms-vscode.csharp` for C# support

<a name="tests"></a>
### Running the tests

WebSharper defines and uses its own test framework, WebSharper.Testing. It runs on the client side and is backed by [qUnit](https://qunitjs.com/). So running the WebSharper test suite consists in running a web application which looks like this:

![Unit testing screenshot](https://github.com/dotnet-websharper/core/raw/master/docs/qunit.png)

This repository contains several test projects, detailed in the [project structure](#structure). They are ASP.NET applications hosting WebSharper applications; some of them are Client-Server applications, others are SPAs. Here is how to run them:

* If you are using Visual Studio, you can simply open `WebSharper.sln`, set the test project you want as the startup project, and Run.

* On Linux or OSX, you can browse into the project's folder and simply run `xsp`.

<a name="structure"></a>
### Project structure

Here is the detail of the project structure:

* Libraries:
  * `WebSharper.UI` contains the core library for both reactive programming and DOM management.
  * `WebSharper.UI.CSharp` contains the C#-friendly API for `WebSharper.UI`.
  * `WebSharper.UI.Templating.Common` contains the code for HTML templating that is needed both during compilation and during server-side runtime.
  * `WebSharper.UI.Templating.Runtime` contains the server-side and client-side runtime for the HTML templating.
  * `WebSharper.UI.Templating` contains the F# type provider for HTML templating.
  * `WebSharper.UI.CSharp.Templating` contains the C# code generator for HTML templating.
  * `WebSharper.UI.CSharp.Templating.Analyzer` contains the Roslyn analyzer that invokes the code generator on file save in Visual Studio.
  * `WebSharper.UI.CSharp.Templating.Build` contains the MSBuild task that invokes the code generator pre-compilation.
* Tests:
  * `WebSharper.UI.Tests` contains all the F# tests that do not rely on HTML templating.
  * `WebSharper.UI.Templating.Tests` contains the F# client-side HTML templating tests.
  * `WebSharper.UI.Templating.ServerSide.Tests` contains the F# server-side HTML templating tests.
  * `WebSharper.UI.Routing.Tests` contains the F# router tests.
  * `WebSharper.UI.CSharp.Tests` contains all the C# tests.
