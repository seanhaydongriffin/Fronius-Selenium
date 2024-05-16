using System;
using System.IO;
using System.Collections.Specialized;
using System.Net;
using System.Linq;

namespace Toolkit.Selenium
{
    public class HttpTestObject
    {
        private HttpWebResponse CurrentResponse;
        private String CurrentResponseString;

        public HttpTestObject(HttpWebResponse a, string responseString)
        {
            this.CurrentResponse = a;
            this.CurrentResponseString = responseString;
        }

        // ===============================================================================
        // Name...........:	GET()
        // Description....:	Issues a HTTP GET request.
        // Syntax.........:	GET(String url, String content_type, bool cookies, bool log_data = true, bool exception_on_error = true, String filename = "")
        // Parameters.....:	url                     - the URL for the request
        //                  content_type            - the Content-Type header for the request
        //                  cookies                 - use Cookies in the request
        //                  log_level               - (optional) logging request and response details
        //                                              0 = no logging
        //                                              1 = log request url and response code
        //                                              2 = log request url and response code and response data
        //                  number_of_retries       - (optional) number of retries if the Status Code is not a success
        //                                              0 = no retries (single try)
        //                  filename                - (optional) a filename to receive the contents of the response
        // Return values..: The HTTP test object for this request, otherwise null.
        // Remarks........:	
        // ==========================================================================================

        public static HttpTestObject GET(String url, String content_type, bool cookies, int log_level = 0, int number_of_retries = 0, String filename = "")
        {
            HttpWebResponse response = null;
            String responseString = "";

            for (int retry_num = 0; retry_num <= number_of_retries; retry_num++)
            {
                if (retry_num > 0)
                {
                    Log.WriteLine(url + " failed with status code " + response.StatusCode + ", retry " + retry_num + " of " + number_of_retries + " ...");
                    1.Sleep();
                }

                response = HTTP.GET(url, content_type, cookies, null, null);

                if (filename.Equals(""))

                    responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                else
                {
                    filename = Path.ChangeExtension(filename, response.ContentType.Split('/')[1].Replace("x-icon", "ico").Replace("jpeg", "jpg"));

                    using (var stream = System.IO.File.Create(filename))

                        response.GetResponseStream().CopyTo(stream);
                }

                if (log_level == 1)

                    Log.WriteLine("HTTP Message = Request Url:\r\n" + url + "\r\n\r\nResponse Code:\r\n" + (int)response.StatusCode + " " + response.StatusCode);

                if (log_level == 2)

                    Log.WriteLine("HTTP Message = Request Url:\r\n" + url + "\r\n\r\nResponse Code:\r\n" + (int)response.StatusCode + " " + response.StatusCode + "\r\n\r\nResponse Data:\r\n" + responseString);

                HttpTestObject tmp = new HttpTestObject(response, responseString);

                if (response.StatusCode == HttpStatusCode.OK)

                    return tmp;
            }

            //throw new WebException("HTTP Response Code: " + (int)response.StatusCode + " " + response.StatusCode + ", Data: " + responseString);
            return null;
        }

        // ===============================================================================
        // Name...........:	GET()
        // Description....:	Issues a HTTP GET request.
        // Syntax.........:	GET(String url, String content_type, bool cookies, bool log_data = true, bool exception_on_error = true, String filename = "")
        // Parameters.....:	url                     - the URL for the request
        //                  content_type            - the Content-Type header for the request
        //                  cookies                 - use Cookies in the request
        //                  log_level               - (optional) logging request and response details
        //                                              0 = no logging
        //                                              1 = log request url and response code
        //                                              2 = log request url and response code and response data
        //                  number_of_retries       - (optional) number of retries if the Status Code is not a success
        //                                              0 = no retries (single try)
        //                  filename                - (optional) a filename to receive the contents of the response
        // Return values..: The HTTP test object for this request, otherwise null.
        // Remarks........:	
        // ==========================================================================================

