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
    /// Clase de acceso a datos de la tabla EQ_RELACION_TNA
    /// </summary>
    public class EqRelacionTnaRepository: RepositoryBase, IEqRelacionTnaRepository
    {
        public EqRelacionTnaRepository(string strConn): base(strConn)
        {
        }

        EqRelacionTnaHelper helper = new EqRelacionTnaHelper();

        public int Save(EqRelacionTnaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Reltnacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Relacioncodi, DbType.Int32, entity.Relacioncodi);
            dbProvider.AddInParameter(command, helper.Reltnanombre, DbType.String, entity.Reltnanombre);
            dbProvider.AddInParameter(command, helper.Reltnaestado, DbType.String, entity.Reltnaestado);
            dbProvider.AddInParameter(command, helper.Reltnausucreacion, DbType.String, entity.Reltnausucreacion);
            dbProvider.AddInParameter(command, helper.Reltnafeccreacion, DbType.DateTime, entity.Reltnafeccreacion);
            dbProvider.AddInParameter(command, helper.Reltnausumodificacion, DbType.String, entity.Reltnausumodificacion);
            dbProvider.AddInParameter(command, helper.Reltnafecmodificacion, DbType.DateTime, entity.Reltnafecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EqRelacionTnaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Relacioncodi, DbType.Int32, entity.Relacioncodi);
            dbProvider.AddInParameter(command, helper.Reltnanombre, DbType.String, entity.Reltnanombre);
            dbProvider.AddInParameter(command, helper.Reltnaestado, DbType.String, entity.Reltnaestado);
            dbProvider.AddInParameter(command, helper.Reltnausucreacion, DbType.String, entity.Reltnausucreacion);
            dbProvider.AddInParameter(command, helper.Reltnafeccreacion, DbType.DateTime, entity.Reltnafeccreacion);
            dbProvider.AddInParameter(command, helper.Reltnausumodificacion, DbType.String, entity.Reltnausumodificacion);
            dbProvider.AddInParameter(command, helper.Reltnafecmodificacion, DbType.DateTime, entity.Reltnafecmodificacion);
            dbProvider.AddInParameter(command, helper.Reltnacodi, DbType.Int32, entity.Reltnacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int reltnacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Reltnacodi, DbType.Int32, reltnacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EqRelacionTnaDTO GetById(int reltnacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Reltnacodi, DbType.Int32, reltnacodi);
            EqRelacionTnaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EqRelacionTnaDTO> List()
        {
            List<EqRelacionTnaDTO> entitys = new List<EqRelacionTnaDTO>();
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

        public List<EqRelacionTnaDTO> GetByCriteria(int relacioncodi)
        {
            List<EqRelacionTnaDTO> entitys = new List<EqRelacionTnaDTO>();
            string query = string.Format(helper.SqlGetByCriteria, relacioncodi);
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
