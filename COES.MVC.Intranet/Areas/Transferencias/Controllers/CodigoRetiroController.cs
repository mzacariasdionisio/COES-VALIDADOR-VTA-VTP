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
    public class CodigoRetiroController : Controller
    {
        // GET: /Transferencias/CodigoRetiro/
        //[CustomAuthorize]
        public ActionResult Index()
        {
            EmpresaModel modelEmpGen = new EmpresaModel();
            modelEmpGen.ListaEmpresas = (new EmpresaAppServicio()).ListaInterCoReSoGen();
            EmpresaModel modelEmpCli = new EmpresaModel();
            modelEmpCli.ListaEmpresas = (new EmpresaAppServicio()).ListaInterCoReSoCli();
            BarraModel modelBarr = new BarraModel();
            modelBarr.ListaBarras = (new BarraAppServicio()).ListaInterCoReSo();
            TipoContratoModel modelTipoCont = new TipoContratoModel();
            modelTipoCont.ListaTipoContrato = (new TipoContratoAppServicio()).ListTipoContrato();
            TipoUsuarioModel modelTipoUsu = new TipoUsuarioModel();
            modelTipoUsu.ListaTipoUsuario = (new TipoUsuarioAppServicio()).ListTipoUsuario();

            TempData["EMPRCODI2"] = new SelectList(modelEmpGen.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE");
            TempData["CLICODI2"] = new SelectList(modelEmpCli.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE");
            TempData["BARRCODI2"] = new SelectList(modelBarr.ListaBarras, "BARRCODI", "BARRNOMBBARRTRAN");
            TempData["TIPOCONTCODI2"] = new SelectList(modelTipoCont.ListaTipoContrato, "TIPOCONTCODI", "TIPOCONTNOMBRE");
            TempData["TIPOUSUACODI2"] = new SelectList(modelTipoUsu.ListaTipoUsuario, "TIPOUSUACODI", "TIPOUSUANOMBRE");

            CodigoRetiroModel model = new CodigoRetiroModel();
            model.bNuevo = (new Funcion()).ValidarPermisoNuevo(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            return PartialView(model);
        }

        public ActionResult View(int id = 0)
        {
            CodigoRetiroModel model = new CodigoRetiroModel();
            model.Entidad = (new CodigoRetiroAppServicio()).GetByIdCodigoRetiro(id);

            return PartialView(model);
        }

        //POST
        [HttpPost]
        public ActionResult Lista(string nombreEmp, string tipousu, string tipocont, string barr, string clinomb, string fechaInicio, string fechaFin, string Solicodiretiobservacion, string radiobtn, string codretiro, int NroPagina)
        {
            string estado ="";

            if (nombreEmp.Equals("--Seleccione--") || nombreEmp.Equals(""))
                nombreEmp = null;
            if (tipousu.Equals("--Seleccione--"))
                tipousu = null;
            if (tipocont.Equals("--Seleccione--"))
                tipocont = null;
            if (barr.Equals("--Seleccione--"))
                barr = null;
            if (clinomb.Equals("--Seleccione--"))
                clinomb = null;
            //if (codretiro.Equals(""))
            //    codretiro = null;

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
                dtff = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture); //Convert.ToDateTime(fechaFin);
            }

            if (radiobtn != null)
            {
                if (radiobtn.Equals("TODOS")) estado = null;
                else if (radiobtn.Equals("CON")) estado = "ASI";
                else if (radiobtn.Equals("SIN")) estado = "GEN";
            }
            
            CodigoRetiroModel model = new CodigoRetiroModel();
            model.ListaCodigoRetiro = (new CodigoRetiroAppServicio()).BuscarCodigoRetiro(nombreEmp, tipousu, tipocont, barr, clinomb, dtfi, dtff, Solicodiretiobservacion, estado,codretiro, NroPagina, Funcion.PageSizeCodigoRetiro);
            foreach (var x in model.ListaCodigoRetiro)
            {
                if (x.SoliCodiRetiCodigo == null)
                    x.SoliCodiRetiCodigo = "Sin asignar";
            }
            //TempData["tdListaCodigoRetiro"] = model.ListaCodigoRetiro;
            model.bEditar = (new Funcion()).ValidarPermisoEditar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            model.bEliminar = (new Funcion()).ValidarPermisoEliminar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        [HttpPost]
        public PartialViewResult Paginado(string nombreEmp, string tipousu, string tipocont, string barr, string clinomb, string fechaInicio, string fechaFin, string Solicodiretiobservacion, string radiobtn,string codretiro)
        {
            CodigoRetiroModel model = new CodigoRetiroModel();
            model.IndicadorPagina = false;

            string estado = "";

            if (nombreEmp.Equals("--Seleccione--") || nombreEmp.Equals(""))
                nombreEmp = null;
            if (tipousu.Equals("--Seleccione--"))
                tipousu = null;
            if (tipocont.Equals("--Seleccione--"))
                tipocont = null;
            if (barr.Equals("--Seleccione--"))
                barr = null;
            if (clinomb.Equals("--Seleccione--"))
                clinomb = null;
            //if (codretiro.Equals(""))
            //    codretiro = null;


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
                dtff = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture); //Convert.ToDateTime(fechaFin);
            }

            if (radiobtn != null)
            {
                if (radiobtn.Equals("TODOS")) estado = null;
                else if (radiobtn.Equals("CON")) estado = "ASI";
                else if (radiobtn.Equals("SIN")) estado = "GEN";
            }

            model.NroRegistros = (new CodigoRetiroAppServicio()).ObtenerNroFilasCodigoRetiro(nombreEmp, tipousu, tipocont, barr, clinomb, dtfi, dtff, Solicodiretiobservacion, estado, codretiro);
            TempData["tdListaCodigoRetiro"] = (new CodigoRetiroAppServicio()).BuscarCodigoRetiro(nombreEmp, tipousu, tipocont, barr, clinomb, dtfi, dtff, Solicodiretiobservacion, estado, codretiro, 1, model.NroRegistros);
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

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(CodigoRetiroModel modelo)
        {
            CodigoRetiroModel modeloAux = new CodigoRetiroModel(); //Consultamos si el codigo ya encuentra registrado
            modelo.Entidad.SoliCodiRetiCodigo = (new Funcion()).CorregirCodigo(modelo.Entidad.SoliCodiRetiCodigo);
            modeloAux.Entidad = (new CodigoRetiroAppServicio()).GetCodigoRetiroByCodigo(modelo.Entidad.SoliCodiRetiCodigo);
            if (modeloAux.Entidad != null && modeloAux.Entidad.SoliCodiRetiCodi != 0 && modelo.Entidad.SoliCodiRetiCodi != modeloAux.Entidad.SoliCodiRetiCodi)
                modelo.sError = "El Código de Entrega [" + modelo.Entidad.SoliCodiRetiCodigo + "], ya se encuentra registrado";
            else if (ModelState.IsValid)
            {
                modelo.Entidad.CoesUserName = User.Identity.Name;
                modelo.Entidad.SoliCodiRetiEstado = "ASI"; //Codigo Asignado
                if (modelo.Solicodiretifechafin != "" && modelo.Solicodiretifechafin != null)
                    modelo.Entidad.SoliCodiRetiFechaFin = DateTime.ParseExact(modelo.Solicodiretifechafin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                modelo.IdcodRetiro = (new CodigoRetiroAppServicio()).SaveOrUpdateCodigoRetiro(modelo.Entidad);
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return RedirectToAction("Index");
            }
            if (modelo.sError == null) modelo.sError = "Se ha producido un error al insertar la información";
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            EmpresaModel modelCliente = new EmpresaModel();
            modelCliente.ListaEmpresas = (new EmpresaAppServicio()).ListEmpresas();
            TempData["CLICODI2"] = new SelectList(modelCliente.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE", modelo.Entidad.CliCodi);
            BarraModel modelBarr = new BarraModel();
            modelBarr.ListaBarras = (new BarraAppServicio()).ListaBarraTransferencia();
            TempData["BARRCODI2"] = new SelectList(modelBarr.ListaBarras, "BARRCODI", "BARRNOMBBARRTRAN", modelo.Entidad.BarrCodi);
            TipoContratoModel modelTipoCont = new TipoContratoModel();
            modelTipoCont.ListaTipoContrato = (new TipoContratoAppServicio()).ListTipoContrato();
            TempData["TIPOCONTCODI2"] = new SelectList(modelTipoCont.ListaTipoContrato, "TIPOCONTCODI", "TIPOCONTNOMBRE", modelo.Entidad.TipoContCodi);
            TipoUsuarioModel modelTipoUsu = new TipoUsuarioModel();
            modelTipoUsu.ListaTipoUsuario = (new TipoUsuarioAppServicio()).ListTipoUsuario();
            TempData["TIPOUSUACODI2"] = new SelectList(modelTipoUsu.ListaTipoUsuario, "TIPOUSUACODI", "TIPOUSUANOMBRE", modelo.Entidad.TipoUsuaCodi);

            return PartialView(modelo); 
        }

        //GET
        public ActionResult Edit(int id = 0)
        {
            CodigoRetiroModel modelo = new CodigoRetiroModel();
            modelo.Entidad = (new CodigoRetiroAppServicio()).GetByIdCodigoRetiro(id);
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            if (modelo.Entidad.SoliCodiRetiFechaInicio != null)
            { modelo.Solicodiretifechainicio = modelo.Entidad.SoliCodiRetiFechaInicio.GetValueOrDefault().ToString("dd/MM/yyyy"); }
            if (modelo.Entidad.SoliCodiRetiFechaFin != null)
            { modelo.Solicodiretifechafin = modelo.Entidad.SoliCodiRetiFechaFin.GetValueOrDefault().ToString("dd/MM/yyyy"); }
            EmpresaModel modelCliente = new EmpresaModel();
            modelCliente.ListaEmpresas = (new EmpresaAppServicio()).ListEmpresas();
            TempData["CLICODI2"] = new SelectList(modelCliente.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE", modelo.Entidad.CliCodi);
            BarraModel modelBarr = new BarraModel();
            modelBarr.ListaBarras = (new BarraAppServicio()).ListaBarraTransferencia();
            TempData["BARRCODI2"] = new SelectList(modelBarr.ListaBarras, "BARRCODI", "BARRNOMBBARRTRAN", modelo.Entidad.BarrCodi);
            TipoContratoModel modelTipoCont = new TipoContratoModel();
            modelTipoCont.ListaTipoContrato = (new TipoContratoAppServicio()).ListTipoContrato();
            TempData["TIPOCONTCODI2"] = new SelectList(modelTipoCont.ListaTipoContrato, "TIPOCONTCODI", "TIPOCONTNOMBRE", modelo.Entidad.TipoContCodi);
            TipoUsuarioModel modelTipoUsu = new TipoUsuarioModel();
            modelTipoUsu.ListaTipoUsuario = (new TipoUsuarioAppServicio()).ListTipoUsuario();
            TempData["TIPOUSUACODI2"] = new SelectList(modelTipoUsu.ListaTipoUsuario, "TIPOUSUACODI", "TIPOUSUANOMBRE", modelo.Entidad.TipoUsuaCodi);

            return PartialView(modelo); 
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int id = 0)
        {
            CodigoRetiroModel model = new CodigoRetiroModel();
            if (id != 0)
            {
                model.Entidad = (new CodigoRetiroAppServicio()).GetByIdCodigoRetiro(id);
                //SOLICITAR DAR DE DABAJA = SOLBAJAOK
                model.Entidad.SoliCodiRetiObservacion = "SOLBAJAOK";
                model.Entidad.SoliCodiRetiFechaBaja = DateTime.Now.Date;
            }
            model.IdcodRetiro = (new CodigoRetiroAppServicio()).SaveOrUpdateCodigoRetiro(model.Entidad);

            return "true";
        }

        [HttpPost]
        public JsonResult GenerarExcel()
        {
            int indicador = 1;
            string estado = "ASI";
            try
            {
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();

                CodigoRetiroModel model = new CodigoRetiroModel();
                if (TempData["tdListaCodigoRetiro"] != null)
                    model.ListaCodigoRetiro = (List<CodigoRetiroDTO>)TempData["tdListaCodigoRetiro"];
                else
                    model.ListaCodigoRetiro = (new CodigoRetiroAppServicio()).ListCodigoRetiro(estado);

                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteCodigoRetiroExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteCodigoRetiroExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        ws.Cells[2, 3].Value = "LISTA DE CÓDIGOS DE RETIRO SOLICITADOS";
                        ExcelRange rg = ws.Cells[2, 3, 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "EMPRESA";
                        ws.Cells[5, 3].Value = "CLIENTE";
                        ws.Cells[5, 4].Value = "BARRA TRANSFERENCIA";
                        ws.Cells[5, 5].Value = "INICIO OPERACIÓN";
                        ws.Cells[5, 6].Value = "FIN OPERACIÓN";
                        ws.Cells[5, 7].Value = "TIPO CONTRATO";
                        ws.Cells[5, 8].Value = "TIPO USUARIO";
                        ws.Cells[5, 9].Value = "DESCRIPCION";
                        ws.Cells[5, 10].Value = "CÓDIGO RETIRO";

                        rg = ws.Cells[5, 2, 5, 10];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int row = 6;
                        foreach (var item in model.ListaCodigoRetiro)
                        {
                            ws.Cells[row, 2].Value = (item.EmprNombre != null) ? item.EmprNombre.ToString() : string.Empty;
                            ws.Cells[row, 3].Value = (item.CliNombre != null) ? item.CliNombre.ToString() : string.Empty;
                            ws.Cells[row, 4].Value = (item.BarrNombBarrTran != null) ? item.BarrNombBarrTran : string.Empty;
                            ws.Cells[row, 5].Value = (item.SoliCodiRetiFechaInicio != null) ? item.SoliCodiRetiFechaInicio.Value.ToString("dd/MM/yyyy") : string.Empty;
                            ws.Cells[row, 6].Value = (item.SoliCodiRetiFechaFin != null) ? item.SoliCodiRetiFechaFin.Value.ToString("dd/MM/yyyy") : string.Empty;
                            ws.Cells[row, 7].Value = (item.TipoContNombre != null) ? item.TipoContNombre.ToString() : string.Empty;
                            ws.Cells[row, 8].Value = (item.TipoUsuaNombre != null) ? item.TipoUsuaNombre.ToString() : string.Empty;
                            ws.Cells[row, 9].Value = (item.SoliCodiRetiDescripcion != null) ? item.SoliCodiRetiDescripcion.ToString() : string.Empty;
                            ws.Cells[row, 10].Value = (item.SoliCodiRetiCodigo != null) ? item.SoliCodiRetiCodigo.ToString() : string.Empty;
                            //Border por celda
                            rg = ws.Cells[row, 2, row, 10];
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
                        ws.View.FreezePanes(6, 11);
                        rg = ws.Cells[5, 2, row, 10];
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
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteCodigoRetiroExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteCodigoRetiroExcel);
        } 
    }
}
