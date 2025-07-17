using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IRdoCumplimientoRepository
    {
        
        //List<EpoEstudioEoDTO> List();
        List<RdoCumplimiento> GetByCriteria(RdoCumplimiento estudioeo);
        //int ObtenerNroRegistroBusqueda(EpoEstudioEoDTO estudioeo);
        
    }
}
