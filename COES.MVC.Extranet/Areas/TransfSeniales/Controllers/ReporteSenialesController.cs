using COES.MVC.Extranet.Areas.TransfSeniales.Helper;
using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Areas.TransfSeniales;
using COES.MVC.Extranet.Areas.TransfSeniales.Models;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.TiempoReal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.TransfSeniales;
using log4net;
using COES.MVC.Extranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.RegistroObservacion;
using COES.Servicios.Aplicacion.Eventos;

namespace COES.MVC.Extranet.Areas.TransfSeniales.Controllers
{
    public class ReporteSenialesController : BaseController      
      {
        TransfSenialesAppServicio logic = new TransfSenialesAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        EventoAppServicio evento = new EventoAppServicio();

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ReporteSenialesController));
          protected override void OnException(ExceptionContext filterContext)
          {
              try
              {
                  log4net.Config.XmlConfigurator.Configure();
                  Exception objErr = filterContext.Exception;
                  Log.Error("ReporteSenialesController", objErr);
              }
              catch (Exception ex)
              {
                  Log.Fatal("ReporteSenialesController", ex);
                  throw;
              }
          }

          public ReporteSenialesController()
          {
              log4net.Config.XmlConfigurator.Configure();
          }

        public ActionResult IndexSeniales()
        {
            ValidarSesionUsuario();
            TransfSenialesModel model = new TransfSenialesModel();
            //model.GetListaEmpresaRis7 = this.seguridad.ObtenerEmpresasPorUsuario(User.Identity.Name).Select(x => new ScEmpresaDTO { Emprcodi = x.EMPRCODI, Emprenomb = x.EMPRNOMB }).ToList();

            model.fechahoraactual = DateTime.Now;
            
            if (base.VerificarAccesoAccion(Acciones.AccesoEmpresa, base.UserName))
            {
                model.GetListaEmpresaRis7 = evento.ListarEmpresas().Where(x => (x.SCADACODI > 0) && (x.EMPRESTADO == "A")).Select(x =>
                    new ScEmpresaDTO { Emprcodi = x.EMPRCODI, Emprenomb = x.EMPRRAZSOCIAL }).OrderBy(x => x.Emprenomb).ToList();
            }
            else
            {
                model.GetListaEmpresaRis7 = this.seguridad.ObtenerEmpresasPorUsuario(User.Identity.Name).Select(x =>
                    new ScEmpresaDTO { Emprcodi = x.EMPRCODI, Emprenomb = x.EMPRNOMB }).ToList();
            }

            if (base.UserName == "gignacio@rep.com.pe" || base.UserName == "mauris@rep.com.pe" || base.UserName == "cc-rep@rep.com.pe")
            {
                model.GetListaEmpresaRis7.Add(new ScEmpresaDTO { Emprcodi = 39, Emprenomb = "ETESELVA" });
                model.GetListaEmpresaRis7.Add(new ScEmpresaDTO { Emprcodi = 10465, Emprenomb = "ETENORTE" });
            }

            if (model.GetListaEmpresaRis7.Count == 1)
            {
                model.IdEmpresa = model.GetListaEmpresaRis7[0].Emprcodi;
            }
            return View(model);
        }




        [HttpPost]
        public PartialViewResult ListaSeniales(int emprcodi)
        {
            TransfSenialesModel model = new TransfSenialesModel();

            SiEmpresaDTO empresa = (new EmpresaAppServicio()).ObtenerEmpresa(emprcodi);

            List<TrMuestrarisSp7DTO> list = this.logic.GetListMuestraRis(empresa.Scadacodi);

            model.GetMuestraRis7 = list;

            return PartialView(model);

        }



      }

}