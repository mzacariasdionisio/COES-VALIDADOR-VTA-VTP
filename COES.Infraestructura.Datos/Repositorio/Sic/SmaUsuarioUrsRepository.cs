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
    /// Clase de acceso a datos de la tabla SMA_USUARIO_URS
    /// </summary>
    public class SmaUsuarioUrsRepository: RepositoryBase, ISmaUsuarioUrsRepository
    {
        public SmaUsuarioUrsRepository(string strConn): base(strConn)
        {
        }

        SmaUsuarioUrsHelper helper = new SmaUsuarioUrsHelper();

        public int Save(SmaUsuarioUrsDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Uurscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Urscodi, DbType.Int32, entity.Urscodi);
            dbProvider.AddInParameter(command, helper.Uursusucreacion, DbType.String, entity.Uursusucreacion);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SmaUsuarioUrsDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Urscodi, DbType.Int32, entity.Urscodi);
            dbProvider.AddInParameter(command, helper.Uursusumodificacion, DbType.String, entity.Uursusumodificacion);
            dbProvider.AddInParameter(command, helper.Uurscodi, DbType.Int32, entity.Uurscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateUsuAct(int uurscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Uurscodi, DbType.Int32, uurscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int uurscodi, string user)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Uursusumodificacion, DbType.String, user);
            dbProvider.AddInParameter(command, helper.Uurscodi, DbType.Int32, uurscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SmaUsuarioUrsDTO GetById(int uurscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Uurscodi, DbType.Int32, uurscodi);
            SmaUsuarioUrsDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.CreateList(dr);
                }
            }

            return entity;
        }

        public List<SmaUsuarioUrsDTO> List()
        {
            List<SmaUsuarioUrsDTO> entitys = new List<SmaUsuarioUrsDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.CreateList(dr);

                    int iUserlogin = dr.GetOrdinal(helper.Userlogin);
                    if (!dr.IsDBNull(iUserlogin)) entity.Userlogin = dr.GetString(iUserlogin);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SmaUsuarioUrsDTO> GetByCriteria(int usercode)
        {
            List<SmaUsuarioUrsDTO> entitys = new List<SmaUsuarioUrsDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, usercode);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateList(dr));
                }
            }

            return entitys;
        }

        public List<SmaUsuarioUrsDTO> GetUsuUrsAct(SmaUsuarioUrsDTO entity, string estado)
        {
            List<SmaUsuarioUrsDTO> entitys = new List<SmaUsuarioUrsDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListGetUsuUrsAct);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Urscodi, DbType.Int32, entity.Urscodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateList(dr));
                }
            }

            return entitys;
        }

        public List<SmaUsuarioUrsDTO> GetByCriteriaMO(int usercode)
        {
            List<SmaUsuarioUrsDTO> entitys = new List<SmaUsuarioUrsDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaMO);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, usercode);

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
