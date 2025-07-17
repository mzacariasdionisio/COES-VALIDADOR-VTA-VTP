using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Publico.Areas.Publicaciones.Models;
using COES.Storage.App.Servicio;
using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata;
using COES.Storage.App.Metadata.Entidad;
using System.IO;
using System.Configuration;

namespace COES.MVC.Publico.Areas.Publicaciones.Controllers
{
    public class EstadisticasController : Controller
    {
        //
        // GET: /Publicaciones/EstadisticasA/
        FileManager fileManager = new FileManager();

        public string LocalDirectory = ConfigurationManager.AppSettings["RutaDirectorio"];
        public string Fileserver = ConfigurationManager.AppSettings["LocalDirectory"];

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Estadistica(int anio)
        {
            EstadisticasAnualesModel model = new EstadisticasAnualesModel();
            model.anio = anio;
            String ruta = "areas/Publicaciones/Content/componente/infografiacoesestadistica" + anio + "/estadistica" + anio + ".html";
            String ejecutiva = "Publicaciones/Estadisticas Anuales/" + anio + "/Estadistica Ejecutiva";
            FileInfo newFile = new FileInfo(this.LocalDirectory + ruta);

            if (newFile.Exists)
            {
                model.infografia = 1;
            }
            else model.infografia = 0;

            if (Directory.Exists(this.Fileserver + ejecutiva))
            {
                model.ejecutiva = 1;
            }
            else model.ejecutiva = 0;

            return View(model);
        }
        public PartialViewResult Detalle(int anio)
        {
            EstadisticasAnualesModel model = new EstadisticasAnualesModel();

            model.anio = anio;            

            List<WbBlobcolumnDTO> columnas = new List<WbBlobcolumnDTO>();

            string url = "Publicaciones/Estadisticas Anuales/" + anio + "/Excel/";
            string indicador = "S";
            string ordenamiento = string.Empty;
            string issuuind = string.Empty;
            string issuulink = string.Empty;
            string issuupos = string.Empty;
            string issuulenx = string.Empty;
            string issuuleny = string.Empty;
            bool indHeader = true;
            bool indicadorTree = false;

            model.ListadoExcel = this.fileManager.HabilitarVistaDatosWeb(url, out columnas, string.Empty, out ordenamiento, out issuuind,
                out issuulink, out issuupos, out issuulenx, out issuuleny, indicador, out indHeader, out indicadorTree, 1);

            return PartialView(model);
        }
        //public ActionResult Estadistica2019()
        //{
        //    return View();
        //}

        //public ActionResult Estadistica2020()
        //{
        //    return View();
        //}

        //public ActionResult Estadistica2021()
        //{
        //    return View();
        //}

        public ActionResult Visualizacion(string capitulo)
        {
            return View();
        }
    }
}
