using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

//sqlite-net-pcl package
namespace TimeTracker.Interfaces
{
    public interface IDatabaseConnection
    {
        SQLiteConnection DbConnection();
    }
}
