using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.Informefalla;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.Eventos.Models;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.PruebasAleatorias;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.Evento;
using System.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using COES.MVC.Intranet.Areas.Eventos.Helper;

namespace COES.MVC.Intranet.Areas.Eventos.Controllers
{
    public class InformefallaController : BaseController
    {

        /// <summary>
        /// Fecha de Inicio de consulta
        /// </summary>
        public DateTime? FechaInicio
        {
            get
            {
                return (Session[DatosSesion.FechaConsultaInicio] != null) ?
                    (DateTime?)(Session[DatosSesion.FechaConsultaInicio]) : null;
            }
            set
            {
                Session[DatosSesion.FechaConsultaInicio] = value;
            }
        }

        /// <summary>
        /// Fecha de Fin de Consulta
        /// </summary>
        public DateTime? FechaFinal
        {
            get
            {
                return (Session[DatosSesion.FechaConsultaFin] != null) ?
                  (DateTime?)(Session[DatosSesion.FechaConsultaFin]) : null;
            }
            set
            {
                Session[DatosSesion.FechaConsultaFin] = value;
            }
        }

        /// <summary>
        /// Estado Informe
        /// </summary>
        public string InformeFalla
        {
            get { return (string) Session[DatosSesion.FiltroInformeFalla]; }
            set { Session[DatosSesion.FiltroInformeFalla] = value; }
        }


        InformefallaAppServicio servInformefalla = new InformefallaAppServicio();
        PruebasAleatoriasAppServicio servPruebasaleatorias = new PruebasAleatoriasAppServicio();
        GeneralAppServicio servEmpresa = new GeneralAppServicio();
        EventosAppServicio servEventos = new EventosAppServicio();

        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            BusquedaEveInformefallaModel model = new BusquedaEveInformefallaModel();

            model.ListaEmpresa = servEmpresa.ObtenerEmpresasCOES();
            model.FechaIni = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaIni = (this.FechaInicio != null) ? ((DateTime)this.FechaInicio).ToString(Constantes.FormatoFecha) :
               DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaFin = (this.FechaFinal != null) ? ((DateTime)this.FechaFinal).ToString(Constantes.FormatoFecha) :
                DateTime.Now.ToString(Constantes.FormatoFecha);
            model.InformeFalla = (this.InformeFalla != null) ? this.InformeFalla : "N";

            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            return View(model);
        }



        /// <summary>
        /// Permite editar un informe de falla
        /// </summary>
        /// <param name="id">Código de informe</param>
        /// <param name="accion">Acción a realizar</param>
        /// <returns>Informe</returns>
        public ActionResult Editar(int id, int accion)
        {

            EveInformefallaModel model = new EveInformefallaModel();
            EveInformefallaDTO eveInformefalla = null;

            model.ListaProgramador = this.servPruebasaleatorias.ListarProgramador();

            if (id != 0)
                eveInformefalla = servInformefalla.GetByIdEveInformefalla(id);

            if (eveInformefalla != null)
            {
                model.EveInformefalla = eveInformefalla;
            }
            else
            {
                eveInformefalla = new EveInformefallaDTO();

                eveInformefalla.Eveninflastuser = User.Identity.Name;
                eveInformefalla.Eveninflastdate = DateTime.Now;
                model.EveInformefalla = eveInformefalla;
            }

            model.Accion = accion;
            return View(model);

        }



