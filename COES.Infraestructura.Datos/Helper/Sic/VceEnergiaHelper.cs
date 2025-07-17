using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCE_ENERGIA
    /// </summary>
    public class VceEnergiaHelper : HelperBase
    {
        public VceEnergiaHelper()
            : base(Consultas.VceEnergiaSql)
        {
        }

        public VceEnergiaDTO Create(IDataReader dr)
        {
            VceEnergiaDTO entity = new VceEnergiaDTO();

            int iCrmeh47 = dr.GetOrdinal(this.Crmeh47);
            if (!dr.IsDBNull(iCrmeh47)) entity.Crmeh47 = dr.GetDecimal(iCrmeh47);

            int iCrmeh46 = dr.GetOrdinal(this.Crmeh46);
            if (!dr.IsDBNull(iCrmeh46)) entity.Crmeh46 = dr.GetDecimal(iCrmeh46);

            int iCrmeh45 = dr.GetOrdinal(this.Crmeh45);
            if (!dr.IsDBNull(iCrmeh45)) entity.Crmeh45 = dr.GetDecimal(iCrmeh45);

            int iCrmeh44 = dr.GetOrdinal(this.Crmeh44);
            if (!dr.IsDBNull(iCrmeh44)) entity.Crmeh44 = dr.GetDecimal(iCrmeh44);

            int iCrmeh43 = dr.GetOrdinal(this.Crmeh43);
            if (!dr.IsDBNull(iCrmeh43)) entity.Crmeh43 = dr.GetDecimal(iCrmeh43);

            int iCrmeh42 = dr.GetOrdinal(this.Crmeh42);
            if (!dr.IsDBNull(iCrmeh42)) entity.Crmeh42 = dr.GetDecimal(iCrmeh42);

            int iCrmeh41 = dr.GetOrdinal(this.Crmeh41);
            if (!dr.IsDBNull(iCrmeh41)) entity.Crmeh41 = dr.GetDecimal(iCrmeh41);

            int iCrmeh40 = dr.GetOrdinal(this.Crmeh40);
            if (!dr.IsDBNull(iCrmeh40)) entity.Crmeh40 = dr.GetDecimal(iCrmeh40);

            int iCrmeh39 = dr.GetOrdinal(this.Crmeh39);
            if (!dr.IsDBNull(iCrmeh39)) entity.Crmeh39 = dr.GetDecimal(iCrmeh39);

            int iCrmeh38 = dr.GetOrdinal(this.Crmeh38);
            if (!dr.IsDBNull(iCrmeh38)) entity.Crmeh38 = dr.GetDecimal(iCrmeh38);

            int iCrmeh37 = dr.GetOrdinal(this.Crmeh37);
            if (!dr.IsDBNull(iCrmeh37)) entity.Crmeh37 = dr.GetDecimal(iCrmeh37);

            int iCrmeh36 = dr.GetOrdinal(this.Crmeh36);
            if (!dr.IsDBNull(iCrmeh36)) entity.Crmeh36 = dr.GetDecimal(iCrmeh36);

            int iCrmeh35 = dr.GetOrdinal(this.Crmeh35);
            if (!dr.IsDBNull(iCrmeh35)) entity.Crmeh35 = dr.GetDecimal(iCrmeh35);

            int iCrmeh34 = dr.GetOrdinal(this.Crmeh34);
            if (!dr.IsDBNull(iCrmeh34)) entity.Crmeh34 = dr.GetDecimal(iCrmeh34);

            int iCrmeh33 = dr.GetOrdinal(this.Crmeh33);
            if (!dr.IsDBNull(iCrmeh33)) entity.Crmeh33 = dr.GetDecimal(iCrmeh33);

            int iCrmeh32 = dr.GetOrdinal(this.Crmeh32);
            if (!dr.IsDBNull(iCrmeh32)) entity.Crmeh32 = dr.GetDecimal(iCrmeh32);

            int iCrmeh31 = dr.GetOrdinal(this.Crmeh31);
            if (!dr.IsDBNull(iCrmeh31)) entity.Crmeh31 = dr.GetDecimal(iCrmeh31);

            int iCrmeh30 = dr.GetOrdinal(this.Crmeh30);
            if (!dr.IsDBNull(iCrmeh30)) entity.Crmeh30 = dr.GetDecimal(iCrmeh30);

            int iCrmeh29 = dr.GetOrdinal(this.Crmeh29);
            if (!dr.IsDBNull(iCrmeh29)) entity.Crmeh29 = dr.GetDecimal(iCrmeh29);

            int iCrmeh28 = dr.GetOrdinal(this.Crmeh28);
            if (!dr.IsDBNull(iCrmeh28)) entity.Crmeh28 = dr.GetDecimal(iCrmeh28);

            int iCrmeh27 = dr.GetOrdinal(this.Crmeh27);
            if (!dr.IsDBNull(iCrmeh27)) entity.Crmeh27 = dr.GetDecimal(iCrmeh27);

            int iCrmeh26 = dr.GetOrdinal(this.Crmeh26);
            if (!dr.IsDBNull(iCrmeh26)) entity.Crmeh26 = dr.GetDecimal(iCrmeh26);

            int iCrmeh25 = dr.GetOrdinal(this.Crmeh25);
            if (!dr.IsDBNull(iCrmeh25)) entity.Crmeh25 = dr.GetDecimal(iCrmeh25);

            int iCrmeh24 = dr.GetOrdinal(this.Crmeh24);
            if (!dr.IsDBNull(iCrmeh24)) entity.Crmeh24 = dr.GetDecimal(iCrmeh24);

            int iCrmeh23 = dr.GetOrdinal(this.Crmeh23);
            if (!dr.IsDBNull(iCrmeh23)) entity.Crmeh23 = dr.GetDecimal(iCrmeh23);

            int iCrmeh22 = dr.GetOrdinal(this.Crmeh22);
            if (!dr.IsDBNull(iCrmeh22)) entity.Crmeh22 = dr.GetDecimal(iCrmeh22);

            int iCrmeh21 = dr.GetOrdinal(this.Crmeh21);
            if (!dr.IsDBNull(iCrmeh21)) entity.Crmeh21 = dr.GetDecimal(iCrmeh21);

            int iCrmeh20 = dr.GetOrdinal(this.Crmeh20);
            if (!dr.IsDBNull(iCrmeh20)) entity.Crmeh20 = dr.GetDecimal(iCrmeh20);

            int iCrmeh19 = dr.GetOrdinal(this.Crmeh19);
            if (!dr.IsDBNull(iCrmeh19)) entity.Crmeh19 = dr.GetDecimal(iCrmeh19);

            int iCrmeh18 = dr.GetOrdinal(this.Crmeh18);
            if (!dr.IsDBNull(iCrmeh18)) entity.Crmeh18 = dr.GetDecimal(iCrmeh18);

            int iCrmeh17 = dr.GetOrdinal(this.Crmeh17);
            if (!dr.IsDBNull(iCrmeh17)) entity.Crmeh17 = dr.GetDecimal(iCrmeh17);

            int iCrmeh16 = dr.GetOrdinal(this.Crmeh16);
            if (!dr.IsDBNull(iCrmeh16)) entity.Crmeh16 = dr.GetDecimal(iCrmeh16);

            int iCrmeh15 = dr.GetOrdinal(this.Crmeh15);
            if (!dr.IsDBNull(iCrmeh15)) entity.Crmeh15 = dr.GetDecimal(iCrmeh15);

            int iCrmeh14 = dr.GetOrdinal(this.Crmeh14);
            if (!dr.IsDBNull(iCrmeh14)) entity.Crmeh14 = dr.GetDecimal(iCrmeh14);

            int iCrmeh13 = dr.GetOrdinal(this.Crmeh13);
            if (!dr.IsDBNull(iCrmeh13)) entity.Crmeh13 = dr.GetDecimal(iCrmeh13);

            int iCrmeh12 = dr.GetOrdinal(this.Crmeh12);
            if (!dr.IsDBNull(iCrmeh12)) entity.Crmeh12 = dr.GetDecimal(iCrmeh12);

            int iCrmeh11 = dr.GetOrdinal(this.Crmeh11);
            if (!dr.IsDBNull(iCrmeh11)) entity.Crmeh11 = dr.GetDecimal(iCrmeh11);

            int iCrmeh10 = dr.GetOrdinal(this.Crmeh10);
            if (!dr.IsDBNull(iCrmeh10)) entity.Crmeh10 = dr.GetDecimal(iCrmeh10);

            int iCrmeh9 = dr.GetOrdinal(this.Crmeh9);
            if (!dr.IsDBNull(iCrmeh9)) entity.Crmeh9 = dr.GetDecimal(iCrmeh9);

            int iCrmeh8 = dr.GetOrdinal(this.Crmeh8);
            if (!dr.IsDBNull(iCrmeh8)) entity.Crmeh8 = dr.GetDecimal(iCrmeh8);

            int iCrmeh7 = dr.GetOrdinal(this.Crmeh7);
            if (!dr.IsDBNull(iCrmeh7)) entity.Crmeh7 = dr.GetDecimal(iCrmeh7);

            int iCrmeh6 = dr.GetOrdinal(this.Crmeh6);
            if (!dr.IsDBNull(iCrmeh6)) entity.Crmeh6 = dr.GetDecimal(iCrmeh6);

            int iCrmeh5 = dr.GetOrdinal(this.Crmeh5);
            if (!dr.IsDBNull(iCrmeh5)) entity.Crmeh5 = dr.GetDecimal(iCrmeh5);

            int iCrmeh4 = dr.GetOrdinal(this.Crmeh4);
            if (!dr.IsDBNull(iCrmeh4)) entity.Crmeh4 = dr.GetDecimal(iCrmeh4);

            int iCrmeh3 = dr.GetOrdinal(this.Crmeh3);
            if (!dr.IsDBNull(iCrmeh3)) entity.Crmeh3 = dr.GetDecimal(iCrmeh3);

            int iCrmeh2 = dr.GetOrdinal(this.Crmeh2);
            if (!dr.IsDBNull(iCrmeh2)) entity.Crmeh2 = dr.GetDecimal(iCrmeh2);

            int iCrmeh1 = dr.GetOrdinal(this.Crmeh1);
            if (!dr.IsDBNull(iCrmeh1)) entity.Crmeh1 = dr.GetDecimal(iCrmeh1);

            int iCrmemeditotal = dr.GetOrdinal(this.Crmemeditotal);
            if (!dr.IsDBNull(iCrmemeditotal)) entity.Crmemeditotal = dr.GetDecimal(iCrmemeditotal);

            int iCrmefecha = dr.GetOrdinal(this.Crmefecha);
            if (!dr.IsDBNull(iCrmefecha)) entity.Crmefecha = dr.GetDateTime(iCrmefecha);

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iPecacodi = dr.GetOrdinal(this.Pecacodi);
            if (!dr.IsDBNull(iPecacodi)) entity.PecaCodi = Convert.ToInt32(dr.GetValue(iPecacodi));

            int iCrmeh96 = dr.GetOrdinal(this.Crmeh96);
            if (!dr.IsDBNull(iCrmeh96)) entity.Crmeh96 = dr.GetDecimal(iCrmeh96);

            int iCrmeh95 = dr.GetOrdinal(this.Crmeh95);
            if (!dr.IsDBNull(iCrmeh95)) entity.Crmeh95 = dr.GetDecimal(iCrmeh95);

            int iCrmeh94 = dr.GetOrdinal(this.Crmeh94);
            if (!dr.IsDBNull(iCrmeh94)) entity.Crmeh94 = dr.GetDecimal(iCrmeh94);

            int iCrmeh93 = dr.GetOrdinal(this.Crmeh93);
            if (!dr.IsDBNull(iCrmeh93)) entity.Crmeh93 = dr.GetDecimal(iCrmeh93);

            int iCrmeh92 = dr.GetOrdinal(this.Crmeh92);
            if (!dr.IsDBNull(iCrmeh92)) entity.Crmeh92 = dr.GetDecimal(iCrmeh92);

            int iCrmeh91 = dr.GetOrdinal(this.Crmeh91);
            if (!dr.IsDBNull(iCrmeh91)) entity.Crmeh91 = dr.GetDecimal(iCrmeh91);

            int iCrmeh90 = dr.GetOrdinal(this.Crmeh90);
            if (!dr.IsDBNull(iCrmeh90)) entity.Crmeh90 = dr.GetDecimal(iCrmeh90);

            int iCrmeh89 = dr.GetOrdinal(this.Crmeh89);
            if (!dr.IsDBNull(iCrmeh89)) entity.Crmeh89 = dr.GetDecimal(iCrmeh89);

            int iCrmeh88 = dr.GetOrdinal(this.Crmeh88);
            if (!dr.IsDBNull(iCrmeh88)) entity.Crmeh88 = dr.GetDecimal(iCrmeh88);

            int iCrmeh87 = dr.GetOrdinal(this.Crmeh87);
            if (!dr.IsDBNull(iCrmeh87)) entity.Crmeh87 = dr.GetDecimal(iCrmeh87);

            int iCrmeh86 = dr.GetOrdinal(this.Crmeh86);
            if (!dr.IsDBNull(iCrmeh86)) entity.Crmeh86 = dr.GetDecimal(iCrmeh86);

            int iCrmeh85 = dr.GetOrdinal(this.Crmeh85);
            if (!dr.IsDBNull(iCrmeh85)) entity.Crmeh85 = dr.GetDecimal(iCrmeh85);

            int iCrmeh84 = dr.GetOrdinal(this.Crmeh84);
            if (!dr.IsDBNull(iCrmeh84)) entity.Crmeh84 = dr.GetDecimal(iCrmeh84);

            int iCrmeh83 = dr.GetOrdinal(this.Crmeh83);
            if (!dr.IsDBNull(iCrmeh83)) entity.Crmeh83 = dr.GetDecimal(iCrmeh83);

            int iCrmeh82 = dr.GetOrdinal(this.Crmeh82);
            if (!dr.IsDBNull(iCrmeh82)) entity.Crmeh82 = dr.GetDecimal(iCrmeh82);

            int iCrmeh81 = dr.GetOrdinal(this.Crmeh81);
            if (!dr.IsDBNull(iCrmeh81)) entity.Crmeh81 = dr.GetDecimal(iCrmeh81);

            int iCrmeh80 = dr.GetOrdinal(this.Crmeh80);
            if (!dr.IsDBNull(iCrmeh80)) entity.Crmeh80 = dr.GetDecimal(iCrmeh80);

            int iCrmeh79 = dr.GetOrdinal(this.Crmeh79);
            if (!dr.IsDBNull(iCrmeh79)) entity.Crmeh79 = dr.GetDecimal(iCrmeh79);

            int iCrmeh78 = dr.GetOrdinal(this.Crmeh78);
            if (!dr.IsDBNull(iCrmeh78)) entity.Crmeh78 = dr.GetDecimal(iCrmeh78);

            int iCrmeh77 = dr.GetOrdinal(this.Crmeh77);
            if (!dr.IsDBNull(iCrmeh77)) entity.Crmeh77 = dr.GetDecimal(iCrmeh77);

            int iCrmeh76 = dr.GetOrdinal(this.Crmeh76);
            if (!dr.IsDBNull(iCrmeh76)) entity.Crmeh76 = dr.GetDecimal(iCrmeh76);

            int iCrmeh75 = dr.GetOrdinal(this.Crmeh75);
            if (!dr.IsDBNull(iCrmeh75)) entity.Crmeh75 = dr.GetDecimal(iCrmeh75);

            int iCrmeh74 = dr.GetOrdinal(this.Crmeh74);
            if (!dr.IsDBNull(iCrmeh74)) entity.Crmeh74 = dr.GetDecimal(iCrmeh74);

            int iCrmeh73 = dr.GetOrdinal(this.Crmeh73);
            if (!dr.IsDBNull(iCrmeh73)) entity.Crmeh73 = dr.GetDecimal(iCrmeh73);

            int iCrmeh72 = dr.GetOrdinal(this.Crmeh72);
            if (!dr.IsDBNull(iCrmeh72)) entity.Crmeh72 = dr.GetDecimal(iCrmeh72);

            int iCrmeh71 = dr.GetOrdinal(this.Crmeh71);
            if (!dr.IsDBNull(iCrmeh71)) entity.Crmeh71 = dr.GetDecimal(iCrmeh71);

            int iCrmeh70 = dr.GetOrdinal(this.Crmeh70);
            if (!dr.IsDBNull(iCrmeh70)) entity.Crmeh70 = dr.GetDecimal(iCrmeh70);

            int iCrmeh69 = dr.GetOrdinal(this.Crmeh69);
            if (!dr.IsDBNull(iCrmeh69)) entity.Crmeh69 = dr.GetDecimal(iCrmeh69);

            int iCrmeh68 = dr.GetOrdinal(this.Crmeh68);
            if (!dr.IsDBNull(iCrmeh68)) entity.Crmeh68 = dr.GetDecimal(iCrmeh68);

            int iCrmeh67 = dr.GetOrdinal(this.Crmeh67);
            if (!dr.IsDBNull(iCrmeh67)) entity.Crmeh67 = dr.GetDecimal(iCrmeh67);

            int iCrmeh66 = dr.GetOrdinal(this.Crmeh66);
            if (!dr.IsDBNull(iCrmeh66)) entity.Crmeh66 = dr.GetDecimal(iCrmeh66);

            int iCrmeh65 = dr.GetOrdinal(this.Crmeh65);
            if (!dr.IsDBNull(iCrmeh65)) entity.Crmeh65 = dr.GetDecimal(iCrmeh65);

            int iCrmeh64 = dr.GetOrdinal(this.Crmeh64);
            if (!dr.IsDBNull(iCrmeh64)) entity.Crmeh64 = dr.GetDecimal(iCrmeh64);

            int iCrmeh63 = dr.GetOrdinal(this.Crmeh63);
            if (!dr.IsDBNull(iCrmeh63)) entity.Crmeh63 = dr.GetDecimal(iCrmeh63);

            int iCrmeh62 = dr.GetOrdinal(this.Crmeh62);
            if (!dr.IsDBNull(iCrmeh62)) entity.Crmeh62 = dr.GetDecimal(iCrmeh62);

            int iCrmeh61 = dr.GetOrdinal(this.Crmeh61);
            if (!dr.IsDBNull(iCrmeh61)) entity.Crmeh61 = dr.GetDecimal(iCrmeh61);

            int iCrmeh60 = dr.GetOrdinal(this.Crmeh60);
            if (!dr.IsDBNull(iCrmeh60)) entity.Crmeh60 = dr.GetDecimal(iCrmeh60);

            int iCrmeh59 = dr.GetOrdinal(this.Crmeh59);
            if (!dr.IsDBNull(iCrmeh59)) entity.Crmeh59 = dr.GetDecimal(iCrmeh59);

            int iCrmeh58 = dr.GetOrdinal(this.Crmeh58);
            if (!dr.IsDBNull(iCrmeh58)) entity.Crmeh58 = dr.GetDecimal(iCrmeh58);

            int iCrmeh57 = dr.GetOrdinal(this.Crmeh57);
            if (!dr.IsDBNull(iCrmeh57)) entity.Crmeh57 = dr.GetDecimal(iCrmeh57);

            int iCrmeh56 = dr.GetOrdinal(this.Crmeh56);
            if (!dr.IsDBNull(iCrmeh56)) entity.Crmeh56 = dr.GetDecimal(iCrmeh56);

            int iCrmeh55 = dr.GetOrdinal(this.Crmeh55);
            if (!dr.IsDBNull(iCrmeh55)) entity.Crmeh55 = dr.GetDecimal(iCrmeh55);

            int iCrmeh54 = dr.GetOrdinal(this.Crmeh54);
            if (!dr.IsDBNull(iCrmeh54)) entity.Crmeh54 = dr.GetDecimal(iCrmeh54);

            int iCrmeh53 = dr.GetOrdinal(this.Crmeh53);
            if (!dr.IsDBNull(iCrmeh53)) entity.Crmeh53 = dr.GetDecimal(iCrmeh53);

            int iCrmeh52 = dr.GetOrdinal(this.Crmeh52);
            if (!dr.IsDBNull(iCrmeh52)) entity.Crmeh52 = dr.GetDecimal(iCrmeh52);

            int iCrmeh51 = dr.GetOrdinal(this.Crmeh51);
            if (!dr.IsDBNull(iCrmeh51)) entity.Crmeh51 = dr.GetDecimal(iCrmeh51);

            int iCrmeh50 = dr.GetOrdinal(this.Crmeh50);
            if (!dr.IsDBNull(iCrmeh50)) entity.Crmeh50 = dr.GetDecimal(iCrmeh50);

            int iCrmeh49 = dr.GetOrdinal(this.Crmeh49);
            if (!dr.IsDBNull(iCrmeh49)) entity.Crmeh49 = dr.GetDecimal(iCrmeh49);

            int iCrmeh48 = dr.GetOrdinal(this.Crmeh48);
            if (!dr.IsDBNull(iCrmeh48)) entity.Crmeh48 = dr.GetDecimal(iCrmeh48);

            return entity;
        }


        #region Mapeo de Campos

        public string Crmeh47 = "CRMEH47";
        public string Crmeh46 = "CRMEH46";
        public string Crmeh45 = "CRMEH45";
        public string Crmeh44 = "CRMEH44";
        public string Crmeh43 = "CRMEH43";
        public string Crmeh42 = "CRMEH42";
        public string Crmeh41 = "CRMEH41";
        public string Crmeh40 = "CRMEH40";
        public string Crmeh39 = "CRMEH39";
        public string Crmeh38 = "CRMEH38";
        public string Crmeh37 = "CRMEH37";
        public string Crmeh36 = "CRMEH36";
        public string Crmeh35 = "CRMEH35";
        public string Crmeh34 = "CRMEH34";
        public string Crmeh33 = "CRMEH33";
        public string Crmeh32 = "CRMEH32";
        public string Crmeh31 = "CRMEH31";
        public string Crmeh30 = "CRMEH30";
        public string Crmeh29 = "CRMEH29";
        public string Crmeh28 = "CRMEH28";
        public string Crmeh27 = "CRMEH27";
        public string Crmeh26 = "CRMEH26";
        public string Crmeh25 = "CRMEH25";
        public string Crmeh24 = "CRMEH24";
        public string Crmeh23 = "CRMEH23";
        public string Crmeh22 = "CRMEH22";
        public string Crmeh21 = "CRMEH21";
        public string Crmeh20 = "CRMEH20";
        public string Crmeh19 = "CRMEH19";
        public string Crmeh18 = "CRMEH18";
        public string Crmeh17 = "CRMEH17";
        public string Crmeh16 = "CRMEH16";
        public string Crmeh15 = "CRMEH15";
        public string Crmeh14 = "CRMEH14";
        public string Crmeh13 = "CRMEH13";
        public string Crmeh12 = "CRMEH12";
        public string Crmeh11 = "CRMEH11";
        public string Crmeh10 = "CRMEH10";
        public string Crmeh9 = "CRMEH9";
        public string Crmeh8 = "CRMEH8";
        public string Crmeh7 = "CRMEH7";
        public string Crmeh6 = "CRMEH6";
        public string Crmeh5 = "CRMEH5";
        public string Crmeh4 = "CRMEH4";
        public string Crmeh3 = "CRMEH3";
        public string Crmeh2 = "CRMEH2";
        public string Crmeh1 = "CRMEH1";
        public string Crmemeditotal = "CRMEMEDITOTAL";
        public string Crmefecha = "CRMEFECHA";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Pecacodi = "PECACODI";
        public string Crmeh96 = "CRMEH96";
        public string Crmeh95 = "CRMEH95";
        public string Crmeh94 = "CRMEH94";
        public string Crmeh93 = "CRMEH93";
        public string Crmeh92 = "CRMEH92";
        public string Crmeh91 = "CRMEH91";
        public string Crmeh90 = "CRMEH90";
        public string Crmeh89 = "CRMEH89";
        public string Crmeh88 = "CRMEH88";
        public string Crmeh87 = "CRMEH87";
        public string Crmeh86 = "CRMEH86";
        public string Crmeh85 = "CRMEH85";
        public string Crmeh84 = "CRMEH84";
        public string Crmeh83 = "CRMEH83";
        public string Crmeh82 = "CRMEH82";
        public string Crmeh81 = "CRMEH81";
        public string Crmeh80 = "CRMEH80";
        public string Crmeh79 = "CRMEH79";
        public string Crmeh78 = "CRMEH78";
        public string Crmeh77 = "CRMEH77";
        public string Crmeh76 = "CRMEH76";
        public string Crmeh75 = "CRMEH75";
        public string Crmeh74 = "CRMEH74";
        public string Crmeh73 = "CRMEH73";
        public string Crmeh72 = "CRMEH72";
        public string Crmeh71 = "CRMEH71";
        public string Crmeh70 = "CRMEH70";
        public string Crmeh69 = "CRMEH69";
        public string Crmeh68 = "CRMEH68";
        public string Crmeh67 = "CRMEH67";
        public string Crmeh66 = "CRMEH66";
        public string Crmeh65 = "CRMEH65";
        public string Crmeh64 = "CRMEH64";
        public string Crmeh63 = "CRMEH63";
        public string Crmeh62 = "CRMEH62";
        public string Crmeh61 = "CRMEH61";
        public string Crmeh60 = "CRMEH60";
        public string Crmeh59 = "CRMEH59";
        public string Crmeh58 = "CRMEH58";
        public string Crmeh57 = "CRMEH57";
        public string Crmeh56 = "CRMEH56";
        public string Crmeh55 = "CRMEH55";
        public string Crmeh54 = "CRMEH54";
        public string Crmeh53 = "CRMEH53";
        public string Crmeh52 = "CRMEH52";
        public string Crmeh51 = "CRMEH51";
        public string Crmeh50 = "CRMEH50";
        public string Crmeh49 = "CRMEH49";
        public string Crmeh48 = "CRMEH48";

        #endregion

        public string SqlSaveFromMeMedicion96
        {
            get { return base.GetSqlXml("SaveFromMeMedicion96"); }
        }

        public string SqlDeletexFecha
        {
            get { return base.GetSqlXml("DeletexFecha"); }
        }

        public string SqlListByCriteria
        {
            get { return base.GetSqlXml("ListByCriteria"); }
        }

        public string SqlSaveFromOtherVersion
        {
            get { return base.GetSqlXml("SaveFromOtherVersion"); }
        }

        public string SqlDeleteByVersion
        {
            get { return base.GetSqlXml("DeleteByVersion"); }
        }
    }
}
