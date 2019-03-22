using NetApp.PlayASP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetApp.PlayASP.Abstract
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products();
    }
}