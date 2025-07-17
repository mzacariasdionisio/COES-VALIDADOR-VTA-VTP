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
    /// Clase de acceso a datos de la tabla DAI_PRESUPUESTO
    /// </summary>
    public class DaiPresupuestoRepository: RepositoryBase, IDaiPresupuestoRepository
    {
        public DaiPresupuestoRepository(string strConn): base(strConn)
        {
        }

        DaiPresupuestoHelper helper = new DaiPresupuestoHelper();

        public int Save(DaiPresupuestoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Prescodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Presanio, DbType.String, entity.Presanio);
            dbProvider.AddInParameter(command, helper.Presmonto, DbType.Decimal, entity.Presmonto);
            dbProvider.AddInParameter(command, helper.Presamortizacion, DbType.Int32, entity.Presamortizacion);
            dbProvider.AddInParameter(command, helper.Presinteres, DbType.Decimal, entity.Presinteres);
            dbProvider.AddInParameter(command, helper.Presactivo, DbType.String, entity.Presactivo);
            dbProvider.AddInParameter(command, helper.Presusucreacion, DbType.String, entity.Presusucreacion);
            dbProvider.AddInParameter(command, helper.Presfeccreacion, DbType.DateTime, entity.Presfeccreacion);
            dbProvider.AddInParameter(command, helper.Presusumodificacion, DbType.String, entity.Presusumodificacion);
            dbProvider.AddInParameter(command, helper.Presfecmodificacion, DbType.DateTime, entity.Presfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(DaiPresupuestoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Presanio, DbType.String, entity.Presanio);
            dbProvider.AddInParameter(command, helper.Presmonto, DbType.Decimal, entity.Presmonto);
            dbProvider.AddInParameter(command, helper.Presamortizacion, DbType.Int32, entity.Presamortizacion);
            dbProvider.AddInParameter(command, helper.Presinteres, DbType.Decimal, entity.Presinteres);
            dbProvider.AddInParameter(command, helper.Presusumodificacion, DbType.String, entity.Presusumodificacion);
            dbProvider.AddInParameter(command, helper.Presfecmodificacion, DbType.DateTime, entity.Presfecmodificacion);
            dbProvider.AddInParameter(command, helper.Presprocesada, DbType.String, entity.presprocesada);
            dbProvider.AddInParameter(command, helper.Prescodi, DbType.Int32, entity.Prescodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(DaiPresupuestoDTO presupuesto)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Presusumodificacion, DbType.String, presupuesto.Presusumodificacion);
            dbProvider.AddInParameter(command, helper.Presfecmodificacion, DbType.DateTime, presupuesto.Presfecmodificacion);
            dbProvider.AddInParameter(command, helper.Prescodi, DbType.Int32, presupuesto.Prescodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public DaiPresupuestoDTO GetById(int prescodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Prescodi, DbType.Int32, prescodi);
            DaiPresupuestoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iPresprocesada = dr.GetOrdinal(helper.Presprocesada);
                    if (!dr.IsDBNull(iPresprocesada)) entity.presprocesada = dr.GetString(iPresprocesada);
                }
            }

            return entity;
        }

        public List<DaiPresupuestoDTO> List()
        {
            List<DaiPresupuestoDTO> entitys = new List<DaiPresupuestoDTO>();
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

        public List<DaiPresupuestoDTO> GetByCriteria()
        {
            List<DaiPresupuestoDTO> entitys = new List<DaiPresupuestoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DaiPresupuestoDTO entity = helper.Create(dr);

                    int iTieneaportantes = dr.GetOrdinal(helper.Tieneaportantes);
                    if (!dr.IsDBNull(iTieneaportantes)) entity.Tieneaportantes = Convert.ToInt32(dr.GetValue(iTieneaportantes));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
