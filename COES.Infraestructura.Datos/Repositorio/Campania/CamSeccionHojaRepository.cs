using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.Interfaces.Campania;
using COES.Infraestructura.Datos.Helper.Campania;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamSeccionHojaRepository: RepositoryBase, ICamSeccionHojasRepository
    {

        public CamSeccionHojaRepository(string strConn) : base(strConn) { }

        CamSeccionHojaHelper Helper = new CamSeccionHojaHelper();

        public List<SeccionHojasDTO> GetSeccionHojaByHojaCod(int hojaCodi)
        {
            List<SeccionHojasDTO> SeccionHojaDTOs = new List<SeccionHojasDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetSeccionHojaHojaCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, hojaCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SeccionHojasDTO ob = new SeccionHojasDTO();
                    ob.SeccCodi = !dr.IsDBNull(dr.GetOrdinal("SECCCODI")) ? dr.GetInt32(dr.GetOrdinal("ARCHCODI")) : 0;
                    ob.HojaCodi = !dr.IsDBNull(dr.GetOrdinal("HOJACODI")) ? dr.GetInt32(dr.GetOrdinal("SECCCODI")) : 0;
                    ob.SeccNombre = !dr.IsDBNull(dr.GetOrdinal("SECCNOMBRE")) ? dr.GetString(dr.GetOrdinal("SECCNOMBRE")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    SeccionHojaDTOs.Add(ob);
                }
            }

            return SeccionHojaDTOs;
        }
    }
}
