using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla ME_RELACIONPTO
    /// </summary>
    public class MeRelacionptoRepository : RepositoryBase, IMeRelacionptoRepository
    {
        public MeRelacionptoRepository(string strConn)
            : base(strConn)
        {
        }

        MeRelacionptoHelper helper = new MeRelacionptoHelper();

        public int Save(MeRelacionptoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Relptocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ptomedicodi1, DbType.Int32, entity.Ptomedicodi1);
            dbProvider.AddInParameter(command, helper.Ptomedicodi2, DbType.Int32, entity.Ptomedicodi2);
            dbProvider.AddInParameter(command, helper.Trptocodi, DbType.Int32, entity.Trptocodi);
            dbProvider.AddInParameter(command, helper.Relptofactor, DbType.Decimal, entity.Relptofactor);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Tptomedicodi, DbType.Int32, entity.Tptomedicodi);
            dbProvider.AddInParameter(command, helper.Relptotabmed, DbType.Int32, entity.Relptotabmed);
            dbProvider.AddInParameter(command, helper.Funptocodi, DbType.Int32, entity.Funptocodi);
            dbProvider.AddInParameter(command, helper.Relptopotencia, DbType.Decimal, entity.Relptopotencia);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MeRelacionptoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ptomedicodi1, DbType.Int32, entity.Ptomedicodi1);
            dbProvider.AddInParameter(command, helper.Ptomedicodi2, DbType.Int32, entity.Ptomedicodi2);
            dbProvider.AddInParameter(command, helper.Trptocodi, DbType.Int32, entity.Trptocodi);
            dbProvider.AddInParameter(command, helper.Relptofactor, DbType.Decimal, entity.Relptofactor);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Tptomedicodi, DbType.Int32, entity.Tptomedicodi);
            dbProvider.AddInParameter(command, helper.Relptotabmed, DbType.Int32, entity.Relptotabmed);
            dbProvider.AddInParameter(command, helper.Funptocodi, DbType.Int32, entity.Funptocodi);
            dbProvider.AddInParameter(command, helper.Relptopotencia, DbType.Decimal, entity.Relptopotencia);

            dbProvider.AddInParameter(command, helper.Relptocodi, DbType.Int32, entity.Relptocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int relptocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Relptocodi, DbType.Int32, relptocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeRelacionptoDTO GetById(int relptocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Relptocodi, DbType.Int32, relptocodi);
            MeRelacionptoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeRelacionptoDTO> List()
        {
            List<MeRelacionptoDTO> entitys = new List<MeRelacionptoDTO>();
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

        public List<MeRelacionptoDTO> GetByCriteria(string ptomedicodical, string ptomedicodi)
        {
            List<MeRelacionptoDTO> entitys = new List<MeRelacionptoDTO>();
            string query = string.Format(helper.SqlGetByCriteria, ptomedicodical, ptomedicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            MeRelacionptoDTO entity = new MeRelacionptoDTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iPtomedibarranomb = dr.GetOrdinal(helper.Ptomedinomb);
                    if (!dr.IsDBNull(iPtomedibarranomb)) entity.Ptomedinomb = dr.GetString(iPtomedibarranomb);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iRelptotabmed = dr.GetOrdinal(helper.Relptotabmed);
                    if (!dr.IsDBNull(iRelptotabmed)) entity.Relptotabmed = dr.GetInt32(iRelptotabmed);

                    int iFunptocodi = dr.GetOrdinal(this.helper.Funptocodi);
                    if (!dr.IsDBNull(iFunptocodi)) entity.Funptocodi = dr.GetInt32(iFunptocodi);

                    int iFunptofuncion = dr.GetOrdinal(this.helper.Funptofuncion);
                    if (!dr.IsDBNull(iFunptofuncion)) entity.Funptofuncion = dr.GetString(iFunptofuncion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}