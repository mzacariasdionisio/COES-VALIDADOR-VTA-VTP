using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.General;
using COES.MVC.Intranet.Areas.Despacho.ViewModels;
using COES.Servicios.Aplicacion.Helper;
using OfficeOpenXml.ConditionalFormatting;
using COES.MVC.Intranet.Controllers;

namespace COES.MVC.Intranet.Areas.Despacho.Controllers
{
    public class DatosModoOperacionController : BaseController
    {
        //
        // GET: /Despacho/DatosModoOperacion/
        private GeneralAppServicio appGeneral = new GeneralAppServicio();
        private EquipamientoAppServicio appEquipamiento = new EquipamientoAppServicio();
        private DespachoAppServicio appDespacho = new DespachoAppServicio();

        private List<EstadoModel> lsEstados = new List<EstadoModel>();
        public DatosModoOperacionController()
        {
            lsEstados.Add(new EstadoModel { EstadoCodigo = "S", EstadoDescripcion = "Activo" });
            lsEstados.Add(new EstadoModel { EstadoCodigo = "N", EstadoDescripcion = "Inactivo" });
        }

        public ActionResult Index(int repcodi)
        {
            var modelo = new IndexDatosMOViewModel();
            var oRepCV = appDespacho.GetByIdPrRepcv(repcodi);
            modelo.EmpresaLista = appGeneral.ListarEmpresasPorTipoEmpresa(3);
            //modelo.CentralesLista = appEquipamiento.ListarEquiposxFiltro(0,"A",0,3,"",0);//Listado de Todos Equipos x Tipo Empresa Generadores, Equipos activos
            modelo.CentralId = 0;
            modelo.EmpresaId = 0;
            modelo.EstadoLista = lsEstados;
            modelo.EstadoId = "S";
            modelo.NombreModo = "";
            modelo.FechaRepCV = oRepCV.Repfecha;
            modelo.RepCodi = oRepCV.Repcodi;
            return View(modelo);
        }

        [HttpPost]
        public PartialViewResult Paginado(IndexDatosMOViewModel modelo)
        {
            IndexDatosMOViewModel model = new IndexDatosMOViewModel();
            model.IndicadorPagina = false;
            int nroRegistros = appDespacho.CantidadModosOperacionxFiltro(modelo.EmpresaId, modelo.EstadoId, modelo.NombreModo);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeEvento;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }
        [HttpPost]
        public PartialViewResult Lista(IndexDatosMOViewModel modelo)
        {
            modelo.ResultadoLista = appDespacho.ModosOperacionxFiltro(modelo.EmpresaId, modelo.EstadoId, modelo.NombreModo, modelo.NroPagina, Constantes.PageSizeEvento);
            return PartialView(modelo);
        }

