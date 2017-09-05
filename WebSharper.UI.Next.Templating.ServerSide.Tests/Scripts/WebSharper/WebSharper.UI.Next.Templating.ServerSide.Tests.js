(function()
{
 "use strict";
 var Global,WebSharper,UI,Next,ServerSide,Tests,Client,WebSharper$UI$Next$Templating$ServerSide$Tests_Templates,Var,Doc,List;
 Global=window;
 WebSharper=Global.WebSharper=Global.WebSharper||{};
 UI=WebSharper.UI=WebSharper.UI||{};
 Next=UI.Next=UI.Next||{};
 ServerSide=Next.ServerSide=Next.ServerSide||{};
 Tests=ServerSide.Tests=ServerSide.Tests||{};
 Client=Tests.Client=Tests.Client||{};
 WebSharper$UI$Next$Templating$ServerSide$Tests_Templates=Global.WebSharper$UI$Next$Templating$ServerSide$Tests_Templates=Global.WebSharper$UI$Next$Templating$ServerSide$Tests_Templates||{};
 Var=Next&&Next.Var;
 Doc=Next&&Next.Doc;
 List=WebSharper&&WebSharper.List;
 Client.OnClick=function(el,ev)
 {
  Global.alert("clicked!");
 };
 Client.OldMain=function(init)
 {
  var I;
  I=Var.Create$1(init);
  return Doc.Concat([Doc.Element("div",[],[Doc.TextNode("\n            "),Doc.Input([],I),Doc.TextNode("\n            "),Doc.TextView(I.RView()),Doc.TextNode("\n        ")])]);
 };
 Client.Main=function(init)
 {
  var t,t$1;
  return WebSharper$UI$Next$Templating$ServerSide$Tests_Templates.clienttemplate((t=(t$1=new List.T({
   $:1,
   $0:{
    $:1,
    $0:"before",
    $1:init
   },
   $1:List.T.Empty
  }),new List.T({
   $:1,
   $0:{
    $:6,
    $0:"input",
    $1:Var.Create$1(init)
   },
   $1:t$1
  })),new List.T({
   $:1,
   $0:{
    $:11,
    $0:"opacity",
    $1:Var.Create$1(init.length/10)
   },
   $1:t
  })));
 };
 WebSharper$UI$Next$Templating$ServerSide$Tests_Templates.clienttemplate=function(h)
 {
  Doc.LoadLocalTemplates("main");
  return h?Doc.NamedTemplate("main",{
   $:1,
   $0:"clienttemplate"
  },h):void 0;
 };
}());

//# sourceMappingURL=WebSharper.UI.Next.Templating.ServerSide.Tests.map