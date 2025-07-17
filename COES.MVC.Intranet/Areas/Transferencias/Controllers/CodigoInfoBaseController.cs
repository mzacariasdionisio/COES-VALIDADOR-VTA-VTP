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
    public class CodigoInfoBaseController : Controller
    {
        // GET: /Transferencias/CodigoInfoBase/
        //[CustomAuthorize]
        public ActionResult Index()
        {
            EmpresaModel modelEmp = new EmpresaModel();
            modelEmp.ListaEmpresas = (new EmpresaAppServicio()).ListaInterCodInfoBase();

            CentralGeneracionModel modelCentralGene = new CentralGeneracionModel();
            modelCentralGene.ListaCentralGeneracion = (new CentralGeneracionAppServicio()).ListaInterCodInfoBase();

            BarraModel modelBarr = new BarraModel();
            modelBarr.ListaBarras = (new BarraAppServicio()).ListaInterCodInfoBase();

            TempData["EMPRCODI2"] = new SelectList(modelEmp.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE");

            TempData["CENTGENECODI2"] = new SelectList(modelCentralGene.ListaCentralGeneracion, "CENTGENECODI", "CENTGENENOMBRE");

            TempData["BARRCODI2"] = new SelectList(modelBarr.ListaBarras, "BARRCODI", "BARRNOMBBARRTRAN");

            CodigoInfoBaseModel model = new CodigoInfoBaseModel();
            model.bNuevo = (new Funcion()).ValidarPermisoNuevo(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            return PartialView(model);
        }

        //POST
        [HttpPost]
        public ActionResult Lista(string nombreEmp, string centralGene, string nombreBarra, string fechaInicio, string fechaFin, string estado, string codinfobase, int NroPagina)
        {
            if (nombreEmp.Equals("--Seleccione--") || nombreEmp.Equals(""))
                nombreEmp = null;
            if (centralGene.Equals("--Seleccione--"))
                centralGene = null;
            if (nombreBarra.Equals("--Seleccione--"))
                nombreBarra = null;

            if (String.IsNullOrWhiteSpace(codinfobase))
                codinfobase = null;
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

            CodigoInfoBaseModel model = new CodigoInfoBaseModel();
            model.ListaCodigoInfoBase = (new CodigoInfoBaseAppServicio()).BuscarCodigoInfoBase(nombreEmp, centralGene, nombreBarra, dtfi, dtff, estado, codinfobase, NroPagina, Funcion.PageSizeCodigoInfoBase);
            //TempData["tdListaCodigoInfoBase"] = model.ListaCodigoInfoBase;
            model.bEditar = (new Funcion()).ValidarPermisoEditar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            model.bEliminar = (new Funcion()).ValidarPermisoEliminar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        [HttpPost]
        public PartialViewResult Paginado(string nombreEmp, string centralGene, string nombreBarra, string fechaInicio, string fechaFin, string estado, string codinfobase)
        {
            CodigoInfoBaseModel model = new CodigoInfoBaseModel(); ;
            model.IndicadorPagina = false;

            if (nombreEmp.Equals("--Seleccione--") || nombreEmp.Equals(""))
                nombreEmp = null;
            if (centralGene.Equals("--Seleccione--"))
                centralGene = null;
            if (nombreBarra.Equals("--Seleccione--"))
                nombreBarra = null;

            if (String.IsNullOrWhiteSpace(codinfobase))
                codinfobase = null;
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

            model.NroRegistros = (new CodigoInfoBaseAppServicio()).ObtenerNroFilasCodigoInfoBase(nombreEmp, centralGene, nombreBarra, dtfi, dtff, estado, codinfobase);
            TempData["tdListaCodigoInfoBase"] = (new CodigoInfoBaseAppServicio()).BuscarCodigoInfoBase(nombreEmp, centralGene, nombreBarra, dtfi, dtff, estado, codinfobase, 1, model.NroRegistros);
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

        public ActionResult View(int id = 0)
        {
            CodigoInfoBaseModel model = new CodigoInfoBaseModel();
            model.Entidad = (new CodigoInfoBaseAppServicio()).GetByIdCodigoInfoBase(id);

            return PartialView(model);
        }

        public ActionResult New()
        {
            CodigoInfoBaseModel modelo = new CodigoInfoBaseModel();
            modelo.Entidad = new CodigoInfoBaseDTO();
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }
            modelo.Entidad.CoInfBCodi = 0;
            modelo.Coinfbfechainicio = System.DateTime.Now.ToString("dd/MM/yyyy");
            modelo.Coinfbfechafin = System.DateTime.Now.ToString("dd/MM/yyyy");
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

            CentralGeneracionModel modelCentralGene = new CentralGeneracionModel();
            modelCentralGene.Entidad = new CentralGeneracionDTO();
            modelCentralGene.ListaCentralGeneracion = (new CentralGeneracionAppServicio()).ListCentralGeneracionInfoBase();
            TempData["CENTGENECODI2"] = modelCentralGene;

            EmpresaModel modelEmp = new EmpresaModel();
            modelEmp.Entidad = new EmpresaDTO();
            modelEmp.ListaEmpresas = (new EmpresaAppServicio()).ListEmpresas();
            TempData["EMPRCODI2"] = modelEmp;

            BarraModel modelBarr = new BarraModel();
            modelBarr.Entidad = new BarraDTO();
            modelBarr.ListaBarras = (new BarraAppServicio()).ListaBarraTransferencia();
            TempData["BARRCODI2"] = modelBarr;
            return PartialView(modelo);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(CodigoInfoBaseModel modelo)
        {
            CodigoInfoBaseModel modeloAux = new CodigoInfoBaseModel(); //Consultamos si el codigo ya encuentra registrado
            modelo.Entidad.CoInfBCodigo = (new Funcion()).CorregirCodigo(modelo.Entidad.CoInfBCodigo);
            modeloAux.Entidad = (new CodigoInfoBaseAppServicio()).GetByCodigoInfoBaseCodigo(modelo.Entidad.CoInfBCodigo);
            if (modeloAux.Entidad != null && modelo.Entidad.CoInfBCodi != modeloAux.Entidad.CoInfBCodi)
                modelo.sError = "El Código de Entrega [" + modelo.Entidad.CoInfBCodigo + "], ya se encuentra registrado";
            else if (ModelState.IsValid)
            {   //El modelo es valido
                modelo.Entidad.CoInfBUserName = User.Identity.Name;
                if (modelo.Entidad.CoInfBEstado == null)
                    modelo.Entidad.CoInfBEstado = "ACT";
                if (modelo.Coinfbfechainicio != "" && modelo.Coinfbfechainicio != null)
                    modelo.Entidad.CoInfBFechaInicio = DateTime.ParseExact(modelo.Coinfbfechainicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (modelo.Coinfbfechafin != "" && modelo.Coinfbfechafin != null)
                    modelo.Entidad.CoInfBFechaFin = DateTime.ParseExact(modelo.Coinfbfechafin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                modelo.IdCodigoInfoBase = (new CodigoInfoBaseAppServicio()).SaveOrUpdateCodigoInfoBase(modelo.Entidad);
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return RedirectToAction("Index");
            }
            //Error
            if (modelo.sError == null) modelo.sError = "Se ha producido un error al insertar la información";
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

            CentralGeneracionModel modelCentralGene = new CentralGeneracionModel();
            modelCentralGene.ListaCentralGeneracion = (new CentralGeneracionAppServicio()).ListCentralGeneracionInfoBase();
            modelCentralGene.Entidad = new CentralGeneracionDTO();
            modelCentralGene.Entidad.CentGeneCodi = modelo.Entidad.CentGeneCodi;
            TempData["CENTGENECODI2"] = modelCentralGene;
            EmpresaModel modelEmp = new EmpresaModel();
            modelEmp.ListaEmpresas = (new EmpresaAppServicio()).ListEmpresas();
            modelEmp.Entidad = new EmpresaDTO();
            modelEmp.Entidad.EmprCodi = modelo.Entidad.EmprCodi;
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
            CodigoInfoBaseModel modelo = new CodigoInfoBaseModel();
            modelo.Entidad = (new CodigoInfoBaseAppServicio()).GetByIdCodigoInfoBase(id);
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }
            modelo.Coinfbfechainicio = modelo.Entidad.CoInfBFechaInicio.ToString("dd/MM/yyyy");
            if (modelo.Entidad.CoInfBFechaFin != null)
            { modelo.Coinfbfechafin = modelo.Entidad.CoInfBFechaFin.GetValueOrDefault().ToString("dd/MM/yyyy"); }
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

            CentralGeneracionModel modelCentralGene = new CentralGeneracionModel();
            modelCentralGene.ListaCentralGeneracion = (new CentralGeneracionAppServicio()).ListCentralGeneracionInfoBase();
            modelCentralGene.Entidad = new CentralGeneracionDTO();
            modelCentralGene.Entidad.CentGeneCodi = modelo.Entidad.CentGeneCodi;
            TempData["CENTGENECODI2"] = modelCentralGene;
            EmpresaModel modelEmp = new EmpresaModel();
            modelEmp.ListaEmpresas = (new EmpresaAppServicio()).ListEmpresas();
            modelEmp.Entidad = new EmpresaDTO();
            modelEmp.Entidad.EmprCodi = modelo.Entidad.EmprCodi;
            TempData["EMPRCODI2"] = modelEmp;
            BarraModel modelBarr = new BarraModel();
            modelBarr.ListaBarras = (new BarraAppServicio()).ListaBarraTransferencia();
            modelBarr.Entidad = new BarraDTO();
            modelBarr.Entidad.BarrCodi = modelo.Entidad.BarrCodi;
            TempData["BARRCODI2"] = modelBarr;

            return PartialView(modelo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int id = 0)
        {
            CodigoInfoBaseModel model = new CodigoInfoBaseModel();
            model.IdCodigoInfoBase = (new CodigoInfoBaseAppServicio()).DeleteCodigoInfoBase(id);
            return "true";
        }

        [HttpPost]
        public JsonResult GenerarExcel()
        {
            int indicador = 1;

            try
            {
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();

                CodigoInfoBaseModel model = new CodigoInfoBaseModel();
                if (TempData["tdListaCodigoInfoBase"] != null)
                    model.ListaCodigoInfoBase = (List<CodigoInfoBaseDTO>)TempData["tdListaCodigoInfoBase"];
                else
                    model.ListaCodigoInfoBase = (new CodigoInfoBaseAppServicio()).ListCodigoInfoBase();

                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteCodigoInfoBaseExcel);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteCodigoInfoBaseExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        ws.Cells[2, 3].Value = "LISTA DE CÓDIGOS DE INFORMACIÓN BASE";
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
                        foreach (var item in model.ListaCodigoInfoBase)
                        {
                            ws.Cells[row, 2].Value = (item.CentGeneNombre != null) ? item.CentGeneNombre.ToString() : string.Empty;
                            ws.Cells[row, 3].Value = (item.BarrNombBarrTran != null) ? item.BarrNombBarrTran : string.Empty;
                            ws.Cells[row, 4].Value = (item.EmprNomb != null) ? item.EmprNomb.ToString() : string.Empty;
                            ws.Cells[row, 5].Value = (item.CoInfBFechaInicio != null) ? item.CoInfBFechaInicio.ToString("dd/MM/yyyy") : string.Empty;
                            ws.Cells[row, 6].Value = (item.CoInfBFechaFin != null) ? item.CoInfBFechaFin.Value.ToString("dd/MM/yyyy") : string.Empty;
                            ws.Cells[row, 7].Value = (item.CoInfBCodigo != null) ? item.CoInfBCodigo.ToString() : string.Empty;
                            ws.Cells[row, 8].Value = string.Empty;
                            if (item.CoInfBEstado != null)
                            {
                                if (item.CoInfBEstado.ToString().Equals("ACT")) ws.Cells[row, 8].Value = "Activo";
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
            string path = AppDomain.CurrentDomain.BaseDirectory +Funcion.RutaReporte+ Funcion.NombreReporteCodigoInfoBaseExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteCodigoInfoBaseExcel);
        }        
    }
}
