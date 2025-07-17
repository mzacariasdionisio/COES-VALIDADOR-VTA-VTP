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
    /// Clase de acceso a datos de la tabla RER_FAC_PER_MED_DET
    /// </summary>
    public class RerFacPerMedDetRepository : RepositoryBase, IRerFacPerMedDetRepository
    {
        public RerFacPerMedDetRepository(string strConn)
            : base(strConn)
        {
        }

        RerFacPerMedDetHelper helper = new RerFacPerMedDetHelper();

        public int Save(RerFacPerMedDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rerfpdcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rerfpmcodi, DbType.Int32, entity.Rerfpmcodi);
            dbProvider.AddInParameter(command, helper.Codentcodi, DbType.Int32, entity.Codentcodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rerfpdfactperdida, DbType.Decimal, entity.Rerfpdfactperdida);
            dbProvider.AddInParameter(command, helper.Rerfpdusucreacion, DbType.String, entity.Rerfpdusucreacion);
            dbProvider.AddInParameter(command, helper.Rerfpdfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Rerfpdusumodificacion, DbType.String, entity.Rerfpdusumodificacion);
            dbProvider.AddInParameter(command, helper.Rerfpdfecmodificacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RerFacPerMedDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rerfpmcodi, DbType.Int32, entity.Rerfpmcodi);
            dbProvider.AddInParameter(command, helper.Codentcodi, DbType.Int32, entity.Codentcodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rerfpdfactperdida, DbType.Decimal, entity.Rerfpdfactperdida);
            dbProvider.AddInParameter(command, helper.Rerfpdusucreacion, DbType.String, entity.Rerfpdusucreacion);
            dbProvider.AddInParameter(command, helper.Rerfpdfecmodificacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Rerfpdcodi, DbType.Int32, entity.Rerfpdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rerFpdCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rerfpdcodi, DbType.Int32, rerFpdCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RerFacPerMedDetDTO GetById(int rerFpdCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rerfpdcodi, DbType.Int32, rerFpdCodi);
            RerFacPerMedDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RerFacPerMedDetDTO> List()
        {
            List<RerFacPerMedDetDTO> entities = new List<RerFacPerMedDetDTO>();
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

        public List<RerFacPerMedDetDTO> GetByCriteria(int rerFpmCodi)
        {
            List<RerFacPerMedDetDTO> entities = new List<RerFacPerMedDetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Rerfpmcodi, DbType.Int32, rerFpmCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

        public List<RerFacPerMedDetDTO> ListByFPM(int id)
        {
            List<RerFacPerMedDetDTO> entitys = new List<RerFacPerMedDetDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByFPM);
            dbProvider.AddInParameter(command, helper.Rerfpmcodi, DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerFacPerMedDetDTO entity = helper.Create(dr);

                    int iCodentcodigo = dr.GetOrdinal(helper.Codentcodigo);
                    if (!dr.IsDBNull(iCodentcodigo)) entity.Codentcodigo = dr.GetString(iCodentcodigo);

                    int iBarrnombre = dr.GetOrdinal(helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = dr.GetString(iBarrnombre);

                    int iEmpresanombre = dr.GetOrdinal(helper.Empresanombre);
                    if (!dr.IsDBNull(iEmpresanombre)) entity.Empresanombre = dr.GetString(iEmpresanombre);

                    int iEquiponombre = dr.GetOrdinal(helper.Equiponombre);
                    if (!dr.IsDBNull(iEquiponombre)) entity.Equiponombre = dr.GetString(iEquiponombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RerFacPerMedDetDTO> GetByRangeDate(DateTime dtInicio, DateTime dtFin)
        {
            string strInicio = dtInicio.ToString(ConstantesBase.FormatoFechaBase);
            string strFin = dtFin.ToString(ConstantesBase.FormatoFechaBase);

            string query = String.Format(helper.SqlGetByRangeDate, strInicio, strFin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerFacPerMedDetDTO> entities = new List<RerFacPerMedDetDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateByRangeDate(dr));
                }
            }

            return entities;
        }

    }
}
