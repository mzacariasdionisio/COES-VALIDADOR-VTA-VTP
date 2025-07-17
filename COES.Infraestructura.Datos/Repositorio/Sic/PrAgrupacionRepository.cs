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
    /// Clase de acceso a datos de la tabla PR_AGRUPACION
    /// </summary>
    public class PrAgrupacionRepository : RepositoryBase, IPrAgrupacionRepository
    {
        public PrAgrupacionRepository(string strConn)
            : base(strConn)
        {
        }

        PrAgrupacionHelper helper = new PrAgrupacionHelper();

        public int Save(PrAgrupacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Agrupcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Agrupnombre, DbType.String, entity.Agrupnombre);
            dbProvider.AddInParameter(command, helper.Agrupusucreacion, DbType.String, entity.Agrupusucreacion);
            dbProvider.AddInParameter(command, helper.Agrupfeccreacion, DbType.DateTime, entity.Agrupfeccreacion);
            dbProvider.AddInParameter(command, helper.Agrupusumodificacion, DbType.String, entity.Agrupusumodificacion);
            dbProvider.AddInParameter(command, helper.Agrupfecmodificacion, DbType.DateTime, entity.Agrupfecmodificacion);
            dbProvider.AddInParameter(command, helper.Agrupestado, DbType.String, entity.Agrupestado);
            dbProvider.AddInParameter(command, helper.Agrupfuente, DbType.Int32, entity.Agrupfuente);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrAgrupacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Agrupnombre, DbType.String, entity.Agrupnombre);
            dbProvider.AddInParameter(command, helper.Agrupusucreacion, DbType.String, entity.Agrupusucreacion);
            dbProvider.AddInParameter(command, helper.Agrupfeccreacion, DbType.DateTime, entity.Agrupfeccreacion);
            dbProvider.AddInParameter(command, helper.Agrupusumodificacion, DbType.String, entity.Agrupusumodificacion);
            dbProvider.AddInParameter(command, helper.Agrupfecmodificacion, DbType.DateTime, entity.Agrupfecmodificacion);
            dbProvider.AddInParameter(command, helper.Agrupestado, DbType.String, entity.Agrupestado);
            //dbProvider.AddInParameter(command, helper.Agrupfuente, DbType.Int32, entity.Agrupfuente);
            dbProvider.AddInParameter(command, helper.Agrupcodi, DbType.Int32, entity.Agrupcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(PrAgrupacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Agrupusumodificacion, DbType.String, entity.Agrupusumodificacion);
            dbProvider.AddInParameter(command, helper.Agrupfecmodificacion, DbType.DateTime, entity.Agrupfecmodificacion);

            dbProvider.AddInParameter(command, helper.Agrupcodi, DbType.Int32, entity.Agrupcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrAgrupacionDTO GetById(int agrupcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Agrupcodi, DbType.Int32, agrupcodi);
            PrAgrupacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrAgrupacionDTO> List()
        {
            List<PrAgrupacionDTO> entitys = new List<PrAgrupacionDTO>();
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

        public List<PrAgrupacionDTO> GetByCriteria(int agrupfuente)
        {
            List<PrAgrupacionDTO> entitys = new List<PrAgrupacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.Agrupfuente, DbType.Int32, agrupfuente);

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
