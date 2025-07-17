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
    /// Clase de acceso a datos de la tabla RE_EVENTO_LOGENVIO
    /// </summary>
    public class ReEventoLogenvioRepository: RepositoryBase, IReEventoLogenvioRepository
    {
        public ReEventoLogenvioRepository(string strConn): base(strConn)
        {
        }

        ReEventoLogenvioHelper helper = new ReEventoLogenvioHelper();

        public int Save(ReEventoLogenvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Reevlocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Reevprcodi, DbType.Int32, entity.Reevprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Reevloindcarga, DbType.String, entity.Reevloindcarga);
            dbProvider.AddInParameter(command, helper.Reevlomotivocarga, DbType.String, entity.Reevlomotivocarga);
            dbProvider.AddInParameter(command, helper.Reevlousucreacion, DbType.String, entity.Reevlousucreacion);
            dbProvider.AddInParameter(command, helper.Reevlofeccreacion, DbType.DateTime, entity.Reevlofeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ReEventoLogenvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Reevprcodi, DbType.Int32, entity.Reevprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Reevloindcarga, DbType.String, entity.Reevloindcarga);
            dbProvider.AddInParameter(command, helper.Reevlomotivocarga, DbType.String, entity.Reevlomotivocarga);
            dbProvider.AddInParameter(command, helper.Reevlousucreacion, DbType.String, entity.Reevlousucreacion);
            dbProvider.AddInParameter(command, helper.Reevlofeccreacion, DbType.DateTime, entity.Reevlofeccreacion);
            dbProvider.AddInParameter(command, helper.Reevlocodi, DbType.Int32, entity.Reevlocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int reevlocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Reevlocodi, DbType.Int32, reevlocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ReEventoLogenvioDTO GetById(int reevlocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Reevlocodi, DbType.Int32, reevlocodi);
            ReEventoLogenvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ReEventoLogenvioDTO> List()
        {
            List<ReEventoLogenvioDTO> entitys = new List<ReEventoLogenvioDTO>();
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

        public List<ReEventoLogenvioDTO> GetByCriteria()
        {
            List<ReEventoLogenvioDTO> entitys = new List<ReEventoLogenvioDTO>();
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

        public List<ReEventoLogenvioDTO> ObtenerEnvios(int idEmpresa, int idEvento)
        {
            List<ReEventoLogenvioDTO> entitys = new List<ReEventoLogenvioDTO>();
            string sql = string.Format(helper.SqlObtenerEnvios, idEmpresa, idEvento);
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
