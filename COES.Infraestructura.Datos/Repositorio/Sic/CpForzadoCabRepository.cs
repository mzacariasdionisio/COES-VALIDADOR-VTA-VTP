using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CP_FORZADO_CAB
    /// </summary>
    public class CpForzadoCabRepository : RepositoryBase, ICpForzadoCabRepository
    {
        public CpForzadoCabRepository(string strConn) : base(strConn)
        {
        }

        CpForzadoCabHelper helper = new CpForzadoCabHelper();

        public int Save(CpForzadoCabDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.Cpfzfecha, DbType.DateTime, entity.Cpfzfecha);
            dbProvider.AddInParameter(command, helper.Cpfzbloquehorario, DbType.Int32, entity.Cpfzbloquehorario);
            dbProvider.AddInParameter(command, helper.Cpfzusuregistro, DbType.String, entity.Cpfzusuregistro);
            dbProvider.AddInParameter(command, helper.Cpfzfecregistro, DbType.DateTime, entity.Cpfzfecregistro);
            dbProvider.AddInParameter(command, helper.Cpfzcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CpForzadoCabDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.Cpfzfecha, DbType.DateTime, entity.Cpfzfecha);
            dbProvider.AddInParameter(command, helper.Cpfzbloquehorario, DbType.Int32, entity.Cpfzbloquehorario);
            dbProvider.AddInParameter(command, helper.Cpfzusuregistro, DbType.String, entity.Cpfzusuregistro);
            dbProvider.AddInParameter(command, helper.Cpfzfecregistro, DbType.DateTime, entity.Cpfzfecregistro);
            dbProvider.AddInParameter(command, helper.Cpfzcodi, DbType.Int32, entity.Cpfzcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpfzcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpfzcodi, DbType.Int32, cpfzcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpForzadoCabDTO GetById(int cpfzcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpfzcodi, DbType.Int32, cpfzcodi);
            CpForzadoCabDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpForzadoCabDTO> List()
        {
            List<CpForzadoCabDTO> entitys = new List<CpForzadoCabDTO>();
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

        public List<CpForzadoCabDTO> GetByCriteria()
        {
            List<CpForzadoCabDTO> entitys = new List<CpForzadoCabDTO>();
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

        public CpForzadoCabDTO GetByDate(DateTime fechaHora)
        {
            var querySql = string.Format(helper.SqlGetByDate, fechaHora.ToString(ConstantesBase.FormatoFecha), fechaHora.Hour);
            DbCommand command = dbProvider.GetSqlStringCommand(querySql);

            CpForzadoCabDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpForzadoCabDTO> GetListByDate(DateTime fechaHora)
        {
            List<CpForzadoCabDTO> entitys = new List<CpForzadoCabDTO>();
            var querySql = string.Format(helper.SqlGetListByDate, fechaHora.ToString(ConstantesBase.FormatoFecha), fechaHora.Hour);
            DbCommand command = dbProvider.GetSqlStringCommand(querySql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

    }
}
