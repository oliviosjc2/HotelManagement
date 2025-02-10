using FluentValidation;
using HM.Application.Events.Hotel;
using HM.Application.Handlers.Hotel;
using HM.Application.Validators.Hotel;
using HM.Domain.Entities;
using HM.Domain.Repositories;
using HM.Infra.Cache;
using HM.Infra.Cache.Interfaces;
using HM.Infra.Context;
using HM.Infra.Repositories;
using HM.Infra.RequestContext;
using HM.Infra.UOW;

namespace HM.API.Helpers
{
    public static class DependencyInjection
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.AddResponseCaching();
            services.AddMemoryCache();

            services.AddMediatR(m => m.RegisterServicesFromAssembly(typeof(CreateHotelCommandHandler).Assembly));
            services.AddValidatorsFromAssemblyContaining<CreateHotelCommandValidator>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<HMContext>();

            services.AddScoped<IRepository<Hotel>, Repository<Hotel>>();

            services.AddScoped<DatabaseSeeder>();

            services.AddScoped<IUserContextService, UserContextService>();

            services.AddScoped<HotelCreatedEvent>();
            services.AddScoped<UpdateReserveNoPaidEvent>();
            services.AddScoped<ReserveCreatedEvent>();

            services.AddScoped<IReserveCacheService, ReserveCacheService>();
        }
    }
}
