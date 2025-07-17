using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Sic;
using COES.Framework.Base.Tools;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla EQ_EQUIPO
    /// </summary>
    public class EqEquipoRepository : RepositoryBase, IEqEquipoRepository
    {
        public EqEquipoRepository(string strConn) : base(strConn)
        {
        }

        EqEquipoHelper helper = new EqEquipoHelper();

        public int Save(EqEquipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Elecodi, DbType.Int32, entity.Elecodi);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);
            dbProvider.AddInParameter(command, helper.Equiabrev, DbType.String, entity.Equiabrev);
            dbProvider.AddInParameter(command, helper.Equinomb, DbType.String, entity.Equinomb);
            dbProvider.AddInParameter(command, helper.Equiabrev2, DbType.String, entity.Equiabrev2);
            dbProvider.AddInParameter(command, helper.Equitension, DbType.Decimal, entity.Equitension);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre);
            dbProvider.AddInParameter(command, helper.Equipot, DbType.Decimal, entity.Equipot);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            //dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Ecodigo, DbType.String, entity.Ecodigo);
            dbProvider.AddInParameter(command, helper.Equiestado, DbType.String, entity.Equiestado);
            //dbProvider.AddInParameter(command, helper.Osigrupocodi, DbType.String, entity.Osigrupocodi);
            dbProvider.AddInParameter(command, helper.Lastcodi, DbType.Int32, entity.Lastcodi);
            dbProvider.AddInParameter(command, helper.Equifechiniopcom, DbType.DateTime, entity.Equifechiniopcom);
            dbProvider.AddInParameter(command, helper.Equifechfinopcom, DbType.DateTime, entity.Equifechfinopcom);
            dbProvider.AddInParameter(command, helper.Equimaniobra, DbType.String, string.IsNullOrEmpty(entity.EquiManiobra) ? "N" : entity.EquiManiobra);
            //- alpha.HDT - Inicio 23/07/2017: Cambio para atender el requerimiento.
            dbProvider.AddInParameter(command, helper.Osinergcodi, DbType.String, entity.Osinergcodi);
            //- HDT Fin
            dbProvider.AddInParameter(command, helper.Operadoremprcodi, DbType.Int32, entity.Operadoremprcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EqEquipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);


            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Elecodi, DbType.Int32, entity.Elecodi);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);
            dbProvider.AddInParameter(command, helper.Equiabrev, DbType.String, entity.Equiabrev);
            dbProvider.AddInParameter(command, helper.Equinomb, DbType.String, entity.Equinomb);
            dbProvider.AddInParameter(command, helper.Equiabrev2, DbType.String, entity.Equiabrev2);
            dbProvider.AddInParameter(command, helper.Equitension, DbType.Decimal, entity.Equitension);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre);
            dbProvider.AddInParameter(command, helper.Equipot, DbType.Decimal, entity.Equipot);
            //dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            //dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Ecodigo, DbType.String, entity.Ecodigo);
            dbProvider.AddInParameter(command, helper.Equiestado, DbType.String, entity.Equiestado);
            dbProvider.AddInParameter(command, helper.Osinergcodi, DbType.String, entity.Osinergcodi);
            dbProvider.AddInParameter(command, helper.OsinergcodiGen, DbType.String, entity.OsinergcodiGen);        // ticket-6068
            dbProvider.AddInParameter(command, helper.Lastcodi, DbType.Int32, entity.Lastcodi);
            dbProvider.AddInParameter(command, helper.Equifechiniopcom, DbType.DateTime, entity.Equifechiniopcom);
            dbProvider.AddInParameter(command, helper.Equifechfinopcom, DbType.DateTime, entity.Equifechfinopcom);
            dbProvider.AddInParameter(command, helper.Equimaniobra, DbType.String, string.IsNullOrEmpty(entity.EquiManiobra) ? "N" : entity.EquiManiobra);
            dbProvider.AddInParameter(command, helper.UsuarioUpdate, DbType.String, entity.UsuarioUpdate);
            dbProvider.AddInParameter(command, helper.Operadoremprcodi, DbType.Int32, entity.Operadoremprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(int equicodi, string user)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);

            dbProvider.AddInParameter(command, helper.UsuarioUpdate, DbType.String, user);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EqEquipoDTO GetById(int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            EqEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iOperadoremprcodi = dr.GetOrdinal(helper.Operadoremprcodi);
                    if (!dr.IsDBNull(iOperadoremprcodi)) entity.Operadoremprcodi = Convert.ToInt32(dr.GetValue(iOperadoremprcodi));

                    entity.EMPRNOMB = dr.GetString(dr.GetOrdinal("EMPRNOMB"));
                    entity.Emprnomb = entity.EMPRNOMB;
                    entity.AREANOMB = dr.GetString(dr.GetOrdinal("AREANOMB"));
                    entity.Areanomb = entity.AREANOMB;
                    entity.Famnomb = dr.GetString(dr.GetOrdinal("Famnomb"));
                    entity.Famabrev = dr.GetString(dr.GetOrdinal("Famabrev"));
                    entity.TAREAABREV = dr.GetString(dr.GetOrdinal("TAREAABREV"));
                }
            }

            return entity;
        }

        public List<EqEquipoDTO> List()
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
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

        public List<EqEquipoDTO> GetByCriteria()
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
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

        public List<EqEquipoDTO> BasicList()
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetBasicList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oEquipo = new EqEquipoDTO();

                    oEquipo.Equicodi = dr.IsDBNull(dr.GetOrdinal("EQUICODI")) ? -1 : dr.GetInt32(dr.GetOrdinal("EQUICODI"));
                    oEquipo.Equiabrev = dr.IsDBNull(dr.GetOrdinal("EQUIABREV")) ? "" : dr.GetString(dr.GetOrdinal("EQUIABREV"));
                    oEquipo.Equinomb = dr.IsDBNull(dr.GetOrdinal("EQUINOMB")) ? "" : dr.GetString(dr.GetOrdinal("EQUINOMB"));
                    oEquipo.Areacodi = dr.IsDBNull(dr.GetOrdinal("AREACODI")) ? -1 : dr.GetInt32(dr.GetOrdinal("AREACODI"));
                    oEquipo.Emprcodi = dr.IsDBNull(dr.GetOrdinal("EMPRCODI")) ? -1 : dr.GetInt32(dr.GetOrdinal("EMPRCODI"));
                    oEquipo.Famcodi = dr.IsDBNull(dr.GetOrdinal("FAMCODI")) ? -1 : dr.GetInt32(dr.GetOrdinal("FAMCODI"));
                    oEquipo.Equiestado = dr.IsDBNull(dr.GetOrdinal("EQUIESTADO")) ? "B" : dr.GetString(dr.GetOrdinal("EQUIESTADO"));
                    entitys.Add(oEquipo);
                }
            }

            return entitys;
        }
        public List<EqEquipoDTO> ListadoCentralesOsinergmin()
        {
            var entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListadoCentralesOsinergmin);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oEquipo = new EqEquipoDTO();
                    oEquipo.Emprcodi = dr.IsDBNull(dr.GetOrdinal("EMPRCODI")) ? -1 : dr.GetInt32(dr.GetOrdinal("EMPRCODI"));
                    oEquipo.Equifechfinopcom = dr.IsDBNull(dr.GetOrdinal("EQUIFECHFINOPCOM")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("EQUIFECHFINOPCOM"));
                    oEquipo.Equifechiniopcom = dr.IsDBNull(dr.GetOrdinal("EQUIFECHINIOPCOM")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("EQUIFECHINIOPCOM"));
                    oEquipo.Equicodi = dr.IsDBNull(dr.GetOrdinal("Equicodi")) ? -1 : dr.GetInt32(dr.GetOrdinal("Equicodi"));
                    oEquipo.Equinomb = dr.GetString(dr.GetOrdinal("EQUINOMB"));
                    oEquipo.Famcodi = dr.IsDBNull(dr.GetOrdinal("FAMCODI")) ? -1 : dr.GetInt32(dr.GetOrdinal("FAMCODI"));
                    oEquipo.Grupocodi = dr.IsDBNull(dr.GetOrdinal("GRUPOCODI")) ? -1 : dr.GetInt32(dr.GetOrdinal("GRUPOCODI"));
                    entitys.Add(oEquipo);
                }
            }
            return entitys;
        }


        /// <summary>
        /// Listado de Equipos por filtro de Empresa, Familia, Tipo Empresa y Estado Equipo
        /// </summary>
        /// <param name="idEmpresa">Código Empresa</param>
        /// <param name="sEstado">Estado de Equipo</param>
        /// <param name="idTipoEquipo">Código Familia</param>
        /// <param name="idTipoEmpresa">Código Tipo Empresa</param>
        /// <param name="sNombreEquipo">Nombre de Equipo</param>
        /// <param name="idPadre">Código Padre Equipo * Usar -99 para evitar este filtro*</param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarEquiposxFiltro(int idEmpresa, string sEstado, int idTipoEquipo, int idTipoEmpresa, string sNombreEquipo, int idPadre)
        {
            var entitys = new List<EqEquipoDTO>();
            string strComando = string.Format(helper.SQLListarEquiposxFiltro, idEmpresa, idTipoEquipo, idTipoEmpresa, sNombreEquipo.ToUpperInvariant(), idPadre);

            DbCommand command = dbProvider.GetSqlStringCommand(strComando);
            //dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, idEmpresa);
            //dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, idTipoEquipo);
            //dbProvider.AddInParameter(command, "TIPOEMPRCODI", DbType.Int32, idTipoEmpresa);
            //dbProvider.AddInParameter(command, helper.Equinomb, DbType.String, sNombreEquipo);
            //dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, idPadre);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oEquipo = new EqEquipoDTO();
                    oEquipo = helper.Create(dr);
                    int iOperadoremprcodi = dr.GetOrdinal(helper.Operadoremprcodi);
                    if (!dr.IsDBNull(iOperadoremprcodi)) oEquipo.Operadoremprcodi = Convert.ToInt32(dr.GetValue(iOperadoremprcodi));

                    oEquipo.EMPRNOMB = dr.GetString(dr.GetOrdinal("EMPRNOMB"));
                    oEquipo.AREANOMB = dr.GetString(dr.GetOrdinal("AREANOMB"));
                    oEquipo.Famnomb = dr.GetString(dr.GetOrdinal("Famnomb"));

                    entitys.Add(oEquipo);
                }
            }
            entitys = sEstado == "AF" ? entitys.Where(eq => eq.Equiestado == "A" || eq.Equiestado == "F").ToList() : entitys.Where(eq => eq.Equiestado == sEstado).ToList();
            return entitys;
        }

        /// <summary>
        /// Listado de Equipos filtrado por nombre.
        /// Datos solo de tabla Equipo
        /// </summary>
        /// <param name="coincidencia">Nombre del Equipo</param>
        /// <param name="nroPagina">N° de página</param>
        /// <param name="nroFilas">N° de Filas</param>
        /// <returns></returns>
        public List<EqEquipoDTO> BuscarEquipoxNombre(string coincidencia, int nroPagina, int nroFilas)
        {
            var entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlBuscarEquipoPorNombrePaginado);
            dbProvider.AddInParameter(command, helper.Equinomb, DbType.String, coincidencia);
            dbProvider.AddInParameter(command, "nropaginas", DbType.Int32, nroPagina);
            dbProvider.AddInParameter(command, "nroFilas", DbType.Int32, nroFilas);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oEquipo = new EqEquipoDTO();
                    oEquipo = helper.Create(dr);
                    entitys.Add(oEquipo);
                }
            }
            return entitys;
        }

        /// <summary>
        /// Listado de Unidades filtradas para el ensayo        
        /// </summary>
        /// <param name="idEnsayo">codigo del ensayo</param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarUnidadesxEnsayo(int idEnsayo)
        {


            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string query = string.Format(helper.SqlListarUnidadesxEnsayo, idEnsayo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

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
        /// Listado de Equipos filtrado por varias familias.
        /// Datos de Equipo, Familia, Empresa y Area
        /// </summary>
        /// <param name="iCodFamilias">Código de Familias</param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarEquipoxFamilias(params int[] iCodFamilias)
        {
            var entitys = new List<EqEquipoDTO>();
            if (iCodFamilias.Length > 0)
            {
                string sCodFamilias = string.Empty;
                foreach (var ifamcodi in iCodFamilias)
                {
                    sCodFamilias = sCodFamilias + "," + ifamcodi;
                }
                sCodFamilias = sCodFamilias.Substring(1);

                DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlEquiposxFamilias, sCodFamilias));
                //dbProvider.AddInParameter(command, helper.Famcodi, DbType.String, sCodFamilias);
                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        var oEquipo = new EqEquipoDTO();
                        oEquipo = helper.Create(dr);
                        oEquipo.EMPRNOMB = dr.GetString(dr.GetOrdinal("EMPRNOMB"));
                        oEquipo.Emprnomb = oEquipo.EMPRNOMB;
                        oEquipo.AREANOMB = dr.GetString(dr.GetOrdinal("AREANOMB"));
                        oEquipo.Areanomb = oEquipo.AREANOMB;
                        oEquipo.Famnomb = dr.GetString(dr.GetOrdinal("Famnomb"));

                        entitys.Add(oEquipo);
                    }
                }

            }
            return entitys;
        }

        /// <summary>
        /// Listado de Equipos filtrado por varias familias.
        /// Datos de Equipo, Familia, Empresa y Area
        /// </summary>
        /// <param name="iCodFamilias">Código de Familias</param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarEquipoxFamiliasActivosyProyectos(params int[] iCodFamilias)
        {
            var entitys = new List<EqEquipoDTO>();
            if (iCodFamilias.Length > 0)
            {
                string sCodFamilias = string.Empty;
                foreach (var ifamcodi in iCodFamilias)
                {
                    sCodFamilias = sCodFamilias + "," + ifamcodi;
                }
                sCodFamilias = sCodFamilias.Substring(1);

                DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlListarEquipoxFamiliasActivosyProyectos, sCodFamilias));
                //dbProvider.AddInParameter(command, helper.Famcodi, DbType.String, sCodFamilias);
                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        var oEquipo = new EqEquipoDTO();
                        oEquipo = helper.Create(dr);
                        oEquipo.EMPRNOMB = dr.GetString(dr.GetOrdinal("EMPRNOMB"));
                        oEquipo.Emprnomb = oEquipo.EMPRNOMB;
                        oEquipo.AREANOMB = dr.GetString(dr.GetOrdinal("AREANOMB"));
                        oEquipo.Areanomb = oEquipo.AREANOMB;
                        oEquipo.Famnomb = dr.GetString(dr.GetOrdinal("Famnomb"));

                        entitys.Add(oEquipo);
                    }
                }

            }
            return entitys;
        }
        public List<EqEquipoDTO> ListarEquipoxFamiliasEmpresas(int[] iCodFamilias, int[] iEmpresas)
        {
            var entitys = new List<EqEquipoDTO>();
            if (iCodFamilias.Length > 0)
            {
                string sCodFamilias = string.Empty;
                string sCodEmpresas = string.Empty;
                foreach (var ifamcodi in iCodFamilias)
                {
                    sCodFamilias = sCodFamilias + "," + ifamcodi;
                }
                sCodFamilias = sCodFamilias.Substring(1);
                foreach (var iEmpresa in iEmpresas)
                {
                    sCodEmpresas = sCodEmpresas + "," + iEmpresa;
                }
                sCodEmpresas = sCodEmpresas.Substring(1);

                DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlEquiposxFamiliasEmpresas, sCodFamilias, sCodEmpresas));

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        var oEquipo = new EqEquipoDTO();
                        oEquipo = helper.Create(dr);
                        oEquipo.EMPRNOMB = dr.GetString(dr.GetOrdinal("EMPRNOMB"));
                        oEquipo.AREANOMB = dr.GetString(dr.GetOrdinal("AREANOMB"));
                        oEquipo.Famnomb = dr.GetString(dr.GetOrdinal("Famnomb"));

                        entitys.Add(oEquipo);
                    }
                }

            }
            return entitys;
        }
        public List<EqEquipoDTO> ListarEquiposxFiltroPaginado(int idEmpresa, string sEstado, int idTipoEquipo, int idTipoEmpresa,
            string sNombreEquipo, int idPadre, int nroPagina, int nroFilas, ref int totalPaginas, ref int totalRegistros)
        {
            var resultadoTotal = this.ListarEquiposxFiltro(idEmpresa, sEstado, idTipoEquipo, idTipoEmpresa, sNombreEquipo, idPadre);
            totalRegistros = resultadoTotal.Count();
            totalPaginas = (int)Math.Ceiling((float)totalRegistros / (float)nroFilas);
            int pageIndex = nroPagina - 1;
            var lsEquipos = resultadoTotal.Skip(pageIndex * nroFilas).Take(nroFilas).ToList();
            return lsEquipos;
        }


        public EqEquipoDTO ObtenerDetalleEquipo(int idEquipo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerDetalleEquipo);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, idEquipo);

            EqEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new EqEquipoDTO();

                    int iEmprNomb = dr.GetOrdinal(this.helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EMPRNOMB = dr.GetString(iEmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    int iTareaAbrev = dr.GetOrdinal(this.helper.TAREAABREV);
                    if (!dr.IsDBNull(iTareaAbrev)) entity.TAREAABREV = dr.GetString(iTareaAbrev);

                    int iAreaNomb = dr.GetOrdinal(this.helper.AREANOMB);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AREANOMB = dr.GetString(iAreaNomb);

                    int iFamAbrev = dr.GetOrdinal(this.helper.FAMABREV);
                    if (!dr.IsDBNull(iFamAbrev)) entity.FAMABREV = dr.GetString(iFamAbrev);

                    int IFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(IFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(IFamcodi));

                    int iDesCentral = dr.GetOrdinal(this.helper.DESCENTRAL);
                    if (!dr.IsDBNull(iDesCentral)) entity.DESCENTRAL = dr.GetString(iDesCentral);

                    int iEquiAbrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.Equiabrev = dr.GetString(iEquiAbrev);

                    int iEquitension = dr.GetOrdinal(this.helper.Equitension);
                    if (!dr.IsDBNull(iEquitension)) entity.Equitension = dr.GetDecimal(iEquitension);

                    int iEquiCodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquiCodi)) entity.Equicodi = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    int iEquiNomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.Equinomb = dr.GetString(iEquiNomb);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                }
            }

            return entity;
        }

        public List<EqEquipoDTO> ObtenerEquipoPorAreaEmpresa(int idEmpresa, int idArea)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();

            string sql = string.Format(helper.SqlObtenerEquipoPorAreaEmpresa, idEmpresa, idArea);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamAbrev = dr.GetOrdinal(this.helper.FAMABREV);
                    if (!dr.IsDBNull(iFamAbrev)) entity.Famabrev = dr.GetString(iFamAbrev);

                    int iEquiCodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquiCodi)) entity.Equicodi = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    int iEquiNomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.Equinomb = dr.GetString(iEquiNomb);

                    int iEquiestado = dr.GetOrdinal(helper.Equiestado);
                    if (!dr.IsDBNull(iEquiestado)) entity.Equiestado = dr.GetString(iEquiestado);

                    int iEmprNomb = dr.GetOrdinal(this.helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    entity.Valor = string.Format("{0} - {1} ({2})", entity.Famabrev, entity.Equiabrev, entity.Equicodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListarCentralesXCombustible(int emprcodi, int tipocombustible)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string query = string.Format(helper.SqlCentralesXCombustible, tipocombustible, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListarCentralesXEmpresaGEN(string emprcodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string query = string.Format(helper.SqlCentralesXEmpresaGEN, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListarCentralesXEmpresaGEN2(string emprcodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string query = string.Format(helper.SqlCentralesXEmpresaGEN2, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListarEquiposEnsayo(string equicodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string query = string.Format(helper.SqlListaEquiposEnsayo, equicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

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
        /// Listado que Equipos para el nuevo módulo de Equipamiento
        /// </summary>
        /// <param name="iEmpresa">Códgo de Empresa</param>
        /// <param name="iFamilia">Código de Familia</param>
        /// <param name="iTipoEmpresa">Código de Tipo Empresa</param>
        /// <param name="iEquipo">Código de Equipo</param>
        /// <param name="sEstado">Estado de equipo</param>
        /// <param name="nroPagina">Página</param>
        /// <param name="nroFilas">Tamaño de página</param>
        /// <returns>Listado de Equipos</returns>
        public List<EqEquipoDTO> ListaEquipamientoPaginado(int iEmpresa, int iFamilia, int iTipoEmpresa, int iEquipo, string sEstado, string nombre, int nroPagina, int nroFilas)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            nombre = nombre == null ? string.Empty : nombre.ToLowerInvariant();
            string query = string.Format(helper.SqlListadoEquipamientoPaginado, iEmpresa, iFamilia, iTipoEmpresa, iEquipo, sEstado, nombre, nroPagina, nroFilas);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO oEquipo;
                    oEquipo = helper.Create(dr);
                    int iOperadoremprcodi = dr.GetOrdinal(helper.Operadoremprcodi);
                    if (!dr.IsDBNull(iOperadoremprcodi)) oEquipo.Operadoremprcodi = Convert.ToInt32(dr.GetValue(iOperadoremprcodi));

                    oEquipo.EMPRNOMB = dr.GetString(dr.GetOrdinal("EMPRNOMB"));
                    oEquipo.AREANOMB = dr.GetString(dr.GetOrdinal("AREANOMB"));
                    oEquipo.Emprnomb = dr.GetString(dr.GetOrdinal(helper.EMPRNOMB));
                    oEquipo.Areanomb = dr.GetString(dr.GetOrdinal(helper.AREANOMB));
                    oEquipo.Famnomb = dr.GetString(dr.GetOrdinal("FAMNOMB"));
                    oEquipo.Famabrev = dr.GetString(dr.GetOrdinal(helper.FAMABREV));
                    oEquipo.TipoEmpresa = dr.GetString(dr.GetOrdinal("tipoemprdesc"));
                    entitys.Add(oEquipo);
                }
            }

            return entitys;
        }

        public int TotalEquipamiento(int iEmpresa, int iFamilia, int iTipoEmpresa, int iEquipo, string sEstado, string nombre)
        {
            nombre = nombre == null ? string.Empty : nombre.ToLowerInvariant();
            String query = String.Format(helper.SqlTotalListadoEquipamiento, iEmpresa, iFamilia, iTipoEmpresa, iEquipo, sEstado, nombre);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }


        public List<EqEquipoDTO> ObtenerEquiposProcManiobras(int famCodi, int propcodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEquiposProcManiobras);
            dbProvider.AddInParameter(command, helper.Propcodi, DbType.Int32, propcodi);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, famCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.EMPRNOMB = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iAreanomb)) entity.AREANOMB = dr.GetString(iAreanomb);

                    int iFamabrev = dr.GetOrdinal(helper.FAMABREV);
                    if (!dr.IsDBNull(iFamabrev)) entity.FAMABREV = dr.GetString(iFamabrev);

                    int iTareaabrev = dr.GetOrdinal(helper.TAREAABREV);
                    if (!dr.IsDBNull(iTareaabrev)) entity.TAREAABREV = dr.GetString(iTareaabrev);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int IFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(IFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(IFamcodi));

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iUrlmaniobra = dr.GetOrdinal(helper.Urlmaniobra);
                    if (!dr.IsDBNull(iUrlmaniobra)) entity.Urlmaniobra = dr.GetString(iUrlmaniobra);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListaRecursosxCuenca(string empresas, string cuencas, string recursos)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string query = string.Format(helper.SqlListaRecursosxCuenca, empresas, cuencas, recursos);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oEquipo = new EqEquipoDTO();
                    oEquipo = helper.Create(dr);
                    if (oEquipo.Famcodi == 43)
                    {
                        oEquipo.Equinomb = "Río : " + dr.GetString(dr.GetOrdinal("Cuenca")) + " -- Estación: " + dr.GetString(dr.GetOrdinal("Equinomb"));
                        oEquipo.Cuencanomb = dr.GetString(dr.GetOrdinal("Cuenca2"));
                    }
                    else
                    {

                        oEquipo.Cuencanomb = dr.GetString(dr.GetOrdinal("Cuenca"));
                    }
                    oEquipo.Emprnomb = dr.GetString(dr.GetOrdinal("Emprnomb"));
                    oEquipo.Famnomb = dr.GetString(dr.GetOrdinal("Famabrev"));

                    entitys.Add(oEquipo);
                }
            }
            return entitys;
        }

        public EqEquipoDTO GetByIdEquipo(int idEquipo)
        {
            string queryString = string.Format(helper.SqlGetByIdEquipo, idEquipo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            EqEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EqEquipoDTO> ListByIdEquipo(string idEquipo)
        {
            string queryString = string.Format(helper.SqlListByIdEquipo, idEquipo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            EqEquipoDTO entity = new EqEquipoDTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = dr.GetInt32(iFenergcodi);

                    int iTgenercodi = dr.GetOrdinal(helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = dr.GetInt32(iTgenercodi);

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iGrupotipocogen = dr.GetOrdinal(helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    int iEmprnomb = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFamAbrev = dr.GetOrdinal(this.helper.FAMABREV);
                    if (!dr.IsDBNull(iFamAbrev)) entity.Famabrev = dr.GetString(iFamAbrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> GetByEmprFam(int emprcodi, int famcodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, famcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ObtenerEquipoPorFamilia(int emprcodi, int famcodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEquiposPorFamilia);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, famcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iAreanomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iAreanomb)) entity.AREANOMB = dr.GetString(iAreanomb);

                    int iTareaabrev = dr.GetOrdinal(helper.TAREAABREV);
                    if (!dr.IsDBNull(iTareaabrev)) entity.TAREAABREV = dr.GetString(iTareaabrev);

                    entity.AREANOMB = entity.TAREAABREV + " " + entity.AREANOMB;

                    if (string.IsNullOrEmpty(entity.Equinomb)) entity.Equinomb = entity.Equiabrev;

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iEmprnomb = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Método que retorna el listado de Generadores térmicos que intervienen en un modo de operación
        /// </summary>
        /// <param name="grupocodi">Código del modo de operación</param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarGeneradoresTermicosPorModoOperacion(int grupocodi)
        {
            var entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlGeneradoresPorModoOperacionCombinado, grupocodi));
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            if (entitys.Count > 0)
                return entitys;

            command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlGeneradoresPorModoOperacionSimple, grupocodi));
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }
        public List<EqEquipoDTO> List(int minRowToFetch, int maxRowToFetch, EqFamiliaDTO eqFamiliaDto, EqEquipoDTO eqEquipoDto)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, EqFamiliaHelper.Nomb, DbType.String, string.IsNullOrEmpty(eqFamiliaDto.Famnomb) ? string.Empty : eqFamiliaDto.Famnomb);
            dbProvider.AddInParameter(command, helper.Equinomb, DbType.String, string.IsNullOrEmpty(eqEquipoDto.Equinomb) ? string.Empty : eqEquipoDto.Equinomb);
            dbProvider.AddInParameter(command, EqEquipoHelper.MaxRowToFetch, DbType.Int32, maxRowToFetch);
            dbProvider.AddInParameter(command, EqEquipoHelper.MinRowToFetch, DbType.Int32, minRowToFetch);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = helper.Create(dr);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public int GetTotalRecords(EqFamiliaDTO eqFamiliaDto, EqEquipoDTO eqEquipoDto)
        {
            var command = dbProvider.GetSqlStringCommand(helper.SqlTotalRecords);
            dbProvider.AddInParameter(command, EqFamiliaHelper.Nomb, DbType.String, string.IsNullOrEmpty(eqFamiliaDto.Famnomb) ? string.Empty : eqFamiliaDto.Famnomb);
            dbProvider.AddInParameter(command, helper.Equinomb, DbType.String, string.IsNullOrEmpty(eqEquipoDto.Equinomb) ? string.Empty : eqEquipoDto.Equinomb);
            var result = dbProvider.ExecuteScalar(command);
            return (result != null ? Convert.ToInt32(result) : 0);
        }
        public void GetAll(EqFamiliaDTO eqFamiliaDto, out List<string[]> registros, out Dictionary<int, MetadataDTO> metaDatosDictionary)
        {
            registros = new List<string[]>();
            metaDatosDictionary = new Dictionary<int, MetadataDTO>();
            var maxRowToFetch = GetTotalRecords(eqFamiliaDto, new EqEquipoDTO());

            var command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Equinomb, DbType.String, string.Empty);
            dbProvider.AddInParameter(command, EqEquipoHelper.MaxRowToFetch, DbType.Int32, maxRowToFetch);
            dbProvider.AddInParameter(command, EqEquipoHelper.MinRowToFetch, DbType.Int32, 1);

            using (var dataReader = dbProvider.ExecuteReader(command))
            {

                helper.ObtenerMetaDatos(dataReader, ref metaDatosDictionary);

                while (dataReader.Read())
                {
                    var objeto = new object[dataReader.FieldCount];
                    var conteo = dataReader.GetValues(objeto);
                    var valores = new string[conteo];

                    for (int i = 0; i < conteo; i++)
                    {
                        var valor = objeto[i].ToString();
                        valores[i] = valor;
                    }

                    registros.Add(valores);
                }
            }
        }

        public List<DetalleReportePotenciaDTO> DatosReportePotenciaEfectivaHidraulicas(int iEmpresa, int iCentral, DateTime fechaIni, DateTime fechaFin)
        {
            List<DetalleReportePotenciaDTO> resultado = new List<DetalleReportePotenciaDTO>();
            string sConsultaFechas = "";
            var lPeriodos = EPDate.GetPeriodos(fechaIni, fechaFin);
            int iCantPeriodos = 0;
            foreach (var oPeriodo in lPeriodos)
            {
                sConsultaFechas = sConsultaFechas + ",fn_sdatopropiedadfechas(eqp.equicodi,164,to_date('" + oPeriodo.FechaInicio.ToString("YYYY-MM-DD") + "','YYYY-MM-DD'),to_date('" + oPeriodo.FechaFin.ToString("YYYY-MM-DD") + "','YYYY-MM-DD'))as pe" + iCantPeriodos;
                iCantPeriodos++;
            }
            string sQuery = string.Format(helper.SqlReportePotenciaEfectivaHidraulicas, iEmpresa, iCentral, sConsultaFechas);
            var command = dbProvider.GetSqlStringCommand(sQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DetalleReportePotenciaDTO oAux = new DetalleReportePotenciaDTO();
                    oAux.Central = dr.GetString(dr.GetOrdinal("Central"));
                    oAux.Empresa = dr.GetString(dr.GetOrdinal("emprnomb"));
                    oAux.GeneradorModo = dr.GetString(dr.GetOrdinal("equinomb"));
                    oAux.Potencia = new List<decimal?>();
                    for (int i = 0; i < lPeriodos.Count; i++)
                    {
                        oAux.Potencia.Add(dr.GetDecimal(dr.GetOrdinal("pe" + i)));
                    }

                    resultado.Add(oAux);
                }
            }
            return resultado;
        }

        public List<DetalleReportePotenciaDTO> DatosReportePotenciaEfectivaSolares(int iEmpresa, int iCentral, DateTime fechaIni, DateTime fechaFin)
        {
            List<DetalleReportePotenciaDTO> resultado = new List<DetalleReportePotenciaDTO>();
            string sConsultaFechas = "";
            var lPeriodos = EPDate.GetPeriodos(fechaIni, fechaFin);
            int iCantPeriodos = 0;
            foreach (var oPeriodo in lPeriodos)
            {
                sConsultaFechas = sConsultaFechas + ",fn_sdatopropiedadfechas(eqp.equicodi,1710,to_date('" + oPeriodo.FechaInicio.ToString("YYYY-MM-DD") + "','YYYY-MM-DD'),to_date('" + oPeriodo.FechaFin.ToString("YYYY-MM-DD") + "','YYYY-MM-DD'))as pe" + iCantPeriodos;
                iCantPeriodos++;
            }
            string sQuery = string.Format(helper.SqlReportePotenciaEfectivaSolares, iEmpresa, iCentral, sConsultaFechas);
            var command = dbProvider.GetSqlStringCommand(sQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DetalleReportePotenciaDTO oAux = new DetalleReportePotenciaDTO();
                    oAux.Central = dr.GetString(dr.GetOrdinal("Central"));
                    oAux.Empresa = dr.GetString(dr.GetOrdinal("emprnomb"));
                    oAux.GeneradorModo = dr.GetString(dr.GetOrdinal("equinomb"));
                    oAux.Potencia = new List<decimal?>();
                    for (int i = 0; i < lPeriodos.Count; i++)
                    {
                        oAux.Potencia.Add(dr.GetDecimal(dr.GetOrdinal("pe" + i)));
                    }
                    resultado.Add(oAux);
                }
            }
            return resultado;
        }

        public List<EqEquipoDTO> ObtenerPorPadre(int? idEquipo)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPorPadre);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, idEquipo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEquiestado = dr.GetOrdinal(helper.Equiestado);
                    if (!dr.IsDBNull(iEquiestado)) entity.Equiestado = dr.GetString(iEquiestado);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<DetalleReportePotenciaDTO> DatosReportePotenciaEfectivaEolicas(int iEmpresa, int iCentral, DateTime fechaIni, DateTime fechaFin)
        {
            List<DetalleReportePotenciaDTO> resultado = new List<DetalleReportePotenciaDTO>();
            string sConsultaFechas = "";
            var lPeriodos = EPDate.GetPeriodos(fechaIni, fechaFin);
            int iCantPeriodos = 0;
            foreach (var oPeriodo in lPeriodos)
            {
                sConsultaFechas = sConsultaFechas + ",fn_sdatopropiedadfechas(eqp.equicodi,1602,to_date('" + oPeriodo.FechaInicio.ToString("YYYY-MM-DD") + "','YYYY-MM-DD'),to_date('" + oPeriodo.FechaFin.ToString("YYYY-MM-DD") + "','YYYY-MM-DD'))as pe" + iCantPeriodos;
                iCantPeriodos++;
            }
            string sQuery = string.Format(helper.SqlReportePotenciaEfectivaEolicas, iEmpresa, iCentral, sConsultaFechas);
            var command = dbProvider.GetSqlStringCommand(sQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DetalleReportePotenciaDTO oAux = new DetalleReportePotenciaDTO();
                    oAux.Central = dr.GetString(dr.GetOrdinal("Central"));
                    oAux.Empresa = dr.GetString(dr.GetOrdinal("emprnomb"));
                    oAux.GeneradorModo = dr.GetString(dr.GetOrdinal("equinomb"));
                    oAux.Potencia = new List<decimal?>();
                    for (int i = 0; i < lPeriodos.Count; i++)
                    {
                        oAux.Potencia.Add(dr.GetDecimal(dr.GetOrdinal("pe" + i)));
                    }
                    resultado.Add(oAux);
                }
            }
            return resultado;
        }


        public List<EqEquipoDTO> ListarEquiposPorFamilia(string idFamilia)
        {
            var entitys = new List<EqEquipoDTO>();
            string query = string.Format(helper.SqlEquiposXFamilia, idFamilia);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = helper.Create(dr);
                    int iosinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iosinergcodi)) entity.Osinergcodi = Convert.ToString(dr.GetValue(iosinergcodi));
                    int iOsinergcodiDespacho = dr.GetOrdinal(helper.OsinergcodiDespacho);
                    if (!dr.IsDBNull(iOsinergcodiDespacho)) entity.OsinergcodiDespacho = Convert.ToString(dr.GetValue(iOsinergcodiDespacho));

                    int iTgenercodi = dr.GetOrdinal(this.helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt16(dr.GetValue(iTgenercodi));
                    int iTgenernomb = dr.GetOrdinal(this.helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iFenergcodi = dr.GetOrdinal(this.helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));
                    int iFenergnomb = dr.GetOrdinal(this.helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iTipogenerrer = dr.GetOrdinal(this.helper.Tipogenerrer);
                    if (!dr.IsDBNull(iTipogenerrer)) entity.Tipogenerrer = dr.GetString(iTipogenerrer);

                    int iGrupotipocogen = dr.GetOrdinal(helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    int iEmprnomb = dr.GetOrdinal(this.helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprabrev = dr.GetOrdinal(this.helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iTipoemprcodi = dr.GetOrdinal(this.helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    int iAreanomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iFamAbrev = dr.GetOrdinal(this.helper.FAMABREV);
                    if (!dr.IsDBNull(iFamAbrev)) entity.Famabrev = dr.GetString(iFamAbrev);

                    int iTareaAbrev = dr.GetOrdinal(this.helper.TAREAABREV);
                    if (!dr.IsDBNull(iTareaAbrev)) entity.Tareaabrev = dr.GetString(iTareaAbrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> UpdateOsinergminCodigo(int? equicodi, string osinergmincodi)
        {
            var entitys = new List<EqEquipoDTO>();
            string query = string.Format(helper.SqlUpdateOsinergminCodigo, equicodi, osinergmincodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = helper.Create(dr);
                    int iosinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iosinergcodi)) entity.Osinergcodi = Convert.ToString(dr.GetValue(iosinergcodi));

                    int iEquiCodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquiCodi)) entity.Equicodi = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }



        public void UpdateOsinergmin(EqEquipoDTO equipoCOES)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.UpdateOsinergmin);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, equipoCOES.Emprcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, equipoCOES.Grupocodi);
            dbProvider.AddInParameter(command, helper.Elecodi, DbType.Int32, equipoCOES.Elecodi);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, equipoCOES.Areacodi);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, equipoCOES.Famcodi);
            dbProvider.AddInParameter(command, helper.Equiabrev, DbType.String, equipoCOES.Equiabrev);
            dbProvider.AddInParameter(command, helper.Equinomb, DbType.String, equipoCOES.Equinomb);
            dbProvider.AddInParameter(command, helper.Equiabrev2, DbType.String, equipoCOES.Equiabrev2);
            dbProvider.AddInParameter(command, helper.Equitension, DbType.Decimal, equipoCOES.Equitension);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, equipoCOES.Equipadre);
            dbProvider.AddInParameter(command, helper.Equipot, DbType.Decimal, equipoCOES.Equipot);
            dbProvider.AddInParameter(command, helper.Ecodigo, DbType.String, equipoCOES.Ecodigo);
            dbProvider.AddInParameter(command, helper.Equiestado, DbType.String, equipoCOES.Equiestado);
            dbProvider.AddInParameter(command, helper.Osigrupocodi, DbType.String, equipoCOES.Osigrupocodi);
            dbProvider.AddInParameter(command, helper.Lastcodi, DbType.Int32, equipoCOES.Lastcodi);
            dbProvider.AddInParameter(command, helper.Equifechiniopcom, DbType.DateTime, equipoCOES.Equifechiniopcom);
            dbProvider.AddInParameter(command, helper.Equifechfinopcom, DbType.DateTime, equipoCOES.Equifechfinopcom);
            dbProvider.AddInParameter(command, helper.Equimaniobra, DbType.String, string.IsNullOrEmpty(equipoCOES.EquiManiobra) ? "N" : equipoCOES.EquiManiobra);
            dbProvider.AddInParameter(command, helper.UsuarioUpdate, DbType.String, equipoCOES.UsuarioUpdate);
            dbProvider.AddInParameter(command, helper.Osinergcodi, DbType.String, equipoCOES.Osinergcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equipoCOES.Equicodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public EqEquipoDTO GetByIdOsinergmin(int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByOsinergmin);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            EqEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprNomb = dr.GetOrdinal("EMPRNOMB");
                    if (!dr.IsDBNull(iEmprNomb)) entity.EMPRNOMB = dr.GetString(iEmprNomb);

                    int iAreaNomb = dr.GetOrdinal("AREANOMB");
                    if (!dr.IsDBNull(iAreaNomb)) entity.AREANOMB = dr.GetString(iAreaNomb);

                    int iFamnomb = dr.GetOrdinal("Famnomb");
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(dr.GetOrdinal("Famnomb"));

                    int iOsinergcodi = dr.GetOrdinal(this.helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);
                }
            }

            return entity;
        }

        public EqEquipoDTO GetByCodOsinergmin(string codOsinergmin)
        {
            string queryString = string.Format(helper.SqlGetByCodOsinergmin, codOsinergmin);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            EqEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);
                }
            }
            return entity;
        }

        public List<EqEquipoDTO> ListarEquiposPropiedadesAGC(string fecha)
        {
            var entitys = new List<EqEquipoDTO>();

            string query = string.Format(helper.SqlObtenerEquipoPropiedadAGC, fecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iEmprnomb = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iB2 = dr.GetOrdinal(helper.B2);
                    if (!dr.IsDBNull(iB2)) entity.B2 = dr.GetString(iB2);

                    int iB3 = dr.GetOrdinal(helper.B3);
                    if (!dr.IsDBNull(iB3)) entity.B3 = dr.GetString(iB3);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListarEquiposAGC()
        {
            var entitys = new List<EqEquipoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaEquipoAGC);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<EqEquipoDTO> ListadoEquipoNombre(string famcodis)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string sql = string.Format(helper.SqlListaEquipoNombre, famcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);


                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<EqEquipoDTO> ListarEquiposPropiedades(int propcodi, DateTime fecha, int emprCodi, int areacodi, int famcodi, int nroPage, int pageSize)
        {

            string fechaConsulta = fecha.ToString(ConstantesBase.FormatoFecha);

            var entitys = new List<EqEquipoDTO>();

            string query = string.Format(helper.SqlObtenerEquipoPropiedad, propcodi, fechaConsulta, emprCodi, areacodi, famcodi, nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEmprnomb = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquinombre = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinombre)) entity.Equinomb = dr.GetString(iEquinombre);

                    int iB2 = dr.GetOrdinal(helper.B2);
                    if (!dr.IsDBNull(iB2)) entity.B2 = dr.GetString(iB2);

                    int iFecha = dr.GetOrdinal(helper.FechaUpdate);
                    if (!dr.IsDBNull(iFecha)) entity.Lastdate = dr.IsDBNull(iFecha) ? (DateTime?)null : dr.GetDateTime(iFecha);

                    int iEquiestado = dr.GetOrdinal(helper.Equiestado);
                    if (!dr.IsDBNull(iEquiestado)) entity.Equiestado = dr.GetString(iEquiestado);

                    int iPropcodi = dr.GetOrdinal(helper.Propcodi);
                    if (!dr.IsDBNull(iPropcodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iPropcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla EQ_EQUIPO
        /// </summary>
        public int TotalEquiposPropiedades(int emprCodi, int areacodi, int famCodi)
        {
            String sql = String.Format(this.helper.SqlTotalEquipoPropiedad, emprCodi, areacodi, famCodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }


        public List<EqEquipoDTO> ListarEquiposPrGrupo(string grupoCodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();

            string query = string.Format(helper.SqlListaEquipoPrGrupo, grupoCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> CentralesXEmpresaHorasOperacion(int emprCodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string query = string.Format(helper.SqlCentralesXEmpresaHorasOperacion, emprCodi, DateTime.Today.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oEquipo = new EqEquipoDTO();
                    oEquipo = helper.Create(dr);
                    oEquipo.Famnomb = dr.GetString(dr.GetOrdinal(helper.Famnomb));
                    oEquipo.Famabrev = dr.GetString(dr.GetOrdinal(helper.FAMABREV));
                    oEquipo.Famcodipadre = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("Famcodipadre")));
                    oEquipo.Nombrecentral = dr.GetString(dr.GetOrdinal("Nombrecentral"));
                    oEquipo.Codipadre = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("Codipadre")));


                    entitys.Add(oEquipo);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> CentralesXEmpresaHorasOperacion(int emprCodi, string fecha)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string query = string.Format(helper.SqlCentralesXEmpresaHorasOperacion, emprCodi, fecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oEquipo = new EqEquipoDTO();
                    oEquipo = helper.Create(dr);
                    oEquipo.Famnomb = dr.GetString(dr.GetOrdinal(helper.Famnomb));
                    oEquipo.Famabrev = dr.GetString(dr.GetOrdinal(helper.FAMABREV));
                    oEquipo.Famcodipadre = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("Famcodipadre")));
                    oEquipo.Nombrecentral = dr.GetString(dr.GetOrdinal("Nombrecentral"));
                    oEquipo.Codipadre = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("Codipadre")));


                    entitys.Add(oEquipo);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListarCentralesXEmpresaXFamiliaGEN(string emprCodi, string sFamCodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string query = string.Format(helper.SqlCentralesXEmpresaXFamiliaGEN, emprCodi, sFamCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oEquipo = new EqEquipoDTO();
                    oEquipo = helper.Create(dr);
                    oEquipo.Famnomb = dr.GetString(dr.GetOrdinal(helper.Famnomb));
                    entitys.Add(oEquipo);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListadoXEmpresa(int emprCodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string query = string.Format(helper.SqlListadoXEmpresa, emprCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oEquipo = new EqEquipoDTO();
                    oEquipo = helper.Create(dr);
                    oEquipo.Famnomb = dr.GetString(dr.GetOrdinal(helper.Famnomb));
                    oEquipo.Famabrev = dr.GetString(dr.GetOrdinal(helper.FAMABREV));
                    entitys.Add(oEquipo);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> GetByEmprFam2(int grupoCodi, int famcodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string query = string.Format(helper.SqlGetByEmprFam2, grupoCodi, famcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public List<EqEquipoDTO> ListarCentralesXEmpresaXFamiliaGEN2(string sEmprCodi, string sFamCodi, string equiestado)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string query = string.Format(helper.SqlCentralesXEmpresaXFamiliaGEN2, sEmprCodi, sFamCodi, equiestado);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iFenergcodi = dr.GetOrdinal(this.helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = dr.GetInt32(iFenergcodi);

                    int iGruporeservafria = dr.GetOrdinal(this.helper.Gruporeservafria);
                    if (!dr.IsDBNull(iGruporeservafria)) entity.Gruporeservafria = dr.GetInt32(iGruporeservafria);

                    entity.Famnomb = dr.GetString(dr.GetOrdinal(helper.Famnomb));

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListarTopologiaEquipoPadres(string empresa, string familias, int tiporelcodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string strComando = string.Format(helper.SqlListarTopologiaEquipoPadres, empresa, familias, tiporelcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iFamnomb = dr.GetOrdinal(this.helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iEquirelfecmodificacion = dr.GetOrdinal(this.helper.Equirelfecmodificacion);
                    if (!dr.IsDBNull(iEquirelfecmodificacion)) entity.Equirelfecmodificacion = dr.GetDateTime(iEquirelfecmodificacion);

                    int iEquirelusumodificacion = dr.GetOrdinal(this.helper.Equirelusumodificacion);
                    if (!dr.IsDBNull(iEquirelusumodificacion)) entity.Equirelusumodificacion = dr.GetString(iEquirelusumodificacion);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }


        public List<EqEquipoDTO> ObtenerEquipoPadresHidrologicosCuenca()
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPadresHidrologia);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquiCodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquiCodi)) entity.Equicodi = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    int iEquiNomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.Equinomb = dr.GetString(iEquiNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public List<EqEquipoDTO> ObtenerEquipoPadresHidrologicosCuencaTodos()
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPadresHidrologiaTodos);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquiCodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquiCodi)) entity.Equicodi = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    int iEquiNomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.Equinomb = dr.GetString(iEquiNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<EqEquipoDTO> ObtenerEquipoPorAreaEmpresaTodos(int idEmpresa, int idArea)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEquipoPorAreaEmpresaTodos);

            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, idArea);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, idEmpresa);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquiCodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquiCodi)) entity.Equicodi = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    int iEquiNomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.Equinomb = dr.GetString(iEquiNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public EqEquipoDTO GetEqEquipo(string codOsinergmin, int codigoFamilia)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetEquipoParaCreacion);

            dbProvider.AddInParameter(command, helper.Osinergcodi, DbType.String, codOsinergmin);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, codigoFamilia);

            EqEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprNomb = dr.GetOrdinal("EMPRNOMB");
                    if (!dr.IsDBNull(iEmprNomb)) entity.EMPRNOMB = dr.GetString(iEmprNomb);

                    int iAreaNomb = dr.GetOrdinal("AREANOMB");
                    if (!dr.IsDBNull(iAreaNomb)) entity.AREANOMB = dr.GetString(iAreaNomb);

                    int iFamnomb = dr.GetOrdinal("Famnomb");
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(dr.GetOrdinal("Famnomb"));

                    int iOsinergcodi = dr.GetOrdinal(this.helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                }
            }

            return entity;
        }

        #region PR5
        public List<EqEquipoDTO> ListarEquipoXAreasXTipoEquipos(string sTipoEquipos, string idArea)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string strComando = string.Format(helper.SqlListarEquiposXTipoEquipos, sTipoEquipos, idArea);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEquiAbrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.Equiabrev = dr.GetString(iEquiAbrev);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(this.helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreacodi = dr.GetOrdinal(this.helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);

                    int iAreanomb = dr.GetOrdinal(this.helper.AREANOMB);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEquiestado = dr.GetOrdinal(this.helper.Equiestado);
                    if (!dr.IsDBNull(iEquiestado)) entity.Equiestado = dr.GetString(iEquiestado);

                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EqEquipoDTO> ListarEquipoXAreasXEmpresa(string idEmpresa, string idArea)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string strComando = string.Format(helper.SqlListarEquiposXEmpresasXArea, idEmpresa, idArea);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEquiAbrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.Equiabrev = dr.GetString(iEquiAbrev);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(this.helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreacodi = dr.GetOrdinal(this.helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);

                    int iAreanomb = dr.GetOrdinal(this.helper.AREANOMB);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);


                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EqEquipoDTO> ObtenerEquiposPorFamiliaOriglectcodi(int emprcodi, int famcodi, int origlectcodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string sql = string.Format(helper.SqlObtenerEquiposPorFamiliaOriglectcodi, origlectcodi, emprcodi, famcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iAreanomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iAreanomb)) entity.AREANOMB = dr.GetString(iAreanomb);

                    int iTareaabrev = dr.GetOrdinal(helper.TAREAABREV);
                    if (!dr.IsDBNull(iTareaabrev)) entity.TAREAABREV = dr.GetString(iTareaabrev);

                    entity.AREANOMB = entity.TAREAABREV + " " + entity.AREANOMB;

                    if (string.IsNullOrEmpty(entity.Equinomb)) entity.Equinomb = entity.Equiabrev;

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iEmprnomb = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region Indisponibilidades

        public List<EqEquipoDTO> ListarEquiposTTIE(string famcodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string query = string.Format(helper.SqlListarEquiposTTIE, famcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprnomb = dr.GetOrdinal(this.helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));
                    int iGrupoabrev = dr.GetOrdinal(this.helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);

                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEquiestado = dr.GetOrdinal(this.helper.Equiestado);
                    if (!dr.IsDBNull(iEquiestado)) entity.Equiestado = dr.GetString(iEquiestado);

                    int iHeqdatfecha = dr.GetOrdinal(this.helper.Heqdatfecha);
                    if (!dr.IsDBNull(iHeqdatfecha)) entity.Heqdatfecha = dr.GetDateTime(iHeqdatfecha);

                    int iHeqdatestado = dr.GetOrdinal(this.helper.Heqdatestado);
                    if (!dr.IsDBNull(iHeqdatestado)) entity.Heqdatestado = dr.GetString(iHeqdatestado);

                    int iEquicodiactual = dr.GetOrdinal(this.helper.Equicodiactual);
                    if (!dr.IsDBNull(iEquicodiactual)) entity.Equicodiactual = Convert.ToInt32(dr.GetValue(iEquicodiactual));

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iTgenercodi = dr.GetOrdinal(this.helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = dr.GetInt32(iTgenercodi);

                    entitys.Add(entity);
                }
                return entitys;
            }
        }

        #endregion

        #region Rechazo Carga
        //Metodos Rechazo Carga
        public List<EqEquipoDTO> ObtenerEquipoPorFamiliaRechazoCarga(int emprcodi, int famcodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string condicion = "";
            if (emprcodi > 0)
            {
                condicion = condicion + " and equipo.emprcodi = " + emprcodi.ToString("");
            }

            string queryString = string.Format(helper.SqlObtenerEquiposPorFamiliaRechazoCarga, condicion);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, famcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region BarrasModeladas

        public List<EqEquipoDTO> ListadoEquiposPorGrupoCodiFamilia(int grupocodi, int famcodi)
        {
            string query = string.Format(helper.SqlListEquiposGrupocodiFamilia, grupocodi, famcodi);
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    entity = helper.Create(dr);
                    entity.EMPRNOMB = dr.GetString(dr.GetOrdinal("EMPRNOMB"));
                    entity.AREANOMB = dr.GetString(dr.GetOrdinal("AREANOMB"));
                    entity.Famnomb = dr.GetString(dr.GetOrdinal("Famnomb"));
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public void ActualizarGrupoCodiPorEquipoFamilia(string sCodigosEquipo, int? iGrupoCodi, int iFamcodi, string sUsuario)
        {
            string query = string.Format(helper.SqlUpdateGrupoCodiPorCodigoFamilia, iGrupoCodi.HasValue ? iGrupoCodi.Value.ToString() : "null", sCodigosEquipo, iFamcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQueryAudit(command, sUsuario);
        }

        #endregion
        #region EMS
        public List<EqEquipoDTO> GetByEmprFamCentral(int emprcodi, int famcodi, int idCentral)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string sql = String.Format(helper.SqlGetByEmprFamCentral, emprcodi, famcodi, idCentral);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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

        #region NotificacionesCambiosEquipamiento
        public List<EqEquipoDTO> ListadoEquiposModificados(DateTime dtFechaInicio, DateTime dtFechaFin)
        {
            string query = string.Format(helper.SqlEquiposModificados, dtFechaInicio.ToString("dd-MM-yyyy HH:mm"), dtFechaFin.ToString("dd-MM-yyyy HH:mm"));
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iDesEmpresa = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iDesEmpresa)) entity.EMPRNOMB = dr.GetString(iDesEmpresa);

                    int iDesEquipo = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iDesEquipo)) entity.Equinomb = dr.GetString(iDesEquipo);

                    int iDesAbreviatura = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iDesAbreviatura)) entity.Equiabrev = dr.GetString(iDesAbreviatura);

                    int iDesFamilia = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iDesFamilia)) entity.Famnomb = dr.GetString(iDesFamilia);

                    int iLastUser = dr.GetOrdinal(helper.Lastuser);
                    if (!dr.IsDBNull(iLastUser)) entity.Lastuser = dr.GetString(iLastUser);

                    int iLastDate = dr.GetOrdinal(helper.Lastdate);
                    if (!dr.IsDBNull(iLastDate)) entity.Lastdate = dr.GetDateTime(iLastDate);

                    int iIdEquipo = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iIdEquipo)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iIdEquipo));

                    int iUsuarioUpdate = dr.GetOrdinal(helper.UsuarioUpdate);
                    if (!dr.IsDBNull(iUsuarioUpdate)) entity.UsuarioUpdate = dr.GetString(iUsuarioUpdate);

                    int iFechaUpdate = dr.GetOrdinal(helper.FechaUpdate);
                    if (!dr.IsDBNull(iFechaUpdate)) entity.FechaUpdate = dr.GetDateTime(iFechaUpdate);

                    int iEquiestado = dr.GetOrdinal(helper.Equiestado);
                    if (!dr.IsDBNull(iEquiestado)) entity.Equiestado = dr.GetString(iEquiestado);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        #endregion



        #region Transferencia de Equipos
        public List<EqEquipoDTO> ListarEquipoXEmpresa(string idEmpresa)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string strComando = string.Format(helper.SqlListarEquiposXEmpresas, idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEquiAbrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.Equiabrev = dr.GetString(iEquiAbrev);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(this.helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreacodi = dr.GetOrdinal(this.helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);

                    int iAreanomb = dr.GetOrdinal(this.helper.AREANOMB);
                    if (!dr.IsDBNull(iAreanomb)) entity.AREANOMB = dr.GetString(iAreanomb);

                    int iEquiEstado = dr.GetOrdinal(this.helper.Equiestado);
                    if (!dr.IsDBNull(iEquiEstado)) entity.Equiestado = dr.GetString(iEquiEstado);

                    int iFamnomb = dr.GetOrdinal(this.helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iEquitension = dr.GetOrdinal(this.helper.Equitension);
                    if (!dr.IsDBNull(iEquitension)) entity.Equitension = dr.GetDecimal(iEquitension);

                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    entitys.Add(entity);

                }
            }
            return entitys;
        }

        public List<EqEquipoDTO> ListarEqEquipoXEmpresaOrigenMigracion(int idEmpresa)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string strComando = string.Format(helper.SqlListarEquiposXEmpresaOrigenMigracion, idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }



        public List<EqEquipoDTO> ListaEquiposSiEquipoMigrarByMigracodi(int idMigracion)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string strComando = string.Format(helper.SqlGetListaEquiposSiEquipoMigrarByMigracodi, idMigracion);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEquiAbrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.Equiabrev = dr.GetString(iEquiAbrev);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(this.helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinombOrigen = dr.GetOrdinal(this.helper.EmpresaOrigen);
                    if (!dr.IsDBNull(iEquinombOrigen)) entity.EmpresaOrigen = dr.GetString(iEquinombOrigen);

                    int iAreacodi = dr.GetOrdinal(this.helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);

                    int iAreanomb = dr.GetOrdinal(this.helper.AREANOMB);
                    if (!dr.IsDBNull(iAreanomb)) entity.AREANOMB = dr.GetString(iAreanomb);

                    int iEquiEstado = dr.GetOrdinal(this.helper.Equiestado);
                    if (!dr.IsDBNull(iEquiEstado)) entity.Equiestado = dr.GetString(iEquiEstado);

                    int iFamnomb = dr.GetOrdinal(this.helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iEquitension = dr.GetOrdinal(this.helper.Equitension);
                    if (!dr.IsDBNull(iEquitension)) entity.Equitension = dr.GetDecimal(iEquitension);

                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iOperadornomb = dr.GetOrdinal(this.helper.Operadornomb);
                    if (!dr.IsDBNull(iOperadornomb)) entity.Operadornomb = dr.GetString(iOperadornomb);

                    entitys.Add(entity);
                }

            }
            return entitys;
        }

        #endregion

        #region MigracionSGOCOES-GrupoB
        public List<EqEquipoDTO> ListaLineasDigsilent(string propcodiDigsilente, string famcodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string strComando = string.Format(helper.SqlListaLineasDigsilent, propcodiDigsilente, famcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iValor = dr.GetOrdinal(this.helper.Valor);
                    if (!dr.IsDBNull(iValor)) entity.Valor = dr.GetString(iValor);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EqEquipoDTO> ListaPruebasAleatorias(DateTime fecIni)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string query = string.Format(helper.SqlListaPruebasAleatorias, fecIni.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEmprabrev = dr.GetOrdinal(this.helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprabrev)) entity.EMPRNOMB = dr.GetString(iEmprabrev);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEcodigo = dr.GetOrdinal(this.helper.Ecodigo);
                    if (!dr.IsDBNull(iEcodigo)) entity.Ecodigo = dr.GetString(iEcodigo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> GetListaPotencias(DateTime f_)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string query = string.Format(helper.SqlGetListaPotencias, f_.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    decimal potencia = 0; string strValor;
                    var entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iPmin = dr.GetOrdinal("Pmin");
                    if (!dr.IsDBNull(iPmin) && !string.IsNullOrEmpty(dr.GetString(iPmin)))
                    {
                        strValor = dr.GetString(iPmin);
                        if (decimal.TryParse(strValor, out potencia))
                        {
                            entity.Pmin = potencia;
                        }
                    }

                    int iPmax = dr.GetOrdinal("Pmax");
                    if (!dr.IsDBNull(iPmax) && !string.IsNullOrEmpty(dr.GetString(iPmax)))
                    {
                        strValor = dr.GetString(iPmax);
                        try { entity.Pmax = decimal.Parse(strValor); }
                        catch { }
                    }

                    int iPe = dr.GetOrdinal("Pe");
                    if (!dr.IsDBNull(iPe) && !string.IsNullOrEmpty(dr.GetString(iPe)))
                    {
                        strValor = dr.GetString(iPe);
                        try { entity.Pe = decimal.Parse(strValor); }
                        catch { }
                    }

                    int iTminparada = dr.GetOrdinal("Tminparada");
                    if (!dr.IsDBNull(iTminparada)) entity.Tminparadadesc = dr.GetString(iTminparada);

                    int iTminopera = dr.GetOrdinal("Tminopera");
                    if (!dr.IsDBNull(iTminopera)) entity.Tminoperadesc = dr.GetString(iTminopera);

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = dr.GetInt32(iFamcodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region INTERVENCIONES
        public List<EqEquipoDTO> ListarComboEquiposXUbicaciones(string IdArea)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(String.Format(helper.SqlListarComboEquiposXUbicaciones, IdArea));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquipocodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquipocodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquipocodi));

                    int iEquiponomb = dr.GetOrdinal(helper.Equidesc);
                    if (!dr.IsDBNull(iEquiponomb)) entity.Equinomb = dr.GetString(iEquiponomb);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void ObtenerDatosEquipamiento(int IdEquipo, ref int IdUbicacion, ref int IdEmpresa, out int equicodi)
        {
            equicodi = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlTraerDatosEquipamientoByIdEquipo);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, IdEquipo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    IdUbicacion = Convert.ToInt32(dr["Areacodi"].ToString().Trim());
                    IdEmpresa = Convert.ToInt32(dr["Emprcodi"].ToString().Trim());
                    equicodi = Convert.ToInt32(dr["Equicodi"].ToString().Trim());
                }
            }
        }

        // Mtodos agregados para Procedimiento Maniobra
        public List<EqEquipoDTO> ListarEquiposXIds(string idsEquipos)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            var query = string.Format(helper.SqlListarEquiposByIds, idsEquipos);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquiCodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquiCodi)) entity.Equicodi = dr.GetInt32(iEquiCodi);

                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = dr.GetInt32(iFamcodi);

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iEquiNomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.Equinomb = dr.GetString(iEquiNomb);

                    int iEquitension = dr.GetOrdinal(this.helper.Equitension);
                    if (!dr.IsDBNull(iEquitension)) entity.Equitension = dr.GetDecimal(iEquitension);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreaNomb = dr.GetOrdinal(this.helper.AREANOMB);
                    if (!dr.IsDBNull(iAreaNomb)) entity.Areanomb = dr.GetString(iAreaNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ObtenerEnlaces(string idsEquipos)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(String.Format(helper.SqlObtenerEnlacesByIds, idsEquipos));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquiCodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquiCodi)) entity.Equicodi = dr.GetInt32(iEquiCodi);

                    int iUrlmaniobra = dr.GetOrdinal(helper.Urlmaniobra);
                    if (!dr.IsDBNull(iUrlmaniobra)) entity.Urlmaniobra = dr.GetString(iUrlmaniobra);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListarComboEquiposXUbicacionesXFamilia(string idArea, string idFamilia)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(String.Format(helper.SqlListarComboEquiposXUbicacionesXFamilia, idArea, idFamilia));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquipocodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquipocodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquipocodi));

                    int iEquiponomb = dr.GetOrdinal(helper.Equidesc);
                    if (!dr.IsDBNull(iEquiponomb)) entity.Equinomb = dr.GetString(iEquiponomb);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListarEquiposXTipoprograma(int evenclasecodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(String.Format(helper.SqlListarEquiposXPrograma, evenclasecodi));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquipocodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquipocodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquipocodi));

                    int iEquiponomb = dr.GetOrdinal(helper.Equidesc);
                    if (!dr.IsDBNull(iEquiponomb)) entity.Equinomb = dr.GetString(iEquiponomb);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListarLineasValidas()
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string strComando = string.Format(helper.SqlListarLineasValidas);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();
                    entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EqEquipoDTO> ListarCeldasValidas()
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string strComando = string.Format(helper.SqlListarCeldasValidas);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();
                    entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        #endregion

        #region Numerales Datos Base
        public List<EqEquipoDTO> ListaNumerales_DatosBase_5_6_6()
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBase_5_6_6);

            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iOsigrupocodi = dr.GetOrdinal(helper.Osigrupocodi);
                    if (!dr.IsDBNull(iOsigrupocodi)) entity.Osigrupocodi = dr.GetString(iOsigrupocodi);


                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iEquiestado = dr.GetOrdinal(helper.Equiestado);
                    if (!dr.IsDBNull(iEquiestado)) entity.Equiestado = dr.GetString(iEquiestado);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        #endregion

        #region Medidores de Generación PR15
        /// <summary>
        /// Método que retorna el listado de equipos asociados a un punto de medición
        /// </summary>
        /// <param name="ptomedicodi">Código de punto de medición</param>
        /// <returns>Listado de Equipos asociados al punto de medición</returns>
        public List<EqEquipoDTO> GetEquipoByPuntoMedicion(int ptomedicodi)
        {

            var entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlEquipoByPtoMedicion, ptomedicodi));
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oEquipo = new EqEquipoDTO();
                    oEquipo = helper.Create(dr);
                    entitys.Add(oEquipo);
                }
            }
            return entitys;
        }
        #endregion

        #region Equipos sin datos de ficha técnica

        public List<EqEquipoDTO> ListaEqEmpresaFamilia(int emprCodi, int famCodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            String query = String.Format(helper.SqlListaEqEmpresaFamilia, emprCodi, famCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oEquipo = new EqEquipoDTO();
                    oEquipo = helper.Create(dr);

                    oEquipo.Emprnomb = dr.GetString(dr.GetOrdinal(helper.EMPRNOMB));
                    oEquipo.Famnomb = dr.GetString(dr.GetOrdinal(helper.Famnomb));

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) oEquipo.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    int iAreadesc = dr.GetOrdinal(helper.Areadesc);
                    if (!dr.IsDBNull(iAreadesc)) oEquipo.Areadesc = dr.GetString(iAreadesc);

                    entitys.Add(oEquipo);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListarIngresoSalidaOperacionComercialSEIN(DateTime fechaini, DateTime fechafin, string famcodi)
        {
            throw new NotImplementedException();
        }

        public List<EqEquipoDTO> ListarIngresoOperacionComercialSEIN(DateTime fechaini, DateTime fechafin)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Mejoras Yupana
        public List<EqEquipoDTO> ListarUnidadesxPlanta2(string pCodigos, int pFamilia)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string queryString = string.Format(helper.SqlListarUnidadesxPlanta2, pCodigos, pFamilia);
            EqEquipoDTO entity = new EqEquipoDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EqEquipoDTO();
                    entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        #endregion

        #region FICHA TÉCNICA

        public int Save(EqEquipoDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equicodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Elecodi, DbType.Int32, entity.Elecodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Areacodi, DbType.Int32, entity.Areacodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Famcodi, DbType.Int32, entity.Famcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equiabrev, DbType.String, entity.Equiabrev));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equinomb, DbType.String, entity.Equinomb));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equiabrev2, DbType.String, entity.Equiabrev2));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equitension, DbType.Decimal, entity.Equitension));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equipadre, DbType.Int32, entity.Equipadre));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equipot, DbType.Decimal, entity.Equipot));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Lastuser, DbType.String, entity.Lastuser));
                //dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Lastdate, DbType.DateTime, entity.Lastdate));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ecodigo, DbType.String, entity.Ecodigo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equiestado, DbType.String, entity.Equiestado));

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Lastcodi, DbType.Int32, entity.Lastcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equifechiniopcom, DbType.DateTime, entity.Equifechiniopcom));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equifechfinopcom, DbType.DateTime, entity.Equifechfinopcom));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equimaniobra, DbType.String, string.IsNullOrEmpty(entity.EquiManiobra) ? "N" : entity.EquiManiobra));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Osinergcodi, DbType.String, entity.Osinergcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Operadoremprcodi, DbType.Int32, entity.Operadoremprcodi));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public List<EqEquipoDTO> ListarSubestacionFT()
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarSubestacionFT);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iAreadesc = dr.GetOrdinal("AREADESC");
                    if (!dr.IsDBNull(iAreadesc)) entity.Areadesc = dr.GetString(iAreadesc);

                    int iEmprnomb = dr.GetOrdinal("EMPRNOMB");
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region Ficha tecnica 2023
        public List<EqEquipoDTO> ListarPorEmpresaPropietaria(int emprcodi, int famcodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();

            string query = string.Format(helper.SqlListarPorEmpresaPropietaria, emprcodi, famcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iIdelemento = dr.GetOrdinal(helper.Idelemento);
                    if (!dr.IsDBNull(iIdelemento)) entity.Idelemento = Convert.ToInt32(dr.GetValue(iIdelemento));

                    int iIdempresaelemento = dr.GetOrdinal(helper.Idempresaelemento);
                    if (!dr.IsDBNull(iIdempresaelemento)) entity.Idempresaelemento = Convert.ToInt32(dr.GetValue(iIdempresaelemento));

                    int iNombempresaelemento = dr.GetOrdinal(helper.Nombempresaelemento);
                    if (!dr.IsDBNull(iNombempresaelemento)) entity.Nombempresaelemento = dr.GetString(iNombempresaelemento);

                    int iIdempresacopelemento = dr.GetOrdinal(helper.Idempresacopelemento);
                    if (!dr.IsDBNull(iIdempresacopelemento)) entity.Idempresacopelemento = Convert.ToInt32(dr.GetValue(iIdempresacopelemento));

                    int iNombempresacopelemento = dr.GetOrdinal(helper.Nombempresacopelemento);
                    if (!dr.IsDBNull(iNombempresacopelemento)) entity.Nombempresacopelemento = dr.GetString(iNombempresacopelemento); 

                    int iNombreelemento = dr.GetOrdinal(helper.Nombreelemento);
                    if (!dr.IsDBNull(iNombreelemento)) entity.Nombreelemento = dr.GetString(iNombreelemento);

                    int iTipoelemento = dr.GetOrdinal(helper.Tipoelemento);
                    if (!dr.IsDBNull(iTipoelemento)) entity.Tipoelemento = dr.GetString(iTipoelemento);

                    int iAreaelemento = dr.GetOrdinal(helper.Areaelemento);
                    if (!dr.IsDBNull(iAreaelemento)) entity.Areaelemento = dr.GetString(iAreaelemento);

                    int iEstadoelemento = dr.GetOrdinal(helper.Estadoelemento);
                    if (!dr.IsDBNull(iEstadoelemento)) entity.Estadoelemento = dr.GetString(iEstadoelemento);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListarPorEmpresaCoPropietaria(int emprcodi, int famcodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();

            string query = string.Format(helper.SqlListarPorEmpresaCopropietaria, emprcodi, famcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iIdelemento = dr.GetOrdinal(helper.Idelemento);
                    if (!dr.IsDBNull(iIdelemento)) entity.Idelemento = Convert.ToInt32(dr.GetValue(iIdelemento));

                    int iIdempresaelemento = dr.GetOrdinal(helper.Idempresaelemento);
                    if (!dr.IsDBNull(iIdempresaelemento)) entity.Idempresaelemento = Convert.ToInt32(dr.GetValue(iIdempresaelemento));

                    int iNombempresaelemento = dr.GetOrdinal(helper.Nombempresaelemento);
                    if (!dr.IsDBNull(iNombempresaelemento)) entity.Nombempresaelemento = dr.GetString(iNombempresaelemento);

                    int iIdempresacopelemento = dr.GetOrdinal(helper.Idempresacopelemento);
                    if (!dr.IsDBNull(iIdempresacopelemento)) entity.Idempresacopelemento = Convert.ToInt32(dr.GetValue(iIdempresacopelemento));

                    int iNombempresacopelemento = dr.GetOrdinal(helper.Nombempresacopelemento);
                    if (!dr.IsDBNull(iNombempresacopelemento)) entity.Nombempresacopelemento = dr.GetString(iNombempresacopelemento);

                    int iNombreelemento = dr.GetOrdinal(helper.Nombreelemento);
                    if (!dr.IsDBNull(iNombreelemento)) entity.Nombreelemento = dr.GetString(iNombreelemento);

                    int iTipoelemento = dr.GetOrdinal(helper.Tipoelemento);
                    if (!dr.IsDBNull(iTipoelemento)) entity.Tipoelemento = dr.GetString(iTipoelemento);

                    int iAreaelemento = dr.GetOrdinal(helper.Areaelemento);
                    if (!dr.IsDBNull(iAreaelemento)) entity.Areaelemento = dr.GetString(iAreaelemento);

                    int iEstadoelemento = dr.GetOrdinal(helper.Estadoelemento);
                    if (!dr.IsDBNull(iEstadoelemento)) entity.Estadoelemento = dr.GetString(iEstadoelemento);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> GetByEmprFamCentral2(int emprcodi, int famcodi, int idCentral)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string sql = String.Format(helper.SqlGetByEmprFamCentral2, emprcodi, famcodi, idCentral);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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

        #region Demanda DPO - Iteracion 2
        public List<EqEquipoDTO> ListaEquipoByEmpresa(int idEmpresa)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();

            string sql = string.Format(helper.SqlListaEquipoByEmpresa, idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquiCodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquiCodi)) entity.Equicodi = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    int iEquiNomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.Equinomb = dr.GetString(iEquiNomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region CPPA.ASSETEC.2024
        public List<EqEquipoDTO> ListaCentralesGeneracion()
        {
            EqEquipoDTO entity = new EqEquipoDTO();
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaCentralesGeneradoras);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEquinombConcatenado = dr.GetOrdinal(helper.EquinombConcatenado);
                    if (!dr.IsDBNull(iEquinombConcatenado)) entity.EquinombConcatenado = dr.GetString(iEquinombConcatenado);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        //INICIO GESPROTECT - 20241031
        #region GESPROTEC - 20241031
        public List<EqEquipoDTO> ListaEquipoCOES(String idUbicacion, String idTipoEquipo, String nombreEquipo, string sProgramaExistente)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();

            string sql = string.Format(helper.ListaEquipoCOES);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.String, idUbicacion);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.String, idUbicacion);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.String, idTipoEquipo);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.String, idTipoEquipo);
            dbProvider.AddInParameter(command, helper.Equinomb, DbType.String, nombreEquipo);
            dbProvider.AddInParameter(command, helper.Equinomb, DbType.String, nombreEquipo);
            dbProvider.AddInParameter(command, helper.Equiabrev, DbType.String, nombreEquipo);
            dbProvider.AddInParameter(command, helper.Equiabrev, DbType.String, sProgramaExistente);
            dbProvider.AddInParameter(command, helper.Equiabrev, DbType.String, sProgramaExistente);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquiCodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquiCodi)) entity.Equicodi = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    int iEquiNomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.Equinomb = dr.GetString(iEquiNomb);

                    int iEquiestado = dr.GetOrdinal(helper.Equiestado);
                    if (!dr.IsDBNull(iEquiestado)) entity.Equiestado = dr.GetString(iEquiestado);

                    int iAreanombre = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iAreanombre)) entity.Areadesc = dr.GetString(iAreanombre);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iEmprnomb = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iFlaggenprotec = dr.GetOrdinal(helper.Flaggenprotec);
                    if (!dr.IsDBNull(iFlaggenprotec)) entity.Flaggenprotec = dr.GetString(iFlaggenprotec);

                    int iEpequinombenprotec = dr.GetOrdinal(helper.Epequinombenprotec);
                    if (!dr.IsDBNull(iEpequinombenprotec)) entity.Epequinombenprotec = dr.GetString(iEpequinombenprotec);

                    int iEpequicodi = dr.GetOrdinal(helper.Epequicodi);
                    if (!dr.IsDBNull(iEpequicodi)) entity.Epequicodi = Convert.ToInt32(dr.GetString(iEpequicodi));
                    

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListaExportarEquipoCOES(string idUbicacion, string idTipoEquipo, string nombreEquipo)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();

            string sql = string.Format(helper.ListaReporteEquipoCOES);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.AddInParameter(command, "areacodi", DbType.String, idUbicacion);
            dbProvider.AddInParameter(command, "areacodi", DbType.String, idUbicacion);
            dbProvider.AddInParameter(command, "famcodi", DbType.String, idTipoEquipo);
            dbProvider.AddInParameter(command, "famcodi", DbType.String, idTipoEquipo);
            dbProvider.AddInParameter(command, "equinomb", DbType.String, nombreEquipo);
            dbProvider.AddInParameter(command, "equinomb", DbType.String, nombreEquipo);
            //dbProvider.AddInParameter(command, "equinomb", DbType.String, nombreEquipo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iAreaCodi = dr.GetOrdinal("AREACODI");
                    if (!dr.IsDBNull(iAreaCodi)) entity.Areacodi = Convert.ToInt16(dr.GetValue(iAreaCodi));

                    int iAreaNomb = dr.GetOrdinal("ZONA");
                    if (!dr.IsDBNull(iAreaNomb)) entity.Areanomb = dr.GetString(iAreaNomb);

                    int iEmprNomb = dr.GetOrdinal("TITULAR");
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    int iSubestacion = dr.GetOrdinal("SUBESTACION");
                    if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = dr.GetString(iSubestacion);

                    int iCelda = dr.GetOrdinal("CELDA");
                    if (!dr.IsDBNull(iCelda)) entity.Celda = dr.GetString(iCelda);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListLineaEvaluacion(string equiCodi, string codigo, string ubicacion, string emprCodigo, string equiEstado)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();

            string sql = string.Format(helper.SqlListLineaEvaluacion);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.String, equiCodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.String, equiCodi);
            dbProvider.AddInParameter(command, helper.Codigo, DbType.String, codigo);
            dbProvider.AddInParameter(command, helper.Codigo, DbType.String, codigo);
            dbProvider.AddInParameter(command, helper.AREANOMB, DbType.String, ubicacion);
            dbProvider.AddInParameter(command, helper.AREANOMB, DbType.String, ubicacion);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.String, emprCodigo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.String, emprCodigo);
            dbProvider.AddInParameter(command, helper.Equiestado, DbType.String, equiEstado);
            dbProvider.AddInParameter(command, helper.Equiestado, DbType.String, equiEstado);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();


                    
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iAreadesc = dr.GetOrdinal(helper.Areadesc);
                    if (!dr.IsDBNull(iAreadesc)) entity.Areadesc = Convert.ToString(dr.GetValue(iAreadesc));

                    int iEMPRNOMB = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = Convert.ToString(dr.GetValue(iEMPRNOMB));

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iAREANOMB = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iAREANOMB)) entity.AREANOMB = dr.GetString(iAREANOMB);

                    int iEquitension = dr.GetOrdinal(helper.Equitension);
                    if (!dr.IsDBNull(iEquitension)) entity.Equitension = dr.GetDecimal(iEquitension);

                    int iEquiestado = dr.GetOrdinal(helper.Equiestado);
                    if (!dr.IsDBNull(iEquiestado)) entity.Equiestado = dr.GetString(iEquiestado);

                    int iEquiestadoDesc = dr.GetOrdinal(helper.EquiestadoDesc);
                    if (!dr.IsDBNull(iEquiestadoDesc)) entity.EquiestadoDesc = dr.GetString(iEquiestadoDesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region Eqiupo Proteccion 2024

        public List<EqEquipoDTO> ListarMaestroCeldaProteccion()
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListMaestroCeldaProteccion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprabrev = dr.GetString(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Areanomb = dr.GetString(iEmprnomb);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Equinomb = dr.GetString(iEmprrazsocial);

                    int iTipoemprdesc = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Emprnomb = dr.GetString(iTipoemprdesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListarMaestroReleProteccion(int tipoUso)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListMaestroReleProteccion);
            dbProvider.AddInParameter(command, helper.TipoUso, DbType.Int32, tipoUso);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprabrev = dr.GetString(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Areanomb = dr.GetString(iEmprnomb);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Equinomb = dr.GetString(iEmprrazsocial);

                    int iTipoemprdesc = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Emprnomb = dr.GetString(iTipoemprdesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListarMaestroInterruptorProteccion()
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListMaestroInterruptorProteccion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprabrev = dr.GetString(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Areanomb = dr.GetString(iEmprnomb);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Equinomb = dr.GetString(iEmprrazsocial);

                    int iTipoemprdesc = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Emprnomb = dr.GetString(iTipoemprdesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region GESPROTECT-20250206
        public List<EqEquipoDTO> ListarMaestroEquiposLinea()
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListMaestroEquiposLinea);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprabrev = dr.GetString(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Areanomb = dr.GetString(iEmprnomb);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Equinomb = dr.GetString(iEmprrazsocial);

                    int iTipoemprdesc = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Emprnomb = dr.GetString(iTipoemprdesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListarMaestroEquiposArea()
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListMaestroEquiposArea);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Areadesc);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Areadesc = dr.GetString(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Areanomb = dr.GetString(iEmprnomb);                  

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListarMaestroEquiposCondensador()
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListMaestroEquiposCondensador);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprabrev = dr.GetString(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Areanomb = dr.GetString(iEmprnomb);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Equinomb = dr.GetString(iEmprrazsocial);

                    int iTipoemprdesc = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Emprnomb = dr.GetString(iTipoemprdesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListarMaestroEquiposReactor()
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListMaestroEquiposReactor);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprabrev = dr.GetString(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Areanomb = dr.GetString(iEmprnomb);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Equinomb = dr.GetString(iEmprrazsocial);

                    int iTipoemprdesc = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Emprnomb = dr.GetString(iTipoemprdesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListarMaestroEquiposCeldasAcoplamiento()
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListMaestroEquiposCeldasAcoplamiento);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprabrev = dr.GetString(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Areanomb = dr.GetString(iEmprnomb);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Equinomb = dr.GetString(iEmprrazsocial);

                    int iTipoemprdesc = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Emprnomb = dr.GetString(iTipoemprdesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListarMaestroEquiposTransformador()
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListMaestroEquiposTransformador);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprabrev = dr.GetString(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Areanomb = dr.GetString(iEmprnomb);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Equinomb = dr.GetString(iEmprrazsocial);

                    int iTipoemprdesc = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Emprnomb = dr.GetString(iTipoemprdesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion
    }
}
