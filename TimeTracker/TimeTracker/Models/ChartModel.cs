using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.Models
{
    public class ChartModel
    {
        public string Name { get; set; }

        public double Height { get; set; }
        /// <summary>
        /// hack field just to differentiat which type of data contain Productive/Unproductive/ Misc. (used for ColumnStackBar click event)
        /// </summary>
        public string Type { get; set; }
        public string Week { get; set; }
        public ChartModel(string TaskType="", double height=0, string type = "dummy", string week = "Week 1")
        {
            this.Name = TaskType;
            this.Height = height;
            this.Type = type;
            this.Week = week;
        }
    }
}
