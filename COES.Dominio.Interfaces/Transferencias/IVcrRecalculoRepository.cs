using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_RECALCULO
    /// </summary>
    public interface IVcrRecalculoRepository
    {
        int Save(VcrRecalculoDTO entity);
        void Update(VcrRecalculoDTO entity);
        void Delete(int vcrecacodi);
        VcrRecalculoDTO GetById(int vcrecacodi);
        VcrRecalculoDTO GetByIncumplimiento(int vcrinccodi);
        List<VcrRecalculoDTO> List(int pericodi);
        List<VcrRecalculoDTO> GetByCriteria();
        VcrRecalculoDTO GetByIdView(int pericodi, int vcrecacodi);
        List<VcrRecalculoDTO> ListAllRecalculos();
        VcrRecalculoDTO GetByIdUpDate(int pericodi, int vcrecacodi);
        List<VcrRecalculoDTO> ListReg(int vcrecacodi);
        VcrRecalculoDTO GetByIdViewIndex(int pericodi, int vcrecacodi);
    }
}
