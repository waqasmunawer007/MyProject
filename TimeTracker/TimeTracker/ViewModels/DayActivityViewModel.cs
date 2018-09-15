using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TimeTracker.Constants;
using TimeTracker.Helpers;
using TimeTracker.Models;
using Xamarin.Forms;

namespace TimeTracker.ViewModels
{
    public class DayActivityViewModel : BaseViewModel
    {
        public ObservableCollection<CarocelModel> taskForDayList;
        public ObservableCollection<int> taskForDayListForMonth;
        private DateTime selectedDateForDayTask;
        private Taskk currentSelectedTask;
        private List<DayActivity> _AllTasksForDay;
        #region CalenderViewPage
        //Handler 
        public EventHandler OpenTaskStatsViewPageHandler;
        //Commands
        public ICommand DateChosenForViewTasksCommand { get; set; }
        public ICommand DateChosenForAddTaskCommand { get; set; }
        #endregion
        public DayActivityViewModel()
        {
            taskForDayList = new ObservableCollection<CarocelModel>();
            taskForDayListForMonth = new ObservableCollection<int>();
            currentSelectedTask = new Taskk();
            CurrentSelectedDate = DateTime.Now;

            //Commands Definations
            DateChosenForAddTaskCommand = new Command(() =>
            {
                SaveTask();
            });

            DateChosenForViewTasksCommand = new Command(() =>
            {
               //  OpenTaskStatsViewPageHandler(CurrentSelectedDate.ToString("yyyy-MM-dd"), null);

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
        public ObservableCollection<CarocelModel> TaskForDayList
        {
            get { return taskForDayList; }
            set { taskForDayList = value; OnPropertyChanged("TaskForDayList"); }
        }
        /// <summary>
        /// find range items from list to add the new item(day task)
        /// </summary>
        public void SaveTask()
        {
            try
            {
                DayActivity dayTask = PrepareDayTaskModel();
                bool isFoundFlage = false;
                List<DayActivity> allTasks = databaseHelper.GetAllDayTasksBaseOnDayMonth(dayTask.CreatedAt);
                DateTime st = new DateTime(Convert.ToDateTime(dayTask.CreatedAt).Year, Convert.ToDateTime(dayTask.CreatedAt).Month, Convert.ToDateTime(dayTask.CreatedAt).Day, Convert.ToDateTime(dayTask.CreatedAt).Hour, Convert.ToDateTime(dayTask.CreatedAt).Minute, Convert.ToDateTime(dayTask.CreatedAt).Second, Convert.ToDateTime(dayTask.TaskStartedAt).Kind);
                if (allTasks.Count > 0)
                { // if there are more than one item in the list then this block invoke 
                    isFoundFlage = true;
                    //find the rang items(time range) in between item/task need to move to mainain the sequence
                    for (int i = 0; i < allTasks.Count; i++)
                    {
                        if (DateTime.Compare(Convert.ToDateTime(Convert.ToDateTime(dayTask.TaskStartedAt).ToString("hh:mm:ss tt")), Convert.ToDateTime(Convert.ToDateTime(allTasks[i].TaskStartedAt).ToString("hh:mm:ss tt"))) < 0)
                        { //if  appropriate range item found then add task between it
                            SaveDayTask(allTasks[i], dayTask, "Up");
                            isFoundFlage = false;
                            break;
                        }
                    }
                    if (isFoundFlage)
                    {
                        //if no appropriate range item found and there are more than one item in the list then
                        //needs to append task  at bottom of list
                        SaveDayTask(allTasks[allTasks.Count - 1], dayTask, "Down");
                        isFoundFlage = false;
                    }
                }
                else if (allTasks.Count == 0)
                {   //if there is not item in current day then add it as a first day task
                    string isUpdated = databaseHelper.AddDayTask(dayTask);
                    if (isUpdated != null)
                    {
                        Settings.LatestInsertedDayTaskTrackId = isUpdated;
                        OpenTaskStatsViewPageHandler(DateTime.Now.ToString("yyyy-MM-dd"), null);
                    }
                }
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
            }
        }
        /// <summary>
        /// add new Day Task on appropriate position
        /// </summary>
        /// <param name="task">task to up or down new task will be added</param>
        /// <param name="newTask">new item that is going to b added</param>
        /// <param name="position">up/down of selected position</param>
        /// <returns></returns>
        public string SaveDayTask(DayActivity task, DayActivity newTask, string position = "Up")
        {
            try
            {
                DayActivity prevTask, afterTask;
                List<DayActivity> allTasks = databaseHelper.GetAllDayTasksBaseOnDayMonth(task.CreatedAt);
                CurrentSelectedDayTask = task;   //selected task from the list (to which up/down new task to be added)                                                                         // CurrentSelectedDayTask = task;
                string taskId = null;
                if (position.Equals("Up")) //add task above the selected task
                {

                    int selectedTaskIndex = allTasks.FindIndex(x => x.DayTaskTrackId == task.DayTaskTrackId);
                    if (selectedTaskIndex > 0)
                    {
                        prevTask = allTasks[selectedTaskIndex - 1];
                        if (DateTime.Compare(Convert.ToDateTime(Convert.ToDateTime(newTask.TaskStartedAt).ToString("hh:mm:ss tt")), Convert.ToDateTime(Convert.ToDateTime(prevTask.TaskStartedAt).ToString("hh:mm:ss tt"))) > 0 && DateTime.Compare(Convert.ToDateTime(Convert.ToDateTime(newTask.TaskStartedAt).ToString("hh:mm:ss tt")), Convert.ToDateTime(Convert.ToDateTime(task.TaskStartedAt).ToString("hh:mm:ss tt"))) < 0)
                        {
                            //in up between of two items
                            prevTask.SpentTime = AppUtil.AppUtil.CalculateSpendedTime(Convert.ToDateTime(Convert.ToDateTime(prevTask.TaskStartedAt).ToString("hh:mm:ss tt")), Convert.ToDateTime(Convert.ToDateTime(newTask.TaskStartedAt).ToString("hh:mm:ss tt")));
                            databaseHelper.AddDayTask(prevTask);
                            newTask.SpentTime = AppUtil.AppUtil.CalculateSpendedTime(Convert.ToDateTime(Convert.ToDateTime(newTask.TaskStartedAt).ToString("hh:mm:ss tt")), Convert.ToDateTime(Convert.ToDateTime(task.TaskStartedAt).ToString("hh:mm:ss tt")));
                            taskId = databaseHelper.AddDayTask(newTask);

                        }
                        else
                        {
                            Application.Current.MainPage.DisplayAlert(AppConstant.SelectePositionToAddTaskTitleText, AppConstant.CurrentTaskIsInProgressText, "OK");
                        }
                    }
                    else if (selectedTaskIndex == 0) // add task at top most of list
                    {
                        if (DateTime.Compare(Convert.ToDateTime(Convert.ToDateTime(Convert.ToDateTime(newTask.TaskStartedAt).ToString("hh:mm:ss tt"))), Convert.ToDateTime(Convert.ToDateTime(task.TaskStartedAt).ToString("hh:mm:ss tt"))) < 0)
                        {
                            //inserted top most
                            newTask.SpentTime = AppUtil.AppUtil.CalculateSpendedTime(Convert.ToDateTime(Convert.ToDateTime(newTask.TaskStartedAt).ToString("hh:mm:ss tt")), Convert.ToDateTime(Convert.ToDateTime(task.TaskStartedAt).ToString("hh:mm:ss tt")));
                            taskId = databaseHelper.AddDayTask(newTask);

                        }
                        else
                        {
                            Application.Current.MainPage.DisplayAlert(AppConstant.SelectePositionToAddTaskTitleText, AppConstant.CurrentTaskIsInProgressText, "OK");
                        }
                    }
                    else
                    {
                        //todo
                    }
                }
                else if (position.Equals("Down")) // add task bottom of selected task
                {
                    int selectedTaskIndex = allTasks.FindIndex(x => x.DayTaskTrackId == task.DayTaskTrackId);
                    if (selectedTaskIndex < allTasks.Count - 1)
                    {
                        afterTask = allTasks[selectedTaskIndex + 1];
                        if (DateTime.Compare(Convert.ToDateTime(Convert.ToDateTime(newTask.TaskStartedAt).ToString("hh:mm:ss tt")), Convert.ToDateTime(Convert.ToDateTime(task.TaskStartedAt).ToString("hh:mm:ss tt"))) > 0 && DateTime.Compare(Convert.ToDateTime(Convert.ToDateTime(newTask.TaskStartedAt).ToString("hh:mm:ss tt")), Convert.ToDateTime(Convert.ToDateTime(afterTask.TaskStartedAt).ToString("hh:mm:ss tt"))) < 0)
                        {
                            //below of selected one task
                            //blow Inbetween of` two items
                            task.SpentTime = AppUtil.AppUtil.CalculateSpendedTime(Convert.ToDateTime(Convert.ToDateTime(task.TaskStartedAt).ToString("hh:mm:ss tt")), Convert.ToDateTime(Convert.ToDateTime(newTask.TaskStartedAt).ToString("hh:mm:ss tt")));
                            databaseHelper.AddDayTask(task);
                            newTask.SpentTime = AppUtil.AppUtil.CalculateSpendedTime(Convert.ToDateTime(Convert.ToDateTime(newTask.TaskStartedAt).ToString("hh:mm:ss tt")), Convert.ToDateTime(Convert.ToDateTime(afterTask.TaskStartedAt).ToString("hh:mm:ss tt")));
                            taskId = databaseHelper.AddDayTask(newTask);

                        }
                        else
                        {
                            Application.Current.MainPage.DisplayAlert(AppConstant.SelectePositionToAddTaskTitleText, AppConstant.CurrentTaskIsInProgressText, "OK");
                        }
                    }
                    else if (selectedTaskIndex == allTasks.Count - 1) // inserted most bottom of list
                    {
                        if (DateTime.Compare(Convert.ToDateTime(Convert.ToDateTime(newTask.TaskStartedAt).ToString("hh:mm:ss tt")), Convert.ToDateTime(task.TaskStartedAt)) > 0)
                        {
                            DayActivity lastRecord = allTasks[selectedTaskIndex];
                            lastRecord.SpentTime = AppUtil.AppUtil.CalculateSpendedTime(Convert.ToDateTime(Convert.ToDateTime(lastRecord.TaskStartedAt).ToString("hh:mm:ss tt")), Convert.ToDateTime(Convert.ToDateTime(newTask.TaskStartedAt).ToString("hh:mm:ss tt")));
                            string isUpdated = databaseHelper.AddDayTask(lastRecord);
                            if (isUpdated != null)
                            {
                                taskId = databaseHelper.AddDayTask(newTask);
                                Helpers.Settings.LatestInsertedDayTaskTrackId = taskId;
                            }
                        }
                        else
                        {
                            Application.Current.MainPage.DisplayAlert(AppConstant.SelectePositionToAddTaskTitleText, AppConstant.CurrentTaskIsInProgressText, "OK");
                        }
                    }
                    else
                    {
                        //todo
                    }

                }

                if (taskId != null)
                { //if task inserted then arrange the position of new task (up/down) to maintain the Sequence of list
                    ArrangeTask(task.CreatedAt, position);
                    OpenTaskStatsViewPageHandler(CurrentSelectedDate.ToString("yyyy-MM-dd"), null);//open the Day Task page
                    return taskId;
                }
                return taskId;
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
                return "";
            }
        }
        /// <summary>
        /// formalize the models for carocel page list
        /// find the days in particular month which indicate the count of carocel pages to be in stats page
        /// </summary>
        /// <param name="d"></param>
        public void GetAndSetDayForMonth(string d)
        {
            try
            {
                CarocelModel PrepareCarocelModel = new CarocelModel();
                DateTime FirstDate = new DateTime(CurrentSelectedDate.Year, CurrentSelectedDate.Month, 1); //first date of selected month
                int daysInMonth = DateTime.DaysInMonth(CurrentSelectedDate.Year, CurrentSelectedDate.Month); // days in selected month ;// count of carocel page
                for (int i = 1; i <= daysInMonth; i++)
                {
                    string date = "" + CurrentSelectedDate.Year + "-" + CurrentSelectedDate.Month + "-" + i; // date s of month--->in the following patteren -> year-month-day  (day = 1 to end of months)
                    _AllTasksForDay = databaseHelper.GetAllDayTasksBaseOnDayMonth(date); // fetch task for every invidual date of month
                    PrepareCarocelModel.ModelTaskForDayList.Clear();
                    PrepareCarocelModel.CurrentDateForCurrentCarocel = Convert.ToDateTime(date);
                    string e = DateTime.Now.ToString("yyyy-M-d");
                    if (Convert.ToDateTime(date).CompareTo(Convert.ToDateTime(e)) == 0)
                    { // if current day date then enable list
                        PrepareCarocelModel.IsEnableCell = true;
                    }
                    else
                    {   //disable the list
                        PrepareCarocelModel.IsEnableCell = false;
                    }
                    PrepareCarocelModel.MonthAndDate = Convert.ToDateTime(date).ToString("d MMMM");
                    for (int j = 0; j < _AllTasksForDay.Count; j++)
                    {    // formalize the every task to populate on UI
                        DayActivity dt = _AllTasksForDay[j];
                        dt.TaskStartedAt = Convert.ToDateTime(dt.TaskStartedAt).ToString("h:mm:ss tt"); // change format of date Time like 10:23:02 AM/PM
                        if (dt.SpentTime != null)
                        {
                            dt.SpentTime = AppUtil.AppUtil.FormatTheSpendedTime(dt.SpentTime); // change the format of spended time like 2h and 20m and 44s
                        }

                        PrepareCarocelModel.ModelTaskForDayList.Add(dt);
                    }

                    //Add the newly formalized model to main Croucel list
                    CarocelModel carocelModel = new CarocelModel();
                    List<DayActivity> DeepCopyOfAllDayTaskList = PrepareCarocelModel.ModelTaskForDayList.Take(PrepareCarocelModel.ModelTaskForDayList.Count).ToList(); // take() return the deep copy of every invidual items
                    foreach (DayActivity dayTask in DeepCopyOfAllDayTaskList)
                    {
                        carocelModel.ModelTaskForDayList.Add(dayTask);
                    }
                    carocelModel.MonthAndDate = PrepareCarocelModel.MonthAndDate;
                    carocelModel.CurrentDateForCurrentCarocel = Convert.ToDateTime(PrepareCarocelModel.CurrentDateForCurrentCarocel);
                    if (carocelModel.ModelTaskForDayList.Count > 0)
                    {
                        carocelModel.IsShowError = false;
                        carocelModel.IsShowList = true;
                    }
                    else
                    {
                        carocelModel.IsShowError = true;
                        carocelModel.IsShowList = false;
                    }
                    carocelModel.IsEnableCell = PrepareCarocelModel.IsEnableCell;
                    TaskForDayList.Add(carocelModel);
                }
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
            }
        }
        /// <summary>
        /// invoke on update day task on specific date
        /// this methods needs to update/refres the list
        /// </summary>
        /// <param name="d"></param>
        /// <param name="PageIndex"></param>
        public void GetAndSetDayForSingleDay(string d, int PageIndex)
        {
            try
            {
                List<DayActivity> list = GetAllDayTasksBaseOnDayMonth(d);
                TaskForDayList[PageIndex].ModelTaskForDayList.Clear();
                foreach (var w in list)
                {
                    w.TaskStartedAt = Convert.ToDateTime(w.TaskStartedAt).ToString("h:mm:ss tt"); // change format of date Time like 10:23:02 AM/PM
                    if (w.SpentTime != null)
                    {
                        w.SpentTime = AppUtil.AppUtil.FormatTheSpendedTime(w.SpentTime); // change the format of spended time like 2h and 20m and 44s
                    }
                    TaskForDayList[PageIndex].ModelTaskForDayList.Add(w);
                }
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
            }
        }
        /// <summary>
        /// update the day task 
        /// </summary>
        /// <param name="dayTask"></param>
        /// <returns></returns>
        public string UpDateDayTask(DayActivity dayTask)
        {
            string isUpdatededTask = databaseHelper.AddDayTask(dayTask);
            if (isUpdatededTask != null)
            {
                return isUpdatededTask;
            }
            return null;
        }
        /// <summary>
        /// Formalize the new day task
        /// </summary>
        /// <returns></returns>
        public DayActivity PrepareDayTaskModel()
        {
            try
            {
                DayActivity dayTask = new DayActivity();
                dayTask.TaskTitle = CurrentSelectedTaskk.Title;
                dayTask.TaskType = CurrentSelectedTaskk.TaskType;
                dayTask.IsSelected = false;
                dayTask.DayTaskTrackId = null;
                dayTask.TaskStartedAt = AppUtil.AppUtil.CurrrentDateTimeInSqliteSupportedFormate();
                dayTask.SpentTime = null;
                dayTask.UpdatedAt = AppUtil.AppUtil.CurrrentDateTimeInSqliteSupportedFormate(); //
                DateTime currentDateTime = new DateTime(CurrentSelectedDate.Year, CurrentSelectedDate.Month, CurrentSelectedDate.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTimeKind.Local);
                dayTask.CreatedAt = currentDateTime.ToString("yyyy-MM-dd hh:mm:ss");
                dayTask.BGColor = CurrentSelectedTaskk.BGColor;
                dayTask.RefTaskTrackId = CurrentSelectedTaskk.TaskTrackId;
                return dayTask;
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// retrieve and return the all day tasks from db
        /// </summary>
        /// <returns></returns>
        public List<DayActivity> GetAllDayTasks()
        {
            List<DayActivity> list = databaseHelper.GetAllDayTasks();
            if (list != null)
            {
                return list;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// retrieve and return the all day tasks created on  specifc date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<DayActivity> GetAllDayTasksBaseOnDayMonth(string date)
        {
            List<DayActivity> list = databaseHelper.GetAllDayTasksBaseOnDayMonth(date);
            if (list != null)
            {
                return list;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// retrieve and return the dAY task base on specific track id
        /// </summary>
        /// <param name="trackId"></param>
        /// <returns></returns>
        public DayActivity GetDayTaskByTrackId(string trackId)
        {
            DayActivity task = databaseHelper.GetDayTaskByTrackId(trackId);
            if (task != null)
            {
                return task;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// retrieve and return the most lastly inserted dAY task 
        /// </summary>
        /// <returns></returns>
        public DayActivity GetLastlyAddedDayTask()
        {
            DayActivity task = databaseHelper.GetLatestInsertedDayTask();
            if (task != null)
            {
                return task;
            }
            else
            {
                return null;
            }
        }


    }
}
