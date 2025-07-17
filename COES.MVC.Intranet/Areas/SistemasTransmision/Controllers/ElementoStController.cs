using COES.MVC.Intranet.Areas.SistemasTransmision.Models;
using COES.Servicios.Aplicacion.SistemasTransmision;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;
using COES.MVC.Intranet.Controllers;
using System.Configuration;
using COES.Servicios.Aplicacion.SistemasTransmision.Helper;

namespace COES.MVC.Intranet.Areas.SistemasTransmision.Controllers
{
    public class ElementoStController : BaseController
    {
        //
        // GET: /SistemasTransmision/ElementoSt/
        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        SistemasTransmisionAppServicio servicioSistemasTransmision = new SistemasTransmisionAppServicio();

        public ActionResult Index(int pericodi = 0, int recacodi = 0)
        {
            ElementoStModel model = new ElementoStModel();
            model.ListaPeriodos = this.servicioSistemasTransmision.ListStPeriodos();
            if (model.ListaPeriodos.Count > 0 && pericodi == 0)
            {
                pericodi = model.ListaPeriodos[0].Stpercodi;
            }
            if (pericodi > 0)
            {
                model.ListaRecalculo = this.servicioSistemasTransmision.ListStRecalculos(pericodi);
                if (model.ListaRecalculo.Count > 0 && recacodi == 0)
                {
                    recacodi = model.ListaRecalculo[0].Strecacodi;
                }
            }
            if (pericodi > 0 && recacodi > 0)
            {
                model.EntidadRecalculo = this.servicioSistemasTransmision.GetByIdStRecalculo(recacodi);
            }
            model.IdPeriodo = pericodi;
            model.IdRecalculo = recacodi;
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            return View(model);
        }

        [HttpPost]
        public ActionResult Lista(int recacodi)
        {
            ElementoStModel model = new ElementoStModel();
            model.ListaSistema = this.servicioSistemasTransmision.GetByCriteriaStSistematranss(recacodi);
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            return PartialView(model);
        }

        public ActionResult View(int id)
        {
            ElementoStModel model = new ElementoStModel();
            model.EntidadSistema = this.servicioSistemasTransmision.GetByIdStSistematrans(id);
            return PartialView(model);
        }

        public ActionResult New(int pericodi = 0, int recacodi = 0, int emprcodi = 0)
        {
            ElementoStModel model = new ElementoStModel();
            model.ListaEmpresa = (new EmpresaAppServicio()).ListEmpresas();

            model.EntidadEmpresa = (new EmpresaAppServicio()).GetByIdEmpresa(emprcodi);
            model.EntidadRecalculo = this.servicioSistemasTransmision.GetByIdStRecalculo(recacodi);

            model.EntidadSistema = new StSistematransDTO();
            model.EntidadPeriodo = new StPeriodoDTO();
            if (model.EntidadEmpresa == null)
            {
                return HttpNotFound();
            }
            model.IdEmpresa = emprcodi;
            model.EntidadSistema.Strecacodi = model.EntidadRecalculo.Strecacodi;
            model.EntidadSistema.Emprcodi = model.EntidadEmpresa.EmprCodi;
            model.EntidadSistema.Sistrncodi = 0;
            model.EntidadPeriodo.Stpercodi = pericodi;
            model.ListaSistema = this.servicioSistemasTransmision.ListStSistematranss(recacodi);
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(ElementoStModel model)
        {
            if (ModelState.IsValid)
            {
                model.EntidadSistema.Sistrnsucreacion = User.Identity.Name;
                model.EntidadSistema.Sistrnsumodificacion = User.Identity.Name;
                model.EntidadSistema.Sistrnfeccreacion = DateTime.Now;
                model.EntidadSistema.Sistrnfecmodificacion = DateTime.Now;
                model.EntidadFactorActualizacion = new StFactorDTO();
                model.EntidadFactorActualizacion.Stfactusucreacion = User.Identity.Name;
                model.EntidadFactorActualizacion.Stfactfeccreacion = DateTime.Now;
                model.EntidadFactorActualizacion.Stfacttor = 1;
                model.EntidadFactorActualizacion.Strecacodi = model.EntidadSistema.Strecacodi;


                if (model.EntidadSistema.Sistrncodi == 0)
                {
                    model.ListaSistema = this.servicioSistemasTransmision.ListStSistematranss(model.EntidadSistema.Strecacodi);
                    foreach (var item in model.ListaSistema)
                    {
                        if (model.EntidadSistema.Sistrnnombre == item.Sistrnnombre)
                        {
                            model.EntidadEmpresa = new EmpresaDTO();
                            model.ListaEmpresa = (new EmpresaAppServicio()).ListEmpresas();

                            model.sError = "Nombre de Sistema Repetido";
                            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                            return PartialView(model);
                        }
                    }
                    model.EntidadFactorActualizacion.Sistrncodi = model.ListaSistema.Count + 1;
                    this.servicioSistemasTransmision.SaveStSistematrans(model.EntidadSistema);
                    this.servicioSistemasTransmision.SaveStFactor(model.EntidadFactorActualizacion);
                }
                else
                {
                    this.servicioSistemasTransmision.UpdateStSistematrans(model.EntidadSistema);
                }
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return RedirectToAction("Index", new { pericodi = model.EntidadPeriodo.Stpercodi, recacodi = model.EntidadSistema.Strecacodi });
            }

            model.EntidadEmpresa = new EmpresaDTO();
            model.ListaEmpresa = (new EmpresaAppServicio()).ListEmpresas();
            model.sError = "Se ha producido un error al insertar la información";
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(model);
        }

        public ActionResult Edit(int id, int emprcodi = 0)
        {
            ElementoStModel model = new ElementoStModel();
            model.EntidadEmpresa = (new EmpresaAppServicio()).GetByIdEmpresa(emprcodi);
            model.EntidadPeriodo = new StPeriodoDTO();
            model.EntidadSistema = this.servicioSistemasTransmision.GetByIdStSistematrans(id);
            if (model.EntidadSistema == null)
            {
                return HttpNotFound();
            }
            if (emprcodi != 0)
            {
                model.EntidadSistema.Emprcodi = model.EntidadEmpresa.EmprCodi;
            }

            int IdRecalculo = model.EntidadSistema.Strecacodi;
            model.EntidadRecalculo = this.servicioSistemasTransmision.GetByIdStRecalculo(IdRecalculo);
            int IdPeriodo = model.EntidadRecalculo.Stpercodi;
            model.EntidadPeriodo.Stpercodi = IdPeriodo;

            model.ListaEmpresa = (new EmpresaAppServicio()).ListEmpresas();
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            return PartialView(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int id)
        {
            if (id > 0)
            {
                ElementoStModel model = new ElementoStModel();
                this.servicioSistemasTransmision.DeleteStFactor(id);
                this.servicioSistemasTransmision.DeleteStSistematrans(id);

                return "true";
            }
            return "False";
        }

        /// <summary>
        /// Permite exportar a un archivo excel la lista de sistemas de transmisión
        /// </summary>
        /// <param name="stpercodi">Código del Mes de cálculo</param>
        /// <param name="strecacodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarSistema(int stpercodi = 0, int strecacodi = 0, int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
                string file = "-1";

                file = this.servicioSistemasTransmision.GenerarListaSistemasTransmision(strecacodi, formato, pathFile, pathLogo);
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