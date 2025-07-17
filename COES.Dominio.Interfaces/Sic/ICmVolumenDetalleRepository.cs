using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_VOLUMEN_DETALLE
    /// </summary>
    public interface ICmVolumenDetalleRepository
    {
        int Save(CmVolumenDetalleDTO entity);
        void Update(CmVolumenDetalleDTO entity);
        void Delete(int voldetcodi);
        CmVolumenDetalleDTO GetById(int voldetcodi);
        List<CmVolumenDetalleDTO> List();
        List<CmVolumenDetalleDTO> GetByCriteria(int volcalcodi);
    }
}
