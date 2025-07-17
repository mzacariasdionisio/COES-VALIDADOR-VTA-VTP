using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ENVIO
    /// </summary>
    public interface IFtExtEnvioRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int Save(FtExtEnvioDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(FtExtEnvioDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(int ftenvcodi);
        FtExtEnvioDTO GetById(int ftenvcodi);
        List<FtExtEnvioDTO> List();
        List<FtExtEnvioDTO> GetByCriteria();
        int ObtenerTotalXFiltro(string emprcodi, int estenvcodi, DateTime fechaInicio, DateTime fechaFin, int ftetcodi);
        List<FtExtEnvioDTO> ObtenerEnviosEtapas(string emprcodi, int estenvcodi, DateTime fechaInicio, DateTime fechaFin, string ftetcodi);
        List<FtExtEnvioDTO> ObtenerEnviosPorEstado(string emprcodi, int estenvcodi, int ftetcodi);
        List<FtExtEnvioDTO> ListarEnvioAutoguardado(int emprcodi, int estenvcodi, int ftetcodi, int ftenvtipoenvio);
        List<FtExtEnvioDTO> ListarEnviosYEqNoSeleccionable(string emprcodis, int ftetcodi);
        List<FtExtEnvioDTO> ListarEnviosYEqAprobado(string emprcodis, int ftetcodi);
        List<FtExtEnvioDTO> ObtenerEnviosEtapasParaAreas(string emprcodi, string ftetcodi, DateTime fechaInicio, DateTime fechaFin, string envarestado, string faremcodis);
        List<FtExtEnvioDTO> ListarEnviosDerivadosPorCarpetaYEstado(int estenvcodi, int ftetcodi, string estadoRevision);
        List<FtExtEnvioDTO> ListarRelacionEnvioVersionArea(int estenvcodi, int ftetcodi, int ftevertipo);
    }
}
