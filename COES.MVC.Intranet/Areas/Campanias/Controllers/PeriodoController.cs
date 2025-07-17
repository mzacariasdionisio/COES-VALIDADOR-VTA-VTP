using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using COES.Dominio.DTO.Campania;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Campanias.Helper;
using COES.MVC.Intranet.Areas.Campanias.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CalculoResarcimientos;
using COES.Servicios.Aplicacion.Campanias;
using COES.Servicios.Aplicacion.CPPA.Helper;
using iTextSharp.text;
using log4net;

namespace COES.MVC.Intranet.Areas.Campanias.Controllers
{
    public class PeriodoController : BaseController
    {
        private readonly CampaniasAppService _campaniasAppService;
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(PeriodoController));

        public PeriodoController()
        {
            _campaniasAppService = new CampaniasAppService();
        }

        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            PeriodoModel periodoModel = new PeriodoModel();
            return View(periodoModel);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Buscar(int anio, string estado)
        {
            
            PeriodoModel periodoModel = new PeriodoModel();
            if (estado.Equals(Constantes.Todos))
            {
                periodoModel.ListaPeriodos = _campaniasAppService.ListPeriodosByAnio(anio);
            }
            else
            {
                periodoModel.ListaPeriodos = _campaniasAppService.ListPeriodosByAnioAndEstado(anio, estado);
            }
            return View(periodoModel);
        }

        public ActionResult Listar()
        {
            PeriodoModel periodoModel = new PeriodoModel();
            periodoModel.ListaPeriodos = _campaniasAppService.ListPeriodos();
            return View(periodoModel);
        }

