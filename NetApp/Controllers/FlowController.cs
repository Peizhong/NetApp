using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetApp.Workflow;
using NetApp.Workflows;

namespace NetApp.Controllers
{
    public class FlowController : Controller
    {
        // GET: Flow
        public ActionResult Index()
        {
            return View();
        }

        // GET: Flow/Create
        public ActionResult Create()
        {
            var demoflow = WorkflowFactory.Instance.CreateWorkflow("aa", "workflows/flowdemo.json");
            var entrance = Activator.CreateInstance(Type.GetType(demoflow.EntranceNodeType));

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