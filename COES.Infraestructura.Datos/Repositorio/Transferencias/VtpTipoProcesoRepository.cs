using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    public class VtpTipoProcesoRepository : RepositoryBase, IVtpTipoProcesoRepository
    {
        public VtpTipoProcesoRepository(string strConn)
          : base(strConn)
        {
        }

        VtpTipoProcesoHelper helper = new VtpTipoProcesoHelper();


        public List<VtpTipoProcesoDTO> ListTipoProceso(int tipaplicodi)
        {
            List<VtpTipoProcesoDTO> entitys = new List<VtpTipoProcesoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListTipoProceso);
            dbProvider.AddInParameter(command, helper.Tipprocodi, DbType.Int32, tipaplicodi);
          
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpTipoProcesoDTO entity = new VtpTipoProcesoDTO();

                    int iTipprodescripcion = dr.GetOrdinal(this.helper.Tipprodescripcion);
                    if (!dr.IsDBNull(iTipprodescripcion)) entity.Tipprodescripcion = Convert.ToString(dr.GetValue(iTipprodescripcion));

                    int iTipprocodi = dr.GetOrdinal(this.helper.Tipprocodi);
                    if (!dr.IsDBNull(iTipprocodi)) entity.Tipprocodi = Convert.ToInt32(dr.GetValue(iTipprocodi));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
