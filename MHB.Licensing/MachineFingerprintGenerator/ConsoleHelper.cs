using System.Collections.Generic;

namespace MachineFingerprintGenerator
{
    internal class ConsoleHelper
    {
        internal static Dictionary<string, string> ParseArguments(string[] args, out string[] optionArgs)
        {
            Dictionary<string, string> arguments = new Dictionary<string, string>();

            List<string> options = new List<string>();

            foreach (string argument in args)
            {
                string arg = argument.TrimStart('-', '/');

                string[] splitted = arg.Split('=');

                if (splitted.Length == 2)
                {
                    arguments[splitted[0]] = splitted[1];
                }
                else
                {
                    options.Add(arg);
                }
            }

            optionArgs = options.ToArray();

            return arguments;
        }
    }
}