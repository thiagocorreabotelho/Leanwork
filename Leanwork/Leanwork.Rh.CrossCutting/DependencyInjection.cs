using Leanwork.Rh.Application;
using Leanwork.Rh.Application.Interface;
using Leanwork.Rh.Application.Notification;
using Leanwork.Rh.Application.Service;
using Leanwork.Rh.Domain;
using Leanwork.Rh.Domain.Interface;
using Leanwork.Rh.Infrastructure;
using Leanwork.Rh.Infrastructure.Repository;

using Microsoft.Extensions.DependencyInjection;

namespace Leanwork.Rh.CrossCutting
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Configura a injeção de dependência das camadas do projeto.
        /// </summary>
        /// <param name="services">Coleção de serviços do ASP.NET Core.</param>
        /// <returns>A coleção de serviços do ASP.NET Core após a configuração da injeção de dependência.</returns>
        public static IServiceCollection LayerDependency(this IServiceCollection services)
        {
            RepositoryDependency(services);
            ServicesDependency(services);
            OutherDependency(services);

            return services;
        }

        private static void RepositoryDependency(IServiceCollection services)
        {
            services.AddScoped<IRepositoryTechnology, RepositoryTechnology>();
            services.AddScoped<IRepositoryCompany, RepositoryCompany>();
            services.AddScoped<IRepositoryCandidate, RepositoryCandidate>();
            services.AddScoped<IRepositoryGender, RepositoryGender>();
            services.AddScoped<IRepositoryAddress, RepositoryAddress>();
            services.AddScoped<IRepositoryCandidateTechnologyRel, RepositoryCandidateTechnologyRel>();
            services.AddScoped<IRepositoryCompanyTechnologyRel, RepositoryCompanyTechnologyRel>();
            services.AddScoped<IRepositoryJobOpening, RepositoryJobOpening>();
            services.AddScoped<IRepositoryResponsibility, RepositoryResponsibility>();
            services.AddScoped<IRepositoryInterview, RepositoryInterview>();
            services.AddScoped<IRepositoryJobInterviewWeight, RepositoryJobInterviewWeight>();
            services.AddScoped<IRepositoryReportCandidate, RepositoryReportCandidate>();
        }

        private static void ServicesDependency(IServiceCollection services)
        {
            services.AddScoped<IServiceTechnology, ServiceTechnology>();
            services.AddScoped<IServiceCompany, ServiceCompany>();
            services.AddScoped<IServiceCandidate, ServiceCandidate>();
            services.AddScoped<IServiceGender, ServiceGender>();
            services.AddScoped<IServiceAddress, ServiceAddress>();
            services.AddScoped<IServiceCandidateTechnologyRel, ServiceCandidateTechnologyRel>();
            services.AddScoped<IServiceCompanyTechnologyRel, ServiceCompanyTechnologyRel>();
            services.AddScoped<IServiceJobOpening, ServiceJobOpening>();
            services.AddScoped<IServiceReesponsibility, ServiceResponsibility>();
            services.AddScoped<IServiceInterview, ServiceInterview>();
            services.AddScoped<IServiceJobInterviewWeight, ServiceJobInterviewWeight>();
            services.AddScoped<IServiceReportCandidate, ServiceReportCandidate>();
        }

        private static void OutherDependency(IServiceCollection services)
        {
            services.AddScoped<INotificationError, NoitificationErro>();
        }
    }
}
