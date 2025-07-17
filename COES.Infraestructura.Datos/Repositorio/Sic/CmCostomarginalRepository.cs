using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CM_COSTOMARGINAL
    /// </summary>
    public class CmCostomarginalRepository: RepositoryBase, ICmCostomarginalRepository
    {
        public CmCostomarginalRepository(string strConn): base(strConn)
        {
        }

        CmCostomarginalHelper helper = new CmCostomarginalHelper();

        public int Save(CmCostomarginalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cmgncodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cnfbarcodi, DbType.Int32, entity.Cnfbarcodi);
            dbProvider.AddInParameter(command, helper.Cmgnenergia, DbType.Decimal, entity.Cmgnenergia);
            dbProvider.AddInParameter(command, helper.Cmgncongestion, DbType.Decimal, entity.Cmgncongestion);
            dbProvider.AddInParameter(command, helper.Cmgntotal, DbType.Decimal, entity.Cmgntotal);
            dbProvider.AddInParameter(command, helper.Cmgncorrelativo, DbType.Int32, entity.Cmgncorrelativo);
            dbProvider.AddInParameter(command, helper.Cmgnfecha, DbType.DateTime, entity.Cmgnfecha);
            dbProvider.AddInParameter(command, helper.Cmgnusucreacion, DbType.String, entity.Cmgnusucreacion);
            dbProvider.AddInParameter(command, helper.Cmgnfeccreacion, DbType.DateTime, entity.Cmgnfeccreacion);
            dbProvider.AddInParameter(command, helper.Cmgndemanda, DbType.Decimal, entity.Cmgndemanda);
            dbProvider.AddInParameter(command, helper.Cmgnreproceso, DbType.String, entity.Cmgnreproceso);
            dbProvider.AddInParameter(command, helper.Cmgnoperativo, DbType.Int32, entity.Cmgnoperativo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmCostomarginalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cnfbarcodi, DbType.Int32, entity.Cnfbarcodi);
            dbProvider.AddInParameter(command, helper.Cmgnenergia, DbType.Decimal, entity.Cmgnenergia);
            dbProvider.AddInParameter(command, helper.Cmgncongestion, DbType.Decimal, entity.Cmgncongestion);
            dbProvider.AddInParameter(command, helper.Cmgntotal, DbType.Decimal, entity.Cmgntotal);
            dbProvider.AddInParameter(command, helper.Cmgncorrelativo, DbType.Int32, entity.Cmgncorrelativo);
            dbProvider.AddInParameter(command, helper.Cmgnfecha, DbType.DateTime, entity.Cmgnfecha);
            dbProvider.AddInParameter(command, helper.Cmgnusucreacion, DbType.String, entity.Cmgnusucreacion);
            dbProvider.AddInParameter(command, helper.Cmgnfeccreacion, DbType.DateTime, entity.Cmgnfeccreacion);
            dbProvider.AddInParameter(command, helper.Cmgnoperativo, DbType.Int32, entity.Cmgnoperativo);
            dbProvider.AddInParameter(command, helper.Cmgncodi, DbType.Int32, entity.Cmgncodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cmgncodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cmgncodi, DbType.Int32, cmgncodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmCostomarginalDTO GetById(int cmgncodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cmgncodi, DbType.Int32, cmgncodi);
            CmCostomarginalDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmCostomarginalDTO> List()
        {
            List<CmCostomarginalDTO> entitys = new List<CmCostomarginalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CmCostomarginalDTO> GetByCriteria()
        {
            List<CmCostomarginalDTO> entitys = new List<CmCostomarginalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public int ObtenerMaxCorrelativo()
        {
            int correlativo = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerCorrelativo);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                correlativo = Convert.ToInt32(result) + 1;

                command = dbProvider.GetSqlStringCommand(helper.SqlActualizarCorrelativo);
                dbProvider.AddInParameter(command, helper.Cmgncorrelativo, DbType.Int32, correlativo);
                dbProvider.ExecuteNonQuery(command);
            }

            return correlativo;
        }    

        public List<CmCostomarginalDTO> ObtenerResultadoCostoMarginal(DateTime fecha)
        {
            List<CmCostomarginalDTO> entitys = new List<CmCostomarginalDTO>();
            string query = string.Format(helper.SqlObtenerResultadoCostoMarginal, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmCostomarginalDTO entity = new CmCostomarginalDTO();

                    int iCmgncorrelativo = dr.GetOrdinal(helper.Cmgncorrelativo);
                    if (!dr.IsDBNull(iCmgncorrelativo)) entity.Cmgncorrelativo = Convert.ToInt32(dr.GetValue(iCmgncorrelativo));

                    int iCmgnfecha = dr.GetOrdinal(helper.Cmgnfecha);
                    if (!dr.IsDBNull(iCmgnfecha)) entity.Cmgnfecha = dr.GetDateTime(iCmgnfecha);

                    int iCmgnfeccreacion = dr.GetOrdinal(helper.Cmgnfeccreacion);
                    if (!dr.IsDBNull(iCmgnfeccreacion)) entity.Cmgnfeccreacion = dr.GetDateTime(iCmgnfeccreacion);

                    int iCmgnusucreacion = dr.GetOrdinal(helper.Cmgnusucreacion);
                    if (!dr.IsDBNull(iCmgnusucreacion)) entity.Cmgnusucreacion = dr.GetString(iCmgnusucreacion);

                    int iTipoestimador = dr.GetOrdinal(helper.Tipoestimador);
                    if (!dr.IsDBNull(iTipoestimador)) entity.TipoEstimador = dr.GetString(iTipoestimador);

                    int iTipoproceso = dr.GetOrdinal(helper.Tipoproceso);
                    if (!dr.IsDBNull(iTipoproceso)) entity.TipoProceso = dr.GetString(iTipoproceso);

                    int iVersionpdo = dr.GetOrdinal(helper.Versionpdo);
                    if (!dr.IsDBNull(iVersionpdo)) entity.VersionPDO = dr.GetString(iVersionpdo);

                    int iCmveprversion = dr.GetOrdinal(helper.Cmveprversion);
                    if (!dr.IsDBNull(iCmveprversion)) entity.Cmveprversion = Convert.ToInt32(dr.GetValue(iCmveprversion));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CmCostomarginalDTO> ObtenerResultadoCostoMarginalExtranet(DateTime fecha)
        {
            List<CmCostomarginalDTO> entitys = new List<CmCostomarginalDTO>();
            string query = string.Format(helper.SqlObtenerResultadoCostoMarginalExtranet, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmCostomarginalDTO entity = new CmCostomarginalDTO();

                    int iCmgncorrelativo = dr.GetOrdinal(helper.Cmgncorrelativo);
                    if (!dr.IsDBNull(iCmgncorrelativo)) entity.Cmgncorrelativo = Convert.ToInt32(dr.GetValue(iCmgncorrelativo));

                    int iCmgnfecha = dr.GetOrdinal(helper.Cmgnfecha);
                    if (!dr.IsDBNull(iCmgnfecha)) entity.Cmgnfecha = dr.GetDateTime(iCmgnfecha);

                    int iCmgnfeccreacion = dr.GetOrdinal(helper.Cmgnfeccreacion);
                    if (!dr.IsDBNull(iCmgnfeccreacion)) entity.Cmgnfeccreacion = dr.GetDateTime(iCmgnfeccreacion);

                    int iCmgnusucreacion = dr.GetOrdinal(helper.Cmgnusucreacion);
                    if (!dr.IsDBNull(iCmgnusucreacion)) entity.Cmgnusucreacion = dr.GetString(iCmgnusucreacion);

                    int iTipoestimador = dr.GetOrdinal(helper.Tipoestimador);
                    if (!dr.IsDBNull(iTipoestimador)) entity.TipoEstimador = dr.GetString(iTipoestimador);

                    int iTipoproceso = dr.GetOrdinal(helper.Tipoproceso);
                    if (!dr.IsDBNull(iTipoproceso)) entity.TipoProceso = dr.GetString(iTipoproceso);

                    int iVersionpdo = dr.GetOrdinal(helper.Versionpdo);
                    if (!dr.IsDBNull(iVersionpdo)) entity.VersionPDO = dr.GetString(iVersionpdo);

                    int iCmveprversion = dr.GetOrdinal(helper.Cmveprversion);
                    if (!dr.IsDBNull(iCmveprversion)) entity.Cmveprversion = Convert.ToInt32(dr.GetValue(iCmveprversion));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public List<CmCostomarginalDTO> ObtenerResultadoCostoMarginalWeb(DateTime fecha, int version)
        {
            List<CmCostomarginalDTO> entitys = new List<CmCostomarginalDTO>();
            string query = string.Format(helper.SqlObtenerResultadoCostoMarginalWeb, 
                fecha.ToString(ConstantesBase.FormatoFecha), version);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmCostomarginalDTO entity = new CmCostomarginalDTO();

                    int iCmgncorrelativo = dr.GetOrdinal(helper.Cmgncorrelativo);
                    if (!dr.IsDBNull(iCmgncorrelativo)) entity.Cmgncorrelativo = Convert.ToInt32(dr.GetValue(iCmgncorrelativo));

                    int iCmgnfecha = dr.GetOrdinal(helper.Cmgnfecha);
                    if (!dr.IsDBNull(iCmgnfecha)) entity.Cmgnfecha = dr.GetDateTime(iCmgnfecha);

                    int iCmgnfeccreacion = dr.GetOrdinal(helper.Cmgnfeccreacion);
                    if (!dr.IsDBNull(iCmgnfeccreacion)) entity.Cmgnfeccreacion = dr.GetDateTime(iCmgnfeccreacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CmCostomarginalDTO> ObtenerDatosCostoMarginalCorrida(int correlativo)
        {
            List<CmCostomarginalDTO> entitys = new List<CmCostomarginalDTO>();
            string query = string.Format(helper.SqlObtenerDatosCostoMarginalCorrida, correlativo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmCostomarginalDTO entity = helper.Create(dr);

                    int iCnfbarnodo = dr.GetOrdinal(helper.Cnfbarnodo);
                    if (!dr.IsDBNull(iCnfbarnodo)) entity.Cnfbarnodo = dr.GetString(iCnfbarnodo);

                    int iCnfbarnombre = dr.GetOrdinal(helper.Cnfbarnombre);
                    if (!dr.IsDBNull(iCnfbarnombre)) entity.Cnfbarnombre = dr.GetString(iCnfbarnombre);

                    int iCnfbarcoorx = dr.GetOrdinal(helper.Cnfbarcoorx);
                    if (!dr.IsDBNull(iCnfbarcoorx)) entity.Cnfbarcoorx = dr.GetString(iCnfbarcoorx);

                    int iCnfbarcoory = dr.GetOrdinal(helper.Cnfbarcoory);
                    if (!dr.IsDBNull(iCnfbarcoory)) entity.Cnfbarcoory = dr.GetString(iCnfbarcoory);

                    int iCnfbarindpublicacion = dr.GetOrdinal(helper.Cnfbarindpublicacion);
                    if (!dr.IsDBNull(iCnfbarindpublicacion)) entity.Cnfbarindpublicacion = dr.GetString(iCnfbarindpublicacion);

                    int iCnfbardefecto = dr.GetOrdinal(helper.Cnfbardefecto);
                    if (!dr.IsDBNull(iCnfbardefecto)) entity.IndDefecto = dr.GetString(iCnfbardefecto);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CmCostomarginalDTO> ObtenerReporteCostosMarginales(DateTime fechaInicio, DateTime fechaFin, string estimador, string fuentepd,
            int version)
        {
            List<CmCostomarginalDTO> entitys = new List<CmCostomarginalDTO>();
            string query = string.Format(helper.SqlObtenerReporteCostosMarginales, 
                fechaInicio.ToString(ConstantesBase.FormatoFecha), 
                fechaFin.ToString(ConstantesBase.FormatoFecha), estimador, fuentepd, version);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmCostomarginalDTO entity = helper.Create(dr);

                    int iCnfbarnodo = dr.GetOrdinal(helper.Cnfbarnodo);
                    if (!dr.IsDBNull(iCnfbarnodo)) entity.Cnfbarnodo = dr.GetString(iCnfbarnodo);

                    int iCnfbarnombre = dr.GetOrdinal(helper.Cnfbarnombre);
                    if (!dr.IsDBNull(iCnfbarnombre)) entity.Cnfbarnombre = dr.GetString(iCnfbarnombre);

                    int iCnfbarcoorx = dr.GetOrdinal(helper.Cnfbarcoorx);
                    if (!dr.IsDBNull(iCnfbarcoorx)) entity.Cnfbarcoorx = dr.GetString(iCnfbarcoorx);

                    int iCnfbarcoory = dr.GetOrdinal(helper.Cnfbarcoory);
                    if (!dr.IsDBNull(iCnfbarcoory)) entity.Cnfbarcoory = dr.GetString(iCnfbarcoory);

                    int iCnfbarindpublicacion = dr.GetOrdinal(helper.Cnfbarindpublicacion);
                    if (!dr.IsDBNull(iCnfbarindpublicacion)) entity.Cnfbarindpublicacion = dr.GetString(iCnfbarindpublicacion);

                    int iCnfbardefecto = dr.GetOrdinal(helper.Cnfbardefecto);
                    if (!dr.IsDBNull(iCnfbardefecto)) entity.IndDefecto = dr.GetString(iCnfbardefecto);

                    int iTopnombre = dr.GetOrdinal(helper.Topnombre);
                    if (!dr.IsDBNull(iTopnombre)) entity.Topnombre = dr.GetString(iTopnombre);                    

                    int iTopcodi = dr.GetOrdinal(helper.Topcodi);
                    if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

                    if (entity.Cmgnfecha.Hour == 23 && entity.Cmgnfecha.Minute == 59)
                        entity.Periodo = 48;
                    else
                        entity.Periodo = entity.Cmgnfecha.Hour * 2 + (int)(entity.Cmgnfecha.Minute / 30);

                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        
        public List<CmCostomarginalDTO> ObtenerReporteCostosMarginalesTR(DateTime fechaInicio, DateTime fechaFin)
        {
            List<CmCostomarginalDTO> entitys = new List<CmCostomarginalDTO>();
            string query = string.Format(helper.SqlObtenerReporteCostosMarginalesTR, fechaInicio.ToString(ConstantesBase.FormatoFecha), 
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmCostomarginalDTO entity = helper.Create(dr);

                    int iCnfbarnodo = dr.GetOrdinal(helper.Cnfbarnodo);
                    if (!dr.IsDBNull(iCnfbarnodo)) entity.Cnfbarnodo = dr.GetString(iCnfbarnodo);

                    int iCnfbarnombre = dr.GetOrdinal(helper.Cnfbarnombre);
                    if (!dr.IsDBNull(iCnfbarnombre)) entity.Cnfbarnombre = dr.GetString(iCnfbarnombre);

                    int iCnfbarcoorx = dr.GetOrdinal(helper.Cnfbarcoorx);
                    if (!dr.IsDBNull(iCnfbarcoorx)) entity.Cnfbarcoorx = dr.GetString(iCnfbarcoorx);

                    int iCnfbarcoory = dr.GetOrdinal(helper.Cnfbarcoory);
                    if (!dr.IsDBNull(iCnfbarcoory)) entity.Cnfbarcoory = dr.GetString(iCnfbarcoory);

                    int iCnfbarindpublicacion = dr.GetOrdinal(helper.Cnfbarindpublicacion);
                    if (!dr.IsDBNull(iCnfbarindpublicacion)) entity.Cnfbarindpublicacion = dr.GetString(iCnfbarindpublicacion);

                    int iCnfbardefecto = dr.GetOrdinal(helper.Cnfbardefecto);
                    if (!dr.IsDBNull(iCnfbardefecto)) entity.IndDefecto = dr.GetString(iCnfbardefecto);

                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public List<CmCostomarginalDTO> ObtenerReporteCostosMarginalesWeb(DateTime fechaInicio, DateTime fechaFin, string defecto)
        {
            List<CmCostomarginalDTO> entitys = new List<CmCostomarginalDTO>();
            string query = string.Format(helper.SqlObtenerReporteCostosMarginalesWeb, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), defecto);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmCostomarginalDTO entity = helper.Create(dr);

                    int iCnfbarnodo = dr.GetOrdinal(helper.Cnfbarnodo);
                    if (!dr.IsDBNull(iCnfbarnodo)) entity.Cnfbarnodo = dr.GetString(iCnfbarnodo);

                    int iCnfbarnombre = dr.GetOrdinal(helper.Cnfbarnombre);
                    if (!dr.IsDBNull(iCnfbarnombre)) entity.Cnfbarnombre = dr.GetString(iCnfbarnombre);

                    int iCnfbarcoorx = dr.GetOrdinal(helper.Cnfbarcoorx);
                    if (!dr.IsDBNull(iCnfbarcoorx)) entity.Cnfbarcoorx = dr.GetString(iCnfbarcoorx);

                    int iCnfbarcoory = dr.GetOrdinal(helper.Cnfbarcoory);
                    if (!dr.IsDBNull(iCnfbarcoory)) entity.Cnfbarcoory = dr.GetString(iCnfbarcoory);

                    int iCnfbarindpublicacion = dr.GetOrdinal(helper.Cnfbarindpublicacion);
                    if (!dr.IsDBNull(iCnfbarindpublicacion)) entity.Cnfbarindpublicacion = dr.GetString(iCnfbarindpublicacion);

                    int iCnfbardefecto = dr.GetOrdinal(helper.Cnfbardefecto);
                    if (!dr.IsDBNull(iCnfbardefecto)) entity.IndDefecto = dr.GetString(iCnfbardefecto);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerIndicadorHora(int correlativo)
        {
            List<CmCostomarginalDTO> entitys = new List<CmCostomarginalDTO>();
            
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerIndicadorHora);
            dbProvider.AddInParameter(command, helper.Cmgncorrelativo, DbType.Int32, correlativo);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;        
        }

        public void GrabarRepresentativo(int correlativo, decimal representativo, DateTime fechaProceso)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGrabarRepresentativo);

            dbProvider.AddInParameter(command, helper.Cmgncorrelativo, DbType.Int32, correlativo);
            dbProvider.AddInParameter(command, helper.Cmgntotal, DbType.Decimal, representativo);
            dbProvider.AddInParameter(command, helper.Cmgnfecha, DbType.DateTime, fechaProceso);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmCostomarginalDTO ObtenerResumenCM()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerResumenCM);
            CmCostomarginalDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CmCostomarginalDTO();

                    int iCmgnfecha = dr.GetOrdinal(helper.Cmgnfecha);
                    if (!dr.IsDBNull(iCmgnfecha)) entity.Cmgnfecha = dr.GetDateTime(iCmgnfecha);

                    int iCmgncorrelativo = dr.GetOrdinal(helper.Cmgncorrelativo);
                    if (!dr.IsDBNull(iCmgncorrelativo)) entity.Cmgncorrelativo = Convert.ToInt32(dr.GetValue(iCmgncorrelativo));

                    int iCmgntotal = dr.GetOrdinal(helper.Cmgntotal);
                    if (!dr.IsDBNull(iCmgntotal)) entity.Cmgntotal = dr.GetDecimal(iCmgntotal);
                }     
            }
            return entity;
        }

        public void EliminarCorridaCostoMarginal(int correlativo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlEliminarCorridaCostoMarginal);
            dbProvider.AddInParameter(command, helper.Cmgncorrelativo, DbType.Int32, correlativo);

            dbProvider.ExecuteNonQuery(command);
           
        }

        public List<CmCostomarginalDTO> ObtenerDatosCostoMarginalXPeriodos(DateTime fecha)
        {
            List<CmCostomarginalDTO> entitys = new List<CmCostomarginalDTO>();
            string query = string.Format(helper.SqlObtenerDatosCostoMarginalXPeriodos, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmCostomarginalDTO entity = helper.Create(dr);

                    int iCnfbarnodo = dr.GetOrdinal(helper.Cnfbarnodo);
                    if (!dr.IsDBNull(iCnfbarnodo)) entity.Cnfbarnodo = dr.GetString(iCnfbarnodo);

                    int iCnfbarnombre = dr.GetOrdinal(helper.Cnfbarnombre);
                    if (!dr.IsDBNull(iCnfbarnombre)) entity.Cnfbarnombre = dr.GetString(iCnfbarnombre);

                    int iCnfbarcoorx = dr.GetOrdinal(helper.Cnfbarcoorx);
                    if (!dr.IsDBNull(iCnfbarcoorx)) entity.Cnfbarcoorx = dr.GetString(iCnfbarcoorx);

                    int iCnfbarcoory = dr.GetOrdinal(helper.Cnfbarcoory);
                    if (!dr.IsDBNull(iCnfbarcoory)) entity.Cnfbarcoory = dr.GetString(iCnfbarcoory);

                    int iCnfbarindpublicacion = dr.GetOrdinal(helper.Cnfbarindpublicacion);
                    if (!dr.IsDBNull(iCnfbarindpublicacion)) entity.Cnfbarindpublicacion = dr.GetString(iCnfbarindpublicacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region Mejoras CMgN

        public List<CmCostomarginalDTO> ObtenerComparativoCM(int cngbarcodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<CmCostomarginalDTO> entitys = new List<CmCostomarginalDTO>();

            string query = string.Format(helper.SqlObtenerComparativoCM, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), cngbarcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmCostomarginalDTO entity = new CmCostomarginalDTO();

                    int iCnfbarcodi = dr.GetOrdinal(helper.Cnfbarcodi);
                    if (!dr.IsDBNull(iCnfbarcodi)) entity.Cnfbarcodi = Convert.ToInt32(dr.GetValue(iCnfbarcodi));                    

                    int iCmgntotal = dr.GetOrdinal(helper.Cmgntotal);
                    if (!dr.IsDBNull(iCmgntotal)) entity.Cmgntotal = dr.GetDecimal(iCmgntotal);                  

                    int iCmgnfecha = dr.GetOrdinal(helper.Cmgnfecha);
                    if (!dr.IsDBNull(iCmgnfecha)) entity.Cmgnfecha = dr.GetDateTime(iCmgnfecha);                   

                    int iCmgnDemanda = dr.GetOrdinal(helper.Cmgndemanda);
                    if (!dr.IsDBNull(iCmgnDemanda)) entity.Cmgndemanda = dr.GetDecimal(iCmgnDemanda);

                    if (entity.Cmgnfecha.Hour == 23 && entity.Cmgnfecha.Minute == 59)
                        entity.Cmgncorrelativo = 48;
                    else
                        entity.Cmgncorrelativo = entity.Cmgnfecha.Hour * 2 + (int)(entity.Cmgnfecha.Minute / 30);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CmCostomarginalDTO> ObtenerUltimosProcesosCM(DateTime fecha)
        {
            List<CmCostomarginalDTO> entitys = new List<CmCostomarginalDTO>();
            string query = string.Format(helper.SqlObtenerUltimosProcesosCM, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmCostomarginalDTO entity = new CmCostomarginalDTO();

                    int iCmgncorrelativo = dr.GetOrdinal(helper.Cmgncorrelativo);
                    if (!dr.IsDBNull(iCmgncorrelativo)) entity.Cmgncorrelativo = Convert.ToInt32(dr.GetValue(iCmgncorrelativo));

                    int iCmgnfecha = dr.GetOrdinal(helper.Cmgnfecha);
                    if (!dr.IsDBNull(iCmgnfecha)) entity.Cmgnfecha = dr.GetDateTime(iCmgnfecha);

                    int iCmgnfeccreacion = dr.GetOrdinal(helper.Cmgnfeccreacion);
                    if (!dr.IsDBNull(iCmgnfeccreacion)) entity.Cmgnfeccreacion = dr.GetDateTime(iCmgnfeccreacion);

                    int iTipoestimador = dr.GetOrdinal(helper.Tipoestimador);
                    if (!dr.IsDBNull(iTipoestimador)) entity.TipoEstimador = dr.GetString(iTipoestimador);

                    int iVersionpdo = dr.GetOrdinal(helper.Versionpdo);
                    if (!dr.IsDBNull(iVersionpdo)) entity.VersionPDO = dr.GetString(iVersionpdo);

                    int iTopcodi = dr.GetOrdinal(helper.Topcodi);
                    if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

                    if (entity.Cmgnfecha.Hour == 23 && entity.Cmgnfecha.Minute == 59)
                        entity.Cmgncodi = 48;
                    else
                        entity.Cmgncodi = entity.Cmgnfecha.Hour * 2 + (int)(entity.Cmgnfecha.Minute / 30);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CmCostomarginalDTO> ObtenerUltimosProcesosCMPorVersion(DateTime fecha, int version)
        {
            List<CmCostomarginalDTO> entitys = new List<CmCostomarginalDTO>();
            string query = string.Format(helper.SqlObtenerUltimosProcesosCMPorVersion, fecha.ToString(ConstantesBase.FormatoFecha), version);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmCostomarginalDTO entity = new CmCostomarginalDTO();

                    int iCmgncorrelativo = dr.GetOrdinal(helper.Cmgncorrelativo);
                    if (!dr.IsDBNull(iCmgncorrelativo)) entity.Cmgncorrelativo = Convert.ToInt32(dr.GetValue(iCmgncorrelativo));

                    int iCmgnfecha = dr.GetOrdinal(helper.Cmgnfecha);
                    if (!dr.IsDBNull(iCmgnfecha)) entity.Cmgnfecha = dr.GetDateTime(iCmgnfecha);

                    int iCmgnfeccreacion = dr.GetOrdinal(helper.Cmgnfeccreacion);
                    if (!dr.IsDBNull(iCmgnfeccreacion)) entity.Cmgnfeccreacion = dr.GetDateTime(iCmgnfeccreacion);

                    int iTipoestimador = dr.GetOrdinal(helper.Tipoestimador);
                    if (!dr.IsDBNull(iTipoestimador)) entity.TipoEstimador = dr.GetString(iTipoestimador);

                    int iVersionpdo = dr.GetOrdinal(helper.Versionpdo);
                    if (!dr.IsDBNull(iVersionpdo)) entity.VersionPDO = dr.GetString(iVersionpdo);

                    int iTopcodi = dr.GetOrdinal(helper.Topcodi);
                    if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

                    if (entity.Cmgnfecha.Hour == 23 && entity.Cmgnfecha.Minute == 59)
                        entity.Cmgncodi = 48;
                    else
                        entity.Cmgncodi = entity.Cmgnfecha.Hour * 2 + (int)(entity.Cmgnfecha.Minute / 30);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

    }
}
