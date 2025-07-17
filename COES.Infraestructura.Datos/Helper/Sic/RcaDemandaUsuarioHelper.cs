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
    public class RcaDemandaUsuarioHelper : HelperBase
    {
        public RcaDemandaUsuarioHelper()
            : base(Consultas.RcaDemandaUsuarioSql)
        {
        }

        public RcaDemandaUsuarioDTO Create(IDataReader dr)
        {
            RcaDemandaUsuarioDTO entity = new RcaDemandaUsuarioDTO();

            int iRcdeulcodi = dr.GetOrdinal(this.Rcdeulcodi);
            if (!dr.IsDBNull(iRcdeulcodi)) entity.Rcdeulcodi = dr.GetInt32(iRcdeulcodi);

            int iRcdeulperiodo = dr.GetOrdinal(this.Rcdeulperiodo);
            if (!dr.IsDBNull(iRcdeulperiodo)) entity.Rcdeulperiodo = dr.GetString(iRcdeulperiodo);

            int iRcdeulfecmaxdem = dr.GetOrdinal(this.Rcdeulfecmaxdem);
            if (!dr.IsDBNull(iRcdeulfecmaxdem)) entity.Rcdeulfecmaxdem = dr.GetDateTime(iRcdeulfecmaxdem);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

            int iRcdeuldemandahp = dr.GetOrdinal(this.Rcdeuldemandahp);
            if (!dr.IsDBNull(iRcdeuldemandahp)) entity.Rcdeuldemandahp = dr.GetDecimal(iRcdeuldemandahp);

            int iRcdeuldemandahfp = dr.GetOrdinal(this.Rcdeuldemandahfp);
            if (!dr.IsDBNull(iRcdeuldemandahfp)) entity.Rcdeuldemandahfp = dr.GetDecimal(iRcdeuldemandahfp);

            int iRcdeulfuente = dr.GetOrdinal(this.Rcdeulfuente);
            if (!dr.IsDBNull(iRcdeulfuente)) entity.Rcdeulfuente = dr.GetString(iRcdeulfuente);

            int iEquinomb = dr.GetOrdinal(this.Equinomb);
            if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

            int iEmprrazsocial = dr.GetOrdinal(this.Emprrazsocial);
            if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

            int iRCDEULH1 = dr.GetOrdinal(this.RCDEULH1);
            if (!dr.IsDBNull(iRCDEULH1)) entity.RCDEULH1 = dr.GetDecimal(iRCDEULH1);

            int iRCDEULH2 = dr.GetOrdinal(this.RCDEULH2);
            if (!dr.IsDBNull(iRCDEULH2)) entity.RCDEULH2 = dr.GetDecimal(iRCDEULH2);

            int iRCDEULH3 = dr.GetOrdinal(this.RCDEULH3);
            if (!dr.IsDBNull(iRCDEULH3)) entity.RCDEULH3 = dr.GetDecimal(iRCDEULH3);

            int iRCDEULH4 = dr.GetOrdinal(this.RCDEULH4);
            if (!dr.IsDBNull(iRCDEULH4)) entity.RCDEULH4 = dr.GetDecimal(iRCDEULH4);

            int iRCDEULH5 = dr.GetOrdinal(this.RCDEULH5);
            if (!dr.IsDBNull(iRCDEULH5)) entity.RCDEULH5 = dr.GetDecimal(iRCDEULH5);

            int iRCDEULH6 = dr.GetOrdinal(this.RCDEULH6);
            if (!dr.IsDBNull(iRCDEULH6)) entity.RCDEULH6 = dr.GetDecimal(iRCDEULH6);

            int iRCDEULH7 = dr.GetOrdinal(this.RCDEULH7);
            if (!dr.IsDBNull(iRCDEULH7)) entity.RCDEULH7 = dr.GetDecimal(iRCDEULH7);

            int iRCDEULH8 = dr.GetOrdinal(this.RCDEULH8);
            if (!dr.IsDBNull(iRCDEULH8)) entity.RCDEULH8 = dr.GetDecimal(iRCDEULH8);

            int iRCDEULH9 = dr.GetOrdinal(this.RCDEULH9);
            if (!dr.IsDBNull(iRCDEULH9)) entity.RCDEULH9 = dr.GetDecimal(iRCDEULH9);

            int iRCDEULH10 = dr.GetOrdinal(this.RCDEULH10);
            if (!dr.IsDBNull(iRCDEULH10)) entity.RCDEULH10 = dr.GetDecimal(iRCDEULH10);

            int iRCDEULH11 = dr.GetOrdinal(this.RCDEULH11);
            if (!dr.IsDBNull(iRCDEULH11)) entity.RCDEULH11 = dr.GetDecimal(iRCDEULH11);

            int iRCDEULH12 = dr.GetOrdinal(this.RCDEULH12);
            if (!dr.IsDBNull(iRCDEULH12)) entity.RCDEULH12 = dr.GetDecimal(iRCDEULH12);

            int iRCDEULH13 = dr.GetOrdinal(this.RCDEULH13);
            if (!dr.IsDBNull(iRCDEULH13)) entity.RCDEULH13 = dr.GetDecimal(iRCDEULH13);

            int iRCDEULH14 = dr.GetOrdinal(this.RCDEULH14);
            if (!dr.IsDBNull(iRCDEULH14)) entity.RCDEULH14 = dr.GetDecimal(iRCDEULH14);

            int iRCDEULH15 = dr.GetOrdinal(this.RCDEULH15);
            if (!dr.IsDBNull(iRCDEULH15)) entity.RCDEULH15 = dr.GetDecimal(iRCDEULH15);

            int iRCDEULH16 = dr.GetOrdinal(this.RCDEULH16);
            if (!dr.IsDBNull(iRCDEULH16)) entity.RCDEULH16 = dr.GetDecimal(iRCDEULH16);

            int iRCDEULH17 = dr.GetOrdinal(this.RCDEULH17);
            if (!dr.IsDBNull(iRCDEULH17)) entity.RCDEULH17 = dr.GetDecimal(iRCDEULH17);

            int iRCDEULH18 = dr.GetOrdinal(this.RCDEULH18);
            if (!dr.IsDBNull(iRCDEULH18)) entity.RCDEULH18 = dr.GetDecimal(iRCDEULH18);

            int iRCDEULH19 = dr.GetOrdinal(this.RCDEULH19);
            if (!dr.IsDBNull(iRCDEULH19)) entity.RCDEULH19 = dr.GetDecimal(iRCDEULH19);

            int iRCDEULH20 = dr.GetOrdinal(this.RCDEULH20);
            if (!dr.IsDBNull(iRCDEULH20)) entity.RCDEULH20 = dr.GetDecimal(iRCDEULH20);

            int iRCDEULH21 = dr.GetOrdinal(this.RCDEULH21);
            if (!dr.IsDBNull(iRCDEULH21)) entity.RCDEULH21 = dr.GetDecimal(iRCDEULH21);

            int iRCDEULH22 = dr.GetOrdinal(this.RCDEULH22);
            if (!dr.IsDBNull(iRCDEULH22)) entity.RCDEULH22 = dr.GetDecimal(iRCDEULH22);

            int iRCDEULH23 = dr.GetOrdinal(this.RCDEULH23);
            if (!dr.IsDBNull(iRCDEULH23)) entity.RCDEULH23 = dr.GetDecimal(iRCDEULH23);

            int iRCDEULH24 = dr.GetOrdinal(this.RCDEULH24);
            if (!dr.IsDBNull(iRCDEULH24)) entity.RCDEULH24 = dr.GetDecimal(iRCDEULH24);

            int iRCDEULH25 = dr.GetOrdinal(this.RCDEULH25);
            if (!dr.IsDBNull(iRCDEULH25)) entity.RCDEULH25 = dr.GetDecimal(iRCDEULH25);

            int iRCDEULH26 = dr.GetOrdinal(this.RCDEULH26);
            if (!dr.IsDBNull(iRCDEULH26)) entity.RCDEULH26 = dr.GetDecimal(iRCDEULH26);

            int iRCDEULH27 = dr.GetOrdinal(this.RCDEULH27);
            if (!dr.IsDBNull(iRCDEULH27)) entity.RCDEULH27 = dr.GetDecimal(iRCDEULH27);

            int iRCDEULH28 = dr.GetOrdinal(this.RCDEULH28);
            if (!dr.IsDBNull(iRCDEULH28)) entity.RCDEULH28 = dr.GetDecimal(iRCDEULH28);

            int iRCDEULH29 = dr.GetOrdinal(this.RCDEULH29);
            if (!dr.IsDBNull(iRCDEULH29)) entity.RCDEULH29 = dr.GetDecimal(iRCDEULH29);

            int iRCDEULH30 = dr.GetOrdinal(this.RCDEULH30);
            if (!dr.IsDBNull(iRCDEULH30)) entity.RCDEULH30 = dr.GetDecimal(iRCDEULH30);

            int iRCDEULH31 = dr.GetOrdinal(this.RCDEULH31);
            if (!dr.IsDBNull(iRCDEULH31)) entity.RCDEULH31 = dr.GetDecimal(iRCDEULH31);

            int iRCDEULH32 = dr.GetOrdinal(this.RCDEULH32);
            if (!dr.IsDBNull(iRCDEULH32)) entity.RCDEULH32 = dr.GetDecimal(iRCDEULH32);

            int iRCDEULH33 = dr.GetOrdinal(this.RCDEULH33);
            if (!dr.IsDBNull(iRCDEULH33)) entity.RCDEULH33 = dr.GetDecimal(iRCDEULH33);

            int iRCDEULH34 = dr.GetOrdinal(this.RCDEULH34);
            if (!dr.IsDBNull(iRCDEULH34)) entity.RCDEULH34 = dr.GetDecimal(iRCDEULH34);

            int iRCDEULH35 = dr.GetOrdinal(this.RCDEULH35);
            if (!dr.IsDBNull(iRCDEULH35)) entity.RCDEULH35 = dr.GetDecimal(iRCDEULH35);

            int iRCDEULH36 = dr.GetOrdinal(this.RCDEULH36);
            if (!dr.IsDBNull(iRCDEULH36)) entity.RCDEULH36 = dr.GetDecimal(iRCDEULH36);

            int iRCDEULH37 = dr.GetOrdinal(this.RCDEULH37);
            if (!dr.IsDBNull(iRCDEULH37)) entity.RCDEULH37 = dr.GetDecimal(iRCDEULH37);

            int iRCDEULH38 = dr.GetOrdinal(this.RCDEULH38);
            if (!dr.IsDBNull(iRCDEULH38)) entity.RCDEULH38 = dr.GetDecimal(iRCDEULH38);

            int iRCDEULH39 = dr.GetOrdinal(this.RCDEULH39);
            if (!dr.IsDBNull(iRCDEULH39)) entity.RCDEULH39 = dr.GetDecimal(iRCDEULH39);

            int iRCDEULH40 = dr.GetOrdinal(this.RCDEULH40);
            if (!dr.IsDBNull(iRCDEULH40)) entity.RCDEULH40 = dr.GetDecimal(iRCDEULH40);

            int iRCDEULH41 = dr.GetOrdinal(this.RCDEULH41);
            if (!dr.IsDBNull(iRCDEULH41)) entity.RCDEULH41 = dr.GetDecimal(iRCDEULH41);

            int iRCDEULH42 = dr.GetOrdinal(this.RCDEULH42);
            if (!dr.IsDBNull(iRCDEULH42)) entity.RCDEULH42 = dr.GetDecimal(iRCDEULH42);

            int iRCDEULH43 = dr.GetOrdinal(this.RCDEULH43);
            if (!dr.IsDBNull(iRCDEULH43)) entity.RCDEULH43 = dr.GetDecimal(iRCDEULH43);

            int iRCDEULH44 = dr.GetOrdinal(this.RCDEULH44);
            if (!dr.IsDBNull(iRCDEULH44)) entity.RCDEULH44 = dr.GetDecimal(iRCDEULH44);

            int iRCDEULH45 = dr.GetOrdinal(this.RCDEULH45);
            if (!dr.IsDBNull(iRCDEULH45)) entity.RCDEULH45 = dr.GetDecimal(iRCDEULH45);

            int iRCDEULH46 = dr.GetOrdinal(this.RCDEULH46);
            if (!dr.IsDBNull(iRCDEULH46)) entity.RCDEULH46 = dr.GetDecimal(iRCDEULH46);

            int iRCDEULH47 = dr.GetOrdinal(this.RCDEULH47);
            if (!dr.IsDBNull(iRCDEULH47)) entity.RCDEULH47 = dr.GetDecimal(iRCDEULH47);

            int iRCDEULH48 = dr.GetOrdinal(this.RCDEULH48);
            if (!dr.IsDBNull(iRCDEULH48)) entity.RCDEULH48 = dr.GetDecimal(iRCDEULH48);

            int iRCDEULH49 = dr.GetOrdinal(this.RCDEULH49);
            if (!dr.IsDBNull(iRCDEULH49)) entity.RCDEULH49 = dr.GetDecimal(iRCDEULH49);

            int iRCDEULH50 = dr.GetOrdinal(this.RCDEULH50);
            if (!dr.IsDBNull(iRCDEULH50)) entity.RCDEULH50 = dr.GetDecimal(iRCDEULH50);

            int iRCDEULH51 = dr.GetOrdinal(this.RCDEULH51);
            if (!dr.IsDBNull(iRCDEULH51)) entity.RCDEULH51 = dr.GetDecimal(iRCDEULH51);

            int iRCDEULH52 = dr.GetOrdinal(this.RCDEULH52);
            if (!dr.IsDBNull(iRCDEULH52)) entity.RCDEULH52 = dr.GetDecimal(iRCDEULH52);

            int iRCDEULH53 = dr.GetOrdinal(this.RCDEULH53);
            if (!dr.IsDBNull(iRCDEULH53)) entity.RCDEULH53 = dr.GetDecimal(iRCDEULH53);

            int iRCDEULH54 = dr.GetOrdinal(this.RCDEULH54);
            if (!dr.IsDBNull(iRCDEULH54)) entity.RCDEULH54 = dr.GetDecimal(iRCDEULH54);

            int iRCDEULH55 = dr.GetOrdinal(this.RCDEULH55);
            if (!dr.IsDBNull(iRCDEULH55)) entity.RCDEULH55 = dr.GetDecimal(iRCDEULH55);

            int iRCDEULH56 = dr.GetOrdinal(this.RCDEULH56);
            if (!dr.IsDBNull(iRCDEULH56)) entity.RCDEULH56 = dr.GetDecimal(iRCDEULH56);

            int iRCDEULH57 = dr.GetOrdinal(this.RCDEULH57);
            if (!dr.IsDBNull(iRCDEULH57)) entity.RCDEULH57 = dr.GetDecimal(iRCDEULH57);

            int iRCDEULH58 = dr.GetOrdinal(this.RCDEULH58);
            if (!dr.IsDBNull(iRCDEULH58)) entity.RCDEULH58 = dr.GetDecimal(iRCDEULH58);

            int iRCDEULH59 = dr.GetOrdinal(this.RCDEULH59);
            if (!dr.IsDBNull(iRCDEULH59)) entity.RCDEULH59 = dr.GetDecimal(iRCDEULH59);

            int iRCDEULH60 = dr.GetOrdinal(this.RCDEULH60);
            if (!dr.IsDBNull(iRCDEULH60)) entity.RCDEULH60 = dr.GetDecimal(iRCDEULH60);

            int iRCDEULH61 = dr.GetOrdinal(this.RCDEULH61);
            if (!dr.IsDBNull(iRCDEULH61)) entity.RCDEULH61 = dr.GetDecimal(iRCDEULH61);

            int iRCDEULH62 = dr.GetOrdinal(this.RCDEULH62);
            if (!dr.IsDBNull(iRCDEULH62)) entity.RCDEULH62 = dr.GetDecimal(iRCDEULH62);

            int iRCDEULH63 = dr.GetOrdinal(this.RCDEULH63);
            if (!dr.IsDBNull(iRCDEULH63)) entity.RCDEULH63 = dr.GetDecimal(iRCDEULH63);

            int iRCDEULH64 = dr.GetOrdinal(this.RCDEULH64);
            if (!dr.IsDBNull(iRCDEULH64)) entity.RCDEULH64 = dr.GetDecimal(iRCDEULH64);

            int iRCDEULH65 = dr.GetOrdinal(this.RCDEULH65);
            if (!dr.IsDBNull(iRCDEULH65)) entity.RCDEULH65 = dr.GetDecimal(iRCDEULH65);

            int iRCDEULH66 = dr.GetOrdinal(this.RCDEULH66);
            if (!dr.IsDBNull(iRCDEULH66)) entity.RCDEULH66 = dr.GetDecimal(iRCDEULH66);

            int iRCDEULH67 = dr.GetOrdinal(this.RCDEULH67);
            if (!dr.IsDBNull(iRCDEULH67)) entity.RCDEULH67 = dr.GetDecimal(iRCDEULH67);

            int iRCDEULH68 = dr.GetOrdinal(this.RCDEULH68);
            if (!dr.IsDBNull(iRCDEULH68)) entity.RCDEULH68 = dr.GetDecimal(iRCDEULH68);

            int iRCDEULH69 = dr.GetOrdinal(this.RCDEULH69);
            if (!dr.IsDBNull(iRCDEULH69)) entity.RCDEULH69 = dr.GetDecimal(iRCDEULH69);

            int iRCDEULH70 = dr.GetOrdinal(this.RCDEULH70);
            if (!dr.IsDBNull(iRCDEULH70)) entity.RCDEULH70 = dr.GetDecimal(iRCDEULH70);

            int iRCDEULH71 = dr.GetOrdinal(this.RCDEULH71);
            if (!dr.IsDBNull(iRCDEULH71)) entity.RCDEULH71 = dr.GetDecimal(iRCDEULH71);

            int iRCDEULH72 = dr.GetOrdinal(this.RCDEULH72);
            if (!dr.IsDBNull(iRCDEULH72)) entity.RCDEULH72 = dr.GetDecimal(iRCDEULH72);

            int iRCDEULH73 = dr.GetOrdinal(this.RCDEULH73);
            if (!dr.IsDBNull(iRCDEULH73)) entity.RCDEULH73 = dr.GetDecimal(iRCDEULH73);

            int iRCDEULH74 = dr.GetOrdinal(this.RCDEULH74);
            if (!dr.IsDBNull(iRCDEULH74)) entity.RCDEULH74 = dr.GetDecimal(iRCDEULH74);

            int iRCDEULH75 = dr.GetOrdinal(this.RCDEULH75);
            if (!dr.IsDBNull(iRCDEULH75)) entity.RCDEULH75 = dr.GetDecimal(iRCDEULH75);

            int iRCDEULH76 = dr.GetOrdinal(this.RCDEULH76);
            if (!dr.IsDBNull(iRCDEULH76)) entity.RCDEULH76 = dr.GetDecimal(iRCDEULH76);

            int iRCDEULH77 = dr.GetOrdinal(this.RCDEULH77);
            if (!dr.IsDBNull(iRCDEULH77)) entity.RCDEULH77 = dr.GetDecimal(iRCDEULH77);

            int iRCDEULH78 = dr.GetOrdinal(this.RCDEULH78);
            if (!dr.IsDBNull(iRCDEULH78)) entity.RCDEULH78 = dr.GetDecimal(iRCDEULH78);

            int iRCDEULH79 = dr.GetOrdinal(this.RCDEULH79);
            if (!dr.IsDBNull(iRCDEULH79)) entity.RCDEULH79 = dr.GetDecimal(iRCDEULH79);

            int iRCDEULH80 = dr.GetOrdinal(this.RCDEULH80);
            if (!dr.IsDBNull(iRCDEULH80)) entity.RCDEULH80 = dr.GetDecimal(iRCDEULH80);

            int iRCDEULH81 = dr.GetOrdinal(this.RCDEULH81);
            if (!dr.IsDBNull(iRCDEULH81)) entity.RCDEULH81 = dr.GetDecimal(iRCDEULH81);

            int iRCDEULH82 = dr.GetOrdinal(this.RCDEULH82);
            if (!dr.IsDBNull(iRCDEULH82)) entity.RCDEULH82 = dr.GetDecimal(iRCDEULH82);

            int iRCDEULH83 = dr.GetOrdinal(this.RCDEULH83);
            if (!dr.IsDBNull(iRCDEULH83)) entity.RCDEULH83 = dr.GetDecimal(iRCDEULH83);

            int iRCDEULH84 = dr.GetOrdinal(this.RCDEULH84);
            if (!dr.IsDBNull(iRCDEULH84)) entity.RCDEULH84 = dr.GetDecimal(iRCDEULH84);

            int iRCDEULH85 = dr.GetOrdinal(this.RCDEULH85);
            if (!dr.IsDBNull(iRCDEULH85)) entity.RCDEULH85 = dr.GetDecimal(iRCDEULH85);

            int iRCDEULH86 = dr.GetOrdinal(this.RCDEULH86);
            if (!dr.IsDBNull(iRCDEULH86)) entity.RCDEULH86 = dr.GetDecimal(iRCDEULH86);

            int iRCDEULH87 = dr.GetOrdinal(this.RCDEULH87);
            if (!dr.IsDBNull(iRCDEULH87)) entity.RCDEULH87 = dr.GetDecimal(iRCDEULH87);

            int iRCDEULH88 = dr.GetOrdinal(this.RCDEULH88);
            if (!dr.IsDBNull(iRCDEULH88)) entity.RCDEULH88 = dr.GetDecimal(iRCDEULH88);

            int iRCDEULH89 = dr.GetOrdinal(this.RCDEULH89);
            if (!dr.IsDBNull(iRCDEULH89)) entity.RCDEULH89 = dr.GetDecimal(iRCDEULH89);

            int iRCDEULH90 = dr.GetOrdinal(this.RCDEULH90);
            if (!dr.IsDBNull(iRCDEULH90)) entity.RCDEULH90 = dr.GetDecimal(iRCDEULH90);

            int iRCDEULH91 = dr.GetOrdinal(this.RCDEULH91);
            if (!dr.IsDBNull(iRCDEULH91)) entity.RCDEULH91 = dr.GetDecimal(iRCDEULH91);

            int iRCDEULH92 = dr.GetOrdinal(this.RCDEULH92);
            if (!dr.IsDBNull(iRCDEULH92)) entity.RCDEULH92 = dr.GetDecimal(iRCDEULH92);

            int iRCDEULH93 = dr.GetOrdinal(this.RCDEULH93);
            if (!dr.IsDBNull(iRCDEULH93)) entity.RCDEULH93 = dr.GetDecimal(iRCDEULH93);

            int iRCDEULH94 = dr.GetOrdinal(this.RCDEULH94);
            if (!dr.IsDBNull(iRCDEULH94)) entity.RCDEULH94 = dr.GetDecimal(iRCDEULH94);

            int iRCDEULH95 = dr.GetOrdinal(this.RCDEULH95);
            if (!dr.IsDBNull(iRCDEULH95)) entity.RCDEULH95 = dr.GetDecimal(iRCDEULH95);

            int iRCDEULH96 = dr.GetOrdinal(this.RCDEULH96);
            if (!dr.IsDBNull(iRCDEULH96)) entity.RCDEULH96 = dr.GetDecimal(iRCDEULH96);

            return entity;
        }

        #region Mapeo de Campos

        public string Rcdeulcodi = "RCDEULCODI";
        public string Rcdeulperiodo = "RCDEULPERIODO";
        public string Rcdeulfecmaxdem = "RCDEULFECMAXDEM";
        public string Rcdeuldemandahp = "RCDEULDEMANDAHP";
        public string Rcdeuldemandahfp = "RCDEULDEMANDAHFP";
        public string Rcdeulfuente = "RCDEULFUENTE";        
        public string Equicodi = "EQUICODI";
        public string Equinomb = "EQUINOMB";
        public string Emprcodi = "EMPRCODI";
        public string Emprrazsocial = "EMPRRAZSOCIAL";



        public string Rcdeulfeccreacion = "RCDEULFECCREACION";
        public string Rcdeulusucreacion = "RCDEULUSUCREACION";
        
        public string RCDEULH1 = "RCDEULH1";
        public string RCDEULH2 = "RCDEULH2";
        public string RCDEULH3 = "RCDEULH3";
        public string RCDEULH4 = "RCDEULH4";
        public string RCDEULH5 = "RCDEULH5";
        public string RCDEULH6 = "RCDEULH6";
        public string RCDEULH7 = "RCDEULH7";
        public string RCDEULH8 = "RCDEULH8";
        public string RCDEULH9 = "RCDEULH9";
        public string RCDEULH10 = "RCDEULH10";
        public string RCDEULH11 = "RCDEULH11";
        public string RCDEULH12 = "RCDEULH12";
        public string RCDEULH13 = "RCDEULH13";
        public string RCDEULH14 = "RCDEULH14";
        public string RCDEULH15 = "RCDEULH15";
        public string RCDEULH16 = "RCDEULH16";
        public string RCDEULH17 = "RCDEULH17";
        public string RCDEULH18 = "RCDEULH18";
        public string RCDEULH19 = "RCDEULH19";
        public string RCDEULH20 = "RCDEULH20";
        public string RCDEULH21 = "RCDEULH21";
        public string RCDEULH22 = "RCDEULH22";
        public string RCDEULH23 = "RCDEULH23";
        public string RCDEULH24 = "RCDEULH24";
        public string RCDEULH25 = "RCDEULH25";
        public string RCDEULH26 = "RCDEULH26";
        public string RCDEULH27 = "RCDEULH27";
        public string RCDEULH28 = "RCDEULH28";
        public string RCDEULH29 = "RCDEULH29";
        public string RCDEULH30 = "RCDEULH30";
        public string RCDEULH31 = "RCDEULH31";
        public string RCDEULH32 = "RCDEULH32";
        public string RCDEULH33 = "RCDEULH33";
        public string RCDEULH34 = "RCDEULH34";
        public string RCDEULH35 = "RCDEULH35";
        public string RCDEULH36 = "RCDEULH36";
        public string RCDEULH37 = "RCDEULH37";
        public string RCDEULH38 = "RCDEULH38";
        public string RCDEULH39 = "RCDEULH39";
        public string RCDEULH40 = "RCDEULH40";
        public string RCDEULH41 = "RCDEULH41";
        public string RCDEULH42 = "RCDEULH42";
        public string RCDEULH43 = "RCDEULH43";
        public string RCDEULH44 = "RCDEULH44";
        public string RCDEULH45 = "RCDEULH45";
        public string RCDEULH46 = "RCDEULH46";
        public string RCDEULH47 = "RCDEULH47";
        public string RCDEULH48 = "RCDEULH48";
        public string RCDEULH49 = "RCDEULH49";
        public string RCDEULH50 = "RCDEULH50";
        public string RCDEULH51 = "RCDEULH51";
        public string RCDEULH52 = "RCDEULH52";
        public string RCDEULH53 = "RCDEULH53";
        public string RCDEULH54 = "RCDEULH54";
        public string RCDEULH55 = "RCDEULH55";
        public string RCDEULH56 = "RCDEULH56";
        public string RCDEULH57 = "RCDEULH57";
        public string RCDEULH58 = "RCDEULH58";
        public string RCDEULH59 = "RCDEULH59";
        public string RCDEULH60 = "RCDEULH60";
        public string RCDEULH61 = "RCDEULH61";
        public string RCDEULH62 = "RCDEULH62";
        public string RCDEULH63 = "RCDEULH63";
        public string RCDEULH64 = "RCDEULH64";
        public string RCDEULH65 = "RCDEULH65";
        public string RCDEULH66 = "RCDEULH66";
        public string RCDEULH67 = "RCDEULH67";
        public string RCDEULH68 = "RCDEULH68";
        public string RCDEULH69 = "RCDEULH69";
        public string RCDEULH70 = "RCDEULH70";
        public string RCDEULH71 = "RCDEULH71";
        public string RCDEULH72 = "RCDEULH72";
        public string RCDEULH73 = "RCDEULH73";
        public string RCDEULH74 = "RCDEULH74";
        public string RCDEULH75 = "RCDEULH75";
        public string RCDEULH76 = "RCDEULH76";
        public string RCDEULH77 = "RCDEULH77";
        public string RCDEULH78 = "RCDEULH78";
        public string RCDEULH79 = "RCDEULH79";
        public string RCDEULH80 = "RCDEULH80";
        public string RCDEULH81 = "RCDEULH81";
        public string RCDEULH82 = "RCDEULH82";
        public string RCDEULH83 = "RCDEULH83";
        public string RCDEULH84 = "RCDEULH84";
        public string RCDEULH85 = "RCDEULH85";
        public string RCDEULH86 = "RCDEULH86";
        public string RCDEULH87 = "RCDEULH87";
        public string RCDEULH88 = "RCDEULH88";
        public string RCDEULH89 = "RCDEULH89";
        public string RCDEULH90 = "RCDEULH90";
        public string RCDEULH91 = "RCDEULH91";
        public string RCDEULH92 = "RCDEULH92";
        public string RCDEULH93 = "RCDEULH93";
        public string RCDEULH94 = "RCDEULH94";
        public string RCDEULH95 = "RCDEULH95";
        public string RCDEULH96 = "RCDEULH96";


        /// <summary>
        /// Aplicativo PR16
        /// </summary>      
        public string Enviocodi = "ENVIOCODI";
        public string Item = "ITEM";
        public string Periodo = "PERIODO";
        public string IniRemision = "INI_REMISION";        
        public string Suministrador = "SUMINISTRADOR";
        public string Ruc = "RUC";
        public string NombreEmpresa = "NOMBRE_EMPRESA";
        public string Subestacion = "SUBESTACION";
        public string Tension = "TENSION";        
       
        public string Qregistros = "Q_REGISTROS";
               
        #endregion


        public string Osinergcodi = "OSINERGCODI";
        public string SqlObtenerListaPeriodoReporte
        {
            get { return base.GetSqlXml("ObtenerListaPeriodoReporte"); }
        }
        public string SqlObtenerReporteDemandaUsuarioCount
        {
            get { return base.GetSqlXml("ObtenerReporteDemandaUsuarioCount"); }
        }
        public string SqlObtenerReporteDemandaUsuario
        {
            get { return base.GetSqlXml("ObtenerReporteDemandaUsuario"); }
        }
        public string SqlObtenerEquipos
        {
            get { return base.GetSqlXml("ObtenerEquipos"); }
        }

        public string SqObtenerDemandaUsuarioErroresPag
        {
            get { return base.GetSqlXml("ObtenerDemandaUsuarioErroresPag"); }
        }

        public string SqObtenerDemandaUsuarioErroresExcel
        {
            get { return base.GetSqlXml("ObtenerDemandaUsuarioErroresExcel"); }
        }

        public string SqObtenerReporteDemandaUsuarioExcel
        {
            get { return base.GetSqlXml("ObtenerReporteDemandaUsuarioExcel"); }
        }
    }
}
