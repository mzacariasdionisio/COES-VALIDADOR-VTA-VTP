using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Servicios.Aplicacion.PronosticoDemanda.Helper
{
    public class PrnFormatoExcel
    {
        public string Titulo { get; set; }
        public string Subtitulo1 { get; set; }
        public string Subtitulo2 { get; set; }
        public string[] Cabecera { get; set; }
        public string[][] Contenido { get; set; }
        public int[] AnchoColumnas { get; set; }
        public string NombreLibro { get; set; }
        public List<PrnExcelHeader> NestedHeader1 { get; set; }
        public List<PrnExcelHeader> NestedHeader2 { get; set; }
        public List<PrnExcelHeader> NestedHeader3 { get; set; }
        public List<PrnExcelHeader> NestedHeader4 { get; set; }
        public string[][] ColorByCells { get; set; }
    }

    public class PrnExcelHeader
    {
        public int Columnas { get; set; }
        public string Etiqueta { get; set; }
    }

    public class PrnPatronModel
    {
        public bool EsLunes { get; set; }
        public int NDias { get; set; }
        public string Mensaje { get; set; }
        public List<DateTime> Fechas { get; set; }
        public List<string> StrFechas { get; set; }
        public List<string> StrFechasTarde { get; set; }
        public decimal[] Patron { get; set; }
        public decimal[] PatronDefecto { get; set; }
        public List<decimal[]> Mediciones { get; set; }
        public List<PrnMedicion48DTO> PrnMediciones { get; set; }
        public List<PrnMedicionesRawDTO> PrnMedicionesRaw { get; set; }
        public List<PrnEstimadorRawDTO> PrnEstimadorRaw { get; set; }
        public string TipoPatron { get; set; }
        //Prodem3 Estimador
        public List<PrnMediciongrpDTO> PrnMedicionesEstimador { get; set; }
    }
}
