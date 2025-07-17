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
    /// Clase que contiene el mapeo de la tabla VTP_CODIGO_RETIRO_GENERADO
    /// </summary>
    public class CodigoRetiroGeneradoHelper : HelperBase
    {

        public CodigoRetiroGeneradoHelper() : base(Consultas.CodigoRetiroGeneradoSql)
        {
        }
        public string CORESOCODI = "CORESOCODI";
        public string CoresoCodigo = "coresoCodigo";
        #region Mapeo de Campos
        public string CoregeCodi = "COREGECODI";
        public string PeriCodi = "PERICODI";
        public string SoliCodiRetiFechaInicio = "CodiRetiFechaInicio";
        public string SoliCodiRetiFechaFin = "CodiRetiFechaFin";
        public string Trnpctipopotencia = "Trnpctipopotencia";
        public string PeriAnio = "PeriAnio";
        public string PeriMes = "PeriMes";
        public string CoresdCcodi = "CORESDCCODI";
        public string CoregeEstado = "COREGEESTADO";
        public string GenemprCodi = "GENEMPRCODI";
        public string CliemprCodi = "CLIEMPRCODI";
        public string BarrCodiTra = "BARRCODITRA";
        public string BarrCodiSum = "BARRCODISUM";
        public string BarrNombre = "BARRNOMBRE";
        public string CoregeCodVTP = "COREGECODVTP";
        public string EstdDescripcion = "ESTDDESCRIPCION";
        public string EstdAbrev = "ESTDABREV";

        //Para la vista

        public string TretCodigo = "TRETCODIGO";
        public string TretCoresoCorescCodi = "TRETCORESOCORESCCODI";
        public string TretTabla = "TRETTABLA";
        public string FechaInicio = "FECHAINICIO";
        public string FechaFin = "FECHAFIN";
        public string NroPagina = "NROPAGINA";
        public string PageSize = "PAGESIZE";
        public string EmprNomb = "EMPRNOMB";
        public string TipConNombre = "TIPCONNOMBRE";
        public string TipUsuNombre = "TIPUSUNOMBRE";
        public string PegrdPoteCalculada = "PEGRDPOTECALCULADA";
        public string PegrdPoteCoincidente = "PEGRDPOTECOINCIDENTE";
        public string PegrdPoteDeclarada = "PEGRDPOTEDECLARADA";
        public string PegrdPeajeUnitario = "PEGRDPEAJEUNITARIO";
        public string PegrdFacPerdida = "PEGRDFACPERDIDA";
        public string PegrdCalidad = "PEGRDCALIDAD";
        public string TrnPcTipoPotencia = "TRNPCTIPOPOTENCIA";


        #endregion
        private bool columnsExist(string columnName, IDataReader dr)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {

                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        public CodigoRetiroGeneradoDTO Create(IDataReader dr)
        {
            CodigoRetiroGeneradoDTO entity = new CodigoRetiroGeneradoDTO();



            if (columnsExist(this.CORESOCODI, dr))
            {
                int iCORESOCODI = dr.GetOrdinal(this.CORESOCODI);
                if (!dr.IsDBNull(iCORESOCODI)) entity.CoresoCodi = dr.GetInt32(iCORESOCODI);
            }


            if (columnsExist(this.CoregeCodi, dr))
            {
                int iCOREGECODI = dr.GetOrdinal(this.CoregeCodi);
                if (!dr.IsDBNull(iCOREGECODI)) entity.CoregeCodi = dr.GetInt32(iCOREGECODI);
            }

            if (columnsExist(this.BarrCodiSum, dr))
            {
                int iBARRCODISUM = dr.GetOrdinal(this.BarrCodiSum);
                if (!dr.IsDBNull(iBARRCODISUM)) entity.BarrCodiSum = dr.GetInt32(iBARRCODISUM);
            }

            if (columnsExist(this.BarrNombre, dr))
            {
                int iBARRNOMBRE = dr.GetOrdinal(this.BarrNombre);
                if (!dr.IsDBNull(iBARRNOMBRE)) entity.BarrNombre = dr.GetString(iBARRNOMBRE);
            }
            if (columnsExist(this.CoregeCodVTP, dr))
            {
                int iCOREGECODVTP = dr.GetOrdinal(this.CoregeCodVTP);
                if (!dr.IsDBNull(iCOREGECODVTP)) entity.CoregeCodVTP = dr.GetString(iCOREGECODVTP);
            }
            if (columnsExist(this.EstdAbrev, dr))
            {
                int iESTDABREV = dr.GetOrdinal(this.EstdAbrev);
                if (!dr.IsDBNull(iESTDABREV)) entity.EstdAbrev = dr.GetString(iESTDABREV);
            }
            if (columnsExist(this.EstdDescripcion, dr))
            {
                int iESTDDESCRIPCION = dr.GetOrdinal(this.EstdDescripcion);
                if (!dr.IsDBNull(iESTDDESCRIPCION)) entity.EstdDescripcion = dr.GetString(iESTDDESCRIPCION);
            }
            if (columnsExist(this.EmprNomb, dr))
            {
                int iEMPRNOMB = dr.GetOrdinal(this.EmprNomb);
                if (!dr.IsDBNull(iEMPRNOMB)) entity.Emprnomb = dr.GetString(iEMPRNOMB);
            }
            if (columnsExist(this.TipConNombre, dr))
            {
                int iTIPCONNOMBRE = dr.GetOrdinal(this.TipConNombre);
                if (!dr.IsDBNull(iTIPCONNOMBRE)) entity.Tipconnombre = dr.GetString(iTIPCONNOMBRE);
            }
            if (columnsExist(this.TipUsuNombre, dr))
            {
                int iTIPUSUNOMBRE = dr.GetOrdinal(this.TipUsuNombre);
                if (!dr.IsDBNull(iTIPUSUNOMBRE)) entity.Tipusunombre = dr.GetString(iTIPUSUNOMBRE);
            }
            if (columnsExist(this.PegrdPoteCalculada, dr))
            {
                int iPEGRDPOTECALCULADA = dr.GetOrdinal(this.PegrdPoteCalculada);
                if (!dr.IsDBNull(iPEGRDPOTECALCULADA)) entity.Pegrdpotecalculada = dr.GetDecimal(iPEGRDPOTECALCULADA);
            }
            if (columnsExist(this.PegrdPoteCoincidente, dr))
            {
                int iPEGRDPOTECOINCIDENTE = dr.GetOrdinal(this.PegrdPoteCoincidente);
                if (!dr.IsDBNull(iPEGRDPOTECOINCIDENTE)) entity.Pegrdpotecoincidente = dr.GetDecimal(iPEGRDPOTECOINCIDENTE);
            }
            if (columnsExist(this.PegrdPoteDeclarada, dr))
            {
                int iPEGRDPOTEDECLARADA = dr.GetOrdinal(this.PegrdPoteDeclarada);
                if (!dr.IsDBNull(iPEGRDPOTEDECLARADA)) entity.Pegrdpotedeclarada = dr.GetDecimal(iPEGRDPOTEDECLARADA);
            }
            if (columnsExist(this.PegrdPeajeUnitario, dr))
            {
                int iPEGRDPEAJEUNITARIO = dr.GetOrdinal(this.PegrdPeajeUnitario);
                if (!dr.IsDBNull(iPEGRDPEAJEUNITARIO)) entity.Pegrdpeajeunitario = dr.GetDecimal(iPEGRDPEAJEUNITARIO);
            }
            if (columnsExist(this.PegrdFacPerdida, dr))
            {
                int iPEGRDFACPERDIDA = dr.GetOrdinal(this.PegrdFacPerdida);
                if (!dr.IsDBNull(iPEGRDFACPERDIDA)) entity.Pegrdfacperdida = dr.GetDecimal(iPEGRDFACPERDIDA);
            }
            if (columnsExist(this.PegrdCalidad, dr))
            {
                int iPEGRDCALIDAD = dr.GetOrdinal(this.PegrdCalidad);
                if (!dr.IsDBNull(iPEGRDCALIDAD)) entity.Pegrdcalidad = dr.GetString(iPEGRDCALIDAD);
            }
            return entity;
        }
        public string SqlListarCodigosGeneradoVTP
        {
            get { return base.GetSqlXml("ListarCodigosGeneradoVTP"); }
        }
        public string SqlListarCodigosVTPByEmpBar
        {
            get { return base.GetSqlXml("ListarCodigosVTPByEmpBar"); }
        }
        public string SqlUpdateEstado
        {
            get { return base.GetSqlXml("UpdateEstado"); }
        }
        public string SqlGenerarAprobacion
        {
            get { return base.GetSqlXml("GenerarAprobacion"); }
        }
        public string SqlDesactivarSolicitudPeriodoActual
        {
            get { return base.GetSqlXml("DesactivarSolicitudPeriodoActual"); }
        }
        public string SqlGenerarPotenciasPeriodosAbiertos
        {
            get { return base.GetSqlXml("GenerarPotenciasPeriodosAbiertos"); }
        }
        public string SqlGenerarVTAPeriodosAbiertos
        {
            get { return base.GetSqlXml("GenerarVTAPeriodosAbiertos"); }
        }
        public string SqlGenerarVTPPeriodosAbiertos
        {
            get { return base.GetSqlXml("GenerarVTPPeriodosAbiertos"); }
        }

        public string SqlListarCodigosGeneradoVTPExtranet
        {
            get { return base.GetSqlXml("ListarCodigosGeneradosVTPExtranet"); }
        }

        public string SqlCodigoGeneradoVTPByCodivoVTP
        {
            get { return base.GetSqlXml("CodigoGeneradoVTPByCodivoVTP"); }
        }
    }
}
