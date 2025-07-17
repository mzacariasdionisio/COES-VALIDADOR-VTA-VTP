using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IExtEstadoEnvioRepository
    {
        List<ExtEstadoEnvioDTO> List();

    }
}
