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
    /// Clase de acceso a datos de la tabla SIO_DATOPRIE
    /// </summary>
    public class SioDatoprieRepository : RepositoryBase, ISioDatoprieRepository
    {
        public SioDatoprieRepository(string strConn)
            : base(strConn)
        {
        }

        SioDatoprieHelper helper = new SioDatoprieHelper();

        public int Save(SioDatoprieDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Dpriecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Dprievalor, DbType.String, entity.Dprievalor);
            dbProvider.AddInParameter(command, helper.Dprieperiodo, DbType.DateTime, entity.Dprieperiodo);
            dbProvider.AddInParameter(command, helper.Dpriefechadia, DbType.DateTime, entity.Dpriefechadia);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi2, DbType.Int32, entity.Emprcodi2);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Dprieusuario, DbType.String, entity.Dprieusuario);
            dbProvider.AddInParameter(command, helper.Dpriefecha, DbType.DateTime, entity.Dpriefecha);
            dbProvider.AddInParameter(command, helper.Cabpricodi, DbType.Int32, entity.Cabpricodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SioDatoprieDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Dpriecodi, DbType.Int32, entity.Dpriecodi);
            dbProvider.AddInParameter(command, helper.Dprievalor, DbType.String, entity.Dprievalor);
            dbProvider.AddInParameter(command, helper.Dprieperiodo, DbType.DateTime, entity.Dprieperiodo);
            dbProvider.AddInParameter(command, helper.Dpriefechadia, DbType.DateTime, entity.Dpriefechadia);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Dprieusuario, DbType.String, entity.Dprieusuario);
            dbProvider.AddInParameter(command, helper.Dpriefecha, DbType.DateTime, entity.Dpriefecha);
            dbProvider.AddInParameter(command, helper.Cabpricodi, DbType.Int32, entity.Cabpricodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int dpriecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Dpriecodi, DbType.Int32, dpriecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SioDatoprieDTO GetById(int dpriecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Dpriecodi, DbType.Int32, dpriecodi);
            SioDatoprieDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SioDatoprieDTO> List()
        {
            List<SioDatoprieDTO> entitys = new List<SioDatoprieDTO>();
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

        public List<SioDatoprieDTO> GetByCriteria()
        {
            List<SioDatoprieDTO> entitys = new List<SioDatoprieDTO>();
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

        public List<SioDatoprieDTO> GetByCabpricodi(string equicodi, string cabpricodi)
        {
            List<SioDatoprieDTO> entitys = new List<SioDatoprieDTO>();
            string query = string.Format(helper.SqlGetByCabpricodi, equicodi, cabpricodi);
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

        public int ValidarDataPorCodigoCabecera(int Dpriecodi)
        {
            int count = 0;

            string query = string.Format(helper.SqlValidarDataPorCodigoCabecera, Dpriecodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iCount = dr.GetOrdinal(helper.Count);
                    if (!dr.IsDBNull(iCount)) count = Convert.ToInt32(dr.GetValue(iCount));
                }

            }

            return count;
        }

        public IDataReader GetDataReader(int Cabpricodi)
        {
            string query = string.Format(helper.SqlGetByCabpricodi, "-1", Cabpricodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            IDataReader dr = dbProvider.ExecuteReader(command);
            return dr;
        }

        public int BorrarDataPorCodigoCabecera(int Cabpricodi)
        {
            int exito = 0;
            string query = string.Format(helper.SqlBorrarDataPorCodigoCabecera, Cabpricodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            exito = dbProvider.ExecuteNonQuery(command);

            return exito;
        }

        public List<SioDatoprieDTO> GetListaDifusionEnergPrie(int lectcodi, DateTime dfecha, string familia)
        {
            List<SioDatoprieDTO> entitys = new List<SioDatoprieDTO>();
            SioDatoprieDTO entity;
            string query = string.Format(helper.SqlGetListaDifusionEnergPrie, lectcodi, dfecha.ToString(ConstantesBase.FormatoFecha), familia);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new SioDatoprieDTO();
                    entity.Equicodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("Equicodi")));
                    entity.Equinomb = dr.GetString(dr.GetOrdinal("Equinomb"));
                    entity.Emprcodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("Emprcodi")));
                    entity.Emprnomb = dr.GetString(dr.GetOrdinal("Emprnomb"));
                    entity.Fenergnomb = dr.GetString(dr.GetOrdinal("Fenergnomb"));
                    entity.Fenergcodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("Fenergcodi")));
                    entity.Dprieperiodo = dr.GetDateTime(dr.GetOrdinal("Dprieperiodo"));
                    entity.Dpriefechadia = dr.GetDateTime(dr.GetOrdinal("Dpriefechadia"));
                    entity.Dprievalor = dr.GetString(dr.GetOrdinal("Dprievalor"));
                    entity.Tgenercodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("TGENERCODI")));
                    entity.Tgenernomb = dr.GetString(dr.GetOrdinal("TGENERNOMB"));

                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        ////////////FIN TABLA 26 /////////////

        public List<SioDatoprieDTO> GetListaByCabpricodi(string equicodi, string cabpricodi)
        {
            List<SioDatoprieDTO> entitys = new List<SioDatoprieDTO>();
            string query = string.Format(helper.SqlGetListaByCabpricodi, equicodi, cabpricodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    // entitys.Add(helper.Create(dr));

                    SioDatoprieDTO entity = new SioDatoprieDTO();

                    int iDpriecodi = dr.GetOrdinal("Dpriecodi");
                    if (!dr.IsDBNull(iDpriecodi)) entity.Dpriecodi = Convert.ToInt32(dr.GetValue(iDpriecodi));

                    int iDprievalor = dr.GetOrdinal("Dprievalor");
                    if (!dr.IsDBNull(iDprievalor)) entity.Dprievalor = dr.GetString(iDprievalor);

                    int iDprieperiodo = dr.GetOrdinal("Dprieperiodo");
                    if (!dr.IsDBNull(iDprieperiodo)) entity.Dprieperiodo = dr.GetDateTime(iDprieperiodo);

                    int iDpriefechadia = dr.GetOrdinal("Dpriefechadia");
                    if (!dr.IsDBNull(iDpriefechadia)) entity.Dpriefechadia = dr.GetDateTime(iDpriefechadia);

                    int iEquicodi = dr.GetOrdinal("Equicodi");
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iGrupocodi = dr.GetOrdinal("Grupocodi");
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iDprieusuario = dr.GetOrdinal("dprieusucreacion");
                    if (!dr.IsDBNull(iDprieusuario)) entity.Dprieusuario = dr.GetString(iDprieusuario);

                    int iDpriefecha = dr.GetOrdinal("dpriefeccreacion");
                    if (!dr.IsDBNull(iDpriefecha)) entity.Dpriefecha = dr.GetDateTime(iDpriefecha);

                    int iCabpricodi = dr.GetOrdinal("Cabpricodi");
                    if (!dr.IsDBNull(iCabpricodi)) entity.Cabpricodi = Convert.ToInt32(dr.GetValue(iCabpricodi));

                    int iEmprambito = dr.GetOrdinal("emprambito");
                    if (!dr.IsDBNull(iEmprambito)) entity.Emprambito = dr.GetString(iEmprambito);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SioDatoprieDTO> GetListaDifusionEnergPrieByFiltro(int lectcodi, DateTime dfecha, string familia, string idEmpresa, string tipoGene, string recenerg)
        {
            List<SioDatoprieDTO> entitys = new List<SioDatoprieDTO>();
            SioDatoprieDTO entity;
            string query = string.Format(helper.SqlGetListaDifusionEnergPrieByFiltro, lectcodi, dfecha.ToString(ConstantesBase.FormatoFecha), familia, idEmpresa, tipoGene, recenerg);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new SioDatoprieDTO();
                    entity.Equicodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("Equicodi")));
                    entity.Equinomb = dr.GetString(dr.GetOrdinal("Equinomb"));
                    entity.Emprcodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("Emprcodi")));
                    entity.Emprnomb = dr.GetString(dr.GetOrdinal("Emprnomb"));
                    entity.Fenergnomb = dr.GetString(dr.GetOrdinal("Fenergnomb"));
                    entity.Fenergcodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("Fenergcodi")));
                    entity.Dprieperiodo = dr.GetDateTime(dr.GetOrdinal("Dprieperiodo"));
                    entity.Dpriefechadia = dr.GetDateTime(dr.GetOrdinal("Dpriefechadia"));
                    entity.Dprievalor = dr.GetString(dr.GetOrdinal("Dprievalor"));
                    entity.Tgenercodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("TGENERCODI")));
                    entity.Tgenernomb = dr.GetString(dr.GetOrdinal("TGENERNOMB"));

                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public List<SioDatoprieDTO> GetByEmpLectPtomedFechaOrden(string idEmpresa, int lectcodi, string ptomedicodi, DateTime dfecha, string orden)
        {
            List<SioDatoprieDTO> entitys = new List<SioDatoprieDTO>();
            SioDatoprieDTO entity;
            string query = string.Format(helper.SqlGetByLectPtomedFechaOrdenPrie, lectcodi, dfecha.ToString(ConstantesBase.FormatoFecha), idEmpresa, ptomedicodi, orden);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new SioDatoprieDTO();
                    entity.Equicodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("Equicodi")));
                    entity.Equinomb = dr.GetString(dr.GetOrdinal("Equinomb"));
                    entity.Emprcodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("Emprcodi")));
                    entity.Emprnomb = dr.GetString(dr.GetOrdinal("Emprnomb"));
                    entity.Fenergnomb = dr.GetString(dr.GetOrdinal("Fenergnomb"));
                    entity.Fenergcodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("Fenergcodi")));
                    entity.Dprieperiodo = dr.GetDateTime(dr.GetOrdinal("Dprieperiodo"));
                    entity.Dpriefechadia = dr.GetDateTime(dr.GetOrdinal("Dpriefechadia"));
                    entity.Orden = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("Orden")));
                    entity.Dprievalor = dr.GetString(dr.GetOrdinal("Dprievalor"));
                    entity.Tgenercodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("TGENERCODI")));
                    entity.Tgenernomb = dr.GetString(dr.GetOrdinal("TGENERNOMB"));

                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public List<SioDatoprieDTO> GetListaDifusionCostoMarginal(string cabprie, DateTime dfecIniMes, DateTime dfecFinMes, string idBarra, string Tensiones, string idAreas)
        {
            List<SioDatoprieDTO> entitys = new List<SioDatoprieDTO>();
            SioDatoprieDTO entity;
            string query = string.Format(helper.SqlGetListaDifusionCostoMarginal, cabprie, dfecIniMes.ToString(ConstantesBase.FormatoFecha), dfecFinMes.ToString(ConstantesBase.FormatoFecha), idBarra, Tensiones, idAreas);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    entity = new SioDatoprieDTO();

                    int iCabpricodi = dr.GetOrdinal("Cabpricodi");
                    if (!dr.IsDBNull(iCabpricodi)) entity.Cabpricodi = Convert.ToInt32(dr.GetValue(iCabpricodi));

                    int iAreanomb = dr.GetOrdinal("Areanomb");
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iAreacodi = dr.GetOrdinal("Areacodi");
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iBarrtension = dr.GetOrdinal("Barrtension");
                    if (!dr.IsDBNull(iBarrtension)) entity.Barrtension = dr.GetString(iBarrtension);

                    int iEquicodi = dr.GetOrdinal("Equicodi");
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iDpriecodi = dr.GetOrdinal("Dpriecodi");
                    if (!dr.IsDBNull(iDpriecodi)) entity.Dpriecodi = Convert.ToInt32(dr.GetValue(iDpriecodi));

                    int iDprieperiodo = dr.GetOrdinal("Dprieperiodo");
                    if (!dr.IsDBNull(iDprieperiodo)) entity.Dprieperiodo = dr.GetDateTime(iDprieperiodo);

                    int iDpriefechadia = dr.GetOrdinal("Dpriefechadia");
                    if (!dr.IsDBNull(iDpriefechadia)) entity.Dpriefechadia = dr.GetDateTime(iDpriefechadia);

                    int iDprievalor = dr.GetOrdinal("Dprievalor");
                    if (!dr.IsDBNull(iDprievalor)) entity.Dprievalor = dr.GetString(iDprievalor);

                    int iGrupocodi = dr.GetOrdinal("Grupocodi");
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public List<SioDatoprieDTO> GetCostoVariableByFiltro(string cabprie, DateTime dfecIniMes, DateTime dfecFinMes, string modoOpe, string tipoCombustible, string precioComb, string tipoCostoVar)
        {
            List<SioDatoprieDTO> entitys = new List<SioDatoprieDTO>();
            SioDatoprieDTO entity;
            string query = string.Format(helper.SqlGetCostoVariableByFiltro, cabprie, dfecIniMes.ToString(ConstantesBase.FormatoFecha), tipoCombustible, modoOpe);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new SioDatoprieDTO();
                    entity.Grupocodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("grupocodi")));
                    entity.Gruponomb = dr.GetString(dr.GetOrdinal("gruponomb"));
                    entity.Fenergcodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("Fenergcodi")));
                    entity.Fenergnomb = dr.GetString(dr.GetOrdinal("Fenergnomb"));
                    entity.Dprievalor = dr.GetString(dr.GetOrdinal("Dprievalor"));
                    entity.Dprieperiodo = dr.GetDateTime(dr.GetOrdinal("Dprieperiodo"));
                    entity.Dpriefechadia = dr.GetDateTime(dr.GetOrdinal("Dpriefechadia"));
                    entity.Cabpricodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("cabpricodi")));

                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public List<SioDatoprieDTO> GetDifusionTransfPotencia(string equicodi, int cabpricodi)
        {
            List<SioDatoprieDTO> entitys = new List<SioDatoprieDTO>();
            string query = string.Format(helper.SqlGetDifusionTransfPotencia, equicodi, cabpricodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    // entitys.Add(helper.Create(dr));

                    SioDatoprieDTO entity = new SioDatoprieDTO();

                    int iDpriecodi = dr.GetOrdinal("Dpriecodi");
                    if (!dr.IsDBNull(iDpriecodi)) entity.Dpriecodi = Convert.ToInt32(dr.GetValue(iDpriecodi));

                    int iDprievalor = dr.GetOrdinal("Dprievalor");
                    if (!dr.IsDBNull(iDprievalor)) entity.Dprievalor = dr.GetString(iDprievalor);

                    int iDprieperiodo = dr.GetOrdinal("Dprieperiodo");
                    if (!dr.IsDBNull(iDprieperiodo)) entity.Dprieperiodo = dr.GetDateTime(iDprieperiodo);

                    int iDpriefechadia = dr.GetOrdinal("Dpriefechadia");
                    if (!dr.IsDBNull(iDpriefechadia)) entity.Dpriefechadia = dr.GetDateTime(iDpriefechadia);

                    int iEquicodi = dr.GetOrdinal("Equicodi");
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iGrupocodi = dr.GetOrdinal("Grupocodi");
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iDprieusuario = dr.GetOrdinal("dprieusucreacion");
                    if (!dr.IsDBNull(iDprieusuario)) entity.Dprieusuario = dr.GetString(iDprieusuario);

                    int iDpriefecha = dr.GetOrdinal("dpriefeccreacion");
                    if (!dr.IsDBNull(iDpriefecha)) entity.Dpriefecha = dr.GetDateTime(iDpriefecha);

                    int iCabpricodi = dr.GetOrdinal("Cabpricodi");
                    if (!dr.IsDBNull(iCabpricodi)) entity.Cabpricodi = Convert.ToInt32(dr.GetValue(iCabpricodi));

                    int iEmprambito = dr.GetOrdinal("emprambito");
                    if (!dr.IsDBNull(iEmprambito)) entity.Emprambito = dr.GetString(iEmprambito);

                    int iEmprambitoc = dr.GetOrdinal("emprambitoc");
                    if (!dr.IsDBNull(iEmprambitoc)) entity.Emprambc = dr.GetString(iEmprambitoc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int Save(SioDatoprieDTO entity, IDbConnection connection, IDbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Dpriecodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Dprievalor, DbType.String, entity.Dprievalor));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Dprieperiodo, DbType.DateTime, entity.Dprieperiodo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Dpriefechadia, DbType.DateTime, entity.Dpriefechadia));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equicodi, DbType.Int32, entity.Equicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Barrcodi, DbType.Int32, entity.Barrcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Emprcodi2, DbType.Int32, entity.Emprcodi2));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Dprieusuario, DbType.String, entity.Dprieusuario));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Dpriefecha, DbType.DateTime, entity.Dpriefecha));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cabpricodi, DbType.Int32, entity.Cabpricodi));

                dbCommand.ExecuteNonQuery();
                return id;

            }
        }

        public void Delete(DateTime periodo, int tpriecodi, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = string.Format(helper.SqlDeleteByPeriodoAndPriecodi, periodo.ToString(ConstantesBase.FormatoFecha), tpriecodi);
                dbCommand.ExecuteNonQuery();
            }
        }

        public List<SioDatoprieDTO> GetSioDatosprieByCriteria(int cabpricodi, string equicodi, string grupocodi, string barrcodi, string emprcodi)
        {
            List<SioDatoprieDTO> entitys = new List<SioDatoprieDTO>();
            string query = string.Format(helper.SqlGetSioDatosprieByCriteria, cabpricodi, equicodi, grupocodi, barrcodi, emprcodi);
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

        #region métodos "SIOSEIN-PRIE-2021"
        public List<SioDatoprieDTO> GetByCabpricodi2(string equicodi, string cabpricodi)
        {
            List<SioDatoprieDTO> entitys = new List<SioDatoprieDTO>();
            string query = string.Format(helper.SqlGetByCabpricodi2, equicodi, cabpricodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SioDatoprieDTO entity = new SioDatoprieDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iTipogrupocodi = dr.GetOrdinal(helper.Tipogrupocodi);
                    if (!dr.IsDBNull(iTipogrupocodi)) entity.Tipogrupocodi = Convert.ToInt32(dr.GetValue(iTipogrupocodi));

                    int iTipogenerrer = dr.GetOrdinal(helper.Tipogenerrer);
                    if (!dr.IsDBNull(iTipogenerrer)) entity.Tipogenerrer = dr.GetString(iTipogenerrer);

                    int iTgenercodi = dr.GetOrdinal(helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

                    int iTgenernomb = dr.GetOrdinal(helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region SIOSEIN-PRIE-2021
        public List<SioDatoprieDTO> GetReporteRR05ByOsinergcodi(string osinergcodi)
        {
            List<SioDatoprieDTO> entitys = new List<SioDatoprieDTO>();
            string query = string.Format(helper.SqlGetReporteRR05ByOsinergcodi, osinergcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    SioDatoprieDTO entity = new SioDatoprieDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iCodcentral = dr.GetOrdinal(helper.Codcentral);
                    if (!dr.IsDBNull(iCodcentral)) entity.Codcentral = Convert.ToInt32(dr.GetValue(iCodcentral));

                    int iCentral = dr.GetOrdinal(helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iTgenercodi = dr.GetOrdinal(helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

                    int iTgenernomb = dr.GetOrdinal(helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iTipogrupocodi = dr.GetOrdinal(helper.Tipogrupocodi);
                    if (!dr.IsDBNull(iTipogrupocodi)) entity.Tipogrupocodi = Convert.ToInt32(dr.GetValue(iTipogrupocodi));

                    int iGrupomiembro = dr.GetOrdinal(helper.Grupomiembro);
                    if (!dr.IsDBNull(iGrupomiembro)) entity.Grupomiembro = dr.GetString(iGrupomiembro);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<SioDatoprieDTO> GetReporteR05MDTByOsinergcodi(string osinergcodi)
        {
            List<SioDatoprieDTO> entitys = new List<SioDatoprieDTO>();
            string query = string.Format(helper.SqlGetReporteR05MDTByOsinergcodi, osinergcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SioDatoprieDTO entity = new SioDatoprieDTO();

                    int iTipogrupocodi = dr.GetOrdinal(helper.Tipogrupocodi);
                    if (!dr.IsDBNull(iTipogrupocodi)) entity.Tipogrupocodi = Convert.ToInt32(dr.GetValue(iTipogrupocodi));

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iCtgdetnomb = dr.GetOrdinal(helper.Ctgdetnomb);
                    if (!dr.IsDBNull(iCtgdetnomb)) entity.Ctgdetnomb = dr.GetString(iCtgdetnomb);

                    int iTgenercodi = dr.GetOrdinal(helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

                    int iTgenernomb = dr.GetOrdinal(helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iTipogenerrer = dr.GetOrdinal(helper.Tipogenerrer);
                    if (!dr.IsDBNull(iTipogenerrer)) entity.Tipogenerrer = dr.GetString(iTipogenerrer);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<SioDatoprieDTO> GetReporteR05IEyR05MDE(string osinergcodi, string medifecha)
        {
            List<SioDatoprieDTO> entitys = new List<SioDatoprieDTO>();
            string query = string.Format(helper.SqlGetReporteR05IEyR05MDE, osinergcodi, medifecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SioDatoprieDTO entity = new SioDatoprieDTO();

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region SIOSEIN-PRIE-2021
        public SioDatoprieDTO ObtenerMeMedicion48(string osinergCodi)
        {
            SioDatoprieDTO entity = null;
            string query = string.Format(helper.SqlObtenerMeMedicion48, osinergCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new SioDatoprieDTO();

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iBarrTension = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iBarrTension)) entity.Gruponomb = dr.GetString(iBarrTension);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                }
            }
            return entity;
        }

        public SioDatoprieDTO ObtenerMeMedicion24(string osinergCodi)
        {
            //por completar el helper
            SioDatoprieDTO entity = null;
            string query = string.Format(helper.SqlObtenerMeMedicion24, osinergCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new SioDatoprieDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iTptomedicodi = dr.GetOrdinal(helper.Tptomedicodi);
                    if (!dr.IsDBNull(iTptomedicodi)) entity.Tptomedicodi = Convert.ToInt32(dr.GetValue(iTptomedicodi));

                    int iTptomedinomb = dr.GetOrdinal(helper.Tptomedinomb);
                    if (!dr.IsDBNull(iTptomedinomb)) entity.Tptomedinomb = dr.GetString(iTptomedinomb);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iOsicodi = dr.GetOrdinal(helper.Osicodi);
                    if (!dr.IsDBNull(iOsicodi)) entity.Osicodi = dr.GetString(iOsicodi);
                }
            }
            return entity;
        }

        public SioDatoprieDTO ObtenerMeMedicionxIntervalo(string osinergCodi)
        {
            //por completar
            SioDatoprieDTO entity = null;
            string query = string.Format(helper.SqlObtenerMeMedicionxIntervalo, osinergCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new SioDatoprieDTO();

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int IEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(IEquiabrev)) entity.Equiabrev = dr.GetString(IEquiabrev);

                    int iMedinth1 = dr.GetOrdinal(helper.Medinth1);
                    if (!dr.IsDBNull(iMedinth1)) entity.Medinth1 = Convert.ToDecimal(dr.GetValue(iMedinth1));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                }
            }
            return entity;
        }
        #endregion

    }
}
