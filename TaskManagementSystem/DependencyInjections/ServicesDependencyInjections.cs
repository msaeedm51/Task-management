using TaskManagementSystem.Service.Interfaces;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.DependencyInjections
{
    public static class ServicesDependencyInjections
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }

    }
}
