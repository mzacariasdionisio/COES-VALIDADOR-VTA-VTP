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
    public class VcrVerporctreservRepository: RepositoryBase, IVcrVerporctreservRepository
    {
        public VcrVerporctreservRepository(string strConn): base(strConn)
        {
        }

        VcrVerporctreservHelper helper = new VcrVerporctreservHelper();

        public int Save(VcrVerporctreservDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcrvprcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrinccodi, DbType.Int32, entity.Vcrinccodi);
            dbProvider.AddInParameter(command, helper.Equicodicen, DbType.Int32, entity.Equicodicen);
            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, entity.Equicodiuni);
            dbProvider.AddInParameter(command, helper.Vcrvprfecha, DbType.DateTime, entity.Vcrvprfecha);
            dbProvider.AddInParameter(command, helper.Vcrvprrpns, DbType.Decimal, entity.Vcrvprrpns);
            dbProvider.AddInParameter(command, helper.Vcrvprusucreacion, DbType.String, entity.Vcrvprusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrvprfeccreacion, DbType.DateTime, DateTime.Now);
            
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrVerporctreservDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrinccodi, DbType.Int32, entity.Vcrinccodi);
            dbProvider.AddInParameter(command, helper.Equicodicen, DbType.Int32, entity.Equicodicen);
            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, entity.Equicodiuni);
            dbProvider.AddInParameter(command, helper.Vcrvprfecha, DbType.DateTime, entity.Vcrvprfecha);
            dbProvider.AddInParameter(command, helper.Vcrvprrpns, DbType.Decimal, entity.Vcrvprrpns);
            dbProvider.AddInParameter(command, helper.Vcrvprusucreacion, DbType.String, entity.Vcrvprusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrvprfeccreacion, DbType.DateTime, entity.Vcrvprfeccreacion);
            //where
            dbProvider.AddInParameter(command, helper.Vcrvprcodi, DbType.Int32, entity.Vcrvprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrinccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrinccodi, DbType.Int32, vcrinccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrVerporctreservDTO GetById(int vcrinccodi, DateTime Vcrvprfecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcrinccodi, DbType.Int32, vcrinccodi);
            dbProvider.AddInParameter(command, helper.Vcrvprfecha, DbType.DateTime, Vcrvprfecha);
            VcrVerporctreservDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrVerporctreservDTO> List(int vcrinccodi, int equicodicen, int equicodiuni)
        {
            List<VcrVerporctreservDTO> entitys = new List<VcrVerporctreservDTO>();
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

        public List<VcrVerporctreservDTO> GetByCriteria(int vcrinccodi)
        {
            List<VcrVerporctreservDTO> entitys = new List<VcrVerporctreservDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Vcrinccodi, DbType.Int32, vcrinccodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrVerporctreservDTO entity = new VcrVerporctreservDTO();

                    int iEquicodicen = dr.GetOrdinal(this.helper.Equicodicen);
                    if (!dr.IsDBNull(iEquicodicen)) entity.Equicodicen = Convert.ToInt32(dr.GetValue(iEquicodicen));

                    int iEquicodiuni = dr.GetOrdinal(this.helper.Equicodiuni);
                    if (!dr.IsDBNull(iEquicodiuni)) entity.Equicodiuni = Convert.ToInt32(dr.GetValue(iEquicodiuni));

                    int iCentralnombre = dr.GetOrdinal(this.helper.Centralnombre);
                    if (!dr.IsDBNull(iCentralnombre)) entity.CentralNombre = dr.GetString(iCentralnombre);

                    int iUnidadnombre = dr.GetOrdinal(this.helper.Unidadnombre);
                    if (!dr.IsDBNull(iUnidadnombre)) entity.UnidadNombre = dr.GetString(iUnidadnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public VcrVerporctreservDTO GetByIdPorUnidad(int vcrinccodi, int equicodiuni, int equicodicen)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdPorUnidad);

            dbProvider.AddInParameter(command, helper.Vcrinccodi, DbType.Int32, vcrinccodi);
            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, equicodiuni);
            dbProvider.AddInParameter(command, helper.Equicodicen, DbType.Int32, equicodicen);
            VcrVerporctreservDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new VcrVerporctreservDTO();

                    int iVcrvprrpns = dr.GetOrdinal(this.helper.Vcrvprrpns);
                    if (!dr.IsDBNull(iVcrvprrpns)) entity.Vcrvprrpns = dr.GetDecimal(iVcrvprrpns);
                }
            }

            return entity;
        }
    }
}
