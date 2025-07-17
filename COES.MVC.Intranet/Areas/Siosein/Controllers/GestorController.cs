using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Siosein.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.SIOSEIN;
using COES.MVC.Intranet.Models;
using COES.MVC.Intranet.SeguridadServicio;
using COES.MVC.Intranet.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Areas.Siosein.Helper;
using log4net;
using COES.Base.Core;
using System.Globalization;

namespace COES.MVC.Intranet.Areas.Siosein.Controllers
{
    public class GestorController : BaseController
    {
        //
        // GET: /Siosein/Gestor/
        private SIOSEINAppServicio logic;

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(GestorController));
        private static string NameController = "GestorController";
        private static List<EstadoModel> ListaEstadoSistemaA = new List<EstadoModel>();

        /// <summary>
        /// Protected de log de errores page
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

        /// <summary>
        /// listado ventos del controller
        /// </summary>
        public GestorController()
        {
            logic = new SIOSEINAppServicio();
            ListaEstadoSistemaA = new List<EstadoModel>();
            ListaEstadoSistemaA.Add(new EstadoModel() { EstadoCodigo = "0", EstadoDescripcion = "NO" });
            ListaEstadoSistemaA.Add(new EstadoModel() { EstadoCodigo = "1", EstadoDescripcion = "SÍ" });
        }

        /// <summary>
        /// pagina inicio gestor de contenido
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            GestorModel model = new GestorModel();

            //List<GenericoDTO> ListaFPendientes = new List<GenericoDTO>();
            List<SiLogDTO> ListLog = new List<SiLogDTO>();

