using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
//Transaction
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla SI_MIGRACION
    /// </summary>
    public class SiMigracionRepository : RepositoryBase, ISiMigracionRepository
    {
        private string strConexion;
        public SiMigracionRepository(string strConn)
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

        SiMigracionHelper helper = new SiMigracionHelper();

        public int Save(SiMigracionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migracodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Tmopercodi, DbType.Int32, entity.Tmopercodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migradescripcion, DbType.String, entity.Migradescripcion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migrafeccorte, DbType.DateTime, entity.Migrafeccorte));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migrausucreacion, DbType.String, entity.Migrausucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migrafeccreacion, DbType.DateTime, entity.Migrafeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migrausumodificacion, DbType.String, entity.Migrausumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migrafecmodificacion, DbType.DateTime, entity.Migrafecmodificacion));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migradeleted, DbType.Int32, entity.Migradeleted));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migrareldeleted, DbType.Int32, entity.Migrareldeleted));
            dbProvider.AddInParameter(command, helper.Migraflagstr, DbType.Int32, entity.Migraflagstr);

            command.ExecuteNonQuery();
            return id;
        }

        public void Update(SiMigracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Tmopercodi, DbType.Int32, entity.Tmopercodi);
            dbProvider.AddInParameter(command, helper.Migradescripcion, DbType.String, entity.Migradescripcion);
            dbProvider.AddInParameter(command, helper.Migrafeccorte, DbType.DateTime, entity.Migrafeccorte);
            dbProvider.AddInParameter(command, helper.Migrausucreacion, DbType.String, entity.Migrausucreacion);
            dbProvider.AddInParameter(command, helper.Migrafeccreacion, DbType.DateTime, entity.Migrafeccreacion);
            dbProvider.AddInParameter(command, helper.Migrausumodificacion, DbType.String, entity.Migrausumodificacion);
            dbProvider.AddInParameter(command, helper.Migrafecmodificacion, DbType.DateTime, entity.Migrafecmodificacion);
            dbProvider.AddInParameter(command, helper.Migradeleted, DbType.Int32, entity.Migradeleted);
            dbProvider.AddInParameter(command, helper.Migrareldeleted, DbType.Int32, entity.Migradeleted);
            dbProvider.AddInParameter(command, helper.Migraflagstr, DbType.Int32, entity.Migraflagstr);
            dbProvider.AddInParameter(command, helper.Migracodi, DbType.Int32, entity.Migracodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int migracodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Migracodi, DbType.Int32, migracodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiMigracionDTO GetById(int migracodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Migracodi, DbType.Int32, migracodi);
            SiMigracionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                    entity.Emprnombdestino = dr.GetString(dr.GetOrdinal("EMPRNOMBDESTINO"));
                    entity.Tmoperdescripcion = dr.GetString(dr.GetOrdinal("TMOPERDESCRIPCION"));

                    entity.Emprnomborigen = dr.GetString(dr.GetOrdinal("EMPRNOMBORIGEN"));
                    entity.Emprcodiorigen = dr.GetInt32(dr.GetOrdinal("EMPRCODIORIGEN"));

                    entity.MigrafeccorteSTR = dr.GetDateTime(dr.GetOrdinal("MIGRAFECCORTESTR"));
                }
            }

            return entity;
        }

        public List<SiMigracionDTO> List()
        {
            List<SiMigracionDTO> entitys = new List<SiMigracionDTO>();
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

        public List<SiMigracionDTO> GetByCriteria()
        {
            List<SiMigracionDTO> entitys = new List<SiMigracionDTO>();
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

        public List<SiMigracionDTO> ListarTransferenciasXEmpresaOrigenXEmpresaDestino(int iEmpresaOrigen, int iEmpresaDestino, string sDescripcion)
        {
            List<SiMigracionDTO> entitys = new List<SiMigracionDTO>();
            sDescripcion = sDescripcion == null ? string.Empty : sDescripcion.ToLowerInvariant();
            string query = string.Format(helper.SqlListarTransferenciasXEmpresaOrigenXEmpresaDestino, iEmpresaOrigen, iEmpresaDestino, sDescripcion);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiMigracionDTO entity = helper.Create(dr);

                    int iEmprnomborigen = dr.GetOrdinal(helper.Emprnomborigen);
                    if (!dr.IsDBNull(iEmprnomborigen)) entity.Emprnomborigen = dr.GetString(iEmprnomborigen);

                    int iEmprnombdestino = dr.GetOrdinal(helper.Emprnombdestino);
                    if (!dr.IsDBNull(iEmprnombdestino)) entity.Emprnombdestino = dr.GetString(iEmprnombdestino);

                    int iTmoperdescripcion = dr.GetOrdinal(helper.Tmoperdescripcion);
                    if (!dr.IsDBNull(iTmoperdescripcion)) entity.Tmoperdescripcion = dr.GetString(iTmoperdescripcion);

                    int iTotal = dr.GetOrdinal(helper.Total);
                    if (!dr.IsDBNull(iTotal)) entity.Total = dr.GetInt32(iTotal);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiMigracionDTO> ListarHistoricoEstadoEmpresa(string emprcodi, DateTime fechaConsulta)
        {
            List<SiMigracionDTO> entitys = new List<SiMigracionDTO>();
            string query = string.Format(helper.SqlListarHistoricoEstadoEmpresa, emprcodi, fechaConsulta.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiMigracionDTO entity = helper.Create(dr);

                    int iEmprnomborigen = dr.GetOrdinal(helper.Emprnomborigen);
                    if (!dr.IsDBNull(iEmprnomborigen)) entity.Emprnomborigen = dr.GetString(iEmprnomborigen);

                    int iEmprabrevorigen = dr.GetOrdinal(helper.Emprabrevorigen);
                    if (!dr.IsDBNull(iEmprabrevorigen)) entity.Emprabrevorigen = dr.GetString(iEmprabrevorigen);

                    int iEmprestadoorigen = dr.GetOrdinal(helper.Emprestadoorigen);
                    if (!dr.IsDBNull(iEmprestadoorigen)) entity.Emprestadoorigen = dr.GetString(iEmprestadoorigen);

                    int iTmoperdescripcion = dr.GetOrdinal(helper.Tmoperdescripcion);
                    if (!dr.IsDBNull(iTmoperdescripcion)) entity.Tmoperdescripcion = dr.GetString(iTmoperdescripcion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region siosein2
        public List<SiMigracionDTO> ListarTransferenciasXTipoMigracion(string tmopercodi, DateTime fechaInicio, DateTime fechaFin)
        {
            var query = string.Format(helper.SqlListarTransferenciasXTipoMigracion, tmopercodi, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<SiMigracionDTO> entitys = new List<SiMigracionDTO>();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiMigracionDTO entity;
                    entity = helper.Create(dr);

                    int iEmprnomborigen = dr.GetOrdinal(helper.Emprnomborigen);
                    if (!dr.IsDBNull(iEmprnomborigen)) entity.EmpresaNombreOrigen = dr.GetString(iEmprnomborigen);

                    int iEmprnombdestino = dr.GetOrdinal(helper.Emprnombdestino);
                    if (!dr.IsDBNull(iEmprnombdestino)) entity.EmpresaNombreDestino = dr.GetString(iEmprnombdestino);

                    int iTmoperdescripcion = dr.GetOrdinal(helper.Tmoperdescripcion);
                    if (!dr.IsDBNull(iTmoperdescripcion)) entity.DescripcionTipoOperacion = dr.GetString(iTmoperdescripcion);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        #endregion

        public void UpdateMigraAnulacion(int migracodi, int migrareldeleted, string usuario, DateTime fechaActualizacion, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateMigraAnulacion);
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migrareldeleted, DbType.Int32, migrareldeleted));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migrausumodificacion, DbType.String, usuario));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migrafecmodificacion, DbType.DateTime, fechaActualizacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migracodi, DbType.Int32, migracodi));
            command.ExecuteNonQuery();

        }

        public void UpdateMigraProcesoPendiente(int migracodi, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateMigraProcesoPendiente);
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migracodi, DbType.Int32, migracodi));

            command.ExecuteNonQuery();
        }
    }
}