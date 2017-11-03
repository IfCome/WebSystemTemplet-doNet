using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace WebSystemTemplet.Utility
{
    public static class IpAddressHelper
    {
        public static string GetAreaByIp(string ip)
        {
            if (ip.IsNullOrWhiteSpace())
            {
                return "未知";
            }
            if (CacheHelper.GetCache(ip) != null)
            {
                return CacheHelper.GetCache(ip).ToString();
            }
            string result = DownLoadRemoteData("http://ip.taobao.com/service/getIpInfo.php?ip=" + ip);
            try
            {
                JavaScriptSerializer Jss = new JavaScriptSerializer();
                Dictionary<string, object> resultDic = (Dictionary<string, object>)Jss.DeserializeObject(result);
                if (resultDic.ContainsKey("code") && resultDic["code"].ToString() == "0")
                {
                    Dictionary<string, object> dataDic = (Dictionary<string, object>)resultDic["data"];
                    string area = string.Format("{0}-{1}-{2}", dataDic["country"], dataDic["region"], dataDic["city"]);

                    CacheHelper.SetCache(ip, area, new TimeSpan(1, 0, 0));   //1小时的缓存
                    return area;
                }
            }
            catch (Exception)
            {

            }
            return "未知";
        }

        /// <summary>
        /// 获取远程接口数据-字符串(GB2312)
        /// </summary>
        /// <param name="target">目标接口</param>
        /// <returns>字符结果集</returns>
        public static string DownLoadRemoteData(string target)
        {
            string responseData;
            try
            {
                HttpWebRequest Request = System.Net.WebRequest.Create(target) as HttpWebRequest;
                Request.Method = "Get";
                //设置超时时间
                Request.Timeout = 10000;
                Request.ReadWriteTimeout = 10000;
                //设置缓存
                RequestCachePolicy policy = new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable);
                Request.CachePolicy = policy;

                using (StreamReader responseReader = new StreamReader(Request.GetResponse().GetResponseStream(), Encoding.GetEncoding("gb2312")))
                {
                    responseData = responseReader.ReadToEnd();
                }
            }
            catch
            {
                responseData = string.Empty;
            }
            return responseData;
        }

        /// <summary>
        /// 获取远程接口数据-字符串(Utf8)
        /// </summary>
        /// <param name="target">目标接口</param>
        /// <returns>字符结果集</returns>
        public static string DownLoadRemoteUtf8Data(string target)
        {
            string responseData;
            try
            {
                HttpWebRequest Request = System.Net.WebRequest.Create(target) as HttpWebRequest;
                Request.Method = "Get";
                //设置超时时间
                Request.Timeout = 10000;
                Request.ReadWriteTimeout = 10000;
                //设置缓存
                RequestCachePolicy policy = new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable);
                Request.CachePolicy = policy;

                using (StreamReader responseReader = new StreamReader(Request.GetResponse().GetResponseStream(), Encoding.GetEncoding("utf-8")))
                {
                    responseData = responseReader.ReadToEnd();
                }
            }
            catch
            {
                responseData = string.Empty;
            }
            return responseData;
        }
    }

}
