using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CPA_PARAMETRO
    /// </summary>
    public class CpaParametroHistoricoRepository : RepositoryBase, ICpaParametroHistoricoRepository
    {
        public CpaParametroHistoricoRepository(string strConn)
            : base(strConn)
        {
        }

        CpaParametroHistoricoHelper helper = new CpaParametroHistoricoHelper();

        public int Save(CpaParametroHistoricoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpaphscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpaprmcodi, DbType.Int32, entity.Cpaprmcodi);
            dbProvider.AddInParameter(command, helper.Cpaphstipo, DbType.String, entity.Cpaphstipo);
            dbProvider.AddInParameter(command, helper.Cpaphsusuario, DbType.String, entity.Cpaphsusuario);
            dbProvider.AddInParameter(command, helper.Cpaphsfecha, DbType.DateTime, entity.Cpaphsfecha);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public List<CpaParametroHistoricoDTO> ListaParametrosHistoricos(int parametro)
        {
            CpaParametroHistoricoDTO entity = new CpaParametroHistoricoDTO();
            List<CpaParametroHistoricoDTO> entitys = new List<CpaParametroHistoricoDTO>();
            string query = string.Format(helper.SqlListaParametrosHistoricos, parametro);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpaParametroHistoricoDTO();

                    int iCpaphscodi = dr.GetOrdinal(helper.Cpaphscodi);
                    if (!dr.IsDBNull(iCpaphscodi)) entity.Cpaphscodi = dr.GetInt32(iCpaphscodi);

                    int iCpaprmcodi = dr.GetOrdinal(helper.Cpaprmcodi);
                    if (!dr.IsDBNull(iCpaprmcodi)) entity.Cpaprmcodi = dr.GetInt32(iCpaprmcodi);

                    int iCpaphstipo = dr.GetOrdinal(helper.Cpaphstipo);
                    if (!dr.IsDBNull(iCpaphstipo)) entity.Cpaphstipo = dr.GetString(iCpaphstipo);

                    int iCpaphsusuario = dr.GetOrdinal(helper.Cpaphsusuario);
                    if (!dr.IsDBNull(iCpaphsusuario)) entity.Cpaphsusuario = dr.GetString(iCpaphsusuario);

                    int iCpaphsfecha = dr.GetOrdinal(helper.Cpaphsfecha);
                    if (!dr.IsDBNull(iCpaphsfecha)) entity.Cpaphsfecha = dr.GetDateTime(iCpaphsfecha);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
