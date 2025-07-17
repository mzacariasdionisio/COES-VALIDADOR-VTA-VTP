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
    /// Clase de acceso a datos de la tabla CM_FPMDETALLE
    /// </summary>
    public class CmFpmdetalleRepository: RepositoryBase, ICmFpmdetalleRepository
    {
        public CmFpmdetalleRepository(string strConn): base(strConn)
        {
        }

        CmFpmdetalleHelper helper = new CmFpmdetalleHelper();

        public int Save(CmFpmdetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cmfpmdcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cmfpmcodi, DbType.Int32, entity.Cmfpmcodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Cmfpmdbase, DbType.Decimal, entity.Cmfpmdbase);
            dbProvider.AddInParameter(command, helper.Cmfpmdmedia, DbType.Decimal, entity.Cmfpmdmedia);
            dbProvider.AddInParameter(command, helper.Cmfpmdpunta, DbType.Decimal, entity.Cmfpmdpunta);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmFpmdetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cmfpmcodi, DbType.Int32, entity.Cmfpmcodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Cmfpmdbase, DbType.Decimal, entity.Cmfpmdbase);
            dbProvider.AddInParameter(command, helper.Cmfpmdmedia, DbType.Decimal, entity.Cmfpmdmedia);
            dbProvider.AddInParameter(command, helper.Cmfpmdpunta, DbType.Decimal, entity.Cmfpmdpunta);
            dbProvider.AddInParameter(command, helper.Cmfpmdcodi, DbType.Int32, entity.Cmfpmdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(DateTime fecha)
        {
            string sql = string.Format(helper.SqlDelete, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.ExecuteNonQuery(command);
        }

        public CmFpmdetalleDTO GetById(int cmfpmdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cmfpmdcodi, DbType.Int32, cmfpmdcodi);
            CmFpmdetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmFpmdetalleDTO> List()
        {
            List<CmFpmdetalleDTO> entitys = new List<CmFpmdetalleDTO>();
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

        public List<CmFpmdetalleDTO> GetByCriteria(int idPadre)
        {
            List<CmFpmdetalleDTO> entitys = new List<CmFpmdetalleDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, idPadre);
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

        public List<CmFpmdetalleDTO> ObtenerPorFecha(DateTime fecha)
        {
            List<CmFpmdetalleDTO> entitys = new List<CmFpmdetalleDTO>();
            string sql = string.Format(helper.SqlObtenerPorFecha, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmFpmdetalleDTO entity = helper.Create(dr);

                    int iFechamax = dr.GetOrdinal(helper.Fechamax);
                    if (!dr.IsDBNull(iFechamax)) entity.FechaDatos = dr.GetDateTime(iFechamax);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
