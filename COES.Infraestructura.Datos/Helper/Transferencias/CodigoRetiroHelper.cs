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
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TRN_CODIGO_RETIRO_SOLICITUD
    /// </summary>
    public class CodigoRetiroHelper : HelperBase
    {
        string campos = "";
        public CodigoRetiroHelper() : base(Consultas.CodigoRetiroSql)
        {
        }
        private bool columnsExist(string columnName, IDataReader dr)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {

                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        public CodigoRetiroDTO Create(IDataReader dr)
        {
            CodigoRetiroDTO entity = new CodigoRetiroDTO();

            if (columnsExist(this.SoliCodiRetiCodi, dr))
            {
                int iSOLICODIRETICODI = dr.GetOrdinal(this.SoliCodiRetiCodi);
            if (!dr.IsDBNull(iSOLICODIRETICODI)) entity.SoliCodiRetiCodi = dr.GetInt32(iSOLICODIRETICODI);
            }

            if (columnsExist(this.EmprCodi, dr))
            {
                int iEMPRCODI = dr.GetOrdinal(this.EmprCodi);
            if (!dr.IsDBNull(iEMPRCODI)) entity.EmprCodi = dr.GetInt32(iEMPRCODI);
            }

            if (columnsExist(this.EmprCodi, dr))
            {
                int iEMPRCODI = dr.GetOrdinal(this.EmprCodi);
                if (!dr.IsDBNull(iEMPRCODI)) entity.EmprCodi = dr.GetInt32(iEMPRCODI);
            }

            if (columnsExist(this.BarrCodi, dr))
            {
                int iBARRCODI = dr.GetOrdinal(this.BarrCodi);
                if (!dr.IsDBNull(iBARRCODI)) entity.BarrCodi = dr.GetInt32(iBARRCODI);
            }
            if (columnsExist(this.UsuaCodi, dr))
            {
                int iUSUACODI = dr.GetOrdinal(this.UsuaCodi);
            if (!dr.IsDBNull(iUSUACODI)) entity.UsuaCodi = dr.GetString(iUSUACODI);
            }
            if (columnsExist(this.TipoContCodi, dr))
            {
                int iTIPOCONTCODI = dr.GetOrdinal(this.TipoContCodi);
            if (!dr.IsDBNull(iTIPOCONTCODI)) entity.TipoContCodi = dr.GetInt32(iTIPOCONTCODI);
            }

            if (columnsExist(this.TipoUsuaCodi, dr))
            {
                int iTIPOUSUACODI = dr.GetOrdinal(this.TipoUsuaCodi);
            if (!dr.IsDBNull(iTIPOUSUACODI)) entity.TipoUsuaCodi = dr.GetInt32(iTIPOUSUACODI);
            }

            if (columnsExist(this.CliCodi, dr))
            {
                int iCLICODI = dr.GetOrdinal(this.CliCodi);
            if (!dr.IsDBNull(iCLICODI)) entity.CliCodi = dr.GetInt32(iCLICODI);
            }

            if (columnsExist(this.SoliCodiRetiCodigo, dr))
            {
                int iSOLICODIRETICODIGO = dr.GetOrdinal(this.SoliCodiRetiCodigo);
            if (!dr.IsDBNull(iSOLICODIRETICODIGO)) entity.SoliCodiRetiCodigo = dr.GetString(iSOLICODIRETICODIGO);
            }

            if (columnsExist(this.SoliCodiRetiFechaRegistro, dr))
            {
                int iSOLICODIRETIFECHAREGISTRO = dr.GetOrdinal(this.SoliCodiRetiFechaRegistro);
            if (!dr.IsDBNull(iSOLICODIRETIFECHAREGISTRO)) entity.SoliCodiRetiFechaRegistro = dr.GetDateTime(iSOLICODIRETIFECHAREGISTRO);
            }
            if (columnsExist(this.SoliCodiRetiDescripcion, dr))
            {
                int iSOLICODIRETIDESCRIPCION = dr.GetOrdinal(this.SoliCodiRetiDescripcion);
            if (!dr.IsDBNull(iSOLICODIRETIDESCRIPCION)) entity.SoliCodiRetiDescripcion = dr.GetString(iSOLICODIRETIDESCRIPCION);
            }
            if (columnsExist(this.SoliCodiRetiDetalleAmplio, dr))
            {
                int iSOLICODIRETIDETALLEAMPLIO = dr.GetOrdinal(this.SoliCodiRetiDetalleAmplio);
            if (!dr.IsDBNull(iSOLICODIRETIDETALLEAMPLIO)) entity.SoliCodiRetiDetalleAmplio = dr.GetString(iSOLICODIRETIDETALLEAMPLIO);
            }
            if (columnsExist(this.SoliCodiRetiFechaInicio, dr))
            {
                int iSOLICODIRETIFECHAINICIO = dr.GetOrdinal(this.SoliCodiRetiFechaInicio);
            if (!dr.IsDBNull(iSOLICODIRETIFECHAINICIO)) entity.SoliCodiRetiFechaInicio = dr.GetDateTime(iSOLICODIRETIFECHAINICIO);
            }
            if (columnsExist(this.SoliCodiRetiFechaFin, dr))
            {
                int iSOLICODIRETIFECHAFIN = dr.GetOrdinal(this.SoliCodiRetiFechaFin);
            if (!dr.IsDBNull(iSOLICODIRETIFECHAFIN)) entity.SoliCodiRetiFechaFin = dr.GetDateTime(iSOLICODIRETIFECHAFIN);
            }

            if (columnsExist(this.SoliCodiRetiObservacion, dr))
            {
                int iSOLICODIRETIOBSERVACION = dr.GetOrdinal(this.SoliCodiRetiObservacion);
            if (!dr.IsDBNull(iSOLICODIRETIOBSERVACION)) entity.SoliCodiRetiObservacion = dr.GetString(iSOLICODIRETIOBSERVACION);
            }


            if (columnsExist(this.SoliCodiRetiEstado, dr))
            {
                int iSOLICODIRETIESTADO = dr.GetOrdinal(this.SoliCodiRetiEstado);
            if (!dr.IsDBNull(iSOLICODIRETIESTADO)) entity.SoliCodiRetiEstado = dr.GetString(iSOLICODIRETIESTADO);
            }

            if (columnsExist(this.CoesUserName, dr))
            {
                int iCOESUSERNAME = dr.GetOrdinal(this.CoesUserName);
            if (!dr.IsDBNull(iCOESUSERNAME)) entity.CoesUserName = dr.GetString(iCOESUSERNAME);
            }
            if (columnsExist(this.UsuaCodi, dr))
            {
                int iUSUACODI = dr.GetOrdinal(this.UsuaCodi);
                if (!dr.IsDBNull(iUSUACODI)) entity.Seinusername = dr.GetString(iUSUACODI);
            }
            
            if (columnsExist(this.SoliCodiRetiFecIns, dr))
            {
                int iSOLICODIRETIFECINS = dr.GetOrdinal(this.SoliCodiRetiFecIns);
            if (!dr.IsDBNull(iSOLICODIRETIFECINS)) entity.SoliCodiRetiFecIns = dr.GetDateTime(iSOLICODIRETIFECINS);
            }

            if (columnsExist(this.SolidCodiRetiFecAct, dr))
            {
                int iSOLICODIRETIFECACT = dr.GetOrdinal(this.SolidCodiRetiFecAct);
            if (!dr.IsDBNull(iSOLICODIRETIFECACT)) entity.SoliCodiRetiFecAct = dr.GetDateTime(iSOLICODIRETIFECACT);
            }

            if (columnsExist(this.SoliCodiRetiFechaSolBaja, dr))
            {
                int iSOLICODIRETIFECHASOLBAJA = dr.GetOrdinal(this.SoliCodiRetiFechaSolBaja);
            if (!dr.IsDBNull(iSOLICODIRETIFECHASOLBAJA)) entity.SoliCodiretiFechaSolBaja = dr.GetDateTime(iSOLICODIRETIFECHASOLBAJA);
            }

            if (columnsExist(this.SOLICODIRETIFECHADEBAJA, dr))
            {
            int iSOLICODIRETIFECHADEBAJA = dr.GetOrdinal(this.SOLICODIRETIFECHADEBAJA);
            if (!dr.IsDBNull(iSOLICODIRETIFECHADEBAJA)) entity.SoliCodiRetiFechaBaja = dr.GetDateTime(iSOLICODIRETIFECHADEBAJA);
            }

            if (columnsExist(this.EmprNomb, dr))
            {
                int iEMPRNOMB = dr.GetOrdinal(this.EmprNomb);
            if (!dr.IsDBNull(iEMPRNOMB)) entity.EmprNombre = dr.GetString(iEMPRNOMB);
            }

            if (columnsExist(this.BarrNomBarrTran, dr))
            {
                int iBARRNOMBBARRTRAN = dr.GetOrdinal(this.BarrNomBarrTran);
            if (!dr.IsDBNull(iBARRNOMBBARRTRAN)) entity.BarrNombBarrTran = dr.GetString(iBARRNOMBBARRTRAN);      
            }
            
            if (columnsExist(this.TipoContNombre, dr))
            {
                int iTIPOCONTNOMBRE = dr.GetOrdinal(this.TipoContNombre);
            if (!dr.IsDBNull(iTIPOCONTNOMBRE)) entity.TipoContNombre = dr.GetString(iTIPOCONTNOMBRE);
            }
                  
            if (columnsExist(this.TipoUsuaNombre, dr))
            {
                int iTIPOUSUANOMBRE = dr.GetOrdinal(this.TipoUsuaNombre);
            if (!dr.IsDBNull(iTIPOUSUANOMBRE)) entity.TipoUsuaNombre = dr.GetString(iTIPOUSUANOMBRE);
            }

            if (columnsExist(this.CliRuc, dr))
            {
                int iCLIRUC = dr.GetOrdinal(this.CliRuc);
                if (!dr.IsDBNull(iCLIRUC)) entity.CliRuc = dr.GetString(iCLIRUC);
            }

            if (columnsExist(this.CliNombre, dr))
            {
                int iCLINOMBRE = dr.GetOrdinal(this.CliNombre);
            if (!dr.IsDBNull(iCLINOMBRE)) entity.CliNombre = dr.GetString(iCLINOMBRE);
            }

            if (columnsExist(this.EstdCodi, dr))
            {
                int iESTDCODI = dr.GetOrdinal(this.EstdCodi);
                if (!dr.IsDBNull(iESTDCODI)) entity.EstCodigo = dr.GetString(iESTDCODI);
            }
            if (columnsExist(this.EstdDescripcion, dr))
            {
                int iESTDESCRIPCION = dr.GetOrdinal(this.EstdDescripcion);
                if (!dr.IsDBNull(iESTDESCRIPCION)) entity.EstDescripcion = dr.GetString(iESTDESCRIPCION);
            }
            if (columnsExist(this.EstdAbrev, dr))
            {
                int iESTDABREV = dr.GetOrdinal(this.EstdAbrev);
                if (!dr.IsDBNull(iESTDABREV)) entity.EstAbrev = dr.GetString(iESTDABREV);
            }
            if (columnsExist(this.CoresoVariacion, dr))
            {
                int iCORESOVARIACION = dr.GetOrdinal(this.CoresoVariacion);
                if (!dr.IsDBNull(iCORESOVARIACION)) entity.Variacion = dr.GetDecimal(iCORESOVARIACION);
            }

            if (columnsExist(this.EmprAbrev, dr))
            {
                int iEMPRABREV = dr.GetOrdinal(this.EmprAbrev);
                if (!dr.IsDBNull(iEMPRABREV)) entity.EmprAbrevia = dr.GetString(iEMPRABREV);
            }
            if (columnsExist(this.CoregeCodi, dr))
            {
                int iCoregeCodi = dr.GetOrdinal(this.CoregeCodi);
                if (!dr.IsDBNull(iCoregeCodi)) entity.CoregeCodi = dr.GetInt32(iCoregeCodi);
            }

            if (columnsExist(this.BARRCODISUM, dr))
            {
                int iBARRCODISUM = dr.GetOrdinal(this.BARRCODISUM);
                if (!dr.IsDBNull(iBARRCODISUM)) entity.BarrCodiSum = dr.GetInt32(iBARRCODISUM);
            }

            if (columnsExist(this.BarrNombre, dr))
            {
                int iBarrNombre = dr.GetOrdinal(this.BarrNombre);
                if (!dr.IsDBNull(iBarrNombre)) entity.BarrNombBarrSum = dr.GetString(iBarrNombre);
            }

            if (columnsExist(this.CoregeCodVTP, dr))
            {
                int iCoregeCodVTP = dr.GetOrdinal(this.CoregeCodVTP);
                if (!dr.IsDBNull(iCoregeCodVTP)) entity.CoregeCodVTP = dr.GetString(iCoregeCodVTP);
            }
            if (columnsExist(this.EstdDescripcionVTP, dr))
            {
                int iEstdDescripcionVTP = dr.GetOrdinal(this.EstdDescripcionVTP);
                if (!dr.IsDBNull(iEstdDescripcionVTP)) entity.EstDescripcionVTP = dr.GetString(iEstdDescripcionVTP);
            }
            if (columnsExist(this.EstdAbrevVTP, dr))
            {
                int iEstdAbrevVTP = dr.GetOrdinal(this.EstdAbrevVTP);
                if (!dr.IsDBNull(iEstdAbrevVTP)) entity.EstAbrevVTP = dr.GetString(iEstdAbrevVTP);
            }
            
            if (columnsExist(this.TrnpcTipoCasoAgrupado, dr))
            {
                int iTrnpcTipoCasoAgrupado = dr.GetOrdinal(this.TrnpcTipoCasoAgrupado);
                if (!dr.IsDBNull(iTrnpcTipoCasoAgrupado)) entity.TrnpcTipoCasoAgrupado = dr.GetString(iTrnpcTipoCasoAgrupado);
            }

            if (columnsExist(this.EstApr, dr))
            {
                int iESTDAPR = dr.GetOrdinal(this.EstApr);
                if (!dr.IsDBNull(iESTDAPR)) entity.EstApr = dr.GetString(iESTDAPR);
            }

            if (columnsExist(this.TrnpcTipoPotencia, dr))
            {
                int iTipoPotencia = dr.GetOrdinal(this.TrnpcTipoPotencia);
                if (!dr.IsDBNull(iTipoPotencia)) entity.TrnpcTipoPotencia = dr.GetInt32(iTipoPotencia);
            }

            return entity;
        }

        #region Mapeo de Campos
        public string SoliCodiRetiCodi = "CORESOCODI";
        public string CoregeCodVTEAVTP = "COREGECODVTEAVTP";
        public string EmprCodi = "GENEMPRCODI";
        public string BarrCodi = "BARRCODI";
        public string BARRCODISUM = "BARRCODISUM";
        public string BarrNombre = "BARRNOMBRE";
        public string UsuaCodi = "SEINUSERNAME";
        public string TipoContCodi = "TIPCONCODI";
        public string TipoUsuaCodi = "TIPUSUCODI";
        public string CliCodi = "CLIEMPRCODI";
        public string SoliCodiRetiCodigo = "CORESOCODIGO";
        public string SoliCodiRetiFechaRegistro = "CORESOFECHAREGISTRO";
        public string SoliCodiRetiDescripcion = "CORESODESCRIPCION";
        public string SoliCodiRetiDetalleAmplio = "CORESODETALLE";
        public string SoliCodiRetiFechaInicio = "CORESOFECHAINICIO";
        public string SoliCodiRetiFechaFin = "CORESOFECHAFIN";
        public string PeriCodi = "PERICODI";
        public string SoliCodiRetiObservacion = "CORESOOBSERVACION";
        public string SoliCodiRetiEstado = "CORESOESTADO";
        public string SoliCodiRetiEstadoAprobar = "CORESOESTAPR";
        public string SoliCodiRetiEstadoVTP = "CORESOESTADOVTP";
        public string CoesUserName = "COESUSERNAME";
        public string SoliCodiRetiFecIns = "CORESOFECINS";
        public string SolidCodiRetiFecAct = "CORESOFECACT";
        public string SoliCodiRetiFechaSolBaja = "CORESOFECHASOLICITUDBAJA";
        public string SOLICODIRETIFECHADEBAJA = "CORESOFECHADEBAJA";
        public string EmprNomb = "EMPRNOMB";
        public string CoregeCodVTP = "COREGECODVTP";
        public string CoregeCodi = "COREGECODI";
        public string EmprAbrev = "EMPRABREV";
        public string BarrNomBarrTran = "BARRBARRATRANSFERENCIA";
        public string TipoContNombre = "TIPCONNOMBRE";
        public string TipoUsuaNombre = "TIPUSUNOMBRE";
        public string CliRuc = "CLIRUC";
        public string CliNombre = "CLINOMBRE";
        public string EstdCodi = "ESTDCODI";
        public string EstdAbrev = "ESTDABREV";
        public string EstdAbrevVTP = "ESTDABREVVTP";
        public string EstdDescripcion = "ESTDDESCRIPCION";
        public string EstdDescripcionVTP = "ESTDDESCRIPCIONVTP";
        public string CoresoVariacion = "CORESOVARIACION";
        public string EstApr = "ESTAPR";
        //Para la vista

        public string TretCodigo = "TRETCODIGO";
        public string TretCoresoCorescCodi = "TRETCORESOCORESCCODI";
        public string TretTabla = "TRETTABLA";
        public string FechaInicio = "FECHAINICIO";
        public string FechaFin = "FECHAFIN";
        public string NroPagina = "NROPAGINA";
        public string PageSize = "PAGESIZE";


        public string TipConNombre = "TIPCONNOMBRE";
        public string TipUsuNombre = "TIPUSUNOMBRE";
        public string PegrdPoteCalculada = "PEGRDPOTECALCULADA";
        public string PegrdPoteCoincidente = "PEGRDPOTECOINCIDENTE";
        public string PegrdPoteDeclarada = "PEGRDPOTEDECLARADA";
        public string PegrdPeajeUnitario = "PEGRDPEAJEUNITARIO";
        public string PegrdFacPerdida = "PEGRDFACPERDIDA";
        public string PegrdCalidad = "PEGRDCALIDAD";

        //Potencia contratada
 
         
        public string TrnpctCodi = "TRNPCTCODI";
        public string CoresoCodiPotcn = "CORESOCODIPOTCN";
        public string CoregeCodiPotcn = "COREGECODIPOTCN";
        public string TrnpCagrp = "TRNPCAGRP";
        public string TrnpcNumordm = "TRNPCNUMORDM";
        public string TrnpCcodiCas = "TRNPCCODICAS";
        public string TipCasAbrev = "TIPCASABREV";
        public string TrnpcTipoCasoAgrupado = "TrnpcTipoCasoAgrupado";


        public string TrnPctTotalmwFija = "TRNPCTTOTALMWFIJA";
        public string TrnPctHpmwFija = "TRNPCTHPMWFIJA";
        public string TrnPctHfpmwFija = "TRNPCTHFPMWFIJA";
        public string TrnPctTotalmwVariable = "TRNPCTTOTALMWVARIABLE";
        public string TrnPctHpmwFijaVariable = "TRNPCTHPMWFIJAVARIABLE";
        public string TrnPctHfpmwFijaVariable = "TRNPCTHFPMWFIJAVARIABLE";

        public string TrnPctTotalmwFijaApr = "TRNPCTTOTALMWFIJAAPR";
        public string TrnPctHpmwFijaApr = "TRNPCTHPMWFIJAAPR";
        public string TrnPctHfpmwFijaApr = "TRNPCTHFPMWFIJAAPR";
        public string TrnPctTotalmwVariableApr = "TRNPCTTOTALMWVARIABLEAPR";
        public string TrnPctHpmwFijaVariableApr = "TRNPCTHPMWFIJAVARIABLEAPR";
        public string TrnPctHfpmwFijaVariableApr = "TRNPCTHFPMWFIJAVARIABLEAPR";

        public string TrnPctComeObs = "trnpctcomeobs";
        public string TrnPcExcel = "trnpcexcel";


        public string TrnpcTipoPotencia = "TRNPCTIPOPOTENCIA";

        #endregion
        public string SqlUpdateBajaCodigoVTEA
        {
            get { return base.GetSqlXml("UpdateBajaCodigoVTEA"); }
        }

        public string SqlUpdateBajaCodigoVTEAVTP
        {
            get { return base.GetSqlXml("UpdateBajaCodigoVTEAVTP"); }
        }

        public string SqlUpdateEstado
        {
            get { return base.GetSqlXml("UpdateEstado"); }
        }
        public string SqlUpdateEstadoRechazar
        {
            get { return base.GetSqlXml("UpdateEstadoRechazar"); }
        }
        public string SqlUpdateVariacion
        {
            get { return base.GetSqlXml("UpdateVariacion"); }
        }
        public string SqlListarGestionCodigosVTEAVTP
        {
            get { return base.GetSqlXml("ListarGestionCodigosVTEAVTP"); }
        }
        public string SqlListarGestionCodigosVTEAVTPAprobar
        {
            get { return base.GetSqlXml("ListarGestionCodigosVTEAVTPAprobar"); }
        }
        public string SqlListarGestionCodigosExportarVTEAVTP
        {
            get { return base.GetSqlXml("ListarGestionCodigosExportarVTEAVTP"); }
        }
        public string SqlListarCodigoVTEAByEmprBarr
        {
            get { return base.GetSqlXml("ListarCodigoVTEAByEmprBarr"); }
        }
        public string SqlListarCodigosGeneradoVTP
        {
            get { return base.GetSqlXml("ListarCodigosGeneradoVTP"); }
        }
        public string SqlTotalRecordsGestionarCodigosVTEAVTP
        {
            get { return base.GetSqlXml("TotalRecordsGestionarCodigosVTEAVTP"); }
        }
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
        public string SqlGetByIdGestionCodigosVTEAVTP
        {
            get
            {
                return base.GetSqlXml("GetByIdGestionCodigosVTEAVTP");
            }
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

        public string SqlValidarExisteCodigoEnEnvios
        {
            get { return base.GetSqlXml("ValidarExisteCodigoEnEnvios"); }
        }

        #region PrimasRER.2023
        public string SqlListCodRetirosByEmpresaYFecha
        {
            get { return base.GetSqlXml("ListCodRetirosByEmpresaYFecha"); }
        }
        #endregion
    }
}
