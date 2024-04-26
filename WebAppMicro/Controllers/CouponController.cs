using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using WebAppMicro.Models;
using WebAppMicro.Service;
using WebAppMicro.Service.IService;

namespace WebAppMicro.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService=couponService;
        }
        public async Task<IActionResult> Index()
        {
            List<CouponDto>? list=new List<CouponDto>();
            ResponseDto? responseDto = await _couponService.GetAllCouponAsync();
            if(responseDto != null && responseDto.IsSuccess) {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(responseDto.Result));

            }
            return View(list);
        }
		public async Task<IActionResult> CreateCoupon()
		{
			return View(new CouponDto());
		}
        [HttpPost]
        public async Task<IActionResult> CreateCoupon(CouponDto couponDto)
        {
            if(ModelState.IsValid)
            {
                ResponseDto? responseDto = await _couponService.CreateCouponAsync(couponDto);
                if (responseDto != null && responseDto.IsSuccess)
                {
                   
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(couponDto);
        }
        public async Task<IActionResult> DeleteCoupon(int couponId)
        {
            ResponseDto? responseDto = await _couponService.DeleteCouponAsync(couponId);
            return RedirectToAction(nameof(Index));
        }
    }
}
