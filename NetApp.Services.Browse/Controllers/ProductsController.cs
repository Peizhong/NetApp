using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetApp.Common.Models;
using NetApp.Common.Events;
using NetApp.Common.Abstractions;
using NetApp.Services.Lib.Controllers;
using Microsoft.Extensions.Caching.Distributed;
using NetApp.Services.Browse.Events;

namespace NetApp.Services.Browse.Controllers
{
    public class ProductsController : ListController<Product>
    {
        private readonly IBrowseIntegrationEventService _browseIntegrationEventService;

        public ProductsController(
            ILogger<ProductsController> logger,
            IDistributedCache cache,
            IListRepo<Product> repo,
            IBrowseIntegrationEventService browseIntegrationEventService)
            : base(logger, cache, repo)
        {
            _browseIntegrationEventService = browseIntegrationEventService;
        }

        // POST: api/Products
        [HttpPost]
        public Task PostAsync([FromBody] Product value)
        {
            return _repo.UpdateAsync(value);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Product value)
        {
            var item = await _repo.FindAsync(value.Id);
            if (item == null)
            {
                return NotFound(new { Message = $"Products with id {value.Id} not found." });
            }
            var oldPrice = item.Price;
            var raiseProductPriceChangedEvent = oldPrice != value.Price;
            if (raiseProductPriceChangedEvent) // Save product's data and publish integration event through the Event Bus if price has changed
            {
                //Create Integration Event to be published through the Event Bus
                var priceChangedEvent = new ProductPriceChangedIntegrationEvent(value.Id, value.Price, oldPrice);

                // Achieving atomicity between original Catalog database operation and the IntegrationEventLog thanks to a local transaction
                await _browseIntegrationEventService.SaveEventAndCatalogContextChangesAsync(priceChangedEvent);

                // Publish through the Event Bus and mark the saved event as published
                await _browseIntegrationEventService.PublishThroughEventBusAsync(priceChangedEvent);
            }
            else // Just save the updated product because the Product's Price hasn't changed.
            {
                //await _catalogContext.SaveChangesAsync();
            }
            return CreatedAtAction(nameof(GetByIdAsync), new { id = value.Id }, null);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}