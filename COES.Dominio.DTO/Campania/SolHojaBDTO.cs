using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class SolHojaBDTO
    {

        public int SolhojabCodi { get; set; }
        public int ProyCodi { get; set; }
        public Decimal? Estudiofactibilidad { get; set; }
        public Decimal? Investigacionescampo { get; set; }
        public Decimal? Gestionesfinancieras { get; set; }
        public Decimal? Disenospermisos { get; set; }
        public Decimal? Obrasciviles { get; set; }
        public Decimal? Equipamiento { get; set; }
        public Decimal? Lineatransmision { get; set; }
        public Decimal? Administracion { get; set; }
        public Decimal? Aduanas { get; set; }
        public Decimal? Supervision { get; set; }
        public Decimal? Gastosgestion { get; set; }
        public Decimal? Imprevistos { get; set; }
        public Decimal? Igv { get; set; }
        public Decimal? Otrosgastos { get; set; }
        public Decimal? Inversiontotalsinigv { get; set; }
        public Decimal? Inversiontotalconigv { get; set; }
        public string Financiamientotipo { get; set; }
        public string Financiamientoestado { get; set; }
        public Decimal? Porcentajefinanciado { get; set; }
        public string Concesiondefinitiva { get; set; }
        public string Ventaenergia { get; set; }
        public string Ejecucionobra { get; set; }
        public string Contratosfinancieros { get; set; }
        public string Observaciones { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }

        public string Empresa { get; set; }

        public string NombreProyecto { get; set; }

        public string TipoProyecto { get; set; }

        public string SubTipoProyecto { get; set; }

        public string DetalleProyecto { get; set; }

        public string Confidencial { get; set; }
    }
}