        public ActionResult DetalleModoOperacionDatos(int id,int repCodi)
        {
            var modeloDetalle = new DetalleDatosMOViewModel();
            var oModo = appDespacho.ObtenerModoOperacion(id);
            var oRepCodi = appDespacho.GetByIdPrRepcv(repCodi);
            modeloDetalle.AbreviaturaModo = oModo.Grupoabrev;
            modeloDetalle.Empresa = oModo.EmprNomb;
            modeloDetalle.Estado = oModo.Grupoactivo=="S"?"Activo":"Inactivo";
            modeloDetalle.NombreModo = oModo.Gruponomb;
            modeloDetalle.Tipo = oModo.Grupotipo=="T"?"Termoeléctrico":"Hidroeléctrico";
            modeloDetalle.iGrupoCodi = oModo.Grupocodi;
            modeloDetalle.cbFicha = false;
            modeloDetalle.repCodi = repCodi;
            modeloDetalle.FechaRepCV = oRepCodi.Repfecha;
            bool AccesoNuevo = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Nuevo);
            bool AccesoEditar = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Editar);
            modeloDetalle.EnableNuevo = AccesoNuevo ? "" : "disabled=disabled";
            modeloDetalle.EnableEditar = AccesoEditar ? "" : "disableClick";
            return View(modeloDetalle);
        }

        public PartialViewResult ListaDatosModoOperacion(DetalleDatosMOViewModel modelo)
        {
            var oRepCV = appDespacho.GetByIdPrRepcv(modelo.repCodi);
            modelo.ListaResultado = appDespacho.ListarDatosVigentesPorModoOperacion(modelo.iGrupoCodi, modelo.cbFicha, oRepCV.Repfecha);
            bool AccesoNuevo = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Nuevo);
            bool AccesoEditar = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Editar);
            modelo.EnableNuevo = AccesoNuevo ? "" : "disabled=disabled";
            modelo.EnableEditar = AccesoEditar ? "" : "disableClick";
            return PartialView(modelo);
        }

        [HttpPost]
        public PartialViewResult EdicionParametro(int idGrupo,int idConcepto,string sModo)
        {
            var modelo = new EditarNuevoDatoMOViewModel();
            if (sModo == "EDITAR")
            {
                var oConcepto = appDespacho.GetByIdPrConcepto(idConcepto);
                modelo.ConcepCodi = idConcepto;
                modelo.GrupoCodi = idGrupo;
                modelo.NombreConcepto = oConcepto.Concepdesc;
                modelo.FechaDat = DateTime.Today.ToShortDateString();
                modelo.Formula = "";
            }
            if (sModo == "NUEVO")
            {
                modelo.ListadoConceptos = ConceptosPorCategoriasGrupo(idGrupo);//appDespacho.GetByCriteriaPrConceptos().OrderBy(t => t.Concepdesc).ToList();
                modelo.FechaDat = DateTime.Today.ToShortDateString();
                modelo.Formula = "";
                modelo.GrupoCodi = idGrupo;
            }
            modelo.sModoEdicion = sModo.Trim();
            return PartialView(modelo);
        }

        [HttpPost]
        public PartialViewResult HistorialParametro(int idGrupo, int idConcepto)
        {
            var modelo = new HistorialParametroViewModel();
            var oConcepto=appDespacho.GetByIdPrConcepto(idConcepto);
            var lsHistorico = appDespacho.ListarValoresHistoricosDespacho(idConcepto, idGrupo);
            modelo.ListadoHistorico = lsHistorico;
            modelo.NombreConcepto = oConcepto.Concepdesc;
            return PartialView(modelo);
        }

        [HttpPost]
        public JsonResult ActualizarValorParametro(string sModo,int iGrupoCodi, int iConcepCodi, string sValor,string sFechaDat)
        {
            try
            {
                var oDatoParametro = new PrGrupodatDTO();
                var usuario = User.Identity.Name;
                if (sModo == "EDITAR")
                {
                    oDatoParametro.Deleted = 0;
                    oDatoParametro.Concepcodi = iConcepCodi;
                    oDatoParametro.Fechaact = DateTime.Now;
                    oDatoParametro.Fechadat = DateTime.Parse(sFechaDat);
                    oDatoParametro.Formuladat = sValor;
                    oDatoParametro.Grupocodi = iGrupoCodi;
                    oDatoParametro.Lastuser = usuario;
                    var datoParametro=appDespacho.GetByIdPrGrupodat((DateTime)oDatoParametro.Fechadat, oDatoParametro.Concepcodi,
                        oDatoParametro.Grupocodi, oDatoParametro.Deleted);
                    if (datoParametro == null)
                    {
                        appDespacho.SavePrGrupodat(oDatoParametro);
                    }
                    else
                    {
                        appDespacho.DeletePrGrupodat((DateTime)oDatoParametro.Fechadat, oDatoParametro.Concepcodi,
                        oDatoParametro.Grupocodi, base.UserName);
                        appDespacho.SavePrGrupodat(oDatoParametro);
                    }
                    
                }
                if (sModo == "NUEVO")
                {
                    var oConcepto = appDespacho.GetByIdPrConcepto(iConcepCodi);
                    int iCodGrupo = 0;
                    var oGrupoF = appDespacho.GetByIdPrGrupo(iGrupoCodi);
                    PrGrupoDTO oGrupoG = new PrGrupoDTO();
                    PrGrupoDTO oGrupoC= new PrGrupoDTO();
                    if (oGrupoF.Grupopadre != null)
                    {
                        oGrupoG = appDespacho.GetByIdPrGrupo(oGrupoF.Grupopadre.Value);
                    }
                    if (oGrupoG.Grupopadre != null) oGrupoC = appDespacho.GetByIdPrGrupo(oGrupoG.Grupopadre.Value);

                    switch (oConcepto.Catecodi)
                    {
                        case 2:
                            iCodGrupo = iGrupoCodi;
                            break;
                        case 3:
                        case 5:
                            iCodGrupo = oGrupoG.Grupocodi;
                            break;
                        case 6:
                        case 11:
                        case 101:
                        case 102:
                        case 103:
                        case 104:
                        case 105:
                        case 111:
                            iCodGrupo = oGrupoC.Grupocodi;
                            break;
                            
                    }

                    oDatoParametro.Deleted = 0;
                    oDatoParametro.Concepcodi = iConcepCodi;
                    oDatoParametro.Fechaact = DateTime.Now;
                    oDatoParametro.Fechadat = DateTime.Parse(sFechaDat);
                    oDatoParametro.Formuladat = sValor;
                    oDatoParametro.Grupocodi = iCodGrupo;
                    oDatoParametro.Lastuser = usuario;
                    appDespacho.SavePrGrupodat(oDatoParametro);
                }
                return Json(1);
            }
            catch(Exception ex)
            {
                return Json(-1);
            }
        }
        /// <summary>
        /// Filtra los conceptos que coinciden con las categorias del modo de operación
        /// </summary>
        /// <param name="iGrupoCodi">Codigo de grupo funcional</param>
        /// <returns></returns>
        protected List<PrConceptoDTO> ConceptosPorCategoriasGrupo(int iGrupoCodi)
        {
            var ConceptosFinales = new List<PrConceptoDTO>();
            var todosConceptos = appDespacho.GetByCriteriaPrConceptos();
            var oGrupoF = appDespacho.GetByIdPrGrupo(iGrupoCodi);
            PrGrupoDTO oGrupoG = new PrGrupoDTO();
            PrGrupoDTO oGrupoC = new PrGrupoDTO();
            if (oGrupoF.Grupopadre != null)
            {
                oGrupoG = appDespacho.GetByIdPrGrupo(oGrupoF.Grupopadre.Value);
            }
            if (oGrupoG.Grupopadre != null) oGrupoC = appDespacho.GetByIdPrGrupo(oGrupoG.Grupopadre.Value);

            foreach (var oConcepto in todosConceptos)
            {
                if (oGrupoF.Catecodi == oConcepto.Catecodi)
                {
                    ConceptosFinales.Add(oConcepto);
                    continue;
                }
                if (oGrupoG.Catecodi == oConcepto.Catecodi)
                {
                    ConceptosFinales.Add(oConcepto);
                    continue;
                }
                if (oGrupoC.Catecodi == oConcepto.Catecodi)
                {
                    ConceptosFinales.Add(oConcepto);
                    continue;
                }
                if (999 == oConcepto.Catecodi)
                {
                    ConceptosFinales.Add(oConcepto);
                    continue;
                }

            }
            return ConceptosFinales.OrderBy(t => t.Concepdesc).ToList();
        }

    }
}
