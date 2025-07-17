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
    /// Clase de acceso a datos de la tabla RE_PERIODO
    /// </summary>
    public class RePeriodoRepository: RepositoryBase, IRePeriodoRepository
    {
        public RePeriodoRepository(string strConn): base(strConn)
        {
        }

        RePeriodoHelper helper = new RePeriodoHelper();

        public int Save(RePeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Reperanio, DbType.Int32, entity.Reperanio);
            dbProvider.AddInParameter(command, helper.Repertipo, DbType.String, entity.Repertipo);
            dbProvider.AddInParameter(command, helper.Repernombre, DbType.String, entity.Repernombre);
            dbProvider.AddInParameter(command, helper.Reperpadre, DbType.Int32, entity.Reperpadre);
            dbProvider.AddInParameter(command, helper.Reperrevision, DbType.String, entity.Reperrevision);
            dbProvider.AddInParameter(command, helper.Reperfecinicio, DbType.DateTime, entity.Reperfecinicio);
            dbProvider.AddInParameter(command, helper.Reperfecfin, DbType.DateTime, entity.Reperfecfin);
            dbProvider.AddInParameter(command, helper.Reperestado, DbType.String, entity.Reperestado);
            dbProvider.AddInParameter(command, helper.Reperorden, DbType.Int32, entity.Reperorden);
            dbProvider.AddInParameter(command, helper.Repertcambio, DbType.Decimal, entity.Repertcambio);
            dbProvider.AddInParameter(command, helper.Reperfactorcomp, DbType.Decimal, entity.Reperfactorcomp);
            dbProvider.AddInParameter(command, helper.Reperusucreacion, DbType.String, entity.Reperusucreacion);
            dbProvider.AddInParameter(command, helper.Reperfeccreacion, DbType.DateTime, entity.Reperfeccreacion);
            dbProvider.AddInParameter(command, helper.Reperusumodificacion, DbType.String, entity.Reperusumodificacion);
            dbProvider.AddInParameter(command, helper.Reperfecmodificacion, DbType.DateTime, entity.Reperfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RePeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Reperanio, DbType.Int32, entity.Reperanio);
            dbProvider.AddInParameter(command, helper.Repertipo, DbType.String, entity.Repertipo);
            dbProvider.AddInParameter(command, helper.Repernombre, DbType.String, entity.Repernombre);
            dbProvider.AddInParameter(command, helper.Reperpadre, DbType.Int32, entity.Reperpadre);
            dbProvider.AddInParameter(command, helper.Reperrevision, DbType.String, entity.Reperrevision);
            dbProvider.AddInParameter(command, helper.Reperfecinicio, DbType.DateTime, entity.Reperfecinicio);
            dbProvider.AddInParameter(command, helper.Reperfecfin, DbType.DateTime, entity.Reperfecfin);
            dbProvider.AddInParameter(command, helper.Reperestado, DbType.String, entity.Reperestado);
            dbProvider.AddInParameter(command, helper.Reperorden, DbType.Int32, entity.Reperorden);
            dbProvider.AddInParameter(command, helper.Repertcambio, DbType.Decimal, entity.Repertcambio);
            dbProvider.AddInParameter(command, helper.Reperfactorcomp, DbType.Decimal, entity.Reperfactorcomp);
            //dbProvider.AddInParameter(command, helper.Reperusucreacion, DbType.String, entity.Reperusucreacion);
            //dbProvider.AddInParameter(command, helper.Reperfeccreacion, DbType.DateTime, entity.Reperfeccreacion);
            dbProvider.AddInParameter(command, helper.Reperusumodificacion, DbType.String, entity.Reperusumodificacion);
            dbProvider.AddInParameter(command, helper.Reperfecmodificacion, DbType.DateTime, entity.Reperfecmodificacion);
            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int repercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, repercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RePeriodoDTO GetById(int repercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, repercodi);
            RePeriodoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RePeriodoDTO> List()
        {
            List<RePeriodoDTO> entitys = new List<RePeriodoDTO>();
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

        public List<RePeriodoDTO> GetByCriteria(int anioDesde, int anioHasta, string estado)
        {
            List<RePeriodoDTO> entitys = new List<RePeriodoDTO>();
            string query = string.Format(helper.SqlGetByCriteria, anioDesde, anioHasta, estado);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RePeriodoDTO entity = helper.Create(dr);

                    int iRepernombrepadre = dr.GetOrdinal(helper.Repernombrepadre);
                    if (!dr.IsDBNull(iRepernombrepadre)) entity.Repernombrepadre = dr.GetString(iRepernombrepadre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RePeriodoDTO> ObtenerPeriodosPadre(int anio)
        {
            List<RePeriodoDTO> entitys = new List<RePeriodoDTO>();
            string query = string.Format(helper.SqlObtenerPeriodosPadre, anio);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public int ValidarNombre(string nombre, int id)
        {
            string query = string.Format(helper.SqlValidarNombre, nombre, id);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object result = dbProvider.ExecuteScalar(command);
            int count = 0;
            if (result != null) count = Convert.ToInt32(result);
            return count;            
        }


        public List<RePeriodoDTO> ObtenerPeriodosCercanos(int anio, int id)
        {
            List<RePeriodoDTO> entitys = new List<RePeriodoDTO>();
            string query = string.Format(helper.SqlObtenerPeriodosCercanos, anio, id);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

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
