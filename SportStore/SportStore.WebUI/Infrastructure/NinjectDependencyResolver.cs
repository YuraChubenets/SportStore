using Moq;
using Ninject;
using SportStore.Domain.Abstract;
using SportStore.Domain.Concrete;
using SportStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
                      {
                          new Game { GameId=1, Name = "SimCity", Price = 1499, Category="Categ1" },
                          new Game {GameId=2, Name = "TITANFALL", Price=2299,Category="Categ2" },
                          new Game {GameId=3, Name = "Battlefield 4", Price=899.4M,Category="Categ1" },
                          new Game {GameId=4, Name = "StarCraft", Price=999,Category="Categ3" },
                          new Game {GameId=5, Name = "HalfLife", Price=785.5m,Category="Categ1" }
                      });
           // kernel.Bind<IGameRepository>().ToConstant(mock.Object);

             kernel.Bind<IGameRepository>().To<EFGameRepository>();
        }
    }
}