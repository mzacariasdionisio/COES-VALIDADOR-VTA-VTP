using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnMedicionesRawRepository
    {
        List<PrnMedicionesRawDTO> List();
        PrnMedicionesRawDTO GetById(int codigo);
        void Delete(int codigo);
        void Save(PrnMedicionesRawDTO entity);
        void Update(PrnMedicionesRawDTO entity);
        List<PrnMedicionesRawDTO> ListMedicionesRaw(int unidad, DateTime fecha, int idVariable, string idFuente, string modulo);
        List<PrnMedicionesRawDTO> ListMedicionesRawPorRango(int unidad, DateTime fechaInicio, DateTime fechaFin, int idVariable, string idFuente, string modulo);
        List<PrnMedicionesRawDTO> ListMedicionesRawPorAsociacion(int unidad, DateTime fecha, int idVariable, string idFuente, string modulo);
        List<PrnMedicionesRawDTO> ListMedicionesRawPorRangoPorAsociacion(int unidad, DateTime fechaInicio, DateTime fechaFin, int idVariable, string idFuente, string modulo);
        PrnMedicionesRawDTO GetByKey(PrnMedicionesRawDTO entidad);
    }
}
