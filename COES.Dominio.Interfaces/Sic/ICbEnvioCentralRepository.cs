using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CB_ENVIO_CENTRAL
    /// </summary>
    public interface ICbEnvioCentralRepository
    {
        int GetMaxId();
        int Save(CbEnvioCentralDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(CbEnvioCentralDTO entity);
        void Delete(int cbcentcodi);
        CbEnvioCentralDTO GetById(int cbcentcodi);
        List<CbEnvioCentralDTO> List();
        List<CbEnvioCentralDTO> ListarPorIds(string cbcentcodis);
        List<CbEnvioCentralDTO> GetByCriteria(int cbvercodi);
        List<CbEnvioCentralDTO> ObtenerCentrales(int cbenvcodi);
        List<CbEnvioCentralDTO> GetCentralesConInfoEnviada(int mes, int anio);
        List<CbEnvioCentralDTO> ListarCentralUltimoEnvioXMes(int mes, int anio);
        List<CbEnvioCentralDTO> ListarCentralUltimoEnvioXDato(string equicodis, DateTime fechaPeriodo, int ccombcodi, string valor);
        List<CbEnvioCentralDTO> ListarCentralNuevaUltimoEnvioXDato(string equicodis, DateTime fechaPeriodo, int ccombcodi);
        List<CbEnvioCentralDTO> ListarCentralUltimoEnvioXDia(DateTime fechaPeriodo, string centrales);
        List<CbEnvioCentralDTO> GetByEstadoYVersion(string estado, string versioncodis);
        List<CbEnvioCentralDTO> ListarCentralXRangoMes(DateTime fechaIniPeriodo, DateTime fechaFinPeriodo, string centrales);
    }
}
