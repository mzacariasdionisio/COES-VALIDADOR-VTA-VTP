using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla ABI_POTEFEC
    /// </summary>
    public class AbiPotefecRepository : RepositoryBase, IAbiPotefecRepository
    {
        public AbiPotefecRepository(string strConn) : base(strConn)
        {
        }

        AbiPotefecHelper helper = new AbiPotefecHelper();

        public int Save(AbiPotefecDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pefecfecmodificacion, DbType.DateTime, entity.Pefecfecmodificacion);
            dbProvider.AddInParameter(command, helper.Pefecusumodificacion, DbType.String, entity.Pefecusumodificacion);
            dbProvider.AddInParameter(command, helper.Pefectipogenerrer, DbType.String, entity.Pefectipogenerrer);
            dbProvider.AddInParameter(command, helper.Pefecintegrante, DbType.String, entity.Pefecintegrante);
            dbProvider.AddInParameter(command, helper.Pefecvalorpinst, DbType.Decimal, entity.Pefecvalorpinst);
            dbProvider.AddInParameter(command, helper.Pefecvalorpe, DbType.Decimal, entity.Pefecvalorpe);
            dbProvider.AddInParameter(command, helper.Pefecfechames, DbType.DateTime, entity.Pefecfechames);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre);
            dbProvider.AddInParameter(command, helper.Ctgdetcodi2, DbType.Int32, entity.Ctgdetcodi2);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ctgdetcodi, DbType.Int32, entity.Ctgdetcodi);
            dbProvider.AddInParameter(command, helper.Tgenercodi, DbType.Int32, entity.Tgenercodi);
            dbProvider.AddInParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi);

            dbProvider.AddInParameter(command, helper.Pefeccodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AbiPotefecDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pefecfecmodificacion, DbType.DateTime, entity.Pefecfecmodificacion);
            dbProvider.AddInParameter(command, helper.Pefecusumodificacion, DbType.String, entity.Pefecusumodificacion);
            dbProvider.AddInParameter(command, helper.Pefectipogenerrer, DbType.String, entity.Pefectipogenerrer);
            dbProvider.AddInParameter(command, helper.Pefecintegrante, DbType.String, entity.Pefecintegrante);
            dbProvider.AddInParameter(command, helper.Pefecvalorpinst, DbType.Decimal, entity.Pefecvalorpinst);
            dbProvider.AddInParameter(command, helper.Pefecvalorpe, DbType.Decimal, entity.Pefecvalorpe);
            dbProvider.AddInParameter(command, helper.Pefecfechames, DbType.DateTime, entity.Pefecfechames);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre);
            dbProvider.AddInParameter(command, helper.Ctgdetcodi2, DbType.Int32, entity.Ctgdetcodi2);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ctgdetcodi, DbType.Int32, entity.Ctgdetcodi);
            dbProvider.AddInParameter(command, helper.Tgenercodi, DbType.Int32, entity.Tgenercodi);
            dbProvider.AddInParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi);
            dbProvider.AddInParameter(command, helper.Pefeccodi, DbType.Int32, entity.Pefeccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pefeccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pefeccodi, DbType.Int32, pefeccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByMes(DateTime fechaPeriodo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByMes);

            dbProvider.AddInParameter(command, helper.Pefecfechames, DbType.DateTime, fechaPeriodo);

            dbProvider.ExecuteNonQuery(command);
        }

        public AbiPotefecDTO GetById(int pefeccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pefeccodi, DbType.Int32, pefeccodi);
            AbiPotefecDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AbiPotefecDTO> List()
        {
            List<AbiPotefecDTO> entitys = new List<AbiPotefecDTO>();
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

        public List<AbiPotefecDTO> GetByCriteria()
        {
            List<AbiPotefecDTO> entitys = new List<AbiPotefecDTO>();
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

        public List<AbiPotefecDTO> ListaPorMes(DateTime fecIni, DateTime fecFin)
        {
            List<AbiPotefecDTO> entitys = new List<AbiPotefecDTO>();

            string query = string.Format(helper.SqlListaPorMes, fecIni.ToString(ConstantesBase.FormatoFechaFullBase), fecFin.ToString(ConstantesBase.FormatoFechaFullBase));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

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
