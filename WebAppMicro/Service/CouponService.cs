using WebAppMicro.Models;
using WebAppMicro.Service.IService;
using WebAppMicro.Utility;

namespace WebAppMicro.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService baseService;
        public CouponService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

       public async Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto)
        {
            return await baseService.SendAsync(new RequestDto { ApiType = SD.ApiType.POST, Url = SD.CouponBaseAPI + "/api/coupon", Data = couponDto });

        }

        public async Task<ResponseDto?> DeleteCouponAsync(int id)
        {

            return await baseService.SendAsync(new RequestDto
            {
                ApiType=SD.ApiType.DELETE,
                Url=SD.CouponBaseAPI+"/api/coupon/"+id
            });
        }

        public async Task<ResponseDto?> GetAllCouponAsync()
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponBaseAPI + "/api/coupon"
            });
        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.CouponBaseAPI + "/api/coupon"+id


            });
        }

        public async Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto)
        {
            return await baseService.SendAsync(new RequestDto { ApiType = SD.ApiType.PUT, Url = SD.CouponBaseAPI + "/api/coupon", Data = couponDto });

        }
    }
}
