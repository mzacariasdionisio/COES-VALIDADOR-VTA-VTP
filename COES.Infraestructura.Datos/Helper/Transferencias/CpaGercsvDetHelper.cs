using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_GERCSVDET_TMP
    /// </summary>
    public class CpaGercsvDetHelper : HelperBase
    {
        public CpaGercsvDetHelper() : base(Consultas.CpaGercsvDetSql)
        {
        }

        public CpaGercsvDetDTO Create(IDataReader dr)
        {
            CpaGercsvDetDTO entity = new CpaGercsvDetDTO();

            int iCpagedcodi = dr.GetOrdinal(this.Cpagedcodi);
            if (!dr.IsDBNull(iCpagedcodi)) entity.Cpagedcodi = Convert.ToInt32(dr.GetValue(iCpagedcodi));

            int iCpagercodi = dr.GetOrdinal(this.Cpagercodi);
            if (!dr.IsDBNull(iCpagercodi)) entity.Cpagercodi = Convert.ToInt32(dr.GetValue(iCpagercodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iCpagedtipcsv = dr.GetOrdinal(this.Cpagedtipcsv);
            if (!dr.IsDBNull(iCpagedtipcsv)) entity.Cpagedtipcsv = dr.GetString(iCpagedtipcsv);

            int iCpagedfecha = dr.GetOrdinal(this.Cpagedfecha);
            if (!dr.IsDBNull(iCpagedfecha)) entity.Cpagedfecha = dr.GetDateTime(iCpagedfecha);

            int iCpagedh1 = dr.GetOrdinal(this.Cpagedh1);
            if (!dr.IsDBNull(iCpagedh1)) entity.Cpagedh1 = dr.GetDecimal(iCpagedh1);

            int iCpagedh2 = dr.GetOrdinal(this.Cpagedh2);
            if (!dr.IsDBNull(iCpagedh2)) entity.Cpagedh2 = dr.GetDecimal(iCpagedh2);

            int iCpagedh3 = dr.GetOrdinal(this.Cpagedh3);
            if (!dr.IsDBNull(iCpagedh3)) entity.Cpagedh3 = dr.GetDecimal(iCpagedh3);

            int iCpagedh4 = dr.GetOrdinal(this.Cpagedh4);
            if (!dr.IsDBNull(iCpagedh4)) entity.Cpagedh4 = dr.GetDecimal(iCpagedh4);

            int iCpagedh5 = dr.GetOrdinal(this.Cpagedh5);
            if (!dr.IsDBNull(iCpagedh5)) entity.Cpagedh5 = dr.GetDecimal(iCpagedh5);

            int iCpagedh6 = dr.GetOrdinal(this.Cpagedh6);
            if (!dr.IsDBNull(iCpagedh6)) entity.Cpagedh6 = dr.GetDecimal(iCpagedh6);

            int iCpagedh7 = dr.GetOrdinal(this.Cpagedh7);
            if (!dr.IsDBNull(iCpagedh7)) entity.Cpagedh7 = dr.GetDecimal(iCpagedh7);

            int iCpagedh8 = dr.GetOrdinal(this.Cpagedh8);
            if (!dr.IsDBNull(iCpagedh8)) entity.Cpagedh8 = dr.GetDecimal(iCpagedh8);

            int iCpagedh9 = dr.GetOrdinal(this.Cpagedh9);
            if (!dr.IsDBNull(iCpagedh9)) entity.Cpagedh9 = dr.GetDecimal(iCpagedh9);

            int iCpagedh10 = dr.GetOrdinal(this.Cpagedh10);
            if (!dr.IsDBNull(iCpagedh10)) entity.Cpagedh10 = dr.GetDecimal(iCpagedh10);

            int iCpagedh11 = dr.GetOrdinal(this.Cpagedh11);
            if (!dr.IsDBNull(iCpagedh11)) entity.Cpagedh11 = dr.GetDecimal(iCpagedh11);

            int iCpagedh12 = dr.GetOrdinal(this.Cpagedh12);
            if (!dr.IsDBNull(iCpagedh12)) entity.Cpagedh12 = dr.GetDecimal(iCpagedh12);

            int iCpagedh13 = dr.GetOrdinal(this.Cpagedh13);
            if (!dr.IsDBNull(iCpagedh13)) entity.Cpagedh13 = dr.GetDecimal(iCpagedh13);

            int iCpagedh14 = dr.GetOrdinal(this.Cpagedh14);
            if (!dr.IsDBNull(iCpagedh14)) entity.Cpagedh14 = dr.GetDecimal(iCpagedh14);

            int iCpagedh15 = dr.GetOrdinal(this.Cpagedh15);
            if (!dr.IsDBNull(iCpagedh15)) entity.Cpagedh15 = dr.GetDecimal(iCpagedh15);

            int iCpagedh16 = dr.GetOrdinal(this.Cpagedh16);
            if (!dr.IsDBNull(iCpagedh16)) entity.Cpagedh16 = dr.GetDecimal(iCpagedh16);

            int iCpagedh17 = dr.GetOrdinal(this.Cpagedh17);
            if (!dr.IsDBNull(iCpagedh17)) entity.Cpagedh17 = dr.GetDecimal(iCpagedh17);

            int iCpagedh18 = dr.GetOrdinal(this.Cpagedh18);
            if (!dr.IsDBNull(iCpagedh18)) entity.Cpagedh18 = dr.GetDecimal(iCpagedh18);

            int iCpagedh19 = dr.GetOrdinal(this.Cpagedh19);
            if (!dr.IsDBNull(iCpagedh19)) entity.Cpagedh19 = dr.GetDecimal(iCpagedh19);

            int iCpagedh20 = dr.GetOrdinal(this.Cpagedh20);
            if (!dr.IsDBNull(iCpagedh20)) entity.Cpagedh20 = dr.GetDecimal(iCpagedh20);

            int iCpagedh21 = dr.GetOrdinal(this.Cpagedh21);
            if (!dr.IsDBNull(iCpagedh21)) entity.Cpagedh21 = dr.GetDecimal(iCpagedh21);

            int iCpagedh22 = dr.GetOrdinal(this.Cpagedh22);
            if (!dr.IsDBNull(iCpagedh22)) entity.Cpagedh22 = dr.GetDecimal(iCpagedh22);

            int iCpagedh23 = dr.GetOrdinal(this.Cpagedh23);
            if (!dr.IsDBNull(iCpagedh23)) entity.Cpagedh23 = dr.GetDecimal(iCpagedh23);

            int iCpagedh24 = dr.GetOrdinal(this.Cpagedh24);
            if (!dr.IsDBNull(iCpagedh24)) entity.Cpagedh24 = dr.GetDecimal(iCpagedh24);

            int iCpagedh25 = dr.GetOrdinal(this.Cpagedh25);
            if (!dr.IsDBNull(iCpagedh25)) entity.Cpagedh25 = dr.GetDecimal(iCpagedh25);

            int iCpagedh26 = dr.GetOrdinal(this.Cpagedh26);
            if (!dr.IsDBNull(iCpagedh26)) entity.Cpagedh26 = dr.GetDecimal(iCpagedh26);

            int iCpagedh27 = dr.GetOrdinal(this.Cpagedh27);
            if (!dr.IsDBNull(iCpagedh27)) entity.Cpagedh27 = dr.GetDecimal(iCpagedh27);

            int iCpagedh28 = dr.GetOrdinal(this.Cpagedh28);
            if (!dr.IsDBNull(iCpagedh28)) entity.Cpagedh28 = dr.GetDecimal(iCpagedh28);

            int iCpagedh29 = dr.GetOrdinal(this.Cpagedh29);
            if (!dr.IsDBNull(iCpagedh29)) entity.Cpagedh29 = dr.GetDecimal(iCpagedh29);

            int iCpagedh30 = dr.GetOrdinal(this.Cpagedh30);
            if (!dr.IsDBNull(iCpagedh30)) entity.Cpagedh30 = dr.GetDecimal(iCpagedh30);

            int iCpagedh31 = dr.GetOrdinal(this.Cpagedh31);
            if (!dr.IsDBNull(iCpagedh31)) entity.Cpagedh31 = dr.GetDecimal(iCpagedh31);

            int iCpagedh32 = dr.GetOrdinal(this.Cpagedh32);
            if (!dr.IsDBNull(iCpagedh32)) entity.Cpagedh32 = dr.GetDecimal(iCpagedh32);

            int iCpagedh33 = dr.GetOrdinal(this.Cpagedh33);
            if (!dr.IsDBNull(iCpagedh33)) entity.Cpagedh33 = dr.GetDecimal(iCpagedh33);

            int iCpagedh34 = dr.GetOrdinal(this.Cpagedh34);
            if (!dr.IsDBNull(iCpagedh34)) entity.Cpagedh34 = dr.GetDecimal(iCpagedh34);

            int iCpagedh35 = dr.GetOrdinal(this.Cpagedh35);
            if (!dr.IsDBNull(iCpagedh35)) entity.Cpagedh35 = dr.GetDecimal(iCpagedh35);

            int iCpagedh36 = dr.GetOrdinal(this.Cpagedh36);
            if (!dr.IsDBNull(iCpagedh36)) entity.Cpagedh36 = dr.GetDecimal(iCpagedh36);

            int iCpagedh37 = dr.GetOrdinal(this.Cpagedh37);
            if (!dr.IsDBNull(iCpagedh37)) entity.Cpagedh37 = dr.GetDecimal(iCpagedh37);

            int iCpagedh38 = dr.GetOrdinal(this.Cpagedh38);
            if (!dr.IsDBNull(iCpagedh38)) entity.Cpagedh38 = dr.GetDecimal(iCpagedh38);

            int iCpagedh39 = dr.GetOrdinal(this.Cpagedh39);
            if (!dr.IsDBNull(iCpagedh39)) entity.Cpagedh39 = dr.GetDecimal(iCpagedh39);

            int iCpagedh40 = dr.GetOrdinal(this.Cpagedh40);
            if (!dr.IsDBNull(iCpagedh40)) entity.Cpagedh40 = dr.GetDecimal(iCpagedh40);

            int iCpagedh41 = dr.GetOrdinal(this.Cpagedh41);
            if (!dr.IsDBNull(iCpagedh41)) entity.Cpagedh41 = dr.GetDecimal(iCpagedh41);

            int iCpagedh42 = dr.GetOrdinal(this.Cpagedh42);
            if (!dr.IsDBNull(iCpagedh42)) entity.Cpagedh42 = dr.GetDecimal(iCpagedh42);

            int iCpagedh43 = dr.GetOrdinal(this.Cpagedh43);
            if (!dr.IsDBNull(iCpagedh43)) entity.Cpagedh43 = dr.GetDecimal(iCpagedh43);

            int iCpagedh44 = dr.GetOrdinal(this.Cpagedh44);
            if (!dr.IsDBNull(iCpagedh44)) entity.Cpagedh44 = dr.GetDecimal(iCpagedh44);

            int iCpagedh45 = dr.GetOrdinal(this.Cpagedh45);
            if (!dr.IsDBNull(iCpagedh45)) entity.Cpagedh45 = dr.GetDecimal(iCpagedh45);

            int iCpagedh46 = dr.GetOrdinal(this.Cpagedh46);
            if (!dr.IsDBNull(iCpagedh46)) entity.Cpagedh46 = dr.GetDecimal(iCpagedh46);

            int iCpagedh47 = dr.GetOrdinal(this.Cpagedh47);
            if (!dr.IsDBNull(iCpagedh47)) entity.Cpagedh47 = dr.GetDecimal(iCpagedh47);

            int iCpagedh48 = dr.GetOrdinal(this.Cpagedh48);
            if (!dr.IsDBNull(iCpagedh48)) entity.Cpagedh48 = dr.GetDecimal(iCpagedh48);

            int iCpagedh49 = dr.GetOrdinal(this.Cpagedh49);
            if (!dr.IsDBNull(iCpagedh49)) entity.Cpagedh49 = dr.GetDecimal(iCpagedh49);

            int iCpagedh50 = dr.GetOrdinal(this.Cpagedh50);
            if (!dr.IsDBNull(iCpagedh50)) entity.Cpagedh50 = dr.GetDecimal(iCpagedh50);

            int iCpagedh51 = dr.GetOrdinal(this.Cpagedh51);
            if (!dr.IsDBNull(iCpagedh51)) entity.Cpagedh51 = dr.GetDecimal(iCpagedh51);

            int iCpagedh52 = dr.GetOrdinal(this.Cpagedh52);
            if (!dr.IsDBNull(iCpagedh52)) entity.Cpagedh52 = dr.GetDecimal(iCpagedh52);

            int iCpagedh53 = dr.GetOrdinal(this.Cpagedh53);
            if (!dr.IsDBNull(iCpagedh53)) entity.Cpagedh53 = dr.GetDecimal(iCpagedh53);

            int iCpagedh54 = dr.GetOrdinal(this.Cpagedh54);
            if (!dr.IsDBNull(iCpagedh54)) entity.Cpagedh54 = dr.GetDecimal(iCpagedh54);

            int iCpagedh55 = dr.GetOrdinal(this.Cpagedh55);
            if (!dr.IsDBNull(iCpagedh55)) entity.Cpagedh55 = dr.GetDecimal(iCpagedh55);

            int iCpagedh56 = dr.GetOrdinal(this.Cpagedh56);
            if (!dr.IsDBNull(iCpagedh56)) entity.Cpagedh56 = dr.GetDecimal(iCpagedh56);

            int iCpagedh57 = dr.GetOrdinal(this.Cpagedh57);
            if (!dr.IsDBNull(iCpagedh57)) entity.Cpagedh57 = dr.GetDecimal(iCpagedh57);

            int iCpagedh58 = dr.GetOrdinal(this.Cpagedh58);
            if (!dr.IsDBNull(iCpagedh58)) entity.Cpagedh58 = dr.GetDecimal(iCpagedh58);

            int iCpagedh59 = dr.GetOrdinal(this.Cpagedh59);
            if (!dr.IsDBNull(iCpagedh59)) entity.Cpagedh59 = dr.GetDecimal(iCpagedh59);

            int iCpagedh60 = dr.GetOrdinal(this.Cpagedh60);
            if (!dr.IsDBNull(iCpagedh60)) entity.Cpagedh60 = dr.GetDecimal(iCpagedh60);

            int iCpagedh61 = dr.GetOrdinal(this.Cpagedh61);
            if (!dr.IsDBNull(iCpagedh61)) entity.Cpagedh61 = dr.GetDecimal(iCpagedh61);

            int iCpagedh62 = dr.GetOrdinal(this.Cpagedh62);
            if (!dr.IsDBNull(iCpagedh62)) entity.Cpagedh62 = dr.GetDecimal(iCpagedh62);

            int iCpagedh63 = dr.GetOrdinal(this.Cpagedh63);
            if (!dr.IsDBNull(iCpagedh63)) entity.Cpagedh63 = dr.GetDecimal(iCpagedh63);

            int iCpagedh64 = dr.GetOrdinal(this.Cpagedh64);
            if (!dr.IsDBNull(iCpagedh64)) entity.Cpagedh64 = dr.GetDecimal(iCpagedh64);

            int iCpagedh65 = dr.GetOrdinal(this.Cpagedh65);
            if (!dr.IsDBNull(iCpagedh65)) entity.Cpagedh65 = dr.GetDecimal(iCpagedh65);

            int iCpagedh66 = dr.GetOrdinal(this.Cpagedh66);
            if (!dr.IsDBNull(iCpagedh66)) entity.Cpagedh66 = dr.GetDecimal(iCpagedh66);

            int iCpagedh67 = dr.GetOrdinal(this.Cpagedh67);
            if (!dr.IsDBNull(iCpagedh67)) entity.Cpagedh67 = dr.GetDecimal(iCpagedh67);

            int iCpagedh68 = dr.GetOrdinal(this.Cpagedh68);
            if (!dr.IsDBNull(iCpagedh68)) entity.Cpagedh68 = dr.GetDecimal(iCpagedh68);

            int iCpagedh69 = dr.GetOrdinal(this.Cpagedh69);
            if (!dr.IsDBNull(iCpagedh69)) entity.Cpagedh69 = dr.GetDecimal(iCpagedh69);

            int iCpagedh70 = dr.GetOrdinal(this.Cpagedh70);
            if (!dr.IsDBNull(iCpagedh70)) entity.Cpagedh70 = dr.GetDecimal(iCpagedh70);

            int iCpagedh71 = dr.GetOrdinal(this.Cpagedh71);
            if (!dr.IsDBNull(iCpagedh71)) entity.Cpagedh71 = dr.GetDecimal(iCpagedh71);

            int iCpagedh72 = dr.GetOrdinal(this.Cpagedh72);
            if (!dr.IsDBNull(iCpagedh72)) entity.Cpagedh72 = dr.GetDecimal(iCpagedh72);

            int iCpagedh73 = dr.GetOrdinal(this.Cpagedh73);
            if (!dr.IsDBNull(iCpagedh73)) entity.Cpagedh73 = dr.GetDecimal(iCpagedh73);

            int iCpagedh74 = dr.GetOrdinal(this.Cpagedh74);
            if (!dr.IsDBNull(iCpagedh74)) entity.Cpagedh74 = dr.GetDecimal(iCpagedh74);

            int iCpagedh75 = dr.GetOrdinal(this.Cpagedh75);
            if (!dr.IsDBNull(iCpagedh75)) entity.Cpagedh75 = dr.GetDecimal(iCpagedh75);

            int iCpagedh76 = dr.GetOrdinal(this.Cpagedh76);
            if (!dr.IsDBNull(iCpagedh76)) entity.Cpagedh76 = dr.GetDecimal(iCpagedh76);

            int iCpagedh77 = dr.GetOrdinal(this.Cpagedh77);
            if (!dr.IsDBNull(iCpagedh77)) entity.Cpagedh77 = dr.GetDecimal(iCpagedh77);

            int iCpagedh78 = dr.GetOrdinal(this.Cpagedh78);
            if (!dr.IsDBNull(iCpagedh78)) entity.Cpagedh78 = dr.GetDecimal(iCpagedh78);

            int iCpagedh79 = dr.GetOrdinal(this.Cpagedh79);
            if (!dr.IsDBNull(iCpagedh79)) entity.Cpagedh79 = dr.GetDecimal(iCpagedh79);

            int iCpagedh80 = dr.GetOrdinal(this.Cpagedh80);
            if (!dr.IsDBNull(iCpagedh80)) entity.Cpagedh80 = dr.GetDecimal(iCpagedh80);

            int iCpagedh81 = dr.GetOrdinal(this.Cpagedh81);
            if (!dr.IsDBNull(iCpagedh81)) entity.Cpagedh81 = dr.GetDecimal(iCpagedh81);

            int iCpagedh82 = dr.GetOrdinal(this.Cpagedh82);
            if (!dr.IsDBNull(iCpagedh82)) entity.Cpagedh82 = dr.GetDecimal(iCpagedh82);

            int iCpagedh83 = dr.GetOrdinal(this.Cpagedh83);
            if (!dr.IsDBNull(iCpagedh83)) entity.Cpagedh83 = dr.GetDecimal(iCpagedh83);

            int iCpagedh84 = dr.GetOrdinal(this.Cpagedh84);
            if (!dr.IsDBNull(iCpagedh84)) entity.Cpagedh84 = dr.GetDecimal(iCpagedh84);

            int iCpagedh85 = dr.GetOrdinal(this.Cpagedh85);
            if (!dr.IsDBNull(iCpagedh85)) entity.Cpagedh85 = dr.GetDecimal(iCpagedh85);

            int iCpagedh86 = dr.GetOrdinal(this.Cpagedh86);
            if (!dr.IsDBNull(iCpagedh86)) entity.Cpagedh86 = dr.GetDecimal(iCpagedh86);

            int iCpagedh87 = dr.GetOrdinal(this.Cpagedh87);
            if (!dr.IsDBNull(iCpagedh87)) entity.Cpagedh87 = dr.GetDecimal(iCpagedh87);

            int iCpagedh88 = dr.GetOrdinal(this.Cpagedh88);
            if (!dr.IsDBNull(iCpagedh88)) entity.Cpagedh88 = dr.GetDecimal(iCpagedh88);

            int iCpagedh89 = dr.GetOrdinal(this.Cpagedh89);
            if (!dr.IsDBNull(iCpagedh89)) entity.Cpagedh89 = dr.GetDecimal(iCpagedh89);

            int iCpagedh90 = dr.GetOrdinal(this.Cpagedh90);
            if (!dr.IsDBNull(iCpagedh90)) entity.Cpagedh90 = dr.GetDecimal(iCpagedh90);

            int iCpagedh91 = dr.GetOrdinal(this.Cpagedh91);
            if (!dr.IsDBNull(iCpagedh91)) entity.Cpagedh91 = dr.GetDecimal(iCpagedh91);

            int iCpagedh92 = dr.GetOrdinal(this.Cpagedh92);
            if (!dr.IsDBNull(iCpagedh92)) entity.Cpagedh92 = dr.GetDecimal(iCpagedh92);

            int iCpagedh93 = dr.GetOrdinal(this.Cpagedh93);
            if (!dr.IsDBNull(iCpagedh93)) entity.Cpagedh93 = dr.GetDecimal(iCpagedh93);

            int iCpagedh94 = dr.GetOrdinal(this.Cpagedh94);
            if (!dr.IsDBNull(iCpagedh94)) entity.Cpagedh94 = dr.GetDecimal(iCpagedh94);

            int iCpagedh95 = dr.GetOrdinal(this.Cpagedh95);
            if (!dr.IsDBNull(iCpagedh95)) entity.Cpagedh95 = dr.GetDecimal(iCpagedh95);

            int iCpagedh96 = dr.GetOrdinal(this.Cpagedh96);
            if (!dr.IsDBNull(iCpagedh96)) entity.Cpagedh96 = dr.GetDecimal(iCpagedh96);

            int iCpagedusucreacion = dr.GetOrdinal(this.Cpagedusucreacion);
            if (!dr.IsDBNull(iCpagedusucreacion)) entity.Cpagedusucreacion = dr.GetString(iCpagedusucreacion);

            int iCpagedfeccreacion = dr.GetOrdinal(this.Cpagedfeccreacion);
            if (!dr.IsDBNull(iCpagedfeccreacion)) entity.Cpagedfeccreacion = dr.GetDateTime(iCpagedfeccreacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Cpagedcodi = "CPAGEDCODI";
        public string Cpagercodi = "CPAGERCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Cpagedtipcsv = "CPAGEDTIPCSV";
        public string Cpagedfecha = "CPAGEDFECHA";
        public string Cpagedh1 = "CPAGEDH1";
        public string Cpagedh2 = "CPAGEDH2";
        public string Cpagedh3 = "CPAGEDH3";
        public string Cpagedh4 = "CPAGEDH4";
        public string Cpagedh5 = "CPAGEDH5";
        public string Cpagedh6 = "CPAGEDH6";
        public string Cpagedh7 = "CPAGEDH7";
        public string Cpagedh8 = "CPAGEDH8";
        public string Cpagedh9 = "CPAGEDH9";
        public string Cpagedh10 = "CPAGEDH10";
        public string Cpagedh11 = "CPAGEDH11";
        public string Cpagedh12 = "CPAGEDH12";
        public string Cpagedh13 = "CPAGEDH13";
        public string Cpagedh14 = "CPAGEDH14";
        public string Cpagedh15 = "CPAGEDH15";
        public string Cpagedh16 = "CPAGEDH16";
        public string Cpagedh17 = "CPAGEDH17";
        public string Cpagedh18 = "CPAGEDH18";
        public string Cpagedh19 = "CPAGEDH19";
        public string Cpagedh20 = "CPAGEDH20";
        public string Cpagedh21 = "CPAGEDH21";
        public string Cpagedh22 = "CPAGEDH22";
        public string Cpagedh23 = "CPAGEDH23";
        public string Cpagedh24 = "CPAGEDH24";
        public string Cpagedh25 = "CPAGEDH25";
        public string Cpagedh26 = "CPAGEDH26";
        public string Cpagedh27 = "CPAGEDH27";
        public string Cpagedh28 = "CPAGEDH28";
        public string Cpagedh29 = "CPAGEDH29";
        public string Cpagedh30 = "CPAGEDH30";
        public string Cpagedh31 = "CPAGEDH31";
        public string Cpagedh32 = "CPAGEDH32";
        public string Cpagedh33 = "CPAGEDH33";
        public string Cpagedh34 = "CPAGEDH34";
        public string Cpagedh35 = "CPAGEDH35";
        public string Cpagedh36 = "CPAGEDH36";
        public string Cpagedh37 = "CPAGEDH37";
        public string Cpagedh38 = "CPAGEDH38";
        public string Cpagedh39 = "CPAGEDH39";
        public string Cpagedh40 = "CPAGEDH40";
        public string Cpagedh41 = "CPAGEDH41";
        public string Cpagedh42 = "CPAGEDH42";
        public string Cpagedh43 = "CPAGEDH43";
        public string Cpagedh44 = "CPAGEDH44";
        public string Cpagedh45 = "CPAGEDH45";
        public string Cpagedh46 = "CPAGEDH46";
        public string Cpagedh47 = "CPAGEDH47";
        public string Cpagedh48 = "CPAGEDH48";
        public string Cpagedh49 = "CPAGEDH49";
        public string Cpagedh50 = "CPAGEDH50";
        public string Cpagedh51 = "CPAGEDH51";
        public string Cpagedh52 = "CPAGEDH52";
        public string Cpagedh53 = "CPAGEDH53";
        public string Cpagedh54 = "CPAGEDH54";
        public string Cpagedh55 = "CPAGEDH55";
        public string Cpagedh56 = "CPAGEDH56";
        public string Cpagedh57 = "CPAGEDH57";
        public string Cpagedh58 = "CPAGEDH58";
        public string Cpagedh59 = "CPAGEDH59";
        public string Cpagedh60 = "CPAGEDH60";
        public string Cpagedh61 = "CPAGEDH61";
        public string Cpagedh62 = "CPAGEDH62";
        public string Cpagedh63 = "CPAGEDH63";
        public string Cpagedh64 = "CPAGEDH64";
        public string Cpagedh65 = "CPAGEDH65";
        public string Cpagedh66 = "CPAGEDH66";
        public string Cpagedh67 = "CPAGEDH67";
        public string Cpagedh68 = "CPAGEDH68";
        public string Cpagedh69 = "CPAGEDH69";
        public string Cpagedh70 = "CPAGEDH70";
        public string Cpagedh71 = "CPAGEDH71";
        public string Cpagedh72 = "CPAGEDH72";
        public string Cpagedh73 = "CPAGEDH73";
        public string Cpagedh74 = "CPAGEDH74";
        public string Cpagedh75 = "CPAGEDH75";
        public string Cpagedh76 = "CPAGEDH76";
        public string Cpagedh77 = "CPAGEDH77";
        public string Cpagedh78 = "CPAGEDH78";
        public string Cpagedh79 = "CPAGEDH79";
        public string Cpagedh80 = "CPAGEDH80";
        public string Cpagedh81 = "CPAGEDH81";
        public string Cpagedh82 = "CPAGEDH82";
        public string Cpagedh83 = "CPAGEDH83";
        public string Cpagedh84 = "CPAGEDH84";
        public string Cpagedh85 = "CPAGEDH85";
        public string Cpagedh86 = "CPAGEDH86";
        public string Cpagedh87 = "CPAGEDH87";
        public string Cpagedh88 = "CPAGEDH88";
        public string Cpagedh89 = "CPAGEDH89";
        public string Cpagedh90 = "CPAGEDH90";
        public string Cpagedh91 = "CPAGEDH91";
        public string Cpagedh92 = "CPAGEDH92";
        public string Cpagedh93 = "CPAGEDH93";
        public string Cpagedh94 = "CPAGEDH94";
        public string Cpagedh95 = "CPAGEDH95";
        public string Cpagedh96 = "CPAGEDH96";
        public string Cpagedusucreacion = "CPAGEDUSUCREACION";
        public string Cpagedfeccreacion = "CPAGEDFECCREACION";

        //Adcionales de consulta
        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "EQUINOMB";
        #endregion      

        public string SqlTruncateTmp
        {
            get { return base.GetSqlXml("TruncateTmp"); }
        }
    }
}
