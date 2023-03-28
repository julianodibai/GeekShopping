using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Infra.Context;
using ProductAPI.Models;
using ProductAPI.Services.DTOs;

namespace ProductAPI.Infra.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;
        private IMapper _mapper;

        public ProductRepository(ProductContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> FindAll()
        {
            List<Product> products = await _context.Products.ToListAsync();

            return _mapper.Map<List<ProductDTO>>(products);
        }

        public async Task<ProductDTO> FindById(long id)
        {
            Product product = await _context.Products
                                        .Where(p => p.Id == id)
                                        .FirstOrDefaultAsync() ?? new Product();

            return _mapper.Map<ProductDTO>(product);    
        }

        public async Task<ProductAddDTO> Create(ProductAddDTO productDTO)
        {
            Product product = _mapper.Map<Product>(productDTO);

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return _mapper.Map<ProductAddDTO>(product);
        }

        public async Task<ProductDTO> Update(ProductDTO productDTO)
        {
            Product product = _mapper.Map<Product>(productDTO);

            _context.Products.Update(product);

            await _context.SaveChangesAsync();

            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<bool> Delete(long id)
        {
            try
            {
                Product product = await _context.Products
                                            .Where(p => p.Id == id)
                                            .FirstOrDefaultAsync() ?? new Product();

                if (product.Id <= 0) 
                    return false;

                _context.Products.Remove(product);
                
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



    }
}