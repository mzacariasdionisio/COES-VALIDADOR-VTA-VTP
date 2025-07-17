using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CAI_EQUISDDPUNI
    /// </summary>
    public interface ICaiEquisddpuniRepository
    {
        int Save(CaiEquisddpuniDTO entity);
        void Update(CaiEquisddpuniDTO entity);
        void Delete(int casdducodi);
        CaiEquisddpuniDTO GetById(int casdducodi);
        CaiEquisddpuniDTO GetByIdCaiEquisddpuni(int casdducodi);
        CaiEquisddpuniDTO GetByNombreEquipoSddp(string sddpgmnombre);
        List<CaiEquisddpuniDTO> List();
        List<CaiEquisddpuniDTO> ListCaiEquisddpuni();
        List<CaiEquisddpuniDTO> GetByCriteria();
        List<CaiEquisddpuniDTO> GetByCriteriaCaiEquiunidbarrsNoIns();
        List<CentralGeneracionDTO> Unidad();
    }
}
