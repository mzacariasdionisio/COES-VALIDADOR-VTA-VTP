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
    public class DpoSubestacionRepository : RepositoryBase, IDpoSubestacionRepository
    {
        public DpoSubestacionRepository(string strConn) : base(strConn)
        {
        }

        DpoSubestacionHelper helper = new DpoSubestacionHelper();

        public void Save(DpoSubestacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Dposubcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Dposubcodiexcel, DbType.String, entity.Dposubcodiexcel);
            dbProvider.AddInParameter(command, helper.Dposubnombre, DbType.String, entity.Dposubnombre);
            dbProvider.AddInParameter(command, helper.Dposubusucreacion, DbType.String, entity.Dposubusucreacion);
            dbProvider.AddInParameter(command, helper.Dposubfeccreacion, DbType.DateTime, entity.Dposubfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(DpoSubestacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Dposubcodi, DbType.String, entity.Dposubcodi);
            dbProvider.AddInParameter(command, helper.Dposubnombre, DbType.String, entity.Dposubnombre);
            dbProvider.AddInParameter(command, helper.Dposubusucreacion, DbType.String, entity.Dposubusucreacion);
            dbProvider.AddInParameter(command, helper.Dposubfeccreacion, DbType.DateTime, entity.Dposubfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(string codi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Dposubcodi, DbType.String, codi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<DpoSubestacionDTO> List()
        {
            List<DpoSubestacionDTO> entitys = new List<DpoSubestacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoSubestacionDTO entity = new DpoSubestacionDTO();

                    int iDposubcodi = dr.GetOrdinal(helper.Dposubcodi);
                    if (!dr.IsDBNull(iDposubcodi)) entity.Dposubcodi = Convert.ToInt32(dr.GetValue(iDposubcodi));

                    int iDposubcodiexcel = dr.GetOrdinal(helper.Dposubcodiexcel);
                    if (!dr.IsDBNull(iDposubcodiexcel)) entity.Dposubcodiexcel = dr.GetString(iDposubcodiexcel);

                    int iDposubnombre = dr.GetOrdinal(helper.Dposubnombre);
                    if (!dr.IsDBNull(iDposubnombre)) entity.Dposubnombre = dr.GetString(iDposubnombre);

                    int iDposubusucreacion = dr.GetOrdinal(helper.Dposubusucreacion);
                    if (!dr.IsDBNull(iDposubusucreacion)) entity.Dposubusucreacion = dr.GetString(iDposubusucreacion);

                    int iDposubfeccreacion = dr.GetOrdinal(helper.Dposubfeccreacion);
                    if (!dr.IsDBNull(iDposubfeccreacion)) entity.Dposubfeccreacion = dr.GetDateTime(iDposubfeccreacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public DpoSubestacionDTO GetById(string codi)
        {
            DpoSubestacionDTO entity = new DpoSubestacionDTO();

            string query = string.Format(helper.SqlGetById, codi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }
}
