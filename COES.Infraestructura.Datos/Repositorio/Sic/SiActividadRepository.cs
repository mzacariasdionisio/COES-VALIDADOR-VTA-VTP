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
    /// Clase de acceso a datos de la tabla SI_ACTIVIDAD
    /// </summary>
    public class SiActividadRepository : RepositoryBase, ISiActividadRepository
    {
        public SiActividadRepository(string strConn)
            : base(strConn)
        {
        }

        SiActividadHelper helper = new SiActividadHelper();

        public int Save(SiActividadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Actcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Actabrev, DbType.String, entity.Actabrev);
            dbProvider.AddInParameter(command, helper.Actnomb, DbType.String, entity.Actnomb);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiActividadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Actabrev, DbType.String, entity.Actabrev);
            dbProvider.AddInParameter(command, helper.Actnomb, DbType.String, entity.Actnomb);
            dbProvider.AddInParameter(command, helper.Actcodi, DbType.Int32, entity.Actcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int actcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Actcodi, DbType.Int32, actcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiActividadDTO GetById(int actcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Actcodi, DbType.Int32, actcodi);
            SiActividadDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiActividadDTO> List()
        {
            List<SiActividadDTO> entitys = new List<SiActividadDTO>();
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

        public List<SiActividadDTO> GetByCriteria()
        {
            List<SiActividadDTO> entitys = new List<SiActividadDTO>();
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

        public List<SiActividadDTO> GetListaActividadesPersonal(string areacodi)
        {
            List<SiActividadDTO> entitys = new List<SiActividadDTO>();
            string query = string.Format(helper.SqlGetListaActividadesPersonal, areacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiActividadDTO entity = new SiActividadDTO();

                    entity = helper.Create(dr);

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
