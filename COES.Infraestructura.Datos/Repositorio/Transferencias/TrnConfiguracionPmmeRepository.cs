using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using COES.Base.Core;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    public class TrnConfiguracionPmmeRepository : RepositoryBase,ITrnConfiguracionPmmeRepository
    {
        public TrnConfiguracionPmmeRepository(string strConn) : base(strConn)
        {

        }

        TrnConfiguracionPmmeHelper helper = new TrnConfiguracionPmmeHelper();

        public int Save(TrnConfiguracionPmmeDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Confconcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Fechavigencia, DbType.DateTime, entity.Fechavigencia);
            dbProvider.AddInParameter(command, helper.Vigencia, DbType.String, entity.Vigencia);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public List<TrnConfiguracionPmmeDTO> List(int emprcodi, DateTime fechaInicio, DateTime fechaFin)
        {
            String query = String.Format(helper.SqlList, emprcodi, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<TrnConfiguracionPmmeDTO> entitys = new List<TrnConfiguracionPmmeDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    // EMPRNOMB
                    int iEmprNomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iPtoMediDesc = dr.GetOrdinal(helper.PtoMediDesc);
                    if (!dr.IsDBNull(iPtoMediDesc)) entity.PtoMediDesc = dr.GetString(iPtoMediDesc);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public bool ValidarExistencia(int emprcodi, int ptomedicodi, string vigencia)
        {
            bool flag = false;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlValidarExistencia);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Vigencia, DbType.String, vigencia);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                if (Convert.ToInt32(result) > 0)
                {
                    flag = true;
                }
            }

            return flag;
        }

        public List<TrnConfiguracionPmmeDTO> ListaConfPtosMMExEmpresa(int emprcodi)
        {
            String query = String.Format(helper.SqlListxEmpresa, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<TrnConfiguracionPmmeDTO> entitys = new List<TrnConfiguracionPmmeDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<TrnConfiguracionPmmeDTO> ListaTrnConfiguracionPmme(int emprcodi, int ptomedicodi, string vigencia)
        {
            String query = String.Format(helper.SqlListTrnConfiguracionxVigencia, emprcodi, ptomedicodi, vigencia);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<TrnConfiguracionPmmeDTO> entitys = new List<TrnConfiguracionPmmeDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {                 
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public TrnConfiguracionPmmeDTO GetById(int confconcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Confconcodi, DbType.Int32, confconcodi);
            TrnConfiguracionPmmeDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public void Update(TrnConfiguracionPmmeDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Fechavigencia, DbType.DateTime, entity.Fechavigencia);
            dbProvider.AddInParameter(command, helper.Vigencia, DbType.String, entity.Vigencia);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Confconcodi, DbType.Int32, entity.Confconcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int confconcodi, string estado)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);           
            dbProvider.AddInParameter(command, helper.Estado, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.Confconcodi, DbType.Int32, confconcodi);

            dbProvider.ExecuteNonQuery(command);
        }
    }
}
