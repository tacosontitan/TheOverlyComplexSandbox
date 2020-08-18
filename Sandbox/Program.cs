using Sandbox.Core;
using Sandbox.Services;
using System;

namespace Sandbox {
    class Program {

        #region TODOs

        // Clean up post response.
        // Add async support.
        // Add support for inline args with a command.

        #endregion

        #region Constants

        private const ConsoleColor DEFAULT_COLOR = ConsoleColor.Gray;
        private const ConsoleColor INFORMATIONAL_COLOR = ConsoleColor.Magenta;
        private const ConsoleColor SUCCESS_COLOR = ConsoleColor.Green;
        private const ConsoleColor WARNING_COLOR = ConsoleColor.Yellow;
        private const ConsoleColor FAILURE_COLOR = ConsoleColor.Red;
        private const string INTRODUCTION_MESSAGE = @"Welcome to the Sandbox! In order to get started, you'll need to supply a command. If you're not sure on which commands are available, you can request a list of supported commands with `cmds`. You can exit the application at any time by sending `exit` as a response to any request.";

        #endregion

        #region Fields

        private static bool wasCommandCanceled = false;

        #endregion

        #region Main Method

        static void Main(string[] args) {
            // Welcome the user to the sandbox.
            Console.WriteLine(INTRODUCTION_MESSAGE);

            // Subscribe to the module manager's events.
            ModuleService.Instance.ExecutionStarted += Instance_ExecutionStarted;
            ModuleService.Instance.ExecutionCompleted += Instance_ExecutionCompleted;
            ModuleService.Instance.ExecutionFailed += Instance_ExecutionFailed;
            ModuleService.Instance.ExecutionCancelled += Instance_ExecutionCancelled;
            ModuleService.Instance.RequestReceived += Instance_RequestReceived;
            ModuleService.Instance.ResponseReceived += Instance_ResponseReceived;

            // Begin the pimary loop.
            int invalidResponses = 0;
            while (!wasCommandCanceled) {
                Console.Write("Please enter a command: ");
                string executionKey = Console.ReadLine();
                Console.WriteLine();
                if (ModuleService.Instance.Exists(executionKey)) {
                    ModuleParameter[] parameters = ModuleService.Instance.GetModuleParameters(executionKey);
                    foreach (ModuleParameter parameter in parameters) {
                        WriteRequest(new SandboxEventArgs(null, string.Empty, parameter.RequestMessage, SandboxEventType.None));
                        parameter.Value = Console.ReadLine();
                    }

                    if (ModuleService.Instance.TryExecute(executionKey, parameters)) {
                        Console.WriteLine();
                        invalidResponses = 0;
                    } else if (++invalidResponses % 3 == 0)
                        Console.WriteLine(INTRODUCTION_MESSAGE);
                }
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
            Console.ForegroundColor = INFORMATIONAL_COLOR;
            e.Data = Console.ReadLine();
            Console.ForegroundColor = DEFAULT_COLOR;
        }
        private static void Instance_ResponseReceived(object sender, SandboxEventArgs e) => WriteResponseAsLine(e);

        #endregion

        #region Helper Methods

        private static void WriteRequest(SandboxEventArgs e) {
            Console.ForegroundColor = GetConsoleColorForResponseType(e);
            Console.Write($"{e.Message} ");
            Console.ForegroundColor = DEFAULT_COLOR;
        }
        private static void WriteResponseAsLine(SandboxEventArgs e) {
            Console.ForegroundColor = GetConsoleColorForResponseType(e);
            Console.WriteLine(e.Message);
            Console.ForegroundColor = DEFAULT_COLOR;
        }
        public static ConsoleColor GetConsoleColorForResponseType(SandboxEventArgs e) {
            switch (e.EventType) {
                case SandboxEventType.Success: return SUCCESS_COLOR;
                case SandboxEventType.Warning: return WARNING_COLOR;
                case SandboxEventType.Failure: return FAILURE_COLOR;
                case SandboxEventType.Information: return INFORMATIONAL_COLOR;
                default: return DEFAULT_COLOR;
            }
        }

        #endregion

    }
}