using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    public class TransferenciaInformacionBaseDetalleHelper : HelperBase
    {

        public TransferenciaInformacionBaseDetalleHelper()
            : base(Consultas.TransferenciaInformacionBaseDetalleSql)
        {
        }

        public TransferenciaInformacionBaseDetalleDTO Create(IDataReader dr)
        {
            TransferenciaInformacionBaseDetalleDTO entity = new TransferenciaInformacionBaseDetalleDTO();

            int ITINFBCODI = dr.GetOrdinal(this.TINFBCODI);
            if (!dr.IsDBNull(ITINFBCODI)) entity.TinfbCodi = dr.GetInt32(ITINFBCODI);

            int ITINFBDECODI = dr.GetOrdinal(this.TINFBDECODI);
            if (!dr.IsDBNull(ITINFBDECODI)) entity.TinfbDeCodi = dr.GetDecimal(ITINFBDECODI);

            int ITINTFBDEVERSION = dr.GetOrdinal(this.TINTFBDEVERSION);
            if (!dr.IsDBNull(ITINTFBDEVERSION)) entity.TinfbDeVersion = dr.GetInt32(ITINTFBDEVERSION);

            int ITINFBDEDIA = dr.GetOrdinal(this.TINFBDEDIA);
            if (!dr.IsDBNull(ITINFBDEDIA)) entity.TinfbDeDia = dr.GetInt32(ITINFBDEDIA);

            int ITINFBDEPROMEDIODIA = dr.GetOrdinal(this.TINFBDEPROMEDIODIA);
            if (!dr.IsDBNull(ITINFBDEPROMEDIODIA)) entity.TinfbDePromedioDia = dr.GetDecimal(ITINFBDEPROMEDIODIA);

            int iTINFBDESUMADIA = dr.GetOrdinal(this.TINFBDESUMADIA);
            if (!dr.IsDBNull(iTINFBDESUMADIA)) entity.TinfbDeSumaDia = dr.GetDecimal(iTINFBDESUMADIA);

            int iTINFBDE1 = dr.GetOrdinal(this.TINFBDE1);
            if (!dr.IsDBNull(iTINFBDE1)) entity.TinfbDe1 = dr.GetDecimal(iTINFBDE1);

            int iTINFBDE2 = dr.GetOrdinal(this.TINFBDE2);
            if (!dr.IsDBNull(iTINFBDE2)) entity.TinfbDe2 = dr.GetDecimal(iTINFBDE2);

            int iTINFBDE3 = dr.GetOrdinal(this.TINFBDE3);
            if (!dr.IsDBNull(iTINFBDE3)) entity.TinfbDe3 = dr.GetDecimal(iTINFBDE3);

            int iTINFBDE4 = dr.GetOrdinal(this.TINFBDE4);
            if (!dr.IsDBNull(iTINFBDE4)) entity.TinfbDe4 = dr.GetDecimal(iTINFBDE4);

            int iTINFBDE5 = dr.GetOrdinal(this.TINFBDE5);
            if (!dr.IsDBNull(iTINFBDE5)) entity.TinfbDe5 = dr.GetDecimal(iTINFBDE5);

            int iTINFBDE6 = dr.GetOrdinal(this.TINFBDE6);
            if (!dr.IsDBNull(iTINFBDE6)) entity.TinfbDe6 = dr.GetDecimal(iTINFBDE6);

            int iTINFBDE7 = dr.GetOrdinal(this.TINFBDE7);
            if (!dr.IsDBNull(iTINFBDE7)) entity.TinfbDe7 = dr.GetDecimal(iTINFBDE7);

            int iTINFBDE8 = dr.GetOrdinal(this.TINFBDE8);
            if (!dr.IsDBNull(iTINFBDE8)) entity.TinfbDe8 = dr.GetDecimal(iTINFBDE8);

            int iTINFBDE9 = dr.GetOrdinal(this.TINFBDE9);
            if (!dr.IsDBNull(iTINFBDE9)) entity.TinfbDe9 = dr.GetDecimal(iTINFBDE9);

            int iTINFBDE10 = dr.GetOrdinal(this.TINFBDE10);
            if (!dr.IsDBNull(iTINFBDE10)) entity.TinfbDe10 = dr.GetDecimal(iTINFBDE10);

            int iTINFBDE11 = dr.GetOrdinal(this.TINFBDE11);
            if (!dr.IsDBNull(iTINFBDE11)) entity.TinfbDe11 = dr.GetDecimal(iTINFBDE11);

            int iTINFBDE12 = dr.GetOrdinal(this.TINFBDE12);
            if (!dr.IsDBNull(iTINFBDE12)) entity.TinfbDe12 = dr.GetDecimal(iTINFBDE12);

            int iTINFBDE13 = dr.GetOrdinal(this.TINFBDE13);
            if (!dr.IsDBNull(iTINFBDE13)) entity.TinfbDe13 = dr.GetDecimal(iTINFBDE13);

            int iTINFBDE14 = dr.GetOrdinal(this.TINFBDE14);
            if (!dr.IsDBNull(iTINFBDE14)) entity.TinfbDe14 = dr.GetDecimal(iTINFBDE14);

            int iTINFBDE15 = dr.GetOrdinal(this.TINFBDE15);
            if (!dr.IsDBNull(iTINFBDE15)) entity.TinfbDe15 = dr.GetDecimal(iTINFBDE15);

            int iTINFBDE16 = dr.GetOrdinal(this.TINFBDE16);
            if (!dr.IsDBNull(iTINFBDE16)) entity.TinfbDe16 = dr.GetDecimal(iTINFBDE16);

            int iTINFBDE17 = dr.GetOrdinal(this.TINFBDE17);
            if (!dr.IsDBNull(iTINFBDE17)) entity.TinfbDe17 = dr.GetDecimal(iTINFBDE17);

            int iTINFBDE18 = dr.GetOrdinal(this.TINFBDE18);
            if (!dr.IsDBNull(iTINFBDE18)) entity.TinfbDe18 = dr.GetDecimal(iTINFBDE18);

            int iTINFBDE19 = dr.GetOrdinal(this.TINFBDE19);
            if (!dr.IsDBNull(iTINFBDE19)) entity.TinfbDe19 = dr.GetDecimal(iTINFBDE19);

            int iTINFBDE20 = dr.GetOrdinal(this.TINFBDE20);
            if (!dr.IsDBNull(iTINFBDE20)) entity.TinfbDe20 = dr.GetDecimal(iTINFBDE20);

            int iTINFBDE21 = dr.GetOrdinal(this.TINFBDE21);
            if (!dr.IsDBNull(iTINFBDE21)) entity.TinfbDe21 = dr.GetDecimal(iTINFBDE21);

            int iTINFBDE22 = dr.GetOrdinal(this.TINFBDE22);
            if (!dr.IsDBNull(iTINFBDE22)) entity.TinfbDe22 = dr.GetDecimal(iTINFBDE22);

            int iTINFBDE23 = dr.GetOrdinal(this.TINFBDE23);
            if (!dr.IsDBNull(iTINFBDE23)) entity.TinfbDe23 = dr.GetDecimal(iTINFBDE23);

            int iTINFBDE24 = dr.GetOrdinal(this.TINFBDE24);
            if (!dr.IsDBNull(iTINFBDE24)) entity.TinfbDe24 = dr.GetDecimal(iTINFBDE24);

            int iTINFBDE25 = dr.GetOrdinal(this.TINFBDE25);
            if (!dr.IsDBNull(iTINFBDE25)) entity.TinfbDe25 = dr.GetDecimal(iTINFBDE25);

            int iTINFBDE26 = dr.GetOrdinal(this.TINFBDE26);
            if (!dr.IsDBNull(iTINFBDE26)) entity.TinfbDe26 = dr.GetDecimal(iTINFBDE26);

            int iTINFBDE27 = dr.GetOrdinal(this.TINFBDE27);
            if (!dr.IsDBNull(iTINFBDE27)) entity.TinfbDe27 = dr.GetDecimal(iTINFBDE27);

            int iTINFBDE28 = dr.GetOrdinal(this.TINFBDE28);
            if (!dr.IsDBNull(iTINFBDE28)) entity.TinfbDe28 = dr.GetDecimal(iTINFBDE28);

            int iTINFBDE29 = dr.GetOrdinal(this.TINFBDE29);
            if (!dr.IsDBNull(iTINFBDE29)) entity.TinfbDe29 = dr.GetDecimal(iTINFBDE29);

            int iTINFBDE30 = dr.GetOrdinal(this.TINFBDE30);
            if (!dr.IsDBNull(iTINFBDE30)) entity.TinfbDe30 = dr.GetDecimal(iTINFBDE30);

            int iTINFBDE31 = dr.GetOrdinal(this.TINFBDE31);
            if (!dr.IsDBNull(iTINFBDE31)) entity.TinfbDe31 = dr.GetDecimal(iTINFBDE31);

            int iTINFBDE32 = dr.GetOrdinal(this.TINFBDE32);
            if (!dr.IsDBNull(iTINFBDE32)) entity.TinfbDe32 = dr.GetDecimal(iTINFBDE32);

            int iTINFBDE33 = dr.GetOrdinal(this.TINFBDE33);
            if (!dr.IsDBNull(iTINFBDE33)) entity.TinfbDe33 = dr.GetDecimal(iTINFBDE33);

            int iTINFBDE34 = dr.GetOrdinal(this.TINFBDE34);
            if (!dr.IsDBNull(iTINFBDE34)) entity.TinfbDe34 = dr.GetDecimal(iTINFBDE34);

            int iTINFBDE35 = dr.GetOrdinal(this.TINFBDE35);
            if (!dr.IsDBNull(iTINFBDE35)) entity.TinfbDe35 = dr.GetDecimal(iTINFBDE35);

            int iTINFBDE36 = dr.GetOrdinal(this.TINFBDE36);
            if (!dr.IsDBNull(iTINFBDE36)) entity.TinfbDe36 = dr.GetDecimal(iTINFBDE36);

            int iTINFBDE37 = dr.GetOrdinal(this.TINFBDE37);
            if (!dr.IsDBNull(iTINFBDE37)) entity.TinfbDe37 = dr.GetDecimal(iTINFBDE37);

            int iTINFBDE38 = dr.GetOrdinal(this.TINFBDE38);
            if (!dr.IsDBNull(iTINFBDE38)) entity.TinfbDe38 = dr.GetDecimal(iTINFBDE38);

            int iTINFBDE39 = dr.GetOrdinal(this.TINFBDE39);
            if (!dr.IsDBNull(iTINFBDE39)) entity.TinfbDe39 = dr.GetDecimal(iTINFBDE39);

            int iTINFBDE40 = dr.GetOrdinal(this.TINFBDE40);
            if (!dr.IsDBNull(iTINFBDE40)) entity.TinfbDe40 = dr.GetDecimal(iTINFBDE40);

            int iTINFBDE41 = dr.GetOrdinal(this.TINFBDE41);
            if (!dr.IsDBNull(iTINFBDE41)) entity.TinfbDe41 = dr.GetDecimal(iTINFBDE41);

            int iTINFBDE42 = dr.GetOrdinal(this.TINFBDE42);
            if (!dr.IsDBNull(iTINFBDE42)) entity.TinfbDe42 = dr.GetDecimal(iTINFBDE42);

            int iTINFBDE43 = dr.GetOrdinal(this.TINFBDE43);
            if (!dr.IsDBNull(iTINFBDE43)) entity.TinfbDe43 = dr.GetDecimal(iTINFBDE43);

            int iTINFBDE44 = dr.GetOrdinal(this.TINFBDE44);
            if (!dr.IsDBNull(iTINFBDE44)) entity.TinfbDe44 = dr.GetDecimal(iTINFBDE44);

            int iTINFBDE45 = dr.GetOrdinal(this.TINFBDE45);
            if (!dr.IsDBNull(iTINFBDE45)) entity.TinfbDe45 = dr.GetDecimal(iTINFBDE45);

            int iTINFBDE46 = dr.GetOrdinal(this.TINFBDE46);
            if (!dr.IsDBNull(iTINFBDE46)) entity.TinfbDe46 = dr.GetDecimal(iTINFBDE46);

            int iTINFBDE47 = dr.GetOrdinal(this.TINFBDE47);
            if (!dr.IsDBNull(iTINFBDE47)) entity.TinfbDe47 = dr.GetDecimal(iTINFBDE47);

            int iTINFBDE48 = dr.GetOrdinal(this.TINFBDE48);
            if (!dr.IsDBNull(iTINFBDE48)) entity.TinfbDe48 = dr.GetDecimal(iTINFBDE48);

            int iTINFBDE49 = dr.GetOrdinal(this.TINFBDE49);
            if (!dr.IsDBNull(iTINFBDE49)) entity.TinfbDe49 = dr.GetDecimal(iTINFBDE49);

            int iTINFBDE50 = dr.GetOrdinal(this.TINFBDE50);
            if (!dr.IsDBNull(iTINFBDE50)) entity.TinfbDe50 = dr.GetDecimal(iTINFBDE50);

            int iTINFBDE51 = dr.GetOrdinal(this.TINFBDE51);
            if (!dr.IsDBNull(iTINFBDE50)) entity.TinfbDe51 = dr.GetDecimal(iTINFBDE51);

            int iTINFBDE52 = dr.GetOrdinal(this.TINFBDE52);
            if (!dr.IsDBNull(iTINFBDE52)) entity.TinfbDe52 = dr.GetDecimal(iTINFBDE52);

            int iTINFBDE53 = dr.GetOrdinal(this.TINFBDE53);
            if (!dr.IsDBNull(iTINFBDE53)) entity.TinfbDe53 = dr.GetDecimal(iTINFBDE53);

            int iTINFBDE54 = dr.GetOrdinal(this.TINFBDE54);
            if (!dr.IsDBNull(iTINFBDE54)) entity.TinfbDe54 = dr.GetDecimal(iTINFBDE54);

            int iTINFBDE55 = dr.GetOrdinal(this.TINFBDE55);
            if (!dr.IsDBNull(iTINFBDE55)) entity.TinfbDe55 = dr.GetDecimal(iTINFBDE55);

            int iTINFBDE56 = dr.GetOrdinal(this.TINFBDE56);
            if (!dr.IsDBNull(iTINFBDE56)) entity.TinfbDe56 = dr.GetDecimal(iTINFBDE56);

            int iTINFBDE57 = dr.GetOrdinal(this.TINFBDE57);
            if (!dr.IsDBNull(iTINFBDE57)) entity.TinfbDe57 = dr.GetDecimal(iTINFBDE57);

            int iTINFBDE58 = dr.GetOrdinal(this.TINFBDE58);
            if (!dr.IsDBNull(iTINFBDE58)) entity.TinfbDe58 = dr.GetDecimal(iTINFBDE58);

            int iTINFBDE59 = dr.GetOrdinal(this.TINFBDE59);
            if (!dr.IsDBNull(iTINFBDE59)) entity.TinfbDe59 = dr.GetDecimal(iTINFBDE59);

            int iTINFBDE60 = dr.GetOrdinal(this.TINFBDE60);
            if (!dr.IsDBNull(iTINFBDE60)) entity.TinfbDe60 = dr.GetDecimal(iTINFBDE60);

            int iTINFBDE61 = dr.GetOrdinal(this.TINFBDE61);
            if (!dr.IsDBNull(iTINFBDE61)) entity.TinfbDe61 = dr.GetDecimal(iTINFBDE61);

            int iTINFBDE62 = dr.GetOrdinal(this.TINFBDE62);
            if (!dr.IsDBNull(iTINFBDE62)) entity.TinfbDe62 = dr.GetDecimal(iTINFBDE62);

            int iTINFBDE63 = dr.GetOrdinal(this.TINFBDE63);
            if (!dr.IsDBNull(iTINFBDE63)) entity.TinfbDe63 = dr.GetDecimal(iTINFBDE63);

            int iTINFBDE64 = dr.GetOrdinal(this.TINFBDE64);
            if (!dr.IsDBNull(iTINFBDE64)) entity.TinfbDe64 = dr.GetDecimal(iTINFBDE64);

            int iTINFBDE65 = dr.GetOrdinal(this.TINFBDE65);
            if (!dr.IsDBNull(iTINFBDE65)) entity.TinfbDe65 = dr.GetDecimal(iTINFBDE65);

            int iTINFBDE66 = dr.GetOrdinal(this.TINFBDE66);
            if (!dr.IsDBNull(iTINFBDE66)) entity.TinfbDe66 = dr.GetDecimal(iTINFBDE66);

            int iTINFBDE67 = dr.GetOrdinal(this.TINFBDE67);
            if (!dr.IsDBNull(iTINFBDE67)) entity.TinfbDe67 = dr.GetDecimal(iTINFBDE67);

            int iTINFBDE68 = dr.GetOrdinal(this.TINFBDE68);
            if (!dr.IsDBNull(iTINFBDE68)) entity.TinfbDe68 = dr.GetDecimal(iTINFBDE68);

            int iTINFBDE69 = dr.GetOrdinal(this.TINFBDE69);
            if (!dr.IsDBNull(iTINFBDE69)) entity.TinfbDe69 = dr.GetDecimal(iTINFBDE69);

            int iTINFBDE70 = dr.GetOrdinal(this.TINFBDE70);
            if (!dr.IsDBNull(iTINFBDE70)) entity.TinfbDe70 = dr.GetDecimal(iTINFBDE70);

            int iTINFBDE71 = dr.GetOrdinal(this.TINFBDE71);
            if (!dr.IsDBNull(iTINFBDE71)) entity.TinfbDe71 = dr.GetDecimal(iTINFBDE71);

            int iTINFBDE72 = dr.GetOrdinal(this.TINFBDE72);
            if (!dr.IsDBNull(iTINFBDE72)) entity.TinfbDe72 = dr.GetDecimal(iTINFBDE72);

            int iTINFBDE73 = dr.GetOrdinal(this.TINFBDE73);
            if (!dr.IsDBNull(iTINFBDE73)) entity.TinfbDe73 = dr.GetDecimal(iTINFBDE73);

            int iTINFBDE74 = dr.GetOrdinal(this.TINFBDE74);
            if (!dr.IsDBNull(iTINFBDE74)) entity.TinfbDe74 = dr.GetDecimal(iTINFBDE74);

            int iTINFBDE75 = dr.GetOrdinal(this.TINFBDE75);
            if (!dr.IsDBNull(iTINFBDE75)) entity.TinfbDe75 = dr.GetDecimal(iTINFBDE75);

            int iTINFBDE76 = dr.GetOrdinal(this.TINFBDE76);
            if (!dr.IsDBNull(iTINFBDE76)) entity.TinfbDe76 = dr.GetDecimal(iTINFBDE76);

            int iTINFBDE77 = dr.GetOrdinal(this.TINFBDE77);
            if (!dr.IsDBNull(iTINFBDE77)) entity.TinfbDe77 = dr.GetDecimal(iTINFBDE77);

            int iTINFBDE78 = dr.GetOrdinal(this.TINFBDE78);
            if (!dr.IsDBNull(iTINFBDE78)) entity.TinfbDe78 = dr.GetDecimal(iTINFBDE78);

            int iTINFBDE79 = dr.GetOrdinal(this.TINFBDE79);
            if (!dr.IsDBNull(iTINFBDE79)) entity.TinfbDe79 = dr.GetDecimal(iTINFBDE79);

            int iTINFBDE80 = dr.GetOrdinal(this.TINFBDE80);
            if (!dr.IsDBNull(iTINFBDE80)) entity.TinfbDe80 = dr.GetDecimal(iTINFBDE80);

            int iTINFBDE81 = dr.GetOrdinal(this.TINFBDE81);
            if (!dr.IsDBNull(iTINFBDE81)) entity.TinfbDe81 = dr.GetDecimal(iTINFBDE81);

            int iTINFBDE82 = dr.GetOrdinal(this.TINFBDE82);
            if (!dr.IsDBNull(iTINFBDE82)) entity.TinfbDe82 = dr.GetDecimal(iTINFBDE82);

            int iTINFBDE83 = dr.GetOrdinal(this.TINFBDE83);
            if (!dr.IsDBNull(iTINFBDE83)) entity.TinfbDe83 = dr.GetDecimal(iTINFBDE83);

            int iTINFBDE84 = dr.GetOrdinal(this.TINFBDE84);
            if (!dr.IsDBNull(iTINFBDE84)) entity.TinfbDe84 = dr.GetDecimal(iTINFBDE84);

            int iTINFBDE85 = dr.GetOrdinal(this.TINFBDE85);
            if (!dr.IsDBNull(iTINFBDE85)) entity.TinfbDe85 = dr.GetDecimal(iTINFBDE85);

            int iTINFBDE86 = dr.GetOrdinal(this.TINFBDE86);
            if (!dr.IsDBNull(iTINFBDE86)) entity.TinfbDe86 = dr.GetDecimal(iTINFBDE86);

            int iTINFBDE87 = dr.GetOrdinal(this.TINFBDE87);
            if (!dr.IsDBNull(iTINFBDE87)) entity.TinfbDe87 = dr.GetDecimal(iTINFBDE87);

            int iTINFBDE88 = dr.GetOrdinal(this.TINFBDE88);
            if (!dr.IsDBNull(iTINFBDE88)) entity.TinfbDe88 = dr.GetDecimal(iTINFBDE88);

            int iTINFBDE89 = dr.GetOrdinal(this.TINFBDE89);
            if (!dr.IsDBNull(iTINFBDE89)) entity.TinfbDe89 = dr.GetDecimal(iTINFBDE89);

            int iTINFBDE90 = dr.GetOrdinal(this.TINFBDE90);
            if (!dr.IsDBNull(iTINFBDE90)) entity.TinfbDe90 = dr.GetDecimal(iTINFBDE90);

            int iTINFBDE91 = dr.GetOrdinal(this.TINFBDE91);
            if (!dr.IsDBNull(iTINFBDE91)) entity.TinfbDe91 = dr.GetDecimal(iTINFBDE91);

            int iTINFBDE92 = dr.GetOrdinal(this.TINFBDE92);
            if (!dr.IsDBNull(iTINFBDE92)) entity.TinfbDe92 = dr.GetDecimal(iTINFBDE92);

            int iTINFBDE93 = dr.GetOrdinal(this.TINFBDE93);
            if (!dr.IsDBNull(iTINFBDE93)) entity.TinfbDe93 = dr.GetDecimal(iTINFBDE93);

            int iTINFBDE94 = dr.GetOrdinal(this.TINFBDE94);
            if (!dr.IsDBNull(iTINFBDE94)) entity.TinfbDe94 = dr.GetDecimal(iTINFBDE94);

            int iTINFBDE95 = dr.GetOrdinal(this.TINFBDE95);
            if (!dr.IsDBNull(iTINFBDE95)) entity.TinfbDe95 = dr.GetDecimal(iTINFBDE95);

            int iTINFBDE96 = dr.GetOrdinal(this.TINFBDE96);
            if (!dr.IsDBNull(iTINFBDE96)) entity.TinfbDe96 = dr.GetDecimal(iTINFBDE96);

            int iTINFBDEUSERNAME = dr.GetOrdinal(this.TINFBDEUSERNAME);
            if (!dr.IsDBNull(iTINFBDEUSERNAME)) entity.TinfbDeUserName = dr.GetString(iTINFBDEUSERNAME);

            int iTINFBDEFECINS = dr.GetOrdinal(this.TINFBDEFECINS);
            if (!dr.IsDBNull(iTINFBDEFECINS)) entity.TinfbDeFecIns = dr.GetDateTime(iTINFBDEFECINS);

            int iTINFBDEFECACT = dr.GetOrdinal(this.TINFBDEFECACT);
            if (!dr.IsDBNull(iTINFBDEFECACT)) entity.TinfbDeFecAct = dr.GetDateTime(iTINFBDEFECACT);

            return entity;
        }


        #region Mapeo de Campos

        public string TINFBCODI = "TINFBCODI";
        public string TINFBDECODI = "TINFBDECODI";
        public string TINTFBDEVERSION = "TINFBDEVERSION";
        public string TINFBDEDIA = "TINFBDEDIA";
        public string TINFBDEPROMEDIODIA = "TINFBDEPROMEDIODIA";
        public string TINFBDESUMADIA = "TINFBDESUMADIA";
        public string TINFBDE1 = "TINFBDE1";
        public string TINFBDE2 = "TINFBDE2";
        public string TINFBDE3 = "TINFBDE3";
        public string TINFBDE4 = "TINFBDE4";
        public string TINFBDE5 = "TINFBDE5";
        public string TINFBDE6 = "TINFBDE6";
        public string TINFBDE7 = "TINFBDE7";
        public string TINFBDE8 = "TINFBDE8";
        public string TINFBDE9 = "TINFBDE9";
        public string TINFBDE10 = "TINFBDE10";
        public string TINFBDE11 = "TINFBDE11";
        public string TINFBDE12 = "TINFBDE12";
        public string TINFBDE13 = "TINFBDE13";
        public string TINFBDE14 = "TINFBDE14";
        public string TINFBDE15 = "TINFBDE15";
        public string TINFBDE16 = "TINFBDE16";
        public string TINFBDE17 = "TINFBDE17";
        public string TINFBDE18 = "TINFBDE18";
        public string TINFBDE19 = "TINFBDE19";
        public string TINFBDE20 = "TINFBDE20";
        public string TINFBDE21 = "TINFBDE21";
        public string TINFBDE22 = "TINFBDE22";
        public string TINFBDE23 = "TINFBDE23";
        public string TINFBDE24 = "TINFBDE24";
        public string TINFBDE25 = "TINFBDE25";
        public string TINFBDE26 = "TINFBDE26";
        public string TINFBDE27 = "TINFBDE27";
        public string TINFBDE28 = "TINFBDE28";
        public string TINFBDE29 = "TINFBDE29";
        public string TINFBDE30 = "TINFBDE30";
        public string TINFBDE31 = "TINFBDE31";
        public string TINFBDE32 = "TINFBDE32";
        public string TINFBDE33 = "TINFBDE33";
        public string TINFBDE34 = "TINFBDE34";
        public string TINFBDE35 = "TINFBDE35";
        public string TINFBDE36 = "TINFBDE36";
        public string TINFBDE37 = "TINFBDE37";
        public string TINFBDE38 = "TINFBDE38";
        public string TINFBDE39 = "TINFBDE39";
        public string TINFBDE40 = "TINFBDE40";
        public string TINFBDE41 = "TINFBDE41";
        public string TINFBDE42 = "TINFBDE42";
        public string TINFBDE43 = "TINFBDE43";
        public string TINFBDE44 = "TINFBDE44";
        public string TINFBDE45 = "TINFBDE45";
        public string TINFBDE46 = "TINFBDE46";
        public string TINFBDE47 = "TINFBDE47";
        public string TINFBDE48 = "TINFBDE48";
        public string TINFBDE49 = "TINFBDE49";
        public string TINFBDE50 = "TINFBDE50";
        public string TINFBDE51 = "TINFBDE51";
        public string TINFBDE52 = "TINFBDE52";
        public string TINFBDE53 = "TINFBDE53";
        public string TINFBDE54 = "TINFBDE54";
        public string TINFBDE55 = "TINFBDE55";
        public string TINFBDE56 = "TINFBDE56";
        public string TINFBDE57 = "TINFBDE57";
        public string TINFBDE58 = "TINFBDE58";
        public string TINFBDE59 = "TINFBDE59";
        public string TINFBDE60 = "TINFBDE60";
        public string TINFBDE61 = "TINFBDE61";
        public string TINFBDE62 = "TINFBDE62";
        public string TINFBDE63 = "TINFBDE63";
        public string TINFBDE64 = "TINFBDE64";
        public string TINFBDE65 = "TINFBDE65";
        public string TINFBDE66 = "TINFBDE66";
        public string TINFBDE67 = "TINFBDE67";
        public string TINFBDE68 = "TINFBDE68";
        public string TINFBDE69 = "TINFBDE69";
        public string TINFBDE70 = "TINFBDE70";
        public string TINFBDE71 = "TINFBDE71";
        public string TINFBDE72 = "TINFBDE72";
        public string TINFBDE73 = "TINFBDE73";
        public string TINFBDE74 = "TINFBDE74";
        public string TINFBDE75 = "TINFBDE75";
        public string TINFBDE76 = "TINFBDE76";
        public string TINFBDE77 = "TINFBDE77";
        public string TINFBDE78 = "TINFBDE78";
        public string TINFBDE79 = "TINFBDE79";
        public string TINFBDE80 = "TINFBDE80";
        public string TINFBDE81 = "TINFBDE81";
        public string TINFBDE82 = "TINFBDE82";
        public string TINFBDE83 = "TINFBDE83";
        public string TINFBDE84 = "TINFBDE84";
        public string TINFBDE85 = "TINFBDE85";
        public string TINFBDE86 = "TINFBDE86";
        public string TINFBDE87 = "TINFBDE87";
        public string TINFBDE88 = "TINFBDE88";
        public string TINFBDE89 = "TINFBDE89";
        public string TINFBDE90 = "TINFBDE90";
        public string TINFBDE91 = "TINFBDE91";
        public string TINFBDE92 = "TINFBDE92";
        public string TINFBDE93 = "TINFBDE93";
        public string TINFBDE94 = "TINFBDE94";
        public string TINFBDE95 = "TINFBDE95";
        public string TINFBDE96 = "TINFBDE96";
        public string TINFBDEUSERNAME = "TINFBDEUSERNAME";
        public string TINFBDEFECINS = "TINFBDEFECINS";
        public string TINFBDEFECACT = "TINFBDEFECACT";
        //para los parametros 
        public string PERICODI = "PERICODI";
        public string EMPRCODI = "EMPRCODI";
        public string TINFBVERSION = "TINFBVERSION";
        //por mientras del list cabe
        public string TINFBCODIGO = "TINFBCODIGO";
        public string BARRCODI = "BARRCODI";
        public string EQUICODI = "EQUICODI";
        //public string TINFBCODIGO = "TINFBCODIGO";
        public string TINFBTIPOINFORMACION = "TINFBTIPOINFORMACION";
        public string ENERGIA = "ENERGIA";
        public string NOMBBARRA = "NOMBBARRA";
        
        #endregion


        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlListByPeriodoVersion
        {
            get { return base.GetSqlXml("GetByPeriodoVersion"); }
        }

        public string SqlBalanceEnergiaActiva
        {
            get { return base.GetSqlXml("BalanceEnergiaActiva"); }
        }

        public string SqlListByTransferenciaInfoBase
        {
            get { return base.GetSqlXml("ListByTransferenciaInfoBase"); }

        }

        public string SqlDeleteListaTransferenciaInfoBaseDetalle
        {
            get { return base.GetSqlXml("DeleteListaTransferenciaInfoBaseDetalle"); }

        }

    }
}
