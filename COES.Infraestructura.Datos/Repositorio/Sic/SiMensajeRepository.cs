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
    public class SiMensajeRepository : RepositoryBase, ISiMensajeRepository
    {
        public SiMensajeRepository(string strConn)
            : base(strConn)
        {
        }

        SiMensajeHelper helper = new SiMensajeHelper();

        public List<SiMensajeDTO> Listar(string correo, int nroPagina, string orden, int pageSize)
        {
            List<SiMensajeDTO> entitys = new List<SiMensajeDTO>();
            string formato = "";
            if (Convert.ToInt32(orden) == 1)
            {
                formato = helper.SqlListar;
            }
            if (Convert.ToInt32(orden) == 2)
            {
                formato = helper.SqlListarEnviados;
            }

            string query = string.Format(formato, correo, nroPagina, orden, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            /* SiMensajeDTO entity;*/

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }


        public int TotalMensajes(string correo, string orden)
        {
            List<SiMensajeDTO> entitys = new List<SiMensajeDTO>();

            int total = 0;
            string formato = "";
            if (Convert.ToInt32(orden) == 1)
            {
                formato = helper.SqlTotalListaRecibidos;
            }
            if (Convert.ToInt32(orden) == 2)
            {
                formato = helper.SqlTotalListaEnviados;
            }

            string query = string.Format(formato, correo, orden);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            /* SiMensajeDTO entity;*/

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) total = Convert.ToInt32(result);

            return total;
        }

        public int Save(string fechaActual, int idFuente, int TipoCorreo, int EstMsg, string Mensaje, string Periodo, int CodModulo, int EmprCodi, int FormatCodi, string usuario, string Correo, string CorreoFrom, string usuarioNom, string Asunto, int flagAdj)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);




            string query = string.Format(helper.SqlSave, id, fechaActual, idFuente, TipoCorreo, EstMsg, Mensaje, (Periodo == "" ? fechaActual.Split(' ')[0] : Periodo), CodModulo, EmprCodi, FormatCodi, usuario, Correo, CorreoFrom, usuarioNom, Asunto, flagAdj);
            command = dbProvider.GetSqlStringCommand(query);



            object idMsg = dbProvider.ExecuteReader(command);

            /*if (idMsg != null) id = Convert.ToInt32(idMsg);*/


            return id;
        }

        public int UpdateEstado(int estado, int MsgCodi, int CodModulo, int EmprCodi, string usuariomod, string fechamod)
        {

            int id = 1;

            string query = string.Format(helper.SqlUpdateEstado, estado, MsgCodi, CodModulo, EmprCodi, usuariomod, fechamod);
            DbCommand command = dbProvider.GetSqlStringCommand(query);



            object idMsg = dbProvider.ExecuteReader(command);

            if (estado == 2)
            {
                string hoy = (Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy")).AddHours(14)).ToString();


                query = string.Format(helper.SqlInsertAmpl, estado, MsgCodi, CodModulo, EmprCodi, usuariomod, fechamod);
                command = dbProvider.GetSqlStringCommand(query);


                dbProvider.ExecuteReader(command);
            }


            /*if (idMsg != null) id = Convert.ToInt32(idMsg);*/


            return id;
        }


        #region Siosein

        public int TotalMensajesxCategoria(string correo, string orden, int modulo, int categoria, int carpeta, string periodo)
        {
            List<SiMensajeDTO> entitys = new List<SiMensajeDTO>();

            int total = 0;
            string formato = "";

            if (Convert.ToInt32(orden) == 2)
            {
                formato = helper.SqlTotalListaEnviadosSiosein;
            }
            else { formato = helper.SqlTotalListaRecibidosSiosein; }

            if (carpeta > 3) { periodo = "0"; }

            string query = string.Format(formato, correo, orden, modulo, categoria, carpeta, periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            /* SiMensajeDTO entity;*/

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) total = Convert.ToInt32(result);

            return total;
        }

        public List<SiMensajeDTO> GetLista(string mailto, string mailfrom, int modcodi, int tipomensaje, int carpeta, int estmsgcodi, DateTime periodo, string msgestado)
        {
            List<SiMensajeDTO> entitys = new List<SiMensajeDTO>();
            string query = string.Format(helper.SqlListarxUsuario, mailto, mailfrom, modcodi, tipomensaje, carpeta, estmsgcodi, periodo.ToString(ConstantesBase.FormatoFechaBase), msgestado);
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

        public int SaveCorreoSiosein(SiMensajeDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSaveCorreoSiosein);

            dbProvider.AddInParameter(command, helper.Msgfecmodificacion, DbType.DateTime, entity.Msgfecmodificacion);
            dbProvider.AddInParameter(command, helper.Msgusumodificacion, DbType.String, entity.Msgusumodificacion);
            dbProvider.AddInParameter(command, helper.Msgfeccreacion, DbType.DateTime, entity.Msgfeccreacion);
            dbProvider.AddInParameter(command, helper.Msgusucreacion, DbType.String, entity.Msgusucreacion);
            dbProvider.AddInParameter(command, helper.Msgtipo, DbType.Int32, entity.Msgtipo);
            dbProvider.AddInParameter(command, helper.Msgestado, DbType.String, entity.Msgestado);
            dbProvider.AddInParameter(command, helper.Bandcodi, DbType.Int32, entity.Bandcodi);
            dbProvider.AddInParameter(command, helper.Msgflagadj, DbType.Int32, entity.Msgflagadj);
            dbProvider.AddInParameter(command, helper.Msgfromname, DbType.String, entity.Msgfromname);
            dbProvider.AddInParameter(command, helper.Msgfrom, DbType.String, entity.Msgfrom);
            dbProvider.AddInParameter(command, helper.Msgto, DbType.String, entity.Msgto);
            dbProvider.AddInParameter(command, helper.Msgasunto, DbType.String, entity.Msgasunto);
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, entity.Modcodi);
            dbProvider.AddInParameter(command, helper.Msgfechaperiodo, DbType.DateTime, entity.Msgfechaperiodo);
            dbProvider.AddInParameter(command, helper.Msgcontenido, DbType.String, entity.Msgcontenido);
            dbProvider.AddInParameter(command, helper.Estmsgcodi, DbType.Int32, entity.Estmsgcodi);
            dbProvider.AddInParameter(command, helper.Tmsgcodi, DbType.Int32, entity.Tmsgcodi);
            dbProvider.AddInParameter(command, helper.Fdatcodi, DbType.Int32, entity.Fdatcodi);
            dbProvider.AddInParameter(command, helper.Msgfecha, DbType.DateTime, entity.Msgfecha);
            dbProvider.AddInParameter(command, helper.Msgcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int UpdateCarpeta(int carcodi, string Correo, int modcodi, string msgcodi)
        {
            int id = 1;
            string query = string.Format(helper.SqlUpdateCarpeta, carcodi, Correo, modcodi, msgcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteReader(command);

            if (carcodi == 3)
            {
                query = string.Format(helper.SqlUpdateEstadoEliminado, carcodi, Correo, modcodi, msgcodi);
                command = dbProvider.GetSqlStringCommand(query);
            }

            dbProvider.ExecuteReader(command);

            return id;
        }
        #endregion

        #region INTERVENCIONES      
        public SiMensajeDTO GetMensajePorId(int idMensaje)
        {
            SiMensajeDTO entity = new SiMensajeDTO();
            string sql = string.Format(helper.SqlGetById, idMensaje);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.CrearMensaje(dr);
                }
            }

            return entity;
        }

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public void Save(SiMensajeDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand commandTbMensaje = (DbCommand)conn.CreateCommand();
            commandTbMensaje.CommandText = helper.SqlEnviar;
            commandTbMensaje.Transaction = tran;
            commandTbMensaje.Connection = (DbConnection)conn;

            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Msgcodi, DbType.Int32, entity.Msgcodi));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Msgfecha, DbType.DateTime, null));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Fdatcodi, DbType.Int32, entity.Fdatcodi));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Tmsgcodi, DbType.Int32, entity.Tmsgcodi));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Estmsgcodi, DbType.Int32, entity.Estmsgcodi));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Msgcontenido, DbType.String, entity.Msgcontenido));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Msgfechaperiodo, DbType.DateTime, null));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Modcodi, DbType.Int32, entity.Modcodi));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Formatcodi, DbType.Int32, entity.Formatcodi));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Msgasunto, DbType.String, entity.Msgasunto));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Msgto, DbType.String, entity.Msgto));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Msgfrom, DbType.String, entity.Msgfrom));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Msgfromname, DbType.String, entity.Msgfromname));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Msgflagadj, DbType.Int32, entity.Msgflagadj));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Bandcodi, DbType.Int32, null));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Msgestado, DbType.String, null));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Msgtipo, DbType.Int32, entity.Msgtipo));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Msgusucreacion, DbType.String, entity.Msgusucreacion));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Msgfeccreacion, DbType.DateTime, entity.Msgfeccreacion));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Msgusumodificacion, DbType.String, null));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Msgfecmodificacion, DbType.DateTime, null));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Msgcc, DbType.String, entity.Msgcc));
            commandTbMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbMensaje, helper.Msgbcc, DbType.String, entity.Msgbcc));

            commandTbMensaje.ExecuteNonQuery();
        }

        public List<SiMensajeDTO> ListSiMensajesIntervencion(int modcodi, int progrcodi, string intercodi, DateTime? fechaInicio, DateTime? fechaFin)
        {
            List<SiMensajeDTO> entitys = new List<SiMensajeDTO>();

            string sql = string.Format(helper.SqlListMensajeIntervencion, progrcodi, intercodi, (fechaInicio == null ? "null" : fechaInicio.Value.ToString(ConstantesBase.FormatoFechaPE)), (fechaFin == null ? "null" : fechaFin.Value.AddDays(1).ToString(ConstantesBase.FormatoFechaPE)), modcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiMensajeDTO entity = helper.CrearMensaje(dr);

                    int iIntercodi = dr.GetOrdinal(this.helper.Intercodi);
                    if (!dr.IsDBNull(iIntercodi)) entity.Intercodi = Convert.ToInt32(dr.GetValue(iIntercodi));

                    int iProgrcodi = dr.GetOrdinal(this.helper.Progrcodi);
                    if (!dr.IsDBNull(iProgrcodi)) entity.Progrcodi = Convert.ToInt32(dr.GetValue(iProgrcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiMensajeDTO> BusquedaSiMensajesIntervencion(int modcodi, int evenclasecodi, int progrcodi, DateTime? fechaInicio, DateTime? fechaFin)
        {
            List<SiMensajeDTO> entitys = new List<SiMensajeDTO>();

            string sql = string.Format(helper.SqlBusquedaSiMensajesIntervencion, evenclasecodi, progrcodi, (fechaInicio == null ? "null" : fechaInicio.Value.ToString(ConstantesBase.FormatoFechaPE)), (fechaFin == null ? "null" : fechaFin.Value.AddDays(1).ToString(ConstantesBase.FormatoFechaPE)), modcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiMensajeDTO entity = helper.CrearMensaje(dr);

                    int iIntercodi = dr.GetOrdinal(this.helper.Intercodi);
                    if (!dr.IsDBNull(iIntercodi)) entity.Intercodi = Convert.ToInt32(dr.GetValue(iIntercodi));

                    int iProgrcodi = dr.GetOrdinal(this.helper.Progrcodi);
                    if (!dr.IsDBNull(iProgrcodi)) entity.Progrcodi = Convert.ToInt32(dr.GetValue(iProgrcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iIntercodivigente = dr.GetOrdinal(helper.Intercodivigente);
                    if (!dr.IsDBNull(iIntercodivigente)) entity.Intercodivigente = Convert.ToInt32(dr.GetValue(iIntercodivigente));

                    if (entity.Intercodivigente == null) entity.Intercodivigente = entity.Intercodi;

                    int iProgramacion = dr.GetOrdinal(helper.Programacion);
                    if (!dr.IsDBNull(iProgramacion)) entity.Programacion = dr.GetString(iProgramacion);

                    int iLectura = dr.GetOrdinal(helper.Msglectura);
                    if (!dr.IsDBNull(iLectura)) entity.Msglectura = dr.GetString(iLectura);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiMensajeDTO> ReporteMensajes(int modcodi, DateTime fechaInicio, DateTime fechaFin, int tipoMensaje)
        {
            List<SiMensajeDTO> entitys = new List<SiMensajeDTO>();
            string sql = string.Format(helper.SqlRptConsultasMensajes, fechaInicio.ToString(ConstantesBase.FormatoFechaPE), fechaFin.AddDays(1).ToString(ConstantesBase.FormatoFechaPE), modcodi, tipoMensaje);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiMensajeDTO entity = helper.CrearMensaje(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        #endregion

        #region GESTOR SIOSEIN
        public void EliminarMensaje(string msgcodi, string username)
        {
            var query = string.Format(helper.SqlDelete, msgcodi, username, DateTime.Now.ToString(ConstantesBase.FormatFechaFull));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }
        #endregion
    }
}
