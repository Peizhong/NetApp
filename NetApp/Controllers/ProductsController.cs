using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetApp.Common.Abstractions;
using NetApp.Models;

namespace NetApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HttpClient _client;
        public ProductsController(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient();
        }

        public ActionResult Play()
        {
            Response.Cookies.Append("I", "see you");
            return Ok();
        }

        // GET: Products
        public async Task<ActionResult> Index()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, @"http://localhost:5010/ClientService/products");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            var response = await _client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            var result = PageableQueryResult<Product>.FromJson(json);

            return View(result.Items);
        }


        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
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

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $@"http://localhost:5010/ClientService/products/{id}");
            request.Headers.Add("Accept", "application/json");

            var response = await _client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            var result = Product.FromJson(json);
            if (result == null)
                return NotFound();
            return View(result);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, [FromForm]Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _client.PostAsJsonAsync($@"http://localhost:5010/ClientService/products/{id}", product);
                    if (response.IsSuccessStatusCode)
                    {
                        RedirectToAction(nameof(Index));
                    }
                    return View(product);
                }
                catch (Exception ex)
                {
                    return View();
                }
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(string id)
        {
            return View();
        }

        // POST: Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
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