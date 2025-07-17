using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.FormatoMedicion;
namespace COES.MVC.Intranet.Areas.Evaluacion.Models
{
    public class TransformadorModel
    {
        public List<EprAreaDTO> ListaUbicacion { get; set; }
        public List<EqFamiliaDTO> ListaTipoArea { get; set; }
        public List<List<string>> listaEquipos { get; set; }

        public List<EprAreaDTO> listaSubestacion { get; set; }
        public List<SiAreaDTO> listaArea { get; set; }
        public List<SiEmpresaDTO> listaEmpresa { get; set; }
        public int idSubestacion1 { get; set; }
        public int idSubestacion2 { get; set; }
        public int idTipoArea { get; set; }
        public int idArea { get; set; }
        public int idEmpresa { get; set; }
        public int idUbicacion { get; set; }
        public HandsonModel Handson { get; set; }
        public string estado { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<EqAreaDTO> ListaArea { get; set; }
        public List<EprPropCatalogoDataDTO> ListaEstado { get; set; }

        public List<EqFamiliaDTO> ListaTipo { get; set; }

        public string Resultado { get; set; }
        public string StrMensaje { get; set; }
        public string Detalle { get; set; }
        public string NombreArchivo { get; set; }


        public string equicodi { get; set; }
        public string codigo { get; set; }
        public int tipo { get; set; }
        public int ubicacion { get; set; }
        public int empresa { get; set; }
        public int area { get; set; }
        public string tension { get; set; }
        public int incluirCalcular { get; set; }

    }

    public class ListadoTransformadorModel
    {
        public List<EprEquipoDTO> ListaTransformador { get; set; }
    }

    public class TransformadorEditarModel
    {
        public int Id { get; set; }
        public int IdEpe { get; set; }
        public string Nombre { get; set; }
        public string FlagCambioNombre { get; set; }

    }

    public class TransformadorEliminarModel
    {
        public int Estado { get; set; }
        public string Mensaje { get; set; }
    }

    public class BuscarTransformadorListaModel
    {
        public List<EprEquipoDTO> ListaBuscarTransformador { get; set; }

    }

    public class TransformadorIncluirModel
    {
        public int Id { get; set; }
        public int idProyecto { get; set; }
        public List<EprProyectoActEqpDTO> listaProyecto { get; set; }

    }


    public class BuscarTransformadorModel
    {
        public int Id { get; set; }
        public int idProyecto { get; set; }
        public int idCelda { get; set; }
        public int idCelda2 { get; set; }
        public int idBanco { get; set; }
        public int idTitular { get; set; }
        public int idUbicacion { get; set; }
        public string estado { get; set; }

        public List<EprAreaDTO> ListaUbicacion { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<EprPropCatalogoDataDTO> ListaEstado { get; set; }
        public List<EqFamiliaDTO> ListaTipo { get; set; }
    }

