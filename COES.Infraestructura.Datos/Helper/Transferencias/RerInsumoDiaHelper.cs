using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_INSUMO_DIA
    /// </summary>
    public class RerInsumoDiaHelper : HelperBase
    {
        public RerInsumoDiaHelper() : base(Consultas.RerInsumoDiaSql)
        {
        }

        public RerInsumoDiaDTO Create(IDataReader dr)
        {
            RerInsumoDiaDTO entity = new RerInsumoDiaDTO();

            int iRerinddiacodi = dr.GetOrdinal(this.Rerinddiacodi);
            if (!dr.IsDBNull(iRerinddiacodi)) entity.Rerinddiacodi = Convert.ToInt32(dr.GetValue(iRerinddiacodi));

            int iRerinmmescodi = dr.GetOrdinal(this.Rerinmmescodi);
            if (!dr.IsDBNull(iRerinmmescodi)) entity.Rerinmmescodi = Convert.ToInt32(dr.GetValue(iRerinmmescodi));

            int iRerpprcodi = dr.GetOrdinal(this.Rerpprcodi);
            if (!dr.IsDBNull(iRerpprcodi)) entity.Rerpprcodi = Convert.ToInt32(dr.GetValue(iRerpprcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
            
            int iRerinddiafecdia = dr.GetOrdinal(this.Rerinddiafecdia);
            if (!dr.IsDBNull(iRerinddiafecdia)) entity.Rerinddiafecdia = dr.GetDateTime(iRerinddiafecdia);

            int iRerindtipresultado = dr.GetOrdinal(this.Rerindtipresultado);
            if (!dr.IsDBNull(iRerindtipresultado)) entity.Rerindtipresultado = dr.GetString(iRerindtipresultado);

            int iRerindtipinformacion = dr.GetOrdinal(this.Rerindtipinformacion);
            if (!dr.IsDBNull(iRerindtipinformacion)) entity.Rerindtipinformacion = dr.GetString(iRerindtipinformacion);

            int iRerinddiah1 = dr.GetOrdinal(this.Rerinddiah1);
            if (!dr.IsDBNull(iRerinddiah1)) entity.Rerinddiah1 = dr.GetDecimal(iRerinddiah1);

            int iRerinddiah2 = dr.GetOrdinal(this.Rerinddiah2);
            if (!dr.IsDBNull(iRerinddiah2)) entity.Rerinddiah2 = dr.GetDecimal(iRerinddiah2);

            int iRerinddiah3 = dr.GetOrdinal(this.Rerinddiah3);
            if (!dr.IsDBNull(iRerinddiah3)) entity.Rerinddiah3 = dr.GetDecimal(iRerinddiah3);

            int iRerinddiah4 = dr.GetOrdinal(this.Rerinddiah4);
            if (!dr.IsDBNull(iRerinddiah4)) entity.Rerinddiah4 = dr.GetDecimal(iRerinddiah4);

            int iRerinddiah5 = dr.GetOrdinal(this.Rerinddiah5);
            if (!dr.IsDBNull(iRerinddiah5)) entity.Rerinddiah5 = dr.GetDecimal(iRerinddiah5);

            int iRerinddiah6 = dr.GetOrdinal(this.Rerinddiah6);
            if (!dr.IsDBNull(iRerinddiah6)) entity.Rerinddiah6 = dr.GetDecimal(iRerinddiah6);

            int iRerinddiah7 = dr.GetOrdinal(this.Rerinddiah7);
            if (!dr.IsDBNull(iRerinddiah7)) entity.Rerinddiah7 = dr.GetDecimal(iRerinddiah7);

            int iRerinddiah8 = dr.GetOrdinal(this.Rerinddiah8);
            if (!dr.IsDBNull(iRerinddiah8)) entity.Rerinddiah8 = dr.GetDecimal(iRerinddiah8);

            int iRerinddiah9 = dr.GetOrdinal(this.Rerinddiah9);
            if (!dr.IsDBNull(iRerinddiah9)) entity.Rerinddiah9 = dr.GetDecimal(iRerinddiah9);

            int iRerinddiah10 = dr.GetOrdinal(this.Rerinddiah10);
            if (!dr.IsDBNull(iRerinddiah10)) entity.Rerinddiah10 = dr.GetDecimal(iRerinddiah10);

            int iRerinddiah11 = dr.GetOrdinal(this.Rerinddiah11);
            if (!dr.IsDBNull(iRerinddiah11)) entity.Rerinddiah11 = dr.GetDecimal(iRerinddiah11);

            int iRerinddiah12 = dr.GetOrdinal(this.Rerinddiah12);
            if (!dr.IsDBNull(iRerinddiah12)) entity.Rerinddiah12 = dr.GetDecimal(iRerinddiah12);

            int iRerinddiah13 = dr.GetOrdinal(this.Rerinddiah13);
            if (!dr.IsDBNull(iRerinddiah13)) entity.Rerinddiah13 = dr.GetDecimal(iRerinddiah13);

            int iRerinddiah14 = dr.GetOrdinal(this.Rerinddiah14);
            if (!dr.IsDBNull(iRerinddiah14)) entity.Rerinddiah14 = dr.GetDecimal(iRerinddiah14);

            int iRerinddiah15 = dr.GetOrdinal(this.Rerinddiah15);
            if (!dr.IsDBNull(iRerinddiah15)) entity.Rerinddiah15 = dr.GetDecimal(iRerinddiah15);

            int iRerinddiah16 = dr.GetOrdinal(this.Rerinddiah16);
            if (!dr.IsDBNull(iRerinddiah16)) entity.Rerinddiah16 = dr.GetDecimal(iRerinddiah16);

            int iRerinddiah17 = dr.GetOrdinal(this.Rerinddiah17);
            if (!dr.IsDBNull(iRerinddiah17)) entity.Rerinddiah17 = dr.GetDecimal(iRerinddiah17);

            int iRerinddiah18 = dr.GetOrdinal(this.Rerinddiah18);
            if (!dr.IsDBNull(iRerinddiah18)) entity.Rerinddiah18 = dr.GetDecimal(iRerinddiah18);

            int iRerinddiah19 = dr.GetOrdinal(this.Rerinddiah19);
            if (!dr.IsDBNull(iRerinddiah19)) entity.Rerinddiah19 = dr.GetDecimal(iRerinddiah19);

            int iRerinddiah20 = dr.GetOrdinal(this.Rerinddiah20);
            if (!dr.IsDBNull(iRerinddiah20)) entity.Rerinddiah20 = dr.GetDecimal(iRerinddiah20);

            int iRerinddiah21 = dr.GetOrdinal(this.Rerinddiah21);
            if (!dr.IsDBNull(iRerinddiah21)) entity.Rerinddiah21 = dr.GetDecimal(iRerinddiah21);

            int iRerinddiah22 = dr.GetOrdinal(this.Rerinddiah22);
            if (!dr.IsDBNull(iRerinddiah22)) entity.Rerinddiah22 = dr.GetDecimal(iRerinddiah22);

            int iRerinddiah23 = dr.GetOrdinal(this.Rerinddiah23);
            if (!dr.IsDBNull(iRerinddiah23)) entity.Rerinddiah23 = dr.GetDecimal(iRerinddiah23);

            int iRerinddiah24 = dr.GetOrdinal(this.Rerinddiah24);
            if (!dr.IsDBNull(iRerinddiah24)) entity.Rerinddiah24 = dr.GetDecimal(iRerinddiah24);

            int iRerinddiah25 = dr.GetOrdinal(this.Rerinddiah25);
            if (!dr.IsDBNull(iRerinddiah25)) entity.Rerinddiah25 = dr.GetDecimal(iRerinddiah25);

            int iRerinddiah26 = dr.GetOrdinal(this.Rerinddiah26);
            if (!dr.IsDBNull(iRerinddiah26)) entity.Rerinddiah26 = dr.GetDecimal(iRerinddiah26);

            int iRerinddiah27 = dr.GetOrdinal(this.Rerinddiah27);
            if (!dr.IsDBNull(iRerinddiah27)) entity.Rerinddiah27 = dr.GetDecimal(iRerinddiah27);

            int iRerinddiah28 = dr.GetOrdinal(this.Rerinddiah28);
            if (!dr.IsDBNull(iRerinddiah28)) entity.Rerinddiah28 = dr.GetDecimal(iRerinddiah28);

            int iRerinddiah29 = dr.GetOrdinal(this.Rerinddiah29);
            if (!dr.IsDBNull(iRerinddiah29)) entity.Rerinddiah29 = dr.GetDecimal(iRerinddiah29);

            int iRerinddiah30 = dr.GetOrdinal(this.Rerinddiah30);
            if (!dr.IsDBNull(iRerinddiah30)) entity.Rerinddiah30 = dr.GetDecimal(iRerinddiah30);

            int iRerinddiah31 = dr.GetOrdinal(this.Rerinddiah31);
            if (!dr.IsDBNull(iRerinddiah31)) entity.Rerinddiah31 = dr.GetDecimal(iRerinddiah31);

            int iRerinddiah32 = dr.GetOrdinal(this.Rerinddiah32);
            if (!dr.IsDBNull(iRerinddiah32)) entity.Rerinddiah32 = dr.GetDecimal(iRerinddiah32);

            int iRerinddiah33 = dr.GetOrdinal(this.Rerinddiah33);
            if (!dr.IsDBNull(iRerinddiah33)) entity.Rerinddiah33 = dr.GetDecimal(iRerinddiah33);

            int iRerinddiah34 = dr.GetOrdinal(this.Rerinddiah34);
            if (!dr.IsDBNull(iRerinddiah34)) entity.Rerinddiah34 = dr.GetDecimal(iRerinddiah34);

            int iRerinddiah35 = dr.GetOrdinal(this.Rerinddiah35);
            if (!dr.IsDBNull(iRerinddiah35)) entity.Rerinddiah35 = dr.GetDecimal(iRerinddiah35);

            int iRerinddiah36 = dr.GetOrdinal(this.Rerinddiah36);
            if (!dr.IsDBNull(iRerinddiah36)) entity.Rerinddiah36 = dr.GetDecimal(iRerinddiah36);

            int iRerinddiah37 = dr.GetOrdinal(this.Rerinddiah37);
            if (!dr.IsDBNull(iRerinddiah37)) entity.Rerinddiah37 = dr.GetDecimal(iRerinddiah37);

            int iRerinddiah38 = dr.GetOrdinal(this.Rerinddiah38);
            if (!dr.IsDBNull(iRerinddiah38)) entity.Rerinddiah38 = dr.GetDecimal(iRerinddiah38);

            int iRerinddiah39 = dr.GetOrdinal(this.Rerinddiah39);
            if (!dr.IsDBNull(iRerinddiah39)) entity.Rerinddiah39 = dr.GetDecimal(iRerinddiah39);

            int iRerinddiah40 = dr.GetOrdinal(this.Rerinddiah40);
            if (!dr.IsDBNull(iRerinddiah40)) entity.Rerinddiah40 = dr.GetDecimal(iRerinddiah40);

            int iRerinddiah41 = dr.GetOrdinal(this.Rerinddiah41);
            if (!dr.IsDBNull(iRerinddiah41)) entity.Rerinddiah41 = dr.GetDecimal(iRerinddiah41);

            int iRerinddiah42 = dr.GetOrdinal(this.Rerinddiah42);
            if (!dr.IsDBNull(iRerinddiah42)) entity.Rerinddiah42 = dr.GetDecimal(iRerinddiah42);

            int iRerinddiah43 = dr.GetOrdinal(this.Rerinddiah43);
            if (!dr.IsDBNull(iRerinddiah43)) entity.Rerinddiah43 = dr.GetDecimal(iRerinddiah43);

            int iRerinddiah44 = dr.GetOrdinal(this.Rerinddiah44);
            if (!dr.IsDBNull(iRerinddiah44)) entity.Rerinddiah44 = dr.GetDecimal(iRerinddiah44);

            int iRerinddiah45 = dr.GetOrdinal(this.Rerinddiah45);
            if (!dr.IsDBNull(iRerinddiah45)) entity.Rerinddiah45 = dr.GetDecimal(iRerinddiah45);

            int iRerinddiah46 = dr.GetOrdinal(this.Rerinddiah46);
            if (!dr.IsDBNull(iRerinddiah46)) entity.Rerinddiah46 = dr.GetDecimal(iRerinddiah46);

            int iRerinddiah47 = dr.GetOrdinal(this.Rerinddiah47);
            if (!dr.IsDBNull(iRerinddiah47)) entity.Rerinddiah47 = dr.GetDecimal(iRerinddiah47);

            int iRerinddiah48 = dr.GetOrdinal(this.Rerinddiah48);
            if (!dr.IsDBNull(iRerinddiah48)) entity.Rerinddiah48 = dr.GetDecimal(iRerinddiah48);

            int iRerinddiah49 = dr.GetOrdinal(this.Rerinddiah49);
            if (!dr.IsDBNull(iRerinddiah49)) entity.Rerinddiah49 = dr.GetDecimal(iRerinddiah49);

            int iRerinddiah50 = dr.GetOrdinal(this.Rerinddiah50);
            if (!dr.IsDBNull(iRerinddiah50)) entity.Rerinddiah50 = dr.GetDecimal(iRerinddiah50);

            int iRerinddiah51 = dr.GetOrdinal(this.Rerinddiah51);
            if (!dr.IsDBNull(iRerinddiah51)) entity.Rerinddiah51 = dr.GetDecimal(iRerinddiah51);

            int iRerinddiah52 = dr.GetOrdinal(this.Rerinddiah52);
            if (!dr.IsDBNull(iRerinddiah52)) entity.Rerinddiah52 = dr.GetDecimal(iRerinddiah52);

            int iRerinddiah53 = dr.GetOrdinal(this.Rerinddiah53);
            if (!dr.IsDBNull(iRerinddiah53)) entity.Rerinddiah53 = dr.GetDecimal(iRerinddiah53);

            int iRerinddiah54 = dr.GetOrdinal(this.Rerinddiah54);
            if (!dr.IsDBNull(iRerinddiah54)) entity.Rerinddiah54 = dr.GetDecimal(iRerinddiah54);

            int iRerinddiah55 = dr.GetOrdinal(this.Rerinddiah55);
            if (!dr.IsDBNull(iRerinddiah55)) entity.Rerinddiah55 = dr.GetDecimal(iRerinddiah55);

            int iRerinddiah56 = dr.GetOrdinal(this.Rerinddiah56);
            if (!dr.IsDBNull(iRerinddiah56)) entity.Rerinddiah56 = dr.GetDecimal(iRerinddiah56);

            int iRerinddiah57 = dr.GetOrdinal(this.Rerinddiah57);
            if (!dr.IsDBNull(iRerinddiah57)) entity.Rerinddiah57 = dr.GetDecimal(iRerinddiah57);

            int iRerinddiah58 = dr.GetOrdinal(this.Rerinddiah58);
            if (!dr.IsDBNull(iRerinddiah58)) entity.Rerinddiah58 = dr.GetDecimal(iRerinddiah58);

            int iRerinddiah59 = dr.GetOrdinal(this.Rerinddiah59);
            if (!dr.IsDBNull(iRerinddiah59)) entity.Rerinddiah59 = dr.GetDecimal(iRerinddiah59);

            int iRerinddiah60 = dr.GetOrdinal(this.Rerinddiah60);
            if (!dr.IsDBNull(iRerinddiah60)) entity.Rerinddiah60 = dr.GetDecimal(iRerinddiah60);

            int iRerinddiah61 = dr.GetOrdinal(this.Rerinddiah61);
            if (!dr.IsDBNull(iRerinddiah61)) entity.Rerinddiah61 = dr.GetDecimal(iRerinddiah61);

            int iRerinddiah62 = dr.GetOrdinal(this.Rerinddiah62);
            if (!dr.IsDBNull(iRerinddiah62)) entity.Rerinddiah62 = dr.GetDecimal(iRerinddiah62);

            int iRerinddiah63 = dr.GetOrdinal(this.Rerinddiah63);
            if (!dr.IsDBNull(iRerinddiah63)) entity.Rerinddiah63 = dr.GetDecimal(iRerinddiah63);

            int iRerinddiah64 = dr.GetOrdinal(this.Rerinddiah64);
            if (!dr.IsDBNull(iRerinddiah64)) entity.Rerinddiah64 = dr.GetDecimal(iRerinddiah64);

            int iRerinddiah65 = dr.GetOrdinal(this.Rerinddiah65);
            if (!dr.IsDBNull(iRerinddiah65)) entity.Rerinddiah65 = dr.GetDecimal(iRerinddiah65);

            int iRerinddiah66 = dr.GetOrdinal(this.Rerinddiah66);
            if (!dr.IsDBNull(iRerinddiah66)) entity.Rerinddiah66 = dr.GetDecimal(iRerinddiah66);

            int iRerinddiah67 = dr.GetOrdinal(this.Rerinddiah67);
            if (!dr.IsDBNull(iRerinddiah67)) entity.Rerinddiah67 = dr.GetDecimal(iRerinddiah67);

            int iRerinddiah68 = dr.GetOrdinal(this.Rerinddiah68);
            if (!dr.IsDBNull(iRerinddiah68)) entity.Rerinddiah68 = dr.GetDecimal(iRerinddiah68);

            int iRerinddiah69 = dr.GetOrdinal(this.Rerinddiah69);
            if (!dr.IsDBNull(iRerinddiah69)) entity.Rerinddiah69 = dr.GetDecimal(iRerinddiah69);

            int iRerinddiah70 = dr.GetOrdinal(this.Rerinddiah70);
            if (!dr.IsDBNull(iRerinddiah70)) entity.Rerinddiah70 = dr.GetDecimal(iRerinddiah70);

            int iRerinddiah71 = dr.GetOrdinal(this.Rerinddiah71);
            if (!dr.IsDBNull(iRerinddiah71)) entity.Rerinddiah71 = dr.GetDecimal(iRerinddiah71);

            int iRerinddiah72 = dr.GetOrdinal(this.Rerinddiah72);
            if (!dr.IsDBNull(iRerinddiah72)) entity.Rerinddiah72 = dr.GetDecimal(iRerinddiah72);

            int iRerinddiah73 = dr.GetOrdinal(this.Rerinddiah73);
            if (!dr.IsDBNull(iRerinddiah73)) entity.Rerinddiah73 = dr.GetDecimal(iRerinddiah73);

            int iRerinddiah74 = dr.GetOrdinal(this.Rerinddiah74);
            if (!dr.IsDBNull(iRerinddiah74)) entity.Rerinddiah74 = dr.GetDecimal(iRerinddiah74);

            int iRerinddiah75 = dr.GetOrdinal(this.Rerinddiah75);
            if (!dr.IsDBNull(iRerinddiah75)) entity.Rerinddiah75 = dr.GetDecimal(iRerinddiah75);

            int iRerinddiah76 = dr.GetOrdinal(this.Rerinddiah76);
            if (!dr.IsDBNull(iRerinddiah76)) entity.Rerinddiah76 = dr.GetDecimal(iRerinddiah76);

            int iRerinddiah77 = dr.GetOrdinal(this.Rerinddiah77);
            if (!dr.IsDBNull(iRerinddiah77)) entity.Rerinddiah77 = dr.GetDecimal(iRerinddiah77);

            int iRerinddiah78 = dr.GetOrdinal(this.Rerinddiah78);
            if (!dr.IsDBNull(iRerinddiah78)) entity.Rerinddiah78 = dr.GetDecimal(iRerinddiah78);

            int iRerinddiah79 = dr.GetOrdinal(this.Rerinddiah79);
            if (!dr.IsDBNull(iRerinddiah79)) entity.Rerinddiah79 = dr.GetDecimal(iRerinddiah79);

            int iRerinddiah80 = dr.GetOrdinal(this.Rerinddiah80);
            if (!dr.IsDBNull(iRerinddiah80)) entity.Rerinddiah80 = dr.GetDecimal(iRerinddiah80);

            int iRerinddiah81 = dr.GetOrdinal(this.Rerinddiah81);
            if (!dr.IsDBNull(iRerinddiah81)) entity.Rerinddiah81 = dr.GetDecimal(iRerinddiah81);

            int iRerinddiah82 = dr.GetOrdinal(this.Rerinddiah82);
            if (!dr.IsDBNull(iRerinddiah82)) entity.Rerinddiah82 = dr.GetDecimal(iRerinddiah82);

            int iRerinddiah83 = dr.GetOrdinal(this.Rerinddiah83);
            if (!dr.IsDBNull(iRerinddiah83)) entity.Rerinddiah83 = dr.GetDecimal(iRerinddiah83);

            int iRerinddiah84 = dr.GetOrdinal(this.Rerinddiah84);
            if (!dr.IsDBNull(iRerinddiah84)) entity.Rerinddiah84 = dr.GetDecimal(iRerinddiah84);

            int iRerinddiah85 = dr.GetOrdinal(this.Rerinddiah85);
            if (!dr.IsDBNull(iRerinddiah85)) entity.Rerinddiah85 = dr.GetDecimal(iRerinddiah85);

            int iRerinddiah86 = dr.GetOrdinal(this.Rerinddiah86);
            if (!dr.IsDBNull(iRerinddiah86)) entity.Rerinddiah86 = dr.GetDecimal(iRerinddiah86);

            int iRerinddiah87 = dr.GetOrdinal(this.Rerinddiah87);
            if (!dr.IsDBNull(iRerinddiah87)) entity.Rerinddiah87 = dr.GetDecimal(iRerinddiah87);

            int iRerinddiah88 = dr.GetOrdinal(this.Rerinddiah88);
            if (!dr.IsDBNull(iRerinddiah88)) entity.Rerinddiah88 = dr.GetDecimal(iRerinddiah88);

            int iRerinddiah89 = dr.GetOrdinal(this.Rerinddiah89);
            if (!dr.IsDBNull(iRerinddiah89)) entity.Rerinddiah89 = dr.GetDecimal(iRerinddiah89);

            int iRerinddiah90 = dr.GetOrdinal(this.Rerinddiah90);
            if (!dr.IsDBNull(iRerinddiah90)) entity.Rerinddiah90 = dr.GetDecimal(iRerinddiah90);

            int iRerinddiah91 = dr.GetOrdinal(this.Rerinddiah91);
            if (!dr.IsDBNull(iRerinddiah91)) entity.Rerinddiah91 = dr.GetDecimal(iRerinddiah91);

            int iRerinddiah92 = dr.GetOrdinal(this.Rerinddiah92);
            if (!dr.IsDBNull(iRerinddiah92)) entity.Rerinddiah92 = dr.GetDecimal(iRerinddiah92);

            int iRerinddiah93 = dr.GetOrdinal(this.Rerinddiah93);
            if (!dr.IsDBNull(iRerinddiah93)) entity.Rerinddiah93 = dr.GetDecimal(iRerinddiah93);

            int iRerinddiah94 = dr.GetOrdinal(this.Rerinddiah94);
            if (!dr.IsDBNull(iRerinddiah94)) entity.Rerinddiah94 = dr.GetDecimal(iRerinddiah94);

            int iRerinddiah95 = dr.GetOrdinal(this.Rerinddiah95);
            if (!dr.IsDBNull(iRerinddiah95)) entity.Rerinddiah95 = dr.GetDecimal(iRerinddiah95);

            int iRerinddiah96 = dr.GetOrdinal(this.Rerinddiah96);
            if (!dr.IsDBNull(iRerinddiah96)) entity.Rerinddiah96 = dr.GetDecimal(iRerinddiah96);

            int iRerinddiatotal = dr.GetOrdinal(this.Rerinddiatotal);
            if (!dr.IsDBNull(iRerinddiatotal)) entity.Rerinddiatotal = dr.GetDecimal(iRerinddiatotal);

            int iRerinddiausucreacion = dr.GetOrdinal(this.Rerinddiausucreacion);
            if (!dr.IsDBNull(iRerinddiausucreacion)) entity.Rerinddiausucreacion = dr.GetString(iRerinddiausucreacion);

            int iRerinddiafeccreacion = dr.GetOrdinal(this.Rerinddiafeccreacion);
            if (!dr.IsDBNull(iRerinddiafeccreacion)) entity.Rerinddiafeccreacion = dr.GetDateTime(iRerinddiafeccreacion);

            return entity;
        }

        public RerInsumoDiaDTO CreateByTipoResultadoByPeriodo(IDataReader dr)
        {
            RerInsumoDiaDTO entity = Create(dr);

            int iEquinomb = dr.GetOrdinal(this.Equinomb);
            if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

            return entity;
        }

        #region Mapeo de Campos
        public string Rerinddiacodi = "RERINDDIACODI";
        public string Rerinmmescodi = "RERINMMESCODI";
        public string Rerpprcodi = "RERPPRCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Rerinddiafecdia = "RERINDDIAFECDIA";
        public string Rerindtipresultado = "RERINDTIPRESULTADO";
        public string Rerindtipinformacion = "RERINDTIPINFORMACION";
        public string Rerinddiah1 = "RERINDDIAH1";
        public string Rerinddiah2 = "RERINDDIAH2";
        public string Rerinddiah3 = "RERINDDIAH3";
        public string Rerinddiah4 = "RERINDDIAH4";
        public string Rerinddiah5 = "RERINDDIAH5";
        public string Rerinddiah6 = "RERINDDIAH6";
        public string Rerinddiah7 = "RERINDDIAH7";
        public string Rerinddiah8 = "RERINDDIAH8";
        public string Rerinddiah9 = "RERINDDIAH9";
        public string Rerinddiah10 = "RERINDDIAH10";
        public string Rerinddiah11 = "RERINDDIAH11";
        public string Rerinddiah12 = "RERINDDIAH12";
        public string Rerinddiah13 = "RERINDDIAH13";
        public string Rerinddiah14 = "RERINDDIAH14";
        public string Rerinddiah15 = "RERINDDIAH15";
        public string Rerinddiah16 = "RERINDDIAH16";
        public string Rerinddiah17 = "RERINDDIAH17";
        public string Rerinddiah18 = "RERINDDIAH18";
        public string Rerinddiah19 = "RERINDDIAH19";
        public string Rerinddiah20 = "RERINDDIAH20";
        public string Rerinddiah21 = "RERINDDIAH21";
        public string Rerinddiah22 = "RERINDDIAH22";
        public string Rerinddiah23 = "RERINDDIAH23";
        public string Rerinddiah24 = "RERINDDIAH24";
        public string Rerinddiah25 = "RERINDDIAH25";
        public string Rerinddiah26 = "RERINDDIAH26";
        public string Rerinddiah27 = "RERINDDIAH27";
        public string Rerinddiah28 = "RERINDDIAH28";
        public string Rerinddiah29 = "RERINDDIAH29";
        public string Rerinddiah30 = "RERINDDIAH30";
        public string Rerinddiah31 = "RERINDDIAH31";
        public string Rerinddiah32 = "RERINDDIAH32";
        public string Rerinddiah33 = "RERINDDIAH33";
        public string Rerinddiah34 = "RERINDDIAH34";
        public string Rerinddiah35 = "RERINDDIAH35";
        public string Rerinddiah36 = "RERINDDIAH36";
        public string Rerinddiah37 = "RERINDDIAH37";
        public string Rerinddiah38 = "RERINDDIAH38";
        public string Rerinddiah39 = "RERINDDIAH39";
        public string Rerinddiah40 = "RERINDDIAH40";
        public string Rerinddiah41 = "RERINDDIAH41";
        public string Rerinddiah42 = "RERINDDIAH42";
        public string Rerinddiah43 = "RERINDDIAH43";
        public string Rerinddiah44 = "RERINDDIAH44";
        public string Rerinddiah45 = "RERINDDIAH45";
        public string Rerinddiah46 = "RERINDDIAH46";
        public string Rerinddiah47 = "RERINDDIAH47";
        public string Rerinddiah48 = "RERINDDIAH48";
        public string Rerinddiah49 = "RERINDDIAH49";
        public string Rerinddiah50 = "RERINDDIAH50";
        public string Rerinddiah51 = "RERINDDIAH51";
        public string Rerinddiah52 = "RERINDDIAH52";
        public string Rerinddiah53 = "RERINDDIAH53";
        public string Rerinddiah54 = "RERINDDIAH54";
        public string Rerinddiah55 = "RERINDDIAH55";
        public string Rerinddiah56 = "RERINDDIAH56";
        public string Rerinddiah57 = "RERINDDIAH57";
        public string Rerinddiah58 = "RERINDDIAH58";
        public string Rerinddiah59 = "RERINDDIAH59";
        public string Rerinddiah60 = "RERINDDIAH60";
        public string Rerinddiah61 = "RERINDDIAH61";
        public string Rerinddiah62 = "RERINDDIAH62";
        public string Rerinddiah63 = "RERINDDIAH63";
        public string Rerinddiah64 = "RERINDDIAH64";
        public string Rerinddiah65 = "RERINDDIAH65";
        public string Rerinddiah66 = "RERINDDIAH66";
        public string Rerinddiah67 = "RERINDDIAH67";
        public string Rerinddiah68 = "RERINDDIAH68";
        public string Rerinddiah69 = "RERINDDIAH69";
        public string Rerinddiah70 = "RERINDDIAH70";
        public string Rerinddiah71 = "RERINDDIAH71";
        public string Rerinddiah72 = "RERINDDIAH72";
        public string Rerinddiah73 = "RERINDDIAH73";
        public string Rerinddiah74 = "RERINDDIAH74";
        public string Rerinddiah75 = "RERINDDIAH75";
        public string Rerinddiah76 = "RERINDDIAH76";
        public string Rerinddiah77 = "RERINDDIAH77";
        public string Rerinddiah78 = "RERINDDIAH78";
        public string Rerinddiah79 = "RERINDDIAH79";
        public string Rerinddiah80 = "RERINDDIAH80";
        public string Rerinddiah81 = "RERINDDIAH81";
        public string Rerinddiah82 = "RERINDDIAH82";
        public string Rerinddiah83 = "RERINDDIAH83";
        public string Rerinddiah84 = "RERINDDIAH84";
        public string Rerinddiah85 = "RERINDDIAH85";
        public string Rerinddiah86 = "RERINDDIAH86";
        public string Rerinddiah87 = "RERINDDIAH87";
        public string Rerinddiah88 = "RERINDDIAH88";
        public string Rerinddiah89 = "RERINDDIAH89";
        public string Rerinddiah90 = "RERINDDIAH90";
        public string Rerinddiah91 = "RERINDDIAH91";
        public string Rerinddiah92 = "RERINDDIAH92";
        public string Rerinddiah93 = "RERINDDIAH93";
        public string Rerinddiah94 = "RERINDDIAH94";
        public string Rerinddiah95 = "RERINDDIAH95";
        public string Rerinddiah96 = "RERINDDIAH96";
        public string Rerinddiatotal = "RERINDDIATOTAL";
        public string Rerinddiausucreacion = "RERINDDIAUSUCREACION";
        public string Rerinddiafeccreacion = "RERINDDIAFECCREACION";

        //Atributos de consulta
        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "EQUINOMB";
        public string Rerpprmes = "RERPPRMES";
        public string Rerinmtipresultado = "RERINMTIPRESULTADO";

        // Atributos de tabla temporal
        public string Ptomedicodi = "PTOMEDICODI";
        public string Ptomedidesc = "PTOMEDIDESC";
        #endregion

        public string SqlGetByCriteria
        {
            get { return base.GetSqlXml("GetByCriteria"); }
        }

        public string SqlDeleteByParametroPrimaAndTipo
        {
            get { return base.GetSqlXml("DeleteByParametroPrimaAndTipo"); }
        }

        public string SqlGetByMesByEmpresaByCentral
        {
            get { return base.GetSqlXml("GetByMesByEmpresaByCentral"); }
        }

        public string SqlGetByTipoResultadoByPeriodo
        {
            get { return base.GetSqlXml("GetByTipoResultadoByPeriodo"); }
        }



        public string SqlTruncateTablaTemporal
        {
            get { return base.GetSqlXml("TruncateTablaTemporal"); }
        }
        public string SqlSaveInsumoDiaTemporal
        {
            get { return base.GetSqlXml("SaveInsumoDiaTemporal"); }
        }

    }
}

