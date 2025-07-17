using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Scada;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Scada;
using System.Data.Odbc;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Scada
{
    /// <summary>
    /// Clase de acceso a datos de la tabla ME_SCADA_SP7
    /// </summary>
    public class MeScadaSp7Repository : RepositoryBase, IMeScadaSp7Repository
    {
        public MeScadaSp7Repository(string strConn) : base(strConn)
        {
        }

        MeScadaSp7Helper helper = new MeScadaSp7Helper();

        public void Save(MeScadaSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, entity.Medifecha);
            dbProvider.AddInParameter(command, helper.Meditotal, DbType.Decimal, entity.Meditotal);
            dbProvider.AddInParameter(command, helper.Mediestado, DbType.String, entity.Mediestado);
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
            dbProvider.AddInParameter(command, helper.Medicambio, DbType.String, entity.Medicambio);
            dbProvider.AddInParameter(command, helper.Medscdusucreacion, DbType.String, entity.Medscdusucreacion);
            dbProvider.AddInParameter(command, helper.Medscdfeccreacion, DbType.DateTime, entity.Medscdfeccreacion);
            dbProvider.AddInParameter(command, helper.Medscdusumodificacion, DbType.String, entity.Medscdusumodificacion);
            dbProvider.AddInParameter(command, helper.Medscdfecmodificacion, DbType.DateTime, entity.Medscdfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(MeScadaSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Meditotal, DbType.Decimal, entity.Meditotal);
            dbProvider.AddInParameter(command, helper.Mediestado, DbType.String, entity.Mediestado);
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
            dbProvider.AddInParameter(command, helper.Medicambio, DbType.String, entity.Medicambio);
            dbProvider.AddInParameter(command, helper.Medscdusucreacion, DbType.String, entity.Medscdusucreacion);
            dbProvider.AddInParameter(command, helper.Medscdfeccreacion, DbType.DateTime, entity.Medscdfeccreacion);
            dbProvider.AddInParameter(command, helper.Medscdusumodificacion, DbType.String, entity.Medscdusumodificacion);
            dbProvider.AddInParameter(command, helper.Medscdfecmodificacion, DbType.DateTime, entity.Medscdfecmodificacion);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, entity.Medifecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int canalcodi, DateTime medifecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, canalcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, medifecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeScadaSp7DTO GetById(int canalcodi, DateTime medifecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, canalcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, medifecha);
            MeScadaSp7DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeScadaSp7DTO> List()
        {
            List<MeScadaSp7DTO> entitys = new List<MeScadaSp7DTO>();
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

        public List<MeScadaSp7DTO> GetByCriteria(DateTime fechaIni, DateTime fechaFin, string canalcodi)
        {
            List<MeScadaSp7DTO> entitys = new List<MeScadaSp7DTO>();
            string query = string.Format(helper.SqlGetByCriteria, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), canalcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iCanalcalidad = dr.GetOrdinal(helper.Canalcalidad);
                    if (!dr.IsDBNull(iCanalcalidad)) entity.Canalcalidad = dr.GetInt32(iCanalcalidad);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeScadaSp7DTO> BuscarOperaciones(bool ssee, int zonaCodi, DateTime mediFechaIni, DateTime mediFechaFin, int nroPage, int pageSize)
        {
            List<MeScadaSp7DTO> entitys = new List<MeScadaSp7DTO>();
            String sql = "";

            if (ssee)
            {
                sql = String.Format(this.helper.ObtenerListadoSSEE,
                    zonaCodi,
                    mediFechaIni.ToString(ConstantesBase.FormatoFecha),
                    mediFechaFin.ToString(ConstantesBase.FormatoFecha),
                    nroPage, pageSize);
            }
            else
            {
                sql = String.Format(this.helper.ObtenerListadoFiltro,
                    zonaCodi,
                    mediFechaIni.ToString(ConstantesBase.FormatoFecha),
                    mediFechaFin.ToString(ConstantesBase.FormatoFecha),
                    nroPage, pageSize);
            }

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeScadaSp7DTO entity = new MeScadaSp7DTO();

                    int iCanalcodi = dr.GetOrdinal(this.helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iMedifecha = dr.GetOrdinal(this.helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);
                    /*
                    int iMeditotal = dr.GetOrdinal(this.helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    int iMediestado = dr.GetOrdinal(this.helper.Mediestado);
                    if (!dr.IsDBNull(iMediestado)) entity.Mediestado = dr.GetString(iMediestado);
                    */
                    int iH1 = dr.GetOrdinal(this.helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(this.helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(this.helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(this.helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(this.helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(this.helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(this.helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(this.helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(this.helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(this.helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(this.helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(this.helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(this.helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(this.helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(this.helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(this.helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(this.helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(this.helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(this.helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(this.helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(this.helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(this.helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(this.helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(this.helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(this.helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(this.helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(this.helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(this.helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(this.helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(this.helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(this.helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(this.helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(this.helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(this.helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(this.helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(this.helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(this.helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(this.helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(this.helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(this.helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(this.helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(this.helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(this.helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(this.helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(this.helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(this.helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(this.helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(this.helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH49 = dr.GetOrdinal(this.helper.H49);
                    if (!dr.IsDBNull(iH49)) entity.H49 = dr.GetDecimal(iH49);

                    int iH50 = dr.GetOrdinal(this.helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH51 = dr.GetOrdinal(this.helper.H51);
                    if (!dr.IsDBNull(iH51)) entity.H51 = dr.GetDecimal(iH51);

                    int iH52 = dr.GetOrdinal(this.helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH53 = dr.GetOrdinal(this.helper.H53);
                    if (!dr.IsDBNull(iH53)) entity.H53 = dr.GetDecimal(iH53);

                    int iH54 = dr.GetOrdinal(this.helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH55 = dr.GetOrdinal(this.helper.H55);
                    if (!dr.IsDBNull(iH55)) entity.H55 = dr.GetDecimal(iH55);

                    int iH56 = dr.GetOrdinal(this.helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH57 = dr.GetOrdinal(this.helper.H57);
                    if (!dr.IsDBNull(iH57)) entity.H57 = dr.GetDecimal(iH57);

                    int iH58 = dr.GetOrdinal(this.helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH59 = dr.GetOrdinal(this.helper.H59);
                    if (!dr.IsDBNull(iH59)) entity.H59 = dr.GetDecimal(iH59);

                    int iH60 = dr.GetOrdinal(this.helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH61 = dr.GetOrdinal(this.helper.H61);
                    if (!dr.IsDBNull(iH61)) entity.H61 = dr.GetDecimal(iH61);

                    int iH62 = dr.GetOrdinal(this.helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH63 = dr.GetOrdinal(this.helper.H63);
                    if (!dr.IsDBNull(iH63)) entity.H63 = dr.GetDecimal(iH63);

                    int iH64 = dr.GetOrdinal(this.helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH65 = dr.GetOrdinal(this.helper.H65);
                    if (!dr.IsDBNull(iH65)) entity.H65 = dr.GetDecimal(iH65);

                    int iH66 = dr.GetOrdinal(this.helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH67 = dr.GetOrdinal(this.helper.H67);
                    if (!dr.IsDBNull(iH67)) entity.H67 = dr.GetDecimal(iH67);

                    int iH68 = dr.GetOrdinal(this.helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH69 = dr.GetOrdinal(this.helper.H69);
                    if (!dr.IsDBNull(iH69)) entity.H69 = dr.GetDecimal(iH69);

                    int iH70 = dr.GetOrdinal(this.helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH71 = dr.GetOrdinal(this.helper.H71);
                    if (!dr.IsDBNull(iH71)) entity.H71 = dr.GetDecimal(iH71);

                    int iH72 = dr.GetOrdinal(this.helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH73 = dr.GetOrdinal(this.helper.H73);
                    if (!dr.IsDBNull(iH73)) entity.H73 = dr.GetDecimal(iH73);

                    int iH74 = dr.GetOrdinal(this.helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH75 = dr.GetOrdinal(this.helper.H75);
                    if (!dr.IsDBNull(iH75)) entity.H75 = dr.GetDecimal(iH75);

                    int iH76 = dr.GetOrdinal(this.helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH77 = dr.GetOrdinal(this.helper.H77);
                    if (!dr.IsDBNull(iH77)) entity.H77 = dr.GetDecimal(iH77);

                    int iH78 = dr.GetOrdinal(this.helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH79 = dr.GetOrdinal(this.helper.H79);
                    if (!dr.IsDBNull(iH79)) entity.H79 = dr.GetDecimal(iH79);

                    int iH80 = dr.GetOrdinal(this.helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH81 = dr.GetOrdinal(this.helper.H81);
                    if (!dr.IsDBNull(iH81)) entity.H81 = dr.GetDecimal(iH81);

                    int iH82 = dr.GetOrdinal(this.helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH83 = dr.GetOrdinal(this.helper.H83);
                    if (!dr.IsDBNull(iH83)) entity.H83 = dr.GetDecimal(iH83);

                    int iH84 = dr.GetOrdinal(this.helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH85 = dr.GetOrdinal(this.helper.H85);
                    if (!dr.IsDBNull(iH85)) entity.H85 = dr.GetDecimal(iH85);

                    int iH86 = dr.GetOrdinal(this.helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH87 = dr.GetOrdinal(this.helper.H87);
                    if (!dr.IsDBNull(iH87)) entity.H87 = dr.GetDecimal(iH87);

                    int iH88 = dr.GetOrdinal(this.helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH89 = dr.GetOrdinal(this.helper.H89);
                    if (!dr.IsDBNull(iH89)) entity.H89 = dr.GetDecimal(iH89);

                    int iH90 = dr.GetOrdinal(this.helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH91 = dr.GetOrdinal(this.helper.H91);
                    if (!dr.IsDBNull(iH91)) entity.H91 = dr.GetDecimal(iH91);

                    int iH92 = dr.GetOrdinal(this.helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH93 = dr.GetOrdinal(this.helper.H93);
                    if (!dr.IsDBNull(iH93)) entity.H93 = dr.GetDecimal(iH93);

                    int iH94 = dr.GetOrdinal(this.helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH95 = dr.GetOrdinal(this.helper.H95);
                    if (!dr.IsDBNull(iH95)) entity.H95 = dr.GetDecimal(iH95);

                    int iH96 = dr.GetOrdinal(this.helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);

                    /*
                    int iMedicambio = dr.GetOrdinal(this.helper.Medicambio);
                    if (!dr.IsDBNull(iMedicambio)) entity.Medicambio = dr.GetString(iMedicambio);

                    int iMedscdusucreacion = dr.GetOrdinal(this.helper.Medscdusucreacion);
                    if (!dr.IsDBNull(iMedscdusucreacion)) entity.Medscdusucreacion = dr.GetString(iMedscdusucreacion);

                    int iMedscdfeccreacion = dr.GetOrdinal(this.helper.Medscdfeccreacion);
                    if (!dr.IsDBNull(iMedscdfeccreacion)) entity.Medscdfeccreacion = dr.GetDateTime(iMedscdfeccreacion);

                    int iMedscdusumodificacion = dr.GetOrdinal(this.helper.Medscdusumodificacion);
                    if (!dr.IsDBNull(iMedscdusumodificacion)) entity.Medscdusumodificacion = dr.GetString(iMedscdusumodificacion);

                    int iMedscdfecmodificacion = dr.GetOrdinal(this.helper.Medscdfecmodificacion);
                    if (!dr.IsDBNull(iMedscdfecmodificacion)) entity.Medscdfecmodificacion = dr.GetDateTime(iMedscdfecmodificacion);
                    */
                    int iZonanomb = dr.GetOrdinal(this.helper.Zonanomb);
                    if (!dr.IsDBNull(iZonanomb)) entity.Zonanomb = dr.GetString(iZonanomb);

                    int iCanalnomb = dr.GetOrdinal(this.helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);


                    int iCanalunidad = dr.GetOrdinal(this.helper.Canalunidad);
                    if (!dr.IsDBNull(iCanalunidad)) entity.Canalunidad = dr.GetString(iCanalunidad);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(bool ssee, int zonaCodi, DateTime mediFechaIni, DateTime mediFechaFin)
        {
            List<MeScadaSp7DTO> entitys = new List<MeScadaSp7DTO>();



            String sql = "";

            if (ssee)
            {
                sql = String.Format(this.helper.TotalRegistrosSSEE, zonaCodi,
                mediFechaIni.ToString(ConstantesBase.FormatoFecha),
                mediFechaFin.ToString(ConstantesBase.FormatoFecha));
            }
            else
            {
                sql = String.Format(this.helper.TotalRegistrosFiltro, zonaCodi,
                    mediFechaIni.ToString(ConstantesBase.FormatoFecha),
                    mediFechaFin.ToString(ConstantesBase.FormatoFecha));

            }


            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }


        public List<MeScadaSp7DTO> BuscarOperacionesReporte(bool ssee, int zonaCodi, DateTime mediFechaIni, DateTime mediFechaFin)
        {
            List<MeScadaSp7DTO> entitys = new List<MeScadaSp7DTO>();
            String sql = "";

            if (ssee)
            {
                sql = String.Format(this.helper.ObtenerListadoReporteSSEE,
                    zonaCodi,
                    mediFechaIni.ToString(ConstantesBase.FormatoFecha),
                    mediFechaFin.ToString(ConstantesBase.FormatoFecha));
            }
            else
            {
                sql = String.Format(this.helper.ObtenerListadoReporteFiltro,
                    zonaCodi,
                    mediFechaIni.ToString(ConstantesBase.FormatoFecha),
                    mediFechaFin.ToString(ConstantesBase.FormatoFecha));
            }

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeScadaSp7DTO entity = new MeScadaSp7DTO();

                    int iCanalcodi = dr.GetOrdinal(this.helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iMedifecha = dr.GetOrdinal(this.helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);
                    /*
                    int iMeditotal = dr.GetOrdinal(this.helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    int iMediestado = dr.GetOrdinal(this.helper.Mediestado);
                    if (!dr.IsDBNull(iMediestado)) entity.Mediestado = dr.GetString(iMediestado);
                    */
                    int iH1 = dr.GetOrdinal(this.helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(this.helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(this.helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(this.helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(this.helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(this.helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(this.helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(this.helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(this.helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(this.helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(this.helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(this.helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(this.helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(this.helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(this.helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(this.helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(this.helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(this.helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(this.helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(this.helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(this.helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(this.helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(this.helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(this.helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(this.helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(this.helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(this.helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(this.helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(this.helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(this.helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(this.helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(this.helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(this.helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(this.helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(this.helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(this.helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(this.helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(this.helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(this.helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(this.helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(this.helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(this.helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(this.helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(this.helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(this.helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(this.helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(this.helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(this.helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH49 = dr.GetOrdinal(this.helper.H49);
                    if (!dr.IsDBNull(iH49)) entity.H49 = dr.GetDecimal(iH49);

                    int iH50 = dr.GetOrdinal(this.helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH51 = dr.GetOrdinal(this.helper.H51);
                    if (!dr.IsDBNull(iH51)) entity.H51 = dr.GetDecimal(iH51);

                    int iH52 = dr.GetOrdinal(this.helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH53 = dr.GetOrdinal(this.helper.H53);
                    if (!dr.IsDBNull(iH53)) entity.H53 = dr.GetDecimal(iH53);

                    int iH54 = dr.GetOrdinal(this.helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH55 = dr.GetOrdinal(this.helper.H55);
                    if (!dr.IsDBNull(iH55)) entity.H55 = dr.GetDecimal(iH55);

                    int iH56 = dr.GetOrdinal(this.helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH57 = dr.GetOrdinal(this.helper.H57);
                    if (!dr.IsDBNull(iH57)) entity.H57 = dr.GetDecimal(iH57);

                    int iH58 = dr.GetOrdinal(this.helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH59 = dr.GetOrdinal(this.helper.H59);
                    if (!dr.IsDBNull(iH59)) entity.H59 = dr.GetDecimal(iH59);

                    int iH60 = dr.GetOrdinal(this.helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH61 = dr.GetOrdinal(this.helper.H61);
                    if (!dr.IsDBNull(iH61)) entity.H61 = dr.GetDecimal(iH61);

                    int iH62 = dr.GetOrdinal(this.helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH63 = dr.GetOrdinal(this.helper.H63);
                    if (!dr.IsDBNull(iH63)) entity.H63 = dr.GetDecimal(iH63);

                    int iH64 = dr.GetOrdinal(this.helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH65 = dr.GetOrdinal(this.helper.H65);
                    if (!dr.IsDBNull(iH65)) entity.H65 = dr.GetDecimal(iH65);

                    int iH66 = dr.GetOrdinal(this.helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH67 = dr.GetOrdinal(this.helper.H67);
                    if (!dr.IsDBNull(iH67)) entity.H67 = dr.GetDecimal(iH67);

                    int iH68 = dr.GetOrdinal(this.helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH69 = dr.GetOrdinal(this.helper.H69);
                    if (!dr.IsDBNull(iH69)) entity.H69 = dr.GetDecimal(iH69);

                    int iH70 = dr.GetOrdinal(this.helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH71 = dr.GetOrdinal(this.helper.H71);
                    if (!dr.IsDBNull(iH71)) entity.H71 = dr.GetDecimal(iH71);

                    int iH72 = dr.GetOrdinal(this.helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH73 = dr.GetOrdinal(this.helper.H73);
                    if (!dr.IsDBNull(iH73)) entity.H73 = dr.GetDecimal(iH73);

                    int iH74 = dr.GetOrdinal(this.helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH75 = dr.GetOrdinal(this.helper.H75);
                    if (!dr.IsDBNull(iH75)) entity.H75 = dr.GetDecimal(iH75);

                    int iH76 = dr.GetOrdinal(this.helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH77 = dr.GetOrdinal(this.helper.H77);
                    if (!dr.IsDBNull(iH77)) entity.H77 = dr.GetDecimal(iH77);

                    int iH78 = dr.GetOrdinal(this.helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH79 = dr.GetOrdinal(this.helper.H79);
                    if (!dr.IsDBNull(iH79)) entity.H79 = dr.GetDecimal(iH79);

                    int iH80 = dr.GetOrdinal(this.helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH81 = dr.GetOrdinal(this.helper.H81);
                    if (!dr.IsDBNull(iH81)) entity.H81 = dr.GetDecimal(iH81);

                    int iH82 = dr.GetOrdinal(this.helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH83 = dr.GetOrdinal(this.helper.H83);
                    if (!dr.IsDBNull(iH83)) entity.H83 = dr.GetDecimal(iH83);

                    int iH84 = dr.GetOrdinal(this.helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH85 = dr.GetOrdinal(this.helper.H85);
                    if (!dr.IsDBNull(iH85)) entity.H85 = dr.GetDecimal(iH85);

                    int iH86 = dr.GetOrdinal(this.helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH87 = dr.GetOrdinal(this.helper.H87);
                    if (!dr.IsDBNull(iH87)) entity.H87 = dr.GetDecimal(iH87);

                    int iH88 = dr.GetOrdinal(this.helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH89 = dr.GetOrdinal(this.helper.H89);
                    if (!dr.IsDBNull(iH89)) entity.H89 = dr.GetDecimal(iH89);

                    int iH90 = dr.GetOrdinal(this.helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH91 = dr.GetOrdinal(this.helper.H91);
                    if (!dr.IsDBNull(iH91)) entity.H91 = dr.GetDecimal(iH91);

                    int iH92 = dr.GetOrdinal(this.helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH93 = dr.GetOrdinal(this.helper.H93);
                    if (!dr.IsDBNull(iH93)) entity.H93 = dr.GetDecimal(iH93);

                    int iH94 = dr.GetOrdinal(this.helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH95 = dr.GetOrdinal(this.helper.H95);
                    if (!dr.IsDBNull(iH95)) entity.H95 = dr.GetDecimal(iH95);

                    int iH96 = dr.GetOrdinal(this.helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);

                    /*
                    int iMedicambio = dr.GetOrdinal(this.helper.Medicambio);
                    if (!dr.IsDBNull(iMedicambio)) entity.Medicambio = dr.GetString(iMedicambio);

                    int iMedscdusucreacion = dr.GetOrdinal(this.helper.Medscdusucreacion);
                    if (!dr.IsDBNull(iMedscdusucreacion)) entity.Medscdusucreacion = dr.GetString(iMedscdusucreacion);

                    int iMedscdfeccreacion = dr.GetOrdinal(this.helper.Medscdfeccreacion);
                    if (!dr.IsDBNull(iMedscdfeccreacion)) entity.Medscdfeccreacion = dr.GetDateTime(iMedscdfeccreacion);

                    int iMedscdusumodificacion = dr.GetOrdinal(this.helper.Medscdusumodificacion);
                    if (!dr.IsDBNull(iMedscdusumodificacion)) entity.Medscdusumodificacion = dr.GetString(iMedscdusumodificacion);

                    int iMedscdfecmodificacion = dr.GetOrdinal(this.helper.Medscdfecmodificacion);
                    if (!dr.IsDBNull(iMedscdfecmodificacion)) entity.Medscdfecmodificacion = dr.GetDateTime(iMedscdfecmodificacion);
                    */
                    int iZonanomb = dr.GetOrdinal(this.helper.Zonanomb);
                    if (!dr.IsDBNull(iZonanomb)) entity.Zonanomb = dr.GetString(iZonanomb);

                    int iCanalnomb = dr.GetOrdinal(this.helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);


                    int iCanalunidad = dr.GetOrdinal(this.helper.Canalunidad);
                    if (!dr.IsDBNull(iCanalunidad)) entity.Canalunidad = dr.GetString(iCanalunidad);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<MeScadaSp7DTO> GetDatosScadaAFormato(int formatcodi, int emprcodi, DateTime fechainicio, DateTime fechafin, string famcodis, string tipoinfocodis)
        {
            string query = string.Format(helper.SqlGetDatosScadaAFormato, formatcodi, emprcodi, fechainicio.ToString(ConstantesBase.FormatoFecha),
                fechafin.ToString(ConstantesBase.FormatoFecha), famcodis, tipoinfocodis);
            List<MeScadaSp7DTO> entitys = new List<MeScadaSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeScadaSp7DTO entity = helper.Create(dr);
                    entity.Medifecha = entity.Medifecha.Date; //existen registros que no empiezan a las 12:00:00am sino 12:00:00pm

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));
                    int iTipoinfocodi = dr.GetOrdinal(helper.Tipoinfocodi);
                    if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);
                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iEquiCodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquiCodi)) entity.Equicodi = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    int iEquiNomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.Equinomb = dr.GetString(iEquiNomb);

                    int iEquiAbrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.Equiabrev = dr.GetString(iEquiAbrev);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprNomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        
        /// <summary>
        /// Obtener data scada por equipos y tipo de información
        /// </summary>
        /// <param name="fechainicio"></param>
        /// <param name="fechafin"></param>
        /// <param name="emprcodi"></param>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        public List<MeScadaSp7DTO> GetDatosScadaByEquipo(DateTime fechainicio, DateTime fechafin, string emprcodi, string equicodi)
        {
            string query = string.Format(helper.SqlGetDatosScadaByEquipo, fechainicio.ToString(ConstantesBase.FormatoFecha), fechafin.ToString(ConstantesBase.FormatoFecha), emprcodi, equicodi);
            List<MeScadaSp7DTO> entitys = new List<MeScadaSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeScadaSp7DTO entity = helper.Create(dr);

                    int iTipoinfocodi = dr.GetOrdinal(helper.Tipoinfocodi);
                    if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iEquiCodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquiCodi)) entity.Equicodi = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    int iEquiNomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.Equinomb = dr.GetString(iEquiNomb);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeScadaSp7DTO> GetByCriteria(DateTime fechainicio, DateTime fechafin, int formatcodi, int tipoinfocodi, int ptomedicodi)
        {
            string query = string.Format(helper.SqlObtenerScada, formatcodi, tipoinfocodi, ptomedicodi, fechainicio.ToString(ConstantesBase.FormatoFecha),
                fechafin.ToString(ConstantesBase.FormatoFecha));
            List<MeScadaSp7DTO> entitys = new List<MeScadaSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeScadaSp7DTO entity = helper.Create(dr);
                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));
                    int iTipoinfocodi = dr.GetOrdinal(helper.Tipoinfocodi);
                    if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeScadaSp7DTO> GetByCriteriaByPtoAndTipoinfocodi(DateTime fechainicio, DateTime fechafin, int tipoinfocodi, int ptomedicodi)
        {
            string query = string.Format(helper.SqlGetByCriteriaByPtoAndTipoinfocodi, fechainicio.ToString(ConstantesBase.FormatoFecha),
                fechafin.ToString(ConstantesBase.FormatoFecha), tipoinfocodi, ptomedicodi);
            List<MeScadaSp7DTO> entitys = new List<MeScadaSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeScadaSp7DTO entity = helper.Create(dr);
                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));
                    int iTipoinfocodi = dr.GetOrdinal(helper.Tipoinfocodi);
                    if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));
                    entitys.Add(entity);
                }
            }
            return entitys;
        }


        public List<MeScadaSp7DTO> ObtenerDatosSupervisionDemanda(DateTime fecha)
        {
            List<MeScadaSp7DTO> entitys = new List<MeScadaSp7DTO>();
            string sql = string.Format(helper.SqlObtenerDatosSupervisionDemanda, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeScadaSp7DTO entity = new MeScadaSp7DTO();

                    //int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    //if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iH1 = dr.GetOrdinal(this.helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(this.helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(this.helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(this.helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(this.helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(this.helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(this.helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(this.helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(this.helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(this.helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(this.helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(this.helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(this.helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(this.helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(this.helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(this.helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(this.helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(this.helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(this.helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(this.helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(this.helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(this.helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(this.helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(this.helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(this.helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(this.helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(this.helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(this.helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(this.helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(this.helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(this.helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(this.helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(this.helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(this.helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(this.helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(this.helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(this.helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(this.helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(this.helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(this.helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(this.helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(this.helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(this.helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(this.helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(this.helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(this.helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(this.helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(this.helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH49 = dr.GetOrdinal(this.helper.H49);
                    if (!dr.IsDBNull(iH49)) entity.H49 = dr.GetDecimal(iH49);

                    int iH50 = dr.GetOrdinal(this.helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH51 = dr.GetOrdinal(this.helper.H51);
                    if (!dr.IsDBNull(iH51)) entity.H51 = dr.GetDecimal(iH51);

                    int iH52 = dr.GetOrdinal(this.helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH53 = dr.GetOrdinal(this.helper.H53);
                    if (!dr.IsDBNull(iH53)) entity.H53 = dr.GetDecimal(iH53);

                    int iH54 = dr.GetOrdinal(this.helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH55 = dr.GetOrdinal(this.helper.H55);
                    if (!dr.IsDBNull(iH55)) entity.H55 = dr.GetDecimal(iH55);

                    int iH56 = dr.GetOrdinal(this.helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH57 = dr.GetOrdinal(this.helper.H57);
                    if (!dr.IsDBNull(iH57)) entity.H57 = dr.GetDecimal(iH57);

                    int iH58 = dr.GetOrdinal(this.helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH59 = dr.GetOrdinal(this.helper.H59);
                    if (!dr.IsDBNull(iH59)) entity.H59 = dr.GetDecimal(iH59);

                    int iH60 = dr.GetOrdinal(this.helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH61 = dr.GetOrdinal(this.helper.H61);
                    if (!dr.IsDBNull(iH61)) entity.H61 = dr.GetDecimal(iH61);

                    int iH62 = dr.GetOrdinal(this.helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH63 = dr.GetOrdinal(this.helper.H63);
                    if (!dr.IsDBNull(iH63)) entity.H63 = dr.GetDecimal(iH63);

                    int iH64 = dr.GetOrdinal(this.helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH65 = dr.GetOrdinal(this.helper.H65);
                    if (!dr.IsDBNull(iH65)) entity.H65 = dr.GetDecimal(iH65);

                    int iH66 = dr.GetOrdinal(this.helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH67 = dr.GetOrdinal(this.helper.H67);
                    if (!dr.IsDBNull(iH67)) entity.H67 = dr.GetDecimal(iH67);

                    int iH68 = dr.GetOrdinal(this.helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH69 = dr.GetOrdinal(this.helper.H69);
                    if (!dr.IsDBNull(iH69)) entity.H69 = dr.GetDecimal(iH69);

                    int iH70 = dr.GetOrdinal(this.helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH71 = dr.GetOrdinal(this.helper.H71);
                    if (!dr.IsDBNull(iH71)) entity.H71 = dr.GetDecimal(iH71);

                    int iH72 = dr.GetOrdinal(this.helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH73 = dr.GetOrdinal(this.helper.H73);
                    if (!dr.IsDBNull(iH73)) entity.H73 = dr.GetDecimal(iH73);

                    int iH74 = dr.GetOrdinal(this.helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH75 = dr.GetOrdinal(this.helper.H75);
                    if (!dr.IsDBNull(iH75)) entity.H75 = dr.GetDecimal(iH75);

                    int iH76 = dr.GetOrdinal(this.helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH77 = dr.GetOrdinal(this.helper.H77);
                    if (!dr.IsDBNull(iH77)) entity.H77 = dr.GetDecimal(iH77);

                    int iH78 = dr.GetOrdinal(this.helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH79 = dr.GetOrdinal(this.helper.H79);
                    if (!dr.IsDBNull(iH79)) entity.H79 = dr.GetDecimal(iH79);

                    int iH80 = dr.GetOrdinal(this.helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH81 = dr.GetOrdinal(this.helper.H81);
                    if (!dr.IsDBNull(iH81)) entity.H81 = dr.GetDecimal(iH81);

                    int iH82 = dr.GetOrdinal(this.helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH83 = dr.GetOrdinal(this.helper.H83);
                    if (!dr.IsDBNull(iH83)) entity.H83 = dr.GetDecimal(iH83);

                    int iH84 = dr.GetOrdinal(this.helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH85 = dr.GetOrdinal(this.helper.H85);
                    if (!dr.IsDBNull(iH85)) entity.H85 = dr.GetDecimal(iH85);

                    int iH86 = dr.GetOrdinal(this.helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH87 = dr.GetOrdinal(this.helper.H87);
                    if (!dr.IsDBNull(iH87)) entity.H87 = dr.GetDecimal(iH87);

                    int iH88 = dr.GetOrdinal(this.helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH89 = dr.GetOrdinal(this.helper.H89);
                    if (!dr.IsDBNull(iH89)) entity.H89 = dr.GetDecimal(iH89);

                    int iH90 = dr.GetOrdinal(this.helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH91 = dr.GetOrdinal(this.helper.H91);
                    if (!dr.IsDBNull(iH91)) entity.H91 = dr.GetDecimal(iH91);

                    int iH92 = dr.GetOrdinal(this.helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH93 = dr.GetOrdinal(this.helper.H93);
                    if (!dr.IsDBNull(iH93)) entity.H93 = dr.GetDecimal(iH93);

                    int iH94 = dr.GetOrdinal(this.helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH95 = dr.GetOrdinal(this.helper.H95);
                    if (!dr.IsDBNull(iH95)) entity.H95 = dr.GetDecimal(iH95);

                    int iH96 = dr.GetOrdinal(this.helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);

                    int iTgenercodi = dr.GetOrdinal(this.helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));


                            

                    entitys.Add(entity);
                }

                return entitys;
            }
        }

        #region Mejoras CMgN

        public List<MeScadaSp7DTO> ObtenerComparativoDemanda(int cnfbarcodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeScadaSp7DTO> entitys = new List<MeScadaSp7DTO>();

            string sql = string.Format(helper.SqlObtenerComparativoDemanda,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), cnfbarcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeScadaSp7DTO entity = new MeScadaSp7DTO();

                    int iCanalcodi = dr.GetOrdinal(helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    for (int i = 1; i <= 48; i++)
                    {
                        decimal? valor = null;
                        int index = dr.GetOrdinal("H" + i * 2);
                        if (!dr.IsDBNull(index)) valor = dr.GetDecimal(index);

                        entity.GetType().GetProperty("H" + i * 2).SetValue(entity, valor);
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public MeScadaSp7DTO ObtenerDemandaPorInterconexion(DateTime fecha, int canalcodi)
        {
            MeScadaSp7DTO entity = null;

            string sql = string.Format(helper.SqlObtenerDemandaPorInterconexion, fecha.ToString(ConstantesBase.FormatoFecha), canalcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new MeScadaSp7DTO();

                    int iCanalcodi = dr.GetOrdinal(helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    for (int i = 1; i <= 48; i++)
                    {
                        decimal? valor = null;
                        int index = dr.GetOrdinal("H" + i * 2);
                        if (!dr.IsDBNull(index)) valor = dr.GetDecimal(index);

                        entity.GetType().GetProperty("H" + i * 2).SetValue(entity, valor);
                    }                    
                }
            }

            return entity;
        }

        #endregion

        #region Informes SGI

        public List<MeMedicion48DTO> ObtenerDatosPorReporte(int reporte, DateTime fecha, int tipoinfocodi)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();

            string sql = string.Format(helper.SqlObtenerDatosPorReporte, reporte, fecha.ToString(ConstantesBase.FormatoFecha), tipoinfocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));                                      

                    for (int i = 1; i <= 48; i++)
                    {
                        decimal? valor = null;
                        int index = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(index)) valor = dr.GetDecimal(index);

                        entity.GetType().GetProperty("H" + i).SetValue(entity, valor);

                        int tipo = -1;
                        if (valor != null) tipo = 3;
                        entity.GetType().GetProperty("T" + i).SetValue(entity, tipo);
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        #endregion


        public bool SiExisteRegistroPorFechaYCanal(int canalcodi, DateTime fecha)
        {
            String sql = String.Format(this.helper.SqlSiExisteRegistroPorFechaYCanal, canalcodi, this.getFechaBloque24horas(fecha).ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            if (Convert.ToInt32(dbProvider.ExecuteScalar(command)) == 1)
            {
                return true;
            }

            return false;
        }

        public void AgregarRegistroPorFechaYBloque(int canalcodi, DateTime fecha, string bloque, string usuario, decimal valor)
        {
            String sql = String.Format(this.helper.SqlAgregarRegistroPorFechaYBloque, bloque, canalcodi, this.getFechaBloque24horas(fecha).ToString(ConstantesBase.FormatoFecha), valor, usuario);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarRegistroPorFechaYBloque(int canalcodi, DateTime fecha, string bloque, string usuario, decimal valor)
        {
            String sql = String.Format(this.helper.SqlActualizarRegistroPorFechaYBloque, bloque, valor, usuario, this.getFechaBloque24horas(fecha).ToString(ConstantesBase.FormatoFecha), canalcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarRegistrosNulosPorFechaYBloque(DateTime fecha, string bloque)
        {
            String sql = String.Format(this.helper.SqlActualizarRegistrosNulosPorFechaYBloque, bloque, this.getFechaBloque24horas(fecha).ToString(ConstantesBase.FormatoFecha), bloque);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.ExecuteNonQuery(command);
        }

        private DateTime getFechaBloque24horas(DateTime fecha)
        {
            if (fecha.Hour == 0 && fecha.Minute == 0)
            {
                return fecha.AddMinutes(-1);
            }

            return fecha;
        }

    }
}