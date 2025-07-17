using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class VceCompBajaeficHelper : HelperBase
    {
        public VceCompBajaeficHelper()
            : base(Consultas.VceCompBajaeficSql)
        {
        }

        public VceCompBajaeficDTO Create(IDataReader dr)
        {
            VceCompBajaeficDTO entity = new VceCompBajaeficDTO();

            int iCrcbetipocalc = dr.GetOrdinal(this.Crcbetipocalc);
            if (!dr.IsDBNull(iCrcbetipocalc)) entity.Crcbetipocalc = dr.GetString(iCrcbetipocalc);

            int iCrcbecompensacion = dr.GetOrdinal(this.Crcbecompensacion);
            if (!dr.IsDBNull(iCrcbecompensacion)) entity.Crcbecompensacion = dr.GetDecimal(iCrcbecompensacion);

            int iCrcbecvt = dr.GetOrdinal(this.Crcbecvt);
            if (!dr.IsDBNull(iCrcbecvt)) entity.Crcbecvt = dr.GetDecimal(iCrcbecvt);

            int iCrcbecvnc = dr.GetOrdinal(this.Crcbecvnc);
            if (!dr.IsDBNull(iCrcbecvnc)) entity.Crcbecvnc = dr.GetDecimal(iCrcbecvnc);

            int iCrcbecvc = dr.GetOrdinal(this.Crcbecvc);
            if (!dr.IsDBNull(iCrcbecvc)) entity.Crcbecvc = dr.GetDecimal(iCrcbecvc);

            int iCrcbeconsumo = dr.GetOrdinal(this.Crcbeconsumo);
            if (!dr.IsDBNull(iCrcbeconsumo)) entity.Crcbeconsumo = dr.GetDecimal(iCrcbeconsumo);

            int iCrcbepotencia = dr.GetOrdinal(this.Crcbepotencia);
            if (!dr.IsDBNull(iCrcbepotencia)) entity.Crcbepotencia = dr.GetDecimal(iCrcbepotencia);

            int iCrcbehorfin = dr.GetOrdinal(this.Crcbehorfin);
            if (!dr.IsDBNull(iCrcbehorfin)) entity.Crcbehorfin = dr.GetDateTime(iCrcbehorfin);

            int iCrcbehorini = dr.GetOrdinal(this.Crcbehorini);
            if (!dr.IsDBNull(iCrcbehorini)) entity.Crcbehorini = dr.GetDateTime(iCrcbehorini);

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iPecacodi = dr.GetOrdinal(this.Pecacodi);
            if (!dr.IsDBNull(iPecacodi)) entity.PecaCodi = Convert.ToInt32(dr.GetValue(iPecacodi));

            // Inicio de Agregados 31/05/2017 - Sistema de Compensaciones
            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

            int iSubcausadesc = dr.GetOrdinal(this.Subcausadesc);
            if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            int iMinimacarga = dr.GetOrdinal(this.Minimacarga);
            if (!dr.IsDBNull(iMinimacarga)) entity.Minimacarga = dr.GetDecimal(iMinimacarga);

            int iPruebapr25 = dr.GetOrdinal(this.Pruebapr25);
            if (!dr.IsDBNull(iPruebapr25)) entity.Pruebapr25 = dr.GetDecimal(iPruebapr25);

            int iPagocuenta = dr.GetOrdinal(this.Pagocuenta);
            if (!dr.IsDBNull(iPagocuenta)) entity.Pagocuenta = dr.GetDecimal(iPagocuenta);

            int iTotalmodo = dr.GetOrdinal(this.Totalmodo);
            if (!dr.IsDBNull(iTotalmodo)) entity.Totalmodo = dr.GetDecimal(iTotalmodo);

            int iSeguridad = dr.GetOrdinal(this.Seguridad);
            if (!dr.IsDBNull(iSeguridad)) entity.Seguridad = dr.GetDecimal(iSeguridad);

            int iRsf = dr.GetOrdinal(this.Rsf);
            if (!dr.IsDBNull(iRsf)) entity.Rsf = dr.GetDecimal(iRsf);

            int iReservaesp = dr.GetOrdinal(this.Reservaesp);
            if (!dr.IsDBNull(iReservaesp)) entity.Reservaesp = dr.GetDecimal(iReservaesp);

            int iTension = dr.GetOrdinal(this.Tension);
            if (!dr.IsDBNull(iTension)) entity.Tension = dr.GetDecimal(iTension);

            int iTotalemp = dr.GetOrdinal(this.Totalemp);
            if (!dr.IsDBNull(iTotalemp)) entity.Totalemp = dr.GetDecimal(iTotalemp);
            // Fin de Agregado

            return entity;
        }

        #region Mapeo de Campos

        public string Crcbetipocalc = "CRCBETIPOCALC";
        public string Crcbecompensacion = "CRCBECOMPENSACION";
        public string Crcbecvt = "CRCBECVT";
        public string Crcbecvnc = "CRCBECVNC";
        public string Crcbecvc = "CRCBECVC";
        public string Crcbeconsumo = "CRCBECONSUMO";
        public string Crcbepotencia = "CRCBEPOTENCIA";
        public string Crcbehorfin = "CRCBEHORFIN";
        public string Crcbehorini = "CRCBEHORINI";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Grupocodi = "GRUPOCODI";
        public string Pecacodi = "PECACODI";


        //Adicionales
        public string Emprnomb = "EMPRNOMB";
        public string Subcausadesc = "SUBCAUSADESC";
        public string Gruponomb = "GRUPONOMB";
        
        public string Minimacarga = "MINIMACARGA";
        public string Pruebapr25 = "PRUEBAPR25";
        public string Pagocuenta = "PAGOCUENTA";
        public string Totalmodo = "TOTALMODO";
        public string Seguridad = "SEGURIDAD";
        public string Rsf = "RSF";
        public string Reservaesp = "RESERVAESP";
        public string Tension = "TENSION";
        public string Totalemp = "TOTALEMP";
        public string Potenciaenergia = "POTENCIAENERGIA";
        #endregion

        public string SqlListCompensacionesRegulares
        {
            get { return base.GetSqlXml("ListCompensacionesRegulares"); }
        }

        public string SqlListModoOperacion
        {
            get { return base.GetSqlXml("ListModoOperacion"); }
        }
        public string SqlListMedicion
        {
            get { return base.GetSqlXml("ListMedicion"); }
        }

        public string SqlGetPeriodoCompensacion
        {
            get { return base.GetSqlXml("GetPeriodoCompensacion"); }
        }

        public string SqlGetDatCalculo
        {
            get { return base.GetSqlXml("GetDatCalculo"); }
        }

        public string SqlDeleteCompensacionManual
        {
            get { return base.GetSqlXml("DeleteCompensacionManual"); }
        }

        public string SqlDeleteByVersion
        {
            get { return base.GetSqlXml("DeleteByVersion"); }
        }

        public string SqlDeleteByVersionTipoCalculoAutomatico
        {
            get { return base.GetSqlXml("DeleteByVersionTipoCalculoAutomatico"); }
        }

        public string SqlSaveManual
        {
            get { return base.GetSqlXml("SaveManual"); }
        }

        public string SqlSaveFromOtherVersion
        {
            get { return base.GetSqlXml("SaveFromOtherVersion"); }
        }

        public string SqlListCompensacionOperacionInflexibilidad
        {
            get { return base.GetSqlXml("ListCompensacionOperacionInflexibilidad"); }
        }

        public string SqlListCompensacionOperacionSeguridad
        {
            get { return base.GetSqlXml("ListCompensacionOperacionSeguridad"); }
        }
        public string SqlListCompensacionOperacionRSF
        {
            get { return base.GetSqlXml("ListCompensacionOperacionRSF"); }
        }

        public string SqlListCompensacionRegulacionTension
        {
            get { return base.GetSqlXml("ListCompensacionRegulacionTension"); }
        }

        public string SqlListCompensacionOperacionMME
        {
            get { return base.GetSqlXml("ListCompensacionOperacionMME"); }
        }
        public string SqlListCompensacionDiarioMME
        {
            get { return base.GetSqlXml("ListCompensacionDiarioMME"); }
        }
    }
}
