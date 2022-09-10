using System.Text;
using System.Security.Cryptography;

namespace Encryption{
    // MD5加密类
    public class MD5{
        private static MD5? instance;

        private MD5(){}
        public static MD5 GetInstance()
        {
            if(instance ==null)
            {
                instance = new MD5();
            }
            return instance;
        }

        /// <summary>
        /// 将utf-8字符串转换为md5散列值
        /// </summary>
        /// <param name = "code">待转换的utf-8编码字符串</param>
        public string StringToMD5_UTF8(string code)
        {
            var lstData = Encoding.GetEncoding("utf-8").GetBytes(code);
            var lstHash = new MD5CryptoServiceProvider().ComputeHash(lstData);
            var result = new StringBuilder(32);
            for (int i = 0; i < lstHash.Length; i++)
            {
                result.Append(lstHash[i].ToString("x2").ToUpper());
            }
            return result.ToString();
        }

    }
}