using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System.Collections.Generic;

namespace COES.MVC.Extranet.Areas.ValorizacionDiaria.Models
{
    public class ConsultasModel
    {
        public List<VtdMontoPorEnergiaDTO> MontoPorEnergia { get; set; }
        public List<VtdMontoPorCapacidadDTO> MontoPorCapacidad { get; set; }
        public List<VtdMontoPorPeajeDTO> MontoPorPeaje { get; set; }
        public List<VtdMontoSCeIODTO> MontoSCeIO { get; set; }
        public List<VtdMontoPorExcesoDTO> MontoPorExceso { get; set; }
        public List<VtdLogProcesoDTO> LogProceso { get; set; }
        public List<SiEmpresaDTO> Empresa { get; set; }
        public List<ValorizacionDiariaDTO> ValorizacionDiaria { get; set; }
        public List<ValorizacionDiariaDTO> ValorizacionDiariaMes { get; set; }

        //Reporte Informacion Prevista Remitida al participante
        public List<MeMedicion96DTO> InformacionPrevista { get; set; }

        public List<VtdCargoEneReacDTO> EnergiaReactiva { get; set; }

        public decimal montoCapacidad { get; set; }
        public decimal montoPeaje { get; set; }
    }


    public class PaginadoModel
    {
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
    }
}