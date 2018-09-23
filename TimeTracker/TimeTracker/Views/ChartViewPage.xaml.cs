using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Constants;
using TimeTracker.Models;
using TimeTracker.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChartViewPage : ContentPage
    {
        ChartViewModel charViewModel;
        string currentOpnChart = " ";//daily , weekly,monthly
        string currentDate;
        bool flag, isFirstTimeOpenPage;
        // string d;
        public ChartViewPage(string date)
        {
            InitializeComponent();
            currentDate = date;
            charViewModel = new ChartViewModel(date);
            BindingContext = charViewModel;
            flag = true;
            isFirstTimeOpenPage = true;
            // d = date;
            DailyBarChart.SecondaryAxis.ActualRangeChanged += SecondaryAxis_ActualRangeChanged;
            WeekelyBarChart.SecondaryAxis.ActualRangeChanged += SecondaryAxis_WeeklyRangeChaged;
        }
        private void weeklyPie_SelectionChanging(object sender, ChartSelectionChangingEventArgs e)
        {
            e.Cancel = true;
            //(e.SelectedSeries as PieSeries).ExplodeIndex = e.SelectedDataPointIndex;
            var index = e.SelectedDataPointIndex;
            var series = e.SelectedSeries;
            var itemSource = series.ItemsSource;
            var CastedItemSource = itemSource.Cast<ChartModel>().ToList();
            var week = CastedItemSource[index].Week;
            weeklyPie_OnSelection(index, week);
        }
        private void weeklyDoughnutChart_SelectionChanging(object sender, ChartSelectionChangingEventArgs e)
        {
            e.Cancel = true;
            //(e.SelectedSeries as DoughnutSeries).ExplodeIndex = e.SelectedDataPointIndex;
            var index = e.SelectedDataPointIndex;
            var series = e.SelectedSeries;
            var itemSource = series.ItemsSource;
            var CastedItemSource = itemSource.Cast<ChartModel>().ToList();
            var week = CastedItemSource[index].Week;
            weeklyDoughnutChart_OnSelection(index, week);
        }
        private void WeeklyParyramid_SelectionChanging(object sender, ChartSelectionChangingEventArgs e)
        {
            e.Cancel = true;
            //(e.SelectedSeries as PyramidSeries).ExplodeIndex = e.SelectedDataPointIndex;
            var index = e.SelectedDataPointIndex;
            var series = e.SelectedSeries;
            var itemSource = series.ItemsSource;
            var CastedItemSource = itemSource.Cast<ChartModel>().ToList();
            var week = CastedItemSource[index].Week;
            WeeklyParyramid_OnSelection(index, week);
        }
        private void WeekelyBarChart_SelectionChanging(object sender, ChartSelectionChangingEventArgs e)
        {
            e.Cancel = true; //disable selection
            var index = e.SelectedDataPointIndex;
            var series = e.SelectedSeries;
            var itemSource = series.ItemsSource;
            var CastedItemSource = itemSource.Cast<ChartModel>().ToList();
            var type = CastedItemSource[index].Type;
            var indexOfType = AppUtil.AppUtil.IndexOfTaskksTypes(type);
            WeekelyBarChart_OnSelection(index, indexOfType);
        }

        private void PayramidChart_SelectionChanging(object sender, ChartSelectionChangingEventArgs e)
        {
            e.Cancel = true;
            //(e.SelectedSeries as PyramidSeries).ExplodeIndex = e.SelectedDataPointIndex;
            PayramidChart_OnSelection(e.SelectedDataPointIndex);
        }

        private void DoughnutChart_SelectionChanging(object sender, ChartSelectionChangingEventArgs e)
        {
            e.Cancel = true;
            //(e.SelectedSeries as DoughnutSeries).ExplodeIndex = e.SelectedDataPointIndex;
            DoughnutChart_OnSelection(e.SelectedDataPointIndex);
        }

        private void PieChart_SelectionChanging(object sender, ChartSelectionChangingEventArgs e)
        {   //daily/monthly Pie Chart
            e.Cancel = true;
            // (e.SelectedSeries as PieSeries).ExplodeIndex = e.SelectedDataPointIndex;
            PieChart_OnSelection(e.SelectedDataPointIndex);

        }
        private void DailyBarChart_SelectionChanging(object sender, ChartSelectionChangingEventArgs e)
        {
            e.Cancel = true;
            DailyBarChart_OnSelection(e.SelectedDataPointIndex);
        }

        private void SecondaryAxis_WeeklyRangeChaged(object sender, ActualRangeChangedEventArgs e)
        {
            e.VisibleMaximum = Convert.ToDouble(e.ActualMaximum);
        }
        private void SecondaryAxis_ActualRangeChanged(object sender, ActualRangeChangedEventArgs e)
        {
            e.VisibleMaximum = Convert.ToDouble(e.ActualMaximum);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void SegControl_ValueChanged(object sender, SegmentedControl.FormsPlugin.Abstractions.ValueChangedEventArgs e)
        {
            switch (e.NewValue)
            {
                case 0: //daily
                    if (isFirstTimeOpenPage)
                    {
                        charViewModel.ChartDataPopulation(currentDate);
                        isFirstTimeOpenPage = false;
                    }
                    ShowDailyChart();
                    break;
                case 1: // weekly
                    ShowWeeklyChart();
                    break;
                case 2: //mothly
                    ShowMonthlyChart();
                    break;
            }
        }
        //invoke on selection of any chart type from type picker
        private void TypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentOpnChart.Equals(AppConstant.monthlyDivisionFlag))
            {
                ShowMonthlyChart();
            }
            else if (currentOpnChart.Equals(AppConstant.weeklyDivisionFlag))
            {
                ShowWeeklyChart();
            }
            else if (currentOpnChart.Equals(AppConstant.dailyDivisionFlag))
            {
                ShowDailyChart();
            }
            else
            {
                ShowDailyChart();
            }
        }
        public void ShowMonthlyChart()
        {
            currentOpnChart = AppConstant.monthlyDivisionFlag;
            if (charViewModel.SelectedChartType.Equals(AppConstant.BarChartTypeLable))
            { //bar
                Title = "Stats(Bar)";
                DailyBarChart.IsVisible = true;
                PieChart.IsVisible = false;
                WeekelyBarChart.IsVisible = false;
                WeeklyPieChart.IsVisible = false;
                DoughnutChart.IsVisible = false;
                WeeklyDoughuntChart.IsVisible = false;
                PayramidChart.IsVisible = false;
                WeeklyPayramidChart.IsVisible = false;
                DailyBarChart.Title.Text = charViewModel.CurrentMonth;
                DailyBarChart.PrimaryAxis.Title.Text = charViewModel.MonthChartLabel;
                DailyBarSeries.ItemsSource = charViewModel.MonthlyBarChart;
            }
            else if (charViewModel.SelectedChartType.Equals(AppConstant.DoughuntChartTypeLable))
            {//doughunt
                Title = "Stats(Doughunt)";
                DailyBarChart.IsVisible = false;
                PieChart.IsVisible = false;
                WeekelyBarChart.IsVisible = false;
                WeeklyPieChart.IsVisible = false;
                WeeklyDoughuntChart.IsVisible = false;
                DoughnutChart.IsVisible = true;
                DoughnutChart.Title.Text = charViewModel.CurrentMonth;
                DoughuntSeries.ItemsSource = charViewModel.MonthlyBarChart;
                PayramidChart.IsVisible = false;
                WeeklyPayramidChart.IsVisible = false;
            }
            else
            { 
            }

        }
        public void ShowWeeklyChart()
        {

            currentOpnChart = AppConstant.weeklyDivisionFlag;
            if (charViewModel.SelectedChartType.Equals(AppConstant.BarChartTypeLable))
            {//bar
                Title = "Stats(Bar)";
                WeekelyBarChart.IsVisible = true;
                WeeklyPieChart.IsVisible = false;
                DailyBarChart.IsVisible = false;
                PieChart.IsVisible = false;
                WeeklyDoughuntChart.IsVisible = false;
                DoughnutChart.IsVisible = false;
                PayramidChart.IsVisible = false;
                WeeklyPayramidChart.IsVisible = false;
            }
            else if (charViewModel.SelectedChartType.Equals(AppConstant.DoughuntChartTypeLable))
            {//doughunt
                Title = "Stats(Doughunt)";
                WeeklyPieChart.IsVisible = false;
                WeekelyBarChart.IsVisible = false;
                DailyBarChart.IsVisible = false;
                PieChart.IsVisible = false;
                WeeklyDoughuntChart.IsVisible = true;
                DoughnutChart.IsVisible = false;
                PayramidChart.IsVisible = false;
                WeeklyPayramidChart.IsVisible = false;
            }
            else
            {
            }
        }
        public void ShowDailyChart()
        {
            if (!flag)
            {
                currentOpnChart = AppConstant.dailyDivisionFlag;
                if (charViewModel.SelectedChartType.Equals(AppConstant.BarChartTypeLable))
                { //bar
                    Title = "Stats(Bar)";
                    DailyBarChart.IsVisible = true;
                    PieChart.IsVisible = false;
                    WeekelyBarChart.IsVisible = false;
                    WeeklyPieChart.IsVisible = false;
                    DoughnutChart.IsVisible = false;
                    WeeklyDoughuntChart.IsVisible = false;
                    PayramidChart.IsVisible = false;
                    WeeklyPayramidChart.IsVisible = false;
                    DailyBarChart.PrimaryAxis.Title.Text = charViewModel.DailyChartLabel;
                    DailyBarChart.Title.Text = charViewModel.CurrentDayDate;
                    DailyBarSeries.ItemsSource = charViewModel.DailyBarChart;

                }
                else if (charViewModel.SelectedChartType.Equals(AppConstant.DoughuntChartTypeLable))
                { //doughunt
                    Title = "Stats(Doughunt)";
                    PieChart.IsVisible = false;
                    DailyBarChart.IsVisible = false;
                    WeekelyBarChart.IsVisible = false;
                    WeeklyPieChart.IsVisible = false;
                    DoughnutChart.IsVisible = true;
                    WeeklyDoughuntChart.IsVisible = false;
                    DoughnutChart.Title.Text = charViewModel.CurrentDayDate;
                    DoughuntSeries.ItemsSource = charViewModel.DailyBarChart;
                    PayramidChart.IsVisible = false;
                    WeeklyPayramidChart.IsVisible = false;
                }
            }
            else
            {//bar -invoke when fist time app constructor get called 
                flag = false;
                Title = "Stats(Bar)";
                currentOpnChart = "daily";
                DailyBarChart.IsVisible = true;
                PieChart.IsVisible = false;
                WeekelyBarChart.IsVisible = false;
                DoughnutChart.IsVisible = false;
                WeeklyDoughuntChart.IsVisible = false;
                PayramidChart.IsVisible = false;
                WeeklyPayramidChart.IsVisible = false;
            }
        }
        //show out the chart Type picker
        private void ChartTypeToolbarItem_Activated(object sender, EventArgs e)
        {
            ChartTypePicker.Focus();
        }
        /// <summary>
        /// daily and monthly  bar chart click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DailyBarChart_SelectionChanged(object sender, ChartSelectionEventArgs e)
        {
            var index = e.SelectedDataPointIndex;
            if (index < 0)
            {
            }
            else
            {
            }
        }
        private void DailyBarChart_OnSelection(int e)
        {
            DailyAndMonthlyGraphsClickNavigations(e);
        }
        private void PieChart_OnSelection(int e)
        {
            DailyAndMonthlyGraphsClickNavigations(e);

        }
        private void DoughnutChart_OnSelection(int e)
        {
            DailyAndMonthlyGraphsClickNavigations(e);
        }
        private void PayramidChart_OnSelection(int e)
        {
            DailyAndMonthlyGraphsClickNavigations(e);
        }
        private void WeekelyBarChart_OnSelection(int index, int indexOfType)
        {
           
        }
        /// <summary>
        ///  weekly  pie chart click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void weeklyPie_OnSelection(int index, string week)
        {
         
        }

        private void weeklyDoughnutChart_OnSelection(int index, string week)
        {
            
        }
        private void WeeklyParyramid_OnSelection(int index, string week)
        {
           
        }

        private void DailyAndMonthlyGraphsClickNavigations(int index)
        {
            if (currentOpnChart.Equals(AppConstant.monthlyDivisionFlag))
            {

            }
            else if (currentOpnChart.Equals(AppConstant.weeklyDivisionFlag))
            {

            }
            else if (currentOpnChart.Equals(AppConstant.dailyDivisionFlag))
            {
            }
            else
            {
                //todo
            }
        }
        private void Week1PieChart_Scroll(object sender, ChartScrollEventArgs e)
        {
            //todo
        }
    }

}