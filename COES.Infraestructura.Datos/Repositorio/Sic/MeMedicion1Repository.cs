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
    /// Clase de acceso a datos de la tabla ME_MEDICION1
    /// </summary>
    public class MeMedicion1Repository : RepositoryBase, IMeMedicion1Repository
    {
        public MeMedicion1Repository(string strConn)
            : base(strConn)
        {
        }

        MeMedicion1Helper helper = new MeMedicion1Helper();

        public void Save(MeMedicion1DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            entity.Tipoptomedicodi = entity.Tipoptomedicodi > 0 ? entity.Tipoptomedicodi : -1;
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, entity.Medifecha);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Tipoptomedicodi, DbType.Int32, entity.Tipoptomedicodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int lectcodi, DateTime medifecha, int tipoinfocodi, int ptomedicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, lectcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, medifecha);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeMedicion1DTO GetById(int lectcodi, DateTime medifecha, int tipoinfocodi, int ptomedicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, lectcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, medifecha);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            MeMedicion1DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeMedicion1DTO> List()
        {
            List<MeMedicion1DTO> entitys = new List<MeMedicion1DTO>();
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

        public List<MeMedicion1DTO> GetByCriteria2(DateTime fechaInicio, DateTime fechaFin, int? idEmpresa, int? idGrupo, int? idTipoCombustible)
        {
            List<MeMedicion1DTO> entitys = new List<MeMedicion1DTO>();

            string sql = String.Format(helper.SqlGetByCriteria, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), idEmpresa, idGrupo, idTipoCombustible);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion1DTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iTipoinfodesc = dr.GetOrdinal(helper.Tipoinfodesc);
                    if (!dr.IsDBNull(iTipoinfodesc)) entity.Tipoinfodesc = dr.GetString(iTipoinfodesc);

                    entity.IndInformo = ConstantesBase.SI;

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iNota = dr.GetOrdinal(helper.Nota);
                    if (!dr.IsDBNull(iNota)) entity.Nota = dr.GetString(iNota);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiEmpresaDTO> ObtenerEmpresasStock()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEmpresasStock);
            SiEmpresaHelper empresahelper = new SiEmpresaHelper();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(empresahelper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(empresahelper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }


            return entitys;
        }

        public List<PrGrupoDTO> ObtenerGruposStock(int idEmpresa)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerGruposStock);
            PrGrupoHelper grupoHelper = new PrGrupoHelper();

            dbProvider.AddInParameter(command, grupoHelper.Emprcodi, DbType.Int32, idEmpresa);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iPtomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.PtoMediCodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iGruponomb = dr.GetOrdinal(grupoHelper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion1DTO> ObtenerTipoCombustible()
        {
            List<MeMedicion1DTO> entitys = new List<MeMedicion1DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerTipoCombustible);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion1DTO entity = new MeMedicion1DTO();

                    int iTipoinfocodi = dr.GetOrdinal(helper.Tipoinfocodi);
                    if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

                    int iTipoinfodesc = dr.GetOrdinal(helper.Tipoinfodesc);
                    if (!dr.IsDBNull(iTipoinfodesc)) entity.Tipoinfodesc = dr.GetString(iTipoinfodesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion1DTO> ObtenerEstructura(int? idEmpresa, int? idGrupo, int? idTipoCombustible)
        {
            List<MeMedicion1DTO> entitys = new List<MeMedicion1DTO>();
            String query = String.Format(helper.SqlObtenerEstructura, idEmpresa, idGrupo, idTipoCombustible);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion1DTO entity = new MeMedicion1DTO();

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iTipoinfodesc = dr.GetOrdinal(helper.Tipoinfodesc);
                    if (!dr.IsDBNull(iTipoinfodesc)) entity.Tipoinfodesc = dr.GetString(iTipoinfodesc);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iTipoinfocodi = dr.GetOrdinal(helper.Tipoinfocodi);
                    if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void DeleteEnvioArchivo(int idLectura, DateTime fechaInicio, DateTime fechaFin, int idFormato, int idEmpresa)
        {
            string sqlDelete = string.Format(helper.SqlDeleteEnvioArchivo, idLectura, fechaInicio.ToString(ConstantesBase.FormatoFecha),
              fechaFin.ToString(ConstantesBase.FormatoFecha), idFormato, idEmpresa);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlDelete);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<MeMedicion1DTO> GetEnvioArchivo(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin, int lectocodi = -1)
        {
            List<MeMedicion1DTO> entitys = new List<MeMedicion1DTO>();
            string queryString = string.Format(helper.SqlGetEnvioArchivo, idFormato, idEmpresa, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), lectocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MeMedicion1DTO> GetDataFormatoSecundario(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion1DTO> entitys = new List<MeMedicion1DTO>();
            string queryString = string.Format(helper.SqlGetDataFormatoSec, idFormato, idEmpresa, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MeMedicion1DTO> GetHidrologia(int idLectura, int idOrigenLectura, string idsEmpresa, string idsCuenca,
            string idsFamilia, DateTime fechaInicio, DateTime fechaFin, string idsPtoMedicion)
        {
            List<MeMedicion1DTO> entitys = new List<MeMedicion1DTO>();
            string sqlQuery = string.Format(helper.SqlGetHidrologia, idLectura, idOrigenLectura, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), idsEmpresa, idsCuenca, idsPtoMedicion, idsFamilia);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion1DTO entity = helper.Create(dr);
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iCuenca = dr.GetOrdinal(helper.Cuenca);
                    if (!dr.IsDBNull(iCuenca)) entity.Cuenca = dr.GetString(iCuenca);
                    int iTipoptomedinomb = dr.GetOrdinal(helper.Tipoptomedinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);
                    int iTipoinfoabrev = dr.GetOrdinal(helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);
                    int iPtomedinomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedinomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedinomb);
                    int iTipoptomedicodi = dr.GetOrdinal(helper.Tipoptomedicodi);
                    if (!dr.IsDBNull(iTipoptomedicodi)) entity.Tipoptomedicodi = Convert.ToInt32(dr.GetValue(iTipoptomedicodi));
                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iFamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion1DTO> GetListaMedicion1(int lectCodiRecepcion, DateTime fechaInicial, DateTime fechaFinal)
        {
            List<MeMedicion1DTO> entitys = new List<MeMedicion1DTO>();
            string sqlQuery = string.Format(helper.SqlGetListaMedicion1, lectCodiRecepcion, fechaInicial.ToString(ConstantesBase.FormatoFecha),
                fechaFinal.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion1DTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeMedicion1DTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin, int lectcodi,
            int tipoinfocodi, string ptomedicodi)
        {
            List<MeMedicion1DTO> entitys = new List<MeMedicion1DTO>();

            string sql = String.Format(helper.SqlObtenerMedicion1, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), lectcodi, tipoinfocodi, ptomedicodi);
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

        //inicio agregado
        public List<MeMedicion1DTO> GetDataPronosticoHidrologia(int reportecodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion1DTO> entitys = new List<MeMedicion1DTO>();

            string sql = String.Format(helper.SqlGetMedicionPronosticoHidrologia, reportecodi, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion1DTO entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal("EMPRCODI");
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);
                    int iEmprnomb = dr.GetOrdinal("EMPRNOMB");
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iUbicacioncodi = dr.GetOrdinal("CODI_UBICACION");
                    if (!dr.IsDBNull(iUbicacioncodi)) entity.Ubicacioncodi = dr.GetInt32(iUbicacioncodi);
                    int iUbicaciondesc = dr.GetOrdinal("DESC_UBICACION");
                    if (!dr.IsDBNull(iUbicaciondesc)) entity.Ubicaciondesc = dr.GetString(iUbicaciondesc);

                    int iOrigenPtomedicodi = dr.GetOrdinal("PTOMEDICODI_ORIGEN");
                    if (!dr.IsDBNull(iOrigenPtomedicodi)) entity.OrigenPtomedicodi = dr.GetInt32(iOrigenPtomedicodi);
                    int iOrigendesc = dr.GetOrdinal("DESC_ORIGEN");
                    if (!dr.IsDBNull(iOrigendesc)) entity.OrigenPtomedidesc = dr.GetString(iOrigendesc);

                    int iCalculadoPtomedicodi = dr.GetOrdinal("PTOMEDICODI_CALCULADO");
                    if (!dr.IsDBNull(iCalculadoPtomedicodi)) entity.CalculadoPtomedicodi = dr.GetInt32(iCalculadoPtomedicodi);
                    int iCalculadodesc = dr.GetOrdinal("DESC_CALCULADO");
                    if (!dr.IsDBNull(iCalculadodesc)) entity.CalculadoPtomedidesc = dr.GetString(iCalculadodesc);
                    int iCalculadoorden = dr.GetOrdinal("ORDEN_CALCULADO");
                    if (!dr.IsDBNull(iCalculadoorden)) entity.CalculadoOrden = dr.GetInt32(iCalculadoorden);

                    int iTipoRelacioncodi = dr.GetOrdinal("TRPTOCODI");
                    if (!dr.IsDBNull(iTipoRelacioncodi)) entity.TipoRelacioncodi = dr.GetInt32(iTipoRelacioncodi);

                    int iCalculadoFactor = dr.GetOrdinal("FACTOR_CALCULADO");
                    if (!dr.IsDBNull(iCalculadoFactor)) entity.CalculadoFactor = dr.GetDecimal(iCalculadoFactor);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion1DTO> GetDataPronosticoHidrologiaByPtoCalculadoAndFecha(int reportecodi, int ptocalculadocodi, DateTime fecha)
        {
            List<MeMedicion1DTO> entitys = new List<MeMedicion1DTO>();

            string sql = String.Format(helper.SqlGetMedicionPronosticoHidrologiaByPtoCalculadoAndFecha, reportecodi, ptocalculadocodi, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion1DTO entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal("EMPRCODI");
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);
                    int iEmprnomb = dr.GetOrdinal("EMPRNOMB");
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iUbicacioncodi = dr.GetOrdinal("CODI_UBICACION");
                    if (!dr.IsDBNull(iUbicacioncodi)) entity.Ubicacioncodi = dr.GetInt32(iUbicacioncodi);
                    int iUbicaciondesc = dr.GetOrdinal("DESC_UBICACION");
                    if (!dr.IsDBNull(iUbicaciondesc)) entity.Ubicaciondesc = dr.GetString(iUbicaciondesc);

                    int iOrigenPtomedicodi = dr.GetOrdinal("PTOMEDICODI_ORIGEN");
                    if (!dr.IsDBNull(iOrigenPtomedicodi)) entity.OrigenPtomedicodi = dr.GetInt32(iOrigenPtomedicodi);
                    int iOrigendesc = dr.GetOrdinal("DESC_ORIGEN");
                    if (!dr.IsDBNull(iOrigendesc)) entity.OrigenPtomedidesc = dr.GetString(iOrigendesc);

                    int iCalculadoPtomedicodi = dr.GetOrdinal("PTOMEDICODI_CALCULADO");
                    if (!dr.IsDBNull(iCalculadoPtomedicodi)) entity.CalculadoPtomedicodi = dr.GetInt32(iCalculadoPtomedicodi);
                    int iCalculadodesc = dr.GetOrdinal("DESC_CALCULADO");
                    if (!dr.IsDBNull(iCalculadodesc)) entity.CalculadoPtomedidesc = dr.GetString(iCalculadodesc);
                    int iCalculadoorden = dr.GetOrdinal("ORDEN_CALCULADO");
                    if (!dr.IsDBNull(iCalculadoorden)) entity.CalculadoOrden = dr.GetInt32(iCalculadoorden);

                    int iTipoRelacioncodi = dr.GetOrdinal("TRPTOCODI");
                    if (!dr.IsDBNull(iTipoRelacioncodi)) entity.TipoRelacioncodi = dr.GetInt32(iTipoRelacioncodi);

                    int iCalculadoFactor = dr.GetOrdinal("FACTOR_CALCULADO");
                    if (!dr.IsDBNull(iCalculadoFactor)) entity.CalculadoFactor = dr.GetDecimal(iCalculadoFactor);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void DeleteEnvioArchivo2(int idTptomedi, int idLectura, DateTime fechaInicio, DateTime fechaFin, int idFormato, int idEmpresa)
        {
            string sqlDelete = string.Format(helper.SqlDeleteEnvioArchivo2, idLectura, fechaInicio.ToString(ConstantesBase.FormatoFecha),
              fechaFin.ToString(ConstantesBase.FormatoFecha), idFormato, idEmpresa, idTptomedi);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlDelete);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<MeMedicion1DTO> GetDataSemanalPowel(DateTime fechaInicio, DateTime fechaFin, int reportecodi)
        {
            List<MeMedicion1DTO> entitys = new List<MeMedicion1DTO>();

            string sql = String.Format(helper.SqlListadoInformacionSemanalPowel, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), reportecodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion1DTO entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal("EMPRCODI");
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);
                    int iEmprnomb = dr.GetOrdinal("EMPRNOMB");
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquicodi = dr.GetOrdinal("equicodi");
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEquinomb = dr.GetOrdinal("EQUINOMB");
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iTipoptomedinomb = dr.GetOrdinal("TPTOMEDINOMB");
                    if (!dr.IsDBNull(iTipoptomedinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);

                    int iTipoinfoabrev = dr.GetOrdinal("TIPOINFOABREV");
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region INDISPONIBILIDADES

        public List<MeMedicion1DTO> GetListaMedicion1ContratoCombustible(int lectcodi, DateTime fechaInicial, DateTime fechaFinal)
        {
            List<MeMedicion1DTO> entitys = new List<MeMedicion1DTO>();
            string sqlQuery = string.Format(helper.SqlGetListaMedicion1ContratoCombustible, lectcodi, fechaInicial.ToString(ConstantesBase.FormatoFecha),
                fechaFinal.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion1DTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region SIOSEIN2

        public List<MeMedicion1DTO> GetDataEjecCaudales(DateTime fechaInicio, DateTime fechaFin, string ptomedicodi, int lectcodi, int tipoinfocodi)
        {
            List<MeMedicion1DTO> entitys = new List<MeMedicion1DTO>();
            string sqlQuery = string.Format(helper.SqlGetDataEjecCaudales, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), ptomedicodi, lectcodi, tipoinfocodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion1DTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal("EMPRNOMB");
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region Numerales Datos Base
        public List<MeMedicion1DTO> ListaNumerales_DatosBase_5_7_1(DateTime fecha, string fechaIni, string fechaFin)
        {
            string sqlQuery = "";
            if (fecha >= new DateTime(2009, 10, 01))
            {
                sqlQuery = string.Format(this.helper.SqlDatosBase_5_7_1_1, fechaIni, fechaFin);
            }
            else
            {
                sqlQuery = string.Format(this.helper.SqlDatosBase_5_7_1_2, fechaIni, fechaFin);
            }



            List<MeMedicion1DTO> entitys = new List<MeMedicion1DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion1DTO entity = new MeMedicion1DTO();

                    int iH1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iDia = dr.GetOrdinal(helper.Dia);
                    if (!dr.IsDBNull(iDia)) entity.Dia = dr.GetString(iDia);

                    int iFecha = dr.GetOrdinal(helper.Fecha);
                    if (!dr.IsDBNull(iFecha)) entity.Fecha = dr.GetDateTime(iFecha);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeMedicion1DTO> ListaNumerales_DatosBase_5_7_2(string fechaIni, string fechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBase_5_7_2, fechaIni, fechaFin);

            List<MeMedicion1DTO> entitys = new List<MeMedicion1DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion1DTO entity = new MeMedicion1DTO();

                    int iH1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iDia = dr.GetOrdinal(helper.Dia);
                    if (!dr.IsDBNull(iDia)) entity.Dia = dr.GetString(iDia);


                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        #endregion

    }
}
