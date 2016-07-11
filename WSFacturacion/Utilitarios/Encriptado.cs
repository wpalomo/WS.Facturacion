using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace Telectronica.Utilitarios
{
    public static class Encriptado
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Función Encriptado de Contraseñas
        /// </summary>
        /// <param name="sPassword">string - Password a encriptar
        /// <returns>Encriptado de sPasword</returns>
        /// ***********************************************************************************************
        public static string EncriptarPassword(string sPassword)
        {
            //string skey= "E38AD214943DAAD1D64C102FAE029DE4AFE9DA3D" 
            byte[] key = {0xE3, 0x8A, 0xD2, 0x14, 0x94, 0x3D, 0xAA, 0xD1, 0xD6, 0x4C, 0x10, 0x2F, 0xAE, 0x02, 0x9D, 0xE4, 0xAF, 0xE9, 0xDA, 0x3D};
            //string origen = Convert.ToBase64String(key);

            HMACSHA1 myhmacsha1 = new HMACSHA1(key);
            ASCIIEncoding encoding = new ASCIIEncoding();

            byte[] hashValue = myhmacsha1.ComputeHash(encoding.GetBytes(sPassword));
            return Convert.ToBase64String(hashValue);

        }
        /// ***********************************************************************************************
        /// <summary>
        /// Función CryptIn de VB.
        /// </summary>
        /// <param name="sCadena">string - dato a encriptar
        /// <returns>Encriptado de sCadena</returns>
        /// ***********************************************************************************************
        public static string Encriptar(string sCadena)
        {
            string sRet = string.Empty;

            //Convertimos a un array de bytes ASCII
            System.Text.Encoding encoding = Encoding.UTF8;
            byte[] bArray = encoding.GetBytes(sCadena);

            foreach (byte bChar in bArray)
            {
                sRet += (bChar ^ 255) + 255;
            }
            //                strCrypt = strCrypt & (Asc(Mid$(Cadena, i, 1)) Xor 255) + 255
            return sRet;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Función CryptOut de VB.
        /// </summary>
        /// <param name="sCadena">string - dato a Desencriptar
        /// <returns>sCadena desencriptada</returns>
        /// ***********************************************************************************************
        public static string Desencriptar(String sCadena)
        {
            string sRet = string.Empty;

            try
            {
                for (int i = 0; i < sCadena.Length; i += 3)
                {
                    sRet += Convert.ToChar((Convert.ToInt32(sCadena.Substring(i, 3)) - 255) ^ 255);
                }
                // strDeCrypt = strDeCrypt & chr((val(Mid$(Cadena, i, 3)) - 255) Xor 255)

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            return sRet;
        }


        private static byte[] RijndaelKey = null;
        //private static byte[] RijndaelIV = null;
        /// ***********************************************************************************************
        /// <summary>
        /// Genera un encriptado que se puede desencriptar.
        /// </summary>
        /// <param name="sDato">string - dato a encriptar
        /// <param name="len">int - longitud del dato a encriptar
        /// <returns>Encriptado de sDato</returns>
        /// ***********************************************************************************************
        public static byte[] Encriptar(string sData, int len)
        {
            byte[] bts = null;
            try
            {
                // Create a key
                CreateKey();


                // Encrypt text using key, and IV.
                bts = encryptStringToBytes_AES(sData, RijndaelKey);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bts;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Gdesencripta lo encriptado con Encriptar
        /// </summary>
        /// <param name="sDato">string - dato a encriptar
        /// <param name="len">int - longitud del dato a encriptar
        /// <returns>Encriptado de sDato</returns>
        /// ***********************************************************************************************
        public static string Desencriptar(byte[] btData, int len)
        {
            string sDataDesencriptada = "";
            try
            {
                // Create a key
                CreateKey();

                // Decrypt the text from a file using the file name, key, and IV.
                sDataDesencriptada = decryptStringFromBytes_AES(btData, RijndaelKey);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sDataDesencriptada;
        }

        private static void CreateKey()
        {
            // Set the encryption key to the password hash
            RijndaelKey = Hash("jN325#87");
            //RijndaelIV = new byte[16];
            //Array.Copy(Hash("klm0987%"), RijndaelIV, 16);
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Genera un encriptado que no se puede desencriptar.
        /// usar para passwords
        /// </summary>
        /// <param name="sDato">string - dato a encriptar
        /// <param name="len">int - longitud del dato a encriptar
        /// <returns>Encriptado de sDato</returns>
        /// ***********************************************************************************************
        public static byte[] Hash(string sDato)
        {
            // Get a 256 bit hash from the password string 
            byte[] pwBytes = Encoding.UTF8.GetBytes(sDato);
            SHA256Managed sha = new SHA256Managed();

            // Set the encryption key to the password hash
            return sha.ComputeHash(pwBytes);
        }

        static byte[] encryptStringToBytes_AES(string plainText, byte[] Key )
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");

            byte[] IV=null;

            // Declare the stream used to encrypt to an in memory
            // array of bytes.
            MemoryStream msEncrypt = null;

            // Declare the RijndaelManaged object
            // used to encrypt the data.
            RijndaelManaged aesAlg = null;

            try
            {
                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged();
                aesAlg.Key = Key;
                //aesAlg.IV = RijndaelIV;
                aesAlg.GenerateIV();
                //La salvamos porque cambia
                IV = aesAlg.IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                msEncrypt = new MemoryStream();
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {

                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                }

            }
            finally
            {

                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.

            // Get encrypted array of bytes, add the IV at the beginning
            byte[] encrypted = msEncrypt.ToArray();
            
            byte[] cryptBytes = new byte[encrypted.Length + 16];
            IV.CopyTo(cryptBytes, 0);
            encrypted.CopyTo(cryptBytes, 16);

            return cryptBytes;
            
            //return encrypted;
        }

        static string decryptStringFromBytes_AES(byte[] cipherText, byte[] Key)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");

            
            // Create an array to hold the initialization vector (IV)
            byte[] IV = new byte[16];

            // Create an array to hold the encrypted text
            byte[] cryptBytes = new byte[cipherText.Length - 16];

            // Extract the IV from the input byte array.
            Array.Copy(cipherText, 0, IV, 0, 16);

            // Extract the encrypted message from the input byte array.
            Array.Copy(cipherText, 16, cryptBytes, 0, cryptBytes.Length);
            //Array.Copy(cipherText, 0, cryptBytes, 0, cryptBytes.Length);
            

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            try
            {
                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged();
                aesAlg.Key = Key;
                //IV = RijndaelIV;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cryptBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }

            }
            finally
            {

                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;

        }
    }
}
