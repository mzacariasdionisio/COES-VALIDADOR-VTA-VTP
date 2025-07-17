using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Dominio.Interfaces.Sic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class PrnVariableexogenaRepository : RepositoryBase, IPrnVariableexogenaRepository
    {
        public PrnVariableexogenaRepository(string strConn)
            : base(strConn)
        {
        }

        PrnVariableexogenaHelper helper = new PrnVariableexogenaHelper();

        public void Save(PrnVariableexogenaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Varexocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Varexonombre, DbType.String, entity.Varexonombre);
            dbProvider.AddInParameter(command, helper.Varexousucreacion, DbType.String, entity.Varexousucreacion);
            dbProvider.AddInParameter(command, helper.Varexofeccreacion, DbType.DateTime, entity.Varexofeccreacion);
            dbProvider.AddInParameter(command, helper.Varexousumodificacion, DbType.String, entity.Varexousumodificacion);
            dbProvider.AddInParameter(command, helper.Varexofecmodificacion, DbType.DateTime, entity.Varexofecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(PrnVariableexogenaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Varexonombre, DbType.String, entity.Varexonombre);
            dbProvider.AddInParameter(command, helper.Varexousumodificacion, DbType.String, entity.Varexousumodificacion);
            dbProvider.AddInParameter(command, helper.Varexofecmodificacion, DbType.DateTime, entity.Varexofecmodificacion);

            dbProvider.AddInParameter(command, helper.Varexocodi, DbType.Int32, entity.Varexocodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int varexocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Varexocodi, DbType.Int32, varexocodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<PrnVariableexogenaDTO> List()
        {
            List<PrnVariableexogenaDTO> entitys = new List<PrnVariableexogenaDTO>();
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
    }
}