        public static HttpTestObject GET(String url, String content_type, bool cookies, NameValueCollection headers, int log_level = 0, int number_of_retries = 0, String filename = "")
        {
            HttpWebResponse response = null;
            String responseString = "";

            for (int retry_num = 0; retry_num <= number_of_retries; retry_num++)
            {
                if (retry_num > 0)
                {
                    Log.WriteLine(url + " failed with status code " + response.StatusCode + ", retry " + retry_num + " of " + number_of_retries + " ...");
                    1.Sleep();
                }

                response = HTTP.GET(url, content_type, cookies, null, headers);

                if (filename.Equals(""))

                    responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                else

                    using (var stream = System.IO.File.Create(filename))

                        response.GetResponseStream().CopyTo(stream);

                if (log_level == 1)

                    Log.WriteLine("HTTP Message = Request Url:\r\n" + url + "\r\n\r\nResponse Code:\r\n" + (int)response.StatusCode + " " + response.StatusCode);

                if (log_level == 2)

                    Log.WriteLine("HTTP Message = Request Url:\r\n" + url + "\r\n\r\nResponse Code:\r\n" + (int)response.StatusCode + " " + response.StatusCode + "\r\n\r\nResponse Data:\r\n" + responseString);

                HttpTestObject tmp = new HttpTestObject(response, responseString);

                if (response.StatusCode == HttpStatusCode.OK)

                    return tmp;
            }

            //throw new WebException("HTTP Response Code: " + (int)response.StatusCode + " " + response.StatusCode + ", Data: " + responseString);
            return null;
        }

        // ===============================================================================
        // Name...........:	POST()
        // Description....:	Issues a HTTP POST request.
        // Syntax.........:	POST(String url, String post_data, String content_type, bool cookies, bool log_data = true, bool exception_on_error = true, double timeout = 600, int num_retries_on_exception = 3)
        // Parameters.....:	url                         - the URL for the request
        //                  post_data                   - the data to post in the request
        //                  content_type                - the Content-Type header for the request
        //                  cookies                     - use Cookies in the request
        //                  log_level                   - (optional) logging request and response details
        //                                                  0 = no logging
        //                                                  1 = log request url and request data and response code
        //                                                  2 = log request url and request data and response code and response data
        //                  number_of_retries           - (optional) number of retries if the Status Code is not a success
        //                                                  0 = no retries (single try)
        //                  timeout                     - (optional) the timeout for the request
        //                  num_retries_on_exception    - (optional) the number of times to retry the request should it fail 
        // Return values..: The HTTP test object for this request, otherwise null.
        // Remarks........:	
        // ==========================================================================================

        public static HttpTestObject POST(String url, String post_data, String content_type, bool cookies, int log_level = 0, int number_of_retries = 0, double timeout = 600, String filename = "")
        {
            HttpWebResponse response = null;
            String responseString = "";

            for (int retry_num = 0; retry_num <= number_of_retries; retry_num++)
            {
                if (retry_num > 0)
                {
                    Log.WriteLine(url + " failed with status code " + response.StatusCode + ", retry " + retry_num + " of " + number_of_retries + " ...");
                    1.Sleep();
                }

                response = HTTP.POST(url, post_data, content_type, null, null, cookies, timeout);

                if (filename.Equals(""))

                    responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                else
                {
                    filename = Path.ChangeExtension(filename, response.ContentType.Split('/')[1].Replace("x-icon", "ico").Replace("jpeg", "jpg"));

                    using (var stream = System.IO.File.Create(filename))

                        response.GetResponseStream().CopyTo(stream);
                }


                if (log_level == 1)

                    Log.WriteLine("HTTP Message = Request Url:\r\n" + url + "\r\n\r\nRequest Data:\r\n" + post_data + "\r\n\r\nResponse Code:\r\n" + (int)response.StatusCode + " " + response.StatusCode);

                if (log_level == 2)

                    Log.WriteLine("HTTP Message = Request Url:\r\n" + url + "\r\n\r\nRequest Data:\r\n" + post_data + "\r\n\r\nResponse Code:\r\n" + (int)response.StatusCode + " " + response.StatusCode + "\r\n\r\nResponse Data:\r\n" + responseString);

                HttpTestObject tmp = new HttpTestObject(response, responseString);

                if (response.StatusCode == HttpStatusCode.OK)

                    return tmp;
            }

            //throw new WebException("HTTP Response Code: " + (int)response.StatusCode + " " + response.StatusCode + ", Data: " + responseString);
            return null;
        }

