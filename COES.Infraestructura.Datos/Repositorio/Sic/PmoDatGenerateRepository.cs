using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class PmoDatGenerateRepository : RepositoryBase, IPmoDatGenerateRepository
    {
        public PmoDatGenerateRepository(string strConn)
            : base(strConn)
        {
        }

        PmoDatGenerateHelper helper = new PmoDatGenerateHelper();
        
        public void GenerateDat(int PeriCodi, string TableName, string Usuario)
        {

            if (TableName.Equals("cgndpe.dat")) {
                string queryString = string.Format(helper.SqlGenerateDatCgnd);
                DbCommand command = dbProvider.GetSqlStringCommand(queryString);                
                dbProvider.AddInParameter(command, helper.VPMPeriCodiVal, DbType.String, PeriCodi);                
                var result = dbProvider.ExecuteNonQuery(command);
            }
            else if(TableName.Equals("mgndpe.dat")){
                //
                string queryString = string.Format(helper.SqlGenerateDatMgnd);
                DbCommand command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.AddInParameter(command, helper.VPMPeriCodiVal, DbType.String, PeriCodi);
                var result = dbProvider.ExecuteNonQuery(command);
            }
            else
            {

                string queryString = string.Format(helper.SqlGenerateDat);
                DbCommand command = dbProvider.GetSqlStringCommand(queryString);

                dbProvider.AddInParameter(command, helper.VTableNameVal, DbType.String, TableName);
                dbProvider.AddInParameter(command, helper.VPMPeriCodiVal, DbType.Int32, PeriCodi);
                dbProvider.AddInParameter(command, helper.VUsuarioVal, DbType.String, Usuario);

                var result = dbProvider.ExecuteNonQuery(command);
            }
        }

        public void GenerateDatMgndPtoInstFactOpe(DateTime fecha)
        {

            string queryString = string.Format(helper.SqlGenerateDatMgndPtoInstFactOpe);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            dbProvider.AddInParameter(command, helper.VFechaVal, DbType.Date, fecha);
            
            var result = dbProvider.ExecuteNonQuery(command);

        }        

        #region NET 20190225 - Procesamiento de archivos DAT

        public void DeleteDataPorPeriodoYtipo(int pericodi, string tipo)
        {
            string sqlDelete = string.Format(helper.SqlDeleteDataDisponibilidadPorPeriodoYtipo, pericodi, tipo);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlDelete);
            dbProvider.ExecuteNonQuery(command);
        }

        public Dictionary<string, object> GetFechasProcesamientoDisponibilidad(int pericodi)
        {
            
            String query = String.Format(helper.SqlGetFechasProcesamientoDisponibilidad, pericodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            Dictionary<string, object> fechas = new Dictionary<string, object>();
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    //entity = helperEM.Create(dr);

                    int iFechaperiodo = dr.GetOrdinal(helper.Fechaperiodo);
                    if (!dr.IsDBNull(iFechaperiodo)) fechas.Add("Fechaperiodo", Convert.ToDateTime(dr.GetValue(iFechaperiodo)));

                    int iRdgfechainicio = dr.GetOrdinal(helper.Rdgfechainicio);
                    if (!dr.IsDBNull(iRdgfechainicio)) fechas.Add("Rdgfechainicio", Convert.ToDateTime(dr.GetValue(iRdgfechainicio)));

                    int iRdgfechafin = dr.GetOrdinal(helper.Rdgfechafin);
                    if (!dr.IsDBNull(iRdgfechafin)) fechas.Add("Rdgfechafin", Convert.ToDateTime(dr.GetValue(iRdgfechafin)));

                    int iMantfechainicio = dr.GetOrdinal(helper.Mantfechainicio);
                    if (!dr.IsDBNull(iMantfechainicio)) fechas.Add("Mantfechainicio", Convert.ToDateTime(dr.GetValue(iMantfechainicio)));

                    int iMantfechafin = dr.GetOrdinal(helper.Mantfechafin);
                    if (!dr.IsDBNull(iMantfechafin)) fechas.Add("Mantfechafin", Convert.ToDateTime(dr.GetValue(iMantfechafin)));

                    int iAnio = dr.GetOrdinal(helper.anio);
                    if (!dr.IsDBNull(iAnio)) fechas.Add("anio", dr.GetValue(iAnio));

                    int iMes = dr.GetOrdinal(helper.mes);
                    if (!dr.IsDBNull(iMes)) fechas.Add("mes", dr.GetValue(iMes));
                    
                }
            }

            return fechas;
        }

        PrGrupoRelasoHelper helperGR = new PrGrupoRelasoHelper();
        public List<PrGrupoRelasoDTO> GetDataGrupoRelaso(string strGrrdefcodi)
        {

            String query = String.Format(helper.SqlGetDataGrupoRelaso, strGrrdefcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<PrGrupoRelasoDTO> entitys = new List<PrGrupoRelasoDTO>();
            PrGrupoRelasoDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrGrupoRelasoDTO();

                    int iCodrel = dr.GetOrdinal(helperGR.Codrel);
                    if (!dr.IsDBNull(iCodrel)) entity.Grrasocodi = Convert.ToInt32(dr.GetValue(iCodrel));

                    int iTiporel = dr.GetOrdinal(helperGR.Tiporel);
                    if (!dr.IsDBNull(iTiporel)) entity.Tiporel = dr.GetString(iTiporel);

                    int iCodsddp = dr.GetOrdinal(helperGR.Codsddp);
                    if (!dr.IsDBNull(iCodsddp)) entity.Codsddp = Convert.ToInt32(dr.GetValue(iCodsddp));

                    int idescsddp = dr.GetOrdinal(helperGR.descsddp);
                    if (!dr.IsDBNull(idescsddp)) entity.descsddp = dr.GetString(idescsddp);

                    int iCodsic = dr.GetOrdinal(helperGR.Codsic);
                    if (!dr.IsDBNull(iCodsic)) entity.Codsic = Convert.ToInt32(dr.GetValue(iCodsic));

                    int iDescsic = dr.GetOrdinal(helperGR.Descsic);
                    if (!dr.IsDBNull(iDescsic)) entity.Descsic = dr.GetString(iDescsic);

                    int iTag = dr.GetOrdinal(helperGR.Tag);
                    if (!dr.IsDBNull(iTag)) entity.Grrasotag = dr.GetString(iTag);

                    int iSecuencia = dr.GetOrdinal(helperGR.Secuencia);
                    if (!dr.IsDBNull(iSecuencia)) entity.Grrasosecuencia = dr.GetInt32(iSecuencia);

                    int iUsuamodi = dr.GetOrdinal(helperGR.Usuamodi);
                    if (!dr.IsDBNull(iUsuamodi)) entity.Grrasousumodificacion = dr.GetString(iUsuamodi);

                    int iFechamodi = dr.GetOrdinal(helperGR.Fechamodi);
                    if (!dr.IsDBNull(iFechamodi)) entity.Grrasofecmodificacion =  Convert.ToDateTime(dr.GetValue(iFechamodi));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        
        #endregion

    }
}
