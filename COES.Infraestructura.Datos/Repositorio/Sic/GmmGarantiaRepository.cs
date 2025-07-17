using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class GmmGarantiaRepository : RepositoryBase, IGmmGarantiaRepository
    {
        public GmmGarantiaRepository(string strConn)
            : base(strConn)
        {
        }

        GmmGarantiaHelper helper = new GmmGarantiaHelper();

        public int Save(GmmGarantiaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Garacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Garafecinicio, DbType.String, Convert.ToDateTime(entity.GARAFECINICIO).ToString("yyyy-MM-dd"));
            dbProvider.AddInParameter(command, helper.Garafecfin, DbType.String, Convert.ToDateTime(entity.GARAFECFIN).ToString("yyyy-MM-dd")); // > 0 ? entity.Equicodi : (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Garamontogarantia, DbType.Decimal, entity.GARAMONTOGARANTIA);
            dbProvider.AddInParameter(command, helper.Garaarchivo, DbType.String, entity.GARAARCHIVO);
            dbProvider.AddInParameter(command, helper.Garaestado, DbType.String, entity.GARAESTADO);
            dbProvider.AddInParameter(command, helper.Garausucreacion, DbType.String, entity.GARAUSUCREACION);
            //dbProvider.AddInParameter(command, helper.Garafeccreacion, DbType.DateTime, entity.GARAFECCREACION);
            //dbProvider.AddInParameter(command, helper.Garausumodificacion, DbType.String, entity.GARAUSUMODIFICACION);
            //dbProvider.AddInParameter(command, helper.Garafecmodificacion, DbType.DateTime, entity.GARAFECMODIFICACION);
            dbProvider.AddInParameter(command, helper.Tcercodi, DbType.String, entity.TCERCODI);
            dbProvider.AddInParameter(command, helper.Tmodcodi, DbType.String, entity.TMODCODI);
            dbProvider.AddInParameter(command, helper.Empgcodi, DbType.Int32, entity.EMPGCODI);

            var iRslt = dbProvider.ExecuteNonQuery(command);
            if (iRslt <= 0)
                id = 0;

            return id;
        }

        public int Update(GmmGarantiaDTO entity)
        {
            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            //object result = dbProvider.ExecuteScalar(command);


            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);


            dbProvider.AddInParameter(command, helper.Garafecinicio, DbType.String, Convert.ToDateTime(entity.GARAFECINICIO).ToString("yyyy-MM-dd"));
            dbProvider.AddInParameter(command, helper.Garafecfin, DbType.String, Convert.ToDateTime(entity.GARAFECFIN).ToString("yyyy-MM-dd")); // > 0 ? entity.Equicodi : (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Garamontogarantia, DbType.Decimal, entity.GARAMONTOGARANTIA);
            dbProvider.AddInParameter(command, helper.Garaarchivo, DbType.String, entity.GARAARCHIVO);
            dbProvider.AddInParameter(command, helper.Tcercodi, DbType.String, entity.TCERCODI);
            dbProvider.AddInParameter(command, helper.Tmodcodi, DbType.String, entity.TMODCODI);
            dbProvider.AddInParameter(command, helper.Garaestado, DbType.String, entity.GARAESTADO);
            // dbProvider.AddInParameter(command, helper.Garausucreacion, DbType.String, entity.GARAUSUCREACION);
            //dbProvider.AddInParameter(command, helper.Garafeccreacion, DbType.DateTime, entity.GARAFECCREACION);
            dbProvider.AddInParameter(command, helper.Garausumodificacion, DbType.String, entity.GARAUSUMODIFICACION);
            //dbProvider.AddInParameter(command, helper.Garafecmodificacion, DbType.DateTime, entity.GARAFECMODIFICACION);

            dbProvider.AddInParameter(command, helper.Garacodi, DbType.Int32, entity.GARACODI);

            var iRslt = dbProvider.ExecuteNonQuery(command);
            if (iRslt <= 0)
                iRslt = 0;

            return iRslt;
        }
        public bool Delete(GmmGarantiaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


            dbProvider.AddInParameter(command, helper.Garausumodificacion, DbType.String, entity.GARAUSUMODIFICACION);
            dbProvider.AddInParameter(command, helper.Garacodi, DbType.Int32, entity.GARACODI);
            //dbProvider.AddInParameter(command, helper.Garafecmodificacion, DbType.DateTime, entity.GARAFECMODIFICACION);

            int numberOfRecords = dbProvider.ExecuteNonQuery(command);

            if (numberOfRecords > 0)
                return true;
            else
                return false;
        }

        public GmmGarantiaDTO GetByEmpgcodi(int empgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByEmpgcodi);

            dbProvider.AddInParameter(command, helper.Empgcodi, DbType.Int32, empgcodi);
            GmmGarantiaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public GmmGarantiaDTO mensajeProcesamiento(int anio, string mes)
        {
            GmmGarantiaDTO entity = new GmmGarantiaDTO(); ;
            string queryString = string.Format(helper.SqlGetListadoProcesamiento, anio, mes);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.CreateListadoProcesamiento(dr);
                }
            }
            return entity;
        }
        public List<GmmGarantiaDTO> mensajeProcesamientoParticipante(int anio, string mes)
        {
            List<GmmGarantiaDTO> lista = new List<GmmGarantiaDTO>();
            GmmGarantiaDTO entity = new GmmGarantiaDTO(); ;
            string queryString = string.Format(helper.SqlGetListadoProcesamientoParticipante, anio, mes);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.CreateListadoProcesamiento(dr);
                    lista.Add(entity);
                }
            }
            return lista;
        }
    }
}
