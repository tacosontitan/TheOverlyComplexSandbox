namespace Sandbox.Modules.FSharp.General

open Sandbox.Core

[<SandboxModule("Hello World", "fs-hello", "This module is a simple test of integrating the F# module collection with the module service.")>]
[<ModuleTags("general", "sandbox", "testing")>]
type DemoModule() =
   inherit SandboxModule()
   override b.Execute() = b.SendResponse(SandboxEventType.None, "Hello from F#!")