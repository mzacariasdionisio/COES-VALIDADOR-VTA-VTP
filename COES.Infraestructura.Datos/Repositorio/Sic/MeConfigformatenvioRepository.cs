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
    /// Clase de acceso a datos de la tabla ME_CONFIGFORMATENVIO
    /// </summary>
    public class MeConfigformatenvioRepository : RepositoryBase, IMeConfigformatenvioRepository
    {
        public MeConfigformatenvioRepository(string strConn)
            : base(strConn)
        {
        }

        MeConfigformatenvioHelper helper = new MeConfigformatenvioHelper();


        public int Save(MeConfigformatenvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Cfgenvcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cfgenvptos, DbType.String, entity.Cfgenvptos);
            dbProvider.AddInParameter(command, helper.Cfgenvorden, DbType.String, entity.Cfgenvorden);
            dbProvider.AddInParameter(command, helper.Cfgenvfecha, DbType.DateTime, entity.Cfgenvfecha);
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Cfgenvtipoinf, DbType.String, entity.Cfgenvtipoinf);
            dbProvider.AddInParameter(command, helper.Cfgenvhojas, DbType.String, entity.Cfgenvhojas);
            dbProvider.AddInParameter(command, helper.Cfgenvtipopto, DbType.String, entity.Cfgenvtipopto);
            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


            dbProvider.ExecuteNonQuery(command);
        }

        public MeConfigformatenvioDTO GetById(int idCfgenv)
        {
            string strSql = string.Format(helper.SqlGetById, idCfgenv);
            DbCommand command = dbProvider.GetSqlStringCommand(strSql);

            MeConfigformatenvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeConfigformatenvioDTO> List()
        {
            List<MeConfigformatenvioDTO> entitys = new List<MeConfigformatenvioDTO>();
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

        public List<MeConfigformatenvioDTO> GetByCriteria(int idEmpresa, int idFormato)
        {
            string strSql = string.Format(helper.SqlGetByCriteria, idFormato, idEmpresa);
            List<MeConfigformatenvioDTO> entitys = new List<MeConfigformatenvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(strSql);

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
