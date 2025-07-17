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
    /// Clase de acceso a datos de la tabla VCR_COSTOPORTUNIDAD
    /// </summary>
    public class VcrCostoportunidadRepository: RepositoryBase, IVcrCostoportunidadRepository
    {
        public VcrCostoportunidadRepository(string strConn): base(strConn)
        {
        }

        VcrCostoportunidadHelper helper = new VcrCostoportunidadHelper();

        public int Save(VcrCostoportunidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcrcopcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Vcrcopfecha, DbType.DateTime, entity.Vcrcopfecha);
            dbProvider.AddInParameter(command, helper.Vcrcopcosto, DbType.Decimal, entity.Vcrcopcosto);
            dbProvider.AddInParameter(command, helper.Vcrcopusucreacion, DbType.String, entity.Vcrcopusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrcopfeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrCostoportunidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Vcrcopfecha, DbType.DateTime, entity.Vcrcopfecha);
            dbProvider.AddInParameter(command, helper.Vcrcopcosto, DbType.Decimal, entity.Vcrcopcosto);
            dbProvider.AddInParameter(command, helper.Vcrcopusucreacion, DbType.String, entity.Vcrcopusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrcopfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Vcrcopcodi, DbType.Int32, entity.Vcrcopcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrCostoportunidadDTO GetById(int vcrcopcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcrcopcodi, DbType.Int32, vcrcopcodi);
            VcrCostoportunidadDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public VcrCostoportunidadDTO GetByIdEmpresa(int vcrecacodi, int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdEmpresa);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            VcrCostoportunidadDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new VcrCostoportunidadDTO();

                    int iVcrcopcosto = dr.GetOrdinal(this.helper.Vcrcopcosto);
                    if (!dr.IsDBNull(iVcrcopcosto)) entity.Vcrcopcosto = dr.GetDecimal(iVcrcopcosto);
                }
            }

            return entity;
        }

        public List<VcrCostoportunidadDTO> List()
        {
            List<VcrCostoportunidadDTO> entitys = new List<VcrCostoportunidadDTO>();
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

        public List<VcrCostoportunidadDTO> GetByCriteria()
        {
            List<VcrCostoportunidadDTO> entitys = new List<VcrCostoportunidadDTO>();
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
