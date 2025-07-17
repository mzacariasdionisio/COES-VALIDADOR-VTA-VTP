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
    /// Clase de acceso a datos de la tabla AF_ERACMF_EVENTO
    /// </summary>
    public class AfEracmfEventoRepository : RepositoryBase, IAfEracmfEventoRepository
    {
        public AfEracmfEventoRepository(string strConn) : base(strConn)
        {
        }

        AfEracmfEventoHelper helper = new AfEracmfEventoHelper();

        public int Save(AfEracmfEventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);


            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Eracmfusumodificacion, DbType.String, entity.Eracmfusumodificacion);
            dbProvider.AddInParameter(command, helper.Eracmfusucreacion, DbType.String, entity.Eracmfusucreacion);
            dbProvider.AddInParameter(command, helper.Eracmffecmodificacion, DbType.DateTime, entity.Eracmffecmodificacion);
            dbProvider.AddInParameter(command, helper.Eracmffeccreacion, DbType.DateTime, entity.Eracmffeccreacion);
            dbProvider.AddInParameter(command, helper.Eracmfcodrele, DbType.String, entity.Eracmfcodrele);
            dbProvider.AddInParameter(command, helper.Eracmftiporegistro, DbType.String, entity.Eracmftiporegistro);
            dbProvider.AddInParameter(command, helper.Eracmffechretiro, DbType.String, entity.Eracmffechretiro);
            dbProvider.AddInParameter(command, helper.Eracmffechingreso, DbType.String, entity.Eracmffechingreso);
            dbProvider.AddInParameter(command, helper.Eracmffechimplementacion, DbType.String, entity.Eracmffechimplementacion);
            dbProvider.AddInParameter(command, helper.Eracmfobservaciones, DbType.String, entity.Eracmfobservaciones);
            dbProvider.AddInParameter(command, helper.Eracmfsuministrador, DbType.String, entity.Eracmfsuministrador);
            dbProvider.AddInParameter(command, helper.Eracmfdreferencia, DbType.Decimal, entity.Eracmfdreferencia);
            dbProvider.AddInParameter(command, helper.Eracmfmindregistrada, DbType.Decimal, entity.Eracmfmindregistrada);
            dbProvider.AddInParameter(command, helper.Eracmfmediadregistrada, DbType.Decimal, entity.Eracmfmediadregistrada);
            dbProvider.AddInParameter(command, helper.Eracmfmaxdregistrada, DbType.Decimal, entity.Eracmfmaxdregistrada);
            dbProvider.AddInParameter(command, helper.Eracmftiemporderivada, DbType.Decimal, entity.Eracmftiemporderivada);
            dbProvider.AddInParameter(command, helper.Eracmfdfdtrderivada, DbType.Decimal, entity.Eracmfdfdtrderivada);
            dbProvider.AddInParameter(command, helper.Eracmfarranqrderivada, DbType.Decimal, entity.Eracmfarranqrderivada);
            dbProvider.AddInParameter(command, helper.Eracmftiemporumbral, DbType.Decimal, entity.Eracmftiemporumbral);
            dbProvider.AddInParameter(command, helper.Eracmfarranqrumbral, DbType.Decimal, entity.Eracmfarranqrumbral);
            dbProvider.AddInParameter(command, helper.Eracmfnumetapa, DbType.String, entity.Eracmfnumetapa);
            dbProvider.AddInParameter(command, helper.Eracmfcodinterruptor, DbType.String, entity.Eracmfcodinterruptor);
            dbProvider.AddInParameter(command, helper.Eracmfciralimentador, DbType.String, entity.Eracmfciralimentador);
            dbProvider.AddInParameter(command, helper.Eracmfnivtension, DbType.String, entity.Eracmfnivtension);
            dbProvider.AddInParameter(command, helper.Eracmfsubestacion, DbType.String, entity.Eracmfsubestacion);
            dbProvider.AddInParameter(command, helper.Eracmfnroserie, DbType.String, entity.Eracmfnroserie);
            dbProvider.AddInParameter(command, helper.Eracmfmodelo, DbType.String, entity.Eracmfmodelo);
            dbProvider.AddInParameter(command, helper.Eracmfmarca, DbType.String, entity.Eracmfmarca);
            dbProvider.AddInParameter(command, helper.Eracmfzona, DbType.String, entity.Eracmfzona);
            dbProvider.AddInParameter(command, helper.Eracmfemprnomb, DbType.String, entity.Eracmfemprnomb);
            dbProvider.AddInParameter(command, helper.Eracmfcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AfEracmfEventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Eracmfusumodificacion, DbType.String, entity.Eracmfusumodificacion);
            dbProvider.AddInParameter(command, helper.Eracmfusucreacion, DbType.String, entity.Eracmfusucreacion);
            dbProvider.AddInParameter(command, helper.Eracmffecmodificacion, DbType.DateTime, entity.Eracmffecmodificacion);
            dbProvider.AddInParameter(command, helper.Eracmffeccreacion, DbType.DateTime, entity.Eracmffeccreacion);
            dbProvider.AddInParameter(command, helper.Eracmfcodrele, DbType.String, entity.Eracmfcodrele);
            dbProvider.AddInParameter(command, helper.Eracmftiporegistro, DbType.String, entity.Eracmftiporegistro);
            dbProvider.AddInParameter(command, helper.Eracmffechretiro, DbType.String, entity.Eracmffechretiro);
            dbProvider.AddInParameter(command, helper.Eracmffechingreso, DbType.String, entity.Eracmffechingreso);
            dbProvider.AddInParameter(command, helper.Eracmffechimplementacion, DbType.String, entity.Eracmffechimplementacion);
            dbProvider.AddInParameter(command, helper.Eracmfobservaciones, DbType.String, entity.Eracmfobservaciones);
            dbProvider.AddInParameter(command, helper.Eracmfsuministrador, DbType.String, entity.Eracmfsuministrador);
            dbProvider.AddInParameter(command, helper.Eracmfdreferencia, DbType.Decimal, entity.Eracmfdreferencia);
            dbProvider.AddInParameter(command, helper.Eracmfmindregistrada, DbType.Decimal, entity.Eracmfmindregistrada);
            dbProvider.AddInParameter(command, helper.Eracmfmediadregistrada, DbType.Decimal, entity.Eracmfmediadregistrada);
            dbProvider.AddInParameter(command, helper.Eracmfmaxdregistrada, DbType.Decimal, entity.Eracmfmaxdregistrada);
            dbProvider.AddInParameter(command, helper.Eracmftiemporderivada, DbType.Decimal, entity.Eracmftiemporderivada);
            dbProvider.AddInParameter(command, helper.Eracmfdfdtrderivada, DbType.Decimal, entity.Eracmfdfdtrderivada);
            dbProvider.AddInParameter(command, helper.Eracmfarranqrderivada, DbType.Decimal, entity.Eracmfarranqrderivada);
            dbProvider.AddInParameter(command, helper.Eracmftiemporumbral, DbType.Decimal, entity.Eracmftiemporumbral);
            dbProvider.AddInParameter(command, helper.Eracmfarranqrumbral, DbType.Decimal, entity.Eracmfarranqrumbral);
            dbProvider.AddInParameter(command, helper.Eracmfnumetapa, DbType.String, entity.Eracmfnumetapa);
            dbProvider.AddInParameter(command, helper.Eracmfcodinterruptor, DbType.String, entity.Eracmfcodinterruptor);
            dbProvider.AddInParameter(command, helper.Eracmfciralimentador, DbType.String, entity.Eracmfciralimentador);
            dbProvider.AddInParameter(command, helper.Eracmfnivtension, DbType.String, entity.Eracmfnivtension);
            dbProvider.AddInParameter(command, helper.Eracmfsubestacion, DbType.String, entity.Eracmfsubestacion);
            dbProvider.AddInParameter(command, helper.Eracmfnroserie, DbType.String, entity.Eracmfnroserie);
            dbProvider.AddInParameter(command, helper.Eracmfmodelo, DbType.String, entity.Eracmfmodelo);
            dbProvider.AddInParameter(command, helper.Eracmfmarca, DbType.String, entity.Eracmfmarca);
            dbProvider.AddInParameter(command, helper.Eracmfzona, DbType.String, entity.Eracmfzona);
            dbProvider.AddInParameter(command, helper.Eracmfemprnomb, DbType.String, entity.Eracmfemprnomb);
            dbProvider.AddInParameter(command, helper.Eracmfcodi, DbType.Int32, entity.Eracmfcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int eracmfcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Eracmfcodi, DbType.Int32, eracmfcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AfEracmfEventoDTO GetById(int eracmfcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Eracmfcodi, DbType.Int32, eracmfcodi);
            AfEracmfEventoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AfEracmfEventoDTO> List()
        {
            List<AfEracmfEventoDTO> entitys = new List<AfEracmfEventoDTO>();
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

        public List<AfEracmfEventoDTO> GetByCriteria()
        {
            List<AfEracmfEventoDTO> entitys = new List<AfEracmfEventoDTO>();
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

        /// <summary>
        /// Devuelve lista eracmf deacuerdo a su evencodi
        /// </summary>
        /// <param name="evencodi"></param>
        /// <returns></returns>
        public List<AfEracmfEventoDTO> GetByEvencodi(int evencodi)
        {
            List<AfEracmfEventoDTO> entitys = new List<AfEracmfEventoDTO>();


            String sql = String.Format(helper.SqlGetByEvencodi, evencodi);
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


        public List<AfEracmfEventoDTO> GetByEvento(int emprcodi, int evencodi)
        {
            List<AfEracmfEventoDTO> entitys = new List<AfEracmfEventoDTO>();
            var query = string.Format(helper.SqlGetByEvento, evencodi, emprcodi);
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
