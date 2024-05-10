﻿using System.Globalization;
using AutoMapper;
using PortalGalaxy.Entities.Infos;
using PortalGalaxy.Shared.Response;

namespace PortalGalaxy.Services.Profiles;

public class TallerProfile : Profile
{
    public TallerProfile()
    {
        var configuracionRegional = new CultureInfo("es-MX");
        
        CreateMap<TallerInfo, TallerDtoResponse>()
            .ForMember(d => d.Fecha, o => o.MapFrom(x => x.Fecha.ToString("d", configuracionRegional))); //05/06/2024
    }
}