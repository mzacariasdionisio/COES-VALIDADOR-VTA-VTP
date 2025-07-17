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
    public class VtpPeajeEgresoDetalleRepository : RepositoryBase, IVtpPeajeEgresoDetalleRepository
    {
        public VtpPeajeEgresoDetalleRepository(string strConn) : base(strConn)
        {
        }

        VtpPeajeEgresoDetalleHelper helper = new VtpPeajeEgresoDetalleHelper();

        public int Save(VtpPeajeEgresoDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pegrdcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pegrcodi, DbType.Int32, entity.Pegrcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Pegrdtipousuario, DbType.String, entity.Pegrdtipousuario);
            dbProvider.AddInParameter(command, helper.Pegrdlicitacion, DbType.String, entity.Pegrdlicitacion);
            dbProvider.AddInParameter(command, helper.Pegrdpotecalculada, DbType.Decimal, entity.Pegrdpotecalculada);
            dbProvider.AddInParameter(command, helper.Pegrdpotedeclarada, DbType.Decimal, entity.Pegrdpotedeclarada);
            dbProvider.AddInParameter(command, helper.Pegrdcalidad, DbType.String, entity.Pegrdcalidad);
            dbProvider.AddInParameter(command, helper.Pegrdpreciopote, DbType.Decimal, entity.Pegrdpreciopote);
            dbProvider.AddInParameter(command, helper.Pegrdpoteegreso, DbType.Decimal, entity.Pegrdpoteegreso);
            dbProvider.AddInParameter(command, helper.Pegrdpeajeunitario, DbType.Decimal, entity.Pegrdpeajeunitario);
            dbProvider.AddInParameter(command, helper.Barrcodifco, DbType.Int32, entity.Barrcodifco);
            dbProvider.AddInParameter(command, helper.Pegrdpoteactiva, DbType.Decimal, entity.Pegrdpoteactiva);
            dbProvider.AddInParameter(command, helper.Pegrdpotereactiva, DbType.Decimal, entity.Pegrdpotereactiva);
            dbProvider.AddInParameter(command, helper.Pegrdusucreacion, DbType.String, entity.Pegrdusucreacion);
            dbProvider.AddInParameter(command, helper.Pegrdfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.PegrdPoteCoincidente, DbType.Decimal, entity.Pegrdpotecoincidente);
            dbProvider.AddInParameter(command, helper.PegrdFacPerdida, DbType.Decimal, entity.Pegrdfacperdida);
            dbProvider.AddInParameter(command, helper.CoregeCodVtp, DbType.String, entity.Coregecodvtp);
            dbProvider.AddInParameter(command, helper.TipConCondi, DbType.Int32, entity.TipConCondi);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtpPeajeEgresoDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pegrcodi, DbType.Int32, entity.Pegrcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Pegrdtipousuario, DbType.String, entity.Pegrdtipousuario);
            dbProvider.AddInParameter(command, helper.Pegrdlicitacion, DbType.String, entity.Pegrdlicitacion);
            dbProvider.AddInParameter(command, helper.Pegrdpotecalculada, DbType.Decimal, entity.Pegrdpotecalculada);
            dbProvider.AddInParameter(command, helper.Pegrdpotedeclarada, DbType.Decimal, entity.Pegrdpotedeclarada);
            dbProvider.AddInParameter(command, helper.Pegrdcalidad, DbType.String, entity.Pegrdcalidad);
            dbProvider.AddInParameter(command, helper.Pegrdpreciopote, DbType.Decimal, entity.Pegrdpreciopote);
            dbProvider.AddInParameter(command, helper.Pegrdpoteegreso, DbType.Decimal, entity.Pegrdpoteegreso);
            dbProvider.AddInParameter(command, helper.Pegrdpeajeunitario, DbType.Decimal, entity.Pegrdpeajeunitario);
            dbProvider.AddInParameter(command, helper.Barrcodifco, DbType.Int32, entity.Barrcodifco);
            dbProvider.AddInParameter(command, helper.Pegrdpoteactiva, DbType.Decimal, entity.Pegrdpoteactiva);
            dbProvider.AddInParameter(command, helper.Pegrdpotereactiva, DbType.Decimal, entity.Pegrdpotereactiva);
            dbProvider.AddInParameter(command, helper.Pegrdusucreacion, DbType.String, entity.Pegrdusucreacion);
            dbProvider.AddInParameter(command, helper.Pegrdfeccreacion, DbType.DateTime, entity.Pegrdfeccreacion);

            dbProvider.AddInParameter(command, helper.Pegrdcodi, DbType.Int32, entity.Pegrdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pegrdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Pegrdcodi, DbType.Int32, pegrdcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteria(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteria);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.RecPotCodi, DbType.Int32, recpotcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public VtpPeajeEgresoDetalleDTO GetById(int pegrdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pegrcodi, DbType.Int32, pegrdcodi);
            VtpPeajeEgresoDetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtpPeajeEgresoDetalleDTO> List()
        {
            List<VtpPeajeEgresoDetalleDTO> entitys = new List<VtpPeajeEgresoDetalleDTO>();
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

        public List<VtpPeajeEgresoDetalleDTO> GetByCriteria(int pegrcodi)
        {
            List<VtpPeajeEgresoDetalleDTO> entitys = new List<VtpPeajeEgresoDetalleDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.Pegrcodi, DbType.Int32, pegrcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEgresoDetalleDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    int iBarrnombre = dr.GetOrdinal(helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = Convert.ToString(dr.GetValue(iBarrnombre));

                    int iBarrnombrefco = dr.GetOrdinal(helper.Barrnombrefco);
                    if (!dr.IsDBNull(iBarrnombrefco)) entity.Barrnombrefco = Convert.ToString(dr.GetValue(iBarrnombrefco));

                    int iCoregecodvtp = dr.GetOrdinal(helper.CoregeCodVtp);
                    if (!dr.IsDBNull(iCoregecodvtp)) entity.Coregecodvtp = Convert.ToString(dr.GetValue(iCoregecodvtp));

                    int iCoregecodi = dr.GetOrdinal(helper.CoregeCodi);
                    if (!dr.IsDBNull(iCoregecodi)) entity.Coregecodi = Convert.ToInt32(dr.GetValue(iCoregecodi));

                    int iPegrdpotecoincidente = dr.GetOrdinal(helper.PegrdPoteCoincidente);
                    if (!dr.IsDBNull(iPegrdpotecoincidente)) entity.Pegrdpotecoincidente = Convert.ToDecimal(dr.GetValue(iPegrdpotecoincidente));

                    int iPegrdfacperdida = dr.GetOrdinal(helper.PegrdFacPerdida);
                    if (!dr.IsDBNull(iPegrdfacperdida)) entity.Pegrdfacperdida = Convert.ToDecimal(dr.GetValue(iPegrdfacperdida));

                    int iCodcncodivtp = dr.GetOrdinal(helper.CodCnCodiVtp);
                    if (!dr.IsDBNull(iCodcncodivtp)) entity.Codcncodivtp = Convert.ToString(dr.GetValue(iCodcncodivtp));

                    int iTipConNombre = dr.GetOrdinal(helper.TipConNombre);
                    if (!dr.IsDBNull(iTipConNombre)) entity.TipConNombre = Convert.ToString(dr.GetValue(iTipConNombre));

                    //entity.Coregecodvtp = entity.Codcncodivtp;

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<VtpPeajeEgresoDetalleDTO> GetByCriteriaPeriodoAnterior(int emprcodi , int periCodi)
        {
            List<VtpPeajeEgresoDetalleDTO> entitys = new List<VtpPeajeEgresoDetalleDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaPeriodoAnterior);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEgresoDetalleDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    int iBarrnombre = dr.GetOrdinal(helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = Convert.ToString(dr.GetValue(iBarrnombre));

                    int iBarrnombrefco = dr.GetOrdinal(helper.Barrnombrefco);
                    if (!dr.IsDBNull(iBarrnombrefco)) entity.Barrnombrefco = Convert.ToString(dr.GetValue(iBarrnombrefco));

                    int iCoregecodvtp = dr.GetOrdinal(helper.CoregeCodVtp);
                    if (!dr.IsDBNull(iCoregecodvtp)) entity.Coregecodvtp = Convert.ToString(dr.GetValue(iCoregecodvtp));

                    int iCoregecodi = dr.GetOrdinal(helper.CoregeCodi);
                    if (!dr.IsDBNull(iCoregecodi)) entity.Coregecodi = Convert.ToInt32(dr.GetValue(iCoregecodi));

                    int iPegrdpotecoincidente = dr.GetOrdinal(helper.PegrdPoteCoincidente);
                    if (!dr.IsDBNull(iPegrdpotecoincidente)) entity.Pegrdpotecoincidente = Convert.ToDecimal(dr.GetValue(iPegrdpotecoincidente));

                    int iPegrdfacperdida = dr.GetOrdinal(helper.PegrdFacPerdida);
                    if (!dr.IsDBNull(iPegrdfacperdida)) entity.Pegrdfacperdida = Convert.ToDecimal(dr.GetValue(iPegrdfacperdida));

                    int iCodcncodivtp = dr.GetOrdinal(helper.CodCnCodiVtp);
                    if (!dr.IsDBNull(iCodcncodivtp)) entity.Codcncodivtp = Convert.ToString(dr.GetValue(iCodcncodivtp));

                    int iTipConNombre = dr.GetOrdinal(helper.TipConNombre);
                    if (!dr.IsDBNull(iTipConNombre)) entity.TipConNombre = Convert.ToString(dr.GetValue(iTipConNombre));

                    //entity.Coregecodvtp = entity.Codcncodivtp;

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<VtpPeajeEgresoDetalleDTO> ListarCodigosByEmprcodi(int emprcodi, int periCodi)
        {
            List<VtpPeajeEgresoDetalleDTO> entitys = new List<VtpPeajeEgresoDetalleDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarCodigosByEmprcodi);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, periCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEgresoDetalleDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    int iBarrnombre = dr.GetOrdinal(helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = Convert.ToString(dr.GetValue(iBarrnombre));

                    int iBarrnombrefco = dr.GetOrdinal(helper.Barrnombrefco);
                    if (!dr.IsDBNull(iBarrnombrefco)) entity.Barrnombrefco = Convert.ToString(dr.GetValue(iBarrnombrefco));

                    int iCoregecodvtp = dr.GetOrdinal(helper.CoregeCodVtp);
                    if (!dr.IsDBNull(iCoregecodvtp)) entity.Coregecodvtp = Convert.ToString(dr.GetValue(iCoregecodvtp));

                    int iCoregecodi = dr.GetOrdinal(helper.CoregeCodi);
                    if (!dr.IsDBNull(iCoregecodi)) entity.Coregecodi = Convert.ToInt32(dr.GetValue(iCoregecodi));

                    int iPegrdpotecoincidente = dr.GetOrdinal(helper.PegrdPoteCoincidente);
                    if (!dr.IsDBNull(iPegrdpotecoincidente)) entity.Pegrdpotecoincidente = Convert.ToDecimal(dr.GetValue(iPegrdpotecoincidente));

                    int iPegrdfacperdida = dr.GetOrdinal(helper.PegrdFacPerdida);
                    if (!dr.IsDBNull(iPegrdfacperdida)) entity.Pegrdfacperdida = Convert.ToDecimal(dr.GetValue(iPegrdfacperdida));

                    int iCodcncodivtp = dr.GetOrdinal(helper.CodCnCodiVtp);
                    if (!dr.IsDBNull(iCodcncodivtp)) entity.Codcncodivtp = Convert.ToString(dr.GetValue(iCodcncodivtp));

                    int iTipConNombre = dr.GetOrdinal(helper.TipConNombre);
                    if (!dr.IsDBNull(iTipConNombre)) entity.TipConNombre = Convert.ToString(dr.GetValue(iTipConNombre));

                    int iTipConCodi = dr.GetOrdinal("tipconcodi");
                    if (!dr.IsDBNull(iTipConCodi)) entity.TipConCondi = Convert.ToInt32(dr.GetValue(iTipConCodi));
                    //entity.Coregecodvtp = entity.Codcncodivtp;

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<VtpPeajeEgresoDetalleDTO> ListView(int pegrcodi)
        {
            List<VtpPeajeEgresoDetalleDTO> entitys = new List<VtpPeajeEgresoDetalleDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListView);
            dbProvider.AddInParameter(command, helper.Pegrcodi, DbType.Int32, pegrcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEgresoDetalleDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    int iBarrnombre = dr.GetOrdinal(helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = Convert.ToString(dr.GetValue(iBarrnombre));

                    int iBarrnombrefco = dr.GetOrdinal(helper.Barrnombrefco);
                    if (!dr.IsDBNull(iBarrnombrefco)) entity.Barrnombrefco = Convert.ToString(dr.GetValue(iBarrnombrefco));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public VtpPeajeEgresoDetalleDTO GetByIdMinfo(int pegrcodi, int emprcodi, int barrcodi, string pegrdtipousuario)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdMinfo);

            dbProvider.AddInParameter(command, helper.Pegrcodi, DbType.Int32, pegrcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, barrcodi);
            dbProvider.AddInParameter(command, helper.Pegrdtipousuario, DbType.String, pegrdtipousuario);

            VtpPeajeEgresoDetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public VtpPeajeEgresoDetalleDTO GetByPegrCodiAndCodVtp(int pegrcodi, string coregecodvtp)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByPegrCodiAndCodVtp);

            dbProvider.AddInParameter(command, helper.Pegrcodi, DbType.Int32, pegrcodi);
            dbProvider.AddInParameter(command, helper.CoregeCodVtp, DbType.String, coregecodvtp);

            VtpPeajeEgresoDetalleDTO entity = null;

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
