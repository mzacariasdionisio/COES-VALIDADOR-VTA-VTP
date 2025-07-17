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
    /// Clase de acceso a datos de la tabla MAP_EMPRESAUL
    /// </summary>
    public class MapDemandaRepository: RepositoryBase, IMapDemandaRepository
    {
        public MapDemandaRepository(string strConn): base(strConn)
        {
        }

        MapDemandaHelper helper = new MapDemandaHelper();

        public int Save(MapDemandaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mapdemcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Mapdemtipo, DbType.Int32, entity.Mapdemtipo);
            dbProvider.AddInParameter(command, helper.Vermcodi, DbType.Int32, entity.Vermcodi);
            dbProvider.AddInParameter(command, helper.Mapdemvalor, DbType.Decimal, entity.Mapdemvalor);
            dbProvider.AddInParameter(command, helper.Mapdemfechaperiodo, DbType.DateTime, entity.Mapdemfechaperiodo);
            dbProvider.AddInParameter(command, helper.Mapdemfecha, DbType.DateTime, entity.Mapdemfecha);
            dbProvider.AddInParameter(command, helper.Mapdemperiodo, DbType.Int32, entity.Mapdemperiodo);
            dbProvider.AddInParameter(command, helper.Mapdemfechafin, DbType.DateTime, entity.Mapdemfechafin);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MapDemandaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mapdemtipo, DbType.Int32, entity.Mapdemtipo);
            dbProvider.AddInParameter(command, helper.Vermcodi, DbType.Int32, entity.Vermcodi);
            dbProvider.AddInParameter(command, helper.Mapdemvalor, DbType.Decimal, entity.Mapdemvalor);
            dbProvider.AddInParameter(command, helper.Mapdemfechaperiodo, DbType.DateTime, entity.Mapdemfechaperiodo);
            dbProvider.AddInParameter(command, helper.Mapdemfecha, DbType.DateTime, entity.Mapdemfecha);
            dbProvider.AddInParameter(command, helper.Mapdemperiodo, DbType.Int32, entity.Mapdemperiodo);
            dbProvider.AddInParameter(command, helper.Mapdemfechafin, DbType.DateTime, entity.Mapdemfechafin);
            dbProvider.AddInParameter(command, helper.Mapdemcodi, DbType.Int32, helper.Mapdemcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mapdemcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mapdemcodi, DbType.Int32, mapdemcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MapDemandaDTO GetById(int mapdemcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mapdemcodi, DbType.Int32, mapdemcodi);
            MapDemandaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MapDemandaDTO> List()
        {
            List<MapDemandaDTO> entitys = new List<MapDemandaDTO>();
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

        public List<MapDemandaDTO> GetByCriteria(int vermcodi)
        {
            string sqlquery = string.Format(helper.SqlGetByCriteria, vermcodi);
            List<MapDemandaDTO> entitys = new List<MapDemandaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlquery);
            MapDemandaDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
