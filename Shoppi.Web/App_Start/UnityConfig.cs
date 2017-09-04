using Microsoft.Practices.Unity;
using Shoppi.Data.Abstract;
using Shoppi.Data.Repositories;
using Shoppi.Logic.Abstract;
using Shoppi.Logic.Implementation;
using System.Web.Mvc;
using Unity.Mvc5;

namespace Shoppi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            RegisterServices(container);
            RegisterRepositories(container);

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static void RegisterServices(IUnityContainer container)
        {
            container.RegisterType<IProductServices, ProductServices>();
            container.RegisterType<ICategoryServices, CategoryServices>();
            container.RegisterType<IAddressServices, AddressServices>();
            container.RegisterType<IUserServices, UserServices>();
            container.RegisterType<ICartServices, CartServices>();
            container.RegisterType<IImageServices, ImageServices>();
            container.RegisterType<IBrandServices, BrandServices>();
            container.RegisterType<ITypeServices, TypeServices>();
        }

        private static void RegisterRepositories(IUnityContainer container)
        {
            container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<ICategoryRepository, CategoryRepository>();
            container.RegisterType<IAddressRepository, AddressRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<ICartRepository, SessionCartRepository>();
            container.RegisterType<IBrandRepository, BrandRepository>();
            container.RegisterType<ITypeRepository, TypeRepository>();
        }
    }
}