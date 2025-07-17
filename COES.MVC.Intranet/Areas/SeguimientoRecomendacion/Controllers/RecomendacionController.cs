using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Globalization;
using COES.Servicios.Aplicacion.Recomendacion;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.SeguimientoRecomendacion.Models;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.Auditoria;
using COES.MVC.Intranet.Areas.SeguimientoRecomendacion.Helper;
using COES.Servicios.Aplicacion.Evento;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System.IO;
using COES.Framework.Base.Tools;
using log4net;
using COES.Servicios.Aplicacion.Eventos;

namespace COES.MVC.Intranet.Areas.SeguimientoRecomendacion.Controllers
{
    public class RecomendacionController : BaseController
    {

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(RecomendacionController));
        public RecomendacionController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("RecomendacionController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("RecomendacionController", ex);
                throw;
            }
        }


        SeguimientoRecomendacionAppServicio servSegRecomendacion = new SeguimientoRecomendacionAppServicio();
        AuditoriaAppServicio servAuditoria = new AuditoriaAppServicio();
        EventosAppServicio servEvento = new EventosAppServicio();
        EquipamientoAppServicio ServEquipo = new EquipamientoAppServicio();
        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        


        /// <summary>
        /// Maneja el código de recomendación
        /// </summary>
        public string SesionIdRec
        {
            get
            {
                return (Session[ConstanteSeguimientoRecomendacion.SesionIdRecom] != null) ?
                   Session[ConstanteSeguimientoRecomendacion.SesionIdRecom].ToString() : "";
            }
            set { Session[ConstanteSeguimientoRecomendacion.SesionIdRecom] = value; }
        }


        /// <summary>
        /// Muestra la pantalla inicial del módulo
        /// </summary>
        /// <param name="id">id de evento</param>
        /// <returns></returns>
        public ActionResult Index(int id)
        {
            EveEventoDTO evento = null;
            List<EveEventoDTO> lstEventosAsociados;
            List<EventoDTO> lstEvento = new List<EventoDTO>();
            AnalisisFallasAppServicio appAnalisisFallas = new AnalisisFallasAppServicio();
            BusquedaSrmRecomendacionModel model = new BusquedaSrmRecomendacionModel();
            
            if (id != 0)
            {
                evento = servEvento.GetByIdEveEvento((int)id);
                if(evento.Evenrcmctaf == "S")
                {
                    model.Tipo = "EVENTO CTAF";

                    lstEventosAsociados = appAnalisisFallas.ListadoEventosAsoCtaf(id).DistinctBy(y => y.Evencodi).ToList();
                    if(lstEventosAsociados.Count > 0)
                    {
                        foreach (var j in lstEventosAsociados)
                        {
                            EventoDTO Asociado = appAnalisisFallas.EventoDTOAsoCtaf(j.Evencodi);
                            if (Asociado.CODIGO != null)
                                lstEvento.Add(Asociado);
                        }
                    }
                    else
                    {
                        EventoDTO Asociado = appAnalisisFallas.EventoDTOAsoCtaf(evento.Evencodi);
                        lstEvento.Add(Asociado);
                    }
                    

                    EventoDTO primer_evento = (lstEvento.OrderBy(c => DateTime.ParseExact(c.FECHA_EVENTO, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture)).FirstOrDefault());

                    evento = servEvento.GetByIdEveEvento((int)primer_evento.EVENCODI);
                }
                else if (evento.Evenpreliminar == "A")
                {
                    model.Tipo = "EVENTO";
                }
                else
                {
                    if (evento.Evenasegoperacion == "S")
                        model.Tipo = "EVENTO AO";
                    else
                        model.Tipo = "";
                }

                model.FechaIni = ((DateTime)(evento.Evenini)).ToString(Constantes.FormatoFechaHora);
                model.FechaFin = evento.Evenfin == null ? "" : ((DateTime)(evento.Evenfin)).ToString(Constantes.FormatoFecha);
                model.Evenasunto = evento.Evenasunto;

                int equicodi=(int)evento.Equicodi;
                int emprcodi = (int)evento.Emprcodi;
                model.Equiabrev = ServEquipo.GetByIdEqEquipo(equicodi).Equiabrev;
                model.Emprnomb = servFormato.GetByIdSiEmpresa(emprcodi).Emprnomb;
                model.Evencodi = id;

                


            }

            
            
            model.ListaEveEvento = new List<EveEventoDTO>();
            model.ListaEqEquipo = new List<EqEquipoDTO>();
            model.ListaSrmCriticidad = servSegRecomendacion.ListSrmCriticidads();
            model.ListaSrmEstado = servSegRecomendacion.ListSrmEstados();
            model.ListaFwUser = servAuditoria.ListUserRol(ConstanteSeguimientoRecomendacion.RolAseguramiento);

            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            

            return View(model);
        }


        /// <summary>
        /// Permite editar el registro
        /// </summary>
        /// <param name="id">id de recomendación</param>
        /// <param name="accion">acción</param>
        /// <param name="evencodi">código de evento</param>
        /// <returns></returns>
        public ActionResult Editar(int id, int accion, int evencodi)
        {

            SrmRecomendacionModel model = new SrmRecomendacionModel();
            model.ListaEveEvento = new List<EveEventoDTO>();
            model.ListaEqEquipo = new List<EqEquipoDTO>();
            model.ListaSrmCriticidad = servSegRecomendacion.ListSrmCriticidads();
            model.ListaSrmEstado = servSegRecomendacion.ListSrmEstados().Where(x=> x.Srmstdcodi>0).ToList();
            model.ListaFwUser = servAuditoria.ListUserRol(ConstanteSeguimientoRecomendacion.RolAseguramiento);
            SrmRecomendacionDTO srmRecomendacion =null;

            if (id != 0)
            {
                srmRecomendacion = servSegRecomendacion.GetByIdSrmRecomendacion(id);
                if (srmRecomendacion.Equicodi!=null && srmRecomendacion.Equicodi!=-1)
                {
                    EqEquipoDTO equipo = ServEquipo.GetByIdEqEquipo((int)srmRecomendacion.Equicodi);
                    //obtener empresa
                    model.EquiAbrev = equipo.Equiabrev;
                    //obtener abrev. equipo
                    model.Emprnomb = servFormato.GetByIdSiEmpresa((int)equipo.Emprcodi).Emprnomb;

                    //obtener subestacion                    
                    EqAreaDTO area = ServEquipo.GetByIdEqArea((int)equipo.Areacodi);
                    model.Areanomb = area.Areanomb;

                }
            }

            if (srmRecomendacion != null)
            {
                srmRecomendacion.Fecharecomendacion = (srmRecomendacion.Srmrecfecharecomend != null) ? ((DateTime)srmRecomendacion.Srmrecfecharecomend).ToString(Constantes.FormatoFecha) : "";
                srmRecomendacion.Fechavencimiento = (srmRecomendacion.Srmrecfechavencim != null) ? ((DateTime)srmRecomendacion.Srmrecfechavencim).ToString(Constantes.FormatoFecha) : "";

                model.SrmRecomendacion = srmRecomendacion;
            }
            else
            {
                srmRecomendacion = new SrmRecomendacionDTO();

                srmRecomendacion.Evencodi = evencodi;
                srmRecomendacion.Equicodi = Convert.ToInt32(Constantes.ParametroDefecto);
                srmRecomendacion.Srmcrtcodi = Convert.ToInt32(Constantes.ParametroDefecto);
                srmRecomendacion.Srmstdcodi = Convert.ToInt32(Constantes.ParametroDefecto);
                srmRecomendacion.Usercode = Convert.ToInt32(Constantes.ParametroDefecto);

                //srmRecomendacion.Srmrecfecharecomend = Convert.ToDateTime(DateTime.Now.ToString(Constantes.FormatoFecha));
                //srmRecomendacion.Srmrecfechavencim = Convert.ToDateTime(DateTime.Now.ToString(Constantes.FormatoFecha));

                srmRecomendacion.Fecharecomendacion = DateTime.Now.ToString(Constantes.FormatoFecha);
                srmRecomendacion.Fechavencimiento = DateTime.Now.ToString(Constantes.FormatoFecha);


                srmRecomendacion.Srmrecdianotifplazo = ConstanteSeguimientoRecomendacion.DiaNotificacion;

                model.SrmRecomendacion = srmRecomendacion;

            }

            model.Accion = accion;
            model.IndicadorGrabar = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);

            return View(model);
            
        }


        /// <summary>
        /// Permite eliminar el registro de recomendación
        /// </summary>
        /// <param name="id">id de recomendación</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            try
            {
                servSegRecomendacion.DeleteSrmRecomendacion(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite activar el registro
        /// </summary>
        /// <param name="id">id de recomendación</param>
        /// <param name="activo">activo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Activar(int id,string activo)
        {
            try
            {
                SrmRecomendacionDTO recom = servSegRecomendacion.GetByIdSrmRecomendacion(id);
                recom.Srmrecactivo = activo;
                recom.Srmrecfecmodificacion = DateTime.Now;
                recom.Srmrecusumodificacion = User.Identity.Name;
                servSegRecomendacion.SaveSrmRecomendacionId(recom);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite grabar el registro
        /// </summary>
        /// <param name="model">modelo tipo Recomendación</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(SrmRecomendacionModel model)
        {
            try
            {

                SrmRecomendacionDTO entity = new SrmRecomendacionDTO();

                entity.Srmreccodi = model.SrmrecCodi;
                entity.Evencodi = model.EvenCodi;
                entity.Equicodi = model.EquiCodi;
                entity.Srmcrtcodi = model.Srmcrtcodi;
                entity.Srmstdcodi = model.Srmstdcodi;
                entity.Usercode = model.Usercode;
                entity.Srmrecfecharecomend = DateTime.ParseExact(model.SrmrecFecharecomend, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                entity.Srmrecfechavencim = DateTime.ParseExact(model.SrmrecFechavencim, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                entity.Srmrecdianotifplazo = model.SrmrecDianotifplazo;
                entity.Srmrectitulo = model.SrmrecTitulo;
                entity.Srmrecrecomendacion = model.SrmrecRecomendacion;

                if (model.SrmrecActivo == null)
                {
                    entity.Srmrecactivo = "S";
                }
                else
                {
                    entity.Srmrecactivo = model.SrmrecActivo;
                }

                
                //---
                if (entity.Srmreccodi == 0)
                {
                    entity.Srmrecusucreacion = User.Identity.Name;
                    entity.Srmrecfeccreacion = DateTime.Now;
                }
                else
                {

                    if (model.SrmrecUsucreacion != null)
                    {
                        entity.Srmrecusucreacion = model.SrmrecUsucreacion;
                    }

                    if (model.SrmrecFeccreacion != null)
                    {
                        entity.Srmrecfeccreacion = DateTime.ParseExact(model.SrmrecFeccreacion, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }

                    entity.Srmrecusumodificacion = User.Identity.Name;
                    entity.Srmrecfecmodificacion = DateTime.Now; 
                               
                }
                //--

                EqEquipoDTO equipo = ServEquipo.GetByIdEqEquipo((int)entity.Equicodi);
                entity.Equiabrev = equipo.Equiabrev;
                //obtener abrev. equipo
                entity.Emprnomb = servFormato.GetByIdSiEmpresa((int)equipo.Emprcodi).Emprnomb;

                //obtener subestacion                    
                EqAreaDTO area = ServEquipo.GetByIdEqArea((int)equipo.Areacodi);
                entity.Areanomb = area.Areanomb;

                model.ListaSrmCriticidad = servSegRecomendacion.ListSrmCriticidads();
                SrmCriticidadDTO Criticidad = model.ListaSrmCriticidad.Find(x => x.Srmcrtcodi == entity.Srmcrtcodi);
                entity.Srmcrtdescrip = Criticidad.Srmcrtdescrip;
                int id = this.servSegRecomendacion.SaveSrmRecomendacionId(entity);

                SesionIdRec = id.ToString();

                if(entity.Srmstdcodi == 3)
                    servSegRecomendacion.EnvioCorreoRecomendacionCTAF(entity, Constantes.IdPlantilla, "", "");
                   

                return Json(id);

            }
            catch
            {
                SesionIdRec = "-1";
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite listar recomendaciones
        /// </summary>
        /// <param name="evenCodi">código de evento</param>
        /// <param name="activo">activo</param>
        /// <param name="nroPage">nro. de página</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(int evenCodi,string activo, int nroPage)
        {
            BusquedaSrmRecomendacionModel model = new BusquedaSrmRecomendacionModel();

            model.ListaSrmRecomendacion = servSegRecomendacion.BuscarOperacionesRec(evenCodi, activo, nroPage, ConstanteSeguimientoRecomendacion.PageSizeRecomendacion).ToList();
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            
            return PartialView(model);
        }


        /// <summary>
        /// PErmite realizar el paginado
        /// </summary>
        /// <param name="evenCodi">código de evento</param>
        /// <param name="equiCodi">código de equipo</param>
        /// <param name="srmcrtcodi">código de criticidad</param>
        /// <param name="srmstdcodi">código de estado</param>
        /// <param name="usercode">código de usuario</param>
        /// <param name="srmrecFecharecomend">fecha de recomendación</param>
        /// <param name="srmrecFechavencim">fecha de venciimiento</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(int evenCodi,string activo)
        {
            Paginacion model = new Paginacion();

            int nroRegistros = servSegRecomendacion.ObtenerNroFilasRec(evenCodi, activo);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = ConstanteSeguimientoRecomendacion.PageSizeRecomendacion;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return base.Paginado(model);
        }

        //manejo de archivos
        /// <summary>
        /// Permite cargar los documentos
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload(int chunks, int chunk, string name)
        {
            try
            {
                //int idcom = SesionIdRecCom;
                string srmreccodi = SesionIdRec;

                string pathBaseSeguimiento = RutaBaseArchivos();
                
                FileServer.CreateFolder(pathBaseSeguimiento, srmreccodi.ToString(), null);

                pathBaseSeguimiento += srmreccodi;

                if (Request.Files.Count > 0)
                {
                    if (chunks > 1)
                    {
                        var file = Request.Files[0];
                        string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;
                        using (var fs = new FileStream(Path.Combine(path, name), chunk == 0 ? FileMode.Create : FileMode.Append))
                        {
                            var buffer = new byte[file.InputStream.Length];
                            file.InputStream.Read(buffer, 0, buffer.Length);
                            fs.Write(buffer, 0, buffer.Length);
                        }

                        if (chunk == chunks - 1)
                        {
                            System.IO.FileInfo info = new FileInfo(path + name);
                            FileServer.UploadFromFileDirectory(path + name, base.PathFiles, name, string.Empty);
                        }
                    }
                    else
                    {
                        var file = Request.Files[0];
                        FileServer.UploadFromStream(file.InputStream, pathBaseSeguimiento + "\\", file.FileName, string.Empty);
                    }
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// Permite obtener la ruta base de los archivos
        /// </summary>
        /// <returns></returns>
        public string RutaBaseArchivos()
        {

            string LocalDirectory = base.PathFiles; //ConstanteSeguimientoRecomendacion.PathSeguimientoRecomendaciones;
            //string LocalDirectory = ConstanteSeguimientoRecomendacion.PathSeguimientoRecomendaciones;

            return LocalDirectory;
        }


        /// <summary>
        /// Permite mostrar los archivos de un directorio
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Archivos(string tipo,string srmreccodi)
        {
            FileModel model = new FileModel();

            //retorno en caso de nuevo
            if (srmreccodi == "0" || srmreccodi.IndexOf("\\0")>=0)
            {
                model.ListaDocumentos = new List<FileData>();
            }
            else
            {
                
                if (srmreccodi != "" && srmreccodi != "\\")
                {
                    
                    string pathBaseSeguimiento = RutaBaseArchivos() + srmreccodi + "\\";


                    //if (FileServer.VerificarExistenciaDirectorio(pathBaseSeguimiento, string.Empty))
                    //{
                        model.ListaDocumentos = FileServer.ListarArhivos(pathBaseSeguimiento, string.Empty).Where(x => x.Extension != null).ToList();
                        model.Famabrev = "'"+tipo + "'"; //"'R'";
                    //}
                    //else
                    //{
                    //    model.ListaDocumentos = new List<FileData>();                        
                    //}
                }
                else
                {
                    model.ListaDocumentos = new List<FileData>();
                }
            }

            return PartialView(model);
        }


        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Download(string url)
        {
            Stream stream = FileServer.DownloadToStream(url, string.Empty);

            int indexOf = url.LastIndexOf('/');
            string fileName = url;
            if (indexOf >= 0)
            {
                fileName = url.Substring(indexOf + 1, url.Length - indexOf - 1);
            }
            indexOf = fileName.LastIndexOf('.');
            string extension = fileName.Substring(indexOf + 1, fileName.Length - indexOf - 1);

            return File(stream, extension, fileName);
        }


        /// <summary>
        /// Permite eliminar el archivo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(string url)
        {
            try
            {
                FileServer.DeleteBlob(url, string.Empty);
                return Json(1);
            }
            catch
            {
                
            }

            return Json(-1);
            
        }
        


    }
}
