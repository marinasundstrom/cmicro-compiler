# C Micro compiler

Compiler school project 2008-2009.

My first ever compiler. Written in C#/.NET. Originally for .NET Framework but ported to .NET 9.

The program is compiling "example.cm" into "output.exe". 

## Purpose

To show how much I have evolved since then.

The up-to-date source code was not available, so had to de-compile. Thus it doesn't entirely reflect my style of code. But I left an earlier snapshot in this repo.

You maye also want to check out my [Visual Basic Lite compiler](https://github.com/marinasundstrom/vb-lite-compiler), from 2 years after this when I was at university.

## When looking back

Some thoughts about the code. More to be added.

### Parsing expressions

I think I implemented the [Shunting yard algorithm](https://en.wikipedia.org/wiki/Shunting_yard_algorithm) for parsing operator precedence in binary expressions.

In later compilers, I implemented a [Operator-precedence parser](https://en.wikipedia.org/wiki/Operator-precedence_parser) that was based on the source code for IronPython. Though I didn't know the name back then.

### Overused of interfaces

In this project, I use interfaces without reason or understanding why. Not every class needs an interface.

## Repository

### Decompiled

Decompiled with IL Spy 7.2.0.0 rc on Avalonia 0.10. On March 29 2024.

The output might not be runnable at this moment.

Minor changes to deal with the fact that we jumped from .NET Framework to .NET 9 Preview 2 (.NET Core). The APIs are not exactly the same.

I have left comments in places where this applies.

Summary of changes:
* Had to deal with the changes to the Reflection Emit APIs.
* Removal of use of AppDomains when generating assemblies with IL bytecode - AppDomain do no longer exist in modern .NET.
* Changes to the use of threading. It was used in an unnecessary and even inappropriate way.

### Compiled

Contains the executables from which the files in Decompiled are derived. 

### Project files (Sep 2008)

The Project files folder contains a snapshot of the code from approx. September 2008. 

It has been upgraded to run on .NET 8 with minimal modifications.