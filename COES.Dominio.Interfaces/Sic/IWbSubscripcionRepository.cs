using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_SUBSCRIPCION
    /// </summary>
    public interface IWbSubscripcionRepository
    {
        int Save(WbSubscripcionDTO entity);
        void Update(WbSubscripcionDTO entity);
        void Delete(int subscripcodi);
        WbSubscripcionDTO GetById(int subscripcodi);
        List<WbSubscripcionDTO> List();
        List<WbSubscripcionDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin, int? idPublicacion);
        List<WbSubscripcionDTO> ObtenerExportacion(DateTime fechaInicio, DateTime fechaFin, int? idPublicacion);
    }
}
