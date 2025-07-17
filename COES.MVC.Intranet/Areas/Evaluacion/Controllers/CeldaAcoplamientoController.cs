using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento;
using log4net;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.Evaluacion.Models;
using COES.Servicios.Aplicacion.Evaluacion;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.Mediciones;

using COES.MVC.Intranet.Areas.Proteccion.Helper;

using COES.MVC.Intranet.Areas.Evaluacion.Helper;
using COES.Framework.Base.Tools;
using EvaluacionHelperCalculos = COES.Servicios.Aplicacion.Evaluacion.Helper;

using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using DevExpress.Data.Filtering.Helpers;

namespace COES.MVC.Intranet.Areas.Evaluacion.Controllers
{
    public class CeldaAcoplamientoController : BaseController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(CeldaAcoplamientoController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        EquipamientoAppServicio servicioEquipamiento = new EquipamientoAppServicio();
        EquipoProteccionAppServicio equipoProteccion = new EquipoProteccionAppServicio();
        ConsultaMedidoresAppServicio consultaMedidores = new ConsultaMedidoresAppServicio();
        MigracionesAppServicio migraciones = new MigracionesAppServicio();
        CeldaAcoplamientoAppServicio servicioCeldaAcoplamiento = new CeldaAcoplamientoAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        CalculosAppServicio calculo = new CalculosAppServicio();

        #region Propiedades

        ProyectoActualizacionAppServicio servicioProyectoActualizacion = new ProyectoActualizacionAppServicio();
        TransversalAppServicio servicioTransversal = new TransversalAppServicio();


        /// <summary>
        /// Ruta y nombre del archivo
        /// </summary>
        public String RutaCompletaArchivo
        {
            get
            {
                return (Session[DatosSesionEvaluacion.RutaCompletaArchivo] != null) ?
                    Session[DatosSesionEvaluacion.RutaCompletaArchivo].ToString() : null;
            }
            set { Session[DatosSesionEvaluacion.RutaCompletaArchivo] = value; }
        }
        #endregion
        public CeldaAcoplamientoController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                log.Fatal(NameController, ex);
                throw;
            }
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            CeldaAcoplamientoModel modelo = new CeldaAcoplamientoModel();

            modelo.ListaUbicacion = equipoProteccion.ListSubEstacion();
            modelo.ListaEmpresa = consultaMedidores.ListObtenerEmpresaSEINProtecciones();
            modelo.ListaArea = servicioEquipamiento.ListarZonasxNivel(5);
            modelo.ListaEstado = equipoProteccion.ListPropCatalogoData(ConstantesProteccion.EstadoLinea);
            
            modelo.equicodi = Session[ConstantesEvaluacion.CA_Equicodi] != null ? Session[ConstantesEvaluacion.CA_Equicodi].ToString() : "";
            modelo.codigo = Session[ConstantesEvaluacion.CA_Codigo] != null ? Session[ConstantesEvaluacion.CA_Codigo].ToString() : "";
            modelo.ubicacion = Session[ConstantesEvaluacion.CA_Ubicacion] != null ? (int)Session[ConstantesEvaluacion.CA_Ubicacion] : 0;
            modelo.empresa = Session[ConstantesEvaluacion.CA_Empresa] != null ? (int)Session[ConstantesEvaluacion.CA_Empresa] : 0;
            modelo.area = Session[ConstantesEvaluacion.CA_Area] != null ? (int)Session[ConstantesEvaluacion.CA_Area] : 0;
            modelo.tension = Session[ConstantesEvaluacion.CA_Tension] != null ? Session[ConstantesEvaluacion.CA_Tension].ToString() : "";
            modelo.estado = Session[ConstantesEvaluacion.CA_Estado] != null ? Session[ConstantesEvaluacion.CA_Estado].ToString() : "";
            modelo.incluirCalcular = Session[ConstantesEvaluacion.CA_IncluirCalcular] != null ? (int)Session[ConstantesEvaluacion.CA_IncluirCalcular] : 0;

