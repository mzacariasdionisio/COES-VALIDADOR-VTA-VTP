using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CAI_PRESUPUESTO
    /// </summary>
    public class CaiPresupuestoRepository: RepositoryBase, ICaiPresupuestoRepository
    {
        public CaiPresupuestoRepository(string strConn): base(strConn)
        {
        }

        CaiPresupuestoHelper helper = new CaiPresupuestoHelper();

        public int Save(CaiPresupuestoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Caiprscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Caiprsanio, DbType.Int32, entity.Caiprsanio);
            dbProvider.AddInParameter(command, helper.Caiprsmesinicio, DbType.Int32, entity.Caiprsmesinicio);
            dbProvider.AddInParameter(command, helper.Caiprsnromeses, DbType.Int32, entity.Caiprsnromeses);
            dbProvider.AddInParameter(command, helper.Caiprsnombre, DbType.String, entity.Caiprsnombre);
            dbProvider.AddInParameter(command, helper.Caiprsusucreacion, DbType.String, entity.Caiprsusucreacion);
            dbProvider.AddInParameter(command, helper.Caiprsfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Caiprsusumodificacion, DbType.String, entity.Caiprsusumodificacion);
            dbProvider.AddInParameter(command, helper.Caiprsfecmodificacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CaiPresupuestoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Caiprsanio, DbType.Int32, entity.Caiprsanio);
            dbProvider.AddInParameter(command, helper.Caiprsmesinicio, DbType.Int32, entity.Caiprsmesinicio);
            dbProvider.AddInParameter(command, helper.Caiprsnromeses, DbType.Int32, entity.Caiprsnromeses);
            dbProvider.AddInParameter(command, helper.Caiprsnombre, DbType.String, entity.Caiprsnombre);
            dbProvider.AddInParameter(command, helper.Caiprsusumodificacion, DbType.String, entity.Caiprsusumodificacion);
            dbProvider.AddInParameter(command, helper.Caiprsfecmodificacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Caiprscodi, DbType.Int32, entity.Caiprscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int caiprscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Caiprscodi, DbType.Int32, caiprscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CaiPresupuestoDTO GetById(int caiprscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Caiprscodi, DbType.Int32, caiprscodi);
            CaiPresupuestoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CaiPresupuestoDTO> List()
        {
            List<CaiPresupuestoDTO> entitys = new List<CaiPresupuestoDTO>();
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

        public List<CaiPresupuestoDTO> GetByCriteria()
        {
            List<CaiPresupuestoDTO> entitys = new List<CaiPresupuestoDTO>();
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
