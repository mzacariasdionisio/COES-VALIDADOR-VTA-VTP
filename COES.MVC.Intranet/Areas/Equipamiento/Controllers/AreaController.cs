using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.MVC.Intranet.Helper;
using log4net;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Equipamiento.Helper;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class AreaController : Controller
    {
        /// <summary>
        /// Instancia de clase para el acceso a datos
        /// </summary>
        EquipamientoAppServicio servicio = new EquipamientoAppServicio();
        readonly List<EstadoModel> lsEstadosArea= new List<EstadoModel>();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(AreaController));
        public AreaController()
        {
            lsEstadosArea.Add(new EstadoModel{EstadoCodigo = "A",EstadoDescripcion = "Activo"});
            lsEstadosArea.Add(new EstadoModel { EstadoCodigo = "B", EstadoDescripcion = "Baja" });
            lsEstadosArea.Add(new EstadoModel { EstadoCodigo = "P", EstadoDescripcion = "Pendiente" });
            lsEstadosArea.Add(new EstadoModel { EstadoCodigo = "X", EstadoDescripcion = "Anulado" });
            log4net.Config.XmlConfigurator.Configure();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("AreaController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("AreaController", ex);
                throw;
            }
        }
        //
        // GET: /Equipamiento/Area/

        public ActionResult Index()
        {
            AreaModel modelo = new AreaModel();
            modelo.ListaTipoArea = servicio.ListEqTipoareas();
            modelo.idTipoArea = 0;
            modelo.ListaEstados = lsEstadosArea;
            bool AccesoNuevo = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Nuevo);
            bool AccesoEditar = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Editar);
            modelo.EnableNuevo = AccesoNuevo ? "" : "disabled=disabled";
            modelo.EnableEditar = AccesoEditar ? "" : "disableClick";
            return View(modelo);
        }

        [ActionName("Index"), HttpPost]
        public ActionResult IndexPost(AreaModel datosVentana)
        {
            datosVentana.ListaTipoArea = servicio.ListEqTipoareas();
            datosVentana.ListaEstados = lsEstadosArea;
            return View(datosVentana);
        }


        //public ActionResult ListadoAreas(string sidx, string sord, int page, int rows,string pTipoArea,string pNombre)
        //{
        //    int totalPages = 0;
        //    int iTipoArea = pTipoArea=="0"?-99:Convert.ToInt32(pTipoArea);
        //    var lsResultado = servicio.ListarxFiltro(iTipoArea, pNombre, page, Constantes.PageSizeEvento);
        //    int totalRecords = servicio.CantidadListarxFiltro(iTipoArea, pNombre);
        //    totalPages = (totalRecords % rows == 0) ? totalRecords / rows : totalRecords / rows + 1;
        //    var jsondata = new
        //    {
        //        total = totalPages,
        //        page,
        //        records = totalRecords,
        //        rows = (
        //        from q in lsResultado
        //        select new
        //        {
        //            cell = new string[]
        //            {
        //                q.Areacodi.ToString(),
        //                q.Areanomb,
        //                string.IsNullOrEmpty(q.Areaabrev) ?"":q.Areaabrev,
        //                q.Areacodi.ToString(),
        //            }
        //        }
        //        ).ToArray()
        //    };
        //    return Json(jsondata, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public PartialViewResult ListarAreas(AreaModel datosVentana)
        {
            datosVentana.IndicadorPagina = false;
            int iTipoArea = datosVentana.idTipoArea == 0 ? -99 : datosVentana.idTipoArea;
            string sEstadoArea = datosVentana.EstadoCodigo ?? " ";
            var sNombreArea = string.IsNullOrEmpty(datosVentana.NombreArea)?"":datosVentana.NombreArea.ToUpperInvariant();
            int nroRegistros = servicio.CantidadListarxFiltro(iTipoArea, sNombreArea, sEstadoArea);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeEvento;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                datosVentana.NroPaginas = nroPaginas;
                datosVentana.NroMostrar = Constantes.NroPageShow;
                datosVentana.IndicadorPagina = true;
            }

            var lsResultado = servicio.ListarxFiltro(iTipoArea, sNombreArea, sEstadoArea, datosVentana.NroPagina, Constantes.PageSizeEvento);
            foreach (var oArea in lsResultado)
            {
                oArea.EstadoDesc = EquipamientoHelper.EstadoDescripcion(oArea.Areaestado);
                oArea.UsuarioCreacion = EquipamientoHelper.EstiloEstado(oArea.Areaestado);
            }
            datosVentana.ListaArea = lsResultado;

            return PartialView(datosVentana);
        }

        public PartialViewResult Detalle(int id)
        {
            var oArea= servicio.GetByIdEqArea(id);
            var modelo = new AreaDetalleModel();
            modelo.Areaabrev = oArea.Areaabrev;
            modelo.Areacodi = oArea.Areacodi;
            modelo.Areanomb = oArea.Areanomb;
            modelo.Areapadre = oArea.Areapadre;
            modelo.Tareacodi = oArea.Tareacodi;
            modelo.AreaUsuIns = oArea.UsuarioCreacion;
            modelo.FechaIns = (DateTime.Equals(oArea.FechaCreacion, null) ? "" : oArea.FechaCreacion.ToString("dd/MM/yyyy"));
            modelo.AreaUsuUpd = oArea.UsuarioUpdate;
            modelo.FechaUpd = (DateTime.Equals(oArea.FechaUpdate, null) ? "" : oArea.FechaUpdate.ToString("dd/MM/yyyy"));
            modelo.EstadoDesc = EquipamientoHelper.EstadoDescripcion(oArea.Areaestado);

            return PartialView(modelo);
        }
        
        public PartialViewResult Editar(int id)
        {
            var oArea = servicio.GetByIdEqArea(id);
            var modelo = new AreaDetalleModel();
            modelo.Areaabrev = oArea.Areaabrev;
            modelo.Areacodi = oArea.Areacodi;
            modelo.Areanomb = oArea.Areanomb;
            modelo.Areapadre = oArea.Areapadre;
            modelo.Tareacodi = oArea.Tareacodi;
            modelo.ListaTipoArea = servicio.ListEqTipoareas();
            modelo.lsEstado = lsEstadosArea;
            modelo.EstadoCodigo = oArea.Areaestado;

            return PartialView(modelo);
        }

        //[ActionName("Index"), HttpPost]
        public JsonResult EditarPost(int iAreaCodi, string sAreanomb, string sAreaabrev, int idTipoArea, string sEstado)
        {
            try
            {
                var iCodArea = Convert.ToInt32(iAreaCodi);
                var oArea = servicio.GetByIdEqArea(iCodArea);
                oArea.Areaabrev = sAreaabrev;
                oArea.Areanomb = sAreanomb;
                oArea.Tareacodi = Convert.ToInt32(idTipoArea);
                oArea.UsuarioUpdate = User.Identity.Name;
                oArea.Areaestado = sEstado;
                servicio.UpdateEqArea(oArea);
                return Json(1);
            }
            catch (Exception)
            {
                return Json(-1);
            }
        }

        public PartialViewResult NuevaArea()
        {
            var modeloNuevo = new AreaDetalleModel();
            modeloNuevo.ListaTipoArea = servicio.ListEqTipoareas();
            modeloNuevo.lsEstado = lsEstadosArea;
            return PartialView(modeloNuevo);
        }

        public JsonResult GuardarArea(int iTipoArea,string sNombreArea,string sAreaAbrev,string sEstado)
        {
            try
            {
                var oNuevaArea= new EqAreaDTO();
                oNuevaArea.Areaabrev = sAreaAbrev;
                oNuevaArea.Areanomb = sNombreArea;
                oNuevaArea.Areapadre = -1;
                oNuevaArea.Areaestado = sEstado;
                oNuevaArea.Tareacodi = iTipoArea;
                oNuevaArea.UsuarioCreacion = User.Identity.Name;
                servicio.SaveEqArea(oNuevaArea);
                return Json(1);
            }
            catch (Exception)
            {
                return Json(-1);
            }
        }

    }
}
