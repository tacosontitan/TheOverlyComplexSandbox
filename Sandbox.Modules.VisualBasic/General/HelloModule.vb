Imports Sandbox.Core
Namespace Sandbox.Modules.VisualBasic.General
    <SandboxModule("Hello World", "vb-hello", "This module is a simple test of integrating the Visual Basic module collection with the module service.")>
    <ModuleTags("general", "sandbox", "testing")>
    Public Class HelloModule
        Inherits SandboxModule

        Protected Overrides Sub Execute()
            SendResponse(SandboxEventType.None, "Hello from Visual Basic!")
        End Sub
    End Class
End Namespace