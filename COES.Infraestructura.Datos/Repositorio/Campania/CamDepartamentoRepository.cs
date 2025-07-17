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
    public class CamDepartamentoRepository : RepositoryBase, ICamDepartamentoRepository
    {

        public CamDepartamentoRepository(string strConn) : base(strConn) { }

        CamDepartamentoHelper Helper = new CamDepartamentoHelper();

        public List<DepartamentoDTO> GetDepartamento()
        {
            List<DepartamentoDTO> departamentoDTOs = new List<DepartamentoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetDepartamentos);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DepartamentoDTO ob = new DepartamentoDTO();
                    ob.Id = dr["ID"].ToString();
                    ob.Nombre = dr["NOMBRE"].ToString();
                    departamentoDTOs.Add(ob);
                }
            }
            return departamentoDTOs;
        }



    }
}
