using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_YUPCON_CFGDET
    /// </summary>
    public interface ICpYupconCfgdetRepository
    {
        int Save(CpYupconCfgdetDTO entity);
        void Update(CpYupconCfgdetDTO entity);
        void Delete(int ycdetcodi);
        CpYupconCfgdetDTO GetById(int ycdetcodi);
        List<CpYupconCfgdetDTO> List();
        List<CpYupconCfgdetDTO> GetByCriteria(int yupcfgcodi, int recurcodi);
    }
}
