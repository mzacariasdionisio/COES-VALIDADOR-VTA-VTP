using COES.Dominio.DTO.Transferencias;
using COES.MVC.Extranet.Areas.Transferencias.Helper;
using COES.MVC.Extranet.Areas.Transferencias.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Transferencias.Controllers
{
    public class CodigoRetiroController : BaseController
    {
        //[CustomAuthorize]
        //Pagina de inicio del controlador
        public ActionResult Index()
        {
            SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
            List<SeguridadServicio.EmpresaDTO> listTotal = Funcion.ObtenerEmpresasPorUsuario(User.Identity.Name);
            List<SeguridadServicio.EmpresaDTO> list = new List<SeguridadServicio.EmpresaDTO>();

            //- aca debemos hacer jugada para escoger la empresa
            List<TrnInfoadicionalDTO> listaInfoAdicional = (new TransferenciasAppServicio()).ListTrnInfoadicionals();
            List<int> idsEmpresas = listaInfoAdicional.Where(x => x.Emprcodi != null).Select(x => (int)x.Emprcodi).Distinct().ToList();

            foreach (var item in listTotal)
            {
                list.Add(item);
                //ASSETEC 20190111 - comparar contra otros conceptos
                Int16 iEmpcodi = Convert.ToInt16(item.EMPRCODI);
                foreach (TrnInfoadicionalDTO dtoOtroConcepto in listaInfoAdicional)
                {
                    Int16 iOtroConcepto = Convert.ToInt16(dtoOtroConcepto.Emprcodi);
                    if (iEmpcodi == iOtroConcepto)
                    {
                        SeguridadServicio.EmpresaDTO dtoEmpresaConcepto = new SeguridadServicio.EmpresaDTO();
                        dtoEmpresaConcepto.EMPRCODI = Convert.ToInt16(dtoOtroConcepto.Infadicodi);
                        dtoEmpresaConcepto.EMPRNOMB = dtoOtroConcepto.Infadinomb;
                        dtoEmpresaConcepto.TIPOEMPRCODI = Convert.ToInt16(dtoOtroConcepto.Tipoemprcodi);
                        list.Add(dtoEmpresaConcepto);
                    }
                }
                //--------------------------------------------------
            }


            TempData["EMPRNRO"] = list.Count();
            if (list.Count() == 1)
            {
                TempData["EMPRNOMB"] = list[0].EMPRNOMB;
                Session["EmprNomb"] = list[0].EMPRNOMB;
                Session["EmprCodi"] = list[0].EMPRCODI;
            }
            else if (Session["EmprCodi"] != null)
            {
                int iEmprCodi = Convert.ToInt32(Session["EmprCodi"].ToString());
                EmpresaDTO dtoEmpresa = new EmpresaDTO();
                dtoEmpresa = (new EmpresaAppServicio()).GetByIdEmpresa(iEmprCodi);
                TempData["EMPRNOMB"] = dtoEmpresa.EmprNombre;
                Session["EmprNomb"] = dtoEmpresa.EmprNombre;
            }
            else if (list.Count() > 1)
            {
                TempData["EMPRNOMB"] = "";
                return View();

            }
            else
            {
                //No hay empresa asociada a la cuenta
                TempData["EMPRNOMB"] = "";
                TempData["EMPRNRO"] = -1;
                return View();
            }
            EmpresaModel modelCliente = new EmpresaModel();
            modelCliente.ListaEmpresas = (new EmpresaAppServicio()).ListaInterCoReSoCli();

            BarraModel modelBarra = new BarraModel();
            modelBarra.ListaBarras = (new BarraAppServicio()).ListaInterCoReSo();

            TipoContratoModel modelTipoCont = new TipoContratoModel();
            modelTipoCont.ListaTipoContrato = (new TipoContratoAppServicio()).ListTipoContrato();

            TipoUsuarioModel modelTipoUsu = new TipoUsuarioModel();
            modelTipoUsu.ListaTipoUsuario = (new TipoUsuarioAppServicio()).ListTipoUsuario();

            TempData["CLICODI2"] = new SelectList(modelCliente.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE");
            TempData["BARRCODI2"] = new SelectList(modelBarra.ListaBarras, "BARRCODI", "BARRNOMBBARRTRAN");
            TempData["TIPOCONTCODI2"] = new SelectList(modelTipoCont.ListaTipoContrato, "TIPOCONTCODI", "TIPOCONTNOMBRE");
            TempData["TIPOUSUACODI2"] = new SelectList(modelTipoUsu.ListaTipoUsuario, "TIPOUSUACODI", "TIPOUSUANOMBRE");

            return View();
        }

        /// Permite cargar los la lista
        //POST
        [HttpPost]
        public ActionResult Lista(string nombre, string tipousu, string tipocont, string bartran, string clinomb, string fechaInicio, string fechaFin)
        {
            if (tipousu.Equals("--Seleccione--"))
                tipousu = null;
            if (tipocont.Equals("--Seleccione--"))
                tipocont = null;
            if (bartran.Equals("--Seleccione--"))
                bartran = null;
            if (clinomb.Equals("--Seleccione--"))
                clinomb = null;

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

            if (Session["EmprCodi"] != null)
            {
                int iEmprCodi = Convert.ToInt32(Session["EmprCodi"].ToString());
                EmpresaDTO dtoEmpresa = new EmpresaDTO();
                dtoEmpresa = (new EmpresaAppServicio()).GetByIdEmpresa(iEmprCodi);
                nombre = dtoEmpresa.EmprNombre;

                string Solicodiretiobservacion = null;
                string estado = null;
                CodigoRetiroModel model = new CodigoRetiroModel();
                model.ListaCodigoRetiro = (new CodigoRetiroAppServicio()).BuscarCodigoRetiroExtranet(nombre, tipousu, tipocont, bartran, clinomb, dtfi, dtff, Solicodiretiobservacion, estado);
                TempData["tdListaCodigoRetiro"] = model.ListaCodigoRetiro;
                foreach (var x in model.ListaCodigoRetiro)
                {
                    if (x.SoliCodiRetiCodigo == null)
                        x.SoliCodiRetiCodigo = "Sin asignar";
                }
                return PartialView(model);
            }
            return RedirectToAction("Index");
        }

        /// Permite  cargar la vista (detalle) de un registro
        [HttpPost]
        public ActionResult View(int id = 0)
        {
            CodigoRetiroModel model = new CodigoRetiroModel();
            if (id != 0)
            {
                model.Entidad = (new CodigoRetiroAppServicio()).GetByIdCodigoRetiro(id);
                if (model.Entidad.SoliCodiRetiCodigo == null)
                    model.Entidad.SoliCodiRetiCodigo = "----PENDIENTE----";
            }
            return PartialView(model);
        }

        /// Permite mostrar el formulario para un nuevo registro
        public ActionResult New()
        {
            CodigoRetiroModel modelo = new CodigoRetiroModel();
            modelo.Entidad = new CodigoRetiroDTO();
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }
            modelo.Entidad.SoliCodiRetiCodi = 0;
            modelo.Solicodiretifechainicio = System.DateTime.Now.ToString("dd/MM/yyyy");
            modelo.Solicodiretifechafin = System.DateTime.Now.ToString("dd/MM/yyyy");

            EmpresaModel modelCliente = new EmpresaModel();
            modelCliente.ListaEmpresas = (new EmpresaAppServicio()).ListEmpresas();
            TempData["CLICODI2"] = modelCliente;

            BarraModel modelBarra = new BarraModel();
            modelBarra.ListaBarras = (new BarraAppServicio()).ListBarras();
            TempData["BARRCODI2"] = modelBarra;

            TipoContratoModel modelTipoCont = new TipoContratoModel();
            modelTipoCont.ListaTipoContrato = (new TipoContratoAppServicio()).ListTipoContrato();
            TempData["TIPOCONTCODI2"] = new SelectList(modelTipoCont.ListaTipoContrato, "TIPOCONTCODI", "TIPOCONTNOMBRE");
            TipoUsuarioModel modelTipoUsu = new TipoUsuarioModel();
            modelTipoUsu.ListaTipoUsuario = (new TipoUsuarioAppServicio()).ListTipoUsuario();
            TempData["TIPOUSUACODI2"] = new SelectList(modelTipoUsu.ListaTipoUsuario, "TIPOUSUACODI", "TIPOUSUANOMBRE");
            TempData["EMPRNOMB"] = Session["EmprNomb"];
            return PartialView(modelo);
        }

        /// Permite actualizar o grabar un registro
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(CodigoRetiroModel modelo)
        {
            if (ModelState.IsValid)
            {
                modelo.Entidad.SoliCodiRetiFechaRegistro = DateTime.Now;
                modelo.Entidad.SoliCodiRetiCodigo = null;
                modelo.Entidad.SoliCodiRetiEstado = "GEN";
                modelo.Entidad.SoliCodiRetiObservacion = "SOLBAJANO";
                if (modelo.Solicodiretifechainicio != "" && modelo.Solicodiretifechainicio != null)
                    modelo.Entidad.SoliCodiRetiFechaInicio = DateTime.ParseExact(modelo.Solicodiretifechainicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (modelo.Solicodiretifechafin != "" && modelo.Solicodiretifechafin != null)
                    modelo.Entidad.SoliCodiRetiFechaFin = DateTime.ParseExact(modelo.Solicodiretifechafin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                //CAPTURAR DEL LOGUEO USUASEIN
                modelo.Entidad.UsuaCodi = User.Identity.Name;
                if (Session["EmprCodi"] != null)
                {
                    modelo.Entidad.EmprCodi = Convert.ToInt32(Session["EmprCodi"].ToString());
                    modelo.IdcodRetiro = (new CodigoRetiroAppServicio()).SaveOrUpdateCodigoRetiro(modelo.Entidad);
                    TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                }
                else
                    TempData["sMensajeExito"] = "Lo sentimos, no se ha procesado la información, favor de seleccionar una empresa primero";

                return RedirectToAction("Index");
            }
            //Error
            modelo.sError = "No se ha podido almacenar correctamente la información, favor de verificar los datos registrados";
            EmpresaModel modelCliente = new EmpresaModel();
            modelCliente.ListaEmpresas = (new EmpresaAppServicio()).ListEmpresas();
            TempData["CLICODI2"] = new SelectList(modelCliente.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE", modelo.Entidad.EmprCodi);
            BarraModel modelBarra = new BarraModel();
            modelBarra.ListaBarras = (new BarraAppServicio()).ListBarras();
            TempData["BARRCODI2"] = new SelectList(modelBarra.ListaBarras, "BARRCODI", "BARRNOMBBARRTRAN", modelo.Entidad.BarrCodi);
            TipoContratoModel modelTipoCont = new TipoContratoModel();
            modelTipoCont.ListaTipoContrato = (new TipoContratoAppServicio()).ListTipoContrato();
            TempData["TIPOCONTCODI2"] = new SelectList(modelTipoCont.ListaTipoContrato, "TIPOCONTCODI", "TIPOCONTNOMBRE", modelo.Entidad.TipoContCodi);
            TipoUsuarioModel modelTipoUsu = new TipoUsuarioModel();
            modelTipoUsu.ListaTipoUsuario = (new TipoUsuarioAppServicio()).ListTipoUsuario();
            TempData["TIPOUSUACODI2"] = new SelectList(modelTipoUsu.ListaTipoUsuario, "TIPOUSUACODI", "TIPOUSUANOMBRE", modelo.Entidad.TipoUsuaCodi);
            TempData["EMPRNOMB"] = Session["EmprNomb"];
            return PartialView(modelo);
        }

        /// Permite controlar un registro mediante estados
        /// en este caso delete significa que esta solicitandose de baja = SOLBAJAPEN
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int id = 0)
        {
            CodigoRetiroModel model = new CodigoRetiroModel();
            if (id != 0)
            {
                model.Entidad = (new CodigoRetiroAppServicio()).GetByIdCodigoRetiro(id);

                if (model.Entidad.SoliCodiRetiEstado == "GEN")
                {
                    model.Entidad.SoliCodiRetiObservacion = "SOLBAJAOK";
                    model.Entidad.SoliCodiretiFechaSolBaja = DateTime.Now.Date;
                    model.Entidad.SoliCodiRetiFechaBaja = DateTime.Now.Date;
                    model.Entidad.CoesUserName = base.UserName;
                    //model.IdcodRetiro = (new CodigoRetiroAppServicio()).SaveOrUpdateCodigoRetiro(model.Entidad);
                    (new CodigoRetiroAppServicio()).DeleteCodigoRetiroTotal(model.Entidad.SoliCodiRetiCodi);
                }
                else
                {
                    //SOLICITAR DAR DE DABAJA = SOLBAJAPEN
                    if (model.Entidad.SoliCodiRetiObservacion == null)
                        model.Entidad.SoliCodiRetiObservacion = "SOLBAJAPEN";
                    model.Entidad.SoliCodiretiFechaSolBaja = DateTime.Now.Date;
                    model.IdcodRetiro = (new CodigoRetiroAppServicio()).SaveOrUpdateCodigoRetiro(model.Entidad);
                }
            }

            return "true";
        }

        /// Permite seleccionar a la empresa con la que desea trabajar
        [HttpPost]
        public ActionResult EscogerEmpresa()
        {
            SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
            List<TrnInfoadicionalDTO> listaInfoAdicional = (new TransferenciasAppServicio()).ListTrnInfoadicionals();
            List<int> idsEmpresas = listaInfoAdicional.Where(x => x.Emprcodi != null).Select(x => (int)x.Emprcodi).Distinct().ToList();
            List<SeguridadServicio.EmpresaDTO> list = Funcion.ObtenerEmpresasPorUsuario(User.Identity.Name, idsEmpresas);

            EmpresaModel model = new EmpresaModel();
            List<EmpresaDTO> lista = new List<EmpresaDTO>();
            foreach (var item in list)
            {
                EmpresaDTO dtoEmpresa = new EmpresaDTO();
                dtoEmpresa = (new EmpresaAppServicio()).GetByIdEmpresa(item.EMPRCODI);
                lista.Add(dtoEmpresa);

                //ASSETEC 20190111 - comparar contra otros conceptos
                Int16 iEmpcodi = Convert.ToInt16(item.EMPRCODI);
                foreach (TrnInfoadicionalDTO dtoOtroConcepto in listaInfoAdicional)
                {
                    Int16 iOtroConcepto = Convert.ToInt16(dtoOtroConcepto.Emprcodi);
                    if (iEmpcodi == iOtroConcepto)
                    {
                        EmpresaDTO dtoEmpresaConcepto = new EmpresaDTO();
                        dtoEmpresaConcepto.EmprCodi = Convert.ToInt16(dtoOtroConcepto.Infadicodi);
                        dtoEmpresaConcepto.EmprNombre = dtoOtroConcepto.Infadinomb;
                        dtoEmpresaConcepto.TipoEmprCodi = Convert.ToInt16(dtoOtroConcepto.Tipoemprcodi);
                        lista.Add(dtoEmpresaConcepto);
                    }
                }
                //--------------------------------------------------
            }

            //model.ListaEmpresas = lista.Where(x => !idsEmpresas.Any(y => x.EmprCodi == y)).OrderBy(x => x.EmprNombre).ToList();
            model.ListaEmpresas = lista.OrderBy(x => x.EmprNombre).ToList();
            return PartialView(model);
        }

        /// Permite actualizar o grabar un registro
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmpresaElegida(int EmprCodi)
        {
            //ASSETEC 20190111
            if (EmprCodi != 0)
            {
                Session["EmprCodi"] = EmprCodi;
            }
            return RedirectToAction("Index");
        }


        //Exporta la lista de registro a un excel
        [HttpPost]
        public JsonResult GenerarExcel(string nombre, string tipousu, string tipocont, string bartran, string clinomb, string fechaInicio, string fechaFin)
        {
            int indicador = 1;

            if (Session["EmprCodi"] != null)
            {
                try
                {
                    int iEmprCodi = Convert.ToInt32(Session["EmprCodi"].ToString());
                    EmpresaDTO dtoEmpresa = new EmpresaDTO();
                    dtoEmpresa = (new EmpresaAppServicio()).GetByIdEmpresa(iEmprCodi);
                    nombre = dtoEmpresa.EmprNombre;

                    string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();

                    CodigoRetiroModel model = new CodigoRetiroModel();
                    if (TempData["tdListaCodigoRetiro"] != null)
                        model.ListaCodigoRetiro = (List<CodigoRetiroDTO>)TempData["tdListaCodigoRetiro"];
                    else
                    {
                        if (tipousu.Equals("--Seleccione--"))
                            tipousu = null;
                        if (tipocont.Equals("--Seleccione--"))
                            tipocont = null;
                        if (bartran.Equals("--Seleccione--"))
                            bartran = null;
                        if (clinomb.Equals("--Seleccione--"))
                            clinomb = null;
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
                        string Solicodiretiobservacion = null;
                        string estado = null;
                        model.ListaCodigoRetiro = (new CodigoRetiroAppServicio()).BuscarCodigoRetiroExtranet(nombre, tipousu, tipocont, bartran, clinomb, dtfi, dtff, Solicodiretiobservacion, estado);

                    }

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
                            ws.Cells[2, 3].Value = "LISTA DE CÓDIGOS DE RETIRO SOLICITADOS POR " + nombre;
                            ExcelRange rg = ws.Cells[2, 3, 2, 3];
                            rg.Style.Font.Size = 16;
                            rg.Style.Font.Bold = true;
                            //CABECERA DE TABLA
                            ws.Cells[5, 2].Value = "CLIENTE";
                            ws.Cells[5, 3].Value = "BARRA TRANSFERENCIA";
                            ws.Cells[5, 4].Value = "INICIO OPERACIÓN";
                            ws.Cells[5, 5].Value = "FIN OPERACIÓN";
                            ws.Cells[5, 6].Value = "TIPO CONTRATO";
                            ws.Cells[5, 7].Value = "TIPO USUARIO";
                            ws.Cells[5, 8].Value = "DESCRIPCION";
                            ws.Cells[5, 9].Value = "CÓDIGO RETIRO";

                            rg = ws.Cells[5, 2, 5, 9];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            int row = 6;
                            foreach (var item in model.ListaCodigoRetiro)
                            {
                                ws.Cells[row, 2].Value = (item.CliNombre != null) ? item.CliNombre.ToString() : string.Empty;
                                ws.Cells[row, 3].Value = (item.BarrNombBarrTran != null) ? item.BarrNombBarrTran : string.Empty;
                                ws.Cells[row, 4].Value = (item.SoliCodiRetiFechaInicio != null) ? item.SoliCodiRetiFechaInicio.Value.ToString("dd/MM/yyyy") : string.Empty;
                                ws.Cells[row, 5].Value = (item.SoliCodiRetiFechaFin != null) ? item.SoliCodiRetiFechaFin.Value.ToString("dd/MM/yyyy") : string.Empty;
                                ws.Cells[row, 6].Value = (item.TipoContNombre != null) ? item.TipoContNombre.ToString() : string.Empty;
                                ws.Cells[row, 7].Value = (item.TipoUsuaNombre != null) ? item.TipoUsuaNombre.ToString() : string.Empty;
                                ws.Cells[row, 8].Value = (item.SoliCodiRetiDescripcion != null) ? item.SoliCodiRetiDescripcion.ToString() : string.Empty;
                                ws.Cells[row, 9].Value = (item.SoliCodiRetiCodigo != null) ? item.SoliCodiRetiCodigo.ToString() : string.Empty;
                                //Border por celda
                                rg = ws.Cells[row, 2, row, 9];
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
                            ws.View.FreezePanes(6, 10);
                            rg = ws.Cells[5, 2, row, 9];
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
            }
            else
                indicador = -1;

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
