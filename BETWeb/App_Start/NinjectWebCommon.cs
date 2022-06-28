[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(BETWeb.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(BETWeb.App_Start.NinjectWebCommon), "Stop")]

namespace BETWeb.App_Start
{
    using System;
    using System.Web;
    using BET.Domain;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using BET.Domain.Interfaces;
    using BETWeb.Managers;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            var uow = new BETUnitOfWork(new App_Data.BETDataEntities());
            kernel.Bind<IBETUnitOfWork>().ToConstructor(i => new BETUnitOfWork(new App_Data.BETDataEntities()));
            //kernel.Bind<ICart>().ToConstructor(i => new CartManager(uow));
            //kernel.Bind<IProducts>().ToConstructor(i => new ProductsManager(uow));
            //kernel.Bind<ICheckout>().ToConstructor(i => new CheckoutManager(uow));

            kernel.Bind<ICart>().To<CartManager>();
            kernel.Bind<IProducts>().To<ProductsManager>();
        }        
    }
}
