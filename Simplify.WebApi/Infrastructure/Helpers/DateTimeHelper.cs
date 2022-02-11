using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml;

namespace Simplify.WebApi.Infrastructure.Helpers
{
    public static class DateTimeHelper
    {
        public static long ConvertDateTimeToJsonDateLong(string strdate) {
            //DateTime date = Convert.ToDateTime(strdate);
            DateTime date = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
            JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            };

            int year = date.Year;
            int month = date.Month;
            int day = date.Day;
            DateTime now = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
            string microsoftJsonDate = JsonConvert.SerializeObject(now, microsoftDateFormatSettings);
            microsoftJsonDate = microsoftJsonDate.Replace("\"\\/Date(", "").Replace(")\\/\"", "");

            return Convert.ToInt64(microsoftJsonDate);

        }
        public static DateTime ParseDateTimeFromStringLiteral(string str)
        {
            string jDate = $"/Date({str})/";

            return JsonConvert.DeserializeObject<DateTime>(@"""" + jDate + @"""");
        }

        public static string ToRfc3339String(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", DateTimeFormatInfo.InvariantInfo);
        }

        //public static string ConvertToRfc3339DateTime(DateTime datetime) {

        //    XmlConvert.ToString(datetime, XmlDateTimeSerializationMode dateTimOption);
        //}
    }

}