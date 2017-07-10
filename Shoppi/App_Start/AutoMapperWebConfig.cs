using AutoMapper;
using Shoppi.Data.Models;
using Shoppi.Models;

namespace Shoppi.App_Start
{
    public static class AutoMapperWebConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new ProductProfile());
            });
        }
    }

    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductCreateViewModel, Product>();
        }
    }
}