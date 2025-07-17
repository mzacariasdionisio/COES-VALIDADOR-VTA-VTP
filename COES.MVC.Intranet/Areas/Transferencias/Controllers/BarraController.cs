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
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Configuration;
using log4net;
using COES.Dominio.DTO.Enum;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class BarraController : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BarraController));

        // GET: /Transferencias/barra/
        //[CustomAuthorize]

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        BarraAppServicio servicioBarra = new BarraAppServicio();
        AuditoriaProcesoAppServicio servicioAuditoria = new AuditoriaProcesoAppServicio();

        /// <summary>
        /// Muestra la pantalla inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            BarraModel model = new BarraModel();
            model.bNuevo = (new Funcion()).ValidarPermisoNuevo(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            return View(model);
        }

        /// <summary>
        /// Muestra la lista de datos de la Barra
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Lista()
        {
            BarraModel model = new BarraModel();
            model.ListaBarras = this.servicioBarra.ListVista(); //Lista todas las barras incluido el atributo Nombre area
            model.bEditar = (new Funcion()).ValidarPermisoEditar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            model.bEliminar = (new Funcion()).ValidarPermisoEliminar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            return PartialView(model);
        }

        /// <summary>
        /// Muestra un registro 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult View(int id = 0)
        {
            BarraModel model = new BarraModel();
            model.Entidad = this.servicioBarra.GetByIdBarra(id);

            return PartialView(model);
        }

        /// <summary>
        /// Prepara una vista para ingresar un nuevo registro
        /// </summary>
        /// <returns></returns>
        public ActionResult New()
        {
            BarraModel modelo = new BarraModel();
            modelo.Entidad = new BarraDTO();
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }
            modelo.Entidad.BarrCodi = 0;
            modelo.Entidad.BarrPuntoSumirer = "SI";
            modelo.Entidad.BarrBarraBgr = "SI";
            modelo.Entidad.BarrFlagDesbalance = "NO";
            modelo.Entidad.BarrFlagBarrTran = "NO";
            modelo.Entidad.BarrEstado = "ACT";
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

            AreaModel modelArea = new AreaModel();
            modelArea.ListaAreas = (new AreaAppServicio()).ListAreas();
            TempData["Areacodigo"] = new SelectList(modelArea.ListaAreas, "Areacodi", "Areanombre");

            return PartialView(modelo); 
        }

        /// <summary>
        /// Prepara una vista para editar un nuevo registro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            AreaModel modelArea = new AreaModel();
            modelArea.ListaAreas = (new AreaAppServicio()).ListAreas();
            TempData["Areacodigo"] = new SelectList(modelArea.ListaAreas, "Areacodi", "Areanombre");

            BarraModel modelo = new BarraModel();
            modelo.Entidad = this.servicioBarra.GetByIdBarra(id);
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            return PartialView(modelo);
        }

        /// <summary>
        /// Permite grabar los datos del formulario
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(BarraModel modelo)
        {
            if (ModelState.IsValid)
            {
                modelo.Entidad.BarrUserName = User.Identity.Name;
                modelo.IdBarra = this.servicioBarra.SaveOrUpdateBarra(modelo.Entidad);
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return RedirectToAction("Index");
            }
            //Error
            modelo.sError = "Se ha producido un error al insertar la información";
            AreaModel modelArea = new AreaModel();
            modelArea.ListaAreas = (new AreaAppServicio()).ListAreas();
            TempData["Areacodigo"] = new SelectList(modelArea.ListaAreas, "Areacodi", "Areanombre");
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            return PartialView(modelo);
        }

        /// <summary>
        /// Permite eliminar un registro de la db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int id = 0)
        {
            BarraModel model = new BarraModel();
            model.IdBarra = this.servicioBarra.DeleteBarra(id, User.Identity.Name);
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
                BarraModel model = new BarraModel();
                model.ListaBarras = this.servicioBarra.ListVista(); // Lista todas las barras incluido el atributo Nombre area

                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteBarraExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteBarraExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        ws.Cells[2, 4].Value = "LISTA DE BARRAS Y BARRAS DE TRANSFERENCIA";
                        ExcelRange rg = ws.Cells[2, 4, 2, 4];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "CÓDIGO";
                        ws.Column(2).Style.Numberformat.Format = "#";
                        ws.Cells[5, 3].Value = "BARRA";
                        ws.Cells[5, 4].Value = "TENSIÓN";
                        ws.Cells[5, 5].Value = "PUNTO DE SUMINISTRO";
                        ws.Cells[5, 6].Value = "BARRA BGR";
                        ws.Cells[5, 7].Value = "ÁREA";
                        ws.Cells[5, 8].Value = "VERIF.DESBAL?";
                        ws.Cells[5, 9].Value = "TIENE BT?";
                        ws.Cells[5, 10].Value = "BARRA TRANSFERENCIA";
                        ws.Cells[5, 11].Value = "FACTOR PÉRDIDA";
                        ws.Cells[5, 12].Value = "ESTADO";

                        rg = ws.Cells[5, 2, 5, 12];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int row = 6;
                        foreach (var item in model.ListaBarras)
                        {
                            ws.Cells[row, 2].Value = item.BarrCodi;
                            ws.Cells[row, 3].Value = (item.BarrNombre != null) ? item.BarrNombre : string.Empty;
                            ws.Cells[row, 4].Value = (item.BarrTension != null) ? item.BarrTension.ToString() : string.Empty;
                            ws.Cells[row, 5].Value = (item.BarrPuntoSumirer != null) ? item.BarrPuntoSumirer.ToString() : string.Empty;
                            ws.Cells[row, 6].Value = (item.BarrBarraBgr != null) ? item.BarrBarraBgr.ToString() : string.Empty;
                            ws.Cells[row, 7].Value = (item.AreaNombre != null) ? item.AreaNombre.ToString() : string.Empty;
                            ws.Cells[row, 8].Value = (item.BarrFlagDesbalance != null) ? item.BarrFlagDesbalance.ToString() : string.Empty;
                            ws.Cells[row, 9].Value = (item.BarrFlagBarrTran != null) ? item.BarrFlagBarrTran.ToString() : string.Empty;
                            ws.Cells[row, 10].Value = (item.BarrNombBarrTran != null) ? item.BarrNombBarrTran.ToString() : string.Empty;
                            ws.Cells[row, 11].Value = (item.BarrFactorPerdida != null) ? item.BarrFactorPerdida.ToString() : string.Empty;
                            ws.Cells[row, 12].Value = string.Empty;
                            if (item.BarrEstado != null)
                            {
                                if (item.BarrEstado.ToString().Equals("ACT")) ws.Cells[row, 12].Value = "Activo";
                                else ws.Cells[row, 12].Value = "Inactivo";
                            }
                            //Border por celda
                            rg = ws.Cells[row, 2, row, 12];
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
                        ws.View.FreezePanes(6, 12);
                        rg = ws.Cells[5, 2, row, 12];
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
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteBarraExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Funcion.AppExcel, sFecha + "_" + Funcion.NombreReporteBarraExcel);
        }

        /// <summary>
        /// Prepara una vista para ingresar un nuevo registro
        /// </summary>
        /// <returns></returns>
        public ActionResult RelacionarBarra()
        {
            BarraModel modelo = new BarraModel();
            modelo.Entidad = new BarraDTO();
            modelo.ListaBarrasSum = new List<BarraDTO>();
            modelo.ListaBarras = new List<BarraDTO>();
            modelo.ListaBarras = this.servicioBarra.ListaBarraTransferencia();
            return PartialView(modelo);
        }

        [HttpPost]
        public ActionResult ListaSuministro(string barrtTra)
        {
            int idBarrTra = (string.IsNullOrEmpty(barrtTra)) ? 0 : Convert.ToInt32(barrtTra);

            BarraModel modelo = new BarraModel();
            modelo.ListaBarrasSum = new List<BarraDTO>();

            Session["BARRCODITRA"] = idBarrTra.ToString();

            if (idBarrTra > 0)
            {
                modelo.Entidad = this.servicioBarra.GetByIdBarra(idBarrTra);
                modelo.ListaRelacionBarras = this.servicioBarra.ListaRelacion(idBarrTra);
            }

            List<BarraDTO> lBarraSuministro = new List<BarraDTO>();
            //lBarraSuministro = this.servicioBarra.ListarBarraSuministro();
            lBarraSuministro = this.servicioBarra.ListBarras();

            TempData["BARRNOMBTRA"] = modelo.Entidad.BarrNombre;
            TempData["BARRCODISUM"] = lBarraSuministro;

            return PartialView(modelo);
        }

        [HttpPost]
        public ActionResult RegistrarRelacion(string barraSum , string barraTrans, string barraSumText)
        {
            int idBarrSum = (string.IsNullOrEmpty(barraSum)) ? 0 : Convert.ToInt32(barraSum);

            ResultadoDTO<int> resultado = new ResultadoDTO<int>();
            BarraRelacionDTO entity = new BarraRelacionDTO();
            entity.BareBarrCodiTra = Convert.ToInt32(Session["BARRCODITRA"]);
            entity.BareBarrCodiSum = idBarrSum;
            entity.BareEstado = ConstanteEstados.Activo;
            entity.BareUsuarioRegistro = User.Identity.Name;

            bool existe=this.servicioBarra.ExisteRelacionBarra(entity);
            if (!existe)
            {
                resultado.Data = this.servicioBarra.RegistrarRelacionBarra(entity);

                if (resultado.Data > 0)
                {
                    resultado.EsCorrecto = (int)EnumResultado.correcto;
                    resultado.Mensaje = "Se ha registrado la relación de barras";
                    resultado.Data = entity.BareBarrCodiTra;

                    #region AuditoriaProceso

                    VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                    objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.BarrasDeTransferencia;
                    objAuditoria.Estdcodi = (int)EVtpEstados.Registrar;
                    objAuditoria.Audproproceso = "Se realizó el registro de la relación de barra";
                    objAuditoria.Audprodescripcion = "Se realizó el registro de la relación para la barra de transferencia : " + barraTrans  + " con la barra de suministro : "+ barraSumText+ " - cantidad de errores - 0";
                    objAuditoria.Audprousucreacion = User.Identity.Name;
                    objAuditoria.Audprofeccreacion = DateTime.Now;

                    _= this.servicioAuditoria.save(objAuditoria);

                    #endregion
                }
                else
                {
                    resultado.EsCorrecto = (int)EnumResultado.error;
                    resultado.Mensaje = "No se ha registrado la relación de barras";
                }
            }
            else
            {
                resultado.EsCorrecto = (int)EnumResultado.error;
                resultado.Mensaje = "La relación de barras ya existe";
            }
            
            return Json(resultado);
        }

        [HttpPost]
        public ActionResult EliminarRelacionBarra(string id)
        {
            int bareCodi = (string.IsNullOrEmpty(id)) ? 0 : Convert.ToInt32(id);

            ResultadoDTO<int> resultado = new ResultadoDTO<int>();
            BarraRelacionDTO entity = new BarraRelacionDTO();
            entity.BareBarrCodiTra = Convert.ToInt32(Session["BARRCODITRA"]);
            entity.BareCodi = bareCodi;
            entity.BareEstado = ConstanteEstados.Inactivo;
            entity.BareUsuarioRegistro = User.Identity.Name;
            resultado.Data = this.servicioBarra.EliminarRelacionBarra(entity);

            if (resultado.Data > 0)
            {
                resultado.EsCorrecto = (int)EnumResultado.correcto;
                resultado.Mensaje = "Se ha quitado la relación de barras";
                resultado.Data = entity.BareBarrCodiTra;
            }
            else
            {
                resultado.EsCorrecto = (int)EnumResultado.error;
                resultado.Mensaje = "No se ha podido quitar la relación de barras";

            }

            return Json(resultado);
        }

    }
}
