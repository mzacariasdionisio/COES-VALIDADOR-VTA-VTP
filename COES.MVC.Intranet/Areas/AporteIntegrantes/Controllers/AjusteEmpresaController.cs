using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.CalculoPorcentajes;
using COES.MVC.Intranet.Areas.AporteIntegrantes.Models;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Dominio.DTO.Transferencias;
using System.Globalization;
using COES.Dominio.DTO.Sic;
using System.Reflection;
using log4net;
using System.Collections;
using COES.Servicios.Aplicacion.Transferencias;
using System.Configuration;
using COES.Servicios.Aplicacion.CalculoPorcentajes.Helper;
using System.Data;
using COES.Servicios.Aplicacion.Informe;


namespace COES.MVC.Intranet.Areas.AporteIntegrantes.Controllers
{
    public class AjusteEmpresaController : BaseController
    {
        // GET: /AporteIntegrantes/AjusteEmpresa/
        public AjusteEmpresaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }
        
        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        CalculoPorcentajesAppServicio servicioCalculoPorcentaje = new CalculoPorcentajesAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        
        //parametro = codigo del ajuste
        public ActionResult Index(int caiajcodi = 0, string caiajetipoinfo = "")
        {
            base.ValidarSesionUsuario();
            AjusteEmpresaModel model = new AjusteEmpresaModel();
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            Log.Info("Entidad Ajuste - GetByIdCaiAjuste");
            model.EntidadAjuste = this.servicioCalculoPorcentaje.GetByIdCaiAjuste(caiajcodi);
            Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
            model.EntidadPresupuesto = this.servicioCalculoPorcentaje.GetByIdCaiPresupuesto(model.EntidadAjuste.Caiprscodi);
            model.Caiajetipoinfo = caiajetipoinfo;
            return View(model);
        }

        [HttpPost]
        public ActionResult Lista(int caiajcodi, string caiajetipoinfo)
        {
            AjusteEmpresaModel model = new AjusteEmpresaModel();
            Log.Info("Lista AjusteEmpresa - ListCaiAjusteempresasByAjuste");
            model.ListaAjusteEmpresa = this.servicioCalculoPorcentaje.ListCaiAjusteempresasByAjuste(caiajcodi, caiajetipoinfo);
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            model.Caiajetipoinfo = caiajetipoinfo;
            return PartialView(model);
        }

