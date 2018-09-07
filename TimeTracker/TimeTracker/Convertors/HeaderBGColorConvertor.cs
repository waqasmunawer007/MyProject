using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TimeTracker.Constants;
using Xamarin.Forms;

namespace TimeTracker.Convertors
{
    public class HeaderBGColorConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string taskType = (string)value;
                if (taskType.Equals(AppConstant.ProductiveTaskType))
                {
                    return AppConstant.ProductiveTaskColor;
                }
                else if (taskType.Equals(AppConstant.UnProductiveTaskType))
                {
                    return AppConstant.UnProductiveTaskColor;
                }
                else if (taskType.Equals(AppConstant.MiscellaneousTaskType))
                {
                    return AppConstant.MiscellaneousTaskColor;
                }
                else
                {
                    return "#7E313B";
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
