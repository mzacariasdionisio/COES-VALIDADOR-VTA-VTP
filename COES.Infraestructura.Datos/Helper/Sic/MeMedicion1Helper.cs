using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_MEDICION1
    /// </summary>
    public class MeMedicion1Helper : HelperBase
    {
        public MeMedicion1Helper(): base(Consultas.MeMedicion1Sql)
        {
        }

        public MeMedicion1DTO Create(IDataReader dr)
        {
            MeMedicion1DTO entity = new MeMedicion1DTO();

            int iLectcodi = dr.GetOrdinal(this.Lectcodi);
            if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

            int iMedifecha = dr.GetOrdinal(this.Medifecha);
            if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iH1 = dr.GetOrdinal(this.H1);
            if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iTipoptomedicodi = dr.GetOrdinal(this.Tipoptomedicodi);
            if (!dr.IsDBNull(iTipoptomedicodi)) entity.Tipoptomedicodi = Convert.ToInt32(dr.GetValue(iTipoptomedicodi));
            entity.Tipoptomedicodi = entity.Tipoptomedicodi > 0 ? entity.Tipoptomedicodi : -1;

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            return entity;
        }


        #region Mapeo de Campos

        public string Lectcodi = "LECTCODI";
        public string Medifecha = "MEDIFECHA";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string H1 = "H1";
        public string Nota = "NOTA";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Emprnomb = "EMPRNOMB";
        public string Grupocodi = "GRUPOCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Tipoinfodesc = "TIPOINFODESC";

        public string Equinomb = "Equinomb";
        public string Cuenca = "Cuenca";
        public string Tipoptomedinomb = "Tptomedinomb";
        public string Tipoinfoabrev = "Tipoinfoabrev";
        public string Ptomedibarranomb = "Ptomedibarranomb";
        public string Tipoptomedicodi = "Tptomedicodi";
        public string Famcodi = "Famcodi";
        public string Famabrev = "Famabrev";

        public string Osicodi = "Osicodi";
        public string Equicodi = "Equicodi";
        public string Emprcodi = "Emprcodi";
        public string Medifecha2 = "Medifecha2";

        #region Datos Base
        public string Nombre = "NOMBRE";
        public string Fecha = "FECHA";
        public string Valor = "VALOR";
        public string Dia = "DIA";
        #endregion

        #endregion

        public string SqlObtenerEmpresasStock
        {
            get { return base.GetSqlXml("ObtenerEmpresasStock"); }
        }

        public string SqlObtenerGruposStock
        {
            get { return base.GetSqlXml("ObtenerGruposStock"); }
        }

        public string SqlObtenerTipoCombustible
        {
            get { return base.GetSqlXml("ObtenerTipoCombustible"); }
        }

        public string SqlObtenerEstructura
        {
            get { return base.GetSqlXml("ObtenerEstructura"); }
        }

        public string SqlDeleteEnvioArchivo
        {
            get { return base.GetSqlXml("DeleteEnvioArchivo"); }
        }

        public string SqlGetEnvioArchivo
        {
            get { return base.GetSqlXml("GetEnvioArchivo"); }
        }

        public string SqlGetHidrologia
        {
            get { return base.GetSqlXml("GetHidrologia"); }
        }

        public string SqlGetDataFormatoSec
        {
            get { return base.GetSqlXml("GetDataFormatoSec"); }
        }

        public string SqlGetListaMedicion1
        {
            get { return base.GetSqlXml("GetListaMedicion1"); }
        }

        public string SqlGetMedicionPronosticoHidrologia
        {
            get { return base.GetSqlXml("GetMedicionPronosticoHidrologia"); }
        }

        public string SqlGetMedicionPronosticoHidrologiaByPtoCalculadoAndFecha
        {
            get { return base.GetSqlXml("GetMedicionPronosticoHidrologiaByPtoCalculadoAndFecha"); }
        }

        public string SqlDeleteEnvioArchivo2
        {
            get { return base.GetSqlXml("DeleteEnvioArchivo1"); }
        }

        public string SqlListadoInformacionSemanalPowel
        {
            get { return base.GetSqlXml("ListadoInformacionSemanalPowel"); }
        }

        public string SqlObtenerMedicion1
        {
            get { return base.GetSqlXml("ObtenerMedicion1"); }
        }


        #region INDISPONIBILIDADES

        public string SqlGetListaMedicion1ContratoCombustible
        {
            get { return base.GetSqlXml("GetListaMedicion1ContratoCombustible"); }
        }

        #endregion

        #region SIOSEIN2

        public string SqlGetDataEjecCaudales
        {
            get { return base.GetSqlXml("GetDataEjecCaudales"); }
        }

        #endregion

        #region Numerales Datos Base
        public string SqlDatosBase_5_7_1_1
        {
            get { return base.GetSqlXml("ListaDatosBase_5_7_1_1"); }
        }

        public string SqlDatosBase_5_7_1_2
        {
            get { return base.GetSqlXml("ListaDatosBase_5_7_1_2"); }
        }

        public string SqlDatosBase_5_7_2
        {
            get { return base.GetSqlXml("ListaDatosBase_5_7_2"); }
        }

        #endregion
    }
}
