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
    /// Clase de acceso a datos de la tabla RE_INTERRUPCION_INSUMO
    /// </summary>
    public class ReInterrupcionInsumoRepository: RepositoryBase, IReInterrupcionInsumoRepository
    {
        public ReInterrupcionInsumoRepository(string strConn): base(strConn)
        {
        }

        ReInterrupcionInsumoHelper helper = new ReInterrupcionInsumoHelper();

        public int Save(ReInterrupcionInsumoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Reinincodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Reinincorrelativo, DbType.Int32, entity.Reinincorrelativo);
            dbProvider.AddInParameter(command, helper.Repentcodi, DbType.Int32, entity.Repentcodi);
            dbProvider.AddInParameter(command, helper.Reininifecinicio, DbType.DateTime, entity.Reininifecinicio);
            dbProvider.AddInParameter(command, helper.Reininfecfin, DbType.DateTime, entity.Reininfecfin);
            dbProvider.AddInParameter(command, helper.Reininprogifecinicio, DbType.DateTime, entity.Reininprogifecinicio);
            dbProvider.AddInParameter(command, helper.Reininprogfecfin, DbType.DateTime, entity.Reininprogfecfin);
            dbProvider.AddInParameter(command, helper.Retintcodi, DbType.Int32, entity.Retintcodi);
            dbProvider.AddInParameter(command, helper.Reninitipo, DbType.String, entity.Reninitipo);
            dbProvider.AddInParameter(command, helper.Reninicausa, DbType.String, entity.Reninicausa);
            dbProvider.AddInParameter(command, helper.Recintcodi, DbType.Int32, entity.Recintcodi);
            dbProvider.AddInParameter(command, helper.Reinincodosi, DbType.String, entity.Reinincodosi);
            dbProvider.AddInParameter(command, helper.Reinincliente, DbType.Int32, entity.Reinincliente);
            dbProvider.AddInParameter(command, helper.Reininsuministrador, DbType.Int32, entity.Reininsuministrador);
            dbProvider.AddInParameter(command, helper.Reininobservacion, DbType.String, entity.Reininobservacion);
            dbProvider.AddInParameter(command, helper.Reininresponsable1, DbType.Int32, entity.Reininresponsable1);
            dbProvider.AddInParameter(command, helper.Reininporcentaje1, DbType.Decimal, entity.Reininporcentaje1);
            dbProvider.AddInParameter(command, helper.Reininresponsable2, DbType.Int32, entity.Reininresponsable2);
            dbProvider.AddInParameter(command, helper.Reininporcentaje2, DbType.Decimal, entity.Reininporcentaje2);
            dbProvider.AddInParameter(command, helper.Reininresponsable3, DbType.Int32, entity.Reininresponsable3);
            dbProvider.AddInParameter(command, helper.Reininporcentaje3, DbType.Decimal, entity.Reininporcentaje3);
            dbProvider.AddInParameter(command, helper.Reininresponsable4, DbType.Int32, entity.Reininresponsable4);
            dbProvider.AddInParameter(command, helper.Reininporcentaje4, DbType.Decimal, entity.Reininporcentaje4);
            dbProvider.AddInParameter(command, helper.Reininresponsable5, DbType.Int32, entity.Reininresponsable5);
            dbProvider.AddInParameter(command, helper.Reininporcentaje5, DbType.Decimal, entity.Reininporcentaje5);
            dbProvider.AddInParameter(command, helper.Reininusucreacion, DbType.String, entity.Reininusucreacion);
            dbProvider.AddInParameter(command, helper.Reininfeccreacion, DbType.DateTime, entity.Reininfeccreacion);
            dbProvider.AddInParameter(command, helper.Reininusumodificacion, DbType.String, entity.Reininusumodificacion);
            dbProvider.AddInParameter(command, helper.Reininfecmodificacion, DbType.DateTime, entity.Reininfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ReInterrupcionInsumoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Reinincodi, DbType.Int32, entity.Reinincodi);
            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Reinincorrelativo, DbType.Int32, entity.Reinincorrelativo);
            dbProvider.AddInParameter(command, helper.Repentcodi, DbType.Int32, entity.Repentcodi);
            dbProvider.AddInParameter(command, helper.Reininifecinicio, DbType.DateTime, entity.Reininifecinicio);
            dbProvider.AddInParameter(command, helper.Reininfecfin, DbType.DateTime, entity.Reininfecfin);
            dbProvider.AddInParameter(command, helper.Reininprogifecinicio, DbType.DateTime, entity.Reininprogifecinicio);
            dbProvider.AddInParameter(command, helper.Reininprogfecfin, DbType.DateTime, entity.Reininprogfecfin);
            dbProvider.AddInParameter(command, helper.Retintcodi, DbType.Int32, entity.Retintcodi);
            dbProvider.AddInParameter(command, helper.Reninitipo, DbType.String, entity.Reninitipo);
            dbProvider.AddInParameter(command, helper.Reninicausa, DbType.String, entity.Reninicausa);
            dbProvider.AddInParameter(command, helper.Recintcodi, DbType.Int32, entity.Recintcodi);
            dbProvider.AddInParameter(command, helper.Reinincodosi, DbType.String, entity.Reinincodosi);
            dbProvider.AddInParameter(command, helper.Reinincliente, DbType.Int32, entity.Reinincliente);
            dbProvider.AddInParameter(command, helper.Reininsuministrador, DbType.Int32, entity.Reininsuministrador);
            dbProvider.AddInParameter(command, helper.Reininobservacion, DbType.String, entity.Reininobservacion);
            dbProvider.AddInParameter(command, helper.Reininresponsable1, DbType.Int32, entity.Reininresponsable1);
            dbProvider.AddInParameter(command, helper.Reininporcentaje1, DbType.Decimal, entity.Reininporcentaje1);
            dbProvider.AddInParameter(command, helper.Reininresponsable2, DbType.Int32, entity.Reininresponsable2);
            dbProvider.AddInParameter(command, helper.Reininporcentaje2, DbType.Decimal, entity.Reininporcentaje2);
            dbProvider.AddInParameter(command, helper.Reininresponsable3, DbType.Int32, entity.Reininresponsable3);
            dbProvider.AddInParameter(command, helper.Reininporcentaje3, DbType.Decimal, entity.Reininporcentaje3);
            dbProvider.AddInParameter(command, helper.Reininresponsable4, DbType.Int32, entity.Reininresponsable4);
            dbProvider.AddInParameter(command, helper.Reininporcentaje4, DbType.Decimal, entity.Reininporcentaje4);
            dbProvider.AddInParameter(command, helper.Reininresponsable5, DbType.Int32, entity.Reininresponsable5);
            dbProvider.AddInParameter(command, helper.Reininporcentaje5, DbType.Decimal, entity.Reininporcentaje5);
            dbProvider.AddInParameter(command, helper.Reininusucreacion, DbType.String, entity.Reininusucreacion);
            dbProvider.AddInParameter(command, helper.Reininfeccreacion, DbType.DateTime, entity.Reininfeccreacion);
            dbProvider.AddInParameter(command, helper.Reininusumodificacion, DbType.String, entity.Reininusumodificacion);
            dbProvider.AddInParameter(command, helper.Reininfecmodificacion, DbType.DateTime, entity.Reininfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int reinincodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Reinincodi, DbType.Int32, reinincodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ReInterrupcionInsumoDTO GetById(int reinincodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Reinincodi, DbType.Int32, reinincodi);
            ReInterrupcionInsumoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ReInterrupcionInsumoDTO> List()
        {
            List<ReInterrupcionInsumoDTO> entitys = new List<ReInterrupcionInsumoDTO>();
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

        public List<ReInterrupcionInsumoDTO> GetByCriteria()
        {
            List<ReInterrupcionInsumoDTO> entitys = new List<ReInterrupcionInsumoDTO>();
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

        public List<ReInterrupcionInsumoDTO> ObtenerPorPeriodo(int idPeriodo)
        {
            List<ReInterrupcionInsumoDTO> entitys = new List<ReInterrupcionInsumoDTO>();
            string sql = string.Format(helper.SqlObtenerPorPeriodo, idPeriodo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
