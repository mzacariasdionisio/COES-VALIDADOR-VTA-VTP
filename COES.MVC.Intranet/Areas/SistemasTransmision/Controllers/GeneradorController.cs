using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.SistemasTransmision.Models;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.SistemasTransmision;
using COES.Servicios.Aplicacion.SistemasTransmision.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.SistemasTransmision.Controllers
{
    public class GeneradorController : BaseController
    {
        //
        // GET: /SistemasTransmision/Generador/

        SistemasTransmisionAppServicio servicioSistemasTransmision = new SistemasTransmisionAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New(int stpercodi = 0, int strecacodi = 0)
        {
            GeneradorModel modelo = new GeneradorModel();
            modelo.Entidad = new StGeneradorDTO();
            modelo.EntidadPeriodo = new StPeriodoDTO();
            modelo.EntidadStRecalculo = new StRecalculoDTO();
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }
            int emprcodi = 0;
            modelo.Entidad.Stgenrcodi = 0;

            modelo.EntidadEmpresa = new EmpresaDTO();
            modelo.ListaEmpresas = this.servicioEmpresa.ListEmpresas();
            modelo.EntidadPeriodo.Stpercodi = stpercodi;
            modelo.Entidad.Strecacodi = strecacodi;
            modelo.IdEmpresa = emprcodi;
            modelo.EntidadEmpresa.EmprCodi = emprcodi;
            modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            return PartialView(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(GeneradorModel modelo)
        {
            if (ModelState.IsValid)
            {
                modelo.Entidad.Stgenrusucreacion = User.Identity.Name;
                modelo.Entidad.Stgenrfeccreacion = DateTime.Now;

                if (modelo.Entidad.Stgenrcodi == 0)
                {
                    this.servicioSistemasTransmision.SaveStGenerador(modelo.Entidad);
                }
                else
                {
                    this.servicioSistemasTransmision.UpdateStGenerador(modelo.Entidad);
                }

                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                //return RedirectToAction("Index", "Empresa", new { stpercodi = modelo.EntidadPeriodo.Stpercodi, strecacodi = modelo.Entidad.Strecacodi });
                return new RedirectResult(Url.Action("Index", "Empresa", new { stpercodi = modelo.EntidadPeriodo.Stpercodi, strecacodi = modelo.Entidad.Strecacodi }) + "#paso2");
            }

            modelo.ListaEmpresas = (new EmpresaAppServicio()).ListEmpresas();
            modelo.sError = "Se ha producido un error al insertar la información";
            modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(modelo);
        }


        [HttpPost]
        public ActionResult List(int strecacodi)
        {
            GeneradorModel model = new GeneradorModel();
            model.ListaEmpresaGeneradora = this.servicioSistemasTransmision.ListStGeneradors(strecacodi);
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            return PartialView(model);
        }

        [HttpPost, ActionName("DeleteEG")]
        [ValidateAntiForgeryToken]
        public string DeleteEG(int id)
        {
            if (id > 0)
            {
                GeneradorModel model = new GeneradorModel();
                this.servicioSistemasTransmision.DeleteStGenerador(id);
                return "true";
            }
            return "False";
        }

        public ActionResult Edit(int id, int emprecodi = 0)
        {
            GeneradorModel model = new GeneradorModel();
            model.EntidadEmpresa = (new EmpresaAppServicio()).GetByIdEmpresa(emprecodi);
            model.EntidadPeriodo = new StPeriodoDTO();
            model.EntidadStRecalculo = new StRecalculoDTO();

            model.Entidad = this.servicioSistemasTransmision.GetByIdStGenerador(id);
            if (model.Entidad == null)
            {
                return HttpNotFound();
            }

            if (emprecodi != 0)
            {
                model.Entidad.Emprcodi = model.EntidadEmpresa.EmprCodi;
            }

            int IdRecalculo = model.Entidad.Strecacodi;
            model.EntidadStRecalculo = this.servicioSistemasTransmision.GetByIdStRecalculo(IdRecalculo);
            int IdPeriodo = model.EntidadStRecalculo.Stpercodi;
            model.EntidadPeriodo.Stpercodi = IdPeriodo;

            model.ListaEmpresas = this.servicioEmpresa.ListEmpresas();
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);

            return PartialView(model);
        }

        /// <summary>
        /// Permite exportar a un archivo excel la lista de empresas generadoras
        /// </summary>
        /// <param name="stpercodi">Código del Mes de cálculo</param>
        /// <param name="strecacodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarGenerador(int stpercodi = 0, int strecacodi = 0, int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
                string file = "-1";

                file = this.servicioSistemasTransmision.GenerarListaEmpresasGeneradoras(strecacodi, formato, pathFile, pathLogo);
                return Json(file);
            }
            catch (Exception e)
            {
                string sMensaje = e.Message;
                return Json(-1);
            }
        }

        /// <summary>
        /// Abrir el archivo
        /// </summary>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString() + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;
            return File(path, app, sFecha + "_" + file);
        }
    }
}