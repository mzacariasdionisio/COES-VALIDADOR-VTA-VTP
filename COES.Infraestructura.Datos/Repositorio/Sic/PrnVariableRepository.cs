using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class PrnVariableRepository : RepositoryBase, IPrnVariableRepository
    {
        public PrnVariableRepository(string strConn)
        : base(strConn)
        {
        }

        PrnVariableHelper helper = new PrnVariableHelper();

        public List<PrnVariableDTO> List()
        {
            List<PrnVariableDTO> entitys = new List<PrnVariableDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnVariableDTO entity = new PrnVariableDTO();

                    int iPrnvarcodi = dr.GetOrdinal(helper.Prnvarcodi);
                    if (!dr.IsDBNull(iPrnvarcodi)) entity.Prnvarcodi = Convert.ToInt32(dr.GetValue(iPrnvarcodi));

                    int iPrnvarnom = dr.GetOrdinal(helper.Prnvarnom);
                    if (!dr.IsDBNull(iPrnvarnom)) entity.Prnvarnom = dr.GetString(iPrnvarnom);

                    int iPrnvarabrev = dr.GetOrdinal(helper.Prnvarabrev);
                    if (!dr.IsDBNull(iPrnvarabrev)) entity.Prnvarabrev = dr.GetString(iPrnvarabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public void Save(PrnVariableDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Prnvarcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Prnvarnom, DbType.String, entity.Prnvarnom);
            dbProvider.AddInParameter(command, helper.Prnvarabrev, DbType.String, entity.Prnvarabrev);
            dbProvider.ExecuteNonQuery(command);
        }
        public void Update(PrnVariableDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            dbProvider.AddInParameter(command, helper.Prnvarcodi, DbType.Int32, entity.Prnvarcodi);
            dbProvider.AddInParameter(command, helper.Prnvarnom, DbType.String, entity.Prnvarnom);
            dbProvider.AddInParameter(command, helper.Prnvarabrev, DbType.String, entity.Prnvarabrev);
            dbProvider.ExecuteNonQuery(command);
        }
        public PrnVariableDTO GetById(int codigo)
        {
            PrnVariableDTO entity = new PrnVariableDTO();

            string query = string.Format(helper.SqlGetById, codigo);
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
        public void Delete(int codigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Prnvarcodi, DbType.Int32, codigo);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<PrnVariableDTO> ListVariableByTipo(string tipo)
        {

            List<PrnVariableDTO> entitys = new List<PrnVariableDTO>();
            string query = string.Format(helper.SqlListVariableByTipo, tipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnVariableDTO entity = new PrnVariableDTO();

                    int iPrnvarcodi = dr.GetOrdinal(helper.Prnvarcodi);
                    if (!dr.IsDBNull(iPrnvarcodi)) entity.Prnvarcodi = Convert.ToInt32(dr.GetValue(iPrnvarcodi));

                    int iPrnvarnom = dr.GetOrdinal(helper.Prnvarnom);
                    if (!dr.IsDBNull(iPrnvarnom)) entity.Prnvarnom = dr.GetString(iPrnvarnom);

                    int iPrnvarabrev = dr.GetOrdinal(helper.Prnvarabrev);
                    if (!dr.IsDBNull(iPrnvarabrev)) entity.Prnvarabrev = dr.GetString(iPrnvarabrev);

                    int iPrnvartipomedi = dr.GetOrdinal(helper.Prnvartipomedi);
                    if (!dr.IsDBNull(iPrnvartipomedi)) entity.Prnvartipomedi = dr.GetString(iPrnvartipomedi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
