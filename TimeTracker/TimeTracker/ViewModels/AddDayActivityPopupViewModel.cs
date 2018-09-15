using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TimeTracker.Constants;
using TimeTracker.Helpers;
using TimeTracker.Models;
using Xamarin.Forms;

namespace TimeTracker.ViewModels
{
    public class AddDayActivityPopupViewModel : BaseViewModel
    {
        private DateTime _sTime; //Start Time
        private List<string> tasktypesList;
        private ObservableCollection<string> taskList;
        private INavigation navigation;
        private string selectedTaskType;//task type selected for new task from picker
        private string selectedTask;//task selected for new task from picker
                                    // DayTask CurrentSelectedDayTask;
        DayActivity _newDayTask;
        bool isDayTaskEditRequest;
        string prevSelectedTaskTitle; //previously selected task title in case of edit request
        public AddDayActivityPopupViewModel(INavigation nav)
        {
            StartTime = DateTime.Now;
            taskList = new ObservableCollection<string>();
            tasktypesList = new List<string>();
            navigation = nav;
            isDayTaskEditRequest = false;
            _newDayTask = new DayActivity();
            // CurrentSelectedDayTask = new DayTask();
            TasktypesList = AppUtil.AppUtil.TaskksTypesList();
        }
        public DayActivity NewDayTask { get { return _newDayTask; } set { this._newDayTask = value; OnPropertyChanged("NewDayTask"); } }// new task going to be added
        public string SelectedTaskType { get { return selectedTaskType; } set { selectedTaskType = value; OnPropertyChanged("SelectedTaskType"); } }//task type selected for new task from picker
        public string SelectedTask { get { return selectedTask; } set { selectedTask = value; OnPropertyChanged("SelectedTask"); } }//task type selected for new task from picker
        public List<string> TasktypesList { get { return tasktypesList; } set { tasktypesList = value; OnPropertyChanged("TasktypesList"); } }
        public ObservableCollection<string> TaskList { get { return taskList; } set { taskList = value; OnPropertyChanged("TaskList"); } }
        public DateTime StartTime
        {
            get
            {
                return _sTime;
            }
            set
            {
                _sTime = new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Kind);

                OnPropertyChanged("StartTime");
            }
        }
        /// <summary>
        /// invoke when user select task type from picker
        /// then this method invoke automatically to retriev the all its tasks and bind it to Task's title picker
        /// </summary>
        int TaskTypeChangeAttempts = 1;
        public void GetAllTTaskAgainstTaskType()
        {
            try
            {
                List<Taskk> list = new List<Taskk>();
                list.Clear();
                TaskList.Clear();
                list = databaseHelper.GetAllTaskksBaseOnTaskType(SelectedTaskType);
                if (list.Count > 0)
                {
                    foreach (var t in list)
                    {   // dont add the add custom's task buttons titles
                        if (t.Title == Constants.AppConstant.addCustomMiscellaneousTaskTextTitle ||
                            t.Title == Constants.AppConstant.addCustomProductiveTaskTextTitle ||
                            t.Title == Constants.AppConstant.addCustomUnProductiveTaskTrackId)
                        {
                            //skip to add this one
                        }
                        else
                        {
                            TaskList.Add(t.Title);
                        }
                    }
                    // need to set it (SelectedTask) to appropriate text here to propmt user otherwise select task picker title will b gone off
                    if (isDayTaskEditRequest && TaskTypeChangeAttempts == 1)
                    {// if request was for Edit day task then set it to previously assigned task title

                        SelectedTask = prevSelectedTaskTitle;
                        TaskTypeChangeAttempts++;
                    }
                    else
                    {
                        SelectedTask = "Select Task";
                        TaskTypeChangeAttempts++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
            }
        }
        /// <summary>
        /// Edit the day task
        /// Note: 'CurrentSelectedDayTask' hold the item that going to be updating
        /// CurrentSelectedDayTask item firstly delete from db then add the new task that would be same as  CurrentSelectedDayTask with new changes
        /// </summary>
        /// <param name="isEditRequest"></param>
        public void SaveTask(bool isEditRequest = false)
        {
            try
            {
                List<DayActivity> allTasks = databaseHelper.GetAllDayTasksBaseOnDayMonth(CurrentSelectedDayTask.CreatedAt);
                DateTime st = new DateTime(Convert.ToDateTime(CurrentSelectedDayTask.CreatedAt).Year, Convert.ToDateTime(CurrentSelectedDayTask.CreatedAt).Month, Convert.ToDateTime(CurrentSelectedDayTask.CreatedAt).Day, StartTime.Hour, StartTime.Minute, StartTime.Second, StartTime.Kind);
                if (isEditRequest && allTasks.Count > 1)
                { // if there are more than one item in the list then this block invoke to edit the specific item
                    bool isEditedValue = true;
                    //find the rang items(time range) in between item/task need to move to mainain the sequence
                    for (int i = 0; i < allTasks.Count; i++)
                    {
                        if (CurrentSelectedDayTask.DayTaskTrackId != allTasks[i].DayTaskTrackId) //skip the comparison for the item that was currently updating
                            if (DateTime.Compare(Convert.ToDateTime(st.ToString("hh:mm:ss tt")), Convert.ToDateTime(Convert.ToDateTime(allTasks[i].TaskStartedAt).ToString("hh:mm:ss tt"))) < 0)
                            { //if  appropriate range item found then add task between it
                                DeleteTaskBaseOnId(CurrentSelectedDayTask.DayTaskTrackId);
                                CurrentSelectedDayTask = null;
                                SaveDayTask(allTasks[i], "Up", true);
                                isEditedValue = false;
                                break;
                            }
                    }
                    int indexOFCurrentItem = allTasks.FindIndex(x => x.DayTaskTrackId == CurrentSelectedDayTask.DayTaskTrackId);
                    if (isEditedValue && indexOFCurrentItem == allTasks.Count - 1)
                    { //user is about to update bottom most task (last item). and 
                      //after updation if this task again need to be fitted bottom most (as a last most item)
                      //then this block invok
                        DeleteTaskBaseOnId(CurrentSelectedDayTask.DayTaskTrackId);
                        CurrentSelectedDayTask = null;
                        SaveDayTask(allTasks[allTasks.Count - 2], "Down", true);
                        isEditedValue = false;
                    }
                    else if (isEditedValue)//indexOFCurrentItem != allTasks.Count - 1
                    {   //if no appropriate range item found and there are more than one item in the list then
                        //needs to append task  at bottom of list
                        DeleteTaskBaseOnId(CurrentSelectedDayTask.DayTaskTrackId);
                        CurrentSelectedDayTask = null;
                        SaveDayTask(allTasks[allTasks.Count - 1], "Down", true);
                        isEditedValue = false;
                    }
                }
                else if (allTasks.Count == 1)
                {
                    PrePareNewTask();
                    if (CurrentSelectedDayTask != null) // change the date of newly created task according to the Date of the Selected (selected task mean : to up or down new task will be added)
                    {
                        NewDayTask.TaskStartedAt = new DateTime(Convert.ToDateTime(CurrentSelectedDayTask.CreatedAt).Year, Convert.ToDateTime(CurrentSelectedDayTask.CreatedAt).Month, Convert.ToDateTime(CurrentSelectedDayTask.CreatedAt).Day, StartTime.Hour, StartTime.Minute, StartTime.Second, StartTime.Kind).ToString("yyyy-MM-dd hh:mm:ss tt");
                        NewDayTask.CreatedAt = new DateTime(Convert.ToDateTime(CurrentSelectedDayTask.CreatedAt).Year, Convert.ToDateTime(CurrentSelectedDayTask.CreatedAt).Month, Convert.ToDateTime(CurrentSelectedDayTask.CreatedAt).Day, StartTime.Hour, StartTime.Minute, StartTime.Second, StartTime.Kind).ToString("yyyy-MM-dd hh:mm:ss");
                        string isUpdated = databaseHelper.AddDayTask(NewDayTask);
                        if (isUpdated != null)
                        {
                            string trackIdForPreviousTask = Settings.LatestInsertedDayTaskTrackId;
                            if (CurrentSelectedDayTask.DayTaskTrackId.Equals(trackIdForPreviousTask))
                            {
                                Settings.LatestInsertedDayTaskTrackId = isUpdated;
                            }
                            int i = databaseHelper.DeleteDayTaskByTrackId(CurrentSelectedDayTask.DayTaskTrackId);
                            //CurrentSelectedDayTask = null;
                            if (i > 0)
                            {
                                MessagingCenter.Send(this, "UpdateTask", true);
                            }
                        }
                    }
                }
                navigation.PopAllPopupAsync();
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
            }
        }
        /// <summary>
        /// to save day task from stat page on specific position
        /// 'CurrentSelectedDayTask' point to task which selected by the user from the list  with the intention to add new task according to it (up /bottom) 
        /// </summary>
        /// <param name="task"> comming task, currently selected by user; to up or down new task to be added</param>
        /// <param name="position">possible values are up or down</param>
        /// <param name="isEditRequest">is this method invoke for the updation of a task or not </param>
        /// <param name="TaskToBeDeleted">in case of updation this param we requred.this perme hold dayTas that will b deleted after the its updated form saved successfully in db </param>
        public string SaveDayTask(DayActivity task, string position = "Up", bool isEditRequest = false, DayActivity TaskToBeDeleted = null)
        {
            try
            {

                DayActivity prevTask, afterTask;
                List<DayActivity> allTasks = databaseHelper.GetAllDayTasksBaseOnDayMonth(task.CreatedAt);
                CurrentSelectedDayTask = task;   //selected task from the list (to which up/down new task to be added)                                                                         // CurrentSelectedDayTask = task;
                PrePareNewTask();
                if (task != null) // change the date of newly created task according to the Date of the Selected (selected task mean : to up or down new task will be added)
                {
                    NewDayTask.TaskStartedAt = new DateTime(Convert.ToDateTime(task.CreatedAt).Year, Convert.ToDateTime(task.CreatedAt).Month, Convert.ToDateTime(task.CreatedAt).Day, StartTime.Hour, StartTime.Minute, StartTime.Second, StartTime.Kind).ToString("yyyy-MM-dd hh:mm:ss tt");
                    NewDayTask.CreatedAt = new DateTime(Convert.ToDateTime(task.CreatedAt).Year, Convert.ToDateTime(task.CreatedAt).Month, Convert.ToDateTime(task.CreatedAt).Day, StartTime.Hour, StartTime.Minute, StartTime.Second, StartTime.Kind).ToString("yyyy-MM-dd hh:mm:ss");
                }
                // change date for the selected time
                DateTime st = new DateTime(Convert.ToDateTime(task.CreatedAt).Year, Convert.ToDateTime(task.CreatedAt).Month, Convert.ToDateTime(task.CreatedAt).Day, StartTime.Hour, StartTime.Minute, StartTime.Second, StartTime.Kind);
                string taskId = null;
                if (position.Equals("Up")) //add task above the selected task
                {

                    int selectedTaskIndex = allTasks.FindIndex(x => x.DayTaskTrackId == task.DayTaskTrackId);
                    if (selectedTaskIndex > 0)
                    {
                        prevTask = allTasks[selectedTaskIndex - 1];
                        if (DateTime.Compare(Convert.ToDateTime(st.ToString("hh:mm:ss tt")), Convert.ToDateTime(Convert.ToDateTime(prevTask.TaskStartedAt).ToString("hh:mm:ss tt"))) > 0 && DateTime.Compare(Convert.ToDateTime(st.ToString("hh:mm:ss tt")), Convert.ToDateTime(Convert.ToDateTime(task.TaskStartedAt).ToString("hh:mm:ss tt"))) < 0)
                        {
                            //in up between
                            prevTask.SpentTime = AppUtil.AppUtil.CalculateSpendedTime(Convert.ToDateTime(Convert.ToDateTime(prevTask.TaskStartedAt).ToString("hh:mm:ss tt")), Convert.ToDateTime(st.ToString("hh:mm:ss tt")));
                            databaseHelper.AddDayTask(prevTask);
                            NewDayTask.SpentTime = AppUtil.AppUtil.CalculateSpendedTime(Convert.ToDateTime(st.ToString("hh:mm:ss tt")), Convert.ToDateTime(Convert.ToDateTime(task.TaskStartedAt).ToString("hh:mm:ss tt")));
                            taskId = databaseHelper.AddDayTask(NewDayTask);
                        }
                        else
                        {
                            navigation.PopAllPopupAsync();
                            Application.Current.MainPage.DisplayAlert(AppConstant.SelectePositionToAddTaskTitleText, AppConstant.CurrentTaskIsInProgressText, "OK");
                        }
                    }
                    else if (selectedTaskIndex == 0) // add task at top most of list
                    {
                        if (DateTime.Compare(Convert.ToDateTime(st.ToString("hh:mm:ss tt")), Convert.ToDateTime(Convert.ToDateTime(task.TaskStartedAt).ToString("hh:mm:ss tt"))) < 0)
                        {
                            //inserted top most
                            NewDayTask.SpentTime = AppUtil.AppUtil.CalculateSpendedTime(Convert.ToDateTime(st.ToString("hh:mm:ss tt")), Convert.ToDateTime(Convert.ToDateTime(task.TaskStartedAt).ToString("hh:mm:ss tt")));
                            taskId = databaseHelper.AddDayTask(NewDayTask);
                        }
                        else
                        {
                            navigation.PopAllPopupAsync();
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
                        if (DateTime.Compare(Convert.ToDateTime(st.ToString("hh:mm:ss tt")), Convert.ToDateTime(Convert.ToDateTime(task.TaskStartedAt).ToString("hh:mm:ss tt"))) > 0 && DateTime.Compare(Convert.ToDateTime(st.ToString("hh:mm:ss tt")), Convert.ToDateTime(Convert.ToDateTime(afterTask.TaskStartedAt).ToString("hh:mm:ss tt"))) < 0)
                        {
                            //below of selected one task
                            //blow in between
                            task.SpentTime = AppUtil.AppUtil.CalculateSpendedTime(Convert.ToDateTime(Convert.ToDateTime(task.TaskStartedAt).ToString("hh:mm:ss tt")), Convert.ToDateTime(st.ToString("hh:mm:ss tt")));
                            databaseHelper.AddDayTask(task);
                            NewDayTask.SpentTime = AppUtil.AppUtil.CalculateSpendedTime(Convert.ToDateTime(st.ToString("hh:mm:ss tt")), Convert.ToDateTime(Convert.ToDateTime(afterTask.TaskStartedAt).ToString("hh:mm:ss tt")));
                            taskId = databaseHelper.AddDayTask(NewDayTask);
                        }
                        else
                        {
                            navigation.PopAllPopupAsync();
                            Application.Current.MainPage.DisplayAlert(AppConstant.SelectePositionToAddTaskTitleText, AppConstant.CurrentTaskIsInProgressText, "OK");
                        }
                    }
                    else if (selectedTaskIndex == allTasks.Count - 1) // inserted most bottom of list
                    {
                        if (DateTime.Compare(Convert.ToDateTime(st.ToString("hh:mm:ss tt")), Convert.ToDateTime(task.TaskStartedAt)) > 0)
                        {
                            // SaveTask();
                            DayActivity lastRecord = allTasks[selectedTaskIndex];
                            lastRecord.SpentTime = AppUtil.AppUtil.CalculateSpendedTime(Convert.ToDateTime(Convert.ToDateTime(lastRecord.TaskStartedAt).ToString("hh:mm:ss tt")), Convert.ToDateTime(st.ToString("hh:mm:ss tt")));
                            string isUpdated = databaseHelper.AddDayTask(lastRecord);
                            if (isUpdated != null)
                            {
                                taskId = databaseHelper.AddDayTask(NewDayTask);
                                Helpers.Settings.LatestInsertedDayTaskTrackId = taskId;

                            }
                        }
                        else
                        {
                            navigation.PopAllPopupAsync();
                            Application.Current.MainPage.DisplayAlert(AppConstant.SelectePositionToAddTaskTitleText, AppConstant.CurrentTaskIsInProgressText, "OK");
                        }
                    }
                    else
                    {
                        //todo
                    }
                }
                if (taskId != null)
                {

                    ArrangeTask(task.CreatedAt, position);
                    navigation.PopAllPopupAsync();
                    if (isEditRequest)
                    {// notify changes to its subscriber 
                        MessagingCenter.Send(this, "UpdateTask", true);
                    }
                    else
                    { // if task added from 'stats' page then need to broadcast 'InsertedTask'. in order to notify changes  
                        MessagingCenter.Send(this, "InsertedTask", taskId);
                    }

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
        /// Retrieve task from DB bases on id
        /// populate it to view
        /// </summary>
        /// <param name="id"></param>
        public void RetrieveAndPopulateTaskToViewBaseOnId(string id)
        {
            try
            {
                DayActivity dayTask = databaseHelper.GetDayTaskByTrackId(id);
                CurrentSelectedDayTask = dayTask;
                SelectedTaskType = dayTask.TaskType;
                SelectedTask = dayTask.TaskTitle;
                prevSelectedTaskTitle = dayTask.TaskTitle;
                isDayTaskEditRequest = true;
                StartTime = Convert.ToDateTime(dayTask.TaskStartedAt);
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
            }
        }
        /// <summary>
        /// deletes all task from db at once
        /// </summary>
        /// <returns></returns>
        public int TruncateDayTaskTable()
        {
            int deletedTasksCount = databaseHelper.DeleteAllDayTask();
            return deletedTasksCount;
        }
        /// <summary>
        /// Prepare the new day task to be going to added
        /// </summary>
        public void PrePareNewTask()
        {
            try
            {
                Taskk taskk = databaseHelper.GetTaskkBaseOnTitle(SelectedTask, SelectedTaskType);
                NewDayTask.TaskType = SelectedTaskType;
                NewDayTask.IsSelected = false;
                NewDayTask.TaskTitle = SelectedTask;
                NewDayTask.RefTaskTrackId = taskk.TaskTrackId;
                NewDayTask.BGColor = taskk.BGColor;
                NewDayTask.DayTaskTrackId = null;
                NewDayTask.UpdatedAt = AppUtil.AppUtil.CurrrentDateTimeInSqliteSupportedFormate();
                NewDayTask.TaskStartedAt = StartTime.ToString("yyyy-MM-dd hh:mm:ss tt");
                NewDayTask.CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
            }

        }

    }
}
