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
        private async void ShowCalenderPage(object sender, EventArgs e)
        {
            var selectedTask = (Taskk)sender;
            await DisplayAlert("", "I am click", "OK");
            // Navigation.PushAsync(new CalenderPageView(selectedTask));
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
             await  DisplayAlert("", "I am click", "OK");
            //await Navigation.PushPopupAsync(new AddAndEditTasksPopUpViewPage(selectedTask.TaskTrackId));
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
            await DisplayAlert("", "I am click", "OK");
           // await Navigation.PushPopupAsync(new AddAndEditTasksPopUpViewPage(selectedItemTrackId, true));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

    }
}