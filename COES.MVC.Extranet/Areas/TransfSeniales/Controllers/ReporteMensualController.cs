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
using COES.Dominio.DTO.Sp7;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Medidores;
using COES.Servicios.Aplicacion.FormatoMedicion;

namespace COES.MVC.Extranet.Areas.TransfSeniales.Controllers
{
    public class ReporteMensualController : BaseController
    {
        TransfSenialesAppServicio logic = new TransfSenialesAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        EventoAppServicio evento = new EventoAppServicio();

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ReporteMensualController));
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("ReporteMensualController", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("ReporteMensualController", ex);
                throw;
            }
        }

        public ReporteMensualController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public ActionResult IndexMensual()
        {
            ValidarSesionUsuario();
            TransfSenialesModel model = new TransfSenialesModel();

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


            model.FechaPeriodo = DateTime.Now.ToString(Constantes.FormatoMesAnio);
            model.fechahoraactual = DateTime.Now;
            return View(model);
        }


        [HttpPost]
        public PartialViewResult ListaMensual(int emprcodi, string fechaPeriodo)
        {
            TransfSenialesModel model = new TransfSenialesModel();

            DateTime fechas = DateTime.ParseExact(fechaPeriodo, Constantes.FormatoMesAnio,
            CultureInfo.InvariantCulture);

            SiEmpresaDTO empresa = (new EmpresaAppServicio()).ObtenerEmpresa(emprcodi);

            if (empresa == null || empresa.Scadacodi == 0)
            {
                return PartialView(model);
            }

            DateTime fechaCorte = DateTime.ParseExact("01/09/2018", Constantes.FormatoFecha, CultureInfo.InvariantCulture);


            List<TrReporteversionSp7DTO> list2 = new List<TrReporteversionSp7DTO>();

            if (fechas <= fechaCorte)
            {
                List<TrIndempresatSp7DTO> list = this.logic.GetListaDispMensual(empresa.Scadacodi, fechas);
                list = this.logic.GetListaDispMensual(empresa.Scadacodi, fechas);

                foreach (TrIndempresatSp7DTO item in list)
                {
                    TrReporteversionSp7DTO rep = new TrReporteversionSp7DTO();
                    rep.Revfecha = item.Fecha;
                    rep.Revciccpe = item.disponibilidad;
                    list2.Add(rep);
                }
            }
            else
            {
                list2 = this.logic.GetListaDispMensualVersion(empresa.Scadacodi, fechas);
            }

            model.GetDispMensual7Version = list2;

            return PartialView(model);

        }



    }

}