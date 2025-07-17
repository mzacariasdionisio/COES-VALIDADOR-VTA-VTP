using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_CARGOINCUMPL
    /// </summary>
    public interface IVcrCargoincumplRepository
    {
        int Save(VcrCargoincumplDTO entity);
        void Update(VcrCargoincumplDTO entity);
        void Delete(int vcrecacodi);
        VcrCargoincumplDTO GetById(int vcrecacodi, int equicodi);
        List<VcrCargoincumplDTO> List(int vcrecacodi);
        List<VcrCargoincumplDTO> GetByCriteria();
        //ASSETEC 202012
        List<VcrCargoincumplDTO> ListCargoIncumplGrupoCalculado(int vcrecacodi);
        decimal TotalMesServicioRSFConsiderados(int vcrecacodi);
    }
}
