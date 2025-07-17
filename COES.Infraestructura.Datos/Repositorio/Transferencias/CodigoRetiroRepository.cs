using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class CodigoRetiroRepository : RepositoryBase, ICodigoRetiroRepository
    {
        CodigoRetiroHelper helper = new CodigoRetiroHelper();
        private string strConexion;
        public CodigoRetiroRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }


        public IDbConnection BeginConnection()
        {
            Database db = DatabaseFactory.CreateDatabase(strConexion);
            IDbConnection conn = db.CreateConnection();
            conn.Open();
            return conn;
        }

        public DbTransaction StartTransaction(IDbConnection conn)
        {
            return (DbTransaction)conn.BeginTransaction();

        }
        public int Save(CodigoRetiroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiCodi, DbType.Int32, GetCodigoGenerado());
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, entity.BarrCodi);
            dbProvider.AddInParameter(command, helper.UsuaCodi, DbType.String, entity.UsuaCodi);
            dbProvider.AddInParameter(command, helper.TipoContCodi, DbType.Int32, entity.TipoContCodi);
            dbProvider.AddInParameter(command, helper.TipoUsuaCodi, DbType.Int32, entity.TipoUsuaCodi);
            dbProvider.AddInParameter(command, helper.CliCodi, DbType.Int32, entity.CliCodi);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiCodigo, DbType.String, entity.SoliCodiRetiCodigo);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaRegistro, DbType.DateTime, entity.SoliCodiRetiFechaRegistro);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiDescripcion, DbType.String, entity.SoliCodiRetiDescripcion);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiDetalleAmplio, DbType.String, entity.SoliCodiRetiDetalleAmplio);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaInicio, DbType.DateTime, entity.SoliCodiRetiFechaInicio);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaFin, DbType.DateTime, entity.SoliCodiRetiFechaFin);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiObservacion, DbType.String, entity.SoliCodiRetiObservacion);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, entity.SoliCodiRetiEstado);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFecIns, DbType.DateTime, DateTime.Now.Date);

            return dbProvider.ExecuteNonQuery(command);
        }

        public void Update(CodigoRetiroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, entity.BarrCodi);
            dbProvider.AddInParameter(command, helper.CoesUserName, DbType.String, entity.CoesUserName);
            dbProvider.AddInParameter(command, helper.TipoContCodi, DbType.Int32, entity.TipoContCodi);
            dbProvider.AddInParameter(command, helper.TipoUsuaCodi, DbType.Int32, entity.TipoUsuaCodi);
            dbProvider.AddInParameter(command, helper.CliCodi, DbType.Int32, entity.CliCodi);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiCodigo, DbType.String, entity.SoliCodiRetiCodigo);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaRegistro, DbType.DateTime, entity.SoliCodiRetiFechaRegistro);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiDescripcion, DbType.String, entity.SoliCodiRetiDescripcion);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiDetalleAmplio, DbType.String, entity.SoliCodiRetiDetalleAmplio);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaInicio, DbType.DateTime, entity.SoliCodiRetiFechaInicio);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaFin, DbType.DateTime, entity.SoliCodiRetiFechaFin);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiObservacion, DbType.String, entity.SoliCodiRetiObservacion);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, entity.SoliCodiRetiEstado);
            dbProvider.AddInParameter(command, helper.SolidCodiRetiFecAct, DbType.DateTime, DateTime.Now.Date);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaSolBaja, DbType.DateTime, entity.SoliCodiretiFechaSolBaja);
            dbProvider.AddInParameter(command, helper.SOLICODIRETIFECHADEBAJA, DbType.DateTime, entity.SoliCodiRetiFechaBaja);

            dbProvider.AddInParameter(command, helper.SoliCodiRetiCodi, DbType.Int32, entity.SoliCodiRetiCodi);

            dbProvider.ExecuteNonQuery(command);
        }
        public void UpdateEstadoAprobacion(CodigoRetiroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateEstado);

            dbProvider.AddInParameter(command, helper.SoliCodiRetiCodigo, DbType.String, entity.SoliCodiRetiCodigo);
            dbProvider.AddInParameter(command, helper.CoesUserName, DbType.String, entity.CoesUserName);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, entity.SoliCodiRetiEstado);
            dbProvider.AddInParameter(command, helper.SolidCodiRetiFecAct, DbType.DateTime, DateTime.Now.Date);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaInicio, DbType.DateTime, entity.SoliCodiRetiFechaInicio);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaFin, DbType.DateTime, entity.SoliCodiRetiFechaFin);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiCodi, DbType.Int32, entity.SoliCodiRetiCodi);
            dbProvider.ExecuteNonQuery(command);
        }
        public void UpdateEstadoRechazar(CodigoRetiroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateEstadoRechazar);

            dbProvider.AddInParameter(command, helper.SoliCodiRetiCodigo, DbType.String, entity.SoliCodiRetiCodigo);
            dbProvider.AddInParameter(command, helper.CoesUserName, DbType.String, entity.CoesUserName);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, entity.SoliCodiRetiEstado);
            dbProvider.AddInParameter(command, helper.SolidCodiRetiFecAct, DbType.DateTime, DateTime.Now.Date); 
            dbProvider.AddInParameter(command, helper.SoliCodiRetiCodi, DbType.Int32, entity.SoliCodiRetiCodi);
            dbProvider.ExecuteNonQuery(command);
        }
        public void UpdateBajaCodigoVTEA(CodigoRetiroDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateBajaCodigoVTEA);
            if (tran != null)
            {
                command.Transaction = tran;
                command.Connection = (DbConnection)conn;
            }
            dbProvider.AddInParameter(command, helper.SoliCodiRetiCodi, DbType.Int32, entity.SoliCodiRetiCodi);
            dbProvider.AddInParameter(command, helper.CoesUserName, DbType.String, entity.CoesUserName);
            dbProvider.AddInParameter(command, helper.SolidCodiRetiFecAct, DbType.DateTime, DateTime.Now.Date);
            dbProvider.ExecuteNonQuery(command);
        }
        public void UpdateBajaCodigoVTEAVTP(int iCoReSoCodi, string sCoesUserName)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateBajaCodigoVTEAVTP);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiCodi, DbType.Int32, iCoReSoCodi);
            dbProvider.AddInParameter(command, helper.CoesUserName, DbType.String, sCoesUserName);
            dbProvider.AddInParameter(command, helper.SolidCodiRetiFecAct, DbType.DateTime, DateTime.Now.Date);

            dbProvider.ExecuteNonQuery(command);
        }
        public void UpdateVariacion(CodigoRetiroDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateVariacion);
            if (tran != null)
            {
                command.Transaction = tran;
                command.Connection = (DbConnection)conn;
            }
            dbProvider.AddInParameter(command, helper.CoesUserName, DbType.String, entity.CoesUserName);
            dbProvider.AddInParameter(command, helper.CoresoVariacion, DbType.String, entity.Variacion);
            dbProvider.AddInParameter(command, helper.SolidCodiRetiFecAct, DbType.DateTime, DateTime.Now.Date);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiCodi, DbType.Int32, entity.SoliCodiRetiCodi);
            dbProvider.ExecuteNonQuery(command);
        }
        public void Delete(System.Int32 id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.SolidCodiRetiFecAct, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiCodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }

        public CodigoRetiroDTO GetById(System.Int32 id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.SoliCodiRetiCodi, DbType.Int32, id);
            CodigoRetiroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
        public CodigoRetiroDTO GetByIdGestionCodigosVTEAVTP(System.Int32 id, Int32 pericodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdGestionCodigosVTEAVTP);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiCodi, DbType.Int32, id);
            CodigoRetiroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
        public List<CodigoRetiroDTO> List(string estado)
        {
            List<CodigoRetiroDTO> entitys = new List<CodigoRetiroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, estado);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        public List<CodigoRetiroDTO> ListarGestionCodigosVTEAVTP(int? genEmprCodi, int? cliCodi, int? tipoCont, int? tipoUsu, int? barrCodi, int? barrCodiSum, string coresoEstado, string coresoestapr, string coregeCodVteaVtp, DateTime? fechaIni, DateTime? fechaFin, int periCodi, int nroPagina, int pageSize)
        {
            List<CodigoRetiroDTO> entitys = new List<CodigoRetiroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarGestionCodigosVTEAVTP);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, genEmprCodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, genEmprCodi);
            dbProvider.AddInParameter(command, helper.CliCodi, DbType.Int32, cliCodi);
            dbProvider.AddInParameter(command, helper.CliCodi, DbType.Int32, cliCodi);
            dbProvider.AddInParameter(command, helper.TipoContCodi, DbType.Int32, tipoCont);
            dbProvider.AddInParameter(command, helper.TipoContCodi, DbType.Int32, tipoCont);
            dbProvider.AddInParameter(command, helper.TipoUsuaCodi, DbType.Int32, tipoUsu);
            dbProvider.AddInParameter(command, helper.TipoUsuaCodi, DbType.Int32, tipoUsu);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barrCodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barrCodi);
            dbProvider.AddInParameter(command, helper.BARRCODISUM, DbType.Int32, barrCodiSum);
            dbProvider.AddInParameter(command, helper.BARRCODISUM, DbType.Int32, barrCodiSum);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, string.IsNullOrEmpty(coresoEstado) ? null : coresoEstado);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, string.IsNullOrEmpty(coresoEstado) ? null : coresoEstado);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, string.IsNullOrEmpty(coresoEstado) ? null : coresoEstado);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, string.IsNullOrEmpty(coresoEstado) ? null : coresoEstado);
            dbProvider.AddInParameter(command, helper.CoregeCodVTEAVTP, DbType.String, string.IsNullOrEmpty(coregeCodVteaVtp) ? null : coregeCodVteaVtp);
            dbProvider.AddInParameter(command, helper.CoregeCodVTEAVTP, DbType.String, string.IsNullOrEmpty(coregeCodVteaVtp) ? null : coregeCodVteaVtp);
            dbProvider.AddInParameter(command, helper.CoregeCodVTEAVTP, DbType.String, string.IsNullOrEmpty(coregeCodVteaVtp) ? null : coregeCodVteaVtp);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);

            dbProvider.AddInParameter(command, helper.NroPagina, DbType.Int32, nroPagina);
            dbProvider.AddInParameter(command, helper.PageSize, DbType.Int32, pageSize);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CodigoRetiroDTO entity = new CodigoRetiroDTO();

                    entity.SoliCodiRetiCodi = dr[helper.SoliCodiRetiCodi] == DBNull.Value ? 0 : Convert.ToInt32(dr[helper.SoliCodiRetiCodi].ToString());
                    entity.EmprNombre = dr[helper.EmprNomb] == DBNull.Value ? string.Empty : dr[helper.EmprNomb].ToString();
                    entity.CliRuc = dr[helper.CliRuc] == DBNull.Value ? string.Empty : dr[helper.CliRuc].ToString();
                    entity.CliNombre = dr[helper.CliNombre] == DBNull.Value ? string.Empty : dr[helper.CliNombre].ToString();
                    entity.TipoContNombre = dr[helper.TipoContNombre] == DBNull.Value ? string.Empty : dr[helper.TipoContNombre].ToString();
                    entity.TipoUsuaNombre = dr[helper.TipoUsuaNombre] == DBNull.Value ? string.Empty : dr[helper.TipoUsuaNombre].ToString();

                    entity.SoliCodiRetiFechaInicio = dr[helper.SoliCodiRetiFechaInicio] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr[helper.SoliCodiRetiFechaInicio].ToString());
                    entity.SoliCodiRetiFechaFin = dr[helper.SoliCodiRetiFechaFin] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr[helper.SoliCodiRetiFechaFin].ToString());
                    entity.EstDescripcion = dr[helper.EstdDescripcion] == DBNull.Value ? null : Convert.ToString(dr[helper.EstdDescripcion].ToString());
                    entity.EstAbrev = dr[helper.EstdAbrev] == DBNull.Value ? null : Convert.ToString(dr[helper.EstdAbrev].ToString());
                    entity.BarrNombBarrTran = dr[helper.BarrNomBarrTran] == DBNull.Value ? null : Convert.ToString(dr[helper.BarrNomBarrTran].ToString());

                    entity.SoliCodiRetiCodigo = dr[helper.SoliCodiRetiCodigo] == DBNull.Value ? null : Convert.ToString(dr[helper.SoliCodiRetiCodigo].ToString());
                    entity.Variacion = dr[helper.CoresoVariacion] == DBNull.Value ? 0 : (decimal?)Convert.ToDecimal(dr[helper.CoresoVariacion].ToString());
                    entity.CoregeCodi = dr[helper.CoregeCodi] == DBNull.Value ? 0 : Convert.ToInt32(dr[helper.CoregeCodi].ToString());
                    entity.BarrCodiSum = dr[helper.BARRCODISUM] == DBNull.Value ? 0 : Convert.ToInt32(dr[helper.BARRCODISUM].ToString());
                    entity.BarrNombBarrSum = dr[helper.BarrNombre] == DBNull.Value ? null : dr[helper.BarrNombre].ToString();
                    entity.CoregeCodVTP = dr[helper.CoregeCodVTP] == DBNull.Value ? null : dr[helper.CoregeCodVTP].ToString();
                    entity.EstDescripcionVTP = dr[helper.EstdDescripcionVTP] == DBNull.Value ? null : dr[helper.EstdDescripcionVTP].ToString();
                    entity.EstAbrevVTP = dr[helper.EstdAbrevVTP] == DBNull.Value ? null : dr[helper.EstdAbrevVTP].ToString();

                    //Potencia

                    entity.SoliCodiRetiCodigo = dr[helper.SoliCodiRetiCodigo] == DBNull.Value ? null : Convert.ToString(dr[helper.SoliCodiRetiCodigo].ToString());
                    entity.Variacion = dr[helper.CoresoVariacion] == DBNull.Value ? 0 : (decimal?)Convert.ToDecimal(dr[helper.CoresoVariacion].ToString());
                    entity.CoregeCodi = dr[helper.CoregeCodi] == DBNull.Value ? 0 : Convert.ToInt32(dr[helper.CoregeCodi].ToString());
                    entity.BarrCodiSum = dr[helper.BARRCODISUM] == DBNull.Value ? 0 : Convert.ToInt32(dr[helper.BARRCODISUM].ToString());
                    entity.BarrNombBarrSum = dr[helper.BarrNombre] == DBNull.Value ? null : dr[helper.BarrNombre].ToString();
                    entity.CoregeCodVTP = dr[helper.CoregeCodVTP] == DBNull.Value ? null : dr[helper.CoregeCodVTP].ToString();
                    entity.EstDescripcionVTP = dr[helper.EstdDescripcionVTP] == DBNull.Value ? null : dr[helper.EstdDescripcionVTP].ToString();
                    entity.EstAbrevVTP = dr[helper.EstdAbrevVTP] == DBNull.Value ? null : dr[helper.EstdAbrevVTP].ToString();


                    //potencia contrato
                    entity.TrnpcTipoPotencia = dr[helper.TrnpcTipoPotencia] == DBNull.Value ? null : (int?)Convert.ToInt32(dr[helper.TrnpcTipoPotencia].ToString());
                    entity.TrnpctCodi = dr[helper.TrnpctCodi] == DBNull.Value ? null : (int?)Convert.ToInt32(dr[helper.TrnpctCodi].ToString());
                    entity.CoresoCodiPotcn = dr[helper.CoresoCodiPotcn] == DBNull.Value ? null : (int?)Convert.ToInt32(dr[helper.CoresoCodiPotcn].ToString());
                    entity.CoregeCodiPotcn = dr[helper.CoregeCodiPotcn] == DBNull.Value ? null : (int?)Convert.ToInt32(dr[helper.CoregeCodiPotcn].ToString());
                    entity.TrnpCagrp = dr[helper.TrnpCagrp] == DBNull.Value ? null : (int?)Convert.ToInt32(dr[helper.TrnpCagrp].ToString());
                    entity.TrnpcNumordm = dr[helper.TrnpcNumordm] == DBNull.Value ? null : (int?)Convert.ToInt32(dr[helper.TrnpcNumordm].ToString());
                    entity.TrnpCcodiCas = dr[helper.TrnpCcodiCas] == DBNull.Value ? null : (int?)Convert.ToInt32(dr[helper.TrnpCcodiCas].ToString());
                    entity.TipCasaAbrev = dr[helper.TipCasAbrev] == DBNull.Value ? null : dr[helper.TipCasAbrev].ToString();

                    entity.TrnPctTotalmwFija = dr[helper.TrnPctTotalmwFija] == DBNull.Value ? null : (decimal?)Convert.ToDouble(dr[helper.TrnPctTotalmwFija].ToString());
                    entity.TrnPctHpmwFija = dr[helper.TrnPctHpmwFija] == DBNull.Value ? null : (decimal?)Convert.ToDouble(dr[helper.TrnPctHpmwFija].ToString());
                    entity.TrnPctHfpmwFija = dr[helper.TrnPctHfpmwFija] == DBNull.Value ? null : (decimal?)Convert.ToDouble(dr[helper.TrnPctHfpmwFija].ToString());
                    entity.TrnPctTotalmwVariable = dr[helper.TrnPctTotalmwVariable] == DBNull.Value ? null : (decimal?)Convert.ToDouble(dr[helper.TrnPctTotalmwVariable].ToString());
                    entity.TrnPctHpmwFijaVariable = dr[helper.TrnPctHpmwFijaVariable] == DBNull.Value ? null : (decimal?)Convert.ToDouble(dr[helper.TrnPctHpmwFijaVariable].ToString());
                    entity.TrnPctHfpmwFijaVariable = dr[helper.TrnPctHfpmwFijaVariable] == DBNull.Value ? null : (decimal?)Convert.ToDouble(dr[helper.TrnPctHfpmwFijaVariable].ToString());
                    
                    entity.TrnPctComeObs = dr[helper.TrnPctComeObs] == DBNull.Value ? null : dr[helper.TrnPctComeObs].ToString();
                    entity.TrnPctExcel = dr[helper.TrnPcExcel] == DBNull.Value ? 0 : Convert.ToInt32(dr[helper.TrnPcExcel].ToString());
                    //entity.TrnpcTipoPotencia = dr[helper.TrnpcTipoPotencia] == DBNull.Value ? 0 : (int?)Convert.ToInt32(dr[helper.TrnpcTipoPotencia].ToString());
                    //entity.TrnpcTipoCasoAgrupado = dr[helper.TrnpcTipoCasoAgrupado] == DBNull.Value ? null : dr[helper.TrnpcTipoCasoAgrupado].ToString();
                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<CodigoRetiroDTO> ListarGestionCodigosVTEAVTPAprobar(int? genEmprCodi, int? cliCodi, int? tipoCont, int? tipoUsu, int? barrCodi, int? barrCodiSum, string coresoEstado, string coresoestapr, string coregeCodVteaVtp, DateTime? fechaIni, DateTime? fechaFin, int periCodi, int nroPagina, int pageSize)
        {
            List<CodigoRetiroDTO> entitys = new List<CodigoRetiroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarGestionCodigosVTEAVTPAprobar);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            //dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, genEmprCodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, genEmprCodi);
            dbProvider.AddInParameter(command, helper.CliCodi, DbType.Int32, cliCodi);
            dbProvider.AddInParameter(command, helper.CliCodi, DbType.Int32, cliCodi);
            dbProvider.AddInParameter(command, helper.TipoContCodi, DbType.Int32, tipoCont);
            dbProvider.AddInParameter(command, helper.TipoContCodi, DbType.Int32, tipoCont);
            dbProvider.AddInParameter(command, helper.TipoUsuaCodi, DbType.Int32, tipoUsu);
            dbProvider.AddInParameter(command, helper.TipoUsuaCodi, DbType.Int32, tipoUsu);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barrCodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barrCodi);
            dbProvider.AddInParameter(command, helper.BARRCODISUM, DbType.Int32, barrCodiSum);
            dbProvider.AddInParameter(command, helper.BARRCODISUM, DbType.Int32, barrCodiSum);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, string.IsNullOrEmpty(coresoEstado) ? null : coresoEstado);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, string.IsNullOrEmpty(coresoEstado) ? null : coresoEstado);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, string.IsNullOrEmpty(coresoEstado) ? null : coresoEstado);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, string.IsNullOrEmpty(coresoEstado) ? null : coresoEstado);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstadoAprobar, DbType.String, string.IsNullOrEmpty(coresoestapr) ? null : coresoestapr);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstadoAprobar, DbType.String, string.IsNullOrEmpty(coresoestapr) ? null : coresoestapr);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            //dbProvider.AddInParameter(command, helper.SoliCodiRetiEstadoAprobar, DbType.String, string.IsNullOrEmpty(coresoestapr) ? null : coresoestapr);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.CoregeCodVTEAVTP, DbType.String, string.IsNullOrEmpty(coregeCodVteaVtp) ? null : coregeCodVteaVtp);
            dbProvider.AddInParameter(command, helper.CoregeCodVTEAVTP, DbType.String, string.IsNullOrEmpty(coregeCodVteaVtp) ? null : coregeCodVteaVtp);
            dbProvider.AddInParameter(command, helper.CoregeCodVTEAVTP, DbType.String, string.IsNullOrEmpty(coregeCodVteaVtp) ? null : coregeCodVteaVtp);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);

            dbProvider.AddInParameter(command, helper.NroPagina, DbType.Int32, nroPagina);
            dbProvider.AddInParameter(command, helper.PageSize, DbType.Int32, pageSize);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CodigoRetiroDTO entity = new CodigoRetiroDTO();

                    entity.SoliCodiRetiCodi = dr[helper.SoliCodiRetiCodi] == DBNull.Value ? 0 : Convert.ToInt32(dr[helper.SoliCodiRetiCodi].ToString());
                    entity.EmprNombre = dr[helper.EmprNomb] == DBNull.Value ? string.Empty : dr[helper.EmprNomb].ToString();
                    entity.CliRuc = dr[helper.CliRuc] == DBNull.Value ? string.Empty : dr[helper.CliRuc].ToString();
                    entity.CliNombre = dr[helper.CliNombre] == DBNull.Value ? string.Empty : dr[helper.CliNombre].ToString();
                    entity.TipoContNombre = dr[helper.TipoContNombre] == DBNull.Value ? string.Empty : dr[helper.TipoContNombre].ToString();
                    entity.TipoUsuaNombre = dr[helper.TipoUsuaNombre] == DBNull.Value ? string.Empty : dr[helper.TipoUsuaNombre].ToString();

                    entity.SoliCodiRetiFechaInicio = dr[helper.SoliCodiRetiFechaInicio] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr[helper.SoliCodiRetiFechaInicio].ToString());
                    entity.SoliCodiRetiFechaFin = dr[helper.SoliCodiRetiFechaFin] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr[helper.SoliCodiRetiFechaFin].ToString());
                    entity.EstDescripcion = dr[helper.EstdDescripcion] == DBNull.Value ? null : Convert.ToString(dr[helper.EstdDescripcion].ToString());
                    entity.EstAbrev = dr[helper.EstdAbrev] == DBNull.Value ? null : Convert.ToString(dr[helper.EstdAbrev].ToString());
                    entity.BarrNombBarrTran = dr[helper.BarrNomBarrTran] == DBNull.Value ? null : Convert.ToString(dr[helper.BarrNomBarrTran].ToString());

                    entity.SoliCodiRetiCodigo = dr[helper.SoliCodiRetiCodigo] == DBNull.Value ? null : Convert.ToString(dr[helper.SoliCodiRetiCodigo].ToString());
                    entity.Variacion = dr[helper.CoresoVariacion] == DBNull.Value ? 0 : (decimal?)Convert.ToDecimal(dr[helper.CoresoVariacion].ToString());
                    entity.CoregeCodi = dr[helper.CoregeCodi] == DBNull.Value ? 0 : Convert.ToInt32(dr[helper.CoregeCodi].ToString());
                    entity.BarrCodiSum = dr[helper.BARRCODISUM] == DBNull.Value ? 0 : Convert.ToInt32(dr[helper.BARRCODISUM].ToString());
                    entity.BarrNombBarrSum = dr[helper.BarrNombre] == DBNull.Value ? null : dr[helper.BarrNombre].ToString();
                    entity.CoregeCodVTP = dr[helper.CoregeCodVTP] == DBNull.Value ? null : dr[helper.CoregeCodVTP].ToString();
                    entity.EstDescripcionVTP = dr[helper.EstdDescripcionVTP] == DBNull.Value ? null : dr[helper.EstdDescripcionVTP].ToString();
                    entity.EstAbrevVTP = dr[helper.EstdAbrevVTP] == DBNull.Value ? null : dr[helper.EstdAbrevVTP].ToString();

                    //Potencia

                    entity.SoliCodiRetiCodigo = dr[helper.SoliCodiRetiCodigo] == DBNull.Value ? null : Convert.ToString(dr[helper.SoliCodiRetiCodigo].ToString());
                    entity.Variacion = dr[helper.CoresoVariacion] == DBNull.Value ? 0 : (decimal?)Convert.ToDecimal(dr[helper.CoresoVariacion].ToString());
                    entity.CoregeCodi = dr[helper.CoregeCodi] == DBNull.Value ? 0 : Convert.ToInt32(dr[helper.CoregeCodi].ToString());
                    entity.BarrCodiSum = dr[helper.BARRCODISUM] == DBNull.Value ? 0 : Convert.ToInt32(dr[helper.BARRCODISUM].ToString());
                    entity.BarrNombBarrSum = dr[helper.BarrNombre] == DBNull.Value ? null : dr[helper.BarrNombre].ToString();
                    entity.CoregeCodVTP = dr[helper.CoregeCodVTP] == DBNull.Value ? null : dr[helper.CoregeCodVTP].ToString();
                    entity.EstDescripcionVTP = dr[helper.EstdDescripcionVTP] == DBNull.Value ? null : dr[helper.EstdDescripcionVTP].ToString();
                    entity.EstAbrevVTP = dr[helper.EstdAbrevVTP] == DBNull.Value ? null : dr[helper.EstdAbrevVTP].ToString();


                    //potencia contrato
                    entity.TrnpcTipoPotencia = dr[helper.TrnpcTipoPotencia] == DBNull.Value ? null : (int?)Convert.ToInt32(dr[helper.TrnpcTipoPotencia].ToString());
                    entity.TrnpctCodi = dr[helper.TrnpctCodi] == DBNull.Value ? null : (int?)Convert.ToInt32(dr[helper.TrnpctCodi].ToString());
                    entity.CoresoCodiPotcn = dr[helper.CoresoCodiPotcn] == DBNull.Value ? null : (int?)Convert.ToInt32(dr[helper.CoresoCodiPotcn].ToString());
                    entity.CoregeCodiPotcn = dr[helper.CoregeCodiPotcn] == DBNull.Value ? null : (int?)Convert.ToInt32(dr[helper.CoregeCodiPotcn].ToString());
                    entity.TrnpCagrp = dr[helper.TrnpCagrp] == DBNull.Value ? null : (int?)Convert.ToInt32(dr[helper.TrnpCagrp].ToString());
                    entity.TrnpcNumordm = dr[helper.TrnpcNumordm] == DBNull.Value ? null : (int?)Convert.ToInt32(dr[helper.TrnpcNumordm].ToString());
                    entity.TrnpCcodiCas = dr[helper.TrnpCcodiCas] == DBNull.Value ? null : (int?)Convert.ToInt32(dr[helper.TrnpCcodiCas].ToString());
                    entity.TipCasaAbrev = dr[helper.TipCasAbrev] == DBNull.Value ? null : dr[helper.TipCasAbrev].ToString();

                    entity.TrnPctTotalmwFija = dr[helper.TrnPctTotalmwFija] == DBNull.Value ? null : (decimal?)Convert.ToDouble(dr[helper.TrnPctTotalmwFija].ToString());
                    entity.TrnPctHpmwFija = dr[helper.TrnPctHpmwFija] == DBNull.Value ? null : (decimal?)Convert.ToDouble(dr[helper.TrnPctHpmwFija].ToString());
                    entity.TrnPctHfpmwFija = dr[helper.TrnPctHfpmwFija] == DBNull.Value ? null : (decimal?)Convert.ToDouble(dr[helper.TrnPctHfpmwFija].ToString());
                    entity.TrnPctTotalmwVariable = dr[helper.TrnPctTotalmwVariable] == DBNull.Value ? null : (decimal?)Convert.ToDouble(dr[helper.TrnPctTotalmwVariable].ToString());
                    entity.TrnPctHpmwFijaVariable = dr[helper.TrnPctHpmwFijaVariable] == DBNull.Value ? null : (decimal?)Convert.ToDouble(dr[helper.TrnPctHpmwFijaVariable].ToString());
                    entity.TrnPctHfpmwFijaVariable = dr[helper.TrnPctHfpmwFijaVariable] == DBNull.Value ? null : (decimal?)Convert.ToDouble(dr[helper.TrnPctHfpmwFijaVariable].ToString());
                    /*if (string.IsNullOrEmpty(coresoestapr))
                    {
                        entity.TrnPctTotalmwFija = dr[helper.TrnPctTotalmwFija] == DBNull.Value ? null : (double?)Convert.ToDouble(dr[helper.TrnPctTotalmwFija].ToString());
                        entity.TrnPctHpmwFija = dr[helper.TrnPctHpmwFija] == DBNull.Value ? null : (double?)Convert.ToDouble(dr[helper.TrnPctHpmwFija].ToString());
                        entity.TrnPctHfpmwFija = dr[helper.TrnPctHfpmwFija] == DBNull.Value ? null : (double?)Convert.ToDouble(dr[helper.TrnPctHfpmwFija].ToString());
                        entity.TrnPctTotalmwVariable = dr[helper.TrnPctTotalmwVariable] == DBNull.Value ? null : (double?)Convert.ToDouble(dr[helper.TrnPctTotalmwVariable].ToString());
                        entity.TrnPctHpmwFijaVariable = dr[helper.TrnPctHpmwFijaVariable] == DBNull.Value ? null : (double?)Convert.ToDouble(dr[helper.TrnPctHpmwFijaVariable].ToString());
                        entity.TrnPctHfpmwFijaVariable = dr[helper.TrnPctHfpmwFijaVariable] == DBNull.Value ? null : (double?)Convert.ToDouble(dr[helper.TrnPctHfpmwFijaVariable].ToString());
                    }
                    else
                    {
                        entity.TrnPctTotalmwFija = dr[helper.TrnPctTotalmwFijaApr] == DBNull.Value ? null : (double?)Convert.ToDouble(dr[helper.TrnPctTotalmwFijaApr].ToString());
                        entity.TrnPctHpmwFija = dr[helper.TrnPctHpmwFijaApr] == DBNull.Value ? null : (double?)Convert.ToDouble(dr[helper.TrnPctHpmwFijaApr].ToString());
                        entity.TrnPctHfpmwFija = dr[helper.TrnPctHfpmwFijaApr] == DBNull.Value ? null : (double?)Convert.ToDouble(dr[helper.TrnPctHfpmwFijaApr].ToString());
                        entity.TrnPctTotalmwVariable = dr[helper.TrnPctTotalmwVariableApr] == DBNull.Value ? null : (double?)Convert.ToDouble(dr[helper.TrnPctTotalmwVariableApr].ToString());
                        entity.TrnPctHpmwFijaVariable = dr[helper.TrnPctHpmwFijaVariableApr] == DBNull.Value ? null : (double?)Convert.ToDouble(dr[helper.TrnPctHpmwFijaVariableApr].ToString());
                        entity.TrnPctHfpmwFijaVariable = dr[helper.TrnPctHfpmwFijaVariableApr] == DBNull.Value ? null : (double?)Convert.ToDouble(dr[helper.TrnPctHfpmwFijaVariableApr].ToString());
                    }*/


                    entity.TrnPctComeObs = dr[helper.TrnPctComeObs] == DBNull.Value ? null : dr[helper.TrnPctComeObs].ToString();
                    entity.TrnPctExcel = dr[helper.TrnPcExcel] == DBNull.Value ? 0 : Convert.ToInt32(dr[helper.TrnPcExcel].ToString());
                    //entity.TrnpcTipoPotencia = dr[helper.TrnpcTipoPotencia] == DBNull.Value ? 0 : (int?)Convert.ToInt32(dr[helper.TrnpcTipoPotencia].ToString());
                    //entity.TrnpcTipoCasoAgrupado = dr[helper.TrnpcTipoCasoAgrupado] == DBNull.Value ? null : dr[helper.TrnpcTipoCasoAgrupado].ToString();
                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<CodigoRetiroDTO> ListarGestionCodigosExportarVTEAVTP(int? genEmprCodi, int? cliCodi, int? tipoCont, int? tipoUsu, int? barrCodi, int? barrCodiSum, string coresoEstado, string coregeCodVteaVtp, DateTime? fechaIni, DateTime? fechaFin, int periCodi, int nroPagina, int pageSize)
        {
            List<CodigoRetiroDTO> entitys = new List<CodigoRetiroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarGestionCodigosExportarVTEAVTP);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, genEmprCodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, genEmprCodi);
            dbProvider.AddInParameter(command, helper.CliCodi, DbType.Int32, cliCodi);
            dbProvider.AddInParameter(command, helper.CliCodi, DbType.Int32, cliCodi);
            dbProvider.AddInParameter(command, helper.TipoContCodi, DbType.Int32, tipoCont);
            dbProvider.AddInParameter(command, helper.TipoContCodi, DbType.Int32, tipoCont);
            dbProvider.AddInParameter(command, helper.TipoUsuaCodi, DbType.Int32, tipoUsu);
            dbProvider.AddInParameter(command, helper.TipoUsuaCodi, DbType.Int32, tipoUsu);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barrCodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barrCodi);
            dbProvider.AddInParameter(command, helper.BARRCODISUM, DbType.Int32, barrCodiSum);
            dbProvider.AddInParameter(command, helper.BARRCODISUM, DbType.Int32, barrCodiSum);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, string.IsNullOrEmpty(coresoEstado) ? null : coresoEstado);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, string.IsNullOrEmpty(coresoEstado) ? null : coresoEstado);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, string.IsNullOrEmpty(coresoEstado) ? null : coresoEstado);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, string.IsNullOrEmpty(coresoEstado) ? null : coresoEstado);
            dbProvider.AddInParameter(command, helper.CoregeCodVTEAVTP, DbType.String, string.IsNullOrEmpty(coregeCodVteaVtp) ? null : coregeCodVteaVtp);
            dbProvider.AddInParameter(command, helper.CoregeCodVTEAVTP, DbType.String, string.IsNullOrEmpty(coregeCodVteaVtp) ? null : coregeCodVteaVtp);
            dbProvider.AddInParameter(command, helper.CoregeCodVTEAVTP, DbType.String, string.IsNullOrEmpty(coregeCodVteaVtp) ? null : coregeCodVteaVtp);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            
            dbProvider.AddInParameter(command, helper.NroPagina, DbType.Int32, nroPagina);
            dbProvider.AddInParameter(command, helper.PageSize, DbType.Int32, pageSize);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CodigoRetiroDTO entity = new CodigoRetiroDTO();

                    entity.SoliCodiRetiCodi = dr[helper.SoliCodiRetiCodi] == DBNull.Value ? 0 : Convert.ToInt32(dr[helper.SoliCodiRetiCodi].ToString());
                    entity.EmprNombre = dr[helper.EmprNomb] == DBNull.Value ? string.Empty : dr[helper.EmprNomb].ToString();
                    entity.CliRuc = dr[helper.CliRuc] == DBNull.Value ? string.Empty : dr[helper.CliRuc].ToString();
                    entity.CliNombre = dr[helper.CliNombre] == DBNull.Value ? string.Empty : dr[helper.CliNombre].ToString();
                    entity.TipoContNombre = dr[helper.TipoContNombre] == DBNull.Value ? string.Empty : dr[helper.TipoContNombre].ToString();
                    entity.TipoUsuaNombre = dr[helper.TipoUsuaNombre] == DBNull.Value ? string.Empty : dr[helper.TipoUsuaNombre].ToString();

                    entity.SoliCodiRetiFechaInicio = dr[helper.SoliCodiRetiFechaInicio] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr[helper.SoliCodiRetiFechaInicio].ToString());
                    entity.SoliCodiRetiFechaFin = dr[helper.SoliCodiRetiFechaFin] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr[helper.SoliCodiRetiFechaFin].ToString());
                    entity.EstDescripcion = dr[helper.EstdDescripcion] == DBNull.Value ? null : Convert.ToString(dr[helper.EstdDescripcion].ToString());
                    entity.EstAbrev = dr[helper.EstdAbrev] == DBNull.Value ? null : Convert.ToString(dr[helper.EstdAbrev].ToString());
                    entity.BarrNombBarrTran = dr[helper.BarrNomBarrTran] == DBNull.Value ? null : Convert.ToString(dr[helper.BarrNomBarrTran].ToString());

                    entity.SoliCodiRetiCodigo = dr[helper.SoliCodiRetiCodigo] == DBNull.Value ? null : Convert.ToString(dr[helper.SoliCodiRetiCodigo].ToString());
                    entity.Variacion = dr[helper.CoresoVariacion] == DBNull.Value ? 0 : (decimal?)Convert.ToDecimal(dr[helper.CoresoVariacion].ToString());
                    entity.CoregeCodi = dr[helper.CoregeCodi] == DBNull.Value ? 0 : Convert.ToInt32(dr[helper.CoregeCodi].ToString());
                    entity.BarrCodiSum = dr[helper.BARRCODISUM] == DBNull.Value ? 0 : Convert.ToInt32(dr[helper.BARRCODISUM].ToString());
                    entity.BarrNombBarrSum = dr[helper.BarrNombre] == DBNull.Value ? null : dr[helper.BarrNombre].ToString();
                    entity.CoregeCodVTP = dr[helper.CoregeCodVTP] == DBNull.Value ? null : dr[helper.CoregeCodVTP].ToString();
                    entity.EstDescripcionVTP = dr[helper.EstdDescripcionVTP] == DBNull.Value ? null : dr[helper.EstdDescripcionVTP].ToString();
                    entity.EstAbrevVTP = dr[helper.EstdAbrevVTP] == DBNull.Value ? null : dr[helper.EstdAbrevVTP].ToString();

                    //Potencia

                    entity.SoliCodiRetiCodigo = dr[helper.SoliCodiRetiCodigo] == DBNull.Value ? null : Convert.ToString(dr[helper.SoliCodiRetiCodigo].ToString());
                    entity.Variacion = dr[helper.CoresoVariacion] == DBNull.Value ? 0 : (decimal?)Convert.ToDecimal(dr[helper.CoresoVariacion].ToString());
                    entity.CoregeCodi = dr[helper.CoregeCodi] == DBNull.Value ? 0 : Convert.ToInt32(dr[helper.CoregeCodi].ToString());
                    entity.BarrCodiSum = dr[helper.BARRCODISUM] == DBNull.Value ? 0 : Convert.ToInt32(dr[helper.BARRCODISUM].ToString());
                    entity.BarrNombBarrSum = dr[helper.BarrNombre] == DBNull.Value ? null : dr[helper.BarrNombre].ToString();
                    entity.CoregeCodVTP = dr[helper.CoregeCodVTP] == DBNull.Value ? null : dr[helper.CoregeCodVTP].ToString();
                    entity.EstDescripcionVTP = dr[helper.EstdDescripcionVTP] == DBNull.Value ? null : dr[helper.EstdDescripcionVTP].ToString();
                    entity.EstAbrevVTP = dr[helper.EstdAbrevVTP] == DBNull.Value ? null : dr[helper.EstdAbrevVTP].ToString();


                    //potencia contrato
                    entity.TrnpcTipoPotencia = dr[helper.TrnpcTipoPotencia] == DBNull.Value ? null : (int?)Convert.ToInt32(dr[helper.TrnpcTipoPotencia].ToString());
                    entity.TrnpctCodi = dr[helper.TrnpctCodi] == DBNull.Value ? null : (int?)Convert.ToInt32(dr[helper.TrnpctCodi].ToString());
                    entity.CoresoCodiPotcn = dr[helper.CoresoCodiPotcn] == DBNull.Value ? null : (int?)Convert.ToInt32(dr[helper.CoresoCodiPotcn].ToString());
                    entity.CoregeCodiPotcn = dr[helper.CoregeCodiPotcn] == DBNull.Value ? null : (int?)Convert.ToInt32(dr[helper.CoregeCodiPotcn].ToString());
                    entity.TrnpCagrp = dr[helper.TrnpCagrp] == DBNull.Value ? null : (int?)Convert.ToInt32(dr[helper.TrnpCagrp].ToString());
                    entity.TrnpcNumordm = dr[helper.TrnpcNumordm] == DBNull.Value ? null : (int?)Convert.ToInt32(dr[helper.TrnpcNumordm].ToString());
                    entity.TrnpCcodiCas = dr[helper.TrnpCcodiCas] == DBNull.Value ? null : (int?)Convert.ToInt32(dr[helper.TrnpCcodiCas].ToString());
                    entity.TipCasaAbrev = dr[helper.TipCasAbrev] == DBNull.Value ? null : dr[helper.TipCasAbrev].ToString();
                    entity.TrnPctTotalmwFija = dr[helper.TrnPctTotalmwFija] == DBNull.Value ? null : (decimal?)Convert.ToDouble(dr[helper.TrnPctTotalmwFija].ToString());
                    entity.TrnPctHpmwFija = dr[helper.TrnPctHpmwFija] == DBNull.Value ? null : (decimal?)Convert.ToDouble(dr[helper.TrnPctHpmwFija].ToString());
                    entity.TrnPctHfpmwFija = dr[helper.TrnPctHfpmwFija] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(dr[helper.TrnPctHfpmwFija].ToString());
                    entity.TrnPctTotalmwVariable = dr[helper.TrnPctTotalmwVariable] == DBNull.Value ? null : (decimal?)Convert.ToDouble(dr[helper.TrnPctTotalmwVariable].ToString());
                    entity.TrnPctHpmwFijaVariable = dr[helper.TrnPctHpmwFijaVariable] == DBNull.Value ? null : (decimal?)Convert.ToDouble(dr[helper.TrnPctHpmwFijaVariable].ToString());
                    entity.TrnPctHfpmwFijaVariable = dr[helper.TrnPctHfpmwFijaVariable] == DBNull.Value ? null : (decimal?)Convert.ToDouble(dr[helper.TrnPctHfpmwFijaVariable].ToString());
                    entity.TrnPctComeObs = dr[helper.TrnPctComeObs] == DBNull.Value ? null : dr[helper.TrnPctComeObs].ToString();
                    entity.TrnPctExcel = dr[helper.TrnPcExcel] == DBNull.Value ? 0 : Convert.ToInt32(dr[helper.TrnPcExcel].ToString());
                    //entity.TrnpcTipoPotencia = dr[helper.TrnpcTipoPotencia] == DBNull.Value ? 0 : (int?)Convert.ToInt32(dr[helper.TrnpcTipoPotencia].ToString());
                    //entity.TrnpcTipoCasoAgrupado = dr[helper.TrnpcTipoCasoAgrupado] == DBNull.Value ? null : dr[helper.TrnpcTipoCasoAgrupado].ToString();
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CodigoRetiroDTO> ListarCodigoVTEAByEmprBarr(int? genemprcodi, int? cliemprcodi, int? barrcodi)
        {
            List<CodigoRetiroDTO> entitys = new List<CodigoRetiroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarCodigoVTEAByEmprBarr);

            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, genemprcodi);
            dbProvider.AddInParameter(command, helper.CliCodi, DbType.Int32, cliemprcodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barrcodi);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CodigoRetiroDTO> GetByCriteriaExtranet(string nombreEmp, string tipousu, string tipocont, string bartran, string clinomb, DateTime? fechaIni, DateTime? fechaFin, string Solicodiretiobservacion, string estado)
        {
            List<CodigoRetiroDTO> entitys = new List<CodigoRetiroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaExtranet);

            dbProvider.AddInParameter(command, helper.EmprNomb, DbType.String, nombreEmp);
            dbProvider.AddInParameter(command, helper.EmprNomb, DbType.String, nombreEmp);
            dbProvider.AddInParameter(command, helper.TipoUsuaNombre, DbType.String, tipousu);
            dbProvider.AddInParameter(command, helper.TipoUsuaNombre, DbType.String, tipousu);
            dbProvider.AddInParameter(command, helper.TipoContNombre, DbType.String, tipocont);
            dbProvider.AddInParameter(command, helper.TipoContNombre, DbType.String, tipocont);
            dbProvider.AddInParameter(command, helper.BarrNomBarrTran, DbType.String, bartran);
            dbProvider.AddInParameter(command, helper.BarrNomBarrTran, DbType.String, bartran);
            dbProvider.AddInParameter(command, helper.CliNombre, DbType.String, clinomb);
            dbProvider.AddInParameter(command, helper.CliNombre, DbType.String, clinomb);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaInicio, DbType.DateTime, fechaIni);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaInicio, DbType.DateTime, fechaIni);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaFin, DbType.DateTime, fechaFin);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaFin, DbType.DateTime, fechaFin);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiObservacion, DbType.String, Solicodiretiobservacion);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiObservacion, DbType.String, Solicodiretiobservacion);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, estado);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CodigoRetiroDTO> GetByCriteria(string nombreEmp, string tipousu, string tipocont, string bartran, string clinomb, DateTime? fechaIni, DateTime? fechaFin, string Solicodiretiobservacion, string estado, string codretiro, int NroPagina, int PageSize)
        {
            List<CodigoRetiroDTO> entitys = new List<CodigoRetiroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.EmprNomb, DbType.String, nombreEmp);
            dbProvider.AddInParameter(command, helper.EmprNomb, DbType.String, nombreEmp);
            dbProvider.AddInParameter(command, helper.TipoUsuaNombre, DbType.String, tipousu);
            dbProvider.AddInParameter(command, helper.TipoUsuaNombre, DbType.String, tipousu);
            dbProvider.AddInParameter(command, helper.TipoContNombre, DbType.String, tipocont);
            dbProvider.AddInParameter(command, helper.TipoContNombre, DbType.String, tipocont);
            dbProvider.AddInParameter(command, helper.BarrNomBarrTran, DbType.String, bartran);
            dbProvider.AddInParameter(command, helper.BarrNomBarrTran, DbType.String, bartran);
            dbProvider.AddInParameter(command, helper.CliNombre, DbType.String, clinomb);
            dbProvider.AddInParameter(command, helper.CliNombre, DbType.String, clinomb);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaInicio, DbType.DateTime, fechaIni);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaInicio, DbType.DateTime, fechaIni);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaFin, DbType.DateTime, fechaFin);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaFin, DbType.DateTime, fechaFin);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiObservacion, DbType.String, Solicodiretiobservacion);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiObservacion, DbType.String, Solicodiretiobservacion);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiCodigo, DbType.String, codretiro);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiCodigo, DbType.String, codretiro);
            dbProvider.AddInParameter(command, helper.NroPagina, DbType.Int32, NroPagina);
            dbProvider.AddInParameter(command, helper.PageSize, DbType.Int32, PageSize);
            dbProvider.AddInParameter(command, helper.NroPagina, DbType.Int32, NroPagina);
            dbProvider.AddInParameter(command, helper.PageSize, DbType.Int32, PageSize);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public int ObtenerNroRegistros(string nombreEmp, string tipousu, string tipocont, string bartran, string clinomb, DateTime? fechaIni, DateTime? fechaFin, string Solicodiretiobservacion, string estado, string codretiro)
        {
            int NroRegistros = 0;
            List<CodigoRetiroDTO> entitys = new List<CodigoRetiroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlTotalRecords);

            dbProvider.AddInParameter(command, helper.EmprNomb, DbType.String, nombreEmp);
            dbProvider.AddInParameter(command, helper.EmprNomb, DbType.String, nombreEmp);
            dbProvider.AddInParameter(command, helper.TipoUsuaNombre, DbType.String, tipousu);
            dbProvider.AddInParameter(command, helper.TipoUsuaNombre, DbType.String, tipousu);
            dbProvider.AddInParameter(command, helper.TipoContNombre, DbType.String, tipocont);
            dbProvider.AddInParameter(command, helper.TipoContNombre, DbType.String, tipocont);
            dbProvider.AddInParameter(command, helper.BarrNomBarrTran, DbType.String, bartran);
            dbProvider.AddInParameter(command, helper.BarrNomBarrTran, DbType.String, bartran);
            dbProvider.AddInParameter(command, helper.CliNombre, DbType.String, clinomb);
            dbProvider.AddInParameter(command, helper.CliNombre, DbType.String, clinomb);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaInicio, DbType.DateTime, fechaIni);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaInicio, DbType.DateTime, fechaIni);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaFin, DbType.DateTime, fechaFin);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiFechaFin, DbType.DateTime, fechaFin);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiObservacion, DbType.String, Solicodiretiobservacion);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiObservacion, DbType.String, Solicodiretiobservacion);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiCodigo, DbType.String, codretiro);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiCodigo, DbType.String, codretiro);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    NroRegistros = Convert.ToInt32(dr["NroRegistros"].ToString().Trim());
                }
            }

            return NroRegistros;
        }

        public int ObtenerNroRegistrosGestionCodigosVTEAVTP(int periCodi, int? genEmprCodi, int? cliCodi, int? tipoCont, int? tipoUsu, int? barrCodi, int? barrCodiSum, string coresoEstado, string coregeCodVteaVtp, DateTime? fechaIni, DateTime? fechaFin)
        {

            int NroRegistros = 0;
            List<CodigoRetiroDTO> entitys = new List<CodigoRetiroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlTotalRecordsGestionarCodigosVTEAVTP);


            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, genEmprCodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, genEmprCodi);
            dbProvider.AddInParameter(command, helper.CliCodi, DbType.Int32, cliCodi);
            dbProvider.AddInParameter(command, helper.CliCodi, DbType.Int32, cliCodi);
            dbProvider.AddInParameter(command, helper.TipoContCodi, DbType.Int32, tipoCont);
            dbProvider.AddInParameter(command, helper.TipoContCodi, DbType.Int32, tipoCont);
            dbProvider.AddInParameter(command, helper.TipoUsuaCodi, DbType.Int32, tipoUsu);
            dbProvider.AddInParameter(command, helper.TipoUsuaCodi, DbType.Int32, tipoUsu);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barrCodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barrCodi);
            dbProvider.AddInParameter(command, helper.BARRCODISUM, DbType.Int32, barrCodiSum);
            dbProvider.AddInParameter(command, helper.BARRCODISUM, DbType.Int32, barrCodiSum);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, string.IsNullOrEmpty(coresoEstado) ? null : coresoEstado);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, string.IsNullOrEmpty(coresoEstado) ? null : coresoEstado);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, string.IsNullOrEmpty(coresoEstado) ? null : coresoEstado);
            dbProvider.AddInParameter(command, helper.SoliCodiRetiEstado, DbType.String, string.IsNullOrEmpty(coresoEstado) ? null : coresoEstado);
            dbProvider.AddInParameter(command, helper.CoregeCodVTEAVTP, DbType.String, string.IsNullOrEmpty(coregeCodVteaVtp) ? null : coregeCodVteaVtp);
            dbProvider.AddInParameter(command, helper.CoregeCodVTEAVTP, DbType.String, string.IsNullOrEmpty(coregeCodVteaVtp) ? null : coregeCodVteaVtp);
            dbProvider.AddInParameter(command, helper.CoregeCodVTEAVTP, DbType.String, string.IsNullOrEmpty(coregeCodVteaVtp) ? null : coregeCodVteaVtp);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    NroRegistros = Convert.ToInt32(dr["NroRegistros"].ToString().Trim());
                }
            }
            return NroRegistros;

        }
        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public CodigoRetiroDTO GetByCodigoRetiCodigo(System.String sTRetCodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPorCodigoRetiroCodigo);

            dbProvider.AddInParameter(command, helper.TretCodigo, DbType.String, sTRetCodigo);

            CodigoRetiroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new CodigoRetiroDTO();

                    int iTRETTABLA = dr.GetOrdinal(this.helper.TretTabla);
                    if (!dr.IsDBNull(iTRETTABLA)) entity.TretTabla = dr.GetString(iTRETTABLA);

                    int iTRETCORESOCORESCCODI = dr.GetOrdinal(this.helper.TretCoresoCorescCodi);
                    if (!dr.IsDBNull(iTRETCORESOCORESCCODI)) entity.SoliCodiRetiCodi = dr.GetInt32(iTRETCORESOCORESCCODI);

                    int iTRETCODIGO = dr.GetOrdinal(this.helper.TretCodigo);
                    if (!dr.IsDBNull(iTRETCODIGO)) entity.SoliCodiRetiCodigo = dr.GetString(iTRETCODIGO);

                    int iBARRCODI = dr.GetOrdinal(this.helper.BarrCodi);
                    if (!dr.IsDBNull(iBARRCODI)) entity.BarrCodi = dr.GetInt32(iBARRCODI);

                    int iEMPRCODI = dr.GetOrdinal(this.helper.EmprCodi);
                    if (!dr.IsDBNull(iEMPRCODI)) entity.EmprCodi = dr.GetInt32(iEMPRCODI);

                    int iCLICODI = dr.GetOrdinal(this.helper.CliCodi);
                    if (!dr.IsDBNull(iCLICODI)) entity.CliCodi = dr.GetInt32(iCLICODI);

                }
            }

            return entity;
        }

        public CodigoRetiroDTO GetCodigoRetiroByCodigo(string sCoReSoCodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetCodigoRetiroByCodigo);

            dbProvider.AddInParameter(command, helper.SoliCodiRetiCodigo, DbType.String, sCoReSoCodigo);

            CodigoRetiroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new CodigoRetiroDTO();

                    int iSOLICODIRETICODI = dr.GetOrdinal(this.helper.SoliCodiRetiCodi);
                    if (!dr.IsDBNull(iSOLICODIRETICODI)) entity.SoliCodiRetiCodi = dr.GetInt32(iSOLICODIRETICODI);

                    int iEMPRCODI = dr.GetOrdinal(this.helper.EmprCodi);
                    if (!dr.IsDBNull(iEMPRCODI)) entity.EmprCodi = dr.GetInt32(iEMPRCODI);

                    int iBARRCODI = dr.GetOrdinal(this.helper.BarrCodi);
                    if (!dr.IsDBNull(iBARRCODI)) entity.BarrCodi = dr.GetInt32(iBARRCODI);

                    int iUSUACODI = dr.GetOrdinal(this.helper.UsuaCodi);
                    if (!dr.IsDBNull(iUSUACODI)) entity.UsuaCodi = dr.GetString(iUSUACODI);

                    int iTIPOCONTCODI = dr.GetOrdinal(this.helper.TipoContCodi);
                    if (!dr.IsDBNull(iTIPOCONTCODI)) entity.TipoContCodi = dr.GetInt32(iTIPOCONTCODI);

                    int iTIPOUSUACODI = dr.GetOrdinal(this.helper.TipoUsuaCodi);
                    if (!dr.IsDBNull(iTIPOUSUACODI)) entity.TipoUsuaCodi = dr.GetInt32(iTIPOUSUACODI);

                    int iCLICODI = dr.GetOrdinal(this.helper.CliCodi);
                    if (!dr.IsDBNull(iCLICODI)) entity.CliCodi = dr.GetInt32(iCLICODI);

                    int iSOLICODIRETICODIGO = dr.GetOrdinal(this.helper.SoliCodiRetiCodigo);
                    if (!dr.IsDBNull(iSOLICODIRETICODIGO)) entity.SoliCodiRetiCodigo = dr.GetString(iSOLICODIRETICODIGO);

                    int iSOLICODIRETIFECHAREGISTRO = dr.GetOrdinal(this.helper.SoliCodiRetiFechaRegistro);
                    if (!dr.IsDBNull(iSOLICODIRETIFECHAREGISTRO)) entity.SoliCodiRetiFechaRegistro = dr.GetDateTime(iSOLICODIRETIFECHAREGISTRO);

                    int iSOLICODIRETIDESCRIPCION = dr.GetOrdinal(this.helper.SoliCodiRetiDescripcion);
                    if (!dr.IsDBNull(iSOLICODIRETIDESCRIPCION)) entity.SoliCodiRetiDescripcion = dr.GetString(iSOLICODIRETIDESCRIPCION);

                    int iSOLICODIRETIDETALLEAMPLIO = dr.GetOrdinal(this.helper.SoliCodiRetiDetalleAmplio);
                    if (!dr.IsDBNull(iSOLICODIRETIDETALLEAMPLIO)) entity.SoliCodiRetiDetalleAmplio = dr.GetString(iSOLICODIRETIDETALLEAMPLIO);

                    int iSOLICODIRETIFECHAINICIO = dr.GetOrdinal(this.helper.SoliCodiRetiFechaInicio);
                    if (!dr.IsDBNull(iSOLICODIRETIFECHAINICIO)) entity.SoliCodiRetiFechaInicio = dr.GetDateTime(iSOLICODIRETIFECHAINICIO);

                    int iSOLICODIRETIFECHAFIN = dr.GetOrdinal(this.helper.SoliCodiRetiFechaFin);
                    if (!dr.IsDBNull(iSOLICODIRETIFECHAFIN)) entity.SoliCodiRetiFechaFin = dr.GetDateTime(iSOLICODIRETIFECHAFIN);

                    int iSOLICODIRETIOBSERVACION = dr.GetOrdinal(this.helper.SoliCodiRetiObservacion);
                    if (!dr.IsDBNull(iSOLICODIRETIOBSERVACION)) entity.SoliCodiRetiObservacion = dr.GetString(iSOLICODIRETIOBSERVACION);

                    int iSOLICODIRETIESTADO = dr.GetOrdinal(this.helper.SoliCodiRetiEstado);
                    if (!dr.IsDBNull(iSOLICODIRETIESTADO)) entity.SoliCodiRetiEstado = dr.GetString(iSOLICODIRETIESTADO);

                    int iCOESUSERNAME = dr.GetOrdinal(this.helper.CoesUserName);
                    if (!dr.IsDBNull(iCOESUSERNAME)) entity.CoesUserName = dr.GetString(iCOESUSERNAME);

                    int iSOLICODIRETIFECINS = dr.GetOrdinal(this.helper.SoliCodiRetiFecIns);
                    if (!dr.IsDBNull(iSOLICODIRETIFECINS)) entity.SoliCodiRetiFecIns = dr.GetDateTime(iSOLICODIRETIFECINS);

                    int iSOLICODIRETIFECACT = dr.GetOrdinal(this.helper.SolidCodiRetiFecAct);
                    if (!dr.IsDBNull(iSOLICODIRETIFECACT)) entity.SoliCodiRetiFecAct = dr.GetDateTime(iSOLICODIRETIFECACT);

                    int iSOLICODIRETIFECHASOLBAJA = dr.GetOrdinal(this.helper.SoliCodiRetiFechaSolBaja);
                    if (!dr.IsDBNull(iSOLICODIRETIFECHASOLBAJA)) entity.SoliCodiretiFechaSolBaja = dr.GetDateTime(iSOLICODIRETIFECHASOLBAJA);

                    int iSOLICODIRETIFECHADEBAJA = dr.GetOrdinal(this.helper.SOLICODIRETIFECHADEBAJA);
                    if (!dr.IsDBNull(iSOLICODIRETIFECHADEBAJA)) entity.SoliCodiRetiFechaBaja = dr.GetDateTime(iSOLICODIRETIFECHADEBAJA);

                }
            }
            return entity;
        }

        public CodigoRetiroDTO CodigoRetiroVigenteByPeriodo(int iPericodi, System.String sCodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoRetiroVigenteByPeriodo);
            dbProvider.AddInParameter(command, helper.TretCodigo, DbType.String, sCodigo);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, iPericodi);
            CodigoRetiroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new CodigoRetiroDTO();

                    int iTRETTABLA = dr.GetOrdinal(this.helper.TretTabla);
                    if (!dr.IsDBNull(iTRETTABLA)) entity.TretTabla = dr.GetString(iTRETTABLA);

                    int iTRETCORESOCORESCCODI = dr.GetOrdinal(this.helper.TretCoresoCorescCodi);
                    if (!dr.IsDBNull(iTRETCORESOCORESCCODI)) entity.SoliCodiRetiCodi = dr.GetInt32(iTRETCORESOCORESCCODI);

                    int iTRETCODIGO = dr.GetOrdinal(this.helper.TretCodigo);
                    if (!dr.IsDBNull(iTRETCODIGO)) entity.SoliCodiRetiCodigo = dr.GetString(iTRETCODIGO);

                    int iBARRCODI = dr.GetOrdinal(this.helper.BarrCodi);
                    if (!dr.IsDBNull(iBARRCODI)) entity.BarrCodi = dr.GetInt32(iBARRCODI);

                    int iEMPRCODI = dr.GetOrdinal(this.helper.EmprCodi);
                    if (!dr.IsDBNull(iEMPRCODI)) entity.EmprCodi = dr.GetInt32(iEMPRCODI);

                    int iCLICODI = dr.GetOrdinal(this.helper.CliCodi);
                    if (!dr.IsDBNull(iCLICODI)) entity.CliCodi = dr.GetInt32(iCLICODI);

                }
            }

            return entity;
        }

        // ASSETEC 2019-11
        public List<CodigoRetiroDTO> ImportarPotenciasContratadas(int pericodi, int idEmpresa)
        {
            List<CodigoRetiroDTO> entitys = new List<CodigoRetiroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlImportarCodigosRetiroSolicitud);

            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, idEmpresa);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, pericodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CodigoRetiroDTO entity = new CodigoRetiroDTO();

                    // CORESOCODI
                    int iSOLICODIRETICODI = dr.GetOrdinal(this.helper.SoliCodiRetiCodi);
                    if (!dr.IsDBNull(iSOLICODIRETICODI)) entity.SoliCodiRetiCodi = dr.GetInt32(iSOLICODIRETICODI);

                    // CORESOCODIGO
                    int iSOLICODIRETICODIGO = dr.GetOrdinal(this.helper.SoliCodiRetiCodigo);
                    if (!dr.IsDBNull(iSOLICODIRETICODIGO)) entity.SoliCodiRetiCodigo = dr.GetString(iSOLICODIRETICODIGO);

                    // GENEMPRCODI
                    int iEMPRCODI = dr.GetOrdinal(this.helper.EmprCodi);
                    if (!dr.IsDBNull(iEMPRCODI)) entity.EmprCodi = dr.GetInt32(iEMPRCODI);

                    // CLIEMPRCODI
                    int iCLICODI = dr.GetOrdinal(this.helper.CliCodi);
                    if (!dr.IsDBNull(iCLICODI)) entity.CliCodi = dr.GetInt32(iCLICODI);

                    // TIPCONCODI
                    int iTIPOCONTCODI = dr.GetOrdinal(this.helper.TipoContCodi);
                    if (!dr.IsDBNull(iTIPOCONTCODI)) entity.TipoContCodi = dr.GetInt32(iTIPOCONTCODI);

                    // TIPUSUCODI
                    int iTIPOUSUACODI = dr.GetOrdinal(this.helper.TipoUsuaCodi);
                    if (!dr.IsDBNull(iTIPOUSUACODI)) entity.TipoUsuaCodi = dr.GetInt32(iTIPOUSUACODI);

                    // BARRCODI
                    int iBARRCODI = dr.GetOrdinal(this.helper.BarrCodi);
                    if (!dr.IsDBNull(iBARRCODI)) entity.BarrCodi = dr.GetInt32(iBARRCODI);

                    // CORESOFECHAINICIO
                    int iSOLICODIRETIFECHAINICIO = dr.GetOrdinal(this.helper.SoliCodiRetiFechaInicio);
                    if (!dr.IsDBNull(iSOLICODIRETIFECHAINICIO)) entity.SoliCodiRetiFechaInicio = dr.GetDateTime(iSOLICODIRETIFECHAINICIO);

                    // CORESOFECHAFIN
                    int iSOLICODIRETIFECHAFIN = dr.GetOrdinal(this.helper.SoliCodiRetiFechaFin);
                    if (!dr.IsDBNull(iSOLICODIRETIFECHAFIN)) entity.SoliCodiRetiFechaFin = dr.GetDateTime(iSOLICODIRETIFECHAFIN);

                    // EMPRNOMB (EMPRNOMB)
                    int iEMPRNOMB = dr.GetOrdinal(this.helper.EmprNomb);
                    if (!dr.IsDBNull(iEMPRNOMB)) entity.EmprNombre = dr.GetString(iEMPRNOMB);

                    // CLINOMBRE
                    int iCLINOMBRE = dr.GetOrdinal(this.helper.CliNombre);
                    if (!dr.IsDBNull(iCLINOMBRE)) entity.CliNombre = dr.GetString(iCLINOMBRE);

                    // BARRBARRATRANSFERENCIA
                    int iBARRNOMBBARRTRAN = dr.GetOrdinal(this.helper.BarrNomBarrTran);
                    if (!dr.IsDBNull(iBARRNOMBBARRTRAN)) entity.BarrNombBarrTran = dr.GetString(iBARRNOMBBARRTRAN);

                    // TIPCONNOMBRE
                    int iTIPOCONTNOMBRE = dr.GetOrdinal(this.helper.TipoContNombre);
                    if (!dr.IsDBNull(iTIPOCONTNOMBRE)) entity.TipoContNombre = dr.GetString(iTIPOCONTNOMBRE);

                    // // TIPUSUNOMBRE
                    int iTIPOUSUANOMBRE = dr.GetOrdinal(this.helper.TipoUsuaNombre);
                    if (!dr.IsDBNull(iTIPOUSUANOMBRE)) entity.TipoUsuaNombre = dr.GetString(iTIPOUSUANOMBRE);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public bool ValidarExisteCodigoEnEnvios(string sTRetCodigo)
        {
            var result = false;            
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlValidarExisteCodigoEnEnvios);

            dbProvider.AddInParameter(command, helper.TretCodigo, DbType.String, sTRetCodigo);
            dbProvider.AddInParameter(command, helper.TretCodigo, DbType.String, sTRetCodigo);
            dbProvider.AddInParameter(command, helper.TretCodigo, DbType.String, sTRetCodigo);
            dbProvider.AddInParameter(command, helper.TretCodigo, DbType.String, sTRetCodigo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {                                     
                    int iSoliCodiRetiCodigo = dr.GetOrdinal(helper.SoliCodiRetiCodigo);
                    if (!dr.IsDBNull(iSoliCodiRetiCodigo))
                    {
                        result = true;
                        break;
                    }
                }
            }            

            return result;
        }


        #region PrimasRER.2023
        public List<CodigoRetiroDTO> ListCodRetirosByEmpresaYFecha(int genemprcodi, string coresofechainicio, string coresofechafin)
        {
            List<CodigoRetiroDTO> entitys = new List<CodigoRetiroDTO>();
            string query = string.Format(helper.SqlListCodRetirosByEmpresaYFecha, genemprcodi, coresofechainicio, coresofechafin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CodigoRetiroDTO entity = new CodigoRetiroDTO();

                    // CORESOCODI
                    int iSOLICODIRETICODI = dr.GetOrdinal(this.helper.SoliCodiRetiCodi);
                    if (!dr.IsDBNull(iSOLICODIRETICODI)) entity.SoliCodiRetiCodi = dr.GetInt32(iSOLICODIRETICODI);

                    // GENEMPRCODI
                    int iEMPRCODI = dr.GetOrdinal(this.helper.EmprCodi);
                    if (!dr.IsDBNull(iEMPRCODI)) entity.EmprCodi = dr.GetInt32(iEMPRCODI);

                    //BARRCODI
                    int iBARRCODI = dr.GetOrdinal(this.helper.BarrCodi);
                    if (!dr.IsDBNull(iBARRCODI)) entity.BarrCodi = dr.GetInt32(iBARRCODI);

                    // CORESOCODIGO
                    int iSOLICODIRETICODIGO = dr.GetOrdinal(this.helper.SoliCodiRetiCodigo);
                    if (!dr.IsDBNull(iSOLICODIRETICODIGO)) entity.SoliCodiRetiCodigo = dr.GetString(iSOLICODIRETICODIGO);

                    // CORESOFECHAINICIO
                    int iSOLICODIRETIFECHAINICIO = dr.GetOrdinal(this.helper.SoliCodiRetiFechaInicio);
                    if (!dr.IsDBNull(iSOLICODIRETIFECHAINICIO)) entity.SoliCodiRetiFechaInicio = dr.GetDateTime(iSOLICODIRETIFECHAINICIO);

                    // CORESOFECHAFIN
                    int iSOLICODIRETIFECHAFIN = dr.GetOrdinal(this.helper.SoliCodiRetiFechaFin);
                    if (!dr.IsDBNull(iSOLICODIRETIFECHAFIN)) entity.SoliCodiRetiFechaFin = dr.GetDateTime(iSOLICODIRETIFECHAFIN);

                    // BARRNOMBBARRTRAN
                    int iBARRNOMBBARRTRAN = dr.GetOrdinal(this.helper.BarrNomBarrTran);
                    if (!dr.IsDBNull(iBARRNOMBBARRTRAN)) entity.BarrNombBarrTran = dr.GetString(iBARRNOMBBARRTRAN);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion
    }
}
