using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class CrEtapaEventoRepository : RepositoryBase, ICrEtapaEventoRepository
    {
        private string strConexion;
        public CrEtapaEventoRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }
        CrEtapaEventoHelper helper = new CrEtapaEventoHelper();

        public CrEtapaEventoDTO ObtenerCrEtapaEvento(int crevencodi, int cretapa)
        {
            string query = String.Format(helper.SqlObtenerCrEtapaEvento, crevencodi, cretapa);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            CrEtapaEventoDTO entity = new CrEtapaEventoDTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {               
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public int save(CrEtapaEventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cretapacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Crevencodi, DbType.Int32, entity.CREVENCODI);
            dbProvider.AddInParameter(command, helper.Cretapa, DbType.Int32, entity.CRETAPA);
            dbProvider.AddInParameter(command, helper.Crfechdesicion, DbType.DateTime, entity.CRFECHDESICION);
            dbProvider.AddInParameter(command, helper.Creventodescripcion, DbType.String, entity.CREVENTODESCRIPCION);
            dbProvider.AddInParameter(command, helper.Crresumencriterio, DbType.String, entity.CRRESUMENCRITERIO);
            dbProvider.AddInParameter(command, helper.Crcomentarios_Responsables, DbType.String, entity.CRCOMENTARIOS_RESPONSABLES);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.LASTDATE);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.LASTUSER);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CrEtapaEventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            //dbProvider.AddInParameter(command, helper.Crcriteriocodi, DbType.Int32, entity.CRCRITERIOCODI);
            dbProvider.AddInParameter(command, helper.Cretapa, DbType.Int32, entity.CRETAPA);
            dbProvider.AddInParameter(command, helper.Crfechdesicion, DbType.DateTime, entity.CRFECHDESICION);
            dbProvider.AddInParameter(command, helper.Creventodescripcion, DbType.String, entity.CREVENTODESCRIPCION);
            dbProvider.AddInParameter(command, helper.Crresumencriterio, DbType.String, entity.CRRESUMENCRITERIO);
            dbProvider.AddInParameter(command, helper.Crcomentarios_Responsables, DbType.String, entity.CRCOMENTARIOS_RESPONSABLES);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.LASTDATE);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.LASTUSER);
            dbProvider.AddInParameter(command, helper.Cretapacodi, DbType.Int32, entity.CRETAPACODI);

            dbProvider.ExecuteNonQuery(command);

        }

        public List<CrEtapaEventoDTO> ListarCrEtapaEvento(int crevencodi)
        {
            List<CrEtapaEventoDTO> entitys = new List<CrEtapaEventoDTO>();

            string query = string.Format(helper.SqlListarCrEtapaEventoAf, crevencodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CrEtapaEventoDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void Delete(int cretapacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Cretapacodi, DbType.Int32, cretapacodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<CrEtapaEventoDTO> ObtenerCriterioxEtapaEvento(int crevencodi)
        {
            List<CrEtapaEventoDTO> entitys = new List<CrEtapaEventoDTO>();

            string query = string.Format(helper.SqlObtenerCriterioxEtapaEvento, crevencodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CrEtapaEventoDTO entity = helper.Create(dr);
                    int iCrcriteriocodi = dr.GetOrdinal(helper.Crcriteriocodi);
                    if (!dr.IsDBNull(iCrcriteriocodi)) entity.CRCRITERIOCODI = dr.GetInt32(iCrcriteriocodi);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public IDbConnection BeginConnection()
        {
            Database db = DatabaseFactory.CreateDatabase(strConexion);
            IDbConnection conn = db.CreateConnection();
            conn.Open();

            return conn;
        }

        public DbTransaction StartTransaction(IDbConnection conn)
        {
            return (DbTransaction)conn.BeginTransaction();
        }

        public int SaveR(CrEtapaEventoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result) + entity.CRETAPA - 1;

            DbCommand commandUp = (DbCommand)conn.CreateCommand();
            commandUp.CommandText = helper.SqlSave;
            commandUp.Transaction = tran;
            commandUp.Connection = (DbConnection)conn;

            IDbDataParameter param = null;

            param = commandUp.CreateParameter(); param.ParameterName = helper.Cretapacodi; param.Value = id; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Crevencodi; param.Value = entity.CREVENCODI; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Cretapa; param.Value = entity.CRETAPA; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Crfechdesicion; param.Value = entity.CRFECHDESICION; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Creventodescripcion; param.Value = entity.CREVENTODESCRIPCION; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Crresumencriterio; param.Value = entity.CRRESUMENCRITERIO; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Crcomentarios_Responsables; param.Value = entity.CRCOMENTARIOS_RESPONSABLES; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Lastdate; param.Value = entity.LASTDATE; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Lastuser; param.Value = entity.LASTUSER; commandUp.Parameters.Add(param);

            try
            {
                commandUp.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return -1;
            }

            return id;
        }
    }
}
