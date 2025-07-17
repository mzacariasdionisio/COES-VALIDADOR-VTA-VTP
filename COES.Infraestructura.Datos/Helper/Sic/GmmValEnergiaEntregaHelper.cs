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
    public class GmmValEnergiaEntregaHelper : HelperBase
    {
        public GmmValEnergiaEntregaHelper()
            : base(Consultas.GmmValEnergiaEntrega)
        {

        }

        #region Mapeo de Campos
        public string VALENECODI = "VALENECODI";
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
        public string VALENEVALENEVAL1 = "VALENEVALENEVAL1";
        public string VALENEVALENEVAL2 = "VALENEVALENEVAL2";
        public string VALENEVALENEVAL3 = "VALENEVALENEVAL3";
        public string VALENEVALENEVAL4 = "VALENEVALENEVAL4";
        public string VALENEVALENEVAL5 = "VALENEVALENEVAL5";
        public string VALENEVALENEVAL6 = "VALENEVALENEVAL6";
        public string VALENEVALENEVAL7 = "VALENEVALENEVAL7";
        public string VALENEVALENEVAL8 = "VALENEVALENEVAL8";
        public string VALENEVALENEVAL9 = "VALENEVALENEVAL9";
        public string VALENEVALENEVAL10 = "VALENEVALENEVAL10";
        public string VALENEVALENEVAL11 = "VALENEVALENEVAL11";
        public string VALENEVALENEVAL12 = "VALENEVALENEVAL12";
        public string VALENEVALENEVAL13 = "VALENEVALENEVAL13";
        public string VALENEVALENEVAL14 = "VALENEVALENEVAL14";
        public string VALENEVALENEVAL15 = "VALENEVALENEVAL15";
        public string VALENEVALENEVAL16 = "VALENEVALENEVAL16";
        public string VALENEVALENEVAL17 = "VALENEVALENEVAL17";
        public string VALENEVALENEVAL18 = "VALENEVALENEVAL18";
        public string VALENEVALENEVAL19 = "VALENEVALENEVAL19";
        public string VALENEVALENEVAL20 = "VALENEVALENEVAL20";
        public string VALENEVALENEVAL21 = "VALENEVALENEVAL21";
        public string VALENEVALENEVAL22 = "VALENEVALENEVAL22";
        public string VALENEVALENEVAL23 = "VALENEVALENEVAL23";
        public string VALENEVALENEVAL24 = "VALENEVALENEVAL24";
        public string VALENEVALENEVAL25 = "VALENEVALENEVAL25";
        public string VALENEVALENEVAL26 = "VALENEVALENEVAL26";
        public string VALENEVALENEVAL27 = "VALENEVALENEVAL27";
        public string VALENEVALENEVAL28 = "VALENEVALENEVAL28";
        public string VALENEVALENEVAL29 = "VALENEVALENEVAL29";
        public string VALENEVALENEVAL30 = "VALENEVALENEVAL30";
        public string VALENEVALENEVAL31 = "VALENEVALENEVAL31";
        public string VALENEVALENEVAL32 = "VALENEVALENEVAL32";
        public string VALENEVALENEVAL33 = "VALENEVALENEVAL33";
        public string VALENEVALENEVAL34 = "VALENEVALENEVAL34";
        public string VALENEVALENEVAL35 = "VALENEVALENEVAL35";
        public string VALENEVALENEVAL36 = "VALENEVALENEVAL36";
        public string VALENEVALENEVAL37 = "VALENEVALENEVAL37";
        public string VALENEVALENEVAL38 = "VALENEVALENEVAL38";
        public string VALENEVALENEVAL39 = "VALENEVALENEVAL39";
        public string VALENEVALENEVAL40 = "VALENEVALENEVAL40";
        public string VALENEVALENEVAL41 = "VALENEVALENEVAL41";
        public string VALENEVALENEVAL42 = "VALENEVALENEVAL42";
        public string VALENEVALENEVAL43 = "VALENEVALENEVAL43";
        public string VALENEVALENEVAL44 = "VALENEVALENEVAL44";
        public string VALENEVALENEVAL45 = "VALENEVALENEVAL45";
        public string VALENEVALENEVAL46 = "VALENEVALENEVAL46";
        public string VALENEVALENEVAL47 = "VALENEVALENEVAL47";
        public string VALENEVALENEVAL48 = "VALENEVALENEVAL48";
        public string VALENEVALENEVAL49 = "VALENEVALENEVAL49";
        public string VALENEVALENEVAL50 = "VALENEVALENEVAL50";
        public string VALENEVALENEVAL51 = "VALENEVALENEVAL51";
        public string VALENEVALENEVAL52 = "VALENEVALENEVAL52";
        public string VALENEVALENEVAL53 = "VALENEVALENEVAL53";
        public string VALENEVALENEVAL54 = "VALENEVALENEVAL54";
        public string VALENEVALENEVAL55 = "VALENEVALENEVAL55";
        public string VALENEVALENEVAL56 = "VALENEVALENEVAL56";
        public string VALENEVALENEVAL57 = "VALENEVALENEVAL57";
        public string VALENEVALENEVAL58 = "VALENEVALENEVAL58";
        public string VALENEVALENEVAL59 = "VALENEVALENEVAL59";
        public string VALENEVALENEVAL60 = "VALENEVALENEVAL60";
        public string VALENEVALENEVAL61 = "VALENEVALENEVAL61";
        public string VALENEVALENEVAL62 = "VALENEVALENEVAL62";
        public string VALENEVALENEVAL63 = "VALENEVALENEVAL63";
        public string VALENEVALENEVAL64 = "VALENEVALENEVAL64";
        public string VALENEVALENEVAL65 = "VALENEVALENEVAL65";
        public string VALENEVALENEVAL66 = "VALENEVALENEVAL66";
        public string VALENEVALENEVAL67 = "VALENEVALENEVAL67";
        public string VALENEVALENEVAL68 = "VALENEVALENEVAL68";
        public string VALENEVALENEVAL69 = "VALENEVALENEVAL69";
        public string VALENEVALENEVAL70 = "VALENEVALENEVAL70";
        public string VALENEVALENEVAL71 = "VALENEVALENEVAL71";
        public string VALENEVALENEVAL72 = "VALENEVALENEVAL72";
        public string VALENEVALENEVAL73 = "VALENEVALENEVAL73";
        public string VALENEVALENEVAL74 = "VALENEVALENEVAL74";
        public string VALENEVALENEVAL75 = "VALENEVALENEVAL75";
        public string VALENEVALENEVAL76 = "VALENEVALENEVAL76";
        public string VALENEVALENEVAL77 = "VALENEVALENEVAL77";
        public string VALENEVALENEVAL78 = "VALENEVALENEVAL78";
        public string VALENEVALENEVAL79 = "VALENEVALENEVAL79";
        public string VALENEVALENEVAL80 = "VALENEVALENEVAL80";
        public string VALENEVALENEVAL81 = "VALENEVALENEVAL81";
        public string VALENEVALENEVAL82 = "VALENEVALENEVAL82";
        public string VALENEVALENEVAL83 = "VALENEVALENEVAL83";
        public string VALENEVALENEVAL84 = "VALENEVALENEVAL84";
        public string VALENEVALENEVAL85 = "VALENEVALENEVAL85";
        public string VALENEVALENEVAL86 = "VALENEVALENEVAL86";
        public string VALENEVALENEVAL87 = "VALENEVALENEVAL87";
        public string VALENEVALENEVAL88 = "VALENEVALENEVAL88";
        public string VALENEVALENEVAL89 = "VALENEVALENEVAL89";
        public string VALENEVALENEVAL90 = "VALENEVALENEVAL90";
        public string VALENEVALENEVAL91 = "VALENEVALENEVAL91";
        public string VALENEVALENEVAL92 = "VALENEVALENEVAL92";
        public string VALENEVALENEVAL93 = "VALENEVALENEVAL93";
        public string VALENEVALENEVAL94 = "VALENEVALENEVAL94";
        public string VALENEVALENEVAL95 = "VALENEVALENEVAL95";
        public string VALENEVALENEVAL96 = "VALENEVALENEVAL96";

        public string VALENEVALENEVALCM1 = "VALENEVALENEVALCM1";
        public string VALENEVALENEVALCM2 = "VALENEVALENEVALCM2";
        public string VALENEVALENEVALCM3 = "VALENEVALENEVALCM3";
        public string VALENEVALENEVALCM4 = "VALENEVALENEVALCM4";
        public string VALENEVALENEVALCM5 = "VALENEVALENEVALCM5";
        public string VALENEVALENEVALCM6 = "VALENEVALENEVALCM6";
        public string VALENEVALENEVALCM7 = "VALENEVALENEVALCM7";
        public string VALENEVALENEVALCM8 = "VALENEVALENEVALCM8";
        public string VALENEVALENEVALCM9 = "VALENEVALENEVALCM9";
        public string VALENEVALENEVALCM10 = "VALENEVALENEVALCM10";
        public string VALENEVALENEVALCM11 = "VALENEVALENEVALCM11";
        public string VALENEVALENEVALCM12 = "VALENEVALENEVALCM12";
        public string VALENEVALENEVALCM13 = "VALENEVALENEVALCM13";
        public string VALENEVALENEVALCM14 = "VALENEVALENEVALCM14";
        public string VALENEVALENEVALCM15 = "VALENEVALENEVALCM15";
        public string VALENEVALENEVALCM16 = "VALENEVALENEVALCM16";
        public string VALENEVALENEVALCM17 = "VALENEVALENEVALCM17";
        public string VALENEVALENEVALCM18 = "VALENEVALENEVALCM18";
        public string VALENEVALENEVALCM19 = "VALENEVALENEVALCM19";
        public string VALENEVALENEVALCM20 = "VALENEVALENEVALCM20";
        public string VALENEVALENEVALCM21 = "VALENEVALENEVALCM21";
        public string VALENEVALENEVALCM22 = "VALENEVALENEVALCM22";
        public string VALENEVALENEVALCM23 = "VALENEVALENEVALCM23";
        public string VALENEVALENEVALCM24 = "VALENEVALENEVALCM24";
        public string VALENEVALENEVALCM25 = "VALENEVALENEVALCM25";
        public string VALENEVALENEVALCM26 = "VALENEVALENEVALCM26";
        public string VALENEVALENEVALCM27 = "VALENEVALENEVALCM27";
        public string VALENEVALENEVALCM28 = "VALENEVALENEVALCM28";
        public string VALENEVALENEVALCM29 = "VALENEVALENEVALCM29";
        public string VALENEVALENEVALCM30 = "VALENEVALENEVALCM30";
        public string VALENEVALENEVALCM31 = "VALENEVALENEVALCM31";
        public string VALENEVALENEVALCM32 = "VALENEVALENEVALCM32";
        public string VALENEVALENEVALCM33 = "VALENEVALENEVALCM33";
        public string VALENEVALENEVALCM34 = "VALENEVALENEVALCM34";
        public string VALENEVALENEVALCM35 = "VALENEVALENEVALCM35";
        public string VALENEVALENEVALCM36 = "VALENEVALENEVALCM36";
        public string VALENEVALENEVALCM37 = "VALENEVALENEVALCM37";
        public string VALENEVALENEVALCM38 = "VALENEVALENEVALCM38";
        public string VALENEVALENEVALCM39 = "VALENEVALENEVALCM39";
        public string VALENEVALENEVALCM40 = "VALENEVALENEVALCM40";
        public string VALENEVALENEVALCM41 = "VALENEVALENEVALCM41";
        public string VALENEVALENEVALCM42 = "VALENEVALENEVALCM42";
        public string VALENEVALENEVALCM43 = "VALENEVALENEVALCM43";
        public string VALENEVALENEVALCM44 = "VALENEVALENEVALCM44";
        public string VALENEVALENEVALCM45 = "VALENEVALENEVALCM45";
        public string VALENEVALENEVALCM46 = "VALENEVALENEVALCM46";
        public string VALENEVALENEVALCM47 = "VALENEVALENEVALCM47";
        public string VALENEVALENEVALCM48 = "VALENEVALENEVALCM48";
        public string VALENEVALENEVALCM49 = "VALENEVALENEVALCM49";
        public string VALENEVALENEVALCM50 = "VALENEVALENEVALCM50";
        public string VALENEVALENEVALCM51 = "VALENEVALENEVALCM51";
        public string VALENEVALENEVALCM52 = "VALENEVALENEVALCM52";
        public string VALENEVALENEVALCM53 = "VALENEVALENEVALCM53";
        public string VALENEVALENEVALCM54 = "VALENEVALENEVALCM54";
        public string VALENEVALENEVALCM55 = "VALENEVALENEVALCM55";
        public string VALENEVALENEVALCM56 = "VALENEVALENEVALCM56";
        public string VALENEVALENEVALCM57 = "VALENEVALENEVALCM57";
        public string VALENEVALENEVALCM58 = "VALENEVALENEVALCM58";
        public string VALENEVALENEVALCM59 = "VALENEVALENEVALCM59";
        public string VALENEVALENEVALCM60 = "VALENEVALENEVALCM60";
        public string VALENEVALENEVALCM61 = "VALENEVALENEVALCM61";
        public string VALENEVALENEVALCM62 = "VALENEVALENEVALCM62";
        public string VALENEVALENEVALCM63 = "VALENEVALENEVALCM63";
        public string VALENEVALENEVALCM64 = "VALENEVALENEVALCM64";
        public string VALENEVALENEVALCM65 = "VALENEVALENEVALCM65";
        public string VALENEVALENEVALCM66 = "VALENEVALENEVALCM66";
        public string VALENEVALENEVALCM67 = "VALENEVALENEVALCM67";
        public string VALENEVALENEVALCM68 = "VALENEVALENEVALCM68";
        public string VALENEVALENEVALCM69 = "VALENEVALENEVALCM69";
        public string VALENEVALENEVALCM70 = "VALENEVALENEVALCM70";
        public string VALENEVALENEVALCM71 = "VALENEVALENEVALCM71";
        public string VALENEVALENEVALCM72 = "VALENEVALENEVALCM72";
        public string VALENEVALENEVALCM73 = "VALENEVALENEVALCM73";
        public string VALENEVALENEVALCM74 = "VALENEVALENEVALCM74";
        public string VALENEVALENEVALCM75 = "VALENEVALENEVALCM75";
        public string VALENEVALENEVALCM76 = "VALENEVALENEVALCM76";
        public string VALENEVALENEVALCM77 = "VALENEVALENEVALCM77";
        public string VALENEVALENEVALCM78 = "VALENEVALENEVALCM78";
        public string VALENEVALENEVALCM79 = "VALENEVALENEVALCM79";
        public string VALENEVALENEVALCM80 = "VALENEVALENEVALCM80";
        public string VALENEVALENEVALCM81 = "VALENEVALENEVALCM81";
        public string VALENEVALENEVALCM82 = "VALENEVALENEVALCM82";
        public string VALENEVALENEVALCM83 = "VALENEVALENEVALCM83";
        public string VALENEVALENEVALCM84 = "VALENEVALENEVALCM84";
        public string VALENEVALENEVALCM85 = "VALENEVALENEVALCM85";
        public string VALENEVALENEVALCM86 = "VALENEVALENEVALCM86";
        public string VALENEVALENEVALCM87 = "VALENEVALENEVALCM87";
        public string VALENEVALENEVALCM88 = "VALENEVALENEVALCM88";
        public string VALENEVALENEVALCM89 = "VALENEVALENEVALCM89";
        public string VALENEVALENEVALCM90 = "VALENEVALENEVALCM90";
        public string VALENEVALENEVALCM91 = "VALENEVALENEVALCM91";
        public string VALENEVALENEVALCM92 = "VALENEVALENEVALCM92";
        public string VALENEVALENEVALCM93 = "VALENEVALENEVALCM93";
        public string VALENEVALENEVALCM94 = "VALENEVALENEVALCM94";
        public string VALENEVALENEVALCM95 = "VALENEVALENEVALCM95";
        public string VALENEVALENEVALCM96 = "VALENEVALENEVALCM96";

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

        public GmmValEnergiaEntregaDTO CreateListaValores(IDataReader dr)
        {
            GmmValEnergiaEntregaDTO entity = new GmmValEnergiaEntregaDTO();

            #region Lista Valores de Energia
            int iLecturaCodi = dr.GetOrdinal(this.LECTCODI);
            if (!dr.IsDBNull(iLecturaCodi)) entity.LECTCODI = Convert.ToInt32(dr.GetValue(iLecturaCodi));
            int iEMedifecha = dr.GetOrdinal(this.MEDIFECHA);
            if (!dr.IsDBNull(iEMedifecha)) entity.MEDIFECHA = dr.GetDateTime(iEMedifecha);

            int iVALANIO = dr.GetOrdinal(this.VALANIO);
            if (!dr.IsDBNull(iVALANIO)) entity.VALANIO = dr.GetInt32(iVALANIO);
            int iVALMES = dr.GetOrdinal(this.VALMES);
            if (!dr.IsDBNull(iVALMES)) entity.VALMES = dr.GetInt32(iVALMES);

            int iValValor1 = dr.GetOrdinal(this.VALENEVALENEVAL1);
            if (!dr.IsDBNull(iValValor1)) entity.VALENEVALENEVAL1 = dr.GetDecimal(iValValor1);
            int iValValor2 = dr.GetOrdinal(this.VALENEVALENEVAL2);
            if (!dr.IsDBNull(iValValor2)) entity.VALENEVALENEVAL2 = dr.GetDecimal(iValValor2);
            int iValValor3 = dr.GetOrdinal(this.VALENEVALENEVAL3);
            if (!dr.IsDBNull(iValValor3)) entity.VALENEVALENEVAL3 = dr.GetDecimal(iValValor3);
            int iValValor4 = dr.GetOrdinal(this.VALENEVALENEVAL4);
            if (!dr.IsDBNull(iValValor4)) entity.VALENEVALENEVAL4 = dr.GetDecimal(iValValor4);
            int iValValor5 = dr.GetOrdinal(this.VALENEVALENEVAL5);
            if (!dr.IsDBNull(iValValor5)) entity.VALENEVALENEVAL5 = dr.GetDecimal(iValValor5);
            int iValValor6 = dr.GetOrdinal(this.VALENEVALENEVAL6);
            if (!dr.IsDBNull(iValValor6)) entity.VALENEVALENEVAL6 = dr.GetDecimal(iValValor6);
            int iValValor7 = dr.GetOrdinal(this.VALENEVALENEVAL7);
            if (!dr.IsDBNull(iValValor7)) entity.VALENEVALENEVAL7 = dr.GetDecimal(iValValor7);
            int iValValor8 = dr.GetOrdinal(this.VALENEVALENEVAL8);
            if (!dr.IsDBNull(iValValor8)) entity.VALENEVALENEVAL8 = dr.GetDecimal(iValValor8);
            int iValValor9 = dr.GetOrdinal(this.VALENEVALENEVAL9);
            if (!dr.IsDBNull(iValValor9)) entity.VALENEVALENEVAL9 = dr.GetDecimal(iValValor9);
            int iValValor10 = dr.GetOrdinal(this.VALENEVALENEVAL10);
            if (!dr.IsDBNull(iValValor10)) entity.VALENEVALENEVAL10 = dr.GetDecimal(iValValor10);
            int iValValor11 = dr.GetOrdinal(this.VALENEVALENEVAL11);
            if (!dr.IsDBNull(iValValor11)) entity.VALENEVALENEVAL11 = dr.GetDecimal(iValValor11);
            int iValValor12 = dr.GetOrdinal(this.VALENEVALENEVAL12);
            if (!dr.IsDBNull(iValValor12)) entity.VALENEVALENEVAL12 = dr.GetDecimal(iValValor12);
            int iValValor13 = dr.GetOrdinal(this.VALENEVALENEVAL13);
            if (!dr.IsDBNull(iValValor13)) entity.VALENEVALENEVAL13 = dr.GetDecimal(iValValor13);
            int iValValor14 = dr.GetOrdinal(this.VALENEVALENEVAL14);
            if (!dr.IsDBNull(iValValor14)) entity.VALENEVALENEVAL14 = dr.GetDecimal(iValValor14);
            int iValValor15 = dr.GetOrdinal(this.VALENEVALENEVAL15);
            if (!dr.IsDBNull(iValValor15)) entity.VALENEVALENEVAL15 = dr.GetDecimal(iValValor15);
            int iValValor16 = dr.GetOrdinal(this.VALENEVALENEVAL16);
            if (!dr.IsDBNull(iValValor16)) entity.VALENEVALENEVAL16 = dr.GetDecimal(iValValor16);
            int iValValor17 = dr.GetOrdinal(this.VALENEVALENEVAL17);
            if (!dr.IsDBNull(iValValor17)) entity.VALENEVALENEVAL17 = dr.GetDecimal(iValValor17);
            int iValValor18 = dr.GetOrdinal(this.VALENEVALENEVAL18);
            if (!dr.IsDBNull(iValValor18)) entity.VALENEVALENEVAL18 = dr.GetDecimal(iValValor18);
            int iValValor19 = dr.GetOrdinal(this.VALENEVALENEVAL19);
            if (!dr.IsDBNull(iValValor19)) entity.VALENEVALENEVAL19 = dr.GetDecimal(iValValor19);
            int iValValor20 = dr.GetOrdinal(this.VALENEVALENEVAL20);
            if (!dr.IsDBNull(iValValor20)) entity.VALENEVALENEVAL20 = dr.GetDecimal(iValValor20);
            int iValValor21 = dr.GetOrdinal(this.VALENEVALENEVAL21);
            if (!dr.IsDBNull(iValValor21)) entity.VALENEVALENEVAL21 = dr.GetDecimal(iValValor21);
            int iValValor22 = dr.GetOrdinal(this.VALENEVALENEVAL22);
            if (!dr.IsDBNull(iValValor22)) entity.VALENEVALENEVAL22 = dr.GetDecimal(iValValor22);
            int iValValor23 = dr.GetOrdinal(this.VALENEVALENEVAL23);
            if (!dr.IsDBNull(iValValor23)) entity.VALENEVALENEVAL23 = dr.GetDecimal(iValValor23);
            int iValValor24 = dr.GetOrdinal(this.VALENEVALENEVAL24);
            if (!dr.IsDBNull(iValValor24)) entity.VALENEVALENEVAL24 = dr.GetDecimal(iValValor24);
            int iValValor25 = dr.GetOrdinal(this.VALENEVALENEVAL25);
            if (!dr.IsDBNull(iValValor25)) entity.VALENEVALENEVAL25 = dr.GetDecimal(iValValor25);
            int iValValor26 = dr.GetOrdinal(this.VALENEVALENEVAL26);
            if (!dr.IsDBNull(iValValor26)) entity.VALENEVALENEVAL26 = dr.GetDecimal(iValValor26);
            int iValValor27 = dr.GetOrdinal(this.VALENEVALENEVAL27);
            if (!dr.IsDBNull(iValValor27)) entity.VALENEVALENEVAL27 = dr.GetDecimal(iValValor27);
            int iValValor28 = dr.GetOrdinal(this.VALENEVALENEVAL28);
            if (!dr.IsDBNull(iValValor28)) entity.VALENEVALENEVAL28 = dr.GetDecimal(iValValor28);
            int iValValor29 = dr.GetOrdinal(this.VALENEVALENEVAL29);
            if (!dr.IsDBNull(iValValor29)) entity.VALENEVALENEVAL29 = dr.GetDecimal(iValValor29);
            int iValValor30 = dr.GetOrdinal(this.VALENEVALENEVAL30);
            if (!dr.IsDBNull(iValValor30)) entity.VALENEVALENEVAL30 = dr.GetDecimal(iValValor30);
            int iValValor31 = dr.GetOrdinal(this.VALENEVALENEVAL31);
            if (!dr.IsDBNull(iValValor31)) entity.VALENEVALENEVAL31 = dr.GetDecimal(iValValor31);
            int iValValor32 = dr.GetOrdinal(this.VALENEVALENEVAL32);
            if (!dr.IsDBNull(iValValor32)) entity.VALENEVALENEVAL32 = dr.GetDecimal(iValValor32);
            int iValValor33 = dr.GetOrdinal(this.VALENEVALENEVAL33);
            if (!dr.IsDBNull(iValValor33)) entity.VALENEVALENEVAL33 = dr.GetDecimal(iValValor33);
            int iValValor34 = dr.GetOrdinal(this.VALENEVALENEVAL34);
            if (!dr.IsDBNull(iValValor34)) entity.VALENEVALENEVAL34 = dr.GetDecimal(iValValor34);
            int iValValor35 = dr.GetOrdinal(this.VALENEVALENEVAL35);
            if (!dr.IsDBNull(iValValor35)) entity.VALENEVALENEVAL35 = dr.GetDecimal(iValValor35);
            int iValValor36 = dr.GetOrdinal(this.VALENEVALENEVAL36);
            if (!dr.IsDBNull(iValValor36)) entity.VALENEVALENEVAL36 = dr.GetDecimal(iValValor36);
            int iValValor37 = dr.GetOrdinal(this.VALENEVALENEVAL37);
            if (!dr.IsDBNull(iValValor37)) entity.VALENEVALENEVAL37 = dr.GetDecimal(iValValor37);
            int iValValor38 = dr.GetOrdinal(this.VALENEVALENEVAL38);
            if (!dr.IsDBNull(iValValor38)) entity.VALENEVALENEVAL38 = dr.GetDecimal(iValValor38);
            int iValValor39 = dr.GetOrdinal(this.VALENEVALENEVAL39);
            if (!dr.IsDBNull(iValValor39)) entity.VALENEVALENEVAL39 = dr.GetDecimal(iValValor39);
            int iValValor40 = dr.GetOrdinal(this.VALENEVALENEVAL40);
            if (!dr.IsDBNull(iValValor40)) entity.VALENEVALENEVAL40 = dr.GetDecimal(iValValor40);
            int iValValor41 = dr.GetOrdinal(this.VALENEVALENEVAL41);
            if (!dr.IsDBNull(iValValor41)) entity.VALENEVALENEVAL41 = dr.GetDecimal(iValValor41);
            int iValValor42 = dr.GetOrdinal(this.VALENEVALENEVAL42);
            if (!dr.IsDBNull(iValValor42)) entity.VALENEVALENEVAL42 = dr.GetDecimal(iValValor42);
            int iValValor43 = dr.GetOrdinal(this.VALENEVALENEVAL43);
            if (!dr.IsDBNull(iValValor43)) entity.VALENEVALENEVAL43 = dr.GetDecimal(iValValor43);
            int iValValor44 = dr.GetOrdinal(this.VALENEVALENEVAL44);
            if (!dr.IsDBNull(iValValor44)) entity.VALENEVALENEVAL44 = dr.GetDecimal(iValValor44);
            int iValValor45 = dr.GetOrdinal(this.VALENEVALENEVAL45);
            if (!dr.IsDBNull(iValValor45)) entity.VALENEVALENEVAL45 = dr.GetDecimal(iValValor45);
            int iValValor46 = dr.GetOrdinal(this.VALENEVALENEVAL46);
            if (!dr.IsDBNull(iValValor46)) entity.VALENEVALENEVAL46 = dr.GetDecimal(iValValor46);
            int iValValor47 = dr.GetOrdinal(this.VALENEVALENEVAL47);
            if (!dr.IsDBNull(iValValor47)) entity.VALENEVALENEVAL47 = dr.GetDecimal(iValValor47);
            int iValValor48 = dr.GetOrdinal(this.VALENEVALENEVAL48);
            if (!dr.IsDBNull(iValValor48)) entity.VALENEVALENEVAL48 = dr.GetDecimal(iValValor48);
            int iValValor49 = dr.GetOrdinal(this.VALENEVALENEVAL49);
            if (!dr.IsDBNull(iValValor49)) entity.VALENEVALENEVAL49 = dr.GetDecimal(iValValor49);
            int iValValor50 = dr.GetOrdinal(this.VALENEVALENEVAL50);
            if (!dr.IsDBNull(iValValor50)) entity.VALENEVALENEVAL50 = dr.GetDecimal(iValValor50);
            int iValValor51 = dr.GetOrdinal(this.VALENEVALENEVAL51);
            if (!dr.IsDBNull(iValValor51)) entity.VALENEVALENEVAL51 = dr.GetDecimal(iValValor51);
            int iValValor52 = dr.GetOrdinal(this.VALENEVALENEVAL52);
            if (!dr.IsDBNull(iValValor52)) entity.VALENEVALENEVAL52 = dr.GetDecimal(iValValor52);
            int iValValor53 = dr.GetOrdinal(this.VALENEVALENEVAL53);
            if (!dr.IsDBNull(iValValor53)) entity.VALENEVALENEVAL53 = dr.GetDecimal(iValValor53);
            int iValValor54 = dr.GetOrdinal(this.VALENEVALENEVAL54);
            if (!dr.IsDBNull(iValValor54)) entity.VALENEVALENEVAL54 = dr.GetDecimal(iValValor54);
            int iValValor55 = dr.GetOrdinal(this.VALENEVALENEVAL55);
            if (!dr.IsDBNull(iValValor55)) entity.VALENEVALENEVAL55 = dr.GetDecimal(iValValor55);
            int iValValor56 = dr.GetOrdinal(this.VALENEVALENEVAL56);
            if (!dr.IsDBNull(iValValor56)) entity.VALENEVALENEVAL56 = dr.GetDecimal(iValValor56);
            int iValValor57 = dr.GetOrdinal(this.VALENEVALENEVAL57);
            if (!dr.IsDBNull(iValValor57)) entity.VALENEVALENEVAL57 = dr.GetDecimal(iValValor57);
            int iValValor58 = dr.GetOrdinal(this.VALENEVALENEVAL58);
            if (!dr.IsDBNull(iValValor58)) entity.VALENEVALENEVAL58 = dr.GetDecimal(iValValor58);
            int iValValor59 = dr.GetOrdinal(this.VALENEVALENEVAL59);
            if (!dr.IsDBNull(iValValor59)) entity.VALENEVALENEVAL59 = dr.GetDecimal(iValValor59);
            int iValValor60 = dr.GetOrdinal(this.VALENEVALENEVAL60);
            if (!dr.IsDBNull(iValValor60)) entity.VALENEVALENEVAL60 = dr.GetDecimal(iValValor60);
            int iValValor61 = dr.GetOrdinal(this.VALENEVALENEVAL61);
            if (!dr.IsDBNull(iValValor61)) entity.VALENEVALENEVAL61 = dr.GetDecimal(iValValor61);
            int iValValor62 = dr.GetOrdinal(this.VALENEVALENEVAL62);
            if (!dr.IsDBNull(iValValor62)) entity.VALENEVALENEVAL62 = dr.GetDecimal(iValValor62);
            int iValValor63 = dr.GetOrdinal(this.VALENEVALENEVAL63);
            if (!dr.IsDBNull(iValValor63)) entity.VALENEVALENEVAL63 = dr.GetDecimal(iValValor63);
            int iValValor64 = dr.GetOrdinal(this.VALENEVALENEVAL64);
            if (!dr.IsDBNull(iValValor64)) entity.VALENEVALENEVAL64 = dr.GetDecimal(iValValor64);
            int iValValor65 = dr.GetOrdinal(this.VALENEVALENEVAL65);
            if (!dr.IsDBNull(iValValor65)) entity.VALENEVALENEVAL65 = dr.GetDecimal(iValValor65);
            int iValValor66 = dr.GetOrdinal(this.VALENEVALENEVAL66);
            if (!dr.IsDBNull(iValValor66)) entity.VALENEVALENEVAL66 = dr.GetDecimal(iValValor66);
            int iValValor67 = dr.GetOrdinal(this.VALENEVALENEVAL67);
            if (!dr.IsDBNull(iValValor67)) entity.VALENEVALENEVAL67 = dr.GetDecimal(iValValor67);
            int iValValor68 = dr.GetOrdinal(this.VALENEVALENEVAL68);
            if (!dr.IsDBNull(iValValor68)) entity.VALENEVALENEVAL68 = dr.GetDecimal(iValValor68);
            int iValValor69 = dr.GetOrdinal(this.VALENEVALENEVAL69);
            if (!dr.IsDBNull(iValValor69)) entity.VALENEVALENEVAL69 = dr.GetDecimal(iValValor69);
            int iValValor70 = dr.GetOrdinal(this.VALENEVALENEVAL70);
            if (!dr.IsDBNull(iValValor70)) entity.VALENEVALENEVAL70 = dr.GetDecimal(iValValor70);
            int iValValor71 = dr.GetOrdinal(this.VALENEVALENEVAL71);
            if (!dr.IsDBNull(iValValor71)) entity.VALENEVALENEVAL71 = dr.GetDecimal(iValValor71);
            int iValValor72 = dr.GetOrdinal(this.VALENEVALENEVAL72);
            if (!dr.IsDBNull(iValValor72)) entity.VALENEVALENEVAL72 = dr.GetDecimal(iValValor72);
            int iValValor73 = dr.GetOrdinal(this.VALENEVALENEVAL73);
            if (!dr.IsDBNull(iValValor73)) entity.VALENEVALENEVAL73 = dr.GetDecimal(iValValor73);
            int iValValor74 = dr.GetOrdinal(this.VALENEVALENEVAL74);
            if (!dr.IsDBNull(iValValor74)) entity.VALENEVALENEVAL74 = dr.GetDecimal(iValValor74);
            int iValValor75 = dr.GetOrdinal(this.VALENEVALENEVAL75);
            if (!dr.IsDBNull(iValValor75)) entity.VALENEVALENEVAL75 = dr.GetDecimal(iValValor75);
            int iValValor76 = dr.GetOrdinal(this.VALENEVALENEVAL76);
            if (!dr.IsDBNull(iValValor76)) entity.VALENEVALENEVAL76 = dr.GetDecimal(iValValor76);
            int iValValor77 = dr.GetOrdinal(this.VALENEVALENEVAL77);
            if (!dr.IsDBNull(iValValor77)) entity.VALENEVALENEVAL77 = dr.GetDecimal(iValValor77);
            int iValValor78 = dr.GetOrdinal(this.VALENEVALENEVAL78);
            if (!dr.IsDBNull(iValValor78)) entity.VALENEVALENEVAL78 = dr.GetDecimal(iValValor78);
            int iValValor79 = dr.GetOrdinal(this.VALENEVALENEVAL79);
            if (!dr.IsDBNull(iValValor79)) entity.VALENEVALENEVAL79 = dr.GetDecimal(iValValor79);
            int iValValor80 = dr.GetOrdinal(this.VALENEVALENEVAL80);
            if (!dr.IsDBNull(iValValor80)) entity.VALENEVALENEVAL80 = dr.GetDecimal(iValValor80);
            int iValValor81 = dr.GetOrdinal(this.VALENEVALENEVAL81);
            if (!dr.IsDBNull(iValValor81)) entity.VALENEVALENEVAL81 = dr.GetDecimal(iValValor81);
            int iValValor82 = dr.GetOrdinal(this.VALENEVALENEVAL82);
            if (!dr.IsDBNull(iValValor82)) entity.VALENEVALENEVAL82 = dr.GetDecimal(iValValor82);
            int iValValor83 = dr.GetOrdinal(this.VALENEVALENEVAL83);
            if (!dr.IsDBNull(iValValor83)) entity.VALENEVALENEVAL83 = dr.GetDecimal(iValValor83);
            int iValValor84 = dr.GetOrdinal(this.VALENEVALENEVAL84);
            if (!dr.IsDBNull(iValValor84)) entity.VALENEVALENEVAL84 = dr.GetDecimal(iValValor84);
            int iValValor85 = dr.GetOrdinal(this.VALENEVALENEVAL85);
            if (!dr.IsDBNull(iValValor85)) entity.VALENEVALENEVAL85 = dr.GetDecimal(iValValor85);
            int iValValor86 = dr.GetOrdinal(this.VALENEVALENEVAL86);
            if (!dr.IsDBNull(iValValor86)) entity.VALENEVALENEVAL86 = dr.GetDecimal(iValValor86);
            int iValValor87 = dr.GetOrdinal(this.VALENEVALENEVAL87);
            if (!dr.IsDBNull(iValValor87)) entity.VALENEVALENEVAL87 = dr.GetDecimal(iValValor87);
            int iValValor88 = dr.GetOrdinal(this.VALENEVALENEVAL88);
            if (!dr.IsDBNull(iValValor88)) entity.VALENEVALENEVAL88 = dr.GetDecimal(iValValor88);
            int iValValor89 = dr.GetOrdinal(this.VALENEVALENEVAL89);
            if (!dr.IsDBNull(iValValor89)) entity.VALENEVALENEVAL89 = dr.GetDecimal(iValValor89);
            int iValValor90 = dr.GetOrdinal(this.VALENEVALENEVAL90);
            if (!dr.IsDBNull(iValValor90)) entity.VALENEVALENEVAL90 = dr.GetDecimal(iValValor90);
            int iValValor91 = dr.GetOrdinal(this.VALENEVALENEVAL91);
            if (!dr.IsDBNull(iValValor91)) entity.VALENEVALENEVAL91 = dr.GetDecimal(iValValor91);
            int iValValor92 = dr.GetOrdinal(this.VALENEVALENEVAL92);
            if (!dr.IsDBNull(iValValor92)) entity.VALENEVALENEVAL92 = dr.GetDecimal(iValValor92);
            int iValValor93 = dr.GetOrdinal(this.VALENEVALENEVAL93);
            if (!dr.IsDBNull(iValValor93)) entity.VALENEVALENEVAL93 = dr.GetDecimal(iValValor93);
            int iValValor94 = dr.GetOrdinal(this.VALENEVALENEVAL94);
            if (!dr.IsDBNull(iValValor94)) entity.VALENEVALENEVAL94 = dr.GetDecimal(iValValor94);
            int iValValor95 = dr.GetOrdinal(this.VALENEVALENEVAL95);
            if (!dr.IsDBNull(iValValor95)) entity.VALENEVALENEVAL95 = dr.GetDecimal(iValValor95);
            int iValValor96 = dr.GetOrdinal(this.VALENEVALENEVAL96);
            if (!dr.IsDBNull(iValValor96)) entity.VALENEVALENEVAL96 = dr.GetDecimal(iValValor96);

            #endregion
            return entity;
        }

        public GmmValEnergiaEntregaDTO CreateListaValoresCostoMarginal(IDataReader dr)
        {
            GmmValEnergiaEntregaDTO entity = new GmmValEnergiaEntregaDTO();

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

