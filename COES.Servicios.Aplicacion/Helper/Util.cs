using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Siosein2;
using HighchartsExportClient;

namespace COES.Servicios.Aplicacion.Helper
{
    /// <summary>
    /// Clase para colocar métodos comunes utilizados
    /// en la capa de aplicación
    /// </summary>
    public class Util
    {
        public static string EstadoDescripcion(string areaestado)
        {
            string estadoDescripcion;
            areaestado = areaestado == null ? string.Empty : areaestado.ToUpper().Trim();
            switch (areaestado)
            {
                case ConstantesAppServicio.Activo:
                    estadoDescripcion = ConstantesAppServicio.ActivoDesc;
                    break;
                case ConstantesAppServicio.Baja:
                    estadoDescripcion = ConstantesAppServicio.BajaDesc;
                    break;
                case ConstantesAppServicio.Proyecto:
                    estadoDescripcion = ConstantesAppServicio.ProyectoDesc;
                    break;
                case ConstantesAppServicio.Anulado:
                    estadoDescripcion = ConstantesAppServicio.AnuladoDesc;
                    break;
                case ConstantesAppServicio.FueraCOES:
                    estadoDescripcion = ConstantesAppServicio.FueraCOESDesc;
                    break;
                default:
                    estadoDescripcion = string.Empty;
                    break;
            }
            return estadoDescripcion;
        }

        /// <summary>
        /// Descripcion SI/NO
        /// </summary>
        /// <param name="sValor"></param>
        /// <returns></returns>
        public static string SiNoDescripcion(string sValor)
        {
            string siNoDesc;
            switch (sValor.ToUpperInvariant().Trim())
            {
                case ConstantesAppServicio.SI:
                    siNoDesc = ConstantesAppServicio.SIDesc;
                    break;
                case ConstantesAppServicio.NO:
                    siNoDesc = ConstantesAppServicio.NODesc;
                    break;
                default:
                    siNoDesc = string.Empty;
                    break;
            }
            return siNoDesc;
        }

        /// <summary>
        /// Descripción del estado de la empresa
        /// </summary>
        /// <param name="estado"></param>
        /// <returns></returns>
        public static string EmpresaEstadoDescripcion(string estado)
        {
            string estadoDescripcion = ConstantesAppServicio.EliminadoDesc;
            switch (estado)
            {
                case ConstantesAppServicio.Activo:
                    estadoDescripcion = ConstantesAppServicio.ActivoDesc;
                    break;
                case ConstantesAppServicio.Baja:
                    estadoDescripcion = ConstantesAppServicio.BajaDesc;
                    break;
                case ConstantesAppServicio.Eliminado:
                    estadoDescripcion = ConstantesAppServicio.EliminadoDesc;
                    break;
            }
            return estadoDescripcion;
        }

        /// <summary>
        /// Descripción Si la empresa es Agente (S) o no es Agente (N
        /// </summary>
        /// <param name="estado"></param>
        /// <returns></returns>
        public static string AgenteEstadoDescripcion(string estado)
        {
            string siNoDesc = string.Empty;
            switch (estado)
            {
                case ConstantesAppServicio.SI:
                    siNoDesc = ConstantesAppServicio.SIDesc;
                    break;
                case ConstantesAppServicio.NoAgente:
                case ConstantesAppServicio.NO:
                    siNoDesc = ConstantesAppServicio.NODesc;
                    break;
            }
            return siNoDesc;
        }

        /// <summary>
        /// Listar todos los estados de un registro
        /// </summary>
        /// <returns></returns>
        public static List<EstadoParametro> ListaEstadoAll()
        {
            List<EstadoParametro> ListaEstado = new List<EstadoParametro>();
            ListaEstado.Add(new EstadoParametro { EstadoCodigo = ConstantesAppServicio.Activo, EstadoDescripcion = ConstantesAppServicio.ActivoDesc });
            ListaEstado.Add(new EstadoParametro { EstadoCodigo = ConstantesAppServicio.Baja, EstadoDescripcion = ConstantesAppServicio.BajaDesc });
            ListaEstado.Add(new EstadoParametro { EstadoCodigo = ConstantesAppServicio.Anulado, EstadoDescripcion = ConstantesAppServicio.AnuladoDesc });
            ListaEstado.Add(new EstadoParametro { EstadoCodigo = ConstantesAppServicio.Proyecto, EstadoDescripcion = ConstantesAppServicio.ProyectoDesc });
            ListaEstado.Add(new EstadoParametro { EstadoCodigo = ConstantesAppServicio.FueraCOES, EstadoDescripcion = ConstantesAppServicio.FueraCOESDesc });

            return ListaEstado;
        }

