using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using TimeTracker.Models;
using Xamarin.Forms;

namespace TimeTracker.ViewModels
{
    public class DayActivityViewModel : BaseViewModel
    {
        private DateTime selectedDateForDayTask;
        private Taskk currentSelectedTask;
        #region CalenderViewPage
        //Handler 
        public EventHandler OpenTaskStatsViewPageHandler;
        //Commands
        public ICommand DateChosenForViewTasksCommand { get; set; }
        public ICommand DateChosenForAddTaskCommand { get; set; }
        #endregion
        public DayActivityViewModel()
        {
            currentSelectedTask = new Taskk();
            CurrentSelectedDate = DateTime.Now;

            //Commands Definations
            DateChosenForAddTaskCommand = new Command(() =>
            {
                SaveTask();
            });

            DateChosenForViewTasksCommand = new Command(() =>
            {
                // OpenTaskStatsViewPageHandler(CurrentSelectedDate.ToString("yyyy-MM-dd"), null);

            });
        }
        public DateTime CurrentSelectedDate
        {
            get { return selectedDateForDayTask; }
            set { selectedDateForDayTask = value; OnPropertyChanged("CurrentSelectedDate"); }
        }
        public Taskk CurrentSelectedTaskk
        {
            get { return currentSelectedTask; }
            set { currentSelectedTask = value; OnPropertyChanged("CurrentSelectedTaskk"); }
        }
        /// <summary>
        /// find range items from list to add the new item(day task)
        /// </summary>
        public void SaveTask()
        {
           //todo
        }
    }
}
