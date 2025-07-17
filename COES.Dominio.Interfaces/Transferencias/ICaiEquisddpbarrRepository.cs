using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CAI_EQUISDDPBARR
    /// </summary>
    public interface ICaiEquisddpbarrRepository
    {
        int Save(CaiEquisddpbarrDTO entity);
        void Update(CaiEquisddpbarrDTO entity);
        void Delete(int casddbcodi);
        CaiEquisddpbarrDTO GetById(int casddbcodi);
        CaiEquisddpbarrDTO GetByIdCaiEquisddpbarr(int casddbcodi);
        CaiEquisddpbarrDTO GetByNombreBarraSddp(string sddpgmnombre);
        List<CaiEquisddpbarrDTO> List();
        List<CaiEquisddpbarrDTO> ListCaiEquisddpbarr();
        List<CaiEquisddpbarrDTO> GetByCriteria();
        List<CaiEquisddpbarrDTO> GetByCriteriaCaiEquiunidbarrsNoIns();
        CaiEquisddpbarrDTO GetByBarraNombreSddp(int barrcodi, string sddpgmnombre);
    }
}
