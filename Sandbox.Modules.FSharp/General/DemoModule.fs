namespace Sandbox.Modules.FSharp.General

open Sandbox.Core

[<SandboxModule("F# Demo Module", "fs-demo", "This module is a simple test of integrating the Visual Basic module collection with the module service.")>]
[<ModuleTags("general", "sandbox", "testing")>]
type DemoModule() =
   inherit SandboxModule()
   override b.Execute() = b.SendResponse(SandboxEventType.None, "Hello World from an F# module!")