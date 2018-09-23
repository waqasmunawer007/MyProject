using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeTracker.Constants;
using TimeTracker.Models;
using Xamarin.Forms;

namespace TimeTracker.ViewModels
{
    class ChartViewModel : BaseViewModel
    {
        private List<DayActivity> _AllTasksForDay;
        public List<Color> Colors { get; set; }
        public string CurrentDayDate { get; set; }
        public string CurrentMonth { get; set; }
        public string MonthChartLabel { get; set; }
        public string WeekelyChartLabel { get; set; }
        public string DailyChartLabel { get; set; }
        private string _selectedChartType;
        private List<string> _chartTypesList;
        private bool _isBusy = false;
        private string _barUpperRange;
        private bool _isEnabledAds;
        public List<ChartModel> DailyBarChart { get; set; }
        public List<ChartModel> MonthlyBarChart { get; set; }
        public List<ChartModel> wProductiveStackBarChartList { get; set; }
        public List<ChartModel> wUnProductiveStackBarChartList { get; set; }
        public List<ChartModel> wMiscellaneousStackBarChartList { get; set; }
        public List<ChartModel> WeekFourBarChart { get; set; }
        ChartCarocelModel WeeklychartCarocelModel1, WeeklychartCarocelModel2, WeeklychartCarocelModel3, WeeklychartCarocelModel4;
        public List<ChartCarocelModel> WeeklyPieCharts { get; set; }
        public ChartViewModel(string date)
        {
            DailyBarChart = new List<ChartModel>();
            MonthlyBarChart = new List<ChartModel>();
            wProductiveStackBarChartList = new List<ChartModel>();
            wUnProductiveStackBarChartList = new List<ChartModel>();
            wMiscellaneousStackBarChartList = new List<ChartModel>();
            WeeklyPieCharts = new List<ChartCarocelModel>();
            WeeklychartCarocelModel1 = new ChartCarocelModel();
            WeeklychartCarocelModel2 = new ChartCarocelModel();
            WeeklychartCarocelModel3 = new ChartCarocelModel();
            WeeklychartCarocelModel4 = new ChartCarocelModel();
            _chartTypesList = new List<string>();
            CurrentDayDate = Convert.ToDateTime(date).ToString("d MMMM yyyy");
            CurrentMonth = Convert.ToDateTime(date).ToString("MMMM yyyy");
            MonthChartLabel = AppConstant.MonthlyChartLable;
            WeekelyChartLabel = AppConstant.WeekelyChartLable;
            DailyChartLabel = AppConstant.DailyChartLable;
            ChartTypesList = AppUtil.AppUtil.ChartsTypesList();
            SelectedChartType = Constants.AppConstant.BarChartTypeLable;
            Colors = new List<Color>();
            #region Color Scheme For Charts
            Colors.Add(Color.FromHex(AppConstant.ProductiveTaskColor));
            Colors.Add(Color.FromHex(AppConstant.UnProductiveTaskColor));
            Colors.Add(Color.FromHex(AppConstant.MiscellaneousTaskColor));
            #endregion
        }

