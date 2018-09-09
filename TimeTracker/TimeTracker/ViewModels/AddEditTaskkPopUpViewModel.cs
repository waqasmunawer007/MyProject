using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.Constants;
using TimeTracker.Models;
using Xamarin.Forms;

namespace TimeTracker.ViewModels
{
    public class AddEditTaskkPopUpViewModel : BaseViewModel
    {
        private INavigation navigation;
        private Taskk task; // new task going to be added
        public AddEditTaskkPopUpViewModel(INavigation nav)
        {
            navigation = nav;
            task = new Taskk();
        }
        public void SaveTask()
        {
            try
            {
                List<Taskk> list = null;
                if (Task.TaskTrackId != Constants.AppConstant.addCustomMiscellaneousTaskTrackId && Task.TaskTrackId != Constants.AppConstant.addCustomProductiveTaskTrackId && Task.TaskTrackId != Constants.AppConstant.addCustomUnProductiveTaskTrackId && Task.TaskTrackId != null)
                {
                    //up date task
                    var s = databaseHelper.GetTaskkBaseOnTitle(Task.Title, Task.TaskType);
                    if (s == null)
                    {
                        int isInserted = databaseHelper.AddTaskk(Task);
                    }
                    else
                    {    // task already exist
                        Application.Current.MainPage.DisplayAlert("", AppConstant.AlreadyTaskExist, "OK");
                    }

                }
                else if (Task.TaskTrackId == Constants.AppConstant.addCustomProductiveTaskTrackId)
                {
                    //insert new UnProductive task
                    list = databaseHelper.GetAllTaskks();
                    for (int j = 0; j < list.Count; j++)
                    {
                        Taskk addTask = list[j];
                        if (addTask.TaskTrackId.Equals(Constants.AppConstant.addCustomProductiveTaskTrackId))
                        {
                            var s = databaseHelper.GetTaskkBaseOnTitle(Task.Title, Task.TaskType);
                            if (s == null)
                            {
                                list.Insert(j, new Taskk() { TaskTrackId = Guid.NewGuid().ToString(), Title = Task.Title, TaskType = Task.TaskType, BGColor = Task.BGColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                                break;
                            }
                            else
                            {   // task already exist
                                Application.Current.MainPage.DisplayAlert("", AppConstant.AlreadyTaskExist, "OK");
                                break;
                            }
                        }
                    }
                }
                else if (Task.TaskTrackId == Constants.AppConstant.addCustomUnProductiveTaskTrackId)
                {
                    //insert new UnProductive task
                    list = databaseHelper.GetAllTaskks();
                    for (int j = 0; j < list.Count; j++)
                    {
                        Taskk addTask = list[j];
                        if (addTask.TaskTrackId.Equals(Constants.AppConstant.addCustomUnProductiveTaskTrackId))
                        {
                            var s = databaseHelper.GetTaskkBaseOnTitle(Task.Title, Task.TaskType);
                            if (s == null)
                            {
                                list.Insert(j, new Taskk() { TaskTrackId = Guid.NewGuid().ToString(), Title = Task.Title, TaskType = Task.TaskType, BGColor = Task.BGColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                                break;
                            }
                            else
                            {   //already exist
                                Application.Current.MainPage.DisplayAlert("", AppConstant.AlreadyTaskExist, "OK");
                                break;
                            }
                        }
                    }

                }
                else if (Task.TaskTrackId == Constants.AppConstant.addCustomMiscellaneousTaskTrackId)
                {
                    //insert new Miscellaneous task
                    list = databaseHelper.GetAllTaskks();
                    for (int j = 0; j < list.Count; j++)
                    {
                        Taskk addTask = list[j];
                        if (addTask.TaskTrackId.Equals(Constants.AppConstant.addCustomMiscellaneousTaskTrackId))
                        {
                            var s = databaseHelper.GetTaskkBaseOnTitle(Task.Title, Task.TaskType);
                            if (s == null)
                            {
                                list.Insert(j, new Taskk() { TaskTrackId = Guid.NewGuid().ToString(), Title = Task.Title, TaskType = Task.TaskType, BGColor = Task.BGColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                                break;
                            }
                            else
                            {  //already exist
                                Application.Current.MainPage.DisplayAlert("", AppConstant.AlreadyTaskExist, "OK");
                                break;
                            }
                        }
                    }
                }
                else
                {
                    //to do; add button handling for forth button
                }
                if (Task.TaskTrackId != Constants.AppConstant.addCustomMiscellaneousTaskTrackId &&
                    Task.TaskTrackId != Constants.AppConstant.addCustomProductiveTaskTrackId &&
                    Task.TaskTrackId != Constants.AppConstant.addCustomUnProductiveTaskTrackId &&
                    Task.TaskTrackId != null)
                {
                    //broadcast the update task message
                    MessagingCenter.Send(this, "UpdateTask", true);
                }
                else
                { //broadcast the new task inserted message
                    databaseHelper.DeleteAllTasks_Taskk();
                    int i = databaseHelper.InsertedGroupOfTaskks(list);
                    if (i > 0)
                    {
                        MessagingCenter.Send(this, "InsertedTask", true);
                    }

                }
                navigation.PopAllPopupAsync();

            }catch(Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString()); 
            }
       }
        /// <summary>
        /// get task base on taskk track id
        /// </summary>
        /// <param name="trackId"></param>
        /// <returns></returns>
        public Taskk GetTaskBaseOnTrackID(string trackId)
        {
            return databaseHelper.GetTaskBasesOnSpecificId(trackId);
        }

        public Taskk Task { get { return task; } set { this.task = value; OnPropertyChanged("Task"); } } // current task to be 
    }
}
