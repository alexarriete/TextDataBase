using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextDatabase;

namespace ConsoleTester
{
    public class TestCase
    {
        public static void DropDatabase()
        {
            var fileHandler = TextDatabase.FileHandler.GetInstance();
            DataBaseHandler.DropDataBase(fileHandler.AppName);
        }
       
        public static void List(List<Product> products)
        {

            // AppName is optional GetInstance("AppName"). If it is null or empty we will try to resolve the Module.Name.
            var fileHandler = TextDatabase.FileHandler.GetInstance();

            var result = fileHandler.InsertList<Product>(products);
            IEnumerable<Product> insertedProducts = fileHandler.Get<Product>();

            foreach (Product item in insertedProducts)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Product: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(item.Id);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" Description: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(item.description);
                Console.WriteLine();
            }           

            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Data is inserted on DataBase and listed");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Resume();
        }


        public static void Update3(List<Product> products)
        {                        
            var fileHandler = TextDatabase.FileHandler.GetInstance();

            var result = fileHandler.InsertList<Product>(products);
            var product3 = fileHandler.GetById<Product>(3);
            var product3Updated = new Product() { Id = product3.Id, description = "New description for product 3." };
            fileHandler.Update<Product>(product3Updated);
            product3Updated = fileHandler.GetById<Product>(3);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Product3: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(product3.Id);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" Description: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(product3.description);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Product3: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(product3Updated.Id);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" Description: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(product3Updated.description);
            Console.WriteLine();
            
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Data is inserted on DataBase, product3 is updated.");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Resume();
        }

        public static void Insert1(List<Product> products)
        {
            Product? newProduct = new Product()
            {
                description = "this is the product number 31"
            };


            var fileHandler = TextDatabase.FileHandler.GetInstance();
            var result = fileHandler.InsertList<Product>(products);
            newProduct = fileHandler.Insert<Product>(newProduct);

            List<Product> insertedProducts = fileHandler.Get<Product>().ToList();

            foreach (Product item in insertedProducts)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Product: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(item.Id);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" Description: ");
                Console.ForegroundColor = insertedProducts.IndexOf(item)== insertedProducts.Count-1  ? ConsoleColor.DarkGray: ConsoleColor.White;
                Console.Write(item.description);
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("New product is inserted on DataBase and listed");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Resume();
        }

        public static void DeleteRange(List<Product> products)
        {

            var fileHandler = TextDatabase.FileHandler.GetInstance();
            var result = fileHandler.InsertList<Product>(products);
            List<Product> productsToDelete = products.Where(x => x.Id % 2 != 0).ToList();

            fileHandler.DeleteRange<Product>(productsToDelete);

            IEnumerable<Product> insertedProducts = fileHandler.Get<Product>();

            foreach (Product item in insertedProducts)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Product: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(item.Id);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" Description: ");
                Console.ForegroundColor = item.Id < 31 ? ConsoleColor.White : ConsoleColor.DarkGray;
                Console.Write(item.description);
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Products are inserted on DataBase. Odds ids are deleted and rest is listed");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Resume();
        }

        private static void Resume()
        {
            Console.WriteLine($"Database dropped to avoid conflicts with another test cases. If you wish to see de files comment Resume() and check {FileHandler.BasePath}");
            TestCase.DropDatabase();
            Console.WriteLine("Enter to finish");
            Console.ReadLine();
        }
    }
}
