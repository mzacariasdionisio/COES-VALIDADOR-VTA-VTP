using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Infraestructura.Datos.Repositorio.Sic;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;

namespace COES.Servicios.Aplicacion.Equipamiento
{
    public class NotificacionCambioEquipos
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NotificacionCambioEquipos));
        
        public NotificacionCambioEquipos()
        {
            //log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Permite obtener el cuerpo del correo a enviar
        /// </summary>
        /// <param name="listaEquipo"></param>
        /// <param name="listaPropiedad"></param>
        /// <param name="listaGrupo"></param>
        protected string ObtenerCuerpoCorreo(List<EqEquipoDTO> listaEquipo, List<EqPropequiDTO> listaPropiedad, List<PrGrupodatDTO> listaGrupo)
        {
            StringBuilder htmlEquipo = new StringBuilder();
            StringBuilder htmlPropiedad = new StringBuilder();
            StringBuilder htmlGrupo = new StringBuilder();

            foreach (EqEquipoDTO equipo in listaEquipo)
            {
                string sUsuario;
                DateTime? dFecha;

                if (equipo.FechaUpdate.HasValue)
                {
                    sUsuario = equipo.UsuarioUpdate;
                    dFecha = equipo.FechaUpdate;
                }
                else
                {
                    sUsuario = equipo.Lastuser;
                    dFecha = equipo.Lastdate;
                }

                htmlEquipo.Append(String.Format(EquipamientoHelper.Html.HtmlEquipo, equipo.EMPRNOMB, equipo.Equinomb, equipo.Equiabrev,equipo.Famnomb, sUsuario, dFecha));
            }

            foreach (EqPropequiDTO propiedad in listaPropiedad)
            {
                htmlPropiedad.Append(String.Format(EquipamientoHelper.Html.HtmlPropiedad, propiedad.Emprnomb, propiedad.Propnomb, propiedad.Equinomb,propiedad.Valor, propiedad.FechapropequiDesc, propiedad.UltimaModificacionUsuarioDesc, propiedad.UltimaModificacionFechaDesc));
            }

            foreach (PrGrupodatDTO grupo in listaGrupo)
            {
                htmlGrupo.Append(String.Format(EquipamientoHelper.Html.HtmlGrupo, grupo.GrupoNomb, grupo.ConcepDesc, grupo.Formuladat, grupo.Fechadat, grupo.Lastuser,grupo.Fechaact));
            }

            String mensaje = String.Format(EquipamientoHelper.Html.HtmlCuerpo, htmlEquipo.ToString(), htmlPropiedad.ToString(), htmlGrupo.ToString());
            mensaje = mensaje.Replace("[", "{");
            mensaje = mensaje.Replace("]", "}");


            return mensaje;
        }

        /// <summary>
        /// Permite obtener el cuerpo del correo a enviar
        /// </summary>
        /// <param name="listaEquipo"></param>
        /// <param name="listaPropiedad"></param>
        /// <param name="listaGrupo"></param>
        protected string ObtenerCuerpoCorreoCurva(List<PrGrupodatDTO> listaGrupo)
        {
            StringBuilder htmlGrupo = new StringBuilder();

            foreach (PrGrupodatDTO grupo in listaGrupo)
            {
                htmlGrupo.Append(String.Format(EquipamientoHelper.Html.HtmlGrupo, grupo.GrupoNomb, grupo.ConcepDesc, grupo.Formuladat, grupo.Lastuser, grupo.Fechaact));
            }

            String mensaje = String.Format(EquipamientoHelper.Html.HtmlCuerpoCurva, htmlGrupo.ToString());
            mensaje = mensaje.Replace("[", "{");
            mensaje = mensaje.Replace("]", "}");


            return mensaje;
        }

        public void Procesar(int nroMinutos)
        {
            try
            {
                DateTime dtFechaInicio = DateTime.Now.AddMinutes(-1 * nroMinutos);
                DateTime dtFechaFin = DateTime.Now;

                List<EqEquipoDTO> listaEquipo = Factory.FactorySic.GetEqEquipoRepository().ListadoEquiposModificados(dtFechaInicio, dtFechaFin);
                List<EqPropequiDTO> listaPropiedad = ListadoPropiedadesValoresModificados(-1, -1, dtFechaInicio, dtFechaFin);
                List<PrGrupodatDTO> listaGrupo = Factory.FactorySic.GetPrGrupodatRepository().ListadoConceptosActualizados(dtFechaInicio, dtFechaFin);

                if (listaEquipo.Count > 0 || listaPropiedad.Count > 0 || listaGrupo.Count > 0)
                {
                    string mensaje = this.ObtenerCuerpoCorreo(listaEquipo, listaPropiedad, listaGrupo);

                    string sTo = ConfigurationManager.AppSettings["EquipamientoNotificacionMailTo"];
                    string sBcc = ConfigurationManager.AppSettings["EquipamientoNotificacionMailBcc"];
                    List<string> listTo = new List<string>();
                    listTo = sTo.Split(';').ToList();
                    List<string> listBCc = new List<string>();
                    listBCc = sBcc.Split(';').ToList();
                    List<string> listCC= new List<string>();

                    Base.Tools.Util.SendEmail(listTo, listCC, listBCc, "Notificación de Cambios en Equipamiento", mensaje, "equipamiento-noreply@coes.org.pe");
                    log.Info("El proceso Alerta Equipamiento ejecutado a las  {0} ha sido existoso. "+ dtFechaFin.ToString());
                }
                else
                {
                    log.Info("El proceso Alerta Equipamiento ejecutado a las  {0} no tuvo datos que enviar. "+ dtFechaFin.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error("El proceso Alerta Equipamiento ejecutado a las  {0} ha sido fallido. "+DateTime.Now,ex);
                throw ex;
            }
        }

        /// <summary>
        /// Permite crear las notificaciones para en caso existan cambios en la curva de ensayo de potencia
        /// </summary>
        /// <param name="nroMinutos"></param>
        public void ProcesarNotificacionCurvaEnsayo(int nroMinutos)
        {
            try
            {
                DateTime dtFechaInicio = DateTime.Now.AddMinutes(-1 * nroMinutos);
                DateTime dtFechaFin = DateTime.Now;
                int[] categorias = { 14, 175, 176, 177, 178, 179, 180, 181, 182, 183 };

                List<PrGrupodatDTO> listaGrupo = Factory.FactorySic.GetPrGrupodatRepository().ListadoConceptosActualizados(dtFechaInicio,
                    dtFechaFin).Where(x => categorias.Any(y => x.Concepcodi == y)).ToList();

                if (listaGrupo.Count > 0)
                {
                    string mensaje = this.ObtenerCuerpoCorreoCurva(listaGrupo);

                    string sTo = ConfigurationManager.AppSettings["EquipamientoNotificacionCurva"];
                    string sBcc = ConfigurationManager.AppSettings["EquipamientoNotificacionMailBcc"];
                    List<string> listTo = new List<string>();
                    listTo.Add(sTo);
                    List<string> listBCc = new List<string>();
                    listBCc.Add(sBcc);
                    List<string> listCC = new List<string>();

                    Base.Tools.Util.SendEmail(listTo, listCC, listBCc, "Notificación de Cambios en Curva de Ensayo de Potencia", mensaje, "equipamiento-noreply@coes.org.pe");
                    //log.Info("El proceso Alerta Equipamiento ejecutado a las  {0} ha sido existoso. " + DateTime.Now.ToString());
                }
                else
                {
                    //log.Info("El proceso Alerta Equipamiento ejecutado a las  {0} no tuvo datos que enviar. " + DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
                //log.Error("El proceso Alerta Equipamiento ejecutado a las  {0} ha sido fallido. " + DateTime.Now, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Listado Equipos Modificados
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListadoEquiposModificados(DateTime f1, DateTime f2)
        {
            return Factory.FactorySic.GetEqEquipoRepository().ListadoEquiposModificados(f1, f2);
        }

        /// <summary>
        /// Listado Propiedades Valores Modificados
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns></returns>
        public List<EqPropequiDTO> ListadoPropiedadesValoresModificados(int emprcodi, int famcodi, DateTime f1, DateTime f2)
        {
            var lista = Factory.FactorySic.GetEqPropequiRepository().ListadoPropiedadesValoresModificados(emprcodi, famcodi, f1, f2);

            foreach (var reg in lista)
            {
                EquipamientoAppServicio.FormatearPropequi(reg);
            }

            return lista;
        }

        /// <summary>
        /// Listado Conceptos Actualizados
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns></returns>
        public List<PrGrupodatDTO> ListadoConceptosActualizados(DateTime f1, DateTime f2)
        {
            return Factory.FactorySic.GetPrGrupodatRepository().ListadoConceptosActualizados(f1, f2);
        }

        #region Interfas reporte

        /// <summary>
        /// Reporte cambios de Parametros
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <param name="count"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public List<string> ReporteParametros(List<EqEquipoDTO> equiposmod, List<EqEquipoDTO> equiposnew, List<EqPropequiDTO> equiposbaj, List<EqPropequiDTO> propiedadesEqui, List<PrGrupodatDTO> listaGrupo, List<EqPropequiDTO> operacionesCome)
        {
            var ListaRep = new List<string>();
            if (equiposmod.Count > 0 || equiposnew.Count > 0 || equiposbaj.Count > 0 || propiedadesEqui.Count > 0 || listaGrupo.Count > 0 || operacionesCome.Count > 0)
            {
                ListaRep.Add(this.EquiposModHtml(equiposmod));
                ListaRep.Add(this.EquiposNewHtml(equiposnew));
                ListaRep.Add(this.EquiposBajHtml(equiposbaj));
                ListaRep.Add(this.PropiedadesEquiposHtml(propiedadesEqui));
                ListaRep.Add(this.FormulasModosOpeHtml(listaGrupo));
                ListaRep.Add(this.OperacionComercialHtml(operacionesCome));
            }

            return ListaRep;
        }

        /// <summary>
        /// Reporte de equipos
        /// </summary>
        /// <param name="listaEquipo"></param>
        /// <returns></returns>
        private string EquiposModHtml(List<EqEquipoDTO> equiposmod)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='tabla-formulario tabla-adicional' id='tb01'>");

            strHtml.Append(this.CabeceraEquipos());

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var d in equiposmod)
            {
                string sUsuario;
                DateTime dFecha;

                if (d.FechaUpdate.HasValue)
                {
                    sUsuario = d.UsuarioUpdate;
                    dFecha = d.FechaUpdate.Value;

                    strHtml.Append("<tr>");
                    strHtml.Append("<td>" + dFecha.ToString(ConstantesAppServicio.FormatoFecha) + "</td>");
                    strHtml.Append("<td>" + d.EMPRNOMB + "</td>");
                    strHtml.Append("<td>" + d.Equinomb + "</td>");
                    strHtml.Append("<td>" + d.Equiabrev + "</td>");
                    strHtml.Append("<td>" + d.Famnomb + "</td>");
                    strHtml.Append("<td>" + EstadosDesc(d.Equiestado.Trim()) + "</td>");
                    strHtml.Append("<td>" + sUsuario + "</td>");
                    strHtml.Append("<td>" + dFecha.ToString(ConstantesAppServicio.FormatoFechaFull) + "</td>");
                    strHtml.Append("</tr>");
                }
            }
            if (equiposmod.Count == 0) { strHtml.Append("<tr><td colspan='8' style='text-align:center'> Sin informacion </td></tr>"); }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// Reporte de equipos
        /// </summary>
        /// <param name="listaEquipo"></param>
        /// <returns></returns>
        private string EquiposNewHtml(List<EqEquipoDTO> equiposnew)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='tabla-formulario tabla-adicional' id='tb02'>");

            strHtml.Append(this.CabeceraEquipos());

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var d in equiposnew)
            {
                string sUsuario;
                DateTime dFecha;

                if (d.Lastdate.HasValue)
                {
                    sUsuario = d.Lastuser;
                    dFecha = d.Lastdate.Value;

                    strHtml.Append("<tr>");
                    strHtml.Append("<td>" + dFecha.ToString(ConstantesAppServicio.FormatoFecha) + "</td>");
                    strHtml.Append("<td>" + d.EMPRNOMB + "</td>");
                    strHtml.Append("<td>" + d.Equinomb + "</td>");
                    strHtml.Append("<td>" + d.Equiabrev + "</td>");
                    strHtml.Append("<td>" + d.Famnomb + "</td>");
                    strHtml.Append("<td>" + EstadosDesc(d.Equiestado) + "</td>");
                    strHtml.Append("<td>" + sUsuario + "</td>");
                    strHtml.Append("<td>" + dFecha.ToString(ConstantesAppServicio.FormatoFechaFull) + "</td>");
                    strHtml.Append("</tr>");
                }
            }
            if (equiposnew.Count == 0) { strHtml.Append("<tr><td colspan='8' style='text-align:center'> Sin informacion </td></tr>"); }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// Reporte de propiedades de equipo
        /// </summary>
        /// <param name="listaPropiedad"></param>
        /// <returns></returns>
        private string EquiposBajHtml(List<EqPropequiDTO> equiposbaj)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='tabla-formulario tabla-adicional' id='tb03'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>FECHA</th>");
            strHtml.Append("<th>EMPRESA</th>");
            strHtml.Append("<th>EQUIPO</th>");
            strHtml.Append("<th>ABREVIATURA</th>");
            strHtml.Append("<th>FAMILIA</th>");
            strHtml.Append("<th>VIGENCIA A PARTIR DE</th>");
            strHtml.Append("<th>ESTADO</th>");
            strHtml.Append("<th>USUARIO</th>");
            strHtml.Append("<th>FECHA - HORA</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");


            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var d in equiposbaj)
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td>" + d.FechaCambioDesc + "</td>");
                strHtml.Append("<td>" + d.Emprnomb + "</td>");
                strHtml.Append("<td>" + d.Equinomb + "</td>");
                strHtml.Append("<td>" + d.Equiabrev + "</td>");
                strHtml.Append("<td>" + d.Famnomb + "</td>");
                strHtml.Append("<td>" + d.FechapropequiDesc + "</td>");
                strHtml.Append("<td>" + EstadosDesc(d.Equiestado) + "</td>");
                strHtml.Append("<td>" + d.UltimaModificacionUsuarioDesc + "</td>");
                strHtml.Append("<td>" + d.UltimaModificacionFechaDesc + "</td>");
                strHtml.Append("</tr>");
            }
            if (equiposbaj.Count == 0) { strHtml.Append("<tr><td colspan='9' style='text-align:center'> Sin informacion </td></tr>"); }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// Reporte de propiedades de equipo
        /// </summary>
        /// <param name="listaPropiedad"></param>
        /// <returns></returns>
        private string PropiedadesEquiposHtml(List<EqPropequiDTO> listaPropiedad)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='tabla-formulario tabla-adicional' id='tb04'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>FECHA</th>");
            strHtml.Append("<th>EMPRESA</th>");
            strHtml.Append("<th>EQUIPO</th>");
            strHtml.Append("<th>PROPIEDAD</th>");
            strHtml.Append("<th>VALOR</th>");
            strHtml.Append("<th>VIGENTE</th>");
            strHtml.Append("<th>VIGENCIA A PARTIR DE</th>");
            strHtml.Append("<th>ESTADO</th>");
            strHtml.Append("<th>USUARIO</th>");
            strHtml.Append("<th>FECHA - HORA</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var d in listaPropiedad)
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td>" + d.FechaCambioDesc + "</td>");
                strHtml.Append("<td>" + d.Emprnomb + "</td>");
                strHtml.Append("<td>" + d.Equinomb + "</td>");
                strHtml.Append("<td>" + d.Propnomb + "</td>");
                strHtml.Append("<td>" + d.Valor + "</td>");
                strHtml.Append("<td>" + (d.Propequideleted == 0 ? "Si" : "No") + "</td>");
                strHtml.Append("<td>" + d.FechapropequiDesc + "</td>");
                strHtml.Append("<td>" + EstadosDesc(d.Equiestado) + "</td>");
                strHtml.Append("<td>" + d.UltimaModificacionUsuarioDesc + "</td>");
                strHtml.Append("<td>" + d.UltimaModificacionFechaDesc + "</td>");
                strHtml.Append("</tr>");
            }
            if (listaPropiedad.Count == 0) { strHtml.Append("<tr><td colspan='10' style='text-align:center'> Sin informacion </td></tr>"); }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// Reporte de propiedades de equipo
        /// </summary>
        /// <param name="listaPropiedad"></param>
        /// <returns></returns>
        private string FormulasModosOpeHtml(List<PrGrupodatDTO> listaGrupo)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='tabla-formulario tabla-adicional' id='tb05'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>FECHA</th>");
            strHtml.Append("<th>EMPRESA</th>");
            strHtml.Append("<th>GRUPO</th>");
            strHtml.Append("<th>CONCEPTO</th>");
            strHtml.Append("<th>FORMULA</th>");
            strHtml.Append("<th>VIGENTE</th>");
            strHtml.Append("<th>VIGENCIA A PARTIR DE</th>");
            strHtml.Append("<th>ESTADO</th>");
            strHtml.Append("<th>USUARIO</th>");
            strHtml.Append("<th>FECHA - HORA</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var d in listaGrupo)
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td>" + d.Fechaact.Value.ToString(ConstantesAppServicio.FormatoFecha) + "</td>");
                strHtml.Append("<td>" + d.Emprnomb + "</td>");
                strHtml.Append("<td>" + d.GrupoNomb + "</td>");
                strHtml.Append("<td>" + d.ConcepDesc + "</td>");
                strHtml.Append("<td>" + d.Formuladat + "</td>");
                strHtml.Append("<td>" + (d.Deleted == 0 ? "Si" : "No") + "</td>");
                strHtml.Append("<td>" + d.Fechadat.Value.ToString(ConstantesAppServicio.FormatoFecha) + "</td>");
                strHtml.Append("<td>" + (d.Grupoestado != null ? EstadosDesc(d.Grupoestado.Trim()) : "") + "</td>");
                strHtml.Append("<td>" + d.Lastuser + "</td>");
                strHtml.Append("<td>" + d.Fechaact.Value.ToString(ConstantesAppServicio.FormatoFechaFull) + "</td>");
                strHtml.Append("</tr>");
            }
            if (listaGrupo.Count == 0) { strHtml.Append("<tr><td colspan='10' style='text-align:center'> Sin informacion </td></tr>"); }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// Reporte de propiedades de equipo
        /// </summary>
        /// <param name="listaPropiedad"></param>
        /// <returns></returns>
        private string OperacionComercialHtml(List<EqPropequiDTO> listaPropiedad)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='tabla-formulario tabla-adicional' id='tb06'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>FECHA</th>");
            strHtml.Append("<th>EMPRESA</th>");
            strHtml.Append("<th>EQUIPO</th>");
            strHtml.Append("<th>TIPO DE EQUIPO</th>");
            strHtml.Append("<th>CONCEPTO</th>");
            strHtml.Append("<th>VALOR</th>");
            strHtml.Append("<th>VIGENTE</th>");
            strHtml.Append("<th>VIGENCIA A PARTIR DE</th>");
            strHtml.Append("<th>ESTADO EQUIPO</th>");
            strHtml.Append("<th>USUARIO</th>");
            strHtml.Append("<th>FECHA - HORA</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var d in listaPropiedad)
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td>" + d.FechaCambioDesc + "</td>");
                strHtml.Append("<td>" + d.Emprnomb + "</td>");
                strHtml.Append("<td>" + d.Equinomb + "</td>");
                strHtml.Append("<td>" + d.Famnomb + "</td>");
                strHtml.Append("<td>" + d.Propnomb + "</td>");
                strHtml.Append("<td>" + (d.Valor == ConstantesAppServicio.SI ? "Ingreso" : "Retiro") + "</td>");
                strHtml.Append("<td>" + (d.Propequideleted == 0 ? "Sí" : "No") + "</td>");
                strHtml.Append("<td>" + d.FechapropequiDesc + "</td>");
                strHtml.Append("<td>" + EstadosDesc(d.Equiestado) + "</td>");
                strHtml.Append("<td>" + d.UltimaModificacionUsuarioDesc + "</td>");
                strHtml.Append("<td>" + d.UltimaModificacionFechaDesc + "</td>");
                strHtml.Append("</tr>");
            }
            if (listaPropiedad.Count == 0) { strHtml.Append("<tr><td colspan='9' style='text-align:center'> Sin informacion </td></tr>"); }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// Cabecera de Reporte de Equipamientos
        /// </summary>
        /// <returns></returns>
        private string CabeceraEquipos()
        {
            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>FECHA</th>");
            strHtml.Append("<th>EMPRESA</th>");
            strHtml.Append("<th>EQUIPO</th>");
            strHtml.Append("<th>ABREVIATURA</th>");
            strHtml.Append("<th>FAMILIA</th>");
            strHtml.Append("<th>ESTADO</th>");
            strHtml.Append("<th>USUARIO</th>");
            strHtml.Append("<th>FECHA - HORA</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            return strHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="estado"></param>
        /// <returns></returns>
        private string EstadosDesc(string estado)
        {
            string estado_ = string.Empty;

            switch (estado)
            {
                case ConstantesAppServicio.Activo: estado_ = ConstantesAppServicio.ActivoDesc; break;
                case ConstantesAppServicio.Baja: estado_ = ConstantesAppServicio.BajaDesc; break;
                case ConstantesAppServicio.Anulado: estado_ = ConstantesAppServicio.AnuladoDesc; break;
                case ConstantesAppServicio.Proyecto: estado_ = ConstantesAppServicio.ProyectoDesc; break;
            }
            return estado_;
        }

        #endregion

        #region exportacion excel

        /// <summary>
        /// Cabecera Exportacion Excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        private void ExcelCabRtp(ref ExcelWorksheet ws, ExcelPackage xlPackage, string nameWS, string titulo, DateTime fecha1, DateTime fecha2)
        {
            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];
            ws.View.ShowGridLines = false;

            ws.Cells[2, 5].Value = titulo;
            ws.Cells[2, 5].Style.Font.Size = 16;
            ws.Cells[2, 5].Style.Font.Bold = true;
            ws.Cells[2, 5, 2, 9].Merge = true;

            ws.Cells[4, 5].Value = "Del:";
            ws.Cells[4, 5].Style.Font.Bold = true;
            ws.Cells[4, 6].Value = fecha1.ToString(ConstantesAppServicio.FormatoFecha);

            ws.Cells[5, 5].Value = "Del:";
            ws.Cells[5, 5].Style.Font.Bold = true;
            ws.Cells[5, 6].Value = fecha2.ToString(ConstantesAppServicio.FormatoFecha);
        }

        /// <summary>
        /// Configura la imagen que ira en el excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="tipo"></param>
        private void AddImage(ExcelWorksheet ws, int tipo, string url)
        {
            //How to Add a Image using EP Plus
            HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            System.Drawing.Image image = System.Drawing.Image.FromStream(response.GetResponseStream());

            ExcelPicture picture = null;
            if (image != null)
            {
                picture = ws.Drawings.AddPicture("Imagen", image);
                picture.From.Column = 0;
                picture.From.Row = 0;
                picture.From.ColumnOff = Pixel2MTU(12);
                picture.From.RowOff = Pixel2MTU(10);
                picture.SetSize(200, 100);
            }
        }

        /// <summary>
        /// Deterina ancho en pixeles para el logo
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public int Pixel2MTU(int pixels)
        {
            //convert pixel to MTU
            int MTU_PER_PIXEL = 9525;
            int mtus = pixels * MTU_PER_PIXEL;
            return mtus;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        public void borderCeldas(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin)
        {
            var borderTabla = ws.Cells[rowIni, colIni, rowFin, colFin].Style.Border;
            borderTabla.Bottom.Style = borderTabla.Top.Style = borderTabla.Left.Style = borderTabla.Right.Style = ExcelBorderStyle.Hair;
        }

        /// <summary>
        /// formato excel Cabecera
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        /// <param name="sizeFont"></param>
        public void formatoCabecera(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin, int sizeFont)
        {
            using (ExcelRange r1 = ws.Cells[rowIni, colIni, rowFin, colFin])
            {
                r1.Style.Font.Color.SetColor(Color.White);
                r1.Style.Font.Size = sizeFont;
                r1.Style.Font.Bold = true;
                r1.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                r1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                r1.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r1.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                r1.Style.WrapText = true;
            }
        }

        /// <summary>
        /// formato autowidth columnas excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="Ini"></param>
        /// <param name="Fin"></param>
        private void AddAutoWidthColumn(ExcelWorksheet ws, int Ini, int Fin)
        {
            for (int z = Ini; z <= Fin; z++) { ws.Column(z).AutoFit(); }
        }

        /// <summary>
        /// Generar Excel Reporte Parametros
        /// </summary>
        /// <param name="listaEquipo"></param>
        /// <param name="propiedadesEqui"></param>
        /// <param name="listaGrupo"></param>
        /// <param name="operacionesCome"></param>
        /// <param name="ruta"></param>
        public void GenerarExcelReporteParametros(List<EqEquipoDTO> equiposmod, List<EqEquipoDTO> equiposnew, List<EqPropequiDTO> equiposbaj, List<EqPropequiDTO> propiedadesEqui, List<PrGrupodatDTO> listaGrupo, List<EqPropequiDTO> operacionesCome
            , string rutaFile, string url, DateTime fecha1, DateTime fecha2)
        {
            FileInfo newFile = new FileInfo(rutaFile);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;

                this.ExcelCabRtp(ref ws, xlPackage, "Equipos_Modificados", "Cambios en Equipamiento", fecha1, fecha2);
                this.ExcelDetEqMod(ref ws, equiposmod);
                AddImage(ws, 0, url);

                this.ExcelCabRtp(ref ws, xlPackage, "Equipos_Nuevos", "Ingreso Nuevos Equipos", fecha1, fecha2);
                this.ExcelDetEqNew(ref ws, equiposnew);
                AddImage(ws, 0, url);

                this.ExcelCabRtp(ref ws, xlPackage, "Equipos_Baja", "Retiro de Equipos", fecha1, fecha2);
                this.ExcelDetEqBaj(ref ws, equiposbaj);
                AddImage(ws, 0, url);

                this.ExcelCabRtp(ref ws, xlPackage, "Equipos_CambioPropiedades", "Cambios Propiedades de Equipo", fecha1, fecha2);
                this.ExcelDetPropEq(ref ws, propiedadesEqui);
                AddImage(ws, 0, url);

                this.ExcelCabRtp(ref ws, xlPackage, "Formulas_ModosOpe", "Cambio en Formulas", fecha1, fecha2);
                this.ExcelDetFormulasMO(ref ws, listaGrupo);
                AddImage(ws, 0, url);

                this.ExcelCabRtp(ref ws, xlPackage, "Operacion_Comercial", "Operaciones Comerciales", fecha1, fecha2);
                this.ExcelDetOpeCom(ref ws, operacionesCome);
                AddImage(ws, 0, url);

                xlPackage.Save();

                xlPackage.Dispose();
                ws.Dispose();
            }
        }

        /// <summary>
        /// Excel lista Equipo modificados
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="listaEquipo"></param>
        private void ExcelDetEqMod(ref ExcelWorksheet ws, List<EqEquipoDTO> equiposmod)
        {
            int rowIni = 7, columnIni = 3, row = 8, column = 3, size = 10;

            #region cabecera

            string cabeceraArr = "FECHA,EMPRESA,EQUIPO,ABREVIATURA,FAMILIA,ESTADO,USUARIO,FECHA - HORA";
            foreach (var cab in cabeceraArr.Split(',').ToList())
            {
                ws.Cells[rowIni, column++].Value = cab;
            }
            this.formatoCabecera(ws, rowIni, columnIni, rowIni, column - 1, size);

            #endregion

            #region detalle

            foreach (var det in equiposmod)
            {
                column = 3;
                ws.Cells[row, column++].Value = det.FechaUpdate.Value.ToString(ConstantesAppServicio.FormatoFecha);
                ws.Cells[row, column++].Value = det.EMPRNOMB;
                ws.Cells[row, column++].Value = det.Equinomb;
                ws.Cells[row, column++].Value = det.Equiabrev;
                ws.Cells[row, column++].Value = det.Famnomb;
                ws.Cells[row, column++].Value = EstadosDesc(det.Equiestado);
                ws.Cells[row, column++].Value = det.UsuarioUpdate;
                ws.Cells[row, column++].Value = det.FechaUpdate.Value.ToString(ConstantesAppServicio.FormatoFechaHoraAMPM);
                row++;
            }

            using (var range = ws.Cells[rowIni, columnIni, row - 1, column - 1])
            {
                range.Style.Font.Size = size - 2;
            }

            this.AddAutoWidthColumn(ws, columnIni, column - 1);
            this.borderCeldas(ws, rowIni, columnIni, row - 1, column - 1);

            #endregion
        }

        /// <summary>
        /// Excel lista Equipo nuevos
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="listaEquipo"></param>
        private void ExcelDetEqNew(ref ExcelWorksheet ws, List<EqEquipoDTO> equiposnew)
        {
            int rowIni = 7, columnIni = 3, row = 8, column = 3, size = 10;

            #region cabecera

            string cabeceraArr = "FECHA,EMPRESA,EQUIPO,ABREVIATURA,FAMILIA,ESTADO,USUARIO,FECHA - HORA";
            foreach (var cab in cabeceraArr.Split(',').ToList())
            {
                ws.Cells[rowIni, column++].Value = cab;
            }
            this.formatoCabecera(ws, rowIni, columnIni, rowIni, column - 1, size);

            #endregion

            #region detalle

            foreach (var det in equiposnew)
            {
                column = 3;
                ws.Cells[row, column++].Value = det.Lastdate.Value.ToString(ConstantesAppServicio.FormatoFecha);
                ws.Cells[row, column++].Value = det.EMPRNOMB;
                ws.Cells[row, column++].Value = det.Equinomb;
                ws.Cells[row, column++].Value = det.Equiabrev;
                ws.Cells[row, column++].Value = det.Famnomb;
                ws.Cells[row, column++].Value = EstadosDesc(det.Equiestado);
                ws.Cells[row, column++].Value = det.Lastuser;
                ws.Cells[row, column++].Value = det.Lastdate.Value.ToString(ConstantesAppServicio.FormatoFechaHoraAMPM);
                row++;
            }

            using (var range = ws.Cells[rowIni, columnIni, row - 1, column - 1])
            {
                range.Style.Font.Size = size - 2;
            }

            this.AddAutoWidthColumn(ws, columnIni, column - 1);
            this.borderCeldas(ws, rowIni, columnIni, row - 1, column - 1);

            #endregion
        }

        /// <summary>
        /// Excel lista Equipo de baja
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="listaEquipo"></param>
        private void ExcelDetEqBaj(ref ExcelWorksheet ws, List<EqPropequiDTO> equiposbaj)
        {
            int rowIni = 7, columnIni = 3, row = 8, column = 3, size = 10;

            #region cabecera

            string cabeceraArr = "FECHA,EMPRESA,EQUIPO,ABREVIATURA,VIGENCIA A PARTIR DE,FAMILIA,ESTADO,USUARIO,FECHA - HORA";
            foreach (var cab in cabeceraArr.Split(',').ToList())
            {
                ws.Cells[rowIni, column++].Value = cab;
            }
            this.formatoCabecera(ws, rowIni, columnIni, rowIni, column - 1, size);

            #endregion

            #region detalle

            foreach (var det in equiposbaj)
            {
                column = 3;
                ws.Cells[row, column++].Value = det.FechaCambioDesc;
                ws.Cells[row, column++].Value = det.Emprnomb;
                ws.Cells[row, column++].Value = det.Equinomb;
                ws.Cells[row, column++].Value = det.Equiabrev;
                ws.Cells[row, column++].Value = det.Famnomb;
                ws.Cells[row, column++].Value = det.FechapropequiDesc;
                ws.Cells[row, column++].Value = EstadosDesc(det.Equiestado);
                ws.Cells[row, column++].Value = det.UltimaModificacionUsuarioDesc;
                ws.Cells[row, column++].Value = det.UltimaModificacionFechaDesc;
                row++;
            }

            using (var range = ws.Cells[rowIni, columnIni, row - 1, column - 1])
            {
                range.Style.Font.Size = size - 2;
            }

            this.AddAutoWidthColumn(ws, columnIni, column - 1);
            this.borderCeldas(ws, rowIni, columnIni, row - 1, column - 1);

            #endregion
        }

        /// <summary>
        /// Excel lista Propiedades de Equipo 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="listaEquipo"></param>
        private void ExcelDetPropEq(ref ExcelWorksheet ws, List<EqPropequiDTO> propiedadesEqui)
        {
            int rowIni = 7, columnIni = 3, row = 8, column = 3, size = 10;

            #region cabecera

            string cabeceraArr = "FECHA,EMPRESA,EQUIPO,PROPIEDAD,VALOR,VIGENTE,VIGENCIA A PARTIR DE,ESTADO,USUARIO,FECHA - HORA";
            foreach (var cab in cabeceraArr.Split(',').ToList())
            {
                ws.Cells[rowIni, column++].Value = cab;
            }
            this.formatoCabecera(ws, rowIni, columnIni, rowIni, column - 1, size);

            #endregion

            #region detalle

            foreach (var det in propiedadesEqui)
            {
                column = 3;
                ws.Cells[row, column++].Value = det.FechaCambioDesc;
                ws.Cells[row, column++].Value = det.Emprnomb;
                ws.Cells[row, column++].Value = det.Equinomb;
                ws.Cells[row, column++].Value = det.Propnomb;
                ws.Cells[row, column++].Value = det.Valor;
                ws.Cells[row, column++].Value = (det.Propequideleted == 0 ? "Si" : "No");
                ws.Cells[row, column++].Value = det.FechapropequiDesc;
                ws.Cells[row, column++].Value = EstadosDesc(det.Equiestado);
                ws.Cells[row, column++].Value = det.UltimaModificacionUsuarioDesc;
                ws.Cells[row, column++].Value = det.UltimaModificacionFechaDesc;
                row++;
            }

            using (var range = ws.Cells[rowIni, columnIni, row - 1, column - 1])
            {
                range.Style.Font.Size = size - 2;
            }

            this.AddAutoWidthColumn(ws, columnIni, column - 1);
            this.borderCeldas(ws, rowIni, columnIni, row - 1, column - 1);

            #endregion
        }

        /// <summary>
        /// Excel lista Equipo de baja
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="listaEquipo"></param>
        private void ExcelDetFormulasMO(ref ExcelWorksheet ws, List<PrGrupodatDTO> listaGrupo)
        {
            int rowIni = 7, columnIni = 3, row = 8, column = 3, size = 10;

            #region cabecera

            string cabeceraArr = "FECHA,EMPRESA,GRUPO,CONCEPTO,FORMULA,VIGENTE,VIGENCIA A PARTIR DE,ESTADO,USUARIO,FECHA VIGENCIA";
            foreach (var cab in cabeceraArr.Split(',').ToList())
            {
                ws.Cells[rowIni, column++].Value = cab;
            }
            this.formatoCabecera(ws, rowIni, columnIni, rowIni, column - 1, size);

            #endregion

            #region detalle

            foreach (var det in listaGrupo)
            {
                column = 3;
                ws.Cells[row, column++].Value = det.Fechaact.Value.ToString(ConstantesAppServicio.FormatoFechaHoraAMPM);
                ws.Cells[row, column++].Value = det.Emprnomb;
                ws.Cells[row, column++].Value = det.GrupoNomb;
                ws.Cells[row, column++].Value = det.ConcepDesc;
                ws.Cells[row, column++].Value = det.Formuladat;
                ws.Cells[row, column++].Value = (det.Deleted == 0 ? "Si" : "No");
                ws.Cells[row, column++].Value = det.Fechadat.Value.ToString(ConstantesAppServicio.FormatoFecha);
                ws.Cells[row, column++].Value = (det.Grupoestado != null ? EstadosDesc(det.Grupoestado.Trim()) : "");
                ws.Cells[row, column++].Value = det.Lastuser;
                ws.Cells[row, column++].Value = det.Fechadat.Value.ToString(ConstantesAppServicio.FormatoFecha);
                row++;
            }

            using (var range = ws.Cells[rowIni, columnIni, row - 1, column - 1])
            {
                range.Style.Font.Size = size - 2;
            }

            this.AddAutoWidthColumn(ws, columnIni, column - 1);
            this.borderCeldas(ws, rowIni, columnIni, row - 1, column - 1);

            #endregion
        }

        /// <summary>
        /// Excel lista Propiedades de Equipo 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="listaEquipo"></param>
        private void ExcelDetOpeCom(ref ExcelWorksheet ws, List<EqPropequiDTO> operacionesCome)
        {
            int rowIni = 7, columnIni = 3, row = 8, column = 3, size = 10;

            #region cabecera

            string cabeceraArr = "FECHA,EMPRESA,EQUIPO,TIPO DE EQUIPO,CONCEPTO,VALOR,VIGENTE,VIGENCIA A PARTIR DE,ESTADO EQUIPO,USUARIO,FECHA - HORA";
            foreach (var cab in cabeceraArr.Split(',').ToList())
            {
                ws.Cells[rowIni, column++].Value = cab;
            }
            this.formatoCabecera(ws, rowIni, columnIni, rowIni, column - 1, size);

            #endregion

            #region detalle

            foreach (var det in operacionesCome)
            {
                column = 3;
                ws.Cells[row, column++].Value = det.FechaCambioDesc;
                ws.Cells[row, column++].Value = det.Emprnomb;
                ws.Cells[row, column++].Value = det.Equinomb;
                ws.Cells[row, column++].Value = det.Famnomb;
                ws.Cells[row, column++].Value = det.Propnomb;
                ws.Cells[row, column++].Value = det.Valor;
                ws.Cells[row, column++].Value = (det.Valor == ConstantesAppServicio.SI ? "Ingreso" : "Retiro");
                ws.Cells[row, column++].Value = det.FechapropequiDesc;
                ws.Cells[row, column++].Value = EstadosDesc(det.Equiestado);
                ws.Cells[row, column++].Value = det.UltimaModificacionUsuarioDesc;
                ws.Cells[row, column++].Value = det.UltimaModificacionFechaDesc;
                row++;
            }

            using (var range = ws.Cells[rowIni, columnIni, row - 1, column - 1])
            {
                range.Style.Font.Size = size - 2;
            }

            this.AddAutoWidthColumn(ws, columnIni, column - 1);
            this.borderCeldas(ws, rowIni, columnIni, row - 1, column - 1);

            #endregion
        }

        #endregion
    }
}