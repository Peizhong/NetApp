using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using NetApp.Entities.Avmt;
using NetApp.Business.Interfaces;

namespace NetApp.Controllers
{
    public class AvmtController : Controller
    {
        private readonly IDistributedCache _cache;
        private readonly IAvmtApp _avmtApp;
        public AvmtController(IDistributedCache cache, IAvmtApp avmtApp)
        {
            _cache = cache;
            _avmtApp = avmtApp;
        }

        // GET: Avmt
        public async Task<ViewResult> Index()
        {
            await _cache.SetStringAsync("lastServerStartTime", DateTime.Now.ToString());
            var bills = await _avmtApp.GetBillsAsync("");
            return View(bills);
        }

        public async Task<IActionResult> MainTransferBillDetail(string id)
        {
            var bills = await _avmtApp.GetBillsAsync("");
            return View(bills.First(b => b.Id == id));
        }

        public async Task<IActionResult> DisTransferBillDetail(string id)
        {
            var bills = await _avmtApp.GetBillsAsync("");
            return View(bills.First(b => b.Id == id));
        }

        public async Task<IActionResult> ChangeBillDetail(string id)
        {
            var bills = await _avmtApp.GetBillsAsync("");
            return View(bills.First(b => b.Id == id));
        }

        public async Task<IActionResult> Workspaces(string billId)
        {
            var bills = await _avmtApp.GetBillsAsync("");
            var bill = bills.FirstOrDefault(b => b.Id == billId);
            return PartialView(bill.Workspaces);
        }

        public async Task<IActionResult> WorkspaceDetail(string id, int startIndex, int pageSize = 16)
        {
            var functionlocations = await _avmtApp.GetFunctionLocationsAsync(id, startIndex, pageSize);
            var classifies = await _avmtApp.GetClassifiesAsync(functionlocations.Select(f => f.ClassifyId).Distinct());
            ViewBag.CurrentIndex = startIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.Classifies = classifies;
            return View(functionlocations);
        }

        public async Task<IActionResult> FunctionLocationDetail(string id, string workspaceId)
        {
            var functionlocation = await _avmtApp.FindFunctionLocationAsync(id, workspaceId);
            await _avmtApp.LoadFunctionLocationDetail(functionlocation);
            return View(functionlocation);
        }

        public async Task<IActionResult> FunctionLocationEdit(string id, string workspaceId)
        {
            var functionlocation = await _avmtApp.FindFunctionLocationAsync(id, workspaceId);
            return View(functionlocation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FunctionLocationEdit(string id, string workspaceId, IFormCollection collection)
        {
            var functionlocation = await _avmtApp.FindFunctionLocationAsync(id, workspaceId);
            return View(functionlocation);
        }

        public IActionResult DetailsAsModel()
        {
            IEnumerable<FunctionLocation> FunctionLocations = Enumerable.Range(1, 10).Select(n =>
            new FunctionLocation
            {
                Id = Guid.NewGuid().ToString("N"),
                FlName = $"FL{n}"
            }).ToList();

            return View(FunctionLocations);
        }

        // GET: Avmt/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Avmt/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Avmt/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Avmt/Edit/5
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

        // GET: Avmt/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Avmt/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult RedirectDemo()
        {
            return Redirect("http://www.baidu.com");
        }
    }
}