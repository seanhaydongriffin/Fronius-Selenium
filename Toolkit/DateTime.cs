using System;

namespace Toolkit
{
    public static class DateTime
    {
        private const string UsDateFormat =                                 "MM/dd/yy";                     // mm/dd/yy
        private const string UsWithCenturyDateFormat =                      "MM/dd/yyyy";                   // mm/dd/yyyy
        private const string AnsiDateFormat =                               "yy.MM.dd";                     // yy.mm.dd
        private const string AnsiWithCenturyDateFormat =                    "yyyy.MM.dd";                   // yyyy.mm.dd
        private const string BritishDateFormat =                            "dd/MM/yy";                     // dd/mm/yy
        private const string BritishWithCenturyDateFormat =                 "dd/MM/yyyy";                   // dd/mm/yyyy
        private const string GermanDateFormat =                             "dd.MM.yy";                     // dd.mm.yy
        private const string GermanWithCenturyDateFormat =                  "dd.MM.yyyy";                   // dd.mm.yyyy
        private const string ItalianDateFormat =                            "dd-MM-yy";                     // dd-mm-yy
        private const string ItalianWithCenturyDateFormat =                 "dd-MM-yyyy";                   // dd-mm-yyyy
        private const string DefaultPlusMillisecondsDateTimeFormat =        "MMMM dd yyyy hh:mm:ss:ffftt";  // mon dd yyyy hh:mi:ss:mmmAM (or PM)
        private const string UsaDateFormat =                                "MM-dd-yy";                     // mm-dd-yy
        private const string UsaWithCenturyDateFormat =                     "MM-dd-yyyy";                   // mm-dd-yyyy
        private const string JapanDateFormat =                              "yy/MM/dd";                     // yy/mm/dd
        private const string JapanWithCenturyDateFormat =                   "yyyy/MM/dd";                   // yyyy/mm/dd
        private const string IsoDateFormat =                                "yyMMdd";                       // yymmdd
        private const string IsoWithCenturyDateFormat =                     "yyyyMMdd";                     // yyyymmdd
        private const string EuropeDefaultWithMillisecondsDateTimeFormat =  "dd MMMM yyyy HH:mm:ss:fff";    // dd mon yyyy hh:mi:ss:mmm (24h)
        private const string OdbcCanonicalDateTimeFormat =                  "yyyy-MM-dd HH:mm:ss";          // yyyy-mm-dd hh:mi:ss (24h)
        private const string OdbcCanonicalWithMillisecondsDateTimeFormat =  "yyyy-MM-dd HH:mm:ss.fff";      // yyyy-mm-dd hh:mi:ss.mmm (24h)
        private const string Us2DateTimeFormat =                            "MM/dd/yy hh:mm:ss tt";         // mm/dd/yy hh:mi:ss AM (or PM)
        private const string Us3DateTimeFormat =                            "M/d/yyyy h:mm tt";             // m/d/yyyy h:mi AM (or PM)
        private const string ISO8601DateFormat =                            "yyyy-MM-dd";                   // yyyy-mm-dd
        private const string ISO8601DateTimeFormat =                        "yyyy-MM-ddTHH:mm:ss.fff";      // yyyy-mm-ddThh:mi:ss.mmm (no spaces)
        private const string ISO8601WithZuluDateTimeFormat =                "yyyy-MM-ddTHH:mm:ss.fffz";     // yyyy-mm-ddThh:mi:ss.mmmZ (no spaces)
        private const string HijriDateTimeFormat =                          "dd MMMM yyyy hh:mm:ss:ffftt";  // dd mon yyyy hh:mi:ss:mmmAM
        private const string Hijri2DateTimeFormat =                         "dd/MM/yyyy hh:mm:ss:ffftt";    // dd/mm/yyyy hh:mi:ss:mmmAM
        private const string MilitaryTimeFormat =                           "hh\\:mm\\:ss";                     // hh:mi:ss
        private const string Military2TimeFormat =                          "hh:mm";                        // hh:mi


        public static System.DateTime Today()
        {
            return System.DateTime.Today;
        }

        public static System.DateTime AddDays(this System.DateTime date, double value)
        {
            return date.AddDays(value);
        }

        public static System.DateTime Parse(string s)
        {
            return System.DateTime.Parse(s);
        }

        public static System.DateTime Now()
        {
            return System.DateTime.Now;
        }

        public static System.DateTime NowDate()
        {
            return System.DateTime.Now.Date;
        }

        public static System.DateTime UtcNow()
        {
            return System.DateTime.UtcNow;
        }

        public static long NowInSeconds()
        {
            return System.DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond / 1000;
        }

        public static string NowInUsDateFormat()
        {
            return System.DateTime.Now.ToString(UsDateFormat);
        }

