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
    /// Clase de acceso a datos de la tabla PR_ESCENARIO
    /// </summary>
    public class PrEscenarioRepository: RepositoryBase, IPrEscenarioRepository
    {
        public PrEscenarioRepository(string strConn): base(strConn)
        {
        }

        PrEscenarioHelper helper = new PrEscenarioHelper();

        public int Save(PrEscenarioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Escecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Escefecha, DbType.DateTime, entity.Escefecha);
            dbProvider.AddInParameter(command, helper.Escenum, DbType.Int32, entity.Escenum);
            dbProvider.AddInParameter(command, helper.Escenomb, DbType.String, entity.Escenomb);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrEscenarioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Escefecha, DbType.DateTime, entity.Escefecha);
            dbProvider.AddInParameter(command, helper.Escecodi, DbType.Int32, entity.Escecodi);
            dbProvider.AddInParameter(command, helper.Escenum, DbType.Int32, entity.Escenum);
            dbProvider.AddInParameter(command, helper.Escenomb, DbType.String, entity.Escenomb);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int escecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Escecodi, DbType.Int32, escecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrEscenarioDTO GetById(int escecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Escecodi, DbType.Int32, escecodi);
            PrEscenarioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrEscenarioDTO> List()
        {
            List<PrEscenarioDTO> entitys = new List<PrEscenarioDTO>();
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

        public List<PrEscenarioDTO> GetByCriteria()
        {
            List<PrEscenarioDTO> entitys = new List<PrEscenarioDTO>();
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


        public List<PrEscenarioDTO> GetEscenariosPorFechaRepCv(DateTime fecha)
        {
            List<PrEscenarioDTO> entitys = new List<PrEscenarioDTO>();
            string sComando = String.Format(helper.SqlEscenarioPorFecha, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sComando);

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
