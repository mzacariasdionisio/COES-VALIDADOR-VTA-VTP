using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Extranet.Areas.Transferencias.Models;
using COES.Servicios.Aplicacion.Transferencias;
using COES.MVC.Extranet.Areas.Transferencias.Helper;

namespace COES.MVC.Extranet.Areas.Transferencias.Controllers
{
    public class TramiteController : Controller
    {
        //
        // GET: /Transferencias/Tramite/
        //[CustomAuthorize]
        public ActionResult Index(int iPericodi = 0)
        {
            SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
            List<SeguridadServicio.EmpresaDTO> list = Funcion.ObtenerEmpresasPorUsuario(User.Identity.Name);
            TempData["EMPRNRO"] = list.Count();
            if (list.Count() == 1)
            {
                TempData["EMPRNOMB"] = list[0].EMPRNOMB;
                Session["EmprNomb"] = list[0].EMPRNOMB;
                Session["EmprCodi"] = list[0].EMPRCODI;
            }
            else if (Session["EmprCodi"] != null)
            {
                int iEmprCodi = Convert.ToInt32(Session["EmprCodi"].ToString());
                EmpresaDTO dtoEmpresa = new EmpresaDTO();
                dtoEmpresa = (new EmpresaAppServicio()).GetByIdEmpresa(iEmprCodi);
                TempData["EMPRNOMB"] = dtoEmpresa.EmprNombre;
                Session["EmprNomb"] = dtoEmpresa.EmprNombre;
            }
            else if (list.Count() > 1)
            {
                TempData["EMPRNOMB"] = "";
                return View();

            }
            else
            {
                //No hay empresa asociada a la cuenta
                TempData["EMPRNOMB"] = "";
                TempData["EMPRNRO"] = -1;
                return View();
            }
            PeriodoModel model = new PeriodoModel();
            model.ListaPeriodo = (new PeriodoAppServicio()).ListarByEstadoPublicarCerrado();
            TempData["Periodocodigo"] = new SelectList(model.ListaPeriodo, "Pericodi", "Perinombre", iPericodi);
            Session["iTrmVersion"] = 0;
            return View();
        }

        public JsonResult GetVersion(int iPeriCodi)
        {
            RecalculoModel modelRecalculo = new RecalculoModel();
            modelRecalculo.ListaRecalculos = (new RecalculoAppServicio()).ListRecalculos(iPeriCodi);
            Session["iTrmVersion"] = modelRecalculo.ListaRecalculos[0].RecaCodi;
            return Json(modelRecalculo.ListaRecalculos);
        }

        [HttpPost]
        public ActionResult Lista(int iPeriCodi = 0, int iTrmVersion = 0)
        {
            if (iTrmVersion == 0 && !Session["iTrmVersion"].Equals(null)) iTrmVersion = Convert.ToInt32(Session["iTrmVersion"].ToString());
            TramiteModel model = new TramiteModel();
            model.bNuevo = false; //TempData["codi"]

            if (iPeriCodi > 0 && iTrmVersion > 0)
            {
                RecalculoModel modelRecalculo = new RecalculoModel();
                modelRecalculo.Entidad = (new RecalculoAppServicio()).GetByIdRecalculo(iPeriCodi, iTrmVersion);
                if (modelRecalculo.Entidad.RecaFechaObservacion >= System.DateTime.Now)
                {
                    model.bNuevo = true;
                }
            }
            model.ListaTramites = (new TramiteAppServicio()).BuscarTramite(null, null, iPeriCodi, iTrmVersion);
            return PartialView(model);
            
        }

        public ActionResult View(int id )
        {
            TramiteModel model = new TramiteModel();
            model.Entidad = (new TramiteAppServicio()).GetByIdTramite(id);

            return PartialView(model);
        }

