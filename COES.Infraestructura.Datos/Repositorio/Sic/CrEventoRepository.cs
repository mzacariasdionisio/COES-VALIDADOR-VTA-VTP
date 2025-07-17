using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class CrEventoRepository : RepositoryBase, ICrEventoRepository
    {
        private string strConexion;
        public CrEventoRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }
        CrEventoHelper helper = new CrEventoHelper();
        public List<CrEventoDTO> ListCrEventos(CrEventoDTO oEventoDTO)
        {
            List<CrEventoDTO> entitys = new List<CrEventoDTO>();
            try
            {
                string filtro = string.Empty;
                if (Convert.ToInt32(oEventoDTO.CriterioDecision) > 0 && Convert.ToInt32(oEventoDTO.CriteriosImpugnacion) > 0)
                    filtro = " ((" + oEventoDTO.CriterioDecision + "= 0 or ECRD.CRCRITERIOCODI= " + oEventoDTO.CriterioDecision + ") OR (" + oEventoDTO.CriteriosImpugnacion + "= 0 or ECRD.CRCRITERIOCODI= " + oEventoDTO.CriteriosImpugnacion + "))";
                else
                    filtro = " ((" + oEventoDTO.CriterioDecision + "= 0 or ECRD.CRCRITERIOCODI= " + oEventoDTO.CriterioDecision + ") AND (" + oEventoDTO.CriteriosImpugnacion + "= 0 or ECRD.CRCRITERIOCODI= " + oEventoDTO.CriteriosImpugnacion + "))";

                string query = string.Format(helper.SqlList,
                    oEventoDTO.DI, //0
                    oEventoDTO.DF, //1
                    oEventoDTO.EmpresaPropietaria,  //2
                    oEventoDTO.EmpresaInvolucrada, //3
                    oEventoDTO.CriterioDecision, //4
                    oEventoDTO.CasosEspeciales, //5
                    oEventoDTO.Impugnacionc, //6
                    oEventoDTO.CriteriosImpugnacion,//7
                    filtro); //8
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        CrEventoDTO entity = new CrEventoDTO();
                        //EVENTO
                        int iCREVENCODI = dr.GetOrdinal("CREVENCODI");
                        int iCODIGO = dr.GetOrdinal("CODIGO");
                        int iFECHA_EVENTO = dr.GetOrdinal("FECHA_EVENTO");
                        int iNOMBRE_EVENTO = dr.GetOrdinal("NOMBRE_EVENTO");
                        int iCASOS_ESPECIAL = dr.GetOrdinal("CASO_ESPECIAL");
                        int iIMPUGNACION = dr.GetOrdinal("IMPUGNACION");
                        int iDESCRIPCION_EVE_EVENTO = dr.GetOrdinal("DESCRIPCION_EVE_EVENTO");
                        int iAFEITDECFECHAELAB = dr.GetOrdinal("AFEITDECFECHAELAB");
                        int iEVENDESCCTAF = dr.GetOrdinal("EVENDESCCTAF");
                        //ETAPA1
                        //int iCODIGO_ETAPA = dr.GetOrdinal("CODIGO_ETAPA");
                        //int iETAPA = dr.GetOrdinal("ETAPA");
                        //int iFECHA_DECISION = dr.GetOrdinal("FECHA_DECISION");
                        //int iDESCRIPCION_EVENTO_DECISION = dr.GetOrdinal("DESCRIPCION_EVENTO");

                        //int iRESUMEN_DECISION = dr.GetOrdinal("RESUMEN_DECISION");
                        //int iRESPONSABLE_DECISION = dr.GetOrdinal("EMPRESAS_RESPONSABLES");
                        //int iCOMENTARIO_EMPRESA_DECISION = dr.GetOrdinal("COMENTARIO_EMPRESA");
                        //int iCRITERIO_DECISION = dr.GetOrdinal("CRITERIO_RESPONSABLE");

                        //EVENTO
                        if (!dr.IsDBNull(iCREVENCODI)) entity.CREVENCODI = dr.GetInt32(iCREVENCODI);
                        if (!dr.IsDBNull(iCODIGO)) entity.CODIGO = dr.GetString(iCODIGO);
                        if (!dr.IsDBNull(iFECHA_EVENTO)) entity.FECHA_EVENTO = dr.GetString(iFECHA_EVENTO);
                        if (!dr.IsDBNull(iNOMBRE_EVENTO)) entity.NOMBRE_EVENTO = dr.GetString(iNOMBRE_EVENTO);
                        if (!dr.IsDBNull(iCASOS_ESPECIAL)) entity.CASOS_ESPECIAL = dr.GetString(iCASOS_ESPECIAL);
                        if (!dr.IsDBNull(iIMPUGNACION)) entity.IMPUGNACION = dr.GetString(iIMPUGNACION);
                        if (!dr.IsDBNull(iDESCRIPCION_EVE_EVENTO)) entity.DESCRIPCION_EVE_EVENTO = dr.GetString(iDESCRIPCION_EVE_EVENTO);
                        if (!dr.IsDBNull(iAFEITDECFECHAELAB)) entity.AFEITDECFECHAELAB = dr.GetDateTime(iAFEITDECFECHAELAB);
                        if (!dr.IsDBNull(iEVENDESCCTAF)) entity.EVENDESCCTAF = dr.GetString(iEVENDESCCTAF);
                        //ETAPA1
                        //if (!dr.IsDBNull(iCODIGO_ETAPA)) entity.CODIGO_ETAPA = dr.GetInt32(iCODIGO_ETAPA);
                        //if (!dr.IsDBNull(iETAPA)) entity.ETAPA = dr.GetInt32(iETAPA);
                        //if (!dr.IsDBNull(iFECHA_DECISION)) entity.FECHA_DECISION = dr.GetString(iFECHA_DECISION);
                        //if (!dr.IsDBNull(iDESCRIPCION_EVENTO_DECISION)) entity.DESCRIPCION_EVENTO_DECISION = dr.GetString(iDESCRIPCION_EVENTO_DECISION);

                        //if (!dr.IsDBNull(iRESUMEN_DECISION)) entity.RESUMEN_DECISION = dr.GetString(iRESUMEN_DECISION);
                        //if (!dr.IsDBNull(iRESPONSABLE_DECISION)) entity.RESPONSABLE_DECISION = dr.GetString(iRESPONSABLE_DECISION);
                        //if (!dr.IsDBNull(iCOMENTARIO_EMPRESA_DECISION)) entity.COMENTARIO_EMPRESA_DECISION = dr.GetString(iCOMENTARIO_EMPRESA_DECISION);
                        //if (!dr.IsDBNull(iCRITERIO_DECISION)) entity.CRITERIO_DECISION = dr.GetString(iCRITERIO_DECISION);

                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            
            return entitys;
        }

        public void Update(CrEventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Crespecialcodi, DbType.Int32, entity.CRESPECIALCODI);          
            dbProvider.AddInParameter(command, helper.Crevencodi, DbType.Int32, entity.CREVENCODI);

            dbProvider.ExecuteNonQuery(command);
        }

        public CrEventoDTO GetById(int crevencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Crevencodi, DbType.Int32, crevencodi);
            CrEventoDTO entity = new CrEventoDTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iCodigoEvento = dr.GetOrdinal(helper.CodigoEvento);
                    if (!dr.IsDBNull(iCodigoEvento)) entity.CODIGO = dr.GetString(iCodigoEvento);

                    int iFechaevento = dr.GetOrdinal(helper.Fechaevento);
                    if (!dr.IsDBNull(iFechaevento)) entity.FECHA_EVENTO = dr.GetString(iFechaevento);

                    int iNombreevento = dr.GetOrdinal(helper.Nombreevento);
                    if (!dr.IsDBNull(iNombreevento)) entity.NOMBRE_EVENTO = dr.GetString(iNombreevento);

                    int iImpugnacion = dr.GetOrdinal(helper.Impugnacion);
                    if (!dr.IsDBNull(iImpugnacion)) entity.IMPUGNACION = dr.GetString(iImpugnacion);

                    int iAfeitdecfechaelab = dr.GetOrdinal(helper.Afeitdecfechaelab);
                    if (!dr.IsDBNull(iAfeitdecfechaelab)) entity.AFEITDECFECHAELAB = dr.GetDateTime(iAfeitdecfechaelab);

                    int iEvendescctaf = dr.GetOrdinal(helper.Evendescctaf);
                    if (!dr.IsDBNull(iEvendescctaf)) entity.EVENDESCCTAF = dr.GetString(iEvendescctaf);
                }
            }

            return entity;
        }

        public int Save(CrEventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Crevencodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Afecodi, DbType.Int32, entity.AFECODI);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.LASTDATE);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.LASTUSER);

            dbProvider.ExecuteNonQuery(command);

            return id;

        }

        public CrEventoDTO GetByAfecodi(int afecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByAfecodi);
            dbProvider.AddInParameter(command, helper.Afecodi, DbType.Int32, afecodi);
            CrEventoDTO entity = new CrEventoDTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public void Delete(int crevencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Crevencodi, DbType.Int32, crevencodi);
            dbProvider.ExecuteNonQuery(command);
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

        public int SaveR(CrEventoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            DbCommand commandUp = (DbCommand)conn.CreateCommand();
            commandUp.CommandText = helper.SqlSave;
            commandUp.Transaction = tran;
            commandUp.Connection = (DbConnection)conn;

            IDbDataParameter param = null;

            param = commandUp.CreateParameter(); param.ParameterName = helper.Crevencodi; param.Value = id; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Afecodi; param.Value = entity.AFECODI; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Lastdate; param.Value = entity.LASTDATE; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Lastuser; param.Value = entity.LASTUSER; commandUp.Parameters.Add(param);

            try
            {
                commandUp.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return -1;
            }

            return id;
        }
    }
}
