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
    /// Clase de acceso a datos de la tabla ST_ELEMENTO_COMPENSADO
    /// </summary>
    public class StElementoCompensadoRepository: RepositoryBase, IStElementoCompensadoRepository
    {
        public StElementoCompensadoRepository(string strConn): base(strConn)
        {
        }

        StElementoCompensadoHelper helper = new StElementoCompensadoHelper();

        public int Save(StElementoCompensadoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Elecmpcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            dbProvider.AddInParameter(command, helper.Stfactcodi, DbType.Int32, entity.Stfactcodi);
            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, entity.Stcompcodi);
            dbProvider.AddInParameter(command, helper.Elecmpmonto, DbType.Decimal, entity.Elecmpmonto);
            dbProvider.AddInParameter(command, helper.Elecmpusucreacion, DbType.String, entity.Elecmpusucreacion);
            dbProvider.AddInParameter(command, helper.Elecmpfeccreacion, DbType.DateTime, entity.Elecmpfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(StElementoCompensadoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            dbProvider.AddInParameter(command, helper.Stfactcodi, DbType.Int32, entity.Stfactcodi);
            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, entity.Stcompcodi);
            dbProvider.AddInParameter(command, helper.Elecmpmonto, DbType.Decimal, entity.Elecmpmonto);
            dbProvider.AddInParameter(command, helper.Elecmpusucreacion, DbType.String, entity.Elecmpusucreacion);
            dbProvider.AddInParameter(command, helper.Elecmpfeccreacion, DbType.DateTime, entity.Elecmpfeccreacion);
            dbProvider.AddInParameter(command, helper.Elecmpcodi, DbType.Int32, entity.Elecmpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public StElementoCompensadoDTO GetById(int elecmpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Elecmpcodi, DbType.Int32, elecmpcodi);
            StElementoCompensadoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StElementoCompensadoDTO> List()
        {
            List<StElementoCompensadoDTO> entitys = new List<StElementoCompensadoDTO>();
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

        public List<StElementoCompensadoDTO> GetByCriteria()
        {
            List<StElementoCompensadoDTO> entitys = new List<StElementoCompensadoDTO>();
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
