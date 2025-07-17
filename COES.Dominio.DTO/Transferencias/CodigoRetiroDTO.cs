using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_CODIGO_RETIRO_SOLICITUD
    /// </summary>
    public class CodigoRetiroDTO
    {
        public string tipoPotencia { get; set; }
        public TrnPotenciaContratadaDTO ListaPotenciaTran { get; set; }
        public List<TrnPotenciaContratadaDTO> ListaPotenciaSum { get; set; }
        public int SoliCodiRetiCodiVTAParent { get; set; }
        public System.Int32 SoliCodiRetiCodi { get; set; }
        public System.Int32 EmprCodi { get; set; }
        public System.Int32 BarrCodi { get; set; }
        public System.Int32 BarrCodiSum { get; set; }
        public System.Int32 PeridcCodi { get; set; }
        public System.String PeridcEstado { get; set; }
        public System.String UsuaCodi { get; set; }
        public System.String UsuarioAgenteRegistro { get; set; }
        public System.Int32 TipoContCodi { get; set; }
        public System.Int32 TipoUsuaCodi { get; set; }
        public System.Int32 CliCodi { get; set; }
        public System.String SoliCodiRetiCodigo { get; set; }
        public System.DateTime SoliCodiRetiFechaRegistro { get; set; }
        public System.String SoliCodiRetiDescripcion { get; set; }
        public System.String SoliCodiRetiDetalleAmplio { get; set; }
        public System.DateTime? SoliCodiRetiFechaInicio { get; set; }
        public System.DateTime? SoliCodiRetiFechaFin { get; set; }
        public System.String SoliCodiRetiObservacion { get; set; }
        public System.String SoliCodiRetiEstado { get; set; }
        public System.String CoesUserName { get; set; }
        public System.String Seinusername { get; set; }
        public System.DateTime SoliCodiRetiFecIns { get; set; }
        public System.DateTime SoliCodiRetiFecAct { get; set; }
        public System.DateTime SoliCodiretiFechaSolBaja { get; set; }
        public System.DateTime SoliCodiRetiFechaBaja { get; set; }
        public System.String EmprNombre { get; set; }
        public System.String EmprAbrevia { get; set; }
        public System.String BarrNombBarrTran { get; set; }
        public System.String BarrNombBarrSum { get; set; }
        public System.Int32 CoregeCodi { get; set; }
        public System.String CoregeCodVTP { get; set; }
        public System.String TipoContNombre { get; set; }
        public System.String TipoUsuaNombre { get; set; }
        public System.String CliRuc { get; set; }
        public System.String CliNombre { get; set; }
        public System.String EstCodigo { get; set; }
        public System.String EstDescripcion { get; set; }
        public System.String EstDescripcionVTP { get; set; }
        public System.String EstAbrev { get; set; }
        public System.String EstAbrevVTP { get; set; }
        public System.Decimal? Variacion { get; set; }
        //Para la vista
        public System.String TretTabla { get; set; }
        public System.String FechaInicio { get; set; }
        public System.String FechaFin { get; set; }

        public List<CodigoRetiroDetalleDTO> ListarBarraDetalle { get; set; }
        public List<CodigoRetiroGeneradoDTO> ListarBarraSuministro { get; set; }

        // Potencia Contrato

        public int OmitirFilaVTA { get; set; }
        public int? TrnpctCodi { get; set; }
        public int? CoresoCodiPotcn { get; set; }
        public int? CoregeCodiPotcn { get; set; }

        //Es modificado por excel
        public int TrnPctExcel { get; set; }

        public int? TrnpcTipoPotencia { get; set; }
        public string TrnpcTipoCasoAgrupado { get; set; }
        public List<TrnPotenciaContratadaDTO> ListaPotenciaContratadas { get; set; }

        public int? TrnpCagrp { get; set; }
        public int? TrnpcNumordm { get; set; }
        public int? TrnpCcodiCas { get; set; }
        public string TipCasaAbrev { get; set; }
        public int? esPrimerRegistro { get; set; }
        public decimal? TrnPctTotalmwFija { get; set; }
        public decimal? TrnPctHpmwFija { get; set; }
        public decimal? TrnPctHfpmwFija { get; set; }
        public decimal? TrnPctTotalmwVariable { get; set; }
        public decimal? TrnPctHpmwFijaVariable { get; set; }
        public decimal? TrnPctHfpmwFijaVariable { get; set; }
        public string TrnPctComeObs { get; set; }
        public int Encontrado { get; set; }
        public int EsAgrupado { get; set; }
        public int IndiceGrupo { get; set; }
        public string EstApr { get; set; }
    }
}
