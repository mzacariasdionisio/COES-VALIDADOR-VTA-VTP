using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Coordinacion.Helper;
using COES.MVC.Intranet.Areas.Eventos.Helper;
using COES.MVC.Intranet.Areas.Eventos.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Evento;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Informe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Eventos.Controllers
{
    public class InformeController : Controller
    {
        /// <summary>
        /// Instancia de la clase servicio de eventos
        /// </summary>
        EventosAppServicio servicio = new EventosAppServicio();
        DetalleEventoAppServicio detalleEvento = new DetalleEventoAppServicio();

        /// <summary>
        /// Instancia de la clases servicio de los informes
        /// </summary>
        InformeAppServicio informeServicio = new InformeAppServicio();
        EquipamientoAppServicio equipoServicio = new EquipamientoAppServicio();



        /// <summary>
        /// Lista de equipos
        /// </summary>
        public List<EqEquipoDTO> ListaEquipos
        {
            get
            {
                return (Session[DatosSesion.ListaEquipos] != null) ?
                    (List<EqEquipoDTO>)Session[DatosSesion.ListaEquipos] : new List<EqEquipoDTO>();
            }
            set { Session[DatosSesion.ListaEquipos] = value; }
        }



        /// <summary>
        /// Almacena las empresas del usario
        /// </summary>
        public List<int> ListaEmpresa
        {
            get
            {
                return (Session[DatosSesion.ListaEmpresa] != null) ?
                    (List<int>)Session[DatosSesion.ListaEmpresa] : new List<int>();
            }
            set { Session[DatosSesion.ListaEmpresa] = value; }
        }



        /// <summary>
        /// Pagina de informe del evento
        /// </summary>
        /// <param name="evento"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public ActionResult Index(int evento, int empresa, int? indicador)
        {
            InformeModel model = new InformeModel();
            model.Entidad = this.servicio.GetDetalleEvento(evento);

            bool idPermisoSCO = (new EventoHelper()).VerificarAcceso(Session[DatosSesion.SesionIdOpcion],
                User.Identity.Name, Acciones.PermisoSCO);

            bool idPermisoSEV = (new EventoHelper()).VerificarAcceso(Session[DatosSesion.SesionIdOpcion],
                User.Identity.Name, Acciones.PermisoSEV);

            if ((idPermisoSCO && empresa == -1) || (idPermisoSEV && empresa == 0) || (idPermisoSEV && empresa == -1))
            {
                bool flag = (indicador == -1 || indicador == 0) ? true : false;
                this.informeServicio.VerificarExistenciaInforme(evento, empresa, model.Entidad, User.Identity.Name, flag);
            }

            if (empresa == 0 && idPermisoSEV)
            {
                List<EventoInformeReporte> reporte = (new InformeAppServicio()).ObtenerInformeResumenIntranet(evento);
                model.ListaEmpresaCopia = reporte.Where(x => x.Emprcodi != -1).ToList();
            }
            else
            {
                model.ListaEmpresaCopia = new List<EventoInformeReporte>();
            }

            if (model.Entidad.Evenini != null)
            {
                DateTime fecha = (DateTime)model.Entidad.Evenini;
                model.PlazoPreliminarInicial = string.Empty;
                model.PlazoPreliminar = string.Empty;
                model.PlazoFinal = string.Empty;

                if (empresa != -1 && empresa != 0)
                {
                    model.PlazoPreliminar = fecha.AddHours((double)2.5M).ToString(Constantes.FormatoFechaHora);
                    model.PlazoFinal = fecha.AddHours((double)60).ToString(Constantes.FormatoFechaHora);
                }
                else if (empresa == -1)
                {
                    model.PlazoPreliminarInicial = fecha.AddHours((double)4).ToString(Constantes.FormatoFechaHora);
                    model.PlazoPreliminar = fecha.AddHours((double)6).ToString(Constantes.FormatoFechaHora);
                    model.PlazoFinal = fecha.AddHours((double)72).ToString(Constantes.FormatoFechaHora);
                }
            }

            model.ListaEquipos = this.servicio.GetEquiposPorEvento(evento);
            List<EveInformeDTO> informes = this.informeServicio.ObtenerInformesEmpresa(evento, empresa);
            this.ListaEquipos = this.informeServicio.ObtenerEquiposSeleccion(empresa);

            SiEmpresaDTO entityEmpresa = this.informeServicio.ObtenerEmpresa(empresa);
            model.EmpresaReporta = entityEmpresa.Emprnomb;
            model.IdEmpresa = entityEmpresa.Emprcodi;

            if (model.IdEmpresa == -1)
            {
                model.EmpresaReporta = ConstantesEvento.AreaSCO;
            }
            if (model.IdEmpresa == 0)
            {
                model.EmpresaReporta = ConstantesEvento.InformeConsolidadoSEV;
            }

            model.IdInformePreliminar = (informes.Where(x => x.Infversion ==
                ConstantesEvento.InformePreliminar).FirstOrDefault()).Eveninfcodi;
            model.IdInformeFinal = (informes.Where(x => x.Infversion ==
                ConstantesEvento.InformeFinal).FirstOrDefault()).Eveninfcodi;
            model.IdInformeComplementario = (informes.Where(x => x.Infversion ==
                ConstantesEvento.InformeComplementario).FirstOrDefault()).Eveninfcodi;
            model.IdInformeFile = (informes.Where(x => x.Infversion ==
                ConstantesEvento.InformeArchivos).FirstOrDefault()).Eveninfcodi;

            if (empresa == -1)
            {
                model.IdInformePreliminarInicial = (informes.Where(x => x.Infversion ==
               ConstantesEvento.InformePreliminarInicial).FirstOrDefault()).Eveninfcodi;

            }

            return View(model);
        }



        /// <summary>
        /// Permite obtener los datos del informe
        /// </summary>
        /// <param name="idInforme"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Informe(int idEvento, int idInforme, string indicador)
        {
            InformeModel model = new InformeModel();
            model.ListaItems = this.informeServicio.ObtenerItemInformeEvento(idInforme);
            EveEventoDTO evento = this.servicio.GetDetalleEvento(idEvento);
            EveInformeDTO informe = this.informeServicio.GetByIdEveInforme(idInforme);
            model.IndicadorEdicion = Constantes.NO;
            model.IndicadorFinalizar = Constantes.NO;

            bool perfilSEV = true; (new EventoHelper()).VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name,
                 Acciones.PermisoSEV);
            bool perfilSCO = (new EventoHelper()).VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name,
                Acciones.PermisoSCO);

            if (perfilSEV)
            {
                model.IndicadorEdicion = Constantes.SI;
                model.IndicadorCopia = Constantes.SI;
                model.IndicadorRevision = Constantes.SI;
            }
            else if (perfilSCO)
            {
                if (informe.Emprcodi == -1)
                {
                    if (!(informe.Infestado == ConstantesEvento.EstadoFinalizado ||
                        informe.Infestado == ConstantesEvento.EstadoRevisado))
                    {
                        model.IndicadorEdicion = Constantes.SI;
                        model.IndicadorFinalizar = Constantes.SI;
                    }
                }
            }

            model.IdInforme = idInforme;
            model.Indicador = Constantes.SI;
            if (indicador == ConstantesEvento.InformeArchivos)
            {
                model.Indicador = Constantes.NO;
            }

            var descripcion = model.ListaItems.Where(x => x.Itemnumber == 4).FirstOrDefault();
            if (descripcion != null)
            {
                model.IdItemDescripcion = descripcion.Infitemcodi;
                model.DesItemDescripcion = descripcion.Descomentario;
            }

            var analisis = model.ListaItems.Where(x => x.Itemnumber == 9).FirstOrDefault();

            if (analisis != null)
            {
                model.IdItemAnalisis = analisis.Infitemcodi;
                model.DesItemAnalisis = analisis.Descomentario;
            }
            model.TipoInforme = indicador;
            model.AsuntoEvento = evento.Tipoevenabrev;
            model.IdEmpresa = informe.Emprcodi;

            if (evento.Evenini != null)
            {
                DateTime fecha = (DateTime)evento.Evenini;
                model.FechaEvento = fecha.ToString(Constantes.FormatoFecha);
                model.HoraEvento = fecha.ToString(Constantes.FormatoOnlyHora);

                if (informe.Emprcodi == -1)
                {
                    if (indicador == ConstantesEvento.InformePreliminarInicial)
                    {
                        decimal plazo = 4;
                        model.PlazoMaximo = ConstantesEvento.TextoHasta
                       + fecha.AddHours((double)plazo).ToString(Constantes.FormatoFechaHora);

                        TimeSpan ts = DateTime.Now.Subtract(fecha.AddHours((double)plazo));

                        if (ts.TotalSeconds <= 0)
                        {
                            model.IndicadorPlazo = ConstantesEvento.TextoEnPlazo;
                        }
                        else
                        {
                            model.IndicadorPlazo = ConstantesEvento.TextoFueraPlazo;
                        }
                    }
                }

                if (indicador == ConstantesEvento.InformePreliminar)
                {
                    decimal plazo = 2.5M;
                    if (informe.Emprcodi == -1) plazo = 6;

                    model.PlazoMaximo = ConstantesEvento.TextoHasta
                        + fecha.AddHours((double)plazo).ToString(Constantes.FormatoFechaHora);

                    TimeSpan ts = DateTime.Now.Subtract(fecha.AddHours((double)plazo));

                    if (ts.TotalSeconds <= 0)
                    {
                        model.IndicadorPlazo = ConstantesEvento.TextoEnPlazo;
                    }
                    else
                    {
                        model.IndicadorPlazo = ConstantesEvento.TextoFueraPlazo;
                    }

                }
                if (indicador == ConstantesEvento.InformeFinal)
                {
                    decimal plazo = 60;
                    if (informe.Emprcodi == -1) plazo = 72;

                    model.PlazoMaximo = ConstantesEvento.TextoHasta +
                        fecha.AddHours((double)plazo).ToString(Constantes.FormatoFechaHora);

                    TimeSpan ts = DateTime.Now.Subtract(fecha.AddHours((double)plazo));

                    if (ts.TotalSeconds <= 0)
                    {
                        model.IndicadorPlazo = ConstantesEvento.TextoEnPlazo;
                    }
                    else
                    {
                        model.IndicadorPlazo = ConstantesEvento.TextoFueraPlazo;
                    }
                }
                if (indicador == ConstantesEvento.InformeComplementario)
                {
                    model.PlazoMaximo = ConstantesEvento.TextoDespues +
                        fecha.AddHours((double)60).ToString(Constantes.FormatoFechaHora);
                    model.IndicadorPlazo = ConstantesEvento.TextoEnPlazo;
                }

            }

            return PartialView(model);
        }



        /// <summary>
        /// Permite obtener los files del informe
        /// </summary>
        /// <param name="idInforme"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Anexo(int idInforme, string indicador)
        {
            InformeModel model = new InformeModel();
            model.ListaFiles = this.informeServicio.ObtenerFilesInformeEvento(idInforme);
            model.IdInforme = idInforme;
            model.IndicadorEdicion = indicador;
            return PartialView(model);
        }



        /// <summary>
        /// Permite grabar o actualizar el evento
        /// </summary>
        /// <param name="model"></param>
        /// <param name="idInforme"></param>
        /// <param name="itemNumber"></param>
        /// <param name="subItemNumber"></param>
        /// <param name="idItemInforme"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarElemento(ElementoModel model)
        {
            try
            {
                EveInformeItemDTO entity = new EveInformeItemDTO();
                entity.Potactiva = model.Potactiva;
                entity.Potreactiva = model.Potreactiva;
                entity.Equicodi = model.Equicodi;
                entity.Niveltension = model.Niveltension;
                entity.Desobservacion = model.Desobservacion;

                entity.Itemhora = model.Itemhora;
                entity.Senializacion = model.Senializacion;
                entity.Interrcodi = model.Interrcodi;
                entity.Ac = model.Ac;
                entity.Ra = model.Ra;
                entity.Ta = model.Ta;
                entity.Sa = model.Sa;
                entity.Rd = model.Rd;
                entity.Sd = model.Sd;
                entity.Td = model.Td;
                entity.Descomentario = model.Descomentario;
                entity.Sumininistro = model.Sumininistro;
                entity.Potenciamw = model.Potenciamw;
                entity.Proteccion = model.Proteccion;
                entity.Intinicio = (!string.IsNullOrEmpty(model.Intinicio)) ?
                    (DateTime?)DateTime.ParseExact(model.Intinicio, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture) : null;
                entity.Intfin = (!string.IsNullOrEmpty(model.Intfin)) ?
                    (DateTime?)DateTime.ParseExact(model.Intfin, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture) : null;
                entity.Subestacionde = model.Subestacionde;
                entity.Subestacionhasta = model.Subestacionhasta;
                entity.Eveninfcodi = model.IdInforme;
                entity.Itemnumber = model.ItemInforme;
                entity.Subitemnumber = model.SubItemInforme;
                entity.Infitemcodi = model.IdItemInforme;

                EveInformeItemDTO resultado = this.informeServicio.SaveEveInformeItem(entity);

                if (resultado.Intinicio != null && resultado.Intfin != null)
                {
                    resultado.DesIntInicio = ((DateTime)resultado.Intinicio).ToString(Constantes.FormatoFechaFull);
                    resultado.DesIntFin = ((DateTime)resultado.Intfin).ToString(Constantes.FormatoFechaFull);
                }

                model.Entidad = resultado;

                if (model.IdItemInforme > 0)
                {
                    model.Indicador = 2;
                }
                else
                {
                    model.Indicador = 1;
                }

                return Json(model);
            }
            catch
            {
                return Json(-1);
            }
        }



        /// <summary>
        /// Permite actualizar las cajas del texto
        /// </summary>
        /// <param name="idInforme"></param>
        /// <param name="itemNumber"></param>
        /// <param name="comentario"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarTexto(int idItemInforme, string comentario)
        {
            try
            {
                this.informeServicio.ActualizarTextoInforme(idItemInforme, comentario);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }



        /// <summary>
        /// Permite mostrar el formulario para agregar elementos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Elemento(int idInforme, int itemNumber, int subItemNumber, int idItemInforme)
        {
            ElementoModel model = new ElementoModel();
            model.IdInforme = idInforme;
            model.ItemInforme = itemNumber;
            model.SubItemInforme = subItemNumber;
            model.IdItemInforme = idItemInforme;

            if (idItemInforme != 0)
            {
                EveInformeItemDTO entity = this.informeServicio.GetByIdEveInformeItem(idItemInforme);

                model.Potactiva = entity.Potactiva;
                model.Potreactiva = entity.Potreactiva;
                model.Equicodi = entity.Equicodi;
                model.Niveltension = entity.Niveltension;
                model.Desobservacion = entity.Desobservacion;
                model.Itemhora = entity.Itemhora;
                model.Senializacion = entity.Senializacion;
                model.Interrcodi = entity.Interrcodi;
                model.Ac = entity.Ac;
                model.Ra = entity.Ra;
                model.Sa = entity.Sa;
                model.Ta = entity.Ta;
                model.Rd = entity.Rd;
                model.Sd = entity.Sd;
                model.Td = entity.Td;
                model.Descomentario = entity.Descomentario;
                model.Sumininistro = entity.Sumininistro;
                model.Potenciamw = entity.Potenciamw;
                model.Proteccion = entity.Proteccion;

                if (entity.Intinicio != null && entity.Intfin != null)
                {
                    model.Intinicio = ((DateTime)entity.Intinicio).ToString(Constantes.FormatoFechaFull);
                    model.Intfin = ((DateTime)entity.Intfin).ToString(Constantes.FormatoFechaFull);

                    TimeSpan duracion = ((DateTime)entity.Intfin) - ((DateTime)entity.Intinicio);
                    entity.Duracion = (decimal)(Math.Truncate(duracion.TotalMinutes * 100) / 100);
                }

                model.Subestacionde = entity.Subestacionde;
                model.Subestacionhasta = entity.Subestacionhasta;
            }

            if (itemNumber == 5)
            {
                if (subItemNumber == 1)
                {
                    model.ListaGeneradores = this.ListaEquipos.Where(x =>
                        ConstantesEvento.GetIdUnidades().Any(y => y == x.Famcodi)).ToList();
                }
                if (subItemNumber == 2)
                {
                    model.ListaGeneradores = this.ListaEquipos.Where(x =>
                        ConstantesEvento.GetIdLinea().Any(y => y == x.Famcodi)).ToList();
                }
                if (subItemNumber == 3)
                {
                    model.ListaGeneradores = this.ListaEquipos.Where(x =>
                        ConstantesEvento.GetIdTransformador().Any(y => y == x.Famcodi)).ToList();
                }
            }
            else if (itemNumber == 7)
            {
                model.ListaGeneradores = this.ListaEquipos.Where(x =>
                   ConstantesEvento.GetIdUnidades().Any(y => y == x.Famcodi)).ToList();
            }
            else if (itemNumber == 8)
            {
                model.ListaGeneradores = this.ListaEquipos.Where(x =>
                   ConstantesEvento.GetIdCelda().Any(y => y == x.Famcodi)).ToList();
            }
            else
            {
                model.ListaGeneradores = new List<EqEquipoDTO>();
            }

            model.ListaInterruptores = this.ListaEquipos.Where(x =>
                ConstantesEvento.GetIdInterruptor().Any(y => y == x.Famcodi)).ToList();

            return PartialView(model);
        }



        /// <summary>
        /// Permite quitar un elemento del reporte
        /// </summary>
        /// <param name="idItemInforme"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarElemento(int idItemInforme)
        {
            try
            {
                this.informeServicio.DeleteEveInformeItem(idItemInforme);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }



        /// <summary>
        /// Permite eliminar un file del reporte
        /// </summary>
        /// <param name="idFile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarFile(int idFile)
        {
            try
            {
                this.informeServicio.DeleteEveInformeFile(idFile);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }



        /// <summary>
        /// Permite editar la descripción de un archivo
        /// </summary>
        /// <param name="idFile"></param>
        /// <param name="descripcion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarFile(int idFile, string descripcion)
        {
            try
            {
                this.informeServicio.UpdateEveInformeFile(idFile, descripcion);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }



        /// <summary>
        /// Permite cargar los documentos
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload(int idInforme)
        {
            try
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    int lastIndex = file.FileName.LastIndexOf(Constantes.CaracterPunto);
                    string descripcion = file.FileName.Substring(0, lastIndex);
                    string extension = file.FileName.Substring(lastIndex + 1, file.FileName.Length - lastIndex - 1);

                    EveInformeFileDTO entity = new EveInformeFileDTO();
                    entity.Desfile = descripcion;
                    entity.Eveninfcodi = idInforme;
                    entity.Extfile = extension;
                    entity.Lastuser = User.Identity.Name;
                    int id = this.informeServicio.SaveEveInformeFile(entity);
                    string fileName = String.Format(ConstantesEvento.InformeFileName, id, extension);

                    string ruta = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaEvento];
                    file.SaveAs(ruta + fileName);

                    //file.SaveAs(AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeEvento + fileName);

                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }



        /// <summary>
        /// Permite cargar el archivo de interrupciones
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadInterrupcion()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeEvento;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = path + NombreArchivo.FormatoCargaInterrupcion;

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
        /// Permite obtener
        /// </summary>
        /// <param name="itemNumber"></param>
        /// <param name="subItemNumber"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerEquipos(string indicador)
        {
            List<EqEquipoDTO> list = new List<EqEquipoDTO>();

            if (indicador == ConstantesEvento.Unidad)
            {
                list = this.ListaEquipos.Where(x =>
                    ConstantesEvento.GetIdUnidades().Any(y => y == x.Famcodi)).ToList();
            }
            if (indicador == ConstantesEvento.Linea)
            {
                list = this.ListaEquipos.Where(x =>
                    ConstantesEvento.GetIdLinea().Any(y => y == x.Famcodi)).ToList();
            }
            if (indicador == ConstantesEvento.Transformador)
            {
                list = this.ListaEquipos.Where(x =>
                    ConstantesEvento.GetIdTransformador().Any(y => y == x.Famcodi)).ToList();
            }
            if (indicador == ConstantesEvento.Celda)
            {
                list = this.ListaEquipos.Where(x =>
                        ConstantesEvento.GetIdCelda().Any(y => y == x.Famcodi)).ToList();
            }

            return Json(list);
        }



        /// <summary>
        /// Permite generar el archivo del reporte
        /// </summary>
        /// <param name="idEvento"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(int idEvento, int idInforme, int idEmpresa, string tipo)
        {
            try
            {
                List<EveInformeItemDTO> list = this.informeServicio.ObtenerItemInformeEvento(idInforme);
                EveEventoDTO evento = this.servicio.GetDetalleEvento(idEvento);
                SiEmpresaDTO empresa = this.informeServicio.ObtenerEmpresa(idEmpresa);

                string path = ConfigurationManager.AppSettings[RutaDirectorio.RutaExportacionInformeEvento].ToString();
                string fileName = string.Empty;
                string pathLogo = string.Empty;
                int indicadorLogo = 1;

                if (idEmpresa != 0 && idEmpresa != -1)
                {
                    pathLogo = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaEvento] + RutaDirectorio.FileLogoEvento;
                    indicadorLogo = -1;

                    string fileGif = string.Format(NombreArchivo.LogoEmpresa, idEmpresa, Constantes.ExtensionGif);
                    string fileJpg = string.Format(NombreArchivo.LogoEmpresa, idEmpresa, Constantes.ExtensionJpg);
                    string filePng = string.Format(NombreArchivo.LogoEmpresa, idEmpresa, Constantes.ExtensionPng);

                    if (System.IO.File.Exists(pathLogo + fileGif))
                    {
                        pathLogo = pathLogo + fileGif;
                        indicadorLogo = 0;
                    }
                    else if (System.IO.File.Exists(pathLogo + fileJpg))
                    {
                        pathLogo = pathLogo + fileJpg;
                        indicadorLogo = 0;
                    }
                    else if (System.IO.File.Exists(pathLogo + filePng))
                    {
                        pathLogo = pathLogo + filePng;
                        indicadorLogo = 0;
                    }
                }

                string emprnomb = empresa.Emprnomb;
                if (idEmpresa == -1) emprnomb = "SCO";
                if (idEmpresa == 0) emprnomb = "SEV";

                if (tipo == Constantes.FormatoWord)
                {
                    fileName = NombreArchivo.FormatoInformeWORD;
                    (new COES.Servicios.Aplicacion.Eventos.Helper.WordDocument()).GenerarReporteInforme(evento, list, idInforme, path, fileName,
                        indicadorLogo, pathLogo, emprnomb);
                }

                if (tipo == Constantes.FormatoPDF)
                {
                    fileName = NombreArchivo.FormatoInformePDF;
                    (new COES.Servicios.Aplicacion.Eventos.Helper.PdfDocument()).GenerarReporteInforme(evento, list, idInforme, path, fileName,
                        indicadorLogo, pathLogo, emprnomb);
                }

                return Json(1);
            }
            catch (Exception)
            {
                return Json(-1);
            }
        }



        /// <summary>
        /// Permite abrir el archivo generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar(string tipo)
        {
            string file = string.Empty;
            string app = string.Empty;

            if (tipo == Constantes.FormatoWord)
            {
                file = NombreArchivo.FormatoInformeWORD;
                app = Constantes.AppWord;
            }
            if (tipo == Constantes.FormatoPDF)
            {
                file = NombreArchivo.FormatoInformePDF;
                app = Constantes.AppPdf;
            }

            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.RutaExportacionInformeEvento] + file;
            return File(fullPath, app, file);
        }



        /// <summary>
        /// Permite copiar los datos de los informes
        /// </summary>
        /// <param name="idOrigen"></param>
        /// <param name="idDestino"></param>
        /// <param name="indicador"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Copiar(int idOrigen, int idDestino, string indicador)
        {
            try
            {
                this.informeServicio.CopiarInforme(idOrigen, idDestino, indicador);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }



        /// <summary>
        /// Permite visualizar la auditoria del informe
        /// </summary>
        /// <param name="idInforme"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Auditoria(int idInforme)
        {
            InformeModel model = new InformeModel();
            EveInformeDTO informe = this.informeServicio.GetByIdEveInforme(idInforme);
            model.UsuarioInforme = informe.Lastuser;
            model.UsuarioRevisionInforme = informe.Lastuserrev;
            model.FechaInforme = (informe.Lastdate != null) ?
                ((DateTime)informe.Lastdate).ToString(Constantes.FormatoFechaHora) : string.Empty;
            model.FechaRevisionInforme = (informe.Lastdaterev != null) ?
                ((DateTime)informe.Lastdaterev).ToString(Constantes.FormatoFechaHora) : string.Empty;
            model.IndicadorCompletoInforme = Constantes.SI;
            model.EstadoInforme = ConstantesEvento.TextoInformeElaboracion;

            if (informe.Emprcodi == 0)
            {
                model.IndicadorCompletoInforme = Constantes.NO;
            }

            if (informe.Infestado == ConstantesEvento.EstadoFinalizado)
            {
                model.EstadoInforme = ConstantesEvento.TextoInformeFinalizado;
            }
            else if (informe.Infestado == ConstantesEvento.EstadoRevisado)
            {
                model.EstadoInforme = ConstantesEvento.TextoInformeRevisado;
            }

            if (informe.Indplazo == Constantes.SI)
            {
                model.PlazoInforme = ConstantesEvento.TextoEnPlazo;
            }
            else if (informe.Indplazo == Constantes.NO)
            {
                model.PlazoInforme = ConstantesEvento.TextoFueraPlazo;
            }

            return PartialView(model);
        }



        /// <summary>
        /// Permite copiar los datos entre los reporte de las empresas
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idDestinos"></param>
        /// <param name="tipo"></param>
        /// <param name="indicador"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CopiarEmpresa(int idEvento, int idEmpresa, int idDestino, string tipo, string indicador)
        {
            try
            {
                this.informeServicio.CopiarInformeEmpresa(idEvento, idEmpresa, idDestino, tipo, indicador);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }



        /// <summary>
        /// Permite finalizar el reporte seleccionado
        /// </summary>
        /// <param name="idInforme"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Finalizar(int idEvento, string tipo, int idInforme, int idEmpresa)
        {
            try
            {
                int indicador = this.informeServicio.FinalizarInforme(idEvento, tipo, idInforme, User.Identity.Name, idEmpresa);
                return Json(indicador);
            }
            catch
            {
                return Json(-1);
            }
        }



        /// <summary>
        /// Permite dar por revisado el informe 
        /// </summary>
        /// <param name="idEvento"></param>
        /// <param name="tipo"></param>
        /// <param name="idInforme"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Revisar(int idEvento, string tipo, int idInforme)
        {
            try
            {
                informeServicio.RevisarInforme(idEvento, tipo, idInforme, User.Identity.Name);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }



        /// <summary>
        /// Permite validar si está en plazo
        /// </summary>
        /// <param name="idEvento"></param>
        /// <param name="indicador"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarFinalizar(int idEvento, string indicador, int idEmpresa)
        {
            int resultado = 1;
            decimal plazo = -1;

            if (idEmpresa != -1)
            {
                if (indicador == ConstantesEvento.InformePreliminar) plazo = 2.5M;
                if (indicador == ConstantesEvento.InformeFinal) plazo = 60;
            }
            else
            {
                if (indicador == ConstantesEvento.InformePreliminarInicial) plazo = 4;
                if (indicador == ConstantesEvento.InformePreliminar) plazo = 6;
                if (indicador == ConstantesEvento.InformeFinal) plazo = 72;
            }

            if (plazo > 0)
            {
                EveEventoDTO evento = this.servicio.GetDetalleEvento(idEvento);
                if (evento.Evenini != null)
                {
                    DateTime fecha = (DateTime)evento.Evenini;
                    TimeSpan ts = DateTime.Now.Subtract(fecha.AddHours((double)plazo));

                    if (ts.TotalSeconds > 0)
                    {
                        resultado = 2;
                    }
                }
            }

            return Json(resultado);
        }



        /// <summary>
        /// Permite realizar la importación
        /// </summary>
        /// <param name="clasificacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarInterrupcion(int idInforme)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeEvento +
                    NombreArchivo.FormatoCargaInterrupcion;
                List<string> errors = new List<string>();
                List<EveInformeItemDTO> entitys = InformeHelper.ImporatInterrupcion(path, idInforme, out errors);

                if (errors.Count == 0)
                {
                    this.informeServicio.GrabarInterrupciones(entitys);
                    return Json(1);
                }
                else
                {
                    return Json(errors);
                }
            }
            catch
            {
                return Json(-1);
            }
        }



        /// <summary>
        /// Permite pintar la data por defecto
        /// </summary>
        /// <param name="data">Matriz de datos</param>
        /// <param name="columna">Número de columnas</param>
        /// <returns></returns>
        private string[][] PintarCeldas(string[][] data, int columna)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new string[columna];

                for (int j = 0; j < columna; j++)
                {
                    data[i][j] = string.Empty;
                }
            }

            return data;
        }



        /// <summary>
        /// Permite obtener la configuracion de la grilla
        /// </summary>
        /// <param name="idEvento">Identificador de evento</param>
        /// <param name="idInforme">Identificador de informe</param>
        /// <param name="indicador">indicador</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDataGrilla(int idEvento, int idInforme, string indicador)
        {


            InformeModel model = new InformeModel();
            model.ListaItems = this.informeServicio.ObtenerItemInformeEvento(idInforme);
            EveEventoDTO evento = this.servicio.GetDetalleEvento(idEvento);
            EveInformeDTO informe = this.informeServicio.GetByIdEveInforme(idInforme);

            var descripcion = model.ListaItems.Where(x => x.Itemnumber == 4).FirstOrDefault();
            if (descripcion != null)
            {
                model.IdItemDescripcion = descripcion.Infitemcodi;
                model.DesItemDescripcion = descripcion.Descomentario;
            }

            var analisis = model.ListaItems.Where(x => x.Itemnumber == 9).FirstOrDefault();

            if (analisis != null)
            {
                model.IdItemAnalisis = analisis.Infitemcodi;
                model.DesItemAnalisis = analisis.Descomentario;
            }

            model.TipoInforme = indicador;
            model.AsuntoEvento = evento.Tipoevenabrev;
            model.IdEmpresa = informe.Emprcodi;

            if (evento.Evenini != null)
            {
                DateTime fecha = (DateTime)evento.Evenini;
                model.FechaEvento = fecha.ToString(Constantes.FormatoFecha);
                model.HoraEvento = fecha.ToString(Constantes.FormatoOnlyHora);
            }

            SiCambioTurnoDTO datos = new SiCambioTurnoDTO();
            List<SiCambioTurnoSubseccionDTO> list = new List<SiCambioTurnoSubseccionDTO>();
            // this.servicio.ObtenerContenidoSecciones(fechaConsulta, turno, out datos, out indicador, User.Identity.Name);
            List<MergeModel> merge = new List<MergeModel>();
            List<int> titulos = new List<int>();
            List<int> subTitulos = new List<int>();
            List<int> agrupaciones = new List<int>();
            List<int> comentarios = new List<int>();
            List<int> adicionales = new List<int>();
            List<int> finales = new List<int>();
            List<int> mantenimientos = new List<int>();
            List<ValidacionListaCelda> validaciones = new List<ValidacionListaCelda>();
            List<CeldaValidacionLongitud> longitudes = new List<CeldaValidacionLongitud>();
            List<AlineacionCelda> centros = new List<AlineacionCelda>();
            List<AlineacionCelda> derechos = new List<AlineacionCelda>();
            List<TipoCelda> tipos = new List<TipoCelda>();
            List<ValidacionListaCelda> fechahora = new List<ValidacionListaCelda>();

            List<EqEquipoDTO> listaEquipo = equipoServicio.ListadoEquipoNombre(ConstantesAppServicio.ParametroDefecto); //new List<string>();//

            //lista de generadores
            List<string> listaGenerador = listaEquipo.Where(
                    x => x.Famcodi == 39 || x.Famcodi == 38 || x.Famcodi == 4 || x.Famcodi == 2 || x.Famcodi == 37 ||
                         x.Famcodi == 36 || x.Famcodi == 5 || x.Famcodi == 3 || x.Famcodi == -1)
                .Select(x => x.Equinomb)
                .ToList();

            //lista de lineas
            List<string> listaLinea = listaEquipo.Where(
                x => x.Famcodi == 8 || x.Famcodi == -1).Select(x => x.Equinomb).ToList();

            //lista de transformadores
            List<string> listaTransformador = listaEquipo.Where(
                x => x.Famcodi == 9 || x.Famcodi == 10 || x.Famcodi == 26 || x.Famcodi == 27 || x.Famcodi == 29 ||
                     x.Famcodi == -1).Select(x => x.Equinomb).ToList();

            //lista de protecciones
            List<string> listaProteccion = listaEquipo.Where(
                x => x.Famcodi == -1 || x.Famcodi == 1 || x.Famcodi == 2 || x.Famcodi == 3 ||
                     x.Famcodi == 4 || x.Famcodi == 5 || x.Famcodi == 6 || x.Famcodi == 36 ||
                     x.Famcodi == 37 || x.Famcodi == 38 || x.Famcodi == 39).Select(x => x.Equinomb).ToList();

            //lista de interruptores
            List<string> listaInterruptor = listaEquipo.Where(
                x => x.Famcodi == 16 ||
                     x.Famcodi == -1).Select(x => x.Equinomb).ToList();

            var subList = model.ListaItems.Where(x => x.Itemnumber == 5 && x.Subitemnumber == 1).ToList();
            var subList1 = model.ListaItems.Where(x => x.Itemnumber == 5 && x.Subitemnumber == 2).ToList();
            var subList2 = model.ListaItems.Where(x => x.Itemnumber == 5 && x.Subitemnumber == 3).ToList();
            var subList3 = model.ListaItems.Where(x => x.Itemnumber == 6).ToList();
            var subList4 = model.ListaItems.Where(x => x.Itemnumber == 7).ToList();
            var subList5 = model.ListaItems.Where(x => x.Itemnumber == 8).ToList();
            var subList6 = model.ListaItems.Where(x => x.Itemnumber == 10).ToList();
            var subList7 = model.ListaItems.Where(x => x.Itemnumber == 11).ToList();
            var subList8 = model.ListaItems.Where(x => x.Itemnumber == 12).ToList();
            var subList9 = model.ListaItems.Where(x => x.Itemnumber == 13).ToList();

            int registrosTotal = 29 + 1 + 1 +
                                 (subList.Count == 0 ? 1 : subList.Count) +
                                 (subList1.Count == 0 ? 1 : subList1.Count) +
                                 (subList2.Count == 0 ? 1 : subList2.Count) +
                                 (subList3.Count == 0 ? 1 : subList3.Count) +
                                 (subList4.Count == 0 ? 1 : subList4.Count) +
                                 (subList5.Count == 0 ? 1 : subList5.Count) +
                                 (subList6.Count == 0 ? 1 : subList6.Count) +
                                 (subList7.Count == 0 ? 1 : subList7.Count) +
                                 (subList8.Count == 0 ? 1 : subList8.Count) +
                                 (subList9.Count == 0 ? 1 : subList9.Count);


            string[][] data = new string[registrosTotal + 1][];
            data = PintarCeldas(data, 13);

            #region Pintado de datos

            int indice = 0;

            data[indice][0] = "1. EVENTO";
            merge.Add(new MergeModel { row = indice, col = 2, rowspan = 1, colspan = 11 });
            titulos.Add(indice);
            data[indice][1] = evento.Tipoevenabrev;

            indice++;
            data[indice][0] = "2. FECHA";
            merge.Add(new MergeModel { row = indice, col = 2, rowspan = 1, colspan = 11 });
            titulos.Add(indice);
            data[indice][1] = ((DateTime)evento.Evenini).ToString(Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            indice++;
            data[indice][0] = "3. HORA";
            merge.Add(new MergeModel { row = indice, col = 2, rowspan = 1, colspan = 11 });
            data[indice][1] = ((DateTime)evento.Evenini).ToString(Constantes.FormatoHoraMinuto,
                CultureInfo.InvariantCulture);

            titulos.Add(indice);
            indice++;

            data[indice][0] = "4. DESCRIPCIÓN DEL EVENTO";
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 13 });
            titulos.Add(indice);
            indice++;

            data[indice][0] = model.DesItemDescripcion;

            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 13 });
            indice++;

            data[indice][0] = "5. CONDICIONES OPERATIVAS PREVIAS";
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 13 });
            titulos.Add(indice);
            indice++;

            data[indice][0] = "A. GENERACIÓN";
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 13 });

            subTitulos.Add(indice);
            indice++;

            data[indice][0] = "N°";
            data[indice][1] = "Subestación - Unidad";

            data[indice][4] = "Potencia Activa(MW)";
            data[indice][6] = "Potencia Reactiva(MVAR)";
            data[indice][9] = "Observaciones";

            subTitulos.Add(indice);

            merge.Add(new MergeModel { row = indice, col = 1, rowspan = 1, colspan = 3 });

            merge.Add(new MergeModel { row = indice, col = 4, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice, col = 6, rowspan = 1, colspan = 3 });
            merge.Add(new MergeModel { row = indice, col = 9, rowspan = 1, colspan = 4 });

            //contenido
            indice++;

            //5. condiciones operativas previas
            var idGeneracion = model.IdInforme + "_5_1";

            //var subList = model.ListaItems.Where(x => x.Itemnumber == 5 && x.Subitemnumber == 1).ToList();
            var indiceItem = 1;
            foreach (var item in subList)
            {
                merge.Add(new MergeModel { row = indice, col = 1, rowspan = 1, colspan = 3 });
                merge.Add(new MergeModel { row = indice, col = 4, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 6, rowspan = 1, colspan = 3 });
                merge.Add(new MergeModel { row = indice, col = 9, rowspan = 1, colspan = 4 });

                var idGeneracion1 = idGeneracion + "_" + item.Infitemcodi;
                data[indice][0] = indiceItem.ToString();

                if (item.Equicodi == null)
                    item.Equicodi = -1;

                data[indice][1] = (listaEquipo.Where(x => x.Equicodi == item.Equicodi).ToList())[0].Equinomb;

                data[indice][4] = item.Potactiva.ToString();
                data[indice][6] = item.Potreactiva.ToString();
                data[indice][9] = item.Desobservacion;

                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 1, Elementos = listaGenerador });
                //tipos.Add(new TipoCelda { Row = indice, Column = 0, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 4, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 6, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 1, Tipo = "numeric" }); //combo

                indiceItem++;
                indice++;
            }

            if (subList.Count == 0)
            {
                merge.Add(new MergeModel { row = indice, col = 1, rowspan = 1, colspan = 3 });
                merge.Add(new MergeModel { row = indice, col = 4, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 6, rowspan = 1, colspan = 3 });
                merge.Add(new MergeModel { row = indice, col = 9, rowspan = 1, colspan = 4 });

                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 1, Elementos = listaGenerador });
                //tipos.Add(new TipoCelda { Row = indice, Column = 0, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 4, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 6, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 1, Tipo = "numeric" }); //combo

                indice++;
            }

            data[indice][0] = "B. FLUJO DE POTENCIAS EN LAS LÍNEAS";
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 13 });
            subTitulos.Add(indice);
            indice++;

            data[indice][0] = "N°";
            data[indice][1] = "Código L.T.";
            data[indice][3] = "Subestaciones";
            data[indice][7] = "P.Activa(MW)";
            data[indice][8] = "P.Reactiva(MVAR)";
            data[indice][9] = "Observaciones";
            subTitulos.Add(indice);

            data[indice + 1][3] = "De";
            data[indice + 1][5] = "A";
            subTitulos.Add(indice + 1);

            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 2, colspan = 1 });
            merge.Add(new MergeModel { row = indice, col = 1, rowspan = 2, colspan = 2 });
            merge.Add(new MergeModel { row = indice, col = 3, rowspan = 1, colspan = 4 });
            merge.Add(new MergeModel { row = indice, col = 7, rowspan = 2, colspan = 1 });
            merge.Add(new MergeModel { row = indice, col = 8, rowspan = 2, colspan = 1 });
            merge.Add(new MergeModel { row = indice, col = 9, rowspan = 2, colspan = 4 });

            merge.Add(new MergeModel { row = indice + 1, col = 3, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice + 1, col = 5, rowspan = 1, colspan = 2 });

            indice++;
            indice++;

            //var subList1 = model.ListaItems.Where(x => x.Itemnumber == 5 && x.Subitemnumber == 2).ToList();
            int indice1 = 1;
            foreach (var item in subList1)
            {
                merge.Add(new MergeModel { row = indice, col = 1, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 3, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 5, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 9, rowspan = 1, colspan = 4 });

                data[indice][0] = indice1.ToString();

                if (item.Equicodi == null)
                    item.Equicodi = -1;

                data[indice][1] = (listaEquipo.Where(x => x.Equicodi == item.Equicodi).ToList())[0].Equinomb;

                data[indice][3] = item.Subestacionde; //2
                data[indice][5] = item.Subestacionhasta; //3
                data[indice][7] = item.Potactiva.ToString(); //4
                data[indice][8] = item.Potreactiva.ToString(); //6
                data[indice][9] = item.Desobservacion;

                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 1, Elementos = listaLinea });
                //tipos.Add(new TipoCelda { Row = indice, Column = 0, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 7, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 8, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 1, Tipo = "numeric" }); //combo
                indice1++;
                indice++;
            }

            if (subList1.Count == 0)
            {
                merge.Add(new MergeModel { row = indice, col = 1, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 3, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 5, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 9, rowspan = 1, colspan = 4 });

                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 1, Elementos = listaLinea });
                //tipos.Add(new TipoCelda { Row = indice, Column = 0, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 7, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 8, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 1, Tipo = "numeric" }); //combo
                indice++;
            }

            //indice++;
            data[indice][0] = "C. FLUJO DE POTENCIAS EN LOS TRANSFORMADORES";
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 13 });
            subTitulos.Add(indice);
            indice++;

            data[indice][0] = "N°";
            data[indice][1] = "Subestación";
            data[indice][4] = "Potencia Activa(MW)";
            data[indice][6] = "Potencia Reactiva(MVAR)";
            data[indice][9] = "N. Tensión";

            data[indice][10] = "Observaciones";
            subTitulos.Add(indice);

            merge.Add(new MergeModel { row = indice, col = 1, rowspan = 1, colspan = 3 });
            merge.Add(new MergeModel { row = indice, col = 4, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice, col = 6, rowspan = 1, colspan = 3 });
            merge.Add(new MergeModel { row = indice, col = 10, rowspan = 1, colspan = 3 });

            indice++;

            //var subList2 = model.ListaItems.Where(x => x.Itemnumber == 5 && x.Subitemnumber == 3).ToList();
            int indice2 = 1;
            foreach (var item in subList2)
            {
                merge.Add(new MergeModel { row = indice, col = 1, rowspan = 1, colspan = 3 });
                merge.Add(new MergeModel { row = indice, col = 4, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 6, rowspan = 1, colspan = 3 });
                merge.Add(new MergeModel { row = indice, col = 10, rowspan = 1, colspan = 3 });

                data[indice][0] = indice2.ToString();

                if (item.Equicodi == null)
                    item.Equicodi = -1;

                data[indice][1] = (listaEquipo.Where(x => x.Equicodi == item.Equicodi).ToList())[0].Equinomb;

                data[indice][4] = item.Potactiva.ToString();
                data[indice][6] = item.Potreactiva.ToString();
                data[indice][9] = item.Niveltension.ToString();
                data[indice][10] = item.Desobservacion;

                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 1, Elementos = listaTransformador });
                //tipos.Add(new TipoCelda { Row = indice, Column = 0, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 4, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 6, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 9, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 1, Tipo = "numeric" }); //combo

                indice2++;
                indice++;
            }

            if (subList2.Count == 0)
            {
                merge.Add(new MergeModel { row = indice, col = 1, rowspan = 1, colspan = 3 });
                merge.Add(new MergeModel { row = indice, col = 4, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 6, rowspan = 1, colspan = 3 });
                merge.Add(new MergeModel { row = indice, col = 10, rowspan = 1, colspan = 3 });

                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 1, Elementos = listaTransformador });
                //tipos.Add(new TipoCelda { Row = indice, Column = 0, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 4, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 6, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 9, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 1, Tipo = "numeric" }); //combo

                indice++;
            }

            data[indice][0] = "6. SECUENCIA CRONOLÓGICA";
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 13 });
            titulos.Add(indice);
            indice++;

            data[indice][0] = "Hora";
            data[indice][1] = "Descripción del evento";

            merge.Add(new MergeModel { row = indice, col = 1, rowspan = 1, colspan = 12 });
            subTitulos.Add(indice);
            indice++;

            //var subList3 = model.ListaItems.Where(x => x.Itemnumber == 6).ToList();
            foreach (var item in subList3)
            {
                merge.Add(new MergeModel { row = indice, col = 1, rowspan = 1, colspan = 12 });

                data[indice][0] = item.Itemhora;
                data[indice][1] = item.Desobservacion;
                fechahora.Add(new ValidacionListaCelda { Row = indice, Column = 0 }); //fecha
                indice++;
            }

            if (subList3.Count == 0)
            {
                merge.Add(new MergeModel { row = indice, col = 1, rowspan = 1, colspan = 12 });
                fechahora.Add(new ValidacionListaCelda { Row = indice, Column = 0 }); //fecha
                indice++;
            }

            //indice++;
            data[indice][0] = "7. ACTUACIÓN DE LAS PROTECCIONES";
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 13 });
            titulos.Add(indice);
            indice++;
            data[indice][0] = "Subestación - Equipo";

            data[indice][2] = "Señalización";
            data[indice][4] = "INT";
            data[indice][6] = "A/C";

            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice, col = 2, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice, col = 4, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice, col = 6, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice, col = 8, rowspan = 1, colspan = 2 });
            subTitulos.Add(indice);

            indice++;


            //var subList4 = model.ListaItems.Where(x => x.Itemnumber == 7).ToList();
            foreach (var item in subList4)
            {
                merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 2, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 4, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 6, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 8, rowspan = 1, colspan = 2 });

                if (item.Equicodi == null)
                    item.Equicodi = -1;

                data[indice][0] = (listaEquipo.Where(x => x.Equicodi == item.Equicodi).ToList())[0].Equinomb;

                data[indice][2] = item.Senializacion;
                data[indice][4] = item.Desobservacion;
                data[indice][6] = item.Ac;

                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 0, Elementos = listaProteccion });
                tipos.Add(new TipoCelda { Row = indice, Column = 0, Tipo = "numeric" }); //combo
                indice++;
            }

            if (subList4.Count == 0)
            {
                merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 2, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 4, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 6, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 8, rowspan = 1, colspan = 2 });

                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 0, Elementos = listaProteccion });
                tipos.Add(new TipoCelda { Row = indice, Column = 0, Tipo = "numeric" }); //combo
                indice++;
            }

            data[indice][0] = "8. CONTADOR DE INTERRUPTORES Y PARARRAYOS";
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 13 });
            titulos.Add(indice);

            indice++;
            data[indice][0] = "Subestación - Celda";

            data[indice][2] = "Punto Interrupción";
            data[indice][4] = "Antes";
            data[indice][7] = "Después";

            data[indice + 1][4] = "R";
            data[indice + 1][5] = "S";
            data[indice + 1][6] = "T";

            data[indice + 1][7] = "R";
            data[indice + 1][8] = "S";
            data[indice + 1][9] = "T";

            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 2, colspan = 2 });
            merge.Add(new MergeModel { row = indice, col = 2, rowspan = 2, colspan = 2 });
            merge.Add(new MergeModel { row = indice, col = 4, rowspan = 1, colspan = 3 });
            merge.Add(new MergeModel { row = indice, col = 7, rowspan = 1, colspan = 3 });
            subTitulos.Add(indice);
            subTitulos.Add(indice + 1);

            //contenido
            merge.Add(new MergeModel { row = indice + 1, col = 0, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice + 1, col = 2, rowspan = 1, colspan = 2 });

            indice += 2;

            //contenido
            //var subList5 = model.ListaItems.Where(x => x.Itemnumber == 8).ToList();
            foreach (var item in subList5)
            {
                merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 2, rowspan = 1, colspan = 2 });

                if (item.Equicodi == null)
                    item.Equicodi = -1;

                data[indice][0] = (listaEquipo.Where(x => x.Equicodi == item.Equicodi).ToList())[0].Equinomb;

                int codigoInterruptor = (item.Interrcodi == null ? -1 : (int)item.Interrcodi);
                data[indice][2] = (listaEquipo.Where(x => x.Equicodi == codigoInterruptor).ToList())[0].Equinomb;

                data[indice][4] = ((int)item.Ra).ToString();
                data[indice][5] = ((int)item.Sa).ToString();
                data[indice][6] = ((int)item.Ta).ToString();
                data[indice][7] = ((int)item.Rd).ToString();
                data[indice][8] = ((int)item.Sd).ToString();
                data[indice][9] = ((int)item.Td).ToString();

                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 0, Elementos = listaProteccion });
                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 2, Elementos = listaInterruptor });
                tipos.Add(new TipoCelda { Row = indice, Column = 0, Tipo = "numeric" }); //combo
                tipos.Add(new TipoCelda { Row = indice, Column = 2, Tipo = "numeric" }); //combo
                tipos.Add(new TipoCelda { Row = indice, Column = 4, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 5, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 6, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 7, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 8, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 9, Tipo = "numeric" });


                indice++;
            }

            if (subList5.Count == 0)
            {
                merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 2, rowspan = 1, colspan = 2 });

                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 0, Elementos = listaProteccion });
                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 2, Elementos = listaInterruptor });
                tipos.Add(new TipoCelda { Row = indice, Column = 0, Tipo = "numeric" }); //combo
                tipos.Add(new TipoCelda { Row = indice, Column = 2, Tipo = "numeric" }); //combo
                tipos.Add(new TipoCelda { Row = indice, Column = 4, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 5, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 6, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 7, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 8, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 9, Tipo = "numeric" });
                indice++;
            }

            data[indice][0] = "9. ANÁLISIS DEL EVENTO";
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 13 });
            titulos.Add(indice);
            indice++;

            //contenido
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 13 });

            if (model.DesItemAnalisis != null && model.DesItemAnalisis != "")
                data[indice][0] = model.DesItemAnalisis;

            indice++;

            data[indice][0] = "10. SUMINISTROS INTERRUMPIDOS";
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 13 });
            titulos.Add(indice);
            indice++;

            data[indice][0] = "N°";
            data[indice][1] = "Suministro";
            data[indice][2] = "Potencia MW";
            data[indice][4] = "Tiempo de desconexión (min)";
            data[indice][7] = "Protección";
            subTitulos.Add(indice);
            data[indice + 1][4] = "Inicio";
            data[indice + 1][5] = "Final";
            data[indice + 1][6] = "Duración";
            subTitulos.Add(indice + 1);

            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 2, colspan = 1 });
            merge.Add(new MergeModel { row = indice, col = 1, rowspan = 2, colspan = 1 });
            merge.Add(new MergeModel { row = indice, col = 2, rowspan = 2, colspan = 2 });

            merge.Add(new MergeModel { row = indice, col = 4, rowspan = 1, colspan = 3 });
            merge.Add(new MergeModel { row = indice, col = 7, rowspan = 2, colspan = 2 });

            indice += 2;

            //contenido
            //var subList6 = model.ListaItems.Where(x => x.Itemnumber == 10).ToList();
            var indice6 = 1;
            foreach (var item in subList6)
            {
                merge.Add(new MergeModel { row = indice, col = 2, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 7, rowspan = 1, colspan = 2 });

                data[indice][0] = indice6.ToString();
                data[indice][1] = item.Sumininistro;
                data[indice][2] = item.Potenciamw.ToString();
                data[indice][4] = (((DateTime)item.Intinicio).ToString("dd/MM/yyyy HH:mm:ss"));
                data[indice][5] = (((DateTime)item.Intfin).ToString("dd/MM/yyyy HH:mm:ss"));

                int minutos = 0;
                try
                {
                    TimeSpan ts = (DateTime)item.Intfin - (DateTime)item.Intinicio;
                    minutos = Convert.ToInt32(Math.Round(ts.TotalMinutes, 0));
                }
                catch
                {

                }

                if (minutos < 0) minutos = 0;

                data[indice][6] = minutos.ToString();
                data[indice][7] = item.Proteccion;

                //tipos.Add(new TipoCelda { Row = indice, Column = 0, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 2, Tipo = "numeric" });
                fechahora.Add(new ValidacionListaCelda { Row = indice, Column = 4 }); //fecha
                fechahora.Add(new ValidacionListaCelda { Row = indice, Column = 5 }); //fecha
                indice6++;
                indice++;
            }


            if (subList6.Count == 0)
            {
                merge.Add(new MergeModel { row = indice, col = 2, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 7, rowspan = 1, colspan = 2 });
                //tipos.Add(new TipoCelda { Row = indice, Column = 0, Tipo = "numeric" });
                tipos.Add(new TipoCelda { Row = indice, Column = 2, Tipo = "numeric" });
                fechahora.Add(new ValidacionListaCelda { Row = indice, Column = 4 }); //fecha
                fechahora.Add(new ValidacionListaCelda { Row = indice, Column = 5 }); //fecha
                indice++;
            }


            data[indice][0] = "11. CONCLUSIONES";
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 13 });
            titulos.Add(indice);
            indice++;

            data[indice][0] = "N°";
            data[indice][1] = "Conclusión";
            merge.Add(new MergeModel { row = indice, col = 1, rowspan = 1, colspan = 12 });
            subTitulos.Add(indice);
            indice++;

            //contenido
            //var subList7 = model.ListaItems.Where(x => x.Itemnumber == 11).ToList();
            int indice7 = 1;
            foreach (var item in subList7)
            {
                merge.Add(new MergeModel { row = indice, col = 1, rowspan = 1, colspan = 12 });
                data[indice][0] = indice7.ToString();
                data[indice][1] = item.Desobservacion;
                //tipos.Add(new TipoCelda { Row = indice, Column = 0, Tipo = "numeric" });
                indice7++;
                indice++;
            }

            if (subList7.Count == 0)
            {
                merge.Add(new MergeModel { row = indice, col = 1, rowspan = 1, colspan = 12 });
                //tipos.Add(new TipoCelda { Row = indice, Column = 0, Tipo = "numeric" });
                indice++;
            }

            data[indice][0] = "12. ACCIONES EJECUTADAS";
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 13 });
            titulos.Add(indice);
            indice++;

            data[indice][0] = "N°";
            data[indice][1] = "Acción ejecutada";
            merge.Add(new MergeModel { row = indice, col = 1, rowspan = 1, colspan = 12 });
            subTitulos.Add(indice);
            indice++;

            //contenido
            //var subList8 = model.ListaItems.Where(x => x.Itemnumber == 12).ToList();
            int indice8 = 1;
            foreach (var item in subList8)
            {
                merge.Add(new MergeModel { row = indice, col = 1, rowspan = 1, colspan = 12 });
                data[indice][0] = indice8.ToString();
                data[indice][1] = item.Desobservacion;
                //tipos.Add(new TipoCelda { Row = indice, Column = 0, Tipo = "numeric" });
                indice8++;
                indice++;

            }


            if (subList8.Count == 0)
            {
                merge.Add(new MergeModel { row = indice, col = 1, rowspan = 1, colspan = 12 });
                //tipos.Add(new TipoCelda { Row = indice, Column = 0, Tipo = "numeric" });
                indice++;
            }

            data[indice][0] = "13. OBSERVACIONES Y RECOMENDACIONES";
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 13 });
            titulos.Add(indice);
            indice++;

            data[indice][0] = "N°";
            data[indice][1] = "Observación / Recomendación";
            merge.Add(new MergeModel { row = indice, col = 1, rowspan = 1, colspan = 12 });
            subTitulos.Add(indice);
            indice++;

            //contenido
            //var subList9 = model.ListaItems.Where(x => x.Itemnumber == 13).ToList();
            int indice9 = 1;
            foreach (var item in subList9)
            {
                merge.Add(new MergeModel { row = indice, col = 1, rowspan = 1, colspan = 12 });
                data[indice][0] = indice9.ToString();
                data[indice][1] = item.Desobservacion;
                //tipos.Add(new TipoCelda { Row = indice, Column = 0, Tipo = "numeric" });
                indice9++;
                indice++;
            }

            if (subList9.Count == 0)
            {
                merge.Add(new MergeModel { row = indice, col = 1, rowspan = 1, colspan = 12 });
                //tipos.Add(new TipoCelda { Row = indice, Column = 0, Tipo = "numeric" });
                indice++;
            }

            #endregion

            model.Datos = data;
            model.Merge = merge.ToArray();
            model.IndicesTitulo = titulos.ToArray();
            model.IndicesSubtitulo = subTitulos.ToArray();
            model.IndicesAgrupacion = agrupaciones.ToArray();
            model.IndicesComentario = comentarios.ToArray();
            model.IndicesAdicional = adicionales.ToArray();
            model.IndicesFinal = finales.ToArray();
            model.IndiceMantenimiento = mantenimientos.ToArray();
            model.IdPersona = datos.Coordinadorresp;
            model.Validaciones = validaciones;
            model.Longitudes = longitudes;

            model.Centros = centros;
            model.Derechos = derechos;
            model.Tipos = tipos;
            model.FechaHora = fechahora;

            return Json(model);
        }



        /// <summary>
        /// Permite grabar la data
        /// </summary>
        /// <param name="data">Datos en arreglo</param>
        /// <param name="titulos">Titulos</param>
        /// <param name="subtitulos">Subtitulos</param>
        /// <param name="idEvento">Código de evento</param>
        /// <param name="idInforme">Código de informe</param>
        /// <param name="tipo">Tipo de informe</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(string[][] data, int[] titulos, int[] subtitulos, int idEvento, int idInforme,
            string tipo)
        {
            string erroritem = "";
            try
            {
                List<EveInformeItemDTO> entitys = ObtenerDatos(data, titulos.ToList(), subtitulos.ToList(),
                    idInforme, ref erroritem);

                if (erroritem == "")
                {
                    //actualizando la cabecera
                    EveInformeDTO informe = this.informeServicio.GetByIdEveInforme(idInforme);
                    informe.Lastuserrev = User.Identity.Name;
                    informe.Lastdaterev = DateTime.Now;

                    informeServicio.SaveEveInforme(informe);

                    //borrando el detalle
                    informeServicio.DeleteEveInformeItemTotal(idInforme);

                    //actualizando el detalle
                    foreach (EveInformeItemDTO item in entitys)
                    {
                        informeServicio.SaveEveInformeItem(item);
                    }


                    return Json(1);
                }
                else
                {
                    return Json(erroritem);
                }

            }
            catch
            {
                return Json(-1);
            }
        }



        /// <summary>
        /// Permite obtener los datos para ser grabados
        /// </summary>
        /// <param name="estructura">Datos en un arreglo</param>
        /// <param name="titulos">Titulos</param>
        /// <param name="subTitulos">Subtitulos</param>
        /// <param name="idInforme">Código de informe</param>
        /// <returns></returns>
        private List<EveInformeItemDTO> ObtenerDatos(string[][] estructura, List<int> titulos, List<int> subTitulos, int idInforme, ref String error)
        {

            string erroritem = "";
            List<EqEquipoDTO> listaEquipo = equipoServicio.ListadoEquipoNombre(ConstantesAppServicio.ParametroDefecto);
            List<EveInformeItemDTO> entitys = new List<EveInformeItemDTO>();

            #region Recopilacion de datos

            //seccion 4. DESCRIPCIÓN DEL EVENTO
            for (int i = titulos[3] + 1; i < titulos[4]; i++)
            {
                EveInformeItemDTO entity = new EveInformeItemDTO();
                entity.Eveninfcodi = idInforme;
                entity.Itemnumber = 4;
                entity.Descomentario = ValorCadena(estructura[i][0], 400, ref erroritem);
                if (erroritem != "") error += "Descripción del evento: " + erroritem + "\r\n";

                entitys.Add(entity);
            }

            //seccion 5. CONDICIONES OPERATIVAS PREVIAS
            //A. GENERACIÓN
            //
            int nroitem = 0;
            for (int i = subTitulos[1] + 1; i < subTitulos[2]; i++)
            {
                EveInformeItemDTO entity = new EveInformeItemDTO();

                entity.Eveninfcodi = idInforme;
                entity.Itemnumber = 5;
                entity.Subitemnumber = 1;

                entity.Nroitem = ++nroitem;
                entity.Equicodi = ObtenerCodigoEquipo(listaEquipo, estructura[i][1], -1);
                if (entity.Equicodi == -1) error += "Item 5.A. Debe elegir equipo.\r\n";

                entity.Potactiva = ValorDecimal(estructura[i][4].ToString(), 6, ref erroritem);
                if (erroritem != "") error += "Item 5.A. Potencia Activa: " + erroritem + "\r\n";

                entity.Potreactiva = ValorDecimal(estructura[i][6], 6, ref erroritem);
                if (erroritem != "") error += "Item 5.A. Potencia Reactiva: " + erroritem + "\r\n";

                entity.Desobservacion = ValorCadena(estructura[i][9], 400, ref erroritem);
                if (erroritem != "") error += "Item 5.A. Observación: " + erroritem + "\r\n";

                entitys.Add(entity);
            }


            //B. FLUJO DE POTENCIA EN LINEAS
            //
            nroitem = 0;
            for (int i = subTitulos[4] + 1; i < subTitulos[5]; i++)
            {
                EveInformeItemDTO entity = new EveInformeItemDTO();

                entity.Eveninfcodi = idInforme;
                entity.Itemnumber = 5;
                entity.Subitemnumber = 2;

                entity.Nroitem = ++nroitem;
                entity.Equicodi = ObtenerCodigoEquipo(listaEquipo, estructura[i][1], -1);
                if (entity.Equicodi == -1) error += "Item 5.B. Debe elegir equipo.\r\n";

                entity.Subestacionde = ValorCadena(estructura[i][3], 40, ref erroritem);
                if (erroritem != "") error += "Item 5.B. Subestación de: " + erroritem + "\r\n";

                entity.Subestacionhasta = ValorCadena(estructura[i][5], 40, ref erroritem);
                if (erroritem != "") error += "Item 5.B. Subestación hasta: " + erroritem + "\r\n";

                entity.Potactiva = ValorDecimal(estructura[i][7], 6, ref erroritem);
                if (erroritem != "") error += "Item 5.B. Potencia Activa: " + erroritem + "\r\n";

                entity.Potreactiva = ValorDecimal(estructura[i][8], 6, ref erroritem);
                if (erroritem != "") error += "Item 5.B. Potencia Rectiva: " + erroritem + "\r\n";

                entity.Desobservacion = ValorCadena(estructura[i][9], 400, ref erroritem);
                if (erroritem != "") error += "Item 5.B. Observación: " + erroritem + "\r\n";

                entitys.Add(entity);
            }


            //C. FLUJO DE POTENCIA EN LOS TRANSFORMADORES
            //
            nroitem = 0;
            for (int i = subTitulos[6] + 1; i < titulos[5]; i++)
            {
                EveInformeItemDTO entity = new EveInformeItemDTO();

                entity.Eveninfcodi = idInforme;
                entity.Itemnumber = 5;
                entity.Subitemnumber = 3;

                entity.Nroitem = ++nroitem;
                entity.Equicodi = ObtenerCodigoEquipo(listaEquipo, estructura[i][1], -1);
                if (entity.Equicodi == -1) error += "Item 5.C. Debe elegir equipo.\r\n";

                entity.Potactiva = ValorDecimal(estructura[i][4], 6, ref erroritem);
                if (erroritem != "") error += "Item 5.C. Potencia Activa: " + erroritem + "\r\n";

                entity.Potreactiva = ValorDecimal(estructura[i][6], 6, ref erroritem);
                if (erroritem != "") error += "Item 5.C. Potencia Rectiva: " + erroritem + "\r\n";

                entity.Niveltension = ValorDecimal(estructura[i][9], 6, ref erroritem);
                if (erroritem != "") error += "Item 5.C. Nivel de tensión: " + erroritem + "\r\n";

                entity.Desobservacion = ValorCadena(estructura[i][10], 400, ref erroritem);
                if (erroritem != "") error += "Item 5.C. Observación: " + erroritem + "\r\n";

                entitys.Add(entity);
            }

            //6. SECUENCIA CRONOLÓGICA
            //
            for (int i = subTitulos[7] + 1; i < titulos[6]; i++)
            {
                EveInformeItemDTO entity = new EveInformeItemDTO();

                entity.Eveninfcodi = idInforme;
                entity.Itemnumber = 6;

                entity.Itemhora = ValorFechaCadena(estructura[i][0], ref erroritem);
                if (erroritem != "") error += "Item 6. Hora: " + erroritem + "\r\n";

                entity.Desobservacion = ValorCadena(estructura[i][1], 400, ref erroritem);
                if (erroritem != "") error += "Item 6. Observación: " + erroritem + "\r\n";

                entitys.Add(entity);
            }


            //7. ACTUACIÓN DE LAS PROTECCIONES
            //
            for (int i = subTitulos[8] + 1; i < titulos[7]; i++)
            {
                EveInformeItemDTO entity = new EveInformeItemDTO();

                entity.Eveninfcodi = idInforme;
                entity.Itemnumber = 7;

                entity.Equicodi = ObtenerCodigoEquipo(listaEquipo, estructura[i][0], -1);
                if (entity.Equicodi == -1) error += "Item 7. Debe elegir equipo.\r\n";

                entity.Senializacion = ValorCadena(estructura[i][2], 30, ref erroritem);
                if (erroritem != "") error += "Item 7. Señalización: " + erroritem + "\r\n";

                entity.Desobservacion = ValorCadena(estructura[i][4], 30, ref erroritem);
                if (erroritem != "") error += "Item 7. INT: " + erroritem + "\r\n";

                entity.Ac = ValorCadena(estructura[i][6], 1, ref erroritem);
                if (erroritem != "") error += "Item 7. A/C: " + erroritem + "\r\n";

                entitys.Add(entity);
            }


            //8. CONTADOR DE INTERRUPTORES Y PARARRAYOS
            //
            for (int i = subTitulos[10] + 1; i < titulos[8]; i++)
            {
                EveInformeItemDTO entity = new EveInformeItemDTO();

                entity.Eveninfcodi = idInforme;
                entity.Itemnumber = 8;

                entity.Equicodi = ObtenerCodigoEquipo(listaEquipo, estructura[i][0], -1);
                if (entity.Equicodi == -1) error += "Item 8. Debe elegir equipo.\r\n";

                entity.Interrcodi = ObtenerCodigoEquipo(listaEquipo, estructura[i][2], -1);
                if (entity.Interrcodi == -1) error += "Item 8. Debe elegir interruptor.\r\n";

                entity.Ra = ValorEntero(estructura[i][4], ref erroritem);
                if (erroritem != "") error += "Item 8. Ra: " + erroritem + "\r\n";

                entity.Sa = ValorEntero(estructura[i][5], ref erroritem);
                if (erroritem != "") error += "Item 8. Sa: " + erroritem + "\r\n";

                entity.Ta = ValorEntero(estructura[i][6], ref erroritem);
                if (erroritem != "") error += "Item 8. Ta: " + erroritem + "\r\n";

                entity.Rd = ValorEntero(estructura[i][7], ref erroritem);
                if (erroritem != "") error += "Item 8. Rd: " + erroritem + "\r\n";

                entity.Sd = ValorEntero(estructura[i][8], ref erroritem);
                if (erroritem != "") error += "Item 8. Sd: " + erroritem + "\r\n";

                entity.Td = ValorEntero(estructura[i][9], ref erroritem);
                if (erroritem != "") error += "Item 8. Td: " + erroritem + "\r\n";

                entitys.Add(entity);
            }

            //9. ANÁLISIS DEL EVENTO
            //
            for (int i = titulos[8] + 1; i < titulos[9]; i++)
            {
                EveInformeItemDTO entity = new EveInformeItemDTO();

                entity.Eveninfcodi = idInforme;
                entity.Itemnumber = 9;

                entity.Descomentario = ValorCadena(estructura[i][0], 4000, ref erroritem);
                if (erroritem != "") error += "Item 9. Análisis: " + erroritem + "\r\n";

                entitys.Add(entity);
            }

            //10. SUMINISTROS INTERRUMPIDOS
            //
            nroitem = 0;
            for (int i = subTitulos[12] + 1; i < titulos[10]; i++)
            {
                EveInformeItemDTO entity = new EveInformeItemDTO();

                entity.Eveninfcodi = idInforme;
                entity.Itemnumber = 10;

                entity.Nroitem = ++nroitem;
                entity.Sumininistro = ValorCadena(estructura[i][1], 40, ref erroritem);
                if (erroritem != "") error += "Item 10. Suministro: " + erroritem + "\r\n";

                entity.Potenciamw = ValorDecimal(estructura[i][2], 6, ref erroritem);
                if (erroritem != "") error += "Item 10. Potencia MW: " + erroritem + "\r\n";

                entity.Intinicio = ValorFecha(estructura[i][4], Constantes.FormatoFechaFull, estructura[1][1], ref erroritem);
                if (erroritem != "") error += "Item 10. Fecha inicio: " + erroritem + "\r\n";

                entity.Intfin = ValorFecha(estructura[i][5], Constantes.FormatoFechaFull, estructura[1][1], ref erroritem);
                if (erroritem != "") error += "Item 10. Fecha fin: " + erroritem + "\r\n";

                entity.Duracion = ValorDecimal(estructura[i][6], 6, ref erroritem);
                //if (erroritem != "") error += "Item 10. Duración: no válida (revise fechas de inicio-fin)\r\n";

                entity.Proteccion = ValorCadena(estructura[i][7], 50, ref erroritem);
                if (erroritem != "") error += "Item 10. Protección: " + erroritem + "\r\n";

                entitys.Add(entity);
            }

            //11. CONCLUSIONES
            //
            nroitem = 0;
            for (int i = subTitulos[13] + 1; i < titulos[11]; i++)
            {
                EveInformeItemDTO entity = new EveInformeItemDTO();

                entity.Eveninfcodi = idInforme;
                entity.Itemnumber = 11;

                entity.Nroitem = ++nroitem;
                entity.Desobservacion = ValorCadena(estructura[i][1], 400, ref erroritem);
                if (erroritem != "") error += "Item 11. Conclusiones: " + erroritem + "\r\n";

                entitys.Add(entity);
            }

            //12. ACCIONES EJECUTADAS
            //
            nroitem = 0;
            for (int i = subTitulos[14] + 1; i < titulos[12]; i++)
            {
                EveInformeItemDTO entity = new EveInformeItemDTO();

                entity.Eveninfcodi = idInforme;
                entity.Itemnumber = 12;

                entity.Nroitem = ++nroitem;
                entity.Desobservacion = ValorCadena(estructura[i][1], 400, ref erroritem);
                if (erroritem != "") error += "Item 12. Acciones ejecutadas: " + erroritem + "\r\n";

                entitys.Add(entity);
            }

            //13. OBSERVACIONES Y RECOMENDACIONES
            //
            nroitem = 0;
            int indice = subTitulos[15] + 1;
            while (estructura[indice][1] != "")
            {
                EveInformeItemDTO entity = new EveInformeItemDTO();

                entity.Eveninfcodi = idInforme;
                entity.Itemnumber = 13;

                entity.Nroitem = ++nroitem;
                entity.Desobservacion = ValorCadena(estructura[indice][1], 400, ref erroritem);
                if (erroritem != "") error += "Item 13. Observaciones y recomendaciones: " + erroritem + "\r\n";

                entitys.Add(entity);
                indice++;
            }



            #endregion



            return entitys;
        }



        /// <summary>
        /// Permite obtener el código por defecto
        /// </summary>
        /// <param name="listaEquipo">Lista de equipo</param>
        /// <param name="valor">Valor</param>
        /// <param name="codigoDefecto">Código por defecto</param>
        /// <returns></returns>
        private int ObtenerCodigoEquipo(List<EqEquipoDTO> listaEquipo, string valor, int codigoDefecto)
        {
            try
            {
                return (listaEquipo.Where(x => x.Equinomb == valor).ToList())[0].Equicodi;
            }
            catch
            {
                return codigoDefecto;
            }
        }



        /// <summary>
        /// Permite obtener el código de protección
        /// </summary>
        /// <param name="listaPtoInterr">Lista de Puntos de interrupción</param>
        /// <param name="valor">Valor</param>
        /// <param name="codigoDefecto">Código por defecto</param>
        /// <returns>Código por defecto</returns>
        private int ObtenerCodigoProteccion(List<EvePtointerrupDTO> listaPtoInterr, string valor, int codigoDefecto)
        {
            try
            {
                return (listaPtoInterr.Where(x => x.Ptointerrupnomb == valor).ToList())[0].Ptointerrcodi;
            }
            catch
            {
                return codigoDefecto;
            }
        }



        /// <summary>
        /// Permite obtener un decimal
        /// </summary>
        /// <param name="valor">Cadena</param>
        /// <param name="numeroEntero">Cantidad de enteros</param>
        /// <param name="error">Especifica el error, si hubiera</param>
        /// <returns></returns>
        public decimal ValorDecimal(string valor, int numeroEntero, ref string error)
        {
            error = "";
            try
            {
                decimal valorFinal = Convert.ToDecimal(valor);
                decimal valorMaximo = (decimal)Math.Pow(10, numeroEntero);

                if (Math.Abs(valorFinal) < valorMaximo)
                    return valorFinal;
                else
                    return Math.Sign(valorFinal) * (valorMaximo - 1);
            }
            catch
            {
                error = "Debe ingresar número válido.";
                return 0;
            }

        }



        /// <summary>
        /// Permite obtener un valor entero
        /// </summary>
        /// <param name="valor">Cadena</param>
        /// <param name="error">Especifica el error, si hubiera</param>
        /// <returns>Valor entero. 0: si hay error de conversión</returns>
        public int ValorEntero(string valor, ref string error)
        {
            error = "";
            try
            {
                return Convert.ToInt32(valor);
            }
            catch
            {
                error = "Debe ingresar número válido.";
                return 0;
            }

        }



        /// <summary>
        /// Permite obtener una fecha de acuerdo a una máscara
        /// </summary>
        /// <param name="valor">Texto</param>
        /// <param name="mascara">Máscara de fecha</param>
        /// <param name="fechaDefecto">Fecha por defecto</param>
        /// <param name="error">Especifica el error, si hubiera</param>
        /// <returns></returns>
        public DateTime ValorFecha(string valor, string mascara, string fechaDefecto, ref string error)
        {
            error = "";
            try
            {
                return (DateTime)DateTime.ParseExact(valor, mascara, CultureInfo.InvariantCulture);
            }
            catch
            {
                error = "Debe ingresar fecha válida.";
                return (DateTime)DateTime.ParseExact(fechaDefecto + " 00:00:00", mascara, CultureInfo.InvariantCulture); ;
            }

        }



        /// <summary>
        /// Permite obtener un valor de una longitud dada
        /// </summary>
        /// <param name="valor">Texto</param>
        /// <param name="longitud">Longitud de valor</param>
        /// <param name="error">Especifica el error, si hubiera</param>
        /// <returns>Valor obtenido</returns>
        public string ValorCadena(string valor, int longitud, ref string error)
        {
            error = "";
            try
            {
                string valorFinal = valor.Trim();

                if (valorFinal.Length > longitud)
                {
                    valorFinal = valorFinal.Substring(0, longitud);
                }

                return valorFinal;
            }
            catch
            {
                error = "Debe ingresar texto.";
                return "";

            }
        }

        /// <summary>
        /// Permite obtener un valor de fecha y la expresa como cadena
        /// </summary>
        /// <param name="valor">Texto</param>
        /// <param name="longitud">Longitud de valor</param>
        /// <param name="error">Especifica el error, si hubiera</param>
        /// <returns>Valor obtenido</returns>
        public string ValorFechaCadena(string valor, ref string error)
        {
            error = "";
            try
            {
                string valorFinal = valor.Trim();
                DateTime fecha = DateTime.ParseExact(valor, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);

                valorFinal = fecha.ToString(Constantes.FormatoFechaFull);

                return valorFinal;
            }
            catch
            {
                error = "Debe ingresar fecha.";
                return "";

            }
        }

    }


}
