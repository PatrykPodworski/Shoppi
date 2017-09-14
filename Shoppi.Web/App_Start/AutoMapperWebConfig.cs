using AutoMapper;
using Shoppi.Data.Models;
using Shoppi.Logic.Factories;
using Shoppi.Web.Models.AccountViewModels;
using Shoppi.Web.Models.AddressViewModels;
using Shoppi.Web.Models.BrandViewModels;
using Shoppi.Web.Models.CartViewModels;
using Shoppi.Web.Models.CategoryViewModels;
using Shoppi.Web.Models.ProductViewModels;
using System.Web.Mvc;

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
            CreateAddressMaps();
            CreateCartMaps();
            CreateBrandMaps();
        }

        private void CreateProductMaps()
        {
            CreateMap<ProductCreateViewModel, Product>();
            CreateMap<Product, ProductEditViewModel>();
            CreateMap<ProductEditViewModel, Product>();
            CreateMap<Product, ProductDeleteViewModel>();
            CreateMap<Product, ProductDetailsViewModel>();
            CreateMap<ProductType, SelectListItem>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id.ToString()));
            CreateMap<ProductIndexViewModel, ProductSpecificationFilters>();
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
            CreateMap<ShoppiUser, MyAccountViewModel>();
            CreateMap<Address, DefaultAddressViewModel>();
        }

        private void CreateAddressMaps()
        {
            CreateMap<Address, AddressIndexPart>();
            CreateMap<AddressCreateViewModel, Address>();
            CreateMap<Address, AddressDeleteViewModel>();
            CreateMap<Address, AddressEditViewModel>();
            CreateMap<AddressEditViewModel, Address>();
        }

        private void CreateCartMaps()
        {
            CreateMap<Cart, CartIndexViewModel>();
            CreateMap<CartLine, CartLineViewModel>();
        }

        private void CreateBrandMaps()
        {
            CreateMap<BrandCreateViewModel, Brand>();
        }
    }
}