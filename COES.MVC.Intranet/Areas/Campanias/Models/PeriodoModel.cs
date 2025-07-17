using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Campanias.Models
{
    public class PeriodoModel
    {
        public int PeriCodi { get; set; }

        public string PeriNombre { get; set; }

        public DateTime PeriFechaInicio { get; set; }
        public DateTime PeriFechaFin { get; set; } 

        public int PeriHorizonteAtras { get; set; }

        public int PeriHorizonteAdelante { get; set; }
         
        public string PeriComentario { get; set; } 
        public DateTime PeriHoraFin { get; set; }
        public string PeriEstado { get; set; }

        public List<string> PeriHojaProyecto { get; set; }

        public List<PeriodoDTO> ListaPeriodos { get; set; }
        public List<TipoProyectoDTO> ListaTipoProyecto { get; set; }

        public List<DataCatalogoDTO> ListaDataCatalogo {  get; set; }

        public PeriodoDTO PeriodoDTO { get; set; }

        public string Disabled { get; set; }

    }
}