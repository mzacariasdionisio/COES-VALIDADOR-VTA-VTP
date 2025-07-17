using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla AF_SOLICITUD_RESP
    /// </summary>
    public class AfSolicitudRespRepository : RepositoryBase, IAfSolicitudRespRepository
    {
        public AfSolicitudRespRepository(string strConn) : base(strConn)
        {
        }

        AfSolicitudRespHelper helper = new AfSolicitudRespHelper();

        public int Save(AfSolicitudRespDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, entity.Enviocodi);
            dbProvider.AddInParameter(command, helper.Sorespcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Soresparchivootros, DbType.String, entity.Soresparchivootros);
            dbProvider.AddInParameter(command, helper.Soresparchivoinf, DbType.String, entity.Soresparchivoinf);
            dbProvider.AddInParameter(command, helper.Sorespobsarchivo, DbType.String, entity.Sorespobsarchivo);
            dbProvider.AddInParameter(command, helper.Sorespobs, DbType.String, entity.Sorespobs);
            dbProvider.AddInParameter(command, helper.Sorespusucreacion, DbType.String, entity.Sorespusucreacion);
            dbProvider.AddInParameter(command, helper.Sorespfeccreacion, DbType.DateTime, entity.Sorespfeccreacion);
            dbProvider.AddInParameter(command, helper.Sorespusumodificacion, DbType.String, entity.Sorespusumodificacion);
            dbProvider.AddInParameter(command, helper.Sorespfecmodificacion, DbType.DateTime, entity.Sorespfecmodificacion);
            dbProvider.AddInParameter(command, helper.Sorespestadosol, DbType.String, entity.Sorespestadosol);
            dbProvider.AddInParameter(command, helper.Sorespdesc, DbType.String, entity.Sorespdesc);
            dbProvider.AddInParameter(command, helper.Sorespfechaevento, DbType.DateTime, entity.Sorespfechaevento);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Update(AfSolicitudRespDTO entity)
        {
            int id = -1;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, entity.Enviocodi);
            dbProvider.AddInParameter(command, helper.Soresparchivootros, DbType.String, entity.Soresparchivootros);
            dbProvider.AddInParameter(command, helper.Soresparchivoinf, DbType.String, entity.Soresparchivoinf);
            dbProvider.AddInParameter(command, helper.Sorespobsarchivo, DbType.String, entity.Sorespobsarchivo);
            dbProvider.AddInParameter(command, helper.Sorespobs, DbType.String, entity.Sorespobs);
            dbProvider.AddInParameter(command, helper.Sorespusucreacion, DbType.String, entity.Sorespusucreacion);
            dbProvider.AddInParameter(command, helper.Sorespfeccreacion, DbType.DateTime, entity.Sorespfeccreacion);
            dbProvider.AddInParameter(command, helper.Sorespusumodificacion, DbType.String, entity.Sorespusumodificacion);
            dbProvider.AddInParameter(command, helper.Sorespfecmodificacion, DbType.DateTime, entity.Sorespfecmodificacion);
            dbProvider.AddInParameter(command, helper.Sorespestadosol, DbType.String, entity.Sorespestadosol);
            dbProvider.AddInParameter(command, helper.Sorespdesc, DbType.String, entity.Sorespdesc);
            dbProvider.AddInParameter(command, helper.Sorespfechaevento, DbType.DateTime, entity.Sorespfechaevento);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Sorespcodi, DbType.Int32, entity.Sorespcodi);

            id = dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Delete(int sorespcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Sorespcodi, DbType.Int32, sorespcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AfSolicitudRespDTO GetById(int sorespcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Sorespcodi, DbType.Int32, sorespcodi);
            AfSolicitudRespDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                }
            }

            return entity;
        }

        public List<AfSolicitudRespDTO> List()
        {
            List<AfSolicitudRespDTO> entitys = new List<AfSolicitudRespDTO>();
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

        public List<AfSolicitudRespDTO> GetByCriteria()
        {
            List<AfSolicitudRespDTO> entitys = new List<AfSolicitudRespDTO>();
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

        public List<AfSolicitudRespDTO> ListarSolicitudesxFiltro(AfSolicitudRespDTO solicitud)
        {
            var entitys = new List<AfSolicitudRespDTO>();
            string strComando = string.Format(helper.sqlListarSolicitudesxFiltro,
                solicitud.Empresa,
                solicitud.Sorespestadosol,
                solicitud.Di,
                solicitud.Df
                );

            DbCommand command = dbProvider.GetSqlStringCommand(strComando);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

    }
}
