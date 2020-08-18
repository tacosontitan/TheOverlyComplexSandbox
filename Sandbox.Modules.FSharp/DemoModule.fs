namespace Sandbox.Modules.FSharp

open Sandbox.Core

[<SandboxModule("F# Demo Module", "fs-demo", "This module is a simple test of integrating the Visual Basic module collection with the module service.")>]
type DemoModule() =
   inherit SandboxModule()
   override b.Execute() = printfn "Hello World from an F# module!"