using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    public class SolicitudCodigoHelper : HelperBase
    {
        private bool columnsExist(string columnName, IDataReader dr)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {

                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        private object valorReturn(IDataReader dr, string sColumna)
        {
            object resultado = null;
            int iIndex;
            if (columnsExist(sColumna, dr))
            {
                iIndex = dr.GetOrdinal(sColumna);
                if (!dr.IsDBNull(iIndex))
                    resultado = dr.GetValue(iIndex);
            }
            return resultado?.ToString();
        }
        public static T? ConvertToNull<T>(object x) where T : struct
        {
            return x == null ? null : (T?)Convert.ChangeType(x, typeof(T));
        }
        public SolicitudCodigoHelper() : base(Consultas.SolicitudCodigoSql)
        {
        }

        public SolicitudCodigoDTO Create(IDataReader dr)
        {
            SolicitudCodigoDTO entity = new SolicitudCodigoDTO();

            int iSOLICODIRETICODI = dr.GetOrdinal(this.Solicodireticodi);
            if (!dr.IsDBNull(iSOLICODIRETICODI)) entity.SoliCodiRetiCodi = dr.GetInt32(iSOLICODIRETICODI);

            int iEMPRCODI = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEMPRCODI)) entity.EmprCodi = dr.GetInt32(iEMPRCODI);

            int iBARRCODI = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBARRCODI)) entity.BarrCodi = dr.GetInt32(iBARRCODI);

            int iUSUACODI = dr.GetOrdinal(this.Usuacodi);
            if (!dr.IsDBNull(iUSUACODI)) entity.UsuaCodi = dr.GetString(iUSUACODI);

            int iTIPOCONTCODI = dr.GetOrdinal(this.Tipocontcodi);
            if (!dr.IsDBNull(iTIPOCONTCODI)) entity.TipoContCodi = dr.GetInt32(iTIPOCONTCODI);

            int iTIPOUSUACODI = dr.GetOrdinal(this.Tipousuacodi);
            if (!dr.IsDBNull(iTIPOUSUACODI)) entity.TipoUsuaCodi = dr.GetInt32(iTIPOUSUACODI);

            int iCLICODI = dr.GetOrdinal(this.Clicodi);
            if (!dr.IsDBNull(iCLICODI)) entity.CliCodi = dr.GetInt32(iCLICODI);

            int iSOLICODIRETICODIGO = dr.GetOrdinal(this.Solicodireticodigo);
            if (!dr.IsDBNull(iSOLICODIRETICODIGO)) entity.SoliCodiRetiCodigo = dr.GetString(iSOLICODIRETICODIGO);

            int iSOLICODIRETIFECHAREGISTRO = dr.GetOrdinal(this.Solicodiretifecharegistro);
            if (!dr.IsDBNull(iSOLICODIRETIFECHAREGISTRO)) entity.SoliCodiRetiFechaRegistro = dr.GetDateTime(iSOLICODIRETIFECHAREGISTRO);

            int iSOLICODIRETIDESCRIPCION = dr.GetOrdinal(this.Solicodiretidescripcion);
            if (!dr.IsDBNull(iSOLICODIRETIDESCRIPCION)) entity.SoliCodiRetiDescripcion = dr.GetString(iSOLICODIRETIDESCRIPCION);

            int iSOLICODIRETIDETALLEAMPLIO = dr.GetOrdinal(this.Solicodiretidetalleamplio);
            if (!dr.IsDBNull(iSOLICODIRETIDETALLEAMPLIO)) entity.SoliCodiRetiDetalleAmplio = dr.GetString(iSOLICODIRETIDETALLEAMPLIO);

            int iSOLICODIRETIFECHAINICIO = dr.GetOrdinal(this.Solicodiretifechainicio);
            if (!dr.IsDBNull(iSOLICODIRETIFECHAINICIO)) entity.SoliCodiRetiFechaInicio = dr.GetDateTime(iSOLICODIRETIFECHAINICIO);

            int iSOLICODIRETIFECHAFIN = dr.GetOrdinal(this.Solicodiretifechafin);
            if (!dr.IsDBNull(iSOLICODIRETIFECHAFIN)) entity.SoliCodiRetiFechaFin = dr.GetDateTime(iSOLICODIRETIFECHAFIN);

            int iSOLICODIRETIOBSERVACION = dr.GetOrdinal(this.Solicodiretiobservacion);
            if (!dr.IsDBNull(iSOLICODIRETIOBSERVACION)) entity.SoliCodiRetiObservacion = dr.GetString(iSOLICODIRETIOBSERVACION);

            int iSOLICODIRETIESTADO = dr.GetOrdinal(this.Solicodiretiestado);
            if (!dr.IsDBNull(iSOLICODIRETIESTADO)) entity.SoliCodiRetiEstado = dr.GetString(iSOLICODIRETIESTADO);

            int iCOESUSERNAME = dr.GetOrdinal(this.Coesusername);
            if (!dr.IsDBNull(iCOESUSERNAME)) entity.CoesUserName = dr.GetString(iCOESUSERNAME);

            int iSOLICODIRETIFECINS = dr.GetOrdinal(this.Solicodiretifecins);
            if (!dr.IsDBNull(iSOLICODIRETIFECINS)) entity.SoliCodiRetiFecIns = dr.GetDateTime(iSOLICODIRETIFECINS);

            int iSOLICODIRETIFECACT = dr.GetOrdinal(this.Solicodiretifecact);
            if (!dr.IsDBNull(iSOLICODIRETIFECACT)) entity.SoliCodiRetiFecAct = dr.GetDateTime(iSOLICODIRETIFECACT);

            int iSOLICODIRETIFECHASOLBAJA = dr.GetOrdinal(this.Solicodiretifechasolbaja);
            if (!dr.IsDBNull(iSOLICODIRETIFECHASOLBAJA)) entity.SoliCodiretiFechaSolBaja = dr.GetDateTime(iSOLICODIRETIFECHASOLBAJA);

            int iSOLICODIRETIFECHADEBAJA = dr.GetOrdinal(this.Solicodiretifechadebaja);
            if (!dr.IsDBNull(iSOLICODIRETIFECHADEBAJA)) entity.SoliCodiRetiFechaBaja = dr.GetDateTime(iSOLICODIRETIFECHADEBAJA);

            int iEMPRNOMB = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEMPRNOMB)) entity.EmprNombre = dr.GetString(iEMPRNOMB);

            int iBARRNOMBBARRTRAN = dr.GetOrdinal(this.Barrnombbarrtran);
            if (!dr.IsDBNull(iBARRNOMBBARRTRAN)) entity.BarrNombBarrTran = dr.GetString(iBARRNOMBBARRTRAN);

            int iTIPOCONTNOMBRE = dr.GetOrdinal(this.Tipocontnombre);
            if (!dr.IsDBNull(iTIPOCONTNOMBRE)) entity.TipoContNombre = dr.GetString(iTIPOCONTNOMBRE);

            int iTIPOUSUANOMBRE = dr.GetOrdinal(this.Tipousuanombre);
            if (!dr.IsDBNull(iTIPOUSUANOMBRE)) entity.TipoUsuaNombre = dr.GetString(iTIPOUSUANOMBRE);

            int iCLINOMBRE = dr.GetOrdinal(this.Clinombre);
            if (!dr.IsDBNull(iCLINOMBRE)) entity.CliNombre = dr.GetString(iCLINOMBRE);

            int iBARRNOMBBARRSUM = dr.GetOrdinal(this.Barrnombbarrsum);
            if (!dr.IsDBNull(iBARRNOMBBARRSUM)) entity.BarraNomBarrSum = dr.GetString(iBARRNOMBBARRSUM);

            int iSOLICODIRETICODIGOVTP = dr.GetOrdinal(this.Solicodireticodigovtp);
            if (!dr.IsDBNull(iSOLICODIRETICODIGOVTP)) entity.SoliCodiRetiCodigoVTP = dr.GetString(iSOLICODIRETICODIGOVTP);

            int iSOLICODIRETIESTADOVTP = dr.GetOrdinal(this.Solicodiretiestadovtp);
            if (!dr.IsDBNull(iSOLICODIRETIESTADOVTP)) entity.SoliCodiRetiEstadoVTP = dr.GetString(iSOLICODIRETIESTADOVTP);

            int iDESCRIPCIONESTADOVTEA = dr.GetOrdinal(this.Descripcionestadovtea);
            if (!dr.IsDBNull(iDESCRIPCIONESTADOVTEA)) entity.EstadoDescripcionVTEA = dr.GetString(iDESCRIPCIONESTADOVTEA);

            int iDESCRIPCIONESTADOVTP = dr.GetOrdinal(this.Descripcionestadovtp);
            if (!dr.IsDBNull(iDESCRIPCIONESTADOVTP)) entity.EstadoDescripcionVTP = dr.GetString(iDESCRIPCIONESTADOVTP);

            int iCODRETGENCODI = dr.GetOrdinal(this.Codretgencodi);
            if (!dr.IsDBNull(iCODRETGENCODI)) entity.Codretgencodi = dr.GetInt32(iCODRETGENCODI);


            //potencia contrato
            entity.TrnpctCodi = Convert.ToInt32(valorReturn(dr, TrnpctCodi) ?? 0);
            entity.CoresoCodiPotcn = ConvertToNull<int>(valorReturn(dr, CoresoCodiPotcn));
            entity.CoregeCodiPotcn = ConvertToNull<int>(valorReturn(dr, CoregeCodiPotcn));
            entity.TrnpCagrp = ConvertToNull<int>(valorReturn(dr, TrnpCagrp));
            entity.TrnpCagrpVTA = ConvertToNull<int>(valorReturn(dr, TrnpCagrpVTA));
            entity.TrnpCagrpVTP = ConvertToNull<int>(valorReturn(dr, TrnpCagrpVTP));
            entity.TrnpcNumordm = ConvertToNull<int>(valorReturn(dr, TrnpcNumordm));
            entity.TrnpcNumordmVTA = ConvertToNull<int>(valorReturn(dr, TrnpcNumordmVTA));
            entity.TrnpcNumordmVTP = ConvertToNull<int>(valorReturn(dr, TrnpcNumordmVTP));
            entity.TrnpCcodiCas = ConvertToNull<int>(valorReturn(dr, TrnpCcodiCas));
            entity.TipCasaAbrev = valorReturn(dr, TipCasAbrev)?.ToString();
            entity.TrnPctTotalmwFija = ConvertToNull<decimal>(valorReturn(dr, TrnPctTotalmwFija));
            entity.TrnPctHpmwFija = ConvertToNull<decimal>(valorReturn(dr, TrnPctHpmwFija));
            entity.TrnPctHfpmwFija = ConvertToNull<decimal>(valorReturn(dr, TrnPctHfpmwFija));
            entity.TrnPctTotalmwVariable = ConvertToNull<decimal>(valorReturn(dr, TrnPctTotalmwVariable));
            entity.TrnPctHpmwFijaVariable = ConvertToNull<decimal>(valorReturn(dr, TrnPctHpmwFijaVariable));
            entity.TrnPctHfpmwFijaVariable = ConvertToNull<decimal>(valorReturn(dr, TrnPctHfpmwFijaVariable));
            entity.TrnPctComeObs = valorReturn(dr, TrnPctComeObs)?.ToString();
            entity.TrnPctExcel = Convert.ToInt32(valorReturn(dr, TrnPcExcel) ?? 0);
            entity.TrnpcTipoPotencia = ConvertToNull<int>(valorReturn(dr, TrnpcTipoPotencia));
            entity.TrnpcTipoCasoAgrupado = valorReturn(dr, TrnpcTipoCasoAgrupado)?.ToString();
            entity.abrevEstadoVTA = valorReturn(dr, AbrevEstadoVTA)?.ToString();
            entity.abrevEstadoVTP = valorReturn(dr, AbrevEstadoVTP)?.ToString();


            return entity;
        }

        #region Mapeo de Campos
        public string Solicodireticodi = "CORESOCODI";
        public string PeriCodi = "PERICODI";
        public string Emprcodi = "GENEMPRCODI";
        public string Barrcodi = "BARRCODI";
        public string Usuacodi = "SEINUSERNAME";
        public string Tipocontcodi = "TIPCONCODI";
        public string Tipousuacodi = "TIPUSUCODI";
        public string Clicodi = "CLIEMPRCODI";
        public string Solicodireticodigo = "CORESOCODIGO";
        public string Solicodiretifecharegistro = "CORESOFECHAREGISTRO";
        public string Solicodiretidescripcion = "CORESODESCRIPCION";
        public string Solicodiretidetalleamplio = "CORESODETALLE";
        public string Solicodiretifechainicio = "CORESOFECHAINICIO";
        public string Solicodiretifechafin = "CORESOFECHAFIN";
        public string Solicodiretiobservacion = "CORESOOBSERVACION";
        public string Solicodiretiestado = "CORESOESTADO";
        public string Coesusername = "COESUSERNAME";
        public string Solicodiretifecins = "CORESOFECINS";
        public string Solicodiretifecact = "CORESOFECACT";
        public string Solicodiretifechasolbaja = "CORESOFECHASOLICITUDBAJA";
        public string Solicodiretifechadebaja = "CORESOFECHADEBAJA";
        public string Emprnomb = "EMPRNOMB";
        public string Barrnombbarrtran = "BARRBARRATRANSFERENCIA";
        public string Tipocontnombre = "TIPCONNOMBRE";
        public string Tipousuanombre = "TIPUSUNOMBRE";
        public string Clinombre = "CLINOMBRE";
        public string Solicodiretiusumodificacion = "CORESOUSUMODIFICACION";
        public string Solicodiretifecmodificacion = "CORESOFECMODIFICACION";




        //potencia contrato
        public string PeridcCodi = "PERIDCCODI";
        public string CodcnpeUsuarioRegi = "CODCNPEUSUARIOREGI";
        public string TrnpctCodi = "TRNPCTCODI";
        public string CoresoCodiPotcn = "CORESOCODIPOTCN";
        public string CoregeCodiPotcn = "COREGECODIPOTCN";
        public string TrnpCagrp = "TRNPCAGRP";
        public string TrnpCagrpVTA = "TRNPCAGRPVTA";
        public string TrnpCagrpVTP = "TRNPCAGRPVTP";
        public string TrnpcNumordm = "TRNPCNUMORDM";
        public string TrnpcNumordmVTA = "TRNPCNUMORDMVTA";
        public string TrnpcNumordmVTP = "TRNPCNUMORDMVTP";
        public string TrnpCcodiCas = "TRNPCCODICAS";
        public string TipCasAbrev = "TIPCASABREV";
        public string TrnpcTipoCasoAgrupado = "TrnpcTipoCasoAgrupado";
        public string CoresoCodi = "CORESOCODI";


        public string TrnPctTotalmwFija = "TRNPCTTOTALMWFIJA";
        public string TrnPctHpmwFija = "TRNPCTHPMWFIJA";
        public string TrnPctHfpmwFija = "TRNPCTHFPMWFIJA";
        public string TrnPctTotalmwVariable = "TRNPCTTOTALMWVARIABLE";
        public string TrnPctHpmwFijaVariable = "TRNPCTHPMWFIJAVARIABLE";
        public string TrnPctHfpmwFijaVariable = "TRNPCTHFPMWFIJAVARIABLE";
        public string TrnPctComeObs = "trnpctcomeobs";
        public string TrnPcExcel = "trnpcexcel";


        public string TrnpcTipoPotencia = "TRNPCTIPOPOTENCIA";

        // modificaciones 20201230
        public string Barrnombbarrsum = "BARRNOMBRE";
        public string Solicodireticodigovtp = "COREGECODVTP";
        public string Solicodiretiestadovtp = "COREGEESTADO";
        public string Descripcionestadovtea = "ESTADOVTEA";
        public string Descripcionestadovtp = "ESTADOVTP";
        public string Codretgencodi = "COREGECODI";
        public string AbrevEstadoVTA = "abrevEstadoVTA";
        public string AbrevEstadoVTP = "abrevEstadoVTP";

        //Para la vista
        public string Tretcodigo = "TRETCODIGO";
        public string Tretcoresocoresccodi = "TRETCORESOCORESCCODI";
        public string Trettabla = "TRETTABLA";
        public string Fechainicio = "FECHAINICIO";
        public string Fechafin = "FECHAFIN";
        public string Nropagina = "NROPAGINA";
        public string Pagesize = "PAGESIZE";

        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlObtenerPorCodigoRetiroCodigo
        {
            get { return base.GetSqlXml("GetBySoliCodireticodigo"); }
        }

        public string SqlGetByCriteriaExtranet
        {
            get { return base.GetSqlXml("GetByCriteriaExtranet"); }
        }

        public string SqlGetCodigoRetiroByCodigo
        {
            get { return base.GetSqlXml("GetCodigoRetiroByCodigo"); }
        }

        public string SqlCodigoRetiroVigenteByPeriodo
        {
            get { return base.GetSqlXml("CodigoRetiroVigenteByPeriodo"); }
        }

        //ASSETEC 202001
        public string SqlImportarCodigosRetiroSolicitud
        {
            get { return base.GetSqlXml("ImportarCodigosRetiroSolicitud"); }
        }

        public string SqlListarCodigoRetiro
        {
            get { return base.GetSqlXml("ListarCodigoRetiro"); }
        }
        public string SqlListarPaginado
        {
            get { return base.GetSqlXml("ListarCodigoRetiroPaginado"); }
        }
        public string SqlListarExportacionCodigoRetiro
        {
            get { return base.GetSqlXml("ListarExportacionCodigoRetiro"); }
        }

        public string SqlSolicitarBajar
        {
            get { return base.GetSqlXml("SolicitarBajar"); }
        }
        public string SqlUpdateObservacion
        {
            get { return base.GetSqlXml("UpdateObservacion"); }
        }
        public string SqlSaveSolicitudPeriodo
        {
            get { return base.GetSqlXml("SaveSolicitudPeriodo"); }
        }
        public string SqlSaveSolicitudPeriodoVTP
        {
            get { return base.GetSqlXml("SaveSolicitudPeriodoVTP"); }
        }

        public string SqlUpdateTipPotCodConsolidadoPeriodo
        {
            get { return base.GetSqlXml("UpdateTipPotCodConsolidadoPeriodo"); }
        }

        public string SqlUpdateTipPotCodCodigoRetiro
        {
            get { return base.GetSqlXml("UpdateTipPotCodCodigoRetiro"); }
        }

    }
}
