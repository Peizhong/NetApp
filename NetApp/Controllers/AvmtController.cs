using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NetApp.Controllers
{
    public class AvmtController : Controller
    {
        // GET: Avmt
        public ActionResult Index()
        {
            ViewBag.Title = "mimi";
            ViewBag.Text = "alalal";
            return View();
        }

        // GET: Avmt/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
    }
}