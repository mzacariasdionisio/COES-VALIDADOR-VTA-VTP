using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Scada;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Scada;
using System.Data.Odbc;

namespace COES.Infraestructura.Datos.Repositorio.Scada
{
    /// <summary>
    /// Clase de acceso a datos de la tabla ME_SCADA_PTOFILTRO_SP7
    /// </summary>
    public class MeScadaPtofiltroSp7Repository: RepositoryBase, IMeScadaPtofiltroSp7Repository
    {
        public MeScadaPtofiltroSp7Repository(string strConn): base(strConn)
        {
        }

        MeScadaPtofiltroSp7Helper helper = new MeScadaPtofiltroSp7Helper();

        public int Save(MeScadaPtofiltroSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Scdpficodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Filtrocodi, DbType.Int32, entity.Filtrocodi);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Scdpfiusucreacion, DbType.String, entity.Scdpfiusucreacion);
            dbProvider.AddInParameter(command, helper.Scdpfifeccreacion, DbType.DateTime, entity.Scdpfifeccreacion);
            dbProvider.AddInParameter(command, helper.Scdpfiusumodificacion, DbType.String, entity.Scdpfiusumodificacion);
            dbProvider.AddInParameter(command, helper.Scdpfifecmodificacion, DbType.DateTime, entity.Scdpfifecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MeScadaPtofiltroSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Filtrocodi, DbType.Int32, entity.Filtrocodi);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Scdpfiusucreacion, DbType.String, entity.Scdpfiusucreacion);
            dbProvider.AddInParameter(command, helper.Scdpfifeccreacion, DbType.DateTime, entity.Scdpfifeccreacion);
            dbProvider.AddInParameter(command, helper.Scdpfiusumodificacion, DbType.String, entity.Scdpfiusumodificacion);
            dbProvider.AddInParameter(command, helper.Scdpfifecmodificacion, DbType.DateTime, entity.Scdpfifecmodificacion);
            dbProvider.AddInParameter(command, helper.Scdpficodi, DbType.Int32, entity.Scdpficodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int scdpficodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Scdpficodi, DbType.Int32, scdpficodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteFiltro(int filtrocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteFiltro);

            dbProvider.AddInParameter(command, helper.Filtrocodi, DbType.Int32, filtrocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        

        public MeScadaPtofiltroSp7DTO GetById(int scdpficodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Scdpficodi, DbType.Int32, scdpficodi);
            MeScadaPtofiltroSp7DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeScadaPtofiltroSp7DTO> List()
        {
            List<MeScadaPtofiltroSp7DTO> entitys = new List<MeScadaPtofiltroSp7DTO>();
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

        public List<MeScadaPtofiltroSp7DTO> GetByCriteria()
        {
            List<MeScadaPtofiltroSp7DTO> entitys = new List<MeScadaPtofiltroSp7DTO>();
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
        /// <summary>
        /// Graba los datos de la tabla ME_SCADA_PTOFILTRO_SP7
        /// </summary>
        public int SaveMeScadaPtofiltroSp7Id(MeScadaPtofiltroSp7DTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Scdpficodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Scdpficodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }


    }
}
