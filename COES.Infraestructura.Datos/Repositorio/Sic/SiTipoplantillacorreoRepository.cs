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
    /// Clase de acceso a datos de la tabla SI_TIPOPLANTILLACORREO
    /// </summary>
    public class SiTipoplantillacorreoRepository: RepositoryBase, ISiTipoplantillacorreoRepository
    {
        public SiTipoplantillacorreoRepository(string strConn): base(strConn)
        {
        }

        SiTipoplantillacorreoHelper helper = new SiTipoplantillacorreoHelper();              
        
        public void Update(SiTipoplantillacorreoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tpcorrcodi, DbType.Int32, entity.Tpcorrcodi);
            dbProvider.AddInParameter(command, helper.Tpcorrdescrip, DbType.String, entity.Tpcorrdescrip);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tpcorrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tpcorrcodi, DbType.Int32, tpcorrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiTipoplantillacorreoDTO GetById(int tpcorrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tpcorrcodi, DbType.Int32, tpcorrcodi);
            SiTipoplantillacorreoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiTipoplantillacorreoDTO> List()
        {
            List<SiTipoplantillacorreoDTO> entitys = new List<SiTipoplantillacorreoDTO>();
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

        public List<SiTipoplantillacorreoDTO> GetByCriteria()
        {
            List<SiTipoplantillacorreoDTO> entitys = new List<SiTipoplantillacorreoDTO>();
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
