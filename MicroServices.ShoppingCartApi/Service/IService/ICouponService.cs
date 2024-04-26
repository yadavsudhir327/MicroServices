using MicroServices.ShoppingCartApi.Models.Dto;

namespace MicroServices.ShoppingCartApi.Service.IService
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon(string couponCode);
    }
}
