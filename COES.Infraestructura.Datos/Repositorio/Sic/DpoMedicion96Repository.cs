using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection.Emit;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class DpoMedicion96Repository : RepositoryBase, IDpoMedicion96Repository
    {
        public DpoMedicion96Repository(string strConn) : base(strConn)
        {
        }

        DpoMedicion96Helper helper = new DpoMedicion96Helper();
        PrnMediciongrpHelper helperMediciongrp = new PrnMediciongrpHelper();

        public void Save(DpoMedicion96DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Dpomedcodi, DbType.Int32, entity.Dpomedcodi);
            dbProvider.AddInParameter(command, helper.Dpotmecodi, DbType.Int32, entity.Dpotmecodi);
            dbProvider.AddInParameter(command, helper.Dpotdtcodi, DbType.Int32, entity.Dpotdtcodi);
            dbProvider.AddInParameter(command, helper.Dpomedfecha, DbType.DateTime, entity.Dpomedfecha);
            dbProvider.AddInParameter(command, helper.Vergrpcodi, DbType.Int32, entity.Vergrpcodi);
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
            dbProvider.AddInParameter(command, helper.Dpomedusucreacion, DbType.String, entity.Dpomedusucreacion);
            dbProvider.AddInParameter(command, helper.Dpomedfeccreacion, DbType.DateTime, entity.Dpomedfeccreacion);
            dbProvider.AddInParameter(command, helper.Dpomedusumodificacion, DbType.String, entity.Dpomedusumodificacion);
            dbProvider.AddInParameter(command, helper.Dpomedfecmodificacion, DbType.DateTime, entity.Dpomedfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(DpoMedicion96DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

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
            dbProvider.AddInParameter(command, helper.Dpomedusumodificacion, DbType.String, entity.Dpomedusumodificacion);
            dbProvider.AddInParameter(command, helper.Dpomedfecmodificacion, DbType.DateTime, entity.Dpomedfecmodificacion);

            dbProvider.AddInParameter(command, helper.Dpomedcodi, DbType.Int32, entity.Dpomedcodi);
            dbProvider.AddInParameter(command, helper.Dpotmecodi, DbType.Int32, entity.Dpotmecodi);
            dbProvider.AddInParameter(command, helper.Dpotdtcodi, DbType.Int32, entity.Dpotdtcodi);
            dbProvider.AddInParameter(command, helper.Dpomedfecha, DbType.DateTime, entity.Dpomedfecha);
            dbProvider.AddInParameter(command, helper.Vergrpcodi, DbType.Int32, entity.Vergrpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int dpomedcodi, int dpotmecodi,
            int dpotdtcodi, int vergrpcodi,
            DateTime medifecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Dpomedcodi, DbType.Int32, dpomedcodi);
            dbProvider.AddInParameter(command, helper.Dpotmecodi, DbType.Int32, dpotmecodi);
            dbProvider.AddInParameter(command, helper.Dpotdtcodi, DbType.Int32, dpotdtcodi);
            dbProvider.AddInParameter(command, helper.Vergrpcodi, DbType.Int32, vergrpcodi);
            dbProvider.AddInParameter(command, helper.Dpomedfecha, DbType.DateTime, medifecha);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByVersion(int vergrpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByVersion);

            dbProvider.AddInParameter(command, helper.Vergrpcodi, DbType.Int32, vergrpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<DpoMedicion96DTO> List()
        {
            List<DpoMedicion96DTO> entitys = new List<DpoMedicion96DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<DpoMedicion96DTO> ListById(int dpomedcodi,
            int dpotmecodi, int dpotdtcodi, 
            int vergrpcodi,  string fecini,
            string fecfin)
        {
            List<DpoMedicion96DTO> entitys = new List<DpoMedicion96DTO>();
            string query = string.Format(helper.SqlListById,
                dpomedcodi, dpotmecodi, dpotdtcodi,
                vergrpcodi, fecini, fecini);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<DpoMedicion96DTO> ListByVersion(int dpotmecodi,
            int dpotdtcodi, int vergrpcodi, 
            string fecini, string fecfin)
        {
            List<DpoMedicion96DTO> entitys = new List<DpoMedicion96DTO>();
            string query = string.Format(helper.SqlListByVersion,
                dpotmecodi, dpotdtcodi, vergrpcodi, 
                fecini, fecini);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<DpoMedicion96DTO> ListByFiltros(string dpomedcodi, int dpotmecodi,
                                                    int dpotdtcodi, int vergrpcodi,
                                                    int anio, string mes)
        {
            List<DpoMedicion96DTO> entitys = new List<DpoMedicion96DTO>();
            string query = string.Format(helper.SqlListByFiltros, dpomedcodi,
                                        dpotmecodi, dpotdtcodi, vergrpcodi,
                                        anio, mes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<DpoMedicion96DTO> ObtenerVersionComparacion(
            string medifecha, int vergrpcodi)
        {
            List<DpoMedicion96DTO> entitys = new List<DpoMedicion96DTO>();
            string query = string.Format(helper.SqlObtenerVersionComparacion,
                medifecha, vergrpcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoMedicion96DTO entity = new DpoMedicion96DTO();

                    int iDpomedcodi = dr.GetOrdinal(helper.Dpomedcodi);
                    if (!dr.IsDBNull(iDpomedcodi)) entity.Dpomedcodi = Convert.ToInt32(dr.GetValue(iDpomedcodi));

                    int iDpomedfecha = dr.GetOrdinal(helper.Dpomedfecha);
                    if (!dr.IsDBNull(iDpomedfecha)) entity.Dpomedfecha = dr.GetDateTime(iDpomedfecha);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<DpoMedicion96DTO> ObtenerDatosMediciongrp(
            string grupocodi, string prnmgrtipo,
            string medifecha, string vergrpcodi)
        {
            List<DpoMedicion96DTO> entitys = new List<DpoMedicion96DTO>();
            string query = string.Format(helper.SqlObtenerDatosMediciongrp,
                grupocodi, prnmgrtipo, medifecha, vergrpcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoMedicion96DTO entity = new DpoMedicion96DTO();

                    int iDpomedcodi = dr.GetOrdinal(helper.Dpomedcodi);
                    if (!dr.IsDBNull(iDpomedcodi)) entity.Dpomedcodi = Convert.ToInt32(dr.GetValue(iDpomedcodi));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iDpomedfecha = dr.GetOrdinal(helper.Dpomedfecha);
                    if (!dr.IsDBNull(iDpomedfecha)) entity.Dpomedfecha = dr.GetDateTime(iDpomedfecha);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<DpoMedicion96DTO> ObtenerDatosPorVersion(
            int dpotdtcodi, string dpotmecodi, 
            int vergrpcodi, string dpomedfecha)
        {
            List<DpoMedicion96DTO> entitys = new List<DpoMedicion96DTO>();
            string query = string.Format(helper.SqlObtenerDatosPorVersion,
                dpotdtcodi, dpotmecodi, vergrpcodi,
                dpomedfecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoMedicion96DTO entity = new DpoMedicion96DTO();

                    int iDpomedcodi = dr.GetOrdinal(helper.Dpomedcodi);
                    if (!dr.IsDBNull(iDpomedcodi)) entity.Dpomedcodi = Convert.ToInt32(dr.GetValue(iDpomedcodi));

                    int iDpomedfecha = dr.GetOrdinal(helper.Dpomedfecha);
                    if (!dr.IsDBNull(iDpomedfecha)) entity.Dpomedfecha = dr.GetDateTime(iDpomedfecha);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<DpoMedicion96DTO> ObtenerDatosPorVersionNulleable(
            int dpotdtcodi, string dpotmecodi,
            int vergrpcodi, string dpomedfecha)
        {
            List<DpoMedicion96DTO> entitys = new List<DpoMedicion96DTO>();
            string query = string.Format(helper.SqlObtenerDatosPorVersionNulleable,
                dpotdtcodi, dpotmecodi, vergrpcodi,
                dpomedfecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoMedicion96DTO entity = new DpoMedicion96DTO();

                    int iDpomedcodi = dr.GetOrdinal(helper.Dpomedcodi);
                    if (!dr.IsDBNull(iDpomedcodi)) entity.Dpomedcodi = Convert.ToInt32(dr.GetValue(iDpomedcodi));

                    int iDpomedfecha = dr.GetOrdinal(helper.Dpomedfecha);
                    if (!dr.IsDBNull(iDpomedfecha)) entity.Dpomedfecha = dr.GetDateTime(iDpomedfecha);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public DpoMedicion96DTO ObtenerDatosPorId(int dpomedcodi,
            int dpotdtcodi, string dpotmecodi, int vergrpcodi, 
            string dpomedfecha)
        {
            DpoMedicion96DTO entity = new DpoMedicion96DTO();
            string query = string.Format(helper.SqlObtenerDatosPorId,
                dpomedcodi, dpotdtcodi, dpotmecodi, vergrpcodi,
                dpomedfecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {                   
                    int iDpomedcodi = dr.GetOrdinal(helper.Dpomedcodi);
                    if (!dr.IsDBNull(iDpomedcodi)) entity.Dpomedcodi = Convert.ToInt32(dr.GetValue(iDpomedcodi));

                    int iDpomedfecha = dr.GetOrdinal(helper.Dpomedfecha);
                    if (!dr.IsDBNull(iDpomedfecha)) entity.Dpomedfecha = dr.GetDateTime(iDpomedfecha);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);
                }
            }
            return entity;
        }

        public DpoMedicion96DTO ObtenerDatosAgrupados(string dpomedcodi,
            int dpotdtcodi, string dpotmecodi, int vergrpcodi,
            string dpomedfecha)
        {
            DpoMedicion96DTO entity = new DpoMedicion96DTO();
            string query = string.Format(helper.SqlObtenerDatosAgrupados, 
                dpomedcodi, dpotdtcodi, dpotmecodi, vergrpcodi,
                dpomedfecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {                    
                    int iDpomedfecha = dr.GetOrdinal(helper.Dpomedfecha);
                    if (!dr.IsDBNull(iDpomedfecha)) entity.Dpomedfecha = dr.GetDateTime(iDpomedfecha);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);
                }
            }
            return entity;
        }

        public DpoMedicion96DTO ObtenerDatosCompAgrupados(string grupocodi,
            int vergrpcodi, string dpomedfecha)
        {
            DpoMedicion96DTO entity = new DpoMedicion96DTO();
            string query = string.Format(helper.SqlObtenerDatosCompAgrupados,
                grupocodi, vergrpcodi, dpomedfecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iDpomedfecha = dr.GetOrdinal(helper.Dpomedfecha);
                    if (!dr.IsDBNull(iDpomedfecha)) entity.Dpomedfecha = dr.GetDateTime(iDpomedfecha);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);
                }
            }
            return entity;
        }

        public List<DpoMedicion96DTO> ObtenerDemRDOPrevExtranet(int formatcodi,
            int lectcodi, string dpomedfecha)
        {
            List<DpoMedicion96DTO> entities = new List<DpoMedicion96DTO>();
            string query = string.Format(helper.SqlObtenerDemRDOPrevExtranet,
                formatcodi, lectcodi, dpomedfecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoMedicion96DTO entity = new DpoMedicion96DTO();

                    int iDpomedcodi = dr.GetOrdinal(helper.Dpomedcodi);
                    if (!dr.IsDBNull(iDpomedcodi)) entity.Dpomedcodi = Convert.ToInt32(dr.GetValue(iDpomedcodi));

                    int iDpomedfecha = dr.GetOrdinal(helper.Dpomedfecha);
                    if (!dr.IsDBNull(iDpomedfecha)) entity.Dpomedfecha = dr.GetDateTime(iDpomedfecha);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);

                    entities.Add(entity);
                }
            }
            return entities;
        }

        public DpoMedicion96DTO ObtenerDatosEscenarioYupana(
            int topcodi, int srestcodi, string medifecha)
        {
            DpoMedicion96DTO entity = new DpoMedicion96DTO();
            string query = string.Format(helper.SqlObtenerDatosEscenarioYupana,
                topcodi, srestcodi, medifecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {                    
                    int iDpomedfecha = dr.GetOrdinal(helper.Dpomedfecha);
                    if (!dr.IsDBNull(iDpomedfecha)) entity.Dpomedfecha = dr.GetDateTime(iDpomedfecha);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);
                }
            }
            return entity;
        }

        public DpoMedicion96DTO ObtenerDatosSumEscenarioYupana(
            int topcodi, int srestcodi, string medifecha)
        {
            DpoMedicion96DTO entity = new DpoMedicion96DTO();
            string query = string.Format(helper.SqlObtenerDatosSumEscenarioYupana,
                topcodi, srestcodi, medifecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iDpomedfecha = dr.GetOrdinal(helper.Dpomedfecha);
                    if (!dr.IsDBNull(iDpomedfecha)) entity.Dpomedfecha = dr.GetDateTime(iDpomedfecha);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);
                }
            }
            return entity;
        }

        public DpoMedicion96DTO ObtenerDatosFormulaM48(int lectcodi, 
            int tipoinfocodi, string ptomedicodi, string fecIni,
            string fecFin)
        {
            DpoMedicion96DTO entity = new DpoMedicion96DTO();
            string query = string.Format(helper.SqlObtenerDatosFormulaM48,
                lectcodi, tipoinfocodi, ptomedicodi, fecIni, fecFin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iDpomedfecha = dr.GetOrdinal(helper.Dpomedfecha);
                    if (!dr.IsDBNull(iDpomedfecha)) entity.Dpomedfecha = dr.GetDateTime(iDpomedfecha);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);
                }
            }
            return entity;
        }

        public DpoMedicion96DTO ObtenerDatosMedicion48(int lectcodi,
            int tipoinfocodi, string ptomedicodi, string fecIni,
            string fecFin)
        {
            DpoMedicion96DTO entity = new DpoMedicion96DTO();
            string query = string.Format(helper.SqlObtenerDatosMedicion48,
                lectcodi, tipoinfocodi, ptomedicodi, fecIni, fecFin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iDpomedfecha = dr.GetOrdinal(helper.Dpomedfecha);
                    if (!dr.IsDBNull(iDpomedfecha)) entity.Dpomedfecha = dr.GetDateTime(iDpomedfecha);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);
                }
            }
            return entity;
        }
    }
}
