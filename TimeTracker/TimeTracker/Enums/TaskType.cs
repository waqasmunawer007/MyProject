using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.Enums
{
    /// <summary>
    ///  Routine = 1 --> general/rotine task /good
    /// Procasination  = 2  --> bed task
    /// Miscellaneous = 3  -->normal tasks
    /// </summary>
    public enum TaskType
    {
        None = 0, Routine = 1, Procasination = 2, Miscellaneous = 3
    }
}
