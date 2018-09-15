using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TimeTracker.Enums;
using Xamarin.Forms;

namespace TimeTracker.Convertors
{
    class TaskHighlighterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Color.White;
            Color returnColor = Color.White;
            var val = (TaskType)value;
            switch (val)
            {
                case TaskType.Procasination: //bed task
                    returnColor = (Color)Application.Current.Resources["ProcasinationTaskColor"];
                    break;
                case TaskType.Routine:  //good task
                                        // returnColor = Color.FromRgb(124, 252, 0);
                    returnColor = (Color)Application.Current.Resources["RoutineTaskColor"];

                    break;
                case TaskType.Miscellaneous: // normal task
                    returnColor = Color.LightGray;
                    break;

            }
            return returnColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
