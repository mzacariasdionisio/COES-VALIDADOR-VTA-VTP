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
    /// Clase de acceso a datos de la tabla AUD_ARCHIVO
    /// </summary>
    public class AudArchivoRepository: RepositoryBase, IAudArchivoRepository
    {
        public AudArchivoRepository(string strConn): base(strConn)
        {
        }

        AudArchivoHelper helper = new AudArchivoHelper();

        public int Save(AudArchivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Archcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Archnombre, DbType.String, entity.Archnombre);
            dbProvider.AddInParameter(command, helper.Archruta, DbType.String, entity.Archruta);
            dbProvider.AddInParameter(command, helper.Archactivo, DbType.String, entity.Archactivo);
            dbProvider.AddInParameter(command, helper.Archusucreacion, DbType.String, entity.Archusucreacion);
            dbProvider.AddInParameter(command, helper.Archfechacreacion, DbType.DateTime, entity.Archfechacreacion);
            dbProvider.AddInParameter(command, helper.Archusumodificacion, DbType.String, entity.Archusumodificacion);
            dbProvider.AddInParameter(command, helper.Archfechamodificacion, DbType.DateTime, entity.Archfechamodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AudArchivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Archnombre, DbType.String, entity.Archnombre);
            dbProvider.AddInParameter(command, helper.Archruta, DbType.String, entity.Archruta);
            dbProvider.AddInParameter(command, helper.Archactivo, DbType.String, entity.Archactivo);
            dbProvider.AddInParameter(command, helper.Archusucreacion, DbType.String, entity.Archusucreacion);
            dbProvider.AddInParameter(command, helper.Archfechacreacion, DbType.DateTime, entity.Archfechacreacion);
            dbProvider.AddInParameter(command, helper.Archusumodificacion, DbType.String, entity.Archusumodificacion);
            dbProvider.AddInParameter(command, helper.Archfechamodificacion, DbType.DateTime, entity.Archfechamodificacion);
            dbProvider.AddInParameter(command, helper.Archcodi, DbType.Int32, entity.Archcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int archcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Archcodi, DbType.Int32, archcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AudArchivoDTO GetById(int archcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Archcodi, DbType.Int32, archcodi);
            AudArchivoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AudArchivoDTO> List()
        {
            List<AudArchivoDTO> entitys = new List<AudArchivoDTO>();
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

        public List<AudArchivoDTO> GetByCriteria()
        {
            List<AudArchivoDTO> entitys = new List<AudArchivoDTO>();
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
    }
}
