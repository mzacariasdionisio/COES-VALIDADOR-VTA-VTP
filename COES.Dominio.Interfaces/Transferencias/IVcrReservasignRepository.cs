using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_RESERVASIGN
    /// </summary>
    public interface IVcrReservasignRepository
    {
        int Save(VcrReservasignDTO entity);
        void Update(VcrReservasignDTO entity);
        void Delete(int vcrecacodi);
        VcrReservasignDTO GetById(int vcrasgcodi);
        List<VcrReservasignDTO> List();
        List<VcrReservasignDTO> GetByCriteria();
        List<VcrReservasignDTO> GetByCriteriaURSDia(int vcrecacodi, int grupocodi, DateTime dFecha);
        List<VcrReservasignDTO> GetByCriteriaDia(int vcrecacodi, DateTime dFecha);
    }
}
