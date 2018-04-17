<p align="center">
  <img src="http://res.cloudinary.com/vidsy/image/upload/v1503160820/CoZ_Icon_DARKBLUE_200x178px_oq0gxm.png" width="125px">
</p>

<h1 align="center">neo-hypervm</h1>

<p align="center">      
  <a href="https://travis-ci.org/CityOfZion/neo-hypervm">
    <img src="https://travis-ci.org/CityOfZion/neo-hypervm.svg?branch=master">
  </a>
  <a href="https://github.com/CityOfZion/neo-hypervm/blob/master/LICENSE.md">
    <img src="https://img.shields.io/badge/license-MIT-blue.svg">
  </a>
</p>

<p align="center">
    Virtual Machine for the <a href="https://neo.org">NEO</a> blockchain written in <b>C++</b>
</p>

# Overview

**neo-hypervm** aims to be a virtual machine in C++ 100% compatible with the NEO standard.

# Projects

* NeoVM - C++ Native Virtual Machine
* NeoVM.Interop - Interoperability for C# calls
* NeoVM.Interop.Tests - Unit Tests

# Installation

### Visual studio (Windows Users)
For debugging the native source make sure to append the following line in `**/Properties/launchSettings.json` 

```
"nativeDebugging": true
```
Copy binaries or create a symbolic link, also you can set `NEO_HYPERVM_PATH`environment variable with the path of the native library

```
cd C:\neo-hypervm\tests\NeoVM.Interop.Tests\bin\Debug\netcoreapp2.0
mkdir Windows
cd Windows
mkdir x86
mkdir x64

cd x86
cmd /c mklink NeoVM.dll C:\neo-hypervm\src\NeoVM\Win32\Debug\NeoVM.dll
cd ../x64
cmd /c mklink NeoVM.dll C:\neo-hypervm\src\NeoVM\x64\Debug\NeoVM.dll
```

### Other
There is a `Makefile` in the root of the project. To build the `VM` source:

```
make
```

# Contributing

Feel free to contribute to this project after reading the
[contributing guidelines](https://github.com/CityOfZion/neo-go/blob/master/CONTRIBUTING.md).

Before starting to work on a certain topic, create an new issue first,
describing the feauture/topic you are going to implement and remember to use [development](https://github.com/CityOfZion/neo-hypervm/tree/development) branch.

# License

- Open-source [MIT](https://github.com/CityOfZion/neo-hypervm/blob/master/LICENCE.md)
