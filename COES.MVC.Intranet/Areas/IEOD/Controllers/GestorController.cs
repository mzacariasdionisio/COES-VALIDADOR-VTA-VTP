using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.IEOD.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.IEOD;
using COES.MVC.Intranet.SeguridadServicio;
using COES.MVC.Intranet.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using COES.MVC.Intranet.Models;
using COES.MVC.Intranet.Areas.IEOD.Models;

namespace COES.MVC.Intranet.Areas.IEOD.Controllers
{
    public class GestorController : BaseController
    {
        //
        // GET: /IEOD/Gestor/
        IEODAppServicio logic = new IEODAppServicio();
        CorreoAppServicio servCorreo = new CorreoAppServicio();
        SeguridadServicioClient seguridad = new SeguridadServicioClient();
        PR5ReportesAppServicio servicio = new PR5ReportesAppServicio();

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(GestorController));
        private static string NameController = "GestorController";
        private static List<EstadoModel> ListaEstadoSistemaA = new List<EstadoModel>();

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

        public GestorController()
        {
            ListaEstadoSistemaA = new List<EstadoModel>();
            ListaEstadoSistemaA.Add(new EstadoModel() { EstadoCodigo = "0", EstadoDescripcion = "NO" });
            ListaEstadoSistemaA.Add(new EstadoModel() { EstadoCodigo = "1", EstadoDescripcion = "SÍ" });
        }

