using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Configuration;
using COES.MVC.Intranet.Controllers;
using COES.Framework.Base.Core;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class TransferenciaRentaCongestionController : Controller
    {
        //
        // GET: /Transferencias/TransferenciaRentaCongestion/

        TransferenciaInformacionBaseAppServicio servicio = new TransferenciaInformacionBaseAppServicio();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListarPeriodosRentaCongestion(int nroPagina)
        {
            TransferenciaRentaCongestionModel transferenciaRentaCongestion = new TransferenciaRentaCongestionModel();

            int regIni = 0;
            int regFin = 0;

            regIni = (nroPagina - 1) * Funcion.PageSizePeriodoRentaCongestion + 1;
            regFin = nroPagina * Funcion.PageSizePeriodoRentaCongestion;

            transferenciaRentaCongestion.ListaPeriodosRentaCongestion = servicio.ListPeriodosRentaCongestion(0, 0, regIni, regFin);

            return PartialView("ListarPeriodosRentaCongestion", transferenciaRentaCongestion);

        }

        /// <summary>
        /// Permite generar la vista del paginado
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado()
        {
            Paginacion model = new Paginacion();

            int nroRegistros = this.servicio.ListPeriodosRentaCongestionCount();

            if (nroRegistros > Funcion.NroPageShow)
            {
                int pageSize = Funcion.PageSizePeriodoRentaCongestion;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Funcion.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);

        }

        [HttpPost]
        public JsonResult ObtenerCantidadRegistrosPeriodo()
        {
            int nroRegistros = 0;
            try
            {
                nroRegistros = this.servicio.ListPeriodosRentaCongestionCount();

                return Json(nroRegistros.ToString());
            }
            catch
            {
                return Json("-1");
            }

        }
        
    }
}
