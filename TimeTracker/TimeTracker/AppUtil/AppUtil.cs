using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.Constants;
using Xamarin.Forms;

namespace TimeTracker.AppUtil
{
    public class AppUtil
    {
        /// <summary>
        /// culculate the total time diffrence between two times
        /// </summary>
        /// <param name="dateTime1">Earlier time</param>
        /// <param name="dateTime2">later time</param>
        /// <returns></returns>
        public static string CalculateSpendedTime(DateTime dateTime1, DateTime dateTime2)
        {
            try
            {
                TimeSpan timeSpan = dateTime2.Subtract(dateTime1);
                // TimeSpan timeSpan = newTaskTime.Subtract(runningTaskTime);
                string spendedTime = timeSpan.Hours + ":" + timeSpan.Minutes + ":" + timeSpan.Seconds; // HH:Minuts:second
                return spendedTime;

            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
            }
            return "";
        }
        /// <summary>
        /// Format the Spended time in  hour and minut and second e.g 3h and 23m and 34s
        /// </summary>
        /// <param name="spendedTime"></param>
        /// <returns></returns>
        public static string FormatTheSpendedTime(string spendedTime)
        {
            try
            {
                string[] token = spendedTime.Split(':');
                StringBuilder stringBuilder = new StringBuilder();
                if (token[0].ToString().Equals("0") == false)      // append h (represent  hour) with count of hours
                {
                    stringBuilder.Append(token[0] + "h");
                }
                if (token[1].ToString().Equals("0") == false)
                {
                    if (stringBuilder.ToString().Contains("h")) // if h previously appended then also append the 'and'
                        stringBuilder.Append(" and ");
                    stringBuilder.Append(token[1] + "m");  // append m (represent  minuts) with count of minuts

                }
                if (token[2].ToString().Equals("0") == false)
                {
                    if (stringBuilder.ToString().Contains("m") || stringBuilder.ToString().Contains("h"))
                        stringBuilder.Append(" and ");
                    stringBuilder.Append(token[2] + "s");  // append s (represent  seconds) with count of seconds
                }
                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
            }
            return "";
        }
        /// <summary>
        /// change the date formate
        /// return the date in format which is compatible with  sqlite query
        /// (input) 2018-2-9 change to 2018-02-09 (output)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ChangeDateAccordingToQliteQueryFormate(string date)
        {
            try
            {
                string day, m, y;
                DateTime d = Convert.ToDateTime(date);
                day = d.Day + "";
                m = d.Month + "";
                y = d.Year + "";
                if (d.Day.ToString().Length <= 1)
                {
                    day = "0" + d.Day;
                }
                if (d.Month.ToString().Length <= 1)
                {
                    m = "0" + d.Month;
                }
                string desireFormate = "'" + y + "-" + m + "-" + day + "'";
                return desireFormate;
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
            }
            return "";
        }
        /// <summary>
        /// return the current dateTime string value in the formate 'yyyy-MM-dd hh:mm:ss tt'
        /// this 'yyyy-MM-dd hh:mm:ss tt' formate is directly understanable by SQlite 
        /// </summary>
        /// <returns></returns>
        public static string CurrrentDateTimeInSqliteSupportedFormate()
        {

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
            return currentDateTime;
        }

        public static List<string> ChartsTypesList()
        {
            var chartsCategory = new List<string>();
            chartsCategory.Add(Constants.AppConstant.BarChartTypeLable);
            chartsCategory.Add(Constants.AppConstant.PieChartTypeLable);

            return chartsCategory;
        }
        /// <summary>
        /// Return the type of tasks
        /// </summary>
        /// <returns>List<string></returns>
        public static List<string> TaskksTypesList()
        {
            var weeks = new List<string>();
            weeks.Add(Constants.AppConstant.ProductiveTaskType);
            weeks.Add(Constants.AppConstant.UnProductiveTaskType);
            weeks.Add(Constants.AppConstant.MiscellaneousTaskType);
            return weeks;
        }


        /// <summary>
        /// reurn the day of week
        /// </summary>
        /// <returns>List<string></returns>
        public static List<string> WeekDays()
        {

            var dayOfWeek = new List<string>();
            dayOfWeek.Add("Monday");
            dayOfWeek.Add("Tuesday");
            dayOfWeek.Add("Wednesday");
            dayOfWeek.Add("Thursday");
            dayOfWeek.Add("Friday");
            dayOfWeek.Add("Friday");
            dayOfWeek.Add("Saturday");
            dayOfWeek.Add("Sunday");
            return dayOfWeek;
        }
        /// <summary>
        /// reurn the day of month
        /// </summary>
        /// <returns>List<string></returns>
        public static List<string> Months()
        {

            var months = new List<string>();
            months.Add("January");
            months.Add("February");
            months.Add("March");
            months.Add("April");
            months.Add("May");
            months.Add("June");
            months.Add("July");
            months.Add("August");
            months.Add("September");
            months.Add("October");
            months.Add("December");
            return months;
        }
        /// <summary>
        /// Return the weeks
        /// </summary>
        /// <returns>List<string></returns>
        public static List<string> Weeks()
        {
            var weeks = new List<string>();
            weeks.Add("Week 1");
            weeks.Add("Week 2");
            weeks.Add("Week 3");
            weeks.Add("Week 4");
            return weeks;
        }
        /// <summary>
        /// Return the type of tasks
        /// </summary>
        /// <returns>List<string></returns>
        public static List<string> TasksTypesList()
        {
            var weeks = new List<string>();
            weeks.Add("Task");
            weeks.Add("Procasination");
            weeks.Add("Break/Miscellaneous");
            return weeks;
        }
    }
}
