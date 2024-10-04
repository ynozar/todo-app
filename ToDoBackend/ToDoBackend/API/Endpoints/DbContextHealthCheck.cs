using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ToDoBackend.API.Endpoints;

public class DbContextHealthCheck<TContext> : IHealthCheck where TContext : DbContext
{
    private readonly TContext _context;

    public DbContextHealthCheck(TContext context)
    {
        _context = context;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            // Manually specify entity types
            await _context.Set<ToDoItem>().Take(1).ToListAsync(cancellationToken);
            await _context.Set<Group>().Take(1).ToListAsync(cancellationToken);
            await _context.Set<User>().Take(1).ToListAsync(cancellationToken);
            // Add more entities as needed

            return HealthCheckResult.Healthy("Database is accessible and models are queryable.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database is not accessible or models are not queryable.", ex);
        }
    }
}
