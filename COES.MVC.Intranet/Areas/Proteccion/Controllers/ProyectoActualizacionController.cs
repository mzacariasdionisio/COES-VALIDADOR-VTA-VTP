using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Areas.Proteccion.Models;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.Transferencias;
using log4net;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Areas.Proteccion.Helper;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Controllers;


namespace COES.MVC.Intranet.Areas.Proteccion.Controllers
{
    public class ProyectoActualizacionController : BaseController
    {
        /// <summary>
        /// Instancia de clase para el acceso a datos
        /// </summary>
        
        EquipamientoAppServicio servicioEquipamiento = new EquipamientoAppServicio();
        AreaAppServicio servicioArea = new AreaAppServicio();
        ProyectoActualizacionAppServicio servicioProyectoActualizacion = new ProyectoActualizacionAppServicio();
        ConsultaMedidoresAppServicio consultaMedidores = new ConsultaMedidoresAppServicio();
        MigracionesAppServicio migraciones = new MigracionesAppServicio();
        private readonly List<EstadoModel> _lsEstadosFlag = new List<EstadoModel>();
        FichaTecnicaAppServicio servicioFT = new FichaTecnicaAppServicio();
        EquipoProteccionAppServicio equipoProteccion = new EquipoProteccionAppServicio();

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ProyectoActualizacionController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public ProyectoActualizacionController()
        {
            log4net.Config.XmlConfigurator.Configure();
            _lsEstadosFlag.Add(new EstadoModel { EstadoCodigo = "S", EstadoDescripcion = "Si" });
            _lsEstadosFlag.Add(new EstadoModel { EstadoCodigo = "N", EstadoDescripcion = "No" });
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            ProyectoActualizacionModel modelo = new ProyectoActualizacionModel();
            modelo.ListaArea = migraciones.ListSiAreasSGOCOES();
            modelo.listaTitular = consultaMedidores.ListObtenerEmpresaSEINProtecciones();

            List<SiTipoempresaDTO> listaTipoEmpresa = servicioFT.ListarTipoEmpresas();
            listaTipoEmpresa.Sort((s1, s2) => s1.Tipoemprdesc.CompareTo(s2.Tipoemprdesc));
            modelo.listaTipoEmpresa = listaTipoEmpresa;

            modelo.ListaEstadoFlag = _lsEstadosFlag;

            List<EprPropCatalogoDataDTO> listaEstado = equipoProteccion.ListPropCatalogoData(1);
            modelo.listaEstado = listaEstado;

            return View(modelo);
        }

