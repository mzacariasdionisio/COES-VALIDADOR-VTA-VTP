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
    /// Clase de acceso a datos de la tabla RCA_CUADRO_EJEC_USU_DET
    /// </summary>
    public class RcaCuadroEjecUsuarioDetRepository: RepositoryBase, IRcaCuadroEjecUsuarioDetRepository
    {
        public RcaCuadroEjecUsuarioDetRepository(string strConn): base(strConn)
        {
        }

        RcaCuadroEjecUsuarioDetHelper helper = new RcaCuadroEjecUsuarioDetHelper();

        public int Save(RcaCuadroEjecUsuarioDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rcejedcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rcejeucodi, DbType.Int32, entity.Rcejeucodi);
            //dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            //dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rcejedfechor, DbType.DateTime, entity.Rcejedfechor);
            dbProvider.AddInParameter(command, helper.Rcejedpotencia, DbType.Decimal, entity.Rcejedpotencia);
           
            dbProvider.AddInParameter(command, helper.Rcejedusucreacion, DbType.String, entity.Rcejedusucreacion);
            dbProvider.AddInParameter(command, helper.Rcejedfeccreacion, DbType.DateTime, entity.Rcejedfeccreacion);
            //dbProvider.AddInParameter(command, helper.Rcejeuusumodificacion, DbType.String, entity.Rcejeuusumodificacion);
            //dbProvider.AddInParameter(command, helper.Rcejeufecmodificacion, DbType.DateTime, entity.Rcejeufecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RcaCuadroEjecUsuarioDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

           
            dbProvider.AddInParameter(command, helper.Rcejeucodi, DbType.Int32, entity.Rcejeucodi);
            //dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            //dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rcejedfechor, DbType.DateTime, entity.Rcejedfechor);
            dbProvider.AddInParameter(command, helper.Rcejedpotencia, DbType.Decimal, entity.Rcejedpotencia);
            //dbProvider.AddInParameter(command, helper.Rcejedusucreacion, DbType.String, entity.Rcejedusucreacion);
            //dbProvider.AddInParameter(command, helper.Rcejedfeccreacion, DbType.DateTime, entity.Rcejedfeccreacion);
            dbProvider.AddInParameter(command, helper.Rcejedusumodificacion, DbType.String, entity.Rcejedusumodificacion);
            dbProvider.AddInParameter(command, helper.Rcejedfecmodificacion, DbType.DateTime, entity.Rcejedfecmodificacion);
            dbProvider.AddInParameter(command, helper.Rcejedcodi, DbType.Int32, entity.Rcejedcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rcejedcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rcejedcodi, DbType.Int32, rcejedcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RcaCuadroEjecUsuarioDetDTO GetById(int rcejedcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rcejedcodi, DbType.Int32, rcejedcodi);
            RcaCuadroEjecUsuarioDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RcaCuadroEjecUsuarioDetDTO> List()
        {
            List<RcaCuadroEjecUsuarioDetDTO> entitys = new List<RcaCuadroEjecUsuarioDetDTO>();
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

        public List<RcaCuadroEjecUsuarioDetDTO> GetByCriteria()
        {
            List<RcaCuadroEjecUsuarioDetDTO> entitys = new List<RcaCuadroEjecUsuarioDetDTO>();
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

        public List<RcaCuadroEjecUsuarioDetDTO> ListFiltro(int codigoCuadroEjecucion)
        {
            List<RcaCuadroEjecUsuarioDetDTO> entitys = new List<RcaCuadroEjecUsuarioDetDTO>();
            var condicion = string.Empty;

            //if (!string.IsNullOrEmpty(cuadroPrograma))
            //{
            //    condicion = condicion + " AND CP.RCCUADCODI = " + cuadroPrograma;
            //}                       

            string queryString = string.Format(helper.SqlListFiltro, condicion);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, helper.Rcejeucodi, DbType.Int32, codigoCuadroEjecucion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaCuadroEjecUsuarioDetDTO entity = new RcaCuadroEjecUsuarioDetDTO();

                    int iRcejeucodi = dr.GetOrdinal(helper.Rcejeucodi);
                    if (!dr.IsDBNull(iRcejeucodi)) entity.Rcejeucodi = Convert.ToInt32(dr.GetValue(iRcejeucodi));

                    int iRcejedcodi = dr.GetOrdinal(helper.Rcejedcodi);
                    if (!dr.IsDBNull(iRcejedcodi)) entity.Rcejedcodi = Convert.ToInt32(dr.GetValue(iRcejedcodi));

                    //int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
                    //if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));           

                    int iRcejedpotencia = dr.GetOrdinal(helper.Rcejedpotencia);
                    if (!dr.IsDBNull(iRcejedpotencia)) entity.Rcejedpotencia = dr.GetDecimal(iRcejedpotencia);

                    int iRcejedfechor = dr.GetOrdinal(helper.Rcejedfechor);
                    if (!dr.IsDBNull(iRcejedfechor)) entity.Rcejedfechor = dr.GetDateTime(iRcejedfechor);

                    //int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    //if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void DeletePorCliente(int rcejeucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeletePorCliente);

            dbProvider.AddInParameter(command, helper.Rcejedcodi, DbType.Int32, rcejeucodi);

            dbProvider.ExecuteNonQuery(command);
        }
    }
}
