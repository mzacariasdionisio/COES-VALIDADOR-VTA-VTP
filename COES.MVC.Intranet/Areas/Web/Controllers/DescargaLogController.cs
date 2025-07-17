using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Web.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Web.Controllers
{


    public class DescargaLogController : BaseController
    {

        
        PortalAppServicio servicio = new PortalAppServicio();
        //Metodos de listado y descarga
        public ActionResult Index()
        {

            

            return View();
        }

        public PartialViewResult Listar(string url,string fechaInicio, string fechaFin)
        {
            SubscripcionModel model = new SubscripcionModel();
            
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            List<WbBlobDTO> archivos = servicio.GetWbBlobByUrlParcial(url, fechaInicial, fechaFinal).OrderByDescending(x => x.Blobdateupdate).ToList();

            return PartialView(archivos);
        }


    }
}