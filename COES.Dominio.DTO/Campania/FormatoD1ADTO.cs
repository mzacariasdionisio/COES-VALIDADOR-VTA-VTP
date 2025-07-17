using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class FormatoD1ADTO
    {
        public int FormatoD1ACodi { get; set; }
        public int ProyCodi { get; set; }
        public string TipoCarga { get; set; }
        public string Nombre { get; set; }
        public string EmpresaProp { get; set; }
        public string Distrito { get; set; }
        public string ActDesarrollo { get; set; }
        public string SituacionAct { get; set; }
        public string Exploracion { get; set; }
        public string EstudioPreFactibilidad { get; set; }
        public string EstudioFactibilidad { get; set; }
        public string EstudioImpAmb { get; set; }

        public string Financiamiento1 { get; set; }
       
        public string Ingenieria { get; set; }
        public string Construccion { get; set; }
        public string PuestaMarchar { get; set; }
        public string TipoExtraccionMin { get; set; }
        public string MetalesExtraer { get; set; }
        public string TipoYacimiento { get; set; }
        public int? VidaUtil { get; set; }
        public string Reservas { get; set; }
        public string EscalaProduccion { get; set; }
        public string PlantaBeneficio { get; set; }
        public string RecuperacionMet { get; set; }
        public string LeyesConcentrado { get; set; }
        public string CapacidadTrata { get; set; }
        public string ProduccionAnual { get; set; }
        public string Item { get; set; }
        public string ToneladaMetrica { get; set; }
        public string Energia { get; set; }
        public string Consumo { get; set; }
        public string SubestacionCodi { get; set; }
        public string SubestacionOtros { get; set; }
        public decimal? NivelTension { get; set; }
        public string EmpresaSuminicodi { get; set; }
        public string EmpresaSuminiOtro { get; set; }
        public decimal? FactorPotencia { get; set; }
        public decimal? Inductivo { get; set; }
        public decimal? Capacitivo { get; set; }
        public int? PrimeraEtapa { get; set; }
        public int? SegundaEtapa { get; set; }
        public int? Final { get; set; }
        public decimal? CostoProduccion { get; set; }
        public decimal? Metales { get; set; }
        public decimal? Precio { get; set; }

        public string Financiamiento2 { get; set; }

        public string FacFavEjecuProy { get; set; }
        public string FactDesEjecuProy { get; set; }
        public string Comentarios { get; set; }
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
        public string Condifencial { get; set; }

        public UbicacionDTO ubicacionDTO { get; set; }

        public List<FormatoD1ADet1DTO> ListaFormatoDet1A { get; set; }
        public List<FormatoD1ADet2DTO> ListaFormatoDet2A { get; set; }
        public List<FormatoD1ADet3DTO> ListaFormatoDet3A { get; set; }
        public List<FormatoD1ADet4DTO> ListaFormatoDet4A { get; set; }
        public List<FormatoD1ADet5DTO> ListaFormatoDet5A { get; set; }

    }
}
