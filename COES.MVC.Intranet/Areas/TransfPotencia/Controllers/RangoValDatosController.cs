using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using COES.MVC.Intranet.Areas.TransfPotencia.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Transferencias;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Controllers
{
    public class RangoValDatosController : BaseController
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(RangoValDatosController));
        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        VariacionEmpresaAppServicio servicioVariacionEmpresa = new VariacionEmpresaAppServicio();
        VariacionCodigoAppServicio servicioVariacionCodigo = new VariacionCodigoAppServicio();
        private static string NombreControlador = "RangoValDatosController";
        public ActionResult Index(int tipocomp = 0)
        {
            base.ValidarSesionUsuario();
            RangoValDatosModel model = new RangoValDatosModel();
            model.Entidad = this.servicioVariacionEmpresa.GetDefaultPercentVariationByTipoComp("A");
            model.VarempprocentajeHistorico = model.Entidad !=  null ? model.Entidad.Varempprocentaje.ToString() : "";
            model.Entidad = this.servicioVariacionEmpresa.GetDefaultPercentVariationByTipoComp("B");
            model.VarempprocentajeVTP = model.Entidad != null ? model.Entidad.Varempprocentaje.ToString() : "";
            return View(model);
        }

        /// <summary>
        /// Permite procesar el registro de porcentaje de variación histórico o de energía.
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegistrarDescuentoPorDefecto(string porcentaje = "", int tipocomp = 0)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";
            if (porcentaje == "" || tipocomp == 0)
            {
                sResultado = "Lo sentimos, debe ingresar un porcentaje válido";
                return Json(sResultado);
            }
            try
            {
                #region ActualizarStatusPorcentajesAnteriores

                VtpVariacionEmpresaDTO dtoVariacionEmpresaUpd = new VtpVariacionEmpresaDTO();
                dtoVariacionEmpresaUpd.Varemptipocomp = tipocomp == 1 ? "A" : "B";
                dtoVariacionEmpresaUpd.Varempestado = "INA";
                this.servicioVariacionEmpresa.UpdateStatusVariationByTipoComp(dtoVariacionEmpresaUpd);

                #endregion

                VtpVariacionEmpresaDTO dtoVariacionEmpresa = new VtpVariacionEmpresaDTO();
                dtoVariacionEmpresa.Emprcodi = 0;
                dtoVariacionEmpresa.Varemptipocomp = tipocomp == 1 ? "A" : "B";
                dtoVariacionEmpresa.Varempprocentaje = Convert.ToDecimal(porcentaje);
                dtoVariacionEmpresa.Varempvigencia = DateTime.Now;
                dtoVariacionEmpresa.Varempestado = "ACT";
                dtoVariacionEmpresa.Varempusucreacion = User.Identity.Name;
                dtoVariacionEmpresa.Varempfeccreacion = DateTime.Now;
                dtoVariacionEmpresa.Varempusumodificacion = User.Identity.Name;
                dtoVariacionEmpresa.Varempfecmodificacion = DateTime.Now;
                //Grabar
                int resultado = this.servicioVariacionEmpresa.Save(dtoVariacionEmpresa);
                if (resultado < 1) {
                    sResultado = "Lo sentimos, ocurrió un error al insertar la variación de empresa - Controller";
                }
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador + " - RegistrarDescuentoPorDefecto", ex);
                sResultado = ex.Message;
            }

            return Json(sResultado);
        }

        [HttpPost]
        public ActionResult Lista(int? tipocomp, int NroPagina, string emprNomb)
        {
            string varemptipocomp = tipocomp == 1 ? "A" : "B";
            string varemprnomb = emprNomb == null || emprNomb == "" ? "*" : emprNomb;
            RangoValDatosModel model = new RangoValDatosModel();
            model.ListaVaricionEmpresa = this.servicioVariacionEmpresa.ListValorTransferenciaEmpresaRE(varemptipocomp, NroPagina, Funcion.PageSizeVariacionEmpresa, varemprnomb.ToUpper());
            model.Varempcodigo = tipocomp == 1;
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ListVariacionCodigoByEmprCodi(int emprcodi, int NroPagina, string codiVtp, int tipocomp)
        {
            string varcodivtp = codiVtp == null || codiVtp == "" ? "*" : codiVtp;
            RangoValDatosModel model = new RangoValDatosModel();
            if(tipocomp == 2)
            {
                model.ListaVariacionCodigo = this.servicioVariacionCodigo.ListVariacionCodigoByEmprCodi(emprcodi, NroPagina, Funcion.PageSizeVariacionCodigo, varcodivtp.ToUpper());
            }
            else
            {
                model.ListaVariacionCodigo = this.servicioVariacionCodigo.ListVariacionCodigoVTEAByEmprCodi(emprcodi, NroPagina, Funcion.PageSizeVariacionCodigo, varcodivtp.ToUpper());
            }
            model.VarTipoComp = tipocomp;
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Paginado(int? tipocomp, int NroPagina, string emprNomb)
        {
            string varemptipocomp = tipocomp == 1 ? "A" : "B";
            string varemprnomb = emprNomb == null || emprNomb == "" ? "*" : emprNomb;
            RangoValDatosModel model = new RangoValDatosModel();
            model.IndicadorPagina = false;
            model.NroRegistros = this.servicioVariacionEmpresa.GetNroRecordsVariacionEmpresaByTipoComp(varemptipocomp, varemprnomb);
            if (model.NroRegistros > Funcion.NroPageShow)
            {
                int pageSize = Funcion.PageSizeVariacionEmpresa;
                int nroPaginas = (model.NroRegistros % pageSize == 0) ? model.NroRegistros / pageSize : model.NroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Funcion.NroPageShow;
                model.IndicadorPagina = true;
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult PaginadoVariacionCodigo(int emprcodi, int NroPagina, string codiVtp, int tipocomp)
        {
            RangoValDatosModel model = new RangoValDatosModel();
            model.IndicadorPagina = false;
            string varcodivtp = codiVtp == null || codiVtp == "" ? "*" : codiVtp;
            model.NroRegistros = tipocomp == 2 ? this.servicioVariacionCodigo.GetNroRecordsVariacionCodigoByEmprCodi(emprcodi, varcodivtp) : this.servicioVariacionCodigo.GetNroRecordsVariacionCodigoVTEAByEmprCodi(emprcodi, varcodivtp);
            if (model.NroRegistros > Funcion.NroPageShow)
            {
                int pageSize = Funcion.PageSizeVariacionCodigo;
                int nroPaginas = (model.NroRegistros % pageSize == 0) ? model.NroRegistros / pageSize : model.NroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Funcion.NroPageShow;
                model.IndicadorPagina = true;
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult PaginadoVariacionCodigoVTEA(int emprcodi, string codiVtp)
        {
            RangoValDatosModel model = new RangoValDatosModel();
            model.IndicadorPagina = false;
            string varcodivtp = codiVtp == null || codiVtp == "" ? "*" : codiVtp;
            model.NroRegistros = this.servicioVariacionCodigo.GetNroRecordsVariacionCodigoVTEAByEmprCodi(emprcodi, varcodivtp);
            if (model.NroRegistros > Funcion.NroPageShow)
            {
                int pageSize = Funcion.PageSizeVariacionCodigo;
                int nroPaginas = (model.NroRegistros % pageSize == 0) ? model.NroRegistros / pageSize : model.NroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Funcion.NroPageShow;
                model.IndicadorPagina = true;
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ListaHistorialVariacionEmpresa(int tipocomp, int emprcodi)
        {
            string varemptipocomp = tipocomp == 1 ? "A" : "B";
            RangoValDatosModel model = new RangoValDatosModel();
            model.ListaVaricionEmpresa = this.servicioVariacionEmpresa.ListHistoryVariacionEmpresaByEmprCodiAndTipoComp(varemptipocomp, emprcodi);
            return Json(JsonConvert.SerializeObject(model.ListaVaricionEmpresa));
        }

        [HttpPost]
        public ActionResult ListaHistorialVariacionCodigo(string codigovtp, int tipocomp)
        {
            string varemptipocomp = tipocomp == 1 ? "A" : "B";
            RangoValDatosModel model = new RangoValDatosModel();
            model.ListaVariacionCodigo = this.servicioVariacionCodigo.ListHistoryVariacionCodigoByCodigoVtp(codigovtp, varemptipocomp);
            return Json(JsonConvert.SerializeObject(model.ListaVariacionCodigo));
        }

        [HttpPost]
        public JsonResult RegistrarDescuentoEmpresa(int tipocomp, string datos)
        {
            string sResultado = "1";
            int cantidadRecords = 0;
            List<Dictionary<String, Object>> lstDatosEmpresa;
            try
            {
                lstDatosEmpresa = JsonConvert.DeserializeObject<List<Dictionary<String, Object>>>(datos);
                foreach (Dictionary<String, Object> objEmpresa in lstDatosEmpresa)
                {
                    #region ActualizarStatusPorcentajesAnteriores

                    VtpVariacionEmpresaDTO dtoVariacionEmpresaUpd = new VtpVariacionEmpresaDTO();
                    dtoVariacionEmpresaUpd.Emprcodi = Convert.ToInt32(objEmpresa["idEmp"]);
                    dtoVariacionEmpresaUpd.Varemptipocomp = tipocomp == 1 ? "A" : "B";
                    dtoVariacionEmpresaUpd.Varempestado = "INA";
                    dtoVariacionEmpresaUpd.Varempusumodificacion = User.Identity.Name;
                    dtoVariacionEmpresaUpd.Varempfecmodificacion = DateTime.Now;
                    this.servicioVariacionEmpresa.UpdateStatusVariationByTipoCompAndEmpresa(dtoVariacionEmpresaUpd);

                    #endregion

                    VtpVariacionEmpresaDTO dtoVariacionEmpresa = new VtpVariacionEmpresaDTO();
                    dtoVariacionEmpresa.Emprcodi = Convert.ToInt32(objEmpresa["idEmp"]);
                    dtoVariacionEmpresa.Varemptipocomp = tipocomp == 1 ? "A" : "B";
                    dtoVariacionEmpresa.Varempprocentaje = Convert.ToDecimal(objEmpresa["desc"]);
                    dtoVariacionEmpresa.Varempvigencia = DateTime.Now;
                    dtoVariacionEmpresa.Varempestado = "ACT";
                    dtoVariacionEmpresa.Varempusucreacion = User.Identity.Name;
                    dtoVariacionEmpresa.Varempfeccreacion = DateTime.Now;
                    dtoVariacionEmpresa.Varempusumodificacion = User.Identity.Name;
                    dtoVariacionEmpresa.Varempfecmodificacion = DateTime.Now;

                    int resultado = this.servicioVariacionEmpresa.Save(dtoVariacionEmpresa);
                    if (resultado > 1)
                    {
                        cantidadRecords++;
                    }
                    //Grabar
                }
                if(cantidadRecords != lstDatosEmpresa.Count)
                {
                    sResultado = "Lo sentimos, ocurrió un error al insertar la variación de empresa - Controller";
                }
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador + " - RegistrarDescuentoEmpresa", ex);
                sResultado = ex.Message;
            }
            return Json(sResultado);
        }

        [HttpPost]
        public JsonResult RegistrarDescuentoCodigoVariacion(string datos)
        {
            string sResultado = "1";
            int cantidadRecords = 0;
            List<Dictionary<String, Object>> lstDatosVariacionCodigo;
            try
            {
                lstDatosVariacionCodigo = JsonConvert.DeserializeObject<List<Dictionary<String, Object>>>(datos);
                foreach (Dictionary<String, Object> objCodigo in lstDatosVariacionCodigo)
                {
                    #region ActualizarStatusPorcentajesAnteriores

                    VtpVariacionCodigoDTO dtoVariacionCodigoUpd = new VtpVariacionCodigoDTO();
                    dtoVariacionCodigoUpd.VarCodCodigoVtp = objCodigo["codVtp"].ToString();
                    dtoVariacionCodigoUpd.VarCodTipoComp = Convert.ToInt32(objCodigo["idTipoComp"]) == 2 ? "A" : "B";
                    dtoVariacionCodigoUpd.VarCodEstado = "INA";
                    dtoVariacionCodigoUpd.VarCodUsuModificacion = User.Identity.Name;
                    dtoVariacionCodigoUpd.VarCodFecModificacion = DateTime.Now;
                    this.servicioVariacionCodigo.UpdateStatusVariationByCodigoVtp(dtoVariacionCodigoUpd);

                    #endregion

                    VtpVariacionCodigoDTO dtoVariacionCodigo = new VtpVariacionCodigoDTO();
                    dtoVariacionCodigo.EmprCodi = Convert.ToInt32(objCodigo["idEmp"]);
                    dtoVariacionCodigo.CliCodi = Convert.ToInt32(objCodigo["idCli"]);
                    dtoVariacionCodigo.BarrCodi = Convert.ToInt32(objCodigo["idBarra"]);
                    dtoVariacionCodigo.VarCodCodigoVtp = objCodigo["codVtp"].ToString();
                    dtoVariacionCodigo.VarCodPorcentaje = Convert.ToDecimal(objCodigo["desc"]);
                    dtoVariacionCodigo.VarCodEstado = "ACT";
                    dtoVariacionCodigo.VarCodUsuCreacion = User.Identity.Name;
                    dtoVariacionCodigo.VarCodFecCreacion = DateTime.Now;
                    dtoVariacionCodigo.VarCodUsuModificacion = User.Identity.Name;
                    dtoVariacionCodigo.VarCodFecModificacion = DateTime.Now;
                    dtoVariacionCodigo.VarCodTipoComp = Convert.ToInt32(objCodigo["idTipoComp"]) == 2 ? "A" : "B";

                    int resultado = this.servicioVariacionCodigo.Save(dtoVariacionCodigo);
                    if (resultado > 1)
                    {
                        cantidadRecords++;
                    }
                    //Grabar
                }
                if (cantidadRecords != lstDatosVariacionCodigo.Count)
                {
                    sResultado = "Lo sentimos, ocurrió un error al insertar la variación por codigo vtp - Controller";
                }
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador + " - RegistrarDescuentoCodigoVariacion", ex);
                sResultado = ex.Message;
            }
            return Json(sResultado);
        }
    }
}