        // ===============================================================================
        // Name...........:	POST()
        // Description....:	Issues a HTTP POST request.
        // Syntax.........:	POST(String url, String post_data, String content_type, String referer, bool cookies, bool log_data = true, bool exception_on_error = true, double timeout = 600, int num_retries_on_exception = 3)
        // Parameters.....:	url                         - the URL for the request
        //                  post_data                   - the data to post in the request
        //                  content_type                - the Content-Type header for the request
        //                  referer                     - the Referer header for the request
        //                  cookies                     - use Cookies in the request
        //                  log_level                   - (optional) logging request and response details
        //                                                  0 = no logging
        //                                                  1 = log request url and request data and response code
        //                                                  2 = log request url and request data and response code and response data
        //                  number_of_retries           - (optional) number of retries if the Status Code is not a success
        //                                                  0 = no retries (single try)
        //                  timeout                     - (optional) the timeout for the request
        //                  num_retries_on_exception    - (optional) the number of times to retry the request should it fail 
        // Return values..: The HTTP test object for this request, otherwise null.
        // Remarks........:	
        // ==========================================================================================

        public static HttpTestObject POST(String url, String post_data, String content_type, String referer, bool cookies, int log_level = 0, int number_of_retries = 0, double timeout = 600)
        {
            HttpWebResponse response = null;
            String responseString = "";

            for (int retry_num = 0; retry_num <= number_of_retries; retry_num++)
            {
                if (retry_num > 0)
                {
                    Log.WriteLine(url + " failed with status code " + response.StatusCode + ", retry " + retry_num + " of " + number_of_retries + " ...");
                    1.Sleep();
                }

                response = HTTP.POST(url, post_data, content_type, referer, null, cookies, timeout);
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                if (log_level == 1)

                    Log.WriteLine("HTTP Message = Request Url:\r\n" + url + "\r\n\r\nRequest Data:\r\n" + post_data + "\r\n\r\nResponse Code:\r\n" + (int)response.StatusCode + " " + response.StatusCode);

                if (log_level == 2)

                    Log.WriteLine("HTTP Message = Request Url:\r\n" + url + "\r\n\r\nRequest Data:\r\n" + post_data + "\r\n\r\nResponse Code:\r\n" + (int)response.StatusCode + " " + response.StatusCode + "\r\n\r\nResponse Data:\r\n" + responseString);

                HttpTestObject tmp = new HttpTestObject(response, responseString);

                if (response.StatusCode == HttpStatusCode.OK)

                    return tmp;
            }

            //throw new WebException("HTTP Response Code: " + (int)response.StatusCode + " " + response.StatusCode + ", Data: " + responseString);
            return null;
        }

        // ===============================================================================
        // Name...........:	POST()
        // Description....:	Issues a HTTP POST request.
        // Syntax.........:	POST(String url, String post_data, String content_type, String referer, bool cookies, bool log_data = true, bool exception_on_error = true, double timeout = 600, int num_retries_on_exception = 3)
        // Parameters.....:	url                         - the URL for the request
        //                  post_data                   - the data to post in the request
        //                  content_type                - the Content-Type header for the request
        //                  headers                     - a collection of name-value pairs that make up the headers of the request
        //                  cookies                     - use Cookies in the request
        //                  log_level                   - (optional) logging request and response details
        //                                                  0 = no logging
        //                                                  1 = log request url and request data and response code
        //                                                  2 = log request url and request data and response code and response data
        //                  number_of_retries           - (optional) number of retries if the Status Code is not a success
        //                                                  0 = no retries (single try)
        //                  timeout                     - (optional) the timeout for the request
        //                  num_retries_on_exception    - (optional) the number of times to retry the request should it fail 
        // Return values..: The HTTP test object for this request, otherwise null.
        // Remarks........:	
        // ==========================================================================================

