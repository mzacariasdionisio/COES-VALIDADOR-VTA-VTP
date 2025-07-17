using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Controllers;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.Servicios.Aplicacion.ReportesFrecuencia;
using COES.MVC.Intranet.Areas.ReportesFrecuencia.Models;
using COES.MVC.Intranet.Areas.ReportesFrecuencia.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.IO;
using System.Globalization;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Configuration;
using COES.MVC.Intranet.SeguridadServicio;
using COES.MVC.Intranet.Areas.PMPO.Controllers;


namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Controllers
{
    public class EtapaERAController : BaseController
    {
        /// <summary>
        /// Instancia del Web Service de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        public ActionResult Index()
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            EtapaERAModel model = new EtapaERAModel();

            model.bNuevo = true;
            model.bEditar = true;
            model.bGrabar = true;
            return View(model);
        }


        public ActionResult Lista()
        {
            EtapaERAModel model = new EtapaERAModel();
            model.ListaEtapas = new EtapaERAAppServicio().GetListaEtapas();
            return PartialView(model);
        }

        public ActionResult ListaSelect(string ListaEtapas)
        {
            //string[] arrayIdEtapa = ListaEtapas.Split('|');

            EtapaERAModel model = new EtapaERAModel();
            model.ListaEtapas = new EtapaERAAppServicio().GetListaEtapas();
            model.IdEtapas = ListaEtapas;
            return PartialView(model);
        }

        public ActionResult Delete(int id)
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            EtapaERAModel modelo = new EtapaERAModel();
            modelo.Entidad = new EtapaERAAppServicio().GetBydId(id);

            return View(modelo);
        }

        public ActionResult New()
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            int UltCodGenerado = new EtapaERAAppServicio().GetUltimoCodigoGenerado();

            EtapaERAModel modelo = new EtapaERAModel();
            modelo.Entidad = new EtapaERADTO();
            modelo.Entidad.EtapaCodi = UltCodGenerado;
            modelo.Entidad.EtapaEstado = "A";
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

            return View(modelo);
        }

        public ActionResult Edit(int id)
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            EtapaERAModel modelo = new EtapaERAModel();
            modelo.Entidad = new EtapaERAAppServicio().GetBydId(id);
            //modelo.Entidad.GPSCodiOriginal = modelo.Entidad.GPSCodi;
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

            

            return View(modelo);
        }

        public ActionResult Save(EtapaERAModel modelo)
        {
            string mensajeError = "Se ha producido un error al insertar la información";
            if (modelo.sAccion == "Eliminar")
            {
                ResultadoDTO<EtapaERADTO> resultado = new EtapaERAAppServicio().Eliminar(modelo.Entidad);
                if (resultado.EsCorrecto >= 0)
                {
                    TempData["sMensajeExito"] = "Se ha eliminado correctamente.";
                    mensajeError = "";
                }
                else
                {
                    mensajeError = resultado.Mensaje;
                }
            }
            else
            {
                int numResultado = new EtapaERAAppServicio().ValidarNombreEtapa(modelo.Entidad);

                if (numResultado > 0)
                {
                    mensajeError = "El nombre de etapa ya existe";
                }
                else
                {
                    if (modelo.sAccion == "Insertar")
                    {
                        modelo.Entidad.EtapaEstado = "A";
                        modelo.Entidad.EtapaCodi = 0;
                    } else if (modelo.sAccion== "Actualizar")
                    {
                        modelo.Entidad.EtapaEstado = "A";
                    }
                    ResultadoDTO<EtapaERADTO> resultado = new EtapaERAAppServicio().SaveUpdate(modelo.Entidad);
                    if (resultado.EsCorrecto >= 0)
                    {
                        if (modelo.Entidad.EtapaCodi > 0)
                        {
                            TempData["sMensajeExito"] = "Se ha editado correctamente.";
                        }
                        else
                        {
                            TempData["sMensajeExito"] = "Se ha registrado correctamente.";
                        }
                        mensajeError = "";
                    }
                    else
                    {
                        mensajeError = resultado.Mensaje;
                    }
                }
            }






            modelo.sError = mensajeError;
            return Json(modelo, JsonRequestBehavior.AllowGet);
        }
    }
}

