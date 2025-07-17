using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Cambios 2020/12/30
    /// Clase que mapea la tabla TRN_CODIGO_RETIRO_SOLICITUD 
    /// </summary>
    public class SolicitudCodigoDTO : IComparable<SolicitudCodigoDTO>
    {
        public int PeridcCodi { get; set; }
        public string PeridcEstado { get; set; }
        public string PeridcNombre { get; set; }
        public int SoliCodiRetiCodiVTAParent { get; set; }
        public int SoliCodiRetiCodi { get; set; }
        public int EmprCodi { get; set; }
        public int BarrCodi { get; set; }
        public string UsuaCodi { get; set; }
        public int TipoContCodi { get; set; }
        public int TipoUsuaCodi { get; set; }
        public int CliCodi { get; set; }
        public string SoliCodiRetiCodigo { get; set; }

        public System.DateTime SoliCodiRetiFechaRegistro { get; set; }
        public string SoliCodiRetiDescripcion { get; set; }
        public string SoliCodiRetiDetalleAmplio { get; set; }
        public System.DateTime? SoliCodiRetiFechaInicio { get; set; }
        public System.DateTime? SoliCodiRetiFechaFin { get; set; }
        public System.DateTime? SoliCodiRetiFechaInicioValida { get; set; }
        public string SoliCodiRetiObservacion { get; set; }
        public string SoliCodiRetiEstado { get; set; }
        public string CoesUserName { get; set; }
        //public string Seinusername { get; set; }
        public System.DateTime SoliCodiRetiFecIns { get; set; }
        public System.DateTime SoliCodiRetiFecAct { get; set; }
        public System.DateTime SoliCodiretiFechaSolBaja { get; set; }
        public System.DateTime SoliCodiRetiFechaBaja { get; set; }
        public string EmprNombre { get; set; }
        public string BarrNombBarrTran { get; set; }
        public string TipoContNombre { get; set; }
        public string TipoUsuaNombre { get; set; }
        public string CliNombre { get; set; }
        public int Codretgencodi { get; set; }
        //Para la vista
        public string TretTabla { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }

        public string BarraNomBarrSum { get; set; }
        public string SoliCodiRetiCodigoVTP { get; set; }

        public string SoliCodiRetiEstadoVTP { get; set; }

        public string EstadoDescripcionVTP { get; set; }

        public string EstadoDescripcionVTEA { get; set; }
        public string Ruc { get; set; }
        public int BarrCodiSum { get; set; }
        // Potencia Contrato

        public int? esPrimerRegistro { get; set; }
        public int? CoresoCodi { get; set; }
        public int? TrnpctCodi { get; set; }
        public int? CoresoCodiPotcn { get; set; }
        public int? CoregeCodiPotcn { get; set; }
        public int? TrnpCagrp { get; set; }
        public int? TrnpCagrpVTA { get; set; }
        public int? TrnpCagrpVTP { get; set; }
        public int? TrnpcNumordm { get; set; }
        public int? TrnpcNumordmVTA { get; set; }
        public int? TrnpcNumordmVTP { get; set; }
        public int? TrnpCcodiCas { get; set; }
        public string TipCasaAbrev { get; set; }

        public decimal? TrnPctTotalmwFija { get; set; }
        public decimal? TrnPctHpmwFija { get; set; }
        public decimal? TrnPctHfpmwFija { get; set; }
        public decimal? TrnPctTotalmwVariable { get; set; }
        public decimal? TrnPctHpmwFijaVariable { get; set; }
        public decimal? TrnPctHfpmwFijaVariable { get; set; }
        public string TrnPctComeObs { get; set; }
        //Es modificado por excel
        public int TrnPctExcel { get; set; }

        public int? TrnpcTipoPotencia { get; set; }
        public string TrnpcTipoCasoAgrupado { get; set; }
        public string abrevEstadoVTA { get; set; }
        public string abrevEstadoVTP { get; set; }
        public List<SolicitudCodigoDetalleDTO> ListaCodigoRetiroDetalle { get; set; }
        public List<TrnPotenciaContratadaDTO> ListaPotenciaContratadas { get; set; }
        public int OmitirFilaVTA { get; set; }
        public int Encontrado { get; set; }
        public int NroRegDetalle { get; set; }
        public string SoliCodiRetiUsuRegistro { get; set; }
        public string UserEmail { get; set; }
        //  potcn.trnpcagrp
        //,trn_codigo_retiro_solicitud.coresocodi desc
        //, empresa.emprnomb
        //,cliente.emprnomb
        //,trn_barra.barrbarratransferencia
        //,potcngn.trnpcagrp
        //,trnpcnumordm
        //,generado.coregecodvtp
        public int CompareTo(SolicitudCodigoDTO other)
        {
            /*
          order by
   potcn.trnpcagrp
   ,trn_codigo_retiro_solicitud.coresocodi desc
   ,empresa.emprnomb
   ,cliente.emprnomb
   ,trn_barra.barrbarratransferencia
   ,potcngn.trnpcagrp
   ,trnpcnumordm
   ,generado.coregecodvtp

          */

            // Devuelve:
            //     Valor Significado Menor
            //     que cero Esta instancia precede a other en el criterio de ordenación. Cero Esta
            //     instancia se produce en la misma posición del criterio de ordenación que other.
            //     Mayor que cero Esta instancia sigue a other en el criterio de ordenación.

            int resultado = 0;
            int comparar = (this.TrnpCagrp ?? 0);
            int compararOtro = (other.TrnpCagrp) ?? 0;

            if (other.TrnpCagrp > comparar
                && other.CoresoCodiPotcn >= this.CoresoCodiPotcn

                )
                return 1;
            else if (comparar < compararOtro)
                return -1;
            else
                return -1;

        }

        public int EsAgrupado { get; set; }
        public int IndiceGrupo { get; set; }
    }

    public class SolicitudCodigoAuxiliarDTO
    {
        public int PeridcCodi { get; set; }
        public string PeridcNombre { get; set; }
        public int SoliCodiRetiCodiVTAParent { get; set; }
        public int SoliCodiRetiCodi { get; set; }
        public int EmprCodi { get; set; }
        public int BarrCodi { get; set; }
        public string UsuaCodi { get; set; }
        public int TipoContCodi { get; set; }
        public int TipoUsuaCodi { get; set; }
        public int CliCodi { get; set; }
        public string SoliCodiRetiCodigo { get; set; }

        public System.DateTime SoliCodiRetiFechaRegistro { get; set; }
        public string SoliCodiRetiDescripcion { get; set; }
        public string SoliCodiRetiDetalleAmplio { get; set; }
        public System.DateTime? SoliCodiRetiFechaInicio { get; set; }
        public System.DateTime? SoliCodiRetiFechaFin { get; set; }
        public string SoliCodiRetiObservacion { get; set; }
        public string SoliCodiRetiEstado { get; set; }
        public string CoesUserName { get; set; }
        //public string Seinusername { get; set; }
        public System.DateTime SoliCodiRetiFecIns { get; set; }
        public System.DateTime SoliCodiRetiFecAct { get; set; }
        public System.DateTime SoliCodiretiFechaSolBaja { get; set; }
        public System.DateTime SoliCodiRetiFechaBaja { get; set; }
        public string EmprNombre { get; set; }
        public string BarrNombBarrTran { get; set; }
        public string TipoContNombre { get; set; }
        public string TipoUsuaNombre { get; set; }
        public string CliNombre { get; set; }
        public int Codretgencodi { get; set; }
        //Para la vista
        public string TretTabla { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }

        public string BarraNomBarrSum { get; set; }
        public string SoliCodiRetiCodigoVTP { get; set; }

        public string SoliCodiRetiEstadoVTP { get; set; }

        public string EstadoDescripcionVTP { get; set; }

        public string EstadoDescripcionVTEA { get; set; }
        public string Ruc { get; set; }
        public int BarrCodiSum { get; set; }
        // Potencia Contrato

        public int? TrnpctCodi { get; set; }
        public int? CoresoCodiPotcn { get; set; }
        public int? CoregeCodiPotcn { get; set; }
        public int? TrnpCagrp { get; set; }
        public int? TrnpCagrpVTA { get; set; }
        public int? TrnpCagrpVTP { get; set; }
        public int? TrnpcNumordm { get; set; }
        public int? TrnpcNumordmVTA { get; set; }
        public int? TrnpcNumordmVTP { get; set; }
        public int? TrnpCcodiCas { get; set; }
        public string TipCasaAbrev { get; set; }

        public decimal? TrnPctTotalmwFija { get; set; }
        public decimal? TrnPctHpmwFija { get; set; }
        public decimal? TrnPctHfpmwFija { get; set; }
        public decimal? TrnPctTotalmwVariable { get; set; }
        public decimal? TrnPctHpmwFijaVariable { get; set; }
        public decimal? TrnPctHfpmwFijaVariable { get; set; }
        public string TrnPctComeObs { get; set; }
        //Es modificado por excel
        public int TrnPctExcel { get; set; }

        public int? TrnpcTipoPotencia { get; set; }
        public string TrnpcTipoCasoAgrupado { get; set; }
        public string abrevEstadoVTA { get; set; }
        public string abrevEstadoVTP { get; set; } 
        public int OmitirFilaVTA { get; set; }
        public int Encontrado { get; set; }
        public int NroRegDetalle { get; set; }
        public string SoliCodiRetiUsuRegistro { get; set; }
    
    
    }
}
