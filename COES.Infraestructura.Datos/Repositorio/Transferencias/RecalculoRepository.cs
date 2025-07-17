using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
   public class RecalculoRepository: RepositoryBase, IRecalculoRepository
    {
        private string strConexion;
        public RecalculoRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }

        public IDbConnection BeginConnection()
        {
            Database db = DatabaseFactory.CreateDatabase(strConexion);
            IDbConnection conn = db.CreateConnection();
            conn.Open();
            return conn;
        }

        public DbTransaction StartTransaction(IDbConnection conn)
        {
            return (DbTransaction)conn.BeginTransaction();
        }

        RecalculoHelper helper = new RecalculoHelper();

        public int Save(RecalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            int iRecacodi = GetCodigoGenerado(entity.RecaPeriCodi);

            dbProvider.AddInParameter(command, helper.Recapericodi, DbType.Int32, entity.RecaPeriCodi);
            dbProvider.AddInParameter(command, helper.Recacodi, DbType.Int32, iRecacodi);
            dbProvider.AddInParameter(command, helper.Recafechavalorizacion, DbType.DateTime, entity.RecaFechaValorizacion);
            dbProvider.AddInParameter(command, helper.Recafechalimite, DbType.DateTime, entity.RecaFechaLimite);
            dbProvider.AddInParameter(command, helper.Recahoralimite, DbType.String, entity.RecaHoraLimite);
            dbProvider.AddInParameter(command, helper.Recafechaobservacion, DbType.DateTime, entity.RecaFechaObservacion);
            dbProvider.AddInParameter(command, helper.Recaestado, DbType.String, entity.RecaEstado);
            dbProvider.AddInParameter(command, helper.Recanombre, DbType.String, entity.RecaNombre);
            dbProvider.AddInParameter(command, helper.Recadescripcion, DbType.String, entity.RecaDescripcion);
            dbProvider.AddInParameter(command, helper.Recanroinforme, DbType.String, entity.RecaNroInforme);
            dbProvider.AddInParameter(command, helper.Recamasinfo, DbType.String, entity.RecaMasInfo);
            dbProvider.AddInParameter(command, helper.Recausername, DbType.String, entity.RecaUserName);
            dbProvider.AddInParameter(command, helper.Recafecins, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.PeriCodiDestino, DbType.Int32, entity.PeriCodiDestino);
            dbProvider.AddInParameter(command, helper.Recacuadro1, DbType.String, entity.RecaCuadro1);
            dbProvider.AddInParameter(command, helper.Recacuadro2, DbType.String, entity.RecaCuadro2);
            dbProvider.AddInParameter(command, helper.Recanota2, DbType.String, entity.RecaNota2);
            dbProvider.AddInParameter(command, helper.Recacuadro3, DbType.String, entity.RecaCuadro3);
            dbProvider.AddInParameter(command, helper.Recacuadro4, DbType.String, entity.RecaCuadro4);
            dbProvider.AddInParameter(command, helper.Recacuadro5, DbType.String, entity.RecaCuadro5);
            dbProvider.ExecuteNonQuery(command);

            return iRecacodi;
        }

        public void Update(RecalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Recafechavalorizacion, DbType.DateTime, entity.RecaFechaValorizacion);
            dbProvider.AddInParameter(command, helper.Recafechalimite, DbType.DateTime, entity.RecaFechaLimite);
            dbProvider.AddInParameter(command, helper.Recahoralimite, DbType.String, entity.RecaHoraLimite);
            dbProvider.AddInParameter(command, helper.Recafechaobservacion, DbType.DateTime, entity.RecaFechaObservacion);
            dbProvider.AddInParameter(command, helper.Recaestado, DbType.String, entity.RecaEstado);
            dbProvider.AddInParameter(command, helper.Recanombre, DbType.String, entity.RecaNombre);
            dbProvider.AddInParameter(command, helper.Recadescripcion, DbType.String, entity.RecaDescripcion);
            dbProvider.AddInParameter(command, helper.Recanroinforme, DbType.String, entity.RecaNroInforme);
            dbProvider.AddInParameter(command, helper.Recamasinfo, DbType.String, entity.RecaMasInfo);
            dbProvider.AddInParameter(command, helper.Recafecact, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.PeriCodiDestino, DbType.Int32, entity.PeriCodiDestino);
            dbProvider.AddInParameter(command, helper.Recacuadro1, DbType.String, entity.RecaCuadro1);
            dbProvider.AddInParameter(command, helper.Recacuadro2, DbType.String, entity.RecaCuadro2);
            dbProvider.AddInParameter(command, helper.Recanota2, DbType.String, entity.RecaNota2);
            dbProvider.AddInParameter(command, helper.Recacuadro3, DbType.String, entity.RecaCuadro3);
            dbProvider.AddInParameter(command, helper.Recacuadro4, DbType.String, entity.RecaCuadro4);
            dbProvider.AddInParameter(command, helper.Recacuadro5, DbType.String, entity.RecaCuadro5);

            dbProvider.AddInParameter(command, helper.Recacodi, DbType.Int32, entity.RecaCodi);
            dbProvider.AddInParameter(command, helper.Recapericodi, DbType.Int32, entity.RecaPeriCodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(System.Int32 iPeriCodi, System.Int32 iRecaCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Recapericodi, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.Recacodi, DbType.Int32, iRecaCodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public RecalculoDTO GetById(System.Int32 iPerCodi, System.Int32 iRecaCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Recapericodi, DbType.Int32, iPerCodi);
            dbProvider.AddInParameter(command, helper.Recacodi, DbType.Int32, iRecaCodi);
        
            RecalculoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command)) 
            {
                if (dr.Read())
                {   /*ASSETEC 202001*/
                    entity = helper.Create(dr);
                    int iPeriNombre = dr.GetOrdinal(this.helper.PeriNombre);
                    if (!dr.IsDBNull(iPeriNombre)) entity.PeriNombre = dr.GetString(iPeriNombre);
                }
            }

            return entity;
        }

        public List<RecalculoDTO> List(int id = 0)
        {
            List<RecalculoDTO> entitys = new List<RecalculoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Recapericodi, DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RecalculoDTO entity = helper.Create(dr);
                    int iPeriNombre = dr.GetOrdinal(this.helper.PeriNombre);
                    if (!dr.IsDBNull(iPeriNombre)) entity.PeriNombre = dr.GetString(iPeriNombre);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region PrimasRER.2023
        public List<RecalculoDTO> ListByAnioMes(int anio = 0, int mes = 0)
        {
            List<RecalculoDTO> entitys = new List<RecalculoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByAnioMes);
            dbProvider.AddInParameter(command, helper.Perianio, DbType.Int32, anio);
            dbProvider.AddInParameter(command, helper.Perimes, DbType.Int32, mes);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RecalculoDTO entity = helper.Create(dr);
                    int iPeriNombre = dr.GetOrdinal(this.helper.PeriNombre);
                    if (!dr.IsDBNull(iPeriNombre)) entity.PeriNombre = dr.GetString(iPeriNombre);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RecalculoDTO> ListVTEAByAnioMes(int anio = 0, int mes = 0)
        {
            List<RecalculoDTO> entitys = new List<RecalculoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListVTEAByAnioMes);
            dbProvider.AddInParameter(command, helper.Perianio, DbType.Int32, anio);
            dbProvider.AddInParameter(command, helper.Perimes, DbType.Int32, mes);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RecalculoDTO entity = helper.Create(dr);
                    //int iPeriNombreDestino = dr.GetOrdinal(this.helper.PeriNombreDestino);
                    //if (!dr.IsDBNull(iPeriNombreDestino)) entity.PeriNombreDestino = dr.GetString(iPeriNombreDestino);
                    int iPeriNombre = dr.GetOrdinal(this.helper.PeriNombre);
                    if (!dr.IsDBNull(iPeriNombre)) entity.PeriNombre = dr.GetString(iPeriNombre);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        public List<RecalculoDTO> ListEstadoPublicarCerrado(int id = 0)
        {
            List<RecalculoDTO> entitys = new List<RecalculoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEstadoPublicarCerrado);
            dbProvider.AddInParameter(command, helper.Recapericodi, DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RecalculoDTO entity = helper.Create(dr);
                    int iPeriNombre = dr.GetOrdinal(this.helper.PeriNombre);
                    if (!dr.IsDBNull(iPeriNombre)) entity.PeriNombre = dr.GetString(iPeriNombre);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RecalculoDTO> GetByCriteria(string nombre)
        {
            List<RecalculoDTO> entitys = new List<RecalculoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Recanombre, DbType.String, nombre);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public int GetCodigoGenerado(int iPeriCodi)
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            dbProvider.AddInParameter(command, helper.Recapericodi, DbType.Int32, iPeriCodi);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public int GetUltimaVersion(int pericodi)
        {
            int version = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetUltimaVersion);
            dbProvider.AddInParameter(command, helper.Recapericodi, DbType.Int32, pericodi);
            version = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return version;
        }

        public int ObtenerVersionDTR(int pericodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerVersionDTR);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }


        public List<RecalculoDTO> ListRecalculosTrnCodigoEnviado(int pericodi, int emprcodi)
        {
            List<RecalculoDTO> entitys = new List<RecalculoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListRecalculosTrnCodigoEnviado);
            dbProvider.AddInParameter(command, helper.Recapericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recacodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Recapericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recacodi, DbType.Int32, emprcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RecalculoDTO entity = new RecalculoDTO();
                    int iRecacodi = dr.GetOrdinal(this.helper.Recacodi);
                    if (!dr.IsDBNull(iRecacodi)) entity.RecaCodi = dr.GetInt32(iRecacodi);
                    int iRecanombre = dr.GetOrdinal(this.helper.Recanombre);
                    if (!dr.IsDBNull(iRecanombre)) entity.RecaNombre = dr.GetString(iRecanombre);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //ASSETEC 202108 - TIEE
        public List<RecalculoDTO> ListMaxRecalculoByPeriodo()
        {
            List<RecalculoDTO> entitys = new List<RecalculoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListMaxRecalculoByPeriodo);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RecalculoDTO entity = new RecalculoDTO();
                    int iPericodi = dr.GetOrdinal(this.helper.Pericodi);
                    if (!dr.IsDBNull(iPericodi)) entity.RecaPeriCodi = dr.GetInt32(iPericodi);
                    int iRecacodi = dr.GetOrdinal(this.helper.Recacodi);
                    if (!dr.IsDBNull(iRecacodi)) entity.RecaCodi = dr.GetInt32(iRecacodi);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public string MigrarSaldosVTEA(int emprcodiorigen, int emprcodidestino, int pericodi, int recacodi)
        {
            string sql = string.Format(helper.SqlMigrarSaldosVTEA, emprcodiorigen, emprcodidestino, pericodi, recacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.ExecuteNonQuery(command);
            return sql;
        }

        public string MigrarCalculoVTEA(int emprcodiorigen, int emprcodidestino, int pericodi, int recacodi)
        {
            string sql = string.Format(helper.SqlMigrarCalculoVTEA, emprcodiorigen, emprcodidestino, pericodi, recacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.ExecuteNonQuery(command);
            return sql;
        }
    }
}
