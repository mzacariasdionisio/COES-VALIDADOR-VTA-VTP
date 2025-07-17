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
    /// Clase de acceso a datos de la tabla EVE_IEODCUADRO
    /// </summary>
    public class EveIeodcuadroRepository: RepositoryBase, IEveIeodcuadroRepository
    {
        public EveIeodcuadroRepository(string strConn): base(strConn)
        {
        }

        EveIeodcuadroHelper helper = new EveIeodcuadroHelper();

        public int Save(EveIeodcuadroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Iccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Ichorini, DbType.DateTime, entity.Ichorini);
            dbProvider.AddInParameter(command, helper.Ichorfin, DbType.DateTime, entity.Ichorfin);
            dbProvider.AddInParameter(command, helper.Icdescrip1, DbType.String, entity.Icdescrip1);
            dbProvider.AddInParameter(command, helper.Icdescrip2, DbType.String, entity.Icdescrip2);
            dbProvider.AddInParameter(command, helper.Icdescrip3, DbType.String, entity.Icdescrip3);
            dbProvider.AddInParameter(command, helper.Iccheck1, DbType.String, entity.Iccheck1);
            dbProvider.AddInParameter(command, helper.Icvalor1, DbType.Decimal, entity.Icvalor1);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Numtrsgsubit, DbType.Decimal, entity.Numtrsgsubit);
            dbProvider.AddInParameter(command, helper.Numtrsgsostn, DbType.Decimal, entity.Numtrsgsostn);
            dbProvider.AddInParameter(command, helper.Iccheck2, DbType.String, entity.Iccheck2);
            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, entity.Evenclasecodi);
            dbProvider.AddInParameter(command, helper.Ichor3, DbType.DateTime, entity.Ichor3);
            dbProvider.AddInParameter(command, helper.Ichor4, DbType.DateTime, entity.Ichor4);
            dbProvider.AddInParameter(command, helper.Iccheck3, DbType.String, entity.Iccheck3);
            dbProvider.AddInParameter(command, helper.Iccheck4, DbType.String, entity.Iccheck4);
            dbProvider.AddInParameter(command, helper.Icvalor2, DbType.Decimal, entity.Icvalor2);
            dbProvider.AddInParameter(command, helper.Icnombarchenvio, DbType.String, entity.Icnombarchenvio);
            dbProvider.AddInParameter(command, helper.Icnombarchfisico, DbType.String, entity.Icnombarchfisico);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ichorinicarga, DbType.DateTime, entity.Ichorinicarga);
            dbProvider.AddInParameter(command, helper.Ictipcuadro, DbType.Int32, entity.Ictipcuadro);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveIeodcuadroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Ichorini, DbType.DateTime, entity.Ichorini);
            dbProvider.AddInParameter(command, helper.Ichorfin, DbType.DateTime, entity.Ichorfin);
            dbProvider.AddInParameter(command, helper.Icdescrip1, DbType.String, entity.Icdescrip1);
            dbProvider.AddInParameter(command, helper.Icdescrip2, DbType.String, entity.Icdescrip2);
            dbProvider.AddInParameter(command, helper.Icdescrip3, DbType.String, entity.Icdescrip3);
            dbProvider.AddInParameter(command, helper.Iccheck1, DbType.String, entity.Iccheck1);
            dbProvider.AddInParameter(command, helper.Icvalor1, DbType.Decimal, entity.Icvalor1);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Numtrsgsubit, DbType.Decimal, entity.Numtrsgsubit);
            dbProvider.AddInParameter(command, helper.Numtrsgsostn, DbType.Decimal, entity.Numtrsgsostn);
            dbProvider.AddInParameter(command, helper.Iccheck2, DbType.String, entity.Iccheck2);
            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, entity.Evenclasecodi);
            dbProvider.AddInParameter(command, helper.Ichor3, DbType.DateTime, entity.Ichor3);
            dbProvider.AddInParameter(command, helper.Ichor4, DbType.DateTime, entity.Ichor4);
            dbProvider.AddInParameter(command, helper.Iccheck3, DbType.String, entity.Iccheck3);
            dbProvider.AddInParameter(command, helper.Iccheck4, DbType.String, entity.Iccheck4);
            dbProvider.AddInParameter(command, helper.Icvalor2, DbType.Decimal, entity.Icvalor2);
            dbProvider.AddInParameter(command, helper.Icnombarchenvio, DbType.String, entity.Icnombarchenvio);
            dbProvider.AddInParameter(command, helper.Icnombarchfisico, DbType.String, entity.Icnombarchfisico);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ichorinicarga, DbType.DateTime, entity.Ichorinicarga);
            dbProvider.AddInParameter(command, helper.Ictipcuadro, DbType.Int32, entity.Ictipcuadro);

            dbProvider.AddInParameter(command, helper.Iccodi, DbType.Int32, entity.Iccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int iccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Iccodi, DbType.Int32, iccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveIeodcuadroDTO GetById(int iccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Iccodi, DbType.Int32, iccodi);
            EveIeodcuadroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveIeodcuadroDTO> List()
        {
            List<EveIeodcuadroDTO> entitys = new List<EveIeodcuadroDTO>();
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

        public List<EveIeodcuadroDTO> GetByCriteria()
        {
            List<EveIeodcuadroDTO> entitys = new List<EveIeodcuadroDTO>();
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


        public List<EveIeodcuadroDTO> BuscarOperaciones(int evenClase, int subCausacodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            List<EveIeodcuadroDTO> entitys = new List<EveIeodcuadroDTO>();
            String sql = String.Format(this.helper.ObtenerListado, evenClase, subCausacodi, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFinal.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveIeodcuadroDTO entity = new EveIeodcuadroDTO();

                    int iIccodi = dr.GetOrdinal(this.helper.Iccodi);
                    if (!dr.IsDBNull(iIccodi)) entity.Iccodi = dr.GetInt32(iIccodi);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iIchorini = dr.GetOrdinal(this.helper.Ichorini);
                    if (!dr.IsDBNull(iIchorini)) entity.Ichorini = dr.GetDateTime(iIchorini);

                    int iIchorfin = dr.GetOrdinal(this.helper.Ichorfin);
                    if (!dr.IsDBNull(iIchorfin)) entity.Ichorfin = dr.GetDateTime(iIchorfin);

                    int iSubcausadesc = dr.GetOrdinal(this.helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    int iLastuser = dr.GetOrdinal(this.helper.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

                    int iLastdate = dr.GetOrdinal(this.helper.Lastdate);
                    if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

                    int iSubcausacodi = dr.GetOrdinal(this.helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = dr.GetInt32(iSubcausacodi);

                    int iIcvalor1 = dr.GetOrdinal(this.helper.Icvalor1);
                    if (!dr.IsDBNull(iIcvalor1)) entity.Icvalor1 = dr.GetDecimal(iIcvalor1);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveIeodcuadroDTO> BuscarOperacionesSinPaginado(int evenClase, int subCausacodi, DateTime fechaInicio, DateTime fechaFinal, string idsEmpresa, string idsTipoEquipo)
        {
            List<EveIeodcuadroDTO> entitys = new List<EveIeodcuadroDTO>();
            String sql = String.Format(this.helper.ObtenerListadoSinPaginado, evenClase, subCausacodi, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFinal.ToString(ConstantesBase.FormatoFecha), idsEmpresa, idsTipoEquipo);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveIeodcuadroDTO entity = new EveIeodcuadroDTO();
                    
                    int iIccodi = dr.GetOrdinal(this.helper.Iccodi);
                    if (!dr.IsDBNull(iIccodi)) entity.Iccodi = dr.GetInt32(iIccodi);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreacodi = dr.GetOrdinal(this.helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);
                    
                    int iAreadesc = dr.GetOrdinal(this.helper.Areadesc);
                    if (!dr.IsDBNull(iAreadesc)) entity.Areadesc = dr.GetString(iAreadesc);

                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = dr.GetInt32(iFamcodi);

                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iIctipcuadro = dr.GetOrdinal(this.helper.Ictipcuadro);
                    if (!dr.IsDBNull(iIctipcuadro)) entity.Ictipcuadro = dr.GetInt32(iIctipcuadro);

                    int iIchorinicarga = dr.GetOrdinal(this.helper.Ichorinicarga);
                    if (!dr.IsDBNull(iIchorinicarga)) entity.Ichorinicarga = dr.GetDateTime(iIchorinicarga);

                    int iIchorini = dr.GetOrdinal(this.helper.Ichorini);
                    if (!dr.IsDBNull(iIchorini)) entity.Ichorini = dr.GetDateTime(iIchorini);

                    int iIchorfin = dr.GetOrdinal(this.helper.Ichorfin);
                    if (!dr.IsDBNull(iIchorfin)) entity.Ichorfin = dr.GetDateTime(iIchorfin);

                    int iSubcausadesc = dr.GetOrdinal(this.helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    int iLastuser = dr.GetOrdinal(this.helper.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

                    int iLastdate = dr.GetOrdinal(this.helper.Lastdate);
                    if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

                    int iDescrip1 = dr.GetOrdinal(this.helper.Icdescrip1);
                    if (!dr.IsDBNull(iDescrip1)) entity.Icdescrip1 = dr.GetString(iDescrip1);

                    int iDescrip2 = dr.GetOrdinal(this.helper.Icdescrip2);
                    if (!dr.IsDBNull(iDescrip2)) entity.Icdescrip2 = dr.GetString(iDescrip2);

                    int iDescrip3 = dr.GetOrdinal(this.helper.Icdescrip3);
                    if (!dr.IsDBNull(iDescrip3)) entity.Icdescrip3 = dr.GetString(iDescrip3);

                    int iCheck1 = dr.GetOrdinal(this.helper.Iccheck1);
                    if (!dr.IsDBNull(iCheck1)) entity.Iccheck1 = dr.GetString(iCheck1);

                    int iValor1 = dr.GetOrdinal(this.helper.Icvalor1);
                    if (!dr.IsDBNull(iValor1)) entity.Icvalor1 = dr.GetDecimal(iValor1);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iSubcausacodi = dr.GetOrdinal(this.helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));
                    
                    int iCheck2 = dr.GetOrdinal(this.helper.Iccheck2);
                    if (!dr.IsDBNull(iCheck2)) entity.Iccheck2 = dr.GetString(iCheck2);
                    
                    int iEvenclasecodi = dr.GetOrdinal(this.helper.Evenclasecodi);
                    if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = Convert.ToInt32(dr.GetValue(iEvenclasecodi));
                    
                    int iIchor3 = dr.GetOrdinal(this.helper.Ichor3);
                    if (!dr.IsDBNull(iIchor3)) entity.Ichor3 = dr.GetDateTime(iIchor3);

                    int iIchor4 = dr.GetOrdinal(this.helper.Ichor4);
                    if (!dr.IsDBNull(iIchor4)) entity.Ichor4 = dr.GetDateTime(iIchor4);

                    int iCheck3 = dr.GetOrdinal(this.helper.Iccheck3);
                    if (!dr.IsDBNull(iCheck3)) entity.Iccheck3 = dr.GetString(iCheck3);

                    int iCheck4 = dr.GetOrdinal(this.helper.Iccheck4);
                    if (!dr.IsDBNull(iCheck4)) entity.Iccheck4 = dr.GetString(iCheck4);

                    int iValor2 = dr.GetOrdinal(this.helper.Icvalor2);
                    if (!dr.IsDBNull(iValor2)) entity.Icvalor2 = dr.GetDecimal(iValor2);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }




        public List<EveIeodcuadroDTO> BuscarOperacionesDetallado(int evenClase, int subCausacodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            List<EveIeodcuadroDTO> entitys = new List<EveIeodcuadroDTO>();
            String sql = String.Format(this.helper.ObtenerListadoDetallado, evenClase, subCausacodi, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFinal.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveIeodcuadroDTO entity = new EveIeodcuadroDTO();

                    int iIccodi = dr.GetOrdinal(this.helper.Iccodi);
                    if (!dr.IsDBNull(iIccodi)) entity.Iccodi = dr.GetInt32(iIccodi);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iIchorini = dr.GetOrdinal(this.helper.Ichorini);
                    if (!dr.IsDBNull(iIchorini)) entity.Ichorini = dr.GetDateTime(iIchorini);

                    int iIchorfin = dr.GetOrdinal(this.helper.Ichorfin);
                    if (!dr.IsDBNull(iIchorfin)) entity.Ichorfin = dr.GetDateTime(iIchorfin);

                    int iSubcausadesc = dr.GetOrdinal(this.helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    int iLastuser = dr.GetOrdinal(this.helper.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

                    int iLastdate = dr.GetOrdinal(this.helper.Lastdate);
                    if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

                    int iSubcausacodi = dr.GetOrdinal(this.helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = dr.GetInt32(iSubcausacodi);

                    int iIcvalor1 = dr.GetOrdinal(this.helper.Icvalor1);
                    if (!dr.IsDBNull(iIcvalor1)) entity.Icvalor1 = dr.GetDecimal(iIcvalor1);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iIdescrip1 = dr.GetOrdinal(this.helper.Icdescrip1);
                    if (!dr.IsDBNull(iIdescrip1)) entity.Icdescrip1 = dr.GetString(iIdescrip1);

                    int iIdescrip2 = dr.GetOrdinal(this.helper.Icdescrip2);
                    if (!dr.IsDBNull(iIdescrip2)) entity.Icdescrip2 = dr.GetString(iIdescrip2);

                    int iIdescrip3 = dr.GetOrdinal(this.helper.Icdescrip3);
                    if (!dr.IsDBNull(iIdescrip3)) entity.Icdescrip3 = dr.GetString(iIdescrip3);

                    int iIccheck1 = dr.GetOrdinal(this.helper.Iccheck1);
                    if (!dr.IsDBNull(iIccheck1)) entity.Iccheck1 = dr.GetString(iIccheck1);

                    int iNumtrsgsubit = dr.GetOrdinal(this.helper.Numtrsgsubit);
                    if (!dr.IsDBNull(iNumtrsgsubit)) entity.Numtrsgsubit = dr.GetDecimal(iNumtrsgsubit);

                    int iNumtrsgsostn = dr.GetOrdinal(this.helper.Numtrsgsostn);
                    if (!dr.IsDBNull(iNumtrsgsostn)) entity.Numtrsgsostn = dr.GetDecimal(iNumtrsgsostn);

                    int iIccheck2 = dr.GetOrdinal(this.helper.Iccheck2);
                    if (!dr.IsDBNull(iIccheck2)) entity.Iccheck2 = dr.GetString(iIccheck2);

                    int iEvenclasecodi = dr.GetOrdinal(this.helper.Evenclasecodi);
                    if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = dr.GetInt32(iEvenclasecodi);

                    int iIchor3 = dr.GetOrdinal(this.helper.Ichor3);
                    if (!dr.IsDBNull(iIchor3)) entity.Ichor3 = dr.GetDateTime(iIchor3);

                    int iIchor4 = dr.GetOrdinal(this.helper.Ichor4);
                    if (!dr.IsDBNull(iIchor4)) entity.Ichor4 = dr.GetDateTime(iIchor4);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iIccheck3 = dr.GetOrdinal(this.helper.Iccheck3);
                    if (!dr.IsDBNull(iIccheck3)) entity.Iccheck3 = dr.GetString(iIccheck3);

                    int iIccheck4 = dr.GetOrdinal(this.helper.Iccheck4);
                    if (!dr.IsDBNull(iIccheck4)) entity.Iccheck4 = dr.GetString(iIccheck4);

                    int iIcvalor2 = dr.GetOrdinal(this.helper.Icvalor2);
                    if (!dr.IsDBNull(iIcvalor2)) entity.Icvalor2 = dr.GetDecimal(iIcvalor2);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(int evenClase, int subCausacodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            List<EveIeodcuadroDTO> entitys = new List<EveIeodcuadroDTO>();
            String sql = String.Format(this.helper.TotalRegistros, evenClase, subCausacodi, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFinal.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);
            
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public EveIeodcuadroDTO ObtenerIeodcuadro(int iccodi)
        {
            EveIeodcuadroDTO entity = null;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.ObtenerIeodCuadro);
            dbProvider.AddInParameter(command, helper.Iccodi, DbType.Int32, iccodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new EveIeodcuadroDTO();

                    int iIccodi = dr.GetOrdinal(this.helper.Iccodi);
                    if (!dr.IsDBNull(iIccodi)) entity.Iccodi = dr.GetInt32(iIccodi);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iSubcausacodi = dr.GetOrdinal(this.helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = dr.GetInt32(iSubcausacodi);

                    int iIctipcudro = dr.GetOrdinal(this.helper.Ictipcuadro);
                    if (!dr.IsDBNull(iIctipcudro)) entity.Ictipcuadro = dr.GetInt32(iIctipcudro);

                    int iIchorinicarga = dr.GetOrdinal(this.helper.Ichorinicarga);
                    if (!dr.IsDBNull(iIchorinicarga)) entity.Ichorinicarga = dr.GetDateTime(iIchorinicarga);

                    int iIchorini = dr.GetOrdinal(this.helper.Ichorini);
                    if (!dr.IsDBNull(iIchorini)) entity.Ichorini = dr.GetDateTime(iIchorini);

                    int iIchorfin = dr.GetOrdinal(this.helper.Ichorfin);
                    if (!dr.IsDBNull(iIchorfin)) entity.Ichorfin = dr.GetDateTime(iIchorfin);

                    int iIdescrip1 = dr.GetOrdinal(this.helper.Icdescrip1);
                    if (!dr.IsDBNull(iIdescrip1)) entity.Icdescrip1 = dr.GetString(iIdescrip1);

                    int iIdescrip2 = dr.GetOrdinal(this.helper.Icdescrip2);
                    if (!dr.IsDBNull(iIdescrip2)) entity.Icdescrip2 = dr.GetString(iIdescrip2);

                    int iIdescrip3 = dr.GetOrdinal(this.helper.Icdescrip3);
                    if (!dr.IsDBNull(iIdescrip3)) entity.Icdescrip3 = dr.GetString(iIdescrip3);

                    int iIccheck1 = dr.GetOrdinal(this.helper.Iccheck1);
                    if (!dr.IsDBNull(iIccheck1)) entity.Iccheck1 = dr.GetString(iIccheck1);

                    int iIcvalor1 = dr.GetOrdinal(this.helper.Icvalor1);
                    if (!dr.IsDBNull(iIcvalor1)) entity.Icvalor1 = dr.GetDecimal(iIcvalor1);

                    int iLastuser = dr.GetOrdinal(this.helper.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

                    int iLastdate = dr.GetOrdinal(this.helper.Lastdate);
                    if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

                    int iNumtrsgsubit = dr.GetOrdinal(this.helper.Numtrsgsubit);
                    if (!dr.IsDBNull(iNumtrsgsubit)) entity.Numtrsgsubit = dr.GetDecimal(iNumtrsgsubit);

                    int iNumtrsgsostn = dr.GetOrdinal(this.helper.Numtrsgsostn);
                    if (!dr.IsDBNull(iNumtrsgsostn)) entity.Numtrsgsostn = dr.GetDecimal(iNumtrsgsostn);

                    int iIccheck2 = dr.GetOrdinal(this.helper.Iccheck2);
                    if (!dr.IsDBNull(iIccheck2)) entity.Iccheck2 = dr.GetString(iIccheck2);

                    int iEvenclasecodi = dr.GetOrdinal(this.helper.Evenclasecodi);
                    if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = dr.GetInt32(iEvenclasecodi);

                    int iIchor3 = dr.GetOrdinal(this.helper.Ichor3);
                    if (!dr.IsDBNull(iIchor3)) entity.Ichor3 = dr.GetDateTime(iIchor3);

                    int iIchor4 = dr.GetOrdinal(this.helper.Ichor4);
                    if (!dr.IsDBNull(iIchor4)) entity.Ichor4 = dr.GetDateTime(iIchor4);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iIccheck3 = dr.GetOrdinal(this.helper.Iccheck3);
                    if (!dr.IsDBNull(iIccheck3)) entity.Iccheck3 = dr.GetString(iIccheck3);

                    int iIccheck4 = dr.GetOrdinal(this.helper.Iccheck4);
                    if (!dr.IsDBNull(iIccheck4)) entity.Iccheck4 = dr.GetString(iIccheck4);

                    int iIcvalor2 = dr.GetOrdinal(this.helper.Icvalor2);
                    if (!dr.IsDBNull(iIcvalor2)) entity.Icvalor2 = dr.GetDecimal(iIcvalor2);
                }
            }

            return entity;
        }

        public EveIeodcuadroDTO ObtenerDatosEquipo(int idEquipo)
        {
            EveIeodcuadroDTO entity = null;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.ObtenerDatosEquipo);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, idEquipo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new EveIeodcuadroDTO();
                    
                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);
                    
                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);
                    
                    int iDemHP = dr.GetOrdinal(this.helper.DemHP);
                    if (!dr.IsDBNull(iDemHP)) entity.DemHP = dr.GetString(iDemHP);

                    int iDemFP = dr.GetOrdinal(this.helper.DemFP);
                    if (!dr.IsDBNull(iDemFP)) entity.DemFP = dr.GetString(iDemFP);

                }
            }

            return entity;
        }


        public List<EveIeodcuadroDTO> ListarEveIeodCuadroxEmpresa(DateTime fechaIni, DateTime fechaFin, int subcausaCodi, int emprcodi)
        {
            List<EveIeodcuadroDTO> entitys = new List<EveIeodcuadroDTO>();

            string query = String.Format(helper.SqlListarEveIeodCuadroxEmpresa,
                fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), subcausaCodi, emprcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveIeodcuadroDTO entity = new EveIeodcuadroDTO();
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprCodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = Convert.ToInt16(dr.GetValue(iEmprCodi));

                    int iTareaAbrev = dr.GetOrdinal(this.helper.Tareaabrev);
                    if (!dr.IsDBNull(iTareaAbrev)) entity.Tareaabrev = dr.GetString(iTareaAbrev);

                    int iIcnombarchenvio = dr.GetOrdinal(this.helper.Icnombarchenvio);
                    if (!dr.IsDBNull(iIcnombarchenvio)) entity.Icnombarchenvio = dr.GetString(iIcnombarchenvio);

                    int iIcnombarchfisico = dr.GetOrdinal(this.helper.Icnombarchfisico);
                    if (!dr.IsDBNull(iIcnombarchfisico)) entity.Icnombarchfisico = dr.GetString(iIcnombarchfisico);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveIeodcuadroDTO> GetCriteriaxPKCodis(string pkCodis)
        {
            List<EveIeodcuadroDTO> entitys = new List<EveIeodcuadroDTO>();

            string query = string.Format(helper.SqlGetCriteriaxPKCodis, pkCodis);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveIeodcuadroDTO entity = new EveIeodcuadroDTO();
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprCodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = Convert.ToInt16(dr.GetValue(iEmprCodi));

                    int iTareaAbrev = dr.GetOrdinal(this.helper.Tareaabrev);
                    if (!dr.IsDBNull(iTareaAbrev)) entity.Tareaabrev = dr.GetString(iTareaAbrev);

                    int iIcnombarchenvio = dr.GetOrdinal(this.helper.Icnombarchenvio);
                    if (!dr.IsDBNull(iIcnombarchenvio)) entity.Icnombarchenvio = dr.GetString(iIcnombarchenvio);

                    int iIcnombarchfisico = dr.GetOrdinal(this.helper.Icnombarchfisico);
                    if (!dr.IsDBNull(iIcnombarchfisico)) entity.Icnombarchfisico = dr.GetString(iIcnombarchfisico);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void BorradoLogico(int iccodi)
        {
            string stQuery = string.Format(this.helper.SqlBorradoLogico, iccodi);
            DbCommand command = dbProvider.GetSqlStringCommand(stQuery);
            dbProvider.ExecuteNonQuery(command);
        }

        #region PR5
        public List<EveIeodcuadroDTO> ListarEveIeodCuadroxEmpresaxEquipos(DateTime fechaIni, DateTime fechaFin, int subcausaCodi, string emprcodi, string equipos, int nPagina, int nRegistros)
        {
            List<EveIeodcuadroDTO> entitys = new List<EveIeodcuadroDTO>();

            string query = String.Format(helper.SqlListarEveIeodCuadroxEmpresaxEquipos,
                fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), subcausaCodi, emprcodi, equipos, nPagina, nRegistros);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveIeodcuadroDTO entity = new EveIeodcuadroDTO();
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreacodi = dr.GetOrdinal(this.helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprCodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = Convert.ToInt16(dr.GetValue(iEmprCodi));

                    int iTareaAbrev = dr.GetOrdinal(this.helper.Tareaabrev);
                    if (!dr.IsDBNull(iTareaAbrev)) entity.Tareaabrev = dr.GetString(iTareaAbrev);

                    int iIcnombarchenvio = dr.GetOrdinal(this.helper.Icnombarchenvio);
                    if (!dr.IsDBNull(iIcnombarchenvio)) entity.Icnombarchenvio = dr.GetString(iIcnombarchenvio);

                    int iIcnombarchfisico = dr.GetOrdinal(this.helper.Icnombarchfisico);
                    if (!dr.IsDBNull(iIcnombarchfisico)) entity.Icnombarchfisico = dr.GetString(iIcnombarchfisico);

                    int iDescrip1 = dr.GetOrdinal(this.helper.Icdescrip1);
                    if (!dr.IsDBNull(iDescrip1)) entity.Icdescrip1 = dr.GetString(iDescrip1);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ContarEveIeodCuadroxEmpresaxEquipos(DateTime fechaIni, DateTime fechaFin, int subcausaCodi, string emprcodi, string equipos, string areacodi)
        {
            String query = String.Format(this.helper.SqlContarEveIeodCuadroxEmpresaxEquipos, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), subcausaCodi, emprcodi, equipos, areacodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public int ObtenerNroElementosConsultaRestricciones(DateTime fechaIni, DateTime fechaFin, int subcausaCodi, string emprcodi, string equipos)
        {
            String query = String.Format(this.helper.SqlObtenerNroElementosConsultaRestricciones, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), subcausaCodi, emprcodi, equipos);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public List<EveIeodcuadroDTO> ListarReporteOperacionVaria(DateTime fechaInicio, DateTime fechaFin, string clase, string subcausa)
        {
            List<EveIeodcuadroDTO> entitys = new List<EveIeodcuadroDTO>();
            String sql = String.Format(this.helper.SqlListarReporteOperacionVaria, clase, subcausa, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveIeodcuadroDTO entity = new EveIeodcuadroDTO();

                    int iIccodi = dr.GetOrdinal(this.helper.Iccodi);
                    if (!dr.IsDBNull(iIccodi)) entity.Iccodi = Convert.ToInt32(dr.GetValue(iIccodi));

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprabrev = dr.GetOrdinal(this.helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iAreaabrev = dr.GetOrdinal(this.helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iIchorini = dr.GetOrdinal(this.helper.Ichorini);
                    if (!dr.IsDBNull(iIchorini)) entity.Ichorini = dr.GetDateTime(iIchorini);

                    int iIchorfin = dr.GetOrdinal(this.helper.Ichorfin);
                    if (!dr.IsDBNull(iIchorfin)) entity.Ichorfin = dr.GetDateTime(iIchorfin);

                    int iSubcausadesc = dr.GetOrdinal(this.helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    int iLastuser = dr.GetOrdinal(this.helper.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

                    int iLastdate = dr.GetOrdinal(this.helper.Lastdate);
                    if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

                    int iSubcausacodi = dr.GetOrdinal(this.helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

                    int iIcvalor1 = dr.GetOrdinal(this.helper.Icvalor1);
                    if (!dr.IsDBNull(iIcvalor1)) entity.Icvalor1 = dr.GetDecimal(iIcvalor1);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iDescrip1 = dr.GetOrdinal(this.helper.Icdescrip1);
                    if (!dr.IsDBNull(iDescrip1)) entity.Icdescrip1 = dr.GetString(iDescrip1);

                    int iDescrip2 = dr.GetOrdinal(this.helper.Icdescrip2);
                    if (!dr.IsDBNull(iDescrip2)) entity.Icdescrip2 = dr.GetString(iDescrip2);

                    int iDescrip3 = dr.GetOrdinal(this.helper.Icdescrip3);
                    if (!dr.IsDBNull(iDescrip3)) entity.Icdescrip3 = dr.GetString(iDescrip3);

                    int iNumtrsgsubit = dr.GetOrdinal(helper.Numtrsgsubit);
                    if (!dr.IsDBNull(iNumtrsgsubit)) entity.Numtrsgsubit = dr.GetDecimal(iNumtrsgsubit);

                    int iNumtrsgsostn = dr.GetOrdinal(helper.Numtrsgsostn);
                    if (!dr.IsDBNull(iNumtrsgsostn)) entity.Numtrsgsostn = dr.GetDecimal(iNumtrsgsostn);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region MigracionSGOCOES-GrupoB
        public List<EveIeodcuadroDTO> ListaBitacora(DateTime fecIni, string subcausacodi, string famcodi)
        {
            List<EveIeodcuadroDTO> entitys = new List<EveIeodcuadroDTO>();
            string query = string.Format(helper.SqlListaBitacora, fecIni.ToString(ConstantesBase.FormatoFecha), subcausacodi, famcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveIeodcuadroDTO entity = new EveIeodcuadroDTO();

                    int iEmprabrev = dr.GetOrdinal(this.helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iIchorini = dr.GetOrdinal(this.helper.Ichorini);
                    if (!dr.IsDBNull(iIchorini)) entity.Ichorini = dr.GetDateTime(iIchorini);

                    int iIchorfin = dr.GetOrdinal(this.helper.Ichorfin);
                    if (!dr.IsDBNull(iIchorfin)) entity.Ichorfin = dr.GetDateTime(iIchorfin);

                    int iIcdescrip1 = dr.GetOrdinal(this.helper.Icdescrip1);
                    if (!dr.IsDBNull(iIcdescrip1)) entity.Icdescrip1 = dr.GetString(iIcdescrip1);

                    int iIcdescrip2 = dr.GetOrdinal(this.helper.Icdescrip2);
                    if (!dr.IsDBNull(iIcdescrip2)) entity.Icdescrip2 = dr.GetString(iIcdescrip2);

                    int iIcdescrip3 = dr.GetOrdinal(this.helper.Icdescrip3);
                    if (!dr.IsDBNull(iIcdescrip3)) entity.Icdescrip3 = dr.GetString(iIcdescrip3);

                    int iIccheck1 = dr.GetOrdinal(this.helper.Iccheck1);
                    if (!dr.IsDBNull(iIccheck1)) entity.Iccheck1 = dr.GetString(iIccheck1);

                    int iIcvalor1 = dr.GetOrdinal(this.helper.Icvalor1);
                    if (!dr.IsDBNull(iIcvalor1)) entity.Icvalor1 = dr.GetDecimal(iIcvalor1);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveIeodcuadroDTO> ListaReqPropios(DateTime fecIni, DateTime fecFin)
        {
            List<EveIeodcuadroDTO> entitys = new List<EveIeodcuadroDTO>();
            string query = string.Format(helper.SqlListaReqPropios, fecIni.ToString(ConstantesBase.FormatoFecha), fecFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveIeodcuadroDTO entity = new EveIeodcuadroDTO();

                    int iEmprabrev = dr.GetOrdinal(this.helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iIchorini = dr.GetOrdinal(this.helper.Ichorini);
                    if (!dr.IsDBNull(iIchorini)) entity.Ichorini = dr.GetDateTime(iIchorini);

                    int iIchorfin = dr.GetOrdinal(this.helper.Ichorfin);
                    if (!dr.IsDBNull(iIchorfin)) entity.Ichorfin = dr.GetDateTime(iIchorfin);

                    int iIcdescrip1 = dr.GetOrdinal(this.helper.Icdescrip1);
                    if (!dr.IsDBNull(iIcdescrip1)) entity.Icdescrip1 = dr.GetString(iIcdescrip1);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion
    }
}
