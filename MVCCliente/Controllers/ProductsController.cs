using Microsoft.AspNetCore.Mvc;
using MVCCliente.Models;
using Newtonsoft.Json;
using System.Text;

namespace MVCCliente.Controllers
{
    public class ProductsController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<Product> productList = new List<Product>();

            using (var httpClient  = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:5024/api/Products"))
                {
                    string  apiResponse = await response.Content.ReadAsStringAsync();

                    productList = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
                }
            }
            return View(productList);
        }

        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        { 
            Product addProduct = new Product();

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("http://localhost:5024/api/Products", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    addProduct = JsonConvert.DeserializeObject<Product>(apiResponse);
                }
            
            }
            return View(addProduct);

            }

        public async Task<IActionResult> Update(int id)
        {
            Product product = new Product();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:5024/api/Products/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<Product>(apiResponse);
                }
            }
            return View(product);
        }


        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            Product receivedProduct = new Product();

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync("http://localhost:5024/api/Products/" + product.Id, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    receivedProduct = JsonConvert.DeserializeObject<Product>(apiResponse);
                }
            }

            return View(receivedProduct);

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int ProductId)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("http://localhost:5024/api/Products/" + ProductId))
                {
                    string apiResponse =await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Index");
            }







        }
}
