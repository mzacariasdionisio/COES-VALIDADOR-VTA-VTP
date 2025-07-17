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
    /// Clase de acceso a datos de la tabla SPO_NUMERAL_GENFORZADA
    /// </summary>
    public class SpoNumeralGenforzadaRepository : RepositoryBase, ISpoNumeralGenforzadaRepository
    {
        public SpoNumeralGenforzadaRepository(string strConn) : base(strConn)
        {
        }

        SpoNumeralGenforzadaHelper helper = new SpoNumeralGenforzadaHelper();

        public int Save(SpoNumeralGenforzadaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Genforcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Verncodi, DbType.Int32, entity.Verncodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Hopcausacodi, DbType.Int32, entity.Hopcausacodi);
            dbProvider.AddInParameter(command, helper.Genforhorini, DbType.DateTime, entity.Genforhorini);
            dbProvider.AddInParameter(command, helper.Genforhorfin, DbType.DateTime, entity.Genforhorfin);
            dbProvider.AddInParameter(command, helper.Genformw, DbType.Decimal, entity.Genformw);
            dbProvider.AddInParameter(command, helper.Genforusucreacion, DbType.String, entity.Genforusucreacion);
            dbProvider.AddInParameter(command, helper.Genforfeccreacion, DbType.DateTime, entity.Genforfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SpoNumeralGenforzadaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Genforcodi, DbType.Int32, entity.Genforcodi);
            dbProvider.AddInParameter(command, helper.Verncodi, DbType.Int32, entity.Verncodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Hopcausacodi, DbType.Int32, entity.Hopcausacodi);
            dbProvider.AddInParameter(command, helper.Genforhorini, DbType.DateTime, entity.Genforhorini);
            dbProvider.AddInParameter(command, helper.Genforhorfin, DbType.DateTime, entity.Genforhorfin);
            dbProvider.AddInParameter(command, helper.Genformw, DbType.Decimal, entity.Genformw);
            dbProvider.AddInParameter(command, helper.Genforusucreacion, DbType.String, entity.Genforusucreacion);
            dbProvider.AddInParameter(command, helper.Genforfeccreacion, DbType.DateTime, entity.Genforfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int genforcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Genforcodi, DbType.Int32, genforcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SpoNumeralGenforzadaDTO GetById(int genforcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Genforcodi, DbType.Int32, genforcodi);
            SpoNumeralGenforzadaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SpoNumeralGenforzadaDTO> List()
        {
            List<SpoNumeralGenforzadaDTO> entitys = new List<SpoNumeralGenforzadaDTO>();
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

        public List<SpoNumeralGenforzadaDTO> GetByCriteria(int verncodi)
        {
            List<SpoNumeralGenforzadaDTO> entitys = new List<SpoNumeralGenforzadaDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, verncodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
