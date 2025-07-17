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
    /// Clase de acceso a datos de la tabla VCR_VERINCUMPLIM
    /// </summary>
    public class VcrVerincumplimRepository: RepositoryBase, IVcrVerincumplimRepository
    {
        public VcrVerincumplimRepository(string strConn): base(strConn)
        {
        }

        VcrVerincumplimHelper helper = new VcrVerincumplimHelper();

        public int Save(VcrVerincumplimDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcrinccodi, DbType.Int32, entity.Vcrinccodi);
            dbProvider.AddInParameter(command, helper.Equicodicen, DbType.Int32, entity.Equicodicen);
            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, entity.Equicodiuni);
            dbProvider.AddInParameter(command, helper.Vcrvincodrpf, DbType.Int32, entity.Vcrvincodrpf);
            dbProvider.AddInParameter(command, helper.Vcrvinfecha, DbType.DateTime, entity.Vcrvinfecha);
            dbProvider.AddInParameter(command, helper.Vcrvincumpli, DbType.Decimal, entity.Vcrvincumpli);
            dbProvider.AddInParameter(command, helper.Vcrvinobserv, DbType.String, entity.Vcrvinobserv);
            dbProvider.AddInParameter(command, helper.Vcrvinusucreacion, DbType.String, entity.Vcrvinusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrvinfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Vcrvincodi, DbType.Int32, id);
            
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrVerincumplimDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrinccodi, DbType.Int32, entity.Vcrinccodi);
            dbProvider.AddInParameter(command, helper.Equicodicen, DbType.Int32, entity.Equicodicen);
            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, entity.Equicodiuni);
            dbProvider.AddInParameter(command, helper.Vcrvincodrpf, DbType.Int32, entity.Vcrvincodrpf);
            dbProvider.AddInParameter(command, helper.Vcrvinfecha, DbType.DateTime, entity.Vcrvinfecha);
            dbProvider.AddInParameter(command, helper.Vcrvincumpli, DbType.Decimal, entity.Vcrvincumpli);
            dbProvider.AddInParameter(command, helper.Vcrvinobserv, DbType.String, entity.Vcrvinobserv);
            dbProvider.AddInParameter(command, helper.Vcrvinusucreacion, DbType.String, entity.Vcrvinusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrvinfeccreacion, DbType.DateTime, entity.Vcrvinfeccreacion);
            dbProvider.AddInParameter(command, helper.Vcrvincodi, DbType.Int32, entity.Vcrvincodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrinccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrinccodi, DbType.Int32, vcrinccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrVerincumplimDTO GetById(int vcrinccodi, DateTime vcrvinfecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcrinccodi, DbType.Int32, vcrinccodi);
            dbProvider.AddInParameter(command, helper.Vcrvinfecha, DbType.DateTime, vcrvinfecha);
            VcrVerincumplimDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrVerincumplimDTO> List(int vcrinccodi, int equicodicen, int equicodiuni)
        {
            List<VcrVerincumplimDTO> entitys = new List<VcrVerincumplimDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Vcrinccodi, DbType.Int32, vcrinccodi);
            dbProvider.AddInParameter(command, helper.Equicodicen, DbType.Int32, equicodicen);
            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, equicodiuni);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<VcrVerincumplimDTO> GetByCriteria(int vcrinccodi)
        {
            List<VcrVerincumplimDTO> entitys = new List<VcrVerincumplimDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Vcrinccodi, DbType.Int32, vcrinccodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrVerincumplimDTO entity = new VcrVerincumplimDTO();

                    int iEquicodicen = dr.GetOrdinal(this.helper.Equicodicen);
                    if (!dr.IsDBNull(iEquicodicen)) entity.Equicodicen = Convert.ToInt32(dr.GetValue(iEquicodicen));

                    int iEquicodiuni = dr.GetOrdinal(this.helper.Equicodiuni);
                    if (!dr.IsDBNull(iEquicodiuni)) entity.Equicodiuni = Convert.ToInt32(dr.GetValue(iEquicodiuni));

                    int iCentralnombre = dr.GetOrdinal(this.helper.Centralnombre);
                    if (!dr.IsDBNull(iCentralnombre)) entity.CentralNombre = dr.GetString(iCentralnombre);

                    int iUninombre = dr.GetOrdinal(this.helper.Uninombre);
                    if (!dr.IsDBNull(iUninombre)) entity.UniNombre = dr.GetString(iUninombre);

                    int iVcrvincodrpf = dr.GetOrdinal(this.helper.Vcrvincodrpf);
                    if (!dr.IsDBNull(iVcrvincodrpf)) entity.Vcrvincodrpf = Convert.ToInt32(dr.GetValue(iVcrvincodrpf));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public VcrVerincumplimDTO GetByIdPorUnidad(int vcrinccodi, int equicodiuni, int equicodicen)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdPorUnidad);

            dbProvider.AddInParameter(command, helper.Vcrinccodi, DbType.Int32, vcrinccodi);
            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, equicodiuni);
            dbProvider.AddInParameter(command, helper.Equicodicen, DbType.Int32, equicodicen);
            VcrVerincumplimDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new VcrVerincumplimDTO();

                    int iVcrvincumpli = dr.GetOrdinal(this.helper.Vcrvincumpli);
                    if (!dr.IsDBNull(iVcrvincumpli)) entity.Vcrvincumpli = dr.GetDecimal(iVcrvincumpli);
                }
            }

            return entity;
        }
    }
}
