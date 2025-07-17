using COES.Base.Core;
using COES.Dominio.DTO.Campania;
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
    public class CamDetalleperiodoRepository : RepositoryBase, ICamDetalleperiodoRepository
    {
        public CamDetalleperiodoRepository(string strConn) : base(strConn) { }

        CamDetallePeriodoHelper Helper = new CamDetallePeriodoHelper();
        CamPeriodoHelper HelperPeriodo = new CamPeriodoHelper();


        public bool SaveDetalle(DetallePeriodoDTO fichPro)
        {
            DbCommand dbcommad = dbProvider.GetSqlStringCommand(Helper.SQLSaveDetalle);
            dbProvider.AddInParameter(dbcommad, "DETPERICODI", DbType.Int32, fichPro.DetPeriCodigo);
            dbProvider.AddInParameter(dbcommad, "PERICODI", DbType.Int32, fichPro.PeriCodigo);
            dbProvider.AddInParameter(dbcommad, "HOJACODI", DbType.Int32, fichPro.HojaCodigo);
            dbProvider.AddInParameter(dbcommad, "USU_CREACION", DbType.String, fichPro.UsuarioCreacion);
            dbProvider.AddInParameter(dbcommad, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbcommad, "IND_DEL", DbType.String, fichPro.IndDel);
            dbProvider.ExecuteNonQuery(dbcommad);
            return true;
        }

        public bool DeleteDetalleById(int pericodi, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteDetalleById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PERICODI", DbType.Int32, pericodi);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public List<DetallePeriodoDTO> GetDetalleByPericodi(int pericodi, string inddel)
        {
            List<DetallePeriodoDTO> entitys = new List<DetallePeriodoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetDetalleByPericodi);
            dbProvider.AddInParameter(command, "PERICODI", DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, inddel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DetallePeriodoDTO ob = new DetallePeriodoDTO();
                    ob.DetPeriCodigo = Int32.Parse(dr["DETPERICODI"].ToString());
                    ob.PeriCodigo = Int32.Parse(dr["PERICODI"].ToString());
                    ob.HojaCodigo = Int32.Parse(dr["HOJACODI"].ToString());
                    entitys.Add(ob);
                }
            }

            return entitys;
        }


        public List<int> GetDetalleHojaByPericodi(int pericodi, string inddel)
        {
            List<int> entitys = new List<int>();

            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetDetalleByPericodi);
            dbProvider.AddInParameter(command, "PERICODI", DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(Int32.Parse(dr["HOJACODI"].ToString()));
                }
            }

            return entitys;
        }

        public int GetDetalleId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetDetalleId);
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




    }
}
