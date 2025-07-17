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
    /// Clase de acceso a datos de la tabla SI_EMPRESA_CORREO
    /// </summary>
    public class SiEmpresaCorreoRepository : RepositoryBase, ISiEmpresaCorreoRepository
    {
        public SiEmpresaCorreoRepository(string strConn) : base(strConn)
        {
        }

        SiEmpresaCorreoHelper helper = new SiEmpresaCorreoHelper();

        public int Save(SiEmpresaCorreoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Empcorcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, entity.Modcodi);
            dbProvider.AddInParameter(command, helper.Empcornomb, DbType.String, entity.Empcornomb);
            dbProvider.AddInParameter(command, helper.Empcordesc, DbType.String, entity.Empcordesc);
            dbProvider.AddInParameter(command, helper.Empcoremail, DbType.String, entity.Empcoremail);
            dbProvider.AddInParameter(command, helper.Empcorestado, DbType.String, entity.Empcorestado);
            dbProvider.AddInParameter(command, helper.Empcorcargo, DbType.String, entity.Empcorcargo);
            dbProvider.AddInParameter(command, helper.Empcortelefono, DbType.String, entity.Empcortelefono);
            dbProvider.AddInParameter(command, helper.Empcormovil, DbType.String, entity.Empcormovil);
            dbProvider.AddInParameter(command, helper.Empcorindnotic, DbType.String, entity.Empcorindnotic);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiEmpresaCorreoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, entity.Modcodi);
            dbProvider.AddInParameter(command, helper.Empcornomb, DbType.String, entity.Empcornomb);
            dbProvider.AddInParameter(command, helper.Empcordesc, DbType.String, entity.Empcordesc);
            dbProvider.AddInParameter(command, helper.Empcoremail, DbType.String, entity.Empcoremail);
            dbProvider.AddInParameter(command, helper.Empcorestado, DbType.String, entity.Empcorestado);
            dbProvider.AddInParameter(command, helper.Empcorcargo, DbType.String, entity.Empcorcargo);
            dbProvider.AddInParameter(command, helper.Empcortelefono, DbType.String, entity.Empcortelefono);
            dbProvider.AddInParameter(command, helper.Empcormovil, DbType.String, entity.Empcormovil);
            dbProvider.AddInParameter(command, helper.Empcorindnotic, DbType.String, entity.Empcorindnotic);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Empcorcodi, DbType.Int32, entity.Empcorcodi);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int empcorcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Empcorcodi, DbType.Int32, empcorcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(int empcorcodi, string username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);

            dbProvider.AddInParameter(command, helper.Empcorcodi, DbType.Int32, empcorcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiEmpresaCorreoDTO GetById(int empcorcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Empcorcodi, DbType.Int32, empcorcodi);
            SiEmpresaCorreoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiEmpresaCorreoDTO> List()
        {
            List<SiEmpresaCorreoDTO> entitys = new List<SiEmpresaCorreoDTO>();
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

        public List<SiEmpresaCorreoDTO> GetByCriteria(int modcodi, string tipoEmpresas, int emprcodi)
        {
            List<SiEmpresaCorreoDTO> entitys = new List<SiEmpresaCorreoDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, emprcodi, tipoEmpresas, modcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaCorreoDTO entity = helper.Create(dr);

                    int iTipoemprnomb = dr.GetOrdinal(helper.Tipoemprnomb);
                    if (!dr.IsDBNull(iTipoemprnomb)) entity.Tipoemprnomb = dr.GetString(iTipoemprnomb);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iIndnotificacion = dr.GetOrdinal(helper.Indnotificacion);
                    if (!dr.IsDBNull(iIndnotificacion)) entity.Indnotificacion = dr.GetString(iIndnotificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        /// <summary>
        /// Para obtener las empresa que han incumplido con enviar indformación de demanda por la extranet
        /// </summary>
        /// <param name="etacodi">2 si es diario y 3 si es semanal</param>
        /// <param name="fecha">la fecha en consulta , +4 si es semanal</param>
        /// <returns></returns>
        public List<SiEmpresaCorreoDTO> ObtenerEmpresasIncumplimiento(int etacodi, DateTime fecha)
        {
            List<SiEmpresaCorreoDTO> entitys = new List<SiEmpresaCorreoDTO>();
            string sql = string.Format(helper.SqlObtenerEmpresasIncumplimiento, etacodi, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaCorreoDTO entity = new SiEmpresaCorreoDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);



                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<int> ObtenerEmpresasDisponibles()
        {
            List<int> list = new List<int>();
            string sql = string.Format(helper.SqlObtenerEmpresasDisponibles);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) list.Add(Convert.ToInt32(dr.GetValue(iEmprcodi)));
                }
            }

            return list;
        }

        public void ActualizarIndNotifacion(int emprcodi, string indnotificacion, string lastuser)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlHabilitarNotificacion);
            dbProvider.AddInParameter(command, helper.Indnotificacion, DbType.String, indnotificacion);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, lastuser);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<SiEmpresaCorreoDTO> ObtenerCorreosPorMoodulo(int modCodi)
        {
            List<SiEmpresaCorreoDTO> entitys = new List<SiEmpresaCorreoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerCorreoPorModulo);
            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, modCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaCorreoDTO entity = new SiEmpresaCorreoDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iUsername = dr.GetOrdinal(helper.Username);
                    if (!dr.IsDBNull(iUsername)) entity.Username = dr.GetString(iUsername);

                    int iUseremail = dr.GetOrdinal(helper.Useremail);
                    if (!dr.IsDBNull(iUseremail)) entity.Useremail = dr.GetString(iUseremail);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiEmpresaCorreoDTO> ObtenerCorreosPorEmpresaModulo(int idModulo, int idEmpresa)
        {
            List<SiEmpresaCorreoDTO> entitys = new List<SiEmpresaCorreoDTO>();

            string query = string.Format(helper.SqlObtenerCorreoPorEmpresaModulo, idModulo, idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaCorreoDTO entity = new SiEmpresaCorreoDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iUsername = dr.GetOrdinal(helper.Username);
                    if (!dr.IsDBNull(iUsername)) entity.Username = dr.GetString(iUsername);

                    int iUseremail = dr.GetOrdinal(helper.Useremail);
                    if (!dr.IsDBNull(iUseremail)) entity.Useremail = dr.GetString(iUseremail);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiEmpresaCorreoDTO> ObtenerCorreosPorEmpresaModuloAdicional(int idEmpresa, int idModulo)
        {
            List<SiEmpresaCorreoDTO> entitys = new List<SiEmpresaCorreoDTO>();

            string query = string.Format(helper.SqlObtenerCorreosPorEmpresa, idEmpresa, idModulo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaCorreoDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprruc = dr.GetOrdinal(helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiEmpresaCorreoDTO> ObtenerPesonasContactoExportacion(string tipoAgente, int tipoEmpresa)
        {
            List<SiEmpresaCorreoDTO> entitys = new List<SiEmpresaCorreoDTO>();

            string query = string.Format(helper.SqlObtenerReportePersonasContacto, tipoAgente, tipoEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaCorreoDTO entity = new SiEmpresaCorreoDTO();

                    int iEmpcornomb = dr.GetOrdinal(helper.Empcornomb);
                    if (!dr.IsDBNull(iEmpcornomb)) entity.Empcornomb = dr.GetString(iEmpcornomb);                                      

                    int iEmpcoremail = dr.GetOrdinal(helper.Empcoremail);
                    if (!dr.IsDBNull(iEmpcoremail)) entity.Empcoremail = dr.GetString(iEmpcoremail);                                     

                    int iEmpcorcargo = dr.GetOrdinal(helper.Empcorcargo);
                    if (!dr.IsDBNull(iEmpcorcargo)) entity.Empcorcargo = dr.GetString(iEmpcorcargo);

                    int iEmpcortelefono = dr.GetOrdinal(helper.Empcortelefono);
                    if (!dr.IsDBNull(iEmpcortelefono)) entity.Empcortelefono = dr.GetString(iEmpcortelefono);

                    int iEmprocmovil = dr.GetOrdinal(helper.Empcormovil);
                    if (!dr.IsDBNull(iEmprocmovil)) entity.Empcormovil = dr.GetString(iEmprocmovil);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprruc = dr.GetOrdinal(helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprnomb);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprnomb = dr.GetString(iTipoemprdesc);

                    int iEmpcortipo = dr.GetOrdinal(helper.Empcortipo);
                    if (!dr.IsDBNull(iEmpcortipo)) entity.Emprcortipo = dr.GetString(iEmpcortipo);

                    int iEmpcorindnotic = dr.GetOrdinal(helper.Empcorindnotic);
                    if (!dr.IsDBNull(iEmpcorindnotic)) entity.Empcorindnotic = dr.GetString(iEmpcorindnotic);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiEmpresaCorreoDTO> ObtenerCorreosNotificacion(string ruc)
        {
            List<SiEmpresaCorreoDTO> entitys = new List<SiEmpresaCorreoDTO>();

            string query = string.Format(helper.SqlObtenerCorreosNotificacion, ruc);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaCorreoDTO entity = new SiEmpresaCorreoDTO();

                    int iEmpcornomb = dr.GetOrdinal(helper.Empcornomb);
                    if (!dr.IsDBNull(iEmpcornomb)) entity.Empcornomb = dr.GetString(iEmpcornomb);

                    int iEmpcoremail = dr.GetOrdinal(helper.Empcoremail);
                    if (!dr.IsDBNull(iEmpcoremail)) entity.Empcoremail = dr.GetString(iEmpcoremail);

                    int iEmpcorcargo = dr.GetOrdinal(helper.Empcorcargo);
                    if (!dr.IsDBNull(iEmpcorcargo)) entity.Empcorcargo = dr.GetString(iEmpcorcargo);

                    int iEmpcortelefono = dr.GetOrdinal(helper.Empcortelefono);
                    if (!dr.IsDBNull(iEmpcortelefono)) entity.Empcortelefono = dr.GetString(iEmpcortelefono);

                    int iEmprocmovil = dr.GetOrdinal(helper.Empcormovil);
                    if (!dr.IsDBNull(iEmprocmovil)) entity.Empcormovil = dr.GetString(iEmprocmovil);

                    //int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    //if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    //int iEmprruc = dr.GetOrdinal(helper.Emprruc);
                    //if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    //int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprnomb);
                    //if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprnomb = dr.GetString(iTipoemprdesc);

                    int iEmpcortipo = dr.GetOrdinal(helper.Empcortipo);
                    if (!dr.IsDBNull(iEmpcortipo)) entity.Emprcortipo = dr.GetString(iEmpcortipo);

                    int iEmpcorindnotic = dr.GetOrdinal(helper.Empcorindnotic);
                    if (!dr.IsDBNull(iEmpcorindnotic)) entity.Empcorindnotic = dr.GetString(iEmpcorindnotic);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<String> ObtenerListaCorreosNotificacion(string ruc,string tipo)
        {
            List<string> correos = new List<string>();
            int modCodi = tipo.ToUpperInvariant() == "T" ? 34 : tipo.ToUpperInvariant() == "P" ? 31 : -1;

            string query = modCodi == 34 ? string.Format(helper.SqlObtenerListaCorreosNotificacion, ruc, modCodi):
                
                string.Format(helper.SqlObtenerListaCorreosProveedor, ruc, modCodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaCorreoDTO entity = new SiEmpresaCorreoDTO();

                    int iEmpcoremail = dr.GetOrdinal(helper.Empcoremail);
                    if (!dr.IsDBNull(iEmpcoremail)) entity.Empcoremail = dr.GetString(iEmpcoremail);                   

                    correos.Add(entity.Empcoremail.Trim(' '));
                }
            }

            return correos;
        }

        #region Resarcimiento

        public List<SiEmpresaCorreoDTO> ObtenerCorreosPorEmpresaResarcimiento(int idEmpresa)
        {
            List<SiEmpresaCorreoDTO> entitys = new List<SiEmpresaCorreoDTO>();

            string query = string.Format(helper.SqlCorreosPorEmpresaResarcimiento, idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaCorreoDTO entity = new SiEmpresaCorreoDTO();

                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }        

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public void SaveTransaccional(SiEmpresaCorreoDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            DbCommand command2 = (DbCommand)connection.CreateCommand();

            command2.CommandText = helper.SqlSave;
            command2.Transaction = (DbTransaction)transaction;
            command2.Connection = (DbConnection)connection;

            command2.Parameters.Add(dbProvider.CreateParameter(command2, helper.Empcorcodi, DbType.Int32, entity.Empcorcodi));
            command2.Parameters.Add(dbProvider.CreateParameter(command2, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command2.Parameters.Add(dbProvider.CreateParameter(command2, helper.Modcodi, DbType.Int32, entity.Modcodi));
            command2.Parameters.Add(dbProvider.CreateParameter(command2, helper.Empcornomb, DbType.String, entity.Empcornomb));
            command2.Parameters.Add(dbProvider.CreateParameter(command2, helper.Empcordesc, DbType.String, entity.Empcordesc));
            command2.Parameters.Add(dbProvider.CreateParameter(command2, helper.Empcoremail, DbType.String, entity.Empcoremail));
            command2.Parameters.Add(dbProvider.CreateParameter(command2, helper.Empcorestado, DbType.String, entity.Empcorestado));
            command2.Parameters.Add(dbProvider.CreateParameter(command2, helper.Empcorcargo, DbType.String, entity.Empcorcargo));
            command2.Parameters.Add(dbProvider.CreateParameter(command2, helper.Empcortelefono, DbType.String, entity.Empcortelefono));
            command2.Parameters.Add(dbProvider.CreateParameter(command2, helper.Empcormovil, DbType.String, entity.Empcormovil));
            command2.Parameters.Add(dbProvider.CreateParameter(command2, helper.Empcorindnotic, DbType.String, entity.Empcorindnotic));
            command2.Parameters.Add(dbProvider.CreateParameter(command2, helper.Lastuser, DbType.String, entity.Lastuser));            

            command2.ExecuteNonQuery();

        }

        public void DeleteResarcimiento(int emprcodi)
        {            
            string query = string.Format(helper.SqlEliminarPorEmpresaResarcimiento, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            IDataReader dr = dbProvider.ExecuteReader(command);
        }

        public List<SiEmpresaCorreoDTO> ListarSoloResarcimiento()
        {
            List<SiEmpresaCorreoDTO> entitys = new List<SiEmpresaCorreoDTO>();

            string query = string.Format(helper.SqlListarSoloResarcimiento);
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
        


        #endregion
    }
}
