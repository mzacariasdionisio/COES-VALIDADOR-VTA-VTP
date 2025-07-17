using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tala WB_SERVICIO_RPF
    /// </summary>
    public class ServicioRpfDTO
    {
        public int RPFCODI { get; set; }
        public int PTOMEDICODI { get; set; }
        public string INDESTADO { get; set; }
        public string LASTUSER { get; set; }
        public DateTime LASTDATE { get; set; }
        public string EMPRNOMB { get; set; }
        public string EQUINOMB { get; set; }
        public string EQUIABREV { get; set; }
        public int FAMCODI { get; set; }
        public string INDICADOR { get; set; }
        public int EQUICODI { get; set; }
        public decimal POTENCIAMAX { get; set; }
        public int CONTADOR { get; set; }
        public string INDCUMPLIMIENTO { get; set; }
        public decimal PORCENTAJE { get; set; }
        public List<ServicioRpfItemDTO> ListaItems { get; set; }
        public string INDICADORCARGA { get; set; }
        public string INDICADORPOTENCIA { get; set; }
        public string INDICADORFRECUENCIA { get; set; }
        public string HORAINICIO { get; set; }
        public string HORAFIN { get; set; }
        public decimal BALANCE { get; set; }
        public decimal NRODATOS { get; set; }
        public List<ServicioRpfSerie> ListaSerie { get; set; }
        public List<ServicioRpfSerie> ListaSuperior { get; set; }
        public List<ServicioRpfSerie> ListaInferior { get; set; }
        public List<ServicioRpfSerie> ListaPuntos { get; set; }
        public List<ServicioRpfSerie> ListaFrecuencia { get; set; }
        public List<ServicioRpfSerie> ListaPotencia { get; set; }
        public List<ServicioRpfSerie> ListaArea { get; set; }
        public List<ServicioRpfSerie> ListaSanJuan { get; set; }
        public List<ServicioRpfSerie> ListaRecta { get; set; }
        public decimal ValorRA { get; set; }
        public string ValorM { get; set; }
        public string ValorR2 { get; set; }
        public string ValorEequiv { get; set; }
        public string FechaCarga { get; set; }
        public decimal Consistencia { get; set; }
        public string EstadoOperativo { get; set; }
        public string EstadoInformacion { get; set; }
        public string IndicadorConsistencia { get; set; }
        
    }

    /// <summary>
    /// Clase que permite manejar las horas
    /// </summary>
    public class ServicioRpfItemDTO
    {
        public DateTime FechaHora { get; set; }
        public decimal Frecuencia { get; set; }
        public decimal Potencia { get; set; }
    }

    /// <summary>
    /// Objeto para almacenar los pares ordenados
    /// </summary>
    public class ServicioRpfSerie
    {
        public decimal Frecuencia { get; set; }
        public decimal Potencia { get; set; }
        public decimal Segundo { get; set; }
        public decimal Valor { get; set; }
    }

    /// <summary>
    /// Objeto para almacenar los gpss
    /// </summary>
    public class ServicioGps
    {
        public int GpsCodi { get; set; }
        public string GpsNombre { get; set; }
        public int Cantidad { get; set; }
        public string Indicador { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Frecuencia { get; set; }
        public string IndicadorCompletado { get; set; }
    }
}
