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
    /// Clase de acceso a datos de la tabla ME_MEDICION24
    /// </summary>
    public class MeMedicion24Repository : RepositoryBase, IMeMedicion24Repository
    {
        public MeMedicion24Repository(string strConn)
            : base(strConn)
        {
        }

        MeMedicion24Helper helper = new MeMedicion24Helper();

        public void Save(MeMedicion24DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            entity.Tipoptomedicodi = entity.Tipoptomedicodi > 0 ? entity.Tipoptomedicodi : -1;
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, entity.Medifecha);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Meditotal, DbType.Decimal, entity.Meditotal);
            dbProvider.AddInParameter(command, helper.Mediestado, DbType.String, entity.Mediestado);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.H2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.H3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.H4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.H5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.H6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.H7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.H8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.H9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.H10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.H11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.H12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.H13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.H14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.H15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.H16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.H17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.H18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.H19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.H20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.H21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.H22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.H23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.H24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.AddInParameter(command, helper.T1, DbType.Int32, entity.T1);
            dbProvider.AddInParameter(command, helper.T2, DbType.Int32, entity.T2);
            dbProvider.AddInParameter(command, helper.T3, DbType.Int32, entity.T3);
            dbProvider.AddInParameter(command, helper.T4, DbType.Int32, entity.T4);
            dbProvider.AddInParameter(command, helper.T5, DbType.Int32, entity.T5);
            dbProvider.AddInParameter(command, helper.T6, DbType.Int32, entity.T6);
            dbProvider.AddInParameter(command, helper.T7, DbType.Int32, entity.T7);
            dbProvider.AddInParameter(command, helper.T8, DbType.Int32, entity.T8);
            dbProvider.AddInParameter(command, helper.T9, DbType.Int32, entity.T9);
            dbProvider.AddInParameter(command, helper.T10, DbType.Int32, entity.T10);
            dbProvider.AddInParameter(command, helper.T11, DbType.Int32, entity.T11);
            dbProvider.AddInParameter(command, helper.T12, DbType.Int32, entity.T12);
            dbProvider.AddInParameter(command, helper.T13, DbType.Int32, entity.T13);
            dbProvider.AddInParameter(command, helper.T14, DbType.Int32, entity.T14);
            dbProvider.AddInParameter(command, helper.T15, DbType.Int32, entity.T15);
            dbProvider.AddInParameter(command, helper.T16, DbType.Int32, entity.T16);
            dbProvider.AddInParameter(command, helper.T17, DbType.Int32, entity.T17);
            dbProvider.AddInParameter(command, helper.T18, DbType.Int32, entity.T18);
            dbProvider.AddInParameter(command, helper.T19, DbType.Int32, entity.T19);
            dbProvider.AddInParameter(command, helper.T20, DbType.Int32, entity.T20);
            dbProvider.AddInParameter(command, helper.T21, DbType.Int32, entity.T21);
            dbProvider.AddInParameter(command, helper.T22, DbType.Int32, entity.T22);
            dbProvider.AddInParameter(command, helper.T23, DbType.Int32, entity.T23);
            dbProvider.AddInParameter(command, helper.T24, DbType.Int32, entity.T24);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(MeMedicion24DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, entity.Medifecha);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Meditotal, DbType.Decimal, entity.Meditotal);
            dbProvider.AddInParameter(command, helper.Mediestado, DbType.String, entity.Mediestado);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.H2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.H3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.H4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.H5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.H6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.H7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.H8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.H9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.H10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.H11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.H12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.H13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.H14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.H15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.H16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.H17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.H18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.H19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.H20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.H21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.H22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.H23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.H24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);


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

        public MeMedicion24DTO GetById()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            MeMedicion24DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeMedicion24DTO> List(DateTime fechaInicio, DateTime fechaFin)
        {
            string queryString = string.Format(helper.SqlList, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            List<MeMedicion24DTO> entitys = new List<MeMedicion24DTO>();
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

        public List<MeMedicion24DTO> GetByCriteria()
        {
            List<MeMedicion24DTO> entitys = new List<MeMedicion24DTO>();
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
        /// Borra las mediciones enviadas en un archivo
        /// </summary>
        /// <param name="medifecha"></param>
        public void DeleteEnvioArchivo(int idLectura, DateTime fechaInicio, DateTime fechaFin, int idFormato, int idEmpresa)
        {
            string sqlDelete = string.Format(helper.SqlDeleteEnvioArchivo, idLectura, fechaInicio.ToString(ConstantesBase.FormatoFecha),
              fechaFin.ToString(ConstantesBase.FormatoFecha), idFormato, idEmpresa);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlDelete);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<MeMedicion24DTO> GetEnvioArchivo(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion24DTO> entitys = new List<MeMedicion24DTO>();
            string queryString = string.Format(helper.SqlGetEnvioArchivo, idFormato, idEmpresa, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);
                    helper.CreateTipo(dr, entity);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion24DTO> GetHidrologia(int idLectura, int idOrigenLectura, string idsEmpresa, string idsCuenca, string idsFamilia, DateTime fechaInicio, DateTime fechaFin, string idsPtoMedicion)
        {
            List<MeMedicion24DTO> entitys = new List<MeMedicion24DTO>();
            string sqlQuery = string.Format(helper.SqlGetHidrologia, idLectura, idOrigenLectura, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), idsEmpresa, idsCuenca, idsPtoMedicion, idsFamilia);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion24DTO entity = helper.Create(dr);
                    helper.CreateTipo(dr, entity);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iCuenca = dr.GetOrdinal(helper.Cuenca);
                    if (!dr.IsDBNull(iCuenca)) entity.Cuenca = dr.GetString(iCuenca);
                    int iTipoptomedinomb = dr.GetOrdinal(helper.Tipoptomedinomb);
                    if (!dr.IsDBNull(iTipoptomedinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);
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
                    decimal? total = null;
                    decimal? valor = null;
                    for (var i = 1; i <= 24; i++)
                    {
                        valor = (decimal?)entity.GetType().GetProperty("H" + i.ToString()).GetValue(entity, null);
                        if (valor != null)
                        {
                            if (total != null)
                                total = total + valor;
                            else
                                total = valor;
                        }
                    }
                    entity.Meditotal = total;
                }
            }

            return entitys;
        }

        public List<MeMedicion24DTO> GetHidrologiaTiempoReal(int reporcodi, int idOrigenLectura, string idsEmpresa, DateTime fechaInicio,
            DateTime fechaFin, string idsTipoPtoMedicion, int lectCodi)
        {
            List<MeMedicion24DTO> entitys = new List<MeMedicion24DTO>();
            string sqlQuery = string.Format(helper.SqlGetHidrologiaTiempoReal, reporcodi, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), idsEmpresa, idsTipoPtoMedicion, lectCodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion24DTO entity = helper.Create(dr);
                    helper.CreateTipo(dr, entity);

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
                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion24DTO> GetInterconexiones(int idLectura, int idOrigenLectura, string ptomedicodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion24DTO> entitys = new List<MeMedicion24DTO>();
            string sqlQuery = string.Format(helper.SqlGetInterconexiones, idLectura, idOrigenLectura, ptomedicodi, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion24DTO entity = helper.Create(dr);
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iTipoptomedinomb = dr.GetOrdinal(helper.Tipoptomedinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);
                    int iTipoinfoabrev = dr.GetOrdinal(helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion24DTO> GetDataFormatoSecundario(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion24DTO> entitys = new List<MeMedicion24DTO>();
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

        public List<MeMedicion24DTO> GetLista24PresionGas(int lectCodi, int origlectcodi, string sEmprCodi, DateTime fechaInicio, DateTime fechaFin, int TipotomedicodiPresionGas, int tipoinfocodiPresion, string strCentralInt)
        {
            List<MeMedicion24DTO> entitys = new List<MeMedicion24DTO>();
            string sqlQuery = string.Format(helper.SqlGetLista24PresionGas, lectCodi, origlectcodi, sEmprCodi, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), TipotomedicodiPresionGas, tipoinfocodiPresion, strCentralInt);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion24DTO entity = helper.Create(dr);
                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iTipoptomedinomb = dr.GetOrdinal(this.helper.Tipoptomedinomb);
                    if (!dr.IsDBNull(iTipoptomedinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);

                    int iTipoinfoabrev = dr.GetOrdinal(this.helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);

                    int iPtomedinomb = dr.GetOrdinal(this.helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedinomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedinomb);

                    int iTipoptomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iTipoptomedicodi)) entity.Tipoptomedicodi = Convert.ToInt32(dr.GetValue(iTipoptomedicodi));

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iPtomedielenomb = dr.GetOrdinal(this.helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iEquipopadre = dr.GetOrdinal(this.helper.Equipopadre);
                    if (!dr.IsDBNull(iEquipopadre)) entity.Equipopadre = dr.GetString(iEquipopadre);

                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion24DTO> GetLista24TemperaturaAmbiente(int lectCodi, int origlectcodi, string sEmprCodi, DateTime fechaInicio, DateTime fechaFin, string strCentralInt)
        {
            List<MeMedicion24DTO> entitys = new List<MeMedicion24DTO>();
            string sqlQuery = string.Format(helper.SqlGetLista24TemperaturaAmbiente, lectCodi, origlectcodi, sEmprCodi, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), strCentralInt);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion24DTO entity = helper.Create(dr);
                    int iEquinomb = dr.GetOrdinal("Equinomb");
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iTipoptomedinomb = dr.GetOrdinal("Tptomedinomb");
                    if (!dr.IsDBNull(iTipoptomedinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);

                    int iTipoinfoabrev = dr.GetOrdinal("Tipoinfoabrev");
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);

                    int iPtomedinomb = dr.GetOrdinal("Ptomedibarranomb");
                    if (!dr.IsDBNull(iPtomedinomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedinomb);

                    int iTipoptomedicodi = dr.GetOrdinal("Tptomedicodi");
                    if (!dr.IsDBNull(iTipoptomedicodi)) entity.Tipoptomedicodi = Convert.ToInt32(dr.GetValue(iTipoptomedicodi));

                    int iEmprnomb = dr.GetOrdinal("Emprnomb");
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iEquipopadre = dr.GetOrdinal(this.helper.Equipopadre);
                    if (!dr.IsDBNull(iEquipopadre)) entity.Equipopadre = dr.GetString(iEquipopadre);

                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion24DTO> GetDataHistoricoHidrologia(int reportecodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion24DTO> entitys = new List<MeMedicion24DTO>();

            string sql = String.Format(helper.SqlGetMedicionHistoricoHidrologia, reportecodi, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion24DTO entity = helper.Create(dr);

                    int iReporteOrden = dr.GetOrdinal("REPPTOORDEN");
                    if (!dr.IsDBNull(iReporteOrden)) entity.ReporteOrden = dr.GetInt32(iReporteOrden);

                    int iEmprcodi = dr.GetOrdinal("EMPRCODI");
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal("EMPRNOMB");
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGrupocodi = dr.GetOrdinal("GRUPOCODI");
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    int iGruponomb = dr.GetOrdinal("GRUPONOMB");
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

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

        //inicio modificado
        public List<MeMedicion24DTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin, int lectcodi, int tipoinfocodi, string ptomedicodi)
        {
            List<MeMedicion24DTO> entitys = new List<MeMedicion24DTO>();

            string sql = String.Format(helper.SqlObtenerMedicion24, fechaInicio.ToString(ConstantesBase.FormatoFecha),
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
        //fin modificado

        #region SIOSEIN

        public List<MeMedicion24DTO> GetHidrologiaSioSein(int reporcodi, DateTime dfechaIni, DateTime dfechaFin)
        {
            List<MeMedicion24DTO> entitys = new List<MeMedicion24DTO>();
            string query = string.Format(helper.SqlGetHidrologiaSioSein, reporcodi, dfechaIni.ToString(ConstantesBase.FormatoFecha), dfechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            MeMedicion24DTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iReporteOrden = dr.GetOrdinal("REPPTOORDEN");
                    if (!dr.IsDBNull(iReporteOrden)) entity.ReporteOrden = dr.GetInt32(iReporteOrden);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iTipoptomedicodi = dr.GetOrdinal(helper.Tipoptomedicodi);
                    if (!dr.IsDBNull(iTipoptomedicodi)) entity.Tipoptomedicodi = dr.GetInt32(iTipoptomedicodi);

                    int iTipoptomedinomb = dr.GetOrdinal(helper.Tipoptomedinomb);
                    if (!dr.IsDBNull(iTipoptomedinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iOsicodi = dr.GetOrdinal(helper.Osicodi);
                    if (!dr.IsDBNull(iOsicodi)) entity.Osicodi = dr.GetString(iOsicodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region MigracionSGOCOES-GrupoB
        public List<MeMedicion24DTO> ListaGeneracionDIgSILENT(DateTime fecha)
        {
            List<MeMedicion24DTO> entitys = new List<MeMedicion24DTO>();
            string query = string.Format(helper.SqlListaGeneracionOpera, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            MeMedicion24DTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MeMedicion24DTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    int iGrupoabrev = dr.GetOrdinal(helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iGrupotipo = dr.GetOrdinal(helper.Grupotipo);
                    if (!dr.IsDBNull(iGrupotipo)) entity.Grupotipo = dr.GetString(iGrupotipo);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);

                    int iCentral = dr.GetOrdinal(helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = dr.GetInt32(iFamcodi);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iMinimo = dr.GetOrdinal(helper.Minimo);
                    if (!dr.IsDBNull(iMinimo)) entity.Minimo = dr.GetDecimal(iMinimo);

                    int iPotenciaEfectiva = dr.GetOrdinal(helper.PotenciaEfectiva);
                    if (!dr.IsDBNull(iPotenciaEfectiva)) entity.PotenciaEfectiva = dr.GetDecimal(iPotenciaEfectiva);

                    int iFechapropequiMin = dr.GetOrdinal(helper.FechapropequiMin);
                    if (!dr.IsDBNull(iFechapropequiMin)) entity.FechapropequiMin = Convert.ToDateTime(dr.GetValue(iFechapropequiMin));

                    int iFechapropequiPotefec = dr.GetOrdinal(helper.FechapropequiPotefec);
                    if (!dr.IsDBNull(iFechapropequiPotefec)) entity.FechapropequiPotefec = Convert.ToDateTime(dr.GetValue(iFechapropequiPotefec));

                    int iDigsilent = dr.GetOrdinal(helper.Digsilent);
                    if (!dr.IsDBNull(iDigsilent)) entity.Digsilent = dr.GetString(iDigsilent);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion24DTO> ListaDemandaDigsilent(string propcodi, string famcodi, DateTime fecha)
        {
            List<MeMedicion24DTO> entitys = new List<MeMedicion24DTO>();
            string query = string.Format(helper.SqlListaDemandaDigsilent, propcodi, famcodi, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            MeMedicion24DTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    //int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    //if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = dr.GetInt32(iPtomedicodi);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = dr.GetInt32(iFamcodi);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iDigsilent = dr.GetOrdinal(helper.Digsilent);
                    if (!dr.IsDBNull(iDigsilent)) entity.Digsilent = dr.GetString(iDigsilent);

                    int iTipoinfocodi = dr.GetOrdinal(helper.Tipoinfocodi);
                    if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = dr.GetInt32(iTipoinfocodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void SaveMemedicion24masivo(List<MeMedicion24DTO> entitys)
        {
            dbProvider.AddColumnMapping(helper.Lectcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Medifecha, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Tipoinfocodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Ptomedicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Meditotal, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Mediestado, DbType.String);
            dbProvider.AddColumnMapping(helper.H1, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H2, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H3, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H4, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H5, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H6, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H7, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H8, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H9, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H10, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H11, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H12, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H13, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H14, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H15, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H16, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H17, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H18, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H19, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H20, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H21, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H22, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H23, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H24, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Lastuser, DbType.String);
            dbProvider.AddColumnMapping(helper.Lastdate, DbType.DateTime);

            dbProvider.BulkInsert<MeMedicion24DTO>(entitys, helper.TableName);
        }

        public void DeleteMasivo(int lectcodi, DateTime medifecha, string tipoinfocodi, string ptomedicodi)
        {
            string query = string.Format(helper.SqlDeleteMasivo, lectcodi, medifecha.ToString(ConstantesBase.FormatoFecha), ptomedicodi, tipoinfocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }
        #endregion

        public List<MeMedicion24DTO> ObtenerVolumenUtil(DateTime fechaInicio, DateTime fechaFin, int lectcodi, int tipoinfocodi, string ptomedicodi)
        {
            string query = string.Format(helper.SqlObtenerVolumenUtil, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), lectcodi, tipoinfocodi, ptomedicodi);
            List<MeMedicion24DTO> entitys = new List<MeMedicion24DTO>();
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

        #region Mejoras RDO
        public void SaveEjecutados(MeMedicion24DTO entity, int idEnvio, string usuario, int idEmpresa)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveEjecutados);
            entity.Tipoptomedicodi = entity.Tipoptomedicodi > 0 ? entity.Tipoptomedicodi : -1;
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, idEnvio);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, entity.Medifecha);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Meditotal, DbType.Decimal, entity.Meditotal);
            dbProvider.AddInParameter(command, helper.Mediestado, DbType.String, entity.Mediestado);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.H2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.H3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.H4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.H5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.H6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.H7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.H8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.H9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.H10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.H11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.H12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.H13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.H14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.H15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.H16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.H17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.H18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.H19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.H20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.H21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.H22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.H23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.H24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.E1, DbType.String, entity.E1);
            dbProvider.AddInParameter(command, helper.E2, DbType.String, entity.E2);
            dbProvider.AddInParameter(command, helper.E3, DbType.String, entity.E3);
            dbProvider.AddInParameter(command, helper.E4, DbType.String, entity.E4);
            dbProvider.AddInParameter(command, helper.E5, DbType.String, entity.E5);
            dbProvider.AddInParameter(command, helper.E6, DbType.String, entity.E6);
            dbProvider.AddInParameter(command, helper.E7, DbType.String, entity.E7);
            dbProvider.AddInParameter(command, helper.E8, DbType.String, entity.E8);
            dbProvider.AddInParameter(command, helper.E9, DbType.String, entity.E9);
            dbProvider.AddInParameter(command, helper.E10, DbType.String, entity.E10);
            dbProvider.AddInParameter(command, helper.E11, DbType.String, entity.E11);
            dbProvider.AddInParameter(command, helper.E12, DbType.String, entity.E12);
            dbProvider.AddInParameter(command, helper.E13, DbType.String, entity.E13);
            dbProvider.AddInParameter(command, helper.E14, DbType.String, entity.E14);
            dbProvider.AddInParameter(command, helper.E15, DbType.String, entity.E15);
            dbProvider.AddInParameter(command, helper.E16, DbType.String, entity.E16);
            dbProvider.AddInParameter(command, helper.E17, DbType.String, entity.E17);
            dbProvider.AddInParameter(command, helper.E18, DbType.String, entity.E18);
            dbProvider.AddInParameter(command, helper.E19, DbType.String, entity.E19);
            dbProvider.AddInParameter(command, helper.E20, DbType.String, entity.E20);
            dbProvider.AddInParameter(command, helper.E21, DbType.String, entity.E21);
            dbProvider.AddInParameter(command, helper.E22, DbType.String, entity.E22);
            dbProvider.AddInParameter(command, helper.E23, DbType.String, entity.E23);
            dbProvider.AddInParameter(command, helper.E24, DbType.String, entity.E24);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, usuario);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, idEmpresa);

            dbProvider.ExecuteNonQuery(command);
        }
        public List<MeMedicion24DTO> GetEnvioArchivoEjecutados(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin, string horario)
        {
            List<MeMedicion24DTO> entitys = new List<MeMedicion24DTO>();
            string queryString = string.Format(helper.SqlGetEnvioArchivoEjecutados, idFormato, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), horario);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateEjecutados(dr));
                }
            }

            return entitys;
        }
        public List<MeMedicion24DTO> GetEnvioArchivoIntranet(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin, string horario)
        {
            List<MeMedicion24DTO> entitys = new List<MeMedicion24DTO>();
            string queryString = string.Format(helper.SqlGetEnvioArchivoIntranet, idFormato, idEmpresa, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
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
        public void SaveIntranet(MeMedicion24DTO entity, int idEnvio)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveIntranet);
            entity.Tipoptomedicodi = entity.Tipoptomedicodi > 0 ? entity.Tipoptomedicodi : -1;
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, idEnvio);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, entity.Medifecha);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Meditotal, DbType.Decimal, entity.Meditotal);
            dbProvider.AddInParameter(command, helper.Mediestado, DbType.String, entity.Mediestado);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.H2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.H3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.H4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.H5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.H6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.H7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.H8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.H9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.H10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.H11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.H12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.H13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.H14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.H15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.H16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.H17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.H18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.H19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.H20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.H21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.H22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.H23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.H24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }
        public List<MeMedicion24DTO> GetEnvioMeMedicion24Intranet(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion24DTO> entitys = new List<MeMedicion24DTO>();
            string queryString = string.Format(helper.SqlGetEnvioMeMedicion24Intranet, idFormato, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
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
        #endregion
    }
}
