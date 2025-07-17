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
    /// Clase de acceso a datos de la tabla AUD_TABLACODIGO
    /// </summary>
    public class AudTablacodigoRepository: RepositoryBase, IAudTablacodigoRepository
    {
        public AudTablacodigoRepository(string strConn): base(strConn)
        {
        }

        AudTablacodigoHelper helper = new AudTablacodigoHelper();

        public int Save(AudTablacodigoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tabccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tabcdescripcion, DbType.String, entity.Tabcdescripcion);
            dbProvider.AddInParameter(command, helper.Tabcactivo, DbType.String, entity.Tabcactivo);
            dbProvider.AddInParameter(command, helper.Tabcusucreacion, DbType.String, entity.Tabcusucreacion);
            dbProvider.AddInParameter(command, helper.Tabcfeccreacion, DbType.DateTime, entity.Tabcfeccreacion);
            dbProvider.AddInParameter(command, helper.Tabcusumodificacion, DbType.String, entity.Tabcusumodificacion);
            dbProvider.AddInParameter(command, helper.Tabcfecmodificacion, DbType.DateTime, entity.Tabcfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AudTablacodigoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tabcdescripcion, DbType.String, entity.Tabcdescripcion);
            dbProvider.AddInParameter(command, helper.Tabcactivo, DbType.String, entity.Tabcactivo);
            dbProvider.AddInParameter(command, helper.Tabcusucreacion, DbType.String, entity.Tabcusucreacion);
            dbProvider.AddInParameter(command, helper.Tabcfeccreacion, DbType.DateTime, entity.Tabcfeccreacion);
            dbProvider.AddInParameter(command, helper.Tabcusumodificacion, DbType.String, entity.Tabcusumodificacion);
            dbProvider.AddInParameter(command, helper.Tabcfecmodificacion, DbType.DateTime, entity.Tabcfecmodificacion);
            dbProvider.AddInParameter(command, helper.Tabccodi, DbType.Int32, entity.Tabccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tabccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tabccodi, DbType.Int32, tabccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AudTablacodigoDTO GetById(int tabccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tabccodi, DbType.Int32, tabccodi);
            AudTablacodigoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AudTablacodigoDTO> List()
        {
            List<AudTablacodigoDTO> entitys = new List<AudTablacodigoDTO>();
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

        public List<AudTablacodigoDTO> GetByCriteria()
        {
            List<AudTablacodigoDTO> entitys = new List<AudTablacodigoDTO>();
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
