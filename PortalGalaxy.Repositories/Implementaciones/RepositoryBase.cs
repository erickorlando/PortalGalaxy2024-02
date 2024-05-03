using Microsoft.EntityFrameworkCore;
using PortalGalaxy.DataAccess;
using PortalGalaxy.Entities;
using PortalGalaxy.Repositories.Interfaces;
using System.Linq.Expressions;

namespace PortalGalaxy.Repositories.Implementaciones;

public class RepositoryBase<TEntity>(PortalGalaxyDbContext context) : IRepositoryBase<TEntity>
    where TEntity : EntityBase
{
    protected readonly PortalGalaxyDbContext Context = context;

    public async Task<ICollection<TEntity>> ListAsync()
    {
        return await Context.Set<TEntity>()
            .AsNoTracking() // Esto permite que no me traiga el ChangeTracker
            .ToListAsync();
    }

    public async Task<ICollection<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Context.Set<TEntity>()
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<TEntity?> FindByIdAsync(int id)
    {
        return await Context.Set<TEntity>().FindAsync(id);
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await Context.Set<TEntity>().AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync()
    {
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var registro = await FindByIdAsync(id);
        if (registro is not null)
        {
            registro.Estado = false;
            await UpdateAsync();
        }
    }
}