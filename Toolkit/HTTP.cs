using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using System.Collections.Specialized;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Reflection;
using System.Net.Configuration;
using System.Text.RegularExpressions;

namespace Toolkit
{

    public static class HTTP
    {
        public static CookieContainer current_cookies = null;

        public static HttpWebResponse GET(String url, String content_type, bool cookies, String referer, NameValueCollection headers)
        {
            HttpWebResponse response = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                // ignore SSL errors...
                request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                request.Method = "GET";
                request.ContentType = content_type;

                // The following fixes a response code of 500 in the RMS Auth/Login endpoint
                request.Accept = "*/*";

                request.Timeout = 10 * 60 * 1000;
                request.ReadWriteTimeout = 10 * 60 * 1000;

                if (referer != null)

                    request.Referer = referer;

                if (headers != null)

                    request.Headers.Add(headers);

                if (cookies)
                {
                    if (current_cookies == null)

                        current_cookies = new CookieContainer();    //create cookie container

                    request.CookieContainer = current_cookies;      //set container for HttpWebRequest 
                }

                response = (HttpWebResponse)request.GetResponse();

                foreach (Cookie cook in response.Cookies)
                {
                    Console.WriteLine("Domain: {0}, Name: {1}, value: {2}", cook.Domain, cook.Name, cook.Value);
                }
            }
            catch (WebException wex)
            {
                return (HttpWebResponse)wex.Response;
            }

            return response;
        }




        public static HttpWebResponse POST(String url, String post_data, String content_type, String referer, NameValueCollection headers, bool cookies, double timeout)
        {
            HttpWebResponse response = null;

            // Enable UseUnsafeHeaderParsing
            //if (!ToggleAllowUnsafeHeaderParsing(true))
            //{
            //    // Couldn't set flag. Log the fact, throw an exception or whatever.
            //}

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                // ignore SSL errors...
                request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                request.Method = "POST";
                request.ContentType = content_type;

                // The following fixes a response code of 500 in the RMS Auth/Login endpoint
                request.Accept = "*/*";
                //                request.Accept = "text/html, application/xhtml+xml, */*";
                //request.ServicePoint.Expect100Continue = false;

                request.Timeout = Convert.ToInt32(timeout * 1000);
                request.ReadWriteTimeout = Convert.ToInt32(timeout * 1000);

                if (referer != null)

                    request.Referer = referer;

                if (headers != null)

                    request.Headers.Add(headers);

                if (cookies)
                {
                    if (current_cookies == null)

                        current_cookies = new CookieContainer();    //create cookie container

                    request.CookieContainer = current_cookies;      //set container for HttpWebRequest 
                }

                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(post_data); //write your request payload
                }

                response = (HttpWebResponse)request.GetResponse();

