using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{

    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class TransferenciaRetiroRepository : RepositoryBase, ITransferenciaRetiroRepository
    {
        public TransferenciaRetiroRepository(string strConn)
            : base(strConn)
        {
        }

        TransferenciaRetiroHelper helper = new TransferenciaRetiroHelper();

        public int Save(TransferenciaRetiroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            int iTRetCodi = GetCodigoGenerado();
            dbProvider.AddInParameter(command, helper.TRANRETICODI, DbType.Int32, iTRetCodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, entity.BarrCodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.CLICODI, DbType.Int32, entity.CliCodi);
            dbProvider.AddInParameter(command, helper.TRETTABLA, DbType.String, entity.TretTabla);
            dbProvider.AddInParameter(command, helper.TRETCORESOCORESCCODI, DbType.Int32, entity.TRetCoresoCorescCodi);
            dbProvider.AddInParameter(command, helper.SOLICODIRETICODIGO, DbType.String, entity.SoliCodiRetiCodigo);
            dbProvider.AddInParameter(command, helper.TRANRETIVERSION, DbType.Int32, entity.TranRetiVersion);
            dbProvider.AddInParameter(command, helper.TRANRETITIPOINFORMACION, DbType.String, entity.TranRetiTipoInformacion);
            dbProvider.AddInParameter(command, helper.TRANRETIESTADO, DbType.String, "ACT");
            dbProvider.AddInParameter(command, helper.TRETUSERNAME, DbType.String, entity.TretUserName);
            dbProvider.AddInParameter(command, helper.TRANRETIFECINS, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return iTRetCodi;
        }

        public void Update(TransferenciaRetiroDTO entity)
        {

        }

        public void Delete(int pericodi, int version, string sCodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TRANRETIVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.SOLICODIRETICODIGO, DbType.String, sCodigo);
            dbProvider.ExecuteNonQuery(command);
        }

        public TransferenciaRetiroDTO GetById(System.Int32 id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.TRANRETICODI, DbType.Int32, id);
            TransferenciaRetiroDTO entity = null;
            return entity;
        }

        public List<TransferenciaRetiroDTO> List(int emprcodi, int pericodi, int version, int barracodi)
        {
            List<TransferenciaRetiroDTO> entitys = new List<TransferenciaRetiroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TRANRETIVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.TRANRETIVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, barracodi);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, barracodi);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, barracodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TransferenciaRetiroDTO entity = new TransferenciaRetiroDTO();

                    int iSOLICODIRETICODIGO = dr.GetOrdinal(helper.SOLICODIRETICODIGO);
                    if (!dr.IsDBNull(iSOLICODIRETICODIGO)) entity.SoliCodiRetiCodigo = dr.GetString(iSOLICODIRETICODIGO);

                    int iEMPRNOMBRE = dr.GetOrdinal(helper.EMPRNOMBRE);
                    if (!dr.IsDBNull(iEMPRNOMBRE)) entity.EmprNombre = dr.GetString(iEMPRNOMBRE);

                    int iCLINOMBRE = dr.GetOrdinal(helper.CLINOMBRE);
                    if (!dr.IsDBNull(iCLINOMBRE)) entity.CliNombre = dr.GetString(iCLINOMBRE);

                    int iBARRNOMBRE = dr.GetOrdinal(helper.BARRNOMBRE);
                    if (!dr.IsDBNull(iBARRNOMBRE)) entity.BarrNombre = dr.GetString(iBARRNOMBRE);

                    int iTRANRETITIPOINFORMACION = dr.GetOrdinal(helper.TRANRETITIPOINFORMACION);
                    if (!dr.IsDBNull(iTRANRETITIPOINFORMACION)) entity.TranRetiTipoInformacion = dr.GetString(iTRANRETITIPOINFORMACION);

                    int iTOTAL = dr.GetOrdinal(helper.TOTAL);
                    if (!dr.IsDBNull(iTOTAL)) entity.Total = dr.GetDecimal(iTOTAL);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public int GetCodigoGeneradoDesc()
        {
            int newId = -1;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGeneradoDec);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public TransferenciaRetiroDTO GetTransferenciaRetiroByCodigo(int iEmprCodi, int iPeriCodi, int iTEntVersion, string sCodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetTransferenciaRetiroByCodigo);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iEmprCodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iEmprCodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.TRANRETIVERSION, DbType.Int32, iTEntVersion);
            dbProvider.AddInParameter(command, helper.SOLICODIRETICODIGO, DbType.String, sCodigo);

            TransferenciaRetiroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TransferenciaRetiroDTO> ListByPeriodoVersion(int iPericodi, int iVersion)
        {
            List<TransferenciaRetiroDTO> entitys = new List<TransferenciaRetiroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByPeriodoVersion);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPericodi);
            dbProvider.AddInParameter(command, helper.TRANRETIVERSION, DbType.Int32, iVersion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public List<TransferenciaRetiroDTO> ListByPeriodoVersionEmpresa(int iPericodi, int iVersion, int iEmprCodi)
        {
            List<TransferenciaRetiroDTO> entitys = new List<TransferenciaRetiroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByPeriodoVersionEmpresa);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPericodi);
            dbProvider.AddInParameter(command, helper.TRANRETIVERSION, DbType.Int32, iVersion);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iEmprCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public void DeleteListaTransferenciaRetiro(int iPeriCodi, int iVersion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteListaTransferenciaRetiro);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.TRANRETIVERSION, DbType.Int32, iVersion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteListaTransferenciaRetiroEmpresa(int iPeriCodi, int iVersion, int iEmprCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteListaTransferenciaRetiroEmpresa);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.TRANRETIVERSION, DbType.Int32, iVersion);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iEmprCodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void BulkInsert(List<TrnTransRetiroBullk> entitys)
        {
            dbProvider.AddColumnMapping(helper.TRANRETICODI, DbType.Int32);
            dbProvider.AddColumnMapping(helper.PERICODI, DbType.Int32);
            dbProvider.AddColumnMapping(helper.BARRCODI, DbType.Int32);
            dbProvider.AddColumnMapping(helper.EMPRCODI, DbType.Int32);
            dbProvider.AddColumnMapping(helper.CLICODI, DbType.Int32);
            dbProvider.AddColumnMapping(helper.TRETTABLA, DbType.String);
            dbProvider.AddColumnMapping(helper.TRETCORESOCORESCCODI, DbType.Int32);
            dbProvider.AddColumnMapping(helper.SOLICODIRETICODIGO, DbType.String);
            dbProvider.AddColumnMapping(helper.TRANRETIVERSION, DbType.Int32);
            dbProvider.AddColumnMapping(helper.TRANRETITIPOINFORMACION, DbType.String);
            dbProvider.AddColumnMapping(helper.TRANRETIESTADO, DbType.String);
            dbProvider.AddColumnMapping(helper.TRETUSERNAME, DbType.String);
            dbProvider.AddColumnMapping(helper.TRANRETIFECINS, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.TRANRETIFECACT, DbType.DateTime);

            dbProvider.BulkInsert<TrnTransRetiroBullk>(entitys, helper.TableName);
        }

        public void CopiarRetiros(int iPeriCodi, int iVersionOld, int iVersionNew, int iTranRetiCodi, int iTranRetiDetaCodi, IDbConnection conn, DbTransaction tran)
        {
            //1.- Insertamos la nueva versión en la tabla trn_trans_retiro
            DbCommand commandTR = (DbCommand)conn.CreateCommand();
            commandTR.Transaction = tran;
            commandTR.Connection = (DbConnection)conn;
            commandTR.CommandText = helper.SqlCopiarRetiros;
            IDbDataParameter paramTR = commandTR.CreateParameter();
            paramTR.ParameterName = helper.TRANRETICODI;
            paramTR.Value = iTranRetiCodi;
            commandTR.Parameters.Add(paramTR);

            paramTR = commandTR.CreateParameter();
            paramTR.ParameterName = helper.PERICODI;
            paramTR.Value = iPeriCodi;
            commandTR.Parameters.Add(paramTR);

            paramTR = commandTR.CreateParameter();
            paramTR.ParameterName = helper.TRANRETIVERSION;
            paramTR.Value = iVersionNew;
            commandTR.Parameters.Add(paramTR);

            paramTR = commandTR.CreateParameter();
            paramTR.ParameterName = helper.PERICODI;
            paramTR.Value = iPeriCodi;
            commandTR.Parameters.Add(paramTR);

            paramTR = commandTR.CreateParameter();
            paramTR.ParameterName = helper.BARRCODI;
            paramTR.Value = iVersionOld;
            commandTR.Parameters.Add(paramTR);

            commandTR.ExecuteNonQuery();

            /*
            //--------------------------------------------------------------------------------------------------
            //2.- Insertamos en la tabla de equivalencias los Id
            DbCommand commandTMP = (DbCommand)conn.CreateCommand();
            commandTMP.Transaction = tran;
            commandTMP.Connection = (DbConnection)conn;

            //Limpiamos tabla temporal
            commandTMP.CommandText = "delete from trn_tmp_equiv";
            commandTMP.ExecuteNonQuery();

            //insertamos en la tabla temporal
            commandTMP.CommandText = helper.SqlCopiarTemporal;
            IDbDataParameter paramTMP = commandTMP.CreateParameter();
            paramTMP = commandTMP.CreateParameter();
            paramTMP.ParameterName = helper.TRANRETICODI;
            paramTMP.Value = iTranRetiCodi;
            commandTMP.Parameters.Add(paramTMP);

            paramTMP = commandTMP.CreateParameter();
            paramTMP.ParameterName = helper.PERICODI;
            paramTMP.Value = iPeriCodi;
            commandTMP.Parameters.Add(paramTMP);

            paramTMP = commandTMP.CreateParameter();
            paramTMP.ParameterName = helper.BARRCODI;
            paramTMP.Value = iVersionOld;
            commandTMP.Parameters.Add(paramTMP);

            commandTMP.ExecuteNonQuery();*/            
           
            //-----------------------------------------------------------------------------------------------------
            //3.- Insertamos en trn_trans_retiro_detalle
            DbCommand commandTRD = (DbCommand)conn.CreateCommand();
            commandTRD.Transaction = tran;
            commandTRD.Connection = (DbConnection)conn;
            commandTRD.CommandText = helper.SqlCopiarRetirosDetalle;
            IDbDataParameter paramTRD = commandTRD.CreateParameter();
            /*paramTRD = commandTRD.CreateParameter();
            paramTRD.ParameterName = helper.EMPRCODI;
            paramTRD.Value = iTranRetiDetaCodi;
            commandTRD.Parameters.Add(paramTRD);*/

            paramTRD = commandTRD.CreateParameter();
            paramTRD.ParameterName = helper.TRANRETIVERSION;
            paramTRD.Value = iVersionNew;
            commandTRD.Parameters.Add(paramTRD);

            paramTRD = commandTRD.CreateParameter();
            paramTRD.ParameterName = helper.PERICODI;
            paramTRD.Value = iPeriCodi;
            commandTRD.Parameters.Add(paramTRD);

            //ASSETEC: 20200804
            paramTRD = commandTRD.CreateParameter();
            paramTRD.ParameterName = helper.BARRCODI;
            paramTRD.Value = iVersionOld;
            commandTRD.Parameters.Add(paramTRD);

            paramTRD = commandTRD.CreateParameter();
            paramTRD.ParameterName = helper.PERICODI;
            paramTRD.Value = iPeriCodi;
            commandTRD.Parameters.Add(paramTRD);

            paramTRD = commandTRD.CreateParameter();
            paramTRD.ParameterName = helper.TRANRETIVERSION;
            paramTRD.Value = iVersionNew;
            commandTRD.Parameters.Add(paramTRD);

            commandTRD.ExecuteNonQuery();
        }

        //ASSETEC 202001
        public TransferenciaRetiroDTO GetTransferenciaRetiroByCodigoEnvio(int iEmprCodi, int iPeriCodi, int iTEntVersion, int trnenvcodi, string sCodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetTransferenciaRetiroByCodigoEnvio);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iEmprCodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iEmprCodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.TRANRETIVERSION, DbType.Int32, iTEntVersion);
            dbProvider.AddInParameter(command, helper.TRNENVCODI, DbType.Int32, trnenvcodi);
            dbProvider.AddInParameter(command, helper.TRNENVCODI, DbType.Int32, trnenvcodi);
            dbProvider.AddInParameter(command, helper.SOLICODIRETICODIGO, DbType.String, sCodigo);

            TransferenciaRetiroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public void UpdateTransferenciaRetiroEstadoINA(int pericodi, int recacodi, List<string> listaRetiros, int genemprcodi, string suser)
        {
            string sqlUpdate = string.Format(helper.SqlUpdateEstadoINA, pericodi, recacodi, string.Join(", ", listaRetiros), genemprcodi, suser); //TRN_TRANS_RETIRO -> tretestado = INA
            DbCommand command = dbProvider.GetSqlStringCommand(sqlUpdate);
            command.CommandTimeout = 0;
            dbProvider.ExecuteNonQuery(command);
        }

        //GetRetiroBy
        public List<TransferenciaRetiroDTO> GetRetiroBy(int periodo, int recacodi, int genemprcodi)
        {
            List<TransferenciaRetiroDTO> entitys = new List<TransferenciaRetiroDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetTransferenciaRetiroBy);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, periodo);
            dbProvider.AddInParameter(command, helper.TRANRETIVERSION, DbType.Int32, recacodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, genemprcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }
    }
}
