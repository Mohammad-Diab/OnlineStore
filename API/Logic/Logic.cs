using System.Text.Json;

namespace OnlineStore
{
    public static class Logic
    {
        public static bool CreateUser(string email, string username, string password, string fullName, string phoneNumber)
        {
            var isExists = User.IsExists(username);
            if (!isExists)
            {
                User newUser = new User(username, fullName, email, password, phoneNumber);
                return newUser.Insert() == 1;
            }
            else
            {
                throw new Exception("User is already exists");
            }
        }

        internal static bool CreateProduct(string userId, ProductTable product, byte[] imageData, string fileEtension)
        {
            string pId = Guid.NewGuid().ToString();
            Product p = new Product(product.Name, product.Description, userId, product.Warranty, product.Price, product.AvailableStock, pId + fileEtension, pId);
            var result = p.Insert();
            if(result == 1)
            {
                return Files.StoreImage(imageData, p.Image);
            }
            return false;
        }

        internal static bool DeleteProduct(string id, string userId)
        {
            Product? p = Product.GetById(id);
            if (p == null)
            {
                throw new Exception("لم يتم العثور على المنتج المحدد");
            }
            if (userId == p.UserId)
            {
                return p.Delete(userId) > 0;
            }
            throw new Exception("المنتج لم تتم اضافته بواسطة المستخدم الحالي");
        }

        internal static List<UserTable> GetAllUsers()
        {
            List<User> users = User.GetUsersList();
            return users.Select(x => new UserTable(x)).ToList();
        }

        internal static List<ProductTable> GetMyProducts(string userId)
        {
            if (userId != null)
            {
                List<Product> products = Product.GetProductsList(userId);
                return products.Select(x => new ProductTable(x)).ToList();
            }
            return new List<ProductTable>();
        }

        internal static List<ProductGrid> GetTodayDeals()
        {

            List<Product> products = Product.GetTodayDeals(10);
            return products.Select(x => new ProductGrid(x)).ToList();
        }

        internal static List<ProductGrid> GetTopProducts()
        {
            List<Product> products = Product.GetTopProducts(10);
            return products.Select(x => new ProductGrid(x)).ToList();

        }

        internal static string GetImage(string imageName)
        {
            var imageBytes = Files.GetImage(imageName);
            return $"data:image/jpg;base64,{Convert.ToBase64String(imageBytes)}";
        }
    }
}
