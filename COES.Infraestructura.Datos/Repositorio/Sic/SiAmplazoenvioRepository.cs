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
    /// Clase de acceso a datos de la tabla SI_AMPLAZOENVIO
    /// </summary>
    public class SiAmplazoenvioRepository : RepositoryBase, ISiAmplazoenvioRepository
    {
        public SiAmplazoenvioRepository(string strConn)
            : base(strConn)
        {
        }

        SiAmplazoenvioHelper helper = new SiAmplazoenvioHelper();

        public int Save(SiAmplazoenvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Amplzcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Fdatcodi, DbType.Int32, entity.Fdatcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Amplzfecha, DbType.DateTime, entity.Amplzfecha);
            dbProvider.AddInParameter(command, helper.Amplzfechaperiodo, DbType.DateTime, entity.Amplzfechaperiodo);
            dbProvider.AddInParameter(command, helper.Amplzusucreacion, DbType.String, entity.Amplzusucreacion);
            dbProvider.AddInParameter(command, helper.Amplzfeccreacion, DbType.DateTime, entity.Amplzfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiAmplazoenvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Amplzfecha, DbType.DateTime, entity.Amplzfecha);
            dbProvider.AddInParameter(command, helper.Amplzusumodificacion, DbType.String, entity.Amplzusumodificacion);
            dbProvider.AddInParameter(command, helper.Amplzfecmodificacion, DbType.DateTime, entity.Amplzfecmodificacion);

            dbProvider.AddInParameter(command, helper.Amplzcodi, DbType.Int32, entity.Amplzcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int amplzcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Amplzcodi, DbType.Int32, amplzcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiAmplazoenvioDTO GetById(int amplzcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Amplzcodi, DbType.Int32, amplzcodi);
            SiAmplazoenvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiAmplazoenvioDTO> List()
        {
            List<SiAmplazoenvioDTO> entitys = new List<SiAmplazoenvioDTO>();
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

        public List<SiAmplazoenvioDTO> GetByCriteria()
        {
            List<SiAmplazoenvioDTO> entitys = new List<SiAmplazoenvioDTO>();
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

        public List<SiAmplazoenvioDTO> GetListaMultiple(DateTime fechaIni, DateTime fechaFin, string sEmpresa, string fdatcodi)
        {
            List<SiAmplazoenvioDTO> entitys = new List<SiAmplazoenvioDTO>();
            string queryString = string.Format(helper.SqlListarAmpliacionMultiple, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha)
                , sEmpresa, fdatcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiAmplazoenvioDTO entity = helper.Create(dr);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iFdatnombre = dr.GetOrdinal(helper.Fdatnombre);
                    if (!dr.IsDBNull(iFdatnombre)) entity.Fdatnombre = dr.GetString(iFdatnombre);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public SiAmplazoenvioDTO GetByIdCriteria(DateTime fecha, int empresa, int fdatcodi)
        {
            string queryString = string.Format(helper.SqlGetByIdCriteria, fecha.ToString(ConstantesBase.FormatoFecha), empresa, fdatcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            SiAmplazoenvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }
            return entity;
        }
    }
}
