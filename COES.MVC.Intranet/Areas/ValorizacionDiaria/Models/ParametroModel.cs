using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ValorizacionDiaria.Models
{
    public class ParametroModel 
    {           
        public List<PrGrupodatDTO> MargenReserva { get; set; }

        public List<PrGrupodatDTO> FactorRepartoLunes { get; set; }
        public List<PrGrupodatDTO> FactorRepartoMartes { get; set; }
        public List<PrGrupodatDTO> FactorRepartoMiercoles { get; set; }
        public List<PrGrupodatDTO> FactorRepartoJueves { get; set; }
        public List<PrGrupodatDTO> FactorRepartoViernes { get; set; }
        public List<PrGrupodatDTO> FactorRepartoSabado { get; set; }
        public List<PrGrupodatDTO> FactorRepartoDomingo { get; set; }        
        public List<PrGrupodatDTO> PorcentajePerdida { get; set; }
        public List<PrGrupodatDTO> CostoOportunidad { get; set; }        
        public List<PrGrupodatDTO> CargoPorConsumo { get; set; }
        public List<PrGrupodatDTO> CostoFueraBanda { get; set; }
        public List<PrGrupodatDTO> CostosOtrosEquipos { get; set; }
        public List<PrGrupodatDTO> FRECTotal { get; set; }
        

        public List<VtdCargoEneReacDTO> EnergiaReactiva { get; set; }
        public List<SiEmpresaDTO> Empresas { get; set; }
        public string HoraEjecucion { get; set; }
        /// <summary>
        /// Tabla Vtd_Cargosenereac
        /// Caermonto => Monto
        /// </summary>
        //public int Monto { get; set; }

    }
    
}
