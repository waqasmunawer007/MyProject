using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeTracker.Constants;
using TimeTracker.Helpers;
using TimeTracker.Interfaces;
using TimeTracker.Models;
using Xamarin.Forms;

namespace TimeTracker.Database
{
    public class DatabaseHelper
    {
        List<Taskk> initTaskksData;
        private SQLiteConnection database;
        private static DatabaseHelper databaseHelper;
        private static object collisionLock = new object();
        private DatabaseHelper()
        {
            database = DependencyService.Get<IDatabaseConnection>().DbConnection();

        }
        /// <summary>
        /// return the DatabaseHelper instance (singleton pattern)
        /// </summary>
        public static DatabaseHelper GetInstance()
        {
            if (databaseHelper == null)
            {
                databaseHelper = new DatabaseHelper();
            }
            return databaseHelper;
        }
        /// <summary>
        /// Creates Tables in Database if not exist
        /// </summary>
        public void CreateDatabase()
        {
            try
            {
                database.CreateTable<Taskk>();
               // database.Query<DayTask>("CREATE TABLE if not exists `DayTask` (`Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,`DayTaskTrackId` TEXT,`TaskTitle` TEXT,`TaskType`    TEXT,`TaskStartedAt` TEXT,`SpentTime` TEXT,`CreatedAt` TEXT,`UpdatedAt` TEXT,`BGColor` TEXT,`IsSelected` INTEGER,`RefTaskTrackId` TEXT);");
                DumpInitialDataInTaskkLocalDB();
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
            }
        }
        /// <summary>
        /// Some initial task
        /// </summary>
        public void DumpInitialDataInTaskkLocalDB()
        {
            if (Settings.IsDataDumpedInLocalDB == false)
            {

                initTaskksData = new List<Taskk>();
                //productive task
                initTaskksData.Add(new Taskk() { TaskTrackId = Guid.NewGuid().ToString(), Title = "Excercise", TaskType = AppConstant.ProductiveTaskType, BGColor = AppConstant.ProductiveTaskColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                initTaskksData.Add(new Taskk() { TaskTrackId = Guid.NewGuid().ToString(), Title = "Studying", TaskType = AppConstant.ProductiveTaskType, BGColor = AppConstant.ProductiveTaskColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                initTaskksData.Add(new Taskk() { TaskTrackId = Guid.NewGuid().ToString(), Title = "HomeWork", TaskType = AppConstant.ProductiveTaskType, BGColor = AppConstant.ProductiveTaskColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                initTaskksData.Add(new Taskk() { TaskTrackId = Guid.NewGuid().ToString(), Title = "Reading", TaskType = AppConstant.ProductiveTaskType, BGColor = AppConstant.ProductiveTaskColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                initTaskksData.Add(new Taskk() { TaskTrackId = Guid.NewGuid().ToString(), Title = "Work", TaskType = AppConstant.ProductiveTaskType, BGColor = AppConstant.ProductiveTaskColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                initTaskksData.Add(new Taskk() { TaskTrackId = Guid.NewGuid().ToString(), Title = "Meditation", TaskType = AppConstant.ProductiveTaskType, BGColor = AppConstant.ProductiveTaskColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                initTaskksData.Add(new Taskk() { TaskTrackId = AppConstant.addCustomProductiveTaskTrackId, Title = AppConstant.addCustomProductiveTaskTextTitle, TaskType = AppConstant.ProductiveTaskType, BGColor = AppConstant.ProductiveTaskColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                //un productive task
                initTaskksData.Add(new Taskk() { TaskTrackId = Guid.NewGuid().ToString(), Title = "Partying", TaskType = AppConstant.UnProductiveTaskType, BGColor = AppConstant.UnProductiveTaskColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                initTaskksData.Add(new Taskk() { TaskTrackId = Guid.NewGuid().ToString(), Title = "Web Surfing", TaskType = AppConstant.UnProductiveTaskType, BGColor = AppConstant.UnProductiveTaskColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                initTaskksData.Add(new Taskk() { TaskTrackId = Guid.NewGuid().ToString(), Title = "Smoking", TaskType = AppConstant.UnProductiveTaskType, BGColor = AppConstant.UnProductiveTaskColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                initTaskksData.Add(new Taskk() { TaskTrackId = Guid.NewGuid().ToString(), Title = "Emails", TaskType = AppConstant.UnProductiveTaskType, BGColor = AppConstant.UnProductiveTaskColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                initTaskksData.Add(new Taskk() { TaskTrackId = Guid.NewGuid().ToString(), Title = "Watching TV", TaskType = AppConstant.UnProductiveTaskType, BGColor = AppConstant.UnProductiveTaskColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                initTaskksData.Add(new Taskk() { TaskTrackId = Guid.NewGuid().ToString(), Title = "Playing", TaskType = AppConstant.UnProductiveTaskType, BGColor = AppConstant.UnProductiveTaskColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                initTaskksData.Add(new Taskk() { TaskTrackId = AppConstant.addCustomUnProductiveTaskTrackId, Title = AppConstant.addCustomUnProductiveTaskTextTitle, TaskType = AppConstant.UnProductiveTaskType, BGColor = AppConstant.UnProductiveTaskColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                //miscellaneous task
                initTaskksData.Add(new Taskk() { TaskTrackId = Guid.NewGuid().ToString(), Title = "Eating", TaskType = AppConstant.MiscellaneousTaskType, BGColor = AppConstant.MiscellaneousTaskColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                initTaskksData.Add(new Taskk() { TaskTrackId = Guid.NewGuid().ToString(), Title = "Short Break", TaskType = AppConstant.MiscellaneousTaskType, BGColor = AppConstant.MiscellaneousTaskColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                initTaskksData.Add(new Taskk() { TaskTrackId = Guid.NewGuid().ToString(), Title = "Long Break", TaskType = AppConstant.MiscellaneousTaskType, BGColor = AppConstant.MiscellaneousTaskColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                initTaskksData.Add(new Taskk() { TaskTrackId = Guid.NewGuid().ToString(), Title = "Shopping", TaskType = AppConstant.MiscellaneousTaskType, BGColor = AppConstant.MiscellaneousTaskColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                initTaskksData.Add(new Taskk() { TaskTrackId = Guid.NewGuid().ToString(), Title = "Birthday", TaskType = AppConstant.MiscellaneousTaskType, BGColor = AppConstant.MiscellaneousTaskColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                initTaskksData.Add(new Taskk() { TaskTrackId = Guid.NewGuid().ToString(), Title = "Bathroom", TaskType = AppConstant.MiscellaneousTaskType, BGColor = AppConstant.MiscellaneousTaskColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                initTaskksData.Add(new Taskk() { TaskTrackId = AppConstant.addCustomMiscellaneousTaskTrackId, Title = AppConstant.addCustomMiscellaneousTaskTextTitle, TaskType = AppConstant.MiscellaneousTaskType, BGColor = AppConstant.MiscellaneousTaskColor, IsSelected = false, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                InsertedGroupOfTaskks(initTaskksData);
                Settings.IsDataDumpedInLocalDB = true;
            }
        }
        //---------------Taskk Table--------------
        public int InsertedGroupOfTaskks(List<Taskk> list)
        {
            try
            {
                lock (collisionLock)
                {
                    int recentAddedEnterdTask = 0;
                    recentAddedEnterdTask = database.InsertAll(list);
                    return recentAddedEnterdTask;
                }
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
                return 0;
            }
        }
        /// <summary>
        /// insert new Taskk to DB
        /// or Update the Taskk
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public int AddTaskk(Taskk task)
        {
            int recentAddedEnterdTask = 0;
            try
            {
                lock (collisionLock)
                {
                    if (task.TaskTrackId != null) //update record
                    {
                        recentAddedEnterdTask = database.Update(task);
                        return recentAddedEnterdTask;
                    }
                    else
                    { // enter as new entry
                        recentAddedEnterdTask = database.Insert(task);
                        return recentAddedEnterdTask;
                    }
                }
            }
            catch (Exception ex)
            {
               // MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
                return 0;
            }
        }
        /// <summary>
        /// return taskk base on task title and type
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Taskk GetTaskkBaseOnTitle(string title, string type)
        {
            try
            {
                lock (collisionLock)
                {
                    if (title != null)
                    {
                        //var s1 = database.Table<Taskk>().First(x => x.Title.Equals(title) && x.TaskType.Equals(type));
                        // var s = database.Query<Taskk>("SELECT * FROM Taskk WHERE Title='"+title+"'");
                        var s = database.Query<Taskk>("SELECT * FROM Taskk WHERE UPPER(Title) = UPPER('" + title + "') AND TaskType ='" + type + "'");
                        if (s.Count > 0)
                        {
                            return s.First();
                        }
                        return null;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// return All Taskks
        /// </summary>
        /// <returns></returns>
        public List<Taskk> GetAllTaskks()
        {
            try
            {
                lock (collisionLock)
                {

                    return database.Table<Taskk>().ToList();
                }
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// return all taskks bases on specific type
        /// </summary>
        /// <param name="taskkType"></param>
        /// <returns></returns>
        public List<Taskk> GetAllTaskksBaseOnTaskType(string taskkType)
        {
            try
            {
                lock (collisionLock)
                {
                    var s = database.Table<Taskk>().Where(x => x.TaskType.Equals(taskkType)).ToList();
                    return s;
                }
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// return Taskk base on ID
        /// </summary>
        /// <param name="trackId"></param>
        /// <returns></returns>
        public Taskk GetTaskBasesOnSpecificId(string trackId)
        {
            try
            {
                lock (collisionLock)
                {
                    if (trackId != null)

                    {
                        Taskk task = database.Table<Taskk>().First(x => x.TaskTrackId == trackId);
                        return task;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// delete All taskks
        /// </summary>
        /// <returns></returns>
        public int DeleteAllTasks_Taskk()
        {
            try
            {
                var countOfDeletedRecord = 0;
                lock (collisionLock)
                {
                  countOfDeletedRecord = database.DeleteAll<Taskk>();
                }
                return countOfDeletedRecord;
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
                return 0;
            }
        }
        /// <summary>
        /// return the latest one inserted taskk
        /// </summary>
        /// <returns></returns>
        public Taskk GetLastlyInsertedTask_Taskk()
        {
            try
            {
                lock (collisionLock)
                {
                   Taskk task = database.Table<Taskk>().Last();
                   return task;
                }
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
                return null;
            }

        }
        //public void ABC()
        //{
        //    Taskk t = new Taskk();
        //    // t.CreatedAt
        //    //t.CreatedAt
        //    string d = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        //    DateTime dt = Convert.ToDateTime(d);
        //    var s = database.Query<Taskk>("SELECT * FROM Taskk WHERE strftime('%m', CreatedAt) IN ('02', '01')");
        //}
    }
}
