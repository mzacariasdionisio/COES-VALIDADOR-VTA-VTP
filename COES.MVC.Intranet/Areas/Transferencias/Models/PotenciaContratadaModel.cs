using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    // ASSETEC 2019-11
    public class PotenciaContratadaModel
    {
        public List<TrnPotenciaContratadaDTO> ListaPotenciasContratadas { get; set; }
        public TrnPotenciaContratadaDTO Entidad { get; set; }
        public int IdPotenciaContratada { get; set; }

        public string sError { get; set; }

        public List<EmpresaDTO> ListaEmpresas { get; set; }
        public List<EmpresaDTO> ListaClientes { get; set; }
        public List<BarraDTO> ListaBarrasTransferencia { get; set; }
        public List<PeriodoDTO> ListaPeriodos { get; set; }
    }
}