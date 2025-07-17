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
    /// Clase de acceso a datos de la tabla SI_PLAZOENVIO
    /// </summary>
    public class SiPlazoenvioRepository : RepositoryBase, ISiPlazoenvioRepository
    {
        public SiPlazoenvioRepository(string strConn)
            : base(strConn)
        {
        }

        SiPlazoenvioHelper helper = new SiPlazoenvioHelper();

        public int Save(SiPlazoenvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Plazcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Fdatcodi, DbType.Int32, entity.Fdatcodi);
            dbProvider.AddInParameter(command, helper.Plazperiodo, DbType.Int32, entity.Plazperiodo);
            dbProvider.AddInParameter(command, helper.Plazinimin, DbType.Int32, entity.Plazinimin);
            dbProvider.AddInParameter(command, helper.Plazinidia, DbType.Int32, entity.Plazinidia);
            dbProvider.AddInParameter(command, helper.Plazfindia, DbType.Int32, entity.Plazfindia);
            dbProvider.AddInParameter(command, helper.Plazfinmin, DbType.Int32, entity.Plazfinmin);
            dbProvider.AddInParameter(command, helper.Plazfueradia, DbType.Int32, entity.Plazfueradia);
            dbProvider.AddInParameter(command, helper.Plazfueramin, DbType.Int32, entity.Plazfueramin);
            dbProvider.AddInParameter(command, helper.Plazusucreacion, DbType.String, entity.Plazusucreacion);
            dbProvider.AddInParameter(command, helper.Plazfeccreacion, DbType.DateTime, entity.Plazfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiPlazoenvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Plazperiodo, DbType.Int32, entity.Plazperiodo);
            dbProvider.AddInParameter(command, helper.Plazinimin, DbType.Int32, entity.Plazinimin);
            dbProvider.AddInParameter(command, helper.Plazinidia, DbType.Int32, entity.Plazinidia);
            dbProvider.AddInParameter(command, helper.Plazfindia, DbType.Int32, entity.Plazfindia);
            dbProvider.AddInParameter(command, helper.Plazfinmin, DbType.Int32, entity.Plazfinmin);
            dbProvider.AddInParameter(command, helper.Plazfueradia, DbType.Int32, entity.Plazfueradia);
            dbProvider.AddInParameter(command, helper.Plazfueramin, DbType.Int32, entity.Plazfueramin);
            dbProvider.AddInParameter(command, helper.Plazusumodificacion, DbType.String, entity.Plazusumodificacion);
            dbProvider.AddInParameter(command, helper.Plazfecmodificacion, DbType.DateTime, entity.Plazfecmodificacion);

            dbProvider.AddInParameter(command, helper.Plazcodi, DbType.Int32, entity.Plazcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int plazcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Plazcodi, DbType.Int32, plazcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiPlazoenvioDTO GetById(int plazcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Plazcodi, DbType.Int32, plazcodi);
            SiPlazoenvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iIFdatnombre = dr.GetOrdinal(helper.Fdatnombre);
                    if (!dr.IsDBNull(iIFdatnombre)) entity.Fdatnombre = dr.GetString(iIFdatnombre);
                }
            }

            return entity;
        }

        public SiPlazoenvioDTO GetByFdatcodi(int fdatcodi)
        {
            string queryString = string.Format(helper.SqlGetByFdatcodi, fdatcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            SiPlazoenvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iIFdatnombre = dr.GetOrdinal(helper.Fdatnombre);
                    if (!dr.IsDBNull(iIFdatnombre)) entity.Fdatnombre = dr.GetString(iIFdatnombre);
                }
            }

            return entity;
        }

        public List<SiPlazoenvioDTO> List()
        {
            List<SiPlazoenvioDTO> entitys = new List<SiPlazoenvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            SiPlazoenvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iIFdatnombre = dr.GetOrdinal(helper.Fdatnombre);
                    if (!dr.IsDBNull(iIFdatnombre)) entity.Fdatnombre = dr.GetString(iIFdatnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiPlazoenvioDTO> GetByCriteria()
        {
            List<SiPlazoenvioDTO> entitys = new List<SiPlazoenvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            SiPlazoenvioDTO entity = null;

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
