using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.MVC.Intranet.Controllers;
using System.Reflection;
using log4net;
using System.Globalization;
using System.Data;
using System.Configuration;


namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class ConceptoController : BaseController
    {
        // GET: /Transferencias/Concepto/
        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        TransferenciasAppServicio servicioTransferencias = new TransferenciasAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        TipoEmpresaAppServicio servicioTipoEmpresa = new TipoEmpresaAppServicio();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Detalle() 
        {
            List<TrnInfoadicionalDTO> detalle = new List<TrnInfoadicionalDTO>();
            detalle = servicioTransferencias.ListTrnInfoadicionals();
            return Json(detalle);
        }

        public JsonResult Nuevo() {
            ConceptoModel model = new ConceptoModel();
            model.ListaEmpresa = servicioEmpresa.ListEmpresas();
            model.ListaTipoEmpresa = servicioTipoEmpresa.ListTipoEmpresas();
            return Json(model);
        }
        
        public JsonResult Grabar(int infadicodi, string infadinomb, string infadicodosinergmin, int tipoemprcodi, int emprcodi) 
        {
            TrnInfoadicionalDTO entity = new TrnInfoadicionalDTO();
            entity.Infadinomb = infadinomb;
            entity.Infadicodosinergmin = infadicodosinergmin;
            entity.Tipoemprcodi = tipoemprcodi;
            if (emprcodi == -1)
                entity.Emprcodi = null;
            else
                entity.Emprcodi = emprcodi;
            if (infadicodi == 0) { 
                entity.Infadiestado = "I";
                entity.UsuCreacion = base.UserName;
                servicioTransferencias.SaveTrnInfoadicional(entity);
            }
            else
            {
                TrnInfoadicionalDTO entityHist = servicioTransferencias.GetByIdTrnInfoadicional(infadicodi);
                entityHist.UsuUpdate = base.UserName;
                servicioTransferencias.UpdateTrnInfoadicional(entityHist);
                entity.Infadicodi = infadicodi;
                entity.Infadiestado = "I";
                entity.UsuCreacion = base.UserName;
                servicioTransferencias.SaveTrnInfoadicional(entity);
            }
            return Json("");
        }

        public JsonResult Edit(int infadicodi)
        {
            ConceptoModel model = new ConceptoModel();
            model.EntidadConcepto = servicioTransferencias.GetByIdTrnInfoadicional(infadicodi);
            model.ListaEmpresa = servicioEmpresa.ListEmpresas();
            model.ListaTipoEmpresa = servicioTipoEmpresa.ListTipoEmpresas();
            return Json(model);
        }

        public JsonResult Delete(int infadicodi) 
        {
            TrnInfoadicionalDTO entity = servicioTransferencias.GetByIdTrnInfoadicional(infadicodi);
            entity.UsuUpdate = base.UserName;
            servicioTransferencias.UpdateTrnInfoadicional(entity);
            //servicioTransferencias.DeleteTrnInfoadicional(infadicodi);
            return Json("");
        }

        public JsonResult DetalleConcepto(int infadicodi)
        {
            List<TrnInfoadicionalDTO> detalle = new List<TrnInfoadicionalDTO>();
            detalle = servicioTransferencias.ListVersionTrnInfoadicionals(infadicodi);
            return Json(detalle);
        }
    }
}
