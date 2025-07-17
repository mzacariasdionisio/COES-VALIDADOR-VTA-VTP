using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class GmmValEnergiaHelper : HelperBase
    {
        public GmmValEnergiaHelper()
            : base(Consultas.GmmValEnergia)
        {

        }

        #region Mapeo de Campos
        public string VALOCODI = "VALOCODI";
        public string PERICODI = "PERICODI";
        public string BARRCODI = "BARRCODI";
        public string CASDDBBARRA = "CASDDBBARRA";
        public string PTOMEDICODI = "PTOMEDICODI";
        public string MEDIFECHA = "MEDIFECHA";
        public string TPTOMEDICODI = "TPTOMEDICODI";
        public string TIPOINFOCODI = "TIPOINFOCODI";
        public string LECTCODI = "LECTCODI";
        public string EMPRCODI = "EMPRCODI";
        public string EMPGCODI = "EMPGCODI";
        public string VALANIO = "VALANIO";
        public string VALMES = "VALMES";
        public string VALOVALOR1 = "VALOVALOR1";
        public string VALOVALOR2 = "VALOVALOR2";
        public string VALOVALOR3 = "VALOVALOR3";
        public string VALOVALOR4 = "VALOVALOR4";
        public string VALOVALOR5 = "VALOVALOR5";
        public string VALOVALOR6 = "VALOVALOR6";
        public string VALOVALOR7 = "VALOVALOR7";
        public string VALOVALOR8 = "VALOVALOR8";
        public string VALOVALOR9 = "VALOVALOR9";
        public string VALOVALOR10 = "VALOVALOR10";
        public string VALOVALOR11 = "VALOVALOR11";
        public string VALOVALOR12 = "VALOVALOR12";
        public string VALOVALOR13 = "VALOVALOR13";
        public string VALOVALOR14 = "VALOVALOR14";
        public string VALOVALOR15 = "VALOVALOR15";
        public string VALOVALOR16 = "VALOVALOR16";
        public string VALOVALOR17 = "VALOVALOR17";
        public string VALOVALOR18 = "VALOVALOR18";
        public string VALOVALOR19 = "VALOVALOR19";
        public string VALOVALOR20 = "VALOVALOR20";
        public string VALOVALOR21 = "VALOVALOR21";
        public string VALOVALOR22 = "VALOVALOR22";
        public string VALOVALOR23 = "VALOVALOR23";
        public string VALOVALOR24 = "VALOVALOR24";
        public string VALOVALOR25 = "VALOVALOR25";
        public string VALOVALOR26 = "VALOVALOR26";
        public string VALOVALOR27 = "VALOVALOR27";
        public string VALOVALOR28 = "VALOVALOR28";
        public string VALOVALOR29 = "VALOVALOR29";
        public string VALOVALOR30 = "VALOVALOR30";
        public string VALOVALOR31 = "VALOVALOR31";
        public string VALOVALOR32 = "VALOVALOR32";
        public string VALOVALOR33 = "VALOVALOR33";
        public string VALOVALOR34 = "VALOVALOR34";
        public string VALOVALOR35 = "VALOVALOR35";
        public string VALOVALOR36 = "VALOVALOR36";
        public string VALOVALOR37 = "VALOVALOR37";
        public string VALOVALOR38 = "VALOVALOR38";
        public string VALOVALOR39 = "VALOVALOR39";
        public string VALOVALOR40 = "VALOVALOR40";
        public string VALOVALOR41 = "VALOVALOR41";
        public string VALOVALOR42 = "VALOVALOR42";
        public string VALOVALOR43 = "VALOVALOR43";
        public string VALOVALOR44 = "VALOVALOR44";
        public string VALOVALOR45 = "VALOVALOR45";
        public string VALOVALOR46 = "VALOVALOR46";
        public string VALOVALOR47 = "VALOVALOR47";
        public string VALOVALOR48 = "VALOVALOR48";
        public string VALOVALOR49 = "VALOVALOR49";
        public string VALOVALOR50 = "VALOVALOR50";
        public string VALOVALOR51 = "VALOVALOR51";
        public string VALOVALOR52 = "VALOVALOR52";
        public string VALOVALOR53 = "VALOVALOR53";
        public string VALOVALOR54 = "VALOVALOR54";
        public string VALOVALOR55 = "VALOVALOR55";
        public string VALOVALOR56 = "VALOVALOR56";
        public string VALOVALOR57 = "VALOVALOR57";
        public string VALOVALOR58 = "VALOVALOR58";
        public string VALOVALOR59 = "VALOVALOR59";
        public string VALOVALOR60 = "VALOVALOR60";
        public string VALOVALOR61 = "VALOVALOR61";
        public string VALOVALOR62 = "VALOVALOR62";
        public string VALOVALOR63 = "VALOVALOR63";
        public string VALOVALOR64 = "VALOVALOR64";
        public string VALOVALOR65 = "VALOVALOR65";
        public string VALOVALOR66 = "VALOVALOR66";
        public string VALOVALOR67 = "VALOVALOR67";
        public string VALOVALOR68 = "VALOVALOR68";
        public string VALOVALOR69 = "VALOVALOR69";
        public string VALOVALOR70 = "VALOVALOR70";
        public string VALOVALOR71 = "VALOVALOR71";
        public string VALOVALOR72 = "VALOVALOR72";
        public string VALOVALOR73 = "VALOVALOR73";
        public string VALOVALOR74 = "VALOVALOR74";
        public string VALOVALOR75 = "VALOVALOR75";
        public string VALOVALOR76 = "VALOVALOR76";
        public string VALOVALOR77 = "VALOVALOR77";
        public string VALOVALOR78 = "VALOVALOR78";
        public string VALOVALOR79 = "VALOVALOR79";
        public string VALOVALOR80 = "VALOVALOR80";
        public string VALOVALOR81 = "VALOVALOR81";
        public string VALOVALOR82 = "VALOVALOR82";
        public string VALOVALOR83 = "VALOVALOR83";
        public string VALOVALOR84 = "VALOVALOR84";
        public string VALOVALOR85 = "VALOVALOR85";
        public string VALOVALOR86 = "VALOVALOR86";
        public string VALOVALOR87 = "VALOVALOR87";
        public string VALOVALOR88 = "VALOVALOR88";
        public string VALOVALOR89 = "VALOVALOR89";
        public string VALOVALOR90 = "VALOVALOR90";
        public string VALOVALOR91 = "VALOVALOR91";
        public string VALOVALOR92 = "VALOVALOR92";
        public string VALOVALOR93 = "VALOVALOR93";
        public string VALOVALOR94 = "VALOVALOR94";
        public string VALOVALOR95 = "VALOVALOR95";
        public string VALOVALOR96 = "VALOVALOR96";

        public string VALOVALORCM1 = "VALOVALORCM1";
        public string VALOVALORCM2 = "VALOVALORCM2";
        public string VALOVALORCM3 = "VALOVALORCM3";
        public string VALOVALORCM4 = "VALOVALORCM4";
        public string VALOVALORCM5 = "VALOVALORCM5";
        public string VALOVALORCM6 = "VALOVALORCM6";
        public string VALOVALORCM7 = "VALOVALORCM7";
        public string VALOVALORCM8 = "VALOVALORCM8";
        public string VALOVALORCM9 = "VALOVALORCM9";
        public string VALOVALORCM10 = "VALOVALORCM10";
        public string VALOVALORCM11 = "VALOVALORCM11";
        public string VALOVALORCM12 = "VALOVALORCM12";
        public string VALOVALORCM13 = "VALOVALORCM13";
        public string VALOVALORCM14 = "VALOVALORCM14";
        public string VALOVALORCM15 = "VALOVALORCM15";
        public string VALOVALORCM16 = "VALOVALORCM16";
        public string VALOVALORCM17 = "VALOVALORCM17";
        public string VALOVALORCM18 = "VALOVALORCM18";
        public string VALOVALORCM19 = "VALOVALORCM19";
        public string VALOVALORCM20 = "VALOVALORCM20";
        public string VALOVALORCM21 = "VALOVALORCM21";
        public string VALOVALORCM22 = "VALOVALORCM22";
        public string VALOVALORCM23 = "VALOVALORCM23";
        public string VALOVALORCM24 = "VALOVALORCM24";
        public string VALOVALORCM25 = "VALOVALORCM25";
        public string VALOVALORCM26 = "VALOVALORCM26";
        public string VALOVALORCM27 = "VALOVALORCM27";
        public string VALOVALORCM28 = "VALOVALORCM28";
        public string VALOVALORCM29 = "VALOVALORCM29";
        public string VALOVALORCM30 = "VALOVALORCM30";
        public string VALOVALORCM31 = "VALOVALORCM31";
        public string VALOVALORCM32 = "VALOVALORCM32";
        public string VALOVALORCM33 = "VALOVALORCM33";
        public string VALOVALORCM34 = "VALOVALORCM34";
        public string VALOVALORCM35 = "VALOVALORCM35";
        public string VALOVALORCM36 = "VALOVALORCM36";
        public string VALOVALORCM37 = "VALOVALORCM37";
        public string VALOVALORCM38 = "VALOVALORCM38";
        public string VALOVALORCM39 = "VALOVALORCM39";
        public string VALOVALORCM40 = "VALOVALORCM40";
        public string VALOVALORCM41 = "VALOVALORCM41";
        public string VALOVALORCM42 = "VALOVALORCM42";
        public string VALOVALORCM43 = "VALOVALORCM43";
        public string VALOVALORCM44 = "VALOVALORCM44";
        public string VALOVALORCM45 = "VALOVALORCM45";
        public string VALOVALORCM46 = "VALOVALORCM46";
        public string VALOVALORCM47 = "VALOVALORCM47";
        public string VALOVALORCM48 = "VALOVALORCM48";
        public string VALOVALORCM49 = "VALOVALORCM49";
        public string VALOVALORCM50 = "VALOVALORCM50";
        public string VALOVALORCM51 = "VALOVALORCM51";
        public string VALOVALORCM52 = "VALOVALORCM52";
        public string VALOVALORCM53 = "VALOVALORCM53";
        public string VALOVALORCM54 = "VALOVALORCM54";
        public string VALOVALORCM55 = "VALOVALORCM55";
        public string VALOVALORCM56 = "VALOVALORCM56";
        public string VALOVALORCM57 = "VALOVALORCM57";
        public string VALOVALORCM58 = "VALOVALORCM58";
        public string VALOVALORCM59 = "VALOVALORCM59";
        public string VALOVALORCM60 = "VALOVALORCM60";
        public string VALOVALORCM61 = "VALOVALORCM61";
        public string VALOVALORCM62 = "VALOVALORCM62";
        public string VALOVALORCM63 = "VALOVALORCM63";
        public string VALOVALORCM64 = "VALOVALORCM64";
        public string VALOVALORCM65 = "VALOVALORCM65";
        public string VALOVALORCM66 = "VALOVALORCM66";
        public string VALOVALORCM67 = "VALOVALORCM67";
        public string VALOVALORCM68 = "VALOVALORCM68";
        public string VALOVALORCM69 = "VALOVALORCM69";
        public string VALOVALORCM70 = "VALOVALORCM70";
        public string VALOVALORCM71 = "VALOVALORCM71";
        public string VALOVALORCM72 = "VALOVALORCM72";
        public string VALOVALORCM73 = "VALOVALORCM73";
        public string VALOVALORCM74 = "VALOVALORCM74";
        public string VALOVALORCM75 = "VALOVALORCM75";
        public string VALOVALORCM76 = "VALOVALORCM76";
        public string VALOVALORCM77 = "VALOVALORCM77";
        public string VALOVALORCM78 = "VALOVALORCM78";
        public string VALOVALORCM79 = "VALOVALORCM79";
        public string VALOVALORCM80 = "VALOVALORCM80";
        public string VALOVALORCM81 = "VALOVALORCM81";
        public string VALOVALORCM82 = "VALOVALORCM82";
        public string VALOVALORCM83 = "VALOVALORCM83";
        public string VALOVALORCM84 = "VALOVALORCM84";
        public string VALOVALORCM85 = "VALOVALORCM85";
        public string VALOVALORCM86 = "VALOVALORCM86";
        public string VALOVALORCM87 = "VALOVALORCM87";
        public string VALOVALORCM88 = "VALOVALORCM88";
        public string VALOVALORCM89 = "VALOVALORCM89";
        public string VALOVALORCM90 = "VALOVALORCM90";
        public string VALOVALORCM91 = "VALOVALORCM91";
        public string VALOVALORCM92 = "VALOVALORCM92";
        public string VALOVALORCM93 = "VALOVALORCM93";
        public string VALOVALORCM94 = "VALOVALORCM94";
        public string VALOVALORCM95 = "VALOVALORCM95";
        public string VALOVALORCM96 = "VALOVALORCM96";

        public string VALUSUCREACION = "VALUSUCREACION";
        public string VALFECCREACION = "VALFECCREACION";

        public string MEDINTBLQNUMERO = "MEDINTBLQNUMERO";
        public string MEDINTFECHAINI = "MEDINTFECHAINI";
        public string MEDINTH1 = "MEDINTH1";

        #endregion

        public string SqlListarValores96Originales
        {
            get { return base.GetSqlXml("ListarValores96Originales"); }
        }

        public string SqlListarValoresCostoMarginal
        {
            get { return base.GetSqlXml("ListarValoresCostoMarginal"); }
        }

        public string SqlGetSemana
        {
            get { return base.GetSqlXml("GetSemana"); }
        }

        public string SqlGetEnvio
        {
            get { return base.GetSqlXml("GetEnvio"); }
        }

        public GmmValEnergiaDTO CreateListaValores(IDataReader dr)
        {
            GmmValEnergiaDTO entity = new GmmValEnergiaDTO();

            #region Lista Valores de Energia
            int iLecturaCodi = dr.GetOrdinal(this.LECTCODI);
            if (!dr.IsDBNull(iLecturaCodi)) entity.LECTCODI = Convert.ToInt32(dr.GetValue(iLecturaCodi));
            int iEMedifecha = dr.GetOrdinal(this.MEDIFECHA);
            if (!dr.IsDBNull(iEMedifecha)) entity.MEDIFECHA = dr.GetDateTime(iEMedifecha);

            int iVALANIO = dr.GetOrdinal(this.VALANIO);
            if (!dr.IsDBNull(iVALANIO)) entity.VALANIO = dr.GetInt32(iVALANIO);
            int iVALMES = dr.GetOrdinal(this.VALMES);
            if (!dr.IsDBNull(iVALMES)) entity.VALMES = dr.GetInt32(iVALMES);

            int iValValor1 = dr.GetOrdinal(this.VALOVALOR1);
            if (!dr.IsDBNull(iValValor1)) entity.VALOVALOR1 = dr.GetDecimal(iValValor1);
            int iValValor2 = dr.GetOrdinal(this.VALOVALOR2);
            if (!dr.IsDBNull(iValValor2)) entity.VALOVALOR2 = dr.GetDecimal(iValValor2);
            int iValValor3 = dr.GetOrdinal(this.VALOVALOR3);
            if (!dr.IsDBNull(iValValor3)) entity.VALOVALOR3 = dr.GetDecimal(iValValor3);
            int iValValor4 = dr.GetOrdinal(this.VALOVALOR4);
            if (!dr.IsDBNull(iValValor4)) entity.VALOVALOR4 = dr.GetDecimal(iValValor4);
            int iValValor5 = dr.GetOrdinal(this.VALOVALOR5);
            if (!dr.IsDBNull(iValValor5)) entity.VALOVALOR5 = dr.GetDecimal(iValValor5);
            int iValValor6 = dr.GetOrdinal(this.VALOVALOR6);
            if (!dr.IsDBNull(iValValor6)) entity.VALOVALOR6 = dr.GetDecimal(iValValor6);
            int iValValor7 = dr.GetOrdinal(this.VALOVALOR7);
            if (!dr.IsDBNull(iValValor7)) entity.VALOVALOR7 = dr.GetDecimal(iValValor7);
            int iValValor8 = dr.GetOrdinal(this.VALOVALOR8);
            if (!dr.IsDBNull(iValValor8)) entity.VALOVALOR8 = dr.GetDecimal(iValValor8);
            int iValValor9 = dr.GetOrdinal(this.VALOVALOR9);
            if (!dr.IsDBNull(iValValor9)) entity.VALOVALOR9 = dr.GetDecimal(iValValor9);
            int iValValor10 = dr.GetOrdinal(this.VALOVALOR10);
            if (!dr.IsDBNull(iValValor10)) entity.VALOVALOR10 = dr.GetDecimal(iValValor10);
            int iValValor11 = dr.GetOrdinal(this.VALOVALOR11);
            if (!dr.IsDBNull(iValValor11)) entity.VALOVALOR11 = dr.GetDecimal(iValValor11);
            int iValValor12 = dr.GetOrdinal(this.VALOVALOR12);
            if (!dr.IsDBNull(iValValor12)) entity.VALOVALOR12 = dr.GetDecimal(iValValor12);
            int iValValor13 = dr.GetOrdinal(this.VALOVALOR13);
            if (!dr.IsDBNull(iValValor13)) entity.VALOVALOR13 = dr.GetDecimal(iValValor13);
            int iValValor14 = dr.GetOrdinal(this.VALOVALOR14);
            if (!dr.IsDBNull(iValValor14)) entity.VALOVALOR14 = dr.GetDecimal(iValValor14);
            int iValValor15 = dr.GetOrdinal(this.VALOVALOR15);
            if (!dr.IsDBNull(iValValor15)) entity.VALOVALOR15 = dr.GetDecimal(iValValor15);
            int iValValor16 = dr.GetOrdinal(this.VALOVALOR16);
            if (!dr.IsDBNull(iValValor16)) entity.VALOVALOR16 = dr.GetDecimal(iValValor16);
            int iValValor17 = dr.GetOrdinal(this.VALOVALOR17);
            if (!dr.IsDBNull(iValValor17)) entity.VALOVALOR17 = dr.GetDecimal(iValValor17);
            int iValValor18 = dr.GetOrdinal(this.VALOVALOR18);
            if (!dr.IsDBNull(iValValor18)) entity.VALOVALOR18 = dr.GetDecimal(iValValor18);
            int iValValor19 = dr.GetOrdinal(this.VALOVALOR19);
            if (!dr.IsDBNull(iValValor19)) entity.VALOVALOR19 = dr.GetDecimal(iValValor19);
            int iValValor20 = dr.GetOrdinal(this.VALOVALOR20);
            if (!dr.IsDBNull(iValValor20)) entity.VALOVALOR20 = dr.GetDecimal(iValValor20);
            int iValValor21 = dr.GetOrdinal(this.VALOVALOR21);
            if (!dr.IsDBNull(iValValor21)) entity.VALOVALOR21 = dr.GetDecimal(iValValor21);
            int iValValor22 = dr.GetOrdinal(this.VALOVALOR22);
            if (!dr.IsDBNull(iValValor22)) entity.VALOVALOR22 = dr.GetDecimal(iValValor22);
            int iValValor23 = dr.GetOrdinal(this.VALOVALOR23);
            if (!dr.IsDBNull(iValValor23)) entity.VALOVALOR23 = dr.GetDecimal(iValValor23);
            int iValValor24 = dr.GetOrdinal(this.VALOVALOR24);
            if (!dr.IsDBNull(iValValor24)) entity.VALOVALOR24 = dr.GetDecimal(iValValor24);
            int iValValor25 = dr.GetOrdinal(this.VALOVALOR25);
            if (!dr.IsDBNull(iValValor25)) entity.VALOVALOR25 = dr.GetDecimal(iValValor25);
            int iValValor26 = dr.GetOrdinal(this.VALOVALOR26);
            if (!dr.IsDBNull(iValValor26)) entity.VALOVALOR26 = dr.GetDecimal(iValValor26);
            int iValValor27 = dr.GetOrdinal(this.VALOVALOR27);
            if (!dr.IsDBNull(iValValor27)) entity.VALOVALOR27 = dr.GetDecimal(iValValor27);
            int iValValor28 = dr.GetOrdinal(this.VALOVALOR28);
            if (!dr.IsDBNull(iValValor28)) entity.VALOVALOR28 = dr.GetDecimal(iValValor28);
            int iValValor29 = dr.GetOrdinal(this.VALOVALOR29);
            if (!dr.IsDBNull(iValValor29)) entity.VALOVALOR29 = dr.GetDecimal(iValValor29);
            int iValValor30 = dr.GetOrdinal(this.VALOVALOR30);
            if (!dr.IsDBNull(iValValor30)) entity.VALOVALOR30 = dr.GetDecimal(iValValor30);
            int iValValor31 = dr.GetOrdinal(this.VALOVALOR31);
            if (!dr.IsDBNull(iValValor31)) entity.VALOVALOR31 = dr.GetDecimal(iValValor31);
            int iValValor32 = dr.GetOrdinal(this.VALOVALOR32);
            if (!dr.IsDBNull(iValValor32)) entity.VALOVALOR32 = dr.GetDecimal(iValValor32);
            int iValValor33 = dr.GetOrdinal(this.VALOVALOR33);
            if (!dr.IsDBNull(iValValor33)) entity.VALOVALOR33 = dr.GetDecimal(iValValor33);
            int iValValor34 = dr.GetOrdinal(this.VALOVALOR34);
            if (!dr.IsDBNull(iValValor34)) entity.VALOVALOR34 = dr.GetDecimal(iValValor34);
            int iValValor35 = dr.GetOrdinal(this.VALOVALOR35);
            if (!dr.IsDBNull(iValValor35)) entity.VALOVALOR35 = dr.GetDecimal(iValValor35);
            int iValValor36 = dr.GetOrdinal(this.VALOVALOR36);
            if (!dr.IsDBNull(iValValor36)) entity.VALOVALOR36 = dr.GetDecimal(iValValor36);
            int iValValor37 = dr.GetOrdinal(this.VALOVALOR37);
            if (!dr.IsDBNull(iValValor37)) entity.VALOVALOR37 = dr.GetDecimal(iValValor37);
            int iValValor38 = dr.GetOrdinal(this.VALOVALOR38);
            if (!dr.IsDBNull(iValValor38)) entity.VALOVALOR38 = dr.GetDecimal(iValValor38);
            int iValValor39 = dr.GetOrdinal(this.VALOVALOR39);
            if (!dr.IsDBNull(iValValor39)) entity.VALOVALOR39 = dr.GetDecimal(iValValor39);
            int iValValor40 = dr.GetOrdinal(this.VALOVALOR40);
            if (!dr.IsDBNull(iValValor40)) entity.VALOVALOR40 = dr.GetDecimal(iValValor40);
            int iValValor41 = dr.GetOrdinal(this.VALOVALOR41);
            if (!dr.IsDBNull(iValValor41)) entity.VALOVALOR41 = dr.GetDecimal(iValValor41);
            int iValValor42 = dr.GetOrdinal(this.VALOVALOR42);
            if (!dr.IsDBNull(iValValor42)) entity.VALOVALOR42 = dr.GetDecimal(iValValor42);
            int iValValor43 = dr.GetOrdinal(this.VALOVALOR43);
            if (!dr.IsDBNull(iValValor43)) entity.VALOVALOR43 = dr.GetDecimal(iValValor43);
            int iValValor44 = dr.GetOrdinal(this.VALOVALOR44);
            if (!dr.IsDBNull(iValValor44)) entity.VALOVALOR44 = dr.GetDecimal(iValValor44);
            int iValValor45 = dr.GetOrdinal(this.VALOVALOR45);
            if (!dr.IsDBNull(iValValor45)) entity.VALOVALOR45 = dr.GetDecimal(iValValor45);
            int iValValor46 = dr.GetOrdinal(this.VALOVALOR46);
            if (!dr.IsDBNull(iValValor46)) entity.VALOVALOR46 = dr.GetDecimal(iValValor46);
            int iValValor47 = dr.GetOrdinal(this.VALOVALOR47);
            if (!dr.IsDBNull(iValValor47)) entity.VALOVALOR47 = dr.GetDecimal(iValValor47);
            int iValValor48 = dr.GetOrdinal(this.VALOVALOR48);
            if (!dr.IsDBNull(iValValor48)) entity.VALOVALOR48 = dr.GetDecimal(iValValor48);
            int iValValor49 = dr.GetOrdinal(this.VALOVALOR49);
            if (!dr.IsDBNull(iValValor49)) entity.VALOVALOR49 = dr.GetDecimal(iValValor49);
            int iValValor50 = dr.GetOrdinal(this.VALOVALOR50);
            if (!dr.IsDBNull(iValValor50)) entity.VALOVALOR50 = dr.GetDecimal(iValValor50);
            int iValValor51 = dr.GetOrdinal(this.VALOVALOR51);
            if (!dr.IsDBNull(iValValor51)) entity.VALOVALOR51 = dr.GetDecimal(iValValor51);
            int iValValor52 = dr.GetOrdinal(this.VALOVALOR52);
            if (!dr.IsDBNull(iValValor52)) entity.VALOVALOR52 = dr.GetDecimal(iValValor52);
            int iValValor53 = dr.GetOrdinal(this.VALOVALOR53);
            if (!dr.IsDBNull(iValValor53)) entity.VALOVALOR53 = dr.GetDecimal(iValValor53);
            int iValValor54 = dr.GetOrdinal(this.VALOVALOR54);
            if (!dr.IsDBNull(iValValor54)) entity.VALOVALOR54 = dr.GetDecimal(iValValor54);
            int iValValor55 = dr.GetOrdinal(this.VALOVALOR55);
            if (!dr.IsDBNull(iValValor55)) entity.VALOVALOR55 = dr.GetDecimal(iValValor55);
            int iValValor56 = dr.GetOrdinal(this.VALOVALOR56);
            if (!dr.IsDBNull(iValValor56)) entity.VALOVALOR56 = dr.GetDecimal(iValValor56);
            int iValValor57 = dr.GetOrdinal(this.VALOVALOR57);
            if (!dr.IsDBNull(iValValor57)) entity.VALOVALOR57 = dr.GetDecimal(iValValor57);
            int iValValor58 = dr.GetOrdinal(this.VALOVALOR58);
            if (!dr.IsDBNull(iValValor58)) entity.VALOVALOR58 = dr.GetDecimal(iValValor58);
            int iValValor59 = dr.GetOrdinal(this.VALOVALOR59);
            if (!dr.IsDBNull(iValValor59)) entity.VALOVALOR59 = dr.GetDecimal(iValValor59);
            int iValValor60 = dr.GetOrdinal(this.VALOVALOR60);
            if (!dr.IsDBNull(iValValor60)) entity.VALOVALOR60 = dr.GetDecimal(iValValor60);
            int iValValor61 = dr.GetOrdinal(this.VALOVALOR61);
            if (!dr.IsDBNull(iValValor61)) entity.VALOVALOR61 = dr.GetDecimal(iValValor61);
            int iValValor62 = dr.GetOrdinal(this.VALOVALOR62);
            if (!dr.IsDBNull(iValValor62)) entity.VALOVALOR62 = dr.GetDecimal(iValValor62);
            int iValValor63 = dr.GetOrdinal(this.VALOVALOR63);
            if (!dr.IsDBNull(iValValor63)) entity.VALOVALOR63 = dr.GetDecimal(iValValor63);
            int iValValor64 = dr.GetOrdinal(this.VALOVALOR64);
            if (!dr.IsDBNull(iValValor64)) entity.VALOVALOR64 = dr.GetDecimal(iValValor64);
            int iValValor65 = dr.GetOrdinal(this.VALOVALOR65);
            if (!dr.IsDBNull(iValValor65)) entity.VALOVALOR65 = dr.GetDecimal(iValValor65);
            int iValValor66 = dr.GetOrdinal(this.VALOVALOR66);
            if (!dr.IsDBNull(iValValor66)) entity.VALOVALOR66 = dr.GetDecimal(iValValor66);
            int iValValor67 = dr.GetOrdinal(this.VALOVALOR67);
            if (!dr.IsDBNull(iValValor67)) entity.VALOVALOR67 = dr.GetDecimal(iValValor67);
            int iValValor68 = dr.GetOrdinal(this.VALOVALOR68);
            if (!dr.IsDBNull(iValValor68)) entity.VALOVALOR68 = dr.GetDecimal(iValValor68);
            int iValValor69 = dr.GetOrdinal(this.VALOVALOR69);
            if (!dr.IsDBNull(iValValor69)) entity.VALOVALOR69 = dr.GetDecimal(iValValor69);
            int iValValor70 = dr.GetOrdinal(this.VALOVALOR70);
            if (!dr.IsDBNull(iValValor70)) entity.VALOVALOR70 = dr.GetDecimal(iValValor70);
            int iValValor71 = dr.GetOrdinal(this.VALOVALOR71);
            if (!dr.IsDBNull(iValValor71)) entity.VALOVALOR71 = dr.GetDecimal(iValValor71);
            int iValValor72 = dr.GetOrdinal(this.VALOVALOR72);
            if (!dr.IsDBNull(iValValor72)) entity.VALOVALOR72 = dr.GetDecimal(iValValor72);
            int iValValor73 = dr.GetOrdinal(this.VALOVALOR73);
            if (!dr.IsDBNull(iValValor73)) entity.VALOVALOR73 = dr.GetDecimal(iValValor73);
            int iValValor74 = dr.GetOrdinal(this.VALOVALOR74);
            if (!dr.IsDBNull(iValValor74)) entity.VALOVALOR74 = dr.GetDecimal(iValValor74);
            int iValValor75 = dr.GetOrdinal(this.VALOVALOR75);
            if (!dr.IsDBNull(iValValor75)) entity.VALOVALOR75 = dr.GetDecimal(iValValor75);
            int iValValor76 = dr.GetOrdinal(this.VALOVALOR76);
            if (!dr.IsDBNull(iValValor76)) entity.VALOVALOR76 = dr.GetDecimal(iValValor76);
            int iValValor77 = dr.GetOrdinal(this.VALOVALOR77);
            if (!dr.IsDBNull(iValValor77)) entity.VALOVALOR77 = dr.GetDecimal(iValValor77);
            int iValValor78 = dr.GetOrdinal(this.VALOVALOR78);
            if (!dr.IsDBNull(iValValor78)) entity.VALOVALOR78 = dr.GetDecimal(iValValor78);
            int iValValor79 = dr.GetOrdinal(this.VALOVALOR79);
            if (!dr.IsDBNull(iValValor79)) entity.VALOVALOR79 = dr.GetDecimal(iValValor79);
            int iValValor80 = dr.GetOrdinal(this.VALOVALOR80);
            if (!dr.IsDBNull(iValValor80)) entity.VALOVALOR80 = dr.GetDecimal(iValValor80);
            int iValValor81 = dr.GetOrdinal(this.VALOVALOR81);
            if (!dr.IsDBNull(iValValor81)) entity.VALOVALOR81 = dr.GetDecimal(iValValor81);
            int iValValor82 = dr.GetOrdinal(this.VALOVALOR82);
            if (!dr.IsDBNull(iValValor82)) entity.VALOVALOR82 = dr.GetDecimal(iValValor82);
            int iValValor83 = dr.GetOrdinal(this.VALOVALOR83);
            if (!dr.IsDBNull(iValValor83)) entity.VALOVALOR83 = dr.GetDecimal(iValValor83);
            int iValValor84 = dr.GetOrdinal(this.VALOVALOR84);
            if (!dr.IsDBNull(iValValor84)) entity.VALOVALOR84 = dr.GetDecimal(iValValor84);
            int iValValor85 = dr.GetOrdinal(this.VALOVALOR85);
            if (!dr.IsDBNull(iValValor85)) entity.VALOVALOR85 = dr.GetDecimal(iValValor85);
            int iValValor86 = dr.GetOrdinal(this.VALOVALOR86);
            if (!dr.IsDBNull(iValValor86)) entity.VALOVALOR86 = dr.GetDecimal(iValValor86);
            int iValValor87 = dr.GetOrdinal(this.VALOVALOR87);
            if (!dr.IsDBNull(iValValor87)) entity.VALOVALOR87 = dr.GetDecimal(iValValor87);
            int iValValor88 = dr.GetOrdinal(this.VALOVALOR88);
            if (!dr.IsDBNull(iValValor88)) entity.VALOVALOR88 = dr.GetDecimal(iValValor88);
            int iValValor89 = dr.GetOrdinal(this.VALOVALOR89);
            if (!dr.IsDBNull(iValValor89)) entity.VALOVALOR89 = dr.GetDecimal(iValValor89);
            int iValValor90 = dr.GetOrdinal(this.VALOVALOR90);
            if (!dr.IsDBNull(iValValor90)) entity.VALOVALOR90 = dr.GetDecimal(iValValor90);
            int iValValor91 = dr.GetOrdinal(this.VALOVALOR91);
            if (!dr.IsDBNull(iValValor91)) entity.VALOVALOR91 = dr.GetDecimal(iValValor91);
            int iValValor92 = dr.GetOrdinal(this.VALOVALOR92);
            if (!dr.IsDBNull(iValValor92)) entity.VALOVALOR92 = dr.GetDecimal(iValValor92);
            int iValValor93 = dr.GetOrdinal(this.VALOVALOR93);
            if (!dr.IsDBNull(iValValor93)) entity.VALOVALOR93 = dr.GetDecimal(iValValor93);
            int iValValor94 = dr.GetOrdinal(this.VALOVALOR94);
            if (!dr.IsDBNull(iValValor94)) entity.VALOVALOR94 = dr.GetDecimal(iValValor94);
            int iValValor95 = dr.GetOrdinal(this.VALOVALOR95);
            if (!dr.IsDBNull(iValValor95)) entity.VALOVALOR95 = dr.GetDecimal(iValValor95);
            int iValValor96 = dr.GetOrdinal(this.VALOVALOR96);
            if (!dr.IsDBNull(iValValor96)) entity.VALOVALOR96 = dr.GetDecimal(iValValor96);

            #endregion
            return entity;
        }

        public GmmValEnergiaDTO CreateListaValoresCostoMarginal(IDataReader dr)
        {
            GmmValEnergiaDTO entity = new GmmValEnergiaDTO();

            int iMEDINTBLQNUMERO = dr.GetOrdinal(this.MEDINTBLQNUMERO);
            if (!dr.IsDBNull(iMEDINTBLQNUMERO)) entity.LECTCODI = Convert.ToInt32(dr.GetValue(iMEDINTBLQNUMERO));
            int iMEDINTFECHAINI = dr.GetOrdinal(this.MEDINTFECHAINI);
            if (!dr.IsDBNull(iMEDINTFECHAINI)) entity.MEDINTFECHAINI = dr.GetDateTime(iMEDINTFECHAINI);
            int iMEDINTH1 = dr.GetOrdinal(this.MEDINTH1);
            if (!dr.IsDBNull(iMEDINTH1)) entity.MEDINTH1 = dr.GetDecimal(iMEDINTH1);

            return entity;
        }
    }
}