        /// <summary>
        /// Listar todos los estados de un registro
        /// </summary>
        /// <returns></returns>
        public static List<EstadoParametro> ListaEstado()
        {
            List<EstadoParametro> ListaEstado = new List<EstadoParametro>();
            ListaEstado.Add(new EstadoParametro { EstadoCodigo = ConstantesAppServicio.Activo, EstadoDescripcion = ConstantesAppServicio.ActivoDesc });
            ListaEstado.Add(new EstadoParametro { EstadoCodigo = ConstantesAppServicio.Baja, EstadoDescripcion = ConstantesAppServicio.BajaDesc });
            ListaEstado.Add(new EstadoParametro { EstadoCodigo = ConstantesAppServicio.Anulado, EstadoDescripcion = ConstantesAppServicio.AnuladoDesc });
            ListaEstado.Add(new EstadoParametro { EstadoCodigo = ConstantesAppServicio.Proyecto, EstadoDescripcion = ConstantesAppServicio.ProyectoDesc });

            return ListaEstado;
        }

        /// <summary>
        /// Get Posicion H inicial de un rango
        /// </summary>
        /// <param name="horaIni"></param>
        /// <param name="horaFin"></param>
        /// <returns></returns>
        public static int[] GetPosicionHoraInicial48(DateTime horaIni)
        {
            int[] lista = new int[2];
            //posicion inicial
            int hh1 = horaIni.Hour;
            int mi1 = horaIni.Minute;
            int pp1 = 0, tiempo1 = 0;

            if ((0 <= mi1) && (mi1 <= 30))
            {
                pp1 = 1;
                tiempo1 = 30 - mi1;
            }
            if ((30 < mi1) && (mi1 < 60))
            {
                pp1 = 2;
                tiempo1 = 60 - mi1;
            }
            int pos1 = hh1 * 2 + pp1;

            //
            lista[0] = pos1;
            lista[1] = tiempo1;
            return lista;
        }

        /// <summary>
        /// Get Posicion H final de un rango
        /// </summary>
        /// <param name="horaIni"></param>
        /// <returns></returns>
        public static int[] GetPosicionHoraFinal48(DateTime horaFin)
        {
            int[] lista = new int[2];
            //posicion final
            int hh2 = horaFin.Hour;
            int mi2 = horaFin.Minute;
            int pp2 = 0, tiempo2 = 0;

            if ((0 <= mi2) && (mi2 <= 30))
            {
                pp2 = 1;
                tiempo2 = mi2 - 15;
            }
            if ((30 < mi2) && (mi2 < 60))
            {
                pp2 = 2;
                tiempo2 = mi2 - 30;
            }
            int pos2 = hh2 * 2 + pp2;
            if (hh2 == 0 && mi2 == 0)
            {
                pos2 = hh2 * 2 + 48;
            }

            //
            lista[0] = pos2;
            lista[1] = tiempo2;
            return lista;
        }

        /// <summary>
        /// Get Posicion H inicial de un rango
        /// </summary>
        /// <param name="horaIni"></param>
        /// <param name="horaFin"></param>
        /// <returns></returns>
        public static int[] GetPosicionHoraInicial96(DateTime horaIni)
        {
            int[] lista = new int[2];
            //posicion inicial
            int hh1 = horaIni.Hour;
            int mi1 = horaIni.Minute;
            int pp1 = 0, tiempo1 = 0;

            if ((0 <= mi1) && (mi1 <= 15))
            {
                pp1 = 1;
                tiempo1 = 15 - mi1;
            }
            if ((15 < mi1) && (mi1 <= 30))
            {
                pp1 = 2;
                tiempo1 = 30 - mi1;
            }
            if ((30 < mi1) && (mi1 <= 45))
            {
                pp1 = 3;
                tiempo1 = 45 - mi1;
            }
            if ((45 < mi1) && (mi1 < 60))
            {
                pp1 = 4;
                tiempo1 = 60 - mi1;
            }
            int pos1 = hh1 * 4 + pp1;

            //
            lista[0] = pos1;
            lista[1] = tiempo1;
            return lista;
        }

        /// <summary>
        /// Get Posicion H final de un rango
        /// </summary>
        /// <param name="horaIni"></param>
        /// <returns></returns>
        public static int[] GetPosicionHoraFinal96(DateTime horaFin)
        {
            int[] lista = new int[2];
            //posicion final
            int hh2 = horaFin.Hour;
            int mi2 = horaFin.Minute;
            int pp2 = 0, tiempo2 = 0;

            if ((0 <= mi2) && (mi2 <= 15))
            {
                pp2 = 1;
                tiempo2 = mi2;
            }
            if ((15 < mi2) && (mi2 <= 30))
            {
                pp2 = 2;
                tiempo2 = mi2 - 15;
            }
            if ((30 < mi2) && (mi2 <= 45))
            {
                pp2 = 3;
                tiempo2 = mi2 - 30;
            }
            if ((45 < mi2) && (mi2 < 60))
            {
                pp2 = 4;
                tiempo2 = mi2 - 45;
            }
            int pos2 = hh2 * 4 + pp2;
            if (hh2 == 0 && mi2 == 0)
            {
                pos2 = hh2 * 4 + 96;
            }

            //
            lista[0] = pos2;
            lista[1] = tiempo2;
            return lista;
        }

