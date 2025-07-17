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
    /// Clase de acceso a datos de la tabla RTU_CONFIGURACION
    /// </summary>
    public class RtuConfiguracionRepository : RepositoryBase, IRtuConfiguracionRepository
    {
        public RtuConfiguracionRepository(string strConn) : base(strConn)
        {
        }

        RtuConfiguracionHelper helper = new RtuConfiguracionHelper();

        public int Save(RtuConfiguracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rtuconcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rtuconanio, DbType.Int32, entity.Rtuconanio);
            dbProvider.AddInParameter(command, helper.Rtuconmes, DbType.Int32, entity.Rtuconmes);
            dbProvider.AddInParameter(command, helper.Rtuconusucreacion, DbType.String, entity.Rtuconusucreacion);
            dbProvider.AddInParameter(command, helper.Rtuconfeccreacion, DbType.DateTime, entity.Rtuconfeccreacion);
            dbProvider.AddInParameter(command, helper.Rtuconfecmodificacion, DbType.DateTime, entity.Rtuconfecmodificacion);
            dbProvider.AddInParameter(command, helper.Rtuconusumodificacion, DbType.String, entity.Rtuconusumodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RtuConfiguracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rtuconanio, DbType.Int32, entity.Rtuconanio);
            dbProvider.AddInParameter(command, helper.Rtuconmes, DbType.Int32, entity.Rtuconmes);
            dbProvider.AddInParameter(command, helper.Rtuconusucreacion, DbType.String, entity.Rtuconusucreacion);
            dbProvider.AddInParameter(command, helper.Rtuconfeccreacion, DbType.DateTime, entity.Rtuconfeccreacion);
            dbProvider.AddInParameter(command, helper.Rtuconfecmodificacion, DbType.DateTime, entity.Rtuconfecmodificacion);
            dbProvider.AddInParameter(command, helper.Rtuconusumodificacion, DbType.String, entity.Rtuconusumodificacion);
            dbProvider.AddInParameter(command, helper.Rtuconcodi, DbType.Int32, entity.Rtuconcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rtuconcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rtuconcodi, DbType.Int32, rtuconcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RtuConfiguracionDTO GetById(int rtuconcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rtuconcodi, DbType.Int32, rtuconcodi);
            RtuConfiguracionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public RtuConfiguracionDTO GetByAnioMes(int anio, int mes)
        {
            string sql = string.Format(helper.SqlGetByAnioMes, anio, mes);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            RtuConfiguracionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RtuConfiguracionDTO> List()
        {
            List<RtuConfiguracionDTO> entitys = new List<RtuConfiguracionDTO>();
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

        public List<RtuConfiguracionDTO> ObtenerConfguracion(int anio, int mes)
        {
            List<RtuConfiguracionDTO> entitys = new List<RtuConfiguracionDTO>();
            string sql = string.Format(helper.SqlObtenerConfiguracion, anio, mes);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RtuConfiguracionDTO entity = new RtuConfiguracionDTO();

                    int iPernomb = dr.GetOrdinal(helper.Pernomb);
                    if (!dr.IsDBNull(iPernomb)) entity.Pernomb = dr.GetString(iPernomb);

                    int iPercodi = dr.GetOrdinal(helper.Percodi);
                    if (!dr.IsDBNull(iPercodi)) entity.Percodi = Convert.ToInt32(dr.GetValue(iPercodi));

                    int iPerorden = dr.GetOrdinal(helper.Perorden);
                    if (!dr.IsDBNull(iPerorden)) entity.Perorden = Convert.ToInt32(dr.GetValue(iPerorden));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruptipo = dr.GetOrdinal(helper.Grupotipo);
                    if (!dr.IsDBNull(iGruptipo)) entity.Gruptipo = dr.GetString(iGruptipo);

                    int iGrupoorden = dr.GetOrdinal(helper.Grupoorden);
                    if (!dr.IsDBNull(iGrupoorden)) entity.Grupoorden = Convert.ToInt32(dr.GetValue(iGrupoorden));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RtuConfiguracionDTO> ObtenerConfiguracionReciente(int anio, int mes)
        {
            List<RtuConfiguracionDTO> entitys = new List<RtuConfiguracionDTO>();
            string sql = string.Format(helper.SqlObtenerConfiguracionReciente, anio, mes);
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
    }
}
