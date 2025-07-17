using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_GERCSV_DET
    /// </summary>
    public class RerGerCsvDetHelper : HelperBase
    {
        public RerGerCsvDetHelper() : base(Consultas.RerGerCsvDetSql)
        {
        }

        public RerGerCsvDetDTO Create(IDataReader dr)
        {
            RerGerCsvDetDTO entity = new RerGerCsvDetDTO();

            int iRegedcodi = dr.GetOrdinal(this.Regedcodi);
            if (!dr.IsDBNull(iRegedcodi)) entity.Regedcodi = Convert.ToInt32(dr.GetValue(iRegedcodi));

            int iRegercodi = dr.GetOrdinal(this.Regercodi);
            if (!dr.IsDBNull(iRegercodi)) entity.Regercodi = Convert.ToInt32(dr.GetValue(iRegercodi));


            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iRegedtipcsv = dr.GetOrdinal(this.Regedtipcsv);
            if (!dr.IsDBNull(iRegedtipcsv)) entity.Regedtipcsv = dr.GetString(iRegedtipcsv);

            int iRegedfecha = dr.GetOrdinal(this.Regedfecha);
            if (!dr.IsDBNull(iRegedfecha)) entity.Regedfecha = dr.GetDateTime(iRegedfecha);


            int iRegedh1 = dr.GetOrdinal(this.Regedh1);
            if (!dr.IsDBNull(iRegedh1)) entity.Regedh1 = dr.GetDecimal(iRegedh1);

            int iRegedh2 = dr.GetOrdinal(this.Regedh2);
            if (!dr.IsDBNull(iRegedh2)) entity.Regedh2 = dr.GetDecimal(iRegedh2);

            int iRegedh3 = dr.GetOrdinal(this.Regedh3);
            if (!dr.IsDBNull(iRegedh3)) entity.Regedh3 = dr.GetDecimal(iRegedh3);

            int iRegedh4 = dr.GetOrdinal(this.Regedh4);
            if (!dr.IsDBNull(iRegedh4)) entity.Regedh4 = dr.GetDecimal(iRegedh4);

            int iRegedh5 = dr.GetOrdinal(this.Regedh5);
            if (!dr.IsDBNull(iRegedh5)) entity.Regedh5 = dr.GetDecimal(iRegedh5);

            int iRegedh6 = dr.GetOrdinal(this.Regedh6);
            if (!dr.IsDBNull(iRegedh6)) entity.Regedh6 = dr.GetDecimal(iRegedh6);

            int iRegedh7 = dr.GetOrdinal(this.Regedh7);
            if (!dr.IsDBNull(iRegedh7)) entity.Regedh7 = dr.GetDecimal(iRegedh7);

            int iRegedh8 = dr.GetOrdinal(this.Regedh8);
            if (!dr.IsDBNull(iRegedh8)) entity.Regedh8 = dr.GetDecimal(iRegedh8);

            int iRegedh9 = dr.GetOrdinal(this.Regedh9);
            if (!dr.IsDBNull(iRegedh9)) entity.Regedh9 = dr.GetDecimal(iRegedh9);

            int iRegedh10 = dr.GetOrdinal(this.Regedh10);
            if (!dr.IsDBNull(iRegedh10)) entity.Regedh10 = dr.GetDecimal(iRegedh10);

            int iRegedh11 = dr.GetOrdinal(this.Regedh11);
            if (!dr.IsDBNull(iRegedh11)) entity.Regedh11 = dr.GetDecimal(iRegedh11);

            int iRegedh12 = dr.GetOrdinal(this.Regedh12);
            if (!dr.IsDBNull(iRegedh12)) entity.Regedh12 = dr.GetDecimal(iRegedh12);

            int iRegedh13 = dr.GetOrdinal(this.Regedh13);
            if (!dr.IsDBNull(iRegedh13)) entity.Regedh13 = dr.GetDecimal(iRegedh13);

            int iRegedh14 = dr.GetOrdinal(this.Regedh14);
            if (!dr.IsDBNull(iRegedh14)) entity.Regedh14 = dr.GetDecimal(iRegedh14);

            int iRegedh15 = dr.GetOrdinal(this.Regedh15);
            if (!dr.IsDBNull(iRegedh15)) entity.Regedh15 = dr.GetDecimal(iRegedh15);

            int iRegedh16 = dr.GetOrdinal(this.Regedh16);
            if (!dr.IsDBNull(iRegedh16)) entity.Regedh16 = dr.GetDecimal(iRegedh16);

            int iRegedh17 = dr.GetOrdinal(this.Regedh17);
            if (!dr.IsDBNull(iRegedh17)) entity.Regedh17 = dr.GetDecimal(iRegedh17);

            int iRegedh18 = dr.GetOrdinal(this.Regedh18);
            if (!dr.IsDBNull(iRegedh18)) entity.Regedh18 = dr.GetDecimal(iRegedh18);

            int iRegedh19 = dr.GetOrdinal(this.Regedh19);
            if (!dr.IsDBNull(iRegedh19)) entity.Regedh19 = dr.GetDecimal(iRegedh19);

            int iRegedh20 = dr.GetOrdinal(this.Regedh20);
            if (!dr.IsDBNull(iRegedh20)) entity.Regedh20 = dr.GetDecimal(iRegedh20);

            int iRegedh21 = dr.GetOrdinal(this.Regedh21);
            if (!dr.IsDBNull(iRegedh21)) entity.Regedh21 = dr.GetDecimal(iRegedh21);

            int iRegedh22 = dr.GetOrdinal(this.Regedh22);
            if (!dr.IsDBNull(iRegedh22)) entity.Regedh22 = dr.GetDecimal(iRegedh22);

            int iRegedh23 = dr.GetOrdinal(this.Regedh23);
            if (!dr.IsDBNull(iRegedh23)) entity.Regedh23 = dr.GetDecimal(iRegedh23);

            int iRegedh24 = dr.GetOrdinal(this.Regedh24);
            if (!dr.IsDBNull(iRegedh24)) entity.Regedh24 = dr.GetDecimal(iRegedh24);

            int iRegedh25 = dr.GetOrdinal(this.Regedh25);
            if (!dr.IsDBNull(iRegedh25)) entity.Regedh25 = dr.GetDecimal(iRegedh25);

            int iRegedh26 = dr.GetOrdinal(this.Regedh26);
            if (!dr.IsDBNull(iRegedh26)) entity.Regedh26 = dr.GetDecimal(iRegedh26);

            int iRegedh27 = dr.GetOrdinal(this.Regedh27);
            if (!dr.IsDBNull(iRegedh27)) entity.Regedh27 = dr.GetDecimal(iRegedh27);

            int iRegedh28 = dr.GetOrdinal(this.Regedh28);
            if (!dr.IsDBNull(iRegedh28)) entity.Regedh28 = dr.GetDecimal(iRegedh28);

            int iRegedh29 = dr.GetOrdinal(this.Regedh29);
            if (!dr.IsDBNull(iRegedh29)) entity.Regedh29 = dr.GetDecimal(iRegedh29);

            int iRegedh30 = dr.GetOrdinal(this.Regedh30);
            if (!dr.IsDBNull(iRegedh30)) entity.Regedh30 = dr.GetDecimal(iRegedh30);

            int iRegedh31 = dr.GetOrdinal(this.Regedh31);
            if (!dr.IsDBNull(iRegedh31)) entity.Regedh31 = dr.GetDecimal(iRegedh31);

            int iRegedh32 = dr.GetOrdinal(this.Regedh32);
            if (!dr.IsDBNull(iRegedh32)) entity.Regedh32 = dr.GetDecimal(iRegedh32);

            int iRegedh33 = dr.GetOrdinal(this.Regedh33);
            if (!dr.IsDBNull(iRegedh33)) entity.Regedh33 = dr.GetDecimal(iRegedh33);

            int iRegedh34 = dr.GetOrdinal(this.Regedh34);
            if (!dr.IsDBNull(iRegedh34)) entity.Regedh34 = dr.GetDecimal(iRegedh34);

            int iRegedh35 = dr.GetOrdinal(this.Regedh35);
            if (!dr.IsDBNull(iRegedh35)) entity.Regedh35 = dr.GetDecimal(iRegedh35);

            int iRegedh36 = dr.GetOrdinal(this.Regedh36);
            if (!dr.IsDBNull(iRegedh36)) entity.Regedh36 = dr.GetDecimal(iRegedh36);

            int iRegedh37 = dr.GetOrdinal(this.Regedh37);
            if (!dr.IsDBNull(iRegedh37)) entity.Regedh37 = dr.GetDecimal(iRegedh37);

            int iRegedh38 = dr.GetOrdinal(this.Regedh38);
            if (!dr.IsDBNull(iRegedh38)) entity.Regedh38 = dr.GetDecimal(iRegedh38);

            int iRegedh39 = dr.GetOrdinal(this.Regedh39);
            if (!dr.IsDBNull(iRegedh39)) entity.Regedh39 = dr.GetDecimal(iRegedh39);

            int iRegedh40 = dr.GetOrdinal(this.Regedh40);
            if (!dr.IsDBNull(iRegedh40)) entity.Regedh40 = dr.GetDecimal(iRegedh40);

            int iRegedh41 = dr.GetOrdinal(this.Regedh41);
            if (!dr.IsDBNull(iRegedh41)) entity.Regedh41 = dr.GetDecimal(iRegedh41);

            int iRegedh42 = dr.GetOrdinal(this.Regedh42);
            if (!dr.IsDBNull(iRegedh42)) entity.Regedh42 = dr.GetDecimal(iRegedh42);

            int iRegedh43 = dr.GetOrdinal(this.Regedh43);
            if (!dr.IsDBNull(iRegedh43)) entity.Regedh43 = dr.GetDecimal(iRegedh43);

            int iRegedh44 = dr.GetOrdinal(this.Regedh44);
            if (!dr.IsDBNull(iRegedh44)) entity.Regedh44 = dr.GetDecimal(iRegedh44);

            int iRegedh45 = dr.GetOrdinal(this.Regedh45);
            if (!dr.IsDBNull(iRegedh45)) entity.Regedh45 = dr.GetDecimal(iRegedh45);

            int iRegedh46 = dr.GetOrdinal(this.Regedh46);
            if (!dr.IsDBNull(iRegedh46)) entity.Regedh46 = dr.GetDecimal(iRegedh46);

            int iRegedh47 = dr.GetOrdinal(this.Regedh47);
            if (!dr.IsDBNull(iRegedh47)) entity.Regedh47 = dr.GetDecimal(iRegedh47);

            int iRegedh48 = dr.GetOrdinal(this.Regedh48);
            if (!dr.IsDBNull(iRegedh48)) entity.Regedh48 = dr.GetDecimal(iRegedh48);

            int iRegedh49 = dr.GetOrdinal(this.Regedh49);
            if (!dr.IsDBNull(iRegedh49)) entity.Regedh49 = dr.GetDecimal(iRegedh49);

            int iRegedh50 = dr.GetOrdinal(this.Regedh50);
            if (!dr.IsDBNull(iRegedh50)) entity.Regedh50 = dr.GetDecimal(iRegedh50);

            int iRegedh51 = dr.GetOrdinal(this.Regedh51);
            if (!dr.IsDBNull(iRegedh51)) entity.Regedh51 = dr.GetDecimal(iRegedh51);

            int iRegedh52 = dr.GetOrdinal(this.Regedh52);
            if (!dr.IsDBNull(iRegedh52)) entity.Regedh52 = dr.GetDecimal(iRegedh52);

            int iRegedh53 = dr.GetOrdinal(this.Regedh53);
            if (!dr.IsDBNull(iRegedh53)) entity.Regedh53 = dr.GetDecimal(iRegedh53);

            int iRegedh54 = dr.GetOrdinal(this.Regedh54);
            if (!dr.IsDBNull(iRegedh54)) entity.Regedh54 = dr.GetDecimal(iRegedh54);

            int iRegedh55 = dr.GetOrdinal(this.Regedh55);
            if (!dr.IsDBNull(iRegedh55)) entity.Regedh55 = dr.GetDecimal(iRegedh55);

            int iRegedh56 = dr.GetOrdinal(this.Regedh56);
            if (!dr.IsDBNull(iRegedh56)) entity.Regedh56 = dr.GetDecimal(iRegedh56);

            int iRegedh57 = dr.GetOrdinal(this.Regedh57);
            if (!dr.IsDBNull(iRegedh57)) entity.Regedh57 = dr.GetDecimal(iRegedh57);

            int iRegedh58 = dr.GetOrdinal(this.Regedh58);
            if (!dr.IsDBNull(iRegedh58)) entity.Regedh58 = dr.GetDecimal(iRegedh58);

            int iRegedh59 = dr.GetOrdinal(this.Regedh59);
            if (!dr.IsDBNull(iRegedh59)) entity.Regedh59 = dr.GetDecimal(iRegedh59);

            int iRegedh60 = dr.GetOrdinal(this.Regedh60);
            if (!dr.IsDBNull(iRegedh60)) entity.Regedh60 = dr.GetDecimal(iRegedh60);

            int iRegedh61 = dr.GetOrdinal(this.Regedh61);
            if (!dr.IsDBNull(iRegedh61)) entity.Regedh61 = dr.GetDecimal(iRegedh61);

            int iRegedh62 = dr.GetOrdinal(this.Regedh62);
            if (!dr.IsDBNull(iRegedh62)) entity.Regedh62 = dr.GetDecimal(iRegedh62);

            int iRegedh63 = dr.GetOrdinal(this.Regedh63);
            if (!dr.IsDBNull(iRegedh63)) entity.Regedh63 = dr.GetDecimal(iRegedh63);

            int iRegedh64 = dr.GetOrdinal(this.Regedh64);
            if (!dr.IsDBNull(iRegedh64)) entity.Regedh64 = dr.GetDecimal(iRegedh64);

            int iRegedh65 = dr.GetOrdinal(this.Regedh65);
            if (!dr.IsDBNull(iRegedh65)) entity.Regedh65 = dr.GetDecimal(iRegedh65);

            int iRegedh66 = dr.GetOrdinal(this.Regedh66);
            if (!dr.IsDBNull(iRegedh66)) entity.Regedh66 = dr.GetDecimal(iRegedh66);

            int iRegedh67 = dr.GetOrdinal(this.Regedh67);
            if (!dr.IsDBNull(iRegedh67)) entity.Regedh67 = dr.GetDecimal(iRegedh67);

            int iRegedh68 = dr.GetOrdinal(this.Regedh68);
            if (!dr.IsDBNull(iRegedh68)) entity.Regedh68 = dr.GetDecimal(iRegedh68);

            int iRegedh69 = dr.GetOrdinal(this.Regedh69);
            if (!dr.IsDBNull(iRegedh69)) entity.Regedh69 = dr.GetDecimal(iRegedh69);

            int iRegedh70 = dr.GetOrdinal(this.Regedh70);
            if (!dr.IsDBNull(iRegedh70)) entity.Regedh70 = dr.GetDecimal(iRegedh70);

            int iRegedh71 = dr.GetOrdinal(this.Regedh71);
            if (!dr.IsDBNull(iRegedh71)) entity.Regedh71 = dr.GetDecimal(iRegedh71);

            int iRegedh72 = dr.GetOrdinal(this.Regedh72);
            if (!dr.IsDBNull(iRegedh72)) entity.Regedh72 = dr.GetDecimal(iRegedh72);

            int iRegedh73 = dr.GetOrdinal(this.Regedh73);
            if (!dr.IsDBNull(iRegedh73)) entity.Regedh73 = dr.GetDecimal(iRegedh73);

            int iRegedh74 = dr.GetOrdinal(this.Regedh74);
            if (!dr.IsDBNull(iRegedh74)) entity.Regedh74 = dr.GetDecimal(iRegedh74);

            int iRegedh75 = dr.GetOrdinal(this.Regedh75);
            if (!dr.IsDBNull(iRegedh75)) entity.Regedh75 = dr.GetDecimal(iRegedh75);

            int iRegedh76 = dr.GetOrdinal(this.Regedh76);
            if (!dr.IsDBNull(iRegedh76)) entity.Regedh76 = dr.GetDecimal(iRegedh76);

            int iRegedh77 = dr.GetOrdinal(this.Regedh77);
            if (!dr.IsDBNull(iRegedh77)) entity.Regedh77 = dr.GetDecimal(iRegedh77);

            int iRegedh78 = dr.GetOrdinal(this.Regedh78);
            if (!dr.IsDBNull(iRegedh78)) entity.Regedh78 = dr.GetDecimal(iRegedh78);

            int iRegedh79 = dr.GetOrdinal(this.Regedh79);
            if (!dr.IsDBNull(iRegedh79)) entity.Regedh79 = dr.GetDecimal(iRegedh79);

            int iRegedh80 = dr.GetOrdinal(this.Regedh80);
            if (!dr.IsDBNull(iRegedh80)) entity.Regedh80 = dr.GetDecimal(iRegedh80);

            int iRegedh81 = dr.GetOrdinal(this.Regedh81);
            if (!dr.IsDBNull(iRegedh81)) entity.Regedh81 = dr.GetDecimal(iRegedh81);

            int iRegedh82 = dr.GetOrdinal(this.Regedh82);
            if (!dr.IsDBNull(iRegedh82)) entity.Regedh82 = dr.GetDecimal(iRegedh82);

            int iRegedh83 = dr.GetOrdinal(this.Regedh83);
            if (!dr.IsDBNull(iRegedh83)) entity.Regedh83 = dr.GetDecimal(iRegedh83);

            int iRegedh84 = dr.GetOrdinal(this.Regedh84);
            if (!dr.IsDBNull(iRegedh84)) entity.Regedh84 = dr.GetDecimal(iRegedh84);

            int iRegedh85 = dr.GetOrdinal(this.Regedh85);
            if (!dr.IsDBNull(iRegedh85)) entity.Regedh85 = dr.GetDecimal(iRegedh85);

            int iRegedh86 = dr.GetOrdinal(this.Regedh86);
            if (!dr.IsDBNull(iRegedh86)) entity.Regedh86 = dr.GetDecimal(iRegedh86);

            int iRegedh87 = dr.GetOrdinal(this.Regedh87);
            if (!dr.IsDBNull(iRegedh87)) entity.Regedh87 = dr.GetDecimal(iRegedh87);

            int iRegedh88 = dr.GetOrdinal(this.Regedh88);
            if (!dr.IsDBNull(iRegedh88)) entity.Regedh88 = dr.GetDecimal(iRegedh88);

            int iRegedh89 = dr.GetOrdinal(this.Regedh89);
            if (!dr.IsDBNull(iRegedh89)) entity.Regedh89 = dr.GetDecimal(iRegedh89);

            int iRegedh90 = dr.GetOrdinal(this.Regedh90);
            if (!dr.IsDBNull(iRegedh90)) entity.Regedh90 = dr.GetDecimal(iRegedh90);

            int iRegedh91 = dr.GetOrdinal(this.Regedh91);
            if (!dr.IsDBNull(iRegedh91)) entity.Regedh91 = dr.GetDecimal(iRegedh91);

            int iRegedh92 = dr.GetOrdinal(this.Regedh92);
            if (!dr.IsDBNull(iRegedh92)) entity.Regedh92 = dr.GetDecimal(iRegedh92);

            int iRegedh93 = dr.GetOrdinal(this.Regedh93);
            if (!dr.IsDBNull(iRegedh93)) entity.Regedh93 = dr.GetDecimal(iRegedh93);

            int iRegedh94 = dr.GetOrdinal(this.Regedh94);
            if (!dr.IsDBNull(iRegedh94)) entity.Regedh94 = dr.GetDecimal(iRegedh94);

            int iRegedh95 = dr.GetOrdinal(this.Regedh95);
            if (!dr.IsDBNull(iRegedh95)) entity.Regedh95 = dr.GetDecimal(iRegedh95);

            int iRegedh96 = dr.GetOrdinal(this.Regedh96);
            if (!dr.IsDBNull(iRegedh96)) entity.Regedh96 = dr.GetDecimal(iRegedh96);

            int iRegedususcreacion = dr.GetOrdinal(this.Regedusucreacion);
            if (!dr.IsDBNull(iRegedususcreacion)) entity.Regedusucreacion = dr.GetString(iRegedususcreacion);

            int iRegedfeccreacion = dr.GetOrdinal(this.Regedfeccreacion);
            if (!dr.IsDBNull(iRegedfeccreacion)) entity.Regedfeccreacion = dr.GetDateTime(iRegedfeccreacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Regedcodi = "REGEDCODI";
        public string Regercodi = "REGERCODI";

        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Regedtipcsv = "REGEDTIPCSV";
        public string Regedfecha = "REGEDFECHA";

        public string Regedh1 = "REGEDH1";
        public string Regedh2 = "REGEDH2";
        public string Regedh3 = "REGEDH3";
        public string Regedh4 = "REGEDH4";
        public string Regedh5 = "REGEDH5";
        public string Regedh6 = "REGEDH6";
        public string Regedh7 = "REGEDH7";
        public string Regedh8 = "REGEDH8";
        public string Regedh9 = "REGEDH9";
        public string Regedh10 = "REGEDH10";
        public string Regedh11 = "REGEDH11";
        public string Regedh12 = "REGEDH12";
        public string Regedh13 = "REGEDH13";
        public string Regedh14 = "REGEDH14";
        public string Regedh15 = "REGEDH15";
        public string Regedh16 = "REGEDH16";
        public string Regedh17 = "REGEDH17";
        public string Regedh18 = "REGEDH18";
        public string Regedh19 = "REGEDH19";
        public string Regedh20 = "REGEDH20";
        public string Regedh21 = "REGEDH21";
        public string Regedh22 = "REGEDH22";
        public string Regedh23 = "REGEDH23";
        public string Regedh24 = "REGEDH24";
        public string Regedh25 = "REGEDH25";
        public string Regedh26 = "REGEDH26";
        public string Regedh27 = "REGEDH27";
        public string Regedh28 = "REGEDH28";
        public string Regedh29 = "REGEDH29";
        public string Regedh30 = "REGEDH30";
        public string Regedh31 = "REGEDH31";
        public string Regedh32 = "REGEDH32";
        public string Regedh33 = "REGEDH33";
        public string Regedh34 = "REGEDH34";
        public string Regedh35 = "REGEDH35";
        public string Regedh36 = "REGEDH36";
        public string Regedh37 = "REGEDH37";
        public string Regedh38 = "REGEDH38";
        public string Regedh39 = "REGEDH39";
        public string Regedh40 = "REGEDH40";
        public string Regedh41 = "REGEDH41";
        public string Regedh42 = "REGEDH42";
        public string Regedh43 = "REGEDH43";
        public string Regedh44 = "REGEDH44";
        public string Regedh45 = "REGEDH45";
        public string Regedh46 = "REGEDH46";
        public string Regedh47 = "REGEDH47";
        public string Regedh48 = "REGEDH48";
        public string Regedh49 = "REGEDH49";
        public string Regedh50 = "REGEDH50";
        public string Regedh51 = "REGEDH51";
        public string Regedh52 = "REGEDH52";
        public string Regedh53 = "REGEDH53";
        public string Regedh54 = "REGEDH54";
        public string Regedh55 = "REGEDH55";
        public string Regedh56 = "REGEDH56";
        public string Regedh57 = "REGEDH57";
        public string Regedh58 = "REGEDH58";
        public string Regedh59 = "REGEDH59";
        public string Regedh60 = "REGEDH60";
        public string Regedh61 = "REGEDH61";
        public string Regedh62 = "REGEDH62";
        public string Regedh63 = "REGEDH63";
        public string Regedh64 = "REGEDH64";
        public string Regedh65 = "REGEDH65";
        public string Regedh66 = "REGEDH66";
        public string Regedh67 = "REGEDH67";
        public string Regedh68 = "REGEDH68";
        public string Regedh69 = "REGEDH69";
        public string Regedh70 = "REGEDH70";
        public string Regedh71 = "REGEDH71";
        public string Regedh72 = "REGEDH72";
        public string Regedh73 = "REGEDH73";
        public string Regedh74 = "REGEDH74";
        public string Regedh75 = "REGEDH75";
        public string Regedh76 = "REGEDH76";
        public string Regedh77 = "REGEDH77";
        public string Regedh78 = "REGEDH78";
        public string Regedh79 = "REGEDH79";
        public string Regedh80 = "REGEDH80";
        public string Regedh81 = "REGEDH81";
        public string Regedh82 = "REGEDH82";
        public string Regedh83 = "REGEDH83";
        public string Regedh84 = "REGEDH84";
        public string Regedh85 = "REGEDH85";
        public string Regedh86 = "REGEDH86";
        public string Regedh87 = "REGEDH87";
        public string Regedh88 = "REGEDH88";
        public string Regedh89 = "REGEDH89";
        public string Regedh90 = "REGEDH90";
        public string Regedh91 = "REGEDH91";
        public string Regedh92 = "REGEDH92";
        public string Regedh93 = "REGEDH93";
        public string Regedh94 = "REGEDH94";
        public string Regedh95 = "REGEDH95";
        public string Regedh96 = "REGEDH96";
        public string Regedusucreacion = "REGEDUSUCREACION";
        public string Regedfeccreacion = "REGEDFECCREACION";

        public string Equinomb = "EQUINOMB";
        public string Emprnomb = "EMPRNOMB";
        #endregion

        #region Querys
        public string SqlListByEquipo
        {
            get { return base.GetSqlXml("ListByEquipo"); }
        }

        #endregion

        public string SqlListByEmprcodiEquicodiTipo
        {
            get { return base.GetSqlXml("ListByEmprcodiEquicodiTipo"); }
        }
    }
}

