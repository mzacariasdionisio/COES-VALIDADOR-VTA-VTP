using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Linq;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla ME_MEDICIONXINTERVALO
    /// </summary>
    public class MeMedicionxintervaloRepository : RepositoryBase, IMeMedicionxintervaloRepository
    {
        public int PtoMedUti5 = 42435;

        public MeMedicionxintervaloRepository(string strConn)
            : base(strConn)
        {
        }

        MeMedicionxintervaloHelper helper = new MeMedicionxintervaloHelper();

        public void Save(MeMedicionxintervaloDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);
            entity.Tptomedicodi = entity.Tptomedicodi > 0 ? entity.Tptomedicodi : -1;
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Medintcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Medintfechaini, DbType.DateTime, entity.Medintfechaini);
            dbProvider.AddInParameter(command, helper.Medintfechafin, DbType.DateTime, entity.Medintfechafin);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Medinth1, DbType.Decimal, entity.Medinth1);
            dbProvider.AddInParameter(command, helper.Medintusumodificacion, DbType.String, entity.Medintusumodificacion);
            dbProvider.AddInParameter(command, helper.Medintfecmodificacion, DbType.DateTime, entity.Medintfecmodificacion);
            dbProvider.AddInParameter(command, helper.Medintdescrip, DbType.String, entity.Medintdescrip);
            dbProvider.AddInParameter(command, helper.Medestcodi, DbType.Int32, entity.Medestcodi);
            dbProvider.AddInParameter(command, helper.Medintsemana, DbType.Int32, entity.Medintsemana);
            dbProvider.AddInParameter(command, helper.Medintanio, DbType.DateTime, entity.Medintanio);
            dbProvider.AddInParameter(command, helper.Medintblqhoras, DbType.Decimal, entity.Medintblqhoras);
            dbProvider.AddInParameter(command, helper.Medintblqnumero, DbType.Int32, entity.Medintblqnumero);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, entity.Enviocodi);
            dbProvider.AddInParameter(command, helper.Tptomedicodi, DbType.Int32, entity.Tptomedicodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(MeMedicionxintervaloDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            entity.Tptomedicodi = entity.Tptomedicodi > 0 ? entity.Tptomedicodi : -1;
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            dbProvider.AddInParameter(command, helper.Medintfechaini, DbType.DateTime, entity.Medintfechaini);
            dbProvider.AddInParameter(command, helper.Medintfechafin, DbType.DateTime, entity.Medintfechafin);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Medinth1, DbType.Decimal, entity.Medinth1);
            dbProvider.AddInParameter(command, helper.Medintusumodificacion, DbType.String, entity.Medintusumodificacion);
            dbProvider.AddInParameter(command, helper.Medintfecmodificacion, DbType.DateTime, entity.Medintfecmodificacion);
            dbProvider.AddInParameter(command, helper.Medintdescrip, DbType.String, entity.Medintdescrip);
            dbProvider.AddInParameter(command, helper.Medestcodi, DbType.Int32, entity.Medestcodi);
            dbProvider.AddInParameter(command, helper.Medintsemana, DbType.Int32, entity.Medintsemana);
            dbProvider.AddInParameter(command, helper.Medintanio, DbType.DateTime, entity.Medintanio);
            dbProvider.AddInParameter(command, helper.Medintblqhoras, DbType.Int32, entity.Medintblqhoras);
            dbProvider.AddInParameter(command, helper.Medintblqnumero, DbType.Int32, entity.Medintblqnumero);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, entity.Enviocodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.AddInParameter(command, helper.Medintcodi, DbType.String, entity.Medintcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeMedicionxintervaloDTO GetById(int ptoMedicodi, DateTime fechaIni)
        {
            string queryString = string.Format(helper.SqlGetById, ptoMedicodi, fechaIni.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            MeMedicionxintervaloDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeMedicionxintervaloDTO> List()
        {
            List<MeMedicionxintervaloDTO> entitys = new List<MeMedicionxintervaloDTO>();
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

        public List<MeMedicionxintervaloDTO> GetByCriteria(int enviocodi)
        {
            List<MeMedicionxintervaloDTO> entitys = new List<MeMedicionxintervaloDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, enviocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity  = helper.Create(dr);

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iTipoptomedinomb = dr.GetOrdinal(helper.Tipoptomedinomb);
                    if (!dr.IsDBNull(iTipoptomedinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);

                    int iTipoinfoabrev = dr.GetOrdinal(helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void DeleteEnvioArchivo(int idLectura, DateTime fechaInicio, DateTime fechaFin, int idFormato, int idEmpresa)
        {
            string sqlDelete = string.Format(helper.SqlDeleteEnvioArchivo, idLectura, fechaInicio.ToString(ConstantesBase.FormatoFechaExtendido),
              fechaFin.ToString(ConstantesBase.FormatoFechaExtendido), idFormato, idEmpresa);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlDelete);
            dbProvider.ExecuteNonQuery(command);
        }


        public List<MeMedicionxintervaloDTO> GetHidrologiaDescargaVert(int formatCodi, string idsEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicionxintervaloDTO> entitys = new List<MeMedicionxintervaloDTO>();
            string sqlQuery = string.Format(helper.SqlGetHidrologiaDescargaVert, formatCodi, idsEmpresa, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicionxintervaloDTO entity = helper.Create(dr);
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iTipoptomedinomb = dr.GetOrdinal(helper.Tipoptomedinomb);
                    if (!dr.IsDBNull(iTipoptomedinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);
                    int iTipoinfoabrev = dr.GetOrdinal(helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);
                    int iPtomedinomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedinomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedinomb);
                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iFamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);
                    int iEquipade = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipade)) entity.Equipadre = dr.GetInt32(iEquipade);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicionxintervaloDTO> GetHidrologiaDescargaVertPag(int formatCodi, string idsEmpresa, DateTime fechaInicio, DateTime fechaFin, int nroPaginas, int pageSize)
        {
            List<MeMedicionxintervaloDTO> entitys = new List<MeMedicionxintervaloDTO>();
            string sqlQuery = string.Format(helper.SqlGetHidrologiaDescargaVertPag, formatCodi, idsEmpresa, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), nroPaginas, pageSize);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicionxintervaloDTO entity = helper.Create(dr);
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iTipoptomedinomb = dr.GetOrdinal(helper.Tipoptomedinomb);
                    if (!dr.IsDBNull(iTipoptomedinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);
                    int iTipoinfoabrev = dr.GetOrdinal(helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);
                    int iPtomedinomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedinomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedinomb);
                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iFamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicionxintervaloDTO> GetListaMedxintervStock(int lectCodi, int origlectcodi, string sEmprCodi, int famCodi, DateTime fechaInicial,
            DateTime fechaFinal, string idsEstado, string idsRecurso, string strCentralInt, string idsEquipo, string tptomedicodi)
        {
            List<MeMedicionxintervaloDTO> entitys = new List<MeMedicionxintervaloDTO>();
            string sqlQuery = string.Format(helper.SqlGetListaMedxintervStock, lectCodi, origlectcodi, sEmprCodi, famCodi, fechaInicial.ToString(ConstantesBase.FormatoFecha),
                fechaFinal.ToString(ConstantesBase.FormatoFecha), idsRecurso, strCentralInt, idsEquipo, tptomedicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicionxintervaloDTO entity = helper.Create(dr);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));
                    int iTipoptomedinomb = dr.GetOrdinal(helper.Tipoptomedinomb);
                    if (!dr.IsDBNull(iTipoptomedinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iTipoinfoabrev = dr.GetOrdinal(helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);
                    int iPtomedinomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedinomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedinomb);
                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iFamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);
                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = dr.GetInt32(iFenergcodi);
                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);
                    int iFenercolor = dr.GetOrdinal(helper.Fenercolor);
                    if (!dr.IsDBNull(iFenercolor)) entity.Fenercolor = dr.GetString(iFenercolor);
                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public List<MeMedicionxintervaloDTO> GetListaMedxintervStockPag(int lectCodi, int origlectcodi, string sEmprCodi, int famCodi, DateTime fechaInicial,
            DateTime fechaFinal, string idsEstado, string idsRecurso, string strCentralInt, string idsEquipo, int nroPaginas, int pageSize, string tptomedicodi)
        {
            List<MeMedicionxintervaloDTO> entitys = new List<MeMedicionxintervaloDTO>();
            string sqlQuery = string.Format(helper.SqlGetListaMedxintervStockPag, lectCodi, origlectcodi, sEmprCodi, famCodi, fechaInicial.ToString(ConstantesBase.FormatoFecha),
                fechaFinal.ToString(ConstantesBase.FormatoFecha), idsRecurso, strCentralInt, idsEquipo, nroPaginas, pageSize, tptomedicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicionxintervaloDTO entity = helper.Create(dr);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));
                    int iTipoptomedinomb = dr.GetOrdinal(helper.Tipoptomedinomb);
                    if (!dr.IsDBNull(iTipoptomedinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iTipoinfoabrev = dr.GetOrdinal(helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);
                    int iPtomedinomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedinomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedinomb);
                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iFamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);
                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);
                    int iFenercolor = dr.GetOrdinal(helper.Fenercolor);
                    if (!dr.IsDBNull(iFenercolor)) entity.Fenercolor = dr.GetString(iFenercolor);
                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public List<MeMedicionxintervaloDTO> GetListaMedxintervConsumo(int lectCodi, int origlectcodi, string sEmprCodi, DateTime fechaInicial, DateTime fechaFinal, string idsEstado, string idsRecurso, string strCentralInt, string ptomedicodi)
        {
            List<MeMedicionxintervaloDTO> entitys = new List<MeMedicionxintervaloDTO>();
            string sqlQuery = string.Format(helper.SqlGetListaMedxintervConsumo, lectCodi, origlectcodi, sEmprCodi, fechaInicial.ToString(ConstantesBase.FormatoFecha),
                fechaFinal.ToString(ConstantesBase.FormatoFecha), idsRecurso, strCentralInt, ptomedicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicionxintervaloDTO entity = helper.Create(dr);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));
                    int iTipoptomedinomb = dr.GetOrdinal(helper.Tipoptomedinomb);
                    if (!dr.IsDBNull(iTipoptomedinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);
                    int iTipoinfoabrev = dr.GetOrdinal(helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);
                    int iPtomedinomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedinomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedinomb);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprcoes = dr.GetOrdinal(helper.Emprcoes);
                    if (!dr.IsDBNull(iEmprcoes)) entity.Emprcoes = dr.GetString(iEmprcoes);
                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));
                    int iEquipopadre = dr.GetOrdinal(helper.Equipopadre);
                    if (!dr.IsDBNull(iEquipopadre)) entity.Equipopadre = dr.GetString(iEquipopadre);
                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = dr.GetInt32(iFenergcodi);
                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);
                    int iFenercolor = dr.GetOrdinal(helper.Fenercolor);
                    if (!dr.IsDBNull(iFenercolor)) entity.Fenercolor = dr.GetString(iFenercolor);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    if (entity.Ptomedicodi == this.PtoMedUti5)
                    {
                        entity.Equinomb = entity.Ptomedielenomb;
                    }
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicionxintervaloDTO> GetListaMedxintervDisponibilidad(int lectCodi, int origlectcodi, string sEmprCodi, DateTime fechaInicial, DateTime fechaFinal, string strCentralInt, int idYacimGas, string idsYacimientos)
        {
            List<MeMedicionxintervaloDTO> entitys = new List<MeMedicionxintervaloDTO>();
            string sqlQuery = string.Format(helper.SqlGetListaMedxintervDisponibilidad, lectCodi, origlectcodi, sEmprCodi, fechaInicial.ToString(ConstantesBase.FormatoFecha),
                fechaFinal.ToString(ConstantesBase.FormatoFecha), strCentralInt, idsYacimientos, idYacimGas);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicionxintervaloDTO entity = helper.Create(dr);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));
                    int iTipoptomedinomb = dr.GetOrdinal(helper.Tipoptomedinomb);
                    if (!dr.IsDBNull(iTipoptomedinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iTipoinfoabrev = dr.GetOrdinal(helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);
                    int iPtomedinomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedinomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedinomb);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeMedicionxintervaloDTO> GetListaMedxintervQuema(int lectCodi, int origlectcodi, string sEmprCodi, DateTime fechaInicial, DateTime fechaFinal, string strCentralInt)
        {
            List<MeMedicionxintervaloDTO> entitys = new List<MeMedicionxintervaloDTO>();
            string sqlQuery = string.Format(helper.SqlGetListaMedxintervQuema, lectCodi, origlectcodi, sEmprCodi, fechaInicial.ToString(ConstantesBase.FormatoFecha),
                fechaFinal.ToString(ConstantesBase.FormatoFecha), strCentralInt);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicionxintervaloDTO entity = helper.Create(dr);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));
                    int iTipoptomedinomb = dr.GetOrdinal(helper.Tipoptomedinomb);
                    if (!dr.IsDBNull(iTipoptomedinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iTipoinfoabrev = dr.GetOrdinal(helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);
                    int iPtomedinomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedinomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedinomb);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeMedicionxintervaloDTO> GetConsumoCentral(DateTime fecha, int idformato, int idempresa)
        {
            List<MeMedicionxintervaloDTO> entitys = new List<MeMedicionxintervaloDTO>();
            string sqlQuery = string.Format(helper.SqlGetConsumoCentral, fecha.ToString(ConstantesBase.FormatoFecha), idempresa, idformato);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            MeMedicionxintervaloDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MeMedicionxintervaloDTO();
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iMedinth1 = dr.GetOrdinal(helper.Medinth1);
                    if (!dr.IsDBNull(iMedinth1)) entity.Medinth1 = dr.GetDecimal(iMedinth1);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public void DeleteEnvioFormato(DateTime fechaInicio, DateTime fechaFin, int idFormato, int idEmpresa)
        {
            string sqlDelete = string.Format(helper.SqlDeleteEnvioFormato, fechaInicio.ToString(ConstantesBase.FormatoFechaExtendido),
              fechaFin.ToString(ConstantesBase.FormatoFechaExtendido), idFormato, idEmpresa);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlDelete);
            dbProvider.ExecuteNonQuery(command);
        }


        public void DeleteEnvioFormatoHojaColumna(DateTime fechaInicio, DateTime fechaFin, int idFormato, int idEmpresa, int hoja, string sTptomedicion)
        {
            string sqlDelete = string.Format(helper.SqlDeleteEnvioFormatoColumna, fechaInicio.ToString(ConstantesBase.FormatoFecha),
              fechaFin.ToString(ConstantesBase.FormatoFecha), idFormato, idEmpresa, hoja, sTptomedicion);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlDelete);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<MeMedicionxintervaloDTO> List(int ptomedicodi, int lectcodi, int tipoinfocodi, DateTime fechaInicio,
            DateTime fechaFin)
        {
            List<MeMedicionxintervaloDTO> entitys = new List<MeMedicionxintervaloDTO>();

            string sqlQuery = string.Format(helper.SqlListaFiltrada, ptomedicodi, lectcodi, tipoinfocodi,
                fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));

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


        public void Delete(int medintcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Medintcodi, DbType.Int32, medintcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<MeMedicionxintervaloDTO> GetEnvioArchivo(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicionxintervaloDTO> entitys = new List<MeMedicionxintervaloDTO>();
            string queryString = string.Format(helper.SqlGetEnvioArchivo, idFormato, idEmpresa, fechaInicio.ToString(ConstantesBase.FormatoFechaExtendido), fechaFin.ToString(ConstantesBase.FormatoFechaExtendido));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            MeMedicionxintervaloDTO entity = new MeMedicionxintervaloDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public void DeleteEnvioArchivo(int enviocodi)
        {
            string sqlDelete = string.Format(helper.SqlDeleteEnvioMedicionxIntervalo, enviocodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlDelete);
            dbProvider.ExecuteNonQuery(command);
        }

        public int SaveTransaccional(MeMedicionxintervaloDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            entity.Tptomedicodi = entity.Tptomedicodi > 0 ? entity.Tptomedicodi : -1;
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Medintcodi, DbType.Int32, entity.Medintcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Medintfechaini, DbType.DateTime, entity.Medintfechaini));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Medintfechafin, DbType.DateTime, entity.Medintfechafin));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Lectcodi, DbType.Int32, entity.Lectcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Medinth1, DbType.Decimal, entity.Medinth1));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Medintusumodificacion, DbType.String, entity.Medintusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Medintfecmodificacion, DbType.DateTime, entity.Medintfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Medintdescrip, DbType.String, entity.Medintdescrip));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Medestcodi, DbType.Int32, entity.Medestcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Medintsemana, DbType.Int32, entity.Medintsemana));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Medintanio, DbType.DateTime, entity.Medintanio));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Medintblqhoras, DbType.Decimal, entity.Medintblqhoras));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Medintblqnumero, DbType.Int32, entity.Medintblqnumero));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Enviocodi, DbType.Int32, entity.Enviocodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Tptomedicodi, DbType.Int32, entity.Tptomedicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Emprcodi, DbType.Int32, entity.Emprcodi));

                dbCommand.ExecuteNonQuery();

                return entity.Medintcodi;
            }
        }

        public List<MeMedicionxintervaloDTO> BuscarRegistroPeriodo(DateTime fechaini, DateTime fechafin, int ptomedicodi, int tipoinfocodi, int lectcodi)
        {
            List<MeMedicionxintervaloDTO> entitys = new List<MeMedicionxintervaloDTO>();
            string queryString = string.Format(helper.SqlBuscarRegistroPeriodo, fechaini.ToString(ConstantesBase.FormatoFecha),
                fechafin.ToString(ConstantesBase.FormatoFecha),
                ptomedicodi, tipoinfocodi, lectcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            MeMedicionxintervaloDTO entity = new MeMedicionxintervaloDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region Pr31

        public List<MeMedicionxintervaloDTO> GetCombustibleXCentral(DateTime fechaIni, DateTime fechaFin, string ptomedicion, int fenergcodi, int grupocodi)
        {
            List<MeMedicionxintervaloDTO> entitys = new List<MeMedicionxintervaloDTO>();

            string queryString = string.Format(helper.SqlGetCombustibleXCentral, ptomedicion, fenergcodi, grupocodi
                                                , fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicionxintervaloDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region PMPO 

        public List<MeMedicionxintervaloDTO> ListarReporteGeneracionSDDP(int codigoenvio)
        {
            List<MeMedicionxintervaloDTO> entitys = new List<MeMedicionxintervaloDTO>();
            string queryString = string.Format(helper.SqlListarReporteGeneracionSDDP, codigoenvio);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            MeMedicionxintervaloDTO entity = new MeMedicionxintervaloDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MeMedicionxintervaloDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iSemana = dr.GetOrdinal(helper.Semana);
                    if (!dr.IsDBNull(iSemana)) entity.Semana = dr.GetString(iSemana);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iMedinth1 = dr.GetOrdinal(helper.Medinth1);
                    if (!dr.IsDBNull(iMedinth1)) entity.Medinth1 = dr.GetDecimal(iMedinth1);

                    int iMedintfechaini = dr.GetOrdinal(helper.Medintfechaini);
                    if (!dr.IsDBNull(iMedintfechaini)) entity.Medintfechaini = dr.GetDateTime(iMedintfechaini);

                    int iCatecodi = dr.GetOrdinal(this.helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iTgenercodi = dr.GetOrdinal(helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

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

        public List<MeMedicionxintervaloDTO> ListarReporteSDDP(int codigoenvio, string tptomedicodi, string ptomedicodi)
        {
            List<MeMedicionxintervaloDTO> entitys = new List<MeMedicionxintervaloDTO>();
            string queryString = string.Format(helper.SqlListarReporteSDDP, codigoenvio, tptomedicodi, ptomedicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            MeMedicionxintervaloDTO entity = new MeMedicionxintervaloDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MeMedicionxintervaloDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iBarrcodi = dr.GetOrdinal(helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = dr.GetInt32(iBarrcodi);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iPmbloqnombre = dr.GetOrdinal(helper.Pmbloqnombre);
                    if (!dr.IsDBNull(iPmbloqnombre)) entity.Pmbloqnombre = dr.GetString(iPmbloqnombre);

                    int iSemana = dr.GetOrdinal(helper.Semana);
                    if (!dr.IsDBNull(iSemana)) entity.Semana = dr.GetString(iSemana);

                    int iMedinth1 = dr.GetOrdinal(helper.Medinth1);
                    if (!dr.IsDBNull(iMedinth1)) entity.Medinth1 = dr.GetDecimal(iMedinth1);

                    int iMedintfechaini = dr.GetOrdinal(helper.Medintfechaini);
                    if (!dr.IsDBNull(iMedintfechaini)) entity.Medintfechaini = dr.GetDateTime(iMedintfechaini);

                    int iMedintanio = dr.GetOrdinal(helper.Medintanio);
                    if (!dr.IsDBNull(iMedintanio)) entity.Medintanio = dr.GetDateTime(iMedintanio);

                    int iMedintblqnumero = dr.GetOrdinal(helper.Medintblqnumero);
                    if (!dr.IsDBNull(iMedintblqnumero)) entity.Medintblqnumero = Convert.ToInt32(dr.GetValue(iMedintblqnumero));

                    int iTptomedicodi = dr.GetOrdinal(helper.Tptomedicodi);
                    if (!dr.IsDBNull(iTptomedicodi)) entity.Tptomedicodi = Convert.ToInt32(dr.GetValue(iTptomedicodi));
                    entity.Tptomedicodi = entity.Tptomedicodi > 0 ? entity.Tptomedicodi : -1;

                    int iTptomedinomb = dr.GetOrdinal(helper.Tptomedinomb);
                    if (!dr.IsDBNull(iTptomedinomb)) entity.Tptomedinomb = dr.GetString(iTptomedinomb);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);
                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

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

        #endregion

        #region FIT - VALORIZACION DIARIA
        public decimal GetDemandaMedianoPlazoCOES(int nWeek, DateTime date)
        {
            decimal DemandaCOES = 0;
            string query = string.Format(helper.SqlGetDemandaMedianoPlazoCOES, nWeek, date.Year);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {

                if (dr.Read())
                {
                    int iMedintfechaini = dr.GetOrdinal(helper.Medinth1);
                    if (!dr.IsDBNull(iMedintfechaini)) DemandaCOES = dr.GetDecimal(iMedintfechaini);
                }
            }

            return DemandaCOES;
        }
        #endregion

        #region siosein2

        /// <summary>
        /// GetListaMedicionXIntervaloByLecturaYTipomedicion
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="lectcodi"></param>
        /// <param name="tptomedicodi"></param>
        /// <param name="ptomedicodi"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        public List<MeMedicionxintervaloDTO> GetListaMedicionXIntervaloByLecturaYTipomedicion(DateTime fechaPerido, DateTime fechaInicio, DateTime fechaFin, int lectcodi, string tptomedicodi, string ptomedicodi)
        {
            List<MeMedicionxintervaloDTO> entitys = new List<MeMedicionxintervaloDTO>();
            string sql = string.Format(helper.SqlGetListaMedicionXIntervaloByLecturaYTipomedicion, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), lectcodi, tptomedicodi, ptomedicodi, fechaPerido.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicionxintervaloDTO entity = helper.Create(dr);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTgenercodi = dr.GetOrdinal(helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

                    int iTipogenerrer = dr.GetOrdinal(helper.Tipogenerrer);
                    if (!dr.IsDBNull(iTipogenerrer)) entity.Tipogenerrer = dr.GetString(iTipogenerrer);

                    int iGrupotipocogen = dr.GetOrdinal(helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    int iGrupointegrante = dr.GetOrdinal(helper.Grupointegrante);
                    if (!dr.IsDBNull(iGrupointegrante)) entity.Grupointegrante = dr.GetString(iGrupointegrante);

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicionxintervaloDTO> GetListaMedicionXIntervaloByLecturaYTipomedicionYCentral(DateTime fechaPerido, DateTime fechaInicio, DateTime fechaFin, int lectcodi, int tptomedicodi, string ptomedicodi)
        {
            List<MeMedicionxintervaloDTO> entitys = new List<MeMedicionxintervaloDTO>();
            string sql = string.Format(helper.SqlGetListaMedicionXIntervaloByLecturaYTipomedicionYCentral, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), lectcodi, tptomedicodi, ptomedicodi, fechaPerido.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicionxintervaloDTO entity = new MeMedicionxintervaloDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iMedintfechaini = dr.GetOrdinal(helper.Medintfechaini);
                    if (!dr.IsDBNull(iMedintfechaini)) entity.Medintfechaini = dr.GetDateTime(iMedintfechaini);

                    int iMedinth1 = dr.GetOrdinal(helper.Medinth1);
                    if (!dr.IsDBNull(iMedinth1)) entity.Medinth1 = dr.GetDecimal(iMedinth1);

                    int iTgenercodi = dr.GetOrdinal(helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

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

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region Mejoras RDO
        public List<MeMedicionxintervaloDTO> GetListaMedDisponibilidadCombustible(int lectCodi, int origlectcodi, string sEmprCodi, DateTime fechaInicial, DateTime fechaFinal, string idsEstado, string idsRecurso, string strCentralInt, string ptomedicodi, string horario)
        {
            List<MeMedicionxintervaloDTO> entitys = new List<MeMedicionxintervaloDTO>();
            string sqlQuery = string.Format(helper.SqlGetListaDisponibilidadCombustible, lectCodi, origlectcodi, sEmprCodi, fechaInicial.ToString(ConstantesBase.FormatoFecha),
                fechaFinal.ToString(ConstantesBase.FormatoFecha), idsRecurso, strCentralInt, ptomedicodi, Convert.ToInt32(horario));

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicionxintervaloDTO entity = helper.Create(dr);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));
                    int iTipoptomedinomb = dr.GetOrdinal(helper.Tipoptomedinomb);
                    if (!dr.IsDBNull(iTipoptomedinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);
                    int iTipoinfoabrev = dr.GetOrdinal(helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);
                    int iPtomedinomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedinomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedinomb);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprcoes = dr.GetOrdinal(helper.Emprcoes);
                    if (!dr.IsDBNull(iEmprcoes)) entity.Emprcoes = dr.GetString(iEmprcoes);
                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));
                    int iEquipopadre = dr.GetOrdinal(helper.Equipopadre);
                    if (!dr.IsDBNull(iEquipopadre)) entity.Equipopadre = dr.GetString(iEquipopadre);
                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = dr.GetInt32(iFenergcodi);
                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);
                    int iFenercolor = dr.GetOrdinal(helper.Fenercolor);
                    if (!dr.IsDBNull(iFenercolor)) entity.Fenercolor = dr.GetString(iFenercolor);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    if (entity.Ptomedicodi == this.PtoMedUti5)
                    {
                        entity.Equinomb = entity.Ptomedielenomb;
                    }
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public void SaveRDO(MeMedicionxintervaloDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);
            entity.Tptomedicodi = entity.Tptomedicodi > 0 ? entity.Tptomedicodi : -1;
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            command = dbProvider.GetSqlStringCommand(helper.SqlSaveintervaloRDO);

            dbProvider.AddInParameter(command, helper.Medintcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Medintfechaini, DbType.DateTime, entity.Medintfechaini);
            dbProvider.AddInParameter(command, helper.Medintfechafin, DbType.DateTime, entity.Medintfechaini);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Medinth1, DbType.Decimal, entity.Medinth1);
            dbProvider.AddInParameter(command, helper.Medintusumodificacion, DbType.String, entity.Medintusumodificacion);
            dbProvider.AddInParameter(command, helper.Medintfecmodificacion, DbType.DateTime, entity.Medintfecmodificacion);
            dbProvider.AddInParameter(command, helper.Medintdescrip, DbType.String, entity.Medintdescrip);
            dbProvider.AddInParameter(command, helper.Medestcodi, DbType.Int32, entity.Medestcodi);
            dbProvider.AddInParameter(command, helper.Medintsemana, DbType.Int32, entity.Medintsemana);
            dbProvider.AddInParameter(command, helper.Medintanio, DbType.DateTime, entity.Medintanio);
            dbProvider.AddInParameter(command, helper.Medintblqhoras, DbType.Decimal, entity.Medintblqhoras);
            dbProvider.AddInParameter(command, helper.Medintblqnumero, DbType.Int32, entity.Medintblqnumero);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, entity.Enviocodi);
            dbProvider.AddInParameter(command, helper.Tptomedicodi, DbType.Int32, entity.Tptomedicodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }
        public List<MeMedicionxintervaloDTO> GetEnvioArchivoRDO(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin, string horario)
        {
            List<MeMedicionxintervaloDTO> entitys = new List<MeMedicionxintervaloDTO>();
            string queryString = string.Format(helper.SqlGetEnvioArchivoRDO, idFormato, idEmpresa, fechaInicio.ToString(ConstantesBase.FormatoFechaExtendido), fechaFin.ToString(ConstantesBase.FormatoFechaExtendido), horario);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            MeMedicionxintervaloDTO entity = new MeMedicionxintervaloDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region PrimasRER.2023
        public List<MeMedicionxintervaloDTO> ListarBarrasPMPO(string fechaInicio, string fechaFin)
        {
            List<MeMedicionxintervaloDTO> entitys = new List<MeMedicionxintervaloDTO>();
            string queryString = string.Format(helper.SqlListarBarrasPMPO, fechaInicio, fechaFin);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicionxintervaloDTO entity = new MeMedicionxintervaloDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicionxintervaloDTO> ListarCentralesPMPO(int emprcodi)
        {
            List<MeMedicionxintervaloDTO> entitys = new List<MeMedicionxintervaloDTO>();
            string queryString = string.Format(helper.SqlListarCentralesPMPO, emprcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicionxintervaloDTO entity = new MeMedicionxintervaloDTO();

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

        #endregion
    }
}
