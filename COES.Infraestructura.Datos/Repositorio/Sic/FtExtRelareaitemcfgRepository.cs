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
    /// Clase de acceso a datos de la tabla FT_EXT_RELAREAITEMCFG
    /// </summary>
    public class FtExtRelareaitemcfgRepository: RepositoryBase, IFtExtRelareaitemcfgRepository
    {
        public FtExtRelareaitemcfgRepository(string strConn): base(strConn)
        {
        }

        FtExtRelareaitemcfgHelper helper = new FtExtRelareaitemcfgHelper();

        public int Save(FtExtRelareaitemcfgDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Friacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Faremcodi, DbType.Int32, entity.Faremcodi);
            dbProvider.AddInParameter(command, helper.Ftitcodi, DbType.Int32, entity.Ftitcodi);
            dbProvider.AddInParameter(command, helper.Friaestado, DbType.String, entity.Friaestado);
            dbProvider.AddInParameter(command, helper.Friafeccreacion, DbType.DateTime, entity.Friafeccreacion);
            dbProvider.AddInParameter(command, helper.Friausucreacion, DbType.String, entity.Friausucreacion);
            dbProvider.AddInParameter(command, helper.Friafecmodificacion, DbType.DateTime, entity.Friafecmodificacion);
            dbProvider.AddInParameter(command, helper.Friausumodificacion, DbType.String, entity.Friausumodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FtExtRelareaitemcfgDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Faremcodi, DbType.Int32, entity.Faremcodi);
            dbProvider.AddInParameter(command, helper.Ftitcodi, DbType.Int32, entity.Ftitcodi);
            dbProvider.AddInParameter(command, helper.Friaestado, DbType.String, entity.Friaestado);
            dbProvider.AddInParameter(command, helper.Friafeccreacion, DbType.DateTime, entity.Friafeccreacion);
            dbProvider.AddInParameter(command, helper.Friausucreacion, DbType.String, entity.Friausucreacion);
            dbProvider.AddInParameter(command, helper.Friafecmodificacion, DbType.DateTime, entity.Friafecmodificacion);
            dbProvider.AddInParameter(command, helper.Friausumodificacion, DbType.String, entity.Friausumodificacion);
            dbProvider.AddInParameter(command, helper.Friacodi, DbType.Int32, entity.Friacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int friacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Friacodi, DbType.Int32, friacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtRelareaitemcfgDTO GetById(int friacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Friacodi, DbType.Int32, friacodi);
            FtExtRelareaitemcfgDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtRelareaitemcfgDTO> List()
        {
            List<FtExtRelareaitemcfgDTO> entitys = new List<FtExtRelareaitemcfgDTO>();
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

        public List<FtExtRelareaitemcfgDTO> GetByCriteria()
        {
            List<FtExtRelareaitemcfgDTO> entitys = new List<FtExtRelareaitemcfgDTO>();
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

        public List<FtExtRelareaitemcfgDTO> ListarPorAreas(string estado, string famercodis)
        {
            List<FtExtRelareaitemcfgDTO> entitys = new List<FtExtRelareaitemcfgDTO>();
            string sql = string.Format(helper.SqlListarPorAreas, estado, famercodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtRelareaitemcfgDTO entity = helper.Create(dr);

                    int iFaremnombre = dr.GetOrdinal(helper.Faremnombre);
                    if (!dr.IsDBNull(iFaremnombre)) entity.Faremnombre = dr.GetString(iFaremnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        
    }
}
