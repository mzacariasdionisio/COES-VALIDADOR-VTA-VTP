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
    /// Clase de acceso a datos de la tabla PR_CONFIGURACION_POT_EFECTIVA
    /// </summary>
    public class PrConfiguracionPotEfectivaRepository: RepositoryBase, IPrConfiguracionPotEfectivaRepository
    {
        public PrConfiguracionPotEfectivaRepository(string strConn): base(strConn)
        {
        }

        PrConfiguracionPotEfectivaHelper helper = new PrConfiguracionPotEfectivaHelper();

        public void Save(PrConfiguracionPotEfectivaDTO entity)
        {
            DbCommand command  = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Confpeusuariocreacion, DbType.String, entity.Confpeusuariocreacion);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(PrConfiguracionPotEfectivaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Confpeusuariocreacion, DbType.String, entity.Confpeusuariocreacion);
            dbProvider.AddInParameter(command, helper.Confpefechacreacion, DbType.DateTime, entity.Confpefechacreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int grupocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrConfiguracionPotEfectivaDTO GetById(int grupocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            PrConfiguracionPotEfectivaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrConfiguracionPotEfectivaDTO> List()
        {
            List<PrConfiguracionPotEfectivaDTO> entitys = new List<PrConfiguracionPotEfectivaDTO>();
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

        public List<PrConfiguracionPotEfectivaDTO> GetByCriteria()
        {
            List<PrConfiguracionPotEfectivaDTO> entitys = new List<PrConfiguracionPotEfectivaDTO>();
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


        public void DeleteAll()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteAll);
            dbProvider.ExecuteNonQuery(command);
        }

        public void SaveAll(List<PrConfiguracionPotEfectivaDTO> listEntity)
        {
            foreach (var entity in listEntity)
            {
                DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
                dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
                dbProvider.AddInParameter(command, helper.Confpeusuariocreacion, DbType.String, entity.Confpeusuariocreacion);
                dbProvider.ExecuteNonQuery(command);
            }
        }
    }
}
