/*
 * Copyright (c) 2013 - 2020 MasterCard International Incorporated
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are 
 * permitted provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list of 
 * conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list of 
 * conditions and the following disclaimer in the documentation and/or other materials 
 * provided with the distribution.
 * Neither the name of the MasterCard International Incorporated nor the names of its 
 * contributors may be used to endorse or promote products derived from this software 
 * without specific prior written permission.
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
 * SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
 * TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
 * OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER 
 * IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING 
 * IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF 
 * SUCH DAMAGE.
 */


using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SimplifyCommerce.Payments
{

    public class PaymentsApi
    {
        public delegate HttpWebRequest HttpWebRequestConfigDelegate(HttpWebRequest httpWebRequest);

        private const int DEFAULT_MAX = 20;

        private RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

        public static String PrivateApiKey { get; set; }

        public static String PublicApiKey { get; set; }

        public static String ApiBaseSandboxUrl { get; set; }

        public static String ApiBaseLiveUrl { get; set; }

        public static String OauthBaseUrl { get; set; }

        public static HttpWebRequestConfigDelegate HttpWebRequestConfig { get; set; }



        private Dictionary<ApiAction, string> ActionToMethodMap = new Dictionary<ApiAction, string>()
        {
             { ApiAction.Create, "POST"},
             { ApiAction.Delete, "DELETE"},
             { ApiAction.Update, "PUT"},
             { ApiAction.List, "GET"},
             { ApiAction.Find, "GET"}
        };

        private bool IsLivePublicKey(String publicKey)
        {
            return publicKey.StartsWith("lvpb");
        }

        static PaymentsApi()
        {
            ApiBaseSandboxUrl = SimplifyConstants.ApiBaseSandboxUrl;
            ApiBaseLiveUrl = SimplifyConstants.ApiBaseLiveUrl;
            OauthBaseUrl = SimplifyConstants.OauthBaseUrl;
        }


        public Object Find(Object obj, Authentication authentication = null)
        {
            Type type = obj.GetType();
            string id = (string)obj.GetType().GetProperty("Id").GetValue(obj, null);

            return Find(type, id, authentication);
        }

        public Object Find(Type type, String id, Authentication authentication = null)
        {
            Dictionary<string, Object> objectMap = new Dictionary<string, Object>();
            objectMap["id"] = id;

            return Execute(type, ApiAction.Find, objectMap, authentication);
        }

        public Object Delete(Object obj, Authentication authentication = null)
        {
            Type type = obj.GetType();
            string id = (string)obj.GetType().GetProperty("Id").GetValue(obj, null);

            return Delete(type, id, authentication);
        }


        public Object Delete(Type type, String id, Authentication authentication = null)
        {
            Dictionary<string, Object> objectMap = new Dictionary<string, Object>();
            objectMap["id"] = id;

            return Execute(type, ApiAction.Delete, objectMap, authentication);
        }




        public Object List(Type type, Dictionary<string, string> sorting = null, Dictionary<string, string> filter = null, int max = DEFAULT_MAX, int offset = 0, Authentication authentication = null)
        {
            Dictionary<string, Object> objectMap = new Dictionary<string, Object>();
            objectMap["max"] = max.ToString();
            objectMap["offset"] = offset.ToString();
            objectMap["sorting"] = sorting;
            objectMap["filter"] = filter;

            return Execute(type, ApiAction.List, objectMap, authentication);
        }

        public Object Create(Object obj, Authentication authentication = null)
        {
            Dictionary<string, Object> objectMap = new Dictionary<string, Object>();
            objectMap["object"] = obj;

            return Execute(obj.GetType(), ApiAction.Create, objectMap, authentication);
        }




        public Object Update(Object obj, Authentication authentication = null)
        {
            Type type = obj.GetType();


            Dictionary<string, Object> objectMap = new Dictionary<string, Object>();
            objectMap["id"] = (string)obj.GetType().GetProperty("Id").GetValue(obj, null);
            objectMap["object"] = obj;

            return Execute(type, ApiAction.Update, objectMap, authentication);
        }


        private Object Execute(Type type, ApiAction action, Dictionary<string, Object> objectMap = null, Authentication authentication = null)
        {
            if (authentication == null)
            {
                authentication = GetDefaultAuthentication();
            }

            string url = null;
            string requestBody = null;
            string signature = null;
            string privateKey = authentication.PrivateApiKey;
            string publicKey = authentication.PublicApiKey;

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;


            switch (action)
            {
                case ApiAction.List:
                    Dictionary<string, string> queryParams = new Dictionary<string, string>();
                    queryParams["max"] = (string)objectMap["max"];
                    queryParams["offset"] = (string)objectMap["offset"];

                    if (objectMap["sorting"] != null)
                    {
                        Dictionary<string, string> sorting = (Dictionary<string, string>)objectMap["sorting"];
                        foreach (var key in sorting.Keys)
                        {
                            queryParams["sorting[" + key + "]"] = sorting[key];
                        }
                    }

                    if (objectMap["filter"] != null)
                    {
                        Dictionary<string, string> filter = (Dictionary<string, string>)objectMap["filter"];
                        foreach (var key in filter.Keys)
                        {
                            queryParams["filter[" + key + "]"] = filter[key];
                        }
                    }


                    url = GetUrl(type, action, publicKey, null, queryParams);

                    signature = JwsEncode(url, null, authentication);
                    break;

                case ApiAction.Find:
                    url = GetUrl(type, action, publicKey, (string)objectMap["id"]);
                    signature = JwsEncode(url, null, authentication);
                    break;
                case ApiAction.Delete:
                    url = GetUrl(type, action, publicKey, (string)objectMap["id"]);
                    signature = JwsEncode(url, null, authentication);
                    break;
                case ApiAction.Create:
                    url = GetUrl(type, action, publicKey, null);

                    requestBody = JsonConvert.SerializeObject(objectMap["object"], settings);
                    requestBody = JwsEncode(url, requestBody, authentication);
                    break;
                case ApiAction.Update:
                    url = GetUrl(type, action, publicKey, (string)objectMap["id"]);
                    requestBody = JsonConvert.SerializeObject(objectMap["object"], settings);
                    requestBody = JwsEncode(url, requestBody, authentication);
                    break;
            }

            HttpWebRequest request = CreateWebRequest(url, requestBody, ActionToMethodMap[action], privateKey, publicKey);

            string result = ExecuteRequest(request, requestBody, signature);
            return ConvertResponse(result, type, action);

        }

        private string ExecuteRequest(HttpWebRequest request, string requestBody = null, string signature = null)
        {
            string result = null;

            try
            {

                if (signature != null)
                {
                    request.Headers["Authorization"] = "JWS " + signature;
                }

                if (requestBody != null)
                {

                    byte[] byteArray = Encoding.UTF8.GetBytes(requestBody);
                    request.ContentLength = byteArray.Length;
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(byteArray, 0, byteArray.Length);
                    requestStream.Close();

                }

                using (StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }

             
            }
            catch (WebException wexc1)
            {
                if (wexc1.Status == WebExceptionStatus.ProtocolError)
                {
                    using (StreamReader sr = new StreamReader(wexc1.Response.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                    }

                    JObject obj = (JObject)JsonConvert.DeserializeObject(result);

                    HttpStatusCode status = ((HttpWebResponse)wexc1.Response).StatusCode;

                    switch (status)
                    {

                        case HttpStatusCode.Unauthorized:
                            throw new AuthenticationException(wexc1.Message, wexc1, obj, (int) status);
                        case HttpStatusCode.BadRequest:
                            throw new InvalidRequestException(wexc1.Message, wexc1, obj, (int) status);
                        case HttpStatusCode.NotFound:
                            throw new ObjectNotFoundException(wexc1.Message, wexc1, obj, (int) status);
                        case HttpStatusCode.MethodNotAllowed:
                            throw new NotAllowedException(wexc1.Message, wexc1, obj, (int) status);
                        case HttpStatusCode.GatewayTimeout:
                            throw new UnknownStateException(wexc1.Message, wexc1, obj, (int) status);
                        default:
                            if ((int)status < 500)
                            {
                                throw new InvalidRequestException(wexc1.Message, wexc1, obj, (int) status);
                            }
                            else
                            {
                                throw new ServerErrorException(wexc1.Message, wexc1, obj, (int) status);
                            }
                    }
                }
                else
                {
                    throw new ApiCommunicationException(wexc1.Message, wexc1);
                }
            }
            catch (IOException ioex)
            {
                throw new ApiCommunicationException(ioex.Message, ioex);
            }
            finally
            {
                if (request != null)
                {
                    try
                    {
                        request.GetResponse().Close();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return result;
        }

        private Object ConvertResponse(string response, Type type, ApiAction action)
        {
            Object result = null;

            switch (action)
            {
                case ApiAction.List:

                    Type listType = typeof(ResourceList<>);
                    result = JsonConvert.DeserializeObject(response, listType.MakeGenericType(type));
                    break;
                case ApiAction.Find:
                case ApiAction.Delete:
                case ApiAction.Create:
                case ApiAction.Update:
                    result = JsonConvert.DeserializeObject(response, type);
                    break;

            }
            return result;
        }

        private String GetUrl(Type type, ApiAction action, String publicKey, String id = null, Dictionary<string, string> queryParams = null)
        {
            string url = ApiBaseSandboxUrl;
            if (IsLivePublicKey(publicKey))
            {
                url = ApiBaseLiveUrl;
            }


            if (!url.EndsWith("/"))
            {
                url += "/";
            }

            string name = type.Name.Substring(0, 1).ToLower() + type.Name.Substring(1);
            url += name;

            switch (action)
            {
                case ApiAction.Find:
                case ApiAction.Delete:
                case ApiAction.Update:
                    url += "/" + id;
                    break;

            }

            if (queryParams != null)
            {
                url += BuildParamString(queryParams);
            }

            return url;
        }

        private string BuildParamString(Dictionary<string, string> queryParams = null, string method = "GET")
        {
            var buffer = new StringBuilder();

            int count = 0;
            bool end = false;
            foreach (var key in queryParams.Keys)
            {
                if (buffer.Length == 0 && method == "GET")
                {
                    buffer.Append("?");
                }
                if (count == queryParams.Count - 1) end = true;

                if (end)
                    buffer.AppendFormat("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(queryParams[key]));
                else
                    buffer.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(queryParams[key]));

                count++;
            }

            return buffer.ToString();
        }



        private HttpWebRequest CreateWebRequest(string uri, string payload, string RequestMethod, string privateKey, string publicKey)
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri);
            webrequest.KeepAlive = false;
            webrequest.Method = RequestMethod;

            string userAgent = "Simplify-C#-SDK/" + SimplifyConstants.Version;


            webrequest.ContentType = "application/json";
            webrequest.Accept = "application/json";
            webrequest.UserAgent = userAgent;

            webrequest.AllowAutoRedirect = true;

            if (HttpWebRequestConfig != null)
            {
                webrequest = HttpWebRequestConfig(webrequest);
            }
            return webrequest;
        }


        public JObject JwsDecode(string payload, string url = null)
        {
            string[] parts = payload.Trim().Split('.');
            string header = DecodeBase64UrlSafe(parts[0]);

            jwsVerifyHeader((JObject)JsonConvert.DeserializeObject(header), PublicApiKey, url);

            // validate sig
            string msg = parts[0] + "." + parts[1];
            string signed = JwsSign(PrivateApiKey, msg);

            if (parts[2] != signed)
            {
                throw new AuthenticationException("JWS signature does not match");
            }

            string decoded = DecodeBase64UrlSafe(parts[1]);
            return (JObject)JsonConvert.DeserializeObject(decoded);



        }

        private void JwsAuthFailure(String reason)
        {
            throw new AuthenticationException("JWS authentication failure: " + reason);
        }


        const String HEADER_URI = "api.simplifycommerce.com/uri";
        const String HEADER_TIME = "api.simplifycommerce.com/timestamp";
        const String HEADER_NONCE = "api.simplifycommerce.com/nonce";
        const String HEADER_OAUTH = "api.simplifycommerce.com/token";
        const String HEADER_UNAME = "uname";
        const long MAX_TIMESTAMP_DIFF = 1000 * 60 * 5; // 5 minutes
        const int NUM_HEADERS = 7;

        private void jwsVerifyHeader(JObject header, string publicKey, string url = null)
        {
            string alg = header.GetValue("alg").Value<string>();
            string typ = header.GetValue("typ").Value<string>();
            string kid = header.GetValue("kid") != null ? header.GetValue("kid").Value<string>() : null;
            string uri = header.GetValue(HEADER_URI) != null ? header.GetValue(HEADER_URI).Value<string>() : null;
            string timestamp = header.GetValue(HEADER_TIME) != null ? header.GetValue(HEADER_TIME).Value<string>() : null;
            string nonce = header.GetValue(HEADER_NONCE) != null ? header.GetValue(HEADER_NONCE).Value<string>() : null;
            string username = header.GetValue(HEADER_UNAME) != null ? header.GetValue(HEADER_UNAME).Value<string>() : null;

            if (alg != "HS256")
            {
                JwsAuthFailure("Incorrect algorithm - found " + alg + " required HS256");
            }

            if (header.Count != NUM_HEADERS)
            {
                JwsAuthFailure("Incorrect number of header parameters - found " + header.Count + " required " + NUM_HEADERS);
            }

            if (typ != "JWS")
            {
                JwsAuthFailure("Incorrect type - found " + typ + " required JWS");
            }

            if (kid == null)
            {
                JwsAuthFailure("Missing key ID");
            }

            if (kid != publicKey)
            {
                // Only test key equality for live keys
                if (IsLivePublicKey(publicKey))
                {
                    JwsAuthFailure("Incorrect key ID");
                }
            }

            if (uri == null)
            {
                JwsAuthFailure("Missing URI");
            }

            if (url != null && uri != url)
            {
                JwsAuthFailure("Invalid URI - found " + uri + " required " + url);
            }

            if (timestamp == null)
            {
                JwsAuthFailure("Missing timestamp");
            }

            long diff = Math.Abs(CurrentTimeMillis() - Convert.ToInt64(timestamp));

            if (diff > MAX_TIMESTAMP_DIFF)
            {
                JwsAuthFailure("Timestamp expired");
            }

            if (nonce == null)
            {
                JwsAuthFailure("Missing nonce");
            }

            if (username == null)
            {
                JwsAuthFailure("Missing usename");
            }

        }

        private string JwsEncode(string uri, string payload, Authentication authentication)
        {
            Dictionary<String, String> jwsHeader = new Dictionary<string, string>();
            string privateKey = authentication.PrivateApiKey;
            string publicKey = authentication.PublicApiKey;
            string oauthToken = null;

            if (authentication is OauthAccessToken)
            {
                oauthToken = ((OauthAccessToken)authentication).AccessToken;
            }

            jwsHeader["typ"] = "JWS";
            jwsHeader["kid"] = publicKey;
            jwsHeader["alg"] = "HS256";
            jwsHeader[HEADER_TIME] = CurrentTimeMillis().ToString();
            jwsHeader[HEADER_URI] = uri;
            byte[] b = new byte[8];
            rng.GetBytes(b);
            jwsHeader[HEADER_NONCE] = Convert.ToBase64String(b);

            if (oauthToken != null)
            {
                jwsHeader[HEADER_OAUTH] = oauthToken;
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            string header = JsonConvert.SerializeObject(jwsHeader, settings);

            if (payload == null)
            {
                payload = "";
            }

            string signatureBaseString = ToBase64UrlSafe(header) + "." + ToBase64UrlSafe(payload);
            string signatureString = JwsSign(privateKey, signatureBaseString);
            string signed = signatureBaseString + "." + signatureString;

            return signed;

        }

        private static readonly DateTime Jan1st1970 = new DateTime
            (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        private String ToBase64UrlSafe(string s)
        {
            string base64Encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(s));

            return base64Encoded.Replace("\n", "").Replace("+", "-").Replace("/", "_").Replace("=", "");

        }

        private String ToBase64UrlSafe(byte[] b)
        {
            string base64Encoded = Convert.ToBase64String(b);

            return base64Encoded.Replace("\n", "").Replace("+", "-").Replace("/", "_").Replace("=", "");

        }

        public String DecodeBase64UrlSafe(string s)
        {

            switch (s.Length % 4)
            {

                case 0:
                    break;
                case 2:
                    s = s + "==";
                    break;
                case 3:
                    s = s + "=";
                    break;

                default:
                    throw new ArgumentException("Webhook event data incorrectly formatted");

            }
            s = s.Replace('-', '+').Replace('_', '/');
            return Encoding.UTF8.GetString(Convert.FromBase64String(s));

        }

        private String JwsSign(string key, string payload)
        {
            HMACSHA256 hmac = new HMACSHA256(Convert.FromBase64String(key));
            byte[] b = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            string s = ToBase64UrlSafe(b);
            return s;

        }

        public object extractObjectFromEvent(JObject webhookEvent)
        {
            JObject ev = (JObject)webhookEvent.GetValue("event");
            string name = ev.GetValue("name").Value<string>();
            string data = ev.GetValue("data").ToString();

            string objectType = name.Split('.')[0];
            objectType = char.ToUpper(objectType[0]) + objectType.Substring(1);
            return JsonConvert.DeserializeObject(data, Type.GetType("SimplifyCommerce.Payments." + objectType));

        }

        public OauthAccessToken RefreshAccessToken(OauthAccessToken token)
        {
            Dictionary<string, string> p = new Dictionary<string, string>();
            p["grant_type"] = "refresh_token";
            p["refresh_token"] = token.RefreshToken;
            return ExecuteTokenRequest(p, token, OauthBaseUrl + "/token");

        }

        public OauthAccessToken RevokeAccessToken(OauthAccessToken token)
        {
            Dictionary<string, string> p = new Dictionary<string, string>();
            p["token"] = token.AccessToken;

            return ExecuteTokenRequest(p, token, OauthBaseUrl + "/revoke", true);

        }

        public OauthAccessToken RequestAccessToken(OauthAccessTokenRequest oauthRequest, Authentication authentication = null)
        {
            Dictionary<string, string> p = new Dictionary<string, string>();
            p["grant_type"] = "authorization_code";
            p["code"] = oauthRequest.Code;
            p["redirect_uri"] = oauthRequest.RedirectUri;
            return ExecuteTokenRequest(p, authentication, OauthBaseUrl + "/token");
        }

        public Authentication GetDefaultAuthentication()
        {
            return new Authentication(PaymentsApi.PublicApiKey, PaymentsApi.PrivateApiKey);
        }

        private OauthAccessToken ExecuteTokenRequest (Dictionary<string, string> p, Authentication authentication, string url, bool revoke = false)
        {
            OauthAccessToken token = null;

            if (authentication == null)
            {
                authentication = GetDefaultAuthentication();
            }
         
            string requestBody = BuildParamString(p, "POST");

            string privateKey = authentication.PrivateApiKey;
            string publicKey = authentication.PublicApiKey;
            string signature = JwsEncode(url, requestBody, authentication);
       
            HttpWebRequest request = CreateWebRequest(url, requestBody, "POST", privateKey, publicKey);
            request.ContentType = "application/x-www-form-urlencoded";
            request.AllowAutoRedirect = false;

            try
            {
                string result = ExecuteRequest(request, requestBody, signature);

                if (!revoke)
                {
                    token = (OauthAccessToken)JsonConvert.DeserializeObject(result, typeof(OauthAccessToken));
                }
                else
                {
                    OauthRevokeAccessToken t = (OauthRevokeAccessToken)JsonConvert.DeserializeObject(result, typeof(OauthRevokeAccessToken));
                    token = new OauthAccessToken();
                    token.AccessToken = t.TokenRevoked;
                }

                token.PrivateApiKey = privateKey;
                token.PublicApiKey = publicKey;
            }
            catch (InvalidRequestException ex)
            {
                JObject o = (JObject)ex.ErrorData;

                string error = o.GetValue("error").Value<string>();
                string errorDescription = o.GetValue("error_description").Value<string>();
                throw new OauthException(error, errorDescription);
            }

            return token;
        }


    }
}

public enum ApiAction
{
    Create, List, Find, Delete, Update
}
