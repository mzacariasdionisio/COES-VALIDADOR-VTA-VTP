using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_COSTOPORTDETALLE
    /// </summary>
    public interface IVcrCostoportdetalleRepository
    {
        int Save(VcrCostoportdetalleDTO entity);
        void Update(VcrCostoportdetalleDTO entity);
        void Delete(int vcrecacodi);
        VcrCostoportdetalleDTO GetById(int vcrcopcodi);
        List<VcrCostoportdetalleDTO> List();
        List<VcrCostoportdetalleDTO> GetByCriteria();
        List<VcrCostoportdetalleDTO> ListPorMesURS(int vcrecacodi, int grupocodi, int equicodi);
    }
}
