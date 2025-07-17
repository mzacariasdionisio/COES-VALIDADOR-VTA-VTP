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
using COES.Servicios.Aplicacion.IEOD;

namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Controllers
{
    public class CopiarInformacionController : BaseController
    {
        /// <summary>
        /// Instancia del Web Service de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        PR5ReportesAppServicio servicio = new PR5ReportesAppServicio();

        public ActionResult Index()
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            CopiarInformacionModel model = new CopiarInformacionModel();
            //model.bNuevo = (new Funcion()).ValidarPermisoNuevo(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

            model.FechaFinal = System.DateTime.Now;
            model.FechaInicial = System.DateTime.Now.AddMonths(-6);

            model.bNuevo = true;
            model.bEditar = true;
            model.bGrabar = true;
            List<CopiarInformacionDTO> lista = new List<CopiarInformacionDTO>();
            //lista = new CopiarInformacionAppServicio().GetListaCopiarInformacion(model.FechaInicial.ToString(), model.FechaFinal.ToString());
            model.ListaCopiarInformacion = lista;

            model.ListaEquiposOrigen = new EquipoGPSAppServicio().GetListaEquipoGPS().Where(x => x.GPSEstado == "A").OrderBy(m => m.NombreEquipo).ToList();
            TempData["ListaEquipos"] = new SelectList(model.ListaEquiposOrigen, "GPSCODI", "NOMBREEQUIPO");


            return View(model);
        }

        //POST
        [HttpPost]
        public ActionResult Lista(CopiarInformacionModel model)
        {
            string mensajeError = "";
            if (DateTime.ParseExact(model.FechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture) > DateTime.ParseExact(model.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture))
            {
                mensajeError = "La fecha final debe ser mayor que la fecha inicial.";
                TempData["sMensajeExito"] = mensajeError;
            }
            else
            {
                model.ListaCopiarInformacion = new CopiarInformacionAppServicio().GetListaCopiarInformacion(model.FechaIni, model.FechaFin, model.CodEquipoOrigen, model.CodEquipoDestino);

            }
            
            return PartialView(model);
        }

        public ActionResult New()
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            CopiarInformacionModel modelo = new CopiarInformacionModel();
            modelo.Entidad = new CopiarInformacionDTO();

            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

            modelo.ListaEquiposOrigen = new EquipoGPSAppServicio().GetListaEquipoGPS().Where(x => x.GPSEstado == "A").OrderBy(m => m.NombreEquipo).ToList();
            TempData["ListaEquiposOrigen"] = new SelectList(modelo.ListaEquiposOrigen, "GPSCODI", "NOMBREEQUIPO", modelo.Entidad.GPSCodiOrigen);

            modelo.ListaEquiposDestino = new EquipoGPSAppServicio().GetListaEquipoGPS().Where(x => x.GPSEstado == "A").OrderBy(m => m.NombreEquipo).ToList();
            TempData["ListaEquiposDestino"] = new SelectList(modelo.ListaEquiposDestino, "GPSCODI", "NOMBREEQUIPO", modelo.Entidad.GPSCodiDest);

            return View(modelo);
        }

        

        public ActionResult Edit(int id)
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            EquipoGPSModel modelo = new EquipoGPSModel();
            modelo.Entidad = new EquipoGPSAppServicio().GetBydId(id);
            modelo.Entidad.GPSCodiOriginal = modelo.Entidad.GPSCodi;
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

            modelo.ListaEquipos = new EquipoGPSAppServicio().GetListaEquipoGPS().Where(x => x.GPSOficial == "S").ToList();
            TempData["ListaEquipos"] = new SelectList(modelo.ListaEquipos, "GPSCODI", "NOMBREEQUIPO", modelo.Entidad.EquipoCodi);

            var empresas = servicio.ListarEmpresaTodo().Where(x => x.Emprsein == "S" && x.Emprestado == "A").ToList();
            modelo.ListaEmpresas = empresas;
            TempData["ListaEmpresas"] = new SelectList(modelo.ListaEmpresas, "EMPRCODI", "EMPRNOMB", modelo.Entidad.EmpresaCodi);

            List<SelectListItem> listaOficial = new List<SelectListItem>();
            listaOficial.Add(new SelectListItem { Text = "NO", Value = "N" });
            listaOficial.Add(new SelectListItem { Text = "SI", Value = "S" });

            TempData["ListaOficial"] = listaOficial;

            List<SelectListItem> listaTipo = new List<SelectListItem>();
            listaTipo.Add(new SelectListItem { Text = "FISICO", Value = "FISICO" });
            listaTipo.Add(new SelectListItem { Text = "VIRTUAL", Value = "VIRTUAL" });

            TempData["ListaTipo"] = listaTipo;

            modelo.ListaEquipos = new EquipoGPSAppServicio().GetListaEquipoGPS().Where(x => x.GPSEstado == "A").ToList();
            TempData["ListaEquipos"] = new SelectList(modelo.ListaEquipos, "GPSCODI", "NOMBREEQUIPO", modelo.Entidad.EquipoCodi);

            List<SelectListItem> listaGenAlarma = new List<SelectListItem>();
            listaGenAlarma.Add(new SelectListItem { Text = "NO", Value = "N" });
            listaGenAlarma.Add(new SelectListItem { Text = "SI", Value = "S" });

            TempData["ListaGenAlarma"] = listaGenAlarma;

            List<SelectListItem> listaEstado = new List<SelectListItem>();
            listaEstado.Add(new SelectListItem { Text = "ACTIVO", Value = "A" });
            listaEstado.Add(new SelectListItem { Text = "BAJA", Value = "B" });
            TempData["ListaEstado"] = listaEstado;

            return View(modelo);
        }

        public ActionResult Delete(int id)
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            CopiarInformacionModel modelo = new CopiarInformacionModel();
            modelo.Entidad = new CopiarInformacionAppServicio().GetBydId(id);
            //modelo.Entidad.GPSCodiOriginal = modelo.Entidad.GPSCodi;
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

            modelo.ListaEquiposOrigen = new EquipoGPSAppServicio().GetListaEquipoGPS().Where(x => x.GPSEstado == "A").OrderBy(m => m.NombreEquipo).ToList();
            TempData["ListaEquiposOrigen"] = new SelectList(modelo.ListaEquiposOrigen, "GPSCODI", "NOMBREEQUIPO", modelo.Entidad.GPSCodiOrigen);

            modelo.ListaEquiposDestino = new EquipoGPSAppServicio().GetListaEquipoGPS().Where(x => x.GPSEstado == "A").OrderBy(m => m.NombreEquipo).ToList();
            TempData["ListaEquiposDestino"] = new SelectList(modelo.ListaEquiposDestino, "GPSCODI", "NOMBREEQUIPO", modelo.Entidad.GPSCodiDest);

            return View(modelo);
        }

        public ActionResult Save(CopiarInformacionModel modelo)
        {
            string strFileName = string.Empty;
            string mensajeError = "Se ha producido un error al insertar la información";
            if (modelo.sAccion == "Eliminar")
            {
                ResultadoDTO<CopiarInformacionDTO> resultado = new CopiarInformacionAppServicio().Eliminar(modelo.Entidad);
                if (resultado.EsCorrecto >= 0)
                {
                    TempData["sMensajeExito"] = "Se ha eliminado correctamente.";
                    mensajeError = "";
                }
                else
                {
                    mensajeError = resultado.Mensaje;
                }
            } else if (modelo.sAccion == "Insertar")
            {
                if (Convert.ToDateTime(modelo.Entidad.FechaHoraInicio) > Convert.ToDateTime(modelo.Entidad.FechaHoraFin))
                {
                    mensajeError = "La fecha hora final debe ser mayor que la fecha hora inicial.";
                    TempData["sMensajeExito"] = mensajeError;
                }
                else if (modelo.Entidad.GPSCodiOrigen == modelo.Entidad.GPSCodiDest)
                {
                    mensajeError = "No puede copiar información del mismo equipo GPS.";
                    TempData["sMensajeExito"] = mensajeError;
                }
                else
                {
                    if (modelo.sAccion == "Insertar")
                    {
                        modelo.Entidad.Estado = "A";
                    }
                    ResultadoDTO<CopiarInformacionDTO> resultado = new CopiarInformacionAppServicio().SaveUpdate(modelo.Entidad);
                    if (resultado.EsCorrecto >= 0)
                    {
                        if (modelo.Entidad.IdCopia > 0)
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
            //return RedirectToAction("Index", "CargaVirtual");
        }
    }
}