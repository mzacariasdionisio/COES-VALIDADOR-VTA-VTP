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
    /// Clase de acceso a datos de la tabla IND_GASEODUCTOXCENTRAL
    /// </summary>
    public class IndGaseoductoxcentralRepository : RepositoryBase, IIndGaseoductoxcentralRepository
    {
        public IndGaseoductoxcentralRepository(string strConn) : base(strConn)
        {
        }

        IndGaseoductoxcentralHelper helper = new IndGaseoductoxcentralHelper();

        public int Save(IndGaseoductoxcentralDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Gasctrcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Gaseoductoequicodi, DbType.Int32, entity.Gaseoductoequicodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Gasctrestado, DbType.String, entity.Gasctrestado);
            dbProvider.AddInParameter(command, helper.Gasctrfeccreacion, DbType.DateTime, entity.Gasctrfeccreacion);
            dbProvider.AddInParameter(command, helper.Gasctrusucreacion, DbType.String, entity.Gasctrusucreacion);
            dbProvider.AddInParameter(command, helper.Gasctrusumodificacion, DbType.String, entity.Gasctrusumodificacion);
            dbProvider.AddInParameter(command, helper.Gasctrfecmodificacion, DbType.DateTime, entity.Gasctrfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(IndGaseoductoxcentralDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Gasctrcodi, DbType.Int32, entity.Gasctrcodi);
            dbProvider.AddInParameter(command, helper.Gaseoductoequicodi, DbType.Int32, entity.Gaseoductoequicodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Gasctrestado, DbType.String, entity.Gasctrestado);
            dbProvider.AddInParameter(command, helper.Gasctrfeccreacion, DbType.DateTime, entity.Gasctrfeccreacion);
            dbProvider.AddInParameter(command, helper.Gasctrusucreacion, DbType.String, entity.Gasctrusucreacion);
            dbProvider.AddInParameter(command, helper.Gasctrusumodificacion, DbType.String, entity.Gasctrusumodificacion);
            dbProvider.AddInParameter(command, helper.Gasctrfecmodificacion, DbType.DateTime, entity.Gasctrfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int gasctrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Gasctrcodi, DbType.Int32, gasctrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IndGaseoductoxcentralDTO GetById(int gasctrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Gasctrcodi, DbType.Int32, gasctrcodi);
            IndGaseoductoxcentralDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<IndGaseoductoxcentralDTO> List()
        {
            List<IndGaseoductoxcentralDTO> entitys = new List<IndGaseoductoxcentralDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEmprNomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    int iCentral = dr.GetOrdinal(helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iGaseoducto = dr.GetOrdinal(helper.Gaseoducto);
                    if (!dr.IsDBNull(iGaseoducto)) entity.Gaseoducto = dr.GetString(iGaseoducto);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndGaseoductoxcentralDTO> GetByCriteria()
        {
            List<IndGaseoductoxcentralDTO> entitys = new List<IndGaseoductoxcentralDTO>();
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

        public bool Inactivar(int gasctrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlInactivar);

            dbProvider.AddInParameter(command, helper.Gasctrcodi, DbType.Int32, gasctrcodi);

            var rowsAfe = dbProvider.ExecuteNonQuery(command);

            return rowsAfe > 0;
        }
    }
}
