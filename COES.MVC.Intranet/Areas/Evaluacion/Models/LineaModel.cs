using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.FormatoMedicion;
namespace COES.MVC.Intranet.Areas.Evaluacion.Models
{
    public class LineaModel
    {
        public List<EprAreaDTO> ListaUbicacion { get; set; }
        public List<EqFamiliaDTO> ListaTipoArea { get; set; }
        public List<List<string>> listaEquipos { get; set; }
        public List<EprAreaDTO> listaSubestacion { get; set; }
        public List<SiAreaDTO> listaArea { get; set; }
        public List<SiEmpresaDTO> listaEmpresa { get; set; }
        public int idSubestacion1 { get; set; }
        public int idSubestacion2 { get; set; }
        public int idArea { get; set; }
        public int idEmpresa { get; set; }
        public HandsonModel Handson { get; set; }

        public string estado { get; set; }
        public List<EprPropCatalogoDataDTO> listaEstado { get; set; }
        public List<EqAreaDTO> ListaArea { get; set; }

        public string Resultado { get; set; }
        public string StrMensaje { get; set; }
        public string Detalle { get; set; }
        public string NombreArchivo { get; set; }
        public string equicodi { get; set; }
        public string codigo { get; set; }
        public string tension { get; set; }
        public int incluirCalcular { get; set; }
        public int subestacion1 { get; set; }
        public int subestacion2 { get; set; }
        public int area { get; set; }
        public int empresa { get; set; }

    }

    public class ListadoLineaModel
    {
        public List<EprEquipoDTO> listaLineaPrincipal { get; set; }
    }


    public class BuscarLineaListaModel
    {
        public List<EqEquipoDTO> ListaLinea { get; set; }

    }

    public class EditarComentarioModel
    {
        public string Comentario { get; set; }

    }
    public class LineaEditarModel
    {
        public int Id { get; set; }
        public int IdEpe { get; set; }
        public string Nombre { get; set; }
        public string FlagCambioNombre { get; set; }


    }

    public class LineaIncluirModel
    {
        public int Id { get; set; }
        public int idProyecto { get; set; }


        public List<EprProyectoActEqpDTO> listaProyecto { get; set; }

    }

    public class LineaIncluirModificarModel
    {
        public string Motivo { get; set; }
        public string FechaMotivo { get; set; }

        public string Equicodi { get; set; }
        public string Codigo { get; set; }
        public string Ubicacion { get; set; }
        public string Empresa { get; set; }
        public string Longitud { get; set; }
        public string Tension { get; set; }

        public int IdProyecto { get; set; }
        public int IdLinea { get; set; }
        public string CapacidadA { get; set; }
        public string CapacidadMva { get; set; }
        public int IdArea { get; set; }
        public int IdCelda { get; set; }

        public string Celda1Posicion { get; set; }
        public string Celda1PickUp { get; set; }

        public int IdCelda2 { get; set; }
        public string Celda2Posicion { get; set; }
        public string Celda2PickUp { get; set; }
        public int IdBancoCondensador { get; set; }
        public string CapacTransCond1Porcen { get; set; }
        public string CapacTransCond1Min { get; set; }
        public string CapacTransCond1A { get; set; }
        public string CapacTransCond2Porcen { get; set; }
        public string CapacTransCond2Min { get; set; }
        public string CapacTransCond2A { get; set; }
        public string CapacidadTransmisionA { get; set; }
        public string CapacidadTransmisionMva { get; set; }
        public string LimiteSegCoes { get; set; }
        public string FactorLimitanteCalc { get; set; }
        public string FactorLimitanteFinal { get; set; }
        public string Observaciones { get; set; }

        public string ActualizadoPor { get; set; }
        public string ActualizadoEl { get; set; }

        public string accion { get; set; }

        public List<EprProyectoActEqpDTO> listaProyecto { get; set; }
        public List<EprAreaDTO> listaArea { get; set; }
        public List<EprEquipoDTO> listaCelda { get; set; }
        public List<EprEquipoDTO> listaCelda2 { get; set; }
        public List<EprEquipoDTO> listaBanco { get; set; }
        public int IdEquipo { get; set; }
        public string UsuarioModificacion { get; set; }
        public string FechaModificacion { get; set; }

        public string MensajeError { get; set; }
        public string CapacidadABancoCondensador { get; set; }
        public string CapacidadMvarBancoCondensador { get; set; }
        public string CapacidadABancoCondensadorComent { get; set; }
        public string CapacidadMvarBancoCondensadorComent { get; set; }
        public string CapacidadAComent { get; set; }
        public string CapacidadMvaComent { get; set; }
        
        public string CapacTransCond1PorcenComent { get; set; }
        public string CapacTransCond1MinComent { get; set; }
        
        public string CapacTransCond1AComent { get; set; }
        public string CapacTransCond2PorcenComent { get; set; }
        
        public string CapacTransCond2MinComent { get; set; }
        public string CapacTransCond2AComent { get; set; }
        
        public string CapacidadTransmisionAComent { get; set; }
        public string CapacidadTransmisionMvaComent { get; set; }

        
        public string LimiteSegCoesComent { get; set; }
        public string FactorLimitanteCalcComent { get; set; }
        public string FactorLimitanteFinalComent { get; set; }


    }

    public class BuscarLineaModel
    {
        public int Id { get; set; }
        public int idProyecto { get; set; }
        public int idCelda { get; set; }
        public int idCelda2 { get; set; }
        public int idBanco { get; set; }
        public List<SiEmpresaDTO> listaTitular { get; set; }
        public int idTitular { get; set; }
        public List<EprAreaDTO> listaUbicacion { get; set; }
        public int idUbicacion { get; set; }
        public List<EprProyectoActEqpDTO> listaProyecto { get; set; }
        public string estado { get; set; }
        public List<EprPropCatalogoDataDTO> listaEstado { get; set; }
    }

    public class LineaEliminarModel
    {
        public int Estado { get; set; }
        public string Mensaje { get; set; }
    }  
}