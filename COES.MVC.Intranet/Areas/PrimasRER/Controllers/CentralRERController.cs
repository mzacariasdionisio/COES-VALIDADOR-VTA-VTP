using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Areas.PrimasRER.Helper;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.Transferencias;
using COES.MVC.Intranet.Areas.PrimasRER.Models;
using OfficeOpenXml;
using System.IO;
using System.Globalization;
using COES.Dominio.DTO.Sic;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Configuration;
using COES.Servicios.Aplicacion.PrimasRER;
using COES.Servicios.Aplicacion.PrimasRER.Helper;
using System.Reflection;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Equipamiento;

namespace COES.MVC.Intranet.Areas.PrimasRER.Controllers
{
    public class CentralRERController : Controller
    {
        // GET: /PrimasRER/CentralRER/

        public CentralRERController()
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
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        PrimasRERAppServicio servicioPrimasRER = new PrimasRERAppServicio();
        CodigoEntregaAppServicio servicioCodigoEntrega = new CodigoEntregaAppServicio();
        BarraAppServicio servicioBarra = new BarraAppServicio();
        CentralGeneracionAppServicio servicioCentralGeneracion = new CentralGeneracionAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();
        FormatoMedicionAppServicio servicioFormatoMedicion = new FormatoMedicionAppServicio();
        EquipamientoAppServicio servicioEquipo = new EquipamientoAppServicio();

        /// <summary>
        /// Muestra la pantalla inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            #region Filtros en Centrales RER
            PrimasRERModel model = new PrimasRERModel();
            model.ListEmpresas = this.servicioEmpresa.ListaInterCodEnt();
            model.ListCentralGeneracion = this.servicioCentralGeneracion.ListaInterCodEnt();
            model.ListBarras = this.servicioBarra.ListaInterCodEnt();
            TempData["EMPRCODI3"] = new SelectList(model.ListEmpresas, "EMPRCODI", "EMPRNOMBRE");
            TempData["CENTGENECODI3"] = new SelectList(model.ListCentralGeneracion, "CENTGENECODI", "CENTGENENOMBRE");
            TempData["BARRCODI3"] = new SelectList(model.ListBarras, "BARRCODI", "BARRNOMBBARRTRAN");
            #endregion

            return View(model);
        }


