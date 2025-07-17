using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Web.Helper;
using COES.MVC.Intranet.Areas.Web.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Web.Controllers
{
    public class CalendarioController : BaseController
    {
        /// <summary>
        /// Extension del archivo subido
        /// </summary>
        public string FileName
        {
            get
            {
                return (Session[DatosSesion.FileInfografia] != null) ?
                    Session[DatosSesion.FileInfografia].ToString() : string.Empty;
            }
            set
            {
                Session[DatosSesion.FileInfografia] = value;
            }
        }

        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        PortalAppServicio servicio = new PortalAppServicio();

        /// <summary>
        /// Pantalla Inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            CalendarioModel model = new CalendarioModel();
            model.FechaInicio = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).ToString(Constantes.FormatoFecha);
            model.FechaFin = (new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1)).ToString(Constantes.FormatoFecha);       

            return View(model);
        }

        /// <summary>
        /// Permite listar los registros de la consulta
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado(string nombre, string fechaInicio, string fechaFin)
        {
            CalendarioModel model = new CalendarioModel();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            model.ListaCalendario = this.servicio.GetByCriteriaWbCalendarios(nombre, fechaInicial, fechaFinal);

            return PartialView(model);
        }

        /// <summary>
        /// Permite editar los datos de la subscripcion
        /// </summary>
        /// <param name="idRelacion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Editar(int idEvento)
        {
            CalendarioModel model = new CalendarioModel();
            model.ListaColores = WebHelper.ObtenerColores();
            model.ListaIconos = WebHelper.ObtenerIconos();
            model.ListaTipo = this.servicio.ListWbCaltipoventos();

            if (idEvento == 0)
            {
                model.Entidad = new WbCalendarioDTO();
                model.FechaInicio = DateTime.Now.ToString(Constantes.FormatoFecha);
                model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
                model.Entidad.Calendestado = Constantes.EstadoActivo;
            }
            else
            {
                model.Entidad = this.servicio.GetByIdWbCalendario(idEvento);
                model.FechaInicio = (model.Entidad.Calendfecinicio != null) ? ((DateTime)model.Entidad.Calendfecinicio).ToString(
                    Constantes.FormatoFecha) : string.Empty;
                //model.FechaFin = (model.Entidad.Calendfecfin != null) ? ((DateTime)model.Entidad.Calendfecfin).ToString(
                //    Constantes.FormatoFecha) : string.Empty;

                //if (model.Entidad.Calendfecinicio != null) 
                //{
                //    DateTime fecha = (DateTime)model.Entidad.Calendfecinicio;

                //    if (fecha.Minute != 0 || fecha.Hour != 0) 
                //    {
                //        model.HoraInicio = fecha.ToString(Constantes.FormatoHoraMinuto);
                //    }
                //}

                //if (model.Entidad.Calendfecfin != null) 
                //{
                //    DateTime fecha = (DateTime)model.Entidad.Calendfecfin;

                //    if (fecha.Minute != 0 || fecha.Hour != 0)
                //    {
                //        model.HoraFin = fecha.ToString(Constantes.FormatoHoraMinuto);
                //    }
                //}

            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar la relación de equivalencia
        /// </summary>
        /// <param name="idRelacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int idEvento)
        {
            try
            {
                this.servicio.DeleteWbCalendario(idEvento);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite grabar la relación de equivalencia
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(CalendarioModel model)
        {
            try
            {
                DateTime fechaInicio = DateTime.ParseExact(model.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                //DateTime fechaFin = DateTime.ParseExact(model.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fecInicio = fechaInicio.AddHours(12);
                DateTime fecFin = fechaInicio.AddHours(12);
                     

                //if (!string.IsNullOrEmpty(model.HoraInicio)) {
                //    string[] hora = model.HoraInicio.Split(Constantes.CaracterDosPuntos);
                //    fecInicio = fechaInicio.AddHours(Convert.ToInt32(hora[0])).AddMinutes(Convert.ToInt32(hora[1]));                    
                //}

                //if (!string.IsNullOrEmpty(model.HoraFin)) {
                //    string[] hora = model.HoraFin.Split(Constantes.CaracterDosPuntos);
                //    fecFin = fechaFin.AddHours(Convert.ToInt32(hora[0])).AddMinutes(Convert.ToInt32(hora[1]));                    
                //}

                //TimeSpan diferencia = fechaFin.Subtract(fechaInicio);

                //if (diferencia.TotalSeconds >= 0)
                //{

                    WbCalendarioDTO entity = new WbCalendarioDTO
                    {
                        Calendcodi = model.Codigo,
                        Calendtitulo = model.Titulo,
                        Calenddescripcion = model.Descripcion,
                        Calendfecinicio = fecInicio,
                        Calendfecfin = fecFin,
                        Calendicon = model.Icono,
                        Calendestado = model.Estado,
                        Calendcolor = model.Color,
                        Tipcalcodi = model.TipoEvento,
                        Calendfecmodificacion = DateTime.Now,
                        Calendusumodificacion = base.UserName
                    };

                    int result = this.servicio.SaveWbCalendario(entity);
                    return Json(result);
                //}
                //else 
                //{
                //    return Json(-2);
                //}                
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite generar el formato horizontal
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(string fechaInicio, string fechaFin, int? idPublicacion)
        {
            try
            {
                //string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionWeb;
                //string file = this.FileExportacion;

                //DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                //DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                //List<WbSubscripcionDTO> list = this.servicio.ListarSubscripcionesExportar(fechaInicial, fechaFinal, idPublicacion);
                //this.servicio.GenerarReporteExcel(list, path, file);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            //string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionWeb + this.FileExportacion;
            //return File(fullPath, Constantes.AppExcel, this.FileExportacion);
            return View();
        }

        /// <summary>
        /// Muestra el calendario de eventos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual PartialViewResult Calendario()
        {
            CalendarioModel model = new CalendarioModel();
            model.ListaLeyenda = this.servicio.ListWbCaltipoventos();

            #region Eventos 

            List<WbCalendarioDTO> list = this.servicio.ListWbCalendarios();
            List<DocDiaEspDTO> feriados = this.servicio.ObtenerDiasFeriados();

            int index = 0;
            StringBuilder strFeriado = new StringBuilder();
            strFeriado.Append("[");

            foreach (DocDiaEspDTO item in feriados)
            {
                string final = @",";
                if (index == feriados.Count - 1) final = @"";

                strFeriado.Append("'" + ((DateTime)item.Diafecha).ToString("yyyy-MM-dd") + "'" + final);
                index++;
            }

            strFeriado.Append("]");
            model.Feriados = strFeriado.ToString();

            StringBuilder str = new StringBuilder();
            str.Append("[");

            index = 0;
            foreach (WbCalendarioDTO item in list)
            {
                string final = @",";
                if (index == list.Count - 1) final = @"";

                string contenido = string.Format(@"               
                [
                    id : {0},
                    title : '{1}',
                    start : '{2}',
                    end : '{3}',
                    color : '{4}',
                    imageurl: '{6}',
                    description : '{7}'
                ]{5}", item.Calendcodi, item.Calendtitulo, ((DateTime)item.Calendfecinicio).ToString("yyyy-MM-ddTHH:mm:00"),
                  ((DateTime)item.Calendfecinicio).ToString("yyyy-MM-ddTHH:mm:00"), item.Calendcolor, final,
                  Url.Content("~/") + "content/images/" + item.Calendicon, item.Calenddescripcion);
                index++;

                contenido = contenido.Replace("[", "{");
                contenido = contenido.Replace("]", "}");
                str.Append(contenido);                    
            }

            str.Append("]");
            model.Data = str.ToString();

            #endregion

            #region Meses

            List<WbMescalendarioDTO> listMeses = this.servicio.ListWbMescalendarios();
            StringBuilder strMeses = new StringBuilder();
            strMeses.Append("[");

            index = 0;
            foreach (WbMescalendarioDTO item in listMeses)
            {
                string image = (!string.IsNullOrEmpty(item.Mescalinfo)) ?
                    string.Format(Constantes.InfografiaPortal, item.Mescalcodi) + item.Mescalinfo : string.Empty;

                string final = @",";
                if (index == listMeses.Count - 1) final = @"";

                string contenido = string.Format(@"               
                [
                    anio : {0},
                    mes : '{1}',
                    imagen : '{2}',
                    color : '{3}',
                    colorsat : '{4}',
                    colorsun: '{5}',
                    titulo: '{6}',
                    subtitulo: '{7}',
                    colortitulo: '{8}',
                    colorsubtitulo: '{9}',
                    colordia: '{10}',
                    aniomes: '{11}'

                ]{12}",
                     item.Mescalanio, COES.Base.Tools.Util.ObtenerNombreMesAbrev((int)item.Mescalmes), 
                     image, item.Mescalcolor, item.Mescalcolorsat, item.Mescalcolorsun, item.Mescaltitulo, item.Mescaldescripcion,
                     item.Mescalcolortit, item.Mescalcolorsubtit,item.Mesdiacolor, item.Mescalanio + "-" + item.Mescalmes.ToString().PadLeft(2,'0'),  final);
                index++;

                contenido = contenido.Replace("[", "{");
                contenido = contenido.Replace("]", "}");
                strMeses.Append(contenido);
            }

            strMeses.Append("]");
            model.Meses = strMeses.ToString();
            model.FechaActual = "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'";

            #endregion


            return PartialView(model);
        }
        
        /// <summary>
        /// Pantalla Inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult InfoIndex()
        {
            MesCalendarioModel model = new MesCalendarioModel();
            model.ListaAnios = WebHelper.ListarAnios();
            model.ListMeses = WebHelper.ListarMeses();
            model.AnioActual = DateTime.Now.Year;
            return View(model);
        }

        /// <summary>
        /// Permite listar los registros de la consulta
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult InfoListado(int? anio, int? mes)
        {
            if (anio == null) anio = -1;
            if (mes == null) mes = -1;
            MesCalendarioModel model = new MesCalendarioModel();
            model.Listado = this.servicio.GetByCriteriaWbMescalendarios(anio, mes);

            return PartialView(model);
        }

        /// <summary>
        /// Permite editar los datos de la subscripcion
        /// </summary>
        /// <param name="idRelacion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult InfoEditar(int idRegistro)
        {
            MesCalendarioModel model = new MesCalendarioModel();
            model.ListaAnios = WebHelper.ListarAnios();
            model.ListMeses = WebHelper.ListarMeses();
            model.ListaColores = WebHelper.ObtenerColores();           

            model.UrlPortal = ConfigurationManager.AppSettings["UrlIntranet"].ToString();
            this.FileName = string.Empty;

            if (idRegistro == 0)
            {
                model.Entidad = new WbMescalendarioDTO();
                model.Entidad.Mescalestado = Constantes.EstadoActivo;
                model.Entidad.Mescalmes = DateTime.Now.Month;
                model.Entidad.Mescalanio = DateTime.Now.Year;
            }
            else
            {
                model.Entidad = this.servicio.GetByIdWbMescalendario(idRegistro);

                if (!string.IsNullOrEmpty(model.Entidad.Mescalinfo))
                {
                    string archivo = archivo = string.Format(Constantes.InfografiaPortal, idRegistro) + model.Entidad.Mescalinfo;
                    model.Entidad.Mescalinfo = archivo;
                }
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar la relación de equivalencia
        /// </summary>
        /// <param name="idRelacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult InfoEliminar(int idRegistro)
        {
            try
            {
                this.servicio.DeleteWbMescalendario(idRegistro);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite cargar el archivo de potencia
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = path + file.FileName;
                    this.FileName = file.FileName;

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Permite grabar la relación de equivalencia
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult InfoGrabar(MesCalendarioModel model)
        {
            try
            {
                string extension = string.Empty;
                string archivo = string.Empty;

                if (this.FileName != string.Empty)
                {
                    int indexOf = this.FileName.LastIndexOf('.');
                    extension = this.FileName.Substring(indexOf + 1, this.FileName.Length - indexOf - 1);                     
                }

                WbMescalendarioDTO entity = new WbMescalendarioDTO
                {
                    Mescalcodi = model.Codigo,
                    Mescaltitulo = model.Titulo,
                    Mescaldescripcion = model.Descripcion,
                    Mescalanio = model.Anio,
                    Mescalmes = model.Mes,
                    Mescalcolor = model.ColorMes,
                    Mescalcolorsat = model.ColorSabado,
                    Mescalcolorsun = model.ColorDomingo,
                    Mescalcolortit = model.ColorTitulo,
                    Mescalcolorsubtit = model.ColorSubtitulo,
                    Mescalestado = model.Estado,
                    Mescalinfo = extension,
                    Mesdiacolor = model.ColorDia,
                    Mescalfecmodificacion = DateTime.Now,
                    Mescalusumodificacion = base.UserName
                };               

                int result = this.servicio.SaveWbMescalendario(entity);

                if (result > 0)
                {
                    if (this.FileName != string.Empty)
                    {                                              
                        archivo = string.Format(Constantes.InfografiaPortal, result) + extension;
                        string pathOrigen = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;
                        string pathDestino = ConfigurationManager.AppSettings[RutaDirectorio.RutaInfografia];
                        if (System.IO.File.Exists(pathDestino + archivo))
                        {
                            System.IO.File.Delete(pathDestino + archivo);
                        }

                        if (System.IO.File.Exists(pathOrigen + this.FileName))
                        {
                            System.IO.File.Move(pathOrigen + this.FileName, pathDestino + archivo);
                            System.IO.File.Delete(pathOrigen + this.FileName);
                        }
                    }
                }

                return Json(result);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite quitar la imagen
        /// </summary>
        /// <param name="idRegisto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult QuitarImagen(int idRegistro, string file)
        {
            try
            {
                string pathDestino = ConfigurationManager.AppSettings[RutaDirectorio.RutaInfografia];
                this.servicio.QuitarImagen(idRegistro);

                if (System.IO.File.Exists(pathDestino + file)) 
                {
                    System.IO.File.Delete(pathDestino + file);
                }

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        #region Tipo de Evento

        /// <summary>
        /// Pantalla Inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult TipoIndex()
        {
            TipoEventoModel model = new TipoEventoModel();            
            return View(model);
        }

        /// <summary>
        /// Permite listar los registros de la consulta
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult TipoListado()
        {           
            TipoEventoModel model = new TipoEventoModel();
            model.Listado = this.servicio.GetByCriteriaWbCaltipoventos();

            return PartialView(model);
        }

        /// <summary>
        /// Permite editar los datos de la subscripcion
        /// </summary>
        /// <param name="idRelacion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult TipoEditar(int id)
        {
            TipoEventoModel model = new TipoEventoModel();
            model.ListaIcono = WebHelper.ObtenerIconos();           

            if (id == 0)
            {
                model.Entidad = new WbCaltipoventoDTO();                 
            }
            else
            {
                model.Entidad = this.servicio.GetByIdWbCaltipovento(id);
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar la relación de equivalencia
        /// </summary>
        /// <param name="idRelacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TipoEliminar(int id)
        {
            try
            {
                this.servicio.DeleteWbCaltipovento(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite grabar la relación de equivalencia
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TipoGrabar(TipoEventoModel model)
        {
            try
            {


                WbCaltipoventoDTO entity = new WbCaltipoventoDTO
                {
                    Tipcalcodi = model.Codigo,
                    Tipcaldesc = model.Descripcion,
                    Tipcalcolor = model.Color,
                    Tipcalicono = model.Icono,
                    Lastuser = base.UserName,
                    Lastdate = DateTime.Now
                };

                int result = this.servicio.SaveWbCaltipovento(entity);

                

                return Json(result);
            }
            catch
            {
                return Json(-1);
            }
        }

        #endregion
    }
}
