using COES.Dominio.DTO.Transferencias;
using System.Collections.Generic;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
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