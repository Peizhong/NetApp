using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetApp.Services.Lib.Attributes
{
    public class MyUser
    {
        public string Id { get; set; }

        public string UserName { get; set; }
    }

    public class MyAttribute : ActionFilterAttribute
    {
        public static MyUser MyUser;
        public MyUser OneUser;

        public MyAttribute()
        {

        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //do something before
            var result = await next();
            //do something after
        }
    }
}
