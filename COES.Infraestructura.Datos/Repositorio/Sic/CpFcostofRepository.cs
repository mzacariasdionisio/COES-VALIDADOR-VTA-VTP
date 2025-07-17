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
    /// Clase de acceso a datos de la tabla CP_FCOSTOF
    /// </summary>
    public class CpFcostofRepository: RepositoryBase, ICpFcostofRepository
    {
        public CpFcostofRepository(string strConn): base(strConn)
        {
        }

        CpFcostofHelper helper = new CpFcostofHelper();

        public void Save(CpFcostofDTO entity)
        {
            string query = string.Format(helper.SqlSave, entity.Fcfembalses, entity.Fcfnumcortes, entity.Topcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public void CrearCopia(int topcodi1, int topcodi2)
        {
            string query = string.Format(helper.SqlCrearCopia, topcodi1, topcodi2);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(CpFcostofDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Fcfembalses, DbType.String, entity.Fcfembalses);
            dbProvider.AddInParameter(command, helper.Fcfnumcortes, DbType.Int32, entity.Fcfnumcortes);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int topcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, topcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpFcostofDTO GetById(int topcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, topcodi);
            CpFcostofDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpFcostofDTO> List()
        {
            List<CpFcostofDTO> entitys = new List<CpFcostofDTO>();
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

        public List<CpFcostofDTO> GetByCriteria()
        {
            List<CpFcostofDTO> entitys = new List<CpFcostofDTO>();
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
