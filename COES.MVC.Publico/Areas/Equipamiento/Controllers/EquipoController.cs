using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.MVC.Publico.Areas.Equipamiento.Models;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.General;
using log4net;
using COES.MVC.Publico.Helper;

namespace COES.MVC.Publico.Areas.Equipamiento.Controllers
{
    public class EquipoController : Controller
    {
        //
        // GET: /Equipamiento/Equipo/
        GeneralAppServicio appGeneral = new GeneralAppServicio();
        EquipamientoAppServicio appEquipamiento = new EquipamientoAppServicio();
        private readonly List<SiEmpresaDTO> _listaEmpresas = new List<SiEmpresaDTO>();
        private readonly List<EqFamiliaDTO> _listaFamilias = new List<EqFamiliaDTO>();
        private readonly int iTamanioPagina = 50;
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(EquipoController));
        public EquipoController()
        {
            _listaEmpresas = this.appGeneral.ListadoComboEmpresasPorTipo(-2);
            _listaFamilias = appEquipamiento.ListEqFamilias().Where(t => t.Famcodi > 0).OrderBy(t => t.Famnomb).ToList();
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("EquipoController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("EquipoController", ex);
                throw;
            }
        }

        public ActionResult Index()
        {
            var modelo = new IndexEquipoModel();
            modelo.ListaTipoEmpresa = appGeneral.ListarTiposEmpresa().Where(t => t.Tipoemprcodi > 0).OrderBy(t => t.Tipoemprdesc).ToList();
            modelo.ListaTipoEquipo = appEquipamiento.ListEqFamilias().Where(t => t.Famcodi > 0).OrderBy(t => t.Famnomb).ToList();
            modelo.iEmpresa = 0;
            modelo.iTipoEmpresa = 0;
            modelo.iTipoEquipo = 0;
            modelo.sEstadoCodi = "A";
            modelo.ListaEmpresa = new List<SiEmpresaDTO>();
            return View(modelo);
        }
        public JsonResult CargarEmpresas(int idTipoEmpresa)
        {
            var entitys = new List<SiEmpresaDTO>();
            entitys = this.appGeneral.ListadoComboEmpresasPorTipo(idTipoEmpresa).Where(x => x.Emprestado == Constantes.EstadoActivo).ToList();

            //var empresa1 = entitys.SingleOrDefault(t => t.Emprcodi == 10490);
            //var empresa2 = entitys.SingleOrDefault(t => t.Emprcodi == 12479);
            //entitys.Remove(empresa1);
            //entitys.Remove(empresa2);
            ////Modelo.Empresas = empresas;

            var list = new SelectList(entitys, "Emprcodi", "Emprnomb");
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult ListadoEquipos(IndexEquipoModel model)
        {
            int iEmpresa = model.iEmpresa;
            int iFamilia = model.iTipoEquipo;
            int iTipoEmpresa = model.iTipoEmpresa;
            int iEquipo = string.IsNullOrEmpty(model.CodigoEquipo) ? -2 : Convert.ToInt32(model.CodigoEquipo.Trim());
            string sEstado = "A";
            var lsResultado = appEquipamiento.ListaEquipamientoPaginado(iEmpresa, iFamilia, iTipoEmpresa, iEquipo, sEstado, "", model.NroPagina, iTamanioPagina);
            //foreach (var oEquipo in lsResultado)
            //{
            //    oEquipo.EstadoDesc = EquipamientoHelper.EstadoDescripcion(oEquipo.Equiestado);
            //    oEquipo.Osigrupocodi = EquipamientoHelper.EstiloEstado(oEquipo.Equiestado);
            //}
            model.ListadoEquipamiento = lsResultado;
            return PartialView(model);
        }
        [HttpPost]
        public PartialViewResult PaginadoEquipos(IndexEquipoModel model)
        {
            int iEmpresa = model.iEmpresa;
            int iFamilia = model.iTipoEquipo;
            int iTipoEmpresa = model.iTipoEmpresa;
            int iEquipo = string.IsNullOrEmpty(model.CodigoEquipo) ? -2 : Convert.ToInt32(model.CodigoEquipo.Trim());
            string sEstado = "A";

            model.IndicadorPagina = false;
            int nroRegistros = appEquipamiento.TotalEquipamiento(iEmpresa, iFamilia, iTipoEmpresa, iEquipo, sEstado, "");

            if (nroRegistros > iTamanioPagina)
            {
                int pageSize = iTamanioPagina;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = 20;
                model.IndicadorPagina = true;
            }
            return PartialView(model);
        }
    }
}
