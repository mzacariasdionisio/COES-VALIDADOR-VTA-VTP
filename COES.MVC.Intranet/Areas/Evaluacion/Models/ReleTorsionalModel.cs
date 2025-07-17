using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.FormatoMedicion;
namespace COES.MVC.Intranet.Areas.Evaluacion.Models
{
    public class ReleTorsionalModel
    {
        public List<EprAreaDTO> ListaSubestacion { get; set; }
        public List<EprEquipoDTO> ListaCelda { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<EqAreaDTO> ListaArea { get; set; }
        public List<EprPropCatalogoDataDTO> ListaEstado { get; set; }
        public HandsonModel Handson { get; set; }

        public string Resultado { get; set; }
        public string StrMensaje { get; set; }
        public string Detalle { get; set; }
        public string NombreArchivo { get; set; }

    }

    public class ListadoReleTorsionalModel
    {
        public List<EprEquipoDTO> ListaReleTorsional { get; set; }
    }

    public class ReleTorsionalEditarModel
    {
        public int Id { get; set; }
        public int IdEpe { get; set; }
        public string Nombre { get; set; }
        public string FlagCambioNombre { get; set; }

    }

    public class ReleTorsionalEliminarModel
    {
        public int Estado { get; set; }
        public string Mensaje { get; set; }
    }
}