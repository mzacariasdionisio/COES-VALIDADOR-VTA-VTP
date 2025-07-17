using System;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data; // STS: Conexion para DB

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    // ASSETEC 2019-11
    /// <summary>
    /// Clase que contiene las operaciones con la tabla TRN_PONTENCIA_CONTRATADA
    /// </summary>
    public class TrnPotenciaContratadaRepository : RepositoryBase, ITrnPotenciaContratadaRepository
    {
        private string strConexion;
        public TrnPotenciaContratadaRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }

        TrnPotenciaContratadaHelper helper = new TrnPotenciaContratadaHelper();

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

        #region Metodos Basicos
        /// <summary>
        /// Función que Crea registro de Potnecias contratadas una por una
        /// </summary>
        /// <param name="entitys">Objeto de tipo entidad de TrnPotenciaContratadaDTO</param>        
        /// <param name="conn">Objeto de tipo IDbConnection</param>
        /// <param name="tran">Objeto de tipo DbTransaction</param>
        /// <returns>Entero</returns>
        public int Save(TrnPotenciaContratadaDTO entity, string usuario, IDbConnection conn, DbTransaction tran, int correlativo)
        {
            int id = -1;

            #region Auto Genera Correlativo
            DbCommand commandMaxId = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            if (correlativo != -1)
            {
                id = correlativo + 1;
            }
            else
            {
                object result = dbProvider.ExecuteScalar(commandMaxId);
                if (result != null)
                {
                    id = Convert.ToInt32(result);
                }
            }
            #endregion

            #region Crear y Llenar Parametros
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.TrnPctCodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.PeriCodi, DbType.Int32, entity.PeriCodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.EmprCodi, DbType.Int32, entity.EmprCodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.CoresoCodi, DbType.Int32, entity.CoresoCodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.TrnPctPtoSumins, DbType.String, entity.TrnPctPtoSumins));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.TrnPctTotalMwFija, DbType.Decimal, entity.TrnPctTotalMwFija));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.TrnPctHpMwFija, DbType.Decimal, entity.TrnPctHpMwFija));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.TrnPctHfpMwFija, DbType.Decimal, entity.TrnPctHfpMwFija));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.TrnPctTotalMwVariable, DbType.Decimal, entity.TrnPctTotalMwVariable));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.TrnPctHpMwFijaVariable, DbType.Decimal, entity.TrnPctHpMwFijaVariable));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.TrnPctHfpMwFijaVariable, DbType.Decimal, entity.TrnPctHfpMwFijaVariable));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.TrnPctComeObs, DbType.String, entity.TrnPctComeObs));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.TrnPctUserNameIns, DbType.String, usuario));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.TrnPctFecIns, DbType.DateTime, DateTime.Now));
            #endregion

            #region Ejecuta Comando
            command.ExecuteNonQuery();
            #endregion

            return id;
        }

        public void Delete(int pericodi, int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, emprcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<TrnPotenciaContratadaDTO> GetByCriteria(int idEmpresa, int idPeriodo, int idCliente, int idBarra)
        {
            List<TrnPotenciaContratadaDTO> entitys = new List<TrnPotenciaContratadaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, idPeriodo);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, idEmpresa);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, idEmpresa);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, idCliente);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, idCliente);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, idBarra);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, idBarra);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, idPeriodo);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, idPeriodo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreatePotenciasContratadas(dr));
                }
            }

            return entitys;
        }

        //dasaji-210212
        public List<TrnPotenciaContratadaDTO> GetEnvios(int idPeriodo)
        {
            List<TrnPotenciaContratadaDTO> entitys = new List<TrnPotenciaContratadaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListarEnvios);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, idPeriodo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreatePotenciasContratadasEnvio(dr));
                }
            }
            return entitys;
        }
        public List<TrnPotenciaContratadaDTO> GetPotenciasContratadas(int coresoCodi, int periCodi)
        {
            List<TrnPotenciaContratadaDTO> entitys = new List<TrnPotenciaContratadaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListarPotencias);
            dbProvider.AddInParameter(command, helper.CoresoCodi, DbType.Int32, coresoCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreatePotenciasContratosLista(dr));
                }
            }
            return entitys;
        }

        public List<TrnPotenciaContratadaDTO> GetPotenciasContratadasAprobar(int coresoCodi, int periCodi)
        {
            List<TrnPotenciaContratadaDTO> entitys = new List<TrnPotenciaContratadaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListarPotenciasAprobar);
            dbProvider.AddInParameter(command, helper.CoresoCodi, DbType.Int32, coresoCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreatePotenciasContratosLista(dr));
                }
            }
            return entitys;
        }
        public List<TrnPotenciaContratadaDTO> GetListaGrupoAsociadoVTA(List<int> coresoCodi, int pericodi)
        {
            List<TrnPotenciaContratadaDTO> entitys = new List<TrnPotenciaContratadaDTO>();
            string query = string.Format(helper.SqlListaGrupoAsociadoVTA, string.Join(",", coresoCodi), pericodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrnPotenciaContratadaDTO entidad = new TrnPotenciaContratadaDTO();
                    entidad.CoresoCodi = Convert.ToInt32(dr["CORESOCODI"]);
                    entidad.CoresoCodiFirst = Convert.ToInt32(dr["CORESOCODIPARENT"]);
                    entitys.Add(entidad);
                }
            }
            return entitys;
        }
        public int? GenerarCodigosAgrupadosReservados(string userName)
        {
            int? CodigoAgrupado = null;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGenerarCodigosAgrupadosReservados);
            dbProvider.AddInParameter(command, helper.TrnPctUserNameIns, DbType.String, userName);
            dbProvider.AddOutParameter(command, helper.CodigoAgrupado, DbType.Int32, Int32.MaxValue);
            dbProvider.ExecuteNonQuery(command);
            CodigoAgrupado = (int?)dbProvider.GetParameterValue(command, helper.CodigoAgrupado);
            return CodigoAgrupado;
        }
        public int? GenerarPotenciasAgrupadas(TrnPotenciaContratadaDTO entity)
        {
            int? CodigoAgrupado = null;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGenerarPotenciasAgrupadas);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, entity.CoresoCodi);
            dbProvider.AddInParameter(command, helper.CoregeCodi, DbType.Int32, entity.CoregeCodi);
            dbProvider.AddInParameter(command, helper.CoresoCodiFirst, DbType.Int32, entity.CoresoCodiFirst);
            dbProvider.AddInParameter(command, helper.CoregeCodiFirst, DbType.Int32, entity.CoregeCodiFirst);
            dbProvider.AddInParameter(command, helper.TrnpcNumord, DbType.Int32, entity.TrnpcNumOrd);
            dbProvider.AddInParameter(command, helper.TrnpCagrp, DbType.Int32, entity.TrnpCagrp);
            dbProvider.AddInParameter(command, helper.TrnnpcCodicas, DbType.String, entity.TrnpcTipoCasoAgrupado);
            dbProvider.AddOutParameter(command, helper.CodigoAgrupado, DbType.Int32, Int32.MaxValue);
            dbProvider.ExecuteNonQuery(command);
            CodigoAgrupado = (int?)dbProvider.GetParameterValue(command, helper.CodigoAgrupado);
            return CodigoAgrupado;

        }
        public void DesagruparPotencias(TrnPotenciaContratadaDTO entity, int omitirExcel = 0)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDesagruparPotencias);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.CoresoCodi, DbType.Int32, entity.CoresoCodi);
            dbProvider.AddInParameter(command, helper.CoregeCodi, DbType.Int32, entity.CoregeCodi);
            dbProvider.AddInParameter(command, helper.TrnnpcCodicas, DbType.String, entity.TrnpcTipoCasoAgrupado);
            dbProvider.AddInParameter(command, helper.OmitirExcel, DbType.Int32, omitirExcel);
            dbProvider.ExecuteNonQuery(command);

        }

        public void DesactivarPotenciasContratadas(int coresoCodi, int pericodi)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDesactivarPotencias);
            dbProvider.AddInParameter(command, helper.CoresoCodi, DbType.Int32, coresoCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DesactivarPotenciasPorBarrSum(int coresoCodi, int coregeCodi, int pericodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDesactivarPotenciasPorBarrSum);
            dbProvider.AddInParameter(command, helper.CoresoCodi, DbType.Int32, coresoCodi);
            dbProvider.AddInParameter(command, helper.CoregeCodi, DbType.Int32, coregeCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public int? GenerarCargaDatosExcelEnvio(TrnPotenciaContratadaDTO entity)
        {
            int? resultado = null;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SaveRegistrosExcelEnvio);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.TrnPctUsernameIns, DbType.String, entity.TrnPctUserNameIns);
            dbProvider.AddOutParameter(command, helper.TrnPcEnvCodi, DbType.Int32, Int32.MaxValue);
            dbProvider.ExecuteNonQuery(command);
            resultado = (int?)dbProvider.GetParameterValue(command, helper.TrnPcEnvCodi);
            return resultado;
        }
        public void GenerarCargaDatosExcel(TrnPotenciaContratadaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveRegistrosExcel);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.CoresoCodi, DbType.Int32, entity.CoresoCodi);
            dbProvider.AddInParameter(command, helper.CoregeCodi, DbType.Int32, entity.CoregeCodi);
            dbProvider.AddInParameter(command, helper.TrnnpcCodicas, DbType.String, entity.TrnpcTipoCasoAgrupado);
            dbProvider.AddInParameter(command, helper.TrnPctTotalMWFija, DbType.Decimal, entity.TrnPctTotalMwFija);
            dbProvider.AddInParameter(command, helper.TrnPctHPMWFija, DbType.Decimal, entity.TrnPctHpMwFija);
            dbProvider.AddInParameter(command, helper.TrnPctHFPMWFija, DbType.Decimal, entity.TrnPctHfpMwFija);
            dbProvider.AddInParameter(command, helper.TrnPctTotalMWVariable, DbType.Decimal, entity.TrnPctTotalMwVariable);
            dbProvider.AddInParameter(command, helper.TrnPctHPMWFijaVariable, DbType.Decimal, entity.TrnPctHpMwFijaVariable);
            dbProvider.AddInParameter(command, helper.TrnPctHFPMWFijaVariable, DbType.Decimal, entity.TrnPctHfpMwFijaVariable);
            dbProvider.AddInParameter(command, helper.TrnPctComeObs, DbType.String, entity.TrnPctComeObs);
            dbProvider.AddInParameter(command, helper.TrnPctUsernameIns, DbType.String, entity.TrnPctUserNameIns);
            dbProvider.AddInParameter(command, helper.TrnPcEnvCodi, DbType.Int32, entity.TrnPcEnvCodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void GenerarCargaDatos(TrnPotenciaContratadaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveRegistrosPotencia);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.CoresoCodi, DbType.Int32, entity.CoresoCodi);
            dbProvider.AddInParameter(command, helper.CoregeCodi, DbType.Int32, entity.CoregeCodi);
            dbProvider.AddInParameter(command, helper.TrnnpcCodicas, DbType.String, entity.TrnpcTipoCasoAgrupado);
            dbProvider.AddInParameter(command, helper.TrnPctTotalMWFija, DbType.Decimal, entity.TrnPctTotalMwFija);
            dbProvider.AddInParameter(command, helper.TrnPctHPMWFija, DbType.Decimal, entity.TrnPctHpMwFija);
            dbProvider.AddInParameter(command, helper.TrnPctHFPMWFija, DbType.Decimal, entity.TrnPctHfpMwFija);
            dbProvider.AddInParameter(command, helper.TrnPctTotalMWVariable, DbType.Decimal, entity.TrnPctTotalMwVariable);
            dbProvider.AddInParameter(command, helper.TrnPctHPMWFijaVariable, DbType.Decimal, entity.TrnPctHpMwFijaVariable);
            dbProvider.AddInParameter(command, helper.TrnPctHFPMWFijaVariable, DbType.Decimal, entity.TrnPctHfpMwFijaVariable);
            dbProvider.AddInParameter(command, helper.TrnPctComeObs, DbType.String, entity.TrnPctComeObs);
            dbProvider.AddInParameter(command, helper.TrnPctUsernameIns, DbType.String, entity.TrnPctUserNameIns);
            dbProvider.AddInParameter(command, helper.TrnPcEnvCodi, DbType.Int32, entity.TrnPcEnvCodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void GenerarCargaDatosAprobar(TrnPotenciaContratadaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveRegistrosPotenciaAprobar);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.CoresoCodi, DbType.Int32, entity.CoresoCodi);
            dbProvider.AddInParameter(command, helper.CoregeCodi, DbType.Int32, entity.CoregeCodi);
            dbProvider.AddInParameter(command, helper.TrnnpcCodicas, DbType.String, entity.TrnpcTipoCasoAgrupado);
            dbProvider.AddInParameter(command, helper.TrnPctTotalMWFija, DbType.Decimal, entity.TrnPctTotalMwFija);
            dbProvider.AddInParameter(command, helper.TrnPctHPMWFija, DbType.Decimal, entity.TrnPctHpMwFija);
            dbProvider.AddInParameter(command, helper.TrnPctHFPMWFija, DbType.Decimal, entity.TrnPctHfpMwFija);
            dbProvider.AddInParameter(command, helper.TrnPctTotalMWVariable, DbType.Decimal, entity.TrnPctTotalMwVariable);
            dbProvider.AddInParameter(command, helper.TrnPctHPMWFijaVariable, DbType.Decimal, entity.TrnPctHpMwFijaVariable);
            dbProvider.AddInParameter(command, helper.TrnPctHFPMWFijaVariable, DbType.Decimal, entity.TrnPctHfpMwFijaVariable);
            dbProvider.AddInParameter(command, helper.TrnPctComeObs, DbType.String, entity.TrnPctComeObs);
            dbProvider.AddInParameter(command, helper.TrnPctUsernameIns, DbType.String, entity.TrnPctUserNameIns);
            dbProvider.AddInParameter(command, helper.TrnPcEnvCodi, DbType.Int32, entity.TrnPcEnvCodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void AprobarSolicitudCambios(TrnPotenciaContratadaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlAprobarSolicitudCambios);
            dbProvider.AddInParameter(command, helper.TrnPctUsernameIns, DbType.String, entity.TrnPctUserNameIns);
            dbProvider.AddInParameter(command, helper.CoresoCodi, DbType.Int32, entity.CoresoCodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void RechazarSolicitudCambios(TrnPotenciaContratadaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlRechazarSolicitudCambios);
            dbProvider.AddInParameter(command, helper.TrnPctUsernameIns, DbType.String, entity.TrnPctUserNameIns);
            dbProvider.AddInParameter(command, helper.CoresoCodi, DbType.Int32, entity.CoresoCodi);
            dbProvider.ExecuteNonQuery(command);
        }


        #endregion

        #region Metodos Adicionales
        //assetec 20200604
        public void CopiarPotenciasContratadasByPeriodo(int idPeriodoActual, int idPeriodoAnterior, string sUser)
        {
            #region Auto Genera Correlativo
            int id = 1;
            DbCommand commandMaxId = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(commandMaxId);
            if (result != null)
            {
                id = Convert.ToInt32(result) - 1;
            }
            #endregion

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCopiarPotenciasContratadasByPeriodo);
            dbProvider.AddInParameter(command, helper.TrnPctCodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, idPeriodoActual);
            dbProvider.AddInParameter(command, helper.TrnPctUserNameIns, DbType.String, sUser);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, idPeriodoAnterior);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByPeriodo(int pericodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByPeriodo);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.ExecuteNonQuery(command);
        }
        #endregion
    }
}