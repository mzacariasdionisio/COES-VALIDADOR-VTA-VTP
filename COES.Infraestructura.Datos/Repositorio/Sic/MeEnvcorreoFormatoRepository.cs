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
    /// Clase de acceso a datos de la tabla ME_ENVCORREO_FORMATO
    /// </summary>
    public class MeEnvcorreoFormatoRepository: RepositoryBase, IMeEnvcorreoFormatoRepository
    {
        public MeEnvcorreoFormatoRepository(string strConn): base(strConn)
        {
        }

        MeEnvcorreoFormatoHelper helper = new MeEnvcorreoFormatoHelper();

        public int Save(MeEnvcorreoFormatoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ecformcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ecformhabilitado, DbType.String, entity.Ecformhabilitado);
            dbProvider.AddInParameter(command, helper.Ecformusucreacion, DbType.String, entity.Ecformusucreacion);
            dbProvider.AddInParameter(command, helper.Ecformfeccreacion, DbType.DateTime, entity.Ecformfeccreacion);
            dbProvider.AddInParameter(command, helper.Ecformusumodificacion, DbType.String, entity.Ecformusumodificacion);
            dbProvider.AddInParameter(command, helper.Ecformfecmodificacion, DbType.DateTime, entity.Ecformfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MeEnvcorreoFormatoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ecformhabilitado, DbType.String, entity.Ecformhabilitado);
            dbProvider.AddInParameter(command, helper.Ecformusucreacion, DbType.String, entity.Ecformusucreacion);
            dbProvider.AddInParameter(command, helper.Ecformfeccreacion, DbType.DateTime, entity.Ecformfeccreacion);
            dbProvider.AddInParameter(command, helper.Ecformusumodificacion, DbType.String, entity.Ecformusumodificacion);
            dbProvider.AddInParameter(command, helper.Ecformfecmodificacion, DbType.DateTime, entity.Ecformfecmodificacion);
            dbProvider.AddInParameter(command, helper.Ecformcodi, DbType.Int32, entity.Ecformcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int idEmpresa, string formatos)
        {
            string sql = string.Format(helper.SqlDelete, idEmpresa, formatos);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);           

            dbProvider.ExecuteNonQuery(command);
        }

        public MeEnvcorreoFormatoDTO GetById(int ecformcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ecformcodi, DbType.Int32, ecformcodi);
            MeEnvcorreoFormatoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeEnvcorreoFormatoDTO> List()
        {
            List<MeEnvcorreoFormatoDTO> entitys = new List<MeEnvcorreoFormatoDTO>();
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

        public List<MeEnvcorreoFormatoDTO> GetByCriteria()
        {
            List<MeEnvcorreoFormatoDTO> entitys = new List<MeEnvcorreoFormatoDTO>();
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

        public List<MeEnvcorreoFormatoDTO> ObtenerCorreoEmpresa()
        {
            List<MeEnvcorreoFormatoDTO> entitys = new List<MeEnvcorreoFormatoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEmpresas);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeEnvcorreoFormatoDTO entity = new MeEnvcorreoFormatoDTO();

                    int iUseremail = dr.GetOrdinal(helper.Useremail);
                    if (dr.IsDBNull(iUseremail)) entity.Empremail = dr.GetString(iUseremail);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iModcodi = dr.GetOrdinal(helper.Modcodi);
                    if (dr.IsDBNull(iModcodi)) entity.Modcodi = Convert.ToInt32(dr.GetValue(iModcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
