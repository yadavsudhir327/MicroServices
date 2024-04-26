using AutoMapper;
using MicroServices.ShoppingCartApi.Data;
using MicroServices.ShoppingCartApi.Models;
using MicroServices.ShoppingCartApi.Models.Dto;
using MicroServices.ShoppingCartApi.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace MicroServices.ShoppingCartApi.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartApiController : ControllerBase
    {
        private ResponseDto responseDto;
        private IMapper mapper;
        private readonly AppDbContext appDbContext;
        private readonly IProductService productService;
        private readonly ICouponService couponService;
        public ShoppingCartApiController(ICouponService couponService,IProductService productService, IMapper mapper, AppDbContext appDbContext)
        {
            this.mapper = mapper;
            this.appDbContext = appDbContext;
            this.responseDto = new ResponseDto();
            this.productService = productService;
            this.couponService = couponService;
        }
        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> Get(string userId)
        {
            try
            {
                CartDto cartDto = new()
                {
                    CartHeader = mapper.Map<CartHeaderDto>(appDbContext.CartHeaders.First(u => u.UserId == userId))
                };
                cartDto.CartDetails = mapper.Map<IEnumerable<CartDetailsDto>>(appDbContext.CartDetails.Where(u => u.CartHeaderId == cartDto.CartHeader.CartHeaderId));
                IEnumerable<ProductDto> productDtos = await productService.GetProducts();

                foreach (var item in cartDto.CartDetails)
                {
                    item.Product = productDtos.FirstOrDefault(u => u.ProductId == item.ProductId);
                    cartDto.CartHeader.CartTotal += item.Count * item.Product.Price;
                }
                // apply  coupn if any
                if (!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
                {
                    CouponDto couponDto = await couponService.GetCoupon(cartDto.CartHeader.CouponCode);
                    if (couponDto != null && cartDto.CartHeader.CartTotal > couponDto.MinAmount)
                    {
                        {
                            cartDto.CartHeader.CartTotal -= couponDto.DiscountAmount;
                            cartDto.CartHeader.Discount = couponDto.DiscountAmount;
                        }
                    }
                    
                }
                responseDto.Result = true;
            }
            catch (Exception ex)
            {
                responseDto.Result = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto>CartUpsert(CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await appDbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
                 if(cartHeaderFromDb == null)
                {
                    //create cartheader details
                    CartHeader cartHeader =mapper.Map<CartHeader>(cartDto.CartHeader); 
                   appDbContext.CartHeaders.Add(cartHeader);
                    appDbContext.SaveChanges();
                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    appDbContext.CartDetails.Add(mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await appDbContext.SaveChangesAsync();
                }
                else
                {//if header is not null check if details has same product
                    var cartDetailsFromDb = await appDbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(u => u.ProductId == cartDto.CartDetails.First().ProductId && u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if(cartDetailsFromDb == null)
                    {
                        //create cartdetails 
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        appDbContext.CartDetails.Add(mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await appDbContext.SaveChangesAsync();
                    }
                    else
                    {
                        //update count in cart details
                        cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                        appDbContext.CartDetails.Update(mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await appDbContext.SaveChangesAsync();
                    }
                }
                responseDto.Result = cartDto;
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }
        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails =appDbContext.CartDetails.First(u=>u.CartDetailsId==cartDetailsId);
                int totalCountofCartItems=appDbContext.CartDetails.Where(u=>u.CartHeaderId==cartDetails.CartHeaderId).Count();
                appDbContext.CartDetails.Remove(cartDetails);
                if (totalCountofCartItems == 1)
                {
                    var cartHeaderToRemove = await appDbContext.CartHeaders.FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);
                    appDbContext.CartHeaders.Remove(cartHeaderToRemove);

                }
                await appDbContext.SaveChangesAsync();
                responseDto.Result = true;
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }
        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var cartFromDb = await appDbContext.CartHeaders.FirstAsync(u => u.UserId == cartDto.CartHeader.UserId
                );
                cartFromDb.CouponCode =cartDto.CartHeader.CouponCode;
                appDbContext.CartHeaders.Update(cartFromDb);
                await appDbContext.SaveChangesAsync();
                responseDto.Result = true;

            }
            catch(Exception ex)
            {
                responseDto.IsSuccess = false;
                   responseDto.Message = ex.Message;
            }
            return responseDto;
        }
        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var cartFromDb = await appDbContext.CartHeaders.FirstAsync(u => u.UserId == cartDto.CartHeader.UserId
                );
                cartFromDb.CouponCode = "";
                appDbContext.CartHeaders.Update(cartFromDb);
                await appDbContext.SaveChangesAsync();
                responseDto.Result = true;

            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }
    }
}
