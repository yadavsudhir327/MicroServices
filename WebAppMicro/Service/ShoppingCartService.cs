using WebAppMicro.Models;
using WebAppMicro.Service.IService;
using WebAppMicro.Utility;

namespace WebAppMicro.Service
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IBaseService baseService;
        public ShoppingCartService(IBaseService baseService)
        {
            this.baseService = baseService;
        }


       public async Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto)
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ShoppingCartBaseAPI + "/api/cart/ApplyCoupon",
                Data=cartDto
            });
        }

      public async  Task<ResponseDto?> GetCartByUserIdAsync(string userId)
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ShoppingCartBaseAPI + "/api/cart/GetCart/" + userId,
            });
        }

        public async Task<ResponseDto?>RemoveFromCartAsync(int cartDetailsId)
        {
            return await baseService.SendAsync(new RequestDto { 
                ApiType = SD.ApiType.POST, Url = SD.ShoppingCartBaseAPI + "/api/cart/RemoveCart",
                Data=cartDetailsId,
            });
        }

        public async Task<ResponseDto?> UpsertCartAsync(CartDto cartDto)
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Url = SD.ShoppingCartBaseAPI + "/api/cart/UpsertCart",
                Data= cartDto
            });
        }
    }
}
