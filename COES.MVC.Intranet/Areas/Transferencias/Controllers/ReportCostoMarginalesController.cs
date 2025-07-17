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
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Data;
using System.Collections;
using OfficeOpenXml;
using System.Globalization;
using OfficeOpenXml.Style;
using System.Drawing;
using COES.Servicios.Aplicacion.SIOSEIN;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class ReportCostoMarginalesController : Controller
    {
        public ActionResult Index()
        {

            ViewBag.cbPeriodo = new SelectList(new PeriodoAppServicio().ListPeriodo(), "PeriCodi", "PeriNombre");

            return View();
        }

        [HttpPost]
        public ActionResult BarrasMarginales(CostoMarginalDTO parametro)
        {
            return Json(new SicCostomarginalGraficosAppServicio().ListarBarraConFechaCMG(parametro));
        }
        [HttpPost]
        public ActionResult BarrasMarginalesDiarioMensual(CostoMarginalDTO parametro)
        {
            return Json(new SicCostomarginalGraficosAppServicio().ListarBarrasPorCMGDiarioMensual(parametro));
        }

        [HttpPost]
        public ActionResult ConsultaCostosMarginales(CostoMarginalDTO parametro)
        {
            JsonResult json = new JsonResult();
            json.MaxJsonLength = int.MaxValue;
            json.Data = new SicCostomarginalGraficosAppServicio().ListarCostoMarginalTotalPorBarra(parametro);
            return json;
        }
        [HttpPost]
        public ActionResult ListarCostoMarginalDesviacion(CostoMarginalDesviacionDTO parametro)
        {
            JsonResult json = new JsonResult();
            json.MaxJsonLength = int.MaxValue;
            json.Data = new SicCostomarginalGraficosAppServicio().ListarCostoMarginalDesviacion(parametro);
            return json;
        }

        [HttpPost]
        public ActionResult ListarPromedioMarginal(CostoMarginalDTO parametro)
        {
            return Json(new SicCostomarginalGraficosAppServicio().ListarPromedioMarginal(parametro));
        }

        [HttpPost]
        public ActionResult ListarVersiones(int periCodi)
        {
            List<RecalculoDTO> versiones = new RecalculoAppServicio().ListRecalculos(periCodi);
            return Json(versiones);
        }

        [HttpPost]
        public ActionResult ObtenerPeriodoPorId(int periCodi)
        {

            PeriodoDTO entidad = new PeriodoAppServicio().GetByIdPeriodo(periCodi);
            return Json(entidad);
        }
    }
}
