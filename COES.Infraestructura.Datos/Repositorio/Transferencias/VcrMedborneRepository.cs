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
    /// Clase de acceso a datos de la tabla VCR_MEDBORNE
    /// </summary>
    public class VcrMedborneRepository: RepositoryBase, IVcrMedborneRepository
    {
        public VcrMedborneRepository(string strConn): base(strConn)
        {
        }

        VcrMedborneHelper helper = new VcrMedborneHelper();

        public int Save(VcrMedborneDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcrmebcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodicen, DbType.Int32, entity.Equicodicen);
            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, entity.Equicodiuni);
            dbProvider.AddInParameter(command, helper.Vcrmebfecha, DbType.DateTime, entity.Vcrmebfecha);
            dbProvider.AddInParameter(command, helper.Vcrmebptomed, DbType.String, entity.Vcrmebptomed);
            dbProvider.AddInParameter(command, helper.Vcrmebpotenciamed, DbType.Decimal, entity.Vcrmebpotenciamed);
            dbProvider.AddInParameter(command, helper.Vcrmebusucreacion, DbType.String, entity.Vcrmebusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrmebfeccreacion, DbType.DateTime, DateTime.Now);
            //ASSETEC 202012
            dbProvider.AddInParameter(command, helper.Vcrmebpotenciamedgrp, DbType.Decimal, entity.Vcrmebpotenciamedgrp);
            dbProvider.AddInParameter(command, helper.Vcrmebpresencia, DbType.Decimal, entity.Vcrmebpresencia);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void BulkInsert(List<VcrMedborneDTO> entitys)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);
            foreach (var item in entitys)
            {
                item.Vcrmebcodi = id;
                id = id + 1;
            }
            dbProvider.AddColumnMapping(helper.Vcrmebcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Vcrecacodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Emprcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Equicodicen, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Equicodiuni, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Vcrmebfecha, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Vcrmebptomed, DbType.String);
            dbProvider.AddColumnMapping(helper.Vcrmebpotenciamed, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Vcrmebusucreacion, DbType.String);
            dbProvider.AddColumnMapping(helper.Vcrmebfeccreacion, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Vcrmebpotenciamedgrp, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Vcrmebpresencia, DbType.Decimal);
           
            dbProvider.BulkInsert<VcrMedborneDTO>(entitys, helper.TableName);
        }

        public void Update(VcrMedborneDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodicen, DbType.Int32, entity.Equicodicen);
            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, entity.Equicodiuni);
            dbProvider.AddInParameter(command, helper.Vcrmebfecha, DbType.DateTime, entity.Vcrmebfecha);
            dbProvider.AddInParameter(command, helper.Vcrmebptomed, DbType.String, entity.Vcrmebptomed);
            dbProvider.AddInParameter(command, helper.Vcrmebpotenciamed, DbType.Decimal, entity.Vcrmebpotenciamed);
            dbProvider.AddInParameter(command, helper.Vcrmebusucreacion, DbType.String, entity.Vcrmebusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrmebfeccreacion, DbType.DateTime, entity.Vcrmebfeccreacion);
            dbProvider.AddInParameter(command, helper.Vcrmebcodi, DbType.Int32, entity.Vcrmebcodi);
            //ASSETEC 202012
            dbProvider.AddInParameter(command, helper.Vcrmebpotenciamedgrp, DbType.Decimal, entity.Vcrmebpotenciamedgrp);
            dbProvider.AddInParameter(command, helper.Vcrmebpresencia, DbType.Decimal, entity.Vcrmebpresencia);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrMedborneDTO GetById(int vcrmebcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcrmebcodi, DbType.Int32, vcrmebcodi);
            VcrMedborneDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrMedborneDTO> List()
        {
            List<VcrMedborneDTO> entitys = new List<VcrMedborneDTO>();
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

        public List<VcrMedborneDTO> GetByCriteria()
        {
            List<VcrMedborneDTO> entitys = new List<VcrMedborneDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<VcrMedborneDTO> ListDistintos(int vcrecacodi)
        {
            List<VcrMedborneDTO> entitys = new List<VcrMedborneDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListDistintos);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrMedborneDTO entity = new VcrMedborneDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodicen = dr.GetOrdinal(this.helper.Equicodicen);
                    if (!dr.IsDBNull(iEquicodicen)) entity.Equicodicen = Convert.ToInt32(dr.GetValue(iEquicodicen));

                    int iEquicodiuni = dr.GetOrdinal(this.helper.Equicodiuni);
                    if (!dr.IsDBNull(iEquicodiuni)) entity.Equicodiuni = Convert.ToInt32(dr.GetValue(iEquicodiuni));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VcrMedborneDTO> ListDiaSinUnidExonRSF(int vcrecacodi, DateTime vcrmebfecha)
        {
            List<VcrMedborneDTO> entitys = new List<VcrMedborneDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListDiaSinUnidExonRSF);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Vcrmebfecha, DbType.DateTime, vcrmebfecha);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<VcrMedborneDTO> ListMes(int vcrecacodi)
        {
            List<VcrMedborneDTO> entitys = new List<VcrMedborneDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListMes);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrMedborneDTO entity = new VcrMedborneDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    int iEquicodicen = dr.GetOrdinal(this.helper.Equicodicen);
                    if (!dr.IsDBNull(iEquicodicen)) entity.Equicodicen = Convert.ToInt32(dr.GetValue(iEquicodicen));

                    int iEquinombcen = dr.GetOrdinal(this.helper.Equinombcen);
                    if (!dr.IsDBNull(iEquinombcen)) entity.Equinombcen = Convert.ToString(dr.GetValue(iEquinombcen));

                    int iEquicodiuni = dr.GetOrdinal(this.helper.Equicodiuni);
                    if (!dr.IsDBNull(iEquicodiuni)) entity.Equicodiuni = Convert.ToInt32(dr.GetValue(iEquicodiuni));

                    int iEquinombuni = dr.GetOrdinal(this.helper.Equinombuni);
                    if (!dr.IsDBNull(iEquinombuni)) entity.Equinombuni = Convert.ToString(dr.GetValue(iEquinombuni));

                    int iVcmbciconsiderar = dr.GetOrdinal(this.helper.Vcmbciconsiderar);
                    if (!dr.IsDBNull(iVcmbciconsiderar)) entity.Vcmbciconsiderar = Convert.ToString(dr.GetValue(iVcmbciconsiderar));

                    int iVcrmebpotenciamed = dr.GetOrdinal(this.helper.Vcrmebpotenciamed);
                    if (!dr.IsDBNull(iVcrmebpotenciamed)) entity.Vcrmebpotenciamed = dr.GetDecimal(iVcrmebpotenciamed);

                    //ASSETEC 202012
                    //int iVcrmebpotenciamedgrp = dr.GetOrdinal(this.helper.Vcrmebpotenciamedgrp);
                    //if (!dr.IsDBNull(iVcrmebpotenciamedgrp)) entity.Vcrmebpotenciamedgrp = dr.GetDecimal(iVcrmebpotenciamedgrp);

                    int iVcrmebpresencia = dr.GetOrdinal(this.helper.Vcrmebpresencia);
                    if (!dr.IsDBNull(iVcrmebpresencia)) entity.Vcrmebpresencia = dr.GetDecimal(iVcrmebpresencia);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VcrMedborneDTO> ListMesConsiderados(int vcrecacodi)
        {
            List<VcrMedborneDTO> entitys = new List<VcrMedborneDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListMesConsiderados);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrMedborneDTO entity = new VcrMedborneDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodicen = dr.GetOrdinal(this.helper.Equicodicen);
                    if (!dr.IsDBNull(iEquicodicen)) entity.Equicodicen = Convert.ToInt32(dr.GetValue(iEquicodicen));

                    int iEquicodiuni = dr.GetOrdinal(this.helper.Equicodiuni);
                    if (!dr.IsDBNull(iEquicodiuni)) entity.Equicodiuni = Convert.ToInt32(dr.GetValue(iEquicodiuni));

                    int iVcrmebpotenciamed = dr.GetOrdinal(this.helper.Vcrmebpotenciamed);
                    if (!dr.IsDBNull(iVcrmebpotenciamed)) entity.Vcrmebpotenciamed = dr.GetDecimal(iVcrmebpotenciamed);

                    //ASSETEC 20210216: Vcrmebpotenciamedgrp almacena temporalmente vcrciincumplsrvrsf
                    int iVcrmebpotenciamedgrp = dr.GetOrdinal(this.helper.Vcrmebpotenciamedgrp);
                    if (!dr.IsDBNull(iVcrmebpotenciamedgrp)) entity.Vcrmebpotenciamedgrp = dr.GetDecimal(iVcrmebpotenciamedgrp);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VcrMedborneDTO> ListMesConsideradosTotales(int vcrecacodi)
        {
            List<VcrMedborneDTO> entitys = new List<VcrMedborneDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListMesConsideradosTotales);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrMedborneDTO entity = new VcrMedborneDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodicen = dr.GetOrdinal(this.helper.Equicodicen);
                    if (!dr.IsDBNull(iEquicodicen)) entity.Equicodicen = Convert.ToInt32(dr.GetValue(iEquicodicen));

                    int iEquicodiuni = dr.GetOrdinal(this.helper.Equicodiuni);
                    if (!dr.IsDBNull(iEquicodiuni)) entity.Equicodiuni = Convert.ToInt32(dr.GetValue(iEquicodiuni));

                    int iVcrmebpotenciamed = dr.GetOrdinal(this.helper.Vcrmebpotenciamed);
                    if (!dr.IsDBNull(iVcrmebpotenciamed)) entity.Vcrmebpotenciamed = dr.GetDecimal(iVcrmebpotenciamed);

                    //ASSETEC 202012
                    //int iVcrmebpotenciamedgrp = dr.GetOrdinal(this.helper.Vcrmebpotenciamedgrp);
                    //if (!dr.IsDBNull(iVcrmebpotenciamedgrp)) entity.Vcrmebpotenciamedgrp = dr.GetDecimal(iVcrmebpotenciamedgrp);

                    //int iVcrmebpresencia = dr.GetOrdinal(this.helper.Vcrmebpresencia);
                    //if (!dr.IsDBNull(iVcrmebpresencia)) entity.Vcrmebpresencia = dr.GetDecimal(iVcrmebpresencia);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //ASSETEC 202012
        public decimal TotalUnidNoExonRSF(int vcrecacodi)
        {
            decimal dTotalUnidNoExonRSF = 0;
            List<VcrMedborneDTO> entitys = new List<VcrMedborneDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlTotalUnidNoExonRSF);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iVcrmebpotenciamed = dr.GetOrdinal(this.helper.Vcrmebpotenciamed);
                    if (!dr.IsDBNull(iVcrmebpotenciamed)) dTotalUnidNoExonRSF = dr.GetDecimal(iVcrmebpotenciamed);
                }
            }

            return dTotalUnidNoExonRSF;
        }
    }
}