        public static HttpTestObject POST(String url, String post_data, String content_type, NameValueCollection headers, bool cookies, int log_level = 0, int number_of_retries = 0, double timeout = 600)
        {
            HttpWebResponse response = null;
            String responseString = "";

            for (int retry_num = 0; retry_num <= number_of_retries; retry_num++)
            {
                if (retry_num > 0)
                {
                    Log.WriteLine(url + " failed with status code " + response.StatusCode + ", retry " + retry_num + " of " + number_of_retries + " ...");
                    1.Sleep();
                }

                response = HTTP.POST(url, post_data, content_type, null, headers, cookies, timeout);
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                if (log_level == 1)

                    Log.WriteLine("HTTP Message = Request Url:\r\n" + url + "\r\n\r\nRequest Data:\r\n" + post_data + "\r\n\r\nResponse Code:\r\n" + (int)response.StatusCode + " " + response.StatusCode);

                if (log_level == 2)

                    Log.WriteLine("HTTP Message = Request Url:\r\n" + url + "\r\n\r\nRequest Data:\r\n" + post_data + "\r\n\r\nResponse Code:\r\n" + (int)response.StatusCode + " " + response.StatusCode + "\r\n\r\nResponse Data:\r\n" + responseString);

                HttpTestObject tmp = new HttpTestObject(response, responseString);

                if (response.StatusCode == HttpStatusCode.OK)

                    return tmp;
            }

            //throw new WebException("HTTP Response Code: " + (int)response.StatusCode + " " + response.StatusCode + ", Data: " + responseString);
            return null;
        }

        // ===============================================================================
        // Name...........:	POST()
        // Description....:	Issues a HTTP POST request.
        // Syntax.........:	POST(String url, String post_data, String content_type, String referer, bool cookies, bool log_data = true, bool exception_on_error = true, double timeout = 600, int num_retries_on_exception = 3)
        // Parameters.....:	url                         - the URL for the request
        //                  post_data                   - the data to post in the request
        //                  content_type                - the Content-Type header for the request
        //                  referer                     - the Referer header for the request
        //                  headers                     - a collection of name-value pairs that make up the headers of the request
        //                  cookies                     - use Cookies in the request
        //                  log_level                   - (optional) logging request and response details
        //                                                  0 = no logging
        //                                                  1 = log request url and request data and response code
        //                                                  2 = log request url and request data and response code and response data
        //                  number_of_retries           - (optional) number of retries if the Status Code is not a success
        //                                                  0 = no retries (single try)
        //                  timeout                     - (optional) the timeout for the request
        //                  num_retries_on_exception    - (optional) the number of times to retry the request should it fail 
        // Return values..: The HTTP test object for this request, otherwise null.
        // Remarks........:	
        // ==========================================================================================

        public static HttpTestObject POST(String url, String post_data, String content_type, String referer, NameValueCollection headers, bool cookies, int log_level = 0, int number_of_retries = 0, double timeout = 600)
        {
            HttpWebResponse response = null;
            String responseString = "";

            for (int retry_num = 0; retry_num <= number_of_retries; retry_num++)
            {
                if (retry_num > 0)
                {
                    Log.WriteLine(url + " failed with status code " + response.StatusCode + ", retry " + retry_num + " of " + number_of_retries + " ...");
                    1.Sleep();
                }

                response = HTTP.POST(url, post_data, content_type, referer, headers, cookies, timeout);
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                if (log_level == 1)

                    Log.WriteLine("HTTP Message = Request Url:\r\n" + url + "\r\n\r\nRequest Data:\r\n" + post_data + "\r\n\r\nResponse Code:\r\n" + (int)response.StatusCode + " " + response.StatusCode);

                if (log_level == 2)

                    Log.WriteLine("HTTP Message = Request Url:\r\n" + url + "\r\n\r\nRequest Data:\r\n" + post_data + "\r\n\r\nResponse Code:\r\n" + (int)response.StatusCode + " " + response.StatusCode + "\r\n\r\nResponse Data:\r\n" + responseString);

                HttpTestObject tmp = new HttpTestObject(response, responseString);

                if (response.StatusCode == HttpStatusCode.OK)

                    return tmp;
            }

            //throw new WebException("HTTP Response Code: " + (int)response.StatusCode + " " + response.StatusCode + ", Data: " + responseString);
            return null;
        }

