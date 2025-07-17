using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla RNT_CONFIGURACION
    /// </summary>
    public class RntConfiguracionRepository : RepositoryBase, IRntConfiguracionRepository
    {
        public RntConfiguracionRepository(string strConn)
            : base(strConn)
        {
        }

        RntConfiguracionHelper helper = new RntConfiguracionHelper();

        public int Save(RntConfiguracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Confatributo, DbType.String, entity.ConfAtributo);
            dbProvider.AddInParameter(command, helper.Confparametro, DbType.String, entity.ConfParametro);
            dbProvider.AddInParameter(command, helper.Confvalor, DbType.String, entity.ConfValor);
            dbProvider.AddInParameter(command, helper.Confestado, DbType.Int32, entity.ConfEstado);
            dbProvider.AddInParameter(command, helper.Confusuariocreacion, DbType.String, entity.ConfUsuarioCreacion);
            dbProvider.AddInParameter(command, helper.Conffechacreacion, DbType.DateTime, entity.ConfFechaCreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RntConfiguracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Confatributo, DbType.String, entity.ConfAtributo);
            dbProvider.AddInParameter(command, helper.Confparametro, DbType.String, entity.ConfParametro);
            dbProvider.AddInParameter(command, helper.Confvalor, DbType.String, entity.ConfValor);
            dbProvider.AddInParameter(command, helper.Confestado, DbType.Int32, entity.ConfEstado);
            dbProvider.AddInParameter(command, helper.Confusuarioupdate, DbType.String, entity.ConfUsuarioUpdate);
            dbProvider.AddInParameter(command, helper.Conffechaupdate, DbType.DateTime, entity.ConfFechaUpdate);
            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, entity.ConfigCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int configcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, configcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RntConfiguracionDTO GetById(int configcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, configcodi);
            RntConfiguracionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RntConfiguracionDTO> List()
        {
            List<RntConfiguracionDTO> entitys = new List<RntConfiguracionDTO>();
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
        public List<RntConfiguracionDTO> ListNivelTension()
        {
            List<RntConfiguracionDTO> entitys = new List<RntConfiguracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListNivelTension);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<RntConfiguracionDTO> GetListParametroRep(string atributo, string parametro)
        {
            List<RntConfiguracionDTO> entitys = new List<RntConfiguracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetListParametroRep);

            dbProvider.AddInParameter(command, helper.Confatributo, DbType.String, atributo);
            dbProvider.AddInParameter(command, helper.Confparametro, DbType.String, parametro);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        public RntConfiguracionDTO ListParametroRep(string parametro, string valor)
        {
            RntConfiguracionDTO entity = new RntConfiguracionDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListParametroRep);
            dbProvider.AddInParameter(command, helper.Confparametro, DbType.String, parametro);
            dbProvider.AddInParameter(command, helper.Confvalor, DbType.String, valor);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
        public List<RntConfiguracionDTO> ListComboNivelTension()
        {
            List<RntConfiguracionDTO> entitys = new List<RntConfiguracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListComboNivelTension);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<RntConfiguracionDTO> ListComboConfiguracion(string parametro)
        {
            List<RntConfiguracionDTO> entitys = new List<RntConfiguracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListComboConfiguracion);
            dbProvider.AddInParameter(command, helper.Confparametro, DbType.String, parametro);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<RntConfiguracionDTO> GetByCriteria()
        {
            List<RntConfiguracionDTO> entitys = new List<RntConfiguracionDTO>();
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

        public List<RntConfiguracionDTO> GetComboParametro(string atributo)
        {
            List<RntConfiguracionDTO> entitys = new List<RntConfiguracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetComboParametro);
            dbProvider.AddInParameter(command, helper.Confatributo, DbType.String, atributo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.ListCombo(dr));
                }
            }

            return entitys;
        }
    }
}
