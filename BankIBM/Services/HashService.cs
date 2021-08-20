using System;
using System.Linq;
using System.Security.Cryptography;

namespace BankIBM.Services
{
    public class HashService
    {
        private static int saltSize = 16;
        private static int keySize = 32;
        private static int iterationsConst = 10000;

        public static string Hash(string password)
        {
            using (var algorithm = new Rfc2898DeriveBytes(password,
                saltSize,
                iterationsConst,
                HashAlgorithmName.SHA512))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(keySize));
                var salt = Convert.ToBase64String(algorithm.Salt);

                return $"{iterationsConst}.{salt}.{key}";
            }
        }

        public static (bool verified, bool needsUpgrade) Check(string hash, string password)
        {
            var parts = hash.Split('.', 3);

            if (parts.Length != 3)
            {
                throw new FormatException("Unexpected hash format. " +
                  "Should be formatted as `{iterations}.{salt}.{hash}`");
            }

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            var needsUpgrade = iterations != iterationsConst;

            using (var algorithm = new Rfc2898DeriveBytes(
              password,
              salt,
              iterations,
              HashAlgorithmName.SHA512))
            {
                var keyToCheck = algorithm.GetBytes(keySize);

                var verified = keyToCheck.SequenceEqual(key);

                return (verified, needsUpgrade);
            }
        }
    }
}
