using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace AutoTestLogic.Encryption {
    public static class PasswordProtect {

        /// <summary>
        /// 将字符串转换为MD5
        /// </summary>
        /// <param name="code">待转换字符串</param>
        /// <returns></returns>
        public static string StringToMD5_UTF8(this string code) {
            var lstData = Encoding.GetEncoding("utf-8").GetBytes(code);
            var lstHash = new MD5CryptoServiceProvider().ComputeHash(lstData);
            var result = new StringBuilder(32);
            for (int i = 0; i < lstHash.Length; i++) {
                result.Append(lstHash[i].ToString("x2").ToUpper());
            }
            return result.ToString();
        }
    }
}
