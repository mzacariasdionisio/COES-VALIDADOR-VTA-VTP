using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla F_LECTURA
    /// </summary>
    public class FLecturaHelper : HelperBase
    {
        public FLecturaHelper(): base(Consultas.FLecturaSql)
        {
        }

        public FLecturaDTO Create(IDataReader dr)
        {
            FLecturaDTO entity = new FLecturaDTO();

            int iFechahora = dr.GetOrdinal(this.Fechahora);
            if (!dr.IsDBNull(iFechahora)) entity.Fechahora = dr.GetDateTime(iFechahora);

            int iGpscodi = dr.GetOrdinal(this.Gpscodi);
            if (!dr.IsDBNull(iGpscodi)) entity.Gpscodi = Convert.ToInt32(dr.GetValue(iGpscodi));

            int iVsf = dr.GetOrdinal(this.Vsf);
            if (!dr.IsDBNull(iVsf)) entity.Vsf = dr.GetDecimal(iVsf);

            int iMaximo = dr.GetOrdinal(this.Maximo);
            if (!dr.IsDBNull(iMaximo)) entity.Maximo = dr.GetDecimal(iMaximo);

            int iMinimo = dr.GetOrdinal(this.Minimo);
            if (!dr.IsDBNull(iMinimo)) entity.Minimo = dr.GetDecimal(iMinimo);

            int iVoltaje = dr.GetOrdinal(this.Voltaje);
            if (!dr.IsDBNull(iVoltaje)) entity.Voltaje = dr.GetDecimal(iVoltaje);

            int iNum = dr.GetOrdinal(this.Num);
            if (!dr.IsDBNull(iNum)) entity.Num = Convert.ToInt32(dr.GetValue(iNum));

            int iDesv = dr.GetOrdinal(this.Desv);
            if (!dr.IsDBNull(iDesv)) entity.Desv = dr.GetDecimal(iDesv);

            int iH0 = dr.GetOrdinal(this.H0);
            if (!dr.IsDBNull(iH0)) entity.H0 = dr.GetDecimal(iH0);

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

            int iH49 = dr.GetOrdinal(this.H49);
            if (!dr.IsDBNull(iH49)) entity.H49 = dr.GetDecimal(iH49);

            int iH50 = dr.GetOrdinal(this.H50);
            if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

            int iH51 = dr.GetOrdinal(this.H51);
            if (!dr.IsDBNull(iH51)) entity.H51 = dr.GetDecimal(iH51);

            int iH52 = dr.GetOrdinal(this.H52);
            if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

            int iH53 = dr.GetOrdinal(this.H53);
            if (!dr.IsDBNull(iH53)) entity.H53 = dr.GetDecimal(iH53);

            int iH54 = dr.GetOrdinal(this.H54);
            if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

            int iH55 = dr.GetOrdinal(this.H55);
            if (!dr.IsDBNull(iH55)) entity.H55 = dr.GetDecimal(iH55);

            int iH56 = dr.GetOrdinal(this.H56);
            if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

            int iH57 = dr.GetOrdinal(this.H57);
            if (!dr.IsDBNull(iH57)) entity.H57 = dr.GetDecimal(iH57);

            int iH58 = dr.GetOrdinal(this.H58);
            if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

            int iH59 = dr.GetOrdinal(this.H59);
            if (!dr.IsDBNull(iH59)) entity.H59 = dr.GetDecimal(iH59);

            int iDevsec = dr.GetOrdinal(this.Devsec);
            if (!dr.IsDBNull(iDevsec)) entity.Devsec = dr.GetDecimal(iDevsec);

            return entity;
        }


        #region Mapeo de Campos

        public string Fechahora = "FECHAHORA";
        public string Gpscodi = "GPSCODI";
        public string Vsf = "VSF";
        public string Maximo = "MAXIMO";
        public string Minimo = "MINIMO";
        public string Voltaje = "VOLTAJE";
        public string Num = "NUM";
        public string Desv = "DESV";
        public string H0 = "H0";
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
        public string H49 = "H49";
        public string H50 = "H50";
        public string H51 = "H51";
        public string H52 = "H52";
        public string H53 = "H53";
        public string H54 = "H54";
        public string H55 = "H55";
        public string H56 = "H56";
        public string H57 = "H57";
        public string H58 = "H58";
        public string H59 = "H59";
        public string Devsec = "DEVSEC";
        public string CaracterH = "H";
        public string Meditotal = "MEDITOTAL";

        #endregion

        public string SqlGetFechaDesvNumPorGpsFecha
        {
            get { return base.GetSqlXml("GetFechaDesvNumPorGpsFecha"); }
        }

        public string SqlObtenerFrecuenciaSein
        {
            get { return base.GetSqlXml("ObtenerFrecuenciaSein"); }
        }
        public string SqlGetByCriteria2
        {
            get { return GetSqlXml("GetByCriteria2"); }
        }
        public string SqlContarRegistrosRepetidos
        {
            get { return GetSqlXml("ContarRegistrosRepetidos"); }
        }
        #region MigracionSGOCOES-GrupoB
        public string SqlListaCalidadProducto
        {
            get { return base.GetSqlXml("ListaCalidadProducto"); }
        }
        #endregion
    }
}
