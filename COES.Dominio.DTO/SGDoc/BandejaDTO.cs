using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.SGDoc
{
    public class BandejaDTO
    {
        public int CodRegistro { get; set; }
        public decimal Correlativo { get; set; }
        public string NombAreaOrigen { get; set; }
        public int Areaorig { get; set; }
        public string NumDocumento { get; set; }
        public string Asunto { get; set; }
        public DateTime FechaLlegadaCoes { get; set; }
        public string Xfileruta { get; set; }
        public int Filecodi { get; set; }
        public string Fljremdetalle { get; set; }
        public string Estado { get; set; }
        public int CodDetregistro { get; set; }
        public string NombAreaDestino { get; set; }
        public int Fljdetdestino { get; set; }
        public int Fljdetorigen { get; set; }
        public int Fljdetnivel { get; set; }
        public string Fljcadatencion { get; set; }
        public string Codatencion { get; set; }
        public string ComentarioPadre { get; set; }
        public DateTime Fljfechaproce { get; set; }
        public int Area { get; set; }
        public int Conplazo { get; set; }
        public DateTime? FechaMaxAtencion { get; set; }
        public string ArchivoAdm { get; set; }
        public string ArchivoAdmSub { get; set; }
        public string NombreEmpresaRem { get; set; }
        public string Confidencial { get; set; }
        public int AnioLibro { get; set; }
        public int Prioridad { get; set; }
        public int Atributos { get; set; }
        public string Rpta { get; set; }
        public string Leido { get; set; }
        public int Nmsg { get; set; }
        public string Estadohijo { get; set; }
        public DateTime? FechaAtencion { get; set; }
        public string Importancia { get; set; }
        public bool TiempoMaxAtencion { get; set; }
        public string EnlaceArchivo { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string Cursiva { get; set; }
        public string ColorFondo { get; set; }
        public bool Bold { get; set; }
        public string ColorLetra { get; set; }
        public string IndOrigenRecordatorio { get; set; }
        public string IndNoAtendidos { get; set; }
    }

    public class MigracionDTO
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public int Flujodetcodi { get; set; }
        public int Flujocodi { get; set; }
        public string Rutafile { get; set; }
        public string Rutacd { get; set; }
        public string Rutavolumen { get; set; }
        public string Fileruta { get; set; }
    }


}