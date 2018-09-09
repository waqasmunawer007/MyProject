using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Models;
using TimeTracker.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamvvm;
/// <summary>
/// 1)Open page For Editting the Particular task
/// 2)Open Page for Adding Custom new Task
/// 3)Open the Calender page to add new Day Task
/// 4)Show the All task (Front page For AddTask's Tab)
/// </summary>
namespace TimeTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskPage : ContentPage, IBasePage<TaskViewModel>
    {
        TaskViewModel taskViewModel;
        public TaskPage()
        {
            InitializeComponent();
            taskViewModel = new TaskViewModel();
            BindingContext = taskViewModel;
            taskViewModel.OpenAddTaskPopupViewHandler += ShowAddTaskPopUpAsync; //handler to open the add task popup page
            taskViewModel.OpenCalenderForStatsHandler += ShowCalenderPage;
        }
        private void ShowCalenderPage(object sender, EventArgs e)
        {
            var selectedTask = (Taskk)sender;
             Navigation.PushAsync(new CreateActivityCalenderPageView(selectedTask));
        }
        /// <summary>
        /// open the popup page to add new task
        /// pass the Id of Selected task as paprameter to to tell the AddTaskPopup page which type of task it need to insert
        /// </summary>
        /// <param name="sender">Clicked task </param>
        /// <param name="e"></param>
        private async void ShowAddTaskPopUpAsync(object sender, EventArgs e)
        {
            Taskk selectedTask = sender as Taskk;
            await Navigation.PushPopupAsync(new AddEditTaskkPopupPage(selectedTask.TaskTrackId));
        }
        /// <summary>
        /// Edit task button handler
        /// Edit the particular task from 'Add task' page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void EditGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var selectedItemTrackId = ((Image)sender).ClassId.ToString();
           await Navigation.PushPopupAsync(new AddEditTaskkPopupPage(selectedItemTrackId, true));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<AddEditTaskkPopUpViewModel, bool>(this, "UpdateTask", (sender, arg) =>
            {
                taskViewModel.SortedAndSetTheTaskList();
                taskViewModel.HighlightTaskAfterNewTaskInserted();

            });
            MessagingCenter.Subscribe<AddEditTaskkPopUpViewModel, bool>(this, "InsertedTask", (sender, arg) =>
            {

                taskViewModel.SortedAndSetTheTaskList();
                taskViewModel.HighlightTaskAfterNewTaskInserted();

            });
            MessagingCenter.Subscribe<AddEditTaskkPopUpViewModel, bool>(this, "Can'tInserTask", (sender, arg) =>
            {
                //todo

            });
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<AddEditTaskkPopUpViewModel, bool>(this, "UpdateTask");
            MessagingCenter.Unsubscribe<AddEditTaskkPopUpViewModel, bool>(this, "InsertedTask");
            MessagingCenter.Unsubscribe<AddEditTaskkPopUpViewModel, bool>(this, "Can'tInserTask");
        }

    }
}