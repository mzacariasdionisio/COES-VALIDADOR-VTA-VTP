using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Coordinacion.Helper;
using COES.MVC.Intranet.Areas.Coordinacion.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Despacho;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.MVC.Intranet.SeguridadServicio;

namespace COES.MVC.Intranet.Areas.Coordinacion.Controllers
{
    public class ConfiguracionSCOSinacController : BaseController
    {
        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>  
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ConfiguracionSCOSinacController));

        /// <summary>
        /// Instancia de la clase RepresentanteAppServicio
        /// </summary>
        private DespachoAppServicio appDespacho = new DespachoAppServicio();

        public ConfiguracionSCOSinacController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("ConfiguracionSCOSinacController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("ConfiguracionSCOSinacController", ex);
                throw;
            }
        }

        /// <summary>
        /// Permite retornar la vista de configuración SCOSinac
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult Test()
        //{
        //    //TEMPORAL FIT
        //    //Prueba de web service rest para add.in

        //    string fechaConsulta = "24-04-2018";
        //    DateTime fecha = DateTime.Now;

        //    if (!string.IsNullOrEmpty(fechaConsulta))
        //    {
        //        fecha = DateTime.ParseExact(fechaConsulta, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        //    }

        //    DespachoAppServicio servicio = new DespachoAppServicio();

        //    List<AddInPrGrupodatDTO> result = servicio.ObtenerListaScoSinacDat(fecha);
        //    List<AddInPrGrupoDTO> result2 = servicio.ObtenerListaScoSinacGrupo();
        //    List<AddInPrGrupoUnificadoDTO> result3 = servicio.ObtenerListaScoSinacUnificado(fecha);

        //    //FIN TEMPORAL FIT

        //    return View();
        //}


        /// <summary>
        /// Permite mostrar el paginado de la lista configuración SCOSinac
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado()
        {
            PaginadoModel model = new PaginadoModel();
            model.IndicadorPagina = false;

            int nroRegistros = this.appDespacho.ObtenerTotalListaConfigScoSinac(DateTime.Now);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView("Paginado", model);
        }

        public ActionResult Listado(int nroPagina)
        {            
            var DatosConfiguracion = this.appDespacho.ObtenerListaConfigScoSinac(DateTime.Now, nroPagina,
                Constantes.PageSize);

            ScoSinacModel modelConfiguracion = new ScoSinacModel();
            modelConfiguracion.ListaConfiguracion = DatosConfiguracion;
            
            return View(modelConfiguracion);

        }

        /// <summary>
        /// Permite cargar los datos del contacto seleccionado
        /// </summary>
        /// <param name="concepcodi">Codigo de concepto</param>
        /// <param name="grupocodi">Codigo de grupo</param>
        /// <param name="deleted">Codigo de deleted</param>
        [HttpPost]
        public PartialViewResult Edicion(string fechadat, int concepcodi, int grupocodi, int deleted, string gruponomb, string concepdesc)
        {
            DateTime fecha = DateTime.Now;
            if (fechadat != null)
            {
                fecha = DateTime.ParseExact(fechadat, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
            }

            PrGrupodatDTO grupoDat = this.appDespacho.GetByIdPrGrupodat(fecha, concepcodi, grupocodi, deleted);
            grupoDat.GrupoNomb = gruponomb;
            grupoDat.ConcepDesc = concepdesc;

            return PartialView(grupoDat);
        }

        /// <summary>
        /// Permite actualizar los datos del contacto
        /// </summary>
        /// <param name="model">model PrGrupodatDTO</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarEdicion(PrGrupodatDTO model)
        {
            try
            {                
                UserDTO userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]);                
                PrGrupodatDTO grupoDat = this.appDespacho.GetByIdPrGrupodat(model.Fechadat.Value, model.Concepcodi, model.Grupocodi, model.Deleted);

                var CodScada = model.Formuladat;

                if (this.ValidarEntrada(CodScada))
                {
                    string scada = string.Empty;
                    string[] codigos = CodScada.Split(',');

                    int i = 0;
                    foreach (string codigo in codigos)
                    {
                        if (i < codigos.Length - 1)
                        {
                            scada = scada + codigo.Trim() + ",";
                        }
                        else
                        {
                            scada = scada + codigo.Trim();
                        }
                        i++;
                    }

                    grupoDat.Formuladat = scada;

                    grupoDat.Fechadat = DateTime.Now;                    
                    grupoDat.Lastuser= userLogin.UserCode.ToString();  
                    
                    //Se cambio de update a insert, segín consulta a DTI
                    this.appDespacho.SavePrGrupodat(grupoDat);
                    //this.appDespacho.UpdatePrGrupodat(grupoDat);

                    return Json(1);
                }
                else
                {
                    return Json(0);
                    //MessageBox.Show("Ingrese números separados por comas.");
                }               
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }


        /// Valida la entrada de datos
        /// </summary>
        /// <returns></returns>
        protected bool ValidarEntrada(string CodScada)
        {
            if (!string.IsNullOrEmpty(CodScada))
            {
                string[] cadenas = CodScada.Split(',');
                int numero = 0;
                foreach (string cadena in cadenas)
                {
                    if (!int.TryParse(cadena.Trim(), out numero))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}