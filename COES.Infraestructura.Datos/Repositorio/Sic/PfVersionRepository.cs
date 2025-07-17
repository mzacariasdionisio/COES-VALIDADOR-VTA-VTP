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
    /// Clase de acceso a datos de la tabla PF_VERSION
    /// </summary>
    public class PfVersionRepository : RepositoryBase, IPfVersionRepository
    {
        public PfVersionRepository(string strConn) : base(strConn)
        {
        }

        PfVersionHelper helper = new PfVersionHelper();

        public int Save(PfVersionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pfverscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pfrecacodi, DbType.Int32, entity.Pfrecacodi);
            dbProvider.AddInParameter(command, helper.Pfrecucodi, DbType.Int32, entity.Pfrecucodi);
            dbProvider.AddInParameter(command, helper.Pfversusucreacion, DbType.String, entity.Pfversusucreacion);
            dbProvider.AddInParameter(command, helper.Pfversfeccreacion, DbType.DateTime, entity.Pfversfeccreacion);
            dbProvider.AddInParameter(command, helper.Pfversnumero, DbType.Int32, entity.Pfversnumero);
            dbProvider.AddInParameter(command, helper.Pfversestado, DbType.String, entity.Pfversestado);
            dbProvider.AddInParameter(command, helper.Irptcodi, DbType.Int32, entity.Irptcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfVersionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfrecacodi, DbType.Int32, entity.Pfrecacodi);
            dbProvider.AddInParameter(command, helper.Pfrecucodi, DbType.Int32, entity.Pfrecucodi);
            dbProvider.AddInParameter(command, helper.Pfversusucreacion, DbType.String, entity.Pfversusucreacion);
            dbProvider.AddInParameter(command, helper.Pfversfeccreacion, DbType.DateTime, entity.Pfversfeccreacion);
            dbProvider.AddInParameter(command, helper.Pfversnumero, DbType.Int32, entity.Pfversnumero);
            dbProvider.AddInParameter(command, helper.Pfversestado, DbType.String, entity.Pfversestado);
            dbProvider.AddInParameter(command, helper.Irptcodi, DbType.Int32, entity.Irptcodi);
            dbProvider.AddInParameter(command, helper.Pfverscodi, DbType.Int32, entity.Pfverscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfverscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfverscodi, DbType.Int32, pfverscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfVersionDTO GetById(int pfverscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfverscodi, DbType.Int32, pfverscodi);
            PfVersionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfVersionDTO> List()
        {
            List<PfVersionDTO> entitys = new List<PfVersionDTO>();
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

        public List<PfVersionDTO> GetByCriteria(int recacodi, int recucodi)
        {
            List<PfVersionDTO> entitys = new List<PfVersionDTO>();

            string query = string.Format(helper.SqlGetByCriteria, recacodi, recucodi);
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
