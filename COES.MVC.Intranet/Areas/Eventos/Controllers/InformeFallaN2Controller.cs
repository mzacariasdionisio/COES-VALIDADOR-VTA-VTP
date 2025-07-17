using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.InformefallaN2;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.Eventos.Models;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.PruebasAleatorias;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.Evento;
using System.Configuration;
using OfficeOpenXml;
using COES.MVC.Intranet.Areas.Eventos.Helper;
using OfficeOpenXml.Style;

namespace COES.MVC.Intranet.Areas.Eventos.Controllers
{
    public class InformeFallaN2Controller : BaseController
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
            get { return (string)Session[DatosSesion.FiltroInformeFalla]; }
            set { Session[DatosSesion.FiltroInformeFalla] = value; }
        }

        InformefallaN2AppServicio servInformefallaN2 = new InformefallaN2AppServicio();
        GeneralAppServicio servEmpresa = new GeneralAppServicio();
        PruebasAleatoriasAppServicio servPruebasaleatorias = new PruebasAleatoriasAppServicio();
        EventosAppServicio servEventos = new EventosAppServicio();


        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            BusquedaEveInformefallaN2Model model = new BusquedaEveInformefallaN2Model();
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

            EveInformefallaN2Model model = new EveInformefallaN2Model();
            EveInformefallaN2DTO eveInformefallaN2 = null;

            model.ListaProgramador = this.servPruebasaleatorias.ListarProgramador();

            if (id != 0)
                eveInformefallaN2 = servInformefallaN2.GetByIdEveInformefallaN2(id);

            if (eveInformefallaN2 != null)
            {
                model.EveInformefallaN2 = eveInformefallaN2;
            }
            else
            {
                eveInformefallaN2 = new EveInformefallaN2DTO();
                eveInformefallaN2.Eveninfn2lastuser = User.Identity.Name;
                eveInformefallaN2.Eveninfn2lastdate = DateTime.Now;
                model.EveInformefallaN2 = eveInformefallaN2;
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
                servInformefallaN2.DeleteEveInformefallaN2(id);
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
        public JsonResult Grabar(EveInformefallaN2Model model)
        {
            try
            {

                EveInformefallaN2DTO entity = new EveInformefallaN2DTO();

                entity.Eveninfn2codi = model.EvenInfN2Codi;
                entity.Evencodi = model.EvenCodi;
                entity.Evenanio = model.EvenAnio;
                entity.Evenn2corr = model.EvenN2Corr;

                //preliminar inicial
                //sin informe de empresa
                if (model.EvenIPIEN2FechEm != null)
                {
                    entity.EvenipiEN2fechem = DateTime.ParseExact(model.EvenIPIEN2FechEm, Constantes.FormatoFechaFull,
                        CultureInfo.InvariantCulture);
                }
                if (model.EvenIPIEN2Elab != null && model.EvenIPIEN2Elab != "" && model.EvenIPIEN2Elab != "0")
                {
                    entity.EvenipiEN2elab = model.EvenIPIEN2Elab;
                }
                entity.EvenipiEN2emitido = model.EvenIPIEN2Emitido;

                //informe
                if (model.EvenInfPIN2FechEmis != null)
                {
                    entity.Eveninfpin2fechemis = DateTime.ParseExact(model.EvenInfPIN2FechEmis,
                        Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                }
                if (model.EvenInfPIN2Elab != null && model.EvenInfPIN2Elab != "" && model.EvenInfPIN2Elab != "0")
                {
                    entity.Eveninfpin2elab = model.EvenInfPIN2Elab;
                }
                entity.Eveninfpin2emitido = model.EvenInfPIN2Emitido;


                //informe final
                //sin informe de empresa
                if (model.EvenIFEN2FechEm != null)
                {
                    entity.EvenifEN2fechem = DateTime.ParseExact(model.EvenIFEN2FechEm, Constantes.FormatoFechaFull,
                        CultureInfo.InvariantCulture);
                }

                if (model.EvenIFEN2Elab != null && model.EvenIFEN2Elab != "" && model.EvenIFEN2Elab != "0")
                {
                    entity.EvenifEN2elab = model.EvenIFEN2Elab;
                }
                entity.EvenifEN2emitido = model.EvenIFEN2Emitido;

                //informe
                if (model.EvenInfFN2FechEmis != null)
                {
                    entity.Eveninffn2fechemis = DateTime.ParseExact(model.EvenInfFN2FechEmis,
                        Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                }

                if (model.EvenInfFN2Elab != null && model.EvenInfFN2Elab != "" && model.EvenInfFN2Elab != "0")
                {
                    entity.Eveninffn2elab = model.EvenInfFN2Elab;
                }
                entity.Eveninffn2emitido = model.EvenInfFN2Emitido;
                
                entity.Eveninfn2lastuser = User.Identity.Name;
                entity.Eveninfn2lastdate = DateTime.Now;

                int id = this.servInformefallaN2.SaveEveInformefallaN2Id(entity);

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
        /// <param name="infEmitido">Informe Emitido (S/N)</param>
        /// <param name="emprCodi">Código de empresa</param>
        /// <param name="equiAbrev">Abrevistura de equipo</param>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <param name="nroPage">Número de página</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string infEmitido, int emprCodi, string equiAbrev,
            string fechaIni, string fechaFin, int nroPage)
        {
            BusquedaEveInformefallaN2Model model = new BusquedaEveInformefallaN2Model();

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

            model.ListaEveInformefallaN2 = servInformefallaN2.BuscarOperaciones(infEmitido, emprCodi, equiAbrev,
                fechaInicio, fechaFinal, nroPage, Constantes.PageSizeEvento).ToList();
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            return PartialView(model);
        }



        /// <summary>
        /// Permite hacer el paginado
        /// </summary>
        /// <param name="infEmitido">Informe Emitido (S/N)</param>
        /// <param name="emprCodi">Código de empresa</param>
        /// <param name="equiAbrev">Abrevistura de equipo</param>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Paginado</returns>
        [HttpPost]
        public PartialViewResult Paginado(string infEmitido, int emprCodi, string equiAbrev,
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


            int nroRegistros = servInformefallaN2.ObtenerNroFilas(infEmitido, emprCodi, equiAbrev,
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
            dto.TipoFalla = "2";
            EventoModel datos = new EventoModel();
            datos.ListaEnvios = this.servEventos.ListaInformeEnviosLog(dto);

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet ws = null;
                ws = package.Workbook.Worksheets.Add("Reporte");
                ws.View.ShowGridLines = true;
                FormatoEventoHelper.AddImage(ws, 0, 0, rutaLogo);

                ws.Cells[2, 3].Value = "Reporte de Envíos de Informes de Falla N2";

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
