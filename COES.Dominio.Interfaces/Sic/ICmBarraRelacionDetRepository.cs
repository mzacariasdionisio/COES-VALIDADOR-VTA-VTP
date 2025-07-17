using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_BARRA_RELACION_DET
    /// </summary>
    public interface ICmBarraRelacionDetRepository
    {
        int Save(CmBarraRelacionDetDTO entity);
        void Update(CmBarraRelacionDetDTO entity);
        void Delete(int cmbadecodi);
        CmBarraRelacionDetDTO GetById(int cmbadecodi);
        List<CmBarraRelacionDetDTO> List();
        List<CmBarraRelacionDetDTO> GetByCriteria(int idRelacion);
    }
}
