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
    /// Clase de acceso a datos de la tabla AUD_TABLACODIGO_DETALLE
    /// </summary>
    public class AudTablacodigoDetalleRepository: RepositoryBase, IAudTablacodigoDetalleRepository
    {
        public AudTablacodigoDetalleRepository(string strConn): base(strConn)
        {
        }

        AudTablacodigoDetalleHelper helper = new AudTablacodigoDetalleHelper();

        public int Save(AudTablacodigoDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tabcdcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tabccodi, DbType.Int32, entity.Tabccodi);
            dbProvider.AddInParameter(command, helper.Tabcddescripcion, DbType.String, entity.Tabcddescripcion);
            dbProvider.AddInParameter(command, helper.Tabcdactivo, DbType.String, entity.Tabcdactivo);
            dbProvider.AddInParameter(command, helper.Tabcdusucreacion, DbType.String, entity.Tabcdusucreacion);
            dbProvider.AddInParameter(command, helper.Tabcdfeccreacion, DbType.DateTime, entity.Tabcdfeccreacion);
            dbProvider.AddInParameter(command, helper.Tabcdusumodificacion, DbType.String, entity.Tabcdusumodificacion);
            dbProvider.AddInParameter(command, helper.Tabcdfecmodificacion, DbType.DateTime, entity.Tabcdfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AudTablacodigoDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tabccodi, DbType.Int32, entity.Tabccodi);
            dbProvider.AddInParameter(command, helper.Tabcdvalor, DbType.String, entity.Tabcdvalor);
            dbProvider.AddInParameter(command, helper.Tabcddescripcion, DbType.String, entity.Tabcddescripcion);
            dbProvider.AddInParameter(command, helper.Tabcdactivo, DbType.String, entity.Tabcdactivo);
            dbProvider.AddInParameter(command, helper.Tabcdusucreacion, DbType.String, entity.Tabcdusucreacion);
            dbProvider.AddInParameter(command, helper.Tabcdfeccreacion, DbType.DateTime, entity.Tabcdfeccreacion);
            dbProvider.AddInParameter(command, helper.Tabcdusumodificacion, DbType.String, entity.Tabcdusumodificacion);
            dbProvider.AddInParameter(command, helper.Tabcdfecmodificacion, DbType.DateTime, entity.Tabcdfecmodificacion);
            dbProvider.AddInParameter(command, helper.Tabcdcodi, DbType.Int32, entity.Tabcdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tabcdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tabcdcodi, DbType.Int32, tabcdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AudTablacodigoDetalleDTO GetById(int tabcdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tabcdcodi, DbType.Int32, tabcdcodi);
            AudTablacodigoDetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public AudTablacodigoDetalleDTO GetByDescripcion(string descripcion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetByDescripcion);

            dbProvider.AddInParameter(command, helper.Tabcddescripcion, DbType.String, descripcion);
            AudTablacodigoDetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AudTablacodigoDetalleDTO> List()
        {
            List<AudTablacodigoDetalleDTO> entitys = new List<AudTablacodigoDetalleDTO>();
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

        public List<AudTablacodigoDetalleDTO> GetByCriteria(int tabccodi)
        {
            List<AudTablacodigoDetalleDTO> entitys = new List<AudTablacodigoDetalleDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, tabccodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
