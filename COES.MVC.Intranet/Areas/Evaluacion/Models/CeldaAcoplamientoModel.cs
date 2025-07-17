using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.FormatoMedicion;
namespace COES.MVC.Intranet.Areas.Evaluacion.Models
{
    public class CeldaAcoplamientoModel
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
        public string tension { get; set; }
        public string estado { get; set; }
        public int incluirCalcular { get; set; }

    }

    public class ListadoCeldaAcoplamientoModel
    {
        public List<EprEquipoDTO> ListaCeldaAcoplamiento { get; set; }
    }

    public class CeldaAcoplamientoEditarModel
    {
        public int Id { get; set; }
        public int IdEpe { get; set; }
        public string Nombre { get; set; }
        public string FlagCambioNombre { get; set; }

    }

    public class CeldaAcoplamientoEliminarModel
    {
        public int Estado { get; set; }
        public string Mensaje { get; set; }
    }

    public class BuscarCeldaAcoplamientoModel
    {
        public List<EprAreaDTO> ListaUbicacion { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<EprPropCatalogoDataDTO> ListaEstado { get; set; }
    }

    public class BuscarCeldaAcoplamientoListaModel
    {
        public List<EprEquipoDTO> ListaCeldaAcoplamiento { get; set; }

    }

    public class CeldaAcoplamientoIncluirModel
    {
        public int Id { get; set; }
        public int idProyecto { get; set; }


        public List<EprProyectoActEqpDTO> listaProyecto { get; set; }

    }

    public class CeldaAcoplamientoIncluirModificarModel
    {
        public int IdCeldaAcoplamiento { get; set; }
        public int IdProyecto { get; set; }
        public string Proyecto { get; set; }
        public string FechaActualizacion { get; set; }
        public string Equicodi { get; set; }
        public string Codigo { get; set; }
        public int IdUbicacion { get; set; }
        public string Ubicacion { get; set; }
        public string IdEmpresa { get; set; }
        public string Empresa { get; set; }
        public string PosicionNucleoTc { get; set; }
        public string PickUp { get; set; }
        public string IdInterruptor { get; set; }
        public string InterruptorEmpresa { get; set; }
        public string InterruptorTension { get; set; }
        public string InterruptorCapacidadA { get; set; }
        public string InterruptorCapacidadAComent { get; set; }
        public string InterruptorCapacidadMva { get; set; }
        public string InterruptorCapacidadMvaComent { get; set; }
        public string CapacidadTransmisionA { get; set; }
        public string CapacidadTransmisionAComent { get; set; }
        public string CapacidadTransmisionMva { get; set; }
        public string CapacidadTransmisionMvaComent { get; set; }
        public string FactorLimitanteCalc { get; set; }
        public string FactorLimitanteCalcComent { get; set; }
        public string FactorLimitanteFinal { get; set; }
        public string FactorLimitanteFinalComent { get; set; }
        public string Observaciones { get; set; }
        public string Accion { get; set; }

        public string Fecha { get; set; }
        public string CapacidadA { get; set; }
        public string CapacidadAComent { get; set; }
        public string CapacidadMvar { get; set; }
        public string CapacidadMvarComent { get; set; }
        public string CapacidadTransmisionMvar { get; set; }
        public string CapacidadTransmisionMvarComent { get; set; }
        public int IdCelda { get; set; }
        public int IdEquipo { get; set; }
        public string MotivoActualizacion { get; set; }
        public string Area { get; set; }
        public List<EprEquipoDTO> ListaInterruptor { get; set; }
        public string UsuarioModificacion { get; set; }
        public string FechaModificacion { get; set; }

        public string MensajeError { get; set; }
        
    }

    public class CeldaACoplamientoEditarComentarioModel
    {
        public string Comentario { get; set; }

    }
}