using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TimeTracker.Models
{
    public class DayActivity : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private int _id;
        private string _dayTaskTrackId;
        private string _taskTitle;
        private string _taskType;
        private bool _isSelected;
        private string _taskStartedAt;
        private string _spentTime;

        private string _createdAt;
        private string _updatedAt;
        private string _color;
        private string _RefTaskTrackId;
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return _id; }
            set { this._id = value; }
        }
        public string DayTaskTrackId
        {
            get { return _dayTaskTrackId; }
            set { this._dayTaskTrackId = value; OnPropertyChanged("DayTaskTrackId"); }
        }
        public string TaskTitle
        {
            get { return _taskTitle; }
            set { this._taskTitle = value; OnPropertyChanged("TaskTitle"); }
        }
        public string TaskType
        {
            get { return _taskType; }
            set { this._taskType = value; OnPropertyChanged("TaskType"); }
        }

        public string TaskStartedAt
        {
            get { return _taskStartedAt; }
            set { this._taskStartedAt = value; OnPropertyChanged("TaskStartedAt"); }
        }
        // totale spented time
        public string SpentTime
        {
            get { return _spentTime; }
            set { this._spentTime = value; OnPropertyChanged("SpentTime"); }
        }

        public string CreatedAt
        {
            get { return _createdAt; }
            set { this._createdAt = value; OnPropertyChanged("CreatedAt"); }
        }
        public string UpdatedAt
        {
            get { return _updatedAt; }
            set { this._updatedAt = value; OnPropertyChanged("UpdatedAt"); }
        }
        public string BGColor
        {
            get { return _color; }
            set { this._color = value; OnPropertyChanged("BGColor"); }
        }
        public bool IsSelected
        {
            get
            { return _isSelected; }
            set
            { this._isSelected = value; OnPropertyChanged("IsSelected"); }
        }
        //taskk table taskTrackId
        public string RefTaskTrackId
        {
            get { return _RefTaskTrackId; }
            set { this._RefTaskTrackId = value; OnPropertyChanged("RefTaskTrackId"); }
        }
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this,
              new PropertyChangedEventArgs(propertyName));
        }
    }
}
