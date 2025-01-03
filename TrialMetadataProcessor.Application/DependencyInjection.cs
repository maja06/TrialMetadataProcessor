
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TrialMetadataProcessor.Application.ClinicalTrials.Commands.CreateClinicalTrial;
using TrialMetadataProcessor.Application.ClinicalTrials.DTOs;
using TrialMetadataProcessor.Application.ClinicalTrials.Queries.GetTrialById;
using TrialMetadataProcessor.Application.ClinicalTrials.Queries.GetTrialsByFilterQuery;
using TrialMetadataProcessor.Application.ClinicalTrials.Queries.GetTrialsByStatus;
using TrialMetadataProcessor.Application.FileValidation.Services;

namespace TrialMetadataProcessor.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            services.AddScoped<IRequestHandler<CreateClinicalTrialCommand, Guid>, CreateClinicalTrialCommandHandler>();
            services.AddScoped<IRequestHandler<GetClinicalTrialByIdQuery, GetClinicalTrialDto>, GetClinicalTrialByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetTrialsByStatusQuery, IEnumerable<GetClinicalTrialDto>>, GetTrialsByStatusQueryHandler>();
            services.AddScoped<IRequestHandler<GetTrialsByFilterQuery, IEnumerable<GetClinicalTrialDto>>, GetTrialsByFilterQueryHandler>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            services.AddScoped<IFileValidationService, FileValidationService>();

            return services;
        }
    }
}
