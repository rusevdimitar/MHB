using MHB.Licensing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MachineFingerprintGenerator
{
    internal class Program
    {
        #region Constants

        private const string OptionGetMachineKey = "GetMachineKey";

        private const string OptionValidateLicense = "ValidateLicense";

        private const string OptionLicenseFilePath = "LicenseFilePath";

        private const string OptionHelp1 = "Help";

        private const string OptionHelp2 = "h";

        private const string HelpContents =
@"MHB.Licensing.CLI Help:

usage: MHB.Licensing.CLI.exe
	[-GetMachineKey]
    [-ValidateLicense
		-PublicKeyPath=<file path>
		-LicenseFilePath=<file path>]
These are common MHB.Licensing.CLI commands used in various situations:

/GetMachineKey
	Generates an unique identifier based on hardware
	parameters of the machine being executed on

/ValidateLicense
	Validates a license - required parameters are:
	PublicKeyPath - path to public key file and
	LicenseFilePath- path to the license file to be validated";

        #endregion Constants

        #region Application entry point

        private static void Main(string[] args)
        {
            OutputMachineKey();

            string[] optionSwitches = new string[] { };

            Dictionary<string, string> arguments = ConsoleHelper.ParseArguments(args, out optionSwitches);

            try
            {
                if (optionSwitches.Length == 0)
                {
                    OutputHelp();
                }

                foreach (string option in optionSwitches)
                {
                    switch (option)
                    {
                        case Program.OptionGetMachineKey:

                            OutputMachineKey();

                            break;

                        case Program.OptionValidateLicense:

                            OutputValidateLicense(arguments);

                            break;

                        case Program.OptionHelp1:
                        case Program.OptionHelp2:

                            OutputHelp();

                            break;

                        default:
                            OutputHelp();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                OutputHelp();
            }
            finally
            {
                Console.Read();
            }
        }

        #endregion Application entry point

        #region Private members

        private static void OutputHelp()
        {
            Console.Write(Program.HelpContents);
        }

        private static void OutputMachineKey()
        {
            string machineKey = new MachineInfo().GetMachineFingerprint();

            Console.WriteLine(machineKey);

            File.WriteAllText("MachineKey", machineKey);
        }

        private static void OutputValidateLicense(Dictionary<string, string> arguments)
        {
            string licenseFilePath = string.Empty;

            arguments.TryGetValue(Program.OptionLicenseFilePath, out licenseFilePath);

            bool result = File.Exists(licenseFilePath) ? ValidateLicense(licenseFilePath) : ValidateLicense();

            Console.WriteLine($"License valid: {result}");
        }

        private static bool ValidateLicense(string licenseFilePath = @"License\License.lic")
        {
            bool licenseIsValid = false;

            string output = string.Empty;

            try
            {
                LicenseManager licenseManager = new LicenseManager(licenseFilePath);

                licenseIsValid = licenseManager.ValidateLicense(out output);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ValidateLicense: {ex.Message}");
            }
            finally
            {
                Console.WriteLine(output);
            }

            return licenseIsValid;
        }

        #endregion Private members
    }
}