﻿using AutoMapper;
using PortalGalaxy.Entities;
using PortalGalaxy.Entities.Infos;
using PortalGalaxy.Shared.Request;
using PortalGalaxy.Shared.Response;

namespace PortalGalaxy.Services.Profiles;

public class InscripcionProfile : Profile
{
    public InscripcionProfile()
    {
        CreateMap<InscripcionInfo, InscripcionDtoResponse>();
        CreateMap<InscripcionDtoRequest, Inscripcion>();
    }
}