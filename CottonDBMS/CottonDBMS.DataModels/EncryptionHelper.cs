//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.IO;
using System.Text;
using PCLCrypto;
using static PCLCrypto.WinRTCrypto;

namespace CottonDBMS.Helpers
{
    /// <summary>
    /// This class is used to encode/decode the settings written to the USB drive for installing the truck software and for
    /// masking the data contained in the Azure Keys transmitted in the QR code that link RFID Module Scan.
    /// </summary>
    public static class EncryptionHelper
    {
        private static string password = "G1n@pp7711#!0000";

        public static string Encrypt(string s)
        {
            var symProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmName.Aes, SymmetricAlgorithmMode.Cbc, SymmetricAlgorithmPadding.Zeros);
            ICryptographicKey key = null;
            key = symProvider.CreateSymmetricKey(Encoding.UTF8.GetBytes(password));
            var plainString = s;
            var plain = Encoding.UTF8.GetBytes(plainString);
            var encrypted = CryptographicEngine.Encrypt(key, plain);
            var encryptedString = Convert.ToBase64String(encrypted);
            return encryptedString;
        }

        public static string Decrypt(string encryptedString)
        {
            var symProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmName.Aes, SymmetricAlgorithmMode.Cbc, SymmetricAlgorithmPadding.None);
            ICryptographicKey key = null;
            key = symProvider.CreateSymmetricKey(Encoding.UTF8.GetBytes(password));
            var encrypted = Convert.FromBase64String(encryptedString);
            var decrypted = CryptographicEngine.Decrypt(key, encrypted);
            var decryptedString = Encoding.UTF8.GetString(decrypted, 0, decrypted.Length);
            return decryptedString;
        }
    }

}