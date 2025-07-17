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
    /// Clase de acceso a datos de la tabla PR_AGRUPACIONCONCEPTO
    /// </summary>
    public class PrAgrupacionConceptoRepository : RepositoryBase, IPrAgrupacionConceptoRepository
    {
        public PrAgrupacionConceptoRepository(string strConn)
            : base(strConn)
        {
        }

        PrAgrupacionConceptoHelper helper = new PrAgrupacionConceptoHelper();

        public int Save(PrAgrupacionConceptoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Agrconcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Agrupcodi, DbType.Int32, entity.Agrupcodi);
            dbProvider.AddInParameter(command, helper.Agrconfecha, DbType.DateTime, entity.Agrconfecha);
            dbProvider.AddInParameter(command, helper.Agrconactivo, DbType.Int32, entity.Agrconactivo);
            dbProvider.AddInParameter(command, helper.Agrconfeccreacion, DbType.DateTime, entity.Agrconfeccreacion);
            dbProvider.AddInParameter(command, helper.Agrconusucreacion, DbType.String, entity.Agrconusucreacion);
            dbProvider.AddInParameter(command, helper.Agrconusumodificacion, DbType.String, entity.Agrconusumodificacion);
            dbProvider.AddInParameter(command, helper.Agrconfecmodificacion, DbType.DateTime, entity.Agrconfecmodificacion);
            dbProvider.AddInParameter(command, helper.Concepcodi, DbType.Int32, entity.Concepcodi);
            dbProvider.AddInParameter(command, helper.Propcodi, DbType.Int32, entity.Propcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrAgrupacionConceptoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Agrupcodi, DbType.Int32, entity.Agrupcodi);
            dbProvider.AddInParameter(command, helper.Agrconfecha, DbType.DateTime, entity.Agrconfecha);
            dbProvider.AddInParameter(command, helper.Agrconactivo, DbType.Int32, entity.Agrconactivo);
            dbProvider.AddInParameter(command, helper.Agrconfeccreacion, DbType.DateTime, entity.Agrconfeccreacion);
            dbProvider.AddInParameter(command, helper.Agrconusucreacion, DbType.String, entity.Agrconusucreacion);
            dbProvider.AddInParameter(command, helper.Agrconusumodificacion, DbType.String, entity.Agrconusumodificacion);
            dbProvider.AddInParameter(command, helper.Agrconfecmodificacion, DbType.DateTime, entity.Agrconfecmodificacion);
            dbProvider.AddInParameter(command, helper.Concepcodi, DbType.Int32, entity.Concepcodi);
            dbProvider.AddInParameter(command, helper.Propcodi, DbType.Int32, entity.Propcodi);

            dbProvider.AddInParameter(command, helper.Agrconcodi, DbType.Int32, entity.Agrconcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int agrconcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Agrconcodi, DbType.Int32, agrconcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrAgrupacionConceptoDTO GetById(int agrconcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Agrconcodi, DbType.Int32, agrconcodi);
            PrAgrupacionConceptoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrAgrupacionConceptoDTO> List()
        {
            List<PrAgrupacionConceptoDTO> entitys = new List<PrAgrupacionConceptoDTO>();
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

        public List<PrAgrupacionConceptoDTO> GetByCriteria(int agrupcodi)
        {
            List<PrAgrupacionConceptoDTO> entitys = new List<PrAgrupacionConceptoDTO>();

            string query = string.Format(helper.SqlGetByCriteria, agrupcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrAgrupacionConceptoDTO entity = helper.Create(dr);
                    
                    int iConcepabrev = dr.GetOrdinal(this.helper.Concepabrev);
                    if (!dr.IsDBNull(iConcepabrev)) entity.Concepabrev = dr.GetString(iConcepabrev);

                    int iConcepdesc = dr.GetOrdinal(this.helper.Concepdesc);
                    if (!dr.IsDBNull(iConcepdesc)) entity.Concepdesc = dr.GetString(iConcepdesc);

                    int iConcepnombficha = dr.GetOrdinal(helper.Concepnombficha);
                    if (!dr.IsDBNull(iConcepnombficha)) entity.Concepnombficha = dr.GetString(iConcepnombficha);

                    int iConcepunid = dr.GetOrdinal(this.helper.Concepunid);
                    if (!dr.IsDBNull(iConcepunid)) entity.Concepunid = dr.GetString(iConcepunid);

                    int iConceptipo = dr.GetOrdinal(this.helper.Conceptipo);
                    if (!dr.IsDBNull(iConceptipo)) entity.Conceptipo = dr.GetString(iConceptipo);

                    int iCatenomb = dr.GetOrdinal(helper.Catenomb);
                    if (!dr.IsDBNull(iCatenomb)) entity.Catenomb = dr.GetString(iCatenomb);

                    int iCatecodi = dr.GetOrdinal(helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

                    int iCateabrev = dr.GetOrdinal(helper.Cateabrev);
                    if (!dr.IsDBNull(iCateabrev)) entity.Cateabrev = dr.GetString(iCateabrev);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
