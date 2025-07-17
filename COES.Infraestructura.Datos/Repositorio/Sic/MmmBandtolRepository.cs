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
    /// Clase de acceso a datos de la tabla MMM_BANDTOL
    /// </summary>
    public class MmmBandtolRepository : RepositoryBase, IMmmBandtolRepository
    {
        public MmmBandtolRepository(string strConn)
            : base(strConn)
        {
        }

        MmmBandtolHelper helper = new MmmBandtolHelper();

        public int Save(MmmBandtolDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mmmtolcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Mmmtolfechavigencia, DbType.DateTime, entity.Mmmtolfechavigencia);
            dbProvider.AddInParameter(command, helper.Mmmtolusucreacion, DbType.String, entity.Mmmtolusucreacion);
            dbProvider.AddInParameter(command, helper.Mmmtolfeccreacion, DbType.DateTime, entity.Mmmtolfeccreacion);
            dbProvider.AddInParameter(command, helper.Mmmtolnormativa, DbType.String, entity.Mmmtolnormativa);
            dbProvider.AddInParameter(command, helper.Mmmtolusumodificacion, DbType.String, entity.Mmmtolusumodificacion);
            dbProvider.AddInParameter(command, helper.Mmmtolfecmodificacion, DbType.DateTime, entity.Mmmtolfecmodificacion);
            dbProvider.AddInParameter(command, helper.Immecodi, DbType.Int32, entity.Immecodi);
            dbProvider.AddInParameter(command, helper.Mmmtolcriterio, DbType.String, entity.Mmmtolcriterio);
            dbProvider.AddInParameter(command, helper.Mmmtolvalorreferencia, DbType.Decimal, entity.Mmmtolvalorreferencia);
            dbProvider.AddInParameter(command, helper.Mmmtolvalortolerancia, DbType.Decimal, entity.Mmmtolvalortolerancia);
            dbProvider.AddInParameter(command, helper.Mmmtolestado, DbType.String, entity.Mmmtolestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MmmBandtolDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mmmtolfechavigencia, DbType.DateTime, entity.Mmmtolfechavigencia);
            dbProvider.AddInParameter(command, helper.Mmmtolusucreacion, DbType.String, entity.Mmmtolusucreacion);
            dbProvider.AddInParameter(command, helper.Mmmtolfeccreacion, DbType.DateTime, entity.Mmmtolfeccreacion);
            dbProvider.AddInParameter(command, helper.Mmmtolnormativa, DbType.String, entity.Mmmtolnormativa);
            dbProvider.AddInParameter(command, helper.Mmmtolusumodificacion, DbType.String, entity.Mmmtolusumodificacion);
            dbProvider.AddInParameter(command, helper.Mmmtolfecmodificacion, DbType.DateTime, entity.Mmmtolfecmodificacion);
            dbProvider.AddInParameter(command, helper.Immecodi, DbType.Int32, entity.Immecodi);
            dbProvider.AddInParameter(command, helper.Mmmtolcriterio, DbType.String, entity.Mmmtolcriterio);
            dbProvider.AddInParameter(command, helper.Mmmtolvalorreferencia, DbType.Decimal, entity.Mmmtolvalorreferencia);
            dbProvider.AddInParameter(command, helper.Mmmtolvalortolerancia, DbType.Decimal, entity.Mmmtolvalortolerancia);
            dbProvider.AddInParameter(command, helper.Mmmtolestado, DbType.String, entity.Mmmtolestado);

            dbProvider.AddInParameter(command, helper.Mmmtolcodi, DbType.Int32, entity.Mmmtolcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mmmtolcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mmmtolcodi, DbType.Int32, mmmtolcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MmmBandtolDTO GetById(int mmmtolcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mmmtolcodi, DbType.Int32, mmmtolcodi);
            MmmBandtolDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iImmenombre = dr.GetOrdinal(this.helper.Immenombre);
                    if (!dr.IsDBNull(iImmenombre)) entity.Immenombre = dr.GetString(iImmenombre);

                    int iImmecodigo = dr.GetOrdinal(this.helper.Immecodigo);
                    if (!dr.IsDBNull(iImmecodigo)) entity.Immecodigo = dr.GetString(iImmecodigo);
                }
            }

            return entity;
        }

        public MmmBandtolDTO GetByIndicadorYPeriodo(int immecodi, DateTime fechaPeriodo)
        {
            string sql = String.Format(helper.SqlGetByIndicadorYPeriodo, immecodi, fechaPeriodo.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            MmmBandtolDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iImmenombre = dr.GetOrdinal(this.helper.Immenombre);
                    if (!dr.IsDBNull(iImmenombre)) entity.Immenombre = dr.GetString(iImmenombre);

                    int iImmecodigo = dr.GetOrdinal(this.helper.Immecodigo);
                    if (!dr.IsDBNull(iImmecodigo)) entity.Immecodigo = dr.GetString(iImmecodigo);
                }
            }

            return entity;
        }

        public List<MmmBandtolDTO> List()
        {
            List<MmmBandtolDTO> entitys = new List<MmmBandtolDTO>();
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

        public List<MmmBandtolDTO> GetByCriteria()
        {
            List<MmmBandtolDTO> entitys = new List<MmmBandtolDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MmmBandtolDTO entity = helper.Create(dr);

                    int iImmenombre = dr.GetOrdinal(this.helper.Immenombre);
                    if (!dr.IsDBNull(iImmenombre)) entity.Immenombre = dr.GetString(iImmenombre);

                    int iImmecodigo = dr.GetOrdinal(this.helper.Immecodigo);
                    if (!dr.IsDBNull(iImmecodigo)) entity.Immecodigo = dr.GetString(iImmecodigo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
