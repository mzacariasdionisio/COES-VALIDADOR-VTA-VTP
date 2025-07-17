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
using COES.MVC.Intranet.ServicioCloud;
using COES.Servicios.Aplicacion.ServicioRPF;
using log4net;
using System.Reflection;

namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Controllers
{
    public class CargaVirtualController : BaseController
    {
        /// <summary>
        /// Instancia del Web Service de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        PR5ReportesAppServicio servicio = new PR5ReportesAppServicio();

        ServicioCloudClient servicioCloud = new ServicioCloudClient();
        RpfAppServicio logic = new RpfAppServicio();
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            CargaVirtualModel model = new CargaVirtualModel();

            model.bNuevo = true;
            model.bEditar = true;
            model.bGrabar = true;

            model.FechaFinal = System.DateTime.Now;
            model.FechaInicial = System.DateTime.Now.AddMonths(-1);

            model.ListaEquipos = new EquipoGPSAppServicio().GetListaEquipoGPS().Where(x => x.GPSEstado == "A" && x.GPSTipo == "VIRTUAL").OrderBy(m => m.NombreEquipo).ToList();
            TempData["ListaEquipos"] = new SelectList(model.ListaEquipos, "GPSCODI", "NOMBREEQUIPO");

            List<CargaVirtualDTO> lista = new List<CargaVirtualDTO>();
            model.ListaCargaVirtual = lista;

            return View(model);
        }

        //POST
        [HttpPost]
        public ActionResult Lista(CargaVirtualModel model)
        {
            string mensajeError = "";
            if (DateTime.ParseExact(model.FechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture) > DateTime.ParseExact(model.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture))
            {
                mensajeError = "La fecha final debe ser mayor que la fecha inicial.";
                TempData["sMensajeExito"] = mensajeError;
            }
            else
            {
                model.ListaCargaVirtual = new CargaVirtualAppServicio().GetListaCargaVirtual(model.FechaIni, model.FechaFin, model.CodEquipo);

            }
            return PartialView(model);
        }

        public ActionResult New()
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            CargaVirtualModel modelo = new CargaVirtualModel();
            modelo.Entidad = new CargaVirtualDTO();
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

            modelo.ListaEquipos = new EquipoGPSAppServicio().GetListaEquipoGPS().Where(x => x.GPSTipo == "VIRTUAL" && x.GPSEstado == "A").OrderBy(m => m.NombreEquipo).ToList();
            TempData["ListaEquipos"] = new SelectList(modelo.ListaEquipos, "GPSCODI", "NOMBREEQUIPO", modelo.Entidad.GPSCodi);

            var empresas = new CargaVirtualAppServicio().GetListaEmpresasCargaVirtual().OrderBy(x => x.Emprnomb).ToList();
            modelo.ListaEmpresas = empresas;
            TempData["ListaEmpresas"] = new SelectList(modelo.ListaEmpresas, "EMPRCODI", "EMPRNOMB", modelo.Entidad.CodEmpresa);

            List<SelectListItem> listaCentral = new List<SelectListItem>();
            TempData["listaCentral"] = listaCentral;

            List<SelectListItem> listaUnidad = new List<SelectListItem>();
            TempData["ListaUnidad"] = listaUnidad;

            return View(modelo);
        }

        public ActionResult Externo()
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            CargaVirtualModel modelo = new CargaVirtualModel();
            modelo.Entidad = new CargaVirtualDTO();

            modelo.ListaEquipos = new EquipoGPSAppServicio().GetListaEquipoGPS().Where(x => x.GPSTipo == "VIRTUAL" && x.GPSEstado == "A").OrderBy(m => m.NombreEquipo).ToList();
            TempData["ListaEquipos"] = new SelectList(modelo.ListaEquipos, "GPSCODI", "NOMBREEQUIPO", modelo.Entidad.GPSCodi);

            return View(modelo);
        }

        [HttpPost]
        public ActionResult ListarCentralPorEmpresa(int CodEmpresa)
        {
            return Json(new CargaVirtualAppServicio().GetListaCentralPorEmpresa(CodEmpresa));
        }

        [HttpPost]
        public ActionResult ListarUnidadPorCentralEmpresa(int CodEmpresa, string Central)
        {
            return Json(new CargaVirtualAppServicio().GetListaUnidadPorCentralEmpresa(CodEmpresa, Central));
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

        public ActionResult VerRegistroCarga(int IdCarga)
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            CargaVirtualModel modelo = new CargaVirtualModel();
            modelo.Entidad = new CargaVirtualAppServicio().GetBydId(IdCarga);
            modelo.ListaLecturaVirtual = new CargaVirtualAppServicio().GetListaLecturaVirtual(IdCarga);

            return View(modelo);
        }

        

        [HttpPost]
        public ActionResult ListaCarga(CargaVirtualModel model)
        {
            //model.ListaCargaVirtual = new CargaVirtualAppServicio().GetListaLecturaVirtual(model.FechaIni, model.FechaFin, model.CodEquipo);
            return PartialView(model);
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

        public ActionResult Save(CargaVirtualModel modelo)
        {

            string strFileName = string.Empty;
            string mensajeError = "Se ha producido un error al insertar la información";
            

            if (modelo.sAccion == "Grabar")
            {
                if (DateTime.ParseExact(modelo.Entidad.FechaCargaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture) > DateTime.ParseExact(modelo.Entidad.FechaCargaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture))
                {
                    mensajeError = "La fecha final debe ser mayor que la fecha inicial.";
                    TempData["sMensajeExito"] = mensajeError;
                }
                else 
                {
                    modelo.Entidad.TipoCarga = "REPORTE PR-21";
                    DateTime start = DateTime.ParseExact(modelo.Entidad.FechaCargaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime end = DateTime.ParseExact(modelo.Entidad.FechaCargaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    ResultadoDTO<CargaVirtualDTO> resultado = new CargaVirtualAppServicio().SaveUpdate(modelo.Entidad);
                    if (resultado.EsCorrecto >= 0)
                    {
                        TempData["sMensajeExito"] = "Se ha registrado correctamente.";
                        mensajeError = "";
                    }
                    else
                    {
                        mensajeError = resultado.Mensaje;
                    }

                    TimeSpan difference = end - start;
                    int intNumRegistros = 0;
                    for (int i = 0; i <= difference.Days; i++)
                    {
                        DateTime fechaInicio = DateTime.ParseExact(modelo.Entidad.FechaCargaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        DateTime fechaActual = fechaInicio.AddDays(i);
                        int intAnio = 0;
                        int intMes = 0;
                        int intDia = 0;
                        intAnio = fechaActual.Year;
                        intMes = fechaActual.Month;
                        intDia = fechaActual.Day;
                        string strAnio = intAnio.ToString();
                        string strMes = intMes.ToString();
                        string strDia = intDia.ToString();

                        if (intMes < 10)
                        {
                            strMes = "0" + strMes;
                        }
                        if (intDia < 10)
                        {
                            strDia = "0" + strDia;
                        }

                        string strFechaActual = strDia + "/" + strMes + "/" + strAnio;


                        //modelo.Entidad.FechaCarga = strFechaActual;
                        //var file = Request.Files[i];
                        //strFileName = file.FileName;
                        modelo.Entidad.ArchivoCarga = strFileName;

                        JsonResult resultadoJson = new JsonResult();
                        string strCodUnidad = modelo.Entidad.CodUnidad.ToString() + ",";
                        resultadoJson = GenerarArchivo(strCodUnidad, strFechaActual, resultado.Data.IdCarga, modelo.Entidad.GPSCodi);
                        intNumRegistros = intNumRegistros + Convert.ToInt32(resultadoJson.Data);

                        if (Convert.ToInt32(resultadoJson.Data) >= 0)
                        {
                            TempData["sMensajeExito"] = "Se ha registrado correctamente "+ intNumRegistros + " registros.";
                            mensajeError = "";
                        }

                        
                    }
                }

                

                


            }


            modelo.sError = mensajeError;
            return Json(modelo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GenerarArchivo(string puntos, string fecha, int IdCarga, int GPSCodi)
        {
            int result = 1;
            int numRegistros = 0;
            try
            {
                string[] codigos = puntos.Split(Constantes.CaracterComa);
                List<int> list = new List<int>();
                foreach (string codigo in codigos)
                    if (!string.IsNullOrEmpty(codigo))
                        list.Add(int.Parse(codigo));

                DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                List<Medicion> entitys = new List<Medicion>();

                int lenPagina = 10;
                int nroPaginas = (list.Count % lenPagina == 0) ? list.Count / lenPagina : list.Count / lenPagina + 1;

                for (int i = 1; i <= nroPaginas; i++)
                {
                    int indexInicio = (i - 1) * lenPagina;
                    int indexFin = (i < nroPaginas) ? i * lenPagina - 1 : list.Count - 1;

                    List<int> listPuntos = new List<int>();

                    for (int k = indexInicio; k <= indexFin; k++)
                    {
                        listPuntos.Add(list[k]);
                    }

                    entitys.AddRange(servicioCloud.DescargarEnvio(listPuntos.ToArray(), fechaConsulta).ToList());
                }

                //string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF] + Constantes.NombreCSVServicioRPF;

                //this.GenerarArchivo(entitys, fullPath, fecha);

                List<LecturaDTO> listaLectura = new List<LecturaDTO>();

                string queryLectura = string.Empty;
                string strResultado = string.Empty;
                int contadorMinutos = 0;

                if (entitys.Count>0)
                {
                    foreach (Medicion variable in entitys)
                    {
                        if (variable.TIPOINFOCODI==6)
                        {
                            contadorMinutos++;
                            LecturaVirtualDTO lecturaVirtual = new LecturaVirtualDTO();
                            lecturaVirtual.IdCarga = IdCarga;
                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:ss");
                            lecturaVirtual.Frecuencia = variable.H0;
                            //var resultadoH0 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:01");
                            lecturaVirtual.Frecuencia = variable.H1;
                            //var resultadoH1 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:02");
                            lecturaVirtual.Frecuencia = variable.H2;
                            //var resultadoH2 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:03");
                            lecturaVirtual.Frecuencia = variable.H3;
                            //var resultadoH3 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:04");
                            lecturaVirtual.Frecuencia = variable.H4;
                            //var resultadoH4 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:05");
                            lecturaVirtual.Frecuencia = variable.H5;
                            //var resultadoH5 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:06");
                            lecturaVirtual.Frecuencia = variable.H6;
                            //var resultadoH6 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:07");
                            lecturaVirtual.Frecuencia = variable.H7;
                            //var resultadoH7 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:08");
                            lecturaVirtual.Frecuencia = variable.H8;
                            //var resultadoH8 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:09");
                            lecturaVirtual.Frecuencia = variable.H9;
                            //var resultadoH9 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:10");
                            lecturaVirtual.Frecuencia = variable.H10;
                            //var resultadoH10 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:11");
                            lecturaVirtual.Frecuencia = variable.H11;
                            //var resultadoH11 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:12");
                            lecturaVirtual.Frecuencia = variable.H12;
                            //var resultadoH12 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:13");
                            lecturaVirtual.Frecuencia = variable.H13;
                            //var resultadoH13 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:14");
                            lecturaVirtual.Frecuencia = variable.H14;
                            //var resultadoH14 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:15");
                            lecturaVirtual.Frecuencia = variable.H15;
                            //var resultadoH15 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:16");
                            lecturaVirtual.Frecuencia = variable.H16;
                            //var resultadoH16 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:17");
                            lecturaVirtual.Frecuencia = variable.H17;
                            //var resultadoH17 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:18");
                            lecturaVirtual.Frecuencia = variable.H18;
                            //var resultadoH18 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:19");
                            lecturaVirtual.Frecuencia = variable.H19;
                            //var resultadoH19 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:20");
                            lecturaVirtual.Frecuencia = variable.H20;
                            //var resultadoH20 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:21");
                            lecturaVirtual.Frecuencia = variable.H21;
                            //var resultadoH21 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:22");
                            lecturaVirtual.Frecuencia = variable.H22;
                            //var resultadoH22 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:23");
                            lecturaVirtual.Frecuencia = variable.H23;
                            //var resultadoH23 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:24");
                            lecturaVirtual.Frecuencia = variable.H24;
                            //var resultadoH24 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:25");
                            lecturaVirtual.Frecuencia = variable.H25;
                            //var resultadoH25 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:26");
                            lecturaVirtual.Frecuencia = variable.H26;
                            //var resultadoH26 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:27");
                            lecturaVirtual.Frecuencia = variable.H27;
                            //var resultadoH27 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:28");
                            lecturaVirtual.Frecuencia = variable.H28;
                            //var resultadoH28 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:29");
                            lecturaVirtual.Frecuencia = variable.H29;
                            //var resultadoH29 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:30");
                            lecturaVirtual.Frecuencia = variable.H30;
                            //var resultadoH30 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:31");
                            lecturaVirtual.Frecuencia = variable.H31;
                            //var resultadoH31 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:32");
                            lecturaVirtual.Frecuencia = variable.H32;
                            //var resultadoH32 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:33");
                            lecturaVirtual.Frecuencia = variable.H33;
                            //var resultadoH33 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:34");
                            lecturaVirtual.Frecuencia = variable.H34;
                            //var resultadoH34 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:35");
                            lecturaVirtual.Frecuencia = variable.H35;
                            //var resultadoH35 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:36");
                            lecturaVirtual.Frecuencia = variable.H36;
                            //var resultadoH36 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:37");
                            lecturaVirtual.Frecuencia = variable.H37;
                            //var resultadoH37 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:38");
                            lecturaVirtual.Frecuencia = variable.H38;
                            //var resultadoH38 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:39");
                            lecturaVirtual.Frecuencia = variable.H39;
                            //var resultadoH39 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:40");
                            lecturaVirtual.Frecuencia = variable.H40;
                            //var resultadoH40 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:41");
                            lecturaVirtual.Frecuencia = variable.H41;
                            //var resultadoH41 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:42");
                            lecturaVirtual.Frecuencia = variable.H42;
                            //var resultadoH42 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:43");
                            lecturaVirtual.Frecuencia = variable.H43;
                            //var resultadoH43 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:44");
                            lecturaVirtual.Frecuencia = variable.H44;
                            //var resultadoH44 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:45");
                            lecturaVirtual.Frecuencia = variable.H45;
                            //var resultadoH45 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:46");
                            lecturaVirtual.Frecuencia = variable.H46;
                            //var resultadoH46 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:47");
                            lecturaVirtual.Frecuencia = variable.H47;
                            //var resultadoH47 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:48");
                            lecturaVirtual.Frecuencia = variable.H48;
                            //var resultadoH48 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:49");
                            lecturaVirtual.Frecuencia = variable.H49;
                            //var resultadoH49 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:50");
                            lecturaVirtual.Frecuencia = variable.H50;
                            //var resultadoH50 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:51");
                            lecturaVirtual.Frecuencia = variable.H51;
                            //var resultadoH51 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:52");
                            lecturaVirtual.Frecuencia = variable.H52;
                            //var resultadoH52 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:53");
                            lecturaVirtual.Frecuencia = variable.H53;
                            //var resultadoH53 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:54");
                            lecturaVirtual.Frecuencia = variable.H54;
                            //var resultadoH54 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:55");
                            lecturaVirtual.Frecuencia = variable.H55;
                            //var resultadoH55 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:56");
                            lecturaVirtual.Frecuencia = variable.H56;
                            //var resultadoH56 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:57");
                            lecturaVirtual.Frecuencia = variable.H57;
                            //var resultadoH57 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:58");
                            lecturaVirtual.Frecuencia = variable.H58;
                            //var resultadoH58 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";

                            lecturaVirtual.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:59");
                            lecturaVirtual.Frecuencia = variable.H59;
                            //var resultadoH59 = new CargaVirtualAppServicio().SaveLecturaVirtual(lecturaVirtual);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaVirtualString(lecturaVirtual);
                            queryLectura = queryLectura + strResultado + ";";




                            LecturaDTO lectura = new LecturaDTO();
                            lectura.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:ss");
                            lectura.GPSCodi = GPSCodi;
                            lectura.H0 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H0));
                            lectura.H1 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H1));
                            lectura.H2 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H2));
                            lectura.H3 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H3));
                            lectura.H4 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H4));
                            lectura.H5 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H5));
                            lectura.H6 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H6));
                            lectura.H7 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H7));
                            lectura.H8 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H8));
                            lectura.H9 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H9));
                            lectura.H10 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H10));
                            lectura.H11 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H11));
                            lectura.H12 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H12));
                            lectura.H13 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H13));
                            lectura.H14 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H14));
                            lectura.H15 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H15));
                            lectura.H16 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H16));
                            lectura.H17 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H17));
                            lectura.H18 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H18));
                            lectura.H19 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H19));
                            lectura.H20 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H20));
                            lectura.H21 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H21));
                            lectura.H22 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H22));
                            lectura.H23 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H23));
                            lectura.H24 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H24));
                            lectura.H25 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H25));
                            lectura.H26 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H26));
                            lectura.H27 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H27));
                            lectura.H28 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H28));
                            lectura.H29 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H29));
                            lectura.H30 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H30));
                            lectura.H31 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H31));
                            lectura.H32 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H32));
                            lectura.H33 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H33));
                            lectura.H34 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H34));
                            lectura.H35 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H35));
                            lectura.H36 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H36));
                            lectura.H37 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H37));
                            lectura.H38 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H38));
                            lectura.H39 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H39));
                            lectura.H40 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H40));
                            lectura.H41 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H41));
                            lectura.H42 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H42));
                            lectura.H43 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H43));
                            lectura.H44 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H44));
                            lectura.H45 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H45));
                            lectura.H46 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H46));
                            lectura.H47 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H47));
                            lectura.H48 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H48));
                            lectura.H49 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H49));
                            lectura.H50 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H50));
                            lectura.H51 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H51));
                            lectura.H52 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H52));
                            lectura.H53 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H53));
                            lectura.H54 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H54));
                            lectura.H55 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H55));
                            lectura.H56 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H56));
                            lectura.H57 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H57));
                            lectura.H58 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H58));
                            lectura.H59 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H59));

                            numRegistros = numRegistros + 60;

                            List<decimal?> listaFrec = new List<decimal?>();
                            listaFrec.Add(lectura.H0);
                            listaFrec.Add(lectura.H1);
                            listaFrec.Add(lectura.H2);
                            listaFrec.Add(lectura.H3);
                            listaFrec.Add(lectura.H4);
                            listaFrec.Add(lectura.H5);
                            listaFrec.Add(lectura.H6);
                            listaFrec.Add(lectura.H7);
                            listaFrec.Add(lectura.H8);
                            listaFrec.Add(lectura.H9);
                            listaFrec.Add(lectura.H10);
                            listaFrec.Add(lectura.H11);
                            listaFrec.Add(lectura.H12);
                            listaFrec.Add(lectura.H13);
                            listaFrec.Add(lectura.H14);
                            listaFrec.Add(lectura.H15);
                            listaFrec.Add(lectura.H16);
                            listaFrec.Add(lectura.H17);
                            listaFrec.Add(lectura.H18);
                            listaFrec.Add(lectura.H19);
                            listaFrec.Add(lectura.H20);
                            listaFrec.Add(lectura.H21);
                            listaFrec.Add(lectura.H22);
                            listaFrec.Add(lectura.H23);
                            listaFrec.Add(lectura.H24);
                            listaFrec.Add(lectura.H25);
                            listaFrec.Add(lectura.H26);
                            listaFrec.Add(lectura.H27);
                            listaFrec.Add(lectura.H28);
                            listaFrec.Add(lectura.H29);
                            listaFrec.Add(lectura.H30);
                            listaFrec.Add(lectura.H31);
                            listaFrec.Add(lectura.H32);
                            listaFrec.Add(lectura.H33);
                            listaFrec.Add(lectura.H34);
                            listaFrec.Add(lectura.H35);
                            listaFrec.Add(lectura.H36);
                            listaFrec.Add(lectura.H37);
                            listaFrec.Add(lectura.H38);
                            listaFrec.Add(lectura.H39);
                            listaFrec.Add(lectura.H40);
                            listaFrec.Add(lectura.H41);
                            listaFrec.Add(lectura.H42);
                            listaFrec.Add(lectura.H43);
                            listaFrec.Add(lectura.H44);
                            listaFrec.Add(lectura.H45);
                            listaFrec.Add(lectura.H46);
                            listaFrec.Add(lectura.H47);
                            listaFrec.Add(lectura.H48);
                            listaFrec.Add(lectura.H49);
                            listaFrec.Add(lectura.H50);
                            listaFrec.Add(lectura.H51);
                            listaFrec.Add(lectura.H52);
                            listaFrec.Add(lectura.H53);
                            listaFrec.Add(lectura.H54);
                            listaFrec.Add(lectura.H55);
                            listaFrec.Add(lectura.H56);
                            listaFrec.Add(lectura.H57);
                            listaFrec.Add(lectura.H58);
                            listaFrec.Add(lectura.H59);
                            lectura.Maximo = listaFrec.Max();
                            lectura.Minimo = listaFrec.Min();
                            lectura.Num = listaFrec.Count();

                            decimal? decMedia = listaFrec.Average();
                            double dblSumDifCuadrado = 0;
                            double dblSumFrecCuadrado = 0;
                            double dblSumValores = 0;
                            foreach (decimal decFrecuencia in listaFrec)
                            {
                                decimal? decDif = decFrecuencia - decMedia;
                                double decDifCuadrado = Math.Pow(Convert.ToDouble(decDif), 2);
                                dblSumDifCuadrado = dblSumDifCuadrado + decDifCuadrado;

                                double dblFrecCuadrado = Math.Pow(Convert.ToDouble(decFrecuencia), 2);
                                dblSumFrecCuadrado = dblSumFrecCuadrado + dblFrecCuadrado;
                                dblSumValores = dblSumValores + Convert.ToDouble(decFrecuencia);
                            }
                            double dblDesvStandar = Math.Sqrt(dblSumDifCuadrado / lectura.Num);

                            dblSumValores = (dblSumValores - (lectura.Num * 60)) / 60;
                            double dblVSF = (Math.Sqrt(dblSumFrecCuadrado / lectura.Num) - 60);

                            lectura.Desv = Convert.ToDecimal(dblSumValores);
                            lectura.VSF = Convert.ToDecimal(dblVSF);

                            LecturaDTO lecturaTension = new LecturaDTO();
                            lecturaTension = listaLectura.Where(x => x.FecHora == lectura.FecHora && x.GPSCodi == lectura.GPSCodi).FirstOrDefault();
                            if (lecturaTension != null)
                            {
                                //lectura.Voltaje = lecturaTension.H0;
                            }
                            //var resultadoLectura = new CargaVirtualAppServicio().SaveLectura(lectura);
                            strResultado = new CargaVirtualAppServicio().SaveLecturaString(lectura);
                            queryLectura = queryLectura + strResultado + ";";
                        } else if (variable.TIPOINFOCODI == 1)
                        {
                            LecturaDTO lectura = new LecturaDTO();
                            lectura.FecHora = variable.FECHAHORA.ToString("dd-MM-yyyy HH:mm:ss");
                            lectura.GPSCodi = GPSCodi;
                            lectura.H0 = Convert.ToDecimal(String.Format("{0:0.###}", variable.H0));
                            listaLectura.Add(lectura);
                        }

                        if(contadorMinutos > 60) {
                            //Grabar Query String
                            if (!string.IsNullOrEmpty(queryLectura))
                            {
                                int intResultado = new CargaVirtualAppServicio().SaveLecturaQuery(queryLectura);
                                queryLectura = string.Empty;
                            }
                            contadorMinutos = 0;
                        }


                    }
                }

                //Grabar Query String
                if (!string.IsNullOrEmpty(queryLectura))
                {
                    int intResultado = new CargaVirtualAppServicio().SaveLecturaQuery(queryLectura);
                }

                result = numRegistros;
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                result = -1;
            }

            return Json(result);
        }

        /// <summary>
        /// Generar el archivo CSV con los datos cargados
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="fileName"></param>
        /// <param name="fecha"></param>
        /*protected void GenerarArchivo(List<Medicion> entitys, string fileName, string fecha)
        {
            try
            {
                DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                List<int> puntos = (from punto in entitys select punto.PTOMEDICODI).Distinct().ToList();
                List<ServicioRpfDTO> configuracion = this.logic.ObtenerUnidadesCarga(fechaConsulta);

                string[] texto = new string[86406];
                int[] tipos = { 1, 6 };

                texto[0] = " ";
                texto[1] = " ";
                texto[2] = "Empresa";
                texto[3] = "Central";
                texto[4] = "Unidad";
                texto[5] = "Fecha: ," + fecha;
                int t = 0;
                foreach (int punto in puntos)
                {
                    ServicioRpfDTO entidad = configuracion.Where(x => x.PTOMEDICODI == punto).FirstOrDefault();

                    for (int i = 0; i < tipos.Length; i++)
                    {
                        texto[0] = texto[0] + "," + punto;
                        texto[1] = texto[1] + "," + tipos[i];
                        texto[2] = texto[2] + "," + entidad.EMPRNOMB;
                        texto[3] = texto[3] + "," + entidad.EQUINOMB;
                        texto[4] = texto[4] + "," + entidad.EQUIABREV;

                        List<Medicion> list = entitys.Where(x => x.PTOMEDICODI == punto && x.TIPOINFOCODI == tipos[i]).OrderBy(x => x.FECHAHORA).ToList();

                        int k = 6;
                        foreach (Medicion item in list)
                        {
                            for (int j = 0; j <= 59; j++)
                            {
                                if (t == 0 && i == 0)
                                    texto[k] = item.FECHAHORA.AddSeconds(j).ToString("HH:mm:ss") + "," + item.GetType().GetProperty("H" + j.ToString()).GetValue(item, null);
                                else
                                    texto[k] = texto[k] + "," + item.GetType().GetProperty("H" + j.ToString()).GetValue(item, null);
                                k++;
                            }
                        }
                    }

                    t++;
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
                {
                    foreach (string item in texto)
                    {
                        file.WriteLine(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }*/


        public ActionResult SaveExterno(CargaVirtualModel modelo)
        {

            string strFileName = string.Empty;
            string mensajeError = "Se ha producido un error al insertar la información";


            if (modelo.sAccion == "Grabar")
            {
                modelo.Entidad.TipoCarga = "ARCHIVO EXTERNO";
                var file = Request.Files[0];
                strFileName = file.FileName;
                modelo.Entidad.ArchivoCarga = strFileName;

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesReportesFrecuencia.FolderReporte;
                    var path = Path.Combine(ruta, fileName);
                    file.SaveAs(path);
                    string text = System.IO.File.ReadAllText(path);
                    modelo.Entidad.DataCarga = text;

                    System.IO.File.Delete(path);

                }

                ResultadoDTO<CargaVirtualDTO> resultado = new CargaVirtualAppServicio().SaveUpdateExterno(modelo.Entidad);
                if (resultado.EsCorrecto >= 0)
                {

                    TempData["sMensajeExito"] = "Se ha registrado correctamente.";
                    mensajeError = "";
                }
                else
                {
                    mensajeError = resultado.Mensaje;
                }






            }



            modelo.sError = mensajeError;
            //return Json(modelo, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Index", "CargaVirtual");
        }


    }
}
