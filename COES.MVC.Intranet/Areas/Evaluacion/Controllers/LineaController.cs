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

using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.Evaluacion;
using EvaluacionHelperCalculos = COES.Servicios.Aplicacion.Evaluacion.Helper;

using COES.MVC.Intranet.Areas.Proteccion.Helper;
using COES.MVC.Intranet.Helper;

using COES.MVC.Intranet.Areas.Evaluacion.Helper;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Helper;


namespace COES.MVC.Intranet.Areas.Evaluacion.Controllers
{
    public class LineaController : BaseController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(LineaController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        EquipamientoAppServicio servicioEquipamiento = new EquipamientoAppServicio();
        EquipoProteccionAppServicio equipoProteccion = new EquipoProteccionAppServicio();
        ConsultaMedidoresAppServicio consultaMedidores = new ConsultaMedidoresAppServicio();
        MigracionesAppServicio migraciones = new MigracionesAppServicio();
        LineaAppServicio linea = new LineaAppServicio();
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

        public LineaController()
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
            LineaModel modelo = new LineaModel();

            modelo.ListaUbicacion = equipoProteccion.ListSubEstacion();

            List<EqFamiliaDTO> listTipoArea = servicioEquipamiento.ListEqFamiliasEquipamientoCOES();
            listTipoArea.Sort((s1, s2) => s1.Famnomb.CompareTo(s2.Famnomb));
            modelo.ListaTipoArea = listTipoArea;
            modelo.listaSubestacion = equipoProteccion.ListSubEstacion();
            modelo.ListaArea = servicioEquipamiento.ListarZonasxNivel(5);
            modelo.listaEmpresa = consultaMedidores.ListObtenerEmpresaSEINProtecciones();
            modelo.listaEstado = equipoProteccion.ListPropCatalogoData(ConstantesEvaluacion.EstadoLinea);

            modelo.equicodi = Session[ConstantesEvaluacion.L_Equicodi] != null ? Session[ConstantesEvaluacion.L_Equicodi].ToString() : "";
            modelo.codigo = Session[ConstantesEvaluacion.L_Codigo] != null ? Session[ConstantesEvaluacion.L_Codigo].ToString() : "";

            if (Session[ConstantesEvaluacion.L_SubEstacion1] != null && !Session[ConstantesEvaluacion.L_SubEstacion1].ToString().Equals(""))
            {
                modelo.subestacion1 = Int32.Parse(Session[ConstantesEvaluacion.L_SubEstacion1].ToString());
            }
            else
            {
                modelo.subestacion1 = 0;
            }

            if (Session[ConstantesEvaluacion.L_SubEstacion2] != null && !Session[ConstantesEvaluacion.L_SubEstacion2].ToString().Equals(""))
            {
                modelo.subestacion2 = Int32.Parse(Session[ConstantesEvaluacion.L_SubEstacion2].ToString());
            }
            else
            {
                modelo.subestacion2 = 0;
            }

            if (Session[ConstantesEvaluacion.L_Area] != null && !Session[ConstantesEvaluacion.L_Area].ToString().Equals(""))
            {
                modelo.area = Int32.Parse(Session[ConstantesEvaluacion.L_Area].ToString());
            }
            else
            {
                modelo.area = 0;
            }

            if (Session[ConstantesEvaluacion.L_Empresa] != null && !Session[ConstantesEvaluacion.L_Empresa].ToString().Equals(""))
            {
                modelo.empresa = Int32.Parse(Session[ConstantesEvaluacion.L_Empresa].ToString());
            }
            else
            {
                modelo.empresa = 0;
            }

            modelo.estado = Session[ConstantesEvaluacion.L_Estado] != null ? Session[ConstantesEvaluacion.L_Estado].ToString() : "";
            modelo.tension = Session[ConstantesEvaluacion.L_Tension] != null ? Session[ConstantesEvaluacion.L_Tension].ToString() : "";
            modelo.incluirCalcular = Session[ConstantesEvaluacion.L_IncluirCalcular] != null ? (int)Session[ConstantesEvaluacion.L_IncluirCalcular] : 0;

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
        public ActionResult IndexPost(LineaModel datos)
        {
            return View(datos);
        }

        public PartialViewResult Incluir(int id)
        {
            var model = new LineaIncluirModel();
            model.listaProyecto = equipoProteccion.ListProyectoProyectoActualizacion(id);
            model.idProyecto = 0;
            return PartialView("~/Areas/Evaluacion/Views/Linea/Incluir.cshtml", model);
        }

        public ActionResult IncluirModificar(int idLinea, int idProyecto, int idEquipo, string accion)
        {
            var model = new LineaIncluirModificarModel();

            model.listaProyecto = equipoProteccion.ListProyectoProyectoActualizacion(idLinea);
            model.listaArea = linea.ListAreaxCelda("0", "0");
            model.listaCelda = equipoProteccion.ListCeldaEvaluacion("0");
            model.listaCelda2 = equipoProteccion.ListCeldaEvaluacion("0");
            model.listaBanco = equipoProteccion.ListBancoEvaluacion();
            model.IdProyecto = idProyecto;
            model.Equicodi = idLinea.ToString();
            model.accion = accion;
            model.IdEquipo = idEquipo;

            EprProyectoActEqpDTO proyecto = servicioProyectoActualizacion.EprProyectoGetById(idProyecto);
            model.Motivo = proyecto.Epproynemotecnico + ": " + proyecto.Epproynomb;
            model.FechaMotivo = proyecto.Epproyfecregistro;

            EprEquipoDTO equipo = servicioTransversal.ObtenerCabeceraEquipoPorId(idEquipo);
            model.Equicodi = equipo.Equicodi.ToString();
            model.Codigo = equipo.Codigo;
            model.Ubicacion = equipo.Ubicacion;
            model.Empresa = equipo.Empresa;

            
            EprEquipoDTO eqpo = linea.GetIdLineaIncluir(idEquipo);
            if (eqpo != null)
            {
                model.Tension = eqpo.Tension;
                model.Longitud = eqpo.Longitud;
                model.CapacidadA = eqpo.CapacidadA;
                model.CapacidadMva = eqpo.CapacidadMva;
                model.IdArea = eqpo.IdArea;
                model.IdCelda = eqpo.IdCelda;
                model.IdCelda2 = eqpo.IdCelda2;
                model.Celda1Posicion = eqpo.CeldaPosicionNucleoTc;
                model.Celda1PickUp = eqpo.CeldaPickUp;
                model.Celda2Posicion = eqpo.Celda2PosicionNucleoTc;
                model.Celda2PickUp = eqpo.Celda2PickUp;
                model.IdBancoCondensador = eqpo.IdBancoCondensador;
                model.CapacTransCond1Porcen = eqpo.CapacTransCond1Porcen;
                model.CapacTransCond1Min = eqpo.CapacTransCond1Min;
                model.CapacTransCond1A = eqpo.CapacTransCond1A;
                model.CapacTransCond2Porcen = eqpo.CapacTransCond2Porcen;
                model.CapacTransCond2Min = eqpo.CapacTransCond2Min;
                model.CapacTransCond2A = eqpo.CapacTransCond2A;
                model.CapacidadTransmisionA = eqpo.CapacidadTransmisionA;
                model.CapacidadTransmisionMva = eqpo.CapacidadTransmisionMva;
                model.LimiteSegCoes = eqpo.LimiteSegCoes;
                model.FactorLimitanteCalc = eqpo.FactorLimitanteCalc;
                model.FactorLimitanteFinal = eqpo.FactorLimitanteFinal;
                model.Observaciones = eqpo.Observaciones;
                model.ActualizadoPor = eqpo.UsuarioAuditoria;
                model.ActualizadoEl = eqpo.Fechamodificacionstr;
                model.UsuarioModificacion = eqpo.UsuarioModificacion;
                model.FechaModificacion = eqpo.FechaModificacion;
                model.CapacidadABancoCondensador = eqpo.CapacidadABancoCondensador;
                model.CapacidadMvarBancoCondensador = eqpo.CapacidadMvarBancoCondensador;


                model.CapacidadABancoCondensadorComent = eqpo.CapacidadABancoCondensadorComent;
                model.CapacidadMvarBancoCondensadorComent = eqpo.CapacidadMvarBancoCondensadorComent;
                model.CapacidadAComent = eqpo.CapacidadAComent;
                model.CapacidadMvaComent = eqpo.CapacidadMvaComent;
                model.CapacTransCond1PorcenComent = eqpo.CapacTransCond1PorcenComent;
                model.CapacTransCond1MinComent = eqpo.CapacTransCond1MinComent;
                model.CapacTransCond1AComent = eqpo.CapacTransCond1AComent;
                model.CapacTransCond2PorcenComent = eqpo.CapacTransCond2PorcenComent;
                model.CapacTransCond2MinComent = eqpo.CapacTransCond2MinComent;
                model.CapacTransCond2AComent = eqpo.CapacTransCond2AComent;
                model.CapacidadTransmisionAComent = eqpo.CapacidadTransmisionAComent;
                model.CapacidadTransmisionMvaComent = eqpo.CapacidadTransmisionMvaComent;
                    

                model.LimiteSegCoesComent = eqpo.LimiteSegCoesComent;
                model.FactorLimitanteCalcComent = eqpo.FactorLimitanteCalcComent;
                model.FactorLimitanteFinalComent = eqpo.FactorLimitanteFinalComent;
            }
            

            return View("~/Areas/Evaluacion/Views/Linea/IncluirModificar.cshtml", model);
        }

        public ActionResult BuscarLinea()
        {
            var model = new BuscarLineaModel();
            model.listaProyecto = new List<EprProyectoActEqpDTO>();
            model.idProyecto = 0;
            model.listaEstado = equipoProteccion.ListPropCatalogoData(ConstantesEvaluacion.EstadoLinea);
            model.listaUbicacion = equipoProteccion.ListSubEstacion();
            model.listaTitular = consultaMedidores.ListObtenerEmpresaSEINProtecciones();
            return View("~/Areas/Evaluacion/Views/Linea/BuscarLinea.cshtml", model);
        }

        [HttpPost]
        public PartialViewResult ListaLinea(string equicodi, string codigo, string emprcodi, string equiestado, string idsuestacion1, string idsuestacion2, string idarea, string tension, int incluirCalcular)
        {           

            Session[ConstantesEvaluacion.L_Equicodi] = equicodi;
            Session[ConstantesEvaluacion.L_Codigo] = codigo;
            Session[ConstantesEvaluacion.L_Empresa] = emprcodi;
            Session[ConstantesEvaluacion.L_Estado] = equiestado;
            Session[ConstantesEvaluacion.L_SubEstacion1] = idsuestacion1;
            Session[ConstantesEvaluacion.L_SubEstacion2] = idsuestacion2;
            Session[ConstantesEvaluacion.L_Area] = idarea;
            Session[ConstantesEvaluacion.L_Tension] = tension;
            Session[ConstantesEvaluacion.L_IncluirCalcular] = incluirCalcular;

            ListadoLineaModel model = new ListadoLineaModel();
            model.listaLineaPrincipal = linea.ListLineaEvaluacionPrincipal(equicodi, codigo, emprcodi, equiestado, idsuestacion1, idsuestacion2, idarea, tension, incluirCalcular);

            int id = (Session[DatosSesion.SesionIdOpcion] != null) ? Convert.ToInt32(Session[DatosSesion.SesionIdOpcion]) : 0;
            bool permisoEdicion = this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, Acciones.Editar, User.Identity.Name);
            bool permisoEliminar = this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, Acciones.Eliminar, User.Identity.Name);

