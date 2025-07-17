using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_TABLA_AUDITABLE
    /// </summary>
    public interface ISiTablaAuditableRepository
    {
        int Save(SiTablaAuditableDTO entity);
        void Update(SiTablaAuditableDTO entity);
        void Delete(int configcodi);
        SiTablaAuditableDTO GetById(int configcodi);
        List<SiTablaAuditableDTO> List();
        List<SiTablaAuditableDTO> GetByCriteria();
        List<fwUserDTO> ListUserRol(int rolcode);
    }
}
