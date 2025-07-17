using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.FormatoMedicion;
namespace COES.MVC.Intranet.Areas.Evaluacion.Models
{
    public class ReleSincronismoModel
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

    public class ListadoReleSincronismoModel
    {
        public List<EprEquipoDTO> ListaReleSincronismo { get; set; }
    }

    public class ReleSincronismoEditarModel
    {
        public int Id { get; set; }
        public int IdEpe { get; set; }
        public string Nombre { get; set; }
        public string FlagCambioNombre { get; set; }

    }

    public class ReleSincronismoEliminarModel
    {
        public int Estado { get; set; }
        public string Mensaje { get; set; }
    }
}