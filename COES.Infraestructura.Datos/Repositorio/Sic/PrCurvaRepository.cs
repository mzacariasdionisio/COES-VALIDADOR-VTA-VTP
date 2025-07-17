using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Framework.Base.Tools;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla PR_CURVA
    /// </summary>
    public class PrCurvaRepository : RepositoryBase, IPrCurvaRepository
    {
        public PrCurvaRepository(string strConn)
            : base(strConn)
        {
        }

        PrCurvaHelper helper = new PrCurvaHelper();

        public int Save(PrCurvaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.CurvCodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.CurvNombre, DbType.String, entity.Curvnombre);
            dbProvider.AddInParameter(command, helper.CurvEstado, DbType.String, entity.Curvestado);
            dbProvider.AddInParameter(command, helper.CurvUsuCreacion, DbType.String, entity.Curvusucreacion);
            dbProvider.AddInParameter(command, helper.CurvFecCreacion, DbType.DateTime, entity.Curvfeccreacion);
            dbProvider.AddInParameter(command, helper.CurvUsuModificacion, DbType.String, entity.Curvusumodificacion);
            dbProvider.AddInParameter(command, helper.CurvFecModificacion, DbType.DateTime, entity.Curvfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrCurvaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
                        
            dbProvider.AddInParameter(command, helper.CurvNombre, DbType.String, entity.Curvnombre);
            dbProvider.AddInParameter(command, helper.CurvEstado, DbType.String, entity.Curvestado);
            dbProvider.AddInParameter(command, helper.CurvUsuCreacion, DbType.String, entity.Curvusucreacion);
            dbProvider.AddInParameter(command, helper.CurvFecCreacion, DbType.DateTime, entity.Curvfeccreacion);
            dbProvider.AddInParameter(command, helper.CurvUsuModificacion, DbType.String, entity.Curvusumodificacion);
            dbProvider.AddInParameter(command, helper.CurvFecModificacion, DbType.DateTime, entity.Curvfecmodificacion);
            dbProvider.AddInParameter(command, helper.CurvCodi, DbType.Int32, entity.Curvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int curvacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.CurvCodi, DbType.Int32, curvacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteDetail(int curvacodi, int grupocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteDetail);

            dbProvider.AddInParameter(command, helper.GrupoCodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.CurvCodi, DbType.Int32, curvacodi);


            dbProvider.ExecuteNonQuery(command);
        }

        public PrCurvaDTO GetById(int curvacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.CurvCodi, DbType.Int32, curvacodi);
            PrCurvaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrCurvaDTO> List()
        {
            List<PrCurvaDTO> entitys = new List<PrCurvaDTO>();
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

        public void AddDetail(int curvacodi, int grupocodi)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlAddDetail);

            dbProvider.AddInParameter(command, helper.CurvCodi, DbType.Int32, curvacodi);
            dbProvider.AddInParameter(command, helper.GrupoCodi, DbType.Int32, grupocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdatePrincipal(int curvacodi, int grupocodi)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdatePrincipal);

            dbProvider.AddInParameter(command, helper.GrupoCodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.CurvCodi, DbType.Int32, curvacodi);

            dbProvider.ExecuteNonQuery(command);
        }


    }
}
