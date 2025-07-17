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
    /// Clase de acceso a datos de la tabla PF_RECURSO
    /// </summary>
    public class PfRecursoRepository : RepositoryBase, IPfRecursoRepository
    {
        public PfRecursoRepository(string strConn) : base(strConn)
        {
        }

        PfRecursoHelper helper = new PfRecursoHelper();

        public int Save(PfRecursoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pfrecucodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pfrecunomb, DbType.String, entity.Pfrecunomb);
            dbProvider.AddInParameter(command, helper.Pfrecudescripcion, DbType.String, entity.Pfrecudescripcion);
            dbProvider.AddInParameter(command, helper.Pfrecutipo, DbType.Int32, entity.Pfrecutipo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfRecursoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfrecucodi, DbType.Int32, entity.Pfrecucodi);
            dbProvider.AddInParameter(command, helper.Pfrecunomb, DbType.String, entity.Pfrecunomb);
            dbProvider.AddInParameter(command, helper.Pfrecudescripcion, DbType.String, entity.Pfrecudescripcion);
            dbProvider.AddInParameter(command, helper.Pfrecutipo, DbType.Int32, entity.Pfrecutipo);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfrecucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfrecucodi, DbType.Int32, pfrecucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfRecursoDTO GetById(int pfrecucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfrecucodi, DbType.Int32, pfrecucodi);
            PfRecursoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfRecursoDTO> List()
        {
            List<PfRecursoDTO> entitys = new List<PfRecursoDTO>();
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

        public List<PfRecursoDTO> GetByCriteria()
        {
            List<PfRecursoDTO> entitys = new List<PfRecursoDTO>();
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
