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
    /// Clase de acceso a datos de la tabla RE_EVENTO_SUMINISTRADOR
    /// </summary>
    public class ReEventoSuministradorRepository: RepositoryBase, IReEventoSuministradorRepository
    {
        public ReEventoSuministradorRepository(string strConn): base(strConn)
        {
        }

        ReEventoSuministradorHelper helper = new ReEventoSuministradorHelper();

        public int Save(ReEventoSuministradorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Reevsucodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Reevprcodi, DbType.Int32, entity.Reevprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Reevsuindcarga, DbType.String, entity.Reevsuindcarga);
            dbProvider.AddInParameter(command, helper.Reevsuresarcimiento, DbType.Decimal, entity.Reevsuresarcimiento);
            dbProvider.AddInParameter(command, helper.Reevsuestado, DbType.String, entity.Reevsuestado);
            dbProvider.AddInParameter(command, helper.Reevsuusucreacion, DbType.String, entity.Reevsuusucreacion);
            dbProvider.AddInParameter(command, helper.Reevsufeccreacion, DbType.DateTime, entity.Reevsufeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ReEventoSuministradorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Reevprcodi, DbType.Int32, entity.Reevprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Reevsuindcarga, DbType.String, entity.Reevsuindcarga);
            dbProvider.AddInParameter(command, helper.Reevsuresarcimiento, DbType.Decimal, entity.Reevsuresarcimiento);
            dbProvider.AddInParameter(command, helper.Reevsuestado, DbType.String, entity.Reevsuestado);
            dbProvider.AddInParameter(command, helper.Reevsuusucreacion, DbType.String, entity.Reevsuusucreacion);
            dbProvider.AddInParameter(command, helper.Reevsufeccreacion, DbType.DateTime, entity.Reevsufeccreacion);
            dbProvider.AddInParameter(command, helper.Reevsucodi, DbType.Int32, entity.Reevsucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int reevsucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Reevsucodi, DbType.Int32, reevsucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ReEventoSuministradorDTO GetById(int reevsucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Reevsucodi, DbType.Int32, reevsucodi);
            ReEventoSuministradorDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ReEventoSuministradorDTO> List()
        {
            List<ReEventoSuministradorDTO> entitys = new List<ReEventoSuministradorDTO>();
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

        public List<ReEventoSuministradorDTO> GetByCriteria()
        {
            List<ReEventoSuministradorDTO> entitys = new List<ReEventoSuministradorDTO>();
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

        public List<ReEmpresaDTO> ObtenerSuministradoresPorEvento(int idEvento)
        {
            List<ReEmpresaDTO> entitys = new List<ReEmpresaDTO>();
            string sql = string.Format(helper.SqlObtenerSuministradoresPorEvento, idEvento);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReEmpresaDTO entity = new ReEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public ReEventoSuministradorDTO ObtenerSuministrador(int idEvento, int idEmpresa)
        {
            string sql = string.Format(helper.SqlObtenerSuministrador, idEvento, idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);                        
            ReEventoSuministradorDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ReEventoSuministradorDTO> ListarPorEvento(int idEvento)
        {
            List<ReEventoSuministradorDTO> entitys = new List<ReEventoSuministradorDTO>();
            string sql = string.Format(helper.SqlListarPorEvento, idEvento);
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
