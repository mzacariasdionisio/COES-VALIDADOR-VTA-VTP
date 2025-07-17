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
    /// Clase de acceso a datos de la tabla WB_CALTIPOVENTO
    /// </summary>
    public class WbCaltipoventoRepository: RepositoryBase, IWbCaltipoventoRepository
    {
        public WbCaltipoventoRepository(string strConn): base(strConn)
        {
        }

        WbCaltipoventoHelper helper = new WbCaltipoventoHelper();

        public int Save(WbCaltipoventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tipcalcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tipcaldesc, DbType.String, entity.Tipcaldesc);
            dbProvider.AddInParameter(command, helper.Tipcalcolor, DbType.String, entity.Tipcalcolor);
            dbProvider.AddInParameter(command, helper.Tipcalicono, DbType.String, entity.Tipcalicono);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbCaltipoventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tipcaldesc, DbType.String, entity.Tipcaldesc);
            dbProvider.AddInParameter(command, helper.Tipcalcolor, DbType.String, entity.Tipcalcolor);
            dbProvider.AddInParameter(command, helper.Tipcalicono, DbType.String, entity.Tipcalicono);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Tipcalcodi, DbType.Int32, entity.Tipcalcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tipcalcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tipcalcodi, DbType.Int32, tipcalcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbCaltipoventoDTO GetById(int tipcalcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tipcalcodi, DbType.Int32, tipcalcodi);
            WbCaltipoventoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbCaltipoventoDTO> List()
        {
            List<WbCaltipoventoDTO> entitys = new List<WbCaltipoventoDTO>();
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

        public List<WbCaltipoventoDTO> GetByCriteria()
        {
            List<WbCaltipoventoDTO> entitys = new List<WbCaltipoventoDTO>();
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
