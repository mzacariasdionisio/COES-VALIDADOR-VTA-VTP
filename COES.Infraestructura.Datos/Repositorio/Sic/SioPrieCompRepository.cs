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
    /// Clase de acceso a datos de la tabla SIO_PRIE_COMP
    /// </summary>
    public class SioPrieCompRepository: RepositoryBase, ISioPrieCompRepository
    {
        public SioPrieCompRepository(string strConn): base(strConn)
        {
        }

        SioPrieCompHelper helper = new SioPrieCompHelper();

        public int Save(SioPrieCompDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tbcompcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Tbcompfecperiodo, DbType.DateTime, entity.Tbcompfecperiodo);
            dbProvider.AddInParameter(command, helper.Tbcompte, DbType.Decimal, entity.Tbcompte);
            dbProvider.AddInParameter(command, helper.Tbcomppsr, DbType.Decimal, entity.Tbcomppsr);
            dbProvider.AddInParameter(command, helper.Tbcomprscd, DbType.Decimal, entity.Tbcomprscd);
            dbProvider.AddInParameter(command, helper.Tbcomprscul, DbType.Decimal, entity.Tbcomprscul);
            dbProvider.AddInParameter(command, helper.Tbcompcbec, DbType.Decimal, entity.Tbcompcbec);
            dbProvider.AddInParameter(command, helper.Tbcompcrf, DbType.Decimal, entity.Tbcompcrf);
            dbProvider.AddInParameter(command, helper.Tbcompcio, DbType.Decimal, entity.Tbcompcio);
            dbProvider.AddInParameter(command, helper.Tbcompsma, DbType.Decimal, entity.Tbcompsma);
            dbProvider.AddInParameter(command, helper.Tbcompoc, DbType.Decimal, entity.Tbcompoc);
            dbProvider.AddInParameter(command, helper.Tbcompusucreacion, DbType.String, entity.Tbcompusucreacion);
            dbProvider.AddInParameter(command, helper.Tbcompfeccreacion, DbType.DateTime, entity.Tbcompfeccreacion);
            dbProvider.AddInParameter(command, helper.Tbcompusumodificacion, DbType.String, entity.Tbcompusumodificacion);
            dbProvider.AddInParameter(command, helper.Tbcompfecmodificacion, DbType.DateTime, entity.Tbcompfecmodificacion);
            dbProvider.AddInParameter(command, helper.Tbcompcpa, DbType.Decimal, entity.Tbcompcpa);
            dbProvider.AddInParameter(command, helper.Tbcompcodosinergmin, DbType.String, entity.Tbcompcodosinergmin);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SioPrieCompDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tbcompcodi, DbType.Int32, entity.Tbcompcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Tbcompfecperiodo, DbType.DateTime, entity.Tbcompfecperiodo);
            dbProvider.AddInParameter(command, helper.Tbcompte, DbType.Decimal, entity.Tbcompte);
            dbProvider.AddInParameter(command, helper.Tbcomppsr, DbType.Decimal, entity.Tbcomppsr);
            dbProvider.AddInParameter(command, helper.Tbcomprscd, DbType.Decimal, entity.Tbcomprscd);
            dbProvider.AddInParameter(command, helper.Tbcomprscul, DbType.Decimal, entity.Tbcomprscul);
            dbProvider.AddInParameter(command, helper.Tbcompcbec, DbType.Decimal, entity.Tbcompcbec);
            dbProvider.AddInParameter(command, helper.Tbcompcrf, DbType.Decimal, entity.Tbcompcrf);
            dbProvider.AddInParameter(command, helper.Tbcompcio, DbType.Decimal, entity.Tbcompcio);
            dbProvider.AddInParameter(command, helper.Tbcompsma, DbType.Decimal, entity.Tbcompsma);
            dbProvider.AddInParameter(command, helper.Tbcompoc, DbType.Decimal, entity.Tbcompoc);
            dbProvider.AddInParameter(command, helper.Tbcompusucreacion, DbType.String, entity.Tbcompusucreacion);
            dbProvider.AddInParameter(command, helper.Tbcompfeccreacion, DbType.DateTime, entity.Tbcompfeccreacion);
            dbProvider.AddInParameter(command, helper.Tbcompusumodificacion, DbType.String, entity.Tbcompusumodificacion);
            dbProvider.AddInParameter(command, helper.Tbcompfecmodificacion, DbType.DateTime, entity.Tbcompfecmodificacion);
            dbProvider.AddInParameter(command, helper.Tbcompcpa, DbType.Decimal, entity.Tbcompcpa);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(DateTime fechaPeriodo)
        {
            string sql = string.Format(helper.SqlDelete, fechaPeriodo.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            dbProvider.ExecuteNonQuery(command);
        }

        public SioPrieCompDTO GetById(int tbcompcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tbcompcodi, DbType.Int32, tbcompcodi);
            SioPrieCompDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SioPrieCompDTO> List()
        {
            List<SioPrieCompDTO> entitys = new List<SioPrieCompDTO>();
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

        public List<SioPrieCompDTO> GetByCriteria(DateTime fechaPeriodo)
        {
            List<SioPrieCompDTO> entitys = new List<SioPrieCompDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, fechaPeriodo.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int IEmprcodosinergmin = dr.GetOrdinal(helper.Emprcodosinergmin);
                    if (!dr.IsDBNull(IEmprcodosinergmin)) entity.Emprcodosinergmin = dr.GetString(IEmprcodosinergmin);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
