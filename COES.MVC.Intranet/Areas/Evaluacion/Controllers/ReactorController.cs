using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;

using COES.Servicios.Aplicacion.Equipamiento;
using log4net;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.Evaluacion.Models;
using COES.MVC.Intranet.Areas.Proteccion.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Evaluacion;
using COES.Servicios.Aplicacion.Migraciones;

using COES.Framework.Base.Tools;
using System.Collections.Generic;
using COES.MVC.Intranet.Areas.Evaluacion.Helper;
using EvaluacionHelperCalculos = COES.Servicios.Aplicacion.Evaluacion.Helper;

using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;


namespace COES.MVC.Intranet.Areas.Evaluacion.Controllers
{
    public class ReactorController : BaseController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ReactorController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        EquipamientoAppServicio servicioEquipamiento = new EquipamientoAppServicio();
        EquipoProteccionAppServicio equipoProteccion = new EquipoProteccionAppServicio();
        ConsultaMedidoresAppServicio consultaMedidores = new ConsultaMedidoresAppServicio();
        ReactorAppServicio servicioReactor = new ReactorAppServicio();
        MigracionesAppServicio migraciones = new MigracionesAppServicio();
        ProyectoActualizacionAppServicio servicioProyectoActualizacion = new ProyectoActualizacionAppServicio();
        TransversalAppServicio servicioTransversal = new TransversalAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        CalculosAppServicio calculo = new CalculosAppServicio();

        #region Propiedades

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

        public ReactorController()
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
            ReactorModel modelo = new ReactorModel();

            modelo.ListaUbicacion = equipoProteccion.ListSubEstacion();
            modelo.ListaEmpresa = consultaMedidores.ListObtenerEmpresaSEINProtecciones();
            modelo.ListaArea = servicioEquipamiento.ListarZonasxNivel(5);
            modelo.ListaEstado = equipoProteccion.ListPropCatalogoData(ConstantesProteccion.EstadoLinea);


            modelo.equicodi = Session[ConstantesEvaluacion.R_Equicodi] != null ? Session[ConstantesEvaluacion.R_Equicodi].ToString() : "";
            modelo.codigo = Session[ConstantesEvaluacion.R_Codigo] != null ? Session[ConstantesEvaluacion.R_Codigo].ToString() : "";
            modelo.ubicacion = Session[ConstantesEvaluacion.R_Ubicacion] != null ? (int)Session[ConstantesEvaluacion.R_Ubicacion] : 0;
            modelo.empresa = Session[ConstantesEvaluacion.R_Empresa] != null ? (int)Session[ConstantesEvaluacion.R_Empresa] : 0;
            modelo.area = Session[ConstantesEvaluacion.R_Area] != null ? (int)Session[ConstantesEvaluacion.R_Area] : 0;
            modelo.estado = Session[ConstantesEvaluacion.R_Estado] != null ? Session[ConstantesEvaluacion.R_Estado].ToString() : "";
            modelo.incluirCalcular = Session[ConstantesEvaluacion.R_IncluirCalcular] != null ? (int)Session[ConstantesEvaluacion.R_IncluirCalcular] : 0;

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
        public ActionResult IndexPost(ReactorModel datos)
        {
            return View(datos);
        }

