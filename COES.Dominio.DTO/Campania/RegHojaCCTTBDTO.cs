using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class RegHojaCCTTBDTO
    {
        public int Centralcodi { get; set; }
        public int Proycodi { get; set; }
        public decimal? Estudiofactibilidad { get; set; }
        public decimal? Investigacionescampo { get; set; }
        public decimal? Gestionesfinancieras { get; set; }
        public decimal? Disenospermisos { get; set; }
        public decimal? Obrasciviles { get; set; }
        public decimal? Equipamiento { get; set; }
        public decimal? Lineatransmision { get; set; }
        public decimal? Obrasregulacion { get; set; }
        public decimal? Administracion { get; set; }
        public decimal? Aduanas { get; set; }
        public decimal? Supervision { get; set; }
        public decimal? Gastosgestion { get; set; }
        public decimal? Imprevistos { get; set; }
        public decimal? Igv { get; set; }
        public decimal? Usoagua { get; set; }
        public decimal? Otrosgastos { get; set; }
        public decimal? Inversiontotalsinigv { get; set; }
        public decimal? Inversiontotalconigv { get; set; }
        public string Financiamientotipo { get; set; }
        public string Financiamientoestado { get; set; }
        public decimal? Porcentajefinanciado { get; set; }
        public string Concesiondefinitiva { get; set; }
        public string Ventaenergia { get; set; }
        public string Ejecucionobra { get; set; }
        public string Contratosfinancieros { get; set; }
        public string Observaciones { get; set; }
        public string Usucreacion { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Usumodificacion { get; set; }
        public DateTime Fechamodificacion { get; set; }
        public string IndDel { get; set; }

        public string Empresa { get; set; }

        public string NombreProyecto { get; set; }

        public string TipoProyecto { get; set; }

        public string SubTipoProyecto { get; set; }

        public string DetalleProyecto { get; set; }
        public string Condifencial { get; set; }
    }
}
