using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Entities.Interfaces
{
    public interface IQuery
    {
        string Id { get; set; }

        string Name { get; set; }
    }
}
