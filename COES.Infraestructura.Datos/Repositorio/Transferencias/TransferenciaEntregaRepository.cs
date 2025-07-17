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
    public class TransferenciaEntregaRepository : RepositoryBase, ITransferenciaEntregaRepository
    {
        public TransferenciaEntregaRepository(string strConn)
            : base(strConn)
        {
        }

        TransferenciaEntregaHelper helper = new TransferenciaEntregaHelper();

        public int Save(TransferenciaEntregaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            int iTEntCodi = GetCodigoGenerado();

            dbProvider.AddInParameter(command, helper.TRANENTRCODI, DbType.Int32, iTEntCodi);

            dbProvider.AddInParameter(command, helper.CODENTCODI, DbType.Int32, entity.CodEntCodi);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, entity.BarrCodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.CENTGENECODI, DbType.Int32, entity.CentGeneCodi);
            dbProvider.AddInParameter(command, helper.CODIENTRCODIGO, DbType.String, entity.CodiEntrCodigo);
            dbProvider.AddInParameter(command, helper.TRANENTRVERSION, DbType.Int32, entity.TranEntrVersion);
            dbProvider.AddInParameter(command, helper.TRANENTRTIPOINFORMACION, DbType.String, entity.TranEntrTipoInformacion);
            dbProvider.AddInParameter(command, helper.TRANENTRESTADO, DbType.String, "ACT");
            dbProvider.AddInParameter(command, helper.TENTUSERNAME, DbType.String, entity.TentUserName);
            dbProvider.AddInParameter(command, helper.TRANENTRFECINS, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return iTEntCodi;
        }

        public void Update(TransferenciaEntregaDTO entity)
        {

        }

        public void Delete(int pericodi, int version, string sCodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TRANENTRVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.CODIENTRCODIGO, DbType.String, sCodigo);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteListaTransferenciaEntrega(int pericodi, int version)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteListaTransferenciaEntrega);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TRANENTRVERSION, DbType.Int32, version);

            dbProvider.ExecuteNonQuery(command);
        }

        public TransferenciaEntregaDTO GetById(System.Int32 id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.TRANENTRCODI, DbType.Int32, id);
            TransferenciaEntregaDTO entity = null;

            return entity;
        }

        public List<TransferenciaEntregaDTO> List(int emprcodi, int pericodi, int version, int barracodi)
        {
            List<TransferenciaEntregaDTO> entitys = new List<TransferenciaEntregaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TRANENTRVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.TRANENTRVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, barracodi);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, barracodi);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, barracodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TransferenciaEntregaDTO entity = new TransferenciaEntregaDTO();

                    int iCODIENTRCODIGO = dr.GetOrdinal(helper.CODIENTRCODIGO);
                    if (!dr.IsDBNull(iCODIENTRCODIGO)) entity.CodiEntrCodigo = dr.GetString(iCODIENTRCODIGO);

                    int iEMPRNOMBRE = dr.GetOrdinal(helper.EMPRNOMBRE);
                    if (!dr.IsDBNull(iEMPRNOMBRE)) entity.EmprNombre = dr.GetString(iEMPRNOMBRE);

                    int iCENTGENENOMBRE = dr.GetOrdinal(helper.CENTGENENOMBRE);
                    if (!dr.IsDBNull(iCENTGENENOMBRE)) entity.CentGeneNombre = dr.GetString(iCENTGENENOMBRE);

                    int iBARRNOMBRE = dr.GetOrdinal(helper.BARRNOMBRE);
                    if (!dr.IsDBNull(iBARRNOMBRE)) entity.BarrNombre = dr.GetString(iBARRNOMBRE);

                    int iTRANENTRTIPOINFORMACION = dr.GetOrdinal(helper.TRANENTRTIPOINFORMACION);
                    if (!dr.IsDBNull(iTRANENTRTIPOINFORMACION)) entity.TranEntrTipoInformacion = dr.GetString(iTRANENTRTIPOINFORMACION);

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

        public int GetCodigoGeneradoDec()
        {
            int newId = -1;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGeneradoDec);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public TransferenciaEntregaDTO GetTransferenciaEntregaByCodigo(int iEmprCodi, int iPeriCodi, int iTEntVersion, string sCodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetTransferenciaEntregaByCodigo);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iEmprCodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iEmprCodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.TRANENTRVERSION, DbType.Int32, iTEntVersion);
            dbProvider.AddInParameter(command, helper.CODIENTRCODIGO, DbType.String, sCodigo);

            TransferenciaEntregaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TransferenciaEntregaDTO> ListByPeriodoVersion(int iPericodi, int iVersion)
        {
            List<TransferenciaEntregaDTO> entitys = new List<TransferenciaEntregaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByPeriodoVersion);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPericodi);
            dbProvider.AddInParameter(command, helper.TRANENTRVERSION, DbType.Int32, iVersion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public void BulkInsert(List<TrnTransEntregaBullk> entitys)
        {
            dbProvider.AddColumnMapping(helper.TRANENTRCODI, DbType.Int32);
            dbProvider.AddColumnMapping(helper.CODENTCODI, DbType.Int32);
            dbProvider.AddColumnMapping(helper.BARRCODI, DbType.Int32);
            dbProvider.AddColumnMapping(helper.PERICODI, DbType.Int32);
            dbProvider.AddColumnMapping(helper.EMPRCODI, DbType.Int32);
            dbProvider.AddColumnMapping(helper.CENTGENECODI, DbType.Int32);
            dbProvider.AddColumnMapping(helper.CODIENTRCODIGO, DbType.String);
            dbProvider.AddColumnMapping(helper.TRANENTRVERSION, DbType.Int32);
            dbProvider.AddColumnMapping(helper.TRANENTRTIPOINFORMACION, DbType.String);
            dbProvider.AddColumnMapping(helper.TRANENTRESTADO, DbType.String);
            dbProvider.AddColumnMapping(helper.TENTUSERNAME, DbType.String);
            dbProvider.AddColumnMapping(helper.TRANENTRFECINS, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.TRANENTRFECACT, DbType.DateTime);

            dbProvider.BulkInsert<TrnTransEntregaBullk>(entitys, helper.TableName);
        }

        public void CopiarEntregas(int iPeriCodi, int iVersionOld, int iVersionNew, int iTransEntrCodi, int iTransEntrDetCodi, IDbConnection conn, DbTransaction tran)
        {
            //1.- Insertamos la nueva versión en la tabla trn_trans_entrega
            DbCommand commandTE = (DbCommand)conn.CreateCommand();
            commandTE.Transaction = tran;
            commandTE.Connection = (DbConnection)conn;
            commandTE.CommandText = helper.SqlCopiarEntregas;
            IDbDataParameter paramTE = commandTE.CreateParameter();
            paramTE.ParameterName = helper.TRANENTRCODI;
            paramTE.Value = iTransEntrCodi;
            commandTE.Parameters.Add(paramTE);

            paramTE = commandTE.CreateParameter();
            paramTE.ParameterName = helper.PERICODI;
            paramTE.Value = iPeriCodi;
            commandTE.Parameters.Add(paramTE);

            paramTE = commandTE.CreateParameter();
            paramTE.ParameterName = helper.TRANENTRVERSION;
            paramTE.Value = iVersionNew;
            commandTE.Parameters.Add(paramTE);

            paramTE = commandTE.CreateParameter();
            paramTE.ParameterName = helper.PERICODI;
            paramTE.Value = iPeriCodi;
            commandTE.Parameters.Add(paramTE);

            paramTE = commandTE.CreateParameter();
            paramTE.ParameterName = helper.BARRCODI;
            paramTE.Value = iVersionOld;
            commandTE.Parameters.Add(paramTE);

            commandTE.ExecuteNonQuery();

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
            paramTMP.ParameterName = helper.TRANENTRCODI;
            paramTMP.Value = iTransEntrCodi;
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
            //3.- Insertamos en trn_trans_entrega_detalle
            DbCommand commandTED = (DbCommand)conn.CreateCommand();
            commandTED.Transaction = tran;
            commandTED.Connection = (DbConnection)conn;
            commandTED.CommandText = helper.SqlCopiarEntregasDetalle;
            IDbDataParameter paramTED = commandTED.CreateParameter();
            /*paramTED = commandTED.CreateParameter();
            paramTED.ParameterName = helper.EMPRCODI;
            paramTED.Value = iTransEntrDetCodi;
            commandTED.Parameters.Add(paramTED);*/

            paramTED = commandTED.CreateParameter();
            paramTED.ParameterName = helper.TRANENTRVERSION;
            paramTED.Value = iVersionNew;
            commandTED.Parameters.Add(paramTED);

            paramTED = commandTED.CreateParameter();
            paramTED.ParameterName = helper.PERICODI;
            paramTED.Value = iPeriCodi;
            commandTED.Parameters.Add(paramTED);

            //ASSETEC: 20200804
            paramTED = commandTED.CreateParameter();
            paramTED.ParameterName = helper.BARRCODI;
            paramTED.Value = iVersionOld;
            commandTED.Parameters.Add(paramTED);

            paramTED = commandTED.CreateParameter();
            paramTED.ParameterName = helper.PERICODI;
            paramTED.Value = iPeriCodi;
            commandTED.Parameters.Add(paramTED);

            paramTED = commandTED.CreateParameter();
            paramTED.ParameterName = helper.TRANENTRVERSION;
            paramTED.Value = iVersionNew;
            commandTED.Parameters.Add(paramTED);

            commandTED.ExecuteNonQuery();
        }

        //ASSETEC 202001
        public TransferenciaEntregaDTO GetTransferenciaEntregaByCodigoEnvio(int iEmprCodi, int iPeriCodi, int iTEntVersion, int trnenvcodi, string sCodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetTransferenciaEntregaByCodigoEnvio);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iEmprCodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iEmprCodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.TRANENTRVERSION, DbType.Int32, iTEntVersion);
            dbProvider.AddInParameter(command, helper.TRNENVCODI, DbType.Int32, trnenvcodi);
            dbProvider.AddInParameter(command, helper.TRNENVCODI, DbType.Int32, trnenvcodi);
            dbProvider.AddInParameter(command, helper.CODIENTRCODIGO, DbType.String, sCodigo);

            TransferenciaEntregaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
        public void UpdateTransferenciaEntregaEstadoINA(int pericodi, int recacodi, List<string> listaEntregas, int emprcodi, string suser)
        {
            string sqlUpdate = string.Format(helper.SqlUpdateEstadoINA, pericodi, recacodi, string.Join(", ", listaEntregas), emprcodi, suser); //TRN_TRANS_ENTREGA -> tentestado = INA
            DbCommand command = dbProvider.GetSqlStringCommand(sqlUpdate);
            command.CommandTimeout = 0;
            dbProvider.ExecuteNonQuery(command);
        }
    }
}