        [HttpPost]
        public PartialViewResult ListaReactor(string codigoId, string codigo, int ubicacion,
            int empresa, int area, string estado, int incluirCalcular)
        {

            Session[ConstantesEvaluacion.R_Equicodi] = codigoId;
            Session[ConstantesEvaluacion.R_Codigo] = codigo;
            Session[ConstantesEvaluacion.R_Ubicacion] = ubicacion;
            Session[ConstantesEvaluacion.R_Empresa] = empresa;
            Session[ConstantesEvaluacion.R_Area] = area;
            Session[ConstantesEvaluacion.R_Estado] = estado;
            Session[ConstantesEvaluacion.R_IncluirCalcular] = incluirCalcular;

            ListadoReactorModel model = new ListadoReactorModel();
            model.ListaReactor = servicioReactor.ListaReactor(codigoId, codigo, ubicacion,
            empresa, area, estado, incluirCalcular).ToList();

            int id = (Session[DatosSesion.SesionIdOpcion] != null) ? Convert.ToInt32(Session[DatosSesion.SesionIdOpcion]) : 0;
            bool permisoEdicion = this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, Acciones.Editar, User.Identity.Name);
            bool permisoEliminar = this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, Acciones.Eliminar, User.Identity.Name);

            ViewBag.PermisoEditar = permisoEdicion ? "1" : "0";
            ViewBag.PermisoEliminar = permisoEliminar ? "1" : "0";

            return PartialView("~/Areas/Evaluacion/Views/Reactor/ListaReactor.cshtml", model);
        }

        public ActionResult BuscarReactor()
        {
            var model = new BuscarReactorModel();

            model.ListaUbicacion = equipoProteccion.ListSubEstacion();
            model.ListaEmpresa = consultaMedidores.ListObtenerEmpresaSEINProtecciones();
            model.ListaEstado = equipoProteccion.ListPropCatalogoData(6);

            return View("~/Areas/Evaluacion/Views/Reactor/BuscarReactor.cshtml", model);
        }

        [HttpPost]
        public PartialViewResult BuscarReactorLista(string equiCodi, string codigo, int ubicacion, int empresa, string estado)
        {
            BuscarReactorListaModel model = new BuscarReactorListaModel();
            model.ListaBuscarReactor = servicioReactor.ListaBuscarReactor(equiCodi, codigo, ubicacion, empresa, estado).ToList();

            return PartialView("~/Areas/Evaluacion/Views/Reactor/BuscarReactorLista.cshtml", model);
        }

        public PartialViewResult Incluir(int id)
        {
            var model = new ReactorIncluirModel();
            model.listaProyecto = equipoProteccion.ListProyectoProyectoActualizacion(id);
            model.idProyecto = 0;
            return PartialView("~/Areas/Evaluacion/Views/Reactor/Incluir.cshtml", model);
        }

        public ActionResult IncluirModificar(int idReactor, int idProyecto, int idEquipo, string accion)
        {
            EprEquipoDTO equipo = servicioTransversal.ObtenerCabeceraEquipoPorId(idEquipo);

            var model = new ReactorIncluirModificarModel();
            model.ListaCelda = equipoProteccion.ListCeldaEvaluacion("0");
            model.ListaCelda2 = equipoProteccion.ListCeldaEvaluacion("0");

            model.IdReactor = idReactor;
            model.IdEquipo = idEquipo;
            model.IdProyecto = idProyecto;
            model.Equicodi = idReactor.ToString();
            model.Accion = accion;

            EprProyectoActEqpDTO proyecto = servicioProyectoActualizacion.EprProyectoGetById(idProyecto);
            model.MotivoActualizacion = proyecto.Epproynemotecnico + ": " + proyecto.Epproynomb;
            model.FechaActualizacion = proyecto.Epproyfecregistro;

           
            model.Equicodi = equipo.Equicodi.ToString();
            model.Codigo = equipo.Codigo;
            model.Ubicacion = equipo.Ubicacion;
            model.Area = equipo.Area;
            model.Empresa = equipo.Empresa;

       
            EprEquipoDTO reactor = servicioReactor.ObtenerReactorPorId(idEquipo);
            if (equipo != null)
            {
                model.IdCelda1 = reactor.IdCelda1;
                model.Celda1PosicionNucleoTc = reactor.Celda1PosicionNucleoTc;
                model.Celda1PickUp = reactor.Celda1PickUp;
                model.IdCelda2 = reactor.IdCelda22;
                model.Celda2PosicionNucleoTc = reactor.Celda2PosicionNucleoTc;
                model.Celda2PickUp = reactor.Celda2PickUp;
                model.NivelTension = reactor.NivelTension;
                model.CapacidadMvar = reactor.CapacidadMvar;
                model.CapacidadA = reactor.CapacidadA;
                model.CapacidadTransmisionA = reactor.CapacidadTransmisionA;
                model.CapacidadTransmisionAComent = reactor.CapacidadTransmisionAComent;
                model.CapacidadTransmisionMvar = reactor.CapacidadTransmisionMvar;
                model.CapacidadTransmisionMvarComent = reactor.CapacidadTransmisionMvarComent;
                model.FactorLimitanteCalc = reactor.FactorLimitanteCalc;
                model.FactorLimitanteCalcComent = reactor.FactorLimitanteCalcComent;
                model.FactorLimitanteFinal = reactor.FactorLimitanteFinal;
                model.FactorLimitanteFinalComent = reactor.FactorLimitanteFinalComent;
                model.Observaciones = reactor.Observaciones;
                model.UsuarioModificacion = reactor.UsuarioModificacion;
                model.FechaModificacion = reactor.FechaModificacion;

            }
            

            return View("~/Areas/Evaluacion/Views/Reactor/IncluirModificar.cshtml", model);
        }

        [HttpPost]
        public PartialViewResult EditarComentario(string comentario)
        {
            ReactorEditarComentarioModel model = new ReactorEditarComentarioModel();
            model.Comentario = comentario;

            return PartialView("~/Areas/Evaluacion/Views/Reactor/EditarComentario.cshtml", model);
        }

        [HttpPost]
        public JsonResult GuardarReactor(ReactorIncluirModificarModel model)
        {
            string resultado = string.Empty;
            try
            {
                EprEquipoDTO oEquipo = null;

                oEquipo = new EprEquipoDTO();
                oEquipo.IdReactor = Convert.ToInt32(model.Equicodi);
                oEquipo.IdProyecto = model.IdProyecto;
                oEquipo.Fecha = model.Fecha;
                oEquipo.IdCelda1 = model.IdCelda1;
                oEquipo.IdCelda22 = model.IdCelda2;
                oEquipo.CapacidadMvar = model.CapacidadMvar;
                oEquipo.CapacidadA = model.CapacidadA;
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

                resultado = servicioReactor.RegistrarReactor(oEquipo);


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

        public JsonResult ExportarPlantilla()
        {
            ReactorModel model = new ReactorModel();

            try
            {
                base.ValidarSesionJsonResult();

                string fileName = ConstantesEvaluacion.NombrePlantillaExcelReactor;
                string pathOrigen = ConstantesEvaluacion.FolderGestProtec + "/" + ConstantesEvaluacion.Plantilla;
                string pathDestino = FileServer.GetDirectory() + ConstantesEvaluacion.FolderTemporal + "/";

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);

                servicioReactor.GenerarExcelPlantilla(pathDestino, fileName);

                model.Resultado = "1";
                model.NombreArchivo = fileName;
                model.StrMensaje = "";


            }
            catch (Exception ex)
            {
                log.Error("ReactorController", ex);
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
                List<EprCargaMasivaLineaDTO> listaRegistros = null;
                Respuesta matrizValida = new Respuesta();

                var titulos = 8;
                listaRegistros = Evaluacion.Helper.FormatoHelper.LeerExcelCargadoReactor(this.RutaCompletaArchivo, titulos, out matrizValida);


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

                            var res = servicioReactor.SaveCargaMasivaReactor(registro);

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
                            ListaErrores = listaRegistros.Where(p => p.Error.Length > 0).ToList(),
                            Mensaje = ""
                        });

                    }

                    Evaluacion.Helper.FormatoHelper.BorrarArchivo(this.RutaCompletaArchivo);
                    return Json(new Respuesta { Exito = true, RegistrosProcesados = listaRegistros.Count() });
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

        private void ValidarRegistros(List<EprCargaMasivaLineaDTO> listaRegistros)
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
                    var res = servicioReactor.ValidarCargaMasivaReactor(registro);

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

        public JsonResult ExportarDatos(string equicodi, string codigo, string emprcodi, string equiestado, string idsubestacion,  string idarea)
        {                     

            ReactorModel model = new ReactorModel();

            try
            {
                base.ValidarSesionJsonResult();

                string fileName = ConstantesEvaluacion.NombrePlantillaExcelExportacionReactores;
                string pathOrigen = ConstantesEvaluacion.FolderGestProtec + "/" + ConstantesEvaluacion.Plantilla;
                string pathDestino = FileServer.GetDirectory() + ConstantesEvaluacion.FolderTemporal + "/";

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);


                //validaciones
                if (string.IsNullOrEmpty(equicodi)) equicodi = "0";
                if (string.IsNullOrEmpty(emprcodi)) emprcodi = "0";
                if (string.IsNullOrEmpty(idsubestacion)) idsubestacion = "0";               
                if (string.IsNullOrEmpty(idarea)) idarea = "0";           

                servicioReactor.GenerarExcelExportar(pathDestino, fileName, equicodi, codigo, emprcodi, equiestado, idsubestacion, idarea);

                model.Resultado = "1";
                model.NombreArchivo = fileName;
                model.StrMensaje = "";


            }
            catch (Exception ex)
            {
                log.Error("ReactorController", ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult ConsultaCelda(int IdCelda)
        {
            EprEquipoDTO equipo  = equipoProteccion.ObtenerDatoCelda(IdCelda);
           
            return Json(equipo);
        }

        public JsonResult CalcularReactor(ReactorIncluirModificarModel model)
        {
            string resultado = string.Empty;
            try
            {
                EprEquipoDTO oEquipo = null;

                oEquipo = new EprEquipoDTO();
                oEquipo.IdReactor = Convert.ToInt32(model.Equicodi);
                oEquipo.IdProyecto = model.IdProyecto;               
                oEquipo.IdCelda1 = model.IdCelda1;
                oEquipo.IdCelda22 = model.IdCelda2;
                oEquipo.CapacidadMvar = model.CapacidadMvar;
                oEquipo.CapacidadA = model.CapacidadA;
                oEquipo.CapacidadTransmisionA = model.CapacidadTransmisionA;               
                oEquipo.CapacidadTransmisionMvar = model.CapacidadTransmisionMvar;                
                oEquipo.FactorLimitanteCalc = model.FactorLimitanteCalc;               
                oEquipo.FactorLimitanteFinal = model.FactorLimitanteFinal;
                oEquipo.NivelTension = model.NivelTension;
                  

                var calculosEquipo = calculo.ListCalculosFormulasReactor(oEquipo, 1);
                EvaluacionHelperCalculos.n_calc.EvaluarFormulas(calculosEquipo);
                                                        

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NombreCapacidadTransmisionA && p.Estado == 1))
                {
                    model.CapacidadTransmisionA = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NombreCapacidadTransmisionA && p.Estado == 1).Valor);
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
                return Json(model);
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
