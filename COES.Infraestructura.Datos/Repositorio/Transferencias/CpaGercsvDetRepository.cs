using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CPA_GERCSVDET_TMP
    /// </summary>
    public class CpaGercsvDetRepository : RepositoryBase, ICpaGercsvDetRepository
    {
        public CpaGercsvDetRepository(string strConn)
            : base(strConn)
        {
        }

        CpaGercsvDetHelper helper = new CpaGercsvDetHelper();

        public int Save(CpaGercsvDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpagedcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpagercodi, DbType.Int32, entity.Cpagercodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Cpagedtipcsv, DbType.String, entity.Cpagedtipcsv);
            dbProvider.AddInParameter(command, helper.Cpagedfecha, DbType.DateTime, entity.Cpagedfecha);
            dbProvider.AddInParameter(command, helper.Cpagedh1, DbType.Decimal, entity.Cpagedh1);
            dbProvider.AddInParameter(command, helper.Cpagedh2, DbType.Decimal, entity.Cpagedh2);
            dbProvider.AddInParameter(command, helper.Cpagedh3, DbType.Decimal, entity.Cpagedh3);
            dbProvider.AddInParameter(command, helper.Cpagedh4, DbType.Decimal, entity.Cpagedh4);
            dbProvider.AddInParameter(command, helper.Cpagedh5, DbType.Decimal, entity.Cpagedh5);
            dbProvider.AddInParameter(command, helper.Cpagedh6, DbType.Decimal, entity.Cpagedh6);
            dbProvider.AddInParameter(command, helper.Cpagedh7, DbType.Decimal, entity.Cpagedh7);
            dbProvider.AddInParameter(command, helper.Cpagedh8, DbType.Decimal, entity.Cpagedh8);
            dbProvider.AddInParameter(command, helper.Cpagedh9, DbType.Decimal, entity.Cpagedh9);
            dbProvider.AddInParameter(command, helper.Cpagedh10, DbType.Decimal, entity.Cpagedh10);
            dbProvider.AddInParameter(command, helper.Cpagedh11, DbType.Decimal, entity.Cpagedh11);
            dbProvider.AddInParameter(command, helper.Cpagedh12, DbType.Decimal, entity.Cpagedh12);
            dbProvider.AddInParameter(command, helper.Cpagedh13, DbType.Decimal, entity.Cpagedh13);
            dbProvider.AddInParameter(command, helper.Cpagedh14, DbType.Decimal, entity.Cpagedh14);
            dbProvider.AddInParameter(command, helper.Cpagedh15, DbType.Decimal, entity.Cpagedh15);
            dbProvider.AddInParameter(command, helper.Cpagedh16, DbType.Decimal, entity.Cpagedh16);
            dbProvider.AddInParameter(command, helper.Cpagedh17, DbType.Decimal, entity.Cpagedh17);
            dbProvider.AddInParameter(command, helper.Cpagedh18, DbType.Decimal, entity.Cpagedh18);
            dbProvider.AddInParameter(command, helper.Cpagedh19, DbType.Decimal, entity.Cpagedh19);
            dbProvider.AddInParameter(command, helper.Cpagedh20, DbType.Decimal, entity.Cpagedh20);
            dbProvider.AddInParameter(command, helper.Cpagedh21, DbType.Decimal, entity.Cpagedh21);
            dbProvider.AddInParameter(command, helper.Cpagedh22, DbType.Decimal, entity.Cpagedh22);
            dbProvider.AddInParameter(command, helper.Cpagedh23, DbType.Decimal, entity.Cpagedh23);
            dbProvider.AddInParameter(command, helper.Cpagedh24, DbType.Decimal, entity.Cpagedh24);
            dbProvider.AddInParameter(command, helper.Cpagedh25, DbType.Decimal, entity.Cpagedh25);
            dbProvider.AddInParameter(command, helper.Cpagedh26, DbType.Decimal, entity.Cpagedh26);
            dbProvider.AddInParameter(command, helper.Cpagedh27, DbType.Decimal, entity.Cpagedh27);
            dbProvider.AddInParameter(command, helper.Cpagedh28, DbType.Decimal, entity.Cpagedh28);
            dbProvider.AddInParameter(command, helper.Cpagedh29, DbType.Decimal, entity.Cpagedh29);
            dbProvider.AddInParameter(command, helper.Cpagedh30, DbType.Decimal, entity.Cpagedh30);
            dbProvider.AddInParameter(command, helper.Cpagedh31, DbType.Decimal, entity.Cpagedh31);
            dbProvider.AddInParameter(command, helper.Cpagedh32, DbType.Decimal, entity.Cpagedh32);
            dbProvider.AddInParameter(command, helper.Cpagedh33, DbType.Decimal, entity.Cpagedh33);
            dbProvider.AddInParameter(command, helper.Cpagedh34, DbType.Decimal, entity.Cpagedh34);
            dbProvider.AddInParameter(command, helper.Cpagedh35, DbType.Decimal, entity.Cpagedh35);
            dbProvider.AddInParameter(command, helper.Cpagedh36, DbType.Decimal, entity.Cpagedh36);
            dbProvider.AddInParameter(command, helper.Cpagedh37, DbType.Decimal, entity.Cpagedh37);
            dbProvider.AddInParameter(command, helper.Cpagedh38, DbType.Decimal, entity.Cpagedh38);
            dbProvider.AddInParameter(command, helper.Cpagedh39, DbType.Decimal, entity.Cpagedh39);
            dbProvider.AddInParameter(command, helper.Cpagedh40, DbType.Decimal, entity.Cpagedh40);
            dbProvider.AddInParameter(command, helper.Cpagedh41, DbType.Decimal, entity.Cpagedh41);
            dbProvider.AddInParameter(command, helper.Cpagedh42, DbType.Decimal, entity.Cpagedh42);
            dbProvider.AddInParameter(command, helper.Cpagedh43, DbType.Decimal, entity.Cpagedh43);
            dbProvider.AddInParameter(command, helper.Cpagedh44, DbType.Decimal, entity.Cpagedh44);
            dbProvider.AddInParameter(command, helper.Cpagedh45, DbType.Decimal, entity.Cpagedh45);
            dbProvider.AddInParameter(command, helper.Cpagedh46, DbType.Decimal, entity.Cpagedh46);
            dbProvider.AddInParameter(command, helper.Cpagedh47, DbType.Decimal, entity.Cpagedh47);
            dbProvider.AddInParameter(command, helper.Cpagedh48, DbType.Decimal, entity.Cpagedh48);
            dbProvider.AddInParameter(command, helper.Cpagedh49, DbType.Decimal, entity.Cpagedh49);
            dbProvider.AddInParameter(command, helper.Cpagedh50, DbType.Decimal, entity.Cpagedh50);
            dbProvider.AddInParameter(command, helper.Cpagedh51, DbType.Decimal, entity.Cpagedh51);
            dbProvider.AddInParameter(command, helper.Cpagedh52, DbType.Decimal, entity.Cpagedh52);
            dbProvider.AddInParameter(command, helper.Cpagedh53, DbType.Decimal, entity.Cpagedh53);
            dbProvider.AddInParameter(command, helper.Cpagedh54, DbType.Decimal, entity.Cpagedh54);
            dbProvider.AddInParameter(command, helper.Cpagedh55, DbType.Decimal, entity.Cpagedh55);
            dbProvider.AddInParameter(command, helper.Cpagedh56, DbType.Decimal, entity.Cpagedh56);
            dbProvider.AddInParameter(command, helper.Cpagedh57, DbType.Decimal, entity.Cpagedh57);
            dbProvider.AddInParameter(command, helper.Cpagedh58, DbType.Decimal, entity.Cpagedh58);
            dbProvider.AddInParameter(command, helper.Cpagedh59, DbType.Decimal, entity.Cpagedh59);
            dbProvider.AddInParameter(command, helper.Cpagedh60, DbType.Decimal, entity.Cpagedh60);
            dbProvider.AddInParameter(command, helper.Cpagedh61, DbType.Decimal, entity.Cpagedh61);
            dbProvider.AddInParameter(command, helper.Cpagedh62, DbType.Decimal, entity.Cpagedh62);
            dbProvider.AddInParameter(command, helper.Cpagedh63, DbType.Decimal, entity.Cpagedh63);
            dbProvider.AddInParameter(command, helper.Cpagedh64, DbType.Decimal, entity.Cpagedh64);
            dbProvider.AddInParameter(command, helper.Cpagedh65, DbType.Decimal, entity.Cpagedh65);
            dbProvider.AddInParameter(command, helper.Cpagedh66, DbType.Decimal, entity.Cpagedh66);
            dbProvider.AddInParameter(command, helper.Cpagedh67, DbType.Decimal, entity.Cpagedh67);
            dbProvider.AddInParameter(command, helper.Cpagedh68, DbType.Decimal, entity.Cpagedh68);
            dbProvider.AddInParameter(command, helper.Cpagedh69, DbType.Decimal, entity.Cpagedh69);
            dbProvider.AddInParameter(command, helper.Cpagedh70, DbType.Decimal, entity.Cpagedh70);
            dbProvider.AddInParameter(command, helper.Cpagedh71, DbType.Decimal, entity.Cpagedh71);
            dbProvider.AddInParameter(command, helper.Cpagedh72, DbType.Decimal, entity.Cpagedh72);
            dbProvider.AddInParameter(command, helper.Cpagedh73, DbType.Decimal, entity.Cpagedh73);
            dbProvider.AddInParameter(command, helper.Cpagedh74, DbType.Decimal, entity.Cpagedh74);
            dbProvider.AddInParameter(command, helper.Cpagedh75, DbType.Decimal, entity.Cpagedh75);
            dbProvider.AddInParameter(command, helper.Cpagedh76, DbType.Decimal, entity.Cpagedh76);
            dbProvider.AddInParameter(command, helper.Cpagedh77, DbType.Decimal, entity.Cpagedh77);
            dbProvider.AddInParameter(command, helper.Cpagedh78, DbType.Decimal, entity.Cpagedh78);
            dbProvider.AddInParameter(command, helper.Cpagedh79, DbType.Decimal, entity.Cpagedh79);
            dbProvider.AddInParameter(command, helper.Cpagedh80, DbType.Decimal, entity.Cpagedh80);
            dbProvider.AddInParameter(command, helper.Cpagedh81, DbType.Decimal, entity.Cpagedh81);
            dbProvider.AddInParameter(command, helper.Cpagedh82, DbType.Decimal, entity.Cpagedh82);
            dbProvider.AddInParameter(command, helper.Cpagedh83, DbType.Decimal, entity.Cpagedh83);
            dbProvider.AddInParameter(command, helper.Cpagedh84, DbType.Decimal, entity.Cpagedh84);
            dbProvider.AddInParameter(command, helper.Cpagedh85, DbType.Decimal, entity.Cpagedh85);
            dbProvider.AddInParameter(command, helper.Cpagedh86, DbType.Decimal, entity.Cpagedh86);
            dbProvider.AddInParameter(command, helper.Cpagedh87, DbType.Decimal, entity.Cpagedh87);
            dbProvider.AddInParameter(command, helper.Cpagedh88, DbType.Decimal, entity.Cpagedh88);
            dbProvider.AddInParameter(command, helper.Cpagedh89, DbType.Decimal, entity.Cpagedh89);
            dbProvider.AddInParameter(command, helper.Cpagedh90, DbType.Decimal, entity.Cpagedh90);
            dbProvider.AddInParameter(command, helper.Cpagedh91, DbType.Decimal, entity.Cpagedh91);
            dbProvider.AddInParameter(command, helper.Cpagedh92, DbType.Decimal, entity.Cpagedh92);
            dbProvider.AddInParameter(command, helper.Cpagedh93, DbType.Decimal, entity.Cpagedh93);
            dbProvider.AddInParameter(command, helper.Cpagedh94, DbType.Decimal, entity.Cpagedh94);
            dbProvider.AddInParameter(command, helper.Cpagedh95, DbType.Decimal, entity.Cpagedh95);
            dbProvider.AddInParameter(command, helper.Cpagedh96, DbType.Decimal, entity.Cpagedh96);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CpaGercsvDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cpagercodi, DbType.Int32, entity.Cpagercodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Cpagedtipcsv, DbType.String, entity.Cpagedtipcsv);
            dbProvider.AddInParameter(command, helper.Cpagedfecha, DbType.DateTime, entity.Cpagedfecha);
            dbProvider.AddInParameter(command, helper.Cpagedh1, DbType.Decimal, entity.Cpagedh1);
            dbProvider.AddInParameter(command, helper.Cpagedh2, DbType.Decimal, entity.Cpagedh2);
            dbProvider.AddInParameter(command, helper.Cpagedh3, DbType.Decimal, entity.Cpagedh3);
            dbProvider.AddInParameter(command, helper.Cpagedh4, DbType.Decimal, entity.Cpagedh4);
            dbProvider.AddInParameter(command, helper.Cpagedh5, DbType.Decimal, entity.Cpagedh5);
            dbProvider.AddInParameter(command, helper.Cpagedh6, DbType.Decimal, entity.Cpagedh6);
            dbProvider.AddInParameter(command, helper.Cpagedh7, DbType.Decimal, entity.Cpagedh7);
            dbProvider.AddInParameter(command, helper.Cpagedh8, DbType.Decimal, entity.Cpagedh8);
            dbProvider.AddInParameter(command, helper.Cpagedh9, DbType.Decimal, entity.Cpagedh9);
            dbProvider.AddInParameter(command, helper.Cpagedh10, DbType.Decimal, entity.Cpagedh10);
            dbProvider.AddInParameter(command, helper.Cpagedh11, DbType.Decimal, entity.Cpagedh11);
            dbProvider.AddInParameter(command, helper.Cpagedh12, DbType.Decimal, entity.Cpagedh12);
            dbProvider.AddInParameter(command, helper.Cpagedh13, DbType.Decimal, entity.Cpagedh13);
            dbProvider.AddInParameter(command, helper.Cpagedh14, DbType.Decimal, entity.Cpagedh14);
            dbProvider.AddInParameter(command, helper.Cpagedh15, DbType.Decimal, entity.Cpagedh15);
            dbProvider.AddInParameter(command, helper.Cpagedh16, DbType.Decimal, entity.Cpagedh16);
            dbProvider.AddInParameter(command, helper.Cpagedh17, DbType.Decimal, entity.Cpagedh17);
            dbProvider.AddInParameter(command, helper.Cpagedh18, DbType.Decimal, entity.Cpagedh18);
            dbProvider.AddInParameter(command, helper.Cpagedh19, DbType.Decimal, entity.Cpagedh19);
            dbProvider.AddInParameter(command, helper.Cpagedh20, DbType.Decimal, entity.Cpagedh20);
            dbProvider.AddInParameter(command, helper.Cpagedh21, DbType.Decimal, entity.Cpagedh21);
            dbProvider.AddInParameter(command, helper.Cpagedh22, DbType.Decimal, entity.Cpagedh22);
            dbProvider.AddInParameter(command, helper.Cpagedh23, DbType.Decimal, entity.Cpagedh23);
            dbProvider.AddInParameter(command, helper.Cpagedh24, DbType.Decimal, entity.Cpagedh24);
            dbProvider.AddInParameter(command, helper.Cpagedh25, DbType.Decimal, entity.Cpagedh25);
            dbProvider.AddInParameter(command, helper.Cpagedh26, DbType.Decimal, entity.Cpagedh26);
            dbProvider.AddInParameter(command, helper.Cpagedh27, DbType.Decimal, entity.Cpagedh27);
            dbProvider.AddInParameter(command, helper.Cpagedh28, DbType.Decimal, entity.Cpagedh28);
            dbProvider.AddInParameter(command, helper.Cpagedh29, DbType.Decimal, entity.Cpagedh29);
            dbProvider.AddInParameter(command, helper.Cpagedh30, DbType.Decimal, entity.Cpagedh30);
            dbProvider.AddInParameter(command, helper.Cpagedh31, DbType.Decimal, entity.Cpagedh31);
            dbProvider.AddInParameter(command, helper.Cpagedh32, DbType.Decimal, entity.Cpagedh32);
            dbProvider.AddInParameter(command, helper.Cpagedh33, DbType.Decimal, entity.Cpagedh33);
            dbProvider.AddInParameter(command, helper.Cpagedh34, DbType.Decimal, entity.Cpagedh34);
            dbProvider.AddInParameter(command, helper.Cpagedh35, DbType.Decimal, entity.Cpagedh35);
            dbProvider.AddInParameter(command, helper.Cpagedh36, DbType.Decimal, entity.Cpagedh36);
            dbProvider.AddInParameter(command, helper.Cpagedh37, DbType.Decimal, entity.Cpagedh37);
            dbProvider.AddInParameter(command, helper.Cpagedh38, DbType.Decimal, entity.Cpagedh38);
            dbProvider.AddInParameter(command, helper.Cpagedh39, DbType.Decimal, entity.Cpagedh39);
            dbProvider.AddInParameter(command, helper.Cpagedh40, DbType.Decimal, entity.Cpagedh40);
            dbProvider.AddInParameter(command, helper.Cpagedh41, DbType.Decimal, entity.Cpagedh41);
            dbProvider.AddInParameter(command, helper.Cpagedh42, DbType.Decimal, entity.Cpagedh42);
            dbProvider.AddInParameter(command, helper.Cpagedh43, DbType.Decimal, entity.Cpagedh43);
            dbProvider.AddInParameter(command, helper.Cpagedh44, DbType.Decimal, entity.Cpagedh44);
            dbProvider.AddInParameter(command, helper.Cpagedh45, DbType.Decimal, entity.Cpagedh45);
            dbProvider.AddInParameter(command, helper.Cpagedh46, DbType.Decimal, entity.Cpagedh46);
            dbProvider.AddInParameter(command, helper.Cpagedh47, DbType.Decimal, entity.Cpagedh47);
            dbProvider.AddInParameter(command, helper.Cpagedh48, DbType.Decimal, entity.Cpagedh48);
            dbProvider.AddInParameter(command, helper.Cpagedh49, DbType.Decimal, entity.Cpagedh49);
            dbProvider.AddInParameter(command, helper.Cpagedh50, DbType.Decimal, entity.Cpagedh50);
            dbProvider.AddInParameter(command, helper.Cpagedh51, DbType.Decimal, entity.Cpagedh51);
            dbProvider.AddInParameter(command, helper.Cpagedh52, DbType.Decimal, entity.Cpagedh52);
            dbProvider.AddInParameter(command, helper.Cpagedh53, DbType.Decimal, entity.Cpagedh53);
            dbProvider.AddInParameter(command, helper.Cpagedh54, DbType.Decimal, entity.Cpagedh54);
            dbProvider.AddInParameter(command, helper.Cpagedh55, DbType.Decimal, entity.Cpagedh55);
            dbProvider.AddInParameter(command, helper.Cpagedh56, DbType.Decimal, entity.Cpagedh56);
            dbProvider.AddInParameter(command, helper.Cpagedh57, DbType.Decimal, entity.Cpagedh57);
            dbProvider.AddInParameter(command, helper.Cpagedh58, DbType.Decimal, entity.Cpagedh58);
            dbProvider.AddInParameter(command, helper.Cpagedh59, DbType.Decimal, entity.Cpagedh59);
            dbProvider.AddInParameter(command, helper.Cpagedh60, DbType.Decimal, entity.Cpagedh60);
            dbProvider.AddInParameter(command, helper.Cpagedcodi, DbType.Int32, entity.Cpagedcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpaGercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpagedcodi, DbType.Int32, cpaGercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpaGercsvDetDTO GetById(int cpaGercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpagedcodi, DbType.Int32, cpaGercodi);
            CpaGercsvDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpaGercsvDetDTO> List(int cpagercodi, string cpagedtipcsv, DateTime dFecEjercicio, DateTime dFecEjercicioFin)
        {
            List<CpaGercsvDetDTO> entities = new List<CpaGercsvDetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Cpagercodi, DbType.Int32, cpagercodi);
            dbProvider.AddInParameter(command, helper.Cpagedtipcsv, DbType.String, cpagedtipcsv);
            dbProvider.AddInParameter(command, helper.Cpagedfecha, DbType.DateTime, dFecEjercicio);
            dbProvider.AddInParameter(command, helper.Cpagedfecha, DbType.DateTime, dFecEjercicioFin);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpaGercsvDetDTO entity = new CpaGercsvDetDTO();

                    int iCpagercodi = dr.GetOrdinal(helper.Cpagercodi);
                    if (!dr.IsDBNull(iCpagercodi)) entity.Cpagercodi = Convert.ToInt32(dr.GetValue(iCpagercodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCpagedtipcsv = dr.GetOrdinal(helper.Cpagedtipcsv);
                    if (!dr.IsDBNull(iCpagedtipcsv)) entity.Cpagedtipcsv = dr.GetString(iCpagedtipcsv);

                    int iCpagedfecha = dr.GetOrdinal(helper.Cpagedfecha);
                    if (!dr.IsDBNull(iCpagedfecha)) entity.Cpagedfecha = dr.GetDateTime(iCpagedfecha);

                    int iCpagedh1 = dr.GetOrdinal(helper.Cpagedh1);
                    if (!dr.IsDBNull(iCpagedh1)) entity.Cpagedh1 = dr.GetDecimal(iCpagedh1);

                    int iCpagedh2 = dr.GetOrdinal(helper.Cpagedh2);
                    if (!dr.IsDBNull(iCpagedh2)) entity.Cpagedh2 = dr.GetDecimal(iCpagedh2);

                    int iCpagedh3 = dr.GetOrdinal(helper.Cpagedh3);
                    if (!dr.IsDBNull(iCpagedh3)) entity.Cpagedh3 = dr.GetDecimal(iCpagedh3);

                    int iCpagedh4 = dr.GetOrdinal(helper.Cpagedh4);
                    if (!dr.IsDBNull(iCpagedh4)) entity.Cpagedh4 = dr.GetDecimal(iCpagedh4);

                    int iCpagedh5 = dr.GetOrdinal(helper.Cpagedh5);
                    if (!dr.IsDBNull(iCpagedh5)) entity.Cpagedh5 = dr.GetDecimal(iCpagedh5);

                    int iCpagedh6 = dr.GetOrdinal(helper.Cpagedh6);
                    if (!dr.IsDBNull(iCpagedh6)) entity.Cpagedh6 = dr.GetDecimal(iCpagedh6);

                    int iCpagedh7 = dr.GetOrdinal(helper.Cpagedh7);
                    if (!dr.IsDBNull(iCpagedh7)) entity.Cpagedh7 = dr.GetDecimal(iCpagedh7);

                    int iCpagedh8 = dr.GetOrdinal(helper.Cpagedh8);
                    if (!dr.IsDBNull(iCpagedh8)) entity.Cpagedh8 = dr.GetDecimal(iCpagedh8);

                    int iCpagedh9 = dr.GetOrdinal(helper.Cpagedh9);
                    if (!dr.IsDBNull(iCpagedh9)) entity.Cpagedh9 = dr.GetDecimal(iCpagedh9);

                    int iCpagedh10 = dr.GetOrdinal(helper.Cpagedh10);
                    if (!dr.IsDBNull(iCpagedh10)) entity.Cpagedh10 = dr.GetDecimal(iCpagedh10);

                    int iCpagedh11 = dr.GetOrdinal(helper.Cpagedh11);
                    if (!dr.IsDBNull(iCpagedh11)) entity.Cpagedh11 = dr.GetDecimal(iCpagedh11);

                    int iCpagedh12 = dr.GetOrdinal(helper.Cpagedh12);
                    if (!dr.IsDBNull(iCpagedh12)) entity.Cpagedh12 = dr.GetDecimal(iCpagedh12);

                    int iCpagedh13 = dr.GetOrdinal(helper.Cpagedh13);
                    if (!dr.IsDBNull(iCpagedh13)) entity.Cpagedh13 = dr.GetDecimal(iCpagedh13);

                    int iCpagedh14 = dr.GetOrdinal(helper.Cpagedh14);
                    if (!dr.IsDBNull(iCpagedh14)) entity.Cpagedh14 = dr.GetDecimal(iCpagedh14);

                    int iCpagedh15 = dr.GetOrdinal(helper.Cpagedh15);
                    if (!dr.IsDBNull(iCpagedh15)) entity.Cpagedh15 = dr.GetDecimal(iCpagedh15);

                    int iCpagedh16 = dr.GetOrdinal(helper.Cpagedh16);
                    if (!dr.IsDBNull(iCpagedh16)) entity.Cpagedh16 = dr.GetDecimal(iCpagedh16);

                    int iCpagedh17 = dr.GetOrdinal(helper.Cpagedh17);
                    if (!dr.IsDBNull(iCpagedh17)) entity.Cpagedh17 = dr.GetDecimal(iCpagedh17);

                    int iCpagedh18 = dr.GetOrdinal(helper.Cpagedh18);
                    if (!dr.IsDBNull(iCpagedh18)) entity.Cpagedh18 = dr.GetDecimal(iCpagedh18);

                    int iCpagedh19 = dr.GetOrdinal(helper.Cpagedh19);
                    if (!dr.IsDBNull(iCpagedh19)) entity.Cpagedh19 = dr.GetDecimal(iCpagedh19);

                    int iCpagedh20 = dr.GetOrdinal(helper.Cpagedh20);
                    if (!dr.IsDBNull(iCpagedh20)) entity.Cpagedh20 = dr.GetDecimal(iCpagedh20);

                    int iCpagedh21 = dr.GetOrdinal(helper.Cpagedh21);
                    if (!dr.IsDBNull(iCpagedh21)) entity.Cpagedh21 = dr.GetDecimal(iCpagedh21);

                    int iCpagedh22 = dr.GetOrdinal(helper.Cpagedh22);
                    if (!dr.IsDBNull(iCpagedh22)) entity.Cpagedh22 = dr.GetDecimal(iCpagedh22);

                    int iCpagedh23 = dr.GetOrdinal(helper.Cpagedh23);
                    if (!dr.IsDBNull(iCpagedh23)) entity.Cpagedh23 = dr.GetDecimal(iCpagedh23);

                    int iCpagedh24 = dr.GetOrdinal(helper.Cpagedh24);
                    if (!dr.IsDBNull(iCpagedh24)) entity.Cpagedh24 = dr.GetDecimal(iCpagedh24);

                    int iCpagedh25 = dr.GetOrdinal(helper.Cpagedh25);
                    if (!dr.IsDBNull(iCpagedh25)) entity.Cpagedh25 = dr.GetDecimal(iCpagedh25);

                    int iCpagedh26 = dr.GetOrdinal(helper.Cpagedh26);
                    if (!dr.IsDBNull(iCpagedh26)) entity.Cpagedh26 = dr.GetDecimal(iCpagedh26);

                    int iCpagedh27 = dr.GetOrdinal(helper.Cpagedh27);
                    if (!dr.IsDBNull(iCpagedh27)) entity.Cpagedh27 = dr.GetDecimal(iCpagedh27);

                    int iCpagedh28 = dr.GetOrdinal(helper.Cpagedh28);
                    if (!dr.IsDBNull(iCpagedh28)) entity.Cpagedh28 = dr.GetDecimal(iCpagedh28);

                    int iCpagedh29 = dr.GetOrdinal(helper.Cpagedh29);
                    if (!dr.IsDBNull(iCpagedh29)) entity.Cpagedh29 = dr.GetDecimal(iCpagedh29);

                    int iCpagedh30 = dr.GetOrdinal(helper.Cpagedh30);
                    if (!dr.IsDBNull(iCpagedh30)) entity.Cpagedh30 = dr.GetDecimal(iCpagedh30);

                    int iCpagedh31 = dr.GetOrdinal(helper.Cpagedh31);
                    if (!dr.IsDBNull(iCpagedh31)) entity.Cpagedh31 = dr.GetDecimal(iCpagedh31);

                    int iCpagedh32 = dr.GetOrdinal(helper.Cpagedh32);
                    if (!dr.IsDBNull(iCpagedh32)) entity.Cpagedh32 = dr.GetDecimal(iCpagedh32);

                    int iCpagedh33 = dr.GetOrdinal(helper.Cpagedh33);
                    if (!dr.IsDBNull(iCpagedh33)) entity.Cpagedh33 = dr.GetDecimal(iCpagedh33);

                    int iCpagedh34 = dr.GetOrdinal(helper.Cpagedh34);
                    if (!dr.IsDBNull(iCpagedh34)) entity.Cpagedh34 = dr.GetDecimal(iCpagedh34);

                    int iCpagedh35 = dr.GetOrdinal(helper.Cpagedh35);
                    if (!dr.IsDBNull(iCpagedh35)) entity.Cpagedh35 = dr.GetDecimal(iCpagedh35);

                    int iCpagedh36 = dr.GetOrdinal(helper.Cpagedh36);
                    if (!dr.IsDBNull(iCpagedh36)) entity.Cpagedh36 = dr.GetDecimal(iCpagedh36);

                    int iCpagedh37 = dr.GetOrdinal(helper.Cpagedh37);
                    if (!dr.IsDBNull(iCpagedh37)) entity.Cpagedh37 = dr.GetDecimal(iCpagedh37);

                    int iCpagedh38 = dr.GetOrdinal(helper.Cpagedh38);
                    if (!dr.IsDBNull(iCpagedh38)) entity.Cpagedh38 = dr.GetDecimal(iCpagedh38);

                    int iCpagedh39 = dr.GetOrdinal(helper.Cpagedh39);
                    if (!dr.IsDBNull(iCpagedh39)) entity.Cpagedh39 = dr.GetDecimal(iCpagedh39);

                    int iCpagedh40 = dr.GetOrdinal(helper.Cpagedh40);
                    if (!dr.IsDBNull(iCpagedh40)) entity.Cpagedh40 = dr.GetDecimal(iCpagedh40);

                    int iCpagedh41 = dr.GetOrdinal(helper.Cpagedh41);
                    if (!dr.IsDBNull(iCpagedh41)) entity.Cpagedh41 = dr.GetDecimal(iCpagedh41);

                    int iCpagedh42 = dr.GetOrdinal(helper.Cpagedh42);
                    if (!dr.IsDBNull(iCpagedh42)) entity.Cpagedh42 = dr.GetDecimal(iCpagedh42);

                    int iCpagedh43 = dr.GetOrdinal(helper.Cpagedh43);
                    if (!dr.IsDBNull(iCpagedh43)) entity.Cpagedh43 = dr.GetDecimal(iCpagedh43);

                    int iCpagedh44 = dr.GetOrdinal(helper.Cpagedh44);
                    if (!dr.IsDBNull(iCpagedh44)) entity.Cpagedh44 = dr.GetDecimal(iCpagedh44);

                    int iCpagedh45 = dr.GetOrdinal(helper.Cpagedh45);
                    if (!dr.IsDBNull(iCpagedh45)) entity.Cpagedh45 = dr.GetDecimal(iCpagedh45);

                    int iCpagedh46 = dr.GetOrdinal(helper.Cpagedh46);
                    if (!dr.IsDBNull(iCpagedh46)) entity.Cpagedh46 = dr.GetDecimal(iCpagedh46);

                    int iCpagedh47 = dr.GetOrdinal(helper.Cpagedh47);
                    if (!dr.IsDBNull(iCpagedh47)) entity.Cpagedh47 = dr.GetDecimal(iCpagedh47);

                    int iCpagedh48 = dr.GetOrdinal(helper.Cpagedh48);
                    if (!dr.IsDBNull(iCpagedh48)) entity.Cpagedh48 = dr.GetDecimal(iCpagedh48);

                    int iCpagedh49 = dr.GetOrdinal(helper.Cpagedh49);
                    if (!dr.IsDBNull(iCpagedh49)) entity.Cpagedh49 = dr.GetDecimal(iCpagedh49);

                    int iCpagedh50 = dr.GetOrdinal(helper.Cpagedh50);
                    if (!dr.IsDBNull(iCpagedh50)) entity.Cpagedh50 = dr.GetDecimal(iCpagedh50);

                    int iCpagedh51 = dr.GetOrdinal(helper.Cpagedh51);
                    if (!dr.IsDBNull(iCpagedh51)) entity.Cpagedh51 = dr.GetDecimal(iCpagedh51);

                    int iCpagedh52 = dr.GetOrdinal(helper.Cpagedh52);
                    if (!dr.IsDBNull(iCpagedh52)) entity.Cpagedh52 = dr.GetDecimal(iCpagedh52);

                    int iCpagedh53 = dr.GetOrdinal(helper.Cpagedh53);
                    if (!dr.IsDBNull(iCpagedh53)) entity.Cpagedh53 = dr.GetDecimal(iCpagedh53);

                    int iCpagedh54 = dr.GetOrdinal(helper.Cpagedh54);
                    if (!dr.IsDBNull(iCpagedh54)) entity.Cpagedh54 = dr.GetDecimal(iCpagedh54);

                    int iCpagedh55 = dr.GetOrdinal(helper.Cpagedh55);
                    if (!dr.IsDBNull(iCpagedh55)) entity.Cpagedh55 = dr.GetDecimal(iCpagedh55);

                    int iCpagedh56 = dr.GetOrdinal(helper.Cpagedh56);
                    if (!dr.IsDBNull(iCpagedh56)) entity.Cpagedh56 = dr.GetDecimal(iCpagedh56);

                    int iCpagedh57 = dr.GetOrdinal(helper.Cpagedh57);
                    if (!dr.IsDBNull(iCpagedh57)) entity.Cpagedh57 = dr.GetDecimal(iCpagedh57);

                    int iCpagedh58 = dr.GetOrdinal(helper.Cpagedh58);
                    if (!dr.IsDBNull(iCpagedh58)) entity.Cpagedh58 = dr.GetDecimal(iCpagedh58);

                    int iCpagedh59 = dr.GetOrdinal(helper.Cpagedh59);
                    if (!dr.IsDBNull(iCpagedh59)) entity.Cpagedh59 = dr.GetDecimal(iCpagedh59);

                    int iCpagedh60 = dr.GetOrdinal(helper.Cpagedh60);
                    if (!dr.IsDBNull(iCpagedh60)) entity.Cpagedh60 = dr.GetDecimal(iCpagedh60);

                    int iCpagedh61 = dr.GetOrdinal(helper.Cpagedh61);
                    if (!dr.IsDBNull(iCpagedh61)) entity.Cpagedh61 = dr.GetDecimal(iCpagedh61);

                    int iCpagedh62 = dr.GetOrdinal(helper.Cpagedh62);
                    if (!dr.IsDBNull(iCpagedh62)) entity.Cpagedh62 = dr.GetDecimal(iCpagedh62);

                    int iCpagedh63 = dr.GetOrdinal(helper.Cpagedh63);
                    if (!dr.IsDBNull(iCpagedh63)) entity.Cpagedh63 = dr.GetDecimal(iCpagedh63);

                    int iCpagedh64 = dr.GetOrdinal(helper.Cpagedh64);
                    if (!dr.IsDBNull(iCpagedh64)) entity.Cpagedh64 = dr.GetDecimal(iCpagedh64);

                    int iCpagedh65 = dr.GetOrdinal(helper.Cpagedh65);
                    if (!dr.IsDBNull(iCpagedh65)) entity.Cpagedh65 = dr.GetDecimal(iCpagedh65);

                    int iCpagedh66 = dr.GetOrdinal(helper.Cpagedh66);
                    if (!dr.IsDBNull(iCpagedh66)) entity.Cpagedh66 = dr.GetDecimal(iCpagedh66);

                    int iCpagedh67 = dr.GetOrdinal(helper.Cpagedh67);
                    if (!dr.IsDBNull(iCpagedh67)) entity.Cpagedh67 = dr.GetDecimal(iCpagedh67);

                    int iCpagedh68 = dr.GetOrdinal(helper.Cpagedh68);
                    if (!dr.IsDBNull(iCpagedh68)) entity.Cpagedh68 = dr.GetDecimal(iCpagedh68);

                    int iCpagedh69 = dr.GetOrdinal(helper.Cpagedh69);
                    if (!dr.IsDBNull(iCpagedh69)) entity.Cpagedh69 = dr.GetDecimal(iCpagedh69);

                    int iCpagedh70 = dr.GetOrdinal(helper.Cpagedh70);
                    if (!dr.IsDBNull(iCpagedh70)) entity.Cpagedh70 = dr.GetDecimal(iCpagedh70);

                    int iCpagedh71 = dr.GetOrdinal(helper.Cpagedh71);
                    if (!dr.IsDBNull(iCpagedh71)) entity.Cpagedh71 = dr.GetDecimal(iCpagedh71);

                    int iCpagedh72 = dr.GetOrdinal(helper.Cpagedh72);
                    if (!dr.IsDBNull(iCpagedh72)) entity.Cpagedh72 = dr.GetDecimal(iCpagedh72);

                    int iCpagedh73 = dr.GetOrdinal(helper.Cpagedh73);
                    if (!dr.IsDBNull(iCpagedh73)) entity.Cpagedh73 = dr.GetDecimal(iCpagedh73);

                    int iCpagedh74 = dr.GetOrdinal(helper.Cpagedh74);
                    if (!dr.IsDBNull(iCpagedh74)) entity.Cpagedh74 = dr.GetDecimal(iCpagedh74);

                    int iCpagedh75 = dr.GetOrdinal(helper.Cpagedh75);
                    if (!dr.IsDBNull(iCpagedh75)) entity.Cpagedh75 = dr.GetDecimal(iCpagedh75);

                    int iCpagedh76 = dr.GetOrdinal(helper.Cpagedh76);
                    if (!dr.IsDBNull(iCpagedh76)) entity.Cpagedh76 = dr.GetDecimal(iCpagedh76);

                    int iCpagedh77 = dr.GetOrdinal(helper.Cpagedh77);
                    if (!dr.IsDBNull(iCpagedh77)) entity.Cpagedh77 = dr.GetDecimal(iCpagedh77);

                    int iCpagedh78 = dr.GetOrdinal(helper.Cpagedh78);
                    if (!dr.IsDBNull(iCpagedh78)) entity.Cpagedh78 = dr.GetDecimal(iCpagedh78);

                    int iCpagedh79 = dr.GetOrdinal(helper.Cpagedh79);
                    if (!dr.IsDBNull(iCpagedh79)) entity.Cpagedh79 = dr.GetDecimal(iCpagedh79);

                    int iCpagedh80 = dr.GetOrdinal(helper.Cpagedh80);
                    if (!dr.IsDBNull(iCpagedh80)) entity.Cpagedh80 = dr.GetDecimal(iCpagedh80);

                    int iCpagedh81 = dr.GetOrdinal(helper.Cpagedh81);
                    if (!dr.IsDBNull(iCpagedh81)) entity.Cpagedh81 = dr.GetDecimal(iCpagedh81);

                    int iCpagedh82 = dr.GetOrdinal(helper.Cpagedh82);
                    if (!dr.IsDBNull(iCpagedh82)) entity.Cpagedh82 = dr.GetDecimal(iCpagedh82);

                    int iCpagedh83 = dr.GetOrdinal(helper.Cpagedh83);
                    if (!dr.IsDBNull(iCpagedh83)) entity.Cpagedh83 = dr.GetDecimal(iCpagedh83);

                    int iCpagedh84 = dr.GetOrdinal(helper.Cpagedh84);
                    if (!dr.IsDBNull(iCpagedh84)) entity.Cpagedh84 = dr.GetDecimal(iCpagedh84);

                    int iCpagedh85 = dr.GetOrdinal(helper.Cpagedh85);
                    if (!dr.IsDBNull(iCpagedh85)) entity.Cpagedh85 = dr.GetDecimal(iCpagedh85);

                    int iCpagedh86 = dr.GetOrdinal(helper.Cpagedh86);
                    if (!dr.IsDBNull(iCpagedh86)) entity.Cpagedh86 = dr.GetDecimal(iCpagedh86);

                    int iCpagedh87 = dr.GetOrdinal(helper.Cpagedh87);
                    if (!dr.IsDBNull(iCpagedh87)) entity.Cpagedh87 = dr.GetDecimal(iCpagedh87);

                    int iCpagedh88 = dr.GetOrdinal(helper.Cpagedh88);
                    if (!dr.IsDBNull(iCpagedh88)) entity.Cpagedh88 = dr.GetDecimal(iCpagedh88);

                    int iCpagedh89 = dr.GetOrdinal(helper.Cpagedh89);
                    if (!dr.IsDBNull(iCpagedh89)) entity.Cpagedh89 = dr.GetDecimal(iCpagedh89);

                    int iCpagedh90 = dr.GetOrdinal(helper.Cpagedh90);
                    if (!dr.IsDBNull(iCpagedh90)) entity.Cpagedh90 = dr.GetDecimal(iCpagedh90);

                    int iCpagedh91 = dr.GetOrdinal(helper.Cpagedh91);
                    if (!dr.IsDBNull(iCpagedh91)) entity.Cpagedh91 = dr.GetDecimal(iCpagedh91);

                    int iCpagedh92 = dr.GetOrdinal(helper.Cpagedh92);
                    if (!dr.IsDBNull(iCpagedh92)) entity.Cpagedh92 = dr.GetDecimal(iCpagedh92);

                    int iCpagedh93 = dr.GetOrdinal(helper.Cpagedh93);
                    if (!dr.IsDBNull(iCpagedh93)) entity.Cpagedh93 = dr.GetDecimal(iCpagedh93);

                    int iCpagedh94 = dr.GetOrdinal(helper.Cpagedh94);
                    if (!dr.IsDBNull(iCpagedh94)) entity.Cpagedh94 = dr.GetDecimal(iCpagedh94);

                    int iCpagedh95 = dr.GetOrdinal(helper.Cpagedh95);
                    if (!dr.IsDBNull(iCpagedh95)) entity.Cpagedh95 = dr.GetDecimal(iCpagedh95);

                    int iCpagedh96 = dr.GetOrdinal(helper.Cpagedh96);
                    if (!dr.IsDBNull(iCpagedh96)) entity.Cpagedh96 = dr.GetDecimal(iCpagedh96);

                    //CpaGercsvDetDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entities.Add(entity);
                }
            }

            return entities;
        }

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;
            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public void BulkInsertCpaGerCsvDet(List<CpaGercsvDetDTO> entitys, string nombreTabla)
        {
            dbProvider.AddColumnMapping(helper.Cpagedcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cpagercodi, DbType.Int32);

            dbProvider.AddColumnMapping(helper.Emprcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Equicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cpagedtipcsv, DbType.String);
            dbProvider.AddColumnMapping(helper.Cpagedfecha, DbType.DateTime);

            dbProvider.AddColumnMapping(helper.Cpagedh1, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh2, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh3, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh4, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh5, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh6, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh7, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh8, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh9, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh10, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh11, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh12, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh13, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh14, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh15, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh16, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh17, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh18, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh19, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh20, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh21, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh22, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh23, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh24, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh25, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh26, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh27, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh28, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh29, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh30, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh31, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh32, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh33, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh34, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh35, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh36, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh37, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh38, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh39, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh40, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh41, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh42, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh43, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh44, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh45, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh46, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh47, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh48, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh49, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh50, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh51, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh52, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh53, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh54, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh55, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh56, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh57, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh58, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh59, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh60, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh61, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh62, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh63, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh64, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh65, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh66, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh67, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh68, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh69, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh70, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh71, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh72, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh73, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh74, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh75, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh76, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh77, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh78, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh79, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh80, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh81, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh82, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh83, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh84, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh85, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh86, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh87, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh88, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh89, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh90, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh91, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh92, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh93, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh94, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh95, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedh96, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpagedusucreacion, DbType.String);
            dbProvider.AddColumnMapping(helper.Cpagedfeccreacion, DbType.DateTime);

            dbProvider.BulkInsert<CpaGercsvDetDTO>(entitys, nombreTabla);
        }

        //Funciones: CPA_GERCSVDET_TMP
        public void TruncateTmp()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlTruncateTmp);
            dbProvider.ExecuteNonQuery(command);
        }


    }
}