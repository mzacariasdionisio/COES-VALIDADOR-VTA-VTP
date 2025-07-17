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
    /// Clase de acceso a datos de la tabla VCR_VERRNS
    /// </summary>
    public class VcrVerrnsRepository: RepositoryBase, IVcrVerrnsRepository
    {
        public VcrVerrnsRepository(string strConn): base(strConn)
        {
        }

        VcrVerrnsHelper helper = new VcrVerrnsHelper();

        public int Save(VcrVerrnsDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcvrnscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, entity.Vcrdsrcodi);
            dbProvider.AddInParameter(command, helper.Vcvrnsfecha, DbType.DateTime, entity.Vcvrnsfecha);
            dbProvider.AddInParameter(command, helper.Vcvrnshorinicio, DbType.DateTime, entity.Vcvrnshorinicio);
            dbProvider.AddInParameter(command, helper.Vcvrnshorfinal, DbType.DateTime, entity.Vcvrnshorfinal);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Vcvrnsrns, DbType.Decimal, entity.Vcvrnsrns);
            dbProvider.AddInParameter(command, helper.Vcvrnsusucreacion, DbType.String, entity.Vcvrnsusucreacion);
            dbProvider.AddInParameter(command, helper.Vcvrnsfeccreacion, DbType.DateTime, entity.Vcvrnsfeccreacion);
            dbProvider.AddInParameter(command, helper.Vcvrnstipocarga, DbType.Int32, entity.Vcvrnstipocarga);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrVerrnsDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, entity.Vcrdsrcodi);
            dbProvider.AddInParameter(command, helper.Vcvrnsfecha, DbType.DateTime, entity.Vcvrnsfecha);
            dbProvider.AddInParameter(command, helper.Vcvrnshorinicio, DbType.DateTime, entity.Vcvrnshorinicio);
            dbProvider.AddInParameter(command, helper.Vcvrnshorfinal, DbType.DateTime, entity.Vcvrnshorfinal);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Vcvrnsrns, DbType.Decimal, entity.Vcvrnsrns);
            dbProvider.AddInParameter(command, helper.Vcvrnsusucreacion, DbType.String, entity.Vcvrnsusucreacion);
            dbProvider.AddInParameter(command, helper.Vcvrnsfeccreacion, DbType.DateTime, entity.Vcvrnsfeccreacion);
            dbProvider.AddInParameter(command, helper.Vcvrnstipocarga, DbType.Int32, entity.Vcvrnstipocarga);
            dbProvider.AddInParameter(command, helper.Vcvrnscodi, DbType.Int32, entity.Vcvrnscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrdsrcodi, int vcvrnstipocarga)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, vcrdsrcodi);
            dbProvider.AddInParameter(command, helper.Vcvrnstipocarga, DbType.Int32, vcvrnstipocarga);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrVerrnsDTO GetById(int vcvrnscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcvrnscodi, DbType.Int32, vcvrnscodi);
            VcrVerrnsDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrVerrnsDTO> List()
        {
            List<VcrVerrnsDTO> entitys = new List<VcrVerrnsDTO>();
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

        public List<VcrVerrnsDTO> GetByCriteria(int vcrdsrcodi, int vcvrnstipocarga)
        {
            List<VcrVerrnsDTO> entitys = new List<VcrVerrnsDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, vcrdsrcodi);
            dbProvider.AddInParameter(command, helper.Vcvrnstipocarga, DbType.Int32, vcvrnstipocarga);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrVerrnsDTO entity = helper.Create(dr);

                    int iEmprNombre = dr.GetOrdinal(this.helper.EmprNombre);
                    if (!dr.IsDBNull(iEmprNombre)) entity.EmprNombre = dr.GetString(iEmprNombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VcrVerrnsDTO> ListDia(int vcrdsrcodi, int grupocodi, DateTime vcvrnsfecha, int vcvrnstipocarga)
        {
            List<VcrVerrnsDTO> entitys = new List<VcrVerrnsDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListDia);
            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, vcrdsrcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Vcvrnsfecha, DbType.DateTime, vcvrnsfecha);
            dbProvider.AddInParameter(command, helper.Vcvrnstipocarga, DbType.Int32, vcvrnstipocarga);
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
