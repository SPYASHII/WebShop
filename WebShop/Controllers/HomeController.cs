using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using WebShop.Data;
using WebShop.Enums;
using WebShop.Interfaces;
using WebShop.Models;
using WebShop.Models.Abstract;

namespace WebShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShopDbContext _shopDb;
        private readonly ISerializer _serializer;
        private readonly IMemoryCache _memoryCache;
        public HomeController(ShopDbContext shopDb, ISerializer serializer, IMemoryCache memoryCache)
        {
            _shopDb = shopDb;
            _serializer = serializer;
            _memoryCache = memoryCache;
        }
        private void AddUserData()
        {
            bool success = bool.TryParse(HttpContext.Session.GetString(SessionItems.Loged.ToString()), out bool loged);

            if (success && loged)
            {
                ViewBag.Loged = loged;
                //ViewBag.BackgroundColor = 
            }
        }
        private void AddUserLogin()
        {
            var bytes = HttpContext.Session.Get(SessionItems.User.ToString());

            if (bytes is not null)
            {
                var user = _serializer.Deserialize<User>(bytes);

                ViewBag.Login = user?.Login;
            }
        }
        private IEnumerable<Product> GetProducts()
        {
            IEnumerable<Product> products;

            if (_memoryCache.TryGetValue(MemoryCacheItems.Products, out object? value))
            {
                products = (IEnumerable<Product>)value;
            }
            else
            {
                products = _shopDb.Products.ToList();

                var options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
                };

                _memoryCache.Set(MemoryCacheItems.Products, products, options);
            }

            if (products == null)
                products = new List<Product>();

            return products;
        }
        public IActionResult Catalog()
        {
            AddUserData();

            var products = GetProducts();

            return View(products);
        }

        public IActionResult Cart()
        {
            AddUserData();

            var cart = GetCartFromSession();

            return View(cart);
        }
        [HttpPost]
        public IActionResult Cart(int productId, int qty)
        {
            var product = _shopDb.Products.Single(p => p.Id == productId);

            //HACK: при добавлении в корзину обращение к бд чтобы убедитьс€ что на складе есть продукты
            //TODO: лучше сделать в процессе покупки
            //TODO: пользователь должен быть оповещен что не получилось купить/добавить в корзину т.к. товара нет на складе
            if (product.Qty >= qty)
            {
                var bytes = HttpContext.Session.Get(SessionItems.Cart.ToString());

                Cart? cart = new Cart();

                if (bytes != null)
                    cart = _serializer.Deserialize<Cart>(bytes);

                var productQty = cart.IdProductQtyPairs;


                if (productQty.ContainsKey(product.Id))
                {
                    //HACK: у€звимость, можно добавить в корзину товара больше чем есть на складе
                    productQty[product.Id].Qty += qty;
                }
                else
                {
                    var item = new CartItem(product, qty);

                    productQty.Add(product.Id, item);
                }

                bytes = _serializer.Serialize<Cart>(cart);

                HttpContext.Session.Set(SessionItems.Cart.ToString(), bytes);
            }


            return RedirectToAction("Catalog");
        }

        public IActionResult Buy()
        {
            AddUserLogin();
            AddUserData();

            var cart = GetCartFromSession();

            if (cart != null)
                ProcessCartPurchase(cart);

            ClearCart();

            return View();
        }

        public IActionResult Profile()
        {
            AddUserLogin();
            AddUserData();

            return View();
        }
        [HttpPost]
        public IActionResult Profile(string headerColor)
        {
            if (headerColor != null)
            {
                var options = new CookieOptions()
                {
                    Expires = DateTime.UtcNow.AddMinutes(30)
                };
                Response.Cookies.Append(CookieItems.HeaderBackgroundColor.ToString(), headerColor, options);
            }

            return Profile();
        }

        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(SessionItems.Cart.ToString());

            return RedirectToAction("Cart");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void ProcessCartPurchase(Cart cart)
        {
            var ids = cart.IdProductQtyPairs.Keys;

            var products = _shopDb.Products.Where(p => ids.Contains(p.Id));

            foreach (var product in products)
            {
                var mQty = cart.IdProductQtyPairs[product.Id].Qty;

                if (product.Qty >= mQty)
                    product.Qty -= mQty;
            }

            _shopDb.SaveChangesAsync();
        }


        private Cart? GetCartFromSession()
        {
            var bytes = HttpContext.Session.Get(SessionItems.Cart.ToString());
            Cart? cart = new();

            if (bytes != null)
                cart = _serializer.Deserialize<Cart>(bytes);

            return cart;
        }
    }
}
