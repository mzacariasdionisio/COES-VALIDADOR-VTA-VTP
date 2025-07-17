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
    /// Clase de acceso a datos de la tabla DAI_TABLACODIGO
    /// </summary>
    public class DaiTablacodigoRepository: RepositoryBase, IDaiTablacodigoRepository
    {
        public DaiTablacodigoRepository(string strConn): base(strConn)
        {
        }

        DaiTablacodigoHelper helper = new DaiTablacodigoHelper();

        public int Save(DaiTablacodigoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tabcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tabdescripcion, DbType.String, entity.Tabdescripcion);
            dbProvider.AddInParameter(command, helper.Tabactivo, DbType.String, entity.Tabactivo);
            dbProvider.AddInParameter(command, helper.Tabusucreacion, DbType.String, entity.Tabusucreacion);
            dbProvider.AddInParameter(command, helper.Tabfeccreacion, DbType.DateTime, entity.Tabfeccreacion);
            dbProvider.AddInParameter(command, helper.Tabusumodificacion, DbType.String, entity.Tabusumodificacion);
            dbProvider.AddInParameter(command, helper.Tabfecmodificacion, DbType.DateTime, entity.Tabfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(DaiTablacodigoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tabdescripcion, DbType.String, entity.Tabdescripcion);
            dbProvider.AddInParameter(command, helper.Tabactivo, DbType.String, entity.Tabactivo);
            dbProvider.AddInParameter(command, helper.Tabusucreacion, DbType.String, entity.Tabusucreacion);
            dbProvider.AddInParameter(command, helper.Tabfeccreacion, DbType.DateTime, entity.Tabfeccreacion);
            dbProvider.AddInParameter(command, helper.Tabusumodificacion, DbType.String, entity.Tabusumodificacion);
            dbProvider.AddInParameter(command, helper.Tabfecmodificacion, DbType.DateTime, entity.Tabfecmodificacion);
            dbProvider.AddInParameter(command, helper.Tabcodi, DbType.Int32, entity.Tabcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tabcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tabcodi, DbType.Int32, tabcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public DaiTablacodigoDTO GetById(int tabcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tabcodi, DbType.Int32, tabcodi);
            DaiTablacodigoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<DaiTablacodigoDTO> List()
        {
            List<DaiTablacodigoDTO> entitys = new List<DaiTablacodigoDTO>();
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

        public List<DaiTablacodigoDTO> GetByCriteria()
        {
            List<DaiTablacodigoDTO> entitys = new List<DaiTablacodigoDTO>();
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