            int id = (Session[DatosSesion.SesionIdOpcion] != null) ? Convert.ToInt32(Session[DatosSesion.SesionIdOpcion]) : 0;
            bool permisoNuevo = this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, Acciones.Nuevo, User.Identity.Name);
            bool permisoExportar = this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, Acciones.Exportar, User.Identity.Name);
            bool permisoImportar = this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, Acciones.Importar, User.Identity.Name);

            ViewBag.PermisoExportar = permisoExportar ? "1" : "0";
            ViewBag.PermisoNuevo = permisoNuevo ? "1" : "0";
            ViewBag.PermisoImportar = permisoImportar ? "1" : "0";

            return View(modelo);
        }

        [ActionName("Index"), HttpPost]
        public ActionResult IndexPost(CeldaAcoplamientoModel datos)
        {
            return View(datos);
        }

        [HttpPost]
        public PartialViewResult ListaCeldaAcoplamiento(string codigoId, string codigo, int ubicacion,
            int empresa, int area, string estado, int incluirCalcular, string tension)
        {
            
            Session[ConstantesEvaluacion.CA_Equicodi] = codigoId;
            Session[ConstantesEvaluacion.CA_Codigo] = codigo;
            Session[ConstantesEvaluacion.CA_Ubicacion] = ubicacion;
            Session[ConstantesEvaluacion.CA_Empresa] = empresa;
            Session[ConstantesEvaluacion.CA_Area] = area;
            Session[ConstantesEvaluacion.CA_Tension] = tension;
            Session[ConstantesEvaluacion.CA_Estado] = estado;
            Session[ConstantesEvaluacion.CA_IncluirCalcular] = incluirCalcular;

            ListadoCeldaAcoplamientoModel model = new ListadoCeldaAcoplamientoModel();
            model.ListaCeldaAcoplamiento = servicioCeldaAcoplamiento.ListaCeldaAcoplamiento(codigoId, codigo, ubicacion,
            empresa, area, estado, incluirCalcular, tension).ToList();

            int id = (Session[DatosSesion.SesionIdOpcion] != null) ? Convert.ToInt32(Session[DatosSesion.SesionIdOpcion]) : 0;
            bool permisoEdicion = this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, Acciones.Editar, User.Identity.Name);
            bool permisoEliminar = this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, Acciones.Eliminar, User.Identity.Name);

            ViewBag.PermisoEditar = permisoEdicion ? "1" : "0";
            ViewBag.PermisoEliminar = permisoEliminar ? "1" : "0";

            return PartialView("~/Areas/Evaluacion/Views/CeldaAcoplamiento/ListaCeldaAcoplamiento.cshtml", model);
        }

        public ActionResult BuscarCeldaAcoplamiento()
        {
            var model = new BuscarCeldaAcoplamientoModel();

            model.ListaUbicacion = equipoProteccion.ListSubEstacion();
            model.ListaEmpresa = consultaMedidores.ListObtenerEmpresaSEINProtecciones();
            model.ListaEstado = equipoProteccion.ListPropCatalogoData(6);

            return View("~/Areas/Evaluacion/Views/CeldaAcoplamiento/BuscarCeldaAcoplamiento.cshtml", model);
        }

        [HttpPost]
        public PartialViewResult BuscarCeldaAcoplamientoLista(string equiCodi, string codigo, int ubicacion, int empresa, string estado)
        {
            BuscarCeldaAcoplamientoListaModel model = new BuscarCeldaAcoplamientoListaModel();
            model.ListaCeldaAcoplamiento = servicioCeldaAcoplamiento.ListaBuscarCeldaAcoplamiento(equiCodi, codigo, ubicacion, empresa, estado).ToList();

            return PartialView("~/Areas/Evaluacion/Views/CeldaAcoplamiento/BuscarCeldaAcoplamientoLista.cshtml", model);
        }

        public PartialViewResult Incluir(int id)
        {
            var model = new CeldaAcoplamientoIncluirModel();
            model.listaProyecto = equipoProteccion.ListProyectoProyectoActualizacion(id);
            model.idProyecto = 0;
            return PartialView("~/Areas/Evaluacion/Views/CeldaAcoplamiento/Incluir.cshtml", model);
        }

        public ActionResult IncluirModificar(int idCeldaAcoplamiento, int idProyecto, int idEquipo, string accion)
        {
            var model = new CeldaAcoplamientoIncluirModificarModel();

            model.IdEquipo = idEquipo;
            model.IdProyecto = idProyecto;
            model.Equicodi = idCeldaAcoplamiento.ToString();
            model.Accion = accion;

            EprProyectoActEqpDTO proyecto = servicioProyectoActualizacion.EprProyectoGetById(idProyecto);
            model.MotivoActualizacion = proyecto.Epproynemotecnico + ": " + proyecto.Epproynomb;
            model.FechaActualizacion = proyecto.Epproyfecregistro;

            EprEquipoDTO equipo = servicioTransversal.ObtenerCabeceraEquipoPorId(idEquipo);
            model.Equicodi = equipo.Equicodi.ToString();
            model.Codigo = equipo.Codigo;
            model.Ubicacion = equipo.Ubicacion;
            model.Area = equipo.Area;
            model.Empresa = equipo.Empresa;

            List<EprEquipoDTO> ListaInterruptor = servicioTransversal.ListaInterruptorPorAreacodi(equipo.Areacodigo);
            model.ListaInterruptor = ListaInterruptor;

            EprEquipoDTO celdaAcoplamiento = servicioCeldaAcoplamiento.ObtenerCeldaAcoplamientoPorId(idEquipo);
            if (equipo != null)
            {
                model.PosicionNucleoTc = celdaAcoplamiento.PosicionNucleoTc;
                model.PickUp = celdaAcoplamiento.PickUp;
                model.IdInterruptor = celdaAcoplamiento.IdInterruptor;
                model.InterruptorEmpresa = celdaAcoplamiento.InterruptorEmpresa;
                model.InterruptorTension = celdaAcoplamiento.InterruptorTension;
                model.InterruptorCapacidadA = celdaAcoplamiento.InterruptorCapacidadA;
                model.InterruptorCapacidadAComent = celdaAcoplamiento.InterruptorCapacidadAComent;
                model.InterruptorCapacidadMva = celdaAcoplamiento.InterruptorCapacidadMva;
                model.InterruptorCapacidadMvaComent = celdaAcoplamiento.InterruptorCapacidadMvaComent;
                model.CapacidadTransmisionA = celdaAcoplamiento.CapacidadTransmisionA;
                model.CapacidadTransmisionAComent = celdaAcoplamiento.CapacidadTransmisionAComent;
                model.CapacidadTransmisionMva = celdaAcoplamiento.CapacidadTransmisionMva;
                model.CapacidadTransmisionMvaComent = celdaAcoplamiento.CapacidadTransmisionMvaComent;
                model.FactorLimitanteCalc = celdaAcoplamiento.FactorLimitanteCalc;
                model.FactorLimitanteCalcComent = celdaAcoplamiento.FactorLimitanteCalcComent;
                model.FactorLimitanteFinal = celdaAcoplamiento.FactorLimitanteFinal;
                model.FactorLimitanteFinalComent = celdaAcoplamiento.FactorLimitanteFinalComent;
                model.Observaciones = celdaAcoplamiento.Observaciones;
                model.UsuarioModificacion = celdaAcoplamiento.UsuarioModificacion;
                model.FechaModificacion = celdaAcoplamiento.FechaModificacion;
            }

            return View("~/Areas/Evaluacion/Views/CeldaAcoplamiento/IncluirModificar.cshtml", model);
        }


        public JsonResult SeleccionarInterruptor(int idEquipo, int idInterruptor)
        {
            EprEquipoDTO empresa = null;
            try
            {
                EprEquipoDTO equipo = servicioTransversal.ObtenerCabeceraEquipoPorId(idEquipo);
                List<EprEquipoDTO> ListaInterruptor = servicioTransversal.ListaInterruptorPorAreacodi(equipo.Areacodigo);
                empresa = ListaInterruptor.FirstOrDefault(o => o.Equicodi == idInterruptor);
                return Json(empresa);
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                return Json(empresa);
            }
        }


        [HttpPost]
        public PartialViewResult EditarComentario(string comentario)
        {
            CeldaACoplamientoEditarComentarioModel model = new CeldaACoplamientoEditarComentarioModel();
            model.Comentario = comentario;

            return PartialView("~/Areas/Evaluacion/Views/CeldaAcoplamiento/EditarComentario.cshtml", model);
        }


        public JsonResult ExportarPlantilla()
        {
            CeldaAcoplamientoModel model = new CeldaAcoplamientoModel();

            try
            {
                base.ValidarSesionJsonResult();

                string fileName = ConstantesEvaluacion.NombrePlantillaExcelCeldaAcoplamiento;
                string pathOrigen = ConstantesEvaluacion.FolderGestProtec + "/" + ConstantesEvaluacion.Plantilla;
                string pathDestino = FileServer.GetDirectory() + ConstantesEvaluacion.FolderTemporal + "/";

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);

                servicioCeldaAcoplamiento.GenerarExcelPlantilla(pathDestino, fileName);

                model.Resultado = "1";
                model.NombreArchivo = fileName;
                model.StrMensaje = "";


            }
            catch (Exception ex)
            {
                log.Error("CeldaAcoplamientoController", ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        public virtual FileResult AbrirArchivo(string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");

            byte[] buffer = new EvaluacionHelper().GetBufferArchivoAdjunto(file, base.PathFiles, ConstantesEvaluacion.FolderTemporal);
            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, sFecha + "_" + file);
        }

        [HttpPost]
        public JsonResult LeerExcelSubido()
        {
            var registroObservados = 0;

            try
            {
                List<EprCargaMasivaCeldaAcoplamientoDTO> listaRegistros = null;
                Respuesta matrizValida = new Respuesta();

                var titulos = 6;
                listaRegistros = Evaluacion.Helper.FormatoHelper.LeerExcelCargadoCeldaAcoplamiento(this.RutaCompletaArchivo, titulos, out matrizValida);

                if (matrizValida.Exito && listaRegistros.Count > 0)
                {
                    //Validacion
                    ValidarRegistros(listaRegistros);


                    if (listaRegistros.Any(p => p.Error.Length > 0))
                    {
                        registroObservados = listaRegistros.Count(p => p.Error.Length > 0);
                    }

                    //Si no hay errores se graba
                    if (registroObservados == 0)
                    {
                        foreach (var registro in listaRegistros)
                        {
                            registro.NombreUsuario = User.Identity.Name;

                            var res = servicioCeldaAcoplamiento.SaveCargaMasivaCeldaAcoplamiento(registro);

                            registro.Error = !string.IsNullOrEmpty(res) ? res : "";
                        }

                        if (listaRegistros.Any(p => p.Error.Length > 0))
                        {
                            registroObservados = listaRegistros.Count(p => p.Error.Length > 0);
                        }
                    }
                    else
                    {

                        Evaluacion.Helper.FormatoHelper.BorrarArchivo(this.RutaCompletaArchivo);
                        return Json(new Respuesta
                        {
                            Exito = false,
                            RegistrosObservados = registroObservados,
                            ListaErroresCeldasAcoplamiento = listaRegistros.Where(p => p.Error.Length > 0).ToList(),
                            Mensaje = ""
                        });

                    }

                    Evaluacion.Helper.FormatoHelper.BorrarArchivo(this.RutaCompletaArchivo);
                    return Json(new Respuesta { Exito = true, RegistrosProcesados = listaRegistros.Count });
                }
                else
                {
                    Evaluacion.Helper.FormatoHelper.BorrarArchivo(this.RutaCompletaArchivo);
                    return Json(new Respuesta
                    {
                        Exito = false,
                        RegistrosObservados = registroObservados,
                        RegistrosProcesados = 0,
                        Mensaje = !string.IsNullOrEmpty(matrizValida.Mensaje) ? matrizValida.Mensaje : ""
                    });
                }

            }
            catch (Exception ex)
            {
                log.Error("LeerExcelSubido", ex);
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        private void ValidarRegistros(List<EprCargaMasivaCeldaAcoplamientoDTO> listaRegistros)
        {
            var codigosRepetidos = listaRegistros
            .GroupBy(e => e.Codigo)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

            foreach (var codigo in codigosRepetidos)
            {
                var registrosRepetidos = listaRegistros.Where(e => e.Codigo == codigo).ToList();

                foreach (var item in registrosRepetidos)
                {
                    item.Error = "Código ID duplicado. Revisar.";
                }
            }

            //Si no hay codigo duplicados se valida todo el registro
            if (codigosRepetidos.Count() == 0)
            {
                foreach (var registro in listaRegistros)
                {
                    var res = servicioCeldaAcoplamiento.ValidarCargaMasivaCeldaAcoplamiento(registro);

                    registro.Error = !string.IsNullOrEmpty(res) ? res : "";
                }
            }

        }

        public ActionResult Upload(string fecha)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + Intranet.Helper.Constantes.RutaCarga;

                string extension = string.Empty;
                string nombreArchivo = string.Empty;
                string nombreArchivoFinal = string.Empty;
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    nombreArchivo = System.IO.Path.GetFileNameWithoutExtension(file.FileName);

                    extension = System.IO.Path.GetExtension(file.FileName);
                    nombreArchivoFinal = nombreArchivo + "_" + fecha + extension;
                    string fileName = path + nombreArchivoFinal;

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    this.RutaCompletaArchivo = fileName;
                    file.SaveAs(fileName);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                log.Fatal("Upload", ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ExportarDatos(string equicodi, string codigo, string emprcodi, string equiestado, string idsubestacion, string idarea, string tension)
        {

            CeldaAcoplamientoModel model = new CeldaAcoplamientoModel();

            try
            {
                base.ValidarSesionJsonResult();

                string fileName = ConstantesEvaluacion.NombrePlantillaExcelExportacionCeldasAcoplamiento;
                string pathOrigen = ConstantesEvaluacion.FolderGestProtec + "/" + ConstantesEvaluacion.Plantilla;
                string pathDestino = FileServer.GetDirectory() + ConstantesEvaluacion.FolderTemporal + "/";

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);


                //validaciones
                if (string.IsNullOrEmpty(equicodi)) equicodi = "0";
                if (string.IsNullOrEmpty(emprcodi)) emprcodi = "0";
                if (string.IsNullOrEmpty(idsubestacion)) idsubestacion = "0";
                if (string.IsNullOrEmpty(idarea)) idarea = "0";
                if (string.IsNullOrEmpty(tension)) tension = "0";

                servicioCeldaAcoplamiento.GenerarExcelExportar(pathDestino, fileName, equicodi, codigo, emprcodi, equiestado, idsubestacion, idarea, tension);

                model.Resultado = "1";
                model.NombreArchivo = fileName;
                model.StrMensaje = "";


            }
            catch (Exception ex)
            {
                log.Error("CeldaAcoplamientoController", ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        public JsonResult GuardarCeldaAcoplamiento(CeldaAcoplamientoIncluirModificarModel model)
        {
            string resultado = string.Empty;
            try
            {
                EprEquipoDTO oEquipo = null;

                oEquipo = new EprEquipoDTO();
                oEquipo.IdCelda = Convert.ToInt32(model.Equicodi);
                oEquipo.IdProyecto = model.IdProyecto;
                oEquipo.Fecha = model.Fecha;
                oEquipo.IdInterruptor = model.IdInterruptor;
                oEquipo.CapacidadA = model.CapacidadA;
                oEquipo.CapacidadAComent = model.CapacidadAComent;
                oEquipo.CapacidadMvar = model.CapacidadMvar;
                oEquipo.CapacidadMvarComent = model.CapacidadMvarComent;
                oEquipo.CapacidadTransmisionA = model.CapacidadTransmisionA;
                oEquipo.CapacidadTransmisionAComent = model.CapacidadTransmisionAComent;
                oEquipo.CapacidadTransmisionMvar = model.CapacidadTransmisionMvar;
                oEquipo.CapacidadTransmisionMvarComent = model.CapacidadTransmisionMvarComent;
                oEquipo.FactorLimitanteCalc = model.FactorLimitanteCalc;
                oEquipo.FactorLimitanteCalcComent = model.FactorLimitanteCalcComent;
                oEquipo.FactorLimitanteFinal = model.FactorLimitanteFinal;
                oEquipo.FactorLimitanteFinalComent = model.FactorLimitanteFinalComent;
                oEquipo.Observaciones = model.Observaciones;
                oEquipo.UsuarioAuditoria = User.Identity.Name;

                resultado = servicioCeldaAcoplamiento.RegistrarCeldaAcoplamiento(oEquipo);

                if (resultado != "")
                {
                    return Json("Ocurrio un error: " + resultado);
                }
                else
                {
                    return Json(resultado);
                }


            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                resultado = "Ocurrio un error";
                return Json(resultado);
            }
        }

        public JsonResult CalcularCeldaAcoplamiento(CeldaAcoplamientoIncluirModificarModel model)
        {
            string resultado = string.Empty;
            try
            {
                EprEquipoDTO oEquipo = null;

                oEquipo = new EprEquipoDTO();
                oEquipo.IdCelda = Convert.ToInt32(model.Equicodi);
                oEquipo.IdProyecto = model.IdProyecto;
                
                oEquipo.IdInterruptor = model.IdInterruptor;
                oEquipo.CapacidadA = model.CapacidadA;               
                oEquipo.CapacidadMvar = model.CapacidadMvar;                
                oEquipo.CapacidadTransmisionA = model.CapacidadTransmisionA;               
                oEquipo.CapacidadTransmisionMvar = model.CapacidadTransmisionMvar;              
                oEquipo.FactorLimitanteCalc = model.FactorLimitanteCalc;               
                oEquipo.FactorLimitanteFinal = model.FactorLimitanteFinal;

                var calculosInterruptor = calculo.ListCalculosFormulasInterruptor(oEquipo);
                EvaluacionHelperCalculos.n_calc.EvaluarFormulas(calculosInterruptor);

                 var calculosEquipo = calculo.ListCalculosFormulasCelda(oEquipo, 1);
                EvaluacionHelperCalculos.n_calc.EvaluarFormulas(calculosEquipo);


                if (calculosInterruptor.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NombreCapacidadMva && p.Estado == 1))
                {
                    model.CapacidadMvar = EvaluacionHelper.RedondearValor(calculosInterruptor.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NombreCapacidadMva && p.Estado == 1).Valor, 2);
                }
                else
                {
                    model.CapacidadMvar = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NombreCapacidadTransmisionA && p.Estado == 1))
                {
                    model.CapacidadTransmisionA = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NombreCapacidadTransmisionA && p.Estado == 1).Valor, 2);
                }
                else
                {
                    model.CapacidadTransmisionA = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NombreCapacidadTransmisionMvar && p.Estado == 1))
                {
                    model.CapacidadTransmisionMvar = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NombreCapacidadTransmisionMvar && p.Estado == 1).Valor, 2);
                }
                else
                {
                    model.CapacidadTransmisionMvar = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NombreFactorLimitanteCalc && p.Estado == 1))
                {
                    model.FactorLimitanteCalc = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NombreFactorLimitanteCalc && p.Estado == 1).Valor.ToString();
                }
                else
                {
                    model.FactorLimitanteCalc = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NombreFactorLimitanteFinal && p.Estado == 1))
                {
                    model.FactorLimitanteFinal = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NombreFactorLimitanteFinal && p.Estado == 1).Valor.ToString();
                }
                else
                {
                    model.FactorLimitanteFinal = "";
                }


                model.MensajeError = "";

                return Json(model);


            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                model.MensajeError = "Ocurrio un error";
                return Json(resultado);
            }
        }

        [HttpPost]
        public JsonResult ExcluirEquipo(int id)
        {
            string resultado = string.Empty;
            try
            {
                EprEquipoDTO equipo = new EprEquipoDTO();
                equipo.Equicodi = id;
                equipo.UsuarioAuditoria = User.Identity.Name;

                resultado = servicioTransversal.ExcluirEquipoProtecciones(equipo);
                return Json(resultado);
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                resultado = "Ocurrio un error";
                return Json(resultado);
            }
        }
    }
}
