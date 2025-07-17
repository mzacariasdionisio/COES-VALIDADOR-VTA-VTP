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
    /// Clase de acceso a datos de la tabla RTU_ROLTURNO
    /// </summary>
    public class RtuRolturnoRepository : RepositoryBase, IRtuRolturnoRepository
    {
        public RtuRolturnoRepository(string strConn) : base(strConn)
        {
        }

        RtuRolturnoHelper helper = new RtuRolturnoHelper();

        public int Save(RtuRolturnoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rturolcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rturolusucreacion, DbType.String, entity.Rturolusucreacion);
            dbProvider.AddInParameter(command, helper.Rturolfeccreacion, DbType.DateTime, entity.Rturolfeccreacion);
            dbProvider.AddInParameter(command, helper.Rturolusumodificacion, DbType.String, entity.Rturolusumodificacion);
            dbProvider.AddInParameter(command, helper.Rturolfecmodificacion, DbType.DateTime, entity.Rturolfecmodificacion);
            dbProvider.AddInParameter(command, helper.Rturolanio, DbType.Int32, entity.Rturolanio);
            dbProvider.AddInParameter(command, helper.Rturolmes, DbType.Int32, entity.Rturolmes);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RtuRolturnoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rturolusucreacion, DbType.String, entity.Rturolusucreacion);
            dbProvider.AddInParameter(command, helper.Rturolfeccreacion, DbType.DateTime, entity.Rturolfeccreacion);
            dbProvider.AddInParameter(command, helper.Rturolusumodificacion, DbType.String, entity.Rturolusumodificacion);
            dbProvider.AddInParameter(command, helper.Rturolfecmodificacion, DbType.DateTime, entity.Rturolfecmodificacion);
            dbProvider.AddInParameter(command, helper.Rturolanio, DbType.Int32, entity.Rturolanio);
            dbProvider.AddInParameter(command, helper.Rturolmes, DbType.Int32, entity.Rturolmes);
            dbProvider.AddInParameter(command, helper.Rturolcodi, DbType.Int32, entity.Rturolcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rturolcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rturolcodi, DbType.Int32, rturolcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RtuRolturnoDTO GetById(int anio, int mes)
        {
            string sql = string.Format(helper.SqlGetById, anio, mes);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            RtuRolturnoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RtuRolturnoDTO> List()
        {
            List<RtuRolturnoDTO> entitys = new List<RtuRolturnoDTO>();
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

        public List<RtuRolturnoDTO> ObtenerEstructura(int anio, int mes)
        {
            List<RtuRolturnoDTO> entitys = new List<RtuRolturnoDTO>();
            string sql = string.Format(helper.SqlObtenerEstructura, anio, mes);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RtuRolturnoDTO entity = helper.Create(dr);

                    int iRtunrodia = dr.GetOrdinal(helper.Rtunrodia);
                    if (!dr.IsDBNull(iRtunrodia)) entity.Rtunrodia = Convert.ToInt32(dr.GetValue(iRtunrodia));

                    int iRtumodtrabajo = dr.GetOrdinal(helper.Rtumodtrabajo);
                    if (!dr.IsDBNull(iRtumodtrabajo)) entity.Rtumodtrabajo = dr.GetString(iRtumodtrabajo);

                    int iPercodi = dr.GetOrdinal(helper.Percodi);
                    if (!dr.IsDBNull(iPercodi)) entity.Percodi = Convert.ToInt32(dr.GetValue(iPercodi));

                    int iActcodi = dr.GetOrdinal(helper.Actcodi);
                    if (!dr.IsDBNull(iActcodi)) entity.Actcodi = Convert.ToInt32(dr.GetValue(iActcodi));

                    int iActnombre = dr.GetOrdinal(helper.Actnombre);
                    if (!dr.IsDBNull(iActnombre)) entity.Actnombre = dr.GetString(iActnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RtuRolturnoDTO> ObtenerDatosPorFecha(DateTime fecha, int tipoReporte)
        {
            List<RtuRolturnoDTO> entitys = new List<RtuRolturnoDTO>();
            string sql = string.Format(helper.SqlObtenerDatosPorFecha, fecha.Year, fecha.Month, fecha.Day, tipoReporte);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RtuRolturnoDTO entity = new RtuRolturnoDTO();

                    int iPercodi = dr.GetOrdinal(helper.Percodi);
                    if (!dr.IsDBNull(iPercodi)) entity.Percodi = Convert.ToInt32(dr.GetValue(iPercodi));

                    int iPernombre = dr.GetOrdinal(helper.Pernombre);
                    if (!dr.IsDBNull(iPernombre)) entity.Pernombre = dr.GetString(iPernombre);

                    int iActnombre = dr.GetOrdinal(helper.Actnombre);
                    if (!dr.IsDBNull(iActnombre)) entity.Actnombre = dr.GetString(iActnombre);

                    int iRtuactabreviatura = dr.GetOrdinal(helper.Rtuactabreviatura);
                    if (!dr.IsDBNull(iRtuactabreviatura)) entity.Rtuactabreviatura = dr.GetString(iRtuactabreviatura);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