        /// <summary>
        /// Descripción del periodo
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public static string PeriodoDescripcion(int periodo)
        {
            string descripcion = string.Empty;
            switch (periodo)
            {
                case 1:
                    descripcion = "Diario";
                    break;
                case 2:
                    descripcion = "Semanal";
                    break;
                case 3:
                    descripcion = "Mensual";
                    break;
                case 4:
                    descripcion = "Anual";
                    break;
                case 5:
                    descripcion = "Mensual x Semana";
                    break;
                case 6:
                    descripcion = "Periodo PMPO";
                    break;
                default:
                    descripcion = "No Definido";
                    break;
            }

            return descripcion;
        }

        /// <summary>
        /// Descripción de la resolución
        /// </summary>
        /// <param name="resolucion"></param>
        /// <returns></returns>
        public static string ResolucionDescripcion(int resolucion)
        {
            string descripcion = string.Empty;
            switch (resolucion)
            {
                case 15:
                    descripcion = "15 Minutos";
                    break;
                case 30:
                    descripcion = "30 Minutos";
                    break;
                case 60:
                    descripcion = "1 Hora";
                    break;
                case 1440:
                    descripcion = "1 Día";
                    break;
                case 10080:
                    descripcion = "1 Semana";
                    break;
                case 43200:
                    descripcion = "1 Mes";
                    break;
                default:
                    descripcion = "No Definido";
                    break;
            }

            return descripcion;
        }

        /// <summary>
        /// Descripción de horizonte
        /// </summary>
        /// <param name="horizonte"></param>
        /// <returns></returns>
        public static string HorizonteDescripcion(int horizonte)
        {
            string descripcion = string.Empty;
            switch (horizonte)
            {
                case 1:
                    descripcion = "1 Día";
                    break;
                case 3:
                    descripcion = "3 Días";
                    break;
                case 7:
                    descripcion = "7 Días";
                    break;
                case 10:
                    descripcion = "10 Días";
                    break;
                case 30:
                    descripcion = "1 Mes";
                    break;
                case 90:
                    descripcion = "3 Mes";
                    break;
                case 365:
                    descripcion = "1 Año";
                    break;
                default:
                    descripcion = "No Definido";
                    break;
            }

            return descripcion;
        }

        /// <summary>
        /// Serializando any lista
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static byte[] SerializarObjeto(object lista)
        {
            //Serializando
            try
            {
                byte[] listByte = null;
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, lista);
                    listByte = ms.ToArray();
                }

