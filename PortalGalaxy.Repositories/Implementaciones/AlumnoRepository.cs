using Microsoft.EntityFrameworkCore;
using PortalGalaxy.DataAccess;
using PortalGalaxy.Entities;
using PortalGalaxy.Repositories.Interfaces;

namespace PortalGalaxy.Repositories.Implementaciones;

public class AlumnoRepository(PortalGalaxyDbContext context) : RepositoryBase<Alumno>(context), IAlumnoRepository
{
    public async Task<Alumno?> FindByEmailAsync(string email)
    {
        return await Context.Set<Alumno>()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Correo == email);
    }
}