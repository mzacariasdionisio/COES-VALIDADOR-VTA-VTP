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
    /// Clase de acceso a datos de la tabla VTP_RETIRO_POTESC
    /// </summary>
    public class VtpRetiroPotescRepository: RepositoryBase, IVtpRetiroPotescRepository
    {
        public VtpRetiroPotescRepository(string strConn): base(strConn)
        {
        }

        VtpRetiroPotescHelper helper = new VtpRetiroPotescHelper();

        public int Save(VtpRetiroPotescDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rpsccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Rpsctipousuario, DbType.String, entity.Rpsctipousuario);
            dbProvider.AddInParameter(command, helper.Rpsccalidad, DbType.String, entity.Rpsccalidad);
            dbProvider.AddInParameter(command, helper.Rpscprecioppb, DbType.Decimal, entity.Rpscprecioppb);
            dbProvider.AddInParameter(command, helper.Rpscpreciopote, DbType.Decimal, entity.Rpscpreciopote);
            dbProvider.AddInParameter(command, helper.Rpscpoteegreso, DbType.Decimal, entity.Rpscpoteegreso);
            dbProvider.AddInParameter(command, helper.Rpscpeajeunitario, DbType.Decimal, entity.Rpscpeajeunitario);
            dbProvider.AddInParameter(command, helper.Barrcodifco, DbType.Int32, entity.Barrcodifco);
            dbProvider.AddInParameter(command, helper.Rpscpoteactiva, DbType.Decimal, entity.Rpscpoteactiva);
            dbProvider.AddInParameter(command, helper.Rpscpotereactiva, DbType.Decimal, entity.Rpscpotereactiva);
            dbProvider.AddInParameter(command, helper.Rpscusucreacion, DbType.String, entity.Rpscusucreacion);
            dbProvider.AddInParameter(command, helper.Rpscfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Rpscusumodificacion, DbType.String, entity.Rpscusumodificacion);
            dbProvider.AddInParameter(command, helper.Rpscfecmodificacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtpRetiroPotescDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Rpsctipousuario, DbType.String, entity.Rpsctipousuario);
            dbProvider.AddInParameter(command, helper.Rpsccalidad, DbType.String, entity.Rpsccalidad);
            dbProvider.AddInParameter(command, helper.Rpscprecioppb, DbType.Decimal, entity.Rpscprecioppb);
            dbProvider.AddInParameter(command, helper.Rpscpreciopote, DbType.Decimal, entity.Rpscpreciopote);
            dbProvider.AddInParameter(command, helper.Rpscpoteegreso, DbType.Decimal, entity.Rpscpoteegreso);
            dbProvider.AddInParameter(command, helper.Rpscpeajeunitario, DbType.Decimal, entity.Rpscpeajeunitario);
            dbProvider.AddInParameter(command, helper.Barrcodifco, DbType.Int32, entity.Barrcodifco);
            dbProvider.AddInParameter(command, helper.Rpscpoteactiva, DbType.Decimal, entity.Rpscpoteactiva);
            dbProvider.AddInParameter(command, helper.Rpscpotereactiva, DbType.Decimal, entity.Rpscpotereactiva);
            dbProvider.AddInParameter(command, helper.Rpscusumodificacion, DbType.String, entity.Rpscusumodificacion);
            dbProvider.AddInParameter(command, helper.Rpscfecmodificacion, DbType.DateTime, DateTime.Now);
            //Where:
            dbProvider.AddInParameter(command, helper.Rpsccodi, DbType.Int32, entity.Rpsccodi);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Pericodi, int Recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            //dbProvider.AddInParameter(command, helper.Rpsccodi, DbType.Int32, rpsccodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, Recpotcodi);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public VtpRetiroPotescDTO GetById(int rpsccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rpsccodi, DbType.Int32, rpsccodi);
            VtpRetiroPotescDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtpRetiroPotescDTO> List()
        {
            List<VtpRetiroPotescDTO> entitys = new List<VtpRetiroPotescDTO>();
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

        public List<VtpRetiroPotescDTO> GetByCriteria()
        {
            List<VtpRetiroPotescDTO> entitys = new List<VtpRetiroPotescDTO>();
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

        public List<VtpRetiroPotescDTO> ListView(int pericodi, int recpotcodi)
        {
            List<VtpRetiroPotescDTO> entitys = new List<VtpRetiroPotescDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListView);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpRetiroPotescDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iBarrnombre = dr.GetOrdinal(this.helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = dr.GetString(iBarrnombre);

                    int iBarrnombrefco = dr.GetOrdinal(this.helper.Barrnombrefco);
                    if (!dr.IsDBNull(iBarrnombrefco)) entity.Barrnombrefco = dr.GetString(iBarrnombrefco);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpRetiroPotescDTO> ListByEmpresa(int pericodi, int recpotcodi)
        {
            List<VtpRetiroPotescDTO> entitys = new List<VtpRetiroPotescDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByEmpresa);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpRetiroPotescDTO entity = new VtpRetiroPotescDTO();

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iRpscpoteegreso = dr.GetOrdinal(this.helper.Rpscpoteegreso);
                    if (!dr.IsDBNull(iRpscpoteegreso)) entity.Rpscpoteegreso = dr.GetDecimal(iRpscpoteegreso);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
