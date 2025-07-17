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
    /// Clase de acceso a datos de la tabla CO_CONFIGURACION_SENIAL
    /// </summary>
    public class CoConfiguracionSenialRepository: RepositoryBase, ICoConfiguracionSenialRepository
    {
        public CoConfiguracionSenialRepository(string strConn): base(strConn)
        {
        }

        CoConfiguracionSenialHelper helper = new CoConfiguracionSenialHelper();

        public int Save(CoConfiguracionSenialDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Consencodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Courdecodi, DbType.Int32, entity.Courdecodi);
            dbProvider.AddInParameter(command, helper.Cotidacodi, DbType.Int32, entity.Cotidacodi);
            dbProvider.AddInParameter(command, helper.Zonacodi, DbType.Int32, entity.Zonacodi);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Consenvalinicial, DbType.Decimal, entity.Consenvalinicial);
            dbProvider.AddInParameter(command, helper.Consenusucreacion, DbType.String, entity.Consenusucreacion);
            dbProvider.AddInParameter(command, helper.Consenfeccreacion, DbType.DateTime, entity.Consenfeccreacion);
            dbProvider.AddInParameter(command, helper.Consenusumodificacion, DbType.String, entity.Consenusumodificacion);
            dbProvider.AddInParameter(command, helper.Consenfecmodificacion, DbType.DateTime, entity.Consenfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CoConfiguracionSenialDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Consencodi, DbType.Int32, entity.Consencodi);
            dbProvider.AddInParameter(command, helper.Courdecodi, DbType.Int32, entity.Courdecodi);
            dbProvider.AddInParameter(command, helper.Cotidacodi, DbType.Int32, entity.Cotidacodi);
            dbProvider.AddInParameter(command, helper.Zonacodi, DbType.Int32, entity.Zonacodi);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Consenvalinicial, DbType.Decimal, entity.Consenvalinicial);
            dbProvider.AddInParameter(command, helper.Consenusucreacion, DbType.String, entity.Consenusucreacion);
            dbProvider.AddInParameter(command, helper.Consenfeccreacion, DbType.DateTime, entity.Consenfeccreacion);
            dbProvider.AddInParameter(command, helper.Consenusumodificacion, DbType.String, entity.Consenusumodificacion);
            dbProvider.AddInParameter(command, helper.Consenfecmodificacion, DbType.DateTime, entity.Consenfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int consencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Consencodi, DbType.Int32, consencodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoConfiguracionSenialDTO GetById(int consencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Consencodi, DbType.Int32, consencodi);
            CoConfiguracionSenialDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoConfiguracionSenialDTO> List()
        {
            List<CoConfiguracionSenialDTO> entitys = new List<CoConfiguracionSenialDTO>();
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

        public List<CoConfiguracionSenialDTO> GetByCriteria(int idConfiguracionDet)
        {
            List<CoConfiguracionSenialDTO> entitys = new List<CoConfiguracionSenialDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, idConfiguracionDet);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CoConfiguracionSenialDTO> ListarSeniales(int copercodi, int covercodi)
        {
            List<CoConfiguracionSenialDTO> entitys = new List<CoConfiguracionSenialDTO>();
            string sql = string.Format(helper.SqlListarSeniales, copercodi, covercodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CoConfiguracionSenialDTO> ListarSenialesPeriodosAnteriores(int anio, int mes, string strCanalcodis)
        {
            List<CoConfiguracionSenialDTO> entitys = new List<CoConfiguracionSenialDTO>();
            string sql = string.Format(helper.SqlListarSenialesPeriodosAnteriores, anio, mes, strCanalcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            CoConfiguracionSenialDTO entity = new CoConfiguracionSenialDTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iCoperanio = dr.GetOrdinal(helper.Coperanio);
                    if (!dr.IsDBNull(iCoperanio)) entity.Coperanio = dr.GetInt32(iCoperanio);

                    int iCopermes = dr.GetOrdinal(helper.Copermes);
                    if (!dr.IsDBNull(iCopermes)) entity.Copermes = dr.GetInt32(iCopermes);

                    int iCovercodi = dr.GetOrdinal(helper.Covercodi);
                    if (!dr.IsDBNull(iCovercodi)) entity.Covercodi = dr.GetInt32(iCovercodi);

                    int iCanalnomb = dr.GetOrdinal(helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CoConfiguracionSenialDTO>  ObtenerSenialesPorUrs(int idUrs)
        {
            List<CoConfiguracionSenialDTO> entitys = new List<CoConfiguracionSenialDTO>();
            string sql = string.Format(helper.SqlObtenerCanalesPorURS, idUrs);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoConfiguracionSenialDTO entity = new CoConfiguracionSenialDTO();

                    int iCanalcodi = dr.GetOrdinal(helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iCanalnomb = dr.GetOrdinal(helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
