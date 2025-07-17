using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class RegHojaADTO
    {
        public int Fichaacodi { get; set; }
        public int Proycodi { get; set; }
        public string Centralnombre { get; set; }
        public string Distrito { get; set; }
        public string Cuenca { get; set; }
        public string Rio { get; set; }
        public string Propietario { get; set; }
        public string Sociooperador { get; set; }
        public string Socioinversionista { get; set; }
        public string Concesiontemporal { get; set; }
        public DateTime? Fechaconcesiontemporal { get; set; }
        public string Tipoconcesionactual { get; set; }
        public DateTime? Fechaconcesionactual { get; set; }
        public string Nombreestacion { get; set; }
        public string Numestacion { get; set; }
        public string Periodohistorica { get; set; }
        public string Periodonaturalizada { get; set; }
        public string Demandaagua { get; set; }
        public string Estudiogeologico { get; set; }
        public string Perfodiamantinas { get; set; }
        public string Numcalicatas { get; set; }
        public string EstudioTopografico { get; set; }
        public string Levantamientotopografico { get; set; }
        public string Alturabruta { get; set; }
        public string Alturaneta { get; set; }
        public string Caudaldiseno { get; set; }
        public string Potenciainstalada { get; set; }
        public string Conduccionlongitud { get; set; }
        public string Tunelarea { get; set; }
        public string Tuneltipo { get; set; }
        public string Tuberialongitud { get; set; }
        public string Tuberiadiametro { get; set; }
        public string Tuberiatipo { get; set; }
        public string Maquinatipo { get; set; }
        public string Maquinaaltitud { get; set; }
        public string Regestacionalvbruto { get; set; }
        public string Regestacionalvutil { get; set; }
        public string Regestacionalhpresa { get; set; }
        public string Reghorariavutil { get; set; }
        public string Reghorariahpresa { get; set; }
        public string Reghorariaubicacion { get; set; }
        public string Energhorapunta { get; set; }
        public string Energfuerapunta { get; set; }
        public string Tipoturbina { get; set; }
        public string Velnomrotacion { get; set; }
        public string Potturbina { get; set; }
        public string Numturbinas { get; set; }
        public string Potgenerador { get; set; }
        public string Numgeneradores { get; set; }
        public string Tensiongeneracion { get; set; }
        public string Rendimientogenerador { get; set; }
        public string Tensionkv { get; set; }
        public string Longitudkm { get; set; }
        public string Numternas { get; set; }
        public string Nombresubestacion { get; set; }
        public string Perfil { get; set; }
        public string Prefactibilidad { get; set; }
        public string Factibilidad { get; set; }
        public string Estudiodefinitivo { get; set; }
        public string Eia { get; set; }
        public string Fechainicioconstruccion { get; set; }
        public string Periodoconstruccion { get; set; }
        public string Fechaoperacioncomercial { get; set; }
        public string Comentarios { get; set; }
        public string Usucreacion { get; set; }

        public UbicacionDTO ubicacionDTO { get; set; }
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
