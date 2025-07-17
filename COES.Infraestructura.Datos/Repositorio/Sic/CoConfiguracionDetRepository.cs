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
    /// Clase de acceso a datos de la tabla CO_CONFIGURACION_DET
    /// </summary>
    public class CoConfiguracionDetRepository: RepositoryBase, ICoConfiguracionDetRepository
    {
        public CoConfiguracionDetRepository(string strConn): base(strConn)
        {
        }

        CoConfiguracionDetHelper helper = new CoConfiguracionDetHelper();

        public int Save(CoConfiguracionDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Courdecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Conurscodi, DbType.Int32, entity.Conurscodi);
            dbProvider.AddInParameter(command, helper.Courdetipo, DbType.String, entity.Courdetipo);
            dbProvider.AddInParameter(command, helper.Courdeoperacion, DbType.String, entity.Courdeoperacion);
            dbProvider.AddInParameter(command, helper.Courdereporte, DbType.String, entity.Courdereporte);
            dbProvider.AddInParameter(command, helper.Courdeequipo, DbType.String, entity.Courdeequipo);
            dbProvider.AddInParameter(command, helper.Courderequip, DbType.Decimal, entity.Courderequip);
            dbProvider.AddInParameter(command, helper.Courdevigenciadesde, DbType.DateTime, entity.Courdevigenciadesde);
            dbProvider.AddInParameter(command, helper.Courdevigenciahasta, DbType.DateTime, entity.Courdevigenciahasta);
            dbProvider.AddInParameter(command, helper.Courdeusucreacion, DbType.String, entity.Courdeusucreacion);
            dbProvider.AddInParameter(command, helper.Courdefeccreacion, DbType.DateTime, entity.Courdefeccreacion);
            dbProvider.AddInParameter(command, helper.Courdeusumodificacion, DbType.String, entity.Courdeusumodificacion);
            dbProvider.AddInParameter(command, helper.Courdefecmodificacion, DbType.DateTime, entity.Courdefecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CoConfiguracionDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
                        
            dbProvider.AddInParameter(command, helper.Courdetipo, DbType.String, entity.Courdetipo);
            dbProvider.AddInParameter(command, helper.Conurscodi, DbType.Int32, entity.Conurscodi);
            dbProvider.AddInParameter(command, helper.Courdeoperacion, DbType.String, entity.Courdeoperacion);
            dbProvider.AddInParameter(command, helper.Courdereporte, DbType.String, entity.Courdereporte);
            dbProvider.AddInParameter(command, helper.Courdeequipo, DbType.String, entity.Courdeequipo);
            dbProvider.AddInParameter(command, helper.Courderequip, DbType.Decimal, entity.Courderequip);
            dbProvider.AddInParameter(command, helper.Courdevigenciadesde, DbType.DateTime, entity.Courdevigenciadesde);
            dbProvider.AddInParameter(command, helper.Courdevigenciahasta, DbType.DateTime, entity.Courdevigenciahasta);
            dbProvider.AddInParameter(command, helper.Courdeusucreacion, DbType.String, entity.Courdeusucreacion);
            dbProvider.AddInParameter(command, helper.Courdefeccreacion, DbType.DateTime, entity.Courdefeccreacion);
            dbProvider.AddInParameter(command, helper.Courdeusumodificacion, DbType.String, entity.Courdeusumodificacion);
            dbProvider.AddInParameter(command, helper.Courdefecmodificacion, DbType.DateTime, entity.Courdefecmodificacion);
            dbProvider.AddInParameter(command, helper.Courdecodi, DbType.Int32, entity.Courdecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int courdecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Courdecodi, DbType.Int32, courdecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoConfiguracionDetDTO GetById(int courdecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Courdecodi, DbType.Int32, courdecodi);
            CoConfiguracionDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoConfiguracionDetDTO> List()
        {
            List<CoConfiguracionDetDTO> entitys = new List<CoConfiguracionDetDTO>();
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

        public List<CoConfiguracionDetDTO> GetByCriteria(int idConfiguracion)
        {
            List<CoConfiguracionDetDTO> entitys = new List<CoConfiguracionDetDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, idConfiguracion);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CoConfiguracionDetDTO> ObtenerConfiguracion(int idPeriodo, int idVersion)
        {
            List<CoConfiguracionDetDTO> entitys = new List<CoConfiguracionDetDTO>();
            
            string sql = string.Format(helper.SqlObtenerConfiguracion, idPeriodo, idVersion);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoConfiguracionDetDTO entity = helper.Create(dr);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iFecInicioHabilitacion = dr.GetOrdinal(helper.FecInicioHabilitacion);
                    if (!dr.IsDBNull(iFecInicioHabilitacion)) entity.FecInicioHabilitacion = dr.GetDateTime(iFecInicioHabilitacion);

                    int iFecFinHabilitacion = dr.GetOrdinal(helper.FecFinHabilitacion);
                    if (!dr.IsDBNull(iFecFinHabilitacion)) entity.FecFinHabilitacion = dr.GetDateTime(iFecFinHabilitacion);

                    int iContador = dr.GetOrdinal(helper.ContadorSenial);
                    if (!dr.IsDBNull(iContador)) entity.ContadorSenial = Convert.ToInt32(dr.GetValue(iContador));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CoConfiguracionDetDTO> ObtenerConfiguracionDetalle(string strFecha)
        {
            List<CoConfiguracionDetDTO> entitys = new List<CoConfiguracionDetDTO>();

            string sql = string.Format(helper.SqlObtenerInfoConfiguracionUrs, strFecha);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {                    
                    CoConfiguracionDetDTO entity = helper.Create(dr);

                    int iCoverdesc = dr.GetOrdinal(helper.Coverdesc);
                    if (!dr.IsDBNull(iCoverdesc)) entity.Coverdesc = dr.GetString(iCoverdesc);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iCopercodi = dr.GetOrdinal(helper.Copercodi);
                    if (!dr.IsDBNull(iCopercodi)) entity.Copercodi = Convert.ToInt32(dr.GetValue(iCopercodi));

                    int iCovercodi = dr.GetOrdinal(helper.Covercodi);
                    if (!dr.IsDBNull(iCovercodi)) entity.Covercodi = Convert.ToInt32(dr.GetValue(iCovercodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
