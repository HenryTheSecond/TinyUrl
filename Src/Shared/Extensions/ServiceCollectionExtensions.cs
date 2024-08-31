using Microsoft.Extensions.DependencyInjection;
using Shared.Attributes;
using System.Reflection;

namespace Shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static Dictionary<LifeCycle, MethodInfo> registerMethodByLifeCycle;
        static ServiceCollectionExtensions()
        {
            var accessModifier = BindingFlags.Static | BindingFlags.Public;
            Type[] parameters = [typeof(IServiceCollection), typeof(Type), typeof(Type)];

            registerMethodByLifeCycle = new()
            {
                { LifeCycle.SINGLETON, typeof(ServiceCollectionServiceExtensions).GetMethod("AddSingleton", accessModifier, parameters)!},
                { LifeCycle.SCOPE, typeof(ServiceCollectionServiceExtensions).GetMethod("AddScoped", accessModifier, parameters)!},
                { LifeCycle.TRANSIENT, typeof(ServiceCollectionServiceExtensions).GetMethod("AddTransient", accessModifier, parameters)!}
            };
        }

        public static IServiceCollection AddExportedServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetCallingAssembly();
            var exportedServices = assembly.GetExportedTypes().Where(x => x.GetCustomAttribute<ExportAttribute>() != null);
            foreach (var exportedService in exportedServices)
            {
                var lifeCycle = exportedService.GetCustomAttribute<ExportAttribute>()!.LifeCycle;
                var interfaces = exportedService.GetInterfaces();
                foreach (var iinterface in interfaces)
                {
                    registerMethodByLifeCycle[lifeCycle].Invoke(null, [services, iinterface, exportedService]);
                }    
            }
            return services;
        }
    }
}
