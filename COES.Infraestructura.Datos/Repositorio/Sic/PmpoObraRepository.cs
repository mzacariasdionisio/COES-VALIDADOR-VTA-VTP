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
    /// Clase de acceso a datos de la tabla PMPO_OBRA
    /// </summary>
    public class PmpoObraRepository : RepositoryBase, IPmpoObraRepository
    {
        public PmpoObraRepository(string strConn)
            : base(strConn)
        {
        }

        PmpoObraHelper helper = new PmpoObraHelper();

        public void Update(PmpoObraDTO entity)
        {

            DateTime? dte = entity.Obrafechaplanificada;
            string fecha = String.Format("{0:yyyy/MM/dd}", dte);

            string sqlQuery = string.Format(helper.SqlUpdate, fecha, entity.Obradescripcion, entity.Obrausucreacion, entity.Obracodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.ExecuteNonQuery(command);
        }

        public int Save(PmpoObraDTO entity)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Obracodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.TObracodi, DbType.Int32, entity.TObracodi);
            dbProvider.AddInParameter(command, helper.Obrafechaplanificada, DbType.DateTime, entity.Obrafechaplanificada);
            dbProvider.AddInParameter(command, helper.ObraFlagFormat, DbType.Int32, entity.ObraFlagFormat);
            dbProvider.AddInParameter(command, helper.Obradescripcion, DbType.String, entity.Obradescripcion);
            dbProvider.AddInParameter(command, helper.Obrausucreacion, DbType.String, entity.Obrausucreacion);
            

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void Delete(int obracodi)
        {
            string sqlQuery = string.Format(helper.SqlDelete, obracodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.ExecuteNonQuery(command);
        }

        public PmpoObraDTO GetById(int obracodi, int idTipoObra)
        {
            string queryString = string.Format(helper.SqlGetById, obracodi);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            PmpoObraDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }


        public List<PmpoObraDTO> List(string idEmpresa, int idTipoObra, DateTime fechaIni, DateTime fechaFin, string formatList)
        {
            List<PmpoObraDTO> entitys = new List<PmpoObraDTO>();
            string queryString = string.Format(helper.SqlList, fechaIni.ToString(ConstantesBase.FormatoFechaMes), fechaFin.ToString(ConstantesBase.FormatoFechaMes), idEmpresa, idTipoObra, formatList);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmpoObraDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PmpoObraDTO> ListObras(string idEmpresa, int idTipoObra, DateTime fechaIni, DateTime fechaFin)
        {
            List<PmpoObraDTO> entitys = new List<PmpoObraDTO>();
            string queryString = string.Format(helper.SqlListObras, fechaIni.ToString(ConstantesBase.FormatoFechaMes), fechaFin.ToString(ConstantesBase.FormatoFechaMes), idEmpresa, idTipoObra);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmpoObraDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
