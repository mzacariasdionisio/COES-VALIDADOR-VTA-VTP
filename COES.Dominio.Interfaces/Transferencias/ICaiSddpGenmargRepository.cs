using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CAI_SDDP_GENMARG
    /// </summary>
    public interface ICaiSddpGenmargRepository
    {
        int Save(CaiSddpGenmargDTO entity);
        void Update(CaiSddpGenmargDTO entity);
        void Delete(string tipo);
        CaiSddpGenmargDTO GetById(int sddpgmcodi);
        List<CaiSddpGenmargDTO> List();
        List<CaiSddpGenmargDTO> GetByCriteria(string sddpgmtipo);
        void BulkInsert(List<CaiSddpGenmargDTO> entitys);
        int GetCodigoGenerado();
        decimal GetSumSddpGenmargByEtapaB1(int sddpgmetapa, string sddpgmnombre);
        decimal GetSumSddpGenmargByEtapaB2(int sddpgmetapa, string sddpgmnombre);
        decimal GetSumSddpGenmargByEtapaB3(int sddpgmetapa, string sddpgmnombre);
        decimal GetSumSddpGenmargByEtapaB4(int sddpgmetapa, string sddpgmnombre);
        decimal GetSumSddpGenmargByEtapaB5(int sddpgmetapa, string sddpgmnombre);
        List<CaiSddpGenmargDTO> GetByCriteriaCaiSddpGenmargsBarrNoIns(string sddpgmtipo);
        decimal GetSumSddpGenmargByEtapa(string sddpgmtipo, int sddpgmetapa, int sddpgmbloque, string sddpgmnombre);
    }
}
