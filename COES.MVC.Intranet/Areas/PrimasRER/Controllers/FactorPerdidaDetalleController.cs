using System;
using System.Linq;
using log4net;
using System.Web.Mvc;
using System.Configuration;
using COES.MVC.Intranet.Areas.PrimasRER.Models;
using COES.Servicios.Aplicacion.PrimasRER;
using System.Reflection;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.PrimasRER.Helper;
using System.Data;
using System.Globalization;
using COES.Servicios.Aplicacion.Transferencias;
using System.Collections.Generic;
using COES.MVC.Intranet.Helper;

namespace COES.MVC.Intranet.Areas.PrimasRER.Controllers
{
    public class FactorPerdidaDetalleController : BaseController
    {
        // GET: /PrimasRER/ParametroRER/

        public FactorPerdidaDetalleController()
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

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        PrimasRERAppServicio servicioPrimasRER = new PrimasRERAppServicio();
        CodigoEntregaAppServicio servicioCodigoEntrega = new CodigoEntregaAppServicio();

        /// <summary>
        /// PrimasRER.2023
        /// Mostrar la pantalla principal con los registros de un archivo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Index(int id = 0)
        {
            base.ValidarSesionUsuario();
            PrimasRERModel model = new PrimasRERModel();
            model.IdFacPerMedDTO = id;
            if (id != 0)
            {
                model.FacPerMedDTO = this.servicioPrimasRER.GetByIdRerFacPerMed(id);
            }
            else {
                model.FacPerMedDTO = new RerFacPerMedDTO();
            }
            return View(model);
        }

        /// <summary>
        /// PrimasRER.2023
        /// Permite mostrar un listado de registros de un archivo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Lista(int id = 0)
        {
            base.ValidarSesionUsuario();
            PrimasRERModel model = new PrimasRERModel();
            model.ListFacPerMedDetDTO = this.servicioPrimasRER.ListRerFacPerMedDetsByFPM(id);
            return PartialView(model);
        }

        /// <summary>
        /// Prepara una vista para editar un nuevo registro
        /// </summary>
        /// <param Rerfpdcodi="Rerfpdcodi"></param>
        /// <returns></returns>
        public PartialViewResult Edit(int Rerfpdcodi = 0)
        {
            base.ValidarSesionUsuario();
            PrimasRERModel model = new PrimasRERModel();
            model.FacPerMedDetDTO = this.servicioPrimasRER.GetByIdRerFacPerMedDet(Rerfpdcodi);
            return PartialView(model);
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
            base.ValidarSesionUsuario();
            PrimasRERModel model = new PrimasRERModel();
            model.ListFacPerMedDetDTO = this.servicioPrimasRER.ListRerFacPerMedDets().Where(det => det.Rerfpmcodi == id).ToList();
            foreach (var facPerMedDet in model.ListFacPerMedDetDTO)
            {
                this.servicioPrimasRER.DeleteRerFacPerMedDet(facPerMedDet.Rerfpdcodi);
            }
            this.servicioPrimasRER.DeleteRerFacPerMed(id);
            return "true";
        }

        /// <summary>
        /// Permite grabar los datos del formulario
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(PrimasRERModel modelo)
        {
            base.ValidarSesionUsuario();
            PrimasRERModel modelSave = new PrimasRERModel();
            
            return PartialView(modelo);
        }

