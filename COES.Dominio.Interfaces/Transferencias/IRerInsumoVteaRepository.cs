using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RER_INSUMO_VTEA
    /// </summary>
    public interface IRerInsumoVteaRepository
    {
        int Save(RerInsumoVteaDTO entity);
        void Update(RerInsumoVteaDTO entity);
        void Delete(int rerInsumoVteaId);
        RerInsumoVteaDTO GetById(int rerInsumoVteaId);
        List<RerInsumoVteaDTO> List();
        List<RerInsumoVteaDTO> GetByPeriodo(int reravcodi, string rerpprmes);
        int GetMaxId();
        void BulkInsertRerInsumoVtea(List<RerInsumoVteaDTO> entitys);
        void DeleteByParametroPrimaAndMes(int iRerpprcodi);
        decimal ObtenerSaldoVteaByInsumoVTEA(int iRerpprcodi, int iEmprcodi, int iEquicodi);
    }
}
