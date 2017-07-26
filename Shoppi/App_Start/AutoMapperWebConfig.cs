using AutoMapper;
using Shoppi.Data.Models;
using Shoppi.Models;
using Shoppi.Models.Account;

namespace Shoppi.App_Start
{
    public static class AutoMapperWebConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new ShoppiProfile());
            });
        }
    }

    public class ShoppiProfile : Profile
    {
        public ShoppiProfile()
        {
            CreateProductMaps();
            CreateCategoryMaps();
            CreateAccountMaps();
        }

        private void CreateProductMaps()
        {
            CreateMap<ProductCreateViewModel, Product>();
            CreateMap<Product, ProductEditViewModel>();
            CreateMap<ProductEditViewModel, Product>();
            CreateMap<Product, ProductDeleteViewModel>();
        }

        private void CreateCategoryMaps()
        {
            CreateMap<CategoryCreateViewModel, Category>();
            CreateMap<CategoryEditViewModel, Category>();
            CreateMap<Category, CategoryEditViewModel>();
            CreateMap<Category, CategoryDeleteViewModel>();
        }

        private void CreateAccountMaps()
        {
            CreateMap<AccountRegisterViewModel, ShoppiUser>();
        }
    }
}