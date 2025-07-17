using COES.Dominio.DTO.Transferencias;
using COES.MVC.Extranet.Areas.TransfPotencia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class CodigoRetiroModel: BaseModel
    {
        public List<CodigoRetiroDTO> ListaCodigoRetiro { get; set; }
        public CodigoRetiroDTO Entidad { get; set; }
        public int IdcodRetiro { get; set; }

        public string Solicodiretifechainicio { get; set; }
        public string Solicodiretifechafin { get; set; }
        public string sError { get; set; }
        
    }
}