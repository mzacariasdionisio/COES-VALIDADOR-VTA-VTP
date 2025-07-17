using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using System.Collections.Generic;


namespace COES.MVC.Intranet.Areas.Equipamiento.Models
{    
    public class FTProyectoModel
    {
        public bool TienePermisoAdmin { get; set; }
        public bool TienePermisoNuevo { get; set; }
        public bool TienePermisoEditar { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Ruta { get; set; }

        public EmpresaCoes Empresa { get; set; }
        public FtExtProyectoDTO Proyecto { get; set; }
        public FtExtReleqpryDTO RelEquipoProyecto { get; set; }
        public FtExtReleqempltDTO RelLTEmpresa { get; set; }
        
        public List<FtExtProyectoDTO> ListadoProyectos { get; set; }
        public List<EpoEstudioEoDTO> ListadoEstudiosEo { get; set; }
        public EpoEstudioEoDTO EstudioEO { get; set; }
        

        public List<EmpresaCoes> ListaEmpresas { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresas { get; set; }
        public string RangoIni { get; set; }
        public string RangoFin { get; set; }
                
    }

}