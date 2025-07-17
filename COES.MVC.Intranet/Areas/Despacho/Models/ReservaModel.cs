using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.Despacho.Models
{
    public class ReservaModel
    {
        public int IdReserva { get; set; }
        public PrReservaDTO Entidad { get; set; }
        public Datos48Reserva Datos48Reserva { get; set; }
        public List<PrCategoriaDTO> ListaCategoria { get; set; }
        public List<PrReservaDTO> ListaReserva { get; set; }
        public string FechaConsulta { get; set; }
        public string FechaVigenciaActual { get; set; }

        public string Resultado { get; set; }
        public string StrMensaje { get; set; }
        public string Detalle { get; set; }
        public bool TienePermiso { get; set; }

        public bool AccionNuevo { get; set; }
        public bool AccionEditar { get; set; }

        public string NombreArchivo { get; set; }
        public List<PrReservaDTO> ListaReservaErrores { get; set; }
        public List<PrReservaDTO> ListaReservaCorrectos { get; set; }
    }
}