        /// <summary>
        /// Muestra la lista de datos de la central RER
        /// </summary>
        /// <param name="emprcodi">Id de la empresa</param>
        /// <param name="equicodi">Id de la central</param>
        /// <param name="barrcodi">Codigo de la Barra</param>
        /// <param name="fechaInicio">Fecha de inicio de la CentralRER</param>
        /// <param name="fechaFin">Fecha final de la CentralRER</param>
        /// <param name="estado">Estado de la CentralRER</param>
        /// <param name="codEntrega"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Lista(int emprcodi = -2, int equicodi = -2, int barrcodi = -2, string fechaInicio = null, string fechaFin = null, string estado = null, string codEntrega = null)
        {
            emprcodi = emprcodi.Equals(-1) ? -2: emprcodi;
            equicodi = equicodi.Equals(-1) ? -2 : equicodi;
            barrcodi = barrcodi.Equals(-1) ? -2 : barrcodi;
            codEntrega = String.IsNullOrWhiteSpace(codEntrega) ? null : codEntrega;
            estado = (String.IsNullOrWhiteSpace(estado) || estado.Equals("TODOS")) ? null : estado;
            String dtfi = string.IsNullOrEmpty(fechaInicio) ? null : fechaInicio;
            String dtff = string.IsNullOrEmpty(fechaFin) ? null : fechaFin;

            PrimasRERModel model = new PrimasRERModel();
            model.ListCentralRER = this.servicioPrimasRER.ListByFiltros(equicodi, emprcodi, -2, dtfi, dtff, estado, codEntrega, barrcodi).OrderBy(item => item.Equinomb).ToList();
            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar la CentralRER asi como LVTP y PMPO asociados
        /// </summary>
        /// <param name="id">Id de la central RER</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int id = 0)
        {
            PrimasRERModel model = new PrimasRERModel();
            model.ListRerCentralLvtp = this.servicioPrimasRER.ListRerCentralLvtps().Where(item => item.Rercencodi == id).ToList();
            model.ListRerCentralPmpo = this.servicioPrimasRER.ListRerCentralPmpos().Where(item => item.Rercencodi == id).ToList();
            try {
                this.servicioPrimasRER.DeleteAllLvtpByRercencodi(id);
                this.servicioPrimasRER.DeleteAllPmpoByRercencodi(id);
                this.servicioPrimasRER.DeleteRerCentral(id);
                return "true";
            }
            catch (Exception ex) {
                return ex.Message;
            }
            
        }

        /// <summary>
        /// Prepara una vista para ingresar una nueva CentralRER
        /// </summary>
        /// <returns></returns>
        public ActionResult New()
        {
            PrimasRERModel model = new PrimasRERModel();
            model.Rercenfechainicio = DateTime.Now.ToString("dd/MM/yyyy");
            model.Rercenfechafin = DateTime.Now.ToString("dd/MM/yyyy");

            #region Combos desplegable de Centrales RER
            model.ListEmpresas = this.servicioEmpresa.ListaInterCodEnt();
            model.ListCentralGeneracion = this.servicioCentralGeneracion.ListaInterCodEnt();
            model.ListBarras = this.servicioBarra.ListaInterCodEnt();
            #endregion
            model.CentralRER = new RerCentralDTO();
            model.CentralRER.Ptomedicodi = -1;
            return PartialView(model);
        }

        /// <summary>
        /// Muestra los datos de una CentralRER en un popup
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult View(int id = 0)
        {
            PrimasRERModel model = new PrimasRERModel();
            model.ListCentralRER = this.servicioPrimasRER.ListByFiltros(-2, -2, -2, null, null, null, null, -2).Where(centralRER => centralRER.Rercencodi == id).ToList();
            model.CentralRER = (model.ListCentralRER.Count != 0) ? model.ListCentralRER[0] : new RerCentralDTO();
            model.Rercenfechainicio = model.CentralRER.Rercenfechainicio.ToString("dd/MM/yyyy");
            model.Rercenfechafin = model.CentralRER.Rercenfechafin.ToString("dd/MM/yyyy");

            return PartialView(model);
        }

        /// <summary>
        /// Gurdar los datos del formulario de la CentralRER
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Save(PrimasRERModel model)
        {
            try
            {
                // Verificamos que la fecha final sea mayo a la inicial
                if (model.CentralRER.Rercenfechainicio > model.CentralRER.Rercenfechafin)
                {
                    model.Resultado = "-2";
                    model.MensajeError = "La fecha inicial es mayor a la fecha fin";
                    return Json(model);
                }

                model.MensajeError = "En la presente central, el intervalo seleccionado se cruza con el registro previo: ";

                String fechaInicial = model.Rercenfechainicio;
                String fechaFinal = model.Rercenfechafin;

                List<RerCentralDTO> listaCentralesRER = this.servicioPrimasRER.ListByEquiEmprFecha(-2, model.CentralRER.Equicodi, model.CentralRER.Emprcodi, fechaInicial, fechaFinal);
                if (listaCentralesRER != null && listaCentralesRER.Count != 0)
                {
                    for (int l = 0; l < listaCentralesRER.Count; l++)
                    {
                        model.MensajeError += "<br>[" + listaCentralesRER[l].Rercenfechainicio.ToString("dd/MM/yyyy") + " - " + listaCentralesRER[l].Rercenfechafin.ToString("dd/MM/yyyy") + "]";
                    }
                    model.Resultado = "-2";
                    return Json(model);
                }

                //return Json(-1);
                char delimitador = '-';
                string[] LVTEAArrayString = model.codigosLVTEA.Split(delimitador);
                int[] codigosLVTEAArray = Array.ConvertAll(LVTEAArrayString, int.Parse);

                string[] PMPOArrayString = model.codigosPMPO.Split(delimitador);
                int[] codigosPMPOArray = Array.ConvertAll(PMPOArrayString, int.Parse);

                DateTime fechaActual = DateTime.Now;
                string usuarioActual = User.Identity.Name;

                //Actualizando la CentralRER
                RerCentralDTO centralRERDto = new RerCentralDTO();
                EqEquipoDTO EqEquipo = new EqEquipoDTO();
                EqEquipo = this.servicioEquipo.GetByIdEqEquipo(model.CentralRER.Equicodi);

                centralRERDto.Emprcodi = model.CentralRER.Emprcodi;
                centralRERDto.Equicodi = model.CentralRER.Equicodi;
                centralRERDto.Famcodi = EqEquipo.Famcodi;
                centralRERDto.Rercenestado = model.CentralRER.Rercenestado;
                centralRERDto.Rercenfechainicio = DateTime.ParseExact(fechaInicial, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                centralRERDto.Rercenfechafin = DateTime.ParseExact(fechaFinal, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                centralRERDto.Rercenenergadj = model.CentralRER.Rercenenergadj;
                centralRERDto.Rercenprecbase = model.CentralRER.Rercenprecbase;
                centralRERDto.Rerceninflabase = model.CentralRER.Rerceninflabase;
                centralRERDto.Rercendesccontrato = model.CentralRER.Rercendesccontrato;
                centralRERDto.Codentcodi = model.CentralRER.Codentcodi;
                centralRERDto.Pingnombre = model.CentralRER.Pingnombre == "-1" || model.CentralRER.Pingnombre == null ? null: model.CentralRER.Pingnombre;
                centralRERDto.Ptomedicodi = model.CentralRER.Ptomedicodi == -1 ? null : model.CentralRER.Ptomedicodi;
                centralRERDto.Rercenfeccreacion = fechaActual;
                centralRERDto.Rercenfecmodificacion = fechaActual;
                centralRERDto.Rercenusumodificacion = usuarioActual;
                centralRERDto.Rercenusucreacion = usuarioActual;

                int Rercencodi = this.servicioPrimasRER.SaveRerCentral(centralRERDto);

                //Guardammos las Central/Unidad LVTP
                RerCentralLvtpDTO centralLvtpDTO = new RerCentralLvtpDTO();
                centralLvtpDTO.Rercencodi = Rercencodi;
                centralLvtpDTO.Rerctpfeccreacion = fechaActual;
                centralLvtpDTO.Rerctpusucreacion = usuarioActual;
                for (int i = 1; i < codigosLVTEAArray.Count(); i++)
                {
                    centralLvtpDTO.Equicodi = codigosLVTEAArray[i];
                    this.servicioPrimasRER.SaveRerCentralLvtp(centralLvtpDTO);
                }

                //Guardammos las Central PMPO
                RerCentralPmpoDTO centralPmpoDTO = new RerCentralPmpoDTO();
                centralPmpoDTO.Rercencodi = Rercencodi;
                centralPmpoDTO.Rercpmfeccreacion = fechaActual;
                centralPmpoDTO.Rercpmusucreacion = usuarioActual;
                for (int j = 1; j < codigosPMPOArray.Count(); j++)
                {
                    centralPmpoDTO.Ptomedicodi = codigosPMPOArray[j];
                    this.servicioPrimasRER.SaveRerCentralPmpo(centralPmpoDTO);
                }

                model.Resultado = "1";
                return Json(model);

            }
            catch (Exception)
            {
                model.Resultado = "-1";
                return Json(model);
            }
        }

        /// <summary>
        /// Prepara una vista para editar una Central RER
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            PrimasRERModel model = new PrimasRERModel();
            model.ListCentralRER = this.servicioPrimasRER.ListByFiltros(-2, -2, -2, null, null, null, null, -2).Where(item => item.Rercencodi == id).ToList();
            model.CentralRER = model.ListCentralRER[0];

            //Valores que se listan en los combos desplegables
            model.ListaCodigoEntrega = this.servicioCodigoEntrega.ListCodigoEntrega().Where(item => (item.EmprCodi == model.CentralRER.Emprcodi)).ToList();

            //Valores que pertenecen a la CentralRER seleccionada y se visualizan en tablas
            model.ListRerCentralLvtp = this.servicioPrimasRER.ListRerCentralLvtpsByRercencodi(model.CentralRER.Rercencodi);
            model.ListRerCentralPmpo = this.servicioPrimasRER.ListRerCentralPmposByRercencodi(model.CentralRER.Rercencodi);
            foreach (var reg in model.ListRerCentralPmpo)
            {
                var desc = (reg.Ptomedidesc ?? "").Trim();
                desc = desc.Replace("CH_", "");
                desc = desc.Replace("_B", "");
                desc = desc.Replace("_H", "");
                desc = desc.Replace("_E", "");
                desc = desc.Replace("_S", "");

                if (reg.Fenergcodi == ConstantesPrimasRER.FenergcodiAgua) desc = "CH " + desc;
                if (reg.Fenergcodi == ConstantesPrimasRER.FenergcodiEolica) desc = "GenEo " + desc;
                if (reg.Fenergcodi == ConstantesPrimasRER.FenergcodiSolar) desc = "GenSol " + desc;
                if (reg.Tgenercodi == ConstantesPrimasRER.TgenercodiTermo)
                {
                    if (reg.Grupotipocogen == ConstantesPrimasRER.SI)
                    {
                        desc = "CoGen " + desc;
                    }
                    else
                    {
                        if (reg.Fenergcodi == ConstantesPrimasRER.FenergcodiBiogas) desc = "GenBio " + desc;
                        if (reg.Fenergcodi == ConstantesPrimasRER.FenergcodiGas) desc = "CT " + desc;
                    }
                }
                reg.Ptomedidesc = "[" + reg.Ptomedicodi.ToString() + "] " + desc;
            }
            return PartialView(model);
        }

        /// <summary>
        /// Actualizar los datos del formulario de la CentralRER
        /// </summary>
        /// <param name="model">Contiene los datos de la CentralRER del formulario y los códigos LVTP y PMPO</param>
        /// <returns></returns>
        public JsonResult Update(PrimasRERModel model)
        {
            try
            {
                String fechaInicial = model.Rercenfechainicio;
                String fechaFinal = model.Rercenfechafin;

                List<RerCentralDTO> listaCentralesRER = this.servicioPrimasRER.ListByEquiEmprFecha(model.CentralRER.Rercencodi, model.CentralRER.Equicodi, model.CentralRER.Emprcodi, fechaInicial, fechaFinal);
                if (listaCentralesRER != null && listaCentralesRER.Count != 0)
                {
                    model.MensajeError = "En la presente central, el intervalo seleccionado se cruza con el registro previo: ";
                    for (int l = 0; l< listaCentralesRER.Count; l++) {
                        model.MensajeError += "<br>[" + listaCentralesRER[l].Rercenfechainicio.ToString("dd/MM/yyyy") + " - " + listaCentralesRER[l].Rercenfechafin.ToString("dd/MM/yyyy") + "]";
                    }
                    model.Resultado = "-2";
                    return Json(model);
                }

                //Eliminando las Central/Unidad y Central PMPO guardadas anteriormente
                this.servicioPrimasRER.DeleteAllLvtpByRercencodi(model.CentralRER.Rercencodi);
                this.servicioPrimasRER.DeleteAllPmpoByRercencodi(model.CentralRER.Rercencodi);

                //Actualizando la CentralRER
                RerCentralDTO centralRERDto = new RerCentralDTO();
                centralRERDto = this.servicioPrimasRER.GetByIdRerCentral(model.CentralRER.Rercencodi);
                
                EqEquipoDTO EqEquipo = new EqEquipoDTO();
                EqEquipo = this.servicioEquipo.GetByIdEqEquipo(model.CentralRER.Equicodi);
                centralRERDto.Famcodi = EqEquipo.Famcodi;

                centralRERDto.Rercenestado = model.CentralRER.Rercenestado;
                centralRERDto.Rercenfechainicio = DateTime.ParseExact(fechaInicial, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                centralRERDto.Rercenfechafin = DateTime.ParseExact(fechaFinal, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                centralRERDto.Rercenenergadj = model.CentralRER.Rercenenergadj;
                centralRERDto.Rercenprecbase = model.CentralRER.Rercenprecbase;
                centralRERDto.Rerceninflabase = model.CentralRER.Rerceninflabase;
                centralRERDto.Rercendesccontrato = model.CentralRER.Rercendesccontrato;
                centralRERDto.Codentcodi = model.CentralRER.Codentcodi;
                centralRERDto.Pingnombre = model.CentralRER.Pingnombre == "-1" ? null : model.CentralRER.Pingnombre;
                centralRERDto.Ptomedicodi = model.CentralRER.Ptomedicodi == -1 ? null : model.CentralRER.Ptomedicodi;
                centralRERDto.Rercenfecmodificacion = DateTime.Now;
                centralRERDto.Rercenusumodificacion = User.Identity.Name;

                this.servicioPrimasRER.UpdateRerCentral(centralRERDto);

                // Obtenemos los códigos de LVTP y PMPO 
                char delimitador = '-';
                string[] LVTEAArrayString = model.codigosLVTEA.Split(delimitador);
                int[] codigosLVTEAArray = Array.ConvertAll(LVTEAArrayString, int.Parse);

                string[] PMPOArrayString = model.codigosPMPO.Split(delimitador);
                int[] codigosPMPOArray = Array.ConvertAll(PMPOArrayString, int.Parse);

                //Guardammos las Central/Unidad LVTP
                RerCentralLvtpDTO centralLvtpDTO = new RerCentralLvtpDTO();
                centralLvtpDTO.Rercencodi = model.CentralRER.Rercencodi;
                centralLvtpDTO.Rerctpfeccreacion = DateTime.Now;
                centralLvtpDTO.Rerctpusucreacion = User.Identity.Name;
                for (int i = 1;i< codigosLVTEAArray.Count(); i++)
                {
                    centralLvtpDTO.Equicodi = codigosLVTEAArray[i];
                    this.servicioPrimasRER.SaveRerCentralLvtp(centralLvtpDTO);
                }

                //Guardammos las Central PMPO
                RerCentralPmpoDTO centralPmpoDTO = new RerCentralPmpoDTO();
                centralPmpoDTO.Rercencodi = model.CentralRER.Rercencodi;
                centralPmpoDTO.Rercpmfeccreacion = DateTime.Now;
                centralPmpoDTO.Rercpmusucreacion = User.Identity.Name;
                for (int j = 1; j < codigosPMPOArray.Count(); j++)
                {
                    centralPmpoDTO.Ptomedicodi = codigosPMPOArray[j];
                    this.servicioPrimasRER.SaveRerCentralPmpo(centralPmpoDTO);
                }
                model.Resultado = "1";

                return Json(model);

            }
            catch (Exception)
            {
                model.Resultado = "-1";
                return Json(model);
            }
        }

        /// <summary>
        /// Prepara una lista de centrales y Cargo Prima Rer, los cuales serán mostrados en combos desplegables.
        /// </summary>
        /// <param name="emprcodi">Id de la empresa</param>
        /// <returns></returns>
        public ActionResult ObtenerCentral(int emprcodi)
        {
            PrimasRERModel model = new PrimasRERModel();
            model.ListCentralGeneracion = this.servicioCentralGeneracion.ListaCentralByEmpresa(emprcodi);       // Central

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Prepara el combo de código de entrega y el fecha inicial de la siguiente CentralRER a crear
        /// </summary>
        /// <param name="emprcodi">Id de la empresa</param>
        /// <param name="equicodi">Id de la central</param>
        /// <returns></returns>
        public ActionResult ObtenerCodEntrega(int emprcodi, int equicodi)
        {
            PrimasRERModel model = new PrimasRERModel();
            // Datos del combo de Código de Entrega
            model.ListaCodigoEntrega = this.servicioCodigoEntrega.ListCodigoEntrega().Where(item => (item.EmprCodi == emprcodi)).ToList();
            
            // Obteniendo la siguiente fecha inicial si se escoge nuevamente el par empresa-central
            model.ListCentralRER = this.servicioPrimasRER.ListRerCentrales().Where(item => (item.Emprcodi == emprcodi) && (item.Equicodi == equicodi)).OrderBy(item => item.Rercenfechainicio).ToList();
            model.Rercenfechainicio = "";
            if (model.ListCentralRER.Count != 0) {
                // Agrego un dia a la fecha final para crear otra fecha que no se intersecte con las demas fechas
                model.Rercenfechainicio = model.ListCentralRER.Last().Rercenfechafin.AddDays(1).ToString("dd/MM/yyyy");
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Prepara el nombre de la Barra Transferencia
        /// </summary>
        /// <param name="emprcodi">Id de la empresa</param>
        /// <param name="equicodi">Id de la central</param>
        /// <param name="codiEntrCodi">Id del código de entrega</param>
        /// <returns></returns>
        public ActionResult ObtenerBarrTransferencia(int emprcodi, int equicodi, int codiEntrCodi)
        {
            PrimasRERModel model = new PrimasRERModel();
            model.CodigoEntrega = new CodigoEntregaDTO();
            model.ListaCodigoEntrega = this.servicioCodigoEntrega.ListCodigoEntrega().Where(item => (item.EmprCodi == emprcodi) && (item.CodiEntrCodi == codiEntrCodi)).ToList();
            if (model.ListaCodigoEntrega.Count == 0 || model.ListaCodigoEntrega == null) {
                model.CodigoEntrega.BarrNombBarrTran = "";
            }
            else { 
                model.CodigoEntrega = model.ListaCodigoEntrega[0];
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Prepara los datos del combo descplegable de la central en PMPO
        /// </summary>
        /// <param name="fechaInicial">Fecha inicial de la CentralRER</param>
        /// <param name="fechaFinal">Fecha final de la CentralRER</param>
        /// <returns></returns>
        public ActionResult ObtenerBarrasPMPO(string fechaInicial, string fechaFinal)
        {
            PrimasRERModel model = new PrimasRERModel();
            model.ListBarraMedicion = this.servicioFormatoMedicion.ListarBarrasPMPO(fechaInicial, fechaFinal);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Prepara los datos del combo descplegable de la central en PMPO
        /// </summary>
        /// <param name="emprcodi">Id de la empresa</param>
        /// <returns></returns>
        public ActionResult ObtenerInformacionLVTP(int emprcodi)
        {
            PrimasRERModel model = new PrimasRERModel();
            try {
                // Información LVTP
                model.ListCentralUnidad = this.servicioCentralGeneracion.ListaCentralUnidadByEmpresa(emprcodi);     // Central/Unidad
                model.ListCentralUnidadLVTP = this.servicioTransfPotencia.GetCentralUnidadByEmpresa(emprcodi);      // Central/Unidad
                for (int i = 0; i < model.ListCentralUnidadLVTP.Count; i++) {
                    model.ListCentralUnidadLVTP[i].Ipefrdunidadnomb = "[" + model.ListCentralUnidadLVTP[i].Cenequicodi + "] " + model.ListCentralUnidadLVTP[i].Cenequinomb;
                }
                model.ListVtpPeajeIngreso = this.servicioTransfPotencia.ListCargoPrimaRER(emprcodi);                // Cargo Prima RER   
                model.Resultado = "1";
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                model.Resultado = "-1";
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Prepara los datos del combo descplegable de la central en PMPO
        /// </summary>
        /// <param name="emprcodi">Id de la empresa</param>
        /// <param name="fechaInicial">Fecha inicial de la CentralRER</param>
        /// <param name="fechaFinal">Fecha final de la CentralRER</param>
        /// <returns></returns>
        public ActionResult ObtenerInformacionPMPO(int emprcodi, string fechaInicial, string fechaFinal, int? ptomediCodi)
        {
            PrimasRERModel model = new PrimasRERModel();
            try
            {
                // Información PMPO
                // Central
                model.ListCentralMedicion = this.servicioFormatoMedicion.ListarCentralesPMPO(emprcodi);

                // Renombramos a Ptomedidesc
                foreach (var reg in model.ListCentralMedicion)
                {
                    var desc = (reg.Ptomedidesc ?? "").Trim();
                    desc = desc.Replace("CH_", "");
                    desc = desc.Replace("_B", "");
                    desc = desc.Replace("_H", "");
                    desc = desc.Replace("_E", "");
                    desc = desc.Replace("_S", "");

                    if (reg.Fenergcodi == ConstantesPrimasRER.FenergcodiAgua) desc = "CH " + desc;
                    if (reg.Fenergcodi == ConstantesPrimasRER.FenergcodiEolica) desc = "GenEo " + desc;
                    if (reg.Fenergcodi == ConstantesPrimasRER.FenergcodiSolar) desc = "GenSol " + desc;
                    if (reg.Tgenercodi == ConstantesPrimasRER.TgenercodiTermo)
                    {
                        if (reg.Grupotipocogen == ConstantesPrimasRER.SI)
                        {
                            desc = "CoGen " + desc;
                        }
                        else
                        {
                            if (reg.Fenergcodi == ConstantesPrimasRER.FenergcodiBiogas) desc = "GenBio " + desc;
                            if (reg.Fenergcodi == ConstantesPrimasRER.FenergcodiGas) desc = "CT " + desc;
                        }
                    }
                    reg.Ptomedidesc = "[" + reg.Ptomedicodi.ToString() + "] " + desc;
                }

                // Barra
                model.ListBarraMedicion = this.servicioFormatoMedicion.ListarBarrasPMPO(fechaInicial, fechaFinal);

                var indexElemento = model.ListBarraMedicion.FindIndex(item => item.Ptomedicodi == ptomediCodi);
                if (indexElemento != -1)
                {
                    // Extraer el elemento
                    var elemento = model.ListBarraMedicion[indexElemento];
                    // Remover el elemento de su posición actual
                    model.ListBarraMedicion.RemoveAt(indexElemento);
                    // Insertar el elemento al inicio
                    model.ListBarraMedicion.Insert(0, elemento);
                }

                model.Resultado = "1";
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                model.Resultado = "-1";
                return Json(model, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// Permite exportar un archivo excel de todos los registros
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarExcel()
        {
            PrimasRERModel model = new PrimasRERModel();
            try
            {
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();

                model.ListCentralRER = this.servicioPrimasRER.ListByFiltros(-2, -2, -2, null, null, null, null, -2).OrderBy(item => item.Equinomb).ToList();

                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteCentralRERExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteCentralRERExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        ws.Cells[2, 3].Value = "LISTA DE CENTRALES RER";
                        ExcelRange rg = ws.Cells[2, 3, 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "CENTRAL GENERACION";
                        ws.Cells[5, 3].Value = "BARRA TRANSFERENCIA";
                        ws.Cells[5, 4].Value = "EMPRESA";
                        ws.Cells[5, 5].Value = "INICIO CONTRATO";
                        ws.Cells[5, 6].Value = "FIN CONTRATO";
                        ws.Cells[5, 7].Value = "ENERGIA ADJ.";
                        ws.Cells[5, 8].Value = "PRECIO BASE";
                        ws.Cells[5, 9].Value = "INFLACIÓN BASE";
                        ws.Cells[5, 10].Value = "ESTADO";

                        rg = ws.Cells[5, 2, 5, 10];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int row = 6;
                        foreach (var item in model.ListCentralRER)
                        {
                            ws.Cells[row, 2].Value = (item.Equinomb != null) ? item.Equinomb.ToString() : string.Empty;
                            ws.Cells[row, 3].Value = (item.Barrbarratransferencia != null) ? item.Barrbarratransferencia.ToString() : string.Empty;
                            ws.Cells[row, 4].Value = (item.Emprnomb != null) ? item.Emprnomb.ToString() : string.Empty;

                            ws.Cells[row, 5].Value = (item.Rercenfechainicio != null) ? item.Rercenfechainicio.ToString("dd/MM/yyyy") : string.Empty;
                            ws.Cells[row, 6].Value = (item.Rercenfechafin != null) ? item.Rercenfechafin.ToString("dd/MM/yyyy") : string.Empty;
                            ws.Cells[row, 7].Value = item.Rercenenergadj;
                            ws.Cells[row, 8].Value = item.Rercenprecbase;
                            ws.Cells[row, 9].Value = item.Rerceninflabase;
                            if (item.Rercenestado != null)
                            {
                                if (item.Rercenestado.ToString().Equals("A")) ws.Cells[row, 10].Value = "Activo";
                                else ws.Cells[row, 10].Value = "Inactivo";
                            }
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
                        ws.View.FreezePanes(6, 10);
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
                model.Resultado = "1";
            }
            catch (FileNotFoundException e1)
            {
                model.Resultado = "-2";
                model.Mensaje = e1.Message;
                model.Detalle = e1.StackTrace;
                Log.Error(NameController, e1);
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Descarga el archivo excel
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult AbrirExcel()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteCentralRERExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteCentralRERExcel);
        }

    }
}