        /// <summary>
        /// //Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesCampanias.ModuloManualUsuario;
            string nombreArchivo = ConstantesCampanias.ArchivoManualUsuarioIntranet;
            string pathDestino = modulo + ConstantesCampanias.FolderRaizCampaniasModuloManual;
            string pathAlternativo = ConfigurationManager.AppSettings["FileSystemPortal"];

            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);

                    return File(buffer, Constantes.AppPdf, nombreArchivo);
                }
                else
                    throw new ArgumentException("No se pudo descargar el archivo del servidor.");

            }
            catch (Exception ex)
            {
                throw new ArgumentException("ERROR: ", ex);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult Grabar(PeriodoModel periodoModel)
        {
            int result = 0;
            PeriodoDTO periodoDTO = new PeriodoDTO();
            periodoDTO.PeriCodigo = _campaniasAppService.GetPeriodoId();
            periodoDTO.PeriNombre = periodoModel.PeriNombre;
            periodoDTO.PeriFechaInicio = periodoModel.PeriFechaInicio;
            periodoDTO.PeriFechaFin = periodoModel.PeriFechaFin;
            periodoDTO.PeriEstado = periodoModel.PeriEstado;
            periodoDTO.PeriHoraFin = periodoModel.PeriHoraFin;
            periodoDTO.PeriHorizonteAdelante = periodoModel.PeriHorizonteAdelante;
            periodoDTO.PeriHorizonteAtras = periodoModel.PeriHorizonteAtras;
            periodoDTO.PeriComentario = periodoModel.PeriComentario;
            periodoDTO.UsuarioCreacion = User.Identity.Name;
            periodoDTO.IndDel = Constantes.IndDel;
            int periodoDate = _campaniasAppService.GetPeriodoByDate(periodoModel.PeriFechaInicio,periodoModel.PeriFechaFin, periodoDTO.PeriCodigo);
            if(periodoDate == 0){

                if (_campaniasAppService.SavePeriodo(periodoDTO))
                {
                    List<DetallePeriodoDTO> hojaProyectoDTOs = new List<DetallePeriodoDTO>();

                    int lastFichaId = _campaniasAppService.GetFichaId();
                    foreach (var item in periodoModel.PeriHojaProyecto)
                    {
                        DetallePeriodoDTO detallePeriodoDTO = new DetallePeriodoDTO();
                        detallePeriodoDTO.DetPeriCodigo = lastFichaId;
                        detallePeriodoDTO.PeriCodigo = periodoDTO.PeriCodigo;
                        detallePeriodoDTO.HojaCodigo = Int32.Parse(item);
                        detallePeriodoDTO.UsuarioCreacion = User.Identity.Name;
                        detallePeriodoDTO.IndDel = Constantes.IndDel;
                        hojaProyectoDTOs.Add(detallePeriodoDTO);
                        lastFichaId++;
                    }
                    _campaniasAppService.SaveFicha(hojaProyectoDTOs);
                    result = 1;
                }
                else
                {
                    result = 0;
                }
            } else {
                result = 2;
            }
           

            return Json(result);
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult Actualizar(PeriodoModel periodoModel)
        {
            int result = 0;
            PeriodoDTO periodoDTO = new PeriodoDTO();
            periodoDTO.PeriCodigo = periodoModel.PeriCodi;
            periodoDTO.PeriNombre = periodoModel.PeriNombre;
            periodoDTO.PeriFechaInicio = periodoModel.PeriFechaInicio;
            periodoDTO.PeriFechaFin = periodoModel.PeriFechaFin;
            periodoDTO.PeriEstado = periodoModel.PeriEstado;
            periodoDTO.PeriHoraFin = periodoModel.PeriHoraFin;
            periodoDTO.PeriHorizonteAdelante = periodoModel.PeriHorizonteAdelante;
            periodoDTO.PeriHorizonteAtras = periodoModel.PeriHorizonteAtras;
            periodoDTO.PeriComentario = periodoModel.PeriComentario;
            periodoDTO.UsuarioModificacion = User.Identity.Name;

            int periodoDate = _campaniasAppService.GetPeriodoByDate(periodoModel.PeriFechaInicio,periodoModel.PeriFechaFin,periodoModel.PeriCodi);
            if(periodoDate == 0){
            
                if (_campaniasAppService.DeleteFichaById(periodoDTO.PeriCodigo, User.Identity.Name))
                {
                        if (_campaniasAppService.UpdatePeriodo(periodoDTO))
                        {
                            List<DetallePeriodoDTO> detallePeriodoDTOs = new List<DetallePeriodoDTO>();

                            int lastFichaId = _campaniasAppService.GetFichaId();
                            foreach (var item in periodoModel.PeriHojaProyecto)
                            {

                                DetallePeriodoDTO detallePeriodoDTO = new DetallePeriodoDTO();
                                detallePeriodoDTO.DetPeriCodigo = lastFichaId;
                                detallePeriodoDTO.PeriCodigo = periodoDTO.PeriCodigo;
                                detallePeriodoDTO.HojaCodigo = Int32.Parse(item);
                                detallePeriodoDTO.UsuarioCreacion = User.Identity.Name;
                                detallePeriodoDTO.IndDel = Constantes.IndDel;
                                detallePeriodoDTOs.Add(detallePeriodoDTO);
                                lastFichaId++;
                        
                            }
                            _campaniasAppService.SaveFicha(detallePeriodoDTOs);
                            result = 1;
                        }
                        else
                        {
                            result = 0;
                        }
                    }
                
                else
                {
                    result = 0;
                } 
            } else {
                result = 2;
            }
            
            
            return Json(result);
        }

        [System.Web.Mvc.HttpPost]
        public PartialViewResult Editar(int pericodi)
        {
            PeriodoModel periodoModel = new PeriodoModel();
            periodoModel.PeriodoDTO = _campaniasAppService.GetPeriodoDTOById(pericodi);
            periodoModel.ListaTipoProyecto = _campaniasAppService.ListTipoProyecto();
            periodoModel.Disabled = _campaniasAppService.GetTransmisionProyectoByPeriodo(pericodi).Count > 0 ? "disabled" : null;
            return PartialView(periodoModel);
        }


        [System.Web.Mvc.HttpGet]
        public PartialViewResult Registrar()
        {
            PeriodoModel periodoModel = new PeriodoModel();
            periodoModel.PeriCodi = _campaniasAppService.GetPeriodoId();
            periodoModel.ListaTipoProyecto = _campaniasAppService.ListTipoProyecto();
            return PartialView(periodoModel);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult Eliminar(int idPeriodo)
        {
            int result = 0;
            string usuario = User.Identity.Name;
            List<TransmisionProyectoDTO>  listaRegistro = _campaniasAppService.GetTransmisionProyectoByPeriodo(idPeriodo);
            if( listaRegistro.Count > 0){
                result = 2;
            } else {
               if (_campaniasAppService.DeleteFichaById(idPeriodo, usuario))
                {
                    if (_campaniasAppService.DeletePeriodoById(idPeriodo, usuario))
                    {
                        result = 1;
                    }                
                }
                else
                {
                    result=0;
                } 
            }
           
            return Json(result);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult ListarCatalogo(string id)
        {
            List<DataCatalogoDTO> dataCatalogoDTOs = new List<DataCatalogoDTO>();
            dataCatalogoDTOs = _campaniasAppService.ListParametria(Constantes.EstadoPeriodo);
            return Json(dataCatalogoDTOs);
        }
    }
}
