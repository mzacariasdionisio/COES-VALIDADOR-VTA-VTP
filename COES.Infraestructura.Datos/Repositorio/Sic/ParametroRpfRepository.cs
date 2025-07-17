using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class ParametroRpfRepository: RepositoryBase
    {
        public ParametroRpfRepository(string strConn)
            : base(strConn)
        {

        }

        ParametroRpfHelper helper = new ParametroRpfHelper();

        public void Save(List<ParametroRpfDTO> entitys)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            foreach (ParametroRpfDTO entity in entitys)
            {
                command.Parameters.Clear();

                dbProvider.AddInParameter(command, helper.PARAMVALUE, DbType.String, entity.PARAMVALUE);
                dbProvider.AddInParameter(command, helper.PARAMRPFCODI, DbType.Int32, entity.PARAMRPFCODI);
                //dbProvider.AddInParameter(command, helper.PARAMTIPO, DbType.String, entity.PARAMTIPO);
                //dbProvider.AddInParameter(command, helper.PARAMVALUE, DbType.String, entity.PARAMVALUE);
                //dbProvider.AddInParameter(command, helper.PARAMMODULO, DbType.String, entity.PARAMMODULO);

                dbProvider.ExecuteNonQuery(command);
            }
        }
        
        public void Save(ParametroRpfDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.PARAMRPFCODI, DbType.Int32, entity.PARAMRPFCODI);
            dbProvider.AddInParameter(command, helper.PARAMTIPO, DbType.String, entity.PARAMTIPO);
            dbProvider.AddInParameter(command, helper.PARAMVALUE, DbType.String, entity.PARAMVALUE);
            dbProvider.AddInParameter(command, helper.PARAMMODULO, DbType.String, entity.PARAMMODULO);

            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarParametro(int id, decimal valor)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.PARAMVALUE, DbType.String, valor.ToString());
            dbProvider.AddInParameter(command, helper.PARAMRPFCODI, DbType.Int32, id);            

            dbProvider.ExecuteNonQuery(command);
        }


        public void GrabarHistorico(ParametroDetRpfDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxIdHistorico);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlGrabarHistorico);

            dbProvider.AddInParameter(command, helper.PARAMRPFCODI, DbType.Int32, entity.Paramrpfcodi);
            dbProvider.AddInParameter(command, helper.Paramdetcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Paramusuario, DbType.String, entity.Paramusuario);
            dbProvider.AddInParameter(command, helper.Paramdate, DbType.Date, entity.Paramdate);
            dbProvider.AddInParameter(command, helper.Paramvigencia, DbType.Date, entity.Paramvigencia);
            dbProvider.AddInParameter(command, helper.Paramvalor, DbType.Decimal, entity.Paramvalor);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Detele(string modulo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.PARAMMODULO, DbType.String, modulo);

            dbProvider.ExecuteNonQuery(command);
        }
        
        public ParametroRpfDTO GetById(int id)
        {
            ParametroRpfDTO entity = null;
            
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.PARAMRPFCODI, DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;        
        }

        public List<ParametroRpfDTO> GetByCriteria(string modulo)
        {
            List<ParametroRpfDTO> entitys = new List<ParametroRpfDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.PARAMMODULO, DbType.String, modulo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        
        public List<ParametroDetRpfDTO> ListarHistoricoParametro(int idParametro)
        {
            List<ParametroDetRpfDTO> entitys = new List<ParametroDetRpfDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarHistoricoParametro);
            dbProvider.AddInParameter(command, helper.PARAMRPFCODI, DbType.Int32, idParametro);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ParametroDetRpfDTO entity = new ParametroDetRpfDTO();

                    int iParamrpfcodi = dr.GetOrdinal(helper.PARAMRPFCODI);
                    if (!dr.IsDBNull(iParamrpfcodi)) entity.Paramrpfcodi = Convert.ToInt32(iParamrpfcodi);

                    int iParamdetcodi = dr.GetOrdinal(helper.Paramdetcodi);
                    if (!dr.IsDBNull(iParamdetcodi)) entity.Paramdetcodi = Convert.ToInt32(iParamdetcodi);

                    int iParamusuario = dr.GetOrdinal(helper.Paramusuario);
                    if (!dr.IsDBNull(iParamusuario)) entity.Paramusuario = dr.GetString(iParamusuario);

                    int iParamdate = dr.GetOrdinal(helper.Paramdate);
                    if (!dr.IsDBNull(iParamdate)) entity.Paramdate = dr.GetDateTime(iParamdate);

                    int iParamvigencia = dr.GetOrdinal(helper.Paramvigencia);
                    if (!dr.IsDBNull(iParamvigencia)) entity.Paramvigencia = dr.GetDateTime(iParamvigencia);

                    int iParamvalor = dr.GetOrdinal(helper.Paramvalor);
                    if (!dr.IsDBNull(iParamvalor)) entity.Paramvalor = dr.GetDecimal(iParamvalor);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
