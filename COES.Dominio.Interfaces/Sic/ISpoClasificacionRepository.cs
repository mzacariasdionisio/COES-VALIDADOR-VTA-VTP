using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SPO_CLASIFICACION
    /// </summary>
    public interface ISpoClasificacionRepository
    {
        int Save(SpoClasificacionDTO entity);
        void Update(SpoClasificacionDTO entity);
        void Delete(int clasicodi);
        SpoClasificacionDTO GetById(int clasicodi);
        List<SpoClasificacionDTO> List();
        List<SpoClasificacionDTO> GetByCriteria();
    }
}
