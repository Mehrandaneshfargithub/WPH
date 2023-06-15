using System;
using System.Collections.Generic;
using System.Linq;
//using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WPH.Helper
{
    public class CheckLicence
    {
        public string CheckAccess(string encryptSerialKey)
        {
            try
            {
                var (Key, IVBase64) = InitSymmetricEncryptionKeyIV();
                string serialKey = Decrypt(encryptSerialKey, IVBase64, Key);

                string mainSerial = CreateSerial();
                string serial = "";
                string date = "";
                for (int i = 0; i < serialKey.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        serial += serialKey[i];
                    }
                    else
                    {
                        date += serialKey[i];
                    }
                }

                if (serial.Length > mainSerial.Length)
                {
                    mainSerial = mainSerial.PadLeft(serial.Length, 'A');
                }

                if (!mainSerial.Equals(serial))
                    return "KeyNotValid";

                date = date.Replace("A", "");
                char[] dateArray = date.ToCharArray();
                DateTime expireDate = DateTime.ParseExact($"{dateArray[0]}{dateArray[1]}/{dateArray[2]}{dateArray[3]}/{dateArray[4]}{dateArray[5]}{dateArray[6]}{dateArray[7]} 23:59", "dd/MM/yyyy HH:mm", null);

                if (DateTime.Now > expireDate)
                    return "DateExpired";

                long def = (long)(expireDate - DateTime.Now).TotalDays;
                return def.ToString();
            }
            catch
            {
                return "KeyNotValid";
            }
        }

        public string GetDate(string encryptSerialKey)
        {
            try
            {
                var (Key, IVBase64) = InitSymmetricEncryptionKeyIV();
                string serialKey = Decrypt(encryptSerialKey, IVBase64, Key);

                string mainSerial = CreateSerial();
                string serial = "";
                string date = "";
                for (int i = 0; i < serialKey.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        serial += serialKey[i];
                    }
                    else
                    {
                        date += serialKey[i];
                    }
                }

                if (serial.Length > mainSerial.Length)
                {
                    mainSerial = mainSerial.PadLeft(serial.Length, 'A');
                }

                if (!mainSerial.Equals(serial))
                    return "KeyNotValid";

                date = date.Replace("A", "");
                char[] dateArray = date.ToCharArray();

                return $"{dateArray[0]}{dateArray[1]}/{dateArray[2]}{dateArray[3]}/{dateArray[4]}{dateArray[5]}{dateArray[6]}{dateArray[7]}";
            }
            catch
            {
                return "";
            }
        }

        public string GetSerial()
        {
            var (Key, IVBase64) = InitSymmetricEncryptionKeyIV();

            var result = Encrypt(CreateSerial(), IVBase64, Key);
            return result;
        }

        public string GetComputerSerial()
        {
            string expand = "QlSOlUkzO74kziMppZBacsXusjJBOfzR6a6GYDUDWLvFVYNouFFsDegFFgOdtdsq4aj18UEQvGtgIEFuYv2r/wNfFBltBhN27U8LpPhplw3l9280U6yVszZtFTYME4uG";
            string processId = GetProcessorID();
            string motherBoardId = GetMotherBoardId();

            int padLength = Math.Abs(processId.Length - motherBoardId.Length);
            string pad = "";
            for (int i = 0; i < padLength; i++)
            {
                pad += expand[i];
            }

            if (processId.Length > motherBoardId.Length)
            {
                motherBoardId = pad + motherBoardId;
            }
            else
            {
                processId = pad + processId;
            }

            int length = processId.Length / 2;
            char[] serial = new char[length * 2];
            for (int i = 0; i < length; i++)
            {
                serial[i * 2] = processId[i];
                serial[(i * 2) + 1] = motherBoardId[i];
            }

            var result = new string(serial);
            return result;
        }

        private string CreateSerial()
        {
            string processId = GetProcessorID();
            string motherBoardId = GetMotherBoardId();

            if (processId.Length > motherBoardId.Length)
            {
                motherBoardId = motherBoardId.PadLeft(processId.Length, '0');
            }
            else
            {
                processId = processId.PadLeft(motherBoardId.Length, '0');
            }

            int length = processId.Length;
            char[] serial = new char[(length / 2) * 2];
            for (int i = 0; i < serial.Length; i++)
            {
                if (i % 2 == 0)
                {
                    serial[i] = processId[i];
                }
                else
                {
                    serial[i] = motherBoardId[i];
                }
            }

            var resutl = new string(serial);
            return resutl;
        }

        private string GetProcessorID()
        {
            //ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select ProcessorID From Win32_processor");
            //ManagementObjectCollection mbsList = mbs.Get();
            //string id = "";
            //foreach (ManagementObject mo in mbsList)
            //{
            //    id = mo["ProcessorID"].ToString();
            //}
            //return id;
            return null;
        }

        private string GetMotherBoardId()
        {
            //ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard");
            //ManagementObjectCollection moc = mos.Get();
            //string serial = "";
            //foreach (ManagementObject mo in moc)
            //{
            //    serial = (string)mo["SerialNumber"];
            //}
            //return serial;
            return null;
        }

        private (string Key, string IVBase64) InitSymmetricEncryptionKeyIV()
        {
            var key = "RpYUrq6iseYczkK9fjYruTHY1M7qP2qC6Y8FbQ4+52E=";
            var IVBase64 = "0T/j0PWSKHa68eOs42MkQw==";
            return (key, IVBase64);
        }

        private Aes CreateCipher(string keyBase64)
        {
            Aes cipher = Aes.Create();
            cipher.Mode = CipherMode.CBC;

            cipher.Padding = PaddingMode.ISO10126;
            cipher.Key = Convert.FromBase64String(keyBase64);

            return cipher;
        }

        private string Encrypt(string text, string IV, string key)
        {
            Aes cipher = CreateCipher(key);
            cipher.IV = Convert.FromBase64String(IV);

            ICryptoTransform cryptTransform = cipher.CreateEncryptor();
            byte[] plaintext = Encoding.UTF8.GetBytes(text);
            byte[] cipherText = cryptTransform.TransformFinalBlock(plaintext, 0, plaintext.Length);

            return Convert.ToBase64String(cipherText);
        }

        private string Decrypt(string encryptedText, string IV, string key)
        {
            Aes cipher = CreateCipher(key);
            cipher.IV = Convert.FromBase64String(IV);

            ICryptoTransform cryptTransform = cipher.CreateDecryptor();
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] plainBytes = cryptTransform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
