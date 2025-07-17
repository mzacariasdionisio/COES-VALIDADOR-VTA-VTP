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
    /// Clase de acceso a datos de la tabla AF_ERACMF_ZONA
    /// </summary>
    public class AfEracmfZonaRepository : RepositoryBase, IAfEracmfZonaRepository
    {
        public AfEracmfZonaRepository(string strConn) : base(strConn)
        {
        }

        AfEracmfZonaHelper helper = new AfEracmfZonaHelper();

        public int Save(AfEracmfZonaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Aferaccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Aferacfeccreacion, DbType.DateTime, entity.Aferacfeccreacion);
            dbProvider.AddInParameter(command, helper.Aferacusucreacion, DbType.String, entity.Aferacusucreacion);
            dbProvider.AddInParameter(command, helper.Aferacdertemp, DbType.Decimal, entity.Aferacdertemp);
            dbProvider.AddInParameter(command, helper.Aferacderpend, DbType.Decimal, entity.Aferacderpend);
            dbProvider.AddInParameter(command, helper.Aferacderarrq, DbType.Decimal, entity.Aferacderarrq);
            dbProvider.AddInParameter(command, helper.Aferacumbraltemp, DbType.Decimal, entity.Aferacumbraltemp);
            dbProvider.AddInParameter(command, helper.Aferacumbralarrq, DbType.Decimal, entity.Aferacumbralarrq);
            dbProvider.AddInParameter(command, helper.Aferacporcrechazo, DbType.Decimal, entity.Aferacporcrechazo);
            dbProvider.AddInParameter(command, helper.Aferacnumetapa, DbType.Int32, entity.Aferacnumetapa);
            dbProvider.AddInParameter(command, helper.Aferaczona, DbType.String, entity.Aferaczona);
            dbProvider.AddInParameter(command, helper.Aferacfechaperiodo, DbType.DateTime, entity.Aferacfechaperiodo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AfEracmfZonaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Aferacfeccreacion, DbType.DateTime, entity.Aferacfeccreacion);
            dbProvider.AddInParameter(command, helper.Aferacusucreacion, DbType.String, entity.Aferacusucreacion);
            dbProvider.AddInParameter(command, helper.Aferacdertemp, DbType.Decimal, entity.Aferacdertemp);
            dbProvider.AddInParameter(command, helper.Aferacderpend, DbType.Decimal, entity.Aferacderpend);
            dbProvider.AddInParameter(command, helper.Aferacderarrq, DbType.Decimal, entity.Aferacderarrq);
            dbProvider.AddInParameter(command, helper.Aferacumbraltemp, DbType.Decimal, entity.Aferacumbraltemp);
            dbProvider.AddInParameter(command, helper.Aferacumbralarrq, DbType.Decimal, entity.Aferacumbralarrq);
            dbProvider.AddInParameter(command, helper.Aferacporcrechazo, DbType.Decimal, entity.Aferacporcrechazo);
            dbProvider.AddInParameter(command, helper.Aferacnumetapa, DbType.Int32, entity.Aferacnumetapa);
            dbProvider.AddInParameter(command, helper.Aferaczona, DbType.String, entity.Aferaczona);
            dbProvider.AddInParameter(command, helper.Aferacfechaperiodo, DbType.DateTime, entity.Aferacfechaperiodo);
            dbProvider.AddInParameter(command, helper.Aferaccodi, DbType.Int32, entity.Aferaccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int aferaccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Aferaccodi, DbType.Int32, aferaccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AfEracmfZonaDTO GetById(int aferaccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Aferaccodi, DbType.Int32, aferaccodi);
            AfEracmfZonaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AfEracmfZonaDTO> List()
        {
            List<AfEracmfZonaDTO> entitys = new List<AfEracmfZonaDTO>();
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

        public List<AfEracmfZonaDTO> GetByCriteria()
        {
            List<AfEracmfZonaDTO> entitys = new List<AfEracmfZonaDTO>();
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
