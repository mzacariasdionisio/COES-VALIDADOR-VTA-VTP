using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CPA_SDDP
    /// </summary>
    public class CpaSddpRepository : RepositoryBase, ICpaSddpRepository
    {
        public CpaSddpRepository(string strConn)
            : base(strConn)
        {
        }

        CpaSddpHelper helper = new CpaSddpHelper();

        public int Save(CpaSddpDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpsddpcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Cpsddpcorrelativo, DbType.Int32, entity.Cpsddpcorrelativo);
            dbProvider.AddInParameter(command, helper.Cpsddpnomarchivo, DbType.String, entity.Cpsddpnomarchivo);
            dbProvider.AddInParameter(command, helper.Cpsddpsemanaini, DbType.Int32, entity.Cpsddpsemanaini);
            dbProvider.AddInParameter(command, helper.Cpsddpanioini, DbType.Int32, entity.Cpsddpanioini);
            dbProvider.AddInParameter(command, helper.Cpsddpnroseries, DbType.Int32, entity.Cpsddpnroseries);
            dbProvider.AddInParameter(command, helper.Cpsddpdiainicio, DbType.DateTime, entity.Cpsddpdiainicio);
            dbProvider.AddInParameter(command, helper.Cpsddpusucreacion, DbType.String, entity.Cpsddpusucreacion);
            dbProvider.AddInParameter(command, helper.Cpsddpfeccreacion, DbType.DateTime, entity.Cpsddpfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CpaSddpDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Cpsddpcorrelativo, DbType.Int32, entity.Cpsddpcorrelativo);
            dbProvider.AddInParameter(command, helper.Cpsddpnomarchivo, DbType.String, entity.Cpsddpnomarchivo);
            dbProvider.AddInParameter(command, helper.Cpsddpsemanaini, DbType.Int32, entity.Cpsddpsemanaini);
            dbProvider.AddInParameter(command, helper.Cpsddpanioini, DbType.Int32, entity.Cpsddpanioini);
            dbProvider.AddInParameter(command, helper.Cpsddpnroseries, DbType.Int32, entity.Cpsddpnroseries);
            dbProvider.AddInParameter(command, helper.Cpsddpdiainicio, DbType.DateTime, entity.Cpsddpdiainicio);
            dbProvider.AddInParameter(command, helper.Cpsddpusucreacion, DbType.String, entity.Cpsddpusucreacion);
            dbProvider.AddInParameter(command, helper.Cpsddpfeccreacion, DbType.DateTime, entity.Cpsddpfeccreacion);
            dbProvider.AddInParameter(command, helper.Cpsddpcodi, DbType.Int32, entity.Cpsddpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpsddpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpsddpcodi, DbType.Int32, cpsddpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpaSddpDTO GetById(int Cparcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, Cparcodi);
            CpaSddpDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpaSddpDTO> List()
        {
            List<CpaSddpDTO> entities = new List<CpaSddpDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

        public int GetByCorrelativoSddp(int Cparcodi)
        {
            int idCorrelativo = 1;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxCorrelativo);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, Cparcodi);
            object result = dbProvider.ExecuteScalar(command);
            if (result != null) idCorrelativo = Convert.ToInt32(result);

            return idCorrelativo;
        }
    }
}
