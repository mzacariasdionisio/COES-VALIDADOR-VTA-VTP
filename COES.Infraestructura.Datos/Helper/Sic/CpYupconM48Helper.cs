using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_YUPCON_M48
    /// </summary>
    public class CpYupconM48Helper : HelperBase
    {
        public CpYupconM48Helper() : base(Consultas.CpYupconM48Sql)
        {
        }

        public CpYupconM48DTO Create(IDataReader dr)
        {
            CpYupconM48DTO entity = new CpYupconM48DTO();

            int iT27 = dr.GetOrdinal(this.T27);
            if (!dr.IsDBNull(iT27)) entity.T27 = Convert.ToInt32(dr.GetValue(iT27));

            int iT28 = dr.GetOrdinal(this.T28);
            if (!dr.IsDBNull(iT28)) entity.T28 = Convert.ToInt32(dr.GetValue(iT28));

            int iT29 = dr.GetOrdinal(this.T29);
            if (!dr.IsDBNull(iT29)) entity.T29 = Convert.ToInt32(dr.GetValue(iT29));

            int iT30 = dr.GetOrdinal(this.T30);
            if (!dr.IsDBNull(iT30)) entity.T30 = Convert.ToInt32(dr.GetValue(iT30));

            int iT31 = dr.GetOrdinal(this.T31);
            if (!dr.IsDBNull(iT31)) entity.T31 = Convert.ToInt32(dr.GetValue(iT31));

            int iT32 = dr.GetOrdinal(this.T32);
            if (!dr.IsDBNull(iT32)) entity.T32 = Convert.ToInt32(dr.GetValue(iT32));

            int iT33 = dr.GetOrdinal(this.T33);
            if (!dr.IsDBNull(iT33)) entity.T33 = Convert.ToInt32(dr.GetValue(iT33));

            int iT34 = dr.GetOrdinal(this.T34);
            if (!dr.IsDBNull(iT34)) entity.T34 = Convert.ToInt32(dr.GetValue(iT34));

            int iT35 = dr.GetOrdinal(this.T35);
            if (!dr.IsDBNull(iT35)) entity.T35 = Convert.ToInt32(dr.GetValue(iT35));

            int iT36 = dr.GetOrdinal(this.T36);
            if (!dr.IsDBNull(iT36)) entity.T36 = Convert.ToInt32(dr.GetValue(iT36));

            int iT37 = dr.GetOrdinal(this.T37);
            if (!dr.IsDBNull(iT37)) entity.T37 = Convert.ToInt32(dr.GetValue(iT37));

            int iT38 = dr.GetOrdinal(this.T38);
            if (!dr.IsDBNull(iT38)) entity.T38 = Convert.ToInt32(dr.GetValue(iT38));

            int iT39 = dr.GetOrdinal(this.T39);
            if (!dr.IsDBNull(iT39)) entity.T39 = Convert.ToInt32(dr.GetValue(iT39));

            int iT40 = dr.GetOrdinal(this.T40);
            if (!dr.IsDBNull(iT40)) entity.T40 = Convert.ToInt32(dr.GetValue(iT40));

            int iT41 = dr.GetOrdinal(this.T41);
            if (!dr.IsDBNull(iT41)) entity.T41 = Convert.ToInt32(dr.GetValue(iT41));

            int iT42 = dr.GetOrdinal(this.T42);
            if (!dr.IsDBNull(iT42)) entity.T42 = Convert.ToInt32(dr.GetValue(iT42));

            int iT43 = dr.GetOrdinal(this.T43);
            if (!dr.IsDBNull(iT43)) entity.T43 = Convert.ToInt32(dr.GetValue(iT43));

            int iT44 = dr.GetOrdinal(this.T44);
            if (!dr.IsDBNull(iT44)) entity.T44 = Convert.ToInt32(dr.GetValue(iT44));

            int iT45 = dr.GetOrdinal(this.T45);
            if (!dr.IsDBNull(iT45)) entity.T45 = Convert.ToInt32(dr.GetValue(iT45));

            int iT46 = dr.GetOrdinal(this.T46);
            if (!dr.IsDBNull(iT46)) entity.T46 = Convert.ToInt32(dr.GetValue(iT46));

            int iT47 = dr.GetOrdinal(this.T47);
            if (!dr.IsDBNull(iT47)) entity.T47 = Convert.ToInt32(dr.GetValue(iT47));

            int iT48 = dr.GetOrdinal(this.T48);
            if (!dr.IsDBNull(iT48)) entity.T48 = Convert.ToInt32(dr.GetValue(iT48));

            int iDyupcodi = dr.GetOrdinal(this.Dyupcodi);
            if (!dr.IsDBNull(iDyupcodi)) entity.Dyupcodi = Convert.ToInt32(dr.GetValue(iDyupcodi));

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            int iRecurcodi = dr.GetOrdinal(this.Recurcodi);
            if (!dr.IsDBNull(iRecurcodi)) entity.Recurcodi = Convert.ToInt32(dr.GetValue(iRecurcodi));

            int iCyupcodi = dr.GetOrdinal(this.Cyupcodi);
            if (!dr.IsDBNull(iCyupcodi)) entity.Cyupcodi = Convert.ToInt32(dr.GetValue(iCyupcodi));

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

            int iT1 = dr.GetOrdinal(this.T1);
            if (!dr.IsDBNull(iT1)) entity.T1 = Convert.ToInt32(dr.GetValue(iT1));

            int iT2 = dr.GetOrdinal(this.T2);
            if (!dr.IsDBNull(iT2)) entity.T2 = Convert.ToInt32(dr.GetValue(iT2));

            int iT3 = dr.GetOrdinal(this.T3);
            if (!dr.IsDBNull(iT3)) entity.T3 = Convert.ToInt32(dr.GetValue(iT3));

            int iT4 = dr.GetOrdinal(this.T4);
            if (!dr.IsDBNull(iT4)) entity.T4 = Convert.ToInt32(dr.GetValue(iT4));

            int iT5 = dr.GetOrdinal(this.T5);
            if (!dr.IsDBNull(iT5)) entity.T5 = Convert.ToInt32(dr.GetValue(iT5));

            int iT6 = dr.GetOrdinal(this.T6);
            if (!dr.IsDBNull(iT6)) entity.T6 = Convert.ToInt32(dr.GetValue(iT6));

            int iT7 = dr.GetOrdinal(this.T7);
            if (!dr.IsDBNull(iT7)) entity.T7 = Convert.ToInt32(dr.GetValue(iT7));

            int iT8 = dr.GetOrdinal(this.T8);
            if (!dr.IsDBNull(iT8)) entity.T8 = Convert.ToInt32(dr.GetValue(iT8));

            int iT9 = dr.GetOrdinal(this.T9);
            if (!dr.IsDBNull(iT9)) entity.T9 = Convert.ToInt32(dr.GetValue(iT9));

            int iT10 = dr.GetOrdinal(this.T10);
            if (!dr.IsDBNull(iT10)) entity.T10 = Convert.ToInt32(dr.GetValue(iT10));

            int iT11 = dr.GetOrdinal(this.T11);
            if (!dr.IsDBNull(iT11)) entity.T11 = Convert.ToInt32(dr.GetValue(iT11));

            int iT12 = dr.GetOrdinal(this.T12);
            if (!dr.IsDBNull(iT12)) entity.T12 = Convert.ToInt32(dr.GetValue(iT12));

            int iT13 = dr.GetOrdinal(this.T13);
            if (!dr.IsDBNull(iT13)) entity.T13 = Convert.ToInt32(dr.GetValue(iT13));

            int iT14 = dr.GetOrdinal(this.T14);
            if (!dr.IsDBNull(iT14)) entity.T14 = Convert.ToInt32(dr.GetValue(iT14));

            int iT15 = dr.GetOrdinal(this.T15);
            if (!dr.IsDBNull(iT15)) entity.T15 = Convert.ToInt32(dr.GetValue(iT15));

            int iT16 = dr.GetOrdinal(this.T16);
            if (!dr.IsDBNull(iT16)) entity.T16 = Convert.ToInt32(dr.GetValue(iT16));

            int iT17 = dr.GetOrdinal(this.T17);
            if (!dr.IsDBNull(iT17)) entity.T17 = Convert.ToInt32(dr.GetValue(iT17));

            int iT18 = dr.GetOrdinal(this.T18);
            if (!dr.IsDBNull(iT18)) entity.T18 = Convert.ToInt32(dr.GetValue(iT18));

            int iT19 = dr.GetOrdinal(this.T19);
            if (!dr.IsDBNull(iT19)) entity.T19 = Convert.ToInt32(dr.GetValue(iT19));

            int iT20 = dr.GetOrdinal(this.T20);
            if (!dr.IsDBNull(iT20)) entity.T20 = Convert.ToInt32(dr.GetValue(iT20));

            int iT21 = dr.GetOrdinal(this.T21);
            if (!dr.IsDBNull(iT21)) entity.T21 = Convert.ToInt32(dr.GetValue(iT21));

            int iT22 = dr.GetOrdinal(this.T22);
            if (!dr.IsDBNull(iT22)) entity.T22 = Convert.ToInt32(dr.GetValue(iT22));

            int iT23 = dr.GetOrdinal(this.T23);
            if (!dr.IsDBNull(iT23)) entity.T23 = Convert.ToInt32(dr.GetValue(iT23));

            int iT24 = dr.GetOrdinal(this.T24);
            if (!dr.IsDBNull(iT24)) entity.T24 = Convert.ToInt32(dr.GetValue(iT24));

            int iT25 = dr.GetOrdinal(this.T25);
            if (!dr.IsDBNull(iT25)) entity.T25 = Convert.ToInt32(dr.GetValue(iT25));

            int iT26 = dr.GetOrdinal(this.T26);
            if (!dr.IsDBNull(iT26)) entity.T26 = Convert.ToInt32(dr.GetValue(iT26));

            return entity;
        }

        #region Mapeo de Campos

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
        public string Dyupcodi = "DYUPCODI";
        public string Topcodi = "TOPCODI";
        public string Recurcodi = "RECURCODI";
        public string Cyupcodi = "CYUPCODI";
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

        public string Recurnombre = "RECURNOMBRE";
        public string Catcodi = "CATCODI";

        #endregion
    }
}
