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
    /// Clase de acceso a datos de la tabla IN_INTERVENCION
    /// </summary>
    public class InIntervencionRepository : RepositoryBase, IInIntervencionRepository
    {
        private string strConexion;
        public InIntervencionRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }

        InIntervencionHelper helper = new InIntervencionHelper();

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

        #region Métodos de Intervenciones (Carga de datos)

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        /// <summary>
        /// Función que Crea registro de Intervenciones una por una
        /// </summary>
        /// <param name="entitys">Objeto de tipo entidad de intervenciones</param>        
        /// <param name="conn">Objeto de tipo IDbConnection</param>
        /// <param name="tran">Objeto de tipo DbTransaction</param>
        /// <returns>Entero</returns>
        public void Save(InIntervencionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            if (entity.Emprcodi <= 0) throw new Exception("Empresa: campo requerido.");
            if (entity.Equicodi <= 0) throw new Exception("Equipo: campo requerido.");
            if (entity.Areacodi <= 0) throw new Exception("Ubicación: campo requerido.");
            if (entity.Operadoremprcodi <= 0) entity.Operadoremprcodi = entity.Emprcodi;
            if ((entity.Interfechafin - entity.Interfechaini).TotalMinutes <= 0) throw new Exception("Fecha Fin: debe ser mayor que la fecha Inicio.");
            if (entity.Intercarpetafiles <= 0) throw new Exception("Registro inválido");

            switch (entity.Interindispo)
            {
                case "E/S":
                    entity.Interindispo = "E";
                    break;
                case "F/S":
                    entity.Interindispo = "F";
                    break;
                default:
                    break;
            }

            DbCommand commandTbIntervencion = (DbCommand)conn.CreateCommand();
            commandTbIntervencion.CommandText = helper.SqlSave;
            commandTbIntervencion.Transaction = tran;
            commandTbIntervencion.Connection = (DbConnection)conn;

            #region CREAR Y LLENAR LOS PARAMETROS

            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Intercodi, DbType.Int32, entity.Intercodi));

            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Tipoevencodi, DbType.Int32, entity.Tipoevencodi));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Progrcodi, DbType.Int32, entity.Progrcodi));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Equicodi, DbType.Int32, entity.Equicodi));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Areacodi, DbType.Int32, entity.Areacodi));


            // --------------------------------------------------------------
            // INDICADOR SI EL REGISTRO TIENE MENSAJES O COMUNICACIONES
            // --------------------------------------------------------------
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Intermensaje, DbType.String, entity.Intermensaje));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Intermensajecoes, DbType.String, entity.Intermensajecoes));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Intermensajeagente, DbType.String, entity.Intermensajeagente));
            // --------------------------------------------------------------

            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interfechapreini, DbType.DateTime, entity.Interfechapreini));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interfechaini, DbType.DateTime, entity.Interfechaini));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interfechaprefin, DbType.DateTime, entity.Interfechaprefin));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interfechafin, DbType.DateTime, entity.Interfechafin));


            // --------------------------------------------------------------
            // CLASE PROGRAMACION
            // --------------------------------------------------------------
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Claprocodi, DbType.Int32, entity.Claprocodi));
            // --------------------------------------------------------------

            // --------------------------------------------------------------
            // CODIGO PARA ASEGURAR LA TRAZABILIDAD
            // --------------------------------------------------------------
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Intercodsegempr, DbType.String, entity.Intercodsegempr));
            // --------------------------------------------------------------

            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interrepetir, DbType.String, entity.Interrepetir));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Intermwindispo, DbType.Double, entity.Intermwindispo));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interindispo, DbType.String, entity.Interindispo));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interinterrup, DbType.String, entity.Interinterrup));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Intermantrelev, DbType.Int32, entity.Intermantrelev));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interconexionprov, DbType.String, entity.Interconexionprov));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Intersistemaaislado, DbType.String, entity.Intersistemaaislado));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interdescrip, DbType.String, entity.Interdescrip));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interjustifaprobrechaz, DbType.String, entity.Interjustifaprobrechaz));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interfecaprobrechaz, DbType.DateTime, entity.Interfecaprobrechaz));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interprocesado, DbType.Int32, entity.Interprocesado));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interisfiles, DbType.String, entity.Interisfiles));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Intermanttocodi, DbType.Int32, entity.Intermanttocodi));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interevenpadre, DbType.Int32, entity.Interevenpadre));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Evenclasecodi, DbType.Int32, entity.Evenclasecodi));


            // --------------------------------------------------------------
            // DATO CLAVE PARA GENERAR EL HISTORIAL
            // --------------------------------------------------------------
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Intercodipadre, DbType.Int32, entity.Intercodipadre));
            // --------------------------------------------------------------

            // --------------------------------------------------------------
            // INDICADOR DE REGISTRO ACTIVO
            // --------------------------------------------------------------
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interregprevactivo, DbType.String, entity.Interregprevactivo));
            // --------------------------------------------------------------
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Estadocodi, DbType.Int32, entity.Estadocodi));

            // ------------------------------------------------------------
            // FLAG DE CREADO
            // ------------------------------------------------------------
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Intercreated, DbType.Int32, entity.Intercreated));
            // ------------------------------------------------------------

            // ------------------------------------------------------------
            // FLAG DE ELIMINADO
            // ------------------------------------------------------------
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interdeleted, DbType.Int32, entity.Interdeleted));
            // ------------------------------------------------------------

            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interfuentestado, DbType.Int32, entity.Interfuentestado));

            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Intertipoindisp, DbType.String, entity.Intertipoindisp));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interpr, DbType.Decimal, entity.Interpr));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interasocproc, DbType.String, entity.Interasocproc));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Intercarpetafiles, DbType.Int32, entity.Intercarpetafiles));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interipagente, DbType.String, entity.Interipagente));

            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interusucreacion, DbType.String, entity.Interusucreacion));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interfeccreacion, DbType.DateTime, entity.Interfeccreacion));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interusumodificacion, DbType.String, entity.Interusumodificacion));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interfecmodificacion, DbType.DateTime, entity.Interfecmodificacion));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Operadoremprcodi, DbType.Int32, entity.Operadoremprcodi));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interusuagrup, DbType.String, entity.Interusuagrup));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interfecagrup, DbType.DateTime, entity.Interfecagrup));

            // ------------------------------------------------------------
            // FLAG DE LEIDO POR AGENTE
            // ------------------------------------------------------------
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interleido, DbType.Int32, entity.Interleido));
            // -

            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Internota, DbType.String, entity.Internota));
            commandTbIntervencion.Parameters.Add(dbProvider.CreateParameter(commandTbIntervencion, helper.Interflagsustento, DbType.Int32, entity.Interflagsustento));
            #endregion

            commandTbIntervencion.ExecuteNonQuery();
        }

        /// <summary>
        /// Función que desabilita una intervencion (SE ACTUALIZA EL STATUS A "N") 
        /// </summary>
        /// <param name="entitys">Objeto  intervencion </param>        
        /// <param name="conn">Objeto de tipo IDbConnection</param>
        /// <param name="tran">Objeto de tipo DbTransaction</param>
        /// <returns>Entero</returns>
        public int DesabilitarIntervencion(string intercodi, IDbConnection conn, DbTransaction tran)
        {
            int id = 1;

            string sql = string.Format(helper.SqlDesabilitarIntervencionConTrazabilidad, intercodi);

            DbCommand commandDesabilitarIntervencion = (DbCommand)conn.CreateCommand();
            commandDesabilitarIntervencion.CommandText = sql;
            commandDesabilitarIntervencion.Transaction = tran;
            commandDesabilitarIntervencion.Connection = (DbConnection)conn;

            commandDesabilitarIntervencion.Parameters.Add(dbProvider.CreateParameter(commandDesabilitarIntervencion, helper.Interregprevactivo, DbType.String, "N"));// N: NO; S: SI
            commandDesabilitarIntervencion.Parameters.Add(dbProvider.CreateParameter(commandDesabilitarIntervencion, helper.Intercreated, DbType.Int32, 1)); // 0: NO; 1: SI
            commandDesabilitarIntervencion.Parameters.Add(dbProvider.CreateParameter(commandDesabilitarIntervencion, helper.Interdeleted, DbType.Int32, 0)); // 0: NO; 1: SI
            //commandDesabilitarIntervencion.Parameters.Add(dbProvider.CreateParameter(commandDesabilitarIntervencion, helper.Intercodi, DbType.Int32, entity.Intercodi));// CÓDIGO DE LA INTERVENCIÓN

            commandDesabilitarIntervencion.ExecuteNonQuery();

            return id;
        }

        /// <summary>
        /// Función que desabilita una intervencion (SE ACTUALIZA EL STATUS A "N") 
        /// </summary>
        /// <param name="entitys">Objeto  intervencion </param>        
        /// <param name="conn">Objeto de tipo IDbConnection</param>
        /// <param name="tran">Objeto de tipo DbTransaction</param>
        /// <returns>Entero</returns>
        public int DeshabilitarIntervencionesRecepcion(int progrcodi, IDbConnection conn, DbTransaction tran)
        {
            int id = 1;
            DbCommand commandDesabilitarIntervencion = (DbCommand)conn.CreateCommand();
            commandDesabilitarIntervencion.CommandText = helper.SqlDeshabilitarIntervencionesRecepcion;
            commandDesabilitarIntervencion.Transaction = tran;
            commandDesabilitarIntervencion.Connection = (DbConnection)conn;

            commandDesabilitarIntervencion.Parameters.Add(dbProvider.CreateParameter(commandDesabilitarIntervencion, helper.Progrcodi, DbType.Int32, progrcodi));

            commandDesabilitarIntervencion.ExecuteNonQuery();

            return id;
        }

        /// <summary>
        /// Función que desabilita una intervencion (SE ACTUALIZA EL STATUS A "N") 
        /// </summary>
        /// <param name="entitys">Objeto  intervencion </param>        
        /// <param name="conn">Objeto de tipo IDbConnection</param>
        /// <param name="tran">Objeto de tipo DbTransaction</param>
        /// <returns>Entero</returns>
        public int DeshabilitarIntervencionesEnReversion(int progrcodi, IDbConnection conn, DbTransaction tran)
        {
            int id = 1;
            DbCommand commandDesabilitarIntervencion = (DbCommand)conn.CreateCommand();
            commandDesabilitarIntervencion.CommandText = helper.SqlDeshabilitarIntervencionesEnReversion;
            commandDesabilitarIntervencion.Transaction = tran;
            commandDesabilitarIntervencion.Connection = (DbConnection)conn;

            commandDesabilitarIntervencion.Parameters.Add(dbProvider.CreateParameter(commandDesabilitarIntervencion, helper.Progrcodi, DbType.Int32, progrcodi));

            commandDesabilitarIntervencion.ExecuteNonQuery();

            return id;
        }

        public void UpdateIsFiles(string intercodi)
        {
            string sql = string.Format(helper.SqlUpdateIsFiles, intercodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateTieneMensaje(string intercodi)
        {
            string sql = string.Format(helper.SqlUpdateTieneMensaje, intercodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateEstadoMensajeCOES(string intercodi, int estado)
        {
            string sql = string.Format(helper.SqlUpdateEstadoMensajeCOES, intercodi, estado);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateEstadoMensajeAgente(string intercodi, int estado)
        {
            string sql = string.Format(helper.SqlUpdateEstadoMensajeAgente, intercodi, estado);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateLeidoAgente(string intercodi)
        {
            string sql = string.Format(helper.SqlUpdateIntervencionLeidoAgente, intercodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.ExecuteNonQuery(command);
        }

        public InIntervencionDTO GetById(int interCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Intercodi, DbType.Int32, interCodi);
            InIntervencionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iTipoEvenDesc = dr.GetOrdinal(this.helper.TipoEvenDesc);
                    if (!dr.IsDBNull(iTipoEvenDesc)) entity.TipoEvenDesc = dr.GetString(iTipoEvenDesc);

                    int iTipoevenabrev = dr.GetOrdinal(this.helper.Tipoevenabrev);
                    if (!dr.IsDBNull(iTipoevenabrev)) entity.Tipoevenabrev = dr.GetString(iTipoevenabrev);

                    int iEmprNomb = dr.GetOrdinal(this.helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iAreaNomb = dr.GetOrdinal(this.helper.AreaNomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

                    int iFamNomb = dr.GetOrdinal(this.helper.FamNomb);
                    if (!dr.IsDBNull(iFamNomb)) entity.FamNomb = dr.GetString(iFamNomb);

                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iAreacodi = dr.GetOrdinal(this.helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iGrupotipocogen = dr.GetOrdinal(this.helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    int iOperadornomb = dr.GetOrdinal(this.helper.Operadornomb);
                    if (!dr.IsDBNull(iOperadornomb)) entity.Operadornomb = dr.GetString(iOperadornomb);

                    int iProgrsololectura = dr.GetOrdinal(helper.Progrsololectura);
                    if (!dr.IsDBNull(iProgrsololectura)) entity.Progrsololectura = dr.GetInt32(iProgrsololectura);

                    int iEvenclasedesc = dr.GetOrdinal(helper.IntNombTipoProgramacion);
                    if (!dr.IsDBNull(iEvenclasedesc)) entity.IntNombTipoProgramacion = dr.GetString(iEvenclasedesc);

                    int iInterleido = dr.GetOrdinal(this.helper.Interleido);
                    if (!dr.IsDBNull(iInterleido)) entity.Interleido = Convert.ToInt32(dr.GetValue(iInterleido));
                }
            }

            return entity;
        }

        public InIntervencionDTO GetByCodigoPadre(string intercodipadre)
        {
            string sql = string.Format(helper.SqlGetByCodigoPadre, intercodipadre);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            InIntervencionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<InIntervencionDTO> ListarIntervencionCandidatoPorCriterio(int equicodi, int tipoevencodi, DateTime fechaIni, DateTime fechaFin, int progrcodi)
        {
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();

            string sql = string.Format(helper.SqlListarIntervencionCandidatoPorCriterio, progrcodi, tipoevencodi, equicodi, fechaIni.ToString(ConstantesBase.FormatoFechaExtendido),
                                            fechaFin.ToString(ConstantesBase.FormatoFechaExtendido));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new InIntervencionDTO();

                    int iIntercodi = dr.GetOrdinal(helper.Intercodi);
                    if (!dr.IsDBNull(iIntercodi)) entity.Intercodi = Convert.ToInt32(dr.GetValue(iIntercodi));

                    int iIntercodsegempr = dr.GetOrdinal(helper.Intercodsegempr);
                    if (!dr.IsDBNull(iIntercodsegempr)) entity.Intercodsegempr = dr.GetString(iIntercodsegempr);
                    entity.Intercodsegempr = entity.Intercodsegempr ?? "";

                    int iInterdescrip = dr.GetOrdinal(helper.Interdescrip);
                    if (!dr.IsDBNull(iInterdescrip)) entity.Interdescrip = dr.GetString(iInterdescrip);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<InIntervencionDTO> ExisteCodigoSeguimiento(string codSeguimiento)
        {
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlExisteCodigoSeguimiento);
            dbProvider.AddInParameter(command, helper.Intercodsegempr, DbType.String, codSeguimiento);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var reg = new InIntervencionDTO();

                    reg.NroItem = Convert.ToInt32(dr["nroregistros"].ToString().Trim());

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) reg.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    entitys.Add(reg);
                }
            }

            return entitys;
        }

        public List<InIntervencionDTO> ConsultarIntervenciones(int idProgramacion, string idTipoProgramacion, string strIdsEmpresa,
                                    string strIdsTipoIntervencion, string strIdsAreas, string strIdsFamilias,
                                    string strIndisponible, string strIdsEstados, int flagEliminado, int flagAprobado,
                                    DateTime fechaIni, DateTime fechaFin, string strIdEquipo = "0")
        {
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();

            string sql = string.Format(helper.SqlConsultarIntervenciones, idProgramacion, idTipoProgramacion, strIdsEmpresa,
                                        strIdsTipoIntervencion, strIdsAreas, strIdsFamilias, strIndisponible,
                                        strIdsEstados, flagEliminado, flagAprobado,
                                        fechaIni.Date.ToString(ConstantesBase.FormatoFecha), fechaFin.Date.AddDays(1).ToString(ConstantesBase.FormatoFecha), strIdEquipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.CreateQuery(dr);

                    int iEquimaniobra = dr.GetOrdinal(helper.Equimaniobra);
                    if (!dr.IsDBNull(iEquimaniobra)) entity.Equimaniobra = (dr.GetString(iEquimaniobra) ?? "N").Trim();
                    if (entity.Equimaniobra != "S") entity.Equimaniobra = "N";

                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iEstadopadre = dr.GetOrdinal(helper.Estadopadre);
                    if (!dr.IsDBNull(iEstadopadre)) entity.Estadopadre = Convert.ToInt32(dr.GetValue(iEstadopadre));

                    int iAreadesc = dr.GetOrdinal(helper.Areadesc);
                    if (!dr.IsDBNull(iAreadesc)) entity.Areadesc = dr.GetString(iAreadesc);

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<InIntervencionDTO> ConsultarTrazabilidad(int interCodi, int tipoProgramacion, string interCodSegEmpr)
        {
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlConsultarTrazabilidad));
            dbProvider.AddInParameter(command, helper.Intercodi, DbType.Int32, interCodi);
            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, tipoProgramacion);
            dbProvider.AddInParameter(command, helper.Intercodsegempr, DbType.String, interCodSegEmpr);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    //-------------------------------------------------------------------------------------------------------
                    // ASSETEC.SGH - 11/10/2017: CAMPOS ADICIONALES CALCULADOS
                    //-------------------------------------------------------------------------------------------------------
                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iTipoEvenDesc = dr.GetOrdinal(helper.TipoEvenDesc);
                    if (!dr.IsDBNull(iTipoEvenDesc)) entity.TipoEvenDesc = dr.GetString(iTipoEvenDesc);

                    int iEmprNomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iAreaNomb = dr.GetOrdinal(helper.AreaNomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

                    int iFamNomb = dr.GetOrdinal(helper.FamNomb);
                    if (!dr.IsDBNull(iFamNomb)) entity.FamNomb = dr.GetString(iFamNomb);

                    int iEquiNomb = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiNomb)) entity.Equiabrev = dr.GetString(iEquiNomb);

                    //-------------------------------------------------------------------------------------------------------
                    // ASSETEC.SGH - 14/06/2018: CAMPOS FLAG PARA PRESENTAR REGISTROS CON STYLOS
                    //-------------------------------------------------------------------------------------------------------
                    int iEvenclasedesc = dr.GetOrdinal(helper.IntNombTipoProgramacion);
                    if (!dr.IsDBNull(iEvenclasedesc)) entity.IntNombTipoProgramacion = dr.GetString(iEvenclasedesc);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<InIntervencionDTO> ConsultarIntervencionesXIds(string intercodis)
        {
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();

            string sql = string.Format(helper.SqlConsultarIntervencionesXIds, intercodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.CreateQuery(dr);

                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iEvenclasedesc = dr.GetOrdinal(helper.IntNombTipoProgramacion);
                    if (!dr.IsDBNull(iEvenclasedesc)) entity.IntNombTipoProgramacion = dr.GetString(iEvenclasedesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Deshabilita todas las intervenciones creadas, editadas durante la reversión
        /// </summary>
        /// <param name="progrcodi"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int DeshabilitarIntervencionEnReversion(int progrcodi, IDbConnection conn, DbTransaction tran)
        {
            int id = 1;
            DbCommand commandDesabilitarIntervencion = (DbCommand)conn.CreateCommand();
            commandDesabilitarIntervencion.CommandText = helper.SqlDeshabilitarIntervencionEnReversion;
            commandDesabilitarIntervencion.Transaction = tran;
            commandDesabilitarIntervencion.Connection = (DbConnection)conn;

            commandDesabilitarIntervencion.Parameters.Add(dbProvider.CreateParameter(commandDesabilitarIntervencion, helper.Progrcodi, DbType.Int32, progrcodi));

            commandDesabilitarIntervencion.ExecuteNonQuery();

            return id;
        }

        /// <summary>
        /// Habilita las intervenciones aprobadas  que perdieron su estado (sea por edicion o eliminación durante la reversión)
        /// </summary>
        /// <param name="progrcodi"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int HabilitarIntervencionesRevertidas(int progrcodi, IDbConnection conn, DbTransaction tran)
        {
            int id = 1;
            DbCommand commandDesabilitarIntervencion = (DbCommand)conn.CreateCommand();
            commandDesabilitarIntervencion.CommandText = helper.SqlHabilitarIntervencionesRevertidas;
            commandDesabilitarIntervencion.Transaction = tran;
            commandDesabilitarIntervencion.Connection = (DbConnection)conn;

            commandDesabilitarIntervencion.Parameters.Add(dbProvider.CreateParameter(commandDesabilitarIntervencion, helper.Progrcodi, DbType.Int32, progrcodi));

            commandDesabilitarIntervencion.ExecuteNonQuery();

            return id;
        }

        /// <summary>
        /// Deshabilita las intervencion eliminada o rechazada
        /// </summary>
        /// <param name="intercodi"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int DeshabilitarIntervencionEliminadaRechazada(int intercodi, int interprocesado, IDbConnection conn, DbTransaction tran)
        {
            int id = 1;
            DbCommand commandDesabilitarIntervencionRecuperada = (DbCommand)conn.CreateCommand();
            commandDesabilitarIntervencionRecuperada.CommandText = helper.SqlDeshabilitarIntervencionEliminadaRechazada;
            commandDesabilitarIntervencionRecuperada.Transaction = tran;
            commandDesabilitarIntervencionRecuperada.Connection = (DbConnection)conn;

            commandDesabilitarIntervencionRecuperada.Parameters.Add(dbProvider.CreateParameter(commandDesabilitarIntervencionRecuperada, helper.Interprocesado, DbType.Int32, interprocesado));
            commandDesabilitarIntervencionRecuperada.Parameters.Add(dbProvider.CreateParameter(commandDesabilitarIntervencionRecuperada, helper.Intercodi, DbType.Int32, intercodi));

            commandDesabilitarIntervencionRecuperada.ExecuteNonQuery();

            return id;
        }

        /// <summary>
        /// Habilita las intervención eliminada o rechazada, es decir la recupera 
        /// </summary>
        /// <param name="intercodi"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int RecuperarIntervencion(int intercodi, int interprocesado, IDbConnection conn, DbTransaction tran)
        {
            int id = 1;
            DbCommand commandDesabilitarIntervencion = (DbCommand)conn.CreateCommand();
            commandDesabilitarIntervencion.CommandText = helper.SqlRecuperarIntervencion;
            commandDesabilitarIntervencion.Transaction = tran;
            commandDesabilitarIntervencion.Connection = (DbConnection)conn;

            commandDesabilitarIntervencion.Parameters.Add(dbProvider.CreateParameter(commandDesabilitarIntervencion, helper.Interprocesado, DbType.Int32, interprocesado));
            commandDesabilitarIntervencion.Parameters.Add(dbProvider.CreateParameter(commandDesabilitarIntervencion, helper.Intercodi, DbType.Int32, intercodi));

            commandDesabilitarIntervencion.ExecuteNonQuery();

            return id;
        }

        public List<InIntervencionDTO> ListarIntervencionesSinArchivo(int progrcodi)
        {
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();

            string sql = string.Format(helper.SqlListarIntervencionesSinArchivo, progrcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region Métodos Para Reportes
        #region Anexos Programacion Anual
        public List<InIntervencionDTO> ListadoIntervencionesMayores(int idProgAnual, string strIdsEmpresa, string strIntersistemaAislado, string strInterinterrup, string strIndisponible)
        {
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlRptIntervencionesMayores, idProgAnual, strIdsEmpresa, strIntersistemaAislado, strInterinterrup, strIndisponible));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateReportMayores(dr));
                }
            }

            return entitys;
        }

        public List<InIntervencionDTO> ReporteCruzadoIntervencionesMayoresGeneracionESFS(int idProgAnual, string strIdsEmpresa, int anio, int mes, string strIndispo)
        {
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlRptCruzadoIntervencionesMayoresGeneracionESFS, idProgAnual, strIdsEmpresa, anio, mes, strIndispo));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateReporteCruzadoMayores(dr));
                }
            }

            return entitys;
        }

        public List<InIntervencionDTO> ReporteCruzadoIntervencionesMayoresTransmisionESFS(int idProgAnual, string strIdsEmpresa, int anio, int mes, string strIndispo)
        {
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlRptCruzadoIntervencionesMayoresTransmisionESFS, idProgAnual, strIdsEmpresa, anio, mes, strIndispo));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateReporteCruzadoMayores(dr));
                }
            }

            return entitys;
        }
        #endregion

        #region Intervenciones Mayores
        public List<InIntervencionDTO> ReporteIntervencionesMayoresPorPeriodo(int progrCodi, int idTipoProgramacion, string strIdsEmpresa, DateTime? fechaInicio, DateTime? fechaFin)
        {
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlRptIntervencionesMayoresPorPeriodo, idTipoProgramacion, strIdsEmpresa, (fechaInicio == null ? "null" : Convert.ToDateTime(fechaInicio).ToString("dd/MM/yyyy")), (fechaFin == null ? "null" : Convert.ToDateTime(fechaFin).AddDays(1).ToString("dd/MM/yyyy"))));
            //DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlRptIntervencionesMayoresPorPeriodo, progrCodi, idTipoProgramacion, strIdsEmpresa, (fechaInicio == null ? "null" : Convert.ToDateTime(fechaInicio).ToString("dd/MM/yyyy")), (fechaFin == null ? "null" : Convert.ToDateTime(fechaFin).AddDays(1).ToString("dd/MM/yyyy"))));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateReportMayoresPorPeriodo(dr));
                }
            }

            return entitys;
        }


        #endregion

        #region Intervenciones Importantes
        public List<InIntervencionDTO> ReporteIntervencionesImportantes(int progrCodi, int idTipoProgramacion, string strIdsEmpresa, DateTime? fechaInicio, DateTime? fechaFin)
        {
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();
            string sqlQuery = string.Format(helper.SqlRptIntervencionesImportantes, progrCodi, idTipoProgramacion, strIdsEmpresa, (fechaInicio == null ? "null" : Convert.ToDateTime(fechaInicio).ToString("dd/MM/yyyy")), (fechaFin == null ? "null" : Convert.ToDateTime(fechaFin).AddDays(1).ToString("dd/MM/yyyy")));
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.CreateReportImportantes(dr);
                    int iTipoevencodi = dr.GetOrdinal(helper.Tipoevencodi);
                    if (!dr.IsDBNull(iTipoevencodi)) entity.Tipoevencodi = Convert.ToInt32(dr.GetValue(iTipoevencodi));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region Eventos
        public List<InIntervencionDTO> ReporteEventos(int progrCodi, int idTipoProgramacion, string strIdsEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlRptEventos, progrCodi, idTipoProgramacion, strIdsEmpresa, fechaInicio.ToString("dd/MM/yyyy"), fechaFin.AddDays(1).ToString("dd/MM/yyyy")));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateReportImportantes(dr));
                }
            }

            return entitys;
        }
        #endregion

        #region Conexiones Provisionales
        public List<InIntervencionDTO> ReporteConexionesProvisionales(int progrCodi, int idTipoProgramacion, string strIdsEmpresa, DateTime? fechaInicio, DateTime? fechaFin)
        {
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();
            string strSql = string.Format(helper.SqlRptConexionesProvisionales, progrCodi, idTipoProgramacion, strIdsEmpresa, (fechaInicio == null ? "null" : Convert.ToDateTime(fechaInicio).ToString("dd/MM/yyyy")), (fechaFin == null ? "null" : Convert.ToDateTime(fechaFin).AddDays(1).ToString("dd/MM/yyyy")));
            DbCommand command = dbProvider.GetSqlStringCommand(strSql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateReportConexionesProvisionales(dr));
                }
            }

            return entitys;
        }



        #endregion

        #region Sistemas Aislados
        public List<InIntervencionDTO> ReporteSistemasAislados(int progrCodi, int idTipoProgramacion, string strIdsEmpresa, DateTime? fechaInicio, DateTime? fechaFin)
        {
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlRptSistemasAislados, progrCodi, idTipoProgramacion, strIdsEmpresa, (fechaInicio == null ? "null" : Convert.ToDateTime(fechaInicio).ToString("dd/MM/yyyy")), (fechaFin == null ? "null" : Convert.ToDateTime(fechaFin).AddDays(1).ToString("dd/MM/yyyy"))));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateReportSistemasAislados(dr));
                }
            }

            return entitys;
        }


        #endregion

        #region Interrupcion o Restriccion de Suministros
        public List<InIntervencionDTO> ReporteInterrupcionRestriccionSuministros(int progrCodi, int idTipoProgramacion, string strIdsEmpresa, DateTime? fechaInicio, DateTime? fechaFin)
        {
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlRptInterrupcionRestriccionSuministros, progrCodi, idTipoProgramacion, strIdsEmpresa, (fechaInicio == null ? "null" : Convert.ToDateTime(fechaInicio).ToString("dd/MM/yyyy")), (fechaFin == null ? "null" : Convert.ToDateTime(fechaFin).AddDays(1).ToString("dd/MM/yyyy"))));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateReportInterrupcionRestriccionSuministros(dr));
                }
            }

            return entitys;
        }
        #endregion

        #region Agentes
        public List<InIntervencionDTO> ReporteAgentes(int progrCodi, int idTipoProgramacion, string strIdsEmpresa, DateTime? fechaInicio, DateTime? fechaFin)
        {
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlRptAgentes, progrCodi, idTipoProgramacion, strIdsEmpresa, (fechaInicio == null ? "null" : Convert.ToDateTime(fechaInicio).ToString("dd/MM/yyyy")), (fechaFin == null ? "null" : Convert.ToDateTime(fechaFin).AddDays(1).ToString("dd/MM/yyyy"))));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateReportAgentes(dr));
                }
            }

            return entitys;
        }
        #endregion

        #region Proc - 25 OSINERGMIN
        public List<InIntervencionDTO> ReporteOSINERGMINProc257dListado()
        {
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlRptOSINERGMINProc257dListado);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateReportOSINERGMIN7d(dr));
                }
            }

            return entitys;
        }
        #endregion

        #region Intervenciones Formato OSINERGMIN
        public List<InIntervencionDTO> ReporteIntervenciones(int idTipoProgramacion, string StrIdsEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlRptIntervenciones, idTipoProgramacion, StrIdsEmpresa, (fechaInicio == null ? "null" : Convert.ToDateTime(fechaInicio).ToString("dd/MM/yyyy")), (fechaFin == null ? "null" : Convert.ToDateTime(fechaFin).AddDays(1).ToString("dd/MM/yyyy"))));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateReportIntervenciones(dr));
                }
            }

            return entitys;
        }

        public List<InIntervencionDTO> ListaMantenimientos25(int evenclasecodi, string evenclasedesc, DateTime fechaini, DateTime fechafin)
        {
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();
            string query = string.Format(helper.SqlListaMantenimientos25, evenclasecodi, evenclasedesc, fechaini.ToString(ConstantesBase.FormatoFechaBase), fechafin.ToString(ConstantesBase.FormatoFechaBase));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    InIntervencionDTO entity = new InIntervencionDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprNomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iAreaNomb = dr.GetOrdinal(helper.AreaNomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iInterfechaini = dr.GetOrdinal(helper.Interfechaini);
                    if (!dr.IsDBNull(iInterfechaini)) entity.Interfechaini = dr.GetDateTime(iInterfechaini);

                    int iInterfechafin = dr.GetOrdinal(helper.Interfechafin);
                    if (!dr.IsDBNull(iInterfechafin)) entity.Interfechafin = dr.GetDateTime(iInterfechafin);

                    int iTipoEvenDesc = dr.GetOrdinal(helper.IntNombTipoProgramacion);
                    if (!dr.IsDBNull(iTipoEvenDesc)) entity.TipoEvenDesc = dr.GetString(iTipoEvenDesc);

                    int iInterdescrip = dr.GetOrdinal(helper.Interdescrip);
                    if (!dr.IsDBNull(iInterdescrip)) entity.Interdescrip = dr.GetString(iInterdescrip);

                    int iTareacodi = dr.GetOrdinal(helper.Tareacodi);
                    if (!dr.IsDBNull(iTareacodi)) entity.Tareacodi = dr.GetInt32(iTareacodi);

                    int iEvenclasedesc = dr.GetOrdinal(helper.IntNombTipoProgramacion);
                    if (!dr.IsDBNull(iEvenclasedesc)) entity.IntNombTipoProgramacion = dr.GetString(iEvenclasedesc);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<InIntervencionDTO> ReporteIntervencionesOsinergmin(int progrCodi, int idTipoProgramacion, string StrIdsEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            string fechaIni = Convert.ToDateTime(fechaInicio).ToString("dd/MM/yyyy");
            string fechaFinal = Convert.ToDateTime(fechaFin).AddDays(1).ToString("dd/MM/yyyy");

            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlRptIntervencionesOsinergmin, progrCodi, idTipoProgramacion, StrIdsEmpresa, fechaIni, fechaFinal));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateReportIntervenciones(dr));
                }
            }

            return entitys;
        }
        #endregion    

        #region F1F2
        #region PARA INTERVENCIONES PROGRAMADAS
        public List<InIntervencionDTO> ReporteIntervencionesF1F2Programados(int anio, int mes)
        {
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlRptIntervencionesF1F2Programados, anio, mes));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateReportF1F2(dr));
                }
            }

            return entitys;
        }

        public bool BuscarEjecutadoPorCodSeguimiento(string codSeguimiento)
        {
            bool rpta = false; // No existe el registro con el codigo de seguimiento

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlRptBuscarEjecutadoPorCodSeguimiento);

            dbProvider.AddInParameter(command, helper.Intercodsegempr, DbType.String, codSeguimiento);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int nroReg = Convert.ToInt32(dr["CodSeguimiento"].ToString().Trim());

                    if (nroReg > 0)
                    {
                        rpta = true; // Si existe el registro con el codigo de seguimiento
                    }
                }
            }

            return rpta;
        }
        #endregion

        #region PARA INTERVENCIONES EJECUTADAS
        public List<InIntervencionDTO> ReporteIntervencionesF1F2Ejecutados(int anio, int mes)
        {
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlRptIntervencionesF1F2Ejecutados, anio, mes));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateReportF1F2(dr));
                }
            }

            return entitys;
        }

        public bool BuscarMensualProgramadoPorCodSeguimiento(string codSeguimiento)
        {
            bool rpta = false; // No existe el registro con el codigo de seguimiento

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlRptBuscarMensualProgramadoPorCodSeguimiento);

            dbProvider.AddInParameter(command, helper.Intercodsegempr, DbType.String, codSeguimiento);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int nroReg = Convert.ToInt32(dr["CodSeguimiento"].ToString().Trim());

                    if (nroReg > 0)
                    {
                        rpta = true; // Si existe el registro con el codigo de seguimiento
                    }
                }
            }

            return rpta;
        }
        #endregion

        #endregion
        #endregion

        #region Métodos para Validación con Aplicativos

        public List<InIntervencionDTO> ListarIntervencionesEquiposGen(DateTime dFechaInicio, DateTime dFechaFin, int famcodiGenerador, int famcodiCentral)
        {
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();

            string sql = string.Format(helper.SqlListarIntervencionesEquiposGen, dFechaInicio.ToString(ConstantesBase.FormatoFecha), dFechaFin.ToString(ConstantesBase.FormatoFecha), famcodiGenerador, famcodiCentral);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateQuery(dr));
                }
            }

            return entitys;
        }

        #endregion

        #region Seguridad Permisos

        public int UpdateUserPermiso(int userCodi, int valor)
        {
            int id = -1;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateUserPermiso);
            dbProvider.AddInParameter(command, helper.UserflagPermiso, DbType.Int32, valor);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, userCodi);

            id = dbProvider.ExecuteNonQuery(command);

            return id;
        }


        public int ObtenerFlagUserPermiso(int userCodi)
        {
            String query = String.Format(helper.SqlObtenerFlagUserPermiso, userCodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object val = dbProvider.ExecuteScalar(command);

            if (val != null) return Convert.ToInt32(val);

            return 0;
        }

        public int ConsultarPermisos(string userlogin)
        {
            String query = String.Format(helper.SqlConsultarPermisos, userlogin);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object val = dbProvider.ExecuteScalar(command);

            if (val != null) return Convert.ToInt32(val);

            return 0;
        }

        #endregion

        #region Yupana - Portal

        public List<InIntervencionDTO> ListarIntervencionesXPagina(string idsTipoMantenimiento, DateTime fechaInicio, DateTime fechaFin, string indispo,
            string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, string indInterrupcion, string idstipoMantto, int nroPagina, int nroFilas)
        {
            String query = String.Format(helper.SqlListarIntervencionesXPagina, idsTipoMantenimiento, idsEmpresa, idsTipoEquipo,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), nroPagina,
                nroFilas, idsTipoEmpresa, indInterrupcion, idstipoMantto, indispo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();
            InIntervencionDTO entity = new InIntervencionDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.EmprNomb = dr.GetString(iEmprnomb);

                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEvenclasedesc = dr.GetOrdinal(helper.Evenclasedesc);
                    if (!dr.IsDBNull(iEvenclasedesc)) entity.Evenclasedesc = dr.GetString(iEvenclasedesc);

                    int iAreanomb = dr.GetOrdinal(helper.AreaNomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.AreaNomb = dr.GetString(iAreanomb);

                    int iAreadesc = dr.GetOrdinal(helper.Areadesc);
                    if (!dr.IsDBNull(iAreadesc)) entity.Areadesc = dr.GetString(iAreadesc);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamnomb = dr.GetOrdinal(helper.FamNomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.FamNomb = dr.GetString(iFamnomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iCausaevenabrev = dr.GetOrdinal(helper.Causaevenabrev);
                    if (!dr.IsDBNull(iCausaevenabrev)) entity.Causaevenabrev = dr.GetString(iCausaevenabrev);

                    int iEquitension = dr.GetOrdinal(helper.Equitension);
                    if (!dr.IsDBNull(iEquitension)) entity.Equitension = dr.GetDecimal(iEquitension);

                    int iTipoevenabrev = dr.GetOrdinal(helper.Tipoevenabrev);
                    if (!dr.IsDBNull(iTipoevenabrev)) entity.Tipoevenabrev = dr.GetString(iTipoevenabrev);

                    int iTipoevendesc = dr.GetOrdinal(helper.Tipoevendesc);
                    if (!dr.IsDBNull(iTipoevendesc)) entity.TipoEvenDesc = dr.GetString(iTipoevendesc);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(iTipoemprcodi);

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iEvenclaseabrev = dr.GetOrdinal(helper.Evenclaseabrev);
                    if (!dr.IsDBNull(iEvenclaseabrev)) entity.Evenclaseabrev = dr.GetString(iEvenclaseabrev);

                    int ifamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(ifamabrev)) entity.Famabrev = dr.GetString(ifamabrev);

                    int iOsigrupocodi = dr.GetOrdinal(helper.Osigrupocodi);
                    if (!dr.IsDBNull(iOsigrupocodi)) entity.Osigrupocodi = dr.GetString(iOsigrupocodi);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);

                    entitys.Add(entity);
                }
            }


            return entitys;
        }

        public int ObtenerNroRegistrosPaginado(string idsTipoMantenimiento, DateTime fechaInicio, DateTime fechaFin, string indispo,
           string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, string indInterrupcion, string idstipoMantto)
        {
            String query = String.Format(helper.SqlObtenerNroRegistrosPaginado, idsTipoMantenimiento, idsEmpresa, idsTipoEquipo,
                    fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha),
                     idsTipoEmpresa, indInterrupcion, idstipoMantto, indispo);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public List<InIntervencionDTO> ListarIntervencionesGrafico(string idsTipoMantenimiento, DateTime fechaInicio, DateTime fechaFin, string indispo,
                    string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, string indInterrupcion, string idstipoMantto)
        {
            String query = String.Format(helper.SqlListarIntervencionesGrafico, idsTipoMantenimiento, idsEmpresa, idsTipoEquipo,
               fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha),
               idsTipoEmpresa, indInterrupcion, idstipoMantto, indispo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    InIntervencionDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.EmprNomb = dr.GetString(iEmprnomb);

                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEvenclasedesc = dr.GetOrdinal(helper.Evenclasedesc);
                    if (!dr.IsDBNull(iEvenclasedesc)) entity.Evenclasedesc = dr.GetString(iEvenclasedesc);

                    int iEvenclaseabrev = dr.GetOrdinal(helper.Evenclaseabrev);
                    if (!dr.IsDBNull(iEvenclaseabrev)) entity.Evenclaseabrev = dr.GetString(iEvenclaseabrev);

                    int iAreanomb = dr.GetOrdinal(helper.AreaNomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.AreaNomb = dr.GetString(iAreanomb);

                    int iAreadesc = dr.GetOrdinal(helper.Areadesc);
                    if (!dr.IsDBNull(iAreadesc)) entity.Areadesc = dr.GetString(iAreadesc);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int ifamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(ifamabrev)) entity.Famabrev = dr.GetString(ifamabrev);

                    int iFamnomb = dr.GetOrdinal(helper.FamNomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.FamNomb = dr.GetString(iFamnomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iCausaevenabrev = dr.GetOrdinal(helper.Causaevenabrev);
                    if (!dr.IsDBNull(iCausaevenabrev)) entity.Causaevenabrev = dr.GetString(iCausaevenabrev);

                    int iEquitension = dr.GetOrdinal(helper.Equitension);
                    if (!dr.IsDBNull(iEquitension)) entity.Equitension = dr.GetDecimal(iEquitension);

                    int iTipoevenabrev = dr.GetOrdinal(helper.Tipoevenabrev);
                    if (!dr.IsDBNull(iTipoevenabrev)) entity.Tipoevenabrev = dr.GetString(iTipoevenabrev);

                    int iTipoevendesc = dr.GetOrdinal(helper.Tipoevendesc);
                    if (!dr.IsDBNull(iTipoevendesc)) entity.TipoEvenDesc = dr.GetString(iTipoevendesc);

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iOsigrupocodi = dr.GetOrdinal(helper.Osigrupocodi);
                    if (!dr.IsDBNull(iOsigrupocodi)) entity.Osigrupocodi = dr.GetString(iOsigrupocodi);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);

                    int iEquinomb = dr.GetOrdinal(helper.EquiNomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.EquiNomb = dr.GetString(iEquinomb);

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);

                    int iEventipoindisp = dr.GetOrdinal(helper.Intertipoindisp);
                    if (!dr.IsDBNull(iEventipoindisp)) entity.Intertipoindisp = dr.GetString(iEventipoindisp);

                    int iEvenpr = dr.GetOrdinal(helper.Interpr);
                    if (!dr.IsDBNull(iEvenpr)) entity.Interpr = Convert.ToDecimal(dr.GetValue(iEvenpr));

                    int iEvenasocproc = dr.GetOrdinal(helper.Interasocproc);
                    if (!dr.IsDBNull(iEvenasocproc)) entity.Interasocproc = dr.GetString(iEvenasocproc);

                    int iGrupotipocogen = dr.GetOrdinal(this.helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<InIntervencionDTO> ConsultarIntervencionesCruzadasTIITR(DateTime dFechaInicio, DateTime dFechaFin)
        {
            String query = String.Format(helper.SqlListarIntervencionesTIITR, dFechaInicio.ToString(ConstantesBase.FormatoFecha), dFechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<InIntervencionDTO> entitys = new List<InIntervencionDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    InIntervencionDTO entity = helper.Create(dr);

                    int iIntercodi = dr.GetOrdinal(helper.Intercodi);
                    if (!dr.IsDBNull(iIntercodi)) entity.Intercodi = Convert.ToInt32(dr.GetValue(iIntercodi));

                    int iTipoevencodi = dr.GetOrdinal(helper.Tipoevencodi);
                    if (!dr.IsDBNull(iTipoevencodi)) entity.Tipoevencodi = Convert.ToInt32(dr.GetValue(iTipoevencodi));

                    int iProgrcodi = dr.GetOrdinal(helper.Progrcodi);
                    if (!dr.IsDBNull(iProgrcodi)) entity.Progrcodi = Convert.ToInt32(dr.GetValue(iProgrcodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iInterfechaini = dr.GetOrdinal(helper.Interfechaini);
                    if (!dr.IsDBNull(iInterfechaini)) entity.Interfechaini = dr.GetDateTime(iInterfechaini);

                    int iInterfechafin = dr.GetOrdinal(helper.Interfechafin);
                    if (!dr.IsDBNull(iInterfechafin)) entity.Interfechafin = dr.GetDateTime(iInterfechafin);

                    int iClaprocodi = dr.GetOrdinal(helper.Claprocodi);
                    if (!dr.IsDBNull(iClaprocodi)) entity.Claprocodi = Convert.ToInt32(dr.GetValue(iClaprocodi));

                    int iIntermwindispo = dr.GetOrdinal(helper.Intermwindispo);
                    if (!dr.IsDBNull(iIntermwindispo)) entity.Intermwindispo = Convert.ToDecimal(dr.GetValue(iIntermwindispo));

                    int iInterindispo = dr.GetOrdinal(helper.Interindispo);
                    if (!dr.IsDBNull(iInterindispo)) entity.Interindispo = dr.GetString(iInterindispo);

                    int iInterinterrup = dr.GetOrdinal(helper.Interinterrup);
                    if (!dr.IsDBNull(iInterinterrup)) entity.Interinterrup = dr.GetString(iInterinterrup);

                    int iIntermantrelev = dr.GetOrdinal(helper.Intermantrelev);
                    if (!dr.IsDBNull(iIntermantrelev)) entity.Intermantrelev = Convert.ToInt32(dr.GetValue(iIntermantrelev));

                    int iInterdescrip = dr.GetOrdinal(helper.Interdescrip);
                    if (!dr.IsDBNull(iInterdescrip)) entity.Interdescrip = dr.GetString(iInterdescrip);

                    int iSubcausacodi = dr.GetOrdinal(helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

                    int iEvenclasecodi = dr.GetOrdinal(helper.Evenclasecodi);
                    if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = Convert.ToInt32(dr.GetValue(iEvenclasecodi));

                    int iEstadocodi = dr.GetOrdinal(helper.Estadocodi);
                    if (!dr.IsDBNull(iEstadocodi)) entity.Estadocodi = Convert.ToInt32(dr.GetValue(iEstadocodi));

                    //--------------------------------------------------------------------------------
                    // ASSETEC.SGH - 21/11/2017: CAMPOS ADICIONALES A LA ESTRUCTURA ORIGINAL
                    //--------------------------------------------------------------------------------
                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iInterconexionprov = dr.GetOrdinal(helper.Interconexionprov);
                    if (!dr.IsDBNull(iInterconexionprov)) entity.Interconexionprov = dr.GetString(iInterconexionprov);

                    int iIntersistemaaislado = dr.GetOrdinal(helper.Intersistemaaislado);
                    if (!dr.IsDBNull(iIntersistemaaislado)) entity.Intersistemaaislado = dr.GetString(iIntersistemaaislado);

                    //--------------------------------------------------------------------------------
                    // ASSETEC.SGH - 11/10/2017: CAMPOS ADICIONALES CALCULADOS
                    //--------------------------------------------------------------------------------
                    int iEmprNomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iAreaNomb = dr.GetOrdinal(helper.AreaNomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

                    //--------------------------------------------------------------------------------
                    // ASSETEC.SGH - 22/01/2017: CAMPOS ADICIONALES CALCULADOS
                    //--------------------------------------------------------------------------------
                    int iFamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iTareacodi = dr.GetOrdinal(helper.Tareacodi);
                    if (!dr.IsDBNull(iTareacodi)) entity.Tareacodi = Convert.ToInt32(dr.GetValue(iTareacodi));

                    int iEquicod = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicod)) entity.Equicodi = dr.GetInt32(iEquicod);

                    int iEquipot = dr.GetOrdinal(helper.Equipot);
                    if (!dr.IsDBNull(iEquipot)) entity.Equipot = Convert.ToDecimal(dr.GetValue(iEquipot));

                    int iEquimaniobra = dr.GetOrdinal(helper.Equimaniobra);
                    if (!dr.IsDBNull(iEquimaniobra)) entity.Equimaniobra = dr.GetString(iEquimaniobra);

                    //operador
                    int iOperadoremprcodi = dr.GetOrdinal(helper.Operadoremprcodi);
                    if (!dr.IsDBNull(iOperadoremprcodi)) entity.Operadoremprcodi = Convert.ToInt32(dr.GetValue(iOperadoremprcodi));

                    //Indicador de solo lectura de la programación
                    int iProgrsololectura = dr.GetOrdinal(helper.Progrsololectura);
                    if (!dr.IsDBNull(iProgrsololectura)) entity.Progrsololectura = dr.GetInt32(iProgrsololectura);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

    }
}
