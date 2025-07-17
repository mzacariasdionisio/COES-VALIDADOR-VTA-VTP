using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla SI_MIGRAEMPRORIGEN
    /// </summary>
    public class SiMigraemprorigenRepository : RepositoryBase, ISiMigraemprOrigenRepository
    {

        private string strConexion;
        public SiMigraemprorigenRepository(string strConn)
            : base(strConn)
        {
            strConexion = strConn;
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

        SiMigraemprorigenHelper helper = new SiMigraemprorigenHelper();

        public int Save(SiMigraemprOrigenDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migempcodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migracodi, DbType.Int32, entity.Migracodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migempusucreacion, DbType.String, entity.Migempusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migempfeccreacion, DbType.DateTime, entity.Migempfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migempusumodificacion, DbType.String, entity.Migempusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migempfecmodificacion, DbType.DateTime, entity.Migempfecmodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.MigempestadoOrig, DbType.String, entity.MigempestadoDest));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.MigempcodosinergminDest, DbType.String, entity.MigempcodosinergminDest));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.MigempscadacodiDest, DbType.Int32, entity.MigempscadacodiDest));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.MigempnombrecomercialDest, DbType.String, entity.MigempnombrecomercialDest));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.MigempdomiciliolegalDest, DbType.String, entity.MigempdomiciliolegalDest));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.MigempnumpartidaregDest, DbType.String, entity.MigempnumpartidaregDest));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.MigempabrevDest, DbType.String, entity.MigempabrevDest));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.MigempordenDest, DbType.Int32, entity.MigempordenDest));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.MigemptelefonoDest, DbType.String, entity.MigemptelefonoDest));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.MigempestadoregistroDest, DbType.String, entity.MigempestadoregistroDest));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.MigempfecinscripcionDest, DbType.DateTime, entity.MigempfecinscripcionDest));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.MigempcondicionDest, DbType.String, entity.MigempcondicionDest));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.MigempnroconstanciaDest, DbType.Int32, entity.MigempnroconstanciaDest));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.MigempfecingresoDest, DbType.DateTime, entity.MigempfecingresoDest));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.MigempnroregistroDest, DbType.Int32, entity.MigempnroregistroDest));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.MigempindusutramiteDest, DbType.String, entity.MigempindusutramiteDest));

            command.ExecuteNonQuery();
            return id;
        }

        public void Update(SiMigraemprOrigenDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Migracodi, DbType.Int32, entity.Migracodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Migempusucreacion, DbType.String, entity.Migempusucreacion);
            dbProvider.AddInParameter(command, helper.Migempfeccreacion, DbType.DateTime, entity.Migempfeccreacion);
            dbProvider.AddInParameter(command, helper.Migempusumodificacion, DbType.String, entity.Migempusumodificacion);
            dbProvider.AddInParameter(command, helper.Migempfecmodificacion, DbType.DateTime, entity.Migempfecmodificacion);
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.MigempestadoOrig, DbType.String, entity.MigempestadoDest));

            dbProvider.AddInParameter(command, helper.Migempcodi, DbType.Int32, entity.Migempcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int migempcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Migempcodi, DbType.Int32, migempcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiMigraemprOrigenDTO GetById(int migempcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Migempcodi, DbType.Int32, migempcodi);
            SiMigraemprOrigenDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiMigraemprOrigenDTO> List()
        {
            List<SiMigraemprOrigenDTO> entitys = new List<SiMigraemprOrigenDTO>();
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

        public List<SiMigraemprOrigenDTO> GetByCriteria()
        {
            List<SiMigraemprOrigenDTO> entitys = new List<SiMigraemprOrigenDTO>();
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

        public List<SiMigraemprOrigenDTO> ListadoTransferenciaXEmprOrigenXEmprDestino(int iEmpresaOrigen, int iEmpresaDestino, string SDescripcion)
        {
            List<SiMigraemprOrigenDTO> entitys = new List<SiMigraemprOrigenDTO>();

            SDescripcion = SDescripcion == null ? string.Empty : SDescripcion.ToLowerInvariant();

            string query = string.Format(helper.SqlListadoTransferenciaXEmprOrigenXEmprDestino, iEmpresaOrigen, iEmpresaDestino, SDescripcion);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiMigraemprOrigenDTO oMigraEmprOrigen;
                    oMigraEmprOrigen = helper.Create(dr);
                    oMigraEmprOrigen.EMPRESANOMBREORIGEN = dr.GetString(dr.GetOrdinal("EMPRESANOMBREORIGEN"));
                    oMigraEmprOrigen.EMPRESANOMBREDESTINO = dr.GetString(dr.GetOrdinal("EMPRESANOMBREDESTINO"));

                    entitys.Add(oMigraEmprOrigen);
                }
            }
            return entitys;
        }

    }
}