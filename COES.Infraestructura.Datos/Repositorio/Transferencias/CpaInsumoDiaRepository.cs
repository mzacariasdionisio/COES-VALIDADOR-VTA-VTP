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
    /// Clase de acceso a datos de la tabla CPA_INSUMO_DIA
    /// </summary>
    public class CpaInsumoDiaRepository : RepositoryBase, ICpaInsumoDiaRepository
    {
        public CpaInsumoDiaRepository(string strConn)
            : base(strConn)
        {
        }

        CpaInsumoDiaHelper helper = new CpaInsumoDiaHelper();

        public int Save(CpaInsumoDiaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpaindcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpainmcodi, DbType.Int32, entity.Cpainmcodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Cpaindtipinsumo, DbType.String, entity.Cpaindtipinsumo);
            dbProvider.AddInParameter(command, helper.Cpaindtipproceso, DbType.String, entity.Cpaindtipproceso);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Cpainddia, DbType.DateTime, entity.Cpainddia);
            dbProvider.AddInParameter(command, helper.Cpaindtotaldia, DbType.Decimal, entity.Cpaindtotaldia);
            dbProvider.AddInParameter(command, helper.Cpaindh1, DbType.Decimal, entity.Cpaindh1);
            dbProvider.AddInParameter(command, helper.Cpaindh2, DbType.Decimal, entity.Cpaindh2);
            dbProvider.AddInParameter(command, helper.Cpaindh3, DbType.Decimal, entity.Cpaindh3);
            dbProvider.AddInParameter(command, helper.Cpaindh4, DbType.Decimal, entity.Cpaindh4);
            dbProvider.AddInParameter(command, helper.Cpaindh5, DbType.Decimal, entity.Cpaindh5);
            dbProvider.AddInParameter(command, helper.Cpaindh6, DbType.Decimal, entity.Cpaindh6);
            dbProvider.AddInParameter(command, helper.Cpaindh7, DbType.Decimal, entity.Cpaindh7);
            dbProvider.AddInParameter(command, helper.Cpaindh8, DbType.Decimal, entity.Cpaindh8);
            dbProvider.AddInParameter(command, helper.Cpaindh9, DbType.Decimal, entity.Cpaindh9);
            dbProvider.AddInParameter(command, helper.Cpaindh10, DbType.Decimal, entity.Cpaindh10);
            dbProvider.AddInParameter(command, helper.Cpaindh11, DbType.Decimal, entity.Cpaindh11);
            dbProvider.AddInParameter(command, helper.Cpaindh12, DbType.Decimal, entity.Cpaindh12);
            dbProvider.AddInParameter(command, helper.Cpaindh13, DbType.Decimal, entity.Cpaindh13);
            dbProvider.AddInParameter(command, helper.Cpaindh14, DbType.Decimal, entity.Cpaindh14);
            dbProvider.AddInParameter(command, helper.Cpaindh15, DbType.Decimal, entity.Cpaindh15);
            dbProvider.AddInParameter(command, helper.Cpaindh16, DbType.Decimal, entity.Cpaindh16);
            dbProvider.AddInParameter(command, helper.Cpaindh17, DbType.Decimal, entity.Cpaindh17);
            dbProvider.AddInParameter(command, helper.Cpaindh18, DbType.Decimal, entity.Cpaindh18);
            dbProvider.AddInParameter(command, helper.Cpaindh19, DbType.Decimal, entity.Cpaindh19);
            dbProvider.AddInParameter(command, helper.Cpaindh20, DbType.Decimal, entity.Cpaindh20);
            dbProvider.AddInParameter(command, helper.Cpaindh21, DbType.Decimal, entity.Cpaindh21);
            dbProvider.AddInParameter(command, helper.Cpaindh22, DbType.Decimal, entity.Cpaindh22);
            dbProvider.AddInParameter(command, helper.Cpaindh23, DbType.Decimal, entity.Cpaindh23);
            dbProvider.AddInParameter(command, helper.Cpaindh24, DbType.Decimal, entity.Cpaindh24);
            dbProvider.AddInParameter(command, helper.Cpaindh25, DbType.Decimal, entity.Cpaindh25);
            dbProvider.AddInParameter(command, helper.Cpaindh26, DbType.Decimal, entity.Cpaindh26);
            dbProvider.AddInParameter(command, helper.Cpaindh27, DbType.Decimal, entity.Cpaindh27);
            dbProvider.AddInParameter(command, helper.Cpaindh28, DbType.Decimal, entity.Cpaindh28);
            dbProvider.AddInParameter(command, helper.Cpaindh29, DbType.Decimal, entity.Cpaindh29);
            dbProvider.AddInParameter(command, helper.Cpaindh30, DbType.Decimal, entity.Cpaindh30);
            dbProvider.AddInParameter(command, helper.Cpaindh31, DbType.Decimal, entity.Cpaindh31);
            dbProvider.AddInParameter(command, helper.Cpaindh32, DbType.Decimal, entity.Cpaindh32);
            dbProvider.AddInParameter(command, helper.Cpaindh33, DbType.Decimal, entity.Cpaindh33);
            dbProvider.AddInParameter(command, helper.Cpaindh34, DbType.Decimal, entity.Cpaindh34);
            dbProvider.AddInParameter(command, helper.Cpaindh35, DbType.Decimal, entity.Cpaindh35);
            dbProvider.AddInParameter(command, helper.Cpaindh36, DbType.Decimal, entity.Cpaindh36);
            dbProvider.AddInParameter(command, helper.Cpaindh37, DbType.Decimal, entity.Cpaindh37);
            dbProvider.AddInParameter(command, helper.Cpaindh38, DbType.Decimal, entity.Cpaindh38);
            dbProvider.AddInParameter(command, helper.Cpaindh39, DbType.Decimal, entity.Cpaindh39);
            dbProvider.AddInParameter(command, helper.Cpaindh40, DbType.Decimal, entity.Cpaindh40);
            dbProvider.AddInParameter(command, helper.Cpaindh41, DbType.Decimal, entity.Cpaindh41);
            dbProvider.AddInParameter(command, helper.Cpaindh42, DbType.Decimal, entity.Cpaindh42);
            dbProvider.AddInParameter(command, helper.Cpaindh43, DbType.Decimal, entity.Cpaindh43);
            dbProvider.AddInParameter(command, helper.Cpaindh44, DbType.Decimal, entity.Cpaindh44);
            dbProvider.AddInParameter(command, helper.Cpaindh45, DbType.Decimal, entity.Cpaindh45);
            dbProvider.AddInParameter(command, helper.Cpaindh46, DbType.Decimal, entity.Cpaindh46);
            dbProvider.AddInParameter(command, helper.Cpaindh47, DbType.Decimal, entity.Cpaindh47);
            dbProvider.AddInParameter(command, helper.Cpaindh48, DbType.Decimal, entity.Cpaindh48);
            dbProvider.AddInParameter(command, helper.Cpaindh49, DbType.Decimal, entity.Cpaindh49);
            dbProvider.AddInParameter(command, helper.Cpaindh50, DbType.Decimal, entity.Cpaindh50);
            dbProvider.AddInParameter(command, helper.Cpaindh51, DbType.Decimal, entity.Cpaindh51);
            dbProvider.AddInParameter(command, helper.Cpaindh52, DbType.Decimal, entity.Cpaindh52);
            dbProvider.AddInParameter(command, helper.Cpaindh53, DbType.Decimal, entity.Cpaindh53);
            dbProvider.AddInParameter(command, helper.Cpaindh54, DbType.Decimal, entity.Cpaindh54);
            dbProvider.AddInParameter(command, helper.Cpaindh55, DbType.Decimal, entity.Cpaindh55);
            dbProvider.AddInParameter(command, helper.Cpaindh56, DbType.Decimal, entity.Cpaindh56);
            dbProvider.AddInParameter(command, helper.Cpaindh57, DbType.Decimal, entity.Cpaindh57);
            dbProvider.AddInParameter(command, helper.Cpaindh58, DbType.Decimal, entity.Cpaindh58);
            dbProvider.AddInParameter(command, helper.Cpaindh59, DbType.Decimal, entity.Cpaindh59);
            dbProvider.AddInParameter(command, helper.Cpaindh60, DbType.Decimal, entity.Cpaindh60);
            dbProvider.AddInParameter(command, helper.Cpaindh61, DbType.Decimal, entity.Cpaindh61);
            dbProvider.AddInParameter(command, helper.Cpaindh62, DbType.Decimal, entity.Cpaindh62);
            dbProvider.AddInParameter(command, helper.Cpaindh63, DbType.Decimal, entity.Cpaindh63);
            dbProvider.AddInParameter(command, helper.Cpaindh64, DbType.Decimal, entity.Cpaindh64);
            dbProvider.AddInParameter(command, helper.Cpaindh65, DbType.Decimal, entity.Cpaindh65);
            dbProvider.AddInParameter(command, helper.Cpaindh66, DbType.Decimal, entity.Cpaindh66);
            dbProvider.AddInParameter(command, helper.Cpaindh67, DbType.Decimal, entity.Cpaindh67);
            dbProvider.AddInParameter(command, helper.Cpaindh68, DbType.Decimal, entity.Cpaindh68);
            dbProvider.AddInParameter(command, helper.Cpaindh69, DbType.Decimal, entity.Cpaindh69);
            dbProvider.AddInParameter(command, helper.Cpaindh70, DbType.Decimal, entity.Cpaindh70);
            dbProvider.AddInParameter(command, helper.Cpaindh71, DbType.Decimal, entity.Cpaindh71);
            dbProvider.AddInParameter(command, helper.Cpaindh72, DbType.Decimal, entity.Cpaindh72);
            dbProvider.AddInParameter(command, helper.Cpaindh73, DbType.Decimal, entity.Cpaindh73);
            dbProvider.AddInParameter(command, helper.Cpaindh74, DbType.Decimal, entity.Cpaindh74);
            dbProvider.AddInParameter(command, helper.Cpaindh75, DbType.Decimal, entity.Cpaindh75);
            dbProvider.AddInParameter(command, helper.Cpaindh76, DbType.Decimal, entity.Cpaindh76);
            dbProvider.AddInParameter(command, helper.Cpaindh77, DbType.Decimal, entity.Cpaindh77);
            dbProvider.AddInParameter(command, helper.Cpaindh78, DbType.Decimal, entity.Cpaindh78);
            dbProvider.AddInParameter(command, helper.Cpaindh79, DbType.Decimal, entity.Cpaindh79);
            dbProvider.AddInParameter(command, helper.Cpaindh80, DbType.Decimal, entity.Cpaindh80);
            dbProvider.AddInParameter(command, helper.Cpaindh81, DbType.Decimal, entity.Cpaindh81);
            dbProvider.AddInParameter(command, helper.Cpaindh82, DbType.Decimal, entity.Cpaindh82);
            dbProvider.AddInParameter(command, helper.Cpaindh83, DbType.Decimal, entity.Cpaindh83);
            dbProvider.AddInParameter(command, helper.Cpaindh84, DbType.Decimal, entity.Cpaindh84);
            dbProvider.AddInParameter(command, helper.Cpaindh85, DbType.Decimal, entity.Cpaindh85);
            dbProvider.AddInParameter(command, helper.Cpaindh86, DbType.Decimal, entity.Cpaindh86);
            dbProvider.AddInParameter(command, helper.Cpaindh87, DbType.Decimal, entity.Cpaindh87);
            dbProvider.AddInParameter(command, helper.Cpaindh88, DbType.Decimal, entity.Cpaindh88);
            dbProvider.AddInParameter(command, helper.Cpaindh89, DbType.Decimal, entity.Cpaindh89);
            dbProvider.AddInParameter(command, helper.Cpaindh90, DbType.Decimal, entity.Cpaindh90);
            dbProvider.AddInParameter(command, helper.Cpaindh91, DbType.Decimal, entity.Cpaindh91);
            dbProvider.AddInParameter(command, helper.Cpaindh92, DbType.Decimal, entity.Cpaindh92);
            dbProvider.AddInParameter(command, helper.Cpaindh93, DbType.Decimal, entity.Cpaindh93);
            dbProvider.AddInParameter(command, helper.Cpaindh94, DbType.Decimal, entity.Cpaindh94);
            dbProvider.AddInParameter(command, helper.Cpaindh95, DbType.Decimal, entity.Cpaindh95);
            dbProvider.AddInParameter(command, helper.Cpaindh96, DbType.Decimal, entity.Cpaindh96);
            dbProvider.AddInParameter(command, helper.Cpaindusucreacion, DbType.String, entity.Cpaindusucreacion);
            dbProvider.AddInParameter(command, helper.Cpaindfeccreacion, DbType.DateTime, entity.Cpaindfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Delete(int cpaIndCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpaindcodi, DbType.Int32, cpaIndCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpaInsumoDiaDTO GetById(int cpaIndCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpaindcodi, DbType.Int32, cpaIndCodi);
            CpaInsumoDiaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpaInsumoDiaDTO> List()
        {
            List<CpaInsumoDiaDTO> entities = new List<CpaInsumoDiaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

        public List<CpaInsumoDiaDTO> ListByTipoInsumoByPeriodo(string Cpaindtipinsumo, int Cparcodi, DateTime dFecInicio, DateTime dFecFin)
        {
            List<CpaInsumoDiaDTO> entities = new List<CpaInsumoDiaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByTipoInsumoByPeriodo);

            dbProvider.AddInParameter(command, helper.Cpaindtipinsumo, DbType.String, Cpaindtipinsumo);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, Cparcodi);
            dbProvider.AddInParameter(command, helper.Cpaindfeccreacion, DbType.DateTime, dFecInicio);
            dbProvider.AddInParameter(command, helper.Cpaindfeccreacion, DbType.DateTime, dFecFin);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpaInsumoDiaDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iBarrbarratransferencia = dr.GetOrdinal(helper.Barrbarratransferencia);
                    if (!dr.IsDBNull(iBarrbarratransferencia)) entity.Barrbarratransferencia = dr.GetString(iBarrbarratransferencia);

                    entities.Add(entity);
                }
            }

            return entities;
        }

        public void DeleteByCentral(int cparcodi, int equicodi, string cpaindtipinsumo, int cpainmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCentral);

            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, cparcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Cpaindtipinsumo, DbType.Int32, cpaindtipinsumo);
            dbProvider.AddInParameter(command, helper.Cpainmcodi, DbType.Int32, cpainmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByRevision(int cparcodi, string cpaindtipinsumo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByRevision);

            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, cparcodi);
            dbProvider.AddInParameter(command, helper.Cpaindtipinsumo, DbType.Int32, cpaindtipinsumo);

            dbProvider.ExecuteNonQuery(command);
        }

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;
            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public void InsertarInsumoDiaByTMP(int cpaindcodi, int cpainmcodi, int emprcodi, int equicodi, DateTime fecinicio, DateTime fecfin)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlInsertarInsumoDiaByTMP);

            dbProvider.AddInParameter(command, helper.Cpaindcodi, DbType.Int32, cpaindcodi);
            dbProvider.AddInParameter(command, helper.Cpainmcodi, DbType.Int32, cpainmcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Cpainddia, DbType.DateTime, fecinicio);
            dbProvider.AddInParameter(command, helper.Cpaindfeccreacion, DbType.DateTime, fecfin);

            dbProvider.ExecuteNonQuery(command);
        }

        public void InsertarInsumoDiaByCMg(int cpaindcodi, int cpainmcodi, int cparcodi, int emprcodi, int equicodi, DateTime fecinicio, int pericodi, int recacodi, int barrcodi, string cpaindusucreacion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlInsertarInsumoDiaByCMg);

            dbProvider.AddInParameter(command, helper.Cpaindcodi, DbType.Int32, cpaindcodi);
            dbProvider.AddInParameter(command, helper.Cpainmcodi, DbType.Int32, cpainmcodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, cparcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Cpaindusucreacion, DbType.String, cpaindusucreacion);
            dbProvider.AddInParameter(command, helper.Cpainddia, DbType.DateTime, fecinicio);
            dbProvider.AddInParameter(command, helper.Cpaindcodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Cpaindcodi, DbType.Int32, recacodi);
            dbProvider.AddInParameter(command, helper.Cpaindcodi, DbType.Int32, barrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void InsertarInsumoDiaByCMgPMPO(int cpaindcodi, int cpainmcodi, int cparcodi, int emprcodi, int equicodi, int ptomedicodi, DateTime fecinicio, DateTime fecfin, decimal dTipoCambio, string cpaindusucreacion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlInsertarInsumoDiaByCMgPMPO);

            dbProvider.AddInParameter(command, helper.Cpaindcodi, DbType.Int32, cpaindcodi);
            dbProvider.AddInParameter(command, helper.Cpainmcodi, DbType.Int32, cpainmcodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, cparcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Cpaindusucreacion, DbType.String, cpaindusucreacion);
            for (int i = 0; i <= 96; i++) {
                dbProvider.AddInParameter(command, helper.Cpaindtotaldia, DbType.Decimal, dTipoCambio);
            }
            dbProvider.AddInParameter(command, helper.Cpaindcodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Cpaindtipproceso, DbType.String, equicodi.ToString());
            dbProvider.AddInParameter(command, helper.Cpainddia, DbType.DateTime, fecinicio);
            dbProvider.AddInParameter(command, helper.Cpaindfeccreacion, DbType.DateTime, fecfin);

            dbProvider.ExecuteNonQuery(command);
        }

        public void InsertarInsumoDiaBySddp(int cpaindcodi, int cpainmcodi, int cparcodi, int emprcodi, int equicodi, DateTime fecinicio, DateTime fecfin)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlInsertarInsumoDiaBySddp);

            dbProvider.AddInParameter(command, helper.Cpaindcodi, DbType.Int32, cpaindcodi);
            dbProvider.AddInParameter(command, helper.Cpainmcodi, DbType.Int32, cpainmcodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, cparcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Cpainddia, DbType.DateTime, fecinicio);
            dbProvider.AddInParameter(command, helper.Cpaindfeccreacion, DbType.DateTime, fecfin);

            dbProvider.ExecuteNonQuery(command);
        }

        public void BulkInsertCpaInsumoDia(List<CpaInsumoDiaDTO> entitys)
        {
            string nombreTabla = "CPA_INSUMO_DIA";

            dbProvider.AddColumnMapping(helper.Cpaindcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cpainmcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cparcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cpaindtipinsumo, DbType.String);
            dbProvider.AddColumnMapping(helper.Cpaindtipproceso, DbType.String);
            dbProvider.AddColumnMapping(helper.Emprcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Equicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cpainddia, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Cpaindtotaldia, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh1, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh2, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh3, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh4, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh5, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh6, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh7, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh8, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh9, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh10, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh11, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh12, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh13, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh14, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh15, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh16, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh17, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh18, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh19, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh20, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh21, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh22, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh23, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh24, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh25, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh26, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh27, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh28, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh29, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh30, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh31, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh32, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh33, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh34, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh35, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh36, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh37, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh38, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh39, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh40, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh41, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh42, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh43, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh44, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh45, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh46, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh47, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh48, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh49, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh50, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh51, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh52, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh53, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh54, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh55, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh56, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh57, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh58, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh59, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh60, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh61, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh62, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh63, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh64, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh65, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh66, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh67, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh68, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh69, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh70, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh71, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh72, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh73, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh74, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh75, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh76, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh77, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh78, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh79, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh80, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh81, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh82, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh83, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh84, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh85, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh86, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh87, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh88, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh89, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh90, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh91, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh92, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh93, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh94, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh95, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindh96, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cpaindusucreacion, DbType.String);
            dbProvider.AddColumnMapping(helper.Cpaindfeccreacion, DbType.DateTime);

            dbProvider.BulkInsert<CpaInsumoDiaDTO>(entitys, nombreTabla);
        }

        public List<CpaInsumoDiaDTO> GetByRevisionByTipo(int cparcodi, string tipo)
        {
            string query = string.Format(helper.SqlGetByRevisionByTipo, cparcodi, tipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<CpaInsumoDiaDTO> entitys = new List<CpaInsumoDiaDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpaInsumoDiaDTO entity = helper.Create(dr);

                    int iCpainmmes = dr.GetOrdinal(helper.Cpainmmes);
                    if (!dr.IsDBNull(iCpainmmes)) entity.Cpainmmes = dr.GetInt32(iCpainmmes);
                    
                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public int GetNumRegistrosByFecha(DateTime fecinicio)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetNumRegistrosByFecha);
            dbProvider.AddInParameter(command, helper.Cpainddia, DbType.DateTime, fecinicio);
            int iNumReg = 0;
            object result = dbProvider.ExecuteScalar(command);
            if (result != null) iNumReg = Convert.ToInt32(result);

            return iNumReg;
        }

        public void UpdateMesEquipo(int cpainmcodi, int iNumAnio)
        {
            int iNumMeses = -12 * iNumAnio;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateMesEquipo);

            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, iNumMeses);
            dbProvider.AddInParameter(command, helper.Cpainmcodi, DbType.Int32, cpainmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public int GetNumRegistrosCMgByFecha(int ptomedicodi, int equicodi, DateTime fecinicio)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetNumRegistrosCMgByFecha);
            dbProvider.AddInParameter(command, helper.Cpaindcodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Cpaindtipproceso, DbType.String, equicodi.ToString());
            dbProvider.AddInParameter(command, helper.Cpainddia, DbType.DateTime, fecinicio);
            int iNumReg = 0;
            object result = dbProvider.ExecuteScalar(command);
            if (result != null) iNumReg = Convert.ToInt32(result);

            return iNumReg;
        }
    }
}
