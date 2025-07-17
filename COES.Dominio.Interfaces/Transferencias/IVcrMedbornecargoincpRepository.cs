using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_MEDBORNECARGOINCP
    /// </summary>
    public interface IVcrMedbornecargoincpRepository
    {
        int Save(VcrMedbornecargoincpDTO entity);
        void Update(VcrMedbornecargoincpDTO entity);
        void Delete(int vcrecacodi);
        VcrMedbornecargoincpDTO GetById(int vcmbcicodi);
        List<VcrMedbornecargoincpDTO> List(int vcrecacodi);
        List<VcrMedbornecargoincpDTO> GetByCriteria();
        void UpdateVersionNO(int vcrecacodi, string sUser);
        void UpdateVersionSI(int vcrecacodi, string sUser, int vcmbcicodi);
    }
}
