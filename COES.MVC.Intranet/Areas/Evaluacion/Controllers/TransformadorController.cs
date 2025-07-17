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
using COES.MVC.Intranet.Areas.Evaluacion.Helper;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.Mediciones;
using COES.MVC.Intranet.Areas.Proteccion.Helper;
using COES.Servicios.Aplicacion.Evaluacion;
using COES.Framework.Base.Tools;
using EvaluacionHelperCalculos = COES.Servicios.Aplicacion.Evaluacion.Helper;

using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;

namespace COES.MVC.Intranet.Areas.Evaluacion.Controllers
{
    public class TransformadorController : BaseController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(TransformadorController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        EquipamientoAppServicio servicioEquipamiento = new EquipamientoAppServicio();
        EquipoProteccionAppServicio equipoProteccion = new EquipoProteccionAppServicio();
        MigracionesAppServicio migraciones = new MigracionesAppServicio();
        ConsultaMedidoresAppServicio consultaMedidores = new ConsultaMedidoresAppServicio();
        TransformadorAppServicio servicioTransformador = new TransformadorAppServicio();
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

        public TransformadorController()
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
            TransformadorModel modelo = new TransformadorModel();

            modelo.ListaUbicacion = equipoProteccion.ListSubEstacion();
            modelo.ListaEmpresa = consultaMedidores.ListObtenerEmpresaSEINProtecciones();
            modelo.ListaArea = servicioEquipamiento.ListarZonasxNivel(5);
            modelo.ListaEstado = equipoProteccion.ListPropCatalogoData(ConstantesProteccion.EstadoLinea);
            modelo.ListaTipo = servicioTransformador.ListaTransformadores();


            modelo.equicodi = Session[ConstantesEvaluacion.T_Equicodi] != null ? Session[ConstantesEvaluacion.T_Equicodi].ToString() : "";
            modelo.codigo = Session[ConstantesEvaluacion.T_Codigo] != null ? Session[ConstantesEvaluacion.T_Codigo].ToString() : "";
            modelo.tipo = Session[ConstantesEvaluacion.T_Tipo] != null ? (int)Session[ConstantesEvaluacion.T_Tipo] : 0;
            modelo.ubicacion = Session[ConstantesEvaluacion.T_Ubicacion] != null ? (int)Session[ConstantesEvaluacion.T_Ubicacion] : 0;
            modelo.empresa = Session[ConstantesEvaluacion.T_Empresa] != null ? (int)Session[ConstantesEvaluacion.T_Empresa] : 0;
            modelo.area = Session[ConstantesEvaluacion.T_Area] != null ? (int)Session[ConstantesEvaluacion.T_Area] : 0;
            modelo.tension = Session[ConstantesEvaluacion.T_Tension] != null ? Session[ConstantesEvaluacion.T_Tension].ToString() : "";
            modelo.estado = Session[ConstantesEvaluacion.T_Estado] != null ? Session[ConstantesEvaluacion.T_Estado].ToString() : "";
            modelo.incluirCalcular = Session[ConstantesEvaluacion.T_IncluirCalcular] != null ? (int)Session[ConstantesEvaluacion.T_IncluirCalcular] : 0;

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
        public ActionResult IndexPost(TransformadorModel datos)
        {
            return View(datos);
        }

        [HttpPost]
        public PartialViewResult ListaTransformador(string codigoId, string codigo, int tipo, int subestacion,
            int empresa, int area, string estado, string tension, int incluirCalcular)
        {

            Session[ConstantesEvaluacion.T_Equicodi] = codigoId;
            Session[ConstantesEvaluacion.T_Codigo] = codigo;
            Session[ConstantesEvaluacion.T_Tipo] = tipo;
            Session[ConstantesEvaluacion.T_Ubicacion] = subestacion;
            Session[ConstantesEvaluacion.T_Empresa] = empresa;
            Session[ConstantesEvaluacion.T_Area] = area;
            Session[ConstantesEvaluacion.T_Tension] = tension;
            Session[ConstantesEvaluacion.T_Estado] = estado;
            Session[ConstantesEvaluacion.T_IncluirCalcular] = incluirCalcular;

            ListadoTransformadorModel model = new ListadoTransformadorModel();
            model.ListaTransformador = servicioTransformador.ListaTransformador(codigoId, codigo, tipo, subestacion,
            empresa, area, estado, tension, incluirCalcular).ToList();

            int id = (Session[DatosSesion.SesionIdOpcion] != null) ? Convert.ToInt32(Session[DatosSesion.SesionIdOpcion]) : 0;
            bool permisoEdicion = this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, Acciones.Editar, User.Identity.Name);
            bool permisoEliminar = this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, Acciones.Eliminar, User.Identity.Name);

            ViewBag.PermisoEditar = permisoEdicion ? "1" : "0";
            ViewBag.PermisoEliminar = permisoEliminar ? "1" : "0";

