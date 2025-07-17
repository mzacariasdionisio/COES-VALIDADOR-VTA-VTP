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
    /// Clase de acceso a datos de la tabla SMA_CONFIGURACION
    /// </summary>
    public class SmaConfiguracionRepository: RepositoryBase, ISmaConfiguracionRepository
    {
        public SmaConfiguracionRepository(string strConn): base(strConn)
        {
        }

        SmaConfiguracionHelper helper = new SmaConfiguracionHelper();

        public int Save(SmaConfiguracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Confsmcorrelativo, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Confsmatributo, DbType.String, entity.Confsmatributo);
            dbProvider.AddInParameter(command, helper.Confsmparametro, DbType.String, entity.Confsmparametro);
            dbProvider.AddInParameter(command, helper.Confsmvalor, DbType.String, entity.Confsmvalor);
            dbProvider.AddInParameter(command, helper.Confsmusucreacion, DbType.String, entity.Confsmusucreacion);
            
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SmaConfiguracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Confsmatributo, DbType.String, entity.Confsmatributo);
            dbProvider.AddInParameter(command, helper.Confsmparametro, DbType.String, entity.Confsmparametro);
            dbProvider.AddInParameter(command, helper.Confsmvalor, DbType.String, entity.Confsmvalor);
            dbProvider.AddInParameter(command, helper.Confsmusumodificacion, DbType.String, entity.Confsmusumodificacion);
            dbProvider.AddInParameter(command, helper.Confsmcorrelativo, DbType.Int32, entity.Confsmcorrelativo);


            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(string user, int confsmcorrelativo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Confsmusumodificacion, DbType.String, user);
            dbProvider.AddInParameter(command, helper.Confsmcorrelativo, DbType.Int32, confsmcorrelativo);

            dbProvider.ExecuteNonQuery(command);
        }

        public SmaConfiguracionDTO GetById(int confsmcorrelativo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Confsmcorrelativo, DbType.Int32, confsmcorrelativo);
            SmaConfiguracionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SmaConfiguracionDTO> List()
        {
            List<SmaConfiguracionDTO> entitys = new List<SmaConfiguracionDTO>();
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

        public List<SmaConfiguracionDTO> GetByAtributo(string atributo)
        {
            List<SmaConfiguracionDTO> entitys = new List<SmaConfiguracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByAtributo);
            dbProvider.AddInParameter(command, helper.Confsmatributo, DbType.String, atributo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateAtributos(dr));
                }
            }

            return entitys;
        }

        public SmaConfiguracionDTO GetValor(string confsmatributo, string confsmparametro)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetValor);

            dbProvider.AddInParameter(command, helper.Confsmatributo, DbType.String, confsmatributo);
            dbProvider.AddInParameter(command, helper.Confsmparametro, DbType.String, confsmparametro);
            SmaConfiguracionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;

        }

        public SmaConfiguracionDTO GetValorxID(int correlativo)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetValorxID);

            dbProvider.AddInParameter(command, helper.Confsmcorrelativo, DbType.Int32, correlativo);
            SmaConfiguracionDTO entity = null;

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
