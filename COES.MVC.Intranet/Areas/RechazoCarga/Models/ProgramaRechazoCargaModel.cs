using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.FormatoMedicion;

namespace COES.MVC.Intranet.Areas.RechazoCarga.Models
{
    public class ProgramaRechazoCargaModel
    {
        public List<RcaHorizonteProgDTO> Horizontes { get; set; }

        public List<RcaConfiguracionProgDTO> Configuraciones { get; set; }

        public List<RcaCuadroProgUsuarioDTO> ListProgramaRechazoCargaEmpresa { get; set; }

        public List<RcaProgramaDTO> Programas { get; set; }

        public RcaCuadroProgDTO RcaCuadroProgDTO { get; set; }

        public RcaProgramaDTO RcaProgramaDTO { get; set; }

        public List<AreaDTO> SubEstaciones { get; set; }

        public List<AreaDTO> Zonas { get; set; }

        public bool esConsulta { get; set; }
        public bool bEditar { get; set; }
        public bool bAdicional { get; set; }

        public int SemanaActual { get; set; }

        public List<SiEmpresaDTO> ListSiEmpresa { get; set; }

        /// <summary>
        /// Propiedad para cargar datos de distribuidores
        /// </summary>
        public string[][] DatosMatriz { get; set; }
        public int Registro { get; set; }

        public string MensajeDistribucion { get; set; }

        public FormatoModel ModeloUsuariosLibres { get; set; }

        public bool CargarDistribuidores { get; set; }

        public int Perfil { get; set; }

        public string mensajeErrorBuscadorClientes { get; set; }

        public bool editarDemanda { get; set; }

        public string DistribuidoresEliminados { get; set; }
    }
}