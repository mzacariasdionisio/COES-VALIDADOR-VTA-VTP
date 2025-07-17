using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Models
{
    public class RecalculoPotenciaModel : BaseModel
    {
        //Objetos del Modelo RecalculoPotencia
        public VtpRecalculoPotenciaDTO Entidad { get; set; }
        public string Recpotinterpuntames { get; set; }
        public string Recpotfechalimite { get; set; }

        //Objetos complementarios al Modelo
        public List<RecalculoDTO> ListaRecalculo { get; set; }
    }
}