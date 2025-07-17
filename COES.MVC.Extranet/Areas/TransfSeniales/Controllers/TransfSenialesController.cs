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

namespace COES.MVC.Extranet.Areas.TransfSeniales.Controllers
{
    public class TransfSenialesController : BaseController
    {
        TransfSenialesAppServicio logic = new TransfSenialesAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(TransfSenialesController));
        EventoAppServicio evento = new EventoAppServicio();

        //DISPDIARIO

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("TransfSenialesController", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("TransfSenialesController", ex);
                throw;
            }
        }

        public TransfSenialesController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public ActionResult Index()
        {
            ValidarSesionUsuario();
            TransfSenialesModel model = new TransfSenialesModel();

            //model.GetListaEmpresaRis7 = this.seguridad.ObtenerEmpresasPorUsuario(User.Identity.Name).Select(x => new ScEmpresaDTO { Emprcodi = x.EMPRCODI, Emprenomb = x.EMPRNOMB }).ToList();


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

            model.FechaReporte = DateTime.Today.AddDays(-1).ToString(Constantes.FormatoFecha);



            return View(model);
        }




        [HttpPost]
        public PartialViewResult Lista(string fecha, int empresa)
        {
            TransfSenialesModel model = new TransfSenialesModel();

            DateTime fechaReportes = DateTime.ParseExact(fecha, Constantes.FormatoFecha,
                CultureInfo.InvariantCulture);

            SiEmpresaDTO empresas = (new EmpresaAppServicio()).ObtenerEmpresa(empresa);

            DateTime fechaCorte = DateTime.ParseExact("30/09/2018", Constantes.FormatoFecha, CultureInfo.InvariantCulture);



            List<TrEstadcanalrSp7DTO> list2 = new List<TrEstadcanalrSp7DTO>();

            if (fechaReportes <= fechaCorte)
            {
                //List<TrEstadcanalSp7DTO> list = this.logic.GetDispDiaSignal(fechaReportes, empresas.Scadacodi);

                List<TrEstadcanalSp7DTO> list = this.logic.GetDispDiaSignal(fechaReportes, empresas.Scadacodi);


                foreach (TrEstadcanalSp7DTO item in list)
                {
                    TrEstadcanalrSp7DTO rep = new TrEstadcanalrSp7DTO();
                    rep.Zonanomb = item.zona;
                    rep.Estcnltvalido = item.dispo;
                    rep.Canalnomb = item.nombcanal;
                    rep.Canaliccp = item.iccp;
                    rep.Canalunidad = item.unidad;
                    list2.Add(rep);
                }
            }
            else
            {
                //list2 = this.logic.GetListaDispMensualVersion(empresa.Scadacodi, fechas);
                //List<TrEstadcanalrSp7DTO> list = this.logic.GetDispDiaSignalVersion(fechaReportes, empresas.Scadacodi);
                list2 = this.logic.GetDispDiaSignalVersion(fechaReportes, empresas.Scadacodi);
            }

            model.GetDispDiaSignal7Version = list2;




            //model.GetDispDiaSignal7Version = list;

            model.Version = this.logic.GetVersion(fechaReportes);

            return PartialView(model);

        }

        [HttpPost]
        public JsonResult Version(string fecha)
        {
            DateTime fechaReportes = DateTime.ParseExact(fecha, Constantes.FormatoFecha,
                CultureInfo.InvariantCulture);

            int version = this.logic.GetVersion(fechaReportes);

            return Json(version);

        }



    }

}