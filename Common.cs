using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;

namespace WpfQuery
{
    public class Common
    {
        //通用单个匹配
        public static string MatchURL(string ma, string start, string end)
        {
            Regex regex = new Regex("(?<=(" + start + "))[.\\s\\S]*?(?=(" + end + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            Match mc = regex.Match(ma);
            return mc.ToString();
        }

        //通用多个匹配
        public static MatchCollection MatchURLs(string ma, string start, string end)
        {
            Regex regex = new Regex("(?<=(" + start + "))[.\\s\\S]*?(?=(" + end + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection mc = regex.Matches(ma);
            return mc;
        }

        //从html中提取纯文本
        public static string StripHT(string strHtml)
        {
            strHtml = strHtml.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ");
            Regex regex = new Regex("<.+?>", RegexOptions.IgnoreCase);
            string strOutput = regex.Replace(strHtml, "");//替换掉"<"和">"之间的内容
            strOutput = strOutput.Replace("<", "");
            strOutput = strOutput.Replace(">", "");
            strOutput = strOutput.Replace("&nbsp;", "");
            return strOutput;
        }


        //规则不行
        public static bool isurl(string url)
        {
            Regex r = new Regex(@"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            if (r.Match(url).Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool isurl2(string url)
        {
            Regex r = new Regex(@"^((https|http|ftp|rtsp|mms)?:\/\/)?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?(([0-9]{1,3}\.){3}[0-9]{1,3}|([0-9a-z_!~*'()-]+\.)*([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\.[a-z]{2,6})(:[0-9]{1,4})?((\/?)|(\/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+\/?)$");
            if (r.Match(url).Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool compareString(string murl, string url)
        {
            url = url.Replace("http://", "");
            int n = murl.Length < url.Length ? murl.Length : url.Length;
            int m = 0;
            for (int i = 0; i < n; i++)
            {
                if (murl[i] == url[i])
                    m++;
                else
                    break;
            }
            if (m >= 4) //如：g.cn
                return true;
            else
                return false;
        }

        public static string BDSegment(string totalLines, int rank)
        {
            string str = " id=\"" + rank.ToString() + "\"";
            string str_end = "id=\"page\"";
            string str_next = " id=\"" + (rank + 1).ToString() + "\"";

            if (rank % 10 != 0)
            {
                if (!totalLines.Contains(str))
                {
                    return "";
                }

                if (totalLines.Contains(str) && !totalLines.Contains(str_next))
                {
                    return MatchURL(totalLines, str, str_end);
                }

                if (totalLines.Contains(str) && totalLines.Contains(str_next))
                {
                    return MatchURL(totalLines, str, str_next);
                }

            }
            else
            {
                return MatchURL(totalLines, str, str_end);
            }

            return "";
        }

        public static int RandomInt(int iDown, int iUp)
        {
            Random ro = new Random();
            return ro.Next(iDown, iUp);
        }

        public static string domain(string urlstr)
        {
            string url = DealURL(urlstr);
            Regex reg = new Regex(@"^(\w*://)?\w*\.?(\w*\.(com.cn|net.cn|org.cn|gov.cn|org|net|gov|cn|com|mobi|me|info|name|biz|cc|tv|asia|hk|网络|公司|中国)).*$");
            Match mat = reg.Match(url);
            return mat.Groups[2].Value;
        }

        public static string DealURL(string url)
        {
            string newurl = "";
            if (url.Split('.').Length == 2)
            {
                return "w." + url;
            }
            else if (url.Split('.').Length == 3)
            {
                return url;
            }
            else if (url.Split('.').Length >= 4)
            {
                newurl = url.Replace(".com.cn", "").Replace(".net.cn", "").Replace(".org.cn", "").Replace(".gov.cn", "").Replace(".org", "").Replace(".net", "").Replace(".gov", "").Replace(".cn", "").Replace(".com", "").Replace(".mobi", "").Replace(".me", "").Replace(".info", "").Replace(".name", "").Replace(".biz", "").Replace(".cc", "").Replace(".tv", "").Replace(".asia", "").Replace(".hk", "").Replace(".网络", "").Replace(".公司", "").Replace(".中国", "");
                string[] str = newurl.Split('.');
                string substr = "";
                for (int i = 0; i < str.Length - 1; i++)
                {
                    substr += str[i] + ".";
                }
                return url.Replace(substr, "w.");
            }

            return String.Empty;
        }

        /// <summary>
        /// 判断某软件是否安装
        /// </summary>
        /// <returns></returns>
        public static bool checkLogParser()
        {
            Microsoft.Win32.RegistryKey uninstallNode = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            foreach (string subKeyName in uninstallNode.GetSubKeyNames())
            {
                Microsoft.Win32.RegistryKey subKey = uninstallNode.OpenSubKey(subKeyName);
                object displayName = subKey.GetValue("DisplayName");
                if (displayName != null)
                {
                    //if (displayName.ToString().Contains("Adobe Reader"))
                    if (displayName.ToString().Contains("Log Parser"))
                    {
                        return true;
                    }
                }
            }
            return false; 
        }

        //判断是否乱码
        public static bool isLuan(string txt)
        {
            var bytes = Encoding.UTF8.GetBytes(txt);
            //239 191 189
            for (var i = 0; i < bytes.Length; i++)
            {
                if (i < bytes.Length - 3)
                    if (bytes[i] == 239 && bytes[i + 1] == 191 && bytes[i + 2] == 189)
                    {
                        return true;
                    }
            }
            return false;
        }

        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.Default.GetBytes(str);//System.Text.Encoding.UTF8.GetBytes(str); //默认是
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }


        //默认密钥向量
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串 </returns>
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));//转换为字节
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();//实例化数据加密标准
                MemoryStream mStream = new MemoryStream();//实例化内存流
                //将数据流链接到加密转换的流
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }


        //调用
        //string EncryptStr = EncryptDESString.EncryptDES("aaaaaaaaaa", "ssssssss");  //返回加密后的字符串
        //string DecryptStr = EncryptDESString.DecryptDES(EncryptStr, "ssssssss");//解密字符串

    }
}
