using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{   /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class CodigoRetiroDetalleRepository : RepositoryBase, ICodigoRetiroDetalleRepository
    {
        public CodigoRetiroDetalleRepository(string strConn) : base(strConn)
        {
        }
        CodigoRetiroDetalleHelper helper = new CodigoRetiroDetalleHelper();
        public List<CodigoRetiroDetalleDTO> ListarCodigoRetiroDetalle(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarCodigoRetiroDetalle);

            dbProvider.AddInParameter(command, helper.CoresoCodi, DbType.Int32, id);
            List<CodigoRetiroDetalleDTO> entitys = new List<CodigoRetiroDetalleDTO>();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
    }
}
