using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TimeTracker.Constants;
using Xamarin.Forms;

namespace TimeTracker.Convertors
{
    public class DateTimeToTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                DateTime dt = (DateTime)value;
                TimeSpan? ts = DateTimeConverter.DateTimeToTimeSpan(dt);
                return ts.GetValueOrDefault(TimeSpan.MinValue);
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
                return TimeSpan.MinValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                TimeSpan ts = (TimeSpan)value;
                DateTime? dt = DateTimeConverter.TimeSpanToDateTime(ts);
                DateTime d = dt.GetValueOrDefault(DateTime.MaxValue);
                return dt.GetValueOrDefault(DateTime.MaxValue);
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
                return DateTime.MinValue;
            }
        }
    }
    public static class DateTimeConverter
    {
        public static TimeSpan? DateTimeToTimeSpan(DateTime dt)
        {
            TimeSpan FResult;
            try
            {
                FResult = dt - dt.Date;
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
                return null;
            }

            return FResult;
        }


        public static DateTime? TimeSpanToDateTime(TimeSpan ts)
        {
            DateTime? FResult = null;
            try
            {
                //string year = string.Format("{0:0000}", DateTime.MinValue.Date.Year);
                //string month = string.Format("{0:00}", DateTime.MinValue.Date.Month);
                //string day = string.Format("{0:00}", DateTime.MinValue.Date.Day);
                string year = string.Format("{0:0000}", DateTime.Now.Year);
                string month = string.Format("{0:00}", DateTime.Now.Month);
                string day = string.Format("{0:00}", DateTime.Now.Day);

                string hours = string.Format("{0:00}", ts.Hours);
                string minutes = string.Format("{0:00}", ts.Minutes);
                string seconds = string.Format("{0:00}", ts.Seconds);

                string dSep = "-"; string tSep = ":"; string dtSep = "T";

                // yyyy-mm-ddTHH:mm:ss
                string dtStr = string.Concat(year, dSep, month, dSep, day, dtSep, hours, tSep, minutes, tSep, seconds);

                DateTime dt;
                if (DateTime.TryParseExact(dtStr, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out dt))
                {
                    FResult = dt;
                }
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
                throw;
            }

            return FResult;
        }
        public static DateTimeOffset? DateTimeToDateTimeOffSet(DateTime dt)
        {
            try
            {
                return new DateTimeOffset(dt);
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
                return null;
            }
        }
        public static DateTime? DateTimeOffSetToDateTime(DateTimeOffset dto)
        {
            try
            {
                return dto.DateTime;
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
                return null;
            }
        }
    }
}
