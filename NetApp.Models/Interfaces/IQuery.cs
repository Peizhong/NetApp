using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Models.Interfaces
{
    public interface IQuery
    {
        string Id { get; }

        string Name { get; }
        
        int DataStatus { get; }
    }
}
