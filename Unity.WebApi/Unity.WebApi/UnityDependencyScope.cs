using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

using Microsoft.Practices.Unity;

namespace Unity.WebApi
{
    public class UnityDependencyScope : IDependencyScope
    {
        public UnityDependencyScope(IUnityContainer container)
        {
            Container = container;
        }

        protected IUnityContainer Container { get; private set; }

        public void Dispose()
        {
            Container.Dispose();
        }
        
        public object GetService(Type serviceType)
        {
            if (!Container.IsRegistered(serviceType))
            {
                if (serviceType.IsAbstract || serviceType.IsInterface)
                {
                    return null;
                }
            }

            return Container.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Container.ResolveAll(serviceType);
        }
    }
}