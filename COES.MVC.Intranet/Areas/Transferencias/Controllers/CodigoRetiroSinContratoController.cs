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
    public class CodigoRetiroSinContratoController : Controller
    {
        // GET: /Transferencias/CodigoRetiroSinContrato/
        //[CustomAuthorize]
        public ActionResult Index()
        {
            EmpresaModel modelEmp = new EmpresaModel();
            modelEmp.ListaEmpresas = (new EmpresaAppServicio()).ListaInterCoReSC();
            BarraModel modelBarr = new BarraModel();
            modelBarr.ListaBarras = (new BarraAppServicio()).ListaInterCoReSC();

            TempData["EMPRCODI2"] = new SelectList(modelEmp.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE");
            TempData["BARRCODI2"] = new SelectList(modelBarr.ListaBarras, "BARRCODI", "BARRNOMBBARRTRAN");

            CodigoRetiroSinContratoModel model = new CodigoRetiroSinContratoModel();
            model.bNuevo = (new Funcion()).ValidarPermisoNuevo(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            return PartialView(model);
        }

        //POST
        [HttpPost]
        public ActionResult Lista(string nombreCli, string nombreBarra, string fechaInicio, string fechaFin, string estado,string  codretirosc , int NroPagina)
        {
            if (nombreCli.Equals("--Seleccione--"))
                nombreCli = null;
            if (nombreBarra.Equals("--Seleccione--"))
                nombreBarra = null;
            if (String.IsNullOrWhiteSpace(codretirosc))
                codretirosc = null;
            if (String.IsNullOrWhiteSpace(estado) || estado.Equals("TODOS"))
                estado = null;
           
            DateTime? dtfi = null;
            if (string.IsNullOrEmpty(fechaInicio))
            {
                dtfi = null;
            }
            else
            {
                dtfi = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture); //Convert.ToDateTime(fechaInicio);
            }

            DateTime? dtff = null;
            if (string.IsNullOrEmpty(fechaFin))
            {
                dtff = null;
            }
            else
            {
                dtff = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);  //Convert.ToDateTime(fechaFin);
            }

            CodigoRetiroSinContratoModel model = new CodigoRetiroSinContratoModel();
            model.ListaCodigoRetiroSinContrato = (new CodigoRetiroSinContratoAppServicio()).BuscarCodigoRetiroSinContrato(nombreCli, nombreBarra, dtfi, dtff, estado, codretirosc, NroPagina, Funcion.PageSizeCodigoRetiroSC);
            //TempData["tdListaCodigoRetiroSinContrato"] = model.ListaCodigoRetiroSinContrato;
            model.bEditar = (new Funcion()).ValidarPermisoEditar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            model.bEliminar = (new Funcion()).ValidarPermisoEliminar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        [HttpPost]
        public PartialViewResult Paginado(string nombreCli, string nombreBarra, string fechaInicio, string fechaFin, string estado, string codretirosc)
        {
            CodigoRetiroSinContratoModel model = new CodigoRetiroSinContratoModel();
            model.IndicadorPagina = false;

            if (nombreCli.Equals("--Seleccione--"))
                nombreCli = null;
            if (nombreBarra.Equals("--Seleccione--"))
                nombreBarra = null;
            if (String.IsNullOrWhiteSpace(codretirosc))
                codretirosc = null;
            if (String.IsNullOrWhiteSpace(estado) || estado.Equals("TODOS"))
                estado = null;

            DateTime? dtfi = null;
            if (string.IsNullOrEmpty(fechaInicio))
            {
                dtfi = null;
            }
            else
            {
                dtfi = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture); //Convert.ToDateTime(fechaInicio);
            }

            DateTime? dtff = null;
            if (string.IsNullOrEmpty(fechaFin))
            {
                dtfi = null;
            }
            else
            {
                dtfi = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);  //Convert.ToDateTime(fechaFin);
            }

            model.NroRegistros = (new CodigoRetiroSinContratoAppServicio()).ObtenerNroFilasCodigoRetiroSinContrato(nombreCli, nombreBarra, dtfi, dtff, estado,  codretirosc);
            TempData["tdListaCodigoRetiroSinContrato"] = (new CodigoRetiroSinContratoAppServicio()).BuscarCodigoRetiroSinContrato(nombreCli, nombreBarra, dtfi, dtff, estado, codretirosc, 1, model.NroRegistros);
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

        //GET
        public ActionResult View(int id = 0)
        {
            CodigoRetiroSinContratoModel model = new CodigoRetiroSinContratoModel();
            model.Entidad = (new CodigoRetiroSinContratoAppServicio()).GetByIdCodigoRetiroSinContrato(id);
            return PartialView(model);
        }

        //GET
        public ActionResult New()
        {
            CodigoRetiroSinContratoModel modelo = new CodigoRetiroSinContratoModel();
            modelo.Entidad = new CodigoRetiroSinContratoDTO();
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }
            modelo.Entidad.CodRetiSinConCodi = 0;
            modelo.Codretisinconfechainicio = System.DateTime.Now.ToString("dd/MM/yyyy");
            modelo.Codretisinconfechafin = System.DateTime.Now.ToString("dd/MM/yyyy");
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

            EmpresaModel modelEmp = new EmpresaModel();
            modelEmp.Entidad = new EmpresaDTO();
            modelEmp.ListaEmpresas = (new EmpresaAppServicio()).ListEmpresas();
            TempData["EMPRCODI2"] = modelEmp;

            BarraModel modelBarr = new BarraModel();
            modelBarr.Entidad = new BarraDTO();
            modelBarr.ListaBarras = (new BarraAppServicio()).ListaBarraTransferencia();
            TempData["BARRCODI2"] = modelBarr;

            modelo.Entidad.GenEmprCodi = Funcion.CODIEMPR_SINCONTRATO;
            return PartialView(modelo); 
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(CodigoRetiroSinContratoModel modelo)
        {
            CodigoRetiroSinContratoModel modeloAux = new CodigoRetiroSinContratoModel(); //Consultamos si el codigo ya encuentra registrado
            modelo.Entidad.CodRetiSinConCodigo = (new Funcion()).CorregirCodigo(modelo.Entidad.CodRetiSinConCodigo);
            modeloAux.Entidad = (new CodigoRetiroSinContratoAppServicio()).BuscarCodigoRetiroSinContratoCodigo(modelo.Entidad.CodRetiSinConCodigo);
            if (modeloAux.Entidad != null && modelo.Entidad.CodRetiSinConCodi != modeloAux.Entidad.CodRetiSinConCodi)
                modelo.sError = "El Código de retiro sin contrato [" + modelo.Entidad.CodRetiSinConCodigo + "], ya se encuentra registrado";
            else if (ModelState.IsValid)
            {
                modelo.Entidad.CodRetiSinConUserName = User.Identity.Name;
                if (modelo.Codretisinconfechainicio != "" && modelo.Codretisinconfechainicio != null)
                    modelo.Entidad.CodRetiSinConFechaInicio = DateTime.ParseExact(modelo.Codretisinconfechainicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (modelo.Codretisinconfechafin != "" && modelo.Codretisinconfechafin != null)
                    modelo.Entidad.CodRetiSinConFechaFin = DateTime.ParseExact(modelo.Codretisinconfechafin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                modelo.IdCodigoRetirosinContrato = (new CodigoRetiroSinContratoAppServicio()).SaveOrUpdateCodigoRetiroSinContrato(modelo.Entidad);
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return RedirectToAction("Index");
            }
            //Error
            if (modelo.sError == null) modelo.sError = "Se ha producido un error al insertar la información";
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            EmpresaModel modelEmp = new EmpresaModel();
            modelEmp.ListaEmpresas = (new EmpresaAppServicio()).ListEmpresas();
            modelEmp.Entidad = new EmpresaDTO();
            modelEmp.Entidad.EmprCodi = modelo.Entidad.CliCodi;
            TempData["EMPRCODI2"] = modelEmp;

            BarraModel modelBarr = new BarraModel();
            modelBarr.ListaBarras = (new BarraAppServicio()).ListaBarraTransferencia();
            modelBarr.Entidad = new BarraDTO();
            modelBarr.Entidad.BarrCodi = modelo.Entidad.BarrCodi;
            TempData["BARRCODI2"] = modelBarr;
            return PartialView(modelo); 
        }

        //GET
        public ActionResult Edit(int id = 0)
        {
            CodigoRetiroSinContratoModel modelo = new CodigoRetiroSinContratoModel();
            modelo.Entidad = (new CodigoRetiroSinContratoAppServicio()).GetByIdCodigoRetiroSinContrato(id);
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }
            modelo.Codretisinconfechainicio = modelo.Entidad.CodRetiSinConFechaInicio.ToString("dd/MM/yyyy");
            if (modelo.Entidad.CodRetiSinConFechaFin != null)
            { modelo.Codretisinconfechafin = modelo.Entidad.CodRetiSinConFechaFin.GetValueOrDefault().ToString("dd/MM/yyyy"); }
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

            EmpresaModel modelEmp = new EmpresaModel();
            modelEmp.ListaEmpresas = (new EmpresaAppServicio()).ListEmpresas();
            modelEmp.Entidad = new EmpresaDTO();
            modelEmp.Entidad.EmprCodi = modelo.Entidad.CliCodi;
            TempData["EMPRCODI2"] = modelEmp;

            BarraModel modelBarr = new BarraModel();
            modelBarr.ListaBarras = (new BarraAppServicio()).ListaBarraTransferencia();
            modelBarr.Entidad = new BarraDTO();
            modelBarr.Entidad.BarrCodi = modelo.Entidad.BarrCodi;
            TempData["BARRCODI2"] = modelBarr;

            
            return PartialView(modelo); 
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int id)
        {
            CodigoRetiroSinContratoModel model = new CodigoRetiroSinContratoModel();
            model.IdCodigoRetirosinContrato = (new CodigoRetiroSinContratoAppServicio()).DeleteCodigoRetiroSinContrato(id);
            return "True";
        }

        //ASSETEC 20191116
        [HttpPost]
        public JsonResult GenerarExcel()
        {
            int indicador = 1;
            try
            {
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();

                CodigoRetiroSinContratoModel model = new CodigoRetiroSinContratoModel();
                if (TempData["tdListaCodigoRetiroSinContrato"] != null)
                    model.ListaCodigoRetiroSinContrato = (List<CodigoRetiroSinContratoDTO>)TempData["tdListaCodigoRetiroSinContrato"];
                else
                    model.ListaCodigoRetiroSinContrato = (new CodigoRetiroSinContratoAppServicio()).ListCodigoRetiroSinContrato();

                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteCodigoRetiroSinContratoExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteCodigoRetiroSinContratoExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        ws.Cells[2, 3].Value = "LISTA DE CÓDIGOS DE RETIRO NO CUBIERTO EN EL MCP";
                        ExcelRange rg = ws.Cells[2, 3, 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "BARRA TRANSFERENCIA";
                        ws.Cells[5, 3].Value = "CLIENTE";
                        ws.Cells[5, 4].Value = "RUC CLIENTE";
                        ws.Cells[5, 5].Value = "INICIO OPERACIÓN";
                        ws.Cells[5, 6].Value = "FIN OPERACIÓN";
                        ws.Cells[5, 7].Value = "CODIGO RETIRO SIN CONTRATO";
                        ws.Cells[5, 8].Value = "ESTADO";

                        rg = ws.Cells[5, 2, 5, 8];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int row = 6;
                        foreach (var item in model.ListaCodigoRetiroSinContrato)
                        {
                            ws.Cells[row, 2].Value = (item.BarrNombBarrTran != null) ? item.BarrNombBarrTran.ToString() : string.Empty;
                            ws.Cells[row, 3].Value = (item.CliNombre != null) ? item.CliNombre : string.Empty;
                            ws.Cells[row, 4].Value = (item.CliRuc != null) ? item.CliRuc.ToString() : string.Empty;
                            ws.Cells[row, 5].Value = (item.CodRetiSinConFechaInicio != null) ? item.CodRetiSinConFechaInicio.ToString("dd/MM/yyyy") : string.Empty;
                            ws.Cells[row, 6].Value = (item.CodRetiSinConFechaFin != null) ? item.CodRetiSinConFechaFin.Value.ToString("dd/MM/yyyy") : string.Empty;
                            ws.Cells[row, 7].Value = (item.CodRetiSinConCodigo != null) ? item.CodRetiSinConCodigo.ToString() : string.Empty;
                            ws.Cells[row, 8].Value = string.Empty;
                            if (item.CodRetiSinConEstado != null)
                            {
                                if (item.CodRetiSinConEstado.ToString().Equals("ACT")) ws.Cells[row, 8].Value = "Activo";
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
        [HttpGet]
        public virtual ActionResult AbrirExcel()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteCodigoRetiroSinContratoExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteCodigoRetiroSinContratoExcel);
        }  
    }
}
