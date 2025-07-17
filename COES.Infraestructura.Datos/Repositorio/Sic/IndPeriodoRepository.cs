using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla IND_PERIODO
    /// </summary>
    public class IndPeriodoRepository : RepositoryBase, IIndPeriodoRepository
    {
        public IndPeriodoRepository(string strConn) : base(strConn)
        {
        }

        IndPeriodoHelper helper = new IndPeriodoHelper();

        public int Save(IndPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ipericodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Iperinombre, DbType.String, entity.Iperinombre);
            dbProvider.AddInParameter(command, helper.Iperianio, DbType.Int32, entity.Iperianio);
            dbProvider.AddInParameter(command, helper.Iperimes, DbType.Int32, entity.Iperimes);
            dbProvider.AddInParameter(command, helper.Iperianiofin, DbType.Int32, entity.Iperianiofin);
            dbProvider.AddInParameter(command, helper.Iperimesfin, DbType.Int32, entity.Iperimesfin);
            dbProvider.AddInParameter(command, helper.Iperianiomes, DbType.Int32, entity.Iperianiomes);
            dbProvider.AddInParameter(command, helper.Iperiestado, DbType.String, entity.Iperiestado);
            dbProvider.AddInParameter(command, helper.Iperihorizonte, DbType.String, entity.Iperihorizonte);
            dbProvider.AddInParameter(command, helper.Iperiusucreacion, DbType.String, entity.Iperiusucreacion);
            dbProvider.AddInParameter(command, helper.Iperifeccreacion, DbType.DateTime, entity.Iperifeccreacion);
            dbProvider.AddInParameter(command, helper.Iperiusumodificacion, DbType.String, entity.Iperiusumodificacion);
            dbProvider.AddInParameter(command, helper.Iperifecmodificacion, DbType.DateTime, entity.Iperifecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(IndPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ipericodi, DbType.Int32, entity.Ipericodi);
            dbProvider.AddInParameter(command, helper.Iperinombre, DbType.String, entity.Iperinombre);
            dbProvider.AddInParameter(command, helper.Iperianio, DbType.Int32, entity.Iperianio);
            dbProvider.AddInParameter(command, helper.Iperimes, DbType.Int32, entity.Iperimes);
            dbProvider.AddInParameter(command, helper.Iperianiomes, DbType.Int32, entity.Iperianiomes);
            dbProvider.AddInParameter(command, helper.Iperiestado, DbType.String, entity.Iperiestado);
            dbProvider.AddInParameter(command, helper.Iperihorizonte, DbType.String, entity.Iperihorizonte);
            dbProvider.AddInParameter(command, helper.Iperiusucreacion, DbType.String, entity.Iperiusucreacion);
            dbProvider.AddInParameter(command, helper.Iperifeccreacion, DbType.DateTime, entity.Iperifeccreacion);
            dbProvider.AddInParameter(command, helper.Iperiusumodificacion, DbType.String, entity.Iperiusumodificacion);
            dbProvider.AddInParameter(command, helper.Iperifecmodificacion, DbType.DateTime, entity.Iperifecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ipericodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ipericodi, DbType.Int32, ipericodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IndPeriodoDTO GetById(int ipericodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ipericodi, DbType.Int32, ipericodi);
            IndPeriodoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<IndPeriodoDTO> List()
        {
            List<IndPeriodoDTO> entitys = new List<IndPeriodoDTO>();
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

        public List<IndPeriodoDTO> GetByCriteria(string horizonte, int anio)
        {
            List<IndPeriodoDTO> entitys = new List<IndPeriodoDTO>();
            string query = string.Format(helper.SqlGetByCriteria, horizonte, anio);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

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
