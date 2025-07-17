using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.CalculoResarcimientos;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.CalculoResarcimiento.Models
{
    public class CorreoModel
    {
        public bool TienePermisoAdmin { get; set; }
        public bool TienePermisoNuevo { get; set; }
        public bool TienePermisoEditar { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public List<ReLogcorreoDTO> ListadoCorreosEnviados { get; set; }
        public List<RePeriodoDTO> ListaPeriodo { get; set; }
        public List<ReEmpresaDTO> ListaEmpresas { get; set; }
        public SiCorreoDTO Correo { get; set; }
        public bool EsSemestral { get; set; }
        public string CampoEmpresa { get; set; }
        public bool MuestraEmpresa { get; set; }

        public List<EmpresaCorreo> ListaEmpresasCorreo { get; set; }
        public string ListaCorreosPorEmpresa { get; set; }
        public int IdPlantilla { get; set; }
        public string NombreEmpresa { get; set; }

        public int Anio { get; set; }
        public int Repercodi { get; set; }
        public bool Grabar { get; set; }
        
    }
}