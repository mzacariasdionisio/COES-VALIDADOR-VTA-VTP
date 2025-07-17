using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Scada
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TR_CANALCAMBIO_SP7
    /// </summary>
    public interface ITrCanalcambioSp7Repository
    {
        void Save(TrCanalcambioSp7DTO entity);
        void Update(TrCanalcambioSp7DTO entity);
        void Delete(int canalcodi, DateTime canalcmfeccreacion);
        TrCanalcambioSp7DTO GetById(int canalcodi, DateTime canalcmfeccreacion);
        List<TrCanalcambioSp7DTO> List();
        List<TrCanalcambioSp7DTO> GetByCriteria();
        List<TrCanalcambioSp7DTO> GetByFecha(DateTime fechaInicial, DateTime fechaFinal, int nroPage, int pageSize);
        int SaveTrCanalcambioSp7Id(TrCanalcambioSp7DTO entity);
        List<TrCanalcambioSp7DTO> BuscarOperaciones(DateTime canalCmfeccreacion,int nroPage, int pageSize);
        int ObtenerNroFilas(DateTime canalCmfeccreacion);
    }
}
