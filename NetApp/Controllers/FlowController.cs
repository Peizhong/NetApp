using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetApp.Models;
using NetApp.Workflow;
using Newtonsoft.Json;

namespace NetApp.Controllers
{
    public class FlowController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        
        public FlowController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // GET: Flow
        public ActionResult Index()
        {
            return View();
        }

        // GET: Flow/Create
        public ActionResult Create()
        {
            var flowConfig = WorkflowFactory.Instance.CreateWorkflow("test", "workflows/flowdemo.json", _serviceProvider);
            Message hello = new Message
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Hi",
                Value = "World",
                Status = 0
            };
            flowConfig.MoveOn("CreateOrder", JsonConvert.SerializeObject(hello));
            return View();
        }

        // POST: Flow/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Flow/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Flow/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}