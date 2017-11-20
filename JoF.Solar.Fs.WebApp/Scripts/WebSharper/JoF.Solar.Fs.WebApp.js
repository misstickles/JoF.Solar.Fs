(function()
{
 "use strict";
 var Global,JoF,Solar,Fs,WebApp,Client,SC$1,WebSharper,UI,Next,Doc,AttrProxy,Charting,Renderers,ChartJs,Remoting,AjaxRemotingProvider;
 Global=window;
 JoF=Global.JoF=Global.JoF||{};
 Solar=JoF.Solar=JoF.Solar||{};
 Fs=Solar.Fs=Solar.Fs||{};
 WebApp=Fs.WebApp=Fs.WebApp||{};
 Client=WebApp.Client=WebApp.Client||{};
 SC$1=Global.StartupCode$JoF_Solar_Fs_WebApp$Client=Global.StartupCode$JoF_Solar_Fs_WebApp$Client||{};
 WebSharper=Global.WebSharper;
 UI=WebSharper&&WebSharper.UI;
 Next=UI&&UI.Next;
 Doc=Next&&Next.Doc;
 AttrProxy=Next&&Next.AttrProxy;
 Charting=WebSharper&&WebSharper.Charting;
 Renderers=Charting&&Charting.Renderers;
 ChartJs=Renderers&&Renderers.ChartJs;
 Remoting=WebSharper&&WebSharper.Remoting;
 AjaxRemotingProvider=Remoting&&Remoting.AjaxRemotingProvider;
 Client.Main=function()
 {
  return Doc.Element("div",[],[Doc.Element("div",[AttrProxy.Create("class","jumbotron")],[Client.RenderChart()])]);
 };
 Client.RenderChart=function()
 {
  SC$1.$cctor();
  return SC$1.RenderChart;
 };
 SC$1.$cctor=function()
 {
  SC$1.$cctor=Global.ignore;
  SC$1.RenderChart=ChartJs.Render$2((new AjaxRemotingProvider.New()).Sync("JoF.Solar.Fs.WebApp:JoF.Solar.Fs.WebApp.Server.SunRiseSet:1389197670",[]),{
   $:1,
   $0:{
    $:0,
    $0:500,
    $1:500
   }
  },{
   $:1,
   $0:{}
  },null);
 };
}());
