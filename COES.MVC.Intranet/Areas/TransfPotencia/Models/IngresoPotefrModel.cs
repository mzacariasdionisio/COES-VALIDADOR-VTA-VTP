using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Models
{
    public class IngresoPotefrModel : BaseModel
    {
        //Objetos del Modelo IngresoPotefr
        public VtpIngresoPotefrDTO Entidad { get; set; }
        public List<VtpIngresoPotefrDTO> ListaIngresoPotefr { get; set; }
        public List<PfrRecalculoDTO> ListaRecalculopfr { get; set; }

        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Resultado { get; set; }
        public List<CabeceraEscenario> ListaCabecera { get; set; }
    }

    public class CabeceraEscenario
    {
        public int intervalo { get; set; }
        public int dias { get; set; }
        public char descripcion { get; set; }
    }
}