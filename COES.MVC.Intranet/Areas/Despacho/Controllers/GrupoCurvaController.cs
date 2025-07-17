using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Despacho.Helper;
using COES.MVC.Intranet.Areas.Despacho.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Despacho;
using COES.Servicios.Aplicacion.Despacho.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace COES.MVC.Intranet.Areas.Despacho.Controllers
{
    public class GrupoCurvaController : BaseController
    {

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>  
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(GrupoCurvaController));

        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        GrupoCurvaAppServicio servicio = new GrupoCurvaAppServicio();

        /// <summary>
        /// Permite mostrar la página inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            return View();
        }

        /// <summary>
        /// Permite pintar la lista de grupos curva
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Listado()
        {
            GrupoCurvaModel model = new GrupoCurvaModel();
            model.ListaCentral = this.servicio.ObtenerGruposCurva();

            return PartialView(model);
        }


        /// <summary>
        /// Permite grabar un nuevo grupo curva
        /// </summary>
        /// <param name="model">model ContactoModel</param>
        /// <returns></returns>
        public JsonResult GrabarNuevo(EntidadGrupoCurvaModel model)
        {
            try
            {

                UserDTO userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]);

                PrCurvaDTO entidad = new PrCurvaDTO();
                entidad.Curvestado = ConstantesDespacho.Activo;
                entidad.Curvnombre = model.Nombres;


                entidad.Curvusucreacion = userLogin.UserCode.ToString();
                entidad.Curvfeccreacion = DateTime.Now;

                servicio.Insertar(entidad);

                return Json(1);

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite cargar los datos del grupo curva seleccionado
        /// </summary>
        /// <param name="CurvCodi">Codigo de Grupo Curva</param>
        [HttpPost]
        public PartialViewResult Edicion(int CurvCodi)
        {
            EntidadGrupoCurvaModel model = new EntidadGrupoCurvaModel();
            var Datos = this.servicio.GetById(CurvCodi);

            model.Codigo = Datos.Curvcodi;
            model.Nombres = Datos.Curvnombre;

            return PartialView(model);
        }


        /// <summary>
        /// Permite actualizar los datos de grupo curva
        /// </summary>
        /// <param name="model">model EntidadGrupoCurvaModel</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarEdicion(EntidadGrupoCurvaModel model)
        {
            try
            {
                UserDTO userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]);
                var entidad = this.servicio.GetById(model.Codigo);

                entidad.Curvnombre = model.Nombres;



                entidad.Curvusumodificacion = userLogin.UserCode.ToString();
                entidad.Curvfecmodificacion = DateTime.Now;

                this.servicio.Actualizar(entidad);

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }



        /// <summary>
        /// Permite guardar el detalle los datos de grupo curva
        /// </summary>
        /// <param name="model">model EntidadDetalleGrupoCurvaModel</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarDetalle(EntidadDetalleGrupoCurvaModel model)
        {
            try
            {
                bool principal = false;

                if (model.grupoPrincipal == Constantes.SI)
                    principal = true;
                else
                    principal = false;

                this.servicio.AgregarDetalle(model.codigoCurva, model.codigoGrupo, principal);

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite eliminar un elemento de detalle grupo curva seleccionado
        /// </summary>
        /// <param name="CurvCodi">Codigo de curva</param>
        /// <param name="GrupoCodi">Codigo de grupo</param>
        [HttpPost]
        public JsonResult EliminarDetalle(int CurvCodi, int GrupoCodi)
        {
            try
            {

                this.servicio.DeleteDetail(CurvCodi, GrupoCodi);

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite cargar los detalles del grupo curva seleccionado
        /// </summary>
        /// <param name="CurvCodi">Codigo de Grupo Curva</param>
        [HttpPost]
        public PartialViewResult Detalle(int CurvCodi)
        {
            DetalleGrupoCurvaModel model = new DetalleGrupoCurvaModel();
            var Datos = this.servicio.GetById(CurvCodi);

            model.Codigo = Datos.Curvcodi;
            model.Nombres = Datos.Curvnombre;

            return PartialView(model);
        }

        /// <summary>
        /// Permite pintar la lista de grupos curva
        /// </summary>
        /// <param name="CurvCodi">Codigo de curva</param>
        /// <returns></returns>
        public PartialViewResult ListadoDetalle(int CurvCodi)
        {
            DetalleGrupoCurvaModel model = new DetalleGrupoCurvaModel();
            model.ListaDetalle = this.servicio.ObtenerDetalleGrupoCurva(CurvCodi);

            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar el grupo curva seleccionado
        /// </summary>
        /// <param name="CurvCodi">Codigo de curva</param>
        [HttpPost]
        public JsonResult Eliminar(int CurvCodi)
        {
            try
            {

                this.servicio.Delete(CurvCodi);

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite otener las centrales por tipo de central
        /// </summary>
        /// <param name="tipoCentral"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Central(string tipoCentral)
        {
            GrupoDespachoModel model = new GrupoDespachoModel();
            List<PrGrupoDTO> list = this.servicio.ObtenerCentrales(tipoCentral);
            model.ListaCentral = list;

            return PartialView(model);
        }

        /// <summary>
        /// Permite otener grupo por codigo grupoA
        /// </summary>
        /// <param name="tipoGrupoA"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult GrupoA(string codigoGrupo)
        {
            GrupoDespachoModel model = new GrupoDespachoModel();
            List<PrGrupoDTO> list = this.servicio.ObtenerGrupo(codigoGrupo);
            model.ListaCentral = list;

            return PartialView(model);
        }


        /// <summary>
        /// Permite otener grupo por codigo grupoB
        /// </summary>
        /// <param name="tipoGrupoA"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult GrupoB(string codigoGrupo)
        {
            GrupoDespachoModel model = new GrupoDespachoModel();
            List<PrGrupoDTO> list = this.servicio.ObtenerGrupo(codigoGrupo);
            model.ListaCentral = list;

            return PartialView(model);
        }

    }
}
