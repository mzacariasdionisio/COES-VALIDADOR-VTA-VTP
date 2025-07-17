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
    /// Clase de acceso a datos de la tabla ME_HOJAPTOMED
    /// </summary>
    public class MeHojaptomedRepository : RepositoryBase, IMeHojaptomedRepository
    {
        public MeHojaptomedRepository(string strConn)
            : base(strConn)
        {
        }

        MeHojaptomedHelper helper = new MeHojaptomedHelper();


        public void Save(MeHojaptomedDTO entity, int empresa)
        {
            DbCommand command;
            int order = entity.Hojaptoorden;
            if (empresa > 0)
            {
                string sqlMax = string.Format(helper.SqlGetMaxOrder, entity.Formatcodi, empresa);
                command = dbProvider.GetSqlStringCommand(sqlMax);
                object result = dbProvider.ExecuteScalar(command);

                order = 1;
                if (result != null) order = Convert.ToInt32(result);
            }

            command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object resultID = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (resultID != null) id = Convert.ToInt32(resultID);
            entity.Tptomedicodi = entity.Tptomedicodi > 0 ? entity.Tptomedicodi : -1;

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Hojaptocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Hojaptolimsup, DbType.Decimal, entity.Hojaptolimsup);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);
            dbProvider.AddInParameter(command, helper.Hojaptoliminf, DbType.Decimal, entity.Hojaptoliminf);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Hojaptoactivo, DbType.Int32, entity.Hojaptoactivo);
            dbProvider.AddInParameter(command, helper.Hojaptoorden, DbType.Int32, order);
            dbProvider.AddInParameter(command, helper.Hojaptosigno, DbType.Int32, entity.Hojaptosigno);
            dbProvider.AddInParameter(command, helper.Hojacodi, DbType.Int32, entity.Hojacodi);
            dbProvider.AddInParameter(command, helper.Tptomedicodi, DbType.String, entity.Tptomedicodi);
            dbProvider.AddInParameter(command, helper.Hptoobservacion, DbType.String, entity.Hptoobservacion);
            dbProvider.AddInParameter(command, helper.Hptoindcheck, DbType.String, entity.Hptoindcheck);

            dbProvider.ExecuteNonQuery(command);

        }
        public void Update(MeHojaptomedDTO entity)
        {
            entity.Tptomedicodi = entity.Tptomedicodi > 0 ? entity.Tptomedicodi : -1;

            string sqlUptade = string.Format(helper.SqlUpdate, entity.Hojaptomedcodi, entity.Hojaptolimsup == null ? "null" : entity.Hojaptolimsup.ToString(), entity.Hojaptoliminf,
                entity.Lastuser == null ? "original" : entity.Lastuser,
                entity.Lastdate == null ? new DateTime(2000, 1, 1).ToString(ConstantesBase.FormatoFechaExtendido) : entity.Lastdate.Value.ToString(ConstantesBase.FormatoFechaExtendido),
                entity.Hojaptoactivo, entity.Hojaptoorden,
                entity.Tptomedicodi, entity.Hptoobservacion, entity.Hptoindcheck);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlUptade);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update2(MeHojaptomedDTO entity)
        {
            entity.Tptomedicodi = entity.Tptomedicodi > 0 ? entity.Tptomedicodi : -1;

            string sqlUptade = string.Format(helper.SqlUpdate, entity.Hojaptomedcodi, entity.Hojaptolimsup == null ? "null" : entity.Hojaptolimsup.ToString(), entity.Hojaptoliminf, entity.Lastuser,
                entity.Lastdate.Value.ToString(ConstantesBase.FormatoFechaExtendido), entity.Hojaptoactivo, entity.Hojaptoorden,
                entity.Tptomedicodi, entity.Hptoobservacion, entity.Hptoindcheck);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlUptade);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteById(int hojaptomedcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteById);
            dbProvider.AddInParameter(command, helper.Hojaptocodi, DbType.Int32, hojaptomedcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int formatcodi, int tipoinfocodi, int hojaptoorden, int ptomedicodi, int tptomedi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, formatcodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Hojaptoorden, DbType.Int32, hojaptoorden);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Tptomedicodi, DbType.Int32, tptomedi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteHoja(int formatcodi, int tipoinfocodi, int hojaptoorden, int ptomedicodi, int tptomedi, int hoja)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteHoja);
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, formatcodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Hojaptoorden, DbType.Int32, hojaptoorden);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Tptomedicodi, DbType.Int32, tptomedi);
            dbProvider.AddInParameter(command, helper.Hojacodi, DbType.Int32, hoja);

            dbProvider.ExecuteNonQuery(command);
        }


        public MeHojaptomedDTO GetById(int formatcodi, int tipoinfocodi, int ptomedicodi, int hojaptosigno, int tptomedicodi)
        {
            tptomedicodi = tptomedicodi != 0 ? tptomedicodi : -1;
            string sqlQuery = string.Format(helper.SqlGetById, formatcodi, tipoinfocodi, ptomedicodi, hojaptosigno, tptomedicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            MeHojaptomedDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                    entity.Tipoinfoabrev = dr.GetString(dr.GetOrdinal(helper.Tipoinfoabrev));
                    entity.Equinomb = dr.GetString(dr.GetOrdinal(helper.Equinomb));
                    entity.Emprabrev = dr.GetString(dr.GetOrdinal(helper.Emprabrev));
                    entity.Tipoptomedinomb = dr.GetString(dr.GetOrdinal(helper.Tipoptomedinomb));
                }
            }

            return entity;
        }

        public MeHojaptomedDTO GetByIdHoja(int formatcodi, int tipoinfocodi, int ptomedicodi, int hojaptosigno, int tptomedicodi, int hojacodi)
        {
            tptomedicodi = tptomedicodi != 0 ? tptomedicodi : -1;
            string sqlQuery = string.Format(helper.SqlGetByIdHoja, formatcodi, tipoinfocodi, ptomedicodi, hojaptosigno, tptomedicodi, hojacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            MeHojaptomedDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                    entity.Tipoinfoabrev = dr.GetString(dr.GetOrdinal(helper.Tipoinfoabrev));
                    entity.Equinomb = dr.GetString(dr.GetOrdinal(helper.Equinomb));
                    entity.Emprabrev = dr.GetString(dr.GetOrdinal(helper.Emprabrev));
                    entity.Tipoptomedinomb = dr.GetString(dr.GetOrdinal(helper.Tipoptomedinomb));
                }
            }

            return entity;
        }

        public List<MeHojaptomedDTO> List()
        {
            List<MeHojaptomedDTO> entitys = new List<MeHojaptomedDTO>();
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

        //inicio agregado
        public List<MeHojaptomedDTO> GetByCriteria(int emprcodi, int formatcodi, DateTime fechaIni, DateTime fechaFin)
        {
            List<MeHojaptomedDTO> entitys = new List<MeHojaptomedDTO>();
            string queryString = string.Format(helper.SqlGetByCriteria, emprcodi, formatcodi, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            MeHojaptomedDTO entity = new MeHojaptomedDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    entity.Tipoinfoabrev = dr.GetString(dr.GetOrdinal(helper.Tipoinfoabrev));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Equinomb))) entity.Equinomb = dr.GetString(dr.GetOrdinal(helper.Equinomb));
                    //- pr16.JDEL - Inicio 23/11/2016: Verificar que no sea null.

                    //Anterior
                    //entity.Emprabrev = dr.GetString(dr.GetOrdinal(helper.Emprabrev));
                    //entity.Tipoptomedicodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Tipoptomedicodi)));
                    //entity.Famcodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Famcodi)));
                    //entity.Tipoptomedinomb = dr.GetString(dr.GetOrdinal(helper.Tipoptomedinomb));

                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Emprcodi))) entity.Emprcodi = dr.GetInt32(dr.GetOrdinal(helper.Emprcodi));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Emprabrev))) entity.Emprabrev = dr.GetString(dr.GetOrdinal(helper.Emprabrev));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Emprnomb))) entity.Emprnomb = dr.GetString(dr.GetOrdinal(helper.Emprnomb));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Famcodi))) entity.Famcodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Famcodi)));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Tipoptomedinomb))) entity.Tipoptomedinomb = dr.GetString(dr.GetOrdinal(helper.Tipoptomedinomb));

                    //- JDEL Fin

                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Ptomedibarranomb))) entity.Ptomedibarranomb = dr.GetString(dr.GetOrdinal(helper.Ptomedibarranomb));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Ptomedidesc))) entity.Ptomedidesc = dr.GetString(dr.GetOrdinal(helper.Ptomedidesc));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Equicodi))) entity.Equicodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Equicodi)));

                    //-
                    //Inicio Assetec - DemandaPO
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Grupocodi))) entity.Grupocodi = dr.GetInt32(dr.GetOrdinal(helper.Grupocodi));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Areapadre))) entity.Areapadre = dr.GetInt32(dr.GetOrdinal(helper.Areapadre));
                    //Fin Assetec - DemandaPO
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region Modificación PR5-DemandaDiaria 29-11-2017
        public List<MeHojaptomedDTO> GetByCriteria2(int emprcodi, int formatcodi, string query, DateTime fechaIni, DateTime fechaFin)
        {
            List<MeHojaptomedDTO> entitys = new List<MeHojaptomedDTO>();
            string queryString = string.Format(query, emprcodi, formatcodi, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            MeHojaptomedDTO entity = new MeHojaptomedDTO();
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
                    entity = helper.Create(dr, false);
                    try
                    {
                        find = listaCampo.Find(x => x == helper.Tipoinfoabrev.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Tipoinfoabrev))) entity.Tipoinfoabrev = dr.GetString(dr.GetOrdinal(helper.Tipoinfoabrev));
                        }
                        find = listaCampo.Find(x => x == helper.Equinomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Equinomb))) entity.Equinomb = dr.GetString(dr.GetOrdinal(helper.Equinomb));
                        }
                        find = listaCampo.Find(x => x == helper.Emprabrev.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Emprabrev))) entity.Emprabrev = dr.GetString(dr.GetOrdinal(helper.Emprabrev));
                        }
                        find = listaCampo.Find(x => x == helper.Emprcodi.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Emprcodi))) entity.Emprcodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Emprcodi)));
                        }
                        find = listaCampo.Find(x => x == helper.Emprnomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Emprnomb))) entity.Emprnomb = dr.GetString(dr.GetOrdinal(helper.Emprnomb));
                        }
                        find = listaCampo.Find(x => x == helper.Famcodi.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Famcodi))) entity.Famcodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Famcodi)));
                        }
                        find = listaCampo.Find(x => x == helper.Tipoptomedinomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Tipoptomedinomb))) entity.Tipoptomedinomb = dr.GetString(dr.GetOrdinal(helper.Tipoptomedinomb));
                        }
                        find = listaCampo.Find(x => x == helper.Ptomedibarranomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Ptomedibarranomb))) entity.Ptomedibarranomb = dr.GetString(dr.GetOrdinal(helper.Ptomedibarranomb));
                        }
                        find = listaCampo.Find(x => x == helper.Ptomedidesc.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Ptomedidesc))) entity.Ptomedidesc = dr.GetString(dr.GetOrdinal(helper.Ptomedidesc));
                        }

                        find = listaCampo.Find(x => x == helper.Equipopadre.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Equipopadre))) entity.Equipopadre = dr.GetString(dr.GetOrdinal(helper.Equipopadre));
                        }
                        find = listaCampo.Find(x => x == helper.Areanomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Areanomb))) entity.Areanomb = dr.GetString(dr.GetOrdinal(helper.Areanomb));
                        }

                        find = listaCampo.Find(x => x == helper.AreaOperativa.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.AreaOperativa))) entity.AreaOperativa = dr.GetString(dr.GetOrdinal(helper.AreaOperativa));
                        }
                        find = listaCampo.Find(x => x == helper.Valor1.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Valor1))) entity.Valor1 = Convert.ToDecimal(dr.GetValue(dr.GetOrdinal(helper.Valor1))).ToString();
                        }

                        find = listaCampo.Find(x => x == helper.Equicodi.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Equicodi))) entity.Equicodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Equicodi)));
                        }

                        find = listaCampo.Find(x => x == helper.Equipadre.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Equipadre))) entity.Equipadre = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Equipadre)));
                        }

                        find = listaCampo.Find(x => x == helper.Equiabrev.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Equiabrev))) entity.Equiabrev = dr.GetString(dr.GetOrdinal(helper.Equiabrev));
                        }

                        find = listaCampo.Find(x => x == helper.Suministrador.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Suministrador))) entity.Suministrador = dr.GetString(dr.GetOrdinal(helper.Suministrador));
                        }

                        find = listaCampo.Find(x => x == helper.CodigoOsinergmin.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.CodigoOsinergmin))) entity.CodigoOsinergmin = dr.GetString(dr.GetOrdinal(helper.CodigoOsinergmin));
                        }

                        find = listaCampo.Find(x => x == helper.Unidad.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Unidad))) entity.Unidad = dr.GetString(dr.GetOrdinal(helper.Unidad));
                        }

                        find = listaCampo.Find(x => x == helper.PtoMediEleNomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.PtoMediEleNomb))) entity.PtoMediEleNomb = dr.GetString(dr.GetOrdinal(helper.PtoMediEleNomb));
                        }

                        find = listaCampo.Find(x => x == helper.Famabrev.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Famabrev))) entity.Famabrev = dr.GetString(dr.GetOrdinal(helper.Famabrev));
                        }

                        find = listaCampo.Find(x => x == helper.Areacodi.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Areacodi))) entity.Areacodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Areacodi)));
                        }

                        find = listaCampo.Find(x => x == helper.Gruponomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Gruponomb))) entity.Gruponomb = dr.GetString(dr.GetOrdinal(helper.Gruponomb));
                        }

                        find = listaCampo.Find(x => x == helper.Valor4.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Valor4))) entity.Valor4 = dr.GetString(dr.GetOrdinal(helper.Valor4));
                        }

                        find = listaCampo.Find(x => x == helper.Medidornomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Medidornomb))) entity.Medidornomb = dr.GetString(dr.GetOrdinal(helper.Medidornomb));
                        }

                        find = listaCampo.Find(x => x == helper.Medidorserie.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Medidorserie))) entity.Medidorserie = dr.GetString(dr.GetOrdinal(helper.Medidorserie));
                        }

                        find = listaCampo.Find(x => x == helper.Medidorclaseprecision.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Medidorclaseprecision))) entity.Medidorclaseprecision = dr.GetString(dr.GetOrdinal(helper.Medidorclaseprecision));
                        }

                        find = listaCampo.Find(x => x == helper.Valor5.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Valor5))) entity.Valor5 = dr.GetString(dr.GetOrdinal(helper.Valor5));
                        }

                        find = listaCampo.Find(x => x == helper.Clientenomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Clientenomb))) entity.Clientenomb = dr.GetString(dr.GetOrdinal(helper.Clientenomb));
                        }

                        find = listaCampo.Find(x => x == helper.Barranomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Barranomb))) entity.Barranomb = dr.GetString(dr.GetOrdinal(helper.Barranomb));
                        }


                        #region Mejoras IEOD

                        find = listaCampo.Find(x => x == helper.Equitension.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Equitension))) entity.Equitension = Convert.ToDecimal(dr.GetValue(dr.GetOrdinal(helper.Equitension)));
                        }
                        
                        #endregion

                        #region Mejoras RDO
                        find = listaCampo.Find(x => x == helper.Cuenca.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Cuenca))) entity.Cuenca = dr.GetString(dr.GetOrdinal(helper.Cuenca));
                        }

                        find = listaCampo.Find(x => x == helper.Tptomedinomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Tptomedinomb))) entity.Tptomedinomb = dr.GetString(dr.GetOrdinal(helper.Tptomedinomb));
                        }
                        #endregion

                        find = listaCampo.Find(x => x == helper.Hptoindcheck.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Hptoindcheck))) entity.Hptoindcheck = dr.GetString(dr.GetOrdinal(helper.Hptoindcheck));
                        }

                        entitys.Add(entity);
                    }
                    catch
                    {

                    }
                }
            }
            return entitys;
        }
        #endregion
        //fin agregado

        public List<SiEmpresaDTO> ObtenerEmpresasPorFormato(int idFormato)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEmpresaFormato);
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, idFormato);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }



        public List<MeHojaptomedDTO> GetPtoMedicionPR16(int emprcodi, int formatcodi, string periodo, string query)
        {
            List<MeHojaptomedDTO> entitys = new List<MeHojaptomedDTO>();
            string queryString = string.Format(query, emprcodi, formatcodi, periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            MeHojaptomedDTO entity = new MeHojaptomedDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Tipoinfoabrev))) entity.Tipoinfoabrev = dr.GetString(dr.GetOrdinal(helper.Tipoinfoabrev));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Equinomb))) entity.Equinomb = dr.GetString(dr.GetOrdinal(helper.Equinomb));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Emprabrev))) entity.Emprabrev = dr.GetString(dr.GetOrdinal(helper.Emprabrev));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Famcodi))) entity.Famcodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Famcodi)));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Tipoptomedinomb))) entity.Tipoptomedinomb = dr.GetString(dr.GetOrdinal(helper.Tipoptomedinomb));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Ptomedibarranomb))) entity.Ptomedibarranomb = dr.GetString(dr.GetOrdinal(helper.Ptomedibarranomb));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Ptomedidesc))) entity.Ptomedidesc = dr.GetString(dr.GetOrdinal(helper.Ptomedidesc));

                    try
                    {
                        try
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Emprcodi))) entity.Emprcodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Emprcodi)));
                        }
                        catch { }

                        try
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Equipadre))) entity.Equipadre = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Equipadre)));
                        }
                        catch { }

                        try
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Areanomb))) entity.Areanomb = dr.GetString(dr.GetOrdinal(helper.Areanomb));
                        }
                        catch { }

                        try
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.AreaOperativa))) entity.AreaOperativa = dr.GetString(dr.GetOrdinal(helper.AreaOperativa));
                        }
                        catch { }

                        try
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Valor1))) entity.Valor1 = Convert.ToDecimal(dr.GetValue(dr.GetOrdinal(helper.Valor1))).ToString();
                        }
                        catch { }

                        try
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Equipopadre))) entity.Equipopadre = dr.GetString(dr.GetOrdinal(helper.Equipopadre));
                        }
                        catch { }

                        try
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Equicodi))) entity.Equicodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Equicodi)));
                        }
                        catch { }

                        try
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Equiabrev))) entity.Equiabrev = dr.GetString(dr.GetOrdinal(helper.Equiabrev));
                        }
                        catch { }

                        try
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Suministrador))) entity.Suministrador = dr.GetString(dr.GetOrdinal(helper.Suministrador));
                        }
                        catch { }

                        try
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.CodigoOsinergmin))) entity.CodigoOsinergmin = dr.GetString(dr.GetOrdinal(helper.CodigoOsinergmin));
                        }
                        catch { }

                        try
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Unidad))) entity.Unidad = dr.GetString(dr.GetOrdinal(helper.Unidad));
                        }
                        catch { }

                        try
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.PtoMediEleNomb))) entity.PtoMediEleNomb = dr.GetString(dr.GetOrdinal(helper.PtoMediEleNomb));
                        }
                        catch { }

                        try
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Clientenomb))) entity.Clientenomb = dr.GetString(dr.GetOrdinal(helper.Clientenomb));
                        }
                        catch { }

                        try
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.PuntoConexion))) entity.PuntoConexion = dr.GetString(dr.GetOrdinal(helper.PuntoConexion));
                        }
                        catch { }
                    }
                    catch (Exception ex)
                    {
                        var m = ex.Message;
                    }
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeHojaptomedDTO> ObtenerPtosXFormato(int formatcodi, int emprcodi)
        {
            string sqlQuery = string.Format(helper.SqlObtenerPtosXFormato, formatcodi, emprcodi);
            List<MeHojaptomedDTO> entitys = new List<MeHojaptomedDTO>();
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

        public List<MeHojaptomedDTO> GetPuntosFormato(int emprcodi, int formatcodi)
        {
            List<MeHojaptomedDTO> entitys = new List<MeHojaptomedDTO>();
            string query = string.Format(helper.SqlGetPuntosFormato, emprcodi, formatcodi);
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

        public List<MeHojaptomedDTO> GetByCriteria2(int emprcodi, int formatcodi, int hojacodi, string query, DateTime fechaPeriodo)
        {
            List<MeHojaptomedDTO> entitys = new List<MeHojaptomedDTO>();
            string queryString = string.Format(query, emprcodi, formatcodi, hojacodi, fechaPeriodo.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            MeHojaptomedDTO entity = new MeHojaptomedDTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = entity = new MeHojaptomedDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iTptomedinomb = dr.GetOrdinal(helper.Tptomedinomb);
                    if (!dr.IsDBNull(iTptomedinomb)) entity.Tptomedinomb = dr.GetString(iTptomedinomb);

                    int iHojaptoliminf = dr.GetOrdinal(helper.Hojaptoliminf);
                    if (!dr.IsDBNull(iHojaptoliminf)) entity.Hojaptoliminf = dr.GetDecimal(iHojaptoliminf);

                    int iHojaptolimsup = dr.GetOrdinal(helper.Hojaptolimsup);
                    if (!dr.IsDBNull(iHojaptolimsup)) entity.Hojaptolimsup = dr.GetDecimal(iHojaptolimsup);

                    try
                    {
                        int iObraFechaPlanificada = dr.GetOrdinal(helper.ObraFechaPlanificada);
                        if (!dr.IsDBNull(iObraFechaPlanificada)) entity.ObraFechaPlanificada = dr.GetDateTime(iObraFechaPlanificada);
                        if (!dr.IsDBNull(dr.GetOrdinal(helper.PtoMediEleNomb))) entity.PtoMediEleNomb = dr.GetString(dr.GetOrdinal(helper.PtoMediEleNomb));
                    }
                    catch (Exception)
                    {

                    }

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        //inicio agregado
        public List<MeHojaptomedDTO> ListPtosWithTipoGeneracion(int formatcodi, int tgenercodi)
        {
            List<MeHojaptomedDTO> entitys = new List<MeHojaptomedDTO>();
            string queryString = string.Format(helper.SqlListPtosWithTipoGeneracion, formatcodi, tgenercodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            MeHojaptomedDTO entity = new MeHojaptomedDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Emprcodi))) entity.Emprcodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Emprcodi)));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Emprnomb))) entity.Emprnomb = dr.GetString(dr.GetOrdinal(helper.Emprnomb));

                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Tgenercodi))) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Tgenercodi)));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Tgenernomb))) entity.Tgenernomb = dr.GetString(dr.GetOrdinal(helper.Tgenernomb));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        //fin agregado

        public List<MeHojaptomedDTO> ListarHojaPtoByFormatoAndEmpresa(int emprcodi, string formatcodi)
        {
            List<MeHojaptomedDTO> entitys = new List<MeHojaptomedDTO>();
            string queryString = string.Format(helper.SqlListarHojaPtoByFormatoAndEmpresa, emprcodi, formatcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            MeHojaptomedDTO entity = new MeHojaptomedDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Tipoinfoabrev))) entity.Tipoinfoabrev = dr.GetString(dr.GetOrdinal(helper.Tipoinfoabrev));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Equinomb))) entity.Equinomb = dr.GetString(dr.GetOrdinal(helper.Equinomb));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Emprabrev))) entity.Emprabrev = dr.GetString(dr.GetOrdinal(helper.Emprabrev));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Famcodi))) entity.Famcodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Famcodi)));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Tipoptomedinomb))) entity.Tipoptomedinomb = dr.GetString(dr.GetOrdinal(helper.Tipoptomedinomb));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Ptomedibarranomb))) entity.Ptomedibarranomb = dr.GetString(dr.GetOrdinal(helper.Ptomedibarranomb));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Ptomedidesc))) entity.Ptomedidesc = dr.GetString(dr.GetOrdinal(helper.Ptomedidesc));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.PtoMediEleNomb))) entity.PtoMediEleNomb = dr.GetString(dr.GetOrdinal(helper.PtoMediEleNomb));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Equicodi))) entity.Equicodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Equicodi)));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Gruponomb))) entity.Gruponomb = dr.GetString(dr.GetOrdinal(helper.Gruponomb));

                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Clientenomb))) entity.Clientenomb = dr.GetString(dr.GetOrdinal(helper.Clientenomb));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Barranomb))) entity.Barranomb = dr.GetString(dr.GetOrdinal(helper.Barranomb));

                    int iOriglectnombre = dr.GetOrdinal(helper.Origlectnombre);
                    if (!dr.IsDBNull(iOriglectnombre)) entity.Origlectnombre = dr.GetString(iOriglectnombre);
                    int iOriglectcodi = dr.GetOrdinal(helper.Origlectcodi);
                    if (!dr.IsDBNull(iOriglectcodi)) entity.Origlectcodi = Convert.ToInt32(dr.GetValue(iOriglectcodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));
                    int iEquipopadre = dr.GetOrdinal(helper.Equipopadre);
                    if (!dr.IsDBNull(iEquipopadre)) entity.Equipopadre = dr.GetString(iEquipopadre);

                    int iFormatnombre = dr.GetOrdinal(helper.Formatnombre);
                    if (!dr.IsDBNull(iFormatnombre)) entity.Formatnombre = dr.GetString(iFormatnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeHojaptomedDTO> ListarHojaPtoByFormatoAndEmpresaHoja(int emprcodi, int formatcodi, int hojacodi)
        {
            List<MeHojaptomedDTO> entitys = new List<MeHojaptomedDTO>();
            string queryString = string.Empty;
            if(formatcodi==54)
                queryString = string.Format(helper.SqlListarHojaPtoByFormatoAndEmpresaHojaPr16, emprcodi, formatcodi, hojacodi);
            else 
                queryString = string.Format(helper.SqlListarHojaPtoByFormatoAndEmpresaHoja, emprcodi, formatcodi, hojacodi);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            MeHojaptomedDTO entity = new MeHojaptomedDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Tipoinfoabrev))) entity.Tipoinfoabrev = dr.GetString(dr.GetOrdinal(helper.Tipoinfoabrev));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Equinomb))) entity.Equinomb = dr.GetString(dr.GetOrdinal(helper.Equinomb));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Emprabrev))) entity.Emprabrev = dr.GetString(dr.GetOrdinal(helper.Emprabrev));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Famcodi))) entity.Famcodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Famcodi)));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Tipoptomedinomb))) entity.Tipoptomedinomb = dr.GetString(dr.GetOrdinal(helper.Tipoptomedinomb));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Ptomedibarranomb))) entity.Ptomedibarranomb = dr.GetString(dr.GetOrdinal(helper.Ptomedibarranomb));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Ptomedidesc))) entity.Ptomedidesc = dr.GetString(dr.GetOrdinal(helper.Ptomedidesc));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Equicodi))) entity.Equicodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Equicodi)));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Gruponomb))) entity.Gruponomb = dr.GetString(dr.GetOrdinal(helper.Gruponomb));

                    int iOriglectnombre = dr.GetOrdinal(helper.Origlectnombre);
                    if (!dr.IsDBNull(iOriglectnombre)) entity.Origlectnombre = dr.GetString(iOriglectnombre);
                    int iOriglectcodi = dr.GetOrdinal(helper.Origlectcodi);
                    if (!dr.IsDBNull(iOriglectcodi)) entity.Origlectcodi = Convert.ToInt32(dr.GetValue(iOriglectcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeHojaptomedDTO> GetByCriteria3(int emprcodi, int formatcodi, int cuenca, string query)
        {
            List<MeHojaptomedDTO> entitys = new List<MeHojaptomedDTO>();
            string queryString = string.Format(query, emprcodi, formatcodi, cuenca);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            MeHojaptomedDTO entity = new MeHojaptomedDTO();
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
                    entity = helper.Create(dr);
                    try
                    {
                        find = listaCampo.Find(x => x == helper.Tipoinfoabrev.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Tipoinfoabrev))) entity.Tipoinfoabrev = dr.GetString(dr.GetOrdinal(helper.Tipoinfoabrev));
                        }
                        find = listaCampo.Find(x => x == helper.Equinomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Equinomb))) entity.Equinomb = dr.GetString(dr.GetOrdinal(helper.Equinomb));
                        }
                        find = listaCampo.Find(x => x == helper.Emprabrev.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Emprabrev))) entity.Emprabrev = dr.GetString(dr.GetOrdinal(helper.Emprabrev));
                        }
                        find = listaCampo.Find(x => x == helper.Famcodi.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Famcodi))) entity.Famcodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Famcodi)));
                        }
                        find = listaCampo.Find(x => x == helper.Tipoptomedinomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Tipoptomedinomb))) entity.Tipoptomedinomb = dr.GetString(dr.GetOrdinal(helper.Tipoptomedinomb));
                        }
                        find = listaCampo.Find(x => x == helper.Ptomedibarranomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Ptomedibarranomb))) entity.Ptomedibarranomb = dr.GetString(dr.GetOrdinal(helper.Ptomedibarranomb));
                        }
                        find = listaCampo.Find(x => x == helper.Ptomedidesc.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Ptomedidesc))) entity.Ptomedidesc = dr.GetString(dr.GetOrdinal(helper.Ptomedidesc));
                        }

                        find = listaCampo.Find(x => x == helper.Equipopadre.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Equipopadre))) entity.Equipopadre = dr.GetString(dr.GetOrdinal(helper.Equipopadre));
                        }
                        find = listaCampo.Find(x => x == helper.Areanomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Areanomb))) entity.Areanomb = dr.GetString(dr.GetOrdinal(helper.Areanomb));
                        }

                        find = listaCampo.Find(x => x == helper.AreaOperativa.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.AreaOperativa))) entity.AreaOperativa = dr.GetString(dr.GetOrdinal(helper.AreaOperativa));
                        }
                        find = listaCampo.Find(x => x == helper.Valor1.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Valor1))) entity.Valor1 = Convert.ToDecimal(dr.GetValue(dr.GetOrdinal(helper.Valor1))).ToString();
                        }

                        find = listaCampo.Find(x => x == helper.Equicodi.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Equicodi))) entity.Equicodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Equicodi)));
                        }

                        find = listaCampo.Find(x => x == helper.Equipadre.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Equipadre))) entity.Equipadre = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Equipadre)));
                        }

                        find = listaCampo.Find(x => x == helper.Equiabrev.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Equiabrev))) entity.Equiabrev = dr.GetString(dr.GetOrdinal(helper.Equiabrev));
                        }

                        find = listaCampo.Find(x => x == helper.Suministrador.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Suministrador))) entity.Suministrador = dr.GetString(dr.GetOrdinal(helper.Suministrador));
                        }

                        find = listaCampo.Find(x => x == helper.CodigoOsinergmin.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.CodigoOsinergmin))) entity.CodigoOsinergmin = dr.GetString(dr.GetOrdinal(helper.CodigoOsinergmin));
                        }

                        find = listaCampo.Find(x => x == helper.Unidad.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Unidad))) entity.Unidad = dr.GetString(dr.GetOrdinal(helper.Unidad));
                        }

                        find = listaCampo.Find(x => x == helper.PtoMediEleNomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.PtoMediEleNomb))) entity.PtoMediEleNomb = dr.GetString(dr.GetOrdinal(helper.PtoMediEleNomb));
                        }

                        find = listaCampo.Find(x => x == helper.Famabrev.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Famabrev))) entity.Famabrev = dr.GetString(dr.GetOrdinal(helper.Famabrev));
                        }

                        find = listaCampo.Find(x => x == helper.Areacodi.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Areacodi))) entity.Areacodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Areacodi)));
                        }

                        find = listaCampo.Find(x => x == helper.Gruponomb.ToUpper());
                        if (find != null)
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal(helper.Gruponomb))) entity.Gruponomb = dr.GetString(dr.GetOrdinal(helper.Gruponomb));
                        }

                        entitys.Add(entity);
                    }
                    catch
                    {

                    }
                }
            }
            return entitys;
        }

        public List<MeHojaptomedDTO> ListByFormatcodi(string formatcodi)
        {
            List<MeHojaptomedDTO> entitys = new List<MeHojaptomedDTO>();

            string sql = string.Format(helper.SqlListByFormatcodi, formatcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Ptomedibarranomb))) entity.Ptomedibarranomb = dr.GetString(dr.GetOrdinal(helper.Ptomedibarranomb));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Ptomedidesc))) entity.Ptomedidesc = dr.GetString(dr.GetOrdinal(helper.Ptomedidesc));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.PtoMediEleNomb))) entity.PtoMediEleNomb = dr.GetString(dr.GetOrdinal(helper.PtoMediEleNomb));
                    if (!dr.IsDBNull(dr.GetOrdinal(helper.Ptomediestado))) entity.Ptomediestado = dr.GetString(dr.GetOrdinal(helper.Ptomediestado));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
