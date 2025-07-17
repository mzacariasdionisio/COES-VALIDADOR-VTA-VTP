using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
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

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class CodigoEntregaController : Controller
    {
        // GET: /Transferencias/CodigoEntrega/
        //[CustomAuthorize]

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        CodigoEntregaAppServicio servicioCodigoEntrega = new CodigoEntregaAppServicio();
        BarraAppServicio servicioBarra = new BarraAppServicio();
        CentralGeneracionAppServicio servicioCentralGeneracion = new CentralGeneracionAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();

        /// <summary>
        /// Muestra la pantalla inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            EmpresaModel modelEmp = new EmpresaModel();
            modelEmp.ListaEmpresas = this.servicioEmpresa.ListaInterCodEnt();

            CentralGeneracionModel modelCentralGene = new CentralGeneracionModel();
            modelCentralGene.ListaCentralGeneracion = this.servicioCentralGeneracion.ListaInterCodEnt();
       
            BarraModel modelBarr = new BarraModel();
            modelBarr.ListaBarras = this.servicioBarra.ListaInterCodEnt();

            TempData["EMPRCODI3"] = new SelectList(modelEmp.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE");

            TempData["CENTGENECODI3"] = new SelectList(modelCentralGene.ListaCentralGeneracion, "CENTGENECODI", "CENTGENENOMBRE");

            TempData["BARRCODI3"] = new SelectList(modelBarr.ListaBarras, "BARRCODI", "BARRNOMBBARRTRAN");

            CodigoEntregaModel model = new CodigoEntregaModel();
            model.bNuevo = (new Funcion()).ValidarPermisoNuevo(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            return View(model);
        }

        /// <summary>
        /// Muestra la lista de datos de la Barra
        /// </summary>
        /// <param name="nombreEmp">Nombre Empresa</param>
        /// <param name="centralGene">Central Generación</param>
        /// <param name="nombreBarra">Nombre Barra</param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="estado"></param>
        /// <param name="codentrega"></param>
        /// <param name="NroPagina"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Lista(string nombreEmp, string centralGene, string nombreBarra, string fechaInicio, string fechaFin, string estado, string codentrega, int NroPagina)
        {
            if (nombreEmp.Equals("--Seleccione--") || nombreEmp.Equals(""))
                nombreEmp = null;
            if (centralGene.Equals("--Seleccione--"))
                centralGene = null;
            if (nombreBarra.Equals("--Seleccione--"))
                nombreBarra = null;
            if (String.IsNullOrWhiteSpace(codentrega))
                codentrega = null;
            if (String.IsNullOrWhiteSpace(estado) || estado.Equals("TODOS"))
                estado = null;

            DateTime? dtfi = null;
            if (string.IsNullOrEmpty(fechaInicio))
            {
                dtfi = null;
            }
            else
            {
                dtfi = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);  //Convert.ToDateTime(fechaInicio);
            }

            DateTime? dtff = null;
            if (string.IsNullOrEmpty(fechaFin))
            {
                dtff = null;
            }
            else
            {
                dtff = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture); //Convert.ToDateTime(fechaFin);
            }
      
            CodigoEntregaModel model = new CodigoEntregaModel();
            model.ListaCodigoEntrega = this.servicioCodigoEntrega.BuscarCodigoEntrega(nombreEmp, centralGene, nombreBarra, dtfi, dtff, estado, codentrega, NroPagina, Funcion.PageSizeCodigoEntrega);
            //TempData["tdListaCodigoEntrega"] = model.ListaCodigoEntrega;
            model.bEditar = (new Funcion()).ValidarPermisoEditar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            model.bEliminar = (new Funcion()).ValidarPermisoEliminar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            return PartialView(model);
        }

        /// <summary>
        /// Permite pintar la vista del paginado
        /// </summary>
        [HttpPost]
        public PartialViewResult Paginado(string nombreEmp, string centralGene, string nombreBarra, string fechaInicio, string fechaFin, string estado,string codentrega)
        {
            CodigoEntregaModel model = new CodigoEntregaModel();
            model.IndicadorPagina = false;

            if (nombreEmp.Equals("--Seleccione--") || nombreEmp.Equals(""))
                nombreEmp = null;
            if (centralGene.Equals("--Seleccione--"))
                centralGene = null;
            if (nombreBarra.Equals("--Seleccione--"))
                nombreBarra = null;
            if (String.IsNullOrWhiteSpace(codentrega))
                codentrega = null;
            if (String.IsNullOrWhiteSpace(estado) || estado.Equals("TODOS"))
                estado = null;
            
            DateTime? dtfi = null;
            if (string.IsNullOrEmpty(fechaInicio))
            {
                dtfi = null;
            }
            else
            {
                dtfi = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);  //Convert.ToDateTime(fechaInicio);
            }

            DateTime? dtff = null;
            if (string.IsNullOrEmpty(fechaFin))
            {
                dtff = null;
            }
            else
            {
                dtff = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture); //Convert.ToDateTime(fechaFin);
            }

            model.NroRegistros = this.servicioCodigoEntrega.ObtenerNroFilasCodigoEntrega(nombreEmp, centralGene, nombreBarra, dtfi, dtff, estado, codentrega);
            TempData["tdListaCodigoEntrega"] = this.servicioCodigoEntrega.BuscarCodigoEntrega(nombreEmp, centralGene, nombreBarra, dtfi, dtff, estado, codentrega, 1, model.NroRegistros);
            
            if (model.NroRegistros > Funcion.NroPageShow)
            {
                int pageSize = Funcion.PageSizeCodigoEntrega;
                int nroPaginas = (model.NroRegistros % pageSize == 0) ? model.NroRegistros / pageSize : model.NroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Funcion.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Muestra un registro 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult View(int id = 0)
        {
            CodigoEntregaModel model = new CodigoEntregaModel();
            model.Entidad = this.servicioCodigoEntrega.GetByIdCodigoEntra(id);

            return PartialView(model);
        }

        /// <summary>
        /// Prepara una vista para ingresar un nuevo registro
        /// </summary>
        /// <returns></returns>
        public ActionResult New()
        {
            CodigoEntregaModel modelo = new CodigoEntregaModel();
            modelo.Entidad = new CodigoEntregaDTO();
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }
            modelo.Entidad.CodiEntrCodi = 0;
            modelo.Codientrfechainicio = System.DateTime.Now.ToString("dd/MM/yyyy");
            modelo.Codientrfechafin = System.DateTime.Now.ToString("dd/MM/yyyy");
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

            CentralGeneracionModel modelCentralGene = new CentralGeneracionModel();
            modelCentralGene.Entidad = new CentralGeneracionDTO();
            modelCentralGene.ListaCentralGeneracion = this.servicioCentralGeneracion.ListCentralGeneracion();
            TempData["CENTGENECODI2"] = modelCentralGene;

            EmpresaModel modelEmp = new EmpresaModel();
            modelEmp.Entidad = new EmpresaDTO();
            modelEmp.ListaEmpresas = this.servicioEmpresa.ListEmpresas();
            TempData["EMPRCODI2"] = modelEmp;

            BarraModel modelBarr = new BarraModel();
            modelBarr.Entidad = new BarraDTO();
            modelBarr.ListaBarras = this.servicioBarra.ListaBarraTransferencia();
            TempData["BARRCODI2"] = modelBarr;
            return PartialView(modelo); 
        }

        /// <summary>
        /// Prepara una vista para editar un nuevo registro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            CodigoEntregaModel modelo = new CodigoEntregaModel();
            modelo.Entidad = this.servicioCodigoEntrega.GetByIdCodigoEntra(id);
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }
            modelo.Codientrfechainicio = modelo.Entidad.CodiEntrFechaInicio.ToString("dd/MM/yyyy");
            if (modelo.Entidad.CodiEntrFechaFin != null)
            { modelo.Codientrfechafin = modelo.Entidad.CodiEntrFechaFin.GetValueOrDefault().ToString("dd/MM/yyyy"); }
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

            CentralGeneracionModel modelCentralGene = new CentralGeneracionModel();
            modelCentralGene.ListaCentralGeneracion = this.servicioCentralGeneracion.ListCentralGeneracion();
            modelCentralGene.Entidad = new CentralGeneracionDTO();
            modelCentralGene.Entidad.CentGeneCodi = modelo.Entidad.CentGeneCodi;
            TempData["CENTGENECODI2"] = modelCentralGene;

            EmpresaModel modelEmp = new EmpresaModel();
            modelEmp.ListaEmpresas = this.servicioEmpresa.ListEmpresas();
            modelEmp.Entidad = new EmpresaDTO();
            modelEmp.Entidad.EmprCodi = modelo.Entidad.EmprCodi;
            TempData["EMPRCODI2"] = modelEmp;

            BarraModel modelBarr = new BarraModel();
            modelBarr.ListaBarras = this.servicioBarra.ListaBarraTransferencia();
            modelBarr.Entidad = new BarraDTO();
            modelBarr.Entidad.BarrCodi = modelo.Entidad.BarrCodi;
            TempData["BARRCODI2"] = modelBarr;
            return PartialView(modelo);
        }

        /// <summary>
        /// Permite grabar los datos del formulario
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(CodigoEntregaModel modelo)
        {
            CodigoEntregaModel modeloAux = new CodigoEntregaModel(); //Consultamos si el codigo ya encuentra registrado
            modelo.Entidad.CodiEntrCodigo = (new Funcion()).CorregirCodigo(modelo.Entidad.CodiEntrCodigo);
            modeloAux.Entidad = this.servicioCodigoEntrega.GetByCodigoEntregaCodigo(modelo.Entidad.CodiEntrCodigo);
            if (modeloAux.Entidad != null && modelo.Entidad.CodiEntrCodi != modeloAux.Entidad.CodiEntrCodi)
                modelo.sError = "El Código de Entrega [" + modelo.Entidad.CodiEntrCodigo + "], ya se encuentra registrado";
            else if (ModelState.IsValid)
            {   //El modelo es valido
                modelo.Entidad.CodiEntrUserName = User.Identity.Name;

                if (modelo.Entidad.CodiEntrEstado == null)
                    modelo.Entidad.CodiEntrEstado = "ACT";
                if (modelo.Codientrfechainicio != "" && modelo.Codientrfechainicio != null)
                    modelo.Entidad.CodiEntrFechaInicio = DateTime.ParseExact(modelo.Codientrfechainicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (modelo.Codientrfechafin != "" && modelo.Codientrfechafin != null)
                    modelo.Entidad.CodiEntrFechaFin = DateTime.ParseExact(modelo.Codientrfechafin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                modelo.IdCodigoEntrega = this.servicioCodigoEntrega.SaveOrUpdateCodigoEntrega(modelo.Entidad);
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return RedirectToAction("Index");
            }
            //Error
            if (modelo.sError == null) modelo.sError = "Se ha producido un error al insertar la información";
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            
            CentralGeneracionModel modelCentralGene = new CentralGeneracionModel();
            modelCentralGene.ListaCentralGeneracion = this.servicioCentralGeneracion.ListCentralGeneracion();
            modelCentralGene.Entidad = new CentralGeneracionDTO();
            modelCentralGene.Entidad.CentGeneCodi = modelo.Entidad.CentGeneCodi;
            TempData["CENTGENECODI2"] = modelCentralGene; //new SelectList(modelCentralGene.ListaCentralGeneracion, "CENTGENECODI", "CENTGENENOMBRE", modelo.Entidad.CentGeneCodi);
            
            EmpresaModel modelEmp = new EmpresaModel();
            modelEmp.ListaEmpresas = this.servicioEmpresa.ListEmpresas();
            modelEmp.Entidad = new EmpresaDTO();
            modelEmp.Entidad.EmprCodi = modelo.Entidad.EmprCodi;
            TempData["EMPRCODI2"] = modelEmp; // new SelectList(modelEmp.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE", modelo.Entidad.EmprCodi);
            
            BarraModel modelBarr = new BarraModel();
            modelBarr.ListaBarras = this.servicioBarra.ListaBarraTransferencia();
            modelBarr.Entidad = new BarraDTO();
            modelBarr.Entidad.BarrCodi = modelo.Entidad.BarrCodi;
            TempData["BARRCODI2"] = modelBarr; //new SelectList(modelBarr.ListaBarras, "BARRCODI", "BARRNOMBBARRTRAN", modelo.Entidad.BarrCodi);
            return PartialView(modelo); 
        }

        /// <summary>
        /// Permite eliminar un registro de la db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int id = 0)
        {
            CodigoEntregaModel model = new CodigoEntregaModel();
            model.IdCodigoEntrega = this.servicioCodigoEntrega.DeleteCodigoEntrega(id);
            return "true";
        }

        /// <summary>
        /// Permite exportar un archivo excel de todos los registros
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarExcel()
        {
            int indicador = 1;
            try
            {
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();

                CodigoEntregaModel model = new CodigoEntregaModel();
                if (TempData["tdListaCodigoEntrega"] != null)
                    model.ListaCodigoEntrega = (List<CodigoEntregaDTO>)TempData["tdListaCodigoEntrega"];
                else
                    model.ListaCodigoEntrega = this.servicioCodigoEntrega.ListCodigoEntrega();

                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteCodigoEntregaExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteCodigoEntregaExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        ws.Cells[2, 3].Value = "LISTA DE CÓDIGOS DE ENTREGA";
                        ExcelRange rg = ws.Cells[2, 3, 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "CENTRAL GENERACION";
                        ws.Cells[5, 3].Value = "BARRA TRANSFERENCIA";
                        ws.Cells[5, 4].Value = "EMPRESA";
                        ws.Cells[5, 5].Value = "INICIO OPERACIÓN";
                        ws.Cells[5, 6].Value = "FIN OPERACIÓN";
                        ws.Cells[5, 7].Value = "CÓDIGO ENTREGA";
                        ws.Cells[5, 8].Value = "ESTADO";

                        rg = ws.Cells[5, 2, 5, 8];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int row = 6;
                        foreach (var item in model.ListaCodigoEntrega)
                        {
                            ws.Cells[row, 2].Value = (item.CentGeneNombre != null) ? item.CentGeneNombre.ToString() : string.Empty;
                            ws.Cells[row, 3].Value = (item.BarrNombBarrTran != null) ? item.BarrNombBarrTran : string.Empty;
                            ws.Cells[row, 4].Value = (item.EmprNomb != null) ? item.EmprNomb.ToString() : string.Empty;
                            ws.Cells[row, 5].Value = (item.CodiEntrFechaInicio != null) ? item.CodiEntrFechaInicio.ToString("dd/MM/yyyy") : string.Empty;
                            ws.Cells[row, 6].Value = (item.CodiEntrFechaFin != null) ? item.CodiEntrFechaFin.Value.ToString("dd/MM/yyyy") : string.Empty;
                            ws.Cells[row, 7].Value = (item.CodiEntrCodigo != null) ? item.CodiEntrCodigo.ToString() : string.Empty;
                            ws.Cells[row, 8].Value = string.Empty;
                            if (item.CodiEntrEstado != null)
                            {
                                if (item.CodiEntrEstado.ToString().Equals("ACT")) ws.Cells[row, 8].Value = "Activo";
                                else ws.Cells[row, 8].Value = "Inactivo";
                            }
                            //Border por celda
                            rg = ws.Cells[row, 2, row, 8];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;
                            row++;
                        }

                        //Fijar panel
                        ws.View.FreezePanes(6, 9);
                        rg = ws.Cells[5, 2, row, 8];
                        rg.AutoFitColumns();

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }
                    xlPackage.Save();
                }
                indicador = 1;
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Descarga el archivo excel
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult AbrirExcel()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteCodigoEntregaExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteCodigoEntregaExcel);
        }        
    }
}
