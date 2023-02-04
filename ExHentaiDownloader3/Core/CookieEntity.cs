using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.Core
{
    [DataContract]
    public class CookieEntity
    {
        [DataMember]
        public string domain { get; set; }

        [DataMember]
        public string expirationDate { get; set; }

        [DataMember]
        public string hostOnly { get; set; }

        [DataMember]
        public string httpOnly { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string path { get; set; }

        [DataMember]
        public string secure { get; set; }

        [DataMember]
        public string session { get; set; }

        [DataMember]
        public string storeId { get; set; }

        [DataMember]
        public string value { get; set; }

        [DataMember]
        public string id { get; set; }

        static public CookieCollection GetCookieCollection(CookieEntity[] cookieEntitys)
        {
            CookieCollection cookieCollection = new CookieCollection();
            foreach (CookieEntity c in cookieEntitys)
            {
                Cookie cookie = new()
                {
                    Domain = c.domain,
                    Expires = GetTime(c.expirationDate),
                    HttpOnly = c.httpOnly.ToLower().Equals("true"),
                    Name = c.name,
                    Path = c.path,
                    Secure = c.secure.ToLower().Equals("true"),
                    Value = c.value
                };

                cookieCollection.Add(cookie);
            }
            return cookieCollection;
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name=”timeStamp”></param>
        /// <returns></returns>
        static private DateTime GetTime(string timeStamp)
        {
            double dTime = double.Parse(timeStamp);
            UInt32 uiStamp = (UInt32)dTime;
            DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(uiStamp);
            return dt;
        }
    }
}
