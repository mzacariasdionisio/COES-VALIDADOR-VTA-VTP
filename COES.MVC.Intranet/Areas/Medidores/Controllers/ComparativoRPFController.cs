using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Medidores.Models;
using COES.MVC.Intranet.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Medidores.Controllers
{
    public class ComparativoRPFController : Controller
    {
        /// <summary>
        /// Primera pantalla del aplicativo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ComparativoRPFModel model = new ComparativoRPFModel();
            model.ListaEmpresa = new List<SiEmpresaDTO>();
            model.FechaConsulta = DateTime.Now.ToString(Constantes.FormatoFecha);

            return View(model);
        }

    }
}
