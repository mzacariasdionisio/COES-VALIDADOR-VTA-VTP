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
    public class DpoBarraRepository : RepositoryBase, IDpoBarraRepository
    {
        public DpoBarraRepository(string strConn) : base(strConn)
        {
        }

        DpoBarraHelper helper = new DpoBarraHelper();

        public void Save(DpoBarraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Dpobarcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Dpobarcodiexcel, DbType.String, entity.Dpobarcodiexcel);
            dbProvider.AddInParameter(command, helper.Dpobarnombre, DbType.String, entity.Dpobarnombre);
            dbProvider.AddInParameter(command, helper.Dpobartension, DbType.Decimal, entity.Dpobartension);
            dbProvider.AddInParameter(command, helper.Dpobarusucreacion, DbType.String, entity.Dpobarusucreacion);
            dbProvider.AddInParameter(command, helper.Dpobarfeccreacion, DbType.DateTime, entity.Dpobarfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(DpoBarraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Dpobarcodi, DbType.String, entity.Dpobarcodi);
            dbProvider.AddInParameter(command, helper.Dpobarnombre, DbType.String, entity.Dpobarnombre);
            dbProvider.AddInParameter(command, helper.Dpobartension, DbType.Decimal, entity.Dpobartension);
            dbProvider.AddInParameter(command, helper.Dpobarusucreacion, DbType.String, entity.Dpobarusucreacion);
            dbProvider.AddInParameter(command, helper.Dpobarfeccreacion, DbType.DateTime, entity.Dpobarfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(string codi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Dpobarcodi, DbType.String, codi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<DpoBarraDTO> List()
        {
            List<DpoBarraDTO> entitys = new List<DpoBarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoBarraDTO entity = new DpoBarraDTO();

                    int iDpobarcodi = dr.GetOrdinal(helper.Dpobarcodi);
                    if (!dr.IsDBNull(iDpobarcodi)) entity.Dpobarcodi = Convert.ToInt32(dr.GetValue(iDpobarcodi));

                    int iDpobarcodiexcel = dr.GetOrdinal(helper.Dpobarcodiexcel);
                    if (!dr.IsDBNull(iDpobarcodiexcel)) entity.Dpobarcodiexcel = dr.GetString(iDpobarcodiexcel);

                    int iDpobarnombre = dr.GetOrdinal(helper.Dpobarnombre);
                    if (!dr.IsDBNull(iDpobarnombre)) entity.Dpobarnombre = dr.GetString(iDpobarnombre);

                    int iDpobartension = dr.GetOrdinal(helper.Dpobartension);
                    if (!dr.IsDBNull(iDpobartension)) entity.Dpobartension = dr.GetDecimal(iDpobartension);

                    int iDpobarusucreacion = dr.GetOrdinal(helper.Dpobarusucreacion);
                    if (!dr.IsDBNull(iDpobarusucreacion)) entity.Dpobarusucreacion = dr.GetString(iDpobarusucreacion);

                    int iDpobarfeccreacion = dr.GetOrdinal(helper.Dpobarfeccreacion);
                    if (!dr.IsDBNull(iDpobarfeccreacion)) entity.Dpobarfeccreacion = dr.GetDateTime(iDpobarfeccreacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public DpoBarraDTO GetById(string codi)
        {
            DpoBarraDTO entity = new DpoBarraDTO();

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
