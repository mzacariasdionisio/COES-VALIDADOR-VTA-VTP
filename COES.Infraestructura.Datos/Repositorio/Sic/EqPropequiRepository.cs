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
    /// Clase de acceso a datos de la tabla EQ_PROPEQUI
    /// </summary>
    public class EqPropequiRepository : RepositoryBase, IEqPropequiRepository
    {
        public EqPropequiRepository(string strConn) : base(strConn)
        {
        }

        EqPropequiHelper helper = new EqPropequiHelper();

        public void Save(EqPropequiDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Propcodi, DbType.Int32, entity.Propcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Fechapropequi, DbType.DateTime, entity.Fechapropequi);
            dbProvider.AddInParameter(command, helper.Propequideleted, DbType.Int32, entity.Propequideleted);
            dbProvider.AddInParameter(command, helper.Valor, DbType.String, entity.Valor);
            dbProvider.AddInParameter(command, helper.Propequiobservacion, DbType.String, entity.Propequiobservacion);
            dbProvider.AddInParameter(command, helper.Propequiusucreacion, DbType.String, entity.Propequiusucreacion);
            dbProvider.AddInParameter(command, helper.Propequifeccreacion, DbType.DateTime, entity.Propequifeccreacion);
            dbProvider.AddInParameter(command, helper.Propequiusumodificacion, DbType.String, entity.Propequiusumodificacion);
            dbProvider.AddInParameter(command, helper.Propequifecmodificacion, DbType.DateTime, entity.Propequifecmodificacion);
            dbProvider.AddInParameter(command, helper.Propequisustento, DbType.String, entity.Propequisustento);
            dbProvider.AddInParameter(command, helper.Propequicheckcero, DbType.Int32, entity.Propequicheckcero);
            dbProvider.AddInParameter(command, helper.Propequicomentario, DbType.String, entity.Propequicomentario);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.ExecuteNonQueryAudit(command, entity.Propequiusucreacion);
        }

        public string SaveCambioEstado(EqPropequiDTO entity)
        {

            string resultado = "";

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SaveCambioEstadoFn);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Valor, DbType.Int32, entity.Valor);
            dbProvider.AddInParameter(command, helper.Epproycodi, DbType.Int32, entity.Epproycodi);
            dbProvider.AddInParameter(command, helper.Fechapropequi, DbType.String, entity.FechapropequiDesc);
            dbProvider.AddInParameter(command, helper.Propequicomentario, DbType.String, entity.Propequicomentario);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Propequiusucreacion);

            dbProvider.AddOutParameter(command, helper.Resultado, DbType.String, 4000);

            dbProvider.ExecuteNonQuery(command);

            resultado = dbProvider.GetParameterValue(command, helper.Resultado) == DBNull.Value ? "" : (string)dbProvider.GetParameterValue(command, helper.Resultado);

            return resultado;
        }

        public void Update(EqPropequiDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Valor, DbType.String, entity.Valor);
            dbProvider.AddInParameter(command, helper.Propequiobservacion, DbType.String, entity.Propequiobservacion);
            //dbProvider.AddInParameter(command, helper.Propequiobservacion, DbType.String, entity.Propequiobservacion);
            dbProvider.AddInParameter(command, helper.Propequideleted2, DbType.Int32, entity.Propequideleted2);
            dbProvider.AddInParameter(command, helper.Propequiusucreacion, DbType.String, entity.Propequiusucreacion);
            dbProvider.AddInParameter(command, helper.Propequifeccreacion, DbType.DateTime, entity.Propequifeccreacion);
            dbProvider.AddInParameter(command, helper.Propequiusumodificacion, DbType.String, entity.Propequiusumodificacion);
            dbProvider.AddInParameter(command, helper.Propequifecmodificacion, DbType.DateTime, entity.Propequifecmodificacion);
            dbProvider.AddInParameter(command, helper.Propequisustento, DbType.String, entity.Propequisustento);
            dbProvider.AddInParameter(command, helper.Propequicheckcero, DbType.Int32, entity.Propequicheckcero);
            dbProvider.AddInParameter(command, helper.Propequicomentario, DbType.String, entity.Propequicomentario);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Propcodi, DbType.Int32, entity.Propcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Fechapropequi, DbType.DateTime, entity.Fechapropequi);
            dbProvider.AddInParameter(command, helper.Propequideleted, DbType.Int32, entity.Propequideleted);

            dbProvider.ExecuteNonQuery(command);
        }


        public void UpdateCambioEstado(EqPropequiDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateCambioEstado);

            dbProvider.AddInParameter(command, helper.Valor, DbType.String, entity.Valor);
            dbProvider.AddInParameter(command, helper.Propequiobservacion, DbType.String, entity.Propequiobservacion);
            //dbProvider.AddInParameter(command, helper.Propequiobservacion, DbType.String, entity.Propequiobservacion);
            dbProvider.AddInParameter(command, helper.Propequideleted2, DbType.Int32, entity.Propequideleted2);
            dbProvider.AddInParameter(command, helper.Propequiusucreacion, DbType.String, entity.Propequiusucreacion);
            dbProvider.AddInParameter(command, helper.Propequifeccreacion, DbType.DateTime, entity.Propequifeccreacion);
            dbProvider.AddInParameter(command, helper.Propequiusumodificacion, DbType.String, entity.Propequiusumodificacion);
            //dbProvider.AddInParameter(command, helper.Propequifecmodificacion, DbType.DateTime, entity.Propequifecmodificacion);
            dbProvider.AddInParameter(command, helper.Propequisustento, DbType.String, entity.Propequisustento);
            dbProvider.AddInParameter(command, helper.Propequicheckcero, DbType.Int32, entity.Propequicheckcero);
            dbProvider.AddInParameter(command, helper.Propequicomentario, DbType.String, entity.Propequicomentario);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Epproycodi, DbType.Int32, entity.Epproycodi);
            dbProvider.AddInParameter(command, helper.Propcodi, DbType.Int32, entity.Propcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Fechapropequi, DbType.DateTime, entity.Fechapropequi);
            dbProvider.AddInParameter(command, helper.Propequideleted, DbType.Int32, entity.Propequideleted);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int propcodi, int equicodi, DateTime fechapropequi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Propcodi, DbType.Int32, propcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Fechapropequi, DbType.DateTime, fechapropequi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(int propcodi, int equicodi, DateTime fechapropequi, string username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Propequiusumodificacion, DbType.String, username);

            dbProvider.AddInParameter(command, helper.Propcodi, DbType.Int32, propcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Fechapropequi, DbType.DateTime, fechapropequi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EqPropequiDTO GetById(int propcodi, int equicodi, DateTime fechapropequi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Propcodi, DbType.Int32, propcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Fechapropequi, DbType.DateTime, fechapropequi);
            EqPropequiDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EqPropequiDTO> List()
        {
            List<EqPropequiDTO> entitys = new List<EqPropequiDTO>();
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

        public List<EqPropequiDTO> GetByCriteria()
        {
            List<EqPropequiDTO> entitys = new List<EqPropequiDTO>();
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

        public void EliminarHistorico(int propcodi, int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlEliminarHistorico);

            dbProvider.AddInParameter(command, helper.Propcodi, DbType.Int32, propcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);

            dbProvider.ExecuteNonQuery(command);
        }


        public List<EqPropequiDTO> ListarValoresVigentesPropiedades(DateTime fechaConsulta, string iEquipo, string iFamilia, string idEmpresa, string propiedades, string sNombrePropiedad, string esFichaTecnica)
        {
            var entitys = new List<EqPropequiDTO>();
            sNombrePropiedad = !string.IsNullOrEmpty(sNombrePropiedad) ? sNombrePropiedad.ToUpperInvariant() : string.Empty;
            string strCommand = string.Format(helper.SqlValoresPropiedadesVigentes, fechaConsulta.ToString(ConstantesBase.FormatoFecha), iEquipo, iFamilia, idEmpresa, propiedades, sNombrePropiedad, esFichaTecnica);
            DbCommand command = dbProvider.GetSqlStringCommand(strCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = dr.GetInt32(iFamcodi);

                    int iPropnomb = dr.GetOrdinal(helper.Propnomb);
                    if (!dr.IsDBNull(iPropnomb)) entity.Propnomb = Convert.ToString(dr.GetValue(iPropnomb));
                    int iPropunidad = dr.GetOrdinal(helper.Propunidad);
                    if (!dr.IsDBNull(iPropunidad)) entity.Propunidad = Convert.ToString(dr.GetValue(iPropunidad));
                    int iPropfile = dr.GetOrdinal(helper.Propfile);
                    if (!dr.IsDBNull(iPropfile)) entity.Propfile = Convert.ToString(dr.GetValue(iPropfile));

                    int iPropocultocomentario = dr.GetOrdinal(helper.Propocultocomentario);
                    if (!dr.IsDBNull(iPropocultocomentario)) entity.Propocultocomentario = Convert.ToString(dr.GetValue(iPropocultocomentario));

                    int iDesPropiedad = dr.GetOrdinal(helper.Propnomb);
                    if (!dr.IsDBNull(iDesPropiedad)) entity.Propnomb = dr.GetString(iDesPropiedad);

                    int iDesEquipo = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iDesEquipo)) entity.Equinomb = dr.GetString(iDesEquipo);

                    int iFechaActualizacion = dr.GetOrdinal(helper.Fechapropequi);
                    if (!dr.IsDBNull(iFechaActualizacion)) entity.Fechapropequi = dr.GetDateTime(iFechaActualizacion);

                    int iValor = dr.GetOrdinal(helper.Valor);
                    if (!dr.IsDBNull(iValor)) entity.Valor = dr.GetString(iValor);

                    int iIdPropiedad = dr.GetOrdinal(helper.Propcodi);
                    if (!dr.IsDBNull(iIdPropiedad)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iIdPropiedad));

                    int iIdEquipo = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iIdEquipo)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iIdEquipo));

                    int iIdEmpresa = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iIdEmpresa)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iIdEmpresa));

                    int iDesEmpresa = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iDesEmpresa)) entity.Emprnomb = dr.GetString(iDesEmpresa);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iIdFamilia = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iIdFamilia)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iIdFamilia));

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iAreadesc = dr.GetOrdinal(helper.Areadesc);
                    if (!dr.IsDBNull(iAreadesc)) entity.Areadesc = dr.GetString(iAreadesc);

                    int iEquiestado = dr.GetOrdinal(helper.Equiestado);
                    if (!dr.IsDBNull(iEquiestado)) entity.Equiestado = dr.GetString(iEquiestado);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqPropequiDTO> ListarValoresVigentesPropiedadesPaginado(int iEquipo, int iFamilia, string sNombrePropiedad, int nroPagina, int nroFilas)
        {
            var entitys = new List<EqPropequiDTO>();
            sNombrePropiedad = !string.IsNullOrEmpty(sNombrePropiedad) ? sNombrePropiedad.ToUpperInvariant() : string.Empty;
            string strCommand = string.Format(helper.SqlValoresPropiedadesVigentesPaginado, iEquipo, iFamilia, nroPagina, nroFilas, sNombrePropiedad);
            DbCommand command = dbProvider.GetSqlStringCommand(strCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oEntity = new EqPropequiDTO();

                    int iPropcodi = dr.GetOrdinal(helper.Propcodi);
                    if (!dr.IsDBNull(iPropcodi)) oEntity.Propcodi = Convert.ToInt32(dr.GetValue(iPropcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) oEntity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iFechapropequi = dr.GetOrdinal(helper.Fechapropequi);
                    if (!dr.IsDBNull(iFechapropequi)) oEntity.Fechapropequi = Convert.ToDateTime(dr.GetValue(iFechapropequi));

                    int iValor = dr.GetOrdinal(helper.Valor);
                    if (!dr.IsDBNull(iValor)) oEntity.Valor = Convert.ToString(dr.GetValue(iValor));

                    int iPropnomb = dr.GetOrdinal("PROPNOMB");
                    if (!dr.IsDBNull(iPropnomb)) oEntity.Propnomb = Convert.ToString(dr.GetValue(iPropnomb));
                    int iPropunidad = dr.GetOrdinal("PROPUNIDAD");
                    if (!dr.IsDBNull(iPropunidad)) oEntity.Propunidad = Convert.ToString(dr.GetValue(iPropunidad));

                    #region ObservacionesValorPropiedades
                    int iPropequiobservacion = dr.GetOrdinal(helper.Propequiobservacion);
                    if (!dr.IsDBNull(iPropequiobservacion)) oEntity.Propequiobservacion = Convert.ToString(dr.GetValue(iPropequiobservacion));
                    #endregion

                    int iFechapropequiCreacion = dr.GetOrdinal(helper.Propequifeccreacion);
                    if (!dr.IsDBNull(iFechapropequiCreacion)) oEntity.Propequifeccreacion = Convert.ToDateTime(dr.GetValue(iFechapropequiCreacion));

                    int iUsuarioCreacion = dr.GetOrdinal(helper.Propequiusucreacion);
                    if (!dr.IsDBNull(iUsuarioCreacion)) oEntity.Propequiusucreacion = Convert.ToString(dr.GetValue(iUsuarioCreacion));

                    entitys.Add(oEntity);
                }
            }

            return entitys;
        }

        public int TotalListarValoresVigentesPropiedadesPaginado(int iFamilia, string sNombrePropiedad)
        {
            sNombrePropiedad = !string.IsNullOrEmpty(sNombrePropiedad) ? sNombrePropiedad.ToUpperInvariant() : string.Empty;
            String query = String.Format(helper.SqlTotalValoresPropiedadesVigentesPaginado, iFamilia, sNombrePropiedad);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public List<EqPropequiDTO> ListarValoresHistoricosPropiedadPorEquipo(int iEquipo, string iPropiedad)
        {
            var entitys = new List<EqPropequiDTO>();
            String query = String.Format(helper.SqlHistoricoPropiedad, iEquipo, iPropiedad);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

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

        public string GetValorPropiedad(int idPropiedad, int idEquipo)
        {
            string valor = string.Empty;
            string query = string.Format(helper.SqlGetValorPropiedad, idPropiedad, idEquipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            EqPropequiDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }
            if (entity != null)
                valor = entity.Valor;
            return valor;
        }

        public string ObtenerValorPropiedadEquipoFecha(int propcodi, int equicodi, string fecha)
        {
            string strCommand = string.Format(helper.SqlObtenerValorPropiedadEquipoFecha, propcodi, equicodi, fecha);
            DbCommand command = dbProvider.GetSqlStringCommand(strCommand);
            object valor = dbProvider.ExecuteScalar(command);
            if (valor != null) return Convert.ToString(valor);

            return "";


        }
        /// <summary>
        /// Método que copia los últimos valores vigentes de un equipo a otro.
        /// </summary>
        /// <param name="iEquipoOriginal">Código Equipo original</param>
        /// <param name="iEquipoDestino">Código Equipo destino</param>
        /// <param name="usuario">Nombre de usuario</param>
        public void CopiarValoresPropiedadEquipo(int iEquipoOriginal, int iEquipoDestino, string usuario)
        {
            string queryPropiedades = string.Format(helper.SqlPropiedadesVigentesEquipoCopiar, iEquipoOriginal);
            DbCommand command = dbProvider.GetSqlStringCommand(queryPropiedades);

            List<EqPropequiDTO> listaValoresVigentes = new List<EqPropequiDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oEntity = new EqPropequiDTO();
                    int iPropcodi = dr.GetOrdinal("PROPCODI");
                    if (!dr.IsDBNull(iPropcodi)) oEntity.Propcodi = Convert.ToInt32(dr.GetValue(iPropcodi));
                    int iValor = dr.GetOrdinal("VALOR");
                    if (!dr.IsDBNull(iValor)) oEntity.Valor = Convert.ToString(dr.GetValue(iValor));
                    oEntity.Fechapropequi = DateTime.Today;
                    oEntity.Equicodi = iEquipoDestino;
                    oEntity.Propequiusucreacion = usuario;
                    oEntity.Propequifeccreacion = DateTime.Now;
                    listaValoresVigentes.Add(oEntity);
                }
            }
            foreach (var oPropiedadNueva in listaValoresVigentes)
            {
                this.Save(oPropiedadNueva);
            }
        }

        #region SIOSEIN

        public List<EqPropequiDTO> GetPotEfectivaAndPotInstaladaPorUnidad(string strCodEquipos)
        {
            var entitys = new List<EqPropequiDTO>();
            String query = String.Format(helper.SqlGetPotEfectivaAndPotInstaladaPorUnidad, strCodEquipos);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqPropequiDTO entity = new EqPropequiDTO();

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iPropcodi = dr.GetOrdinal(this.helper.Propcodi);
                    if (!dr.IsDBNull(iPropcodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iPropcodi));

                    int iPropnomb = dr.GetOrdinal(this.helper.Propnomb);
                    if (!dr.IsDBNull(iPropnomb)) entity.Propnomb = dr.GetString(iPropnomb);

                    int iValor = dr.GetOrdinal(this.helper.Valor);
                    if (!dr.IsDBNull(iValor)) entity.Valor = Convert.ToString(dr.GetValue(iValor));

                    int iFechapropequi = dr.GetOrdinal(this.helper.Fechapropequi);
                    if (!dr.IsDBNull(iFechapropequi)) entity.Fechapropequi = Convert.ToDateTime(dr.GetValue(iFechapropequi));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = dr.GetInt32(iFamcodi);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        #endregion

        #region NotificacionesCambiosEquipamiento
        public List<EqPropequiDTO> ListadoPropiedadesValoresModificados(int emprCodi, int famCodi, DateTime dtFechaInicio, DateTime dtFechaFin)
        {
            var entitys = new List<EqPropequiDTO>();
            String query = String.Format(helper.SqlPropiedadesModificadas, emprCodi, famCodi, dtFechaInicio.ToString("dd-MM-yyyy HH:mm"), dtFechaFin.ToString("dd-MM-yyyy HH:mm"));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iDesPropiedad = dr.GetOrdinal(helper.Propnomb);
                    if (!dr.IsDBNull(iDesPropiedad)) entity.Propnomb = dr.GetString(iDesPropiedad);

                    int iDesEquipo = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iDesEquipo)) entity.Equinomb = dr.GetString(iDesEquipo);

                    int iIdEmpresa = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iIdEmpresa)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iIdEmpresa));

                    int iDesEmpresa = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iDesEmpresa)) entity.Emprnomb = dr.GetString(iDesEmpresa);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iIdFamilia = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iIdFamilia)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iIdFamilia));

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iEquiestado = dr.GetOrdinal(helper.Equiestado);
                    if (!dr.IsDBNull(iEquiestado)) entity.Equiestado = dr.GetString(iEquiestado);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        #endregion

        #region Numerales Datos Base
        public List<EqPropequiDTO> ListaNumerales_DatosBase_5_6_5()
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBase_5_6_5);

            List<EqPropequiDTO> entitys = new List<EqPropequiDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqPropequiDTO entity = new EqPropequiDTO();

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iOsigrupocodi = dr.GetOrdinal(helper.Osigrupocodi);
                    if (!dr.IsDBNull(iOsigrupocodi)) entity.Osigrupocodi = dr.GetString(iOsigrupocodi);

                    int iFechapropequi = dr.GetOrdinal(helper.Fechapropequi);
                    if (!dr.IsDBNull(iFechapropequi)) entity.Fechapropequi = dr.GetDateTime(iFechapropequi);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iValor = dr.GetOrdinal(helper.Valor);
                    if (!dr.IsDBNull(iValor)) entity.Valor = dr.GetString(iValor);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        #endregion

        #region FICHA TÉCNICA

        public void Save(EqPropequiDTO entity, IDbConnection connection, IDbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                //object result = dbCommand.ExecuteScalar();
                //int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propcodi, DbType.Int32, entity.Propcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equicodi, DbType.Int32, entity.Equicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fechapropequi, DbType.DateTime, entity.Fechapropequi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propequideleted, DbType.Int32, entity.Propequideleted));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Valor, DbType.String, entity.Valor));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propequiobservacion, DbType.String, entity.Propequiobservacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propequiusucreacion, DbType.String, entity.Propequiusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propequifeccreacion, DbType.DateTime, entity.Propequifeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propequiusumodificacion, DbType.String, entity.Propequiusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propequifecmodificacion, DbType.DateTime, entity.Propequifecmodificacion));

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propequisustento, DbType.String, entity.Propequisustento));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propequicheckcero, DbType.Int32, entity.Propequicheckcero));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propequicomentario, DbType.String, entity.Propequicomentario));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Lastuser, DbType.String, entity.Lastuser));

                dbCommand.ExecuteNonQuery();
            }
        }

        public void Update(EqPropequiDTO entity, IDbConnection connection, IDbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlUpdate;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Valor, DbType.String, entity.Valor));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propequiobservacion, DbType.String, entity.Propequiobservacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propequideleted2, DbType.Int32, entity.Propequideleted2));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propequiusucreacion, DbType.String, entity.Propequiusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propequifeccreacion, DbType.DateTime, entity.Propequifeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propequiusumodificacion, DbType.String, entity.Propequiusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propequifecmodificacion, DbType.DateTime, entity.Propequifecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propequisustento, DbType.String, entity.Propequisustento));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propequicheckcero, DbType.Int32, entity.Propequicheckcero));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propequicomentario, DbType.String, entity.Propequicomentario));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Lastuser, DbType.String, entity.Lastuser));

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propcodi, DbType.Int32, entity.Propcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equicodi, DbType.Int32, entity.Equicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fechapropequi, DbType.DateTime, entity.Fechapropequi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propequideleted, DbType.Int32, entity.Propequideleted));

                dbCommand.ExecuteNonQuery();
            }
        }

        public List<EqPropequiDTO> ListarEquipoConValorModificado(DateTime dtFechaInicio, DateTime dtFechaFin, string propcodis, string famcodis)
        {
            var entitys = new List<EqPropequiDTO>();
            string query = String.Format(helper.SqlListarEquipoConValorModificado, dtFechaInicio.ToString(ConstantesBase.FormatoFecha),
                                                            dtFechaFin.ToString(ConstantesBase.FormatoFecha), propcodis, famcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqPropequiDTO entity = new EqPropequiDTO();

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquiestado = dr.GetOrdinal(helper.Equiestado);
                    if (!dr.IsDBNull(iEquiestado)) entity.Equiestado = dr.GetString(iEquiestado);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iDesEmpresa = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iDesEmpresa)) entity.Emprnomb = dr.GetString(iDesEmpresa);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFechapropequi = dr.GetOrdinal(this.helper.Fechapropequi);
                    if (!dr.IsDBNull(iFechapropequi)) entity.Fechapropequi = Convert.ToDateTime(dr.GetValue(iFechapropequi));

                    int iIdEmpresa = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iIdEmpresa)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iIdEmpresa));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = dr.GetInt32(iFamcodi);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        #endregion

        #region GestProtect

        public EqPropequiDTO GetIdCambioEstado(int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetIdCambioEstado);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            EqPropequiDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new EqPropequiDTO();
                    int iFechapropequi = dr.GetOrdinal(helper.Fechapropequi);
                    if (!dr.IsDBNull(iFechapropequi)) entity.Fechapropequi = Convert.ToDateTime(dr.GetValue(iFechapropequi));

                    int iValor = dr.GetOrdinal(helper.Valor);
                    if (!dr.IsDBNull(iValor)) entity.Valor = Convert.ToString(dr.GetValue(iValor));

                    int iEpproycodi = dr.GetOrdinal(helper.Epproycodi);
                    if (!dr.IsDBNull(iEpproycodi)) entity.Epproycodi = Convert.ToInt32(dr.GetValue(iEpproycodi));

                    int iPropequicomentario = dr.GetOrdinal(helper.Propequicomentario);
                    if (!dr.IsDBNull(iPropequicomentario)) entity.Propequicomentario = Convert.ToString(dr.GetValue(iPropequicomentario));
                }
            }

            return entity;
        }

        #endregion



    }
}
