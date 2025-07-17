using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla VTP_RECALCULO_POTENCIA
    /// </summary>
    public class VtpRecalculoPotenciaRepository : RepositoryBase, IVtpRecalculoPotenciaRepository
    {
        public VtpRecalculoPotenciaRepository(string strConn)
            : base(strConn)
        {
        }

        VtpRecalculoPotenciaHelper helper = new VtpRecalculoPotenciaHelper();

        public int Save(VtpRecalculoPotenciaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            int iRecpotcodi = GetCodigoGenerado(entity.Pericodi);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, iRecpotcodi); //Agregar el codigo que sigue
            dbProvider.AddInParameter(command, helper.Recpotnombre, DbType.String, entity.Recpotnombre);
            dbProvider.AddInParameter(command, helper.Recpotinfovtp, DbType.String, entity.Recpotinfovtp);
            dbProvider.AddInParameter(command, helper.Recpotfactincecontrantacion, DbType.Decimal, entity.Recpotfactincecontrantacion);
            dbProvider.AddInParameter(command, helper.Recpotfactincedespacho, DbType.Decimal, entity.Recpotfactincedespacho);
            dbProvider.AddInParameter(command, helper.Recpotmaxidemamensual, DbType.Decimal, entity.Recpotmaxidemamensual);
            dbProvider.AddInParameter(command, helper.Recpotinterpuntames, DbType.DateTime, entity.Recpotinterpuntames);
            dbProvider.AddInParameter(command, helper.Recpotpreciopoteppm, DbType.Decimal, entity.Recpotpreciopoteppm);
            dbProvider.AddInParameter(command, helper.Recpotpreciopeajeppm, DbType.Decimal, entity.Recpotpreciopeajeppm);
            dbProvider.AddInParameter(command, helper.Recpotpreciocostracionamiento, DbType.Decimal, entity.Recpotpreciocostracionamiento);
            dbProvider.AddInParameter(command, helper.Recpotpreciodemaservauxiliares, DbType.Decimal, entity.Recpotpreciodemaservauxiliares);
            dbProvider.AddInParameter(command, helper.Recpotconsumidademanda, DbType.Decimal, entity.Recpotconsumidademanda);
            dbProvider.AddInParameter(command, helper.Recpotfechalimite, DbType.DateTime, entity.Recpotfechalimite);
            dbProvider.AddInParameter(command, helper.Recpothoralimite, DbType.String, entity.Recpothoralimite);
            dbProvider.AddInParameter(command, helper.Recpotcuadro1, DbType.String, entity.Recpotcuadro1);
            dbProvider.AddInParameter(command, helper.Recpotnota1, DbType.String, entity.Recpotnota1);
            dbProvider.AddInParameter(command, helper.Recpotcomegeneral, DbType.String, entity.Recpotcomegeneral);
            dbProvider.AddInParameter(command, helper.Recacodi, DbType.Int32, entity.Recacodi);
            dbProvider.AddInParameter(command, helper.Pericodidestino, DbType.Int32, entity.Pericodidestino);
            dbProvider.AddInParameter(command, helper.Recpotestado, DbType.String, entity.Recpotestado);
            dbProvider.AddInParameter(command, helper.Recpotusucreacion, DbType.String, entity.Recpotusucreacion);
            dbProvider.AddInParameter(command, helper.Recpotfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Recpotusumodificacion, DbType.String, entity.Recpotusumodificacion);
            dbProvider.AddInParameter(command, helper.Recpotfecmodificacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Recpotcargapfr, DbType.Int32, entity.Recpotcargapfr);

            dbProvider.ExecuteNonQuery(command);

            return iRecpotcodi;
        }

        public void Update(VtpRecalculoPotenciaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Recpotnombre, DbType.String, entity.Recpotnombre);
            dbProvider.AddInParameter(command, helper.Recpotinfovtp, DbType.String, entity.Recpotinfovtp);
            dbProvider.AddInParameter(command, helper.Recpotfactincecontrantacion, DbType.Decimal, entity.Recpotfactincecontrantacion);
            dbProvider.AddInParameter(command, helper.Recpotfactincedespacho, DbType.Decimal, entity.Recpotfactincedespacho);
            dbProvider.AddInParameter(command, helper.Recpotmaxidemamensual, DbType.Decimal, entity.Recpotmaxidemamensual);
            dbProvider.AddInParameter(command, helper.Recpotinterpuntames, DbType.DateTime, entity.Recpotinterpuntames);
            dbProvider.AddInParameter(command, helper.Recpotpreciopoteppm, DbType.Decimal, entity.Recpotpreciopoteppm);
            dbProvider.AddInParameter(command, helper.Recpotpreciopeajeppm, DbType.Decimal, entity.Recpotpreciopeajeppm);
            dbProvider.AddInParameter(command, helper.Recpotpreciocostracionamiento, DbType.Decimal, entity.Recpotpreciocostracionamiento);
            dbProvider.AddInParameter(command, helper.Recpotpreciodemaservauxiliares, DbType.Decimal, entity.Recpotpreciodemaservauxiliares);
            dbProvider.AddInParameter(command, helper.Recpotconsumidademanda, DbType.Decimal, entity.Recpotconsumidademanda);
            dbProvider.AddInParameter(command, helper.Recpotfechalimite, DbType.DateTime, entity.Recpotfechalimite);
            dbProvider.AddInParameter(command, helper.Recpothoralimite, DbType.String, entity.Recpothoralimite);
            dbProvider.AddInParameter(command, helper.Recpotcuadro1, DbType.String, entity.Recpotcuadro1);
            dbProvider.AddInParameter(command, helper.Recpotnota1, DbType.String, entity.Recpotnota1);
            dbProvider.AddInParameter(command, helper.Recpotcomegeneral, DbType.String, entity.Recpotcomegeneral);
            dbProvider.AddInParameter(command, helper.Recacodi, DbType.Int32, entity.Recacodi);
            dbProvider.AddInParameter(command, helper.Pericodidestino, DbType.Int32, entity.Pericodidestino);
            dbProvider.AddInParameter(command, helper.Recpotestado, DbType.String, entity.Recpotestado);
            dbProvider.AddInParameter(command, helper.Recpotusumodificacion, DbType.String, entity.Recpotusumodificacion);
            dbProvider.AddInParameter(command, helper.Recpotfecmodificacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Recpotcargapfr, DbType.Int32, entity.Recpotcargapfr);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtpRecalculoPotenciaDTO GetById(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            VtpRecalculoPotenciaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtpRecalculoPotenciaDTO> List()
        {
            List<VtpRecalculoPotenciaDTO> entitys = new List<VtpRecalculoPotenciaDTO>();
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

        public List<VtpRecalculoPotenciaDTO> GetByCriteria()
        {
            List<VtpRecalculoPotenciaDTO> entitys = new List<VtpRecalculoPotenciaDTO>();
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

        public int GetCodigoGenerado(int iPeriCodi)
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, iPeriCodi);
            object result = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());
            if (result != null) newId = Convert.ToInt32(result);

            return newId;
        }

        public List<VtpRecalculoPotenciaDTO> ListView()
        {
            List<VtpRecalculoPotenciaDTO> entitys = new List<VtpRecalculoPotenciaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListView);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpRecalculoPotenciaDTO entity = helper.Create(dr);

                    int iPerinombre = dr.GetOrdinal(this.helper.Perinombre);
                    if (!dr.IsDBNull(iPerinombre)) entity.Perinombre = dr.GetString(iPerinombre);

                    int iRecanombre = dr.GetOrdinal(this.helper.Recanombre);
                    if (!dr.IsDBNull(iRecanombre)) entity.Recanombre = dr.GetString(iRecanombre);

                    int iPerinombredestino = dr.GetOrdinal(this.helper.Perinombredestino);
                    if (!dr.IsDBNull(iPerinombredestino)) entity.Perinombredestino = dr.GetString(iPerinombredestino);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region PrimasRER.2023
        public List<VtpRecalculoPotenciaDTO> ListVTP(int anio, int mes)
        {
            List<VtpRecalculoPotenciaDTO> entitys = new List<VtpRecalculoPotenciaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListVTP);
            dbProvider.AddInParameter(command, helper.Perianio, DbType.Int32, anio);
            dbProvider.AddInParameter(command, helper.Perimes, DbType.Int32, mes);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpRecalculoPotenciaDTO entity = helper.Create(dr);

                    int iPerinombre = dr.GetOrdinal(this.helper.Perinombre);
                    if (!dr.IsDBNull(iPerinombre)) entity.Perinombre = dr.GetString(iPerinombre);

                    //int iRecanombre = dr.GetOrdinal(this.helper.Recanombre);
                    //if (!dr.IsDBNull(iRecanombre)) entity.Recanombre = dr.GetString(iRecanombre);

                    //int iPerinombredestino = dr.GetOrdinal(this.helper.Perinombredestino);
                    //if (!dr.IsDBNull(iPerinombredestino)) entity.Perinombredestino = dr.GetString(iPerinombredestino);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public VtpRecalculoPotenciaDTO GetByIdCerrado(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdCerrado);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            VtpRecalculoPotenciaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
        #endregion
        public VtpRecalculoPotenciaDTO GetByIdView(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdView);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            VtpRecalculoPotenciaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iPerinombre = dr.GetOrdinal(this.helper.Perinombre);
                    if (!dr.IsDBNull(iPerinombre)) entity.Perinombre = dr.GetString(iPerinombre);

                    int iRecanombre = dr.GetOrdinal(this.helper.Recanombre);
                    if (!dr.IsDBNull(iRecanombre)) entity.Recanombre = dr.GetString(iRecanombre);

                    int iPerinombredestino = dr.GetOrdinal(this.helper.Perinombredestino);
                    if (!dr.IsDBNull(iPerinombredestino)) entity.Perinombredestino = dr.GetString(iPerinombredestino);

                }
            }

            return entity;
        }

        public List<VtpRecalculoPotenciaDTO> ListByPericodi(int iPeriCodi)
        {
            List<VtpRecalculoPotenciaDTO> entitys = new List<VtpRecalculoPotenciaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByPericodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, iPeriCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public int GetByMaxIdRecPotCodi(int iPeriCodi)
        {
            int iRecpotcodi = GetCodigoGenerado(iPeriCodi);
            iRecpotcodi--;
            return iRecpotcodi;
        }

        //ASSETEC 202108 - TIEE
        public List<VtpRecalculoPotenciaDTO> ListMaxRecalculoByPeriodo()
        {
            List<VtpRecalculoPotenciaDTO> entitys = new List<VtpRecalculoPotenciaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListMaxRecalculoByPeriodo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpRecalculoPotenciaDTO entity = new VtpRecalculoPotenciaDTO();

                    int iPericodi = dr.GetOrdinal(this.helper.Pericodi);
                    if (!dr.IsDBNull(iPericodi)) entity.Pericodi = dr.GetInt32(iPericodi);

                    int iRecpotcodi = dr.GetOrdinal(this.helper.Recpotcodi);
                    if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = dr.GetInt32(iRecpotcodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public string MigrarSaldosVTP(int emprcodiorigen, int emprcodidestino, int pericodi, int recpotcodi)
        {
            string sql = string.Format(helper.SqlMigrarSaldosVTP, emprcodiorigen, emprcodidestino, pericodi, recpotcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.ExecuteNonQuery(command);
            return sql;
        }

        public string MigrarCalculoVTP(int emprcodiorigen, int emprcodidestino, int pericodi, int recpotcodi)
        {
            string sql = string.Format(helper.SqlMigrarCalculoVTP, emprcodiorigen, emprcodidestino, pericodi, recpotcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.ExecuteNonQuery(command);
            return sql;
        }

        #region CPA.CU05
        public List<VtpRecalculoPotenciaDTO> ListRecalculoByPeriodo(int anio)
        {
            VtpRecalculoPotenciaDTO entity = new VtpRecalculoPotenciaDTO();
            List<VtpRecalculoPotenciaDTO> entitys = new List<VtpRecalculoPotenciaDTO>();
            string query = string.Format(helper.SqlListRecalculoByPeriodo, anio);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new VtpRecalculoPotenciaDTO();

                    int iPerinombre = dr.GetOrdinal(helper.Perinombre);
                    if (!dr.IsDBNull(iPerinombre)) entity.Perinombre = dr.GetString(iPerinombre);

                    int iPerianio = dr.GetOrdinal(helper.Perianio);
                    if (!dr.IsDBNull(iPerianio)) entity.Perianio = dr.GetInt32(iPerianio);

                    int iPerimes = dr.GetOrdinal(helper.Perimes);
                    if (!dr.IsDBNull(iPerimes)) entity.Perimes = dr.GetInt32(iPerimes);

                    int iRecpotinterpuntames = dr.GetOrdinal(helper.Recpotinterpuntames);
                    if (!dr.IsDBNull(iRecpotinterpuntames)) entity.Recpotinterpuntames = dr.GetDateTime(iRecpotinterpuntames);

                    int iRecpotpreciopoteppm = dr.GetOrdinal(helper.Recpotpreciopoteppm);
                    if (!dr.IsDBNull(iRecpotpreciopoteppm)) entity.Recpotpreciopoteppm = dr.GetDecimal(iRecpotpreciopoteppm);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion
    }
}
