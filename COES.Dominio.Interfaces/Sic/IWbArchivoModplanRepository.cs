using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_ARCHIVO_MODPLAN
    /// </summary>
    public interface IWbArchivoModplanRepository
    {
        int Save(WbArchivoModplanDTO entity);
        void Update(WbArchivoModplanDTO entity);
        void Delete(int arcmplcodi);
        WbArchivoModplanDTO GetById(int arcmplcodi);
        List<WbArchivoModplanDTO> List();
        List<WbArchivoModplanDTO> GetByCriteria(int vermplcodi);
        WbArchivoModplanDTO ObtenerDocumento(int idVersion, string tipo);
    }
}