        public static System.DateTime ToUsDate(this string date)
        {
            return System.DateTime.ParseExact(date, UsDateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInUsWithCenturyDateFormat()
        {
            return System.DateTime.Now.ToString(UsWithCenturyDateFormat);
        }

        public static System.DateTime ToUsWithCenturyDate(this string date)
        {
            return System.DateTime.ParseExact(date, UsWithCenturyDateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInAnsiDateFormat()
        {
            return System.DateTime.Now.ToString(AnsiDateFormat);
        }

        public static System.DateTime ToAnsiDate(this string date)
        {
            return System.DateTime.ParseExact(date, AnsiDateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInAnsiWithCenturyDateFormat()
        {
            return System.DateTime.Now.ToString(AnsiWithCenturyDateFormat);
        }

        public static System.DateTime ToAnsiWithCenturyDate(this string date)
        {
            return System.DateTime.ParseExact(date, AnsiWithCenturyDateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInBritishDateFormat()
        {
            return System.DateTime.Now.ToString(BritishDateFormat);
        }

        public static System.DateTime ToBritishDate(this string date)
        {
            return System.DateTime.ParseExact(date, BritishDateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInBritishWithCenturyDateFormat()
        {
            return System.DateTime.Now.ToString(BritishWithCenturyDateFormat);
        }

        public static System.DateTime ToBritishWithCenturyDate(this string date)
        {
            return System.DateTime.ParseExact(date, BritishWithCenturyDateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string ToBritishWithCenturyDate(this System.DateTime date)
        {
            return date.ToString(BritishWithCenturyDateFormat);
        }

        public static string NowInGermanDateFormat()
        {
            return System.DateTime.Now.ToString(GermanDateFormat);
        }

        public static System.DateTime ToGermanDate(this string date)
        {
            return System.DateTime.ParseExact(date, GermanDateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInGermanWithCenturyDateFormat()
        {
            return System.DateTime.Now.ToString(GermanWithCenturyDateFormat);
        }

        public static System.DateTime ToGermanWithCenturyDate(this string date)
        {
            return System.DateTime.ParseExact(date, GermanWithCenturyDateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInItalianDateFormat()
        {
            return System.DateTime.Now.ToString(ItalianDateFormat);
        }

        public static System.DateTime ToItalianDate(this string date)
        {
            return System.DateTime.ParseExact(date, ItalianDateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInItalianWithCenturyDateFormat()
        {
            return System.DateTime.Now.ToString(ItalianWithCenturyDateFormat);
        }

        public static System.DateTime ToItalianWithCenturyDate(this string date)
        {
            return System.DateTime.ParseExact(date, ItalianWithCenturyDateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInDefaultPlusMillisecondsDateTimeFormat()
        {
            return System.DateTime.Now.ToString(DefaultPlusMillisecondsDateTimeFormat);
        }

        public static System.DateTime ToDefaultPlusMillisecondsDateTime(this string date)
        {
            return System.DateTime.ParseExact(date, DefaultPlusMillisecondsDateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInUsaDateFormat()
        {
            return System.DateTime.Now.ToString(UsaDateFormat);
        }

        public static System.DateTime ToUsaDate(this string date)
        {
            return System.DateTime.ParseExact(date, UsaDateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInUsaWithCenturyDateFormat()
        {
            return System.DateTime.Now.ToString(UsaWithCenturyDateFormat);
        }

        public static System.DateTime ToUsaWithCenturyDate(this string date)
        {
            return System.DateTime.ParseExact(date, UsaWithCenturyDateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInJapanDateFormat()
        {
            return System.DateTime.Now.ToString(JapanDateFormat);
        }

        public static System.DateTime ToJapanDate(this string date)
        {
            return System.DateTime.ParseExact(date, JapanDateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInJapanWithCenturyDateFormat()
        {
            return System.DateTime.Now.ToString(JapanWithCenturyDateFormat);
        }

        public static System.DateTime ToJapanWithCenturyDate(this string date)
        {
            return System.DateTime.ParseExact(date, JapanWithCenturyDateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInIsoDateFormat()
        {
            return System.DateTime.Now.ToString(IsoDateFormat);
        }

        public static System.DateTime ToIsoDate(this string date)
        {
            return System.DateTime.ParseExact(date, IsoDateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInIsoWithCenturyDateFormat()
        {
            return System.DateTime.Now.ToString(IsoWithCenturyDateFormat);
        }

        public static System.DateTime ToIsoWithCenturyDate(this string date)
        {
            return System.DateTime.ParseExact(date, IsoWithCenturyDateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInEuropeDefaultWithMillisecondsDateTimeFormat()
        {
            return System.DateTime.Now.ToString(EuropeDefaultWithMillisecondsDateTimeFormat);
        }

        public static System.DateTime ToEuropeDefaultWithMillisecondsDateTime(this string date)
        {
            return System.DateTime.ParseExact(date, EuropeDefaultWithMillisecondsDateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInOdbcCanonicalDateTimeFormat()
        {
            return System.DateTime.Now.ToString(OdbcCanonicalDateTimeFormat);
        }

        public static System.DateTime ToOdbcCanonicalDateTime(this string datetime)
        {
            return System.DateTime.ParseExact(datetime, OdbcCanonicalDateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string SubtractOdbcCanonicalDateTimeToMilitaryTime(this string datetime1, string datetime2)
        {
            return datetime1.ToOdbcCanonicalDateTime().Subtract(datetime2.ToOdbcCanonicalDateTime()).InMilitaryTime();
        }

        public static string NowInOdbcCanonicalWithMillisecondsDateTimeFormat()
        {
            return System.DateTime.Now.ToString(OdbcCanonicalWithMillisecondsDateTimeFormat);
        }

        public static System.DateTime ToOdbcCanonicalWithMillisecondsDateTime(this string date)
        {
            return System.DateTime.ParseExact(date, OdbcCanonicalWithMillisecondsDateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInUs2DateTimeFormat()
        {
            return System.DateTime.Now.ToString(Us2DateTimeFormat);
        }

        public static System.DateTime ToUs2DateTime(this string date)
        {
            return System.DateTime.ParseExact(date, Us2DateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInUs3DateTimeFormat()
        {
            return System.DateTime.Now.ToString(Us3DateTimeFormat);
        }

        public static System.DateTime ToUs3DateTime(this string date)
        {
            return System.DateTime.ParseExact(date, Us3DateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInISO8601DateFormat()
        {
           return System.DateTime.Now.ToString(ISO8601DateFormat);
        }

        public static System.DateTime ToISO8601Date(this string date)
        {
            return System.DateTime.ParseExact(date, ISO8601DateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInISO8601DateTimeFormat()
        {
            return System.DateTime.Now.ToString(ISO8601DateTimeFormat);
        }

        public static System.DateTime ToISO8601DateTime(this string date)
        {
            return System.DateTime.ParseExact(date, ISO8601DateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInISO8601WithZuluDateTimeFormat()
        {
            return System.DateTime.Now.ToString(ISO8601WithZuluDateTimeFormat);
        }

        public static string ToISO8601WithZuluDateTimeFormat(this System.DateTime date)
        {
            return date.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }

        public static System.DateTime ToISO8601WithZuluDateTime(this string date)
        {
            return System.DateTime.ParseExact(date, ISO8601WithZuluDateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInHijriDateTimeFormat()
        {
            return System.DateTime.Now.ToString(HijriDateTimeFormat);
        }

        public static System.DateTime ToHijriDateTime(this string date)
        {
            return System.DateTime.ParseExact(date, HijriDateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string NowInHijri2DateTimeFormat()
        {
            return System.DateTime.Now.ToString(Hijri2DateTimeFormat);
        }

        public static System.DateTime ToHijri2DateTime(this string date)
        {
            return System.DateTime.ParseExact(date, Hijri2DateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string InMilitaryTime(this System.TimeSpan time)
        {
            return time.ToString(MilitaryTimeFormat);
        }

        public static System.TimeSpan ToMilitaryTime(this string time)
        {
            return System.TimeSpan.ParseExact(time, MilitaryTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static System.TimeSpan ToMilitary2Time(this string time)
        {
            return System.TimeSpan.ParseExact(time, Military2TimeFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static System.TimeSpan To24HourTime(this string time)
        {
            System.DateTime dt;
            if (!System.DateTime.TryParseExact(time, "HH:mm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
            {
                // handle validation error
                return System.TimeSpan.Zero;
            }
            return dt.TimeOfDay;
        }

        public static double DifferenceInAbsoluteSecondsTo(this System.DateTime datetime1, System.DateTime datetime2)
        {
            return Math.Abs((datetime1 - datetime2).TotalSeconds);
        }

        public static double DifferenceInAbsoluteMinutesTo(this System.DateTime datetime1, System.DateTime datetime2)
        {
            return Math.Abs((datetime1 - datetime2).TotalMinutes);
        }

        public static double DifferenceInAbsoluteSecondsTo(this System.TimeSpan time1, System.TimeSpan time2)
        {
            return Math.Abs((time1 - time2).TotalSeconds);
        }

        public static double DifferenceInAbsoluteMinutesTo(this System.TimeSpan time1, System.TimeSpan time2)
        {
            return Math.Abs((time1 - time2).TotalMinutes);
        }

    }
}
