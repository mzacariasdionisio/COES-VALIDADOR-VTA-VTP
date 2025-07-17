using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_INSUMO_DIA
    /// </summary>
    public class CpaInsumoDiaHelper : HelperBase
    {
        public CpaInsumoDiaHelper() : base(Consultas.CpaInsumoDiaSql)
        {
        }

        public CpaInsumoDiaDTO Create(IDataReader dr)
        {
            CpaInsumoDiaDTO entity = new CpaInsumoDiaDTO();

            int iCpaindcodi = dr.GetOrdinal(Cpaindcodi);
            if (!dr.IsDBNull(iCpaindcodi)) entity.Cpaindcodi = dr.GetInt32(iCpaindcodi);

            int iCpainmcodi = dr.GetOrdinal(Cpainmcodi);
            if (!dr.IsDBNull(iCpainmcodi)) entity.Cpainmcodi = dr.GetInt32(iCpainmcodi);

            int iCparcodi = dr.GetOrdinal(Cparcodi);
            if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

            int iCpaindtipinsumo = dr.GetOrdinal(Cpaindtipinsumo);
            if (!dr.IsDBNull(iCpaindtipinsumo)) entity.Cpaindtipinsumo = dr.GetString(iCpaindtipinsumo);

            int iCpaindtipproceso = dr.GetOrdinal(Cpaindtipproceso);
            if (!dr.IsDBNull(iCpaindtipproceso)) entity.Cpaindtipproceso = dr.GetString(iCpaindtipproceso);

            int iEmprcodi = dr.GetOrdinal(Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

            int iEquicodi = dr.GetOrdinal(Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

            int iCpainddia = dr.GetOrdinal(Cpainddia);
            if (!dr.IsDBNull(iCpainddia)) entity.Cpainddia = dr.GetDateTime(iCpainddia);

            int iCpaindtotaldia = dr.GetOrdinal(Cpaindtotaldia);
            if (!dr.IsDBNull(iCpaindtotaldia)) entity.Cpaindtotaldia = dr.GetDecimal(iCpaindtotaldia);

            int iCpaindh1 = dr.GetOrdinal(Cpaindh1);
            if (!dr.IsDBNull(iCpaindh1)) entity.Cpaindh1 = dr.GetDecimal(iCpaindh1);

            int iCpaindh2 = dr.GetOrdinal(Cpaindh2);
            if (!dr.IsDBNull(iCpaindh2)) entity.Cpaindh2 = dr.GetDecimal(iCpaindh2);

            int iCpaindh3 = dr.GetOrdinal(Cpaindh3);
            if (!dr.IsDBNull(iCpaindh3)) entity.Cpaindh3 = dr.GetDecimal(iCpaindh3);

            int iCpaindh4 = dr.GetOrdinal(Cpaindh4);
            if (!dr.IsDBNull(iCpaindh4)) entity.Cpaindh4 = dr.GetDecimal(iCpaindh4);

            int iCpaindh5 = dr.GetOrdinal(Cpaindh5);
            if (!dr.IsDBNull(iCpaindh5)) entity.Cpaindh5 = dr.GetDecimal(iCpaindh5);

            int iCpaindh6 = dr.GetOrdinal(Cpaindh6);
            if (!dr.IsDBNull(iCpaindh6)) entity.Cpaindh6 = dr.GetDecimal(iCpaindh6);

            int iCpaindh7 = dr.GetOrdinal(Cpaindh7);
            if (!dr.IsDBNull(iCpaindh7)) entity.Cpaindh7 = dr.GetDecimal(iCpaindh7);

            int iCpaindh8 = dr.GetOrdinal(Cpaindh8);
            if (!dr.IsDBNull(iCpaindh8)) entity.Cpaindh8 = dr.GetDecimal(iCpaindh8);

            int iCpaindh9 = dr.GetOrdinal(Cpaindh9);
            if (!dr.IsDBNull(iCpaindh9)) entity.Cpaindh9 = dr.GetDecimal(iCpaindh9);

            int iCpaindh10 = dr.GetOrdinal(Cpaindh10);
            if (!dr.IsDBNull(iCpaindh10)) entity.Cpaindh10 = dr.GetDecimal(iCpaindh10);

            int iCpaindh11 = dr.GetOrdinal(Cpaindh11);
            if (!dr.IsDBNull(iCpaindh11)) entity.Cpaindh11 = dr.GetDecimal(iCpaindh11);

            int iCpaindh12 = dr.GetOrdinal(Cpaindh12);
            if (!dr.IsDBNull(iCpaindh12)) entity.Cpaindh12 = dr.GetDecimal(iCpaindh12);

            int iCpaindh13 = dr.GetOrdinal(Cpaindh13);
            if (!dr.IsDBNull(iCpaindh13)) entity.Cpaindh13 = dr.GetDecimal(iCpaindh13);

            int iCpaindh14 = dr.GetOrdinal(Cpaindh14);
            if (!dr.IsDBNull(iCpaindh14)) entity.Cpaindh14 = dr.GetDecimal(iCpaindh14);

            int iCpaindh15 = dr.GetOrdinal(Cpaindh15);
            if (!dr.IsDBNull(iCpaindh15)) entity.Cpaindh15 = dr.GetDecimal(iCpaindh15);

            int iCpaindh16 = dr.GetOrdinal(Cpaindh16);
            if (!dr.IsDBNull(iCpaindh16)) entity.Cpaindh16 = dr.GetDecimal(iCpaindh16);

            int iCpaindh17 = dr.GetOrdinal(Cpaindh17);
            if (!dr.IsDBNull(iCpaindh17)) entity.Cpaindh17 = dr.GetDecimal(iCpaindh17);

            int iCpaindh18 = dr.GetOrdinal(Cpaindh18);
            if (!dr.IsDBNull(iCpaindh18)) entity.Cpaindh18 = dr.GetDecimal(iCpaindh18);

            int iCpaindh19 = dr.GetOrdinal(Cpaindh19);
            if (!dr.IsDBNull(iCpaindh19)) entity.Cpaindh19 = dr.GetDecimal(iCpaindh19);

            int iCpaindh20 = dr.GetOrdinal(Cpaindh20);
            if (!dr.IsDBNull(iCpaindh20)) entity.Cpaindh20 = dr.GetDecimal(iCpaindh20);

            int iCpaindh21 = dr.GetOrdinal(Cpaindh21);
            if (!dr.IsDBNull(iCpaindh21)) entity.Cpaindh21 = dr.GetDecimal(iCpaindh21);

            int iCpaindh22 = dr.GetOrdinal(Cpaindh22);
            if (!dr.IsDBNull(iCpaindh22)) entity.Cpaindh22 = dr.GetDecimal(iCpaindh22);

            int iCpaindh23 = dr.GetOrdinal(Cpaindh23);
            if (!dr.IsDBNull(iCpaindh23)) entity.Cpaindh23 = dr.GetDecimal(iCpaindh23);

            int iCpaindh24 = dr.GetOrdinal(Cpaindh24);
            if (!dr.IsDBNull(iCpaindh24)) entity.Cpaindh24 = dr.GetDecimal(iCpaindh24);

            int iCpaindh25 = dr.GetOrdinal(Cpaindh25);
            if (!dr.IsDBNull(iCpaindh25)) entity.Cpaindh25 = dr.GetDecimal(iCpaindh25);

            int iCpaindh26 = dr.GetOrdinal(Cpaindh26);
            if (!dr.IsDBNull(iCpaindh26)) entity.Cpaindh26 = dr.GetDecimal(iCpaindh26);

            int iCpaindh27 = dr.GetOrdinal(Cpaindh27);
            if (!dr.IsDBNull(iCpaindh27)) entity.Cpaindh27 = dr.GetDecimal(iCpaindh27);

            int iCpaindh28 = dr.GetOrdinal(Cpaindh28);
            if (!dr.IsDBNull(iCpaindh28)) entity.Cpaindh28 = dr.GetDecimal(iCpaindh28);

            int iCpaindh29 = dr.GetOrdinal(Cpaindh29);
            if (!dr.IsDBNull(iCpaindh29)) entity.Cpaindh29 = dr.GetDecimal(iCpaindh29);

            int iCpaindh30 = dr.GetOrdinal(Cpaindh30);
            if (!dr.IsDBNull(iCpaindh30)) entity.Cpaindh30 = dr.GetDecimal(iCpaindh30);

            int iCpaindh31 = dr.GetOrdinal(Cpaindh31);
            if (!dr.IsDBNull(iCpaindh31)) entity.Cpaindh31 = dr.GetDecimal(iCpaindh31);

            int iCpaindh32 = dr.GetOrdinal(Cpaindh32);
            if (!dr.IsDBNull(iCpaindh32)) entity.Cpaindh32 = dr.GetDecimal(iCpaindh32);

            int iCpaindh33 = dr.GetOrdinal(Cpaindh33);
            if (!dr.IsDBNull(iCpaindh33)) entity.Cpaindh33 = dr.GetDecimal(iCpaindh33);

            int iCpaindh34 = dr.GetOrdinal(Cpaindh34);
            if (!dr.IsDBNull(iCpaindh34)) entity.Cpaindh34 = dr.GetDecimal(iCpaindh34);

            int iCpaindh35 = dr.GetOrdinal(Cpaindh35);
            if (!dr.IsDBNull(iCpaindh35)) entity.Cpaindh35 = dr.GetDecimal(iCpaindh35);

            int iCpaindh36 = dr.GetOrdinal(Cpaindh36);
            if (!dr.IsDBNull(iCpaindh36)) entity.Cpaindh36 = dr.GetDecimal(iCpaindh36);

            int iCpaindh37 = dr.GetOrdinal(Cpaindh37);
            if (!dr.IsDBNull(iCpaindh37)) entity.Cpaindh37 = dr.GetDecimal(iCpaindh37);

            int iCpaindh38 = dr.GetOrdinal(Cpaindh38);
            if (!dr.IsDBNull(iCpaindh38)) entity.Cpaindh38 = dr.GetDecimal(iCpaindh38);

            int iCpaindh39 = dr.GetOrdinal(Cpaindh39);
            if (!dr.IsDBNull(iCpaindh39)) entity.Cpaindh39 = dr.GetDecimal(iCpaindh39);

            int iCpaindh40 = dr.GetOrdinal(Cpaindh40);
            if (!dr.IsDBNull(iCpaindh40)) entity.Cpaindh40 = dr.GetDecimal(iCpaindh40);

            int iCpaindh41 = dr.GetOrdinal(Cpaindh41);
            if (!dr.IsDBNull(iCpaindh41)) entity.Cpaindh41 = dr.GetDecimal(iCpaindh41);

            int iCpaindh42 = dr.GetOrdinal(Cpaindh42);
            if (!dr.IsDBNull(iCpaindh42)) entity.Cpaindh42 = dr.GetDecimal(iCpaindh42);

            int iCpaindh43 = dr.GetOrdinal(Cpaindh43);
            if (!dr.IsDBNull(iCpaindh43)) entity.Cpaindh43 = dr.GetDecimal(iCpaindh43);

            int iCpaindh44 = dr.GetOrdinal(Cpaindh44);
            if (!dr.IsDBNull(iCpaindh44)) entity.Cpaindh44 = dr.GetDecimal(iCpaindh44);

            int iCpaindh45 = dr.GetOrdinal(Cpaindh45);
            if (!dr.IsDBNull(iCpaindh45)) entity.Cpaindh45 = dr.GetDecimal(iCpaindh45);

            int iCpaindh46 = dr.GetOrdinal(Cpaindh46);
            if (!dr.IsDBNull(iCpaindh46)) entity.Cpaindh46 = dr.GetDecimal(iCpaindh46);

            int iCpaindh47 = dr.GetOrdinal(Cpaindh47);
            if (!dr.IsDBNull(iCpaindh47)) entity.Cpaindh47 = dr.GetDecimal(iCpaindh47);

            int iCpaindh48 = dr.GetOrdinal(Cpaindh48);
            if (!dr.IsDBNull(iCpaindh48)) entity.Cpaindh48 = dr.GetDecimal(iCpaindh48);

            int iCpaindh49 = dr.GetOrdinal(Cpaindh49);
            if (!dr.IsDBNull(iCpaindh49)) entity.Cpaindh49 = dr.GetDecimal(iCpaindh49);

            int iCpaindh50 = dr.GetOrdinal(Cpaindh50);
            if (!dr.IsDBNull(iCpaindh50)) entity.Cpaindh50 = dr.GetDecimal(iCpaindh50);

            int iCpaindh51 = dr.GetOrdinal(Cpaindh51);
            if (!dr.IsDBNull(iCpaindh51)) entity.Cpaindh51 = dr.GetDecimal(iCpaindh51);

            int iCpaindh52 = dr.GetOrdinal(Cpaindh52);
            if (!dr.IsDBNull(iCpaindh52)) entity.Cpaindh52 = dr.GetDecimal(iCpaindh52);

            int iCpaindh53 = dr.GetOrdinal(Cpaindh53);
            if (!dr.IsDBNull(iCpaindh53)) entity.Cpaindh53 = dr.GetDecimal(iCpaindh53);

            int iCpaindh54 = dr.GetOrdinal(Cpaindh54);
            if (!dr.IsDBNull(iCpaindh54)) entity.Cpaindh54 = dr.GetDecimal(iCpaindh54);

            int iCpaindh55 = dr.GetOrdinal(Cpaindh55);
            if (!dr.IsDBNull(iCpaindh55)) entity.Cpaindh55 = dr.GetDecimal(iCpaindh55);

            int iCpaindh56 = dr.GetOrdinal(Cpaindh56);
            if (!dr.IsDBNull(iCpaindh56)) entity.Cpaindh56 = dr.GetDecimal(iCpaindh56);

            int iCpaindh57 = dr.GetOrdinal(Cpaindh57);
            if (!dr.IsDBNull(iCpaindh57)) entity.Cpaindh57 = dr.GetDecimal(iCpaindh57);

            int iCpaindh58 = dr.GetOrdinal(Cpaindh58);
            if (!dr.IsDBNull(iCpaindh58)) entity.Cpaindh58 = dr.GetDecimal(iCpaindh58);

            int iCpaindh59 = dr.GetOrdinal(Cpaindh59);
            if (!dr.IsDBNull(iCpaindh59)) entity.Cpaindh59 = dr.GetDecimal(iCpaindh59);

            int iCpaindh60 = dr.GetOrdinal(Cpaindh60);
            if (!dr.IsDBNull(iCpaindh60)) entity.Cpaindh60 = dr.GetDecimal(iCpaindh60);

            int iCpaindh61 = dr.GetOrdinal(Cpaindh61);
            if (!dr.IsDBNull(iCpaindh61)) entity.Cpaindh61 = dr.GetDecimal(iCpaindh61);

            int iCpaindh62 = dr.GetOrdinal(Cpaindh62);
            if (!dr.IsDBNull(iCpaindh62)) entity.Cpaindh62 = dr.GetDecimal(iCpaindh62);

            int iCpaindh63 = dr.GetOrdinal(Cpaindh63);
            if (!dr.IsDBNull(iCpaindh63)) entity.Cpaindh63 = dr.GetDecimal(iCpaindh63);

            int iCpaindh64 = dr.GetOrdinal(Cpaindh64);
            if (!dr.IsDBNull(iCpaindh64)) entity.Cpaindh64 = dr.GetDecimal(iCpaindh64);

            int iCpaindh65 = dr.GetOrdinal(Cpaindh65);
            if (!dr.IsDBNull(iCpaindh65)) entity.Cpaindh65 = dr.GetDecimal(iCpaindh65);

            int iCpaindh66 = dr.GetOrdinal(Cpaindh66);
            if (!dr.IsDBNull(iCpaindh66)) entity.Cpaindh66 = dr.GetDecimal(iCpaindh66);

            int iCpaindh67 = dr.GetOrdinal(Cpaindh67);
            if (!dr.IsDBNull(iCpaindh67)) entity.Cpaindh67 = dr.GetDecimal(iCpaindh67);

            int iCpaindh68 = dr.GetOrdinal(Cpaindh68);
            if (!dr.IsDBNull(iCpaindh68)) entity.Cpaindh68 = dr.GetDecimal(iCpaindh68);

            int iCpaindh69 = dr.GetOrdinal(Cpaindh69);
            if (!dr.IsDBNull(iCpaindh69)) entity.Cpaindh69 = dr.GetDecimal(iCpaindh69);

            int iCpaindh70 = dr.GetOrdinal(Cpaindh70);
            if (!dr.IsDBNull(iCpaindh70)) entity.Cpaindh70 = dr.GetDecimal(iCpaindh70);

            int iCpaindh71 = dr.GetOrdinal(Cpaindh71);
            if (!dr.IsDBNull(iCpaindh71)) entity.Cpaindh71 = dr.GetDecimal(iCpaindh71);

            int iCpaindh72 = dr.GetOrdinal(Cpaindh72);
            if (!dr.IsDBNull(iCpaindh72)) entity.Cpaindh72 = dr.GetDecimal(iCpaindh72);

            int iCpaindh73 = dr.GetOrdinal(Cpaindh73);
            if (!dr.IsDBNull(iCpaindh73)) entity.Cpaindh73 = dr.GetDecimal(iCpaindh73);

            int iCpaindh74 = dr.GetOrdinal(Cpaindh74);
            if (!dr.IsDBNull(iCpaindh74)) entity.Cpaindh74 = dr.GetDecimal(iCpaindh74);

            int iCpaindh75 = dr.GetOrdinal(Cpaindh75);
            if (!dr.IsDBNull(iCpaindh75)) entity.Cpaindh75 = dr.GetDecimal(iCpaindh75);

            int iCpaindh76 = dr.GetOrdinal(Cpaindh76);
            if (!dr.IsDBNull(iCpaindh76)) entity.Cpaindh76 = dr.GetDecimal(iCpaindh76);

            int iCpaindh77 = dr.GetOrdinal(Cpaindh77);
            if (!dr.IsDBNull(iCpaindh77)) entity.Cpaindh77 = dr.GetDecimal(iCpaindh77);

            int iCpaindh78 = dr.GetOrdinal(Cpaindh78);
            if (!dr.IsDBNull(iCpaindh78)) entity.Cpaindh78 = dr.GetDecimal(iCpaindh78);

            int iCpaindh79 = dr.GetOrdinal(Cpaindh79);
            if (!dr.IsDBNull(iCpaindh79)) entity.Cpaindh79 = dr.GetDecimal(iCpaindh79);

            int iCpaindh80 = dr.GetOrdinal(Cpaindh80);
            if (!dr.IsDBNull(iCpaindh80)) entity.Cpaindh80 = dr.GetDecimal(iCpaindh80);

            int iCpaindh81 = dr.GetOrdinal(Cpaindh81);
            if (!dr.IsDBNull(iCpaindh81)) entity.Cpaindh81 = dr.GetDecimal(iCpaindh81);

            int iCpaindh82 = dr.GetOrdinal(Cpaindh82);
            if (!dr.IsDBNull(iCpaindh82)) entity.Cpaindh82 = dr.GetDecimal(iCpaindh82);

            int iCpaindh83 = dr.GetOrdinal(Cpaindh83);
            if (!dr.IsDBNull(iCpaindh83)) entity.Cpaindh83 = dr.GetDecimal(iCpaindh83);

            int iCpaindh84 = dr.GetOrdinal(Cpaindh84);
            if (!dr.IsDBNull(iCpaindh84)) entity.Cpaindh84 = dr.GetDecimal(iCpaindh84);

            int iCpaindh85 = dr.GetOrdinal(Cpaindh85);
            if (!dr.IsDBNull(iCpaindh85)) entity.Cpaindh85 = dr.GetDecimal(iCpaindh85);

            int iCpaindh86 = dr.GetOrdinal(Cpaindh86);
            if (!dr.IsDBNull(iCpaindh86)) entity.Cpaindh86 = dr.GetDecimal(iCpaindh86);

            int iCpaindh87 = dr.GetOrdinal(Cpaindh87);
            if (!dr.IsDBNull(iCpaindh87)) entity.Cpaindh87 = dr.GetDecimal(iCpaindh87);

            int iCpaindh88 = dr.GetOrdinal(Cpaindh88);
            if (!dr.IsDBNull(iCpaindh88)) entity.Cpaindh88 = dr.GetDecimal(iCpaindh88);

            int iCpaindh89 = dr.GetOrdinal(Cpaindh89);
            if (!dr.IsDBNull(iCpaindh89)) entity.Cpaindh89 = dr.GetDecimal(iCpaindh89);

            int iCpaindh90 = dr.GetOrdinal(Cpaindh90);
            if (!dr.IsDBNull(iCpaindh90)) entity.Cpaindh90 = dr.GetDecimal(iCpaindh90);

            int iCpaindh91 = dr.GetOrdinal(Cpaindh91);
            if (!dr.IsDBNull(iCpaindh91)) entity.Cpaindh91 = dr.GetDecimal(iCpaindh91);

            int iCpaindh92 = dr.GetOrdinal(Cpaindh92);
            if (!dr.IsDBNull(iCpaindh92)) entity.Cpaindh92 = dr.GetDecimal(iCpaindh92);

            int iCpaindh93 = dr.GetOrdinal(Cpaindh93);
            if (!dr.IsDBNull(iCpaindh93)) entity.Cpaindh93 = dr.GetDecimal(iCpaindh93);

            int iCpaindh94 = dr.GetOrdinal(Cpaindh94);
            if (!dr.IsDBNull(iCpaindh94)) entity.Cpaindh94 = dr.GetDecimal(iCpaindh94);

            int iCpaindh95 = dr.GetOrdinal(Cpaindh95);
            if (!dr.IsDBNull(iCpaindh95)) entity.Cpaindh95 = dr.GetDecimal(iCpaindh95);

            int iCpaindh96 = dr.GetOrdinal(Cpaindh96);
            if (!dr.IsDBNull(iCpaindh96)) entity.Cpaindh96 = dr.GetDecimal(iCpaindh96);

            int iCpaindusucreacion = dr.GetOrdinal(Cpaindusucreacion);
            if (!dr.IsDBNull(iCpaindusucreacion)) entity.Cpaindusucreacion = dr.GetString(iCpaindusucreacion);

            int iCpaindfeccreacion = dr.GetOrdinal(Cpaindfeccreacion);
            if (!dr.IsDBNull(iCpaindfeccreacion)) entity.Cpaindfeccreacion = dr.GetDateTime(iCpaindfeccreacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Cpaindcodi = "CPAINDCODI";
        public string Cpainmcodi = "CPAINMCODI";
        public string Cparcodi = "CPARCODI";
        public string Cpaindtipinsumo = "CPAINDTIPINSUMO";
        public string Cpaindtipproceso = "CPAINDTIPPROCESO";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Cpainddia = "CPAINDDIA";
        public string Cpaindtotaldia = "CPAINDTOTALDIA";
        public string Cpaindh1 = "CPAINDH1";
        public string Cpaindh2 = "CPAINDH2";
        public string Cpaindh3 = "CPAINDH3";
        public string Cpaindh4 = "CPAINDH4";
        public string Cpaindh5 = "CPAINDH5";
        public string Cpaindh6 = "CPAINDH6";
        public string Cpaindh7 = "CPAINDH7";
        public string Cpaindh8 = "CPAINDH8";
        public string Cpaindh9 = "CPAINDH9";
        public string Cpaindh10 = "CPAINDH10";
        public string Cpaindh11 = "CPAINDH11";
        public string Cpaindh12 = "CPAINDH12";
        public string Cpaindh13 = "CPAINDH13";
        public string Cpaindh14 = "CPAINDH14";
        public string Cpaindh15 = "CPAINDH15";
        public string Cpaindh16 = "CPAINDH16";
        public string Cpaindh17 = "CPAINDH17";
        public string Cpaindh18 = "CPAINDH18";
        public string Cpaindh19 = "CPAINDH19";
        public string Cpaindh20 = "CPAINDH20";
        public string Cpaindh21 = "CPAINDH21";
        public string Cpaindh22 = "CPAINDH22";
        public string Cpaindh23 = "CPAINDH23";
        public string Cpaindh24 = "CPAINDH24";
        public string Cpaindh25 = "CPAINDH25";
        public string Cpaindh26 = "CPAINDH26";
        public string Cpaindh27 = "CPAINDH27";
        public string Cpaindh28 = "CPAINDH28";
        public string Cpaindh29 = "CPAINDH29";
        public string Cpaindh30 = "CPAINDH30";
        public string Cpaindh31 = "CPAINDH31";
        public string Cpaindh32 = "CPAINDH32";
        public string Cpaindh33 = "CPAINDH33";
        public string Cpaindh34 = "CPAINDH34";
        public string Cpaindh35 = "CPAINDH35";
        public string Cpaindh36 = "CPAINDH36";
        public string Cpaindh37 = "CPAINDH37";
        public string Cpaindh38 = "CPAINDH38";
        public string Cpaindh39 = "CPAINDH39";
        public string Cpaindh40 = "CPAINDH40";
        public string Cpaindh41 = "CPAINDH41";
        public string Cpaindh42 = "CPAINDH42";
        public string Cpaindh43 = "CPAINDH43";
        public string Cpaindh44 = "CPAINDH44";
        public string Cpaindh45 = "CPAINDH45";
        public string Cpaindh46 = "CPAINDH46";
        public string Cpaindh47 = "CPAINDH47";
        public string Cpaindh48 = "CPAINDH48";
        public string Cpaindh49 = "CPAINDH49";
        public string Cpaindh50 = "CPAINDH50";
        public string Cpaindh51 = "CPAINDH51";
        public string Cpaindh52 = "CPAINDH52";
        public string Cpaindh53 = "CPAINDH53";
        public string Cpaindh54 = "CPAINDH54";
        public string Cpaindh55 = "CPAINDH55";
        public string Cpaindh56 = "CPAINDH56";
        public string Cpaindh57 = "CPAINDH57";
        public string Cpaindh58 = "CPAINDH58";
        public string Cpaindh59 = "CPAINDH59";
        public string Cpaindh60 = "CPAINDH60";
        public string Cpaindh61 = "CPAINDH61";
        public string Cpaindh62 = "CPAINDH62";
        public string Cpaindh63 = "CPAINDH63";
        public string Cpaindh64 = "CPAINDH64";
        public string Cpaindh65 = "CPAINDH65";
        public string Cpaindh66 = "CPAINDH66";
        public string Cpaindh67 = "CPAINDH67";
        public string Cpaindh68 = "CPAINDH68";
        public string Cpaindh69 = "CPAINDH69";
        public string Cpaindh70 = "CPAINDH70";
        public string Cpaindh71 = "CPAINDH71";
        public string Cpaindh72 = "CPAINDH72";
        public string Cpaindh73 = "CPAINDH73";
        public string Cpaindh74 = "CPAINDH74";
        public string Cpaindh75 = "CPAINDH75";
        public string Cpaindh76 = "CPAINDH76";
        public string Cpaindh77 = "CPAINDH77";
        public string Cpaindh78 = "CPAINDH78";
        public string Cpaindh79 = "CPAINDH79";
        public string Cpaindh80 = "CPAINDH80";
        public string Cpaindh81 = "CPAINDH81";
        public string Cpaindh82 = "CPAINDH82";
        public string Cpaindh83 = "CPAINDH83";
        public string Cpaindh84 = "CPAINDH84";
        public string Cpaindh85 = "CPAINDH85";
        public string Cpaindh86 = "CPAINDH86";
        public string Cpaindh87 = "CPAINDH87";
        public string Cpaindh88 = "CPAINDH88";
        public string Cpaindh89 = "CPAINDH89";
        public string Cpaindh90 = "CPAINDH90";
        public string Cpaindh91 = "CPAINDH91";
        public string Cpaindh92 = "CPAINDH92";
        public string Cpaindh93 = "CPAINDH93";
        public string Cpaindh94 = "CPAINDH94";
        public string Cpaindh95 = "CPAINDH95";
        public string Cpaindh96 = "CPAINDH96";
        public string Cpaindusucreacion = "CPAINDUSUCREACION";
        public string Cpaindfeccreacion = "CPAINDFECCREACION";

        public string Equinomb = "EQUINOMB";
        public string Barrbarratransferencia = "BARRBARRATRANSFERENCIA";
        //Additional
        public string Cpainmmes = "CPAINMMES";
        #endregion

        //Metodos complementarios:
        public string SqlDeleteByCentral
        {
            get { return base.GetSqlXml("DeleteByCentral"); }
        }
        public string SqlDeleteByRevision
        {
            get { return base.GetSqlXml("DeleteByRevision"); }
        }

        public string SqlInsertarInsumoDiaByTMP
        {
            get { return base.GetSqlXml("InsertarInsumoDiaByTMP"); }
        }

        public string SqlInsertarInsumoDiaByCMg
        {
            get { return base.GetSqlXml("InsertarInsumoDiaByCMg"); }
        }

        public string SqlInsertarInsumoDiaByCMgPMPO
        {
            get { return base.GetSqlXml("InsertarInsumoDiaByCMgPMPO"); }
        }
        public string SqlListByTipoInsumoByPeriodo
        {
            get { return base.GetSqlXml("ListByTipoInsumoByPeriodo"); }
        }
        public string SqlInsertarInsumoDiaBySddp
        {
            get { return base.GetSqlXml("InsertarInsumoDiaBySddp"); }
        }

        public string SqlGetByRevisionByTipo
        {
            get { return base.GetSqlXml("GetByRevisionByTipo"); }
        }

        public string SqlGetNumRegistrosByFecha
        {
            get { return base.GetSqlXml("GetNumRegistrosByFecha"); }
        }

        public string SqlUpdateMesEquipo
        {
            get { return base.GetSqlXml("UpdateMesEquipo"); }
        }

        public string SqlGetNumRegistrosCMgByFecha
        {
            get { return base.GetSqlXml("GetNumRegistrosCMgByFecha"); }
        }
    }
}

