using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    // ASSETEC 2019-11
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TRN_PONTENCIA_CONTRATADA
    /// </summary>
    public class TrnPotenciaContratadaHelper : HelperBase
    {
        #region Constructor
        public TrnPotenciaContratadaHelper() : base(Consultas.TrnPotenciaContratadaSql)
        {

        }
        //dasaji-210212
        private bool columnsExist(string columnName, IDataReader dr)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {

                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        //dasaji-210212
        private object valorReturn(IDataReader dr, string sColumna)
        {
            object resultado = null;
            int iIndex;
            if (columnsExist(sColumna, dr))
            {
                iIndex = dr.GetOrdinal(sColumna);
                if (!dr.IsDBNull(iIndex)) resultado = dr.GetValue(iIndex);
            }
            return resultado;
        }
        public static T? ConvertToNull<T>(object x) where T : struct
        {
            return x == null ? null : (T?)Convert.ChangeType(x, typeof(T));
        }
        #endregion

        #region Helpers
        public TrnPotenciaContratadaDTO Create(IDataReader dr)
        {
            TrnPotenciaContratadaDTO entity = new TrnPotenciaContratadaDTO();

            #region Campos Originales
            // TRNPCTCODI
            int iTrnPctCodi = dr.GetOrdinal(this.TrnPctCodi);
            if (!dr.IsDBNull(iTrnPctCodi)) entity.TrnPctCodi = dr.GetInt32(iTrnPctCodi);

            // PERICODI

            int iPeriCodi = dr.GetOrdinal(this.PeriCodi);
            if (!dr.IsDBNull(iPeriCodi)) entity.PeriCodi = dr.GetInt32(iPeriCodi);

            // EMPRCODI
            int iEmprCodi = dr.GetOrdinal(this.EmprCodi);
            if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = dr.GetInt32(iEmprCodi);

            // CORESOCODI
            int iCoresoCodi = dr.GetOrdinal(this.CoresoCodi);
            if (!dr.IsDBNull(iCoresoCodi)) entity.CoresoCodi = dr.GetInt32(iCoresoCodi);

            // TRNPCTPTOSUMINS
            int iTrnPctPtoSumins = dr.GetOrdinal(this.TrnPctPtoSumins);
            if (!dr.IsDBNull(iTrnPctPtoSumins)) entity.TrnPctPtoSumins = dr.GetString(iTrnPctPtoSumins);

            // TRNPCTTOTALMWFIJA
            int iTrnPctTotalMwFija = dr.GetOrdinal(this.TrnPctTotalMwFija);
            if (!dr.IsDBNull(iTrnPctTotalMwFija)) entity.TrnPctTotalMwFija = dr.GetDecimal(iTrnPctTotalMwFija);

            // TRNPCTHPMWFIJA
            int iTrnPctHpMwFija = dr.GetOrdinal(this.TrnPctHpMwFija);
            if (!dr.IsDBNull(iTrnPctHpMwFija)) entity.TrnPctHpMwFija = dr.GetDecimal(iTrnPctHpMwFija);

            // TRNPCTHFPMWFIJA
            int iTrnPctHfpMwFija = dr.GetOrdinal(this.TrnPctHfpMwFija);
            if (!dr.IsDBNull(iTrnPctHfpMwFija)) entity.TrnPctHfpMwFija = dr.GetDecimal(iTrnPctHfpMwFija);

            // TRNPCTTOTALMWVARIABLE
            int iTrnPctTotalMwVariable = dr.GetOrdinal(this.TrnPctTotalMwVariable);
            if (!dr.IsDBNull(iTrnPctTotalMwVariable)) entity.TrnPctTotalMwVariable = dr.GetDecimal(iTrnPctTotalMwVariable);

            // TRNPCTHPMWFIJAVARIABLE
            int iTrnPctHpMwFijaVariable = dr.GetOrdinal(this.TrnPctHpMwFijaVariable);
            if (!dr.IsDBNull(iTrnPctHpMwFijaVariable)) entity.TrnPctHpMwFijaVariable = dr.GetDecimal(iTrnPctHpMwFijaVariable);

            // TRNPCTHFPMWFIJAVARIABLE
            int iTrnPctHfpMwFijaVariable = dr.GetOrdinal(this.TrnPctHfpMwFijaVariable);
            if (!dr.IsDBNull(iTrnPctHfpMwFijaVariable)) entity.TrnPctHfpMwFijaVariable = dr.GetDecimal(iTrnPctHfpMwFijaVariable);

            // TRNPCTCOMEOBS
            int iTrnPctComeObs = dr.GetOrdinal(this.TrnPctComeObs);
            if (!dr.IsDBNull(iTrnPctComeObs)) entity.TrnPctComeObs = dr.GetString(iTrnPctComeObs);

            // TRNPCTUSERNAMEINS
            int iTrnPctUserNameIns = dr.GetOrdinal(this.TrnPctUserNameIns);
            if (!dr.IsDBNull(iTrnPctUserNameIns)) entity.TrnPctUserNameIns = dr.GetString(iTrnPctUserNameIns);

            // TRNPCTFECINS
            int iTrnPctFecIns = dr.GetOrdinal(this.TrnPctFecIns);
            if (!dr.IsDBNull(iTrnPctFecIns)) entity.TrnPctFecIns = dr.GetDateTime(iTrnPctFecIns);
            #endregion






            #region Campos Agregados

            #endregion Campos Agregados

            return entity;
        }

        public TrnPotenciaContratadaDTO CreatePotenciasContratadas(IDataReader dr)
        {
            TrnPotenciaContratadaDTO entity = new TrnPotenciaContratadaDTO();

            #region Campos Originales
            // TRNPCTCODI
            int iTrnPctCodi = dr.GetOrdinal(this.TrnPctCodi);
            if (!dr.IsDBNull(iTrnPctCodi)) entity.TrnPctCodi = dr.GetInt32(iTrnPctCodi);

            // PERICODI
            int iPeriCodi = dr.GetOrdinal(this.PeriCodi);
            if (!dr.IsDBNull(iPeriCodi)) entity.PeriCodi = dr.GetInt32(iPeriCodi);

            // EMPRCODI
            int iEmprCodi = dr.GetOrdinal(this.EmprCodi);
            if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = dr.GetInt32(iEmprCodi);

            // CORESOCODI
            int iCoresoCodi = dr.GetOrdinal(this.CoresoCodi);
            if (!dr.IsDBNull(iCoresoCodi)) entity.CoresoCodi = dr.GetInt32(iCoresoCodi);

            // TRNPCTPTOSUMINS
            int iTrnPctPtoSumins = dr.GetOrdinal(this.TrnPctPtoSumins);
            if (!dr.IsDBNull(iTrnPctPtoSumins)) entity.TrnPctPtoSumins = dr.GetString(iTrnPctPtoSumins);

            // TRNPCTTOTALMWFIJA
            int iTrnPctTotalMwFija = dr.GetOrdinal(this.TrnPctTotalMwFija);
            if (!dr.IsDBNull(iTrnPctTotalMwFija)) entity.TrnPctTotalMwFija = dr.GetDecimal(iTrnPctTotalMwFija);

            // TRNPCTHPMWFIJA
            int iTrnPctHpMwFija = dr.GetOrdinal(this.TrnPctHpMwFija);
            if (!dr.IsDBNull(iTrnPctHpMwFija)) entity.TrnPctHpMwFija = dr.GetDecimal(iTrnPctHpMwFija);

            // TRNPCTHFPMWFIJA
            int iTrnPctHfpMwFija = dr.GetOrdinal(this.TrnPctHfpMwFija);
            if (!dr.IsDBNull(iTrnPctHfpMwFija)) entity.TrnPctHfpMwFija = dr.GetDecimal(iTrnPctHfpMwFija);

            // TRNPCTTOTALMWVARIABLE
            int iTrnPctTotalMwVariable = dr.GetOrdinal(this.TrnPctTotalMwVariable);
            if (!dr.IsDBNull(iTrnPctTotalMwVariable)) entity.TrnPctTotalMwVariable = dr.GetDecimal(iTrnPctTotalMwVariable);

            // TRNPCTHPMWFIJAVARIABLE
            int iTrnPctHpMwFijaVariable = dr.GetOrdinal(this.TrnPctHpMwFijaVariable);
            if (!dr.IsDBNull(iTrnPctHpMwFijaVariable)) entity.TrnPctHpMwFijaVariable = dr.GetDecimal(iTrnPctHpMwFijaVariable);

            // TRNPCTHFPMWFIJAVARIABLE
            int iTrnPctHfpMwFijaVariable = dr.GetOrdinal(this.TrnPctHfpMwFijaVariable);
            if (!dr.IsDBNull(iTrnPctHfpMwFijaVariable)) entity.TrnPctHfpMwFijaVariable = dr.GetDecimal(iTrnPctHfpMwFijaVariable);

            // TRNPCTCOMEOBS
            int iTrnPctComeObs = dr.GetOrdinal(this.TrnPctComeObs);
            if (!dr.IsDBNull(iTrnPctComeObs)) entity.TrnPctComeObs = dr.GetString(iTrnPctComeObs);



            #endregion

            #region Campos Adicionales
            // CORESOCODIGO
            int iCoresoCodigo = dr.GetOrdinal(this.CoresoCodigo);
            if (!dr.IsDBNull(iCoresoCodigo)) entity.CoresoCodigo = dr.GetString(iCoresoCodigo);

            // GENEMPRCODI
            int iGenEmprCodi = dr.GetOrdinal(this.EmprCodi);
            if (!dr.IsDBNull(iGenEmprCodi)) entity.EmprCodi = dr.GetInt32(iGenEmprCodi);

            // CLIEMPRCODI
            int iCliEmprCodi = dr.GetOrdinal(this.CliEmprCodi);
            if (!dr.IsDBNull(iCliEmprCodi)) entity.CliEmprCodi = dr.GetInt32(iCliEmprCodi);

            // TIPCONCODI
            int iTipConCodi = dr.GetOrdinal(this.TipConCodi);
            if (!dr.IsDBNull(iTipConCodi)) entity.TipConCodi = dr.GetInt32(iTipConCodi);

            // TIPUSUCODI
            int iTipUsuCodi = dr.GetOrdinal(this.TipUsuCodi);
            if (!dr.IsDBNull(iTipUsuCodi)) entity.TipUsuCodi = dr.GetInt32(iTipUsuCodi);

            // BARRCODI
            int iBarrCodi = dr.GetOrdinal(this.BarrCodi);
            if (!dr.IsDBNull(iBarrCodi)) entity.BarrCodi = dr.GetInt32(iBarrCodi);

            // CORESOFECHAINICIO
            int iCoresoFechaInicio = dr.GetOrdinal(this.CoresoFechaInicio);
            if (!dr.IsDBNull(iCoresoFechaInicio)) entity.CoresoFechaInicio = dr.GetDateTime(iCoresoFechaInicio);

            // CORESOFECHAFIN
            int iCoresoFechaFin = dr.GetOrdinal(this.CoresoFechaFin);
            if (!dr.IsDBNull(iCoresoFechaFin)) entity.CoresoFechaFin = dr.GetDateTime(iCoresoFechaFin);

            // EMPRNOMB (EMPRNOMB)
            int iEmprNomb = dr.GetOrdinal(this.EmprNomb);
            if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

            // CLINOMBRE
            int iCliNombre = dr.GetOrdinal(this.CliNombre);
            if (!dr.IsDBNull(iCliNombre)) entity.CliNombre = dr.GetString(iCliNombre);

            // BARRBARRATRANSFERENCIA
            int iBarrBarraTransferencia = dr.GetOrdinal(this.BarrBarraTransferencia);
            if (!dr.IsDBNull(iBarrBarraTransferencia)) entity.BarrBarraTransferencia = dr.GetString(iBarrBarraTransferencia);

            // TIPCONNOMBRE
            int iTipConNombre = dr.GetOrdinal(this.TipConNombre);
            if (!dr.IsDBNull(iTipConNombre)) entity.TipConNombre = dr.GetString(iTipConNombre);

            // // TIPUSUNOMBRE
            int iTipUsuNombre = dr.GetOrdinal(this.TipUsuNombre);
            if (!dr.IsDBNull(iTipUsuNombre)) entity.TipUsuNombre = dr.GetString(iTipUsuNombre);

            // PERINOMBRE
            int iPeriNombre = dr.GetOrdinal(this.PeriNombre);
            if (!dr.IsDBNull(iPeriNombre)) entity.PeriNombre = dr.GetString(iPeriNombre);

            // TRNPCTUSERNAMEINS
            int iTrnPctUserNameIns = dr.GetOrdinal(this.TrnPctUserNameIns);
            if (!dr.IsDBNull(iTrnPctUserNameIns)) entity.TrnPctUserNameIns = dr.GetString(iTrnPctUserNameIns);

            // TRNPCTFECINS
            int iTrnPctFecIns = dr.GetOrdinal(this.TrnPctFecIns);
            if (!dr.IsDBNull(iTrnPctFecIns)) entity.TrnPctFecIns = dr.GetDateTime(iTrnPctFecIns);


            #endregion


            return entity;
        }


        public TrnPotenciaContratadaDTO CreatePotenciasContratadasEnvio(IDataReader dr)
        {
            TrnPotenciaContratadaDTO entity = new TrnPotenciaContratadaDTO();
            int iTrnPcEnvCodi = dr.GetOrdinal(this.TrnPcEnvCodi);
            if (!dr.IsDBNull(iTrnPcEnvCodi)) entity.TrnPcEnvCodi = dr.GetInt32(iTrnPcEnvCodi);

            int iTrnPcEnvUsuario = dr.GetOrdinal(this.TrnPcEnvUsuario);
            if (!dr.IsDBNull(iTrnPcEnvUsuario)) entity.TrnPcEnvUsuario = dr.GetString(iTrnPcEnvUsuario);

            int iTrnPcEnvFechaRe = dr.GetOrdinal(this.TrnPcEnvFechaRe);
            if (!dr.IsDBNull(iTrnPcEnvFechaRe)) entity.TrnPcEnvFechaRe = dr.GetDateTime(iTrnPcEnvFechaRe);


            return entity;

        }

        public TrnPotenciaContratadaDTO CreatePotenciasContratosLista(IDataReader dr)
        {
            TrnPotenciaContratadaDTO entity = new TrnPotenciaContratadaDTO();
            //potencia contrato
            entity.BarrCodi = ConvertToNull<int>(valorReturn(dr, BarrCodi));
            entity.BarrSuministro = valorReturn(dr, BarrNombre)?.ToString();
            entity.TrnPctCodi = Convert.ToInt32(valorReturn(dr, TrnPctCodi) ?? 0);
            entity.CoresoCodi = Convert.ToInt32(valorReturn(dr, CoresoCodiPotcn) ?? 0);
            entity.CoregeCodi = ConvertToNull<int>(valorReturn(dr, CoregeCodiPotcn));
            entity.TrnpCagrp = ConvertToNull<int>(valorReturn(dr, TrnpCagrp));
            entity.TrnpcNumOrd = ConvertToNull<int>(valorReturn(dr, TrnpcNumordm));
            entity.TrnpCcodiCas = ConvertToNull<int>(valorReturn(dr, TrnpCcodiCas));

            entity.TrnPctTotalMwFija = Convert.ToDecimal(valorReturn(dr, TrnPctTotalmwFija));
            entity.TrnPctHpMwFija = Convert.ToDecimal(valorReturn(dr, TrnPctHpmwFija));
            entity.TrnPctHfpMwFija = Convert.ToDecimal(valorReturn(dr, TrnPctHfpmwFija));
            entity.TrnPctTotalMwVariable = Convert.ToDecimal(valorReturn(dr, TrnPctTotalmwVariable));
            entity.TrnPctHpMwFijaVariable = Convert.ToDecimal(valorReturn(dr, TrnPctHpmwFijaVariable));
            entity.TrnPctHfpMwFijaVariable = Convert.ToDecimal(valorReturn(dr, TrnPctHfpmwFijaVariable));
            entity.TrnPctComeObs = valorReturn(dr, TrnPctComeObs)?.ToString();
            entity.TrnPctExcel = Convert.ToInt32(valorReturn(dr, TrnPcExcel) ?? 0);
            entity.esPrimerRegistro =  Convert.ToInt32(valorReturn(dr, "esPrimerRegistro"));
            entity.TrnPctPtoSumins = valorReturn(dr, trnpctptosumins)?.ToString();
            return entity;
        }
        #endregion

        #region Mapeo de Campos
        #region Campos Originales
        public string TrnPctCodi = "trnpctcodi";
        public string PeriCodi = "pericodi";
        public string EmprCodi = "emprcodi";
        public string CoresoCodi = "CoresoCodi";
        public string TrnPctPtoSumins = "trnpctptosumins";
        public string TrnPctTotalMwFija = "trnpcttotalmwfija";
        public string TrnPctHpMwFija = "trnpcthpmwfija";
        public string TrnPctHfpMwFija = "trnpcthfpmwfija";
        public string TrnPctTotalMwVariable = "trnpcttotalmwvariable";
        public string TrnPctHpMwFijaVariable = "trnpcthfpmwfijavariable";
        public string TrnPctHfpMwFijaVariable = "trnpcthfpmwfijavariable";
        public string TrnPctComeObs = "trnpctcomeobs";
        public string TrnPctUserNameIns = "trnpctusernameins";
        public string TrnPctFecIns = "trnpctfecins";
        #endregion

        #region Campos Adicionales
        public string GenEmprCodi = "genemprcodi";
        public string PeriNombre = "perinombre";
        public string BarrCodi = "barrcodi";
        public string BarrNombre = "barrnombre";
        public string TipConCodi = "tipconcodi";
        public string TipUsuCodi = "tipusucodi";
        public string CliEmprCodi = "cliemprcodi";
        public string CoresoCodigo = "coresocodigo";
        public string CoresoFechaRegistro = "coresofecharegistro";
        public string CoresoDescripcion = "coresodescripcion";
        public string CoresoDetalle = "coresodetalle";
        public string CoresoFechaInicio = "coresofechainicio";
        public string CoresoFechaFin = "coresofechafin";
        public string EmprNomb = "emprnomb";
        public string CliNombre = "clinombre";
        public string BarrBarraTransferencia = "barrbarratransferencia";
        public string TipConNombre = "tipconnombre";
        public string TrnpcTipoPotencia = "TRNPCTIPOPOTENCIA";
        public string TipUsuNombre = "TipUsuNombre";

        //dasaji-210212 
        public string CoregeCodi = "CoregeCodi";
        public string CoresoCodiFirst = "CoresoCodiFirst";
        public string CoregeCodiFirst = "CoregeCodiFirst";
        public string TrnpcNumord = "TrnpcNumord";
        public string TrnpCagrp = "TrnpCagrp";
        public string TrnnpcCodicas = "TrnnpcCodicas";
        public string OmitirExcel = "OmitirExcel";

        public string TrnPctTotalMWFija = "TrnPctTotalMWFija";
        public string TrnPctHPMWFija = "TrnPctHPMWFija";
        public string TrnPctHFPMWFija = "TrnPctHFPMWFija";
        public string TrnPctTotalMWVariable = "TrnPctTotalMWVariable";
        public string TrnPctHPMWFijaVariable = "TrnPctHPMWFijaVariable";
        public string TrnPctHFPMWFijaVariable = "TrnPctHFPMWFijaVariable";
        public string TrnPctUsernameIns = "TrnPctUsernameIns";
        public string CodigoAgrupado = "CodigoAgrupado";
        public string TrnPcEnvCodi = "TrnPcEnvCodi";
        public string TrnPcEnvUsuario = "TrnPcEnvUsuario";
        public string TrnPcEnvFechaRe = "TrnPcEnvFechaRe";
        public string TrnPcEnvHoraRe = "TrnPcEnvHoraRe";

        public string TrnpctCodi = "TRNPCTCODI";
        public string CoresoCodiPotcn = "CORESOCODIPOTCN";
        public string CoregeCodiPotcn = "COREGECODIPOTCN";

        public string TrnpcNumordm = "TRNPCNUMORDM";
        public string TrnpCcodiCas = "TRNPCCODICAS";
        public string TipCasAbrev = "TIPCASABREV";


        public string TrnPctTotalmwFija = "TRNPCTTOTALMWFIJA";
        public string TrnPctHpmwFija = "TRNPCTHPMWFIJA";
        public string TrnPctHfpmwFija = "TRNPCTHFPMWFIJA";
        public string TrnPctTotalmwVariable = "TRNPCTTOTALMWVARIABLE";
        public string TrnPctHpmwFijaVariable = "TRNPCTHPMWFIJAVARIABLE";
        public string TrnPctHfpmwFijaVariable = "TRNPCTHFPMWFIJAVARIABLE";
        public string trnpctptosumins = "TRNPCTPTOSUMINS";

        public string TrnPcExcel = "trnpcexcel";

        #endregion
        #endregion

        #region Querys SQL
        //assetec 20200604
        public string SqlCopiarPotenciasContratadasByPeriodo
        {
            get { return base.GetSqlXml("CopiarPotenciasContratadasByPeriodo"); }
        }

        public string SqlDeleteByPeriodo
        {
            get { return base.GetSqlXml("DeleteByPeriodo"); }
        }

        //dasaji-210212
        public string SqlGenerarCodigosAgrupadosReservados
        {
            get { return base.GetSqlXml("GenerarCodigosAgrupadosReservados"); }
        }
        public string SqlGenerarPotenciasAgrupadas
        {
            get { return base.GetSqlXml("GenerarPotenciasAgrupadas"); }
        }
        public string SqlDesagruparPotencias
        {
            get { return base.GetSqlXml("DesagruparPotencias"); }
        }
        public string SqlSaveRegistrosExcel
        {
            get { return base.GetSqlXml("SaveRegistrosExcel"); }
        }
        public string SaveRegistrosExcelEnvio
        {
            get { return base.GetSqlXml("SaveRegistrosExcelEnvio"); }
        }
        public string SqlDesactivarPotencias
        {
            get { return base.GetSqlXml("DesactivarPotencias"); }
        }
        public string SqlDesactivarPotenciasPorBarrSum
        {
            get { return base.GetSqlXml("DesactivarPotenciasPorBarrSum"); }
        }
        public string SqlSaveRegistrosPotencia
        {
            get { return base.GetSqlXml("SaveRegistrosPotencia"); }
        }
        public string SqlSaveRegistrosPotenciaAprobar
        {
            get { return base.GetSqlXml("SaveRegistrosPotenciaAprobar"); }
        }
        public string SqlAprobarSolicitudCambios
        {
            get { return base.GetSqlXml("AprobarSolicitudCambios"); }
        }
        public string SqlRechazarSolicitudCambios
        {
            get { return base.GetSqlXml("RechazarSolicitudCambios"); }
        }
        public string ListarEnvios
        {
            get { return base.GetSqlXml("ListarEnvios"); }
        }
        public string ListarPotencias
        {
            get { return base.GetSqlXml("ListarPotencias"); }
        }
        public string ListarPotenciasAprobar
        {
            get { return base.GetSqlXml("ListarPotenciasAprobar"); }
        }
        public string SqlListaGrupoAsociadoVTA
        {
            get { return base.GetSqlXml("ListaGrupoAsociadoVTA"); }
        }
        #endregion
    }
}
