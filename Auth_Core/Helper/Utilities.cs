using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Net.NetworkInformation;
using System.Net.Security;
using Newtonsoft.Json.Linq;

namespace SME_Core
{
    public class Utilities
    {
        // private readonly HttpContextAccessor _httpContext;
        public const string InternationalPhoneCode = "00";        public const string InternationalPhoneSymbol = "+";        public const string Zero = "0";        public const string SaudiInternationalPhoneCode = "966";

        public Utilities()
        {
            //_httpContext = httpContext;
        }
        private static readonly string[] HashAlgorithms = new string[] { "SHA256", "SHA1", "MD5", "SHA512" };
        private const int SaltValueSize = 16;

        /// <summary>
        /// Gets the cookie.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>

        /// <summary>
        /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        private static string UTF8ByteArrayToString(byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        /// <summary>
        /// Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        private static Byte[] StringToUTF8ByteArray(string pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }
        public static object GetValueFromCache(string CacheKey)
        {

            //if (HttpContext. == null || System.Web.HttpContext.Current.Cache == null)
            //    return null;

            //if (System.Web.HttpContext.Current.Cache[CacheKey] != null)
            //{
            //    return System.Web.HttpContext.Current.Cache[CacheKey];
            //}
            return null;
        }
        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="pObject">The p object.</param>
        /// <returns></returns>
        public static String SerializeObject(Object pObject)
        {
            String XmlizedString = null;
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(pObject.GetType());
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            xs.Serialize(xmlTextWriter, pObject);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
            memoryStream.Dispose();
            return XmlizedString;
        }

        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="pXmlizedString">The p xmlized string.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static Object DeserializeObject(String pXmlizedString, Type type)
        {
            XmlSerializer xs = new XmlSerializer(type);
            using (MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString)))
            {
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                return xs.Deserialize(memoryStream);
            }
        }
        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="pXmlizedString">The p xmlized string.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static Object DeserializeObject(String pXmlizedString, string type)
        {
            XmlSerializer xs = new XmlSerializer(Type.GetType(type));
            using (MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString)))
            {
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                return xs.Deserialize(memoryStream);
            }

        }

        public static string GetBriefString(string Body, int maxLength)
        {
            string brief = Body;
            if (string.IsNullOrEmpty(brief))
            {
                return string.Empty;
            }
            string newBrief = brief;
            //cut the brief with substring
            if (brief.Length > maxLength)
            {
                if (brief.Substring(0, maxLength + 1).EndsWith(string.Empty))
                    return brief.Substring(0, maxLength);
                for (int i = 0; i < 20; i++)
                {
                    if (maxLength - 1 - i > 0)
                        newBrief = brief.Substring(0, maxLength - i);
                    else
                        newBrief = string.Empty;

                    if (newBrief.EndsWith(string.Empty))
                    {
                        //correct
                        break;
                    }
                }
            }
            else
            {
                newBrief = brief;
            }
            return newBrief;
        }

        /// <summary>
        /// Base64s the decodein bayte.
        /// </summary>
        /// <param name="base64EncodedData">The base64 encoded data.</param>
        /// <returns></returns>
        public static byte[] Base64DecodeinBayte(string base64EncodedData)
        {
            return System.Convert.FromBase64String(base64EncodedData);
        }


        public static bool IsValidPhoneNo(string dial)
        {

            Regex regexDial = new Regex(@"^(009665|9665|\+9665|05|5)(5|0|3|6|4|9|1|8|7)([0-9]{7})$");
            return regexDial.IsMatch(dial);
        }


        public static string GetDelimitedString<T>(IEnumerable<T> items, string delimiter)
        {
            var stringBuilder = new StringBuilder();

            foreach (var item in items)
                stringBuilder.AppendFormat("{0}{1}", item, delimiter);

            var delimiterLength = delimiter.Length;
            if (stringBuilder.Length >= delimiterLength)
                stringBuilder.Remove(stringBuilder.Length - delimiterLength, delimiterLength);

            return stringBuilder.ToString();
        }
        public static Byte[] DecodeHex16(string srcString)
        {
            if (null == srcString)
            {
                throw new ArgumentNullException("srcString");
            }

            int arrayLength = srcString.Length / 2;

            Byte[] outputBytes = new Byte[arrayLength];

            for (int index = 0; index < arrayLength; index++)
            {
                outputBytes[index] = Byte.Parse(srcString.Substring(index * 2, 2), NumberStyles.AllowHexSpecifier);
            }

            return outputBytes;
        }

        /// <summary>
        /// checks browser and it's version used
        /// </summary>
        /// <returns></returns>

        public static bool IsValidPin(string pin)
        {
            try
            {
                Regex regexDial = new Regex(@"^\d{6}$", RegexOptions.Compiled);
                return regexDial.IsMatch(pin);
            }
            catch (Exception exp)
            {
                // ErrorLogger.LogError(exp.Message, exp, false);
                return false;
            }
        }

        /// <summary>
        /// Gets the current browserinfo.
        /// </summary>
        /// <returns></returns>

        public static DateTime? ConvertStringToDateTimeFromAllianz(string strValue)
        {
            DateTime dt;
            var value = strValue;
            var dateComponents = strValue.Split('-');
            if (dateComponents.Length > 2 && dateComponents[2].Length >= 4)
            {
                string year = strValue.Substring(0, 4);
                string month = strValue.Substring(5, 2);
                string day = strValue.Substring(8, 2);
                string hour = strValue.Substring(11, 2);
                string mintues = strValue.Substring(14, 2);
                string seconds = strValue.Substring(17, 2);
                value = year + "-" + month + "-" + day + " " + hour + ":" + mintues + ":" + seconds;

            }

            if (DateTime.TryParseExact(value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return dt;
            }
            else if (DateTime.TryParseExact(value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return dt;
            }
            else if (DateTime.TryParse(strValue, out dt))
            {
                return dt;
            }
            return null;
        }

        public static DateTime? ConvertStringToDateTimeFromMedGulf(string strValue)
        {
            DateTime dt;
            var value = strValue;
            var dateComponents = strValue.Split('-');
            if (dateComponents.Length > 2 && dateComponents[2].Length >= 4)
            {
                string year = strValue.Substring(0, 4);
                string month = strValue.Substring(5, 2);
                string day = strValue.Substring(8, 2);
                string hour = strValue.Substring(10, 2);
                string mintues = strValue.Substring(13, 2);
                string seconds = strValue.Substring(16, 2);
                value = year + "-" + month + "-" + day + " " + hour + ":" + mintues + ":" + seconds;

            }

            if (DateTime.TryParseExact(value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return dt;
            }
            else if (DateTime.TryParseExact(value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return dt;
            }
            else if (DateTime.TryParse(strValue, out dt))
            {
                return dt;
            }
            return null;
        }
        public static string Remove966FromMobileNumber(string mobile)
        {
            mobile = mobile.Replace("+", "00");
            if (mobile.StartsWith("966"))
            {
                Regex rex = new Regex("966", RegexOptions.IgnoreCase);
                mobile = rex.Replace(mobile, "0", 1);
            }
            if (mobile.StartsWith("00966"))
            {
                Regex rex = new Regex("00966", RegexOptions.IgnoreCase);
                mobile = rex.Replace(mobile, "0", 1);
            }
            return mobile;
        }
        public static string GetAppSetting(string strKey)
        {
            try
            {
                string strValue = string.Empty;
                //  (string)GetValueFromCache(strKey);
                if (string.IsNullOrEmpty(strValue))
                {
                    strValue = "";// System.Configuration.ConfigurationManager.AppSettings[strKey];  need to fix 
                    //if (!string.IsNullOrEmpty(strValue))
                    //{
                    //    AddValueToCache(strKey, strValue, 300);
                    //}
                }
                return strValue;
            }
            catch (Exception exp)
            {
                //  ErrorLogger.LogError(exp.Message, exp, false);
                return string.Empty;
            }
        }
        public static string SaveCompanyFile(string referenceId, byte[] file, string companyName, bool isPolicy)
        {
            try
            {
                string generatedReportFileName = string.Empty;

                int referenceLength = referenceId.Length;

                generatedReportFileName = string.Format("{0}_{1}_{2}.{3}",
                  referenceId.Replace("-", "").Substring(0, referenceLength), companyName,
                   DateTime.Now.ToString("HHmmss"), "pdf");

                string generatedReportDirPath = Utilities.GetAppSetting("PdfCompaniesFilesBaseFolder");// @"D:\BcarePdf";
                if (isPolicy)
                    generatedReportDirPath = Path.Combine(generatedReportDirPath, companyName, "Policies", DateTime.Now.Date.ToString("dd-MM-yyyy"), DateTime.Now.Hour.ToString());
                else
                    generatedReportDirPath = Path.Combine(generatedReportDirPath, companyName, "Invoices", DateTime.Now.Date.ToString("dd-MM-yyyy"), DateTime.Now.Hour.ToString());

                string generatedReportFilePath = Path.Combine(generatedReportDirPath, generatedReportFileName);
                if (!Directory.Exists(generatedReportDirPath))
                    Directory.CreateDirectory(generatedReportDirPath);

                File.WriteAllBytes(generatedReportFilePath, file);
                return generatedReportFilePath;
            }
            catch (Exception exp)
            {
                //   ErrorLogger.LogError(exp.Message, exp, false);
                return string.Empty;
            }
        }

        public static string RemoveWhiteSpaces(string s)
        {
            return string.Join(" ", s.Split(new char[] { ' ' },
                   StringSplitOptions.RemoveEmptyEntries));
        }

        public static int GetSocialStatusId(string socialStatus)
        {
            if (socialStatus == "مطلقة" || socialStatus == "Divorced Female")
            {
                return 5;
            }
            if (socialStatus == "متزوجة" || socialStatus == "Married Female")
            {
                return 4;
            }
            if (socialStatus == "متزوج" || socialStatus == "Married Male")
            {
                return 2;
            }
            if (socialStatus == "غير متاح" || socialStatus == "Not Available")
            {
                return 0;
            }
            if (socialStatus == "غير ذلك" || socialStatus == "Other")
            {
                return 7;
            }
            if (socialStatus == "غير متزوجة" || socialStatus == "Single Female")
            {
                return 3;
            }
            if (socialStatus == "أعزب" || socialStatus == "Single Male")
            {
                return 1;
            }
            if (socialStatus == "ارملة" || socialStatus == "Widowed Female")
            {
                return 6;
            }
            return 1;
        }

        public static string Removemultiplespaces(string value)
        {
            return Regex.Replace(value, @"\s+", " ");
        }
        public static bool ValidPolicyEffectiveDate(DateTime policyEffectiveDate)
        {

            if (policyEffectiveDate < DateTime.Now.Date.AddDays(1) || policyEffectiveDate > DateTime.Now.AddDays(14))
            {
                return false;
            }
            return true;
        }

        public static string GoogleUrlShortener(string longUrl)
        {
            string key = "AIzaSyBhDtJotdkh3WB2QtZpyjYmtXh7Eoc2NKI";
            string finalURL = "";
            string post = "{\"longUrl\": \"" + longUrl + "\"}";
            string shortUrl = longUrl;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/urlshortener/v1/url?key=" + key);
            try
            {
                request.ServicePoint.Expect100Continue = false;
                request.Method = "POST";
                request.ContentLength = post.Length;
                request.ContentType = "application/json";
                request.Headers.Add("Cache-Control", "no-cache");
                using (Stream requestStream = request.GetRequestStream())
                {
                    byte[] postBuffer = Encoding.ASCII.GetBytes(post);
                    requestStream.Write(postBuffer, 0, postBuffer.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader responseReader = new StreamReader(responseStream))
                        {
                            string json = responseReader.ReadToEnd();

                            JObject jsonResult = JObject.Parse(json);
                            finalURL = jsonResult["id"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //     ErrorLogger.LogError(ex.Message,ex,false);
            }
            return finalURL;
        }
        public static string GetShortUrl(string i_sLongUrl)
        {
            string finalURL = string.Empty;
            try
            {
                string i_sBitlyUserName = "bcare2019";
                string i_sBitlyAPIKey = "R_40b626d0a62049099177ffc8a415b887";
                //Construct a valid URL and parameters to connect to Bitly Server
                StringBuilder sbURL = new StringBuilder("http://api.bitly.com/v3/shorten?");
                sbURL.Append("&format=xml");
                sbURL.Append("&longUrl=");
                sbURL.Append(HttpUtility.UrlEncode(i_sLongUrl));
                sbURL.Append("&login=");
                sbURL.Append(System.Web.HttpUtility.UrlEncode(i_sBitlyUserName));
                sbURL.Append("&apiKey=");
                sbURL.Append(System.Web.HttpUtility.UrlEncode(i_sBitlyAPIKey));

                HttpWebRequest objRequest = WebRequest.Create(sbURL.ToString()) as HttpWebRequest;
                objRequest.Method = "GET";
                objRequest.ContentType = "application/x-www-form-urlencoded";
                objRequest.ServicePoint.Expect100Continue = false;
                objRequest.ContentLength = 0;


                //Send the Request and Get the Response. The Response will have the status of operation and the bitlyURL
                WebResponse objResponse = objRequest.GetResponse();
                StreamReader myXML = new StreamReader(objResponse.GetResponseStream());
                dynamic xr = XmlReader.Create(myXML);

                //Retrieve the Status and URL from the Response
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(xr);

                string sStat = xdoc.ChildNodes[1].ChildNodes[1].ChildNodes[0].Value;
                if (sStat == "OK")
                {
                    finalURL = xdoc.ChildNodes[1].ChildNodes[2].ChildNodes[0].ChildNodes[0].Value;
                }
                //else
                //{
                //    return sStat;
                //}

            }
            catch (Exception ex)
            {
                //     ErrorLogger.LogError(ex.Message, ex, false);
            }
            return finalURL;
        }
        public static string ValidatePhoneNumber(string phoneNumber)        {            if (phoneNumber.StartsWith(InternationalPhoneCode))                phoneNumber = phoneNumber.Substring(InternationalPhoneCode.Length);            else if (phoneNumber.StartsWith(InternationalPhoneSymbol))                phoneNumber = phoneNumber.Substring(InternationalPhoneSymbol.Length);            if (!phoneNumber.StartsWith(SaudiInternationalPhoneCode))            {                if (phoneNumber.StartsWith(Zero))                    phoneNumber = phoneNumber.Substring(Zero.Length);                phoneNumber = SaudiInternationalPhoneCode + phoneNumber;            }            return phoneNumber;        }
        public static bool IsValidIBAN(string iban)        {            try            {                iban = iban.ToLower().Replace("sa", "");                if (iban.Length < 22)                {                    return false;                }                return true;            }            catch (Exception exp)            {
                //  ErrorLogger.LogError(exp.Message, exp, false);
                return false;            }        }

        public static string GetDecodeUrl(string url)
        {
            if (HttpUtility.UrlDecode(url).Replace(" ", "+") != url)
            {
                return HttpUtility.UrlDecode(url).Replace(" ", "+");
            }
            else
            {
                return url;
            }
        }

        public static string HandleHijriDate(string dateH)
        {
            string resultDateH = dateH;
            if (!string.IsNullOrEmpty(dateH))
            {
                var convertedDateOfBirthH = dateH.Split('-');
                if (convertedDateOfBirthH == null || convertedDateOfBirthH.Count() != 3)
                { return dateH; }

                int day = 0;
                Int32.TryParse(convertedDateOfBirthH[0], out day);
                int month = 0;
                Int32.TryParse(convertedDateOfBirthH[1], out month);
                int year = 0;
                Int32.TryParse(convertedDateOfBirthH[2], out year);

                if (day == 0 || month == 0 || year == 0)
                { return dateH; }

                if (day > 30 && (month == 1 || month == 3 || month == 5 || month == 7 || month == 9 || month == 11))
                {
                    resultDateH = String.Format("30-{0, 0:D2}-{1}", month, year);
                }
                else if (day > 29 && (month == 4 || month == 6 || month == 8 || month == 10 || month == 12))
                {
                    resultDateH = String.Format("29-{0, 0:D2}-{1}", month, year);
                }
                else if (day > 28 && month == 2)
                {
                    resultDateH = String.Format("28-{0, 0:D2}-{1}", month, year);
                }
            }
            return resultDateH;
        }
        public static string FormatDateString(string dateString)
        {
            string formatedResult = string.Empty;
            try
            {
                if (dateString?.Length < 10 && dateString.Contains("-"))
                {
                    var day = dateString.Split('-')[0];
                    var month = dateString.Split('-')[1];
                    var year = dateString.Split('-')[2];
                    int d = 0;
                    int m = 0;
                    if (int.TryParse(dateString.Split('-')[0], out d))
                    {
                        if (d < 10 && d > 0)
                        {
                            day = "0" + day;
                        }
                        else if (d == 0)
                        {
                            day = "01";
                        }
                    }
                    if (int.TryParse(dateString.Split('-')[1], out m))
                    {
                        if (m < 10 && m > 0)
                        {
                            month = "0" + month;
                        }
                        else if (m == 0)
                        {
                            month = "01";
                        }
                    }
                    formatedResult = day + "-" + month + "-" + year;
                }
                else
                {
                    formatedResult = dateString;
                }
            }
            catch
            {

            }

            if (string.IsNullOrEmpty(dateString))
            {
                try
                {
                    System.Globalization.DateTimeFormatInfo HijriDTFI;
                    HijriDTFI = new System.Globalization.CultureInfo("ar-SA", false).DateTimeFormat;
                    HijriDTFI.Calendar = new System.Globalization.UmAlQuraCalendar();
                    HijriDTFI.ShortDatePattern = "dd-MM-yyyy";
                    DateTime dt = DateTime.Now;
                    formatedResult = dt.ToString("dd-MM-yyyy", HijriDTFI);
                }
                catch
                {

                }
            }

            return formatedResult;
        }

        public string GetUserIP()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // `Dns.Resolve()` method is deprecated.
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            return ipAddress.ToString();
        }
        public static byte[] GetUserIPAsBytes()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // `Dns.Resolve()` method is deprecated.
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            return ipAddress.GetAddressBytes();
        }

        public static int GenerateOTP()
        {
            Random rnd = new Random();
            return rnd.Next(1000, 9999);
        }
        public string GetServerIP()
        {
            //  return _httpContext?.HttpContext.Connection.RemoteIpAddress.ToString();
            return "";
        }
        public string GetUserAgent()
        {
            //return _httpContext?.HttpContext?.Request.Headers["User-Agent"].ToString();
            return "";
        }
        public string GetDeviceName()
        {
            //var ip = _httpContext?.HttpContext.Connection?.RemoteIpAddress?.ToString();
            var ip = "";
            //var hostEntry = Dns.GetHostEntry(ip);
            return ip;
        }

        public string GetMacAddress()
        {
            string mac = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {

                if (nic.OperationalStatus == OperationalStatus.Up && (!nic.Description.Contains("Virtual") && !nic.Description.Contains("Pseudo")))
                {
                    if (nic.GetPhysicalAddress().ToString() != "")
                    {
                        mac = nic.GetPhysicalAddress().ToString();
                    }
                }
            }
            return mac;
        }



        public static string HashData(string plainText, string salt)
        {
            //if (hashAlgorithm == null)
            string hashAlgorithm = HashAlgorithms[0];
            Encoding encoding = Encoding.Unicode;
            //string salt = null;
            if (salt == null)
            {
                salt = Guid.NewGuid().ToString("N").Substring(0, SaltValueSize * 2);
            }
            int saltSize = string.IsNullOrEmpty(salt) ? 0 : salt.Length / 2;
            byte[] valueToHash = new byte[saltSize + encoding.GetByteCount(plainText)];
            for (int i = 0; i < saltSize; i++)
            {
                valueToHash[i] = byte.Parse(salt.Substring(i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
            }
            encoding.GetBytes(plainText, 0, plainText.Length, valueToHash, saltSize);

            using (HashAlgorithm hash = HashAlgorithm.Create(hashAlgorithm))
            {
                byte[] hashValue = hash.ComputeHash(valueToHash);
                StringBuilder hashedText = new StringBuilder((hashValue.Length + saltSize) * 2);
                if (!string.IsNullOrEmpty(salt))
                    hashedText.Append(salt);

                foreach (byte hexdigit in hashValue)
                {
                    hashedText.AppendFormat(CultureInfo.InvariantCulture.NumberFormat, "{0:X2}", hexdigit);
                }
                return hashedText.ToString();
            }
        }
        public static bool VerifyHashedData(string hashedText, string plainText)
        {
            try
            {
                string salt = hashedText.Substring(0, SaltValueSize * 2);
                //foreach (string hashAlgorithm in HashAlgorithms)
                {
                    string computedHash = HashData(plainText, salt);
                    if (string.Equals(computedHash, hashedText, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception exp)
            {
                return false;
                //   throw new AppException(ExceptionEnum.WrongHashing);
            }
        }

        public static bool IsValidMail(string mail)
        {
            try
            {
                Regex regexEmail = new Regex(@"\w+([-+.']\w+)*[.,-]?@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                if (regexEmail.IsMatch(mail))
                {
                    return true;
                }
                return false;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

    }
}
