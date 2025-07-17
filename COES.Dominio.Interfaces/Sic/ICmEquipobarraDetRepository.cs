using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_EQUIPOBARRA_DET
    /// </summary>
    public interface ICmEquipobarraDetRepository
    {
        int Save(CmEquipobarraDetDTO entity);
        void Update(CmEquipobarraDetDTO entity);
        void Delete(int cmebdecodi);
        CmEquipobarraDetDTO GetById(int cmebdecodi);
        List<CmEquipobarraDetDTO> List();
        List<CmEquipobarraDetDTO> GetByCriteria(int idPadre);
    }
}
