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
    /// Clase de acceso a datos de la tabla EVE_INFORME_ITEM
    /// </summary>
    public class EveInformeItemRepository: RepositoryBase, IEveInformeItemRepository
    {
        public EveInformeItemRepository(string strConn): base(strConn)
        {
        }

        EveInformeItemHelper helper = new EveInformeItemHelper();

        public int Save(EveInformeItemDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Infitemcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Eveninfcodi, DbType.Int32, entity.Eveninfcodi);
            dbProvider.AddInParameter(command, helper.Itemnumber, DbType.Int32, entity.Itemnumber);
            dbProvider.AddInParameter(command, helper.Subitemnumber, DbType.Int32, entity.Subitemnumber);
            dbProvider.AddInParameter(command, helper.Nroitem, DbType.Int32, entity.Nroitem);
            dbProvider.AddInParameter(command, helper.Potactiva, DbType.Decimal, entity.Potactiva);
            dbProvider.AddInParameter(command, helper.Potreactiva, DbType.Decimal, entity.Potreactiva);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Niveltension, DbType.Decimal, entity.Niveltension);
            dbProvider.AddInParameter(command, helper.Desobservacion, DbType.String, entity.Desobservacion);
            dbProvider.AddInParameter(command, helper.Itemhora, DbType.String, entity.Itemhora);
            dbProvider.AddInParameter(command, helper.Senializacion, DbType.String, entity.Senializacion);
            dbProvider.AddInParameter(command, helper.Interrcodi, DbType.Int32, entity.Interrcodi);
            dbProvider.AddInParameter(command, helper.Ac, DbType.String, entity.Ac);
            dbProvider.AddInParameter(command, helper.Ra, DbType.Int32, entity.Ra);
            dbProvider.AddInParameter(command, helper.Sa, DbType.Int32, entity.Sa);
            dbProvider.AddInParameter(command, helper.Ta, DbType.Int32, entity.Ta);
            dbProvider.AddInParameter(command, helper.Rd, DbType.Int32, entity.Rd);
            dbProvider.AddInParameter(command, helper.Sd, DbType.Int32, entity.Sd);
            dbProvider.AddInParameter(command, helper.Td, DbType.Int32, entity.Td);
            dbProvider.AddInParameter(command, helper.Descomentario, DbType.String, entity.Descomentario);
            dbProvider.AddInParameter(command, helper.Sumininistro, DbType.String, entity.Sumininistro);
            dbProvider.AddInParameter(command, helper.Potenciamw, DbType.Decimal, entity.Potenciamw);
            dbProvider.AddInParameter(command, helper.Proteccion, DbType.String, entity.Proteccion);
            dbProvider.AddInParameter(command, helper.Intinicio, DbType.DateTime, entity.Intinicio);
            dbProvider.AddInParameter(command, helper.Intfin, DbType.DateTime, entity.Intfin);
            dbProvider.AddInParameter(command, helper.Subestacionde, DbType.String, entity.Subestacionde);
            dbProvider.AddInParameter(command, helper.Subestacionhasta, DbType.String, entity.Subestacionhasta);
            dbProvider.AddInParameter(command, helper.Ptointerrcodi, DbType.Int32, entity.Ptointerrcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void SaveConsolidado(int idEvento, int idInforme, string version)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            string sql = string.Format(helper.SqlSaveConsolidado, idEvento, id, idInforme, version);
            command = dbProvider.GetSqlStringCommand(sql);

            dbProvider.ExecuteNonQuery(command);        
        }
        
        public void DeleteConsolidado(int idEvento)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteConsolidado);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, idEvento);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(EveInformeItemDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
                        
            dbProvider.AddInParameter(command, helper.Eveninfcodi, DbType.Int32, entity.Eveninfcodi);
            dbProvider.AddInParameter(command, helper.Itemnumber, DbType.Int32, entity.Itemnumber);
            dbProvider.AddInParameter(command, helper.Subitemnumber, DbType.Int32, entity.Subitemnumber);
            dbProvider.AddInParameter(command, helper.Nroitem, DbType.Int32, entity.Nroitem);
            dbProvider.AddInParameter(command, helper.Potactiva, DbType.Decimal, entity.Potactiva);
            dbProvider.AddInParameter(command, helper.Potreactiva, DbType.Decimal, entity.Potreactiva);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Niveltension, DbType.Decimal, entity.Niveltension);
            dbProvider.AddInParameter(command, helper.Desobservacion, DbType.String, entity.Desobservacion);
            dbProvider.AddInParameter(command, helper.Itemhora, DbType.String, entity.Itemhora);
            dbProvider.AddInParameter(command, helper.Senializacion, DbType.String, entity.Senializacion);
            dbProvider.AddInParameter(command, helper.Interrcodi, DbType.Int32, entity.Interrcodi);
            dbProvider.AddInParameter(command, helper.Ac, DbType.String, entity.Ac);
            dbProvider.AddInParameter(command, helper.Ra, DbType.Int32, entity.Ra);
            dbProvider.AddInParameter(command, helper.Sa, DbType.Int32, entity.Sa);
            dbProvider.AddInParameter(command, helper.Ta, DbType.Int32, entity.Ta);
            dbProvider.AddInParameter(command, helper.Rd, DbType.Int32, entity.Rd);
            dbProvider.AddInParameter(command, helper.Sd, DbType.Int32, entity.Sd);
            dbProvider.AddInParameter(command, helper.Td, DbType.Int32, entity.Td);
            dbProvider.AddInParameter(command, helper.Descomentario, DbType.String, entity.Descomentario);
            dbProvider.AddInParameter(command, helper.Sumininistro, DbType.String, entity.Sumininistro);
            dbProvider.AddInParameter(command, helper.Potenciamw, DbType.Decimal, entity.Potenciamw);
            dbProvider.AddInParameter(command, helper.Proteccion, DbType.String, entity.Proteccion);
            dbProvider.AddInParameter(command, helper.Intinicio, DbType.DateTime, entity.Intinicio);
            dbProvider.AddInParameter(command, helper.Intfin, DbType.DateTime, entity.Intfin);          
            dbProvider.AddInParameter(command, helper.Subestacionde, DbType.String, entity.Subestacionde);
            dbProvider.AddInParameter(command, helper.Subestacionhasta, DbType.String, entity.Subestacionhasta);            
            dbProvider.AddInParameter(command, helper.Ptointerrcodi, DbType.Int32, entity.Ptointerrcodi);
            dbProvider.AddInParameter(command, helper.Infitemcodi, DbType.Int32, entity.Infitemcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int infitemcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Infitemcodi, DbType.Int32, infitemcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeletePorInforme(int idInforme)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeletePorInforme);

            dbProvider.AddInParameter(command, helper.Eveninfcodi, DbType.Int32, idInforme);

            dbProvider.ExecuteNonQuery(command);
        }

        

        public EveInformeItemDTO GetById(int infitemcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Infitemcodi, DbType.Int32, infitemcodi);
            EveInformeItemDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveInformeItemDTO> List()
        {
            List<EveInformeItemDTO> entitys = new List<EveInformeItemDTO>();
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

        public List<EveInformeItemDTO> GetByCriteria(int idInforme)
        {
            List<EveInformeItemDTO> entitys = new List<EveInformeItemDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Eveninfcodi, DbType.Int32, idInforme);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EveInformeItemDTO> ObtenerItemInformeEvento(int idInforme)
        {
            List<EveInformeItemDTO> entitys = new List<EveInformeItemDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerItemInformeEvento);
            dbProvider.AddInParameter(command, helper.Eveninfcodi, DbType.Int32, idInforme);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveInformeItemDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iInternomb = dr.GetOrdinal(helper.Internomb);
                    if (!dr.IsDBNull(iInternomb)) entity.Internomb = dr.GetString(iInternomb);

                    if (entity.Intinicio != null && entity.Intfin != null)
                    {
                        TimeSpan duracion = ((DateTime)entity.Intfin) - ((DateTime)entity.Intinicio);
                        entity.Duracion = (decimal)(Math.Truncate(duracion.TotalMinutes * 100) / 100);
                    }
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveInformeItemDTO> ObtenerItemInformeEvento(int idEvento, int idEmpresa)
        {
            List<EveInformeItemDTO> entitys = new List<EveInformeItemDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerItemTotalInformeEvento);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, idEvento);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, idEmpresa);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveInformeItemDTO entity = helper.Create(dr);
                    
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);
                    
                    int iInternomb = dr.GetOrdinal(helper.Internomb);
                    if (!dr.IsDBNull(iInternomb)) entity.Internomb = dr.GetString(iInternomb);

                    if (entity.Intinicio != null && entity.Intfin != null)
                    {
                        TimeSpan duracion = ((DateTime)entity.Intfin) - ((DateTime)entity.Intinicio);
                        entity.Duracion = (decimal)(Math.Truncate(duracion.TotalMinutes * 100) / 100);
                    }
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public EveInformeItemDTO ObtenerItemInformePorId(int idItemCodi)
        {
            EveInformeItemDTO entity = null;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerItemInformePorId);
            dbProvider.AddInParameter(command, helper.Infitemcodi, DbType.Int32, idItemCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iInternomb = dr.GetOrdinal(helper.Internomb);
                    if (!dr.IsDBNull(iInternomb)) entity.Internomb = dr.GetString(iInternomb);

                    if (entity.Intinicio != null && entity.Intfin != null)
                    {
                        TimeSpan duracion = ((DateTime)entity.Intfin) - ((DateTime)entity.Intinicio);
                        entity.Duracion = (decimal)(Math.Truncate(duracion.TotalMinutes * 100) / 100);
                    }
                }
            }

            return entity;
        }

        public bool VerificarExistencia(int idInforme, int itemNumber)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlVerificarExistencia);
            dbProvider.AddInParameter(command, helper.Eveninfcodi, DbType.Int32, idInforme);
            dbProvider.AddInParameter(command, helper.Itemnumber, DbType.Int32, itemNumber);

            bool flag = false;

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

        public void ActualizarTextoInforme(int idItemInforme, string comentario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarTextoInforme);
            dbProvider.AddInParameter(command, helper.Descomentario, DbType.String, comentario);
            dbProvider.AddInParameter(command, helper.Infitemcodi, DbType.Int32, idItemInforme);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<EveInformeItemDTO> ObtenerInformeInterrupcion(int idEvento)
        {
            List<EveInformeItemDTO> entitys = new List<EveInformeItemDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerInformeInterrupcion);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, idEvento);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveInformeItemDTO entity = helper.Create(dr);

                    if (entity.Intinicio != null && entity.Intfin != null)
                    {
                        TimeSpan duracion = ((DateTime)entity.Intfin) - ((DateTime)entity.Intinicio);
                        entity.Duracion = (decimal)(Math.Truncate(duracion.TotalMinutes * 100) / 100);
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void ActualizarInformeItem(int idItemInforme, int idPtoInterrupcion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarInformeItem);

            dbProvider.AddInParameter(command, helper.Ptointerrcodi, DbType.Int32, idPtoInterrupcion);
            dbProvider.AddInParameter(command, helper.Infitemcodi, DbType.Int32, idItemInforme);

            dbProvider.ExecuteNonQuery(command);
        }

    }
}
