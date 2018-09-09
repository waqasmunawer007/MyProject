using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TimeTracker.Database;
using TimeTracker.Interfaces;
using TimeTracker.Models;
using Xamarin.Forms;

namespace TimeTracker.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private SQLiteConnection database;
        private static object collisionLock = new object();
        public DatabaseHelper databaseHelper;
        public DayActivity CurrentSelectedDayActivity;
        public BaseViewModel()
        {
            databaseHelper = DatabaseHelper.GetInstance();
            database = DependencyService.Get<IDatabaseConnection>().DbConnection();
            CurrentSelectedDayActivity = new DayActivity();
        }
        public void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this,
              new PropertyChangedEventArgs(propertyName));
        }
    }
}
