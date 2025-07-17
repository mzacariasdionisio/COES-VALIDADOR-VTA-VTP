using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_PROCESO_DIARIO
    /// </summary>
    public interface ICoProcesoDiarioRepository
    {
        int Save(CoProcesoDiarioDTO entity);
        void Update(CoProcesoDiarioDTO entity);
        void Delete(int prodiacodi);
        CoProcesoDiarioDTO GetById(int prodiacodi);
        List<CoProcesoDiarioDTO> List();
        List<CoProcesoDiarioDTO> GetByCriteria(string tipo, int copercodi, int covercodi, DateTime fechaInicio, DateTime fechaFin);
        List<CoProcesoDiarioDTO> Listar(int periodo, int version);
        List<CoProcesoDiarioDTO> ListarByPeriodo(int periodo);
        int Save(CoProcesoDiarioDTO entity, IDbConnection connection, IDbTransaction transaction);
        void EliminarProcesosDiarios(string strProdiacodis);
        List<CoProcesoDiarioDTO> ListarPorRangoFechas(string fechaIni, string fechaFin, string prodiatipo);
        CoProcesoDiarioDTO ObtenerProcesoDiario(DateTime fecha);
    }
}
