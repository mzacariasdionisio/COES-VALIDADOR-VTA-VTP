using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla ME_REPORPTOMED
    /// </summary>
    public class MeReporptomedRepository : RepositoryBase, IMeReporptomedRepository
    {
        public MeReporptomedRepository(string strConn)
            : base(strConn)
        {
        }

        MeReporptomedHelper helper = new MeReporptomedHelper();

        public int Save(MeReporptomedDTO entity)
        {
            if (entity.Tipoinfocodi <= 0) entity.Tipoinfocodi = -1;
            if (entity.Lectcodi <= 0) entity.Lectcodi = -1;
            if (entity.Repptotabmed <= 0) entity.Repptotabmed = -1;
            if (entity.Tipoptomedicodi <= 0) entity.Tipoptomedicodi = -1;
            if (entity.Funptocodi <= 0) entity.Funptocodi = -1;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Repptocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Reporcodi, DbType.Int32, entity.Reporcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Repptoorden, DbType.Int32, entity.Repptoorden);
            dbProvider.AddInParameter(command, helper.Repptoestado, DbType.Int32, entity.Repptoestado);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Repptotabmed, DbType.Int32, entity.Repptotabmed);
            dbProvider.AddInParameter(command, helper.Tipoptomedicodi, DbType.Int32, entity.Tipoptomedicodi);
            dbProvider.AddInParameter(command, helper.Funptocodi, DbType.Int32, entity.Funptocodi);
            dbProvider.AddInParameter(command, helper.Repptonomb, DbType.String, entity.Repptonomb);
            dbProvider.AddInParameter(command, helper.Repptocolorcelda, DbType.String, entity.Repptocolorcelda);
            dbProvider.AddInParameter(command, helper.Repptoequivpto, DbType.Int32, entity.Repptoequivpto);
            dbProvider.AddInParameter(command, helper.Repptoindcopiado, DbType.String, entity.Repptoindcopiado);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void Update(MeReporptomedDTO entity)
        {
            if (entity.Tipoinfocodi <= 0) entity.Tipoinfocodi = -1;
            if (entity.Lectcodi <= 0) entity.Lectcodi = -1;
            if (entity.Repptotabmed <= 0) entity.Repptotabmed = -1;
            if (entity.Tipoptomedicodi <= 0) entity.Tipoptomedicodi = -1;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Repptoorden, DbType.Int32, entity.Repptoorden);
            dbProvider.AddInParameter(command, helper.Repptoestado, DbType.Int32, entity.Repptoestado);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Tipoptomedicodi, DbType.Int32, entity.Tipoptomedicodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Repptotabmed, DbType.Int32, entity.Repptotabmed);
            dbProvider.AddInParameter(command, helper.Repptonomb, DbType.String, entity.Repptonomb);
            dbProvider.AddInParameter(command, helper.Repptocolorcelda, DbType.String, entity.Repptocolorcelda);
            dbProvider.AddInParameter(command, helper.Repptoequivpto, DbType.Int32, entity.Repptoequivpto);
            dbProvider.AddInParameter(command, helper.Repptoindcopiado, DbType.String, entity.Repptoindcopiado);
            dbProvider.AddInParameter(command, helper.Repptocodi, DbType.Int32, entity.Repptocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int repptocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Repptocodi, DbType.Int32, repptocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeReporptomedDTO GetById(int repptocodi)
        {
            string query = string.Format(helper.SqlGetById, repptocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            MeReporptomedDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public MeReporptomedDTO GetById2(int reporcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById2);
            dbProvider.AddInParameter(command, helper.Reporcodi, DbType.Int32, reporcodi);

            MeReporptomedDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }


        public MeReporptomedDTO GetById3(int reporcodi, int ptomedicodi, int lectcodi, int tipoinfocodi, int tptomedicodi)
        {
            string sqlQuery = string.Format(helper.SqlGetById3, reporcodi, ptomedicodi, lectcodi, tipoinfocodi, tptomedicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            MeReporptomedDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeReporptomedDTO> List()
        {
            List<MeReporptomedDTO> entitys = new List<MeReporptomedDTO>();
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

        public List<MeReporptomedDTO> GetByCriteria(int reporcodi, int ptomedicodi)
        {
            List<MeReporptomedDTO> entitys = new List<MeReporptomedDTO>();
            string sqlQuery = string.Format(helper.SqlGetByCriteria, reporcodi, ptomedicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            MeReporptomedDTO entity = new MeReporptomedDTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iPtomedibarranomb = dr.GetOrdinal(this.helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedibarranomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedibarranomb);

                    int iPtomediCalculado = dr.GetOrdinal(this.helper.PtomediCalculado);
                    if (!dr.IsDBNull(iPtomediCalculado)) entity.PtomediCalculado = dr.GetString(iPtomediCalculado);

                    int iPtomedielenomb = dr.GetOrdinal(this.helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iRepptotabmed = dr.GetOrdinal(this.helper.Repptotabmed);
                    if (!dr.IsDBNull(iRepptotabmed)) entity.Repptotabmed = dr.GetInt32(iRepptotabmed);

                    int iRepptonomb = dr.GetOrdinal(this.helper.Repptonomb);
                    if (!dr.IsDBNull(iRepptonomb)) entity.Repptonomb = dr.GetString(iRepptonomb);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iTipoinfoabrev = dr.GetOrdinal(helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeReporptomedDTO> GetByCriteria2(int reporcodi, string query)
        {
            List<MeReporptomedDTO> entitys = new List<MeReporptomedDTO>();
            string queryString = string.Format(this.helper.GetQueryMeReporptomed(query), reporcodi);

            MeReporptomedDTO entity = null;
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                var dtable = dr.GetSchemaTable();
                object find = null;
                List<string> listaCampo = new List<string>();
                foreach (DataRow reg in dtable.Rows)
                {
                    listaCampo.Add(reg.ItemArray[0].ToString());
                }

                while (dr.Read())
                {
                    try
                    {
                        entity = helper.Create(dr);

                        //punto de medicion
                        find = listaCampo.Find(x => x.ToUpper() == helper.Ptomedibarranomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Ptomedibarranomb))) entity.Ptomedibarranomb = dr.GetString(dr.GetOrdinal(helper.Ptomedibarranomb));
                        }
                        find = listaCampo.Find(x => x.ToUpper() == helper.Ptomedidesc.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Ptomedidesc))) entity.Ptomedidesc = dr.GetString(dr.GetOrdinal(helper.Ptomedidesc));
                        }
                        find = listaCampo.Find(x => x.ToUpper() == helper.PtoMediEleNomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.PtoMediEleNomb))) entity.Ptomedielenomb = dr.GetString(dr.GetOrdinal(helper.PtoMediEleNomb));
                        }

                        //medida
                        find = listaCampo.Find(x => x.ToUpper() == helper.Tipoinfoabrev.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Tipoinfoabrev))) entity.Tipoinfoabrev = dr.GetString(dr.GetOrdinal(helper.Tipoinfoabrev));
                        }

                        //tipo punto de medicion
                        find = listaCampo.Find(x => x.ToUpper() == helper.Tipoptomedicodi.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Tipoptomedicodi))) entity.Tipoptomedicodi = dr.GetInt32(dr.GetOrdinal(helper.Tipoptomedicodi));
                        }
                        find = listaCampo.Find(x => x.ToUpper() == helper.Tipoptomedinomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Tipoptomedinomb))) entity.Tipoptomedinomb = dr.GetString(dr.GetOrdinal(helper.Tipoptomedinomb));
                        }

                        //equipo
                        find = listaCampo.Find(x => x.ToUpper() == helper.Equicodi.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Equicodi))) entity.Equicodi = dr.GetInt32(dr.GetOrdinal(helper.Equicodi));
                        }
                        find = listaCampo.Find(x => x.ToUpper() == helper.Equinomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Equinomb))) entity.Equinomb = dr.GetString(dr.GetOrdinal(helper.Equinomb));
                        }

                        //tipo de equipo
                        find = listaCampo.Find(x => x.ToUpper() == helper.Famcodi.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Famcodi))) entity.Famcodi = dr.GetInt32(dr.GetOrdinal(helper.Famcodi));
                        }
                        find = listaCampo.Find(x => x.ToUpper() == helper.Famabrev.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Famabrev))) entity.Famabrev = dr.GetString(dr.GetOrdinal(helper.Famabrev));
                        }

                        //empresa
                        find = listaCampo.Find(x => x.ToUpper() == helper.Emprcodi.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Emprcodi))) entity.Emprcodi = dr.GetInt32(dr.GetOrdinal(helper.Emprcodi));
                        }
                        find = listaCampo.Find(x => x.ToUpper() == helper.Emprnomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Emprnomb))) entity.Emprnomb = dr.GetString(dr.GetOrdinal(helper.Emprnomb));
                        }
                        find = listaCampo.Find(x => x.ToUpper() == helper.Emprabrev.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Emprabrev))) entity.Emprabrev = dr.GetString(dr.GetOrdinal(helper.Emprabrev));
                        }

                        //area
                        find = listaCampo.Find(x => x.ToUpper() == helper.Areacodi.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Areacodi))) entity.Areacodi = dr.GetInt32(dr.GetOrdinal(helper.Areacodi));
                        }
                        find = listaCampo.Find(x => x.ToUpper() == helper.Areanomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Areanomb))) entity.Areanomb = dr.GetString(dr.GetOrdinal(helper.Areanomb));
                        }

                        find = listaCampo.Find(x => x.ToUpper() == helper.Subestacioncodi.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Subestacioncodi))) entity.Subestacioncodi = dr.GetInt32(dr.GetOrdinal(helper.Subestacioncodi));
                        }
                        find = listaCampo.Find(x => x.ToUpper() == helper.AreaOperativa.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.AreaOperativa))) entity.AreaOperativa = dr.GetString(dr.GetOrdinal(helper.AreaOperativa));
                        }

                        //otros
                        find = listaCampo.Find(x => x.ToUpper() == helper.Valor1.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Valor1))) entity.Valor1 = Convert.ToDecimal(dr.GetValue(dr.GetOrdinal(helper.Valor1))).ToString();
                        }
                        find = listaCampo.Find(x => x.ToUpper() == helper.Valor4.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Valor4))) entity.Valor4 = dr.GetString(dr.GetOrdinal(helper.Valor4));
                        }
                        find = listaCampo.Find(x => x.ToUpper() == helper.Valor5.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Valor5))) entity.Valor5 = dr.GetString(dr.GetOrdinal(helper.Valor5));
                        }

                        entitys.Add(entity);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            return entitys;
        }

        public List<MeReporptomedDTO> ListarEncabezado(int reporcodi, string idsEmpresa, string idsTipoPtoMed)
        {
            List<MeReporptomedDTO> entitys = new List<MeReporptomedDTO>();
            string sqlQuery = string.Format(helper.SqlListEncabezado, reporcodi, idsEmpresa, idsTipoPtoMed);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeReporptomedDTO entity = helper.Create(dr);
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iCuenca = dr.GetOrdinal(helper.Cuenca);
                    if (!dr.IsDBNull(iCuenca)) entity.Cuenca = dr.GetString(iCuenca);
                    int iFamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);
                    int iTipoptomedinomb = dr.GetOrdinal(helper.Tipoptomedinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);
                    int iTipoinfoabrev = dr.GetOrdinal(helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);
                    int iPtomedinomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedinomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedinomb);
                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);
                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iTipoptomedicodi = dr.GetOrdinal(helper.Tipoptomedicodi);
                    if (!dr.IsDBNull(iTipoptomedicodi)) entity.Tipoptomedicodi = Convert.ToInt32(dr.GetValue(iTipoptomedicodi));
                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeReporptomedDTO> ListarPuntoReporte(int reporcodi, DateTime fechaPeriodo)
        {
            List<MeReporptomedDTO> entitys = new List<MeReporptomedDTO>();

            string sqlQuery = string.Format(helper.SqlListarPuntoReporte, reporcodi, fechaPeriodo.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                MeReporptomedDTO entity;
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprabrev = dr.GetOrdinal(this.helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);
                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int ifamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(ifamcodi)) entity.Famcodi = dr.GetInt32(ifamcodi);
                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);
                    int iFamAbrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamAbrev)) entity.Famabrev = dr.GetString(iFamAbrev);

                    int iPtomedibarranomb = dr.GetOrdinal(this.helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedibarranomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedibarranomb);
                    int iPtomedielenomb = dr.GetOrdinal(this.helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);
                    int iPtomedidesc = dr.GetOrdinal(this.helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);
                    int iPtomedicalculado = dr.GetOrdinal(this.helper.PtomediCalculado);
                    if (!dr.IsDBNull(iPtomedicalculado)) entity.PtomediCalculado = dr.GetString(iPtomedicalculado);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));
                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    // SIOSEIN-PRIE-2021
                    int iCodigoOsinergmin = dr.GetOrdinal(this.helper.CodigoOsinergmin);
                    if (!dr.IsDBNull(iCodigoOsinergmin)) entity.CodigoOsinergmin = dr.GetString(iCodigoOsinergmin);
                    //

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iEquiAbrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.Equiabrev = dr.GetString(iEquiAbrev);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);
                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iTipoinfoabrev = dr.GetOrdinal(this.helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);

                    int iTipoptomedicodi = dr.GetOrdinal(this.helper.Tipoptomedicodi);
                    if (!dr.IsDBNull(iTipoptomedicodi)) entity.Tipoptomedicodi = Convert.ToInt16(dr.GetValue(iTipoptomedicodi));
                    int iTptomedinomb = dr.GetOrdinal(this.helper.Tptomedinomb);
                    if (!dr.IsDBNull(iTptomedinomb)) entity.Tipoptomedinomb = dr.GetString(iTptomedinomb);

                    int iOriglectcodi = dr.GetOrdinal(this.helper.Origlectcodi);
                    if (!dr.IsDBNull(iOriglectcodi)) entity.Origlectcodi = Convert.ToInt32(dr.GetValue(iOriglectcodi));
                    int iOriglectnombre = dr.GetOrdinal(helper.Origlectnombre);
                    if (!dr.IsDBNull(iOriglectnombre)) entity.Origlectnombre = dr.GetString(iOriglectnombre);

                    int iLectnomb = dr.GetOrdinal(helper.Lectnomb);
                    if (!dr.IsDBNull(iLectnomb)) entity.Lectnomb = dr.GetString(iLectnomb);

                    int iRepptotabmed = dr.GetOrdinal(this.helper.Repptotabmed);
                    if (!dr.IsDBNull(iRepptotabmed)) entity.Repptotabmed = Convert.ToInt32(dr.GetValue(iRepptotabmed));

                    int iRepptonomb = dr.GetOrdinal(this.helper.Repptonomb);
                    if (!dr.IsDBNull(iRepptonomb)) entity.Repptonomb = dr.GetString(iRepptonomb);

                    int iFunptocodi = dr.GetOrdinal(this.helper.Funptocodi);
                    if (!dr.IsDBNull(iFunptocodi)) entity.Funptocodi = dr.GetInt32(iFunptocodi);

                    int iFunptofuncion = dr.GetOrdinal(this.helper.Funptofuncion);
                    if (!dr.IsDBNull(iFunptofuncion)) entity.Funptofuncion = dr.GetString(iFunptofuncion);

                    int iOsicodi = dr.GetOrdinal(this.helper.Osicodi);
                    if (!dr.IsDBNull(iOsicodi)) entity.Osicodi = dr.GetString(iOsicodi);

                    int iCodref = dr.GetOrdinal(this.helper.Codref);
                    if (!dr.IsDBNull(iCodref)) entity.Codref = Convert.ToInt32(dr.GetValue(iCodref));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int GetOrdenPto(int reporcodi, int empresa)
        {
            string sqlMax = string.Format(helper.SqlGetMaxOrder, reporcodi, empresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlMax);
            object result = dbProvider.ExecuteScalar(command);
            int order = 1;
            if (result != null) order = Convert.ToInt32(result);

            return order;
        }

        public List<DateTime> PaginacionReporte(int idReporte, int lectnro, DateTime fechaInicio, DateTime fechaFin)
        {
            List<DateTime> entitys = new List<DateTime>();
            DateTime fecha;
            string strQuery = string.Format(helper.SqlPaginacionReporte, lectnro, idReporte, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(strQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iFecha = dr.GetOrdinal("fecha");
                    if (!dr.IsDBNull(iFecha))
                    {
                        fecha = dr.GetDateTime(iFecha);
                        entitys.Add(fecha);
                    }
                }
            }
            return entitys;
        }

        public List<MeReporptomedDTO> ListarEncabezadoPowel(int reporcodi)
        {
            List<MeReporptomedDTO> entitys = new List<MeReporptomedDTO>();
            string sqlQuery = string.Format(helper.SqlListEncabezadoPowel, reporcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeReporptomedDTO entity = helper.Create(dr);
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iFamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);
                    int iTipoptomedinomb = dr.GetOrdinal(helper.Tipoptomedinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);
                    int iTipoinfoabrev = dr.GetOrdinal(helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);
                    int iPtomedinomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedinomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedinomb);
                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);
                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iTipoptomedicodi = dr.GetOrdinal(helper.Tipoptomedicodi);
                    if (!dr.IsDBNull(iTipoptomedicodi)) entity.Tipoptomedicodi = Convert.ToInt32(dr.GetValue(iTipoptomedicodi));
                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
