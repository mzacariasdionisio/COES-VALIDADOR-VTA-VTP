using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class ScadaHelper: HelperBase
    {
        public ScadaHelper(): base(Consultas.PerfilScadaSql) 
        {

        }

        public ScadaDTO Create(IDataReader dr)
        {
            ScadaDTO entity = new ScadaDTO();

            int iCanalCodi = dr.GetOrdinal(this.CANALCODI);
            if (!dr.IsDBNull(iCanalCodi)) entity.CANALCODI = Convert.ToInt32(dr.GetValue(iCanalCodi));

            int iPtoMediCodi = dr.GetOrdinal(this.PTOMEDICODI);
            if (!dr.IsDBNull(iPtoMediCodi)) entity.PTOMEDICODI = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

            int iMediFecha = dr.GetOrdinal(this.MEDIFECHA);
            if (!dr.IsDBNull(iMediFecha)) entity.MEDIFECHA = dr.GetDateTime(iMediFecha);    

            int iH2 = dr.GetOrdinal(this.H2);
            if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

            int iH4 = dr.GetOrdinal(this.H4);
            if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

            int iH6 = dr.GetOrdinal(this.H6);
            if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

            int iH8 = dr.GetOrdinal(this.H8);
            if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

            int iH10 = dr.GetOrdinal(this.H10);
            if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

            int iH12 = dr.GetOrdinal(this.H12);
            if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

            int iH14 = dr.GetOrdinal(this.H14);
            if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

            int iH16 = dr.GetOrdinal(this.H16);
            if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

            int iH18 = dr.GetOrdinal(this.H18);
            if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

            int iH20 = dr.GetOrdinal(this.H20);
            if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);
            
            int iH22 = dr.GetOrdinal(this.H22);
            if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

            int iH24 = dr.GetOrdinal(this.H24);
            if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

            int iH26 = dr.GetOrdinal(this.H26);
            if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

            int iH28 = dr.GetOrdinal(this.H28);
            if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

            int iH30 = dr.GetOrdinal(this.H30);
            if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

            int iH32 = dr.GetOrdinal(this.H32);
            if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

            int iH34 = dr.GetOrdinal(this.H34);
            if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

            int iH36 = dr.GetOrdinal(this.H36);
            if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

            int iH38 = dr.GetOrdinal(this.H38);
            if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

            int iH40 = dr.GetOrdinal(this.H40);
            if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

            int iH42 = dr.GetOrdinal(this.H42);
            if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

            int iH44 = dr.GetOrdinal(this.H44);
            if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

            int iH46 = dr.GetOrdinal(this.H46);
            if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

            int iH48 = dr.GetOrdinal(this.H48);
            if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

            int iH50 = dr.GetOrdinal(this.H50);
            if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

            int iH52 = dr.GetOrdinal(this.H52);
            if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

            int iH54 = dr.GetOrdinal(this.H54);
            if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

            int iH56 = dr.GetOrdinal(this.H56);
            if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

            int iH58 = dr.GetOrdinal(this.H58);
            if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

            int iH60 = dr.GetOrdinal(this.H60);
            if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

            int iH62 = dr.GetOrdinal(this.H62);
            if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

            int iH64 = dr.GetOrdinal(this.H64);
            if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

            int iH66 = dr.GetOrdinal(this.H66);
            if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

            int iH68 = dr.GetOrdinal(this.H68);
            if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

            int iH70 = dr.GetOrdinal(this.H70);
            if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

            int iH72 = dr.GetOrdinal(this.H72);
            if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

            int iH74 = dr.GetOrdinal(this.H74);
            if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

            int iH76 = dr.GetOrdinal(this.H76);
            if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

            int iH78 = dr.GetOrdinal(this.H78);
            if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

            int iH80 = dr.GetOrdinal(this.H80);
            if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

            int iH82 = dr.GetOrdinal(this.H82);
            if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

            int iH84 = dr.GetOrdinal(this.H84);
            if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

            int iH86 = dr.GetOrdinal(this.H86);
            if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

            int iH88 = dr.GetOrdinal(this.H88);
            if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

            int iH90 = dr.GetOrdinal(this.H90);
            if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

            int iH92 = dr.GetOrdinal(this.H92);
            if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

            int iH94 = dr.GetOrdinal(this.H94);
            if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

            int iH96 = dr.GetOrdinal(this.H96);
            if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);      

            return entity;
        }

        public PerfilScadaDTO CreatePerfil(IDataReader dr)
        {
            PerfilScadaDTO entity = new PerfilScadaDTO();

            int iPERFCODI = dr.GetOrdinal(this.PERFCODI);
            if (!dr.IsDBNull(iPERFCODI)) entity.PERFCODI = Convert.ToInt32(dr.GetValue(iPERFCODI));

            int iPERFDESC = dr.GetOrdinal(this.PERFDESC);
            if (!dr.IsDBNull(iPERFDESC)) entity.PERFDESC = dr.GetString(iPERFDESC);

            int iFECREGISTRO = dr.GetOrdinal(this.FECREGISTRO);
            if (!dr.IsDBNull(iFECREGISTRO)) entity.FECREGISTRO = dr.GetDateTime(iFECREGISTRO);

            int iFECINICIO = dr.GetOrdinal(this.FECINICIO);
            if (!dr.IsDBNull(iFECINICIO)) entity.FECINICIO = dr.GetDateTime(iFECINICIO);

            int iFECFIN = dr.GetOrdinal(this.FECFIN);
            if (!dr.IsDBNull(iFECFIN)) entity.FECFIN = dr.GetDateTime(iFECFIN);

            int iLASTUSER = dr.GetOrdinal(this.LASTUSER);
            if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);

            int iLASTDATE = dr.GetOrdinal(this.LASTDATE);
            if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetDateTime(iLASTDATE);

            int iEJRUCODI = dr.GetOrdinal(this.EJRUCODI);
            if (!dr.IsDBNull(iEJRUCODI)) entity.EJRUCODI = Convert.ToInt32(dr.GetValue(iEJRUCODI));

            int iPERFCLASI = dr.GetOrdinal(this.PERFCLASI);
            if (!dr.IsDBNull(iPERFCLASI)) entity.PERFCLASI = Convert.ToInt32(dr.GetValue(iPERFCLASI));

            int iPERFORIG = dr.GetOrdinal(this.PERFORIG);
            if (!dr.IsDBNull(iPERFORIG)) entity.PERFORIG = dr.GetString(iPERFORIG);

            return entity;
        }

        public PerfilScadaDetDTO CreatePerfilDetalle(IDataReader dr)
        {
            PerfilScadaDetDTO entity = new PerfilScadaDetDTO();

            int iPERFDETCODI = dr.GetOrdinal(this.PERFDETCODI);
            if (!dr.IsDBNull(iPERFDETCODI)) entity.PERFDETCODI = Convert.ToInt32(dr.GetValue(iPERFDETCODI));
            
            int iPERFCODI = dr.GetOrdinal(this.PERFCODI);
            if (!dr.IsDBNull(iPERFCODI)) entity.PERFCODI = Convert.ToInt32(dr.GetValue(iPERFCODI));

            int iPERFCLASI = dr.GetOrdinal(this.PERFCLASI);
            if (!dr.IsDBNull(iPERFCLASI)) entity.PERFCLASI = Convert.ToInt32(dr.GetValue(iPERFCLASI));

            int iH1 = dr.GetOrdinal(this.H1);
            if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

            int iH2 = dr.GetOrdinal(this.H2);
            if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

            int iH3 = dr.GetOrdinal(this.H3);
            if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

            int iH4 = dr.GetOrdinal(this.H4);
            if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

            int iH5 = dr.GetOrdinal(this.H5);
            if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

            int iH6 = dr.GetOrdinal(this.H6);
            if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

            int iH7 = dr.GetOrdinal(this.H7);
            if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

            int iH8 = dr.GetOrdinal(this.H8);
            if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);
            
            int iH9 = dr.GetOrdinal(this.H9);
            if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

            int iH10 = dr.GetOrdinal(this.H10);
            if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

            int iH11 = dr.GetOrdinal(this.H11);
            if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

            int iH12 = dr.GetOrdinal(this.H12);
            if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

            int iH13 = dr.GetOrdinal(this.H13);
            if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

            int iH14 = dr.GetOrdinal(this.H14);
            if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

            int iH15 = dr.GetOrdinal(this.H15);
            if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

            int iH16 = dr.GetOrdinal(this.H16);
            if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

            int iH17 = dr.GetOrdinal(this.H17);
            if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

            int iH18 = dr.GetOrdinal(this.H18);
            if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

            int iH19 = dr.GetOrdinal(this.H19);
            if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

            int iH20 = dr.GetOrdinal(this.H20);
            if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

            int iH21 = dr.GetOrdinal(this.H21);
            if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

            int iH22 = dr.GetOrdinal(this.H22);
            if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

            int iH23 = dr.GetOrdinal(this.H23);
            if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

            int iH24 = dr.GetOrdinal(this.H24);
            if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

            int iH25 = dr.GetOrdinal(this.H25);
            if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

            int iH26 = dr.GetOrdinal(this.H26);
            if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

            int iH27 = dr.GetOrdinal(this.H27);
            if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

            int iH28 = dr.GetOrdinal(this.H28);
            if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

            int iH29 = dr.GetOrdinal(this.H29);
            if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

            int iH30 = dr.GetOrdinal(this.H30);
            if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

            int iH31 = dr.GetOrdinal(this.H31);
            if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

            int iH32 = dr.GetOrdinal(this.H32);
            if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

            int iH33 = dr.GetOrdinal(this.H33);
            if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

            int iH34 = dr.GetOrdinal(this.H34);
            if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

            int iH35 = dr.GetOrdinal(this.H35);
            if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

            int iH36 = dr.GetOrdinal(this.H36);
            if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

            int iH37 = dr.GetOrdinal(this.H37);
            if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

            int iH38 = dr.GetOrdinal(this.H38);
            if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

            int iH39 = dr.GetOrdinal(this.H39);
            if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

            int iH40 = dr.GetOrdinal(this.H40);
            if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);
            
            int iH41 = dr.GetOrdinal(this.H41);
            if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

            int iH42 = dr.GetOrdinal(this.H42);
            if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

            int iH43 = dr.GetOrdinal(this.H43);
            if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);
               
            int iH44 = dr.GetOrdinal(this.H44);
            if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);
            
            int iH45 = dr.GetOrdinal(this.H45);
            if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);
            
            int iH46 = dr.GetOrdinal(this.H46);
            if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

            int iH47 = dr.GetOrdinal(this.H47);
            if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

            int iH48 = dr.GetOrdinal(this.H48);
            if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

            int iTH1 = dr.GetOrdinal(this.TH1);
            if (!dr.IsDBNull(iTH1)) entity.TH1 = dr.GetDecimal(iTH1);

            int iTH2 = dr.GetOrdinal(this.TH2);
            if (!dr.IsDBNull(iTH2)) entity.TH2 = dr.GetDecimal(iTH2);

            int iTH3 = dr.GetOrdinal(this.TH3);
            if (!dr.IsDBNull(iTH3)) entity.TH3 = dr.GetDecimal(iTH3);
            
            int iTH4 = dr.GetOrdinal(this.TH4);
            if (!dr.IsDBNull(iTH4)) entity.TH4 = dr.GetDecimal(iTH4);
            
            int iTH5 = dr.GetOrdinal(this.TH5);
            if (!dr.IsDBNull(iTH5)) entity.TH5 = dr.GetDecimal(iTH5);

            int iTH6 = dr.GetOrdinal(this.TH6);
            if (!dr.IsDBNull(iTH6)) entity.TH6 = dr.GetDecimal(iTH6);

            int iTH7 = dr.GetOrdinal(this.TH7);
            if (!dr.IsDBNull(iTH7)) entity.TH7 = dr.GetDecimal(iTH7);

            int iTH8 = dr.GetOrdinal(this.TH8);
            if (!dr.IsDBNull(iTH8)) entity.TH8 = dr.GetDecimal(iTH8);

            int iTH9 = dr.GetOrdinal(this.TH9);
            if (!dr.IsDBNull(iTH9)) entity.TH9 = dr.GetDecimal(iTH9);

            int iTH10 = dr.GetOrdinal(this.TH10);
            if (!dr.IsDBNull(iTH10)) entity.TH10 = dr.GetDecimal(iTH10);

            int iTH11 = dr.GetOrdinal(this.TH11);
            if (!dr.IsDBNull(iTH11)) entity.TH11 = dr.GetDecimal(iTH11);

            int iTH12 = dr.GetOrdinal(this.TH12);
            if (!dr.IsDBNull(iTH12)) entity.TH12 = dr.GetDecimal(iTH12);

            int iTH13 = dr.GetOrdinal(this.TH13);
            if (!dr.IsDBNull(iTH13)) entity.TH13 = dr.GetDecimal(iTH13);

            int iTH14 = dr.GetOrdinal(this.TH14);
            if (!dr.IsDBNull(iTH14)) entity.TH14 = dr.GetDecimal(iTH14);

            int iTH15 = dr.GetOrdinal(this.TH15);
            if (!dr.IsDBNull(iTH15)) entity.TH15 = dr.GetDecimal(iTH15);

            int iTH16 = dr.GetOrdinal(this.TH16);
            if (!dr.IsDBNull(iTH16)) entity.TH16 = dr.GetDecimal(iTH16);

            int iTH17 = dr.GetOrdinal(this.TH17);
            if (!dr.IsDBNull(iTH17)) entity.TH17 = dr.GetDecimal(iTH17);

            int iTH18 = dr.GetOrdinal(this.TH18);
            if (!dr.IsDBNull(iTH18)) entity.TH18 = dr.GetDecimal(iTH18);

            int iTH19 = dr.GetOrdinal(this.TH19);
            if (!dr.IsDBNull(iTH19)) entity.TH19 = dr.GetDecimal(iTH19);

            int iTH20 = dr.GetOrdinal(this.TH20);
            if (!dr.IsDBNull(iTH20)) entity.TH20 = dr.GetDecimal(iTH20);

            int iTH21 = dr.GetOrdinal(this.TH21);
            if (!dr.IsDBNull(iTH21)) entity.TH21 = dr.GetDecimal(iTH21);

            int iTH22 = dr.GetOrdinal(this.TH22);
            if (!dr.IsDBNull(iTH22)) entity.TH22 = dr.GetDecimal(iTH22);

            int iTH23 = dr.GetOrdinal(this.TH23);
            if (!dr.IsDBNull(iTH23)) entity.TH23 = dr.GetDecimal(iTH23);

            int iTH24 = dr.GetOrdinal(this.TH24);
            if (!dr.IsDBNull(iTH24)) entity.TH24 = dr.GetDecimal(iTH24);

            int iTH25 = dr.GetOrdinal(this.TH25);
            if (!dr.IsDBNull(iTH25)) entity.TH25 = dr.GetDecimal(iTH25);

            int iTH26 = dr.GetOrdinal(this.TH26);
            if (!dr.IsDBNull(iTH26)) entity.TH26 = dr.GetDecimal(iTH26);

            int iTH27 = dr.GetOrdinal(this.TH27);
            if (!dr.IsDBNull(iTH27)) entity.TH27 = dr.GetDecimal(iTH27);

            int iTH28 = dr.GetOrdinal(this.TH28);
            if (!dr.IsDBNull(iTH28)) entity.TH28 = dr.GetDecimal(iTH28);

            int iTH29 = dr.GetOrdinal(this.TH29);
            if (!dr.IsDBNull(iTH29)) entity.TH29 = dr.GetDecimal(iTH29);

            int iTH30 = dr.GetOrdinal(this.TH30);
            if (!dr.IsDBNull(iTH30)) entity.TH30 = dr.GetDecimal(iTH30);

            int iTH31 = dr.GetOrdinal(this.TH31);
            if (!dr.IsDBNull(iTH31)) entity.TH31 = dr.GetDecimal(iTH31);

            int iTH32 = dr.GetOrdinal(this.TH32);
            if (!dr.IsDBNull(iTH32)) entity.TH32 = dr.GetDecimal(iTH32);

            int iTH33 = dr.GetOrdinal(this.TH33);
            if (!dr.IsDBNull(iTH33)) entity.TH33 = dr.GetDecimal(iTH33);

            int iTH34 = dr.GetOrdinal(this.TH34);
            if (!dr.IsDBNull(iTH34)) entity.TH34 = dr.GetDecimal(iTH34);

            int iTH35 = dr.GetOrdinal(this.TH35);
            if (!dr.IsDBNull(iTH35)) entity.TH35 = dr.GetDecimal(iTH35);

            int iTH36 = dr.GetOrdinal(this.TH36);
            if (!dr.IsDBNull(iTH36)) entity.TH36 = dr.GetDecimal(iTH36);

            int iTH37 = dr.GetOrdinal(this.TH37);
            if (!dr.IsDBNull(iTH37)) entity.TH37 = dr.GetDecimal(iTH37);

            int iTH38 = dr.GetOrdinal(this.TH38);
            if (!dr.IsDBNull(iTH38)) entity.TH38 = dr.GetDecimal(iTH38);

            int iTH39 = dr.GetOrdinal(this.TH39);
            if (!dr.IsDBNull(iTH39)) entity.TH39 = dr.GetDecimal(iTH39);

            int iTH40 = dr.GetOrdinal(this.TH40);
            if (!dr.IsDBNull(iTH40)) entity.TH40 = dr.GetDecimal(iTH40);

            int iTH41 = dr.GetOrdinal(this.TH41);
            if (!dr.IsDBNull(iTH41)) entity.TH41 = dr.GetDecimal(iTH41);

            int iTH42 = dr.GetOrdinal(this.TH42);
            if (!dr.IsDBNull(iTH42)) entity.TH42 = dr.GetDecimal(iTH42);

            int iTH43 = dr.GetOrdinal(this.TH43);
            if (!dr.IsDBNull(iTH43)) entity.TH43 = dr.GetDecimal(iTH43);

            int iTH44 = dr.GetOrdinal(this.TH44);
            if (!dr.IsDBNull(iTH44)) entity.TH44 = dr.GetDecimal(iTH44);

            int iTH45 = dr.GetOrdinal(this.TH45);
            if (!dr.IsDBNull(iTH45)) entity.TH45 = dr.GetDecimal(iTH45);

            int iTH46 = dr.GetOrdinal(this.TH46);
            if (!dr.IsDBNull(iTH46)) entity.TH46 = dr.GetDecimal(iTH46);

            int iTH47 = dr.GetOrdinal(this.TH47);
            if (!dr.IsDBNull(iTH47)) entity.TH47 = dr.GetDecimal(iTH47);

            int iTH48 = dr.GetOrdinal(this.TH48);
            if (!dr.IsDBNull(iTH48)) entity.TH48 = dr.GetDecimal(iTH48);
            
            return entity;
        }


        #region Mapeo        

        public string CANALCODI = "CANALCODI";
        public string PTOMEDICODI = "PTOMEDICODI";
        public string MEDIFECHA = "MEDIFECHA";
        public string MEDIESTADO = "MEDIESTADO";
        public string FECHAINICIO = "FECHAINICIO";
        public string FECHAFIN = "FECHAFIN";
        public string H2 = "H2";
        public string H4 = "H4";
        public string H6 = "H6";
        public string H8 = "H8";
        public string H10 = "H10";
        public string H12 = "H12";
        public string H14 = "H14";
        public string H16 = "H16";
        public string H18 = "H18";
        public string H20 = "H20";
        public string H22 = "H22";
        public string H24 = "H24";
        public string H26 = "H26";
        public string H28 = "H28";
        public string H30 = "H30";
        public string H32 = "H32";
        public string H34 = "H34";
        public string H36 = "H36";
        public string H38 = "H38";
        public string H40 = "H40";
        public string H42 = "H42";
        public string H44 = "H44";
        public string H46 = "H46";
        public string H48 = "H48";
        public string H50 = "H50";
        public string H52 = "H52";
        public string H54 = "H54";
        public string H56 = "H56";
        public string H58 = "H58";
        public string H60 = "H60";
        public string H62 = "H62";
        public string H64 = "H64";
        public string H66 = "H66";
        public string H68 = "H68";
        public string H70 = "H70";
        public string H72 = "H72";
        public string H74 = "H74";
        public string H76 = "H76";
        public string H78 = "H78";
        public string H80 = "H80";
        public string H82 = "H82";
        public string H84 = "H84";
        public string H86 = "H86";
        public string H88 = "H88";
        public string H90 = "H90";
        public string H92 = "H92";
        public string H94 = "H94";
        public string H96 = "H96";

        public string PERFCODI = "PERFCODI";
        public string PERFDESC = "PERFDESC";
        public string FECREGISTRO = "FECREGISTRO";
        public string FECINICIO = "FECINICIO";
        public string FECFIN = "FECFIN";
        public string LASTUSER = "LASTUSER";
        public string LASTDATE = "LASTDATE";
        public string EJRUCODI = "EJRUCODI";
        public string PRRUNOMB = "PRRUNOMB";
        public string PRRUABREV = "PRRUABREV";
        public string AREACODE = "AREACODE";        
        public string PERFDETCODI = "PERFDETCODI";
        public string PERFCLASI = "PERFCLASI";
        public string PERFORIG = "PERFORIG";

        public string H1 = "H1";        
        public string H3 = "H3";        
        public string H5 = "H5";        
        public string H7 = "H7";        
        public string H9 = "H9";
        public string H11 = "H11";        
        public string H13 = "H13";        
        public string H15 = "H15";        
        public string H17 = "H17";        
        public string H19 = "H19";        
        public string H21 = "H21";        
        public string H23 = "H23";        
        public string H25 = "H25";        
        public string H27 = "H27";        
        public string H29 = "H29";        
        public string H31 = "H31";        
        public string H33 = "H33";        
        public string H35 = "H35";        
        public string H37 = "H37";        
        public string H39 = "H39";        
        public string H41 = "H41";        
        public string H43 = "H43";        
        public string H45 = "H45";        
        public string H47 = "H47";        
        public string TH1 = "TH1";
        public string TH2 = "TH2";
        public string TH3 = "TH3";
        public string TH4 = "TH4";
        public string TH5 = "TH5";
        public string TH6 = "TH6";
        public string TH7 = "TH7";
        public string TH8 = "TH8";
        public string TH9 = "TH9";
        public string TH10 = "TH10";
        public string TH11 = "TH11";
        public string TH12 = "TH12";
        public string TH13 = "TH13";
        public string TH14 = "TH14";
        public string TH15 = "TH15";
        public string TH16 = "TH16";
        public string TH17 = "TH17";
        public string TH18 = "TH18";
        public string TH19 = "TH19";
        public string TH20 = "TH20";
        public string TH21 = "TH21";
        public string TH22 = "TH22";
        public string TH23 = "TH23";
        public string TH24 = "TH24";
        public string TH25 = "TH25";
        public string TH26 = "TH26";
        public string TH27 = "TH27";
        public string TH28 = "TH28";
        public string TH29 = "TH29";
        public string TH30 = "TH30";
        public string TH31 = "TH31";
        public string TH32 = "TH32";
        public string TH33 = "TH33";
        public string TH34 = "TH34";
        public string TH35 = "TH35";
        public string TH36 = "TH36";
        public string TH37 = "TH37";
        public string TH38 = "TH38";
        public string TH39 = "TH39";
        public string TH40 = "TH40";
        public string TH41 = "TH41";
        public string TH42 = "TH42";
        public string TH43 = "TH43";
        public string TH44 = "TH44";
        public string TH45 = "TH45";
        public string TH46 = "TH46";
        public string TH47 = "TH47";
        public string TH48 = "TH48";


        public string SqlGetFromScada
        {
            get { return base.GetSqlXml("GetFromScada"); }
        }

        public string SqlGetFromMedicion
        {
            get { return base.GetSqlXml("GetFromMedicion"); }
        }

        public string SqlGetFromMedicionMedDistribucion
        {
            get { return base.GetSqlXml("GetFromMedicionMedDistribucion"); }
        }

        public string SqlGetFromDemandaULyD
        {
            get { return base.GetSqlXml("GetFromDemandaULyD"); }
        }

        public string SqlGetFromMedicionMedGeneracion
        {
            get { return base.GetSqlXml("GetFromMedicionMedGeneracion"); }
        }

        public string SqlSaveDetalle
        {
            get { return base.GetSqlXml("SaveDetalle"); }
        }
               

        public string SqlGetMaxIdDetalle
        {
            get { return base.GetSqlXml("GetMaxIdDetalle"); }
        }

        public string SqlBuscarPerfiles
        {
            get { return base.GetSqlXml("BuscarPerfiles"); }
        }

        public string SqlObtenerExportacion
        {
            get { return base.GetSqlXml("ObtenerExportacion"); }
        }

        public string SqlGetByFormula
        {
            get { return base.GetSqlXml("ObtenerPorFormula"); }
        }

        public string SqlObtenerCongestion
        {
            get { return base.GetSqlXml("ObtenerCongestion"); }
        }

        public string SqlGetFromScadaSP7
        {
            get { return base.GetSqlXml("GetFromScadaSP7"); }
        }

        #endregion

    }
}
