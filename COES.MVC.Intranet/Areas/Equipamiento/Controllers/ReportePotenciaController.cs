using COES.MVC.Intranet.Areas.Equipamiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.General;
using log4net;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class ReportePotenciaController : Controller
    {
        private readonly List<SiEmpresaDTO> _listaEmpresas;
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ReportePotenciaController));
        GeneralAppServicio appGeneral = new GeneralAppServicio();
        DespachoAppServicio appDespacho = new DespachoAppServicio();
        EquipamientoAppServicio appEquipamiento = new EquipamientoAppServicio();
        private readonly List<int> listadoFamiliasCentrales = new List<int>();
        public ReportePotenciaController()
        {
            _listaEmpresas = this.appGeneral.ListadoComboEmpresasPorTipo(3);
            listadoFamiliasCentrales.Add(4);
            listadoFamiliasCentrales.Add(5);
            listadoFamiliasCentrales.Add(37);
            listadoFamiliasCentrales.Add(39);
            log4net.Config.XmlConfigurator.Configure();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("ReportePotenciaController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("ReportePotenciaController", ex);
                throw;
            }
        }
        //
        // GET: /Equipamiento/ReportePotencia/

        public ActionResult IndexConfiguracion()
        {
            ConfiguracionModel modelo = new ConfiguracionModel
            {
                ListaEmpresas = _listaEmpresas
            };
            return View(modelo);
        }

        [HttpPost]
        public PartialViewResult ListConfiguracion(int iEmpresa)
        {
            ConfiguracionModel modelo = new ConfiguracionModel
            {
                ListaModosConfigurados = appDespacho.ListarModosOperacionConfigurados(iEmpresa)
            };
            return PartialView(modelo);
        }

        public PartialViewResult ModosConfiguracion(int iEmpresa)
        {
            ConfiguracionModel modelo = new ConfiguracionModel
            {
                ListaModosDisponibles = appDespacho.ListarModosOperacionNoConfigurados(iEmpresa)
            };
            return PartialView(modelo);
        }
        [HttpPost]
        public JsonResult Eliminar(int iGrupocodi)
        {
            try
            {
                this.appDespacho.DeletePrConfiguracionPotEfectiva(iGrupocodi);
                return Json(1);
            }
            catch(Exception e)
            {
                log.Error("Eliminar", e);
                return Json(-1);
            }
        }
        [HttpPost]
        public JsonResult Guardar(string[] modosConfigurados)
        {
            try
            {
                var modosConf = new List<PrConfiguracionPotEfectivaDTO>();
                foreach (var grupo in modosConfigurados)
                {
                    modosConf.Add(new PrConfiguracionPotEfectivaDTO()
                    {
                        Grupocodi = int.Parse(grupo),
                        Confpeusuariocreacion = User.Identity.Name
                    });
                }
                this.appDespacho.SaveAllPrConfiguracionPotEfectiva(modosConf);
                return Json(1);
            }
            catch(Exception e)
            {
                log.Error("Guardar", e);
                return Json(-1);
            }
        }

        public ActionResult IndexReporte()
        {
            

            ReportePotenciaModel modelo = new ReportePotenciaModel()
            {
                ListaTipoGeneracion = appGeneral.ListarTiposGeneracion().Where(t=>t.Tgenercodi>0).OrderBy(t=>t.Tgenernomb).ToList(),
                ListaCentrales = appEquipamiento.ListarEquipoxFamilias(listadoFamiliasCentrales.ToArray()).OrderBy(t=>t.Equinomb).ToList(),
                ListaEmpresas = _listaEmpresas
            };
            return View(modelo);
        }

        public JsonResult CargarCentrales(int iEmpresas)
        {
            var entitys = new List<EqEquipoDTO>();
            //entitys = appEquipamiento.ListarEquipoxFamiliasxEmpresas(listadoFamiliasCentrales.ToArray()).OrderBy(t => t.Equinomb).ToList();
            var list = new SelectList(entitys, "Emprcodi", "Emprnomb");
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ListadoReporte()
        {
            return PartialView();
        }
    }
}
