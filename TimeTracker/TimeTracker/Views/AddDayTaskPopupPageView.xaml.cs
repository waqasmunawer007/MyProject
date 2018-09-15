using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Models;
using TimeTracker.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddDayTaskPopupPageView : PopupPage
    {
        AddDayActivityPopupViewModel addDayTaskPopupViewModel;
        public string insertedTaskPosition = " ";
        bool isDayTaskEditRequest;
        DayActivity SelectedDayTask;
        /// <summary>
        /// Add task from Stats page
        /// </summary>
        /// <param name="dayTask"></param>
        /// <param name="position"></param>
        public AddDayTaskPopupPageView (DayActivity dayTask, string position = "Up")
        {
			InitializeComponent ();
            addDayTaskPopupViewModel = new AddDayActivityPopupViewModel(Navigation);
            BindingContext = addDayTaskPopupViewModel;
            SelectedDayTask = dayTask;
            insertedTaskPosition = position;
            isDayTaskEditRequest = false;
            addDayTaskPopupViewModel.SelectedTask = "Select Task";
            addDayTaskPopupViewModel.SelectedTaskType = "Select Task Type";
        }
        /// <summary>
        /// Edit task Request
        /// </summary>
        /// <param name="trackId"></param>
        /// <param name="isEditRequest"></param>
        public AddDayTaskPopupPageView(string trackId, bool isEditRequest = false)
        {
            InitializeComponent();
            addDayTaskPopupViewModel = new AddDayActivityPopupViewModel(Navigation);
            BindingContext = addDayTaskPopupViewModel;
            addDayTaskPopupViewModel.RetrieveAndPopulateTaskToViewBaseOnId(trackId);
            isDayTaskEditRequest = isEditRequest;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        protected override bool OnBackButtonPressed()
        {
            // Prevent hide popup
            return base.OnBackButtonPressed();
            // return true;
        }
        // Invoced when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return default value - CloseWhenBackgroundIsClicked
            //return base.OnBackgroundClicked();
            CloseAllPopup();
            return false;
        }

        //  Invoked when close button tapped close all popup,s
        private void OnCloseButton_Tapped(object sender, EventArgs e)
        {
            CloseAllPopup();
        }
        // close all open popup,s
        private async void CloseAllPopup()
        {
            await Navigation.PopAllPopupAsync();
        }
        //on selection of any task type from type picker; retrieve appropriate tasks against the type and set to the task picker
        private void TypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            addDayTaskPopupViewModel.GetAllTTaskAgainstTaskType();
        }
        //show out the task Type picker
        private void TypePickerButton_Clicked(object sender, EventArgs e)
        {
            //show the picker for month selection
            TaskTypePicker.Focus();
        }
        //show out the task  picker
        private void TaskPickerButton_Clicked(object sender, EventArgs e)
        {
            //show the picker for month selection
            TaskPicker.Focus();
        }
        private void SaveButton_Clicked(object sender, EventArgs e)
        {

            if (insertedTaskPosition.Equals("Up") || insertedTaskPosition.Equals("Down"))
            { //add new task
                addDayTaskPopupViewModel.SaveDayTask(SelectedDayTask, insertedTaskPosition);

            }
            else
            { //edit task
                addDayTaskPopupViewModel.SaveTask(isDayTaskEditRequest);
            }

            isDayTaskEditRequest = false;
        }

    }
}