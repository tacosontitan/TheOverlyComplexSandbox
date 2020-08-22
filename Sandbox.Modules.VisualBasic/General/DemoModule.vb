Imports Sandbox.Core
Namespace Sandbox.Modules.VisualBasic.General
    <SandboxModule("Visual Basic Demo Module", "vb-demo", "This module is a simple test of integrating the Visual Basic module collection with the module service.")>
    <ModuleTags("general", "sandbox", "testing")>
    Public Class DemoModule
        Inherits SandboxModule

        Protected Overrides Sub Execute()
            SendResponse(SandboxEventType.None, "This is a test of integrating a Visual Basic module!")
        End Sub
    End Class
End Namespace