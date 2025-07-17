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
    /// Clase de acceso a datos de la tabla VCR_VERDEFICIT
    /// </summary>
    public class VcrVerdeficitRepository: RepositoryBase, IVcrVerdeficitRepository
    {
        public VcrVerdeficitRepository(string strConn): base(strConn)
        {
        }

        VcrVerdeficitHelper helper = new VcrVerdeficitHelper();

        public int Save(VcrVerdeficitDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcrvdecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, entity.Vcrdsrcodi);
            dbProvider.AddInParameter(command, helper.Vcrvdefecha, DbType.DateTime, entity.Vcrvdefecha);
            dbProvider.AddInParameter(command, helper.Vcrvdehorinicio, DbType.DateTime, entity.Vcrvdehorinicio);
            dbProvider.AddInParameter(command, helper.Vcrvdehorfinal, DbType.DateTime, entity.Vcrvdehorfinal);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Vcrvdedeficit, DbType.Decimal, entity.Vcrvdedeficit);
            dbProvider.AddInParameter(command, helper.Vcrvdeusucreacion, DbType.String, entity.Vcrvdeusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrvdefeccreacion, DbType.DateTime, entity.Vcrvdefeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrVerdeficitDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, entity.Vcrdsrcodi);
            dbProvider.AddInParameter(command, helper.Vcrvdefecha, DbType.DateTime, entity.Vcrvdefecha);
            dbProvider.AddInParameter(command, helper.Vcrvdehorinicio, DbType.DateTime, entity.Vcrvdehorinicio);
            dbProvider.AddInParameter(command, helper.Vcrvdehorfinal, DbType.DateTime, entity.Vcrvdehorfinal);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Vcrvdedeficit, DbType.Decimal, entity.Vcrvdedeficit);
            dbProvider.AddInParameter(command, helper.Vcrvdeusucreacion, DbType.String, entity.Vcrvdeusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrvdefeccreacion, DbType.DateTime, entity.Vcrvdefeccreacion);
            dbProvider.AddInParameter(command, helper.Vcrvdecodi, DbType.Int32, entity.Vcrvdecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrdsrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, vcrdsrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrVerdeficitDTO GetById(int vcrvdecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcrvdecodi, DbType.Int32, vcrvdecodi);
            VcrVerdeficitDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrVerdeficitDTO> List()
        {
            List<VcrVerdeficitDTO> entitys = new List<VcrVerdeficitDTO>();
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

        public List<VcrVerdeficitDTO> GetByCriteria(int vcrdsrcodi)
        {
            List<VcrVerdeficitDTO> entitys = new List<VcrVerdeficitDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, vcrdsrcodi);
            

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrVerdeficitDTO entity = helper.Create(dr);

                    int iEmprNombre = dr.GetOrdinal(this.helper.EmprNombre);
                    if (!dr.IsDBNull(iEmprNombre)) entity.EmprNombre = dr.GetString(iEmprNombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VcrVerdeficitDTO> ListDia(int vcrdsrcodi, int grupocodi, DateTime vcrvdefecha)
        {
            List<VcrVerdeficitDTO> entitys = new List<VcrVerdeficitDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListDia);
            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, vcrdsrcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Vcrvdefecha, DbType.DateTime, vcrvdefecha);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<VcrVerdeficitDTO> ListDiaHFHI(int vcrdsrcodi, DateTime vcrvdefecha)
        {
            List<VcrVerdeficitDTO> entitys = new List<VcrVerdeficitDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListDiaHFHI);
            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, vcrdsrcodi);
            dbProvider.AddInParameter(command, helper.Vcrvdefecha, DbType.DateTime, vcrvdefecha);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrVerdeficitDTO entity = new VcrVerdeficitDTO();

                    int iVcrvdehorinicio = dr.GetOrdinal(this.helper.Vcrvdehorinicio);
                    if (!dr.IsDBNull(iVcrvdehorinicio)) entity.Vcrvdehorinicio = dr.GetDateTime(iVcrvdehorinicio);

                    int iVcrvdehorfinal = dr.GetOrdinal(this.helper.Vcrvdehorfinal);
                    if (!dr.IsDBNull(iVcrvdehorfinal)) entity.Vcrvdehorfinal = dr.GetDateTime(iVcrvdehorfinal);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
