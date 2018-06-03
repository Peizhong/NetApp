using System;
using System.Collections.Generic;
using System.Text;
using NetApp.Entities.LearningLog;

namespace NetApp.Business.Interfaces
{
    public interface ILogsApp
    {
        IEnumerable<Entry> GetUserEntries(User user);
    }
}