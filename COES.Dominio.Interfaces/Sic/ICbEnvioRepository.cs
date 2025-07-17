using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CB_ENVIO
    /// </summary>
    public interface ICbEnvioRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int Save(CbEnvioDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(CbEnvioDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(int cbenvcodi);
        CbEnvioDTO GetById(int cbenvcodi);
        List<CbEnvioDTO> ListXFiltroPaginado(string emprcodi, string equicodis, int estenvcodi, DateTime fechaInicio, DateTime fechaFin, int nroPaginas, int pageSize);
        int ObtenerTotalXFiltro(string emprcodi, string equicodis, int estenvcodi, DateTime fechaInicio, DateTime fechaFin, string tipoCombustible, string omitirTipoCarga);
        List<CbEnvioDTO> GetByCriteria(string emprcodi, string equicodis, DateTime fechaInicio, DateTime fechaFin);
        List<CbEnvioDTO> ListXEstado(string estenvcodis, string equicodis, int fenergcodi);
        List<CbEnvioDTO> ObtenerEnvios(string emprcodi, int estenvcodi, DateTime fechaInicio, DateTime fechaFin, string tipoCombustible, int tipoEnvio);
        CbEnvioDTO GetByTipoCombustibleYVigenciaYTipocentral(int emprcodi, int estcomcodi, DateTime fechaVigencia, string tipoCentral, int estenvcodi);
        List<CbEnvioDTO> ObtenerAutoguardados(string tipoCentral, string mesDeVigencia, int idEmpresa, int estenvcodi, int enviotipo);
        void CambiarEstadoEnvio(string enviocodis, string estado);
        List<CbEnvioDTO> ObtenerInformacionEnviosReporteCumplimiento(string strEnvioscodi);
        List<CbEnvioDTO> ObtenerEnviosXPeriodo(string emprcodi, int estenvcodi, DateTime fechaInicio, DateTime fechaFin, string tipoCombustible, int tipoEnvio);
    }
}
