using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class CCGDADTO
    {
        public int CcgdaCodi { get; set; }
        public int ProyCodi { get; set; }
        public string NombreUnidad { get; set; }
        public string DistritoCodi { get; set; }
        public string NombreDistribuidor { get; set; }
        public string Propietario { get; set; }
        public string SocioOperador { get; set; }
        public string SocioInversionista { get; set; }
        public string ObjetivoProyecto { get; set; }
        public string OtroObjetivo { get; set; }
        public string IncluidoPlanTrans { get; set; }
        public string EstadoOperacion { get; set; }
        public string CargaRedDistribucion { get; set; }
        public string ConexionTemporal { get; set; }
        public string TipoTecnologia { get; set; }
        public DateTime? FechaAdjudicactem { get; set; }
        public DateTime? FechaAdjutitulo { get; set; }
        public string Perfil { get; set; }
        public string Prefactibilidad { get; set; }
        public string Factibilidad { get; set; }
        public string EstDefinitivo { get; set; }
        public string Eia { get; set; }
        public string FechaInicioConst { get; set; }
        public string PeriodoConst { get; set; }
        public string FechaOpeComercial { get; set; }
        public string PotInstalada { get; set; }
        public string RecursoUsada { get; set; }
        public string Tecnologia { get; set; }
        public string TecOtro { get; set; }
        public string BarraConexion { get; set; }
        public string NivelTension { get; set; }
        public string NombreProyectoGD { get; set; }
        public string DistritoGDCodi { get; set; }
        public string IncluidoPlanTransGD { get; set; }
        public string NomDistribuidorGD { get; set; }
        public string PropietarioGD { get; set; }
        public string SocioOperadorGD { get; set; }
        public string SocioInversionistaGD { get; set; }
        public string EstadoOperacionGD { get; set; }
        public string CargaRedDistribucionGD { get; set; }
        public string BarraConexionGD { get; set; }
        public string NivelTensionGD { get; set; }
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
        public string Confidencial { get; set; }

        public UbicacionDTO ubicacionDTO { get; set; }

        public UbicacionDTO ubicacionDTO2 { get; set; }
    }
}
