using System.Data.SqlClient;

namespace OnlineStore
{
    public class Cart
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public bool Closed { get; set; }
        public List<CartProduct> Products { get; set; }

        static string BaseQuery = "SELECT ID, USER_ID, Closed TBL_CARTS ";

        public Cart()
        {
            Id = "";
            UserId = "";
            Closed = false;
            Products = new List<CartProduct>();
        }

        public Cart(SqlDataReader reader)
        {
            Id = reader[reader.GetOrdinal("ID")].ToString() ?? "";
            UserId = reader[reader.GetOrdinal("USER_ID")].ToString() ?? "";
            Closed = DatabaseConnection.ConvertToBoolean(reader[reader.GetOrdinal("CLOSED")]);
            Products = new List<CartProduct>();
        }

        internal static int Close(string id)
        {
            string query = "UPDATE TBL_CARTS SET CLOSED = TRUE ID = @0";
            return DatabaseConnection.NonQuery(query, id);
        }

        internal static Cart? GetById(string id)
        {
            string whereString = $" WHERE ID = @0";
            SqlDataReader? reader = DatabaseConnection.Query(BaseQuery + whereString, id);
            if (reader?.Read() ?? false)
            {
                Cart cart = new Cart(reader);
                reader.Close();
                return cart;
            }
            return null;
        }

        internal int InsertProduct(string id, string productId)
        {
            var cartProducts = CartProduct.GetCartProductsList(id);
            var cartProduct = cartProducts?.Find(x => x.ProductId == productId);
            if (cartProduct == null)
            {
                cartProduct = new CartProduct(id, productId, 0);
                cartProduct.Insert();
            }
            return cartProduct.UpdateQuantity(1);
        }

        internal void LoadProducts(string id)
        {
            Products = CartProduct.GetCartProductsList(id);
        }

        internal static Cart? GetByUserId(string userId)
        {
            string whereString = $" WHERE Closed = false and USER_ID = @0";
            SqlDataReader? reader = DatabaseConnection.Query(BaseQuery + whereString, userId);
            if (reader?.Read() ?? false)
            {
                Cart cart = new Cart(reader);
                reader.Close();
                return cart;
            }
            return null;
        }

        internal static Cart GetOrCreate(string userId)
        {
            var cart = GetByUserId(userId);
            if(cart == null)
            {
                cart = new Cart(userId);
                cart.Insert();
            }
            return cart;
        }

        public Cart(string userId, string id = "")
        {
            Id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            UserId = userId;
            Closed = false;
            Products = new List<CartProduct>();
        }

        internal int Insert()
        {
            string query = "Insert into TBL_CARTS (ID, USER_ID, CLOSED) VALUES (@0, @1, @2)";
            int result = DatabaseConnection.NonQuery(query, Id, UserId, Closed.ToString());
            return result;
        }
    }
}
