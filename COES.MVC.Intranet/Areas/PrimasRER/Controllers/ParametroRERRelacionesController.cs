using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Web.Mvc;
using COES.MVC.Intranet.Areas.PrimasRER.Models;
using COES.Servicios.Aplicacion.PrimasRER;
using System.Reflection;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;

namespace COES.MVC.Intranet.Areas.PrimasRER.Controllers
{
    public class ParametroRERRelacionesController : Controller
    {
        CodigoRetiroAppServicio servicioCodigoRetiro = new CodigoRetiroAppServicio();
        // GET: /PrimasRER/ParametroRERRelaciones/

        public ParametroRERRelacionesController()
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


        /// <summary>
        /// Muestra la vista inicial de las relaciones de Relaciones de Central RER y cod. retiros
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int Rerpprcodi = 0)
        {
            PrimasRERModel model = new PrimasRERModel();
            model.ListRerCentralCodRetiro = new List<RerCentralCodRetiroDTO>();
            model.ParametroRER = this.servicioPrimasRER.GetByIdRerParametroPrima(Rerpprcodi);
            model.AnioVersion = this.servicioPrimasRER.GetByIdRerAnioVersion(model.ParametroRER.Reravcodi);
            model.ListRerCentralCodRetiro = this.servicioPrimasRER.ListCantidadByRerpprcodi(Rerpprcodi);

            return PartialView(model);
        }

        /// <summary>
        /// PrimasRER.2023
        /// Muestra la vista inicial de Central RER y cod. retiros
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult New(int Rerpprcodi = 0)
        {
            PrimasRERModel model = new PrimasRERModel();
            model.IdRerCentralCodRetiro = Rerpprcodi;
            model.ListCentralRER = this.servicioPrimasRER.ListCentralREREmpresas().OrderBy(empresa => empresa.Emprnomb).ToList();
            
            return PartialView(model);
        }

