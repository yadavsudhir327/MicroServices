using AutoMapper;
using MicroServices.CouponApi.Data;
using MicroServices.CouponApi.Models;
using MicroServices.CouponApi.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroServices.CouponApi.Controllers
{
    [ApiController]
    [Route("api/coupon")]
    [Authorize]
    public class CouponApiController:ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ResponseDto responseDto;
        private readonly IMapper _mapper;
        public CouponApiController(AppDbContext appDbContext,IMapper mapper)
        {
            _context = appDbContext;
            responseDto= new ResponseDto();
            _mapper = mapper;
        }
        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                
                responseDto.Result =_mapper.Map<IEnumerable<CouponDto>>( _context.Coupons.ToList());
                return responseDto;
            }
            catch (Exception ex)
            {
                responseDto.Message = ex.Message;
                responseDto.IsSuccess = false;
            }
            return responseDto;
        }
        [HttpGet("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                Coupon coupon = _context.Coupons.FirstOrDefault(u => u.CouponCode == code);
                responseDto.Result=_mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                responseDto.Message = ex.Message;
                responseDto.IsSuccess = false;
            }
            return responseDto;
        }
        [HttpGet]
        [Route("{id:int}")]
       
        public ResponseDto Get(int id)
        {
            try
            {
               responseDto.Result= _mapper.Map<CouponDto>(_context.Coupons.First(x => x.CouponId == id));
                return responseDto;
            }
            catch (Exception ex)
            {
                responseDto.Message = ex.Message;
                responseDto.IsSuccess = false;
            }

            return responseDto;
            
        }
        [HttpPost]
        [Authorize(Roles ="ADMIN")]
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDto);
                _context.Coupons.Add(obj);
                _context.SaveChanges();
                responseDto.Result = couponDto;
            }
            catch(Exception ex)
            {
                responseDto.Message=ex.Message;
                responseDto.IsSuccess = false;
            }
            return responseDto;

        }
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try {
                Coupon coupon= _mapper.Map<Coupon>(couponDto);
                _context.Coupons.Update(coupon);
            }
            catch(Exception ex)
            {
                responseDto.Message=ex.Message;
                responseDto.IsSuccess = false;
            }
            return responseDto;
        }
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Coupon coupon = _context.Coupons.First(x => x.CouponId == id);
                _context.Remove(coupon);
                _context.SaveChanges(true);
            }
            catch(Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;

        }
    }
}
