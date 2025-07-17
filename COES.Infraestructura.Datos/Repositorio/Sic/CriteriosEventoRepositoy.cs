using COES.Base.DataHelper;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Infraestructura.Datos.Helper.Sic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class CriteriosEventoRepositoy: RepositoryBase
    {
        public CriteriosEventoRepositoy(string strConn)
            : base(strConn)
        {
        }

        CriteriosEventoHelper helper = new CriteriosEventoHelper();

        public List<CrEventoDTO> ConsultarCriterio(CrEventoDTO oCrevento)
        {

            List<CrEventoDTO> entitys = new List<CrEventoDTO>();
            try
            {
                //string query = string.Format(helper.SqlConsultarCriterioEvento,oCrevento.NroPagina, oCrevento.NroRegistros);
                string query = string.Format(helper.SqlConsultarCriterioEvento);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        CrEventoDTO entity = new CrEventoDTO();

                        int iCOD_CRITERIO = dr.GetOrdinal("COD_CRITERIO");
                        int iDESCRIPCION_EVE_EVENTO = dr.GetOrdinal("DESCRIPCION_EVE_EVENTO");
                        int iFECHA_EVE_EVENTO = dr.GetOrdinal("FECHA_EVE_EVENTO");
                        //int iDESCRITERIO = dr.GetOrdinal("DESCRITERIO");
                        int iDESCASOESPECIAL = dr.GetOrdinal("DESCASOESPECIAL");


                        if (!dr.IsDBNull(iCOD_CRITERIO)) entity.COD_CRITERIO = dr.GetInt32(iCOD_CRITERIO);
                        if (!dr.IsDBNull(iDESCRIPCION_EVE_EVENTO)) entity.DESCRIPCION_EVE_EVENTO = dr.GetString(iDESCRIPCION_EVE_EVENTO);
                        if (!dr.IsDBNull(iFECHA_EVE_EVENTO)) entity.FECHA_EVE_EVENTO = dr.GetDateTime(iFECHA_EVE_EVENTO);
                       // if (!dr.IsDBNull(iDESCRITERIO)) entity.DESCRITERIO = dr.GetString(iDESCRITERIO);
                        if (!dr.IsDBNull(iDESCASOESPECIAL)) entity.DESCASOESPECIAL = dr.GetString(iDESCASOESPECIAL);
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

        public List<CrEventoDTO> ConsultarCriterio2(CrEventoDTO oEventoDTO)
        {

            List<CrEventoDTO> entitys = new List<CrEventoDTO>();
            try
            {
                string query = string.Format(helper.SqlConsultarCriterioEvento2,
                    oEventoDTO.DI, //0
                    oEventoDTO.DF, //1
                    oEventoDTO.EmpresaPropietaria,  //2
                    oEventoDTO.EmpresaInvolucrada, //3
                    oEventoDTO.CriterioDecision, //4
                    oEventoDTO.CasosEspeciales, //5
                    oEventoDTO.Impugnacionc, //6
                    oEventoDTO.CriteriosImpugnacion); //7
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
                        int iCASOS_ESPECIAL = dr.GetOrdinal("CASOS_ESPECIAL");
                        int iIMPUGNACION = dr.GetOrdinal("IMPUGNACION");
                        //ETAPA1
                        int iFECHA_DECISION = dr.GetOrdinal("FECHA_DECISION");
                        int iDESCRIPCION_EVENTO_DECISION = dr.GetOrdinal("DESCRIPCION_EVENTO_DECISION");
                        int iRESUMEN_DECISION = dr.GetOrdinal("RESUMEN_DECISION");
                        int iRESPONSABLE_DECISION = dr.GetOrdinal("RESPONSABLE_DECISION");
                        int iCOMENTARIO_EMPRESA_DECISION = dr.GetOrdinal("COMENTARIO_EMPRESA_DECISION");
                        int iCRITERIO_DECISION = dr.GetOrdinal("CRITERIO_DECISION");
                        //ETAPA2
                        int iEMPR_SOLI_RECONSIDERACION = dr.GetOrdinal("EMPR_SOLI_RECONSIDERACION");
                        int iARGUMENTO_RECONCIDERACION = dr.GetOrdinal("ARGUMENTO_RECONCIDERACION");
                        int iDECISION_RECONCIDERACION = dr.GetOrdinal("DECISION_RECONCIDERACION");
                        int iRESPONSABLE_RECONCIDERACION = dr.GetOrdinal("RESPONSABLE_RECONCIDERACION");
                        int iCOMENTARIOS_RECONCIDERACION = dr.GetOrdinal("COMENTARIOS_RECONCIDERACION");
                        int iCRITERIOS_RECONSIDERACION = dr.GetOrdinal("CRITERIOS_RECONSIDERACION");
                        //ETAPA3
                        int iEMPR_SOLI_APELACION = dr.GetOrdinal("EMPR_SOLI_APELACION");
                        int iARGUMENTO_APELACION = dr.GetOrdinal("ARGUMENTO_APELACION");
                        int iDECISION_APELACION = dr.GetOrdinal("DECISION_APELACION");
                        int iRESPONSABLE_APELACION = dr.GetOrdinal("RESPONSABLE_APELACION");
                        int iCOMENTARIOS_APELACION = dr.GetOrdinal("COMENTARIOS_APELACION");
                        int iCRITERIOS_APELACION = dr.GetOrdinal("CRITERIOS_APELACION");
                        //ETAPA4
                        int iEMPR_SOLI_ARBITRAJE = dr.GetOrdinal("EMPR_SOLI_ARBITRAJE");
                        int iARGUMENTO_ARBITRAJE = dr.GetOrdinal("ARGUMENTO_ARBITRAJE");
                        int iDECISION_ARBITRAJE = dr.GetOrdinal("DECISION_ARBITRAJE");
                        int iRESPONSABLE_ARBITRAJE = dr.GetOrdinal("RESPONSABLE_ARBITRAJE");
                        int iCOMENTARIOS_ARBITRAJE = dr.GetOrdinal("COMENTARIOS_ARBITRAJE");
                        int iCRITERIOS_ARBITRAJE = dr.GetOrdinal("CRITERIOS_ARBITRAJE");

                        //EVENTO
                        if (!dr.IsDBNull(iCREVENCODI)) entity.CREVENCODI = dr.GetInt32(iCREVENCODI);
                        if (!dr.IsDBNull(iCODIGO)) entity.CODIGO = dr.GetString(iCODIGO);
                        if (!dr.IsDBNull(iFECHA_EVENTO)) entity.FECHA_EVENTO = dr.GetString(iFECHA_EVENTO);
                        if (!dr.IsDBNull(iNOMBRE_EVENTO)) entity.NOMBRE_EVENTO = dr.GetString(iNOMBRE_EVENTO);
                        if (!dr.IsDBNull(iCASOS_ESPECIAL)) entity.CASOS_ESPECIAL = dr.GetString(iCASOS_ESPECIAL);
                        //ETAPA1
                        if (!dr.IsDBNull(iFECHA_DECISION)) entity.FECHA_DECISION = dr.GetString(iFECHA_DECISION);
                        if (!dr.IsDBNull(iDESCRIPCION_EVENTO_DECISION)) entity.DESCRIPCION_EVENTO_DECISION = dr.GetString(iDESCRIPCION_EVENTO_DECISION);
                        if (!dr.IsDBNull(iRESUMEN_DECISION)) entity.RESUMEN_DECISION = dr.GetString(iRESUMEN_DECISION);
                        if (!dr.IsDBNull(iRESPONSABLE_DECISION)) entity.RESPONSABLE_DECISION = dr.GetString(iRESPONSABLE_DECISION);
                        if (!dr.IsDBNull(iCOMENTARIO_EMPRESA_DECISION)) entity.COMENTARIO_EMPRESA_DECISION = dr.GetString(iCOMENTARIO_EMPRESA_DECISION);
                        if (!dr.IsDBNull(iCRITERIO_DECISION)) entity.CRITERIO_DECISION = dr.GetString(iCRITERIO_DECISION);
                        //ETAPA2
                        if (!dr.IsDBNull(iEMPR_SOLI_RECONSIDERACION)) entity.EMPR_SOLI_RECONSIDERACION = dr.GetString(iEMPR_SOLI_RECONSIDERACION);
                        if (!dr.IsDBNull(iARGUMENTO_RECONCIDERACION)) entity.ARGUMENTO_RECONCIDERACION = dr.GetString(iARGUMENTO_RECONCIDERACION);
                        if (!dr.IsDBNull(iDECISION_RECONCIDERACION)) entity.DECISION_RECONCIDERACION = dr.GetString(iDECISION_RECONCIDERACION);
                        if (!dr.IsDBNull(iRESPONSABLE_RECONCIDERACION)) entity.RESPONSABLE_RECONCIDERACION = dr.GetString(iRESPONSABLE_RECONCIDERACION);
                        if (!dr.IsDBNull(iCOMENTARIOS_RECONCIDERACION)) entity.COMENTARIOS_RECONCIDERACION = dr.GetString(iCOMENTARIOS_RECONCIDERACION);
                        if (!dr.IsDBNull(iCRITERIOS_RECONSIDERACION)) entity.CRITERIOS_RECONSIDERACION = dr.GetString(iCRITERIOS_RECONSIDERACION);
                        //ETAPA3
                        if (!dr.IsDBNull(iEMPR_SOLI_APELACION)) entity.EMPR_SOLI_APELACION = dr.GetString(iEMPR_SOLI_APELACION);
                        if (!dr.IsDBNull(iARGUMENTO_APELACION)) entity.ARGUMENTO_APELACION = dr.GetString(iARGUMENTO_APELACION);
                        if (!dr.IsDBNull(iDECISION_APELACION)) entity.DECISION_APELACION = dr.GetString(iDECISION_APELACION);
                        if (!dr.IsDBNull(iRESPONSABLE_APELACION)) entity.RESPONSABLE_APELACION = dr.GetString(iRESPONSABLE_APELACION);
                        if (!dr.IsDBNull(iCOMENTARIOS_APELACION)) entity.COMENTARIOS_APELACION = dr.GetString(iCOMENTARIOS_APELACION);
                        if (!dr.IsDBNull(iCRITERIOS_APELACION)) entity.CRITERIOS_APELACION = dr.GetString(iCRITERIOS_APELACION);
                        //ETAPA4
                        if (!dr.IsDBNull(iEMPR_SOLI_ARBITRAJE)) entity.EMPR_SOLI_ARBITRAJE = dr.GetString(iEMPR_SOLI_ARBITRAJE);
                        if (!dr.IsDBNull(iARGUMENTO_ARBITRAJE)) entity.ARGUMENTO_ARBITRAJE = dr.GetString(iARGUMENTO_ARBITRAJE);
                        if (!dr.IsDBNull(iDECISION_ARBITRAJE)) entity.DECISION_ARBITRAJE = dr.GetString(iDECISION_ARBITRAJE);
                        if (!dr.IsDBNull(iRESPONSABLE_ARBITRAJE)) entity.RESPONSABLE_ARBITRAJE = dr.GetString(iRESPONSABLE_ARBITRAJE);
                        if (!dr.IsDBNull(iCOMENTARIOS_ARBITRAJE)) entity.COMENTARIOS_ARBITRAJE = dr.GetString(iCOMENTARIOS_ARBITRAJE);
                        if (!dr.IsDBNull(iCRITERIOS_ARBITRAJE)) entity.CRITERIOS_ARBITRAJE = dr.GetString(iCRITERIOS_ARBITRAJE);
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
        
        public string SqlObtenerComentarioXEventoyEtapa(int CREVENCODI, int CRETAPA)
        {
            string rpta = "";
            try
            {
                string query = string.Format(helper.SqlObtenerComentarioXEventoyEtapa,
                    CRETAPA, //0
                    CREVENCODI); //1
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        CrEtapaEventoDTO entity = new CrEtapaEventoDTO();
                        //EVENTO
                        int iCRCOMENTARIOS_RESPONSABLES = dr.GetOrdinal("CRCOMENTARIOS_RESPONSABLES");

                        //EVENTO
                        if (!dr.IsDBNull(iCRCOMENTARIOS_RESPONSABLES)) rpta = dr.GetString(iCRCOMENTARIOS_RESPONSABLES);
                    }
                }
            }
            catch (Exception ex)
            {
                rpta = null;
            }
            return rpta;
        }
        public List<Responsables> SqlObtenerEmpresaResponsable(int CREVENCODI, int CRETAPACODI)
        {
            List<Responsables> entitys = new List<Responsables>();
            try
            {
                string query = string.Format(helper.SqlObtenerEmpresaResponsable,
                    CREVENCODI, //0
                    CRETAPACODI); //1
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        Responsables entity = new Responsables();
                        //EVENTO
                        int iEMPRCODI = dr.GetOrdinal("EMPRCODI");
                        int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");

                        //EVENTO
                        if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = dr.GetInt32(iEMPRCODI);
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);
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
        public List<Solicitantes> SqlObtenerEmpresaSolicitante(int CREVENCODI, int CRETAPACODI)
        {
            List<Solicitantes> entitys = new List<Solicitantes>();
            try
            {
                string query = string.Format(helper.SqlObtenerEmpresaSolicitante,

                    CREVENCODI, //0
                    CRETAPACODI); //1

               DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        Solicitantes entity = new Solicitantes();
                        //EVENTO
                        int iCREVENCODI = dr.GetOrdinal("CREVENCODI");
                        int iCRETAPACODI = dr.GetOrdinal("CRETAPACODI");
                        int iEMPRCODI = dr.GetOrdinal("EMPRCODI");
                        int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");
                        int iCRARGUMENTO = dr.GetOrdinal("CRARGUMENTO");
                        int iCRDECISION = dr.GetOrdinal("CRDECISION");

                        //EVENTO
                        if (!dr.IsDBNull(iCREVENCODI)) entity.CREVENCODI = dr.GetInt32(iCREVENCODI);
                        if (!dr.IsDBNull(iCRETAPACODI)) entity.CRETAPACODI = dr.GetInt32(iCRETAPACODI);
                        if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = dr.GetInt32(iEMPRCODI);
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);
                        if (!dr.IsDBNull(iCRARGUMENTO)) entity.CRARGUMENTO = dr.GetString(iCRARGUMENTO);
                        if (!dr.IsDBNull(iCRDECISION)) entity.CRDECISION = dr.GetString(iCRDECISION);
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
        public List<CrCasosEspecialesDTO> ObtenerCasosEspeciales()
        {

            List<CrCasosEspecialesDTO> entitys = new List<CrCasosEspecialesDTO>();
            try
            {
                string query = helper.SqlObtenerCasosEspeciales;
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        CrCasosEspecialesDTO entity = new CrCasosEspecialesDTO();

                        int iCRESPECIALCODI = dr.GetOrdinal(helper.CRESPECIALCODI);
                        if (!dr.IsDBNull(iCRESPECIALCODI)) entity.CRESPECIALCODI = dr.GetInt32(iCRESPECIALCODI);

                        int iCREDESCRIPCION = dr.GetOrdinal(helper.CREDESCRIPCION);
                        if (!dr.IsDBNull(iCREDESCRIPCION)) entity.CREDESCRIPCION = dr.GetString(iCREDESCRIPCION);

                        entitys.Add(entity);
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex); ;
            }
            return entitys;
        }
        
        public List<CrCriteriosDTO> ObtenerCriterios()
        {

            List<CrCriteriosDTO> entitys = new List<CrCriteriosDTO>();
            try
            {
                string query = helper.SqlObtenerCriterios;
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        CrCriteriosDTO entity = new CrCriteriosDTO();

                        int iCRCRITERIOCODI = dr.GetOrdinal(helper.CRCRITERIOCODI);
                        if (!dr.IsDBNull(iCRCRITERIOCODI)) entity.CRCRITERIOCODI = dr.GetInt32(iCRCRITERIOCODI);

                        int iCREDESCRIPCION = dr.GetOrdinal(helper.CREDESCRIPCIONC);
                        if (!dr.IsDBNull(iCREDESCRIPCION)) entity.CREDESCRIPCION = dr.GetString(iCREDESCRIPCION);

                        entitys.Add(entity);
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex); ;
            }
            return entitys;
        }


        #region Casos Especiales
        public int SaveCasosEspeciales(CrCasosEspecialesDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxIdCasosEspeciales);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);
            command = dbProvider.GetSqlStringCommand(helper.SqlSaveCasosEspeciales);

            dbProvider.AddInParameter(command, helper.CRESPECIALCODI, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.CREDESCRIPCION, DbType.String, entity.CREDESCRIPCION);
            dbProvider.AddInParameter(command, helper.CREESTADO, DbType.String, entity.CREESTADO);
            dbProvider.AddInParameter(command, helper.LASTDATE, DbType.DateTime, entity.LASTDATE);
            dbProvider.AddInParameter(command, helper.LASTUSER, DbType.String, entity.LASTUSER);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int ValidarCasosEspeciales(int crespecialcodi)
        {
            string query = string.Format(helper.SqlValidarCasosEspeciales, crespecialcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            return int.Parse(dbProvider.ExecuteScalar(command).ToString());
        }

        public void UpdateCasosEspeciales(CrCasosEspecialesDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateCasosEspeciales);

            dbProvider.AddInParameter(command, helper.CREDESCRIPCION, DbType.String, entity.CREDESCRIPCION);
            dbProvider.AddInParameter(command, helper.CREESTADO, DbType.String, entity.CREESTADO);
            dbProvider.AddInParameter(command, helper.LASTDATE, DbType.DateTime, entity.LASTDATE);
            dbProvider.AddInParameter(command, helper.LASTUSER, DbType.String, entity.LASTUSER);
            dbProvider.AddInParameter(command, helper.CRESPECIALCODI, DbType.Int32, entity.CRESPECIALCODI);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteCasosEspeciales(int CRESPECIALCODI)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteCasosEspeciales);

            dbProvider.AddInParameter(command, helper.CRESPECIALCODI, DbType.Int32, CRESPECIALCODI);
            dbProvider.ExecuteNonQuery(command);
        }

        public CrCasosEspecialesDTO GetByIdCasosEspeciales(int CRESPECIALCODI)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdCasosEspeciales);

            dbProvider.AddInParameter(command, helper.CRESPECIALCODI, DbType.Int32, CRESPECIALCODI);
            CrCasosEspecialesDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.CreateCasosEspeciales(dr);
                }
            }

            return entity;
        }

        public List<CrCasosEspecialesDTO> ListCasosEspeciales()
        {
            List<CrCasosEspecialesDTO> entitys = new List<CrCasosEspecialesDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListCasosEspeciales);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateCasosEspeciales(dr));
                }
            }


            return entitys;
        }
        #endregion

        #region Criterios
        public int SaveCriterios(CrCriteriosDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxIdCriterios);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSaveCriterios);

            dbProvider.AddInParameter(command, helper.CRESPECIALCODI, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.CREDESCRIPCIONC, DbType.String, entity.CREDESCRIPCION);
            dbProvider.AddInParameter(command, helper.CREESTADOC, DbType.String, entity.CREESTADO);
            dbProvider.AddInParameter(command, helper.LASTDATEC, DbType.DateTime, entity.LASTDATE);
            dbProvider.AddInParameter(command, helper.LASTUSERC, DbType.String, entity.LASTUSER);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }
        public int ValidarCriterios(int crcriteriocodi)
        {
            string query = string.Format(helper.SqlValidarCriterios, crcriteriocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            return int.Parse(dbProvider.ExecuteScalar(command).ToString());
        }
        public void UpdateCriterios(CrCriteriosDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateCriterios);

            dbProvider.AddInParameter(command, helper.CREDESCRIPCIONC, DbType.String, entity.CREDESCRIPCION);
            dbProvider.AddInParameter(command, helper.CREESTADOC, DbType.String, entity.CREESTADO);
            dbProvider.AddInParameter(command, helper.LASTDATEC, DbType.DateTime, entity.LASTDATE);
            dbProvider.AddInParameter(command, helper.LASTUSERC, DbType.String, entity.LASTUSER);
            dbProvider.AddInParameter(command, helper.CRCRITERIOCODI, DbType.Int32, entity.CRCRITERIOCODI);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteCriterios(int CRCRITERIOCODI)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteCriterios);

            dbProvider.AddInParameter(command, helper.CRCRITERIOCODI, DbType.Int32, CRCRITERIOCODI);
            dbProvider.ExecuteNonQuery(command);
        }

        public CrCriteriosDTO GetByIdCriterios(int CRCRITERIOCODI)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdCriterios);

            dbProvider.AddInParameter(command, helper.CRCRITERIOCODI, DbType.Int32, CRCRITERIOCODI);
            CrCriteriosDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.CreateCriterios(dr);
                }
            }

            return entity;
        }

        public List<CrCriteriosDTO> ListCriterios()
        {
            List<CrCriteriosDTO> entitys = new List<CrCriteriosDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListCriterios);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateCriterios(dr));
                }
            }


            return entitys;
        }
        #endregion

    }
}
