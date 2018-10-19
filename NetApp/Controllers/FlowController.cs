using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetApp.Models;
using NetApp.Workflow;
using NetApp.Workflow.Models;
using Newtonsoft.Json;

namespace NetApp.Controllers
{
    public class FlowController : Controller
    {
        private readonly ILogger<FlowController> _logger;
        private readonly WorkflowFactory _workflowFactory;
        private readonly NetAppDbContext _netAppDbContext;

        public FlowController(
            ILogger<FlowController> logger,
            NetAppDbContext netAppDbContext,
            WorkflowFactory workflowFactory)
        {
            _logger = logger;
            _netAppDbContext = netAppDbContext;
            _workflowFactory = workflowFactory;
        }

        // GET: Flow
        public async Task<ActionResult> Index()
        {
            //var mx = await _netAppDbContext.Messages.FromSql("select m.*, w.WorkflowId from messages m join WorkflowRefs w on m.Id = w.objectId").ToListAsync();
            var messages = await _netAppDbContext.Messages.ToListAsync();
            return View(messages);
        }

        // GET: Flow/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Flow/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            Flow workflow = _workflowFactory.CreateWorkflow("test", "workflows/flowdemo.json");
            Message hello = new Message
            {
                Id = Guid.NewGuid().ToString(),
                Name = collection["Name"],
                Value = collection["Value"],
                Status = 0
            };
            await _netAppDbContext.WorkflowRefs.AddAsync(new WorkflowRef
            {
                Id = Guid.NewGuid().ToString(),
                WorkflowId = workflow.FlowId,
                ObjectId = hello.Id
            });
            await _netAppDbContext.SaveChangesAsync();
            await workflow?.MoveOn("NetApp.Workflows.EditOrderNode, NetApp", "CreateOrder", JsonConvert.SerializeObject(hello));
            return RedirectToAction(nameof(Index));
        }

        // GET: Flow/Edit/5
        public async Task<ActionResult> Details(string id)
        {
            var flowRef = await _netAppDbContext.WorkflowRefs.FirstOrDefaultAsync(w => w.ObjectId == id);
            Flow workflow = await _workflowFactory.FindWorkflow(flowRef.WorkflowId);
            ViewBag.Nodes = workflow.Nodes;
            var message = await _netAppDbContext.Messages.FindAsync(id);
            if (message != null)
            {
                return View(message);
            }
            return NotFound();
        }

        // POST: Flow/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Details(string id, IFormCollection collection)
        {
            try
            {
                var message = await _netAppDbContext.Messages.FindAsync(id);
                //默认先保存数据
                string btnSubmit = collection["btnSubmit"];
                message.Name = collection["Name"];
                message.Value = collection["Value"];
                _netAppDbContext.Messages.Update(message);
                await _netAppDbContext.SaveChangesAsync();
                if (btnSubmit == "Approve")
                {
                    var flowRef = await _netAppDbContext.WorkflowRefs.FirstOrDefaultAsync(w => w.ObjectId == id);
                    Flow workflow = await _workflowFactory.FindWorkflow(flowRef.WorkflowId);
                    await workflow?.MoveOn("NetApp.Workflows.ApproveNode, NetApp", "CreateOrder", id);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        
        public async Task<PartialViewResult> WorkflowDetails(string id)
        {
            var flowRef = await _netAppDbContext.WorkflowRefs.FirstOrDefaultAsync(w => w.ObjectId == id);
            return PartialView(flowRef);
        }
    }
}
