<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintDeclarationOfAccident.aspx.cs"
    Inherits="PMIS.HealthSafety.PrintContentPages.PrintDeclarationOfAccident" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">    
    <title>Декларация за трудова злополука</title>
    
    <style type="text/css">
        @media print
        {
           .noPrint
           {
              display: none;
           }
        }
        
        p
        {
           margin-top: 0px;
           margin-bottom: 0px;
        }
        
        body
        {
        	font-family: "Times New Roman";
            font-size: 10pt;
        }
        
        h1
	    {
           margin:0in;
	       margin-bottom:.0001pt;
	       text-align:center;
	       page-break-after:avoid;
	       font-size:9.0pt;
	       font-family:""Times New Roman"";
        }
        
        h2
	    { 
           margin:0in;
	       margin-bottom:.0001pt;
	       text-align:center;
	       page-break-after:avoid;
	       font-size:14.0pt;
	       font-family:""Times New Roman"";
        }
        
        h3
	    {
           margin:0in;
	       margin-bottom: .0001pt;
	       text-align: justify;
	       page-break-after: avoid;
	       font-size: 9.0pt;
	       font-family: ""Times New Roman"";
        }
        
        h4
	    {
           margin:0in;
	       margin-bottom:.0001pt;
	       page-break-after:avoid;
	       font-size:12.0pt;
	       font-family:""Arial Narrow"";
	       font-weight:normal;
	       font-style:italic;
        }
        
        h6
	    {
           margin:0in;
	       margin-bottom:.0001pt;
	       text-align:center;
	       text-indent:-49.65pt;
	       page-break-after:avoid;
	       font-size:9.0pt;
	       font-family:""Times New Roman"";
        }
        
        p.MsoBodyText3, li.MsoBodyText3, div.MsoBodyText3
	    {
           margin:0in;
	       margin-bottom:.0001pt;
	       text-align:justify;
	       font-size:8.0pt;
	       font-family:""Times New Roman"";
	       font-style:italic;
        }
        
        
        .FormValue
        {
           font-family: "Courier New";
           font-size: 9pt;
        }
    </style>
    
    <script type="text/javascript">
    function ExportToWord() {
        document.getElementById("<%= btnGenerateWord.ClientID %>").click();
    }
</script>
</head>
<body>
    <form id="form1" runat="server">
       <table cellpadding="0" cellspacing="0">
          <tr>
             <td>
                <div runat="server" ID="pnlDeclaration" style="width: 705px;"></div>
             </td>
             <td style="position: relative; vertical-align: top; padding-left: 10px;">
                <div style="position: absolute; top: 1px; width: 120px;" class="noPrint">
                    <img style="cursor: pointer;" src="../Images/print.png" title="Печат" alt="Печат" onclick="window.print();" />
                    &nbsp;
                    <img style="cursor: pointer;" src="../Images/WordIcon.gif" title="Запазване в Word" alt="Запазване в Word" onclick="ExportToWord();" />
                    &nbsp;
                    <img style="cursor: pointer;" src="../Images/close.png" title="Затвори" alt="Затвори" onclick="window.close();" />
                </div>
                <div style="position: absolute; bottom: 1px; width: 120px;" class="noPrint">
                    <img style="cursor: pointer;" src="../Images/print.png" title="Печат" alt="Печат" onclick="window.print();" />
                    &nbsp;
                    <img style="cursor: pointer;" src="../Images/WordIcon.gif" title="Запазване в Word" alt="Запазване в Word" onclick="ExportToWord();" />
                    &nbsp;
                    <img style="cursor: pointer;" src="../Images/close.png" title="Затвори" alt="Затвори" onclick="window.close();" />
                </div>
             </td>
          </tr>
       </table>
       
       <asp:Button ID="btnGenerateWord" runat="server" OnClick="btnGenerateWord_Click"  />
    </form>
</body>
</html>
