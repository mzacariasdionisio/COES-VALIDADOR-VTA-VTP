using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class SiLogRepository : RepositoryBase, ISiLogRepository
    {
        #region ASSETEC - CAMBIOS 2
        private string strConexion;
        public SiLogRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }
        #endregion

        SiLogHelper helper = new SiLogHelper();

        #region ASSETEC - CAMBIOS 2
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
        #endregion

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public List<SiLogDTO> Listar(string usuario, string fecha, int ModCodi)
        {
            List<SiLogDTO> entitys = new List<SiLogDTO>();

            string query = string.Format(helper.SqlListar, usuario, fecha, ModCodi);
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

        public int Save(int ModCodi, string evento, string fecha, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);


            string query = string.Format(helper.SqlSave, id, ModCodi, evento, fecha, usuario);
            command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteReader(command);

            return id;
        }

        public int Save(SiLogDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSaveTransferencia);

            dbProvider.AddInParameter(command, helper.LogCodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.ModCodi, DbType.Int32, entity.ModCodi);
            dbProvider.AddInParameter(command, helper.LogDesc, DbType.String, entity.LogDesc);
            dbProvider.AddInParameter(command, helper.LogFecha, DbType.DateTime, entity.LogFecha);
            dbProvider.AddInParameter(command, helper.LogUser, DbType.String, entity.LogUser);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        #region Transferencia de Equipos

        //public int SaveTransferencia(int ModCodi, string evento, string fecha, string usuario, IDbConnection conn, DbTransaction tran, int corrSiLog)
        public int SaveTransferencia(SiLogDTO entity, IDbConnection conn, DbTransaction tran)
        {
            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            //object result = dbProvider.ExecuteScalar(command);
            //int id = 1;
            // if (result != null) id = Convert.ToInt32(result);


            //string query = string.Format(helper.SqlSave, id, ModCodi, evento, fecha, usuario);
            //command = dbProvider.GetSqlStringCommand(query);
            //command.Transaction = tran;
            //command.Connection = (DbConnection)conn;

            //command.ExecuteNonQuery();

            //return id;
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSaveTransferencia;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.LogCodi, DbType.Int32, entity.LogCodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.ModCodi, DbType.Int32, entity.ModCodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.LogDesc, DbType.String, entity.LogDesc));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.LogFecha, DbType.DateTime, entity.LogFecha));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.LogUser, DbType.String, entity.LogUser));

            command.ExecuteNonQuery();

            return entity.LogCodi;
        }


        public List<SiLogDTO> ListLogByMigracion(int migracodi)
        {

            List<SiLogDTO> entitys = new List<SiLogDTO>();
            string query = string.Format(helper.SqlListLogByMigracion, migracodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iLogmigtipo = dr.GetOrdinal(this.helper.Logmigtipo);
                    if (!dr.IsDBNull(iLogmigtipo)) entity.Logmigtipo = Convert.ToInt32(dr.GetValue(iLogmigtipo));

                    int iMiqubamensaje = dr.GetOrdinal(this.helper.Miqubamensaje);
                    if (!dr.IsDBNull(iMiqubamensaje)) entity.Miqubamensaje = dr.GetString(iMiqubamensaje);

                    int iMiqubaflag = dr.GetOrdinal(this.helper.Miqubaflag);
                    if (!dr.IsDBNull(iMiqubaflag)) entity.Miqubaflag = Convert.ToInt32(dr.GetValue(iMiqubaflag));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region INTERVENCIONES

        public void Save(SiLogDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand commandTbSiLog = (DbCommand)conn.CreateCommand();
            commandTbSiLog.CommandText = string.Format(helper.SqlSave, entity.LogCodi, entity.ModCodi, entity.LogDesc, entity.LogUser);
            commandTbSiLog.Transaction = tran;
            commandTbSiLog.Connection = (DbConnection)conn;

            commandTbSiLog.ExecuteNonQuery();
        }

        public List<SiLogDTO> ReporteHistorial(DateTime? fechaInicio, DateTime? fechaFin, string actividad)
        {
            List<SiLogDTO> entitys = new List<SiLogDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlRptHistorialIntervenciones, actividad, actividad, (fechaInicio == null ? "null" : Convert.ToDateTime(fechaInicio).ToString("dd/MM/yyyy")), (fechaFin == null ? "null" : Convert.ToDateTime(fechaFin).AddDays(1).ToString("dd/MM/yyyy"))));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateReportHistorial(dr));
                }
            }

            return entitys;
        }

                
        #endregion
    }
}
