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
    /// Clase de acceso a datos de la tabla PR_GRUPOXCNFBAR
    /// </summary>
    public class PrGrupoxcnfbarRepository : RepositoryBase, IPrGrupoxcnfbarRepository
    {
        public PrGrupoxcnfbarRepository(string strConn)
            : base(strConn)
        {
        }

        PrGrupoxcnfbarHelper helper = new PrGrupoxcnfbarHelper();

        public int Save(PrGrupoxcnfbarDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Grcnfbcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cnfbarcodi, DbType.Int32, entity.Cnfbarcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Grcnfbestado, DbType.Int32, entity.Grcnfbestado);
            dbProvider.AddInParameter(command, helper.Grcnfbusucreacion, DbType.String, entity.Grcnfbusucreacion);
            dbProvider.AddInParameter(command, helper.Grcnfbfeccreacion, DbType.DateTime, entity.Grcnfbfeccreacion);
            dbProvider.AddInParameter(command, helper.Grcnfbusumodificacion, DbType.String, entity.Grcnfbusumodificacion);
            dbProvider.AddInParameter(command, helper.Grcnfbfecmodificacion, DbType.DateTime, entity.Grcnfbfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrGrupoxcnfbarDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cnfbarcodi, DbType.Int32, entity.Cnfbarcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Grcnfbestado, DbType.Int32, entity.Grcnfbestado);
            dbProvider.AddInParameter(command, helper.Grcnfbusucreacion, DbType.String, entity.Grcnfbusucreacion);
            dbProvider.AddInParameter(command, helper.Grcnfbfeccreacion, DbType.DateTime, entity.Grcnfbfeccreacion);
            dbProvider.AddInParameter(command, helper.Grcnfbusumodificacion, DbType.String, entity.Grcnfbusumodificacion);
            dbProvider.AddInParameter(command, helper.Grcnfbfecmodificacion, DbType.DateTime, entity.Grcnfbfecmodificacion);

            dbProvider.AddInParameter(command, helper.Grcnfbcodi, DbType.Int32, entity.Grcnfbcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(PrGrupoxcnfbarDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Grcnfbfecmodificacion, DbType.DateTime, entity.Grcnfbfecmodificacion);
            dbProvider.AddInParameter(command, helper.Grcnfbusumodificacion, DbType.String, entity.Grcnfbusumodificacion);

            dbProvider.AddInParameter(command, helper.Grcnfbcodi, DbType.Int32, entity.Grcnfbcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrGrupoxcnfbarDTO GetById(int grcnfbcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Grcnfbcodi, DbType.Int32, grcnfbcodi);
            PrGrupoxcnfbarDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public PrGrupoxcnfbarDTO GetByGrupocodi(int grupocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByGrupocodi);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            PrGrupoxcnfbarDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrGrupoxcnfbarDTO> List()
        {
            List<PrGrupoxcnfbarDTO> entitys = new List<PrGrupoxcnfbarDTO>();
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

        public List<PrGrupoxcnfbarDTO> GetByCriteria()
        {
            List<PrGrupoxcnfbarDTO> entitys = new List<PrGrupoxcnfbarDTO>();
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
