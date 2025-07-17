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
    /// Clase de acceso a datos de la tabla TRN_MEDBORNE
    /// </summary>
    public class TrnMedborneRepository: RepositoryBase, ITrnMedborneRepository
    {
        public TrnMedborneRepository(string strConn): base(strConn)
        {
        }

        TrnMedborneHelper helper = new TrnMedborneHelper();

        public int Save(TrnMedborneDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Trnmebcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Trnmebversion, DbType.Int32, entity.Trnmebversion);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Trnmebfecha, DbType.DateTime, entity.Trnmebfecha);
            dbProvider.AddInParameter(command, helper.Trnmebptomed, DbType.String, entity.Trnmebptomed);
            dbProvider.AddInParameter(command, helper.Trnmebenergia, DbType.Decimal, entity.Trnmebenergia);
            dbProvider.AddInParameter(command, helper.Trnmebusucreacion, DbType.String, entity.Trnmebusucreacion);
            dbProvider.AddInParameter(command, helper.Trnmebfeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(TrnMedborneDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            //set
            dbProvider.AddInParameter(command, helper.Trnmebenergia, DbType.Decimal, entity.Trnmebenergia);
            //where
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Trnmebversion, DbType.Int32, entity.Trnmebversion);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Trnmebfecha, DbType.DateTime, entity.Trnmebfecha);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pericodi, int version)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Trnmebversion, DbType.Int32, version);

            dbProvider.ExecuteNonQuery(command);
        }

        public TrnMedborneDTO GetById(int trnmebcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Trnmebcodi, DbType.Int32, trnmebcodi);
            TrnMedborneDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TrnMedborneDTO> List()
        {
            List<TrnMedborneDTO> entitys = new List<TrnMedborneDTO>();
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

        public List<TrnMedborneDTO> GetByCriteria()
        {
            List<TrnMedborneDTO> entitys = new List<TrnMedborneDTO>();
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
