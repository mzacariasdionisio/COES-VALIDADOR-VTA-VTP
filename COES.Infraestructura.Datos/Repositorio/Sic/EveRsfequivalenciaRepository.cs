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
    /// Clase de acceso a datos de la tabla EVE_RSFEQUIVALENCIA
    /// </summary>
    public class EveRsfequivalenciaRepository: RepositoryBase, IEveRsfequivalenciaRepository
    {
        public EveRsfequivalenciaRepository(string strConn): base(strConn)
        {
        }

        EveRsfequivalenciaHelper helper = new EveRsfequivalenciaHelper();

        public int Save(EveRsfequivalenciaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rsfequcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rsfequagccent, DbType.String, entity.Rsfequagccent);
            dbProvider.AddInParameter(command, helper.Rsfequagcuni, DbType.String, entity.Rsfequagcuni);
            dbProvider.AddInParameter(command, helper.Rsfequlastuser, DbType.String, entity.Rsfequlastuser);
            dbProvider.AddInParameter(command, helper.Rsfequlastdate, DbType.DateTime, entity.Rsfequlastdate);
            dbProvider.AddInParameter(command, helper.Rsfequindicador, DbType.String, entity.Rsfequindicador);
            dbProvider.AddInParameter(command, helper.Rsfequminimo, DbType.Decimal, entity.Rsfequminimo);
            dbProvider.AddInParameter(command, helper.Rsfequmaximo, DbType.Decimal, entity.Rsfequmaximo);
            //dbProvider.AddInParameter(command, helper.Rsfequrecurcodi, DbType.String, entity.Rsfequrecurcodi);
            dbProvider.AddInParameter(command, helper.Rsfequasignacion, DbType.String, entity.Rsfequasignacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveRsfequivalenciaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            
            dbProvider.AddInParameter(command, helper.Rsfequagccent, DbType.String, entity.Rsfequagccent);
            dbProvider.AddInParameter(command, helper.Rsfequagcuni, DbType.String, entity.Rsfequagcuni);
            dbProvider.AddInParameter(command, helper.Rsfequlastuser, DbType.String, entity.Rsfequlastuser);
            dbProvider.AddInParameter(command, helper.Rsfequlastdate, DbType.DateTime, entity.Rsfequlastdate);
            dbProvider.AddInParameter(command, helper.Rsfequindicador, DbType.String, entity.Rsfequindicador);
            dbProvider.AddInParameter(command, helper.Rsfequminimo, DbType.Decimal, entity.Rsfequminimo);
            dbProvider.AddInParameter(command, helper.Rsfequmaximo, DbType.Decimal, entity.Rsfequmaximo);
            dbProvider.AddInParameter(command, helper.Rsfequrecurcodi, DbType.String, entity.Rsfequrecurcodi);
            //dbProvider.AddInParameter(command, helper.Rsfequasignacion, DbType.String, entity.Rsfequasignacion);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rsfequcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rsfequcodi, DbType.Int32, rsfequcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveRsfequivalenciaDTO GetById(int rsfequcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, rsfequcodi);
            EveRsfequivalenciaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveRsfequivalenciaDTO> List()
        {
            List<EveRsfequivalenciaDTO> entitys = new List<EveRsfequivalenciaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveRsfequivalenciaDTO entity = helper.Create(dr);

                    int iModosoperacion = dr.GetOrdinal(helper.ModosOperacion);
                    if (!dr.IsDBNull(iModosoperacion)) entity.ModosOperacion = dr.GetString(iModosoperacion);

                    int iIndCC = dr.GetOrdinal(helper.IndCC);
                    if (!dr.IsDBNull(iIndCC)) entity.IndCC = Convert.ToInt32(dr.GetValue(iIndCC));

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveRsfequivalenciaDTO> GetByCriteria()
        {
            List<EveRsfequivalenciaDTO> entitys = new List<EveRsfequivalenciaDTO>();
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
