using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System.Configuration;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Informe;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.General;
using System.Globalization;
using COES.Framework.Base.Tools;
using static COES.Servicios.Aplicacion.Migraciones.Helper.ConstantesMigraciones;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace COES.Servicios.Aplicacion.Evento
{
    /// <summary>
    /// Clases con métodos del módulo Evento
    /// </summary>
    public class EventosAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(EventosAppServicio));

        #region Métodos Tabla EVE_EVENTO

        /// <summary>
        /// Inserta un registro de la tabla EVE_EVENTO
        /// </summary>
        public int SaveEveEvento(EveEventoDTO entity, List<int> listEquipo, string user, string remitente)
        {
            try
            {
                int id = 0;
                GregorianCalendar Calendario = new GregorianCalendar();
                int semana = 0;
                string operacion = string.Empty;

                if (listEquipo.Count > 0)
                {
                    int equicodi = listEquipo[0];
                    EqEquipoDTO equipo = FactorySic.GetEqEquipoRepository().GetById(equicodi);
                    entity.Emprcodi = equipo.Emprcodi;
                    entity.Emprcodirespon = equipo.Emprcodi;
                    entity.Equicodi = equicodi;
                }

                entity.Lastdate = DateTime.Now;
                entity.Lastuser = user;

                int countInfFallan1 = 0;
                int countInfFallan2 = 0;

                if (entity.Evencodi == 0)
                {
                    int idEvento = (new GeneralAppServicio()).ObtenerNextIdTabla(ConstantesEvento.TablaEvento);
                    entity.Evencodi = idEvento;
                    id = FactorySic.GetEveEventoRepository().Save(entity);
                    (new GeneralAppServicio()).ActualizarIdTabla(ConstantesEvento.TablaEvento, idEvento);
                    operacion = ConstantesEvento.TextoLogNuevo;
                }
                else
                {
                    FactorySic.GetEveEvenequipoRepository().DeleteEquipos(entity.Evencodi);
                    FactorySic.GetEveEventoRepository().Update(entity);
                    id = entity.Evencodi;
                    operacion = ConstantesEvento.TextoLogActualizacion;
                    countInfFallan1 = FactorySic.GetEveInformefallaRepository().ValidarInformeFallaN1(id);
                    countInfFallan2 = FactorySic.GetEveInformefallaN2Repository().ValidarInformeFallaN2(id);
                }

                foreach (int item in listEquipo)
                {
                    EveEvenequipoDTO itemEquipo = new EveEvenequipoDTO();
                    itemEquipo.Equicodi = item;
                    itemEquipo.Evencodi = id;
                    itemEquipo.Lastdate = DateTime.Now;
                    itemEquipo.Lastuser = user;

                    FactorySic.GetEveEvenequipoRepository().Save(itemEquipo);
                }


                /**Verificamos en la tabla de informe de fallas n1 y n2**/

                semana = Calendario.GetWeekOfYear((DateTime)entity.Evenini, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Saturday);
                DateTime fecha = (DateTime)entity.Evenini;
                //string PathInforme = @"\\fs\Areas\SCO\";
                string PathInforme = ConstantesAppServicio.FileSystemSco;
            

                if (entity.Eveninffalla == ConstantesAppServicio.SI)
                {
                    if (countInfFallan1 == 0 && (entity.Tipoevencodi == 4 || entity.Tipoevencodi == 5))
                    {
                        decimal mwMax = 3700;
                        string infMen = ConstantesAppServicio.NO;
                        if ((entity.Evenmwindisp / mwMax) * 100 >= 5) infMen = ConstantesAppServicio.SI;

                        int correlativoMen = 0;
                        int correlativo = 0;
                        int correlativoSco = 0;
                        int anio = (entity.Evenini != null) ? ((DateTime)entity.Evenini).Year : 0;
                        FactorySic.GetEveInformefallaRepository().ObtenerCorrelativoInformeFalla(anio, out correlativoMen,
                            out correlativo, out correlativoSco);

                        if (infMen != ConstantesAppServicio.SI)
                            correlativoMen = 0;

                        int idInformeFalla = (new GeneralAppServicio()).ObtenerNextIdTabla(ConstantesEvento.TablaInformeFalla);

                        EveInformefallaDTO informeN1 = new EveInformefallaDTO();
                        informeN1.Eveninfcodi = idInformeFalla;
                        informeN1.Evencodi = id;
                        informeN1.Evenanio = anio;
                        informeN1.Evencorr = correlativo;
                        informeN1.Eveninflastuser = entity.Lastuser;
                        informeN1.Eveninflastdate = DateTime.Now;
                        informeN1.Eveninfemitido = ConstantesAppServicio.NO;
                        informeN1.Eveninfpemitido = ConstantesAppServicio.NO;
                        informeN1.Eveninfpiemitido = ConstantesAppServicio.NO;
                        informeN1.Eveninfmem = infMen;
                        informeN1.Evencorrmem = correlativoMen;
                        informeN1.Eveninfmememitido = ConstantesAppServicio.NO;
                        informeN1.EvencorrSco = correlativoSco;
                        informeN1.Eveninfactuacion = entity.Evenactuacion;
                        informeN1.Eveninfplazodiasipi = entity.Eveninfplazodiasipi;
                        informeN1.Eveninfplazodiasif = entity.Eveninfplazodiasif;
                        informeN1.Eveninfplazohoraipi = entity.Eveninfplazohoraipi;
                        informeN1.Eveninfplazohoraif = entity.Eveninfplazohoraif;
                        informeN1.Eveninfplazominipi = entity.Eveninfplazominipi;
                        informeN1.Eveninfplazominif = entity.Eveninfplazominif;
                        FactorySic.GetEveInformefallaRepository().SaveEvento(informeN1);

                        (new GeneralAppServicio()).ActualizarIdTabla(ConstantesEvento.TablaInformeFalla, idInformeFalla);

                        try
                        {
                            //- Generamos la carpeta de informe de fallas
                            string pathN1 =
                                ConstantesEvento.CarpetaInformeFallaN1 +
                                @"/" + anio + @"/Sem" + semana.ToString("00") + anio.ToString().Substring(2, 2) +
                                @"/" + fecha.Day.ToString("00") + fecha.Month.ToString("00") +
                                @"/E" + correlativo + @"/";

                            FileServer.CreateFolder(pathN1, "IF", PathInforme);
                            FileServer.CreateFolder(pathN1, "IP", PathInforme);
                            FileServer.CreateFolder(pathN1, "IPI", PathInforme);
                            FileServer.CreateFolder(pathN1, "Otros", PathInforme);

                            //- Generamos la carpeta de informe ministerio
                            if (infMen == ConstantesAppServicio.SI)
                            {
                                string pathMinisterio =
                                ConstantesEvento.CarpetaInformeFallaN1 +
                                @"/" + anio + @"/" + ConstantesEvento.CarpetaInformeMinisterio +
                                @"/Sem" + semana.ToString("00") + anio.ToString().Substring(2, 2) +
                                @"/" + fecha.Day.ToString("00") + fecha.Month.ToString("00") +
                                @"/E" + correlativoMen + @"/";

                                FileServer.CreateFolder(pathMinisterio, string.Empty, PathInforme);
                            }
                        }
                        catch
                        {
                        }
                    }
                    if (countInfFallan1 > 0 && (entity.Tipoevencodi == 4 || entity.Tipoevencodi == 5))
                    {
                        EveInformefallaDTO informeN1 = new EveInformefallaDTO();

                        if (entity.Eveninfcodi == 0)
                            informeN1.Eveninfcodi = MostrarEventoInformeFalla(entity.Evencodi).Eveninfcodi;
                        else
                            informeN1.Eveninfcodi = entity.Eveninfcodi;
                        
                        informeN1.Eveninfplazodiasipi = entity.Eveninfplazodiasipi;
                        informeN1.Eveninfplazodiasif = entity.Eveninfplazodiasif;
                        informeN1.Eveninfplazohoraipi = entity.Eveninfplazohoraipi;
                        informeN1.Eveninfplazohoraif = entity.Eveninfplazohoraif;
                        informeN1.Eveninfplazominipi = entity.Eveninfplazominipi;
                        informeN1.Eveninfplazominif = entity.Eveninfplazominif;
                        FactorySic.GetEveInformefallaRepository().ActualizarAmpliacion(informeN1);
                    }

                }
                else
                {
                    if (countInfFallan1 > 0)
                        FactorySic.GetEveInformefallaRepository().EliminarInformeFallaN1(id);
                }
                if (entity.Eveninffallan2 == ConstantesAppServicio.SI)
                {
                    if (countInfFallan2 == 0)
                    {

                        int correlativo = 0;
                        int anio = (entity.Evenini != null) ? ((DateTime)entity.Evenini).Year : 0;
                        FactorySic.GetEveInformefallaN2Repository().ObtenerCorrelativoInformeFallaN2(anio, out correlativo);

                        int idInformeFallaN2 = (new GeneralAppServicio()).ObtenerNextIdTabla(ConstantesEvento.TablaInformeFallaN2);

                        EveInformefallaN2DTO informeN2 = new EveInformefallaN2DTO();
                        informeN2.Eveninfn2codi = idInformeFallaN2;
                        informeN2.Evencodi = id;
                        informeN2.Evenanio = anio;
                        informeN2.Evenn2corr = correlativo;
                        informeN2.Eveninfn2lastuser = entity.Lastuser;
                        informeN2.Eveninfn2lastdate = DateTime.Now;
                        informeN2.Eveninfpin2emitido = ConstantesAppServicio.NO;
                        informeN2.Eveninffn2emitido = ConstantesAppServicio.NO;
                        informeN2.Eveninfplazodiasipi = entity.Eveninfplazodiasipi;
                        informeN2.Eveninfplazodiasif = entity.Eveninfplazodiasif;
                        informeN2.Eveninfplazohoraipi = entity.Eveninfplazohoraipi;
                        informeN2.Eveninfplazohoraif = entity.Eveninfplazohoraif;
                        informeN2.Eveninfplazominipi = entity.Eveninfplazominipi;
                        informeN2.Eveninfplazominif = entity.Eveninfplazominif;
                        FactorySic.GetEveInformefallaN2Repository().SaveEvento(informeN2);

                        (new GeneralAppServicio()).ActualizarIdTabla(ConstantesEvento.TablaInformeFallaN2, idInformeFallaN2);

                        try
                        {
                            string pathN2 =
                                ConstantesEvento.CarpetaInformeFallaN2 +
                                @"/" + anio + @"/Sem" + semana.ToString("00") + anio.ToString().Substring(2, 2) +
                                @"/" + fecha.Day.ToString("00") + fecha.Month.ToString("00") +
                                @"/E" + correlativo + @"/";

                            FileServer.CreateFolder(pathN2, "IF", PathInforme);
                            FileServer.CreateFolder(pathN2, "IPI", PathInforme);
                        }
                        catch
                        {
                        }
                    }
                    if (countInfFallan2 > 0)
                    {
                        EveInformefallaN2DTO informeN2 = new EveInformefallaN2DTO();
                        if (entity.Eveninfn2codi == 0)
                            informeN2.Eveninfn2codi = MostrarEventoInformeFallaN2(entity.Evencodi).Eveninfn2codi;
                        else
                            informeN2.Eveninfn2codi = entity.Eveninfn2codi;

                        informeN2.Eveninfplazodiasipi = entity.Eveninfplazodiasipi;
                        informeN2.Eveninfplazodiasif = entity.Eveninfplazodiasif;
                        informeN2.Eveninfplazohoraipi = entity.Eveninfplazohoraipi;
                        informeN2.Eveninfplazohoraif = entity.Eveninfplazohoraif;
                        informeN2.Eveninfplazominipi = entity.Eveninfplazominipi;
                        informeN2.Eveninfplazominif = entity.Eveninfplazominif;
                        FactorySic.GetEveInformefallaN2Repository().ActualizarAmpliacionN2(informeN2);
                    }

                }
                else
                {
                    if (countInfFallan2 > 0)
                        FactorySic.GetEveInformefallaN2Repository().EliminarInformeFallaN2(id);
                }


                this.GrabarAuditoria(operacion, id, user);

                if (entity.Evencodi == 0 && (entity.Tipoevencodi == ConstantesEvento.TipoEventoEvento
                    || entity.Tipoevencodi == ConstantesEvento.TipoEventoFalla))
                {
                    string email = ConfigurationManager.AppSettings[ConstantesEvento.EmailList];
                    string asunto = string.Format(ConstantesEvento.AsuntoEmailEvento, entity.Evenasunto,
                        ((DateTime)entity.Evenini).ToString(ConstantesBase.FormatoFechaHora));
                    string mensaje = HelperEvento.ObtenerMensajeEvento(id, entity.Evenasunto, (DateTime)entity.Evenini, remitente);

                    //Activar cuando se inicie la sección de carga de informe de los agentes
                    //COES.Base.Tools.Util.SendEmail(email, asunto, mensaje);
                }

                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite validar la fecha del evento
        /// </summary>
        /// <param name="fechaEvento"></param>
        public bool ValidarFechaRegistro(DateTime fechaEvento)
        {
            DateTime fecha = DateTime.Now;

            if (fecha.Year == fechaEvento.Year && fecha.Month == fechaEvento.Month &&
                fecha.Day == fechaEvento.Day)
            {
                return true;
            }
            else
            {
                TimeSpan ts = fecha.Subtract(fecha);

                if (ts.TotalSeconds > 0)
                {
                    if (ts.TotalDays == 1)
                    {
                        if (fecha.Hour < 7)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Permite grabar el evento tipo bitacora
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GrabarBitacora(EveEventoDTO entity, string username)
        {
            try
            {
                int id = 0;
                string operacion = string.Empty;
                if (entity.Evencodi == 0)
                {

                    int idEvento = (new GeneralAppServicio()).ObtenerNextIdTabla(ConstantesEvento.TablaEvento);
                    entity.Evencodi = idEvento;
                    id = FactorySic.GetEveEventoRepository().Save(entity);
                    (new GeneralAppServicio()).ActualizarIdTabla(ConstantesEvento.TablaEvento, idEvento);
                    operacion = ConstantesEvento.TextoLogNuevo;
                }
                else
                {
                    FactorySic.GetEveEventoRepository().Update(entity);
                    id = entity.Evencodi;
                    operacion = ConstantesEvento.TextoLogActualizacion;
                }

                this.GrabarAuditoria(operacion, id, username);

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EVE_EVENTO
        /// </summary>
        public void UpdateEveEvento(EveEventoDTO entity)
        {
            try
            {
                FactorySic.GetEveEventoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EVE_EVENTO
        /// </summary>
        public void DeleteEveEvento(int evencodi, string username)
        {
            try
            {
                FactorySic.GetEveEventoRepository().Delete(evencodi);
                FactorySic.GetEveEventoRepository().Delete_UpdateAuditoria(evencodi,username);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void ActualizarEventoAseguramiento(int idEvento)
        {

            try
            {
                FactorySic.GetEveEventoRepository().ActualizarEventoAseguramiento(idEvento);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EVE_EVENTO
        /// </summary>
        public EveEventoDTO GetByIdEveEvento(int evencodi)
        {
            return FactorySic.GetEveEventoRepository().GetById(evencodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_EVENTO
        /// </summary>
        public List<EveEventoDTO> ListEveEventos()
        {
            return FactorySic.GetEveEventoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EveEvento
        /// </summary>
        public List<EveEventoDTO> GetByCriteriaEveEventos()
        {
            return FactorySic.GetEveEventoRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite consultar los eventos para los agentes de la extranet
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idTipoEvento"></param>
        /// <param name="nroPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<EveEventoDTO> ConsultaEventoExtranet(DateTime fechaInicio, DateTime fechaFin,
            int? idTipoEvento, int nroPage, int pageSize)
        {
            return FactorySic.GetEveEventoRepository().ConsultaEventoExtranet(fechaInicio, fechaFin,
                idTipoEvento, nroPage, pageSize);
        }

        /// <summary>
        /// Permite obtener el número de registros de la consulta extranet
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idTipoEvento"></param>
        /// <returns></returns>
        public int ObtenerNroRegistrosConsultaExtranet(DateTime fechaInicio, DateTime fechaFin,
            int? idTipoEvento)
        {
            return FactorySic.GetEveEventoRepository().ObtenerNroRegistrosConsultaExtranet(fechaInicio, fechaFin,
                idTipoEvento);
        }

        /// <summary>
        /// Permite ontener los datos del evento
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        public EveEventoDTO GetDetalleEvento(int idEvento)
        {
            return FactorySic.GetEveEventoRepository().GetDetalleEvento(idEvento);
        }


        /// <summary>
        /// Permite obtener los equipos involucrados en un evento
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> GetEquiposPorEvento(int idEvento)
        {
            return FactorySic.GetEveEvenequipoRepository().GetEquiposPorEvento(idEvento.ToString());
        }

        /// <summary>
        /// Permite grabar el equipo asociado
        /// </summary>
        /// <param name="entity"></param>
        public void GrabarEventoEquipo(EveEvenequipoDTO entity)
        {
            try
            {
                FactorySic.GetEveEvenequipoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Metodos Adicionales

        /// <summary>
        /// Permite obtener los tipos de evento
        /// </summary>
        /// <returns></returns>
        public List<EveTipoeventoDTO> ListarTipoEvento()
        {
            return FactorySic.GetEveTipoeventoRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener las causas del evento
        /// </summary>
        /// <param name="idTipoEvento"></param>
        /// <returns></returns>
        public List<EveSubcausaeventoDTO> ObtenerCausaEvento(int idTipoEvento)
        {
            return FactorySic.GetEveSubcausaeventoRepository().GetByCriteria(idTipoEvento);
        }

        ///// <summary>
        ///// Permite obtener las causas del evento
        ///// </summary>
        ///// <param name="idTipoEvento"></param>
        ///// <returns></returns>
        //public List<EveSubcausaeventoDTO> ObtenerCausasEvento(int idTipoEvento)
        //{
        //    return FactorySic.GetEveSubcausaeventoRepository().ObtenerSubcausaEvento(idTipoEvento);
        //}

        /// <summary>
        /// Permite listar las empresas del SEIN
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresas()
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasSEIN();
        }
        #endregion

        #region Interrupciones

        /// <summary>
        /// Permite listar los puntos de interrupcion
        /// </summary>
        /// <returns></returns>
        public List<EvePtointerrupDTO> ListaPuntosInterrupcion()
        {
            return FactorySic.GetEvePtointerrupRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener los datos de una interrupcion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EveInterrupcionDTO ObtenerInterrupcion(int id)
        {
            return FactorySic.GetEveInterrupcionRepository().GetById(id);
        }

        /// <summary>
        /// Permite eliminar una interrupcion
        /// </summary>
        /// <param name="id"></param>
        public void EliminarInterrupcion(int id, int idEvento, string username)
        {
            FactorySic.GetEveInterrupcionRepository().Delete(id);
            this.GrabarAuditoria(ConstantesEvento.TextoLogEliminacionInterrupcion, idEvento, username);
        }

        /// <summary>
        /// Permite grabar un interrupcion
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int GrabarInterrupcion(EveInterrupcionDTO entity, int idItemInforme, string username)
        {
            try
            {
                int id = 0;
                string operacion = string.Empty;
                if (entity.Interrupcodi == 0)
                {
                    id = FactorySic.GetEveInterrupcionRepository().Save(entity);

                    if (idItemInforme > 0)
                    {
                        FactorySic.GetEveInformeItemRepository().ActualizarInformeItem(idItemInforme, (int)entity.Ptointerrcodi);
                    }

                    operacion = ConstantesEvento.TextoLogLlenadoInterrupcion;
                }
                else
                {
                    FactorySic.GetEveInterrupcionRepository().Update(entity);
                    id = entity.Interrupcodi;
                    operacion = ConstantesEvento.TextoLogEdicionInterrupcion;
                }

                this.GrabarAuditoria(operacion, (int)entity.Evencodi, username);

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite cambiar la version de un documento
        /// </summary>
        /// <param name="idEvento"></param>
        /// <param name="version"></param>
        /// <param name="username"></param>
        public int CambiarVersion(int idEvento, string version, string username)
        {
            try
            {
                EveEventoDTO evento = FactorySic.GetEveEventoRepository().GetById(idEvento);

                if (evento.Tipoevencodi == 4 || evento.Tipoevencodi == 5)
                {
                    FactorySic.GetEveEventoRepository().CambiarVersion(idEvento, version, username);
                    FactorySic.GetEveEvenequipoRepository().DeleteEquipos(idEvento);
                    EveEvenequipoDTO itemEquipo = new EveEvenequipoDTO();
                    itemEquipo.Equicodi = (int)evento.Equicodi;
                    itemEquipo.Evencodi = idEvento;
                    itemEquipo.Lastdate = DateTime.Now;
                    itemEquipo.Lastuser = username;
                    FactorySic.GetEveEvenequipoRepository().Save(itemEquipo);

                    this.GrabarAuditoria(ConstantesEvento.TextoLogConvertirAFinal, idEvento, username);
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite cambiar la versión de un evento a bitácora
        /// </summary>
        /// <param name="idEvento"></param>
        /// <param name="version"></param>
        /// <param name="username"></param>
        public void CambiarBitacora(int idEvento, string version, string username)
        {
            try
            {
                EveEventoDTO evento = FactorySic.GetEveEventoRepository().GetById(idEvento);
                FactorySic.GetEveEventoRepository().CambiarVersion(idEvento, version, username);
                this.GrabarAuditoria(ConstantesEvento.TextoLogConvertirBitacora, idEvento, username);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos Tabla EVE_EVENTO_LOG

        /// <summary>
        /// Inserta un registro de la tabla EVE_EVENTO_LOG
        /// </summary>
        public void GrabarAuditoria(string operacion, int idEvento, string username)
        {
            try
            {
                EveEventoLogDTO entity = new EveEventoLogDTO();
                entity.Desoperacion = operacion;
                entity.Evencodi = idEvento;
                entity.Lastdate = DateTime.Now;
                entity.Lastuser = username;

                FactorySic.GetEveEventoLogRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_EVENTO_LOG
        /// </summary>
        public List<EveEventoLogDTO> ListEveEventoLogs(int idEvento)
        {
            return FactorySic.GetEveEventoLogRepository().List(idEvento);
        }




        #endregion

        #region Mejoras CTAF
        /// <summary>
        /// Permite consultar los eventos para los agentes de la extranet
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<EveEventoDTO> ConsultaEventoSco(DateTime fechaInicio, DateTime fechaFin)
        {
            return FactorySic.GetEveEventoRepository().ListadoEventoSco(fechaInicio, fechaFin);
        }

        /// <summary>
        /// Método que valida los plazos y muestra el color del plazo envío
        /// </summary>
        /// <param name="lista"></param>
        public void CalcularPlazoEventoSco(List<EveEventoDTO> lista)
        {
            foreach (var val in lista)
            {
                DeterminarPlazoEventoSco(val);
            }
        }

        /// <summary>
        /// Permite deterner plazo de envío
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public void DeterminarPlazoEventoSco(EveEventoDTO val)
        {
            val.ColorPlazoIPI = "";
            val.ColorPlazoIF = "";
            val.ColorTextoPlazoIPI = "";
            val.ColorTextoPlazoIF = "";

            if (val.EvenIni != null)
            {
                DateTime fechaActual = DateTime.Now;
                DateTime fechaVencimientoIPI = GetFechaFinEnPlazo(val, ConstantesEvento.PlazoMinIPI, "IPI");
                DateTime fechaVencimientoIF = GetFechaFinEnPlazo(val, ConstantesEvento.PlazoMinIF, "IF");

                if (fechaVencimientoIPI >= fechaActual)
                {
                    var horas = (fechaActual - fechaVencimientoIPI).ToString(@"dd\d\ hh\h\ mm\m\ ss\s");
                    val.PlazoEnvioIPI = "Tiempo para vencer el plazo " + horas + " h";
                    val.ColorPlazoIPI = "";
                    val.ColorTextoPlazoIPI = "#5764FB";
                    val.EnPlazoIPI = true;
                }
                else if (fechaVencimientoIPI < fechaActual)
                {
                    var horas = (fechaVencimientoIPI - fechaActual).ToString(@"dd\d\ hh\h\ mm\m\ ss\s");
                    val.PlazoEnvioIPI = "Plazo vencido hace " + horas + " h";
                    val.ColorPlazoIPI = "#FF7C92";
                    val.ColorTextoPlazoIPI = "Black";
                    val.EnPlazoIPI = false;
                }             

                if (fechaVencimientoIF >= fechaActual)
                {
                    var horas = (fechaActual - fechaVencimientoIF).ToString(@"dd\d\ hh\h\ mm\m\ ss\s");
                    val.PlazoEnvioIF = "Tiempo para vencer el plazo " + horas + " h";
                    val.ColorPlazoIF = "";
                    val.ColorTextoPlazoIF = "#5764FB";
                    val.EnPlazoIF = true;
                }
                else if (fechaVencimientoIF < fechaActual)
                {
                    var horas = (fechaVencimientoIF - fechaActual).ToString(@"dd\d\ hh\h\ mm\m\ ss\s");
                    val.PlazoEnvioIF = "Plazo vencido hace " + horas + " h";
                    val.ColorPlazoIF = "#FF7C92";
                    val.ColorTextoPlazoIF = "Black";
                    val.EnPlazoIF = false;
                }
            }
        }

        /// <summary>
        /// Obtener la fecha de vencimiento de plazo
        /// </summary>
        /// <param name="_evento"></param>
        /// <param name="_minutes"></param>
        /// <param name="_tipo"></param>
        /// <returns></returns>
        public DateTime GetFechaFinEnPlazo(EveEventoDTO _evento, int _minutes, string _tipo)
        {
            int _min_amp = 0;
            if (_tipo == "IPI" && _evento.Eveninffalla == "S")
                _min_amp = Convert.ToInt32(_evento.Eveninfplazodiasipi) * 1440 + Convert.ToInt32(_evento.Eveninfplazohoraipi) * 60 + Convert.ToInt32(_evento.Eveninfplazominipi);
            else if (_tipo == "IPI" && _evento.Eveninffallan2 == "S")
                _min_amp = Convert.ToInt32(_evento.Eveninfplazodiasipi_N2) * 1440 + Convert.ToInt32(_evento.Eveninfplazohoraipi_N2) * 60 + Convert.ToInt32(_evento.Eveninfplazominipi_N2);
            else if (_tipo == "IF" && _evento.Eveninffalla == "S")
                _min_amp = Convert.ToInt32(_evento.Eveninfplazodiasif) * 1440 + Convert.ToInt32(_evento.Eveninfplazohoraif) * 60 + Convert.ToInt32(_evento.Eveninfplazominif);
            else if (_tipo == "IF" && _evento.Eveninffallan2 == "S")
                _min_amp = Convert.ToInt32(_evento.Eveninfplazodiasif_N2) * 1440 + Convert.ToInt32(_evento.Eveninfplazohoraif_N2) * 60 + Convert.ToInt32(_evento.Eveninfplazominif_N2);

            DateTime fechaEvento = _evento.EvenIni.AddMinutes(_minutes);
            return fechaEvento.AddMinutes(_min_amp);


        }

        /// <summary>
        /// Mostrar datos del informe de fallas
        /// </summary>
        /// <param name="evencodi"></param>
        /// <returns></returns>
        public EveInformefallaDTO MostrarEventoInformeFalla(int evencodi)
        {
            return FactorySic.GetEveInformefallaRepository().MostrarEventoInformeFalla(evencodi);
        }
        /// <summary>
        /// Mostrar datos del informe de fallas N2
        /// </summary>
        /// <param name="evencodi"></param>
        /// <returns></returns>

        public EveInformefallaN2DTO MostrarEventoInformeFallaN2(int evencodi)
        {
            return FactorySic.GetEveInformefallaN2Repository().MostrarEventoInformeFallaN2(evencodi);
        }

        /// <summary>
        /// Inserta un registro de la tabla ME_ENVIO_EVE_EVENTO
        /// </summary>
        public int SaveMeEnvioEveEvento(MeEnvioEveEventoDTO entity)
        {
            try
            {
                return FactorySic.GetMeEnvioEveEventoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Inserta un registro de la tabla ME_ENVIO_EVE_EVENTO
        /// </summary>
        public int SaveEveInformesSco(EveInformesScoDTO entity)
        {
            try
            {
                return FactorySic.GetEveInformesScoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Permite listar los informes enviados
        /// </summary>
        /// <param name="IdEvento"></param>
        /// <param name="TipoInforme"></param>
        /// <returns></returns>
        public List<EveInformesScoDTO> ListEveInformesSco(int IdEvento, int TipoInforme)
        {
            return FactorySic.GetEveInformesScoRepository().List(IdEvento, TipoInforme);
        }
        /// <summary>
        /// Permite obtener datos de Empresa
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public SiEmpresaDTO ObtenerEmpresa(int emprcodi)
        {
            return FactorySic.GetSiEmpresaRepository().GetById(emprcodi);
        }
        /// <summary>
        /// Permite obtener Empresa por usuario
        /// </summary>
        /// <param name="userlogin"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresaGetByUser(string userlogin)
        {
            return FactorySic.GetSiEmpresaRepository().GetByUser(userlogin);
        }
        /// <summary>
        /// Genera evento CTAF - Actualiza el campo Evenctaf = 'S' en la tabla EVE_EVENTO
        /// </summary>
        public void generarEventoCtaf(int evencodi)
        {
            try
            {
                FactorySic.GetEveEventoRepository().UpdateEventoCtaf(evencodi,"S");
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Inserta en tabla media los eventos relacionados entre sí
        /// </summary>
        public void insertarEventoEvento(int evencodi, int evencodi_as)
        {
            try
            {
                FactorySic.GetEveEventoRepository().insertarEventoEvento(evencodi, evencodi_as);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Mostrar listado de informes enviados
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<MeEnvioDTO> ListaInformeEnvios(int idEvento)
        {
            return FactorySic.GetMeEnvioRepository().ListaInformeEnvios(idEvento);
        }

        /// <summary>
        /// Mostrar log listado de informes enviados
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<MeEnvioDTO> ListaInformeEnviosLog(MeEnvioDTO entity)
        {
            return FactorySic.GetMeEnvioRepository().ListaInformeEnviosLog(entity);
        }
        /// <summary>
        /// Permite listar los informes enviados
        /// </summary>
        /// <param name="evencodi"></param>
        /// <param name="envetapainforme"></param>
        /// <returns></returns>
        public List<EveInformesScoDTO> ListEveInformesScoxEvento(int evencodi, int envetapainforme)
        {
            return FactorySic.GetEveInformesScoRepository().ListInformesSco(evencodi, envetapainforme);
        }
        /// <summary>
        /// Permite actualizar los informes que se verán en Portal Web
        /// </summary>
        public void ActualizarInformePortalWeb(int eveinfcodi, string PortalWeb, string eveinfcodigo)
        {
            try
            {
                FactorySic.GetEveInformesScoRepository().ActualizarInformePortalWeb(eveinfcodi, PortalWeb, eveinfcodigo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<EventoDTO> ListarEventosSCO(AnalisisFallaDTO dto)
        {
            return FactorySic.ObtenerEventoDao().ListarEventosSCO(dto);
        }
        /// <summary>
        /// Obtener datos de análisis de falla
        /// </summary>
        /// <param name="evencodi"></param>
        /// <returns></returns>
        public EventoDTO ObtenerEventoCtafByEvencodi(int evencodi)
        {
            EventoDTO evento = FactorySic.ObtenerEventoDao().ObtenerInterrupcionByEvencodi(evencodi);
            return evento;
        }
        /// <summary>
        /// Permite obtener los datos de los informes sco.
        /// </summary>
        /// <param name="eveinfcodi"></param>
        /// <returns></returns>
        public EveInformesScoDTO ObtenerInformeSco(int eveinfcodi)
        {
            return FactorySic.GetEveInformesScoRepository().ObtenerInformeSco(eveinfcodi);
        }
        /// <summary>
        /// Permite obtener los datos de análisis de falla por evencodi.
        /// </summary>
        /// <param name="evencodi"></param>
        /// <returns></returns>
        public AnalisisFallaDTO ObtenerAnalisisFalla(int evencodi)
        {
            return FactorySic.ObtenerEventoDao().ObtenerAnalisisFallaxEvento(evencodi);
        }
        #endregion

        /// <summary>
        /// Permite obtener los lista de datos de análisis de falla por evencodi.
        /// </summary>
        /// <param name="evencodi"></param>
        /// <returns></returns>
        public List<AnalisisFallaDTO> ListarAnalisisFalla(int evencodi)
        {
            return FactorySic.ObtenerEventoDao().ListarAnalisisFallaxEvento(evencodi);
        }

        /// <summary>
        /// Inserta en tabla media los eventos relacionados entre sí
        /// </summary>
        public int procesarEventoCtaf(List<int> objEvento, string usuario)
        {
            IDbConnection conn = null, conn1 = null;
            DbTransaction tran = null, tran1 = null;
            bool resultVal = false;
            int resulTran = -1;
            string estado = "S", fileSev = string.Empty, fileSevAgrupado = string.Empty;
            int esMultipleEvento = 0, rtn = -1;
            List<AnalisisFallaDTO> lstEventosCtaf = new List<AnalisisFallaDTO>();
            CriteriosEventoAppServicio servCriterio = new CriteriosEventoAppServicio();
            AnalisisFallasAppServicio servAf = new AnalisisFallasAppServicio();
            try
            {
                conn = FactoryTransferencia.GetVtdValorizacion().BeginConnection();
                tran = FactoryTransferencia.GetVtdValorizacion().StartTransaction(conn);
                conn1 = FactoryTransferencia.GetVtdValorizacion().BeginConnection();
                tran1 = FactoryTransferencia.GetVtdValorizacion().StartTransaction(conn1);

                //FactorySic.GetEveEventoRepository().insertarEventoEvento(evencodi, evencodi_as);

                foreach (var item in objEvento)
                {
                    foreach (var x in objEvento)
                    {
                        resulTran = FactorySic.GetEveEventoRepository().insertarEventoEventoR(item, x, conn, tran);
                        if (resulTran == -1)
                        {
                            tran.Rollback();
                            Logger.Error("Error insertando registro en Tabla eve_evento_evento.");
                            throw new Exception("Error insertando registro en Tabla eve_evento_evento.");
                        }
                    }
                    resulTran = FactorySic.GetEveEventoRepository().UpdateEventoCtafR(item, estado, conn, tran);
                }

                if(resulTran == -1) 
                {
                    tran.Rollback();
                    Logger.Error("Error insertando registro en Tabla eve_evento_evento.");
                    throw new Exception("Error insertando registro en Tabla eve_evento_evento.");
                }
                else
                {
                    //Se ejecutan las transacciones a BD por ser necesario el AF
                    tran.Commit();
                    //Carpetas en Fs SEV
                    foreach (var y in objEvento)
                    {
                        List<AnalisisFallaDTO> lstAnalisisFallaDTO = this.ListarAnalisisFalla(y);

                        foreach (AnalisisFallaDTO itemAF in lstAnalisisFallaDTO)
                        {
                            if (itemAF.AFECODI > 0)
                                lstEventosCtaf.Add(itemAF);
                        }

                    }

                    if (lstEventosCtaf.Count > 0)
                    {
                        esMultipleEvento = lstEventosCtaf.DistinctBy(x => x.EVENCODI).Count();
                        //Obtener datos de evento más antiguo
                        AnalisisFallaDTO _Evento = lstEventosCtaf.OrderBy(x => x.EVENINI).First();
                        AnalisisFallaDTO Evento = this.ObtenerAnalisisFalla(_Evento.EVENCODI);

                        string aaaa = Evento.EVENINI.Value.ToString("yyyy");
                        string FsSev = ConstantesEvento.FileSystemSev;

                        string NombreEvento = Evento.CODIGO + "_" + Evento.EVENINI.Value.ToString("dd.MM.yyyy");
                        DateTime FechaFinSem1 = DateTime.ParseExact(ConstantesEvento.FechaFinSem1 + aaaa, ConstantesEvento.FormatoFecha2, CultureInfo.InvariantCulture);
                        DateTime FechaInicioSem2 = DateTime.ParseExact(ConstantesEvento.FechaInicioSem2 + aaaa, ConstantesEvento.FormatoFecha2, CultureInfo.InvariantCulture);
                        DateTime FechaFinSem2 = DateTime.ParseExact(ConstantesEvento.FechaFinSem2 + aaaa, ConstantesEvento.FormatoFecha2, CultureInfo.InvariantCulture);
                        DateTime FechaEvento = DateTime.ParseExact(Evento.EVENINI.Value.ToString("dd/MM/yyyy"), ConstantesEvento.FormatoFecha2, CultureInfo.InvariantCulture);
                        string semestre = string.Empty;
                        string folderPathSev = string.Empty;
                        if (FechaEvento <= FechaFinSem1)
                        {
                            semestre = "Semestre I";
                        }
                        else if (FechaEvento >= FechaInicioSem2 && FechaEvento <= FechaFinSem2)
                        {
                            semestre = "Semestre II";
                        }

                        fileSev = aaaa + "\\" + semestre + "\\" + NombreEvento + "\\";

                        if (esMultipleEvento == 1)
                        {
                            string fileSevValidar = FsSev + fileSev;
                            if (fileSevValidar.Length > 247)
                                return -2;

                            FileServer.CreateFolder(null, fileSev, FsSev);

                            folderPathSev = FsSev + fileSev;

                            resulTran = UploadFileSev(lstEventosCtaf, fileSev, 1, Evento.EVENCODI);
                            if (resulTran == -1)
                            {
                                FileServer.DeleteFolderAlter("", folderPathSev);
                                servAf.eliminarEventoCtaf(Evento.EVENCODI);
                                Logger.Error("Error copiando archivos.");
                                throw new Exception("Error copiando archivos.");
                                
                                return -1;
                            }
                            resulTran = UploadFileSev(lstEventosCtaf, fileSev, 2, Evento.EVENCODI);
                            if (resulTran == -1)
                            {
                                FileServer.DeleteFolderAlter("", folderPathSev);
                                servAf.eliminarEventoCtaf(Evento.EVENCODI);
                                Logger.Error("Error insertando registro en Tabla cr_etapa_evento.");
                                throw new Exception("Error copiando archivos.");
                                return -1;
                            }
                        }
                        else if (esMultipleEvento > 1)
                        {

                            foreach (var evento in lstEventosCtaf)
                            {
                                fileSevAgrupado = fileSev + evento.EVENINI.Value.ToString("dd.MM.yyyy HH.mm") + "\\";
                                string fileSevValidar = FsSev + fileSevAgrupado;
                                if (fileSevValidar.Length > 247)
                                    return -2;
                                else
                                {
                                    FileServer.CreateFolder(null, fileSevAgrupado, FsSev);
                                    folderPathSev = FsSev + fileSevAgrupado;
                                    resulTran = UploadFileSev(lstEventosCtaf, fileSevAgrupado, 1, evento.EVENCODI);
                                    if (resulTran == -1)
                                    {
                                        FileServer.DeleteFolderAlter("", folderPathSev);
                                        servAf.eliminarEventoCtaf(Evento.EVENCODI);
                                        Logger.Error("Error copiando archivos.");
                                        throw new Exception("Error copiando archivos.");
                                        return -1;
                                    }
                                    resulTran = UploadFileSev(lstEventosCtaf, fileSevAgrupado, 2, evento.EVENCODI);
                                    if (resulTran == -1)
                                    {
                                        FileServer.DeleteFolderAlter("", folderPathSev);
                                        servAf.eliminarEventoCtaf(Evento.EVENCODI);
                                        Logger.Error("Error copiando archivos");
                                        throw new Exception("Error copiando archivos.");
                                        return -1;
                                    }
                                }
                            }
                        }

                        #region InsertarCriterios
                        
                        CrEventoDTO CrCriterio = new CrEventoDTO();
                        int idcrevento = 0;
                        CrCriterio.AFECODI = Evento.AFECODI;
                        CrCriterio.CRESPECIALCODI = 0;
                        CrCriterio.LASTDATE = DateTime.Now;
                        CrCriterio.LASTUSER = usuario;
                        idcrevento = servCriterio.SaveCrEventoR(CrCriterio, conn1, tran1);
                        resulTran = idcrevento;
                        if (resulTran == -1)
                        {
                            tran1.Rollback();
                            FileServer.DeleteFolderAlter("", folderPathSev);
                            servAf.eliminarEventoCtaf(Evento.EVENCODI);
                            Logger.Error("Error insertando registro en Tabla cr_evento.");
                            throw new Exception("Error insertando registro en Tabla cr_evento.");
                        }
                        else
                        {
                            for (int x = 1; x <= 4; x++)
                            {
                                CrEtapaEventoDTO CrEtapa = new CrEtapaEventoDTO();
                                CrEtapa.CREVENCODI = idcrevento;
                                CrEtapa.CRETAPA = x;
                                CrEtapa.LASTUSER = usuario;
                                CrEtapa.LASTDATE = DateTime.Now;
                                resulTran = servCriterio.SaveCrEtapaEventoR(CrEtapa, conn1, tran1);

                                if (resulTran == -1)
                                {
                                    tran1.Rollback();
                                    FileServer.DeleteFolderAlter("", folderPathSev);
                                    servAf.eliminarEventoCtaf(Evento.EVENCODI);
                                    Logger.Error("Error insertando registro en Tabla cr_etapa_evento.");
                                    throw new Exception("Error insertando registro en Tabla cr_etapa_evento.");
                                }
                            }
                        }

                        #endregion
                        tran1.Commit();
                    }
                }
                rtn = 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();

                if (conn1 != null)
                    if (conn1.State == ConnectionState.Open) conn1.Close();
            }

            return rtn;
        }

        public int UploadFileSev(List<AnalisisFallaDTO> lstEventosCtaf, string rutaSev, int tipo, int evencodi)
        {
            EventosAppServicio servEvento = new EventosAppServicio();
            //Obtener lista de informes finales e informes preliminares
            List<EveInformesScoDTO> lstInfFinales = new List<EveInformesScoDTO>();
            List<EveInformesScoDTO> lstInfPreliminares = new List<EveInformesScoDTO>();
            foreach (var evento in lstEventosCtaf.DistinctBy(x => x.EVENCODI))
            {
                if (tipo == 1 && evento.EVENCODI == evencodi)
                {
                    List<EveInformesScoDTO> lstInformesPreliminares = servEvento.ListEveInformesScoxEvento(evento.EVENCODI, 1).ToList(); //Lista de informes preliminares
                    lstInfPreliminares.AddRange(lstInformesPreliminares);
                }
                else if (tipo == 2 && evento.EVENCODI == evencodi)
                {
                    List<EveInformesScoDTO> lstInformesFinales = servEvento.ListEveInformesScoxEvento(evento.EVENCODI, 2).ToList(); //Lista de informes finales
                    lstInfFinales.AddRange(lstInformesFinales);
                }
            }

            //Copiar IPIs de evento Sco a Sev si es que los hubiera.
            foreach (var informeP in lstInfPreliminares)
            {
                EveInformesScoDTO informe = servEvento.ObtenerInformeSco(informeP.Eveinfcodi);
                string etapa = informe.Afiversion == 1 ? "IPI" : "IF";
                string foldername = string.Empty;
                string filename = informe.Eveinfrutaarchivo;
                string fileserverSco = ConstantesEvento.FileSystemSCO;
                string fileserverSev = ConstantesEvento.FileSystemSev;
                EveInformefallaDTO InformeFalla = new EveInformefallaDTO();
                EveInformefallaN2DTO InformeFallaN2 = new EveInformefallaN2DTO();
                if (informe.Eveninffalla == "S")
                {
                    InformeFalla = servEvento.MostrarEventoInformeFalla(informe.Evencodi);
                    foldername = ConstantesEnviarCorreoS.CarpetaInformeFallaN1 + "\\" + informe.Anio + "\\" + informe.Semestre + "\\" + informe.Diames + "\\E" + InformeFalla.Evencorr.ToString() + "\\" + etapa + "\\" + informe.Emprnomb.Trim() + "\\" + informe.Env_Evencodi.ToString() + "\\";
                }
                else if (informe.Eveninffallan2 == "S")
                {
                    InformeFallaN2 = servEvento.MostrarEventoInformeFallaN2(informe.Evencodi);
                    foldername = ConstantesEnviarCorreoS.CarpetaInformeFallaN2 + "\\" + informe.Anio + "\\" + informe.Semestre + "\\" + informe.Diames + "\\E" + InformeFallaN2.Evenn2corr.ToString() + "\\" + etapa + "\\" + informe.Emprnomb.Trim() + "\\" + informe.Env_Evencodi.ToString() + "\\";
                }
                string rfSev = rutaSev + informeP.Emprnomb + "\\IPI\\" + informe.Env_Evencodi.ToString() + "\\";
                string fileSevValida = fileserverSev + rfSev;
                if (fileSevValida.Length > 247)
                    return -2;

                FileServer.CreateFolder(null, rfSev, fileserverSev);
                FileServer.UploadFromFileDirectory(fileserverSco + foldername + filename, "", rfSev + filename, fileserverSev);
            }
            //Copiar IFs de evento Sco a Sev si es que los hubiera.
            foreach (var informeF in lstInfFinales)
            {
                EveInformesScoDTO informe = servEvento.ObtenerInformeSco(informeF.Eveinfcodi);
                string etapa = informe.Afiversion == 1 ? "IPI" : "IF";
                string foldername = string.Empty;
                string filename = informe.Eveinfrutaarchivo;
                string fileserverSco = ConstantesEvento.FileSystemSCO;
                string fileserverSev = ConstantesEvento.FileSystemSev;
                EveInformefallaDTO InformeFalla = new EveInformefallaDTO();
                EveInformefallaN2DTO InformeFallaN2 = new EveInformefallaN2DTO();
                if (informe.Eveninffalla == "S")
                {
                    InformeFalla = servEvento.MostrarEventoInformeFalla(informe.Evencodi);
                    foldername = ConstantesEnviarCorreoS.CarpetaInformeFallaN1 + "\\" + informe.Anio + "\\" + informe.Semestre + "\\" + informe.Diames + "\\E" + InformeFalla.Evencorr.ToString() + "\\" + etapa + "\\" + informe.Emprnomb.Trim() + "\\" + informe.Env_Evencodi.ToString() + "\\";
                }
                else if (informe.Eveninffallan2 == "S")
                {
                    InformeFallaN2 = servEvento.MostrarEventoInformeFallaN2(informe.Evencodi);
                    foldername = ConstantesEnviarCorreoS.CarpetaInformeFallaN2 + "\\" + informe.Anio + "\\" + informe.Semestre + "\\" + informe.Diames + "\\E" + InformeFallaN2.Evenn2corr.ToString() + "\\" + etapa + "\\" + informe.Emprnomb.Trim() + "\\" + informe.Env_Evencodi.ToString() + "\\";
                }
                string rfSev = rutaSev + informeF.Emprnomb + "\\IF\\" + informe.Env_Evencodi.ToString() + "\\";
                string fileSevValida = fileserverSev + rfSev;
                if (fileSevValida.Length > 247)
                    return -2;

                FileServer.CreateFolder(null, rfSev, fileserverSev);
                FileServer.UploadFromFileDirectory(fileserverSco + foldername + filename, "", rfSev + filename, fileserverSev);
            }

            return 1;
        }
    }
}
