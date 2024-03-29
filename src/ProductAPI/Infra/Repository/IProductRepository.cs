using ProductAPI.Services.DTOs;

namespace ProductAPI.Infra.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDTO>> FindAll();
        Task<ProductDTO> FindById(long id);
        Task<ProductAddDTO> Create(ProductAddDTO productDTO);
        Task<ProductDTO> Update(ProductDTO productDTO);
        Task<bool> Delete(long id);
    }
}