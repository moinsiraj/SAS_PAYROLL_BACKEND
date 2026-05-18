using BOL.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace BLL.Utility
{
    public class Getway
    {
        private readonly string reqUrl = "http://103.125.255.105/GetConnectionInfo";

        private static DbConnectionInfo _dbConnectionInfo = new DbConnectionInfo();
        private readonly HttpClient _httpClient;
        private static string _connectionString = null;

        public Getway(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public static void Initialize(DbConnectionInfo dbConnectionInfo)
        {
            _dbConnectionInfo = dbConnectionInfo;
        }
        public async Task<DbConnectionInfo> GetConnectionInfoAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(reqUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    _dbConnectionInfo = JsonSerializer.Deserialize<DbConnectionInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return _dbConnectionInfo;
        }

        public static string SpecFoCon => GenerateConnectionString("SpecFo");
        public static string Dg_Payroll => GenerateConnectionString("dg_hrpayroll");

        private static string GenerateConnectionString(string dbName)
        {
            if (!string.IsNullOrEmpty(_dbConnectionInfo.host) || !string.IsNullOrEmpty(_dbConnectionInfo.user) || !string.IsNullOrEmpty(_dbConnectionInfo.password))
            {
                _connectionString = String.Format("Data Source={0};Max Pool Size=100;pooling='true';TrustServerCertificate=true;connection timeout=5000;MultipleActiveResultSets=True;Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}", Decrypt(_dbConnectionInfo.host), dbName, Decrypt(_dbConnectionInfo.user), Decrypt(_dbConnectionInfo.password));
            }
            return _connectionString;
        }
        private static string Decrypt(string encryptedText)
        {
            using (Aes aes = Aes.Create())
            {
                string key = "cdInfoDb";
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32));
                aes.IV = new byte[16];

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
    }
}
