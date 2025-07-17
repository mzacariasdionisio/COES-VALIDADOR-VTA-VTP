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
    /// Clase de acceso a datos de la tabla CO_VERSION
    /// </summary>
    public class CoVersionRepository: RepositoryBase, ICoVersionRepository
    {
        public CoVersionRepository(string strConn): base(strConn)
        {
        }

        CoVersionHelper helper = new CoVersionHelper();

        public int Save(CoVersionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, entity.Copercodi);
            dbProvider.AddInParameter(command, helper.Coverdesc, DbType.String, entity.Coverdesc);
            dbProvider.AddInParameter(command, helper.Coverfecinicio, DbType.DateTime, entity.Coverfecinicio);
            dbProvider.AddInParameter(command, helper.Coverfecfin, DbType.DateTime, entity.Coverfecfin);
            dbProvider.AddInParameter(command, helper.Coverestado, DbType.String, entity.Coverestado);
            dbProvider.AddInParameter(command, helper.Covertipo, DbType.String, entity.Covertipo);
            dbProvider.AddInParameter(command, helper.Covebacodi, DbType.Int32, entity.Covebacodi);
            dbProvider.AddInParameter(command, helper.Coverusucreacion, DbType.String, entity.Coverusucreacion);
            dbProvider.AddInParameter(command, helper.Coverfeccreacion, DbType.DateTime, entity.Coverfeccreacion);
            dbProvider.AddInParameter(command, helper.Coverusumodificacion, DbType.String, entity.Coverusumodificacion);
            dbProvider.AddInParameter(command, helper.Coverfecmodificacion, DbType.DateTime, entity.Coverfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CoVersionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, entity.Copercodi);
            dbProvider.AddInParameter(command, helper.Coverdesc, DbType.String, entity.Coverdesc);
            dbProvider.AddInParameter(command, helper.Coverfecinicio, DbType.DateTime, entity.Coverfecinicio);
            dbProvider.AddInParameter(command, helper.Coverfecfin, DbType.DateTime, entity.Coverfecfin);
            dbProvider.AddInParameter(command, helper.Coverestado, DbType.String, entity.Coverestado);
            dbProvider.AddInParameter(command, helper.Covertipo, DbType.String, entity.Covertipo);
            dbProvider.AddInParameter(command, helper.Covebacodi, DbType.Int32, entity.Covebacodi);
            dbProvider.AddInParameter(command, helper.Coverusucreacion, DbType.String, entity.Coverusucreacion);
            dbProvider.AddInParameter(command, helper.Coverfeccreacion, DbType.DateTime, entity.Coverfeccreacion);
            dbProvider.AddInParameter(command, helper.Coverusumodificacion, DbType.String, entity.Coverusumodificacion);
            dbProvider.AddInParameter(command, helper.Coverfecmodificacion, DbType.DateTime, entity.Coverfecmodificacion);
            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, entity.Covercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int covercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, covercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoVersionDTO GetById(int covercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, covercodi);
            CoVersionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoVersionDTO> List()
        {
            List<CoVersionDTO> entitys = new List<CoVersionDTO>();
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

        public List<CoVersionDTO> GetByCriteria(int idPeriodo)
        {
            List<CoVersionDTO> entitys = new List<CoVersionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, idPeriodo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoVersionDTO entity = helper.Create(dr);

                    int iDestipo = dr.GetOrdinal(helper.Destipo);
                    if (!dr.IsDBNull(iDestipo)) entity.Destipo = dr.GetString(iDestipo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public CoVersionDTO ObtenerVersionPorFecha(DateTime fecha)
        {
            string sql = string.Format(helper.SqlObtenerVersionPorFecha, fecha.Year, fecha.Month);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            CoVersionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoVersionDTO> GetVersionesPorMes(int mes, int anio)
        {
            List<CoVersionDTO> entitys = new List<CoVersionDTO>();
           
            string sql = string.Format(helper.SqlGetVersionesPorMes, mes, anio);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoVersionDTO entity = helper.Create(dr);

                    int iCoperestado = dr.GetOrdinal(helper.Coperestado);
                    if (!dr.IsDBNull(iCoperestado)) entity.Coperestado = dr.GetString(iCoperestado);
                    
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        
    }
}
