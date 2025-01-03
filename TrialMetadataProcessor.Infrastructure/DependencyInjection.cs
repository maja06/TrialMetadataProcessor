using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrialMetadataProcessor.Domain.Interfaces.Repositories.Base;
using TrialMetadataProcessor.Domain.Interfaces.Repositories.ClinicalTrials;
using TrialMetadataProcessor.Domain.Interfaces.UnitOfWork;
using TrialMetadataProcessor.Infrastructure.Data.Context;
using TrialMetadataProcessor.Infrastructure.Repositories.Base;
using TrialMetadataProcessor.Infrastructure.Repositories.ClinicalTrials;

namespace TrialMetadataProcessor.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped<IClinicalTrialRepository, ClinicalTrialRepository>();
            services.AddScoped<IUnitOfWork, Infrastructure.UnitOfWork.UnitOfWork>();

            return services;
        }
    }
}