            ViewBag.PermisoEditar = permisoEdicion ? "1" : "0";
            ViewBag.PermisoEliminar = permisoEliminar ? "1" : "0";

            return PartialView("~/Areas/Evaluacion/Views/Linea/ListaLinea.cshtml", model);
        }

        [HttpPost]
        public PartialViewResult BuscarLineaLista(string equiCodi, string codigo, string ubicacion, string emprCodigo, string equiEstado)
        {
            BuscarLineaListaModel model = new BuscarLineaListaModel();
            model.ListaLinea = linea.ListLineaEvaluacion(equiCodi, codigo, ubicacion, emprCodigo, equiEstado).ToList();

            return PartialView("~/Areas/Evaluacion/Views/Linea/BuscarLineaLista.cshtml", model);
        }

        [HttpPost]
        public PartialViewResult EditarComentario(string comentario)
        {
            EditarComentarioModel model = new EditarComentarioModel();
            model.Comentario = comentario;

            return PartialView("~/Areas/Evaluacion/Views/Linea/EditarComentario.cshtml", model);
        }

        [HttpPost]
        public JsonResult GuardarLinea(LineaIncluirModificarModel model)
        {
            string resultado = string.Empty;
            try
            {
                EprEquipoDTO oEquipo = new EprEquipoDTO();
                oEquipo.IdLinea = Convert.ToInt32(model.Equicodi);
                oEquipo.IdProyecto = model.IdProyecto;
                oEquipo.Fecha = model.FechaMotivo;
                oEquipo.IdArea = model.IdArea;
                oEquipo.CapacidadA = model.CapacidadA;
                oEquipo.CapacidadMva = model.CapacidadMva;
                oEquipo.IdCelda = model.IdCelda;
                oEquipo.IdCelda2 = model.IdCelda2;
                oEquipo.IdBancoCondensador = model.IdBancoCondensador;
                oEquipo.CapacidadABancoCondensador = model.CapacidadABancoCondensador;
                oEquipo.CapacidadMvarBancoCondensador = model.CapacidadMvarBancoCondensador;
                oEquipo.CapacTransCond1Porcen = model.CapacTransCond1Porcen;
                oEquipo.CapacTransCond1Min = model.CapacTransCond1Min;
                oEquipo.CapacTransCond1A = model.CapacTransCond1A;
                oEquipo.CapacTransCond2Porcen = model.CapacTransCond2Porcen;
                oEquipo.CapacTransCond2Min = model.CapacTransCond2Min;
                oEquipo.CapacTransCond2A = model.CapacTransCond2A;
                oEquipo.CapacidadTransmisionA = model.CapacidadTransmisionA;
                oEquipo.CapacidadTransmisionMva = model.CapacidadTransmisionMva;
                oEquipo.LimiteSegCoes = model.LimiteSegCoes;
                oEquipo.FactorLimitanteCalc = model.FactorLimitanteCalc;
                oEquipo.FactorLimitanteFinal = model.FactorLimitanteFinal;
                
                oEquipo.Observaciones = model.Observaciones;
                oEquipo.UsuarioAuditoria = User.Identity.Name;


                oEquipo.CapacidadAComent = model.CapacidadAComent;
                oEquipo.CapacidadMvaComent = model.CapacidadMvaComent;
                oEquipo.CapacidadABancoCondensadorComent = model.CapacidadABancoCondensadorComent;
                oEquipo.CapacidadMvarBancoCondensadorComent = model.CapacidadMvarBancoCondensadorComent;                
                oEquipo.CapacTransCond1PorcenComent = model.CapacTransCond1PorcenComent;
                oEquipo.CapacTransCond1MinComent = model.CapacTransCond1MinComent;
                oEquipo.CapacTransCond1AComent = model.CapacTransCond1AComent;
                oEquipo.CapacTransCond2PorcenComent = model.CapacTransCond2PorcenComent;
                oEquipo.CapacTransCond2MinComent = model.CapacTransCond2MinComent;
                oEquipo.CapacTransCond2AComent = model.CapacTransCond2AComent;
                oEquipo.CapacidadTransmisionAComent = model.CapacidadTransmisionAComent;
                oEquipo.CapacidadTransmisionMvaComent = model.CapacidadTransmisionMvaComent;
                

                oEquipo.FactorLimitanteCalcComent = model.FactorLimitanteCalcComent;
                oEquipo.LimiteSegCoesComent = model.LimiteSegCoesComent;
                oEquipo.FactorLimitanteFinalComent = model.FactorLimitanteFinalComent;


                resultado = linea.RegistrarLinea(oEquipo);


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
        public JsonResult ConsultaCelda(int IdCelda)
        {
            EprEquipoDTO equipo = equipoProteccion.ObtenerDatoCelda(IdCelda);

            return Json(equipo);
        }

        [HttpPost]
        public JsonResult ConsultaBanco(int IdBanco)
        {
            List<EprEquipoDTO> lst = equipoProteccion.ListBancoEvaluacion();
            EprEquipoDTO equipo = new EprEquipoDTO();
            if (lst.Count > 0 && IdBanco > 0)
            {
                equipo = lst.Find(o => o.Equicodi == IdBanco);
            }

            return Json(equipo);
        }


        #region Carga Masiva

        public JsonResult ExportarPlantilla()
        {
            ReactorModel model = new ReactorModel();

            try
            {
                base.ValidarSesionJsonResult();

                string fileName = ConstantesEvaluacion.NombrePlantillaExcelLinea;
                string pathOrigen = ConstantesEvaluacion.FolderGestProtec + "/" + ConstantesEvaluacion.Plantilla;                
                string pathDestino = FileServer.GetDirectory() + ConstantesEvaluacion.FolderTemporal + "/";

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);

                linea.GenerarExcelPlantilla(pathDestino, fileName);

                model.Resultado = "1";
                model.NombreArchivo = fileName;
                model.StrMensaje = "";


            }
            catch (Exception ex)
            {
                log.Error("LineaController", ex);
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
                Evaluacion.Models.Respuesta matrizValida = new Evaluacion.Models.Respuesta();
                               

                var titulos = 18;
                listaRegistros = Evaluacion.Helper.FormatoHelper.LeerExcelCargado(this.RutaCompletaArchivo, titulos, out matrizValida);

                if (matrizValida.Exito && listaRegistros.Count > 0)
                {
                    //Validacion
                    ValidarRegistros(listaRegistros);

                    if (listaRegistros.Any(p => p.Error.Length > 0))
                    {
                        registroObservados = listaRegistros.Count(p => p.Error.Length > 0);
                    }

                    //Si no hay errores se graba
                    if(registroObservados == 0)
                    {
                        foreach (var registro in listaRegistros)
                        {
                            registro.NombreUsuario = User.Identity.Name;

                            var res = linea.SaveCargaMasivaLinea(registro);

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
                            ListaErrores = listaRegistros.Where(p => p.Error.Length > 0).ToList(),
                            Mensaje = ""
                        }) ;
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
                    var res = linea.ValidarCargaMasivaLinea(registro);

                    registro.Error = !string.IsNullOrEmpty(res) ? res : "";
                }
            }

        }

      
        public JsonResult ExportarDatos(string equicodi, string codigo, string emprcodi, string equiestado, 
            string idsuestacion1, string idsuestacion2, string idarea, string tension)
        {
           
            LineaModel model = new LineaModel();

            try
            {
                base.ValidarSesionJsonResult();

                string fileName = ConstantesEvaluacion.NombrePlantillaExcelExportacionLinea;
                string pathOrigen = ConstantesEvaluacion.FolderGestProtec + "/" + ConstantesEvaluacion.Plantilla;               
                string pathDestino = FileServer.GetDirectory() + ConstantesEvaluacion.FolderTemporal + "/";

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);


                //validaciones
                if (string.IsNullOrEmpty(equicodi)) equicodi = "0";
                if (string.IsNullOrEmpty(emprcodi)) emprcodi = "0";
                if (string.IsNullOrEmpty(idsuestacion1)) idsuestacion1 = "0";
                if (string.IsNullOrEmpty(idsuestacion2)) idsuestacion2 = "0";
                if (string.IsNullOrEmpty(idarea)) idarea = "0";
                if (string.IsNullOrEmpty(tension)) tension = "0";

                linea.GenerarExcelExportar(pathDestino, fileName, equicodi, codigo, emprcodi, equiestado, idsuestacion1, idsuestacion2, idarea, tension);

                model.Resultado = "1";
                model.NombreArchivo = fileName;
                model.StrMensaje = "";


            }
            catch (Exception ex)
            {
                log.Error("LineaController", ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        #endregion


        [HttpPost]
        public JsonResult GenerarReporteDesdePlantilla()
        {
            base.ValidarSesionUsuario();

            string rspta = "-1";
            string file = string.Empty;
            string path = ConstantesEvaluacion.FolderTemporal;
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
            string pathPlantilla = ConstantesEvaluacion.FolderGestProtec + "\\" + ConstantesEvaluacion.Plantilla;
            string pathArchivoPlantilla = FileServer.GetDirectory() + pathPlantilla + ConstantesEvaluacion.NombrePlantillaWord;
            string extension = ConstantesEvaluacion.NombrePlantillaWord.Split('.').Last();
            string nombreCortado = ConstantesEvaluacion.NombrePlantillaWord.Split('.').First();
            string fecha = DateTime.Now.ToString("yyyyMMddhhmm");
            string nombreDinamico = nombreCortado  +"_"+ fecha + "."+ extension;
            //copiando plantilla a reporte
            FileServer.CopiarFileRename(pathPlantilla, ConstantesEvaluacion.FolderTemporal + "/", ConstantesEvaluacion.NombrePlantillaWord, base.PathFiles, nombreDinamico);


            file = ConstantesEvaluacion.NombrePlantillaWord;
            linea.GenerarReporteDesdePlantilla(nombreDinamico, pathLogo, path);


            rspta = nombreDinamico;

            return Json(rspta);
        }


        /// <summary>
        /// Descarga los archivos adjuntados
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual ActionResult DescargarArchivo(string fileName)
        {
            try
            {
                base.ValidarSesionUsuario();
                byte[] buffer = new EvaluacionHelper().GetBufferArchivoAdjunto( fileName, base.PathFiles, ConstantesEvaluacion.FolderTemporal);
                return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                return null;
            }
        }

        public JsonResult CalcularLinea(LineaIncluirModificarModel model)
        {
            //string resultado = string.Empty;
            try
            {
                EprEquipoDTO oEquipo = null;
                
                oEquipo = new EprEquipoDTO();
                oEquipo.IdLinea = Convert.ToInt32(model.Equicodi);
                oEquipo.IdProyecto = model.IdProyecto;
               
                oEquipo.IdArea = model.IdArea;
                oEquipo.CapacidadA = model.CapacidadA;
                oEquipo.CapacidadMva = model.CapacidadMva;
                oEquipo.IdCelda = model.IdCelda;
                oEquipo.IdCelda2 = model.IdCelda2;
                oEquipo.IdBancoCondensador = model.IdBancoCondensador;
                oEquipo.CapacTransCond1Porcen = model.CapacTransCond1Porcen;
                oEquipo.CapacTransCond1Min = model.CapacTransCond1Min;
                oEquipo.CapacTransCond1A = model.CapacTransCond1A;
                oEquipo.CapacTransCond2Porcen = model.CapacTransCond2Porcen;
                oEquipo.CapacTransCond2Min = model.CapacTransCond2Min;
                oEquipo.CapacTransCond2A = model.CapacTransCond2A;
                oEquipo.CapacidadTransmisionA = model.CapacidadTransmisionA;
                oEquipo.CapacidadTransmisionMva = model.CapacidadTransmisionMva;
                oEquipo.LimiteSegCoes = model.LimiteSegCoes;
                oEquipo.FactorLimitanteCalc = model.FactorLimitanteCalc;
                oEquipo.FactorLimitanteFinal = model.FactorLimitanteFinal;
                oEquipo.Tension = model.Tension;

                var calculosEquipo = calculo.ListCalculosFormulasLinea(oEquipo, 1);
                EvaluacionHelperCalculos.n_calc.EvaluarFormulas(calculosEquipo);

                if(calculosEquipo.Any(p=>p.Identificador.ToUpper()== ConstantesEvaluacion.NombreCapacidadMva && p.Estado == 1))
                {                    
                    model.CapacidadMva = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NombreCapacidadMva && p.Estado == 1).Valor);
                }
                else
                {
                    model.CapacidadMva = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NombreCapacTransCond1A && p.Estado == 1))
                {
                    model.CapacTransCond1A = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NombreCapacTransCond1A && p.Estado == 1).Valor, 2);
                }
                else
                {
                    model.CapacTransCond1A = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NombreCapacTransCond2A && p.Estado == 1))
                {
                    model.CapacTransCond2A = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NombreCapacTransCond2A && p.Estado == 1).Valor, 2);
                }
                else
                {
                    model.CapacTransCond2A = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NombreCapacidadTransmisionA && p.Estado == 1))
                {
                    model.CapacidadTransmisionA = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NombreCapacidadTransmisionA && p.Estado == 1).Valor);
                }
                else
                {
                    model.CapacidadTransmisionA = "";
                }

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NombreCapacidadTransmisionMVA && p.Estado == 1))
                {
                    model.CapacidadTransmisionMva = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacion.NombreCapacidadTransmisionMVA && p.Estado == 1).Valor);
                }
                else
                {
                    model.CapacidadTransmisionMva = "";
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

        [HttpPost]
        public JsonResult ConsultaAreaxCelda(string celda1, string celda2)
        {
            List<EprAreaDTO> list = linea.ListAreaxCelda(celda1, celda2);

            return Json(list);
        }

    }

}
