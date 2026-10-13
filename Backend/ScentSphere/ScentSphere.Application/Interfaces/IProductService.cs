using ScentSphere.Application.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScentSphere.Application.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();

        Task<ProductDto?> GetByIdAsync(Guid id);

        Task<List<ProductDto>> SearchAsync(string keyword);

        Task<ProductResponse> CreateAsync(
            CreateProductRequest request);

        Task<ProductResponse> UpdateAsync(
            Guid id,
            UpdateProductRequest request);

        Task<ProductResponse> DeleteAsync(Guid id);

        Task<List<AdminProductDto>>
        GetAdminProductsAsync();

        Task<AdminProductDto?>GetAdminProductByIdAsync(Guid id);
    }
}
