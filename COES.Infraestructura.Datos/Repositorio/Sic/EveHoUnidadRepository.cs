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
    /// Clase de acceso a datos de la tabla EVE_HO_UNIDAD
    /// </summary>
    public class EveHoUnidadRepository : RepositoryBase, IEveHoUnidadRepository
    {
        public EveHoUnidadRepository(string strConn) : base(strConn)
        {
        }

        EveHoUnidadHelper helper = new EveHoUnidadHelper();

        public int Save(EveHoUnidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;
            dbProvider.AddInParameter(command, helper.Hopunicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Hopcodi, DbType.Int32, entity.Hopcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Hopunihorordarranq, DbType.DateTime, entity.Hopunihorordarranq);
            dbProvider.AddInParameter(command, helper.Hopunihorini, DbType.DateTime, entity.Hopunihorini);
            dbProvider.AddInParameter(command, helper.Hopunihorfin, DbType.DateTime, entity.Hopunihorfin);
            dbProvider.AddInParameter(command, helper.Hopunihorarranq, DbType.DateTime, entity.Hopunihorarranq);
            dbProvider.AddInParameter(command, helper.Hopunihorparada, DbType.DateTime, entity.Hopunihorparada);
            dbProvider.AddInParameter(command, helper.Hopuniusucreacion, DbType.String, entity.Hopuniusucreacion);
            dbProvider.AddInParameter(command, helper.Hopunifeccreacion, DbType.DateTime, entity.Hopunifeccreacion);
            dbProvider.AddInParameter(command, helper.Hopuniusumodificacion, DbType.String, entity.Hopuniusumodificacion);
            dbProvider.AddInParameter(command, helper.Hopunifecmodificacion, DbType.DateTime, entity.Hopunifecmodificacion);
            dbProvider.AddInParameter(command, helper.Hopuniactivo, DbType.Int32, entity.Hopuniactivo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveHoUnidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            dbProvider.AddInParameter(command, helper.Hopcodi, DbType.Int32, entity.Hopcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Hopunihorordarranq, DbType.DateTime, entity.Hopunihorordarranq);
            dbProvider.AddInParameter(command, helper.Hopunihorini, DbType.DateTime, entity.Hopunihorini);
            dbProvider.AddInParameter(command, helper.Hopunihorfin, DbType.DateTime, entity.Hopunihorfin);
            dbProvider.AddInParameter(command, helper.Hopunihorarranq, DbType.DateTime, entity.Hopunihorarranq);
            dbProvider.AddInParameter(command, helper.Hopunihorparada, DbType.DateTime, entity.Hopunihorparada);
            dbProvider.AddInParameter(command, helper.Hopuniusumodificacion, DbType.String, entity.Hopuniusumodificacion);
            dbProvider.AddInParameter(command, helper.Hopunifecmodificacion, DbType.DateTime, entity.Hopunifecmodificacion);
            dbProvider.AddInParameter(command, helper.Hopuniactivo, DbType.Int32, entity.Hopuniactivo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.AddInParameter(command, helper.Hopunicodi, DbType.Int32, entity.Hopunicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int hopunicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Hopunicodi, DbType.Int32, hopunicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveHoUnidadDTO GetById(int hopunicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Hopunicodi, DbType.Int32, hopunicodi);
            EveHoUnidadDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveHoUnidadDTO> List()
        {
            List<EveHoUnidadDTO> entitys = new List<EveHoUnidadDTO>();
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

        public List<EveHoUnidadDTO> GetByCriteria()
        {
            List<EveHoUnidadDTO> entitys = new List<EveHoUnidadDTO>();
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
    }
}
