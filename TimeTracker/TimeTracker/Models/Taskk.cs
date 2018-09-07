using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamvvm;

namespace TimeTracker.Models
{
    public class Taskk : BaseModel
    {
        private int _id;
        private string _taskTrackId;
        string _title;
        private string _taskType;
        string _color;
        public bool _isSelected;
        private string _createdAt;
        private string _updatedAt;
        [AutoIncrement, PrimaryKey]
        public int Id
        {
            get { return _id; }
            set { SetField(ref _id, value); }
        }
        [Unique]
        public string TaskTrackId
        {
            get { return _taskTrackId; }
            set { SetField(ref _taskTrackId, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetField(ref _title, value); }
        }
        public string TaskType
        {
            get { return _taskType; }
            set { SetField(ref _taskType, value); }
        }
        public string BGColor
        {
            get { return _color; }
            set { SetField(ref _color, value); }
        }
        public bool IsSelected
        {
            get
            { return _isSelected; }
            set
            { SetField(ref _isSelected, value); }
        }
        public string CreatedAt
        {
            get { return _createdAt; }
            set { SetField(ref _createdAt, value); }
        }
        public string UpdatedAt
        {
            get { return _updatedAt; }
            set { SetField(ref _updatedAt, value); }
        }

        public static explicit operator EventArgs(Taskk v)
        {
            throw new NotImplementedException();
        }
    }
}
