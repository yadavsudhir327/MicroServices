using MicroServices.ShoppingCartApi.Models.Dto;

namespace MicroServices.ShoppingCartApi.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
