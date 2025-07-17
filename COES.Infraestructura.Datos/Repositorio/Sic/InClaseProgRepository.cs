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
    /// Clase de acceso a datos de la tabla IN_CLASEPROG
    /// </summary>
    public class InClaseProgRepository: RepositoryBase, IInClaseProgRepository
    {
        public InClaseProgRepository(string strConn): base(strConn)
        {

        }

        InClaseprogHelper helper = new InClaseprogHelper();        

        //-----------------------------------------------------------------------------------
        // ASSETEC.SGH - 09/10/2017: FUNCIONES PERSONALIZADAS PARA LAS CLASES DE PROGRAMACION
        //-----------------------------------------------------------------------------------
        public List<InClaseProgDTO> ListarComboClasesProgramacion()
        {
            List<InClaseProgDTO> entitys = new List<InClaseProgDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarComboClasesProgramacion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {                    
                    InClaseProgDTO entity = new InClaseProgDTO();

                    int iClaprocodi = dr.GetOrdinal(helper.Claprocodi);
                    if (!dr.IsDBNull(iClaprocodi)) entity.Claprocodi = Convert.ToInt32(dr.GetValue(iClaprocodi));

                    int iClapronombre = dr.GetOrdinal(helper.Clapronombre);
                    if (!dr.IsDBNull(iClapronombre)) entity.Clapronombre = dr.GetString(iClapronombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
