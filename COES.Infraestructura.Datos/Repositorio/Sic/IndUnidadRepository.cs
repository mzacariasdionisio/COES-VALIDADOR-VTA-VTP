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
    /// Clase de acceso a datos de la tabla IND_UNIDAD
    /// </summary>
    public class IndUnidadRepository : RepositoryBase, IIndUnidadRepository
    {
        public IndUnidadRepository(string strConn) : base(strConn)
        {
        }

        IndUnidadHelper helper = new IndUnidadHelper();

        public int Save(IndUnidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Iunicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre);
            dbProvider.AddInParameter(command, helper.Iuniunidadnomb, DbType.String, entity.Iuniunidadnomb);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Iuninombcentral, DbType.String, entity.Iuninombcentral);
            dbProvider.AddInParameter(command, helper.Iuninombunidad, DbType.String, entity.Iuninombunidad);
            dbProvider.AddInParameter(command, helper.Iuniactivo, DbType.Int32, entity.Iuniactivo);
            dbProvider.AddInParameter(command, helper.Iuniusucreacion, DbType.String, entity.Iuniusucreacion);
            dbProvider.AddInParameter(command, helper.Iunifeccreacion, DbType.DateTime, entity.Iunifeccreacion);
            dbProvider.AddInParameter(command, helper.Iuniusumodificacion, DbType.String, entity.Iuniusumodificacion);
            dbProvider.AddInParameter(command, helper.Iunifecmodificacion, DbType.DateTime, entity.Iunifecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(IndUnidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Iuninombcentral, DbType.String, entity.Iuninombcentral);
            dbProvider.AddInParameter(command, helper.Iuninombunidad, DbType.String, entity.Iuninombunidad);
            dbProvider.AddInParameter(command, helper.Iuniactivo, DbType.Int32, entity.Iuniactivo);
            dbProvider.AddInParameter(command, helper.Iuniusumodificacion, DbType.String, entity.Iuniusumodificacion);
            dbProvider.AddInParameter(command, helper.Iunifecmodificacion, DbType.DateTime, entity.Iunifecmodificacion);
            dbProvider.AddInParameter(command, helper.Iunicodi, DbType.Int32, entity.Iunicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int iunicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Iunicodi, DbType.Int32, iunicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IndUnidadDTO GetById(int iunicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Iunicodi, DbType.Int32, iunicodi);
            IndUnidadDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<IndUnidadDTO> List()
        {
            List<IndUnidadDTO> entitys = new List<IndUnidadDTO>();
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

        public List<IndUnidadDTO> GetByCriteria()
        {
            List<IndUnidadDTO> entitys = new List<IndUnidadDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
