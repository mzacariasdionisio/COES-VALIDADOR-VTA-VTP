using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.FormatoMedicion;
namespace COES.MVC.Intranet.Areas.Evaluacion.Models
{
    public class TransversalModel
    {
        public List<EprAreaDTO> ListaSubestacion { get; set; }
        public List<EprEquipoDTO> ListaCelda { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<EqAreaDTO> ListaArea { get; set; }
        public List<EprPropCatalogoDataDTO> ListaEstado { get; set; }
        public string CodigoId { get; set; }
        public string Codigo { get; set; }
        public string Ubicacion { get; set; }
        public string Empresa { get; set; }
        public string Area { get; set; }
        public string PathReturn { get; set; }
        public HandsonModel Handson { get; set; }

    }

    public class ListadoTransversalModel
    {
        public List<EprEquipoDTO> ListaConsultarEquipo { get; set; }
        public List<EqEquipoDTO> ListaEquipoCOES { get; set; }

        public List<EprEquipoDTO> ListaActualizaciones { get; set; }
        public List<EprEquipoDTO> ListaPropiedadActualizada { get; set; }

    }

    public class TransversalEditarModel
    {
        public int Id { get; set; }
        public int IdEpe { get; set; }
        public string Nombre { get; set; }
        public string FlagCambioNombre { get; set; }

    }

    public class TransversalEliminarModel
    {
        public int Estado { get; set; }
        public string Mensaje { get; set; }
    }
}