    public class TransformadorIncluirModificarModel
    {
        public int IdTransformador { get; set; }
        public int IdProyecto { get; set; }
        public string Fecha { get; set; }
        public string D1IdCelda { get; set; }
        public string D1CapacidadOnanMva { get; set; }
        public string D1CapacidadOnanMvaComent { get; set; }
        public string D1CapacidadOnafMva { get; set; }
        public string D1CapacidadOnafMvaComent { get; set; }
        public string D1CapacidadMva { get; set; }
        public string D1CapacidadMvaComent { get; set; }
        public string D1CapacidadA { get; set; }
        public string D1CapacidadAComent { get; set; }
        public string D1PosicionTcA { get; set; }
        public string D1PosicionPickUpA { get; set; }
        public string D1CapacidadTransmisionA { get; set; }
        public string D1CapacidadTransmisionAComent { get; set; }
        public string D1CapacidadTransmisionMva { get; set; }
        public string D1CapacidadTransmisionMvaComent { get; set; }
        public string D1FactorLimitanteCalc { get; set; }
        public string D1FactorLimitanteCalcComent { get; set; }
        public string D1FactorLimitanteFinal { get; set; }
        public string D1FactorLimitanteFinalComent { get; set; }
        public string D2IdCelda { get; set; }
        public string D2CapacidadOnanMva { get; set; }
        public string D2CapacidadOnanMvaComent { get; set; }
        public string D2CapacidadOnafMva { get; set; }
        public string D2CapacidadOnafMvaComent { get; set; }
        public string D2CapacidadMva { get; set; }
        public string D2CapacidadMvaComent { get; set; }
        public string D2CapacidadA { get; set; }
        public string D2CapacidadAComent { get; set; }
        public string D2PosicionTcA { get; set; }
        public string D2PosicionPickUpA { get; set; }
        public string D2CapacidadTransmisionA { get; set; }
        public string D2CapacidadTransmisionAComent { get; set; }
        public string D2CapacidadTransmisionMva { get; set; }
        public string D2CapacidadTransmisionMvaComent { get; set; }
        public string D2FactorLimitanteCalc { get; set; }
        public string D2FactorLimitanteCalcComent { get; set; }
        public string D2FactorLimitanteFinal { get; set; }
        public string D2FactorLimitanteFinalComent { get; set; }
        public string D3IdCelda { get; set; }
        public string D3CapacidadOnanMva { get; set; }
        public string D3CapacidadOnanMvaComent { get; set; }
        public string D3CapacidadOnafMva { get; set; }
        public string D3CapacidadOnafMvaComent { get; set; }
        public string D3CapacidadMva { get; set; }
        public string D3CapacidadMvaComent { get; set; }
        public string D3CapacidadA { get; set; }
        public string D3CapacidadAComent { get; set; }
        public string D3PosicionTcA { get; set; }
        public string D3PosicionPickUpA { get; set; }
        public string D3CapacidadTransmisionA { get; set; }
        public string D3CapacidadTransmisionAComent { get; set; }
        public string D3CapacidadTransmisionMva { get; set; }
        public string D3CapacidadTransmisionMvaComent { get; set; }
        public string D3FactorLimitanteCalc { get; set; }
        public string D3FactorLimitanteCalcComent { get; set; }
        public string D3FactorLimitanteFinal { get; set; }
        public string D3FactorLimitanteFinalComent { get; set; }
        public string Observaciones { get; set; }
        public string UsuarioAuditoria { get; set; }
        public string Equicodi { get; set; }
        public string Accion { get; set; }

        public string Proyecto { get; set; }
        public string FechaActualizacion { get; set; }
        public string Codigo { get; set; }
        public string IdUbicacion { get; set; }
        public string Ubicacion { get; set; }
        public string IdEmpresa { get; set; }
        public string Empresa { get; set; }
        public string D4IdCelda { get; set; }
        public string D1Tension { get; set; }
        public string D2Tension { get; set; }
        public string D3Tension { get; set; }
        public string D4Tension { get; set; }
        public string D4CapacidadOnanMva { get; set; }
        public string D4CapacidadOnanMvaComent { get; set; }
        public string D4CapacidadOnafMva { get; set; }
        public string D4CapacidadOnafMvaComent { get; set; }
        public string D4CapacidadMva { get; set; }
        public string D4CapacidadMvaComent { get; set; }
        public string D4CapacidadA { get; set; }
        public string D4CapacidadAComent { get; set; }
        public string D4CapacidadTransmisionMva { get; set; }
        public string D4CapacidadTransmisionMvaComent { get; set; }
        public string D4CapacidadTransmisionA { get; set; }
        public string D4CapacidadTransmisionAComent { get; set; }
        public string D4FactorLimitanteCalc { get; set; }
        public string D4FactorLimitanteCalcComent { get; set; }
        public string D4FactorLimitanteFinal { get; set; }
        public string D4FactorLimitanteFinalComent { get; set; }
        public string D2Observaciones { get; set; }
        public string D3Observaciones { get; set; }
        public string D4Observaciones { get; set; }
        public string D4PosicionTcA { get; set; }
        public string D4PosicionPickUpA { get; set; }
        public string D1PickUp { get; set; }
        public string D2PickUp { get; set; }
        public string D3PickUp { get; set; }
        public string D4PickUp { get; set; }
        public int IdEquipo { get; set; }
        public string MotivoActualizacion { get; set; }
        public string Area { get; set; }
        public List<EprEquipoDTO> ListaCelda { get; set; }
        public string FamCodigo { get; set; }
        public string FamNombre { get; set; }
        public string UsuarioModificacion { get; set; }
        public string FechaModificacion { get; set; }

        public string MensajeError { get; set;}
    }

    public class TransformadorEditarComentarioModel
    {
        public string Comentario { get; set; }

    }

}