                foreach (Cookie cook in response.Cookies)
                {
                    Console.WriteLine("Domain: {0}, Name: {1}, value: {2}", cook.Domain, cook.Name, cook.Value);
                }
            }
            catch (WebException wex)
            {
                return (HttpWebResponse)wex.Response;
            }

            return response;
        }

        public static HttpWebResponse POSTMultipart(String url, NameValueCollection nvc, NameValueCollection file_nvc, bool cookies)
        {
            String responseString = "";
            HttpWebResponse response = null;

            try
            {
                string boundary = "---------------------------" + System.DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                // ignore SSL errors...
                request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                request.Method = "POST";
                request.ContentType = "multipart/form-data; boundary=" + boundary;

                if (cookies)
                {
                    if (current_cookies == null)

                        current_cookies = new CookieContainer();    //create cookie container

                    request.CookieContainer = current_cookies;      //set container for HttpWebRequest 
                }

                using (Stream writer = request.GetRequestStream())
                {
                    foreach (string key in nvc.Keys)
                    {
                        writer.Write(boundarybytes, 0, boundarybytes.Length);
                        string formitem = string.Format("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}", key, nvc[key]);
                        byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                        writer.Write(formitembytes, 0, formitembytes.Length);
                    }
                    //writer.Write(boundarybytes, 0, boundarybytes.Length);
                    
                    foreach (string key in file_nvc.Keys)
                    {
                        writer.Write(boundarybytes, 0, boundarybytes.Length);

                        if (file_nvc[key].Contains(".zip"))
                        {
                            byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes("Content-Disposition: form-data; name=\"" + key + "\"; filename=\"dummy.zip\"\r\nContent-Type: application/x-zip-compressed\r\n\r\n");
                            writer.Write(formitembytes, 0, formitembytes.Length);
                        }
                        else
                        if (file_nvc[key].Contains(".jpg") || file_nvc[key].Contains(".jpeg"))
                        {
                            byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes("Content-Disposition: form-data; name=\"" + key + "\"; filename=\"" + System.IO.Path.GetFileName(file_nvc[key]) + "\"\r\nContent-Type: image/jpeg\r\n\r\n");
                            writer.Write(formitembytes, 0, formitembytes.Length);
                        }
                        else
                        if (file_nvc[key].Contains(".png"))
                        {
                            byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes("Content-Disposition: form-data; name=\"" + key + "\"; filename=\"" + System.IO.Path.GetFileName(file_nvc[key]) + "\"\r\nContent-Type: image/png\r\n\r\n");
                            writer.Write(formitembytes, 0, formitembytes.Length);
                        }
                        else
                        if (file_nvc[key].Contains(".ico"))
                        {
                            byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes("Content-Disposition: form-data; name=\"" + key + "\"; filename=\"" + System.IO.Path.GetFileName(file_nvc[key]) + "\"\r\nContent-Type: image/x-icon\r\n\r\n");
                            writer.Write(formitembytes, 0, formitembytes.Length);
                        }
                        else
                        if (file_nvc[key].Contains(".xlsx"))
                        {
                            byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes("Content-Disposition: form-data; name=\"" + key + "\"; filename=\"" + System.IO.Path.GetFileName(file_nvc[key]) + "\"\r\nContent-Type: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet\r\n\r\n");
                            writer.Write(formitembytes, 0, formitembytes.Length);
                        }
                        else
                        if (file_nvc[key].Contains(".mp4"))
                        {
                            byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes("Content-Disposition: form-data; name=\"" + key + "\"; filename=\"" + System.IO.Path.GetFileName(file_nvc[key]) + "\"\r\nContent-Type: video/mp4\r\n\r\n");
                            writer.Write(formitembytes, 0, formitembytes.Length);
                        }
                        else
                        if (file_nvc[key].Contains(".xml"))
                        {
                            byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes("Content-Disposition: form-data; name=\"" + key + "\"; filename=\"" + System.IO.Path.GetFileName(file_nvc[key]) + "\"\r\nContent-Type: text/xml\r\n\r\n");
                            writer.Write(formitembytes, 0, formitembytes.Length);
                        }
                        else
                        {
                            byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes("Content-Disposition: form-data; name=\"" + key + "\"; filename=\"dummy.csv\"\r\nContent-Type: application/vnd.ms-excel\r\n\r\n");
                            writer.Write(formitembytes, 0, formitembytes.Length);
                        }

                        byte[] formitembytes2 = System.IO.File.ReadAllBytes(file_nvc[key]);
                        writer.Write(formitembytes2, 0, formitembytes2.Length);
                    }
                    
                    //writer.Write(boundarybytes, 0, boundarybytes.Length);
                    byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                    writer.Write(trailer, 0, trailer.Length);
                    writer.Close();
                }

                response = (HttpWebResponse)request.GetResponse();

                foreach (Cookie cook in response.Cookies)
                {
                    Console.WriteLine("Domain: {0}, Name: {1}, value: {2}", cook.Domain, cook.Name, cook.Value);
                }

            }
            catch (WebException wex)
            {
                return (HttpWebResponse)wex.Response;
            }

            return response;
        }



        // Enable/disable useUnsafeHeaderParsing.
        // See http://o2platform.wordpress.com/2010/10/20/dealing-with-the-server-committed-a-protocol-violation-sectionresponsestatusline/
        public static bool ToggleAllowUnsafeHeaderParsing(bool enable)
        {
            //Get the assembly that contains the internal class
            Assembly assembly = Assembly.GetAssembly(typeof(SettingsSection));
            if (assembly != null)
            {
                //Use the assembly in order to get the internal type for the internal class
                Type settingsSectionType = assembly.GetType("System.Net.Configuration.SettingsSectionInternal");
                if (settingsSectionType != null)
                {
                    //Use the internal static property to get an instance of the internal settings class.
                    //If the static instance isn't created already invoking the property will create it for us.
                    object anInstance = settingsSectionType.InvokeMember("Section",
                    BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic, null, null, new object[] { });
                    if (anInstance != null)
                    {
                        //Locate the private bool field that tells the framework if unsafe header parsing is allowed
                        FieldInfo aUseUnsafeHeaderParsing = settingsSectionType.GetField("useUnsafeHeaderParsing", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (aUseUnsafeHeaderParsing != null)
                        {
                            aUseUnsafeHeaderParsing.SetValue(anInstance, enable);
                            return true;
                        }

                    }
                }
            }
            return false;
        }



        public static HttpWebResponse PUT(String url, String put_data, String content_type, String referer, NameValueCollection headers, bool cookies)
        {
            HttpWebResponse response = null;

            try
            {

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                // ignore SSL errors...
                request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                request.Method = "PUT";
                request.ContentType = content_type;
                //                request.Accept = "*/*";
                //                request.Accept = "application/json";

                if (referer != null)

                    request.Referer = referer;

                if (headers != null)

                    request.Headers.Add(headers);

                //   request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                //      request.ContentLength = data.Length;

                if (cookies)
                {
                    if (current_cookies == null)

                        current_cookies = new CookieContainer();    //create cookie container

                    request.CookieContainer = current_cookies;      //set container for HttpWebRequest 
                }

                byte[] content = Encoding.UTF8.GetBytes(put_data);

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(content, 0, content.Length);
                }

                response = (HttpWebResponse)request.GetResponse();

                foreach (Cookie cook in response.Cookies)
                {
                    Console.WriteLine("Domain: {0}, Name: {1}, value: {2}", cook.Domain, cook.Name, cook.Value);
                }

            }
            catch (WebException wex)
            {
                return (HttpWebResponse)wex.Response;
            }

            return response;
        }


        public static string GenerateHash(string keyString, string document)
        {
            var key = Convert.FromBase64String(keyString);
            string hashString;

            using (var hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(Encoding.ASCII.GetBytes(document));
                hashString = Convert.ToBase64String(hash);
            }

            return hashString;
        }

        public static void DisposeCookies()
        {
            current_cookies = null;
        }

        // ===============================================================================
        // Name...........:	UrlEncode()
        // Description....:	Encodes a Url (or other similar data, for instance HTTP).
        // Syntax.........:	UrlEncode(this String url, int method)
        // Parameters.....:	method		- Optional: A number denoting the method to use to encode the Url.
        //								    1 = via EscapeUriString (default)
        //                                  2 = via EscapeDataString
        //                                  3 = via UrlEncode
        //                                  4 = via UrlPathEncode
        //                                  5 = via HtmlEncode
        //                                  6 = via HtmlAttributeEncode
        // Return values..: The encoded url.
        // Remarks........:	None.
        // ==========================================================================================

        public static String UrlEncode(this String url, int method = 1)
        {
            switch (method)
            {
                case 1:

                    return Uri.EscapeUriString(url);

                case 2:

                    return Uri.EscapeDataString(url);

                case 3:

                    return HttpUtility.UrlEncode(url);

                case 4:

                    return HttpUtility.UrlPathEncode(url);

                case 5:

                    return HttpUtility.HtmlEncode(url);

                case 6:

                    return HttpUtility.HtmlAttributeEncode(url);

                case 7:

                    return System.Net.WebUtility.UrlEncode(url);
            }

            return null;

        }

        public static IDictionary<string, string> _ToKeyValue(this object metaToken)
        {
            if (metaToken == null)
            {
                return null;
            }

            JToken token = metaToken as JToken;
            if (token == null)
            {
                return _ToKeyValue(JObject.FromObject(metaToken));
            }

            if (token.HasValues)
            {
                var contentData = new Dictionary<string, string>();
                foreach (var child in token.Children().ToList())
                {
                    var childContent = child._ToKeyValue();
                    if (childContent != null)
                    {
                        contentData = contentData.Concat(childContent)
                            .ToDictionary(k => k.Key, v => v.Value);
                    }
                }

                return contentData;
            }

            var jValue = token as JValue;




            //            if (jValue?.Value == null)
            if ((jValue != null ? jValue.Value : null) == null)
            {
                return null;
            }

            //            var value = jValue?.Type == JTokenType.Date ? jValue?.ToString("o", CultureInfo.InvariantCulture) : jValue?.ToString(CultureInfo.InvariantCulture);
            var value = jValue.Type == JTokenType.Date ? jValue.ToString("o", CultureInfo.InvariantCulture) : jValue.ToString(CultureInfo.InvariantCulture);

            return new Dictionary<string, string> { { token.Path, value } };
        }


        public static string ToURLEncodedQueryString(this object obj, int encoding_method = 1)
        {
            var keyValues = obj._ToKeyValue();
//            return string.Join("&", keyValues.Select(kvp => string.Format("{0}={1}", kvp.Key, kvp.Value.UrlEncode(encoding_method))));
            return string.Join("&", keyValues.Select(kvp => string.Format("{0}={1}", Regex.Replace(kvp.Key, "\\[[0-9]+\\]$", ""), kvp.Value.UrlEncode(encoding_method))));
        }


        public static bool IsSuccessStatusCode(this HttpWebResponse httpWebResponse)
        {
            return ((int)httpWebResponse.StatusCode >= 200) && ((int)httpWebResponse.StatusCode <= 299);
        }



    }
}