        public void ChartDataPopulation(string date)
        {
            PrepareDailyChart(date);
            PrepareMonthlyChart(date);
        }
        public string SelectedChartType
        {
            get { return _selectedChartType; }
            set { this._selectedChartType = value; OnPropertyChanged("SelectedChartType"); }

        }
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }
        public string BarUpperRange
        {
            get { return _barUpperRange; }
            set
            {
                _barUpperRange = value;
                OnPropertyChanged("BarUpperRange");
            }
        }
        public List<string> ChartTypesList { get { return _chartTypesList; } set { _chartTypesList = value; OnPropertyChanged("ChartTypesList"); } }
        public bool IsEnabledAds { get { return _isEnabledAds; } set { this._isEnabledAds = value; OnPropertyChanged("IsEnabledAds"); } }
        public string FindHourForDay(string d)
        {
            try
            {
                _AllTasksForDay = databaseHelper.GetAllDayTasksBaseOnDayMonth(d);
                List<DayActivity> _TaskList;
                string r = null;
                TimeSpan productiveTimeSpan, unProductiveTimeSpan, miscellaneousTimeSpan, totalTimeSpan;
                if (_AllTasksForDay.Count > 0)
                {
                    _TaskList = _AllTasksForDay.Where(x => x.TaskType == AppConstant.ProductiveTaskType).ToList();
                    //productive task total spended time
                    foreach (DayActivity dayTask in _TaskList)
                    {
                        if (dayTask.SpentTime != null)
                            productiveTimeSpan = productiveTimeSpan.Add(TimeSpan.Parse(dayTask.SpentTime));
                    }
                    //un productive task total spended time
                    _TaskList = _AllTasksForDay.Where(x => x.TaskType == AppConstant.UnProductiveTaskType).ToList();
                    foreach (DayActivity dayTask in _TaskList)
                    {
                        if (dayTask.SpentTime != null)
                            unProductiveTimeSpan = unProductiveTimeSpan.Add(TimeSpan.Parse(dayTask.SpentTime));
                    }
                    //Miscellaneous
                    _TaskList = _AllTasksForDay.Where(x => x.TaskType == AppConstant.MiscellaneousTaskType).ToList();
                    foreach (DayActivity dayTask in _TaskList)
                    {
                        if (dayTask.SpentTime != null)
                            miscellaneousTimeSpan = miscellaneousTimeSpan.Add(TimeSpan.Parse(dayTask.SpentTime));
                    }

                    totalTimeSpan = productiveTimeSpan.Add(unProductiveTimeSpan).Add(miscellaneousTimeSpan);
                    //Hours conversion
                    double ph = ((double)productiveTimeSpan.Hours) + ((double)productiveTimeSpan.Minutes) / 60 + ((double)productiveTimeSpan.Seconds) / 3600;
                    double uph = ((double)unProductiveTimeSpan.Hours) + ((double)unProductiveTimeSpan.Minutes) / 60 + ((double)unProductiveTimeSpan.Seconds) / 3600;
                    double mh = ((double)miscellaneousTimeSpan.Hours) + ((double)miscellaneousTimeSpan.Minutes) / 60 + ((double)miscellaneousTimeSpan.Seconds / 3600);
                    double totalh = ph + uph + mh;
                    r = totalh + "-" + ph + "-" + uph + "-" + mh;
                    return r;
                }
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
            }
            return "";
        }
        /// <summary>
        /// Daily Bar and Pie chart preparations
        /// </summary>
        /// <param name="d"></param>
        public void PrepareDailyChart(string d)
        {
            try
            {
                string r = FindHourForDay(d);
                if (!String.IsNullOrEmpty(r))
                {
                    string[] token = r.Split('-');
                    //Productive date series
                    DailyBarChart.Clear();
                    DailyBarChart.Add(new ChartModel(AppConstant.ProductiveChartTitle_C, Convert.ToDouble(token[1])));
                    DailyBarChart.Add(new ChartModel(AppConstant.UnProductiveChartTitle_C, Convert.ToDouble(token[2])));
                    DailyBarChart.Add(new ChartModel(AppConstant.MiscellaneousChartTitle_C, Convert.ToDouble(token[3])));
                }
                else
                {
                    DailyBarChart.Add(new ChartModel(AppConstant.ProductiveChartTitle_C, 0));
                    DailyBarChart.Add(new ChartModel(AppConstant.UnProductiveChartTitle_C, 0));
                    DailyBarChart.Add(new ChartModel(AppConstant.MiscellaneousChartTitle_C, 0));
                }
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
            }
        }

        /// <summary>
        /// Monthly Bar and Pie chart preparations
        /// </summary>
        /// <param name="d"></param>
        public void PrepareMonthlyChart(string d)
        {
            try
            {
                List<string> allSpendedTimesList = new List<string>();
                int daysInMonth = DateTime.DaysInMonth(Convert.ToDateTime(d).Year, Convert.ToDateTime(d).Month); // days in selected month ;// count of carocel page
                for (int i = 1; i <= daysInMonth; i++)
                {
                    string date = "" + Convert.ToDateTime(d).Year + "-" + Convert.ToDateTime(d).Month + "-" + i; // date s of month--->in the following patteren -> year-month-day  (day = 1 to end of months)
                    allSpendedTimesList.Add(FindHourForDay(date));

                }
                PrepareDataForWeeklyCharts(allSpendedTimesList, daysInMonth); // Weekly data
                                                                              //Hour calculation for month
                double ph = 0.0;
                double uph = 0.0;
                double mh = 0.0;
                if (allSpendedTimesList.Count > 0)
                {
                    foreach (var e in allSpendedTimesList)
                    {
                        if (!String.IsNullOrEmpty(e))
                        {
                            string[] token = e.Split('-');
                            ph = ph + Convert.ToDouble(token[1]);
                            uph = uph + Convert.ToDouble(token[2]);
                            mh = mh + Convert.ToDouble(token[3]);

                        }

                    }
                }
                double totalHourForMonth = ph + uph + mh;
                // end hour calculation
                #region MonthlyBarChartListData
                MonthlyBarChart.Add(new ChartModel(AppConstant.ProductiveChartTitle_C, ph));
                MonthlyBarChart.Add(new ChartModel(AppConstant.UnProductiveChartTitle_C, uph));
                MonthlyBarChart.Add(new ChartModel(AppConstant.MiscellaneousChartTitle_C, mh));
                #endregion               
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
            }
        }

