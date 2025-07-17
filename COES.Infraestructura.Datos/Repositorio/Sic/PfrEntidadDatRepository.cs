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
    /// Clase de acceso a datos de la tabla PFR_ENTIDAD_DAT
    /// </summary>
    public class PfrEntidadDatRepository: RepositoryBase, IPfrEntidadDatRepository
    {
        public PfrEntidadDatRepository(string strConn): base(strConn)
        {
        }

        PfrEntidadDatHelper helper = new PfrEntidadDatHelper();

        public void Save(PfrEntidadDatDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pfrentcodi, DbType.Int32, entity.Pfrentcodi);
            dbProvider.AddInParameter(command, helper.Pfrcnpcodi, DbType.Int32, entity.Pfrcnpcodi);
            dbProvider.AddInParameter(command, helper.Prfdatfechavig, DbType.DateTime, entity.Prfdatfechavig);
            dbProvider.AddInParameter(command, helper.Pfrdatdeleted, DbType.Int32, entity.Pfrdatdeleted);
            dbProvider.AddInParameter(command, helper.Pfrdatvalor, DbType.String, entity.Pfrdatvalor);
            dbProvider.AddInParameter(command, helper.Pfrdatfeccreacion, DbType.DateTime, entity.Pfrdatfeccreacion);
            dbProvider.AddInParameter(command, helper.Pfrdatusucreacion, DbType.String, entity.Pfrdatusucreacion);
            dbProvider.AddInParameter(command, helper.Pfrdatfecmodificacion, DbType.DateTime, entity.Pfrdatfecmodificacion);
            dbProvider.AddInParameter(command, helper.Pfrdatusumodificacion, DbType.String, entity.Pfrdatusumodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(PfrEntidadDatDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfrdatvalor, DbType.String, entity.Pfrdatvalor);
            dbProvider.AddInParameter(command, helper.Pfrdatfeccreacion, DbType.DateTime, entity.Pfrdatfeccreacion);
            dbProvider.AddInParameter(command, helper.Pfrdatusucreacion, DbType.String, entity.Pfrdatusucreacion);
            dbProvider.AddInParameter(command, helper.Pfrdatfecmodificacion, DbType.DateTime, entity.Pfrdatfecmodificacion);
            dbProvider.AddInParameter(command, helper.Pfrdatusumodificacion, DbType.String, entity.Pfrdatusumodificacion);
            dbProvider.AddInParameter(command, helper.Pfrdatdeleted2, DbType.Int32, entity.Pfrdatdeleted2);

            dbProvider.AddInParameter(command, helper.Pfrentcodi, DbType.Int32, entity.Pfrentcodi);
            dbProvider.AddInParameter(command, helper.Pfrcnpcodi, DbType.Int32, entity.Pfrcnpcodi);
            dbProvider.AddInParameter(command, helper.Prfdatfechavig, DbType.DateTime, entity.Prfdatfechavig);
            dbProvider.AddInParameter(command, helper.Pfrdatdeleted, DbType.Int32, entity.Pfrdatdeleted);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfrentcodi, int pfrcnpcodi, DateTime prfdatfechavig, int pfrdatdeleted)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfrentcodi, DbType.Int32, pfrentcodi);
            dbProvider.AddInParameter(command, helper.Pfrcnpcodi, DbType.Int32, pfrcnpcodi);
            dbProvider.AddInParameter(command, helper.Prfdatfechavig, DbType.DateTime, prfdatfechavig);
            dbProvider.AddInParameter(command, helper.Pfrdatdeleted, DbType.Int32, pfrdatdeleted);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfrEntidadDatDTO GetById(int pfrentcodi, int pfrcnpcodi, DateTime prfdatfechavig, int pfrdatdeleted)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfrentcodi, DbType.Int32, pfrentcodi);
            dbProvider.AddInParameter(command, helper.Pfrcnpcodi, DbType.Int32, pfrcnpcodi);
            dbProvider.AddInParameter(command, helper.Prfdatfechavig, DbType.DateTime, prfdatfechavig);
            dbProvider.AddInParameter(command, helper.Pfrdatdeleted, DbType.Int32, pfrdatdeleted);
            PfrEntidadDatDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfrEntidadDatDTO> List()
        {
            List<PfrEntidadDatDTO> entitys = new List<PfrEntidadDatDTO>();
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

        public List<PfrEntidadDatDTO> GetByCriteria(int pfrentcodi, int pfrcnpcodi)
        {
            List<PfrEntidadDatDTO> entitys = new List<PfrEntidadDatDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, pfrentcodi, pfrcnpcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iPfrcnpnomb = dr.GetOrdinal(helper.Pfrcnpnomb);
                    if (!dr.IsDBNull(iPfrcnpnomb)) entity.Pfrcnpnomb = dr.GetString(iPfrcnpnomb);

                    int iPfrcatcodi = dr.GetOrdinal(helper.Pfrcatcodi);
                    if (!dr.IsDBNull(iPfrcatcodi)) entity.Pfrcatcodi = Convert.ToInt32(dr.GetValue(iPfrcatcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PfrEntidadDatDTO> ListarPfrentidadVigente(DateTime fechaVigencia, string pfrentcodis, int pfrcatcodi)
        {
            List<PfrEntidadDatDTO> entitys = new List<PfrEntidadDatDTO>();
            var querySql = string.Format(helper.SqlListarPfrentidadVigente, fechaVigencia.ToString(ConstantesBase.FormatoFechaBase), pfrentcodis, pfrcatcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(querySql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iPfrcnpnomb = dr.GetOrdinal(helper.Pfrcnpnomb);
                    if (!dr.IsDBNull(iPfrcnpnomb)) entity.Pfrcnpnomb = dr.GetString(iPfrcnpnomb);

                    int iPfrcatcodi = dr.GetOrdinal(helper.Pfrcatcodi);
                    if (!dr.IsDBNull(iPfrcatcodi)) entity.Pfrcatcodi = Convert.ToInt32(dr.GetValue(iPfrcatcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
