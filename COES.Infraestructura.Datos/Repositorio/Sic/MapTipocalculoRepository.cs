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
    /// Clase de acceso a datos de la tabla MAP_TIPOCALCULO
    /// </summary>
    public class MapTipocalculoRepository: RepositoryBase, IMapTipocalculoRepository
    {
        public MapTipocalculoRepository(string strConn): base(strConn)
        {
        }

        MapTipocalculoHelper helper = new MapTipocalculoHelper();

        public int Save(MapTipocalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tipoccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tipocdesc, DbType.String, entity.Tipocdesc);
            dbProvider.AddInParameter(command, helper.Tipocabrev, DbType.String, entity.Tipocabrev);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MapTipocalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tipoccodi, DbType.Int32, entity.Tipoccodi);
            dbProvider.AddInParameter(command, helper.Tipocdesc, DbType.String, entity.Tipocdesc);
            dbProvider.AddInParameter(command, helper.Tipocabrev, DbType.String, entity.Tipocabrev);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tipoccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tipoccodi, DbType.Int32, tipoccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MapTipocalculoDTO GetById(int tipoccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tipoccodi, DbType.Int32, tipoccodi);
            MapTipocalculoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MapTipocalculoDTO> List()
        {
            List<MapTipocalculoDTO> entitys = new List<MapTipocalculoDTO>();
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

        public List<MapTipocalculoDTO> GetByCriteria()
        {
            List<MapTipocalculoDTO> entitys = new List<MapTipocalculoDTO>();
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
