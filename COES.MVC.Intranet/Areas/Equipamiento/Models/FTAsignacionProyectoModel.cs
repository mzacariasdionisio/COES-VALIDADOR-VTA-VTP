using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using System.Collections.Generic;


namespace COES.MVC.Intranet.Areas.Equipamiento.Models
{    
    public class FTAsignacionProyectoModel
    {
               
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public FtExtRelempetapaDTO Proyecto  { get; set; }
        public List<EmpresaCoes> ListaEmpresas { get; set; }
        public List<FtExtEtapaDTO> ListaEtapas { get; set; }
        public List<FtExtRelempetapaDTO> ListadoProyectosAsig { get; set; }
        public List<FtExtProyectoDTO> ListadoProyectos { get; set; }
        public List<FTRelacionEGP> ListadoRelacionEGP { get; set; }
        public FtExtEtempdetpryeqDTO DetalleCIO { get; set; }
        public FtExtEtempdeteqDTO DetalleO { get; set; }

        public List<EqFamiliaDTO> ListaFamilia { get; set; }
        public List<PrCategoriaDTO> ListaCategoria { get; set; }
        public List<EqAreaDTO> ListaUbicacion { get; set; }
        
    }

}