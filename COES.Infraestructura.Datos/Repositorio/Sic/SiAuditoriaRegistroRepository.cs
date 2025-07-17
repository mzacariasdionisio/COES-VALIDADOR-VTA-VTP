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
    /// Clase de acceso a datos de la tabla SI_AUDITORIA_REGISTRO
    /// </summary>
    public class SiAuditoriaregistroRepository: RepositoryBase, ISiAuditoriaRegistroRepository
    {
       
        public SiAuditoriaregistroRepository(string strConn): base(strConn)
        {
        }

        SiAuditoriaRegistroHelper helper = new SiAuditoriaRegistroHelper();
        public int Save(SiAuditoriaRegistroDTO entity)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);            
            int id = GetCodigoGenerado();           

            dbProvider.AddInParameter(command, helper.AUDITCODI, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.TAUDITCODI, DbType.Int32, entity.TauditCodi);
            dbProvider.AddInParameter(command, helper.AUDITNOMBRESISTEMA, DbType.String, entity.AuditNombreSistema);
            dbProvider.AddInParameter(command, helper.AUDITTABLACODI, DbType.Int32, entity.AuditTablaCodi);
            dbProvider.AddInParameter(command, helper.AUDITREGDET, DbType.String, entity.AuditRegDet.ToString());
            dbProvider.AddInParameter(command, helper.AUDITUSUARIOCREACION, DbType.String, entity.AuditUsuarioCreacion);
            dbProvider.AddInParameter(command, helper.AUDITFECHACREACION, DbType.DateTime, entity.AuditFechaCreacion);
            
            dbProvider.ExecuteNonQuery(command);
           
            return id;
        }

        /// <summary>
        /// Actualiza registro auditoria.
        /// Por ahora no se emplea - Junio 2015
        /// </summary>        
        public void Update(SiAuditoriaRegistroDTO entity)
        {   
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.TAUDITCODI, DbType.Int32, entity.TauditCodi);
            dbProvider.AddInParameter(command, helper.AUDITNOMBRESISTEMA, DbType.String, entity.AuditNombreSistema);
            dbProvider.AddInParameter(command, helper.AUDITTABLACODI, DbType.Int32, entity.AuditTablaCodi);
            dbProvider.AddInParameter(command, helper.AUDITREGDET, DbType.Xml, entity.AuditRegDet);
            dbProvider.AddInParameter(command, helper.AUDITUSUARIOCREACION, DbType.String, entity.AuditUsuarioCreacion);
            dbProvider.AddInParameter(command, helper.AUDITFECHACREACION, DbType.DateTime, entity.AuditFechaCreacion);
            dbProvider.AddInParameter(command, helper.AUDITCODI, DbType.Int32, entity.AuditCodi);

            dbProvider.ExecuteNonQuery(command);    
        }

        /// <summary>
        /// Elimina registro auditoria.
        /// Por ahora no se emplea - Junio 2015
        /// </summary>        
        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.ExecuteNonQuery(command);
        }

        /// <summary>
        /// Obtener registro uno por uno
        /// Por ahora no se emplea - Junio 2015
        /// </summary>        
        public SiAuditoriaRegistroDTO GetById()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            SiAuditoriaRegistroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }


        /// <summary>
        /// Obtener registros de la tabla Auditoria
        /// </summary>        

        public List<SiAuditoriaRegistroDTO> List(int Taudit,int id)
        {
            List<SiAuditoriaRegistroDTO> entitys = new List<SiAuditoriaRegistroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.TAUDITCODI, DbType.Int32, Taudit);
            dbProvider.AddInParameter(command, helper.AUDITTABLACODI, DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        /// <summary>
        /// Obtener registros de la tabla Auditoria
        /// </summary>  
        public List<SiAuditoriaRegistroDTO> ListPaginado(int Taudit, int id, int NroPaginado, int PageSize)
        {
            List<SiAuditoriaRegistroDTO> entitys = new List<SiAuditoriaRegistroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.TAUDITCODI, DbType.Int32, Taudit);
            dbProvider.AddInParameter(command, helper.AUDITTABLACODI, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.PageNumber, DbType.Int32, NroPaginado);
            dbProvider.AddInParameter(command, helper.PageSize, DbType.Int32, PageSize);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }


        /// <summary>
        /// Obtener registro por criterios
        /// Por ahora no se emplea - Junio 2015
        /// </summary>        
        public List<SiAuditoriaRegistroDTO> GetByCriteria()
        {
            List<SiAuditoriaRegistroDTO> entitys = new List<SiAuditoriaRegistroDTO>();
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

        /// <summary>
        /// Obtener Lista de Usuario de Auditoria
        /// </summary>        
        public List<SiAuditoriaRegistroDTO> GetByUsuariosAuditoria()
        {
            List<SiAuditoriaRegistroDTO> entitys = new List<SiAuditoriaRegistroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByUsuariosAuditoria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiAuditoriaRegistroDTO et=new SiAuditoriaRegistroDTO();

                    int iAUDITCODI = dr.GetOrdinal(helper.AUDITCODI);
                    if (!dr.IsDBNull(iAUDITCODI)) et.AuditCodi = Convert.ToInt32(dr.GetValue(iAUDITCODI));

                    int iAUDITUSUARIOCREACION = dr.GetOrdinal(helper.AUDITUSUARIOCREACION);
                    if (!dr.IsDBNull(iAUDITUSUARIOCREACION)) et.AuditUsuarioCreacion = dr.GetString(iAUDITUSUARIOCREACION);

                    entitys.Add(new SiAuditoriaRegistroDTO()
                    {
                        AuditCodi = et.AuditCodi,
                        AuditUsuarioCreacion = et.AuditUsuarioCreacion
                    });
                }
            }

            return entitys;
        }
        

        /// <summary>
        /// Obtener Codigo Generado ( ID ) para registrar en la tabla auditoria
        /// </summary>        

        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }
    }
}
