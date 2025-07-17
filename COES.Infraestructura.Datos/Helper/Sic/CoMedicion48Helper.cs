using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CO_MEDICION48
    /// </summary>
    public class CoMedicion48Helper : HelperBase
    {
        public CoMedicion48Helper(): base(Consultas.CoMedicion48Sql)
        {
        }

        public CoMedicion48DTO Create(IDataReader dr)
        {
            CoMedicion48DTO entity = new CoMedicion48DTO();

            int iComedcodi = dr.GetOrdinal(this.Comedcodi);
            if (!dr.IsDBNull(iComedcodi)) entity.Comedcodi = Convert.ToInt32(dr.GetValue(iComedcodi));

            int iCotinfcodi = dr.GetOrdinal(this.Cotinfcodi);
            if (!dr.IsDBNull(iCotinfcodi)) entity.Cotinfcodi = Convert.ToInt32(dr.GetValue(iCotinfcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iCopercodi = dr.GetOrdinal(this.Copercodi);
            if (!dr.IsDBNull(iCopercodi)) entity.Copercodi = Convert.ToInt32(dr.GetValue(iCopercodi));

            int iCovercodi = dr.GetOrdinal(this.Covercodi);
            if (!dr.IsDBNull(iCovercodi)) entity.Covercodi = Convert.ToInt32(dr.GetValue(iCovercodi));

            int iComedfecha = dr.GetOrdinal(this.Comedfecha);
            if (!dr.IsDBNull(iComedfecha)) entity.Comedfecha = dr.GetDateTime(iComedfecha);

            int iH1 = dr.GetOrdinal(this.H1);
            if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

            int iH2 = dr.GetOrdinal(this.H2);
            if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

            int iH3 = dr.GetOrdinal(this.H3);
            if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

            int iH4 = dr.GetOrdinal(this.H4);
            if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

            int iH5 = dr.GetOrdinal(this.H5);
            if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

            int iH6 = dr.GetOrdinal(this.H6);
            if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

            int iH7 = dr.GetOrdinal(this.H7);
            if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

            int iH8 = dr.GetOrdinal(this.H8);
            if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

            int iH9 = dr.GetOrdinal(this.H9);
            if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

            int iH10 = dr.GetOrdinal(this.H10);
            if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

            int iH11 = dr.GetOrdinal(this.H11);
            if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

            int iH12 = dr.GetOrdinal(this.H12);
            if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

            int iH13 = dr.GetOrdinal(this.H13);
            if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

            int iH14 = dr.GetOrdinal(this.H14);
            if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

            int iH15 = dr.GetOrdinal(this.H15);
            if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

            int iH16 = dr.GetOrdinal(this.H16);
            if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

            int iH17 = dr.GetOrdinal(this.H17);
            if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

            int iH18 = dr.GetOrdinal(this.H18);
            if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

            int iH19 = dr.GetOrdinal(this.H19);
            if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

            int iH20 = dr.GetOrdinal(this.H20);
            if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

            int iH21 = dr.GetOrdinal(this.H21);
            if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

            int iH22 = dr.GetOrdinal(this.H22);
            if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

            int iH23 = dr.GetOrdinal(this.H23);
            if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

            int iH24 = dr.GetOrdinal(this.H24);
            if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

            int iH25 = dr.GetOrdinal(this.H25);
            if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

            int iH26 = dr.GetOrdinal(this.H26);
            if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

            int iH27 = dr.GetOrdinal(this.H27);
            if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

            int iH28 = dr.GetOrdinal(this.H28);
            if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

            int iH29 = dr.GetOrdinal(this.H29);
            if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

            int iH30 = dr.GetOrdinal(this.H30);
            if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

            int iH31 = dr.GetOrdinal(this.H31);
            if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

            int iH32 = dr.GetOrdinal(this.H32);
            if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

            int iH33 = dr.GetOrdinal(this.H33);
            if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

            int iH34 = dr.GetOrdinal(this.H34);
            if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

            int iH35 = dr.GetOrdinal(this.H35);
            if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

            int iH36 = dr.GetOrdinal(this.H36);
            if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

            int iH37 = dr.GetOrdinal(this.H37);
            if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

            int iH38 = dr.GetOrdinal(this.H38);
            if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

            int iH39 = dr.GetOrdinal(this.H39);
            if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

            int iH40 = dr.GetOrdinal(this.H40);
            if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

            int iH41 = dr.GetOrdinal(this.H41);
            if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

            int iH42 = dr.GetOrdinal(this.H42);
            if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

            int iH43 = dr.GetOrdinal(this.H43);
            if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

            int iH44 = dr.GetOrdinal(this.H44);
            if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

            int iH45 = dr.GetOrdinal(this.H45);
            if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

            int iH46 = dr.GetOrdinal(this.H46);
            if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

            int iH47 = dr.GetOrdinal(this.H47);
            if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

            int iH48 = dr.GetOrdinal(this.H48);
            if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

            return entity;
        }


        #region Mapeo de Campos

        public string Comedcodi = "COMEDCODI";
        public string Cotinfcodi = "COTINFCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Copercodi = "COPERCODI";
        public string Covercodi = "COVERCODI";
        public string Comedfecha = "COMEDFECHA";
        public string H1 = "H1";
        public string H2 = "H2";
        public string H3 = "H3";
        public string H4 = "H4";
        public string H5 = "H5";
        public string H6 = "H6";
        public string H7 = "H7";
        public string H8 = "H8";
        public string H9 = "H9";
        public string H10 = "H10";
        public string H11 = "H11";
        public string H12 = "H12";
        public string H13 = "H13";
        public string H14 = "H14";
        public string H15 = "H15";
        public string H16 = "H16";
        public string H17 = "H17";
        public string H18 = "H18";
        public string H19 = "H19";
        public string H20 = "H20";
        public string H21 = "H21";
        public string H22 = "H22";
        public string H23 = "H23";
        public string H24 = "H24";
        public string H25 = "H25";
        public string H26 = "H26";
        public string H27 = "H27";
        public string H28 = "H28";
        public string H29 = "H29";
        public string H30 = "H30";
        public string H31 = "H31";
        public string H32 = "H32";
        public string H33 = "H33";
        public string H34 = "H34";
        public string H35 = "H35";
        public string H36 = "H36";
        public string H37 = "H37";
        public string H38 = "H38";
        public string H39 = "H39";
        public string H40 = "H40";
        public string H41 = "H41";
        public string H42 = "H42";
        public string H43 = "H43";
        public string H44 = "H44";
        public string H45 = "H45";
        public string H46 = "H46";
        public string H47 = "H47";
        public string H48 = "H48";

        public string T1 = "T1";
        public string T2 = "T2";
        public string T3 = "T3";
        public string T4 = "T4";
        public string T5 = "T5";
        public string T6 = "T6";
        public string T7 = "T7";
        public string T8 = "T8";
        public string T9 = "T9";
        public string T10 = "T10";
        public string T11 = "T11";
        public string T12 = "T12";
        public string T13 = "T13";
        public string T14 = "T14";
        public string T15 = "T15";
        public string T16 = "T16";
        public string T17 = "T17";
        public string T18 = "T18";
        public string T19 = "T19";
        public string T20 = "T20";
        public string T21 = "T21";
        public string T22 = "T22";
        public string T23 = "T23";
        public string T24 = "T24";
        public string T25 = "T25";
        public string T26 = "T26";
        public string T27 = "T27";
        public string T28 = "T28";
        public string T29 = "T29";
        public string T30 = "T30";
        public string T31 = "T31";
        public string T32 = "T32";
        public string T33 = "T33";
        public string T34 = "T34";
        public string T35 = "T35";
        public string T36 = "T36";
        public string T37 = "T37";
        public string T38 = "T38";
        public string T39 = "T39";
        public string T40 = "T40";
        public string T41 = "T41";
        public string T42 = "T42";
        public string T43 = "T43";
        public string T44 = "T44";
        public string T45 = "T45";
        public string T46 = "T46";
        public string T47 = "T47";
        public string T48 = "T48";

        public string Topcodi1 = "TOPCODI1";
        public string Topcodi = "TOPCODI";
        public string Topfecha = "TOPFECHA";
        public string Topfinal = "TOPFINAL";
        public string Topiniciohora = "TOPINICIOHORA";
        public string Rsffecha = "rsfhorfecha";
        public string Rsffechainicio = "rsfhorinicio";
        public string Rsffechafin = "rsfhorfin";
        public string Gruponomb = "gruponomb";
        public string Rsfsubida = "rsfdetsub";
        public string Rsfbajada = "rsfdetbaj";
        public string Ursnomb = "ursnomb";
        public string Equicodi = "EQUICODI";
        public string Famcodi = "FAMCODI";
        public string Comedtipo = "COMEDTIPO";
        public string Equicodi1 = "EQUICODI1";
        public string NombreTabla = "CO_MEDICION48";
        public string Pmin = "PMIN";
        public string Pmax = "PMAX";
        public string Pefec = "PEFEC";
        public string Fechaop = "fechahop";
        public string Hophorini = "hophorini";
        public string Hophorfin = "hophorfin";
        public string Emprcodi = "EMPRCODI";
        public string Topnombre = "TOPNOMBRE";        

        public string SqlObtenerTopologias
        {
            get { return base.GetSqlXml("ObtenerTopologias"); }
        }

        public string SqlObtenerTopologiasSinReserva
        {
            get { return base.GetSqlXml("ObtenerTopologiasSinReserva"); }
        }

        public string SqlObtenerDatosYupana
        {
            get { return base.GetSqlXml("ObtenerDatosYupana"); }
        }

        public string SqlObtenerDatosYupanaAgrupado
        {
            get { return base.GetSqlXml("ObtenerDatosYupanaAgrupado"); }
        }

        public string SqlObtenerDatosRAEjecutado
        {
            get { return base.GetSqlXml("ObtenerDatosRAEjecutado"); }
        }

        public string SqlObtenerDatosProgramaFinal
        {
            get { return base.GetSqlXml("ObtenerDatosProgramaFinal"); }
        }

        public string SqlObtenerDatosReservaFinal
        {
            get { return base.GetSqlXml("ObtenerDatosReservaFinal"); }
        }


        public string SqlObtenerListadoURS
        {
            get { return base.GetSqlXml("ObtenerListadoURS"); }
        }

        public string SqlObtenerPropiedadHidraulica
        {
            get { return base.GetSqlXml("ObtenerPropiedadHidraulica"); }
        }

        public string SqlObtenerReporteBandas
        {
            get { return base.GetSqlXml("ObtenerReporteBandas"); }
        }

        public string SqlObtenerPropiedadTermica
        {
            get { return base.GetSqlXml("ObtenerPropiedadTermica"); }
        }

        public string SqlObtenerPropiedadPotenciaEfectiva
        {
            get { return base.GetSqlXml("ObtenerPropiedadPotenciaEfectiva"); }
        }

        public string SqlObtenerPropiedadPotenciaMinima
        {
            get { return base.GetSqlXml("ObtenerPropiedadPotenciaMinima"); }
        }

        public string SqlObtenerHorasOperacion
        {
            get { return base.GetSqlXml("ObtenerHorasOperacion"); }
        }

        public string SqlObtenerDatosResultadoFinal
        {
            get { return base.GetSqlXml("ObtenerDatosResultadoFinal"); }
        }

        public string SqlObtenerReporteProgramadoFinal
        {
            get { return base.GetSqlXml("ObtenerReporteProgramadoFinal"); }
        }

        public string SqlObtenerReporteReservaFinal
        {
            get { return base.GetSqlXml("ObtenerReporteReservaFinal"); }
        }

        public string SqlObtenerReporteDespachoFinal
        {
            get { return base.GetSqlXml("ObtenerReporteDespachoFinal"); }
        }

        public string SqlObtenerLiquidacionResultadoFinal
        {
            get { return base.GetSqlXml("ObtenerLiquidacionResultadoFinal"); }
        }

        public string SqlEliminarLiquidacion
        {
            get { return base.GetSqlXml("EliminarLiquidacion"); }
        }

        public string SqlObtenerReporteReprograma
        {
            get { return base.GetSqlXml("ObtenerReporteReprograma"); }
        }

        public string SqlObtenerReporteReprogramaSinRSF
        {
            get { return base.GetSqlXml("ObtenerReporteReprogramaSinRSF"); }
        }

        #endregion
    }
}
