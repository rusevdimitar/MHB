using Portable.Licensing.Security.Cryptography;
using System.Xml.Linq;
using System.Xml.XPath;

namespace MHB.Licensing
{
    using Portable.Licensing;
    using Portable.Licensing.Validation;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using License = Portable.Licensing.License;

    /// <summary>
    /// LicenseManager used to create, encrypt and validate licenses, license features and attributes and generate key pairs
    /// </summary>
    public class LicenseManager
    {
        #region Constants

        private const string MachineKeyAttrName = "MachineKey";

        private const string PublicKey = "PublicKey";

        #endregion Constants

        #region Fields

        private readonly string _licenseFilePath;
        private readonly string _publicKey;

        private License _license;

        #endregion Fields

        #region Properties

        public License License
        {
            get
            {
                if (this._license == null)
                {
                    string licenseXml = File.ReadAllText(this._licenseFilePath);

                    this._license = License.Load(licenseXml);
                }

                return this._license;
            }
        }

        #endregion Properties

        #region Constructors

        public LicenseManager(string licenseFilePath)
        {
            if (File.Exists(licenseFilePath))
                this._licenseFilePath = licenseFilePath;
            else
                throw new FileNotFoundException($"Cannot find license file at {licenseFilePath}!");

            this._publicKey = LicenseManager.ParsePublicKeyFromLicenseFile(this._licenseFilePath);
        }

        #endregion Constructors

        #region Public members

        public static void GenerateKeyPair(string password, out string privateKey, out string publicKey)
        {
            KeyGenerator keyGenerator = KeyGenerator.Create();

            KeyPair keyPair = keyGenerator.GenerateKeyPair();

            privateKey = keyPair.ToEncryptedPrivateKeyString(password);

            publicKey = keyPair.ToPublicKeyString();
        }

        public static string GenerateLicense(string keyPassword, string name, string email, DateTime expirationDate, int maxUsers, Dictionary<string, string> features, Dictionary<string, string> additionalAttributes, out string privateKey, out string publicKey)
        {
            GenerateKeyPair(keyPassword, out privateKey, out publicKey);

            additionalAttributes.Add(LicenseManager.PublicKey, publicKey);

            License license = License.New()
                .WithUniqueIdentifier(Guid.NewGuid())
                .As(LicenseType.Standard)
                .ExpiresAt(expirationDate)
                .WithAdditionalAttributes(additionalAttributes)
                .WithMaximumUtilization(maxUsers)
                .WithProductFeatures(features)
                .LicensedTo(name, email)
                .CreateAndSignWithPrivateKey(privateKey, keyPassword);

            return license.ToString();
        }

        public bool ValidateLicense(out string validationOutput)
        {
            bool result = false;

            validationOutput = string.Empty;

            try
            {
                LicenseAttributes licenseAttributes = this.License.AdditionalAttributes;

                string machineFingerprintFromLicenseFile = licenseAttributes.Get(MachineKeyAttrName);

                string actualMachineFingerprint = new MachineInfo().GetMachineFingerprint();

                if (machineFingerprintFromLicenseFile == actualMachineFingerprint)
                {
                    IEnumerable<IValidationFailure> validationFailures = this.License.Validate()
                        .ExpirationDate()
                        .When(lic => lic.Type == LicenseType.Standard)
                        .And()
                        .Signature(this._publicKey)
                        .AssertValidLicense()
                        .ToList();

                    if (!validationFailures.Any())
                    {
                        result = true;
                    }

                    foreach (IValidationFailure failure in validationFailures)
                        validationOutput += $"{failure.GetType().Name} : {failure.Message} - {failure.HowToResolve}{Environment.NewLine}";
                }
                else
                {
                    throw new Exception("License was issued to a different machine!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LicenseManagement.ValidateLicense: {ex.Message}", ex);
            }

            return result;
        }

        public bool ValidateFeatureActivation(string featureName)
        {
            bool result = false;

            try
            {
                LicenseAttributes productFeatures = this.License.ProductFeatures;

                string value = productFeatures.Get(featureName);

                bool.TryParse(value, out result);
            }
            catch (Exception ex)
            {
                throw new Exception($"LicenseManagement.ValidateFeatureActivation: {ex.Message}", ex);
            }

            return result;
        }

        #endregion Public members

        #region Private members

        private static string ParsePublicKeyFromLicenseFile(string licenseFilePath)
        {
            string publicKey = string.Empty;

            try
            {
                XDocument licenseXml = XDocument.Parse(File.ReadAllText(licenseFilePath));

                XElement publicXElement = licenseXml.XPathSelectElement($"/License/LicenseAttributes/Attribute[@name='{LicenseManager.PublicKey}']");

                if (!string.IsNullOrEmpty(publicXElement?.Value))
                {
                    publicKey = publicXElement.Value;
                }
                else
                {
                    throw new Exception($"LicenseManager.ParsePublicKeyFromLicenseFile: Cannot parse public key value from {licenseFilePath}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LicenseManager.ParsePublicKeyFromLicenseFile: {ex.Message}", ex.InnerException);
            }

            return publicKey;
        }

        #endregion Private members
    }
}