using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Areas.Proteccion.Helper;
using COES.MVC.Intranet.Areas.Proteccion.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.Despacho;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.Transferencias;
using log4net;

namespace COES.MVC.Intranet.Areas.Proteccion.Controllers
{
    public class EquipoProteccionController : BaseController
    {
        /// <summary>
        /// Instancia de clase para el acceso a datos
        /// </summary>

        GrupoDespachoAppServicio servicio = new GrupoDespachoAppServicio();

        EquipamientoAppServicio servicioEquipamiento = new EquipamientoAppServicio();
        AreaAppServicio servicioArea = new AreaAppServicio();
        MigracionesAppServicio migraciones = new MigracionesAppServicio();
        EquipoProteccionAppServicio equipoProteccion = new EquipoProteccionAppServicio();
        ConsultaMedidoresAppServicio consultaMedidores = new ConsultaMedidoresAppServicio();

        private readonly List<EstadoModel> _lsEstadosFlag = new List<EstadoModel>();
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(EquipoProteccionController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public EquipoProteccionController()
        {
            log4net.Config.XmlConfigurator.Configure();
            _lsEstadosFlag.Add(new EstadoModel { EstadoCodigo = "S", EstadoDescripcion = "Sí" });
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
            EquipoProteccionModel modelo = new EquipoProteccionModel();
            modelo.ListaArea = servicioEquipamiento.ListarZonasxNivel(5);
            modelo.listaProyecto = equipoProteccion.ListProyectoProyectoActualizacion(0);
            modelo.listaEstado = equipoProteccion.ListPropCatalogoData(1);
            modelo.equicodi = Session[ConstantesProteccion.EP_Equicodi] != null ? (int)Session[ConstantesProteccion.EP_Equicodi] : 0;
            modelo.nivel = Session[ConstantesProteccion.EP_Nivel] != null ? (int)Session[ConstantesProteccion.EP_Nivel] : 0;
            modelo.celda = Session[ConstantesProteccion.EP_Celda] != null ? Session[ConstantesProteccion.EP_Celda].ToString() : "";
            modelo.rele = Session[ConstantesProteccion.EP_Rele] != null ? Session[ConstantesProteccion.EP_Rele].ToString() : "";
            modelo.idArea = Session[ConstantesProteccion.EP_IdArea] != null ? (int)Session[ConstantesProteccion.EP_IdArea] : 0;
            modelo.nombSubestacion = Session[ConstantesProteccion.EP_NombSubestacion] != null ? Session[ConstantesProteccion.EP_NombSubestacion].ToString() : "";
            modelo.tituloRele = Session[ConstantesProteccion.EP_TituloRele] != null ? Session[ConstantesProteccion.EP_TituloRele].ToString() : "";
            return View(modelo);
        }

        private string formatearFecha(string fecha)
        {
            try
            {
                if (fecha != null && fecha != "")
                {
                    DateTime dFecha = DateTime.Parse(fecha);
                    return dFecha.ToString("dd/MM/yyyy");
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return fecha;
            }

        }
        public ActionResult Editar(int id, string accion,int idArbol, string nivel)
        {
            var model = new EquipoProteccionEditarModel();

            model.listaSubestacion = equipoProteccion.ListSubEstacion();
            model.listaCelda = new List<EprEquipoDTO>();

            model.listaInterruptor = new List<EprEquipoDTO>();

            model.listaEstado = equipoProteccion.ListPropCatalogoData(ConstantesProteccion.CatalogoEstadoEquipoProteccion);
            model.listaTitular = consultaMedidores.ListObtenerEmpresaSEINProtecciones();
            model.listaSistemaRele = equipoProteccion.ListPropCatalogoData(ConstantesProteccion.CatalogoSistemaReleEquipoProteccion);
            model.listaMarca = equipoProteccion.ListPropCatalogoData(ConstantesProteccion.CatalogoMarcaProteccion);
            model.listaTipoUso = equipoProteccion.ListPropCatalogoData(ConstantesProteccion.CatalogoTipoUsoEquipoProteccion);
            model.listaMandoSincronizado = equipoProteccion.ListPropCatalogoData(ConstantesProteccion.CatalogoMandoSincronizadoEquipoProteccion);
            model.listaReleTorcionalImpl = equipoProteccion.ListPropCatalogoData(ConstantesProteccion.CatalogoReleTorcionalImplEquipoProteccion);
            model.listaRelePmuImpl = equipoProteccion.ListPropCatalogoData(ConstantesProteccion.CatalogoReleTorcionalImplEquipoProteccion);
            model.accion = accion;

            if (accion == ConstantesProteccion.Editar || accion == ConstantesProteccion.Consulta)
            {

                model.listaProyecto = equipoProteccion.ListProyectoProyectoActualizacion(id);

                EprEquipoDTO equipo = equipoProteccion.GetByIdEquipoProtec(id);

                EprEquipoDTO celda = equipoProteccion.GetByIdCelda(equipo.IdCelda == null ? 0 : equipo.IdCelda);
                if (celda != null)
                {
                    model.idSubestacion = celda.Areacodi;
                    model.listaCelda = equipoProteccion.ListCelda(celda.Areacodi);
                    model.listaInterruptor = equipoProteccion.ListInterruptor(celda.Areacodi);
                    List<EprAreaDTO> listaSubestacion = equipoProteccion.ListSubEstacion();
                    var se = listaSubestacion.FirstOrDefault(o => o.Areacodi == celda.Areacodi);
                    if (se != null)
                    {
                        model.zona = se.Zona;
                        model.fechaCreacion = formatearFecha(se.Epareafeccreacion);

                        model.fechaActualizacion = formatearFecha(se.Epareafecmodificacion);
                    }
                }
                model.equicodi = id;
                model.idEstado = equipo.Estado;
                model.idSistemaRele = equipo.IdSistermaRele;
                model.idTipoUso = equipo.IdTipoUso;
                model.idTitular = equipo.IdTitular;
                model.idMarca = equipo.IdMarca;
                model.idCelda = equipo.IdCelda;
                model.codigoRele = equipo.Codigo;
                model.idProyecto = equipo.IdProyecto;
                model.modeloRele = equipo.Modelo;
                model.rtcp = equipo.RtcPrimario;
                model.rtcs = equipo.RtcSecundario;
                model.rttp = equipo.RttPrimario;
                model.rtts = equipo.RttSecundario;
                model.controlUmbral = equipo.SobreCI;
                model.controlAsaPmu = equipo.PmuAccion;
                model.pCoordinables = equipo.ProtCondinables;
                model.tensionRele = equipo.Tension;
                model.asTension = equipo.DeltaTension;
                model.asAngulo = equipo.DeltaAngulo;
                model.asFrecuencia = equipo.DeltaFrecuencia;
                model.astU = equipo.SobreTU;
                model.astT = equipo.SobreTT;
                model.astUU = equipo.SobreTUU;
                model.astTT = equipo.SobreTTT;
                model.codigoInterruptor = equipo.IdInterruptorMS;
                model.idMandoSincronizado = equipo.IdMandoSincronizado;
                model.medidaMitigacion = equipo.MedidaMitigacion;
                model.pmuAccion = equipo.RelePmuAccion;
                model.idRelePmuImpl = equipo.RelePmuImpl;
                model.asInterruptor = equipo.IdInterruptor;
                model.idReleTorcionalImpl = equipo.ReleTorsImpl;
                model.fechaRele = formatearFecha(equipo.Fecha);
                model.mCalculo = equipo.MemoriaCalculo;
                model.mCalculoTexto = ProteccionHelper.modificarNombreArchivo(equipo.MemoriaCalculo); 
                model.asActivo = equipo.SincroCheckActivo==null?"N": equipo.SincroCheckActivo;
                model.checkUmbral = equipo.SobreCCheckActivo == null ? "N" : equipo.SobreCCheckActivo;
                model.astActivo = equipo.SobreTCheckActivo == null ? "N" : equipo.SobreTCheckActivo;
                model.checkAsaPmu = equipo.PmuCheckActivo == null ? "N" : equipo.PmuCheckActivo;
            }
            else{

                model.listaProyecto = equipoProteccion.ListProyectoProyectoActualizacion(0);
                if (nivel != null && nivel != "") {
                    var arbolDetalle = equipoProteccion.GetDetalleArbolEquipoProteccion(idArbol, Convert.ToInt32(nivel));
                    if (arbolDetalle != null) {
                        model.idSubestacion = arbolDetalle.Areacodi;
                        model.listaCelda = equipoProteccion.ListCelda(model.idSubestacion);
                        model.listaInterruptor = equipoProteccion.ListInterruptor(model.idSubestacion);
                        List<EprAreaDTO> listaSubestacion = equipoProteccion.ListSubEstacion();
                        var se = listaSubestacion.FirstOrDefault(o => o.Areacodi == model.idSubestacion);
                        if (se != null)
                        {
                            model.zona = se.Zona;
                            model.fechaCreacion = formatearFecha(se.Epareafeccreacion);
                            model.fechaActualizacion = formatearFecha(se.Epareafecmodificacion);
                        }

                        if (arbolDetalle.Celda != null && arbolDetalle.Celda != "")
                        {
                            model.idCelda = Convert.ToInt32(arbolDetalle.Celda);
                        }
                    }
                }
            }

            return View("~/Areas/Proteccion/Views/EquipoProteccion/Editar.cshtml", model);
        }

        public JsonResult SeleccionarSubestacion(int idSubEstacion)
        {
            EprAreaDTO subEstacion = null;
            try
            {
                List<EprAreaDTO> listaSubestacion = equipoProteccion.ListSubEstacion();
                subEstacion = listaSubestacion.FirstOrDefault(o => o.Areacodi == idSubEstacion);
                subEstacion.Epareafeccreacion = formatearFecha(subEstacion.Epareafeccreacion);
                subEstacion.Epareafecmodificacion = formatearFecha(subEstacion.Epareafecmodificacion);
                return Json(subEstacion);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                return Json(subEstacion);
            }
        }

        public JsonResult ObtenerDetalleArbol(int equicodi, int nivel)
        {
            EprEquipoDTO detalleArbol = null;
            try
            {
                detalleArbol = equipoProteccion.GetDetalleArbolEquipoProteccion(equicodi, nivel);

                if (detalleArbol != null) {
                    detalleArbol.MemoriaCalculoTexo = ProteccionHelper.modificarNombreArchivo(detalleArbol.MemoriaCalculo);
                }


                return Json(detalleArbol);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                return Json(detalleArbol);
            }
        }

        public JsonResult ConsultarSubestacion()
        {
            var model = new EquipoProteccionEditarModel();
            try
            {
                model.listaSubestacion = equipoProteccion.ListSubEstacion();
                return Json(model);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                return Json(model);
            }
        }

        public JsonResult SeleccionarProyecto(int idproyecto)
        {

            EprProyectoActEqpDTO proyecto = null;
            try
            {
                var listaProyecto = equipoProteccion.ListProyectoProyectoActualizacion(0);

                proyecto = listaProyecto.Find(o => o.Epproycodi == idproyecto);
                return Json(proyecto);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                return Json(proyecto);
            }
        }

        public EquipoProteccionEditarModel prepararTipoUso(EquipoProteccionEditarModel model)
        {
            switch (model.idTipoUso)
            {
                case "101":
                    model.codigoInterruptor = "";
                    model.idMandoSincronizado = "";
                    model.medidaMitigacion = "";
                    model.idReleTorcionalImpl = "";
                    model.pmuAccion = "";
                    model.idRelePmuImpl = "";
                    break;
                case "102":
                    model.asInterruptor = "";
                    model.asTension = "";
                    model.asAngulo = "";
                    model.asFrecuencia = "";
                    model.controlUmbral = "";
                    model.astU = "";
                    model.astT = "";
                    model.astUU = "";
                    model.astTT = "";
                    model.controlAsaPmu = "";
                    model.medidaMitigacion = "";
                    model.idReleTorcionalImpl = "";
                    model.pmuAccion = "";
                    model.idRelePmuImpl = "";
                    break;

                case "103":
                    model.asInterruptor = "";
                    model.asTension = "";
                    model.asAngulo = "";
                    model.asFrecuencia = "";
                    model.controlUmbral = "";
                    model.astU = "";
                    model.astT = "";
                    model.astUU = "";
                    model.astTT = "";
                    model.controlAsaPmu = "";
                    model.codigoInterruptor = "";
                    model.idMandoSincronizado = "";
                    model.pmuAccion = "";
                    model.idRelePmuImpl = "";
                    break;

                case "104":
                    model.asInterruptor = "";
                    model.asTension = "";
                    model.asAngulo = "";
                    model.asFrecuencia = "";
                    model.controlUmbral = "";
                    model.astU = "";
                    model.astT = "";
                    model.astUU = "";
                    model.astTT = "";
                    model.controlAsaPmu = "";
                    model.codigoInterruptor = "";
                    model.idMandoSincronizado = "";
                    model.medidaMitigacion = "";
                    model.idReleTorcionalImpl = "";

                    break;
            }

            return model;


        }

        /// <summary>
        /// Recibe los archivos de medida de cálculo
        /// </summary>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload(int equicodi)
        {

            EquipoProteccionFileModel model = new EquipoProteccionFileModel();
            string sNombreArchivo = "";
            string sNombreArchivoOriginal = "";
            string path = "";
            string pathTemporal = "";
            try
            {
                if (Request.Files.Count == 1)
                {
                
                    var file = Request.Files[0];
                    var nombreOriginal = file.FileName.Split('.').First();
                    string extension = file.FileName.Split('.').Last();
                    sNombreArchivo = equipoProteccion.GetNombreArchivoFormato(equicodi.ToString(),"", extension, nombreOriginal);
                    sNombreArchivoOriginal = file.FileName;

                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;
                    string fileName = ruta + file.FileName;
                    string ruta2 = string.Format("{0}\\{1}\\", base.PathFiles, ConstantesProteccion.FolderRele);
                    path = FileServer.GetDirectory() + ruta2;

                    file.SaveAs(fileName); //Guarda en temporal con nombfisico
                    pathTemporal = Server.MapPath("~/Uploads/") + file.FileName;

                    FileServer.CreateFolder(base.PathFiles, ConstantesProteccion.FolderGestProtec, "");
                    FileServer.CreateFolder(base.PathFiles, ConstantesProteccion.FolderMemoriaCalculo, "");
                    FileServer.CreateFolder(base.PathFiles, ConstantesProteccion.FolderRele, "");
                    FileServer.UploadFromFileDirectory(pathTemporal, ruta2, sNombreArchivo, string.Empty); //graba en raiz

                    //Elimina el archivo temporal
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }
                }
                model.estado = 1;
                model.nombreArchivo = sNombreArchivo;
                model.nombreArchivoTexto = ProteccionHelper.modificarNombreArchivo(sNombreArchivo);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.estado = -1;
                model.nombreArchivo = sNombreArchivo;
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GuardarEquipo(EquipoProteccionEditarModel model)
        {
            string resultado = string.Empty;
            try
            {
                EprEquipoDTO oEquipo = null;


                if (model.accion == ConstantesProteccion.Editar)
                {
                    oEquipo = equipoProteccion.GetByIdEquipoProtec(model.equicodi);
                    oEquipo.EquiCodiRele = model.equicodi;
                    oEquipo.IdCelda = model.idCelda;
                    oEquipo.IdProyecto = model.idProyecto;
                    oEquipo.Codigo = model.codigoRele;
                    oEquipo.Fecha = model.fechaRele;
                    oEquipo.IdTitular = model.idTitular;
                    oEquipo.Tension = model.tensionRele;
                    oEquipo.IdSistermaRele = model.idSistemaRele;
                    oEquipo.Modelo = model.modeloRele;
                    oEquipo.IdMarca = model.idMarca;
                    oEquipo.IdTipoUso = model.idTipoUso;
                    oEquipo.RtcPrimario = model.rtcp;
                    oEquipo.RtcSecundario = model.rtcs;
                    oEquipo.RttPrimario = model.rttp;
                    oEquipo.RttSecundario = model.rtts;
                    oEquipo.ProtCondinables = model.pCoordinables;

                    oEquipo.SincroCheckActivo = model.asActivo;
                    oEquipo.SobreCCheckActivo = model.checkUmbral;
                    oEquipo.SobreTCheckActivo = model.astActivo;
                    oEquipo.PmuCheckActivo = model.checkAsaPmu;

                    oEquipo.DeltaTension = model.asTension;
                    oEquipo.DeltaAngulo = model.asAngulo;
                    oEquipo.DeltaFrecuencia = model.asFrecuencia;
                    oEquipo.SobreCI = model.controlUmbral;
                    oEquipo.IdInterruptor = model.asInterruptor;
                    oEquipo.SobreTU = model.astU;
                    oEquipo.SobreTT = model.astT;
                    oEquipo.SobreTUU = model.astUU;
                    oEquipo.SobreTTT = model.astTT;

                    oEquipo.PmuAccion = model.controlAsaPmu;
                    oEquipo.IdInterruptorMS = model.codigoInterruptor == null ? "" : model.codigoInterruptor;
                    oEquipo.IdMandoSincronizado = model.idMandoSincronizado;
                    oEquipo.MedidaMitigacion = model.medidaMitigacion;
                    oEquipo.ReleTorsImpl = model.idReleTorcionalImpl;
                    oEquipo.RelePmuAccion = model.pmuAccion;
                    oEquipo.RelePmuImpl = model.idRelePmuImpl;
                    oEquipo.UsuarioAuditoria = User.Identity.Name;
                    oEquipo.MemoriaCalculo = model.mCalculo;

                    oEquipo.Epequiusucreacion = User.Identity.Name;

                    resultado = equipoProteccion.UpdateRele(oEquipo);
                }
                else
                {
                    oEquipo = new EprEquipoDTO();
                    oEquipo.IdCelda = model.idCelda;
                    oEquipo.IdProyecto = model.idProyecto;
                    oEquipo.Codigo = model.codigoRele;
                    oEquipo.Fecha = model.fechaRele;
                    oEquipo.IdTitular = model.idTitular;
                    oEquipo.Tension = model.tensionRele;
                    oEquipo.IdSistermaRele = model.idSistemaRele;
                    oEquipo.Modelo = model.modeloRele;
                    oEquipo.IdMarca = model.idMarca;
                    oEquipo.IdTipoUso = model.idTipoUso;
                    oEquipo.RtcPrimario = model.rtcp;
                    oEquipo.RtcSecundario = model.rtcs;
                    oEquipo.RttPrimario = model.rttp;
                    oEquipo.RttSecundario = model.rtts;
                    oEquipo.ProtCondinables = model.pCoordinables;
                    oEquipo.DeltaTension = model.asTension;
                    oEquipo.DeltaAngulo = model.asAngulo;
                    oEquipo.DeltaFrecuencia = model.asFrecuencia;
                    oEquipo.SobreCI = model.controlUmbral;
                    oEquipo.IdInterruptor = model.asInterruptor;
                    oEquipo.SincroCheckActivo = model.asActivo; 
                    oEquipo.SobreCCheckActivo = model.checkUmbral;
                    oEquipo.SobreTCheckActivo = model.astActivo;
                    oEquipo.PmuCheckActivo = model.checkAsaPmu;
                    oEquipo.SobreTU = model.astU;
                    oEquipo.SobreTT = model.astT;
                    oEquipo.SobreTUU = model.astUU;
                    oEquipo.SobreTTT = model.astTT;
                    oEquipo.PmuAccion = model.controlAsaPmu;
                    oEquipo.IdInterruptorMS = model.codigoInterruptor == null ? "" : model.codigoInterruptor;
                    oEquipo.IdMandoSincronizado = model.idMandoSincronizado;
                    oEquipo.MedidaMitigacion = model.medidaMitigacion;
                    oEquipo.ReleTorsImpl = model.idReleTorcionalImpl;
                    oEquipo.RelePmuAccion = model.pmuAccion;
                    oEquipo.RelePmuImpl = model.idRelePmuImpl;
                    oEquipo.UsuarioAuditoria = User.Identity.Name;
                    oEquipo.MemoriaCalculo = model.mCalculo;
                    oEquipo.Epequiusucreacion = User.Identity.Name;
                    resultado = equipoProteccion.SaveRele(oEquipo);
                }

                if (resultado != "")
                {
                    return Json("Ocurrio un error: " + resultado);
                }
                else {
                    return Json(resultado);
                }

                
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                resultado = "Ocurrio un error";
                return Json(resultado);
            }
        }

        [ActionName("Index"), HttpPost]
        public ActionResult IndexPost(UbicacionCOESModel datos)
        {
            return View(datos);
        }


        [HttpPost]
        public PartialViewResult ListaProyectos(int equicodi, int nivel, string celda, string rele, int idArea, string nombSubestacion, string tituloRele)
        {
            Session[ConstantesProteccion.EP_Equicodi] = equicodi;
            Session[ConstantesProteccion.EP_Nivel] = nivel;
            Session[ConstantesProteccion.EP_Celda] = celda;
            Session[ConstantesProteccion.EP_Rele] = rele;
            Session[ConstantesProteccion.EP_IdArea] = idArea;
            Session[ConstantesProteccion.EP_NombSubestacion] = nombSubestacion;
            Session[ConstantesProteccion.EP_TituloRele] = tituloRele.Replace("Nivel: ","");
            

            ListadoEquipoProteccionModelModel model = new ListadoEquipoProteccionModelModel();
            var lista= equipoProteccion.ListEquipoProtGrilla(equicodi, nivel, celda, rele, idArea, nombSubestacion).ToList();
            List<EprEquipoDTO> listaModificada = new List<EprEquipoDTO>();
            foreach (var item in lista)
            {
                item.MemoriaCalculoTexo = ProteccionHelper.modificarNombreArchivo(item.MemoriaCalculo);
                listaModificada.Add(item);
            }

            model.ListaEquipoProteccion = listaModificada;
            return PartialView("~/Areas/Proteccion/Views/EquipoProteccion/ListaEquipoProteccion.cshtml", model);
        }

        [HttpPost]
        public JsonResult ListaCelda(int idSubEstacion)
        {
            return Json(equipoProteccion.ListCelda(idSubEstacion));
        }

        [HttpPost]
        public JsonResult ListaInterruptor(int idSubEstacion)
        {
            return Json(equipoProteccion.ListInterruptor(idSubEstacion));
        }

        [HttpPost]
        public PartialViewResult LineaTiempo(int equicodi)
        {
            ListadoLineaTiempoModel model = new ListadoLineaTiempoModel();
            var listLineTiempo = equipoProteccion.ListLineaTiempo(equicodi);
            var listaLineaTiempoRender = new List<EprEquipoDTO>();
            foreach (var item in listLineTiempo)
            {
                var spl = item.ProyectoDesc.Split('|');
                if (spl.Length == 2) {
                    item.ProyectoDesc = spl[0];
                    item.Modelo = spl[1];
                }

                listaLineaTiempoRender.Add(item);
            }
            model.ListaLineaTiempo = listaLineaTiempoRender;


            Response.ContentEncoding = System.Text.Encoding.UTF8;
            return PartialView("~/Areas/Proteccion/Views/EquipoProteccion/LineaTiempo.cshtml", model);
        }

        /// <summary>
        /// Permite visualizar el arbol
        /// </summary>
        /// <param name="mensajes"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public PartialViewResult Arbol(int idZona, string ubicacion)
        {

            List<EprEquipoDTO> lst = equipoProteccion.ListArbol(idZona, ubicacion);

            List<PrGrupoDTO> list = lst.Select(o => new PrGrupoDTO
            {
                Grupopadre = o.Equicodipadre,
                Grupocodi = o.Equicodi ?? 0,
                Gruponomb = o.Equinomb == null ? "": o.Equinomb.Trim(),
                Catecodi = o.Nivel,
            }).ToList();

            ViewBag.ArbolGrupo = ProteccionHelper.ObtenerArbolGrupo(list);

            return PartialView("~/Areas/Proteccion/Views/EquipoProteccion/Arbol.cshtml");
        }

        


           public JsonResult EditarCambioEstado(int equicodi)
        {
            CambioEstadoEditModel model = new CambioEstadoEditModel();
            try
            {
                EqPropequiDTO eqPropequi = equipoProteccion.GetIdCambioEstado(equicodi);
                if (eqPropequi != null)
                {
                    model.IdEstado = convertirNumeros(eqPropequi.Valor);
                    model.IdProyecto = eqPropequi.Epproycodi;
                    model.Fecha =  eqPropequi.Fechapropequi==null? "" : eqPropequi.Fechapropequi?.Date.ToString("dd/MM/yyyy");
                    model.listaProyecto =  equipoProteccion.ListProyectoProyectoActualizacion(equicodi);

                }
           

                    return Json(model);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.MensajeError = ex.ToString();
                return Json(model);
            }
        }

        private int convertirNumeros(string valor) {
            try
            {
                return Convert.ToInt32(valor);
            }
            catch (Exception)
            {

                return 0;
            }
        }

        public JsonResult GuardarCambiarEstado(string iNuevosEstados, string iFecha, string iMotivo, int iCodigo, int iEpproycodi)
        {
            string resultado = string.Empty;
            try
            {
                ListadoProyectoModel model = new ListadoProyectoModel();

                EqPropequiDTO eqPropequi = new EqPropequiDTO();
                
                eqPropequi = new EqPropequiDTO();
                eqPropequi.Epproycodi = iEpproycodi;
                eqPropequi.Propcodi = 3304;
                eqPropequi.Equicodi = iCodigo;
                eqPropequi.Valor = iNuevosEstados;
                eqPropequi.FechapropequiDesc = iFecha;
                eqPropequi.Propequicomentario = iMotivo;
                eqPropequi.Propequideleted = 0;
                eqPropequi.Propequiusucreacion = User.Identity.Name;
                resultado = equipoProteccion.SaveCambioEstado(eqPropequi);

                if (resultado != "")
                {
                    return Json( resultado);
                }
                else
                {
                    return Json(resultado);
                }

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                resultado = "Ocurrio un error";
                return Json(resultado);
            }
        }

        /// <summary>
        /// Descarga los archivos adjuntados
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="corrcodi"></param>
        /// <returns></returns>
        public virtual ActionResult DescargarManual()
        {
            try
            {
                base.ValidarSesionUsuario();
                string fileName = ConstantesProteccion.ArchivoManualEquipoProteccion;
                byte[] buffer = new ProteccionHelper().GetBufferArchivoAdjunto(0, fileName, base.PathFiles, ConstantesProteccion.FolderManual);
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


        /// <summary>
        /// Descarga los archivos adjuntados
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="corrcodi"></param>
        /// <returns></returns>
        public virtual ActionResult DescargarArchivo(string fileName, int equicodi)
        {
            try
            {
                base.ValidarSesionUsuario();
                byte[] buffer = new ProteccionHelper().GetBufferArchivoAdjunto(equicodi, fileName, base.PathFiles, ConstantesProteccion.FolderRele);
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

        /// <summary>
        /// Descarga los archivos adjuntados
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="epsubecodi"></param>
        /// <returns></returns>
        public virtual ActionResult DescargarArchivoSE(string fileName, int epsubecodi)
        {
            try
            {
                base.ValidarSesionUsuario();
                byte[] buffer = new ProteccionHelper().GetBufferArchivoAdjunto(epsubecodi, fileName, base.PathFiles, ConstantesProteccion.FolderSubestacion);
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


        [HttpPost]
        public JsonResult GenerarReporteRele(int equicodi, int nivel, string celda, string rele, int idArea, string nombSubestacion)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {
                DateTime hoy = DateTime.Now;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string nameFile = "Reporte_Rele_" + hoy.Year + string.Format("{0:D2}", hoy.Month) + string.Format("{0:D2}", hoy.Day) + string.Format("{0:D2}", hoy.Hour) + string.Format("{0:D2}", hoy.Minute) + string.Format("{0:D2}", hoy.Second) + ".xlsx";

                List<EprEquipoDTO> lExportar = equipoProteccion.ReporteEquipoProtGrilla(equicodi, nivel, celda, rele, idArea, nombSubestacion).ToList();

                new ProteccionHelper().GenerarExportacionRele(pathLogo, nameFile, lExportar, base.PathFiles, "LISTADO DETALLADO DE RELÉS");
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
    }
}
