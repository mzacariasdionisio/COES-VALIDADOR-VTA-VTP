using COES.MVC.Intranet.Areas.Eventos.Models;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Correo;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.EnviarCorreos;
using COES.Servicios.Aplicacion.Eventos.Helper;
using System.Globalization;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.PruebasAleatorias;
using COES.Servicios.Aplicacion.Interconexiones;
using COES.Servicios.Aplicacion.OperacionesVarias;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Informefalla;
using COES.Servicios.Aplicacion.InformefallaN2;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Areas.Eventos.Helper;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.General;
using System.IO;
using COES.Servicios.Aplicacion.Pruebaunidad;
using COES.Servicios.Aplicacion.CortoPlazo;
using log4net;
using COES.MVC.Intranet.Areas.Evento.Helper;
using System.Configuration;

namespace COES.MVC.Intranet.Areas.Eventos.Controllers
{
    public class EnviarCorreosController : BaseController
    {
        /// <summary>
        /// Fecha de Inicio de consulta
        /// </summary>
        public DateTime? FechaInicio
        {
            get
            {
                return (Session[DatosSesion.FechaConsultaInicio] != null) ?
                    (DateTime?)(Session[DatosSesion.FechaConsultaInicio]) : null;
            }
            set
            {
                Session[DatosSesion.FechaConsultaInicio] = value;
            }
        }

        /// <summary>
        /// Fecha de Fin de Consulta
        /// </summary>
        public DateTime? FechaFinal
        {
            get
            {
                return (Session[DatosSesion.FechaConsultaFin] != null) ?
                  (DateTime?)(Session[DatosSesion.FechaConsultaFin]) : null;
            }
            set
            {
                Session[DatosSesion.FechaConsultaFin] = value;
            }
        }

