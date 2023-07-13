using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopOnContainers.Web.Sales.HttpAggregator.Models;
using Microsoft.eShopOnContainers.Web.Sales.HttpAggregator.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopOnContainers.Web.Sales.HttpAggregator.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ICatalogService _catalog;
        private readonly IOrderingService _ordering;

        private readonly ILogger<SalesController> _logger;

        public SalesController(ICatalogService catalogService, IOrderingService orderingService, ILogger<SalesController> logger)
        {
            _catalog = catalogService;
            _ordering = orderingService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(SalesDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<SalesDto>>> GetSalesOfTodayByBrand()
        {
            _logger.LogInformation("----- SalesController --> GetTotalSalesAsync()");

            try
            {
                // All catalog items
                var catalogItems = await _catalog.GetCatalogItemAsync();

                // All catalog brands
                var catalogBrands = await _catalog.GetCatalogBrandAsync();

                // All orders
                var orderItems = await _ordering.GetOrdersAsync();

                // Fetch processed sales data
                var salesData = await this.GetSalesData(catalogItems, catalogBrands, orderItems);

                return salesData;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        // Add the GetSalesData code
    }
}
