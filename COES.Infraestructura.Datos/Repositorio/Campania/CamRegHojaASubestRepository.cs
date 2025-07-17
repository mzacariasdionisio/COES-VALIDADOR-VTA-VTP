using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Campania;
using COES.Framework.Base.Tools;
using COES.Infraestructura.Datos.Helper;
using COES.Infraestructura.Datos.Helper.Campania;
using COES.Infraestructura.Datos.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamRegHojaASubestRepository: RepositoryBase, ICamRegHojaASubestRepository
    {

        public CamRegHojaASubestRepository(string strConn) : base(strConn){}

        CamRegHojaASubestHelper Helper = new CamRegHojaASubestHelper();

        public List<RegHojaASubestDTO> GetRegHojaASubestProyCodi(int proyCodi)
        {
            List<RegHojaASubestDTO> regHojaASubestDTOs = new List<RegHojaASubestDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetRegHojaASubestProyCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.String, proyCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RegHojaASubestDTO ob = new RegHojaASubestDTO();
                    ob.Centralcodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALCODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALCODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Nombresubestacion = !dr.IsDBNull(dr.GetOrdinal("NOMBRESUBESTACION")) ? dr.GetString(dr.GetOrdinal("NOMBRESUBESTACION")) : string.Empty;
                    ob.Tipoproyecto = !dr.IsDBNull(dr.GetOrdinal("TIPOPROYECTO")) ? dr.GetString(dr.GetOrdinal("TIPOPROYECTO")) : string.Empty;
                    ob.Fechapuestaservicio = !dr.IsDBNull(dr.GetOrdinal("FECHAPUESTASERVICIO")) ? dr.GetDateTime(dr.GetOrdinal("FECHAPUESTASERVICIO")) : (DateTime?)null;
                    ob.Empresapropietaria = !dr.IsDBNull(dr.GetOrdinal("EMPRESAPROPIETARIA")) ? dr.GetString(dr.GetOrdinal("EMPRESAPROPIETARIA")) : string.Empty;
                    ob.Sistemabarras = !dr.IsDBNull(dr.GetOrdinal("SISTEMABARRAS")) ? dr.GetString(dr.GetOrdinal("SISTEMABARRAS")) : string.Empty;
                    ob.Numtrafos = !dr.IsDBNull(dr.GetOrdinal("NUMTRAFOS")) ? dr.GetInt32(dr.GetOrdinal("NUMTRAFOS")) : 0;
                    ob.Numequipos = !dr.IsDBNull(dr.GetOrdinal("NUMEQUIPOS")) ? dr.GetInt32(dr.GetOrdinal("NUMEQUIPOS")) : 0;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    regHojaASubestDTOs.Add(ob);
                }
            }

            return regHojaASubestDTOs;
        }

        public bool SaveRegHojaASubest(RegHojaASubestDTO regHojaASubestDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveRegHojaASubest);
            dbProvider.AddInParameter(dbCommand, "CENTRALCODI", DbType.Int32, ObtenerValorOrDefault(regHojaASubestDTO.Centralcodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ObtenerValorOrDefault(regHojaASubestDTO.Proycodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "NOMBRESUBESTACION", DbType.String, ObtenerValorOrDefault(regHojaASubestDTO.Nombresubestacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "TIPOPROYECTO", DbType.String, ObtenerValorOrDefault(regHojaASubestDTO.Tipoproyecto, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FECHAPUESTASERVICIO", DbType.DateTime, regHojaASubestDTO.Fechapuestaservicio);
            dbProvider.AddInParameter(dbCommand, "EMPRESAPROPIETARIA", DbType.String, ObtenerValorOrDefault(regHojaASubestDTO.Empresapropietaria, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "SISTEMABARRAS", DbType.String, ObtenerValorOrDefault(regHojaASubestDTO.Sistemabarras, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "NUMTRAFOS", DbType.Int32, ObtenerValorOrDefault(regHojaASubestDTO.Numtrafos, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "NUMEQUIPOS", DbType.Int32, ObtenerValorOrDefault(regHojaASubestDTO.Numequipos, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ObtenerValorOrDefault(regHojaASubestDTO.Usucreacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteRegHojaASubestById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteRegHojaASubestById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastRegHojaASubestId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastRegHojaASubestId);
            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                count = Convert.ToInt32(result) + 1;
            }
            else
            {
                count = 1;
            }
            return count;
        }

        public RegHojaASubestDTO GetRegHojaASubestById(int id)
        {
            RegHojaASubestDTO ob = new RegHojaASubestDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetRegHojaASubestById);
            dbProvider.AddInParameter(commandHoja, "CENTRALCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.Centralcodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALCODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALCODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Nombresubestacion = !dr.IsDBNull(dr.GetOrdinal("NOMBRESUBESTACION")) ? dr.GetString(dr.GetOrdinal("NOMBRESUBESTACION")) : string.Empty;
                    ob.Tipoproyecto = !dr.IsDBNull(dr.GetOrdinal("TIPOPROYECTO")) ? dr.GetString(dr.GetOrdinal("TIPOPROYECTO")) : string.Empty;
                    ob.Fechapuestaservicio = !dr.IsDBNull(dr.GetOrdinal("FECHAPUESTASERVICIO")) ? dr.GetDateTime(dr.GetOrdinal("FECHAPUESTASERVICIO")) : (DateTime?)null;
                    ob.Empresapropietaria = !dr.IsDBNull(dr.GetOrdinal("EMPRESAPROPIETARIA")) ? dr.GetString(dr.GetOrdinal("EMPRESAPROPIETARIA")) : string.Empty;
                    ob.Sistemabarras = !dr.IsDBNull(dr.GetOrdinal("SISTEMABARRAS")) ? dr.GetString(dr.GetOrdinal("SISTEMABARRAS")) : string.Empty;
                    ob.Numtrafos = !dr.IsDBNull(dr.GetOrdinal("NUMTRAFOS")) ? dr.GetInt32(dr.GetOrdinal("NUMTRAFOS")) : 0;
                    ob.Numequipos = !dr.IsDBNull(dr.GetOrdinal("NUMEQUIPOS")) ? dr.GetInt32(dr.GetOrdinal("NUMEQUIPOS")) : 0;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }

            }
            return ob;
        }

        public bool UpdateRegHojaASubest(RegHojaASubestDTO regHojaASubestDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateRegHojaASubest);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, regHojaASubestDTO.Proycodi);
            dbProvider.AddInParameter(dbCommand, "NOMBRESUBESTACION", DbType.String, regHojaASubestDTO.Nombresubestacion);
            dbProvider.AddInParameter(dbCommand, "TIPOPROYECTO", DbType.String, regHojaASubestDTO.Tipoproyecto);
            dbProvider.AddInParameter(dbCommand, "FECHAPUESTASERVICIO", DbType.DateTime, regHojaASubestDTO.Fechapuestaservicio);
            dbProvider.AddInParameter(dbCommand, "EMPRESAPROPIETARIA", DbType.String, regHojaASubestDTO.Empresapropietaria);
            dbProvider.AddInParameter(dbCommand, "SISTEMABARRAS", DbType.String, regHojaASubestDTO.Sistemabarras);
            dbProvider.AddInParameter(dbCommand, "NUMTRAFOS", DbType.Int32, regHojaASubestDTO.Numtrafos);
            dbProvider.AddInParameter(dbCommand, "NUMEQUIPOS", DbType.Int32, regHojaASubestDTO.Numequipos);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, regHojaASubestDTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "CENTRALCODI", DbType.Int32, regHojaASubestDTO.Centralcodi);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        object ObtenerValorOrDefault(object valor, Type tipo)
        {
            DateTime fechaMinimaValida = DateTime.Now;
            if (valor == null || (valor is DateTime && (DateTime)valor == DateTime.MinValue))
            {
                if (tipo == typeof(int) || tipo == typeof(int?))
                {
                    return 0;
                }
                else if (tipo == typeof(string))
                {
                    return "";
                }
                else if (tipo == typeof(DateTime) || tipo == typeof(DateTime?))
                {
                    return fechaMinimaValida;
                }
            }
            return valor;
        }

    }
}
