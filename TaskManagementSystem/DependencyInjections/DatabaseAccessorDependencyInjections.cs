using TaskManagementSystem.DatabaseAccessors;
using TaskManagementSystem.DatabaseAccessors.Interfaces;

namespace TaskManagementSystem.DependencyInjections
{
    public static class DatabaseAccessorDependencyInjections
    {
        public static IServiceCollection AddDatabaseAccessors(this IServiceCollection services)
        {
            services.AddScoped<IDocumentAccessor, DocumentAccessor>();
            services.AddScoped<INoteAccessor, NoteAccessor>();
            services.AddScoped<IRefreshTokenAccessor, RefreshTokenAccessor>();
            services.AddScoped<IRoleAccessor, RoleAccessor>();
            services.AddScoped<ITaskAccessor, TaskAccessor>();
            services.AddScoped<ITeamAccessor, TeamAccessor>();
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<IUserRoleAccessor, UserRoleAccessor>();
            return services;
        }
    }
}
