using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Threading.Tasks.Sources;

namespace ERP
{
    internal class Encryption
    {
        public static byte[] Key = new byte[32];
        public static byte[] IV = new byte[16];

        /*
         * 암호화를 위한 키를 생성한다
         * Key 와 IV 배열의 값은 다른 숫자로 대체 가능하다
         */

        public Encryption()
        {
            for (int i = 0; i < Key.Length; i++) Key[i] = 0;
            for (int i = 0; i < IV.Length; i++) IV[i] = 0;
        }

        /*
         * 입력된 비밀번호를 암호화
         */

        public static string EncryptString(string plainText)
        {
            var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;
            var encryptor = aes.CreateEncryptor();
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            return Convert.ToBase64String(encrypted);
        }

        /*
         * 암호화된 비밀번호를 복호화 (입력한 문자열로 복구)
         */

         public static string DecryptString(string cipherText, byte[] Key, byte[] IV)
        {
            var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;
            var decryptor = aes.CreateDecryptor();
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
