using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
//using Xam.Plugins.Settings package
namespace TimeTracker.Helpers
{
    /// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }
        #region Setting Constants
        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;
        private static readonly bool IsDumpInitialTaskInSqliteDefault = false;
        private const string RunningTaskIdKey = "running_task_id";
        private const string RecentAddedTaskFromStatIdKey = "stat_running_task_id";
        private const string LatestInsertedDayTaskKey = "latest_inserted_day_task_track_id";
        private const string IsDumpInitialTaskInSqliteKey = "is_initial_task_dumped_local_db";
       #endregion
        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }
        public static int RunningTaskId
        {
            get
            {
                return AppSettings.GetValueOrDefault(RunningTaskIdKey, 0);
            }
            set
            {
                AppSettings.AddOrUpdateValue(RunningTaskIdKey, value);
            }
        }
        public static int StatTaskId
        {
            get
            {
                return AppSettings.GetValueOrDefault(RecentAddedTaskFromStatIdKey, 0);
            }
            set
            {
                AppSettings.AddOrUpdateValue(RecentAddedTaskFromStatIdKey, value);
            }
        }
        public static bool IsDataDumpedInLocalDB
        {
            get
            {
                return AppSettings.GetValueOrDefault(IsDumpInitialTaskInSqliteKey, IsDumpInitialTaskInSqliteDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(IsDumpInitialTaskInSqliteKey, value);
            }
        }
        public static string LatestInsertedDayTaskTrackId
        {
            get
            {
                return AppSettings.GetValueOrDefault(LatestInsertedDayTaskKey, null);
            }
            set
            {
                AppSettings.AddOrUpdateValue(LatestInsertedDayTaskKey, value);
            }
        }
    }
}
