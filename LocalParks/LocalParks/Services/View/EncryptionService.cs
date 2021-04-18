using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;

namespace LocalParks.Services.View
{
    public class EncryptionService : IEncryptionService
    {
        private readonly string _parameter;
        private readonly IDataProtector _protector;
        public EncryptionService(IDataProtectionProvider provider, IConfiguration configuration)
        {
            // Could not obtain IOptions - need to fix and reduce configuration exposure to Service
            _protector = provider.CreateProtector(configuration["QueryStrings:Key"]);
            _parameter = configuration["QueryStrings:Param"];
        }
        public string Encrypt(string input, bool appendParam = false)
        {
            if (appendParam) input = $"{_parameter}={input}";
            if (!input.StartsWith('?')) input = $"?{input}";
            string encryptedQuery = _protector.Protect(input);
            return encryptedQuery;
        }
        public string Encrypt(int input, bool appendParam = false)
        {
            return Encrypt(input.ToString(), appendParam);
        }
        public string Encrypt(string param, int value, bool appendParam = false)
        {
            return Encrypt($"{param}={value}", appendParam);
        }
        public string Decrypt(string param, bool removeQuestionMark = true)
        {
            var result = _protector.Unprotect(param);

            if (!removeQuestionMark) return result;
            return result[1..];
        }
    }
}
