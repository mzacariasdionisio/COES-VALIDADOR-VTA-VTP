using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
namespace COES.MVC.Intranet.Areas.Proteccion.Models
{
    public class EquipoProteccionModel
    {
        public string FechaVigente { get; set; }
        public List<EprProyectoActEqpDTO> listaProyecto { get; set; }
        public int estProyecto { get; set; }
        public List<EqAreaDTO> ListaArea { get; set; }
        public string Ubicacion { get; set; }
        public string Equipos { get; set; }
        public string Proyecto { get; set; }
        public List<EprPropCatalogoDataDTO> listaEstado { get; set; }


        public int equicodi { get; set; }
        public int nivel { get; set; }
        public string celda { get; set; }
        public string rele { get; set; }
        public int idArea { get; set; }
        public string nombSubestacion { get; set; }
        public string tituloRele { get; set; }
    }

    public class ListadoEquipoProteccionModelModel
    {
        public List<EprEquipoDTO> ListaEquipoProteccion { get; set; }
    }

    public class ListadoLineaTiempoModel
    {
        public List<EprEquipoDTO> ListaLineaTiempo { get; set; }
    }
    public class ItemLineaTiempoModel
    {
        public string Fecha { get; set; }
        public string Proyecto { get; set; }
        public string Cuerpo { get; set; }
        public string Estado { get; set; }
    }

    public class EquipoProteccionEditarModel
    {
        public List<EprAreaDTO> listaSubestacion { get; set; }
        public int idSubestacion { get; set; }

        public List<EprEquipoDTO> listaCelda { get; set; }
        public int idCelda { get; set; }

        public List<EprProyectoActEqpDTO> listaProyecto { get; set; }
        public int idProyecto { get; set; }

        public List<EprPropCatalogoDataDTO> listaEstado { get; set; }
        public string idEstado { get; set; }

        public List<SiEmpresaDTO> listaTitular { get; set; }
        public int idTitular { get; set; }

        public List<EprPropCatalogoDataDTO> listaSistemaRele { get; set; }
        public string idSistemaRele { get; set; }

        public List<EprPropCatalogoDataDTO> listaMarca { get; set; }
        public string idMarca { get; set; }
        public List<EprPropCatalogoDataDTO> listaTipoUso { get; set; }
        public string idTipoUso { get; set; }

        public List<EprPropCatalogoDataDTO> listaMandoSincronizado { get; set; }
        public string idMandoSincronizado { get; set; }

        public List<EprPropCatalogoDataDTO> listaReleTorcionalImpl { get; set; }
        public string idReleTorcionalImpl { get; set; }

        public List<EprPropCatalogoDataDTO> listaRelePmuImpl { get; set; }
        public string idRelePmuImpl { get; set; }
        public int equicodi { get; set; }
        public string zona { get; set; }
        public string fechaCreacion { get; set; }
        public string fechaActualizacion { get; set; }
        public string codigoRele { get; set; }
        public string fechaRele { get; set; }
        public string tensionRele { get; set; }
        public string modeloRele { get; set; }
        public string rtcp { get; set; }
        public string rtcs { get; set; }
        public string rttp { get; set; }
        public string rtts { get; set; }
        public string pCoordinables { get; set; }
        public string mCalculo { get; set; }
        public string mCalculoTexto { get; set; }
        
        public string asActivo { get; set; }

        public List<EprEquipoDTO> listaInterruptor { get; set; }
        public string asInterruptor { get; set; }

        public string asTension { get; set; }
        public string asAngulo { get; set; }
        public string asFrecuencia { get; set; }
        public string checkUmbral { get; set; }
        public string controlUmbral { get; set; }
        public string astActivo { get; set; }
        public string astU { get; set; }
        public string astT { get; set; }
        public string astUU { get; set; }
        public string astTT { get; set; }
        public string checkAsaPmu { get; set; }
        public string controlAsaPmu { get; set; }
        public string codigoInterruptor { get; set; }
        public string medidaMitigacion { get; set; }
        public string pmuAccion { get; set; }
        public string accion { get; set; }

    }

    public class CambioEstadoEditModel
    {
        public int IdEstado { get; set; }
        public int IdProyecto { get; set; }
        public string Fecha { get; set; }
        public string Motivo { get; set; }
        public List<EprProyectoActEqpDTO> listaProyecto { get; set; }
        public string MensajeError { get; set; }
    }

    public class EquipoProteccionFileModel
    {
        public string nombreArchivo { get; set; }
        public string nombreArchivoTexto { get; set; }
        
        public int estado { get; set; }
    }
}