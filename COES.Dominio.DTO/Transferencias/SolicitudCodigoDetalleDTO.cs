using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Cambios 2020/12/30
    /// Clase que mapea la tabla TRN_CODIGO_RETIRO_SOL_DET
    /// </summary>
    public class SolicitudCodigoDetalleDTO
    {
        public int Coresdcodi { get; set; }
        public int indexBarra { get; set; }
        public string IdGenerado { get; set; }
        public int Coresocodi { get; set; }
        public int Barracoditra { get; set; }
        public int Barracodisum { get; set; }
        public int Coresdnumregistro { get; set; }

        public string Coresdusuarioregistro { get; set; }
        public DateTime Coresdfecharegistro { get; set; }

        //para la vista
        public int Coregecodigo { get; set; }
        public string coregeestado { get; set; }
        public string Barranomsum { get; set; }
        public string Codigovta { get; set; }
        public string Codigovtp { get; set; }
        public int BarraRelacionCodi { get; set; }
        public int Indice { get; set; }

        public int OmitirFila { get; set; }
      

        public int? TrnpctCodi { get; set; }
        public int? CoresoCodiPotcn { get; set; }
        public int? CoregeCodiPotcn { get; set; }
        public int? TrnpCagrp { get; set; }
        public int? TrnpcNumordm { get; set; }
        public int? TrnpCcodiCas { get; set; }
        public string TipCasaAbrev { get; set; }

        public decimal? TrnPctTotalmwFija { get; set; }
        public decimal? TrnPctHpmwFija { get; set; }
        public decimal? TrnPcthFpmwFija { get; set; }
        public decimal? TrnPctTotalmwVariable { get; set; }
        public decimal? TrnPctHpmwFijaVariable { get; set; }
        public decimal? TrnPctHfpmwFijaVariable { get; set; }
        public string TrnPctComeObs{ get; set; }

        //Es modificado por excel
        public int TrnPctTotalmwFijaExcel { get; set; }
        public int TrnPctHpmwFijaExcel { get; set; }
        public int TrnPcthFpmwFijaExcel { get; set; }
        public int TrnPctTotalmwVariableExcel { get; set; }
        public int TrnPctHpmwFijaVariableExcel { get; set; }
        public int TrnPctHfpmwFijaVariableExcel { get; set; }
        public int TrnPctComeObsExcel { get; set; }


        public SolicitudCodigoPotenciaContratadaDTO PotenciaContratadaDTO { get; set; }
    }
}
