using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.FormatoMedicion;
namespace COES.MVC.Intranet.Areas.Evaluacion.Models
{
    public class ReactorModel
    {
        public List<EprAreaDTO> ListaUbicacion { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<EqAreaDTO> ListaArea { get; set; }
        public List<EprPropCatalogoDataDTO> ListaEstado { get; set; }
        public HandsonModel Handson { get; set; }

        public string Resultado { get; set; }
        public string StrMensaje { get; set; }
        public string Detalle { get; set; }
        public string NombreArchivo { get; set; }
        public string equicodi { get; set; }
        public string codigo { get; set; }
        public int ubicacion { get; set; }
        public int empresa { get; set; }
        public int area { get; set; }
        public string  estado { get; set; }
        public int incluirCalcular { get; set; }

    }

    public class ListadoReactorModel
    {
        public List<EprEquipoDTO> ListaReactor { get; set; }
    }

    public class ReactorEditarModel
    {
        public int Id { get; set; }
        public int IdEpe { get; set; }
        public string Nombre { get; set; }
        public string FlagCambioNombre { get; set; }

    }

    public class ReactorEliminarModel
    {
        public int Estado { get; set; }
        public string Mensaje { get; set; }
    }

    public class BuscarReactorModel
    {
        public List<EprAreaDTO> ListaUbicacion { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<EprPropCatalogoDataDTO> ListaEstado { get; set; }
    }

    public class BuscarReactorListaModel
    {
        public List<EprEquipoDTO> ListaBuscarReactor { get; set; }

    }

    public class ReactorIncluirModel
    {
        public int Id { get; set; }
        public int idProyecto { get; set; }


        public List<EprProyectoActEqpDTO> listaProyecto { get; set; }

    }

    public class ReactorIncluirModificarModel
    {
        public int IdReactor { get; set; }
        public int IdProyecto { get; set; }
        public string Fecha { get; set; }
        public string IdCelda1 { get; set; }
        public string IdCelda2 { get; set; }
        public string CapacidadMvar { get; set; }
        public string CapacidadA { get; set; }
        public string CapacidadTransmisionA { get; set; }
        public string CapacidadTransmisionAComent { get; set; }
        public string CapacidadTransmisionMvar { get; set; }
        public string CapacidadTransmisionMvarComent { get; set; }
        public string FactorLimitanteCalc { get; set; }
        public string FactorLimitanteCalcComent { get; set; }
        public string FactorLimitanteFinal { get; set; }
        public string FactorLimitanteFinalComent { get; set; }
        public string Observaciones { get; set; }
        public string UsuarioAuditoria { get; set; }

        public string Proyecto { get; set; }
        public string FechaActualizacion { get; set; }
        public string Equicodi { get; set; }
        public string IdUbicacion { get; set; }
        public string Ubicacion { get; set; }
        public string IdEmpresa { get; set; }
        public string Empresa { get; set; }
        public string Celda1PosicionNucleoTc { get; set; }
        public string Celda1PickUp { get; set; }
        public string Celda2PosicionNucleoTc { get; set; }
        public string Celda2PickUp { get; set; }
        public string NivelTension { get; set; }
        public string Accion { get; set; }
        public string Codigo{ get; set; }

        public List<EprEquipoDTO> ListaCelda { get; set; }
        public List<EprEquipoDTO> ListaCelda2 { get; set; }
        public string MotivoActualizacion { get; set; }
        public string Area { get; set; }
        public int IdEquipo { get; set; }
        public string UsuarioModificacion { get; set; }
        public string FechaModificacion { get; set; }

        public string MensajeError { get; set; }

    }

    public class ReactorEditarComentarioModel
    {
        public string Comentario { get; set; }

    }
}