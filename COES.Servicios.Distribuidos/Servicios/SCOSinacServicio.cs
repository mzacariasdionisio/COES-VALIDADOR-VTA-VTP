using COES.Base.Tools;
using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Hidrologia;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.TiempoReal;
using COES.Servicios.Distribuidos.Contratos;
using COES.Servicios.Distribuidos.Resultados;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;

namespace COES.Servicios.Distribuidos.Servicios
{
    /// <summary>
    /// Implementa los contratos de los servicios AddIn
    /// </summary>
    [Obsolete("El servicio está obsoleto, se debe utilizar el servicio distribuido COES.WebService.SCOSinac.")]
    [HttpMappingErrorBehavior]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]    
    public class SCOSinacServicio : ISCOSinacServicio
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(SCOSinacServicio));
        public SCOSinacServicio() {
            log4net.Config.XmlConfigurator.Configure();
        }
        
        public List<FLecturaSp7DTO> ListaScoSinacLectura(string zeroh, string gpscodi, string fechaConsulta)
        {

            //Lectura
            //string fechaConsulta = "28-04-2017";            
            DateTime inicio = DateTime.ParseExact(fechaConsulta, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            ScadaSp7AppServicio servicioLectura = new ScadaSp7AppServicio();
            List<FLecturaSp7DTO> result = servicioLectura.ObtenerConsultaTablaFrecuencia((Convert.ToInt32(zeroh )== 1?true:false), inicio, inicio.AddDays(1), Convert.ToInt32(gpscodi));            

            return result;
        }

        /// <summary>
        /// Método para obtener información RER de la tabla Me_Medicion48
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<Medicion48> ListaScoSinacDespachoRER(Medicion48Request model)
        {
            int fuente = Int32.Parse(model.Fuente);
            DateTime fecha = DateTime.ParseExact(model.Fecha, ConstantesAppServicio.FormatoFechaWS, CultureInfo.InvariantCulture);

            FormatoMedicionAppServicio servicio = new FormatoMedicionAppServicio();
            List<Medicion48> lista = servicio.GetListaObtenerMedicion48RERWS(fecha, fecha, fuente, model.Puntos);

            return lista;
        }

        /// <summary>
        /// Método para obtener inforamción de la tabla Me_Medicion48
        /// </summary>
        /// <param name="lectura">Lectura de Medicion</param>
        /// <param name="medida">Tipo de informacion</param>
        /// <param name="puntos">Lista de puntos de medición</param>
        /// <param name="fechaConsulta">Fecha con formato dd-MM-yyyy</param>
        /// <returns></returns>
        public List<Medicion48> ListaScoSinacDespacho(Medicion48Request model)
        {
            int lectcodi = Int32.Parse(model.Lectura);
            int tipoinfocodi = Int32.Parse(model.Medida);
            int fuente = Int32.Parse(model.Fuente);
            int topcodi = Int32.Parse(model.Escenario);
            DateTime fecha = DateTime.ParseExact(model.Fecha, ConstantesAppServicio.FormatoFechaWS, CultureInfo.InvariantCulture);

            MigracionesAppServicio servMigr = new MigracionesAppServicio();
            List<Medicion48> lista = servMigr.GetListaObtenerMedicion48WS(fecha, fecha, lectcodi.ToString(), tipoinfocodi, fuente, topcodi, model.Puntos);

            return lista;
        }

        /// <summary>
        /// Obtener la lista de escenarios reprograma para una fecha consultada
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public List<CpTopologiaDTO> ListaScoEscenarioReprogramaYupana(FechaRequest model) 
        {
            DateTime fecha = DateTime.ParseExact(model.Fecha, ConstantesAppServicio.FormatoFechaWS, CultureInfo.InvariantCulture);

            MigracionesAppServicio servMigr = new MigracionesAppServicio();
            return servMigr.ListarEscenarioReprograma(fecha);
        }

        public int Eliminarmedicion48()
        {
            int result = 0;

            try
            {
                DespachoAppServicio servicio = new DespachoAppServicio();
                servicio.Eliminarmedicion48();
            }
            catch (Exception ex)
            {
                log.Error("Eliminarmedicion48", ex);                
            }
            
            return result;
        }

        public int Insertarmedicion48(Insert dato)
        {
            DateTime fechadatime = DateTime.ParseExact(dato.fecha, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            DespachoAppServicio servicio = new DespachoAppServicio();
            return servicio.Insertarmedicion48(fechadatime, Convert.ToInt32(dato.medicodi), dato.valor);
        }

        /// <summary>
        /// Grabar la información de Reprogramado Hidrología 
        /// </summary>
        /// <param name="objJson"></param>
        /// <returns></returns>
        public int GrabarReprogramaHidrologia(HidrologiaResult request)
        {
            DateTime fechaReprograma = DateTime.ParseExact(request.Fecha, ConstantesAppServicio.FormatoFechaWS, CultureInfo.InvariantCulture);
            List<MeMedicion48DTO> lstMedicion48 = new List<MeMedicion48DTO>();

            foreach (var reg in request.Listam48hidro)
            {
                MeMedicion48DTO med48 = new MeMedicion48DTO
                {
                    Ptomedicodi = reg.Ptomedicodi,
                    Tipoinfocodi = reg.Tipoinfocodi
                };

                var index = 0;
                for (int hx = request.Horainicio; hx <= 48; hx++)
                {
                    med48.GetType().GetProperty(ConstantesAppServicio.CaracterH + hx).SetValue(med48, reg.ListaH[index]);
                    index++;
                }
                lstMedicion48.Add(med48);
            }

            HidrologiaAppServicio servicio = new HidrologiaAppServicio();
            return servicio.CargarReprogramaHidrologiaCP(fechaReprograma, request.Horainicio, lstMedicion48, "userAddin");
        }

        /// <summary>
        /// Grabar la información de Rer
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public int GrabarDespachoRER(RERResult request)
        {
            int fuente = Int32.Parse(request.Fuente);
            request.Horainicio = 1;
            DateTime fechaReprograma = DateTime.ParseExact(request.Fecha, ConstantesAppServicio.FormatoFechaWS, CultureInfo.InvariantCulture);

            List<MeMedicion48DTO> lstMedicion48 = new List<MeMedicion48DTO>();

            foreach (var reg in request.Listam48rer)
            {
                MeMedicion48DTO med48 = new MeMedicion48DTO
                {
                    Ptomedicodi = reg.Ptomedicodi,
                    Tipoinfocodi = reg.Tipoinfocodi,
                    Medifecha = fechaReprograma
                };

                var index = 0;
                for (int hx = request.Horainicio; hx <= 48; hx++)
                {
                    med48.GetType().GetProperty(ConstantesAppServicio.CaracterH + hx).SetValue(med48, reg.ListaH[index]);
                    index++;
                }
                lstMedicion48.Add(med48);
            }

            FormatoMedicionAppServicio servicio = new FormatoMedicionAppServicio();
            return servicio.CargarDespachoRERAddin(fechaReprograma, lstMedicion48, fuente, "userAddin");
        }

        #region AddIn

        /// <summary>
        /// Devuelve el listado de potencia para las hojas ACTIVA y REACTIVA
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaScoSinacGetPotenciaScada(PegarScadaRequest data)
        {
            DateTime fecha = DateTime.ParseExact(data.Fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            List<int> lstCabecera = data.LstIds != null ? data.LstIds.OrderBy(x => x).ToList() : new List<int>();
            string hoja = data.NombreHoja;

            MigracionesAppServicio servMigr = new MigracionesAppServicio();
            List<MeMedicion48DTO> salida = servMigr.ListarDatosPotenciaScada(fecha, lstCabecera, hoja);

            return salida;
        }

        /// <summary>
        /// Devuelve el listado de potencia para las hojas FLUJOS
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaScoSinacGetFlujosScada(PegarScadaRequest data)
        {
            DateTime fecha = DateTime.ParseExact(data.Fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            List<int> lstCabecera = data.LstIds != null ? data.LstIds.OrderBy(x => x).ToList() : new List<int>();

            MigracionesAppServicio servMigr = new MigracionesAppServicio();
            List<MeMedicion48DTO> salida = servMigr.ListarDatosFlujoScada(fecha, lstCabecera);

            return salida;
        }

        /// <summary>
        /// Devuelve el listado de potencia para las hojas ACTIVA desde TNA
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaScoSinacGetPotenciaTNA(PegarScadaRequest data)
        {
            DateTime fecha = DateTime.ParseExact(data.Fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            List<int> lstCabecera = data.LstIds != null ? data.LstIds.OrderBy(x => x).ToList() : new List<int>();
            string strPtomedicodis = string.Join(",", lstCabecera);

            MigracionesAppServicio servMigr = new MigracionesAppServicio();
            List<MeMedicion48DTO> salida = servMigr.ListarDataTNAxPtomedicion(fecha, strPtomedicodis);

            return salida;
        }

        /// <summary>
        /// Devuelve el listado de eventos para un rango de fechas
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<PrRepcvDTO> ListaScoSinacGetEventosCV(BuscarEventosRequest data)
        {
            DateTime fechaInicial = DateTime.ParseExact(data.FechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(data.FechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            MigracionesAppServicio servMigr = new MigracionesAppServicio();
            List<PrRepcvDTO> salida = servMigr.ListarDataEventosCVPorFechas(fechaInicial, fechaFinal);

            return salida;
        }                

        /// <summary>
        /// Cambia de formato al listado
        /// </summary>
        /// <param name="listado"></param>
        /// <returns></returns>
        private List<AddInColModo> ObtenerFormatoListadoModoAPintar(List<COES.Servicios.Aplicacion.Migraciones.Helper.AddInColModo> listado)
        {
            List<AddInColModo> lstSalida = new List<AddInColModo>();

            foreach (var item in listado)
            {
                AddInColModo regFormateado = new AddInColModo();
                regFormateado.AbrevModo = item.AbrevModo;
                regFormateado.Fila = item.Fila;
                regFormateado.Columna = item.Columna;
                regFormateado.Asterisco = item.Asterisco;
                regFormateado.Formula = item.Formula;

                lstSalida.Add(regFormateado);
            }

            return lstSalida;
        }

        /// <summary>
        /// Cambia de formato al listado
        /// </summary>
        /// <param name="listado"></param>
        /// <returns></returns>
        private List<COES.Servicios.Aplicacion.Migraciones.Helper.AddInColModo> ObtenerFormatoListadoModoAObtener(List<AddInColModo> listado)
        {
            List<COES.Servicios.Aplicacion.Migraciones.Helper.AddInColModo> lstSalida = new List<COES.Servicios.Aplicacion.Migraciones.Helper.AddInColModo>();

            foreach (var item in listado)
            {
                COES.Servicios.Aplicacion.Migraciones.Helper.AddInColModo regFormateado = new COES.Servicios.Aplicacion.Migraciones.Helper.AddInColModo();
                regFormateado.AbrevModo = item.AbrevModo;
                regFormateado.Fila = item.Fila;
                regFormateado.Columna = item.Columna;
                regFormateado.Asterisco = item.Asterisco;
                regFormateado.Formula = item.Formula;

                lstSalida.Add(regFormateado);
            }

            return lstSalida;
        }

        #endregion
    }
}