using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Subastas.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Subastas;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Subastas.Controllers
{
    public class OfertaController : BaseController
    {
        SubastasAppServicio servicio = new SubastasAppServicio();

        #region Declaración de variables

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }

        public OfertaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        #region Consulta de Ofertas 

        /// <summary>
        /// Muestra ventana principal
        /// </summary>}
        public ActionResult Defecto()
        {
            OfertaModel model = new OfertaModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);

                EmpresaModel empmodel = new EmpresaModel();
                Log.Info("Lista de Empresas - ListEmpresaSmaUserEmpresa");
                empmodel.ListaEmpresaUsuarios = this.servicio.ListEmpresaSmaUserEmpresa(-1);
                empmodel.ListaEmpresaUsuarios.Insert(0, empmodel.ListaComboTodosEmpresa);
                ViewData["empresa"] = new SelectList(empmodel.ListaEmpresaUsuarios, "Emprcodi", "Emprnomb", -1);

                List<SmaUserEmpresaDTO> listUser = this.servicio.ListSmaUserEmpresa(-1)
                                                    .GroupBy(s => s.Usercode)
                                                    .Select(grp => grp.FirstOrDefault())
                                                    .OrderBy(s => s.Username)
                                                    .ToList();

                listUser.Insert(0, empmodel.ListaComboTodosUsuario);
                ViewData["username"] = new SelectList(listUser, "Usercode", "Username", -1);

                Log.Info("Lista de URS - ListSmaUrsModoOperacions_Urs");
                ViewData["urs"] = new SelectList(this.servicio.ListSmaUrsModoOperacions_Urs(-1), "Urscodi", "Ursnomb", "Todos");

                ViewBag.fecha = (DateTime.Now).ToString(ConstantesAppServicio.FormatoMes);
                ViewBag.fechaFin = (DateTime.Now).ToString(ConstantesAppServicio.FormatoMes);

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        /// <summary>
        /// Muestra ventana principal
        /// </summary>}
        public ActionResult Diaria()
        {
            OfertaModel model = new OfertaModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);

                EmpresaModel empmodel = new EmpresaModel();
                empmodel.ListaEmpresaUsuarios = this.servicio.ListEmpresaSmaUserEmpresa(-1);
                empmodel.ListaEmpresaUsuarios.Insert(0, empmodel.ListaComboTodosEmpresa);
                ViewData["empresa"] = new SelectList(empmodel.ListaEmpresaUsuarios, "Emprcodi", "Emprnomb", -1);

                List<SmaUserEmpresaDTO> listUser = this.servicio.ListSmaUserEmpresa(-1)
                                                    .GroupBy(s => s.Usercode)
                                                    .Select(grp => grp.FirstOrDefault())
                                                    .OrderBy(s => s.Username)
                                                    .ToList();

                listUser.Insert(0, empmodel.ListaComboTodosUsuario);
                ViewData["username"] = new SelectList(listUser, "Usercode", "Username", -1);

                //Log.Info("Lista de URS - ListSmaUrsModoOperacions_Urs");
                ViewData["urs"] = new SelectList(this.servicio.ListSmaUrsModoOperacions_Urs(-1), "Urscodi", "Ursnomb", "Todos");

                ViewBag.fecha = (DateTime.Now).ToString(ConstantesAppServicio.FormatoFecha);
                ViewBag.fechaFin = (DateTime.Now).ToString(ConstantesAppServicio.FormatoFecha);

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        /// <summary>
        /// Filto para cargar Lista de Empresas por Usuario
        /// </summary>
        [HttpPost]
        public JsonResult ListarUsuarios(int tipo)
        {
            //Log.Info("Listando de Empresa por Usuario - ListSmaUserEmpresa");
            List<SmaUserEmpresaDTO> Usuario = this.servicio.ListSmaUserEmpresa(tipo)
                                            .OrderBy(s => s.Username).ToList(); ;
            return Json(Usuario);
        }

        /// <summary>
        /// Filtro para cargar Lista de URS por Usuario
        /// </summary>
        [HttpPost]
        public JsonResult ListarUrs(int tipo)
        {
            //Log.Info("Listando de URS por Usuario - ListSmaUrsModoOperacions_InUrs");
            List<SmaUrsModoOperacionDTO> urs = this.servicio.ListSmaUrsModoOperacions_InUrs(tipo);
            return Json(urs);
        }

        /// <summary>
        /// Muestra la grilla excel handsontable
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcel(FormCollection collection)
        {
            OfertaModel modelResult = new OfertaModel();
            modelResult.ListaTab = new List<HandsonModel>();

            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

                int empresaCodi = Convert.ToInt32(collection["empresaCodi"]);
                int userCode = Convert.ToInt32(collection["usercode"]);
                int tipoOferta = Convert.ToInt32(collection["tipooferta"]);
                string fechaString = collection["oferfechaenvio"];
                string fechafinString = collection["oferfechaenviofin"];
                int oferCodi = Convert.ToInt32(collection["ofercodi"]);
                int opcionReporte = Convert.ToInt32(collection["opcion"]);
                string listUrs = collection["urs[]"];


                //
                empresaCodi = (empresaCodi == 0) ? ConstantesSubasta.Todos : empresaCodi;
                oferCodi = (oferCodi == 0) ? ConstantesSubasta.Todos : oferCodi;
                userCode = (userCode == 0) ? ConstantesSubasta.Todos : userCode;
                DateTime fecha = ConstantesSubasta.OfertipoDefecto == tipoOferta ? EPDate.GetFechaIniPeriodo(5, fechaString, string.Empty, string.Empty, string.Empty) : DateTime.ParseExact(fechaString, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechafin = ConstantesSubasta.OfertipoDefecto == tipoOferta ? EPDate.GetFechaIniPeriodo(5, fechafinString, string.Empty, string.Empty, string.Empty) : DateTime.ParseExact(fechafinString, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                listUrs = (listUrs == null) ? ConstantesSubasta.EstadoDefecto : listUrs;

                string oferfechaenvioDesc = string.Empty;
                string oferCodenvio = string.Empty;
                string estado = string.Empty;
                if (oferCodi > 0)
                {
                    var objEnvio = this.servicio.GetByIdSmaOferta(oferCodi);
                    if (objEnvio != null)
                    {
                        fecha = objEnvio.Oferfechainicio.Value;
                        fechafin = objEnvio.Oferfechafin.Value;
                        oferCodenvio = objEnvio.Ofercodenvio;
                        oferfechaenvioDesc = objEnvio.Oferfechaenvio.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
                        estado = objEnvio.Oferestado;
                    }
                    else
                    {
                        throw new Exception("Código de envío inválido.");
                    }
                }

                modelResult.OferCodi = oferCodi;
                modelResult.OferCodenvio = oferCodenvio;
                modelResult.OferfechaenvioDesc = oferfechaenvioDesc;
                modelResult.OferEstado = estado;
                modelResult.ListaTab = this.servicio.GenerarGrillaConsultaOferta(opcionReporte, tipoOferta, fecha, fechafin, userCode, oferCodi, empresaCodi, listUrs);
                modelResult.HtmlListaEnvio = this.servicio.GenerarHtmlListaEnvios(tipoOferta, fecha, fechafin, userCode, empresaCodi, listUrs);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                modelResult.Resultado = -1;
                modelResult.Mensaje = ex.Message;
                modelResult.Detalle = ex.StackTrace;
            }

            return Json(modelResult);
        }

        public virtual ActionResult DescargarReporte()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + NombreArchivo.ReporteOfertasDiarias;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteOfertasDiarias);
        }

        [HttpPost]
        public JsonResult ExportarReporte(FormCollection collection)
        {
            OfertaModel modelResult = new OfertaModel();

            try
            {
                base.ValidarSesionJsonResult();

                int empresaCodi = Convert.ToInt32(collection["empresaCodi"]);
                int userCode = Convert.ToInt32(collection["usercode"]);
                int tipoOferta = Convert.ToInt32(collection["tipooferta"]);
                string fechaString = collection["oferfechaenvio"];
                string fechafinString = collection["oferfechaenviofin"];
                int oferCodi = Convert.ToInt32(collection["ofercodi"]);
                int opcionReporte = Convert.ToInt32(collection["opcion"]);
                string listUrs = collection["urs[]"];

                empresaCodi = (empresaCodi == 0) ? ConstantesSubasta.Todos : empresaCodi;
                oferCodi = (oferCodi == 0) ? ConstantesSubasta.Todos : oferCodi;
                userCode = (userCode == 0) ? ConstantesSubasta.Todos : userCode;
                DateTime fecha = ConstantesSubasta.OfertipoDefecto == tipoOferta ? EPDate.GetFechaIniPeriodo(5, fechaString, string.Empty, string.Empty, string.Empty) : DateTime.ParseExact(fechaString, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechafin = ConstantesSubasta.OfertipoDefecto == tipoOferta ? EPDate.GetFechaIniPeriodo(5, fechafinString, string.Empty, string.Empty, string.Empty) : DateTime.ParseExact(fechafinString, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                listUrs = listUrs ?? ConstantesSubasta.EstadoDefecto;

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string nombreArchivo = NombreArchivo.ReporteOfertasDiarias;

                this.servicio.GenerarArchivoExcelConsultaOferta(ruta, nombreArchivo, opcionReporte, tipoOferta, fecha, fechafin, userCode, oferCodi, empresaCodi, listUrs);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                modelResult.Resultado = -1;
                modelResult.Mensaje = ex.Message;
                modelResult.Detalle = ex.StackTrace;
            }

            return Json(modelResult);
        }

        #endregion

        #region RESERVA SECUNDARIA

        public ActionResult ReservaSecundaria()
        {
            OfertaModel model = new OfertaModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);

                model.FechaInicial = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);
                model.FechaFinal = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Reserva Secundaria automatica
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaReservaSecundaria(string fechaInicial, string fechaFinal)
        {
            DateTime fecInicial = DateTime.ParseExact(fechaInicial, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fecFinal = DateTime.ParseExact(fechaFinal, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            this.servicio.CargarListadoReservaSecundaria(out TablaReporte dataTablaSubir, out TablaReporte dataTablaBajar, fecInicial, fecFinal);


            OfertaModel model = new OfertaModel();

            model.ResultadoSubir = this.servicio.ListadoReservaSecundariaHTML(dataTablaSubir, "subir");
            model.ResultadoBajar = this.servicio.ListadoReservaSecundariaHTML(dataTablaBajar, "bajar");


            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Generar excel de reserva secundaria
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public JsonResult GenerarExcelReservaSecundaria(string fechaInicial, string fechaFinal)
        {
            var model = new OfertaModel();
            DateTime fecInicial = DateTime.ParseExact(fechaInicial, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fecFinal = DateTime.ParseExact(fechaFinal, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string nombreArchivo = NombreArchivo.ReporteReservaSecundaria;

                this.servicio.GenerarArchivoExcelReservaSecundaria(ruta, nombreArchivo, fecInicial, fecFinal);

                model.Detalle = nombreArchivo;
                model.Resultado = 1;
            }
            catch (Exception ex)
            {
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                Log.Error(NameController, ex);
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Permite descargar archivo excel del Reporte Ejecutivo Mensual
        /// </summary>
        /// <param name="nameFile"></param>
        /// <returns></returns>
        public virtual ActionResult ExportarReporteXls(string nameFile)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string fullPath = ruta + nameFile;
            //return File(fullPath, ConstantesAppServicio.ExtensionExcel, nameFile); //se almacena el arch descargado

            // descargamos y borramos el archivo
            byte[] buffer = null;
            if (System.IO.File.Exists(fullPath))
            {
                buffer = System.IO.File.ReadAllBytes(fullPath);
                System.IO.File.Delete(fullPath);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nameFile);

        }

        #endregion

        #region OFERTAS POR DEFECTO ACTIVADAS

        public ActionResult OfertasDefectoActivadas()
        {
            OfertaModel model = new OfertaModel();

            try
            {
                model.ListaEmpresaUsuarios = this.servicio.ListEmpresaSmaUserEmpresa(-1);
                model.ListaUrsModoOperacion = servicio.ListSmaUrsModoOperacions_Urs(-1);
                model.FechaInicial = DateTime.Now.AddDays(1).ToString(ConstantesAppServicio.FormatoFecha);
                model.FechaFinal = DateTime.Now.AddDays(1).ToString(ConstantesAppServicio.FormatoFecha);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Reserva Secundaria automatica
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult ListarOfertasDefectoActivadas(string fechaInicial, string fechaFinal, string emprCodi, string urs, string fuente)
        {
            OfertaModel model = new OfertaModel();
            try
            {
                DateTime fecInicial = DateTime.ParseExact(fechaInicial, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fecFinal = DateTime.ParseExact(fechaFinal, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                this.servicio.CargarListarOfertasDefectoActivadas(fecInicial, fecFinal, emprCodi, urs, fuente, out List<SmaOfertaDTO> listaOfertaSubir, out List<SmaOfertaDTO> listaOfertaBajar);

                if (listaOfertaSubir.Any() || listaOfertaBajar.Any())
                    model.FlagTieneDatos = true; // hay datos

                model.ListaOfertaSubir = listaOfertaSubir;
                model.ListaOfertaBajar = listaOfertaBajar;

                //model.ListaTab = this.servicio.CargarListarOfertasDefectoActivadas2(fecInicial, fecFinal, emprCodi, urs, fuente);

                model.Resultado = 1;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Generar excel de reserva secundaria
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public JsonResult GenerarExcelOfertasDefectoActivadas(string fechaInicial, string fechaFinal, string emprCodi, string urs, string fuente)
        {
            var model = new OfertaModel();
            DateTime fecInicial = DateTime.ParseExact(fechaInicial, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fecFinal = DateTime.ParseExact(fechaFinal, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string nameFile = servicio.GenerarArchivoExcelOfertas(ruta, pathLogo, fecInicial, fecFinal, emprCodi, urs, fuente, out bool existeDatos);

                model.FlagTieneDatos = existeDatos;
                model.NombreArchivo = nameFile;
                model.Resultado = 1;
            }
            catch (Exception ex)
            {
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                Log.Error(NameController, ex);
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Permite descargar el archivo Excel al explorador
        /// </summary>
        /// <param name="file">Nombre del archivo</param>
        /// <returns>Archivo</returns>
        public virtual ActionResult AbrirArchivo(string file)
        {
            base.ValidarSesionJsonResult();
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + file;
            return File(path, Constantes.AppExcel, file);
        }

        #endregion

    }
}