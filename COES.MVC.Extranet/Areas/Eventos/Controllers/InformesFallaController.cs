using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Extranet.Areas.Eventos.Models;
using COES.MVC.Extranet.Areas.Eventos.Helper;
using COES.Servicios.Aplicacion.Evento;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.Eventos.Helper;
using System.Globalization;
using System.Reflection;
using log4net;
using COES.MVC.Extranet.Controllers;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Eventos;
using System.Text.RegularExpressions;
using COES.Servicios.Aplicacion.Correo;
using System.IO;
using System.Web.Hosting;

namespace COES.MVC.Extranet.Areas.Eventos.Controllers
{
    public class InformesFallaController : BaseController
    {
        EventosAppServicio servicioEvento = new EventosAppServicio();
        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        AnalisisFallasAppServicio servAF = new AnalisisFallasAppServicio();
        SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        public InformesFallaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        /// <summary>
        /// Excepciones ocurridas en el controlador
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        // GET: Eventos/InformesFalla
        public ActionResult Index()
        {
            ViewBag.FechaInicio = DateTime.Now.AddDays(-7).ToString(Constantes.FormatoFecha);
            ViewBag.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View();
        }
        /// <summary>
        /// Listar las interrupciones
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado(EventoScoModel miDataM)
        {
            EventoScoModel model = new EventoScoModel();
            DateTime fechaInicio = DateTime.ParseExact(miDataM.DI, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(miDataM.DF, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddDays(1);
            try
            {
                UserDTO usuario = (UserDTO)Session[DatosSesion.SesionUsuario];
                //Obtener lista de modulos
                List<int> idsModulos = this.seguridad.ObtenerRolPorUsuario(usuario.UserCode).
                                               Where(x => x.Seleccion > 0).Select(x => (int)x.RolCode).ToList();
                bool esOsinergmin = idsModulos.Contains(316);
                ViewBag.EsOsinergmin = esOsinergmin;
                model.LstEvento = servicioEvento.ConsultaEventoSco(fechaInicio, fechaFin).OrderByDescending(e => e.EvenIni).ToList();
                
                
                model.Emprcodi = Convert.ToInt32(((UserDTO)Session[DatosSesion.SesionUsuario]).EmprCodi);
                
                //calcular plazo envío
                servicioEvento.CalcularPlazoEventoSco(model.LstEvento);                
            }
            catch (Exception ex)
            {
                model.StrMensaje = "Se produjo un error: " + ex.Message;
                model.Resultado = "-1";
                Log.Error(NameController, ex);
            }
            return PartialView(model);
        }

        

        /// <summary>
        /// Ver detalle de Evento
        /// </summary>
        /// <param name="Evencodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult VerDetalleEvento(int Evencodi)
        {
            EventoScoModel model = new EventoScoModel();
            try
            {
                EveEventoDTO evento = servicioEvento.GetByIdEveEvento(Evencodi);
                model.Detalle = evento.Evendesc;

            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                Log.Error(NameController, ex);
            }
            return PartialView(model);
        }

        /// <summary>
        /// Ver detalle de Evento
        /// </summary>
        /// <param name="Evencodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarInformesEventos(int Evencodi, int Tip_arch, string Plazo, int Emprcodi)
        {
            EventoScoModel model = new EventoScoModel();
            try
            {
                string appName = Constantes.InitialUrl;
                ViewBag.AppName = appName;
                UserDTO usuario = (UserDTO)Session[DatosSesion.SesionUsuario];
                //Obtener lista de modulos
                List<int> idsModulos = this.seguridad.ObtenerRolPorUsuario(usuario.UserCode).
                                               Where(x => x.Seleccion > 0).Select(x => (int)x.RolCode).ToList();
                bool esOsinergmin = idsModulos.Contains(316);
                ViewBag.EsOsinergmin = esOsinergmin;
                EveEventoDTO evento = servicioEvento.GetByIdEveEvento(Evencodi);
                model.IdEvento = Evencodi;
                model.Emprcodi = Emprcodi;
                model.TipoInforme = Tip_arch;
                model.PlazoEnvio = Plazo;
                model.Detalle = evento.Evendesc;
                model.Descripcion = evento.Evenasunto;
                model.FechaEvento = ((DateTime)evento.Evenini).ToString(Constantes.FormatoFechaFull); 
                
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                Log.Error(NameController, ex);
            }
            return PartialView(model);
        }

        public JsonResult GrabarInformes(int IdEvento, int Emprcodi, int TipoInforme,string PlazoEnvio, HttpPostedFileBase[] file)
        {
            try
            {
                int id_Envio = 0;
                int id_EnvioEvento = 0;
                int result = 0;
                int rpta = 0;
                string responseText = "";
                string sNombreArchivo = string.Empty;
                string usuario = Convert.ToString(((UserDTO)Session[DatosSesion.SesionUsuario]).UserEmail);
                EveInformefallaDTO InformeFalla = new EveInformefallaDTO();
                EveInformefallaN2DTO InformeFallaN2 = new EveInformefallaN2DTO();
                EventoDTO oEventoDTO = new EventoDTO();
           
                if (file != null)
                {
                    for (int x = 0; x < file.Length; x++)
                    {
                        if (file[x].FileName.Length > 80)
                            return Json(new { result = -2, responseText = "Nombre de archivo con cantidad de caracteres superior a 80." }, JsonRequestBehavior.AllowGet);
                    }

                    MeEnvioDTO envio = new MeEnvioDTO();
                    envio.Archcodi = 0;
                    envio.Emprcodi = Emprcodi;
                    envio.Enviofecha = DateTime.Now;
                    envio.Enviofechaperiodo = DateTime.Now;
                    envio.Lastdate = DateTime.Now;
                    envio.Enviofechaini = DateTime.Now;
                    envio.Enviofechafin = DateTime.Now;
                    envio.Envioplazo = PlazoEnvio;
                    envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;
                    envio.Lastdate = DateTime.Now;
                    envio.Lastuser = usuario;
                    envio.Userlogin = usuario;
                    envio.Formatcodi = ConstantesEvento.FormatoSco;
                    envio.Cfgenvcodi = 0;
                    id_Envio = servFormato.SaveMeEnvio(envio);

                    MeEnvioEveEventoDTO envioEvento = new MeEnvioEveEventoDTO();
                    envioEvento.Enviocodi = id_Envio;
                    envioEvento.Evencodi = IdEvento;
                    envioEvento.Envetapainforme = TipoInforme;
                    id_EnvioEvento = servicioEvento.SaveMeEnvioEveEvento(envioEvento);

                    EveInformesScoDTO informe = new EveInformesScoDTO();
                    informe.Env_Evencodi = id_EnvioEvento;
                    informe.Lastuser = usuario;

                    //Año de evento
                    EveEventoDTO evento = servicioEvento.GetByIdEveEvento(IdEvento);
                    string aaaa = evento.Evenini.Value.ToString("yyyy");
                    string aa = evento.Evenini.Value.ToString("yy");
                    string mm = evento.Evenini.Value.ToString("MM");
                    string dd = evento.Evenini.Value.ToString("dd");
                    //Datos de Empresa
                    SiEmpresaDTO objEmpresa = servicioEvento.ObtenerEmpresa(Emprcodi);
                    //Semana operativa
                 
                    string ss = Tools.ObtenerNroSemanaAnio(Convert.ToDateTime(evento.Evenini)).ToString("00");

                    for (int x=0; x < file.Length; x++)
                    {
                        informe.Lastdate = DateTime.Now;
                        informe.Eveinfrutaarchivo = file[x].FileName;

                        if (file[x].FileName.Length > 80)
                            return Json(new { result = -2, responseText = "Cantidad de caracteres de archivo es superior a 80." }, JsonRequestBehavior.AllowGet);

                        informe.Eveinfactivo = "S";
                        informe.Anio = aaaa;
                        informe.Semestre = "Sem" + ss + aa;
                        informe.Diames = dd + mm;
                        result = servicioEvento.SaveEveInformesSco(informe);
                    }
                    
                    // Crear carpeta IPI e IF
                    string fileserverExtranet = ConstantesEvento.FileSystemExtranet;
                    string fileserverSco = ConstantesEvento.FileSystemSCO;
                    string CarpetaExtranet = string.Empty;
                    string CarpetaSco = string.Empty;

                    if (evento.Eveninffalla == "S")
                        InformeFalla = servicioEvento.MostrarEventoInformeFalla(evento.Evencodi);
                    else if(evento.Eveninffallan2 == "S")
                        InformeFallaN2 = servicioEvento.MostrarEventoInformeFallaN2(evento.Evencodi);

                    if (TipoInforme == 1 && evento.Eveninffalla == "S")
                    {
                        CarpetaExtranet = ConstantesEvento.CarpetaInformeFallaN1 + "\\" + aaaa + "\\Sem" + ss + aa + "\\" + dd + mm + "\\E" + InformeFalla.Evencorr.ToString() + "\\IPI\\" + objEmpresa.Emprnomb + "\\" + id_EnvioEvento.ToString() + "\\";
                        CarpetaSco = ConstantesEvento.CarpetaInformeFallaN1 + "\\" + aaaa + "\\Sem" + ss + aa + "\\" + dd + mm + "\\E" + InformeFalla.Evencorr.ToString() + "\\IPI\\" + objEmpresa.Emprnomb + "\\" + id_EnvioEvento.ToString() + "\\";
                    }                       
                    else if(TipoInforme == 1 && evento.Eveninffallan2 == "S")
                    {
                        CarpetaExtranet = ConstantesEvento.CarpetaInformeFallaN2 + "\\" + aaaa + "\\Sem" + ss + aa + "\\" + dd + mm + "\\E" + InformeFallaN2.Evenn2corr.ToString() + "\\IPI\\" + objEmpresa.Emprnomb + "\\" + id_EnvioEvento.ToString() + "\\";
                        CarpetaSco = ConstantesEvento.CarpetaInformeFallaN2 + "\\" + aaaa + "\\Sem" + ss + aa + "\\" + dd + mm + "\\E" + InformeFallaN2.Evenn2corr.ToString() + "\\IPI\\" + objEmpresa.Emprnomb + "\\" + id_EnvioEvento.ToString() + "\\";
                    }                        
                    else if (TipoInforme == 2 && evento.Eveninffalla == "S")
                    {
                        CarpetaExtranet = ConstantesEvento.CarpetaInformeFallaN1 + "\\" + aaaa + "\\Sem" + ss + aa + "\\" + dd + mm + "\\E" + InformeFalla.Evencorr.ToString() + "\\IF\\" + objEmpresa.Emprnomb + "\\" + id_EnvioEvento.ToString() + "\\";
                        CarpetaSco = ConstantesEvento.CarpetaInformeFallaN1 + "\\" + aaaa + "\\Sem" + ss + aa + "\\" + dd + mm + "\\E" + InformeFalla.Evencorr.ToString() + "\\IF\\" + objEmpresa.Emprnomb + "\\" + id_EnvioEvento.ToString() + "\\";
                    }                       
                    else if (TipoInforme == 2 && evento.Eveninffallan2 == "S")
                    {
                        CarpetaExtranet = ConstantesEvento.CarpetaInformeFallaN2 + "\\" + aaaa + "\\Sem" + ss + aa + "\\" + dd + mm + "\\E" + InformeFallaN2.Evenn2corr.ToString() + "\\IF\\" + objEmpresa.Emprnomb + "\\" + id_EnvioEvento.ToString() + "\\";
                        CarpetaSco = ConstantesEvento.CarpetaInformeFallaN2 + "\\" + aaaa + "\\Sem" + ss + aa + "\\" + dd + mm + "\\E" + InformeFallaN2.Evenn2corr.ToString() + "\\IF\\" + objEmpresa.Emprnomb + "\\" + id_EnvioEvento.ToString() + "\\";
                    }

                    // Nombres de archivos
                    List<string> filesName = new List<string>();

                    //Copiar archivos a extranet
                    if (file != null)
                    {
                        foreach (var item in file)
                        {
                            filesName.Add(item.FileName);
                            sNombreArchivo = item.FileName;
                            if (FileServer.VerificarExistenciaFile(null, CarpetaExtranet + "\\", fileserverExtranet))
                            {
                                if (FileServer.VerificarExistenciaFile(null, CarpetaExtranet + "\\" + sNombreArchivo, fileserverExtranet))
                                {
                                    FileServer.DeleteBlob(CarpetaExtranet + "\\" + sNombreArchivo, fileserverExtranet);
                                }
                            }                               
                            else
                                FileServer.CreateFolder(null, CarpetaExtranet + "\\", fileserverExtranet);

                            FileServer.UploadFromStream(item.InputStream, CarpetaExtranet, sNombreArchivo, fileserverExtranet);

                     
                            //Copiar archivos a Sco
                            FileServer.CreateFolder(null, CarpetaSco + "\\", fileserverSco);
                            FileServer.CopiarFileAlter(fileserverExtranet, "", CarpetaExtranet + sNombreArchivo, fileserverSco);                           
                        }
                    }

                    //Copia los archivos de Extranet a Sco si es que hubieran sido movidos
                    UploadFileSev(evento.Evencodi, CarpetaExtranet, TipoInforme, 1);

                    List<EveEventoDTO> lstEventosAsociados = servAF.ListadoEventosAsoCtaf(evento.Evencodi).DistinctBy(y => y.Evencodi).ToList();
                    List<EventoDTO> lstEvento = new List<EventoDTO>();
                    AnalisisFallaDTO oAnalisisFallaDTO;
                    foreach (var j in lstEventosAsociados)
                    {
                        EventoDTO Asociado = servAF.EventoDTOAsoCtaf(j.Evencodi);
                        if (Asociado.CODIGO != null)
                            lstEvento.Add(Asociado);
                    }

                    EventoDTO primer_evento = lstEvento.OrderBy(c => DateTime.ParseExact(c.FECHA_EVENTO, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture)).FirstOrDefault();

                    //Copiar archivos IPIs e IFs desde Sco a Fs Sev
                    if(primer_evento != null)
                        oAnalisisFallaDTO = servicioEvento.ObtenerAnalisisFalla(Convert.ToInt32(primer_evento.EVENCODI));
                    else
                        oAnalisisFallaDTO = servicioEvento.ObtenerAnalisisFalla(Convert.ToInt32(evento.Evencodi));

                    if (oAnalisisFallaDTO.AFECODI > 0)
                    {
                        oEventoDTO.Afeanio = oAnalisisFallaDTO.AFEANIO.ToString();
                        oEventoDTO.Afecorr = oAnalisisFallaDTO.AFECORR.ToString();
                        List<EventoDTO> lstEventosSco = servAF.ListarInterrupcionPorEventoSCO(oEventoDTO);
                        string fileSev = string.Empty;
                        if (lstEventosSco.Count > 0)
                        {
                            lstEventosSco.OrderBy(x => x.EVENINI).First();

                            string FsSev = ConstantesEvento.FileSystemSev;
                     
                            //string asunto = (RemoveAccentsWithRegEx(Regex.Replace(oAnalisisFallaDTO.EVENASUNTO, "[\t@,\\\"/:*?<>|\\\\]", string.Empty)).TrimEnd()).TrimStart();
                            //int maxcaracteres = ConstantesEvento.MaxCaractAF;
                            //if (asunto.Length > maxcaracteres)
                            //    asunto = asunto.Substring(0, maxcaracteres).Trim();

                            string NombreEvento = oAnalisisFallaDTO.CODIGO + "_" + oAnalisisFallaDTO.EVENINI.Value.ToString("dd.MM.yyyy");
                            DateTime FechaFinSem1 = DateTime.ParseExact(ConstantesEvento.FechaFinSem1 + aaaa, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                            DateTime FechaInicioSem2 = DateTime.ParseExact(ConstantesEvento.FechaInicioSem2 + aaaa, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                            DateTime FechaFinSem2 = DateTime.ParseExact(ConstantesEvento.FechaFinSem2 + aaaa, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                            DateTime FechaEvento = DateTime.ParseExact(oAnalisisFallaDTO.EVENINI.Value.ToString("dd/MM/yyyy"), Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                            string semestre = string.Empty;

                            if (FechaEvento <= FechaFinSem1)
                            {
                                semestre = "Semestre I";
                            }
                            else if (FechaEvento >= FechaInicioSem2 && FechaEvento <= FechaFinSem2)
                            {
                                semestre = "Semestre II";
                            }

                            fileSev = aaaa + "\\" + semestre + "\\" + NombreEvento + "\\";
                            
                            if (lstEventosSco.Count == 1)
                            {
                                if (oAnalisisFallaDTO.EMPRNOMB != null)
                                {
                                    //fileSev += oAnalisisFallaDTO.EMPRNOMB + "\\";
                                    //FileServer.CreateFolder(null, fileSev + "IPI\\", FsSev);
                                    rpta += UploadFileSev(oAnalisisFallaDTO.EVENCODI, fileSev, 1, 2);

                                    //FileServer.CreateFolder(null, fileSev + "IF\\", FsSev);
                                    rpta += UploadFileSev(oAnalisisFallaDTO.EVENCODI, fileSev, 2, 2);
                                }
                            }
                            else if (lstEventosSco.Count > 1)
                            {
                                AnalisisFallaDTO oAnalisisFallaDTOActual = servicioEvento.ObtenerAnalisisFalla(Convert.ToInt32(evento.Evencodi));

                                fileSev += oAnalisisFallaDTOActual.EVENINI.Value.ToString("dd.MM.yyyy HH.mm") + "\\";
                                if (oAnalisisFallaDTOActual.EMPRNOMB != null)
                                {
                                    //fileSev += oAnalisisFallaDTOActual.EMPRNOMB + "\\";

                                    //FileServer.CreateFolder(null, fileSev + "IPI\\", FsSev);
                                    rpta += UploadFileSev(oAnalisisFallaDTOActual.EVENCODI, fileSev, 1, 2);

                                    //FileServer.CreateFolder(null, fileSev + "IF\\", FsSev);
                                    rpta += UploadFileSev(oAnalisisFallaDTOActual.EVENCODI, fileSev, 2, 2);

                                }
                            }
                        }

                    }

                    string content = "Buen día,<br> se ha realizado exitosamente el envío de información desde el módulo de Informe de Fallas de la Extranet.<br><b>Los datos del envío:</b><br><ul><li>Fecha y hora del Evento: " + evento.Evenfin.ToString() + "</li> <li>Asunto: " + evento.Evenasunto + "</li> <li>Descripción: " + evento.Evendesc + "</li></ul><b>Detalles de los documentos enviados: </b><br><table border="+'"'+"1"+'"'+"><tr><th>Nombre</th><th>Fecha de envío</th><th>Código</th></tr>";
                    List<string> email_files = new List<string>();
                    try
                    {
                        List<EveInformesScoDTO> informesEnviados = servicioEvento.ListEveInformesSco(IdEvento, TipoInforme).ToList();
                        
                        string table = "";
                        for (int i = 0; i < informesEnviados.Count; i++)

                        {
                            for (int j = 0; j < filesName.Count; j++)
                            {
                                if (informesEnviados[i].Eveinfrutaarchivo == filesName[j])
                                {                                    
                                    table += "<tr><td>"+ informesEnviados[i].Eveinfrutaarchivo + "</td>";
                                    table += "<td>"+ ((DateTime)informesEnviados[i].Lastdate).ToString("dd/MM/yyyy HH:mm") + "</td>";
                                    table += "<td align=" +'"' + "center" + '"' + ">" + informesEnviados[i].Env_Evencodi + "</td></tr>";
                                }
                            }                                                        
                        }
                        content += table + "</table><br>Saludos,";
                    }
                    catch (Exception ex)
                    {
                        Log.Error(NameController, ex);
                        return Json(new { result = 0, responseText = ex.ToString() }, JsonRequestBehavior.AllowGet);
                    }

                    CorreoAppServicio correoAppServicio = new CorreoAppServicio();
                    correoAppServicio.EnviarCorreo("", usuario, null, null, "Confirmación envío Módulo Informe de Fallas", content, null, "", email_files);
                }
                return Json(new { result = result, responseText = responseText, rpta = rpta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { result = 0, responseText = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// Ver detalle de Evento
        /// </summary>
        /// <param name="Evencodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListadoInformesEnviados(int IdEvento, int TipoInforme)
        {
            EventoScoModel model = new EventoScoModel();
            try
            {
                string appName = Constantes.InitialUrl;
                ViewBag.AppName = appName;
                UserDTO usuario = (UserDTO)Session[DatosSesion.SesionUsuario];
                //Obtener lista de modulos
                List<int> idsModulos = this.seguridad.ObtenerRolPorUsuario(usuario.UserCode).
                                               Where(x => x.Seleccion > 0).Select(x => (int)x.RolCode).ToList();
                bool esOsinergmin = idsModulos.Contains(316);
                ViewBag.EsOsinergmin = esOsinergmin;
                model.LstInformes = servicioEvento.ListEveInformesSco(IdEvento, TipoInforme).ToList();
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                Log.Error(NameController, ex);
            }
            return PartialView(model);
        }
        [HttpGet]

        /// Permite descargar los archivos desde file server de extranet
        /// </summary>
        /// <returns></returns>
        public FileStreamResult DescargarArchivoSco(string idEvento, string env_evencodi, string eveinfcodi, string nombreArchivo, string anio, string semestre, string diames, string TipoInforme, string emprCodi)
        {
            try
            {
                
                // Obtener el evento
                EveEventoDTO evento = servicioEvento.GetByIdEveEvento(Convert.ToInt32(idEvento));

                string etapa = TipoInforme == "1" ? "IPI" : "IF";
                string carpetaBase = string.Empty;
                string correlativo = string.Empty;

                // Obtener correlativo segun tipo de evento (N1 o N2)
                if (evento.Eveninffalla == "S")
                {
                    var informeFalla = servicioEvento.MostrarEventoInformeFalla(evento.Evencodi);
                    correlativo = informeFalla.Evencorr.ToString();
                    carpetaBase = ConstantesEnviarCorreo.CarpetaInformeFallaN1;
                }
                else if (evento.Eveninffallan2 == "S")
                {
                    var informeFallaN2 = servicioEvento.MostrarEventoInformeFallaN2(evento.Evencodi);
                    correlativo = informeFallaN2.Evenn2corr.ToString();
                    carpetaBase = ConstantesEnviarCorreo.CarpetaInformeFallaN2;
                }
                //Año de evento
                
                string aaaa = evento.Evenini.Value.ToString("yyyy");
                string aa = evento.Evenini.Value.ToString("yy");
                string mm = evento.Evenini.Value.ToString("MM");
                string dd = evento.Evenini.Value.ToString("dd");
                //Datos de Empresa
                SiEmpresaDTO objEmpresa = servicioEvento.ObtenerEmpresa(Convert.ToInt32(emprCodi));
                //Semana operativa

                string ss = Tools.ObtenerNroSemanaAnio(Convert.ToDateTime(evento.Evenini)).ToString("00");

                // Construir ruta del archivo
                string foldername = Path.Combine(
                    carpetaBase,
                    aaaa,
                    "Sem" + ss + aa,
                    dd + mm,
                    "E" + correlativo,
                    etapa,
                    objEmpresa.Emprnomb,
                    env_evencodi
                 ) + "\\";

                string fullpath = foldername + nombreArchivo;

                var path = Constantes.FileSystemSco;
                Stream stream = FileServer.DownloadToStream(fullpath, path);
                FileStream fs = stream as FileStream;

                if (fs != null)
                {
                    return File(fs, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo);
                }
                else
                {
                    Log.Info("No se encontró el archivo: " + fullpath);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }

        public FileResult DescargarZipInformes(string idEvento, string tipoInforme)
        {
            try
            {
                var evento = servicioEvento.GetByIdEveEvento(Convert.ToInt32(idEvento));
                string carpetaBase = string.Empty;
                string correlativo = string.Empty;

                if (evento.Eveninffalla == "S")
                {
                    var informeFalla = servicioEvento.MostrarEventoInformeFalla(evento.Evencodi);
                    correlativo = informeFalla.Evencorr.ToString();
                    carpetaBase = ConstantesEnviarCorreo.CarpetaInformeFallaN1;
                }
                else if (evento.Eveninffallan2 == "S")
                {
                    var informeFallaN2 = servicioEvento.MostrarEventoInformeFallaN2(evento.Evencodi);
                    correlativo = informeFallaN2.Evenn2corr.ToString();
                    carpetaBase = ConstantesEnviarCorreo.CarpetaInformeFallaN2;
                }

                string aaaa = evento.Evenini.Value.ToString("yyyy");
                string aa = evento.Evenini.Value.ToString("yy");
                string mm = evento.Evenini.Value.ToString("MM");
                string dd = evento.Evenini.Value.ToString("dd");
                string ss = Tools.ObtenerNroSemanaAnio(Convert.ToDateTime(evento.Evenini)).ToString("00");
                string etapa = tipoInforme == "1" ? "IPI" : "IF";

                var informes = servicioEvento.ListEveInformesSco(Convert.ToInt32(idEvento), Convert.ToInt32(tipoInforme)).ToList();
                if (informes.Count == 0)
                    return null;

                

                var pathAlternativo = Constantes.FileSystemSco;

                List<string> rutasArchivos = new List<string>();
                foreach (var informe in informes)
                {
                    var empresa = servicioEvento.ObtenerEmpresa(informe.Emprcodi);
                    string foldername = Path.Combine(
                        carpetaBase,
                        aaaa,
                        "Sem" + ss + aa,
                        dd + mm,
                        "E" + correlativo,
                        etapa,
                        empresa.Emprnomb,
                        informe.Env_Evencodi.ToString()
                    ) + "\\";

                    rutasArchivos.Add(foldername + informe.Eveinfrutaarchivo);
                }

                // Ruta temporal para el archivo ZIP
                string nombreZip = $"Informes_{idEvento}_{DateTime.Now:yyyyMMddHHmmss}.zip";
                string pathZipCompleto = Path.Combine(Path.GetTempPath(), nombreZip);

                int result = FileServer.DownloadAsZip(rutasArchivos, pathZipCompleto, pathAlternativo);

                if (result == 1 && System.IO.File.Exists(pathZipCompleto))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(pathZipCompleto);
                    System.IO.File.Delete(pathZipCompleto); // eliminar si ya no se necesita
                    return File(fileBytes, "application/zip", nombreZip);
                }

                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }



        public int UploadFileSev(int evencodi, string rutaSev, int tipo, int tipoCarga)
        {
            string etapa = string.Empty;
            string foldername = string.Empty;
            string filename = string.Empty;
            string fileserverSco = ConstantesEvento.FileSystemSCO;
            string fileserverSev = ConstantesEvento.FileSystemSev;
            string fileserverExtranet = ConstantesEvento.FileSystemExtranet;
            string rfSev = string.Empty;
            int rpta = 0;
            EveInformefallaDTO InformeFalla = new EveInformefallaDTO();
            EveInformefallaN2DTO InformeFallaN2 = new EveInformefallaN2DTO();
            EventosAppServicio servEvento = new EventosAppServicio();
            List<EveInformesScoDTO> lstInfFinales = new List<EveInformesScoDTO>();
            List<EveInformesScoDTO> lstInfPreliminares = new List<EveInformesScoDTO>();

            //Obtener lista de informes finales e informes preliminares
            if (tipo == 1)
            {
                List<EveInformesScoDTO> lstInformesPreliminares = servEvento.ListEveInformesScoxEvento(evencodi, 1).ToList(); //Lista de informes preliminares
                lstInfPreliminares.AddRange(lstInformesPreliminares);               
            }
            else if (tipo == 2)
            {
                List<EveInformesScoDTO> lstInformesFinales = servEvento.ListEveInformesScoxEvento(evencodi, 2).ToList(); //Lista de informes finales
                lstInfFinales.AddRange(lstInformesFinales);
            }

            foreach (var informeP in lstInfPreliminares)
            {
                if(tipoCarga == 1) //Extranet a Sco
                {
                    EveEventoDTO evento = servicioEvento.GetByIdEveEvento(evencodi);

                    etapa = informeP.Afiversion == 1 ? "IPI" : "IF";
                    filename = informeP.Eveinfrutaarchivo;

                    if (evento.Eveninffalla == "S")
                    {
                        InformeFalla = servEvento.MostrarEventoInformeFalla(evento.Evencodi);
                        foldername = ConstantesEvento.CarpetaInformeFallaN1 + "\\" + informeP.Anio + "\\" + informeP.Semestre + "\\" + informeP.Diames + "\\E" + InformeFalla.Evencorr.ToString() + "\\" + etapa + "\\" + informeP.Emprnomb.TrimEnd() + "\\" + informeP.Env_Evencodi.ToString() + "\\";
                    }
                    else if (evento.Eveninffallan2 == "S")
                    {
                        InformeFallaN2 = servEvento.MostrarEventoInformeFallaN2(informeP.Evencodi);
                        foldername = ConstantesEvento.CarpetaInformeFallaN2 + "\\" + informeP.Anio + "\\" + informeP.Semestre + "\\" + informeP.Diames + "\\E" + InformeFallaN2.Evenn2corr.ToString() + "\\" + etapa + "\\" + informeP.Emprnomb + "\\" + informeP.Env_Evencodi.ToString() + "\\";
                    }

                    //Match de IPs de Extranet a Sco por Evento
                    if (FileServer.VerificarExistenciaFile(null, foldername + filename, fileserverExtranet))
                    {
                        if (!FileServer.VerificarExistenciaFile(null, foldername + filename, fileserverSco))
                        {
                            FileServer.CreateFolder(null, foldername, fileserverSco);
                            FileServer.UploadFromFileDirectory(fileserverExtranet + foldername + filename, "", foldername + filename, fileserverSco);
                        }
                    }
                }
                else if(tipoCarga == 2) //Sco a Sev
                {
                    EveInformesScoDTO informe = servEvento.ObtenerInformeSco(informeP.Eveinfcodi);
                    etapa = informe.Afiversion == 1 ? "IPI" : "IF";
                    filename = informe.Eveinfrutaarchivo;
                    fileserverSco = ConstantesEvento.FileSystemSCO;
                    fileserverSev = ConstantesEvento.FileSystemSev;
                    fileserverExtranet = ConstantesEvento.FileSystemExtranet;

                    if (informe.Eveninffalla == "S")
                    {
                        InformeFalla = servEvento.MostrarEventoInformeFalla(informe.Evencodi);
                        foldername = ConstantesEvento.CarpetaInformeFallaN1 + "\\" + informe.Anio + "\\" + informe.Semestre + "\\" + informe.Diames + "\\E" + InformeFalla.Evencorr.ToString() + "\\" + etapa + "\\" + informe.Emprnomb.TrimEnd() + "\\" + informe.Env_Evencodi.ToString() + "\\";
                    }
                    else if (informe.Eveninffallan2 == "S")
                    {
                        InformeFallaN2 = servEvento.MostrarEventoInformeFallaN2(informe.Evencodi);
                        foldername = ConstantesEvento.CarpetaInformeFallaN2 + "\\" + informe.Anio + "\\" + informe.Semestre + "\\" + informe.Diames + "\\E" + InformeFallaN2.Evenn2corr.ToString() + "\\" + etapa + "\\" + informe.Emprnomb + "\\" + informe.Env_Evencodi.ToString() + "\\";
                    }

                    //Copiar IPIs de evento Sco a Sev si es que los hubiera.
                    if (informe.Afecodi > 0)
                    {
                        rfSev = rutaSev + informe.Emprnomb + "\\" + etapa + "\\" + informe.Env_Evencodi.ToString() + "\\";

                        string fileSevValidar = fileserverSev + rfSev + filename;
                        if (fileSevValidar.Length > 247)
                            rpta += 1;
                        else
                        {
                            FileServer.CreateFolder(null, rfSev, fileserverSev);

                            if (FileServer.VerificarExistenciaFile(null, foldername + filename, fileserverSco))
                                FileServer.UploadFromFileDirectory(fileserverSco + foldername + filename, "", rfSev + filename, fileserverSev);
                            else if (FileServer.VerificarExistenciaFile(null, foldername + filename, fileserverExtranet))
                                FileServer.UploadFromFileDirectory(fileserverExtranet + foldername + filename, "", rfSev + filename, fileserverSev);
                        }
                    }
                }
            }

            foreach (var informeF in lstInfFinales)
            {
                if (tipoCarga == 1) //Extranet a Sco
                {
                    EveEventoDTO evento = servicioEvento.GetByIdEveEvento(evencodi);

                    etapa = informeF.Afiversion == 1 ? "IPI" : "IF";
                    filename = informeF.Eveinfrutaarchivo;

                    if (evento.Eveninffalla == "S")
                    {
                        InformeFalla = servEvento.MostrarEventoInformeFalla(evento.Evencodi);
                        foldername = ConstantesEvento.CarpetaInformeFallaN1 + "\\" + informeF.Anio + "\\" + informeF.Semestre + "\\" + informeF.Diames + "\\E" + InformeFalla.Evencorr.ToString() + "\\" + etapa + "\\" + informeF.Emprnomb.TrimEnd() + "\\" + informeF.Env_Evencodi.ToString() + "\\";
                    }
                    else if (evento.Eveninffallan2 == "S")
                    {
                        InformeFallaN2 = servEvento.MostrarEventoInformeFallaN2(informeF.Evencodi);
                        foldername = ConstantesEvento.CarpetaInformeFallaN2 + "\\" + informeF.Anio + "\\" + informeF.Semestre + "\\" + informeF.Diames + "\\E" + InformeFallaN2.Evenn2corr.ToString() + "\\" + etapa + "\\" + informeF.Emprnomb + "\\" + informeF.Env_Evencodi.ToString() + "\\";
                    }

                    //Match de IFs de Extranet a Sco por Evento
                    if (FileServer.VerificarExistenciaFile(null, foldername + filename, fileserverExtranet))
                    {
                        if (!FileServer.VerificarExistenciaFile(null, foldername + filename, fileserverSco))
                        {
                            FileServer.CreateFolder(null, foldername, fileserverSco);
                            FileServer.UploadFromFileDirectory(fileserverExtranet + foldername + filename, "", foldername + filename, fileserverSco);
                        }
                    }
                }
                else if (tipoCarga == 2)//Sco a Sev
                {
                    EveInformesScoDTO informe = servEvento.ObtenerInformeSco(informeF.Eveinfcodi);
                    etapa = informe.Afiversion == 1 ? "IPI" : "IF";
                    filename = informe.Eveinfrutaarchivo;
                    fileserverSco = ConstantesEvento.FileSystemSCO;
                    fileserverSev = ConstantesEvento.FileSystemSev;

                    if (informe.Eveninffalla == "S")
                    {
                        InformeFalla = servEvento.MostrarEventoInformeFalla(informe.Evencodi);
                        foldername = ConstantesEvento.CarpetaInformeFallaN1 + "\\" + informe.Anio + "\\" + informe.Semestre + "\\" + informe.Diames + "\\E" + InformeFalla.Evencorr.ToString() + "\\" + etapa + "\\" + informe.Emprnomb + "\\" + informe.Env_Evencodi.ToString() + "\\";
                    }
                    else if (informe.Eveninffallan2 == "S")
                    {
                        InformeFallaN2 = servEvento.MostrarEventoInformeFallaN2(informe.Evencodi);
                        foldername = ConstantesEvento.CarpetaInformeFallaN2 + "\\" + informe.Anio + "\\" + informe.Semestre + "\\" + informe.Diames + "\\E" + InformeFallaN2.Evenn2corr.ToString() + "\\" + etapa + "\\" + informe.Emprnomb + "\\" + informe.Env_Evencodi.ToString() + "\\";
                    }

                    //Copiar IFs a SEV
                    if (informe.Afecodi > 0)
                    {
                        rfSev = rutaSev + informe.Emprnomb + "\\" + etapa + "\\" + informe.Env_Evencodi.ToString() + "\\";
                        string fileSevValidar = fileserverSev + rfSev + filename;
                        if (fileSevValidar.Length > 247)
                            rpta += 1;

                        FileServer.CreateFolder(null, rfSev, fileserverSev);

                        if (FileServer.VerificarExistenciaFile(null, foldername + filename, fileserverSco))
                            FileServer.UploadFromFileDirectory(fileserverSco + foldername + filename, "", rfSev + filename, fileserverSev);
                        else if (FileServer.VerificarExistenciaFile(null, foldername + filename, fileserverExtranet))
                            FileServer.UploadFromFileDirectory(fileserverExtranet + foldername + filename, "", rfSev + filename, fileserverSev);
                    }
                }
            }

            return rpta;

        }

        public static string RemoveAccentsWithRegEx(string inputString)
        {
            Regex replace_a_Accents = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            Regex replace_A_Accents = new Regex("[Á|À|Ä|Â]", RegexOptions.Compiled);
            Regex replace_e_Accents = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            Regex replace_E_Accents = new Regex("[É|È|Ë|Ê]", RegexOptions.Compiled);
            Regex replace_i_Accents = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            Regex replace_I_Accents = new Regex("[Í|Ì|Ï|Î]", RegexOptions.Compiled);
            Regex replace_o_Accents = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            Regex replace_O_Accents = new Regex("[Ó|Ò|Ö|Ô]", RegexOptions.Compiled);
            Regex replace_u_Accents = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            Regex replace_U_Accents = new Regex("[Ú|Ù|Ü|Û]", RegexOptions.Compiled);

            inputString = replace_a_Accents.Replace(inputString, "a");
            inputString = replace_A_Accents.Replace(inputString, "A");
            inputString = replace_e_Accents.Replace(inputString, "e");
            inputString = replace_E_Accents.Replace(inputString, "E");
            inputString = replace_i_Accents.Replace(inputString, "i");
            inputString = replace_I_Accents.Replace(inputString, "I");
            inputString = replace_o_Accents.Replace(inputString, "o");
            inputString = replace_O_Accents.Replace(inputString, "O");
            inputString = replace_u_Accents.Replace(inputString, "u");
            inputString = replace_U_Accents.Replace(inputString, "U");
            return inputString;
        }
    }
}