            return PartialView("~/Areas/Evaluacion/Views/Transformador/ListaTransformador.cshtml", model);
        }

        public ActionResult BuscarTransformador()
        {
            var model = new BuscarTransformadorModel();
            model.idProyecto = 0;
            model.ListaTipo = servicioTransformador.ListaTransformadores();
            model.ListaUbicacion = equipoProteccion.ListSubEstacion();
            model.ListaEmpresa = consultaMedidores.ListObtenerEmpresaSEINProtecciones();
            model.ListaEstado = equipoProteccion.ListPropCatalogoData(6);

            return View("~/Areas/Evaluacion/Views/Transformador/BuscarTransformador.cshtml", model);
        }

        [HttpPost]
        public PartialViewResult BuscarTransformadorLista(string equiCodi, string codigo, int tipo, int ubicacion, int empresa, string estado)
        {
            BuscarTransformadorListaModel model = new BuscarTransformadorListaModel();
            model.ListaBuscarTransformador = servicioTransformador.ListaBuscarTransformador(tipo, equiCodi, codigo, ubicacion, empresa, estado).ToList();

            return PartialView("~/Areas/Evaluacion/Views/Transformador/BuscarTransformadorLista.cshtml", model);
        }

        public PartialViewResult Incluir(int id)
        {
            var model = new TransformadorIncluirModel();
            model.listaProyecto = equipoProteccion.ListProyectoProyectoActualizacion(id);
            model.idProyecto = 0;
            return PartialView("~/Areas/Evaluacion/Views/Transformador/Incluir.cshtml", model);
        }

        public ActionResult IncluirModificar(int idTransformador, int idProyecto, int idEquipo, string accion)
        {
            EprEquipoDTO equipo = servicioTransversal.ObtenerCabeceraEquipoPorId(idEquipo);

            var model = new TransformadorIncluirModificarModel();
            model.ListaCelda = equipoProteccion.ListCeldaEvaluacion("0");

            model.IdTransformador = idTransformador;
            model.IdEquipo = idEquipo;
            model.IdProyecto = idProyecto;
            model.Equicodi = idTransformador.ToString();
            model.Accion = accion;

            EprProyectoActEqpDTO proyecto = servicioProyectoActualizacion.EprProyectoGetById(idProyecto);
            model.MotivoActualizacion = proyecto.Epproynemotecnico + ": " + proyecto.Epproynomb;
            model.FechaActualizacion = proyecto.Epproyfecregistro;


            model.Equicodi = equipo.Equicodi.ToString();
            model.Codigo = equipo.Codigo;
            model.Ubicacion = equipo.Ubicacion;
            model.Area = equipo.Area;
            model.Empresa = equipo.Empresa;
            model.FamCodigo = equipo.FamCodigo;
            model.FamNombre = equipo.Tipo;

            EprEquipoDTO transformador = servicioTransformador.ObtenerTransformadorPorId(idEquipo);
            if (equipo != null)
            {
                model.D1IdCelda = transformador.D1IdCelda;
                model.D2IdCelda = transformador.D2IdCelda;
                model.D3IdCelda = transformador.D3IdCelda;
                model.D4IdCelda = transformador.D4IdCelda;
                model.D1Tension = transformador.D1Tension;
                model.D2Tension = transformador.D2Tension;
                model.D3Tension = transformador.D3Tension;
                model.D4Tension = transformador.D4Tension;
                model.D1CapacidadOnanMva = transformador.D1CapacidadOnanMva;
                model.D2CapacidadOnanMva = transformador.D2CapacidadOnanMva;
                model.D3CapacidadOnanMva = transformador.D3CapacidadOnanMva;
                model.D4CapacidadOnanMva = transformador.D4CapacidadOnanMva;
                model.D1CapacidadOnanMvaComent = transformador.D1CapacidadOnanMvaComent;
                model.D2CapacidadOnanMvaComent = transformador.D2CapacidadOnanMvaComent;
                model.D3CapacidadOnanMvaComent = transformador.D3CapacidadOnanMvaComent;
                model.D4CapacidadOnanMvaComent = transformador.D4CapacidadOnanMvaComent;
                model.D1CapacidadOnafMva = transformador.D1CapacidadOnafMva;
                model.D2CapacidadOnafMva = transformador.D2CapacidadOnafMva;
                model.D3CapacidadOnafMva = transformador.D3CapacidadOnafMva;
                model.D4CapacidadOnafMva = transformador.D4CapacidadOnafMva;
                model.D1CapacidadOnafMvaComent = transformador.D1CapacidadOnafMvaComent;
                model.D2CapacidadOnafMvaComent = transformador.D2CapacidadOnafMvaComent;
                model.D3CapacidadOnafMvaComent = transformador.D3CapacidadOnafMvaComent;
                model.D4CapacidadOnafMvaComent = transformador.D4CapacidadOnafMvaComent;
                model.D1CapacidadMva = transformador.D1CapacidadMva;
                model.D2CapacidadMva = transformador.D2CapacidadMva;
                model.D3CapacidadMva = transformador.D3CapacidadMva;
                model.D4CapacidadMva = transformador.D4CapacidadMva;
                model.D1CapacidadMvaComent = transformador.D1CapacidadMvaComent;
                model.D2CapacidadMvaComent = transformador.D2CapacidadMvaComent;
                model.D3CapacidadMvaComent = transformador.D3CapacidadMvaComent;
                model.D4CapacidadMvaComent = transformador.D4CapacidadMvaComent;
                model.D1CapacidadA = transformador.D1CapacidadA;
                model.D2CapacidadA = transformador.D2CapacidadA;
                model.D3CapacidadA = transformador.D3CapacidadA;
                model.D4CapacidadA = transformador.D4CapacidadA;
                model.D1CapacidadAComent = transformador.D1CapacidadAComent;
                model.D2CapacidadAComent = transformador.D2CapacidadAComent;
                model.D3CapacidadAComent = transformador.D3CapacidadAComent;
                model.D4CapacidadAComent = transformador.D4CapacidadAComent;
                model.D1CapacidadTransmisionMva = transformador.D1CapacidadTransmisionMva;
                model.D2CapacidadTransmisionMva = transformador.D2CapacidadTransmisionMva;
                model.D3CapacidadTransmisionMva = transformador.D3CapacidadTransmisionMva;
                model.D4CapacidadTransmisionMva = transformador.D4CapacidadTransmisionMva;
                model.D1CapacidadTransmisionMvaComent = transformador.D1CapacidadTransmisionMvaComent;
                model.D2CapacidadTransmisionMvaComent = transformador.D2CapacidadTransmisionMvaComent;
                model.D3CapacidadTransmisionMvaComent = transformador.D3CapacidadTransmisionMvaComent;
                model.D4CapacidadTransmisionMvaComent = transformador.D4CapacidadTransmisionMvaComent;
                model.D1CapacidadTransmisionA = transformador.D1CapacidadTransmisionA;
                model.D2CapacidadTransmisionA = transformador.D2CapacidadTransmisionA;
                model.D3CapacidadTransmisionA = transformador.D3CapacidadTransmisionA;
                model.D4CapacidadTransmisionA = transformador.D4CapacidadTransmisionA;
                model.D1CapacidadTransmisionAComent = transformador.D1CapacidadTransmisionAComent;
                model.D2CapacidadTransmisionAComent = transformador.D2CapacidadTransmisionAComent;
                model.D3CapacidadTransmisionAComent = transformador.D3CapacidadTransmisionAComent;
                model.D4CapacidadTransmisionAComent = transformador.D4CapacidadTransmisionAComent;
                model.D1FactorLimitanteCalc = transformador.D1FactorLimitanteCalc;
                model.D2FactorLimitanteCalc = transformador.D2FactorLimitanteCalc;
                model.D3FactorLimitanteCalc = transformador.D3FactorLimitanteCalc;
                model.D4FactorLimitanteCalc = transformador.D4FactorLimitanteCalc;
                model.D1FactorLimitanteCalcComent = transformador.D1FactorLimitanteCalcComent;
                model.D2FactorLimitanteCalcComent = transformador.D2FactorLimitanteCalcComent;
                model.D3FactorLimitanteCalcComent = transformador.D3FactorLimitanteCalcComent;
                model.D4FactorLimitanteCalcComent = transformador.D4FactorLimitanteCalcComent;
                model.D1FactorLimitanteFinal = transformador.D1FactorLimitanteFinal;
                model.D2FactorLimitanteFinal = transformador.D2FactorLimitanteFinal;
                model.D3FactorLimitanteFinal = transformador.D3FactorLimitanteFinal;
                model.D4FactorLimitanteFinal = transformador.D4FactorLimitanteFinal;
                model.D1FactorLimitanteFinalComent = transformador.D1FactorLimitanteFinalComent;
                model.D2FactorLimitanteFinalComent = transformador.D2FactorLimitanteFinalComent;
                model.D3FactorLimitanteFinalComent = transformador.D3FactorLimitanteFinalComent;
                model.D4FactorLimitanteFinalComent = transformador.D4FactorLimitanteFinalComent;
                model.D2Observaciones = transformador.D2Observaciones;
                model.D3Observaciones = transformador.D3Observaciones;
                model.D4Observaciones = transformador.D4Observaciones;
                model.Observaciones = transformador.Observaciones;

                model.D1PickUp = transformador.D1PickUp;
                model.D2PickUp = transformador.D2PickUp;
                model.D3PickUp = transformador.D3PickUp;
                model.D4PickUp = transformador.D4PickUp;

                model.D1PosicionTcA = transformador.D1PosicionTcA;
                model.D2PosicionTcA = transformador.D2PosicionTcA;
                model.D3PosicionTcA = transformador.D3PosicionTcA;
                model.D4PosicionTcA = transformador.D4PosicionTcA;

                model.UsuarioModificacion = transformador.UsuarioModificacion;
                model.FechaModificacion = transformador.FechaModificacion;
            }
            //}

            return View("~/Areas/Evaluacion/Views/Transformador/IncluirModificar.cshtml", model);
        }

        public JsonResult GuardarTransformador(TransformadorIncluirModificarModel model)
        {
            string resultado = string.Empty;
            try
            {
                EprEquipoDTO oEquipo = null;

                oEquipo = new EprEquipoDTO();
                oEquipo.IdTransformador = Convert.ToInt32(model.Equicodi);
                oEquipo.IdProyecto = model.IdProyecto;
                oEquipo.Fecha = model.Fecha;
                oEquipo.D1IdCelda = model.D1IdCelda;
                oEquipo.D1CapacidadOnanMva = model.D1CapacidadOnanMva;
                oEquipo.D1CapacidadOnanMvaComent = model.D1CapacidadOnanMvaComent;
                oEquipo.D1CapacidadOnafMva = model.D1CapacidadOnafMva;
                oEquipo.D1CapacidadOnafMvaComent = model.D1CapacidadOnafMvaComent;
                oEquipo.D1CapacidadMva = model.D1CapacidadMva;
                oEquipo.D1CapacidadMvaComent = model.D1CapacidadMvaComent;
                oEquipo.D1CapacidadA = model.D1CapacidadA;
                oEquipo.D1CapacidadAComent = model.D1CapacidadAComent;
                oEquipo.D1PosicionTcA = model.D1PosicionTcA;
                oEquipo.D1PosicionPickUpA = model.D1PosicionPickUpA;
                oEquipo.D1CapacidadTransmisionA = model.D1CapacidadTransmisionA;
                oEquipo.D1CapacidadTransmisionAComent = model.D1CapacidadTransmisionAComent;
                oEquipo.D1CapacidadTransmisionMva = model.D1CapacidadTransmisionMva;
                oEquipo.D1CapacidadTransmisionMvaComent = model.D1CapacidadTransmisionMvaComent;
                oEquipo.D1FactorLimitanteCalc = model.D1FactorLimitanteCalc;
                oEquipo.D1FactorLimitanteCalcComent = model.D1FactorLimitanteCalcComent;
                oEquipo.D1FactorLimitanteFinal = model.D1FactorLimitanteFinal;
                oEquipo.D1FactorLimitanteFinalComent = model.D1FactorLimitanteFinalComent;
                oEquipo.D2IdCelda = model.D2IdCelda;
                oEquipo.D2CapacidadOnanMva = model.D2CapacidadOnanMva;
                oEquipo.D2CapacidadOnanMvaComent = model.D2CapacidadOnanMvaComent;
                oEquipo.D2CapacidadOnafMva = model.D2CapacidadOnafMva;
                oEquipo.D2CapacidadOnafMvaComent = model.D2CapacidadOnafMvaComent;
                oEquipo.D2CapacidadMva = model.D2CapacidadMva;
                oEquipo.D2CapacidadMvaComent = model.D2CapacidadMvaComent;
                oEquipo.D2CapacidadA = model.D2CapacidadA;
                oEquipo.D2CapacidadAComent = model.D2CapacidadAComent;
                oEquipo.D2PosicionTcA = model.D2PosicionTcA;
                oEquipo.D2PosicionPickUpA = model.D2PosicionPickUpA;
                oEquipo.D2CapacidadTransmisionA = model.D2CapacidadTransmisionA;
                oEquipo.D2CapacidadTransmisionAComent = model.D2CapacidadTransmisionAComent;
                oEquipo.D2CapacidadTransmisionMva = model.D2CapacidadTransmisionMva;
                oEquipo.D2CapacidadTransmisionMvaComent = model.D2CapacidadTransmisionMvaComent;
                oEquipo.D2FactorLimitanteCalc = model.D2FactorLimitanteCalc;
                oEquipo.D2FactorLimitanteCalcComent = model.D2FactorLimitanteCalcComent;
                oEquipo.D2FactorLimitanteFinal = model.D2FactorLimitanteFinal;
                oEquipo.D2FactorLimitanteFinalComent = model.D2FactorLimitanteFinalComent;
                oEquipo.D3IdCelda = model.D3IdCelda;
                oEquipo.D3CapacidadOnanMva = model.D3CapacidadOnanMva;
                oEquipo.D3CapacidadOnanMvaComent = model.D3CapacidadOnanMvaComent;
                oEquipo.D3CapacidadOnafMva = model.D3CapacidadOnafMva;
                oEquipo.D3CapacidadOnafMvaComent = model.D3CapacidadOnafMvaComent;
                oEquipo.D3CapacidadMva = model.D3CapacidadMva;
                oEquipo.D3CapacidadMvaComent = model.D3CapacidadMvaComent;
                oEquipo.D3CapacidadA = model.D3CapacidadA;
                oEquipo.D3CapacidadAComent = model.D3CapacidadAComent;
                oEquipo.D3PosicionTcA = model.D3PosicionTcA;
                oEquipo.D3PosicionPickUpA = model.D3PosicionPickUpA;
                oEquipo.D3CapacidadTransmisionA = model.D3CapacidadTransmisionA;
                oEquipo.D3CapacidadTransmisionAComent = model.D3CapacidadTransmisionAComent;
                oEquipo.D3CapacidadTransmisionMva = model.D3CapacidadTransmisionMva;
                oEquipo.D3CapacidadTransmisionMvaComent = model.D3CapacidadTransmisionMvaComent;
                oEquipo.D3FactorLimitanteCalc = model.D3FactorLimitanteCalc;
                oEquipo.D3FactorLimitanteCalcComent = model.D3FactorLimitanteCalcComent;
                oEquipo.D3FactorLimitanteFinal = model.D3FactorLimitanteFinal;
                oEquipo.D3FactorLimitanteFinalComent = model.D3FactorLimitanteFinalComent;

                oEquipo.D4IdCelda = model.D4IdCelda;
                oEquipo.D4CapacidadOnanMva = model.D4CapacidadOnanMva;
                oEquipo.D4CapacidadOnanMvaComent = model.D4CapacidadOnanMvaComent;
                oEquipo.D4CapacidadOnafMva = model.D4CapacidadOnafMva;
                oEquipo.D4CapacidadOnafMvaComent = model.D4CapacidadOnafMvaComent;
                oEquipo.D4CapacidadMva = model.D4CapacidadMva;
                oEquipo.D4CapacidadMvaComent = model.D4CapacidadMvaComent;
                oEquipo.D4CapacidadA = model.D4CapacidadA;
                oEquipo.D4CapacidadAComent = model.D4CapacidadAComent;
                oEquipo.D4PosicionTcA = model.D4PosicionTcA;
                oEquipo.D4PosicionPickUpA = model.D4PosicionPickUpA;
                oEquipo.D4CapacidadTransmisionA = model.D4CapacidadTransmisionA;
                oEquipo.D4CapacidadTransmisionAComent = model.D4CapacidadTransmisionAComent;
                oEquipo.D4CapacidadTransmisionMva = model.D4CapacidadTransmisionMva;
                oEquipo.D4CapacidadTransmisionMvaComent = model.D4CapacidadTransmisionMvaComent;
                oEquipo.D4FactorLimitanteCalc = model.D4FactorLimitanteCalc;
                oEquipo.D4FactorLimitanteCalcComent = model.D4FactorLimitanteCalcComent;
                oEquipo.D4FactorLimitanteFinal = model.D4FactorLimitanteFinal;
                oEquipo.D4FactorLimitanteFinalComent = model.D4FactorLimitanteFinalComent;

                oEquipo.Observaciones = model.Observaciones;
                oEquipo.UsuarioAuditoria = User.Identity.Name;

                resultado = servicioTransformador.RegistrarTransformador(oEquipo);


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

        [HttpPost]
        public PartialViewResult EditarComentario(string comentario)
        {
            TransformadorEditarComentarioModel model = new TransformadorEditarComentarioModel();
            model.Comentario = comentario;

            return PartialView("~/Areas/Evaluacion/Views/Transformador/EditarComentario.cshtml", model);
        }

        #region Carga Masiva

        public JsonResult ExportarPlantilla()
        {
            TransformadorModel model = new TransformadorModel();

            try
            {
                base.ValidarSesionJsonResult();

                string fileName = ConstantesEvaluacion.NombrePlantillaExcelTransformadores;
                string pathOrigen = ConstantesEvaluacion.FolderGestProtec + "/" + ConstantesEvaluacion.Plantilla;
                string pathDestino = FileServer.GetDirectory() + ConstantesEvaluacion.FolderTemporal + "/";

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);

                servicioTransformador.GenerarExcelPlantilla(pathDestino, fileName);

                model.Resultado = "1";
                model.NombreArchivo = fileName;
                model.StrMensaje = "";


            }
            catch (Exception ex)
            {
                log.Error("TransformadorController", ex);
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
                List<EprCargaMasivaTransformadorDTO> listaRegistros = null;
                Evaluacion.Models.Respuesta matrizValida = new Evaluacion.Models.Respuesta();


                var titulos = 16;
                listaRegistros = Evaluacion.Helper.FormatoHelper.LeerExcelCargadoTransformador(this.RutaCompletaArchivo, titulos, out matrizValida);

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

                            var res = servicioTransformador.SaveCargaMasivaTransformador(registro);

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
                        return Json(new Evaluacion.Models.Respuesta
                        {
                            Exito = false,
                            RegistrosObservados = registroObservados,
                            ListaErroresTransformadores = listaRegistros.Where(p => p.Error.Length > 0).ToList(),
                            Mensaje = ""
                        });
                    }

                    Evaluacion.Helper.FormatoHelper.BorrarArchivo(this.RutaCompletaArchivo);
                    return Json(new Evaluacion.Models.Respuesta { Exito = true, RegistrosProcesados = listaRegistros.Count });
                }
                else
                {
                    Evaluacion.Helper.FormatoHelper.BorrarArchivo(this.RutaCompletaArchivo);
                    return Json(new Evaluacion.Models.Respuesta
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

        private void ValidarRegistros(List<EprCargaMasivaTransformadorDTO> listaRegistros)
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
                    var res = servicioTransformador.ValidarCargaMasivaTransformador(registro);

                    registro.Error = !string.IsNullOrEmpty(res) ? res : "";
                }
            }

        }

        public JsonResult ExportarDatos(string equicodi, string codigo, string emprcodi, string equiestado,
          string tipo, string idsubestacion, string idarea, string tension)
        {

            TransformadorModel model = new TransformadorModel();

            try
            {
                base.ValidarSesionJsonResult();

                string fileName = ConstantesEvaluacion.NombrePlantillaExcelExportacionTransformadores;
                string pathOrigen = ConstantesEvaluacion.FolderGestProtec + "/" + ConstantesEvaluacion.Plantilla;
                string pathDestino = FileServer.GetDirectory() + ConstantesEvaluacion.FolderTemporal + "/";

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);


                //validaciones
                if (string.IsNullOrEmpty(equicodi)) equicodi = "0";
                if (string.IsNullOrEmpty(emprcodi)) emprcodi = "0";
                if (string.IsNullOrEmpty(tipo)) tipo = "0";
                if (string.IsNullOrEmpty(idsubestacion)) idsubestacion = "0";
                if (string.IsNullOrEmpty(idarea)) idarea = "0";
                if (string.IsNullOrEmpty(tension)) tension = "0";

                servicioTransformador.GenerarExcelExportar(pathDestino, fileName, equicodi, codigo, emprcodi, equiestado, tipo, idsubestacion, idarea, tension);

                model.Resultado = "1";
                model.NombreArchivo = fileName;
                model.StrMensaje = "";


            }
            catch (Exception ex)
            {
                log.Error("TransformadorController", ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        #endregion

        [HttpPost]
        public JsonResult ConsultaCelda(int IdCelda)
        {
            EprEquipoDTO equipo = equipoProteccion.ObtenerDatoCelda(IdCelda);

            return Json(equipo);
        }

        public JsonResult CalcularTransformador(TransformadorIncluirModificarModel model)
        {
            string resultado = string.Empty;
            try
            {
                EprEquipoDTO oEquipo = null;

                oEquipo = new EprEquipoDTO();
                oEquipo.IdTransformador = Convert.ToInt32(model.Equicodi);
                oEquipo.IdProyecto = model.IdProyecto;

                oEquipo.D1IdCelda = model.D1IdCelda;
                oEquipo.D1CapacidadOnanMva = model.D1CapacidadOnanMva;
                oEquipo.D1CapacidadOnafMva = model.D1CapacidadOnafMva;
                oEquipo.D1CapacidadMva = model.D1CapacidadMva;
                oEquipo.D1CapacidadA = model.D1CapacidadA;
                oEquipo.D1PosicionTcA = model.D1PosicionTcA;
                oEquipo.D1PosicionPickUpA = model.D1PosicionPickUpA;
                oEquipo.D1CapacidadTransmisionA = model.D1CapacidadTransmisionA;
                oEquipo.D1CapacidadTransmisionMva = model.D1CapacidadTransmisionMva;
                oEquipo.D1FactorLimitanteCalc = model.D1FactorLimitanteCalc;
                oEquipo.D1FactorLimitanteFinal = model.D1FactorLimitanteFinal;

                oEquipo.D2IdCelda = model.D2IdCelda;
                oEquipo.D2CapacidadOnanMva = model.D2CapacidadOnanMva;
                oEquipo.D2CapacidadOnafMva = model.D2CapacidadOnafMva;
                oEquipo.D2CapacidadMva = model.D2CapacidadMva;
                oEquipo.D2CapacidadA = model.D2CapacidadA;
                oEquipo.D2PosicionTcA = model.D2PosicionTcA;
                oEquipo.D2PosicionPickUpA = model.D2PosicionPickUpA;
                oEquipo.D2CapacidadTransmisionA = model.D2CapacidadTransmisionA;
                oEquipo.D2CapacidadTransmisionMva = model.D2CapacidadTransmisionMva;
                oEquipo.D2FactorLimitanteCalc = model.D2FactorLimitanteCalc;
                oEquipo.D2FactorLimitanteFinal = model.D2FactorLimitanteFinal;

                oEquipo.D3IdCelda = model.D3IdCelda;
                oEquipo.D3CapacidadOnanMva = model.D3CapacidadOnanMva;
                oEquipo.D3CapacidadOnafMva = model.D3CapacidadOnafMva;
                oEquipo.D3CapacidadMva = model.D3CapacidadMva;
                oEquipo.D3CapacidadA = model.D3CapacidadA;
                oEquipo.D3PosicionTcA = model.D3PosicionTcA;
                oEquipo.D3PosicionPickUpA = model.D3PosicionPickUpA;
                oEquipo.D3CapacidadTransmisionA = model.D3CapacidadTransmisionA;
                oEquipo.D3CapacidadTransmisionMva = model.D3CapacidadTransmisionMva;
                oEquipo.D3FactorLimitanteCalc = model.D3FactorLimitanteCalc;
                oEquipo.D3FactorLimitanteFinal = model.D3FactorLimitanteFinal;

                oEquipo.D4IdCelda = model.D4IdCelda;
                oEquipo.D4CapacidadOnanMva = model.D4CapacidadOnanMva;
                oEquipo.D4CapacidadOnafMva = model.D4CapacidadOnafMva;
                oEquipo.D4CapacidadMva = model.D4CapacidadMva;
                oEquipo.D4CapacidadA = model.D4CapacidadA;
                oEquipo.D4PosicionTcA = model.D4PosicionTcA;
                oEquipo.D4PosicionPickUpA = model.D4PosicionPickUpA;
                oEquipo.D4CapacidadTransmisionA = model.D4CapacidadTransmisionA;
                oEquipo.D4CapacidadTransmisionMva = model.D4CapacidadTransmisionMva;
                oEquipo.D4FactorLimitanteCalc = model.D4FactorLimitanteCalc;
                oEquipo.D4FactorLimitanteFinal = model.D4FactorLimitanteFinal;
                oEquipo.FamCodigo = model.FamCodigo;

                oEquipo.D1Tension = model.D1Tension;
                oEquipo.D2Tension = model.D2Tension;
                oEquipo.D3Tension = model.D3Tension;
                oEquipo.D4Tension = model.D4Tension;

                var calculosEquipo = calculo.ListCalculosFormulasTransformadores(oEquipo);
                EvaluacionHelperCalculos.n_calc.EvaluarFormulas(calculosEquipo);

                #region Devanado 1

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D1_CAPACIDAD_MVA && p.Estado == 1))
                {
                    model.D1CapacidadMva = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D1_CAPACIDAD_MVA && p.Estado == 1).Valor, 2);
                }
                else
                {
                    model.D1CapacidadMva = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D1_CAPACIDAD_A && p.Estado == 1))
                {
                    model.D1CapacidadA = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D1_CAPACIDAD_A && p.Estado == 1).Valor, 2);
                }
                else
                {
                    model.D1CapacidadA = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D1_CAPACIDAD_TRANSMISION_A && p.Estado == 1))
                {
                    model.D1CapacidadTransmisionA = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D1_CAPACIDAD_TRANSMISION_A && p.Estado == 1).Valor, 2);
                }
                else
                {
                    model.D1CapacidadTransmisionA = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D1_CAPACIDAD_TRANSMISION_MVA && p.Estado == 1))
                {
                    model.D1CapacidadTransmisionMva = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D1_CAPACIDAD_TRANSMISION_MVA && p.Estado == 1).Valor, 2);
                }
                else
                {
                    model.D1CapacidadTransmisionMva = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D1_FACTOR_LIMITANTE_CALC && p.Estado == 1))
                {
                    model.D1FactorLimitanteCalc = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D1_FACTOR_LIMITANTE_CALC && p.Estado == 1).Valor.ToString();
                }
                else
                {
                    model.D1FactorLimitanteCalc = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D1_FACTOR_LIMITANTE_FINAL && p.Estado == 1))
                {
                    model.D1FactorLimitanteFinal = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D1_FACTOR_LIMITANTE_FINAL && p.Estado == 1).Valor.ToString();
                }
                else
                {
                    model.D1FactorLimitanteFinal = "";
                }

                #endregion

                #region Devanado 2

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D2_CAPACIDAD_MVA && p.Estado == 1))
                {
                    model.D2CapacidadMva = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D2_CAPACIDAD_MVA && p.Estado == 1).Valor, 2);
                }
                else
                {
                    model.D2CapacidadMva = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D2_CAPACIDAD_A && p.Estado == 1))
                {
                    model.D2CapacidadA = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D2_CAPACIDAD_A && p.Estado == 1).Valor, 2);
                }
                else
                {
                    model.D2CapacidadA = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D2_CAPACIDAD_TRANSMISION_A && p.Estado == 1))
                {
                    model.D2CapacidadTransmisionA = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D2_CAPACIDAD_TRANSMISION_A && p.Estado == 1).Valor, 2);
                }
                else
                {
                    model.D2CapacidadTransmisionA = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D2_CAPACIDAD_TRANSMISION_MVA && p.Estado == 1))
                {
                    model.D2CapacidadTransmisionMva = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D2_CAPACIDAD_TRANSMISION_MVA && p.Estado == 1).Valor, 2);
                }
                else
                {
                    model.D2CapacidadTransmisionMva = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D2_FACTOR_LIMITANTE_CALC && p.Estado == 1))
                {
                    model.D2FactorLimitanteCalc = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D2_FACTOR_LIMITANTE_CALC && p.Estado == 1).Valor.ToString();
                }
                else
                {
                    model.D2FactorLimitanteCalc = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D2_FACTOR_LIMITANTE_FINAL && p.Estado == 1))
                {
                    model.D2FactorLimitanteFinal = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D2_FACTOR_LIMITANTE_FINAL && p.Estado == 1).Valor.ToString();
                }
                else
                {
                    model.D2FactorLimitanteFinal = "";
                }


                #endregion

                #region Devanado 3

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D3_CAPACIDAD_MVA && p.Estado == 1))
                {
                    model.D3CapacidadMva = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D3_CAPACIDAD_MVA && p.Estado == 1).Valor, 2);
                }
                else
                {
                    model.D3CapacidadMva = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D3_CAPACIDAD_A && p.Estado == 1))
                {
                    model.D3CapacidadA = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D3_CAPACIDAD_A && p.Estado == 1).Valor, 2);
                }
                else
                {
                    model.D3CapacidadA = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D3_CAPACIDAD_TRANSMISION_A && p.Estado == 1))
                {
                    model.D3CapacidadTransmisionA = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D3_CAPACIDAD_TRANSMISION_A && p.Estado == 1).Valor, 2);
                }
                else
                {
                    model.D3CapacidadTransmisionA = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D3_CAPACIDAD_TRANSMISION_MVA && p.Estado == 1))
                {
                    model.D3CapacidadTransmisionMva = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D3_CAPACIDAD_TRANSMISION_MVA && p.Estado == 1).Valor, 2);
                }
                else
                {
                    model.D3CapacidadTransmisionMva = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D3_FACTOR_LIMITANTE_CALC && p.Estado == 1))
                {
                    model.D3FactorLimitanteCalc = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D3_FACTOR_LIMITANTE_CALC && p.Estado == 1).Valor.ToString();
                }
                else
                {
                    model.D3FactorLimitanteCalc = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D3_FACTOR_LIMITANTE_FINAL && p.Estado == 1))
                {
                    model.D3FactorLimitanteFinal = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D3_FACTOR_LIMITANTE_FINAL && p.Estado == 1).Valor.ToString();
                }
                else
                {
                    model.D3FactorLimitanteFinal = "";
                }


                #endregion

                #region Devanado 4

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D4_CAPACIDAD_MVA && p.Estado == 1))
                {
                    model.D4CapacidadMva = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D4_CAPACIDAD_MVA && p.Estado == 1).Valor, 2);
                }
                else
                {
                    model.D4CapacidadMva = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D4_CAPACIDAD_A && p.Estado == 1))
                {
                    model.D4CapacidadA = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D4_CAPACIDAD_A && p.Estado == 1).Valor, 2);
                }
                else
                {
                    model.D4CapacidadA = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D4_CAPACIDAD_TRANSMISION_A && p.Estado == 1))
                {
                    model.D4CapacidadTransmisionA = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D4_CAPACIDAD_TRANSMISION_A && p.Estado == 1).Valor, 2);
                }
                else
                {
                    model.D4CapacidadTransmisionA = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D4_CAPACIDAD_TRANSMISION_MVA && p.Estado == 1))
                {
                    model.D4CapacidadTransmisionMva = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D4_CAPACIDAD_TRANSMISION_MVA && p.Estado == 1).Valor, 2);
                }
                else
                {
                    model.D4CapacidadTransmisionMva = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D4_FACTOR_LIMITANTE_CALC && p.Estado == 1))
                {
                    model.D4FactorLimitanteCalc = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D4_FACTOR_LIMITANTE_CALC && p.Estado == 1).Valor.ToString();
                }
                else
                {
                    model.D4FactorLimitanteCalc = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D4_FACTOR_LIMITANTE_FINAL && p.Estado == 1))
                {
                    model.D4FactorLimitanteFinal = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NOMBRE_D4_FACTOR_LIMITANTE_FINAL && p.Estado == 1).Valor.ToString();
                }
                else
                {
                    model.D4FactorLimitanteFinal = "";
                }


                #endregion

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
