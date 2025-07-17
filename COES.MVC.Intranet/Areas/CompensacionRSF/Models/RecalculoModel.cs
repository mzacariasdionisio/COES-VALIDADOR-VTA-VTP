using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CompensacionRSF.Models
{
    public class RecalculoModel : BaseModel
    {
        //Entidades
        public VcrVersiondsrnsDTO EntidadVcrSuDeRns { get; set; }
        public VcrVersionincplDTO EntidadVcrIncumplimiento { get; set; }
        public RecalculoDTO EntidadTrnRecalculo { get; set; }
        //Lista de tablas
        public List<VcrVersiondsrnsDTO> ListaVcrSuDeRns { get; set; }
        public List<VcrVersionincplDTO> ListaIncumplimiento { get; set; }
        public List<RecalculoDTO> ListaTrnRecalculo { get; set; }
        public List<VcrRecalculoDTO> ListaRecalculoPeriodo { get; set; }
        //atributos
        public string Vcrecafeccreacion { get; set; }
        public bool bEjecutar { get; set; }
    }
}