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
            var bills = await _avmtApp.Bills();
            return View(bills);
        }
        

        public async Task<ActionResult> MainTransferDetail(string id)
        {
            var bills = await _avmtApp.Bills();
            return View(bills.First(b => b.Id == id));
        }

        private IEnumerable<FunctionLocation> FunctionLocations
            => Enumerable.Range(1, 10).Select(n =>
              new FunctionLocation { Id = Guid.NewGuid().ToString("N"), FlName = $"FL{n}" }).ToList();

        public IActionResult DetailsAsModel() => View(FunctionLocations);

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