        public ActionResult New(int iPeriCodi, int iTrmVersion)
        {
            TramiteModel modelo = new TramiteModel();
            modelo.Entidad = new TramiteDTO();
            modelo.Entidad.PeriCodi = iPeriCodi;
            modelo.Entidad.TramVersion = iTrmVersion;
            TipoTramiteModel modelTipoTramite = new TipoTramiteModel();
            modelTipoTramite.ListaTipoTramites = (new TipoTramiteAppServicio()).ListTipoTramite();
            TempData["Tipotramitecodigo"] = new SelectList(modelTipoTramite.ListaTipoTramites, "Tipotramcodi", "Tipotramnombre");
            PeriodoModel modelPeriodo = new PeriodoModel();
            modelPeriodo.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(iPeriCodi);
            TempData["Periodonombre"] = modelPeriodo.Entidad.PeriNombre; 

            RecalculoModel reca = new RecalculoModel();
            reca.Entidad= new RecalculoDTO();
            reca.Entidad = (new RecalculoAppServicio()).GetByIdRecalculo(iPeriCodi, iTrmVersion);
            TempData["NombreVersion"] = reca.Entidad.RecaNombre;
            TempData["EMPRNOMB"] = Session["EmprNomb"];
            return PartialView(modelo); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(TramiteModel modelo)
        {
            if (ModelState.IsValid)
            {
                if (Session["EmprCodi"] != null)
                {
                    modelo.Entidad.EmprCodi = Convert.ToInt32(Session["EmprCodi"].ToString());
                    modelo.Entidad.UsuaSeinCodi = User.Identity.Name;
                    modelo.Entidad.TramCorrInf = "NO";
                    modelo.Entidad.UsuaCoesCodi = null;
                    modelo.Entidad.TramRespuesta = null;
                    modelo.Entidad.TramFecRes = null;
                    modelo.IdTramite = (new TramiteAppServicio()).SaveOrUpdateTramite(modelo.Entidad);
                    TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                    //return RedirectToAction("Index");
                    return Redirect("index?iPericodi=" + modelo.Entidad.PeriCodi);
                }
                else
                {
                    TempData["sMensajeExito"] = "Lo sentimos, no se ha procesado la información, favor de seleccionar una empresa primero";
                    return Redirect("index?iPericodi=" + modelo.Entidad.PeriCodi);
                }
            }
            TipoTramiteModel modelTipoTramite = new TipoTramiteModel();
            modelTipoTramite.ListaTipoTramites = (new TipoTramiteAppServicio()).ListTipoTramite();
            TempData["Tipotramitecodigo"] = new SelectList(modelTipoTramite.ListaTipoTramites, "Tipotramcodi", "Tipotramnombre", modelo.Entidad.TipoTramcodi);
            PeriodoModel modelPeriodo = new PeriodoModel();
            modelPeriodo.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(modelo.Entidad.PeriCodi);
            TempData["Periodonombre"] = modelPeriodo.Entidad.PeriNombre;
            TempData["EMPRNOMB"] = Session["EmprNomb"];
            return PartialView(modelo); 
        }

        /// Permite seleccionar a la empresa con la que desea trabajar
        [HttpPost]
        public ActionResult EscogerEmpresa()
        {
            SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
            List<SeguridadServicio.EmpresaDTO> list = Funcion.ObtenerEmpresasPorUsuario(User.Identity.Name);
            EmpresaModel model = new EmpresaModel();
            List<EmpresaDTO> lista = new List<EmpresaDTO>();
            foreach (var item in list)
            {
                EmpresaDTO dtoEmpresa = new EmpresaDTO();
                dtoEmpresa = (new EmpresaAppServicio()).GetByIdEmpresa(item.EMPRCODI);
                lista.Add(dtoEmpresa);
            }
            model.ListaEmpresas = lista;
            return PartialView(model);
        }

        /// Permite actualizar o grabar un registro
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmpresaElegida(int EmprCodi)
        {
            if (EmprCodi > 0)
            {
                Session["EmprCodi"] = EmprCodi;
            }
            return RedirectToAction("Index");
        }
    }
}
