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

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IProductServices, ProductServices>();
            container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<ICategoryServices, CategoryServices>();
            container.RegisterType<ICategoryRepository, CategoryRepository>();
            container.RegisterType<IAddressRepository, AddressRepository>();
            container.RegisterType<IAddressServices, AddressServices>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}