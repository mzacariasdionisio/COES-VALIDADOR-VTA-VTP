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
    /// Clase de acceso a datos de la tabla VCR_VERSUPERAVIT
    /// </summary>
    public class VcrVersuperavitRepository: RepositoryBase, IVcrVersuperavitRepository
    {
        public VcrVersuperavitRepository(string strConn): base(strConn)
        {
        }

        VcrVersuperavitHelper helper = new VcrVersuperavitHelper();

        public int Save(VcrVersuperavitDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcrvsacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, entity.Vcrdsrcodi);
            dbProvider.AddInParameter(command, helper.Vcrvsafecha, DbType.DateTime, entity.Vcrvsafecha);
            dbProvider.AddInParameter(command, helper.Vcrvsahorinicio, DbType.DateTime, entity.Vcrvsahorinicio);
            dbProvider.AddInParameter(command, helper.Vcrvsahorfinal, DbType.DateTime, entity.Vcrvsahorfinal);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Vcrvsasuperavit, DbType.Decimal, entity.Vcrvsasuperavit);
            dbProvider.AddInParameter(command, helper.Vcrvsausucreacion, DbType.String, entity.Vcrvsausucreacion);
            dbProvider.AddInParameter(command, helper.Vcrvsafeccreacion, DbType.DateTime, entity.Vcrvsafeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrVersuperavitDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, entity.Vcrdsrcodi);
            dbProvider.AddInParameter(command, helper.Vcrvsafecha, DbType.DateTime, entity.Vcrvsafecha);
            dbProvider.AddInParameter(command, helper.Vcrvsahorinicio, DbType.DateTime, entity.Vcrvsahorinicio);
            dbProvider.AddInParameter(command, helper.Vcrvsahorfinal, DbType.DateTime, entity.Vcrvsahorfinal);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Vcrvsasuperavit, DbType.Decimal, entity.Vcrvsasuperavit);
            dbProvider.AddInParameter(command, helper.Vcrvsausucreacion, DbType.String, entity.Vcrvsausucreacion);
            dbProvider.AddInParameter(command, helper.Vcrvsafeccreacion, DbType.DateTime, entity.Vcrvsafeccreacion);
            dbProvider.AddInParameter(command, helper.Vcrvsacodi, DbType.Int32, entity.Vcrvsacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrdsrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, vcrdsrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrVersuperavitDTO GetById(int vcrvsacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcrvsacodi, DbType.Int32, vcrvsacodi);
            VcrVersuperavitDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrVersuperavitDTO> List()
        {
            List<VcrVersuperavitDTO> entitys = new List<VcrVersuperavitDTO>();
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

        public List<VcrVersuperavitDTO> GetByCriteria(int vcrdsrcodi)
        {
            List<VcrVersuperavitDTO> entitys = new List<VcrVersuperavitDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, vcrdsrcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrVersuperavitDTO entity = helper.Create(dr);

                    int iEmprNombre = dr.GetOrdinal(this.helper.EmprNombre);
                    if (!dr.IsDBNull(iEmprNombre)) entity.EmprNombre = dr.GetString(iEmprNombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VcrVersuperavitDTO> ListDia(int vcrdsrcodi, int grupocodi, DateTime vcrvsafecha)
        {
            List<VcrVersuperavitDTO> entitys = new List<VcrVersuperavitDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListDia);
            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, vcrdsrcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Vcrvsafecha, DbType.DateTime, vcrvsafecha);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        //Agregado el 29-04-2019
        public List<VcrVersuperavitDTO> ListDiaURS(DateTime vcrvsafecha)
        {
            List<VcrVersuperavitDTO> entitys = new List<VcrVersuperavitDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListDiaURS);
            dbProvider.AddInParameter(command, helper.Vcrvsafecha, DbType.DateTime, vcrvsafecha);

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
