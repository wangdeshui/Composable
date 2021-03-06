﻿using AccountManagement.UI.QueryModels.EventStoreGenerated;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Composable.Windsor;
using JetBrains.Annotations;

namespace AccountManagement.UI.QueryModels.ContainerInstallers
{
    [UsedImplicitly]
    public class QueryModelGeneratorsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Kernel.Resolver.AddSubResolver(new TypedCollectionResolver<IAccountManagementQueryModelGenerator>(container.Kernel));

            container.Register(
                //Note the use of a custom interface. This lets us keep query model generators for different systems apart in the wiring easily.
                Classes.FromThisAssembly()
                    .IncludeNonPublicTypes()
                    .BasedOn(typeof(IAccountManagementQueryModelGenerator))
                    .WithServiceBase()
                    .LifestylePerWebRequest()
                );
        }
    }
}
