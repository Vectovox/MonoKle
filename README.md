## Description
MonoKle is an open-source library for [MonoGame](https://github.com/MonoGame/MonoGame). Its main purpose is to make development and prototyping both easier and faster by implementing common functionality for reuse across projects, regardless of platform or game idea.

A lot of the functionality is an abstraction on MonoGame but as the project has grown, it is now more opinionated with regards to how to structure your game project. Therefore, it is split into <b>MonoKle</b> with the core classes and <b>MonoKle.Engine</b> that owns the background scaffolding and leaves you free to work on your idea.

## Requirements
To use MonoKle, your project needs to support or reference the following:

* [MonoGame](https://github.com/MonoGame/MonoGame)
* [.NET Standard 2.1](https://github.com/dotnet/standard)
* [MoreLINQ](https://github.com/morelinq/MoreLINQ)

## Usage
It is easy to get started using MonoKle. Just reference the DLL in your game and you are up and running. Do not forget to copy the .XML documentation file to the same folder as the library!

If you want to contribute to MonoKle or fork it you need to have MonoGame installed (nuget recommended). After that you just clone the repository and open the project file. It is as easy as that!

For full information see the documentation within the repository.

## State of code
As this is still a personal project that grows organically, the code is in constant flux and each version may, and likely does, break compatibility. Expect changes!

## License
MonoKle is under the Microsoft Public License (MS-PL), see the LICENSE.txt file for more details. Third-party libraries used by MonoKle are under their own licenses.
