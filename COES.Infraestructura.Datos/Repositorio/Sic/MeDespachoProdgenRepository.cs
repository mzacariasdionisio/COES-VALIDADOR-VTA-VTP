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
    /// Clase de acceso a datos de la tabla ME_DESPACHO_PRODGEN
    /// </summary>
    public class MeDespachoProdgenRepository : RepositoryBase, IMeDespachoProdgenRepository
    {
        public MeDespachoProdgenRepository(string strConn) : base(strConn)
        {
        }

        MeDespachoProdgenHelper helper = new MeDespachoProdgenHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(MeDespachoProdgenDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dpgencodi, DbType.Int32, entity.Dpgencodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dpgenfecha, DbType.DateTime, entity.Dpgenfecha));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dpgentipo, DbType.Int32, entity.Dpgentipo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dpgenvalor, DbType.Decimal, entity.Dpgenvalor));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dpgenintegrante, DbType.String, entity.Dpgenintegrante));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dpgenrer, DbType.String, entity.Dpgenrer));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Tgenercodi, DbType.Int32, entity.Tgenercodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ctgdetcodi, DbType.Int32, entity.Ctgdetcodi));
            command.ExecuteNonQuery();

            return entity.Dpgencodi;
        }

        public void Update(MeDespachoProdgenDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Dpgencodi, DbType.Int32, entity.Dpgencodi);
            dbProvider.AddInParameter(command, helper.Dpgenfecha, DbType.DateTime, entity.Dpgenfecha);
            dbProvider.AddInParameter(command, helper.Dpgenvalor, DbType.Decimal, entity.Dpgenvalor);
            dbProvider.AddInParameter(command, helper.Dpgenintegrante, DbType.String, entity.Dpgenintegrante);
            dbProvider.AddInParameter(command, helper.Dpgenrer, DbType.String, entity.Dpgenrer);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Tgenercodi, DbType.Int32, entity.Tgenercodi);
            dbProvider.AddInParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi);
            dbProvider.AddInParameter(command, helper.Ctgdetcodi, DbType.Int32, entity.Ctgdetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tipoDato, DateTime fechaIni, DateTime fechaFin, IDbConnection conn, DbTransaction tran)
        {
            string sql = string.Format(helper.SqlDelete, tipoDato, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = sql;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.ExecuteNonQuery();
        }

        public MeDespachoProdgenDTO GetById(int dpgencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Dpgencodi, DbType.Int32, dpgencodi);
            MeDespachoProdgenDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeDespachoProdgenDTO> ListResumen(int tipoDato, DateTime fechaIni, DateTime fechaFin, string flagRER)
        {
            List<MeDespachoProdgenDTO> entitys = new List<MeDespachoProdgenDTO>();

            string sql = string.Format(helper.SqlList, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), flagRER, tipoDato);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iTgenernomb = dr.GetOrdinal(this.helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iFenergnomb = dr.GetOrdinal(this.helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeDespachoProdgenDTO> GetByCriteria(int tipoDato, DateTime fechaIni, DateTime fechaFin, string flagIntegrante, string flagRER)
        {
            List<MeDespachoProdgenDTO> entitys = new List<MeDespachoProdgenDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), flagIntegrante, flagRER, tipoDato);
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
    }
}
