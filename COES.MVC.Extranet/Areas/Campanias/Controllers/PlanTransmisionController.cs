using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Numerics;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using COES.Dominio.DTO.Campania;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.Campanias.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.CalculoResarcimientos;
using COES.Servicios.Aplicacion.Campanias;
using DocumentFormat.OpenXml.Math;
using iTextSharp.text;
using log4net;
using log4net.Repository.Hierarchy;

namespace COES.MVC.Extranet.Areas.Campanias.Controllers
{
    public class PlanTransmisionController : BaseController
    {
        CampaniasAppService campaniasAppService = new CampaniasAppService();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            //if (!base.VerificarAccesoAccion(4, base.UserName)) return base.RedirectToHomeDefault();

            CampaniasModel model = new CampaniasModel();
            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Listado(string empresa, string estado, string periodo, string vigente, string fueraplazo, string estadoExcl)
        {
            CampaniasModel model = new CampaniasModel();
            List<int> idsEmpresas;
            if (empresa.Equals(""))
            {
                idsEmpresas = this.seguridad
                .ObtenerEmpresasPorUsuario(User.Identity.Name)
                .Select(x => (int)x.EMPRCODI)
                .ToList();
            }
            else
            {
                idsEmpresas = new List<int> { int.Parse(empresa) };
            }
            string empresas = string.Join<int>(ConstantesCampania.CaracterComa, idsEmpresas);
            model.ListaPlanTransmicion = campaniasAppService.GetPlanTransmisionByEstadoEmpresa(empresas, estado, periodo, vigente, fueraplazo, estadoExcl);
            return View(model);
        }

        public ActionResult Envio(int? id, int? idN, string emp, int? plan, string consult)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            CampaniasModel model = new CampaniasModel();
            if (id != null)
            {
                model.Plancodi = id.Value; // Utiliza .Value para obtener el valor del int?
            }
             if (idN != null)
            {
                model.PlancodiN = idN.Value; 
            }
            if (!string.IsNullOrEmpty(emp))
            {
                model.Codempresa = emp; 
            }
            if (plan != null)
            {
                model.Pericodi = plan.Value; // Utiliza .Value para obtener el valor del int?
            }
            if (!string.IsNullOrEmpty(consult))
            {
                model.Modo = consult; 
            }
            return View(model);
        }