        public ActionResult New(int caiajcodi, string caiajetipoinfo)
        {
            AjusteEmpresaModel model = new AjusteEmpresaModel();
            model.EntidadAjusteEmpresa = new CaiAjusteempresaDTO();
            model.EntidadAjusteEmpresa.Caiajecodi = 0;
            model.EntidadAjusteEmpresa.Caiajcodi = caiajcodi;
            model.EntidadAjusteEmpresa.Caiajetipoinfo = caiajetipoinfo;
            model.Caiajereteneejeini = System.DateTime.Now.ToString("dd/MM/yyyy");
            model.Caiajereteneejefin = System.DateTime.Now.ToString("dd/MM/yyyy");
            model.Caiajeretenepryaini = System.DateTime.Now.ToString("dd/MM/yyyy");
            model.Caiajeretenepryafin = System.DateTime.Now.ToString("dd/MM/yyyy");
            model.Caiajereteneprybini = System.DateTime.Now.ToString("dd/MM/yyyy");
            model.Caiajereteneprybfin = System.DateTime.Now.ToString("dd/MM/yyyy");
            Log.Info("Lista Empresa - ListEmpresasSTR");
            model.ListaEmpresa = this.servicioEmpresa.ListEmpresasSTR();
            Log.Info("Lista PtoMedicion - ListMePtomedicion");
            model.ListaPtoMedicion = this.servicioCalculoPorcentaje.ListCaiAjusteempresasPtomed(ConstantesCalculoPorcentajes.IdOrigenLectura);
            //model.ListaPtoMedicion = this.servicioCalculoPorcentaje.ListMePtomedicion("", "-1");
            Log.Info("Entidad Ajuste - GetByIdCaiAjuste");
            model.EntidadAjuste = this.servicioCalculoPorcentaje.GetByIdCaiAjuste(caiajcodi);
            Log.Info("Lista Presupuesto - GetByIdCaiPresupuesto");
            model.EntidadPresupuesto = this.servicioCalculoPorcentaje.GetByIdCaiPresupuesto(model.EntidadAjuste.Caiprscodi);
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(AjusteEmpresaModel model)
        {
            if (ModelState.IsValid)
            {
                CaiAjusteempresaDTO dto = new CaiAjusteempresaDTO();
                dto.Caiajecodi = model.EntidadAjusteEmpresa.Caiajecodi;
                dto.Caiajcodi = model.EntidadAjusteEmpresa.Caiajcodi;
                dto.Emprcodi = model.EntidadAjusteEmpresa.Emprcodi;
                dto.Ptomedicodi = model.EntidadAjusteEmpresa.Ptomedicodi;
                dto.Caiajetipoinfo = model.EntidadAjusteEmpresa.Caiajetipoinfo;
                if (model.Caiajereteneejeini != "" && model.Caiajereteneejeini != null)
                    dto.Caiajereteneejeini = DateTime.ParseExact(model.Caiajereteneejeini, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (model.Caiajereteneejefin != "" && model.Caiajereteneejefin != null)
                    dto.Caiajereteneejefin = DateTime.ParseExact(model.Caiajereteneejefin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (model.Caiajeretenepryaini != "" && model.Caiajeretenepryaini != null)
                    dto.Caiajeretenepryaini = DateTime.ParseExact(model.Caiajeretenepryaini, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (model.Caiajeretenepryafin != "" && model.Caiajeretenepryafin != null)
                    dto.Caiajeretenepryafin = DateTime.ParseExact(model.Caiajeretenepryafin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (model.Caiajereteneprybini != "" && model.Caiajereteneprybini != null)
                    dto.Caiajereteneprybini = DateTime.ParseExact(model.Caiajereteneprybini, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (model.Caiajereteneprybfin != "" && model.Caiajereteneprybfin != null)
                    dto.Caiajereteneprybfin = DateTime.ParseExact(model.Caiajereteneprybfin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dto.Caiajeusucreacion = User.Identity.Name;
                dto.Caiajefeccreacion = DateTime.Now;

                if (model.EntidadAjusteEmpresa.Caiajecodi == 0)
                {
                    Log.Info("Insertar registro - SaveCaiAjusteempresa");
                    this.servicioCalculoPorcentaje.SaveCaiAjusteempresa(dto);
                }
                else
                {
                    Log.Info("Actualiza registro - UpdateCaiAjusteempresa");
                    this.servicioCalculoPorcentaje.UpdateCaiAjusteempresa(dto);
                }
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return RedirectToAction("Index", new { caiajcodi = model.EntidadAjusteEmpresa.Caiajcodi, caiajetipoinfo = model.EntidadAjusteEmpresa.Caiajetipoinfo });
            }
            model.sError = "Se ha producido un error al insertar la información";
            Log.Info("Lista Empresa - ListEmpresasSTR");
            model.ListaEmpresa = this.servicioEmpresa.ListEmpresasSTR();
            Log.Info("Lista PtoMedicion - ListMePtomedicion");
            model.ListaPtoMedicion = this.servicioCalculoPorcentaje.ListCaiAjusteempresasPtomed(ConstantesCalculoPorcentajes.IdOrigenLectura);  //this.servicioCalculoPorcentaje.ListMePtomedicion("", "-1");
            Log.Info("Entidad Ajuste - GetByIdCaiAjuste");
            model.EntidadAjuste = this.servicioCalculoPorcentaje.GetByIdCaiAjuste(model.EntidadAjusteEmpresa.Caiajcodi);
            Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
            model.EntidadPresupuesto = this.servicioCalculoPorcentaje.GetByIdCaiPresupuesto(model.EntidadAjuste.Caiprscodi);
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(model);

        }

        public ActionResult Edit(int caiajecodi)
        {
            AjusteEmpresaModel model = new AjusteEmpresaModel();
            Log.Info("Entidad AjusteEmpresa - GetByIdCaiAjusteempresa");
            model.EntidadAjusteEmpresa = this.servicioCalculoPorcentaje.GetByIdCaiAjusteempresa(caiajecodi);
            if (model.EntidadAjusteEmpresa.Caiajereteneejeini != null)
            { model.Caiajereteneejeini = model.EntidadAjusteEmpresa.Caiajereteneejeini.GetValueOrDefault().ToString("dd/MM/yyyy"); }
            if (model.EntidadAjusteEmpresa.Caiajereteneejefin != null)
            { model.Caiajereteneejefin = model.EntidadAjusteEmpresa.Caiajereteneejefin.GetValueOrDefault().ToString("dd/MM/yyyy"); }
            if (model.EntidadAjusteEmpresa.Caiajeretenepryaini != null)
            { model.Caiajeretenepryaini = model.EntidadAjusteEmpresa.Caiajeretenepryaini.GetValueOrDefault().ToString("dd/MM/yyyy"); }
            if (model.EntidadAjusteEmpresa.Caiajeretenepryafin != null)
            { model.Caiajeretenepryafin = model.EntidadAjusteEmpresa.Caiajeretenepryafin.GetValueOrDefault().ToString("dd/MM/yyyy"); }
            if (model.EntidadAjusteEmpresa.Caiajereteneprybini != null)
            { model.Caiajereteneprybini = model.EntidadAjusteEmpresa.Caiajereteneprybini.GetValueOrDefault().ToString("dd/MM/yyyy"); }
            if (model.EntidadAjusteEmpresa.Caiajereteneprybfin != null)
            { model.Caiajereteneprybfin = model.EntidadAjusteEmpresa.Caiajereteneprybfin.GetValueOrDefault().ToString("dd/MM/yyyy"); }
            Log.Info("Lista Empresa - ListEmpresasSTR");
            model.ListaEmpresa = this.servicioEmpresa.ListEmpresasSTR();
            Log.Info("Lista PtoMedicion - ListMePtomedicion");
            model.ListaPtoMedicion = this.servicioCalculoPorcentaje.ListCaiAjusteempresasPtomed(ConstantesCalculoPorcentajes.IdOrigenLectura);  //this.servicioCalculoPorcentaje.ListMePtomedicion("", "-1");
            Log.Info("Entidad Ajuste - GetByIdCaiAjuste");
            model.EntidadAjuste = this.servicioCalculoPorcentaje.GetByIdCaiAjuste(model.EntidadAjusteEmpresa.Caiajcodi);
            Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
            model.EntidadPresupuesto = this.servicioCalculoPorcentaje.GetByIdCaiPresupuesto(model.EntidadAjuste.Caiprscodi);
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(model);
        }

        [HttpPost]
        public string Delete(int id)
        {
            if (id > 0)
            {
                AjusteEmpresaModel model = new AjusteEmpresaModel();
                Log.Info("Elimina registro - DeleteCaiAjusteempresa");
                this.servicioCalculoPorcentaje.DeleteCaiAjusteempresa(id);
                return "true";
            }
            return "false";
        }

        public ActionResult View(int id = 0)
        {
            AjusteEmpresaModel modelo = new AjusteEmpresaModel();
            Log.Info("Lista AjusteEmpresa - GetByIdCaiAjusteempresa");
            modelo.EntidadAjusteEmpresa = this.servicioCalculoPorcentaje.GetByIdCaiAjusteempresa(id);
            return PartialView(modelo);
        }

        /// <summary>
        /// Permite exportar a un archivo excel todos los registros en pantalla de consulta
        /// </summary>
        /// <param name="stpercodi">Código del Mes de carga de datos</param>
        /// <param name="strecacodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarDataAE(int caiajcodi = 0, string caiajetipoinfo="", int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesCalculoPorcentajes.ReporteDirectorio].ToString();
                string file = this.servicioCalculoPorcentaje.GenerarFormatoCaiAjusteEmpresa(caiajcodi, caiajetipoinfo, formato, pathFile, pathLogo);
                return Json(file);
            }
            catch (Exception e)
            {
                string sMensaje = e.Message;
                return Json("-1");
            }
        }

        /// <summary>
        /// Subir excel al repositorio de archivos
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadExcel()
        {
            base.ValidarSesionUsuario();
            string sNombreArchivo = "";
            string path = ConfigurationManager.AppSettings[ConstantesCalculoPorcentajes.ReporteDirectorio].ToString();

            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreArchivo = file.FileName;
                    if (System.IO.File.Exists(path + sNombreArchivo))
                    {
                        System.IO.File.Delete(path + sNombreArchivo);
                    }
                    file.SaveAs(path + sNombreArchivo);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Procesar excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivoAE(string sarchivo, string caiajetipoinfo, int caiajcodi = 0)
        {
            base.ValidarSesionUsuario();
            AjusteEmpresaModel model = new AjusteEmpresaModel();
            model.sMensaje = "";
            model.sError = "";
            string path = ConfigurationManager.AppSettings[ConstantesCalculoPorcentajes.ReporteDirectorio].ToString();
            try
            {
                ////Elimina información de la tabla CAI_SDDP_DURACION
                //this.servicioCAI.DeleteCaiSddpDuracion();

                ////Leemo el contenido del archivo Excel
                DataSet ds = new DataSet();
                Log.Info("Genera dataset - GeneraDataset");
                ds = this.servicioCalculoPorcentaje.GeneraDataset(path + sarchivo, 1);
                //ds = this.servicioCAI.GeneraDataset(path, 1);
                int iFila = 0;
                //Lee todo el contenido del excel
                iFila = 0;
                int i = 1;
                string[][] data = new string[ds.Tables[0].Rows.Count - 4][]; // Lee todo el contenido del excel y le descontamos 4 filas hasta donde empieza la data


                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    //empieza a contar desde la fila donde hay datos
                    if (iFila < 6)
                    {
                        continue;
                    }
                    //Inicio de lectura de archivos
                    int iCaiajecodi = 0;
                    string sCaiajecodi = dtRow[i].ToString();
                    if (sCaiajecodi != null)
                    {   /*Existe  - Update*/
                        iCaiajecodi = Int32.Parse(sCaiajecodi);
                        //busca en la bd si ya hay un ajusteempresa con ese codigo
                        Log.Info("Entidad AjusteEmpresa - GetByIdCaiAjusteempresa");
                        model.EntidadAjusteEmpresa = this.servicioCalculoPorcentaje.GetByIdCaiAjusteempresa(iCaiajecodi);
                        if (model.EntidadAjusteEmpresa == null)
                        {
                            model.sError = "El identificador " + sCaiajecodi  + " no existe";
                            break;
                        }
                    }
                    else
                    {   /*Nuevo  - Insert*/
                        model.EntidadAjusteEmpresa = new CaiAjusteempresaDTO();
                        model.EntidadAjusteEmpresa.Caiajecodi = 0;
                        //Validamos la Empresa
                        string EmpreNombreXL = dtRow[i + 1].ToString();
                        try
                        {
                            Log.Info("Lista Empresa - ListEmpresasSTR");
                            COES.Dominio.DTO.Transferencias.EmpresaDTO Empresa = this.servicioEmpresa.GetByNombre(EmpreNombreXL);
                            model.EntidadAjusteEmpresa.Emprcodi = Empresa.EmprCodi;
                        }
                        catch (Exception e)
                        {
                            model.sError = "Empresa " + EmpreNombreXL + " no registrada en la BD";
                            break;
                        }
                        //Validamos el punto de medicion
                        string PtoMedicionXL = dtRow[i + 2].ToString();
                        try
                        {
                            Log.Info("Lista PtoMedicion - ListMePtomedicion");
                            MePtomedicionDTO PtoMedicion = this.servicioCalculoPorcentaje.GetMePtomedicionByNombre(model.EntidadAjusteEmpresa.Emprcodi, PtoMedicionXL);
                            model.EntidadAjusteEmpresa.Ptomedicodi = Convert.ToInt32(PtoMedicion.Ptomedicodi);
                        }
                        catch (Exception e)
                        {
                            model.sError = "PtoMedicion " + PtoMedicionXL + " no registrada en la BD o esta inactiva";
                            break;
                        }
                        model.EntidadAjusteEmpresa.Caiajetipoinfo = caiajetipoinfo;
                    }
                    //Completamos los otros atributos
                    model.EntidadAjusteEmpresa.Caiajcodi = Convert.ToInt32(caiajcodi);
                    string sCaiajereteneejeini = dtRow[i + 4].ToString().Substring(0,10);
                    string sCaiajereteneejefin = dtRow[i + 5].ToString().Substring(0, 10);
                    string sCaiajeretenepryaini = dtRow[i + 6].ToString().Substring(0, 10);
                    string sCaiajeretenepryafin = dtRow[i + 7].ToString().Substring(0, 10);
                    string sCaiajereteneprybini = dtRow[i + 8].ToString().Substring(0, 10);
                    string sCaiajereteneprybfin = dtRow[i + 9].ToString().Substring(0, 10);


                    if (sCaiajereteneejeini != "" && sCaiajereteneejeini != null)
                        model.EntidadAjusteEmpresa.Caiajereteneejeini = DateTime.ParseExact(sCaiajereteneejeini, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                    if (sCaiajereteneejefin != "" && sCaiajereteneejefin != null)
                        model.EntidadAjusteEmpresa.Caiajereteneejefin = DateTime.ParseExact(sCaiajereteneejefin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                    if (sCaiajeretenepryaini != "" && sCaiajeretenepryaini != null)
                        model.EntidadAjusteEmpresa.Caiajeretenepryaini = DateTime.ParseExact(sCaiajeretenepryaini, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                    if (sCaiajeretenepryafin != "" && sCaiajeretenepryafin != null)
                        model.EntidadAjusteEmpresa.Caiajeretenepryafin = DateTime.ParseExact(sCaiajeretenepryafin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                    if (sCaiajereteneprybini != "" && sCaiajereteneprybini != null)
                        model.EntidadAjusteEmpresa.Caiajereteneprybini = DateTime.ParseExact(sCaiajereteneprybini, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                    if (sCaiajereteneprybfin != "" && sCaiajereteneprybfin != null)
                        model.EntidadAjusteEmpresa.Caiajereteneprybfin = DateTime.ParseExact(sCaiajereteneprybfin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    model.EntidadAjusteEmpresa.Caiajeusucreacion = User.Identity.Name;
                    model.EntidadAjusteEmpresa.Caiajefeccreacion = DateTime.Now;
                    //Almacenamos la información
                    if (iCaiajecodi == 0)
                    {
                        Log.Info("Insertar registro - SaveCaiAjusteempresa");
                        this.servicioCalculoPorcentaje.SaveCaiAjusteempresa(model.EntidadAjusteEmpresa);
                    }
                    else
                    {
                        Log.Info("Actualiza registro - UpdateCaiAjusteempresa");
                        this.servicioCalculoPorcentaje.UpdateCaiAjusteempresa(model.EntidadAjusteEmpresa);
                    }

                }
                model.sMensaje = "Felicidades, la carga de información fue exitosa para los " + (iFila - 5) + " registros, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }

        }

        /// <summary>
        /// Descarga el archivo excel
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesCalculoPorcentajes.ReporteDirectorio].ToString() + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;

            return File(path, app, sFecha + "_" + file);
        }
    }
}
