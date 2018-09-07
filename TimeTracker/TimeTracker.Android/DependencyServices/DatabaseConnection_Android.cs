using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using TimeTracker.Droid.DependencyServices;
using TimeTracker.Interfaces;
//sqlite-net-pcl package
[assembly: Xamarin.Forms.Dependency(typeof(DatabaseConnection_Android))]
namespace TimeTracker.Droid.DependencyServices
{
    public class DatabaseConnection_Android : IDatabaseConnection
    {
        SQLiteConnection IDatabaseConnection.DbConnection()
        {
            var dbName = "IAmProductiveDB.db3";
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbName);
            if (!File.Exists(path))
            {

            }

            return new SQLiteConnection(path);
        }
    }
}