                return listByte;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Deserializando any objeto byte
        /// </summary>
        /// <param name="listByte"></param>0
        /// <returns></returns>
        public static T DeserializarObjeto<T>(byte[] listByte)
        {
            //Deserializando
            try
            {
                if (listByte == null) return default(T);

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream(listByte))
                {
                    return (T)binaryFormatter.Deserialize(ms);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="settings"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string ObtenerEnlaceImagenHighchartsDescargado(string options, HighchartsSetting settings, string filename)
        {
            var enlaceGrafico = ObtenerEnlaceDelGrafico(options, settings);
            DescargarArchivo(new Uri(enlaceGrafico), filename);
            return filename;
        }

        /// <summary>
        /// Retorna Bitmap desde una direccion uri
        /// </summary>
        /// <param name="uri">Dirección URI</param>
        /// <returns></returns>
        public static Bitmap ObtenerBitmapDesdeStringURI(Uri uri)
        {
            using (var client = new WebClient())
            {
                Stream stream = client.OpenRead(uri);
                return new Bitmap(stream ?? throw new InvalidOperationException());
            }
        }

        /// <summary>
        /// Obtiene el enlace del grafico generado en la API de Highcharts
        /// </summary>
        /// <param name="options">JSON string</param>
        /// <param name="settings">Configuración</param>
        /// <returns></returns>
        public static string ObtenerEnlaceDelGrafico(string options, HighchartsSetting settings)
        {

            using (var clientHighchartsClient = new HighchartsClient(settings))
            {
                return clientHighchartsClient.GetChartImageLinkFromOptionsAsync(options).GetAwaiter().GetResult();
            }
        }

        /// <summary>
        /// Descarga aarchivo
        /// </summary>
        /// <param name="enlace">Enlace URI</param>
        /// <param name="filename">Nombre de archivo</param>
        public static void DescargarArchivo(Uri enlace, string filename)
        {
            using (var clientw = new WebClient())
            {
                if (File.Exists(filename)) File.Delete(filename);
                clientw.DownloadFile(enlace, filename);
            }
        }

        /// <summary>
        /// Convierte Hora:Minuto a Hx (H1,H2,...)
        /// </summary>
        /// <param name="fecha">Datetime: fecha</param>
        /// <param name="medicion">TipoMedicion: medicion</param>
        /// <returns></returns>
        public static int ConvertirHoraMinutosAHx(DateTime fecha, ConstantesSiosein2.TipoMedicion medicion)
        {
            int resultado;
            var totalMinutes = fecha.TimeOfDay.TotalMinutes;
            switch (medicion)
            {
                case ConstantesSiosein2.TipoMedicion.Medicion24:
                    resultado = fecha.Hour;
                    break;
                case ConstantesSiosein2.TipoMedicion.Medicion48:
                    resultado = (totalMinutes == 0) ? 48 : ((int)totalMinutes / 30) + ((int)totalMinutes % 30 == 0 ? 0 : 1);
                    break;
                case ConstantesSiosein2.TipoMedicion.Medicion96:
                    resultado = (totalMinutes == 0) ? 96 : ((int)totalMinutes / 15) + ((int)totalMinutes % 15 == 0 ? 0 : 1);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("medicion", medicion, null);
            }

            return resultado;
        }

        /// <summary>
        /// Convierte Hora:Minuto a Hx (H1,H2,...)
        /// </summary>
        /// <param name="fecha">Datetime: fecha</param>
        /// <param name="medicion">TipoMedicion: medicion</param>
        /// <returns></returns>
        public static int ConvertirHoraMinutosAHx(DateTime fecha, ConstantesSiosein2.TipoMedicion medicion, ConstantesSiosein2.TipoHora hora)
        {
            int resultado = 0;
            var totalMinutes = fecha.TimeOfDay.TotalMinutes;
            switch (medicion)
            {
                case ConstantesSiosein2.TipoMedicion.Medicion24:
                    if (totalMinutes == 0 && hora == ConstantesSiosein2.TipoHora.HxFin) { resultado = 24; break; }
                    if (totalMinutes == 0 && hora == ConstantesSiosein2.TipoHora.HxInicio) { resultado = 1; break; }
                    resultado = fecha.Hour;
                    break;
                case ConstantesSiosein2.TipoMedicion.Medicion48:

                    if (totalMinutes == 0 && ConstantesSiosein2.TipoHora.HxInicio == hora) { resultado = 1; break; }
                    if (totalMinutes == 0 && ConstantesSiosein2.TipoHora.HxFin == hora) { resultado = 48; break; }
                    resultado = ((int)totalMinutes / 30) + ((int)totalMinutes % 30 == 0 ? 0 : 1);
                    break;
                case ConstantesSiosein2.TipoMedicion.Medicion96:
                    if (totalMinutes == 0 && hora == ConstantesSiosein2.TipoHora.HxInicio) { resultado = 1; break; }
                    if (totalMinutes == 0 && hora == ConstantesSiosein2.TipoHora.HxFin) { resultado = 96; break; }
                    resultado = ((int)totalMinutes / 15) + ((int)totalMinutes % 15 == 0 ? 0 : 1);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("medicion", medicion, null);
            }

            return resultado;
        }

        public static decimal? ConvertirADecimal(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            return Convert.ToDecimal(value, CultureInfo.InvariantCulture);
        }
        public static int? ConvertirAEntero(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            return Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Intervalo de fechas dividido por dias
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="dayChunkSize"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<DateTime, DateTime>> SplitDateRange(DateTime start, DateTime end, int dayChunkSize)
        {
            DateTime chunkEnd;
            while ((chunkEnd = start.AddDays(dayChunkSize).Date) < end)
            {
                yield return Tuple.Create(start, chunkEnd);
                start = chunkEnd;
            }
            yield return Tuple.Create(start, end);
        }
    }

    /// <summary>
    /// Clase para comparar continuidad entre rangos de fechas
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Range<T> where T : IComparable
    {
        public Range()
        {

        }

        public Range(T min, T max)
        {
            Min = min;
            Max = max;
        }

        public bool IsOverlapped(Range<T> other)
        {
            return Min.CompareTo(other.Max) < 0 && other.Min.CompareTo(Max) < 0;
        }

        public bool IsAdjacent(Range<T> other)
        {
            return Min.CompareTo(other.Max) == 0 || other.Min.CompareTo(Max) == 0;
        }

        public T Min { get; }
        public T Max { get; }
    }
}
