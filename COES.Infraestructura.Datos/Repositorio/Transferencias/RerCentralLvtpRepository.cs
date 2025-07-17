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
    /// Clase de acceso a datos de la tabla RER_CENTRAL_LVTP
    /// </summary>
    public class RerCentralLvtpRepository : RepositoryBase, IRerCentralLvtpRepository
    {
        public RerCentralLvtpRepository(string strConn)
            : base(strConn)
        {
        }

        RerCentralLvtpHelper helper = new RerCentralLvtpHelper();

        public int Save(RerCentralLvtpDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rerctpcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rercencodi, DbType.Int32, entity.Rercencodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rerctpusucreacion, DbType.String, entity.Rerctpusucreacion);
            dbProvider.AddInParameter(command, helper.Rerctpfeccreacion, DbType.DateTime, entity.Rerctpfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RerCentralLvtpDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rercencodi, DbType.Int32, entity.Rercencodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rerctpusucreacion, DbType.String, entity.Rerctpusucreacion);
            dbProvider.AddInParameter(command, helper.Rerctpfeccreacion, DbType.DateTime, entity.Rerctpfeccreacion);
            dbProvider.AddInParameter(command, helper.Rerctpcodi, DbType.Int32, entity.Rerctpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rerCtpCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rerctpcodi, DbType.Int32, rerCtpCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RerCentralLvtpDTO GetById(int rerCtpCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rerctpcodi, DbType.Int32, rerCtpCodi);
            RerCentralLvtpDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RerCentralLvtpDTO> List()
        {
            List<RerCentralLvtpDTO> entities = new List<RerCentralLvtpDTO>();
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

        public List<RerCentralLvtpDTO> ListByRercencodi(int Rercencodi)
        {
            List<RerCentralLvtpDTO> entities = new List<RerCentralLvtpDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByRercencodi);
            dbProvider.AddInParameter(command, helper.Rercencodi, DbType.Int32, Rercencodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerCentralLvtpDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entities.Add(entity);
                }
            }

            return entities;
        }

        public void DeleteAllByRercencodi(int Rercencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteAllByRercencodi);

            dbProvider.AddInParameter(command, helper.Rercencodi, DbType.Int32, Rercencodi);

            dbProvider.ExecuteNonQuery(command);
        }

    }
}
