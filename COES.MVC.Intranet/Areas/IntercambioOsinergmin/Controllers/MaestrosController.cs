using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using COES.MVC.Intranet.Controllers;
//using COES.MVC.Intranet.EntidadOsinergmin;
using COES.Servicios.Aplicacion.IntercambioOsinergmin;
using COES.Servicios.Aplicacion.IntercambioOsinergmin.Helper;
using COES.MVC.Intranet.Areas.IntercambioOsinergmin.Helper;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Helper;
using System.IO;
using System.Text;
using log4net;
using System.Configuration;
using System.Reflection;
using COES.MVC.Intranet.Areas.IntercambioOsinergmin.Models.Maestros;
using COES.Framework.Base.Tools;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.IntercambioOsinergmin.Controllers
{
    public class MaestrosController : BaseController
    {
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

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly SincronizaMaestroAppServicio sincronizaMaestroAppServicio;

        public MaestrosController()
        {
            log4net.Config.XmlConfigurator.Configure();
            sincronizaMaestroAppServicio = new SincronizaMaestroAppServicio();
        }

        /// <summary>
        /// Clase Inicial
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        public ActionResult Index(int anio = 0)
        {
            MaestrosModel model = new MaestrosModel();

            return View("Index", model);
        }

        /// <summary>
        /// Obtiene la estructura de entidades
        /// y la devuelve con la vista parcial 'Arbol'
        /// </summary>
        /// <returns>Html con la vista parcial Arbol</returns>
        [HttpPost]
        public PartialViewResult GetArbol()
        {
            //Obtenemos la estructura del arbol y la mostramos
            var arbol = sincronizaMaestroAppServicio.GetDatosArbolMaestros();
            return PartialView("Arbol", new ListadoArbol
            {
                EstructuraArbol = arbol,
                CodigoEntidad = (int)EntidadSincroniza.Empresa
            });
        }

        /// <summary>
        /// Lista los registros que le corresponden a la entidad dada, con el filtro entregado
        /// y en el numero de página adecuado
        /// </summary>
        /// <param name="entidad"> Entidad seleccionada </param>
        /// <param name="filtro"> Filtro ingresado</param>
        /// <param name="nroPagina"> Numero de página solicitado </param>
        /// <returns> HTML de la vista parcial</returns>
        [HttpPost]
        public PartialViewResult ListRegistrosEntidad(EntidadSincroniza entidad, string filtro, int nroPagina, string combo, string radio)
        {
            //Validamos que el enum esté correcto
            if (!Enum.IsDefined(typeof(EntidadSincroniza), entidad)) throw new ArgumentOutOfRangeException("entidad");

            //Según la entidad, obtenemos el modelo con el listado y pintamos la grilla
            var listadoRegistros = new List<DetalleEntidadModel>();

            List<EntidadListadoDTO> listado = new List<EntidadListadoDTO>();

            string ent = "";
            switch (entidad)
            {
                case EntidadSincroniza.Empresa:
                    listado = sincronizaMaestroAppServicio.ListMaestroEmpresa(filtro);
                    ent = "Empresa";
                    break;
                case EntidadSincroniza.UsuarioLibre:
                    listado = sincronizaMaestroAppServicio.ListMaestroUsuarioLibre(filtro);
                    ent = "UsuarioLibre";    
                    break;
                case EntidadSincroniza.Suministro:
                    listado = sincronizaMaestroAppServicio.ListMaestroSuministrador(filtro);
                    ent = "Suministro";        
                    break;
                case EntidadSincroniza.Barra:
                    listado = sincronizaMaestroAppServicio.ListMaestroBarra(filtro);
                    ent = "Barra";  
                    break;
                case EntidadSincroniza.CentralGeneracion:
                    listado = sincronizaMaestroAppServicio.ListMaestroCentralGeneracion(filtro);
                    ent = "CentralGeneracion";  
                    break;
                case EntidadSincroniza.GrupoGeneracion:
                    listado = sincronizaMaestroAppServicio.ListMaestroGrupoGeneracion(filtro);
                    ent = "GrupoGeneracion";      
                    break;
                case EntidadSincroniza.ModoOperacion:
                    listado = sincronizaMaestroAppServicio.ListMaestroModoOperacion(filtro);
                    ent = "ModoOperacion";      
                    break;
                case EntidadSincroniza.Cuenca:
                    listado = sincronizaMaestroAppServicio.ListMaestroCuenca(filtro);
                    ent = "Cuenca";      
                    break;
                case EntidadSincroniza.Embalse:
                    listado = sincronizaMaestroAppServicio.ListMaestroEmbalse(filtro);
                    ent = "Embalse";      
                    break;
                case EntidadSincroniza.Lago:
                    listado = sincronizaMaestroAppServicio.ListMaestroLago(filtro);
                    ent = "Lago";      
                    break;
                case EntidadSincroniza.Ninguno:

                    if (radio == null)
                    {
                        radio = "P";
                    }

                    listado = sincronizaMaestroAppServicio.ListResutado(combo, radio);
                    ent = "Ninguno";
                    break;

            }

            if (ent.Equals("Empresa"))
            {
                foreach (var item in listado)
                {
                    DetalleEntidadModel obj = new DetalleEntidadModel();
                    obj.Codigo = item.EntidadCodigo.ToString();
                    obj.Descripcion = item.Descripcion;
                    obj.CodigoOsinergmin = item.CodigoOsinergmin;
                    obj.EntidadDescripcion = item.CampoAdicional;
                    listadoRegistros.Add(obj);
                }
            }
            else if (ent.Equals("Ninguno"))
            {
                foreach (var item in listado)
                {
                    DetalleEntidadModel obj = new DetalleEntidadModel();
                    obj.Codigo = item.EntidadCodigo.ToString();
                    obj.Descripcion = item.Descripcion;
                    obj.CodigoOsinergmin = item.CodigoOsinergmin;
                    obj.EntidadDescripcion = item.CampoAdicional;
                    obj.Estado = item.Estado;
                    listadoRegistros.Add(obj);
                }
            }
            else 
            {
                foreach (var item in listado)
                {
                    DetalleEntidadModel obj = new DetalleEntidadModel();
                    obj.Codigo = item.EntidadCodigo.ToString();
                    obj.Descripcion = item.Descripcion;
                    obj.CodigoOsinergmin = item.CodigoOsinergmin;
                    listadoRegistros.Add(obj);
                }
            }

            var entidades = new ListadoEntidades(listadoRegistros.OrderBy(x => x.Descripcion).ToList(), !EntidadesHelper.IsEntidadConAsignacionesPendiente(entidad), ent, radio);

            return PartialView("Listado", entidades);
        }

        /// <summary>
        /// Obtiene el filtro adecuado según la entidad y con su filtro ya inicializado
        /// </summary>
        /// <param name="entidad">Código de la entidad actualmente seleccionada</param>
        /// <param name="filtroActual">Valor del filtro que se esta usando para filtrar los resultados</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult GetFiltro(int entidad = (int) EntidadSincroniza.Empresa, string filtroActual = null)
        {

            var entidadEnum = (EntidadSincroniza)entidad;

            var model = new FiltroDetalleEntidadSincronizada(entidadEnum, filtroActual);
            return PartialView("FiltroAsignado", model);

        }

        /// <summary>
        /// Permite generar el reporte de sincronización
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(EntidadSincroniza entidad, string filtro)
        {
            try
            {
                var pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                var fileName = sincronizaMaestroAppServicio.GenerarReporte(entidad, filtro, AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga, pathLogo);
                return Json(fileName);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite realizar la exportación de los datos Osinergmin de la entidad seleccionada.
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult exportarDatosOsi(EntidadSincroniza entidad)
        {
            try
            {
                var pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                var nombreArchivo 
                    = sincronizaMaestroAppServicio.GenerarReporteDatosOsinergmin(entidad
                                                                               , AppDomain.CurrentDomain.BaseDirectory 
                                                                                 + Constantes.RutaCarga
                                                                               , pathLogo);
                return Json(nombreArchivo);
            }
            catch
            {
                return Json(-1);
            }
        }

        [HttpGet]
        public virtual ActionResult Descargar(string file)
        {
            string path = file;
            string app = Constantes.AppExcel;
            return File(path, app, "ReporteSincronizacion.xls");
        }

        //- alpha.HDT - 29/06/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite descargar los datos del maestro de Osinergmin.
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarMaestroOsinergmin(string archivo)
        {
            string ruta = archivo;
            string app = Constantes.AppExcel;
            FileInfo fileInfo = new FileInfo(archivo);

            return File(ruta, app, fileInfo.Name);
        }

        [HttpPost]
        public JsonResult ObtenerListaEntidad(string entidad)
        {
            EntidadSincroniza entidadSincroniza = ((EntidadSincroniza)Enum.Parse(typeof(EntidadSincroniza), entidad));

            Dictionary<string, string> dictionary = sincronizaMaestroAppServicio.ObtenerListaValoresParaHomologacion(entidadSincroniza);
           
            List<Combo> listaCombo = new List<Combo>();

            Combo entity = new Combo();
            entity.id = "";
            entity.name = " -- SELECCIONE -- ";
            listaCombo.Add(entity);

            foreach (KeyValuePair<string, string> entry in dictionary)
            {
                entity = new Combo();
                entity.name = entry.Value + " (" + entry.Key.Trim() + ")";
                entity.id = entry.Key.Trim();
                listaCombo.Add(entity);
            }

            var jsonSerialiser = new JavaScriptSerializer();

            return Json(jsonSerialiser.Serialize(listaCombo.OrderBy(x => x.name).ToList()));
        }


        [HttpPost]
        public JsonResult GrabarHomologacion(string entidad, string codigoCoes, string codigoOsinergmin)
        {
            EntidadSincroniza entidadSincroniza = ((EntidadSincroniza)Enum.Parse(typeof(EntidadSincroniza), entidad));

            //Obtener el mapencodi de la tabla pendiente
            string mapencodi = sincronizaMaestroAppServicio.ObtenerIdPendiente(entidad, codigoCoes);

            try
            {
                sincronizaMaestroAppServicio.SaveHomologacion(entidadSincroniza, codigoCoes.Trim(), codigoOsinergmin.Trim(), mapencodi);
                return Json(1);
            }
            catch (Exception)
            {
                //Se produce un error al momento de realizar la sincronización
                return Json(0);
                throw;
            }

        }

        [HttpPost]
        public JsonResult QuitarAsignacion(string entidad, string codigo, string codigoOsinergmin)
        {
            EntidadSincroniza entidadSincroniza = ((EntidadSincroniza)Enum.Parse(typeof(EntidadSincroniza), entidad));

            try
            {
                sincronizaMaestroAppServicio.DeleteHomologacion(entidadSincroniza, codigo, codigoOsinergmin);
                return Json(1);
            }
            catch (Exception)
            {
                //Se produce un error al momento de realizar la sincronización
                return Json(0);
                throw;
            }

        }

    }
}
