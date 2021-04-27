<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="PMISAdmin.ContentPages.Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Системна грешка</title>
    
    <link href="../StyleSheets/Main.css" rel="StyleSheet" type="text/css" />    
    
    <style>
        .SystemErrorMessage
        {
        	margin: 0 auto;
        	width: 700px;
        	font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    
    <div class="HeaderPanel">
       <div class="LogoSection" style="z-index: 2;"></div>
       <div class="MasterHeader1">АСУ на човешките ресурси</div>
       <div class="MasterHeader2"></div>
    </div>
    
    <div class="ContentPanel">
        <div style="height: 100px;"></div>
        
        <div class="SystemErrorMessage">
           <img src="../Images/error.png" style="float: left; margin-right: 10px;" />
           Възникна системна грешка по време на изпълнение на програмата, която възпрепятства по нататъшното ѝ изпълнение. <br />
           Проблемът е отразен на сървъра. Моля, свържете с администраторите на системата. <br />
           За да продължите работа с програмата започнете нова сесия.
        </div>
    </div>
    
    <div class="FooterPanel"></div>
    
    </form>
</body>
</html>