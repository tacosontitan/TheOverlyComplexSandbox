using Sandbox.Core;
using Sandbox.Modules;
using System;

namespace Sandbox {
    class Program {

        #region TODOs

        // Clean up post response.
        // Add async support.
        // Add support for inline args with a command.

        #endregion

        #region Constants

        private const string INTRODUCTION_MESSAGE = @"Welcome to the Sandbox! In order to get started, you'll need to supply a command. If you're not sure on which commands are available, you can request a list of supported commands with `cmds`. You can exit the application at any time by sending `exit` as a response to any request.";

        #endregion

        #region Fields

        private static bool wasCommandCanceled = false;

        #endregion

        #region Main Method

        static void Main(string[] args) {
            // Welcome the user to the sandbox.
            Console.WriteLine(INTRODUCTION_MESSAGE);

            // Set the colors used to represent the status of a response.
            ModuleManager.Instance.SuccessColor = ConsoleColor.Green;
            ModuleManager.Instance.WarningColor = ConsoleColor.Yellow;
            ModuleManager.Instance.ErrorColor = ConsoleColor.Red;
            ModuleManager.Instance.InformationColor = ConsoleColor.Cyan;
            ModuleManager.Instance.DefaultColor = ConsoleColor.Gray;

            // Subscribe to the module manager's events.
            ModuleManager.Instance.ExecutionStarted += Instance_ExecutionStarted;
            ModuleManager.Instance.ExecutionCompleted += Instance_ExecutionCompleted;
            ModuleManager.Instance.ExecutionFailed += Instance_ExecutionFailed;
            ModuleManager.Instance.ExecutionCancelled += Instance_ExecutionCancelled;
            ModuleManager.Instance.RequestReceived += Instance_RequestReceived;
            ModuleManager.Instance.ResponseReceived += Instance_ResponseReceived;

            // Begin the pimary loop.
            int invalidResponses = 0;
            while (!wasCommandCanceled) {
                Console.Write("Please enter a command: ");
                string response = Console.ReadLine();
                Console.WriteLine();
                if (ModuleManager.Instance.TryExecute(response)) {
                    Console.WriteLine();
                    invalidResponses = 0;
                } else if (++invalidResponses % 3 == 0)
                    Console.WriteLine(INTRODUCTION_MESSAGE);
            }
        }

        

        #endregion

        #region Module Manager Events

        private static void Instance_ExecutionStarted(object sender, SandboxEventArgs e) => WriteResponseAsLine(e);
        private static void Instance_ExecutionCancelled(object sender, SandboxEventArgs e) {
            wasCommandCanceled = true;
            WriteResponseAsLine(e);
        }
        private static void Instance_ExecutionCompleted(object sender, SandboxEventArgs e) => WriteResponseAsLine(e);
        private static void Instance_ExecutionFailed(object sender, SandboxEventArgs e) => WriteResponseAsLine(e);
        private static void Instance_RequestReceived(object sender, SandboxEventArgs e) {
            WriteRequest(e);
            Console.ForegroundColor = (ConsoleColor)ModuleManager.Instance.InformationColor;
            e.Data = Console.ReadLine();
            Console.ForegroundColor = (ConsoleColor)ModuleManager.Instance.DefaultColor;
        }
        private static void Instance_ResponseReceived(object sender, SandboxEventArgs e) => WriteResponseAsLine(e);

        #endregion

        #region Helper Methods

        private static void WriteRequest(SandboxEventArgs e) {
            Console.ForegroundColor = GetConsoleColorForResponseType(e);
            Console.Write($"{e.Message} ");
            Console.ForegroundColor = (ConsoleColor)ModuleManager.Instance.DefaultColor;
        }
        private static void WriteResponseAsLine(SandboxEventArgs e) {
            Console.ForegroundColor = GetConsoleColorForResponseType(e);
            Console.WriteLine(e.Message);
            Console.ForegroundColor = (ConsoleColor)ModuleManager.Instance.DefaultColor;
        }
        public static ConsoleColor GetConsoleColorForResponseType(SandboxEventArgs e) {
            switch (e.EventType) {
                case SandboxEventType.Success: return (ConsoleColor)ModuleManager.Instance.SuccessColor;
                case SandboxEventType.Warning: return (ConsoleColor)ModuleManager.Instance.WarningColor;
                case SandboxEventType.Failure: return (ConsoleColor)ModuleManager.Instance.ErrorColor;
                case SandboxEventType.Information: return (ConsoleColor)ModuleManager.Instance.InformationColor;
                default: return (ConsoleColor)ModuleManager.Instance.DefaultColor;
            }
        }

        #endregion

    }
}