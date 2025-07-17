using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_YUPCON_CFG
    /// </summary>
    public interface ICpYupconCfgRepository
    {
        int Save(CpYupconCfgDTO entity);
        void Update(CpYupconCfgDTO entity);
        void Delete(int yupcfgcodi);
        CpYupconCfgDTO GetById(int yupcfgcodi);
        List<CpYupconCfgDTO> List();
        List<CpYupconCfgDTO> GetByCriteria(int tyupcodi, DateTime fechaConsulta, int hora);
    }
}
