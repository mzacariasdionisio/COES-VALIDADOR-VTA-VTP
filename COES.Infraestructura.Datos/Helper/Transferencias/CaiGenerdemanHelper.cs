using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CAI_GENERDEMAN
    /// </summary>
    public class CaiGenerdemanHelper : HelperBase
    {
        public CaiGenerdemanHelper(): base(Consultas.CaiGenerdemanSql)
        {
        }

        public CaiGenerdemanDTO Create(IDataReader dr)
        {
            CaiGenerdemanDTO entity = new CaiGenerdemanDTO();

            int iCagdcmcodi = dr.GetOrdinal(this.Cagdcmcodi);
            if (!dr.IsDBNull(iCagdcmcodi)) entity.Cagdcmcodi = Convert.ToInt32(dr.GetValue(iCagdcmcodi));

            int iCaiajcodi = dr.GetOrdinal(this.Caiajcodi);
            if (!dr.IsDBNull(iCaiajcodi)) entity.Caiajcodi = Convert.ToInt32(dr.GetValue(iCaiajcodi));

            int iCagdcmfuentedat = dr.GetOrdinal(this.Cagdcmfuentedat);
            if (!dr.IsDBNull(iCagdcmfuentedat)) entity.Cagdcmfuentedat = dr.GetString(iCagdcmfuentedat);

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodicen = dr.GetOrdinal(this.Equicodicen);
            if (!dr.IsDBNull(iEquicodicen)) entity.Equicodicen = Convert.ToInt32(dr.GetValue(iEquicodicen));

            int iEquicodiuni = dr.GetOrdinal(this.Equicodiuni);
            if (!dr.IsDBNull(iEquicodiuni)) entity.Equicodiuni = Convert.ToInt32(dr.GetValue(iEquicodiuni));

            int iCagdcmcalidadinfo = dr.GetOrdinal(this.Cagdcmcalidadinfo);
            if (!dr.IsDBNull(iCagdcmcalidadinfo)) entity.Cagdcmcalidadinfo = dr.GetString(iCagdcmcalidadinfo);

            int iCagdcmdia = dr.GetOrdinal(this.Cagdcmdia);
            if (!dr.IsDBNull(iCagdcmdia)) entity.Cagdcmdia = dr.GetDateTime(iCagdcmdia);

            int iCagdcmtotaldia = dr.GetOrdinal(this.Cagdcmtotaldia);
            if (!dr.IsDBNull(iCagdcmtotaldia)) entity.Cagdcmtotaldia = dr.GetDecimal(iCagdcmtotaldia);

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

            int iH60 = dr.GetOrdinal(this.H60);
            if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

            int iH61 = dr.GetOrdinal(this.H61);
            if (!dr.IsDBNull(iH61)) entity.H61 = dr.GetDecimal(iH61);

            int iH62 = dr.GetOrdinal(this.H62);
            if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

            int iH63 = dr.GetOrdinal(this.H63);
            if (!dr.IsDBNull(iH63)) entity.H63 = dr.GetDecimal(iH63);

            int iH64 = dr.GetOrdinal(this.H64);
            if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

            int iH65 = dr.GetOrdinal(this.H65);
            if (!dr.IsDBNull(iH65)) entity.H65 = dr.GetDecimal(iH65);

            int iH66 = dr.GetOrdinal(this.H66);
            if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

            int iH67 = dr.GetOrdinal(this.H67);
            if (!dr.IsDBNull(iH67)) entity.H67 = dr.GetDecimal(iH67);

            int iH68 = dr.GetOrdinal(this.H68);
            if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

            int iH69 = dr.GetOrdinal(this.H69);
            if (!dr.IsDBNull(iH69)) entity.H69 = dr.GetDecimal(iH69);

            int iH70 = dr.GetOrdinal(this.H70);
            if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

            int iH71 = dr.GetOrdinal(this.H71);
            if (!dr.IsDBNull(iH71)) entity.H71 = dr.GetDecimal(iH71);

            int iH72 = dr.GetOrdinal(this.H72);
            if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

            int iH73 = dr.GetOrdinal(this.H73);
            if (!dr.IsDBNull(iH73)) entity.H73 = dr.GetDecimal(iH73);

            int iH74 = dr.GetOrdinal(this.H74);
            if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

            int iH75 = dr.GetOrdinal(this.H75);
            if (!dr.IsDBNull(iH75)) entity.H75 = dr.GetDecimal(iH75);

            int iH76 = dr.GetOrdinal(this.H76);
            if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

            int iH77 = dr.GetOrdinal(this.H77);
            if (!dr.IsDBNull(iH77)) entity.H77 = dr.GetDecimal(iH77);

            int iH78 = dr.GetOrdinal(this.H78);
            if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

            int iH79 = dr.GetOrdinal(this.H79);
            if (!dr.IsDBNull(iH79)) entity.H79 = dr.GetDecimal(iH79);

            int iH80 = dr.GetOrdinal(this.H80);
            if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

            int iH81 = dr.GetOrdinal(this.H81);
            if (!dr.IsDBNull(iH81)) entity.H81 = dr.GetDecimal(iH81);

            int iH82 = dr.GetOrdinal(this.H82);
            if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

            int iH83 = dr.GetOrdinal(this.H83);
            if (!dr.IsDBNull(iH83)) entity.H83 = dr.GetDecimal(iH83);

            int iH84 = dr.GetOrdinal(this.H84);
            if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

            int iH85 = dr.GetOrdinal(this.H85);
            if (!dr.IsDBNull(iH85)) entity.H85 = dr.GetDecimal(iH85);

            int iH86 = dr.GetOrdinal(this.H86);
            if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

            int iH87 = dr.GetOrdinal(this.H87);
            if (!dr.IsDBNull(iH87)) entity.H87 = dr.GetDecimal(iH87);

            int iH88 = dr.GetOrdinal(this.H88);
            if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

            int iH89 = dr.GetOrdinal(this.H89);
            if (!dr.IsDBNull(iH89)) entity.H89 = dr.GetDecimal(iH89);

            int iH90 = dr.GetOrdinal(this.H90);
            if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

            int iH91 = dr.GetOrdinal(this.H91);
            if (!dr.IsDBNull(iH91)) entity.H91 = dr.GetDecimal(iH91);

            int iH92 = dr.GetOrdinal(this.H92);
            if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

            int iH93 = dr.GetOrdinal(this.H93);
            if (!dr.IsDBNull(iH93)) entity.H93 = dr.GetDecimal(iH93);

            int iH94 = dr.GetOrdinal(this.H94);
            if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

            int iH95 = dr.GetOrdinal(this.H95);
            if (!dr.IsDBNull(iH95)) entity.H95 = dr.GetDecimal(iH95);

            int iH96 = dr.GetOrdinal(this.H96);
            if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);

            int iT1 = dr.GetOrdinal(this.T1);
            if (!dr.IsDBNull(iT1)) entity.T1 = dr.GetString(iT1);

            int iT2 = dr.GetOrdinal(this.T2);
            if (!dr.IsDBNull(iT2)) entity.T2 = dr.GetString(iT2);

            int iT3 = dr.GetOrdinal(this.T3);
            if (!dr.IsDBNull(iT3)) entity.T3 = dr.GetString(iT3);

            int iT4 = dr.GetOrdinal(this.T4);
            if (!dr.IsDBNull(iT4)) entity.T4 = dr.GetString(iT4);

            int iT5 = dr.GetOrdinal(this.T5);
            if (!dr.IsDBNull(iT5)) entity.T5 = dr.GetString(iT5);

            int iT6 = dr.GetOrdinal(this.T6);
            if (!dr.IsDBNull(iT6)) entity.T6 = dr.GetString(iT6);

            int iT7 = dr.GetOrdinal(this.T7);
            if (!dr.IsDBNull(iT7)) entity.T7 = dr.GetString(iT7);

            int iT8 = dr.GetOrdinal(this.T8);
            if (!dr.IsDBNull(iT8)) entity.T8 = dr.GetString(iT8);

            int iT9 = dr.GetOrdinal(this.T9);
            if (!dr.IsDBNull(iT9)) entity.T9 = dr.GetString(iT9);

            int iT10 = dr.GetOrdinal(this.T10);
            if (!dr.IsDBNull(iT10)) entity.T10 = dr.GetString(iT10);

            int iT11 = dr.GetOrdinal(this.T11);
            if (!dr.IsDBNull(iT11)) entity.T11 = dr.GetString(iT11);

            int iT12 = dr.GetOrdinal(this.T12);
            if (!dr.IsDBNull(iT12)) entity.T12 = dr.GetString(iT12);

            int iT13 = dr.GetOrdinal(this.T13);
            if (!dr.IsDBNull(iT13)) entity.T13 = dr.GetString(iT13);

            int iT14 = dr.GetOrdinal(this.T14);
            if (!dr.IsDBNull(iT14)) entity.T14 = dr.GetString(iT14);

            int iT15 = dr.GetOrdinal(this.T15);
            if (!dr.IsDBNull(iT15)) entity.T15 = dr.GetString(iT15);

            int iT16 = dr.GetOrdinal(this.T16);
            if (!dr.IsDBNull(iT16)) entity.T16 = dr.GetString(iT16);

            int iT17 = dr.GetOrdinal(this.T17);
            if (!dr.IsDBNull(iT17)) entity.T17 = dr.GetString(iT17);

            int iT18 = dr.GetOrdinal(this.T18);
            if (!dr.IsDBNull(iT18)) entity.T18 = dr.GetString(iT18);

            int iT19 = dr.GetOrdinal(this.T19);
            if (!dr.IsDBNull(iT19)) entity.T19 = dr.GetString(iT19);

            int iT20 = dr.GetOrdinal(this.T20);
            if (!dr.IsDBNull(iT20)) entity.T20 = dr.GetString(iT20);

            int iT21 = dr.GetOrdinal(this.T21);
            if (!dr.IsDBNull(iT21)) entity.T21 = dr.GetString(iT21);

            int iT22 = dr.GetOrdinal(this.T22);
            if (!dr.IsDBNull(iT22)) entity.T22 = dr.GetString(iT22);

            int iT23 = dr.GetOrdinal(this.T23);
            if (!dr.IsDBNull(iT23)) entity.T23 = dr.GetString(iT23);

            int iT24 = dr.GetOrdinal(this.T24);
            if (!dr.IsDBNull(iT24)) entity.T24 = dr.GetString(iT24);

            int iT25 = dr.GetOrdinal(this.T25);
            if (!dr.IsDBNull(iT25)) entity.T25 = dr.GetString(iT25);

            int iT26 = dr.GetOrdinal(this.T26);
            if (!dr.IsDBNull(iT26)) entity.T26 = dr.GetString(iT26);

            int iT27 = dr.GetOrdinal(this.T27);
            if (!dr.IsDBNull(iT27)) entity.T27 = dr.GetString(iT27);

            int iT28 = dr.GetOrdinal(this.T28);
            if (!dr.IsDBNull(iT28)) entity.T28 = dr.GetString(iT28);

            int iT29 = dr.GetOrdinal(this.T29);
            if (!dr.IsDBNull(iT29)) entity.T29 = dr.GetString(iT29);

            int iT30 = dr.GetOrdinal(this.T30);
            if (!dr.IsDBNull(iT30)) entity.T30 = dr.GetString(iT30);

            int iT31 = dr.GetOrdinal(this.T31);
            if (!dr.IsDBNull(iT31)) entity.T31 = dr.GetString(iT31);

            int iT32 = dr.GetOrdinal(this.T32);
            if (!dr.IsDBNull(iT32)) entity.T32 = dr.GetString(iT32);

            int iT33 = dr.GetOrdinal(this.T33);
            if (!dr.IsDBNull(iT33)) entity.T33 = dr.GetString(iT33);

            int iT34 = dr.GetOrdinal(this.T34);
            if (!dr.IsDBNull(iT34)) entity.T34 = dr.GetString(iT34);

            int iT35 = dr.GetOrdinal(this.T35);
            if (!dr.IsDBNull(iT35)) entity.T35 = dr.GetString(iT35);

            int iT36 = dr.GetOrdinal(this.T36);
            if (!dr.IsDBNull(iT36)) entity.T36 = dr.GetString(iT36);

            int iT37 = dr.GetOrdinal(this.T37);
            if (!dr.IsDBNull(iT37)) entity.T37 = dr.GetString(iT37);

            int iT38 = dr.GetOrdinal(this.T38);
            if (!dr.IsDBNull(iT38)) entity.T38 = dr.GetString(iT38);

            int iT39 = dr.GetOrdinal(this.T39);
            if (!dr.IsDBNull(iT39)) entity.T39 = dr.GetString(iT39);

            int iT40 = dr.GetOrdinal(this.T40);
            if (!dr.IsDBNull(iT40)) entity.T40 = dr.GetString(iT40);

            int iT41 = dr.GetOrdinal(this.T41);
            if (!dr.IsDBNull(iT41)) entity.T41 = dr.GetString(iT41);

            int iT42 = dr.GetOrdinal(this.T42);
            if (!dr.IsDBNull(iT42)) entity.T42 = dr.GetString(iT42);

            int iT43 = dr.GetOrdinal(this.T43);
            if (!dr.IsDBNull(iT43)) entity.T43 = dr.GetString(iT43);

            int iT44 = dr.GetOrdinal(this.T44);
            if (!dr.IsDBNull(iT44)) entity.T44 = dr.GetString(iT44);

            int iT45 = dr.GetOrdinal(this.T45);
            if (!dr.IsDBNull(iT45)) entity.T45 = dr.GetString(iT45);

            int iT46 = dr.GetOrdinal(this.T46);
            if (!dr.IsDBNull(iT46)) entity.T46 = dr.GetString(iT46);

            int iT47 = dr.GetOrdinal(this.T47);
            if (!dr.IsDBNull(iT47)) entity.T47 = dr.GetString(iT47);

            int iT48 = dr.GetOrdinal(this.T48);
            if (!dr.IsDBNull(iT48)) entity.T48 = dr.GetString(iT48);

            int iT49 = dr.GetOrdinal(this.T49);
            if (!dr.IsDBNull(iT49)) entity.T49 = dr.GetString(iT49);

            int iT50 = dr.GetOrdinal(this.T50);
            if (!dr.IsDBNull(iT50)) entity.T50 = dr.GetString(iT50);

            int iT51 = dr.GetOrdinal(this.T51);
            if (!dr.IsDBNull(iT51)) entity.T51 = dr.GetString(iT51);

            int iT52 = dr.GetOrdinal(this.T52);
            if (!dr.IsDBNull(iT52)) entity.T52 = dr.GetString(iT52);

            int iT53 = dr.GetOrdinal(this.T53);
            if (!dr.IsDBNull(iT53)) entity.T53 = dr.GetString(iT53);

            int iT54 = dr.GetOrdinal(this.T54);
            if (!dr.IsDBNull(iT54)) entity.T54 = dr.GetString(iT54);

            int iT55 = dr.GetOrdinal(this.T55);
            if (!dr.IsDBNull(iT55)) entity.T55 = dr.GetString(iT55);

            int iT56 = dr.GetOrdinal(this.T56);
            if (!dr.IsDBNull(iT56)) entity.T56 = dr.GetString(iT56);

            int iT57 = dr.GetOrdinal(this.T57);
            if (!dr.IsDBNull(iT57)) entity.T57 = dr.GetString(iT57);

            int iT58 = dr.GetOrdinal(this.T58);
            if (!dr.IsDBNull(iT58)) entity.T58 = dr.GetString(iT58);

            int iT59 = dr.GetOrdinal(this.T59);
            if (!dr.IsDBNull(iT59)) entity.T59 = dr.GetString(iT59);

            int iT60 = dr.GetOrdinal(this.T60);
            if (!dr.IsDBNull(iT60)) entity.T60 = dr.GetString(iT60);

            int iT61 = dr.GetOrdinal(this.T61);
            if (!dr.IsDBNull(iT61)) entity.T61 = dr.GetString(iT61);

            int iT62 = dr.GetOrdinal(this.T62);
            if (!dr.IsDBNull(iT62)) entity.T62 = dr.GetString(iT62);

            int iT63 = dr.GetOrdinal(this.T63);
            if (!dr.IsDBNull(iT63)) entity.T63 = dr.GetString(iT63);

            int iT64 = dr.GetOrdinal(this.T64);
            if (!dr.IsDBNull(iT64)) entity.T64 = dr.GetString(iT64);

            int iT65 = dr.GetOrdinal(this.T65);
            if (!dr.IsDBNull(iT65)) entity.T65 = dr.GetString(iT65);

            int iT66 = dr.GetOrdinal(this.T66);
            if (!dr.IsDBNull(iT66)) entity.T66 = dr.GetString(iT66);

            int iT67 = dr.GetOrdinal(this.T67);
            if (!dr.IsDBNull(iT67)) entity.T67 = dr.GetString(iT67);

            int iT68 = dr.GetOrdinal(this.T68);
            if (!dr.IsDBNull(iT68)) entity.T68 = dr.GetString(iT68);

            int iT69 = dr.GetOrdinal(this.T69);
            if (!dr.IsDBNull(iT69)) entity.T69 = dr.GetString(iT69);

            int iT70 = dr.GetOrdinal(this.T70);
            if (!dr.IsDBNull(iT70)) entity.T70 = dr.GetString(iT70);

            int iT71 = dr.GetOrdinal(this.T71);
            if (!dr.IsDBNull(iT71)) entity.T71 = dr.GetString(iT71);

            int iT72 = dr.GetOrdinal(this.T72);
            if (!dr.IsDBNull(iT72)) entity.T72 = dr.GetString(iT72);

            int iT73 = dr.GetOrdinal(this.T73);
            if (!dr.IsDBNull(iT73)) entity.T73 = dr.GetString(iT73);

            int iT74 = dr.GetOrdinal(this.T74);
            if (!dr.IsDBNull(iT74)) entity.T74 = dr.GetString(iT74);

            int iT75 = dr.GetOrdinal(this.T75);
            if (!dr.IsDBNull(iT75)) entity.T75 = dr.GetString(iT75);

            int iT76 = dr.GetOrdinal(this.T76);
            if (!dr.IsDBNull(iT76)) entity.T76 = dr.GetString(iT76);

            int iT77 = dr.GetOrdinal(this.T77);
            if (!dr.IsDBNull(iT77)) entity.T77 = dr.GetString(iT77);

            int iT78 = dr.GetOrdinal(this.T78);
            if (!dr.IsDBNull(iT78)) entity.T78 = dr.GetString(iT78);

            int iT79 = dr.GetOrdinal(this.T79);
            if (!dr.IsDBNull(iT79)) entity.T79 = dr.GetString(iT79);

            int iT80 = dr.GetOrdinal(this.T80);
            if (!dr.IsDBNull(iT80)) entity.T80 = dr.GetString(iT80);

            int iT81 = dr.GetOrdinal(this.T81);
            if (!dr.IsDBNull(iT81)) entity.T81 = dr.GetString(iT81);

            int iT82 = dr.GetOrdinal(this.T82);
            if (!dr.IsDBNull(iT82)) entity.T82 = dr.GetString(iT82);

            int iT83 = dr.GetOrdinal(this.T83);
            if (!dr.IsDBNull(iT83)) entity.T83 = dr.GetString(iT83);

            int iT84 = dr.GetOrdinal(this.T84);
            if (!dr.IsDBNull(iT84)) entity.T84 = dr.GetString(iT84);

            int iT85 = dr.GetOrdinal(this.T85);
            if (!dr.IsDBNull(iT85)) entity.T85 = dr.GetString(iT85);

            int iT86 = dr.GetOrdinal(this.T86);
            if (!dr.IsDBNull(iT86)) entity.T86 = dr.GetString(iT86);

            int iT87 = dr.GetOrdinal(this.T87);
            if (!dr.IsDBNull(iT87)) entity.T87 = dr.GetString(iT87);

            int iT88 = dr.GetOrdinal(this.T88);
            if (!dr.IsDBNull(iT88)) entity.T88 = dr.GetString(iT88);

            int iT89 = dr.GetOrdinal(this.T89);
            if (!dr.IsDBNull(iT89)) entity.T89 = dr.GetString(iT89);

            int iT90 = dr.GetOrdinal(this.T90);
            if (!dr.IsDBNull(iT90)) entity.T90 = dr.GetString(iT90);

            int iT91 = dr.GetOrdinal(this.T91);
            if (!dr.IsDBNull(iT91)) entity.T91 = dr.GetString(iT91);

            int iT92 = dr.GetOrdinal(this.T92);
            if (!dr.IsDBNull(iT92)) entity.T92 = dr.GetString(iT92);

            int iT93 = dr.GetOrdinal(this.T93);
            if (!dr.IsDBNull(iT93)) entity.T93 = dr.GetString(iT93);

            int iT94 = dr.GetOrdinal(this.T94);
            if (!dr.IsDBNull(iT94)) entity.T94 = dr.GetString(iT94);

            int iT95 = dr.GetOrdinal(this.T95);
            if (!dr.IsDBNull(iT95)) entity.T95 = dr.GetString(iT95);

            int iT96 = dr.GetOrdinal(this.T96);
            if (!dr.IsDBNull(iT96)) entity.T96 = dr.GetString(iT96);

            int iCagdcmusucreacion = dr.GetOrdinal(this.Cagdcmusucreacion);
            if (!dr.IsDBNull(iCagdcmusucreacion)) entity.Cagdcmusucreacion = dr.GetString(iCagdcmusucreacion);

            int iCagdcmfeccreacion = dr.GetOrdinal(this.Cagdcmfeccreacion);
            if (!dr.IsDBNull(iCagdcmfeccreacion)) entity.Cagdcmfeccreacion = dr.GetDateTime(iCagdcmfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Cagdcmcodi = "CAGDCMCODI";
        public string Caiajcodi = "CAIAJCODI";
        public string Cagdcmfuentedat = "CAGDCMFUENTEDAT";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodicen = "EQUICODICEN";
        public string Equicodiuni = "EQUICODIUNI";
        public string Cagdcmcalidadinfo = "CAGDCMCALIDADINFO";
        public string Cagdcmdia = "CAGDCMDIA";
        public string Cagdcmtotaldia = "CAGDCMTOTALDIA";
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
        public string H60 = "H60";
        public string H61 = "H61";
        public string H62 = "H62";
        public string H63 = "H63";
        public string H64 = "H64";
        public string H65 = "H65";
        public string H66 = "H66";
        public string H67 = "H67";
        public string H68 = "H68";
        public string H69 = "H69";
        public string H70 = "H70";
        public string H71 = "H71";
        public string H72 = "H72";
        public string H73 = "H73";
        public string H74 = "H74";
        public string H75 = "H75";
        public string H76 = "H76";
        public string H77 = "H77";
        public string H78 = "H78";
        public string H79 = "H79";
        public string H80 = "H80";
        public string H81 = "H81";
        public string H82 = "H82";
        public string H83 = "H83";
        public string H84 = "H84";
        public string H85 = "H85";
        public string H86 = "H86";
        public string H87 = "H87";
        public string H88 = "H88";
        public string H89 = "H89";
        public string H90 = "H90";
        public string H91 = "H91";
        public string H92 = "H92";
        public string H93 = "H93";
        public string H94 = "H94";
        public string H95 = "H95";
        public string H96 = "H96";
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
        public string T49 = "T49";
        public string T50 = "T50";
        public string T51 = "T51";
        public string T52 = "T52";
        public string T53 = "T53";
        public string T54 = "T54";
        public string T55 = "T55";
        public string T56 = "T56";
        public string T57 = "T57";
        public string T58 = "T58";
        public string T59 = "T59";
        public string T60 = "T60";
        public string T61 = "T61";
        public string T62 = "T62";
        public string T63 = "T63";
        public string T64 = "T64";
        public string T65 = "T65";
        public string T66 = "T66";
        public string T67 = "T67";
        public string T68 = "T68";
        public string T69 = "T69";
        public string T70 = "T70";
        public string T71 = "T71";
        public string T72 = "T72";
        public string T73 = "T73";
        public string T74 = "T74";
        public string T75 = "T75";
        public string T76 = "T76";
        public string T77 = "T77";
        public string T78 = "T78";
        public string T79 = "T79";
        public string T80 = "T80";
        public string T81 = "T81";
        public string T82 = "T82";
        public string T83 = "T83";
        public string T84 = "T84";
        public string T85 = "T85";
        public string T86 = "T86";
        public string T87 = "T87";
        public string T88 = "T88";
        public string T89 = "T89";
        public string T90 = "T90";
        public string T91 = "T91";
        public string T92 = "T92";
        public string T93 = "T93";
        public string T94 = "T94";
        public string T95 = "T95";
        public string T96 = "T96";
        public string Cagdcmusucreacion = "CAGDCMUSUCREACION";
        public string Cagdcmfeccreacion = "CAGDCMFECCREACION";

        public string TableName = "CAI_GENERDEMAN";
        //Atributos que se usan para consultas
        public string sFechaInicio = "FECHA_INICIO";
        public string sFechaFin = "FECHA_FIN";
        public string CM1 = "CM1";
        public string CM2 = "CM2";
        public string CM3 = "CM3";
        public string CM4 = "CM4";
        public string CM5 = "CM5";
        public string CM6 = "CM6";
        public string CM7 = "CM7";
        public string CM8 = "CM8";
        public string CM9 = "CM9";
        public string CM10 = "CM10";
        public string CM11 = "CM11";
        public string CM12 = "CM12";
        public string CM13 = "CM13";
        public string CM14 = "CM14";
        public string CM15 = "CM15";
        public string CM16 = "CM16";
        public string CM17 = "CM17";
        public string CM18 = "CM18";
        public string CM19 = "CM19";
        public string CM20 = "CM20";
        public string CM21 = "CM21";
        public string CM22 = "CM22";
        public string CM23 = "CM23";
        public string CM24 = "CM24";
        public string CM25 = "CM25";
        public string CM26 = "CM26";
        public string CM27 = "CM27";
        public string CM28 = "CM28";
        public string CM29 = "CM29";
        public string CM30 = "CM30";
        public string CM31 = "CM31";
        public string CM32 = "CM32";
        public string CM33 = "CM33";
        public string CM34 = "CM34";
        public string CM35 = "CM35";
        public string CM36 = "CM36";
        public string CM37 = "CM37";
        public string CM38 = "CM38";
        public string CM39 = "CM39";
        public string CM40 = "CM40";
        public string CM41 = "CM41";
        public string CM42 = "CM42";
        public string CM43 = "CM43";
        public string CM44 = "CM44";
        public string CM45 = "CM45";
        public string CM46 = "CM46";
        public string CM47 = "CM47";
        public string CM48 = "CM48";
        public string CM49 = "CM49";
        public string CM50 = "CM50";
        public string CM51 = "CM51";
        public string CM52 = "CM52";
        public string CM53 = "CM53";
        public string CM54 = "CM54";
        public string CM55 = "CM55";
        public string CM56 = "CM56";
        public string CM57 = "CM57";
        public string CM58 = "CM58";
        public string CM59 = "CM59";
        public string CM60 = "CM60";
        public string CM61 = "CM61";
        public string CM62 = "CM62";
        public string CM63 = "CM63";
        public string CM64 = "CM64";
        public string CM65 = "CM65";
        public string CM66 = "CM66";
        public string CM67 = "CM67";
        public string CM68 = "CM68";
        public string CM69 = "CM69";
        public string CM70 = "CM70";
        public string CM71 = "CM71";
        public string CM72 = "CM72";
        public string CM73 = "CM73";
        public string CM74 = "CM74";
        public string CM75 = "CM75";
        public string CM76 = "CM76";
        public string CM77 = "CM77";
        public string CM78 = "CM78";
        public string CM79 = "CM79";
        public string CM80 = "CM80";
        public string CM81 = "CM81";
        public string CM82 = "CM82";
        public string CM83 = "CM83";
        public string CM84 = "CM84";
        public string CM85 = "CM85";
        public string CM86 = "CM86";
        public string CM87 = "CM87";
        public string CM88 = "CM88";
        public string CM89 = "CM89";
        public string CM90 = "CM90";
        public string CM91 = "CM91";
        public string CM92 = "CM92";
        public string CM93 = "CM93";
        public string CM94 = "CM94";
        public string CM95 = "CM95";
        public string CM96 = "CM96";
        #endregion

        public string SqlGetByUsuarioLibresSGOCOES
        {
            get { return base.GetSqlXml("GetByUsuarioLibresSGOCOES"); }
        }

        public string SqlListGenDemBarrMes
        {
            get { return base.GetSqlXml("ListGenDemBarrMes"); }
        }

        public string SqlListGenDemProyMes
        {
            get { return base.GetSqlXml("ListGenDemProyMes"); }
        }

        public string SqlSaveAsSelectUsuariosLibres
        {
            get { return base.GetSqlXml("SaveAsSelectUsuariosLibres"); }
        }

        public string SqlSaveAsSelectMeMedicion96
        {
            get { return base.GetSqlXml("SaveAsSelectMeMedicion96"); }
        }

    }
}
