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
    public class PrnPtoEstimadorRepository : RepositoryBase, IPrnPtoEstimadorRepository
    {
        public PrnPtoEstimadorRepository(string strConn)
        : base(strConn)
        {
        }

        PrnPtoEstimadorHelper helper = new PrnPtoEstimadorHelper();

        public List<PrnPtoEstimadorDTO> List()
        {
            List<PrnPtoEstimadorDTO> entitys = new List<PrnPtoEstimadorDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnPtoEstimadorDTO entity = new PrnPtoEstimadorDTO();

                    int iPtoetmcodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtoetmcodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtoetmcodi));

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtoetmtipomedi = dr.GetOrdinal(helper.Ptoetmtipomedi);
                    if (!dr.IsDBNull(iPtoetmtipomedi)) entity.Ptoetmtipomedi = dr.GetString(iPtoetmtipomedi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public void Save(PrnPtoEstimadorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Ptoetmcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Ptoetmtipomedi, DbType.String, entity.Ptoetmtipomedi);
            dbProvider.ExecuteNonQuery(command);
        }
        public void Update(PrnPtoEstimadorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            dbProvider.AddInParameter(command, helper.Ptoetmcodi, DbType.Int32, entity.Ptoetmcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Ptoetmtipomedi, DbType.String, entity.Ptoetmtipomedi);
            dbProvider.ExecuteNonQuery(command);
        }
        public PrnPtoEstimadorDTO GetById(int codigo)
        {
            PrnPtoEstimadorDTO entity = new PrnPtoEstimadorDTO();

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

            dbProvider.AddInParameter(command, helper.Ptoetmcodi, DbType.Int32, codigo);
            dbProvider.ExecuteNonQuery(command);
        }        
    }
}
