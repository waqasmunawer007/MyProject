using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.Constants
{
    public static class AppConstant
    {
        #region email
        public const int ErrorEmailPort = 587;
        public const int ErrorEmailTimeout = 10000;
        public const string ErrorEmailHost = "smtp.gmail.com";
        public static string ErrorEmailAddress = "claritycrashreports@gmail.com";
        public static string ErrorEmailPassword = "Test4321!";
        #endregion


        //App Ui constants
        public static string RoutineTaskText = "Task";
        public static string ProcasinationTaskText = "Procasination";
        public static string MiscellaneousTaskText = "Break/Miscellaneous";
        public static string FinishDayText = "Finish the day";
        public static string CancelDayText = "Cancel the day";
        public static string CancelDayAlertTitle = "Delete";
        public static string ConformationMessagetoAddTask = "Are you sure you want to add this task?";
        public static string CancelDayAlertBodyText = "All tasks will be deleted";
        public static string NotFoundTaskForCancelMessage = "There is no task to cancel";
        public static string GeneralAlertTitle = "Message";
        public static string DeleteAlertTitle = "Delete";
        public static string SucessMessageForDeletedTasks = "All tasks have been deleted";
        public static string SucessMessageForSingleTasks = "Task has been deleted";
        public static string DeleteConformationText = "Do you really want to delete it?";
        public static string AlertBodyTextForDeleteDay = "Do you really want to delete all tasks of ";
        public static string FinishDayFailureAlertBodyText = "There must be at least one task to finish a day";
        public static string CurrentTaskIsInProgressText = "invalid time";
        public static string SelectePositionToAddTaskMessageBodyText = "Please select a position where you want to add a task.";
        public static string SelectePositionToAddTaskTitleText = "Oops!";
       
        #region Toast Constant

        public static string SuccessMessageTitleForToast = "Success";
        public static string FailureMessageTitleForToast = "Error";
        public static string DeletionFailureMessageBody = "Delete operation is failed.Please try again.";
        public static string UpdateFailureMessageBody = "Update operation is failed.Please try again.";
        public static string SuccessUpdationMessageBodyForToast = "The tash has been updated successfully!";
        public static string SuccessfulInsertedTaskMessageBodyForToast = "New task has been inserted successfully!";
        public const string ErrorEvent = "ErrorOccurred";
        public const string ErrorText = "An error has occurred and has been logged for review. We apologize for the inconvenience.";
        #endregion
        public static string AtWhichPositionYouWantAddTask = "Where to add task from selected position?";
        public static string AtBottomOfSelectedTask = "Bottom";
        public static string AtAboveOfSelectedTask = "Above";
        ////
        public static string addCustomProductiveTaskTextTitle = "Add Custom Task";
        public static string addCustomUnProductiveTaskTextTitle = "Add Custom Task";
        public static string addCustomMiscellaneousTaskTextTitle = "Add Custom Task";
        public static string AlreadyTaskExist = "Task Already Exist";
        public static string addCustomProductiveTaskTrackId = "AddProductive100";
        public static string addCustomUnProductiveTaskTrackId = "UnAddProductive101";
        public static string addCustomMiscellaneousTaskTrackId = "Miscellaneous103";
        public static string ProductiveTaskType = "Productive Tasks";
        public static string UnProductiveTaskType = "Unproductive Tasks";
        public static string MiscellaneousTaskType = "Miscellaneous Tasks";
        public static string ProductiveChartTitle = "Productive";
        public static string UnProductiveChartTitle = "Unproductive";
        public static string MiscellaneousChartTitle = "Miscellaneous";
        //TaskType Colors
        public static string ProductiveTaskColor = "#5c6bc0";
        public static string ProductiveSelectiveTaskColor = "#b0fc0e";
        public static string UnProductiveTaskColor = "#ef5350";
        public static string UnProductiveSelectiveTaskColor = "#990d1b";
        public static string MiscellaneousTaskColor = "#9e9e9e"; //like gray
        public static string MiscellaneousSelectiveTaskColor = "#9e9e9e";// like dim gray
        public static string MonthlyChartLable = "Monthly Progress";
        public static string WeekelyChartLable = "Weekely Progress";
        public static string DailyChartLable = "Day Progress";
        //Chart Types
        public static string PieChartTypeLable = "Pie View";
        public static string BarChartTypeLable = "Bar View";
        //Task stats page
        public static string StatsToolbarAddNewTaskButton = "Add";
        public static string StatsToolbargraphsButton = "Stats";
    }
}
