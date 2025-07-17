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
    /// Clase de acceso a datos de la tabla EN_ENSAYOUNIDAD
    /// </summary>
    public class EnEnsayounidadRepository : RepositoryBase, IEnEnsayounidadRepository
    {
        public EnEnsayounidadRepository(string strConn)
            : base(strConn)
        {
        }

        EnEnsayounidadHelper helper = new EnEnsayounidadHelper();

        public int Save(EnEnsayounidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Enunidadcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ensayocodi, DbType.Int32, entity.Ensayocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EnEnsayounidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Enunidadcodi, DbType.Int32, entity.Enunidadcodi);
            dbProvider.AddInParameter(command, helper.Ensayocodi, DbType.Int32, entity.Ensayocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int enunidadcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Enunidadcodi, DbType.Int32, enunidadcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EnEnsayounidadDTO GetById(int enunidadcodi)
        {
            string queryString = string.Format(helper.SqlGetById, enunidadcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            EnEnsayounidadDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EnEnsayounidadDTO> List()
        {
            List<EnEnsayounidadDTO> entitys = new List<EnEnsayounidadDTO>();
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

        public List<EnEnsayounidadDTO> GetByCriteria()
        {
            List<EnEnsayounidadDTO> entitys = new List<EnEnsayounidadDTO>();
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



        public EnEnsayounidadDTO GetEnsayoUnidad(int idensayo, int equicodi)
        {
            EnEnsayounidadDTO entity = new EnEnsayounidadDTO();
            string query = string.Format(helper.SqlGetEnsayoUnidad, idensayo, equicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EnEnsayounidadDTO> GetUnidadesXEnsayo(int idensayo)
        {
            List<EnEnsayounidadDTO> entitys = new List<EnEnsayounidadDTO>();
            string query = string.Format(helper.SqlGetUnidadesXEnsayo, idensayo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            EnEnsayounidadDTO entity = new EnEnsayounidadDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }
            return entitys;

        }

    }
}
