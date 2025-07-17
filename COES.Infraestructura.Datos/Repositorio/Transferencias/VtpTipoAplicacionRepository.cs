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
    public class VtpTipoAplicacionRepository : RepositoryBase, IVtpTipoAplicacionRepository
    {
        public VtpTipoAplicacionRepository(string strConn)
          : base(strConn)
        {
        }

        VtpTipoAplicacionHelper helper = new VtpTipoAplicacionHelper();


        public List<VtpTipoAplicacionDTO> ListTipoAplicacion()
        {
            List<VtpTipoAplicacionDTO> entitys = new List<VtpTipoAplicacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListTipoAplicacion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpTipoAplicacionDTO entity = new VtpTipoAplicacionDTO();

                    int ITipaplinombre = dr.GetOrdinal(this.helper.Tipaplinombre);
                    if (!dr.IsDBNull(ITipaplinombre)) entity.Tipaplinombre = Convert.ToString(dr.GetValue(ITipaplinombre));

                    int ITipaplicodi = dr.GetOrdinal(this.helper.Tipaplicodi);
                    if (!dr.IsDBNull(ITipaplicodi)) entity.Tipaplicodi = Convert.ToInt32(dr.GetValue(ITipaplicodi));
                    
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
