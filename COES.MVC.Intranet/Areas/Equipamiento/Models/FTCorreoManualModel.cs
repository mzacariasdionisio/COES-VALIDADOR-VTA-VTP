using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using System.Collections.Generic;


namespace COES.MVC.Intranet.Areas.Equipamiento.Models
{
    public class FTCorreoManualModel
    {
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Resultado { get; set; }
        public bool TienePermisoAdmin { get; set; }
        public List<EmpresaCoes> ListaEmpresas { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public List<SiCorreoDTO> ListadoCorreosEnviados { get; set; }
        public SiCorreoDTO Correo { get; set; }
        public List<FtExtProyectoDTO> ListadoProyectos { get; set; }
        public List<FTRelacionEGP> ListadoRelacionEGP { get; set; }
    }
}