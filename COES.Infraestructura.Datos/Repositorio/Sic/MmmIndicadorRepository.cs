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
    /// Clase de acceso a datos de la tabla MMM_INDICADOR
    /// </summary>
    public class MmmIndicadorRepository : RepositoryBase, IMmmIndicadorRepository
    {
        public MmmIndicadorRepository(string strConn)
            : base(strConn)
        {
        }

        MmmIndicadorHelper helper = new MmmIndicadorHelper();

        public int Save(MmmIndicadorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Immecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Immeabrev, DbType.String, entity.Immeabrev);
            dbProvider.AddInParameter(command, helper.Immenombre, DbType.String, entity.Immenombre);
            dbProvider.AddInParameter(command, helper.Immedescripcion, DbType.String, entity.Immedescripcion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MmmIndicadorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Immecodi, DbType.Int32, entity.Immecodi);
            dbProvider.AddInParameter(command, helper.Immeabrev, DbType.String, entity.Immeabrev);
            dbProvider.AddInParameter(command, helper.Immenombre, DbType.String, entity.Immenombre);
            dbProvider.AddInParameter(command, helper.Immedescripcion, DbType.String, entity.Immedescripcion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int immecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Immecodi, DbType.Int32, immecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MmmIndicadorDTO GetById(int immecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Immecodi, DbType.Int32, immecodi);
            MmmIndicadorDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MmmIndicadorDTO> List()
        {
            List<MmmIndicadorDTO> entitys = new List<MmmIndicadorDTO>();
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

        public List<MmmIndicadorDTO> GetByCriteria()
        {
            List<MmmIndicadorDTO> entitys = new List<MmmIndicadorDTO>();
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