        // ===============================================================================
        // Name...........:	POSTMultipart()
        // Description....:	Issues a HTTP Multipart POST request.
        // Syntax.........:	POST(String url, String post_data, String content_type, String referer, bool cookies, bool log_data = true, bool exception_on_error = true, double timeout = 600, int num_retries_on_exception = 3)
        // Parameters.....:	url                         - the URL for the request
        //                  nvc                         - a collection of name-value pairs that make up the form of the request
        //                  file_nvc                    - a collection of name-value pairs that make up the files (binary data) of the request
        //                  cookies                     - use Cookies in the request
        // Return values..: The HTTP test object for this request, otherwise null.
        // Remarks........:	
        // ==========================================================================================

        public static HttpTestObject POSTMultipart(String url, NameValueCollection nvc, NameValueCollection file_nvc, bool cookies)
        {
            HttpWebResponse response = null;
            String responseString = "";
            int number_of_retries = 10;

            for (int retry_num = 0; retry_num <= number_of_retries; retry_num++)
            {
                if (retry_num > 0)
                {
                    Log.WriteLine(url + " failed with status code " + response.StatusCode + ", retry " + retry_num + " of 10 ...");
                    1.Sleep();
                }

                response = HTTP.POSTMultipart(url, nvc, file_nvc, cookies);
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                HttpTestObject tmp = new HttpTestObject(response, responseString);

                if (response.StatusCode == HttpStatusCode.OK)

                    return tmp;
            }

            //throw new WebException("HTTP Response Code: " + (int)response.StatusCode + " " + response.StatusCode + ", Data: " + responseString);
            return null;
        }

        // ===============================================================================
        // Name...........:	PUT()
        // Description....:	Issues a HTTP PUT request.
        // Syntax.........:	PUT(String url, String put_data, String content_type, bool cookies, bool log_data = true)
        // Parameters.....:	url                         - the URL for the request
        //                  put_data                    - the data to send in the request
        //                  content_type                - the Content-Type header for the request
        //                  cookies                     - use Cookies in the request
        //                  log_level                   - (optional) logging request and response details
        //                                                  0 = no logging
        //                                                  1 = log request url and response code
        //                                                  2 = log request url and response code and response data
        // Return values..: The HTTP test object for this request, otherwise null.
        // Remarks........:	
        // ==========================================================================================

        public static HttpTestObject PUT(String url, String put_data, String content_type, bool cookies, int log_level = 0)
        {
            var response = HTTP.PUT(url, put_data, content_type, null, null, cookies);
            var responseString = "";

            try
            {
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                if (log_level == 1)

                    Log.WriteLine("HTTP Message = Request Url:\r\n" + url + "\r\n\r\nRequest Data:\r\n" + put_data + "\r\n\r\nResponse Data:\r\n" + responseString);
            }
            catch (Exception e)
            {
                if (log_level == 2)

                    Log.WriteLine("HTTP Message = Request Url:\r\n" + url + "\r\n\r\nRequest Data:\r\n" + put_data + "\r\n\r\nResponse :\r\nNone - timeout\r\n\r\nResponse Data:\r\n" + responseString);
            }

            HttpTestObject tmp = new HttpTestObject(response, responseString);
            return tmp;
        }

        // ===============================================================================
        // Name...........:	PUT()
        // Description....:	Issues a HTTP PUT request.
        // Syntax.........:	PUT(String url, String put_data, String content_type, String referer, bool cookies, bool log_data = true)
        // Parameters.....:	url                         - the URL for the request
        //                  put_data                    - the data to send in the request
        //                  content_type                - the Content-Type header for the request
        //                  referer                     - the Referer header for the request
        //                  cookies                     - use Cookies in the request
        //                  log_level                   - (optional) logging request and response details
        //                                                  0 = no logging
        //                                                  1 = log request url and response code
        //                                                  2 = log request url and response code and response data
        // Return values..: The HTTP test object for this request, otherwise null.
        // Remarks........:	
        // ==========================================================================================

