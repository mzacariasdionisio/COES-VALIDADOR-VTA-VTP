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
    /// Clase de acceso a datos de la tabla IN_ESTADO
    /// </summary>
    public class InEstadoRepository: RepositoryBase, IInEstadoRepository
    {
        public InEstadoRepository(string strConn): base(strConn)
        {

        }

        InEstadoHelper helper = new InEstadoHelper();
        
        //--------------------------------------------------------------------------------
        // ASSETEC.SGH - 09/10/2017: FUNCIONES PERSONALIZADAS PARA ESTADOS
        //--------------------------------------------------------------------------------
        public List<InEstadoDTO> ListarComboEstados(int iEscenario)
        {
            List<InEstadoDTO> entitys = new List<InEstadoDTO>();
            DbCommand command = null;

            if (iEscenario == 1) // 1 = Mantenimiento
                command = dbProvider.GetSqlStringCommand(helper.SqlListarComboEstadosMantenimiento);
            else if (iEscenario == 2) // 2 = Consulta
                command = dbProvider.GetSqlStringCommand(helper.SqlListarComboEstadosConsultas);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    InEstadoDTO entity = new InEstadoDTO();

                    int iEstadocodi = dr.GetOrdinal(helper.Estadocodi);
                    if (!dr.IsDBNull(iEstadocodi)) entity.Estadocodi = Convert.ToInt32(dr.GetValue(iEstadocodi));

                    int iEstadonomb = dr.GetOrdinal(helper.Estadonomb);
                    if (!dr.IsDBNull(iEstadonomb)) entity.Estadonomb = dr.GetString(iEstadonomb);

                    int iEstadopadre = dr.GetOrdinal(helper.Estadopadre);
                    if (!dr.IsDBNull(iEstadopadre)) entity.Estadopadre = Convert.ToInt32(dr.GetValue(iEstadopadre));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }



    }
}
