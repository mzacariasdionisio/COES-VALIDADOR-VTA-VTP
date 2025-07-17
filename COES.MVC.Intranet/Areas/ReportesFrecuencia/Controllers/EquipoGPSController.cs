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
using COES.Framework.Base.Tools;


namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Controllers
{
    public class EquipoGPSController : BaseController
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
            EquipoGPSModel model = new EquipoGPSModel();
            //model.bNuevo = (new Funcion()).ValidarPermisoNuevo(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

            model.bNuevo = true;
            model.bEditar = true;
            model.bGrabar = true;
            return View(model);
        }

        public ActionResult Lista()
        {
            EquipoGPSModel model = new EquipoGPSModel();
            model.ListaEquipos = new EquipoGPSAppServicio().GetListaEquipoGPS();
            return PartialView(model);
        }

        public ActionResult New()
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            int UltCodGenerado = new EquipoGPSAppServicio().GetUltimoCodigoGenerado();
            
            EquipoGPSModel modelo = new EquipoGPSModel();
            modelo.Entidad = new EquipoGPSDTO();
            modelo.Entidad.GPSCodiOriginal = 0;
            modelo.Entidad.GPSCodi = UltCodGenerado;
            modelo.Entidad.GPSEstado = "A";
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

            var empresas = servicio.ListarEmpresaTodo().Where(x => x.Emprsein == "S" && x.Emprestado=="A").ToList();
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

        public ActionResult Edit(int id)
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            EquipoGPSModel modelo = new EquipoGPSModel();
            modelo.Entidad = new EquipoGPSAppServicio().GetBydId(id);
            modelo.Entidad.GPSCodiOriginal = modelo.Entidad.GPSCodi;
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

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

            EquipoGPSModel modelo = new EquipoGPSModel();
            modelo.Entidad = new EquipoGPSAppServicio().GetBydId(id);
            modelo.Entidad.GPSCodiOriginal = modelo.Entidad.GPSCodi;
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

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

            modelo.ListaEquipos = new EquipoGPSAppServicio().GetListaEquipoGPS();
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

        public ActionResult Save(EquipoGPSModel modelo)
        {
            string mensajeError = "Se ha producido un error al insertar la información";
            if (modelo.sAccion=="Eliminar")
            {
                int numRegistros = new EquipoGPSAppServicio().GetNumeroRegistrosPorEquipo(modelo.Entidad.GPSCodi);

                if (numRegistros>0)
                {
                    mensajeError = "El Equipo cuenta con registros en la tabla de lectura.";
                } else
                {
                    ResultadoDTO<EquipoGPSDTO> resultado = new EquipoGPSAppServicio().Eliminar(modelo.Entidad);
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
            } else
            {
                int numResultado = new EquipoGPSAppServicio().ValidarNombreEquipoGPS(modelo.Entidad);

                if (numResultado > 0)
                {
                    mensajeError = "El nombre de Equipo ya existe";
                }
                else
                {
                    if (modelo.sAccion=="Insertar")
                    {
                        modelo.Entidad.GPSEstado = "A";
                    }
                    ResultadoDTO<EquipoGPSDTO> resultado = new EquipoGPSAppServicio().SaveUpdate(modelo.Entidad);
                    if (resultado.EsCorrecto >= 0)
                    {
                        if (modelo.Entidad.GPSCodiOriginal > 0)
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

        /// <summary>
        /// Descargar manual de usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesReportesFrecuencia.ModuloManualUsuario;
            string nombreArchivo = ConstantesReportesFrecuencia.ArchivoManualUsuarioIntranet;
            string pathDestino = modulo + ConstantesReportesFrecuencia.FolderRaizAutomatizacionModuloManual;
            string pathAlternativo = ConfigurationManager.AppSettings["FileSystemPortal"];

            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);

                    return File(buffer, Constantes.AppPdf, nombreArchivo);
                }
                else
                {
                    throw new ArgumentException("No se pudo descargar el archivo del servidor.");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("ERROR: ", ex);
            }
        }
    }
}

