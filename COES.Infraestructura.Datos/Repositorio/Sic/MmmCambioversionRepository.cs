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
    /// Clase de acceso a datos de la tabla MMM_CAMBIOVERSION
    /// </summary>
    public class MmmCambioversionRepository : RepositoryBase, IMmmCambioversionRepository
    {
        public MmmCambioversionRepository(string strConn)
            : base(strConn)
        {
        }

        MmmCambioversionHelper helper = new MmmCambioversionHelper();

        public int Save(MmmCambioversionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Camvercodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vermmcodi, DbType.Int32, entity.Vermmcodi);
            dbProvider.AddInParameter(command, helper.Camvertipo, DbType.Int32, entity.Camvertipo);
            dbProvider.AddInParameter(command, helper.Camverfeccreacion, DbType.DateTime, entity.Camverfeccreacion);
            dbProvider.AddInParameter(command, helper.Camverusucreacion, DbType.String, entity.Camverusucreacion);
            dbProvider.AddInParameter(command, helper.Camvervalor, DbType.Decimal, entity.Camvervalor);
            dbProvider.AddInParameter(command, helper.Mmmdatcodi, DbType.Int32, entity.Mmmdatcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MmmCambioversionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vermmcodi, DbType.Int32, entity.Vermmcodi);
            dbProvider.AddInParameter(command, helper.Camvercodi, DbType.Int32, entity.Camvercodi);
            dbProvider.AddInParameter(command, helper.Camvertipo, DbType.Int32, entity.Camvertipo);
            dbProvider.AddInParameter(command, helper.Camverfeccreacion, DbType.DateTime, entity.Camverfeccreacion);
            dbProvider.AddInParameter(command, helper.Camverusucreacion, DbType.String, entity.Camverusucreacion);
            dbProvider.AddInParameter(command, helper.Camvervalor, DbType.Decimal, entity.Camvervalor);
            dbProvider.AddInParameter(command, helper.Mmmdatcodi, DbType.Int32, entity.Mmmdatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MmmCambioversionDTO GetById(int camvercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Camvercodi, DbType.Int32, camvercodi);
            MmmCambioversionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MmmCambioversionDTO> List()
        {
            List<MmmCambioversionDTO> entitys = new List<MmmCambioversionDTO>();
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

        public List<MmmCambioversionDTO> ListByPeriodo(DateTime fechaPeriodo)
        {
            List<MmmCambioversionDTO> entitys = new List<MmmCambioversionDTO>();
            string sql = string.Format(helper.SqlListByPeriodo, fechaPeriodo.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MmmCambioversionDTO> GetByCriteria(int vermmcodi)
        {
            List<MmmCambioversionDTO> entitys = new List<MmmCambioversionDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, vermmcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MmmCambioversionDTO entity = helper.Create(dr);

                    int iMmmdatofecha = dr.GetOrdinal(this.helper.Mmmdatfecha);
                    if (!dr.IsDBNull(iMmmdatofecha)) entity.Mmmdatfecha = dr.GetDateTime(iMmmdatofecha);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}