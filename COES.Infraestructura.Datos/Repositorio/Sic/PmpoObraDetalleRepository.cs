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
    /// Clase de acceso a datos de la tabla PMPO_OBRA_DETALLE
    /// </summary>
    public class PmpoObraDetalleRepository : RepositoryBase, IPmpoObraDetalleRepository
    {
        public PmpoObraDetalleRepository(string strConn)
            : base(strConn)
        {
        }

        PmpoObraDetalleHelper helper = new PmpoObraDetalleHelper();

        /// <summary>
        /// Grabar Detalle de Obra
        /// </summary>
        /// <param name="entity"></param>
        public void Save(PmpoObraDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);


            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            string queryString = string.Format(helper.SqlSave, id, entity.Obracodi, entity.Grupocodi, entity.Barrcodi, entity.Equicodi, entity.Obradetdescripcion, entity.Obradetusucreacion, entity.Obradetfeccreacion.ToString(ConstantesBase.FormatoFechaExtendido));
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteScalar(command);

        }

        /// <summary>
        /// Actualizar Detalle de Obra
        /// </summary>
        /// <param name="entity"></param>
        public void Update(PmpoObraDetalleDTO entity)
        {
         
            string sqlQuery = string.Format(helper.SqlUpdate, 
                entity.Grupocodi, 
                entity.Barrcodi, 
                entity.Equicodi, 
                entity.Obradetdescripcion,
                entity.Obradetusumodificacion, 
                entity.Obradetcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.ExecuteNonQuery(command);
           
        }
        /// <summary>
        /// Eliminar Detalle de Obra
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(int obradetcodi)
        {

            string sqlQuery = string.Format(helper.SqlDelete, obradetcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.ExecuteNonQuery(command);

        }
        public List<PmpoObraDetalleDTO> GetByCriteria(int obracodi, int tipoObra,int emprcodi)
        {
            List<PmpoObraDetalleDTO> entitys = new List<PmpoObraDetalleDTO>();
            string queryString = string.Format(helper.SqlGetByCriteria, obracodi);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmpoObraDetalleDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PmpoObraDetalleDTO> GetBarras(int codEmpresa)
        {
            List<PmpoObraDetalleDTO> entitys = new List<PmpoObraDetalleDTO>();
            string queryString = string.Format(helper.SqlListBarras, codEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmpoObraDetalleDTO entity = helper.CreateBarra(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PmpoObraDetalleDTO> GetEquipos(int codEmpresa)
        {
            List<PmpoObraDetalleDTO> entitys = new List<PmpoObraDetalleDTO>();
            string queryString = string.Format(helper.SqlListEquipos, codEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmpoObraDetalleDTO entity = helper.CreateEquipo(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //ListDetalleObras(obracodi);
        public List<PmpoObraDetalleDTO> ListDetalleObras(int obracodi)
        {
            List<PmpoObraDetalleDTO> entitys = new List<PmpoObraDetalleDTO>();
            string queryString = string.Format(helper.SqlList, obracodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmpoObraDetalleDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Obtener Grupos
        /// </summary>
        /// <param name="codEmpresa"></param>
        /// <returns></returns>
        public List<PmpoObraDetalleDTO> GetGrupos(int codEmpresa)
        {
            List<PmpoObraDetalleDTO> entitys = new List<PmpoObraDetalleDTO>();
            string queryString = string.Format(helper.SqlListGrupos, codEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmpoObraDetalleDTO entity = helper.CreateGrupo(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }


    }
}
