using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class CuestionarioH2VADTO
    {
        public int H2vaCodi { get; set; }
        public int ProyCodi { get; set; }
        public string ProyNp { get; set; }
        public string SocioOperador { get; set; }
        public string SocioInversionista { get; set; }
        public string Distrito { get; set; }
        public string ActDesarrollar { get; set; }
        public string SituacionAct { get; set; }
        public string TipoElectrolizador { get; set; }
        public string OtroElectrolizador { get; set; }
        public Decimal? VidaUtil { get; set; }
        public Decimal? ProduccionAnual { get; set; }
        public string ObjetivoProyecto { get; set; }
        public string OtroObjetivo { get; set; }
        public string UsoEsperadoHidro { get; set; }
        public string OtroUsoEsperadoHidro { get; set; }
        public string MetodoTransH2 { get; set; }
        public string OtroMetodoTransH2 { get; set; }
        public string PoderCalorifico { get; set; }
        public string SubEstacionSein { get; set; }
        public Decimal? NivelTension { get; set; }
        public string TipoSuministro { get; set; }
        public string OtroSuministro { get; set; }
        public int? PrimeraEtapa { get; set; }
        public int? SegundaEtapa { get; set; }
        public int? Final { get; set; }
        public Decimal? CostoProduccion { get; set; }
        public Decimal? PrecioVenta { get; set; }
        public string Financiamiento { get; set; }
        public string FactFavorecenProy { get; set; }
        public string FactDesfavorecenProy { get; set; }
        public string Comentarios { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }


        public string Empresa { get; set; }
        public string NombreProyecto { get; set; }
        public string TipoProyecto { get; set; }
        //public string SubTipoProyecto { get; set; }
        public string DetalleProyecto { get; set; }
        public string Confidencial { get; set; }

        public UbicacionDTO ubicacionDTO { get; set; }
      //  public UbicacionDTO ubicacionDTO2 { get; set; }
        public List<CuestionarioH2VADet1DTO> ListCH2VADet1DTOs { get; set; }
        public List<CuestionarioH2VADet2DTO> ListCH2VADet2DTOs { get; set; }
    }

   
}
