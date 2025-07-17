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
    /// Clase de acceso a datos de la tabla DAI_TABLACODIGO_DETALLE
    /// </summary>
    public class DaiTablacodigoDetalleRepository: RepositoryBase, IDaiTablacodigoDetalleRepository
    {
        public DaiTablacodigoDetalleRepository(string strConn): base(strConn)
        {
        }

        DaiTablacodigoDetalleHelper helper = new DaiTablacodigoDetalleHelper();

        public int Save(DaiTablacodigoDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tabdcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tabcodi, DbType.Int32, entity.Tabcodi);
            dbProvider.AddInParameter(command, helper.Tabddescripcion, DbType.String, entity.Tabddescripcion);
            dbProvider.AddInParameter(command, helper.Tabdvalor, DbType.String, entity.Tabdvalor);
            dbProvider.AddInParameter(command, helper.Tabdorden, DbType.Int32, entity.Tabdorden);
            dbProvider.AddInParameter(command, helper.Tabdactivo, DbType.String, entity.Tabdactivo);
            dbProvider.AddInParameter(command, helper.Tabdusucreacion, DbType.String, entity.Tabdusucreacion);
            dbProvider.AddInParameter(command, helper.Tabdfeccreacion, DbType.DateTime, entity.Tabdfeccreacion);
            dbProvider.AddInParameter(command, helper.Tabdusumodificacion, DbType.String, entity.Tabdusumodificacion);
            dbProvider.AddInParameter(command, helper.Tabdfecmodificacion, DbType.DateTime, entity.Tabdfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(DaiTablacodigoDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tabcodi, DbType.Int32, entity.Tabcodi);
            dbProvider.AddInParameter(command, helper.Tabddescripcion, DbType.String, entity.Tabddescripcion);
            dbProvider.AddInParameter(command, helper.Tabdvalor, DbType.String, entity.Tabdvalor);
            dbProvider.AddInParameter(command, helper.Tabdorden, DbType.Int32, entity.Tabdorden);
            dbProvider.AddInParameter(command, helper.Tabdactivo, DbType.String, entity.Tabdactivo);
            dbProvider.AddInParameter(command, helper.Tabdusucreacion, DbType.String, entity.Tabdusucreacion);
            dbProvider.AddInParameter(command, helper.Tabdfeccreacion, DbType.DateTime, entity.Tabdfeccreacion);
            dbProvider.AddInParameter(command, helper.Tabdusumodificacion, DbType.String, entity.Tabdusumodificacion);
            dbProvider.AddInParameter(command, helper.Tabdfecmodificacion, DbType.DateTime, entity.Tabdfecmodificacion);
            dbProvider.AddInParameter(command, helper.Tabdcodi, DbType.Int32, entity.Tabdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tabdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tabdcodi, DbType.Int32, tabdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public DaiTablacodigoDetalleDTO GetById(int tabdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tabdcodi, DbType.Int32, tabdcodi);
            DaiTablacodigoDetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<DaiTablacodigoDetalleDTO> List()
        {
            List<DaiTablacodigoDetalleDTO> entitys = new List<DaiTablacodigoDetalleDTO>();
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

        public List<DaiTablacodigoDetalleDTO> GetByCriteria()
        {
            List<DaiTablacodigoDetalleDTO> entitys = new List<DaiTablacodigoDetalleDTO>();
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