        public ActionResult Index()
        {
            int idMod = 9;

            int alerta = 1;
            MensajeModel model = new MensajeModel();

            List<MeFormatoDTO> ListaFormatos = new List<MeFormatoDTO>();
            List<MeFormatoDTO> ListaFormatosAux = new List<MeFormatoDTO>();
            List<MeAmpliacionfechaDTO> ListaAmpliaciones = new List<MeAmpliacionfechaDTO>();
            List<SiFuentedatosDTO> ListaFuenteDatos = new List<SiFuentedatosDTO>();
            List<GenericoDTO> ListaContr = new List<GenericoDTO>();
            List<GenericoDTO> ListaFPendientes = new List<GenericoDTO>();

            List<SiLogDTO> ListLog = new List<SiLogDTO>();

            if (Session["EMPRESA_SELECT"] == null)
            {
                List<SiEmpresaDTO> TEMP = new List<SiEmpresaDTO>();
                SiEmpresaDTO tmp = new SiEmpresaDTO();

                string cod = Convert.ToString(((UserDTO)Session[DatosSesion.SesionUsuario]).Empresas);

                tmp.Emprcodi = 0;
                tmp.Emprnomb = "--SELECCIONE--";
                TEMP.Add(tmp);

                model.ListaEmpresas = TEMP;

                TEMP.AddRange(logic.ListarEmpresasXID(cod));

                model.ListaEmpresas = TEMP;

                Session["LISTA_EMPRESAS"] = model.ListaEmpresas;

                if (model.ListaEmpresas.Count > 0)
                {
                    Session["EMPRESA_SELECT"] = TEMP[1].Emprcodi;
                    Session["NOMEMPRESA_SELECT"] = TEMP[1].Emprnomb;

                }

                //return View("Selector", model);
            }
            /*  else
              {*/

            Session["EMPRESA"] = Session["EMPRESA_SELECT"];

            Session["EMPRESA_SELECT"] = null;

            string Hoy = DateTime.Now.ToString("dd/MM/yyyy");

            ListaFormatos = logic.GetByModuloMeFormatos(idMod);
            ListaFormatosAux = logic.PendientesMeFormatos(idMod, (int)Session["EMPRESA"], Hoy);

            string usuario = Convert.ToString(((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin);

            if (ListaFormatosAux.Count > 0)
            {
                ListaAmpliaciones = logic.GetAmpliaciones((int)Session["EMPRESA"], DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy"));

                int x = 0;
                foreach (var tm in ListaFormatosAux)
                {
                    DateTime feccha = DateTime.ParseExact(DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy"), Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    ListaFormatosAux[x].FechaInicio = feccha;
                    ListaFormatosAux[x].FechaFin = feccha.AddDays(ListaFormatosAux[x].Formathorizonte - 1);
                    ListaFormatosAux[x].FechaPlazo = feccha.AddDays(ListaFormatosAux[x].Formatdiaplazo).AddMinutes(ListaFormatosAux[x].Formatminplazo);
                    ListaFormatosAux[x].FechaPlazoIni = feccha.AddDays(ListaFormatosAux[x].Formatdiaplazo);

                    if (ListaFormatosAux[x].Formatdiaplazo == 0)
                    {
                        ListaFormatosAux[x].FechaPlazo = feccha.AddDays(1).AddMinutes(ListaFormatosAux[x].Formatminplazo);
                    }
                    else
                    {
                        ListaFormatosAux[x].FechaPlazo = feccha.AddDays(ListaFormatosAux[x].Formatdiaplazo).AddMinutes(ListaFormatosAux[x].Formatminplazo);
                    }

                    if (ListaAmpliaciones.Count > 0)
                    {
                        foreach (var YY in ListaAmpliaciones)
                        {
                            if (ListaFormatosAux[x].Formatcodi == YY.Formatcodi)
                            {
                                ListaFormatosAux[x].FechaPlazo = YY.Amplifechaplazo;
                            }
                        }
                    }
                    if (alerta == 1)
                    {
                        //if (ListaFormatosAux[x].FechaPlazo < DateTime.Now)
                        //{
                        //    alerta = 3;
                        //}
                        //else {
                        TimeSpan ts = ListaFormatosAux[x].FechaPlazo - DateTime.Now;
                        int minutos = Convert.ToInt32(ts.TotalMinutes);
                        if (minutos < 30)
                        {
                            alerta = 2;
                        }
                        //  }
                    }
                    if (alerta == 2)
                    {
                        if (ListaFormatosAux[x].FechaPlazo < DateTime.Now)
                        {
                            alerta = 3;
                        }

                    }



                    x = x + 1;
                }
                model.Alerta = alerta;
            }

            /// Proximas Actividades
            if (ListaFormatosAux.Count > 0)
            {
                foreach (var tempp in ListaFormatosAux)
                {
                    GenericoDTO pendientestemp = new GenericoDTO();
                    pendientestemp.Entero1 = tempp.Formatcodi;
                    pendientestemp.String1 = tempp.Formatnombre;
                    pendientestemp.String3 = "green";

                    if (tempp.FechaPlazo < DateTime.Now)
                    {
                        pendientestemp.String3 = "red";
                    }
                    else
                    {
                        TimeSpan ts = tempp.FechaPlazo - DateTime.Now;
                        int minutos = Convert.ToInt32(ts.TotalMinutes);
                        if (minutos < 30)
                        {
                            pendientestemp.String3 = "#FFBF00";
                        }
                        else
                        {
                            pendientestemp.String3 = "green";
                        }
                    }

                    ListaFPendientes.Add(pendientestemp);

                }
                model.ListaPendientes = ListaFPendientes;
            }
            else
            {
                //  model.ListaPendientes = ListaFormatosAux;
            }
            ////

            ListaFuenteDatos = logic.GetByModuloSiFuentedatos(idMod);


            if (ListaFormatos.Count > 0)
            {
                Session["LISTA_FORMATOS"] = ListaFormatos;
                string[] ma = (ConstantesIEOD.Controladores.Replace('"', ' ')).Split('-');

                foreach (var temp in ListaFormatos)
                {
                    GenericoDTO tempList = new GenericoDTO();

                    tempList.Entero1 = temp.Formatcodi;
                    tempList.String1 = temp.Formatnombre;

                    for (int x = 0; x < ma.Count(); x++)
                    {
                        string[] mb = ma[x].Split(',');

                        if (Convert.ToInt32(mb[0]) == temp.Formatcodi)
                        {
                            tempList.String2 = mb[1].Replace("'", "").Trim();
                            tempList.String3 = mb[2].Replace("'", "").Trim();
                        }
                    }
                    ListaContr.Add(tempList);

                }

                model.ListaEnlaces = ListaContr;
            }

            if (ListaFuenteDatos.Count > 0)
            {
                List<GenericoDTO> ListaControler = new List<GenericoDTO>();
                Session["LISTA_FUENTEDATOS"] = ListaFuenteDatos;
                string[] ma = (ConstantesIEOD.ControllerFuenteDatos.Replace('"', ' ')).Split('-');

                foreach (var temp in ListaFuenteDatos)
                {
                    GenericoDTO tempList = new GenericoDTO();

                    tempList.Entero1 = temp.Fdatcodi;
                    tempList.String1 = temp.Fdatnombre;

                    for (int x = 0; x < ma.Count(); x++)
                    {
                        string[] mb = ma[x].Split(',');

                        if (Convert.ToInt32(mb[0]) == temp.Fdatcodi)
                        {
                            tempList.String2 = mb[1].Replace("'", "").Trim();
                            tempList.String3 = mb[2].Replace("'", "").Trim();
                        }
                    }
                    ListaControler.Add(tempList);
                }
                model.ListaEnlaces.AddRange(ListaControler);
            }


            return View(model);
            /*}*/
        }

        public ActionResult Selector()
        {
            MensajeModel model = new MensajeModel();


            string cod = Convert.ToString(((UserDTO)Session[DatosSesion.SesionUsuario]).Empresas);
            model.ListaEmpresas = logic.ListarEmpresasXID(cod);

            return View(model);
        }

        [HttpPost]
        public PartialViewResult MailDetalle(int id)
        {
            MensajeModel model = new MensajeModel();
            List<SiMensajeDTO> mensajes;

            List<MeFormatoDTO> formatos = new List<MeFormatoDTO>();
            List<SiFuentedatosDTO> ListaFuenteDatos = new List<SiFuentedatosDTO>();

            SiSolicitudAmpliacionDTO SolicitudAct = new SiSolicitudAmpliacionDTO();

            formatos = (List<MeFormatoDTO>)Session["LISTA_FORMATOS"];

            ListaFuenteDatos = (List<SiFuentedatosDTO>)Session["LISTA_FUENTEDATOS"];

            if (Session["MENSAJES_TEMP"] != null)
            {
                mensajes = (List<SiMensajeDTO>)Session["MENSAJES_TEMP"];

                foreach (SiMensajeDTO temp in mensajes)
                {
                    if (temp.Msgcodi == id)
                    {
                        model.MsgCodi = id;
                        model.MsgFrom = temp.Msgfrom;
                        model.MsgFromName = temp.Msgfromname;
                        model.MsgAsunto = temp.Msgasunto;
                        model.MsgTo = temp.Msgto;
                        model.EstMsgCodi = temp.Estmsgcodi;
                        model.TMsgCodi = temp.Tmsgcodi;
                        /*string msgAnt = "<p><br><br><br></p><blockquote style='border-left: 4px solid #3d8ab8;'>" + temp.MsgContenido + "</blockquote>";*/
                        string mensajeTEMP = "";


                        if (temp.Tmsgcodi == 2)
                        {
                            string nomEmpr = GetNombreEmpresa(temp.Emprcodi);
                            mensajeTEMP = "<p style='font-size: 13px;font-family: sans-serif;'>Datos:</p><p style='font-size: 13px;font-family: sans-serif;'> Empresa: " + nomEmpr + "</p>";
                            if (Convert.ToInt32(temp.Formatcodi) > 0 && Convert.ToInt32(temp.Formatcodi) < 999)
                            {
                                string formatonomb = "";

                                foreach (var tem in formatos)
                                {
                                    if (tem.Formatcodi == Convert.ToInt32(temp.Formatcodi))
                                    {
                                        formatonomb = tem.Formatnombre;
                                    }
                                }
                                mensajeTEMP = mensajeTEMP + "<p style='font-size: 13px;font-family: sans-serif;'>Formato: " + formatonomb + "</p>";
                            }
                            else
                            {
                                string fuentenomb = "";

                                foreach (var tem in ListaFuenteDatos)
                                {
                                    if (tem.Fdatcodi == Convert.ToInt32(temp.Fdatcodi))
                                    {
                                        fuentenomb = tem.Fdatnombre;
                                    }
                                }
                                mensajeTEMP = mensajeTEMP + "<p style='font-size: 13px;font-family: sans-serif;'>Fuente: " + fuentenomb + "</p>";
                            }
                            if (temp.Msgfechaperiodo != null)
                            {
                                mensajeTEMP = mensajeTEMP + "<p style='font-size: 13px;font-family: sans-serif;'>Periodo: " + ((DateTime)temp.Msgfechaperiodo).ToString("dd/MM/yyyy") + " </p>";
                            }
                            SolicitudAct = this.logic.GetSolicitudAmpXMsg(temp.Msgcodi);

                            if (SolicitudAct.AmpliFechaPlazo != null)
                            {

                                mensajeTEMP = mensajeTEMP + "<p style='font-size: 13px;font-family: sans-serif;'>Fecha de ampliacion: " + ((DateTime)SolicitudAct.AmpliFechaPlazo).ToString("dd/MM/yyyy") + " </p>";
                            }

                        }
                        model.MsgContenido = temp.Msgcontenido;
                        Session["VER_MENSAJE"] = mensajeTEMP + "<br>" + temp.Msgcontenido;
                    }
                }

            }


            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult ListaMail(int id, int nropagina)
        {
            MensajeModel model = new MensajeModel();
            int max = 0;
            int indice = 0;
            int pageSize = 10;/* Constantes.PageSize;*/
            List<SiMensajeDTO> listaTemp = new List<SiMensajeDTO>();

            string correo = Convert.ToString(((UserDTO)Session[DatosSesion.SesionUsuario]).UserEmail);

            listaTemp = logic.ListarMensajes(correo, nropagina, Convert.ToString(id), pageSize);

            if (listaTemp.Count > 0)
            {
                foreach (var tmp in listaTemp)
                {
                    if (tmp.Msgcodi > max)
                    {
                        max = tmp.Msgcodi;
                    }
                }
                max = max + 1;
                foreach (var tmp in listaTemp)
                {
                    if (listaTemp[indice].Tmsgcodi == 9)
                    {
                        listaTemp[indice].Msgcodi = max;
                        max = max + 1;
                    }
                    indice = indice + 1;
                }

            }

            model.ListaMensajes = listaTemp;

            Session["MENSAJES_TEMP"] = model.ListaMensajes;

            Session["TIPO_LISTA"] = id;

            return PartialView(model);
        }

        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string id, string orden)
        {
            MensajeModel model = new MensajeModel();
            model.IndicadorPagina = false;
            string correo = Convert.ToString(((UserDTO)Session[DatosSesion.SesionUsuario]).UserEmail);

            int nroRegistros = logic.ContarMensajes(correo, orden);

            if (nroRegistros > 0)
            {
                int pageSize = 10;/* Constantes.PageSize;*/
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult ListaLog(int id)
        {
            MensajeModel model = new MensajeModel();
            // string correo = Convert.ToString(((UserDTO)Session[DatosSesion.SesionUsuario]).UserEmail);
            string Hoy = DateTime.Now.ToString("dd/MM/yyyy");
            int idMod = 9;
            string usuario = Convert.ToString(((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin);


            model.ListaLog = logic.ObtenerLog(usuario, Hoy, idMod);

            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult Redactar(int id)
        {
            MensajeModel model = new MensajeModel();
            List<SiMensajeDTO> mensajes;
            string destinatarios = "";

            List<MeFormatoDTO> ListaFormatos = new List<MeFormatoDTO>();
            List<SiFuentedatosDTO> ListaFuenteDatos = new List<SiFuentedatosDTO>();
            List<GenericoDTO> ListaContr = new List<GenericoDTO>();


            List<SiFuentedatosDTO> FuenteDatosTemp = new List<SiFuentedatosDTO>();

            List<string> ccEmails = seguridad.ObtenerModulo(9).ListaAdministradores.ToList().Select(x => x.UserEmail).ToList();
            string ff = HelperApp.ObtenerEmailRemitente();

            if (ccEmails.Count > 0)
            {
                int x = 1;
                foreach (var tm in ccEmails)
                {
                    if (x > 1)
                    {
                        destinatarios = destinatarios + ";";
                    }
                    destinatarios = destinatarios + tm;
                    x = x + 1;
                }
            }
            model.MsgTo = destinatarios;

            SiFuentedatosDTO ListF = new SiFuentedatosDTO();
            ListF.Fdatcodi = 0;
            ListF.Fdatnombre = "--SELECCIONE--";
            ListaFuenteDatos.Add(ListF);

            ListaFuenteDatos.AddRange(logic.GetByModuloSiFuentedatos(9));
            model.ListaFuenteDao = ListaFuenteDatos;


            ListaFormatos = logic.GetByModuloMeFormatos(9);

            GenericoDTO ListT = new GenericoDTO();
            ListT.Entero1 = 0;
            ListT.String1 = "--SELECCIONE--";
            ListaContr.Add(ListT);


            if (ListaFormatos.Count > 0)
            {
                foreach (var temp in ListaFormatos)
                {
                    GenericoDTO tempList = new GenericoDTO();

                    tempList.Entero1 = temp.Formatcodi;
                    tempList.String1 = temp.Formatnombre;

                    ListaContr.Add(tempList);
                }
                model.ListaFormatos = ListaContr;
            }

            GenericoDTO ListFinal = new GenericoDTO();
            ListFinal.Entero1 = 999;
            ListFinal.String1 = "OTRO";
            model.ListaFormatos.Add(ListFinal);



            List<SiTipoMensajeDTO> tempL = new List<SiTipoMensajeDTO>();
            SiTipoMensajeDTO tmp = new SiTipoMensajeDTO();
            int encontrado = 0;

            if (Session["MENSAJES_TEMP"] != null)
            {
                mensajes = (List<SiMensajeDTO>)Session["MENSAJES_TEMP"];

                foreach (SiMensajeDTO temp in mensajes)
                {
                    if (temp.Msgcodi == id)
                    {
                        model.MsgTo = temp.Msgfrom;

                        if (temp.Msgasunto.Contains("Re:"))
                        {
                            model.MsgAsunto = temp.Msgasunto;
                        }
                        else
                        {
                            model.MsgAsunto = "Re: " + temp.Msgasunto;
                        }

                        model.MsgFechaPeriodo = temp.Msgfechaperiodo;
                        model.TMsgCodi = (int)temp.Tmsgcodi;
                        model.EstMsgCodi = temp.Estmsgcodi;

                        string mens = (string)Session["VER_MENSAJE"];

                        string msgAnt = "<p><br><br><br></p><blockquote style='border-left: 4px solid #3d8ab8;'><p style='font-size:13px;'>" + temp.Msgfecha + " , " + temp.Msgfromname + " &lt;" + temp.Msgfrom + "&gt;" + " escribio:</p><br>" + mens + "</blockquote>";
                        model.MsgContenido = msgAnt;

                        Session["VER_MENSAJE"] = msgAnt;

                        encontrado = 1;
                    }
                }

            }

            if (encontrado == 0) { model.MsgContenido = ""; Session["VER_MENSAJE"] = ""; }
            model.MsgFrom = Convert.ToString(((UserDTO)Session[DatosSesion.SesionUsuario]).UserEmail);


            tmp.Tmsgcodi = 0;
            tmp.Tmsgnombre = "--SELECCIONE--";
            tempL.Add(tmp);

            model.ListaTipoMensaje = tempL;
            model.ListaTipoMensaje.AddRange(logic.ListarTipoMensaje());


            return PartialView(model);
        }

        /// <summary>
        /// Graba el registro en la base de datos
        /// </summary>
        [HttpPost]
        public JsonResult EnviarMail(string Correo, string Asunto, int TipoCorreo, int EstMsg, string Mensaje, string Periodo, int FormatCodi, int idFuente, string FchAmpl, string seleccion)
        {
            int resultado = -1;
            string date = "";
            string dateA = "";
            int flagAdj = 0;
            int CodModulo = 9;
            int FlagT = 0;

            string logEvento = "";
            // int idFuente = 0;
            //int FormatCodi = 0;

            try
            {
                SiMensajeDTO obj = new SiMensajeDTO();

                string fechaActual = DateTime.Now.ToString(Constantes.FormatoFechaFull);
                string CorreoFrom = Convert.ToString(((UserDTO)Session[DatosSesion.SesionUsuario]).UserEmail);
                string usuario = Convert.ToString(((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin);
                string usuarioNom = Convert.ToString(((UserDTO)Session[DatosSesion.SesionUsuario]).UsernName);

                int EmprCodi = (int)Session["EMPRESA"];

                string NewMensaje = Mensaje.Replace("¬", "<");
                /*  obj.MsgTo = Correo;
                  obj.MsgFrom = Convert.ToString(((UserDTO)Session[DatosSesion.SesionUsuario]).UserEmail);
                  obj.MsgAsunto = Asunto;
                  obj.MsgContenido = Mensaje;
                  obj.MsgFecha = DateTime.Now;*/

                if (FormatCodi > 0 && FormatCodi < 999)
                {
                    FlagT = 1;
                }
                if (idFuente > 0)
                {
                    FlagT = 2;
                }


                int codigo = this.logic.SaveCorreo(fechaActual, idFuente, TipoCorreo, EstMsg, NewMensaje, Periodo, CodModulo, EmprCodi, FormatCodi, usuario, Correo, CorreoFrom, usuarioNom, Asunto, flagAdj);

                logEvento = "Envió un mensaje";

                if (TipoCorreo == 2)
                {

                    int amp = this.logic.SaveSolicitudAmpliacion(Periodo, codigo, EmprCodi, FchAmpl, usuario, fechaActual, FormatCodi, idFuente, FlagT);
                    logEvento = "Solicitud de Ampliacion de Plazo - " + seleccion;
                }

                this.logic.SaveLog(CodModulo, logEvento, DateTime.Now.ToString(Constantes.FormatoFechaFull), usuario);

                /* if (TipoCorreo == 2)
                 {
                     EnviarCorreo(2, usuario, EmprCodi, seleccion, (string)Session["NOMEMPRESA_SELECT"], Convert.ToDateTime(Periodo), Convert.ToDateTime(FchAmpl), "");
                 }*/

                resultado = 1;
            }
            catch (Exception ex)
            {
                resultado = -1;
            }

            return Json(resultado);
        }

        // <summary>
        /// Envia correo segun plantilla y graba el correo en base de datos
        /// </summary>
        /// <param name="enPlazo"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formatoNombre"></param>
        /// <param name="empresaNombre"></param>
        /// <param name="areaNombre"></param>
        /// <param name="fechaProceso"></param>
        /// <param name="fechaEnvio"></param>
        protected void EnviarCorreo(int tipoCorreo, string usuario, int idEmpresa, string formatoNombre, string empresaNombre,
            DateTime fechaProceso, DateTime fechaEnvio, string estado, string remite)
        {
            // var usuario = User.Identity.Name;
            var plantilla = servCorreo.ObtenerPlantillaPorModulo(tipoCorreo, 9);//mod

            if (plantilla != null)
            {
                List<string> ccEmails = seguridad.ObtenerModulo(9).ListaAdministradores.ToList().Select(x => x.UserEmail).ToList();
                /*  List<string> ccEmails = new List<string>();
                 ccEmails.Add("oescudero2@gmail.com");*/

                string correos = "";

                if (ccEmails.Count > 0)
                {
                    int x = 1;
                    foreach (var tm in ccEmails)
                    {
                        if (x > 1)
                        {
                            correos = correos + ";";
                        }
                        correos = correos + tm;
                        x = x + 1;
                    }
                }


                string ccMail = string.Empty;
                string asunto = string.Format(plantilla.Plantasunto, formatoNombre);

                List<string> toMail = new List<string>();
                usuario = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserEmail;

                toMail.Add(usuario);

                string contenido = string.Format(plantilla.Plantcontenido, remite, empresaNombre, formatoNombre
                    , fechaProceso.ToString(Constantes.FormatoFecha), ((DateTime)fechaEnvio).ToString(Constantes.FormatoFechaFull), usuario, estado);

                //     COES.Base.Tools.Util.SendEmail(ccEmails/*toMail*/, ccEmails, asunto, contenido);

                var correo = new SiCorreoDTO();
                correo.Corrasunto = asunto;
                correo.Corrcontenido = contenido;
                correo.Corrfechaenvio = fechaEnvio;
                correo.Corrfechaperiodo = fechaProceso;
                /*correo.Corrfrom = HelperApp.ObtenerEmailRemitente();
                correo.Corrto = usuario;*/
                correo.Corrfrom = usuario;
                correo.Corrto = correos;
                correo.Emprcodi = idEmpresa;
                correo.Enviocodi = 0;
                correo.Plantcodi = 0;/* plantilla.Plantcodi;*/
                servCorreo.SaveSiCorreo(correo);
            }
        }

        [HttpPost]
        public JsonResult SetEmpresa(int emprcodi, string nomEmpr)
        {
            int resultado = 1;

            Session["EMPRESA_SELECT"] = emprcodi;
            Session["NOMEMPRESA_SELECT"] = nomEmpr;


            /*return RedirectToAction("Index", "Gestor");*/

            return Json(resultado);
        }

        [HttpPost]
        public String GetMensaje(int id)
        {
            String resultado = "";

            resultado = (string)Session["VER_MENSAJE"];



            return resultado;
        }

        /// <summary>
        /// Aprobar o rechazar la solicitud
        /// </summary>

        [HttpPost]
        public JsonResult AprobarRechazar(int MsgCodi, int estado, string Texto)
        {
            int resultado = -1;
            string date = "";
            string dateA = "";
            int flagAdj = 0;
            int CodModulo = 9;
            int FlagT = 0;
            int idFuente = 0;
            int EmprCodi = 0;
            string MsgTo = "";
            string asunto = "";
            string periodo = "";
            int Tmsgcodi = 0;
            string sms = "";
            int est = 0;
            int formato = 0;
            string msgAnt = "";
            string logEvento = "";
            string seleccion = "";
            string hoy = "";
            string nombreUS = "";

            List<MeFormatoDTO> formatos = new List<MeFormatoDTO>();
            List<SiMensajeDTO> mensaje = new List<SiMensajeDTO>();

            try
            {
                SiMensajeDTO obj = new SiMensajeDTO();

                string usuario = Convert.ToString(((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin);
                string ff = HelperApp.ObtenerEmailRemitente();

                /*int EmprCodi = (int)Session["EMPRESA"];*/

                if (Session["MENSAJES_TEMP"] != null)
                {

                    mensaje = (List<SiMensajeDTO>)Session["MENSAJES_TEMP"];

                    foreach (var temp in mensaje)
                    {
                        if (MsgCodi == temp.Msgcodi)
                        {
                            EmprCodi = temp.Emprcodi;
                            MsgTo = temp.Msgfrom;
                            idFuente = (int)temp.Fdatcodi;
                            formato = Convert.ToInt32(temp.Formatcodi);
                            nombreUS = temp.Msgfromname;

                            if (temp.Msgasunto.Contains("Re:"))
                            {
                                asunto = temp.Msgasunto;
                            }
                            else
                            {
                                asunto = "Re: " + temp.Msgasunto;
                            }

                            periodo = ((DateTime)temp.Msgfechaperiodo).ToString("dd/MM/yyyy");
                            Tmsgcodi = (int)temp.Tmsgcodi;
                            est = 1;

                            msgAnt = Texto.Replace("¬", "<");

                            hoy = (Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy")).AddHours(14)).ToString();

                            if (estado == 2)
                            {
                                sms = "<p>Se Aprobó la solicitud de ampliacion de plazo, fecha limite: " + hoy + "</p><p><br><br><br></p><blockquote style=\"border-left: 4px solid #3d8ab8;\"><p style=\"font-size:13px;\">" + temp.Msgfecha + " , " + temp.Msgfromname + " &lt;" + temp.Msgfrom + "&gt;" + " escribio:</p><br>" + msgAnt + "</blockquote>";
                                logEvento = "Se Aprobó la solicitud de ampliacion de plazo";
                            }
                            if (estado == 3)
                            {
                                logEvento = "Se Rechazó la solicitud de ampliacion de plazo";
                                sms = "<p>Se Rechazó la solicitud de ampliacion</p><p><br><br><br></p><blockquote style=\"border-left: 4px solid #3d8ab8;\"><p style=\"font-size:13px;\">" + temp.Msgfecha + " , " + temp.Msgfromname + " &lt;" + temp.Msgfrom + "&gt;" + " escribio:</p><br>" + msgAnt + "</blockquote>";
                            }


                        }
                    }

                    if (Session["LISTA_FORMATOS"] != null)
                    {
                        formatos = (List<MeFormatoDTO>)Session["LISTA_FORMATOS"];

                        foreach (var xt in formatos)
                        {
                            if (xt.Formatcodi == formato)
                            {
                                seleccion = xt.Formatnombre;
                            }
                        }
                    }




                    int result = this.logic.UpdateEstado(estado, MsgCodi, CodModulo, EmprCodi, usuario, DateTime.Now.ToString());


                    this.logic.SaveCorreo(DateTime.Now.ToString(), idFuente, Tmsgcodi, estado, sms.ToString(), periodo, CodModulo, EmprCodi, formato, usuario, MsgTo, ff, "Administrador", asunto, flagAdj);

                    this.logic.SaveLog(CodModulo, logEvento, DateTime.Now.ToString(), usuario);


                    if (estado == 2)
                    {
                        EnviarCorreo(2, usuario, EmprCodi, seleccion, (string)Session["NOMEMPRESA_SELECT"], Convert.ToDateTime(periodo), Convert.ToDateTime(hoy), "Aprobó", nombreUS);
                    }
                    if (estado == 3)
                    {
                        EnviarCorreo(2, usuario, EmprCodi, seleccion, (string)Session["NOMEMPRESA_SELECT"], Convert.ToDateTime(periodo), Convert.ToDateTime(hoy), "Rechazó", nombreUS);
                    }

                }

                resultado = 1;
            }
            catch (Exception ex)
            {
                resultado = -1;
            }

            return Json(resultado);
        }


        /// <summary>
        /// Obtiene el nombre de la empresa por codigo de empresa
        /// </summary>
        public string GetNombreEmpresa(int EmprCodi)
        {
            string nombre = "";

            if (Session["LISTA_EMPRESAS"] != null)
            {
                List<SiEmpresaDTO> empresas = new List<SiEmpresaDTO>();
                empresas = (List<SiEmpresaDTO>)Session["LISTA_EMPRESAS"];
                foreach (var temp in empresas)
                {
                    if (EmprCodi == temp.Emprcodi)
                    {
                        nombre = temp.Emprnomb;
                    }
                }
            }

            return nombre;
        }

        #region MENU REPORTE

        public ActionResult AdmMenuReportes()
        {
            BusquedaIEODModel model = new BusquedaIEODModel();

            List<ListaSelect> list = new List<ListaSelect>();
            model.Reptiprepcodi = list;

            return View(model);
        }

        public PartialViewResult CargarListaAdmReporte(int reptiprepcodi)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<SiMenureporteDTO> lista = new List<SiMenureporteDTO>();

            lista = servicio.GetListaAdmReporte(reptiprepcodi);

            model.Resultado = UtilAnexoAPR5.ListaAdmReporteHtml(lista);

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult UpdItemAdmReporte(int repcodi)
        {
            int result = 0;
            try
            {
                servicio.UpdItemAdmReporte(repcodi);
                result = 1;
            }
            catch { }

            return Json(result);
        }
        #endregion
    }
}