using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.CompensacionRSF.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.CompensacionRSF;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CompensacionRSF.Controllers
{
    public class UnidadExoneradaController : BaseController
    {
        // GET: /CompensacionRSF/UnidadExonerada/

        public UnidadExoneradaController()
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
        CompensacionRSFAppServicio servicioCompensacionRsf = new CompensacionRSFAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();

        public ActionResult Index(int pericodi = 0, int vcrecacodi = 0)
        {
            base.ValidarSesionUsuario();
            UnidadExoneradaModel model = new UnidadExoneradaModel();
            Log.Info("Lista de Periodos - ListPeriodo");
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            if (model.ListaPeriodos.Count > 0 && pericodi == 0)
            {
                pericodi = model.ListaPeriodos[0].PeriCodi;
            }
            Log.Info("Entidad Periodo - GetByIdPeriodo");
            model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
            Log.Info("Lista de Versiones - ListVcrRecalculos");
            model.ListaRecalculo = this.servicioCompensacionRsf.ListVcrRecalculos(pericodi); //Ordenado en descendente
            if (model.ListaRecalculo.Count > 0 && vcrecacodi == 0)
            {
                vcrecacodi = (int)model.ListaRecalculo[0].Vcrecacodi;
            }

            if (pericodi > 0 && vcrecacodi > 0)
            {
                Log.Info("EntidadRecalculo - GetByIdVcrRecalculoView");
                model.EntidadRecalculo = this.servicioCompensacionRsf.GetByIdVcrRecalculoView(pericodi, vcrecacodi);
            }
            else
            {
                model.EntidadRecalculo = new VcrRecalculoDTO();
            }
            model.Pericodi = pericodi;
            model.Vcrecacodi = vcrecacodi;
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name);
            return View(model);
        }

        [HttpPost]
        public JsonResult actualizarCampos(int id, string estado, string comentario)
        {
            base.ValidarSesionUsuario();
            UnidadExoneradaModel model = new UnidadExoneradaModel();
            // obtener entidad a actualizar
            Log.Info("EntidadUnidadExonerada - GetByIdVcrUnidadexonerada");
            model.EntidadUnidadExonerada = this.servicioCompensacionRsf.GetByIdVcrUnidadexonerada(id);

            //setear campos nuevos 
            model.EntidadUnidadExonerada.Vcruexonerar = estado;
            model.EntidadUnidadExonerada.Vcruexobservacion = comentario;
            Log.Info("Actualizar el registro - UpdateVcrUnidadexonerada");
            this.servicioCompensacionRsf.UpdateVcrUnidadexonerada(model.EntidadUnidadExonerada);
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, User.Identity.Name);
            return Json(model);
        }
        
        /// <summary>
        /// Muestra la lista de Unidades Exoneradas
        /// </summary>
        /// <param name="pericodi">Código del Mes de cálculo</param>
        /// <param name="vcrecacodi">Versión de cálculo</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Lista(int pericodi = 0, int vcrecacodi = 0)
        {
            UnidadExoneradaModel model = new UnidadExoneradaModel();
            //Lista todas la lista de la tabla VCR_UNIDADEXONERADA incluido los Nombres de Empresa, Central y Unidad
            Log.Info("ListaUnidadExonerada - ListVcrUnidadexoneradasParametro");
            model.ListaUnidadExonerada = this.servicioCompensacionRsf.ListVcrUnidadexoneradasParametro(vcrecacodi);
            return PartialView(model);
        }

        /// <summary>
        /// Muestra un registro de las unidades de exoneracion 
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <returns></returns>
        public ActionResult Exonerar(int vcruexcodi = 0)
        {
            UnidadExoneradaModel model = new UnidadExoneradaModel();
            Log.Info("EntidadUnidadExonerada - GetByIdVcrUnidadexoneradaView");
            model.EntidadUnidadExonerada = this.servicioCompensacionRsf.GetByIdVcrUnidadexoneradaView(vcruexcodi);
            return PartialView(model);
        }

        /// <summary>
        /// Permite actualizar la unidades que son considerados para exonerar
        /// </summary>
        /// <param name="pericodi">Código del Mes de cálculo</param>
        /// <param name="vcrecacodi">Versión de cálculo</param>
        /// <param name="items">Lista de Ids</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarListaExonerar(int pericodi = 0, int vcrecacodi = 0, string items = "")
        {
            base.ValidarSesionUsuario();
            MedicionBornesModel model = new MedicionBornesModel();
            model.sError = "";
            model.iNumReg = 0;
            try
            {
                if (pericodi > 0 && vcrecacodi > 0)
                {
                    //Actualizamos la información, colocando en NO a todas las unidades
                    Log.Info("Actualizamos la información - UpdateVcrUnidadexoneradaVersionNO");
                    this.servicioCompensacionRsf.UpdateVcrUnidadexoneradaVersionNO(vcrecacodi, User.Identity.Name);

                    //Actualizamos la información, colocando en SI, solo a la lista de items seleccionados
                    string[] Ids = items.Split(ConstantesAppServicio.CaracterComa);
                    foreach (string Id in Ids)
                    {
                        int iVcruexcodi = Convert.ToInt32(Id);
                        if (iVcruexcodi > 0)
                        {
                            Log.Info("Actualizamos la información - UpdateVcrUnidadexoneradaVersionSI");
                            this.servicioCompensacionRsf.UpdateVcrUnidadexoneradaVersionSI(vcrecacodi, User.Identity.Name, iVcruexcodi);
                            model.iNumReg++;
                        }
                    }



                }
                else
                    model.sError = "Debe seleccionar un periodo y versión correcto";
            }
            catch (Exception e)
            {
                model.sError = e.Message;
            }
            return Json(model);
        }
    }
}
