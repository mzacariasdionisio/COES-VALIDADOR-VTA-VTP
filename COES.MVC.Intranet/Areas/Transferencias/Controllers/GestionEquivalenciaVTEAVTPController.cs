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
using COES.MVC.Intranet.SeguridadServicio;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{

    public class GestionEquivalenciaVTEAVTPController : Controller
    {
        SeguridadServicioClient servicio = new SeguridadServicioClient();
        CodigoRetiroRelacionEquivalenciasAppServicio servicioEquivalencia = new CodigoRetiroRelacionEquivalenciasAppServicio();

        // GET: /Transferencias/GestionCodigosVTEAVTP/GestionEquivalenciaVTEAVTP
        //[CustomAuthorize]
        public ActionResult Index()
        {
            UserDTO usuario = Session[DatosSesion.SesionUsuario] as UserDTO;

            EmpresaModel modelEmpGen = new EmpresaModel();
            modelEmpGen.ListaEmpresas = (new EmpresaAppServicio()).ListaInterCoReSoGen();
            EmpresaModel modelEmpCli = new EmpresaModel();
            modelEmpCli.ListaEmpresas = (new EmpresaAppServicio()).ListaInterCoReSoCli();
            BarraModel modelBarr = new BarraModel();
            modelBarr.ListaBarras = (new BarraAppServicio()).ListaInterCoReSo();
            modelBarr.ListaBarrasSum = (new BarraAppServicio()).ListarBarraSuministro();

            TipoContratoModel modelTipoCont = new TipoContratoModel();
            modelTipoCont.ListaTipoContrato = (new TipoContratoAppServicio()).ListTipoContrato();

            TipoUsuarioModel modelTipoUsu = new TipoUsuarioModel();
            modelTipoUsu.ListaTipoUsuario = (new TipoUsuarioAppServicio()).ListTipoUsuario();

            ViewBag.EMPRCODI2 = new SelectList(modelEmpGen.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE");
            ViewBag.CLICODI2 = new SelectList(modelEmpCli.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE");
            ViewBag.BARRCODI2 = new SelectList(modelBarr.ListaBarras, "BARRCODI", "BARRNOMBBARRTRAN");
            ViewBag.BARRCODI3 = new SelectList(modelBarr.ListaBarrasSum, "BARRCODI", "BARRNOMBRE");

            ViewBag.TIPOCONTCODI2 = new SelectList(modelTipoCont.ListaTipoContrato, "TIPOCONTCODI", "TIPOCONTNOMBRE");
            ViewBag.TIPOUSUACODI2 = new SelectList(modelTipoUsu.ListaTipoUsuario, "TIPOUSUACODI", "TIPOUSUANOMBRE");
            ViewBag.ESTCODSOL = new SelectList(new Funcion().ObtenerEstados(), "Value", "Text");

            return View();
        }  //POST
        [HttpPost]
        public ActionResult Lista(int NroPagina, int? genEmprCodi, int? cliEmprCodi, int? barrCodiTra, int? barrCodiSum, int? tipConCodi, int? tipUsuCodi,string estado, string codigo)
        {
            ViewBag.NroPagina = NroPagina;

            CodigoRetiroEquivalenciaModel model = new CodigoRetiroEquivalenciaModel();
            model.ListaCodigoRetiroRelacion = new CodigoRetiroRelacionEquivalenciasAppServicio().ListarRelacionCodigoRetiros(NroPagina, Funcion.PageSize,
                genEmprCodi, cliEmprCodi, barrCodiTra, barrCodiSum,tipConCodi,tipUsuCodi, estado, codigo);
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult Paginado(int? genEmprCodi, int? cliEmprCodi, int? barrCodiTra, int? barrCodiSum, int? tipConCodi, int? tipUsuCodi, string estado, string codigo,int paginadoActual = 1)
        {
            ViewBag.paginadoActual = paginadoActual;
            CodigoRetiroModel model = new CodigoRetiroModel();
            model.IndicadorPagina = false;
            model.NroRegistros = new CodigoRetiroRelacionEquivalenciasAppServicio().TotalRecordsRelacionCodigoRetiros(genEmprCodi, cliEmprCodi, barrCodiTra, barrCodiSum, tipConCodi, tipUsuCodi, estado, codigo);
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


        public ActionResult New()
        {
            CodigoRetiroEquivalenciaModel model = new CodigoRetiroEquivalenciaModel();
            model.Entidad = new CodigoRetiroRelacionDTO();
            model.Entidad.RetrelCodi = 0;
            EmpresaModel modelEmpGen = new EmpresaModel();
            modelEmpGen.ListaEmpresas = (new EmpresaAppServicio()).ListaInterCoReSoGen();
            ViewBag.EMPRVTEA = new SelectList(modelEmpGen.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE");
            ViewBag.EMPRVTP = new SelectList(modelEmpGen.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE");
            
            return PartialView(model);
        }


        [HttpPost]
        public ActionResult ListaInterCoReSoCliPorEmpresa(int emprcodi)
        {
            return Json(new SelectList(new EmpresaAppServicio().ListaInterCoReSoCliPorEmpresa(emprcodi), "EMPRCODI", "EMPRNOMBRE"));
        }

        [HttpPost]
        public ActionResult ListaInterCoReSoByEmpr(int genemprcodi, int clienemprcodi)
        {
            return Json(new SelectList(new BarraAppServicio().ListaInterCoReSoByEmpr(genemprcodi, clienemprcodi), "BarrCodi", "BarrNombre"));
        }
        [HttpPost]
        public ActionResult ListarCodigoVTEAByEmprBarr(int genemprcodi, int cliemprcodi, int barrcodi)
        {
            return Json(new CodigoRetiroAppServicio().ListarCodigoVTEAByEmprBarr(genemprcodi, cliemprcodi, barrcodi));
        }

        [HttpPost]
        public ActionResult ListarCodigosVTPByEmpBar(int barrcodisum, int genemprcodi, int cliemprcodi)
        {
            return Json(new CodigoRetiroGeneradoAppServicio().ListarCodigosVTPByEmpBar(barrcodisum, genemprcodi, cliemprcodi));
        }

        [HttpPost]
        public ActionResult ListaInterCoReGeByEmpr(int genemprcodi, int cliemprcodi)
        {
            return Json(new SelectList(new BarraAppServicio().ListaInterCoReGeByEmpr(genemprcodi, cliemprcodi), "BarrCodi", "BarrNombre"));
        }

        [HttpPost]
        public ActionResult RegistrarEquivalencia(string cadenaVtea, string cadenaVtp,string variacion,string id)
        {
            List<CodigoRetiroRelacionDetalleDTO> detalle = new List<CodigoRetiroRelacionDetalleDTO>();
            string[] listVtea = null;
            string[] listVtp = null;

            if (!string.IsNullOrEmpty(cadenaVtea))
            {
                string strVtea = cadenaVtea;
                listVtea = strVtea.Split(',');

            }

            if (!string.IsNullOrEmpty(cadenaVtp))
            {
                string strVtp = cadenaVtp;
                listVtp = strVtp.Split(',');

            }

            if (listVtea.Length >= listVtp.Length)
            {
                for (int i = 0; i < listVtea.Length; i++)
                {
                    string[] datosVtea = listVtea[i].Split('_');

                    CodigoRetiroRelacionDetalleDTO obj = new CodigoRetiroRelacionDetalleDTO();

                    obj.Genemprcodivtea = Convert.ToInt32(datosVtea[0].ToString());
                    obj.Cliemprcodivtea = Convert.ToInt32(datosVtea[1].ToString());
                    obj.Barrcodivtea = Convert.ToInt32(datosVtea[2].ToString());
                    obj.Coresocodvtea = Convert.ToInt32(datosVtea[3].ToString());

                    detalle.Add(obj);
                }
                detalle = detalle.OrderBy(x => x.Genemprnombvtea).ToList();

                for (int i = 0; i < listVtp.Length; i++)
                {
                    string[] datosVtp = listVtp[i].Split('_');
                    detalle[i].Genemprcodivtp = Convert.ToInt32(datosVtp[0].ToString());
                    detalle[i].Cliemprcodivtp = Convert.ToInt32(datosVtp[1].ToString());
                    detalle[i].Barrcodivtp = Convert.ToInt32(datosVtp[2].ToString());
                    detalle[i].Coresocodvtp = Convert.ToInt32(datosVtp[3].ToString());
                }

            }
            else
            {
                for (int i = 0; i < listVtp.Length; i++)
                {
                    string[] datosVtp = listVtp[i].Split('_');

                    CodigoRetiroRelacionDetalleDTO obj = new CodigoRetiroRelacionDetalleDTO();
                    obj.Genemprcodivtp = Convert.ToInt32(datosVtp[0].ToString());
                    obj.Cliemprcodivtp = Convert.ToInt32(datosVtp[1].ToString());
                    obj.Barrcodivtp = Convert.ToInt32(datosVtp[2].ToString());
                    obj.Coresocodvtp = Convert.ToInt32(datosVtp[3].ToString());

                    if (listVtea.Length > i)
                    {
                        string[] datosVtea = listVtea[i].Split('_');
                        obj.Genemprcodivtea = Convert.ToInt32(datosVtea[0].ToString());
                        obj.Cliemprcodivtea = Convert.ToInt32(datosVtea[1].ToString());
                        obj.Barrcodivtea = Convert.ToInt32(datosVtea[2].ToString());
                        obj.Coresocodvtea = Convert.ToInt32(datosVtea[3].ToString());
                    }
                    detalle.Add(obj);
                }

            }

            CodigoRetiroRelacionDTO oCodRetrel = new CodigoRetiroRelacionDTO();
            oCodRetrel.Retrelvari = string.IsNullOrEmpty(variacion) ?0: Convert.ToDecimal(variacion);
            oCodRetrel.Retelestado = ConstanteEstados.Activo;
            oCodRetrel.Retrelusucreacion = User.Identity.Name;
            oCodRetrel.RetrelCodi = string.IsNullOrEmpty(id) ? 0 : Convert.ToInt32(id);
            int result = this.servicioEquivalencia.RegistrarEquivalencia(oCodRetrel, detalle);

            return Json("true");
        }

        public ActionResult Edit(int id)
        {
            CodigoRetiroEquivalenciaModel model = new CodigoRetiroEquivalenciaModel();

            model.Entidad = servicioEquivalencia.ObtenereEquivalencia(id);

            EmpresaModel modelEmpGen = new EmpresaModel();
            modelEmpGen.ListaEmpresas = (new EmpresaAppServicio()).ListaInterCoReSoGen();
            ViewBag.EMPRVTEA = new SelectList(modelEmpGen.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE");
            ViewBag.EMPRVTP = new SelectList(modelEmpGen.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE");

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ListaDetalle(int id)
        {
            CodigoRetiroEquivalenciaModel model = new CodigoRetiroEquivalenciaModel();

            model.Entidad = servicioEquivalencia.ObtenereEquivalencia(id);

            model.ListaEquivalencia = servicioEquivalencia.ObtenereEquivalenciaDetalle(id);


            return Json(model.ListaEquivalencia);
        }

        [HttpPost]
        public ActionResult ExisteVTEA(string id)
        {
            int coresocodvtea = Convert.ToInt32(id);
            bool result = this.servicioEquivalencia.ExisteVTEA(coresocodvtea);

            return Json(result.ToString());
        }

        [HttpPost]
        public ActionResult ExisteVTP(string id)
        {
            int coresocodvtp = Convert.ToInt32(id);
            bool result = this.servicioEquivalencia.ExisteVTP(coresocodvtp);

            return Json(result.ToString());
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            bool response = false;
            int retrelcodi = Convert.ToInt32(id);
            CodigoRetiroRelacionDTO entity = new CodigoRetiroRelacionDTO();
            entity = this.servicioEquivalencia.ObtenereEquivalencia(retrelcodi);

            entity.Retelestado = ConstanteEstados.Inactivo;
            entity.Retrelusumodificacion= User.Identity.Name;

            int result = this.servicioEquivalencia.EliminarEquivalencia(entity);

            if (result>0)
            {
                response = true;
            }

            return Json(response.ToString());
        }

        [HttpPost]
        public JsonResult GenerarExcel(int? genEmprCodi, int? cliEmprCodi, int? barrCodiTra, int? barrCodiSum, int? tipConCodi, int? tipUsuCodi, string estado, string codigo)
        {
            int indicador = -1;
            try
            {
                CodigoRetiroEquivalenciaModel model = new CodigoRetiroEquivalenciaModel();
                model.ListaCodigoRetiroRelacion = new CodigoRetiroRelacionEquivalenciasAppServicio().ListarRelacionCodigoRetiros(1, 5000,
                    genEmprCodi, cliEmprCodi, barrCodiTra, barrCodiSum, tipConCodi, tipUsuCodi, estado, codigo);

                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();

                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteCodigoEquivalenciaExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteCodigoEquivalenciaExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {
                        
                        //TITULO
                        ws.Cells[2, 3].Value = "LISTA DE RELACIÓN DE EQUIVALENCIA DE CÓDIGOS VTP Y VTEA ";
                        ExcelRange rg = ws.Cells[2, 3, 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;

                        int row = 5;
                        ws.Cells[row, 2].Value = "CÓDIGO VTEA";
                        rg = ws.Cells[row, 2, row, 7];
                        rg.Merge = true;
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FF6600"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        ws.Cells[row, 8].Value = "CÓDIGO VTP";
                        rg = ws.Cells[row, 8, row, 13];
                        rg.Merge = true;
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#339900"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        

                        row = 6;
                        //CABECERA DE TABLA
                        ws.Cells[row, 2].Value = "EMPRESA";
                        ws.Cells[row, 3].Value = "CLIENTE";
                        ws.Cells[row, 4].Value = "CONTRATO";
                        ws.Cells[row, 5].Value = "TIPO USUARIO";
                        ws.Cells[row, 6].Value = "BARRA";
                        ws.Cells[row, 7].Value = "CÓDIGO";
                        ws.Cells[row, 8].Value = "EMPRESA";
                        ws.Cells[row, 9].Value = "CLIENTE";
                        ws.Cells[row, 10].Value = "CONTRATO";
                        ws.Cells[row, 11].Value = "TIPO USUARIO";
                        ws.Cells[row, 12].Value = "BARRA";
                        ws.Cells[row, 13].Value = "CÓDIGO";
                        ws.Cells[row, 14].Value = "% VAR.";

                        rg = ws.Cells[row, 2, row, 14];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        row = 7;
                        int rowInit = 0;
                        foreach (var item in model.ListaCodigoRetiroRelacion)
                        {
                            rowInit = row;
                            ws.Cells[row, 14].Value = (item.Retrelvari != null) ? item.Retrelvari.ToString() : "0";
                            foreach (var item2 in item.ListarRelacion)
                            {
                                ws.Cells[row, 2].Value = (item2.Genemprnombvtea != null) ? item2.Genemprnombvtea : string.Empty;
                                ws.Cells[row, 3].Value = (item2.Cliemprnombvtea != null) ? item2.Cliemprnombvtea : string.Empty;
                                ws.Cells[row, 4].Value = (item2.Tipocontratovtea != null) ? item2.Tipocontratovtea : string.Empty;
                                ws.Cells[row, 5].Value = (item2.Tipousuariovtea != null) ? item2.Tipousuariovtea : string.Empty;
                                ws.Cells[row, 6].Value = (item2.Barrnombvtea != null) ? item2.Barrnombvtea : string.Empty;
                                ws.Cells[row, 7].Value = (item2.Codigovtea != null) ? item2.Codigovtea : string.Empty;

                                ws.Cells[row, 8].Value = (item2.Genemprnombvtp != null) ? item2.Genemprnombvtp : string.Empty;
                                ws.Cells[row, 9].Value = (item2.Cliemprnombvtp != null) ? item2.Cliemprnombvtp : string.Empty;
                                ws.Cells[row, 10].Value = (item2.Tipocontratovtp != null) ? item2.Tipocontratovtp : string.Empty;
                                ws.Cells[row, 11].Value = (item2.Tipousuariovtp != null) ? item2.Tipousuariovtp : string.Empty;
                                ws.Cells[row, 12].Value = (item2.Barrnombvtp != null) ? item2.Barrnombvtp : string.Empty;
                                ws.Cells[row, 13].Value = (item2.Codigovtp != null) ? item2.Codigovtp : string.Empty;

                                //Border por celda

                                row++;
                            }
                            
                            rg = ws.Cells[rowInit, 2, row-1, 14];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;
                            rg = ws.Cells[rowInit, 14, row-1, 14];
                            rg.Merge = true;

                        }

                        rg = ws.Cells[6, 2, row, 14];
                        rg.AutoFitColumns();

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create("http://www.coes.org.pe/wcoes/images/logocoes.png");
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);

                        xlPackage.Save();
                    }
                }
                indicador = 1;
            }
            catch (Exception ex)
            {
                indicador = -1;
            }

             return Json(indicador);
        }

        [HttpGet]
        public virtual ActionResult AbrirExcel()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteCodigoEquivalenciaExcel;
            return File(path, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteCodigoEquivalenciaExcel);
        }

    }
}
