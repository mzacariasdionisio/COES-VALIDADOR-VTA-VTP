using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.ReportesMedicion.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.ReportesMedicion;
using COES.Servicios.Aplicacion.ReportesMedicion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.CPPA.Helper;
using System.Configuration;

namespace COES.MVC.Intranet.Areas.ReportesMedicion.Controllers
{
    public class FormatoReporteController : BaseController
    {
        public FormatoReporteAppServicio servicio = new FormatoReporteAppServicio();

        #region Declaracion de variables de Sesión
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Excepciones ocurridas en el controlador
        /// </summary>
        /// <param name="filterContext"></param>
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

        public FormatoReporteController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #endregion

        /// <summary>
        /// Index de inicio de controller FormatoReporte
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FormatoReporteModel model = new FormatoReporteModel();
            model.ListaAreasCoes = servicio.ListFwAreas();
            model.ListaModulos = this.servicio.ListFwModulo();
            return View(model);
        }

        #region Reporte

        /// <summary>
        /// Devuelve vista parcial para mostrar listado de reportes
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public PartialViewResult ListaReporte(int idarea, int idmodulo)
        {
            if (!base.IsValidSesion) throw new Exception(Constantes.MensajeSesionExpirado);

            FormatoReporteModel model = new FormatoReporteModel();
            model.ListaReporte = this.servicio.ListarReporteByAreaAndModulo(idarea, idmodulo);

            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar la vista para la creacion de Reporte
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult AgregarReporte()
        {
            string codigo = "0";
            if (Request["id"] != null)
                codigo = Request["id"];
            int id = int.Parse(codigo);
            FormatoReporteModel model = new FormatoReporteModel();

            model.IdReporte = id;
            model.IdModulo = 0;
            model.IdLectura = 0;
            model.IdCabecera = 0;
            model.CheckEmpresa = 0;
            model.CheckEquipo = 0;
            model.CheckTipoEquipo = 0;
            model.CheckTipoMedida = 0;
            model.IdArea = 0;
            model.Reptiprepcodi = 0;
            model.Mrepcodi = 0;
            model.CheckEsGrafico = 0;

            if (id > 0) //Edicion de Reporte
            {
                var reporte = this.servicio.GetByIdReporte(id);
                if (reporte != null)
                {
                    model.IdReporte = reporte.Reporcodi;
                    model.Nombre = reporte.Repornombre;
                    model.Descripcion = reporte.Repordescrip;
                    model.IdModulo = reporte.Modcodi;
                    model.IdCabecera = reporte.Cabcodi;
                    model.IdArea = reporte.Areacode;
                    model.IdLectura = (int)reporte.Lectcodi;
                    model.CheckEmpresa = reporte.Reporcheckempresa;
                    model.CheckEquipo = reporte.Reporcheckequipo;
                    model.CheckTipoEquipo = reporte.Reporchecktipoequipo;
                    model.CheckTipoMedida = reporte.Reporchecktipomedida;
                    model.Reptiprepcodi = reporte.Reptiprepcodi;
                    model.Mrepcodi = reporte.Mrepcodi.GetValueOrDefault(0);
                    model.NombreEjeY = reporte.Reporejey;
                    model.CheckEsGrafico = (reporte.Reporesgrafico != null) ? (reporte.Reporesgrafico == Constantes.SI ? 1 : 0) : 0;
                }
            }

            model.ListaLectura = this.servicio.ListMeLecturas();
            model.ListaCabecera = this.servicio.GetListMeCabecera();
            model.ListaModulos = this.servicio.ListFwModulo();
            model.ListaAreasCoes = this.servicio.ListFwAreas();
            model.ListaTipoReporte = this.servicio.GetListaMenuReporteTipo();
            model.ListaMenuReporte = servicio.GetListaMenuReporte(model.Reptiprepcodi);

            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar items de proyectos segun el tipo de reporte y proyecto
        /// </summary>
        /// <param name="indicador"></param>
        /// <returns></returns>
        public JsonResult CargarReporteItems(int id)
        {
            List<SiMenureporteDTO> MenuReporte = new List<SiMenureporteDTO>();
            MenuReporte = servicio.GetListaMenuReporte(id);

            return Json(MenuReporte);
        }

        /// <summary>
        /// Graba formato en BD
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarReporte(FormatoReporteModel model)
        {
            int resultado = 0;
            MeReporteDTO reporte = new MeReporteDTO();
            reporte.Repornombre = model.Nombre;
            reporte.Repordescrip = model.Descripcion;
            reporte.Lectcodi = model.IdLectura;
            reporte.Modcodi = model.IdModulo;
            reporte.Cabcodi = model.IdCabecera;
            reporte.Areacode = model.IdArea;
            reporte.Reporcheckequipo = model.CheckEquipo;
            reporte.Reporcheckempresa = model.CheckEmpresa;
            reporte.Reporchecktipoequipo = model.CheckTipoEquipo;
            reporte.Reporchecktipomedida = model.CheckTipoMedida;
            reporte.Reporusucreacion = User.Identity.Name;
            reporte.Reporfeccreacion = DateTime.Now;
            reporte.Reporusumodificacion = User.Identity.Name;
            reporte.Reporfecmodificacion = DateTime.Now;
            reporte.Reporejey = model.NombreEjeY;
            reporte.Reporesgrafico = model.CheckEsGrafico == 1 ? Constantes.SI : Constantes.NO;


            if (model.Mrepcodi > 0)
            {
                reporte.Mrepcodi = model.Mrepcodi;
            }
            else
            {
                reporte.Mrepcodi = null;
            }

            if (model.IdReporte == 0)
            {
                //Nuevo Reporte
                try
                {
                    int idReporte = this.servicio.SaveMeReporte(reporte);
                    resultado = 1;
                }
                catch (Exception ex)
                {
                    Log.Error(NameController, ex);
                    resultado = -1;
                }
            }
            else
            {
                //Edicion de Reporte
                reporte.Reporcodi = model.IdReporte;
                var find = this.servicio.GetByIdReporte(model.IdReporte);
                if (find != null)
                {
                    try
                    {
                        this.servicio.UpdateMeReporte(reporte);
                        resultado = 1;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(NameController, ex);
                        resultado = -1;
                    }
                }
            }

            return Json(resultado);
        }

        #endregion

        #region Detalle Reporte - Configuración Puntos Medición

        /// <summary>
        /// Index para visualizar el detalle de puntos de medicion del reporte
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexDetalle(int? id, int? esConsulta = 0)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            var idReporte = id.GetValueOrDefault(0);
            FormatoReporteModel model = new FormatoReporteModel();

            var reporte = this.servicio.GetByIdReporte(idReporte);
            model.Nombre = reporte != null ? reporte.Repordescrip : string.Empty;
            model.IdReporte = idReporte;//
            model.IdLectura = reporte != null ? reporte.Lectcodi : 0;//
            model.ListaOrigenLectura = this.servicio.ListMeOrigenlecturas().Where(x => x.Origlectcodi != 0 && x.Origlectcodi != -1).OrderBy(x => x.Origlectnombre).ToList();
            model.ListaMedidas = this.servicio.ListSiTipoinformacions();
            model.ListaResolucionPto = servicio.ListarResolucionPto();
            model.EsReporteEditable = esConsulta != 1;

            //para popup AgregarPto
            model.ListaTipoEmpresa = this.servicio.ListarTiposEmpresa().Where(t => t.Tipoemprcodi > 0).ToList();
            model.ListaEmpresa = this.servicio.ListarEmpresas();
            model.ListaMedidas = this.servicio.ListSiTipoinformacions();
            model.ListaFamilia = this.servicio.ListarFamilia().Where(x => x.Famcodi != 0 && x.Famcodi != -1).ToList();
            model.ListaOrigenLectura = this.servicio.ListMeOrigenlecturas().Where(x => x.Origlectcodi != 0 && x.Origlectcodi != -1).OrderBy(x => x.Origlectnombre).ToList();
            var listaLecturas = this.servicio.ListMeLecturas().Where(x => x.Lectcodi != 0 && x.Lectcodi != -1).ToList();
            int lectcodi = model.IdLectura.Value;
            model.Origlectcodi = 0;
            var Lectura = this.servicio.GetByIdMeLectura(lectcodi);
            if (Lectura != null)
            {
                var OrigLectura = this.servicio.GetByOrigenlectura(Lectura.Origlectcodi.GetValueOrDefault(0));
                model.Origlectcodi = OrigLectura != null ? OrigLectura.Origlectcodi : 0;
                model.ListaEmpresa = this.servicio.ObtenerListaEmpresaByOriglectcodi(model.Origlectcodi);
            }
            model.ListaLectura = listaLecturas.Where(x => model.Origlectcodi == x.Origlectcodi.GetValueOrDefault(0)).OrderBy(x => x.Lectnomb).ToList();
            model.ListaReportPtoCal = servicio.ListarPuntosCal().Where(x => x.Origlectcodi == model.Origlectcodi).OrderBy(y => y.Ptomedicodi).ToList();
            model.ListaResolucionPto = servicio.ListarResolucionPto();

            List<int> reportesHidrologia = ConstantesReportesMedicion.FormatosHidrologia.Split(',').Select(int.Parse).ToList(); 

            if (reportesHidrologia.Contains(idReporte))
                model.IndicadorCopiado = "S";

            return View(model);
        }

        /// <summary>
        /// Devuelve vista parcial del listado de ptos de medicion en el detalle de formato
        /// </summary>
        /// <param name="reporte"></param>
        /// <param name="esEditable"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaPtoMedicion(int reporte, int esEditable)
        {
            FormatoReporteModel model = new FormatoReporteModel();

            try
            {
                model.ListaReportPto = this.servicio.ListarTodoPtoReporte(reporte);
                model.IdReporte = reporte;
                model.EsReporteEditable = esEditable == 1;
                model.StrResultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.StrResultado = "-1";
                model.StrMensaje = ex.Message;
                model.StrDetalle = ex.StackTrace;
            }
            return Json(model);
        }

        /// <summary>
        /// AGREGA UN PTO DE MEDICION AL REPORTE
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="formato"></param>
        /// <param name="hoja"></param>
        /// <param name="punto"></param>
        /// <param name="medida"></param>
        /// <param name="limsup"></param>
        /// <param name="orden"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AgregarPto(int empresa, int reporte, int punto, int lectcodi, int medida, int tipoptomedicodi, string calculado, int frecuencia, string repptonomb, string color, int? codptoequiv, string indicadorcopiado)
        {
            FormatoReporteModel model = new FormatoReporteModel();

            try
            {
                MeReporptomedDTO entity = new MeReporptomedDTO();
                var reporte_ = this.servicio.GetByIdMePtomedicion(punto);

                //validar si punto pertenece a la empresa
                entity.Ptomedicodi = punto;
                entity.Reporcodi = reporte;
                entity.Repptoestado = 1;
                entity.Tipoinfocodi = (medida == 0 ? (int)reporte_.Tipoinfocodi : medida);
                entity.Emprcodi = empresa;
                entity.Lectcodi = lectcodi;
                entity.PtomediCalculado = calculado;
                entity.Repptoorden = 300;
                entity.Repptotabmed = frecuencia;
                entity.Repptonomb = repptonomb;
                entity.Tipoptomedicodi = tipoptomedicodi;
                entity.Repptocolorcelda = color;
                entity.Repptoindcopiado = indicadorcopiado;

                if (codptoequiv > 0)
                {
                    entity.Repptoequivpto = codptoequiv;
                }

                var objFind = this.servicio.GetByIdMeReporptomed3(reporte, entity.Ptomedicodi, entity.Lectcodi, entity.Tipoinfocodi, entity.Tipoptomedicodi);
                if (objFind == null)
                {
                    model.HojaPto = this.servicio.GrabarReporptomed(entity);
                    model.Resultado = 1;
                }
                else { model.Resultado = 0; }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.StrResultado = "-1";
                model.StrMensaje = ex.Message;
                model.StrDetalle = ex.StackTrace;
            }
            return Json(model);
        }

        /// <summary>
        /// Actualiza en BD punto editado y devuelve resultado al cliente
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="ptomedicodi"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditarPto(int repptocodi, int estado, int newLectcodi, int newTipoinfocodi, int newTipoptomedicodi, int newFrecuencia, string repptonomb, string color, int? ptomedequiv, string indcopiado)
        {
            FormatoReporteModel model = new FormatoReporteModel();

            try
            {
                MeReporptomedDTO entity = this.servicio.GetByIdMeReporptomed(repptocodi);
                if (entity != null)
                {
                    entity.Repptoestado = estado;
                    entity.Tipoinfocodi = newTipoinfocodi;
                    entity.Lectcodi = newLectcodi;
                    entity.Tipoptomedicodi = newTipoptomedicodi;
                    entity.Repptotabmed = newFrecuencia;
                    entity.Repptonomb = repptonomb;
                    entity.Repptocolorcelda = color;
                    entity.Repptoequivpto = ptomedequiv;
                    entity.Repptoindcopiado = indcopiado;

                    this.servicio.ActualizarReporptomed(entity);
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.StrResultado = "-1";
                model.StrMensaje = ex.Message;
                model.StrDetalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Actualiza en BD punto editado y devuelve resultado al cliente
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditarPtoCal(int ptomedicodi, int estado, string barranomb, string elenomb, string descripcion)
        {
            FormatoReporteModel model = new FormatoReporteModel();

            try
            {
                MePtomedicionDTO entity = this.servicio.GetByIdMePtomedicion(ptomedicodi);
                if (entity != null)
                {
                    entity.Ptomedibarranomb = barranomb;
                    entity.Ptomedielenomb = elenomb;
                    entity.Ptomedidesc = descripcion;
                    entity.Ptomediestado = (estado == 1 ? ConstantesAppServicio.Activo : ConstantesAppServicio.Baja);

                    this.servicio.UpdatePtoMedicion(entity);
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.StrResultado = "-1";
                model.StrMensaje = ex.Message;
                model.StrDetalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Elimina punto de medicion del formato y lo reordena 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="idHoja"></param>
        /// <param name="idOrden"></param>
        /// <param name="idPunto"></param>
        /// <returns></returns>
        public JsonResult EliminarPtoFromReporte(int repptocodi)
        {
            FormatoReporteModel model = new FormatoReporteModel();
            try
            {
                this.servicio.DeleteMeReporptomed(repptocodi);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.StrResultado = "-1";
                model.StrMensaje = ex.Message;
                model.StrDetalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Elimina punto Calculado de medicion del formato y lo reordena 
        /// </summary>
        /// <param name="idPunto"></param>
        /// <returns></returns>
        public JsonResult EliminarPtoCal(int ptomedicodi)
        {
            FormatoReporteModel model = new FormatoReporteModel();
            try
            {
                var dat = servicio.GetByIdMePtomedicion(ptomedicodi);
                dat.Ptomediestado = ConstantesAppServicio.Anulado;

                this.servicio.UpdatePtoMedicion(dat);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.StrResultado = "-1";
                model.StrMensaje = ex.Message;
                model.StrDetalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Abre el popup para ingresar el punto de medicion calculado
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult MostrarAgregarPtoCalculado(int reporte, int lectcodi)
        {
            FormatoReporteModel model = new FormatoReporteModel();

            model.ListaTipoEmpresa = this.servicio.ListarTiposEmpresa().Where(t => t.Tipoemprcodi > 0).ToList();
            model.ListaEmpresa = this.servicio.ListarEmpresas();
            model.ListaMedidas = this.servicio.ListSiTipoinformacions();
            model.ListaFamilia = this.servicio.ListarFamilia().Where(x => x.Famcodi != 0 && x.Famcodi != -1).ToList();
            model.IdReporte = reporte;
            model.ListaOrigenLectura = this.servicio.ListMeOrigenlecturas().Where(x => x.Origlectcodi != 0 && x.Origlectcodi != -1).OrderBy(x => x.Origlectnombre).ToList();
            var listaLecturas = this.servicio.ListMeLecturas().Where(x => x.Lectcodi != 0 && x.Lectcodi != -1).ToList();
            model.IdLectura = lectcodi;

            var Lectura = this.servicio.GetByIdMeLectura(lectcodi);
            if (Lectura != null)
            {
                var OrigLectura = this.servicio.GetByOrigenlectura(Lectura.Origlectcodi.GetValueOrDefault(0));
                model.Origlectcodi = OrigLectura != null ? OrigLectura.Origlectcodi : 0;
            }
            model.ListaLectura = listaLecturas.Where(x => model.Origlectcodi == x.Origlectcodi.GetValueOrDefault(0)).OrderBy(x => x.Lectnomb).ToList();

            return PartialView(model);
        }

        /// <summary>
        /// AGREGA UN PTO DE MEDICION calculado AL REPORTE
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="formato"></param>
        /// <param name="hoja"></param>
        /// <param name="punto"></param>
        /// <param name="medida"></param>
        /// <param name="limsup"></param>
        /// <param name="orden"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AgregarPtoCalculado(int empresa, int reporte, int equipocodi, int lectura, string barranomb, string elenomb, string descripcion)
        {
            FormatoReporteModel model = new FormatoReporteModel();

            try
            {
                //registrar Punto de medición
                MePtomedicionDTO punto = new MePtomedicionDTO();
                punto.Lastuser = User.Identity.Name;
                punto.Lastdate = DateTime.Now;
                punto.Emprcodi = empresa;
                punto.Equicodi = (equipocodi == 0 ? -1 : equipocodi);
                punto.Origlectcodi = lectura;
                punto.Orden = 1;
                punto.Ptomedibarranomb = barranomb;
                punto.Ptomedielenomb = elenomb;
                punto.Ptomedidesc = descripcion;
                punto.Osicodi = null;
                punto.Tipoptomedicodi = -1;
                punto.PtomediCalculado = ConstantesReportesMedicion.PtoCalculadoSiCodigo;
                punto.Ptomediestado = ConstantesAppServicio.Activo;
                punto.Tipoinfocodi = -1;
                punto.TipoSerie = null;

                //registrar pu
                //MeReporptomedDTO hojaptos = new MeReporptomedDTO();
                //hojaptos.Ptomedicodi = ptomedicodi;
                //hojaptos.Reporcodi = reporte;
                //hojaptos.Repptoestado = 1;
                //hojaptos.Tipoinfocodi = medida;
                //hojaptos.Emprcodi = empresa

                int ptomedicodi = this.servicio.SavePtoMedicion(punto);
                model.Resultado = (ptomedicodi > 0 ? 1 : 0);
                //    var entity = this.servicio.GetByIdMeReporptomed(reporte, medida, ptomedicodi);
                //    if (entity == null)
                //    {
                //        model.HojaPto = this.servicio.GrabarReporptomed(hojaptos);
                //        model.Resultado = 1;
                //    }
                //    else
                //        model.Resultado = 0;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.StrResultado = "-1";
                model.StrMensaje = ex.Message;
                model.StrDetalle = ex.StackTrace;
            }
            return Json(model);
        }

        /// <summary>
        /// Actualiza el orden de los ptos de medicion en el reporte 
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="hoja"></param>
        /// <param name="empresa"></param>
        /// <param name="id"></param>
        /// <param name="fromPosition"></param>
        /// <param name="toPosition"></param>
        /// <param name="direction"></param>
        public void UpdateOrderDetalleReporte(int reporcodi, int id, int fromPosition, int toPosition, string direction)
        {
            List<MeReporptomedDTO> listaReportPto = this.servicio.ListarTodoPtoReporte(reporcodi);

            if (direction == "back")
            {
                int orden = toPosition;
                List<MeReporptomedDTO> ltmp = new List<MeReporptomedDTO>();
                ltmp.Add(listaReportPto[fromPosition - 1]);
                ltmp.AddRange(listaReportPto.GetRange(toPosition - 1, fromPosition - toPosition));
                foreach (var reg in ltmp)
                {
                    reg.Repptoorden = orden;
                    this.servicio.ActualizarReporptomed(reg); //Actualizar el orden
                    orden++;
                }
            }
            else
            {
                int orden = fromPosition;
                List<MeReporptomedDTO> ltmp = new List<MeReporptomedDTO>();
                ltmp.AddRange(listaReportPto.GetRange(fromPosition, toPosition - fromPosition));
                ltmp.Add(listaReportPto[fromPosition - 1]);
                foreach (var reg in ltmp)
                {
                    reg.Repptoorden = orden;
                    this.servicio.ActualizarReporptomed(reg); //Actualizar el orden
                    orden++;
                }
            }
        }

        #endregion

        #region Detalle Punto Calculado
        /// <summary>
        /// Index para visualizar el detalle de puntos de medicion del reporte
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexDetalleCalculado(int? pto)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            var ptomedicodi = pto.GetValueOrDefault(0);
            FormatoReporteModel model = new FormatoReporteModel();

            var punto = this.servicio.GetByIdMePtomedicion(ptomedicodi);
            model.Nombre = punto != null ? punto.Ptomedidesc : string.Empty;
            model.Ptomedicodi = ptomedicodi;

            model.ListaOrigenLectura = this.servicio.ListMeOrigenlecturas().Where(x => x.Origlectcodi != 0 && x.Origlectcodi != -1).OrderBy(x => x.Origlectnombre).ToList();
            model.ListaMedidas = this.servicio.ListSiTipoinformacions();
            model.ListaResolucionPto = servicio.ListarResolucionPto();

            return View(model);
        }

        /// <summary>
        /// Devuelve vista parcial del listado de ptos de medicion en el detalle de formato
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="formato"></param>
        /// <returns></returns>
        public PartialViewResult ListaPtoCalculado(int ptomedicodi)
        {
            FormatoReporteModel model = new FormatoReporteModel();
            int reporte = -1;

            model.ListaPtos = this.servicio.ListarPtoMedicionFromCalculado(ptomedicodi);
            model.IdReporte = reporte;
            model.Ptomedicodi = ptomedicodi;

            return PartialView(model);
        }

        /// <summary>
        /// Elimina punto de pto Calculado 
        /// </summary>
        /// <param name="ptomedicodical"></param>
        /// <param name="ptomedicodi"></param>
        /// <returns></returns>
        public JsonResult DeletePto(int relptocodi)
        {
            FormatoReporteModel model = new FormatoReporteModel();
            try
            {
                this.servicio.DeleteMeRelacionpto(relptocodi);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.StrResultado = "-1";
                model.StrMensaje = ex.Message;
                model.StrDetalle = ex.StackTrace;
            }
            return Json(model);
        }

        /// <summary>
        /// Abre el popup para ingresar el punto de medicion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult MostrarAgregarPtoACalculado(int pto)
        {
            FormatoReporteModel model = new FormatoReporteModel();

            MePtomedicionDTO objPto = this.servicio.GetByIdMePtomedicion(pto);

            model.Origlectcodi = objPto.Origlectcodi.Value;

            model.ListaTipoEmpresa = this.servicio.ListarTiposEmpresa().Where(t => t.Tipoemprcodi > 0).ToList();
            model.ListaEmpresa = this.servicio.ObtenerListaEmpresaByOriglectcodi(model.Origlectcodi);
            model.ListaMedidas = this.servicio.ListSiTipoinformacions();
            model.ListaFamilia = this.servicio.ListarFamilia().Where(x => x.Famcodi != 0 && x.Famcodi != -1).ToList();
            model.Ptomedicodi = pto;
            model.ListaOrigenLectura = this.servicio.ListMeOrigenlecturas().Where(x => x.Origlectcodi != 0 && x.Origlectcodi != -1).OrderBy(x => x.Origlectnombre).ToList();
            model.IdLectura = 0;
            model.ListaLectura = this.servicio.ListMeLecturas().Where(x => x.Origlectcodi == objPto.Origlectcodi).OrderBy(x => x.Lectnomb).ToList();
            model.ListaReportPtoCal = servicio.ListarPuntosCal().OrderBy(y => y.Ptomedidesc).ToList();
            model.ListaResolucionPto = servicio.ListarResolucionPto();

            return PartialView(model);
        }

        /// <summary>
        /// //Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesReportesMedicion.ModuloManualUsuarioSGI;
            string nombreArchivo = ConstantesReportesMedicion.ArchivoManualUsuarioIntranetSGI;
            string pathDestino = modulo + ConstantesReportesMedicion.FolderRaizSGIModuloManual;
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

        /// <summary>
        /// Genera listado de puntos de medicion 
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public JsonResult ListarPtoCal(int emprcodi, int equicodi, int origlectcodi, string tpto)
        {
            var ListaPtosCal = servicio.ListarPuntosCal().Where(x => (x.Emprcodi == emprcodi || emprcodi == 0) && (x.Equicodi == equicodi || equicodi == 0)
                 && (x.Origlectcodi == origlectcodi || origlectcodi == 0) && x.PtomediCalculado == tpto).OrderBy(x => x.Ptomedicodi).ToList();

            return Json(ListaPtosCal);
        }

        /// <summary>
        /// AGREGA UN PTO a punto calculado
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="formato"></param>
        /// <param name="hoja"></param>
        /// <param name="punto"></param>
        /// <param name="medida"></param>
        /// <param name="limsup"></param>
        /// <param name="orden"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AgregarPtoAPtoCalculado(string cal, int empresa, int punto, int puntoCalculado, decimal factor, int lectcodi, int medida, int tipmedida,int frecuencia, decimal potencia)
        {
            FormatoReporteModel model = new FormatoReporteModel();
            try
            {
                MeRelacionptoDTO relPto = new MeRelacionptoDTO();

                if (cal == "S")
                {
                    var ptoCal = servicio.GetByIdMePtomedicion(punto);
                    if (ptoCal != null)
                    {
                        medida = (int)ptoCal.Tipoinfocodi;
                    }
                }

                relPto.Ptomedicodi1 = puntoCalculado;
                relPto.Ptomedicodi2 = punto;
                relPto.Trptocodi = 1;
                relPto.Relptofactor = factor;
                relPto.Lectcodi = lectcodi;
                relPto.Tipoinfocodi = medida;
                relPto.Tptomedicodi = tipmedida;
                relPto.Relptotabmed = frecuencia;
                relPto.Relptopotencia = potencia;

                this.servicio.SaveMeRelacionpto(relPto);
                model.Resultado = 1;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.StrResultado = "-1";
                model.StrMensaje = ex.Message;
                model.StrDetalle = ex.StackTrace;
            }
            return Json(model);
        }

        /// <summary>
        /// Editar punto perteneciente a un calculado
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="ptomedicodi"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditarPtoDeCalculado(int relptocodi, decimal factor, int newLectcodi, int newTipoinfocodi, int newtipmedida, int newresolucion, decimal potencia)
        {
            FormatoReporteModel model = new FormatoReporteModel();

            try
            {
                MeRelacionptoDTO entity = this.servicio.GetByIdMeRelacionpto(relptocodi);

                if (entity != null)
                {
                    entity.Relptofactor = factor;
                    entity.Tipoinfocodi = newTipoinfocodi;
                    entity.Tptomedicodi = newtipmedida;
                    entity.Lectcodi = newLectcodi;
                    entity.Relptotabmed = newresolucion;
                    entity.Funptocodi = null;
                    entity.Relptopotencia = potencia;
                }
                this.servicio.UpdateMeRelacionpto(entity);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.StrResultado = "-1";
                model.StrMensaje = ex.Message;
                model.StrDetalle = ex.StackTrace;
            }
            return Json(model);
        }
        #endregion

        #region Visualización Reporte
        /// <summary>
        /// Index para visualizar el reporte
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult IndexVisualizacion(int? id)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            int idreporte = id.GetValueOrDefault(0);

            FormatoReporteModel model = new FormatoReporteModel();
            var reporte = this.servicio.GetByIdReporte(idreporte);
            reporte = reporte != null ? reporte : new MeReporteDTO();

            model.Nombre = reporte.Repornombre;
            model.IdReporte = idreporte;
            model.CheckEmpresa = reporte.Reporcheckempresa;
            model.CheckEquipo = reporte.Reporcheckequipo;
            model.CheckTipoEquipo = reporte.Reporchecktipoequipo;
            model.CheckTipoMedida = reporte.Reporchecktipomedida;
            model.FechaInicio = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            return View(model);
        }


        /// <summary>
        /// Permite verificar si el reporcodi ingresado esta configurado para mostrar lista
        /// </summary>
        /// <param name="ireporcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidadReporcodi(int ireporcodi)
        {
            FormatoReporteModel model = new FormatoReporteModel();
            var reporte = this.servicio.GetByIdReporte(ireporcodi);
            int lecturaReporte = (int)reporte.Lectcodi;
            if (lecturaReporte == 0)
                model.ReporcodiValido = "0";
            else
            {
                if (lecturaReporte == -1)
                    model.ReporcodiValido = "-1";
                else
                    model.ReporcodiValido = "1";
            }

            return Json(model);
        }

        /// <summary>
        /// Listado reporte
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="empresas"></param>
        /// <param name="tipoEquipo"></param>
        /// <param name="equipo"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaVisualizacion(int reporcodi, string empresas, string tipoEquipo, string equipo, string tipoMedida, string fechaInicial, string fechaFinal, int nroPagina)
        {
            FormatoReporteModel model = new FormatoReporteModel();
            DateTime fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture); ;
            DateTime fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            try
            {
                string resultado = this.servicio.GenerarReporteHtml(reporcodi, fechaInicio, fechaFin, nroPagina, empresas, tipoEquipo, equipo, tipoMedida);
                model.StrResultado = resultado;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.StrResultado = "-1";
                model.StrError = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Para paginar Listado reporte
        /// </summary>
        /// <param name="ptomedicion"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult PaginadoVisualizacion(int reporcodi, string fechaInicial, string fechaFinal)
        {
            FormatoReporteModel model = new FormatoReporteModel();

            DateTime fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int nroPaginas = this.servicio.ObtenerTotalPaginacionReporte(reporcodi, fechaInicio, fechaFin);
            if (nroPaginas == -1) // Lectocodi de reporte = -1
            {
                model.MensajeErrorLectura = "ERRORDELECTURA";
            }
            else
            {
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
                model.MensajeErrorLectura = "";
            }

            return PartialView(model);
        }

        /// <summary>
        /// Listar las empresas del reporte
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <returns></returns>
        public JsonResult CargarListaEmpresa(int reporcodi)
        {
            FormatoReporteModel model = new FormatoReporteModel();

            var listaPto = this.servicio.ListarTodoPtoReporte(reporcodi);
            model.ListaEmpresa = listaPto.GroupBy(x => new { x.Emprcodi, x.Emprnomb })
                .Select(x => new SiEmpresaDTO() { Emprcodi = x.Key.Emprcodi, Emprnomb = x.Key.Emprnomb })
                .OrderBy(x => x.Emprnomb).ToList();

            return Json(model);
        }

        /// <summary>
        /// Listar los tipos de equipos del reporte
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <returns></returns>
        public JsonResult CargarListaTipoEquipo(int reporcodi, string emprcodi)
        {
            FormatoReporteModel model = new FormatoReporteModel();

            var listaPto = this.servicio.ListarTodoPtoReporte(reporcodi);
            string[] listaEmp = emprcodi.Split(',');
            model.ListaFamilia = listaPto.Where(x => listaEmp.Contains(x.Emprcodi.ToString()))
                .GroupBy(x => new { x.Famcodi, x.Famnomb })
                .Select(x => new EqFamiliaDTO() { Famcodi = x.Key.Famcodi, Famnomb = x.Key.Famnomb })
                .OrderBy(x => x.Famnomb).ToList();

            return Json(model);
        }

        /// <summary>
        /// Listar los equipos del reporte
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public JsonResult CargarListaEquipo(int reporcodi, string emprcodi, string famcodi)
        {
            FormatoReporteModel model = new FormatoReporteModel();

            var listaPto = this.servicio.ListarTodoPtoReporte(reporcodi);
            string[] listaEmp = emprcodi.Split(',');
            string[] listaFam = famcodi.Split(',');
            model.ListaEquipo = listaPto.Where(x => listaEmp.Contains(x.Emprcodi.ToString()) && listaFam.Contains(x.Famcodi.ToString()))
                .GroupBy(x => new { x.Equicodi, x.Equinomb })
                .Select(x => new EqEquipoDTO() { Equicodi = x.Key.Equicodi, Equinomb = x.Key.Equinomb })
                .OrderBy(x => x.Equinomb).ToList();

            return Json(model);
        }

        /// <summary>
        /// Listar los tipo de informacion del reporte
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="famcodi"></param>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        public JsonResult CargarListaTipoInformacion(int reporcodi, string emprcodi, string famcodi, string equicodi)
        {
            FormatoReporteModel model = new FormatoReporteModel();

            var listaPto = this.servicio.ListarTodoPtoReporte(reporcodi);
            string[] listaEmp = emprcodi.Split(',');
            string[] listaFam = famcodi.Split(',');
            string[] listaEq = equicodi.Split(',');
            model.ListaMedidas = listaPto.Where(x => listaEmp.Contains(x.Emprcodi.ToString()) && listaFam.Contains(x.Famcodi.ToString()) && listaEq.Contains(x.Equicodi.ToString()))
                .GroupBy(x => new { x.Tipoinfocodi, x.Tipoinfoabrev })
                .Select(x => new SiTipoinformacionDTO() { Tipoinfocodi = x.Key.Tipoinfocodi, Tipoinfoabrev = x.Key.Tipoinfoabrev })
                .OrderBy(x => x.Tipoinfoabrev).ToList();

            return Json(model);
        }
        #endregion

        #region Nuevo Interfaces Pto Calculado

        public ActionResult IndexPtoCal()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FormatoReporteModel model = new FormatoReporteModel();

            return View(model);
        }

        /// <summary>
        /// Devuelve vista parcial del listado de ptos de medicion calculados
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ListaPtoMedicionCal()
        {
            FormatoReporteModel model = new FormatoReporteModel();

            model.ListaReportPtoCal = this.servicio.ListarPuntosCal();

            return PartialView(model);
        }

        /// <summary>
        /// Exportar reporte de Puntos calculados
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarExcelReportePtoCalculado()
        {
            string[] datos = new string[2];
            try
            {
                List<MePtomedicionDTO> listaPtoCalculado = this.servicio.ListarPuntosCal();
                List<MePtomedicionDTO> listaPtoDetalle = this.servicio.ListarPtoMedicionFromCalculado(-1);
                string ruta = string.Empty;
                datos[0] = this.servicio.GenerarFileExcelReportePuntoCalculado(listaPtoCalculado, listaPtoDetalle);
                datos[1] = ConstantesIEOD.RptExcelPtoCalculado;

                var jsonResult = Json(datos);
                return jsonResult;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Error = -1, Descripcion = ex.Message });
            }
        }

        #endregion

        #region Util

        /// <summary>
        /// Permite descargar el archivo 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarExcelReporte()
        {
            string strArchivoTemporal = Request["archivo"];
            string strArchivoNombre = Request["nombre"];
            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);
                System.IO.File.Delete(strArchivoTemporal);
            }

            string strNombreArchivo = string.Format("{0}.xlsx", strArchivoNombre);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
        }

        /// <summary>
        /// Listar empresas
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEmpresas(int idTipoEmpresa, int origlectcodi)
        {
            FormatoReporteModel model = new FormatoReporteModel();

            if (origlectcodi == -1)
                model.ListaEmpresa = this.servicio.ListarEmpresasPorTipo(idTipoEmpresa);
            else
                model.ListaEmpresa = this.servicio.ObtenerListaEmpresaByOriglectcodi(origlectcodi).Where(x => x.Tipoemprcodi == idTipoEmpresa || idTipoEmpresa == -2).ToList();

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Genera Vista de listado de equipo
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="familia"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarFamilia(int empresa, int origlectcodi)
        {
            FormatoReporteModel model = new FormatoReporteModel();
            if (origlectcodi == -1)
                model.ListaFamilia = this.servicio.ListarFamiliaXEmp(empresa).Where(x => x.Famcodi > 0).ToList();
            else
                model.ListaFamilia = this.servicio.ObtenerFamiliaPorOrigenLecturaEquipo(origlectcodi, empresa).Where(x => x.Famcodi > 0).ToList();

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Genera Vista de listado de equipo
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="familia"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEquipo(int origlectcodi, int empresa, int familia)
        {
            FormatoReporteModel model = new FormatoReporteModel();
            if (origlectcodi == -1)
                model.ListaEquipo = this.servicio.ObtenerEquiposPorFamilia(empresa, familia).Where(x => x.Equicodi != 0 && x.Equicodi != -1).ToList();
            else
                model.ListaEquipo = this.servicio.ObtenerEquiposPorFamiliaOriglectcodi(empresa, familia, origlectcodi).Where(x => x.Equicodi > 0).ToList();

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Genera listado de puntos de medicion 
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public JsonResult ListarLecturas(int origlectcodi)
        {
            FormatoReporteModel model = new FormatoReporteModel();
            model.ListaLectura = this.servicio.ListMeLecturas().Where(x => x.Origlectcodi == origlectcodi).ToList();

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Genera listado de Tipo punto de medicion 
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public JsonResult ListarTptomedicion(int tipoinfocodi)
        {
            FormatoReporteModel model = new FormatoReporteModel();
            model.ListaTipoMedidas = this.servicio.ListMeTipopuntomedicionByTipoinfocodi(tipoinfocodi);

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Genera listado de puntos de medicion 
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarPto(int equipo, int origlectcodi, int lectcodi, string tpto)
        {
            FormatoReporteModel model = new FormatoReporteModel();
            model.ListaPtos = this.servicio.GetByIdEquipoMePtomedicion(equipo, origlectcodi, lectcodi).Where(x => x.PtomediCalculado == tpto)
                                    .OrderBy(x => x.Ptomedielenomb).ThenBy(x => x.Tipoptomedinomb).ThenBy(x => x.Tipoinfoabrev).ToList();

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Genera listado de Tipo punto de medicion 
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public JsonResult ListarDataFiltroXPtomedicodi(string tpto, int origlectcodi, int ptomedicodi)
        {
            FormatoReporteModel model = new FormatoReporteModel();
            model.IdTipoempresa = -2;
            model.IdLectura = 0;

            try
            {
                MePtomedicionDTO objPto = this.servicio.GetByIdMePtomedicion(ptomedicodi);
                origlectcodi = objPto != null ? objPto.Origlectcodi ?? -1 : origlectcodi;

                //verificar que el punto de medición está registro en el modulo de "Creacion de formatos"
                List<MePtomedicionDTO> listaPto = this.servicio.GetByIdEquipoMePtomedicion(-1, origlectcodi, -1).Where(x => x.PtomediCalculado == tpto)
                                                        .OrderBy(x => x.Ptomedielenomb).ThenBy(x => x.Tipoptomedinomb).ThenBy(x => x.Tipoinfoabrev).ToList();

                List<MePtomedicionDTO> listaByPtomedicodi = listaPto.Where(x => x.Ptomedicodi == ptomedicodi).ToList();
                MePtomedicionDTO regPto = listaByPtomedicodi.FirstOrDefault() ?? objPto;
                
                if (regPto != null)
                {
                    model.ListaOrigenLectura = this.servicio.ListMeOrigenlecturas().Where(x => x.Origlectcodi != 0 && x.Origlectcodi != -1).OrderBy(x => x.Origlectnombre).ToList();
                    model.Origlectcodi = regPto.Origlectcodi ?? -1;

                    model.ListaTipoEmpresa = this.servicio.ListarTiposEmpresa().Where(t => t.Tipoemprcodi > 0).ToList();
                    model.IdTipoempresa = regPto.Tipoemprcodi;

                    model.ListaEmpresa = this.servicio.ListarEmpresasPorTipo(regPto.Tipoemprcodi);
                    model.IdEmpresa = regPto.Emprcodi ?? 0;

                    model.ListaFamilia = this.servicio.ObtenerFamiliaPorOrigenLecturaEquipo(origlectcodi, model.IdEmpresa).Where(x => x.Famcodi > 0).ToList();
                    model.IdFamilia = regPto.Famcodi;

                    model.ListaEquipo = this.servicio.ObtenerEquiposPorFamilia(regPto.Emprcodi ?? 0, regPto.Famcodi).Where(x => x.Equicodi != 0 && x.Equicodi != -1).ToList();
                    model.IdEquipo = regPto.Equicodi ?? 0;

                    model.ListaLectura = this.servicio.ListMeLecturas().Where(x => x.Origlectcodi == origlectcodi).ToList();
                    //if (listaLectcodi.Any()) model.ListaLectura = model.ListaLectura.Where(x => listaLectcodi.Contains(x.Lectcodi)).ToList();
                    model.IdLectura = 0; //listaLectcodi.Count > 1 ? 0 : regPto.Lectcodi;

                    model.ListaPtos = listaPto.Where(x => x.Equicodi == regPto.Equicodi && x.Lectcodi == regPto.Lectcodi).ToList();
                    if (!model.ListaPtos.Any())
                    {
                        model.ListaPtos.Add(regPto);
                    }
                    model.Ptomedicodi = regPto.Ptomedicodi;
                }
                else
                {
                    model.IdTipoempresa = -2;
                    model.IdLectura = 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.StrResultado = ex.Message;
                model.StrDetalle = ex.Message + ConstantesAppServicio.CaracterEnter + ex.StackTrace;
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

    }
}