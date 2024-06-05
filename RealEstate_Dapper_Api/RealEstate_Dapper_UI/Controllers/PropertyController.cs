using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RealEstate_Dapper_UI.Dtos.ProductDetailDtos;
using RealEstate_Dapper_UI.Dtos.ProductDtos;

namespace RealEstate_Dapper_UI.Controllers
{
    [Authorize]
    [Route("property")]
    public class PropertyController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PropertyController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet(Name = "Properties")]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:44345/api/Products/ProductListWithCategory");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultProductDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpGet("search")]
        public async Task<IActionResult> PropertyListWithSearch(string searchKeyValue, int propertyCategoryId, string city)
        {
            ViewBag.searchKeyValue = TempData["searchKeyValue"];
            ViewBag.propertyCategoryId = TempData["propertyCategoryId"];
            ViewBag.city = TempData["city"];

            searchKeyValue = TempData["searchKeyValue"].ToString();
            propertyCategoryId = int.Parse(TempData["propertyCategoryId"].ToString());
            city = TempData["city"].ToString();

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:44345/api/Products/ResultProductWithSearchList?searchKeyValue={searchKeyValue}&propertyCategoryId={propertyCategoryId}&city={city}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultProductWithSearchListDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpGet("{id}", Name = "GetPropertySingle")]
        public async Task<IActionResult> PropertySingle(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var productResponseMessage = await client.GetAsync("https://localhost:44345/api/Products/GetProductByProductId?id=" + id);
            var jsonData = await productResponseMessage.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<ResultProductDto>(jsonData);

            var productDetailResponseMessage = await client.GetAsync("https://localhost:44345/api/ProductDetails/GetProductDetailByProductId?id=" + id);
            jsonData = await productDetailResponseMessage.Content.ReadAsStringAsync();
            var productDetails = JsonConvert.DeserializeObject<GetProductDetailByIdDto>(jsonData);

            ViewBag.productId = product.productID;
            ViewBag.title1 = product.title.ToString();
            ViewBag.price = product.price;
            ViewBag.city = product.city;
            ViewBag.district = product.district;
            ViewBag.address = product.address;
            ViewBag.type = product.type;
            ViewBag.description = product.description;

            ViewBag.bathCount = productDetails.bathCount;
            ViewBag.bedCount = productDetails.bedRoomCount;
            ViewBag.size = productDetails.productSize;
            ViewBag.roomCount = productDetails.roomCount;
            ViewBag.garageCount = productDetails.garageSize;
            ViewBag.buildYear = productDetails.buildYear;
            ViewBag.date = productDetails.AdvertisementDate;
            ViewBag.location = productDetails.location;
            ViewBag.videoUrl = productDetails.videoUrl;

            DateTime date1 = DateTime.Now;
            DateTime date2 = productDetails.AdvertisementDate;

            TimeSpan timeSpan = date1 - date2;
            int month = timeSpan.Days;

            ViewBag.datediff = month / 30;
            return View();
        }
    }
}