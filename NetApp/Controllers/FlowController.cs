using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetApp.Models;
using NetApp.Workflow;
using NetApp.Workflow.Models;
using Newtonsoft.Json;

namespace NetApp.Controllers
{
    public class FlowController : Controller
    {
        private readonly WorkflowFactory _workflowFactory;
        
        public FlowController(WorkflowFactory workflowFactory)
        {
            _workflowFactory = workflowFactory;
        }

        // GET: Flow
        public ActionResult Index()
        {
            return View();
        }

        // GET: Flow/Create
        public async Task<ActionResult> Create(string flowId)
        {
            Flow workflow = null;
            if (string.IsNullOrEmpty(flowId))
            {
                workflow = _workflowFactory.CreateWorkflow("test", "workflows/flowdemo.json");
            }
            else
            {
                workflow = await _workflowFactory.FindWorkflow(flowId);
            }
            Message hello = new Message
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Hi",
                Value = "World",
                Status = 0
            };
            workflow?.MoveOn("NetApp.Workflows.EditOrderNode, NetApp", "CreateOrder", JsonConvert.SerializeObject(hello));
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