        CorreoAppServicio servCorreo = new CorreoAppServicio();
        EnviarCorreosAppServicio servEnvioCorreo = new EnviarCorreosAppServicio();
        OperacionesVariasAppServicio servOperacionesVarias = new OperacionesVariasAppServicio();
        EquipamientoAppServicio servEquipamiento = new EquipamientoAppServicio();
        PruebasAleatoriasAppServicio servPruebasaleatorias = new PruebasAleatoriasAppServicio();
        InformefallaAppServicio servInformeFallas = new InformefallaAppServicio();
        InformefallaN2AppServicio servInformeFallasN2 = new InformefallaN2AppServicio();
        PersonaAppServicio servPersona = new PersonaAppServicio();
        CortoPlazoAppServicio servCP = new CortoPlazoAppServicio();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(EnviarCorreosController));

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("EnviarCorreosController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("EnviarCorreosController", ex);
                throw;
            }
        }

        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            OperacionesVariasAppServicio servicio = new OperacionesVariasAppServicio();
            BusquedaEnviarCorreoModel model = new BusquedaEnviarCorreoModel();
            model.ListaEvensubcausa = servicio.ListarSubcausaevento(ConstantesEnviarCorreos.EvenSubcausa);
            model.Fechaini = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.Fechafin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.Fechaini = (this.FechaInicio != null) ? ((DateTime)this.FechaInicio).ToString(Constantes.FormatoFecha) :
               DateTime.Now.ToString(Constantes.FormatoFecha);
            model.Fechafin = (this.FechaFinal != null) ? ((DateTime)this.FechaFinal).ToString(Constantes.FormatoFecha) :
                DateTime.Now.ToString(Constantes.FormatoFecha);


            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);

            return View(model);

        }

        /// <summary>
        /// Permite editar registro con parámetro
        /// </summary>
        /// <param name="id">id: identificador</param>
        /// <param name="accion">0: ver, 1: editar</param>
        /// <returns></returns>
        public ActionResult Editar(int id, int accion, int top)
        {
            EnviarCorreoModel model = new EnviarCorreoModel();
            model.ListaEvensubcausa =
                servOperacionesVarias.ListarSubcausaevento(ConstantesEnviarCorreos.EvenSubcausa);
            model.ListaProgramador = this.servPruebasaleatorias.ListarProgramador();
            model.ListaCoordinadores = servOperacionesVarias.ListaCoordinadores();
            model.ListaEspecialistaSME = servPersona.ListaEspecialistasSME();

            EveMailsDTO eveMails = null;

            if (id != 0)
                eveMails = this.servEnvioCorreo.GetByIdEveMails(id);

            if (eveMails != null)
            {
                model.EveMail = eveMails;
                if (eveMails.Equicodi != null)
                {
                    EqEquipoDTO equipo = servEquipamiento.ObtenerDetalleEquipo(Convert.ToInt32(eveMails.Equicodi));
                    model.Equinomb = " " + equipo.EMPRNOMB + " : " + equipo.TAREAABREV + " " + equipo.AREANOMB + " - " +
                                     equipo.Equiabrev;
                }
            }
            else
            {
                eveMails = new EveMailsDTO();
                eveMails.Subcausacodi = (top == 0 ? ConstantesEnviarCorreos.EvenSubcausaNuevoRegistro : top);
                eveMails.Mailfecha = DateTime.ParseExact(DateTime.Now.ToString(Constantes.FormatoFecha), Constantes.FormatoFecha,
                        CultureInfo.InvariantCulture);

                model.EveMail = eveMails;
            }
            model.ListaReprogramas = servCP.ListarReprogramas(eveMails.Mailfecha, 1, 1);
            model.Accion = accion;
            return View(model);
        }

        //[HttpPost]
        public JsonResult CargarReprogramas(string fecha)
        {
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            List<CpReprogramaDTO> listReprogramas = new List<CpReprogramaDTO>();
            listReprogramas = servCP.ListarReprogramas(fechaConsulta, 1, 1);
            var list = new SelectList(listReprogramas, "Reprogcodi", "Topnombre");
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Permite eliminar la operacion
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            try
            {
                (new EnviarCorreosAppServicio()).DeleteEveMails(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite obtener información del equipo
        /// </summary>
        /// <param name="idEquipo">Código del equipo</param>
        /// <returns>Información del equipo</returns>
        [HttpPost]
        public JsonResult SeleccEquipo(int idEquipo)
        {
            EqEquipoDTO equipo = servEquipamiento.ObtenerDetalleEquipo(idEquipo);
            return
                Json(" " + equipo.EMPRNOMB + " : " + equipo.TAREAABREV + " " + equipo.AREANOMB + " - " +
                     equipo.Equiabrev);
        }

        /// <summary>
        /// Permite grabar un registro de envío de correo
        /// </summary>
        /// <param name="model">Modelo</param>
        /// <returns>Código y Estado de envío. -1: Si hay error</returns>
        [HttpPost]
        public JsonResult Grabar(EnviarCorreoModel model)
        {
            try
            {

                EveMailsDTO entity = new EveMailsDTO();

                entity.Mailcodi = model.Mailcodi;
                entity.Subcausacodi = model.Subcausacodi;

                if (model.MailTurnoNum != null)
                {
                    entity.Mailturnonum = model.MailTurnoNum;
                }

                if (model.MailReprogCausa != null)
                {
                    entity.Mailreprogcausa = model.MailReprogCausa;
                }

                //MAILCHECK1
                if (model.MailHoja != null)
                {
                    entity.Mailhoja = model.MailHoja;
                }

                entity.Mailprogramador = model.MailProgramador;

                if (model.MailBloqueHorario != null && model.MailBloqueHorario != 0)
                {
                    entity.Mailbloquehorario = model.MailBloqueHorario;
                }

                bool esFechaDiario = model.MailFecha.Contains('/');
                if (!esFechaDiario)
                {
                    string[] subs = model.MailFecha.Split(' ');
                    model.MailFecha = "01/" + subs[0] + "/" + subs[1];
                }

                entity.Mailfecha = DateTime.ParseExact(model.MailFecha, Constantes.FormatoFecha,
                    CultureInfo.InvariantCulture);

                //MAILCHECK2
                entity.Mailemitido = model.MailEmitido;
                if (model.Mailcodi == 0 || entity.Mailemitido == null)
                {
                    entity.Mailemitido = "N";
                }

                //MAILCHECK1
                entity.Mailcheck1 = model.MailCheck1;
                if (model.Equicodi != null && model.Equicodi != 0)
                    entity.Equicodi = model.Equicodi;
                if (model.Subcausacodi == ConstantesEnviarCorreo.SubcausacodiCMgHOparaIEDO || model.Subcausacodi == ConstantesEnviarCorreo.SubcausacodiUpdateCMgHOparaIEDO ||
                    model.Subcausacodi == ConstantesEnviarCorreo.SubcausacodiReporteCMg || model.Subcausacodi == ConstantesEnviarCorreo.SubcausacodiReporteHO ||
                    model.Subcausacodi == ConstantesEnviarCorreo.SubcausacodiReportePremCMg || model.Subcausacodi == ConstantesEnviarCorreo.SubcausacodiReportePremHO ||
                    model.Subcausacodi == ConstantesEnviarCorreo.SubcausacodiReporteFinCMg || model.Subcausacodi == ConstantesEnviarCorreo.SubcausacodiReporteFinHO)
                {

                    if (model.EspecialistaSME != null && model.EspecialistaSME != "0")
                    {
                        entity.Mailespecialista = model.EspecialistaSME;
                    }
                }
                else
                {
                    if (model.MailEspecialista != null && model.MailEspecialista != "0")
                    {
                        entity.Mailespecialista = model.MailEspecialista;
                    }
                }

                if (model.MailTipoPrograma != null)
                {
                    entity.Mailtipoprograma = model.MailTipoPrograma;
                }

                //- Agregado
                if (!string.IsNullOrEmpty(model.Mailhora))
                {
                    string fechaHora = model.MailFecha + " " + model.Mailhora;
                    try
                    {
                        DateTime fec = DateTime.ParseExact(fechaHora, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                        entity.Mailhora = fec;
                    }
                    catch
                    {

                    }
                }

                entity.Mailconsecuencia = model.Mailconsecuencia;
                entity.CoordinadorTurno = model.CoordinadorTurno;

                //-reprogramado
                entity.Topcodi = model.Topcodi;

                //- fin agregado

                entity.Lastuser = base.UserName;
                entity.Lastdate = DateTime.Now;

                //guardar detalle
                int id = servEnvioCorreo.Save(entity);
                return Json(id + "," + entity.Mailemitido);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite actualizar estado de envío de correo a enviado
        /// </summary>
        /// <param name="mailcodi">Código de correo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EstadoCorreoEnviado(int mailcodi)
        {
            try
            {
                EveMailsDTO eveMails = null;
                if (mailcodi != 0)
                {
                    eveMails = servEnvioCorreo.GetByIdEveMails(mailcodi);
                    eveMails.Mailemitido = "S";
                    eveMails.Lastuser = base.UserName;
                    eveMails.Lastdate = DateTime.Now;
                    //guardar detalle
                    int id = servEnvioCorreo.Save(eveMails);
                    return Json(id);
                }
                else
                {
                    return Json(-1);
                }


            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite actualizar el estado de envío del Informe de Fallas N1
        /// </summary>
        /// <param name="id">código de Informe de Falla</param>
        /// <param name="tipo">0: IP, 1: IPI, 2: IF</param>
        /// <returns>Código de Informe. -1: si hay error</returns>
        [HttpPost]
        public JsonResult EstadoCorreoEnviadoInformeFalla(int id, int tipo)
        {
            try
            {
                EveInformefallaDTO eveInfFalla = null;
                if (id != 0)
                {
                    eveInfFalla = this.servInformeFallas.GetByIdEveInformefalla(id);
                    switch (tipo)
                    {
                        case 0: //Informe Preliminar Inicial
                            eveInfFalla.Eveninfpiemitido = "S";
                            eveInfFalla.Eveninfpifechemis = DateTime.Now;
                            break;
                        case 1: //Informe Inicial
                            eveInfFalla.Eveninfpemitido = "S";
                            eveInfFalla.Eveninfpfechemis = DateTime.Now;
                            break;
                        case 2: //Informe Final
                            eveInfFalla.Eveninfemitido = "S";
                            eveInfFalla.Eveninffechemis = DateTime.Now;
                            break;
                    }

                    eveInfFalla.Eveninflastuser = base.UserName;
                    eveInfFalla.Eveninflastdate = DateTime.Now;

                    //guardar detalle
                    int idIFalla = this.servInformeFallas.SaveEveInformefallaId(eveInfFalla);

                    return Json(idIFalla);
                }
                else
                {
                    return Json(-1);
                }

            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite actualizar el estado de envío del Informe de Fallas N2
        /// </summary>
        /// <param name="id">Código de Informe de Falla</param>
        /// <param name="tipo">0: IPI sin informe de empresa, 1: IPI informe, 2: IF sin informe de empresa, 3: IF informe</param>
        /// <returns>Código de informe. -1 si hay error</returns>
        [HttpPost]
        public JsonResult EstadoCorreoEnviadoInformeFallaN2(int id, int tipo)
        {
            try
            {
                EveInformefallaN2DTO eveInfFalla = null;

                if (id != 0)
                {
                    eveInfFalla = this.servInformeFallasN2.GetByIdEveInformefallaN2(id);

                    switch (tipo)
                    {

                        case 0: //Informe Preliminar Inicial sin informe de empresa

                            eveInfFalla.EvenipiEN2emitido = "S";
                            eveInfFalla.EvenipiEN2fechem = DateTime.Now;
                            break;
                        case 1: //Informe Preliminar inicial. Informe COES
                            eveInfFalla.Eveninfpin2emitido = "S";
                            eveInfFalla.Eveninfpin2fechemis = DateTime.Now;
                            break;
                        case 2: //Informe Final sin informe de empresa
                            eveInfFalla.EvenifEN2emitido = "S";
                            eveInfFalla.EvenifEN2fechem = DateTime.Now;
                            break;
                        case 3: //Informe Final. Informe COES
                            eveInfFalla.Eveninffn2emitido = "S";
                            eveInfFalla.Eveninffn2fechemis = DateTime.Now;
                            break;

                    }

                    eveInfFalla.Eveninfn2lastuser = base.UserName;
                    eveInfFalla.Eveninfn2lastdate = DateTime.Now;

                    //guardar detalle
                    int idIFalla = this.servInformeFallasN2.SaveEveInformefallaN2Id(eveInfFalla);

                    return Json(idIFalla);
                }
                else
                {
                    return Json(-1);
                }


            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Listado de Envío de Correo según SubCausa, Fecha inicial, Fecha final y número de página
        /// </summary>        
        /// <param name="subCausacodi">Subclase</param>
        /// <param name="fechaIni">fecha inicial</param>
        /// <param name="fechaFin">fecha final</param>
        /// <param name="nroPage">Nro. de página</param>
        /// <returns>Listado</returns>
        [HttpPost]
        public PartialViewResult Lista(int subCausacodi, string fechaIni, string fechaFin, int nroPage)
        {
            BusquedaEnviarCorreoModel model = new BusquedaEnviarCorreoModel();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (fechaIni != null)
            {
                fechaInicio = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (fechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            this.FechaInicio = fechaInicio;
            this.FechaFinal = fechaFinal;

            model.ListaEveMail =
                servEnvioCorreo.BuscarOperaciones(subCausacodi, fechaInicio, fechaFinal, nroPage,
                    Constantes.PageSizeEvento).ToList();

            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            return PartialView(model);
        }
        /// <summary>
        /// Permite exportar el reporte generado
        /// </summary>
        /// <returns>Archivo descargado</returns>
        [HttpGet]
        public virtual ActionResult DescargarCorreo()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeEvento + NombreArchivo.ReporteEnvíoCorreo;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteEnvíoCorreo);
        }


        /// <summary>
        /// Listado de Envío de Correo según SubCausa, Fecha inicial, Fecha final y número de página
        /// </summary>        
        /// <param name="subCausacodi">Subclase</param>
        /// <param name="fechaIni">fecha inicial</param>
        /// <param name="fechaFin">fecha final</param>
        /// <returns>Listado</returns>
        [HttpPost]
        public JsonResult ExportarCorreo(int subCausacodi, string fechaIni, string fechaFin)
        {
            int result = 1;
            try
            {
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFinal = DateTime.Now;

                if (fechaIni != null)
                {
                    fechaInicio = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (fechaFin != null)
                {
                    fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                List<EveMailsDTO> lista = servEnvioCorreo.ExportarEnvioCorreos(subCausacodi, fechaInicio, fechaFinal).ToList();
                ExcelDocument.GenerarReporteEnvioCorreo(lista, fechaInicio, fechaFinal, subCausacodi);

                result = 1;
            }
            catch (Exception ex)
            {
                result = -1;
            }

            return Json(result);
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="subCausacodi">Código de subcausa</param>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Paginado</returns>
        [HttpPost]
        public PartialViewResult Paginado(int subCausacodi, string fechaIni, string fechaFin)
        {

            Paginacion model = new Paginacion();
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (fechaIni != null)
            {
                fechaInicio = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (fechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            fechaFinal = fechaFinal.AddDays(1);

            int nroRegistros = servEnvioCorreo.ObtenerNroFilas(subCausacodi, fechaInicio, fechaFinal,
                Constantes.NroPageShow, Constantes.PageSizeEvento);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeEvento;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return base.Paginado(model);
        }

        public void AsociarCamposCorreo(ref FormatoCorreoModel formato, string from, string to, string cc, string bcc, string asunto, string contenido, int plantcodi)
        {
            formato.From = from;
            formato.To = to;
            formato.CC = cc;
            formato.BCC = bcc;

            formato.Asunto = asunto;
            formato.Contenido = contenido;
            formato.Plantcodi = plantcodi;
        }

        /// <summary>
        /// Permite mostrar el formato de correo
        /// </summary>
        /// <param name="collection">Parámetros</param>
        /// <returns>Formato de correo</returns>
        [HttpPost]
        public PartialViewResult FormatoCorreo(FormCollection collection)
        {
            string tipoplantilla = ((collection["plantilla"] != null) ? collection["plantilla"] : "0");
            FormatoCorreoModel model = new FormatoCorreoModel();
            string from = String.Empty, to = String.Empty, cc = String.Empty, bcc = String.Empty;
            string asunto = String.Empty;
            string contenido = String.Empty;

            if (tipoplantilla != "0")
            {
                PersonaAppServicio persona = new PersonaAppServicio();
                PruebaunidadAppServicio servPruebaunidad = new PruebaunidadAppServicio();

                var plantilla = new SiPlantillacorreoDTO();
                string[] parametroContenido = new string[15];
                string[] parametroTitulo = new string[10];
                int plantcodi = 0;

                string url = ConfigurationManager.AppSettings["UrlPortal"].ToString();

                switch (tipoplantilla)
                {
                    #region Pruebas aleatorias - Verificacion de disponibilidades

                    case "pruebaaleatoria": //Verificacion de disponibilidades //5

                        PruebasAleatoriasAppServicio paleatoria = new PruebasAleatoriasAppServicio();
                        plantcodi = ConstantesEnviarCorreo.PlantcodiPruebasAleatorias;
                        plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                        ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);



                        DateTime fecha = DateTime.ParseExact(collection["id"], Constantes.FormatoFechaAnioMesDia,
                            CultureInfo.InvariantCulture);

                        var pruebaaleatoria = paleatoria.GetByIdEvePaleatoria(fecha);
                        string programador = pruebaaleatoria.Programador;

                        parametroContenido[0] = DateTime.Now.Day + " de " + Tools.ObtenerNombreMes(DateTime.Now.Month) +
                                                " de " + DateTime.Now.Year;

                        if ((collection["id"] == ""))
                            return PartialView(model);

                        parametroContenido[1] = Tools.ObtenerNombreDia(fecha.DayOfWeek).ToLower() + " " + fecha.Day +
                                                " de " + Tools.ObtenerNombreMes(fecha.Month) + " de " + fecha.Year;

                        SorteoAppServicio servSorteo = new SorteoAppServicio();

                        int balota = servSorteo.ConteoCorreoTipo("XEQ", fecha);

                        parametroContenido[2] = "";

                        if (balota == 0)
                        {
                            parametroContenido[2] += "Balota blanca: \"Día de no prueba\"" + "<br>" + "<br>";
                        }
                        else
                        {
                            parametroContenido[2] += "Balota negra: \"Día de prueba\"";

                            int balotaNegra = servSorteo.ConteoBalotaNegra(fecha);

                            if (balotaNegra == 0)
                            {
                                parametroContenido[2] += ", pero no existen unidades disponibles para esta prueba." +
                                                         "<br>" + "<br>";
                            }
                            else
                            {
                                string equipoPrueba = servSorteo.EquipoPrueba(fecha);
                                parametroContenido[2] += "<br>" + "<br>" + "Unidad sorteada: " + equipoPrueba + "<br>";
                                int equicodi = servSorteo.EquicodiPrueba(fecha);
                                InterconexionesAppServicio servInterconexiones = new InterconexionesAppServicio();

                                string unidad = "MW"; //ls_propcodi = "188";
                                string mensajeError = "***Revisar Potencia efectiva (registro Prueba unidad Sorteada)***";

                                //string pefectiva = servInterconexiones.GetValorPropiedad(188, equicodi);
                                string pefectiva = "";
                                try
                                {
                                    EvePruebaunidadDTO registro = servPruebaunidad.BuscarOperaciones("N", fecha, fecha, -1, -1).FirstOrDefault();

                                    if (registro.Prundpotefectiva != 0)
                                    {
                                        pefectiva = registro.Prundpotefectiva.ToString();
                                    }
                                    else
                                    {
                                        pefectiva = mensajeError;
                                    }
                                }
                                catch
                                {
                                    pefectiva = mensajeError;
                                }

                                parametroContenido[2] += "Potencia efectiva: " + pefectiva + " " + unidad + "<br>";

                                unidad = "Hr."; // ls_propcodi = "205";
                                string tiempoMinimoOperacion = servInterconexiones.GetValorPropiedad(205, equicodi);
                                parametroContenido[2] += "Tiempo mínimo de operación: " + tiempoMinimoOperacion + " " +
                                                         unidad + "<br>";

                                unidad = "Hr."; //ls_propcodi = "204";
                                string tiempoMinimoArranqueSucesivo = servInterconexiones.GetValorPropiedad(204,
                                    equicodi);
                                parametroContenido[2] += "Tiempo mínimo entre arranques sucesivos: " +
                                                         tiempoMinimoArranqueSucesivo + " " + unidad + "<br>" + "<br>";

                            }

                        }

                        parametroContenido[3] = "";
                        string area = "SCO";

                        programador = paleatoria.ProgramadorPrueba(fecha);
                        string cargo = persona.GetCargo(programador);
                        string telefono = persona.GetTelefono(programador);
                        parametroContenido[3] += programador + "<br>" + cargo + "<br>" + area + "<br>Tel. " + telefono +
                                                 "<br>";
                        asunto = plantilla.Plantasunto;
                        contenido = plantilla.Plantcontenido;
                        contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                            parametroContenido[2], parametroContenido[3]);

                        AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);

                        break;

                    #endregion


                    #region Enviar Correos


                    case "enviarcorreo": //Envío de correos
                        int mailCodi = Convert.ToInt32(collection["id"]);
                        EveMailsDTO entity = servEnvioCorreo.GetByIdEveMails(mailCodi);
                        plantcodi = 0;


                        switch ((entity.Subcausacodi))
                        {
                            #region Programa Diario

                            case ConstantesEnviarCorreo.SubcausacodiProgramaDiario: //Programa Diario

                                //costo variable
                                if (entity.Mailcheck1 == null)
                                    entity.Mailcheck1 = "N";

                                //hoja
                                if (entity.Mailhoja == null)
                                    entity.Mailhoja = "A";

                                //turno num
                                if (entity.Mailturnonum == null)
                                    entity.Mailturnonum = 2;

                                if (entity.Mailcheck1 == "S")
                                {
                                    if (entity.Mailturnonum == 2)
                                    {

                                        plantcodi = ConstantesEnviarCorreo.PlantcodiPdm;
                                        plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                        ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                        parametroTitulo[0] = entity.Mailfecha.ToString("dd.MM.yyyy");
                                        parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                                Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                                DateTime.Now.Year.ToString();
                                        parametroContenido[1] = entity.Mailfecha.DayOfYear + "-" +
                                                                entity.Mailfecha.Year.ToString() + " del día " +
                                                                Tools.ObtenerNombreDia(entity.Mailfecha.DayOfWeek)
                                                                    .ToLower() + " " + entity.Mailfecha.Day.ToString() +
                                                                " de " + Tools.ObtenerNombreMes(entity.Mailfecha.Month) +
                                                                " de " + entity.Mailfecha.Year.ToString();
                                        parametroContenido[2] = Tools.ObtenerNroSemanaAnio(entity.Mailfecha).ToString();
                                        parametroContenido[3] = Uri.EscapeUriString(url + "Operacion/ProgManten/ProgDiario?path=Operación/Programa de Mantenimiento/Programa Diario/" + entity.Mailfecha.Year + "/" + entity.Mailfecha.Month.ToString().PadLeft(2, '0') + "_" + Tools.ObtenerNombreMes(entity.Mailfecha.Month).ToUpper() + "/Día " + entity.Mailfecha.Day.ToString().PadLeft(2, '0') + "/");
                                        parametroContenido[4] = "PDI_" + entity.Mailfecha.ToString("ddMM_yyyy");
                                        parametroContenido[5] = entity.Mailprogramador;
                                        parametroContenido[6] = "SPR-COES";
                                        parametroContenido[7] = persona.GetTelefono(entity.Mailprogramador);



                                        asunto = plantilla.Plantasunto;
                                        asunto = String.Format(asunto, parametroTitulo[0]);

                                        contenido = plantilla.Plantcontenido;
                                        contenido = String.Format(contenido, parametroContenido[0],
                                            parametroContenido[1], parametroContenido[2], parametroContenido[3],
                                            parametroContenido[4], parametroContenido[5], parametroContenido[6], parametroContenido[7]);

                                        AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);

                                    }
                                    else
                                    {
                                        if (entity.Mailturnonum == 3)
                                        {
                                            plantcodi = ConstantesEnviarCorreo.PlantcodiPdo;
                                            plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                            ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                            parametroTitulo[0] = entity.Mailfecha.ToString("dd.MM.yyyy");
                                            parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                                    Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                                    DateTime.Now.Year.ToString();
                                            parametroContenido[1] = entity.Mailfecha.DayOfYear + "-" +
                                                                    entity.Mailfecha.Year.ToString() + " del día " +
                                                                    Tools.ObtenerNombreDia(entity.Mailfecha.DayOfWeek)
                                                                        .ToLower() + " " +
                                                                    entity.Mailfecha.Day.ToString() + " de " +
                                                                    Tools.ObtenerNombreMes(entity.Mailfecha.Month) +
                                                                    " de " + entity.Mailfecha.Year.ToString();
                                            parametroContenido[2] =
                                                Tools.ObtenerNroSemanaAnio(entity.Mailfecha).ToString();
                                            parametroContenido[3] = Uri.EscapeUriString(url + "Operacion/ProgOperacion/ProgramaDiario?path=Operación/Programa de Operación/Programa Diario/" + entity.Mailfecha.Year + "/" + entity.Mailfecha.Month.ToString().PadLeft(2, '0') + "_" + Tools.ObtenerNombreMes(entity.Mailfecha.Month).ToUpper() + "/Día " + entity.Mailfecha.Day.ToString().PadLeft(2, '0') + "/");
                                            parametroContenido[4] = "PDO_" + entity.Mailfecha.ToString("ddMM_yyyy");
                                            parametroContenido[5] = entity.Mailprogramador;
                                            parametroContenido[6] = "SPR-COES";
                                            parametroContenido[7] = persona.GetTelefono(entity.Mailprogramador);



                                            asunto = plantilla.Plantasunto;
                                            asunto = String.Format(asunto, parametroTitulo[0]);

                                            contenido = plantilla.Plantcontenido;
                                            contenido = String.Format(contenido, parametroContenido[0],
                                                parametroContenido[1], parametroContenido[2], parametroContenido[3],
                                                parametroContenido[4], parametroContenido[5], parametroContenido[6], parametroContenido[7]);

                                            AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                        }
                                        else
                                        {
                                            plantcodi = ConstantesEnviarCorreo.PlantcodiPdmPdo;
                                            plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                            ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                            parametroTitulo[0] = entity.Mailfecha.ToString("dd.MM.yyyy");
                                            parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                                    Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                                    DateTime.Now.Year.ToString();
                                            parametroContenido[1] = "SPR–IPDI-" + entity.Mailfecha.DayOfYear + "-" +
                                                                    entity.Mailfecha.Year.ToString();
                                            parametroContenido[2] = "SPR–IPDO-" + entity.Mailfecha.DayOfYear + "-" +
                                                                    entity.Mailfecha.Year.ToString();
                                            parametroContenido[3] =
                                                Tools.ObtenerNombreDia(entity.Mailfecha.DayOfWeek).ToLower() + " " +
                                                entity.Mailfecha.Day.ToString() + " de " +
                                                Tools.ObtenerNombreMes(entity.Mailfecha.Month) + " de " +
                                                entity.Mailfecha.Year.ToString();
                                            parametroContenido[4] =
                                                Tools.ObtenerNroSemanaAnio(entity.Mailfecha).ToString();
                                            parametroContenido[5] = Uri.EscapeUriString(url + "Operacion/ProgManten/ProgDiario?path=Operación/Programa de Mantenimiento/Programa Diario/" + entity.Mailfecha.Year + "/" + entity.Mailfecha.Month.ToString().PadLeft(2, '0') + "_" + Tools.ObtenerNombreMes(entity.Mailfecha.Month).ToUpper() + "/Día " + entity.Mailfecha.Day.ToString().PadLeft(2, '0') + "/");
                                            parametroContenido[6] = "PDI_" + entity.Mailfecha.ToString("ddMM_yyyy");
                                            parametroContenido[7] = Uri.EscapeUriString(url + "Operacion/ProgOperacion/ProgramaDiario?path=Operación/Programa de Operación/Programa Diario/" + entity.Mailfecha.Year + "/" + entity.Mailfecha.Month.ToString().PadLeft(2, '0') + "_" + Tools.ObtenerNombreMes(entity.Mailfecha.Month).ToUpper() + "/Día " + entity.Mailfecha.Day.ToString().PadLeft(2, '0') + "/");
                                            parametroContenido[8] = "PDO_" + entity.Mailfecha.ToString("ddMM_yyyy");
                                            parametroContenido[9] = entity.Mailprogramador;
                                            parametroContenido[10] = "SPR-COES";
                                            parametroContenido[11] = persona.GetTelefono(entity.Mailprogramador);



                                            asunto = plantilla.Plantasunto;
                                            asunto = String.Format(asunto, parametroTitulo[0]);

                                            contenido = plantilla.Plantcontenido;
                                            contenido = String.Format(contenido, parametroContenido[0],
                                                parametroContenido[1], parametroContenido[2], parametroContenido[3],
                                                parametroContenido[4], parametroContenido[5], parametroContenido[6],
                                                parametroContenido[7], parametroContenido[8], parametroContenido[9], parametroContenido[10], parametroContenido[11]);

                                            AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);

                                        }
                                    }
                                }
                                else
                                {
                                    if (entity.Mailturnonum == 2)
                                    {
                                        plantcodi = ConstantesEnviarCorreo.PlantcodiPdmSegundoFormato;
                                        plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                        ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                        parametroTitulo[0] = entity.Mailfecha.ToString("dd.MM.yyyy");
                                        parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                                Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                                DateTime.Now.Year.ToString();
                                        parametroContenido[1] = "SPR–IPDI-" + entity.Mailfecha.DayOfYear + "-" +
                                                                entity.Mailfecha.Year.ToString();
                                        parametroContenido[2] =
                                            Tools.ObtenerNombreDia(entity.Mailfecha.DayOfWeek).ToLower() + " " +
                                            entity.Mailfecha.Day.ToString() + " de " +
                                            Tools.ObtenerNombreMes(entity.Mailfecha.Month) + " de " +
                                            entity.Mailfecha.Year.ToString();
                                        parametroContenido[3] = Tools.ObtenerNroSemanaAnio(entity.Mailfecha).ToString();
                                        parametroContenido[4] = Uri.EscapeUriString(url + "Operacion/ProgManten/ProgDiario?path=Operación/Programa de Mantenimiento/Programa Diario/" + entity.Mailfecha.Year + "/"
                                            + entity.Mailfecha.Month.ToString().PadLeft(2, '0') + "_"
                                            + Tools.ObtenerNombreMes(entity.Mailfecha.Month).ToUpper() + "/Día "
                                            + entity.Mailfecha.Day.ToString().PadLeft(2, '0') + "/");
                                        parametroContenido[5] = "PDI_" + entity.Mailfecha.ToString("ddMM_yyyy");
                                        parametroContenido[6] = entity.Mailprogramador;
                                        parametroContenido[7] = "SPR-COES";
                                        parametroContenido[8] = persona.GetTelefono(entity.Mailprogramador);

                                        asunto = plantilla.Plantasunto;
                                        asunto = String.Format(asunto, parametroTitulo[0]);

                                        contenido = plantilla.Plantcontenido;
                                        contenido = String.Format(contenido, parametroContenido[0],
                                            parametroContenido[1], parametroContenido[2], parametroContenido[3],
                                            parametroContenido[4], parametroContenido[5], parametroContenido[6],
                                            parametroContenido[7], parametroContenido[8]);

                                        AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                    }
                                    else
                                    {
                                        if (entity.Mailturnonum == 3)
                                        {
                                            plantcodi = ConstantesEnviarCorreo.PlantcodiPdoSegundoFormato;
                                            plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                            ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                            parametroTitulo[0] = entity.Mailfecha.ToString("dd.MM.yyyy");
                                            parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                                    Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                                    DateTime.Now.Year.ToString();
                                            parametroContenido[1] = "SPR–IPDO-" + entity.Mailfecha.DayOfYear + "-" +
                                                                    entity.Mailfecha.Year.ToString() + " del día " +
                                                                    Tools.ObtenerNombreDia(entity.Mailfecha.DayOfWeek)
                                                                        .ToLower() + " " + entity.Mailfecha.Day + " de " +
                                                                    Tools.ObtenerNombreMes(entity.Mailfecha.Month) +
                                                                    " de " + entity.Mailfecha.Year.ToString();
                                            parametroContenido[2] =
                                                Tools.ObtenerNroSemanaAnio(entity.Mailfecha).ToString();
                                            parametroContenido[3] = Uri.EscapeUriString(url + "Operacion/ProgOperacion/ProgramaDiario?path=Operación/Programa de Operación/Programa Diario/"
                                                + entity.Mailfecha.Year + "/" + entity.Mailfecha.Month.ToString().PadLeft(2, '0')
                                                + "_" + Tools.ObtenerNombreMes(entity.Mailfecha.Month).ToUpper() + "/Día "
                                                + entity.Mailfecha.Day.ToString().PadLeft(2, '0') + "/");
                                            parametroContenido[4] = "PDO_" + entity.Mailfecha.ToString("ddMM_yyyy");
                                            parametroContenido[5] = entity.Mailprogramador;
                                            parametroContenido[6] = "SPR-COES";
                                            parametroContenido[7] = persona.GetTelefono(entity.Mailprogramador);

                                            asunto = plantilla.Plantasunto;
                                            asunto = String.Format(asunto, parametroTitulo[0]);

                                            contenido = plantilla.Plantcontenido;
                                            contenido = String.Format(contenido, parametroContenido[0],
                                                parametroContenido[1], parametroContenido[2], parametroContenido[3],
                                                parametroContenido[4], parametroContenido[5], parametroContenido[6], parametroContenido[7]);

                                            AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                        }
                                        else
                                        {
                                            plantcodi = ConstantesEnviarCorreo.PlantcodiPdmPdoSegundoFormato;
                                            plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                            ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                            parametroTitulo[0] = entity.Mailfecha.ToString("dd.MM.yyyy");
                                            parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                                    Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                                    DateTime.Now.Year.ToString();
                                            parametroContenido[1] = "SPR–IPDI-" + entity.Mailfecha.DayOfYear + "-" +
                                                                    entity.Mailfecha.Year.ToString();
                                            parametroContenido[2] = "SPR–IPDO-" + entity.Mailfecha.DayOfYear + "-" +
                                                                    entity.Mailfecha.Year.ToString();
                                            parametroContenido[3] =
                                                Tools.ObtenerNombreDia(entity.Mailfecha.DayOfWeek).ToLower() + " " +
                                                entity.Mailfecha.Day.ToString() + " de " +
                                                Tools.ObtenerNombreMes(entity.Mailfecha.Month) + " de " +
                                                entity.Mailfecha.Year.ToString();
                                            parametroContenido[4] =
                                                Tools.ObtenerNroSemanaAnio(entity.Mailfecha).ToString();
                                            parametroContenido[5] = Uri.EscapeUriString(url + "Operacion/ProgManten/ProgDiario?path=Operación/Programa de Mantenimiento/Programa Diario/" + entity.Mailfecha.Year + "/" + entity.Mailfecha.Month.ToString().PadLeft(2, '0') + "_" + Tools.ObtenerNombreMes(entity.Mailfecha.Month).ToUpper() + "/Día " + entity.Mailfecha.Day.ToString().PadLeft(2, '0') + "/");
                                            parametroContenido[6] = "PDI_" + entity.Mailfecha.ToString("ddMM_yyyy");
                                            parametroContenido[7] = Uri.EscapeUriString(url + "Operacion/ProgOperacion/ProgramaDiario?path=Operación/Programa de Operación/Programa Diario/"
                                                + entity.Mailfecha.Year + "/" + entity.Mailfecha.Month.ToString().PadLeft(2, '0')
                                                + "_" + Tools.ObtenerNombreMes(entity.Mailfecha.Month).ToUpper() + "/Día "
                                                + entity.Mailfecha.Day.ToString().PadLeft(2, '0') + "/");
                                            parametroContenido[8] = "PDO_" + entity.Mailfecha.ToString("ddMM_yyyy");
                                            parametroContenido[9] = entity.Mailprogramador;
                                            parametroContenido[10] = "SPR-COES";
                                            parametroContenido[11] = persona.GetTelefono(entity.Mailprogramador);

                                            asunto = plantilla.Plantasunto;
                                            asunto = String.Format(asunto, parametroTitulo[0]);

                                            contenido = plantilla.Plantcontenido;
                                            contenido = String.Format(contenido, parametroContenido[0],
                                                parametroContenido[1], parametroContenido[2], parametroContenido[3],
                                                parametroContenido[4], parametroContenido[5], parametroContenido[6],
                                                parametroContenido[7], parametroContenido[8], parametroContenido[9], parametroContenido[10], parametroContenido[11]);

                                            AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);


                                        }
                                    }

                                }

                                break;

                            #endregion


                            #region Reprograma

                            case ConstantesEnviarCorreo.SubcausacodiReprograma: //Reprograma

                                //hoja
                                if (entity.Mailhoja == null)
                                    entity.Mailhoja = "A";

                                //bloque horario
                                if (entity.Mailbloquehorario == null)
                                    entity.Mailbloquehorario = 1;

                                //causa de reprograma
                                if (entity.Mailreprogcausa == null)
                                    entity.Mailreprogcausa = "";


                                plantcodi = ConstantesEnviarCorreo.PlantcodiRdo;
                                plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                parametroTitulo[0] = entity.Mailfecha.DayOfYear + "-" + entity.Mailhoja;
                                parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                        Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                        DateTime.Now.Year.ToString();

                                parametroContenido[1] = parametroTitulo[0] + " del " +
                                                        Tools.ObtenerNombreDia(entity.Mailfecha.DayOfWeek).ToLower() +
                                                        " " + entity.Mailfecha.Day.ToString() + " de " +
                                                        Tools.ObtenerNombreMes(entity.Mailfecha.Month) + " de " +
                                                        entity.Mailfecha.Year.ToString();
                                parametroContenido[2] = Tools.ObtenerNroSemanaAnio(entity.Mailfecha).ToString();
                                parametroContenido[3] = entity.Mailreprogcausa;
                                parametroContenido[4] = Uri.EscapeUriString(url + "Operacion/ProgOperacion/RprogDiarioOp?path=Operación/Programa de Operación/Reprograma Diario Operación/" + entity.Mailfecha.Year + "/" + entity.Mailfecha.Month.ToString().PadLeft(2, '0') + "_" + Tools.ObtenerNombreMes(entity.Mailfecha.Month).ToUpper() + "/Día " + entity.Mailfecha.Day.ToString().PadLeft(2, '0') + "/");


                                parametroContenido[5] = Tools.ObtenerBloqueHorario((Int32)entity.Mailbloquehorario);
                                parametroContenido[6] = entity.Mailprogramador;
                                parametroContenido[7] = persona.GetCargo(entity.Mailprogramador);
                                parametroContenido[8] = persona.GetArea(entity.Mailprogramador);
                                parametroContenido[9] = persona.GetTelefono(entity.Mailprogramador);
                                parametroContenido[10] = "Reprog_" + entity.Mailfecha.Day.ToString().PadLeft(2, '0') + entity.Mailfecha.Month.ToString().PadLeft(2, '0');

                                asunto = plantilla.Plantasunto;
                                asunto = String.Format(asunto, parametroTitulo[0]);

                                contenido = plantilla.Plantcontenido;
                                contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                    parametroContenido[2], parametroContenido[3], parametroContenido[4],
                                    parametroContenido[5], parametroContenido[6], parametroContenido[7],
                                    parametroContenido[8], parametroContenido[9], parametroContenido[10]);

                                AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                break;

                            #endregion


                            #region Costo variable

                            case ConstantesEnviarCorreo.SubcausacodiCostoVariable: //costo variable

                                //hoja
                                if (entity.Mailhoja == null)
                                    entity.Mailhoja = "A";

                                plantcodi = ConstantesEnviarCorreo.PlantcodiCostoVariable;
                                plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                parametroTitulo[0] = Tools.ObtenerNroSemanaAnio(entity.Mailfecha) + "";
                                parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                        Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                        DateTime.Now.Year.ToString();
                                parametroContenido[1] = Tools.ObtenerNroSemanaAnio(entity.Mailfecha).ToString();
                                parametroContenido[2] = Uri.EscapeUriString(url + "Operacion/ProgOperacion/RprogDiarioOp");
                                parametroContenido[3] = entity.Mailprogramador;
                                parametroContenido[4] = persona.GetCargo(entity.Mailprogramador);
                                parametroContenido[5] = persona.GetArea(entity.Mailprogramador);
                                parametroContenido[6] = persona.GetTelefono(entity.Mailprogramador);

                                asunto = plantilla.Plantasunto;
                                asunto = String.Format(asunto, parametroTitulo[0]);

                                contenido = plantilla.Plantcontenido;
                                contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                    parametroContenido[2], parametroContenido[3], parametroContenido[4],
                                    parametroContenido[5], parametroContenido[6]);

                                AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                break;

                            #endregion

                            #region IDCOS

                            case ConstantesEnviarCorreo.SubcausacodiIdcos: //IDCOS

                                plantcodi = ConstantesEnviarCorreo.PlantcodiIdcos;
                                plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                parametroTitulo[0] = entity.Mailfecha.DayOfYear + "-" + entity.Mailfecha.Year.ToString() +
                                                     " - " +
                                                     Tools.ObtenerNombreDia(entity.Mailfecha.DayOfWeek).ToLower() + " " +
                                                     entity.Mailfecha.Day + " de " +
                                                     Tools.ObtenerNombreMes(entity.Mailfecha.Month);

                                parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                        Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                        DateTime.Now.Year.ToString();
                                parametroContenido[1] = Tools.ObtenerNombreDia(entity.Mailfecha.DayOfWeek).ToLower() +
                                                        " " + entity.Mailfecha.Day.ToString() + " de " +
                                                        Tools.ObtenerNombreMes(entity.Mailfecha.Month) + " de " +
                                                        entity.Mailfecha.Year.ToString() + " de la semana operativa " +
                                                        Tools.ObtenerNroSemanaAnio(entity.Mailfecha);


                                parametroContenido[2] = Uri.EscapeUriString(url + "PostOperacion/Reportes/Idcos?path=Post Operación/Reportes/IDCOS/" + entity.Mailfecha.Year + "/" + entity.Mailfecha.Month.ToString().PadLeft(2, '0') + "_" + Tools.ObtenerNombreMes(entity.Mailfecha.Month).ToUpper() + "/Día " + entity.Mailfecha.Day.ToString().PadLeft(2, '0') + "/");
                                parametroContenido[3] = entity.Mailprogramador;
                                parametroContenido[4] = persona.GetCargo(entity.Mailprogramador);
                                parametroContenido[5] = persona.GetArea(entity.Mailprogramador);
                                parametroContenido[6] = persona.GetTelefono(entity.Mailprogramador);
                                parametroContenido[7] = "IDCOS_" + entity.Mailfecha.Day.ToString().PadLeft(2, '0') + entity.Mailfecha.Month.ToString().PadLeft(2, '0');

                                asunto = plantilla.Plantasunto;
                                asunto = String.Format(asunto, parametroTitulo[0]);

                                contenido = plantilla.Plantcontenido;
                                contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                    parametroContenido[2], parametroContenido[3], parametroContenido[4],
                                    parametroContenido[5], parametroContenido[6], parametroContenido[7]);

                                AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);

                                break;

                            #endregion


                            #region Formato Deshabilitado

                            case ConstantesEnviarCorreo.SubcausacodiManto7Dias: //Mantenimiento 7 días
                            case ConstantesEnviarCorreo.SubcausacodiAnalisisAtr: //Analisis ATR

                                ObtenerCampoCorreo("", out from,
                                    "", out to, "", out cc,
                                    "", out bcc);


                                asunto = "FORMATO DESHABILITADO";
                                contenido = "FORMATO DESHABILITADO";

                                AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);

                                break;

                            #endregion


                            #region Información necesaria para el PDO

                            case ConstantesEnviarCorreo.SubcausacodiInfoNecesariaPdo: //Información necesaria para el PDO

                                plantcodi = ConstantesEnviarCorreo.PlantcodiInfoNecesariaPdo;
                                plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                parametroContenido[0] = entity.Mailfecha.Day.ToString() + " de " +
                                                        Tools.ObtenerNombreMes(entity.Mailfecha.Month) + " de " +
                                                        entity.Mailfecha.Year.ToString();

                                parametroContenido[1] = "00:00 a 06:00 horas y desde las 06:00 horas del " +
                                                        entity.Mailfecha.AddDays(1).ToString(Constantes.FormatoFecha) +
                                                        " hasta las 06:00 horas del " +
                                                        entity.Mailfecha.AddDays(2).ToString(Constantes.FormatoFecha);
                                parametroContenido[2] = entity.Mailprogramador;
                                parametroContenido[3] = persona.GetCargo(entity.Mailprogramador);
                                parametroContenido[4] = persona.GetArea(entity.Mailprogramador);
                                parametroContenido[5] = persona.GetTelefono(entity.Mailprogramador);

                                asunto = plantilla.Plantasunto;

                                contenido = plantilla.Plantcontenido;
                                contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                    parametroContenido[2], parametroContenido[3], parametroContenido[4],
                                    parametroContenido[5]);

                                AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                break;

                            #endregion


                            #region Racionamiento de carga

                            case ConstantesEnviarCorreo.SubcausacodiRacionamientoCarga: //Racionamiento de carga

                                plantcodi = ConstantesEnviarCorreo.PlantcodiRacionamientoCarga;
                                plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                EquipamientoAppServicio servEquipo = new EquipamientoAppServicio();
                                EqEquipoDTO equipo = servEquipo.GetByIdEqEquipo((Int32)entity.Equicodi);

                                parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                        Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                        DateTime.Now.Year.ToString();
                                parametroContenido[1] = ((equipo.Equiabrev != null) ? equipo.Equiabrev : "_NO DEFINIDO") +
                                                        " de la " +
                                                        ((equipo.Tareaabrev != null)
                                                            ? equipo.Tareaabrev
                                                            : "_NO DEFINIDO") + " " +
                                                        ((equipo.Areanomb != null) ? equipo.Areanomb : "_NO DEFINIDO");
                                parametroContenido[2] = entity.Mailprogramador;
                                parametroContenido[3] = persona.GetCargo(entity.Mailprogramador);
                                parametroContenido[4] = persona.GetArea(entity.Mailprogramador);
                                parametroContenido[5] = persona.GetTelefono(entity.Mailprogramador);

                                asunto = plantilla.Plantasunto;

                                contenido = plantilla.Plantcontenido;
                                contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                    parametroContenido[2], parametroContenido[3], parametroContenido[4],
                                    parametroContenido[5]);

                                AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                break;

                            #endregion


                            #region Indisponibilidad del Sistema telefónico

                            case ConstantesEnviarCorreo.SubcausacodiIndispSistemaTelef: //Indisponibilidad del Sistema telefónico

                                plantcodi = ConstantesEnviarCorreo.PlantcodiIndispSistemaTelef;
                                plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                        Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                        DateTime.Now.Year.ToString();
                                parametroContenido[1] = entity.Mailprogramador;
                                parametroContenido[2] = persona.GetCargo(entity.Mailprogramador);
                                parametroContenido[3] = persona.GetArea(entity.Mailprogramador);
                                parametroContenido[4] = persona.GetTelefono(entity.Mailprogramador);

                                asunto = plantilla.Plantasunto;

                                contenido = plantilla.Plantcontenido;
                                contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                    parametroContenido[2], parametroContenido[3], parametroContenido[4]);

                                AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                break;

                            #endregion


                            #region Sistema de Comunicación Principal

                            case ConstantesEnviarCorreo.SubcausacodiSistemaComunicPrincipal: //Sistema de Comunicación Principal

                                plantcodi = ConstantesEnviarCorreo.PlantcodiDisponibSistemaTelef;
                                plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                        Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                        DateTime.Now.Year.ToString();
                                parametroContenido[1] = entity.Mailprogramador;
                                parametroContenido[2] = persona.GetCargo(entity.Mailprogramador);
                                parametroContenido[3] = persona.GetArea(entity.Mailprogramador);
                                parametroContenido[4] = persona.GetTelefono(entity.Mailprogramador);

                                asunto = plantilla.Plantasunto;

                                contenido = plantilla.Plantcontenido;
                                contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                    parametroContenido[2], parametroContenido[3], parametroContenido[4]);

                                AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                break;

                            #endregion


                            #region Rechazo manual de carga

                            case ConstantesEnviarCorreo.SubcausacodiRechazoManualCarga: //Rechazo manual de carga

                                //hoja
                                if (entity.Mailhoja == null)
                                    entity.Mailhoja = "A";

                                //bloque horario
                                if (entity.Mailbloquehorario == null)
                                    entity.Mailbloquehorario = 1;

                                //causa de reprograma
                                if (entity.Mailreprogcausa == null)
                                    entity.Mailreprogcausa = "";

                                plantcodi = ConstantesEnviarCorreo.PlantcodiRechazoManualDeCarga;
                                plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                        Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                        DateTime.Now.Year.ToString();

                                parametroContenido[1] = entity.Mailprogramador;
                                parametroContenido[2] = persona.GetCargo(entity.Mailprogramador);
                                parametroContenido[3] = persona.GetArea(entity.Mailprogramador);
                                parametroContenido[4] = persona.GetTelefono(entity.Mailprogramador);

                                asunto = plantilla.Plantasunto;

                                contenido = plantilla.Plantcontenido;
                                contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                    parametroContenido[2], parametroContenido[3], parametroContenido[4]);

                                AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);

                                break;

                            #endregion


                            #region Programa Semanal Preliminar

                            case ConstantesEnviarCorreo.SubcausacodiProgramaSemanalPreliminar: //Programa Semanal Preliminar

                                int semanaActual = Tools.ObtenerNroSemanaAnio(entity.Mailfecha);
                                string semanaSiguiente = (semanaActual).ToString().PadLeft(2, '0');
                                int anioSgteSemana = entity.Mailfecha.AddDays(7).Year;

                                DateTime fechaSabado =
                                    Tools.ObtenerFechaInicioSemanaOperativa(entity.Mailfecha).AddDays(1);

                                plantcodi = ConstantesEnviarCorreo.PlantcodiProgramaSemanalPrelim;
                                plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                parametroTitulo[0] = semanaSiguiente;//semanaSiguiente + "_" + anioSgteSemana;

                                parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                        Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                        DateTime.Now.Year.ToString();
                                parametroContenido[1] = semanaSiguiente;
                                //parametroContenido[2] = url + "Operacion/ProgManten/ProgSemanal";

                                string fechaInicio = fechaSabado.ToString(Constantes.FormatoFecha).Replace("/", ".");
                                string fechaFin = fechaSabado.AddDays(6).ToString(Constantes.FormatoFecha).Replace("/", ".");


                                parametroContenido[2] = Uri.EscapeUriString(url + "Operacion/ProgManten/ProgSemanal?path=Operación/Programa de Mantenimiento/Programa Semanal/" + anioSgteSemana + "/" + semanaSiguiente + "_SEMANAL N° " + semanaSiguiente + " (" + fechaInicio + " - " + fechaFin + ")/Preliminar/");
                                //parametroContenido[3] = "PSM_Sem" + semanaSiguiente + "_" + anioSgteSemana + "_Preliminar";//"Programa Semanal Preliminar de Mantenimiento";
                                //req 12314 may2020
                                parametroContenido[3] = "PSI_Sem" + semanaSiguiente + "_" + anioSgteSemana + "_Preliminar";//"Programa Semanal Preliminar de Mantenimiento";

                                //parametroContenido[4] = url + "Operacion/ProgOperacion/ProgSemanalOp";
                                parametroContenido[4] = Uri.EscapeUriString(url + "Operacion/ProgOperacion/ProgSemanalOp?path=Operación/Programa de Operación/Programa Semanal Operación/" + anioSgteSemana + "/" + semanaSiguiente + "_SEMANAL N° " + semanaSiguiente + " (" + fechaInicio + " - " + fechaFin + ")/Preliminar/");
                                parametroContenido[5] = "PSO_Sem" + semanaSiguiente + "_" + anioSgteSemana + "_Preliminar";//"Programa Semanal Preliminar de Operación";
                                parametroContenido[6] = entity.Mailprogramador;
                                parametroContenido[7] = persona.GetArea(entity.Mailprogramador);
                                parametroContenido[8] = persona.GetTelefono(entity.Mailprogramador);

                                asunto = plantilla.Plantasunto;
                                asunto = String.Format(asunto, parametroTitulo[0]);

                                contenido = plantilla.Plantcontenido;
                                contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                    parametroContenido[2], parametroContenido[3], parametroContenido[4],
                                    parametroContenido[5], parametroContenido[6], parametroContenido[7],
                                    parametroContenido[8]);

                                AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                break;

                            #endregion


                            #region Programa Semanal Final

                            case ConstantesEnviarCorreo.SubcausacodiProgramaSemanalFinal: //Programa Semanal Final

                                semanaActual = Tools.ObtenerNroSemanaAnio(entity.Mailfecha);
                                semanaSiguiente = (Tools.ObtenerNroSemanaAnio(entity.Mailfecha.AddDays(7))).ToString().PadLeft(2, '0');
                                anioSgteSemana = entity.Mailfecha.AddDays(7).Year;

                                fechaSabado = Tools.ObtenerFechaInicioSemanaOperativa(entity.Mailfecha).AddDays(1);

                                if (entity.Mailturnonum == 2)
                                {
                                    plantcodi = ConstantesEnviarCorreo.PlantcodiProgramaSemanalFinalManto;
                                    plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                    ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                    parametroTitulo[0] = semanaSiguiente;
                                    parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                            Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                            DateTime.Now.Year.ToString();

                                    fechaInicio = fechaSabado.ToString(Constantes.FormatoFecha).Replace("/", ".");
                                    fechaFin = fechaSabado.AddDays(6).ToString(Constantes.FormatoFecha).Replace("/", ".");

                                    parametroContenido[1] = semanaSiguiente;
                                    //req 14867-corrección del link - nov 2020
                                    parametroContenido[2] = Uri.EscapeUriString(url + "Operacion/ProgManten/ProgSemanal?path=Operación/Programa de Mantenimiento/Programa Semanal/" + anioSgteSemana + "/" + semanaSiguiente + "_SEMANAL N° " + semanaSiguiente + " (" + fechaInicio + " - " + fechaFin + ")/Final/");
                                    //req 12314 may2020
                                    parametroContenido[3] = "PSI_Sem" + semanaSiguiente + "_" + anioSgteSemana + "_Final"; //"Programa Semanal Mantenimiento";
                                    parametroContenido[4] = entity.Mailprogramador;
                                    parametroContenido[5] = persona.GetArea(entity.Mailprogramador);
                                    parametroContenido[6] = persona.GetTelefono(entity.Mailprogramador);

                                    asunto = plantilla.Plantasunto;
                                    asunto = String.Format(asunto, parametroTitulo[0]);

                                    contenido = plantilla.Plantcontenido;
                                    contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                        parametroContenido[2], parametroContenido[3], parametroContenido[4],
                                        parametroContenido[5], parametroContenido[6]);

                                    AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                }
                                else
                                {
                                    if (entity.Mailturnonum == 3)
                                    {
                                        plantcodi = ConstantesEnviarCorreo.PlantcodiProgramaSemanalFinalOperacion;
                                        plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                        ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                        parametroTitulo[0] = semanaSiguiente;
                                        parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                                Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                                DateTime.Now.Year.ToString();
                                        parametroContenido[1] = semanaSiguiente;
                                        parametroContenido[2] = Uri.EscapeUriString(url + "Operacion/ProgOperacion/ProgSemanalOp");
                                        parametroContenido[3] = "PSO_Sem" + semanaSiguiente + "_" + anioSgteSemana + "_Final";//"Programa Semanal de Operación";
                                        parametroContenido[4] = entity.Mailprogramador;
                                        parametroContenido[5] = persona.GetArea(entity.Mailprogramador);
                                        parametroContenido[6] = persona.GetTelefono(entity.Mailprogramador);

                                        asunto = plantilla.Plantasunto;
                                        asunto = String.Format(asunto, parametroTitulo[0]);

                                        contenido = plantilla.Plantcontenido;
                                        contenido = String.Format(contenido, parametroContenido[0],
                                            parametroContenido[1], parametroContenido[2], parametroContenido[3],
                                            parametroContenido[4], parametroContenido[5], parametroContenido[6]);

                                        AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                    }
                                    else
                                    {
                                        plantcodi = ConstantesEnviarCorreo.PlantcodiProgramaSemanalFinalOperacionManto;
                                        plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                        ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                        parametroTitulo[0] = semanaSiguiente;//semanaSiguiente + "_" + anioSgteSemana;                                        

                                        parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                                Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                                DateTime.Now.Year.ToString();
                                        parametroContenido[1] = semanaSiguiente;

                                        fechaInicio = fechaSabado.ToString(Constantes.FormatoFecha).Replace("/", ".");
                                        fechaFin = fechaSabado.AddDays(6).ToString(Constantes.FormatoFecha).Replace("/", ".");


                                        parametroContenido[2] = Uri.EscapeUriString(url + "Operacion/ProgManten/ProgSemanal?path=Operación/Programa de Mantenimiento/Programa Semanal/" + anioSgteSemana + "/" + semanaSiguiente + "_SEMANAL N° " + semanaSiguiente + " (" + fechaInicio + " - " + fechaFin + ")/Final/");
                                        //parametroContenido[3] = "PSM_Sem" + semanaSiguiente + "_" + anioSgteSemana + "_Final"; //"Programa Semanal de Mantenimiento";
                                        //req 12314 may2020
                                        parametroContenido[3] = "PSI_Sem" + semanaSiguiente + "_" + anioSgteSemana + "_Final"; //"Programa Semanal Mantenimiento";

                                        parametroContenido[4] = Uri.EscapeUriString(url + "Operacion/ProgOperacion/ProgSemanalOp?path=Operación/Programa de Operación/Programa Semanal Operación/" + anioSgteSemana + "/" + semanaSiguiente + "_SEMANAL N° " + semanaSiguiente + " (" + fechaInicio + " - " + fechaFin + ")/Final/");
                                        parametroContenido[5] = "PSO_Sem" + semanaSiguiente + "_" + anioSgteSemana + "_Final";//"Programa Semanal de Operación";
                                        parametroContenido[6] = entity.Mailprogramador;
                                        parametroContenido[7] = persona.GetArea(entity.Mailprogramador);
                                        parametroContenido[8] = persona.GetTelefono(entity.Mailprogramador);

                                        asunto = plantilla.Plantasunto;
                                        asunto = String.Format(asunto, parametroTitulo[0]);

                                        contenido = plantilla.Plantcontenido;
                                        contenido = String.Format(contenido, parametroContenido[0],
                                            parametroContenido[1], parametroContenido[2], parametroContenido[3],
                                            parametroContenido[4], parametroContenido[5], parametroContenido[6],
                                            parametroContenido[7], parametroContenido[8]);

                                        AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                    }

                                }





                                break;

                            #endregion


                            #region Info PSO y PSM

                            case ConstantesEnviarCorreo.SubcausacodiInfoPsoPsm: //Info PSO y PSM

                                semanaActual = Tools.ObtenerNroSemanaAnio(entity.Mailfecha);
                                semanaSiguiente = (semanaActual + 1).ToString().PadLeft(2, '0');

                                DateTime fechaInicioSemana = Tools.ObtenerFechaInicioSemana(entity.Mailfecha.AddDays(7));

                                plantcodi = ConstantesEnviarCorreo.PlantcodiInfoPsoPsm;
                                plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                parametroTitulo[0] = semanaSiguiente + " del " +
                                                     fechaInicioSemana.ToString("dd.MM.yyyy") + " al " +
                                                     fechaInicioSemana.AddDays(6).ToString("dd.MM.yyyy");

                                parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                        Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                        DateTime.Now.Year.ToString();
                                parametroContenido[1] = semanaSiguiente;
                                parametroContenido[2] =
                                    Tools.ObtenerNombreDia(entity.Mailfecha.AddDays(1).DayOfWeek).ToLower() + " " +
                                    entity.Mailfecha.AddDays(1).ToString("dd.MM.yyyy");

                                parametroContenido[3] =
                                    Tools.ObtenerNombreDia(entity.Mailfecha.AddDays(3).DayOfWeek).ToLower() + " " +
                                    entity.Mailfecha.AddDays(3).ToString("dd.MM.yyyy");
                                parametroContenido[4] = entity.Mailprogramador;
                                parametroContenido[5] = persona.GetArea(entity.Mailprogramador);
                                parametroContenido[6] = persona.GetTelefono(entity.Mailprogramador);


                                asunto = plantilla.Plantasunto;
                                asunto = String.Format(asunto, parametroTitulo[0]);

                                contenido = plantilla.Plantcontenido;
                                contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                    parametroContenido[2], parametroContenido[3], parametroContenido[4],
                                    parametroContenido[5], parametroContenido[6]);

                                AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                break;

                            #endregion


                            #region Término Elaboración Programa

                            case ConstantesEnviarCorreo.SubcausacodiTerminoElabPrograma: //Termino Elab. Programa

                                semanaActual = Tools.ObtenerNroSemanaAnio(entity.Mailfecha);
                                semanaSiguiente = (Tools.ObtenerNroSemanaAnio(entity.Mailfecha.AddDays(7))).ToString().PadLeft(2, '0');

                                if (entity.Mailtipoprograma == null)
                                    entity.Mailtipoprograma = 0;

                                int tipoPrograma = (Int32)entity.Mailtipoprograma;
                                string especialista = entity.Mailespecialista;
                                string correoEspecialista = persona.GetMail(especialista);
                                to = "";


                                switch (tipoPrograma)
                                {
                                    case 0:
                                        plantcodi = ConstantesEnviarCorreo.PlantcodiTermElabProgVerFinPsmPso;
                                        plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                        ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                        parametroTitulo[0] = entity.Mailfecha.ToString("dd.MM.yyyy");

                                        parametroContenido[0] = entity.Mailespecialista;
                                        parametroContenido[1] = entity.Mailfecha.ToString("dd.MM.yyyy");
                                        parametroContenido[2] = Uri.EscapeUriString(url + "Operacion/ProgManten/ProgDiario?path=Operación/Programa de Mantenimiento/Programa Diario/" + entity.Mailfecha.Year + "/" + entity.Mailfecha.Month.ToString().PadLeft(2, '0') + "_" + Tools.ObtenerNombreMes(entity.Mailfecha.Month).ToUpper() + "/Día " + entity.Mailfecha.Day.ToString().PadLeft(2, '0') + "/");
                                        string pdi = entity.Mailfecha.Day.ToString().PadLeft(2, '0') + entity.Mailfecha.Month.ToString().PadLeft(2, '0') + "_" + entity.Mailfecha.Year;
                                        parametroContenido[2] = "<a href=\"" + parametroContenido[2] + "\" target=\"_blank\">PDI_" + pdi + "</a><br/><br/>";
                                        parametroContenido[3] = entity.Mailprogramador;
                                        parametroContenido[4] = persona.GetCargo(entity.Mailprogramador);
                                        parametroContenido[5] = persona.GetArea(entity.Mailprogramador);
                                        parametroContenido[6] = persona.GetTelefono(entity.Mailprogramador);

                                        asunto = plantilla.Plantasunto;
                                        asunto = String.Format(asunto, parametroTitulo[0]);

                                        contenido = plantilla.Plantcontenido;
                                        contenido = String.Format(contenido, parametroContenido[0],
                                            parametroContenido[1], parametroContenido[2], parametroContenido[3],
                                            parametroContenido[4], parametroContenido[5], parametroContenido[6]);

                                        to = servPersona.GetMail(entity.Mailespecialista);
                                        cc = servPersona.GetMail(entity.Mailprogramador);

                                        AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                        break;
                                    case 1:

                                        plantcodi = ConstantesEnviarCorreo.PlantcodiTermElabProgPr43;
                                        plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                        ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                        parametroTitulo[0] = entity.Mailfecha.ToString("dd.MM.yyyy");

                                        parametroContenido[0] = entity.Mailespecialista;
                                        parametroContenido[1] = entity.Mailfecha.ToString("dd.MM.yyyy");
                                        parametroContenido[2] = Uri.EscapeUriString(url + "Operacion/ProgOperacion/ProgramaDiario?path=Operación/Programa de Operación/Programa Diario/" + entity.Mailfecha.Year + "/" + entity.Mailfecha.Month.ToString().PadLeft(2, '0') + "_" + Tools.ObtenerNombreMes(entity.Mailfecha.Month).ToUpper() + "/Día " + entity.Mailfecha.Day.ToString().PadLeft(2, '0') + "/");
                                        string pdo = entity.Mailfecha.Day.ToString().PadLeft(2, '0') + entity.Mailfecha.Month.ToString().PadLeft(2, '0') + "_" + entity.Mailfecha.Year;
                                        parametroContenido[2] = "<a href=\"" + parametroContenido[2] + "\" target=\"_blank\">PDO_" + pdo + "</a><br/><br/>";
                                        parametroContenido[3] = entity.Mailprogramador;
                                        parametroContenido[4] = persona.GetCargo(entity.Mailprogramador);
                                        parametroContenido[5] = persona.GetArea(entity.Mailprogramador);
                                        parametroContenido[6] = persona.GetTelefono(entity.Mailprogramador);

                                        asunto = plantilla.Plantasunto;
                                        asunto = String.Format(asunto, parametroTitulo[0]);

                                        contenido = plantilla.Plantcontenido;
                                        contenido = String.Format(contenido, parametroContenido[0],
                                            parametroContenido[1], parametroContenido[2], parametroContenido[3],
                                            parametroContenido[4], parametroContenido[5], parametroContenido[6]);

                                        to = servPersona.GetMail(entity.Mailespecialista);
                                        cc = servPersona.GetMail(entity.Mailprogramador);


                                        AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                        break;
                                    case 2:

                                        plantcodi = ConstantesEnviarCorreo.PlantcodiTermElabProgPdm;
                                        plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                        ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                        parametroTitulo[0] = semanaActual.ToString();

                                        parametroContenido[0] = entity.Mailespecialista;
                                        parametroContenido[1] = semanaActual.ToString();
                                        parametroContenido[2] = Uri.EscapeUriString(url + "Operacion/ProgManten/ProgSemanal?path=Operación/Programa de Mantenimiento/Programa Semanal/" + entity.Mailfecha.AddDays(1).Year +
                                            "/" + semanaActual.ToString().PadLeft(2, '0') + "_SEMANAL N° " + semanaActual.ToString().PadLeft(2, '0') +
                                            " (" + entity.Mailfecha.ToString("dd.MM.yyyy") +
                                            " - " + entity.Mailfecha.AddDays(6).ToString("dd.MM.yyyy") + ")/Final/");
                                        string psi = "Sem" + semanaActual.ToString().PadLeft(2, '0') + "_" + entity.Mailfecha.Year + "_Final";
                                        parametroContenido[2] = "<a href=\"" + parametroContenido[2] + "\" target=\"_blank\">PSI_" + psi + "</a><br/><br/>";
                                        parametroContenido[3] = entity.Mailprogramador;
                                        parametroContenido[4] = persona.GetCargo(entity.Mailprogramador);
                                        parametroContenido[5] = persona.GetArea(entity.Mailprogramador);
                                        parametroContenido[6] = persona.GetTelefono(entity.Mailprogramador);

                                        asunto = plantilla.Plantasunto;
                                        asunto = String.Format(asunto, parametroTitulo[0]);

                                        contenido = plantilla.Plantcontenido;
                                        contenido = String.Format(contenido, parametroContenido[0],
                                            parametroContenido[1], parametroContenido[2], parametroContenido[3],
                                            parametroContenido[4], parametroContenido[5], parametroContenido[6]);

                                        to = servPersona.GetMail(entity.Mailespecialista);
                                        cc = servPersona.GetMail(entity.Mailprogramador);

                                        AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                        break;
                                    case 3:

                                        plantcodi = ConstantesEnviarCorreo.PlantcodiTermElabProgPdo;
                                        plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                        ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                        parametroTitulo[0] = semanaActual.ToString();

                                        parametroContenido[0] = entity.Mailespecialista;
                                        parametroContenido[1] = semanaActual.ToString();
                                        parametroContenido[2] = Uri.EscapeUriString(url + "Operacion/ProgOperacion/ProgSemanalOp?path=Operación/Programa de Operación/Programa Semanal Operación/" + entity.Mailfecha.AddDays(1).Year +
                                            "/" + semanaActual.ToString().PadLeft(2, '0') + "_SEMANAL N° " + semanaActual.ToString().PadLeft(2, '0') +
                                            " (" + entity.Mailfecha.ToString("dd.MM.yyyy") +
                                            " - " + entity.Mailfecha.AddDays(6).ToString("dd.MM.yyyy") + ")/Final/");
                                        string pso = "Sem" + semanaActual.ToString().PadLeft(2, '0') + "_" + entity.Mailfecha.Year + "_Final";
                                        parametroContenido[2] = "<a href=\"" + parametroContenido[2] + "\" target=\"_blank\">PSO_" + pso + "</a><br/><br/>";
                                        parametroContenido[3] = entity.Mailprogramador;
                                        parametroContenido[4] = persona.GetCargo(entity.Mailprogramador);
                                        parametroContenido[5] = persona.GetArea(entity.Mailprogramador);
                                        parametroContenido[6] = persona.GetTelefono(entity.Mailprogramador);

                                        asunto = plantilla.Plantasunto;
                                        asunto = String.Format(asunto, parametroTitulo[0]);

                                        contenido = plantilla.Plantcontenido;
                                        contenido = String.Format(contenido, parametroContenido[0],
                                            parametroContenido[1], parametroContenido[2], parametroContenido[3],
                                            parametroContenido[4], parametroContenido[5], parametroContenido[6]);

                                        to = servPersona.GetMail(entity.Mailespecialista);
                                        cc = servPersona.GetMail(entity.Mailprogramador);

                                        AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                        break;

                                }


                                break;

                            #endregion


                            #region Entrega PDO Y PDM

                            case ConstantesEnviarCorreo.SubcausacodiEntregaPdpoPdm: //Entrega PDO Y PDM

                                string semanaActualPdo =
                                    Tools.ObtenerNroSemanaAnio(entity.Mailfecha).ToString().PadLeft(2, '0');

                                plantcodi = ConstantesEnviarCorreo.PlantcodiEntregaPdoPdm;
                                plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                parametroTitulo[0] = entity.Mailfecha.ToString("dd.MM.yyyy");
                                ;
                                parametroContenido[0] = @"\\coes.org.pe\Areas\SPR\2-DESPACHO\1-PDO\" + entity.Mailfecha.Year +
                                                        @"\Sem" + semanaActualPdo +
                                                        entity.Mailfecha.Year.ToString().Substring(2) + @"\PDO_" +
                                                        entity.Mailfecha.Day.ToString().PadLeft(2, '0') +
                                                        entity.Mailfecha.Month.ToString().PadLeft(2, '0') +
                                                        @"\8-Entrega";
                                parametroContenido[1] = @"\\coes.org.pe\Areas\SPR\1-MANTENIMIENTO\1-PDM\" + entity.Mailfecha.Year +
                                                        @"\Sem" + semanaActualPdo +
                                                        entity.Mailfecha.Year.ToString().Substring(2) + @"\PDM_" +
                                                        entity.Mailfecha.Day.ToString().PadLeft(2, '0') +
                                                        entity.Mailfecha.Month.ToString().PadLeft(2, '0') +
                                                        @"\8-Entrega";
                                parametroContenido[2] = entity.Mailprogramador;
                                parametroContenido[3] = persona.GetCargo(entity.Mailprogramador);
                                parametroContenido[4] = persona.GetArea(entity.Mailprogramador);
                                parametroContenido[5] = persona.GetTelefono(entity.Mailprogramador);

                                asunto = plantilla.Plantasunto;
                                asunto = String.Format(asunto, parametroTitulo[0]);

                                contenido = plantilla.Plantcontenido;
                                contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                    parametroContenido[2], parametroContenido[3], parametroContenido[4],
                                    parametroContenido[5], parametroContenido[6]);

                                AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                break;

                            #endregion

                            #region Reporte de emergencia

                            case ConstantesEnviarCorreo.SubcausacodiReporteemergencia: //Entrega PDO Y PDM

                                plantcodi = ConstantesEnviarCorreo.Plantcodireporteemergencia;

                                plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                EquipamientoAppServicio servEquipo1 = new EquipamientoAppServicio();
                                EqEquipoDTO equipo1 = servEquipo1.GetByIdEqEquipo((Int32)entity.Equicodi);

                                parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                        Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                        DateTime.Now.Year.ToString();
                                parametroContenido[1] = (entity.Mailhora != null) ? ((DateTime)entity.Mailhora).ToString("HH:mm") : "";
                                parametroContenido[2] = ((equipo1.Equiabrev != null) ? equipo1.Equiabrev : "_NO DEFINIDO") +
                                                        " de la " +
                                                        ((equipo1.TAREAABREV != null)
                                                            ? equipo1.TAREAABREV
                                                            : "_NO DEFINIDO") + " " +
                                                        ((equipo1.AREANOMB != null) ? equipo1.AREANOMB : "_NO DEFINIDO");
                                parametroContenido[3] = entity.Mailconsecuencia;

                                parametroContenido[4] = entity.CoordinadorTurno;

                                asunto = plantilla.Plantasunto + " " + parametroContenido[2];

                                contenido = plantilla.Plantcontenido;
                                contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                    parametroContenido[2], parametroContenido[3], parametroContenido[4]);

                                AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                break;


                            #endregion

                            #region COSTOS MARGINALES Y HORAS DE OPERACIÓN PARA IEOD 

                            case ConstantesEnviarCorreo.SubcausacodiCMgHOparaIEDO:

                                plantcodi = ConstantesEnviarCorreo.PlantcodiCMGsHOparaIEOD;
                                plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                DateTime diaAnterior = entity.Mailfecha.AddDays(-1);
                                string diaAnteriorMes = diaAnterior.Day.ToString("00") + diaAnterior.Month.ToString("00");
                                string semAnioDiaAnterior = Tools.ObtenerNroSemanaAnio(diaAnterior).ToString("00") + diaAnterior.Year.ToString().Substring(2, 2);
                                string directorioT = @"\\coes.org.pe\Areas\SME\COSTO MARGINALES CP\{0}\IEOD\sem{1}\{2}";
                                string dir = String.Format(directorioT, diaAnterior.Year, semAnioDiaAnterior, ("CMgCP" + diaAnteriorMes));


                                parametroTitulo[0] = "(CMg_CP_" + diaAnteriorMes + ") y (HOP_" + diaAnteriorMes + ")";
                                parametroContenido[0] = diaAnterior.Day.ToString("00") + "/" + diaAnterior.Month.ToString("00");
                                //parametroContenido[1] = String.Format("<a href='https:{0}'>{0}</a>", dir);
                                parametroContenido[1] = String.Format("{0}", dir);
                                parametroContenido[2] = entity.Mailespecialista.ToUpper();

                                asunto = plantilla.Plantasunto;
                                asunto = String.Format(asunto, parametroTitulo[0]);

                                contenido = plantilla.Plantcontenido;
                                contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1], parametroContenido[2]);

                                AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);

                                break;

                            #endregion

                            #region ACTUALIZACION COSTOS MARGINALES Y HORAS DE OPERACIÓN PARA IEOD 

                            case ConstantesEnviarCorreo.SubcausacodiUpdateCMgHOparaIEDO:

                                plantcodi = ConstantesEnviarCorreo.PlantcodiUpdateCMGsHOparaIEOD;
                                plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                DateTime diaAnteriorUpdate = entity.Mailfecha.AddDays(-1);
                                string diaAnteriorMesUpd = diaAnteriorUpdate.Day.ToString("00") + diaAnteriorUpdate.Month.ToString("00");
                                string semAnioDiaAnteriorUpd = Tools.ObtenerNroSemanaAnio(diaAnteriorUpdate).ToString("00") + diaAnteriorUpdate.Year.ToString().Substring(2, 2);
                                string directorioTUpd = @"\\coes.org.pe\Areas\SME\COSTO MARGINALES CP\{0}\IEOD\sem{1}\{2}";
                                string dirUdp = String.Format(directorioTUpd, diaAnteriorUpdate.Year, semAnioDiaAnteriorUpd, ("CMgCP" + diaAnteriorMesUpd));


                                parametroTitulo[0] = "(CMg_CP_" + diaAnteriorMesUpd + ") y (HOP_" + diaAnteriorMesUpd + ")";
                                parametroContenido[0] = diaAnteriorUpdate.Day.ToString("00") + "/" + diaAnteriorUpdate.Month.ToString("00");
                                //parametroContenido[1] = String.Format("<a href='https:{0}'>{0}</a>", dirUdp);
                                parametroContenido[1] = String.Format("{0}", dirUdp);
                                parametroContenido[2] = entity.Mailespecialista.ToUpper();

                                asunto = plantilla.Plantasunto;
                                asunto = String.Format(asunto, parametroTitulo[0]);

                                contenido = plantilla.Plantcontenido;
                                contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1], parametroContenido[2]);

                                AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);

                                break;

                            #endregion

                            #region REPORTE DE COSTOS MARGINALES (QUINCENAL)

                            case ConstantesEnviarCorreo.SubcausacodiReporteCMg:

                                plantcodi = ConstantesEnviarCorreo.PlantcodiReporteCMg;
                                plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                parametroTitulo[0] = Tools.ObtenerNombreMes(entity.Mailfecha.Month) + " " + entity.Mailfecha.Year.ToString() +
                                    " (revisados al 15/" + entity.Mailfecha.Month.ToString("00") + "/" + entity.Mailfecha.Year.ToString() + ")";

                                parametroContenido[0] = String.Format("{0} de {1} de {2}", DateTime.Now.Day.ToString(), Tools.ObtenerNombreMes(DateTime.Now.Month), DateTime.Now.Year.ToString());

                                parametroContenido[1] = String.Format("PRIMERA QUINCENA de {0} de {1} (revisados del 01 al 15/{2}/{3})", Tools.ObtenerNombreMes(entity.Mailfecha.Month), entity.Mailfecha.Year.ToString(), entity.Mailfecha.Month.ToString("00"), entity.Mailfecha.Year.ToString());

                                parametroContenido[2] = String.Format("<a href='"+url+"mercadomayorista/costosmarginales/revisados?path=Operaci%C3%B3n%2FCostos%20Marginales%20CP%2FRevisados%2F{0}%2F{1}%2F'>Costos Marginales - {2}</a>", entity.Mailfecha.Year.ToString(), (entity.Mailfecha.Month.ToString("00") + "_" + Tools.ObtenerNombreMes(entity.Mailfecha.Month).ToUpper()), Tools.ObtenerNombreMes(entity.Mailfecha.Month));

                                parametroContenido[3] = entity.Mailespecialista.ToUpper();

                                asunto = plantilla.Plantasunto;
                                asunto = String.Format(asunto, parametroTitulo[0]);

                                contenido = plantilla.Plantcontenido;
                                contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                    parametroContenido[2], parametroContenido[3]);

                                AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);

                                break;

                            #endregion

                            #region REPORTE DE HORAS DE OPERACIÓN (QUINCENAL)

                            case ConstantesEnviarCorreo.SubcausacodiReporteHO:

                                plantcodi = ConstantesEnviarCorreo.PlantcodiReporteHO;
                                plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                parametroTitulo[0] = Tools.ObtenerNombreMes(entity.Mailfecha.Month) + " " + entity.Mailfecha.Year.ToString() +
                                    " (revisados al 15/" + entity.Mailfecha.Month.ToString("00") + "/" + entity.Mailfecha.Year.ToString() + ")";

                                parametroContenido[0] = String.Format("{0} de {1} de {2}", DateTime.Now.Day.ToString(), Tools.ObtenerNombreMes(DateTime.Now.Month), DateTime.Now.Year.ToString());

                                parametroContenido[1] = String.Format("PRIMERA QUINCENA de {0} de {1} (revisados del 01 al 15/{2}/{3})", Tools.ObtenerNombreMes(entity.Mailfecha.Month), entity.Mailfecha.Year.ToString(), entity.Mailfecha.Month.ToString("00"), entity.Mailfecha.Year.ToString());

                                parametroContenido[2] = String.Format("<a href='"+url+"PostOperacion/Informes/AnalisisEconomicoDespacho?path=Post%20Operaci%C3%B3n%2FInformes%2FAnalisis%20Economico%20del%20Despacho%2F{0}%2F{1}%2F'>Horas de Operación - {2}</a>", entity.Mailfecha.Year.ToString(), (entity.Mailfecha.Month.ToString("00") + "_" + Tools.ObtenerNombreMes(entity.Mailfecha.Month).ToUpper()), Tools.ObtenerNombreMes(entity.Mailfecha.Month));

                                parametroContenido[3] = entity.Mailespecialista.ToUpper();

                                asunto = plantilla.Plantasunto;
                                asunto = String.Format(asunto, parametroTitulo[0]);

                                contenido = plantilla.Plantcontenido;
                                contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                    parametroContenido[2], parametroContenido[3]);

                                AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);

                                break;

                            #endregion

                            #region REPORTE PRELIMINAR DE COSTOS MARGINALES (MENSUAL)

                            case ConstantesEnviarCorreo.SubcausacodiReportePremCMg:

                                plantcodi = ConstantesEnviarCorreo.PlantcodiReportePremCMg;
                                plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);
                                string nombMesConsulta = Tools.ObtenerNombreMes(entity.Mailfecha.Month);
                                string numAnioMesConsulta = entity.Mailfecha.Year.ToString();
                                string numUltimoDiaMesConsulta = Tools.ObtenerUltimoDiaDelMes(entity.Mailfecha).Day.ToString();
                                string numMesConsulta = entity.Mailfecha.Month.ToString("00");

                                parametroTitulo[0] = nombMesConsulta + " " + numAnioMesConsulta +
                                    " (revisados al " + numUltimoDiaMesConsulta + "/" + numMesConsulta + "/" + numAnioMesConsulta + ")";

                                parametroContenido[0] = String.Format("{0} de {1} de {2}", DateTime.Now.Day.ToString(), Tools.ObtenerNombreMes(DateTime.Now.Month), DateTime.Now.Year.ToString());

                                parametroContenido[1] = String.Format("SEGUNDA QUINCENA de {0} de {1} (revisados del 01 al {2}/{3}/{4})", nombMesConsulta, numAnioMesConsulta, numUltimoDiaMesConsulta, numMesConsulta, numAnioMesConsulta);

                                parametroContenido[2] = String.Format("<a href='"+url+"mercadomayorista/costosmarginales/revisados?path=Operaci%C3%B3n%2FCostos%20Marginales%20CP%2FRevisados%2F{0}%2F{1}%2F'>Costos Marginales - {2}</a>", numAnioMesConsulta, (numMesConsulta + "_" + nombMesConsulta.ToUpper()), nombMesConsulta);

                                parametroContenido[3] = entity.Mailespecialista.ToUpper();

                                asunto = plantilla.Plantasunto;
                                asunto = String.Format(asunto, parametroTitulo[0]);

                                contenido = plantilla.Plantcontenido;
                                contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                    parametroContenido[2], parametroContenido[3]);

                                AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);

                                break;

                            #endregion

                            #region REPORTE PRELIMINAR DE HORAS DE OPERACIÓN (MENSUAL)

                            case ConstantesEnviarCorreo.SubcausacodiReportePremHO:

                                plantcodi = ConstantesEnviarCorreo.PlantcodiReportePremHO;
                                plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);
                                string nombMesConsulta_pho = Tools.ObtenerNombreMes(entity.Mailfecha.Month);
                                string numAnioMesConsulta_pho = entity.Mailfecha.Year.ToString();
                                string numUltimoDiaMesConsulta_pho = Tools.ObtenerUltimoDiaDelMes(entity.Mailfecha).Day.ToString();
                                string numMesConsulta_pho = entity.Mailfecha.Month.ToString("00");

                                parametroTitulo[0] = nombMesConsulta_pho + " " + numAnioMesConsulta_pho +
                                    " (revisados al " + numUltimoDiaMesConsulta_pho + "/" + numMesConsulta_pho + "/" + numAnioMesConsulta_pho + ")";

                                parametroContenido[0] = String.Format("{0} de {1} de {2}", DateTime.Now.Day.ToString(), Tools.ObtenerNombreMes(DateTime.Now.Month), DateTime.Now.Year.ToString());

                                parametroContenido[1] = String.Format("{0} de {1} (revisados del 01 al {2}/{3}/{4})", nombMesConsulta_pho, numAnioMesConsulta_pho, numUltimoDiaMesConsulta_pho, numMesConsulta_pho, numAnioMesConsulta_pho);

                                parametroContenido[2] = String.Format("<a href='"+url+"PostOperacion/Informes/AnalisisEconomicoDespacho?path=Post%20Operaci%C3%B3n%2FInformes%2FAnalisis%20Economico%20del%20Despacho%2F{0}%2F{1}%2F'>Horas de Operación - {2}</a>", numAnioMesConsulta_pho, (numMesConsulta_pho + "_" + nombMesConsulta_pho.ToUpper()), nombMesConsulta_pho);

                                parametroContenido[3] = String.Format("05/{0}/{1}", entity.Mailfecha.AddMonths(1).Month.ToString("00"), entity.Mailfecha.AddMonths(1).Year.ToString());

                                parametroContenido[4] = entity.Mailespecialista.ToUpper();

                                asunto = plantilla.Plantasunto;
                                asunto = String.Format(asunto, parametroTitulo[0]);

                                contenido = plantilla.Plantcontenido;
                                contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                    parametroContenido[2], parametroContenido[3], parametroContenido[4]);

                                AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);

                                break;

                            #endregion

                            #region REPORTE FINAL DE COSTOS MARGINALES (MENSUAL)

                            case ConstantesEnviarCorreo.SubcausacodiReporteFinCMg:

                                plantcodi = ConstantesEnviarCorreo.PlantcodiReporteFinCMg;
                                plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);
                                string nombMesConsulta_fcmg = Tools.ObtenerNombreMes(entity.Mailfecha.Month);
                                string numAnioMesConsulta_fcmg = entity.Mailfecha.Year.ToString();
                                string numUltimoDiaMesConsulta_fcmg = Tools.ObtenerUltimoDiaDelMes(entity.Mailfecha).Day.ToString();
                                string numMesConsulta_fcmg = entity.Mailfecha.Month.ToString("00");

                                parametroTitulo[0] = nombMesConsulta_fcmg + " " + numAnioMesConsulta_fcmg;

                                parametroContenido[0] = String.Format("{0} de {1} de {2}", DateTime.Now.Day.ToString(), Tools.ObtenerNombreMes(DateTime.Now.Month), DateTime.Now.Year.ToString());

                                parametroContenido[1] = String.Format("{0} de {1}", nombMesConsulta_fcmg, numAnioMesConsulta_fcmg);

                                parametroContenido[2] = String.Format("<a href='"+url+"mercadomayorista/costosmarginales/revisados?path=Operaci%C3%B3n%2FCostos%20Marginales%20CP%2FRevisados%2F{0}%2F{1}%2F'>Costos Marginales - {2}</a>", numAnioMesConsulta_fcmg, (numMesConsulta_fcmg + "_" + nombMesConsulta_fcmg.ToUpper()), nombMesConsulta_fcmg);

                                parametroContenido[3] = entity.Mailespecialista.ToUpper();

                                asunto = plantilla.Plantasunto;
                                asunto = String.Format(asunto, parametroTitulo[0]);

                                contenido = plantilla.Plantcontenido;
                                contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                    parametroContenido[2], parametroContenido[3]);

                                AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);

                                break;

                            #endregion

                            #region REPORTE FINAL DE HORAS DE OPERACIÓN (MENSUAL)

                            case ConstantesEnviarCorreo.SubcausacodiReporteFinHO:

                                plantcodi = ConstantesEnviarCorreo.PlantcodiReporteFinHO;
                                plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);
                                string nombMesConsulta_fho = Tools.ObtenerNombreMes(entity.Mailfecha.Month);
                                string numAnioMesConsulta_fho = entity.Mailfecha.Year.ToString();
                                string numUltimoDiaMesConsulta_fho = Tools.ObtenerUltimoDiaDelMes(entity.Mailfecha).Day.ToString();
                                string numMesConsulta_fho = entity.Mailfecha.Month.ToString("00");

                                parametroTitulo[0] = nombMesConsulta_fho + " " + numAnioMesConsulta_fho;

                                parametroContenido[0] = String.Format("{0} de {1} de {2}", DateTime.Now.Day.ToString(), Tools.ObtenerNombreMes(DateTime.Now.Month), DateTime.Now.Year.ToString());

                                parametroContenido[1] = String.Format("{0} de {1}", nombMesConsulta_fho, numAnioMesConsulta_fho);

                                parametroContenido[2] = String.Format("<a href='"+url+"PostOperacion/Informes/AnalisisEconomicoDespacho?path=Post%20Operaci%C3%B3n%2FInformes%2FAnalisis%20Economico%20del%20Despacho%2F{0}%2F{1}%2F'>Horas de Operación - {2}</a>", numAnioMesConsulta_fho, (numMesConsulta_fho + "_" + nombMesConsulta_fho.ToUpper()), nombMesConsulta_fho);

                                parametroContenido[3] = entity.Mailespecialista.ToUpper();

                                asunto = plantilla.Plantasunto;
                                asunto = String.Format(asunto, parametroTitulo[0]);

                                contenido = plantilla.Plantcontenido;
                                contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                    parametroContenido[2], parametroContenido[3]);

                                AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);

                                break;

                            #endregion

                            default:
                                break;

                        }

                        break;

                    #endregion


                    #region Informe de Fallas N1


                    case "informefallan1": //Envío de correos
                        /*
                        id: X,A,B
                        X: código de informe de falla
                        A: 1 (ENVIAR), 0 (REENVIAR)
                        B: 0 (IPI), 1 (IP), 2 (IF)
                        */

                        string id = collection["id"];
                        string[] codigoAb = id.Split(',');

                        int eveninfcodi = Convert.ToInt32(codigoAb[0]);
                        int envio = Convert.ToInt32(codigoAb[1]);
                        int tipoInforme = Convert.ToInt32(codigoAb[2]);

                        InformefallaAppServicio servInformeFallas = new InformefallaAppServicio();
                        EquipamientoAppServicio servEquipamiento = new EquipamientoAppServicio();
                        EveInformefallaDTO entityIF = servInformeFallas.GetByIdEveInformefalla(eveninfcodi);

                        switch (tipoInforme)
                        {
                            #region IPI: Informe Preliminar Inicial

                            case 0: //Informe Preliminar Inicial
                                //envio
                                if (envio == 1)
                                {
                                    plantcodi = ConstantesEnviarCorreo.PlantcodiInformePrelimInicialEnvioN1;
                                    plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                    ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                    parametroTitulo[0] = Convert.ToInt32(entityIF.Evencorr).ToString("000") + "-IPI-" +
                                                         entityIF.Evenini.ToString("yyyy");

                                    asunto = plantilla.Plantasunto;
                                    asunto = String.Format(asunto, parametroTitulo[0]);

                                    parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                            Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                            DateTime.Now.Year.ToString();
                                    parametroContenido[1] = asunto;
                                    parametroContenido[2] = entityIF.Equiabrev + " de la " + entityIF.Tareaabrev + " " +
                                                            entityIF.Areanomb;
                                    parametroContenido[3] =
                                        Tools.ObtenerNombreDia(entityIF.Evenini.DayOfWeek).ToLower() + " " +
                                        entityIF.Evenini.Day + " de " + Tools.ObtenerNombreMes(entityIF.Evenini.Month) +
                                        " de " + entityIF.Evenini.Year + " a las " +
                                        entityIF.Evenini.Hour.ToString("00") + ":" +
                                        entityIF.Evenini.Minute.ToString("00") + " horas";
                                    parametroContenido[4] = entityIF.Eveninfpiemit;
                                    parametroContenido[5] = persona.GetCargo(entityIF.Eveninfpiemit);
                                    parametroContenido[6] = persona.GetArea(entityIF.Eveninfpiemit);
                                    parametroContenido[7] = persona.GetTelefono(entityIF.Eveninfpiemit);

                                    contenido = plantilla.Plantcontenido;
                                    contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                        parametroContenido[2], parametroContenido[3], parametroContenido[4],
                                        parametroContenido[5], parametroContenido[6], parametroContenido[7]);

                                    AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                }
                                else
                                {
                                    if (envio != 0)
                                        break;

                                    plantcodi = ConstantesEnviarCorreo.PlantcodiInformePrelimInicialReenvioN1;
                                    plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                    ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                    parametroTitulo[0] = Convert.ToInt32(entityIF.Evencorr).ToString("000") + "-IPI-" +
                                                         entityIF.Evenini.ToString("yyyy");

                                    asunto = plantilla.Plantasunto;
                                    asunto = String.Format(asunto, parametroTitulo[0]);

                                    parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                            Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                            DateTime.Now.Year.ToString();
                                    parametroContenido[1] = asunto;
                                    parametroContenido[2] = entityIF.Equiabrev + " de la " + entityIF.Tareaabrev + " " +
                                                            entityIF.Areanomb;
                                    parametroContenido[3] =
                                        Tools.ObtenerNombreDia(entityIF.Evenini.DayOfWeek).ToLower() + " " +
                                        entityIF.Evenini.Day + " de " + Tools.ObtenerNombreMes(entityIF.Evenini.Month) +
                                        " de " + entityIF.Evenini.Year + " a las " +
                                        entityIF.Evenini.Hour.ToString("00") + ":" +
                                        entityIF.Evenini.Minute.ToString("00") + " horas";
                                    parametroContenido[4] = entityIF.Eveninfpiemit;
                                    parametroContenido[5] = persona.GetCargo(entityIF.Eveninfpiemit);
                                    parametroContenido[6] = persona.GetArea(entityIF.Eveninfpiemit);
                                    parametroContenido[7] = persona.GetTelefono(entityIF.Eveninfpiemit);

                                    contenido = plantilla.Plantcontenido;
                                    contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                        parametroContenido[2], parametroContenido[3], parametroContenido[4],
                                        parametroContenido[5], parametroContenido[6], parametroContenido[7]);

                                    AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                }

                                break;

                            #endregion

                            #region IP: Informe Preliminar

                            case 1: //Informe Preliminar
                                //envio
                                if (envio == 1)
                                {
                                    plantcodi = ConstantesEnviarCorreo.PlantcodiInformePrelimEnvioN1;
                                    plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                    ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                    parametroTitulo[0] = Convert.ToInt32(entityIF.Evencorr).ToString("000") + "-IP-" +
                                                         entityIF.Evenini.ToString("yyyy");

                                    asunto = plantilla.Plantasunto;
                                    asunto = String.Format(asunto, parametroTitulo[0]);

                                    parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                            Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                            DateTime.Now.Year.ToString();
                                    parametroContenido[1] = asunto;
                                    parametroContenido[2] = entityIF.Equiabrev + " de la " + entityIF.Tareaabrev + " " +
                                                            entityIF.Areanomb;
                                    parametroContenido[3] =
                                        Tools.ObtenerNombreDia(entityIF.Evenini.DayOfWeek).ToLower() + " " +
                                        entityIF.Evenini.Day + " de " + Tools.ObtenerNombreMes(entityIF.Evenini.Month) +
                                        " de " + entityIF.Evenini.Year + " a las " +
                                        entityIF.Evenini.Hour.ToString("00") + ":" +
                                        entityIF.Evenini.Minute.ToString("00") + " horas";
                                    parametroContenido[4] = entityIF.Eveninfpemit;
                                    parametroContenido[5] = persona.GetCargo(entityIF.Eveninfpemit);
                                    parametroContenido[6] = persona.GetArea(entityIF.Eveninfpemit);
                                    parametroContenido[7] = persona.GetTelefono(entityIF.Eveninfpemit);

                                    contenido = plantilla.Plantcontenido;
                                    contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                        parametroContenido[2], parametroContenido[3], parametroContenido[4],
                                        parametroContenido[5], parametroContenido[6], parametroContenido[7]);

                                    AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);

                                }
                                else
                                {
                                    if (envio != 0)
                                        break;

                                    plantcodi = ConstantesEnviarCorreo.PlantcodiInformePrelimReenvioN1;
                                    plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                    ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                    parametroTitulo[0] = Convert.ToInt32(entityIF.Evencorr).ToString("000") + "-IP-" +
                                                         entityIF.Evenini.ToString("yyyy");

                                    asunto = plantilla.Plantasunto;
                                    asunto = String.Format(asunto, parametroTitulo[0]);

                                    parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                            Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                            DateTime.Now.Year.ToString();
                                    parametroContenido[1] = asunto;
                                    parametroContenido[2] = entityIF.Equiabrev + " de la " + entityIF.Tareaabrev + " " +
                                                            entityIF.Areanomb;
                                    parametroContenido[3] =
                                        Tools.ObtenerNombreDia(entityIF.Evenini.DayOfWeek).ToLower() + " " +
                                        entityIF.Evenini.Day + " de " + Tools.ObtenerNombreMes(entityIF.Evenini.Month) +
                                        " de " + entityIF.Evenini.Year + " a las " +
                                        entityIF.Evenini.Hour.ToString("00") + ":" +
                                        entityIF.Evenini.Minute.ToString("00") + " horas";
                                    parametroContenido[4] = entityIF.Eveninfemit;
                                    parametroContenido[5] = persona.GetCargo(entityIF.Eveninfemit);
                                    parametroContenido[6] = persona.GetArea(entityIF.Eveninfemit);
                                    parametroContenido[7] = persona.GetTelefono(entityIF.Eveninfemit);

                                    contenido = plantilla.Plantcontenido;
                                    contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                        parametroContenido[2], parametroContenido[3], parametroContenido[4],
                                        parametroContenido[5], parametroContenido[6], parametroContenido[7]);

                                    AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);

                                }

                                break;

                            #endregion

                            #region IF: Informe Final

                            case 2: //Informe Preliminar
                                //envio
                                if (envio == 1)
                                {
                                    plantcodi = ConstantesEnviarCorreo.PlantcodiInformeFinalEnvioN1;
                                    plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                    ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                    parametroTitulo[0] = Convert.ToInt32(entityIF.Evencorr).ToString("000") + "-IF-" +
                                                         entityIF.Evenini.ToString("yyyy");

                                    asunto = plantilla.Plantasunto;
                                    asunto = String.Format(asunto, parametroTitulo[0]);

                                    parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                            Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                            DateTime.Now.Year.ToString();
                                    parametroContenido[1] = asunto;
                                    parametroContenido[2] = entityIF.Equiabrev + " de la " + entityIF.Tareaabrev + " " +
                                                            entityIF.Areanomb;
                                    parametroContenido[3] =
                                        Tools.ObtenerNombreDia(entityIF.Evenini.DayOfWeek).ToLower() + " " +
                                        entityIF.Evenini.Day + " de " + Tools.ObtenerNombreMes(entityIF.Evenini.Month) +
                                        " de " + entityIF.Evenini.Year + " a las " +
                                        entityIF.Evenini.Hour.ToString("00") + ":" +
                                        entityIF.Evenini.Minute.ToString("00") + " horas.";
                                    parametroContenido[4] = entityIF.Eveninfemitido;
                                    parametroContenido[5] = persona.GetCargo(entityIF.Eveninfemitido);
                                    parametroContenido[6] = persona.GetArea(entityIF.Eveninfemitido);
                                    parametroContenido[7] = persona.GetTelefono(entityIF.Eveninfemitido);

                                    contenido = plantilla.Plantcontenido;
                                    contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                        parametroContenido[2], parametroContenido[3], parametroContenido[4],
                                        parametroContenido[5], parametroContenido[6], parametroContenido[7]);

                                    AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                }
                                else
                                {
                                    if (envio != 0)
                                        break;

                                    plantcodi = ConstantesEnviarCorreo.PlantcodiInformeFinalReenvioN1;
                                    plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                    ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                    parametroTitulo[0] = Convert.ToInt32(entityIF.Evencorr).ToString("000") + "-IF-" +
                                                         entityIF.Evenini.ToString("yyyy");

                                    asunto = plantilla.Plantasunto;
                                    asunto = String.Format(asunto, parametroTitulo[0]);

                                    parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                            Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                            DateTime.Now.Year.ToString();
                                    parametroContenido[1] = asunto;
                                    parametroContenido[2] = entityIF.Equiabrev + " de la " + entityIF.Tareaabrev + " " +
                                                            entityIF.Areanomb;
                                    parametroContenido[3] =
                                        Tools.ObtenerNombreDia(entityIF.Evenini.DayOfWeek).ToLower() + " " +
                                        entityIF.Evenini.Day + " de " + Tools.ObtenerNombreMes(entityIF.Evenini.Month) +
                                        " de " + entityIF.Evenini.Year + " a las " +
                                        entityIF.Evenini.Hour.ToString("00") + ":" +
                                        entityIF.Evenini.Minute.ToString("00") + " horas.";
                                    parametroContenido[4] = entityIF.Eveninfemit;
                                    parametroContenido[5] = persona.GetCargo(entityIF.Eveninfemit);
                                    parametroContenido[6] = persona.GetArea(entityIF.Eveninfemit);
                                    parametroContenido[7] = persona.GetTelefono(entityIF.Eveninfemit);

                                    contenido = plantilla.Plantcontenido;
                                    contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                        parametroContenido[2], parametroContenido[3], parametroContenido[4],
                                        parametroContenido[5], parametroContenido[6], parametroContenido[7]);

                                    AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                }

                                break;

                            #endregion

                            default:
                                break;

                        }

                        break;

                    #endregion



                    #region Informe de Fallas N2


                    case "informefallan2": //Envío de correos
                        /*
                        id: X,A,B
                        X: código de informe de falla
                        A: 1 (ENVIAR), 0 (REENVIAR)
                        B: 0 (IPI sin Inf. Empresa), 1 (IPI), 2 (IF sin Inf. Empresa),  3 (IF)
                        */

                        id = collection["id"];
                        codigoAb = id.Split(',');

                        eveninfcodi = Convert.ToInt32(codigoAb[0]);
                        envio = Convert.ToInt32(codigoAb[1]);
                        tipoInforme = Convert.ToInt32(codigoAb[2]);

                        InformefallaN2AppServicio servInformeFallasN2 = new InformefallaN2AppServicio();

                        EveInformefallaN2DTO entityIFN2 = servInformeFallasN2.GetByIdEveInformefallaN2(eveninfcodi);

                        switch (tipoInforme)
                        {
                            #region IPI sin informe de empresa: Informe Preliminar Inicial

                            case 0: //Informe Preliminar Inicial sin informe de empresa
                                //envio
                                if (envio == 1 || envio == 0)
                                {
                                    plantcodi = ConstantesEnviarCorreo.PlantcodiInformePrelimInicialSinInformeN2;
                                    plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                    ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                    parametroTitulo[0] = Convert.ToInt32(entityIFN2.Evenn2corr).ToString("000") +
                                                         "-IPI-" + entityIFN2.Evenini.ToString("yyyy");

                                    asunto = plantilla.Plantasunto;
                                    asunto = String.Format(asunto, parametroTitulo[0]);

                                    parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                            Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                            DateTime.Now.Year.ToString();
                                    parametroContenido[1] = asunto;
                                    parametroContenido[2] = entityIFN2.Equiabrev + " de la " + entityIFN2.Tareaabrev +
                                                            " " + entityIFN2.Areanomb;
                                    parametroContenido[3] =
                                        Tools.ObtenerNombreDia(entityIFN2.Evenini.DayOfWeek).ToLower() + " " +
                                        entityIFN2.Evenini.Day + " de " +
                                        Tools.ObtenerNombreMes(entityIFN2.Evenini.Month) + " de " +
                                        entityIFN2.Evenini.Year + " a las " + entityIFN2.Evenini.Hour.ToString("00") +
                                        ":" + entityIFN2.Evenini.Minute.ToString("00");
                                    parametroContenido[4] = entityIFN2.Emprnomb;
                                    parametroContenido[5] = entityIFN2.EvenipiEN2elab;
                                    parametroContenido[6] = persona.GetCargo(entityIFN2.EvenipiEN2elab);
                                    parametroContenido[7] = persona.GetArea(entityIFN2.EvenipiEN2elab);
                                    parametroContenido[8] = persona.GetTelefono(entityIFN2.EvenipiEN2elab);

                                    contenido = plantilla.Plantcontenido;
                                    contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                        parametroContenido[2], parametroContenido[3], parametroContenido[4],
                                        parametroContenido[5], parametroContenido[6], parametroContenido[7],
                                        parametroContenido[8]);

                                    AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                }
                                else
                                {
                                    if (envio != 0)
                                        break;

                                }

                                break;

                            #endregion

                            #region IPI: Informe Preliminar Inicial (informe)

                            case 1: //Informe Preliminar

                                //envio
                                if (envio == 1 || envio == 0)
                                {
                                    plantcodi = ConstantesEnviarCorreo.PlantcodiInformePrelimInicialN2;
                                    plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                    ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                    parametroTitulo[0] = Convert.ToInt32(entityIFN2.Evenn2corr).ToString("000") +
                                                         "-IPI-" + entityIFN2.Evenini.ToString("yyyy");

                                    asunto = plantilla.Plantasunto;
                                    asunto = String.Format(asunto, parametroTitulo[0]);

                                    parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                            Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                            DateTime.Now.Year.ToString();
                                    parametroContenido[1] = asunto;
                                    parametroContenido[2] = entityIFN2.Equiabrev + " de la " + entityIFN2.Tareaabrev +
                                                            " " + entityIFN2.Areanomb;
                                    parametroContenido[3] =
                                        Tools.ObtenerNombreDia(entityIFN2.Evenini.DayOfWeek).ToLower() + " " +
                                        entityIFN2.Evenini.Day + " de " +
                                        Tools.ObtenerNombreMes(entityIFN2.Evenini.Month) + " de " +
                                        entityIFN2.Evenini.Year + " a las " + entityIFN2.Evenini.Hour.ToString("00") +
                                        ":" + entityIFN2.Evenini.Minute.ToString("00") + " horas";

                                    if ((entityIFN2.EvenipiEN2emitido == null) || (entityIFN2.EvenipiEN2emitido == "N"))
                                        parametroContenido[4] = "";
                                    else
                                        parametroContenido[4] =
                                            "Cabe señalar, que este informe se emite a falta del respectivo informe preliminar inicial de parte de la empresa " +
                                            entityIFN2.Emprnomb + ".<br/><br/>";

                                    parametroContenido[5] = entityIFN2.Eveninfpin2elab;
                                    parametroContenido[6] = persona.GetCargo(entityIFN2.Eveninfpin2elab);
                                    parametroContenido[7] = persona.GetArea(entityIFN2.Eveninfpin2elab);
                                    parametroContenido[8] = persona.GetTelefono(entityIFN2.Eveninfpin2elab);

                                    contenido = plantilla.Plantcontenido;
                                    contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                                        parametroContenido[2], parametroContenido[3], parametroContenido[4],
                                        parametroContenido[5], parametroContenido[6], parametroContenido[7],
                                        parametroContenido[8]);

                                    AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                }
                                else
                                {
                                    if (envio != 0)
                                        break;

                                }

                                break;

                            #endregion

                            #region IF: Informe Final sin informe de empresa

                            case 2: //Informe Final sin informe de empresa

                                //envio
                                if (envio == 1 || envio == 0)
                                {

                                    if (entityIFN2.EvenipiEN2emitido == null || entityIFN2.EvenipiEN2emitido == "N")
                                    {
                                        plantcodi = ConstantesEnviarCorreo.PlantcodiInformeFinalSinInformeEnvioN2;
                                        plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                        ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                        parametroTitulo[0] = entityIFN2.Emprnomb + " SCO N2-" +
                                                             Convert.ToInt32(entityIFN2.Evenn2corr).ToString("000") +
                                                             "-IF-" + entityIFN2.Evenini.ToString("yyyy");

                                        asunto = plantilla.Plantasunto;
                                        asunto = String.Format(asunto, parametroTitulo[0]);

                                        parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                                Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                                DateTime.Now.Year.ToString();
                                        parametroContenido[1] = asunto;
                                        parametroContenido[2] = entityIFN2.Equiabrev + " de la " + entityIFN2.Tareaabrev +
                                                                " " + entityIFN2.Areanomb;
                                        parametroContenido[3] =
                                            Tools.ObtenerNombreDia(entityIFN2.Evenini.DayOfWeek).ToLower() + " " +
                                            entityIFN2.Evenini.Day + " de " +
                                            Tools.ObtenerNombreMes(entityIFN2.Evenini.Month) + " de " +
                                            entityIFN2.Evenini.Year + " a las " + entityIFN2.Evenini.Hour.ToString("00") +
                                            ":" + entityIFN2.Evenini.Minute.ToString("00");
                                        parametroContenido[4] = entityIFN2.Emprnomb;
                                        parametroContenido[5] = entityIFN2.EvenifEN2elab;
                                        parametroContenido[6] = persona.GetCargo(entityIFN2.EvenifEN2elab);
                                        parametroContenido[7] = persona.GetArea(entityIFN2.EvenifEN2elab);
                                        parametroContenido[8] = persona.GetTelefono(entityIFN2.EvenifEN2elab);

                                        contenido = plantilla.Plantcontenido;
                                        contenido = String.Format(contenido, parametroContenido[0],
                                            parametroContenido[1], parametroContenido[2], parametroContenido[3],
                                            parametroContenido[4], parametroContenido[5], parametroContenido[6],
                                            parametroContenido[7], parametroContenido[8]);

                                        AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);

                                    }
                                    else
                                    {
                                        //plantilla 42
                                        plantcodi = ConstantesEnviarCorreo.PlantcodiInformeFinalSinInformeReenvioN2;
                                        plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                        ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                        parametroTitulo[0] = Convert.ToInt32(entityIFN2.Evenn2corr).ToString("000") +
                                                             "-IF-" + entityIFN2.Evenini.ToString("yyyy");

                                        asunto = plantilla.Plantasunto;
                                        asunto = String.Format(asunto, parametroTitulo[0]);

                                        parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                                Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                                DateTime.Now.Year.ToString();
                                        parametroContenido[1] = Convert.ToInt32(entityIFN2.Evenn2corr).ToString("000") +
                                                                "-IPI-" + entityIFN2.Evenini.ToString("yyyy");
                                        parametroContenido[2] = entityIFN2.Equiabrev + " de la " + entityIFN2.Tareaabrev +
                                                                " " + entityIFN2.Areanomb;
                                        parametroContenido[3] =
                                            Tools.ObtenerNombreDia(entityIFN2.Evenini.DayOfWeek).ToLower() + " " +
                                            entityIFN2.Evenini.Day + " de " +
                                            Tools.ObtenerNombreMes(entityIFN2.Evenini.Month) + " de " +
                                            entityIFN2.Evenini.Year + " a las " + entityIFN2.Evenini.Hour.ToString("00") +
                                            ":" + entityIFN2.Evenini.Minute.ToString("00");
                                        parametroContenido[4] = entityIFN2.Emprnomb;
                                        parametroContenido[5] = entityIFN2.EvenifEN2elab;
                                        parametroContenido[6] = persona.GetCargo(entityIFN2.EvenifEN2elab);
                                        parametroContenido[7] = persona.GetArea(entityIFN2.EvenifEN2elab);
                                        parametroContenido[8] = persona.GetTelefono(entityIFN2.EvenifEN2elab);

                                        contenido = plantilla.Plantcontenido;
                                        contenido = String.Format(contenido, parametroContenido[0],
                                            parametroContenido[1], parametroContenido[2], parametroContenido[3],
                                            parametroContenido[4], parametroContenido[5], parametroContenido[6],
                                            parametroContenido[7], parametroContenido[8]);

                                        AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);

                                    }

                                }
                                else
                                {

                                }

                                break;

                            #endregion


                            #region IF: Informe Final  - informe

                            case 3: //Informe Final -informe


                                //envio
                                if (envio == 1 || envio == 0)
                                {
                                    if (entityIFN2.EvenifEN2emitido == null || entityIFN2.EvenifEN2emitido == "N")
                                    {
                                        //plantilla 43

                                        plantcodi = ConstantesEnviarCorreo.PlantcodiInfFallaFinalNoEmitidoN2;
                                        plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                        ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                        parametroTitulo[0] = Convert.ToInt32(entityIFN2.Evenn2corr).ToString("000") +
                                                             "-IF-" + entityIFN2.Evenini.ToString("yyyy");

                                        asunto = plantilla.Plantasunto;
                                        asunto = String.Format(asunto, parametroTitulo[0]);

                                        parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                                Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                                DateTime.Now.Year.ToString();
                                        parametroContenido[1] = asunto;
                                        parametroContenido[2] = entityIFN2.Equiabrev + " de la " + entityIFN2.Tareaabrev +
                                                                " " + entityIFN2.Areanomb;
                                        parametroContenido[3] =
                                            Tools.ObtenerNombreDia(entityIFN2.Evenini.DayOfWeek).ToLower() + " " +
                                            entityIFN2.Evenini.Day + " de " +
                                            Tools.ObtenerNombreMes(entityIFN2.Evenini.Month) + " de " +
                                            entityIFN2.Evenini.Year + " a las " + entityIFN2.Evenini.Hour.ToString("00") +
                                            ":" + entityIFN2.Evenini.Minute.ToString("00");
                                        parametroContenido[4] = entityIFN2.Eveninffn2elab;
                                        parametroContenido[5] = persona.GetCargo(entityIFN2.Eveninffn2elab);
                                        parametroContenido[6] = persona.GetArea(entityIFN2.Eveninffn2elab);
                                        parametroContenido[7] = persona.GetTelefono(entityIFN2.Eveninffn2elab);

                                        contenido = plantilla.Plantcontenido;
                                        contenido = String.Format(contenido, parametroContenido[0],
                                            parametroContenido[1], parametroContenido[2], parametroContenido[3],
                                            parametroContenido[4], parametroContenido[5], parametroContenido[6],
                                            parametroContenido[7]);

                                        AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                                    }
                                    else
                                    {


                                        if (entityIFN2.EvenipiEN2emitido == null || entityIFN2.EvenipiEN2emitido == "N")
                                        {
                                            //plantilla 44
                                            plantcodi = ConstantesEnviarCorreo.PlantcodiInfFallasFinalSPrelimInicialN2;
                                            plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                            ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                            parametroTitulo[0] = entityIFN2.Emprnomb + " SCO N2-" +
                                                                 Convert.ToInt32(entityIFN2.Evenn2corr).ToString("000") +
                                                                 "-IF-" + entityIFN2.Evenini.ToString("yyyy");
                                            asunto = plantilla.Plantasunto;
                                            asunto = String.Format(asunto, parametroTitulo[0]);

                                            parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                                    Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                                    DateTime.Now.Year.ToString();
                                            parametroContenido[1] = (envio == 1 ? "" : "re");
                                            parametroContenido[2] = entityIFN2.Emprnomb;
                                            parametroContenido[3] =
                                                Convert.ToInt32(entityIFN2.Evenn2corr).ToString("000") + "-IF-" +
                                                entityIFN2.Evenini.Year.ToString();
                                            parametroContenido[4] = entityIFN2.Equiabrev + " de la " +
                                                                    entityIFN2.Tareaabrev + " " + entityIFN2.Areanomb;
                                            parametroContenido[5] =
                                                Tools.ObtenerNombreDia(entityIFN2.Evenini.DayOfWeek).ToLower() + " " +
                                                entityIFN2.Evenini.Day + " de " +
                                                Tools.ObtenerNombreMes(entityIFN2.Evenini.Month) + " de " +
                                                entityIFN2.Evenini.Year + " a las " +
                                                entityIFN2.Evenini.Hour.ToString("00") + ":" +
                                                entityIFN2.Evenini.Minute.ToString("00");
                                            parametroContenido[6] = entityIFN2.Emprnomb;
                                            parametroContenido[7] = entityIFN2.Eveninffn2elab;
                                            parametroContenido[8] = persona.GetCargo(entityIFN2.Eveninffn2elab);
                                            parametroContenido[9] = persona.GetArea(entityIFN2.Eveninffn2elab);
                                            parametroContenido[10] = persona.GetTelefono(entityIFN2.Eveninffn2elab);

                                            contenido = plantilla.Plantcontenido;
                                            contenido = String.Format(contenido, parametroContenido[0],
                                                parametroContenido[1], parametroContenido[2], parametroContenido[3],
                                                parametroContenido[4], parametroContenido[5], parametroContenido[6],
                                                parametroContenido[7], parametroContenido[8], parametroContenido[9],
                                                parametroContenido[10]);

                                            AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);

                                        }
                                        else
                                        {
                                            //plantilla 45
                                            plantcodi = ConstantesEnviarCorreo.PlantcodiInfFallasFinalSinPrelimInicNiFinalEmp;
                                            plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                                            ObtenerCampoCorreo(plantilla.PlanticorreoFrom, out from,
                                                plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                                plantilla.PlanticorreosBcc, out bcc);

                                            parametroTitulo[0] =
                                                Convert.ToInt32(entityIFN2.Evenn2corr).ToString("000") + "-IF-" +
                                                entityIFN2.Evenini.ToString("yyyy");

                                            asunto = plantilla.Plantasunto;
                                            asunto = String.Format(asunto, parametroTitulo[0]);

                                            parametroContenido[0] = DateTime.Now.Day.ToString() + " de " +
                                                                    Tools.ObtenerNombreMes(DateTime.Now.Month) + " de " +
                                                                    DateTime.Now.Year.ToString();
                                            parametroContenido[1] = asunto;
                                            parametroContenido[2] = entityIFN2.Equiabrev + " de la " +
                                                                    entityIFN2.Tareaabrev + " " + entityIFN2.Areanomb;
                                            parametroContenido[3] =
                                                Tools.ObtenerNombreDia(entityIFN2.Evenini.DayOfWeek).ToLower() + " " +
                                                entityIFN2.Evenini.Day + " de " +
                                                Tools.ObtenerNombreMes(entityIFN2.Evenini.Month) + " de " +
                                                entityIFN2.Evenini.Year + " a las " +
                                                entityIFN2.Evenini.Hour.ToString("00") + ":" +
                                                entityIFN2.Evenini.Minute.ToString("00");
                                            parametroContenido[4] = entityIFN2.Emprnomb;
                                            parametroContenido[5] = entityIFN2.Eveninffn2elab;
                                            parametroContenido[6] = persona.GetCargo(entityIFN2.Eveninffn2elab);
                                            parametroContenido[7] = persona.GetArea(entityIFN2.Eveninffn2elab);
                                            parametroContenido[8] = persona.GetTelefono(entityIFN2.Eveninffn2elab);

                                            contenido = plantilla.Plantcontenido;
                                            contenido = String.Format(contenido, parametroContenido[0],
                                                parametroContenido[1], parametroContenido[2], parametroContenido[3],
                                                parametroContenido[4], parametroContenido[5], parametroContenido[6],
                                                parametroContenido[7], parametroContenido[8]);

                                            AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);

                                        }

                                    }

                                }
                                else
                                {


                                }

                                break;

                            #endregion


                            default:
                                break;

                        }

                        break;

                        #endregion

                }



            }


            return PartialView(model);
        }

        private void ObtenerCampoCorreo(string fromValor, out string fromCampo, string toValor, out string toCampo,
            string ccValor, out string ccCampo, string bccValor, out string bccCampo)
        {

            fromCampo = (fromValor != null ? fromValor : "");
            toCampo = (toValor != null ? toValor : "");
            ccCampo = (ccValor != null ? ccValor : "");
            bccCampo = (bccValor != null ? bccValor : "");
        }

        /// <summary>
        /// Permite completar un formato de correo según los parámetros especificados
        /// </summary>
        /// <param name="from">De</param>
        /// <param name="to">Destinatario</param>
        /// <param name="cc">Con copia</param>
        /// <param name="bcc">Con copia oculta</param>
        /// <param name="asunto">Asunto</param>
        /// <param name="contenido">Contenido</param>
        /// <returns></returns>
        public PartialViewResult FormatoCorreo(string from, string to, string cc, string bcc, string asunto,
            string contenido)
        {
            FormatoCorreoModel model = new FormatoCorreoModel();
            model.From = from;
            model.To = to;
            model.CC = cc;
            model.BCC = bcc;
            model.Asunto = asunto;
            model.Contenido = contenido;
            return PartialView(model);

        }

        /// <summary>
        /// Permite enviar un formato de correo actualizado
        /// </summary>
        /// <param name="model">Formato de correo</param>
        /// <returns>1: envío satisfactorio. -1: error</returns>
        public int EnviarCorreo(FormatoCorreoModel model)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEnviarCorreos.RutaCorreo;
                List<string> listFiles = new List<string>();
                string files = model.Archivo;

                if (!string.IsNullOrEmpty(files))
                    listFiles = files.Split('/').ToList();

                CorreoAppServicio servCorreo = new CorreoAppServicio();
                servCorreo.EnviarCorreo(model.From, model.To, model.CC, model.BCC, model.Asunto, model.Contenido,
                    model.Plantcodi, path, listFiles);

                return 1;
            }
            catch (Exception ex)
            {
                log.Error("EnviarCorreos", ex);

                return -1;
            }
        }

        /// <summary>
        /// Permite cargar un archivo
        /// </summary>
        /// <param name="nombreArchivo">Nombre del archivo</param>
        /// <returns>Resultado de carga de archivo</returns>
        [HttpPost]
        public ActionResult Upload(int chunks, int chunk, string name)
        {
            //try
            //{
            //    string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEnviarCorreos.RutaCorreo;

            //    if (Request.Files.Count == 1)
            //    {
            //        var file = Request.Files[0];
            //        string fileName = path + nombreArchivo;
            //        file.SaveAs(fileName);

            //    }
            //    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            //}
            //catch
            //{
            //    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            //}

            try
            {
                if (Request.Files.Count > 0)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEnviarCorreos.RutaCorreo;

                    if (chunks > 1)
                    {
                        if (chunk == 1)
                        {
                            if (System.IO.File.Exists(path + name))
                                System.IO.File.Delete(path + name);
                        }

                        var file = Request.Files[0];
                        using (var fs = new FileStream(Path.Combine(path, name), chunk == 0 ? FileMode.Create : FileMode.Append))
                        {
                            var buffer = new byte[file.InputStream.Length];
                            file.InputStream.Read(buffer, 0, buffer.Length);
                            fs.Write(buffer, 0, buffer.Length);
                        }

                        if (chunk == chunks - 1)
                        {

                        }
                    }
                    else
                    {
                        var file = Request.Files[0];
                        if (System.IO.File.Exists(path + name))
                            System.IO.File.Delete(path + name);
                        file.SaveAs(path + name);
                    }
                }
                return Json(new { success = true, indicador = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Permite mostrar una carpeta
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <param name="tipoplantilla">Tipo de plantilla</param>
        /// <returns></returns>
        public ActionResult VerCarpeta(string id, string tipoplantilla)
        {
            FormatoCorreoModel model = new FormatoCorreoModel();
            model.ruta = "";
            model.pathAlternativo = Constantes.FileSystemSco;
            string ruta = "";

            if (tipoplantilla != "0")
            {
                var plantilla = new SiPlantillacorreoDTO();
                string[] parametroContenido = new string[10];
                string[] parametroTitulo = new string[10];

                int eveninfcodi;
                InformefallaAppServicio servInformeFallas;
                string rutaRelativa;


                switch (tipoplantilla)
                {

                    #region Informe de Fallas N1

                    case "informefallan1": //Informe de falla N1
                        //Ej: ruta base \\fs\Areas\SCO\InformedePerturbaciones
                        eveninfcodi = Convert.ToInt32(id);
                        servInformeFallas = new InformefallaAppServicio();
                        EveInformefallaDTO entityIF = servInformeFallas.GetByIdEveInformefalla(eveninfcodi);

                        //rutarelativa
                        rutaRelativa = entityIF.Evenini.Year + @"/Sem" +
                                       Tools.ObtenerNroSemanaAnio(entityIF.Evenini).ToString("00")
                                       + entityIF.Evenini.Year.ToString().Substring(2, 2) + @"/" +
                                       entityIF.Evenini.Day.ToString("00") +
                                       entityIF.Evenini.Month.ToString("00") + @"/E" + entityIF.Evencorr;

                        //ruta = @"\\fs\Areas\SCO\InformedePerturbaciones\" + rutaRelativa;
                        ruta =// @"\" + 
                            ConstantesEnviarCorreo.CarpetaInformeFallaN1 + @"/" + rutaRelativa;
                        model.ruta = ruta;


                        break;

                    case "informefallan1ministerio": //Informe de falla N1 Ministerio
                        //ruta base \\fs\Areas\SCO\InformedePerturbaciones

                        eveninfcodi = Convert.ToInt32(id);

                        servInformeFallas = new InformefallaAppServicio();
                        EveInformefallaDTO entityIFMin = servInformeFallas.GetByIdEveInformefalla(eveninfcodi);

                        //rutarelativa
                        rutaRelativa = entityIFMin.Evenini.Year + @"/" + ConstantesEnviarCorreo.CarpetaInformeMinisterio +
                                       @"/Sem" + Tools.ObtenerNroSemanaAnio(entityIFMin.Evenini).ToString("00") +
                                       entityIFMin.Evenini.Year.ToString().Substring(2, 2) + @"/" +
                                       entityIFMin.Evenini.Day.ToString("00") + entityIFMin.Evenini.Month.ToString("00");
                        //ruta = @"\InformedeFallas\N1\InformedePerturbaciones\" + rutaRelativa;
                        ruta = //@"\" + 
                            ConstantesEnviarCorreo.CarpetaInformeFallaN1 + @"/" + rutaRelativa;
                        model.ruta = ruta;
                        break;


                    case "informefallan2": //Informe de falla N2
                        //ruta base \\fs\Areas\SCO\InformedePerturbacionesN2

                        eveninfcodi = Convert.ToInt32(id);

                        servInformeFallasN2 = new InformefallaN2AppServicio();
                        EveInformefallaN2DTO entityIFN2 = servInformeFallasN2.GetByIdEveInformefallaN2(eveninfcodi);

                        //rutarelativa
                        rutaRelativa = entityIFN2.Evenini.Year + @"/Sem" +
                                       Tools.ObtenerNroSemanaAnio(entityIFN2.Evenini).ToString("00")
                                       + entityIFN2.Evenini.Year.ToString().Substring(2, 2) + @"/" +
                                       entityIFN2.Evenini.Day.ToString("00") +
                                       entityIFN2.Evenini.Month.ToString("00") + @"/E" + entityIFN2.Evenn2corr;
                        ruta = //@"\" + 
                            ConstantesEnviarCorreo.CarpetaInformeFallaN2 + @"/" + rutaRelativa;
                        model.ruta = ruta;
                        break;
                        #endregion
                }

            }

            return View(model);

        }
    }
}
