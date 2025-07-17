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
    /// Clase de acceso a datos de la tabla RER_CENTRAL_CODRETIRO
    /// </summary>
    public class RerCentralCodRetiroRepository : RepositoryBase, IRerCentralCodRetiroRepository
    {
        public RerCentralCodRetiroRepository(string strConn)
            : base(strConn)
        {
        }

        RerCentralCodRetiroHelper helper = new RerCentralCodRetiroHelper();

        public int Save(RerCentralCodRetiroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rerccrcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, entity.Rerpprcodi);
            dbProvider.AddInParameter(command, helper.Rercencodi, DbType.Int32, entity.Rercencodi);
            dbProvider.AddInParameter(command, helper.Coresocodi, DbType.Int32, entity.Coresocodi);
            dbProvider.AddInParameter(command, helper.Rerccrusucreacion, DbType.String, entity.Rerccrusucreacion);
            dbProvider.AddInParameter(command, helper.Rerccrfeccreacion, DbType.DateTime, entity.Rerccrfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RerCentralCodRetiroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, entity.Rerpprcodi);
            dbProvider.AddInParameter(command, helper.Rercencodi, DbType.Int32, entity.Rercencodi);
            dbProvider.AddInParameter(command, helper.Coresocodi, DbType.Int32, entity.Coresocodi);
            dbProvider.AddInParameter(command, helper.Rerccrusucreacion, DbType.String, entity.Rerccrusucreacion);
            dbProvider.AddInParameter(command, helper.Rerccrfeccreacion, DbType.DateTime, entity.Rerccrfeccreacion);
            dbProvider.AddInParameter(command, helper.Rerccrcodi, DbType.Int32, entity.Rerccrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rerCcrCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rerccrcodi, DbType.Int32, rerCcrCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteAllByRerpprcodiRercencodi(int Rerpprcodi, int Rercencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteAllByRerpprcodiRercencodi);

            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, Rerpprcodi);
            dbProvider.AddInParameter(command, helper.Rercencodi, DbType.Int32, Rercencodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RerCentralCodRetiroDTO GetById(int rerCcrCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rerccrcodi, DbType.Int32, rerCcrCodi);
            RerCentralCodRetiroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RerCentralCodRetiroDTO> List()
        {
            List<RerCentralCodRetiroDTO> entities = new List<RerCentralCodRetiroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

        public List<RerCentralCodRetiroDTO> ListCantidadByRerpprcodi(int rerpprcodi)
        {
            List<RerCentralCodRetiroDTO> entities = new List<RerCentralCodRetiroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListCantidadByRerpprcodi);
            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, rerpprcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerCentralCodRetiroDTO entity = new RerCentralCodRetiroDTO();

                    int iRercencodi = dr.GetOrdinal(helper.Rercencodi);
                    if (!dr.IsDBNull(iRercencodi)) entity.Rercencodi = Convert.ToInt32(dr.GetValue(iRercencodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCantidad = dr.GetOrdinal(helper.Cantidad);
                    if (!dr.IsDBNull(iCantidad)) entity.Cantidad = Convert.ToInt32(dr.GetValue(iCantidad));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entities.Add(entity);
                }
            }

            return entities;
        }

        public List<RerCentralCodRetiroDTO> ListNombreCodRetiroBarrTransferenciaByRerpprcodiRercencodi(int Rerpprcodi, int Rercencodi)
        {
            List<RerCentralCodRetiroDTO> entities = new List<RerCentralCodRetiroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListNombreCodRetiroBarrTransferenciaByRerpprcodiRercencodi);
            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, Rerpprcodi);
            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, Rercencodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerCentralCodRetiroDTO entity = new RerCentralCodRetiroDTO();

                    int iRercencodi = dr.GetOrdinal(helper.Rercencodi);
                    if (!dr.IsDBNull(iRercencodi)) entity.Rercencodi = Convert.ToInt32(dr.GetValue(iRercencodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCantidad = dr.GetOrdinal(helper.Cantidad);
                    if (!dr.IsDBNull(iCantidad)) entity.Cantidad = Convert.ToInt32(dr.GetValue(iCantidad));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entities.Add(entity);
                }
            }

            return entities;
        }
        //CU21
        public string ListaCodigoRetiroByEquipo(int iRerPPrCodi, int iEquiCodi)
        {
            string sListaCodigosRetiros = "";
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaCodigoRetiroByEquipo);
            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, iRerPPrCodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, iEquiCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) sListaCodigosRetiros = dr.GetString(iEquinomb);

                }
            }
            return sListaCodigosRetiros;
        }
    }
}
