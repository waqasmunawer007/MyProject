using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace TimeTracker.Models
{
    public class ChartCarocelModel : INotifyPropertyChanged
    {
        private List<ChartModel> _weeklyPieChart { get; set; }
        private List<Color> _color { get; set; }
        private string _weekChartLabel { get; set; }
        private string _crrentMonth { get; set; }


        public ChartCarocelModel()
        {
            _weeklyPieChart = new List<ChartModel>();
            _color = new List<Color>();
        }
        public List<ChartModel> WeeklyPieChart
        {
            get { return _weeklyPieChart; }
            set
            {
                _weeklyPieChart = value; OnPropertyChanged("WeeklyPieChart");
            }
        }
        public string ChartLable
        {
            get { return _weekChartLabel; }
            set
            {
                _weekChartLabel = value; OnPropertyChanged("ChartLable");
            }
        }
        public List<Color> Colors
        {
            get { return _color; }
            set
            {
                _color = value; OnPropertyChanged("Colors");
            }
        }
        public string CureentMonth
        {
            get { return _crrentMonth; }
            set
            {
                _crrentMonth = value; OnPropertyChanged("CureentMonth");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this,
              new PropertyChangedEventArgs(propertyName));
        }
    }
}
