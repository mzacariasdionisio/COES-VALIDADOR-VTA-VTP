using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla MAP_EMPRESAUL
    /// </summary>
    public class MapEmpresaulRepository : RepositoryBase, IMapEmpresaulRepository
    {
        public MapEmpresaulRepository(string strConn) : base(strConn)
        {
        }

        MapEmpresaulHelper helper = new MapEmpresaulHelper();

        public int Save(MapEmpresaulDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Empulcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Empulfecha, DbType.DateTime, entity.Empulfecha);
            dbProvider.AddInParameter(command, helper.Empuldesv, DbType.Decimal, entity.Empuldesv);
            dbProvider.AddInParameter(command, helper.Empulprog, DbType.Decimal, entity.Empulprog);
            dbProvider.AddInParameter(command, helper.Empulejec, DbType.Decimal, entity.Empulejec);
            dbProvider.AddInParameter(command, helper.Tipoccodi, DbType.Int32, entity.Tipoccodi);
            dbProvider.AddInParameter(command, helper.Vermcodi, DbType.Int32, entity.Vermcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MapEmpresaulDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Empulfecha, DbType.DateTime, entity.Empulfecha);
            dbProvider.AddInParameter(command, helper.Empuldesv, DbType.Decimal, entity.Empuldesv);
            dbProvider.AddInParameter(command, helper.Empulprog, DbType.Decimal, entity.Empulprog);
            dbProvider.AddInParameter(command, helper.Empulejec, DbType.Decimal, entity.Empulejec);
            dbProvider.AddInParameter(command, helper.Tipoccodi, DbType.Int32, entity.Tipoccodi);
            dbProvider.AddInParameter(command, helper.Vermcodi, DbType.Int32, entity.Vermcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Empulcodi, DbType.Int32, entity.Empulcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int empulcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Empulcodi, DbType.Int32, empulcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MapEmpresaulDTO GetById(int empulcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Empulcodi, DbType.Int32, empulcodi);
            MapEmpresaulDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MapEmpresaulDTO> List()
        {
            List<MapEmpresaulDTO> entitys = new List<MapEmpresaulDTO>();
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

        public List<MapEmpresaulDTO> GetByCriteria(int vermcodi)
        {
            string sqlquery = string.Format(helper.SqlGetByCriteria, vermcodi);
            List<MapEmpresaulDTO> entitys = new List<MapEmpresaulDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlquery);
            MapEmpresaulDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iTipocdesc = dr.GetOrdinal(helper.Tipocdesc);
                    if (!dr.IsDBNull(iTipocdesc)) entity.Tipocdesc = dr.GetString(iTipocdesc);
                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);
                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iEquitension = dr.GetOrdinal(helper.Equitension);
                    if (!dr.IsDBNull(iEquitension)) entity.Equitension = Convert.ToDecimal(dr.GetValue(iEquitension));
                    int iBarrcodi = dr.GetOrdinal(helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
