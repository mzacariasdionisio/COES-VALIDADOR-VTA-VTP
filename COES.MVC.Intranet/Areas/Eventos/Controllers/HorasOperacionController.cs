using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Evento.Helper;
using COES.MVC.Intranet.Areas.Eventos.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Eventos;

namespace COES.MVC.Intranet.Areas.Eventos.Controllers
{
    public class HorasOperacionController : Controller
    {
        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        HoraOperacionAppServicio logic = new HoraOperacionAppServicio();

        /// <summary>
        /// Lista que contiene los datos programados
        /// </summary>
        public List<ReservaModel> ListaServicio
        {
            get
            {
                return (Session[DatosSesion.ListaServicio] != null) ?
                    (List<ReservaModel>)Session[DatosSesion.ListaServicio] : new List<ReservaModel>();
            }
            set { Session[DatosSesion.ListaServicio] = value; }
        }

        /// <summary>
        /// Muestra la pantalla inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            this.ListaServicio = null;
            MatrizModel model = new MatrizModel();
            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Permite agregar un elemento
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <param name="central"></param>
        /// <param name="nombre"></param>
        /// <param name="empresa"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Agregar(int idEquipo, string central, string nombre, string empresa, int idEmpresa)
        {
            MatrizModel model = new MatrizModel();
            List<ReservaModel> list = this.ListaServicio;

            if (list.Where(x => x.IdEquipo == idEquipo).ToList().Count == 0)
            {
                ReservaModel entity = new ReservaModel
                {
                    Central = central,
                    Empresa = empresa,
                    Equipo = nombre,
                    IdEmpresa = idEmpresa,
                    IdEquipo = idEquipo,
                    DesURS = this.ObtenerNombreURS(idEquipo)
                };

                if (list.Count > 0)
                {
                    List<ReservaItemModel> items = list[0].ListItems;
                    List<ReservaItemModel> subItems = new List<ReservaItemModel>();
                    foreach (ReservaItemModel item in items)
                    {
                        ReservaItemModel subItem = new ReservaItemModel();
                        subItem.Automatico = 0;
                        subItem.IndAutomatico = Constantes.NO;
                        subItem.IndManual = Constantes.NO;
                        subItem.Manual = 0;
                        subItem.HoraInicio = item.HoraInicio;
                        subItem.HoraFin = item.HoraFin;
                        subItems.Add(subItem);
                    }

                    entity.ListItems = subItems;
                }
                else
                {
                    entity.ListItems = new List<ReservaItemModel>();
                }

                list.Add(entity);
            }

            this.ListaServicio = list;
            model.ListaElementos = this.OrdenarLista(list);

            return PartialView(VistasParciales.Matriz, model);
        }

