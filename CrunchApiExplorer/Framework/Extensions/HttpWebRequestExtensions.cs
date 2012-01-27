using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CrunchApiExplorer.Framework.Extensions
{
    public static class HttpWebRequestExtensions
    {
        public static void SetRequestBody(this HttpWebRequest webRequest, string body, string contentType)
        {
            var data = Encoding.ASCII.GetBytes(body);

            webRequest.ContentLength = data.Length;
            webRequest.ContentType = contentType;

            using (var stream = webRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
        }
    }
}
