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
    /// Clase de acceso a datos de la tabla MP_PROPIEDAD
    /// </summary>
    public class MpPropiedadRepository: RepositoryBase, IMpPropiedadRepository
    {
        public MpPropiedadRepository(string strConn): base(strConn)
        {
        }

        MpPropiedadHelper helper = new MpPropiedadHelper();

        public int Save(MpPropiedadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mpropcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Mcatcodi, DbType.Int32, entity.Mcatcodi);
            dbProvider.AddInParameter(command, helper.Mpropnombre, DbType.String, entity.Mpropnombre);
            dbProvider.AddInParameter(command, helper.Mpropabrev, DbType.String, entity.Mpropabrev);
            dbProvider.AddInParameter(command, helper.Mpropunidad, DbType.String, entity.Mpropunidad);
            dbProvider.AddInParameter(command, helper.Mproporden, DbType.Int32, entity.Mproporden);
            dbProvider.AddInParameter(command, helper.Mproptipo, DbType.String, entity.Mproptipo);
            dbProvider.AddInParameter(command, helper.Mpropcodisicoes, DbType.Int32, entity.Mpropcodisicoes);
            dbProvider.AddInParameter(command, helper.Mpropcodisicoes2, DbType.Int32, entity.Mpropcodisicoes2);
            dbProvider.AddInParameter(command, helper.Mpropusumodificacion, DbType.String, entity.Mpropusumodificacion);
            dbProvider.AddInParameter(command, helper.Mpropfecmodificacion, DbType.DateTime, entity.Mpropfecmodificacion);
            dbProvider.AddInParameter(command, helper.Mpropancho, DbType.Int32, entity.Mpropancho);
            dbProvider.AddInParameter(command, helper.Mpropvalordefault, DbType.String, entity.Mpropvalordefault);
            dbProvider.AddInParameter(command, helper.Mpropvalordefault2, DbType.String, entity.Mpropvalordefault2);
            dbProvider.AddInParameter(command, helper.Mpropprioridad, DbType.Int32, entity.Mpropprioridad);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MpPropiedadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mpropcodi, DbType.Int32, entity.Mpropcodi);
            dbProvider.AddInParameter(command, helper.Mcatcodi, DbType.Int32, entity.Mcatcodi);
            dbProvider.AddInParameter(command, helper.Mpropnombre, DbType.String, entity.Mpropnombre);
            dbProvider.AddInParameter(command, helper.Mpropabrev, DbType.String, entity.Mpropabrev);
            dbProvider.AddInParameter(command, helper.Mpropunidad, DbType.String, entity.Mpropunidad);
            dbProvider.AddInParameter(command, helper.Mproporden, DbType.Int32, entity.Mproporden);
            dbProvider.AddInParameter(command, helper.Mproptipo, DbType.String, entity.Mproptipo);
            dbProvider.AddInParameter(command, helper.Mpropcodisicoes, DbType.Int32, entity.Mpropcodisicoes);
            dbProvider.AddInParameter(command, helper.Mpropcodisicoes2, DbType.Int32, entity.Mpropcodisicoes2);
            dbProvider.AddInParameter(command, helper.Mpropusumodificacion, DbType.String, entity.Mpropusumodificacion);
            dbProvider.AddInParameter(command, helper.Mpropfecmodificacion, DbType.DateTime, entity.Mpropfecmodificacion);
            dbProvider.AddInParameter(command, helper.Mpropancho, DbType.Int32, entity.Mpropancho);
            dbProvider.AddInParameter(command, helper.Mpropvalordefault, DbType.String, entity.Mpropvalordefault);
            dbProvider.AddInParameter(command, helper.Mpropvalordefault2, DbType.String, entity.Mpropvalordefault2);
            dbProvider.AddInParameter(command, helper.Mpropprioridad, DbType.Int32, entity.Mpropprioridad);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mpropcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mpropcodi, DbType.Int32, mpropcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MpPropiedadDTO GetById(int mpropcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mpropcodi, DbType.Int32, mpropcodi);
            MpPropiedadDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MpPropiedadDTO> List()
        {
            List<MpPropiedadDTO> entitys = new List<MpPropiedadDTO>();
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

        public List<MpPropiedadDTO> GetByCriteria()
        {
            List<MpPropiedadDTO> entitys = new List<MpPropiedadDTO>();
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
