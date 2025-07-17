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
    /// Clase que contiene el mapeo de la tabla trn_trans_retiro_detalle
    /// </summary>    
    public class TransferenciaRetiroDetalleHelper : HelperBase
    {
        public TransferenciaRetiroDetalleHelper()
            : base(Consultas.TransferenciaRetiroDetalleSql)
        {
        }

        public TransferenciaRetiroDetalleDTO Create(IDataReader dr)
        {
            TransferenciaRetiroDetalleDTO entity = new TransferenciaRetiroDetalleDTO();

            int iTRANRETICODI = dr.GetOrdinal(this.TRANRETICODI);
            if (!dr.IsDBNull(iTRANRETICODI)) entity.TranRetiCodi = dr.GetInt32(iTRANRETICODI);

            int iTRANRETIDETACODI = dr.GetOrdinal(this.TRANRETIDETACODI);
            if (!dr.IsDBNull(iTRANRETIDETACODI)) entity.TranRetiDetaCodi = dr.GetDecimal(iTRANRETIDETACODI);

            int iTRANRETIDETAVERSION = dr.GetOrdinal(this.TRANRETIDETAVERSION);
            if (!dr.IsDBNull(iTRANRETIDETAVERSION)) entity.TranRetiDetaVersion = dr.GetInt32(iTRANRETIDETAVERSION);

            int iTRANRETIDETADIA = dr.GetOrdinal(this.TRANRETIDETADIA);
            if (!dr.IsDBNull(iTRANRETIDETADIA)) entity.TranRetiDetaDia = dr.GetInt32(iTRANRETIDETADIA);

            int iTRANRETIDETAPROMDIA = dr.GetOrdinal(this.TRANRETIDETAPROMDIA);
            if (!dr.IsDBNull(iTRANRETIDETAPROMDIA)) entity.TranRetiDetaPromDia = dr.GetDecimal(iTRANRETIDETAPROMDIA);

            int iTRANRETIDETASUMADIA = dr.GetOrdinal(this.TRANRETIDETASUMADIA);
            if (!dr.IsDBNull(iTRANRETIDETASUMADIA)) entity.TranRetiDetaSumaDia = dr.GetDecimal(iTRANRETIDETASUMADIA);

            int iTRANRETIDETAH1 = dr.GetOrdinal(this.TRANRETIDETAH1);
            if (!dr.IsDBNull(iTRANRETIDETAH1)) entity.TranRetiDetah1 = dr.GetDecimal(iTRANRETIDETAH1);

            int iTRANRETIDETAH2 = dr.GetOrdinal(this.TRANRETIDETAH2);
            if (!dr.IsDBNull(iTRANRETIDETAH2)) entity.TranRetiDetah2 = dr.GetDecimal(iTRANRETIDETAH2);

            int iTRANRETIDETAH3 = dr.GetOrdinal(this.TRANRETIDETAH3);
            if (!dr.IsDBNull(iTRANRETIDETAH3)) entity.TranRetiDetah3 = dr.GetDecimal(iTRANRETIDETAH3);

            int iTRANRETIDETAH4 = dr.GetOrdinal(this.TRANRETIDETAH4);
            if (!dr.IsDBNull(iTRANRETIDETAH4)) entity.TranRetiDetah4 = dr.GetDecimal(iTRANRETIDETAH4);

            int iTRANRETIDETAH5 = dr.GetOrdinal(this.TRANRETIDETAH5);
            if (!dr.IsDBNull(iTRANRETIDETAH5)) entity.TranRetiDetah5 = dr.GetDecimal(iTRANRETIDETAH5);

            int iTRANRETIDETAH6 = dr.GetOrdinal(this.TRANRETIDETAH6);
            if (!dr.IsDBNull(iTRANRETIDETAH6)) entity.TranRetiDetah6 = dr.GetDecimal(iTRANRETIDETAH6);

            int iTRANRETIDETAH7 = dr.GetOrdinal(this.TRANRETIDETAH7);
            if (!dr.IsDBNull(iTRANRETIDETAH7)) entity.TranRetiDetah7 = dr.GetDecimal(iTRANRETIDETAH7);

            int iTRANRETIDETAH8 = dr.GetOrdinal(this.TRANRETIDETAH8);
            if (!dr.IsDBNull(iTRANRETIDETAH8)) entity.TranRetiDetah8 = dr.GetDecimal(iTRANRETIDETAH8);

            int iTRANRETIDETAH9 = dr.GetOrdinal(this.TRANRETIDETAH9);
            if (!dr.IsDBNull(iTRANRETIDETAH9)) entity.TranRetiDetah9 = dr.GetDecimal(iTRANRETIDETAH9);

            int iTRANRETIDETAH10 = dr.GetOrdinal(this.TRANRETIDETAH10);
            if (!dr.IsDBNull(iTRANRETIDETAH10)) entity.TranRetiDetah10 = dr.GetDecimal(iTRANRETIDETAH10);

            int iTRANRETIDETAH11 = dr.GetOrdinal(this.TRANRETIDETAH11);
            if (!dr.IsDBNull(iTRANRETIDETAH11)) entity.TranRetiDetah11 = dr.GetDecimal(iTRANRETIDETAH11);

            int iTRANRETIDETAH12 = dr.GetOrdinal(this.TRANRETIDETAH12);
            if (!dr.IsDBNull(iTRANRETIDETAH12)) entity.TranRetiDetah12 = dr.GetDecimal(iTRANRETIDETAH12);

            int iTRANRETIDETAH13 = dr.GetOrdinal(this.TRANRETIDETAH13);
            if (!dr.IsDBNull(iTRANRETIDETAH13)) entity.TranRetiDetah13 = dr.GetDecimal(iTRANRETIDETAH13);

            int iTRANRETIDETAH14 = dr.GetOrdinal(this.TRANRETIDETAH14);
            if (!dr.IsDBNull(iTRANRETIDETAH14)) entity.TranRetiDetah14 = dr.GetDecimal(iTRANRETIDETAH14);

            int iTRANRETIDETAH15 = dr.GetOrdinal(this.TRANRETIDETAH15);
            if (!dr.IsDBNull(iTRANRETIDETAH15)) entity.TranRetiDetah15 = dr.GetDecimal(iTRANRETIDETAH15);

            int iTRANRETIDETAH16 = dr.GetOrdinal(this.TRANRETIDETAH16);
            if (!dr.IsDBNull(iTRANRETIDETAH16)) entity.TranRetiDetah16 = dr.GetDecimal(iTRANRETIDETAH16);

            int iTRANRETIDETAH17 = dr.GetOrdinal(this.TRANRETIDETAH17);
            if (!dr.IsDBNull(iTRANRETIDETAH17)) entity.TranRetiDetah17 = dr.GetDecimal(iTRANRETIDETAH17);

            int iTRANRETIDETAH18 = dr.GetOrdinal(this.TRANRETIDETAH18);
            if (!dr.IsDBNull(iTRANRETIDETAH18)) entity.TranRetiDetah18 = dr.GetDecimal(iTRANRETIDETAH18);

            int iTRANRETIDETAH19 = dr.GetOrdinal(this.TRANRETIDETAH19);
            if (!dr.IsDBNull(iTRANRETIDETAH19)) entity.TranRetiDetah19 = dr.GetDecimal(iTRANRETIDETAH19);

            int iTRANRETIDETAH20 = dr.GetOrdinal(this.TRANRETIDETAH20);
            if (!dr.IsDBNull(iTRANRETIDETAH20)) entity.TranRetiDetah20 = dr.GetDecimal(iTRANRETIDETAH20);

            int iTRANRETIDETAH21 = dr.GetOrdinal(this.TRANRETIDETAH21);
            if (!dr.IsDBNull(iTRANRETIDETAH21)) entity.TranRetiDetah21 = dr.GetDecimal(iTRANRETIDETAH21);

            int iTRANRETIDETAH22 = dr.GetOrdinal(this.TRANRETIDETAH22);
            if (!dr.IsDBNull(iTRANRETIDETAH22)) entity.TranRetiDetah22 = dr.GetDecimal(iTRANRETIDETAH22);

            int iTRANRETIDETAH23 = dr.GetOrdinal(this.TRANRETIDETAH23);
            if (!dr.IsDBNull(iTRANRETIDETAH23)) entity.TranRetiDetah23 = dr.GetDecimal(iTRANRETIDETAH23);

            int iTRANRETIDETAH24 = dr.GetOrdinal(this.TRANRETIDETAH24);
            if (!dr.IsDBNull(iTRANRETIDETAH24)) entity.TranRetiDetah24 = dr.GetDecimal(iTRANRETIDETAH24);

            int iTRANRETIDETAH25 = dr.GetOrdinal(this.TRANRETIDETAH25);
            if (!dr.IsDBNull(iTRANRETIDETAH25)) entity.TranRetiDetah25 = dr.GetDecimal(iTRANRETIDETAH25);

            int iTRANRETIDETAH26 = dr.GetOrdinal(this.TRANRETIDETAH26);
            if (!dr.IsDBNull(iTRANRETIDETAH26)) entity.TranRetiDetah26 = dr.GetDecimal(iTRANRETIDETAH26);

            int iTRANRETIDETAH27 = dr.GetOrdinal(this.TRANRETIDETAH27);
            if (!dr.IsDBNull(iTRANRETIDETAH27)) entity.TranRetiDetah27 = dr.GetDecimal(iTRANRETIDETAH27);

            int iTRANRETIDETAH28 = dr.GetOrdinal(this.TRANRETIDETAH28);
            if (!dr.IsDBNull(iTRANRETIDETAH28)) entity.TranRetiDetah28 = dr.GetDecimal(iTRANRETIDETAH28);

            int iTRANRETIDETAH29 = dr.GetOrdinal(this.TRANRETIDETAH29);
            if (!dr.IsDBNull(iTRANRETIDETAH29)) entity.TranRetiDetah29 = dr.GetDecimal(iTRANRETIDETAH29);

            int iTRANRETIDETAH30 = dr.GetOrdinal(this.TRANRETIDETAH30);
            if (!dr.IsDBNull(iTRANRETIDETAH30)) entity.TranRetiDetah30 = dr.GetDecimal(iTRANRETIDETAH30);

            int iTRANRETIDETAH31 = dr.GetOrdinal(this.TRANRETIDETAH31);
            if (!dr.IsDBNull(iTRANRETIDETAH31)) entity.TranRetiDetah31 = dr.GetDecimal(iTRANRETIDETAH31);

            int iTRANRETIDETAH32 = dr.GetOrdinal(this.TRANRETIDETAH32);
            if (!dr.IsDBNull(iTRANRETIDETAH32)) entity.TranRetiDetah32 = dr.GetDecimal(iTRANRETIDETAH32);

            int iTRANRETIDETAH33 = dr.GetOrdinal(this.TRANRETIDETAH33);
            if (!dr.IsDBNull(iTRANRETIDETAH33)) entity.TranRetiDetah33 = dr.GetDecimal(iTRANRETIDETAH33);

            int iTRANRETIDETAH34 = dr.GetOrdinal(this.TRANRETIDETAH34);
            if (!dr.IsDBNull(iTRANRETIDETAH34)) entity.TranRetiDetah34 = dr.GetDecimal(iTRANRETIDETAH34);

            int iTRANRETIDETAH35 = dr.GetOrdinal(this.TRANRETIDETAH35);
            if (!dr.IsDBNull(iTRANRETIDETAH35)) entity.TranRetiDetah35 = dr.GetDecimal(iTRANRETIDETAH35);

            int iTRANRETIDETAH36 = dr.GetOrdinal(this.TRANRETIDETAH36);
            if (!dr.IsDBNull(iTRANRETIDETAH36)) entity.TranRetiDetah36 = dr.GetDecimal(iTRANRETIDETAH36);

            int iTRANRETIDETAH37 = dr.GetOrdinal(this.TRANRETIDETAH37);
            if (!dr.IsDBNull(iTRANRETIDETAH37)) entity.TranRetiDetah37 = dr.GetDecimal(iTRANRETIDETAH37);

            int iTRANRETIDETAH38 = dr.GetOrdinal(this.TRANRETIDETAH38);
            if (!dr.IsDBNull(iTRANRETIDETAH38)) entity.TranRetiDetah38 = dr.GetDecimal(iTRANRETIDETAH38);

            int iTRANRETIDETAH39 = dr.GetOrdinal(this.TRANRETIDETAH39);
            if (!dr.IsDBNull(iTRANRETIDETAH39)) entity.TranRetiDetah39 = dr.GetDecimal(iTRANRETIDETAH39);

            int iTRANRETIDETAH40 = dr.GetOrdinal(this.TRANRETIDETAH40);
            if (!dr.IsDBNull(iTRANRETIDETAH40)) entity.TranRetiDetah40 = dr.GetDecimal(iTRANRETIDETAH40);

            int iTRANRETIDETAH41 = dr.GetOrdinal(this.TRANRETIDETAH41);
            if (!dr.IsDBNull(iTRANRETIDETAH41)) entity.TranRetiDetah41 = dr.GetDecimal(iTRANRETIDETAH41);

            int iTRANRETIDETAH42 = dr.GetOrdinal(this.TRANRETIDETAH42);
            if (!dr.IsDBNull(iTRANRETIDETAH42)) entity.TranRetiDetah42 = dr.GetDecimal(iTRANRETIDETAH42);

            int iTRANRETIDETAH43 = dr.GetOrdinal(this.TRANRETIDETAH43);
            if (!dr.IsDBNull(iTRANRETIDETAH43)) entity.TranRetiDetah43 = dr.GetDecimal(iTRANRETIDETAH43);

            int iTRANRETIDETAH44 = dr.GetOrdinal(this.TRANRETIDETAH44);
            if (!dr.IsDBNull(iTRANRETIDETAH44)) entity.TranRetiDetah44 = dr.GetDecimal(iTRANRETIDETAH44);

            int iTRANRETIDETAH45 = dr.GetOrdinal(this.TRANRETIDETAH45);
            if (!dr.IsDBNull(iTRANRETIDETAH45)) entity.TranRetiDetah45 = dr.GetDecimal(iTRANRETIDETAH45);

            int iTRANRETIDETAH46 = dr.GetOrdinal(this.TRANRETIDETAH46);
            if (!dr.IsDBNull(iTRANRETIDETAH46)) entity.TranRetiDetah46 = dr.GetDecimal(iTRANRETIDETAH46);

            int iTRANRETIDETAH47 = dr.GetOrdinal(this.TRANRETIDETAH47);
            if (!dr.IsDBNull(iTRANRETIDETAH47)) entity.TranRetiDetah47 = dr.GetDecimal(iTRANRETIDETAH47);

            int iTRANRETIDETAH48 = dr.GetOrdinal(this.TRANRETIDETAH48);
            if (!dr.IsDBNull(iTRANRETIDETAH48)) entity.TranRetiDetah48 = dr.GetDecimal(iTRANRETIDETAH48);

            int iTRANRETIDETAH49 = dr.GetOrdinal(this.TRANRETIDETAH49);
            if (!dr.IsDBNull(iTRANRETIDETAH49)) entity.TranRetiDetah49 = dr.GetDecimal(iTRANRETIDETAH49);

            int iTRANRETIDETAH50 = dr.GetOrdinal(this.TRANRETIDETAH50);
            if (!dr.IsDBNull(iTRANRETIDETAH50)) entity.TranRetiDetah50 = dr.GetDecimal(iTRANRETIDETAH50);

            int iTRANRETIDETAH51 = dr.GetOrdinal(this.TRANRETIDETAH51);
            if (!dr.IsDBNull(iTRANRETIDETAH50)) entity.TranRetiDetah51 = dr.GetDecimal(iTRANRETIDETAH51);

            int iTRANRETIDETAH52 = dr.GetOrdinal(this.TRANRETIDETAH52);
            if (!dr.IsDBNull(iTRANRETIDETAH52)) entity.TranRetiDetah52 = dr.GetDecimal(iTRANRETIDETAH52);

            int iTRANRETIDETAH53 = dr.GetOrdinal(this.TRANRETIDETAH53);
            if (!dr.IsDBNull(iTRANRETIDETAH53)) entity.TranRetiDetah53 = dr.GetDecimal(iTRANRETIDETAH53);

            int iTRANRETIDETAH54 = dr.GetOrdinal(this.TRANRETIDETAH54);
            if (!dr.IsDBNull(iTRANRETIDETAH54)) entity.TranRetiDetah54 = dr.GetDecimal(iTRANRETIDETAH54);

            int iTRANRETIDETAH55 = dr.GetOrdinal(this.TRANRETIDETAH55);
            if (!dr.IsDBNull(iTRANRETIDETAH55)) entity.TranRetiDetah55 = dr.GetDecimal(iTRANRETIDETAH55);

            int iTRANRETIDETAH56 = dr.GetOrdinal(this.TRANRETIDETAH56);
            if (!dr.IsDBNull(iTRANRETIDETAH56)) entity.TranRetiDetah56 = dr.GetDecimal(iTRANRETIDETAH56);

            int iTRANRETIDETAH57 = dr.GetOrdinal(this.TRANRETIDETAH57);
            if (!dr.IsDBNull(iTRANRETIDETAH57)) entity.TranRetiDetah57 = dr.GetDecimal(iTRANRETIDETAH57);

            int iTRANRETIDETAH58 = dr.GetOrdinal(this.TRANRETIDETAH58);
            if (!dr.IsDBNull(iTRANRETIDETAH58)) entity.TranRetiDetah58 = dr.GetDecimal(iTRANRETIDETAH58);

            int iTRANRETIDETAH59 = dr.GetOrdinal(this.TRANRETIDETAH59);
            if (!dr.IsDBNull(iTRANRETIDETAH59)) entity.TranRetiDetah59 = dr.GetDecimal(iTRANRETIDETAH59);

            int iTRANRETIDETAH60 = dr.GetOrdinal(this.TRANRETIDETAH60);
            if (!dr.IsDBNull(iTRANRETIDETAH60)) entity.TranRetiDetah60 = dr.GetDecimal(iTRANRETIDETAH60);

            int iTRANRETIDETAH61 = dr.GetOrdinal(this.TRANRETIDETAH61);
            if (!dr.IsDBNull(iTRANRETIDETAH61)) entity.TranRetiDetah61 = dr.GetDecimal(iTRANRETIDETAH61);

            int iTRANRETIDETAH62 = dr.GetOrdinal(this.TRANRETIDETAH62);
            if (!dr.IsDBNull(iTRANRETIDETAH62)) entity.TranRetiDetah62 = dr.GetDecimal(iTRANRETIDETAH62);

            int iTRANRETIDETAH63 = dr.GetOrdinal(this.TRANRETIDETAH63);
            if (!dr.IsDBNull(iTRANRETIDETAH63)) entity.TranRetiDetah63 = dr.GetDecimal(iTRANRETIDETAH63);

            int iTRANRETIDETAH64 = dr.GetOrdinal(this.TRANRETIDETAH64);
            if (!dr.IsDBNull(iTRANRETIDETAH64)) entity.TranRetiDetah64 = dr.GetDecimal(iTRANRETIDETAH64);

            int iTRANRETIDETAH65 = dr.GetOrdinal(this.TRANRETIDETAH65);
            if (!dr.IsDBNull(iTRANRETIDETAH65)) entity.TranRetiDetah65 = dr.GetDecimal(iTRANRETIDETAH65);

            int iTRANRETIDETAH66 = dr.GetOrdinal(this.TRANRETIDETAH66);
            if (!dr.IsDBNull(iTRANRETIDETAH66)) entity.TranRetiDetah66 = dr.GetDecimal(iTRANRETIDETAH66);

            int iTRANRETIDETAH67 = dr.GetOrdinal(this.TRANRETIDETAH67);
            if (!dr.IsDBNull(iTRANRETIDETAH67)) entity.TranRetiDetah67 = dr.GetDecimal(iTRANRETIDETAH67);

            int iTRANRETIDETAH68 = dr.GetOrdinal(this.TRANRETIDETAH68);
            if (!dr.IsDBNull(iTRANRETIDETAH68)) entity.TranRetiDetah68 = dr.GetDecimal(iTRANRETIDETAH68);

            int iTRANRETIDETAH69 = dr.GetOrdinal(this.TRANRETIDETAH69);
            if (!dr.IsDBNull(iTRANRETIDETAH69)) entity.TranRetiDetah69 = dr.GetDecimal(iTRANRETIDETAH69);

            int iTRANRETIDETAH70 = dr.GetOrdinal(this.TRANRETIDETAH70);
            if (!dr.IsDBNull(iTRANRETIDETAH70)) entity.TranRetiDetah70 = dr.GetDecimal(iTRANRETIDETAH70);

            int iTRANRETIDETAH71 = dr.GetOrdinal(this.TRANRETIDETAH71);
            if (!dr.IsDBNull(iTRANRETIDETAH71)) entity.TranRetiDetah71 = dr.GetDecimal(iTRANRETIDETAH71);

            int iTRANRETIDETAH72 = dr.GetOrdinal(this.TRANRETIDETAH72);
            if (!dr.IsDBNull(iTRANRETIDETAH72)) entity.TranRetiDetah72 = dr.GetDecimal(iTRANRETIDETAH72);

            int iTRANRETIDETAH73 = dr.GetOrdinal(this.TRANRETIDETAH73);
            if (!dr.IsDBNull(iTRANRETIDETAH73)) entity.TranRetiDetah73 = dr.GetDecimal(iTRANRETIDETAH73);

            int iTRANRETIDETAH74 = dr.GetOrdinal(this.TRANRETIDETAH74);
            if (!dr.IsDBNull(iTRANRETIDETAH74)) entity.TranRetiDetah74 = dr.GetDecimal(iTRANRETIDETAH74);

            int iTRANRETIDETAH75 = dr.GetOrdinal(this.TRANRETIDETAH75);
            if (!dr.IsDBNull(iTRANRETIDETAH75)) entity.TranRetiDetah75 = dr.GetDecimal(iTRANRETIDETAH75);

            int iTRANRETIDETAH76 = dr.GetOrdinal(this.TRANRETIDETAH76);
            if (!dr.IsDBNull(iTRANRETIDETAH76)) entity.TranRetiDetah76 = dr.GetDecimal(iTRANRETIDETAH76);

            int iTRANRETIDETAH77 = dr.GetOrdinal(this.TRANRETIDETAH77);
            if (!dr.IsDBNull(iTRANRETIDETAH77)) entity.TranRetiDetah77 = dr.GetDecimal(iTRANRETIDETAH77);

            int iTRANRETIDETAH78 = dr.GetOrdinal(this.TRANRETIDETAH78);
            if (!dr.IsDBNull(iTRANRETIDETAH78)) entity.TranRetiDetah78 = dr.GetDecimal(iTRANRETIDETAH78);

            int iTRANRETIDETAH79 = dr.GetOrdinal(this.TRANRETIDETAH79);
            if (!dr.IsDBNull(iTRANRETIDETAH79)) entity.TranRetiDetah79 = dr.GetDecimal(iTRANRETIDETAH79);

            int iTRANRETIDETAH80 = dr.GetOrdinal(this.TRANRETIDETAH80);
            if (!dr.IsDBNull(iTRANRETIDETAH80)) entity.TranRetiDetah80 = dr.GetDecimal(iTRANRETIDETAH80);

            int iTRANRETIDETAH81 = dr.GetOrdinal(this.TRANRETIDETAH81);
            if (!dr.IsDBNull(iTRANRETIDETAH81)) entity.TranRetiDetah81 = dr.GetDecimal(iTRANRETIDETAH81);

            int iTRANRETIDETAH82 = dr.GetOrdinal(this.TRANRETIDETAH82);
            if (!dr.IsDBNull(iTRANRETIDETAH82)) entity.TranRetiDetah82 = dr.GetDecimal(iTRANRETIDETAH82);

            int iTRANRETIDETAH83 = dr.GetOrdinal(this.TRANRETIDETAH83);
            if (!dr.IsDBNull(iTRANRETIDETAH83)) entity.TranRetiDetah83 = dr.GetDecimal(iTRANRETIDETAH83);

            int iTRANRETIDETAH84 = dr.GetOrdinal(this.TRANRETIDETAH84);
            if (!dr.IsDBNull(iTRANRETIDETAH84)) entity.TranRetiDetah84 = dr.GetDecimal(iTRANRETIDETAH84);

            int iTRANRETIDETAH85 = dr.GetOrdinal(this.TRANRETIDETAH85);
            if (!dr.IsDBNull(iTRANRETIDETAH85)) entity.TranRetiDetah85 = dr.GetDecimal(iTRANRETIDETAH85);

            int iTRANRETIDETAH86 = dr.GetOrdinal(this.TRANRETIDETAH86);
            if (!dr.IsDBNull(iTRANRETIDETAH86)) entity.TranRetiDetah86 = dr.GetDecimal(iTRANRETIDETAH86);

            int iTRANRETIDETAH87 = dr.GetOrdinal(this.TRANRETIDETAH87);
            if (!dr.IsDBNull(iTRANRETIDETAH87)) entity.TranRetiDetah87 = dr.GetDecimal(iTRANRETIDETAH87);

            int iTRANRETIDETAH88 = dr.GetOrdinal(this.TRANRETIDETAH88);
            if (!dr.IsDBNull(iTRANRETIDETAH88)) entity.TranRetiDetah88 = dr.GetDecimal(iTRANRETIDETAH88);

            int iTRANRETIDETAH89 = dr.GetOrdinal(this.TRANRETIDETAH89);
            if (!dr.IsDBNull(iTRANRETIDETAH89)) entity.TranRetiDetah89 = dr.GetDecimal(iTRANRETIDETAH89);

            int iTRANRETIDETAH90 = dr.GetOrdinal(this.TRANRETIDETAH90);
            if (!dr.IsDBNull(iTRANRETIDETAH90)) entity.TranRetiDetah90 = dr.GetDecimal(iTRANRETIDETAH90);

            int iTRANRETIDETAH91 = dr.GetOrdinal(this.TRANRETIDETAH91);
            if (!dr.IsDBNull(iTRANRETIDETAH91)) entity.TranRetiDetah91 = dr.GetDecimal(iTRANRETIDETAH91);

            int iTRANRETIDETAH92 = dr.GetOrdinal(this.TRANRETIDETAH92);
            if (!dr.IsDBNull(iTRANRETIDETAH92)) entity.TranRetiDetah92 = dr.GetDecimal(iTRANRETIDETAH92);

            int iTRANRETIDETAH93 = dr.GetOrdinal(this.TRANRETIDETAH93);
            if (!dr.IsDBNull(iTRANRETIDETAH93)) entity.TranRetiDetah93 = dr.GetDecimal(iTRANRETIDETAH93);

            int iTRANRETIDETAH94 = dr.GetOrdinal(this.TRANRETIDETAH94);
            if (!dr.IsDBNull(iTRANRETIDETAH94)) entity.TranRetiDetah94 = dr.GetDecimal(iTRANRETIDETAH94);

            int iTRANRETIDETAH95 = dr.GetOrdinal(this.TRANRETIDETAH95);
            if (!dr.IsDBNull(iTRANRETIDETAH95)) entity.TranRetiDetah95 = dr.GetDecimal(iTRANRETIDETAH95);

            int iTRANRETIDETAH96 = dr.GetOrdinal(this.TRANRETIDETAH96);
            if (!dr.IsDBNull(iTRANRETIDETAH96)) entity.TranRetiDetah96 = dr.GetDecimal(iTRANRETIDETAH96);

            int iTRETDEUSERNAME = dr.GetOrdinal(this.TRETDEUSERNAME);
            if (!dr.IsDBNull(iTRETDEUSERNAME)) entity.TretdeUserName = dr.GetString(iTRETDEUSERNAME);

            int iTRANRETIDETAFECINS = dr.GetOrdinal(this.TRANRETIDETAFECINS);
            if (!dr.IsDBNull(iTRANRETIDETAFECINS)) entity.TranRetiDetaFecIns = dr.GetDateTime(iTRANRETIDETAFECINS);

            int iTRANRETIDETAFECACT = dr.GetOrdinal(this.TRANRETIDETAFECACT);
            if (!dr.IsDBNull(iTRANRETIDETAFECACT)) entity.TranRetiDetaFecAct = dr.GetDateTime(iTRANRETIDETAFECACT);

            return entity;
        }


        #region Mapeo de Campos

        public string TRANRETICODI = "TRETCODI";
        public string TRANRETIDETACODI = "TRETDECODI";
        public string TRANRETIDETAVERSION = "TRETDEVERSION";
        public string TRANRETIDETADIA = "TRETDEDIA";
        public string TRANRETIDETAPROMDIA = "TRETDEPROMEDIODIA";
        public string TRANRETIDETASUMADIA = "TRETDESUMADIA";
        public string TRANRETIDETAH1 = "TRETDE1";
        public string TRANRETIDETAH2 = "TRETDE2";
        public string TRANRETIDETAH3 = "TRETDE3";
        public string TRANRETIDETAH4 = "TRETDE4";
        public string TRANRETIDETAH5 = "TRETDE5";
        public string TRANRETIDETAH6 = "TRETDE6";
        public string TRANRETIDETAH7 = "TRETDE7";
        public string TRANRETIDETAH8 = "TRETDE8";
        public string TRANRETIDETAH9 = "TRETDE9";
        public string TRANRETIDETAH10 = "TRETDE10";
        public string TRANRETIDETAH11 = "TRETDE11";
        public string TRANRETIDETAH12 = "TRETDE12";
        public string TRANRETIDETAH13 = "TRETDE13";
        public string TRANRETIDETAH14 = "TRETDE14";
        public string TRANRETIDETAH15 = "TRETDE15";
        public string TRANRETIDETAH16 = "TRETDE16";
        public string TRANRETIDETAH17 = "TRETDE17";
        public string TRANRETIDETAH18 = "TRETDE18";
        public string TRANRETIDETAH19 = "TRETDE19";
        public string TRANRETIDETAH20 = "TRETDE20";
        public string TRANRETIDETAH21 = "TRETDE21";
        public string TRANRETIDETAH22 = "TRETDE22";
        public string TRANRETIDETAH23 = "TRETDE23";
        public string TRANRETIDETAH24 = "TRETDE24";
        public string TRANRETIDETAH25 = "TRETDE25";
        public string TRANRETIDETAH26 = "TRETDE26";
        public string TRANRETIDETAH27 = "TRETDE27";
        public string TRANRETIDETAH28 = "TRETDE28";
        public string TRANRETIDETAH29 = "TRETDE29";
        public string TRANRETIDETAH30 = "TRETDE30";
        public string TRANRETIDETAH31 = "TRETDE31";
        public string TRANRETIDETAH32 = "TRETDE32";
        public string TRANRETIDETAH33 = "TRETDE33";
        public string TRANRETIDETAH34 = "TRETDE34";
        public string TRANRETIDETAH35 = "TRETDE35";
        public string TRANRETIDETAH36 = "TRETDE36";
        public string TRANRETIDETAH37 = "TRETDE37";
        public string TRANRETIDETAH38 = "TRETDE38";
        public string TRANRETIDETAH39 = "TRETDE39";
        public string TRANRETIDETAH40 = "TRETDE40";
        public string TRANRETIDETAH41 = "TRETDE41";
        public string TRANRETIDETAH42 = "TRETDE42";
        public string TRANRETIDETAH43 = "TRETDE43";
        public string TRANRETIDETAH44 = "TRETDE44";
        public string TRANRETIDETAH45 = "TRETDE45";
        public string TRANRETIDETAH46 = "TRETDE46";
        public string TRANRETIDETAH47 = "TRETDE47";
        public string TRANRETIDETAH48 = "TRETDE48";
        public string TRANRETIDETAH49 = "TRETDE49";
        public string TRANRETIDETAH50 = "TRETDE50";
        public string TRANRETIDETAH51 = "TRETDE51";
        public string TRANRETIDETAH52 = "TRETDE52";
        public string TRANRETIDETAH53 = "TRETDE53";
        public string TRANRETIDETAH54 = "TRETDE54";
        public string TRANRETIDETAH55 = "TRETDE55";
        public string TRANRETIDETAH56 = "TRETDE56";
        public string TRANRETIDETAH57 = "TRETDE57";
        public string TRANRETIDETAH58 = "TRETDE58";
        public string TRANRETIDETAH59 = "TRETDE59";
        public string TRANRETIDETAH60 = "TRETDE60";
        public string TRANRETIDETAH61 = "TRETDE61";
        public string TRANRETIDETAH62 = "TRETDE62";
        public string TRANRETIDETAH63 = "TRETDE63";
        public string TRANRETIDETAH64 = "TRETDE64";
        public string TRANRETIDETAH65 = "TRETDE65";
        public string TRANRETIDETAH66 = "TRETDE66";
        public string TRANRETIDETAH67 = "TRETDE67";
        public string TRANRETIDETAH68 = "TRETDE68";
        public string TRANRETIDETAH69 = "TRETDE69";
        public string TRANRETIDETAH70 = "TRETDE70";
        public string TRANRETIDETAH71 = "TRETDE71";
        public string TRANRETIDETAH72 = "TRETDE72";
        public string TRANRETIDETAH73 = "TRETDE73";
        public string TRANRETIDETAH74 = "TRETDE74";
        public string TRANRETIDETAH75 = "TRETDE75";
        public string TRANRETIDETAH76 = "TRETDE76";
        public string TRANRETIDETAH77 = "TRETDE77";
        public string TRANRETIDETAH78 = "TRETDE78";
        public string TRANRETIDETAH79 = "TRETDE79";
        public string TRANRETIDETAH80 = "TRETDE80";
        public string TRANRETIDETAH81 = "TRETDE81";
        public string TRANRETIDETAH82 = "TRETDE82";
        public string TRANRETIDETAH83 = "TRETDE83";
        public string TRANRETIDETAH84 = "TRETDE84";
        public string TRANRETIDETAH85 = "TRETDE85";
        public string TRANRETIDETAH86 = "TRETDE86";
        public string TRANRETIDETAH87 = "TRETDE87";
        public string TRANRETIDETAH88 = "TRETDE88";
        public string TRANRETIDETAH89 = "TRETDE89";
        public string TRANRETIDETAH90 = "TRETDE90";
        public string TRANRETIDETAH91 = "TRETDE91";
        public string TRANRETIDETAH92 = "TRETDE92";
        public string TRANRETIDETAH93 = "TRETDE93";
        public string TRANRETIDETAH94 = "TRETDE94";
        public string TRANRETIDETAH95 = "TRETDE95";
        public string TRANRETIDETAH96 = "TRETDE96";
        public string TRETDEUSERNAME = "TRETDEUSERNAME";
        public string TRANRETIDETAFECINS = "TRETDEFECINS";
        public string TRANRETIDETAFECACT = "TRETDEFECACT";
        //de la cabezera
        public string PERICODI = "PERICODI";
        public string BARRCODI = "BARRCODI";
        public string EMPRCODI = "GENEMPRCODI";
        public string CLICODI = "CLIEMPRCODI";
        public string SOLICODIRETICODIGO = "TRETCODIGO";
        public string TRANRETIVERSION = "TRETVERSION";
        public string TRANRETITIPOINFORMACION = "TRETTIPOINFORMACION";        
        public string TRETTABLA = "TRETTABLA";
        public string EMPRNOMB = "EMPRNOMB";

        public string TableName = "TRN_TRANS_RETIRO_DETALLE";

        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlListByPeriodoVersion
        {
            get { return base.GetSqlXml("GetByPeriodoVersion"); }
        }

        public string SqlListByTransferenciaRetiro
        {
            get { return base.GetSqlXml("ListByTransferenciaRetiro"); }
        }

        public string SqlDeleteListaTransferenciaRetiroDetalle
        {
            get { return base.GetSqlXml("DeleteListaTransferenciaRetiroDetalle"); }
        }

        public string SqlDeleteListaTransferenciaRetiroDetalleEmpresa
        {
            get { return base.GetSqlXml("DeleteListaTransferenciaRetiroDetalleEmpresa"); }
        }

        public string SqlListaTransRetiroDetallePorPeriodoYVersion
        {
            get { return base.GetSqlXml("ListaTransRetiroDetallePorPeriodoYVersion"); }
        }

        //ASSETEC - GRAN USUARIO - 28/11

        public string SqlListByTransferenciaRetiroDay
        {
            get { return base.GetSqlXml("ListByTransferenciaRetiroDay"); }
        }

        public string SqlGetDemandaRetiroByCodVtea
        {
            get { return base.GetSqlXml("GetDemandaRetiroByCodVtea"); }
        }
        public string SqlGetDemandaRetiroByCodVteaEmpresa
        {
            get { return base.GetSqlXml("GetDemandaRetiroByCodVteaEmpresa"); }
        }
    }
}
