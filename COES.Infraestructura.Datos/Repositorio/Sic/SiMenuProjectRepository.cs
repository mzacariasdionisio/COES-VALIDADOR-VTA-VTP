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
    /// Clase de acceso a datos de la tabla SI_MENU_PROJECT
    /// </summary>
    public class SiMenuProjectRepository: RepositoryBase, ISiMenuProjectRepository
    {
        public SiMenuProjectRepository(string strConn): base(strConn)
        {
        }

        SiMenuProjectHelper helper = new SiMenuProjectHelper();

        public int Save(SiMenuProjectDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mprojcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Mprojdescripcion, DbType.String, entity.Mprojdescripcion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiMenuProjectDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mprojcodi, DbType.Int32, entity.Mprojcodi);
            dbProvider.AddInParameter(command, helper.Mprojdescripcion, DbType.String, entity.Mprojdescripcion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mprojcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mprojcodi, DbType.Int32, mprojcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiMenuProjectDTO GetById(int mprojcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mprojcodi, DbType.Int32, mprojcodi);
            SiMenuProjectDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiMenuProjectDTO> List()
        {
            List<SiMenuProjectDTO> entitys = new List<SiMenuProjectDTO>();
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

        public List<SiMenuProjectDTO> GetByCriteria()
        {
            List<SiMenuProjectDTO> entitys = new List<SiMenuProjectDTO>();
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