        /// <summary>
        /// Actualizar un mes de una version de un año tarifario
        /// </summary>
        /// <param name="emprcodi">Id de la Empresa</param>
        /// <param name="Rerpprcodi">Id del parámetro RER</param>
        /// <returns></returns>
        public ActionResult ObtenerCentralRERCodRetiro(int emprcodi, int Rerpprcodi) {
            PrimasRERModel model = new PrimasRERModel();

            model.ParametroRER = this.servicioPrimasRER.GetByIdRerParametroPrima(Rerpprcodi);
            model.AnioVersion = this.servicioPrimasRER.GetByIdRerAnioVersion(model.ParametroRER.Reravcodi);
            int anio = model.AnioVersion.Reravaniotarif;
            if (model.ParametroRER.Rerpprmes < 5)
            {
                anio += 1;
            }

            DateTime fechaInicial = new DateTime(anio, model.ParametroRER.Rerpprmes, 1);
            DateTime fechaFinal = new DateTime(anio, model.ParametroRER.Rerpprmes, DateTime.DaysInMonth(anio, model.ParametroRER.Rerpprmes));
            string fechaInicialFormateada = fechaInicial.ToString("dd/MM/yyyy");
            string fechaFinalFormateada = fechaFinal.ToString("dd/MM/yyyy");
            model.ListCentralRER = this.servicioPrimasRER.ListByEquiEmprFecha(-2,-2, emprcodi, fechaInicialFormateada, fechaFinalFormateada).OrderBy(equipo=>equipo.Equinomb).ToList();
            model.ListaCodigoRetiro = this.servicioCodigoRetiro.ListCodRetirosByEmpresaYFecha(emprcodi, fechaInicialFormateada, fechaFinalFormateada).OrderBy(codRetiro => codRetiro.SoliCodiRetiCodigo).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// PrimasRER.2023
        /// Actualizar un mes de una version de un año tarifario
        /// </summary>
        /// <param name="rercencodi">Id de la CentralRER</param>
        /// <param name="codigosRetiroRelaciones">Id de los códigos de retiro relacionados a la CentralRER</param>
        /// <param name="rerpprcodi">Id del Parámetro Prima RER</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Save(int rercencodi, string codigosRetiroRelaciones, int rerpprcodi) 
        {
            try
            {
                PrimasRERModel model = new PrimasRERModel();
                List<RerCentralCodRetiroDTO> exiteRerCentralCodRetiro = new List<RerCentralCodRetiroDTO>();
                exiteRerCentralCodRetiro = this.servicioPrimasRER.ListRerCentralCodRetiros().Where(det => (det.Rercencodi == rercencodi) && (det.Rerpprcodi == rerpprcodi)).ToList(); ;

                if (exiteRerCentralCodRetiro.Count!=0 || exiteRerCentralCodRetiro==null) {
                    model.Resultado = "-2";
                    return Json(model);
                }
                char delimitador = '-';
                string[] codigosArrayString = codigosRetiroRelaciones.Split(delimitador);
                int[] codigosRetiro = Array.ConvertAll(codigosArrayString, int.Parse);

                RerCentralCodRetiroDTO RerCentralCodRetiro = new RerCentralCodRetiroDTO();
                string usuarioActual = User.Identity.Name;
                DateTime fechaActual = DateTime.Now;

                RerCentralCodRetiro.Rerccrusucreacion = usuarioActual;
                RerCentralCodRetiro.Rerccrfeccreacion = fechaActual;
                RerCentralCodRetiro.Rercencodi = rercencodi;
                RerCentralCodRetiro.Rerpprcodi = rerpprcodi;
                for (int i = 1; i<codigosRetiro.Count(); i++) {
                    RerCentralCodRetiro.Coresocodi = codigosRetiro[i];
                    this.servicioPrimasRER.SaveRerCentralCodRetiro(RerCentralCodRetiro);
                }
                model.Resultado = "1";
                return Json(model);
            }
            catch (Exception)
            {
                return Json("-1");
            }
        }

        /// <summary>
        /// Editar la relación de una CentralRER y sus códigos de retiro en un mes
        /// </summary>
        /// <param name="Rerpprcodi">Id del Párámetro Prima RER</param>
        /// <param name="Rercencodi">Id de la CentralRER</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Edit(int Rerpprcodi = 0, int Rercencodi = 0)
        {
            PrimasRERModel model = new PrimasRERModel();
            model.ParametroRER = this.servicioPrimasRER.GetByIdRerParametroPrima(Rerpprcodi);
            model.AnioVersion = this.servicioPrimasRER.GetByIdRerAnioVersion(model.ParametroRER.Reravcodi);

            int anio = model.AnioVersion.Reravaniotarif;
            if (model.ParametroRER.Rerpprmes < 5)
            {
                anio += 1;
            }

            DateTime fechaInicial = new DateTime(anio, model.ParametroRER.Rerpprmes, 1);
            DateTime fechaFinal = fechaInicial.AddMonths(1).AddDays(-1);
            string fechaInicialFormateada = fechaInicial.ToString("dd/MM/yyyy");
            string fechaFinalFormateada = fechaFinal.ToString("dd/MM/yyyy");

            model.IdRerCentralCodRetiro = Rerpprcodi;
            model.CentralRER = this.servicioPrimasRER.GetByIdRerCentral(Rercencodi);
            model.ListCentralRER = this.servicioPrimasRER.ListByFiltros(-2, -2, -2, null, null, null, null, -2).Where(item => item.Rercencodi == Rercencodi).ToList();

            //Datos del combo desplegable de codigos de retiro
            model.ListaCodigoRetiro = this.servicioCodigoRetiro.ListCodRetirosByEmpresaYFecha(model.CentralRER.Emprcodi, fechaInicialFormateada, fechaFinalFormateada);
            
            model.ListaCodigoRetiroTabla = new List<CodigoRetiroDTO>();
            List<RerCentralCodRetiroDTO> ListRerCentralCodRetiro = new List<RerCentralCodRetiroDTO>();
            ListRerCentralCodRetiro = this.servicioPrimasRER.ListRerCentralCodRetiros().Where(det => det.Rerpprcodi == Rerpprcodi && det.Rercencodi == Rercencodi).ToList();
            
            foreach (RerCentralCodRetiroDTO RerCentralCodRetiro in ListRerCentralCodRetiro) {
                CodigoRetiroDTO codigoRetiro = model.ListaCodigoRetiro.Where(item => item.SoliCodiRetiCodi == RerCentralCodRetiro.Coresocodi).ToList().FirstOrDefault();
                if (codigoRetiro != null) { 
                    model.ListaCodigoRetiroTabla.Add(codigoRetiro);
                }
            }

            return PartialView(model);
        }

        /// <summary>
        /// PrimasRER.2023
        /// Actualizar en un mes, las relaciones de CentralRER con sus códigos de retiro
        /// </summary>
        /// <param name="rercencodi">Id de la CentralRER</param>
        /// <param name="codigosRetiroRelaciones">Id de los códigos de retiro de solicitud</param>
        /// <param name="rerpprcodi">Id del Parámetro Prima RER</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Update(int rercencodi = 0, string codigosRetiroRelaciones = "", int rerpprcodi = 0)
        {
            try
            {
                PrimasRERModel model = new PrimasRERModel();

                this.servicioPrimasRER.DeleteAllRerCentralCodRetiroByRerpprcodiRercencodi(rerpprcodi, rercencodi);
                char delimitador = '-';
                string[] codigosArrayString = codigosRetiroRelaciones.Split(delimitador);
                int[] codigosRetiro = Array.ConvertAll(codigosArrayString, int.Parse);

                RerCentralCodRetiroDTO RerCentralCodRetiro = new RerCentralCodRetiroDTO();
                string usuarioActual = User.Identity.Name;
                DateTime fechaActual = DateTime.Now;

                RerCentralCodRetiro.Rerccrusucreacion = usuarioActual;
                RerCentralCodRetiro.Rerccrfeccreacion = fechaActual;
                RerCentralCodRetiro.Rercencodi = rercencodi;
                RerCentralCodRetiro.Rerpprcodi = rerpprcodi;
                for (int i = 1; i < codigosRetiro.Count(); i++)
                {
                    RerCentralCodRetiro.Coresocodi = codigosRetiro[i];
                    this.servicioPrimasRER.SaveRerCentralCodRetiro(RerCentralCodRetiro);
                }
                model.Resultado = "1";
                return Json(model);

            }
            catch (Exception)
            {
                return Json("-1");
            }
        }

        /// <summary>
        /// Eliminar la relación de la CentralRER y los códigos de retiro
        /// </summary>
        /// <param name="Rerpprcodi">Id del ParámetroRER</param>
        /// <param name="Rercencodi">Id de la CentralRER</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int Rerpprcodi, int Rercencodi)
        {
            try
            {
                PrimasRERModel model = new PrimasRERModel();
                this.servicioPrimasRER.DeleteAllRerCentralCodRetiroByRerpprcodiRercencodi(Rerpprcodi, Rercencodi);
                model.Resultado = "1";
                return Json(model);
            }
            catch (Exception)
            {
                return Json("-1");
            }
        }

        /// <summary>
        /// Copiar las relaciones de CentralRER con códigos de retiro del mes anterior al mes actual
        /// </summary>
        /// <param name="Rerpprcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult copiarRelacionesMesAnterior(int Rerpprcodi)
        {
            try
            {
                PrimasRERModel model = new PrimasRERModel();
                model.ListRerCentralCodRetiro = new List<RerCentralCodRetiroDTO>();
                model.ListCentralRER = new List<RerCentralDTO>();

                RerParametroPrimaDTO parametroRERActual = this.servicioPrimasRER.GetByIdRerParametroPrima(Rerpprcodi);
                model.AnioVersion = this.servicioPrimasRER.GetByIdRerAnioVersion(parametroRERActual.Reravcodi);

                int mesAnterior = parametroRERActual.Rerpprmes == 1 ? 12 : parametroRERActual.Rerpprmes - 1;
                RerParametroPrimaDTO parametroRerAnterior = this.servicioPrimasRER.ListRerParametroPrimas().Where(item => (item.Reravcodi == parametroRERActual.Reravcodi) && (item.Rerpprmes == mesAnterior)).ToList().First();

                DateTime fechaInicial = new DateTime(model.AnioVersion.Reravaniotarif, parametroRERActual.Rerpprmes, 1);
                DateTime fechaFinal = fechaInicial.AddMonths(1).AddDays(-1);
                string fechaInicialFormateada = fechaInicial.ToString("dd/MM/yyyy");
                string fechaFinalFormateada = fechaFinal.ToString("dd/MM/yyyy");


                List<RerCentralCodRetiroDTO> listRerCentralCodRetiroMesAnterior = new List<RerCentralCodRetiroDTO>();
                listRerCentralCodRetiroMesAnterior = this.servicioPrimasRER.ListRerCentralCodRetiros().Where(det => det.Rerpprcodi == parametroRerAnterior.Rerpprcodi).ToList();

                // Eliminamos las relaciones actuales
                this.servicioPrimasRER.DeleteAllRerCentralCodRetiroByRerpprcodiRercencodi(parametroRERActual.Rerpprcodi, -2);

                foreach (RerCentralCodRetiroDTO RerCentralCodRetiro in listRerCentralCodRetiroMesAnterior) {
                    bool is_valid_centralRER = false;
                    bool is_valid_coresocodi = false;
                    model.ListCentralRER = this.servicioPrimasRER.ListByEquiEmprFecha(-2, -2, -2, fechaInicialFormateada, fechaFinalFormateada).Where(item => item.Rercencodi == RerCentralCodRetiro.Rercencodi).ToList();

                    #region Validamos que exista la central RER en ese mes
                    if (model.ListCentralRER.Count != 0 && model.ListCentralRER != null)
                    {
                        is_valid_centralRER = true;
                    }
                    #endregion

                    model.CentralRER = this.servicioPrimasRER.GetByIdRerCentral(RerCentralCodRetiro.Rercencodi);

                    model.ListaCodigoRetiro = this.servicioCodigoRetiro.ListCodRetirosByEmpresaYFecha(model.CentralRER.Emprcodi, fechaInicialFormateada, fechaFinalFormateada).Where(item => item.SoliCodiRetiCodi == RerCentralCodRetiro.Coresocodi).ToList();

                    #region Validamos que exista el codigo de retiro (coresocodi)
                    if (model.ListaCodigoRetiro.Count != 0 && model.ListaCodigoRetiro != null)
                    {
                        is_valid_coresocodi = true;
                    }
                    #endregion

                    #region Si las validaciones son correstas, lo guardo en db
                    if (is_valid_centralRER && is_valid_coresocodi)
                    {
                        RerCentralCodRetiro.Rerpprcodi = Rerpprcodi;
                        model.ListRerCentralCodRetiro.Add(RerCentralCodRetiro);
                        this.servicioPrimasRER.SaveRerCentralCodRetiro(RerCentralCodRetiro);
                    }
                    #endregion

                }

                model.Resultado = "1";
                return Json(model);
                
            }
            catch (Exception)
            {
                return Json("-1");
            }
        }
    }
}