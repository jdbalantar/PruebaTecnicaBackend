using Application.Interfaces.Infrastructure.Repositories;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.TryAddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.TryAddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
