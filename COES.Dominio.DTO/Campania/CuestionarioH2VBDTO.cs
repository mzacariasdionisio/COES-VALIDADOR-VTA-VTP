using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class CuestionarioH2VBDTO
    {
        public int H2vbCodi { get; set; }
        public int ProyCodi { get; set; }
        public string NombreUnidad { get; set; }
        public string Distrito { get; set; }
        public string Propietario { get; set; }
        public string SocioOperador { get; set; }
        public string SocioInversionista { get; set; }
        public string IncluidoPlanTrans { get; set; }
        public string ConcesionTemporal { get; set; }
        public string TipoElectrolizador { get; set; }
        public DateTime? FechaConcesionTemporal { get; set; }
        public DateTime? FechaTituloHabilitante { get; set; }
        public string Perfil { get; set; }
        public string Prefactibilidad { get; set; }
        public string Factibilidad { get; set; }
        public string EstudioDefinitivo { get; set; }
        public string EIA { get; set; }
        public string FechaInicioConstruccion { get; set; }
        public string PeriodoConstruccion { get; set; }
        public string FechaOperacionComercial { get; set; }
        public Decimal? PotenciaInstalada { get; set; }
        public string RecursoUsado { get; set; }
        public string Tecnologia { get; set; }
        public string OtroTecnologia { get; set; }
        public string BarraConexion { get; set; }
        public string NivelTension { get; set; }
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
    }
}