        public JsonResult Update(PrimasRERModel model)
        {
            try
            {
                base.ValidarSesionUsuario();
                if (((model.FacPerMedDetDTO.Rerfpdfactperdida) >= 0 && (model.FacPerMedDetDTO.Rerfpdfactperdida <= 2)) || model.FacPerMedDetDTO.Rerfpdfactperdida == -1)
                {
                    PrimasRERModel modelUpdate = new PrimasRERModel();
                    modelUpdate.FacPerMedDetDTO = this.servicioPrimasRER.GetByIdRerFacPerMedDet(model.FacPerMedDetDTO.Rerfpdcodi);
                    modelUpdate.FacPerMedDetDTO.Rerfpdfactperdida = model.FacPerMedDetDTO.Rerfpdfactperdida;
                    this.servicioPrimasRER.UpdateRerFacPerMedDet(modelUpdate.FacPerMedDetDTO);

                    return Json(1);
                }
                else {
                    return Json(-2);
                }
            }
            catch (Exception)
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Paso 3: Permite cargar un archivo Excel
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadExcel()
        {
            base.ValidarSesionUsuario();
            string sNombreArchivo = "";
            string path = ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString();

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
        /// Paso 4: Lee datos desde el archivo excel
        /// </summary>
        /// <param name="fecDesde">Fecha de inicio de vigencia de los datos</param>
        /// <param name="fecHasta">Fecha final de la vigencia de los datos</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivo(string sarchivo, string fecDesde, string fecHasta)
        {
            base.ValidarSesionUsuario();
            PrimasRERModel model = new PrimasRERModel();
            string path = ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString();
            int iRegError = 0;
            string sMensajeError = "";
            int iRER_FPM_Codi = 0;
            try
            {
                #region Grabamos la información de la cabecera
                RerFacPerMedDTO dto = new RerFacPerMedDTO();
                dto.Rerfpmnombrearchivo = sarchivo;
                dto.Rerfpmdesde = DateTime.ParseExact(fecDesde, ConstantesPrimasRER.FormatoFecha, CultureInfo.InvariantCulture);
                dto.Rerfpmhasta = DateTime.ParseExact(fecHasta, ConstantesPrimasRER.FormatoFecha, CultureInfo.InvariantCulture);
                dto.Rerfpmusucreacion = User.Identity.Name;
                iRER_FPM_Codi = servicioPrimasRER.SaveRerFacPerMed(dto);
                #endregion

                #region Procedimiento de lectura del Excel
                List<RerFacPerMedDetDTO> lista = new List<RerFacPerMedDetDTO>();
                
                //Traemos la primera hoja del archivo
                DataSet ds = new DataSet();
                ds = this.servicioPrimasRER.GeneraDataset(path + sarchivo, 1);

                string[][] data = new string[ds.Tables[0].Rows.Count][]; // Lee el contenido del excel
                int iFila = 1;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    int iNumFila = iFila + 1;
                    iFila++;
                    string sCodigoEntrega = dtRow[0].ToString();
                    CodigoEntregaDTO dtoCodigoEntrega = this.servicioCodigoEntrega.GetByCodigoEntregaCodigo(sCodigoEntrega);
                    if (dtoCodigoEntrega == null)
                    {
                        sMensajeError += "<br>Fila:" + iNumFila + " - El Código de Entrega no existe: " + sCodigoEntrega;
                        iRegError++;
                        continue;
                    }
                    //Validamos factor de perdida
                    decimal fRER_FPD_FactPerdida = 0;
                    string sFactorPerdida = dtRow[4].ToString();
                    if (sFactorPerdida.Equals("M")) fRER_FPD_FactPerdida = -1;
                    else
                    {
                        fRER_FPD_FactPerdida = UtilPrimasRER.ValidarNumero(sFactorPerdida);
                        if (fRER_FPD_FactPerdida == 0)
                        {
                            sMensajeError += "<br>Fila:" + iNumFila + " - El valor del Factor de Perdida es incorrecto: " + sFactorPerdida;
                            iRegError++;
                            continue;
                        }
                        else if (fRER_FPD_FactPerdida < 0 || fRER_FPD_FactPerdida > 2)
                        {
                            sMensajeError += "<br>Fila:" + iNumFila + " - El valor del Factor de Perdida no esta en el rango permitido: " + sFactorPerdida;
                            iRegError++;
                            continue;
                        }
                    }
                    //Insertamos el detalle en la lista
                    RerFacPerMedDetDTO dtoDetalle = new RerFacPerMedDetDTO();
                    dtoDetalle.Rerfpmcodi = iRER_FPM_Codi;
                    dtoDetalle.Codentcodi = dtoCodigoEntrega.CodiEntrCodi;
                    dtoDetalle.Barrcodi = dtoCodigoEntrega.BarrCodi;
                    dtoDetalle.Barrnombre = dtoCodigoEntrega.BarrNombBarrTran;
                    dtoDetalle.Emprcodi = dtoCodigoEntrega.EmprCodi;
                    dtoDetalle.Equicodi = dtoCodigoEntrega.CentGeneCodi;
                    dtoDetalle.Rerfpdfactperdida = fRER_FPD_FactPerdida;
                    dtoDetalle.Rerfpdusucreacion = User.Identity.Name;
                    dtoDetalle.Rerfpdusumodificacion = User.Identity.Name;
                    lista.Add(dtoDetalle);
                }

                // Verificamos que la fecha Desde sea mayor a la fecha Hasta
                DateTime fechaInicio = DateTime.ParseExact(fecDesde, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFinal = DateTime.ParseExact(fecHasta, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (fechaInicio > fechaFinal) {
                    iRegError++;
                    sMensajeError += "<br>La 'Fecha hasta' es menor a la 'Fecha desde' ";
                }

                // Verificamos que el rango de fechas escogidas no se traslape con otras
                model.ListFacPerMedDTO = this.servicioPrimasRER.ListRerFacPerMeds().OrderBy(item => item.Rerfpmcodi).ToList(); ;
                model.FacPerMedDTO = model.ListFacPerMedDTO.Last();
                foreach (var FacPerMed in model.ListFacPerMedDTO)
                {
                    if (model.FacPerMedDTO.Rerfpmcodi != FacPerMed.Rerfpmcodi) {
                        if (FacPerMed.Rerfpmdesde <= fechaInicio && fechaInicio <= FacPerMed.Rerfpmhasta)
                        {
                            iRegError++;
                            sMensajeError += "<br>La Fecha desde se encuentra con otro rango de fechas guardadas";
                            break;
                        }
                        if (FacPerMed.Rerfpmdesde <= fechaFinal && fechaFinal <= FacPerMed.Rerfpmhasta)
                        {
                            iRegError++;
                            sMensajeError += "<br>La Fecha hasta se encuentra en otro rango de fechas guardadas";
                            break;
                        }
                        if (fechaInicio < FacPerMed.Rerfpmdesde && FacPerMed.Rerfpmhasta < fechaFinal)
                        {
                            iRegError++;
                            sMensajeError += "<br>El rago de fechas escogidos se superpone a otro rango de fechas guardadas";
                            break;
                        }
                    }
                }

                if (iRegError > 0)
                {
                    //Eliminamos la cabecera
                    servicioPrimasRER.DeleteRerFacPerMed(iRER_FPM_Codi);
                }
                else
                {
                    //No hay errores, insertamos la lista en base de datos
                    foreach (RerFacPerMedDetDTO registro in lista)
                    {
                        servicioPrimasRER.SaveRerFacPerMedDet(registro);
                    }
                    model.Mensaje = "Se han cargado correctamente " + (iFila-1) + " registros.";
                }

                #endregion
                model.RegError = iRegError;
                model.MensajeError = sMensajeError;
                model.IdRegistro = iRER_FPM_Codi;
                return Json(model);
            }
            catch (Exception e)
            {
                model.MensajeError = e.Message;
                model.RegError = 1;
                return Json(model);
            }

        }
    }
}