        /// <summary>
        /// Permite cargar las horas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult CargarHora()
        {
            HoraModel model = new HoraModel();

            List<ReservaModel> list = this.ListaServicio;

            if (list.Count > 0)
            {
                if (list[0].ListItems.Count > 0)
                {
                    model.TxtInicio = list[0].ListItems[list[0].ListItems.Count - 1].HoraFin.ToString(Constantes.FormatoHora);
                    model.TxtFin = list[0].ListItems[list[0].ListItems.Count - 1].HoraFin.ToString(Constantes.FormatoHora);
                }
                else
                {
                    model.TxtInicio = Constantes.HoraInicio;
                    model.TxtFin = Constantes.HoraInicio;
                }
            }
            else
            {
                model.TxtInicio = Constantes.HoraInicio;
                model.TxtFin = Constantes.HoraInicio;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite agregar una hora
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="horaInicio"></param>
        /// <param name="horaFin"></param>
        /// <param name="indicador"></param>
        /// <param name="indice"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult AgregarHora(string fecha, string horaInicio, string horaFin, string indicador, int indice)
        {
            DateTime fechaInicio = DateTime.ParseExact(fecha + Constantes.EspacioBlanco + horaInicio,
                Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(fecha + Constantes.EspacioBlanco + horaFin,
                Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);

            MatrizModel model = new MatrizModel();
            List<ReservaModel> list = this.ListaServicio;

            foreach (ReservaModel item in list)
            {
                if (indicador != Constantes.SI)
                {
                    List<ReservaItemModel> entitys = item.ListItems;
                    ReservaItemModel entity = new ReservaItemModel
                        {
                            HoraInicio = fechaInicio,
                            HoraFin = fechaFin,
                            IndAutomatico = Constantes.NO,
                            IndManual = Constantes.NO
                        };

                    entitys.Add(entity);
                    item.ListItems = entitys;
                }
                else
                {
                    if (item.ListItems.Count > indice)
                    {
                        item.ListItems[indice].HoraInicio = fechaInicio;
                        item.ListItems[indice].HoraFin = fechaFin;
                    }
                }
            }

            this.ListaServicio = list;
            model.ListaElementos = this.OrdenarLista(list);
            return PartialView(VistasParciales.Matriz, model);
        }

        /// <summary>
        /// Permite actualizar el valor
        /// </summary>
        /// <param name="indicador"></param>
        /// <param name="idEquipo"></param>
        /// <param name="columna"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarValor(int indicador, int idEquipo, int columna, decimal valor)
        {
            try
            {
                ReservaModel entity = this.ListaServicio.Where(x => x.IdEquipo == idEquipo).FirstOrDefault();

                if (indicador == 1) entity.ListItems[columna].Manual = valor;
                else entity.ListItems[columna].Automatico = valor;

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite actualizar el indicador
        /// </summary>
        /// <param name="indicador"></param>
        /// <param name="idEquipo"></param>
        /// <param name="columna"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarIndicador(int indicador, int idEquipo, int columna, string check)
        {
            try
            {
                ReservaModel entity = this.ListaServicio.Where(x => x.IdEquipo == idEquipo).FirstOrDefault();

                if (indicador == 1) entity.ListItems[columna].IndManual = check;
                else entity.ListItems[columna].IndAutomatico = check;

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite grabar los datos
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(string fecha)
        {
            try
            {
                DateTime fechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                List<IeodCuadroDTO> entitys = new List<IeodCuadroDTO>();

                List<ReservaModel> list = this.ListaServicio;
                foreach (ReservaModel item in list)
                {
                    foreach (ReservaItemModel child in item.ListItems)
                    {
                        IeodCuadroDTO itemAutomatico = new IeodCuadroDTO();
                        itemAutomatico.EQUICODI = item.IdEquipo;
                        itemAutomatico.SUBCAUSACODI = 319;
                        itemAutomatico.ICCHECK1 = child.IndAutomatico;
                        itemAutomatico.ICCHECK2 = Constantes.NO;
                        itemAutomatico.ICHORINI = child.HoraInicio;
                        itemAutomatico.ICHORFIN = child.HoraFin;
                        itemAutomatico.EVENCLASECODI = 1;
                        itemAutomatico.LASTDATE = DateTime.Now;
                        itemAutomatico.LASTUSER = User.Identity.Name;
                        itemAutomatico.ICVALOR1 = child.Automatico;

                        entitys.Add(itemAutomatico);

                        IeodCuadroDTO itemManual = new IeodCuadroDTO();
                        itemManual.EQUICODI = item.IdEquipo;
                        itemManual.SUBCAUSACODI = 318;
                        itemManual.ICCHECK1 = child.IndManual;
                        itemManual.ICCHECK2 = Constantes.NO;
                        itemManual.ICHORINI = child.HoraInicio;
                        itemManual.ICHORFIN = child.HoraFin;
                        itemManual.EVENCLASECODI = 1;
                        itemManual.LASTDATE = DateTime.Now;
                        itemManual.LASTUSER = User.Identity.Name;
                        itemManual.ICVALOR1 = child.Manual;

                        entitys.Add(itemManual);
                    }
                }

                this.logic.GrabarDatos(entitys, fechaProceso);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite consultar los datos
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Consultar(string fecha)
        {
            MatrizModel model = new MatrizModel();
            
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha,
                CultureInfo.InvariantCulture);
            List<IeodCuadroDTO> entitys = this.logic.Consultar(fechaConsulta);
            this.ListaServicio = null;
            List<ReservaModel> resultado = new List<ReservaModel>();

            if (entitys.Count > 0)
            {
                List<int> ids = (from cuadro in entitys select cuadro.EQUICODI).Distinct().ToList();

                foreach (int id in ids)
                {
                    List<IeodCuadroDTO> listManual = (from cuadro in entitys
                                                      orderby cuadro.ICCODI ascending
                                                      where cuadro.EQUICODI == id && cuadro.SUBCAUSACODI == 318
                                                      select cuadro).ToList();

                    List<IeodCuadroDTO> listAutomatico = (from cuadro in entitys
                                                          orderby cuadro.ICCODI ascending
                                                          where cuadro.EQUICODI == id && cuadro.SUBCAUSACODI == 319
                                                          select cuadro).ToList();

                    if (listManual.Count == listAutomatico.Count)
                    {
                        ReservaModel rsf = new ReservaModel();
                        IeodCuadroDTO temporal = entitys.Where(x => x.EQUICODI == id).FirstOrDefault();

                        rsf.Equipo = temporal.EQUIABREV;
                        rsf.Empresa = temporal.EMPRENOMB;
                        rsf.Central = temporal.TAREAABREV + temporal.AREANOMB;
                        rsf.IdEmpresa = (int)temporal.EMPRCODI;
                        rsf.IdEquipo = temporal.EQUICODI;
                        rsf.Potencia = 0;
                        rsf.DesURS = this.ObtenerNombreURS(temporal.EQUICODI);

                        List<ReservaItemModel> itemsRsf = new List<ReservaItemModel>();

                        int k = 0;
                        foreach (IeodCuadroDTO item in listManual)
                        {
                            ReservaItemModel itemRsf = new ReservaItemModel();
                            itemRsf.HoraInicio = item.ICHORINI;
                            itemRsf.HoraFin = item.ICHORFIN;
                            itemRsf.Automatico = listAutomatico[k].ICVALOR1;
                            itemRsf.IndAutomatico = listAutomatico[k].ICCHECK1;
                            itemRsf.Manual = item.ICVALOR1;
                            itemRsf.IndManual = item.ICCHECK1;
                            itemsRsf.Add(itemRsf);

                            k++;
                        }

                        rsf.ListItems = itemsRsf;
                        resultado.Add(rsf);
                    }
                }
            }
            else
            {
                List<IeodCuadroDTO> lista = this.logic.GetConfiguracion();

                foreach (IeodCuadroDTO item in lista)
                {
                    ReservaModel rsf = new ReservaModel();

                    rsf.Equipo = item.EQUIABREV;
                    rsf.Empresa = item.EMPRENOMB;
                    rsf.Central = item.TAREAABREV + item.AREANOMB;
                    rsf.IdEmpresa = (int)item.EMPRCODI;
                    rsf.IdEquipo = item.EQUICODI;
                    rsf.Potencia = 0;
                    rsf.DesURS = this.ObtenerNombreURS(item.EQUICODI);

                    resultado.Add(rsf);
                }
            }

            this.ListaServicio = resultado;
            model.ListaElementos = this.OrdenarLista(resultado);

            return PartialView(VistasParciales.Matriz, model);
        }

        /// <summary>
        /// Permite quitar un equipo
        /// </summary>
        /// <param name="codigos"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult QuitarEquipo(string codigos)
        {
            MatrizModel model = new MatrizModel();
            List<ReservaModel> list = this.ListaServicio;
            List<ReservaModel> resultado = new List<ReservaModel>();

            string[] ids = codigos.Split(Constantes.CaracterComa);

            foreach (ReservaModel item in list)
            {
                if (!ids.Contains(item.IdEquipo.ToString()))
                {
                    resultado.Add(item);
                }
            }

            this.ListaServicio = resultado;
            model.ListaElementos = this.OrdenarLista(resultado);
            return PartialView(VistasParciales.Matriz, model);
        }

        /// <summary>
        /// Permite quitar una hora
        /// </summary>
        /// <param name="codigos"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult QuitarHora(string codigos)
        {
            MatrizModel model = new MatrizModel();
            List<ReservaModel> list = this.ListaServicio;
            string[] ids = codigos.Split(Constantes.CaracterComa);

            foreach (ReservaModel item in list)
            {
                for (int i = 0; i < item.ListItems.Count; i++)
                {
                    for (int k = 0; k < ids.Length - 1; k++)
                    {
                        if (i == int.Parse(ids[k]))
                        {
                            item.ListItems[i].CheckDelete = Constantes.SI;
                        }
                    }
                }

                List<ReservaItemModel> entitys = new List<ReservaItemModel>();
                foreach (ReservaItemModel child in item.ListItems)
                {
                    if (child.CheckDelete != Constantes.SI)
                    {
                        entitys.Add(child);
                    }
                }

                item.ListItems = entitys;
            }

            this.ListaServicio = list;
            model.ListaElementos = this.OrdenarLista(list);

            return PartialView(VistasParciales.Matriz, model);
        }

        /// <summary>
        /// Permite editar una hora
        /// </summary>
        /// <param name="horaInicio"></param>
        /// <param name="horaFin"></param>
        /// <param name="indice"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EditarHora(string horaInicio, string horaFin, int indice)
        {
            HoraModel model = new HoraModel();

            model.TxtInicio = horaInicio;
            model.TxtFin = horaFin;
            model.IndicadorEdicion = Constantes.SI;
            model.Indice = indice;

            return PartialView(VistasParciales.CargarHora, model);
        }

        /// <summary>
        /// Permite obtener la descripción del URS
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <returns></returns>
        private string ObtenerNombreURS(int idEquipo)
        {
            /*
              11571  KALLPA
              12720  FLORES
              291    YAUPI
              276    MALPASO
              260    CAHUA
            */            

            if (idEquipo == 11571) return "URS-KLP-001";
            if (idEquipo == 12720) return "URS-KLP-002";
            if (idEquipo == 291) return "URS-STK-001";
            if (idEquipo == 276) return "URS-STK-002";
            if (idEquipo == 260) return "URS-STK-003";

            return "URS";
        }

        /// <summary>
        /// Permite ordenar la lista
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<ReservaModel> OrdenarLista(List<ReservaModel> list)
        {
            foreach (ReservaModel item in list)
            {
                string[] orders = item.DesURS.Split(Constantes.CaracterGuion);
                if(orders.Length == 3)
                {
                    item.Order = orders[2];
                }
            }

            return list.OrderBy(x => x.Order).ToList();
        }
        
        /// <summary>
        /// Permite generar archivo de exportacion
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Generar(string fecha)
        {
            int result = 1;
            try
            {
                DateTime fechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                ExcelDocument.GenerarArchivoRSF(this.ListaServicio, fechaProceso);
                result = 1;
            }
            catch
            {
                result = -1;
            }

            return Json(result);
        }

        /// <summary>
        /// Permite exportar el archivo excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Exportar()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento] + Constantes.NombreReporteRSF;
            return File(fullPath, Constantes.AppExcel, Constantes.NombreReporteRSF);
        }

        /// <summary>
        /// Permite exportar el archivo cada media hora
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarMediaHora(string fecha)
        {
            int result = 1;
            try
            {
                DateTime fechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                ExcelDocument.GenerarArchivoRSF30(this.ListaServicio, fechaProceso);
                result = 1;
            }
            catch
            {
                result = -1;
            }

            return Json(result);
        }

        /// <summary>
        /// Permite descargar el archivo cada media hora
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarMediaHora()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento] + Constantes.NombreReporteRSF30;
            return File(fullPath, Constantes.AppExcel, Constantes.NombreReporteRSF30);
        }        

        /// <summary>
        /// Permite generar archivo excel con reserva asignada
        /// </summary>
        /// <param name="inicio"></param>
        /// <param name="fin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarReservaAsignada(string inicio, string fin)
        {
            int result = 1;
            try
            {
                DateTime fechaInicio = DateTime.ParseExact(inicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(fin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                List<IeodCuadroDTO> total = new List<IeodCuadroDTO>();

                int dias = (int)fechaFin.Subtract(fechaInicio).TotalDays;

                for (int i = 0; i <= dias; i++)
                {
                    DateTime fecha = fechaInicio.AddDays(i);
                    List<IeodCuadroDTO> all = this.logic.ObtenerReporte(fecha, fecha);
                    List<IeodCuadroDTO> list = new List<IeodCuadroDTO>();

                    list = (from t in all
                            group t by new { t.RUS, t.HORA, t.TIPO }
                                into destino
                                select new IeodCuadroDTO()
                                {
                                    RUS = destino.Key.RUS,
                                    HORA = destino.Key.HORA,
                                    TIPO = destino.Key.TIPO,
                                    ICVALOR1 = destino.Sum(t => t.ICVALOR1),
                                    FECHA = fecha.ToString(Constantes.FormatoFecha)
                                }).ToList();

                    total.AddRange(list);
                }

                ExcelDocument.GenerarArchivoTotal(total, fechaInicio.ToString(Constantes.FormatoFecha), fechaFin.ToString(Constantes.FormatoFecha));

                result = 1;
            }
            catch
            {
                result = -1;
            }

            return Json(result);
        }

        /// <summary>
        /// Permite descargar el archivo de reserva asignada
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarReservaAsignada()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento] + Constantes.NombreReporteRSFGeneral;
            return File(fullPath, Constantes.AppExcel, Constantes.NombreReporteRSFGeneral);
        }
    }
}