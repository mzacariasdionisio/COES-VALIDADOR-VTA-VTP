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
    /// Clase de acceso a datos de la tabla TRN_BARRA_AREA
    /// </summary>
    public class TrnBarraAreaRepository: RepositoryBase, ITrnBarraAreaRepository
    {
        public TrnBarraAreaRepository(string strConn): base(strConn)
        {
        }

        TrnBarraAreaHelper helper = new TrnBarraAreaHelper();

        public int Save(TrnBarraAreaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Bararecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Bararearea, DbType.String, entity.Bararearea);
            dbProvider.AddInParameter(command, helper.Barareejecutiva, DbType.String, entity.Barareejecutiva);
            dbProvider.AddInParameter(command, helper.Barareestado, DbType.String, entity.Barareestado);
            dbProvider.AddInParameter(command, helper.Barareusucreacion, DbType.String, entity.Barareusucreacion);
            dbProvider.AddInParameter(command, helper.Bararefeccreacion, DbType.DateTime, entity.Bararefeccreacion);
            dbProvider.AddInParameter(command, helper.Barareusumodificacion, DbType.String, entity.Barareusumodificacion);
            dbProvider.AddInParameter(command, helper.Bararefecmodificacion, DbType.DateTime, entity.Bararefecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(TrnBarraAreaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Bararearea, DbType.String, entity.Bararearea);
            dbProvider.AddInParameter(command, helper.Barareejecutiva, DbType.String, entity.Barareejecutiva);
            dbProvider.AddInParameter(command, helper.Barareestado, DbType.String, entity.Barareestado);
            dbProvider.AddInParameter(command, helper.Barareusucreacion, DbType.String, entity.Barareusucreacion);
            dbProvider.AddInParameter(command, helper.Bararefeccreacion, DbType.DateTime, entity.Bararefeccreacion);
            dbProvider.AddInParameter(command, helper.Barareusumodificacion, DbType.String, entity.Barareusumodificacion);
            dbProvider.AddInParameter(command, helper.Bararefecmodificacion, DbType.DateTime, entity.Bararefecmodificacion);
            dbProvider.AddInParameter(command, helper.Bararecodi, DbType.Int32, entity.Bararecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int bararecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Bararecodi, DbType.Int32, bararecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public TrnBarraAreaDTO GetById(int bararecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Bararecodi, DbType.Int32, bararecodi);
            TrnBarraAreaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TrnBarraAreaDTO> List()
        {
            List<TrnBarraAreaDTO> entitys = new List<TrnBarraAreaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);
                    int iBararearea = dr.GetOrdinal("barrnombre");
                    if (!dr.IsDBNull(iBararearea)) entity.Barrnombre = dr.GetString(iBararearea);
                    entitys.Add(entity);                    
                }
            }

            return entitys;
        }

        public List<TrnBarraAreaDTO> GetByCriteria()
        {
            List<TrnBarraAreaDTO> entitys = new List<TrnBarraAreaDTO>();
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