        /// <summary>
        /// Calculate the weekly hours
        /// prepair weekly data lists
        /// </summary>
        /// <param name="SpendedTimesList"></param>
        /// <param name="dayInMonth"></param>
        public void PrepareDataForWeeklyCharts(List<string> SpendedTimesList, int dayInMonth)
        {
            try
            {
                double ph, uph, mh;
                ph = 0.0;
                uph = 0.0;
                mh = 0.0;
                //Week One
                if (SpendedTimesList.Count > 0)
                {
                    for (int i = 0; i <= 6; i++)  //1 day to 7 day
                    {
                        if (!String.IsNullOrEmpty(SpendedTimesList[i]))
                        {
                            string[] token = SpendedTimesList[i].Split('-');
                            ph = ph + Convert.ToDouble(token[1]);
                            uph = uph + Convert.ToDouble(token[2]);
                            mh = mh + Convert.ToDouble(token[3]);
                        }
                    }
                    //week 1 bar chart data
                    wProductiveStackBarChartList.Add(new ChartModel(AppUtil.AppUtil.Weeks(0), ph, AppConstant.ProductiveTaskType));
                    wUnProductiveStackBarChartList.Add(new ChartModel(AppUtil.AppUtil.Weeks(0), uph, AppConstant.UnProductiveTaskType));
                    wMiscellaneousStackBarChartList.Add(new ChartModel(AppUtil.AppUtil.Weeks(0), mh, AppConstant.MiscellaneousTaskType));

                    //week 1 pie chart data
                    WeeklychartCarocelModel1.WeeklyPieChart.Add(new ChartModel(AppConstant.ProductiveChartTitle_C, ph, week: AppUtil.AppUtil.Weeks(0)));
                    WeeklychartCarocelModel1.WeeklyPieChart.Add(new ChartModel(AppConstant.UnProductiveChartTitle_C, uph, week: AppUtil.AppUtil.Weeks(0)));
                    WeeklychartCarocelModel1.WeeklyPieChart.Add(new ChartModel(AppConstant.MiscellaneousChartTitle_C, mh, week: AppUtil.AppUtil.Weeks(0)));
                    WeeklychartCarocelModel1.CureentMonth = "(Week 1)-" + CurrentMonth;
                    WeeklychartCarocelModel1.Colors = Colors;
                    WeeklychartCarocelModel1.ChartLable = AppUtil.AppUtil.Weeks(0);
                    WeeklyPieCharts.Add(WeeklychartCarocelModel1);
                    //end week 1

                    //week 2
                    ph = 0.0;
                    uph = 0.0;
                    mh = 0.0;
                    for (int i = 7; i <= 13; i++) //7 day to 14 day
                    {
                        if (!String.IsNullOrEmpty(SpendedTimesList[i]))
                        {
                            string[] token = SpendedTimesList[i].Split('-');
                            ph = ph + Convert.ToDouble(token[1]);
                            uph = uph + Convert.ToDouble(token[2]);
                            mh = mh + Convert.ToDouble(token[3]);
                        }
                    }
                    wProductiveStackBarChartList.Add(new ChartModel(AppUtil.AppUtil.Weeks(1), ph, AppConstant.ProductiveTaskType));
                    wUnProductiveStackBarChartList.Add(new ChartModel(AppUtil.AppUtil.Weeks(1), uph, AppConstant.UnProductiveTaskType));
                    wMiscellaneousStackBarChartList.Add(new ChartModel(AppUtil.AppUtil.Weeks(1), mh, AppConstant.MiscellaneousTaskType));
                    // WeeklychartCarocelModel1 2 pie chart data
                    WeeklychartCarocelModel2.WeeklyPieChart.Add(new ChartModel(AppConstant.ProductiveChartTitle_C, ph, week: AppUtil.AppUtil.Weeks(1)));
                    WeeklychartCarocelModel2.WeeklyPieChart.Add(new ChartModel(AppConstant.UnProductiveChartTitle_C, uph, week: AppUtil.AppUtil.Weeks(1)));
                    WeeklychartCarocelModel2.WeeklyPieChart.Add(new ChartModel(AppConstant.MiscellaneousChartTitle_C, mh, week: AppUtil.AppUtil.Weeks(1)));
                    WeeklychartCarocelModel2.CureentMonth = "(Week 2)-" + CurrentMonth;
                    WeeklychartCarocelModel2.Colors = Colors;
                    WeeklychartCarocelModel2.ChartLable = AppUtil.AppUtil.Weeks(1); ;
                    WeeklyPieCharts.Add(WeeklychartCarocelModel2);
                    //end week 2

                    //week 3
                    ph = 0.0;
                    uph = 0.0;
                    mh = 0.0;
                    for (int i = 14; i <= 20; i++) //15 day to 21 day
                    {
                        if (!String.IsNullOrEmpty(SpendedTimesList[i]))
                        {
                            string[] token = SpendedTimesList[i].Split('-');
                            ph = ph + Convert.ToDouble(token[1]);
                            uph = uph + Convert.ToDouble(token[2]);
                            mh = mh + Convert.ToDouble(token[3]);

                        }

                    }
                    wProductiveStackBarChartList.Add(new ChartModel(AppUtil.AppUtil.Weeks(2), ph, AppConstant.ProductiveTaskType));
                    wUnProductiveStackBarChartList.Add(new ChartModel(AppUtil.AppUtil.Weeks(2), uph, AppConstant.UnProductiveTaskType));
                    wMiscellaneousStackBarChartList.Add(new ChartModel(AppUtil.AppUtil.Weeks(2), mh, AppConstant.MiscellaneousTaskType));
                    //week 3 pie chart data
                    WeeklychartCarocelModel3.WeeklyPieChart.Add(new ChartModel(AppConstant.ProductiveChartTitle_C, ph, week: AppUtil.AppUtil.Weeks(2)));
                    WeeklychartCarocelModel3.WeeklyPieChart.Add(new ChartModel(AppConstant.UnProductiveChartTitle_C, uph, week: AppUtil.AppUtil.Weeks(2)));
                    WeeklychartCarocelModel3.WeeklyPieChart.Add(new ChartModel(AppConstant.MiscellaneousChartTitle_C, mh, week: AppUtil.AppUtil.Weeks(2)));
                    WeeklychartCarocelModel3.CureentMonth = "(Week 3)-" + CurrentMonth;
                    WeeklychartCarocelModel3.Colors = Colors;
                    WeeklychartCarocelModel3.ChartLable = AppUtil.AppUtil.Weeks(2);
                    WeeklyPieCharts.Add(WeeklychartCarocelModel3);
                    //end week 3

                    //week 4
                    ph = 0.0;
                    uph = 0.0;
                    mh = 0.0;
                    for (int i = 21; i <= SpendedTimesList.Count - 1; i++) ////22 day to 28 or/and remaining day
                    {
                        if (!String.IsNullOrEmpty(SpendedTimesList[i]))
                        {
                            string[] token = SpendedTimesList[i].Split('-');
                            ph = ph + Convert.ToDouble(token[1]);
                            uph = uph + Convert.ToDouble(token[2]);
                            mh = mh + Convert.ToDouble(token[3]);

                        }

                    }
                    wProductiveStackBarChartList.Add(new ChartModel(AppUtil.AppUtil.Weeks(3), ph, AppConstant.ProductiveTaskType));
                    wUnProductiveStackBarChartList.Add(new ChartModel(AppUtil.AppUtil.Weeks(3), uph, AppConstant.UnProductiveTaskType));
                    wMiscellaneousStackBarChartList.Add(new ChartModel(AppUtil.AppUtil.Weeks(3), mh, AppConstant.MiscellaneousTaskType));
                    //weekly pie chart (4 week)
                    WeeklychartCarocelModel4.WeeklyPieChart.Add(new ChartModel(AppConstant.ProductiveChartTitle_C, ph, week: AppUtil.AppUtil.Weeks(3)));
                    WeeklychartCarocelModel4.WeeklyPieChart.Add(new ChartModel(AppConstant.UnProductiveChartTitle_C, uph, week: AppUtil.AppUtil.Weeks(3)));
                    WeeklychartCarocelModel4.WeeklyPieChart.Add(new ChartModel(AppConstant.MiscellaneousChartTitle_C, mh, week: AppUtil.AppUtil.Weeks(3)));
                    WeeklychartCarocelModel4.CureentMonth = "(Week 4)-" + CurrentMonth;
                    WeeklychartCarocelModel4.Colors = Colors;
                    WeeklychartCarocelModel4.ChartLable = AppUtil.AppUtil.Weeks(3);
                    WeeklyPieCharts.Add(WeeklychartCarocelModel4);
                }
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
            }
        }
    }
}
