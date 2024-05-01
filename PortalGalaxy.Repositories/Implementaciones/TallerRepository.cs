using Microsoft.EntityFrameworkCore;
using PortalGalaxy.DataAccess;
using PortalGalaxy.Entities;
using PortalGalaxy.Repositories.Interfaces;

namespace PortalGalaxy.Repositories.Implementaciones;

public class TallerRepository(PortalGalaxyDbContext context)
    : RepositoryBase<Taller>(context), ITallerRepository
{
    public async Task<ICollection<Taller>> ListarAsync()
    {
        return await Context.Set<Taller>()
            .Include(p => p.Categoria)
            .AsNoTracking()
            .ToListAsync();
    }
}