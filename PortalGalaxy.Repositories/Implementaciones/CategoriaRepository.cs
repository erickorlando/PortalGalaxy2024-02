using Microsoft.EntityFrameworkCore;
using PortalGalaxy.DataAccess;
using PortalGalaxy.Entities;
using PortalGalaxy.Repositories.Interfaces;

namespace PortalGalaxy.Repositories.Implementaciones;

public class CategoriaRepository(PortalGalaxyDbContext context) : 
    RepositoryBase<Categoria>(context), ICategoriaRepository
{
    public async Task<ICollection<Categoria>> ListarEliminados()
    {
        return await Context.Set<Categoria>()
            .AsNoTracking()
            .IgnoreQueryFilters() // ignora los query filters
            .ToListAsync();
    }
}