using Microsoft.EntityFrameworkCore;
using ScentSphere.Application.DTOs.Products;
using ScentSphere.Application.Interfaces;
using ScentSphere.Domain.Entities;
using ScentSphere.Persistence.Context;

namespace ScentSphere.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            return await _context.Products
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.Images)
                .Where(x => !x.IsDeleted)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    Brand = p.Brand.Name,
                    Category = p.Category.Name,

                    Images = p.Images
                        .OrderBy(i => i.DisplayOrder)
                        .Select(i => new ProductImageDto
                        {
                            ImageUrl = i.ImageUrl,
                            DisplayOrder = i.DisplayOrder
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<ProductDto?> GetByIdAsync(Guid id)
        {
            return await _context.Products
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.Images)
                .Where(x =>
                    x.Id == id &&
                    !x.IsDeleted)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    Brand = p.Brand.Name,
                    Category = p.Category.Name,

                    Images = p.Images
                        .OrderBy(i => i.DisplayOrder)
                        .Select(i => new ProductImageDto
                        {
                            ImageUrl = i.ImageUrl,
                            DisplayOrder = i.DisplayOrder
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<ProductDto>> SearchAsync(string keyword)
        {
            return await _context.Products
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.Images)
                .Where(x =>
                    !x.IsDeleted &&
                    (
                        x.Name.Contains(keyword) ||
                        x.Description.Contains(keyword)
                    ))
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    Brand = p.Brand.Name,
                    Category = p.Category.Name,

                    Images = p.Images
                        .OrderBy(i => i.DisplayOrder)
                        .Select(i => new ProductImageDto
                        {
                            ImageUrl = i.ImageUrl,
                            DisplayOrder = i.DisplayOrder
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<ProductResponse> CreateAsync(
            CreateProductRequest request)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                IsFeatured = request.IsFeatured,
                BrandId = request.BrandId,
                CategoryId = request.CategoryId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return new ProductResponse
            {
                IsSuccess = true,
                Message = "Product created successfully"
            };
        }

        public async Task<ProductResponse> UpdateAsync(
            Guid id,
            UpdateProductRequest request)
        {
            var product =
                await _context.Products
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return new ProductResponse
                {
                    IsSuccess = false,
                    Message = "Product not found"
                };
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.StockQuantity = request.StockQuantity;
            product.IsFeatured = request.IsFeatured;
            product.BrandId = request.BrandId;
            product.CategoryId = request.CategoryId;

            await _context.SaveChangesAsync();

            return new ProductResponse
            {
                IsSuccess = true,
                Message = "Product updated successfully"
            };
        }

        public async Task<ProductResponse> DeleteAsync(Guid id)
        {
            var product =
                await _context.Products
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return new ProductResponse
                {
                    IsSuccess = false,
                    Message = "Product not found"
                };
            }

            product.IsDeleted = true;

            await _context.SaveChangesAsync();

            return new ProductResponse
            {
                IsSuccess = true,
                Message = "Product deleted successfully"
            };
        }
        public async Task<List<AdminProductDto>>
        GetAdminProductsAsync()
        {
            return await _context.Products
                .Select(x => new AdminProductDto
                {
                    Id = x.Id,

                    Name = x.Name,

                    Description = x.Description,

                    Price = x.Price,

                    StockQuantity =
                        x.StockQuantity,

                    IsFeatured =
                        x.IsFeatured,

                    BrandId =
                        x.BrandId,

                    CategoryId =
                        x.CategoryId
                })
                .ToListAsync();
        }


        public async Task<AdminProductDto?>GetAdminProductByIdAsync(Guid id)
        {
            return await _context.Products
                .Where(x => x.Id == id)
                .Select(x => new AdminProductDto
                {
                    Id = x.Id,

                    Name = x.Name,

                    Description = x.Description,

                    Price = x.Price,

                    StockQuantity =
                        x.StockQuantity,

                    IsFeatured =
                        x.IsFeatured,

                    BrandId =
                        x.BrandId,

                    CategoryId =
                        x.CategoryId
                })
                .FirstOrDefaultAsync();
        }
    }
}