        public static HttpTestObject PUT(String url, String put_data, String content_type, String referer, bool cookies, int log_level = 0)
        {
            var response = HTTP.PUT(url, put_data, content_type, referer, null, cookies);
            var responseString = "";

            try
            {
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                if (log_level == 1)

                    Log.WriteLine("HTTP Message = Request Url:\r\n" + url + "\r\n\r\nRequest Data:\r\n" + put_data + "\r\n\r\nResponse Data:\r\n" + responseString);
            }
            catch (Exception e)
            {
                if (log_level == 2)

                    Log.WriteLine("HTTP Message = Request Url:\r\n" + url + "\r\n\r\nRequest Data:\r\n" + put_data + "\r\n\r\nResponse :\r\nNone - timeout\r\n\r\nResponse Data:\r\n" + responseString);
            }

            HttpTestObject tmp = new HttpTestObject(response, responseString);
            return tmp;
        }

        // ===============================================================================
        // Name...........:	PUT()
        // Description....:	Issues a HTTP PUT request.
        // Syntax.........:	PUT(String url, String put_data, String content_type, String referer, bool cookies, bool log_data = true)
        // Parameters.....:	url                         - the URL for the request
        //                  put_data                    - the data to send in the request
        //                  content_type                - the Content-Type header for the request
        //                  headers                     - a collection of name-value pairs that make up the headers of the request
        //                  cookies                     - use Cookies in the request
        //                  log_level                   - (optional) logging request and response details
        //                                                  0 = no logging
        //                                                  1 = log request url and response code
        //                                                  2 = log request url and response code and response data
        // Return values..: The HTTP test object for this request, otherwise null.
        // Remarks........:	
        // ==========================================================================================

        public static HttpTestObject PUT(String url, String put_data, String content_type, NameValueCollection headers, bool cookies, int log_level = 0)
        {
            var response = HTTP.PUT(url, put_data, content_type, null, headers, cookies);
            var responseString = "";

            try
            {
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                if (log_level == 1)

                    Log.WriteLine("HTTP Message = Request Url:\r\n" + url + "\r\n\r\nRequest Data:\r\n" + put_data + "\r\n\r\nResponse Data:\r\n" + responseString);
            }
            catch (Exception e)
            {
                if (log_level == 2)

                    Log.WriteLine("HTTP Message = Request Url:\r\n" + url + "\r\n\r\nRequest Data:\r\n" + put_data + "\r\n\r\nResponse :\r\nNone - timeout\r\n\r\nResponse Data:\r\n" + responseString);
            }

            HttpTestObject tmp = new HttpTestObject(response, responseString);
            return tmp;
        }

        // ===============================================================================
        // Name...........:	GetResponseString()
        // Description....:	Get the response from a HTTP test object (ie. GET, POST, POSTMultipart or PUT request above) as a string.
        // Syntax.........:	GetResponseString()
        // Parameters.....:	
        // Return values..: The response as a string from the HTTP test object.
        // Remarks........:	One of the GET, POST, POSTMultipart or PUT methods above must be called prior to this method.
        // ==========================================================================================

        public String GetResponseString()
        {
            return CurrentResponseString;
        }

        // ===============================================================================
        // Name...........:	GetResponseCode()
        // Description....:	Get the Status Code from a HTTP test object (ie. GET, POST, POSTMultipart or PUT request above).
        // Syntax.........:	GetResponseCode()
        // Parameters.....:	
        // Return values..: The 3 digit HTTP Status Code from the HTTP test object.
        // Remarks........:	One of the GET, POST, POSTMultipart or PUT methods above must be called prior to this method.
        // ==========================================================================================

        public int GetResponseCode()
        {
            return (int)CurrentResponse.StatusCode;
        }

        public String GetResponseLocation()
        {
            return CurrentResponse.Headers["Location"].ToString();
        }

        public String GetResponseUriLastSegment()
        {
            return CurrentResponse.ResponseUri.Segments.Last();
        }

        public String GetResponseUriFourthSegment()
        {
            return CurrentResponse.ResponseUri.Segments[3].Replace("/", "");
        }

        public String GetResponseUriFifthSegment()
        {
            return CurrentResponse.ResponseUri.Segments[4].Replace("/", "");
        }

    }
}
