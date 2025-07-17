using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_UNIDADEXONERADA
    /// </summary>
    public interface IVcrUnidadexoneradaRepository
    {
        int Save(VcrUnidadexoneradaDTO entity);
        void Update(VcrUnidadexoneradaDTO entity);
        void Delete(int vcrecacodi);
        VcrUnidadexoneradaDTO GetById(int vcruexcodi);
        List<VcrUnidadexoneradaDTO> List();
        List<VcrUnidadexoneradaDTO> GetByCriteria();
        List<VcrUnidadexoneradaDTO> ListParametro(int vcrecacodi);
        VcrUnidadexoneradaDTO GetByIdView(int vcruexcodi);
        void UpdateVersionNO(int vcrecacodi, string sUser);
        void UpdateVersionSI(int vcrecacodi, string sUser, int vcruexcodi);
    }
}
