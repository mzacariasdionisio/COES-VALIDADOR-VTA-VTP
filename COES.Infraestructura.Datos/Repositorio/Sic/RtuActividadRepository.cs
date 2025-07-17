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
    /// Clase de acceso a datos de la tabla RTU_ACTIVIDAD
    /// </summary>
    public class RtuActividadRepository : RepositoryBase, IRtuActividadRepository
    {
        public RtuActividadRepository(string strConn) : base(strConn)
        {
        }

        RtuActividadHelper helper = new RtuActividadHelper();

        public int Save(RtuActividadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rtuactcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rtuactdescripcion, DbType.String, entity.Rtuactdescripcion);
            dbProvider.AddInParameter(command, helper.Rtuactabreviatura, DbType.String, entity.Rtuactabreviatura);
            dbProvider.AddInParameter(command, helper.Rtuactestado, DbType.String, entity.Rtuactestado);
            dbProvider.AddInParameter(command, helper.Rtuactreporte, DbType.String, entity.Rtuactreporte);
            dbProvider.AddInParameter(command, helper.Rturescodi, DbType.Int32, entity.Rturescodi);
            dbProvider.AddInParameter(command, helper.Rtuactusucreacion, DbType.String, entity.Rtuactusucreacion);
            dbProvider.AddInParameter(command, helper.Rtuactfeccreacion, DbType.DateTime, entity.Rtuactfeccreacion);
            dbProvider.AddInParameter(command, helper.Rtuactusumodificacion, DbType.String, entity.Rtuactusumodificacion);
            dbProvider.AddInParameter(command, helper.Rtuactfecmodificacion, DbType.DateTime, entity.Rtuactfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RtuActividadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rtuactdescripcion, DbType.String, entity.Rtuactdescripcion);
            dbProvider.AddInParameter(command, helper.Rtuactabreviatura, DbType.String, entity.Rtuactabreviatura);
            dbProvider.AddInParameter(command, helper.Rtuactestado, DbType.String, entity.Rtuactestado);
            dbProvider.AddInParameter(command, helper.Rtuactreporte, DbType.String, entity.Rtuactreporte);
            dbProvider.AddInParameter(command, helper.Rturescodi, DbType.Int32, entity.Rturescodi);
            dbProvider.AddInParameter(command, helper.Rtuactusumodificacion, DbType.String, entity.Rtuactusumodificacion);
            dbProvider.AddInParameter(command, helper.Rtuactfecmodificacion, DbType.DateTime, entity.Rtuactfecmodificacion);
            dbProvider.AddInParameter(command, helper.Rtuactcodi, DbType.Int32, entity.Rtuactcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rtuactcodi, string username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rtuactusumodificacion, DbType.String, username);
            dbProvider.AddInParameter(command, helper.Rtuactcodi, DbType.Int32, rtuactcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RtuActividadDTO GetById(int rtuactcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rtuactcodi, DbType.Int32, rtuactcodi);
            RtuActividadDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RtuActividadDTO> List()
        {
            List<RtuActividadDTO> entitys = new List<RtuActividadDTO>();
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

        public List<RtuActividadDTO> GetByCriteria()
        {
            List<RtuActividadDTO> entitys = new List<RtuActividadDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RtuActividadDTO entity = helper.Create(dr);

                    int iRturesdescripcion = dr.GetOrdinal(helper.Rturesdescripcion);
                    if (!dr.IsDBNull(iRturesdescripcion)) entity.Rturesdescripcion = dr.GetString(iRturesdescripcion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RtuActividadDTO> ObtenerTipoResponsables()
        {
            List<RtuActividadDTO> entitys = new List<RtuActividadDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerTipoResponsable);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RtuActividadDTO entity = new RtuActividadDTO();

                    int iRtuactcodi = dr.GetOrdinal(helper.Rtuactcodi);
                    if (!dr.IsDBNull(iRtuactcodi)) entity.Rtuactcodi = Convert.ToInt32(dr.GetValue(iRtuactcodi));

                    int iRtuactdescripcion = dr.GetOrdinal(helper.Rtuactdescripcion);
                    if (!dr.IsDBNull(iRtuactdescripcion)) entity.Rtuactdescripcion = dr.GetString(iRtuactdescripcion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RtuActividadDTO> ObtenerActividadesPorTipoInforme(int tipoReporte)
        {
            List<RtuActividadDTO> entitys = new List<RtuActividadDTO>();
            string sql = string.Format(helper.SqlObtenerActividadPorTipoInforme, tipoReporte);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RtuActividadDTO entity = helper.Create(dr);

                    int iRturesrol = dr.GetOrdinal(helper.Rturesrol);
                    if (!dr.IsDBNull(iRturesrol)) entity.Rturesrol = dr.GetString(iRturesrol);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
