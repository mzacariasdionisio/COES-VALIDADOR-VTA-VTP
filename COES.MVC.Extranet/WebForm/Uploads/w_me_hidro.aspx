<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_base.Master" AutoEventWireup="true" CodeBehind="w_me_hidro.aspx.cs" Inherits="WSIC2010.Uploads.w_me_idcc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            height: 23px;
        }
        .style2
        {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table id="Table2"   border="1">
				<thead>
					<tr>
						<td style="WIDTH: 35px" class="style1">Item</td>
						<td class="style5">Archivo Excel IDCC</td>
						<td class="style2">Empresa</td>					
						
					</tr>

                    <tr>
					    <td style="HEIGHT: 23px" class="style2">1</td>
					    <td class="style6">
                        <asp:HyperLink id="Hyperlink110" runat="server" Height="15px" 
                            Width="300px" NavigateUrl="http://sicoes.coes.org.pe/formatos/hidro/HIDRO_EDG_ANEXO1_PDIARIOCAUDAL3D_aaaammdd.xls"
							BackColor="Transparent" Font-Size="X-Small" CssClass="style8">HIDRO_EDG_ANEXO1_PDIARIOCAUDAL3D_aaaammdd.xls</asp:HyperLink>
                            <p>
                        <asp:HyperLink id="Hyperlink111" runat="server" Height="15px" 
                            Width="300px" NavigateUrl="http://sicoes.coes.org.pe/formatos/hidro/HIDRO_EDG_ANEXO1_PSEMANALCAUDAL10D_aaaammdd.xls"
							BackColor="Transparent" Font-Size="X-Small" CssClass="style8">HIDRO_EDG_ANEXO1_PSEMANALCAUDAL10D_aaaammdd.xls</asp:HyperLink>
                            <p>
                        <asp:HyperLink id="Hyperlink113" runat="server" Height="15px" 
                            Width="300px" NavigateUrl="http://sicoes.coes.org.pe/formatos/hidro/HIDRO_EDG_ANEXO2_EJECUTADOEMBALSE_aaaammdd.xls"
							BackColor="Transparent" Font-Size="X-Small" CssClass="style8">HIDRO_EDG_ANEXO2_EJECUTADOEMBALSE_aaaammdd.xls</asp:HyperLink>
                            <p>
                        <asp:HyperLink id="Hyperlink114" runat="server" Height="15px" 
                            Width="300px" NavigateUrl="http://sicoes.coes.org.pe/formatos/hidro/HIDRO_EDG_ANEXO3_EJECUTADOCAUDAL_aaaammdd.xls"
							BackColor="Transparent" Font-Size="X-Small" CssClass="style8">HIDRO_EDG_ANEXO3_EJECUTADOCAUDAL_aaaammdd.xls</asp:HyperLink>                            
                    </td>
					<td class="style9">EDEGEL</td>
					
				</tr>
				</thead>

				

			</table>
</asp:Content>
