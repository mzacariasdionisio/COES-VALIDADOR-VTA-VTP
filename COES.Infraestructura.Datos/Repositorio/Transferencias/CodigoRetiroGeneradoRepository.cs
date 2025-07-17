using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{    /// <summary>
     /// Clase que contiene las operaciones con la base de datos
     /// </summary>
    public class CodigoRetiroGeneradoRepository : RepositoryBase, ICodigoRetiroGeneradoRepository
    {
        CodigoRetiroGeneradoHelper helper = new CodigoRetiroGeneradoHelper();
        public CodigoRetiroGeneradoRepository(string strConn) : base(strConn)
        {
        }
        public void UpdateEstado(CodigoRetiroGeneradoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateEstado);
            if (tran != null)
            {
                command.Transaction = tran;
                command.Connection = (DbConnection)conn;
            }
            dbProvider.AddInParameter(command, helper.CoregeEstado, DbType.String, entity.EstdAbrev);
            dbProvider.AddInParameter(command, helper.CoregeCodi, DbType.String, entity.CoregeCodi);
            //   dbProvider.AddInParameter(command, helper.CORESOCODI, DbType.Int32, entity.CoresoCodi);
            dbProvider.ExecuteNonQuery(command);

        }
        public CodigoRetiroGeneradoDTO GenerarAprobacion(CodigoRetiroGeneradoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            CodigoRetiroGeneradoDTO resultado = entity;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGenerarAprobacion);
            if (tran != null)
            {
                command.Transaction = tran;
                command.Connection = (DbConnection)conn;
            }
            dbProvider.AddInParameter(command, helper.CoregeEstado, DbType.String, entity.EstdAbrev);
            dbProvider.AddInParameter(command, helper.CoregeCodi, DbType.Int32, entity.CoregeCodi);
            dbProvider.AddInParameter(command, helper.BarrCodiTra, DbType.Int32, entity.BarrCodiTra);
            dbProvider.AddInParameter(command, helper.BarrCodiSum, DbType.Int32, entity.BarrCodiSum);
            dbProvider.AddInParameter(command, helper.CORESOCODI, DbType.Int32, entity.CoresoCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, entity.PeridcCodi);
            //dbProvider.AddInParameter(command, helper.PeriAnio, DbType.String, entity.PeridcAnio);
            //dbProvider.AddInParameter(command, helper.PeriMes, DbType.String, entity.PeridcMes);
            dbProvider.AddInParameter(command, helper.TrnPcTipoPotencia, DbType.Int32, entity.TrnPcTipoPotencia);
            dbProvider.AddInParameter(command, helper.GenemprCodi, DbType.String, entity.GenemprCodi);
            //   dbProvider.AddInParameter(command, helper.CORESOCODI, DbType.Int32, entity.CoresoCodi);
            dbProvider.AddOutParameter(command, "coregecodiOut", DbType.Int32, int.MaxValue);
            dbProvider.ExecuteNonQuery(command);
            resultado.CoregeCodi = dbProvider.GetParameterValue(command, "coregecodiOut") == DBNull.Value ? 0 : (int)dbProvider.GetParameterValue(command, "coregecodiOut");


            return resultado;
        }

        public void DesactivarSolicitudPeriodoActual(CodigoRetiroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDesactivarSolicitudPeriodoActual);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, entity.PeridcCodi);
            dbProvider.AddInParameter(command, helper.CORESOCODI, DbType.Int32, entity.SoliCodiRetiCodi);
            dbProvider.ExecuteNonQuery(command);
        }
        public void GenerarPotenciasPeriodosAbiertos(CodigoRetiroGeneradoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGenerarPotenciasPeriodosAbiertos);
            dbProvider.AddInParameter(command, helper.CORESOCODI, DbType.Int32, entity.CoresoCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, entity.PeridcCodi);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaInicio, DbType.DateTime, entity.SoliCodiRetiFechaInicio);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaFin, DbType.DateTime, entity.SoliCodiRetiFechaFin);
            //   dbProvider.AddInParameter(command, helper.CORESOCODI, DbType.Int32, entity.CoresoCodi);
            dbProvider.ExecuteNonQuery(command);

        }
        public void GenerarVTAPeriodosAbiertos(CodigoRetiroGeneradoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGenerarVTAPeriodosAbiertos);
            dbProvider.AddInParameter(command, helper.CORESOCODI, DbType.Int32, entity.CoresoCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, entity.PeridcCodi);
            dbProvider.AddInParameter(command, helper.CORESOCODI, DbType.Int32, entity.CoresoCodi);
            dbProvider.AddInParameter(command, helper.CoresoCodigo, DbType.String, entity.CoresoCodigoVTA);
            dbProvider.AddInParameter(command, helper.GenemprCodi, DbType.Int32, entity.GenemprCodi);
            dbProvider.AddInParameter(command, helper.TrnPcTipoPotencia, DbType.Int32, entity.TrnPcTipoPotencia);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, entity.PeridcCodi);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaInicio, DbType.DateTime, entity.SoliCodiRetiFechaInicio);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaFin, DbType.DateTime, entity.SoliCodiRetiFechaFin);
            //   dbProvider.AddInParameter(command, helper.CORESOCODI, DbType.Int32, entity.CoresoCodi);
            dbProvider.ExecuteNonQuery(command);

        }
        public void GenerarVTPPeriodosAbiertos(CodigoRetiroGeneradoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGenerarVTPPeriodosAbiertos);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, entity.PeridcCodi);
            dbProvider.AddInParameter(command, helper.CORESOCODI, DbType.Int32, entity.CoresoCodi);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaInicio, DbType.DateTime, entity.SoliCodiRetiFechaInicio);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaFin, DbType.DateTime, entity.SoliCodiRetiFechaFin);
            //   dbProvider.AddInParameter(command, helper.CORESOCODI, DbType.Int32, entity.CoresoCodi);
            dbProvider.ExecuteNonQuery(command);

        }

        public List<CodigoRetiroGeneradoDTO> ListarCodigosVTPByEmpBar(int barrcodisum, int genemprcodi, int cliemprcodi)
        {

            string query = helper.SqlListarCodigosVTPByEmpBar;
            List<CodigoRetiroGeneradoDTO> entitys = new List<CodigoRetiroGeneradoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.BarrCodiSum, DbType.Int32, barrcodisum);
            dbProvider.AddInParameter(command, helper.GenemprCodi, DbType.Int32, genemprcodi);
            dbProvider.AddInParameter(command, helper.CliemprCodi, DbType.Int32, cliemprcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CodigoRetiroGeneradoDTO> ListarCodigosGeneradoVTP(List<int> coresoCodi, int? barrCodiSum)
        {


            string query = helper.SqlListarCodigosGeneradoVTP;
            List<CodigoRetiroGeneradoDTO> entitys = new List<CodigoRetiroGeneradoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query.Replace("@coresocodiArray", string.Join(",", coresoCodi)));

            dbProvider.AddInParameter(command, helper.BarrCodiSum, DbType.Int32, barrCodiSum);
            dbProvider.AddInParameter(command, helper.BarrCodiSum, DbType.Int32, barrCodiSum);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CodigoRetiroGeneradoDTO> ListarCodigosGeneradoVTPExtranet()
        {

            string query = helper.SqlListarCodigosGeneradoVTPExtranet;
            List<CodigoRetiroGeneradoDTO> entitys = new List<CodigoRetiroGeneradoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public CodigoRetiroGeneradoDTO GetByCodigoVTP(string codigovtp)
        {

            string query = helper.SqlCodigoGeneradoVTPByCodivoVTP;
            CodigoRetiroGeneradoDTO entity = new CodigoRetiroGeneradoDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.CoregeCodVTP, DbType.String, codigovtp);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

    }
}
