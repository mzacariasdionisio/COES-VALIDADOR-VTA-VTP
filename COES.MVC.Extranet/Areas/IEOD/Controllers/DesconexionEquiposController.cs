using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.IEOD.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.IEOD.Controllers
{
    public class DesconexionEquiposController : FormatoController
    {
        DesconexionEquiposAppServicio servicio = new DesconexionEquiposAppServicio();
        IEODAppServicio servIeod = new IEODAppServicio();
        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(DesconexionEquiposController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

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

        #endregion

        /// <summary>
        /// View registro de desconexiones
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            if (this.IdModulo == null) return base.RedirectToHomeDefault();

            DesconexionEquiposModel model = new DesconexionEquiposModel();
            bool accesoEmpresa = base.VerificarAccesoAccion(Acciones.AccesoEmpresa, User.Identity.Name);
            var empresas = this.servIeod.ListarEmpresasxTipoEquipos(ConstantesHorasOperacion.CodFamiliasDesconexion);

            if (accesoEmpresa)
            {
                model.ListaEmpresas = empresas;
            }
            else
            {
                var emprUsuario = base.ListaEmpresas.Where(x => empresas.Any(y => x.EMPRCODI == y.Emprcodi)).
                    Select(x => new SiEmpresaDTO()
                    {
                        Emprcodi = x.EMPRCODI,
                        Emprnomb = x.EMPRNOMB
                    });
                if (emprUsuario.Count() > 0)
                {
                    model.ListaEmpresas = emprUsuario.ToList();
                }
                else
                {
                    model.ListaEmpresas = new List<SiEmpresaDTO>(){
                         new SiEmpresaDTO(){
                             Emprcodi = 0,
                             Emprnomb = "No Existe"
                         }
                     };
                }
            }
            model.Fecha = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            model.Subcausacodi = ConstantesDesconexion.DesconexionEquipo;
            return View(model);
        }

        /// <summary>
        /// Lista de Desconexiones
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="sFecha"></param>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Lista(int idEmpresa, string sFecha, int idEnvio)
        {
            DesconexionEquiposModel jsModel = new DesconexionEquiposModel();
            DateTime fecha = DateTime.Now;
            DateTime fechaActual = DateTime.Now;
            if (sFecha != null)
            {
                fecha = DateTime.ParseExact(sFecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            try
            {
                jsModel.ListaEnvios = servicio.GetByCriteriaMeEnvios(idEmpresa, 0, fecha).Where(x => x.Fdatcodi == ConstantesDesconexion.IdFatcodiDesconex).ToList();
                jsModel.ListaDesconexionesAnt = servicio.GetListarIeodCuadroxEmpresa(fecha.AddDays(-1), fecha.AddDays(-1), ConstantesDesconexion.DesconexionEquipo, idEmpresa); // Lista de desconexiones del dia anterior
                jsModel.Lastuser = User.Identity.Name;

                jsModel.EsEmpresaVigente = servFormato.EsEmpresaVigente(idEmpresa, DateTime.Now);

                /// Validación de Envio
                jsModel.PlazoEnvio = this.servIEOD.GetByIdSiPlazoenvioByFdatcodi(ConstantesDesconexion.IdFatcodiDesconex);
                jsModel.PlazoEnvio.Emprcodi = idEmpresa;
                jsModel.PlazoEnvio.IdEnvio = idEnvio;
                if (idEnvio <= 0)// no hay envios anteriores
                {
                    jsModel.ListaDesconexiones = servicio.GetListarIeodCuadroxEmpresa(fecha, fecha, ConstantesDesconexion.DesconexionEquipo, idEmpresa);

                    jsModel.PlazoEnvio.FechaProceso = EPDate.GetFechaIniPeriodo(ParametrosFormato.PeriodoDiario, string.Empty, string.Empty, sFecha, Constantes.FormatoFecha);
                    this.servIEOD.GetSizePlazoEnvio(jsModel.PlazoEnvio);
                    jsModel.PlazoEnvio.TipoPlazo = this.servIEOD.EnvioValidarPlazo(jsModel.PlazoEnvio);
                    jsModel.EnabledDesconexion = jsModel.PlazoEnvio.TipoPlazo != ConstantesEnvio.ENVIO_PLAZO_DESHABILITADO;
                }
                else// envio anterior
                {
                    /// mostrar los datos de un historico de envios
                    var envioAnt = jsModel.ListaEnvios.Find(x => x.Enviocodi == idEnvio);
                    if (envioAnt != null)
                    {
                        jsModel.PlazoEnvio.FechaEnvio = envioAnt.Enviofecha;
                        jsModel.PlazoEnvio.FechaProceso = (DateTime)envioAnt.Enviofechaperiodo;
                        jsModel.PlazoEnvio.TipoPlazo = envioAnt.Envioplazo;
                        jsModel.FechaEnvio = ((DateTime)envioAnt.Enviofecha).ToString(Constantes.FormatoFechaHora);
                    }

                    var ListaDetalleEnvio = servicio.GetByCriteriaMeEnviodets(idEnvio);

                    List<int> listaDetalleEnvioAux = new List<int>();

                    foreach (var entity in ListaDetalleEnvio)
                    {
                        listaDetalleEnvioAux.Add(entity.Envdetfpkcodi);
                    }
                    string strCodRestric = string.Join(",", listaDetalleEnvioAux.ToArray());
                    jsModel.ListaDesconexiones = servicio.GetListarDesconexionesxCodigos(strCodRestric);
                }

                return Json(jsModel);
            }

            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { error = -1, descripcion = ex.Message });
            }
        }

        /// <summary>
        /// Genera la vista para registrar datos de desconexion de equipos
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idTipoCentral"></param>
        /// <param name="idCentral"></param>
        /// <param name="opcion"></param> opcion 0: modificar, 1: nueva
        /// <param name="idEquipo"></param>
        /// <returns></returns>
        public PartialViewResult ViewDesconexion(int idEmpresa, string fechaIni, string fechaFin, int iccodi, int equicodi, string ichorini, string ichorfin, string icdescrip1, string archEnvio)
        {
            DesconexionEquiposModel model = new DesconexionEquiposModel();
            model.ActFiltro = (iccodi > 0) ? "disabled" : ""; // Para activar o desactivar los filtros de tipo de equipo y equipo
            model.Iccodi = iccodi;
            model.Lastuser = User.Identity.Name;
            model.ListaFamilia = servFormato.ListarFamilia();
            DateTime dtfecha = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int[] tipoFamCodi = { 8, 9, 10, 12 };
            model.ListaFamilia = servFormato.ListarFamilia().Where(x => tipoFamCodi.Contains(x.Famcodi)).ToList();
            model.ListaEquipo = servFormato.ObtenerEquiposPorFamilia(idEmpresa, 0);
            if (iccodi == 0) // nuevo registro de desconexión de equipo
            {
                model.Fecha = fechaIni;
                model.FechaFin = dtfecha.AddDays(1).ToString(Constantes.FormatoFecha);
                model.FamcodiSelect = "0";
                model.HoraFin = "00:00:00";
                model.HoraIni = "00:00:00";
            }
            else // edicion de desconexión de equipo
            {
                var equipo = servicio.GetEquipoxId(equicodi);
                model.Fecha = fechaIni;
                model.FechaFin = fechaFin;
                model.HoraIni = ichorini;
                model.HoraFin = ichorfin;
                model.Equicodi = equicodi;
                model.FamcodiSelect = string.Format("{0}{1}{2}", equipo.Famcodi, "/", equipo.Famabrev);
                model.IcDescrip1 = icdescrip1;
                model.Icnombarchenvio = archEnvio;
                //}

            }
            model.Subcausacodi = ConstantesDesconexion.DesconexionEquipo;
            return PartialView(model);
        }

        /// <summary>
        /// Recibe los archivos enviados en los envios
        /// </summary>
        /// <param name="concepcodi"></param>
        /// <param name="tipocomb"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload(int idEquipo, string horaIni, string fecha)
        {

            string sNombreArchivo = "";
            string sNombreArchivoEnvio = "";
            string path = AppDomain.CurrentDomain.BaseDirectory + "Areas/IEOD/Temporal/";
            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string extension = file.FileName.Split('.').Last().ToUpper();
                    sNombreArchivo = ConstantesDesconexion.ArchEnvioDesconexion + idEquipo + "_" + fecha + horaIni + "." + extension;
                    sNombreArchivoEnvio = file.FileName;

                    if (System.IO.File.Exists(path + sNombreArchivo))
                    {
                        System.IO.File.Delete(path + sNombreArchivo);
                    }
                    file.SaveAs(path + sNombreArchivo);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///  Descarga del servidor la restriccion enviada
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivoDesconexion(string archivo, int icccodi, int cambio)
        {
            if (icccodi > 0 && cambio != 1)
            {// viene de BD y no hubo cmabio en el archivo adjunto                
                string fullPath = "Areas/IEOD/Reporte/" + archivo;
                string path = AppDomain.CurrentDomain.BaseDirectory + fullPath;
                return File(path, Constantes.AppExcel, archivo);
            }
            else
            {
                string fullPath = "Areas/IEOD/Temporal/" + archivo;
                string path = AppDomain.CurrentDomain.BaseDirectory + fullPath;
                return File(path, Constantes.AppExcel, archivo);
            }
        }

        /// <summary>
        /// Genera la lista de tipos de equipos de a empresa seleccionada
        /// </summary>
        /// <param name="idsAgente"></param>
        /// <returns></returns>
        public PartialViewResult CargarEquipos(int idTipoEquipo, int idEmpresa, int idequipo, string actFiltro)
        {
            DesconexionEquiposModel model = new DesconexionEquiposModel();
            List<EqEquipoDTO> Lista = new List<EqEquipoDTO>();
            Lista = servFormato.ObtenerEquiposPorFamilia(idEmpresa, idTipoEquipo);
            model.ListaEquipo = Lista;
            model.Equicodi = idequipo;
            model.ActFiltro = actFiltro;
            return PartialView(model);
        }

        /// <summary>
        /// Graba en BD el Envio de informacion
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegistrarEnvioDesconexiones(List<EveIeodcuadroDTO> data, int idEmpresa, string fecha)
        {
            GrabarModel model = new GrabarModel();

            try
            {
                if (!base.IsValidSesion) throw new Exception(Constantes.MensajeSesionExpirado);

                if (!this.servFormato.EsEmpresaVigente(idEmpresa, DateTime.Now))
                {
                    throw new Exception(Constantes.MensajeEmpresaNoVigente);
                }

                SiPlazoenvioDTO plazoEnvio = this.servIEOD.GetByIdSiPlazoenvioByFdatcodi(ConstantesDesconexion.IdFatcodiDesconex);
                plazoEnvio.Emprcodi = idEmpresa;
                plazoEnvio.FechaProceso = EPDate.GetFechaIniPeriodo(ParametrosFormato.PeriodoDiario, string.Empty, string.Empty, fecha, Constantes.FormatoFecha);
                this.servIEOD.GetSizePlazoEnvio(plazoEnvio);
                plazoEnvio.TipoPlazo = this.servIEOD.EnvioValidarPlazo(plazoEnvio);

                if (ConstantesEnvio.ENVIO_PLAZO_DESHABILITADO == plazoEnvio.TipoPlazo)
                    throw new Exception("El envió no está en el Plazo Permitido. El plazo está definido entre " + plazoEnvio.FechaPlazoIni.ToString(ConstantesAppServicio.FormatoFechaFull) + " y " + plazoEnvio.FechaPlazoFuera.ToString(ConstantesAppServicio.FormatoFechaFull));

                List<int> listCodDesconexiones = new List<int>();

                ///////////////Guardar Envio//////////////////////////                
                MeEnvioDTO envio = new MeEnvioDTO();
                envio.Archcodi = 0;
                envio.Emprcodi = idEmpresa;
                envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;
                envio.Fdatcodi = ConstantesDesconexion.IdFatcodiDesconex;

                envio.Enviofecha = DateTime.Now;
                envio.Enviofechaperiodo = plazoEnvio.FechaProceso;
                envio.Envioplazo = plazoEnvio.TipoPlazo;
                envio.Enviofechaini = plazoEnvio.FechaInicio;
                envio.Enviofechafin = plazoEnvio.FechaFin;
                envio.Enviofechaplazoini = plazoEnvio.FechaPlazoIni;
                envio.Enviofechaplazofin = plazoEnvio.FechaPlazoFin;

                envio.Lastdate = DateTime.Now;
                envio.Lastuser = User.Identity.Name;
                envio.Userlogin = User.Identity.Name;
                envio.Formatcodi = 0; // para horas de operacion;
                int idEnvio = servicio.SaveMeEnvio(envio);
                model.IdEnvio = idEnvio;
                ///////////////////////////////////////////////////////

                /////////Guardar Desconexiones////////////////////                
                GuardarDesconexiones(data, plazoEnvio.FechaProceso, idEnvio, ref listCodDesconexiones);
                envio.Enviocodi = idEnvio;
                envio.Cfgenvcodi = -1;
                envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                servicio.UpdateMeEnvio(envio);

                /////////Grabar Detalle de envios////////////////////
                foreach (var entity in listCodDesconexiones)
                {
                    var entityEnvioDet = new MeEnviodetDTO();
                    entityEnvioDet.Enviocodi = idEnvio;
                    entityEnvioDet.Envdetfpkcodi = entity;
                    servicio.SaveMeEnviodet(entityEnvioDet);
                }
                ///////////////////////////////////////////////////////  

                plazoEnvio.IdEnvio = idEnvio;
                plazoEnvio.FechaEnvio = envio.Enviofecha;

                model.PlazoEnvio = plazoEnvio;
                model.Resultado = 1;
                model.FechaEnvio = plazoEnvio.FechaEnvio.Value.ToString(ConstantesAppServicio.FormatoFechaFull);
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
        /// Guardar desconexiones en BD
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="fecha"></param>
        /// <param name="idEnvio"></param>
        /// <param name="listCodDesconexiones"></param>
        public void GuardarDesconexiones(List<EveIeodcuadroDTO> lista, DateTime fecha, int idEnvio, ref List<int> listCodDesconexiones)
        {
            int idNewDesconexion = 0;
            foreach (var obj in lista)
            {

                if (obj.Iccodi < 0) //nueva desconexión de equipo
                {
                    ///Grabar desconexión////////                    
                    idNewDesconexion = servicio.SaveDesconexion(obj);
                    /// eliminamos el archivo de carpeta temporal y lo copiamos en carpeta Reporte
                    string path = AppDomain.CurrentDomain.BaseDirectory + "Areas/IEOD/Temporal/";
                    string pathRepor = AppDomain.CurrentDomain.BaseDirectory + "Areas/IEOD/Reporte/";
                    if (System.IO.File.Exists(path + obj.Icnombarchfisico))
                    {
                        System.IO.File.Move(path + obj.Icnombarchfisico, pathRepor + obj.Icnombarchfisico);
                        //System.IO.File.Delete(path + obj.Icnombarchfisico);
                    }
                    listCodDesconexiones.Add(idNewDesconexion);
                }
                else // Modificar desconexion de equipo
                {
                    if (obj.opCrud == 2) // si es actualizacion
                    {
                        if (obj.CambioArchivo == 1) // si ha cambiado el archivo
                        {
                            string path = AppDomain.CurrentDomain.BaseDirectory + "Areas/IEOD/Temporal/";
                            string pathRepor = AppDomain.CurrentDomain.BaseDirectory + "Areas/IEOD/Reporte/";
                            //eliminamos el archivo de la carpeta reporte
                            if (System.IO.File.Exists(pathRepor + obj.IcnombarchfisicoAnt))
                            {
                                System.IO.File.Delete(pathRepor + obj.IcnombarchfisicoAnt);
                            }


                            /// eliminamos el archivo de carpeta temporal y lo copiamos en carpeta Reporte
                            if (System.IO.File.Exists(path + obj.Icnombarchfisico))
                            {
                                System.IO.File.Move(path + obj.Icnombarchfisico, pathRepor + obj.Icnombarchfisico);
                                //System.IO.File.Delete(path + obj.Icnombarchfisico);
                            }

                        }
                        servicio.UpdateDesconexion(obj);
                        listCodDesconexiones.Add(obj.Iccodi);

                    }
                    if (obj.opCrud == -1)// eliminado logico
                    {
                        servicio.DeleteLogicoDesconexion(obj.Iccodi);
                    }
                    if (obj.opCrud == 0) // si no hay cambios registrar en el envio la desconexion 
                    {
                        listCodDesconexiones.Add(obj.Iccodi);
                    }
                }


            }
        }

        #region UTIL

        private string GeneraStrCodigos(List<int> listCodHop)
        {
            string cadena = String.Empty;

            for (int i = 0; i < listCodHop.Count; i++)
            {
                if (i == listCodHop.Count - 1) // si es el ultimo
                    cadena += listCodHop[i].ToString();
                else
                    cadena += listCodHop[i].ToString() + ",";
            }
            return cadena;
        }

        #endregion

    }
}
