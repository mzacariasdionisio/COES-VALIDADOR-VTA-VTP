using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Areas.Eventos.Helper;
using COES.MVC.Extranet.Areas.Eventos.Models;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.Evento;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.Informe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Eventos.Controllers
{
    public class InformeController : Controller
    {
        /// <summary>
        /// Instancia de la clase servicio de eventos
        /// </summary>
        EventosAppServicio servicio = new EventosAppServicio();

        /// <summary>
        /// Instancia de la clases servicio de los informes
        /// </summary>
        InformeAppServicio informeServicio = new InformeAppServicio();

        /// <summary>
        /// Lista de equipos
        /// </summary>
        public List<EqEquipoDTO> ListaEquipos
        {
            get { return (Session[DatosSesion.ListaEquipos] != null)?
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
        public ActionResult Index(int evento, int empresa)
        {
            InformeModel model = new InformeModel();
            model.Entidad = this.servicio.GetDetalleEvento(evento);

            List<int> idsEmpresas = this.ListaEmpresa;

            if (idsEmpresas.Contains(empresa) || empresa == -1)
            {
                this.informeServicio.VerificarExistenciaInforme(evento, empresa, model.Entidad, User.Identity.Name, false);
            }

            if (model.Entidad.Evenini != null)
            {
                DateTime fecha = (DateTime)model.Entidad.Evenini;               
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

            string path = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaEvento] + RutaDirectorio.FileLogoEvento;
            string fileGif = string.Format(NombreArchivo.LogoEmpresa, model.IdEmpresa, Constantes.ExtensionGif);
            string fileJpg = string.Format(NombreArchivo.LogoEmpresa, model.IdEmpresa, Constantes.ExtensionJpg);
            string filePng = string.Format(NombreArchivo.LogoEmpresa, model.IdEmpresa, Constantes.ExtensionPng);

            if (System.IO.File.Exists(path + fileGif)) {
                model.LogoEmpresa = fileGif;
            }
            else if (System.IO.File.Exists(path + fileJpg)) {
                model.LogoEmpresa = fileJpg;
            }
            else if (System.IO.File.Exists(path + filePng))
            {
                model.LogoEmpresa = filePng;
            }
            else {
                model.LogoEmpresa = Constantes.NO;
            }          

            model.IdInformePreliminar = (informes.Where(x=> x.Infversion == 
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

            if (this.ListaEmpresa.Contains(informe.Emprcodi))
            {
                if (informe.Infestado != null)
                {
                    if (informe.Infestado == ConstantesEvento.EstadoPediente ||
                        informe.Infestado == string.Empty)
                    {
                        model.IndicadorEdicion = Constantes.SI;
                    }
                }
                else
                {
                    model.IndicadorEdicion = Constantes.SI;
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
            model.IndicaShowPlazo = Constantes.SI;

            if (evento.Evenini != null)
            {
                DateTime fecha = (DateTime)evento.Evenini;
                model.FechaEvento = fecha.ToString(Constantes.FormatoFecha);
                model.HoraEvento = fecha.ToString(Constantes.FormatoOnlyHora);

                if (informe.Emprcodi == -1)
                {
                    model.IndicaShowPlazo = Constantes.NO;
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
            
                model.ListaGeneradores = this.ListaEquipos;
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
        /// Permite cargar el logo de la empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public ActionResult UploadLogo(int idEmpresa)
        {
            try
            {
                string path = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaEvento] + RutaDirectorio.FileLogoEvento;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    int lastIndex = file.FileName.LastIndexOf(Constantes.CaracterPunto);
                    string extension = file.FileName.Substring(lastIndex + 1, file.FileName.Length - lastIndex - 1);
                    string fileName = path + string.Format(NombreArchivo.LogoEmpresa, idEmpresa, extension);

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);
                }
                return Json(new { sucess = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { sucess = false }, JsonRequestBehavior.AllowGet);
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
                string pathLogo = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaEvento] + RutaDirectorio.FileLogoEvento;
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
                if (idEmpresa == -1) emprnomb = "SCO COES";
                if (idEmpresa == 0) emprnomb = "SEV COES";

                if (tipo == Constantes.FormatoWord)
                {
                    fileName = NombreArchivo.FormatoInformeWORD;
                    (new WordDocument()).GenerarReporteInforme(evento, list, idInforme, path, fileName, indicadorLogo, pathLogo, emprnomb);

                }
                if (tipo == Constantes.FormatoPDF)
                {
                    fileName = NombreArchivo.FormatoInformePDF;
                    (new PdfDocument()).GenerarReporteInforme(evento, list, idInforme, path, fileName, indicadorLogo, pathLogo, emprnomb);
                }

                return Json(1);
            }
            catch (Exception )
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
        /// Permite finalizar el reporte seleccionado
        /// </summary>
        /// <param name="idInforme"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Finalizar(int idEvento, string tipo, int idInforme)
        {
            try
            {
                int indicador = this.informeServicio.FinalizarInforme(idEvento, tipo, idInforme, User.Identity.Name, 0);
                return Json(indicador);
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
        public JsonResult ValidarFinalizar(int idEvento, string indicador)
        {
            int resultado = 1;
            decimal plazo = -1;

            if (indicador == ConstantesEvento.InformePreliminar) plazo = 2.5M;
            if (indicador == ConstantesEvento.InformeFinal) plazo = 60;

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
                List<string> errors;
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
    }
}
