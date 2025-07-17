using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.IEOD.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Medidores;
using COES.Servicios.Aplicacion.TiempoReal;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace COES.MVC.Intranet.Areas.IEOD.Controllers
{
    public class ConfiguracionController : BaseController
    {
        MedidoresAppServicio servMedidores = new MedidoresAppServicio();
        ScadaSp7AppServicio servScada = new ScadaSp7AppServicio();
        IEODAppServicio servIeod = new IEODAppServicio();
        EventoAppServicio servEvento = new EventoAppServicio();

        #region Declaración de variables
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ConfiguracionController));
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

        #endregion

        #region Equivalencia de puntos de medicion y scada
        //
        // GET: /IEOD/Configuracion/Equivalencia
        public ActionResult Equivalencia(int? origen)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            ConfiguracionModel model = new ConfiguracionModel();
            model.ListaOrigenlectura = this.servIeod.GetByCriteriaMeorigenlectura().Where(x => (x.Origlectcodi != -1 && x.Origlectcodi != 0)).OrderBy(x => x.Origlectnombre).ToList();
            model.ListaMedida = this.servMedidores.ListaUnidadTr();
            model.Origen = origen ?? 0;

            return View(model);
        }

        /// <summary>
        /// Permite obtener la tabla de equivalencias
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarReporteEquivalencia(int idEmpresa, int idCentral, int medida)
        {
            string url = Url.Content("~/");
            string resultado = this.servMedidores.ReporteEquivalenciaPtomedicionCanalHtml(idEmpresa, idCentral, medida, url);
            return Json(resultado);
        }

        /// <summary>
        /// Permite obtener las centrales de una empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerCentrales(int idEmpresa)
        {
            string origlectcodi = ConstantesIEOD.OrigLectcodiIEODpr5 + "," + ConstantesIEOD.OrigLectcodiIEODpr16;
            var centrales = this.servMedidores.ObtenerListaCentralByOriglectcodiAndEmpresa(origlectcodi, ConstantesIEOD.FamCentral, idEmpresa, -1);
            return Json(centrales);
        }

        /// <summary>
        /// Mostrar registro de nueva equivalencia
        /// </summary>
        [HttpPost]
        public PartialViewResult NuevoEquivalencia()
        {
            ConfiguracionModel modelo = new ConfiguracionModel();

            List<int> listaFamiliaEquivalencia = new List<int>() { 2, 3, 36, 38, 37, 39, 8, 9, 10 };

            modelo.ListaOrigenlectura = this.servIeod.GetByCriteriaMeorigenlectura().Where(x => (x.Origlectcodi != -1 && x.Origlectcodi != 0)).OrderBy(x => x.Origlectnombre).ToList();
            modelo.ListaTrEmpresa = this.servScada.ListarEmpresaCanalBdTreal().Where(x => x.Emprcodi > 0).ToList();
            return PartialView(modelo);
        }

        /// <summary>
        /// Registrar Equivalencia
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="horaInicio"></param>
        /// <param name="horaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegistrarEquivalencia(string dataLista)
        {
            try
            {
                base.ValidarSesionJsonResult();

                JavaScriptSerializer serialize = new JavaScriptSerializer();
                List<MePtomedcanalDTO> objData = serialize.Deserialize<List<MePtomedcanalDTO>>(dataLista);

                string msj = string.Empty;
                foreach (var p in objData)
                {
                    p.Pcanestado = ConstantesAppServicio.Activo;
                    p.Pcanusucreacion = User.Identity.Name;
                    p.Pcanfeccreacion = DateTime.Now;
                    //validacion de existencia
                    var reg = this.servMedidores.GetByIdMePtomedcanal(p.Canalcodi, p.Ptomedicodi, p.Tipoinfocodi);
                    if (reg != null)
                    {
                        msj += "Ya existe la equivalencia [Código de Pto Medición=" + reg.Ptomedicodi + ",Código de canal=" + reg.Canalcodi + "]" + "\n";
                    }
                }

                if (msj != string.Empty)
                {
                    return Json(msj);
                }

                foreach (var p in objData)
                {
                    this.servMedidores.SaveMePtomedcanal(p);
                }

                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Mostrar registro de ver equivalencia
        /// </summary>
        [HttpPost]
        public PartialViewResult VerEquivalencia(int canalcodi, int ptomedicodi, int tipoinfocodi)
        {
            ConfiguracionModel modelo = new ConfiguracionModel();

            modelo.PtomedcanalActual = this.servMedidores.GetByIdMePtomedcanal(canalcodi, ptomedicodi, tipoinfocodi);

            return PartialView(modelo);
        }

        /// <summary>
        /// Mostrar registro de Editar equivalencia
        /// </summary>
        [HttpPost]
        public PartialViewResult EditarEquivalencia(int canalcodi, int ptomedicodi, int tipoinfocodi)
        {
            ConfiguracionModel modelo = new ConfiguracionModel();

            modelo.PtomedcanalActual = this.servMedidores.GetByIdMePtomedcanal(canalcodi, ptomedicodi, tipoinfocodi);
            modelo.ListaEstado = Util.ListaEstadoAll().Where(x => x.EstadoCodigo == ConstantesAppServicio.Activo || x.EstadoCodigo == ConstantesAppServicio.Anulado).ToList();

            return PartialView(modelo);
        }

        /// <summary>
        /// Actualizar Equivalencia Equivalencia
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="horaInicio"></param>
        /// <param name="horaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarEquivalencia(int canalcodi, int ptomedicodi, int tipoinfocodi, string estado, decimal check)
        {
            try
            {
                base.ValidarSesionJsonResult();

                MePtomedcanalDTO reg = this.servMedidores.GetByIdMePtomedcanal(canalcodi, ptomedicodi, tipoinfocodi);
                reg.Pcanestado = estado;
                reg.Pcanusumodificacion = User.Identity.Name;
                reg.Pcanfecmodificacion = DateTime.Now;
                reg.Pcanfactor = check;

                this.servMedidores.UpdateMePtomedcanal(reg);

                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Eliminar Equivalencia
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="horaInicio"></param>
        /// <param name="horaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarEquivalencia(int canalcodi, int ptomedicodi, int tipoinfocodi)
        {
            try
            {
                MePtomedcanalDTO p = this.servMedidores.GetByIdMePtomedcanal(canalcodi, ptomedicodi, tipoinfocodi);
                if (p != null)
                {
                    this.servMedidores.DeleteMePtomedcanal(p.Canalcodi, p.Ptomedicodi, p.Tipoinfocodi);
                }

                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Listar empresa por origen lectura
        /// </summary>
        /// <param name="idTrEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEmpresaXOrigenLectura(int origlectcodi)
        {
            var resultado = this.servMedidores.ObtenerListaEmpresaByOriglectcodi(origlectcodi);

            return Json(resultado);
        }
        /// <summary>
        /// Listar familia por origen lectura y empresa
        /// </summary>
        /// <param name="idTrEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarFamiliaXOrigenLecturaEmpresa(int origlectcodi, int emprcodi)
        {
            var resultado = this.servMedidores.ObtenerFamiliaPorOrigenLecturaEquipo(origlectcodi, emprcodi);
            return Json(resultado);
        }
        /// <summary>
        /// Listar los puntos de medición por familia y empresa
        /// </summary>
        /// <param name="idTrEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarPtomedicion(int idEmpresa, int famcodi, string origlectcodi)
        {
            var resultado = this.servMedidores.ObtenerListaPtomedicionByOriglectcodiEmpresa(origlectcodi, famcodi.ToString(), idEmpresa);
            return Json(resultado);
        }

        /// <summary>
        /// Listar las unidades por zona
        /// </summary>
        /// <param name="idTrEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarUnidadScada(int zonacodi)
        {
            var resultado = this.servMedidores.ListarUnidadPorZona(zonacodi);
            return Json(resultado);
        }

        /// <summary>
        /// Listar las zonas de la empresa
        /// </summary>
        /// <param name="idTrEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarZona(int idTrEmpresa)
        {
            var resultado = this.servScada.ListTrZonaSp7sByEmpresaBdTreal(idTrEmpresa).OrderBy(x => x.Zonanomb).ToList();
            return Json(resultado);
        }

        /// <summary>
        /// Listar canal por zona y unidad
        /// </summary>
        /// <param name="idTrZona"></param>
        /// <param name="idTrUnidad"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarCanal(int idTrEmpresa, int idTrZona, string unidad)
        {
            List<TrCanalSp7DTO> resultado = this.servScada.ListTrCanalSp7sByZonaAndUnidad(ConstantesAppServicio.ParametroDefecto, idTrEmpresa, idTrZona, unidad)
                                                .OrderBy(x => x.Canalnomb).ToList();

            foreach (var reg in resultado)
            {
                reg.Canalnomb = reg.Canaliccp + " " + reg.Canalnomb;
            }

            return Json(resultado);
        }

        /// <summary>
        /// Generar reporte de equivalencia
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <param name="medida"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GeneraReporteExcelEquivalencia(int idEmpresa, int idCentral, int medida)
        {
            int indicador = 1;

            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioReporteEquivalencia;
                List<MePtomedcanalDTO> listaData = this.servMedidores.ListarEquivalenciaPtomedicionCanal(idEmpresa.ToString(), idCentral, medida, ConstantesAppServicio.ParametroDefecto).ToList();
                if (listaData.Count > 0)
                {
                    this.servMedidores.GenerarArchivoExcelEquivalencia(listaData, ruta + ConstantesIEOD.RptExcelEquivalencia, ruta + ConstantesIEOD.NombreLogoCoes);
                    indicador = 1;
                }
                else
                {
                    indicador = 2;
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Permite exportar el reporte general a formato excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarReporteEquivalencia()
        {
            return DescargarArchivoTemporalYEliminarlo(AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioReporteEquivalencia, ConstantesIEOD.RptExcelEquivalencia);
        }

        /// <summary>
        /// Busca el punto de medición por ptomedicodi
        /// </summary>
        /// <param name="idTrEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPtoMedicion(int ptomedicodi)
        {
            ConfiguracionModel model = new ConfiguracionModel();
            try
            {
                var entity = this.servMedidores.GetByIdPtoMedicion(ptomedicodi);

                if (entity != null)
                {
                    var listaEmpresas = this.servMedidores.ObtenerListaEmpresaByOriglectcodi((int)entity.Origlectcodi);
                    var listaTipoEquipo = this.servMedidores.ObtenerFamiliaPorOrigenLecturaEquipo((int)entity.Origlectcodi, (int)entity.Emprcodi);
                    var listaPtoMedicion = new List<MePtomedicionDTO>();
                    if (entity.Emprcodi.GetValueOrDefault(0) <= 0 || entity.Equicodi.GetValueOrDefault(0) <= 0 || entity.Famcodi <= 0)
                        listaPtoMedicion.Add(entity);
                    else
                    {
                        listaPtoMedicion = this.servMedidores.ObtenerListaPtomedicionByOriglectcodiEmpresa(Convert.ToString((int)entity.Origlectcodi), entity.Famcodi.ToString(), (int)entity.Emprcodi);
                    }

                    //debe estar siempre el punto a buscar
                    if (listaPtoMedicion.Find(x => x.Ptomedicodi == ptomedicodi) == null)
                    {
                        listaPtoMedicion.Add(entity);
                    }

                    model.PtoMedicion = entity;
                    model.ListaEmpresas = listaEmpresas;
                    model.ListaFamilia = listaTipoEquipo;
                    model.ListaPtomedicion = listaPtoMedicion;

                    model.Resultado = 1;
                }
                else
                {
                    model.Resultado = -1;
                }
                return Json(model);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(model);
            }
        }

        /// <summary>
        /// Busca el resgistro por el canalcodi
        /// </summary>
        /// <param name="canalcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetTrCanalSp7(int canalcodi)
        {
            ConfiguracionModel model = new ConfiguracionModel();
            try
            {
                var entity = this.servScada.GetByIdTrCanalSp7BdTreal(canalcodi);


                if (entity != null)
                {

                    var listaZona = this.servScada.ListTrZonaSp7sByEmpresaBdTreal((int)entity.Emprcodi).OrderBy(x => x.Zonanomb).ToList();
                    List<TrCanalSp7DTO> listaCanal = this.servScada.ListTrCanalSp7sByZonaAndUnidad(ConstantesAppServicio.ParametroDefecto, (int)entity.Emprcodi, (int)entity.Zonacodi, entity.Canalunidad)
                                                .OrderBy(x => x.Canalnomb).ToList();
                    foreach (var reg in listaCanal)
                    {
                        reg.Canalnomb = reg.Canaliccp + " " + reg.Canalnomb;
                    }
                    var listarUnidadScada = this.servMedidores.ListarUnidadPorZona((int)entity.Zonacodi);

                    model.ListaTrZona = listaZona;
                    model.ListarUnidadPorZona = listarUnidadScada;
                    model.ListTrCanal = listaCanal;

                    var lista = this.servMedidores.ListaUnidadTr();
                    var obj = lista.Find(x => x.Canalunidad == entity.Canalunidad);
                    if (obj != null) entity.Tipoinfocodi = obj.Tipoinfocodi;

                    model.TrCanal = entity;
                    model.Resultado = 1;
                }
                else
                {
                    model.Resultado = -1;
                }
                return Json(model);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(model);
            }

        }

        #endregion

        #region Equivalencia de Equipos y Scada

        //
        // GET: /IEOD/Configuracion/RelacionEquipoScada
        public ActionResult EquivalenciaEquipoScada()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            ConfiguracionModel model = new ConfiguracionModel();
            model.ListaAreasCoes = servIeod.ListFwAreas().Where(x => x.Areacode == 1 || x.Areacode == 7).ToList();
            model.ListaEmpresas = this.servIeod.ListarEmpresas().Where(x => x.Emprcodi > 0).ToList();
            model.ListaFamilia = this.servIeod.ListarFamilia().Where(x => x.Famcodi > 0).OrderBy(x => x.Famnomb).ToList();
            model.ListaMedida = this.servMedidores.ListaUnidadTr();

            return View(model);
        }

        /// <summary>
        /// Permite obtener la tabla de equivalencias
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFamilia"></param>
        /// <param name="medida"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarReporteEquivalenciaEquipoScada(int areacode, int idEmpresa, int idFamilia, int medida)
        {
            if (!base.IsValidSesion) throw new Exception(Constantes.MensajeSesionExpirado);

            string url = Url.Content("~/");
            string resultado = "";

            try
            {
                resultado = this.servMedidores.ReporteEquivalenciaEquipoCanalHtml(areacode, idEmpresa, idFamilia, medida, url);
            }
            catch (Exception ex)
            {
                var jsonError = Json(ex.Message);
                jsonError.MaxJsonLength = Int32.MaxValue;

                return jsonError;
            }

            var json = Json(resultado);
            json.MaxJsonLength = Int32.MaxValue;

            return json;
        }

        /// <summary>
        /// Mostrar registro de nueva equivalencia
        /// </summary>
        [HttpPost]
        public PartialViewResult NuevoEquivalenciaEquipoScada()
        {
            ConfiguracionModel model = new ConfiguracionModel();

            model.ListaAreasCoes = servIeod.ListFwAreas().Where(x => x.Areacode == 1 || x.Areacode == 7).OrderBy(X => X.Areaabrev).ToList();
            model.ListaEmpresas = this.servIeod.ListarEmpresas().Where(x => x.Emprcodi > 0).ToList();
            model.ListaFamilia = this.servIeod.ListarFamilia().Where(x => x.Famcodi > 0).OrderBy(x => x.Famnomb).ToList();
            model.ListaTrEmpresa = this.servScada.ListarEmpresaCanalBdTreal().Where(x => x.Emprcodi > 0).ToList();
            model.ListaMedida = this.servMedidores.ListaUnidadTr();

            return PartialView(model);
        }

        /// <summary>
        /// Registrar Equivalencia
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="horaInicio"></param>
        /// <param name="horaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegistrarEquivalenciaEquipoScada(string dataLista)
        {
            try
            {
                base.ValidarSesionJsonResult();

                JavaScriptSerializer serialize = new JavaScriptSerializer();
                List<EqEquicanalDTO> objData = serialize.Deserialize<List<EqEquicanalDTO>>(dataLista);

                string msj = string.Empty;
                foreach (var p in objData)
                {
                    p.Ecanestado = ConstantesAppServicio.Activo;
                    p.Ecanusucreacion = User.Identity.Name;
                    p.Ecanfeccreacion = DateTime.Now;

                    //validacion de existencia
                    var reg = this.servMedidores.GetByIdEqEquicanal(p.Areacode, p.Canalcodi, p.Equicodi, p.Tipoinfocodi);
                    if (reg != null)
                    {
                        msj += "Ya existe la equivalencia [Código de Equipo=" + reg.Equicodi + ",Código de canal=" + reg.Canalcodi + "]" + "\n";
                    }
                }

                if (msj != string.Empty)
                {
                    return Json(msj);
                }

                foreach (var p in objData)
                {
                    this.servMedidores.SaveEqEquicanal(p);
                }

                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Mostrar registro de ver equivalencia
        /// </summary>
        [HttpPost]
        public PartialViewResult VerEquivalenciaEquipoScada(int areacode, int canalcodi, int equicodi, int tipoinfocodi)
        {
            ConfiguracionModel modelo = new ConfiguracionModel();

            modelo.EquicanalActual = this.servMedidores.GetByIdEqEquicanal(areacode, canalcodi, equicodi, tipoinfocodi);

            return PartialView(modelo);
        }

        /// <summary>
        /// Mostrar registro de Editar equivalencia
        /// </summary>
        [HttpPost]
        public PartialViewResult EditarEquivalenciaEquipoScada(int areacode, int canalcodi, int equicodi, int tipoinfocodi)
        {
            ConfiguracionModel modelo = new ConfiguracionModel();

            modelo.EquicanalActual = this.servMedidores.GetByIdEqEquicanal(areacode, canalcodi, equicodi, tipoinfocodi);
            modelo.ListaEstado = Util.ListaEstadoAll();

            return PartialView(modelo);
        }

        /// <summary>
        /// Actualizar Equivalencia
        /// </summary>
        /// <param name="canalcodi"></param>
        /// <param name="equicodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="estado"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarEquivalenciaEquipoScada(int areacode, int canalcodi, int equicodi, int tipoinfocodi, string estado, decimal check)
        {
            try
            {
                base.ValidarSesionJsonResult();

                EqEquicanalDTO reg = this.servMedidores.GetByIdEqEquicanal(areacode, canalcodi, equicodi, tipoinfocodi);
                reg.Ecanestado = estado;
                reg.Ecanusumodificacion = User.Identity.Name;
                reg.Ecanfecmodificacion = DateTime.Now;
                reg.Ecanfactor = check;

                this.servMedidores.UpdateEqEquicanal(reg);

                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Eliminar Equivalencia
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="horaInicio"></param>
        /// <param name="horaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarEquivalenciaEquipoScada(int areacode, int canalcodi, int equicodi, int tipoinfocodi)
        {
            try
            {
                base.ValidarSesionJsonResult();

                EqEquicanalDTO p = this.servMedidores.GetByIdEqEquicanal(areacode, canalcodi, equicodi, tipoinfocodi);
                if (p != null)
                {
                    this.servMedidores.DeleteEqEquicanal(p.Areacode, p.Canalcodi, p.Equicodi, p.Tipoinfocodi, User.Identity.Name);
                }

                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <param name="medida"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GeneraReporteExcelEquivalenciaEquipoScada(int areacode, int idEmpresa, int idFamilia, int medida)
        {
            int indicador = 1;

            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioReporteEquivalencia;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                List<EqEquicanalDTO> listaData = this.servMedidores.ListarEquivalenciaEquipoCanal(areacode, idEmpresa, idFamilia, medida);
                this.servMedidores.GenerarArchivoExcelEquivalenciaEquipoCanal(listaData, ruta + ConstantesIEOD.RptExcelEquivalencia, pathLogo);
                indicador = 1;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Listar los equios por familia y empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEquipo(int idEmpresa, int famcodi)
        {
            List<EqEquipoDTO> resultado = this.servMedidores.ObtenerEquiposPorFamilia(idEmpresa, famcodi);
            return Json(resultado);
        }

        #endregion

        #region Búsqueda de Equipos

        /// <summary>
        /// View Busqueda Linea de transmision
        /// </summary>
        /// <returns></returns>
        public PartialViewResult BusquedaEquipo(int? filtroFamilia = 0)
        {
            HorasOperacionModel model = new HorasOperacionModel();
            if (filtroFamilia == 0)
            {
                model.ListaEmpresas = this.servIeod.ListarEmpresasxTipoEquipos(this.ListarFamiliaByFiltro(0, 0)).ToList();

                if (model.ListaEmpresas.Count() > 0)
                {
                    model.ListaEmpresas = model.ListaEmpresas.Where(x => x.Emprestado.Trim().ToUpper() != "B").ToList();
                }

                model.ListaFamilia = this.servIeod.ListarFamilia().Where(x =>
                    x.Famcodi == ConstantesHorasOperacion.FamLinea ||
                    x.Famcodi == ConstantesHorasOperacion.FamTrafo ||
                    x.Famcodi == ConstantesHorasOperacion.FamTrafo3D).ToList();
            }
            else
            {
                model.ListaEmpresas = this.servIeod.GetListaCriteria(ConstantesAppServicio.ParametroDefecto).Where(x => x.Emprcodi != 0 && x.Emprcodi != -1).ToList();

                if (model.ListaEmpresas.Count() > 0)
                {
                    model.ListaEmpresas = model.ListaEmpresas.Where(x => x.Emprestado.Trim().ToUpper() != "B").ToList();
                }

                model.ListaFamilia = this.servIeod.ListarFamilia();
            }
            model.FiltroFamilia = filtroFamilia.Value;

            return PartialView(model);
        }

        /// <summary>
        /// Muestra el resultado de la busqueda
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFamilia"></param>
        /// <param name="idArea"></param>
        /// <param name="filtro"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult BusquedaEquipoResultado(int idEmpresa, int idFamilia, string filtro, int nroPagina, int? idArea = 0, int? filtroFamilia = 0)
        {
            HorasOperacionModel model = new HorasOperacionModel();
            List<EqEquipoDTO> listaEquipo = new List<EqEquipoDTO>();
            var listaLinea = this.servEvento.BuscarEquipoEvento(idEmpresa, idArea, this.ListarFamiliaByFiltro(idFamilia, filtroFamilia), filtro, nroPagina, Constantes.NroPageShow);

            foreach (var reg in listaLinea)
            {
                EqEquipoDTO eq = new EqEquipoDTO();
                eq.Emprnomb = reg.EMPRENOMB;
                eq.Areanomb = reg.AREANOMB;
                eq.Equicodi = reg.EQUICODI;
                eq.Equinomb = reg.EQUIABREV;
                eq.Equiabrev = reg.EQUIABREV;
                eq.Famabrev = reg.FAMABREV;
                eq.Emprcodi = reg.EMPRCODI;

                listaEquipo.Add(eq);
            }

            model.ListaLineasCongestion = listaEquipo;
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el paginado 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFamilia"></param>
        /// <param name="idArea"></param>
        /// <param name="filtro"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult BusquedaEquipoPaginado(int idEmpresa, int idFamilia, string filtro, int? idArea = 0, int? filtroFamilia = 0)
        {
            HorasOperacionModel model = new HorasOperacionModel();
            model.IndicadorPagina = false;
            int nroRegistros = this.servEvento.ObtenerNroFilasBusquedaEquipo(idEmpresa, idArea, this.ListarFamiliaByFiltro(idFamilia, filtroFamilia), filtro);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.NroPageShow;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Muestra las areas de una empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult BusquedaEquipoArea(int idEmpresa, int idFamilia, int? filtroFamilia = 0)
        {
            HorasOperacionModel model = new HorasOperacionModel();
            model.ListaArea = this.servEvento.ObtenerAreaPorEmpresa(idEmpresa, this.ListarFamiliaByFiltro(idFamilia, filtroFamilia)).ToList();
            return PartialView(model);
        }

        /// <summary>
        /// Funcion para obtener las familias permitidas. 
        /// </summary>
        /// <param name="idFamilia"></param>
        /// <param name="filtroFamilia"> -1: filtrar todas las familias, 0: filtrar solo para lineas de tranmision </param>
        /// <returns></returns>
        private string ListarFamiliaByFiltro(int idFamilia, int? filtroFamilia = 0)
        {
            if (filtroFamilia == 0)
            {
                return idFamilia == 0 ? ConstantesHorasOperacion.FamLinea.ToString() + ConstantesAppServicio.CaracterComa
                    + ConstantesHorasOperacion.FamTrafo.ToString() + ConstantesAppServicio.CaracterComa + ConstantesHorasOperacion.FamTrafo3D.ToString() : idFamilia.ToString();
            }

            return idFamilia.ToString();
        }

        #endregion

        #region Fuente de Datos - Plazo

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexFuenteDatosPlazo()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            FuenteDatosModel model = new FuenteDatosModel();
            return View(model);
        }

        /// <summary>
        /// Lista de fuente de datos
        /// </summary>
        /// <returns></returns>
        public PartialViewResult FuenteDatosPlazoLista()
        {
            FuenteDatosModel model = new FuenteDatosModel();

            model.ListaSiPlazoEnvioFuenteDatos = this.servIeod.ListSiPlazoenvios();

            return PartialView(model);
        }

        /// <summary>
        /// Formulario de Plazo Envio
        /// </summary>
        /// <param name="idPlazo"></param>
        /// <returns></returns>
        public PartialViewResult FuenteDatosPlazoFormulario(int idPlazo)
        {
            FuenteDatosModel model = new FuenteDatosModel();

            model.FechaPeriodo = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);
            model.PlazoEnvioEdicion = this.servIeod.GetByIdSiPlazoenvio(idPlazo);

            return PartialView(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FuenteDatosPlazoGuardar(string strJson)
        {
            FuenteDatosModel model = new FuenteDatosModel();
            try
            {
                if (!base.IsValidSesion) throw new Exception(Constantes.MensajeSesionExpirado);

                JavaScriptSerializer serialize = new JavaScriptSerializer();
                SiPlazoenvioDTO objData = serialize.Deserialize<SiPlazoenvioDTO>(strJson);

                SiPlazoenvioDTO obj = this.servIeod.GetByIdSiPlazoenvio(objData.Plazcodi);
                if (obj != null)
                {
                    obj.Plazperiodo = objData.Plazperiodo;
                    obj.Plazinidia = objData.Plazinidia;
                    obj.Plazinimin = objData.Plazinimin;
                    obj.Plazfindia = objData.Plazfindia;
                    obj.Plazfinmin = objData.Plazfinmin;
                    obj.Plazfueradia = objData.Plazfueradia;
                    obj.Plazfueramin = objData.Plazfueramin;
                    obj.Plazusumodificacion = User.Identity.Name;
                    obj.Plazfecmodificacion = DateTime.Now;

                    this.servIeod.UpdateSiPlazoenvio(obj);

                    model.Resultado = "1";
                }
                else
                {
                    throw new Exception("No existe el registro");
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        #endregion

        #region Fuente de Datos - Ampliacion de Plazo

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexFuenteDatosAmpliacion()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FuenteDatosModel model = new FuenteDatosModel();
            model.ListaFuentedatos = this.servIeod.ListSiFuentedatos().Where(x => x.Fdatcodi > ConstantesIEOD.FdatcodiPadreHOP).ToList();
            model.FechaInicio = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Obtiene la lista de todas las ampliaciones de plazo.
        /// </summary>
        /// <param name="fuendatos"></param>
        /// <param name="sEmpresa"></param>
        /// <param name="sfechaIni"></param>
        /// <param name="sfechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult FuenteDatosAmpliacionLista(string fuendatos, string sEmpresa, string sfechaIni, string sfechaFin)
        {
            DateTime fechaIni = DateTime.ParseExact(sfechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(sfechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            FuenteDatosModel model = new FuenteDatosModel();
            model.ListaSiAmplazoenvio = this.servIeod.ObtenerListaMultipleMeAmpliacionfechas(fechaIni, fechaFin, sEmpresa, fuendatos);

            return PartialView(model);
        }

        /// <summary>
        /// Obtiene el model para pintar el popup de ingreso de la nueva ampliacion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult FuenteDatosAmpliacionNuevo()
        {
            FuenteDatosModel model = new FuenteDatosModel();
            model.ListaFuentedatos = this.servIeod.ListSiFuentedatos().Where(x => x.Fdatcodi > ConstantesIEOD.FdatcodiPadreHOP).ToList();
            model.FechaPeriodo = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            model.FechaAmpliacion = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.HoraPlazo = DateTime.Now.Hour * 2 + 1;
            return PartialView(model);
        }

        /// <summary>
        /// Graba la Ampliacion ingresada.
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="hora"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FuenteDatosAmpliacionGrabar(int empresa, int fdatcodi, string sfechaPeriodo, string sfechaAmpl, int hora)
        {
            FuenteDatosModel model = new FuenteDatosModel();
            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

                string strFechaHoy = DateTime.Now.Date.ToString(Constantes.FormatoFecha);
                if (strFechaHoy != sfechaAmpl) throw new Exception("La fecha de ampliación no pertenece a la fecha actual");

                DateTime fechaPeriodo = DateTime.ParseExact(sfechaPeriodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                SiAmplazoenvioDTO ampliacion = new SiAmplazoenvioDTO();
                ampliacion.Fdatcodi = fdatcodi;
                ampliacion.Emprcodi = empresa;
                ampliacion.Amplzfechaperiodo = fechaPeriodo;
                ampliacion.Amplzfecha = DateTime.Now.Date.AddMinutes(hora * 30);

                var reg = this.servIeod.GetByIdSiAmplazoenvioCriteria(fechaPeriodo, empresa, fdatcodi);
                if (reg == null)
                {
                    ampliacion.Amplzusucreacion = User.Identity.Name;
                    ampliacion.Amplzfeccreacion = DateTime.Now;
                    this.servIeod.SaveSiAmplazoenvio(ampliacion);
                }
                else
                {
                    ampliacion.Amplzcodi = reg.Amplzcodi;
                    ampliacion.Amplzusumodificacion = User.Identity.Name;
                    ampliacion.Amplzfecmodificacion = DateTime.Now;
                    this.servIeod.UpdateSiAmplazoenvio(ampliacion);
                }

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Obtiene las empresas segun fuente datos
        /// </summary>
        /// <param name="fuendatos"></param>
        /// <returns></returns>
        public PartialViewResult FuenteDatosAmpliacionCargarEmpresas(int fuendatos)
        {
            FuenteDatosModel model = new FuenteDatosModel();
            model.ListaEmpresas = this.servIeod.ListarEmpresasXFdatcodi(fuendatos);
            return PartialView(model);
        }

        /// <summary>
        /// //Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesIEOD.ModuloManualUsuario;
            string nombreArchivo = ConstantesIEOD.ArchivoManualUsuarioIntranet;
            string pathDestino = modulo + ConstantesIEOD.FolderRaizMigracionSCOModuloManual;
            string pathAlternativo = ConfigurationManager.AppSettings["FileSystemPortal"];

            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);

                    return File(buffer, Constantes.AppPdf, nombreArchivo);
                }
                else
                    throw new ArgumentException("No se pudo descargar el archivo del servidor.");

            }
            catch (Exception ex)
            {
                throw new ArgumentException("ERROR: ", ex);
            }
        }

        /// <summary>
        /// Obtiene las empresas segun fuente datos
        /// </summary>
        /// <param name="fuendatos"></param>
        /// <returns></returns>
        public PartialViewResult FuenteDatosAmpliacionCargarEmpresasPopup(int fuendatos)
        {
            FuenteDatosModel model = new FuenteDatosModel();
            model.ListaEmpresas = this.servIeod.ListarEmpresasXFdatcodi(fuendatos);
            return PartialView(model);
        }

        #endregion
    }
}
