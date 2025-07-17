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
    /// Clase de acceso a datos de la tabla MAP_VERSION
    /// </summary>
    public class MapVersionRepository: RepositoryBase, IMapVersionRepository
    {
        public MapVersionRepository(string strConn): base(strConn)
        {
        }

        MapVersionHelper helper = new MapVersionHelper();

        public int Save(MapVersionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vermcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vermfechaperiodo, DbType.DateTime, entity.Vermfechaperiodo);
            dbProvider.AddInParameter(command, helper.Vermusucreacion, DbType.String, entity.Vermusucreacion);
            dbProvider.AddInParameter(command, helper.Vermestado, DbType.Int32, entity.Vermestado);
            dbProvider.AddInParameter(command, helper.Vermfeccreacion, DbType.DateTime, entity.Vermfeccreacion);
            dbProvider.AddInParameter(command, helper.Vermusumodificacion, DbType.String, entity.Vermusumodificacion);
            dbProvider.AddInParameter(command, helper.Vermfecmodificacion, DbType.DateTime, entity.Vermfecmodificacion);
            dbProvider.AddInParameter(command, helper.Vermnumero, DbType.Int32, entity.Vermnumero);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MapVersionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vermfechaperiodo, DbType.DateTime, entity.Vermfechaperiodo);
            dbProvider.AddInParameter(command, helper.Vermusucreacion, DbType.String, entity.Vermusucreacion);
            dbProvider.AddInParameter(command, helper.Vermestado, DbType.Int32, entity.Vermestado);
            dbProvider.AddInParameter(command, helper.Vermfeccreacion, DbType.DateTime, entity.Vermfeccreacion);
            dbProvider.AddInParameter(command, helper.Vermusumodificacion, DbType.String, entity.Vermusumodificacion);
            dbProvider.AddInParameter(command, helper.Vermfecmodificacion, DbType.DateTime, entity.Vermfecmodificacion);
            dbProvider.AddInParameter(command, helper.Vermnumero, DbType.Int32, entity.Vermnumero);
            dbProvider.AddInParameter(command, helper.Vermcodi, DbType.Int32, entity.Vermcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vermcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vermcodi, DbType.Int32, vermcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MapVersionDTO GetById(int vermcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vermcodi, DbType.Int32, vermcodi);
            MapVersionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MapVersionDTO> List()
        {
            List<MapVersionDTO> entitys = new List<MapVersionDTO>();
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

        public List<MapVersionDTO> GetByCriteria()
        {
            List<MapVersionDTO> entitys = new List<MapVersionDTO>();
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

        public int GetMaxVermnumero(DateTime fecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlGetMaxVermnumero,fecha.ToString(ConstantesBase.FormatoFecha)));
            object result = dbProvider.ExecuteScalar(command);
            int vernum = 1;
            if (result != null) vernum = Convert.ToInt32(result);

            return vernum;
        }
    }
}
