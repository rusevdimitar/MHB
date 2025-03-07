using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHB.Licensing.CLI
{
    internal class Program
    {
        #region Constants

        private const string OptionGetMachineKey = "GetMachineKey";

        private const string OptionValidateLicense = "ValidateLicense";

        private const string OptionKeyPassword = "KeyPassword";

        private const string OptionName = "Name";

        private const string OptionEmail = "Email";

        private const string OptionExpirationDate = "ExpirationDate";

        private const string OptionMaxUsers = "MaxUsers";

        private const string OptionFeatures = "Features";

        private const string OptionLicenseFilePath = "LicenseFilePath";

        private const string OptionGenerateLicense = "GenerateLicense";

        private const string FeatureNamePrefix = "Feature";

        private const string AttributeNamePrefix = "Attribute";

        private const string OptionHelp1 = "Help";

        private const string OptionHelp2 = "h";

        private const string HelpContents =
@"Televic.Licensing.CLI Help:

usage: Televic.Licensing.CLI.exe
	[-GetMachineKey]
	[-ValidateLicense
		-PublicKeyPath=<file path>
		-LicenseFilePath=<file path>]
	[-GenerateLicense
		-KeyPassword=<password>
		-Name=<license owner>
		-Email=<owner email>
		-ExpirationDate=<yyyy-mm-dd>
		-MaxUsers=<max license utilization>
		-FeatureXXXX=<value> -FeatureXXXX=<value> ...
		-AttributeXXXX=<value> -AttributeXXXX=<value> ...]
These are common Televic.Licensing.CLI commands used in various situations:

/GetMachineKey
	Generates an unique identifier based on hardware
	parameters of the machine being executed on

/ValidateLicense
	Validates a license - required parameters are:
	PublicKeyPath - path to public key file and
	LicenseFilePath- path to the license file to be validated

/GenerateLicense
	Generates a new license - required parameters are:
	/KeyPassword - passphrase for the key pair
	/Name - License owner name
	/Email - License owner email
	/ExpirationDate - License expiration date (YYYY-MM-DD)
	/MaxUsers - Maximum user utilization
	/FeatureXXXX - Multiple parameter pairs starting
				   with the 'Feature' prefix can be passed and
				   added to the Features list of the license file
	/AttributeXXXX - Multiple parameter pairs starting
					 with the 'Attribute' prefix can be passed and
					 added to the Attributes list of the license file";

        #endregion Constants

        #region Application entry point

        private static void Main(string[] args)
        {
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

                        case Program.OptionGenerateLicense:

                            OutputGenerateLicenseAndFiles(arguments);

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

        private static void OutputGenerateLicenseAndFiles(Dictionary<string, string> arguments)
        {
            string privateKey = string.Empty, publicKey = string.Empty;

            Func<string, string> parseArg = new Func<string, string>(key =>
            {
                string argumentValue;

                if (!arguments.TryGetValue(key, out argumentValue))
                {
                    throw new KeyNotFoundException($"{key} parameter is required!");
                }

                return argumentValue;
            });

            string password = parseArg(Program.OptionKeyPassword);
            string name = parseArg(Program.OptionName);
            string email = parseArg(Program.OptionEmail);
            string expirationDate = parseArg(Program.OptionExpirationDate);
            string maxUsers = parseArg(Program.OptionMaxUsers);

            var features = arguments.Where(arg => arg.Key.StartsWith(Program.FeatureNamePrefix))
                .Select(
                    kv =>
                        new KeyValuePair<string, string>(
                            kv.Key.Replace(Program.FeatureNamePrefix, string.Empty), kv.Value))
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            var additionalAttributes = arguments.Where(arg => arg.Key.StartsWith(Program.AttributeNamePrefix))
                .Select(
                    kv =>
                        new KeyValuePair<string, string>(
                            kv.Key.Replace(Program.AttributeNamePrefix, string.Empty), kv.Value))
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            DateTime date;

            DateTime.TryParse(expirationDate, out date);

            if (date < DateTime.Now)
                throw new ArgumentOutOfRangeException(nameof(expirationDate), "Expiration date cannot be lower than today!");

            int maxUtilization = -1;

            int.TryParse(maxUsers, out maxUtilization);

            string license = LicenseManager.GenerateLicense(password, name, email, date, maxUtilization, features,
                additionalAttributes, out privateKey, out publicKey);

            if (!Directory.Exists("License"))
                Directory.CreateDirectory("License");

            File.WriteAllText(@"License\License.lic", license);

            File.WriteAllText(@"License\PrivateKey", privateKey);

            File.WriteAllText(@"License\PublicKey", publicKey);

            Console.WriteLine("Key-pair and license files generated successfully. Check output directory.");
        }

        private static void OutputValidateLicense(Dictionary<string, string> arguments)
        {
            string licenseFilePath = string.Empty;

            arguments.TryGetValue(Program.OptionLicenseFilePath, out licenseFilePath);

            bool result = File.Exists(licenseFilePath) ? ValidateLicense(licenseFilePath) : ValidateLicense();

            Console.WriteLine($"License valid: {result}");
        }

        private static void OutputMachineKey()
        {
            string machineKey = new MachineInfo().GetMachineFingerprint();

            Console.WriteLine(machineKey);
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