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
    /// <summary>
    /// Clase que contiene el mapeo de la tabla trn_trans_entrega_detalle
    /// </summary>
    public class TransferenciaEntregaDetalleHelper : HelperBase
    {
        public TransferenciaEntregaDetalleHelper()
            : base(Consultas.TransferenciaEntregaDetalleSql)
        {
        }

        public TransferenciaEntregaDetalleDTO Create(IDataReader dr)
        {
            TransferenciaEntregaDetalleDTO entity = new TransferenciaEntregaDetalleDTO();

            int iTRANENTRCODI = dr.GetOrdinal(this.TRANENTRCODI);
            if (!dr.IsDBNull(iTRANENTRCODI)) entity.TranEntrCodi = dr.GetInt32(iTRANENTRCODI);

            int iTRANENTRDETACODI = dr.GetOrdinal(this.TRANENTRDETACODI);
            if (!dr.IsDBNull(iTRANENTRDETACODI)) entity.TranEntrDetaCodi = dr.GetDecimal(iTRANENTRDETACODI);

            int iTRANENTRDETAVERSION = dr.GetOrdinal(this.TRANENTRDETAVERSION);
            if (!dr.IsDBNull(iTRANENTRDETAVERSION)) entity.TranEntrDetaVersion = dr.GetInt32(iTRANENTRDETAVERSION);

            int iTRANENTRDETADIA = dr.GetOrdinal(this.TRANENTRDETADIA);
            if (!dr.IsDBNull(iTRANENTRDETADIA)) entity.TranEntrDetaDia = dr.GetInt32(iTRANENTRDETADIA);

            int iTRANENTRDETAPROMDIA = dr.GetOrdinal(this.TRANENTRDETAPROMDIA);
            if (!dr.IsDBNull(iTRANENTRDETAPROMDIA)) entity.TranEntrDetaPromDia = dr.GetDecimal(iTRANENTRDETAPROMDIA);

            int iTRANENTRDETASUMADIA = dr.GetOrdinal(this.TRANENTRDETASUMADIA);
            if (!dr.IsDBNull(iTRANENTRDETASUMADIA)) entity.TranEntrDetaSumaDia = dr.GetDecimal(iTRANENTRDETASUMADIA);

            int iTRANENTRDETAH1 = dr.GetOrdinal(this.TRANENTRDETAH1);
            if (!dr.IsDBNull(iTRANENTRDETAH1)) entity.TranEntrDetah1 = dr.GetDecimal(iTRANENTRDETAH1);

            int iTRANENTRDETAH2 = dr.GetOrdinal(this.TRANENTRDETAH2);
            if (!dr.IsDBNull(iTRANENTRDETAH2)) entity.TranEntrDetah2 = dr.GetDecimal(iTRANENTRDETAH2);

            int iTRANENTRDETAH3 = dr.GetOrdinal(this.TRANENTRDETAH3);
            if (!dr.IsDBNull(iTRANENTRDETAH3)) entity.TranEntrDetah3 = dr.GetDecimal(iTRANENTRDETAH3);

            int iTRANENTRDETAH4 = dr.GetOrdinal(this.TRANENTRDETAH4);
            if (!dr.IsDBNull(iTRANENTRDETAH4)) entity.TranEntrDetah4 = dr.GetDecimal(iTRANENTRDETAH4);

            int iTRANENTRDETAH5 = dr.GetOrdinal(this.TRANENTRDETAH5);
            if (!dr.IsDBNull(iTRANENTRDETAH5)) entity.TranEntrDetah5 = dr.GetDecimal(iTRANENTRDETAH5);

            int iTRANENTRDETAH6 = dr.GetOrdinal(this.TRANENTRDETAH6);
            if (!dr.IsDBNull(iTRANENTRDETAH6)) entity.TranEntrDetah6 = dr.GetDecimal(iTRANENTRDETAH6);

            int iTRANENTRDETAH7 = dr.GetOrdinal(this.TRANENTRDETAH7);
            if (!dr.IsDBNull(iTRANENTRDETAH7)) entity.TranEntrDetah7 = dr.GetDecimal(iTRANENTRDETAH7);

            int iTRANENTRDETAH8 = dr.GetOrdinal(this.TRANENTRDETAH8);
            if (!dr.IsDBNull(iTRANENTRDETAH8)) entity.TranEntrDetah8 = dr.GetDecimal(iTRANENTRDETAH8);

            int iTRANENTRDETAH9 = dr.GetOrdinal(this.TRANENTRDETAH9);
            if (!dr.IsDBNull(iTRANENTRDETAH9)) entity.TranEntrDetah9 = dr.GetDecimal(iTRANENTRDETAH9);

            int iTRANENTRDETAH10 = dr.GetOrdinal(this.TRANENTRDETAH10);
            if (!dr.IsDBNull(iTRANENTRDETAH10)) entity.TranEntrDetah10 = dr.GetDecimal(iTRANENTRDETAH10);

            int iTRANENTRDETAH11 = dr.GetOrdinal(this.TRANENTRDETAH11);
            if (!dr.IsDBNull(iTRANENTRDETAH11)) entity.TranEntrDetah11 = dr.GetDecimal(iTRANENTRDETAH11);

            int iTRANENTRDETAH12 = dr.GetOrdinal(this.TRANENTRDETAH12);
            if (!dr.IsDBNull(iTRANENTRDETAH12)) entity.TranEntrDetah12 = dr.GetDecimal(iTRANENTRDETAH12);

            int iTRANENTRDETAH13 = dr.GetOrdinal(this.TRANENTRDETAH13);
            if (!dr.IsDBNull(iTRANENTRDETAH13)) entity.TranEntrDetah13 = dr.GetDecimal(iTRANENTRDETAH13);

            int iTRANENTRDETAH14 = dr.GetOrdinal(this.TRANENTRDETAH14);
            if (!dr.IsDBNull(iTRANENTRDETAH14)) entity.TranEntrDetah14 = dr.GetDecimal(iTRANENTRDETAH14);

            int iTRANENTRDETAH15 = dr.GetOrdinal(this.TRANENTRDETAH15);
            if (!dr.IsDBNull(iTRANENTRDETAH15)) entity.TranEntrDetah15 = dr.GetDecimal(iTRANENTRDETAH15);

            int iTRANENTRDETAH16 = dr.GetOrdinal(this.TRANENTRDETAH16);
            if (!dr.IsDBNull(iTRANENTRDETAH16)) entity.TranEntrDetah16 = dr.GetDecimal(iTRANENTRDETAH16);

            int iTRANENTRDETAH17 = dr.GetOrdinal(this.TRANENTRDETAH17);
            if (!dr.IsDBNull(iTRANENTRDETAH17)) entity.TranEntrDetah17 = dr.GetDecimal(iTRANENTRDETAH17);

            int iTRANENTRDETAH18 = dr.GetOrdinal(this.TRANENTRDETAH18);
            if (!dr.IsDBNull(iTRANENTRDETAH18)) entity.TranEntrDetah18 = dr.GetDecimal(iTRANENTRDETAH18);

            int iTRANENTRDETAH19 = dr.GetOrdinal(this.TRANENTRDETAH19);
            if (!dr.IsDBNull(iTRANENTRDETAH19)) entity.TranEntrDetah19 = dr.GetDecimal(iTRANENTRDETAH19);

            int iTRANENTRDETAH20 = dr.GetOrdinal(this.TRANENTRDETAH20);
            if (!dr.IsDBNull(iTRANENTRDETAH20)) entity.TranEntrDetah20 = dr.GetDecimal(iTRANENTRDETAH20);

            int iTRANENTRDETAH21 = dr.GetOrdinal(this.TRANENTRDETAH21);
            if (!dr.IsDBNull(iTRANENTRDETAH21)) entity.TranEntrDetah21 = dr.GetDecimal(iTRANENTRDETAH21);

            int iTRANENTRDETAH22 = dr.GetOrdinal(this.TRANENTRDETAH22);
            if (!dr.IsDBNull(iTRANENTRDETAH22)) entity.TranEntrDetah22 = dr.GetDecimal(iTRANENTRDETAH22);

            int iTRANENTRDETAH23 = dr.GetOrdinal(this.TRANENTRDETAH23);
            if (!dr.IsDBNull(iTRANENTRDETAH23)) entity.TranEntrDetah23 = dr.GetDecimal(iTRANENTRDETAH23);

            int iTRANENTRDETAH24 = dr.GetOrdinal(this.TRANENTRDETAH24);
            if (!dr.IsDBNull(iTRANENTRDETAH24)) entity.TranEntrDetah24 = dr.GetDecimal(iTRANENTRDETAH24);

            int iTRANENTRDETAH25 = dr.GetOrdinal(this.TRANENTRDETAH25);
            if (!dr.IsDBNull(iTRANENTRDETAH25)) entity.TranEntrDetah25 = dr.GetDecimal(iTRANENTRDETAH25);

            int iTRANENTRDETAH26 = dr.GetOrdinal(this.TRANENTRDETAH26);
            if (!dr.IsDBNull(iTRANENTRDETAH26)) entity.TranEntrDetah26 = dr.GetDecimal(iTRANENTRDETAH26);

            int iTRANENTRDETAH27 = dr.GetOrdinal(this.TRANENTRDETAH27);
            if (!dr.IsDBNull(iTRANENTRDETAH27)) entity.TranEntrDetah27 = dr.GetDecimal(iTRANENTRDETAH27);

            int iTRANENTRDETAH28 = dr.GetOrdinal(this.TRANENTRDETAH28);
            if (!dr.IsDBNull(iTRANENTRDETAH28)) entity.TranEntrDetah28 = dr.GetDecimal(iTRANENTRDETAH28);

            int iTRANENTRDETAH29 = dr.GetOrdinal(this.TRANENTRDETAH29);
            if (!dr.IsDBNull(iTRANENTRDETAH29)) entity.TranEntrDetah29 = dr.GetDecimal(iTRANENTRDETAH29);

            int iTRANENTRDETAH30 = dr.GetOrdinal(this.TRANENTRDETAH30);
            if (!dr.IsDBNull(iTRANENTRDETAH30)) entity.TranEntrDetah30 = dr.GetDecimal(iTRANENTRDETAH30);

            int iTRANENTRDETAH31 = dr.GetOrdinal(this.TRANENTRDETAH31);
            if (!dr.IsDBNull(iTRANENTRDETAH31)) entity.TranEntrDetah31 = dr.GetDecimal(iTRANENTRDETAH31);

            int iTRANENTRDETAH32 = dr.GetOrdinal(this.TRANENTRDETAH32);
            if (!dr.IsDBNull(iTRANENTRDETAH32)) entity.TranEntrDetah32 = dr.GetDecimal(iTRANENTRDETAH32);

            int iTRANENTRDETAH33 = dr.GetOrdinal(this.TRANENTRDETAH33);
            if (!dr.IsDBNull(iTRANENTRDETAH33)) entity.TranEntrDetah33 = dr.GetDecimal(iTRANENTRDETAH33);

            int iTRANENTRDETAH34 = dr.GetOrdinal(this.TRANENTRDETAH34);
            if (!dr.IsDBNull(iTRANENTRDETAH34)) entity.TranEntrDetah34 = dr.GetDecimal(iTRANENTRDETAH34);

            int iTRANENTRDETAH35 = dr.GetOrdinal(this.TRANENTRDETAH35);
            if (!dr.IsDBNull(iTRANENTRDETAH35)) entity.TranEntrDetah35 = dr.GetDecimal(iTRANENTRDETAH35);

            int iTRANENTRDETAH36 = dr.GetOrdinal(this.TRANENTRDETAH36);
            if (!dr.IsDBNull(iTRANENTRDETAH36)) entity.TranEntrDetah36 = dr.GetDecimal(iTRANENTRDETAH36);

            int iTRANENTRDETAH37 = dr.GetOrdinal(this.TRANENTRDETAH37);
            if (!dr.IsDBNull(iTRANENTRDETAH37)) entity.TranEntrDetah37 = dr.GetDecimal(iTRANENTRDETAH37);

            int iTRANENTRDETAH38 = dr.GetOrdinal(this.TRANENTRDETAH38);
            if (!dr.IsDBNull(iTRANENTRDETAH38)) entity.TranEntrDetah38 = dr.GetDecimal(iTRANENTRDETAH38);

            int iTRANENTRDETAH39 = dr.GetOrdinal(this.TRANENTRDETAH39);
            if (!dr.IsDBNull(iTRANENTRDETAH39)) entity.TranEntrDetah39 = dr.GetDecimal(iTRANENTRDETAH39);

            int iTRANENTRDETAH40 = dr.GetOrdinal(this.TRANENTRDETAH40);
            if (!dr.IsDBNull(iTRANENTRDETAH40)) entity.TranEntrDetah40 = dr.GetDecimal(iTRANENTRDETAH40);

            int iTRANENTRDETAH41 = dr.GetOrdinal(this.TRANENTRDETAH41);
            if (!dr.IsDBNull(iTRANENTRDETAH41)) entity.TranEntrDetah41 = dr.GetDecimal(iTRANENTRDETAH41);

            int iTRANENTRDETAH42 = dr.GetOrdinal(this.TRANENTRDETAH42);
            if (!dr.IsDBNull(iTRANENTRDETAH42)) entity.TranEntrDetah42 = dr.GetDecimal(iTRANENTRDETAH42);

            int iTRANENTRDETAH43 = dr.GetOrdinal(this.TRANENTRDETAH43);
            if (!dr.IsDBNull(iTRANENTRDETAH43)) entity.TranEntrDetah43 = dr.GetDecimal(iTRANENTRDETAH43);

            int iTRANENTRDETAH44 = dr.GetOrdinal(this.TRANENTRDETAH44);
            if (!dr.IsDBNull(iTRANENTRDETAH44)) entity.TranEntrDetah44 = dr.GetDecimal(iTRANENTRDETAH44);

            int iTRANENTRDETAH45 = dr.GetOrdinal(this.TRANENTRDETAH45);
            if (!dr.IsDBNull(iTRANENTRDETAH45)) entity.TranEntrDetah45 = dr.GetDecimal(iTRANENTRDETAH45);

            int iTRANENTRDETAH46 = dr.GetOrdinal(this.TRANENTRDETAH46);
            if (!dr.IsDBNull(iTRANENTRDETAH46)) entity.TranEntrDetah46 = dr.GetDecimal(iTRANENTRDETAH46);

            int iTRANENTRDETAH47 = dr.GetOrdinal(this.TRANENTRDETAH47);
            if (!dr.IsDBNull(iTRANENTRDETAH47)) entity.TranEntrDetah47 = dr.GetDecimal(iTRANENTRDETAH47);

            int iTRANENTRDETAH48 = dr.GetOrdinal(this.TRANENTRDETAH48);
            if (!dr.IsDBNull(iTRANENTRDETAH48)) entity.TranEntrDetah48 = dr.GetDecimal(iTRANENTRDETAH48);

            int iTRANENTRDETAH49 = dr.GetOrdinal(this.TRANENTRDETAH49);
            if (!dr.IsDBNull(iTRANENTRDETAH49)) entity.TranEntrDetah49 = dr.GetDecimal(iTRANENTRDETAH49);

            int iTRANENTRDETAH50 = dr.GetOrdinal(this.TRANENTRDETAH50);
            if (!dr.IsDBNull(iTRANENTRDETAH50)) entity.TranEntrDetah50 = dr.GetDecimal(iTRANENTRDETAH50);

            int iTRANENTRDETAH51 = dr.GetOrdinal(this.TRANENTRDETAH51);
            if (!dr.IsDBNull(iTRANENTRDETAH50)) entity.TranEntrDetah51 = dr.GetDecimal(iTRANENTRDETAH51);

            int iTRANENTRDETAH52 = dr.GetOrdinal(this.TRANENTRDETAH52);
            if (!dr.IsDBNull(iTRANENTRDETAH52)) entity.TranEntrDetah52 = dr.GetDecimal(iTRANENTRDETAH52);

            int iTRANENTRDETAH53 = dr.GetOrdinal(this.TRANENTRDETAH53);
            if (!dr.IsDBNull(iTRANENTRDETAH53)) entity.TranEntrDetah53 = dr.GetDecimal(iTRANENTRDETAH53);

            int iTRANENTRDETAH54 = dr.GetOrdinal(this.TRANENTRDETAH54);
            if (!dr.IsDBNull(iTRANENTRDETAH54)) entity.TranEntrDetah54 = dr.GetDecimal(iTRANENTRDETAH54);

            int iTRANENTRDETAH55 = dr.GetOrdinal(this.TRANENTRDETAH55);
            if (!dr.IsDBNull(iTRANENTRDETAH55)) entity.TranEntrDetah55 = dr.GetDecimal(iTRANENTRDETAH55);

            int iTRANENTRDETAH56 = dr.GetOrdinal(this.TRANENTRDETAH56);
            if (!dr.IsDBNull(iTRANENTRDETAH56)) entity.TranEntrDetah56 = dr.GetDecimal(iTRANENTRDETAH56);

            int iTRANENTRDETAH57 = dr.GetOrdinal(this.TRANENTRDETAH57);
            if (!dr.IsDBNull(iTRANENTRDETAH57)) entity.TranEntrDetah57 = dr.GetDecimal(iTRANENTRDETAH57);

            int iTRANENTRDETAH58 = dr.GetOrdinal(this.TRANENTRDETAH58);
            if (!dr.IsDBNull(iTRANENTRDETAH58)) entity.TranEntrDetah58 = dr.GetDecimal(iTRANENTRDETAH58);

            int iTRANENTRDETAH59 = dr.GetOrdinal(this.TRANENTRDETAH59);
            if (!dr.IsDBNull(iTRANENTRDETAH59)) entity.TranEntrDetah59 = dr.GetDecimal(iTRANENTRDETAH59);

            int iTRANENTRDETAH60 = dr.GetOrdinal(this.TRANENTRDETAH60);
            if (!dr.IsDBNull(iTRANENTRDETAH60)) entity.TranEntrDetah60 = dr.GetDecimal(iTRANENTRDETAH60);

            int iTRANENTRDETAH61 = dr.GetOrdinal(this.TRANENTRDETAH61);
            if (!dr.IsDBNull(iTRANENTRDETAH61)) entity.TranEntrDetah61 = dr.GetDecimal(iTRANENTRDETAH61);

            int iTRANENTRDETAH62 = dr.GetOrdinal(this.TRANENTRDETAH62);
            if (!dr.IsDBNull(iTRANENTRDETAH62)) entity.TranEntrDetah62 = dr.GetDecimal(iTRANENTRDETAH62);

            int iTRANENTRDETAH63 = dr.GetOrdinal(this.TRANENTRDETAH63);
            if (!dr.IsDBNull(iTRANENTRDETAH63)) entity.TranEntrDetah63 = dr.GetDecimal(iTRANENTRDETAH63);

            int iTRANENTRDETAH64 = dr.GetOrdinal(this.TRANENTRDETAH64);
            if (!dr.IsDBNull(iTRANENTRDETAH64)) entity.TranEntrDetah64 = dr.GetDecimal(iTRANENTRDETAH64);

            int iTRANENTRDETAH65 = dr.GetOrdinal(this.TRANENTRDETAH65);
            if (!dr.IsDBNull(iTRANENTRDETAH65)) entity.TranEntrDetah65 = dr.GetDecimal(iTRANENTRDETAH65);

            int iTRANENTRDETAH66 = dr.GetOrdinal(this.TRANENTRDETAH66);
            if (!dr.IsDBNull(iTRANENTRDETAH66)) entity.TranEntrDetah66 = dr.GetDecimal(iTRANENTRDETAH66);

            int iTRANENTRDETAH67 = dr.GetOrdinal(this.TRANENTRDETAH67);
            if (!dr.IsDBNull(iTRANENTRDETAH67)) entity.TranEntrDetah67 = dr.GetDecimal(iTRANENTRDETAH67);

            int iTRANENTRDETAH68 = dr.GetOrdinal(this.TRANENTRDETAH68);
            if (!dr.IsDBNull(iTRANENTRDETAH68)) entity.TranEntrDetah68 = dr.GetDecimal(iTRANENTRDETAH68);

            int iTRANENTRDETAH69 = dr.GetOrdinal(this.TRANENTRDETAH69);
            if (!dr.IsDBNull(iTRANENTRDETAH69)) entity.TranEntrDetah69 = dr.GetDecimal(iTRANENTRDETAH69);

            int iTRANENTRDETAH70 = dr.GetOrdinal(this.TRANENTRDETAH70);
            if (!dr.IsDBNull(iTRANENTRDETAH70)) entity.TranEntrDetah70 = dr.GetDecimal(iTRANENTRDETAH70);

            int iTRANENTRDETAH71 = dr.GetOrdinal(this.TRANENTRDETAH71);
            if (!dr.IsDBNull(iTRANENTRDETAH71)) entity.TranEntrDetah71 = dr.GetDecimal(iTRANENTRDETAH71);

            int iTRANENTRDETAH72 = dr.GetOrdinal(this.TRANENTRDETAH72);
            if (!dr.IsDBNull(iTRANENTRDETAH72)) entity.TranEntrDetah72 = dr.GetDecimal(iTRANENTRDETAH72);

            int iTRANENTRDETAH73 = dr.GetOrdinal(this.TRANENTRDETAH73);
            if (!dr.IsDBNull(iTRANENTRDETAH73)) entity.TranEntrDetah73 = dr.GetDecimal(iTRANENTRDETAH73);

            int iTRANENTRDETAH74 = dr.GetOrdinal(this.TRANENTRDETAH74);
            if (!dr.IsDBNull(iTRANENTRDETAH74)) entity.TranEntrDetah74 = dr.GetDecimal(iTRANENTRDETAH74);

            int iTRANENTRDETAH75 = dr.GetOrdinal(this.TRANENTRDETAH75);
            if (!dr.IsDBNull(iTRANENTRDETAH75)) entity.TranEntrDetah75 = dr.GetDecimal(iTRANENTRDETAH75);

            int iTRANENTRDETAH76 = dr.GetOrdinal(this.TRANENTRDETAH76);
            if (!dr.IsDBNull(iTRANENTRDETAH76)) entity.TranEntrDetah76 = dr.GetDecimal(iTRANENTRDETAH76);

            int iTRANENTRDETAH77 = dr.GetOrdinal(this.TRANENTRDETAH77);
            if (!dr.IsDBNull(iTRANENTRDETAH77)) entity.TranEntrDetah77 = dr.GetDecimal(iTRANENTRDETAH77);

            int iTRANENTRDETAH78 = dr.GetOrdinal(this.TRANENTRDETAH78);
            if (!dr.IsDBNull(iTRANENTRDETAH78)) entity.TranEntrDetah78 = dr.GetDecimal(iTRANENTRDETAH78);

            int iTRANENTRDETAH79 = dr.GetOrdinal(this.TRANENTRDETAH79);
            if (!dr.IsDBNull(iTRANENTRDETAH79)) entity.TranEntrDetah79 = dr.GetDecimal(iTRANENTRDETAH79);

            int iTRANENTRDETAH80 = dr.GetOrdinal(this.TRANENTRDETAH80);
            if (!dr.IsDBNull(iTRANENTRDETAH80)) entity.TranEntrDetah80 = dr.GetDecimal(iTRANENTRDETAH80);

            int iTRANENTRDETAH81 = dr.GetOrdinal(this.TRANENTRDETAH81);
            if (!dr.IsDBNull(iTRANENTRDETAH81)) entity.TranEntrDetah81 = dr.GetDecimal(iTRANENTRDETAH81);

            int iTRANENTRDETAH82 = dr.GetOrdinal(this.TRANENTRDETAH82);
            if (!dr.IsDBNull(iTRANENTRDETAH82)) entity.TranEntrDetah82 = dr.GetDecimal(iTRANENTRDETAH82);

            int iTRANENTRDETAH83 = dr.GetOrdinal(this.TRANENTRDETAH83);
            if (!dr.IsDBNull(iTRANENTRDETAH83)) entity.TranEntrDetah83 = dr.GetDecimal(iTRANENTRDETAH83);

            int iTRANENTRDETAH84 = dr.GetOrdinal(this.TRANENTRDETAH84);
            if (!dr.IsDBNull(iTRANENTRDETAH84)) entity.TranEntrDetah84 = dr.GetDecimal(iTRANENTRDETAH84);

            int iTRANENTRDETAH85 = dr.GetOrdinal(this.TRANENTRDETAH85);
            if (!dr.IsDBNull(iTRANENTRDETAH85)) entity.TranEntrDetah85 = dr.GetDecimal(iTRANENTRDETAH85);

            int iTRANENTRDETAH86 = dr.GetOrdinal(this.TRANENTRDETAH86);
            if (!dr.IsDBNull(iTRANENTRDETAH86)) entity.TranEntrDetah86 = dr.GetDecimal(iTRANENTRDETAH86);

            int iTRANENTRDETAH87 = dr.GetOrdinal(this.TRANENTRDETAH87);
            if (!dr.IsDBNull(iTRANENTRDETAH87)) entity.TranEntrDetah87 = dr.GetDecimal(iTRANENTRDETAH87);

            int iTRANENTRDETAH88 = dr.GetOrdinal(this.TRANENTRDETAH88);
            if (!dr.IsDBNull(iTRANENTRDETAH88)) entity.TranEntrDetah88 = dr.GetDecimal(iTRANENTRDETAH88);

            int iTRANENTRDETAH89 = dr.GetOrdinal(this.TRANENTRDETAH89);
            if (!dr.IsDBNull(iTRANENTRDETAH89)) entity.TranEntrDetah89 = dr.GetDecimal(iTRANENTRDETAH89);

            int iTRANENTRDETAH90 = dr.GetOrdinal(this.TRANENTRDETAH90);
            if (!dr.IsDBNull(iTRANENTRDETAH90)) entity.TranEntrDetah90 = dr.GetDecimal(iTRANENTRDETAH90);

            int iTRANENTRDETAH91 = dr.GetOrdinal(this.TRANENTRDETAH91);
            if (!dr.IsDBNull(iTRANENTRDETAH91)) entity.TranEntrDetah91 = dr.GetDecimal(iTRANENTRDETAH91);

            int iTRANENTRDETAH92 = dr.GetOrdinal(this.TRANENTRDETAH92);
            if (!dr.IsDBNull(iTRANENTRDETAH92)) entity.TranEntrDetah92 = dr.GetDecimal(iTRANENTRDETAH92);

            int iTRANENTRDETAH93 = dr.GetOrdinal(this.TRANENTRDETAH93);
            if (!dr.IsDBNull(iTRANENTRDETAH93)) entity.TranEntrDetah93 = dr.GetDecimal(iTRANENTRDETAH93);

            int iTRANENTRDETAH94 = dr.GetOrdinal(this.TRANENTRDETAH94);
            if (!dr.IsDBNull(iTRANENTRDETAH94)) entity.TranEntrDetah94 = dr.GetDecimal(iTRANENTRDETAH94);

            int iTRANENTRDETAH95 = dr.GetOrdinal(this.TRANENTRDETAH95);
            if (!dr.IsDBNull(iTRANENTRDETAH95)) entity.TranEntrDetah95 = dr.GetDecimal(iTRANENTRDETAH95);

            int iTRANENTRDETAH96 = dr.GetOrdinal(this.TRANENTRDETAH96);
            if (!dr.IsDBNull(iTRANENTRDETAH96)) entity.TranEntrDetah96 = dr.GetDecimal(iTRANENTRDETAH96);

            int iTENTDEUSERNAME = dr.GetOrdinal(this.TENTDEUSERNAME);
            if (!dr.IsDBNull(iTENTDEUSERNAME)) entity.TentdeUserName = dr.GetString(iTENTDEUSERNAME);

            int iTRANENTRDETAFECINS = dr.GetOrdinal(this.TRANENTRDETAFECINS);
            if (!dr.IsDBNull(iTRANENTRDETAFECINS)) entity.TranEntrDetaFecIns = dr.GetDateTime(iTRANENTRDETAFECINS);

            int iTRANENTRDETAFECACT = dr.GetOrdinal(this.TRANENTRDETAFECACT);
            if (!dr.IsDBNull(iTRANENTRDETAFECACT)) entity.TranEntrDetaFecAct = dr.GetDateTime(iTRANENTRDETAFECACT);

            return entity;
        }


        #region Mapeo de Campos

        public string TRANENTRCODI = "TENTCODI";
        public string TRANENTRDETACODI = "TENTDECODI";
        public string TRANENTRDETAVERSION = "TENTDEVERSION";
        public string TRANENTRDETADIA = "TENTDEDIA";
        public string TRANENTRDETAPROMDIA = "TENTDEPROMEDIODIA";
        public string TRANENTRDETASUMADIA = "TENTDESUMADIA";
        public string TRANENTRDETAH1 = "TENTDE1";
        public string TRANENTRDETAH2 = "TENTDE2";
        public string TRANENTRDETAH3 = "TENTDE3";
        public string TRANENTRDETAH4 = "TENTDE4";
        public string TRANENTRDETAH5 = "TENTDE5";
        public string TRANENTRDETAH6 = "TENTDE6";
        public string TRANENTRDETAH7 = "TENTDE7";
        public string TRANENTRDETAH8 = "TENTDE8";
        public string TRANENTRDETAH9 = "TENTDE9";
        public string TRANENTRDETAH10 = "TENTDE10";
        public string TRANENTRDETAH11 = "TENTDE11";
        public string TRANENTRDETAH12 = "TENTDE12";
        public string TRANENTRDETAH13 = "TENTDE13";
        public string TRANENTRDETAH14 = "TENTDE14";
        public string TRANENTRDETAH15 = "TENTDE15";
        public string TRANENTRDETAH16 = "TENTDE16";
        public string TRANENTRDETAH17 = "TENTDE17";
        public string TRANENTRDETAH18 = "TENTDE18";
        public string TRANENTRDETAH19 = "TENTDE19";
        public string TRANENTRDETAH20 = "TENTDE20";
        public string TRANENTRDETAH21 = "TENTDE21";
        public string TRANENTRDETAH22 = "TENTDE22";
        public string TRANENTRDETAH23 = "TENTDE23";
        public string TRANENTRDETAH24 = "TENTDE24";
        public string TRANENTRDETAH25 = "TENTDE25";
        public string TRANENTRDETAH26 = "TENTDE26";
        public string TRANENTRDETAH27 = "TENTDE27";
        public string TRANENTRDETAH28 = "TENTDE28";
        public string TRANENTRDETAH29 = "TENTDE29";
        public string TRANENTRDETAH30 = "TENTDE30";
        public string TRANENTRDETAH31 = "TENTDE31";
        public string TRANENTRDETAH32 = "TENTDE32";
        public string TRANENTRDETAH33 = "TENTDE33";
        public string TRANENTRDETAH34 = "TENTDE34";
        public string TRANENTRDETAH35 = "TENTDE35";
        public string TRANENTRDETAH36 = "TENTDE36";
        public string TRANENTRDETAH37 = "TENTDE37";
        public string TRANENTRDETAH38 = "TENTDE38";
        public string TRANENTRDETAH39 = "TENTDE39";
        public string TRANENTRDETAH40 = "TENTDE40";
        public string TRANENTRDETAH41 = "TENTDE41";
        public string TRANENTRDETAH42 = "TENTDE42";
        public string TRANENTRDETAH43 = "TENTDE43";
        public string TRANENTRDETAH44 = "TENTDE44";
        public string TRANENTRDETAH45 = "TENTDE45";
        public string TRANENTRDETAH46 = "TENTDE46";
        public string TRANENTRDETAH47 = "TENTDE47";
        public string TRANENTRDETAH48 = "TENTDE48";
        public string TRANENTRDETAH49 = "TENTDE49";
        public string TRANENTRDETAH50 = "TENTDE50";
        public string TRANENTRDETAH51 = "TENTDE51";
        public string TRANENTRDETAH52 = "TENTDE52";
        public string TRANENTRDETAH53 = "TENTDE53";
        public string TRANENTRDETAH54 = "TENTDE54";
        public string TRANENTRDETAH55 = "TENTDE55";
        public string TRANENTRDETAH56 = "TENTDE56";
        public string TRANENTRDETAH57 = "TENTDE57";
        public string TRANENTRDETAH58 = "TENTDE58";
        public string TRANENTRDETAH59 = "TENTDE59";
        public string TRANENTRDETAH60 = "TENTDE60";
        public string TRANENTRDETAH61 = "TENTDE61";
        public string TRANENTRDETAH62 = "TENTDE62";
        public string TRANENTRDETAH63 = "TENTDE63";
        public string TRANENTRDETAH64 = "TENTDE64";
        public string TRANENTRDETAH65 = "TENTDE65";
        public string TRANENTRDETAH66 = "TENTDE66";
        public string TRANENTRDETAH67 = "TENTDE67";
        public string TRANENTRDETAH68 = "TENTDE68";
        public string TRANENTRDETAH69 = "TENTDE69";
        public string TRANENTRDETAH70 = "TENTDE70";
        public string TRANENTRDETAH71 = "TENTDE71";
        public string TRANENTRDETAH72 = "TENTDE72";
        public string TRANENTRDETAH73 = "TENTDE73";
        public string TRANENTRDETAH74 = "TENTDE74";
        public string TRANENTRDETAH75 = "TENTDE75";
        public string TRANENTRDETAH76 = "TENTDE76";
        public string TRANENTRDETAH77 = "TENTDE77";
        public string TRANENTRDETAH78 = "TENTDE78";
        public string TRANENTRDETAH79 = "TENTDE79";
        public string TRANENTRDETAH80 = "TENTDE80";
        public string TRANENTRDETAH81 = "TENTDE81";
        public string TRANENTRDETAH82 = "TENTDE82";
        public string TRANENTRDETAH83 = "TENTDE83";
        public string TRANENTRDETAH84 = "TENTDE84";
        public string TRANENTRDETAH85 = "TENTDE85";
        public string TRANENTRDETAH86 = "TENTDE86";
        public string TRANENTRDETAH87 = "TENTDE87";
        public string TRANENTRDETAH88 = "TENTDE88";
        public string TRANENTRDETAH89 = "TENTDE89";
        public string TRANENTRDETAH90 = "TENTDE90";
        public string TRANENTRDETAH91 = "TENTDE91";
        public string TRANENTRDETAH92 = "TENTDE92";
        public string TRANENTRDETAH93 = "TENTDE93";
        public string TRANENTRDETAH94 = "TENTDE94";
        public string TRANENTRDETAH95 = "TENTDE95";
        public string TRANENTRDETAH96 = "TENTDE96";
        public string TENTDEUSERNAME = "TENTDEUSERNAME";
        public string TRANENTRDETAFECINS = "TENTDEFECINS";
        public string TRANENTRDETAFECACT = "TENTDEFECACT";
        //para los parametros 
        public string PERICODI = "PERICODI";
        public string EMPRCODI = "EMPRCODI";
        public string TRANENTRVERSION = "TENTVERSION";
        //parametro espcial  para ListTransEntrTransReti
        public string FLAGTIPO = "TIPO";
        //por mientras del list cabe
        public string TENTCODIGO = "TENTCODIGO";
        public string BARRCODI = "BARRCODI";
        public string CENTGENECODI = "EQUICODI";
        public string CODIENTRCODIGO = "TENTCODIGO";
        public string TRANENTRTIPOINFORMACION = "TENTTIPOINFORMACION";
        public string ENERGIA = "ENERGIA";
        public string NOMBBARRA = "NOMBBARRA";
        public string NOMBEMPRESA = "EMPRNOMB";
        public string TIPOINFORMACION = "TIPOINFORMACION";
        public string TableName = "TRN_TRANS_ENTREGA_DETALLE";
        #region SIOSEIN 2
        public string BARRNOMBRE = "BARRNOMBRE";
        #endregion
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

        public string SqlListByTransferenciaEntrega
        {
            get { return base.GetSqlXml("ListByTransferenciaEntrega"); }
        }

        public string SqlDeleteListaTransferenciaEntregaDetalle
        {
            get { return base.GetSqlXml("DeleteListaTransferenciaEntregaDetalle"); }
        }

        public string SqlListTransEntrTransReti
        {
            get { return base.GetSqlXml("ListTransEntrTransReti"); }
        }

        public string SqlListarCodigoReportado
        {
            get { return base.GetSqlXml("ListarCodigoReportado"); }
        }

        public string SqlListTransEntrPorPericodiYVersion
        {
            get { return base.GetSqlXml("ListTransEntrPorPericodiYVersion"); }
        }

        public string SqlGetDemandaByCodVtea
        {
            get { return base.GetSqlXml("GetDemandaByCodVtea"); }
        }
        public string SqlGetDemandaByCodVteaEmpresa
        {
            get { return base.GetSqlXml("GetDemandaByCodVteaEmpresa"); }
        }
    }
}
