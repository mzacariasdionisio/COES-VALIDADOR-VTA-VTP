using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.MVC.Publico.Areas.ReportePotencia.Models;
using COES.Servicios.Aplicacion.Despacho;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.General;
using log4net;

namespace COES.MVC.Publico.Areas.ReportePotencia.Controllers
{
    public class ConfiguracionController : Controller
    {
        private readonly List<SiEmpresaDTO> _listaEmpresas = new List<SiEmpresaDTO>();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ConfiguracionController));
        GeneralAppServicio appGeneral = new GeneralAppServicio();
        DespachoAppServicio appDespacho = new DespachoAppServicio();
        readonly List<EstadoModel> listadoEstados = new List<EstadoModel>();

        public ConfiguracionController()
        {
            _listaEmpresas = this.appGeneral.ListadoComboEmpresasPorTipo(3);
            listadoEstados.Add(new EstadoModel { EstadoCodigo = "S", EstadoDescripcion = "Activo" });
            listadoEstados.Add(new EstadoModel { EstadoCodigo = "N", EstadoDescripcion = "Baja" });
            log4net.Config.XmlConfigurator.Configure();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("ConfiguracionController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("ConfiguracionController", ex);
                throw;
            }
        }
        //
        // GET: /ReportePotencia/Configuracion/

        public ActionResult Index()
        {
            IndexConfiguracionModel modelo = new IndexConfiguracionModel()
            {
                ListaEmpresas = _listaEmpresas,
                ListaEstados = listadoEstados
            };
            return View(modelo);
        }

        [HttpPost]
        public PartialViewResult Listado(int iEmpresa, string sEstado)
        {
            ListadoConfiguracion modelo = new ListadoConfiguracion
            {
                ListadoModosOperacionConfigurados = appDespacho.ListadoModosOperacionConfiguracionPe(iEmpresa,sEstado)
            };
            return PartialView(modelo);
        }

    }
}
