using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SocialDataVisualizer
{
    public class TwitterRestApiManager
    {
        public string getdata()
        {
            // oauth application keys
            var oauth_token = "23617089-kJscZ82mGJ8B7Kybor2YQP5VXVL1vJbqPsJmAVIux";
            var oauth_token_secret = "uRRWDBNqWMsFohLfMFYDohZHaXV9qdRMWzNaQVLdSo";
            var oauth_consumer_key = "xjImTCo7LGAylDOQxWW6eQ";
            var oauth_consumer_secret = "oyRssjUXXQuyLfHDxNBIhG2q3jWLbJzoFCK9hPsI";

            // oauth implementation details
            var oauth_version = "1.0";
            var oauth_signature_method = "HMAC-SHA1";

            // unique request details
            var oauth_nonce = Convert.ToBase64String(
            new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            var timeSpan = DateTime.UtcNow
            - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            // message api details
            var resource_url = "https://api.twitter.com/1.1/trends/place.json?id=1";
            
            // create oauth signature
            var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
            "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}";

            var baseString = string.Format(baseFormat,
            oauth_consumer_key,
            oauth_nonce,
            oauth_signature_method,
            oauth_timestamp,
            oauth_token,
            oauth_version
            );

            baseString = string.Concat("GET&", Uri.EscapeDataString(resource_url), "&", Uri.EscapeDataString(baseString));

            var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
            "&", Uri.EscapeDataString(oauth_token_secret));

            string oauth_signature;
            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            {
                oauth_signature = Convert.ToBase64String(
                hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            // create the request header
            var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
            "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
            "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
            "oauth_version=\"{6}\"";

            var authHeader = string.Format(headerFormat,
            Uri.EscapeDataString(oauth_nonce),
            Uri.EscapeDataString(oauth_signature_method),
            Uri.EscapeDataString(oauth_timestamp),
            Uri.EscapeDataString(oauth_consumer_key),
            Uri.EscapeDataString(oauth_token),
            Uri.EscapeDataString(oauth_signature),
            Uri.EscapeDataString(oauth_version)
            );


            // make the request

            ServicePointManager.Expect100Continue = false;

            //var postBody = "screen_name=" + Uri.EscapeDataString(screen_name);//
            //resource_url += "?" + postBody;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
            request.Headers.Add("Authorization", authHeader);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";


            WebResponse response = request.GetResponse();
            string responseData = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseData;
        }


        

    }
}