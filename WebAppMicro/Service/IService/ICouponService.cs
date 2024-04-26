using WebAppMicro.Models;

namespace WebAppMicro.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDto?> GetCouponByIdAsync(int id);
        Task<ResponseDto?> GetAllCouponAsync();
        Task<ResponseDto?> DeleteCouponAsync(int id);
        Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto);
        Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto);


    }
}