        [HttpPost]
        public JsonResult Editar(int id)
        {
            ProyectoActualizacionEditarModel model = new ProyectoActualizacionEditarModel();

            try
            {
                if (id> 0)
                {
                var dto = servicioProyectoActualizacion.EprProyectoGetById(id);
                model.CodigoNemoTecnico = dto.Epproynemotecnico;
                model.Nombre = dto.Epproynomb;
                model.Descripcion = dto.Epproydescripcion;
                model.FechaRegistro = dto.Epproyfecregistro;
                model.idArea = dto.Epproysgcodi??0;
                model.idFamilia = dto.Emprcodi ?? 0;
                model.Codigo = dto.Epproycodi;
                model.Eppproyflgtieneeo = dto.Eppproyflgtieneeo;

                }

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        public PartialViewResult ConsultaEqupamientoModificado(int id)
        {
            EquipamientoModificadoModel model = new EquipamientoModificadoModel();
            try
            {
                EprProyectoActEqpDTO proyecto = new EprProyectoActEqpDTO();
                proyecto = servicioProyectoActualizacion.EprProyectoGetById(id);
                if (proyecto != null) {
                    model.Codigo = proyecto.Epproycodi.ToString();
                    model.MemoTecnico = proyecto.Epproynemotecnico;
                    model.Motivo = proyecto.Epproynomb;
                }      
                
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
            }

            return PartialView("~/Areas/Proteccion/Views/ProyectoActualizacion/ConsultaEquipamientoModificado.cshtml", model);
        }

        [ActionName("Index"), HttpPost]
        public ActionResult IndexPost(UbicacionCOESModel datos)
        {
            return View(datos);
        }

        [HttpPost]
        public PartialViewResult ListaProyectos(int iArea, string sNombre, string sFechaInicio, string sFechaFin)
        {
            ListadoProyectoModel model = new ListadoProyectoModel();
            model.ListaProyecto = servicioProyectoActualizacion.EprProyectoList(iArea, sNombre, sFechaInicio, sFechaFin).ToList();
            return PartialView("~/Areas/Proteccion/Views/ProyectoActualizacion/ListaProyectos.cshtml", model);
        }

        [HttpPost]
        public PartialViewResult ListaEqModificado(int id)
        {
            ListadoEqModificadoModel model = new ListadoEqModificadoModel();

            List<EprEquipoDTO> listaModificada = new List<EprEquipoDTO>();

            var lista = servicioProyectoActualizacion.ListEquipamientoModificado(id).ToList();

            foreach (var item in lista) {
                item.MemoriaCalculoTexo = ProteccionHelper.modificarNombreArchivo(item.MemoriaCalculo);
                listaModificada.Add(item);

            }
            model.ListaEqModificado = listaModificada;

            return PartialView("~/Areas/Proteccion/Views/ProyectoActualizacion/ListaEqModificado.cshtml", model);
        }

        public JsonResult GuardarProyectoActualizacion(int iArea, int iTitular, string sEstado, string sCodigoNemoTecnico, string sNombre, string sDescripcion, string sFecha, int iCodigo)
        {


            try
            {
                int existeCodigo = 0;
                ListadoProyectoModel model = new ListadoProyectoModel();
                
                EprProyectoActEqpDTO oProyectoActualizacion = null;
                var dto = servicioProyectoActualizacion.EprProyectoGetById(iCodigo);

                List<EprProyectoActEqpDTO> lProyectoActualizacion = servicioProyectoActualizacion.EprProyectoList(0, "", "", "").ToList();

                if (dto == null)
                {
                    foreach (var item in lProyectoActualizacion)
                    {
                        if (item.Epproynemotecnico.Equals(sCodigoNemoTecnico))
                        {
                            existeCodigo++;
                        }
                    }
                }
                else
                {
                    foreach (var item in lProyectoActualizacion)
                    {
                        
                        if ((item.Epproycodi != iCodigo) && item.Epproynemotecnico.Equals(sCodigoNemoTecnico))
                        {
                            existeCodigo++;
                        }
                    }
                }


                if (existeCodigo > 0)
                {
                    return Json(2);
                }

                if (dto == null) {
                    oProyectoActualizacion = new EprProyectoActEqpDTO();
                    oProyectoActualizacion.Epproysgcodi = iArea;
                    oProyectoActualizacion.Emprcodi = iTitular;
                    oProyectoActualizacion.Eppproyflgtieneeo = sEstado;
                    oProyectoActualizacion.Epproynomb = sNombre;
                    oProyectoActualizacion.Epproydescripcion = sDescripcion;
                    oProyectoActualizacion.Epproyfecregistro = sFecha;
                    oProyectoActualizacion.Epproynemotecnico = sCodigoNemoTecnico;
                    oProyectoActualizacion.Epproyestregistro = "1";
                    oProyectoActualizacion.Epproyusucreacion = User.Identity.Name;
                    servicioProyectoActualizacion.EprProyectoSave(oProyectoActualizacion);
                }
                else{
                    dto.Epproysgcodi = iArea;
                    dto.Emprcodi = iTitular;
                    dto.Eppproyflgtieneeo = sEstado;
                    dto.Epproynomb = sNombre;
                    dto.Epproydescripcion = sDescripcion;
                    dto.Epproyfecregistro = sFecha;
                    dto.Epproynemotecnico = sCodigoNemoTecnico;
                    dto.Epproyestregistro = "1";
                    dto.Eppproyusumodificacion = User.Identity.Name;
                    servicioProyectoActualizacion.EprProyectoUpdate(dto);
                }
                return Json(1);
            }
            catch (Exception e)
            {
                return Json(-1);
            }
        }

        public JsonResult EliminarProyectoActualizacion(int iCodigo)
        {

            EprProyectoActEqpDTO model = new EprProyectoActEqpDTO();
            try
            {
                EprProyectoActEqpDTO validar = servicioProyectoActualizacion.ValidarProyectoActualizacionPorRele(iCodigo);

                if (validar.NroEquipo == 0 && validar.NroPropiedades == 0)
                {
                    EprProyectoActEqpDTO oProyectoActualizacion = null;

                    oProyectoActualizacion = new EprProyectoActEqpDTO();
                    oProyectoActualizacion.Epproycodi = iCodigo;
                    oProyectoActualizacion.Epproyestregistro = "0";
                    oProyectoActualizacion.Eppproyusumodificacion = User.Identity.Name;
                    servicioProyectoActualizacion.EprProyectoDelete_UpdateAuditoria(oProyectoActualizacion);

                    model.Resultado = "1";
                    model.Mensaje = "El registro fue eliminado";

                } else
                {
                    model.Resultado = "2";
                    model.Mensaje = "No es posible eliminar el proyecto, dado que existen "+ validar.NroPropiedades + " propiedad(es) pertenecientes a "+ validar.NroEquipo + " equipo(s) asociados al mismo";
                }
                
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult CargarEmpresasXTipo(int tipoEmpresa)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {
                model.ListaEmpresas = servicioFT.ObtenerEmpresasPorTipo(tipoEmpresa);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult ListarEstudiosEo(int idEmpresa)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {

                model.ListadoEstudiosEo = servicioFT.ListarEstudiosEo(idEmpresa);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult ObtenerDatoEstudioEO(int esteocodi)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {

                model.EstudioEO = servicioFT.ObtenerEstudioEO(esteocodi);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult GenerarReporteRele(int epproycodi, string memotecnico)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {
                DateTime hoy = DateTime.Now;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string nameFile = "Reporte_Rele_Proy_" + hoy.Year + string.Format("{0:D2}", hoy.Month) + string.Format("{0:D2}", hoy.Day) + string.Format("{0:D2}", hoy.Hour) + string.Format("{0:D2}", hoy.Minute) + string.Format("{0:D2}", hoy.Second) + ".xlsx";

                List<EprEquipoDTO> lExportar = equipoProteccion.ReporteEquipoProtGrillaProyecto(epproycodi).ToList();

                new ProteccionHelper().GenerarExportacionProyecto(pathLogo, nameFile, lExportar, base.PathFiles, "LISTADO DETALLADO DE RELÉS POR PROYECTO " + memotecnico);
                model.Resultado = nameFile;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpGet]
        public virtual FileResult ExportarReporte()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = FileServer.GetDirectory() + base.PathFiles + "/" + ConstantesProteccion.FolderReporte + "/" + nombreArchivo;
            byte[] buffer = null;

            if (System.IO.File.Exists(ruta))
            {
                buffer = System.IO.File.ReadAllBytes(ruta);
                System.IO.File.Delete(ruta);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo);
        }

        /// <summary>
        /// Descarga los archivos adjuntados
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="epsubecodi"></param>
        /// <returns></returns>
        public virtual ActionResult DescargarArchivo(string fileName)
        {
            try
            {
                base.ValidarSesionUsuario();
                byte[] buffer = new ProteccionHelper().GetBufferArchivoAdjunto(0, fileName, base.PathFiles, ConstantesProteccion.FolderRele);
                return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, ProteccionHelper.modificarNombreArchivo(fileName));
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                return null;
            }
        }
    }
}
