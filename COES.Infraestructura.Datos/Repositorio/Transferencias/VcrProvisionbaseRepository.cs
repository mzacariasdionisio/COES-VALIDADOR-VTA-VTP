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
    /// Clase de acceso a datos de la tabla VCR_PROVISIONBASE
    /// </summary>
    public class VcrProvisionbaseRepository: RepositoryBase, IVcrProvisionbaseRepository
    {
        public VcrProvisionbaseRepository(string strConn): base(strConn)
        {
        }

        VcrProvisionbaseHelper helper = new VcrProvisionbaseHelper();

        public int Save(VcrProvisionbaseDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcrpbcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrpbperiodoini, DbType.DateTime, entity.Vcrpbperiodoini);
            dbProvider.AddInParameter(command, helper.Vcrpbperiodofin, DbType.DateTime, entity.Vcrpbperiodofin);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Vcrpbpotenciabf, DbType.Decimal, entity.Vcrpbpotenciabf);
            dbProvider.AddInParameter(command, helper.Vcrpbpreciobf, DbType.Decimal, entity.Vcrpbpreciobf);
            dbProvider.AddInParameter(command, helper.Vcrpbusucreacion, DbType.String, entity.Vcrpbusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrpbfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Vcrpbusumodificacion, DbType.String, entity.Vcrpbusumodificacion);
            dbProvider.AddInParameter(command, helper.Vcrpbfecmodificacion, DbType.DateTime, DateTime.Now);
            //ASSETEC: 202010
            dbProvider.AddInParameter(command, helper.Vcrpbpotenciabfb, DbType.Decimal, entity.Vcrpbpotenciabfb);
            dbProvider.AddInParameter(command, helper.Vcrpbpreciobfb, DbType.Decimal, entity.Vcrpbpreciobfb);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrProvisionbaseDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrpbperiodoini, DbType.DateTime, entity.Vcrpbperiodoini);
            dbProvider.AddInParameter(command, helper.Vcrpbperiodofin, DbType.DateTime, entity.Vcrpbperiodofin);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Vcrpbpotenciabf, DbType.Decimal, entity.Vcrpbpotenciabf);
            dbProvider.AddInParameter(command, helper.Vcrpbpreciobf, DbType.Decimal, entity.Vcrpbpreciobf);
            dbProvider.AddInParameter(command, helper.Vcrpbusumodificacion, DbType.String, entity.Vcrpbusumodificacion);
            dbProvider.AddInParameter(command, helper.Vcrpbfecmodificacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Vcrpbpotenciabfb, DbType.Decimal, entity.Vcrpbpotenciabfb);
            dbProvider.AddInParameter(command, helper.Vcrpbpreciobfb, DbType.Decimal, entity.Vcrpbpreciobfb);

            dbProvider.AddInParameter(command, helper.Vcrpbcodi, DbType.Int32, entity.Vcrpbcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrpbcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrpbcodi, DbType.Int32, vcrpbcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrProvisionbaseDTO GetById(int vcrpbcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcrpbcodi, DbType.Int32, vcrpbcodi);
            VcrProvisionbaseDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrProvisionbaseDTO> List()
        {
            List<VcrProvisionbaseDTO> entitys = new List<VcrProvisionbaseDTO>();
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

        public List<VcrProvisionbaseDTO> GetByCriteria()
        {
            List<VcrProvisionbaseDTO> entitys = new List<VcrProvisionbaseDTO>();
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

        public VcrProvisionbaseDTO GetByIdURS(int grupocodi, string periodo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdURS);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Periodo, DbType.String, periodo);
            dbProvider.AddInParameter(command, helper.Periodo, DbType.String, periodo);
            VcrProvisionbaseDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrProvisionbaseDTO> ListIndex()
        {
            List<VcrProvisionbaseDTO> entitys = new List<VcrProvisionbaseDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListIndex);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrProvisionbaseDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public VcrProvisionbaseDTO GetByIdView(int vcrpbcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdView);
            dbProvider.AddInParameter(command, helper.Vcrpbcodi, DbType.Int32, vcrpbcodi);

            VcrProvisionbaseDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                }
            }

            return entity;
        }
    }
}
