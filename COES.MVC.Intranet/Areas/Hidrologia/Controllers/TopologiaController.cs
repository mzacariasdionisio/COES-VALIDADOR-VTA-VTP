using COES.MVC.Intranet.Areas.Hidrologia.Helper;
using COES.MVC.Intranet.Areas.Hidrologia.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Hidrologia;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Hidrologia.Controllers
{
    public class TopologiaController : Controller
    {
        //
        // GET: /Hidrologia/Topologia/
        HidrologiaAppServicio logic = new HidrologiaAppServicio();
        private GeneralAppServicio logicGeneral;

        public TopologiaController()
        {
            logicGeneral = new GeneralAppServicio();
        }

        /// <summary>
        /// Index para controller topologia
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            HidrologiaModel model = new HidrologiaModel();
            model.ListaEmpresas = logicGeneral.ObtenerEmpresasHidro();
            model.ListaCuenca = this.logic.ListarEquiposXFamilia(ConstantesHidrologia.IdCuenca);
            model.ListaTipoRecursos = this.logic.ListarFamilia().Where(x => x.Tareacodi == 4).ToList();
            return View(model);
        }

        /// <summary>
        /// Genera vista parcial para reporte de lista de topología
        /// </summary>
        /// <param name="sempresas"></param>
        /// <param name="scuencas"></param>
        /// <param name="srecursos"></param>
        /// <returns></returns>
        public PartialViewResult Lista(string sempresas, string scuencas, string srecursos)
        {
            HidrologiaModel model = new HidrologiaModel();
            model.ListaRecursosCuenca = this.logic.ListarRecursosxCuenca(sempresas, scuencas, srecursos);
            return PartialView(model);
        }

        // exporta el reporte general de topología consultado a archivo excel
        [HttpPost]
        public JsonResult GenerarReporteXLS(string sempresas, string scuencas, string srecursos)
        {
            int indicador = 1;
            HidrologiaModel model = new HidrologiaModel();
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
            try
            {
                model.ListaRecursosCuenca = this.logic.ListarRecursosxCuenca(sempresas, scuencas, srecursos);
                ExcelDocument.GenerarArchivoTopologia(model,ruta);
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Permite exportar el reporte general y por tipo de mantenimientos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarReporte()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesHidrologia.NombreReporteTopologia;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }
    }
}
