using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TextDatabase;

namespace ConsoleTester
{
    public class Product:IAR
    {        
        public string title { get; set; }
        public string description { get; set; }
        public int price { get; set; }
        public double discountPercentage { get; set; }
        public double rating { get; set; }
        public int stock { get; set; }
        public string brand { get; set; }
        public string category { get; set; }
        public string thumbnail { get; set; }
        public List<string> images { get; set; }
    }

    public class RootProducts
    {
        public List<Product> products { get; set; }
        public int total { get; set; }
        public int skip { get; set; }
        public int limit { get; set; }

        public static async Task<List<Product>>? GetAllAsync()
        {
            HttpClient client = new HttpClient();
            var root = await client.GetFromJsonAsync<RootProducts>("https://dummyjson.com/products");
            if (root != null)
                return root.products;

            List<Product> products = new List<Product>();
            products.Add(new Product() { Id = 1, description = "this is the product number 1" });
            products.Add(new Product() { Id = 2, description = "this is the product number 2" });
            products.Add(new Product() { Id = 3, description = "this is the product number 3" });

            return products;
        }
    }
}
