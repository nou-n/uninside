using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uninside.Auth
{

    internal static class RandomFidGenerator
    {
        private static readonly byte FID_4BIT_PREFIX = 0b01110000; // 0x70
        private static readonly byte REMOVE_PREFIX_MASK = 0b00001111; // 0x0F
        private const int FID_LENGTH = 22;

        /// <summary>
        /// Creates a random FID of valid format without checking if the FID is already in use by any Firebase Installation.
        /// </summary>
        /// <returns>Random FID value</returns>
        public static string CreateRandomFid()
        {
            // A valid FID has exactly 22 base64 characters, which is 132 bits, or 16.5 bytes.
            byte[] uuidBytes = GetBytesFromUUID(Guid.NewGuid());

            // Ensure the array size is correct
            if (uuidBytes.Length < 17)
            {
                throw new InvalidOperationException("UUID byte array must have at least 17 bytes.");
            }

            uuidBytes[16] = uuidBytes[0];
            uuidBytes[0] = (byte)((REMOVE_PREFIX_MASK & uuidBytes[0]) | FID_4BIT_PREFIX);
            return EncodeFidBase64UrlSafe(uuidBytes);
        }

        private static string EncodeFidBase64UrlSafe(byte[] rawValue)
        {
            string base64String = Convert.ToBase64String(rawValue);
            // Replace + with -, / with _, and remove padding
            base64String = base64String.Replace('+', '-').Replace('/', '_').TrimEnd('=');
            return base64String.Substring(0, FID_LENGTH);
        }

        private static byte[] GetBytesFromUUID(Guid uuid)
        {
            byte[] bytes = new byte[17]; // Changed size to 17
            byte[] guidBytes = uuid.ToByteArray();
            Array.Copy(guidBytes, bytes, guidBytes.Length);
            bytes[16] = 0; // You can initialize the 17th byte as needed
            return bytes;
        }
    }
}
