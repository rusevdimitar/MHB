namespace MHB.Licensing
{
    using System;
    using System.Linq;
    using System.Management;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// MachineInfo is used to generate an unique fingerprint of a machine based on actual hardware identification
    /// </summary>
    public class MachineInfo : IDisposable
    {
        #region Fields

        private readonly ManagementObjectSearcher _managementObjectSearcher;

        private readonly Query _processorQuery;
        private readonly Query _motherBoardProductQuery;
        private readonly Query _motherBoardManufacturerQuery;
        private readonly Query _motherBoardSerialNumberQuery;

        #endregion Fields

        #region Constructors

        public MachineInfo()
        {
            this._managementObjectSearcher = new ManagementObjectSearcher();

            this._processorQuery = new Query("Win32_Processor", "ProcessorId");
            this._motherBoardProductQuery = new Query("Win32_BaseBoard", "Product");
            this._motherBoardManufacturerQuery = new Query("Win32_BaseBoard", "Manufacturer");
            this._motherBoardSerialNumberQuery = new Query("Win32_BaseBoard", "SerialNumber");
        }

        #endregion Constructors

        #region Public members

        public string GetMachineFingerprint()
        {
            string result = string.Empty;

            try
            {
                // Get CPU id
                string processorId = this.ExecuteQuery(this._processorQuery);

                // Get Mainboard name
                string mainBoardProductName = this.ExecuteQuery(this._motherBoardProductQuery);

                // Get Mainboard manufacturer
                string mainBoardManufacturer = this.ExecuteQuery(this._motherBoardManufacturerQuery);

                // Get Mainboard serial number
                string mainBoardSerial = this.ExecuteQuery(this._motherBoardSerialNumberQuery);

                // Combine HWIDs into a single string
                string fingerPrint = $"{processorId}{mainBoardProductName}{mainBoardManufacturer}{mainBoardSerial}";

                // Convert to Base64 and hash to hide sensitive/readable data which would expose identifier used to construct the machine key (i.e. Dell inc. in mainboard manufacturer property)
                string base64Result = this.ToBase64String(fingerPrint);

                result = this.CalculateMd5Hash(base64Result);
            }
            catch (Exception ex)
            {
                throw new Exception($"SystemInfo.GetMachineFingerprint: {ex.Message}", ex);
            }

            return result;
        }

        #endregion Public members

        #region Private members

        private string ExecuteQuery(Query query)
        {
            string result = string.Empty;

            try
            {
                this._managementObjectSearcher.Query = new ObjectQuery(query.QueryText);

                result = this._managementObjectSearcher.Get().OfType<ManagementObject>().FirstOrDefault()?[query.MethodName].ToString();
            }
            catch (Exception ex)
            {
                throw new Exception($"SystemInfo.ExecuteQuery: {ex.Message}", ex);
            }

            return result;
        }

        private struct Query
        {
            public Query(string tableName, string methodName)
            {
                this.QueryText = $"Select {methodName} from {tableName}";
                this.MethodName = methodName;
            }

            public string QueryText { get; private set; }

            public string MethodName { get; private set; }
        }

        private string ToBase64String(string input)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(input);

            return Convert.ToBase64String(plainTextBytes);
        }

        private string CalculateMd5Hash(string input)
        {
            MD5 md5 = MD5.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();

            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        #endregion Private members

        #region Destructor

        public void Dispose()
        {
            this._managementObjectSearcher.Dispose();
        }

        #endregion Destructor
    }
}