using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_MEDBORNE
    /// </summary>
    public interface IVcrMedborneRepository
    {
        int Save(VcrMedborneDTO entity);
        void BulkInsert(List<VcrMedborneDTO> entitys);
        void Update(VcrMedborneDTO entity);
        void Delete(int vcrecacodi);
        VcrMedborneDTO GetById(int vcrmebcodi);
        List<VcrMedborneDTO> List();
        List<VcrMedborneDTO> GetByCriteria();
        List<VcrMedborneDTO> ListDistintos(int vcrecacodi);
        List<VcrMedborneDTO> ListDiaSinUnidExonRSF(int vcrecacodi, DateTime vcrmebfecha);
        List<VcrMedborneDTO> ListMesConsideradosTotales(int vcrecacodi);
        List<VcrMedborneDTO> ListMes(int vcrecacodi);
        List<VcrMedborneDTO> ListMesConsiderados(int vcrecacodi);
        //ASSETEC 202012
        decimal TotalUnidNoExonRSF(int vcrecacodi);
    }
}
