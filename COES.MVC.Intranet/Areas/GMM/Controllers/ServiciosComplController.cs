
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.GMM.Models;
using COES.MVC.Intranet.Areas.ValorizacionDiaria.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.GMM;
using COES.Servicios.Aplicacion.ValorizacionDiaria;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.GMM.Controllers
{
    public class ServiciosComplController : BaseController
    {
        DatInsumoAppServicio datInsumoAppServicio = new DatInsumoAppServicio();
        GarantiaAppServicio garantiaAppServicio = new GarantiaAppServicio();
        ValorizacionDiariaAppServicio servicio = new ValorizacionDiariaAppServicio();
        AgenteAppServicio servicioAgente = new AgenteAppServicio();
        private const string _estadoRegistroEmpresaActivo = "A";
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ServiciosComplController));
        private const string _estadoEmpresaParticipanteActivo = "H";
        public ServiciosComplController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            //base.OnException(filterContext);

            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("ServiciosComplController", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("ServiciosComplController", ex);
                throw;
            }
        }


        //
        // GET: /GMM/ServiciosComplementarios/

        public ActionResult Index()
        {

            ComplementarioModel model = new ComplementarioModel();
            DateTime fechaActual = DateTime.Today;
            int anio = fechaActual.Year;
            int mes = fechaActual.Month;
            model.listadoinsumos1vez = garantiaAppServicio.mensajeProcesamientoParticipante(anio, mes.ToString());



            return View(model);
        }

        [HttpPost]
        public PartialViewResult ListaPrimerMes(string anho, string mes)
        {
            ComplementarioModel model = new ComplementarioModel();
            int _anho = 0;
            bool _try = int.TryParse(anho, out _anho);
            if (!_try) throw new Exception("Año inválido");

            decimal _valor = 0;
            _try = decimal.TryParse(mes, out _valor);
            if (!_try) throw new Exception("Mes inválido");

            string Estado = datInsumoAppServicio.ConsultaEstadoPeriodo(_anho, mes);
            model.listadoinsumos1vez = garantiaAppServicio.mensajeProcesamientoParticipante(_anho, mes);
            model.Estado = Estado;
            return PartialView("ListaPrimerMes", model);
        }
        [HttpPost]
        public PartialViewResult ListarComplementarios(string anho, string mes)
        {
            ComplementarioModel model = new ComplementarioModel();
            int _anho = 0;
            bool _try = int.TryParse(anho, out _anho);
            model.listadoComplementarios = datInsumoAppServicio.ListarDatosInsumoTipoSC(_anho, mes);

            //model.listadoComplementarios.Add(new Dominio.DTO.Sic.GmmDatInsumoDTO() { EMPGCODI = 1, LISTEMPRNOMB = "rick test", LISTDINSVALOR = 15.50M });
            //model.listadoComplementarios.Add(new Dominio.DTO.Sic.GmmDatInsumoDTO() { EMPGCODI = 2, LISTEMPRNOMB = "rick test 2", LISTDINSVALOR = 0M });
            //model.listadoComplementarios.Add(new Dominio.DTO.Sic.GmmDatInsumoDTO() { EMPGCODI = 3, LISTEMPRNOMB = "rick test 3", LISTDINSVALOR = 0M });
            return PartialView("ListarComplementarios", model);
        }

        [HttpPost]
        public PartialViewResult ListarEntregas(string anho, string mes)
        {
            ComplementarioModel model = new ComplementarioModel();
            int _anho = 0;
            bool _try = int.TryParse(anho, out _anho);
            model.listadoEntregas = datInsumoAppServicio.ListarDatosEntregas(_anho, mes);
            return PartialView("ListarEntregas", model);
        }

        [HttpPost]
        public PartialViewResult ListarInflexibilidades(string anho, string mes)
        {
            ComplementarioModel model = new ComplementarioModel();
            int _anho = 0;
            bool _try = int.TryParse(anho, out _anho);
            model.listadoInflexibilidad = datInsumoAppServicio.ListarDatosInflexibilidades(_anho, mes);
            return PartialView("ListarInflexibilidades", model);
        }

        [HttpPost]
        public PartialViewResult ListarRecaudaciones(string anho, string mes)
        {
            ComplementarioModel model = new ComplementarioModel();
            int _anho = 0;
            bool _try = int.TryParse(anho, out _anho);
            model.listadoRecaudacion = datInsumoAppServicio.ListarDatosReacudaciones(_anho, mes);
            return PartialView("ListarRecaudaciones", model);
        }

        [HttpPost]
        public PartialViewResult ListarInsumos(string anho, string mes)
        {
            ComplementarioModel model = new ComplementarioModel();
            int _anho = 0;
            bool _try = int.TryParse(anho, out _anho);
            model.listadoInsumos = datInsumoAppServicio.ListadoInsumos(_anho, mes);
            //model.listadoinsumos1vez = garantiaAppServicio.mensajeProcesamientoParticipante(_anho, mes);
            return PartialView("ListarInsumos", model);
        }
        [HttpPost]
        public PartialViewResult ListarEmpresasParticipante(string empresa)
        {
            AgenteModel model = new AgenteModel();
            model.ListMaestroEmpresa = this.servicioAgente.ListarEmpresasParticipantes(empresa, _estadoEmpresaParticipanteActivo).OrderBy(p => p.Emprrazsocial).ToList();
            return PartialView("ListarEmpresas", model);
        }
        [HttpPost]
        public ActionResult CamposInsumosGen(int anio, string mes)
        {
            List<GmmGarantiaDTO> entitie = new List<GmmGarantiaDTO>();
            entitie = garantiaAppServicio.mensajeProcesamientoParticipante(anio, mes);
            var jsonSerialiser = new JavaScriptSerializer();
            return Json(jsonSerialiser.Serialize(entitie));
        }

        /// <summary>
        /// Guarda o actualiza un DatInsumo
        /// </summary>
        /// <returns></returns>
        public ActionResult Grabar(string anho, string mes, int codigoEmpresa, int codigoParticipante, string valor, int tipo)
        {
            GmmDatInsumoDTO oGmmDatInsumoDTO = new GmmDatInsumoDTO();
            int _anho = 0;
            bool _try = int.TryParse(anho, out _anho);
            if (!_try) throw new Exception("Año inválido");

            decimal _valor = 0;
            _try = decimal.TryParse(valor, out _valor);
            if (!_try) return Json(new { success = false, message = "Valor inválido" });
            bool respuesta = false;
            if (datInsumoAppServicio.ConsultaEstadoPeriodo(_anho, mes) == "A")
            {
                oGmmDatInsumoDTO.EMPRCODI = codigoEmpresa;
                oGmmDatInsumoDTO.EMPGCODI = codigoParticipante;
                oGmmDatInsumoDTO.DINSANIO = _anho;
                oGmmDatInsumoDTO.DINSMES = mes;
                oGmmDatInsumoDTO.DINSVALOR = _valor;
                oGmmDatInsumoDTO.DINSUSUARIO = User.Identity.Name;

                respuesta = datInsumoAppServicio.UpsertDatos(oGmmDatInsumoDTO, tipo);
            }
            return Json(new { success = respuesta, message = "Ok" });
        }

        [HttpPost]
        public PartialViewResult ListarMaestroEmpresas(string empresa)
        {
            ProcesoModel model = new ProcesoModel();
            model.Empresas = servicio.ObtenerEmpresasMME();
            return PartialView("ListarEmpresas", model);
        }

        [HttpPost]
        public ActionResult ConsultaParticipanteExistente(int participante)
        {
            return Json(new { success = datInsumoAppServicio.ConsultaParticipanteExistente(participante), message = "Ok" });
        }

        /// <summary>
        /// Elimina un DatCalculo
        /// </summary>
        /// <returns></returns>
        public ActionResult Eliminar(string anho, string mes, int codigoEmpresa, string valor, int tipo)
        {
            GmmDatInsumoDTO oGmmDatInsumoDTO = new GmmDatInsumoDTO();
            int _anho = 0;
            bool _try = int.TryParse(anho, out _anho);
            if (!_try) throw new Exception("Año inválido");

            decimal _valor = 0;
            _try = decimal.TryParse(valor, out _valor);
            if (!_try) return Json(new { success = false, message = "Valor inválido" });
            bool respuesta = false;
            if (datInsumoAppServicio.ConsultaEstadoPeriodo(_anho, mes) == "A")
            {

                oGmmDatInsumoDTO.EMPRCODI = codigoEmpresa;
                oGmmDatInsumoDTO.DINSANIO = _anho;
                oGmmDatInsumoDTO.DINSMES = mes;
                oGmmDatInsumoDTO.DINSVALOR = _valor;
                oGmmDatInsumoDTO.DINSUSUARIO = User.Identity.Name;

                respuesta = datInsumoAppServicio.UpsertDatos(oGmmDatInsumoDTO, tipo, true);
            }
            return Json(new { success = respuesta, message = "Ok" });
        }
    }
}
