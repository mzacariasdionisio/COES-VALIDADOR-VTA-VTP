
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Subastas;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.Subastas.Models
{
    public class ActivacionOfertaModel
    {
        public bool TienePermisoAdmin { get; set; }
        public bool TienePermisoNuevo { get; set; }
        public bool TienePermisoEditar { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public string DiaSiguiente { get; set; }
        public List<SmaMaestroMotivoDTO> ListaMotivosActivacionSubir { get; set; }
        public List<SmaMaestroMotivoDTO> ListaMotivosActivacionBajar { get; set; }
        public List<SmaActivacionOfertaDTO> HistorialActivaciones { get; set; }        
        public DatoActivacionOferta DataActivacionOferta { get; set; }
        public List<SmaIndisponibilidadTempDetDTO> ListaIndisponibilidadTemporal { get; set; }        
        public string ParamPotenciaUrsMinAuto { get; set; }
        public bool MostrarTablaIndisponibilidad { get; set; }
        public SmaIndisponibilidadTempCabDTO IndisponibilidadCab { get; set; }
        

    }
}