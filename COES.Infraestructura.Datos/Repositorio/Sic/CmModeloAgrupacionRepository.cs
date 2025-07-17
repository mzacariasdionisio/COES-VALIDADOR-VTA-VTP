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
    /// Clase de acceso a datos de la tabla CM_MODELO_AGRUPACION
    /// </summary>
    public class CmModeloAgrupacionRepository : RepositoryBase, ICmModeloAgrupacionRepository
    {
        public CmModeloAgrupacionRepository(string strConn) : base(strConn)
        {
        }

        CmModeloAgrupacionHelper helper = new CmModeloAgrupacionHelper();

        public int Save(CmModeloAgrupacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Modagrcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Modcomcodi, DbType.Int32, entity.Modcomcodi);
            dbProvider.AddInParameter(command, helper.Modagrorden, DbType.Int32, entity.Modagrorden);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmModeloAgrupacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Modagrcodi, DbType.Int32, entity.Modagrcodi);
            dbProvider.AddInParameter(command, helper.Modcomcodi, DbType.Int32, entity.Modcomcodi);
            dbProvider.AddInParameter(command, helper.Modagrorden, DbType.Int32, entity.Modagrorden);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int modagrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Modagrcodi, DbType.Int32, modagrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmModeloAgrupacionDTO GetById(int modagrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Modagrcodi, DbType.Int32, modagrcodi);
            CmModeloAgrupacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmModeloAgrupacionDTO> List()
        {
            List<CmModeloAgrupacionDTO> entitys = new List<CmModeloAgrupacionDTO>();
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

        public List<CmModeloAgrupacionDTO> GetByCriteria(int modembcodi)
        {
            List<CmModeloAgrupacionDTO> entitys = new List<CmModeloAgrupacionDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, modembcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
