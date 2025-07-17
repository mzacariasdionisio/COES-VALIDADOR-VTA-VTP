using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_FPMDETALLE
    /// </summary>
    public interface ICmFpmdetalleRepository
    {
        int Save(CmFpmdetalleDTO entity);
        void Update(CmFpmdetalleDTO entity);
        void Delete(DateTime fecha);
        CmFpmdetalleDTO GetById(int cmfpmdcodi);
        List<CmFpmdetalleDTO> List();
        List<CmFpmdetalleDTO> GetByCriteria(int idPadre);
        List<CmFpmdetalleDTO> ObtenerPorFecha(DateTime fecha);
    }
}
