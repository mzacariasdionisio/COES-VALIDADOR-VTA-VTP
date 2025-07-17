using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.MVC.Intranet.App_Start;
using COES.MVC.Intranet.Areas.GestionEoEpo.Helper;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.GestionEoEpo;
//using DocumentFormat.OpenXml.Spreadsheet;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.GestionEoEpo.Controllers
{
    public class PuntoConexionController : BaseController
    {
        GestionEoEpoAppServicio _svcGestionEoEpo = new GestionEoEpoAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(EstudioEoController));

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("EstudioEoController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("EstudioEoController", ex);
                throw;
            }
        }

        #region Propiedades

        /// <summary>
        /// Codigo de formato
        /// </summary>
        public int IdFormato = 4;
        public int IdLectura = 51;
        public string[][] MatrizExcel
        {
            get
            {
                return (Session["MatrizExcel"] != null) ?
                    (string[][])Session["MatrizExcel"] : new string[1][];
            }
            set { Session["MatrizExcel"] = value; }
        }
        public String FileName
        {
            get
            {
                return (Session["FileName"] != null) ?
                    Session["FileName"].ToString() : null;
            }
            set { Session["FileName"] = value; }
        }
        public String NombreFile
        {
            get
            {
                return (Session["NombreArchivo"] != null) ?
                    Session["NombreArchivo"].ToString() : null;
            }
            set { Session["NombreArchivo"] = value; }
        }
        #endregion

        [ValidarSesion]
        public ActionResult Index()
        {
            List<EpoZonaDTO> listadoZonaProyecto = _svcGestionEoEpo.ListarZona();
            ViewBag.ListadoZonaProyecto = listadoZonaProyecto;
            return View();
        }

        [HttpPost]
        public ActionResult ListadoPuntoConexionEpo(EpoPuntoConexionDTO estudioepo)
        {
            List<EpoPuntoConexionDTO> listadoEstudioEpo = new List<EpoPuntoConexionDTO>();

            try
            {
                listadoEstudioEpo = _svcGestionEoEpo.GetByCriteriaEpoPuntoConexionEpos(estudioepo);
            }
            catch (Exception ex)
            {
                log.Error("ListadoPuntoConexionEpo", ex);
            }

            return PartialView(listadoEstudioEpo);
        }


        [ValidarSesion]
        public ActionResult RegistrarPuntoConexionEpo(int id)
        {
            EpoPuntoConexionDTO puntoConexionEpo = _svcGestionEoEpo.GetByIdEpoPuntoConexionEpo(id);

            ViewBag.Titulo = "Modificar Punto de Conexión EO / EPO";

            if (puntoConexionEpo == null)
            {
                puntoConexionEpo = new EpoPuntoConexionDTO();
                ViewBag.Titulo = "Registrar Punto de Conexión EO / EPO";
            }

            List<EpoZonaDTO> listadoZona = _svcGestionEoEpo.ListarZona();
            ViewBag.ListadoZonaProyecto = listadoZona;
          

            return PartialView(puntoConexionEpo);
        }

        [HttpPost]
        public JsonResult RegistrarPuntoConexionEpo(EpoPuntoConexionDTO puntoConex)
        {
            
            try
            {
                
                puntoConex.LastUser = (string)Session["Usuario"];
                puntoConex.LastDate = DateTime.Now;

                int estudioEpoPuntCode = 0;
                if (puntoConex.PuntCodi == 0)
                {
                    EpoPuntoConexionDTO puntoConexionEpo = _svcGestionEoEpo.GetByDescripcionEpoPuntoConexionEpo(puntoConex.PuntDescripcion);
                    if (puntoConexionEpo != null)
                    {
                        return Json(2, JsonRequestBehavior.AllowGet);
                    }

                    puntoConex.PuntActivo = "1";
                    estudioEpoPuntCode = _svcGestionEoEpo.SaveEpoPuntoConexionEpo(puntoConex);
                }
                else
                {

                    EpoPuntoConexionDTO objAnterior = _svcGestionEoEpo.GetByIdEpoPuntoConexionEpo(puntoConex.PuntCodi);
                    if (objAnterior.PuntDescripcion != puntoConex.PuntDescripcion) 
                    {
                        EpoPuntoConexionDTO puntoConexionEpo = _svcGestionEoEpo.GetByDescripcionEpoPuntoConexionEpo(puntoConex.PuntDescripcion);
                        if (puntoConexionEpo != null)
                        {
                            return Json(2, JsonRequestBehavior.AllowGet);
                        }
                    }

                    



                    puntoConex.PuntActivo = "1";
                    estudioEpoPuntCode = puntoConex.PuntCodi;
                    _svcGestionEoEpo.UpdateEpoPuntoConexionEpo(puntoConex);
                }
            }
            catch (Exception ex)
            {
                log.Error("RegistrarEstudioEPO", ex);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult Paginado(EpoPuntoConexionDTO estudioepo)
        {
            Paginacion model = new Paginacion();

            int nroRegistros = _svcGestionEoEpo.ObtenerNroRegistroBusquedaEpoPuntoConexionEpos(estudioepo);
            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = estudioepo.nroFilas == 0 ? Constantes.PageSize : estudioepo.nroFilas;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            string[] cantidadRegistros = { "20", "30", "50", "100" };

            model.CantidadRegistros = cantidadRegistros;

            return base.Paginado(model);
        }

        public JsonResult Anular(int puntCodi)
        {
            string strMsg = string.Empty;
            bool bResult = true;

            try
            {
                EpoPuntoConexionDTO _estudioEpo = _svcGestionEoEpo.GetByIdEpoPuntoConexionEpo(puntCodi);

                _estudioEpo.LastUser = (string)Session["Usuario"];
                _estudioEpo.LastDate = DateTime.Now;

                _estudioEpo.PuntActivo = "0"; //Anulado;

                _svcGestionEoEpo.UpdateEpoPuntoConexionEpo(_estudioEpo);
            }
            catch (Exception ex)
            {
                bResult = false;
                strMsg = ex.Message.ToString();
                log.Error("AnularEPO", ex);
            }

            var rpta = new
            {
                sMensaje = strMsg,
                bResult = bResult

            };
            return Json(rpta);
        }

    }
}

