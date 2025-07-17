using COES.MVC.Extranet.Areas.ValorizacionDiaria.Models;
using COES.MVC.Extranet.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.ValorizacionDiaria;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.IEOD;
using log4net;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Extranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.ValorizacionDiaria.Helper;
using System.Globalization;
using COES.MVC.Extranet.Areas.ValorizacionDiaria.Helper;

namespace COES.MVC.Extranet.Areas.ValorizacionDiaria.Controllers
{
    public class ConsultasController : Controller
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ConsultasController));
        FormatoMedicionAppServicio logic = new FormatoMedicionAppServicio();

        public MeFormatoDTO Formato
        {
            get
            {
                return (Session[DatosSesionValorizacionDiaria.SesionFormato] != null) ?
                    (MeFormatoDTO)Session[DatosSesionValorizacionDiaria.SesionFormato] : new MeFormatoDTO();
            }
            set { Session[DatosSesionValorizacionDiaria.SesionFormato] = value; }
        }

        public string[][] MatrizExcel
        {
            get
            {
                return (Session["MatrizExcel"] != null) ?
                    (string[][])Session["MatrizExcel"] : new string[1][];
            }
            set { Session["MatrizExcel"] = value; }
        }


        //
        // GET: /Valorizacion/Consultas/

        ValorizacionDiariaAppServicio servicio = new ValorizacionDiariaAppServicio();
        GeneralAppServicio logicGeneral = new GeneralAppServicio();
        PR5ReportesAppServicio servicioEmpr = new PR5ReportesAppServicio();
        private ContactoAppServicio appContacto = new ContactoAppServicio();

        public ActionResult Index()
        {
            ConsultasModel model = new ConsultasModel();
            try
            {
                model.Empresa = this.servicio.ObtenerEmpresasMME(); // appContacto.ObtenerEmpresas(3);

                string anio = DateTime.Now.ToString("yyyy");
                string mes = "";
                switch ((DateTime.Now.ToString("MM")))
                {
                    case "01":
                        mes = "Ene";
                        break;
                    case "02":
                        mes = "Feb";
                        break;
                    case "03":
                        mes = "Mar";
                        break;
                    case "04":
                        mes = "Abr";
                        break;
                    case "05":
                        mes = "May";
                        break;
                    case "06":
                        mes = "Jun";
                        break;
                    case "07":
                        mes = "Jul";
                        break;
                    case "08":
                        mes = "Ago";
                        break;
                    case "09":
                        mes = "Set";
                        break;
                    case "10":
                        mes = "Oct";
                        break;
                    case "11":
                        mes = "Nov";
                        break;
                    case "12":
                        mes = "Dic";
                        break;
                }
                ViewBag.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
                ViewBag.FechaFormatoMesAnio = mes + " " + anio;
                return View(model);
            }
            catch (Exception)
            {
                return View(model);
            }
        }

        #region Monto Por Energia

        //Filtro po rango de fecha -Fit
        public PartialViewResult ListaMontoPorEnergia(int emprcodi, string fechaInicio, string fechaFinal, int nroPagina)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            ConsultasModel model = new ConsultasModel();
            try
            {
                model.MontoPorEnergia = servicio.GetListPageByDateRangeME(emprcodi, dateStart, dateEnd, nroPagina, Constantes.PageSize);
                return PartialView(model);
            }
            catch (Exception)
            {
                return PartialView(model);
            }
        }
        [HttpPost]
        public PartialViewResult PaginadoPorRangoFechaME(int emprcodi, string fechaInicio, string fechaFinal)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            ConsultasModel Cant = new ConsultasModel
            {
                MontoPorEnergia = servicio.GetListFullByDateRangeME(emprcodi, dateStart, dateEnd)
            };

            PaginadoModel model = new PaginadoModel();
            model.IndicadorPagina = false;

            int nroRegistros = Cant.MontoPorEnergia.Count();

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }
        [HttpPost]
        public JsonResult GenerarReporteXLSPorRangoFechaME(int emprcodi, string fechaInicio, string fechaFinal)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int indicador = 1;
            ConsultasModel model = new ConsultasModel();
            string ruta = AppDomain.CurrentDomain.BaseDirectory + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.FolderReporte;
            try
            {
                model.MontoPorEnergia = servicio.GetListFullByDateRangeME(emprcodi, dateStart, dateEnd);
                ExcelDocument.GenerarArchivoMontoEnergia(model, ruta);
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }
        [HttpGet]
        public virtual ActionResult ExportarReporteME()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteMontoPorEnergia;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        #endregion

        #region Monto Por Capacidad

        //Filtro po rango de fecha -Fit
        public PartialViewResult ListaMontoPorCapacidad(int emprcodi, string fechaInicio, string fechaFinal, int nroPagina)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, "d M yyyy", CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, "d M yyyy", CultureInfo.InvariantCulture);

            ConsultasModel model = new ConsultasModel();
            List<VtdMontoPorCapacidadDTO> list = new List<VtdMontoPorCapacidadDTO>();
            VtdMontoPorCapacidadDTO MC = new VtdMontoPorCapacidadDTO();

            try
            {
                if (dateStart.Year == dateEnd.Year)
                {
                    for (int u = dateStart.Month; u <= dateEnd.Month; u++)
                    {
                        int diasDelMes = DateTime.DaysInMonth(dateStart.Year, u);
                        DateTime fechaF = new DateTime(dateStart.Year, u, diasDelMes);
                        DateTime fechaI = new DateTime(dateStart.Year, u, 1);
                        MC = servicio.GetListFullByDateMC(emprcodi, fechaI, fechaF);
                        if (MC.Emprnomb != null)
                        {
                            list.Add(new VtdMontoPorCapacidadDTO() { Valofecha = MC.Valofecha, Emprnomb = MC.Emprnomb, Valomr = MC.Valomr, Valopreciopotencia = MC.Valopreciopotencia, Valdpfirremun = MC.Valdpfirremun, Valddemandacoincidente = MC.Valddemandacoincidente });
                        }
                    };
                }
                else
                {
                    for (int i = dateStart.Year; i <= dateEnd.Year; i++)
                    {
                        if (i == dateEnd.Year)
                        {
                            for (int x = 1; x <= dateEnd.Month; x++)
                            {
                                int diasDelMes = DateTime.DaysInMonth(i, x);
                                DateTime fechaF = new DateTime(i, x, diasDelMes);
                                DateTime fechaI = new DateTime(i, x, 1);
                                MC = servicio.GetListFullByDateMC(emprcodi, fechaI, fechaF);
                                if (MC.Emprnomb != null)
                                {
                                    list.Add(new VtdMontoPorCapacidadDTO() { Valofecha = MC.Valofecha, Emprnomb = MC.Emprnomb, Valomr = MC.Valomr, Valopreciopotencia = MC.Valopreciopotencia, Valdpfirremun = MC.Valdpfirremun, Valddemandacoincidente = MC.Valddemandacoincidente });
                                }
                            };
                        }
                        else
                        {
                            if (i == dateStart.Year)
                            {
                                for (int e = dateStart.Month; e <= 12; e++)
                                {
                                    int diasDelMes = DateTime.DaysInMonth(i, e);
                                    DateTime fechaF = new DateTime(dateStart.Year, e, diasDelMes);
                                    DateTime fechaI = new DateTime(dateStart.Year, e, 1);
                                    MC = servicio.GetListFullByDateMC(emprcodi, fechaI, fechaF);
                                    if (MC.Emprnomb != null)
                                    {
                                        list.Add(new VtdMontoPorCapacidadDTO() { Valofecha = MC.Valofecha, Emprnomb = MC.Emprnomb, Valomr = MC.Valomr, Valopreciopotencia = MC.Valopreciopotencia, Valdpfirremun = MC.Valdpfirremun, Valddemandacoincidente = MC.Valddemandacoincidente });
                                    }
                                };
                            }

                            if (i > dateStart.Year)
                            {
                                for (int x = 1; x <= 12; x++)
                                {
                                    int diasDelMes = DateTime.DaysInMonth(i, x);
                                    DateTime fechaF = new DateTime(i, x, diasDelMes);
                                    DateTime fechaI = new DateTime(i, x, 1);
                                    MC = servicio.GetListFullByDateMC(emprcodi, fechaI, fechaF);
                                    if (MC.Emprnomb != null)
                                    {
                                        list.Add(new VtdMontoPorCapacidadDTO() { Valofecha = MC.Valofecha, Emprnomb = MC.Emprnomb, Valomr = MC.Valomr, Valopreciopotencia = MC.Valopreciopotencia, Valdpfirremun = MC.Valdpfirremun, Valddemandacoincidente = MC.Valddemandacoincidente });
                                    }
                                };
                            }
                        }
                    }
                }
                model.MontoPorCapacidad = list;

                return PartialView(model);
            }
            catch (Exception ex)
            {
                return PartialView(model);
            }
        }
        [HttpPost]
        public PartialViewResult PaginadoPorRangoFechaMC(int emprcodi, string fechaInicio, string fechaFinal)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, "d M yyyy", CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, "d M yyyy", CultureInfo.InvariantCulture);

            PaginadoModel model = new PaginadoModel();
            ConsultasModel modelMC = new ConsultasModel();
            List<VtdMontoPorCapacidadDTO> list = new List<VtdMontoPorCapacidadDTO>();
            VtdMontoPorCapacidadDTO MC = new VtdMontoPorCapacidadDTO();

            try
            {
                if (dateStart.Year == dateEnd.Year)
                {
                    for (int u = dateStart.Month; u <= dateEnd.Month; u++)
                    {
                        int diasDelMes = DateTime.DaysInMonth(dateStart.Year, u);
                        DateTime fechaF = new DateTime(dateStart.Year, u, diasDelMes);
                        DateTime fechaI = new DateTime(dateStart.Year, u, 1);
                        MC = servicio.GetListFullByDateMC(emprcodi, fechaI, fechaF);
                        if (MC.Emprnomb != null)
                        {
                            list.Add(new VtdMontoPorCapacidadDTO() { Valofecha = MC.Valofecha, Emprnomb = MC.Emprnomb, Valomr = MC.Valomr, Valopreciopotencia = MC.Valopreciopotencia, Valdpfirremun = MC.Valdpfirremun, Valddemandacoincidente = MC.Valddemandacoincidente });
                        }
                    };
                }
                else
                {
                    for (int i = dateStart.Year; i <= dateEnd.Year; i++)
                    {
                        if (i == dateEnd.Year)
                        {
                            for (int x = 1; x <= dateEnd.Month; x++)
                            {
                                int diasDelMes = DateTime.DaysInMonth(i, x);
                                DateTime fechaF = new DateTime(i, x, diasDelMes);
                                DateTime fechaI = new DateTime(i, x, 1);
                                MC = servicio.GetListFullByDateMC(emprcodi, fechaI, fechaF);
                                if (MC.Emprnomb != null)
                                {
                                    list.Add(new VtdMontoPorCapacidadDTO() { Valofecha = MC.Valofecha, Emprnomb = MC.Emprnomb, Valomr = MC.Valomr, Valopreciopotencia = MC.Valopreciopotencia, Valdpfirremun = MC.Valdpfirremun, Valddemandacoincidente = MC.Valddemandacoincidente });
                                }
                            };
                        }
                        else
                        {
                            if (i == dateStart.Year)
                            {
                                for (int e = dateStart.Month; e <= 12; e++)
                                {
                                    int diasDelMes = DateTime.DaysInMonth(i, e);
                                    DateTime fechaF = new DateTime(dateStart.Year, e, diasDelMes);
                                    DateTime fechaI = new DateTime(dateStart.Year, e, 1);
                                    MC = servicio.GetListFullByDateMC(emprcodi, fechaI, fechaF);
                                    if (MC.Emprnomb != null)
                                    {
                                        list.Add(new VtdMontoPorCapacidadDTO() { Valofecha = MC.Valofecha, Emprnomb = MC.Emprnomb, Valomr = MC.Valomr, Valopreciopotencia = MC.Valopreciopotencia, Valdpfirremun = MC.Valdpfirremun, Valddemandacoincidente = MC.Valddemandacoincidente });
                                    }
                                };
                            }

                            if (i > dateStart.Year)
                            {
                                for (int x = 1; x <= 12; x++)
                                {
                                    int diasDelMes = DateTime.DaysInMonth(i, x);
                                    DateTime fechaF = new DateTime(i, x, diasDelMes);
                                    DateTime fechaI = new DateTime(i, x, 1);
                                    MC = servicio.GetListFullByDateMC(emprcodi, fechaI, fechaF);
                                    if (MC.Emprnomb != null)
                                    {
                                        list.Add(new VtdMontoPorCapacidadDTO() { Valofecha = MC.Valofecha, Emprnomb = MC.Emprnomb, Valomr = MC.Valomr, Valopreciopotencia = MC.Valopreciopotencia, Valdpfirremun = MC.Valdpfirremun, Valddemandacoincidente = MC.Valddemandacoincidente });
                                    }
                                };
                            }
                        }
                    }
                }
                modelMC.MontoPorCapacidad = list;

                model.IndicadorPagina = false;

                int nroRegistros = list.Count();

                if (nroRegistros > Constantes.NroPageShow)
                {
                    int pageSize = Constantes.PageSize;
                    int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                    model.NroPaginas = nroPaginas;
                    model.NroMostrar = Constantes.NroPageShow;
                    model.IndicadorPagina = true;
                }

                return PartialView(model);
            }

            catch (Exception ex)
            {
                return PartialView(model);
            }


        }
        [HttpPost]
        public JsonResult GenerarReporteXLSPorRangoFechaMC(int emprcodi, string fechaInicio, string fechaFinal)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, "d M yyyy", CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, "d M yyyy", CultureInfo.InvariantCulture);

            int indicador = 1;
            ConsultasModel model = new ConsultasModel();
            List<VtdMontoPorCapacidadDTO> list = new List<VtdMontoPorCapacidadDTO>();
            VtdMontoPorCapacidadDTO MC = new VtdMontoPorCapacidadDTO();

            try
            {
                if (dateStart.Year == dateEnd.Year)
                {
                    for (int u = dateStart.Month; u <= dateEnd.Month; u++)
                    {
                        int diasDelMes = DateTime.DaysInMonth(dateStart.Year, u);
                        DateTime fechaF = new DateTime(dateStart.Year, u, diasDelMes);
                        DateTime fechaI = new DateTime(dateStart.Year, u, 1);
                        MC = servicio.GetListFullByDateMC(emprcodi, fechaI, fechaF);
                        if (MC.Emprnomb != null)
                        {
                            list.Add(new VtdMontoPorCapacidadDTO() { Valofecha = MC.Valofecha, Emprnomb = MC.Emprnomb, Valomr = MC.Valomr, Valopreciopotencia = MC.Valopreciopotencia, Valdpfirremun = MC.Valdpfirremun, Valddemandacoincidente = MC.Valddemandacoincidente });
                        }
                    };
                }
                else
                {
                    for (int i = dateStart.Year; i <= dateEnd.Year; i++)
                    {
                        if (i == dateEnd.Year)
                        {
                            for (int x = 1; x <= dateEnd.Month; x++)
                            {
                                int diasDelMes = DateTime.DaysInMonth(i, x);
                                DateTime fechaF = new DateTime(i, x, diasDelMes);
                                DateTime fechaI = new DateTime(i, x, 1);
                                MC = servicio.GetListFullByDateMC(emprcodi, fechaI, fechaF);
                                if (MC.Emprnomb != null)
                                {
                                    list.Add(new VtdMontoPorCapacidadDTO() { Valofecha = MC.Valofecha, Emprnomb = MC.Emprnomb, Valomr = MC.Valomr, Valopreciopotencia = MC.Valopreciopotencia, Valdpfirremun = MC.Valdpfirremun, Valddemandacoincidente = MC.Valddemandacoincidente });
                                }
                            };
                        }
                        else
                        {
                            if (i == dateStart.Year)
                            {
                                for (int e = dateStart.Month; e <= 12; e++)
                                {
                                    int diasDelMes = DateTime.DaysInMonth(i, e);
                                    DateTime fechaF = new DateTime(dateStart.Year, e, diasDelMes);
                                    DateTime fechaI = new DateTime(dateStart.Year, e, 1);
                                    MC = servicio.GetListFullByDateMC(emprcodi, fechaI, fechaF);
                                    if (MC.Emprnomb != null)
                                    {
                                        list.Add(new VtdMontoPorCapacidadDTO() { Valofecha = MC.Valofecha, Emprnomb = MC.Emprnomb, Valomr = MC.Valomr, Valopreciopotencia = MC.Valopreciopotencia, Valdpfirremun = MC.Valdpfirremun, Valddemandacoincidente = MC.Valddemandacoincidente });
                                    }
                                };
                            }

                            if (i > dateStart.Year)
                            {
                                for (int x = 1; x <= 12; x++)
                                {
                                    int diasDelMes = DateTime.DaysInMonth(i, x);
                                    DateTime fechaF = new DateTime(i, x, diasDelMes);
                                    DateTime fechaI = new DateTime(i, x, 1);
                                    MC = servicio.GetListFullByDateMC(emprcodi, fechaI, fechaF);
                                    if (MC.Emprnomb != null)
                                    {
                                        list.Add(new VtdMontoPorCapacidadDTO() { Valofecha = MC.Valofecha, Emprnomb = MC.Emprnomb, Valomr = MC.Valomr, Valopreciopotencia = MC.Valopreciopotencia, Valdpfirremun = MC.Valdpfirremun, Valddemandacoincidente = MC.Valddemandacoincidente });
                                    }
                                };
                            }
                        }
                    }
                }
                model.MontoPorCapacidad = list;
                string ruta = AppDomain.CurrentDomain.BaseDirectory + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.FolderReporte;
                ExcelDocument.GenerarArchivoMontoCapacidad(model, ruta);
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }


        [HttpGet]
        public virtual ActionResult ExportarReporteMC()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteMontoPorCapacidad;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        #endregion

        #region Monto Por Peaje

        //Filtro po rango de fecha -Fit
        public PartialViewResult ListaMontoPorPeaje(int emprcodi, string fechaInicio, string fechaFinal, int nroPagina)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, "d M yyyy", CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, "d M yyyy", CultureInfo.InvariantCulture);

            ConsultasModel model = new ConsultasModel();
            List<VtdMontoPorPeajeDTO> list = new List<VtdMontoPorPeajeDTO>();
            VtdMontoPorPeajeDTO MP = new VtdMontoPorPeajeDTO();

            try
            {
                if (dateStart.Year == dateEnd.Year)
                {
                    for (int u = dateStart.Month; u <= dateEnd.Month; u++)
                    {
                        int diasDelMes = DateTime.DaysInMonth(dateStart.Year, u);
                        DateTime fechaF = new DateTime(dateStart.Year, u, diasDelMes);
                        DateTime fechaI = new DateTime(dateStart.Year, u, 1);
                        MP = servicio.GetListFullByDateRangeMP(emprcodi, fechaI, fechaF);
                        if (MP.Emprnomb != null)
                        {
                            list.Add(new VtdMontoPorPeajeDTO() { Valofecha = MP.Valofecha, Emprnomb = MP.Emprnomb, Valddemandacoincidente = MP.Valddemandacoincidente, Valdpeajeuni = MP.Valdpeajeuni });
                        }
                    };
                }
                else
                {
                    for (int i = dateStart.Year; i <= dateEnd.Year; i++)
                    {
                        if (i == dateEnd.Year)
                        {
                            for (int x = 1; x <= dateEnd.Month; x++)
                            {
                                int diasDelMes = DateTime.DaysInMonth(i, x);
                                DateTime fechaF = new DateTime(i, x, diasDelMes);
                                DateTime fechaI = new DateTime(i, x, 1);
                                MP = servicio.GetListFullByDateRangeMP(emprcodi, fechaI, fechaF);
                                if (MP.Emprnomb != null)
                                {
                                    list.Add(new VtdMontoPorPeajeDTO() { Valofecha = MP.Valofecha, Emprnomb = MP.Emprnomb, Valddemandacoincidente = MP.Valddemandacoincidente, Valdpeajeuni = MP.Valdpeajeuni });
                                }
                            };
                        }
                        else
                        {
                            if (i == dateStart.Year)
                            {
                                for (int e = dateStart.Month; e <= 12; e++)
                                {
                                    int diasDelMes = DateTime.DaysInMonth(i, e);
                                    DateTime fechaF = new DateTime(dateStart.Year, e, diasDelMes);
                                    DateTime fechaI = new DateTime(dateStart.Year, e, 1);
                                    MP = servicio.GetListFullByDateRangeMP(emprcodi, fechaI, fechaF);
                                    if (MP.Emprnomb != null)
                                    {
                                        list.Add(new VtdMontoPorPeajeDTO() { Valofecha = MP.Valofecha, Emprnomb = MP.Emprnomb, Valddemandacoincidente = MP.Valddemandacoincidente, Valdpeajeuni = MP.Valdpeajeuni });
                                    }
                                };
                            }

                            if (i > dateStart.Year)
                            {
                                for (int x = 1; x <= 12; x++)
                                {
                                    int diasDelMes = DateTime.DaysInMonth(i, x);
                                    DateTime fechaF = new DateTime(i, x, diasDelMes);
                                    DateTime fechaI = new DateTime(i, x, 1);
                                    MP = servicio.GetListFullByDateRangeMP(emprcodi, fechaI, fechaF);
                                    if (MP.Emprnomb != null)
                                    {
                                        list.Add(new VtdMontoPorPeajeDTO() { Valofecha = MP.Valofecha, Emprnomb = MP.Emprnomb, Valddemandacoincidente = MP.Valddemandacoincidente, Valdpeajeuni = MP.Valdpeajeuni });
                                    }
                                };
                            }
                        }
                    }
                }
                model.MontoPorPeaje = list;

                return PartialView(model);
            }
            catch (Exception ex)
            {
                return PartialView(model);
            }
        }
        [HttpPost]
        public PartialViewResult PaginadoPorRangoFechaMP(int emprcodi, string fechaInicio, string fechaFinal)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, "d M yyyy", CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, "d M yyyy", CultureInfo.InvariantCulture);

            PaginadoModel model = new PaginadoModel();
            ConsultasModel modelMC = new ConsultasModel();
            List<VtdMontoPorPeajeDTO> list = new List<VtdMontoPorPeajeDTO>();
            VtdMontoPorPeajeDTO MP = new VtdMontoPorPeajeDTO();

            try
            {
                if (dateStart.Year == dateEnd.Year)
                {
                    for (int u = dateStart.Month; u <= dateEnd.Month; u++)
                    {
                        int diasDelMes = DateTime.DaysInMonth(dateStart.Year, u);
                        DateTime fechaF = new DateTime(dateStart.Year, u, diasDelMes);
                        DateTime fechaI = new DateTime(dateStart.Year, u, 1);
                        MP = servicio.GetListFullByDateRangeMP(emprcodi, fechaI, fechaF);
                        if (MP.Emprnomb != null)
                        {
                            list.Add(new VtdMontoPorPeajeDTO() { Valofecha = MP.Valofecha, Emprnomb = MP.Emprnomb, Valddemandacoincidente = MP.Valddemandacoincidente, Valdpeajeuni = MP.Valdpeajeuni });
                        }
                    };
                }
                else
                {
                    for (int i = dateStart.Year; i <= dateEnd.Year; i++)
                    {
                        if (i == dateEnd.Year)
                        {
                            for (int x = 1; x <= dateEnd.Month; x++)
                            {
                                int diasDelMes = DateTime.DaysInMonth(i, x);
                                DateTime fechaF = new DateTime(i, x, diasDelMes);
                                DateTime fechaI = new DateTime(i, x, 1);
                                MP = servicio.GetListFullByDateRangeMP(emprcodi, fechaI, fechaF);
                                if (MP.Emprnomb != null)
                                {
                                    list.Add(new VtdMontoPorPeajeDTO() { Valofecha = MP.Valofecha, Emprnomb = MP.Emprnomb, Valddemandacoincidente = MP.Valddemandacoincidente, Valdpeajeuni = MP.Valdpeajeuni });
                                }
                            };
                        }
                        else
                        {
                            if (i == dateStart.Year)
                            {
                                for (int e = dateStart.Month; e <= 12; e++)
                                {
                                    int diasDelMes = DateTime.DaysInMonth(i, e);
                                    DateTime fechaF = new DateTime(dateStart.Year, e, diasDelMes);
                                    DateTime fechaI = new DateTime(dateStart.Year, e, 1);
                                    MP = servicio.GetListFullByDateRangeMP(emprcodi, fechaI, fechaF);
                                    if (MP.Emprnomb != null)
                                    {
                                        list.Add(new VtdMontoPorPeajeDTO() { Valofecha = MP.Valofecha, Emprnomb = MP.Emprnomb, Valddemandacoincidente = MP.Valddemandacoincidente, Valdpeajeuni = MP.Valdpeajeuni });
                                    }
                                };
                            }

                            if (i > dateStart.Year)
                            {
                                for (int x = 1; x <= 12; x++)
                                {
                                    int diasDelMes = DateTime.DaysInMonth(i, x);
                                    DateTime fechaF = new DateTime(i, x, diasDelMes);
                                    DateTime fechaI = new DateTime(i, x, 1);
                                    MP = servicio.GetListFullByDateRangeMP(emprcodi, fechaI, fechaF);
                                    if (MP.Emprnomb != null)
                                    {
                                        list.Add(new VtdMontoPorPeajeDTO() { Valofecha = MP.Valofecha, Emprnomb = MP.Emprnomb, Valddemandacoincidente = MP.Valddemandacoincidente, Valdpeajeuni = MP.Valdpeajeuni });
                                    }
                                };
                            }
                        }
                    }
                }
                modelMC.MontoPorPeaje = list;

                model.IndicadorPagina = false;

                int nroRegistros = list.Count();

                if (nroRegistros > Constantes.NroPageShow)
                {
                    int pageSize = Constantes.PageSize;
                    int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                    model.NroPaginas = nroPaginas;
                    model.NroMostrar = Constantes.NroPageShow;
                    model.IndicadorPagina = true;
                }

                return PartialView(model);
            }
            catch (Exception ex)
            {
                return PartialView(model);
            }
        }

        [HttpPost]
        public JsonResult GenerarReporteXLSPorRangoFechaMP(int emprcodi, string fechaInicio, string fechaFinal)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, "d M yyyy", CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, "d M yyyy", CultureInfo.InvariantCulture);

            int indicador = 1;
            ConsultasModel model = new ConsultasModel();
            List<VtdMontoPorPeajeDTO> list = new List<VtdMontoPorPeajeDTO>();
            VtdMontoPorPeajeDTO MP = new VtdMontoPorPeajeDTO();

            try
            {
                if (dateStart.Year == dateEnd.Year)
                {
                    for (int u = dateStart.Month; u <= dateEnd.Month; u++)
                    {
                        int diasDelMes = DateTime.DaysInMonth(dateStart.Year, u);
                        DateTime fechaF = new DateTime(dateStart.Year, u, diasDelMes);
                        DateTime fechaI = new DateTime(dateStart.Year, u, 1);
                        MP = servicio.GetListFullByDateRangeMP(emprcodi, fechaI, fechaF);
                        if (MP.Emprnomb != null)
                        {
                            list.Add(new VtdMontoPorPeajeDTO() { Valofecha = MP.Valofecha, Emprnomb = MP.Emprnomb, Valddemandacoincidente = MP.Valddemandacoincidente, Valdpeajeuni = MP.Valdpeajeuni });
                        }
                    };
                }
                else
                {
                    for (int i = dateStart.Year; i <= dateEnd.Year; i++)
                    {
                        if (i == dateEnd.Year)
                        {
                            for (int x = 1; x <= dateEnd.Month; x++)
                            {
                                int diasDelMes = DateTime.DaysInMonth(i, x);
                                DateTime fechaF = new DateTime(i, x, diasDelMes);
                                DateTime fechaI = new DateTime(i, x, 1);
                                MP = servicio.GetListFullByDateRangeMP(emprcodi, fechaI, fechaF);
                                if (MP.Emprnomb != null)
                                {
                                    list.Add(new VtdMontoPorPeajeDTO() { Valofecha = MP.Valofecha, Emprnomb = MP.Emprnomb, Valddemandacoincidente = MP.Valddemandacoincidente, Valdpeajeuni = MP.Valdpeajeuni });
                                }
                            };
                        }
                        else
                        {
                            if (i == dateStart.Year)
                            {
                                for (int e = dateStart.Month; e <= 12; e++)
                                {
                                    int diasDelMes = DateTime.DaysInMonth(i, e);
                                    DateTime fechaF = new DateTime(dateStart.Year, e, diasDelMes);
                                    DateTime fechaI = new DateTime(dateStart.Year, e, 1);
                                    MP = servicio.GetListFullByDateRangeMP(emprcodi, fechaI, fechaF);
                                    if (MP.Emprnomb != null)
                                    {
                                        list.Add(new VtdMontoPorPeajeDTO() { Valofecha = MP.Valofecha, Emprnomb = MP.Emprnomb, Valddemandacoincidente = MP.Valddemandacoincidente, Valdpeajeuni = MP.Valdpeajeuni });
                                    }
                                };
                            }

                            if (i > dateStart.Year)
                            {
                                for (int x = 1; x <= 12; x++)
                                {
                                    int diasDelMes = DateTime.DaysInMonth(i, x);
                                    DateTime fechaF = new DateTime(i, x, diasDelMes);
                                    DateTime fechaI = new DateTime(i, x, 1);
                                    MP = servicio.GetListFullByDateRangeMP(emprcodi, fechaI, fechaF);
                                    if (MP.Emprnomb != null)
                                    {
                                        list.Add(new VtdMontoPorPeajeDTO() { Valofecha = MP.Valofecha, Emprnomb = MP.Emprnomb, Valddemandacoincidente = MP.Valddemandacoincidente, Valdpeajeuni = MP.Valdpeajeuni });
                                    }
                                };
                            }
                        }
                    }
                    model.MontoPorPeaje = list;
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.FolderReporte;
                    ExcelDocument.GenerarArchivoMontoPeaje(model, ruta);
                }
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }
        [HttpGet]
        public virtual ActionResult ExportarReporteMP()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteMontoPorPeaje;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }


        #endregion

        #region Monto SCeIO

        //Filtro po rango de fecha -Fit
        public PartialViewResult ListaMontoSCeIO(int emprcodi, string fechaInicio, string fechaFinal, int nroPagina)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            ConsultasModel model = new ConsultasModel();
            try
            {
                model.MontoSCeIO = servicio.GetListPageByDateRangeSCeIO(emprcodi, dateStart, dateEnd, nroPagina, Constantes.PageSize);
                return PartialView(model);
            }
            catch (Exception)
            {
                return PartialView(model);
            }
        }
        [HttpPost]
        public PartialViewResult PaginadoPorRangoFechaSCeIO(int emprcodi, string fechaInicio, string fechaFinal)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            ConsultasModel Cant = new ConsultasModel
            {
                MontoSCeIO = servicio.GetListFullByDateRangeSCeIO(emprcodi, dateStart, dateEnd)
            };

            PaginadoModel model = new PaginadoModel();
            model.IndicadorPagina = false;

            int nroRegistros = Cant.MontoSCeIO.Count();

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult GenerarReporteXLSPorRangoFechaSCeIO(int emprcodi, string fechaInicio, string fechaFinal)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int indicador = 1;
            ConsultasModel model = new ConsultasModel();
            string ruta = AppDomain.CurrentDomain.BaseDirectory + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.FolderReporte;
            try
            {
                model.MontoSCeIO = servicio.GetListFullByDateRangeSCeIO(emprcodi, dateStart, dateEnd);
                ExcelDocument.GenerarArchivoMontoSCeIO(model, ruta);
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }
        [HttpGet]
        public virtual ActionResult ExportarReporteSCeIO()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteMontoSCeIO;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }


        #endregion

        #region Monto Por Exceso

        //Filtro po rango de fecha -Fit
        public PartialViewResult ListaMontoPorExceso(int emprcodi, string fechaInicio, string fechaFinal, int nroPagina)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            ConsultasModel model = new ConsultasModel();
            try
            {
                model.MontoPorExceso = servicio.GetListPageByDateRangeMEx(emprcodi, dateStart, dateEnd, nroPagina, Constantes.PageSize);
                return PartialView(model);
            }
            catch (Exception)
            {
                return PartialView(model);
            }
        }
        [HttpPost]
        public PartialViewResult PaginadoPorRangoFechaMEx(int emprcodi, string fechaInicio, string fechaFinal)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            ConsultasModel Cant = new ConsultasModel
            {
                MontoPorExceso = servicio.GetListFullByDateRangeMEx(emprcodi, dateStart, dateEnd)
            };

            PaginadoModel model = new PaginadoModel
            {
                IndicadorPagina = false
            };

            int nroRegistros = Cant.MontoPorExceso.Count();

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }
        [HttpPost]
        public JsonResult GenerarReporteXLSPorRangoFechaMEx(int emprcodi, string fechaInicio, string fechaFinal)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int indicador = 1;
            ConsultasModel model = new ConsultasModel();
            string ruta = AppDomain.CurrentDomain.BaseDirectory + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.FolderReporte;
            try
            {
                model.MontoPorExceso = servicio.GetListFullByDateRangeMEx(emprcodi, dateStart, dateEnd);
                ExcelDocument.GenerarArchivoMontoExceso(model, ruta);
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }
        [HttpGet]
        public virtual ActionResult ExportarReporteMEx()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteMontoPorExceso;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        #endregion

        #region LogProceso
        /// <summary>
        /// Permite ver la vista de log
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IndexLogProceso()
        {
            ViewBag.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View();
        }
        /// <summary>
        /// Muestra los datos del reporte de log
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaLogProceso(string date, int nroPagina)
        {
            DateTime fecha = DateTime.ParseExact(date, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            ConsultasModel model = new ConsultasModel();
            try
            {
                model.LogProceso = servicio.GetListPagedByDateLogProceso(fecha, nroPagina, Constantes.PageSize);
                return PartialView(model);
            }
            catch (Exception)
            {
                return PartialView(model);
            }
        }
        [HttpPost]
        public PartialViewResult PaginadoLogProceso(string date)
        {
            DateTime fecha = DateTime.ParseExact(date, Constantes.FormatoFecha, CultureInfo.InvariantCulture);


            ConsultasModel Cant = new ConsultasModel
            {
                LogProceso = servicio.GetListFullByDateLogProceso(fecha)
            };

            PaginadoModel model = new PaginadoModel();
            model.IndicadorPagina = false;

            int nroRegistros = Cant.LogProceso.Count();

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }
        #endregion


        #region Reportede Valorizaciones Diaria
        public ActionResult IndexValorizacionDiaria()
        {

            ConsultasModel model = new ConsultasModel
            {
                Empresa = servicio.ObtenerEmpresasMME()// appContacto.ObtenerEmpresas(3)
            };

            ViewBag.FormatoFechaAnioMesDia = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }

        public PartialViewResult ListaValorizacionDiaria(int emprcodi, string fechaInicio, string fechaFinal, int nroPagina)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            //Lista
            ConsultasModel model = new ConsultasModel();
            List<ValorizacionDiariaDTO> list = new List<ValorizacionDiariaDTO>();
            ValorizacionDiariaDTO VD = new ValorizacionDiariaDTO();

            //try
            //{
            if (dateStart.Year == dateEnd.Year)
            {
                for (int u = dateStart.Month; u <= dateEnd.Month; u++)
                {
                    int diasDelMes = DateTime.DaysInMonth(dateStart.Year, u);
                    DateTime fechaF = new DateTime(dateStart.Year, u, diasDelMes);
                    DateTime fechaI = new DateTime(dateStart.Year, u, 1);
                    VD = servicio.GetMontoCalculadoPorMesPorParticipante(emprcodi, fechaI, fechaF);
                    if (VD.Emprnomb != null)
                    {
                        list.Add(new ValorizacionDiariaDTO() { Valofecha = VD.Valofecha, Emprnomb = VD.Emprnomb, Valomr = VD.Valomr, Valopreciopotencia = VD.Valopreciopotencia, Valdpfirremun = VD.Valdpfirremun, Valddemandacoincidente = VD.Valddemandacoincidente, Valdpeajeuni = VD.Valdpeajeuni });
                    }
                };
            }
            else
            {
                for (int i = dateStart.Year; i <= dateEnd.Year; i++)
                {
                    if (i == dateEnd.Year)
                    {
                        for (int x = 1; x <= dateEnd.Month; x++)
                        {
                            int diasDelMes = DateTime.DaysInMonth(i, x);
                            DateTime fechaF = new DateTime(i, x, diasDelMes);
                            DateTime fechaI = new DateTime(i, x, 1);
                            VD = servicio.GetMontoCalculadoPorMesPorParticipante(emprcodi, fechaI, fechaF);
                            if (VD.Emprnomb != null)
                            {
                                list.Add(new ValorizacionDiariaDTO() { Valofecha = VD.Valofecha, Emprnomb = VD.Emprnomb, Valomr = VD.Valomr, Valopreciopotencia = VD.Valopreciopotencia, Valdpfirremun = VD.Valdpfirremun, Valddemandacoincidente = VD.Valddemandacoincidente, Valdpeajeuni = VD.Valdpeajeuni });
                            }
                        };
                    }
                    else
                    {
                        if (i == dateStart.Year)
                        {
                            for (int e = dateStart.Month; e <= 12; e++)
                            {
                                int diasDelMes = DateTime.DaysInMonth(i, e);
                                DateTime fechaF = new DateTime(dateStart.Year, e, diasDelMes);
                                DateTime fechaI = new DateTime(dateStart.Year, e, 1);
                                VD = servicio.GetMontoCalculadoPorMesPorParticipante(emprcodi, fechaI, fechaF);
                                if (VD.Emprnomb != null)
                                {
                                    list.Add(new ValorizacionDiariaDTO() { Valofecha = VD.Valofecha, Emprnomb = VD.Emprnomb, Valomr = VD.Valomr, Valopreciopotencia = VD.Valopreciopotencia, Valdpfirremun = VD.Valdpfirremun, Valddemandacoincidente = VD.Valddemandacoincidente, Valdpeajeuni = VD.Valdpeajeuni });
                                }
                            };
                        }

                        if (i > dateStart.Year)
                        {
                            for (int x = 1; x <= 12; x++)
                            {
                                int diasDelMes = DateTime.DaysInMonth(i, x);
                                DateTime fechaF = new DateTime(i, x, diasDelMes);
                                DateTime fechaI = new DateTime(i, x, 1);
                                VD = servicio.GetMontoCalculadoPorMesPorParticipante(emprcodi, fechaI, fechaF);
                                if (VD.Emprnomb != null)
                                {
                                    list.Add(new ValorizacionDiariaDTO() { Valofecha = VD.Valofecha, Emprnomb = VD.Emprnomb, Valomr = VD.Valomr, Valopreciopotencia = VD.Valopreciopotencia, Valdpfirremun = VD.Valdpfirremun, Valddemandacoincidente = VD.Valddemandacoincidente, Valdpeajeuni = VD.Valdpeajeuni });
                                }
                            };
                        }
                    }
                }
            }

            try
            {
                model.ValorizacionDiaria = servicio.GetListPagedValorizacionDiariaByFilter(emprcodi, dateStart, dateEnd, nroPagina, Constantes.PageSize);
                model.ValorizacionDiariaMes = list;

                return PartialView(model);
            }
            catch (Exception ex)
            {
                return PartialView(model);
            }
        }

        [HttpPost]
        public PartialViewResult PaginadoPorFiltroValorizacionDiaria(int emprcodi, string fechaInicio, string fechaFinal)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            ConsultasModel Cant = new ConsultasModel
            {
                ValorizacionDiaria = servicio.GetListFullValorizacionDiariaByFilter(emprcodi, dateStart, dateEnd)
            };

            PaginadoModel model = new PaginadoModel
            {
                IndicadorPagina = false
            };

            int nroRegistros = Cant.ValorizacionDiaria.Count();

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }
        [HttpPost]
        public JsonResult GenerarReporteXLSValorizacionDiaria(int emprcodi, string fechaInicio, string fechaFinal)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int indicador = 1;
            ConsultasModel model = new ConsultasModel();
            string ruta = AppDomain.CurrentDomain.BaseDirectory + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.FolderReporte;
            List<ValorizacionDiariaDTO> list = new List<ValorizacionDiariaDTO>();
            ValorizacionDiariaDTO VD = new ValorizacionDiariaDTO();

            //try
            //{
            if (dateStart.Year == dateEnd.Year)
            {
                for (int u = dateStart.Month; u <= dateEnd.Month; u++)
                {
                    int diasDelMes = DateTime.DaysInMonth(dateStart.Year, u);
                    DateTime fechaF = new DateTime(dateStart.Year, u, diasDelMes);
                    DateTime fechaI = new DateTime(dateStart.Year, u, 1);
                    VD = servicio.GetMontoCalculadoPorMesPorParticipante(emprcodi, fechaI, fechaF);
                    if (VD.Emprnomb != null)
                    {
                        list.Add(new ValorizacionDiariaDTO() { Valofecha = VD.Valofecha, Emprnomb = VD.Emprnomb, Valomr = VD.Valomr, Valopreciopotencia = VD.Valopreciopotencia, Valdpfirremun = VD.Valdpfirremun, Valddemandacoincidente = VD.Valddemandacoincidente, Valdpeajeuni = VD.Valdpeajeuni });
                    }
                };
            }
            else
            {
                for (int i = dateStart.Year; i <= dateEnd.Year; i++)
                {
                    if (i == dateEnd.Year)
                    {
                        for (int x = 1; x <= dateEnd.Month; x++)
                        {
                            int diasDelMes = DateTime.DaysInMonth(i, x);
                            DateTime fechaF = new DateTime(i, x, diasDelMes);
                            DateTime fechaI = new DateTime(i, x, 1);
                            VD = servicio.GetMontoCalculadoPorMesPorParticipante(emprcodi, fechaI, fechaF);
                            if (VD.Emprnomb != null)
                            {
                                list.Add(new ValorizacionDiariaDTO() { Valofecha = VD.Valofecha, Emprnomb = VD.Emprnomb, Valomr = VD.Valomr, Valopreciopotencia = VD.Valopreciopotencia, Valdpfirremun = VD.Valdpfirremun, Valddemandacoincidente = VD.Valddemandacoincidente, Valdpeajeuni = VD.Valdpeajeuni });
                            }
                        };
                    }
                    else
                    {
                        if (i == dateStart.Year)
                        {
                            for (int e = dateStart.Month; e <= 12; e++)
                            {
                                int diasDelMes = DateTime.DaysInMonth(i, e);
                                DateTime fechaF = new DateTime(dateStart.Year, e, diasDelMes);
                                DateTime fechaI = new DateTime(dateStart.Year, e, 1);
                                VD = servicio.GetMontoCalculadoPorMesPorParticipante(emprcodi, fechaI, fechaF);
                                if (VD.Emprnomb != null)
                                {
                                    list.Add(new ValorizacionDiariaDTO() { Valofecha = VD.Valofecha, Emprnomb = VD.Emprnomb, Valomr = VD.Valomr, Valopreciopotencia = VD.Valopreciopotencia, Valdpfirremun = VD.Valdpfirremun, Valddemandacoincidente = VD.Valddemandacoincidente, Valdpeajeuni = VD.Valdpeajeuni });
                                }
                            };
                        }

                        if (i > dateStart.Year)
                        {
                            for (int x = 1; x <= 12; x++)
                            {
                                int diasDelMes = DateTime.DaysInMonth(i, x);
                                DateTime fechaF = new DateTime(i, x, diasDelMes);
                                DateTime fechaI = new DateTime(i, x, 1);
                                VD = servicio.GetMontoCalculadoPorMesPorParticipante(emprcodi, fechaI, fechaF);
                                if (VD.Emprnomb != null)
                                {
                                    list.Add(new ValorizacionDiariaDTO() { Valofecha = VD.Valofecha, Emprnomb = VD.Emprnomb, Valomr = VD.Valomr, Valopreciopotencia = VD.Valopreciopotencia, Valdpfirremun = VD.Valdpfirremun, Valddemandacoincidente = VD.Valddemandacoincidente, Valdpeajeuni = VD.Valdpeajeuni });
                                }
                            };
                        }
                    }
                }
            }

            try
            {
                model.ValorizacionDiaria = servicio.GetListFullValorizacionDiariaByFilter(emprcodi, dateStart, dateEnd);
                model.ValorizacionDiariaMes = list;
                ExcelDocument.GenerarArchivoVD(model, ruta);
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }
        [HttpGet]
        public virtual ActionResult ExportarReporteVD()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteValorizacionDiaria;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        #endregion

        #region Reporte Informacion Prevista Remitida al Particpante

        public ActionResult IndexReporteInformacionPrevista()
        {
            ViewBag.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
            ConsultasModel model = new ConsultasModel
            {
                Empresa = servicioEmpr.ListarEmpresaTodo()
            };
            return View(model);
        }
        public PartialViewResult ListaInformacionPrevistaRemitidaAlParticipante(string fechaInicio, string fechaFinal, int emprcodi, int nroPagina)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            ConsultasModel model = new ConsultasModel();
            try
            {
                model.InformacionPrevista = servicio.GetListPagedByFilterInformacionPRP(dateStart, dateEnd, emprcodi, nroPagina, Constantes.PageSize);
                return PartialView(model);
            }
            catch (Exception)
            {
                return PartialView(model);
            }
        }
        [HttpPost]
        public PartialViewResult PaginadoPorFiltroIPRemitidaAlParticipante(string fechaInicio, string fechaFinal, int emprcodi)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            ConsultasModel Cant = new ConsultasModel
            {
                InformacionPrevista = servicio.GetListFullFilterInformacionPRP(dateStart, dateEnd, emprcodi)
            };

            PaginadoModel model = new PaginadoModel();
            model.IndicadorPagina = false;

            int nroRegistros = Cant.InformacionPrevista.Count();

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }
        [HttpPost]
        public JsonResult GenerarReporteXLSPorFiltrIPRalParticipante(string fechaInicio, string fechaFinal, int emprcodi)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int indicador = 1;
            ConsultasModel model = new ConsultasModel();
            string ruta = AppDomain.CurrentDomain.BaseDirectory + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.FolderReporte;
            try
            {
                model.InformacionPrevista = servicio.GetListFullFilterInformacionPRP(dateStart, dateEnd, emprcodi);
                ExcelDocument.GenerarArchivoInformacionPRP(model, ruta);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }

            return Json(indicador);
        }
        [HttpGet]
        public virtual ActionResult ExportarReporteIPRP()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteInformacionPrevista;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }


        #endregion


        #region Informacion Prevista Remitida por el Participante

        /// <summary>
        /// Muestra El formato Excel en la Web 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="desEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarHojaExcelWeb(int idEmpresa, string fechaInicio, string fechaFinal, int idformato)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            FormatoModel jsModel = new FormatoModel();
            jsModel = BuildHojaExcel(idEmpresa, dateStart, dateEnd, idformato);

            return Json(jsModel);
        }

        /// <summary>
        /// Carga lista de Filas que indican si esta bloqueadas o no, valido para formatos en tiempo real.
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formatcodi"></param>
        /// <param name="filHead"></param>
        /// <param name="filData"></param>
        /// <param name="plazo"></param>
        /// <returns></returns>
        protected List<bool> CargarListaFilaReadOnly(DateTime fechaProceso, int idEmpresa, int formatcodi, int filHead, int filData, bool plazo)
        {
            List<bool> lista = new List<bool>();
            int horaini = 0;
            int horafin = -1;

            DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime fechaActual2 = DateTime.Now;
            int dif = (fechaActual - fechaProceso).Days;
            int hora = fechaActual2.Hour;
            if (dif == 0)
            {

                int periodo = hora / 3;
                horaini = periodo * 3;
                horafin = horaini + 3 - 1;
            }
            if (dif == 1)
            {
                if (hora < 3 - 1)
                {
                    horafin = 3 - 1;
                }
            }

            for (int i = 0; i < filHead; i++)
            {
                lista.Add(true);
            }


            for (int i = 0; i < filData; i++)
            {
                if (i >= horaini && i <= horafin)
                    lista.Add(false);
                else
                    lista.Add(true);

            }
            return lista;
        }

        /// <summary>
        /// Permite generar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarFormato(int idEmpresa, string fechaInicio, string fechaFinal, int idformato)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime dateEnd = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int indicador = 0;
            int idEnvio = 0;

            string ruta = AppDomain.CurrentDomain.BaseDirectory + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.FolderReporte;
            try
            {
                FormatoModel model = BuildHojaExcel(idEmpresa, dateStart, dateEnd, idformato);
                ExcelDocument.GenerarFileExcel(model, ruta);
                indicador = 1;

            }

            catch (Exception ex)
            {
                indicador = -1;
            }
            return Json(indicador);

        }

        /// <summary>
        ///Devuelve el model hidrologia con todos sus capos llenos para mostrar el aexceweb en la interfaz web
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public FormatoModel BuildHojaExcel(int idEmpresa, DateTime fechaInicio, DateTime fechaFinal, int idformato)
        {
            FormatoModel model = new FormatoModel();
            model.Handson = new HandsonModel();
            model.Handson.ListaMerge = new List<CeldaMerge>();
            model.Handson.ListaColWidth = new List<int>();

            int moreDays = (int)(fechaFinal - fechaInicio).TotalDays;

            int idEnvio = 0;

            MeFormatoDTO formato = new MeFormatoDTO()
            {
                Cabcodi = 29, // usar cabecera Cliente - Barra
                Formathorizonte = moreDays,
                Formatresolucion = 15,
                Formatperiodo = (idformato == Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.EPRFORMATODIARIO) ? 1 : 6,
                Formatcodi = idformato
            };

            ////////// Obtiene el Fotmato ////////////////////////
            model.Formato = formato;
            this.Formato = model.Formato;
            var cabecera = logic.GetListMeCabecera().Where(x => x.Cabcodi == model.Formato.Cabcodi).FirstOrDefault();

            /// DEFINICION DEL FORMATO //////
            model.Formato.Formatcols = cabecera.Cabcolumnas;
            model.Formato.Formatrows = cabecera.Cabfilas;
            model.Formato.Formatheaderrow = cabecera.Cabcampodef;
            model.ColumnasCabecera = model.Formato.Formatcols;
            model.FilasCabecera = model.Formato.Formatrows;

            ///
            var entEmpresa = logic.GetByIdSiEmpresa(idEmpresa);
            if (entEmpresa != null)
            {
                model.Empresa = entEmpresa.Emprnomb;
            }

            model.Formato.FechaProceso = new DateTime(fechaInicio.Year, fechaInicio.Month, fechaInicio.Day);

            //Mostrar último envio cuando se muestra la interfaz de Carga de datos de un formato
            int idUltEnvio = 0; //Si se esta consultando el ultimo envio se podra activar el boton editar
            model.ListaEnvios = logic.GetByCriteriaMeEnvios(idEmpresa, idformato, model.Formato.FechaProceso);
            if (model.ListaEnvios.Count > 0)
            {
                model.IdEnvioLast = model.ListaEnvios[model.ListaEnvios.Count - 1].Enviocodi;
                idUltEnvio = model.IdEnvioLast;
                idEnvio = model.IdEnvioLast;
            }

            int idCfgFormato = 0;
            model.Formato.IdEnvio = idEnvio;
            /// Verifica si Formato esta en Plaz0
            string mensaje = string.Empty;
            int horaini = 0;//Para Formato Tiempo Real
            int horafin = 0;//Para Formato Tiempo Real
            if (idEnvio <= 0)
            {
                model.Formato.Emprcodi = idEmpresa;
                //FormatoMedicionAppServicio.GetSizeFormato(model.Formato); // OBTIENE FECHA
                formato.FechaInicio = formato.FechaProceso;
                formato.FechaFin = formato.FechaProceso.AddDays(moreDays);
                formato.Formathorizonte = ((TimeSpan)(formato.FechaFin - formato.FechaInicio)).Days + 1;
                formato.RowPorDia = ParametrosFormato.ResolucionDia / (int)formato.Formatresolucion;
                //model.EnPlazo = true; // logic.ValidarPlazo(model.Formato);
                //model.TipoPlazo = ConstantesEnvio.ENVIO_EN_PLAZO; // logic.EnvioValidarPlazo(model.Formato, idEmpresa);
                model.Handson.ReadOnly = true; // ConstantesEnvio.ENVIO_EN_PLAZO == model.TipoPlazo;

                //ObtenerH24IniFinTR(model.Formato, !model.Handson.ReadOnly, out horaini, out horafin);
            }
            else  // Fecha proceso es obtenida del registro envio
            {
                model.Handson.ReadOnly = true;

                var envioAnt = logic.GetByIdMeEnvio(idEnvio);
                if (envioAnt != null)
                {
                    model.Formato.FechaEnvio = envioAnt.Enviofecha;
                    model.FechaEnvio = ((DateTime)envioAnt.Enviofecha).ToString(Constantes.FormatoFechaHora);
                    model.Formato.FechaProceso = (DateTime)envioAnt.Enviofechaperiodo;
                    if (envioAnt.Cfgenvcodi != null)
                    {
                        idCfgFormato = (int)envioAnt.Cfgenvcodi;
                    }
                    model.EnPlazo = envioAnt.Envioplazo == "P";
                }
                else
                {
                    model.Formato.FechaProceso = DateTime.MinValue;
                }

                formato.FechaInicio = formato.FechaProceso;
                formato.FechaFin = formato.FechaProceso.AddDays(moreDays);
                formato.Formathorizonte = ((TimeSpan)(formato.FechaFin - formato.FechaInicio)).Days + 1;
                formato.RowPorDia = ParametrosFormato.ResolucionDia / (int)formato.Formatresolucion;
            }

            model.ListaHojaPto = logic.GetListaPtos(model.Formato.FechaProceso, idCfgFormato, idEmpresa, idformato, cabecera.Cabquery); // QUERY
            var cabecerasRow = cabecera.Cabcampodef.Split(QueryParametros.SeparadorFila);
            List<CabeceraRow> listaCabeceraRow = new List<CabeceraRow>();
            for (var x = 0; x < cabecerasRow.Length; x++)
            {
                var reg = new CabeceraRow();
                var fila = cabecerasRow[x].Split(QueryParametros.SeparadorCol);
                reg.NombreRow = fila[0];
                reg.TituloRow = fila[1];
                reg.IsMerge = int.Parse(fila[2]);
                listaCabeceraRow.Add(reg);
            }

            model.Editable = model.Handson.ReadOnly ? 1 : 0;
            model.IdEmpresa = idEmpresa;
            model.Anho = model.Formato.FechaProceso.Year.ToString();
            model.Mes = Base.Tools.Util.ObtenerNombreMes(model.Formato.FechaProceso.Month);
            //model.Semana = semana;
            model.Dia = model.Formato.FechaProceso.Day.ToString();
            model.Handson.Width = HandsonConstantes.ColWidth * ((model.ListaHojaPto.Count > HandsonConstantes.ColPorHoja) ? HandsonConstantes.ColPorHoja :
                (model.ListaHojaPto.Count + model.ColumnasCabecera));

            List<object> lista = new List<object>(); /// Contiene los valores traidos de de BD del envio seleccionado.
            List<MeCambioenvioDTO> listaCambios = new List<MeCambioenvioDTO>(); /// contiene los cambios de que ha habido en el envio que se esta consultando.
            int nCol = model.ListaHojaPto.Count;
            int nBloques = model.Formato.Formathorizonte * model.Formato.RowPorDia; // SE DEFINE HORIZONTE (NRO DE DIAS)
            model.Handson.ListaFilaReadOnly = ExcelDocument.InicializaListaFilaReadOnly(model.Formato.Formatrows, nBloques);
            if (model.Formato.Formatresolucion == ParametrosFormato.ResolucionHora && model.Formato.Formatperiodo == ParametrosFormato.PeriodoDiario && model.Formato.Formatdiaplazo == 0)
                model.Handson.ListaFilaReadOnly = CargarListaFilaReadOnly(model.Formato.FechaProceso, idEmpresa, idformato, model.Formato.Formatrows, nBloques, model.EnPlazo);

            model.ListaCambios = new List<CeldaCambios>();
            model.Handson.ListaExcelData = ExcelDocument.InicializaMatrizExcel(model.Formato.Formatrows, nBloques, model.Formato.Formatcols, nCol);

            if (idEnvio == 0)
            {
                lista = logic.GetDataFormato(idEmpresa, model.Formato, idEnvio, idUltEnvio);
                ExcelDocument.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);
            }
            if (idEnvio > 0)
            {
                lista = logic.GetDataFormato(idEmpresa, model.Formato, idEnvio, idUltEnvio);
                listaCambios = logic.GetAllCambioEnvio(idformato, model.Formato.FechaInicio, model.Formato.FechaFin, idEnvio, idEmpresa).Where(x => x.Enviocodi == idEnvio).ToList();
                ExcelDocument.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);
            }
            if (idEnvio < 0)
            {
                model.Handson.ListaExcelData = this.MatrizExcel; /// Data del excel cargado previamente se ha guardado en una variable session
                ExcelDocument.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);
            }

            #region Filas Cabeceras

            for (var ind = 0; ind < model.ColumnasCabecera; ind++)
            {
                model.Handson.ListaColWidth.Add(120);
            }
            string sTitulo = string.Empty;
            string sTituloAnt = string.Empty;
            int column = model.ColumnasCabecera;
            var cellMerge = new CeldaMerge();
            foreach (var reg in model.ListaHojaPto)
            {
                model.Handson.ListaColWidth.Add(100);
                for (var w = 0; w < model.FilasCabecera; w++)
                {
                    if (column == model.ColumnasCabecera)
                    {
                        model.Handson.ListaExcelData[w] = new string[model.ListaHojaPto.Count + model.ColumnasCabecera];
                        model.Handson.ListaExcelData[w][0] = listaCabeceraRow[w].TituloRow;
                    }
                    model.Handson.ListaExcelData[w][column] = (string)reg.GetType().GetProperty(listaCabeceraRow[w].NombreRow).GetValue(reg, null);
                    if (listaCabeceraRow[w].IsMerge == 1)
                    {
                        if (listaCabeceraRow[w].TituloRowAnt != model.Handson.ListaExcelData[w][column])
                        {
                            if (column != model.ColumnasCabecera)
                            {
                                if ((column - listaCabeceraRow[w].ColumnIni) > 1)
                                {
                                    cellMerge = new CeldaMerge();
                                    cellMerge.col = listaCabeceraRow[w].ColumnIni;
                                    cellMerge.row = w;
                                    cellMerge.colspan = (column - listaCabeceraRow[w].ColumnIni);
                                    cellMerge.rowspan = 1;
                                    model.Handson.ListaMerge.Add(cellMerge);
                                }
                            }
                            listaCabeceraRow[w].TituloRowAnt = model.Handson.ListaExcelData[w][column];
                            listaCabeceraRow[w].ColumnIni = column;
                        }
                    }
                }
                column++;

            }
            if ((column - 1) != model.ColumnasCabecera)
            {
                for (var i = 0; i < listaCabeceraRow.Count; i++)
                {
                    if ((listaCabeceraRow[i].TituloRowAnt == model.Handson.ListaExcelData[i][column - 1]))
                    {
                        if ((column - listaCabeceraRow[i].ColumnIni) > 1)
                        {
                            cellMerge = new CeldaMerge();
                            cellMerge.col = listaCabeceraRow[i].ColumnIni;
                            cellMerge.row = i;
                            cellMerge.colspan = (column - listaCabeceraRow[i].ColumnIni);
                            cellMerge.rowspan = 1;
                            model.Handson.ListaMerge.Add(cellMerge);
                        }
                    }
                }
            }

            #endregion

            model.IdEnvio = idEnvio;
            return model;
        }

        /// <summary>
        /// Permite descargar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.FolderReporte;
            string fullPath = ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteInformacionPrevista;
            return File(fullPath, Constantes.AppExcel, Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteInformacionPrevista);
        }
        #endregion
    }
}