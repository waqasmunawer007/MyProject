using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Constants;
using TimeTracker.Models;
using TimeTracker.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateActivityCalenderPageView : ContentPage
    {
        public CreateActivityCalenderPageView(Taskk selectedTask)
        {
            InitializeComponent();
            try
            {
                calender.MaxDate = DateTime.Now.AddDays(0);
                calender.MinDate = DateTime.Now.AddDays(-1);
                calender.Padding = new Thickness(5, Device.RuntimePlatform == Device.iOS ? 25 : 5, 5, 5);
                DayActivityViewModel dayActivityViewModel= new DayActivityViewModel();
                BindingContext = dayActivityViewModel;
                dayActivityViewModel.CurrentSelectedTaskk = selectedTask;
                dayActivityViewModel.OpenTaskStatsViewPageHandler += OpenTaskStatsViewPage;
            }
            catch (Exception ex)
            {
                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, AppConstant.ErrorEvent, ex.ToString());
            }
        }
        /// <summary>
        /// open the Task stats page 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OpenTaskStatsViewPage(object sender, EventArgs e)
        {
            string d = sender as string;
            if (d != null)
            {
                await DisplayAlert("Current date", "" + d, "cancel");
                //  Navigation.PushAsync(new DayActivitysPageView(d));
                // Navigation.RemovePage(this);
            }
        }
        private async void calender_DateClicked(object sender, XamForms.Controls.DateTimeEventArgs e)
        {
            //todo
            await DisplayAlert("Current date", "" + calender.SelectedDate, "cancel");
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
    }
}