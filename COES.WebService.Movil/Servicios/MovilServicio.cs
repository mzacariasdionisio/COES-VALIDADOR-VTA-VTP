using System;
using System.Collections.Generic;
using System.ServiceModel;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.General;
using COES.WebService.Movil.Contratos;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.General.Helper;
using System.Threading.Tasks;
using System.ServiceModel.Activation;
using System.Linq;
using COES.Servicios.Aplicacion.Equipamiento;
using System.Globalization;
using log4net;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using System.Configuration;
using COES.Servicios.Aplicacion.Helper;

namespace COES.WebService.Movil.Servicios
{
    /// <summary>
    /// Implementa los contratos de los servicios
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class MovilServicio : IMovilServicio
    {
        //- DEMANDA

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(MovilServicio));
   
        /// <summary>
        /// 
        /// </summary>
        public MovilServicio()
        {
            log4net.Config.XmlConfigurator.Configure();

        }

        /// <summary>
        /// Método que obtiene los datos de demanda
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public ChartDemanda ObtenerReporteDemanda(string fechaConsulta)
        {
            DateTime fecha = DateTime.Now;

            if (!string.IsNullOrEmpty(fechaConsulta))
            {
                fecha = DateTime.ParseExact(fechaConsulta, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }

            PortalAppServicio servicio = new PortalAppServicio();
            DateTime fechaEjecutado;
            decimal valorEjecutado;
            ChartDemanda demanda = servicio.ObtenerReporteDemandaMovil(fecha, fecha, out fechaEjecutado, out valorEjecutado);
            demanda.LastDate = fechaEjecutado.ToString("dd/MM/yyyy");
            demanda.LastTime = fechaEjecutado.ToString("HH:mm");
            demanda.LastValue = valorEjecutado;

            List<string> ejexDiagrama = new List<string>();
            DateTime fechaDatos = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            for (int i = 0; i < 48; i++)
            {
                //DateTime fechaLeyenda = fechaDatos.AddMinutes(30 * i);
                //ejexDiagrama.Add(fechaLeyenda.ToString("HH:mm"));

                ejexDiagrama.Add((i).ToString() + ":00");
            }
            demanda.EjexDemanda = ejexDiagrama;

            return demanda;
        }

        //- GENERACION

        /// <summary>
        /// Obtiene los datos de generacion
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public GraficoGeneracion ObtenerReporteGeneracion(string fechaConsulta)
        {
            GraficoGeneracion resultado = new GraficoGeneracion();
            DateTime fecha = DateTime.Now.AddDays(-1);

            if (!string.IsNullOrEmpty(fechaConsulta))
            {
                fecha = DateTime.ParseExact(fechaConsulta, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }

            List<MeMedicion48DTO> list = (new PortalAppServicio()).ObtenerReporteGeneracion(fecha);

            List<MeMedicion48DTO> empresas = (from row in list
                                              group row by new { row.Emprnomb, row.Emprcodi } into g
                                              select new MeMedicion48DTO()
                                              {
                                                  Emprnomb = g.Key.Emprnomb,
                                                  Emprcodi = g.Key.Emprcodi,
                                                  Meditotal = g.Sum(x => x.Meditotal)
                                              }).ToList();

            List<MeMedicion48DTO> tipogeneracion = (from row in list
                                                    group row by new { row.Fenergnomb, row.Fenergcodi, row.Fenercolor } into g
                                                    select new MeMedicion48DTO()
                                                    {
                                                        Fenergnomb = g.Key.Fenergnomb,
                                                        Fenergcodi = g.Key.Fenergcodi,
                                                        Fenercolor = g.Key.Fenercolor,
                                                        Meditotal = g.Sum(x => x.Meditotal)
                                                    }).ToList();

            List<MeMedicion48DTO> tipogeneracionPie = (from row in list
                                                    group row by new { row.Fenergnomb, row.Fenergcodi, row.Fenercolor, row.Tipogenerrer } into g
                                                    select new MeMedicion48DTO()
                                                    {
                                                        Fenergnomb = g.Key.Fenergnomb,
                                                        Fenergcodi = g.Key.Fenergcodi,
                                                        Fenercolor = g.Key.Fenercolor,
                                                        Tipogenerrer = g.Key.Tipogenerrer,
                                                        Meditotal = g.Sum(x => x.Meditotal)
                                                    }).ToList();

            var listEmpresa = empresas.OrderByDescending(c => c.Meditotal).Take(10).
                Select(x => new PuntoSerie { Nombre = x.Emprnomb, Valor = (decimal)x.Meditotal }).ToList();
            listEmpresa.Add(new PuntoSerie { Nombre = "OTROS", Valor = empresas.OrderByDescending(c => c.Meditotal).Skip(10).Sum(c => (decimal)c.Meditotal) });

            var listFuenteEnergia = tipogeneracion.OrderByDescending(x => x.Meditotal).Select(x => new PuntoSerie
            {
                Nombre = x.Fenergnomb,
                CodColor = x.Fenercolor,
                Valor = (decimal)x.Meditotal
            }).ToList();

            var listFuenteEnergiaPie = tipogeneracionPie.OrderByDescending(x => x.Meditotal).Select(x => new PuntoSerie
            {
                Nombre = x.Fenergnomb,
                CodColor = x.Fenercolor,
                RER = x.Tipogenerrer,
                Valor = (decimal)x.Meditotal
            }).ToList();

            resultado.GeneracionPorEmpresa = listEmpresa.OrderBy(x => x.Valor).ToList();//valorPorEmpresa;
            resultado.GeneracionPorTipoCombustible = listFuenteEnergia;
            resultado.GeneracionPorTipoCombustiblePie = listFuenteEnergiaPie;
            resultado.LastValue = list.Sum(x => (decimal)x.Meditotal);
            resultado.LastDate = fecha.ToString("dd/MM/yyyy");

            return resultado;
        }

        /// <summary>
        /// Permite obtener la estructura de costos marginales
        /// </summary>
        /// <returns></returns>
        public GraficoGeneracion ObtenerReporteGeneracionTipoGeneracion()
        {
            return ObtenerReporteGeneracionTipoGeneracionbyFecha("");
        }

            /// <summary>
            /// Reporte por tipo de generación
            /// </summary>
            /// <param name="fechaConsulta"></param>
            /// <returns></returns>
        public GraficoGeneracion ObtenerReporteGeneracionTipoGeneracionbyFecha(string fechaConsulta)
        {
            GraficoGeneracion resultado = new GraficoGeneracion();
            DateTime fecha = DateTime.Now.AddDays(-1);

            if (!string.IsNullOrEmpty(fechaConsulta))
            {
                fecha = DateTime.ParseExact(fechaConsulta, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            
            List<MeMedicion48DTO> list = (new PortalAppServicio()).ObtenerReporteGeneracion(fecha);

            List<MeMedicion48DTO> tipoGeneracion = (from row in list
                                                    group row by new { row.Tgenercodi, row.Tgenernomb, row.Tgenercolor } into g
                                                    select new MeMedicion48DTO()
                                                    {
                                                        Tgenernomb = g.Key.Tgenernomb,
                                                        Tgenercodi = g.Key.Tgenercodi,
                                                        Tgenercolor = g.Key.Tgenercolor,
                                                        Meditotal = g.Sum(x => x.Meditotal)
                                                    }).ToList();

            List<PuntoSerie> listSerie = new List<PuntoSerie>();

            foreach (MeMedicion48DTO item in tipoGeneracion)
            {
                PuntoSerie serie = new PuntoSerie
                {
                    Nombre = item.Tgenernomb,
                    CodColor = item.Tgenercolor,
                    Valor = (decimal)item.Meditotal
                };

                /*if (item.Tgenernomb.Trim() == "HIDROELÉCTRICA")
                {
                    serie.CodColor = "#4572A7";
                }
                else if (item.Tgenernomb.Trim() == "SOLAR")
                {
                    serie.CodColor = "#FFD700";
                }
                else if (item.Tgenernomb.Trim() == "TERMOELÉCTRICA")
                {
                    serie.CodColor = "#FF0000";
                }
                else if (item.Tgenernomb.Trim() == "EÓLICA")
                {
                    serie.CodColor = "#69C9E0";
                }*/

                listSerie.Add(serie);
            }

            resultado.GeneracionPorTipoGeneracion = listSerie;
            //resultado.PorcentajeCrecimiento = (new DespachoAppServicio()).ObtenerParametroPorConcepto(23.ToString());
            resultado.PorcentajeCrecimiento = (new PortalAppServicio()).ObtenerPorcentajeCrecimiento();
            resultado.LastDate = fecha.ToString("dd/MM/yyyy");
            return resultado;
        }

        //- EVENTOS RELEVANTES

        /// <summary>
        /// Calcula la lista de eventos relevantes del dia
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public List<EveEventoDTO> ObtenerEventosRelevantes(string fechaConsulta)
        {
            DateTime fecha = DateTime.Today;

            if (!string.IsNullOrEmpty(fechaConsulta))
            {
                fecha = DateTime.ParseExact(fechaConsulta, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }

            List<EveEventoDTO> entitys = (new PortalAppServicio()).ListarResumenEventosWeb(fecha).OrderByDescending(x => (DateTime)x.Evenini).ToList();

            foreach (EveEventoDTO entity in entitys)
            {
                entity.Evenasunto = ((DateTime)entity.Evenini).ToString("dd/MM/yyyy HH:mm") + ".  " + entity.Evenasunto;
            }

            return entitys;
        }

        //- FRECUENCIA DEL SEIN

        /// <summary>
        /// Permite obtener los datos de frecuencia 
        /// </summary>
        /// <returns></returns>
        public List<GraficoFrecuencia> ObtenerFrecuenciaSein()
        {
            int gps = Convert.ToInt32(ConfigurationManager.AppSettings["GpsFrecuencia"].ToString());
            List<GraficoFrecuencia> resultado = ((new PortalAppServicio())).ObtenerFrecuenciaSein(gps);
            return resultado;
        }

        //- MANTENIMIENTOS PROGRAMADOS

        /// <summary>
        /// Permite obtener los mantenimientos programados del día
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public List<EveManttoDTO> ObtenerMantenimientosProgramados(string fechaConsulta)
        {
            DateTime fecha = DateTime.Now;

            if (!string.IsNullOrEmpty(fechaConsulta))
            {
                fecha = DateTime.ParseExact(fechaConsulta, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }

            return (new EventoAppServicio()).ObtenerMantenimientosProgramados(fecha, fecha.AddDays(1));
        }

        /// <summary>
        /// Permite obtener los filtros para los mantenimientos
        /// </summary>
        /// <returns></returns>
        public List<EveManttoTipoDTO> ObtenerFiltroMantenimientos()
        {
            List<EveManttoTipoDTO> entitys = new List<EveManttoTipoDTO>();

            entitys.Add(new EveManttoTipoDTO { Mantipcodi = 0, Mantipdesc = "Todos", Mantipcolor = "" }); ;
            entitys.Add(new EveManttoTipoDTO { Mantipcodi = 1, Mantipdesc = "Generación", Mantipcolor = "#ff5733" });
            entitys.Add(new EveManttoTipoDTO { Mantipcodi = 2, Mantipdesc = "Líneas", Mantipcolor = "#3374ff" });
            entitys.Add(new EveManttoTipoDTO { Mantipcodi = 3, Mantipdesc = "SSEE", Mantipcolor = "#34ad92" });
            entitys.Add(new EveManttoTipoDTO { Mantipcodi = 4, Mantipdesc = "SSEE / Líneas", Mantipcolor = "#f4f747" });

            return entitys;
        }

        /// <summary>
        /// Permite obtener el listado de mantenimientos para el app movil
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public List<EveManttoDTO> ObtenerMantenimientos(string fechaConsulta, string tipo)
        {
            DateTime fecha = DateTime.Now;

            if (!string.IsNullOrEmpty(fechaConsulta))
            {
                fecha = DateTime.ParseExact(fechaConsulta, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }

            return (new EventoAppServicio()).ObtenerMantenimientosProgramadosMovil(fecha, fecha.AddDays(1), int.Parse(tipo));
        }

        //- COSTOS MARGINALES

        /// <summary>
        /// Permite obtener la leyenda de Costos Marginales
        /// </summary>
        /// <returns></returns>
        public List<CmParametroDTO> ObtenerColoresCostosMarginal()
        {
            return (new CortoPlazoAppServicio()).ListCmParametros();
        }

        /// <summary>
        /// Permite obtener la estructura de costos marginales
        /// </summary>
        /// <param name="corrida"></param>
        /// <returns></returns>
        public List<CmCostomarginalDTO> ObtenerMapaCostosMarginal(string corrida)
        {
            int correlativo = 0;
            if (!string.IsNullOrEmpty(corrida))
            {
                if (int.TryParse(corrida, out correlativo)) { }
            }
            else
            {
                CmCostomarginalDTO entity = (new CortoPlazoAppServicio()).ObtenerResumenCostoMarginal();
                if (entity != null)
                {
                    correlativo = (int)entity.Cmgncorrelativo;
                }
            }

            List<CmCostomarginalDTO> result = new List<CmCostomarginalDTO>();
            result = (new CortoPlazoAppServicio()).ObtenerMapaCostoMarginal(correlativo);

            return result;
        }

        /// <summary>
        /// Permite obtener las corridas del costo marginal por dia
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public List<CmCostomarginalDTO> ObtenerCorridasCostoMarginal(string fechaConsulta)
        {
            DateTime fecha = DateTime.Now;

            if (!string.IsNullOrEmpty(fechaConsulta))
            {
                fecha = DateTime.ParseExact(fechaConsulta, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }

            List<CmCostomarginalDTO> result = (new CortoPlazoAppServicio()).ObtenerCorridasPorFecha(fecha, ConstantesCortoPlazo.VersionCMOriginal);

            if (result.Count == 0)
                result = (new CortoPlazoAppServicio()).ObtenerCorridasPorFecha(fecha, ConstantesCortoPlazo.VersionCMPR07);

            return result.OrderByDescending(x => x.Indicador).ToList();
        }

        //- CALENDARIO COES

        /// <summary>
        /// Permite obtener los datos del calendario COES
        /// </summary>
        /// <returns></returns>
        public List<WbCalendarioDTO> ObtenerCalendarioCOES()
        {
            List<WbCalendarioDTO> list = (new PortalAppServicio()).ListWbCalendarios();

            foreach (WbCalendarioDTO item in list)
            {
                item.Calendicon = "https://www.coes.org.pe/Portal/content/images/" + item.Calendicon;
            }

            return list;
        }

        public List<WbCalendarioDTO> ObtenerCalendarioMesCOES(string fechaConsulta)
        {
            List<WbCalendarioDTO> list = null;

            if (fechaConsulta != null && fechaConsulta != "")
            {
                if (fechaConsulta.Count() == 4)
                {
                    int anio = Convert.ToInt32(fechaConsulta);
                    list = (new PortalAppServicio()).ListWbCalendarios().Where(x => x.Calendfecinicio.Value.Year == anio).ToList();

                    foreach (WbCalendarioDTO item in list)
                    {
                        item.Calendicon = "https://www.coes.org.pe/Portal/content/images/" + item.Calendicon;
                    }
                }
                else
                {
                    DateTime fecha = DateTime.ParseExact(fechaConsulta, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                    int mes = fecha.Month;
                    int anio = fecha.Year;
                    list = (new PortalAppServicio()).ListWbCalendarios().Where(x => x.Calendfecinicio.Value.Month == mes && x.Calendfecinicio.Value.Year == anio).ToList();

                    foreach (WbCalendarioDTO item in list)
                    {
                        item.Calendicon = "https://www.coes.org.pe/Portal/content/images/" + item.Calendicon;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Permite obtener los tipos de eventos del calendario COES
        /// </summary>
        /// <returns></returns>
        public List<WbCaltipoventoDTO> ObtenerTipoCalendarioCOES()
        {
            List<WbCaltipoventoDTO> list = (new PortalAppServicio()).ListWbCaltipoventos();
            
            foreach (WbCaltipoventoDTO item in list)
            {
                item.Tipcalicono = "https://www.coes.org.pe/Portal/content/images/" + item.Tipcalicono;
            }

            return list;
        }

        //- MAXIMA DEMANDA

        /// <summary>
        /// Permite obtener los datos de máxima demanda
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public GraficoMaximaDemanda ObtenerReporteMaximaDemanda(string fechaConsulta)
        {            
            return (new PortalAppServicio()).ObtenerDiagramaCarga(fechaConsulta);
        }

        /// <summary>
        /// Permite obtener la estructura de costos marginales
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public GraficoMaximaDemanda ObtenerMaximaDemanda(string fechaConsulta)
        {
            WbResumenmdDTO resumen = (new PortalAppServicio()).ObtenerResumenMD(fechaConsulta);
            return (new PortalAppServicio()).ObtenerMaximaDemanda(resumen);
        }

        //- DATOS RESUMEN

        /// <summary>
        /// Permite obtener datos resumen
        /// </summary>
        /// <returns></returns>
        public DatosResumenMovil ObtenerDatosResumen()
        {
            try
            {
                return (new PortalAppServicio()).ObtenerDatosResumen();
            }
            catch (Exception ex)
            {

                log.Error("EnvioNotificacion", ex);
                return null;

            }
        }

        /// <summary>
        /// Permite enviar las notificaciones
        /// </summary>
        /// <returns></returns>
        public Response EnviarNotificacion()
        {
            return (new PortalAppServicio()).EnviarNotificacion();
        }

        /// <summary>
        /// Acerca del coes
        /// </summary>
        /// <returns></returns>
        public string AcercaDeCoes()
        {
            #region Texto
            return @"
El COES es una entidad privada, sin fines de lucro y con personería de Derecho Público. Está conformado por todos los Agentes del SEIN (Generadores, Transmisores, Distribuidores y Usuarios Libres) y sus decisiones son de cumplimiento obligatorio por los Agentes. Su finalidad es coordinar la operación de corto, mediano y largo plazo del SEIN al mínimo costo, preservando la seguridad del sistema, el mejor aprovec​​​​hamiento de los recursos energéticos, así como planificar el desarrollo de la transmisión del SEIN y administrar el Mercado de Corto Plazo
                    <br /><br />
El COES reúne los esfuerzos de las principales empresas de generación, transmisión y distribución de electricidad, así como de los grandes usuarios libres, contribuyendo a través de su labor al desarrollo y bienestar del país.
                    <br /><br />
Mediante el desarrollo de sus funciones, el COES vela por la seguridad del abastecimiento de energía eléctrica, permitiendo que la población goce del suministro de electricidad en condiciones de calidad y posibilitando las condiciones adecuadas para el desarrollo de la industria y otras actividades económicas. Asimismo, es responsable de administrar el mejor aprovechamiento de los recursos destinados a la generación de energía eléctrica.
                   ";
            #endregion
        }

        /// <summary>
        /// Permite obtener las notificaciones
        /// </summary>
        /// <returns></returns>
        public List<WbNotificacionDTO> ObtnerNotificaciones()
        {
            return (new PortalAppServicio()).ListNotificaciones(string.Empty, null, null).Where(c => c.NotiStatus == 1).ToList();
        }

        /// <summary>
        /// Permite obtener la version actual del app
        /// </summary>
        /// <returns></returns>
        public WbVersionappDTO ObtenerVersionAPP()
        {
            return (new PortalAppServicio()).ObtenerVersionAPP();
        }

        /// <summary>
        /// Permite listar los códigos IMEI
        /// </summary>
        /// <returns></returns>
        public List<WbRegistroimeiDTO> ObtenerListadoCodigoIMEI()
        {
            return (new MovilAppServicio()).ListWbRegistroimeis();
        }

        /// <summary>
        /// Permite obtener los datos de las ventas de ayuda
        /// </summary>
        /// <returns></returns>
        public List<WbAyudaappDTO> ObtenerAyudaVentanaAPP()
        {
            return (new MovilAppServicio()).ListWbAyudaapps();
        }

        /// <summary>
        /// Permite obtener los datos de la agenda COES
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<WbContactoDTO> ObtenerAgendaCOES(string idEmpresa)
        {
            int id = -1;
            if (!string.IsNullOrEmpty(idEmpresa))
            {
                if (int.TryParse(idEmpresa, out id)) { }
            }
            return (new ContactoAppServicio()).GetByCriteriaWbContactos(null, id, null, -1, -1, -1);
        }

        /// <summary>
        /// Permite obtener la estructura de costos marginales
        /// </summary>
        /// <returns></returns>
        public List<EmpresaContactoDTO> ObtenerEmpresas()
        {
            return (new ContactoAppServicio()).ObtenerEmpresasContacto();
        }

        /// <summary>
        /// Permite obtener la estructura de costos marginales
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="clave"></param>
        /// <returns></returns>
        public int ValidarCredenciales(string usuario, string clave)
        {
            return (new MovilAppServicio()).ValidarCredenciales(usuario, clave);
        }

        /// <summary>
        /// Valida credenciales apple
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="clave"></param>
        /// <returns></returns>
        public bool ValidarCredencialesApple(string usuario, string clave)
        {
            return ((new MovilAppServicio()).ValidarCredenciales(usuario, clave) == 1) ? true : false;
        }

        #region METODOS ADICIONALES 

        /// <summary>
        /// Permite obtener los tipos de notificaciones
        /// </summary>
        /// <returns></returns>
        public List<WbTipoNotificacionDTO> ObtenerTipoNotificacionEventos()
        {
            return (new MovilAppServicio()).ObtenerTipoNotificacionEventos();
        }

        /// <summary>
        /// Muestra los datos del compartivo de CM vs Tarifa en Barra
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public List<ChartCmVsTarifaBarra> ObtenerCmVsTarifaBarra(string fechaConsulta)
        {
            return (new PortalAppServicio()).ObtenerCmVsTarifaBarra(fechaConsulta);
        }

        #endregion
    }
}