         public ActionResult AbsolverEnvio(int id)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            CampaniasModel model = new CampaniasModel();
            model.Plancodi = id; 
            bool cerrado = campaniasAppService.GetObservacionByPlanCodi(model.Plancodi);
            model.observacionCerrado = cerrado;
            return View(model);
        }

         [System.Web.Mvc.HttpPost]
        public ActionResult Observacion(int id, string modo)
        {
            CampaniasModel model = new CampaniasModel();
            model.Modo = modo;

            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ListadoProyecto(int id, string modo)
        {
            List<TransmisionProyectoDTO> listaProyecto = campaniasAppService.GetTransmisionProyecto(id);
            ProyectoModel model = new ProyectoModel();
            model.listaProyecto = listaProyecto;
            model.Modo = modo;
            return View(model);
        }

          [System.Web.Mvc.HttpPost]
        public ActionResult ListadoProyectoAbsolver(int id)
        {
            List<TransmisionProyectoDTO> listaProyecto = campaniasAppService.GetTransmisionProyecto(id);
            ProyectoModel model = new ProyectoModel();
            model.listaProyecto = listaProyecto;
            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Proyecto(int id, string modo, string emp, int? plan)
        {
            CampaniasModel model = new CampaniasModel();
            if (modo.Equals(ConstantesCampania.Editar) || modo.Equals(ConstantesCampania.Consultar)) {
                TransmisionProyectoDTO proyecto = campaniasAppService.GetTransmisionProyectoById(id);
                List<int> hojasPeriodo = campaniasAppService.GetDetalleHojaByPericodi(proyecto.Pericodi, Constantes.IndDel);
                model.TransmisionProyecto = proyecto;
                model.ListaHojas = hojasPeriodo;
                model.Modo = modo;
            } else
            {
                model.Modo = modo;
                if (!string.IsNullOrEmpty(emp))
                {
                    model.Codempresa = emp;
                }
                if (plan != null)
                {
                    model.Pericodi = plan.Value; // Utiliza .Value para obtener el valor del int?
                }
            }
            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult AbsolverProyecto(int? id)
        {
            CampaniasModel model = new CampaniasModel();
            model.Proycodi = id.Value; 
            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ListadoObservacion(int id)
        {
            List<ObservacionDTO> lista = campaniasAppService.GetObservacionByProyCodi(id);
            CampaniasModel model = new CampaniasModel();
            model.ListaObservacion = lista;
            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult ListarEmpresas()
        {
            List<SiEmpresaDTO> ListaEmpresaUsuario = this.seguridad.ObtenerEmpresasPorUsuario(User.Identity.Name).
             Select(x => new SiEmpresaDTO
             {
                 Emprcodi = x.EMPRCODI,
                 Emprnomb = x.EMPRNOMB
             }).ToList();
            return Json(ListaEmpresaUsuario);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult ListarPeriodos()
        {
            List<PeriodoDTO> ListaPeriodos = campaniasAppService.ListPeriodos();
            return Json(ListaPeriodos);
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult ListarCatalogoXDesc(string descortaCat)
        {
            List<CatalogoDTO> dataCatalogoDTOs = new List<CatalogoDTO>();
            dataCatalogoDTOs = campaniasAppService.GetCatalogoXdesc(descortaCat);
            return Json(dataCatalogoDTOs);
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult ListarCatalogo(int id)
        {
            List<DataCatalogoDTO> dataCatalogoDTOs = new List<DataCatalogoDTO>();
            dataCatalogoDTOs = campaniasAppService.ListParametria(id);
            return Json(dataCatalogoDTOs);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult BuscarPlanTransmision(int id)
        {
            List<PlanTransmisionDTO> dataPlant = campaniasAppService.GetPlanTransmisionByFilters(id);
            return Json(dataPlant);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult ClonarPlanTransmision(int id)
        {
            List<PlanTransmisionDTO> dataPlant = campaniasAppService.GetPlanTransmisionByFilters(id);
            if(dataPlant != null && dataPlant.Count > 0){
                PlanTransmisionDTO planTransmisionDTO = dataPlant[0];
                planTransmisionDTO.Plancodi = campaniasAppService.GetLastPlanTransmisionId();
                planTransmisionDTO.Planversion = Constantes.VersionInicial;
                planTransmisionDTO.Plancumplimiento = Constantes.CumplimientoInicial;
                planTransmisionDTO.Planestado = Constantes.EstadoRegistrado;
                planTransmisionDTO.IndDel = Constantes.IndDel;
                planTransmisionDTO.Usucreacion = User.Identity.Name;

                if (campaniasAppService.SavePlanTransmision(planTransmisionDTO))
                {
                    List<TransmisionProyectoDTO> listaProyecto = campaniasAppService.GetTransmisionProyecto(id);
                    foreach (var transmisionProyecto in listaProyecto) {
                        TransmisionProyectoDTO transmisionProyectoDTO = transmisionProyecto;
                        int idProy = transmisionProyectoDTO.Proycodi;
                        transmisionProyectoDTO.Plancodi = planTransmisionDTO.Plancodi;
                        transmisionProyectoDTO.Proycodi = campaniasAppService.GetLastTransmisionProyectoId();
                        transmisionProyectoDTO.Usucreacion = User.Identity.Name;
                        if (campaniasAppService.SaveTransmisionProyecto(transmisionProyectoDTO)) {
                            if (transmisionProyectoDTO.Tipocodi == TipoProyecto.Generacion) 
                            {
                                if (transmisionProyectoDTO.Tipoficodi == SubTipoProyecto.CentralHidroeléctrica) 
                                {
                                    // TIPO GENERACIÓN
                                    //// Subtipo Hidroeléctrica
                                    RegHojaADTO regHojaADTO = campaniasAppService.GetRegHojaAById(idProy);
                                    if (regHojaADTO.Proycodi != 0) {
                                        regHojaADTO.Fichaacodi = campaniasAppService.GetLastRegHojaAId();
                                        regHojaADTO.Proycodi = transmisionProyectoDTO.Proycodi;
                                        regHojaADTO.IndDel = Constantes.IndDel;
                                        regHojaADTO.Usucreacion = User.Identity.Name;
                                        campaniasAppService.SaveRegHojaA(regHojaADTO);
                                    }

                                    RegHojaBDTO regHojaBDTO = campaniasAppService.GetRegHojaBById(idProy);
                                    if (regHojaBDTO.Proycodi != 0)
                                    {
                                        regHojaBDTO.Fichabcodi = campaniasAppService.GetLastRegHojaBId();
                                        regHojaBDTO.Proycodi = transmisionProyectoDTO.Proycodi;
                                        regHojaBDTO.IndDel = Constantes.IndDel;
                                        regHojaBDTO.Usucreacion = User.Identity.Name;
                                        campaniasAppService.SaveRegHojaB(regHojaBDTO);
                                    } 

                                    RegHojaCDTO regHojaCDTO = campaniasAppService.GetRegHojaCById(idProy);
                                    if (regHojaCDTO.Proycodi != 0)
                                    {
                                        List<DetRegHojaCDTO> ListaDTO = campaniasAppService.GetDetRegHojaCFichaCCodi(regHojaCDTO.Fichaccodi);
                                        regHojaCDTO.DetRegHojaCs = ListaDTO;
                                        regHojaCDTO.Fichaccodi = campaniasAppService.GetLastRegHojaCId();
                                        regHojaCDTO.Proycodi = transmisionProyectoDTO.Proycodi;
                                        regHojaCDTO.IndDel = Constantes.IndDel;
                                        regHojaCDTO.Usucreacion = User.Identity.Name;
                                        campaniasAppService.SaveRegHojaC(regHojaCDTO);
                                        if (ListaDTO != null)
                                        {
                                            foreach (var detReg in ListaDTO)
                                            {
                                                DetRegHojaCDTO regDetHojaCDTO = detReg;
                                                regDetHojaCDTO.Detareghccodi = campaniasAppService.GetLastDetRegHojaCId();
                                                regDetHojaCDTO.Fichaccodi = regHojaCDTO.Fichaccodi;
                                                regDetHojaCDTO.IndDel = Constantes.IndDel;
                                                regDetHojaCDTO.Usucreacion = User.Identity.Name;
                                                campaniasAppService.SaveDetRegHojaC(regDetHojaCDTO);
                                            }
                                        }
                                    }
                                        

                                    List<RegHojaDDTO> ListRegHojaD = campaniasAppService.GetRegHojaDById(idProy);
                                    if (ListRegHojaD != null) {
                                        foreach (var detReg in ListRegHojaD)
                                        {
                                            RegHojaDDTO regHojaDDTO = detReg;
                                            regHojaDDTO.Hojadcodi = Guid.NewGuid().ToString();
                                            regHojaDDTO.Proycodi = transmisionProyectoDTO.Proycodi;
                                            regHojaDDTO.IndDel = Constantes.IndDel;
                                            regHojaDDTO.Usucreacion = User.Identity.Name;
                                            campaniasAppService.SaveRegHojaD(regHojaDDTO);

                                            List<DetRegHojaDDTO> listaDetalle = campaniasAppService.GetDetRegHojaDFichaCCodi(detReg.Hojadcodi);
                                            if (listaDetalle != null)
                                            {
                                                foreach (var item in listaDetalle)
                                                {
                                                    DetRegHojaDDTO detRegHojaDDTO = item;
                                                    detRegHojaDDTO.Detreghdcodi = Guid.NewGuid().ToString();
                                                    detRegHojaDDTO.Usucreacion = User.Identity.Name;
                                                    detRegHojaDDTO.IndDel = Constantes.IndDel;
                                                    detRegHojaDDTO.Hojadcodi = regHojaDDTO.Hojadcodi;
                                                    campaniasAppService.SaveDetRegHojaD(detRegHojaDDTO);
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (transmisionProyectoDTO.Tipoficodi == SubTipoProyecto.CentralTermoeléctrica) 
                                {
                                    // TIPO GENERACIÓN
                                    //// Subtipo Termoeléctrica
                                    RegHojaCCTTADTO regHojaCCTTADTO = campaniasAppService.GetRegHojaCCTTAById(idProy);
                                    if (regHojaCCTTADTO.Proycodi != 0)
                                    {
                                        regHojaCCTTADTO.Centralcodi = campaniasAppService.GetLastRegHojaCCTTAId();
                                        regHojaCCTTADTO.Proycodi = transmisionProyectoDTO.Proycodi;
                                        regHojaCCTTADTO.IndDel = Constantes.IndDel;
                                        regHojaCCTTADTO.Usucreacion = User.Identity.Name;
                                        campaniasAppService.SaveRegHojaCCTTA(regHojaCCTTADTO);
                                    }  

                                    RegHojaCCTTBDTO regHojaCCTTBDTO = campaniasAppService.GetRegHojaCCTTBById(idProy);
                                    if (regHojaCCTTBDTO.Proycodi != 0)
                                    {
                                        regHojaCCTTBDTO.Centralcodi = campaniasAppService.GetLastRegHojaCCTTBId();
                                        regHojaCCTTBDTO.Proycodi = transmisionProyectoDTO.Proycodi;
                                        regHojaCCTTBDTO.IndDel = Constantes.IndDel;
                                        regHojaCCTTBDTO.Usucreacion = User.Identity.Name;
                                        campaniasAppService.SaveRegHojaCCTTB(regHojaCCTTBDTO);
                                    }

                                    RegHojaCCTTCDTO regHojaCCTTCDTO = campaniasAppService.GetRegHojaCCTTCById(idProy);
                                    if (regHojaCCTTCDTO.Proycodi != 0)
                                    {
                                        List<Det1RegHojaCCTTCDTO> Lista1DTO = campaniasAppService.GetDet1RegHojaCCTTCCentralCodi(regHojaCCTTCDTO.Centralcodi);
                                        List<Det2RegHojaCCTTCDTO> Lista2DTO = campaniasAppService.GetDet2RegHojaCCTTCCentralCodi(regHojaCCTTCDTO.Centralcodi);
                                        regHojaCCTTCDTO.Det1RegHojaCCTTCDTO = Lista1DTO;
                                        regHojaCCTTCDTO.Det2RegHojaCCTTCDTO = Lista2DTO;
                                        regHojaCCTTCDTO.Centralcodi = campaniasAppService.GetLastRegHojaCCTTCId();
                                        regHojaCCTTCDTO.Proycodi = transmisionProyectoDTO.Proycodi;
                                        regHojaCCTTCDTO.IndDel = Constantes.IndDel;
                                        regHojaCCTTCDTO.Usucreacion = User.Identity.Name;
                                        campaniasAppService.SaveRegHojaCCTTC(regHojaCCTTCDTO);
                                        if (regHojaCCTTCDTO.Det1RegHojaCCTTCDTO != null)
                                        {
                                            foreach (var detReg in regHojaCCTTCDTO.Det1RegHojaCCTTCDTO)
                                            {
                                                Det1RegHojaCCTTCDTO regDetHojaCCTTCDTO = detReg;
                                                regDetHojaCCTTCDTO.Det1centermhccodi = campaniasAppService.GetLastDet1RegHojaCCTTCId();
                                                regDetHojaCCTTCDTO.Centralcodi = regHojaCCTTCDTO.Centralcodi;
                                                regDetHojaCCTTCDTO.IndDel = Constantes.IndDel;
                                                regDetHojaCCTTCDTO.Usucreacion = User.Identity.Name;
                                                campaniasAppService.SaveDet1RegHojaCCTTC(regDetHojaCCTTCDTO);
                                            }
                                        }
                                        if (regHojaCCTTCDTO.Det2RegHojaCCTTCDTO != null)
                                        {
                                            foreach (var detReg in regHojaCCTTCDTO.Det2RegHojaCCTTCDTO)
                                            {
                                                Det2RegHojaCCTTCDTO regDetHojaCCTTCDTO = detReg;
                                                regDetHojaCCTTCDTO.Det2centermhccodi = campaniasAppService.GetLastDet2RegHojaCCTTCId();
                                                regDetHojaCCTTCDTO.Centralcodi = regHojaCCTTCDTO.Centralcodi;
                                                regDetHojaCCTTCDTO.IndDel = Constantes.IndDel;
                                                regDetHojaCCTTCDTO.Usucreacion = User.Identity.Name;
                                                campaniasAppService.SaveDet2RegHojaCCTTC(regDetHojaCCTTCDTO);
                                            }
                                        }
                                    }   

                                }
                                else if (transmisionProyectoDTO.Tipoficodi == SubTipoProyecto.CentralEólica)
                                {
                                    // TIPO GENERACIÓN
                                    //// Subtipo Eólica
                                    RegHojaEolADTO regHojaEolADTO = campaniasAppService.GetRegHojaEolAById(idProy);
                                    if (regHojaEolADTO.ProyCodi != 0)
                                    {
                                        List<RegHojaEolADetDTO> regHojaEolADetDTOs = campaniasAppService.GetRegHojaEolADetCodi(regHojaEolADTO.CentralACodi);
                                        regHojaEolADTO.RegHojaEolADetDTOs = regHojaEolADetDTOs;
                                        regHojaEolADTO.CentralACodi = campaniasAppService.GetLastRegHojaEolAId();
                                        regHojaEolADTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        regHojaEolADTO.IndDel = Constantes.IndDel;
                                        regHojaEolADTO.UsuCreacion = User.Identity.Name;
                                        if (campaniasAppService.SaveRegHojaEolA(regHojaEolADTO))
                                        {
                                            if (regHojaEolADTO.RegHojaEolADetDTOs != null)
                                            {
                                                foreach (var item in regHojaEolADTO.RegHojaEolADetDTOs)
                                                {
                                                    RegHojaEolADetDTO regHojaEolADetDTO = item;
                                                    regHojaEolADetDTO.CentralACodi = regHojaEolADTO.CentralACodi;
                                                    regHojaEolADetDTO.CentralADetCodi = campaniasAppService.GetLastRegHojaEolADetId();
                                                    regHojaEolADetDTO.IndDel = Constantes.IndDel;
                                                    regHojaEolADetDTO.UsuCreacion = User.Identity.Name;
                                                    campaniasAppService.SaveRegHojaEolADet(regHojaEolADetDTO);
                                                }
                                            }
                                        }
                                    }

                                    RegHojaEolBDTO regHojaEolBDTO = campaniasAppService.GetRegHojaEolBById(idProy);
                                    if (regHojaEolBDTO.Proycodi != 0)
                                    {
                                        regHojaEolBDTO.CentralBCodi = campaniasAppService.GetLastRegHojaEolBId();
                                        regHojaEolBDTO.Proycodi = transmisionProyectoDTO.Proycodi;
                                        regHojaEolBDTO.IndDel = Constantes.IndDel;
                                        regHojaEolBDTO.Usucreacion = User.Identity.Name;
                                        campaniasAppService.SaveRegHojaEolB(regHojaEolBDTO);
                                    }   

                                    RegHojaEolCDTO regHojaEolCDTO = campaniasAppService.GetRegHojaEolCById(idProy);
                                    if (regHojaEolCDTO.Proycodi != 0) {
                                        List<DetRegHojaEolCDTO> DetRegHojaEolCDTO = campaniasAppService.GetDetRegHojaEolCCodi(regHojaEolCDTO.CentralCCodi);
                                        regHojaEolCDTO.CentralCCodi = campaniasAppService.GetLastRegHojaEolCId();
                                        regHojaEolCDTO.Proycodi = transmisionProyectoDTO.Proycodi;
                                        regHojaEolCDTO.IndDel = Constantes.IndDel;
                                        regHojaEolCDTO.Usucreacion = User.Identity.Name;
                                        if (campaniasAppService.SaveRegHojaEolC(regHojaEolCDTO))
                                        {
                                            if (DetRegHojaEolCDTO != null) 
                                            {
                                                foreach (var item in DetRegHojaEolCDTO)
                                                {
                                                    DetRegHojaEolCDTO detRegHojaEolCDTO = item;
                                                    detRegHojaEolCDTO.DetEloCCodi = campaniasAppService.GetLastDetRegHojaEolCId();
                                                    detRegHojaEolCDTO.Centralccodi = regHojaEolCDTO.CentralCCodi;
                                                    detRegHojaEolCDTO.IndDel = Constantes.IndDel;
                                                    detRegHojaEolCDTO.Usucreacion = User.Identity.Name;
                                                    campaniasAppService.SaveDetRegHojaEolC(detRegHojaEolCDTO);
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (transmisionProyectoDTO.Tipoficodi == SubTipoProyecto.CentralSolar)
                                {
                                    // TIPO GENERACIÓN
                                    //// Subtipo Solar
                                    SolHojaADTO solHojaADTO = campaniasAppService.GetSolHojaAById(idProy);
                                    if (solHojaADTO.Proycodi != 0)
                                    {
                                        solHojaADTO.Solhojaacodi = campaniasAppService.GetLastSolHojaAId();
                                        solHojaADTO.Proycodi = transmisionProyectoDTO.Proycodi;
                                        solHojaADTO.IndDel = Constantes.IndDel;
                                        solHojaADTO.UsuCreacion = User.Identity.Name;
                                        campaniasAppService.SaveSolHojaA(solHojaADTO);
                                    }   

                                    SolHojaBDTO solHojaBDTO = campaniasAppService.GetSolHojaBById(idProy);
                                    if (solHojaBDTO.ProyCodi != 0)
                                    {
                                        solHojaBDTO.SolhojabCodi = campaniasAppService.GetLastSolHojaBId();
                                        solHojaBDTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        solHojaBDTO.IndDel = Constantes.IndDel;
                                        solHojaBDTO.UsuCreacion = User.Identity.Name;
                                        campaniasAppService.SaveSolHojaB(solHojaBDTO);
                                    }

                                    SolHojaCDTO SolHojaCDTO = campaniasAppService.GetSolHojaCById(idProy);
                                    if (SolHojaCDTO.Proycodi != 0)
                                    {
                                        SolHojaCDTO.ListaDetSolHojaCDTO = campaniasAppService.GetDetSolHojaCCodi(SolHojaCDTO.Solhojaccodi);
                                        SolHojaCDTO.Solhojaccodi = campaniasAppService.GetLastSolHojaCId();
                                        SolHojaCDTO.Proycodi = transmisionProyectoDTO.Proycodi;
                                        SolHojaCDTO.IndDel = Constantes.IndDel;
                                        SolHojaCDTO.Usucreacion = User.Identity.Name;
                                        if (campaniasAppService.SaveSolHojaC(SolHojaCDTO))
                                        {
                                            if (SolHojaCDTO.ListaDetSolHojaCDTO != null)
                                            {
                                                foreach (var item in SolHojaCDTO.ListaDetSolHojaCDTO)
                                                {
                                                    DetSolHojaCDTO DetSolHojaCDTO = item;
                                                    DetSolHojaCDTO.Detasolhccodi = campaniasAppService.GetLastDetSolHojaCId();
                                                    DetSolHojaCDTO.Solhojaccodi = SolHojaCDTO.Solhojaccodi;
                                                    DetSolHojaCDTO.IndDel = Constantes.IndDel;
                                                    DetSolHojaCDTO.Usucreacion = User.Identity.Name;
                                                    campaniasAppService.SaveDetSolHojaC(DetSolHojaCDTO);
                                                }
                                            }
                                        }
                                    }  
                                }
                                else if (transmisionProyectoDTO.Tipoficodi == SubTipoProyecto.CentralBiomasa)
                                {
                                    // TIPO GENERACIÓN
                                    //// Subtipo Biomasa
                                    BioHojaADTO BioHojaADTO = campaniasAppService.GetBioHojaAById(idProy);
                                    if (BioHojaADTO.ProyCodi != 0)
                                    {
                                        BioHojaADTO.BiohojaaCodi = campaniasAppService.GetLastBioHojaAId();
                                        BioHojaADTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        BioHojaADTO.IndDel = Constantes.IndDel;
                                        BioHojaADTO.UsuCreacion = User.Identity.Name;
                                        campaniasAppService.SaveBioHojaA(BioHojaADTO);
                                    }

                                    BioHojaBDTO BioHojaBDTO = campaniasAppService.GetBioHojaBById(idProy);
                                    if (BioHojaBDTO.ProyCodi != 0)
                                    {
                                        BioHojaBDTO.BiohojabCodi = campaniasAppService.GetLastBioHojaBId();
                                        BioHojaBDTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        BioHojaBDTO.IndDel = Constantes.IndDel;
                                        BioHojaBDTO.UsuCreacion = User.Identity.Name;
                                        campaniasAppService.SaveBioHojaB(BioHojaBDTO);
                                    } 

                                    BioHojaCDTO BioHojaCDTO = campaniasAppService.GetBioHojaCById(idProy);
                                    if (BioHojaCDTO.Proycodi != 0)
                                    {
                                        BioHojaCDTO.ListaDetBioHojaCDTO = campaniasAppService.GetDetBioHojaCCodi(BioHojaCDTO.Biohojaccodi);
                                        BioHojaCDTO.Biohojaccodi = campaniasAppService.GetLastBioHojaCId();
                                        BioHojaCDTO.Proycodi = transmisionProyectoDTO.Proycodi;
                                        BioHojaCDTO.IndDel = Constantes.IndDel;
                                        BioHojaCDTO.Usucreacion = User.Identity.Name;
                                        if (campaniasAppService.SaveBioHojaC(BioHojaCDTO))
                                        {
                                            if (BioHojaCDTO.ListaDetBioHojaCDTO != null)
                                            {
                                                foreach (var item in BioHojaCDTO.ListaDetBioHojaCDTO)
                                                {
                                                    DetBioHojaCDTO DetBioHojaCDTO = item;
                                                    DetBioHojaCDTO.Detabiohccodi = campaniasAppService.GetLastDetBioHojaCId();
                                                    DetBioHojaCDTO.Biohojaccodi = BioHojaCDTO.Biohojaccodi;
                                                    DetBioHojaCDTO.IndDel = Constantes.IndDel;
                                                    DetBioHojaCDTO.Usucreacion = User.Identity.Name;
                                                    campaniasAppService.SaveDetBioHojaC(DetBioHojaCDTO);
                                                }
                                            }
                                        }
                                    }
                                }
                                // TIPO GENERACIÓN
                                //// Líneas
                                LineasFichaADTO lineasFicha1DTO = campaniasAppService.GetLineasFichaAById(idProy);
                                if (lineasFicha1DTO.ProyCodi != 0)
                                {
                                    lineasFicha1DTO.LineasFichaADet1DTO = campaniasAppService.GetLineasFichaADet1Codi(lineasFicha1DTO.LinFichaACodi);
                                    lineasFicha1DTO.LineasFichaADet2DTO = campaniasAppService.GetLineasFichaADet2Codi(lineasFicha1DTO.LinFichaACodi);
                                    lineasFicha1DTO.LinFichaACodi = campaniasAppService.GetLastLineasFichaAId();
                                    lineasFicha1DTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                    lineasFicha1DTO.IndDel = Constantes.IndDel;
                                    lineasFicha1DTO.UsuCreacion = User.Identity.Name;
                                    campaniasAppService.SaveLineasFichaA(lineasFicha1DTO);
                                    if (lineasFicha1DTO.LineasFichaADet1DTO != null)
                                    {
                                        foreach (var detReg in lineasFicha1DTO.LineasFichaADet1DTO)
                                        {
                                            LineasFichaADet1DTO lineasFichaADet1DTO = detReg;
                                            lineasFichaADet1DTO.LinFichaADet1Codi = campaniasAppService.GetLastLineasFichaADet1Id();
                                            lineasFichaADet1DTO.LinFichaACodi = lineasFicha1DTO.LinFichaACodi;
                                            lineasFichaADet1DTO.IndDel = Constantes.IndDel;
                                            lineasFichaADet1DTO.UsuCreacion = User.Identity.Name;
                                            campaniasAppService.SaveLineasFichaADet1(lineasFichaADet1DTO);
                                        }
                                    }

                                    if (lineasFicha1DTO.LineasFichaADet2DTO != null)
                                    {
                                        foreach (var detReg in lineasFicha1DTO.LineasFichaADet2DTO)
                                        {
                                            LineasFichaADet2DTO lineasFicha1Det2DTO = detReg;
                                            lineasFicha1Det2DTO.LinFichaADet2Codi = campaniasAppService.GetLastLineasFichaADet2Id();
                                            lineasFicha1Det2DTO.LinFichaACodi = lineasFicha1DTO.LinFichaACodi;
                                            lineasFicha1Det2DTO.IndDel = Constantes.IndDel;
                                            lineasFicha1Det2DTO.UsuCreacion = User.Identity.Name;
                                            campaniasAppService.SaveLineasFichaADet2(lineasFicha1Det2DTO);
                                        }
                                    }

                                }

                                LineasFichaBDTO lineasFichaBDTO = campaniasAppService.GetLineasFichaBById(idProy);
                                if (lineasFichaBDTO.ProyCodi != 0)
                                {
                                    lineasFichaBDTO.LineasFichaBDetDTO = campaniasAppService.GetLineasFichaBDetCodi(lineasFichaBDTO.FichaBCodi);
                                    lineasFichaBDTO.FichaBCodi = campaniasAppService.GetLastLineasFichaBId();
                                    lineasFichaBDTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                    lineasFichaBDTO.IndDel = Constantes.IndDel;
                                    lineasFichaBDTO.UsuCreacion = User.Identity.Name;
                                    campaniasAppService.SaveLineasFichaB(lineasFichaBDTO);
                                    if (lineasFichaBDTO.LineasFichaBDetDTO != null) {
                                        foreach (var item in lineasFichaBDTO.LineasFichaBDetDTO)
                                        {
                                            LineasFichaBDetDTO lineasFichaBDetDTO = item;
                                            lineasFichaBDetDTO.FichaBDetCodi = campaniasAppService.GetLastLineasFichaBDetId();
                                            lineasFichaBDetDTO.FichaBCodi = lineasFichaBDTO.FichaBCodi;
                                            lineasFichaBDetDTO.IndDel = Constantes.IndDel;
                                            lineasFichaBDetDTO.UsuCreacion = User.Identity.Name;
                                            campaniasAppService.SaveLineasFichaBDet(lineasFichaBDetDTO);
                                        }
                                    }
                                }

                                //// Subestaciones
                                SubestFicha1DTO subestFicha1DTO = campaniasAppService.GetSubestFicha1ById(idProy);
                                if (subestFicha1DTO.ProyCodi != 0)
                                {
                                    List<SubestFicha1Det1DTO> Listas1DTO = campaniasAppService.GetSubestFicha1Det1ById(subestFicha1DTO.SubestFicha1Codi);
                                    List<SubestFicha1Det2DTO> Listas2DTO = campaniasAppService.GetSubestFicha1Det2ById(subestFicha1DTO.SubestFicha1Codi);
                                    List<SubestFicha1Det3DTO> Listas3DTO = campaniasAppService.GetSubestFicha1Det3ById(subestFicha1DTO.SubestFicha1Codi);
                                    subestFicha1DTO.Lista1DTOs = Listas1DTO;
                                    subestFicha1DTO.Lista2DTOs = Listas2DTO;
                                    subestFicha1DTO.Lista3DTOs = Listas3DTO;
                                    subestFicha1DTO.SubestFicha1Codi = campaniasAppService.GetLastSubestFicha1Id();
                                    subestFicha1DTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                    subestFicha1DTO.IndDel = Constantes.IndDel;
                                    subestFicha1DTO.UsuCreacion = User.Identity.Name;
                                    campaniasAppService.SaveSubestFicha1(subestFicha1DTO);
                                    if (subestFicha1DTO.Lista1DTOs != null)
                                    {
                                        foreach (var detReg in subestFicha1DTO.Lista1DTOs)
                                        {
                                            SubestFicha1Det1DTO subestFicha1Det1DTO = detReg;
                                            subestFicha1Det1DTO.SubestFicha1Det1Codi = campaniasAppService.GetLastSubestFicha1Det1Id();
                                            subestFicha1Det1DTO.SubestFicha1Codi = subestFicha1DTO.SubestFicha1Codi;
                                            subestFicha1Det1DTO.IndDel = Constantes.IndDel;
                                            subestFicha1Det1DTO.UsuCreacion = User.Identity.Name;
                                            campaniasAppService.SaveSubestFicha1Det1(subestFicha1Det1DTO);
                                        }
                                    }
                                    if (subestFicha1DTO.Lista2DTOs != null)
                                    {
                                        foreach (var detReg in subestFicha1DTO.Lista2DTOs)
                                        {
                                            SubestFicha1Det2DTO subestFicha1Det2DTO = detReg;
                                            subestFicha1Det2DTO.SubestFicha1Det2Codi = campaniasAppService.GetLastSubestFicha1Det2Id();
                                            subestFicha1Det2DTO.SubestFicha1Codi = subestFicha1DTO.SubestFicha1Codi;
                                            subestFicha1Det2DTO.IndDel = Constantes.IndDel;
                                            subestFicha1Det2DTO.UsuCreacion = User.Identity.Name;
                                            campaniasAppService.SaveSubestFicha1Det2(subestFicha1Det2DTO);
                                        }
                                    }
                                    if (subestFicha1DTO.Lista3DTOs != null)
                                    {
                                        foreach (var detReg in subestFicha1DTO.Lista3DTOs)
                                        {
                                            SubestFicha1Det3DTO subestFicha1Det3DTO = detReg;
                                            subestFicha1Det3DTO.SubestFicha1Det3Codi = campaniasAppService.GetLastSubestFicha1Det3Id();
                                            subestFicha1Det3DTO.SubestFicha1Codi = subestFicha1DTO.SubestFicha1Codi;
                                            subestFicha1Det3DTO.IndDel = Constantes.IndDel;
                                            subestFicha1Det3DTO.UsuCreacion = User.Identity.Name;
                                            campaniasAppService.SaveSubestFicha1Det3(subestFicha1Det3DTO);
                                        }
                                    }
                                }  
                            }
                            else if (transmisionProyectoDTO.Tipocodi == TipoProyecto.Transmision) 
                            {
                                // TIPO TRANSMISIÓN
                                T1LinFichaADTO lineasFicha1DTO = campaniasAppService.GetLineasT1FichaAById(idProy);
                                if (lineasFicha1DTO.ProyCodi != 0)
                                {
                                    lineasFicha1DTO.LineasFichaADet1DTO = campaniasAppService.GetT1LineasFichaADet1Codi(lineasFicha1DTO.LinFichaACodi);
                                    lineasFicha1DTO.LineasFichaADet2DTO = campaniasAppService.GetT1LineasFichaADet2Codi(lineasFicha1DTO.LinFichaACodi);
                                    lineasFicha1DTO.LinFichaACodi = campaniasAppService.GetLastT1LineasFichaAId();
                                    lineasFicha1DTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                    lineasFicha1DTO.IndDel = Constantes.IndDel;
                                    lineasFicha1DTO.UsuCreacion = User.Identity.Name;
                                    campaniasAppService.SaveT1LineasFichaA(lineasFicha1DTO);

                                    if (lineasFicha1DTO.LineasFichaADet1DTO != null)
                                    {
                                        foreach (var detReg in lineasFicha1DTO.LineasFichaADet1DTO)
                                        {
                                            T1LinFichaADet1DTO lineasFichaADet1DTO = detReg;
                                            lineasFichaADet1DTO.LinFichaADet1Codi = campaniasAppService.GetLastT1LineasFichaADet1Id();
                                            lineasFichaADet1DTO.LinFichaACodi = lineasFicha1DTO.LinFichaACodi;
                                            lineasFichaADet1DTO.IndDel = Constantes.IndDel;
                                            lineasFichaADet1DTO.UsuCreacion = User.Identity.Name;
                                            campaniasAppService.SaveT1LineasFichaADet1(lineasFichaADet1DTO);
                                        }
                                    }

                                    if (lineasFicha1DTO.LineasFichaADet2DTO != null)
                                    {
                                        foreach (var detReg in lineasFicha1DTO.LineasFichaADet2DTO)
                                        {
                                            T1LinFichaADet2DTO lineasFicha1Det2DTO = detReg;
                                            lineasFicha1Det2DTO.LinFichaADet2Codi = campaniasAppService.GetLastT1LineasFichaADet2Id();
                                            lineasFicha1Det2DTO.LinFichaACodi = lineasFicha1DTO.LinFichaACodi;
                                            lineasFicha1Det2DTO.IndDel = Constantes.IndDel;
                                            lineasFicha1Det2DTO.UsuCreacion = User.Identity.Name;
                                            campaniasAppService.SaveT1LineasFichaADet2(lineasFicha1Det2DTO);
                                        }
                                    }
                                }  

                                T2SubestFicha1DTO subestFicha1DTO = campaniasAppService.GetT2SubestFicha1ById(idProy);
                                if (subestFicha1DTO.ProyCodi != 0)
                                {
                                    List<T2SubestFicha1Det1DTO> Lista1DTO = campaniasAppService.GetT2SubestFicha1Det1ById(subestFicha1DTO.SubestFicha1Codi);
                                    List<T2SubestFicha1Det2DTO> Lista2DTO = campaniasAppService.GetT2SubestFicha1Det2ById(subestFicha1DTO.SubestFicha1Codi);
                                    List<T2SubestFicha1Det3DTO> Lista3DTO = campaniasAppService.GetT2SubestFicha1Det3ById(subestFicha1DTO.SubestFicha1Codi);
                                    subestFicha1DTO.Lista1DTOs = Lista1DTO;
                                    subestFicha1DTO.Lista2DTOs = Lista2DTO;
                                    subestFicha1DTO.Lista3DTOs = Lista3DTO;
                                    subestFicha1DTO.SubestFicha1Codi = campaniasAppService.GetLastT2SubestFicha1Id();
                                    subestFicha1DTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                    subestFicha1DTO.IndDel = Constantes.IndDel;
                                    subestFicha1DTO.UsuCreacion = User.Identity.Name;
                                    campaniasAppService.SaveT2SubestFicha1(subestFicha1DTO);

                                    if (subestFicha1DTO.Lista1DTOs != null)
                                    {
                                        foreach (var detReg in subestFicha1DTO.Lista1DTOs)
                                        {
                                            T2SubestFicha1Det1DTO subestFicha1Det1DTO = detReg;
                                            subestFicha1Det1DTO.SubestFicha1Det1Codi = campaniasAppService.GetLastT2SubestFicha1Det1Id();
                                            subestFicha1Det1DTO.SubestFicha1Codi = subestFicha1DTO.SubestFicha1Codi;
                                            subestFicha1Det1DTO.IndDel = Constantes.IndDel;
                                            subestFicha1Det1DTO.UsuCreacion = User.Identity.Name;
                                            campaniasAppService.SaveT2SubestFicha1Det1(subestFicha1Det1DTO);
                                        }
                                    }
                                    if (subestFicha1DTO.Lista2DTOs != null)
                                    {
                                        foreach (var detReg in subestFicha1DTO.Lista2DTOs)
                                        {
                                            T2SubestFicha1Det2DTO subestFicha1Det2DTO = detReg;
                                            subestFicha1Det2DTO.SubestFicha1Det2Codi = campaniasAppService.GetLastT2SubestFicha1Det2Id();
                                            subestFicha1Det2DTO.SubestFicha1Codi = subestFicha1DTO.SubestFicha1Codi;
                                            subestFicha1Det2DTO.IndDel = Constantes.IndDel;
                                            subestFicha1Det2DTO.UsuCreacion = User.Identity.Name;
                                            campaniasAppService.SaveT2SubestFicha1Det2(subestFicha1Det2DTO);
                                        }
                                    }
                                    if (subestFicha1DTO.Lista3DTOs != null)
                                    {
                                        foreach (var detReg in subestFicha1DTO.Lista3DTOs)
                                        {
                                            T2SubestFicha1Det3DTO subestFicha1Det3DTO = detReg;
                                            subestFicha1Det3DTO.SubestFicha1Det3Codi = campaniasAppService.GetLastT2SubestFicha1Det3Id();
                                            subestFicha1Det3DTO.SubestFicha1Codi = subestFicha1DTO.SubestFicha1Codi;
                                            subestFicha1Det3DTO.IndDel = Constantes.IndDel;
                                            subestFicha1Det3DTO.UsuCreacion = User.Identity.Name;
                                            campaniasAppService.SaveT2SubestFicha1Det3(subestFicha1Det3DTO);
                                        }
                                    }
                                }

                                CroFicha1DTO CroFicha1dto = campaniasAppService.GetCroFicha1ById(idProy);
                                if (CroFicha1dto.Proycodi != 0)
                                {
                                    CroFicha1dto.ListaCroFicha1DetDTO = campaniasAppService.GetCroFicha1DetCodi(CroFicha1dto.CroFicha1codi);
                                    CroFicha1dto.CroFicha1codi = campaniasAppService.GetLastCroFicha1Id();
                                    CroFicha1dto.Proycodi = transmisionProyectoDTO.Proycodi;
                                    CroFicha1dto.IndDel = Constantes.IndDel;
                                    CroFicha1dto.Usucreacion = User.Identity.Name;
                                    if (campaniasAppService.SaveCroFicha1(CroFicha1dto))
                                    {
                                        if (CroFicha1dto.ListaCroFicha1DetDTO != null)
                                        {
                                            foreach (var item in CroFicha1dto.ListaCroFicha1DetDTO)
                                            {
                                                CroFicha1DetDTO CroFicha1DetDTO = item;
                                                CroFicha1DetDTO.CroFicha1Detcodi = campaniasAppService.GetLastCroFicha1DetId();
                                                CroFicha1DetDTO.CroFicha1codi = CroFicha1dto.CroFicha1codi;
                                                CroFicha1DetDTO.IndDel = Constantes.IndDel;
                                                CroFicha1DetDTO.Usucreacion = User.Identity.Name;
                                                campaniasAppService.SaveCroFicha1Det(CroFicha1DetDTO);
                                            }
                                        }
                                    }
                                }

                            }
                            else if (transmisionProyectoDTO.Tipocodi == TipoProyecto.ITC) 
                            {
                                // TIPO ITC
                                //// Demanda EDE
                                List<Itcdf104DTO> itcdf104DTOS = campaniasAppService.GetItcdf104ById(idProy);
                                if (itcdf104DTOS != null) {
                                    foreach(var detReg in itcdf104DTOS)
                                    {
                                        Itcdf104DTO ObjItcdf104 = detReg;
                                        ObjItcdf104.Itcdf104Codi = campaniasAppService.GetLastItcdf104Id();
                                        ObjItcdf104.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        ObjItcdf104.IndDel = Constantes.IndDel;
                                        ObjItcdf104.UsuCreacion = User.Identity.Name;
                                        campaniasAppService.SaveItcdf104(ObjItcdf104);
                                    }
                                }

                                List<Itcdf108DTO> itcdf108DTOS = campaniasAppService.GetItcdf108ById(idProy);
                                if (itcdf108DTOS != null)
                                {
                                    foreach (var detReg in itcdf108DTOS)
                                    {
                                        Itcdf108DTO ObjItcdf108 = detReg;
                                        ObjItcdf108.Itcdf108Codi = campaniasAppService.GetLastItcdf108Id();
                                        ObjItcdf108.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        ObjItcdf108.IndDel = Constantes.IndDel;
                                        ObjItcdf108.UsuCreacion = User.Identity.Name;
                                        campaniasAppService.SaveItcdf108(ObjItcdf108);
                                    }
                                }

                                List<Itcdf110DTO> itcdf110DTOs = campaniasAppService.GetItcdf110ById(idProy);
                                for (int i = 0; i < itcdf110DTOs.Count; i++)
                                {
                                    itcdf110DTOs[i].ListItcdf110Det = campaniasAppService.GetItcdf110DetCodi(itcdf110DTOs[i].Itcdf110Codi);
                                }
                                if (itcdf110DTOs != null) {
                                    foreach (var item in itcdf110DTOs)
                                    {
                                        Itcdf110DTO itcdf110DTO = item;
                                        itcdf110DTO.Itcdf110Codi = campaniasAppService.GetLastItcdf110Id();
                                        itcdf110DTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        itcdf110DTO.IndDel = Constantes.IndDel;
                                        itcdf110DTO.UsuCreacion = User.Identity.Name;
                                        campaniasAppService.SaveItcdf110(itcdf110DTO);
                                        if (item.ListItcdf110Det != null) {
                                            foreach (var item1 in item.ListItcdf110Det)
                                            {
                                                Itcdf110DetDTO itcdf110DetDTO = item1;
                                                itcdf110DetDTO.IndDel = Constantes.IndDel;
                                                itcdf110DetDTO.UsuCreacion = User.Identity.Name;
                                                itcdf110DetDTO.Itcdf110DetCodi = campaniasAppService.GetLastItcdf110DetId();
                                                itcdf110DetDTO.Itcdf110Codi = itcdf110DTO.Itcdf110Codi;
                                                campaniasAppService.SaveItcdf110Det(itcdf110DetDTO);
                                            }
                                        }
                                    }
                                }

                                List<Itcdf116DTO> itcdf116DTOs = campaniasAppService.GetItcdf116ById(idProy);
                                for (int i = 0; i < itcdf116DTOs.Count; i++)
                                {
                                    itcdf116DTOs[i].ListItcdf116Det = campaniasAppService.GetItcdf116DetCodi(itcdf116DTOs[i].Itcdf116Codi);
                                }
                                if (itcdf116DTOs != null) {
                                    foreach (var item in itcdf116DTOs)
                                    {
                                        Itcdf116DTO itcdf116DTO = item;
                                        itcdf116DTO.Itcdf116Codi = campaniasAppService.GetLastItcdf116Id();
                                        itcdf116DTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        itcdf116DTO.IndDel = Constantes.IndDel;
                                        itcdf116DTO.UsuCreacion = User.Identity.Name;
                                        campaniasAppService.SaveItcdf116(itcdf116DTO);
                                        if (item.ListItcdf116Det != null) {
                                            foreach (var item1 in item.ListItcdf116Det)
                                            {
                                                Itcdf116DetDTO itcdf116DetDTO = item1;
                                                itcdf116DetDTO.IndDel = Constantes.IndDel;
                                                itcdf116DetDTO.UsuCreacion = User.Identity.Name;
                                                itcdf116DetDTO.Itcdf116DetCodi = campaniasAppService.GetLastItcdf116DetId();
                                                itcdf116DetDTO.Itcdf116Codi = itcdf116DTO.Itcdf116Codi;
                                                campaniasAppService.SaveItcdf116Det(itcdf116DetDTO);
                                            }
                                        }
                                    }
                                }

                                List<Itcdf121DTO> itcdf121DTOs = campaniasAppService.GetItcdf121ById(idProy);
                                for (int i = 0; i < itcdf121DTOs.Count; i++)
                                {
                                    itcdf121DTOs[i].ListItcdf121Det = campaniasAppService.GetItcdf121DetCodi(itcdf121DTOs[i].Itcdf121Codi);
                                }
                                if (itcdf121DTOs != null) {
                                    foreach (var item in itcdf121DTOs)
                                    {
                                        Itcdf121DTO itcdf121DTO = item;
                                        itcdf121DTO.Itcdf121Codi = campaniasAppService.GetLastItcdf121Id();
                                        itcdf121DTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        itcdf121DTO.IndDel = Constantes.IndDel;
                                        itcdf121DTO.UsuCreacion = User.Identity.Name;
                                        campaniasAppService.SaveItcdf121(itcdf121DTO);
                                        if (item.ListItcdf121Det != null) {
                                            foreach (var item1 in item.ListItcdf121Det)
                                            {
                                                Itcdf121DetDTO itcdf121DetDTO = item1;
                                                itcdf121DetDTO.IndDel = Constantes.IndDel;
                                                itcdf121DetDTO.UsuCreacion = User.Identity.Name;
                                                itcdf121DetDTO.Itcdf121DetCodi = campaniasAppService.GetLastItcdf121DetId();
                                                itcdf121DetDTO.Itcdf121Codi = itcdf121DTO.Itcdf121Codi;
                                                campaniasAppService.SaveItcdf121Det(itcdf121DetDTO);
                                            }
                                        } 
                                    }
                                }

                                List<Itcdf123DTO> itcdf123DTOs = campaniasAppService.GetItcdf123ById(idProy);
                                if (itcdf123DTOs != null)
                                {
                                    foreach (var detReg in itcdf123DTOs)
                                    {
                                        Itcdf123DTO ObjItcdf123 = detReg;
                                        ObjItcdf123.Itcdf123Codi = campaniasAppService.GetLastItcdf123Id();
                                        ObjItcdf123.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        ObjItcdf123.IndDel = Constantes.IndDel;
                                        ObjItcdf123.UsuCreacion = User.Identity.Name;
                                        campaniasAppService.SaveItcdf123(ObjItcdf123);
                                    }
                                }

                                Itcdfp011DTO itcdfp011DTO = campaniasAppService.GetItcdfp011ById(idProy);
                                if (itcdfp011DTO.ProyCodi != 0)
                                {
                                    var codItcdfp011 = itcdfp011DTO.Itcdfp011Codi;
                                    //itcdfp011DTO.ListItcdf011Det = campaniasAppService.GetItcdfp011DetById(itcdfp011DTO.Itcdfp011Codi);
                                    itcdfp011DTO.Itcdfp011Codi = campaniasAppService.GetLastItcdfp011Id();
                                    itcdfp011DTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                    itcdfp011DTO.IndDel = Constantes.IndDel;
                                    itcdfp011DTO.UsuCreacion = User.Identity.Name;
                                    campaniasAppService.SaveItcdfp011(itcdfp011DTO);
                                    campaniasAppService.GetCloneItcdfp011DetById(codItcdfp011, itcdfp011DTO.Itcdfp011Codi);
                                    // if (itcdfp011DTO.ListItcdf011Det != null)
                                    // {
                                    //     foreach (var detReg in itcdfp011DTO.ListItcdf011Det)
                                    //     {
                                    //         Itcdfp011DetDTO Itcdfp011DetDTO = detReg;
                                    //         //Itcdfp011DetDTO.Itcdfp011DetCodi = campaniasAppService.GetLastItcdfp011DetId();
                                    //         Itcdfp011DetDTO.Itcdfp011Codi = itcdfp011DTO.Itcdfp011Codi;
                                    //         Itcdfp011DetDTO.IndDel = Constantes.IndDel;
                                    //         Itcdfp011DetDTO.UsuCreacion = User.Identity.Name;
                                    //         campaniasAppService.SaveItcdfp011Det(Itcdfp011DetDTO);
                                    //     }
                                    // }
                                }

                                List<Itcdfp012DTO> itcdfp012DTOs = campaniasAppService.GetItcdfp012ById(idProy);
                                if (itcdfp012DTOs != null)
                                {
                                    foreach (var detReg in itcdfp012DTOs)
                                    {
                                        Itcdfp012DTO ObjItcdfp102 = detReg;
                                        ObjItcdfp102.Itcdfp012Codi = campaniasAppService.GetLastItcdfp012Id();
                                        ObjItcdfp102.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        ObjItcdfp102.IndDel = Constantes.IndDel;
                                        ObjItcdfp102.UsuCreacion = User.Identity.Name;
                                        campaniasAppService.SaveItcdfp012(ObjItcdfp102);
                                    }
                                }

                                List<Itcdfp013DTO> Itcdfp013DTOs = campaniasAppService.GetItcdfp013ById(idProy);
                                for (int i = 0; i < Itcdfp013DTOs.Count; i++)
                                {
                                    Itcdfp013DTOs[i].ListItcdf013Det = campaniasAppService.GetItcdfp013DetCodi(Itcdfp013DTOs[i].Itcdfp013Codi);
                                }
                                if (Itcdfp013DTOs != null) {
                                    foreach (var item in Itcdfp013DTOs)
                                    {
                                        Itcdfp013DTO itcdfp013DTO = item;
                                        itcdfp013DTO.Itcdfp013Codi = campaniasAppService.GetLastItcdfp013Id();
                                        itcdfp013DTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        itcdfp013DTO.IndDel = Constantes.IndDel;
                                        itcdfp013DTO.UsuCreacion = User.Identity.Name;
                                        campaniasAppService.SaveItcdfp013(itcdfp013DTO);
                                        if (item.ListItcdf013Det != null) {
                                            foreach (var item1 in item.ListItcdf013Det)
                                            {
                                                Itcdfp013DetDTO itcdfp013DetDTO = item1;
                                                itcdfp013DetDTO.IndDel = Constantes.IndDel;
                                                itcdfp013DetDTO.UsuCreacion = User.Identity.Name;
                                                itcdfp013DetDTO.Itcdfp013DetCodi = campaniasAppService.GetLastItcdfp013DetId();
                                                itcdfp013DetDTO.Itcdfp013Codi = itcdfp013DTO.Itcdfp013Codi;
                                                campaniasAppService.SaveItcdfp013Det(itcdfp013DetDTO);
                                            }
                                        }
                                    }
                                }

                                //// Sistema Eléctrico y Parámetros
                                List<ItcPrm1Dto> itcPrm1Dtos = campaniasAppService.GetItcprm1ById(idProy);
                                if (itcPrm1Dtos != null)
                                {
                                    foreach (var detReg in itcPrm1Dtos)
                                    {
                                        ItcPrm1Dto itcPrm1Dto = detReg;
                                        itcPrm1Dto.ItcPrm1Codi = campaniasAppService.GetLastItcprm1Id();
                                        itcPrm1Dto.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        itcPrm1Dto.IndDel = Constantes.IndDel;
                                        itcPrm1Dto.Usucreacion = User.Identity.Name;
                                        campaniasAppService.SaveItcprm1(itcPrm1Dto);
                                    }
                                }

                                List<ItcPrm2Dto> itcPrm2Dtos = campaniasAppService.GetItcprm2ById(idProy);
                                if (itcPrm2Dtos != null)
                                {
                                    foreach (var detReg in itcPrm2Dtos)
                                    {
                                        ItcPrm2Dto itcPrm2Dto = detReg;
                                        itcPrm2Dto.ItcPrm2Codi = campaniasAppService.GetLastItcprm2Id();
                                        itcPrm2Dto.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        itcPrm2Dto.IndDel = Constantes.IndDel;
                                        itcPrm2Dto.Usucreacion = User.Identity.Name;
                                        campaniasAppService.SaveItcprm2(itcPrm2Dto);
                                    }
                                }

                                List<ItcRed1Dto> itcRed1Dto = campaniasAppService.GetItcred1ById(idProy);
                                if (itcRed1Dto != null)
                                {
                                    foreach (var detReg in itcRed1Dto)
                                    {
                                        ItcRed1Dto itcred1Dto = detReg;
                                        itcred1Dto.ItcRed1Codi = campaniasAppService.GetLastItcred1Id();
                                        itcred1Dto.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        itcred1Dto.IndDel = Constantes.IndDel;
                                        itcred1Dto.Usucreacion = User.Identity.Name;
                                        campaniasAppService.SaveItcred1(itcred1Dto);
                                    }
                                }

                                List<ItcRed2Dto> ItcRed2Dtos = campaniasAppService.GetItcred2ById(idProy);
                                if (ItcRed2Dtos != null)
                                {
                                    foreach (var detReg in ItcRed2Dtos)
                                    {
                                        ItcRed2Dto itcred2Dto = detReg;
                                        itcred2Dto.ItcRed2Codi = campaniasAppService.GetLastItcred2Id();
                                        itcred2Dto.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        itcred2Dto.IndDel = Constantes.IndDel;
                                        itcred2Dto.Usucreacion = User.Identity.Name;
                                        campaniasAppService.SaveItcred2(itcred2Dto);
                                    }
                                }

                                List<ItcRed3Dto> ItcRed3Dtos = campaniasAppService.GetItcred3ById(idProy);
                                if (ItcRed3Dtos != null)
                                {
                                    foreach (var detReg in ItcRed3Dtos)
                                    {
                                        ItcRed3Dto itcred3Dto = detReg;
                                        itcred3Dto.ItcRed3Codi = campaniasAppService.GetLastItcred3Id();
                                        itcred3Dto.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        itcred3Dto.IndDel = Constantes.IndDel;
                                        itcred3Dto.Usucreacion = User.Identity.Name;
                                        campaniasAppService.SaveItcred3(itcred3Dto);
                                    }
                                }

                                List<ItcRed4Dto> itcRed4Dtos = campaniasAppService.GetItcred4ById(idProy);
                                if (itcRed4Dtos != null)
                                {
                                    foreach (var detReg in itcRed4Dtos)
                                    {
                                        ItcRed4Dto itcred4Dto = detReg;
                                        itcred4Dto.ItcRed4Codi = campaniasAppService.GetLastItcred4Id();
                                        itcred4Dto.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        itcred4Dto.IndDel = Constantes.IndDel;
                                        itcred4Dto.UsuCreacion = User.Identity.Name;
                                        campaniasAppService.SaveItcred4(itcred4Dto);
                                    }
                                }

                                List<ItcRed5Dto> itcRed5Dtos = campaniasAppService.GetItcred5ById(idProy);
                                if (itcRed5Dtos != null)
                                {
                                    foreach (var detReg in itcRed5Dtos)
                                    {
                                        ItcRed5Dto itcred5Dto = detReg;
                                        itcred5Dto.ItcRed5Codi = campaniasAppService.GetLastItcred5Id();
                                        itcred5Dto.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        itcred5Dto.IndDel = Constantes.IndDel;
                                        itcred5Dto.Usucreacion = User.Identity.Name;
                                        campaniasAppService.SaveItcred5(itcred5Dto);
                                    }
                                }
                            }
                            else if (transmisionProyectoDTO.Tipocodi == TipoProyecto.Demanda)
                            {
                                // TIPO DEMANDA
                                FormatoD1ADTO formatoD1ADTO = campaniasAppService.GetFormatoD1AById(idProy);
                                if (formatoD1ADTO.ProyCodi != 0)
                                {
                                    formatoD1ADTO.ListaFormatoDet1A = campaniasAppService.GetFormatoD1ADET1ByProyCodi(formatoD1ADTO.FormatoD1ACodi);
                                    formatoD1ADTO.ListaFormatoDet2A = campaniasAppService.GetFormatoD1ADET2ByProyCodi(formatoD1ADTO.FormatoD1ACodi);
                                    formatoD1ADTO.ListaFormatoDet3A = campaniasAppService.GetFormatoD1ADET3ByProyCodi(formatoD1ADTO.FormatoD1ACodi);
                                    formatoD1ADTO.ListaFormatoDet4A = campaniasAppService.GetFormatoD1ADET4ByProyCodi(formatoD1ADTO.FormatoD1ACodi);
                                    formatoD1ADTO.ListaFormatoDet5A = campaniasAppService.GetFormatoD1ADET5ByProyCodi(formatoD1ADTO.FormatoD1ACodi);
                                    formatoD1ADTO.FormatoD1ACodi = campaniasAppService.GetLastFormatoD1AId();
                                    formatoD1ADTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                    formatoD1ADTO.IndDel = Constantes.IndDel;
                                    formatoD1ADTO.UsuCreacion = User.Identity.Name;
                                    campaniasAppService.SaveFormatoD1A(formatoD1ADTO);

                                    if (formatoD1ADTO.ListaFormatoDet1A != null)
                                    {
                                        foreach (var detReg in formatoD1ADTO.ListaFormatoDet1A)
                                        {
                                            FormatoD1ADet1DTO formatoD1ADetDTO = detReg;
                                            formatoD1ADetDTO.FormatoD1ADet1Codi = campaniasAppService.GetLastFormatoD1ADET1Id();
                                            formatoD1ADetDTO.FormatoD1ACodi = formatoD1ADTO.FormatoD1ACodi;
                                            formatoD1ADetDTO.IndDel = Constantes.IndDel;
                                            formatoD1ADetDTO.UsuCreacion = User.Identity.Name;
                                            campaniasAppService.SaveFormatoD1ADET1(formatoD1ADetDTO);
                                        }
                                    }
                                    if (formatoD1ADTO.ListaFormatoDet2A != null)
                                    {
                                        foreach (var detReg in formatoD1ADTO.ListaFormatoDet2A)
                                        {
                                            FormatoD1ADet2DTO formatoD1ADetDTO = detReg;
                                            formatoD1ADetDTO.FormatoD1ADet2Codi = campaniasAppService.GetLastFormatoD1ADET2Id();
                                            formatoD1ADetDTO.FormatoD1ACodi = formatoD1ADTO.FormatoD1ACodi;
                                            formatoD1ADetDTO.IndDel = Constantes.IndDel;
                                            formatoD1ADetDTO.UsuCreacion = User.Identity.Name;
                                            campaniasAppService.SaveFormatoD1ADET2(formatoD1ADetDTO);
                                        }
                                    }
                                    if (formatoD1ADTO.ListaFormatoDet3A != null)
                                    {
                                        foreach (var detReg in formatoD1ADTO.ListaFormatoDet3A)
                                        {
                                            FormatoD1ADet3DTO formatoD1ADetDTO = detReg;
                                            formatoD1ADetDTO.FormatoD1ADet3Codi = campaniasAppService.GetLastFormatoD1ADET3Id();
                                            formatoD1ADetDTO.FormatoD1ACodi = formatoD1ADTO.FormatoD1ACodi;
                                            formatoD1ADetDTO.IndDel = Constantes.IndDel;
                                            formatoD1ADetDTO.UsuCreacion = User.Identity.Name;
                                            campaniasAppService.SaveFormatoD1ADET3(formatoD1ADetDTO);
                                        }
                                    }
                                    if (formatoD1ADTO.ListaFormatoDet4A != null)
                                    {
                                        foreach (var detReg in formatoD1ADTO.ListaFormatoDet4A)
                                        {
                                            FormatoD1ADet4DTO formatoD1ADetDTO = detReg;
                                            formatoD1ADetDTO.FormatoD1ADet4Codi = campaniasAppService.GetLastFormatoD1ADET4Id();
                                            formatoD1ADetDTO.FormatoD1ACodi = formatoD1ADTO.FormatoD1ACodi;
                                            formatoD1ADetDTO.IndDel = Constantes.IndDel;
                                            formatoD1ADetDTO.UsuCreacion = User.Identity.Name;
                                            campaniasAppService.SaveFormatoD1ADET4(formatoD1ADetDTO);
                                        }
                                    }
                                    if (formatoD1ADTO.ListaFormatoDet5A != null)
                                    {
                                        foreach (var detReg in formatoD1ADTO.ListaFormatoDet5A)
                                        {
                                            FormatoD1ADet5DTO formatoD1ADetDTO = detReg;
                                            formatoD1ADetDTO.FormatoD1ADet5Codi = campaniasAppService.GetLastFormatoD1ADET5Id();
                                            formatoD1ADetDTO.FormatoD1ACodi = formatoD1ADTO.FormatoD1ACodi;
                                            formatoD1ADetDTO.IndDel = Constantes.IndDel;
                                            formatoD1ADetDTO.UsuCreacion = User.Identity.Name;
                                            campaniasAppService.SaveFormatoD1ADET5(formatoD1ADetDTO);
                                        }
                                    }
                                }

                                FormatoD1BDTO formatoD1BDTO = campaniasAppService.GetFormatoD1BById(idProy);
                                if (formatoD1BDTO.ProyCodi != 0)
                                {
                                    formatoD1BDTO.ListaFormatoDet1B = campaniasAppService.GetFormatoD1BDetByCodi(formatoD1BDTO.FormatoD1BCodi);
                                    formatoD1BDTO.FormatoD1BCodi = campaniasAppService.GetLastFormatoD1BId();
                                    formatoD1BDTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                    formatoD1BDTO.IndDel = Constantes.IndDel;
                                    formatoD1BDTO.UsuCreacion = User.Identity.Name;
                                    campaniasAppService.SaveFormatoD1B(formatoD1BDTO);

                                    if (formatoD1BDTO.ListaFormatoDet1B != null)
                                    {
                                        foreach (var detReg in formatoD1BDTO.ListaFormatoDet1B)
                                        {
                                            FormatoD1BDetDTO formatoD1BDetDTO = detReg;
                                            formatoD1BDetDTO.FormatoD1BDetCodi = campaniasAppService.GetLastFormatoD1BDetId();
                                            formatoD1BDetDTO.FormatoD1BCodi = formatoD1BDTO.FormatoD1BCodi;
                                            formatoD1BDetDTO.IndDel = Constantes.IndDel;
                                            formatoD1BDetDTO.UsuCreacion = User.Identity.Name;
                                            campaniasAppService.SaveFormatoD1BDet(formatoD1BDetDTO);
                                        }
                                    }
                                }

                                FormatoD1CDTO formatoD1CDTO = campaniasAppService.GetFormatoD1CById(idProy);
                                if (formatoD1CDTO.ProyCodi != 0)
                                {
                                    formatoD1CDTO.ListaFormatoDe1CDet = campaniasAppService.GetFormatoD1CDetCCodi(formatoD1CDTO.FormatoD1CCodi);
                                    formatoD1CDTO.FormatoD1CCodi = campaniasAppService.GetLastFormatoD1CId();
                                    formatoD1CDTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                    formatoD1CDTO.IndDel = Constantes.IndDel;
                                    formatoD1CDTO.UsuCreacion = User.Identity.Name;
                                    campaniasAppService.SaveFormatoD1C(formatoD1CDTO);
                                    if (formatoD1CDTO.ListaFormatoDe1CDet != null)
                                    {
                                        foreach (var detReg in formatoD1CDTO.ListaFormatoDe1CDet)
                                        {
                                            FormatoD1CDetDTO formatoD1CDetDTO = detReg;
                                            formatoD1CDetDTO.FormatoD1CDetCodi = campaniasAppService.GetLastFormatoD1CDetId();
                                            formatoD1CDetDTO.FormatoD1CCodi = formatoD1CDTO.FormatoD1CCodi;
                                            formatoD1CDetDTO.IndDel = Constantes.IndDel;
                                            formatoD1CDetDTO.UsuCreacion = User.Identity.Name;
                                            campaniasAppService.SaveFormatoD1CDet(formatoD1CDetDTO);
                                        }
                                    }
                                }

                                List<FormatoD1DDTO> formatoD1List = campaniasAppService.GetFormatoD1DById(idProy);
                                if (formatoD1List != null)
                                {
                                    foreach (var detReg in formatoD1List)
                                    {

                                        FormatoD1DDTO formatoD1 = detReg;
                                        formatoD1.FormatoD1DCodi = campaniasAppService.GetLastFormatoD1DId();
                                        formatoD1.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        formatoD1.IndDel = Constantes.IndDel;
                                        formatoD1.UsuCreacion = User.Identity.Name;
                                        campaniasAppService.SaveFormatoD1D(formatoD1);
                                    }
                                }
                            }
                            else if (transmisionProyectoDTO.Tipocodi == TipoProyecto.GeneracionDistribuida)
                            {
                                // TIPO GENERACIÓN DISTRIBUIDA
                                CCGDADTO ccgdaDTO = campaniasAppService.GetCcgdaById(idProy);
                                if (ccgdaDTO.ProyCodi != 0)
                                {
                                    ccgdaDTO.CcgdaCodi = campaniasAppService.GetLastCcgdaId();
                                    ccgdaDTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                    ccgdaDTO.IndDel = Constantes.IndDel;
                                    ccgdaDTO.UsuCreacion = User.Identity.Name;
                                    campaniasAppService.SaveCcgda(ccgdaDTO);
                                }

                                List<CCGDBDTO> Ccgdbdtos = campaniasAppService.GetCcgdbById(idProy);
                                if (Ccgdbdtos != null)
                                {
                                    foreach (var detReg in Ccgdbdtos)
                                    {
                                        CCGDBDTO Ccgdbdto = detReg;
                                        Ccgdbdto.CcgdbCodi = campaniasAppService.GetLastCcgdbId();
                                        Ccgdbdto.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        Ccgdbdto.IndDel = Constantes.IndDel;
                                        Ccgdbdto.UsuCreacion = User.Identity.Name;
                                        campaniasAppService.SaveCcgdb(Ccgdbdto);
                                    }
                                }

                                List<CCGDCOptDTO> Ccgdcopt = campaniasAppService.GetCcgdcOptById(idProy);
                                List<CCGDCPesDTO> Ccgdcpes = campaniasAppService.GetCcgdcPesById(idProy);
                                if (Ccgdcopt != null)
                                {
                                    foreach (var detReg in Ccgdcopt)
                                    {

                                        CCGDCOptDTO CCGDCOpt = detReg;
                                        CCGDCOpt.CcgdcOptCodi = campaniasAppService.GetLastCcgdcOptId();
                                        CCGDCOpt.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        CCGDCOpt.IndDel = Constantes.IndDel;
                                        CCGDCOpt.UsuCreacion = User.Identity.Name;
                                        campaniasAppService.SaveCcgdcOpt(CCGDCOpt);
                                    }
                                }
                                if (Ccgdcpes != null)
                                {
                                    foreach (var detReg in Ccgdcpes)
                                    {

                                        CCGDCPesDTO CCGDCPes = detReg;
                                        CCGDCPes.CcgdcPesCodi = campaniasAppService.GetLastCcgdcPesId();
                                        CCGDCPes.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        CCGDCPes.IndDel = Constantes.IndDel;
                                        CCGDCPes.UsuCreacion = User.Identity.Name;
                                        campaniasAppService.SaveCcgdcPes(CCGDCPes);
                                    }
                                }

                                List<CCGDDDTO> Ccgdddtos = campaniasAppService.GetCcgddById(idProy);
                                if (Ccgdddtos != null)
                                {
                                    foreach (var detReg in Ccgdddtos)
                                    {

                                        CCGDDDTO Ccgvdd = detReg;
                                        Ccgvdd.CcgddCodi = campaniasAppService.GetLastCcgddId();
                                        Ccgvdd.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        Ccgvdd.IndDel = Constantes.IndDel;
                                        Ccgvdd.UsuCreacion = User.Identity.Name;
                                        campaniasAppService.SaveCcgdd(Ccgvdd);
                                    }
                                }

                                CCGDEDTO ccgdeDTO = campaniasAppService.GetCcgdeById(idProy);
                                if (ccgdeDTO.ProyCodi != 0)
                                {
                                    ccgdeDTO.CcgdeCodi = campaniasAppService.GetLastCcgdeId();
                                    ccgdeDTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                    ccgdeDTO.IndDel = Constantes.IndDel;
                                    ccgdeDTO.UsuCreacion = User.Identity.Name;
                                    campaniasAppService.SaveCcgde(ccgdeDTO);
                                }
                                   
                                List<CCGDFDTO> ccgdfDTOs = campaniasAppService.GetCcgdfById(idProy);
                                if (ccgdfDTOs != null)
                                {
                                    foreach (var detReg in ccgdfDTOs)
                                    {

                                        CCGDFDTO ccgdfDTO = detReg;
                                        ccgdfDTO.CcgdfCodi = campaniasAppService.GetLastCcgdfId();
                                        ccgdfDTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        ccgdfDTO.IndDel = Constantes.IndDel;
                                        ccgdfDTO.UsuCreacion = User.Identity.Name;
                                        campaniasAppService.SaveCcgdf(ccgdfDTO);
                                    }
                                }
                            }
                            else if (transmisionProyectoDTO.Tipocodi == TipoProyecto.HidrogenoVerde)
                            {
                                // TIPO HIDRÓGENO VERDE
                                CuestionarioH2VADTO cuestionarioH2VADTO = campaniasAppService.GetCuestionarioH2VAById(idProy);
                                if (cuestionarioH2VADTO.ProyCodi != 0)
                                {
                                    List<CuestionarioH2VADet1DTO> Lista1DTO = campaniasAppService.GetCuestionarioH2VADet1ById(cuestionarioH2VADTO.H2vaCodi);
                                    List<CuestionarioH2VADet2DTO> Lista2DTO = campaniasAppService.GetCuestionarioH2VADet2ById(cuestionarioH2VADTO.H2vaCodi);
                                    cuestionarioH2VADTO.ListCH2VADet1DTOs = Lista1DTO;
                                    cuestionarioH2VADTO.ListCH2VADet2DTOs = Lista2DTO;
                                    cuestionarioH2VADTO.H2vaCodi = campaniasAppService.GetLastCuestionarioH2VAId();
                                    cuestionarioH2VADTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                    cuestionarioH2VADTO.IndDel = Constantes.IndDel;
                                    cuestionarioH2VADTO.UsuCreacion = User.Identity.Name;
                                    if (campaniasAppService.SaveCuestionarioH2VA(cuestionarioH2VADTO))
                                    {
                                        if (cuestionarioH2VADTO.ListCH2VADet1DTOs != null)
                                        {
                                            foreach (var item in cuestionarioH2VADTO.ListCH2VADet1DTOs)
                                            {
                                                CuestionarioH2VADet1DTO cuestionarioH2VADet1DTO = item;
                                                cuestionarioH2VADet1DTO.H2vaDet1Codi = campaniasAppService.GetLastCuestionarioH2VADet1Id();
                                                cuestionarioH2VADet1DTO.H2vaCodi = cuestionarioH2VADTO.H2vaCodi;
                                                cuestionarioH2VADet1DTO.IndDel = Constantes.IndDel;
                                                cuestionarioH2VADet1DTO.UsuCreacion = User.Identity.Name;
                                                campaniasAppService.SaveCuestionarioH2VADet1(cuestionarioH2VADet1DTO);
                                            }
                                        }
                                        if (cuestionarioH2VADTO.ListCH2VADet2DTOs != null)
                                        {
                                            foreach (var item in cuestionarioH2VADTO.ListCH2VADet2DTOs)
                                            {
                                                CuestionarioH2VADet2DTO cuestionarioH2VADet2DTO = item;
                                                cuestionarioH2VADet2DTO.H2vaDet2Codi = campaniasAppService.GetLastCuestionarioH2VADet2Id();
                                                cuestionarioH2VADet2DTO.H2vaCodi = cuestionarioH2VADTO.H2vaCodi;
                                                cuestionarioH2VADet2DTO.IndDel = Constantes.IndDel;
                                                cuestionarioH2VADet2DTO.UsuCreacion = User.Identity.Name;
                                                campaniasAppService.SaveCuestionarioH2VADet2(cuestionarioH2VADet2DTO);
                                            }
                                        }
                                    }
                                }

                                CuestionarioH2VBDTO cuestionarioH2VBDTO = campaniasAppService.GetCuestionarioH2VBById(idProy);
                                if (cuestionarioH2VBDTO.ProyCodi != 0)
                                {
                                    cuestionarioH2VBDTO.H2vbCodi = campaniasAppService.GetLastCuestionarioH2VBId();
                                    cuestionarioH2VBDTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                    cuestionarioH2VBDTO.IndDel = Constantes.IndDel;
                                    cuestionarioH2VBDTO.UsuCreacion = User.Identity.Name;
                                    campaniasAppService.SaveCuestionarioH2VB(cuestionarioH2VBDTO);
                                } 

                                List<CuestionarioH2VCDTO> CH2VCdto = campaniasAppService.GetCuestionarioH2VCById(idProy);
                                if (CH2VCdto != null)
                                {
                                    foreach (var detReg in CH2VCdto)
                                    {

                                        CuestionarioH2VCDTO ch2vcdto = detReg;
                                        ch2vcdto.H2vcCodi = campaniasAppService.GetLastCuestionarioH2VCId();
                                        ch2vcdto.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        ch2vcdto.IndDel = Constantes.IndDel;
                                        ch2vcdto.UsuCreacion = User.Identity.Name;
                                        campaniasAppService.SaveCuestionarioH2VC(ch2vcdto);
                                    }
                                }

                                List<CuestionarioH2VEDTO> CH2VEdto = campaniasAppService.GetCuestionarioH2VEById(idProy);
                                if (CH2VEdto != null)
                                {
                                    foreach (var detReg in CH2VEdto)
                                    {
                                        CuestionarioH2VEDTO ch2vEdto = detReg;
                                        ch2vEdto.H2veCodi = campaniasAppService.GetLastCuestionarioH2VEId();
                                        ch2vEdto.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        ch2vEdto.IndDel = Constantes.IndDel;
                                        ch2vEdto.UsuCreacion = User.Identity.Name;
                                        campaniasAppService.SaveCuestionarioH2VE(ch2vEdto);
                                    }
                                }

                                CuestionarioH2VFDTO CH2VFdto = campaniasAppService.GetCuestionarioH2VFById(idProy);
                                if (CH2VFdto.ProyCodi != 0)
                                {
                                    CH2VFdto.H2vFCodi = campaniasAppService.GetLastCuestionarioH2VFId();
                                    CH2VFdto.ProyCodi = transmisionProyectoDTO.Proycodi;
                                    CH2VFdto.IndDel = Constantes.IndDel;
                                    CH2VFdto.UsuCreacion = User.Identity.Name;
                                    campaniasAppService.SaveCuestionarioH2VF(CH2VFdto);
                                }

                                List<CuestionarioH2VGDTO> CH2VGdto = campaniasAppService.GetCuestionarioH2VGById(idProy);
                                if (CH2VGdto != null)
                                {
                                    foreach (var detReg in CH2VGdto)
                                    {
                                        CuestionarioH2VGDTO ch2vgdto = detReg;
                                        ch2vgdto.H2vGCodi = campaniasAppService.GetLastCuestionarioH2VGId();
                                        ch2vgdto.ProyCodi = transmisionProyectoDTO.Proycodi;
                                        ch2vgdto.IndDel = Constantes.IndDel;
                                        ch2vgdto.UsuCreacion = User.Identity.Name;
                                        campaniasAppService.SaveCuestionarioH2VG(ch2vgdto);
                                    }
                                }
                            }

                            // ARCHIVOS DE PROYECTOS
                            List<ArchivoInfoDTO> listaArchivoDTO = campaniasAppService.GetArchivoInfoByProyCodi(idProy);
                            if (listaArchivoDTO != null )
                            {
                                if(listaArchivoDTO.Count > 0){
                                    //actualizar en db
                                    string plancodigo = transmisionProyectoDTO.Plancodi.ToString();
                                    string planproy = transmisionProyectoDTO.Proycodi.ToString();

                                    string origen = listaArchivoDTO[0].ArchUbicacion;
                                    // Separar la ruta en segmentos
                                    string[] partesRuta = origen.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                                    if (partesRuta.Length >= 2)
                                    {

                                    // Reemplazar la última parte 
                                    partesRuta[partesRuta.Length - 2] = planproy;
                                    partesRuta[partesRuta.Length - 1] = "0";

                                    // Unir nuevamente la ruta con el separador adecuado
                                    string nuevaUbicacion = Path.Combine(partesRuta);

                                        if (partesRuta.Length > 0)
                                        {
                                            foreach (var archivoInfoDTO in listaArchivoDTO)
                                            {
                                                archivoInfoDTO.ArchFechaSubida = DateTime.Now;
                                                archivoInfoDTO.ProyCodi = transmisionProyectoDTO.Proycodi;
                                                archivoInfoDTO.ArchCodi = campaniasAppService.GetLastArchivoInfoId();
                                                archivoInfoDTO.IndDel = Constantes.IndDel;
                                                archivoInfoDTO.UsuarioCreacion = User.Identity.Name;
                                                archivoInfoDTO.ArchUbicacion = nuevaUbicacion;
                                                //archivoInfoDTO.Descripcion=
                                                campaniasAppService.SaveArchivoInfo(archivoInfoDTO);
                                            }
        

                                            Console.WriteLine($"Ubicación original: {origen}");
                                            Console.WriteLine($"Nueva ubicación: {nuevaUbicacion}");
                                            CopiarArchivos(origen, nuevaUbicacion);

                                        }
                                    }
                                }
                            }
                        }
                    }
                    return Json(new { success = true, responseResult = 1, dataPlant = planTransmisionDTO, listProyecto = listaProyecto }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, responseResult = 0, plancodi = planTransmisionDTO.Plancodi }, JsonRequestBehavior.AllowGet);
                }
            } else {
                return Json(new { success = true, responseResult = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult ListarProyectos()
        {
            List<TipoProyectoDTO> ListaProyectos = campaniasAppService.ListTipoProyecto();
            return Json(ListaProyectos);
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarPlanTransmision(PlanTransmisionDTO planTransmisionDTO)
        {
            planTransmisionDTO.Plancodi = campaniasAppService.GetLastPlanTransmisionId();
            planTransmisionDTO.Numreg = 0;
            planTransmisionDTO.Planversion = Constantes.VersionInicial;
            planTransmisionDTO.Planestado = Constantes.EstadoRegistrado;
            planTransmisionDTO.IndDel = Constantes.IndDel;
            planTransmisionDTO.Usucreacion = User.Identity.Name;

            if (campaniasAppService.SavePlanTransmision(planTransmisionDTO))
            {
                return Json(new { success = true, responseResult = 1, plancodi = planTransmisionDTO.Plancodi }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, responseResult = 0, plancodi = planTransmisionDTO.Plancodi }, JsonRequestBehavior.AllowGet);
            }
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarCabecera(ProyectoModel periodoModel)
        {
            int result = 0;
            int CodPlanTransmision = 0;
            TransmisionProyectoDTO transmisionProyectoDTO = periodoModel.TransmisionProyectoDTO;
            transmisionProyectoDTO.Proycodi = campaniasAppService.GetLastTransmisionProyectoId();
            transmisionProyectoDTO.IndDel = Constantes.IndDel;
            transmisionProyectoDTO.Usucreacion = User.Identity.Name;

            if (periodoModel.CodPlanTransmision.Equals(0))
            {
                PlanTransmisionDTO planTransmisionDTO = new PlanTransmisionDTO();
                planTransmisionDTO.Plancodi = campaniasAppService.GetLastPlanTransmisionId();
                planTransmisionDTO.Pericodi = transmisionProyectoDTO.Pericodi;
                planTransmisionDTO.Codempresa = transmisionProyectoDTO.EmpresaCodi;
                planTransmisionDTO.Nomempresa = transmisionProyectoDTO.EmpresaNom;
                planTransmisionDTO.Numreg = 0;
                planTransmisionDTO.Planversion = Constantes.VersionInicial;
                planTransmisionDTO.Planestado = Constantes.EstadoRegistrado;
                planTransmisionDTO.IndDel = Constantes.IndDel;
                planTransmisionDTO.Usucreacion = User.Identity.Name;
                campaniasAppService.SavePlanTransmision(planTransmisionDTO);
                CodPlanTransmision = planTransmisionDTO.Plancodi;
                transmisionProyectoDTO.Plancodi = CodPlanTransmision;
                transmisionProyectoDTO.Proyestado = Constantes.EstadoRegistrado;
            }
            else
            {
                transmisionProyectoDTO.Plancodi = periodoModel.CodPlanTransmision;
                transmisionProyectoDTO.Proyestado = Constantes.EstadoRegistrado;
            }
            if (campaniasAppService.SaveTransmisionProyecto(transmisionProyectoDTO))
            {
                campaniasAppService.UpdateProyRegById(transmisionProyectoDTO.Plancodi);
                List<int> ListaHojas = campaniasAppService.GetDetalleHojaByPericodi(transmisionProyectoDTO.Pericodi, Constantes.IndDel);
                return Json(new { success = true, result = 1, proycodi = transmisionProyectoDTO.Proycodi, hojas = ListaHojas, codPlanTransmision = transmisionProyectoDTO.Plancodi }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, result = 0, proycodi = 0, codPlanTransmision = 0 }, JsonRequestBehavior.AllowGet);
            }
        }



        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarFichaA(ProyectoModel periodoModel)
        {
            int result = 0;
            RegHojaADTO regHojaADTO = periodoModel.RegHojaADTO;
            regHojaADTO.Fichaacodi = campaniasAppService.GetLastRegHojaAId();
            regHojaADTO.Proycodi = periodoModel.ProyCodi;
            regHojaADTO.IndDel = Constantes.IndDel;
            regHojaADTO.Usucreacion = User.Identity.Name;
            Boolean deleteF = campaniasAppService.DeleteRegHojaAById(regHojaADTO.Proycodi, User.Identity.Name);
            if (deleteF)
            {
                campaniasAppService.SaveRegHojaA(regHojaADTO);
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarCCGDA(ProyectoModel periodoModel)
        {
            int result = 0;
            CCGDADTO ccgdaDTO = periodoModel.CcgdaDTO;
            ccgdaDTO.CcgdaCodi = campaniasAppService.GetLastCcgdaId();
            ccgdaDTO.ProyCodi = periodoModel.ProyCodi;
            ccgdaDTO.IndDel = Constantes.IndDel;
            ccgdaDTO.UsuCreacion = User.Identity.Name;
            Boolean deleteF = campaniasAppService.DeleteCcgdaById(ccgdaDTO.ProyCodi, User.Identity.Name);
            if (deleteF)
            {
                campaniasAppService.SaveCcgda(ccgdaDTO);
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarCCGDB(ProyectoModel periodoModel)
        {
            int result = 0;
            List<CCGDBDTO> Ccgdbdtos = periodoModel.Ccgdbdtos;
            Boolean deleteF = campaniasAppService.DeleteCcgdbById(periodoModel.ProyCodi, User.Identity.Name);
            if (deleteF)
            {
                if (Ccgdbdtos != null) {
                    foreach (var detReg in Ccgdbdtos)
                    {

                        CCGDBDTO Ccgdbdto = detReg;
                        Ccgdbdto.CcgdbCodi = campaniasAppService.GetLastCcgdbId();
                        Ccgdbdto.ProyCodi = periodoModel.ProyCodi;
                        Ccgdbdto.IndDel = Constantes.IndDel;
                        Ccgdbdto.UsuCreacion = User.Identity.Name;
                        campaniasAppService.SaveCcgdb(Ccgdbdto);
                    }
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarCCGDD(ProyectoModel periodoModel)
        {
            int result = 0;
            List<CCGDDDTO> Ccgdddtos = periodoModel.Ccgdddtos;
            Boolean deleteF = campaniasAppService.DeleteCcgddById(periodoModel.ProyCodi, User.Identity.Name);

            if (Ccgdddtos == null)
            {
                result = 1;
                return Json(result);
            }

            if (deleteF)
            {
                foreach (var detReg in Ccgdddtos)
                {

                    CCGDDDTO Ccgvdd = detReg;
                    Ccgvdd.CcgddCodi = campaniasAppService.GetLastCcgddId();
                    Ccgvdd.ProyCodi = periodoModel.ProyCodi;
                    Ccgvdd.IndDel = Constantes.IndDel;
                    Ccgvdd.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveCcgdd(Ccgvdd);
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarCCGDC(ProyectoModel periodoModel)
        {
            int result = 0;
            List<CCGDCOptDTO> Ccgdcopt = periodoModel.CCGDCOptDTOs;
            List<CCGDCPesDTO> Ccgdcpes = periodoModel.CCGDCPesDTOs;
            Boolean deleteF = campaniasAppService.DeleteCcgdcOptById(periodoModel.ProyCodi, User.Identity.Name);
            Boolean deleteP = campaniasAppService.DeleteCcgdcPesById(periodoModel.ProyCodi, User.Identity.Name);

            if (deleteF && deleteP)
            {
                if (Ccgdcopt != null) {
                    foreach (var detReg in Ccgdcopt)
                    {

                        CCGDCOptDTO CCGDCOpt = detReg;
                        CCGDCOpt.CcgdcOptCodi = campaniasAppService.GetLastCcgdcOptId();
                        CCGDCOpt.ProyCodi = periodoModel.ProyCodi;
                        CCGDCOpt.IndDel = Constantes.IndDel;
                        CCGDCOpt.UsuCreacion = User.Identity.Name;
                        campaniasAppService.SaveCcgdcOpt(CCGDCOpt);
                    }
                }
                if (Ccgdcpes != null) {
                    foreach (var detReg in Ccgdcpes)
                    {

                        CCGDCPesDTO CCGDCPes = detReg;
                        CCGDCPes.CcgdcPesCodi = campaniasAppService.GetLastCcgdcPesId();
                        CCGDCPes.ProyCodi = periodoModel.ProyCodi;
                        CCGDCPes.IndDel = Constantes.IndDel;
                        CCGDCPes.UsuCreacion = User.Identity.Name;
                        campaniasAppService.SaveCcgdcPes(CCGDCPes);
                    }
                }

                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);

            }
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarCCGDE(ProyectoModel periodoModel)
        {
            int result = 0;
            CCGDEDTO ccgdeDTO = periodoModel.CcgdeDTO;
            ccgdeDTO.CcgdeCodi = campaniasAppService.GetLastCcgdeId();
            ccgdeDTO.ProyCodi = periodoModel.ProyCodi;
            ccgdeDTO.IndDel = Constantes.IndDel;
            ccgdeDTO.UsuCreacion = User.Identity.Name;
            Boolean deleteF = campaniasAppService.DeleteCcgdeById(ccgdeDTO.ProyCodi, User.Identity.Name);
            if (deleteF)
            {
                campaniasAppService.SaveCcgde(ccgdeDTO);
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarCCGDF(ProyectoModel periodoModel)
        {
            int result = 0;
            List<CCGDFDTO> ccgdfDTOs = periodoModel.CcgdfDTOs;
            Boolean deleteF = campaniasAppService.DeleteCcgdfById(periodoModel.ProyCodi, User.Identity.Name);
            if (deleteF)
            {
                if (ccgdfDTOs != null) {
                    foreach (var detReg in ccgdfDTOs)
                    {

                        CCGDFDTO ccgdfDTO = detReg;
                        ccgdfDTO.CcgdfCodi = campaniasAppService.GetLastCcgdfId();
                        ccgdfDTO.ProyCodi = periodoModel.ProyCodi;
                        ccgdfDTO.IndDel = Constantes.IndDel;
                        ccgdfDTO.UsuCreacion = User.Identity.Name;
                        campaniasAppService.SaveCcgdf(ccgdfDTO);
                    }
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarFichaB(ProyectoModel periodoModel)
        {
            int result = 0;
            //llamar hoja b
            RegHojaBDTO regHojaBDTO = periodoModel.RegHojaBDTO;
            regHojaBDTO.Fichabcodi = campaniasAppService.GetLastRegHojaBId();
            regHojaBDTO.Proycodi = periodoModel.ProyCodi;
            regHojaBDTO.IndDel = Constantes.IndDel;
            regHojaBDTO.Usucreacion = User.Identity.Name;
            Boolean deleteF = campaniasAppService.DeleteRegHojaBById(regHojaBDTO.Proycodi, User.Identity.Name);
            if (deleteF)
            {
                campaniasAppService.SaveRegHojaB(regHojaBDTO);
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarFichaC(ProyectoModel periodoModel)
        {
            int result = 0;
            RegHojaCDTO regHojaCDTO = periodoModel.RegHojaCDTO;
            regHojaCDTO.Fichaccodi = campaniasAppService.GetLastRegHojaCId();
            regHojaCDTO.Proycodi = periodoModel.ProyCodi;
            regHojaCDTO.IndDel = Constantes.IndDel;
            regHojaCDTO.Usucreacion = User.Identity.Name;
            Boolean deleteF = campaniasAppService.DeleteRegHojaCById(regHojaCDTO.Proycodi, User.Identity.Name);
            Boolean deleteFList = campaniasAppService.DeleteDetRegHojaCById(regHojaCDTO.Proycodi, User.Identity.Name);
            if (deleteF && deleteFList)
            {
                campaniasAppService.SaveRegHojaC(regHojaCDTO);
                foreach (var detReg in periodoModel.DetRegHojaCDTO)
                {

                    DetRegHojaCDTO regDetHojaCDTO = detReg;
                    regDetHojaCDTO.Detareghccodi = campaniasAppService.GetLastDetRegHojaCId();
                    regDetHojaCDTO.Fichaccodi = regHojaCDTO.Fichaccodi;
                    regDetHojaCDTO.IndDel = Constantes.IndDel;
                    regDetHojaCDTO.Usucreacion = User.Identity.Name;
                    campaniasAppService.SaveDetRegHojaC(regDetHojaCDTO);
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }
        
        
        [System.Web.Mvc.HttpPost]
        public JsonResult EliminarCuenca(int id)
        {
            int result = 0;
            string usuario = User.Identity.Name;
            if (campaniasAppService.DeleteRegHojaDById2(id, usuario))
            {
                result = 1;
            }
            else
            {
                result = 0;
            }
            return Json(result);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarFichaD(ProyectoModel periodoModel)
        {
            int result = 0;

            campaniasAppService.DeleteRegHojaDById(periodoModel.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteDetRegHojaDById(periodoModel.ProyCodi, User.Identity.Name);

            foreach (var detReg in periodoModel.ListRegHojaD)
            {
                RegHojaDDTO regHojaDDTO = detReg;
                regHojaDDTO.Hojadcodi = Guid.NewGuid().ToString();
                regHojaDDTO.Proycodi = periodoModel.ProyCodi;
                regHojaDDTO.IndDel = Constantes.IndDel;
                regHojaDDTO.Usucreacion = User.Identity.Name;
                campaniasAppService.SaveRegHojaD(regHojaDDTO);

                List<DetRegHojaDDTO> listaDetalle = detReg.ListDetRegHojaD;
                if (listaDetalle != null)
                { 
                    foreach (var item in listaDetalle)
                    {
                    DetRegHojaDDTO detRegHojaDDTO = item;
                    detRegHojaDDTO.Detreghdcodi = Guid.NewGuid().ToString();
                    detRegHojaDDTO.Usucreacion = User.Identity.Name;
                    detRegHojaDDTO.IndDel = Constantes.IndDel;
                    detRegHojaDDTO.Hojadcodi = regHojaDDTO.Hojadcodi;
                    campaniasAppService.SaveDetRegHojaD(detRegHojaDDTO);
                    }
                }
            }
            result = 1;
            return Json(result);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarFichaCCTTA(ProyectoModel proyectoModel)
        {
            int result = 0;
            RegHojaCCTTADTO regHojaCCTTADTO = proyectoModel.RegHojaCCTTADTO;
            regHojaCCTTADTO.Centralcodi = campaniasAppService.GetLastRegHojaCCTTAId();
            regHojaCCTTADTO.Proycodi = proyectoModel.ProyCodi;
            regHojaCCTTADTO.IndDel = Constantes.IndDel;
            regHojaCCTTADTO.Usucreacion = User.Identity.Name;
            Boolean deleteF = campaniasAppService.DeleteRegHojaCCTTAById(regHojaCCTTADTO.Proycodi, User.Identity.Name);
            if (deleteF)
            {
                campaniasAppService.SaveRegHojaCCTTA(regHojaCCTTADTO);
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }

        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarFichaCCTTB(ProyectoModel proyectoModel)
        {
            int result = 0;
            //llamar hoja b
            RegHojaCCTTBDTO regHojaCCTTBDTO = proyectoModel.RegHojaCCTTBDTO;
            regHojaCCTTBDTO.Centralcodi = campaniasAppService.GetLastRegHojaCCTTBId();
            regHojaCCTTBDTO.Proycodi = proyectoModel.ProyCodi;
            regHojaCCTTBDTO.IndDel = Constantes.IndDel;
            regHojaCCTTBDTO.Usucreacion = User.Identity.Name;
            Boolean deleteF = campaniasAppService.DeleteRegHojaCCTTBById(regHojaCCTTBDTO.Proycodi, User.Identity.Name);
            if (deleteF)
            {
                campaniasAppService.SaveRegHojaCCTTB(regHojaCCTTBDTO);
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarFichaCCTTC(ProyectoModel proyectoModel)
        {
            int result = 0;
            RegHojaCCTTCDTO regHojaCCTTCDTO = proyectoModel.RegHojaCCTTCDTO;
            regHojaCCTTCDTO.Centralcodi = campaniasAppService.GetLastRegHojaCCTTCId();
            regHojaCCTTCDTO.Proycodi = proyectoModel.ProyCodi;
            regHojaCCTTCDTO.IndDel = Constantes.IndDel;
            regHojaCCTTCDTO.Usucreacion = User.Identity.Name;
            Boolean deleteF = campaniasAppService.DeleteRegHojaCCTTCById(regHojaCCTTCDTO.Proycodi, User.Identity.Name);
            if (deleteF)
            {
                campaniasAppService.SaveRegHojaCCTTC(regHojaCCTTCDTO);
                campaniasAppService.DeleteDet1RegHojaCCTTCById(regHojaCCTTCDTO.Proycodi, User.Identity.Name);
                campaniasAppService.DeleteDet2RegHojaCCTTCById(regHojaCCTTCDTO.Proycodi, User.Identity.Name);
                if (proyectoModel.Det1RegHojaCCTTCDTO != null)
                {
                    foreach (var detReg in proyectoModel.Det1RegHojaCCTTCDTO)
                    {
                        Det1RegHojaCCTTCDTO regDetHojaCCTTCDTO = detReg;
                        regDetHojaCCTTCDTO.Det1centermhccodi = campaniasAppService.GetLastDet1RegHojaCCTTCId();
                        regDetHojaCCTTCDTO.Centralcodi = regHojaCCTTCDTO.Centralcodi;
                        regDetHojaCCTTCDTO.IndDel = Constantes.IndDel;
                        regDetHojaCCTTCDTO.Usucreacion = User.Identity.Name;
                        campaniasAppService.SaveDet1RegHojaCCTTC(regDetHojaCCTTCDTO);
                    }
                }

                if (proyectoModel.Det2RegHojaCCTTCDTO != null)
                {
                    foreach (var detReg in proyectoModel.Det2RegHojaCCTTCDTO)
                    {
                        Det2RegHojaCCTTCDTO regDetHojaCCTTCDTO = detReg;
                        regDetHojaCCTTCDTO.Det2centermhccodi = campaniasAppService.GetLastDet2RegHojaCCTTCId();
                        regDetHojaCCTTCDTO.Centralcodi = regHojaCCTTCDTO.Centralcodi;
                        regDetHojaCCTTCDTO.IndDel = Constantes.IndDel;
                        regDetHojaCCTTCDTO.Usucreacion = User.Identity.Name;
                        campaniasAppService.SaveDet2RegHojaCCTTC(regDetHojaCCTTCDTO);
                    }
                }

                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarFichaASubest(ProyectoModel proyectoModel)
        {
            int result = 0;
            RegHojaASubestDTO regHojaASubestDTO = proyectoModel.RegHojaASubestDTO;
            regHojaASubestDTO.Centralcodi = campaniasAppService.GetLastRegHojaASubestId();
            regHojaASubestDTO.Proycodi = proyectoModel.ProyCodi;
            regHojaASubestDTO.IndDel = Constantes.IndDel;
            regHojaASubestDTO.Usucreacion = User.Identity.Name;
            campaniasAppService.SaveRegHojaASubest(regHojaASubestDTO);
            foreach (var detReg in proyectoModel.DetRegHojaASubestDTO)
            {

                DetRegHojaASubestDTO regDetHojaASubestDTO = detReg;
                regDetHojaASubestDTO.Detsubesthacodi = campaniasAppService.GetLastDetRegHojaASubestId();
                regDetHojaASubestDTO.Centralcodi = regHojaASubestDTO.Centralcodi;
                regDetHojaASubestDTO.IndDel = Constantes.IndDel;
                regDetHojaASubestDTO.Usucreacion = User.Identity.Name;
                campaniasAppService.SaveDetRegHojaASubest(regDetHojaASubestDTO);
            }
            result = 1;
            return Json(result);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarCEHojaB(ProyectoModel periodoModel)
        {
            int result = 0;
            //llamar hoja b
            RegHojaEolBDTO regHojaEolBDTO = periodoModel.RegHojaEolBDTO;
            regHojaEolBDTO.CentralBCodi = campaniasAppService.GetLastRegHojaEolBId();
            regHojaEolBDTO.Proycodi = periodoModel.ProyCodi;
            regHojaEolBDTO.IndDel = Constantes.IndDel;
            regHojaEolBDTO.Usucreacion = User.Identity.Name;
            campaniasAppService.SaveRegHojaEolB(regHojaEolBDTO);
            result = 1;
            return Json(result);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarEolHojaA(ProyectoModel periodoModel)
        {
            int result = 0;
            RegHojaEolADTO regHojaEolADTO = periodoModel.RegHojaEolADTO;
            regHojaEolADTO.CentralACodi = campaniasAppService.GetLastRegHojaEolAId();
            regHojaEolADTO.ProyCodi = periodoModel.ProyCodi;
            regHojaEolADTO.IndDel = Constantes.IndDel;
            regHojaEolADTO.UsuCreacion = User.Identity.Name;
            Boolean deleteF = campaniasAppService.DeleteRegHojaEolAById(regHojaEolADTO.ProyCodi, User.Identity.Name);
            if (deleteF)
            {
                if (campaniasAppService.SaveRegHojaEolA(regHojaEolADTO))
                {
                    campaniasAppService.DeleteRegHojaEolADetById(regHojaEolADTO.ProyCodi, User.Identity.Name);
                    if (periodoModel.RegHojaEolADetDTOs != null)
                    {
                        foreach (var item in periodoModel.RegHojaEolADetDTOs)
                    {
                        RegHojaEolADetDTO regHojaEolADetDTO = item;
                        regHojaEolADetDTO.CentralACodi = regHojaEolADTO.CentralACodi;
                        regHojaEolADetDTO.CentralADetCodi = campaniasAppService.GetLastRegHojaEolADetId();
                        regHojaEolADetDTO.IndDel = Constantes.IndDel;
                        regHojaEolADetDTO.UsuCreacion = User.Identity.Name;
                        campaniasAppService.SaveRegHojaEolADet(regHojaEolADetDTO);
                    }
                    }
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarEolHojaB(ProyectoModel periodoModel)
        {
            int result = 0;
            //llamar hoja b
            RegHojaEolBDTO regHojaEolBDTO = periodoModel.RegHojaEolBDTO;
            regHojaEolBDTO.CentralBCodi = campaniasAppService.GetLastRegHojaEolBId();
            regHojaEolBDTO.Proycodi = periodoModel.ProyCodi;
            regHojaEolBDTO.IndDel = Constantes.IndDel;
            regHojaEolBDTO.Usucreacion = User.Identity.Name;
            Boolean deleteF = campaniasAppService.DeleteRegHojaEolBById(regHojaEolBDTO.Proycodi, User.Identity.Name);
            if (deleteF)
            {
                campaniasAppService.SaveRegHojaEolB(regHojaEolBDTO);
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }

        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarEolHojaC(ProyectoModel periodoModel)
        {
            int result = 0;
            RegHojaEolCDTO regHojaEolCDTO = periodoModel.RegHojaEolCDTO;
            regHojaEolCDTO.CentralCCodi = campaniasAppService.GetLastRegHojaEolCId();
            regHojaEolCDTO.Proycodi = periodoModel.ProyCodi;
            regHojaEolCDTO.IndDel = Constantes.IndDel;
            regHojaEolCDTO.Usucreacion = User.Identity.Name;
            Boolean deleteF = campaniasAppService.DeleteRegHojaEolCById(regHojaEolCDTO.Proycodi, User.Identity.Name);
            Boolean deleteD = campaniasAppService.DeleteDetRegHojaEolCById(regHojaEolCDTO.Proycodi, User.Identity.Name);
            if (deleteF && deleteD)
            {
                if (campaniasAppService.SaveRegHojaEolC(regHojaEolCDTO)) {

                    if (periodoModel.DetRegHojaEolCDTO != null) // Validar si DetRegHojaEolCDTO no es null
                    {
                        foreach (var item in periodoModel.DetRegHojaEolCDTO)
                        {
                            DetRegHojaEolCDTO detRegHojaEolCDTO = item;
                            detRegHojaEolCDTO.DetEloCCodi = campaniasAppService.GetLastDetRegHojaEolCId();
                            detRegHojaEolCDTO.Centralccodi = regHojaEolCDTO.CentralCCodi;
                            detRegHojaEolCDTO.IndDel = Constantes.IndDel;
                            detRegHojaEolCDTO.Usucreacion = User.Identity.Name;
                            campaniasAppService.SaveDetRegHojaEolC(detRegHojaEolCDTO);
                        }
                    }
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarSolHojaA(ProyectoModel periodoModel)
        {
            int result = 0;
            SolHojaADTO solHojaADTO = periodoModel.SolHojaADTO;
            solHojaADTO.Solhojaacodi = campaniasAppService.GetLastSolHojaAId();
            solHojaADTO.Proycodi = periodoModel.ProyCodi;
            solHojaADTO.IndDel = Constantes.IndDel;
            solHojaADTO.UsuCreacion = User.Identity.Name;

            Boolean deleteF = campaniasAppService.DeleteSolHojaAById(solHojaADTO.Proycodi, User.Identity.Name);
            if (deleteF)
            {
                campaniasAppService.SaveSolHojaA(solHojaADTO);
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarSolHojaB(ProyectoModel periodoModel)
        {
            int result = 0;
            SolHojaBDTO solHojaBDTO = periodoModel.SolHojaBDTO;
            solHojaBDTO.SolhojabCodi = campaniasAppService.GetLastSolHojaBId();
            solHojaBDTO.ProyCodi = periodoModel.ProyCodi;
            solHojaBDTO.IndDel = Constantes.IndDel;
            solHojaBDTO.UsuCreacion = User.Identity.Name;

            Boolean deleteF = campaniasAppService.DeleteSolHojaBById(solHojaBDTO.ProyCodi, User.Identity.Name);
            if (deleteF)
            {
                campaniasAppService.SaveSolHojaB(solHojaBDTO);
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarSolHojaC(ProyectoModel periodoModel)
        {
            int result = 0;
            SolHojaCDTO SolHojaCDTO = periodoModel.SolHojaCDTO;
            SolHojaCDTO.Solhojaccodi = campaniasAppService.GetLastSolHojaCId();
            SolHojaCDTO.Proycodi = periodoModel.ProyCodi;
            SolHojaCDTO.IndDel = Constantes.IndDel;
            SolHojaCDTO.Usucreacion = User.Identity.Name;
            Boolean deleteF = campaniasAppService.DeleteSolHojaCById(SolHojaCDTO.Proycodi, User.Identity.Name);
            if (deleteF)
            {
                if (campaniasAppService.SaveSolHojaC(SolHojaCDTO))
                {
                    campaniasAppService.DeleteDetSolHojaCById(SolHojaCDTO.Proycodi, User.Identity.Name);
                    if (periodoModel.ListaDetSolHojaCDTO != null) 
                    {
                        foreach (var item in periodoModel.ListaDetSolHojaCDTO)
                        {
                            DetSolHojaCDTO DetSolHojaCDTO = item;
                            DetSolHojaCDTO.Detasolhccodi = campaniasAppService.GetLastDetSolHojaCId();
                            DetSolHojaCDTO.Solhojaccodi = SolHojaCDTO.Solhojaccodi;
                            DetSolHojaCDTO.IndDel = Constantes.IndDel;
                            DetSolHojaCDTO.Usucreacion = User.Identity.Name;
                            campaniasAppService.SaveDetSolHojaC(DetSolHojaCDTO);
                        }
                    }
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarBioHojaA(ProyectoModel periodoModel)
        {
            int result = 0;
            BioHojaADTO BioHojaADTO = periodoModel.BioHojaADTO;
            BioHojaADTO.BiohojaaCodi = campaniasAppService.GetLastBioHojaAId();
            BioHojaADTO.ProyCodi = periodoModel.ProyCodi;
            BioHojaADTO.IndDel = Constantes.IndDel;
            BioHojaADTO.UsuCreacion = User.Identity.Name;

            Boolean deleteF = campaniasAppService.DeleteBioHojaAById(BioHojaADTO.ProyCodi, User.Identity.Name);
            if (deleteF)
            {
                campaniasAppService.SaveBioHojaA(BioHojaADTO);
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarCuestionarioH2VA(ProyectoModel periodoModel)
        {
            int result = 0;
            CuestionarioH2VADTO cuestionarioH2VADTO = periodoModel.CuestionarioH2VADTO;
            cuestionarioH2VADTO.H2vaCodi = campaniasAppService.GetLastCuestionarioH2VAId();
            cuestionarioH2VADTO.ProyCodi = periodoModel.ProyCodi;
            cuestionarioH2VADTO.IndDel = Constantes.IndDel;
            cuestionarioH2VADTO.UsuCreacion = User.Identity.Name;

            Boolean deleteF = campaniasAppService.DeleteCuestionarioH2VAById(cuestionarioH2VADTO.ProyCodi, User.Identity.Name);
            if (deleteF)
            {
                if (campaniasAppService.SaveCuestionarioH2VA(cuestionarioH2VADTO))
                {
                    campaniasAppService.DeleteCuestionarioH2VADet1ById(cuestionarioH2VADTO.ProyCodi, User.Identity.Name);
                    campaniasAppService.DeleteCuestionarioH2VADet2ById(cuestionarioH2VADTO.ProyCodi, User.Identity.Name);
                    if (periodoModel.CuestionarioH2VADTO.ListCH2VADet1DTOs != null)
                    {
                        foreach (var item in periodoModel.CuestionarioH2VADTO.ListCH2VADet1DTOs)
                        {
                            CuestionarioH2VADet1DTO cuestionarioH2VADet1DTO = item;
                            cuestionarioH2VADet1DTO.H2vaDet1Codi = campaniasAppService.GetLastCuestionarioH2VADet1Id();
                            cuestionarioH2VADet1DTO.H2vaCodi = cuestionarioH2VADTO.H2vaCodi;
                            cuestionarioH2VADet1DTO.IndDel = Constantes.IndDel;
                            cuestionarioH2VADet1DTO.UsuCreacion = User.Identity.Name;
                            campaniasAppService.SaveCuestionarioH2VADet1(cuestionarioH2VADet1DTO);
                        }
                    }

                    if (periodoModel.CuestionarioH2VADTO.ListCH2VADet2DTOs != null)
                    {
                        foreach (var item in periodoModel.CuestionarioH2VADTO.ListCH2VADet2DTOs)
                        {
                            CuestionarioH2VADet2DTO cuestionarioH2VADet2DTO = item;
                            cuestionarioH2VADet2DTO.H2vaDet2Codi = campaniasAppService.GetLastCuestionarioH2VADet2Id();
                            cuestionarioH2VADet2DTO.H2vaCodi = cuestionarioH2VADTO.H2vaCodi;
                            cuestionarioH2VADet2DTO.IndDel = Constantes.IndDel;
                            cuestionarioH2VADet2DTO.UsuCreacion = User.Identity.Name;
                            campaniasAppService.SaveCuestionarioH2VADet2(cuestionarioH2VADet2DTO);
                        }
                    }

                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarCuestionarioH2VB(ProyectoModel periodoModel)
        {
            int result = 0;
            CuestionarioH2VBDTO cuestionarioH2VBDTO = periodoModel.CuestionarioH2VBDTO;
            cuestionarioH2VBDTO.H2vbCodi = campaniasAppService.GetLastCuestionarioH2VBId();
            cuestionarioH2VBDTO.ProyCodi = periodoModel.ProyCodi;
            cuestionarioH2VBDTO.IndDel = Constantes.IndDel;
            cuestionarioH2VBDTO.UsuCreacion = User.Identity.Name;

            Boolean deleteF = campaniasAppService.DeleteCuestionarioH2VBById(cuestionarioH2VBDTO.ProyCodi, User.Identity.Name);
            if (deleteF)
            {
                campaniasAppService.SaveCuestionarioH2VB(cuestionarioH2VBDTO);
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarBioHojaB(ProyectoModel periodoModel)
        {
            int result = 0;
            BioHojaBDTO BioHojaBDTO = periodoModel.BioHojaBDTO;
            BioHojaBDTO.BiohojabCodi = campaniasAppService.GetLastBioHojaBId();
            BioHojaBDTO.ProyCodi = periodoModel.ProyCodi;
            BioHojaBDTO.IndDel = Constantes.IndDel;
            BioHojaBDTO.UsuCreacion = User.Identity.Name;
            Boolean deleteF = campaniasAppService.DeleteBioHojaBById(BioHojaBDTO.ProyCodi, User.Identity.Name);
            if (deleteF)
            {
                campaniasAppService.SaveBioHojaB(BioHojaBDTO);
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarBioHojaC(ProyectoModel periodoModel)
        {
            int result = 0;
            BioHojaCDTO BioHojaCDTO = periodoModel.BioHojaCDTO;
            BioHojaCDTO.Biohojaccodi = campaniasAppService.GetLastBioHojaCId();
            BioHojaCDTO.Proycodi = periodoModel.ProyCodi;
            BioHojaCDTO.IndDel = Constantes.IndDel;
            BioHojaCDTO.Usucreacion = User.Identity.Name;
            Boolean deleteF = campaniasAppService.DeleteBioHojaCById(BioHojaCDTO.Proycodi, User.Identity.Name);
            if (deleteF)
            {
                if (campaniasAppService.SaveBioHojaC(BioHojaCDTO))
                {
                    campaniasAppService.DeleteDetBioHojaCById(BioHojaCDTO.Proycodi, User.Identity.Name);
                    foreach (var item in periodoModel.ListaDetBioHojaCDTO)
                    {
                        DetBioHojaCDTO DetBioHojaCDTO = item;
                        DetBioHojaCDTO.Detabiohccodi = campaniasAppService.GetLastDetBioHojaCId();
                        DetBioHojaCDTO.Biohojaccodi = BioHojaCDTO.Biohojaccodi;
                        DetBioHojaCDTO.IndDel = Constantes.IndDel;
                        DetBioHojaCDTO.Usucreacion = User.Identity.Name;
                        campaniasAppService.SaveDetBioHojaC(DetBioHojaCDTO);
                    }
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }


        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarITCFE01(ProyectoModel periodoModel)
        {
           int result = 0;
           ITCFE01DTO regFE01DTO = periodoModel.ITCFE01DTO;
           regFE01DTO.Fichae01codi = campaniasAppService.GetLastRegITCFE01Id();
           regFE01DTO.Proycodi = periodoModel.ProyCodi;
           regFE01DTO.IndDel = Constantes.IndDel;
           regFE01DTO.Usucreacion = User.Identity.Name;
            Boolean deleteF = campaniasAppService.DeleteRegITCFE01ById(regFE01DTO.Proycodi, User.Identity.Name);
            if (deleteF)
            {
                campaniasAppService.SaveRegITCFE01(regFE01DTO);
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }




        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarItcdf104(ProyectoModel periodoModel)
        {
            int result = 0;

            // Verificar si el modelo es nulo
            if (periodoModel == null)
            {
                return Json(1); // Retorna  si el modelo completo es nulo
            }

            // Siempre intenta eliminar los datos anteriores
            if (campaniasAppService.DeleteItcdf104ById(periodoModel.ProyCodi, User.Identity.Name))
            {
                // Si la lista es nula o vacía, no realiza el guardado, pero ya se eliminaron los datos
                if (periodoModel.Itcdf104DTOs != null && periodoModel.Itcdf104DTOs.Any())
                {
                    // Procesar y guardar los nuevos registros
                    foreach (var detReg in periodoModel.Itcdf104DTOs)
                    {
                        Itcdf104DTO ObjItcdf104 = detReg;
                        ObjItcdf104.Itcdf104Codi = campaniasAppService.GetLastItcdf104Id();
                        ObjItcdf104.ProyCodi = periodoModel.ProyCodi;
                        ObjItcdf104.IndDel = Constantes.IndDel;
                        ObjItcdf104.UsuCreacion = User.Identity.Name;
                        campaniasAppService.SaveItcdf104(ObjItcdf104);
                    }
                }
                result = 1; // Operación exitosa (se eliminó y, si había, se guardaron nuevos datos)
            }
            else
            {
                result = 0; // Error al eliminar
            }

            return Json(result);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarItcdf108(ProyectoModel periodoModel)
        {
            int result = 0;
            List<Itcdf108DTO> itcdf108DTOS = periodoModel.Itcdf108DTOs;

            if (campaniasAppService.DeleteItcdf108ById(periodoModel.ProyCodi, User.Identity.Name))
            {
                // Verificar si la lista es nula o vacía
                if (itcdf108DTOS == null || !itcdf108DTOS.Any())
                {
                    return Json(1); // No realiza ninguna acción si está vacía o nula
                }
                foreach (var detReg in itcdf108DTOS)
                {
                    Itcdf108DTO ObjItcdf108 = detReg;
                    ObjItcdf108.Itcdf108Codi = campaniasAppService.GetLastItcdf108Id();
                    ObjItcdf108.ProyCodi = periodoModel.ProyCodi;
                    ObjItcdf108.IndDel = Constantes.IndDel;
                    ObjItcdf108.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveItcdf108(ObjItcdf108);
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }

        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarItcdf110(ProyectoModel periodoModel)
        {
            int result = 0;

            campaniasAppService.DeleteItcdf110ById(periodoModel.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteItcdf110DetById(periodoModel.ProyCodi, User.Identity.Name);
            // Verificar si la lista es nula o vacía
            if (periodoModel.Itcdf110DTOs == null || !periodoModel.Itcdf110DTOs.Any())
            {
                return Json(1); // No realiza ninguna acción si está vacía o nula
            }
            foreach (var item in periodoModel.Itcdf110DTOs)
            {

                Itcdf110DTO itcdf110DTO = item;
                itcdf110DTO.Itcdf110Codi = campaniasAppService.GetLastItcdf110Id();
                itcdf110DTO.ProyCodi = periodoModel.ProyCodi;
                itcdf110DTO.IndDel = Constantes.IndDel;
                itcdf110DTO.UsuCreacion = User.Identity.Name;
                campaniasAppService.SaveItcdf110(itcdf110DTO);

                foreach (var item1 in item.ListItcdf110Det)
                {
                    Itcdf110DetDTO itcdf110DetDTO = item1;
                    itcdf110DetDTO.IndDel = Constantes.IndDel;
                    itcdf110DetDTO.UsuCreacion = User.Identity.Name;
                    itcdf110DetDTO.Itcdf110DetCodi = campaniasAppService.GetLastItcdf110DetId();
                    itcdf110DetDTO.Itcdf110Codi = itcdf110DTO.Itcdf110Codi;
                    campaniasAppService.SaveItcdf110Det(itcdf110DetDTO);
                }
            }

            result = 1;
            return Json(result);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarItcdf116(ProyectoModel periodoModel)
        {
            int result = 0;

            campaniasAppService.DeleteItcdf116ById(periodoModel.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteItcdf116DetById(periodoModel.ProyCodi, User.Identity.Name);
            // Verificar si la lista es nula o vacía
            if (periodoModel.Itcdf116DTOs == null || !periodoModel.Itcdf116DTOs.Any())
            {
                return Json(1); // No realiza ninguna acción si está vacía o nula
            }
            foreach (var item in periodoModel.Itcdf116DTOs)
            {
                Itcdf116DTO itcdf116DTO = item;
                itcdf116DTO.Itcdf116Codi = campaniasAppService.GetLastItcdf116Id();
                itcdf116DTO.ProyCodi = periodoModel.ProyCodi;
                itcdf116DTO.IndDel = Constantes.IndDel;
                itcdf116DTO.UsuCreacion = User.Identity.Name;
                campaniasAppService.SaveItcdf116(itcdf116DTO);

                if (item.ListItcdf116Det != null && item.ListItcdf116Det.Any())
                {
                    foreach (var item1 in item.ListItcdf116Det)
                    {
                        if (item1.Valor != null)
                        {
                            Itcdf116DetDTO itcdf116DetDTO = item1;
                            itcdf116DetDTO.IndDel = Constantes.IndDel;
                            itcdf116DetDTO.UsuCreacion = User.Identity.Name;
                            itcdf116DetDTO.Itcdf116DetCodi = campaniasAppService.GetLastItcdf116DetId();
                            itcdf116DetDTO.Itcdf116Codi = itcdf116DTO.Itcdf116Codi;
                            campaniasAppService.SaveItcdf116Det(itcdf116DetDTO);
                        }
                    }
                }
            }

            result = 1;
            return Json(result);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarItcdf121(ProyectoModel periodoModel)
        {
            int result = 0;

            campaniasAppService.DeleteItcdf121ById(periodoModel.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteItcdf121DetById(periodoModel.ProyCodi, User.Identity.Name);
            // Verificar si la lista es nula o vacía
            if (periodoModel.Itcdf121DTOs == null || !periodoModel.Itcdf121DTOs.Any())
            {
                return Json(1); // No realiza ninguna acción si está vacía o nula
            }
            foreach (var item in periodoModel.Itcdf121DTOs)
            {
                Itcdf121DTO itcdf121DTO = item;
                itcdf121DTO.Itcdf121Codi = campaniasAppService.GetLastItcdf121Id();
                itcdf121DTO.ProyCodi = periodoModel.ProyCodi;
                itcdf121DTO.IndDel = Constantes.IndDel;
                itcdf121DTO.UsuCreacion = User.Identity.Name;
                campaniasAppService.SaveItcdf121(itcdf121DTO);

                foreach (var item1 in item.ListItcdf121Det)
                {
                    Itcdf121DetDTO itcdf121DetDTO = item1;
                    itcdf121DetDTO.IndDel = Constantes.IndDel;
                    itcdf121DetDTO.UsuCreacion = User.Identity.Name;
                    itcdf121DetDTO.Itcdf121DetCodi = campaniasAppService.GetLastItcdf121DetId();
                    itcdf121DetDTO.Itcdf121Codi = itcdf121DTO.Itcdf121Codi;
                    campaniasAppService.SaveItcdf121Det(itcdf121DetDTO);
                }
            }

            result = 1;
            return Json(result);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarItcdf123(ProyectoModel periodoModel)
        {
            int result = 0;
            List<Itcdf123DTO> itcdf123DTOS = periodoModel.Itcdf123DTOs;

            if (campaniasAppService.DeleteItcdf123ById(periodoModel.ProyCodi, User.Identity.Name))
            {
                // Verificar si la lista es nula o vacía
                if (itcdf123DTOS == null || !itcdf123DTOS.Any())
                {
                    return Json(1); // No realiza ninguna acción si está vacía o nula
                }
                foreach (var detReg in itcdf123DTOS)
                {
                    Itcdf123DTO ObjItcdf123 = detReg;
                    ObjItcdf123.Itcdf123Codi = campaniasAppService.GetLastItcdf123Id();
                    ObjItcdf123.ProyCodi = periodoModel.ProyCodi;
                    ObjItcdf123.IndDel = Constantes.IndDel;
                    ObjItcdf123.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveItcdf123(ObjItcdf123);
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarItcdfp011(ProyectoModel periodoModel)
        {
            int result = 0;
            campaniasAppService.DeleteItcdfp011ById(periodoModel.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteItcdfp011DetById(periodoModel.ProyCodi, User.Identity.Name);
            // Verificar si la lista es nula o vacía
          
            Itcdfp011DTO itcdfp011DTO = periodoModel.Itcdfp011DTO;
            itcdfp011DTO.Itcdfp011Codi = campaniasAppService.GetLastItcdfp011Id();
            itcdfp011DTO.ProyCodi = periodoModel.ProyCodi;
            itcdfp011DTO.IndDel = Constantes.IndDel;
            itcdfp011DTO.UsuCreacion = User.Identity.Name;
            campaniasAppService.SaveItcdfp011(itcdfp011DTO);
            if (periodoModel.ListaItcdfp011DetDTO == null)
            {
                 return Json(new { result = 1, itcdfp011 = itcdfp011DTO });
            }
            var itcdfp011Codi = itcdfp011DTO.Itcdfp011Codi;
            var usuCreacion = User.Identity.Name;
            campaniasAppService.SaveItcdfp011Det(periodoModel.ListaItcdfp011DetDTO, itcdfp011Codi, usuCreacion);
            // foreach (var detReg in periodoModel.ListaItcdfp011DetDTO)
            // { 
            //         Itcdfp011DetDTO Itcdfp011DetDTO = detReg;
            //         //Itcdfp011DetDTO.Itcdfp011DetCodi = campaniasAppService.GetLastItcdfp011DetId();
            //         Itcdfp011DetDTO.Itcdfp011Codi = itcdfp011DTO.Itcdfp011Codi;
            //         Itcdfp011DetDTO.IndDel = Constantes.IndDel;
            //         Itcdfp011DetDTO.UsuCreacion = User.Identity.Name;
            //         campaniasAppService.SaveItcdfp011Det(Itcdfp011DetDTO);
            // }

            result = 1;
            return Json(new { result, itcdfp011 = itcdfp011DTO });
        }

         [System.Web.Mvc.HttpPost]
        public JsonResult GrabarItcdfp011Det(ProyectoModel periodoModel)
        {
            int result = 0;
            
            if (periodoModel.ListaItcdfp011DetDTO == null)
            {
                return Json(1);
            }
            var itcdfp011Codi = periodoModel.Itcdfp011DTO.Itcdfp011Codi;
            var usuCreacion = User.Identity.Name;
            campaniasAppService.SaveItcdfp011Det(periodoModel.ListaItcdfp011DetDTO, itcdfp011Codi, usuCreacion);

            result = 1;
            return Json(result);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarItcdfp012(ProyectoModel periodoModel)
        {
            int result = 0;
            List<Itcdfp012DTO> Itcdfp012DTOs = periodoModel.Itcdfp012DTOs;
            if (campaniasAppService.DeleteItcdfp012ById(periodoModel.ProyCodi, User.Identity.Name))
            {
                // Verificar si la lista es nula o vacía
                if (Itcdfp012DTOs == null)
                {
                    return Json(1); // No realiza ninguna acción si está vacía o nula
                }
                foreach (var detReg in Itcdfp012DTOs)
                {
                    Itcdfp012DTO ObjItcdfp102 = detReg;
                    ObjItcdfp102.Itcdfp012Codi = campaniasAppService.GetLastItcdfp012Id();
                    ObjItcdfp102.ProyCodi = periodoModel.ProyCodi;
                    ObjItcdfp102.IndDel = Constantes.IndDel;
                    ObjItcdfp102.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveItcdfp012(ObjItcdfp102);
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }

        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarItcdfp013(ProyectoModel periodoModel)
        {
            /* int result = 0;
             List<Itcdfp013DTO> itcdfp013DTOs = periodoModel.Itcdfp013DTOs;
             itcdfp013DTOs.Itcdfp013Codi = campaniasAppService.GetLastItcdfp013Id();
             itcdfp013DTOs.ProyCodi = periodoModel.ProyCodi;
             itcdfp013DTOs.IndDel = Constantes.IndDel;
             itcdfp013DTOs.UsuCreacion = User.Identity.Name;
             campaniasAppService.SaveItcdfp013(itcdfp013DTOs);

             foreach (var detReg in periodoModel.ListaItcdfp013DetDTO)
             {
                 Itcdfp013DetDTO Itcdfp013DetDTO = detReg;
                 Itcdfp013DetDTO.Itcdfp013DetCodi = campaniasAppService.GetLastItcdfp013DetId();
                 Itcdfp013DetDTO.Itcdfp013Codi = itcdfp013DTO.Itcdfp013Codi;
                 Itcdfp013DetDTO.IndDel = Constantes.IndDel;
                 Itcdfp013DetDTO.UsuCreacion = User.Identity.Name;
                 campaniasAppService.SaveItcdfp013Det(Itcdfp013DetDTO);
             }

             result = 1;*/
            //return Json(result);



            int result = 0;

            campaniasAppService.DeleteItcdfp013ById(periodoModel.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteItcdfp013DetById(periodoModel.ProyCodi, User.Identity.Name);
            // Verificar si la lista es nula o vacía
            if (periodoModel.Itcdfp013DTOs == null)
            {
                return Json(1); // No realiza ninguna acción si está vacía o nula
            }
            foreach (var item in periodoModel.Itcdfp013DTOs)
            {
                Itcdfp013DTO itcdfp013DTO = item;
                itcdfp013DTO.Itcdfp013Codi = campaniasAppService.GetLastItcdfp013Id();
                itcdfp013DTO.ProyCodi = periodoModel.ProyCodi;
                itcdfp013DTO.IndDel = Constantes.IndDel;
                itcdfp013DTO.UsuCreacion = User.Identity.Name;
                campaniasAppService.SaveItcdfp013(itcdfp013DTO);

                foreach (var item1 in item.ListItcdf013Det)
                {
                    Itcdfp013DetDTO itcdfp013DetDTO = item1;
                    itcdfp013DetDTO.IndDel = Constantes.IndDel;
                    itcdfp013DetDTO.UsuCreacion = User.Identity.Name;
                    itcdfp013DetDTO.Itcdfp013DetCodi = campaniasAppService.GetLastItcdfp013DetId();
                    itcdfp013DetDTO.Itcdfp013Codi = itcdfp013DTO.Itcdfp013Codi;
                    campaniasAppService.SaveItcdfp013Det(itcdfp013DetDTO);
                }
            }

            result = 1;
            return Json(result);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult ObtenerEmpresaPlan(int id)
        {
            PlanTransmisionDTO planTransmisionDTO = campaniasAppService.GetPlanTransmisionById(id);
            return Json(planTransmisionDTO);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult EliminarPlanTrans(int id)
        {
            int result = 0;
            string usuario = User.Identity.Name;
            if (campaniasAppService.DeletePlanTransmisionById(id, usuario))
            {
                    EnvioDto envioDto = new EnvioDto();
                    envioDto.PlanTransmision = campaniasAppService.GetPlanTransmisionById(id);
                    envioDto.ListaTrsmProyecto = campaniasAppService.GetTransmisionProyecto(id);
                    envioDto.Correos = base.UserEmail;
                    int plantillaCorreo = 324;
                    campaniasAppService.EnviarCorreoNotificacion(envioDto, plantillaCorreo);
                    return Json(new { success = true, result = 1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult EliminarProyecto(int id, int idPlan)
        {
            int result = 0;
            string usuario = User.Identity.Name;
            if (campaniasAppService.DeleteTransmisionProyectoById(id, usuario))
            {
                campaniasAppService.UpdateProyRegById(idPlan);
                return Json(new { success = true, result = 1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarItcprm1(ProyectoModel periodoModel)
        {
            int result = 0;
            List<ItcPrm1Dto> ItcPrm1Dtos = periodoModel.ListaItcPrm1DTO;




            if (campaniasAppService.DeleteItcprm1ById(periodoModel.ProyCodi, User.Identity.Name))
            {

                // Verificar si la lista es nula o vacía
                if (ItcPrm1Dtos == null || !ItcPrm1Dtos.Any())
                {
                    return Json(1); // No realiza ninguna acción si está vacía o nula
                }
                foreach (var detReg in ItcPrm1Dtos)
                {
                    ItcPrm1Dto itcPrm1Dto = detReg;
                    itcPrm1Dto.ItcPrm1Codi = campaniasAppService.GetLastItcprm1Id();
                    itcPrm1Dto.ProyCodi = periodoModel.ProyCodi;
                    itcPrm1Dto.IndDel = Constantes.IndDel;
                    itcPrm1Dto.Usucreacion = User.Identity.Name;
                    campaniasAppService.SaveItcprm1(itcPrm1Dto);
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarItcprm2(ProyectoModel periodoModel)
        {
            int result = 0;
            List<ItcPrm2Dto> ItcPrm2Dtos = periodoModel.ListaItcPrm2DTO;

            if (campaniasAppService.DeleteItcprm2ById(periodoModel.ProyCodi, User.Identity.Name))
            {
                // Verificar si la lista es nula o vacía
                if (ItcPrm2Dtos == null || !ItcPrm2Dtos.Any())
                {
                    return Json(1); // No realiza ninguna acción si está vacía o nula
                }
                foreach (var detReg in ItcPrm2Dtos)
                {
                    ItcPrm2Dto itcPrm2Dto = detReg;
                    itcPrm2Dto.ItcPrm2Codi = campaniasAppService.GetLastItcprm2Id();
                    itcPrm2Dto.ProyCodi = periodoModel.ProyCodi;
                    itcPrm2Dto.IndDel = Constantes.IndDel;
                    itcPrm2Dto.Usucreacion = User.Identity.Name;
                    campaniasAppService.SaveItcprm2(itcPrm2Dto);
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarItcprm2_(ProyectoModel periodoModel)
        {
            int result = 0;
            List<ItcPrm2Dto> ItcPrm2Dtos = periodoModel.ListaItcPrm2DTO;

            if (ItcPrm2Dtos == null || !ItcPrm2Dtos.Any())
            {
                return Json(1); // No realiza ninguna acción si está vacía o nula
            }
            foreach (var detReg in ItcPrm2Dtos)
            {
                ItcPrm2Dto itcPrm2Dto = detReg;
                itcPrm2Dto.ItcPrm2Codi = campaniasAppService.GetLastItcprm2Id();
                itcPrm2Dto.ProyCodi = periodoModel.ProyCodi;
                itcPrm2Dto.IndDel = Constantes.IndDel;
                itcPrm2Dto.Usucreacion = User.Identity.Name;
                campaniasAppService.SaveItcprm2(itcPrm2Dto);
            }
            result = 1;
            return Json(result);
            
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarItcred1(ProyectoModel periodoModel)
        {
            int result = 0;
            List<ItcRed1Dto> ItcRed1Dtos = periodoModel.ListaItcRed1DTO;

            if (campaniasAppService.DeleteItcred1ById(periodoModel.ProyCodi, User.Identity.Name))
            {
                // Verificar si la lista es nula o vacía
                if (ItcRed1Dtos == null || !ItcRed1Dtos.Any())
                {
                    return Json(1); // No realiza ninguna acción si está vacía o nula
                }
                foreach (var detReg in ItcRed1Dtos)
                {
                    ItcRed1Dto itcred1Dto = detReg;
                    itcred1Dto.ItcRed1Codi = campaniasAppService.GetLastItcred1Id();
                    itcred1Dto.ProyCodi = periodoModel.ProyCodi;
                    itcred1Dto.IndDel = Constantes.IndDel;
                    itcred1Dto.Usucreacion = User.Identity.Name;
                    campaniasAppService.SaveItcred1(itcred1Dto);
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarItcred2(ProyectoModel periodoModel)
        {
            int result = 0;
            List<ItcRed2Dto> ItcRed2Dtos = periodoModel.ListaItcRed2DTO;

            if (campaniasAppService.DeleteItcred2ById(periodoModel.ProyCodi, User.Identity.Name))
            {

                // Verificar si la lista es nula o vacía
                if (ItcRed2Dtos == null || !ItcRed2Dtos.Any())
                {
                    return Json(1); // No realiza ninguna acción si está vacía o nula
                }
                foreach (var detReg in ItcRed2Dtos)
                {
                    ItcRed2Dto itcred2Dto = detReg;
                    itcred2Dto.ItcRed2Codi = campaniasAppService.GetLastItcred2Id();
                    itcred2Dto.ProyCodi = periodoModel.ProyCodi;
                    itcred2Dto.IndDel = Constantes.IndDel;
                    itcred2Dto.Usucreacion = User.Identity.Name;
                    campaniasAppService.SaveItcred2(itcred2Dto);
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarItcred3(ProyectoModel periodoModel)
        {
            int result = 0;
            List<ItcRed3Dto> ItcRed3Dtos = periodoModel.ListaItcRed3DTO;

            if (campaniasAppService.DeleteItcred3ById(periodoModel.ProyCodi, User.Identity.Name))
            {
                // Verificar si la lista es nula o vacía
                if (ItcRed3Dtos == null || !ItcRed3Dtos.Any())
                {
                    return Json(1); // No realiza ninguna acción si está vacía o nula
                }
                foreach (var detReg in ItcRed3Dtos)
                {
                    ItcRed3Dto itcred3Dto = detReg;
                    itcred3Dto.ItcRed3Codi = campaniasAppService.GetLastItcred3Id();
                    itcred3Dto.ProyCodi = periodoModel.ProyCodi;
                    itcred3Dto.IndDel = Constantes.IndDel;
                    itcred3Dto.Usucreacion = User.Identity.Name;
                    campaniasAppService.SaveItcred3(itcred3Dto);
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarItcred4(ProyectoModel periodoModel)
        {
            int result = 0;
            List<ItcRed4Dto> ItcRed4Dtos = periodoModel.ListaItcRed4DTO;

            if (campaniasAppService.DeleteItcred4ById(periodoModel.ProyCodi, User.Identity.Name))
            {
                // Verificar si la lista es nula o vacía
                if (ItcRed4Dtos == null || !ItcRed4Dtos.Any())
                {
                    return Json(1); // No realiza ninguna acción si está vacía o nula
                }
                foreach (var detReg in ItcRed4Dtos)
                {
                    ItcRed4Dto itcred4Dto = detReg;
                    itcred4Dto.ItcRed4Codi = campaniasAppService.GetLastItcred4Id();
                    itcred4Dto.ProyCodi = periodoModel.ProyCodi;
                    itcred4Dto.IndDel = Constantes.IndDel;
                    itcred4Dto.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveItcred4(itcred4Dto);
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarItcred5(ProyectoModel periodoModel)
        {
            int result = 0;
            List<ItcRed5Dto> ItcRed5Dtos = periodoModel.ListaItcRed5DTO;

            if (campaniasAppService.DeleteItcred5ById(periodoModel.ProyCodi, User.Identity.Name))
            {
                // Verificar si la lista es nula o vacía
                if (ItcRed5Dtos == null || !ItcRed5Dtos.Any())
                {
                    return Json(1); // No realiza ninguna acción si está vacía o nula
                }
                foreach (var detReg in ItcRed5Dtos)
                {
                    ItcRed5Dto itcred5Dto = detReg;
                    itcred5Dto.ItcRed5Codi = campaniasAppService.GetLastItcred5Id();
                    itcred5Dto.ProyCodi = periodoModel.ProyCodi;
                    itcred5Dto.IndDel = Constantes.IndDel;
                    itcred5Dto.Usucreacion = User.Identity.Name;
                    campaniasAppService.SaveItcred5(itcred5Dto);
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarCH2VA(ProyectoModel periodoModel)
        {
            int result = 0;
            CuestionarioH2VADTO cuestionarioH2VADTO = periodoModel.CuestionarioH2VADTO;
            cuestionarioH2VADTO.H2vaCodi = campaniasAppService.GetLastCuestionarioH2VAId();
            cuestionarioH2VADTO.ProyCodi = periodoModel.ProyCodi;
            cuestionarioH2VADTO.IndDel = Constantes.IndDel;
            cuestionarioH2VADTO.UsuCreacion = User.Identity.Name;
            campaniasAppService.SaveCuestionarioH2VA(cuestionarioH2VADTO);

            foreach (var detReg in periodoModel.ListaCH2VADet1DTOs)
            {
                CuestionarioH2VADet1DTO cuestionarioH2VADet1DTO = detReg;
                cuestionarioH2VADet1DTO.H2vaDet1Codi = campaniasAppService.GetLastCuestionarioH2VADet1Id();
                cuestionarioH2VADet1DTO.H2vaCodi = cuestionarioH2VADTO.H2vaCodi;
                cuestionarioH2VADet1DTO.IndDel = Constantes.IndDel;
                cuestionarioH2VADet1DTO.UsuCreacion = User.Identity.Name;
                campaniasAppService.SaveCuestionarioH2VADet1(cuestionarioH2VADet1DTO);
            }
            foreach (var detReg in periodoModel.ListaCH2VADet2DTOs)
            {
                CuestionarioH2VADet2DTO cuestionarioH2VADet2DTO = detReg;
                cuestionarioH2VADet2DTO.H2vaDet2Codi = campaniasAppService.GetLastCuestionarioH2VADet2Id();
                cuestionarioH2VADet2DTO.H2vaCodi = cuestionarioH2VADTO.H2vaCodi;
                cuestionarioH2VADet2DTO.IndDel = Constantes.IndDel;
                cuestionarioH2VADet2DTO.UsuCreacion = User.Identity.Name;
                campaniasAppService.SaveCuestionarioH2VADet2(cuestionarioH2VADet2DTO);
            }

            result = 1;
            return Json(result);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarSubestFicha1(ProyectoModel periodoModel)
        {
            int result = 0;
            SubestFicha1DTO subestFicha1DTO = periodoModel.subestFicha1DTO;
            subestFicha1DTO.SubestFicha1Codi = campaniasAppService.GetLastSubestFicha1Id();
            subestFicha1DTO.ProyCodi = periodoModel.ProyCodi;
            subestFicha1DTO.IndDel = Constantes.IndDel;
            subestFicha1DTO.UsuCreacion = User.Identity.Name;
            campaniasAppService.DeleteSubestFicha1ById(subestFicha1DTO.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteSubestFicha1Det1ById(subestFicha1DTO.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteSubestFicha1Det2ById(subestFicha1DTO.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteSubestFicha1Det3ById(subestFicha1DTO.ProyCodi, User.Identity.Name);
            campaniasAppService.SaveSubestFicha1(subestFicha1DTO);

            if (periodoModel.listaSubestFicha1Det1DTO != null) {
                foreach (var detReg in periodoModel.listaSubestFicha1Det1DTO)
                {
                    SubestFicha1Det1DTO subestFicha1Det1DTO = detReg;
                    subestFicha1Det1DTO.SubestFicha1Det1Codi = campaniasAppService.GetLastSubestFicha1Det1Id();
                    subestFicha1Det1DTO.SubestFicha1Codi = subestFicha1DTO.SubestFicha1Codi;
                    subestFicha1Det1DTO.IndDel = Constantes.IndDel;
                    subestFicha1Det1DTO.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveSubestFicha1Det1(subestFicha1Det1DTO);
                }
            }

            if (periodoModel.listaSubestFicha1Det2DTO != null)
            {
                foreach (var detReg in periodoModel.listaSubestFicha1Det2DTO)
                {
                    SubestFicha1Det2DTO subestFicha1Det2DTO = detReg;
                    subestFicha1Det2DTO.SubestFicha1Det2Codi = campaniasAppService.GetLastSubestFicha1Det2Id();
                    subestFicha1Det2DTO.SubestFicha1Codi = subestFicha1DTO.SubestFicha1Codi;
                    subestFicha1Det2DTO.IndDel = Constantes.IndDel;
                    subestFicha1Det2DTO.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveSubestFicha1Det2(subestFicha1Det2DTO);
                }
            }

            if (periodoModel.listaSubestFicha1Det3DTO != null)
            {
                foreach (var detReg in periodoModel.listaSubestFicha1Det3DTO)
                {
                    SubestFicha1Det3DTO subestFicha1Det3DTO = detReg;
                    subestFicha1Det3DTO.SubestFicha1Det3Codi = campaniasAppService.GetLastSubestFicha1Det3Id();
                    subestFicha1Det3DTO.SubestFicha1Codi = subestFicha1DTO.SubestFicha1Codi;
                    subestFicha1Det3DTO.IndDel = Constantes.IndDel;
                    subestFicha1Det3DTO.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveSubestFicha1Det3(subestFicha1Det3DTO);
                }
            }
            result = 1;
            return Json(result);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarCH2VC(ProyectoModel periodoModel)
        {
            int result = 0;
            List<CuestionarioH2VCDTO> CH2VCdto = periodoModel.Ch2vcDTOs;
            Boolean deleteF = campaniasAppService.DeleteCuestionarioH2VCById(periodoModel.ProyCodi, User.Identity.Name);
            if (deleteF)
            {
                foreach (var detReg in CH2VCdto)
                {

                    CuestionarioH2VCDTO ch2vcdto = detReg;
                    ch2vcdto.H2vcCodi = campaniasAppService.GetLastCuestionarioH2VCId();
                    ch2vcdto.ProyCodi = periodoModel.ProyCodi;
                    ch2vcdto.IndDel = Constantes.IndDel;
                    ch2vcdto.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveCuestionarioH2VC(ch2vcdto);
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarCH2VF(ProyectoModel periodoModel)
        {
            int result = 0;
            CuestionarioH2VFDTO CH2VFdto = periodoModel.Ch2vfDTO;
            CH2VFdto.H2vFCodi = campaniasAppService.GetLastCuestionarioH2VFId();
            CH2VFdto.ProyCodi = periodoModel.ProyCodi;
            CH2VFdto.IndDel = Constantes.IndDel;
            CH2VFdto.UsuCreacion = User.Identity.Name;
            Boolean deleteF = campaniasAppService.DeleteCuestionarioH2VFById(CH2VFdto.ProyCodi, User.Identity.Name);
            if (deleteF)
            {
                campaniasAppService.SaveCuestionarioH2VF(CH2VFdto);
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarCH2VG(ProyectoModel periodoModel)
        {
            int result = 0;
            List<CuestionarioH2VGDTO> CH2VGdto = periodoModel.Ch2vgDTOs;
            Boolean deleteF = campaniasAppService.DeleteCuestionarioH2VGById(periodoModel.ProyCodi, User.Identity.Name);

            if (periodoModel.Ch2vgDTOs == null)
            {
                result = 1;
                return Json(result);
            }

            if (deleteF)
            {
                foreach (var detReg in CH2VGdto)
                {

                    CuestionarioH2VGDTO ch2vgdto = detReg;
                    ch2vgdto.H2vGCodi = campaniasAppService.GetLastCuestionarioH2VGId();
                    ch2vgdto.ProyCodi = periodoModel.ProyCodi;
                    ch2vgdto.IndDel = Constantes.IndDel;
                    ch2vgdto.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveCuestionarioH2VG(ch2vgdto);
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarCH2VE(ProyectoModel periodoModel)
        {
            int result = 0;
            List<CuestionarioH2VEDTO> CH2VEdto = periodoModel.Ch2veDTOs;
            Boolean deleteF = campaniasAppService.DeleteCuestionarioH2VEById(periodoModel.ProyCodi, User.Identity.Name);

            if (CH2VEdto == null)
            {
                result = 1;
                return Json(result);
            }

            if (deleteF)
            {
                foreach (var detReg in CH2VEdto)
                {

                    CuestionarioH2VEDTO ch2vEdto = detReg;
                    ch2vEdto.H2veCodi = campaniasAppService.GetLastCuestionarioH2VEId();
                    ch2vEdto.ProyCodi = periodoModel.ProyCodi;
                    ch2vEdto.IndDel = Constantes.IndDel;
                    ch2vEdto.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveCuestionarioH2VE(ch2vEdto);
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }



        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarCroFicha1(ProyectoModel periodoModel)
        {
            int result = 0;
            CroFicha1DTO CroFicha1dto = periodoModel.CroFicha1DTO;
            CroFicha1dto.CroFicha1codi = campaniasAppService.GetLastCroFicha1Id();
            CroFicha1dto.Proycodi = periodoModel.ProyCodi;
            CroFicha1dto.IndDel = Constantes.IndDel;
            CroFicha1dto.Usucreacion = User.Identity.Name;
            Boolean deleteF = campaniasAppService.DeleteCroFicha1ById(CroFicha1dto.Proycodi, User.Identity.Name);
            if (deleteF)
            {
                if (campaniasAppService.SaveCroFicha1(CroFicha1dto))
                {
                    campaniasAppService.DeleteCroFicha1DetById(CroFicha1dto.Proycodi, User.Identity.Name);
                    if (periodoModel.ListaCroFicha1DetDTO != null)
                    {
                        foreach (var item in periodoModel.ListaCroFicha1DetDTO)
                        {
                            CroFicha1DetDTO CroFicha1DetDTO = item;
                            CroFicha1DetDTO.CroFicha1Detcodi = campaniasAppService.GetLastCroFicha1DetId();
                            CroFicha1DetDTO.CroFicha1codi = CroFicha1dto.CroFicha1codi;
                            CroFicha1DetDTO.IndDel = Constantes.IndDel;
                            CroFicha1DetDTO.Usucreacion = User.Identity.Name;
                            campaniasAppService.SaveCroFicha1Det(CroFicha1DetDTO);
                        }
                    }
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }
        public ActionResult CopiarArchivos(string carpetaOrigen, string carpetaDestino)
        {
            try
            {
                // Construir las rutas basadas en el servicio
                string rutaOrigen = campaniasAppService.ObtenerPathArchivosCampianas() + carpetaOrigen;
                string rutaDestino = campaniasAppService.ObtenerPathArchivosCampianas() + carpetaDestino;

                // Validaciones de las rutas
                if (string.IsNullOrEmpty(rutaOrigen) || string.IsNullOrEmpty(rutaDestino))
                {
                    return Json(new { success = false, message = "Las rutas no pueden estar vacías." }, JsonRequestBehavior.AllowGet);
                }

                if (!System.IO.Directory.Exists(rutaOrigen))
                {
                    return Json(new { success = false, message = "La carpeta de origen no existe." }, JsonRequestBehavior.AllowGet);
                }

                // Crear la carpeta de destino si no existe
                if (!System.IO.Directory.Exists(rutaDestino))
                {
                    System.IO.Directory.CreateDirectory(rutaDestino);
                }

                // Copiar solo los archivos de la carpeta origen a la carpeta destino
                foreach (string archivo in System.IO.Directory.GetFiles(rutaOrigen))
                {
                    string nombreArchivo = Path.GetFileName(archivo);
                    string destinoArchivo = Path.Combine(rutaDestino, nombreArchivo);
                    System.IO.File.Copy(archivo, destinoArchivo, true); // Sobrescribe si ya existe
                }

                return Json(new { success = true, message = "Archivos copiados exitosamente.", rutaDestino }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult EnviarCorreoNotificacion(ProyectoModel periodoModel)
        {
            int result = 0;
            try
            {
                string planestado = Constantes.EstadoEnviado;
                string correo = base.UserEmail;
                if (campaniasAppService.UpdatePlanEstadoEnviarById(periodoModel.CodPlanTransmision, planestado, correo))
                {
                    if (campaniasAppService.UpdateProyEstadoById(periodoModel.CodPlanTransmision, planestado))
                    {
                //obtenerversion
                 PlanTransmisionDTO dataplan=campaniasAppService.GetPlanTransmisionById(periodoModel.CodPlanTransmision);
                //ListaProyectos Migrados
                 List<TransmisionProyectoDTO> listaProyecto = campaniasAppService.GetTransmisionProyecto(periodoModel.CodPlanTransmision);
                    foreach (var listaproy in listaProyecto)
                    {
                        // Obtener la lista de archivos asociados a cada proyecto
                        List<ArchivoInfoDTO> listaArchivoDTO = campaniasAppService.GetArchivoInfoByProyCodi(listaproy.Proycodi);
                        if (listaArchivoDTO.Count == 0)
                        {
                            Console.WriteLine($"No hay archivos para el proyecto {listaproy.Proycodi}. Se omite la operación.");
                            continue; // Saltar a la siguiente iteración del bucle
                        }
                        //Registro a copiar
                        string origen =listaArchivoDTO[0].ArchUbicacion;
                        // Separar la ruta en segmentos
                        string[] partesRuta = origen.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                        if (partesRuta.Length > 0)
                        {
                            // Reemplazar la última parte con "3"
                            partesRuta[partesRuta.Length - 1] = dataplan.Planversion.ToString();

                            // Unir nuevamente la ruta con el separador adecuado
                            string nuevaUbicacion = Path.Combine(partesRuta);
                            campaniasAppService.UpdateArchivoInfoByProyCodi(listaproy.Proycodi, nuevaUbicacion);
                            CopiarArchivos(origen, nuevaUbicacion);

                        }
                    }
                        EnvioDto envioDto = new EnvioDto();
                        envioDto.PlanTransmision = campaniasAppService.GetPlanTransmisionById(periodoModel.CodPlanTransmision);
                        envioDto.ListaTrsmProyecto = campaniasAppService.GetTransmisionProyecto(periodoModel.CodPlanTransmision);
                        envioDto.Correos = correo;
                        envioDto.Comentarios = periodoModel.Comentarios;
                        int plantillaCorreo = 321;
                        campaniasAppService.EnviarCorreoNotificacion(envioDto, plantillaCorreo);
                        result = 1;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                result = -1;
            }
            return Json(result);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarLineasFichaB(ProyectoModel periodoModel)
        {
            int result = 0;
            LineasFichaBDTO lineasFichaBDTO = periodoModel.LineasFichaBDTO;
            lineasFichaBDTO.FichaBCodi = campaniasAppService.GetLastLineasFichaBId();
            lineasFichaBDTO.ProyCodi = periodoModel.ProyCodi;
            lineasFichaBDTO.IndDel = Constantes.IndDel;
            lineasFichaBDTO.UsuCreacion = User.Identity.Name;
            Boolean deleteF = campaniasAppService.DeleteLineasFichaBById(lineasFichaBDTO.ProyCodi, User.Identity.Name);
            Boolean deleteFList = campaniasAppService.DeleteLineasFichaBDetById(lineasFichaBDTO.ProyCodi, User.Identity.Name);
            if (deleteF && deleteFList)
            {
                campaniasAppService.SaveLineasFichaB(lineasFichaBDTO);
                if(periodoModel.ListLineasFichaBDetDTO != null){
                    foreach (var item in periodoModel.ListLineasFichaBDetDTO)
                    {
                        LineasFichaBDetDTO lineasFichaBDetDTO = item;
                        lineasFichaBDetDTO.FichaBDetCodi = campaniasAppService.GetLastLineasFichaBDetId();
                        lineasFichaBDetDTO.FichaBCodi = lineasFichaBDTO.FichaBCodi;
                        lineasFichaBDetDTO.IndDel = Constantes.IndDel;
                        lineasFichaBDetDTO.UsuCreacion = User.Identity.Name;
                        campaniasAppService.SaveLineasFichaBDet(lineasFichaBDetDTO);
                    }
                }

                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarG7LineasFichaA(ProyectoModel periodoModel)
        {
            int result = 0;
            LineasFichaADTO lineasFicha1DTO = periodoModel.lineasFichaADTO;
            lineasFicha1DTO.LinFichaACodi = campaniasAppService.GetLastLineasFichaAId();
            lineasFicha1DTO.ProyCodi = periodoModel.ProyCodi;
            lineasFicha1DTO.IndDel = Constantes.IndDel;
            lineasFicha1DTO.UsuCreacion = User.Identity.Name;
            campaniasAppService.DeleteLineasFichaAById(lineasFicha1DTO.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteLineasFichaADet1ById(lineasFicha1DTO.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteLineasFichaADet2ById(lineasFicha1DTO.ProyCodi, User.Identity.Name);
            campaniasAppService.SaveLineasFichaA(lineasFicha1DTO);

            if (periodoModel.listaLineasFichaADet1DTO != null)
            {
                foreach (var detReg in periodoModel.listaLineasFichaADet1DTO)
                {
                    LineasFichaADet1DTO lineasFichaADet1DTO = detReg;
                    lineasFichaADet1DTO.LinFichaADet1Codi = campaniasAppService.GetLastLineasFichaADet1Id();
                    lineasFichaADet1DTO.LinFichaACodi = lineasFicha1DTO.LinFichaACodi;
                    lineasFichaADet1DTO.IndDel = Constantes.IndDel;
                    lineasFichaADet1DTO.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveLineasFichaADet1(lineasFichaADet1DTO);
                }
            }

            if (periodoModel.listaLineasFichaADet2DTO != null)
            {
                foreach (var detReg in periodoModel.listaLineasFichaADet2DTO)
                {
                    LineasFichaADet2DTO lineasFicha1Det2DTO = detReg;
                    lineasFicha1Det2DTO.LinFichaADet2Codi = campaniasAppService.GetLastLineasFichaADet2Id();
                    lineasFicha1Det2DTO.LinFichaACodi = lineasFicha1DTO.LinFichaACodi;
                    lineasFicha1Det2DTO.IndDel = Constantes.IndDel;
                    lineasFicha1Det2DTO.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveLineasFichaADet2(lineasFicha1Det2DTO);
                }
            }

            result = 1;
            return Json(result);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarT2T2SubestFicha1(ProyectoModel periodoModel)
        {
            int result = 0;
            T2SubestFicha1DTO subestFicha1DTO = periodoModel.t2SubestFicha1DTO;
            subestFicha1DTO.SubestFicha1Codi = campaniasAppService.GetLastT2SubestFicha1Id();
            subestFicha1DTO.ProyCodi = periodoModel.ProyCodi;
            subestFicha1DTO.IndDel = Constantes.IndDel;
            subestFicha1DTO.UsuCreacion = User.Identity.Name;
            campaniasAppService.DeleteT2SubestFicha1ById(subestFicha1DTO.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteT2SubestFicha1Det1ById(subestFicha1DTO.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteT2SubestFicha1Det2ById(subestFicha1DTO.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteT2SubestFicha1Det3ById(subestFicha1DTO.ProyCodi, User.Identity.Name);
            campaniasAppService.SaveT2SubestFicha1(subestFicha1DTO);

            if (periodoModel.listaT2SubestFicha1Det1DTO != null) {
                foreach (var detReg in periodoModel.listaT2SubestFicha1Det1DTO)
                {
                    T2SubestFicha1Det1DTO subestFicha1Det1DTO = detReg;
                    subestFicha1Det1DTO.SubestFicha1Det1Codi = campaniasAppService.GetLastT2SubestFicha1Det1Id();
                    subestFicha1Det1DTO.SubestFicha1Codi = subestFicha1DTO.SubestFicha1Codi;
                    subestFicha1Det1DTO.IndDel = Constantes.IndDel;
                    subestFicha1Det1DTO.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveT2SubestFicha1Det1(subestFicha1Det1DTO);
                }
            }
            if (periodoModel.listaT2SubestFicha1Det2DTO != null)
            {
                foreach (var detReg in periodoModel.listaT2SubestFicha1Det2DTO)
                {
                    T2SubestFicha1Det2DTO subestFicha1Det2DTO = detReg;
                    subestFicha1Det2DTO.SubestFicha1Det2Codi = campaniasAppService.GetLastT2SubestFicha1Det2Id();
                    subestFicha1Det2DTO.SubestFicha1Codi = subestFicha1DTO.SubestFicha1Codi;
                    subestFicha1Det2DTO.IndDel = Constantes.IndDel;
                    subestFicha1Det2DTO.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveT2SubestFicha1Det2(subestFicha1Det2DTO);
                }
            }
            if (periodoModel.listaT2SubestFicha1Det3DTO != null) {
                foreach (var detReg in periodoModel.listaT2SubestFicha1Det3DTO)
                {
                    T2SubestFicha1Det3DTO subestFicha1Det3DTO = detReg;
                    subestFicha1Det3DTO.SubestFicha1Det3Codi = campaniasAppService.GetLastT2SubestFicha1Det3Id();
                    subestFicha1Det3DTO.SubestFicha1Codi = subestFicha1DTO.SubestFicha1Codi;
                    subestFicha1Det3DTO.IndDel = Constantes.IndDel;
                    subestFicha1Det3DTO.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveT2SubestFicha1Det3(subestFicha1Det3DTO);
                }
            }
            result = 1;
            return Json(result);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarDFormatoD(ProyectoModel periodoModel)
        {
            int result = 0;
            List<FormatoD1DDTO> formatoD1List = periodoModel.listaFormatoDs;
            Boolean deleteF = campaniasAppService.DeleteFormatoD1DById(periodoModel.ProyCodi, User.Identity.Name);
            if (deleteF)
            {
                if (formatoD1List != null)
                {
                    foreach (var detReg in formatoD1List)
                    {

                        FormatoD1DDTO formatoD1 = detReg;
                        formatoD1.FormatoD1DCodi = campaniasAppService.GetLastFormatoD1DId();
                        formatoD1.ProyCodi = periodoModel.ProyCodi;
                        formatoD1.IndDel = Constantes.IndDel;
                        formatoD1.UsuCreacion = User.Identity.Name;
                        campaniasAppService.SaveFormatoD1D(formatoD1);
                    }
                }
                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarDFormatoC(ProyectoModel periodoModel)
        {
            int result = 0;
            FormatoD1CDTO formatoD1CDTO = periodoModel.FormatoD1CDTO;
            formatoD1CDTO.FormatoD1CCodi = campaniasAppService.GetLastFormatoD1CId();
            formatoD1CDTO.ProyCodi = periodoModel.ProyCodi;
            formatoD1CDTO.IndDel = Constantes.IndDel;
            formatoD1CDTO.UsuCreacion = User.Identity.Name;
            campaniasAppService.DeleteFormatoD1CById(formatoD1CDTO.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteFormatoD1CDetById(formatoD1CDTO.ProyCodi, User.Identity.Name);
            campaniasAppService.SaveFormatoD1C(formatoD1CDTO);

            if (periodoModel.ListaFormatoDCDet1 != null)
            {
                foreach (var detReg in periodoModel.ListaFormatoDCDet1)
                {
                    FormatoD1CDetDTO formatoD1CDetDTO = detReg;
                    formatoD1CDetDTO.FormatoD1CDetCodi = campaniasAppService.GetLastFormatoD1CDetId();
                    formatoD1CDetDTO.FormatoD1CCodi = formatoD1CDTO.FormatoD1CCodi;
                    formatoD1CDetDTO.IndDel = Constantes.IndDel;
                    formatoD1CDetDTO.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveFormatoD1CDet(formatoD1CDetDTO);
                }
            }
            result = 1;
            return Json(result);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarDFormatoB(ProyectoModel periodoModel)
        {
            int result = 0;
            FormatoD1BDTO formatoD1BDTO = periodoModel.FormatoD1BDTO;
            formatoD1BDTO.FormatoD1BCodi = campaniasAppService.GetLastFormatoD1BId();
            formatoD1BDTO.ProyCodi = periodoModel.ProyCodi;
            formatoD1BDTO.IndDel = Constantes.IndDel;
            formatoD1BDTO.UsuCreacion = User.Identity.Name;
            campaniasAppService.DeleteFormatoD1BById(formatoD1BDTO.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteFormatoD1BDetById(formatoD1BDTO.ProyCodi, User.Identity.Name);
            campaniasAppService.SaveFormatoD1B(formatoD1BDTO);

            if (periodoModel.ListaFormatoDet1B != null)
            {
                foreach (var detReg in periodoModel.ListaFormatoDet1B)
                {
                    FormatoD1BDetDTO formatoD1BDetDTO = detReg;
                    formatoD1BDetDTO.FormatoD1BDetCodi = campaniasAppService.GetLastFormatoD1BDetId();
                    formatoD1BDetDTO.FormatoD1BCodi = formatoD1BDTO.FormatoD1BCodi;
                    formatoD1BDetDTO.IndDel = Constantes.IndDel;
                    formatoD1BDetDTO.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveFormatoD1BDet(formatoD1BDetDTO);
                }
            }
            result = 1;
            return Json(result);
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarDFormatoA(ProyectoModel periodoModel)
        {
            int result = 0;
            FormatoD1ADTO formatoD1ADTO = periodoModel.FormatoD1ADTO;
            formatoD1ADTO.FormatoD1ACodi = campaniasAppService.GetLastFormatoD1AId();
            formatoD1ADTO.ProyCodi = periodoModel.ProyCodi;
            formatoD1ADTO.IndDel = Constantes.IndDel;
            formatoD1ADTO.UsuCreacion = User.Identity.Name;
            campaniasAppService.DeleteFormatoD1AById(formatoD1ADTO.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteFormatoD1ADET1ById(formatoD1ADTO.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteFormatoD1ADET2ById(formatoD1ADTO.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteFormatoD1ADET3ById(formatoD1ADTO.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteFormatoD1ADET4ById(formatoD1ADTO.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteFormatoD1ADET5ById(formatoD1ADTO.ProyCodi, User.Identity.Name);
            campaniasAppService.SaveFormatoD1A(formatoD1ADTO);

            if (periodoModel.ListaFormatoDet1A != null)
            {
                foreach (var detReg in periodoModel.ListaFormatoDet1A)
                {
                    FormatoD1ADet1DTO formatoD1ADetDTO = detReg;
                    formatoD1ADetDTO.FormatoD1ADet1Codi = campaniasAppService.GetLastFormatoD1ADET1Id();
                    formatoD1ADetDTO.FormatoD1ACodi = formatoD1ADTO.FormatoD1ACodi;
                    formatoD1ADetDTO.IndDel = Constantes.IndDel;
                    formatoD1ADetDTO.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveFormatoD1ADET1(formatoD1ADetDTO);
                }
            }
            if (periodoModel.ListaFormatoDet2A != null)
            {
                foreach (var detReg in periodoModel.ListaFormatoDet2A)
                {
                    FormatoD1ADet2DTO formatoD1ADetDTO = detReg;
                    formatoD1ADetDTO.FormatoD1ADet2Codi = campaniasAppService.GetLastFormatoD1ADET2Id();
                    formatoD1ADetDTO.FormatoD1ACodi = formatoD1ADTO.FormatoD1ACodi;
                    formatoD1ADetDTO.IndDel = Constantes.IndDel;
                    formatoD1ADetDTO.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveFormatoD1ADET2(formatoD1ADetDTO);
                }
            }
            if (periodoModel.ListaFormatoDet3A != null)
            {
                foreach (var detReg in periodoModel.ListaFormatoDet3A)
                {
                    FormatoD1ADet3DTO formatoD1ADetDTO = detReg;
                    formatoD1ADetDTO.FormatoD1ADet3Codi = campaniasAppService.GetLastFormatoD1ADET3Id();
                    formatoD1ADetDTO.FormatoD1ACodi = formatoD1ADTO.FormatoD1ACodi;
                    formatoD1ADetDTO.IndDel = Constantes.IndDel;
                    formatoD1ADetDTO.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveFormatoD1ADET3(formatoD1ADetDTO);
                }
            }
            if (periodoModel.ListaFormatoDet4A != null)
            {
                foreach (var detReg in periodoModel.ListaFormatoDet4A)
                {
                    FormatoD1ADet4DTO formatoD1ADetDTO = detReg;
                    formatoD1ADetDTO.FormatoD1ADet4Codi = campaniasAppService.GetLastFormatoD1ADET4Id();
                    formatoD1ADetDTO.FormatoD1ACodi = formatoD1ADTO.FormatoD1ACodi;
                    formatoD1ADetDTO.IndDel = Constantes.IndDel;
                    formatoD1ADetDTO.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveFormatoD1ADET4(formatoD1ADetDTO);
                }
            }
            if (periodoModel.ListaFormatoDet5A != null)
            {
                foreach (var detReg in periodoModel.ListaFormatoDet5A)
                {
                    FormatoD1ADet5DTO formatoD1ADetDTO = detReg;
                    formatoD1ADetDTO.FormatoD1ADet5Codi = campaniasAppService.GetLastFormatoD1ADET5Id();
                    formatoD1ADetDTO.FormatoD1ACodi = formatoD1ADTO.FormatoD1ACodi;
                    formatoD1ADetDTO.IndDel = Constantes.IndDel;
                    formatoD1ADetDTO.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveFormatoD1ADET5(formatoD1ADetDTO);
                }
            }
            result = 1;
            return Json(result);
        }



        [System.Web.Mvc.HttpGet]
        public virtual ActionResult DescargarArchivoCampania(string nombre)
        {
            string path = campaniasAppService.ObtenerPathArchivosCampianas();
            ArchivoInfoDTO archivoInfoDTO = campaniasAppService.GetArchivoInfoNombreGenerado(nombre);
            string fileNameInicial = archivoInfoDTO.ArchNombreGenerado;
            string fileName = archivoInfoDTO.ArchNombre;
            string ubi_detalle = archivoInfoDTO.ArchUbicacion+"\\";
            Stream stream = campaniasAppService.DownloadToStream(path+ ubi_detalle + fileNameInicial);
            return File(stream, archivoInfoDTO.ArchTipo, fileName);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult ListarSubestacion()
        {
            List<DataSubestacionDTO> subestacionDTOs = new List<DataSubestacionDTO>();
            subestacionDTOs = campaniasAppService.ListParamSubestacion();
            return Json(subestacionDTOs);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarT1LineasFichaA(ProyectoModel periodoModel)
        {
            int result = 0;
            T1LinFichaADTO lineasFicha1DTO = periodoModel.t1lineasFichaADTO;
            lineasFicha1DTO.LinFichaACodi = campaniasAppService.GetLastT1LineasFichaAId();
            lineasFicha1DTO.ProyCodi = periodoModel.ProyCodi;
            lineasFicha1DTO.IndDel = Constantes.IndDel;
            lineasFicha1DTO.UsuCreacion = User.Identity.Name;
            campaniasAppService.DeleteT1LineasFichaAById(lineasFicha1DTO.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteLineasT1FichaADet1ById(lineasFicha1DTO.ProyCodi, User.Identity.Name);
            campaniasAppService.DeleteT1LineasFichaADet2ById(lineasFicha1DTO.ProyCodi, User.Identity.Name);
            campaniasAppService.SaveT1LineasFichaA(lineasFicha1DTO);

            if (periodoModel.listaT1LineasFichaADet1DTO != null)
            {
                foreach (var detReg in periodoModel.listaT1LineasFichaADet1DTO)
                {
                    T1LinFichaADet1DTO lineasFichaADet1DTO = detReg;
                    lineasFichaADet1DTO.LinFichaADet1Codi = campaniasAppService.GetLastT1LineasFichaADet1Id();
                    lineasFichaADet1DTO.LinFichaACodi = lineasFicha1DTO.LinFichaACodi;
                    lineasFichaADet1DTO.IndDel = Constantes.IndDel;
                    lineasFichaADet1DTO.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveT1LineasFichaADet1(lineasFichaADet1DTO);
                }
            }

            if (periodoModel.listaT1LineasFichaADet2DTO != null)
            {
                foreach (var detReg in periodoModel.listaT1LineasFichaADet2DTO)
                {
                    T1LinFichaADet2DTO lineasFicha1Det2DTO = detReg;
                    lineasFicha1Det2DTO.LinFichaADet2Codi = campaniasAppService.GetLastT1LineasFichaADet2Id();
                    lineasFicha1Det2DTO.LinFichaACodi = lineasFicha1DTO.LinFichaACodi;
                    lineasFicha1Det2DTO.IndDel = Constantes.IndDel;
                    lineasFicha1Det2DTO.UsuCreacion = User.Identity.Name;
                    campaniasAppService.SaveT1LineasFichaADet2(lineasFicha1Det2DTO);
                }
            }

            result = 1;
            return Json(result);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult EnviarCorreoNotificacionAbsolucion(ProyectoModel periodoModel)
        {
            int result = 0;
            try
            {
                string planestado = Constantes.EstadoAbsuelto;
                string planestadoN = Constantes.EstadoEnviado;
                string correo = base.UserEmail;
                campaniasAppService.UpdatePlanEstadoById(periodoModel.CodPlanTransmision, planestado);
                if (campaniasAppService.UpdatePlanEstadoEnviarById(periodoModel.CodPlanTransmisionN, planestadoN, correo))
                {
                    if (campaniasAppService.UpdateProyEstadoById(periodoModel.CodPlanTransmisionN, planestadoN))
                    {
                        if (campaniasAppService.DesactivatePlanById(periodoModel.CodPlanTransmisionN))
                        {
                            if (campaniasAppService.ActivatePlanById(periodoModel.CodPlanTransmisionN))
                            {
                                EnvioDto envioDto = new EnvioDto();
                                envioDto.PlanTransmision = campaniasAppService.GetPlanTransmisionById(periodoModel.CodPlanTransmisionN);
                                envioDto.Correos = correo;
                                int plantillaCorreo = 326;
                                // Notificacion de absolucion
                                campaniasAppService.EnviarCorreoNotificacionObservacion(envioDto, plantillaCorreo, false);
                                result = 1;
                            }
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                result = -1;
            }
            return Json(result);
        }

        [System.Web.Mvc.HttpGet]
        public virtual ActionResult DescargarArchivoCampaniaObservacion(string nombre)
        {
            string path = campaniasAppService.ObtenerPathArchivosCampianas();
            ArchivoObsDTO archivoObsDTO = campaniasAppService.GetArchivoObsNombreArchivo(nombre);
            string fileNameInicial = archivoObsDTO.NombreArchGen;
            string fileName = archivoObsDTO.NombreArch;
            string ubi_detalle = string.IsNullOrEmpty(archivoObsDTO.RutaArch) ? "" : archivoObsDTO.RutaArch + "\\";
            Stream stream = campaniasAppService.DownloadToStream(path+ ubi_detalle + fileNameInicial);
            return File(stream, archivoObsDTO.Tipo, fileName);
        }

    }

}