            string Hoy = DateTime.Now.ToString("dd/MM/yyyy");
            string usuario = Convert.ToString(((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin);

            model.FechaFiltro = DateTime.Now.ToString("MM/yyyy");
            var listaTipoMensaje = logic.ListarTipoMensajeXMod(ConstantesSiosein.ModcodiGestorSiossein);
            listaTipoMensaje.Select(x => x.Tmsgcolor = ConstantesSiosein.ColorTipoMensaje[x.Tmsgcodi]).ToList();
            model.ListaTipoMensaje = listaTipoMensaje;

            if (usuario == ConstantesSiosein.usuAdmin)
            {
                Session["Es-Admin"] = 1;
            }
            else { Session["Es-Admin"] = 0; }

            return View(model);
        }

        /// <summary>
        /// lista de carpetas
        /// </summary>
        /// <param name="id"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaCarpetas(int id, string periodo)
        {
            GestorModel model = new GestorModel();
            string correo = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserEmail;
            List<SiBandejamensajeUserDTO> lstCarpetas = logic.ListarCarpetaByModUser(ConstantesSiosein.ModcodiGestorSiossein, this.UserName, correo);
            model.ListaCarpetas = lstCarpetas;
            return PartialView(model);
        }

        /// <summary>
        /// metodo guardar una carpeta
        /// </summary>
        /// <param name="nomCarpeta"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarCarpeta(string nomCarpeta)
        {
            var model = new SioseinModel();
            try
            {
                string logEvento = "Se creo la Carperta " + nomCarpeta;

                model.ResultadoInt = logic.SaveCarpeta(nomCarpeta, this.UserName, DateTime.Now);
                model.Mensaje = logEvento;

                //logic.SaveLog(ConstantesSiosein.ModcodiGestorSiossein, logEvento, DateTime.Now, this.UserName);
            }
            catch (Exception ex)
            {
                model.ResultadoInt = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// partial html detalle del email
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult MailDetail(int id)
        {
            GestorModel model = new GestorModel();
            List<GestorModel.GenericoMensaje> mensajes;

            if (Session["MENSAJES_TEMP"] != null)
            {
                mensajes = (List<GestorModel.GenericoMensaje>)Session["MENSAJES_TEMP"];

                foreach (GestorModel.GenericoMensaje temp in mensajes)
                {
                    if (temp.MsgCodi == id)
                    {
                        model.MsgCodi = id;
                        model.MsgFrom = temp.MsgFrom;
                        model.MsgFromName = temp.MsgFromName;
                        model.MsgAsunto = temp.MsgAsunto;
                        model.MsgTo = temp.MsgTo;
                        model.EstMsgCodi = temp.EstMsgCodi;
                        model.TMsgCodi = temp.TMsgCodi;

                        string mensajeTEMP = "";

                        model.MsgContenido = temp.MsgContenido;
                        Session["VER_MENSAJE"] = mensajeTEMP + "<br>" + temp.MsgContenido;
                    }
                }

            }

            return PartialView(model);
        }

        /// <summary>
        /// lista de email a mostrar
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nropagina"></param>
        /// <param name="tipomensaje"></param>
        /// <param name="carpeta"></param>
        /// <param name="estmsgcodi"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListMail(int id, int tipomensaje, int carpeta, int estmsgcodi, string periodo)
        {
            GestorModel model = new GestorModel();
            Session["TITULO"] = "";
            int max = 0;
            int indice = 0;
            int pageSize = 10;/* Constantes.PageSize;*/

            DateTime fechaPeriodo = DateTime.ParseExact(periodo, ConstantesAppServicio.FormatoMesanio, CultureInfo.InvariantCulture);
            string correo = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserEmail;
            string usuario = this.UserName;
            List<SiMensajeDTO> listaTemp = new List<SiMensajeDTO>();

            switch (id)
            {
                case (int)ConstantesSiosein.Mensaje.Recibido:
                    listaTemp = logic.ListarSiMensajes(correo, ConstantesAppServicio.ParametroDefecto, ConstantesSiosein.ModcodiGestorSiossein, tipomensaje, carpeta, estmsgcodi,
                        fechaPeriodo, ConstantesAppServicio.Activo);
                    break;

                case (int)ConstantesSiosein.Mensaje.Enviado:
                    listaTemp = logic.ListarSiMensajes(ConstantesAppServicio.ParametroDefecto, correo, ConstantesSiosein.ModcodiGestorSiossein, tipomensaje, carpeta, estmsgcodi,
                        fechaPeriodo, ConstantesAppServicio.Activo);
                    break;
                case (int)ConstantesSiosein.Mensaje.Eliminado:
                    listaTemp = logic.ListarSiMensajes(ConstantesAppServicio.ParametroDefecto, correo, ConstantesSiosein.ModcodiGestorSiossein, tipomensaje, carpeta, estmsgcodi,
                        fechaPeriodo, ConstantesAppServicio.Baja);
                    break;
            }

            model.ListaCarpetas = logic.ListarCarpetaByModUser(ConstantesSiosein.ModcodiGestorSiossein, usuario, correo);
            List<SiTipoMensajeDTO> listaTipoMensaje = logic.ListarTipoMensajeXMod(ConstantesSiosein.ModcodiGestorSiossein);
            List<GestorModel.GenericoMensaje> ListaTempMensajes = new List<GestorModel.GenericoMensaje>();

            if (listaTemp.Count > 0)
            {
                foreach (var tmp in listaTemp)
                {
                    GestorModel.GenericoMensaje TempMensajes = new GestorModel.GenericoMensaje();
                    TempMensajes.MsgCodi = tmp.Msgcodi;
                    TempMensajes.MsgFecha = tmp.Msgfecha;
                    TempMensajes.TMsgCodi = tmp.Tmsgcodi;
                    TempMensajes.EstMsgCodi = tmp.Estmsgcodi;
                    TempMensajes.MsgContenido = tmp.Msgcontenido;
                    TempMensajes.MsgFecModificacion = tmp.Msgfechaperiodo;
                    TempMensajes.ModCodi = tmp.Modcodi;
                    TempMensajes.FormatCodi = tmp.Formatcodi;
                    TempMensajes.MsgTo = tmp.Msgto;
                    TempMensajes.MsgFrom = tmp.Msgfrom;
                    TempMensajes.MsgFromName = tmp.Msgfromname;
                    TempMensajes.MsgAsunto = tmp.Msgasunto;
                    TempMensajes.MsgFlagAdj = tmp.Msgflagadj;
                    TempMensajes.CarCodi = tmp.Carcodi;
                    TempMensajes.MsgFechaPeriodo = tmp.Msgfechaperiodo;
                    TempMensajes.Tmsgnombre = listaTipoMensaje.Find(x => x.Tmsgcodi == tmp.Tmsgcodi)?.Tmsgnombre;
                    TempMensajes.Tmsgcolor = ConstantesSiosein.ColorTipoMensaje[tmp.Tmsgcodi ?? 0];

                    ListaTempMensajes.Add(TempMensajes);
                }
            }

            model.ListaTempMensajes = ListaTempMensajes;

            Session["MENSAJES_TEMP"] = model.ListaTempMensajes;

            Session["TIPO_LISTA"] = id;


            if (id > 3)
            {
                foreach (var txp in model.ListaCarpetas)
                {
                    if (id == txp.Bandcodi)
                    {
                        Session["TITULO"] = txp.Bandnombre;
                    }
                }

            }

            return PartialView(model);
        }

        /// <summary>
        /// lista de log de eventos del gestor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaLog(int id)
        {
            GestorModel model = new GestorModel();
            string Hoy = DateTime.Now.ToString("dd/MM/yyyy");

            string usuario = Convert.ToString(((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin);

            model.ListaLog = logic.ObtenerLog(usuario, Hoy, ConstantesSiosein.ModcodiGestorSiossein);

            return PartialView(model);
        }

        /// <summary>
        /// partial html para redactar asuntos o comentarios
        /// </summary>
        /// <param name="id"></param>
        /// <param name="periodo"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Redactar(int id, string periodo, int tipo)
        {
            GestorModel model = new GestorModel();
            List<GestorModel.GenericoMensaje> mensajes;
            string destinatarios = "";

            List<MeFormatoDTO> ListaFormatos = new List<MeFormatoDTO>();
            List<SiFuentedatosDTO> ListaFuenteDatos = new List<SiFuentedatosDTO>();

            List<SioTablaprieDTO> lstTablasPrie = new List<SioTablaprieDTO> { new SioTablaprieDTO { Tpriecodi = 0, Tpriedscripcion = "--SELECCIONE--" } };

            lstTablasPrie.AddRange(logic.ListSioTablapries());


            model.ListaTablasPrie = lstTablasPrie;

            Session["LISTA_TABLAS_PRIE"] = lstTablasPrie;

            model.MsgTo = destinatarios;

            int encontrado = 0;

            if (Session["MENSAJES_TEMP"] != null)
            {
                mensajes = (List<GestorModel.GenericoMensaje>)Session["MENSAJES_TEMP"];

                foreach (GestorModel.GenericoMensaje temp in mensajes)
                {
                    if (temp.MsgCodi == id)
                    {
                        if (tipo == 1)
                        {
                            model.MsgTo = temp.MsgFrom;
                        }
                        if (tipo == 2)
                        {
                            model.MsgTo = GetEmailResponsable(Convert.ToInt32(temp.FormatCodi));
                        }

                        if (temp.MsgAsunto.Contains("Re:"))
                        {
                            model.MsgAsunto = temp.MsgAsunto;
                        }
                        else
                        {
                            model.MsgAsunto = "Re: " + temp.MsgAsunto;
                        }

                        model.MsgFechaPeriodo = temp.MsgFechaPeriodo;
                        model.TMsgCodi = (int)temp.TMsgCodi;
                        model.EstMsgCodi = temp.EstMsgCodi;
                        model.FormatCodi = temp.FormatCodi;
                        model.FechaFiltro = ((DateTime)(temp.MsgFechaPeriodo)).ToString("MM/yyyy");
                        model.CarCodi = temp.CarCodi;



                        string mens = (string)Session["VER_MENSAJE"];

                        string msgAnt = "<p><br><br><br></p><blockquote style='border-left: 4px solid #3d8ab8;'><p style='font-size:13px;'>" + temp.MsgFecha + " , " + temp.MsgFromName + " &lt;" + temp.MsgFrom + "&gt;" + " escribio:</p><br>" + mens + "</blockquote>";
                        model.MsgContenido = msgAnt;

                        Session["VER_MENSAJE"] = msgAnt;

                        encontrado = 1;
                    }
                }

            }

            model.ListaTipoMensaje = logic.ListarTipoMensajeXMod(ConstantesSiosein.ModcodiGestorSiossein);


            if (encontrado == 0)
            {
                model.FechaFiltro = periodo;
                model.MsgContenido = "";
                Session["VER_MENSAJE"] = "";
            }
            model.MsgFrom = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserEmail;


            return PartialView(model);
        }

        /// <summary>
        /// Graba el registro en la base de datos
        /// </summary>
        [HttpPost]
        public JsonResult EnviarMail(SiMensajeDTO simensaje, string periodo)
        {
            GestorModel model = new GestorModel();
            try
            {
                DateTime fechaPeriodo = DateTime.ParseExact(periodo, ConstantesAppServicio.FormatoMesanio, CultureInfo.InvariantCulture);

                simensaje.Msgfeccreacion = DateTime.Now;
                simensaje.Msgfechaperiodo = fechaPeriodo;
                simensaje.Modcodi = ConstantesSiosein.ModcodiGestorSiossein;
                simensaje.Msgcontenido = simensaje.Msgcontenido?.Replace("¬", "<");
                simensaje.Msgestado = "A";
                simensaje.Msgfrom = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserEmail;
                simensaje.Msgfromname = ((UserDTO)Session[DatosSesion.SesionUsuario]).UsernName;
                simensaje.Msgusucreacion = this.UserName;

                int codigo = this.logic.SaveCorreoSiosein(simensaje);

                string logEvento = "Envió un mensaje";

                this.logic.SaveLog(ConstantesSiosein.ModcodiGestorSiossein, logEvento, DateTime.Now.ToString(), this.UserName);

                model.Estado = ConstantesSiosein.Estado.Ok;
            }
            catch (Exception ex)
            {
                model.Estado = ConstantesSiosein.Estado.Error;
            }

            return Json(model);
        }

        /// <summary>
        /// Se recupera mensaje en session
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public String GetMensaje(int id)
        {
            String resultado = "";

            resultado = (string)Session["VER_MENSAJE"];

            return resultado;
        }

        /// <summary>
        /// metodo para mover carpetas del gestor
        /// </summary>
        /// <param name="carpeta"></param>
        /// <param name="seleccion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MoveraCarpeta(int carpeta, string seleccion)
        {
            int resultado = -1;

            try
            {

                string usuario = Convert.ToString(((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin);

                this.logic.UpdateCarpeta(carpeta, usuario, ConstantesSiosein.ModcodiGestorSiossein, seleccion);

                resultado = 1;
            }
            catch (Exception ex)
            {
                resultado = -1;
            }

            return Json(resultado);
        }

        /// <summary>
        /// recuperacion de email responsables por metodo session
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public String GetEmailResponsable(int id)
        {
            String resultado = "";
            List<SioTablaprieDTO> temporal = new List<SioTablaprieDTO>();

            if (Session["LISTA_TABLAS_PRIE"] != null)
            {
                temporal = (List<SioTablaprieDTO>)Session["LISTA_TABLAS_PRIE"];

                foreach (var tmp in temporal)
                {
                    if (id == tmp.Tpriecodi)
                    {
                        resultado = tmp.Tprieusutabla;
                    }
                }

            }
            return resultado;
        }

        #region CAMBIOS SIOSEN

        [HttpPost]
        public JsonResult EliminarMensaje(string msgcodi)
        {
            GestorModel model = new GestorModel();
            try
            {
                logic.EliminarMensaje(msgcodi,this.UserName);
                model.Estado = ConstantesSiosein.Estado.Ok;
            }
            catch (Exception)
            {

                model.Estado = ConstantesSiosein.Estado.Error;
            }

            return Json(model);
        }

        #endregion
    }
}
