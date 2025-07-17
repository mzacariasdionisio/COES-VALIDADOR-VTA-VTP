using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Transferencias;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class TramiteModel
    {
        public List<TramiteDTO> ListaTramites { get; set; }
        public TramiteDTO Entidad { get; set; }
        public int IdTramite { get; set; }
        public bool bNuevo { get; set; }
        public bool bGrabar { get; set; }
        public string sError { get; set; }
    }
}