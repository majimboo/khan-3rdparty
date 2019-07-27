namespace EncryptDataSet.Data {
   /// <summary>
   /// This class handles encryption related operations
   /// </summary>
   internal static class Cryptography {
      private static System.Text.RegularExpressions.Regex passwordDigits = new System.Text.RegularExpressions.Regex(@"\d", System.Text.RegularExpressions.RegexOptions.Compiled);
      private static System.Text.RegularExpressions.Regex passwordNonWord = new System.Text.RegularExpressions.Regex(@"\W", System.Text.RegularExpressions.RegexOptions.Compiled);
      private static System.Text.RegularExpressions.Regex passwordUppercase = new System.Text.RegularExpressions.Regex(@"[A-Z]", System.Text.RegularExpressions.RegexOptions.Compiled);
      private static System.Text.RegularExpressions.Regex passwordLowercase = new System.Text.RegularExpressions.Regex(@"[a-z]", System.Text.RegularExpressions.RegexOptions.Compiled);

      /// <summary>
      /// This method calculates the strength of the password
      /// </summary>
      /// <param name="password">Password to check</param>
      /// <returns>Password strength between 0 and 100</returns>
      internal static int PasswordStrength(string password) {
         int strength = 0;

         if (password.Length > 5) strength += 5;
         if (password.Length > 10) strength += 15;
         if (passwordDigits.Matches(password).Count >= 1) strength += 5;
         if (passwordDigits.Matches(password).Count >= 3) strength += 15;
         if (passwordNonWord.Matches(password).Count >= 1) strength += 5;
         if (passwordNonWord.Matches(password).Count >= 3) strength += 15;
         if (passwordUppercase.Matches(password).Count >= 1) strength += 5;
         if (passwordUppercase.Matches(password).Count >= 3) strength += 15;
         if (passwordLowercase.Matches(password).Count >= 1) strength += 5;
         if (passwordLowercase.Matches(password).Count >= 3) strength += 15;

         return strength;
      }

      /// <summary>
      /// This method initializes the Aes used to encrypt or decrypt the dataset.
      /// </summary>
      /// <param name="username">Username to use for the encryption</param>
      /// <param name="password">Password to use for the encryption</param>
      /// <returns>New instance of Aes</returns>
      private static System.Security.Cryptography.Aes InitAes(string username, string password) {
         System.Security.Cryptography.Aes aes = new System.Security.Cryptography.AesManaged();
         System.Security.Cryptography.Rfc2898DeriveBytes rfc2898 = new System.Security.Cryptography.Rfc2898DeriveBytes(password, System.Text.Encoding.Unicode.GetBytes(username));

         aes.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
         aes.KeySize = 128;
         aes.Key = rfc2898.GetBytes(16);
         aes.IV = rfc2898.GetBytes(16);

         return aes;
      }

      /// <summary>
      /// Saves the dataset encrypted in specified file
      /// </summary>
      /// <param name="dataset">Dataset to save</param>
      /// <param name="username">Username for encryption</param>
      /// <param name="password">Password for encryption</param>
      /// <param name="fileName">File name where to save</param>
      /// <param name="compress">Should the file be compressed</param>
      internal static void EncryptDataSet(System.Data.DataSet dataset, string username, string password, string fileName, bool compress) {
         // Check the parameters
         if (dataset == null
            || string.IsNullOrEmpty(username)
            || string.IsNullOrEmpty(password)
            || string.IsNullOrEmpty(fileName)) {
            throw new System.ArgumentNullException("All arguments must be supplied.");
         }

         // Save the dataset as encrypted
         using (System.Security.Cryptography.Aes aes = Cryptography.InitAes(username, password)) {
            using (System.IO.FileStream fileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None)) {
               using (System.Security.Cryptography.CryptoStream cryptoStream = new System.Security.Cryptography.CryptoStream(fileStream, aes.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write)) {
                  if (compress) {
                     // when compression is requested, use GZip
                     using (System.IO.Compression.GZipStream zipStream = new System.IO.Compression.GZipStream(cryptoStream, System.IO.Compression.CompressionMode.Compress)) {
                        dataset.WriteXml(zipStream, System.Data.XmlWriteMode.WriteSchema);
                        zipStream.Flush();
                     }
                  } else {
                     dataset.WriteXml(cryptoStream, System.Data.XmlWriteMode.WriteSchema);
                     cryptoStream.FlushFinalBlock();
                  }
               }
            }
         }
      }

      /// <summary>
      /// Reads and decrypts the dataset from the given file
      /// </summary>
      /// <param name="dataset">Dataset to read</param>
      /// <param name="username">Username for decryption</param>
      /// <param name="password">Password for decryption</param>
      /// <param name="fileName">File name to read</param>
      /// <param name="compressed">Is the file compressed</param>
      /// <returns>XmlReadMode used for reading</returns>
      internal static System.Data.XmlReadMode DecryptDataSet(System.Data.DataSet dataset, string username, string password, string fileName, bool compressed) {
         System.Data.XmlReadMode xmlReadMode;

         // Check the parameters
         if (dataset == null
            || string.IsNullOrEmpty(username)
            || string.IsNullOrEmpty(password)
            || string.IsNullOrEmpty(fileName)) {
            throw new System.ArgumentNullException("All arguments must be supplied.");
         }

         // Read the dataset and encrypt it
         using (System.Security.Cryptography.Aes aes = Cryptography.InitAes(username, password)) {
            using (System.IO.FileStream fileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Open)) {
               using (System.Security.Cryptography.CryptoStream cryptoStream = new System.Security.Cryptography.CryptoStream(fileStream, aes.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Read)) {
                  if (compressed) {
                     // when decompression is requested, use GZip
                     using (System.IO.Compression.GZipStream zipStream = new System.IO.Compression.GZipStream(cryptoStream, System.IO.Compression.CompressionMode.Decompress)) {
                        xmlReadMode = dataset.ReadXml(zipStream, System.Data.XmlReadMode.ReadSchema);
                     }
                  } else {
                     xmlReadMode = dataset.ReadXml(cryptoStream, System.Data.XmlReadMode.ReadSchema);
                  }
               }
            }
         }

         return xmlReadMode;
      }

      /// <summary>
      /// Extension method for a dataset to define WriteXml method with encryption
      /// </summary>
      /// <param name="dataSet">The dataset</param>
      /// <param name="fileName">File name to read</param>
      /// <param name="userName">Username for encryption</param>
      /// <param name="password">Password for encryption</param>
      /// <param name="compress">Should the file be compressed</param>
      public static void WriteXml(this System.Data.DataSet dataSet, string fileName, string userName, string password, bool compress) {
         // Check the parameters
         if (dataSet == null
            || string.IsNullOrEmpty(userName)
            || string.IsNullOrEmpty(password)
            || string.IsNullOrEmpty(fileName)) {
            throw new System.ArgumentNullException("All arguments must be supplied.");
         }

         // Encrypt and save the dataset
         Cryptography.EncryptDataSet(dataSet, userName, password, fileName, compress);
      }

      /// <summary>
      /// Extension method for a dataset to define ReadXml method with decryption
      /// </summary>
      /// <param name="dataSet">The dataset</param>
      /// <param name="fileName">File name to read</param>
      /// <param name="userName">Username for decryption</param>
      /// <param name="password">Password for decryption</param>
      /// <param name="compressed">Is the file compressed</param>
      /// <returns>XmlReadMode used for reading</returns>
      public static System.Data.XmlReadMode ReadXml(this System.Data.DataSet dataSet, string fileName, string userName, string password, bool compressed) {
         // Check the parameters
         if (dataSet == null
            || string.IsNullOrEmpty(userName)
            || string.IsNullOrEmpty(password)
            || string.IsNullOrEmpty(fileName)) {
            throw new System.ArgumentNullException("All arguments must be supplied.");
         }

         // Decrypt the saved dataset
         return Cryptography.DecryptDataSet(dataSet, userName, password, fileName, compressed);
      }

   }
}
