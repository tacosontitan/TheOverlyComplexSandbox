Imports Sandbox.Core

<SandboxModule("Demo Module", "vb-demo", "This module is a simple test of integrating the Visual Basic module collection with the module service.")>
Public Class DemoModule
    Inherits SandboxModule

    Protected Overrides Sub Execute()
        SendResponse(SandboxEventType.None, "This is a test of integrating a Visual Basic module!")
    End Sub
End Class
