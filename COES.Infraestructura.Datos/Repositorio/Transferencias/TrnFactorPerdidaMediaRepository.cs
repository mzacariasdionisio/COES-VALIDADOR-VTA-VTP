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
    /// Clase de acceso a datos de la tabla TRN_FACTOR_PERDIDA_MEDIA
    /// </summary>
    public class TrnFactorPerdidaMediaRepository: RepositoryBase, ITrnFactorPerdidaMediaRepository
    {
        public TrnFactorPerdidaMediaRepository(string strConn): base(strConn)
        {
        }

        TrnFactorPerdidaMediaHelper helper = new TrnFactorPerdidaMediaHelper();

        public int Save(TrnFactorPerdidaMediaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Trnfpmcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Codentcodi, DbType.Int32, entity.Codentcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Trnfpmversion, DbType.Int32, entity.Trnfpmversion);
            dbProvider.AddInParameter(command, helper.Trnfpmvalor, DbType.Decimal, entity.Trnfpmvalor);
            dbProvider.AddInParameter(command, helper.Trnfpmobserv, DbType.String, entity.Trnfpmobserv);
            dbProvider.AddInParameter(command, helper.Trnfpmusername, DbType.String, entity.Trnfpmusername);
            dbProvider.AddInParameter(command, helper.Trnfpmfecins, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(TrnFactorPerdidaMediaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Codentcodi, DbType.Int32, entity.Codentcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Trnfpmversion, DbType.Int32, entity.Trnfpmversion);
            dbProvider.AddInParameter(command, helper.Trnfpmvalor, DbType.Decimal, entity.Trnfpmvalor);
            dbProvider.AddInParameter(command, helper.Trnfpmobserv, DbType.String, entity.Trnfpmobserv);
            dbProvider.AddInParameter(command, helper.Trnfpmusername, DbType.String, entity.Trnfpmusername);
            dbProvider.AddInParameter(command, helper.Trnfpmfecins, DbType.DateTime, entity.Trnfpmfecins);
            dbProvider.AddInParameter(command, helper.Trnfpmcodi, DbType.Int32, entity.Trnfpmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pericodi, int version)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Trnfpmversion, DbType.Int32, version);
            dbProvider.ExecuteNonQuery(command);
        }

        public TrnFactorPerdidaMediaDTO GetById(int trnfpmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Trnfpmcodi, DbType.Int32, trnfpmcodi);
            TrnFactorPerdidaMediaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TrnFactorPerdidaMediaDTO> List(int pericodi, int version)
        {
            List<TrnFactorPerdidaMediaDTO> entitys = new List<TrnFactorPerdidaMediaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Trnfpmversion, DbType.Int32, version);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrnFactorPerdidaMediaDTO entity = helper.Create(dr);
                    //Atributos adicionales

                    int iCodentcodigo = dr.GetOrdinal(this.helper.Codentcodigo);
                    if (!dr.IsDBNull(iCodentcodigo)) entity.Codentcodigo = dr.GetString(iCodentcodigo);

                    int iBarrnombre = dr.GetOrdinal(this.helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = dr.GetString(iBarrnombre);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TrnFactorPerdidaMediaDTO> GetByCriteria()
        {
            List<TrnFactorPerdidaMediaDTO> entitys = new List<TrnFactorPerdidaMediaDTO>();
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

        //ASSETEC 20190104
        public List<TrnFactorPerdidaMediaDTO> ListDesviacionesEntregas(int pericodi, int version)
        {
            List<TrnFactorPerdidaMediaDTO> entitys = new List<TrnFactorPerdidaMediaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListDesvEntregas);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Trnfpmversion, DbType.Int32, version);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrnFactorPerdidaMediaDTO entity = helper.Create(dr);
                    //Atributos adicionales

                    int iCodentcodigo = dr.GetOrdinal(this.helper.Codentcodigo);
                    if (!dr.IsDBNull(iCodentcodigo)) entity.Codentcodigo = dr.GetString(iCodentcodigo);

                    int iBarrnombre = dr.GetOrdinal(this.helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = dr.GetString(iBarrnombre);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEntregaMes = dr.GetOrdinal(this.helper.EntregaMes);
                    if (!dr.IsDBNull(iEntregaMes)) entity.EntregaMes = dr.GetDecimal(iEntregaMes);

                    int iMedidoresMes = dr.GetOrdinal(this.helper.MedidoresMes);
                    if (!dr.IsDBNull(iMedidoresMes)) entity.MedidoresMes = dr.GetDecimal(iMedidoresMes);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
