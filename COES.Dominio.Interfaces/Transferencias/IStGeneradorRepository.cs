using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ST_GENERADOR
    /// </summary>
    public interface IStGeneradorRepository
    {
        int Save(StGeneradorDTO entity);
        void Update(StGeneradorDTO entity);
        void Delete(int stgenrcodi);
        void DeleteVersion(int strecacodi);
        StGeneradorDTO GetById(int stgenrcodi);
        List<StGeneradorDTO> List(int strecacodi);
        List<StGeneradorDTO> ListByStGeneradorVersion(int strecacodi);
        List<StGeneradorDTO> GetByCriteria();
        List<StGeneradorDTO> ListByStGeneradorReporte(int strecacodi);
    }
}
