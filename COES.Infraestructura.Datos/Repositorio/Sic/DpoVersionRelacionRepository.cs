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
    public class DpoVersionRelacionRepository : RepositoryBase, IDpoVersionRelacionRepository
    {
        public DpoVersionRelacionRepository(string strConn) : base(strConn)
        {
        }

        DpoVersionRelacionHelper helper = new DpoVersionRelacionHelper();

        public int Save(DpoVersionRelacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Dposplcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Dposplnombre, DbType.String, entity.Dposplnombre);
            dbProvider.AddInParameter(command, helper.Dposplusucreacion, DbType.String, entity.Dposplusucreacion);
            dbProvider.AddInParameter(command, helper.Dposplfeccreacion, DbType.DateTime, entity.Dposplfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(DpoVersionRelacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Dposplcodi, DbType.Int32, entity.Dposplcodi);
            dbProvider.AddInParameter(command, helper.Dposplnombre, DbType.String, entity.Dposplnombre);
            dbProvider.AddInParameter(command, helper.Dposplusucreacion, DbType.String, entity.Dposplusucreacion);
            dbProvider.AddInParameter(command, helper.Dposplfeccreacion, DbType.DateTime, entity.Dposplfeccreacion);
            dbProvider.AddInParameter(command, helper.Dposplusumodificacion, DbType.String, entity.Dposplusumodificacion);
            dbProvider.AddInParameter(command, helper.Dposplfecmodificacion, DbType.DateTime, entity.Dposplfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int codi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Dposplcodi, DbType.Int32, codi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<DpoVersionRelacionDTO> List()
        {
            List<DpoVersionRelacionDTO> entitys = new List<DpoVersionRelacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoVersionRelacionDTO entity = new DpoVersionRelacionDTO();

                    int iDposplcodi = dr.GetOrdinal(helper.Dposplcodi);
                    if (!dr.IsDBNull(iDposplcodi)) entity.Dposplcodi = Convert.ToInt32(dr.GetValue(iDposplcodi));

                    int iDposplnombre = dr.GetOrdinal(helper.Dposplnombre);
                    if (!dr.IsDBNull(iDposplnombre)) entity.Dposplnombre = dr.GetString(iDposplnombre);

                    int iDposplusucreacion = dr.GetOrdinal(helper.Dposplusucreacion);
                    if (!dr.IsDBNull(iDposplusucreacion)) entity.Dposplusucreacion = dr.GetString(iDposplusucreacion);

                    int iDposplfeccreacion = dr.GetOrdinal(helper.Dposplfeccreacion);
                    if (!dr.IsDBNull(iDposplfeccreacion)) entity.Dposplfeccreacion = dr.GetDateTime(iDposplfeccreacion);

                    int iDposplusumodificacion = dr.GetOrdinal(helper.Dposplusumodificacion);
                    if (!dr.IsDBNull(iDposplusumodificacion)) entity.Dposplusumodificacion = dr.GetString(iDposplusumodificacion);

                    int iDposplfecmodificacion = dr.GetOrdinal(helper.Dposplfecmodificacion);
                    if (!dr.IsDBNull(iDposplfecmodificacion)) entity.Dposplfecmodificacion = dr.GetDateTime(iDposplfecmodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public DpoVersionRelacionDTO GetById(int codi)
        {
            DpoVersionRelacionDTO entity = new DpoVersionRelacionDTO();

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
