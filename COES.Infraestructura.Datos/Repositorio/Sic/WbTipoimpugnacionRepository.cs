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
    /// Clase de acceso a datos de la tabla WB_TIPOIMPUGNACION
    /// </summary>
    public class WbTipoimpugnacionRepository: RepositoryBase, IWbTipoimpugnacionRepository
    {
        public WbTipoimpugnacionRepository(string strConn): base(strConn)
        {
        }

        WbTipoimpugnacionHelper helper = new WbTipoimpugnacionHelper();

        public int Save(WbTipoimpugnacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Timpgcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Timpgnombre, DbType.String, entity.Timpgnombre);
            dbProvider.AddInParameter(command, helper.Timpgnombdecision, DbType.String, entity.Timpgnombdecision);
            dbProvider.AddInParameter(command, helper.Timpgnombrefecha, DbType.String, entity.Timpgnombrefecha);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbTipoimpugnacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Timpgcodi, DbType.Int32, entity.Timpgcodi);
            dbProvider.AddInParameter(command, helper.Timpgnombre, DbType.String, entity.Timpgnombre);
            dbProvider.AddInParameter(command, helper.Timpgnombdecision, DbType.String, entity.Timpgnombdecision);
            dbProvider.AddInParameter(command, helper.Timpgnombrefecha, DbType.String, entity.Timpgnombrefecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int timpgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Timpgcodi, DbType.Int32, timpgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbTipoimpugnacionDTO GetById(int timpgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Timpgcodi, DbType.Int32, timpgcodi);
            WbTipoimpugnacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbTipoimpugnacionDTO> List()
        {
            List<WbTipoimpugnacionDTO> entitys = new List<WbTipoimpugnacionDTO>();
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

        public List<WbTipoimpugnacionDTO> GetByCriteria(string nombreTipoImpugnacion)
        {
            List<WbTipoimpugnacionDTO> entitys = new List<WbTipoimpugnacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.Timpgnombre, DbType.String, nombreTipoImpugnacion);

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
