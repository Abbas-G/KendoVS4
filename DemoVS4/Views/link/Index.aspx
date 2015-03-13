<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Index</title>
</head>
<body>
    <div>
                     <a href="<%=Url.Content("~/Grid/Inline")%>" target="_blank" >Grid/Inline</a><br /> 
                     <a href="<%=Url.Content("~/Grid/InlineCommandImage")%>" target="_blank" >Grid/InlineCommandImage</a><br />     
                     <a href="<%=Url.Content("~/Grid/Popup")%>" target="_blank" >Grid/Popup</a><br />
                     <a href="<%=Url.Content("~/Grid/Mix")%>" target="_blank" >Grid/Mix</a><br />
                     <a href="<%=Url.Content("~/Grid/GridFromModel")%>" target="_blank" >Grid/GridFromModel</a><br />
                     <a href="<%=Url.Content("~/Grid/InCell")%>" target="_blank" >Grid/InCell</a><br />
                     <a href="<%=Url.Content("~/ServerGrid/Index")%>" target="_blank" >ServerGrid/Index</a><br />
                     <a href="<%=Url.Content("~/ServerInline/Index")%>" target="_blank" >ServerInline/Index</a><br />
                     <a href="<%=Url.Content("~/ServerGridDetail/Index")%>" target="_blank" >ServerGridDetail/Index</a><br />
                     <a href="<%=Url.Content("~/GridMultiDropdown/Index")%>" target="_blank" >GridMultiDropdown/Index</a><br />
                     <a href="<%=Url.Content("~/GridMultiDropdown/multiform")%>" target="_blank" >GridMultiDropdown/multiform</a><br />
                     <a href="<%=Url.Content("~/TVP/Index")%>" target="_blank" >TVP/xml</a><br />
    
    </div>
</body>
</html>
