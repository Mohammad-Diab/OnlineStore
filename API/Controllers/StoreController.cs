using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Reflection;

namespace OnlineStore.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StoreController : MyControllerBase
    {
        [HttpGet]
        [ActionName(nameof(GetMyProducts))]
        public ApiResponse<List<ProductTable>> GetMyProducts()
        {
            try
            {
                var userId = Authentication.GetUserId(getTokenValue());
                var result = Logic.GetMyProducts(userId);
                return new ApiResponse<List<ProductTable>>(result);
            }
            catch (Exception ex) { return new ApiResponse<List<ProductTable>>(400, ex.Message); }
        }
        
        [HttpGet]
        [ActionName(nameof(DeleteProduct))]
        public ApiResponse<bool> DeleteProduct(string id)
        {
            try
            {
                string userId = Authentication.GetUserId(getTokenValue());
                bool result = Logic.DeleteProduct(id, userId);
                return new ApiResponse<bool>(result);
            }
            catch (Exception ex) { return new ApiResponse<bool>(400, ex.Message); }
        }

        [HttpPost]
        [ActionName(nameof(CreateProduct))]
        public async Task<ApiResponse<bool>> CreateProduct([FromForm] ProductTable product)
        {
            try
            {
                if (product.Image == null || product.Image.Length == 0)
                {
                    return new ApiResponse<bool>(400, "لم تقم بتحميل صورة");
                }

                if (product.Image.Length > 524288)
                {
                    return new ApiResponse<bool>(400, "الصورة كبيرة جدا... الحجم الأقصى للملف هو 512 كيلوبايت");
                }

                using (var stream = new MemoryStream())
                {
                    await product.Image.CopyToAsync(stream);
                    byte[] imageData = stream.ToArray();
                    var userId = Authentication.GetUserId(getTokenValue());
                    if (string.IsNullOrEmpty(userId))
                    {
                        return new ApiResponse<bool>(500, "يرجى اعادة تسجيل الدخول");
                    }
                    string extension = Path.GetExtension(product.Image.FileName);
                    bool result = Logic.CreateProduct(userId, product, imageData, extension);
                    if (result)
                    {
                        return new ApiResponse<bool>(true);
                    }
                    else
                    {
                        return new ApiResponse<bool>(400, "فشل حفظ المنتج");
                    }
                }
            }
            catch (Exception ex) { return new ApiResponse<bool>(400, ex.Message); }
        }

        [HttpGet]
        [ActionName(nameof(GetTodayDeals))]
        public ApiResponse<List<ProductGrid>> GetTodayDeals()
        {
            try
            {
                var userId = Authentication.GetUserId(getTokenValue());
                if (string.IsNullOrEmpty(userId))
                {
                    return new ApiResponse<List<ProductGrid>>(500, "يرجى اعادة تسجيل الدخول");
                }
                var result = Logic.GetTodayDeals();
                return new ApiResponse<List<ProductGrid>>(result);
            }
            catch (Exception ex) { return new ApiResponse<List<ProductGrid>>(400, ex.Message); }
        }

        [HttpGet]
        [ActionName(nameof(GetTopProducts))]
        public ApiResponse<List<ProductGrid>> GetTopProducts()
        {
            try
            {
                var userId = Authentication.GetUserId(getTokenValue());
                if (string.IsNullOrEmpty(userId))
                {
                    return new ApiResponse<List<ProductGrid>>(500, "يرجى اعادة تسجيل الدخول");
                }
                var result = Logic.GetTopProducts();
                return new ApiResponse<List<ProductGrid>>(result);
            }
            catch (Exception ex) { return new ApiResponse<List<ProductGrid>>(400, ex.Message); }
        }

        [HttpGet]
        [ActionName(nameof(GetImage))]
        public ApiResponse<string> GetImage(string path)
        {
            try
            {
                var userId = Authentication.GetUserId(getTokenValue());
                if (string.IsNullOrEmpty(userId))
                {
                    return new ApiResponse<string> (500, "يرجى اعادة تسجيل الدخول");
                }
                string result = Logic.GetImage(path);
                return new ApiResponse<string>(result);
            }
            catch (Exception)
            {
                return new ApiResponse<string>("data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAQAAAC1+jfqAAAAQklEQVR4AWMgGKy8vLy0tLVbMPDw9XcjIyMjAwMDOxETDw9Xcjk5OTU0tDS0gRI3aSYZaXRB8oAAAAASUVORK5CYII=");
            }
        }
    }
}
