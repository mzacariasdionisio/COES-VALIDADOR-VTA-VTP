using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;


namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class IngresoRetiroSCRepository : RepositoryBase, IIngresoRetiroSCRepository
    {
        public IngresoRetiroSCRepository(string strConn) : base(strConn)
        {
        }

        IngresoRetiroSCHelper helper = new IngresoRetiroSCHelper();

        public int Save(IngresoRetiroSCDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ingrsccodi, DbType.Int32, GetCodigoGenerado());
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.Ingrscversion, DbType.Int32, entity.IngrscVersion);
            dbProvider.AddInParameter(command, helper.Ingrscimporte, DbType.Double, entity.IngrscImporte);
            dbProvider.AddInParameter(command, helper.Ingrscimportevtp, DbType.Double, entity.IngrscImporteVtp);
            dbProvider.AddInParameter(command, helper.Ingrscusername, DbType.String, entity.IngrscUserName);
            dbProvider.AddInParameter(command, helper.Ingrscfecins, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Ingrscfecact, DbType.DateTime, DateTime.Now);

            return dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(System.Int32 PeriCod, System.Int32 IngrscVersion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, PeriCod);
            dbProvider.AddInParameter(command, helper.Ingrscversion, DbType.Int32, IngrscVersion);

            dbProvider.ExecuteNonQuery(command);
        }

        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public List<IngresoRetiroSCDTO> GetByCriteria(int pericodi, int version)
        {

            List<IngresoRetiroSCDTO> entitys = new List<IngresoRetiroSCDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Ingrscversion, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Ingrscversion, DbType.Int32, version);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IngresoRetiroSCDTO entity = new IngresoRetiroSCDTO();

                    int iPeriCodi = dr.GetOrdinal(helper.PeriCodi);
                    if (!dr.IsDBNull(iPeriCodi)) entity.PeriCodi = dr.GetInt32(iPeriCodi);

                    int iIngrscCodi = dr.GetOrdinal(helper.Ingrsccodi);
                    if (!dr.IsDBNull(iIngrscCodi)) entity.IngrscCodi = dr.GetInt32(iIngrscCodi);

                    int iIngrscVers = dr.GetOrdinal(helper.Ingrscversion);
                    if (!dr.IsDBNull(iIngrscVers)) entity.IngrscVersion = dr.GetInt32(iIngrscVers);

                    int iEmprCodi = dr.GetOrdinal(helper.EmprCodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = dr.GetInt32(iEmprCodi);

                    int iIngrscImporte = dr.GetOrdinal(helper.Ingrscimporte);
                    if (!dr.IsDBNull(iIngrscImporte)) entity.IngrscImporte = dr.GetDecimal(iIngrscImporte);

                    int iIngrscImporteVtp = dr.GetOrdinal(helper.Ingrscimportevtp);
                    if (!dr.IsDBNull(iIngrscImporteVtp)) entity.IngrscImporteVtp = dr.GetDecimal(iIngrscImporteVtp);

                    int iIngrscUsername = dr.GetOrdinal(helper.Ingrscusername);
                    if (!dr.IsDBNull(iIngrscUsername)) entity.IngrscUserName = dr.GetString(iIngrscUsername);

                    int iIngrscFecins = dr.GetOrdinal(helper.Ingrscfecins);
                    if (!dr.IsDBNull(iIngrscFecins)) entity.IngrscFecIns = dr.GetDateTime(iIngrscFecins);

                    int iIngrscFecact = dr.GetOrdinal(helper.Ingrscfecact);
                    if (!dr.IsDBNull(iIngrscFecact)) entity.IngrscFecAct = dr.GetDateTime(iIngrscFecact);

                    int iTotal = dr.GetOrdinal(helper.Total);
                    if (!dr.IsDBNull(iTotal)) entity.Total = dr.GetDecimal(iTotal);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IngresoRetiroSCDTO> GetByCodigo(int? pericodi, int? version)
        {

            List<IngresoRetiroSCDTO> entitys = new List<IngresoRetiroSCDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCodigo);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Ingrscversion, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.Ingrscversion, DbType.Int32, version);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IngresoRetiroSCDTO entity = new IngresoRetiroSCDTO();

                    int iPerinombre = dr.GetOrdinal(helper.PeriNombre);
                    if (!dr.IsDBNull(iPerinombre)) entity.PeriNombre = dr.GetString(iPerinombre);

                    int iEmprnombre = dr.GetOrdinal(helper.EmprNombre);
                    if (!dr.IsDBNull(iEmprnombre)) entity.EmprNombre = dr.GetString(iEmprnombre);

                    int iIngrscVers = dr.GetOrdinal(helper.Ingrscversion);
                    if (!dr.IsDBNull(iIngrscVers)) entity.IngrscVersion = dr.GetInt32(iIngrscVers);

                    int iIngrscImporte = dr.GetOrdinal(helper.Ingrscimporte);
                    if (!dr.IsDBNull(iIngrscImporte)) entity.IngrscImporte = dr.GetDecimal(iIngrscImporte);

                    int iIngrscImporteVtp = dr.GetOrdinal(helper.Ingrscimportevtp);
                    if (!dr.IsDBNull(iIngrscImporteVtp)) entity.IngrscImporteVtp = dr.GetDecimal(iIngrscImporteVtp);

                    int iIngrscUsername = dr.GetOrdinal(helper.Ingrscusername);
                    if (!dr.IsDBNull(iIngrscUsername)) entity.IngrscUserName = dr.GetString(iIngrscUsername);

                    int iIngrscFecins = dr.GetOrdinal(helper.Ingrscfecins);
                    if (!dr.IsDBNull(iIngrscFecins)) entity.IngrscFecIns = dr.GetDateTime(iIngrscFecins);

                    //- Linea agregada egjunin
                    int iEmprcodi = dr.GetOrdinal(helper.EmprCodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.EmprCodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IngresoRetiroSCDTO> ListByPeriodoVersion(int iPericodi, int iVersion)
        {
            List<IngresoRetiroSCDTO> entitys = new List<IngresoRetiroSCDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByPeriodoVersion);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, iPericodi);
            dbProvider.AddInParameter(command, helper.Ingrscversion, DbType.Int32, iVersion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public IngresoRetiroSCDTO GetByPeriodoVersionEmpresa(int iPericodi, int iVersion, int iEmprCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByPeriodoVersionEmpresa);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, iPericodi);
            dbProvider.AddInParameter(command, helper.Ingrscversion, DbType.Int32, iVersion);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, iEmprCodi);
            IngresoRetiroSCDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }
            return entity;
        }
    }
}
