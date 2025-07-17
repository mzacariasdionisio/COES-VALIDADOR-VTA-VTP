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
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class VceHoraOperacionRepository : RepositoryBase, IVceHoraOperacionRepository
    {
        public VceHoraOperacionRepository(string strConn) : base(strConn)
        {
        }

        VceHoraOperacionHelper helper = new VceHoraOperacionHelper();

        public void Save(VceHoraOperacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            
            dbProvider.AddInParameter(command, helper.Crhophorfinajust, DbType.DateTime, entity.Crhophorfinajust);
            dbProvider.AddInParameter(command, helper.Crhophoriniajust, DbType.DateTime, entity.Crhophoriniajust);
            dbProvider.AddInParameter(command, helper.Crhopcompordpard, DbType.String, entity.Crhopcompordpard);
            dbProvider.AddInParameter(command, helper.Crhopcompordarrq, DbType.String, entity.Crhopcompordarrq);
            dbProvider.AddInParameter(command, helper.Crhopdesc, DbType.String, entity.Crhopdesc);
            dbProvider.AddInParameter(command, helper.Crhopcausacodi, DbType.Int32, entity.Crhopcausacodi);
            dbProvider.AddInParameter(command, helper.Crhoplimtrans, DbType.String, entity.Crhoplimtrans);
            dbProvider.AddInParameter(command, helper.Crhopsaislado, DbType.Int32, entity.Crhopsaislado);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Crhophorparada, DbType.DateTime, entity.Crhophorparada);
            dbProvider.AddInParameter(command, helper.Crhophorarranq, DbType.DateTime, entity.Crhophorarranq);
            dbProvider.AddInParameter(command, helper.Crhophorfin, DbType.DateTime, entity.Crhophorfin);
            dbProvider.AddInParameter(command, helper.Crhophorini, DbType.DateTime, entity.Crhophorini);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Hopcodi, DbType.Int32, entity.Hopcodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(VceHoraOperacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Crhophorfinajust, DbType.DateTime, entity.Crhophorfinajust);
            dbProvider.AddInParameter(command, helper.Crhophoriniajust, DbType.DateTime, entity.Crhophoriniajust);
            dbProvider.AddInParameter(command, helper.Crhopcompordpard, DbType.String, entity.Crhopcompordpard);
            dbProvider.AddInParameter(command, helper.Crhopcompordarrq, DbType.String, entity.Crhopcompordarrq);
            dbProvider.AddInParameter(command, helper.Crhopdesc, DbType.String, entity.Crhopdesc);
            dbProvider.AddInParameter(command, helper.Crhopcausacodi, DbType.Int32, entity.Crhopcausacodi);
            dbProvider.AddInParameter(command, helper.Crhoplimtrans, DbType.String, entity.Crhoplimtrans);
            dbProvider.AddInParameter(command, helper.Crhopsaislado, DbType.Int32, entity.Crhopsaislado);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Crhophorparada, DbType.DateTime, entity.Crhophorparada);
            dbProvider.AddInParameter(command, helper.Crhophorarranq, DbType.DateTime, entity.Crhophorarranq);
            dbProvider.AddInParameter(command, helper.Crhophorfin, DbType.DateTime, entity.Crhophorfin);
            dbProvider.AddInParameter(command, helper.Crhophorini, DbType.DateTime, entity.Crhophorini);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Hopcodi, DbType.Int32, entity.Hopcodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateRangoHora(VceHoraOperacionDTO entity)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateRangoHora);

            dbProvider.AddInParameter(command, helper.Crhophorfin, DbType.DateTime, entity.Crhophorfin);
            dbProvider.AddInParameter(command, helper.Crhophorini, DbType.DateTime, entity.Crhophorini);
            dbProvider.AddInParameter(command, helper.Hopcodi, DbType.Int32, entity.Hopcodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int hopcodi, int pecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Hopcodi, DbType.Int32, hopcodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, pecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VceHoraOperacionDTO GetById(int hopcodi, int pecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Hopcodi, DbType.Int32, hopcodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, pecacodi);
            VceHoraOperacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VceHoraOperacionDTO> List()
        {
            List<VceHoraOperacionDTO> entitys = new List<VceHoraOperacionDTO>();
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

        public List<VceHoraOperacionDTO> GetByCriteria()
        {
            List<VceHoraOperacionDTO> entitys = new List<VceHoraOperacionDTO>();
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

        //NETC

        public void SaveByRango(int pecacodi, string fechaini, string fechafin)
        {
            string queryString = string.Format(helper.SqlSaveByRango, pecacodi, fechaini, fechafin);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteById(int pecacodi)
        {
            string queryString = string.Format(helper.SqlDeleteById, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }


        public List<VceHoraOperacionDTO> ListFiltro(int pecacodi, string empresa, string central, string grupo, string modo, string tipo, string fecIni, string fecFin, string arranque, string parada)
        {
            List<VceHoraOperacionDTO> entitys = new List<VceHoraOperacionDTO>();

            string condicion = "";

            //- compensaciones.HDT - Inicio 27/02/2017: Cambio para atender el requerimiento.
            if (!string.IsNullOrEmpty(empresa))
            {
                condicion = condicion + " AND EMP.EMPRCODI = " + empresa;            
            }

            if (!string.IsNullOrEmpty(central))
            {
                condicion = condicion + " AND CG.EQUICODI = " + central;
            }

            if (!string.IsNullOrEmpty(grupo))
            {
                condicion = condicion + " AND GG.GRUPOCODI = " + grupo;
            }

            //- HDT Fin

            if (modo != null && !modo.Equals("")) 
            {
                condicion = condicion + " AND MO.GRUPOCODI = " + modo;
            }

            if (tipo != null && !tipo.Equals("") )
            {
                condicion = condicion + " AND HO.SUBCAUSACODI = " + tipo;
            }

            if (fecIni != null && !fecIni.Equals("") )
            {
                condicion = condicion + " AND TRUNC(HO.CRHOPHORINI) >= TO_DATE('" + fecIni + "' , 'DD/MM/YYYY') ";
            }

            if (fecFin != null && !fecFin.Equals(""))
            {
                condicion = condicion + " AND TRUNC(HO.CRHOPHORINI) <= TO_DATE('" + fecFin + "' , 'DD/MM/YYYY') ";
            }

            if ((arranque != null && !arranque.Equals("") ) || (parada != null && !parada.Equals("")))
            {                
                string condicionap = "";

                if (arranque != null && arranque.Equals("S") )
                {
                    condicionap = " HO.CRHOPCOMPORDARRQ = '" + arranque + "'"; 
                }
                if (parada != null && parada.Equals("S"))
                {
                    if (!condicionap.Equals(""))
                        condicionap = condicionap + " OR ";

                    condicionap = condicionap + " HO.CRHOPCOMPORDPARD = '" + parada + "'";  
                }

                if (!condicionap.Equals(""))
                    condicion = condicion + " AND ( " + condicionap+ " ) ";
            }

            string queryString = string.Format(helper.SqlListFiltro, pecacodi, condicion);
            Console.WriteLine(queryString);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VceHoraOperacionDTO entity = new VceHoraOperacionDTO();
                    {

                        int iEmpresa = dr.GetOrdinal(helper.Empresa);
                        if (!dr.IsDBNull(iEmpresa)) entity.Empresa = dr.GetString(iEmpresa);

                        int iCentral = dr.GetOrdinal(helper.Central);
                        if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                        int iGrupo = dr.GetOrdinal(helper.Grupo);
                        if (!dr.IsDBNull(iGrupo)) entity.Grupo = dr.GetString(iGrupo);

                        int iModoOperacion = dr.GetOrdinal(helper.ModoOperacion);
                        if (!dr.IsDBNull(iModoOperacion)) entity.ModoOperacion = dr.GetString(iModoOperacion);

                        int iHopCodi = dr.GetOrdinal(helper.Hopcodi);
                        if (!dr.IsDBNull(iHopCodi)) entity.Hopcodi = dr.GetInt32(iHopCodi);

                        int iPecacodi = dr.GetOrdinal(helper.Pecacodi);
                        if (!dr.IsDBNull(iPecacodi)) entity.PecaCodi = dr.GetInt32(iPecacodi);

                        int iCrhopHorIni = dr.GetOrdinal(helper.Crhophorini);
                        if (!dr.IsDBNull(iCrhopHorIni)) entity.Crhophorini = dr.GetDateTime(iCrhopHorIni);

                        int iCrhopHorFin = dr.GetOrdinal(helper.Crhophorfin);
                        if (!dr.IsDBNull(iCrhopHorFin)) entity.Crhophorfin = dr.GetDateTime(iCrhopHorFin);

                        int iCrhopHorArrAnq = dr.GetOrdinal(helper.Crhophorarranq);
                        if (!dr.IsDBNull(iCrhopHorArrAnq)) entity.Crhophorarranq = dr.GetDateTime(iCrhopHorArrAnq);

                        int iCrhopHorParAda = dr.GetOrdinal(helper.Crhophorparada);
                        if (!dr.IsDBNull(iCrhopHorParAda)) entity.Crhophorparada = dr.GetDateTime(iCrhopHorParAda);

                        int iTipoOperacion = dr.GetOrdinal(helper.TipoOperacion);
                        if (!dr.IsDBNull(iTipoOperacion)) entity.TipoOperacion = dr.GetString(iTipoOperacion);

                        int iCrhopSAislado = dr.GetOrdinal(helper.Crhopsaislado);
                        if (!dr.IsDBNull(iCrhopSAislado)) entity.Crhopsaislado = dr.GetInt32(iCrhopSAislado);

                        int iCrhopLimTrans = dr.GetOrdinal(helper.Crhoplimtrans);
                        if (!dr.IsDBNull(iCrhopLimTrans)) entity.Crhoplimtrans = dr.GetString(iCrhopLimTrans);

                        int iCrhopCausaCodi = dr.GetOrdinal(helper.Crhopcausacodi);
                        if (!dr.IsDBNull(iCrhopCausaCodi)) entity.Crhopcausacodi = dr.GetInt32(iCrhopCausaCodi);

                        int iCrhopDesc = dr.GetOrdinal(helper.Crhopdesc);
                        if (!dr.IsDBNull(iCrhopDesc)) entity.Crhopdesc = dr.GetString(iCrhopDesc);

                        int iCrhopCompOrdArrq = dr.GetOrdinal(helper.Crhopcompordarrq);
                        if (!dr.IsDBNull(iCrhopCompOrdArrq)) entity.Crhopcompordarrq = dr.GetString(iCrhopCompOrdArrq);

                        int iCrhopCompOrdPard = dr.GetOrdinal(helper.Crhopcompordpard);
                        if (!dr.IsDBNull(iCrhopCompOrdPard)) entity.Crhopcompordpard = dr.GetString(iCrhopCompOrdPard);

                        entitys.Add(entity);
                    }
                }
            }
            return entitys;
        }

        public List<VceHoraOperacionDTO> ListById(int pecacodi)
        {
            List<VceHoraOperacionDTO> entitys = new List<VceHoraOperacionDTO>();
            string queryString = string.Format(helper.SqlListById, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VceHoraOperacionDTO entity = new VceHoraOperacionDTO();

                    int iEmpresa = dr.GetOrdinal(helper.Empresa);
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = dr.GetString(iEmpresa);

                    int iCentral = dr.GetOrdinal(helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iGrupo = dr.GetOrdinal(helper.Grupo);
                    if (!dr.IsDBNull(iGrupo)) entity.Grupo = dr.GetString(iGrupo);

                    int iModoOperacion = dr.GetOrdinal(helper.ModoOperacion);
                    if (!dr.IsDBNull(iModoOperacion)) entity.ModoOperacion = dr.GetString(iModoOperacion);

                    int iCrhopHorIni = dr.GetOrdinal(helper.Crhophorini);
                    if (!dr.IsDBNull(iCrhopHorIni)) entity.Crhophorini = dr.GetDateTime(iCrhopHorIni);

                    int iCrhopHorFin = dr.GetOrdinal(helper.Crhophorfin);
                    if (!dr.IsDBNull(iCrhopHorFin)) entity.Crhophorfin = dr.GetDateTime(iCrhopHorFin);

                    int iCrhopHorArrAnq = dr.GetOrdinal(helper.Crhophorarranq);
                    if (!dr.IsDBNull(iCrhopHorArrAnq)) entity.Crhophorarranq = dr.GetDateTime(iCrhopHorArrAnq);

                    int iCrhopHorParAda = dr.GetOrdinal(helper.Crhophorparada);
                    if (!dr.IsDBNull(iCrhopHorParAda)) entity.Crhophorparada = dr.GetDateTime(iCrhopHorParAda);

                    int iTipoOperacion = dr.GetOrdinal(helper.TipoOperacion);
                    if (!dr.IsDBNull(iTipoOperacion)) entity.TipoOperacion = dr.GetString(iTipoOperacion);

                    int iCrhopSAislado = dr.GetOrdinal(helper.Crhopsaislado);
                    if (!dr.IsDBNull(iCrhopSAislado)) entity.Crhopsaislado = dr.GetInt32(iCrhopSAislado);

                    int iCrhopLimTrans = dr.GetOrdinal(helper.Crhoplimtrans);
                    if (!dr.IsDBNull(iCrhopLimTrans)) entity.Crhoplimtrans = dr.GetString(iCrhopLimTrans);

                    int iCrhopCausaCodi = dr.GetOrdinal(helper.Crhopcausacodi);
                    if (!dr.IsDBNull(iCrhopCausaCodi)) entity.Crhopcausacodi = dr.GetInt32(iCrhopCausaCodi);

                    int iCrhopDesc = dr.GetOrdinal(helper.Crhopdesc);
                    if (!dr.IsDBNull(iCrhopDesc)) entity.Crhopdesc = dr.GetString(iCrhopDesc);

                    int iCrhopCompOrdArrq = dr.GetOrdinal(helper.Crhopcompordarrq);
                    if (!dr.IsDBNull(iCrhopCompOrdArrq)) entity.Crhopcompordarrq = dr.GetString(iCrhopCompOrdArrq);

                    int iCrhopCompOrdPard = dr.GetOrdinal(helper.Crhopcompordpard);
                    if (!dr.IsDBNull(iCrhopCompOrdPard)) entity.Crhopcompordpard = dr.GetString(iCrhopCompOrdPard);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public void SaveFromOtherVersion(int pecacodiDestino, int pecacodiOrigen)
        {
            string queryString = string.Format(helper.SqlSaveFromOtherVersion, pecacodiDestino, pecacodiOrigen);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<VceHoraOperacionDTO> ListVerificarHoras(int pecacodi)
        {
            List<VceHoraOperacionDTO> entitys = new List<VceHoraOperacionDTO>();
            string queryString = string.Format(helper.SqlListVerificarHoras, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VceHoraOperacionDTO entity = new VceHoraOperacionDTO();

                    int iModoOperacion = dr.GetOrdinal(helper.ModoOperacion);
                    if (!dr.IsDBNull(iModoOperacion)) entity.ModoOperacion = dr.GetString(iModoOperacion);

                    int iHopCodi = dr.GetOrdinal(helper.Hopcodi);
                    if (!dr.IsDBNull(iHopCodi)) entity.Hopcodi = dr.GetInt32(iHopCodi);

                    int iCrhopHorIni = dr.GetOrdinal(helper.Crhophorini);
                    if (!dr.IsDBNull(iCrhopHorIni)) entity.Crhophorini = dr.GetDateTime(iCrhopHorIni);

                    int iCrhopHorFin = dr.GetOrdinal(helper.Crhophorfin);
                    if (!dr.IsDBNull(iCrhopHorFin)) entity.Crhophorfin = dr.GetDateTime(iCrhopHorFin);

                    int iTipoOperacion = dr.GetOrdinal(helper.TipoOperacion);
                    if (!dr.IsDBNull(iTipoOperacion)) entity.TipoOperacion = dr.GetString(iTipoOperacion);

                    int iHopCodi2 = dr.GetOrdinal(helper.Hopcodi2);
                    if (!dr.IsDBNull(iHopCodi2)) entity.Hopcodi2 = dr.GetInt32(iHopCodi2);

                    int iCrhopHorIni2 = dr.GetOrdinal(helper.Crhophorini2);
                    if (!dr.IsDBNull(iCrhopHorIni2)) entity.Crhophorini2 = dr.GetDateTime(iCrhopHorIni2);

                    int iCrhopHorFin2 = dr.GetOrdinal(helper.Crhophorfin2);
                    if (!dr.IsDBNull(iCrhopHorFin2)) entity.Crhophorfin2 = dr.GetDateTime(iCrhopHorFin2);

                    int iTipoOperacion2 = dr.GetOrdinal(helper.TipoOperacion2);
                    if (!dr.IsDBNull(iTipoOperacion2)) entity.TipoOperacion2 = dr.GetString(iTipoOperacion2);

                    int iPecacodi = dr.GetOrdinal(helper.Pecacodi);
                    if (!dr.IsDBNull(iPecacodi)) entity.PecaCodi = dr.GetInt32(iPecacodi);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public VceHoraOperacionDTO GetDataById(int hopcodi, int pecacodi)
        {
            VceHoraOperacionDTO entity = new VceHoraOperacionDTO();
            string queryString = string.Format(helper.SqlGetDataById, pecacodi, hopcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    int iModoOperacion = dr.GetOrdinal(helper.ModoOperacion);
                    if (!dr.IsDBNull(iModoOperacion)) entity.ModoOperacion = dr.GetString(iModoOperacion);

                    int iHopCodi = dr.GetOrdinal(helper.Hopcodi);
                    if (!dr.IsDBNull(iHopCodi)) entity.Hopcodi = dr.GetInt32(iHopCodi);

                    int iCrhopHorIni = dr.GetOrdinal(helper.Crhophorini);
                    if (!dr.IsDBNull(iCrhopHorIni)) entity.Crhophorini = dr.GetDateTime(iCrhopHorIni);

                    int iCrhopHorFin = dr.GetOrdinal(helper.Crhophorfin);
                    if (!dr.IsDBNull(iCrhopHorFin)) entity.Crhophorfin = dr.GetDateTime(iCrhopHorFin);

                    int iTipoOperacion = dr.GetOrdinal(helper.TipoOperacion);
                    if (!dr.IsDBNull(iTipoOperacion)) entity.TipoOperacion = dr.GetString(iTipoOperacion);

                    int iPecacodi = dr.GetOrdinal(helper.Pecacodi);
                    if (!dr.IsDBNull(iPecacodi)) entity.PecaCodi = dr.GetInt32(iPecacodi);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);
                }
            }

            return entity;
        }

    }

}
