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
    /// Clase de acceso a datos de la tabla MAP_EMPSINREP
    /// </summary>
    public class MapEmpsinrepRepository: RepositoryBase, IMapEmpsinrepRepository
    {
        public MapEmpsinrepRepository(string strConn): base(strConn)
        {
        }

        MapEmpsinrepHelper helper = new MapEmpsinrepHelper();

        public int Save(MapEmpsinrepDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Empsrcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Mediccodi, DbType.Int32, entity.Mediccodi);
            dbProvider.AddInParameter(command, helper.Empsrperiodo, DbType.DateTime, entity.Empsrperiodo);
            dbProvider.AddInParameter(command, helper.Empsrfecha, DbType.DateTime, entity.Empsrfecha);
            dbProvider.AddInParameter(command, helper.Empsrusucreacion, DbType.String, entity.Empsrusucreacion);
            dbProvider.AddInParameter(command, helper.Empsrfeccreacion, DbType.DateTime, entity.Empsrfeccreacion);
            dbProvider.AddInParameter(command, helper.Empsrusumodificacion, DbType.String, entity.Empsrusumodificacion);
            dbProvider.AddInParameter(command, helper.Empsrfecmodificacion, DbType.DateTime, entity.Empsrfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MapEmpsinrepDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Empsrcodi, DbType.Int32, entity.Empsrcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Mediccodi, DbType.Int32, entity.Mediccodi);
            dbProvider.AddInParameter(command, helper.Empsrperiodo, DbType.DateTime, entity.Empsrperiodo);
            dbProvider.AddInParameter(command, helper.Empsrfecha, DbType.DateTime, entity.Empsrfecha);
            dbProvider.AddInParameter(command, helper.Empsrusucreacion, DbType.String, entity.Empsrusucreacion);
            dbProvider.AddInParameter(command, helper.Empsrfeccreacion, DbType.DateTime, entity.Empsrfeccreacion);
            dbProvider.AddInParameter(command, helper.Empsrusumodificacion, DbType.String, entity.Empsrusumodificacion);
            dbProvider.AddInParameter(command, helper.Empsrfecmodificacion, DbType.DateTime, entity.Empsrfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int empsrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Empsrcodi, DbType.Int32, empsrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MapEmpsinrepDTO GetById(int empsrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Empsrcodi, DbType.Int32, empsrcodi);
            MapEmpsinrepDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MapEmpsinrepDTO> List()
        {
            List<MapEmpsinrepDTO> entitys = new List<MapEmpsinrepDTO>();
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

        public List<MapEmpsinrepDTO> GetByCriteria()
        {
            List<MapEmpsinrepDTO> entitys = new List<MapEmpsinrepDTO>();
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
