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
    /// Clase de acceso a datos de la tabla ME_PTOMEDICION
    /// </summary>
    public class MePtomedicionRepository : RepositoryBase, IMePtomedicionRepository
    {
        public MePtomedicionRepository(string strConn) : base(strConn)
        {
        }

        MePtomedicionHelper helper = new MePtomedicionHelper();

        public int Save(MePtomedicionDTO entity)
        {
            //validación campo Ptomedicalculado
            if (string.IsNullOrEmpty(entity.PtomediCalculado)) entity.PtomediCalculado = ConstantesBase.NO;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);
            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Osicodi, DbType.String, entity.Osicodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Codref, DbType.Int32, entity.Codref);
            dbProvider.AddInParameter(command, helper.Ptomedidesc, DbType.String, entity.Ptomedidesc);
            dbProvider.AddInParameter(command, helper.Orden, DbType.Int32, entity.Orden);
            dbProvider.AddInParameter(command, helper.Ptomedielenomb, DbType.String, entity.Ptomedielenomb);
            dbProvider.AddInParameter(command, helper.Ptomedibarranomb, DbType.String, entity.Ptomedibarranomb);
            dbProvider.AddInParameter(command, helper.Origlectcodi, DbType.Int32, entity.Origlectcodi);
            dbProvider.AddInParameter(command, helper.Tipoptomedicodi, DbType.Int16, entity.Tipoptomedicodi);
            dbProvider.AddInParameter(command, helper.Ptomediestado, DbType.String, entity.Ptomediestado);
            dbProvider.AddInParameter(command, helper.Ptomedicalculado, DbType.String, entity.PtomediCalculado);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);


            //-FIT- Aplicativo VTD
            dbProvider.AddInParameter(command, helper.Clientecodi, DbType.Int32, entity.Clientecodi);
            dbProvider.AddInParameter(command, helper.PuntoConexion, DbType.String, entity.PuntoConexion);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.TipoSerie, DbType.Int32, entity.TipoSerie);

            //-FIN FIT

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void Update(MePtomedicionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Osicodi, DbType.String, entity.Osicodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Codref, DbType.Int32, entity.Codref);
            dbProvider.AddInParameter(command, helper.Ptomedidesc, DbType.String, entity.Ptomedidesc);
            dbProvider.AddInParameter(command, helper.Orden, DbType.Int32, entity.Orden);
            dbProvider.AddInParameter(command, helper.Ptomedielenomb, DbType.String, entity.Ptomedielenomb);
            dbProvider.AddInParameter(command, helper.Ptomedibarranomb, DbType.String, entity.Ptomedibarranomb);
            dbProvider.AddInParameter(command, helper.Origlectcodi, DbType.Int32, entity.Origlectcodi);
            dbProvider.AddInParameter(command, helper.Tipoptomedicodi, DbType.Int16, entity.Tipoptomedicodi);
            dbProvider.AddInParameter(command, helper.Ptomediestado, DbType.String, entity.Ptomediestado);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.String, entity.Areacodi);
            //- FIT - Aplicativo VTD
            dbProvider.AddInParameter(command, helper.Clientecodi, DbType.Int32, entity.Clientecodi);
            dbProvider.AddInParameter(command, helper.PuntoConexion, DbType.String, entity.PuntoConexion);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.TipoSerie, DbType.String, entity.TipoSerie);

            //- FIN FIT
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ptomedicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(int ptomedicodi, string username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);

            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, username);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public MePtomedicionDTO GetById(int ptomedicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);

            MePtomedicionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iFAMCODI = dr.GetOrdinal("famcodi");
                    if (!dr.IsDBNull(iFAMCODI)) entity.Famcodi = Convert.ToInt16(dr.GetValue(iFAMCODI));

                    int iPtomedicalculado = dr.GetOrdinal(helper.Ptomedicalculado);
                    if (!dr.IsDBNull(iPtomedicalculado)) entity.PtomediCalculado = dr.GetString(iPtomedicalculado);

                    int iClientecodi = dr.GetOrdinal(helper.Clientecodi);
                    if (!dr.IsDBNull(iClientecodi)) entity.Clientecodi = Convert.ToInt32(dr.GetValue(iClientecodi));

                    int iPuntoConexion = dr.GetOrdinal(helper.PuntoConexion);
                    if (!dr.IsDBNull(iClientecodi))
                    {
                        try
                        {
                            entity.PuntoConexion = dr.GetString(iPuntoConexion);
                        }
                        catch
                        {
                            entity.PuntoConexion = "";
                        }
                    }

                    int iBarrcodi = dr.GetOrdinal(helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

                    int iCatecodi = dr.GetOrdinal(helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

                    // -----------------------------------------------------------------------------------------------------------------
                    // ASSETEC 01-07-2022
                    // -----------------------------------------------------------------------------------------------------------------
                    int iGrupocodibarra = dr.GetOrdinal(helper.Grupocodibarra);
                    if (!dr.IsDBNull(iGrupocodibarra)) entity.Grupocodibarra = Convert.ToInt32(dr.GetValue(iGrupocodibarra));
                    // -----------------------------------------------------------------------------------------------------------------

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                }
            }

            return entity;
        }

        public List<MePtomedicionDTO> List(string ptomedicodi, string origlectcodi)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string query = string.Format(helper.SqlList, ptomedicodi, origlectcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtomedicionDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iGrupoabrev = dr.GetOrdinal(helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);

                    int iEmprenomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprenomb)) entity.Emprnomb = dr.GetString(iEmprenomb);

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);
                    int iCentral = dr.GetOrdinal(helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iFAMCODI = dr.GetOrdinal("famcodi");
                    if (!dr.IsDBNull(iFAMCODI)) entity.Famcodi = Convert.ToInt16(dr.GetValue(iFAMCODI));

                    int iTgenercodi = dr.GetOrdinal(helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));
                    int iTgenernomb = dr.GetOrdinal(helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = dr.GetInt32(iFenergcodi);
                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iTipogenerrer = dr.GetOrdinal(helper.Tipogenerrer);
                    if (!dr.IsDBNull(iTipogenerrer)) entity.Tipogenerrer = dr.GetString(iTipogenerrer);
                    int iGrupotipocogen = dr.GetOrdinal(helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);
                    int iGrupointegrante = dr.GetOrdinal(helper.Grupointegrante);
                    if (!dr.IsDBNull(iGrupointegrante)) entity.Grupointegrante = dr.GetString(iGrupointegrante);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MePtomedicionDTO> GetByIdEquipo(int equicodi)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdEquipo);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtomedicionDTO entity = helper.Create(dr);

                    int iOriglectcodi = dr.GetOrdinal(this.helper.Origlectcodi);
                    if (!dr.IsDBNull(iOriglectcodi)) entity.Origlectcodi = Convert.ToInt32(dr.GetValue(iOriglectcodi));
                    int iOriglectnombre = dr.GetOrdinal(helper.Origlectnombre);
                    if (!dr.IsDBNull(iOriglectnombre)) entity.Origlectnombre = dr.GetString(iOriglectnombre);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MePtomedicionDTO> GetByIdEquipoUsuarioLibre(int equicodi, int emprcodisuministrador)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdEquipoUsuarioLibre);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Emprcodisuministrador, DbType.Int32, emprcodisuministrador);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtomedicionDTO entity = helper.Create(dr);

                    int iOriglectcodi = dr.GetOrdinal(this.helper.Origlectcodi);
                    if (!dr.IsDBNull(iOriglectcodi)) entity.Origlectcodi = Convert.ToInt32(dr.GetValue(iOriglectcodi));
                    int iOriglectnombre = dr.GetOrdinal(helper.Origlectnombre);
                    if (!dr.IsDBNull(iOriglectnombre)) entity.Origlectnombre = dr.GetString(iOriglectnombre);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MePtomedicionDTO> GetByIdGrupo(int grupocodi)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdGrupo);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtomedicionDTO entity = helper.Create(dr);

                    int iOriglectcodi = dr.GetOrdinal(this.helper.Origlectcodi);
                    if (!dr.IsDBNull(iOriglectcodi)) entity.Origlectcodi = Convert.ToInt32(dr.GetValue(iOriglectcodi));
                    int iOriglectnombre = dr.GetOrdinal(helper.Origlectnombre);
                    if (!dr.IsDBNull(iOriglectnombre)) entity.Origlectnombre = dr.GetString(iOriglectnombre);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MePtomedicionDTO> GetByCriteria(string empresas, string idsOriglectura, string idsTipoptomedicion)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string query = string.Format(helper.SqlGetByCriteria, empresas, idsOriglectura, idsTipoptomedicion);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            MePtomedicionDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();
                    entity = helper.Create(dr);
                    int iEmprenomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprenomb)) entity.Emprnomb = dr.GetString(iEmprenomb);
                    int iEmprestado = dr.GetOrdinal(helper.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int iOriglectnombre = dr.GetOrdinal(helper.Origlectnombre);
                    if (!dr.IsDBNull(iOriglectnombre)) entity.Origlectnombre = dr.GetString(iOriglectnombre);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iEquiestado = dr.GetOrdinal(helper.Equiestado);
                    if (!dr.IsDBNull(iEquiestado)) entity.Equiestado = dr.GetString(iEquiestado);

                    int iFAMCODI = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFAMCODI)) entity.Famcodi = Convert.ToInt16(dr.GetValue(iFAMCODI));
                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);
                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iCatecodi = dr.GetOrdinal(helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));
                    int iCatenomb = dr.GetOrdinal(this.helper.Catenomb);
                    if (!dr.IsDBNull(iCatenomb)) entity.Catenomb = dr.GetString(iCatenomb);

                    int iTipoptomedinomb = dr.GetOrdinal(helper.Tipoptomedinomb);
                    if (!dr.IsDBNull(iTipoptomedinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);
                    int iTipoinfoabrev = dr.GetOrdinal(helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);
                    int iGrupoactivo = dr.GetOrdinal(helper.Grupoactivo);
                    if (!dr.IsDBNull(iGrupoactivo)) entity.Grupoactivo = dr.GetString(iGrupoactivo);
                    int iGrupoEstado = dr.GetOrdinal(this.helper.GrupoEstado);
                    if (!dr.IsDBNull(iGrupoEstado)) entity.Grupoestado = dr.GetString(iGrupoEstado);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MePtomedicionDTO> ListarPtoMedicion(string listapto)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string sqlQuery = string.Format(helper.SqlListarPtoMedicion, listapto);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            MePtomedicionDTO entity;


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();
                    entity = helper.Create(dr);
                    int iFAMCODI = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFAMCODI)) entity.Famcodi = Convert.ToInt16(dr.GetValue(iFAMCODI));
                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);
                    int iCENTRALNOMB = dr.GetOrdinal(helper.Centralnomb);
                    if (!dr.IsDBNull(iCENTRALNOMB)) entity.Centralnomb = dr.GetString(iCENTRALNOMB);
                    entitys.Add(entity);
                }
            }

            return entitys;       
        }

        public List<MePtomedicionDTO> ListarPtoDuplicado(int equipo, int origen, int tipopto)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string sqlQuery = string.Format(helper.SqlGetPtoDuplicado, equipo, origen, tipopto);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {

                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
 
             public List<MePtomedicionDTO> ListarPtoDuplicadoNombreEmpresa(string nombrepto, int empresacodi)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string sqlQuery = string.Format(helper.SqlGetPtoDuplicadoNombreEmpresa, nombrepto, empresacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {

                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MePtomedicionDTO> ListarPtoDuplicadoGrupo(int grupo, int origen, int tipopto)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string sqlQuery = string.Format(helper.SqlGetPtoDuplicadoGrupo, grupo, origen, tipopto);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {

                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public int ObtenerTotalPtomedicion(string empresas, string idsOriglectura, string idsTipoptomedicion, string idsFamilia,
            string ubicacion, string categoria, int tipoPunto, int codigo, int? cliente, int? barra)
        {
            string sqlTotal = string.Empty;

            if (tipoPunto == 1)
            {
                sqlTotal = string.Format(helper.SqlTotalListaPtoMedicion, empresas, idsOriglectura, idsTipoptomedicion, idsFamilia, ubicacion, codigo);
            }
            if (tipoPunto == 2)
            {
                sqlTotal = string.Format(helper.SqlTotalListaPtoMedicionGrupo, empresas, idsOriglectura, idsTipoptomedicion, categoria, codigo);
            }
            if (tipoPunto == 3)
            {
                sqlTotal = string.Format(helper.SqlTotalListaPtoMedicionTransferencia, empresas, idsOriglectura, cliente, barra, codigo);
            }
            if (tipoPunto == 4)
            {
                sqlTotal = string.Format(helper.SqlTotalListaPtoMedicionNoDefinido, idsOriglectura, codigo);
            }

            DbCommand command = dbProvider.GetSqlStringCommand(sqlTotal);
            object result = dbProvider.ExecuteScalar(command);
            int total = 0;
            if (result != null) total = Convert.ToInt32(result);
            return total;
        }


        public List<MePtomedicionDTO> ListarDetallePtoMedicionFiltro(string empresas, string idsOriglectura, string idsTipoptomedicion, int nroPaginas,
            int pageSize, string idsFamilia, string ubicacion, string categoria, int tipoPunto, int codigo, int? cliente, int? barra, string campo, string orden)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();

            string query = string.Empty;

            if (tipoPunto == 1)
            {
                query = string.Format(helper.SqlListarDetallePtoMedicionFiltro, empresas, idsOriglectura, idsTipoptomedicion,
                nroPaginas, pageSize, idsFamilia, ubicacion, campo, orden, codigo);
            }
            else if (tipoPunto == 2)
            {
                query = string.Format(helper.SqlListarDetallePtoMedicionFiltroGrupo, empresas, idsOriglectura, idsTipoptomedicion,
                nroPaginas, pageSize, categoria, campo, orden, codigo);
            }
            #region FIT - VALORIZACION DIARIA
            else if (tipoPunto == 3)
            {
                query = string.Format(helper.SqlListarDetallePtoMedicionFiltroTransferencias, empresas, idsOriglectura, cliente, barra, codigo,
                    nroPaginas, pageSize, campo, orden);
            }
            #endregion
            else if (tipoPunto == 4)
            {
                query = string.Format(helper.SqlListarDetallePtoMedicionNoDefinido, idsOriglectura, nroPaginas, pageSize, campo, orden, codigo);
            }

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            MePtomedicionDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    if (tipoPunto != 4)
                    {
                        int iEmprenomb = dr.GetOrdinal(helper.Emprnomb);
                        if (!dr.IsDBNull(iEmprenomb)) entity.Emprnomb = dr.GetString(iEmprenomb);

                        int iOriglectnombre = dr.GetOrdinal(helper.Origlectnombre);
                        if (!dr.IsDBNull(iOriglectnombre)) entity.Origlectnombre = dr.GetString(iOriglectnombre);

                        int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                        if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                        int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                        if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                        int iTipoptomedinomb = dr.GetOrdinal(helper.Tipoptomedinomb);
                        if (!dr.IsDBNull(iTipoptomedinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);

                        int iAreaOperativa = dr.GetOrdinal(helper.AreaOperativa);
                        if (!dr.IsDBNull(iAreaOperativa)) entity.AreaOperativa = dr.GetString(iAreaOperativa);

                        int iNivelTension = dr.GetOrdinal(helper.NivelTension);
                        if (!dr.IsDBNull(iNivelTension)) entity.NivelTension = dr.GetDecimal(iNivelTension);

                        int iDesUbicacion = dr.GetOrdinal(helper.DesUbicacion);
                        if (!dr.IsDBNull(iDesUbicacion)) entity.DesUbicacion = dr.GetString(iDesUbicacion);

                        if (tipoPunto == 1)
                        {
                            int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                            int iareaNomb = dr.GetOrdinal(helper.Areanomb);
                            if (!dr.IsDBNull(iareaNomb)) entity.Areanomb = dr.GetString(iareaNomb);
                        }

                        if (tipoPunto == 3)
                        {
                            int iCliennomb = dr.GetOrdinal(helper.Cliennomb);
                            if (!dr.IsDBNull(iCliennomb)) entity.ClientNomb = dr.GetString(iCliennomb);

                            int iBarrnomb = dr.GetOrdinal(helper.Barranomb);
                            if (!dr.IsDBNull(iCliennomb)) entity.BarrNomb = dr.GetString(iBarrnomb);
                        }

                        try
                        {
                            int iCliennomb = dr.GetOrdinal(helper.Cliennomb);
                            if (!dr.IsDBNull(iCliennomb)) entity.ClientNomb = dr.GetString(iCliennomb);

                            int iPuntoConexion = dr.GetOrdinal(helper.PuntoConexion);
                            if (!dr.IsDBNull(iPuntoConexion)) entity.PuntoConexion = dr.GetString(iPuntoConexion);
                        }
                        catch { }
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int VerificarRelaciones(int ptoMedicion)
        {
            string sql = string.Format(helper.SqlVerificarRelaciones, ptoMedicion);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object result = dbProvider.ExecuteScalar(command);
            int suma = 0;
            if (result != null) suma = Convert.ToInt32(result);
            return suma;
        }

        public List<EqAreaDTO> ObtenerAreasFiltro()
        {
            List<EqAreaDTO> entitys = new List<EqAreaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerAreasFiltro);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqAreaDTO entity = new EqAreaDTO();

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public int VerificarExistencia(int equicodi, int origenlectcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlVerificarExistencia);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Origlectcodi, DbType.Int32, origenlectcodi);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public List<MePtomedicionDTO> ListarPotencia(string familia, int idOriglectura, string control)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();

            string sqlQuery = string.Format(helper.SqlListarPotencia, familia, idOriglectura);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                MePtomedicionDTO entity;

                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iPtomedibarranomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedibarranomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedibarranomb);
                    
                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);
                    
                    int iLastuser = dr.GetOrdinal(helper.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);
                    
                    int iLastdate = dr.GetOrdinal(helper.Lastdate);
                    if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

                    entity.Origlectnombre = control;
                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public MePtomedicionDTO ListarPotenciaEquipo(int ptomedicodi)
        {
            MePtomedicionDTO entity = new MePtomedicionDTO();
            string sqlQuery = string.Format(helper.SqlListarPotenciaEquipo, ptomedicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);                      

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {              
                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iPtomedibarranomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedibarranomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedibarranomb);
                    
                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);
                    
                    int iLastuser = dr.GetOrdinal(helper.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);
                    
                    int iLastdate = dr.GetOrdinal(helper.Lastdate);
                    if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);
                    
                    return entity;
                }
            }

            return entity;
        }

        public List<MePtomedicionDTO> ListarCostoVariableAGC()
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string sqlQuery = string.Format(helper.SqlListarCostoVariableAGC);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                MePtomedicionDTO entity;

                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iPtomedibarranomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedibarranomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedibarranomb);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iLastuser = dr.GetOrdinal(helper.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

                    int iLastdate = dr.GetOrdinal(helper.Lastdate);
                    if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public string ListarTipoGrupo(string puntoMedicion)
        {
            string sql = string.Format(helper.SqlListarTipoGrupo, puntoMedicion);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            object result = dbProvider.ExecuteScalar(command);
            string total = "";
            if (result != null) total = result.ToString();
            return total;
        }


        public MePtomedicionDTO GetByIdAgc(int ptomedicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdAgc);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            MePtomedicionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iPtomedibarranomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedibarranomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedibarranomb);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);
                }
            }
            return entity;
        }
        
        public List<MePtomedicionDTO> ListarControlCentralizado()
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarControlCentralizado);

            MePtomedicionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        
        public List<MePtomedicionDTO> GetByCriteria3(int origlectcodi, string grupocodi, string tipoinfocodis)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();

            string sql = string.Format(helper.SqlGetByCriteria3, origlectcodi, grupocodi, tipoinfocodis);
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
       
        public void UpdateMePtoMedicion(MePtomedicionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateMePtomedicion);

            dbProvider.AddInParameter(command, helper.Ptomedielenomb, DbType.String, entity.Ptomedielenomb);
            dbProvider.AddInParameter(command, helper.Ptomedibarranomb, DbType.String, entity.Ptomedibarranomb);
            dbProvider.AddInParameter(command, helper.Ptomedidesc, DbType.String, entity.Ptomedidesc);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);

            dbProvider.ExecuteNonQuery(command);
        }
                
        public void UpdateMePtoMedicionCVariable(MePtomedicionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateMePtomedicionCVariable);
            
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.String, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Ptomedibarranomb, DbType.String, entity.Ptomedibarranomb);
            dbProvider.AddInParameter(command, helper.Ptomedidesc, DbType.String, entity.Ptomedidesc);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<MePtomedicionDTO> ListByOriglectcodi(string origlectcodi, DateTime fechaIni, DateTime fechaFin)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();

            string sql = string.Format(helper.SqlListOrigenLectura, origlectcodi, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
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

        // Inicio de Agregado - Sistema de Compensaciones
        public List<string> LstGrillaHead(int pecacodi)
        {
            string sqlTotal = string.Format(helper.SqlObtenerMaximoOrden);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlTotal);
            object result = dbProvider.ExecuteScalar(command);
            int total = 0;
            if (result != null) total = Convert.ToInt32(result);

            /*  string query = "SELECT PTO.PTOMEDICODI,PTO.PTOMEDIELENOMB";
              for (int cont = 1; cont <= total; cont++)
              {
                  query = query + " ,MAX(CASE WHEN VC.PMEMOPORDEN = " + cont + " THEN GR.GRUPONOMB END) AS MOPE_NOMB_" + cont;
              }

              query = query + " FROM ME_PTOMEDICION PTO JOIN (SELECT * FROM VCE_PTOMED_MODOPE WHERE PECACODI = " + pecacodi + " ) VC ON PTO.PTOMEDICODI = VC.PTOMEDICODI JOIN PR_GRUPO GR ON VC.GRUPOCODI = GR.GRUPOCODI";
              query = query + " WHERE PTO.ORIGLECTCODI = 1 AND PTO.PTOMEDIESTADO='A'" + " AND VC.PECACODI = " + pecacodi;
              query = query + " GROUP BY PTO.PTOMEDICODI,PTO.PTOMEDIELENOMB";
              query = query + " ORDER BY 2";
              */

            // DSH 26-06-2017 : Se actualizo segun requerimiento
             string query = "SELECT  (CASE WHEN EQ.EQUIPADRE IS NULL  THEN '_NO DEFINIDO' ELSE CG.EQUINOMB END) AS CENTRAL";
            query = query + " , GG.GRUPONOMB AS GRUPO, PTO.PTOMEDICODI AS CODIGO, PTO.PTOMEDIELENOMB AS PUNTO_MEDICION";
            for (int cont = 1; cont <= total; cont++)
            {
                query = query + " ,MAX(CASE WHEN VC.PMEMOPORDEN = " + cont + " THEN MO.GRUPONOMB END) AS MODO_OPERACION_" + cont;
            }

            query = query + " FROM VCE_PTOMED_MODOPE VC";
            query = query + " JOIN (SELECT PTOMEDICODI, ORIGLECTCODI, PTOMEDIELENOMB, EQUICODI, GRUPOCODI, PTOMEDIESTADO FROM ME_PTOMEDICION) PTO ON VC.PTOMEDICODI = PTO.PTOMEDICODI";
            query = query + " JOIN PR_GRUPO MO ON VC.GRUPOCODI = MO.GRUPOCODI";
            query = query + " JOIN PR_GRUPO GG ON PTO.GRUPOCODI = GG.GRUPOCODI";
            query = query + " LEFT JOIN";
            query = query + " (SELECT GRUPOCODI, EMPRCODI, MAX(EQUIPADRE) EQUIPADRE";
            query = query + " FROM EQ_EQUIPO ";
            query = query + " WHERE FAMCODI IN (2,3) AND NVL(GRUPOCODI,-1) != -1  AND EQUIESTADO = 'A'";
            query = query + " GROUP BY GRUPOCODI, EMPRCODI";
            query = query + " UNION";
            query = query + " SELECT GRUPOCODI, EMPRCODI, MAX(EQUIPADRE) EQUIPADRE";
            query = query + " FROM EQ_EQUIPO";
            query = query + " WHERE FAMCODI IN (2,3) AND NVL(GRUPOCODI,-1) != -1  AND EQUIESTADO = 'B'";
            query = query + " AND GRUPOCODI NOT IN (SELECT DISTINCT GRUPOCODI";
            query = query + " FROM EQ_EQUIPO";
            query = query + " WHERE FAMCODI IN (2,3) AND NVL(GRUPOCODI,-1) != -1";
            query = query + " AND EQUIESTADO = 'A' )";
            query = query + " GROUP BY GRUPOCODI, EMPRCODI";
            query = query + " )EQ ON PTO.GRUPOCODI = EQ.GRUPOCODI";
            query = query + " LEFT JOIN EQ_EQUIPO CG ON  EQ.EQUIPADRE = CG.EQUICODI";
            query = query + " WHERE VC.PECACODI = " + pecacodi + " AND PTO.ORIGLECTCODI = 1 AND PTO.PTOMEDIESTADO='A' ";
            query = query + " GROUP BY PTO.PTOMEDICODI, PTO.PTOMEDIELENOMB, (CASE WHEN EQ.EQUIPADRE IS NULL  THEN '_NO DEFINIDO' ELSE CG.EQUINOMB END), GG.GRUPONOMB";
            query = query + " ORDER BY 1,2,4";
            
            command = dbProvider.GetSqlStringCommand(query);

            List<string> lstHead = new List<string>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    lstHead.Add(dr.GetName(i));
                }
            }
            return lstHead;
        }

        // LstGrillaBody Version 2
        public List<ComboCompensaciones> LstGrillaBody(int pecacodi)
        {
            string sqlTotal = string.Format(helper.SqlObtenerMaximoOrden);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlTotal);
            object result = dbProvider.ExecuteScalar(command);
            int total = 0;
            if (result != null) total = Convert.ToInt32(result);
            
            // Version 1
            /* string query = "SELECT PTO.PTOMEDICODI,PTO.PTOMEDIELENOMB";
            for (int cont = 1; cont <= total; cont++)
            {
                query = query + " ,MAX(CASE WHEN VC.PMEMOPORDEN = " + cont + " THEN GR.GRUPONOMB END) AS MOPE_NOMB_" + cont;
            }

            query = query + " FROM ME_PTOMEDICION PTO JOIN (SELECT * FROM VCE_PTOMED_MODOPE WHERE PECACODI = " + pecacodi + " ) VC ON PTO.PTOMEDICODI = VC.PTOMEDICODI JOIN PR_GRUPO GR ON VC.GRUPOCODI = GR.GRUPOCODI";
            query = query + " WHERE PTO.ORIGLECTCODI = 1 AND PTO.PTOMEDIESTADO='A'" + " AND VC.PECACODI = " + pecacodi;
            query = query + " GROUP BY PTO.PTOMEDICODI,PTO.PTOMEDIELENOMB";
            query = query + " ORDER BY 2";
            */

            // DSH 26-06-2017 : Se actualizo segun requerimiento
            // Version 2
            /* string query = "SELECT CG.GRUPONOMB AS CENTRAL, GG.GRUPONOMB AS GRUPO, PTO.PTOMEDICODI AS CODIGO, PTO.PTOMEDIELENOMB AS PUNTO_MEDICION";
            for (int cont = 1; cont <= total; cont++)
            {
                query = query + " ,MAX(CASE WHEN VC.PMEMOPORDEN = " + cont + " THEN MO.GRUPONOMB END) AS MODO_OPERACION_" + cont;
            }

            query = query + " FROM VCE_PTOMED_MODOPE VC";
            query = query + " JOIN (SELECT PTOMEDICODI, ORIGLECTCODI, PTOMEDIELENOMB, EQUICODI, GRUPOCODI, PTOMEDIESTADO FROM ME_PTOMEDICION) PTO ON VC.PTOMEDICODI = PTO.PTOMEDICODI";
            query = query + " JOIN PR_GRUPO GG ON PTO.GRUPOCODI = GG.GRUPOCODI";
            query = query + " JOIN PR_GRUPO CG ON GG.GRUPOPADRE = CG.GRUPOCODI";
            query = query + " JOIN PR_GRUPO MO ON VC.GRUPOCODI = MO.GRUPOCODI";
            query = query + " WHERE VC.PECACODI = " + pecacodi + " AND PTO.ORIGLECTCODI = 1 AND PTO.PTOMEDIESTADO='A' ";
            query = query + " GROUP BY PTO.PTOMEDICODI, PTO.PTOMEDIELENOMB, CG.GRUPONOMB, GG.GRUPONOMB";
            query = query + " ORDER BY 1,2,4";
            */
            // DSH 11-07-2017 : Se actualizo segun requerimiento
            string query = "SELECT  (CASE WHEN EQ.EQUIPADRE IS NULL  THEN '_NO DEFINIDO' ELSE CG.EQUINOMB END) AS CENTRAL";
            query = query + " , GG.GRUPONOMB AS GRUPO, PTO.PTOMEDICODI AS CODIGO, PTO.PTOMEDIELENOMB AS PUNTO_MEDICION";
            for (int cont = 1; cont <= total; cont++)
            {
                query = query + " ,MAX(CASE WHEN VC.PMEMOPORDEN = " + cont + " THEN MO.GRUPONOMB END) AS MODO_OPERACION_" + cont;
            }

            query = query + " FROM VCE_PTOMED_MODOPE VC";
            query = query + " JOIN (SELECT PTOMEDICODI, ORIGLECTCODI, PTOMEDIELENOMB, EQUICODI, GRUPOCODI, PTOMEDIESTADO FROM ME_PTOMEDICION) PTO ON VC.PTOMEDICODI = PTO.PTOMEDICODI";
            query = query + " JOIN PR_GRUPO MO ON VC.GRUPOCODI = MO.GRUPOCODI";
            query = query + " JOIN PR_GRUPO GG ON PTO.GRUPOCODI = GG.GRUPOCODI";
            query = query + " LEFT JOIN";
            query = query + " (SELECT GRUPOCODI, EMPRCODI, MAX(EQUIPADRE) EQUIPADRE";
            query = query + " FROM EQ_EQUIPO ";
            query = query + " WHERE FAMCODI IN (2,3) AND NVL(GRUPOCODI,-1) != -1  AND EQUIESTADO = 'A'";
            query = query + " GROUP BY GRUPOCODI, EMPRCODI";
            query = query + " UNION";
            query = query + " SELECT GRUPOCODI, EMPRCODI, MAX(EQUIPADRE) EQUIPADRE";
            query = query + " FROM EQ_EQUIPO";
            query = query + " WHERE FAMCODI IN (2,3) AND NVL(GRUPOCODI,-1) != -1  AND EQUIESTADO = 'B'";
            query = query + " AND GRUPOCODI NOT IN (SELECT DISTINCT GRUPOCODI";
            query = query + " FROM EQ_EQUIPO";
            query = query + " WHERE FAMCODI IN (2,3) AND NVL(GRUPOCODI,-1) != -1";
            query = query + " AND EQUIESTADO = 'A' )";
            query = query + " GROUP BY GRUPOCODI, EMPRCODI";
            query = query + " )EQ ON PTO.GRUPOCODI = EQ.GRUPOCODI";
            query = query + " LEFT JOIN EQ_EQUIPO CG ON  EQ.EQUIPADRE = CG.EQUICODI";
            query = query + " WHERE VC.PECACODI = " + pecacodi + " AND PTO.ORIGLECTCODI = 1 AND PTO.PTOMEDIESTADO='A' ";
            query = query + " GROUP BY PTO.PTOMEDICODI, PTO.PTOMEDIELENOMB, (CASE WHEN EQ.EQUIPADRE IS NULL  THEN '_NO DEFINIDO' ELSE CG.EQUINOMB END), GG.GRUPONOMB";
            query = query + " ORDER BY 1,2,4";
            command = dbProvider.GetSqlStringCommand(query);

            string strHtml = "";

            List<ComboCompensaciones> lstBody = new List<ComboCompensaciones>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                var columns = new List<string>();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    columns.Add(dr.GetName(i));
                }
                while (dr.Read())
                {
                    ComboCompensaciones entity = new ComboCompensaciones();
                    foreach (var item in columns)
                    {
                        if (strHtml.Equals(""))
                        {
                            //entity.Ptomedicodi = int.Parse(dr[item.Trim()].ToString());
                            entity.id = dr["CODIGO"].ToString();
                            strHtml = dr[item.Trim()].ToString();
                        }
                        else
                        {
                            strHtml = strHtml + "|" + dr[item.Trim()].ToString();
                        }
                    }
                    entity.name = strHtml;

                    lstBody.Add(entity);
                    strHtml = "";
                }
            }
            return lstBody;
        }

        public List<MePtomedicionDTO> ListPtoMedicionCompensaciones(int ptoMediCodi, int pecacodi)
        {
            string qCondicion = "";

            if (ptoMediCodi != 0)
            {
                qCondicion = " PTOMEDICODI = " + ptoMediCodi + " or ";
            }

            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string query = string.Format(helper.SqlListPtoMedicionCompensaciones, qCondicion, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            MePtomedicionDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = dr.GetInt32(iPtomedicodi);

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        // Fin de Agregado - Sistema de Compensaciones

        #region PR5
     
        public List<MePtomedicionDTO> ListarCentralByOriglectcodiAndFormato(string origlectcodi, string famcodi, int formatcodi)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string query = string.Format(helper.SqlListarCentralByOriglectcodi, origlectcodi, famcodi, formatcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            MePtomedicionDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iPtomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        /// <summary>
        /// Listar los puntos de medicion por origen de lectura y formato
        /// </summary>
        /// <param name="origlectcodi"></param>
        /// <param name="formatcodi"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListarPtoMedicionByOriglectcodiAndFormato(int origlectcodi, int formatcodi, DateTime fechaIni, DateTime fechaFin)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string query = string.Format(helper.SqlListarPtoMedicionByOriglectcodiAndFormato, origlectcodi, formatcodi, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            MePtomedicionDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));
                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);
                    int iPtomedidesc = dr.GetOrdinal(this.helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);
                    int ifamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(ifamcodi)) entity.Famcodi = Convert.ToInt16(dr.GetValue(ifamcodi));

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTipoinfocodi = dr.GetOrdinal(this.helper.Tipoinfocodi);
                    if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));
                    int iTipoinfoabrev = dr.GetOrdinal(helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);

                    int iTipoptomedicodi = dr.GetOrdinal(this.helper.Tipoptomedicodi);
                    if (!dr.IsDBNull(iTipoptomedicodi)) entity.Tipoptomedicodi = Convert.ToInt16(dr.GetValue(iTipoptomedicodi));
                    int iTipoptomedinomb = dr.GetOrdinal(helper.Tipoptomedinomb);
                    if (!dr.IsDBNull(iTipoptomedinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));
                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iGrupopadre = dr.GetOrdinal(this.helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));
                    int iCentral = dr.GetOrdinal(this.helper.Grupocentral);
                    if (!dr.IsDBNull(iCentral)) entity.Grupocentral = dr.GetString(iCentral);

                    int iHojaptoactivo = dr.GetOrdinal(this.helper.Hojaptoactivo);
                    if (!dr.IsDBNull(iHojaptoactivo)) entity.Hojaptoactivo = Convert.ToInt32(dr.GetValue(iHojaptoactivo));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }


        public List<MePtomedicionDTO> ListarByEquiOriglectcodi(int equipo, int origlectcodi, int lectcodi)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string query = string.Format(helper.SqlListarByEquiOriglectcodi, equipo, origlectcodi, lectcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            MePtomedicionDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int ifamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(ifamcodi)) entity.Famcodi = Convert.ToInt16(dr.GetValue(ifamcodi));

                    int iPtomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iTipoinfocodi = dr.GetOrdinal(this.helper.Tipoinfocodi);
                    if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

                    int iTipoinfoabrev = dr.GetOrdinal(helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);

                    int iTipoptomedicodi = dr.GetOrdinal(this.helper.Tipoptomedicodi);
                    if (!dr.IsDBNull(iTipoptomedicodi)) entity.Tipoptomedicodi = Convert.ToInt16(dr.GetValue(iTipoptomedicodi));

                    int iTipoptomedinomb = dr.GetOrdinal(helper.Tipoptomedinomb);
                    if (!dr.IsDBNull(iTipoptomedinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);

                    int iPtomedicalculado = dr.GetOrdinal(helper.Ptomedicalculado);
                    if (!dr.IsDBNull(iPtomedicalculado)) entity.PtomediCalculado = dr.GetString(iPtomedicalculado);

                    int iLectcodi = dr.GetOrdinal(this.helper.Lectcodi);
                    if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

                    int iOriglectcodi = dr.GetOrdinal(this.helper.Origlectcodi);
                    if (!dr.IsDBNull(iOriglectcodi)) entity.Origlectcodi = Convert.ToInt32(dr.GetValue(iOriglectcodi));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MePtomedicionDTO> ListarPuntosCalculados()
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();

            string sqlQuery = string.Format(helper.SqlListarPuntosCalculados);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtomedicionDTO entity = helper.Create(dr);

                    int iEmprabrev = dr.GetOrdinal(this.helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int ifamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(ifamcodi)) entity.Famcodi = Convert.ToInt16(dr.GetValue(ifamcodi));

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);
                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);
                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iTptomedinomb = dr.GetOrdinal(this.helper.Tptomedinomb);
                    if (!dr.IsDBNull(iTptomedinomb)) entity.Tptomedinomb = dr.GetString(iTptomedinomb);

                    int iTipoinfoabrev = dr.GetOrdinal(this.helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);

                    int iPtomedicalculado = dr.GetOrdinal(this.helper.Ptomedicalculado);
                    if (!dr.IsDBNull(iPtomedicalculado)) entity.PtomediCalculado = dr.GetString(iPtomedicalculado);

                    int iOriglectnombre = dr.GetOrdinal(helper.Origlectnombre);
                    if (!dr.IsDBNull(iOriglectnombre)) entity.Origlectnombre = dr.GetString(iOriglectnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MePtomedicionDTO> ListarPtoMedicionFromCalculado(string ptomedicalculado)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string query = string.Format(helper.SqlListarPtoMedicionFromCalculado, ptomedicalculado);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            MePtomedicionDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iTipoRelacioncodi = dr.GetOrdinal(this.helper.TipoRelacioncodi);
                    if (!dr.IsDBNull(iTipoRelacioncodi)) entity.TipoRelacioncodi = Convert.ToInt32(dr.GetValue(iTipoRelacioncodi));

                    int iPtomedicalculado = dr.GetOrdinal(this.helper.PtomedicodiCalculado);
                    if (!dr.IsDBNull(iPtomedicalculado)) entity.PtomedicodiCalculado = Convert.ToInt32(dr.GetValue(iPtomedicalculado));

                    int iPtomedicodidesccalculado = dr.GetOrdinal(this.helper.PtomedicodidescCalculado);
                    if (!dr.IsDBNull(iPtomedicodidesccalculado)) entity.PtomedicodiCalculadoDescrip = dr.GetString(iPtomedicodidesccalculado);

                    int iFactorOrigen = dr.GetOrdinal(this.helper.FactorOrigen);
                    if (!dr.IsDBNull(iFactorOrigen)) entity.FactorOrigen = dr.GetDecimal(iFactorOrigen);

                    int iEmprcodiOrigen = dr.GetOrdinal(this.helper.EmprcodiOrigen);
                    if (!dr.IsDBNull(iEmprcodiOrigen)) entity.EmprcodiOrigen = Convert.ToInt32(dr.GetValue(iEmprcodiOrigen));
                    int IEmprabrevOrigen = dr.GetOrdinal(this.helper.EmprabrevOrigen);
                    if (!dr.IsDBNull(IEmprabrevOrigen)) entity.EmprabrevOrigen = dr.GetString(IEmprabrevOrigen);
                    int iEmprnombOrigen = dr.GetOrdinal(this.helper.EmprnombOrigen);
                    if (!dr.IsDBNull(iEmprnombOrigen)) entity.EmprnombOrigen = dr.GetString(iEmprnombOrigen);

                    int iRelptocodi = dr.GetOrdinal(this.helper.Relptocodi);
                    if (!dr.IsDBNull(iRelptocodi)) entity.Relptocodi = Convert.ToInt32(dr.GetValue(iRelptocodi));

                    int iPtomedicodiOrigen = dr.GetOrdinal(this.helper.PtomedicodiOrigen);
                    if (!dr.IsDBNull(iPtomedicodiOrigen)) entity.PtomedicodiOrigen = Convert.ToInt32(dr.GetValue(iPtomedicodiOrigen));
                    int iPtomedibarranombOrigen = dr.GetOrdinal(this.helper.PtomedibarranombOrigen);
                    if (!dr.IsDBNull(iPtomedibarranombOrigen)) entity.PtomedibarranombOrigen = dr.GetString(iPtomedibarranombOrigen);
                    int iPtomedielenombOrigen = dr.GetOrdinal(this.helper.PtomedielenombOrigen);
                    if (!dr.IsDBNull(iPtomedielenombOrigen)) entity.PtomedielenombOrigen = dr.GetString(iPtomedielenombOrigen);
                    int iPtomedicodidescOrigen = dr.GetOrdinal(this.helper.PtomedicodidescOrigen);
                    if (!dr.IsDBNull(iPtomedicodidescOrigen)) entity.PtomedicodidescOrigen = dr.GetString(iPtomedicodidescOrigen);

                    int iEquipadreOrigen = dr.GetOrdinal(this.helper.EquipadreOrigen);
                    if (!dr.IsDBNull(iEquipadreOrigen)) entity.EquipadreOrigen = Convert.ToInt32(dr.GetValue(iEquipadreOrigen));
                    int iCentralOrigen = dr.GetOrdinal(this.helper.CentralOrigen);
                    if (!dr.IsDBNull(iCentralOrigen)) entity.CentralOrigen = dr.GetString(iCentralOrigen);

                    int iEquicodiOrigen = dr.GetOrdinal(this.helper.EquicodiOrigen);
                    if (!dr.IsDBNull(iEquicodiOrigen)) entity.EquicodiOrigen = Convert.ToInt32(dr.GetValue(iEquicodiOrigen));
                    int iEquinombOrigen = dr.GetOrdinal(this.helper.EquinombOrigen);
                    if (!dr.IsDBNull(iEquinombOrigen)) entity.EquinombOrigen = dr.GetString(iEquinombOrigen);
                    int iEquiabrevOrigen = dr.GetOrdinal(this.helper.EquiabrevOrigen);
                    if (!dr.IsDBNull(iEquiabrevOrigen)) entity.EquiabrevOrigen = dr.GetString(iEquiabrevOrigen);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));
                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFamcodiOrigen = dr.GetOrdinal(this.helper.FamcodiOrigen);
                    if (!dr.IsDBNull(iFamcodiOrigen)) entity.FamcodiOrigen = Convert.ToInt32(dr.GetValue(iFamcodiOrigen));
                    int iFamnombOrigen = dr.GetOrdinal(this.helper.FamnombOrigen);
                    if (!dr.IsDBNull(iFamnombOrigen)) entity.FamnombOrigen = dr.GetString(iFamnombOrigen);
                    int iFamabrevOrigen = dr.GetOrdinal(this.helper.FamabrevOrigen);
                    if (!dr.IsDBNull(iFamabrevOrigen)) entity.FamabrevOrigen = dr.GetString(iFamabrevOrigen);

                    int iOriglectcodi = dr.GetOrdinal(this.helper.Origlectcodi);
                    if (!dr.IsDBNull(iOriglectcodi)) entity.Origlectcodi = Convert.ToInt32(dr.GetValue(iOriglectcodi));
                    int iOriglectnombre = dr.GetOrdinal(this.helper.Origlectnombre);
                    if (!dr.IsDBNull(iOriglectnombre)) entity.Origlectnombre = dr.GetString(iOriglectnombre);

                    int iLectcodi = dr.GetOrdinal(this.helper.Lectcodi);
                    if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));
                    int iLectnomb = dr.GetOrdinal(this.helper.Lectnomb);
                    if (!dr.IsDBNull(iLectnomb)) entity.Lectnomb = dr.GetString(iLectnomb);

                    int iTipoinfocodi = dr.GetOrdinal(this.helper.Tipoinfocodi);
                    if (!dr.IsDBNull(iTipoinfocodi)) entity.TipoinfocodiOrigen = Convert.ToInt32(dr.GetValue(iTipoinfocodi));
                    int iTipoinfoabrev = dr.GetOrdinal(this.helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);

                    int iTgenercodi = dr.GetOrdinal(this.helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

                    int iPtomediCalculado = dr.GetOrdinal(this.helper.Ptomedicalculado);
                    if (!dr.IsDBNull(iPtomediCalculado)) entity.PtomediCalculado = dr.GetString(iPtomediCalculado);

                    int iTipoptomedicodi = dr.GetOrdinal(this.helper.Tipoptomedicodi);
                    if (!dr.IsDBNull(iTipoptomedicodi)) entity.Tipoptomedicodi = Convert.ToInt16(dr.GetValue(iTipoptomedicodi));
                    int iTipoptomedinomb = dr.GetOrdinal(helper.Tipoptomedinomb);
                    if (!dr.IsDBNull(iTipoptomedinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);

                    int iRepptotabmed = dr.GetOrdinal(this.helper.Repptotabmed);
                    if (!dr.IsDBNull(iRepptotabmed)) entity.Repptotabmed = Convert.ToInt32(dr.GetValue(iRepptotabmed));

                    int iFactorPotencia = dr.GetOrdinal(this.helper.FactorPotencia);
                    if (!dr.IsDBNull(iFactorPotencia)) entity.FactorPotencia = dr.GetDecimal(iFactorPotencia);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        #endregion


        #region SIOSEIN

        public List<MePtomedicionDTO> ListPtoMedicionMeLectura(int origlectcodi, int lectcodi, int tipoinfocodi)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();

            string sql = string.Format(helper.SqlListPtoMedicionMeLectura, origlectcodi, lectcodi, tipoinfocodi);
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

        public List<MePtomedicionDTO> GetByCriteria2(string equicodi, string origlectcodi)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string query = string.Format(helper.SqlGetByCriteria2, equicodi, origlectcodi);
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

        #endregion
    
        //-Pruebas aleatorias
        public int ObtenerPtomedicionSorteo(DateTime fecha, int origlectcodi)
        {
            int ptomedicodi = -1;
            string fechaYmd = fecha.ToString(ConstantesBase.FormatoFecha);

            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string query = string.Format(helper.SqlObtenerMedicionSorteo, fechaYmd, origlectcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            MePtomedicionDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) ptomedicodi = dr.GetInt32(iPtomedicodi);
                }
            }

            return ptomedicodi;
        }



        #region Transferencia de Equipos
        public List<MePtomedicionDTO> ListarPtosMedicionXEmpresa(int idEmpresa)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string strComando = string.Format(helper.ListarPtosMedicionXEmpresa, idEmpresa);
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
        #endregion


        #region MigracionSGOCOES-GrupoB
        public List<MePtomedicionDTO> ObtenerPuntoMedicionExtranet(int idEquipo)
        {
            string query = string.Format(helper.SqlObtenerPuntoMedicionExtranet, idEquipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            MePtomedicionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = dr.GetInt32(iPtomedicodi);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    entity.Ptomedidesc = entity.Ptomedicodi.ToString().PadLeft(5, ' ') + " - " + entity.Equinomb + " - " + entity.Equiabrev;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MePtomedicionDTO> GetListaPuntoMedicionPorEmpresa(string emprcodi, DateTime fechaPeriodoIni, DateTime fechaPeriodoFin)
        {
            var query = string.Format(helper.SqlGetListaPuntoMedicionXEmpresa, emprcodi, fechaPeriodoIni.ToString(ConstantesBase.FormatoFecha), fechaPeriodoFin.ToString(ConstantesBase.FormatoFecha));

            using (var command = dbProvider.GetSqlStringCommand(query))
            {
                List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();

                MePtomedicionDTO entity;

                using (var dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        entity = new MePtomedicionDTO();

                        var iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                        if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                        int iOriglectcodi = dr.GetOrdinal(this.helper.Origlectcodi);
                        if (!dr.IsDBNull(iOriglectcodi)) entity.Origlectcodi = Convert.ToInt32(dr.GetValue(iOriglectcodi));

                        int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                        if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                        int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                        if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                        int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                        if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                        int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                        if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                        int iEquitension = dr.GetOrdinal(helper.Equitension);
                        if (!dr.IsDBNull(iEquitension)) entity.Equitension = Convert.ToDouble(dr.GetValue(iEquitension));

                        int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                        if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                        int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                        if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                        int iAreaOperativa = dr.GetOrdinal(helper.AreaOperativa);
                        if (!dr.IsDBNull(iAreaOperativa)) entity.AreaOperativa = dr.GetString(iAreaOperativa);

                        int iPtomediestado = dr.GetOrdinal(this.helper.Ptomediestado);
                        if (!dr.IsDBNull(iPtomediestado)) entity.Ptomediestado = dr.GetString(iPtomediestado);

                        entitys.Add(entity);
                    }
                }
                return entitys;
            }
        }

        #endregion

        #region Mejoras IEOD

        public List<MePtomedicionDTO> ListarPtomedicionByOriglectcodi(string origlectcodi, string famcodi, int emprcodi)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string query = string.Format(helper.SqlListarPtomedicionByOriglectcodi, origlectcodi, famcodi, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            MePtomedicionDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iPtomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MePtomedicionDTO> ListarPtomedicionDespachoAntiguo(int emprcodi)
        {

            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string query = string.Format(helper.SqlListarPtomedicionDespachoAntiguo, emprcodi);
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

        #endregion

        #region Titularidad-Instalaciones-Empresas

        public List<MePtomedicionDTO> ListarPtomedicionByMigracodi(int idMigracion)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string strComando = string.Format(helper.SqlListarPtomedicionByMigracodi, idMigracion);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtomedicionDTO entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedielenomb = dr.GetOrdinal(this.helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprnombOrigen = dr.GetOrdinal(this.helper.EmprnombOrigen);
                    if (!dr.IsDBNull(iEmprnombOrigen)) entity.EmprnombOrigen = dr.GetString(iEmprnombOrigen);

                    int iOriglectnombre = dr.GetOrdinal(this.helper.Origlectnombre);
                    if (!dr.IsDBNull(iOriglectnombre)) entity.Origlectnombre = dr.GetString(iOriglectnombre);

                    int iOriglectcodi = dr.GetOrdinal(this.helper.Origlectcodi);
                    if (!dr.IsDBNull(iOriglectcodi)) entity.Origlectcodi = Convert.ToInt32(dr.GetValue(iOriglectcodi));

                    int iPtomediestado = dr.GetOrdinal(this.helper.Ptomediestado);
                    if (!dr.IsDBNull(iPtomediestado)) entity.Ptomediestado = dr.GetString(iPtomediestado);

                    entitys.Add(entity);
                }

            }

            return entitys;
        }

        #endregion

        #region FIT - Aplicativo VTD

        public List<MePtomedicionDTO> GetByIdCliente(int clientecodi)
        {
            List<MePtomedicionDTO> entities = new List<MePtomedicionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdCliente);
            dbProvider.AddInParameter(command, helper.Clientecodi, DbType.Int32, clientecodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

        public List<MePtomedicionDTO> ListarPtoDuplicadoTransferencia(int clientecodi, int barracodi, int origen, int tipopto)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string sqlQuery = string.Format(helper.SqlGetPtoDuplicadoTransferencia, clientecodi, barracodi, origen, tipopto);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {

                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MePtomedicionDTO> GetByIdClienteBarraMePtomedicion(int? idEmpresa, int? cliente, int? barra, int idOrigenLectura)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string sqlQuery = string.Format(helper.SqlGetByIdClienteBarraMePtomedicion, idEmpresa, cliente, barra, idOrigenLectura);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

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

        #region Numerales Datos Base
        public List<MePtomedicionDTO> ListaNumerales_DatosBase_5_8_1(string fechaIni, string fechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBase_5_8_1, fechaIni, fechaFin);

            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtomedicionDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iOsicodi = dr.GetOrdinal(helper.Osicodi);
                    if (!dr.IsDBNull(iOsicodi)) entity.Osicodi = dr.GetString(iOsicodi);

                    int iValor = dr.GetOrdinal(helper.Valor);
                    if (!dr.IsDBNull(iValor)) entity.Valor = dr.GetDecimal(iValor);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);
                    
                    int iPtomedicalculado = dr.GetOrdinal(helper.Ptomedicalculado);
                    if (!dr.IsDBNull(iPtomedicalculado)) entity.PtomediCalculado = dr.GetString(iPtomedicalculado);

                    int iClientecodi = dr.GetOrdinal(helper.Clientecodi);
                    if (!dr.IsDBNull(iClientecodi)) entity.Clientecodi = Convert.ToInt32(dr.GetValue(iClientecodi));

                    int iBarrcodi = dr.GetOrdinal(helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        public List<MePtomedicionDTO> ListaNumerales_DatosBase_5_8_2(string fechaIni, string fechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBase_5_8_2, fechaIni, fechaFin);

            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtomedicionDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iOsicodi = dr.GetOrdinal(helper.Osicodi);
                    if (!dr.IsDBNull(iOsicodi)) entity.Osicodi = dr.GetString(iOsicodi);

                    int iValor = dr.GetOrdinal(helper.Valor);
                    if (!dr.IsDBNull(iValor)) entity.Valor = dr.GetDecimal(iValor);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);

                    int iPtomedicalculado = dr.GetOrdinal(helper.Ptomedicalculado);
                    if (!dr.IsDBNull(iPtomedicalculado)) entity.PtomediCalculado = dr.GetString(iPtomedicalculado);

                    int iClientecodi = dr.GetOrdinal(helper.Clientecodi);
                    if (!dr.IsDBNull(iClientecodi)) entity.Clientecodi = Convert.ToInt32(dr.GetValue(iClientecodi));

                    int iBarrcodi = dr.GetOrdinal(helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        public List<MePtomedicionDTO> ListaPtoUsuariosLibres(int tipoempresa)
        {
            string sqlQuery = string.Format(this.helper.SqlListaPtoUsuariosLibres, tipoempresa);

            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtomedicionDTO entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = dr.GetInt32(iPtomedicodi);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iEmprruc = dr.GetOrdinal(helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    //int iValor = dr.GetOrdinal(helper.Valor);
                    //if (!dr.IsDBNull(iValor)) entity.Valor = dr.GetDecimal(iValor);

                    int iEquitension = dr.GetOrdinal(helper.Equitension);
                    if (!dr.IsDBNull(iEquitension)) entity.Equitension = dr.GetDouble(iEquitension);



                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        #endregion

        #region MDCOES

        public MePtomedicionDTO GetByRecurcodi(int miRecurcodi, int tipo)
        {
            MePtomedicionDTO entity = new MePtomedicionDTO();
            string sqlQuery = "";
            if (tipo == 62)
                sqlQuery = string.Format(helper.SqlListarPorRecurcodiTermo, miRecurcodi);
            if (tipo == 63)
                sqlQuery = string.Format(helper.SqlListarPorRecurcodiHidro, miRecurcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    return entity;
                }
            }

            return entity;
        }
        #endregion

        #region Demanda DPO - Iteracion 2
        public List<MePtomedicionDTO> ListaPuntoMedicionByLista(string puntos)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();

            string sqlQuery = string.Format(helper.SqlListaPuntoMedicionByLista, puntos);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                MePtomedicionDTO entity;

                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<MePtomedicionDTO> ListaPuntoByEquipo(int equipo)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();

            string sqlQuery = string.Format(helper.SqlListaPuntoByEquipo, equipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                MePtomedicionDTO entity;

                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<MePtomedicionDTO> ListaPuntoByOrigenEmpresa(int origen, int empresa)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();

            string sqlQuery = string.Format(helper.SqlListaPuntoByOrigenEmpresa, origen, empresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                MePtomedicionDTO entity;

                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<MePtomedicionDTO> ListaPuntoSicliByEmpresa(int origen, int empresa)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();

            string sqlQuery = string.Format(helper.SqlListaPuntoSicliByEmpresa, origen, empresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                MePtomedicionDTO entity;

                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }
        #endregion

        #region Notificacion de Puntos Modificados

        public List<MePtomedicionDTO> ListadoPtoMedicionModificados(DateTime dtFechaInicio, DateTime dtFechaFin)
        {
            string query = string.Format(helper.SqlPtoMedicionModificados, dtFechaInicio.ToString("dd-MM-yyyy HH:mm"), dtFechaFin.ToString("dd-MM-yyyy HH:mm"));
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtomedicionDTO entity = new MePtomedicionDTO();

                    int iPtomedibarranomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedibarranomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedibarranomb);

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTptomedinomb = dr.GetOrdinal(helper.Tptomedinomb);
                    if (!dr.IsDBNull(iTptomedinomb)) entity.Tptomedinomb = dr.GetString(iTptomedinomb);

                    int iOriglectnombre = dr.GetOrdinal(helper.Origlectnombre);
                    if (!dr.IsDBNull(iOriglectnombre)) entity.Origlectnombre = dr.GetString(iOriglectnombre);

                    int iPtomediestado = dr.GetOrdinal(helper.Ptomediestado);
                    if (!dr.IsDBNull(iPtomediestado)) entity.Ptomediestado = dr.GetString(iPtomediestado);

                    int iLastDate = dr.GetOrdinal(helper.Lastdate);
                    if (!dr.IsDBNull(iLastDate)) entity.Lastdate = dr.GetDateTime(iLastDate);

                    int iLastUser = dr.GetOrdinal(helper.Lastuser);
                    if (!dr.IsDBNull(iLastUser)) entity.Lastuser = dr.GetString(iLastUser);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        #endregion

        public List<MePtomedicionDTO> ObtenerPuntosMedicionReporte(int idReporte, int tipoinfocodi)
        {
            string query = string.Format(helper.SqlObtenerPuntosMedicionReporte, idReporte, tipoinfocodi);
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtomedicionDTO entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iTipoinfocodi = dr.GetOrdinal(helper.Tipoinfocodi);
                    if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iAreaOperativa = dr.GetOrdinal(helper.AreaOperativa);
                    if (!dr.IsDBNull(iAreaOperativa)) entity.AreaOperativa = dr.GetString(iAreaOperativa);

                    int iCanales = dr.GetOrdinal(helper.Canales);
                    if (!dr.IsDBNull(iCanales)) entity.Canales = dr.GetString(iCanales);

                    int iNivelTension = dr.GetOrdinal(helper.NivelTension);
                    if (!dr.IsDBNull(iNivelTension)) entity.NivelTension = dr.GetDecimal(iNivelTension);

                    int iTipoinfoabrev = dr.GetOrdinal(helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);

                    int iOrden = dr.GetOrdinal(helper.Orden);
                    if (!dr.IsDBNull(iOrden)) entity.Orden = Convert.ToInt32(dr.GetValue(iOrden));

                    int iRepptonomb = dr.GetOrdinal(helper.Repptonomb);
                    if (!dr.IsDBNull(iRepptonomb)) entity.Repptonomb = dr.GetString(iRepptonomb);

                    int iColorcelda = dr.GetOrdinal(helper.Colorcelda);
                    if (!dr.IsDBNull(iColorcelda)) entity.Colorcelda = dr.GetString(iColorcelda);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeMedicion96DTO> LeerMedidores(DateTime fechaInicio)
        {
            var lista = new List<MeMedicion96DTO>();
            String query = String.Format(helper.SqlLeerMedidores, fechaInicio.ToString("yyyy-MM-dd"));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new MeMedicion96DTO();
                    entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("PTOMEDICODI")));
                    entity.H1 = dr.IsDBNull(dr.GetOrdinal("H1")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H1"));
                    entity.H2 = dr.IsDBNull(dr.GetOrdinal("H2")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H2"));
                    entity.H3 = dr.IsDBNull(dr.GetOrdinal("H3")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H3"));
                    entity.H4 = dr.IsDBNull(dr.GetOrdinal("H4")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H4"));
                    entity.H5 = dr.IsDBNull(dr.GetOrdinal("H5")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H5"));
                    entity.H6 = dr.IsDBNull(dr.GetOrdinal("H6")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H6"));
                    entity.H7 = dr.IsDBNull(dr.GetOrdinal("H7")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H7"));
                    entity.H8 = dr.IsDBNull(dr.GetOrdinal("H8")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H8"));
                    entity.H9 = dr.IsDBNull(dr.GetOrdinal("H9")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H9"));
                    entity.H10 = dr.IsDBNull(dr.GetOrdinal("H10")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H10"));
                    entity.H11 = dr.IsDBNull(dr.GetOrdinal("H11")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H11"));
                    entity.H12 = dr.IsDBNull(dr.GetOrdinal("H12")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H12"));
                    entity.H13 = dr.IsDBNull(dr.GetOrdinal("H13")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H13"));
                    entity.H14 = dr.IsDBNull(dr.GetOrdinal("H14")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H14"));
                    entity.H15 = dr.IsDBNull(dr.GetOrdinal("H15")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H15"));
                    entity.H16 = dr.IsDBNull(dr.GetOrdinal("H16")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H16"));
                    entity.H17 = dr.IsDBNull(dr.GetOrdinal("H17")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H17"));
                    entity.H18 = dr.IsDBNull(dr.GetOrdinal("H18")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H18"));
                    entity.H19 = dr.IsDBNull(dr.GetOrdinal("H19")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H19"));
                    entity.H20 = dr.IsDBNull(dr.GetOrdinal("H20")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H20"));
                    entity.H21 = dr.IsDBNull(dr.GetOrdinal("H21")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H21"));
                    entity.H22 = dr.IsDBNull(dr.GetOrdinal("H22")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H22"));
                    entity.H23 = dr.IsDBNull(dr.GetOrdinal("H23")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H23"));
                    entity.H24 = dr.IsDBNull(dr.GetOrdinal("H24")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H24"));
                    entity.H25 = dr.IsDBNull(dr.GetOrdinal("H25")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H25"));
                    entity.H26 = dr.IsDBNull(dr.GetOrdinal("H26")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H26"));
                    entity.H27 = dr.IsDBNull(dr.GetOrdinal("H27")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H27"));
                    entity.H28 = dr.IsDBNull(dr.GetOrdinal("H28")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H28"));
                    entity.H29 = dr.IsDBNull(dr.GetOrdinal("H29")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H29"));
                    entity.H30 = dr.IsDBNull(dr.GetOrdinal("H30")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H30"));
                    entity.H31 = dr.IsDBNull(dr.GetOrdinal("H31")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H31"));
                    entity.H32 = dr.IsDBNull(dr.GetOrdinal("H32")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H32"));
                    entity.H33 = dr.IsDBNull(dr.GetOrdinal("H33")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H33"));
                    entity.H34 = dr.IsDBNull(dr.GetOrdinal("H34")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H34"));
                    entity.H35 = dr.IsDBNull(dr.GetOrdinal("H35")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H35"));
                    entity.H36 = dr.IsDBNull(dr.GetOrdinal("H36")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H36"));
                    entity.H37 = dr.IsDBNull(dr.GetOrdinal("H37")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H37"));
                    entity.H38 = dr.IsDBNull(dr.GetOrdinal("H38")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H38"));
                    entity.H39 = dr.IsDBNull(dr.GetOrdinal("H39")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H39"));
                    entity.H40 = dr.IsDBNull(dr.GetOrdinal("H40")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H40"));
                    entity.H41 = dr.IsDBNull(dr.GetOrdinal("H41")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H41"));
                    entity.H42 = dr.IsDBNull(dr.GetOrdinal("H42")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H42"));
                    entity.H43 = dr.IsDBNull(dr.GetOrdinal("H43")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H43"));
                    entity.H44 = dr.IsDBNull(dr.GetOrdinal("H44")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H44"));
                    entity.H45 = dr.IsDBNull(dr.GetOrdinal("H45")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H45"));
                    entity.H46 = dr.IsDBNull(dr.GetOrdinal("H46")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H46"));
                    entity.H47 = dr.IsDBNull(dr.GetOrdinal("H47")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H47"));
                    entity.H48 = dr.IsDBNull(dr.GetOrdinal("H48")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H48"));
                    entity.H49 = dr.IsDBNull(dr.GetOrdinal("H49")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H49"));
                    entity.H50 = dr.IsDBNull(dr.GetOrdinal("H50")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H50"));
                    entity.H51 = dr.IsDBNull(dr.GetOrdinal("H51")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H51"));
                    entity.H52 = dr.IsDBNull(dr.GetOrdinal("H52")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H52"));
                    entity.H53 = dr.IsDBNull(dr.GetOrdinal("H53")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H53"));
                    entity.H54 = dr.IsDBNull(dr.GetOrdinal("H54")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H54"));
                    entity.H55 = dr.IsDBNull(dr.GetOrdinal("H55")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H55"));
                    entity.H56 = dr.IsDBNull(dr.GetOrdinal("H56")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H56"));
                    entity.H57 = dr.IsDBNull(dr.GetOrdinal("H57")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H57"));
                    entity.H58 = dr.IsDBNull(dr.GetOrdinal("H58")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H58"));
                    entity.H59 = dr.IsDBNull(dr.GetOrdinal("H59")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H59"));
                    entity.H60 = dr.IsDBNull(dr.GetOrdinal("H60")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H60"));
                    entity.H61 = dr.IsDBNull(dr.GetOrdinal("H61")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H61"));
                    entity.H62 = dr.IsDBNull(dr.GetOrdinal("H62")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H62"));
                    entity.H63 = dr.IsDBNull(dr.GetOrdinal("H63")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H63"));
                    entity.H64 = dr.IsDBNull(dr.GetOrdinal("H64")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H64"));
                    entity.H65 = dr.IsDBNull(dr.GetOrdinal("H65")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H65"));
                    entity.H66 = dr.IsDBNull(dr.GetOrdinal("H66")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H66"));
                    entity.H67 = dr.IsDBNull(dr.GetOrdinal("H67")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H67"));
                    entity.H68 = dr.IsDBNull(dr.GetOrdinal("H68")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H68"));
                    entity.H69 = dr.IsDBNull(dr.GetOrdinal("H69")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H69"));
                    entity.H70 = dr.IsDBNull(dr.GetOrdinal("H70")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H70"));
                    entity.H71 = dr.IsDBNull(dr.GetOrdinal("H71")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H71"));
                    entity.H72 = dr.IsDBNull(dr.GetOrdinal("H72")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H72"));
                    entity.H73 = dr.IsDBNull(dr.GetOrdinal("H73")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H73"));
                    entity.H74 = dr.IsDBNull(dr.GetOrdinal("H74")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H74"));
                    entity.H75 = dr.IsDBNull(dr.GetOrdinal("H75")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H75"));
                    entity.H76 = dr.IsDBNull(dr.GetOrdinal("H76")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H76"));
                    entity.H77 = dr.IsDBNull(dr.GetOrdinal("H77")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H77"));
                    entity.H78 = dr.IsDBNull(dr.GetOrdinal("H78")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H78"));
                    entity.H79 = dr.IsDBNull(dr.GetOrdinal("H79")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H79"));
                    entity.H80 = dr.IsDBNull(dr.GetOrdinal("H80")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H80"));
                    entity.H81 = dr.IsDBNull(dr.GetOrdinal("H81")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H81"));
                    entity.H82 = dr.IsDBNull(dr.GetOrdinal("H82")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H82"));
                    entity.H83 = dr.IsDBNull(dr.GetOrdinal("H83")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H83"));
                    entity.H84 = dr.IsDBNull(dr.GetOrdinal("H84")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H84"));
                    entity.H85 = dr.IsDBNull(dr.GetOrdinal("H85")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H85"));
                    entity.H86 = dr.IsDBNull(dr.GetOrdinal("H86")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H86"));
                    entity.H87 = dr.IsDBNull(dr.GetOrdinal("H87")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H87"));
                    entity.H88 = dr.IsDBNull(dr.GetOrdinal("H88")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H88"));
                    entity.H89 = dr.IsDBNull(dr.GetOrdinal("H89")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H89"));
                    entity.H90 = dr.IsDBNull(dr.GetOrdinal("H90")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H90"));
                    entity.H91 = dr.IsDBNull(dr.GetOrdinal("H91")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H91"));
                    entity.H92 = dr.IsDBNull(dr.GetOrdinal("H92")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H92"));
                    entity.H93 = dr.IsDBNull(dr.GetOrdinal("H93")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H93"));
                    entity.H94 = dr.IsDBNull(dr.GetOrdinal("H94")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H94"));
                    entity.H95 = dr.IsDBNull(dr.GetOrdinal("H95")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H95"));
                    entity.H96 = dr.IsDBNull(dr.GetOrdinal("H96")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H96"));
                    entity.Tipoinfocodidesc = dr.GetString(dr.GetOrdinal("TIPOINFODESC"));
                    entity.TptoMediCodi = dr.GetInt32(dr.GetOrdinal("TPTOMEDICODI"));
                    entity.tptomedinomb = dr.GetString(dr.GetOrdinal("TPTOMEDINOMB"));

                    lista.Add(entity);
                }
            }
            return lista;
        }

        public List<MePtomedicionDTO> LeerPtoMedicionHidrologia()
        {

            var lista = new List<MePtomedicionDTO>();
            String query = String.Format(helper.SqlLeerPtoMedicionHidrologia);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new MePtomedicionDTO();

                    entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("PTOMEDICODI")));
                    entity.Ptomedidesc = Convert.ToString(dr.GetValue(dr.GetOrdinal("PTOMEDIDESC")));
                    entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("TIPOINFOCODI")));
                    entity.Emprnomb = Convert.ToString(dr.GetValue(dr.GetOrdinal("EMPRNOMB")));

                    lista.Add(entity);
                }
            }

            return lista;
        }


        public List<MePtomedicionDTO> ObtenerDatosHidrologia(DateTime dtFechaInicio, DateTime dtFechaFin)
        {
            var lista = new List<MePtomedicionDTO>();
            String query = String.Format(helper.SqlDatosHidrologia, dtFechaInicio.ToString("yyyy-MM-dd"), dtFechaFin.ToString("yyyy-MM-dd"));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new MePtomedicionDTO();

                    entity.Medifecha = Convert.ToDateTime(dr.GetValue(dr.GetOrdinal("MEDIFECHA")));
                    entity.Ptomedidesc = Convert.ToString(dr.GetValue(dr.GetOrdinal("PTOMEDIDESC")));
                    entity.Tipoinfodesc = Convert.ToString(dr.GetValue(dr.GetOrdinal("TIPOINFODESC")));
                    entity.H1 = dr.IsDBNull(dr.GetOrdinal("H1")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H1"));
                    lista.Add(entity);
                }
            }
            return lista;
        }

        #region CPPA.2024.ASSETEC
        public List<MePtomedicionDTO> ListaCentralesPMPO(int empresa)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string query = string.Format(helper.SqlListaCentralesPMPO, empresa);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            MePtomedicionDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iTgenercodi = dr.GetOrdinal(helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

                    int iGrupotipocogen = dr.GetOrdinal(helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MePtomedicionDTO> ListaBarrasPMPO()
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string query = string.Format(helper.SqlListaBarrasPMPO);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            MePtomedicionDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        #endregion

    }
}
