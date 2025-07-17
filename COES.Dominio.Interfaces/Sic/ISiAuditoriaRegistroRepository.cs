using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_AUDITORIA_REGISTRO
    /// </summary>
    public interface ISiAuditoriaRegistroRepository
    {
        void Update(SiAuditoriaRegistroDTO entity);
        int Save(SiAuditoriaRegistroDTO entity);
        void Delete();
        SiAuditoriaRegistroDTO GetById();
        List<SiAuditoriaRegistroDTO> GetByUsuariosAuditoria();
        List<SiAuditoriaRegistroDTO> List(int Taudit, int id);
        List<SiAuditoriaRegistroDTO> GetByCriteria();
    }
}
