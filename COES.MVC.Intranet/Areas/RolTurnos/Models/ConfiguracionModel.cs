using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.General;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.RolTurnos.Models
{
    public class ConfiguracionModel
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public EstructuraConfiguracionRolTurno Estructura { get; set; }
        public List<RtuActividadDTO> ListaActividad { get; set; }
        public RtuActividadDTO EntidadActividad { get; set; }

        public string AbreviaturaActividad { get; set; }
        public string DescripcionActividad { get; set; }
        public string EstadoActividad { get; set; }
        public int CodigoActividad { get; set; }
        public string Reporte { get; set; }
        public int? TipoResponsble { get; set; }
        public List<RtuActividadDTO> ListaTipoResponsabilidad { get; set; }
    }
}