using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_INSUMO_VTEA
    /// </summary>
    public class RerInsumoVteaHelper : HelperBase
    {
        public RerInsumoVteaHelper() : base(Consultas.RerInsumoVteaSql)
        {
        }

        public RerInsumoVteaDTO Create(IDataReader dr)
        {
            RerInsumoVteaDTO entity = new RerInsumoVteaDTO();

            int iRerIncodi = dr.GetOrdinal(this.Rerinecodi);
            if (!dr.IsDBNull(iRerIncodi)) entity.Rerinecodi = Convert.ToInt32(dr.GetValue(iRerIncodi));

            int iRerinscodi = dr.GetOrdinal(this.Rerinscodi);
            if (!dr.IsDBNull(iRerinscodi)) entity.Rerinscodi = Convert.ToInt32(dr.GetValue(iRerinscodi));

            int iRerPrcodi = dr.GetOrdinal(this.Rerpprcodi);
            if (!dr.IsDBNull(iRerPrcodi)) entity.Rerpprcodi = Convert.ToInt32(dr.GetValue(iRerPrcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iRerInfeccdia = dr.GetOrdinal(this.Rerinefecdia);
            if (!dr.IsDBNull(iRerInfeccdia)) entity.Rerinefecdia = dr.GetDateTime(iRerInfeccdia);

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecacodi = dr.GetOrdinal(this.Recacodi);
            if (!dr.IsDBNull(iRecacodi)) entity.Recacodi = Convert.ToInt32(dr.GetValue(iRecacodi));


            int iRerinediah1 = dr.GetOrdinal(this.Rerinediah1);
            if (!dr.IsDBNull(iRerinediah1)) entity.Rerinediah1 = dr.GetDecimal(iRerinediah1);

            int iRerinediah2 = dr.GetOrdinal(this.Rerinediah2);
            if (!dr.IsDBNull(iRerinediah2)) entity.Rerinediah2 = dr.GetDecimal(iRerinediah2);

            int iRerinediah3 = dr.GetOrdinal(this.Rerinediah3);
            if (!dr.IsDBNull(iRerinediah3)) entity.Rerinediah3 = dr.GetDecimal(iRerinediah3);

            int iRerinediah4 = dr.GetOrdinal(this.Rerinediah4);
            if (!dr.IsDBNull(iRerinediah4)) entity.Rerinediah4 = dr.GetDecimal(iRerinediah4);

            int iRerinediah5 = dr.GetOrdinal(this.Rerinediah5);
            if (!dr.IsDBNull(iRerinediah5)) entity.Rerinediah5 = dr.GetDecimal(iRerinediah5);

            int iRerinediah6 = dr.GetOrdinal(this.Rerinediah6);
            if (!dr.IsDBNull(iRerinediah6)) entity.Rerinediah6 = dr.GetDecimal(iRerinediah6);

            int iRerinediah7 = dr.GetOrdinal(this.Rerinediah7);
            if (!dr.IsDBNull(iRerinediah7)) entity.Rerinediah7 = dr.GetDecimal(iRerinediah7);

            int iRerinediah8 = dr.GetOrdinal(this.Rerinediah8);
            if (!dr.IsDBNull(iRerinediah8)) entity.Rerinediah8 = dr.GetDecimal(iRerinediah8);

            int iRerinediah9 = dr.GetOrdinal(this.Rerinediah9);
            if (!dr.IsDBNull(iRerinediah9)) entity.Rerinediah9 = dr.GetDecimal(iRerinediah9);

            int iRerinediah10 = dr.GetOrdinal(this.Rerinediah10);
            if (!dr.IsDBNull(iRerinediah10)) entity.Rerinediah10 = dr.GetDecimal(iRerinediah10);

            int iRerinediah11 = dr.GetOrdinal(this.Rerinediah11);
            if (!dr.IsDBNull(iRerinediah11)) entity.Rerinediah11 = dr.GetDecimal(iRerinediah11);

            int iRerinediah12 = dr.GetOrdinal(this.Rerinediah12);
            if (!dr.IsDBNull(iRerinediah12)) entity.Rerinediah12 = dr.GetDecimal(iRerinediah12);

            int iRerinediah13 = dr.GetOrdinal(this.Rerinediah13);
            if (!dr.IsDBNull(iRerinediah13)) entity.Rerinediah13 = dr.GetDecimal(iRerinediah13);

            int iRerinediah14 = dr.GetOrdinal(this.Rerinediah14);
            if (!dr.IsDBNull(iRerinediah14)) entity.Rerinediah14 = dr.GetDecimal(iRerinediah14);

            int iRerinediah15 = dr.GetOrdinal(this.Rerinediah15);
            if (!dr.IsDBNull(iRerinediah15)) entity.Rerinediah15 = dr.GetDecimal(iRerinediah15);

            int iRerinediah16 = dr.GetOrdinal(this.Rerinediah16);
            if (!dr.IsDBNull(iRerinediah16)) entity.Rerinediah16 = dr.GetDecimal(iRerinediah16);

            int iRerinediah17 = dr.GetOrdinal(this.Rerinediah17);
            if (!dr.IsDBNull(iRerinediah17)) entity.Rerinediah17 = dr.GetDecimal(iRerinediah17);

            int iRerinediah18 = dr.GetOrdinal(this.Rerinediah18);
            if (!dr.IsDBNull(iRerinediah18)) entity.Rerinediah18 = dr.GetDecimal(iRerinediah18);

            int iRerinediah19 = dr.GetOrdinal(this.Rerinediah19);
            if (!dr.IsDBNull(iRerinediah19)) entity.Rerinediah19 = dr.GetDecimal(iRerinediah19);

            int iRerinediah20 = dr.GetOrdinal(this.Rerinediah20);
            if (!dr.IsDBNull(iRerinediah20)) entity.Rerinediah20 = dr.GetDecimal(iRerinediah20);

            int iRerinediah21 = dr.GetOrdinal(this.Rerinediah21);
            if (!dr.IsDBNull(iRerinediah21)) entity.Rerinediah21 = dr.GetDecimal(iRerinediah21);

            int iRerinediah22 = dr.GetOrdinal(this.Rerinediah22);
            if (!dr.IsDBNull(iRerinediah22)) entity.Rerinediah22 = dr.GetDecimal(iRerinediah22);

            int iRerinediah23 = dr.GetOrdinal(this.Rerinediah23);
            if (!dr.IsDBNull(iRerinediah23)) entity.Rerinediah23 = dr.GetDecimal(iRerinediah23);

            int iRerinediah24 = dr.GetOrdinal(this.Rerinediah24);
            if (!dr.IsDBNull(iRerinediah24)) entity.Rerinediah24 = dr.GetDecimal(iRerinediah24);

            int iRerinediah25 = dr.GetOrdinal(this.Rerinediah25);
            if (!dr.IsDBNull(iRerinediah25)) entity.Rerinediah25 = dr.GetDecimal(iRerinediah25);

            int iRerinediah26 = dr.GetOrdinal(this.Rerinediah26);
            if (!dr.IsDBNull(iRerinediah26)) entity.Rerinediah26 = dr.GetDecimal(iRerinediah26);

            int iRerinediah27 = dr.GetOrdinal(this.Rerinediah27);
            if (!dr.IsDBNull(iRerinediah27)) entity.Rerinediah27 = dr.GetDecimal(iRerinediah27);

            int iRerinediah28 = dr.GetOrdinal(this.Rerinediah28);
            if (!dr.IsDBNull(iRerinediah28)) entity.Rerinediah28 = dr.GetDecimal(iRerinediah28);

            int iRerinediah29 = dr.GetOrdinal(this.Rerinediah29);
            if (!dr.IsDBNull(iRerinediah29)) entity.Rerinediah29 = dr.GetDecimal(iRerinediah29);

            int iRerinediah30 = dr.GetOrdinal(this.Rerinediah30);
            if (!dr.IsDBNull(iRerinediah30)) entity.Rerinediah30 = dr.GetDecimal(iRerinediah30);

            int iRerinediah31 = dr.GetOrdinal(this.Rerinediah31);
            if (!dr.IsDBNull(iRerinediah31)) entity.Rerinediah31 = dr.GetDecimal(iRerinediah31);

            int iRerinediah32 = dr.GetOrdinal(this.Rerinediah32);
            if (!dr.IsDBNull(iRerinediah32)) entity.Rerinediah32 = dr.GetDecimal(iRerinediah32);

            int iRerinediah33 = dr.GetOrdinal(this.Rerinediah33);
            if (!dr.IsDBNull(iRerinediah33)) entity.Rerinediah33 = dr.GetDecimal(iRerinediah33);

            int iRerinediah34 = dr.GetOrdinal(this.Rerinediah34);
            if (!dr.IsDBNull(iRerinediah34)) entity.Rerinediah34 = dr.GetDecimal(iRerinediah34);

            int iRerinediah35 = dr.GetOrdinal(this.Rerinediah35);
            if (!dr.IsDBNull(iRerinediah35)) entity.Rerinediah35 = dr.GetDecimal(iRerinediah35);

            int iRerinediah36 = dr.GetOrdinal(this.Rerinediah36);
            if (!dr.IsDBNull(iRerinediah36)) entity.Rerinediah36 = dr.GetDecimal(iRerinediah36);

            int iRerinediah37 = dr.GetOrdinal(this.Rerinediah37);
            if (!dr.IsDBNull(iRerinediah37)) entity.Rerinediah37 = dr.GetDecimal(iRerinediah37);

            int iRerinediah38 = dr.GetOrdinal(this.Rerinediah38);
            if (!dr.IsDBNull(iRerinediah38)) entity.Rerinediah38 = dr.GetDecimal(iRerinediah38);

            int iRerinediah39 = dr.GetOrdinal(this.Rerinediah39);
            if (!dr.IsDBNull(iRerinediah39)) entity.Rerinediah39 = dr.GetDecimal(iRerinediah39);

            int iRerinediah40 = dr.GetOrdinal(this.Rerinediah40);
            if (!dr.IsDBNull(iRerinediah40)) entity.Rerinediah40 = dr.GetDecimal(iRerinediah40);
           
            int iRerinediah41 = dr.GetOrdinal(this.Rerinediah41);
            if (!dr.IsDBNull(iRerinediah41)) entity.Rerinediah41 = dr.GetDecimal(iRerinediah41);

            int iRerinediah42 = dr.GetOrdinal(this.Rerinediah42);
            if (!dr.IsDBNull(iRerinediah42)) entity.Rerinediah42 = dr.GetDecimal(iRerinediah42);

            int iRerinediah43 = dr.GetOrdinal(this.Rerinediah43);
            if (!dr.IsDBNull(iRerinediah43)) entity.Rerinediah43 = dr.GetDecimal(iRerinediah43);

            int iRerinediah44 = dr.GetOrdinal(this.Rerinediah44);
            if (!dr.IsDBNull(iRerinediah44)) entity.Rerinediah44 = dr.GetDecimal(iRerinediah44);

            int iRerinediah45 = dr.GetOrdinal(this.Rerinediah45);
            if (!dr.IsDBNull(iRerinediah45)) entity.Rerinediah45 = dr.GetDecimal(iRerinediah45);

            int iRerinediah46 = dr.GetOrdinal(this.Rerinediah46);
            if (!dr.IsDBNull(iRerinediah46)) entity.Rerinediah46 = dr.GetDecimal(iRerinediah46);

            int iRerinediah47 = dr.GetOrdinal(this.Rerinediah47);
            if (!dr.IsDBNull(iRerinediah47)) entity.Rerinediah47 = dr.GetDecimal(iRerinediah47);

            int iRerinediah48 = dr.GetOrdinal(this.Rerinediah48);
            if (!dr.IsDBNull(iRerinediah48)) entity.Rerinediah48 = dr.GetDecimal(iRerinediah48);

            int iRerinediah49 = dr.GetOrdinal(this.Rerinediah49);
            if (!dr.IsDBNull(iRerinediah49)) entity.Rerinediah49 = dr.GetDecimal(iRerinediah49);

            int iRerinediah50 = dr.GetOrdinal(this.Rerinediah50);
            if (!dr.IsDBNull(iRerinediah50)) entity.Rerinediah50 = dr.GetDecimal(iRerinediah50);

            int iRerinediah51 = dr.GetOrdinal(this.Rerinediah51);
            if (!dr.IsDBNull(iRerinediah51)) entity.Rerinediah51 = dr.GetDecimal(iRerinediah51);

            int iRerinediah52 = dr.GetOrdinal(this.Rerinediah52);
            if (!dr.IsDBNull(iRerinediah52)) entity.Rerinediah52 = dr.GetDecimal(iRerinediah52);

            int iRerinediah53 = dr.GetOrdinal(this.Rerinediah53);
            if (!dr.IsDBNull(iRerinediah53)) entity.Rerinediah53 = dr.GetDecimal(iRerinediah53);

            int iRerinediah54 = dr.GetOrdinal(this.Rerinediah54);
            if (!dr.IsDBNull(iRerinediah54)) entity.Rerinediah54 = dr.GetDecimal(iRerinediah54);

            int iRerinediah55 = dr.GetOrdinal(this.Rerinediah55);
            if (!dr.IsDBNull(iRerinediah55)) entity.Rerinediah55 = dr.GetDecimal(iRerinediah55);

            int iRerinediah56 = dr.GetOrdinal(this.Rerinediah56);
            if (!dr.IsDBNull(iRerinediah56)) entity.Rerinediah56 = dr.GetDecimal(iRerinediah56);

            int iRerinediah57 = dr.GetOrdinal(this.Rerinediah57);
            if (!dr.IsDBNull(iRerinediah57)) entity.Rerinediah57 = dr.GetDecimal(iRerinediah57);

            int iRerinediah58 = dr.GetOrdinal(this.Rerinediah58);
            if (!dr.IsDBNull(iRerinediah58)) entity.Rerinediah58 = dr.GetDecimal(iRerinediah58);

            int iRerinediah59 = dr.GetOrdinal(this.Rerinediah59);
            if (!dr.IsDBNull(iRerinediah59)) entity.Rerinediah59 = dr.GetDecimal(iRerinediah59);

            int iRerinediah60 = dr.GetOrdinal(this.Rerinediah60);
            if (!dr.IsDBNull(iRerinediah60)) entity.Rerinediah60 = dr.GetDecimal(iRerinediah60);

            int iRerinediah61 = dr.GetOrdinal(this.Rerinediah61);
            if (!dr.IsDBNull(iRerinediah61)) entity.Rerinediah61 = dr.GetDecimal(iRerinediah61);

            int iRerinediah62 = dr.GetOrdinal(this.Rerinediah62);
            if (!dr.IsDBNull(iRerinediah62)) entity.Rerinediah62 = dr.GetDecimal(iRerinediah62);

            int iRerinediah63 = dr.GetOrdinal(this.Rerinediah63);
            if (!dr.IsDBNull(iRerinediah63)) entity.Rerinediah63 = dr.GetDecimal(iRerinediah63);

            int iRerinediah64 = dr.GetOrdinal(this.Rerinediah64);
            if (!dr.IsDBNull(iRerinediah64)) entity.Rerinediah64 = dr.GetDecimal(iRerinediah64);

            int iRerinediah65 = dr.GetOrdinal(this.Rerinediah65);
            if (!dr.IsDBNull(iRerinediah65)) entity.Rerinediah65 = dr.GetDecimal(iRerinediah65);

            int iRerinediah66 = dr.GetOrdinal(this.Rerinediah66);
            if (!dr.IsDBNull(iRerinediah66)) entity.Rerinediah66 = dr.GetDecimal(iRerinediah66);

            int iRerinediah67 = dr.GetOrdinal(this.Rerinediah67);
            if (!dr.IsDBNull(iRerinediah67)) entity.Rerinediah67 = dr.GetDecimal(iRerinediah67);

            int iRerinediah68 = dr.GetOrdinal(this.Rerinediah68);
            if (!dr.IsDBNull(iRerinediah68)) entity.Rerinediah68 = dr.GetDecimal(iRerinediah68);

            int iRerinediah69 = dr.GetOrdinal(this.Rerinediah69);
            if (!dr.IsDBNull(iRerinediah69)) entity.Rerinediah69 = dr.GetDecimal(iRerinediah69);

            int iRerinediah70 = dr.GetOrdinal(this.Rerinediah70);
            if (!dr.IsDBNull(iRerinediah70)) entity.Rerinediah70 = dr.GetDecimal(iRerinediah70);

            int iRerinediah71 = dr.GetOrdinal(this.Rerinediah71);
            if (!dr.IsDBNull(iRerinediah71)) entity.Rerinediah71 = dr.GetDecimal(iRerinediah71);

            int iRerinediah72 = dr.GetOrdinal(this.Rerinediah72);
            if (!dr.IsDBNull(iRerinediah72)) entity.Rerinediah72 = dr.GetDecimal(iRerinediah72);

            int iRerinediah73 = dr.GetOrdinal(this.Rerinediah73);
            if (!dr.IsDBNull(iRerinediah73)) entity.Rerinediah73 = dr.GetDecimal(iRerinediah73);

            int iRerinediah74 = dr.GetOrdinal(this.Rerinediah74);
            if (!dr.IsDBNull(iRerinediah74)) entity.Rerinediah74 = dr.GetDecimal(iRerinediah74);

            int iRerinediah75 = dr.GetOrdinal(this.Rerinediah75);
            if (!dr.IsDBNull(iRerinediah75)) entity.Rerinediah75 = dr.GetDecimal(iRerinediah75);

            int iRerinediah76 = dr.GetOrdinal(this.Rerinediah76);
            if (!dr.IsDBNull(iRerinediah76)) entity.Rerinediah76 = dr.GetDecimal(iRerinediah76);

            int iRerinediah77 = dr.GetOrdinal(this.Rerinediah77);
            if (!dr.IsDBNull(iRerinediah77)) entity.Rerinediah77 = dr.GetDecimal(iRerinediah77);

            int iRerinediah78 = dr.GetOrdinal(this.Rerinediah78);
            if (!dr.IsDBNull(iRerinediah78)) entity.Rerinediah78 = dr.GetDecimal(iRerinediah78);

            int iRerinediah79 = dr.GetOrdinal(this.Rerinediah79);
            if (!dr.IsDBNull(iRerinediah79)) entity.Rerinediah79 = dr.GetDecimal(iRerinediah79);

            int iRerinediah80 = dr.GetOrdinal(this.Rerinediah80);
            if (!dr.IsDBNull(iRerinediah80)) entity.Rerinediah80 = dr.GetDecimal(iRerinediah80);

            int iRerinediah81 = dr.GetOrdinal(this.Rerinediah81);
            if (!dr.IsDBNull(iRerinediah81)) entity.Rerinediah81 = dr.GetDecimal(iRerinediah81);

            int iRerinediah82 = dr.GetOrdinal(this.Rerinediah82);
            if (!dr.IsDBNull(iRerinediah82)) entity.Rerinediah82 = dr.GetDecimal(iRerinediah82);

            int iRerinediah83 = dr.GetOrdinal(this.Rerinediah83);
            if (!dr.IsDBNull(iRerinediah83)) entity.Rerinediah83 = dr.GetDecimal(iRerinediah83);

            int iRerinediah84 = dr.GetOrdinal(this.Rerinediah84);
            if (!dr.IsDBNull(iRerinediah84)) entity.Rerinediah84 = dr.GetDecimal(iRerinediah84);

            int iRerinediah85 = dr.GetOrdinal(this.Rerinediah85);
            if (!dr.IsDBNull(iRerinediah85)) entity.Rerinediah85 = dr.GetDecimal(iRerinediah85);

            int iRerinediah86 = dr.GetOrdinal(this.Rerinediah86);
            if (!dr.IsDBNull(iRerinediah86)) entity.Rerinediah86 = dr.GetDecimal(iRerinediah86);

            int iRerinediah87 = dr.GetOrdinal(this.Rerinediah87);
            if (!dr.IsDBNull(iRerinediah87)) entity.Rerinediah87 = dr.GetDecimal(iRerinediah87);

            int iRerinediah88 = dr.GetOrdinal(this.Rerinediah88);
            if (!dr.IsDBNull(iRerinediah88)) entity.Rerinediah88 = dr.GetDecimal(iRerinediah88);

            int iRerinediah89 = dr.GetOrdinal(this.Rerinediah89);
            if (!dr.IsDBNull(iRerinediah89)) entity.Rerinediah89 = dr.GetDecimal(iRerinediah89);

            int iRerinediah90 = dr.GetOrdinal(this.Rerinediah90);
            if (!dr.IsDBNull(iRerinediah90)) entity.Rerinediah90 = dr.GetDecimal(iRerinediah90);

            int iRerinediah91 = dr.GetOrdinal(this.Rerinediah91);
            if (!dr.IsDBNull(iRerinediah91)) entity.Rerinediah91 = dr.GetDecimal(iRerinediah91);

            int iRerinediah92 = dr.GetOrdinal(this.Rerinediah92);
            if (!dr.IsDBNull(iRerinediah92)) entity.Rerinediah92 = dr.GetDecimal(iRerinediah92);

            int iRerinediah93 = dr.GetOrdinal(this.Rerinediah93);
            if (!dr.IsDBNull(iRerinediah93)) entity.Rerinediah93 = dr.GetDecimal(iRerinediah93);

            int iRerinediah94 = dr.GetOrdinal(this.Rerinediah94);
            if (!dr.IsDBNull(iRerinediah94)) entity.Rerinediah94 = dr.GetDecimal(iRerinediah94);

            int iRerinediah95 = dr.GetOrdinal(this.Rerinediah95);
            if (!dr.IsDBNull(iRerinediah95)) entity.Rerinediah95 = dr.GetDecimal(iRerinediah95);

            int iRerinediah96 = dr.GetOrdinal(this.Rerinediah96);
            if (!dr.IsDBNull(iRerinediah96)) entity.Rerinediah96 = dr.GetDecimal(iRerinediah96);


            int iRerInediatotal = dr.GetOrdinal(this.Rerinediatotal);
            if (!dr.IsDBNull(iRerInediatotal)) entity.Rerinediatotal = dr.GetDecimal(iRerInediatotal);

            int iRerInediausucreacion = dr.GetOrdinal(this.Rerinediausucreacion);
            if (!dr.IsDBNull(iRerInediausucreacion)) entity.Rerinediausucreacion = dr.GetString(iRerInediausucreacion);

            int iRerInediafeccreacion = dr.GetOrdinal(this.Rerinediafeccreacion);
            if (!dr.IsDBNull(iRerInediafeccreacion)) entity.Rerinediafeccreacion = dr.GetDateTime(iRerInediafeccreacion);

            return entity;
        }


        public RerInsumoVteaDTO CreateByPeriodo(IDataReader dr)
        {
            RerInsumoVteaDTO entity = Create(dr);

            //Additonal
            int iPerinombre = dr.GetOrdinal(this.Perinombre);
            if (!dr.IsDBNull(iPerinombre)) entity.Perinombre = dr.GetString(iPerinombre);

            int iRecanombre = dr.GetOrdinal(this.Recanombre);
            if (!dr.IsDBNull(iRecanombre)) entity.Recanombre = dr.GetString(iRecanombre);

            return entity;
        }

        #region Mapeo de Campos
        public string Rerinecodi = "RERINECODI";
        public string Rerinscodi = "RERINSCODI";
        public string Rerpprcodi = "RERPPRCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Rerinefecdia = "RERINEFECDIA";
        public string Recacodi = "RECACODI";
        public string Pericodi = "PERICODI";

        public string Rerinediah1 = "RERINEDIAH1";
        public string Rerinediah2 = "RERINEDIAH2";
        public string Rerinediah3 = "RERINEDIAH3";
        public string Rerinediah4 = "RERINEDIAH4";
        public string Rerinediah5 = "RERINEDIAH5";
        public string Rerinediah6 = "RERINEDIAH6";
        public string Rerinediah7 = "RERINEDIAH7";
        public string Rerinediah8 = "RERINEDIAH8";
        public string Rerinediah9 = "RERINEDIAH9";
        public string Rerinediah10 = "RERINEDIAH10";
        public string Rerinediah11 = "RERINEDIAH11";
        public string Rerinediah12 = "RERINEDIAH12";
        public string Rerinediah13 = "RERINEDIAH13";
        public string Rerinediah14 = "RERINEDIAH14";
        public string Rerinediah15 = "RERINEDIAH15";
        public string Rerinediah16 = "RERINEDIAH16";
        public string Rerinediah17 = "RERINEDIAH17";
        public string Rerinediah18 = "RERINEDIAH18";
        public string Rerinediah19 = "RERINEDIAH19";
        public string Rerinediah20 = "RERINEDIAH20";
        public string Rerinediah21 = "RERINEDIAH21";
        public string Rerinediah22 = "RERINEDIAH22";
        public string Rerinediah23 = "RERINEDIAH23";
        public string Rerinediah24 = "RERINEDIAH24";
        public string Rerinediah25 = "RERINEDIAH25";
        public string Rerinediah26 = "RERINEDIAH26";
        public string Rerinediah27 = "RERINEDIAH27";
        public string Rerinediah28 = "RERINEDIAH28";
        public string Rerinediah29 = "RERINEDIAH29";
        public string Rerinediah30 = "RERINEDIAH30";
        public string Rerinediah31 = "RERINEDIAH31";
        public string Rerinediah32 = "RERINEDIAH32";
        public string Rerinediah33 = "RERINEDIAH33";
        public string Rerinediah34 = "RERINEDIAH34";
        public string Rerinediah35 = "RERINEDIAH35";
        public string Rerinediah36 = "RERINEDIAH36";
        public string Rerinediah37 = "RERINEDIAH37";
        public string Rerinediah38 = "RERINEDIAH38";
        public string Rerinediah39 = "RERINEDIAH39";
        public string Rerinediah40 = "RERINEDIAH40";
        public string Rerinediah41 = "RERINEDIAH41";
        public string Rerinediah42 = "RERINEDIAH42";
        public string Rerinediah43 = "RERINEDIAH43";
        public string Rerinediah44 = "RERINEDIAH44";
        public string Rerinediah45 = "RERINEDIAH45";
        public string Rerinediah46 = "RERINEDIAH46";
        public string Rerinediah47 = "RERINEDIAH47";
        public string Rerinediah48 = "RERINEDIAH48";
        public string Rerinediah49 = "RERINEDIAH49";
        public string Rerinediah50 = "RERINEDIAH50";
        public string Rerinediah51 = "RERINEDIAH51";
        public string Rerinediah52 = "RERINEDIAH52";
        public string Rerinediah53 = "RERINEDIAH53";
        public string Rerinediah54 = "RERINEDIAH54";
        public string Rerinediah55 = "RERINEDIAH55";
        public string Rerinediah56 = "RERINEDIAH56";
        public string Rerinediah57 = "RERINEDIAH57";
        public string Rerinediah58 = "RERINEDIAH58";
        public string Rerinediah59 = "RERINEDIAH59";
        public string Rerinediah60 = "RERINEDIAH60";
        public string Rerinediah61 = "RERINEDIAH61";
        public string Rerinediah62 = "RERINEDIAH62";
        public string Rerinediah63 = "RERINEDIAH63";
        public string Rerinediah64 = "RERINEDIAH64";
        public string Rerinediah65 = "RERINEDIAH65";
        public string Rerinediah66 = "RERINEDIAH66";
        public string Rerinediah67 = "RERINEDIAH67";
        public string Rerinediah68 = "RERINEDIAH68";
        public string Rerinediah69 = "RERINEDIAH69";
        public string Rerinediah70 = "RERINEDIAH70";
        public string Rerinediah71 = "RERINEDIAH71";
        public string Rerinediah72 = "RERINEDIAH72";
        public string Rerinediah73 = "RERINEDIAH73";
        public string Rerinediah74 = "RERINEDIAH74";
        public string Rerinediah75 = "RERINEDIAH75";
        public string Rerinediah76 = "RERINEDIAH76";
        public string Rerinediah77 = "RERINEDIAH77";
        public string Rerinediah78 = "RERINEDIAH78";
        public string Rerinediah79 = "RERINEDIAH79";
        public string Rerinediah80 = "RERINEDIAH80";
        public string Rerinediah81 = "RERINEDIAH81";
        public string Rerinediah82 = "RERINEDIAH82";
        public string Rerinediah83 = "RERINEDIAH83";
        public string Rerinediah84 = "RERINEDIAH84";
        public string Rerinediah85 = "RERINEDIAH85";
        public string Rerinediah86 = "RERINEDIAH86";
        public string Rerinediah87 = "RERINEDIAH87";
        public string Rerinediah88 = "RERINEDIAH88";
        public string Rerinediah89 = "RERINEDIAH89";
        public string Rerinediah90 = "RERINEDIAH90";
        public string Rerinediah91 = "RERINEDIAH91";
        public string Rerinediah92 = "RERINEDIAH92";
        public string Rerinediah93 = "RERINEDIAH93";
        public string Rerinediah94 = "RERINEDIAH94";
        public string Rerinediah95 = "RERINEDIAH95";
        public string Rerinediah96 = "RERINEDIAH96";


        public string Rerinediatotal = "RERINEDIATOTAL";
        public string Rerinediausucreacion = "RERINEDIAUSUCREACION";
        public string Rerinediafeccreacion = "RERINEDIAFECCREACION";

        //Atributos de consulta
        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "EQUINOMB";
        public string Rerpprmes = "RERPPRMES";
        public string Perinombre = "PERINOMBRE";
        public string Recanombre = "RECANOMBRE";
        #endregion

        public string SqlDeleteByParametroPrimaAndMes
        {
            get { return base.GetSqlXml("DeleteByParametroPrimaAndMes"); }
        }

        public string SqlGetByPeriodo
        {
            get { return base.GetSqlXml("GetByPeriodo"); }
        }

        public string SqlObtenerSaldoVteaByInsumoVTEA
        {
            get { return base.GetSqlXml("ObtenerSaldoVteaByInsumoVTEA"); }
        }
    }
}
