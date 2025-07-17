// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Daniel Sanchez Hermosa
// Acronimo: DSH
// Requerimiento: compensaciones
//
// Fecha creacion: 18/05/2017
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================

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

    public class VceTextoReporteRepository : RepositoryBase, IVceTextoReporteRepository
    {
        
        public VceTextoReporteRepository(string strConn) : base(strConn)
        {
        }

        VceTextoReporteHelper helper = new VceTextoReporteHelper();

        public void Save(VceTextoReporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            //- Claves:
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);
            dbProvider.AddInParameter(command, helper.Txtrepcodreporte, DbType.String, entity.Txtrepcodreporte);
            dbProvider.AddInParameter(command, helper.Txtrepcodtexto, DbType.String, entity.Txtrepcodtexto);

            //- Otros:
            dbProvider.AddInParameter(command, helper.Txtreptexto, DbType.String, entity.Txtreptexto);
            dbProvider.AddInParameter(command, helper.Txtrepusucreacion, DbType.String, entity.Txtrepusucreacion);
            dbProvider.AddInParameter(command, helper.Txtrepfeccreacion, DbType.DateTime, entity.Txtrepfeccreacion);
           

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(VceTextoReporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Txtreptexto, DbType.String, entity.Txtreptexto);
            dbProvider.AddInParameter(command, helper.Txtrepusumodificacion, DbType.String, entity.Txtrepusumodificacion);
            dbProvider.AddInParameter(command, helper.Txtrepfecmodificacion, DbType.DateTime, entity.Txtrepfecmodificacion);
            //- Claves:
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);
            dbProvider.AddInParameter(command, helper.Txtrepcodreporte, DbType.String, entity.Txtrepcodreporte);
            dbProvider.AddInParameter(command, helper.Txtrepcodtexto, DbType.String, entity.Txtrepcodtexto);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(VceTextoReporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            //- Claves:
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);
            dbProvider.AddInParameter(command, helper.Txtrepcodreporte, DbType.String, entity.Txtrepcodreporte);
            dbProvider.AddInParameter(command, helper.Txtrepcodtexto, DbType.String, entity.Txtrepcodtexto);

            dbProvider.ExecuteNonQuery(command);
        }


        public VceTextoReporteDTO GetById(int PecaCodi, string Txtrepcodreporte, string Txtrepcodtexto)
        {
            VceTextoReporteDTO entity = null;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, PecaCodi);
            dbProvider.AddInParameter(command, helper.Txtrepcodreporte, DbType.String, Txtrepcodreporte);
            dbProvider.AddInParameter(command, helper.Txtrepcodtexto, DbType.String, Txtrepcodtexto);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VceTextoReporteDTO> ListByPeriodo(int PecaCodi)
        {
            List<VceTextoReporteDTO> entitys = new List<VceTextoReporteDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByPeriodo);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, PecaCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public void DeleteByVersion(int pecacodi)
        {
            try
            {
                string queryString = string.Format(helper.SqlDeleteByVersion, pecacodi);
                DbCommand command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveFromOtherVersion(int pecacodiDestino, int pecacodiOrigen, string usuCreacion)
        {
            string strFecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            string queryString = string.Format(helper.SqlSaveFromOtherVersion, pecacodiDestino, pecacodiOrigen, usuCreacion, strFecha);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }
    }

}
