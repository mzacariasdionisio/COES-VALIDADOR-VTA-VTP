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
    /// Clase de acceso a datos de la tabla EPO_INDICADOR
    /// </summary>
    public class EpoIndicadorRepository: RepositoryBase, IEpoIndicadorRepository
    {
        public EpoIndicadorRepository(string strConn): base(strConn)
        {
        }

        EpoIndicadorHelper helper = new EpoIndicadorHelper();

        public int Save(EpoIndicadorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Indcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Indnomb, DbType.String, entity.Indnomb);
            dbProvider.AddInParameter(command, helper.Indmensajeleyenda, DbType.String, entity.Indmensajeleyenda);
            dbProvider.AddInParameter(command, helper.Indformatoescalavalores, DbType.String, entity.Indformatoescalavalores);
            dbProvider.AddInParameter(command, helper.Indintervalo, DbType.Decimal, entity.Indintervalo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EpoIndicadorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Indcodi, DbType.Int32, entity.Indcodi);
            dbProvider.AddInParameter(command, helper.Indnomb, DbType.String, entity.Indnomb);
            dbProvider.AddInParameter(command, helper.Indmensajeleyenda, DbType.String, entity.Indmensajeleyenda);
            dbProvider.AddInParameter(command, helper.Indformatoescalavalores, DbType.String, entity.Indformatoescalavalores);
            dbProvider.AddInParameter(command, helper.Indintervalo, DbType.Decimal, entity.Indintervalo);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int indcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Indcodi, DbType.Int32, indcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EpoIndicadorDTO GetById(int indcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Indcodi, DbType.Int32, indcodi);
            EpoIndicadorDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EpoIndicadorDTO> List()
        {
            List<EpoIndicadorDTO> entitys = new List<EpoIndicadorDTO>();
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

        public List<EpoIndicadorDTO> GetByCriteria()
        {
            List<EpoIndicadorDTO> entitys = new List<EpoIndicadorDTO>();
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
