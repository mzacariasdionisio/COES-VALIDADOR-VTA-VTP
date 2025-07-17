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
    /// Clase de acceso a datos de la tabla SPO_VERSIONNUM
    /// </summary>
    public class SpoVersionnumRepository: RepositoryBase, ISpoVersionnumRepository
    {
        public SpoVersionnumRepository(string strConn): base(strConn)
        {
        }

        SpoVersionnumHelper helper = new SpoVersionnumHelper();

        public int Save(SpoVersionnumDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            string sqlQuery = string.Format(helper.SqlGetMaxIdVersion, entity.Numecodi, ((DateTime)entity.Vernfechaperiodo).ToString(ConstantesBase.FormatoFecha));
            command = dbProvider.GetSqlStringCommand(sqlQuery);
            result = dbProvider.ExecuteScalar(command);
            int idVer = 1;
            if (result != null) idVer = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Verncodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Numecodi, DbType.Int32, entity.Numecodi);
            dbProvider.AddInParameter(command, helper.Vernfechaperiodo, DbType.DateTime, entity.Vernfechaperiodo);
            dbProvider.AddInParameter(command, helper.Vernusucreacion, DbType.String, entity.Vernusucreacion);
            dbProvider.AddInParameter(command, helper.Vernestado, DbType.Int32, entity.Vernestado);
            dbProvider.AddInParameter(command, helper.Vernnro, DbType.Int32, idVer);
            dbProvider.AddInParameter(command, helper.Vernfeccreacion, DbType.DateTime, entity.Vernfeccreacion);
            dbProvider.AddInParameter(command, helper.Vernusumodificacion, DbType.String, entity.Vernusumodificacion);
            dbProvider.AddInParameter(command, helper.Vernfecmodificacion, DbType.DateTime, entity.Vernfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SpoVersionnumDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Verncodi, DbType.Int32, entity.Verncodi);
            dbProvider.AddInParameter(command, helper.Numecodi, DbType.Int32, entity.Numecodi);
            dbProvider.AddInParameter(command, helper.Vernfechaperiodo, DbType.DateTime, entity.Vernfechaperiodo);
            dbProvider.AddInParameter(command, helper.Vernusucreacion, DbType.String, entity.Vernusucreacion);
            dbProvider.AddInParameter(command, helper.Vernestado, DbType.Int32, entity.Vernestado);
            dbProvider.AddInParameter(command, helper.Vernnro, DbType.Int32, entity.Vernnro);
            dbProvider.AddInParameter(command, helper.Vernfeccreacion, DbType.DateTime, entity.Vernfeccreacion);
            dbProvider.AddInParameter(command, helper.Vernusumodificacion, DbType.String, entity.Vernusumodificacion);
            dbProvider.AddInParameter(command, helper.Vernfecmodificacion, DbType.DateTime, entity.Vernfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateEstado(SpoVersionnumDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateEstado);

            dbProvider.AddInParameter(command, helper.Vernestado, DbType.Int32, entity.Vernestado);
            dbProvider.AddInParameter(command, helper.Vernusumodificacion, DbType.String, entity.Vernusumodificacion);
            dbProvider.AddInParameter(command, helper.Vernfecmodificacion, DbType.DateTime, entity.Vernfecmodificacion);
            dbProvider.AddInParameter(command, helper.Verncodi, DbType.Int32, entity.Verncodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int verncodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Verncodi, DbType.Int32, verncodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SpoVersionnumDTO GetById(int verncodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Verncodi, DbType.Int32, verncodi);
            SpoVersionnumDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SpoVersionnumDTO> List()
        {
            List<SpoVersionnumDTO> entitys = new List<SpoVersionnumDTO>();
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

        public List<SpoVersionnumDTO> GetByCriteria(DateTime fecha, int numeral)
        {
            string ssqlQuery = string.Format(helper.SqlGetByCriteria, numeral, fecha.ToString(ConstantesBase.FormatoFecha));
            List<SpoVersionnumDTO> entitys = new List<SpoVersionnumDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(ssqlQuery);
            SpoVersionnumDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iNumhisabrev = dr.GetOrdinal(helper.Numhisabrev);
                    if (!dr.IsDBNull(iNumhisabrev)) entity.Numhisabrev = dr.GetString(iNumhisabrev);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
