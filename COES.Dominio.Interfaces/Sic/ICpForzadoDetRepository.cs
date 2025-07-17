using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_FORZADO_DET
    /// </summary>
    public interface ICpForzadoDetRepository
    {
        int Save(CpForzadoDetDTO entity);
        void Update(CpForzadoDetDTO entity);
        void Delete(int cpfzdtcodi);
        CpForzadoDetDTO GetById(int cpfzdtcodi);
        List<CpForzadoDetDTO> List();
        List<CpForzadoDetDTO> GetByCriteria();
        List<CpForzadoDetDTO> GetByCpfzcodi(int cpfzcodi);
    }
}
