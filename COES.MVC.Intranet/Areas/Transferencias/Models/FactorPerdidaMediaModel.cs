using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Transferencias;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class FactorPerdidaMediaModel
    {
        public List<TrnFactorPerdidaMediaDTO> ListaFactoresPerdidaMedia { get; set; }
        public TrnFactorPerdidaMediaDTO Entidad { get; set; }
        public int Trnfpmcodi { get; set; }

        //ASSETEC 20190104
        public List<PeriodoDTO> ListaPeriodos { get; set; }
        public List<RecalculoDTO> ListaRecalculo { get; set; }
        public PeriodoDTO EntidadPeriodo { get; set; }
        public RecalculoDTO EntidadRecalculo { get; set; }

        public string sError { get; set; }
        public string sMensaje { get; set; }
        public int iNumReg { get; set; }
    }
}