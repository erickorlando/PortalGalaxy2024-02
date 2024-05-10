﻿using PortalGalaxy.Entities;
using PortalGalaxy.Entities.Infos;

namespace PortalGalaxy.Repositories.Interfaces;

public interface ITallerRepository : IRepositoryBase<Taller>
{
    Task<ICollection<Taller>> ListarAsync();

    Task<(ICollection<TallerInfo> Collection, int Total)> ListarTalleresAsync(string? nombre, int? categoriaId,
        int? situacion, int pagina, int filas);
}