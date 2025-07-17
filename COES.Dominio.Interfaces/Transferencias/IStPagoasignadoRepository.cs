using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ST_PAGOASIGNADO
    /// </summary>
    public interface IStPagoasignadoRepository
    {
        int Save(StPagoasignadoDTO entity);
        void Update(StPagoasignadoDTO entity);
        void Delete(int strecacodi);
        StPagoasignadoDTO GetById(int facpagcodi);
        List<StPagoasignadoDTO> List(int strecacodi);
        StPagoasignadoDTO GetByCriteria(int Stcntgcodi, int Stcompcodi);
        List<StPagoasignadoDTO> GetByCriteriaReporte(int strecacodi);
        List<StPagoasignadoDTO> ListEmpresaGeneradores(int strecacodi);
        List<StPagoasignadoDTO> ListEmpresaSistemas(int strecacodi);
        decimal GetPagoGeneradorXSistema(int strecacodi, int genemprcodi, int sisemprecodi, int sistrncodi);
        void DeleteByCompensacion(int stcompcodi);
    }
}
