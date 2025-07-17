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
    /// Clase de acceso a datos de la tabla RER_GERCSV_DET
    /// </summary>
    public class RerGerCsvDetRepository : RepositoryBase, IRerGerCsvDetRepository
    {
        public RerGerCsvDetRepository(string strConn) : base(strConn)
        {
        }

        readonly RerGerCsvDetHelper helper = new RerGerCsvDetHelper();

        public int Save(RerGerCsvDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Regedcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Regercodi, DbType.Int32, entity.Regercodi);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Regedtipcsv, DbType.String, entity.Regedtipcsv);
            dbProvider.AddInParameter(command, helper.Regedfecha, DbType.DateTime, entity.Regedfecha);

            dbProvider.AddInParameter(command, helper.Regedh1, DbType.Decimal, entity.Regedh1);
            dbProvider.AddInParameter(command, helper.Regedh2, DbType.Decimal, entity.Regedh2);
            dbProvider.AddInParameter(command, helper.Regedh3, DbType.Decimal, entity.Regedh3);
            dbProvider.AddInParameter(command, helper.Regedh4, DbType.Decimal, entity.Regedh4);
            dbProvider.AddInParameter(command, helper.Regedh5, DbType.Decimal, entity.Regedh5);
            dbProvider.AddInParameter(command, helper.Regedh6, DbType.Decimal, entity.Regedh6);
            dbProvider.AddInParameter(command, helper.Regedh7, DbType.Decimal, entity.Regedh7);
            dbProvider.AddInParameter(command, helper.Regedh8, DbType.Decimal, entity.Regedh8);
            dbProvider.AddInParameter(command, helper.Regedh9, DbType.Decimal, entity.Regedh9);
            dbProvider.AddInParameter(command, helper.Regedh10, DbType.Decimal, entity.Regedh10);
            dbProvider.AddInParameter(command, helper.Regedh11, DbType.Decimal, entity.Regedh11);
            dbProvider.AddInParameter(command, helper.Regedh12, DbType.Decimal, entity.Regedh12);
            dbProvider.AddInParameter(command, helper.Regedh13, DbType.Decimal, entity.Regedh13);
            dbProvider.AddInParameter(command, helper.Regedh14, DbType.Decimal, entity.Regedh14);
            dbProvider.AddInParameter(command, helper.Regedh15, DbType.Decimal, entity.Regedh15);
            dbProvider.AddInParameter(command, helper.Regedh16, DbType.Decimal, entity.Regedh16);
            dbProvider.AddInParameter(command, helper.Regedh17, DbType.Decimal, entity.Regedh17);
            dbProvider.AddInParameter(command, helper.Regedh18, DbType.Decimal, entity.Regedh18);
            dbProvider.AddInParameter(command, helper.Regedh19, DbType.Decimal, entity.Regedh19);
            dbProvider.AddInParameter(command, helper.Regedh20, DbType.Decimal, entity.Regedh20);
            dbProvider.AddInParameter(command, helper.Regedh21, DbType.Decimal, entity.Regedh21);
            dbProvider.AddInParameter(command, helper.Regedh22, DbType.Decimal, entity.Regedh22);
            dbProvider.AddInParameter(command, helper.Regedh23, DbType.Decimal, entity.Regedh23);
            dbProvider.AddInParameter(command, helper.Regedh24, DbType.Decimal, entity.Regedh24);
            dbProvider.AddInParameter(command, helper.Regedh25, DbType.Decimal, entity.Regedh25);
            dbProvider.AddInParameter(command, helper.Regedh26, DbType.Decimal, entity.Regedh26);
            dbProvider.AddInParameter(command, helper.Regedh27, DbType.Decimal, entity.Regedh27);
            dbProvider.AddInParameter(command, helper.Regedh28, DbType.Decimal, entity.Regedh28);
            dbProvider.AddInParameter(command, helper.Regedh29, DbType.Decimal, entity.Regedh29);
            dbProvider.AddInParameter(command, helper.Regedh30, DbType.Decimal, entity.Regedh30);
            dbProvider.AddInParameter(command, helper.Regedh31, DbType.Decimal, entity.Regedh31);
            dbProvider.AddInParameter(command, helper.Regedh32, DbType.Decimal, entity.Regedh32);
            dbProvider.AddInParameter(command, helper.Regedh33, DbType.Decimal, entity.Regedh33);
            dbProvider.AddInParameter(command, helper.Regedh34, DbType.Decimal, entity.Regedh34);
            dbProvider.AddInParameter(command, helper.Regedh35, DbType.Decimal, entity.Regedh35);
            dbProvider.AddInParameter(command, helper.Regedh36, DbType.Decimal, entity.Regedh36);
            dbProvider.AddInParameter(command, helper.Regedh37, DbType.Decimal, entity.Regedh37);
            dbProvider.AddInParameter(command, helper.Regedh38, DbType.Decimal, entity.Regedh38);
            dbProvider.AddInParameter(command, helper.Regedh39, DbType.Decimal, entity.Regedh39);
            dbProvider.AddInParameter(command, helper.Regedh40, DbType.Decimal, entity.Regedh40);
            dbProvider.AddInParameter(command, helper.Regedh41, DbType.Decimal, entity.Regedh41);
            dbProvider.AddInParameter(command, helper.Regedh42, DbType.Decimal, entity.Regedh42);
            dbProvider.AddInParameter(command, helper.Regedh43, DbType.Decimal, entity.Regedh43);
            dbProvider.AddInParameter(command, helper.Regedh44, DbType.Decimal, entity.Regedh44);
            dbProvider.AddInParameter(command, helper.Regedh45, DbType.Decimal, entity.Regedh45);
            dbProvider.AddInParameter(command, helper.Regedh46, DbType.Decimal, entity.Regedh46);
            dbProvider.AddInParameter(command, helper.Regedh47, DbType.Decimal, entity.Regedh47);
            dbProvider.AddInParameter(command, helper.Regedh48, DbType.Decimal, entity.Regedh48);
            dbProvider.AddInParameter(command, helper.Regedh49, DbType.Decimal, entity.Regedh49);
            dbProvider.AddInParameter(command, helper.Regedh50, DbType.Decimal, entity.Regedh50);
            dbProvider.AddInParameter(command, helper.Regedh51, DbType.Decimal, entity.Regedh51);
            dbProvider.AddInParameter(command, helper.Regedh52, DbType.Decimal, entity.Regedh52);
            dbProvider.AddInParameter(command, helper.Regedh53, DbType.Decimal, entity.Regedh53);
            dbProvider.AddInParameter(command, helper.Regedh54, DbType.Decimal, entity.Regedh54);
            dbProvider.AddInParameter(command, helper.Regedh55, DbType.Decimal, entity.Regedh55);
            dbProvider.AddInParameter(command, helper.Regedh56, DbType.Decimal, entity.Regedh56);
            dbProvider.AddInParameter(command, helper.Regedh57, DbType.Decimal, entity.Regedh57);
            dbProvider.AddInParameter(command, helper.Regedh58, DbType.Decimal, entity.Regedh58);
            dbProvider.AddInParameter(command, helper.Regedh59, DbType.Decimal, entity.Regedh59);
            dbProvider.AddInParameter(command, helper.Regedh60, DbType.Decimal, entity.Regedh60);
            dbProvider.AddInParameter(command, helper.Regedh61, DbType.Decimal, entity.Regedh61);
            dbProvider.AddInParameter(command, helper.Regedh62, DbType.Decimal, entity.Regedh62);
            dbProvider.AddInParameter(command, helper.Regedh63, DbType.Decimal, entity.Regedh63);
            dbProvider.AddInParameter(command, helper.Regedh64, DbType.Decimal, entity.Regedh64);
            dbProvider.AddInParameter(command, helper.Regedh65, DbType.Decimal, entity.Regedh65);
            dbProvider.AddInParameter(command, helper.Regedh66, DbType.Decimal, entity.Regedh66);
            dbProvider.AddInParameter(command, helper.Regedh67, DbType.Decimal, entity.Regedh67);
            dbProvider.AddInParameter(command, helper.Regedh68, DbType.Decimal, entity.Regedh68);
            dbProvider.AddInParameter(command, helper.Regedh69, DbType.Decimal, entity.Regedh69);
            dbProvider.AddInParameter(command, helper.Regedh70, DbType.Decimal, entity.Regedh70);
            dbProvider.AddInParameter(command, helper.Regedh71, DbType.Decimal, entity.Regedh71);
            dbProvider.AddInParameter(command, helper.Regedh72, DbType.Decimal, entity.Regedh72);
            dbProvider.AddInParameter(command, helper.Regedh73, DbType.Decimal, entity.Regedh73);
            dbProvider.AddInParameter(command, helper.Regedh74, DbType.Decimal, entity.Regedh74);
            dbProvider.AddInParameter(command, helper.Regedh75, DbType.Decimal, entity.Regedh75);
            dbProvider.AddInParameter(command, helper.Regedh76, DbType.Decimal, entity.Regedh76);
            dbProvider.AddInParameter(command, helper.Regedh77, DbType.Decimal, entity.Regedh77);
            dbProvider.AddInParameter(command, helper.Regedh78, DbType.Decimal, entity.Regedh78);
            dbProvider.AddInParameter(command, helper.Regedh79, DbType.Decimal, entity.Regedh79);
            dbProvider.AddInParameter(command, helper.Regedh80, DbType.Decimal, entity.Regedh80);
            dbProvider.AddInParameter(command, helper.Regedh81, DbType.Decimal, entity.Regedh81);
            dbProvider.AddInParameter(command, helper.Regedh82, DbType.Decimal, entity.Regedh82);
            dbProvider.AddInParameter(command, helper.Regedh83, DbType.Decimal, entity.Regedh83);
            dbProvider.AddInParameter(command, helper.Regedh84, DbType.Decimal, entity.Regedh84);
            dbProvider.AddInParameter(command, helper.Regedh85, DbType.Decimal, entity.Regedh85);
            dbProvider.AddInParameter(command, helper.Regedh86, DbType.Decimal, entity.Regedh86);
            dbProvider.AddInParameter(command, helper.Regedh87, DbType.Decimal, entity.Regedh87);
            dbProvider.AddInParameter(command, helper.Regedh88, DbType.Decimal, entity.Regedh88);
            dbProvider.AddInParameter(command, helper.Regedh89, DbType.Decimal, entity.Regedh89);
            dbProvider.AddInParameter(command, helper.Regedh90, DbType.Decimal, entity.Regedh90);
            dbProvider.AddInParameter(command, helper.Regedh91, DbType.Decimal, entity.Regedh91);
            dbProvider.AddInParameter(command, helper.Regedh92, DbType.Decimal, entity.Regedh92);
            dbProvider.AddInParameter(command, helper.Regedh93, DbType.Decimal, entity.Regedh93);
            dbProvider.AddInParameter(command, helper.Regedh94, DbType.Decimal, entity.Regedh94);
            dbProvider.AddInParameter(command, helper.Regedh95, DbType.Decimal, entity.Regedh95);
            dbProvider.AddInParameter(command, helper.Regedh96, DbType.Decimal, entity.Regedh96);
            dbProvider.AddInParameter(command, helper.Regedusucreacion, DbType.String, entity.Regedusucreacion);
            dbProvider.AddInParameter(command, helper.Regedfeccreacion, DbType.DateTime, entity.Regedfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RerGerCsvDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Regercodi, DbType.Int32, entity.Regercodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Regedtipcsv, DbType.String, entity.Regedtipcsv);
            dbProvider.AddInParameter(command, helper.Regedfecha, DbType.DateTime, entity.Regedfecha);
            dbProvider.AddInParameter(command, helper.Regedh1, DbType.Decimal, entity.Regedh1);
            dbProvider.AddInParameter(command, helper.Regedh2, DbType.Decimal, entity.Regedh2);
            dbProvider.AddInParameter(command, helper.Regedh3, DbType.Decimal, entity.Regedh3);
            dbProvider.AddInParameter(command, helper.Regedh4, DbType.Decimal, entity.Regedh4);
            dbProvider.AddInParameter(command, helper.Regedh5, DbType.Decimal, entity.Regedh5);
            dbProvider.AddInParameter(command, helper.Regedh6, DbType.Decimal, entity.Regedh6);
            dbProvider.AddInParameter(command, helper.Regedh7, DbType.Decimal, entity.Regedh7);
            dbProvider.AddInParameter(command, helper.Regedh8, DbType.Decimal, entity.Regedh8);
            dbProvider.AddInParameter(command, helper.Regedh9, DbType.Decimal, entity.Regedh9);
            dbProvider.AddInParameter(command, helper.Regedh10, DbType.Decimal, entity.Regedh10);
            dbProvider.AddInParameter(command, helper.Regedh11, DbType.Decimal, entity.Regedh11);
            dbProvider.AddInParameter(command, helper.Regedh12, DbType.Decimal, entity.Regedh12);
            dbProvider.AddInParameter(command, helper.Regedh13, DbType.Decimal, entity.Regedh13);
            dbProvider.AddInParameter(command, helper.Regedh14, DbType.Decimal, entity.Regedh14);
            dbProvider.AddInParameter(command, helper.Regedh15, DbType.Decimal, entity.Regedh15);
            dbProvider.AddInParameter(command, helper.Regedh16, DbType.Decimal, entity.Regedh16);
            dbProvider.AddInParameter(command, helper.Regedh17, DbType.Decimal, entity.Regedh17);
            dbProvider.AddInParameter(command, helper.Regedh18, DbType.Decimal, entity.Regedh18);
            dbProvider.AddInParameter(command, helper.Regedh19, DbType.Decimal, entity.Regedh19);
            dbProvider.AddInParameter(command, helper.Regedh20, DbType.Decimal, entity.Regedh20);
            dbProvider.AddInParameter(command, helper.Regedh21, DbType.Decimal, entity.Regedh21);
            dbProvider.AddInParameter(command, helper.Regedh22, DbType.Decimal, entity.Regedh22);
            dbProvider.AddInParameter(command, helper.Regedh23, DbType.Decimal, entity.Regedh23);
            dbProvider.AddInParameter(command, helper.Regedh24, DbType.Decimal, entity.Regedh24);
            dbProvider.AddInParameter(command, helper.Regedh25, DbType.Decimal, entity.Regedh25);
            dbProvider.AddInParameter(command, helper.Regedh26, DbType.Decimal, entity.Regedh26);
            dbProvider.AddInParameter(command, helper.Regedh27, DbType.Decimal, entity.Regedh27);
            dbProvider.AddInParameter(command, helper.Regedh28, DbType.Decimal, entity.Regedh28);
            dbProvider.AddInParameter(command, helper.Regedh29, DbType.Decimal, entity.Regedh29);
            dbProvider.AddInParameter(command, helper.Regedh30, DbType.Decimal, entity.Regedh30);
            dbProvider.AddInParameter(command, helper.Regedh31, DbType.Decimal, entity.Regedh31);
            dbProvider.AddInParameter(command, helper.Regedh32, DbType.Decimal, entity.Regedh32);
            dbProvider.AddInParameter(command, helper.Regedh33, DbType.Decimal, entity.Regedh33);
            dbProvider.AddInParameter(command, helper.Regedh34, DbType.Decimal, entity.Regedh34);
            dbProvider.AddInParameter(command, helper.Regedh35, DbType.Decimal, entity.Regedh35);
            dbProvider.AddInParameter(command, helper.Regedh36, DbType.Decimal, entity.Regedh36);
            dbProvider.AddInParameter(command, helper.Regedh37, DbType.Decimal, entity.Regedh37);
            dbProvider.AddInParameter(command, helper.Regedh38, DbType.Decimal, entity.Regedh38);
            dbProvider.AddInParameter(command, helper.Regedh39, DbType.Decimal, entity.Regedh39);
            dbProvider.AddInParameter(command, helper.Regedh40, DbType.Decimal, entity.Regedh40);
            dbProvider.AddInParameter(command, helper.Regedh41, DbType.Decimal, entity.Regedh41);
            dbProvider.AddInParameter(command, helper.Regedh42, DbType.Decimal, entity.Regedh42);
            dbProvider.AddInParameter(command, helper.Regedh43, DbType.Decimal, entity.Regedh43);
            dbProvider.AddInParameter(command, helper.Regedh44, DbType.Decimal, entity.Regedh44);
            dbProvider.AddInParameter(command, helper.Regedh45, DbType.Decimal, entity.Regedh45);
            dbProvider.AddInParameter(command, helper.Regedh46, DbType.Decimal, entity.Regedh46);
            dbProvider.AddInParameter(command, helper.Regedh47, DbType.Decimal, entity.Regedh47);
            dbProvider.AddInParameter(command, helper.Regedh48, DbType.Decimal, entity.Regedh48);
            dbProvider.AddInParameter(command, helper.Regedh49, DbType.Decimal, entity.Regedh49);
            dbProvider.AddInParameter(command, helper.Regedh50, DbType.Decimal, entity.Regedh50);
            dbProvider.AddInParameter(command, helper.Regedh51, DbType.Decimal, entity.Regedh51);
            dbProvider.AddInParameter(command, helper.Regedh52, DbType.Decimal, entity.Regedh52);
            dbProvider.AddInParameter(command, helper.Regedh53, DbType.Decimal, entity.Regedh53);
            dbProvider.AddInParameter(command, helper.Regedh54, DbType.Decimal, entity.Regedh54);
            dbProvider.AddInParameter(command, helper.Regedh55, DbType.Decimal, entity.Regedh55);
            dbProvider.AddInParameter(command, helper.Regedh56, DbType.Decimal, entity.Regedh56);
            dbProvider.AddInParameter(command, helper.Regedh57, DbType.Decimal, entity.Regedh57);
            dbProvider.AddInParameter(command, helper.Regedh58, DbType.Decimal, entity.Regedh58);
            dbProvider.AddInParameter(command, helper.Regedh59, DbType.Decimal, entity.Regedh59);
            dbProvider.AddInParameter(command, helper.Regedh60, DbType.Decimal, entity.Regedh60);
            dbProvider.AddInParameter(command, helper.Regedh61, DbType.Decimal, entity.Regedh61);
            dbProvider.AddInParameter(command, helper.Regedh62, DbType.Decimal, entity.Regedh62);
            dbProvider.AddInParameter(command, helper.Regedh63, DbType.Decimal, entity.Regedh63);
            dbProvider.AddInParameter(command, helper.Regedh64, DbType.Decimal, entity.Regedh64);
            dbProvider.AddInParameter(command, helper.Regedh65, DbType.Decimal, entity.Regedh65);
            dbProvider.AddInParameter(command, helper.Regedh66, DbType.Decimal, entity.Regedh66);
            dbProvider.AddInParameter(command, helper.Regedh67, DbType.Decimal, entity.Regedh67);
            dbProvider.AddInParameter(command, helper.Regedh68, DbType.Decimal, entity.Regedh68);
            dbProvider.AddInParameter(command, helper.Regedh69, DbType.Decimal, entity.Regedh69);
            dbProvider.AddInParameter(command, helper.Regedh70, DbType.Decimal, entity.Regedh70);
            dbProvider.AddInParameter(command, helper.Regedh71, DbType.Decimal, entity.Regedh71);
            dbProvider.AddInParameter(command, helper.Regedh72, DbType.Decimal, entity.Regedh72);
            dbProvider.AddInParameter(command, helper.Regedh73, DbType.Decimal, entity.Regedh73);
            dbProvider.AddInParameter(command, helper.Regedh74, DbType.Decimal, entity.Regedh74);
            dbProvider.AddInParameter(command, helper.Regedh75, DbType.Decimal, entity.Regedh75);
            dbProvider.AddInParameter(command, helper.Regedh76, DbType.Decimal, entity.Regedh76);
            dbProvider.AddInParameter(command, helper.Regedh77, DbType.Decimal, entity.Regedh77);
            dbProvider.AddInParameter(command, helper.Regedh78, DbType.Decimal, entity.Regedh78);
            dbProvider.AddInParameter(command, helper.Regedh79, DbType.Decimal, entity.Regedh79);
            dbProvider.AddInParameter(command, helper.Regedh80, DbType.Decimal, entity.Regedh80);
            dbProvider.AddInParameter(command, helper.Regedh81, DbType.Decimal, entity.Regedh81);
            dbProvider.AddInParameter(command, helper.Regedh82, DbType.Decimal, entity.Regedh82);
            dbProvider.AddInParameter(command, helper.Regedh83, DbType.Decimal, entity.Regedh83);
            dbProvider.AddInParameter(command, helper.Regedh84, DbType.Decimal, entity.Regedh84);
            dbProvider.AddInParameter(command, helper.Regedh85, DbType.Decimal, entity.Regedh85);
            dbProvider.AddInParameter(command, helper.Regedh86, DbType.Decimal, entity.Regedh86);
            dbProvider.AddInParameter(command, helper.Regedh87, DbType.Decimal, entity.Regedh87);
            dbProvider.AddInParameter(command, helper.Regedh88, DbType.Decimal, entity.Regedh88);
            dbProvider.AddInParameter(command, helper.Regedh89, DbType.Decimal, entity.Regedh89);
            dbProvider.AddInParameter(command, helper.Regedh90, DbType.Decimal, entity.Regedh90);
            dbProvider.AddInParameter(command, helper.Regedh91, DbType.Decimal, entity.Regedh91);
            dbProvider.AddInParameter(command, helper.Regedh92, DbType.Decimal, entity.Regedh92);
            dbProvider.AddInParameter(command, helper.Regedh93, DbType.Decimal, entity.Regedh93);
            dbProvider.AddInParameter(command, helper.Regedh94, DbType.Decimal, entity.Regedh94);
            dbProvider.AddInParameter(command, helper.Regedh95, DbType.Decimal, entity.Regedh95);
            dbProvider.AddInParameter(command, helper.Regedh96, DbType.Decimal, entity.Regedh96);
            dbProvider.AddInParameter(command, helper.Regedusucreacion, DbType.String, entity.Regedusucreacion);
            dbProvider.AddInParameter(command, helper.Regedfeccreacion, DbType.DateTime, entity.Regedfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rerGerCsvDetId)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Regedcodi, DbType.Int32, rerGerCsvDetId);

            dbProvider.ExecuteNonQuery(command);
        }

        public RerGerCsvDetDTO GetById(int rerGerCsvDetId)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Regedcodi, DbType.Int32, rerGerCsvDetId);
            RerGerCsvDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RerGerCsvDetDTO> List()
        {
            List<RerGerCsvDetDTO> entitys = new List<RerGerCsvDetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerGerCsvDetDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RerGerCsvDetDTO> GetByEmprcodiEquicodiTipo(int emprcodi, int equicodi, string regedtipcsv, int anioTarifario)
        {
            string query = string.Format(helper.SqlListByEmprcodiEquicodiTipo, emprcodi, equicodi, regedtipcsv, anioTarifario);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerGerCsvDetDTO> entities = new List<RerGerCsvDetDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerGerCsvDetDTO entity = helper.Create(dr);

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

        public void BulkInsertRerGerCsvDet(List<RerGerCsvDetDTO> entitys, string nombreTabla)
        {
            dbProvider.AddColumnMapping(helper.Regedcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Regercodi, DbType.Int32);

            dbProvider.AddColumnMapping(helper.Emprcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Equicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Regedtipcsv, DbType.String);
            dbProvider.AddColumnMapping(helper.Regedfecha, DbType.DateTime);

            dbProvider.AddColumnMapping(helper.Regedh1, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh2, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh3, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh4, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh5, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh6, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh7, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh8, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh9, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh10, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh11, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh12, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh13, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh14, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh15, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh16, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh17, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh18, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh19, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh20, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh21, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh22, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh23, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh24, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh25, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh26, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh27, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh28, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh29, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh30, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh31, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh32, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh33, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh34, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh35, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh36, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh37, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh38, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh39, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh40, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh41, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh42, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh43, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh44, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh45, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh46, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh47, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh48, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh49, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh50, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh51, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh52, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh53, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh54, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh55, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh56, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh57, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh58, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh59, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh60, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh61, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh62, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh63, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh64, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh65, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh66, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh67, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh68, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh69, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh70, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh71, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh72, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh73, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh74, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh75, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh76, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh77, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh78, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh79, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh80, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh81, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh82, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh83, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh84, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh85, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh86, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh87, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh88, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh89, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh90, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh91, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh92, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh93, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh94, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh95, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedh96, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Regedusucreacion, DbType.String);
            dbProvider.AddColumnMapping(helper.Regedfeccreacion, DbType.DateTime);

            dbProvider.BulkInsert<RerGerCsvDetDTO>(entitys, nombreTabla);
        }

        public List<RerGerCsvDetDTO> ListByEquipo(int equicodi, DateTime fechainicio, DateTime fechafin)
        {
            List<RerGerCsvDetDTO> entitys = new List<RerGerCsvDetDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByEquipo);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Regedfecha, DbType.DateTime, fechainicio);
            dbProvider.AddInParameter(command, helper.Regedfecha, DbType.DateTime, fechafin);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerGerCsvDetDTO entity = new RerGerCsvDetDTO();

                    int iRegercodi = dr.GetOrdinal(this.helper.Regercodi);
                    if (!dr.IsDBNull(iRegercodi)) entity.Regercodi = Convert.ToInt32(dr.GetValue(iRegercodi));

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iRegedfecha = dr.GetOrdinal(this.helper.Regedfecha);
                    if (!dr.IsDBNull(iRegedfecha)) entity.Regedfecha = dr.GetDateTime(iRegedfecha);

                    int iRegedh1 = dr.GetOrdinal(this.helper.Regedh1);
                    if (!dr.IsDBNull(iRegedh1)) entity.Regedh1 = dr.GetDecimal(iRegedh1);

                    int iRegedh2 = dr.GetOrdinal(this.helper.Regedh2);
                    if (!dr.IsDBNull(iRegedh2)) entity.Regedh2 = dr.GetDecimal(iRegedh2);

                    int iRegedh3 = dr.GetOrdinal(this.helper.Regedh3);
                    if (!dr.IsDBNull(iRegedh3)) entity.Regedh3 = dr.GetDecimal(iRegedh3);

                    int iRegedh4 = dr.GetOrdinal(this.helper.Regedh4);
                    if (!dr.IsDBNull(iRegedh4)) entity.Regedh4 = dr.GetDecimal(iRegedh4);

                    int iRegedh5 = dr.GetOrdinal(this.helper.Regedh5);
                    if (!dr.IsDBNull(iRegedh5)) entity.Regedh5 = dr.GetDecimal(iRegedh5);

                    int iRegedh6 = dr.GetOrdinal(this.helper.Regedh6);
                    if (!dr.IsDBNull(iRegedh6)) entity.Regedh6 = dr.GetDecimal(iRegedh6);

                    int iRegedh7 = dr.GetOrdinal(this.helper.Regedh7);
                    if (!dr.IsDBNull(iRegedh7)) entity.Regedh7 = dr.GetDecimal(iRegedh7);

                    int iRegedh8 = dr.GetOrdinal(this.helper.Regedh8);
                    if (!dr.IsDBNull(iRegedh8)) entity.Regedh8 = dr.GetDecimal(iRegedh8);

                    int iRegedh9 = dr.GetOrdinal(this.helper.Regedh9);
                    if (!dr.IsDBNull(iRegedh9)) entity.Regedh9 = dr.GetDecimal(iRegedh9);

                    int iRegedh10 = dr.GetOrdinal(this.helper.Regedh10);
                    if (!dr.IsDBNull(iRegedh10)) entity.Regedh10 = dr.GetDecimal(iRegedh10);

                    int iRegedh11 = dr.GetOrdinal(this.helper.Regedh11);
                    if (!dr.IsDBNull(iRegedh11)) entity.Regedh11 = dr.GetDecimal(iRegedh11);

                    int iRegedh12 = dr.GetOrdinal(this.helper.Regedh12);
                    if (!dr.IsDBNull(iRegedh12)) entity.Regedh12 = dr.GetDecimal(iRegedh12);

                    int iRegedh13 = dr.GetOrdinal(this.helper.Regedh13);
                    if (!dr.IsDBNull(iRegedh13)) entity.Regedh13 = dr.GetDecimal(iRegedh13);

                    int iRegedh14 = dr.GetOrdinal(this.helper.Regedh14);
                    if (!dr.IsDBNull(iRegedh14)) entity.Regedh14 = dr.GetDecimal(iRegedh14);

                    int iRegedh15 = dr.GetOrdinal(this.helper.Regedh15);
                    if (!dr.IsDBNull(iRegedh15)) entity.Regedh15 = dr.GetDecimal(iRegedh15);

                    int iRegedh16 = dr.GetOrdinal(this.helper.Regedh16);
                    if (!dr.IsDBNull(iRegedh16)) entity.Regedh16 = dr.GetDecimal(iRegedh16);

                    int iRegedh17 = dr.GetOrdinal(this.helper.Regedh17);
                    if (!dr.IsDBNull(iRegedh17)) entity.Regedh17 = dr.GetDecimal(iRegedh17);

                    int iRegedh18 = dr.GetOrdinal(this.helper.Regedh18);
                    if (!dr.IsDBNull(iRegedh18)) entity.Regedh18 = dr.GetDecimal(iRegedh18);

                    int iRegedh19 = dr.GetOrdinal(this.helper.Regedh19);
                    if (!dr.IsDBNull(iRegedh19)) entity.Regedh19 = dr.GetDecimal(iRegedh19);

                    int iRegedh20 = dr.GetOrdinal(this.helper.Regedh20);
                    if (!dr.IsDBNull(iRegedh20)) entity.Regedh20 = dr.GetDecimal(iRegedh20);

                    int iRegedh21 = dr.GetOrdinal(this.helper.Regedh21);
                    if (!dr.IsDBNull(iRegedh21)) entity.Regedh21 = dr.GetDecimal(iRegedh21);

                    int iRegedh22 = dr.GetOrdinal(this.helper.Regedh22);
                    if (!dr.IsDBNull(iRegedh22)) entity.Regedh22 = dr.GetDecimal(iRegedh22);

                    int iRegedh23 = dr.GetOrdinal(this.helper.Regedh23);
                    if (!dr.IsDBNull(iRegedh23)) entity.Regedh23 = dr.GetDecimal(iRegedh23);

                    int iRegedh24 = dr.GetOrdinal(this.helper.Regedh24);
                    if (!dr.IsDBNull(iRegedh24)) entity.Regedh24 = dr.GetDecimal(iRegedh24);

                    int iRegedh25 = dr.GetOrdinal(this.helper.Regedh25);
                    if (!dr.IsDBNull(iRegedh25)) entity.Regedh25 = dr.GetDecimal(iRegedh25);

                    int iRegedh26 = dr.GetOrdinal(this.helper.Regedh26);
                    if (!dr.IsDBNull(iRegedh26)) entity.Regedh26 = dr.GetDecimal(iRegedh26);

                    int iRegedh27 = dr.GetOrdinal(this.helper.Regedh27);
                    if (!dr.IsDBNull(iRegedh27)) entity.Regedh27 = dr.GetDecimal(iRegedh27);

                    int iRegedh28 = dr.GetOrdinal(this.helper.Regedh28);
                    if (!dr.IsDBNull(iRegedh28)) entity.Regedh28 = dr.GetDecimal(iRegedh28);

                    int iRegedh29 = dr.GetOrdinal(this.helper.Regedh29);
                    if (!dr.IsDBNull(iRegedh29)) entity.Regedh29 = dr.GetDecimal(iRegedh29);

                    int iRegedh30 = dr.GetOrdinal(this.helper.Regedh30);
                    if (!dr.IsDBNull(iRegedh30)) entity.Regedh30 = dr.GetDecimal(iRegedh30);

                    int iRegedh31 = dr.GetOrdinal(this.helper.Regedh31);
                    if (!dr.IsDBNull(iRegedh31)) entity.Regedh31 = dr.GetDecimal(iRegedh31);

                    int iRegedh32 = dr.GetOrdinal(this.helper.Regedh32);
                    if (!dr.IsDBNull(iRegedh32)) entity.Regedh32 = dr.GetDecimal(iRegedh32);

                    int iRegedh33 = dr.GetOrdinal(this.helper.Regedh33);
                    if (!dr.IsDBNull(iRegedh33)) entity.Regedh33 = dr.GetDecimal(iRegedh33);

                    int iRegedh34 = dr.GetOrdinal(this.helper.Regedh34);
                    if (!dr.IsDBNull(iRegedh34)) entity.Regedh34 = dr.GetDecimal(iRegedh34);

                    int iRegedh35 = dr.GetOrdinal(this.helper.Regedh35);
                    if (!dr.IsDBNull(iRegedh35)) entity.Regedh35 = dr.GetDecimal(iRegedh35);

                    int iRegedh36 = dr.GetOrdinal(this.helper.Regedh36);
                    if (!dr.IsDBNull(iRegedh36)) entity.Regedh36 = dr.GetDecimal(iRegedh36);

                    int iRegedh37 = dr.GetOrdinal(this.helper.Regedh37);
                    if (!dr.IsDBNull(iRegedh37)) entity.Regedh37 = dr.GetDecimal(iRegedh37);

                    int iRegedh38 = dr.GetOrdinal(this.helper.Regedh38);
                    if (!dr.IsDBNull(iRegedh38)) entity.Regedh38 = dr.GetDecimal(iRegedh38);

                    int iRegedh39 = dr.GetOrdinal(this.helper.Regedh39);
                    if (!dr.IsDBNull(iRegedh39)) entity.Regedh39 = dr.GetDecimal(iRegedh39);

                    int iRegedh40 = dr.GetOrdinal(this.helper.Regedh40);
                    if (!dr.IsDBNull(iRegedh40)) entity.Regedh40 = dr.GetDecimal(iRegedh40);

                    int iRegedh41 = dr.GetOrdinal(this.helper.Regedh41);
                    if (!dr.IsDBNull(iRegedh41)) entity.Regedh41 = dr.GetDecimal(iRegedh41);

                    int iRegedh42 = dr.GetOrdinal(this.helper.Regedh42);
                    if (!dr.IsDBNull(iRegedh42)) entity.Regedh42 = dr.GetDecimal(iRegedh42);

                    int iRegedh43 = dr.GetOrdinal(this.helper.Regedh43);
                    if (!dr.IsDBNull(iRegedh43)) entity.Regedh43 = dr.GetDecimal(iRegedh43);

                    int iRegedh44 = dr.GetOrdinal(this.helper.Regedh44);
                    if (!dr.IsDBNull(iRegedh44)) entity.Regedh44 = dr.GetDecimal(iRegedh44);

                    int iRegedh45 = dr.GetOrdinal(this.helper.Regedh45);
                    if (!dr.IsDBNull(iRegedh45)) entity.Regedh45 = dr.GetDecimal(iRegedh45);

                    int iRegedh46 = dr.GetOrdinal(this.helper.Regedh46);
                    if (!dr.IsDBNull(iRegedh46)) entity.Regedh46 = dr.GetDecimal(iRegedh46);

                    int iRegedh47 = dr.GetOrdinal(this.helper.Regedh47);
                    if (!dr.IsDBNull(iRegedh47)) entity.Regedh47 = dr.GetDecimal(iRegedh47);

                    int iRegedh48 = dr.GetOrdinal(this.helper.Regedh48);
                    if (!dr.IsDBNull(iRegedh48)) entity.Regedh48 = dr.GetDecimal(iRegedh48);

                    int iRegedh49 = dr.GetOrdinal(this.helper.Regedh49);
                    if (!dr.IsDBNull(iRegedh49)) entity.Regedh49 = dr.GetDecimal(iRegedh49);

                    int iRegedh50 = dr.GetOrdinal(this.helper.Regedh50);
                    if (!dr.IsDBNull(iRegedh50)) entity.Regedh50 = dr.GetDecimal(iRegedh50);

                    int iRegedh51 = dr.GetOrdinal(this.helper.Regedh51);
                    if (!dr.IsDBNull(iRegedh51)) entity.Regedh51 = dr.GetDecimal(iRegedh51);

                    int iRegedh52 = dr.GetOrdinal(this.helper.Regedh52);
                    if (!dr.IsDBNull(iRegedh52)) entity.Regedh52 = dr.GetDecimal(iRegedh52);

                    int iRegedh53 = dr.GetOrdinal(this.helper.Regedh53);
                    if (!dr.IsDBNull(iRegedh53)) entity.Regedh53 = dr.GetDecimal(iRegedh53);

                    int iRegedh54 = dr.GetOrdinal(this.helper.Regedh54);
                    if (!dr.IsDBNull(iRegedh54)) entity.Regedh54 = dr.GetDecimal(iRegedh54);

                    int iRegedh55 = dr.GetOrdinal(this.helper.Regedh55);
                    if (!dr.IsDBNull(iRegedh55)) entity.Regedh55 = dr.GetDecimal(iRegedh55);

                    int iRegedh56 = dr.GetOrdinal(this.helper.Regedh56);
                    if (!dr.IsDBNull(iRegedh56)) entity.Regedh56 = dr.GetDecimal(iRegedh56);

                    int iRegedh57 = dr.GetOrdinal(this.helper.Regedh57);
                    if (!dr.IsDBNull(iRegedh57)) entity.Regedh57 = dr.GetDecimal(iRegedh57);

                    int iRegedh58 = dr.GetOrdinal(this.helper.Regedh58);
                    if (!dr.IsDBNull(iRegedh58)) entity.Regedh58 = dr.GetDecimal(iRegedh58);

                    int iRegedh59 = dr.GetOrdinal(this.helper.Regedh59);
                    if (!dr.IsDBNull(iRegedh59)) entity.Regedh59 = dr.GetDecimal(iRegedh59);

                    int iRegedh60 = dr.GetOrdinal(this.helper.Regedh60);
                    if (!dr.IsDBNull(iRegedh60)) entity.Regedh60 = dr.GetDecimal(iRegedh60);

                    int iRegedh61 = dr.GetOrdinal(this.helper.Regedh61);
                    if (!dr.IsDBNull(iRegedh61)) entity.Regedh61 = dr.GetDecimal(iRegedh61);

                    int iRegedh62 = dr.GetOrdinal(this.helper.Regedh62);
                    if (!dr.IsDBNull(iRegedh62)) entity.Regedh62 = dr.GetDecimal(iRegedh62);

                    int iRegedh63 = dr.GetOrdinal(this.helper.Regedh63);
                    if (!dr.IsDBNull(iRegedh63)) entity.Regedh63 = dr.GetDecimal(iRegedh63);

                    int iRegedh64 = dr.GetOrdinal(this.helper.Regedh64);
                    if (!dr.IsDBNull(iRegedh64)) entity.Regedh64 = dr.GetDecimal(iRegedh64);

                    int iRegedh65 = dr.GetOrdinal(this.helper.Regedh65);
                    if (!dr.IsDBNull(iRegedh65)) entity.Regedh65 = dr.GetDecimal(iRegedh65);

                    int iRegedh66 = dr.GetOrdinal(this.helper.Regedh66);
                    if (!dr.IsDBNull(iRegedh66)) entity.Regedh66 = dr.GetDecimal(iRegedh66);

                    int iRegedh67 = dr.GetOrdinal(this.helper.Regedh67);
                    if (!dr.IsDBNull(iRegedh67)) entity.Regedh67 = dr.GetDecimal(iRegedh67);

                    int iRegedh68 = dr.GetOrdinal(this.helper.Regedh68);
                    if (!dr.IsDBNull(iRegedh68)) entity.Regedh68 = dr.GetDecimal(iRegedh68);

                    int iRegedh69 = dr.GetOrdinal(this.helper.Regedh69);
                    if (!dr.IsDBNull(iRegedh69)) entity.Regedh69 = dr.GetDecimal(iRegedh69);

                    int iRegedh70 = dr.GetOrdinal(this.helper.Regedh70);
                    if (!dr.IsDBNull(iRegedh70)) entity.Regedh70 = dr.GetDecimal(iRegedh70);

                    int iRegedh71 = dr.GetOrdinal(this.helper.Regedh71);
                    if (!dr.IsDBNull(iRegedh71)) entity.Regedh71 = dr.GetDecimal(iRegedh71);

                    int iRegedh72 = dr.GetOrdinal(this.helper.Regedh72);
                    if (!dr.IsDBNull(iRegedh72)) entity.Regedh72 = dr.GetDecimal(iRegedh72);

                    int iRegedh73 = dr.GetOrdinal(this.helper.Regedh73);
                    if (!dr.IsDBNull(iRegedh73)) entity.Regedh73 = dr.GetDecimal(iRegedh73);

                    int iRegedh74 = dr.GetOrdinal(this.helper.Regedh74);
                    if (!dr.IsDBNull(iRegedh74)) entity.Regedh74 = dr.GetDecimal(iRegedh74);

                    int iRegedh75 = dr.GetOrdinal(this.helper.Regedh75);
                    if (!dr.IsDBNull(iRegedh75)) entity.Regedh75 = dr.GetDecimal(iRegedh75);

                    int iRegedh76 = dr.GetOrdinal(this.helper.Regedh76);
                    if (!dr.IsDBNull(iRegedh76)) entity.Regedh76 = dr.GetDecimal(iRegedh76);

                    int iRegedh77 = dr.GetOrdinal(this.helper.Regedh77);
                    if (!dr.IsDBNull(iRegedh77)) entity.Regedh77 = dr.GetDecimal(iRegedh77);

                    int iRegedh78 = dr.GetOrdinal(this.helper.Regedh78);
                    if (!dr.IsDBNull(iRegedh78)) entity.Regedh78 = dr.GetDecimal(iRegedh78);

                    int iRegedh79 = dr.GetOrdinal(this.helper.Regedh79);
                    if (!dr.IsDBNull(iRegedh79)) entity.Regedh79 = dr.GetDecimal(iRegedh79);

                    int iRegedh80 = dr.GetOrdinal(this.helper.Regedh80);
                    if (!dr.IsDBNull(iRegedh80)) entity.Regedh80 = dr.GetDecimal(iRegedh80);

                    int iRegedh81 = dr.GetOrdinal(this.helper.Regedh81);
                    if (!dr.IsDBNull(iRegedh81)) entity.Regedh81 = dr.GetDecimal(iRegedh81);

                    int iRegedh82 = dr.GetOrdinal(this.helper.Regedh82);
                    if (!dr.IsDBNull(iRegedh82)) entity.Regedh82 = dr.GetDecimal(iRegedh82);

                    int iRegedh83 = dr.GetOrdinal(this.helper.Regedh83);
                    if (!dr.IsDBNull(iRegedh83)) entity.Regedh83 = dr.GetDecimal(iRegedh83);

                    int iRegedh84 = dr.GetOrdinal(this.helper.Regedh84);
                    if (!dr.IsDBNull(iRegedh84)) entity.Regedh84 = dr.GetDecimal(iRegedh84);

                    int iRegedh85 = dr.GetOrdinal(this.helper.Regedh85);
                    if (!dr.IsDBNull(iRegedh85)) entity.Regedh85 = dr.GetDecimal(iRegedh85);

                    int iRegedh86 = dr.GetOrdinal(this.helper.Regedh86);
                    if (!dr.IsDBNull(iRegedh86)) entity.Regedh86 = dr.GetDecimal(iRegedh86);

                    int iRegedh87 = dr.GetOrdinal(this.helper.Regedh87);
                    if (!dr.IsDBNull(iRegedh87)) entity.Regedh87 = dr.GetDecimal(iRegedh87);

                    int iRegedh88 = dr.GetOrdinal(this.helper.Regedh88);
                    if (!dr.IsDBNull(iRegedh88)) entity.Regedh88 = dr.GetDecimal(iRegedh88);

                    int iRegedh89 = dr.GetOrdinal(this.helper.Regedh89);
                    if (!dr.IsDBNull(iRegedh89)) entity.Regedh89 = dr.GetDecimal(iRegedh89);

                    int iRegedh90 = dr.GetOrdinal(this.helper.Regedh90);
                    if (!dr.IsDBNull(iRegedh90)) entity.Regedh90 = dr.GetDecimal(iRegedh90);

                    int iRegedh91 = dr.GetOrdinal(this.helper.Regedh91);
                    if (!dr.IsDBNull(iRegedh91)) entity.Regedh91 = dr.GetDecimal(iRegedh91);

                    int iRegedh92 = dr.GetOrdinal(this.helper.Regedh92);
                    if (!dr.IsDBNull(iRegedh92)) entity.Regedh92 = dr.GetDecimal(iRegedh92);

                    int iRegedh93 = dr.GetOrdinal(this.helper.Regedh93);
                    if (!dr.IsDBNull(iRegedh93)) entity.Regedh93 = dr.GetDecimal(iRegedh93);

                    int iRegedh94 = dr.GetOrdinal(this.helper.Regedh94);
                    if (!dr.IsDBNull(iRegedh94)) entity.Regedh94 = dr.GetDecimal(iRegedh94);

                    int iRegedh95 = dr.GetOrdinal(this.helper.Regedh95);
                    if (!dr.IsDBNull(iRegedh95)) entity.Regedh95 = dr.GetDecimal(iRegedh95);

                    int iRegedh96 = dr.GetOrdinal(this.helper.Regedh96);
                    if (!dr.IsDBNull(iRegedh96)) entity.Regedh96 = dr.GetDecimal(iRegedh96);
                    
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}