        /// <summary>
        /// Permite eliminar un informe de falla
        /// </summary>
        /// <param name="id">Código de informe</param>
        /// <returns>1: ok. -1: error</returns>
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            try
            {
                servInformefalla.DeleteEveInformefalla(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite grabar un informe
        /// </summary>
        /// <param name="model">Informe</param>
        /// <returns>Código de informe. -1: si hubo error</returns>
        [HttpPost]
        public JsonResult Grabar(EveInformefallaModel model)
        {
            try
            {

                EveInformefallaDTO entity = new EveInformefallaDTO();

                entity.Eveninfcodi = model.EveninfCodi;
                entity.Evencodi = model.EvenCodi;
                entity.Evenanio = model.EvenAnio;
                entity.Evencorr = model.EvenCorr;

                entity.Eveninfmem = model.EveninfMem;
                entity.Evencorrmem = model.EvenCorrmem;
                entity.EvencorrSco = model.EvenCorrSco;
                entity.Eveninfactuacion = model.EveninfActuacion;

                //extranet osinergmin
                if (model.EveninfActFecha != null)
                {
                    entity.Eveninfactfecha = DateTime.ParseExact(model.EveninfActFecha, Constantes.FormatoFechaFull,
                        CultureInfo.InvariantCulture);
                }

                if (model.EveninfActElab != null && model.EveninfActElab != "" && model.EveninfActElab != "0")
                {
                    entity.Eveninfactelab = model.EveninfActElab;
                }

                entity.Eveninfactllamado = model.EveninfActLlamado;


                //preliminar inicial
                if (model.EveninfPIFechEmis != null)
                {
                    entity.Eveninfpifechemis = DateTime.ParseExact(model.EveninfPIFechEmis, Constantes.FormatoFechaFull,
                        CultureInfo.InvariantCulture);
                }

                if (model.EveninfPIElab != null && model.EveninfPIElab != "" && model.EveninfPIElab != "0")
                {
                    entity.Eveninfpielab = model.EveninfPIElab;
                }

                if (model.EveninfPIRevs != null && model.EveninfPIRevs != "" && model.EveninfPIRevs != "0")
                {
                    entity.Eveninfpirevs = model.EveninfPIRevs;
                }

                if (model.EveninfPIEmit != null && model.EveninfPIEmit != "" && model.EveninfPIEmit != "0")
                {
                    entity.Eveninfpiemit = model.EveninfPIEmit;
                }

                entity.Eveninfpiemitido = model.EveninfPIEmitido;

                //preliminar
                if (model.EveninfPFechEmis != null)
                {
                    entity.Eveninfpfechemis = DateTime.ParseExact(model.EveninfPFechEmis, Constantes.FormatoFechaFull,
                        CultureInfo.InvariantCulture);
                }

                if (model.EveninfPElab != null && model.EveninfPElab != "" && model.EveninfPElab != "0")
                {
                    entity.Eveninfpelab = model.EveninfPElab;
                }

                if (model.EveninfPRevs != null && model.EveninfPRevs != "" && model.EveninfPRevs != "0")
                {
                    entity.Eveninfprevs = model.EveninfPRevs;
                }

                if (model.EveninfPEmit != null && model.EveninfPEmit != "" && model.EveninfPEmit != "0")
                {
                    entity.Eveninfpemit = model.EveninfPEmit;
                }

                entity.Eveninfpemitido = model.EveninfPEmitido;

                //final
                if (model.EveninfFechEmis != null)
                {
                    entity.Eveninffechemis = DateTime.ParseExact(model.EveninfFechEmis, Constantes.FormatoFechaFull,
                        CultureInfo.InvariantCulture);
                }

                if (model.EveninfElab != null && model.EveninfElab != "" && model.EveninfElab != "0")
                {
                    entity.Eveninfelab = model.EveninfElab;
                }

                if (model.EveninfRevs != null && model.EveninfRevs != "" && model.EveninfRevs != "0")
                {
                    entity.Eveninfrevs = model.EveninfRevs;
                }

                if (model.EveninfEmit != null && model.EveninfEmit != "" && model.EveninfEmit != "0")
                {
                    entity.Eveninfemit = model.EveninfEmit;

                }

                entity.Eveninfemitido = model.EveninfEmitido;

                //mem
                if (model.EveninfMemFechEmis != null)
                {
                    entity.Eveninfmemfechemis = DateTime.ParseExact(model.EveninfMemFechEmis,
                        Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                }


                if (model.EveninfMemElab != null && model.EveninfMemElab != "" && model.EveninfMemElab != "0")
                {
                    entity.Eveninfmemelab = model.EveninfMemElab;
                }

                if (model.EveninfMemRevs != null && model.EveninfMemRevs != "" && model.EveninfMemRevs != "0")
                {
                    entity.Eveninfmemrevs = model.EveninfMemRevs;
                }

                if (model.EveninfMemEmit != null && model.EveninfMemEmit != "" && model.EveninfMemEmit != "0")
                {
                    entity.Eveninfmememit = model.EveninfMemEmit;
                }

                entity.Eveninfmememitido = model.EveninfMemEmitido;
                entity.Eveninflastuser = User.Identity.Name;
                entity.Eveninflastdate = DateTime.Now;

                int id = this.servInformefalla.SaveEveInformefallaId(entity);
                return Json(id);

            }
            catch
            {
                return Json(-1);
            }
        }



        /// <summary>
        /// Permite obtener un listado
        /// </summary>
        /// <param name="infMem">Informe MEM (S/N)</param>
        /// <param name="infEmitido">Informe Emitido (S/N)</param>
        /// <param name="emprCodi">Código de empresa</param>
        /// <param name="equiAbrev">Abrevistura de equipo</param>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <param name="nroPage">Número de página</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string infMem, string infEmitido, int emprCodi, string equiAbrev,
            string fechaIni, string fechaFin, int nroPage)
        {
            BusquedaEveInformefallaModel model = new BusquedaEveInformefallaModel();

            DateTime fechaInicio = DateTime.ParseExact("01/01/" + DateTime.Now.Year.ToString(), Constantes.FormatoFecha,
                CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.Now;

            if (fechaIni != null)
            {
                fechaInicio = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            if (fechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            this.FechaInicio = fechaInicio;
            this.FechaFinal = fechaFinal;
            this.InformeFalla = infEmitido;

             model.ListaEveInformefalla = servInformefalla.BuscarOperaciones(infMem, infEmitido, emprCodi, equiAbrev,
                fechaInicio, fechaFinal, nroPage, Constantes.PageSizeEvento).ToList();

            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);

            return PartialView(model);
        }



        /// <summary>
        /// Permite hacer el paginado
        /// </summary>
        /// <param name="infMem">Informe MEM (S/N)</param>
        /// <param name="infEmitido">Informe Emitido (S/N)</param>
        /// <param name="emprCodi">Código de empresa</param>
        /// <param name="equiAbrev">Abrevistura de equipo</param>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Paginado</returns>
        [HttpPost]
        public PartialViewResult Paginado(string infMem, string infEmitido, int emprCodi, string equiAbrev,
            string fechaIni, string fechaFin)
        {
            
            Paginacion model = new Paginacion();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (fechaIni != null)
            {
                fechaInicio = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            if (fechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }


            int nroRegistros = servInformefalla.ObtenerNroFilas(infMem, infEmitido, emprCodi, equiAbrev,
                fechaInicio, fechaFinal);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeEvento;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }
            
            return base.Paginado(model);
        }
        /// <summary>
        /// Permite hacer el paginado
        /// </summary>
        /// <param name="idEvento">Informe MEM (S/N)</param>
        /// <returns>Paginado</returns>
        [HttpPost]

        public PartialViewResult ListaEnviosEventos(int idEvento)
        {
            EventoModel model = new EventoModel();
            model.ListaEnvios = this.servEventos.ListaInformeEnvios(idEvento);
   
            return PartialView(model);
        }


        /// <summary>
        /// Permite hacer el paginado a los envíos
        /// </summary>
        /// <param name="idEvento">Id del evento</param>
        /// <returns>Paginado</returns>
        [HttpPost]
        public PartialViewResult PaginadoEnvios(int idEvento)
        {

            BusquedaEquipoModel model = new BusquedaEquipoModel();


            int nroRegistros = this.servEventos.ListaInformeEnvios(idEvento).Count;

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeEvento;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }



        [HttpPost]
        public JsonResult ExportarLog(BusquedaEveInformefallaModel model)
        {
            string nombreArchivo = "ReporteEnvios" + DateTime.Now.ToString("yyyyMMddHHmm");
            string rutaArchivo = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile] + nombreArchivo + ".xlsx";
            string PathLogo = @"Content\Images\logocoes.png";
            string rutaLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; 

            MeEnvioDTO dto = new MeEnvioDTO();
            dto.FechaIni = model.FechaIni;
            dto.FechaFin = model.FechaFin;
            dto.TipoFalla = "1";

            EventoModel datos = new EventoModel();
            datos.ListaEnvios = this.servEventos.ListaInformeEnviosLog(dto);

                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet ws = null;
                    ws = package.Workbook.Worksheets.Add("Reporte");
                    ws.View.ShowGridLines = true;
                    FormatoEventoHelper.AddImage(ws, 0, 0, rutaLogo);

                    ws.Cells[2, 3].Value = "Reporte de Envíos de Informes de Falla N1";

                    ws.Cells[4, 1].Value = "Código de Envío";
                    ws.Cells[4, 2].Value = "Fecha y Hora del Evento";
                    ws.Cells[4, 3].Value = "Descripción(Resumen)";
                    ws.Cells[4, 4].Value = "Fecha y Hora del Envío de Extranet";
                    ws.Cells[4, 5].Value = "Usuario que reportó";
                    ws.Cells[4, 6].Value = "Empresa";
                    ws.Cells[4, 7].Value = "Tipo de Informe";
                    ws.Cells[4, 8].Value = "Documentos Enviados";

                    System.Drawing.Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#2980B9");
                    ws.Cells["C2:E2"].Style.Font.Bold = true;
                    ws.Cells["A4:H4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells["A4:H4"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    ws.Cells["A4:H4"].Style.Font.Bold = true;
                    ws.Cells["A4:H4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    ws.Cells["A4:H4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //ws.Cells["A4:L4"].Style.WrapText = true;
                    ws.Cells["A4:H4"].Style.Fill.BackgroundColor.SetColor(colFromHex);

                    ws.Column(1).Width = 20;
                    ws.Column(2).Width = 20;
                    ws.Column(3).Width = 50;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 20;
                    ws.Column(6).Width = 20;
                    ws.Column(7).Width = 20;
                    ws.Column(8).Width = 50;

                    int fila = 5;

                foreach (MeEnvioDTO item in datos.ListaEnvios)
                {
                    //"dd/MM/yyyy HH:mm:ss"
                    ws.Cells[fila, 1].Value = item.Envevencodi;
                    ws.Cells[fila, 2].Value = item.Evenini.Value.ToString("dd/MM/yyyy HH:mm:ss");
                    ws.Cells[fila, 3].Value = item.Evenasunto;
                    ws.Cells[fila, 4].Value = item.Enviofecha.Value.ToString("dd/MM/yyyy HH:mm:ss");
                    ws.Cells[fila, 5].Value = item.Userlogin;
                    ws.Cells[fila, 6].Value = item.Emprnomb;
                    ws.Cells[fila, 7].Value = item.TipoInforme; 
                    ws.Cells[fila, 8].Value = item.Eveinfrutaarchivo;

                    fila++;
                }

                System.IO.File.WriteAllBytes(rutaArchivo, package.GetAsByteArray());
                }
     

            return Json(nombreArchivo, JsonRequestBehavior.AllowGet);
        }

        public FileContentResult Descargar(string nombreArchivo)
        {
            string[] archivo = nombreArchivo.Split('|');
            string nombre = string.Empty;
            nombre = "ReporteEnvios.xlsx";
            string rutaArchivo = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile] + archivo[0] + ".xlsx";
            var FileBytesArray = System.IO.File.ReadAllBytes(rutaArchivo);

            System.IO.File.Delete(rutaArchivo);

            return File(FileBytesArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombre);
        }

    }
}
