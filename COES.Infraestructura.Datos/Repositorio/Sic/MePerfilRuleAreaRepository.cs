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
    /// Clase de acceso a datos de la tabla ME_PERFIL_RULE_AREA
    /// </summary>
    public class MePerfilRuleAreaRepository: RepositoryBase, IMePerfilRuleAreaRepository
    {
        public MePerfilRuleAreaRepository(string strConn): base(strConn)
        {
        }

        MePerfilRuleAreaHelper helper = new MePerfilRuleAreaHelper();

        public void Save(MePerfilRuleAreaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, entity.Areacode);
            dbProvider.AddInParameter(command, helper.Prrucodi, DbType.Int32, entity.Prrucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(MePerfilRuleAreaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, entity.Areacode);
            dbProvider.AddInParameter(command, helper.Prrucodi, DbType.Int32, entity.Prrucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int prrucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
                        
            dbProvider.AddInParameter(command, helper.Prrucodi, DbType.Int32, prrucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MePerfilRuleAreaDTO GetById(int areacode, int prrucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, areacode);
            dbProvider.AddInParameter(command, helper.Prrucodi, DbType.Int32, prrucodi);
            MePerfilRuleAreaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MePerfilRuleAreaDTO> List()
        {
            List<MePerfilRuleAreaDTO> entitys = new List<MePerfilRuleAreaDTO>();
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

        public List<MePerfilRuleAreaDTO> GetByCriteria(int id)
        {
            List<MePerfilRuleAreaDTO> entitys = new List<MePerfilRuleAreaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Prrucodi, DbType.Int32, id);

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
