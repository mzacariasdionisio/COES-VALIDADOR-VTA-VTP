using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_TERMSUPERAVIT
    /// </summary>
    public interface IVcrTermsuperavitRepository
    {
        int Save(VcrTermsuperavitDTO entity);
        void Update(VcrTermsuperavitDTO entity);
        void Delete(int vcrecacodi);
        VcrTermsuperavitDTO GetById(int vcrtscodi);
        List<VcrTermsuperavitDTO> List();
        List<VcrTermsuperavitDTO> GetByCriteria();
        List<VcrTermsuperavitDTO> ListPorMesURS(int vcrecacodi, int grupocodi);
        VcrTermsuperavitDTO GetByIdDia(int vcrecacodi, int grupocodi, DateTime vcrtsfecha);
    }
}
