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
    /// Clase de acceso a datos de la tabla SI_TIPOEMPRESA
    /// </summary>
    public class SiTipoempresaRepository: RepositoryBase, ISiTipoempresaRepository
    {
        public SiTipoempresaRepository(string strConn): base(strConn)
        {
        }

        SiTipoempresaHelper helper = new SiTipoempresaHelper();

        public int Save(SiTipoempresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tipoemprdesc, DbType.String, entity.Tipoemprdesc);
            dbProvider.AddInParameter(command, helper.Tipoemprabrev, DbType.String, entity.Tipoemprabrev);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiTipoempresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, entity.Tipoemprcodi);
            dbProvider.AddInParameter(command, helper.Tipoemprdesc, DbType.String, entity.Tipoemprdesc);
            dbProvider.AddInParameter(command, helper.Tipoemprabrev, DbType.String, entity.Tipoemprabrev);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tipoemprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, tipoemprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiTipoempresaDTO GetById(int tipoemprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, tipoemprcodi);
            SiTipoempresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiTipoempresaDTO> List()
        {
            List<SiTipoempresaDTO> entitys = new List<SiTipoempresaDTO>();
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

        public List<SiTipoempresaDTO> ObtenerTiposEmpresaContacto()
        {
            List<SiTipoempresaDTO> entitys = new List<SiTipoempresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerTiposEmpresaContacto);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SiTipoempresaDTO> GetByCriteria()
        {
            List<SiTipoempresaDTO> entitys = new List<SiTipoempresaDTO>();
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
