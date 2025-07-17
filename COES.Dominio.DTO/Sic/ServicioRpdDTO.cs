using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class ServicioRpdDTO
    {
        public int RPFCODI { get; set; }
        public int PTOMEDICODI { get; set; }
        public string INDESTADO { get; set; }
        public string LASTUSER { get; set; }
        public DateTime LASTDATE { get; set; }
        public string EMPRNOMB { get; set; }
        public string EQUINOMB { get; set; }
        public string EQUIABREV { get; set; }
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
        public List<ServicioRpfSerie> ListaSerie { get; set; }
        public List<ServicioRpfSerie> ListaSuperior { get; set; }
        public List<ServicioRpfSerie> ListaInferior { get; set; }
        public List<ServicioRpfSerie> ListaPuntos { get; set; }

    }

    /// <summary>
    /// Clase que permite manejar las horas
    /// </summary>
    public class ServicioRpdItemDTO
    {
        public DateTime FechaHora { get; set; }
        public decimal Frecuencia { get; set; }
        public decimal Potencia { get; set; }
    }

    /// <summary>
    /// Objeto para almacenar los pares ordenados
    /// </summary>
    public class ServicioRpdSerie
    {
        public decimal Frecuencia { get; set; }
        public decimal Potencia { get; set; }
    }

}
