using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace MyLib.MyUtility
{
    /// <summary>
    /// Lớp chứa các hàm về mã hóa, giải mã
    /// </summary>
    public class MySecurity
    {
      

        // Define supported password characters divided into groups.
        // You can add (or remove) characters to (from) these groups.
        private static string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
        private static string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
        private static string PASSWORD_CHARS_NUMERIC = "0123456789";
        private static string PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";

        private static byte[] IV = { 121, 122, 123, 124, 125, 126, 127, 128 };
        
        private static TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
  
        private static bool FixKey3DES(string key, out byte[] result)
        {
            result = null;
            if (key.Length > 32 || key.Length == 0) return false;
            key = key.PadRight(32, 'x');
            try
            {
                result = Convert.FromBase64String(key);
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Dùng để mã hóa mật khẩu hay dữ liệu đầu vào bằng các ký tự phức tạp dựa vào ký hiệu riêng là biến "key" .
        /// </summary>
        public static string TripleDES_En(string input, string key)
        {
            string encrypted = "";
            if (input == null) return null;
            if (input == "") return "";
            byte[] keys;
            if (FixKey3DES(key, out keys) == false)
                throw new Exception("Invalid key format");
            des.Key = keys;
            byte[] Code = UTF8Encoding.Unicode.GetBytes(input);
            encrypted = Convert.ToBase64String(des.CreateEncryptor(keys, IV).TransformFinalBlock(Code, 0, Code.Length));
            return encrypted;
        }

        /// <summary>
        /// Lớp này dùng để giải mã .
        /// </summary>
        public static string TripleDES_De(string input, string key)
        {
            string decrypted = "";
            if (input == null) return null;
            if (input == "") return "";
            byte[] keys;
            if (FixKey3DES(key, out keys) == false)
                throw new Exception("Invalid key format");
            des.Key = keys;
            try
            {
                byte[] Code = Convert.FromBase64String(input);
                decrypted = UTF8Encoding.Unicode.GetString(des.CreateDecryptor(keys, IV).TransformFinalBlock(Code, 0, Code.Length));
            }
            catch
            {
                return null;
            }
            return decrypted;
        }

        /// <summary>
        /// Mã hóa một cách đơn giản dữ liệu
        /// </summary>
        /// <param name="Data">Chuỗi cần mã hóa</param>
        /// <returns>Chuỗi đã được mã hóa</returns>
        public static string EnCodeData(string Data)
        {
            try
            {
                byte[] encbuff = System.Text.Encoding.Unicode.GetBytes(Data);
                return "L" + Convert.ToBase64String(encbuff);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong quá trình EnCode dữ liệu", ex);
            }
        }
        /// <summary>
        /// Giả mã chỗi đã được mã hóa bằng hàm EnCodeDate
        /// </summary>
        /// <param name="Data">Chuỗi cần giản mã</param>
        /// <returns>Chuỗi đã được giải mã</returns>
        public static string DeCodeData(string Data)
        {
            try
            {
                Data = Data.Substring(1, Data.Length - 1);
                byte[] decbuff = Convert.FromBase64String(Data);
                return System.Text.Encoding.Unicode.GetString(decbuff);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong quá trình DeCode dữ liệu", ex);
            }
        }

        public static string DeCodeBase64(string Data)
        {
            try
            {
                byte[] decbuff = Convert.FromBase64String(Data);
                return System.Text.Encoding.Unicode.GetString(decbuff);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong quá trình DeCode dữ liệu", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static string EnCodeBase64_ASCII(string Data)
        {
            try
            {
                byte[] encbuff = System.Text.ASCIIEncoding.ASCII.GetBytes(Data);
                return Convert.ToBase64String(encbuff,Base64FormattingOptions.None);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong quá trình EnCode dữ liệu", ex);
            }
        }

        public static string DeCodeBase64_ASCII(string Data)
        {
            try
            {
                byte[] decbuff = Convert.FromBase64String(Data);
                return System.Text.ASCIIEncoding.ASCII.GetString(decbuff);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong quá trình EnCode dữ liệu", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static string EnCodeBase64(string Data)
        {
            try
            {
                byte[] encbuff = System.Text.Encoding.Unicode.GetBytes(Data);
                return Convert.ToBase64String(encbuff);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong quá trình EnCode dữ liệu", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public static string EnCodeBase64_GetMSISDN(string strCode)
        {
            try
            {
                byte[] encData_byte = new byte[strCode.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(strCode);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Encode" + e.Message);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public static string DeCodeBase64_GetMSISDN(string strCode)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(strCode);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode" + e.Message);
            }
        }
       
        /// <summary>
        /// Mã hóa dữ liệu theo thuật toán MD5
        /// </summary>
        /// <param name="Data">Chuỗi dữ liệu cần mã hóa</param>
        /// <returns>Trả về chuỗi dữ liệu đã được mã hóa</returns>
        public static string Encrypt_MD5(string Data)
        {
            try
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Data, "MD5");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Tao code bang ngay thang hien tai
        /// </summary>
        /// <returns></returns>
        public static string CreateCodeByDate()
        {
            return System.DateTime.Now.ToString("yyyyMMddHHmmssffffff");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FormatDate"></param>
        /// <returns></returns>
        public static string CreateCodeByDate(string FormatDate)
        {
            try
            {
                return System.DateTime.Now.ToString(FormatDate);
            }
            catch
            {
                return System.DateTime.Now.ToString("yyyyMMddHHmmssffffff");
            }
        }

        /// <summary>
        /// Tạo random code với số gồm 5 chữ số
        /// </summary>
        /// <returns></returns>
        public static string GenerateRandomCode()
        {
            Random random = new Random();
            return random.Next(99999).ToString("0####");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="CodePrefix"></param>
        /// <param name="LengthNumber"></param>
        /// <returns></returns>
        public static string CreateMaxCode(string Code, string CodePrefix,int LengthNumber)
        {
            string temp = Code.Replace(CodePrefix, "");

            int iCode = 0;
            if (int.TryParse(temp, out iCode))
            {
                return CodePrefix + CreateCode(LengthNumber, ++iCode);
            }
            else
            {
                return CodePrefix + CreateCode(LengthNumber, ++iCode);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iLenght"></param>
        /// <param name="iNumber"></param>
        /// <returns></returns>
        public static string CreateCode(int iLenght, int iNumber)
        {

            Random random = new Random();
            int Number_Rand = random.Next(iNumber);
            switch (iLenght)
            {
                case 1:
                    return Number_Rand.ToString();
                case 2:
                    return Number_Rand.ToString("0#");
                case 3:
                    return Number_Rand.ToString("0##");
                case 4:
                    return Number_Rand.ToString("0###");
                case 5:
                    return Number_Rand.ToString("0####");
                case 6:
                    return Number_Rand.ToString("0#####");
                case 7:
                    return Number_Rand.ToString("0######");
                case 8:
                    return Number_Rand.ToString("0#######");
                case 9:
                    return Number_Rand.ToString("0########");
                default:
                    return Number_Rand.ToString("0####");
            }

        }

        /// <summary>
        /// Tạo code theo số nguyên với chiều dài truyền vào
        /// </summary>
        /// <param name="iLenght">Chiều dài số cần lấy</param>
        /// <returns></returns>
        public static string CreateCode(int iLenght)
        {

            Random random = new Random();
            switch (iLenght)
            {
                case 1:
                    return random.Next(9).ToString();
                case 2:
                    return random.Next(99).ToString("0#");
                case 3:
                    return random.Next(999).ToString("0##");
                case 4:
                    return random.Next(9999).ToString("0###");
                case 5:
                    return random.Next(99999).ToString("0####");
                case 6:
                    return random.Next(999999).ToString("0#####");
                case 7:
                    return random.Next(9999999).ToString("0######");
                case 8:
                    return random.Next(99999999).ToString("0#######");
                case 9:
                    return random.Next(999999999).ToString("0########");
                default:
                    return random.Next(9).ToString("0####");
            }

        }

        /// <summary>
        /// Tạo password có chiều dài truyền vào
        /// </summary>
        /// <param name="length">Chiều dài của chuỗi password</param>
        /// <returns></returns>
        public static string GeneratePassword(int length)
        {
            return GeneratePassword(length, length);
        }
        
        public static string GeneratePassword(int minLength, int maxLength)
        {
            // Make sure that input parameters are valid.
            if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
                return null;

            // Create a local array containing supported password characters
            // grouped by types. You can remove character groups from this
            // array, but doing so will weaken the password strength.
            char[][] charGroups = new char[][] 
            {
                PASSWORD_CHARS_LCASE.ToCharArray(),
                PASSWORD_CHARS_UCASE.ToCharArray(),
                PASSWORD_CHARS_NUMERIC.ToCharArray(),
                PASSWORD_CHARS_SPECIAL.ToCharArray() 
            };

            // Use this array to track the number of unused characters in each
            // character group.
            int[] charsLeftInGroup = new int[charGroups.Length];
            // Initially, all characters in each group are not used.
            for (int i = 0; i < charsLeftInGroup.Length; i++)
                charsLeftInGroup[i] = charGroups[i].Length;
            // Use this array to track (iterate through) unused character groups. 
            int[] leftGroupsOrder = new int[charGroups.Length];
            // Initially, all character groups are not used.
            for (int i = 0; i < leftGroupsOrder.Length; i++)
                leftGroupsOrder[i] = i;
            byte[] randomBytes = new byte[4];
            // Generate 4 random bytes.
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);
            // Convert 4 bytes into a 32-bit integer value.
            int seed = (randomBytes[0] & 0x7f) << 24 |
                        randomBytes[1] << 16 |
                        randomBytes[2] << 8 |
                        randomBytes[3];
            // Now, this is real randomization.
            Random random = new Random(seed);
            // This array will hold password characters.
            char[] password = null;
            if (minLength < maxLength)
                password = new char[random.Next(minLength, maxLength + 1)];
            else
                password = new char[minLength];
            int nextCharIdx;
            int nextGroupIdx;
            int nextLeftGroupsOrderIdx;
            int lastCharIdx;
            int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
            for (int i = 0; i < password.Length; i++)
            {
                if (lastLeftGroupsOrderIdx == 0)
                    nextLeftGroupsOrderIdx = 0;
                else
                    nextLeftGroupsOrderIdx = random.Next(0,
                                                         lastLeftGroupsOrderIdx);
                nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];
                lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;
                if (lastCharIdx == 0)
                    nextCharIdx = 0;
                else
                    nextCharIdx = random.Next(0, lastCharIdx + 1);
                password[i] = charGroups[nextGroupIdx][nextCharIdx];

                if (lastCharIdx == 0)
                    charsLeftInGroup[nextGroupIdx] =
                                              charGroups[nextGroupIdx].Length;
                else
                {
                    if (lastCharIdx != nextCharIdx)
                    {
                        char temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] =
                                    charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }
                    charsLeftInGroup[nextGroupIdx]--;
                }
                if (lastLeftGroupsOrderIdx == 0)
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                else
                {
                    if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                    {
                        int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] =
                                    leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }

                    lastLeftGroupsOrderIdx--;
                }
            }
            return new string(password);
        }

        /// <summary>
        /// Tạo một chuổi code ngẫu nhiên gồm chữ và số
        /// </summary>
        /// <param name="length">Độ dài của chuỗi cần tạo</param>
        /// <returns></returns>
        public static string GenerateString(int length)
        {
            // Create a local array containing supported password characters
            // grouped by types. You can remove character groups from this
            // array, but doing so will weaken the password strength.
            char[][] charGroups = new char[][] 
            {
                PASSWORD_CHARS_UCASE.ToCharArray(),
                PASSWORD_CHARS_NUMERIC.ToCharArray(),
            };

            // Use this array to track the number of unused characters in each
            // character group.
            int[] charsLeftInGroup = new int[charGroups.Length];
            // Initially, all characters in each group are not used.
            for (int i = 0; i < charsLeftInGroup.Length; i++)
                charsLeftInGroup[i] = charGroups[i].Length;
            // Use this array to track (iterate through) unused character groups. 
            int[] leftGroupsOrder = new int[charGroups.Length];
            // Initially, all character groups are not used.
            for (int i = 0; i < leftGroupsOrder.Length; i++)
                leftGroupsOrder[i] = i;
            byte[] randomBytes = new byte[4];
            // Generate 4 random bytes.
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);
            // Convert 4 bytes into a 32-bit integer value.
            int seed = (randomBytes[0] & 0x7f) << 24 |
                        randomBytes[1] << 16 |
                        randomBytes[2] << 8 |
                        randomBytes[3];
            // Now, this is real randomization.
            Random random = new Random(seed);
            // This array will hold password characters.
            char[] password = null;
            password = new char[length];
            int nextCharIdx;
            int nextGroupIdx;
            int nextLeftGroupsOrderIdx;
            int lastCharIdx;
            int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
            for (int i = 0; i < password.Length; i++)
            {
                if (lastLeftGroupsOrderIdx == 0)
                {
                    nextLeftGroupsOrderIdx = 0;
                }
                else
                {
                    if (lastLeftGroupsOrderIdx > 2)
                    {
                        nextLeftGroupsOrderIdx = random.Next(0, lastLeftGroupsOrderIdx);
                    }
                    else
                    {
                        nextLeftGroupsOrderIdx = random.Next(0, 9)%2;
                    }
                }
                nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];
                lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                if (lastCharIdx == 0)
                {
                    nextCharIdx = 0;
                }
                else
                {
                    nextCharIdx = random.Next(0, lastCharIdx + 1);
                }
                password[i] = charGroups[nextGroupIdx][nextCharIdx];

                if (lastCharIdx == 0)
                {
                    charsLeftInGroup[nextGroupIdx] = charGroups[nextGroupIdx].Length;
                }
                else
                {
                    if (lastCharIdx != nextCharIdx)
                    {
                        char temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] = charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }
                    charsLeftInGroup[nextGroupIdx]--;
                }
                if (lastLeftGroupsOrderIdx == 0)
                {
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                }
                else
                {
                    if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                    {
                        int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] =leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }

                    lastLeftGroupsOrderIdx--;
                }
            }
            return new string(password);
        }

        /// <summary>
        /// tao mat khau voi toan la so
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string GeneratePassword_Number(int minLength, int maxLength)
        {
            // Make sure that input parameters are valid.
            if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
                return null;

            // Create a local array containing supported password characters
            // grouped by types. You can remove character groups from this
            // array, but doing so will weaken the password strength.
            char[][] charGroups = new char[][] 
            {
                PASSWORD_CHARS_NUMERIC.ToCharArray()
            };

            // Use this array to track the number of unused characters in each
            // character group.
            int[] charsLeftInGroup = new int[charGroups.Length];
            // Initially, all characters in each group are not used.
            for (int i = 0; i < charsLeftInGroup.Length; i++)
                charsLeftInGroup[i] = charGroups[i].Length;
            // Use this array to track (iterate through) unused character groups. 
            int[] leftGroupsOrder = new int[charGroups.Length];
            // Initially, all character groups are not used.
            for (int i = 0; i < leftGroupsOrder.Length; i++)
                leftGroupsOrder[i] = i;
            byte[] randomBytes = new byte[4];
            // Generate 4 random bytes.
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);
            // Convert 4 bytes into a 32-bit integer value.
            int seed = (randomBytes[0] & 0x7f) << 24 |
                        randomBytes[1] << 16 |
                        randomBytes[2] << 8 |
                        randomBytes[3];
            // Now, this is real randomization.
            Random random = new Random(seed);
            // This array will hold password characters.
            char[] password = null;
            if (minLength < maxLength)
                password = new char[random.Next(minLength, maxLength + 1)];
            else
                password = new char[minLength];
            int nextCharIdx;
            int nextGroupIdx;
            int nextLeftGroupsOrderIdx;
            int lastCharIdx;
            int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
            for (int i = 0; i < password.Length; i++)
            {
                if (lastLeftGroupsOrderIdx == 0)
                    nextLeftGroupsOrderIdx = 0;
                else
                    nextLeftGroupsOrderIdx = random.Next(0,
                                                         lastLeftGroupsOrderIdx);
                nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];
                lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;
                if (lastCharIdx == 0)
                    nextCharIdx = 0;
                else
                    nextCharIdx = random.Next(0, lastCharIdx + 1);
                password[i] = charGroups[nextGroupIdx][nextCharIdx];

                if (lastCharIdx == 0)
                    charsLeftInGroup[nextGroupIdx] =
                                              charGroups[nextGroupIdx].Length;
                else
                {
                    if (lastCharIdx != nextCharIdx)
                    {
                        char temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] =
                                    charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }
                    charsLeftInGroup[nextGroupIdx]--;
                }
                if (lastLeftGroupsOrderIdx == 0)
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                else
                {
                    if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                    {
                        int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] =
                                    leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }

                    lastLeftGroupsOrderIdx--;
                }
            }
            return new string(password);
        }

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="PlainText">Text to be encrypted</param>
        /// <param name="Password">Password to encrypt with</param>
        /// <param name="Salt">Salt to encrypt with</param>
        /// <param name="HashAlgorithm">Can be either SHA1 or MD5</param>
        /// <param name="PasswordIterations">Number of iterations to do</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">Can be 128, 192, or 256</param>
        /// <returns>An encrypted string</returns>
        public static string AESEncrypt(string PlainText, string Password, string Salt, string HashAlgorithm, int PasswordIterations, string InitialVector, int KeySize)
        {
            try
            {
                byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
                byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
                byte[] PlainTextBytes = Encoding.UTF8.GetBytes(PlainText);
                PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations);
                byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
                RijndaelManaged SymmetricKey = new RijndaelManaged();
                SymmetricKey.Mode = CipherMode.CBC;
                byte[] CipherTextBytes = null;
                using (ICryptoTransform Encryptor = SymmetricKey.CreateEncryptor(KeyBytes, InitialVectorBytes))
                {
                    using (MemoryStream MemStream = new MemoryStream())
                    {
                        using (CryptoStream CryptoStream = new CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write))
                        {
                            CryptoStream.Write(PlainTextBytes, 0, PlainTextBytes.Length);
                            CryptoStream.FlushFinalBlock();
                            CipherTextBytes = MemStream.ToArray();
                            MemStream.Close();
                            CryptoStream.Close();
                        }
                    }
                }
                return Convert.ToBase64String(CipherTextBytes);
            }
            catch (Exception a)
            {
                throw a;
            }
        }

        /// <summary>
        /// Decrypts a string
        /// </summary>
        /// <param name="CipherText">Text to be decrypted</param>
        /// <param name="Password">Password to decrypt with</param>
        /// <param name="Salt">Salt to decrypt with</param>
        /// <param name="HashAlgorithm">Can be either SHA1 or MD5</param>
        /// <param name="PasswordIterations">Number of iterations to do</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">Can be 128, 192, or 256</param>
        /// <returns>A decrypted string</returns>
        public static string AESDecrypt(string CipherText, string Password, string Salt, string HashAlgorithm, int PasswordIterations, string InitialVector, int KeySize)
        {
            try
            {
                byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
                byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
                byte[] CipherTextBytes = Convert.FromBase64String(CipherText);
                PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations);
                byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
                RijndaelManaged SymmetricKey = new RijndaelManaged();
                SymmetricKey.Mode = CipherMode.CBC;
                byte[] PlainTextBytes = new byte[CipherTextBytes.Length];
                int ByteCount = 0;
                using (ICryptoTransform Decryptor = SymmetricKey.CreateDecryptor(KeyBytes, InitialVectorBytes))
                {
                    using (MemoryStream MemStream = new MemoryStream(CipherTextBytes))
                    {
                        using (CryptoStream CryptoStream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read))
                        {

                            ByteCount = CryptoStream.Read(PlainTextBytes, 0, PlainTextBytes.Length);
                            MemStream.Close();
                            CryptoStream.Close();
                        }
                    }
                }
                return Encoding.UTF8.GetString(PlainTextBytes, 0, ByteCount);
            }
            catch (Exception a)
            {
                throw a;
            }
        }
        
        public static string AESEncrypt_Simple(string Content, string Password)
        {
            try
            {
                string key="FRsgyQERTdfgDFRS";
                return AESEncrypt(Content, Password, "IcOm", "SHA1", 2, key, 128);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string AESDecrypt_Simple(string EncryptContent, string Password)
        {
            string key = "FRsgyQERTdfgDFRS";
            return AESDecrypt(EncryptContent, Password, "IcOm", "SHA1", 2, key, 128);
        }

        public static string EnCodeURL(string URL)
        {
            return HttpUtility.UrlEncode(URL);
        }

        public static string SHA1_Encrypt(string Content)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(Content, "SHA1");
        }

        /// <summary>
        /// Tạo 1 chuỗi ngẫu nhiên tên file
        /// </summary>
        /// <returns></returns>
        public static string GenerateFileName(int Lenght)
        {
            try
            {
                string FileName = string.Empty;
                string chars = "12346789abcdefghjkmnpqrtuvwxyz";

                // create random generator
                Random rnd = new Random();

                while (FileName.Length < Lenght)
                {
                    FileName += chars.Substring(rnd.Next(chars.Length), 1);
                }
                return FileName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

     
        /// <summary>
        /// Lấy số fibonacci theo cách đệ quy
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int GetFibonacci(int n)
        {
            if (n == 0 || n == 1 || n == 2)
                return n;

            return GetFibonacci(n - 1) + GetFibonacci(n - 2);
        }


        public class AES
        {
            /*With Java
             * -----------------------------------
	            private final String characterEncoding = "UTF-8";
	            private final String cipherTransformation = "AES/CBC/PKCS5Padding";
	            private final String aesEncryptionAlgorithm = "AES";

	            public byte[] decrypt(byte[] cipherText, byte[] key, byte[] initialVector) throws NoSuchAlgorithmException,
			            NoSuchPaddingException, InvalidKeyException, InvalidAlgorithmParameterException, IllegalBlockSizeException,
			            BadPaddingException
	            {
		            Cipher cipher = Cipher.getInstance(cipherTransformation);
		            SecretKeySpec secretKeySpecy = new SecretKeySpec(key, aesEncryptionAlgorithm);
		            IvParameterSpec ivParameterSpec = new IvParameterSpec(initialVector);
		            cipher.init(Cipher.DECRYPT_MODE, secretKeySpecy, ivParameterSpec);
		            cipherText = cipher.doFinal(cipherText);
		            return cipherText;
	            }

	            public byte[] encrypt(byte[] plainText, byte[] key, byte[] initialVector) throws NoSuchAlgorithmException,
			            NoSuchPaddingException, InvalidKeyException, InvalidAlgorithmParameterException, IllegalBlockSizeException,
			            BadPaddingException
	            {
		            Cipher cipher = Cipher.getInstance(cipherTransformation);
		            SecretKeySpec secretKeySpec = new SecretKeySpec(key, aesEncryptionAlgorithm);
		            IvParameterSpec ivParameterSpec = new IvParameterSpec(initialVector);
		            cipher.init(Cipher.ENCRYPT_MODE, secretKeySpec, ivParameterSpec);
		            plainText = cipher.doFinal(plainText);
		            return plainText;
	            }

	            private byte[] getKeyBytes(String key) throws UnsupportedEncodingException
	            {
		            byte[] keyBytes = new byte[16];
		            byte[] parameterKeyBytes = key.getBytes(characterEncoding);
		            System.arraycopy(parameterKeyBytes, 0, keyBytes, 0, Math.min(parameterKeyBytes.length, keyBytes.length));
		            return keyBytes;
	            }

	            // / <summary>
	            // / Encrypts plaintext using AES 128bit key and a Chain Block Cipher and
	            // returns a base64 encoded string
	            // / </summary>
	            // / <param name="plainText">Plain text to encrypt</param>
	            // / <param name="key">Secret key</param>
	            // / <returns>Base64 encoded string</returns>
	            public String encrypt(String plainText, String key) throws UnsupportedEncodingException, InvalidKeyException,
			            NoSuchAlgorithmException, NoSuchPaddingException, InvalidAlgorithmParameterException,
			            IllegalBlockSizeException, BadPaddingException
	            {
		            byte[] plainTextbytes = plainText.getBytes(characterEncoding);
		            byte[] keyBytes = getKeyBytes(key);
		            //return Base64.encodeToString(encrypt(plainTextbytes, keyBytes, keyBytes), Base64.DEFAULT);
		
		            String S = javax.xml.bind.DatatypeConverter.printBase64Binary(encrypt(plainTextbytes, keyBytes, keyBytes));
		
		            BASE64Encoder mEncode = new BASE64Encoder();
		            return mEncode.encode(encrypt(plainTextbytes, keyBytes, keyBytes));
	            }

	            // / <summary>
	            // / Decrypts a base64 encoded string using the given key (AES 128bit key
	            // and a Chain Block Cipher)
	            // / </summary>
	            // / <param name="encryptedText">Base64 Encoded String</param>
	            // / <param name="key">Secret Key</param>
	            // / <returns>Decrypted String</returns>
	            public String decrypt(String encryptedText, String key) throws KeyException, GeneralSecurityException,
			            GeneralSecurityException, InvalidAlgorithmParameterException, IllegalBlockSizeException,
			            BadPaddingException, IOException
	            {
		            //byte[] cipheredBytes = Base64.decode(encryptedText, Base64.DEFAULT);
		            BASE64Decoder mDecoder = new BASE64Decoder();
		
		            byte[] cipheredBytes = mDecoder.decodeBuffer(encryptedText);
		            byte[] keyBytes = getKeyBytes(key);
		            return new String(decrypt(cipheredBytes, keyBytes, keyBytes), characterEncoding);
	            }
             * --------------------------------------
             */

            public static RijndaelManaged GetRijndaelManaged(String secretKey)
            {
                var keyBytes = new byte[16];
                var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
                Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
                return new RijndaelManaged
                {
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7,
                    KeySize = 128,
                    BlockSize = 128,
                    Key = keyBytes,
                    IV = keyBytes
                };
            }

            public static byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
            {
                return rijndaelManaged.CreateEncryptor()
                    .TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            }

            public static byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
            {
                return rijndaelManaged.CreateDecryptor()
                    .TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            }

            /// <summary>
            /// Encrypts plaintext using AES 128bit key and a Chain Block Cipher and returns a base64 encoded string
            /// </summary>
            /// <param name="plainText">Plain text to encrypt</param>
            /// <param name="key">Secret key</param>
            /// <returns>Base64 encoded string</returns>
            public static String Encrypt(String plainText, String key)
            {
                var plainBytes = Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged(key)));
            }

            /// <summary>
            /// Decrypts a base64 encoded string using the given key (AES 128bit key and a Chain Block Cipher)
            /// </summary>
            /// <param name="encryptedText">Base64 Encoded String</param>
            /// <param name="key">Secret Key</param>
            /// <returns>Decrypted String</returns>
            public static String Decrypt(String encryptedText, String key)
            {
                var encryptedBytes = Convert.FromBase64String(encryptedText);
                return Encoding.UTF8.GetString(Decrypt(encryptedBytes, GetRijndaelManaged(key)));
            }
        }
    }



   
}
