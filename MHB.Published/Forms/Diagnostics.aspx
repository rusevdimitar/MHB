<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="Diagnostics.aspx.vb" Inherits="MHB.Web.Diagnostics" %>

<%@ Import Namespace="MHB.Logging" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <% Me.CheckAccess() %>

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.0/themes/base/jquery-ui.css" />
    <script src="http://code.jquery.com/jquery-1.8.3.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/ui/1.10.0/jquery-ui.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $("#accordion-assemblies").accordion();
            $("#accordion-exceptions").accordion(
            {
                height: 'content'
            });
            $("#accordion-actionlogs").accordion(
            {
                height: 'content'
            });
            $("#tabs").tabs();
        });
    </script>

    <div id="tabs">
        <ul>
            <li><a href="#tabs-1">Loaded assemblies</a></li>
            <li><a href="#tabs-2">Exceptions logs</a></li>
            <li><a href="#tabs-3">Action logs</a></li>
        </ul>
        <div id="tabs-1">
            <div id="accordion-assemblies">
                <%                                        
                    For Each assembly As System.Reflection.Assembly In AppDomain.CurrentDomain.GetAssemblies().OrderByDescending(Function(ass) ass.FullName.StartsWith("MHB"))
                %>
                <h3>
                    <%                
                        Dim location As String = String.Empty
                        Dim fileInfo As System.IO.FileInfo = Nothing
                
                        Try
                            location = assembly.Location
                    
                            If System.IO.File.Exists(location) Then
                                fileInfo = New IO.FileInfo(location)
                                Response.Write(fileInfo.Name)
                            End If
                        Catch
                        End Try
                    %>
                </h3>
                <div>
                    <p>
                        <% 
                                        
                            If Not String.IsNullOrEmpty(location) Then
                                Response.Write(String.Format("<strong>Date modified:</strong> {0}", fileInfo.CreationTime))
                                Response.Write("<br />")
                                Response.Write(String.Format("<strong>Full name:</strong> {0}", assembly.FullName))
                                Response.Write("<br />")
                                Response.Write(String.Format("<strong>Size on disk:</strong> {0} bytes", fileInfo.Length))
                                Response.Write("<br />")
                                Response.Write(String.Format("<strong>File:</strong> {0}", fileInfo.FullName))
                                Response.Write("<br />")
                                Response.Write(String.Format("<strong>Build mode:</strong> {0}", IIf(MHB.Web.Environment.IsAssemblyDebugBuild(assembly), "Debug", "Release")))
                            End If
                    
                        %>
                    </p>
                </div>
                <%
                Next
                %>
            </div>
        </div>
        <div id="tabs-2">
            <div id="accordion-exceptions">
                <% 
                    Dim exceptionLog As ExceptionLog = New ExceptionLog(Me.GetConnectionString())
                
                    Dim exceptions As List(Of ExceptionLog) = exceptionLog.LoadAll(DateTime.Now.ToShortDateString())
                
                    For Each ex As ExceptionLog In exceptions
                    
                %>
                <h3>
                    <% Response.Write(String.Format("Date: {0} Method: {1}", ex.LogDate, ex.MethodName))%>
                </h3>
                <div>
                    <p>
                        <% 
                            Response.Write(String.Format("<strong>User ID:</strong> {0}", ex.UserID))
                            Response.Write("<br />")
                            Response.Write(String.Format("<strong>Source:</strong> {0}", ex.LogSource))
                            Response.Write("<br />")
                            Response.Write(String.Format("<strong>Message:</strong> {0}", ex.LogMessage))
                            Response.Write("<br />")
                            Response.Write(String.Format("<strong>Inner exception:</strong> {0}", ex.LogInnerExceptionMessage))
                            Response.Write("<br />")
                            Response.Write(String.Format("<strong>Stack trace:</strong> {0}", ex.LogStackTrace))
                            Response.Write("<br />")
                            Response.Write(String.Format("<strong>SQL query:</strong> {0}", ex.SqlQuery))
                            Response.Write("<br />")
                        %>
                    </p>
                </div>
                <%                    
                Next
                %>
            </div>
        </div>
        <div id="tabs-3">
            <div id="accordion-actionlogs">
                <% 
                    Dim actionLog As ActionLog = New ActionLog(Me.GetConnectionString())
                    
                    Dim actionLogs As List(Of ActionLog) = actionLog.LoadAll(DateTime.Now.ToShortDateString())
                    
                    For Each log As ActionLog In actionLogs
                %>
                <h3>
                    <% Response.Write(String.Format("{0} {1} {2}", log.UserID, log.Message, log.LogDate))%>
                </h3>
                <div style="background-color: <%= log.Color%>">
                    <p style="background-color: <%= log.Color%>">
                        <%                            
                            Response.Write(String.Format("<strong>User ID: {0}</strong>", log.UserID))
                            Response.Write("<br />")
                            Response.Write(String.Format("<strong>Email: {0}</strong>", log.UserEmail))
                            Response.Write("<br />")
                            Response.Write(String.Format("<strong>Password: {0}</strong>", log.UserPassword))
                            Response.Write("<br />")
                            Response.Write(String.Format("<strong>IP Address: {0}</strong>", log.IP))
                            Response.Write("<br />")
                            Response.Write(String.Format("<strong>Region: {0}</strong>", log.Region))
                            Response.Write("<br />")                            
                            Response.Write(String.Format("<strong>Message: {0}</strong>", log.Message))
                            Response.Write("<br />")
                            Response.Write(String.Format("<strong>Transaction: {0}</strong>", log.TransactionMessage))
                            Response.Write("<br />")
                                          
                        %>
                    </p>
                </div>
                <%                    
                Next
                %>
            </div>
        </div>

    </div>


</asp:Content>
