using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class PrnMedicionesRawRepository : RepositoryBase, IPrnMedicionesRawRepository
    {
        public PrnMedicionesRawRepository(string strConn)
         : base(strConn)
        {
        }

        PrnMedicionesRawHelper helper = new PrnMedicionesRawHelper();

        public List<PrnMedicionesRawDTO> List()
        {
            List<PrnMedicionesRawDTO> entitys = new List<PrnMedicionesRawDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnMedicionesRawDTO entity = new PrnMedicionesRawDTO();

                    int iMedirawcodi = dr.GetOrdinal(helper.Medirawcodi);
                    if (!dr.IsDBNull(iMedirawcodi)) entity.Medirawcodi = Convert.ToInt32(dr.GetValue(iMedirawcodi));

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPrnvarcodi = dr.GetOrdinal(helper.Prnvarcodi);
                    if (!dr.IsDBNull(iPrnvarcodi)) entity.Prnvarcodi = Convert.ToInt32(dr.GetValue(iPrnvarcodi));

                    int iMedirawfuente = dr.GetOrdinal(helper.Medirawfuente);
                    if (!dr.IsDBNull(iMedirawfuente)) entity.Medirawfuente = dr.GetString(iMedirawfuente);

                    int iMedirawfecha = dr.GetOrdinal(helper.Medirawfecha);
                    if (!dr.IsDBNull(iMedirawfecha)) entity.Medirawfecha = dr.GetDateTime(iMedirawfecha);

                    int iMedirawtipomedi = dr.GetOrdinal(helper.Medirawtipomedi);
                    if (!dr.IsDBNull(iMedirawtipomedi)) entity.Medirawtipomedi = dr.GetString(iMedirawtipomedi);

                    int iH1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH49 = dr.GetOrdinal(helper.H49);
                    if (!dr.IsDBNull(iH49)) entity.H49 = dr.GetDecimal(iH49);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH51 = dr.GetOrdinal(helper.H51);
                    if (!dr.IsDBNull(iH51)) entity.H51 = dr.GetDecimal(iH51);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH53 = dr.GetOrdinal(helper.H53);
                    if (!dr.IsDBNull(iH53)) entity.H53 = dr.GetDecimal(iH53);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH55 = dr.GetOrdinal(helper.H55);
                    if (!dr.IsDBNull(iH55)) entity.H55 = dr.GetDecimal(iH55);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH57 = dr.GetOrdinal(helper.H57);
                    if (!dr.IsDBNull(iH57)) entity.H57 = dr.GetDecimal(iH57);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH59 = dr.GetOrdinal(helper.H59);
                    if (!dr.IsDBNull(iH59)) entity.H59 = dr.GetDecimal(iH59);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH61 = dr.GetOrdinal(helper.H61);
                    if (!dr.IsDBNull(iH61)) entity.H61 = dr.GetDecimal(iH61);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH63 = dr.GetOrdinal(helper.H63);
                    if (!dr.IsDBNull(iH63)) entity.H63 = dr.GetDecimal(iH63);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH65 = dr.GetOrdinal(helper.H65);
                    if (!dr.IsDBNull(iH65)) entity.H65 = dr.GetDecimal(iH65);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH67 = dr.GetOrdinal(helper.H67);
                    if (!dr.IsDBNull(iH67)) entity.H67 = dr.GetDecimal(iH67);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH69 = dr.GetOrdinal(helper.H69);
                    if (!dr.IsDBNull(iH69)) entity.H69 = dr.GetDecimal(iH69);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH71 = dr.GetOrdinal(helper.H71);
                    if (!dr.IsDBNull(iH71)) entity.H71 = dr.GetDecimal(iH71);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH73 = dr.GetOrdinal(helper.H73);
                    if (!dr.IsDBNull(iH73)) entity.H73 = dr.GetDecimal(iH73);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH75 = dr.GetOrdinal(helper.H75);
                    if (!dr.IsDBNull(iH75)) entity.H75 = dr.GetDecimal(iH75);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH77 = dr.GetOrdinal(helper.H77);
                    if (!dr.IsDBNull(iH77)) entity.H77 = dr.GetDecimal(iH77);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH79 = dr.GetOrdinal(helper.H79);
                    if (!dr.IsDBNull(iH79)) entity.H79 = dr.GetDecimal(iH79);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH81 = dr.GetOrdinal(helper.H81);
                    if (!dr.IsDBNull(iH81)) entity.H81 = dr.GetDecimal(iH81);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH83 = dr.GetOrdinal(helper.H83);
                    if (!dr.IsDBNull(iH83)) entity.H83 = dr.GetDecimal(iH83);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH85 = dr.GetOrdinal(helper.H85);
                    if (!dr.IsDBNull(iH85)) entity.H85 = dr.GetDecimal(iH85);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH87 = dr.GetOrdinal(helper.H87);
                    if (!dr.IsDBNull(iH87)) entity.H87 = dr.GetDecimal(iH87);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH89 = dr.GetOrdinal(helper.H89);
                    if (!dr.IsDBNull(iH89)) entity.H89 = dr.GetDecimal(iH89);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH91 = dr.GetOrdinal(helper.H91);
                    if (!dr.IsDBNull(iH91)) entity.H91 = dr.GetDecimal(iH91);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH93 = dr.GetOrdinal(helper.H93);
                    if (!dr.IsDBNull(iH93)) entity.H93 = dr.GetDecimal(iH93);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH95 = dr.GetOrdinal(helper.H95);
                    if (!dr.IsDBNull(iH95)) entity.H95 = dr.GetDecimal(iH95);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);

                    int iMedirawusucreacion = dr.GetOrdinal(helper.Medirawusucreacion);
                    if (!dr.IsDBNull(iMedirawusucreacion)) entity.Medirawusucreacion = dr.GetString(iMedirawusucreacion);

                    int iMedirawfeccreacion = dr.GetOrdinal(helper.Medirawfeccreacion);
                    if (!dr.IsDBNull(iMedirawfeccreacion)) entity.Medirawfeccreacion = dr.GetDateTime(iMedirawfeccreacion);

                    int iMedirawusumodificacion = dr.GetOrdinal(helper.Medirawusumodificacion);
                    if (!dr.IsDBNull(iMedirawusumodificacion)) entity.Medirawusumodificacion = dr.GetString(iMedirawusumodificacion);

                    int iMedirawfecmodificacion = dr.GetOrdinal(helper.Medirawfecmodificacion);
                    if (!dr.IsDBNull(iMedirawfecmodificacion)) entity.Medirawfecmodificacion = dr.GetDateTime(iMedirawfecmodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void Save(PrnMedicionesRawDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Medirawcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Prnvarcodi, DbType.Int32, entity.Prnvarcodi);
            dbProvider.AddInParameter(command, helper.Medirawfuente, DbType.String, entity.Medirawfuente);
            dbProvider.AddInParameter(command, helper.Medirawfecha, DbType.DateTime, entity.Medirawfecha);
            dbProvider.AddInParameter(command, helper.Medirawtipomedi, DbType.String, entity.Medirawtipomedi);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.H2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.H3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.H4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.H5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.H6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.H7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.H8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.H9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.H10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.H11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.H12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.H13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.H14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.H15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.H16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.H17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.H18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.H19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.H20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.H21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.H22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.H23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.H24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.H25, DbType.Decimal, entity.H25);
            dbProvider.AddInParameter(command, helper.H26, DbType.Decimal, entity.H26);
            dbProvider.AddInParameter(command, helper.H27, DbType.Decimal, entity.H27);
            dbProvider.AddInParameter(command, helper.H28, DbType.Decimal, entity.H28);
            dbProvider.AddInParameter(command, helper.H29, DbType.Decimal, entity.H29);
            dbProvider.AddInParameter(command, helper.H30, DbType.Decimal, entity.H30);
            dbProvider.AddInParameter(command, helper.H31, DbType.Decimal, entity.H31);
            dbProvider.AddInParameter(command, helper.H32, DbType.Decimal, entity.H32);
            dbProvider.AddInParameter(command, helper.H33, DbType.Decimal, entity.H33);
            dbProvider.AddInParameter(command, helper.H34, DbType.Decimal, entity.H34);
            dbProvider.AddInParameter(command, helper.H35, DbType.Decimal, entity.H35);
            dbProvider.AddInParameter(command, helper.H36, DbType.Decimal, entity.H36);
            dbProvider.AddInParameter(command, helper.H37, DbType.Decimal, entity.H37);
            dbProvider.AddInParameter(command, helper.H38, DbType.Decimal, entity.H38);
            dbProvider.AddInParameter(command, helper.H39, DbType.Decimal, entity.H39);
            dbProvider.AddInParameter(command, helper.H40, DbType.Decimal, entity.H40);
            dbProvider.AddInParameter(command, helper.H41, DbType.Decimal, entity.H41);
            dbProvider.AddInParameter(command, helper.H42, DbType.Decimal, entity.H42);
            dbProvider.AddInParameter(command, helper.H43, DbType.Decimal, entity.H43);
            dbProvider.AddInParameter(command, helper.H44, DbType.Decimal, entity.H44);
            dbProvider.AddInParameter(command, helper.H45, DbType.Decimal, entity.H45);
            dbProvider.AddInParameter(command, helper.H46, DbType.Decimal, entity.H46);
            dbProvider.AddInParameter(command, helper.H47, DbType.Decimal, entity.H47);
            dbProvider.AddInParameter(command, helper.H48, DbType.Decimal, entity.H48);
            dbProvider.AddInParameter(command, helper.H49, DbType.Decimal, entity.H49);
            dbProvider.AddInParameter(command, helper.H50, DbType.Decimal, entity.H50);
            dbProvider.AddInParameter(command, helper.H51, DbType.Decimal, entity.H51);
            dbProvider.AddInParameter(command, helper.H52, DbType.Decimal, entity.H52);
            dbProvider.AddInParameter(command, helper.H53, DbType.Decimal, entity.H53);
            dbProvider.AddInParameter(command, helper.H54, DbType.Decimal, entity.H54);
            dbProvider.AddInParameter(command, helper.H55, DbType.Decimal, entity.H55);
            dbProvider.AddInParameter(command, helper.H56, DbType.Decimal, entity.H56);
            dbProvider.AddInParameter(command, helper.H57, DbType.Decimal, entity.H57);
            dbProvider.AddInParameter(command, helper.H58, DbType.Decimal, entity.H58);
            dbProvider.AddInParameter(command, helper.H59, DbType.Decimal, entity.H59);
            dbProvider.AddInParameter(command, helper.H60, DbType.Decimal, entity.H60);
            dbProvider.AddInParameter(command, helper.H61, DbType.Decimal, entity.H61);
            dbProvider.AddInParameter(command, helper.H62, DbType.Decimal, entity.H62);
            dbProvider.AddInParameter(command, helper.H63, DbType.Decimal, entity.H63);
            dbProvider.AddInParameter(command, helper.H64, DbType.Decimal, entity.H64);
            dbProvider.AddInParameter(command, helper.H65, DbType.Decimal, entity.H65);
            dbProvider.AddInParameter(command, helper.H66, DbType.Decimal, entity.H66);
            dbProvider.AddInParameter(command, helper.H67, DbType.Decimal, entity.H67);
            dbProvider.AddInParameter(command, helper.H68, DbType.Decimal, entity.H68);
            dbProvider.AddInParameter(command, helper.H69, DbType.Decimal, entity.H69);
            dbProvider.AddInParameter(command, helper.H70, DbType.Decimal, entity.H70);
            dbProvider.AddInParameter(command, helper.H71, DbType.Decimal, entity.H71);
            dbProvider.AddInParameter(command, helper.H72, DbType.Decimal, entity.H72);
            dbProvider.AddInParameter(command, helper.H73, DbType.Decimal, entity.H73);
            dbProvider.AddInParameter(command, helper.H74, DbType.Decimal, entity.H74);
            dbProvider.AddInParameter(command, helper.H75, DbType.Decimal, entity.H75);
            dbProvider.AddInParameter(command, helper.H76, DbType.Decimal, entity.H76);
            dbProvider.AddInParameter(command, helper.H77, DbType.Decimal, entity.H77);
            dbProvider.AddInParameter(command, helper.H78, DbType.Decimal, entity.H78);
            dbProvider.AddInParameter(command, helper.H79, DbType.Decimal, entity.H79);
            dbProvider.AddInParameter(command, helper.H80, DbType.Decimal, entity.H80);
            dbProvider.AddInParameter(command, helper.H81, DbType.Decimal, entity.H81);
            dbProvider.AddInParameter(command, helper.H82, DbType.Decimal, entity.H82);
            dbProvider.AddInParameter(command, helper.H83, DbType.Decimal, entity.H83);
            dbProvider.AddInParameter(command, helper.H84, DbType.Decimal, entity.H84);
            dbProvider.AddInParameter(command, helper.H85, DbType.Decimal, entity.H85);
            dbProvider.AddInParameter(command, helper.H86, DbType.Decimal, entity.H86);
            dbProvider.AddInParameter(command, helper.H87, DbType.Decimal, entity.H87);
            dbProvider.AddInParameter(command, helper.H88, DbType.Decimal, entity.H88);
            dbProvider.AddInParameter(command, helper.H89, DbType.Decimal, entity.H89);
            dbProvider.AddInParameter(command, helper.H90, DbType.Decimal, entity.H90);
            dbProvider.AddInParameter(command, helper.H91, DbType.Decimal, entity.H91);
            dbProvider.AddInParameter(command, helper.H92, DbType.Decimal, entity.H92);
            dbProvider.AddInParameter(command, helper.H93, DbType.Decimal, entity.H93);
            dbProvider.AddInParameter(command, helper.H94, DbType.Decimal, entity.H94);
            dbProvider.AddInParameter(command, helper.H95, DbType.Decimal, entity.H95);
            dbProvider.AddInParameter(command, helper.H96, DbType.Decimal, entity.H96);
            dbProvider.AddInParameter(command, helper.Medirawusucreacion, DbType.String, entity.Medirawusucreacion);
            dbProvider.AddInParameter(command, helper.Medirawfeccreacion, DbType.DateTime, entity.Medirawfeccreacion);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(PrnMedicionesRawDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Prnvarcodi, DbType.Int32, entity.Prnvarcodi);
            dbProvider.AddInParameter(command, helper.Medirawfuente, DbType.String, entity.Medirawfuente);
            dbProvider.AddInParameter(command, helper.Medirawfecha, DbType.DateTime, entity.Medirawfecha);
            dbProvider.AddInParameter(command, helper.Medirawtipomedi, DbType.String, entity.Medirawtipomedi);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.H2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.H3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.H4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.H5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.H6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.H7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.H8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.H9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.H10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.H11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.H12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.H13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.H14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.H15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.H16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.H17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.H18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.H19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.H20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.H21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.H22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.H23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.H24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.H25, DbType.Decimal, entity.H25);
            dbProvider.AddInParameter(command, helper.H26, DbType.Decimal, entity.H26);
            dbProvider.AddInParameter(command, helper.H27, DbType.Decimal, entity.H27);
            dbProvider.AddInParameter(command, helper.H28, DbType.Decimal, entity.H28);
            dbProvider.AddInParameter(command, helper.H29, DbType.Decimal, entity.H29);
            dbProvider.AddInParameter(command, helper.H30, DbType.Decimal, entity.H30);
            dbProvider.AddInParameter(command, helper.H31, DbType.Decimal, entity.H31);
            dbProvider.AddInParameter(command, helper.H32, DbType.Decimal, entity.H32);
            dbProvider.AddInParameter(command, helper.H33, DbType.Decimal, entity.H33);
            dbProvider.AddInParameter(command, helper.H34, DbType.Decimal, entity.H34);
            dbProvider.AddInParameter(command, helper.H35, DbType.Decimal, entity.H35);
            dbProvider.AddInParameter(command, helper.H36, DbType.Decimal, entity.H36);
            dbProvider.AddInParameter(command, helper.H37, DbType.Decimal, entity.H37);
            dbProvider.AddInParameter(command, helper.H38, DbType.Decimal, entity.H38);
            dbProvider.AddInParameter(command, helper.H39, DbType.Decimal, entity.H39);
            dbProvider.AddInParameter(command, helper.H40, DbType.Decimal, entity.H40);
            dbProvider.AddInParameter(command, helper.H41, DbType.Decimal, entity.H41);
            dbProvider.AddInParameter(command, helper.H42, DbType.Decimal, entity.H42);
            dbProvider.AddInParameter(command, helper.H43, DbType.Decimal, entity.H43);
            dbProvider.AddInParameter(command, helper.H44, DbType.Decimal, entity.H44);
            dbProvider.AddInParameter(command, helper.H45, DbType.Decimal, entity.H45);
            dbProvider.AddInParameter(command, helper.H46, DbType.Decimal, entity.H46);
            dbProvider.AddInParameter(command, helper.H47, DbType.Decimal, entity.H47);
            dbProvider.AddInParameter(command, helper.H48, DbType.Decimal, entity.H48);
            dbProvider.AddInParameter(command, helper.H49, DbType.Decimal, entity.H49);
            dbProvider.AddInParameter(command, helper.H50, DbType.Decimal, entity.H50);
            dbProvider.AddInParameter(command, helper.H51, DbType.Decimal, entity.H51);
            dbProvider.AddInParameter(command, helper.H52, DbType.Decimal, entity.H52);
            dbProvider.AddInParameter(command, helper.H53, DbType.Decimal, entity.H53);
            dbProvider.AddInParameter(command, helper.H54, DbType.Decimal, entity.H54);
            dbProvider.AddInParameter(command, helper.H55, DbType.Decimal, entity.H55);
            dbProvider.AddInParameter(command, helper.H56, DbType.Decimal, entity.H56);
            dbProvider.AddInParameter(command, helper.H57, DbType.Decimal, entity.H57);
            dbProvider.AddInParameter(command, helper.H58, DbType.Decimal, entity.H58);
            dbProvider.AddInParameter(command, helper.H59, DbType.Decimal, entity.H59);
            dbProvider.AddInParameter(command, helper.H60, DbType.Decimal, entity.H60);
            dbProvider.AddInParameter(command, helper.H61, DbType.Decimal, entity.H61);
            dbProvider.AddInParameter(command, helper.H62, DbType.Decimal, entity.H62);
            dbProvider.AddInParameter(command, helper.H63, DbType.Decimal, entity.H63);
            dbProvider.AddInParameter(command, helper.H64, DbType.Decimal, entity.H64);
            dbProvider.AddInParameter(command, helper.H65, DbType.Decimal, entity.H65);
            dbProvider.AddInParameter(command, helper.H66, DbType.Decimal, entity.H66);
            dbProvider.AddInParameter(command, helper.H67, DbType.Decimal, entity.H67);
            dbProvider.AddInParameter(command, helper.H68, DbType.Decimal, entity.H68);
            dbProvider.AddInParameter(command, helper.H69, DbType.Decimal, entity.H69);
            dbProvider.AddInParameter(command, helper.H70, DbType.Decimal, entity.H70);
            dbProvider.AddInParameter(command, helper.H71, DbType.Decimal, entity.H71);
            dbProvider.AddInParameter(command, helper.H72, DbType.Decimal, entity.H72);
            dbProvider.AddInParameter(command, helper.H73, DbType.Decimal, entity.H73);
            dbProvider.AddInParameter(command, helper.H74, DbType.Decimal, entity.H74);
            dbProvider.AddInParameter(command, helper.H75, DbType.Decimal, entity.H75);
            dbProvider.AddInParameter(command, helper.H76, DbType.Decimal, entity.H76);
            dbProvider.AddInParameter(command, helper.H77, DbType.Decimal, entity.H77);
            dbProvider.AddInParameter(command, helper.H78, DbType.Decimal, entity.H78);
            dbProvider.AddInParameter(command, helper.H79, DbType.Decimal, entity.H79);
            dbProvider.AddInParameter(command, helper.H80, DbType.Decimal, entity.H80);
            dbProvider.AddInParameter(command, helper.H81, DbType.Decimal, entity.H81);
            dbProvider.AddInParameter(command, helper.H82, DbType.Decimal, entity.H82);
            dbProvider.AddInParameter(command, helper.H83, DbType.Decimal, entity.H83);
            dbProvider.AddInParameter(command, helper.H84, DbType.Decimal, entity.H84);
            dbProvider.AddInParameter(command, helper.H85, DbType.Decimal, entity.H85);
            dbProvider.AddInParameter(command, helper.H86, DbType.Decimal, entity.H86);
            dbProvider.AddInParameter(command, helper.H87, DbType.Decimal, entity.H87);
            dbProvider.AddInParameter(command, helper.H88, DbType.Decimal, entity.H88);
            dbProvider.AddInParameter(command, helper.H89, DbType.Decimal, entity.H89);
            dbProvider.AddInParameter(command, helper.H90, DbType.Decimal, entity.H90);
            dbProvider.AddInParameter(command, helper.H91, DbType.Decimal, entity.H91);
            dbProvider.AddInParameter(command, helper.H92, DbType.Decimal, entity.H92);
            dbProvider.AddInParameter(command, helper.H93, DbType.Decimal, entity.H93);
            dbProvider.AddInParameter(command, helper.H94, DbType.Decimal, entity.H94);
            dbProvider.AddInParameter(command, helper.H95, DbType.Decimal, entity.H95);
            dbProvider.AddInParameter(command, helper.H96, DbType.Decimal, entity.H96);
            dbProvider.AddInParameter(command, helper.Medirawusumodificacion, DbType.String, entity.Medirawusumodificacion);
            dbProvider.AddInParameter(command, helper.Medirawfecmodificacion, DbType.DateTime, entity.Medirawfecmodificacion);
            dbProvider.AddInParameter(command, helper.Medirawcodi, DbType.Int32, entity.Medirawcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public PrnMedicionesRawDTO GetById(int codigo)
        {
            PrnMedicionesRawDTO entity = new PrnMedicionesRawDTO();

            string query = string.Format(helper.SqlGetById, codigo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public void Delete(int codigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Medirawcodi, DbType.Int32, codigo);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<PrnMedicionesRawDTO> ListMedicionesRaw(int unidad, DateTime fecha, int idVariable, string idFuente, string modulo)
        {
            List<PrnMedicionesRawDTO> entitys = new List<PrnMedicionesRawDTO>();
            string query = string.Format(helper.SqlListMedicionesRaw, unidad, fecha.ToString("dd/MM/yyyy"), idVariable, idFuente, modulo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnMedicionesRawDTO entity = new PrnMedicionesRawDTO();

                    int iMedirawcodi = dr.GetOrdinal(helper.Medirawcodi);
                    if (!dr.IsDBNull(iMedirawcodi)) entity.Medirawcodi = Convert.ToInt32(dr.GetValue(iMedirawcodi));

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPrnvarcodi = dr.GetOrdinal(helper.Prnvarcodi);
                    if (!dr.IsDBNull(iPrnvarcodi)) entity.Prnvarcodi = Convert.ToInt32(dr.GetValue(iPrnvarcodi));

                    int iMedirawfuente = dr.GetOrdinal(helper.Medirawfuente);
                    if (!dr.IsDBNull(iMedirawfuente)) entity.Medirawfuente = dr.GetString(iMedirawfuente);

                    int iMedirawfecha = dr.GetOrdinal(helper.Medirawfecha);
                    if (!dr.IsDBNull(iMedirawfecha)) entity.Medirawfecha = dr.GetDateTime(iMedirawfecha);

                    int iMedirawtipomedi = dr.GetOrdinal(helper.Medirawtipomedi);
                    if (!dr.IsDBNull(iMedirawtipomedi)) entity.Medirawtipomedi = dr.GetString(iMedirawtipomedi);

                    int iH1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH49 = dr.GetOrdinal(helper.H49);
                    if (!dr.IsDBNull(iH49)) entity.H49 = dr.GetDecimal(iH49);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH51 = dr.GetOrdinal(helper.H51);
                    if (!dr.IsDBNull(iH51)) entity.H51 = dr.GetDecimal(iH51);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH53 = dr.GetOrdinal(helper.H53);
                    if (!dr.IsDBNull(iH53)) entity.H53 = dr.GetDecimal(iH53);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH55 = dr.GetOrdinal(helper.H55);
                    if (!dr.IsDBNull(iH55)) entity.H55 = dr.GetDecimal(iH55);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH57 = dr.GetOrdinal(helper.H57);
                    if (!dr.IsDBNull(iH57)) entity.H57 = dr.GetDecimal(iH57);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH59 = dr.GetOrdinal(helper.H59);
                    if (!dr.IsDBNull(iH59)) entity.H59 = dr.GetDecimal(iH59);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH61 = dr.GetOrdinal(helper.H61);
                    if (!dr.IsDBNull(iH61)) entity.H61 = dr.GetDecimal(iH61);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH63 = dr.GetOrdinal(helper.H63);
                    if (!dr.IsDBNull(iH63)) entity.H63 = dr.GetDecimal(iH63);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH65 = dr.GetOrdinal(helper.H65);
                    if (!dr.IsDBNull(iH65)) entity.H65 = dr.GetDecimal(iH65);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH67 = dr.GetOrdinal(helper.H67);
                    if (!dr.IsDBNull(iH67)) entity.H67 = dr.GetDecimal(iH67);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH69 = dr.GetOrdinal(helper.H69);
                    if (!dr.IsDBNull(iH69)) entity.H69 = dr.GetDecimal(iH69);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH71 = dr.GetOrdinal(helper.H71);
                    if (!dr.IsDBNull(iH71)) entity.H71 = dr.GetDecimal(iH71);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH73 = dr.GetOrdinal(helper.H73);
                    if (!dr.IsDBNull(iH73)) entity.H73 = dr.GetDecimal(iH73);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH75 = dr.GetOrdinal(helper.H75);
                    if (!dr.IsDBNull(iH75)) entity.H75 = dr.GetDecimal(iH75);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH77 = dr.GetOrdinal(helper.H77);
                    if (!dr.IsDBNull(iH77)) entity.H77 = dr.GetDecimal(iH77);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH79 = dr.GetOrdinal(helper.H79);
                    if (!dr.IsDBNull(iH79)) entity.H79 = dr.GetDecimal(iH79);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH81 = dr.GetOrdinal(helper.H81);
                    if (!dr.IsDBNull(iH81)) entity.H81 = dr.GetDecimal(iH81);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH83 = dr.GetOrdinal(helper.H83);
                    if (!dr.IsDBNull(iH83)) entity.H83 = dr.GetDecimal(iH83);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH85 = dr.GetOrdinal(helper.H85);
                    if (!dr.IsDBNull(iH85)) entity.H85 = dr.GetDecimal(iH85);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH87 = dr.GetOrdinal(helper.H87);
                    if (!dr.IsDBNull(iH87)) entity.H87 = dr.GetDecimal(iH87);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH89 = dr.GetOrdinal(helper.H89);
                    if (!dr.IsDBNull(iH89)) entity.H89 = dr.GetDecimal(iH89);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH91 = dr.GetOrdinal(helper.H91);
                    if (!dr.IsDBNull(iH91)) entity.H91 = dr.GetDecimal(iH91);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH93 = dr.GetOrdinal(helper.H93);
                    if (!dr.IsDBNull(iH93)) entity.H93 = dr.GetDecimal(iH93);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH95 = dr.GetOrdinal(helper.H95);
                    if (!dr.IsDBNull(iH95)) entity.H95 = dr.GetDecimal(iH95);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnMedicionesRawDTO> ListMedicionesRawPorRango(int unidad, DateTime fechaInicio, DateTime fechaFin, int idVariable, string idFuente, string modulo)
        {
            List<PrnMedicionesRawDTO> entitys = new List<PrnMedicionesRawDTO>();
            string query = string.Format(helper.SqlListMedicionesRawPorRango, unidad, fechaInicio.ToString("dd/MM/yyyy"), fechaFin.ToString("dd/MM/yyyy"), idVariable, idFuente, modulo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnMedicionesRawDTO entity = new PrnMedicionesRawDTO();

                    int iMedirawcodi = dr.GetOrdinal(helper.Medirawcodi);
                    if (!dr.IsDBNull(iMedirawcodi)) entity.Medirawcodi = Convert.ToInt32(dr.GetValue(iMedirawcodi));

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPrnvarcodi = dr.GetOrdinal(helper.Prnvarcodi);
                    if (!dr.IsDBNull(iPrnvarcodi)) entity.Prnvarcodi = Convert.ToInt32(dr.GetValue(iPrnvarcodi));

                    int iMedirawfuente = dr.GetOrdinal(helper.Medirawfuente);
                    if (!dr.IsDBNull(iMedirawfuente)) entity.Medirawfuente = dr.GetString(iMedirawfuente);

                    int iMedirawfecha = dr.GetOrdinal(helper.Medirawfecha);
                    if (!dr.IsDBNull(iMedirawfecha)) entity.Medirawfecha = dr.GetDateTime(iMedirawfecha);

                    int iMedirawtipomedi = dr.GetOrdinal(helper.Medirawtipomedi);
                    if (!dr.IsDBNull(iMedirawtipomedi)) entity.Medirawtipomedi = dr.GetString(iMedirawtipomedi);

                    int iH1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH49 = dr.GetOrdinal(helper.H49);
                    if (!dr.IsDBNull(iH49)) entity.H49 = dr.GetDecimal(iH49);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH51 = dr.GetOrdinal(helper.H51);
                    if (!dr.IsDBNull(iH51)) entity.H51 = dr.GetDecimal(iH51);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH53 = dr.GetOrdinal(helper.H53);
                    if (!dr.IsDBNull(iH53)) entity.H53 = dr.GetDecimal(iH53);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH55 = dr.GetOrdinal(helper.H55);
                    if (!dr.IsDBNull(iH55)) entity.H55 = dr.GetDecimal(iH55);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH57 = dr.GetOrdinal(helper.H57);
                    if (!dr.IsDBNull(iH57)) entity.H57 = dr.GetDecimal(iH57);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH59 = dr.GetOrdinal(helper.H59);
                    if (!dr.IsDBNull(iH59)) entity.H59 = dr.GetDecimal(iH59);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH61 = dr.GetOrdinal(helper.H61);
                    if (!dr.IsDBNull(iH61)) entity.H61 = dr.GetDecimal(iH61);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH63 = dr.GetOrdinal(helper.H63);
                    if (!dr.IsDBNull(iH63)) entity.H63 = dr.GetDecimal(iH63);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH65 = dr.GetOrdinal(helper.H65);
                    if (!dr.IsDBNull(iH65)) entity.H65 = dr.GetDecimal(iH65);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH67 = dr.GetOrdinal(helper.H67);
                    if (!dr.IsDBNull(iH67)) entity.H67 = dr.GetDecimal(iH67);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH69 = dr.GetOrdinal(helper.H69);
                    if (!dr.IsDBNull(iH69)) entity.H69 = dr.GetDecimal(iH69);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH71 = dr.GetOrdinal(helper.H71);
                    if (!dr.IsDBNull(iH71)) entity.H71 = dr.GetDecimal(iH71);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH73 = dr.GetOrdinal(helper.H73);
                    if (!dr.IsDBNull(iH73)) entity.H73 = dr.GetDecimal(iH73);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH75 = dr.GetOrdinal(helper.H75);
                    if (!dr.IsDBNull(iH75)) entity.H75 = dr.GetDecimal(iH75);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH77 = dr.GetOrdinal(helper.H77);
                    if (!dr.IsDBNull(iH77)) entity.H77 = dr.GetDecimal(iH77);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH79 = dr.GetOrdinal(helper.H79);
                    if (!dr.IsDBNull(iH79)) entity.H79 = dr.GetDecimal(iH79);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH81 = dr.GetOrdinal(helper.H81);
                    if (!dr.IsDBNull(iH81)) entity.H81 = dr.GetDecimal(iH81);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH83 = dr.GetOrdinal(helper.H83);
                    if (!dr.IsDBNull(iH83)) entity.H83 = dr.GetDecimal(iH83);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH85 = dr.GetOrdinal(helper.H85);
                    if (!dr.IsDBNull(iH85)) entity.H85 = dr.GetDecimal(iH85);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH87 = dr.GetOrdinal(helper.H87);
                    if (!dr.IsDBNull(iH87)) entity.H87 = dr.GetDecimal(iH87);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH89 = dr.GetOrdinal(helper.H89);
                    if (!dr.IsDBNull(iH89)) entity.H89 = dr.GetDecimal(iH89);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH91 = dr.GetOrdinal(helper.H91);
                    if (!dr.IsDBNull(iH91)) entity.H91 = dr.GetDecimal(iH91);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH93 = dr.GetOrdinal(helper.H93);
                    if (!dr.IsDBNull(iH93)) entity.H93 = dr.GetDecimal(iH93);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH95 = dr.GetOrdinal(helper.H95);
                    if (!dr.IsDBNull(iH95)) entity.H95 = dr.GetDecimal(iH95);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnMedicionesRawDTO> ListMedicionesRawPorAsociacion(int asociacion, DateTime fecha, int idVariable, string idFuente, string modulo)
        {
            List<PrnMedicionesRawDTO> entitys = new List<PrnMedicionesRawDTO>();
            string query = string.Format(helper.SqlListMedicionesRawPorAsociacion, asociacion, fecha.ToString("dd/MM/yyyy"), idVariable, idFuente, modulo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnMedicionesRawDTO entity = new PrnMedicionesRawDTO();

                    int iAsociacodi = dr.GetOrdinal(helper.Asociacodi);
                    if (!dr.IsDBNull(iAsociacodi)) entity.Asociacodi = Convert.ToInt32(dr.GetValue(iAsociacodi));

                    int iMedirawfecha = dr.GetOrdinal(helper.Medirawfecha);
                    if (!dr.IsDBNull(iMedirawfecha)) entity.Medirawfecha = dr.GetDateTime(iMedirawfecha);

                    int iH1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH49 = dr.GetOrdinal(helper.H49);
                    if (!dr.IsDBNull(iH49)) entity.H49 = dr.GetDecimal(iH49);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH51 = dr.GetOrdinal(helper.H51);
                    if (!dr.IsDBNull(iH51)) entity.H51 = dr.GetDecimal(iH51);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH53 = dr.GetOrdinal(helper.H53);
                    if (!dr.IsDBNull(iH53)) entity.H53 = dr.GetDecimal(iH53);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH55 = dr.GetOrdinal(helper.H55);
                    if (!dr.IsDBNull(iH55)) entity.H55 = dr.GetDecimal(iH55);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH57 = dr.GetOrdinal(helper.H57);
                    if (!dr.IsDBNull(iH57)) entity.H57 = dr.GetDecimal(iH57);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH59 = dr.GetOrdinal(helper.H59);
                    if (!dr.IsDBNull(iH59)) entity.H59 = dr.GetDecimal(iH59);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH61 = dr.GetOrdinal(helper.H61);
                    if (!dr.IsDBNull(iH61)) entity.H61 = dr.GetDecimal(iH61);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH63 = dr.GetOrdinal(helper.H63);
                    if (!dr.IsDBNull(iH63)) entity.H63 = dr.GetDecimal(iH63);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH65 = dr.GetOrdinal(helper.H65);
                    if (!dr.IsDBNull(iH65)) entity.H65 = dr.GetDecimal(iH65);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH67 = dr.GetOrdinal(helper.H67);
                    if (!dr.IsDBNull(iH67)) entity.H67 = dr.GetDecimal(iH67);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH69 = dr.GetOrdinal(helper.H69);
                    if (!dr.IsDBNull(iH69)) entity.H69 = dr.GetDecimal(iH69);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH71 = dr.GetOrdinal(helper.H71);
                    if (!dr.IsDBNull(iH71)) entity.H71 = dr.GetDecimal(iH71);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH73 = dr.GetOrdinal(helper.H73);
                    if (!dr.IsDBNull(iH73)) entity.H73 = dr.GetDecimal(iH73);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH75 = dr.GetOrdinal(helper.H75);
                    if (!dr.IsDBNull(iH75)) entity.H75 = dr.GetDecimal(iH75);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH77 = dr.GetOrdinal(helper.H77);
                    if (!dr.IsDBNull(iH77)) entity.H77 = dr.GetDecimal(iH77);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH79 = dr.GetOrdinal(helper.H79);
                    if (!dr.IsDBNull(iH79)) entity.H79 = dr.GetDecimal(iH79);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH81 = dr.GetOrdinal(helper.H81);
                    if (!dr.IsDBNull(iH81)) entity.H81 = dr.GetDecimal(iH81);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH83 = dr.GetOrdinal(helper.H83);
                    if (!dr.IsDBNull(iH83)) entity.H83 = dr.GetDecimal(iH83);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH85 = dr.GetOrdinal(helper.H85);
                    if (!dr.IsDBNull(iH85)) entity.H85 = dr.GetDecimal(iH85);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH87 = dr.GetOrdinal(helper.H87);
                    if (!dr.IsDBNull(iH87)) entity.H87 = dr.GetDecimal(iH87);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH89 = dr.GetOrdinal(helper.H89);
                    if (!dr.IsDBNull(iH89)) entity.H89 = dr.GetDecimal(iH89);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH91 = dr.GetOrdinal(helper.H91);
                    if (!dr.IsDBNull(iH91)) entity.H91 = dr.GetDecimal(iH91);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH93 = dr.GetOrdinal(helper.H93);
                    if (!dr.IsDBNull(iH93)) entity.H93 = dr.GetDecimal(iH93);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH95 = dr.GetOrdinal(helper.H95);
                    if (!dr.IsDBNull(iH95)) entity.H95 = dr.GetDecimal(iH95);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnMedicionesRawDTO> ListMedicionesRawPorRangoPorAsociacion(int asociacion, DateTime fechaInicio, DateTime fechaFin, int idVariable, string idFuente, string modulo)
        {
            List<PrnMedicionesRawDTO> entitys = new List<PrnMedicionesRawDTO>();
            string query = string.Format(helper.SqlListMedicionesRawPorRangoPorAsociacion, asociacion, fechaInicio.ToString("dd/MM/yyyy"), fechaFin.ToString("dd/MM/yyyy"), idVariable, idFuente, modulo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnMedicionesRawDTO entity = new PrnMedicionesRawDTO();

                    int iAsociacodi = dr.GetOrdinal(helper.Asociacodi);
                    if (!dr.IsDBNull(iAsociacodi)) entity.Asociacodi = Convert.ToInt32(dr.GetValue(iAsociacodi));

                    int iMedirawfecha = dr.GetOrdinal(helper.Medirawfecha);
                    if (!dr.IsDBNull(iMedirawfecha)) entity.Medirawfecha = dr.GetDateTime(iMedirawfecha);

                    int iH1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH49 = dr.GetOrdinal(helper.H49);
                    if (!dr.IsDBNull(iH49)) entity.H49 = dr.GetDecimal(iH49);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH51 = dr.GetOrdinal(helper.H51);
                    if (!dr.IsDBNull(iH51)) entity.H51 = dr.GetDecimal(iH51);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH53 = dr.GetOrdinal(helper.H53);
                    if (!dr.IsDBNull(iH53)) entity.H53 = dr.GetDecimal(iH53);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH55 = dr.GetOrdinal(helper.H55);
                    if (!dr.IsDBNull(iH55)) entity.H55 = dr.GetDecimal(iH55);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH57 = dr.GetOrdinal(helper.H57);
                    if (!dr.IsDBNull(iH57)) entity.H57 = dr.GetDecimal(iH57);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH59 = dr.GetOrdinal(helper.H59);
                    if (!dr.IsDBNull(iH59)) entity.H59 = dr.GetDecimal(iH59);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH61 = dr.GetOrdinal(helper.H61);
                    if (!dr.IsDBNull(iH61)) entity.H61 = dr.GetDecimal(iH61);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH63 = dr.GetOrdinal(helper.H63);
                    if (!dr.IsDBNull(iH63)) entity.H63 = dr.GetDecimal(iH63);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH65 = dr.GetOrdinal(helper.H65);
                    if (!dr.IsDBNull(iH65)) entity.H65 = dr.GetDecimal(iH65);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH67 = dr.GetOrdinal(helper.H67);
                    if (!dr.IsDBNull(iH67)) entity.H67 = dr.GetDecimal(iH67);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH69 = dr.GetOrdinal(helper.H69);
                    if (!dr.IsDBNull(iH69)) entity.H69 = dr.GetDecimal(iH69);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH71 = dr.GetOrdinal(helper.H71);
                    if (!dr.IsDBNull(iH71)) entity.H71 = dr.GetDecimal(iH71);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH73 = dr.GetOrdinal(helper.H73);
                    if (!dr.IsDBNull(iH73)) entity.H73 = dr.GetDecimal(iH73);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH75 = dr.GetOrdinal(helper.H75);
                    if (!dr.IsDBNull(iH75)) entity.H75 = dr.GetDecimal(iH75);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH77 = dr.GetOrdinal(helper.H77);
                    if (!dr.IsDBNull(iH77)) entity.H77 = dr.GetDecimal(iH77);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH79 = dr.GetOrdinal(helper.H79);
                    if (!dr.IsDBNull(iH79)) entity.H79 = dr.GetDecimal(iH79);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH81 = dr.GetOrdinal(helper.H81);
                    if (!dr.IsDBNull(iH81)) entity.H81 = dr.GetDecimal(iH81);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH83 = dr.GetOrdinal(helper.H83);
                    if (!dr.IsDBNull(iH83)) entity.H83 = dr.GetDecimal(iH83);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH85 = dr.GetOrdinal(helper.H85);
                    if (!dr.IsDBNull(iH85)) entity.H85 = dr.GetDecimal(iH85);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH87 = dr.GetOrdinal(helper.H87);
                    if (!dr.IsDBNull(iH87)) entity.H87 = dr.GetDecimal(iH87);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH89 = dr.GetOrdinal(helper.H89);
                    if (!dr.IsDBNull(iH89)) entity.H89 = dr.GetDecimal(iH89);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH91 = dr.GetOrdinal(helper.H91);
                    if (!dr.IsDBNull(iH91)) entity.H91 = dr.GetDecimal(iH91);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH93 = dr.GetOrdinal(helper.H93);
                    if (!dr.IsDBNull(iH93)) entity.H93 = dr.GetDecimal(iH93);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH95 = dr.GetOrdinal(helper.H95);
                    if (!dr.IsDBNull(iH95)) entity.H95 = dr.GetDecimal(iH95);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public PrnMedicionesRawDTO GetByKey(PrnMedicionesRawDTO entidad)
        {
            PrnMedicionesRawDTO entity = new PrnMedicionesRawDTO();

            string query = string.Format(helper.SqlGetByKey, entidad.Ptomedicodi, entidad.Prnvarcodi,
                entidad.Medirawfuente, entidad.Medirawfecha.ToString("dd/MM/yyyy"), entidad.Medirawtipomedi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }
}
