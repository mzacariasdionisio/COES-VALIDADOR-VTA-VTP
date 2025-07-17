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
    /// Clase de acceso a datos de la tabla EQ_AREA
    /// </summary>
    public class EqAreaRepository : RepositoryBase, IEqAreaRepository
    {
        public EqAreaRepository(string strConn) : base(strConn)
        {
        }

        EqAreaHelper helper = new EqAreaHelper();

        public int Save(EqAreaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Anivelcodi, DbType.Int32, entity.ANivelCodi);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tareacodi, DbType.Int32, entity.Tareacodi);
            dbProvider.AddInParameter(command, helper.Areaabrev, DbType.String, entity.Areaabrev);
            dbProvider.AddInParameter(command, helper.Areanomb, DbType.String, entity.Areanomb);
            dbProvider.AddInParameter(command, helper.Areapadre, DbType.Int32, entity.Areapadre);
            dbProvider.AddInParameter(command, helper.Areaestado, DbType.String, entity.Areaestado);
            dbProvider.AddInParameter(command, helper.Usuariocreacion, DbType.String, entity.UsuarioCreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EqAreaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Anivelcodi, DbType.Int32, entity.ANivelCodi);
            dbProvider.AddInParameter(command, helper.Tareacodi, DbType.Int32, entity.Tareacodi);
            dbProvider.AddInParameter(command, helper.Areaabrev, DbType.String, entity.Areaabrev);
            dbProvider.AddInParameter(command, helper.Areanomb, DbType.String, entity.Areanomb);
            dbProvider.AddInParameter(command, helper.Areapadre, DbType.Int32, entity.Areapadre);
            dbProvider.AddInParameter(command, helper.Areaestado, DbType.String, entity.Areaestado);
            dbProvider.AddInParameter(command, helper.Usuarioupdate, DbType.String, entity.UsuarioUpdate);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int areacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, areacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(int areacodi, string username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);

            dbProvider.AddInParameter(command, helper.Usuarioupdate, DbType.String, username);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, areacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EqAreaDTO GetById(int areacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, areacodi);
            EqAreaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EqAreaDTO> List()
        {
            List<EqAreaDTO> entitys = new List<EqAreaDTO>();
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

        public List<EqAreaDTO> GetByCriteria()
        {
            List<EqAreaDTO> entitys = new List<EqAreaDTO>();
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
        /// Listado de áreas filtradas por tipo de área y nombre de área
        /// </summary>
        /// <param name="idTipoArea">Id de Tipo de Área, para evitar este filtro usar -99</param>
        /// <param name="strNombreArea">Nombre de Área</param>
        /// <param name="strEstado">Estado de Área</param>
        /// <param name="nroPagina">Cantidad de Página</param>
        /// <param name="nroFilas">Cantidad de Registros por Página</param>
        /// <returns></returns>
        public List<EqAreaDTO> ListarxFiltro(int idTipoArea, string strNombreArea, string strEstado, int nroPagina, int nroFilas)
        {
            if (string.IsNullOrEmpty(strNombreArea)) strNombreArea = string.Empty;
            List<EqAreaDTO> entitys = new List<EqAreaDTO>();
            string strQuery = string.Format(helper.SqlAreasPorFiltro, idTipoArea, strNombreArea, strEstado, nroPagina, nroFilas);

            DbCommand command = dbProvider.GetSqlStringCommand(strQuery);
            //dbProvider.AddInParameter(command, helper.Tareacodi, DbType.Int32, idTipoArea);
            //dbProvider.AddInParameter(command, helper.Tareacodi, DbType.Int32, idTipoArea);
            //dbProvider.AddInParameter(command, helper.Areanomb, DbType.String, strNombreArea);
            //dbProvider.AddInParameter(command, helper.Areaestado, DbType.String, strEstado);
            //dbProvider.AddInParameter(command, "nropaginas", DbType.Int32, nroPagina);
            //dbProvider.AddInParameter(command, "nroFilas", DbType.Int32, nroFilas);
            //dbProvider.AddInParameter(command, "nropaginas", DbType.Int32, nroPagina);
            //dbProvider.AddInParameter(command, "nroFilas", DbType.Int32, nroFilas);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entidad = new EqAreaDTO();
                    entidad = helper.Create(dr);
                    entidad.Tareanomb = dr.GetString(dr.GetOrdinal("TAREANOMB"));
                    entitys.Add(entidad);
                }
            }
            return entitys;
        }

        /// <summary>
        /// Obtiene la cantidad total de resultados de la Consulta ListarxFiltro
        /// </summary>
        /// <param name="idTipoArea">Id de Tipo de Área, para evitar este filtro usar -99</param>
        /// <param name="strNombreArea">Nombre de Área</param>
        /// <returns></returns>
        public int CantidadListarxFiltro(int idTipoArea, string strNombreArea, string strEstado)
        {
            if (string.IsNullOrEmpty(strNombreArea)) strNombreArea = string.Empty;

            string strQuery = string.Format(helper.SqlCantidadAreasPorFiltro, idTipoArea, strNombreArea, strEstado);

            DbCommand command = dbProvider.GetSqlStringCommand(strQuery);
            //dbProvider.AddInParameter(command, helper.Tareacodi, DbType.Int32, idTipoArea);
            //dbProvider.AddInParameter(command, helper.Tareacodi, DbType.Int32, idTipoArea);
            //dbProvider.AddInParameter(command, helper.Areanomb, DbType.String, strNombreArea);
            //dbProvider.AddInParameter(command, helper.Areaestado, DbType.String, strEstado);
            //dbProvider.AddInParameter(command, helper.Areaestado, DbType.String, strEstado);
            object count = dbProvider.ExecuteScalar(command);
            if (count != null) return Convert.ToInt32(count);
            return 0;
        }

        public List<EqAreaDTO> ObtenerAreaPorEmpresa(int idEmpresa)
        {
            List<EqAreaDTO> entitys = new List<EqAreaDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerAreaPorEmpresa);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, idEmpresa);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqAreaDTO entity = new EqAreaDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqAreaDTO> ListSubEstacion()
        {
            List<EqAreaDTO> entitys = new List<EqAreaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListSubEstacion);

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
        /// Listado de todas las Ubicaciones (Areas) que están activas
        /// </summary>
        /// <returns></returns>
        public List<EqAreaDTO> ListaTodasAreasActivas()
        {
            var entitys = new List<EqAreaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlTodasAreasActivas);

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
        /// Listado de todas las Ubicaciones activas filtradas por tipo de Ubicación.
        /// </summary>
        /// <param name="iTipoArea">Código Tipo de Ubicación,para evitar este filtro usar -99</param>
        /// <returns></returns>
        public List<EqAreaDTO> ListaTodasAreasActivasPorTipoArea(int iTipoArea)
        {
            var entitys = new List<EqAreaDTO>();
            string strCommand = string.Format(helper.SqlTodasAreasActivasPorTipoArea, iTipoArea);
            DbCommand command = dbProvider.GetSqlStringCommand(strCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public void Delete(EqAreaDTO entity)
        {
            var command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<EqAreaDTO> List(int minRowToFetch, int maxRowToFetch)
        {
            var entities = new List<EqAreaDTO>();
            var command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, EqAreaHelper.MinRowToFetch, DbType.Int32, minRowToFetch);
            dbProvider.AddInParameter(command, EqAreaHelper.MaxRowToFetch, DbType.Int32, maxRowToFetch);

            using (var dataReader = dbProvider.ExecuteReader(command))
            {
                while (dataReader.Read())
                {
                    var entity = helper.Create(dataReader);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        #region PR5
        public List<EqAreaDTO> ListarAreaPorEmpresas(string idEmpresa, string estadoEquipo)
        {
            List<EqAreaDTO> entitys = new List<EqAreaDTO>();
            string sqlQuery = string.Format(this.helper.SqlListarAreaPorEmpresas, idEmpresa, estadoEquipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqAreaDTO entity = new EqAreaDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region ZONAS
        /// <summary>
        /// Listado de todas las Zonas que están activas
        /// </summary>
        /// <returns></returns> 
        public List<EqAreaDTO> ListarZonasActivas()
        {
            var entitys = new List<EqAreaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarZonasActivas);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqAreaDTO entity = new EqAreaDTO();
                    entity = helper.Create(dr);
                    entity.ANivelNomb = dr.GetString(dr.GetOrdinal("ANIVELNOMB"));
                    entity.Tareanomb = dr.GetString(dr.GetOrdinal("TAREANOMB"));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Listado de las zonas filtradas por niveles
        /// </summary>
        /// <returns></returns>
        public List<EqAreaDTO> ListarZonasxNivel(int anivelcodi)
        {
            List<EqAreaDTO> entitys = new List<EqAreaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarZonasxNivel);

            dbProvider.AddInParameter(command, helper.Anivelcodi, DbType.Int32, anivelcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqAreaDTO entity = new EqAreaDTO();
                    entity = helper.Create(dr);
                    entity.Tareanomb = dr.GetString(dr.GetOrdinal("TAREANOMB"));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Listado de todas las Areas que están activas y que no son Zonas
        /// </summary>
        /// <returns></returns>
        public List<EqAreaDTO> ListaSoloAreasActivas()
        {
            var entitys = new List<EqAreaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSoloAreasActivas);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    if ((helper.Create(dr).Tareacodi == 1) || (helper.Create(dr).Tareacodi == 2) ||
                        (helper.Create(dr).Tareacodi == 3) || (helper.Create(dr).Tareacodi == 4) ||
                        (helper.Create(dr).Tareacodi == 10) || (helper.Create(dr).Tareacodi == 11))
                    {
                        var entity = helper.Create(dr);

                        entity.Tareanomb = dr.GetString(dr.GetOrdinal("TAREANOMB"));
                        entitys.Add(entity);
                    }


                }
            }

            return entitys;
        }
        public List<EqAreaDTO> ListaSoloAreasActivas(int parametro)
        {
            var entitys = new List<EqAreaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSoloAreasActivas);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    if (helper.Create(dr).Tareacodi == 1)
                    {
                        var entity = helper.Create(dr);

                        entity.Tareanomb = dr.GetString(dr.GetOrdinal("TAREANOMB"));
                        entitys.Add(entity);
                    }

                }
            }

            return entitys;
        }
        #endregion

        #region Intervenciones
        public List<EqAreaDTO> ListarComboUbicacionesXEmpresa(string IdEmpresa)
        {
            List<EqAreaDTO> entitys = new List<EqAreaDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(String.Format(helper.SqlListarComboUbicacionesXEmpresa, IdEmpresa));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqAreaDTO entity = new EqAreaDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areadesc);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region FICHA TÉCNICA

        public List<EqAreaDTO> ListarUbicacionFT()
        {
            List<EqAreaDTO> entitys = new List<EqAreaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarUbicacionFT);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqAreaDTO entity = new EqAreaDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iTareaabrev = dr.GetOrdinal(helper.Tareaabrev);
                    if (!dr.IsDBNull(iTareaabrev)) entity.Tareaabrev = dr.GetString(iTareaabrev);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    int iTareacodi = dr.GetOrdinal(helper.Tareacodi);
                    if (!dr.IsDBNull(iTareacodi)) entity.Tareacodi = Convert.ToInt32(dr.GetValue(iTareacodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        //GESPROTEC - 20241029
        #region GESPROTEC
        public List<EqAreaDTO> ListarUbicacionCoes(string codigoTipoArea, string nombreArea, string programaExistente)
        {
            List<EqAreaDTO> entitys = new List<EqAreaDTO>();

            string sqlQuery = string.Format(helper.SqlListarUbicacionCOES, codigoTipoArea, nombreArea, programaExistente);
                
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqAreaDTO entity = new EqAreaDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iTareaabrev = dr.GetOrdinal(helper.Tareaabrev);
                    if (!dr.IsDBNull(iTareaabrev)) entity.Tareaabrev = dr.GetString(iTareaabrev);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    int iTareacodi = dr.GetOrdinal(helper.Tareacodi);
                    if (!dr.IsDBNull(iTareacodi)) entity.Tareacodi = Convert.ToInt32(dr.GetValue(iTareacodi));

                    int iZona = dr.GetOrdinal(helper.Zona);
                    if (!dr.IsDBNull(iZona)) entity.Zona = Convert.ToString(dr.GetValue(iZona));

                    int iAreanombenprotec = dr.GetOrdinal(helper.Areanombenprotec);
                    if (!dr.IsDBNull(iAreanombenprotec)) entity.Areanombenprotec = Convert.ToString(dr.GetValue(iAreanombenprotec));

                    int iFlagenprotec = dr.GetOrdinal(helper.Flagenprotec);
                    if (!dr.IsDBNull(iFlagenprotec)) entity.Flagenprotec = Convert.ToString(dr.GetValue(iFlagenprotec));

                    int iEpareacodi = dr.GetOrdinal(helper.Epareacodi);
                    if (!dr.IsDBNull(iEpareacodi)) entity.Epareacodi = Convert.ToInt32(dr.GetValue(iEpareacodi));
                    
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqAreaDTO> ListAreaEquipamientoCOES()
        {
            List<EqAreaDTO> entitys = new List<EqAreaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListAreaEquipamientoCOES);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        #endregion
    }
}
