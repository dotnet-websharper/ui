(function()
{
 "use strict";
 var Global,WebSharper,UI,Next,Array,String,Abbrev,Fresh,HashSet,Dict,Slot,Async,Mailbox,SC$1,AppendList,SC$2,DomUtility,SC$3,Trie,LookupResult,SC$4,Snap,Var,Updates,View,ViewBuilder,Submitter,Key,Model,Serializer,Storage,ArrayStorage,LocalStorageBackend,ListModel,ListModels,SC$5,ReactiveExtensions,DoubleInterpolation,Interpolation,Easing,Easings,An,Anims,Trans,SC$6,AnimatedAttrNode,DynamicAttrNode,AttrProxy,Client,Attrs,Dyn,Attrs$1,CheckedInput,AttrModule,SC$7,DocElemNode,Docs,DomNodes,NodeSet,RunState,Docs$1,Doc,Elt,EltUpdater,SC$8,Flow,FlowBuilder,Html,attr,Route,Routing,State,RouteMap,Router,Input,MousePosSt,MouseBtnSt,Mouse,KeyListenerSt,Keyboard,SC$9,List,Arrays,Enumerator,Strings,Char,Collections,HashSet$1,Seq,IntelliFactory,Runtime,Unchecked,console,Concurrency,Map,FSharpMap,List$1,Operators,Queue,Lazy,Dictionary,JSON,Numeric,Node,$,Slice;
 Global=window;
 WebSharper=Global.WebSharper=Global.WebSharper||{};
 UI=WebSharper.UI=WebSharper.UI||{};
 Next=UI.Next=UI.Next||{};
 Array=Next.Array=Next.Array||{};
 String=Next.String=Next.String||{};
 Abbrev=Next.Abbrev=Next.Abbrev||{};
 Fresh=Abbrev.Fresh=Abbrev.Fresh||{};
 HashSet=Abbrev.HashSet=Abbrev.HashSet||{};
 Dict=Abbrev.Dict=Abbrev.Dict||{};
 Slot=Abbrev.Slot=Abbrev.Slot||{};
 Async=Abbrev.Async=Abbrev.Async||{};
 Mailbox=Abbrev.Mailbox=Abbrev.Mailbox||{};
 SC$1=Global.StartupCode$WebSharper_UI_Next$Abbrev=Global.StartupCode$WebSharper_UI_Next$Abbrev||{};
 AppendList=Next.AppendList=Next.AppendList||{};
 SC$2=Global.StartupCode$WebSharper_UI_Next$AppendList=Global.StartupCode$WebSharper_UI_Next$AppendList||{};
 DomUtility=Next.DomUtility=Next.DomUtility||{};
 SC$3=Global.StartupCode$WebSharper_UI_Next$DomUtility=Global.StartupCode$WebSharper_UI_Next$DomUtility||{};
 Trie=Next.Trie=Next.Trie||{};
 LookupResult=Trie.LookupResult=Trie.LookupResult||{};
 SC$4=Global.StartupCode$WebSharper_UI_Next$Trie=Global.StartupCode$WebSharper_UI_Next$Trie||{};
 Snap=Next.Snap=Next.Snap||{};
 Var=Next.Var=Next.Var||{};
 Updates=Next.Updates=Next.Updates||{};
 View=Next.View=Next.View||{};
 ViewBuilder=Next.ViewBuilder=Next.ViewBuilder||{};
 Submitter=Next.Submitter=Next.Submitter||{};
 Key=Next.Key=Next.Key||{};
 Model=Next.Model=Next.Model||{};
 Serializer=Next.Serializer=Next.Serializer||{};
 Storage=Next.Storage=Next.Storage||{};
 ArrayStorage=Storage.ArrayStorage=Storage.ArrayStorage||{};
 LocalStorageBackend=Storage.LocalStorageBackend=Storage.LocalStorageBackend||{};
 ListModel=Next.ListModel=Next.ListModel||{};
 ListModels=Next.ListModels=Next.ListModels||{};
 SC$5=Global.StartupCode$WebSharper_UI_Next$Models=Global.StartupCode$WebSharper_UI_Next$Models||{};
 ReactiveExtensions=Next.ReactiveExtensions=Next.ReactiveExtensions||{};
 DoubleInterpolation=Next.DoubleInterpolation=Next.DoubleInterpolation||{};
 Interpolation=Next.Interpolation=Next.Interpolation||{};
 Easing=Next.Easing=Next.Easing||{};
 Easings=Next.Easings=Next.Easings||{};
 An=Next.An=Next.An||{};
 Anims=Next.Anims=Next.Anims||{};
 Trans=Next.Trans=Next.Trans||{};
 SC$6=Global.StartupCode$WebSharper_UI_Next$Animation=Global.StartupCode$WebSharper_UI_Next$Animation||{};
 AnimatedAttrNode=Next.AnimatedAttrNode=Next.AnimatedAttrNode||{};
 DynamicAttrNode=Next.DynamicAttrNode=Next.DynamicAttrNode||{};
 AttrProxy=Next.AttrProxy=Next.AttrProxy||{};
 Client=Next.Client=Next.Client||{};
 Attrs=Client.Attrs=Client.Attrs||{};
 Dyn=Attrs.Dyn=Attrs.Dyn||{};
 Attrs$1=Next.Attrs=Next.Attrs||{};
 CheckedInput=Next.CheckedInput=Next.CheckedInput||{};
 AttrModule=Next.AttrModule=Next.AttrModule||{};
 SC$7=Global.StartupCode$WebSharper_UI_Next$Attr_Client=Global.StartupCode$WebSharper_UI_Next$Attr_Client||{};
 DocElemNode=Next.DocElemNode=Next.DocElemNode||{};
 Docs=Client.Docs=Client.Docs||{};
 DomNodes=Docs.DomNodes=Docs.DomNodes||{};
 NodeSet=Docs.NodeSet=Docs.NodeSet||{};
 RunState=Docs.RunState=Docs.RunState||{};
 Docs$1=Next.Docs=Next.Docs||{};
 Doc=Next.Doc=Next.Doc||{};
 Elt=Next.Elt=Next.Elt||{};
 EltUpdater=Client.EltUpdater=Client.EltUpdater||{};
 SC$8=Global.StartupCode$WebSharper_UI_Next$Doc_Client=Global.StartupCode$WebSharper_UI_Next$Doc_Client||{};
 Flow=Next.Flow=Next.Flow||{};
 FlowBuilder=Next.FlowBuilder=Next.FlowBuilder||{};
 Html=Next.Html=Next.Html||{};
 attr=Html.attr=Html.attr||{};
 Route=Next.Route=Next.Route||{};
 Routing=Next.Routing=Next.Routing||{};
 State=Routing.State=Routing.State||{};
 RouteMap=Next.RouteMap=Next.RouteMap||{};
 Router=Next.Router=Next.Router||{};
 Input=Next.Input=Next.Input||{};
 MousePosSt=Input.MousePosSt=Input.MousePosSt||{};
 MouseBtnSt=Input.MouseBtnSt=Input.MouseBtnSt||{};
 Mouse=Input.Mouse=Input.Mouse||{};
 KeyListenerSt=Input.KeyListenerSt=Input.KeyListenerSt||{};
 Keyboard=Input.Keyboard=Input.Keyboard||{};
 SC$9=Global.StartupCode$WebSharper_UI_Next$Input=Global.StartupCode$WebSharper_UI_Next$Input||{};
 List=WebSharper&&WebSharper.List;
 Arrays=WebSharper&&WebSharper.Arrays;
 Enumerator=WebSharper&&WebSharper.Enumerator;
 Strings=WebSharper&&WebSharper.Strings;
 Char=WebSharper&&WebSharper.Char;
 Collections=WebSharper&&WebSharper.Collections;
 HashSet$1=Collections&&Collections.HashSet;
 Seq=WebSharper&&WebSharper.Seq;
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
 Unchecked=WebSharper&&WebSharper.Unchecked;
 console=Global.console;
 Concurrency=WebSharper&&WebSharper.Concurrency;
 Map=Collections&&Collections.Map;
 FSharpMap=Collections&&Collections.FSharpMap;
 List$1=Collections&&Collections.List;
 Operators=WebSharper&&WebSharper.Operators;
 Queue=WebSharper&&WebSharper.Queue;
 Lazy=WebSharper&&WebSharper.Lazy;
 Dictionary=Collections&&Collections.Dictionary;
 JSON=Global.JSON;
 Numeric=WebSharper&&WebSharper.Numeric;
 Node=Global.Node;
 $=Global.jQuery;
 Slice=WebSharper&&WebSharper.Slice;
 Array.mapInPlace=function(f,arr)
 {
  var i,$1;
  for(i=0,$1=arr.length-1;i<=$1;i++)arr[i]=f(arr[i]);
  return arr;
 };
 Array.ofSeqNonCopying=function(xs)
 {
  var q,o;
  if(xs instanceof Global.Array)
   return xs;
  else
   if(xs instanceof List.T)
    return Arrays.ofList(xs);
   else
    if(xs===null)
     return[];
    else
     {
      q=[];
      o=Enumerator.Get(xs);
      try
      {
       while(o.MoveNext())
        q.push(o.Current());
       return q;
      }
      finally
      {
       if("Dispose"in o)
        o.Dispose();
      }
     }
 };
 Array.MapTreeReduce=function(mapping,defaultValue,reduction,array)
 {
  var l;
  function loop(off,len)
  {
   var $1,l2;
   return len<=0?defaultValue:len===1&&(off>=0&&off<l&&true)?mapping(Arrays.get(array,off)):(l2=len/2>>0,reduction(loop(off,l2),loop(off+l2,len-l2)));
  }
  l=Arrays.length(array);
  return loop(0,l);
 };
 Array.TreeReduce=function(defaultValue,reduction,array)
 {
  var l;
  function loop(off,len)
  {
   var $1,l2;
   return len<=0?defaultValue:len===1&&(off>=0&&off<l&&true)?Arrays.get(array,off):(l2=len/2>>0,reduction(loop(off,l2),loop(off+l2,len-l2)));
  }
  l=Arrays.length(array);
  return loop(0,l);
 };
 String.isBlank=function(s)
 {
  return Strings.forall(Char.IsWhiteSpace,s);
 };
 Fresh.Id=function()
 {
  Fresh.set_counter(Fresh.counter()+1);
  return"uid"+Global.String(Fresh.counter());
 };
 Fresh.Int=function()
 {
  Fresh.set_counter(Fresh.counter()+1);
  return Fresh.counter();
 };
 Fresh.counter=function()
 {
  SC$1.$cctor();
  return SC$1.counter;
 };
 Fresh.set_counter=function($1)
 {
  SC$1.$cctor();
  SC$1.counter=$1;
 };
 HashSet.Filter=function(ok,set)
 {
  return new HashSet$1.New$2(Arrays.filter(ok,HashSet.ToArray(set)));
 };
 HashSet.Intersect=function(a,b)
 {
  var set;
  set=new HashSet$1.New$2(HashSet.ToArray(a));
  set.IntersectWith(HashSet.ToArray(b));
  return set;
 };
 HashSet.Except=function(excluded,included)
 {
  var set;
  set=new HashSet$1.New$2(HashSet.ToArray(included));
  set.ExceptWith(HashSet.ToArray(excluded));
  return set;
 };
 HashSet.ToArray=function(set)
 {
  var arr;
  arr=Arrays.create(set.get_Count(),void 0);
  set.CopyTo(arr);
  return arr;
 };
 Dict.ToValueArray=function(d)
 {
  var arr;
  arr=Arrays.create(d.count,void 0);
  Seq.iteri(function(i,kv)
  {
   return Arrays.set(arr,i,kv.V);
  },d);
  return arr;
 };
 Dict.ToKeyArray=function(d)
 {
  var arr;
  arr=Arrays.create(d.count,void 0);
  Seq.iteri(function(i,kv)
  {
   return Arrays.set(arr,i,kv.K);
  },d);
  return arr;
 };
 Slot=Abbrev.Slot=Runtime.Class({
  GetHashCode:function()
  {
   return Unchecked.Hash(this.key(this.value));
  },
  Equals:function(o)
  {
   return Unchecked.Equals(this.key(this.value),this.key(o.get_Value()));
  },
  get_Value:function()
  {
   return this.value;
  }
 },WebSharper.Obj,Slot);
 Slot.New=Runtime.Ctor(function(key,value)
 {
  this.key=key;
  this.value=value;
 },Slot);
 Slot.Create=function(key,value)
 {
  return new Slot.New(key,value);
 };
 Async.OnError=function(e)
 {
  return console.log("WebSharper UI.Next: Uncaught asynchronous exception",e);
 };
 Async.StartTo=function(comp,k)
 {
  Concurrency.StartWithContinuations(comp,k,function(e)
  {
   Async.OnError(e);
  },Global.ignore,null);
 };
 Mailbox.StartProcessor=function(procAsync)
 {
  var st;
  function work()
  {
   var b;
   b=null;
   return Concurrency.Delay(function()
   {
    return Concurrency.Bind(procAsync,function()
    {
     var m;
     m=st[0];
     return Unchecked.Equals(m,1)?(st[0]=0,Concurrency.Zero()):Unchecked.Equals(m,2)?(st[0]=1,work()):Concurrency.Zero();
    });
   });
  }
  st=[0];
  return function()
  {
   var m;
   m=st[0];
   Unchecked.Equals(m,0)?(st[0]=1,Concurrency.Start(work(),null)):Unchecked.Equals(m,1)?st[0]=2:void 0;
  };
 };
 SC$1.$cctor=function()
 {
  SC$1.$cctor=Global.ignore;
  SC$1.counter=0;
 };
 AppendList.FromArray=function(xs)
 {
  var m;
  m=xs.length;
  return m===0?{
   $:0
  }:m===1?{
   $:1,
   $0:Arrays.get(xs,0)
  }:{
   $:3,
   $0:xs.slice()
  };
 };
 AppendList.ToArray=function(xs)
 {
  var out;
  function loop(xs$1)
  {
   if(xs$1.$==1)
    out.push(xs$1.$0);
   else
    if(xs$1.$==2)
     {
      loop(xs$1.$0);
      loop(xs$1.$1);
     }
    else
     if(xs$1.$==3)
      Arrays.iter(function(v)
      {
       out.push(v);
      },xs$1.$0);
  }
  out=[];
  loop(xs);
  return out.slice(0);
 };
 AppendList.Single=function(x)
 {
  return{
   $:1,
   $0:x
  };
 };
 AppendList.Concat=function(xs)
 {
  var x;
  x=Array.ofSeqNonCopying(xs);
  return Array.TreeReduce(AppendList.Empty(),AppendList.Append,x);
 };
 AppendList.Append=function(x,y)
 {
  return x.$==0?y:y.$==0?x:{
   $:2,
   $0:x,
   $1:y
  };
 };
 AppendList.Empty=function()
 {
  SC$2.$cctor();
  return SC$2.Empty;
 };
 SC$2.$cctor=function()
 {
  SC$2.$cctor=Global.ignore;
  SC$2.Empty={
   $:0
  };
 };
 DomUtility.SetProperty=function(target,name,value)
 {
  return target.setProperty(name,value);
 };
 DomUtility.IterSelector=function(el,selector,f)
 {
  var l,i,$1;
  l=el.querySelectorAll(selector);
  for(i=0,$1=l.length-1;i<=$1;i++)f(l[i]);
 };
 DomUtility.ChildrenArray=function(element)
 {
  var a,i,$1;
  a=[];
  for(i=0,$1=element.childNodes.length-1;i<=$1;i++)a.push(element.childNodes[i]);
  return a;
 };
 DomUtility.RemoveClass=function(element,cl)
 {
  Global.jQuery(element).removeClass(cl);
 };
 DomUtility.AddClass=function(element,cl)
 {
  Global.jQuery(element).addClass(cl);
 };
 DomUtility.InsertAt=function(parent,pos,node)
 {
  var m;
  if(!(node.parentNode===parent&&pos===(m=node.nextSibling,Unchecked.Equals(m,null)?null:m)))
   parent.insertBefore(node,pos);
 };
 DomUtility.RemoveNode=function(parent,el)
 {
  if(el.parentNode===parent)
   parent.removeChild(el);
 };
 DomUtility.SetStyle=function(el,name,value)
 {
  DomUtility.SetProperty(el.style,name,value);
 };
 DomUtility.SetAttr=function(el,name,value)
 {
  el.setAttribute(name,value);
 };
 DomUtility.RemoveAttr=function(el,attrName)
 {
  el.removeAttribute(attrName);
 };
 DomUtility.CreateAttr=function(name,value)
 {
  var a;
  a=DomUtility.Doc().createAttribute(name);
  a.value=value;
  return a;
 };
 DomUtility.CreateText=function(s)
 {
  return DomUtility.Doc().createTextNode(s);
 };
 DomUtility.CreateSvgElement=function(name)
 {
  return DomUtility.Doc().createElementNS("http://www.w3.org/2000/svg",name);
 };
 DomUtility.CreateElement=function(name)
 {
  return DomUtility.Doc().createElement(name);
 };
 DomUtility.Clear=function(ctx)
 {
  while(ctx.hasChildNodes())
   ctx.removeChild(ctx.firstChild);
 };
 DomUtility.ClearAttrs=function(ctx)
 {
  while(ctx.hasAttributes())
   ctx.removeAttributeNode(ctx.attributes.item(0));
 };
 DomUtility.AppendTo=function(ctx,node)
 {
  ctx.appendChild(node);
 };
 DomUtility.Doc=function()
 {
  SC$3.$cctor();
  return SC$3.Doc;
 };
 SC$3.$cctor=function()
 {
  SC$3.$cctor=Global.ignore;
  SC$3.Doc=Global.document;
 };
 LookupResult.NotFound={
  $:1
 };
 Trie.Empty=function()
 {
  SC$4.$cctor();
  return SC$4.Empty;
 };
 Trie.Lookup=function(trie,key)
 {
  return Trie.Look(List.ofSeq(key),trie);
 };
 Trie.Look=function(key,trie)
 {
  var $1,m;
  switch(trie.$==2?($1=trie.$0,0):trie.$==0?key.$==1?($1=[key.$0,key.$1,trie.$0],1):2:2)
  {
   case 0:
    return{
     $:0,
     $0:$1,
     $1:key
    };
    break;
   case 1:
    m=Map.TryFind($1[0],$1[2]);
    return m==null?LookupResult.NotFound:Trie.Look($1[1],m.$0);
    break;
   case 2:
    return LookupResult.NotFound;
    break;
  }
 };
 Trie.ToArray=function(trie)
 {
  var all;
  all=[];
  Trie.Map(function()
  {
   return function(v)
   {
    return all.push(v);
   };
  },trie);
  return all.slice(0);
 };
 Trie.Mapi=function(f,trie)
 {
  var counter;
  function next()
  {
   var c;
   c=counter[0];
   counter[0]=c+1;
   return c;
  }
  counter=[0];
  return Trie.Map(function(x)
  {
   return f(next(),x);
  },trie);
 };
 Trie.Map=function(f,trie)
 {
  return Trie.MapLoop(List.T.Empty,f,trie);
 };
 Trie.MapLoop=function(loc,f,trie)
 {
  return trie.$==1?{
   $:1
  }:trie.$==2?{
   $:2,
   $0:(f(loc))(trie.$0)
  }:Trie.TrieBranch(Map.Map(function(k,v)
  {
   return Trie.MapLoop(List.append(loc,List.ofArray([k])),f,v);
  },trie.$0));
 };
 Trie.Merge=function(ts)
 {
  var ts$1,m,o;
  ts$1=Array.ofSeqNonCopying(ts);
  m=Arrays.length(ts$1);
  return m===0?{
   $:1,
   $0:{
    $:1
   }
  }:m===1?{
   $:1,
   $0:Arrays.get(ts$1,0)
  }:Arrays.exists(Trie.IsLeaf,ts$1)?null:(o=Trie.MergeMaps(Trie.Merge,Seq.choose(function(a)
  {
   return a.$==0?{
    $:1,
    $0:a.$0
   }:null;
  },ts$1)),o==null?null:{
   $:1,
   $0:Trie.TrieBranch(o.$0)
  });
 };
 Trie.IsLeaf=function(t)
 {
  return t.$==2&&true;
 };
 Trie.MergeMaps=function(merge,maps)
 {
  var o;
  function m(k,vs)
  {
   var o$1;
   o$1=merge(vs);
   return o$1==null?null:{
    $:1,
    $0:[k,o$1.$0]
   };
  }
  o=Trie.AllSome(Seq.map(function($1)
  {
   return m($1[0],$1[1]);
  },Map.ToSeq(Seq.fold(function(s,t)
  {
   return Trie.MultiAdd(t[0],t[1],s);
  },new FSharpMap.New([]),Seq.collect(Map.ToSeq,maps)))));
  return o==null?null:{
   $:1,
   $0:Map.OfArray(Arrays.ofSeq(o.$0))
  };
 };
 Trie.AllSome=function(xs)
 {
  var ok,e,r,m;
  e=Enumerator.Get(xs);
  r=new List$1.New$2();
  ok=true;
  while(ok&&e.MoveNext())
   {
    m=e.Current();
    m!=null&&m.$==1?r.Add(m.$0):ok=false;
   }
  return ok?{
   $:1,
   $0:r.ToArray()
  }:null;
 };
 Trie.MultiAdd=function(key,value,map)
 {
  return map.Add(key,new List.T({
   $:1,
   $0:value,
   $1:Trie.MultiFind(key,map)
  }));
 };
 Trie.MultiFind=function(key,map)
 {
  var x;
  x=Map.TryFind(key,map);
  return x==null?List.T.Empty:x.$0;
 };
 Trie.Prefix=function(key,trie)
 {
  return Trie.TrieBranch(new FSharpMap.New(List.ofArray([[key,trie]])));
 };
 Trie.Leaf=function(v)
 {
  return{
   $:2,
   $0:v
  };
 };
 Trie.TrieBranch=function(xs)
 {
  return xs.get_IsEmpty()?{
   $:1
  }:{
   $:0,
   $0:xs
  };
 };
 SC$4.$cctor=function()
 {
  SC$4.$cctor=Global.ignore;
  SC$4.Empty={
   $:1
  };
 };
 Snap.Obsolete=function(sn)
 {
  var $1,m,i,$2,o;
  m=sn.s;
  if(m==null||(m!=null&&m.$==2?($1=m.$1,false):m!=null&&m.$==3?($1=m.$1,false):true))
   void 0;
  else
   {
    sn.s=null;
    for(i=0,$2=Arrays.length($1)-1;i<=$2;i++){
     o=Arrays.get($1,i);
     typeof o=="object"?function(sn$1)
     {
      Snap.Obsolete(sn$1);
     }(o):o();
    }
   }
 };
 Snap.New=function(State$1)
 {
  return{
   s:State$1
  };
 };
 Snap.MapAsync=function(fn,snap)
 {
  var res;
  res=Snap.New({
   $:3,
   $0:[],
   $1:[]
  });
  Snap.When(snap,function(v)
  {
   Async.StartTo(fn(v),function(v$1)
   {
    Snap.MarkDone(res,snap,v$1);
   });
  },res);
  return res;
 };
 Snap.SnapshotOn=function(sn1,sn2)
 {
  var res;
  function cont(a)
  {
   var m,$1,$2;
   if(!(m=res.s,m!=null&&m.$==0||m!=null&&m.$==2&&true))
    {
     $1=Snap.ValueAndForever(sn1);
     $2=Snap.ValueAndForever(sn2);
     $1!=null&&$1.$==1?$2!=null&&$2.$==1?$1.$0[1]||$2.$0[1]?Snap.MarkForever(res,$2.$0[0]):Snap.MarkReady(res,$2.$0[0]):void 0:void 0;
    }
  }
  res=Snap.New({
   $:3,
   $0:[],
   $1:[]
  });
  Snap.When(sn1,cont,res);
  Snap.WhenReady(sn2,cont);
  return res;
 };
 Snap.Map3=function(fn,sn1,sn2,sn3)
 {
  var $1,$2,$3,res;
  function cont(a)
  {
   var m,$4,$5,$6;
   if(!(m=res.s,m!=null&&m.$==0||m!=null&&m.$==2&&true))
    {
     $4=Snap.ValueAndForever(sn1);
     $5=Snap.ValueAndForever(sn2);
     $6=Snap.ValueAndForever(sn3);
     $4!=null&&$4.$==1?$5!=null&&$5.$==1?$6!=null&&$6.$==1?$4.$0[1]&&$5.$0[1]&&$6.$0[1]?Snap.MarkForever(res,fn($4.$0[0],$5.$0[0],$6.$0[0])):Snap.MarkReady(res,fn($4.$0[0],$5.$0[0],$6.$0[0])):void 0:void 0:void 0;
    }
  }
  $1=sn1.s;
  $2=sn2.s;
  $3=sn3.s;
  return $1!=null&&$1.$==0?$2!=null&&$2.$==0?$3!=null&&$3.$==0?Snap.New({
   $:0,
   $0:fn($1.$0,$2.$0,$3.$0)
  }):Snap.Map3Opt1(fn,$1.$0,$2.$0,sn3):$3!=null&&$3.$==0?Snap.Map3Opt2(fn,$1.$0,$3.$0,sn2):Snap.Map3Opt3(fn,$1.$0,sn2,sn3):$2!=null&&$2.$==0?$3!=null&&$3.$==0?Snap.Map3Opt4(fn,$2.$0,$3.$0,sn1):Snap.Map3Opt5(fn,$2.$0,sn1,sn3):$3!=null&&$3.$==0?Snap.Map3Opt6(fn,$3.$0,sn1,sn2):(res=Snap.New({
   $:3,
   $0:[],
   $1:[]
  }),(Snap.When(sn1,cont,res),Snap.When(sn2,cont,res),Snap.When(sn3,cont,res),res));
 };
 Snap.Map3Opt6=function(fn,z,sn1,sn2)
 {
  return Snap.Map2(function($1,$2)
  {
   return fn($1,$2,z);
  },sn1,sn2);
 };
 Snap.Map3Opt5=function(fn,y,sn1,sn3)
 {
  return Snap.Map2(function($1,$2)
  {
   return fn($1,y,$2);
  },sn1,sn3);
 };
 Snap.Map3Opt4=function(fn,y,z,sn1)
 {
  return Snap.Map(function(x)
  {
   return fn(x,y,z);
  },sn1);
 };
 Snap.Map3Opt3=function(fn,x,sn2,sn3)
 {
  return Snap.Map2(function($1,$2)
  {
   return fn(x,$1,$2);
  },sn2,sn3);
 };
 Snap.Map3Opt2=function(fn,x,z,sn2)
 {
  return Snap.Map(function(y)
  {
   return fn(x,y,z);
  },sn2);
 };
 Snap.Map3Opt1=function(fn,x,y,sn3)
 {
  return Snap.Map(function(z)
  {
   return fn(x,y,z);
  },sn3);
 };
 Snap.Map2Unit=function(sn1,sn2)
 {
  var $1,$2,res;
  function cont()
  {
   var m,$3,$4;
   if(!(m=res.s,m!=null&&m.$==0||m!=null&&m.$==2&&true))
    {
     $3=Snap.ValueAndForever(sn1);
     $4=Snap.ValueAndForever(sn2);
     $3!=null&&$3.$==1?$4!=null&&$4.$==1?$3.$0[1]&&$4.$0[1]?Snap.MarkForever(res,null):Snap.MarkReady(res,null):void 0:void 0;
    }
  }
  $1=sn1.s;
  $2=sn2.s;
  return $1!=null&&$1.$==0?$2!=null&&$2.$==0?Snap.New({
   $:0,
   $0:null
  }):sn2:$2!=null&&$2.$==0?sn1:(res=Snap.New({
   $:3,
   $0:[],
   $1:[]
  }),(Snap.When(sn1,cont,res),Snap.When(sn2,cont,res),res));
 };
 Snap.Map2=function(fn,sn1,sn2)
 {
  var $1,$2,res;
  function cont(a)
  {
   var m,$3,$4;
   if(!(m=res.s,m!=null&&m.$==0||m!=null&&m.$==2&&true))
    {
     $3=Snap.ValueAndForever(sn1);
     $4=Snap.ValueAndForever(sn2);
     $3!=null&&$3.$==1?$4!=null&&$4.$==1?$3.$0[1]&&$4.$0[1]?Snap.MarkForever(res,fn($3.$0[0],$4.$0[0])):Snap.MarkReady(res,fn($3.$0[0],$4.$0[0])):void 0:void 0;
    }
  }
  $1=sn1.s;
  $2=sn2.s;
  return $1!=null&&$1.$==0?$2!=null&&$2.$==0?Snap.New({
   $:0,
   $0:fn($1.$0,$2.$0)
  }):Snap.Map2Opt1(fn,$1.$0,sn2):$2!=null&&$2.$==0?Snap.Map2Opt2(fn,$2.$0,sn1):(res=Snap.New({
   $:3,
   $0:[],
   $1:[]
  }),(Snap.When(sn1,cont,res),Snap.When(sn2,cont,res),res));
 };
 Snap.Map2Opt2=function(fn,y,sn1)
 {
  return Snap.Map(function(x)
  {
   return fn(x,y);
  },sn1);
 };
 Snap.Map2Opt1=function(fn,x,sn2)
 {
  return Snap.Map(function(y)
  {
   return fn(x,y);
  },sn2);
 };
 Snap.MapCachedBy=function(eq,prev,fn,sn)
 {
  return Snap.Map(function(x)
  {
   var m,$1,y;
   m=prev[0];
   return m!=null&&m.$==1&&(eq(x,m.$0[0])&&($1=[m.$0[0],m.$0[1]],true))?$1[1]:(y=fn(x),(prev[0]={
    $:1,
    $0:[x,y]
   },y));
  },sn);
 };
 Snap.Copy=function(sn)
 {
  var m,res,res$1;
  m=sn.s;
  return m==null?sn:m!=null&&m.$==2?(res=Snap.New({
   $:2,
   $0:m.$0,
   $1:[]
  }),(Snap.WhenObsolete(sn,res),res)):m!=null&&m.$==3?(res$1=Snap.New({
   $:3,
   $0:[],
   $1:[]
  }),(Snap.When(sn,function(v)
  {
   Snap.MarkDone(res$1,sn,v);
  },res$1),res$1)):sn;
 };
 Snap.Map=function(fn,sn)
 {
  var m,res;
  m=sn.s;
  return m!=null&&m.$==0?Snap.New({
   $:0,
   $0:fn(m.$0)
  }):(res=Snap.New({
   $:3,
   $0:[],
   $1:[]
  }),(Snap.When(sn,function(a)
  {
   Snap.MarkDone(res,sn,fn(a));
  },res),res));
 };
 Snap.Sequence=function(snaps)
 {
  var snaps$1,res,w;
  function cont(a)
  {
   var vs;
   if(w[0]===0)
    {
     vs=Arrays.map(function(s)
     {
      var m;
      m=s.s;
      return m!=null&&m.$==0?m.$0:m!=null&&m.$==2?m.$0:Operators.FailWith("value not found by View.Sequence");
     },snaps$1);
     Arrays.forall(function(a$1)
     {
      var $1;
      return($1=a$1.s,$1!=null&&$1.$==0)&&true;
     },snaps$1)?Snap.MarkForever(res,vs):Snap.MarkReady(res,vs);
    }
   else
    w[0]--;
  }
  snaps$1=Arrays.ofSeq(snaps);
  return snaps$1.length==0?Snap.New({
   $:0,
   $0:[]
  }):(res=Snap.New({
   $:3,
   $0:[],
   $1:[]
  }),(w=[Arrays.length(snaps$1)-1],(Arrays.iter(function(s)
  {
   Snap.When(s,cont,res);
  },snaps$1),res)));
 };
 Snap.CreateForeverAsync=function(a)
 {
  var o;
  o=Snap.New({
   $:3,
   $0:[],
   $1:[]
  });
  Async.StartTo(a,function(v)
  {
   Snap.MarkForever(o,v);
  });
  return o;
 };
 Snap.JoinInner=function(snap)
 {
  var res;
  res=Snap.New({
   $:3,
   $0:[],
   $1:[]
  });
  Snap.When(snap,function(x)
  {
   var y;
   y=x();
   Snap.When(y,function(v)
   {
    var $1,$2;
    if(($1=y.s,$1!=null&&$1.$==0)&&true&&(($2=snap.s,$2!=null&&$2.$==0)&&true))
     Snap.MarkForever(res,v);
    else
     Snap.MarkReady(res,v);
   },res);
   Snap.WhenObsolete(snap,y);
  },res);
  return res;
 };
 Snap.Join=function(snap)
 {
  var res;
  res=Snap.New({
   $:3,
   $0:[],
   $1:[]
  });
  Snap.When(snap,function(x)
  {
   var y;
   y=x();
   Snap.When(y,function(v)
   {
    var $1,$2;
    if(($1=y.s,$1!=null&&$1.$==0)&&true&&(($2=snap.s,$2!=null&&$2.$==0)&&true))
     Snap.MarkForever(res,v);
    else
     Snap.MarkReady(res,v);
   },res);
  },res);
  return res;
 };
 Snap.ValueAndForever=function(snap)
 {
  var m;
  m=snap.s;
  return m!=null&&m.$==0?{
   $:1,
   $0:[m.$0,true]
  }:m!=null&&m.$==2?{
   $:1,
   $0:[m.$0,false]
  }:null;
 };
 Snap.WhenObsoleteRun=function(snap,obs)
 {
  var m;
  m=snap.s;
  m==null?obs():m!=null&&m.$==2?m.$1.push(obs):m!=null&&m.$==3?m.$1.push(obs):void 0;
 };
 Snap.WhenObsolete=function(snap,obs)
 {
  var m;
  m=snap.s;
  m==null?Snap.Obsolete(obs):m!=null&&m.$==2?Snap.EnqueueSafe(m.$1,obs):m!=null&&m.$==3?Snap.EnqueueSafe(m.$1,obs):void 0;
 };
 Snap.WhenReady=function(snap,avail)
 {
  var $1,m;
  m=snap.s;
  switch(m!=null&&m.$==2?($1=m.$0,0):m==null?1:m!=null&&m.$==3?2:($1=m.$0,0))
  {
   case 0:
    avail($1);
    break;
   case 1:
    null;
    break;
   case 2:
    m.$0.push(avail);
    break;
  }
 };
 Snap.WhenRun=function(snap,avail,obs)
 {
  var m;
  m=snap.s;
  m==null?obs():m!=null&&m.$==2?(m.$1.push(obs),avail(m.$0)):m!=null&&m.$==3?(m.$0.push(avail),m.$1.push(obs)):avail(m.$0);
 };
 Snap.When=function(snap,avail,obs)
 {
  var m;
  m=snap.s;
  m==null?Snap.Obsolete(obs):m!=null&&m.$==2?(Snap.EnqueueSafe(m.$1,obs),avail(m.$0)):m!=null&&m.$==3?(m.$0.push(avail),Snap.EnqueueSafe(m.$1,obs)):avail(m.$0);
 };
 Snap.EnqueueSafe=function(q,x)
 {
  var qcopy,i,$1,o;
  q.push(x);
  if(q.length%20===0)
   {
    qcopy=q.slice(0);
    Queue.Clear(q);
    for(i=0,$1=Arrays.length(qcopy)-1;i<=$1;i++){
     o=Arrays.get(qcopy,i);
     typeof o=="object"?function(sn)
     {
      if(sn.s)
       q.push(sn);
     }(o):function(f)
     {
      q.push(f);
     }(o);
    }
   }
  else
   void 0;
 };
 Snap.MarkDone=function(res,sn,v)
 {
  var $1;
  if(($1=sn.s,$1!=null&&$1.$==0)&&true)
   Snap.MarkForever(res,v);
  else
   Snap.MarkReady(res,v);
 };
 Snap.MarkReady=function(sn,v)
 {
  var m,qa,i,$1;
  m=sn.s;
  if(m!=null&&m.$==3)
   {
    sn.s={
     $:2,
     $0:v,
     $1:m.$1
    };
    qa=m.$0;
    for(i=0,$1=Arrays.length(qa)-1;i<=$1;i++)(Arrays.get(qa,i))(v);
   }
  else
   void 0;
 };
 Snap.MarkForever=function(sn,v)
 {
  var m,qa,i,$1;
  m=sn.s;
  if(m!=null&&m.$==3)
   {
    sn.s={
     $:0,
     $0:v
    };
    qa=m.$0;
    for(i=0,$1=Arrays.length(qa)-1;i<=$1;i++)(Arrays.get(qa,i))(v);
   }
  else
   void 0;
 };
 Var=Next.Var=Runtime.Class({
  RId:function()
  {
   return"uinref"+Global.String(this.i);
  },
  RView:function()
  {
   return this.v;
  },
  RUpdM:function(f)
  {
   var m;
   m=f(this.c);
   m!=null&&m.$==1?Var.Set(this,m.$0):void 0;
  },
  RUpd:function(f)
  {
   Var.Update(this,f);
  },
  set_RVal:function(v)
  {
   Var.Set(this,v);
  },
  RVal:function()
  {
   return this.c;
  },
  RSet:function(v)
  {
   Var.Set(this,v);
  },
  RGet:function()
  {
   return this.c;
  }
 },null,Var);
 Var.New=function(Const,Current,Snap$1,Id,VarView)
 {
  return new Var({
   o:Const,
   c:Current,
   s:Snap$1,
   i:Id,
   v:VarView
  });
 };
 Var.Lens=function(iref,get,update)
 {
  var id,view;
  id=Fresh.Id();
  view=View.Map(get,iref.RView());
  return{
   RGet:function()
   {
    return get(iref.RGet());
   },
   RSet:function(v)
   {
    return iref.RUpd(function(t)
    {
     return update(t,v);
    });
   },
   RVal:function()
   {
    return get(iref.RGet());
   },
   set_RVal:function(v)
   {
    return iref.RUpd(function(t)
    {
     return update(t,v);
    });
   },
   RUpd:function(f)
   {
    return iref.RUpd(function(t)
    {
     return update(t,f(get(t)));
    });
   },
   RUpdM:function(f)
   {
    return iref.RUpdM(function(t)
    {
     var x;
     x=f(get(t));
     return x==null?null:{
      $:1,
      $0:update(t,x.$0)
     };
    });
   },
   RView:function()
   {
    return view;
   },
   RId:function()
   {
    return id;
   }
  };
 };
 Var.Update=function(_var,fn)
 {
  Var.Set(_var,fn(_var.c));
 };
 Var.SetFinal=function(_var,value)
 {
  if(_var.o)
   (function($1)
   {
    return $1("WebSharper UI.Next: invalid attempt to change value of a Var after calling SetFinal");
   }(function(s)
   {
    console.log(s);
   }));
  else
   {
    Snap.Obsolete(_var.s);
    _var.o=true;
    _var.c=value;
    _var.s=Snap.New({
     $:0,
     $0:value
    });
   }
 };
 Var.Set=function(_var,value)
 {
  if(_var.o)
   (function($1)
   {
    return $1("WebSharper UI.Next: invalid attempt to change value of a Var after calling SetFinal");
   }(function(s)
   {
    console.log(s);
   }));
  else
   {
    Snap.Obsolete(_var.s);
    _var.c=value;
    _var.s=Snap.New({
     $:2,
     $0:value,
     $1:[]
    });
   }
 };
 Var.Create=function()
 {
  var _var;
  _var=null;
  _var=Var.New(false,null,Snap.New({
   $:2,
   $0:null,
   $1:[]
  }),Fresh.Int(),function()
  {
   return _var.s;
  });
  return _var;
 };
 Var.CreateLogged=function(name,v)
 {
  var res;
  !window.UINVars?window.UINVars=[]:void 0;
  res=Var.Create$1(v);
  window.UINVars.push([name,res]);
  return res;
 };
 Var.Create$1=function(v)
 {
  var _var;
  _var=null;
  _var=Var.New(false,v,Snap.New({
   $:2,
   $0:v,
   $1:[]
  }),Fresh.Int(),function()
  {
   return _var.s;
  });
  return _var;
 };
 Updates=Next.Updates=Runtime.Class({
  set_Value:function(v)
  {
   var sn;
   sn=this.s;
   !(sn===null)?Snap.Obsolete(sn):void 0;
   this.c=v;
  }
 },null,Updates);
 Updates.Create=function(v)
 {
  var _var;
  _var=null;
  _var=Updates.New(v,null,function()
  {
   var c;
   c=_var.s;
   return c===null?(c=Snap.Copy(_var.c()),_var.s=c,Snap.WhenObsoleteRun(c,function()
   {
    _var.s=null;
   }),c):c;
  });
  return _var;
 };
 Updates.New=function(Current,Snap$1,VarView)
 {
  return new Updates({
   c:Current,
   s:Snap$1,
   v:VarView
  });
 };
 View.get_Do=function()
 {
  return ViewBuilder.B;
 };
 View.Apply=function(fn,view)
 {
  return View.Map2(function(f,x)
  {
   return f(x);
  },fn,view);
 };
 View.AsyncAwait=function(filter,view)
 {
  function a(ok)
  {
   var r,remove;
   function r$1()
   {
    return View.RemovableSink(function(value)
    {
     if(filter(value))
      {
       (Lazy.Force(r))();
       ok(value);
      }
    },view);
   }
   r=Lazy.Create(r$1);
   remove=Lazy.Force(r);
  }
  return Concurrency.FromContinuations(function($1,$2,$3)
  {
   return a.apply(null,[$1,$2,$3]);
  });
 };
 View.RemovableSink=function(act,a)
 {
  var cont;
  function loop()
  {
   Snap.WhenRun(a(),function(x)
   {
    if(cont[0])
     act(x);
   },function()
   {
    if(cont[0])
     Concurrency.scheduler().Fork(loop);
   });
  }
  cont=[true];
  Concurrency.scheduler().Fork(loop);
  return function()
  {
   cont[0]=false;
  };
 };
 View.Sink=function(act,a)
 {
  function loop()
  {
   Snap.WhenRun(a(),act,function()
   {
    Concurrency.scheduler().Fork(loop);
   });
  }
  Concurrency.scheduler().Fork(loop);
 };
 View.TryFinally=function(f,a)
 {
  return View.CreateLazy(function()
  {
   try
   {
    return a();
   }
   finally
   {
    f();
   }
  });
 };
 View.TryWith=function(f,a)
 {
  return View.CreateLazy(function()
  {
   try
   {
    return a();
   }
   catch(exn)
   {
    return(f(exn))();
   }
  });
 };
 View.ConstAsync=function(a)
 {
  var o;
  o=Snap.CreateForeverAsync(a);
  return function()
  {
   return o;
  };
 };
 View.Const=function(x)
 {
  var o;
  o=Snap.New({
   $:0,
   $0:x
  });
  return function()
  {
   return o;
  };
 };
 View.Sequence=function(views)
 {
  return View.CreateLazy(function()
  {
   return Snap.Sequence(Seq.map(function(a)
   {
    return a();
   },views));
  });
 };
 View.UpdateWhile=function(def,v1,v2)
 {
  var value;
  value=[def];
  return View.BindInner(function(pred)
  {
   return pred?View.Map(function(v)
   {
    value[0]=v;
    return v;
   },v2):View.Const(value[0]);
  },v1);
 };
 View.BindInner=function(fn,view)
 {
  return View.JoinInner(View.Map(fn,view));
 };
 View.JoinInner=function(a)
 {
  return View.CreateLazy(function()
  {
   return Snap.JoinInner(a());
  });
 };
 View.Bind=function(fn,view)
 {
  return View.Join(View.Map(fn,view));
 };
 View.Join=function(a)
 {
  return View.CreateLazy(function()
  {
   return Snap.Join(a());
  });
 };
 View.MapSeqCachedView=function(conv,view)
 {
  function a(a$1)
  {
   return conv;
  }
  return View.MapSeqCachedViewBy(Global.id,function($1,$2)
  {
   return(a($1))($2);
  },view);
 };
 View.MapSeqCachedViewBy=function(key,conv,view)
 {
  var state;
  state=[new Dictionary.New$5()];
  return View.Map(function(xs)
  {
   var prevState,newState,result;
   prevState=state[0];
   newState=new Dictionary.New$5();
   result=Array.mapInPlace(function(x)
   {
    var k,node,n;
    k=key(x);
    node=prevState.ContainsKey(k)?(n=prevState.get_Item(k),(Var.Set(n.r,x),n)):View.ConvertSeqNode(function(v)
    {
     return conv(k,v);
    },x);
    newState.set_Item(k,node);
    return node.e;
   },Arrays.ofSeq(xs));
   state[0]=newState;
   return result;
  },view);
 };
 View.ConvertSeqNode=function(conv,value)
 {
  var _var,view;
  _var=Var.Create$1(value);
  view=_var.v;
  return{
   e:conv(view),
   r:_var,
   w:view
  };
 };
 View.MapSeqCached=function(conv,view)
 {
  return View.MapSeqCachedBy(Global.id,conv,view);
 };
 View.MapSeqCachedBy=function(key,conv,view)
 {
  var state;
  state=[new Dictionary.New$5()];
  return View.Map(function(xs)
  {
   var prevState,newState,result;
   prevState=state[0];
   newState=new Dictionary.New$5();
   result=Array.mapInPlace(function(x)
   {
    var k,res;
    k=key(x);
    res=prevState.ContainsKey(k)?prevState.get_Item(k):conv(x);
    newState.set_Item(k,res);
    return res;
   },Arrays.ofSeq(xs));
   state[0]=newState;
   return result;
  },view);
 };
 View.SnapshotOn=function(def,a,a$1)
 {
  var sInit;
  sInit=Snap.New({
   $:2,
   $0:def,
   $1:[]
  });
  return View.CreateLazy(function()
  {
   return sInit.s==null&&true?Snap.SnapshotOn(a(),a$1()):(Snap.WhenObsolete(a(),sInit),sInit);
  });
 };
 View.GetAsync=function(v)
 {
  return Concurrency.FromContinuations(function(ok)
  {
   return View.Get(ok,v);
  });
 };
 View.Get=function(f,a)
 {
  var ok;
  function obs()
  {
   Snap.WhenRun(a(),function(v)
   {
    if(!ok[0])
     {
      ok[0]=true;
      f(v);
     }
   },function()
   {
    if(!ok[0])
     obs();
   });
  }
  ok=[false];
  obs();
 };
 View.MapAsync2=function(fn,v1,v2)
 {
  return View.MapAsync(Global.id,View.Map2(fn,v1,v2));
 };
 View.MapAsync=function(fn,a)
 {
  return View.CreateLazy(function()
  {
   return Snap.MapAsync(fn,a());
  });
 };
 View.Map3=function(fn,a,a$1,a$2)
 {
  return View.CreateLazy(function()
  {
   return Snap.Map3(fn,a(),a$1(),a$2());
  });
 };
 View.Map2Unit=function(a,a$1)
 {
  return View.CreateLazy(function()
  {
   return Snap.Map2Unit(a(),a$1());
  });
 };
 View.Map2=function(fn,a,a$1)
 {
  return View.CreateLazy(function()
  {
   return Snap.Map2(fn,a(),a$1());
  });
 };
 View.MapCached=function(fn,v)
 {
  return View.MapCachedBy(Unchecked.Equals,fn,v);
 };
 View.MapCachedBy=function(eq,fn,a)
 {
  var vref;
  vref=[null];
  return View.CreateLazy(function()
  {
   return Snap.MapCachedBy(eq,vref,fn,a());
  });
 };
 View.Map=function(fn,a)
 {
  return View.CreateLazy(function()
  {
   return Snap.Map(fn,a());
  });
 };
 View.CreateLazy=function(observe)
 {
  var lv;
  lv={
   c:null,
   o:observe
  };
  return function()
  {
   var c,$1;
   c=lv.c;
   return c===null?(c=lv.o(),lv.c=c,($1=c.s,$1!=null&&$1.$==0)&&true?lv.o=null:Snap.WhenObsoleteRun(c,function()
   {
    lv.c=null;
   }),c):c;
  };
 };
 ViewBuilder.B={
  $:0
 };
 Submitter=Next.Submitter=Runtime.Class({
  Trigger:function()
  {
   Var.Set(this["var"],null);
  }
 },WebSharper.Obj,Submitter);
 Submitter.New=Runtime.Ctor(function(input,init)
 {
  this.input=input;
  this["var"]=Var.Create();
  this.view=View.SnapshotOn(init,this["var"].v,this.input);
 },Submitter);
 Submitter.CreateOption=function(input)
 {
  return new Submitter.New(View.Map(function(a)
  {
   return{
    $:1,
    $0:a
   };
  },input),null);
 };
 Key.Fresh=function()
 {
  return{
   $:0,
   $0:Fresh.Int()
  };
 };
 Model=Next.Model=Runtime.Class({},WebSharper.Obj,Model);
 Model.New=Runtime.Ctor(function(proj,init)
 {
  var _var;
  _var=Var.Create$1(init);
  Model.New$1.call(this,_var,View.Map(proj,_var.v));
 },Model);
 Model.New$1=Runtime.Ctor(function(_var,view)
 {
  this["var"]=_var;
  this.view=view;
 },Model);
 Model.Update=function(update,m)
 {
  Var.Update(m["var"],function(x)
  {
   update(x);
   return x;
  });
 };
 Model.Create=function(proj,init)
 {
  return new Model.New(proj,init);
 };
 Serializer.Default=function()
 {
  SC$5.$cctor();
  return SC$5.Default;
 };
 ArrayStorage=Storage.ArrayStorage=Runtime.Class({
  SSet:function(coll)
  {
   return Arrays.ofSeq(coll);
  },
  SSetAt:function(idx,elem,arr)
  {
   Arrays.set(arr,idx,elem);
   return arr;
  },
  SRemoveIf:function(pred,arr)
  {
   return Arrays.filter(function(i)
   {
    return!pred(i);
   },arr);
  },
  SInit:function()
  {
   return this.init;
  },
  SPrependMany:function(is,arr)
  {
   var ps;
   ps=Array.ofSeqNonCopying(is);
   arr.unshift.apply(arr,ps);
   return arr;
  },
  SPrepend:function(i,arr)
  {
   arr.unshift(i);
   return arr;
  },
  SAppendMany:function(is,arr)
  {
   var ps;
   ps=Array.ofSeqNonCopying(is);
   arr.push.apply(arr,ps);
   return arr;
  },
  SAppend:function(i,arr)
  {
   arr.push(i);
   return arr;
  }
 },WebSharper.Obj,ArrayStorage);
 ArrayStorage.New=Runtime.Ctor(function(init)
 {
  this.init=init;
 },ArrayStorage);
 LocalStorageBackend=Storage.LocalStorageBackend=Runtime.Class({
  clear:function()
  {
   this.storage.removeItem(this.id);
  },
  set:function(arr)
  {
   this.storage.setItem(this.id,JSON.stringify(Arrays.map(this.serializer.Encode,arr)));
   return arr;
  },
  SSet:function(coll)
  {
   return this.set(Arrays.ofSeq(coll));
  },
  SSetAt:function(idx,elem,arr)
  {
   Arrays.set(arr,idx,elem);
   return this.set(arr);
  },
  SRemoveIf:function(pred,arr)
  {
   return this.set(Arrays.filter(function(i)
   {
    return!pred(i);
   },arr));
  },
  SInit:function()
  {
   var item;
   item=this.storage.getItem(this.id);
   if(item===null)
    return[];
   else
    try
    {
     return Arrays.map(this.serializer.Decode,JSON.parse(item));
    }
    catch(m)
    {
     return[];
    }
  },
  SPrependMany:function(is,arr)
  {
   var ps;
   ps=Array.ofSeqNonCopying(is);
   arr.unshift.apply(arr,ps);
   return this.set(arr);
  },
  SPrepend:function(i,arr)
  {
   arr.unshift(i);
   return this.set(arr);
  },
  SAppendMany:function(is,arr)
  {
   var ps;
   ps=Array.ofSeqNonCopying(is);
   arr.push.apply(arr,ps);
   return this.set(arr);
  },
  SAppend:function(i,arr)
  {
   arr.push(i);
   return this.set(arr);
  }
 },WebSharper.Obj,LocalStorageBackend);
 LocalStorageBackend.New=Runtime.Ctor(function(id,serializer)
 {
  this.id=id;
  this.serializer=serializer;
  this.storage=Global.localStorage;
 },LocalStorageBackend);
 Storage.LocalStorage=function(id,serializer)
 {
  return new LocalStorageBackend.New(id,serializer);
 };
 Storage.InMemory=function(init)
 {
  return new ArrayStorage.New(init);
 };
 ListModel=Next.ListModel=Runtime.Class({
  Wrap:function(extract,wrap,update)
  {
   return ListModel.Wrap(this,extract,wrap,update);
  },
  Lens:function(key)
  {
   return this.LensInto(Global.id,function(a,x)
   {
    return x;
   },key);
  },
  LensInto:function(get,update,key)
  {
   var $this,id,view;
   $this=this;
   id=Fresh.Id();
   view=View.Map(get,this.FindByKeyAsView(key));
   return{
    RGet:function()
    {
     return get($this.FindByKey(key));
    },
    RSet:function(v)
    {
     return $this.UpdateBy(function(i)
     {
      return{
       $:1,
       $0:update(i,v)
      };
     },key);
    },
    RVal:function()
    {
     return this.RGet();
    },
    set_RVal:function(v)
    {
     return this.RSet(v);
    },
    RUpd:function(f)
    {
     return $this.UpdateBy(function(i)
     {
      return{
       $:1,
       $0:update(i,f(get(i)))
      };
     },key);
    },
    RUpdM:function(f)
    {
     return $this.UpdateBy(function(i)
     {
      var x;
      x=f(get(i));
      return x==null?null:{
       $:1,
       $0:update(i,x.$0)
      };
     },key);
    },
    RView:function()
    {
     return view;
    },
    RId:function()
    {
     return id;
    }
   };
  },
  get_LengthAsView:function()
  {
   return View.Map(Arrays.length,this["var"].RView());
  },
  get_Length:function()
  {
   return Arrays.length(this["var"].RVal());
  },
  Clear:function()
  {
   this["var"].set_RVal(this.storage.SSet([]));
   this.ObsoleteAll();
  },
  UpdateBy:function(fn,key)
  {
   var $this,v,m,index,m$1;
   $this=this;
   v=this["var"].RVal();
   m=Arrays.tryFindIndex(function(it)
   {
    return Unchecked.Equals($this.key(it),key);
   },v);
   m!=null&&m.$==1?(index=m.$0,m$1=fn(Arrays.get(v,index)),m$1!=null&&m$1.$==1?(this["var"].set_RVal(this.storage.SSetAt(index,m$1.$0,v)),this.ObsoleteKey(key)):void 0):void 0;
  },
  UpdateAll:function(fn)
  {
   var $this;
   $this=this;
   this["var"].RUpd(function(a)
   {
    Arrays.iteri(function(i,x)
    {
     var o;
     o=fn(x);
     return o==null?null:Arrays.set(a,i,o.$0);
    },a);
    return $this.storage.SSet(a);
   });
   this.ObsoleteAll();
  },
  FindByKeyAsView:function(key)
  {
   return View.Map(function(o)
   {
    return o.$0;
   },this.TryFindByKeyAsView(key));
  },
  TryFindByKeyAsView:function(key)
  {
   var $this;
   $this=this;
   return function()
   {
    var m,o,sn;
    m=(o=null,[$this.it.TryGetValue(key,{
     get:function()
     {
      return o;
     },
     set:function(v)
     {
      o=v;
     }
    }),o]);
    return m[0]?m[1]:(sn=Snap.New({
     $:2,
     $0:$this.TryFindByKey(key),
     $1:[]
    }),($this.it.Add(key,sn),sn));
   };
  },
  TryFindByKey:function(key)
  {
   var $this;
   $this=this;
   return Arrays.tryFind(function(it)
   {
    return Unchecked.Equals($this.key(it),key);
   },this["var"].RVal());
  },
  FindByKey:function(key)
  {
   var $this;
   $this=this;
   return Arrays.find(function(it)
   {
    return Unchecked.Equals($this.key(it),key);
   },this["var"].RVal());
  },
  TryFindAsView:function(pred)
  {
   return View.Map(function(a)
   {
    return Arrays.tryFind(pred,a);
   },this["var"].RView());
  },
  FindAsView:function(pred)
  {
   return View.Map(function(a)
   {
    return Arrays.find(pred,a);
   },this["var"].RView());
  },
  TryFind:function(pred)
  {
   return Arrays.tryFind(pred,this["var"].RVal());
  },
  Find:function(pred)
  {
   return Arrays.find(pred,this["var"].RVal());
  },
  ContainsKeyAsView:function(key)
  {
   var $this;
   function p(it)
   {
    return Unchecked.Equals($this.key(it),key);
   }
   $this=this;
   return View.Map(function(a)
   {
    return Arrays.exists(p,a);
   },this["var"].RView());
  },
  ContainsKey:function(key)
  {
   var $this;
   $this=this;
   return Arrays.exists(function(it)
   {
    return Unchecked.Equals($this.key(it),key);
   },this["var"].RVal());
  },
  Set:function(lst)
  {
   this["var"].set_RVal(this.storage.SSet(lst));
   this.ObsoleteAll();
  },
  Iter:function(fn)
  {
   Arrays.iter(fn,this["var"].RVal());
  },
  RemoveByKey:function(key)
  {
   var $this;
   $this=this;
   this["var"].set_RVal(this.storage.SRemoveIf(function(i)
   {
    return Unchecked.Equals($this.key(i),key);
   },this["var"].RVal()));
   this.ObsoleteKey(key);
  },
  RemoveBy:function(f)
  {
   var a,i,$1,v;
   a=this["var"].RVal();
   for(i=0,$1=a.length-1;i<=$1;i++){
    v=Arrays.get(a,i);
    f(v)?this.ObsoleteKey(this.key(v)):void 0;
   }
   this["var"].set_RVal(this.storage.SRemoveIf(f,this["var"].RVal()));
  },
  Remove:function(item)
  {
   var $this,v,keyFn,k;
   $this=this;
   v=this["var"].RVal();
   ListModels.Contains($this.key,item,v)?(keyFn=$this.key,k=keyFn(item),this["var"].set_RVal(this.storage.SRemoveIf(function(i)
   {
    return Unchecked.Equals(keyFn(i),k);
   },v)),this.ObsoleteKey(k)):void 0;
  },
  PrependMany:function(items)
  {
   var $this,toPrepend;
   function f(v,item)
   {
    var t,m;
    t=$this.key(item);
    $this.ObsoleteKey(t);
    m=Arrays.tryFindIndex(function(it)
    {
     return Unchecked.Equals($this.key(it),t);
    },v);
    return m==null?(toPrepend.Add(item),v):$this.storage.SSetAt(m.$0,item,v);
   }
   $this=this;
   toPrepend=new List$1.New$2();
   this["var"].set_RVal(this.storage.SPrependMany(toPrepend,(((Runtime.Curried3(Seq.fold))(f))(this["var"].RVal()))(items)));
  },
  Prepend:function(item)
  {
   var $this,v,t,m;
   $this=this;
   v=this["var"].RVal();
   t=this.key(item);
   m=Arrays.tryFindIndex(function(it)
   {
    return Unchecked.Equals($this.key(it),t);
   },v);
   m!=null&&m.$==1?this["var"].set_RVal(this.storage.SSetAt(m.$0,item,v)):this["var"].set_RVal(this.storage.SPrepend(item,v));
   this.ObsoleteKey(t);
  },
  AppendMany:function(items)
  {
   var $this,toAppend;
   function f(v,item)
   {
    var t,m;
    t=$this.key(item);
    $this.ObsoleteKey(t);
    m=Arrays.tryFindIndex(function(it)
    {
     return Unchecked.Equals($this.key(it),t);
    },v);
    return m==null?(toAppend.Add(item),v):$this.storage.SSetAt(m.$0,item,v);
   }
   $this=this;
   toAppend=new List$1.New$2();
   this["var"].set_RVal(this.storage.SAppendMany(toAppend,(((Runtime.Curried3(Seq.fold))(f))(this["var"].RVal()))(items)));
  },
  Append:function(item)
  {
   var $this,v,t,m;
   $this=this;
   v=this["var"].RVal();
   t=this.key(item);
   m=Arrays.tryFindIndex(function(it)
   {
    return Unchecked.Equals($this.key(it),t);
   },v);
   m!=null&&m.$==1?this["var"].set_RVal(this.storage.SSetAt(m.$0,item,v)):this["var"].set_RVal(this.storage.SAppend(item,v));
   this.ObsoleteKey(t);
  },
  ObsoleteAll:function()
  {
   Seq.iter(function(ksn)
   {
    Snap.Obsolete(ksn.V);
   },this.it);
   this.it.Clear();
  },
  ObsoleteKey:function(key)
  {
   var m,o;
   m=(o=null,[this.it.TryGetValue(key,{
    get:function()
    {
     return o;
    },
    set:function(v)
    {
     o=v;
    }
   }),o]);
   m[0]?(Snap.Obsolete(m[1]),this.it.Remove(key)):void 0;
  },
  GetEnumerator0:function()
  {
   return Enumerator.Get0(this["var"].RVal());
  },
  GetEnumerator:function()
  {
   return Enumerator.Get(this["var"].RVal());
  }
 },WebSharper.Obj,ListModel);
 ListModel.New=Runtime.Ctor(function(key,storage)
 {
  ListModel.New$3.call(this,key,Var.Create$1(Arrays.ofSeq(Seq.distinctBy(key,storage.SInit()))),storage);
 },ListModel);
 ListModel.New$1=Runtime.Ctor(function(key)
 {
  ListModel.New$2.call(this,key,[]);
 },ListModel);
 ListModel.New$2=Runtime.Ctor(function(key,init)
 {
  var init$1;
  init$1=Arrays.ofSeq(init);
  ListModel.New$3.call(this,key,Var.Create$1(init$1),Storage.InMemory(init$1));
 },ListModel);
 ListModel.New$3=Runtime.Ctor(function(key,_var,storage)
 {
  this.key=key;
  this["var"]=_var;
  this.storage=storage;
  this.v=View.Map(function(x)
  {
   return x.slice();
  },this["var"].RView());
  this.it=new Dictionary.New$5();
 },ListModel);
 ListModels.Contains=function(keyFn,item,xs)
 {
  var t;
  t=keyFn(item);
  return Arrays.exists(function(it)
  {
   return Unchecked.Equals(keyFn(it),t);
  },xs);
 };
 ListModel.Wrap=function(underlying,extract,createItem,updateItem)
 {
  var state,init;
  state=[new Dictionary.New$5()];
  init=Arrays.map(function(u)
  {
   var t;
   t=createItem(u);
   state[0].set_Item(underlying.key(u),t);
   return t;
  },underlying["var"].RVal());
  return new ListModel.New$3(function(d)
  {
   return underlying.key(extract(d));
  },Var.Lens(underlying["var"],function(us)
  {
   var newState,ts;
   newState=new Dictionary.New$5();
   ts=Arrays.map(function(u)
   {
    var k,t;
    k=underlying.key(u);
    t=state[0].ContainsKey(k)?updateItem(state[0].get_Item(k),u):createItem(u);
    newState.set_Item(k,t);
    return t;
   },us);
   state[0]=newState;
   return ts;
  },function(us,ts)
  {
   var newState,us$1;
   newState=new Dictionary.New$5();
   us$1=Arrays.map(function(t)
   {
    var u;
    u=extract(t);
    newState.set_Item(underlying.key(u),t);
    return u;
   },ts);
   state[0]=newState;
   return us$1;
  }),Storage.InMemory(init));
 };
 ListModel.FromSeq=function(init)
 {
  return ListModel.Create(Global.id,init);
 };
 ListModel.Create=function(key,init)
 {
  return ListModel.CreateWithStorage(key,Storage.InMemory(Arrays.ofSeq(init)));
 };
 ListModel.CreateWithStorage=function(key,storage)
 {
  return new ListModel.New(key,storage);
 };
 SC$5.$cctor=function()
 {
  SC$5.$cctor=Global.ignore;
  SC$5.Default={
   Encode:Global.id,
   Decode:Global.id
  };
 };
 ReactiveExtensions=Next.ReactiveExtensions=Runtime.Class({},WebSharper.Obj,ReactiveExtensions);
 ReactiveExtensions.New=Runtime.Ctor(function()
 {
 },ReactiveExtensions);
 DoubleInterpolation=Next.DoubleInterpolation=Runtime.Class({
  Interpolate:function(t,x,y)
  {
   return x+t*(y-x);
  }
 },null,DoubleInterpolation);
 DoubleInterpolation.DoubleInterpolation=new DoubleInterpolation({
  $:0
 });
 Interpolation.get_Double=function()
 {
  return DoubleInterpolation.DoubleInterpolation;
 };
 Easing=Next.Easing=Runtime.Class({
  TransformTime:function(t)
  {
   return this.transformTime(t);
  }
 },WebSharper.Obj,Easing);
 Easing.get_CubicInOut=function()
 {
  return Easings.CubicInOut();
 };
 Easing.Custom=function(f)
 {
  return new Easing.New(f);
 };
 Easing.New=Runtime.Ctor(function(transformTime)
 {
  this.transformTime=transformTime;
 },Easing);
 Easings.CubicInOut=function()
 {
  SC$6.$cctor();
  return SC$6.CubicInOut;
 };
 An.get_Empty=function()
 {
  return{
   $:0,
   $0:AppendList.Empty()
  };
 };
 An.WhenDone=function(f,main)
 {
  return An.Append({
   $:0,
   $0:AppendList.Single({
    $:0,
    $0:f
   })
  },main);
 };
 An.Run=function(k,anim)
 {
  var dur;
  function a(ok)
  {
   function loop(start,now)
   {
    var t;
    t=now-start;
    anim.Compute(t);
    k();
    return t<=dur?void Global.requestAnimationFrame(function(t$1)
    {
     loop(start,t$1);
    }):ok();
   }
   Global.requestAnimationFrame(function(t)
   {
    loop(t,t);
   });
  }
  dur=anim.Duration;
  return dur===0?Concurrency.Zero():Concurrency.FromContinuations(function($1,$2,$3)
  {
   return a.apply(null,[$1,$2,$3]);
  });
 };
 An.Play=function(anim)
 {
  var b;
  b=null;
  return Concurrency.Delay(function()
  {
   return Concurrency.Bind(An.Run(Global.ignore,Anims.Actions(anim)),function()
   {
    Anims.Finalize(anim);
    return Concurrency.Return(null);
   });
  });
 };
 An.Pack=function(anim)
 {
  return{
   $:0,
   $0:AppendList.Single({
    $:1,
    $0:anim
   })
  };
 };
 An.Map=function(f,anim)
 {
  var f$1;
  return Anims.Def(anim.Duration,(f$1=anim.Compute,function(x)
  {
   return f(f$1(x));
  }));
 };
 An.Delayed=function(inter,easing,dur,delay,x,y)
 {
  return{
   Compute:function(t)
   {
    return t<=delay?x:inter.Interpolate(easing.TransformTime((t-delay)/dur),x,y);
   },
   Duration:dur+delay
  };
 };
 An.Simple=function(inter,easing,dur,x,y)
 {
  return{
   Compute:function(t)
   {
    return inter.Interpolate(easing.TransformTime(t/dur),x,y);
   },
   Duration:dur
  };
 };
 An.Const=function(v)
 {
  return Anims.Const(v);
 };
 An.Concat=function(xs)
 {
  return{
   $:0,
   $0:AppendList.Concat(Seq.map(Anims.List,xs))
  };
 };
 An.Append=function(a,a$1)
 {
  return{
   $:0,
   $0:AppendList.Append(a.$0,a$1.$0)
  };
 };
 An.set_UseAnimations=function(v)
 {
  Anims.set_UseAnimations(v);
 };
 An.get_UseAnimations=function()
 {
  return Anims.UseAnimations();
 };
 Anims.UseAnimations=function()
 {
  SC$6.$cctor();
  return SC$6.UseAnimations;
 };
 Anims.set_UseAnimations=function($1)
 {
  SC$6.$cctor();
  SC$6.UseAnimations=$1;
 };
 Anims.Actions=function(a)
 {
  return Anims.ConcatActions(Arrays.choose(function(a$1)
  {
   return a$1.$==1?{
    $:1,
    $0:a$1.$0
   }:null;
  },AppendList.ToArray(a.$0)));
 };
 Anims.ConcatActions=function(xs)
 {
  var xs$1,m,dur,xs$2;
  xs$1=Array.ofSeqNonCopying(xs);
  m=Arrays.length(xs$1);
  return m===0?Anims.Const():m===1?Arrays.get(xs$1,0):(dur=Seq.max(Seq.map(function(anim)
  {
   return anim.Duration;
  },xs$1)),(xs$2=Arrays.map(function(a)
  {
   return Anims.Prolong(dur,a);
  },xs$1),Anims.Def(dur,function(t)
  {
   Arrays.iter(function(anim)
   {
    anim.Compute(t);
   },xs$2);
  })));
 };
 Anims.Prolong=function(nextDuration,anim)
 {
  var comp,dur,last;
  comp=anim.Compute;
  dur=anim.Duration;
  last=Lazy.Create(function()
  {
   return anim.Compute(anim.Duration);
  });
  return{
   Compute:function(t)
   {
    return t>=dur?last.f():comp(t);
   },
   Duration:nextDuration
  };
 };
 Anims.Const=function(v)
 {
  return Anims.Def(0,function()
  {
   return v;
  });
 };
 Anims.Def=function(d,f)
 {
  return{
   Compute:f,
   Duration:d
  };
 };
 Anims.Finalize=function(a)
 {
  Arrays.iter(function(a$1)
  {
   if(a$1.$==0)
    a$1.$0();
  },AppendList.ToArray(a.$0));
 };
 Anims.List=function(a)
 {
  return a.$0;
 };
 Trans=Next.Trans=Runtime.Class({
  Copy:function(change,enter,exit,flags)
  {
   var $this,ch,d,d$1,d$2;
   function d$3(a,a$1)
   {
    return $this.TChange(a,a$1);
   }
   $this=this;
   ch=change==null?function($1)
   {
    return function($2)
    {
     return d$3($1,$2);
    };
   }:change.$0;
   return new Trans.New$3(function(d$4,d$5)
   {
    return(ch(d$4))(d$5);
   },(d=this.get_TEnter(),enter==null?d:enter.$0),(d$1=this.get_TExit(),exit==null?d$1:exit.$0),(d$2=this.get_TFlags(),flags==null?d$2:flags.$0));
  },
  get_TFlags:function()
  {
   return this.flags;
  },
  get_TExit:function()
  {
   return this.exit;
  },
  get_TEnter:function()
  {
   return this.enter;
  },
  TChange:function(x,y)
  {
   return this.change(x,y);
  }
 },WebSharper.Obj,Trans);
 Trans.New=Runtime.Ctor(function(ch,enter,exit)
 {
  Trans.New$3.call(this,ch,Unchecked.Equals(enter,null)?An.Const:enter,Unchecked.Equals(exit,null)?An.Const:exit,1|(Unchecked.Equals(enter,null)?0:2)|(Unchecked.Equals(exit,null)?0:4));
 },Trans);
 Trans.New$1=Runtime.Ctor(function(ch)
 {
  Trans.New$3.call(this,ch,An.Const,An.Const,1);
 },Trans);
 Trans.New$2=Runtime.Ctor(function()
 {
  Trans.New$3.call(this,function(x,y)
  {
   return An.Const(y);
  },An.Const,An.Const,0);
 },Trans);
 Trans.New$3=Runtime.Ctor(function(change,enter,exit,flags)
 {
  this.change=change;
  this.enter=enter;
  this.exit=exit;
  this.flags=flags;
 },Trans);
 Trans.Exit=function(f,tr)
 {
  return tr.Copy(null,null,{
   $:1,
   $0:f
  },{
   $:1,
   $0:tr.get_TFlags()|4
  });
 };
 Trans.Enter=function(f,tr)
 {
  return tr.Copy(null,{
   $:1,
   $0:f
  },null,{
   $:1,
   $0:tr.get_TFlags()|2
  });
 };
 Trans.Change=function(ch,tr)
 {
  return tr.Copy({
   $:1,
   $0:ch
  },null,null,{
   $:1,
   $0:tr.get_TFlags()|1
  });
 };
 Trans.Create=function(ch)
 {
  return new Trans.New$1(ch);
 };
 Trans.Trivial=function()
 {
  return new Trans.New$2();
 };
 Trans.CanAnimateExit=function(tr)
 {
  var c,flag;
  c=tr.get_TFlags();
  flag=4;
  return(c&flag)===flag;
 };
 Trans.CanAnimateEnter=function(tr)
 {
  var c,flag;
  c=tr.get_TFlags();
  flag=2;
  return(c&flag)===flag;
 };
 Trans.CanAnimateChange=function(tr)
 {
  var c,flag;
  c=tr.get_TFlags();
  flag=1;
  return(c&flag)===flag;
 };
 Trans.AnimateExit=function(tr,x)
 {
  return(tr.get_TExit())(x);
 };
 Trans.AnimateEnter=function(tr,x)
 {
  return(tr.get_TEnter())(x);
 };
 Trans.AnimateChange=function(tr,x,y)
 {
  return tr.TChange(x,y);
 };
 SC$6.$cctor=function()
 {
  SC$6.$cctor=Global.ignore;
  SC$6.CubicInOut=Easing.Custom(function(t)
  {
   var t2;
   t2=t*t;
   return 3*t2-2*(t2*t);
  });
  SC$6.UseAnimations=true;
 };
 AnimatedAttrNode=Next.AnimatedAttrNode=Runtime.Class({
  sync:function(p)
  {
   var x;
   if(this.dirty)
    {
     x=this.logical;
     x==null?void 0:(this.push(p))(x.$0);
     this.visible=this.logical;
     this.dirty=false;
    }
  },
  pushVisible:function(el,v)
  {
   this.visible={
    $:1,
    $0:v
   };
   this.dirty=true;
   (this.push(el))(v);
  },
  NChanged:function()
  {
   return this.updates;
  },
  NSync:Global.ignore,
  NGetExitAnim:function(parent)
  {
   var $this,m;
   $this=this;
   return An.WhenDone(function()
   {
    $this.dirty=true;
    $this.visible=null;
   },(m=this.visible,m!=null&&m.$==1?An.Pack(An.Map(function(v)
   {
    $this.pushVisible(parent,v);
   },Trans.AnimateExit(this.tr,m.$0))):An.get_Empty()));
  },
  NGetEnterAnim:function(parent)
  {
   var $this,$1,$2,$3,$4;
   $this=this;
   return An.WhenDone(function()
   {
    $this.sync(parent);
   },($1=this.visible,($2=this.logical,$1!=null&&$1.$==1&&($2!=null&&$2.$==1&&(this.dirty&&($3=[$2.$0,$1.$0],true)))?An.Pack(An.Map(function(v)
   {
    $this.pushVisible(parent,v);
   },Trans.AnimateChange(this.tr,$3[1],$3[0]))):$1==null&&($2!=null&&$2.$==1&&true)?An.Pack(An.Map(function(v)
   {
    $this.pushVisible(parent,v);
   },Trans.AnimateEnter(this.tr,$2.$0))):An.get_Empty())));
  },
  NGetChangeAnim:function(parent)
  {
   var $this,$1,$2,$3;
   $this=this;
   return An.WhenDone(function()
   {
    $this.sync(parent);
   },($1=this.visible,($2=this.logical,$1!=null&&$1.$==1&&($2!=null&&$2.$==1&&(this.dirty&&($3=[$2.$0,$1.$0],true)))?An.Pack(An.Map(function(v)
   {
    $this.pushVisible(parent,v);
   },Trans.AnimateChange(this.tr,$3[1],$3[0]))):An.get_Empty())));
  }
 },WebSharper.Obj,AnimatedAttrNode);
 AnimatedAttrNode.New=Runtime.Ctor(function(tr,view,push)
 {
  var $this;
  $this=this;
  this.tr=tr;
  this.push=push;
  this.logical=null;
  this.visible=null;
  this.dirty=true;
  this.updates=View.Map(function(x)
  {
   $this.logical={
    $:1,
    $0:x
   };
   $this.dirty=true;
  },view);
 },AnimatedAttrNode);
 DynamicAttrNode=Next.DynamicAttrNode=Runtime.Class({
  NChanged:function()
  {
   return this.updates;
  },
  NSync:function(parent)
  {
   if(this.dirty)
    {
     (this.push(parent))(this.value);
     this.dirty=false;
    }
  },
  NGetExitAnim:function(parent)
  {
   return An.get_Empty();
  },
  NGetEnterAnim:function(parent)
  {
   return An.get_Empty();
  },
  NGetChangeAnim:function(parent)
  {
   return An.get_Empty();
  }
 },WebSharper.Obj,DynamicAttrNode);
 DynamicAttrNode.New=Runtime.Ctor(function(view,push)
 {
  var $this;
  $this=this;
  this.push=push;
  this.value=void 0;
  this.dirty=false;
  this.updates=View.Map(function(x)
  {
   $this.value=x;
   $this.dirty=true;
  },view);
 },DynamicAttrNode);
 AttrProxy=Next.AttrProxy=Runtime.Class({},null,AttrProxy);
 AttrProxy.Handler=function(event,q)
 {
  return Attrs$1.Static(function(el)
  {
   el.addEventListener(event,function(d)
   {
    return(q(el))(d);
   },false);
  });
 };
 AttrProxy.Concat=function(xs)
 {
  var x;
  x=Array.ofSeqNonCopying(xs);
  return Array.TreeReduce(Attrs$1.EmptyAttr(),AttrProxy.Append,x);
 };
 AttrProxy.Append=function(a,b)
 {
  return Attrs$1.AppendTree(a,b);
 };
 AttrProxy.Create=function(name,value)
 {
  return Attrs$1.Static(function(el)
  {
   DomUtility.SetAttr(el,name,value);
  });
 };
 Dyn.New=function(DynElem,DynFlags,DynNodes,OnAfterRender)
 {
  var $1;
  $1={
   DynElem:DynElem,
   DynFlags:DynFlags,
   DynNodes:DynNodes
  };
  Runtime.SetOptional($1,"OnAfterRender",OnAfterRender);
  return $1;
 };
 Attrs$1.Static=function(attr$1)
 {
  return new AttrProxy({
   $:3,
   $0:attr$1
  });
 };
 Attrs$1.Dynamic=function(view,set)
 {
  return new AttrProxy({
   $:1,
   $0:new DynamicAttrNode.New(view,set)
  });
 };
 Attrs$1.Animated=function(tr,view,set)
 {
  var node,flags,n;
  node=new AnimatedAttrNode.New(tr,view,set);
  flags=4;
  Trans.CanAnimateEnter(tr)?flags=flags|1:void 0;
  Trans.CanAnimateExit(tr)?flags=flags|2:void 0;
  n=new AttrProxy({
   $:1,
   $0:node
  });
  Attrs$1.SetFlags(n,flags);
  return n;
 };
 Attrs$1.EmptyAttr=function()
 {
  SC$7.$cctor();
  return SC$7.EmptyAttr;
 };
 Attrs$1.AppendTree=function(a,b)
 {
  var x;
  return a===null?b:b===null?a:(x=new AttrProxy({
   $:2,
   $0:a,
   $1:b
  }),(Attrs$1.SetFlags(x,Attrs$1.Flags(a)|Attrs$1.Flags(b)),x));
 };
 Attrs$1.GetChangeAnim=function(dyn)
 {
  return Attrs$1.GetAnim(dyn,function($1,$2)
  {
   return $1.NGetChangeAnim($2);
  });
 };
 Attrs$1.GetExitAnim=function(dyn)
 {
  return Attrs$1.GetAnim(dyn,function($1,$2)
  {
   return $1.NGetExitAnim($2);
  });
 };
 Attrs$1.GetEnterAnim=function(dyn)
 {
  return Attrs$1.GetAnim(dyn,function($1,$2)
  {
   return $1.NGetEnterAnim($2);
  });
 };
 Attrs$1.GetAnim=function(dyn,f)
 {
  return An.Concat(Arrays.map(function(n)
  {
   return f(n,dyn.DynElem);
  },dyn.DynNodes));
 };
 Attrs$1.Updates=function(dyn)
 {
  return Array.MapTreeReduce(function(x)
  {
   return x.NChanged();
  },View.Const(),View.Map2Unit,dyn.DynNodes);
 };
 Attrs$1.Empty=function(e)
 {
  return Dyn.New(e,0,[],null);
 };
 Attrs$1.Insert=function(elem,tree)
 {
  var nodes,oar,arr;
  function loop(node)
  {
   if(!(node===null))
    if(node!=null&&node.$==1)
     nodes.push(node.$0);
    else
     if(node!=null&&node.$==2)
      {
       loop(node.$0);
       loop(node.$1);
      }
     else
      if(node!=null&&node.$==3)
       node.$0(elem);
      else
       if(node!=null&&node.$==4)
        oar.push(node.$0);
  }
  nodes=[];
  oar=[];
  loop(tree);
  arr=nodes.slice(0);
  return Dyn.New(elem,Attrs$1.Flags(tree),arr,oar.length===0?null:{
   $:1,
   $0:function(el)
   {
    Seq.iter(function(f)
    {
     f(el);
    },oar);
   }
  });
 };
 Attrs$1.Sync=function(elem,dyn)
 {
  Arrays.iter(function(d)
  {
   d.NSync(elem);
  },dyn.DynNodes);
 };
 Attrs$1.SetFlags=function(a,f)
 {
  a.flags=f;
 };
 Attrs$1.Flags=function(a)
 {
  return a!==null&&a.hasOwnProperty("flags")?a.flags:0;
 };
 Attrs$1.HasExitAnim=function(attr$1)
 {
  var flag;
  flag=2;
  return(attr$1.DynFlags&flag)===flag;
 };
 Attrs$1.HasEnterAnim=function(attr$1)
 {
  var flag;
  flag=1;
  return(attr$1.DynFlags&flag)===flag;
 };
 Attrs$1.HasChangeAnim=function(attr$1)
 {
  var flag;
  flag=4;
  return(attr$1.DynFlags&flag)===flag;
 };
 CheckedInput=Next.CheckedInput=Runtime.Class({
  get_Input:function()
  {
   return this.$==1?this.$0:this.$==2?this.$0:this.$1;
  }
 },null,CheckedInput);
 CheckedInput.Make=function(x)
 {
  return new CheckedInput({
   $:0,
   $0:x,
   $1:Global.String(x)
  });
 };
 AttrModule.ValidateForm=function()
 {
  return AttrModule.OnAfterRender(function(e)
  {
   if(window.H5F)
    window.H5F.setup(e);
  });
 };
 AttrModule.Checked=function(_var)
 {
  function onSet(el,ev)
  {
   return!Unchecked.Equals(_var.RVal(),el.checked)?_var.set_RVal(el.checked):null;
  }
  return AttrProxy.Concat([AttrModule.DynamicProp("checked",_var.RView()),AttrModule.Handler("change",function($1)
  {
   return function($2)
   {
    return onSet($1,$2);
   };
  }),AttrModule.Handler("click",function($1)
  {
   return function($2)
   {
    return onSet($1,$2);
   };
  })]);
 };
 AttrModule.FloatValue=function(_var)
 {
  return AttrModule.CustomVar(_var,function($1,$2)
  {
   var i;
   i=$2.get_Input();
   return $1.value!==i?void($1.value=i):null;
  },function(el)
  {
   var s,i;
   s=el.value;
   return{
    $:1,
    $0:String.isBlank(s)?(el.checkValidity?el.checkValidity():true)?new CheckedInput({
     $:2,
     $0:s
    }):new CheckedInput({
     $:1,
     $0:s
    }):(i=+s,Global.isNaN(i)?new CheckedInput({
     $:1,
     $0:s
    }):new CheckedInput({
     $:0,
     $0:i,
     $1:s
    }))
   };
  });
 };
 AttrModule.FloatValueUnchecked=function(_var)
 {
  return AttrModule.CustomValue(_var,Global.String,function(s)
  {
   var pd;
   return String.isBlank(s)?{
    $:1,
    $0:0
   }:(pd=+s,Global.isNaN(pd)?null:{
    $:1,
    $0:pd
   });
  });
 };
 AttrModule.IntValue=function(_var)
 {
  return AttrModule.CustomVar(_var,function($1,$2)
  {
   var i;
   i=$2.get_Input();
   return $1.value!==i?void($1.value=i):null;
  },function(el)
  {
   var s,m,o;
   s=el.value;
   return{
    $:1,
    $0:String.isBlank(s)?(el.checkValidity?el.checkValidity():true)?new CheckedInput({
     $:2,
     $0:s
    }):new CheckedInput({
     $:1,
     $0:s
    }):(m=(o=0,[Numeric.TryParseInt32(s,{
     get:function()
     {
      return o;
     },
     set:function(v)
     {
      o=v;
     }
    }),o]),m[0]?new CheckedInput({
     $:0,
     $0:m[1],
     $1:s
    }):new CheckedInput({
     $:1,
     $0:s
    }))
   };
  });
 };
 AttrModule.IntValueUnchecked=function(_var)
 {
  return AttrModule.CustomValue(_var,Global.String,function(s)
  {
   var pd;
   return String.isBlank(s)?{
    $:1,
    $0:0
   }:(pd=+s,pd!==pd>>0?null:{
    $:1,
    $0:pd
   });
  });
 };
 AttrModule.Value=function(_var)
 {
  function g(a)
  {
   return{
    $:1,
    $0:a
   };
  }
  return AttrModule.CustomValue(_var,Global.id,function(x)
  {
   return g(Global.id(x));
  });
 };
 AttrModule.ContentEditableHtml=function(_var)
 {
  var x;
  x=AttrModule.CustomVar(_var,function($1,$2)
  {
   $1.innerHTML=$2;
  },function(e)
  {
   return{
    $:1,
    $0:e.innerHTML
   };
  });
  return AttrProxy.Append(AttrProxy.Create("contenteditable","true"),x);
 };
 AttrModule.ContentEditableText=function(_var)
 {
  var x;
  x=AttrModule.CustomVar(_var,function($1,$2)
  {
   $1.textContent=$2;
  },function(e)
  {
   return{
    $:1,
    $0:e.textContent
   };
  });
  return AttrProxy.Append(AttrProxy.Create("contenteditable","true"),x);
 };
 AttrModule.CustomValue=function(_var,toString,fromString)
 {
  return AttrModule.CustomVar(_var,function($1,$2)
  {
   $1.value=toString($2);
  },function(e)
  {
   return fromString(e.value);
  });
 };
 AttrModule.CustomVar=function(_var,set,get)
 {
  function onChange(el,e)
  {
   return _var.RUpdM(function(v)
   {
    var m,$1;
    m=get(el);
    return m!=null&&m.$==1&&(!Unchecked.Equals(m.$0,v)&&($1=[m,m.$0],true))?$1[0]:null;
   });
  }
  function set$1(e,v)
  {
   var m,$1;
   m=get(e);
   return m!=null&&m.$==1&&(Unchecked.Equals(m.$0,v)&&($1=m.$0,true))?null:set(e,v);
  }
  return AttrProxy.Concat([AttrModule.Handler("change",function($1)
  {
   return function($2)
   {
    return onChange($1,$2);
   };
  }),AttrModule.Handler("input",function($1)
  {
   return function($2)
   {
    return onChange($1,$2);
   };
  }),AttrModule.Handler("keypress",function($1)
  {
   return function($2)
   {
    return onChange($1,$2);
   };
  }),AttrModule.DynamicCustom(function($1)
  {
   return function($2)
   {
    return set$1($1,$2);
   };
  },_var.RView())]);
 };
 AttrModule.DynamicProp=function(name,view)
 {
  return Attrs$1.Dynamic(view,function(el)
  {
   return function(v)
   {
    el[name]=v;
   };
  });
 };
 AttrModule.DynamicPred=function(name,predView,valView)
 {
  function viewFn(el,t)
  {
   return t[0]?DomUtility.SetAttr(el,name,t[1]):DomUtility.RemoveAttr(el,name);
  }
  return Attrs$1.Dynamic(View.Map2(function(pred,value)
  {
   return[pred,value];
  },predView,valView),function($1)
  {
   return function($2)
   {
    return viewFn($1,$2);
   };
  });
 };
 AttrModule.DynamicClass=function(name,view,ok)
 {
  return Attrs$1.Dynamic(view,function(el)
  {
   return function(v)
   {
    return ok(v)?DomUtility.AddClass(el,name):DomUtility.RemoveClass(el,name);
   };
  });
 };
 AttrModule.OnAfterRenderView=function(v,callback)
 {
  var id;
  id=Fresh.Id();
  return AttrProxy.Append(AttrModule.OnAfterRender(function(el)
  {
   callback(el,el[id]);
  }),AttrModule.DynamicCustom(function(el)
  {
   return function(x)
   {
    el[id]=x;
   };
  },v));
 };
 AttrModule.OnAfterRender=function(callback)
 {
  return new AttrProxy({
   $:4,
   $0:callback
  });
 };
 AttrModule.HandlerView=function(name,view,callback)
 {
  return Attrs$1.Static(function(el)
  {
   var callback$1;
   callback$1=callback(el);
   el.addEventListener(name,function(ev)
   {
    return View.Get(callback$1(ev),view);
   },false);
  });
 };
 AttrModule.Handler=function(name,callback)
 {
  return Attrs$1.Static(function(el)
  {
   el.addEventListener(name,function(d)
   {
    return(callback(el))(d);
   },false);
  });
 };
 AttrModule.DynamicStyle=function(name,view)
 {
  return Attrs$1.Dynamic(view,function(el)
  {
   return function(v)
   {
    return DomUtility.SetStyle(el,name,v);
   };
  });
 };
 AttrModule.DynamicCustom=function(set,view)
 {
  return Attrs$1.Dynamic(view,set);
 };
 AttrModule.Dynamic=function(name,view)
 {
  return Attrs$1.Dynamic(view,function(el)
  {
   return function(v)
   {
    return DomUtility.SetAttr(el,name,v);
   };
  });
 };
 AttrModule.AnimatedStyle=function(name,tr,view,attr$1)
 {
  return Attrs$1.Animated(tr,view,function(el)
  {
   return function(v)
   {
    return DomUtility.SetStyle(el,name,attr$1(v));
   };
  });
 };
 AttrModule.Animated=function(name,tr,view,attr$1)
 {
  return Attrs$1.Animated(tr,view,function(el)
  {
   return function(v)
   {
    return DomUtility.SetAttr(el,name,attr$1(v));
   };
  });
 };
 AttrModule.Class=function(name)
 {
  return Attrs$1.Static(function(el)
  {
   DomUtility.AddClass(el,name);
  });
 };
 AttrModule.Style=function(name,value)
 {
  return Attrs$1.Static(function(el)
  {
   DomUtility.SetStyle(el,name,value);
  });
 };
 SC$7.$cctor=function()
 {
  SC$7.$cctor=Global.ignore;
  SC$7.EmptyAttr=null;
 };
 DocElemNode=Next.DocElemNode=Runtime.Class({
  GetHashCode:function()
  {
   return this.ElKey;
  },
  Equals:function(o)
  {
   return this.ElKey===o.ElKey;
  }
 },null,DocElemNode);
 DocElemNode.New=function(Attr,Children,Delimiters,El,ElKey,Render)
 {
  var $1;
  return new DocElemNode(($1={
   Attr:Attr,
   Children:Children,
   El:El,
   ElKey:ElKey
  },(Runtime.SetOptional($1,"Delimiters",Delimiters),Runtime.SetOptional($1,"Render",Render),$1)));
 };
 DomNodes.FoldBack=function(f,a,z)
 {
  return Arrays.foldBack(f,a.$0,z);
 };
 DomNodes.Iter=function(f,a)
 {
  Arrays.iter(f,a.$0);
 };
 DomNodes.Except=function(a,a$1)
 {
  var excluded;
  excluded=a.$0;
  return{
   $:0,
   $0:Arrays.filter(function(n)
   {
    return Arrays.forall(function(k)
    {
     return!(n===k);
    },excluded);
   },a$1.$0)
  };
 };
 DomNodes.DocChildren=function(node)
 {
  var q;
  function loop(doc)
  {
   if(doc!=null&&doc.$==2)
    loop(doc.$0.Current);
   else
    if(doc!=null&&doc.$==1)
     q.push(doc.$0.El);
    else
     if(doc==null)
      ;
     else
      if(doc!=null&&doc.$==5)
       q.push(doc.$0);
      else
       if(doc!=null&&doc.$==4)
        q.push(doc.$0.Text);
       else
        if(doc!=null&&doc.$==6)
         Arrays.iter(function(a)
         {
          if(a.constructor===Global.Object)
           loop(a);
          else
           q.push(a);
         },doc.$0.Els);
        else
         {
          loop(doc.$0);
          loop(doc.$1);
         }
  }
  q=[];
  loop(node.Children);
  return{
   $:0,
   $0:Array.ofSeqNonCopying(q)
  };
 };
 DomNodes.Children=function(elem,delims)
 {
  var n,o,a;
  if(delims!=null&&delims.$==1)
   {
    a=[];
    n=delims.$0[0].nextSibling;
    while(n!==delims.$0[1])
     {
      a.push(n);
      n=n.nextSibling;
     }
    return{
     $:0,
     $0:a
    };
   }
  else
   return{
    $:0,
    $0:Arrays.init(elem.childNodes.length,(o=elem.childNodes,function(a$1)
    {
     return o[a$1];
    }))
   };
 };
 NodeSet.ToArray=function(a)
 {
  return HashSet.ToArray(a.$0);
 };
 NodeSet.get_Empty=function()
 {
  return{
   $:0,
   $0:new HashSet$1.New$3()
  };
 };
 NodeSet.IsEmpty=function(a)
 {
  return a.$0.get_Count()===0;
 };
 NodeSet.Intersect=function(a,a$1)
 {
  return{
   $:0,
   $0:HashSet.Intersect(a.$0,a$1.$0)
  };
 };
 NodeSet.Except=function(a,a$1)
 {
  return{
   $:0,
   $0:HashSet.Except(a.$0,a$1.$0)
  };
 };
 NodeSet.FindAll=function(doc)
 {
  var q;
  function loop(node)
  {
   if(node!=null&&node.$==0)
    {
     loop(node.$0);
     loop(node.$1);
    }
   else
    if(node!=null&&node.$==1)
     loopEN(node.$0);
    else
     if(node!=null&&node.$==2)
      loop(node.$0.Current);
     else
      if(node!=null&&node.$==6)
       Arrays.iter(loopEN,node.$0.Holes);
  }
  function loopEN(el)
  {
   q.push(el);
   loop(el.Children);
  }
  q=[];
  loop(doc);
  return{
   $:0,
   $0:new HashSet$1.New$2(q)
  };
 };
 NodeSet.Filter=function(f,a)
 {
  return{
   $:0,
   $0:HashSet.Filter(f,a.$0)
  };
 };
 RunState.New=function(PreviousNodes,Top)
 {
  return{
   PreviousNodes:PreviousNodes,
   Top:Top
  };
 };
 Docs$1.TextHoleRE=function()
 {
  SC$8.$cctor();
  return SC$8.TextHoleRE;
 };
 Docs$1.LoadedTemplates=function()
 {
  SC$8.$cctor();
  return SC$8.LoadedTemplates;
 };
 Docs$1.UpdateTextNode=function(n,t)
 {
  n.Value=t;
  n.Dirty=true;
 };
 Docs$1.CreateTextNode=function()
 {
  return{
   Text:DomUtility.CreateText(""),
   Dirty:false,
   Value:""
  };
 };
 Docs$1.UpdateEmbedNode=function(node,upd)
 {
  node.Current=upd;
  node.Dirty=true;
 };
 Docs$1.CreateEmbedNode=function()
 {
  return{
   Current:null,
   Dirty:false
  };
 };
 Docs$1.PerformAnimatedUpdate=function(st,doc)
 {
  var b;
  return An.get_UseAnimations()?(b=null,Concurrency.Delay(function()
  {
   var cur,change,enter;
   cur=NodeSet.FindAll(doc);
   change=Docs$1.ComputeChangeAnim(st,cur);
   enter=Docs$1.ComputeEnterAnim(st,cur);
   return Concurrency.Bind(An.Play(An.Append(change,Docs$1.ComputeExitAnim(st,cur))),function()
   {
    return Concurrency.Bind(Docs$1.SyncElemNodesNextFrame(st),function()
    {
     return Concurrency.Bind(An.Play(enter),function()
     {
      st.PreviousNodes=cur;
      return Concurrency.Return(null);
     });
    });
   });
  })):Docs$1.SyncElemNodesNextFrame(st);
 };
 Docs$1.SyncElemNodesNextFrame=function(st)
 {
  function a(ok)
  {
   Global.requestAnimationFrame(function()
   {
    Docs$1.SyncElemNode(st.Top);
    ok();
   });
  }
  return Concurrency.FromContinuations(function($1,$2,$3)
  {
   return a.apply(null,[$1,$2,$3]);
  });
 };
 Docs$1.ComputeEnterAnim=function(st,cur)
 {
  return An.Concat(Arrays.map(function(n)
  {
   return Attrs$1.GetEnterAnim(n.Attr);
  },NodeSet.ToArray(NodeSet.Except(st.PreviousNodes,NodeSet.Filter(function(n)
  {
   return Attrs$1.HasEnterAnim(n.Attr);
  },cur)))));
 };
 Docs$1.ComputeChangeAnim=function(st,cur)
 {
  var relevant;
  function a(n)
  {
   return Attrs$1.HasChangeAnim(n.Attr);
  }
  relevant=function(a$1)
  {
   return NodeSet.Filter(a,a$1);
  };
  return An.Concat(Arrays.map(function(n)
  {
   return Attrs$1.GetChangeAnim(n.Attr);
  },NodeSet.ToArray(NodeSet.Intersect(relevant(st.PreviousNodes),relevant(cur)))));
 };
 Docs$1.ComputeExitAnim=function(st,cur)
 {
  return An.Concat(Arrays.map(function(n)
  {
   return Attrs$1.GetExitAnim(n.Attr);
  },NodeSet.ToArray(NodeSet.Except(cur,NodeSet.Filter(function(n)
  {
   return Attrs$1.HasExitAnim(n.Attr);
  },st.PreviousNodes)))));
 };
 Docs$1.CreateDelimitedRunState=function(ldelim,rdelim,doc)
 {
  return RunState.New(NodeSet.get_Empty(),Docs$1.CreateDelimitedElemNode(ldelim,rdelim,Attrs$1.EmptyAttr(),doc));
 };
 Docs$1.CreateRunState=function(parent,doc)
 {
  return RunState.New(NodeSet.get_Empty(),Docs$1.CreateElemNode(parent,Attrs$1.EmptyAttr(),doc));
 };
 Docs$1.CreateDelimitedElemNode=function(ldelim,rdelim,attr$1,children)
 {
  var el,attr$2;
  el=ldelim.parentNode;
  Docs$1.LinkPrevElement(rdelim,children);
  attr$2=Attrs$1.Insert(el,attr$1);
  return DocElemNode.New(attr$2,children,{
   $:1,
   $0:[ldelim,rdelim]
  },el,Fresh.Int(),Runtime.GetOptional(attr$2.OnAfterRender));
 };
 Docs$1.CreateElemNode=function(el,attr$1,children)
 {
  var attr$2;
  Docs$1.LinkElement(el,children);
  attr$2=Attrs$1.Insert(el,attr$1);
  return DocElemNode.New(attr$2,children,null,el,Fresh.Int(),Runtime.GetOptional(attr$2.OnAfterRender));
 };
 Docs$1.SyncElemNode=function(el)
 {
  Docs$1.SyncElement(el);
  Docs$1.Sync(el.Children);
  Docs$1.AfterRender(el);
 };
 Docs$1.Sync=function(doc)
 {
  var d,t;
  if(doc!=null&&doc.$==1)
   Docs$1.SyncElemNode(doc.$0);
  else
   if(doc!=null&&doc.$==2)
    Docs$1.Sync(doc.$0.Current);
   else
    if(doc==null)
     ;
    else
     if(doc!=null&&doc.$==5)
      ;
     else
      if(doc!=null&&doc.$==4)
       {
        d=doc.$0;
        d.Dirty?(d.Text.nodeValue=d.Value,d.Dirty=false):void 0;
       }
      else
       if(doc!=null&&doc.$==6)
        {
         t=doc.$0;
         Arrays.iter(function(e)
         {
          Docs$1.SyncElemNode(e);
         },t.Holes);
         Arrays.iter(function(t$1)
         {
          Attrs$1.Sync(t$1[0],t$1[1]);
         },t.Attrs);
         Docs$1.AfterRender(t);
        }
       else
        {
         Docs$1.Sync(doc.$0);
         Docs$1.Sync(doc.$1);
        }
 };
 Docs$1.AfterRender=function(el)
 {
  var m;
  m=Runtime.GetOptional(el.Render);
  m!=null&&m.$==1?(m.$0(el.El),Runtime.SetOptional(el,"Render",null)):void 0;
 };
 Docs$1.InsertBeforeDelim=function(afterDelim,doc)
 {
  var p,before;
  p=afterDelim.parentNode;
  before=Global.document.createTextNode("");
  p.insertBefore(before,afterDelim);
  Docs$1.LinkPrevElement(afterDelim,doc);
  return before;
 };
 Docs$1.LinkPrevElement=function(el,children)
 {
  Docs$1.InsertDoc(el.parentNode,children,el);
 };
 Docs$1.LinkElement=function(el,children)
 {
  Docs$1.InsertDoc(el,children,null);
 };
 Docs$1.SyncElement=function(el)
 {
  function hasDirtyChildren(el$1)
  {
   function dirty(doc)
   {
    var d,t;
    return doc!=null&&doc.$==0?dirty(doc.$0)||dirty(doc.$1):doc!=null&&doc.$==2?(d=doc.$0,d.Dirty||dirty(d.Current)):doc!=null&&doc.$==6&&(t=doc.$0,t.Dirty||Arrays.exists(hasDirtyChildren,t.Holes));
   }
   return dirty(el$1.Children);
  }
  Attrs$1.Sync(el.El,el.Attr);
  hasDirtyChildren(el)?Docs$1.DoSyncElement(el):void 0;
 };
 Docs$1.DoSyncElement=function(el)
 {
  var parent,p,m;
  function ins(doc,pos)
  {
   var d,t;
   return doc!=null&&doc.$==1?doc.$0.El:doc!=null&&doc.$==2?(d=doc.$0,d.Dirty?(d.Dirty=false,Docs$1.InsertDoc(parent,d.Current,pos)):ins(d.Current,pos)):doc==null?pos:doc!=null&&doc.$==4?doc.$0.Text:doc!=null&&doc.$==5?doc.$0:doc!=null&&doc.$==6?(t=doc.$0,(t.Dirty?t.Dirty=false:void 0,Arrays.foldBack(function($1,$2)
   {
    return $1.constructor===Global.Object?ins($1,$2):$1;
   },t.Els,pos))):ins(doc.$0,ins(doc.$1,pos));
  }
  parent=el.El;
  DomNodes.Iter((p=el.El,function(e)
  {
   DomUtility.RemoveNode(p,e);
  }),DomNodes.Except(DomNodes.DocChildren(el),DomNodes.Children(el.El,Runtime.GetOptional(el.Delimiters))));
  ins(el.Children,(m=Runtime.GetOptional(el.Delimiters),m!=null&&m.$==1?m.$0[1]:null));
 };
 Docs$1.InsertDoc=function(parent,doc,pos)
 {
  var d;
  return doc!=null&&doc.$==1?Docs$1.InsertNode(parent,doc.$0.El,pos):doc!=null&&doc.$==2?(d=doc.$0,(d.Dirty=false,Docs$1.InsertDoc(parent,d.Current,pos))):doc==null?pos:doc!=null&&doc.$==4?Docs$1.InsertNode(parent,doc.$0.Text,pos):doc!=null&&doc.$==5?Docs$1.InsertNode(parent,doc.$0,pos):doc!=null&&doc.$==6?Arrays.foldBack(function($1,$2)
  {
   return $1.constructor===Global.Object?Docs$1.InsertDoc(parent,$1,$2):Docs$1.InsertNode(parent,$1,$2);
  },doc.$0.Els,pos):Docs$1.InsertDoc(parent,doc.$0,Docs$1.InsertDoc(parent,doc.$1,pos));
 };
 Docs$1.InsertNode=function(parent,node,pos)
 {
  DomUtility.InsertAt(parent,pos,node);
  return node;
 };
 Doc=Next.Doc=Runtime.Class({
  ReplaceInDom:function(elt)
  {
   var rdelim;
   rdelim=Global.document.createTextNode("");
   elt.parentNode.replaceChild(rdelim,elt);
   Doc.RunBefore(rdelim,this);
  }
 },WebSharper.Obj,Doc);
 Doc.Append=function(a,b)
 {
  return Doc.Mk({
   $:0,
   $0:a.docNode,
   $1:b.docNode
  },View.Map2Unit(a.updates,b.updates));
 };
 Doc.Concat=function(xs)
 {
  var x;
  x=Array.ofSeqNonCopying(xs);
  return Array.TreeReduce(Doc.Empty(),Doc.Append,x);
 };
 Doc.Empty=function()
 {
  return Doc.Mk(null,View.Const());
 };
 Doc.TextNode=function(v)
 {
  return Doc.Mk({
   $:5,
   $0:DomUtility.CreateText(v)
  },View.Const());
 };
 Doc.Verbatim=function(html)
 {
  var m;
  return Doc.Mk(Array.MapTreeReduce(function(n)
  {
   return Unchecked.Equals(n.nodeType,Node.TEXT_NODE)?{
    $:5,
    $0:n
   }:{
    $:1,
    $0:Docs$1.CreateElemNode(n,Attrs$1.EmptyAttr(),null)
   };
  },null,function(x,y)
  {
   return{
    $:0,
    $0:x,
    $1:y
   };
  },(m=$.parseHTML(html),Unchecked.Equals(m,null)?[]:m)),View.Const());
 };
 Doc.SvgElementMixed=function(tagname,nodes)
 {
  var p;
  p=Doc.MixedNodes(nodes);
  return Doc.SvgElement(tagname,p[0],p[1]);
 };
 Doc.SvgElement=function(name,attr$1,children)
 {
  var attr$2,children$1;
  attr$2=AttrProxy.Concat(attr$1);
  children$1=Doc.Concat(children);
  return Elt.New(DomUtility.CreateSvgElement(name),attr$2,children$1);
 };
 Doc.ElementMixed=function(tagname,nodes)
 {
  var p;
  p=Doc.MixedNodes(nodes);
  return Doc.Element(tagname,p[0],p[1]);
 };
 Doc.ConcatMixed=function(elts)
 {
  return Doc.Concat(Seq.map(Doc.ToMixedDoc,elts));
 };
 Doc.MixedNodes=function(nodes)
 {
  var attrs,children,e,n;
  attrs=new List$1.New$2();
  children=new List$1.New$2();
  e=Enumerator.Get(nodes);
  try
  {
   while(e.MoveNext())
    {
     n=e.Current();
     n instanceof AttrProxy?attrs.Add(n):children.Add(Doc.ToMixedDoc(n));
    }
  }
  finally
  {
   if("Dispose"in e)
    e.Dispose();
  }
  return[attrs,children];
 };
 Doc.ToMixedDoc=function(o)
 {
  return o instanceof Doc?o:typeof o=="string"?Doc.TextNode(o):o instanceof Global.Element?Doc.Static(o):typeof o=="function"?Doc.EmbedView(View.Map(Doc.ToMixedDoc,o)):o instanceof Var?Doc.EmbedView(View.Map(Doc.ToMixedDoc,o.v)):Unchecked.Equals(o,null)?Doc.Empty():Doc.TextNode(Global.String(o));
 };
 Doc.Element=function(name,attr$1,children)
 {
  var attr$2,children$1;
  attr$2=AttrProxy.Concat(attr$1);
  children$1=Doc.Concat(children);
  return Elt.New(DomUtility.CreateElement(name),attr$2,children$1);
 };
 Doc.Radio=function(attrs,value,_var)
 {
  var el,valAttr;
  el=DomUtility.CreateElement("input");
  el.addEventListener("click",function()
  {
   return _var.RSet(value);
  },false);
  valAttr=AttrModule.DynamicProp("checked",View.Map(function(x)
  {
   return Unchecked.Equals(x,value);
  },_var.RView()));
  return Elt.New(el,AttrProxy.Concat(List.append(List.ofArray([AttrProxy.Create("type","radio"),AttrProxy.Create("name",_var.RId()),valAttr]),List.ofSeq(attrs))),Doc.Empty());
 };
 Doc.LinkView=function(caption,attrs,view,action)
 {
  var attrs$1;
  attrs$1=AttrProxy.Concat(Seq.append([AttrModule.HandlerView("click",view,function()
  {
   return function()
   {
    return action;
   };
  }),AttrProxy.Create("href","#")],attrs));
  return Elt.New(DomUtility.CreateElement("a"),attrs$1,Doc.TextNode(caption));
 };
 Doc.Link=function(caption,attrs,action)
 {
  var attrs$1,x;
  attrs$1=(x=AttrProxy.Concat(attrs),AttrProxy.Append(AttrProxy.Create("href","#"),x));
  return Elt.New(Doc.Clickable("a",action),attrs$1,Doc.TextNode(caption));
 };
 Doc.ButtonView=function(caption,attrs,view,action)
 {
  var attrs$1;
  attrs$1=AttrProxy.Concat(Seq.append([AttrModule.HandlerView("click",view,function()
  {
   return function()
   {
    return action;
   };
  })],attrs));
  return Elt.New(DomUtility.CreateElement("button"),attrs$1,Doc.TextNode(caption));
 };
 Doc.Button=function(caption,attrs,action)
 {
  var attrs$1;
  attrs$1=AttrProxy.Concat(attrs);
  return Elt.New(Doc.Clickable("button",action),attrs$1,Doc.TextNode(caption));
 };
 Doc.Clickable=function(elem,action)
 {
  var el;
  el=DomUtility.CreateElement(elem);
  el.addEventListener("click",function(ev)
  {
   ev.preventDefault();
   return action();
  },false);
  return el;
 };
 Doc.CheckBoxGroup=function(attrs,item,chk)
 {
  function p(y)
  {
   return Unchecked.Equals(item,y);
  }
  return Doc.CheckBox(attrs,Var.Lens(chk,function(l)
  {
   return List.exists(p,l);
  },function(l,b)
  {
   return b?List.exists(function(y)
   {
    return Unchecked.Equals(item,y);
   },l)?l:new List.T({
    $:1,
    $0:item,
    $1:l
   }):List.filter(function(y)
   {
    return!Unchecked.Equals(item,y);
   },l);
  }));
 };
 Doc.CheckBox=function(attrs,chk)
 {
  return Doc.InputInternal("input",function()
  {
   return Seq.append(attrs,[AttrProxy.Create("type","checkbox"),AttrModule.Checked(chk)]);
  });
 };
 Doc.SelectDynOptional=function(attrs,noneText,show,vOptions,current)
 {
  return Doc.SelectDyn(attrs,function(a)
  {
   return a!=null&&a.$==1?show(a.$0):noneText;
  },View.Map(function(options)
  {
   return new List.T({
    $:1,
    $0:null,
    $1:List.map(function(a)
    {
     return{
      $:1,
      $0:a
     };
    },options)
   });
  },vOptions),current);
 };
 Doc.SelectOptional=function(attrs,noneText,show,options,current)
 {
  return Doc.Select(attrs,function(a)
  {
   return a!=null&&a.$==1?show(a.$0):noneText;
  },new List.T({
   $:1,
   $0:null,
   $1:List.map(function(a)
   {
    return{
     $:1,
     $0:a
    };
   },options)
  }),current);
 };
 Doc.Select=function(attrs,show,options,current)
 {
  return Doc.SelectImpl(attrs,show,function(rOptions)
  {
   rOptions[0]=options;
   return Doc.Concat(List.mapi(function(i,o)
   {
    return Doc.Element("option",List.ofArray([AttrProxy.Create("value",Global.String(i))]),List.ofArray([Doc.TextNode(show(o))]));
   },options));
  },current);
 };
 Doc.SelectDyn=function(attrs,show,vOptions,current)
 {
  return Doc.SelectImpl(attrs,show,function(options)
  {
   function a(i,o)
   {
    return Doc.Element("option",List.ofArray([AttrProxy.Create("value",Global.String(i))]),List.ofArray([Doc.TextNode(show(o))]));
   }
   return Doc.Convert(function($1)
   {
    return a($1[0],$1[1]);
   },View.Map(function(l)
   {
    options[0]=l;
    return Seq.mapi(function(i,x)
    {
     return[i,x];
    },l);
   },vOptions));
  },current);
 };
 Doc.SelectImpl=function(attrs,show,optionElements,current)
 {
  var options,el,selectedItemAttr,x;
  function getIndex(el$1)
  {
   return el$1.selectedIndex;
  }
  function setIndex(el$1,i)
  {
   el$1.selectedIndex=i;
  }
  function getSelectedItem(el$1)
  {
   var i;
   i=getIndex(el$1);
   return options[0].get_Item(i);
  }
  function itemIndex(x$1)
  {
   return Seq.findIndex(function(y)
   {
    return Unchecked.Equals(x$1,y);
   },options[0]);
  }
  function setSelectedItem(el$1,item)
  {
   return setIndex(el$1,itemIndex(item));
  }
  options=[List.T.Empty];
  el=DomUtility.CreateElement("select");
  selectedItemAttr=AttrModule.DynamicCustom(function($1)
  {
   return function($2)
   {
    return setSelectedItem($1,$2);
   };
  },current.RView());
  el.addEventListener("change",function()
  {
   current.RUpdM(function(x$1)
   {
    var y;
    y=getSelectedItem(el);
    return Unchecked.Equals(x$1,y)?null:{
     $:1,
     $0:y
    };
   });
  },false);
  return Elt.New(el,(x=AttrProxy.Append(selectedItemAttr,AttrProxy.Concat(attrs)),AttrProxy.Append(AttrModule.OnAfterRender(function(el$1)
  {
   setSelectedItem(el$1,current.RGet());
  }),x)),optionElements(options));
 };
 Doc.InputArea=function(attr$1,_var)
 {
  return Doc.InputInternal("textarea",function()
  {
   return Seq.append(attr$1,[AttrModule.Value(_var)]);
  });
 };
 Doc.FloatInput=function(attr$1,_var)
 {
  return Doc.InputInternal("input",function()
  {
   return Seq.append(attr$1,[AttrModule.FloatValue(_var),AttrProxy.Create("type","number")]);
  });
 };
 Doc.FloatInputUnchecked=function(attr$1,_var)
 {
  return Doc.InputInternal("input",function()
  {
   return Seq.append(attr$1,[_var.RGet()===0?AttrProxy.Create("value","0"):Attrs$1.EmptyAttr(),AttrModule.FloatValueUnchecked(_var),AttrProxy.Create("type","number")]);
  });
 };
 Doc.IntInput=function(attr$1,_var)
 {
  return Doc.InputInternal("input",function()
  {
   return Seq.append(attr$1,[AttrModule.IntValue(_var),AttrProxy.Create("type","number")]);
  });
 };
 Doc.IntInputUnchecked=function(attr$1,_var)
 {
  return Doc.InputInternal("input",function()
  {
   return Seq.append(attr$1,[_var.RGet()===0?AttrProxy.Create("value","0"):Attrs$1.EmptyAttr(),AttrModule.IntValueUnchecked(_var),AttrProxy.Create("type","number")]);
  });
 };
 Doc.PasswordBox=function(attr$1,_var)
 {
  return Doc.InputInternal("input",function()
  {
   return Seq.append(attr$1,[AttrModule.Value(_var),AttrProxy.Create("type","password")]);
  });
 };
 Doc.Input=function(attr$1,_var)
 {
  return Doc.InputInternal("input",function()
  {
   return Seq.append(attr$1,[AttrModule.Value(_var)]);
  });
 };
 Doc.InputInternal=function(elemTy,attr$1)
 {
  var el;
  el=DomUtility.CreateElement(elemTy);
  return Elt.New(el,AttrProxy.Concat(attr$1(el)),Doc.Empty());
 };
 Doc.ConvertSeqBy=function(key,render,view)
 {
  return Doc.Flatten(View.MapSeqCachedViewBy(key,function($1,$2)
  {
   return(render($1))($2);
  },view));
 };
 Doc.ConvertSeq=function(render,view)
 {
  return Doc.Flatten(View.MapSeqCachedView(render,view));
 };
 Doc.ConvertBy=function(key,render,view)
 {
  return Doc.Flatten(View.MapSeqCachedBy(key,render,view));
 };
 Doc.Convert=function(render,view)
 {
  return Doc.Flatten(View.MapSeqCached(render,view));
 };
 Doc.Flatten=function(view)
 {
  return Doc.EmbedView(View.Map(Doc.Concat,view));
 };
 Doc.GetOrLoadTemplate=function(baseName,name,els,fillWith)
 {
  Doc.PrepareTemplate(baseName,name,els);
  return Doc.NamedTemplate(baseName,name,fillWith);
 };
 Doc.NamedTemplate=function(baseName,name,fillWith)
 {
  var name$1,m,o;
  name$1=Doc.ComposeName(baseName,name);
  m=(o=null,[Docs$1.LoadedTemplates().TryGetValue(name$1,{
   get:function()
   {
    return o;
   },
   set:function(v)
   {
    o=v;
   }
  }),o]);
  return m[0]?Doc.ChildrenTemplate(m[1].cloneNode(true),fillWith):(console.warn("Local template doesn't exist",name$1),Doc.Empty());
 };
 Doc.LoadLocalTemplates=function(baseName)
 {
  (function()
  {
   var m,m$1,name,name$1;
   while(true)
    {
     m=Global.document.querySelector("[ws-template]");
     if(Unchecked.Equals(m,null))
      {
       m$1=Global.document.querySelector("[ws-children-template]");
       if(Unchecked.Equals(m$1,null))
        return null;
       else
        {
         name=m$1.getAttribute("ws-children-template");
         m$1.removeAttribute("ws-children-template");
         Doc.PrepareTemplate(baseName,{
          $:1,
          $0:name
         },function(n)
         {
          return function()
          {
           return DomUtility.ChildrenArray(n);
          };
         }(m$1));
        }
      }
     else
      {
       name$1=m.getAttribute("ws-template");
       Doc.PrepareSingleTemplate(baseName,{
        $:1,
        $0:name$1
       },m);
      }
    }
  }());
 };
 Doc.PrepareTemplate=function(baseName,name,els)
 {
  var els$1,i,$1,el,m;
  if(!Docs$1.LoadedTemplates().ContainsKey(Doc.ComposeName(baseName,name)))
   {
    els$1=els();
    for(i=0,$1=els$1.length-1;i<=$1;i++){
     el=Arrays.get(els$1,i);
     m=el.parentNode;
     Unchecked.Equals(m,null)?void 0:m.removeChild(el);
    }
    Doc.PrepareTemplateStrict(baseName,name,els$1);
   }
 };
 Doc.PrepareTemplateStrict=function(baseName,name,els)
 {
  var fakeroot;
  function convertAttrs(el)
  {
   var attrs,toRemove,events,holedAttrs,i,$1,a,_this;
   function lowercaseAttr(name$1)
   {
    var m;
    m=el.getAttribute(name$1);
    m===null?void 0:el.setAttribute(name$1,m.toLowerCase());
   }
   attrs=el.attributes;
   toRemove=[];
   events=[];
   holedAttrs=[];
   for(i=0,$1=attrs.length-1;i<=$1;i++){
    a=attrs.item(i);
    Strings.StartsWith(a.nodeName,"ws-on")&&a.nodeName!=="ws-onafterrender"&&a.nodeName!=="ws-on"?(toRemove.push(a.nodeName),events.push(Slice.string(a.nodeName,{
     $:1,
     $0:"ws-on".length
    },null)+":"+a.nodeValue.toLowerCase())):!Strings.StartsWith(a.nodeName,"ws-")&&(new Global.RegExp(Docs$1.TextHoleRE())).test(a.nodeValue)?(a.nodeValue=(_this=new Global.RegExp(Docs$1.TextHoleRE(),"g"),a.nodeValue.replace(_this,function($2,$3)
    {
     return"${"+$3.toLowerCase()+"}";
    })),holedAttrs.push(a.nodeName)):void 0;
   }
   !(events.length==0)?el.setAttribute("ws-on",Strings.concat(" ",events)):void 0;
   !(holedAttrs.length==0)?el.setAttribute("ws-attr-holes",Strings.concat(" ",holedAttrs)):void 0;
   lowercaseAttr("ws-hole");
   lowercaseAttr("ws-replace");
   lowercaseAttr("ws-attr");
   lowercaseAttr("ws-onafterrender");
   lowercaseAttr("ws-var");
   Arrays.iter(function(a$1)
   {
    el.removeAttribute(a$1);
   },toRemove);
  }
  function convertTextNode(n)
  {
   var m,li,$1,s,strRE,hole;
   m=null;
   li=0;
   s=n.textContent;
   strRE=new Global.RegExp(Docs$1.TextHoleRE(),"g");
   while(m=strRE.exec(s),m!==null)
    {
     n.parentNode.insertBefore(Global.document.createTextNode(Slice.string(s,{
      $:1,
      $0:li
     },{
      $:1,
      $0:strRE.lastIndex-Arrays.get(m,0).length-1
     })),n);
     li=strRE.lastIndex;
     hole=Global.document.createElement("span");
     hole.setAttribute("ws-replace",Arrays.get(m,1).toLowerCase());
     n.parentNode.insertBefore(hole,n);
    }
   strRE.lastIndex=0;
   n.textContent=Slice.string(s,{
    $:1,
    $0:li
   },null);
  }
  function mapHoles(t,mappings)
  {
   function run(attrName)
   {
    DomUtility.IterSelector(t,"["+attrName+"]",function(e)
    {
     var m,o;
     m=(o=null,[mappings.TryGetValue(e.getAttribute(attrName).toLowerCase(),{
      get:function()
      {
       return o;
      },
      set:function(v)
      {
       o=v;
      }
     }),o]);
     m[0]?e.setAttribute(attrName,m[1]):void 0;
    });
   }
   run("ws-hole");
   run("ws-replace");
   run("ws-attr");
   run("ws-onafterrender");
   run("ws-var");
   DomUtility.IterSelector(t,"[ws-on]",function(e)
   {
    e.setAttribute("ws-on",Strings.concat(" ",Arrays.map(function(x)
    {
     var a,m,o;
     a=Strings.SplitChars(x,[":"],1);
     m=(o=null,[mappings.TryGetValue(Arrays.get(a,1),{
      get:function()
      {
       return o;
      },
      set:function(v)
      {
       o=v;
      }
     }),o]);
     return m[0]?Arrays.get(a,0)+":"+m[1]:x;
    },Strings.SplitChars(e.getAttribute("ws-on"),[" "],1))));
   });
   return DomUtility.IterSelector(t,"[ws-attr-holes]",function(e)
   {
    var holeAttrs,i,$1;
    holeAttrs=Strings.SplitChars(e.getAttribute("ws-attr-holes"),[" "],1);
    for(i=0,$1=holeAttrs.length-1;i<=$1;i++)(function()
    {
     var attrName;
     function f(s,a)
     {
      var a$1;
      a$1=Operators.KeyValue(a);
      return s.replace(new Global.RegExp("\\${"+a$1[0]+"}","ig"),"${"+a$1[1]+"}");
     }
     attrName=Arrays.get(holeAttrs,i);
     return e.setAttribute(attrName,(((Runtime.Curried3(Seq.fold))(f))(e.getAttribute(attrName)))(mappings));
    }());
   });
  }
  function fillInstanceAttrs(instance,fillWith)
  {
   var name$1,m,i,$1,a;
   convertAttrs(fillWith);
   name$1=fillWith.nodeName.toLowerCase();
   m=instance.querySelector("[ws-attr="+name$1+"]");
   if(Unchecked.Equals(m,null))
    return console.warn("Filling non-existent attr hole",name$1);
   else
    {
     m.removeAttribute("ws-attr");
     for(i=0,$1=fillWith.attributes.length-1;i<=$1;i++){
      a=fillWith.attributes.item(i);
      a.name==="class"&&m.hasAttribute("class")?m.setAttribute("class",m.getAttribute("class")+" "+a.nodeValue):m.setAttribute(a.name,a.nodeValue);
     }
     return;
    }
  }
  function removeHolesExcept(instance,dontRemove)
  {
   function run(attrName)
   {
    DomUtility.IterSelector(instance,"["+attrName+"]",function(e)
    {
     if(!dontRemove.Contains(e.getAttribute(attrName)))
      e.removeAttribute(attrName);
    });
   }
   run("ws-attr");
   run("ws-onafterrender");
   run("ws-var");
   DomUtility.IterSelector(instance,"[ws-hole]",function(e)
   {
    if(!dontRemove.Contains(e.getAttribute("ws-hole")))
     {
      e.removeAttribute("ws-hole");
      while(e.hasChildNodes())
       e.removeChild(e.lastChild);
     }
   });
   DomUtility.IterSelector(instance,"[ws-replace]",function(e)
   {
    if(!dontRemove.Contains(e.getAttribute("ws-replace")))
     e.parentNode.removeChild(e);
   });
   DomUtility.IterSelector(instance,"[ws-on]",function(e)
   {
    e.setAttribute("ws-on",Strings.concat(" ",Arrays.filter(function(x)
    {
     return dontRemove.Contains(Arrays.get(Strings.SplitChars(x,[":"],1),1));
    },Strings.SplitChars(e.getAttribute("ws-on"),[" "],1))));
   });
   return DomUtility.IterSelector(instance,"[ws-attr-holes]",function(e)
   {
    var holeAttrs,i,$1,attrName,_this;
    holeAttrs=Strings.SplitChars(e.getAttribute("ws-attr-holes"),[" "],1);
    for(i=0,$1=holeAttrs.length-1;i<=$1;i++){
     attrName=Arrays.get(holeAttrs,i);
     e.setAttribute(attrName,(_this=new Global.RegExp(Docs$1.TextHoleRE(),"g"),e.getAttribute(attrName).replace(_this,function($2,$3)
     {
      return dontRemove.Contains($3)?$2:"";
     })));
    }
   });
  }
  function fillTextHole(instance,fillWith)
  {
   var m;
   m=instance.querySelector("[ws-replace]");
   return Unchecked.Equals(m,null)?(console.warn("Filling non-existent text hole",name),null):(m.parentNode.replaceChild(new Global.Text(fillWith),m),{
    $:1,
    $0:m.getAttribute("ws-replace")
   });
  }
  function fill(fillWith,p,n)
  {
   while(true)
    if(fillWith.hasChildNodes())
     n=p.insertBefore(fillWith.lastChild,n);
    else
     return null;
  }
  function recF(recI,$1,$2)
  {
   var $3,x,f,name$1,name$2,t,instance,usedHoles,mappings,attrs,i,$4,name$3,m,i$1,$5,n,singleTextFill,i$2,$6,n$1,next;
   while(true)
    switch(recI)
    {
     case 0:
      function g(v)
      {
      }
      name$1=Slice.string($1.nodeName,{
       $:1,
       $0:3
      },null).toLowerCase();
      name$2=name$1.indexOf(".")===-1?baseName+"/"+name$1:Strings.Replace(name$1,".","/");
      if(!Docs$1.LoadedTemplates().ContainsKey(name$2))
       return console.warn("Instantiating non-loaded template",name$2);
      else
       {
        t=Docs$1.LoadedTemplates().get_Item(name$2);
        instance=t.cloneNode(true);
        usedHoles=new HashSet$1.New$3();
        mappings=new Dictionary.New$5();
        attrs=$1.attributes;
        for(i=0,$4=attrs.length-1;i<=$4;i++){
         name$3=attrs.item(i).name.toLowerCase();
         mappings.set_Item(name$3,(m=attrs.item(i).nodeValue,m===""?name$3:m.toLowerCase()));
         !usedHoles.Add(name$3)?console.warn("Hole mapped twice",name$3):void 0;
        }
        for(i$1=0,$5=$1.childNodes.length-1;i$1<=$5;i$1++){
         n=$1.childNodes[i$1];
         Unchecked.Equals(n.nodeType,Node.ELEMENT_NODE)?!usedHoles.Add(n.nodeName.toLowerCase())?console.warn("Hole filled twice",name$2):void 0:void 0;
        }
        singleTextFill=$1.childNodes.length===1&&Unchecked.Equals($1.firstChild.nodeType,Node.TEXT_NODE);
        if(singleTextFill)
         {
          x=fillTextHole(instance,$1.firstChild.textContent);
          ((function(a)
          {
           return function(o)
           {
            if(o!=null)
             a(o.$0);
           };
          }((f=function(usedHoles$1)
          {
           return function(a)
           {
            return usedHoles$1.Add(a);
           };
          }(usedHoles),function(x$1)
          {
           return g(f(x$1));
          })))(x));
         }
        removeHolesExcept(instance,usedHoles);
        if(!singleTextFill)
         {
          for(i$2=0,$6=$1.childNodes.length-1;i$2<=$6;i$2++){
           n$1=$1.childNodes[i$2];
           Unchecked.Equals(n$1.nodeType,Node.ELEMENT_NODE)?n$1.hasAttributes()?fillInstanceAttrs(instance,n$1):fillDocHole(instance,n$1):void 0;
          }
         }
        mapHoles(instance,mappings);
        (((function(a)
        {
         function c($7,$8)
         {
          return fill(a,$7,$8);
         }
         return function($7)
         {
          return function($8)
          {
           return c($7,$8);
          };
         };
        }(instance))($1.parentNode))($1));
        {
         $1.parentNode.removeChild($1);
         return;
        }
       }
      break;
     case 1:
      if($2!==null)
       {
        next=$2.nextSibling;
        if(Unchecked.Equals($2.nodeType,Node.TEXT_NODE))
         convertTextNode($2);
        else
         if(Unchecked.Equals($2.nodeType,Node.ELEMENT_NODE))
          convertElement($2);
        $2=next;
       }
      else
       return null;
      break;
    }
  }
  function fillDocHole(instance,fillWith)
  {
   var m,name$1,m$1;
   function fillHole(p,n)
   {
    var parsed,i,$1;
    if(name$1==="title"&&fillWith.hasChildNodes())
     {
      parsed=$.parseHTML(fillWith.textContent);
      fillWith.removeChild(fillWith.firstChild);
      for(i=0,$1=parsed.length-1;i<=$1;i++)fillWith.appendChild(Arrays.get(parsed,i));
     }
    else
     null;
    convertElement(fillWith);
    return fill(fillWith,p,n);
   }
   name$1=fillWith.nodeName.toLowerCase();
   DomUtility.IterSelector(instance,"[ws-attr-holes]",function(e)
   {
    var holeAttrs,i,$1,attrName,_this;
    holeAttrs=Strings.SplitChars(e.getAttribute("ws-attr-holes"),[" "],1);
    for(i=0,$1=holeAttrs.length-1;i<=$1;i++){
     attrName=Arrays.get(holeAttrs,i);
     e.setAttribute(attrName,(_this=new Global.RegExp("\\${"+name$1+"}","ig"),e.getAttribute(attrName).replace(_this,fillWith.textContent)));
    }
   });
   m$1=instance.querySelector("[ws-hole="+name$1+"]");
   if(Unchecked.Equals(m$1,null))
    {
     m=instance.querySelector("[ws-replace="+name$1+"]");
     return Unchecked.Equals(m,null)?null:(fillHole(m.parentNode,m),void m.parentNode.removeChild(m));
    }
   else
    {
     while(m$1.hasChildNodes())
      m$1.removeChild(m$1.lastChild);
     m$1.removeAttribute("ws-hole");
     return fillHole(m$1,null);
    }
  }
  function convertInstantiation(el)
  {
   return recF(0,el);
  }
  function convertElement(el)
  {
   var m,m$1;
   if(Strings.StartsWith(el.nodeName.toLowerCase(),"ws-")&&!el.hasAttribute("ws-template"))
    convertInstantiation(el);
   else
    {
     convertAttrs(el);
     m=el.getAttribute("ws-template");
     if(m===null)
      {
       m$1=el.getAttribute("ws-children-template");
       if(m$1===null)
        convert(el,el.firstChild);
       else
        {
         el.removeAttribute("ws-children-template");
         Doc.PrepareTemplate(baseName,{
          $:1,
          $0:m$1
         },function()
         {
          return DomUtility.ChildrenArray(el);
         });
         while(el.hasChildNodes())
          el.removeChild(el.lastChild);
        }
      }
     else
      Doc.PrepareSingleTemplate(baseName,{
       $:1,
       $0:m
      },el);
    }
  }
  function convert(p,n)
  {
   return recF(1,p,n);
  }
  fakeroot=Doc.FakeRoot(els);
  Docs$1.LoadedTemplates().set_Item(Doc.ComposeName(baseName,name),fakeroot);
  Arrays.length(els)>0?convert(fakeroot,Arrays.get(els,0)):void 0;
 };
 Doc.ComposeName=function(baseName,name)
 {
  return(baseName+"/"+(name==null?"":name.$0)).toLowerCase();
 };
 Doc.PrepareSingleTemplate=function(baseName,name,el)
 {
  var m,m$1,n;
  el.removeAttribute("ws-template");
  m=el.getAttribute("ws-replace");
  m===null?void 0:(el.removeAttribute("ws-replace"),m$1=el.parentNode,Unchecked.Equals(m$1,null)?void 0:(n=Global.document.createElement(el.tagName),n.setAttribute("ws-replace",m),m$1.replaceChild(n,el)));
  Doc.PrepareTemplateStrict(baseName,name,[el]);
 };
 Doc.FakeRoot=function(els)
 {
  var fakeroot,i,$1;
  fakeroot=Global.document.createElement("div");
  for(i=0,$1=els.length-1;i<=$1;i++)fakeroot.appendChild(Arrays.get(els,i));
  return fakeroot;
 };
 Doc.ChildrenTemplate=function(el,fillWith)
 {
  var els,docTreeNode,$1,updates,$2,holes,updates$1,attrs,afterRender,fw,e,x;
  function addAttr(el$1,attr$1)
  {
   var attr$2,o,f;
   attr$2=Attrs$1.Insert(el$1,attr$1);
   updates$1.push(Attrs$1.Updates(attr$2));
   attrs.push([el$1,attr$2]);
   o=Runtime.GetOptional(attr$2.OnAfterRender);
   return o==null?null:(f=o.$0,void afterRender.push(function()
   {
    f(el$1);
   }));
  }
  function tryGetAsDoc(name)
  {
   var m,o;
   m=(o=null,[fw.TryGetValue(name,{
    get:function()
    {
     return o;
    },
    set:function(v)
    {
     o=v;
    }
   }),o]);
   return m[0]?m[1].$==0?{
    $:1,
    $0:m[1].$1
   }:m[1].$==1?{
    $:1,
    $0:Doc.TextNode(m[1].$1)
   }:m[1].$==2?{
    $:1,
    $0:Doc.TextView(m[1].$1)
   }:m[1].$==6?{
    $:1,
    $0:Doc.TextView(m[1].$1.RView())
   }:m[1].$==7?{
    $:1,
    $0:Doc.TextView(View.Map(Global.String,m[1].$1.RView()))
   }:m[1].$==8?{
    $:1,
    $0:Doc.TextView(View.Map(function(i)
    {
     return i.get_Input();
    },m[1].$1.RView()))
   }:m[1].$==9?{
    $:1,
    $0:Doc.TextView(View.Map(Global.String,m[1].$1.RView()))
   }:m[1].$==10?{
    $:1,
    $0:Doc.TextView(View.Map(function(i)
    {
     return i.get_Input();
    },m[1].$1.RView()))
   }:m[1].$==11?{
    $:1,
    $0:Doc.TextView(View.Map(Global.String,m[1].$1.RView()))
   }:(console.warn("Content hole filled with attribute data",name),null):null;
  }
  holes=[];
  updates$1=[];
  attrs=[];
  afterRender=[];
  fw=new Dictionary.New$5();
  e=Enumerator.Get(fillWith);
  try
  {
   while(e.MoveNext())
    {
     x=e.Current();
     fw.set_Item(x.$0,x);
    }
  }
  finally
  {
   if("Dispose"in e)
    e.Dispose();
  }
  els=DomUtility.ChildrenArray(el);
  DomUtility.IterSelector(el,"[ws-hole]",function(p)
  {
   var m,doc,name;
   name=p.getAttribute("ws-hole");
   p.removeAttribute("ws-hole");
   while(p.hasChildNodes())
    p.removeChild(p.lastChild);
   m=tryGetAsDoc(name);
   m!=null&&m.$==1?(doc=m.$0,Docs$1.LinkElement(p,doc.docNode),holes.push(DocElemNode.New(Attrs$1.Empty(p),doc.docNode,null,p,Fresh.Int(),null)),updates$1.push(doc.updates)):void 0;
  });
  DomUtility.IterSelector(el,"[ws-replace]",function(e$1)
  {
   var m,doc,p,after,before,o;
   m=tryGetAsDoc(e$1.getAttribute("ws-replace"));
   m!=null&&m.$==1?(doc=m.$0,p=e$1.parentNode,after=Global.document.createTextNode(""),p.replaceChild(after,e$1),before=Docs$1.InsertBeforeDelim(after,doc.docNode),o=Arrays.tryFindIndex(function(y)
   {
    return e$1===y;
   },els),o==null?void 0:Arrays.set(els,o.$0,doc.docNode),holes.push(DocElemNode.New(Attrs$1.Empty(p),doc.docNode,{
    $:1,
    $0:[before,after]
   },p,Fresh.Int(),null)),updates$1.push(doc.updates)):void 0;
  });
  DomUtility.IterSelector(el,"[ws-attr]",function(e$1)
  {
   var name,m,o;
   name=e$1.getAttribute("ws-attr");
   e$1.removeAttribute("ws-attr");
   m=(o=null,[fw.TryGetValue(name,{
    get:function()
    {
     return o;
    },
    set:function(v)
    {
     o=v;
    }
   }),o]);
   m[0]?m[1].$==3?addAttr(e$1,m[1].$1):console.warn("Attribute hole filled with non-attribute data",name):void 0;
  });
  DomUtility.IterSelector(el,"[ws-on]",function(e$1)
  {
   addAttr(e$1,AttrProxy.Concat(Arrays.choose(function(x$1)
   {
    var a,m,o;
    a=Strings.SplitChars(x$1,[":"],1);
    m=(o=null,[fw.TryGetValue(Arrays.get(a,1),{
     get:function()
     {
      return o;
     },
     set:function(v)
     {
      o=v;
     }
    }),o]);
    return m[0]?m[1].$==4?{
     $:1,
     $0:AttrModule.Handler(Arrays.get(a,0),m[1].$1)
    }:(console.warn("Event hole on"+Arrays.get(a,0)+" filled with non-event data",Arrays.get(a,1)),null):null;
   },Strings.SplitChars(e$1.getAttribute("ws-on"),[" "],1))));
   e$1.removeAttribute("ws-on");
  });
  DomUtility.IterSelector(el,"[ws-onafterrender]",function(e$1)
  {
   var name,m,o;
   name=e$1.getAttribute("ws-onafterrender");
   m=(o=null,[fw.TryGetValue(name,{
    get:function()
    {
     return o;
    },
    set:function(v)
    {
     o=v;
    }
   }),o]);
   m[0]?m[1].$==5?(e$1.removeAttribute("ws-onafterrender"),addAttr(e$1,AttrModule.OnAfterRender(m[1].$1))):console.warn("onafterrender hole filled with non-onafterrender data",name):void 0;
  });
  DomUtility.IterSelector(el,"[ws-var]",function(e$1)
  {
   var name,m,o;
   name=e$1.getAttribute("ws-var");
   e$1.removeAttribute("ws-var");
   m=(o=null,[fw.TryGetValue(name,{
    get:function()
    {
     return o;
    },
    set:function(v)
    {
     o=v;
    }
   }),o]);
   m[0]?m[1].$==6?addAttr(e$1,AttrModule.Value(m[1].$1)):m[1].$==7?addAttr(e$1,AttrModule.Checked(m[1].$1)):m[1].$==8?addAttr(e$1,AttrModule.IntValue(m[1].$1)):m[1].$==9?addAttr(e$1,AttrModule.IntValueUnchecked(m[1].$1)):m[1].$==10?addAttr(e$1,AttrModule.FloatValue(m[1].$1)):m[1].$==11?addAttr(e$1,AttrModule.FloatValueUnchecked(m[1].$1)):console.warn("Var hole filled with non-Var data",name):void 0;
  });
  DomUtility.IterSelector(el,"[ws-attr-holes]",function(e$1)
  {
   var re,holeAttrs,i,$3;
   re=new Global.RegExp(Docs$1.TextHoleRE(),"g");
   holeAttrs=Strings.SplitChars(e$1.getAttribute("ws-attr-holes"),[" "],1);
   e$1.removeAttribute("ws-attr-holes");
   for(i=0,$3=holeAttrs.length-1;i<=$3;i++)(function()
   {
    var m,lastIndex,$4,finalText,value,s,s$1,s$2,s$3,attrName,s$4,res,textBefore;
    attrName=Arrays.get(holeAttrs,i);
    s$4=e$1.getAttribute(attrName);
    m=null;
    lastIndex=0;
    res=[];
    while(m=re.exec(s$4),m!==null)
     {
      textBefore=Slice.string(s$4,{
       $:1,
       $0:lastIndex
      },{
       $:1,
       $0:re.lastIndex-Arrays.get(m,0).length-1
      });
      lastIndex=re.lastIndex;
      res.push([textBefore,Arrays.get(m,1)]);
     }
    finalText=Slice.string(s$4,{
     $:1,
     $0:lastIndex
    },null);
    re.lastIndex=0;
    value=Arrays.foldBack(function($5,$6)
    {
     return(function(t)
     {
      var textBefore$1,holeName;
      textBefore$1=t[0];
      holeName=t[1];
      return function(t$1)
      {
       var textAfter,views,holeContent,m$1,o;
       textAfter=t$1[0];
       views=t$1[1];
       holeContent=(m$1=(o=null,[fw.TryGetValue(holeName,{
        get:function()
        {
         return o;
        },
        set:function(v)
        {
         o=v;
        }
       }),o]),m$1[0]?m$1[1].$==1?{
        $:0,
        $0:m$1[1].$1
       }:m$1[1].$==2?{
        $:1,
        $0:m$1[1].$1
       }:m$1[1].$==6?{
        $:1,
        $0:m$1[1].$1.RView()
       }:m$1[1].$==7?{
        $:1,
        $0:View.Map(Global.String,m$1[1].$1.RView())
       }:m$1[1].$==8?{
        $:1,
        $0:View.Map(function(i$1)
        {
         return i$1.get_Input();
        },m$1[1].$1.RView())
       }:m$1[1].$==9?{
        $:1,
        $0:View.Map(Global.String,m$1[1].$1.RView())
       }:m$1[1].$==10?{
        $:1,
        $0:View.Map(function(i$1)
        {
         return i$1.get_Input();
        },m$1[1].$1.RView())
       }:m$1[1].$==11?{
        $:1,
        $0:View.Map(Global.String,m$1[1].$1.RView())
       }:(console.warn("Attribute value hole filled with non-text data",holeName),{
        $:0,
        $0:""
       }):{
        $:0,
        $0:""
       });
       return holeContent.$==1?[textBefore$1,new List.T({
        $:1,
        $0:textAfter===""?holeContent.$0:View.Map(function(s$5)
        {
         return s$5+textAfter;
        },holeContent.$0),
        $1:views
       })]:[textBefore$1+holeContent.$0+textAfter,views];
      };
     }($5))($6);
    },res,[finalText,List.T.Empty]);
    return addAttr(e$1,value[1].$==1?value[1].$1.$==1?value[1].$1.$1.$==1?value[1].$1.$1.$1.$==0?(s=value[0],AttrModule.Dynamic(attrName,View.Map3(function(v1,v2,v3)
    {
     return s+v1+v2+v3;
    },value[1].$0,value[1].$1.$0,value[1].$1.$1.$0))):(s$1=value[0],AttrModule.Dynamic(attrName,View.Map(function(vs)
    {
     return s$1+Strings.concat("",vs);
    },View.Sequence(value[1])))):(s$2=value[0],AttrModule.Dynamic(attrName,View.Map2(function(v1,v2)
    {
     return s$2+v1+v2;
    },value[1].$0,value[1].$1.$0))):value[0]===""?AttrModule.Dynamic(attrName,value[1].$0):(s$3=value[0],AttrModule.Dynamic(attrName,View.Map(function(v)
    {
     return s$3+v;
    },value[1].$0))):AttrProxy.Create(attrName,value[0]));
   }());
  });
  docTreeNode=Runtime.DeleteEmptyFields({
   Els:els,
   Dirty:true,
   Holes:holes,
   Attrs:attrs,
   Render:($1=afterRender.length==0?null:{
    $:1,
    $0:function(el$1)
    {
     Arrays.iter(function(f)
     {
      f(el$1);
     },afterRender);
    }
   },$1?$1.$0:void 0)
  },["Render"]);
  updates=Array.TreeReduce(View.Const(),View.Map2Unit,updates$1);
  return els&&Arrays.length(els)===1&&(Arrays.get(els,0)instanceof Node&&(Unchecked.Equals(Arrays.get(els,0).nodeType,Node.ELEMENT_NODE)&&($2=Arrays.get(els,0),true)))?Elt.TreeNode(docTreeNode,updates):Doc.Mk({
   $:6,
   $0:docTreeNode
  },updates);
 };
 Doc.Template=function(els,fillWith)
 {
  return Doc.ChildrenTemplate(Doc.FakeRoot(els),fillWith);
 };
 Doc.TextView=function(txt)
 {
  var node;
  node=Docs$1.CreateTextNode();
  return Doc.Mk({
   $:4,
   $0:node
  },View.Map(function(t)
  {
   Docs$1.UpdateTextNode(node,t);
  },txt));
 };
 Doc.RunReplaceById=function(id,tr)
 {
  var m;
  m=DomUtility.Doc().getElementById(id);
  Unchecked.Equals(m,null)?Operators.FailWith("invalid id: "+id):tr.ReplaceInDom(m);
 };
 Doc.RunById=function(id,tr)
 {
  var m;
  m=DomUtility.Doc().getElementById(id);
  Unchecked.Equals(m,null)?Operators.FailWith("invalid id: "+id):Doc.Run(m,tr);
 };
 Doc.Run=function(parent,doc)
 {
  var d;
  d=doc.docNode;
  Docs$1.LinkElement(parent,d);
  View.Sink(Mailbox.StartProcessor(Docs$1.PerformAnimatedUpdate(Docs$1.CreateRunState(parent,d),d)),doc.updates);
 };
 Doc.RunPrependById=function(id,doc)
 {
  var m;
  m=DomUtility.Doc().getElementById(id);
  Unchecked.Equals(m,null)?Operators.FailWith("invalid id: "+id):Doc.RunPrepend(m,doc);
 };
 Doc.RunPrepend=function(parent,doc)
 {
  var rdelim;
  rdelim=Global.document.createTextNode("");
  parent.insertBefore(rdelim,parent.firstChild);
  Doc.RunBefore(rdelim,doc);
 };
 Doc.RunAppendById=function(id,doc)
 {
  var m;
  m=DomUtility.Doc().getElementById(id);
  Unchecked.Equals(m,null)?Operators.FailWith("invalid id: "+id):Doc.RunAppend(m,doc);
 };
 Doc.RunAppend=function(parent,doc)
 {
  var rdelim;
  rdelim=Global.document.createTextNode("");
  parent.appendChild(rdelim);
  Doc.RunBefore(rdelim,doc);
 };
 Doc.RunAfterById=function(id,doc)
 {
  var m;
  m=DomUtility.Doc().getElementById(id);
  Unchecked.Equals(m,null)?Operators.FailWith("invalid id: "+id):Doc.RunAfter(m,doc);
 };
 Doc.RunAfter=function(ldelim,doc)
 {
  var rdelim;
  rdelim=Global.document.createTextNode("");
  ldelim.parentNode.insertBefore(rdelim,ldelim.nextSibling);
  Doc.RunBetween(ldelim,rdelim,doc);
 };
 Doc.RunBeforeById=function(id,doc)
 {
  var m;
  m=DomUtility.Doc().getElementById(id);
  Unchecked.Equals(m,null)?Operators.FailWith("invalid id: "+id):Doc.RunBefore(m,doc);
 };
 Doc.RunBefore=function(rdelim,doc)
 {
  var ldelim;
  ldelim=Global.document.createTextNode("");
  rdelim.parentNode.insertBefore(ldelim,rdelim);
  Doc.RunBetween(ldelim,rdelim,doc);
 };
 Doc.RunBetween=function(ldelim,rdelim,doc)
 {
  Docs$1.LinkPrevElement(rdelim,doc.docNode);
  View.Sink(Mailbox.StartProcessor(Docs$1.PerformAnimatedUpdate(Docs$1.CreateDelimitedRunState(ldelim,rdelim,doc.docNode),doc.docNode)),doc.updates);
 };
 Doc.Async=function(a)
 {
  return Doc.EmbedView(View.MapAsync(Global.id,View.Const(a)));
 };
 Doc.BindView=function(f,view)
 {
  return Doc.EmbedView(View.Map(f,view));
 };
 Doc.EmbedView=function(view)
 {
  var node;
  node=Docs$1.CreateEmbedNode();
  return Doc.Mk({
   $:2,
   $0:node
  },View.Map(Global.ignore,View.Bind(function(doc)
  {
   Docs$1.UpdateEmbedNode(node,doc.docNode);
   return doc.updates;
  },view)));
 };
 Doc.Static=function(el)
 {
  return Elt.New(el,Attrs$1.EmptyAttr(),Doc.Empty());
 };
 Doc.Mk=function(node,updates)
 {
  return new Doc.New(node,updates);
 };
 Doc.New=Runtime.Ctor(function(docNode,updates)
 {
  this.docNode=docNode;
  this.updates=updates;
 },Doc);
 Elt=Next.Elt=Runtime.Class({
  AddClass:function(cls)
  {
   return this.elt.className+=" "+cls;
  },
  ClearHoles:function()
  {
   var m;
   m=this.docNode$1;
   m!=null&&m.$==6?m.$0.Holes=[]:void 0;
  },
  AddHole:function(h)
  {
   var m;
   m=this.docNode$1;
   m!=null&&m.$==6?m.$0.Holes.push(h):void 0;
  },
  SetStyle:function(style,value)
  {
   this.elt.style[style]=value;
  },
  HasClass:function(cls)
  {
   return(new Global.RegExp("(\\s|^)"+cls+"(\\s|$)")).test(this.elt.className);
  },
  RemoveClass:function(cls)
  {
   this.elt.className=this.elt.className.replace(new Global.RegExp("(?:(\\s|^)"+cls+")+(\\s|$)","g"),"$1");
  },
  GetProperty:function(name)
  {
   return this.elt[name];
  },
  SetProperty:function(name,value)
  {
   this.elt[name]=value;
  },
  RemoveAttribute:function(name)
  {
   this.elt.removeAttribute(name);
  },
  HasAttribute:function(name)
  {
   return this.elt.hasAttribute(name);
  },
  GetAttribute:function(name)
  {
   return this.elt.getAttribute(name);
  },
  SetAttribute:function(name,value)
  {
   this.elt.setAttribute(name,value);
  },
  SetText:function(v)
  {
   var m;
   m=this.docNode$1;
   m!=null&&m.$==1?m.$0.Children=null:m!=null&&m.$==6?(m.$0.Els=[],this.ClearHoles()):Operators.FailWith("Invalid docNode in Elt");
   this.rvUpdates.set_Value(View.Const());
   this.elt.textContent=v;
  },
  GetText:function()
  {
   return this.elt.textContent;
  },
  SetValue:function(v)
  {
   this.elt.value=v;
  },
  GetValue:function()
  {
   return this.elt.value;
  },
  Id:function()
  {
   return this.elt.id;
  },
  Html:function()
  {
   return this.elt.outerHTML;
  },
  Clear:function()
  {
   var m;
   m=this.docNode$1;
   m!=null&&m.$==1?m.$0.Children=null:m!=null&&m.$==6?(m.$0.Els=[],this.ClearHoles()):Operators.FailWith("Invalid docNode in Elt");
   this.rvUpdates.set_Value(View.Const());
   while(this.elt.hasChildNodes())
    this.elt.removeChild(this.elt.firstChild);
  },
  Prepend:function(doc)
  {
   var m,e,m$1,after,before;
   m=this.docNode$1;
   m!=null&&m.$==1?(e=m.$0,e.Children={
    $:0,
    $0:doc.docNode,
    $1:e.Children
   },Docs$1.InsertDoc(this.elt,doc.docNode,(m$1=this.elt.firstChild,Unchecked.Equals(m$1,null)?null:m$1))):m!=null&&m.$==6?(after=this.elt.insertBefore(Global.document.createTextNode(""),this.elt.firstChild),before=Docs$1.InsertBeforeDelim(after,doc.docNode),this.AddHole(DocElemNode.New(Attrs$1.Empty(this.elt),doc.docNode,{
    $:1,
    $0:[before,after]
   },this.elt,Fresh.Int(),null))):Operators.FailWith("Invalid docNode in Elt");
   this.rvUpdates.set_Value(View.Map2Unit(this.rvUpdates.c,doc.updates));
  },
  Append:function(doc)
  {
   var m,e,after,before;
   m=this.docNode$1;
   m!=null&&m.$==1?(e=m.$0,e.Children={
    $:0,
    $0:e.Children,
    $1:doc.docNode
   },Docs$1.InsertDoc(this.elt,doc.docNode,null)):m!=null&&m.$==6?(after=this.elt.appendChild(Global.document.createTextNode("")),before=Docs$1.InsertBeforeDelim(after,doc.docNode),this.AddHole(DocElemNode.New(Attrs$1.Empty(this.elt),doc.docNode,{
    $:1,
    $0:[before,after]
   },this.elt,Fresh.Int(),null))):Operators.FailWith("Invalid docNode in Elt");
   this.rvUpdates.set_Value(View.Map2Unit(this.rvUpdates.c,doc.updates));
  },
  ToUpdater:function()
  {
   var m,$1;
   return new EltUpdater.New((m=this.docNode$1,m!=null&&m.$==1?Runtime.DeleteEmptyFields({
    Els:[this.elt],
    Dirty:true,
    Holes:[],
    Attrs:[[this.elt,m.$0.Attr]],
    Render:($1=null,$1?$1.$0:void 0)
   },["Render"]):m!=null&&m.$==6?m.$0:Operators.FailWith("Invalid docNode in Elt")),this.updates$1,this.elt,this.rvUpdates,Var.Create$1([]));
  },
  OnAfterRenderView:function(view,cb)
  {
   var $this,id;
   $this=this;
   id=Fresh.Id();
   this.Append(Doc.BindView(function(x)
   {
    $this.elt[id]=x;
    return Doc.Empty();
   },view));
   return this.OnAfterRender(function(e)
   {
    cb(e,e[id]);
   });
  },
  OnAfterRender:function(cb)
  {
   var m,e,m$1,f,e$1,m$2,f$1;
   m=this.docNode$1;
   m!=null&&m.$==1?(e=m.$0,Runtime.SetOptional(e,"Render",(m$1=Runtime.GetOptional(e.Render),m$1!=null&&m$1.$==1?(f=m$1.$0,{
    $:1,
    $0:function(el)
    {
     f(el);
     cb(el);
    }
   }):{
    $:1,
    $0:cb
   }))):m!=null&&m.$==6?(e$1=m.$0,Runtime.SetOptional(e$1,"Render",(m$2=Runtime.GetOptional(e$1.Render),m$2!=null&&m$2.$==1?(f$1=m$2.$0,{
    $:1,
    $0:function(el)
    {
     f$1(el);
     cb(el);
    }
   }):{
    $:1,
    $0:cb
   }))):Operators.FailWith("Invalid docNode in Elt");
   return this;
  },
  onView:function(ev,view,cb)
  {
   var cb$1;
   cb$1=cb(this.elt);
   this.elt.addEventListener(ev,function(ev$1)
   {
    return View.Get(cb$1(ev$1),view);
   },false);
   return this;
  },
  on:function(ev,cb)
  {
   this.elt.addEventListener(ev,function(d)
   {
    return cb(d.target,d);
   },false);
   return this;
  }
 },Doc,Elt);
 Elt.TreeNode=function(tree,updates)
 {
  var rvUpdates,x;
  function f(t)
  {
   return t[1];
  }
  rvUpdates=Updates.Create(updates);
  return new Elt.New$1({
   $:6,
   $0:tree
  },View.Map2Unit((x=Arrays.map(function(x$1)
  {
   return Attrs$1.Updates(f(x$1));
  },tree.Attrs),Array.TreeReduce(View.Const(),View.Map2Unit,x)),rvUpdates.v),Arrays.get(tree.Els,0),rvUpdates);
 };
 Elt.New=function(el,attr$1,children)
 {
  var node,rvUpdates;
  node=Docs$1.CreateElemNode(el,attr$1,children.docNode);
  rvUpdates=Updates.Create(children.updates);
  return new Elt.New$1({
   $:1,
   $0:node
  },View.Map2Unit(Attrs$1.Updates(node.Attr),rvUpdates.v),el,rvUpdates);
 };
 Elt.New$1=Runtime.Ctor(function(docNode,updates,elt,rvUpdates)
 {
  Doc.New.call(this,docNode,updates);
  this.docNode$1=docNode;
  this.updates$1=updates;
  this.elt=elt;
  this.rvUpdates=rvUpdates;
 },Elt);
 EltUpdater=Client.EltUpdater=Runtime.Class({
  RemoveAllUpdated:function()
  {
   this.treeNode.Holes=this.origHoles;
   Var.Set(this.holeUpdates,[]);
  },
  RemoveUpdated:function(doc)
  {
   var m,k;
   function p($1)
   {
    return $1===k?false:true;
   }
   m=doc.docNode;
   m!=null&&m.$==1?(k=m.$0.ElKey,this.treeNode.Holes=Arrays.filter(function(h)
   {
    return h.ElKey!==k;
   },this.treeNode.Holes),Var.Set(this.holeUpdates,Arrays.filter(function($1)
   {
    return p($1[0]);
   },this.holeUpdates.c))):Operators.FailWith("DocUpdater.RemoveUpdated expects a single element node");
  },
  AddUpdated:function(doc)
  {
   var m,e,hu;
   m=doc.docNode;
   m!=null&&m.$==1?(e=m.$0,this.treeNode.Holes=this.treeNode.Holes.concat([e]),hu=this.holeUpdates.c,hu.push([e.ElKey,doc.updates]),Var.Set(this.holeUpdates,hu)):Operators.FailWith("DocUpdater.AddUpdated expects a single element node");
  },
  ClearHoles:function()
  {
   this.origHoles=[];
   this.treeNode.Holes=[];
   Var.Set(this.holeUpdates,[]);
  },
  AddHole:function(h)
  {
   this.origHoles.push(h);
   this.treeNode.Holes=this.treeNode.Holes.concat([h]);
  }
 },Elt,EltUpdater);
 EltUpdater.New=Runtime.Ctor(function(treeNode,updates,elt,rvUpdates,holeUpdates)
 {
  var f,g,d;
  function m(t)
  {
   return t[1];
  }
  Elt.New$1.call(this,{
   $:6,
   $0:treeNode
  },View.Map2Unit(updates,View.BindInner((f=function(a)
  {
   return Arrays.map(m,a);
  },(g=(d=View.Const(),function(a)
  {
   return Array.TreeReduce(d,View.Map2Unit,a);
  }),function(x)
  {
   return g(f(x));
  })),holeUpdates.v)),elt,rvUpdates);
  this.treeNode=treeNode;
  this.holeUpdates=holeUpdates;
  this.origHoles=this.treeNode.Holes;
 },EltUpdater);
 SC$8.$cctor=function()
 {
  SC$8.$cctor=Global.ignore;
  SC$8.LoadedTemplates=new Dictionary.New$5();
  SC$8.TextHoleRE="\\${([^}]+)}";
 };
 Flow=Next.Flow=Runtime.Class({
  get_Render:function()
  {
   return this.render;
  }
 },WebSharper.Obj,Flow);
 Flow.New=Runtime.Ctor(function(define)
 {
  Flow.New$1.call(this,function(_var)
  {
   return function(cont)
   {
    return Var.Set(_var,define(cont));
   };
  });
 },Flow);
 Flow.New$1=Runtime.Ctor(function(render)
 {
  this.render=render;
 },Flow);
 Flow.get_Do=function()
 {
  return new FlowBuilder.New();
 };
 Flow.Static=function(doc)
 {
  return new Flow.New$1(function(_var)
  {
   return function(cont)
   {
    Var.Set(_var,doc);
    return cont();
   };
  });
 };
 Flow.Define=function(f)
 {
  return new Flow.New(function(x)
  {
   return f(function(a)
   {
    x(a);
   });
  });
 };
 Flow.Embed=function(fl)
 {
  var _var;
  _var=Var.Create$1(Doc.Empty());
  ((fl.get_Render())(_var))(Global.ignore);
  return Doc.EmbedView(_var.v);
 };
 Flow.Return=function(x)
 {
  return new Flow.New$1(function()
  {
   return function(cont)
   {
    return cont(x);
   };
  });
 };
 Flow.Bind=function(m,k)
 {
  return new Flow.New$1(function(_var)
  {
   return function(cont)
   {
    return((m.get_Render())(_var))(function(r)
    {
     ((k(r).get_Render())(_var))(cont);
    });
   };
  });
 };
 Flow.Map=function(f,x)
 {
  return new Flow.New$1(function(_var)
  {
   return function(cont)
   {
    return((x.get_Render())(_var))(function(r)
    {
     cont(f(r));
    });
   };
  });
 };
 FlowBuilder=Next.FlowBuilder=Runtime.Class({
  ReturnFrom:Global.id,
  Return:function(value)
  {
   return Flow.Return(value);
  },
  Bind:function(comp,func)
  {
   return Flow.Bind(comp,func);
  }
 },WebSharper.Obj,FlowBuilder);
 FlowBuilder.New=Runtime.Ctor(function()
 {
 },FlowBuilder);
 attr=Html.attr=Runtime.Class({},WebSharper.Obj,attr);
 attr.New=Runtime.Ctor(function()
 {
 },attr);
 Route.Append=function(a,a$1)
 {
  return{
   $:0,
   $0:AppendList.Append(a.$0,a$1.$0),
   $1:Map.FoldBack(function($1,$2,$3)
   {
    return $3.Add($1,$2);
   },a.$1,a$1.$1)
  };
 };
 Route.FromList=function(xs,q)
 {
  return{
   $:0,
   $0:AppendList.FromArray(Arrays.ofList(xs)),
   $1:q
  };
 };
 Route.ToList=function(a)
 {
  return[List.ofArray(AppendList.ToArray(a.$0)),a.$1];
 };
 Route.SameHash=function(a,b)
 {
  return Route.NoHash(a)===Route.NoHash(b);
 };
 Route.MakeHash=function(a)
 {
  var query,path;
  query=a.$1;
  path=Strings.concat("/",Arrays.map(function(x)
  {
   return Global.encodeURIComponent(x);
  },AppendList.ToArray(a.$0)));
  return query.get_IsEmpty()?path:path+"?"+Strings.concat("&",Seq.map(function(a$1)
  {
   var a$2;
   a$2=Operators.KeyValue(a$1);
   return Global.encodeURIComponent(a$2[0])+"="+Global.encodeURIComponent(a$2[1]);
  },query));
 };
 Route.ParseHash=function(hash)
 {
  var hash$1,p,m,path;
  hash$1=Route.NoHash(hash);
  p=(m=hash$1.indexOf("?"),m===-1?[hash$1,""]:[Slice.string(hash$1,null,{
   $:1,
   $0:m-1
  }),Slice.string(hash$1,{
   $:1,
   $0:m+1
  },null)]);
  path=p[0];
  return{
   $:0,
   $0:AppendList.FromArray(path===""?[]:Arrays.map(function(x)
   {
    return Global.decodeURIComponent(x);
   },Strings.SplitChars(path,["/"],0))),
   $1:Map.OfArray(Arrays.map(function(s)
   {
    var m$1;
    m$1=s.indexOf("=");
    return m$1===-1?[Global.decodeURIComponent(s),""]:[Global.decodeURIComponent(Slice.string(s,null,{
     $:1,
     $0:m$1-1
    })),Global.decodeURIComponent(Slice.string(s,{
     $:1,
     $0:m$1+1
    },null))];
   },Strings.SplitChars(p[1],["&"],0)))
  };
 };
 Route.NoHash=function(s)
 {
  return Strings.StartsWith(s,"#")?s.substring(1):s;
 };
 State.New=function(Bodies,CurrentRoute,CurrentSite,Selection)
 {
  return{
   Bodies:Bodies,
   CurrentRoute:CurrentRoute,
   CurrentSite:CurrentSite,
   Selection:Selection
  };
 };
 Routing.Install=function(key,a)
 {
  var va,currentRoute,state,siteTrie,glob,m,site;
  function a$1(prefix,a$2)
  {
   var id;
   id=a$2.$0;
   return a$2.$1({
    UpdateRoute:function(r)
    {
     Routing.OnInternalSiteUpdate(state,id,prefix,r);
    }
   });
  }
  function parseRoute(route)
  {
   return Trie.Lookup(siteTrie,(Route.ToList(route))[0]);
  }
  function updateRoute(route)
  {
   var m$1;
   m$1=parseRoute(route);
   m$1.$==1?void 0:Routing.OnGlobalRouteChange(state,m$1.$0,Route.FromList(m$1.$1,new FSharpMap.New([])));
  }
  va=a.$0;
  currentRoute=Routing.InstallMap({
   Des:function(t)
   {
    return Route.FromList(t[0],t[1]);
   },
   Ser:Route.ToList
  });
  state=State.New(void 0,currentRoute,0,void 0);
  siteTrie=Trie.Map(function($1)
  {
   return function($2)
   {
    return a$1($1,$2);
   };
  },a.$1);
  state.Bodies=Routing.ComputeBodies(siteTrie);
  glob=(m=parseRoute(currentRoute.c),m.$==0?(site=m.$0,(state.CurrentSite=site.RouteId,Var.Create$1(site.RouteValue))):Var.Create$1(va!=null&&va.$==1?va.$0:Operators.FailWith("Site.Install fails on empty site")));
  state.Selection=glob;
  View.Sink(function(site$1)
  {
   Routing.OnSelectSite(state,key(site$1));
  },glob.v);
  updateRoute(currentRoute.c);
  View.Sink(updateRoute,currentRoute.v);
  return glob;
 };
 Routing.OnSelectSite=function(state,a)
 {
  var id;
  id=a.$0;
  state.CurrentSite!==id?(state.CurrentSite=id,state.Bodies.get_Item(id).OnSelect()):void 0;
 };
 Routing.OnInternalSiteUpdate=function(state,ix,prefix,rest)
 {
  if(state.CurrentSite===ix)
   Routing.SetCurrentRoute(state,Route.Append(Route.FromList(prefix,new FSharpMap.New([])),rest));
 };
 Routing.OnGlobalRouteChange=function(state,site,rest)
 {
  state.CurrentSite!==site.RouteId?(state.CurrentSite=site.RouteId,Var.Set(state.Selection,site.RouteValue)):void 0;
  site.OnRouteChanged(rest);
 };
 Routing.SetCurrentRoute=function(state,route)
 {
  if(!Unchecked.Equals(state.CurrentRoute.c,route))
   Var.Set(state.CurrentRoute,route);
 };
 Routing.ComputeBodies=function(trie)
 {
  var d;
  d=new Dictionary.New$5();
  Arrays.iter(function(body)
  {
   d.set_Item(body.RouteId,body);
  },Trie.ToArray(trie));
  return d;
 };
 Routing.MergeRouters=function(sites)
 {
  var sites$1,merged,value;
  sites$1=Array.ofSeqNonCopying(sites);
  merged=Trie.Merge(Seq.map(function(a)
  {
   return a.$1;
  },sites$1));
  value=Seq.tryPick(function(a)
  {
   return a.$0;
  },sites$1);
  return merged!=null&&merged.$==1?{
   $:0,
   $0:value,
   $1:merged.$0
  }:Operators.FailWith("Invalid Site.Merge: need more prefix disambiguation");
 };
 Routing.DefineRoute=function(r,init,render)
 {
  var state,id,site;
  state=Var.Create$1(init);
  id=Fresh.Int();
  site=render({
   $:0,
   $0:id
  },state);
  return{
   $:0,
   $0:{
    $:1,
    $0:site
   },
   $1:Trie.Leaf({
    $:0,
    $0:id,
    $1:function(ctx)
    {
     View.Sink(function(va)
     {
      ctx.UpdateRoute(Routing.DoLink(r,va));
     },state.v);
     return{
      OnRouteChanged:function(route)
      {
       Var.Set(state,Routing.DoRoute(r,route));
      },
      OnSelect:function()
      {
       ctx.UpdateRoute(Routing.DoLink(r,state.c));
      },
      RouteId:id,
      RouteValue:site
     };
    }
   })
  };
 };
 Routing.DoLink=function(map,va)
 {
  var t;
  t=map.Ser(va);
  return Route.FromList(t[0],t[1]);
 };
 Routing.DoRoute=function(map,route)
 {
  return map.Des(Route.ToList(route));
 };
 Routing.InstallMap=function(rt)
 {
  var _var;
  function same(a,b)
  {
   return Unchecked.Equals(rt.Ser(a),rt.Ser(b));
  }
  function cur()
  {
   return rt.Des(Route.ToList(Route.ParseHash(Global.location.hash)));
  }
  function set(value)
  {
   if(!same(_var.c,value))
    Var.Set(_var,value);
  }
  function onUpdate(evt)
  {
   return set(cur());
  }
  _var=Var.Create$1(cur());
  Global.onpopstate=onUpdate;
  Global.onhashchange=onUpdate;
  View.Sink(function(loc)
  {
   var ha,t;
   ha=Route.MakeHash((t=rt.Ser(loc),Route.FromList(t[0],t[1])));
   !Route.SameHash(Global.location.hash,ha)?Global.location.replace("#"+ha):void 0;
  },_var.v);
  return _var;
 };
 RouteMap.Install=function(map)
 {
  return Routing.InstallMap(map);
 };
 RouteMap.Create=function(ser,des)
 {
  function f(t)
  {
   return t[0];
  }
  return{
   Des:function(x)
   {
    return des(f(x));
   },
   Ser:function(x)
   {
    return[ser(x),new FSharpMap.New([])];
   }
  };
 };
 RouteMap.CreateWithQuery=function(ser,des)
 {
  return{
   Des:des,
   Ser:ser
  };
 };
 Router.Route=function(r,init,render)
 {
  return Routing.DefineRoute(r,init,render);
 };
 Router.Prefix=function(prefix,a)
 {
  return{
   $:0,
   $0:a.$0,
   $1:Trie.Prefix(prefix,a.$1)
  };
 };
 Router.Merge=function(sites)
 {
  return Routing.MergeRouters(sites);
 };
 Router.Install=function(key,site)
 {
  return Routing.Install(key,site);
 };
 Router.Dir=function(prefix,sites)
 {
  return Router.Prefix(prefix,Router.Merge(sites));
 };
 MousePosSt.New=function(Active,PosV)
 {
  return{
   Active:Active,
   PosV:PosV
  };
 };
 MouseBtnSt.New=function(Active,Left,Middle,Right)
 {
  return{
   Active:Active,
   Left:Left,
   Middle:Middle,
   Right:Right
  };
 };
 Mouse.get_MousePressed=function()
 {
  Input.ActivateButtonListener();
  return View.Apply(View.Apply(View.Apply(View.Const(Runtime.Curried3(function(l,m,r)
  {
   return l||m||r;
  })),Input.MouseBtnSt$1().Left.v),Input.MouseBtnSt$1().Middle.v),Input.MouseBtnSt$1().Right.v);
 };
 Mouse.get_RightPressed=function()
 {
  Input.ActivateButtonListener();
  return Input.MouseBtnSt$1().Right.v;
 };
 Mouse.get_MiddlePressed=function()
 {
  Input.ActivateButtonListener();
  return Input.MouseBtnSt$1().Middle.v;
 };
 Mouse.get_LeftPressed=function()
 {
  Input.ActivateButtonListener();
  return Input.MouseBtnSt$1().Left.v;
 };
 Mouse.get_Position=function()
 {
  !Input.MousePosSt$1().Active?(Global.document.addEventListener("mousemove",function(evt)
  {
   Var.Set(Input.MousePosSt$1().PosV,[evt.clientX,evt.clientY]);
  },false),Input.MousePosSt$1().Active=true):void 0;
  return Input.MousePosSt$1().PosV.v;
 };
 KeyListenerSt.New=function(KeysPressed,KeyListenerActive,LastPressed)
 {
  return{
   KeysPressed:KeysPressed,
   KeyListenerActive:KeyListenerActive,
   LastPressed:LastPressed
  };
 };
 Keyboard.IsPressed=function(key)
 {
  function p(x)
  {
   return x===key;
  }
  Input.ActivateKeyListener();
  return View.Map(function(l)
  {
   return List.exists(p,l);
  },Input.KeyListenerState().KeysPressed.v);
 };
 Keyboard.get_LastPressed=function()
 {
  Input.ActivateKeyListener();
  return Input.KeyListenerState().LastPressed.v;
 };
 Keyboard.get_KeysPressed=function()
 {
  Input.ActivateKeyListener();
  return Input.KeyListenerState().KeysPressed.v;
 };
 Input.ActivateKeyListener=function()
 {
  SC$9.$cctor();
  return SC$9.ActivateKeyListener;
 };
 Input.KeyListenerState=function()
 {
  SC$9.$cctor();
  return SC$9.KeyListenerState;
 };
 Input.ActivateButtonListener=function()
 {
  SC$9.$cctor();
  return SC$9.ActivateButtonListener;
 };
 Input.MouseBtnSt$1=function()
 {
  SC$9.$cctor();
  return SC$9.MouseBtnSt;
 };
 Input.MousePosSt$1=function()
 {
  SC$9.$cctor();
  return SC$9.MousePosSt;
 };
 SC$9.$cctor=function()
 {
  SC$9.$cctor=Global.ignore;
  function buttonListener(evt,down)
  {
   var m;
   m=evt.button;
   return m===0?Var.Set(Input.MouseBtnSt$1().Left,down):m===1?Var.Set(Input.MouseBtnSt$1().Middle,down):m===2?Var.Set(Input.MouseBtnSt$1().Right,down):null;
  }
  SC$9.MousePosSt=MousePosSt.New(false,Var.Create$1([0,0]));
  SC$9.MouseBtnSt=MouseBtnSt.New(false,Var.Create$1(false),Var.Create$1(false),Var.Create$1(false));
  SC$9.ActivateButtonListener=!Input.MouseBtnSt$1().Active?(Input.MouseBtnSt$1().Active=true,Global.document.addEventListener("mousedown",function(evt)
  {
   return buttonListener(evt,true);
  },false),Global.document.addEventListener("mouseup",function(evt)
  {
   return buttonListener(evt,false);
  },false)):null;
  SC$9.KeyListenerState=KeyListenerSt.New(Var.Create$1(List.T.Empty),false,Var.Create$1(-1));
  SC$9.ActivateKeyListener=!Input.KeyListenerState().KeyListenerActive?($(Global.document).keydown(function(evt)
  {
   var keyCode,xs;
   keyCode=evt.which;
   Var.Set(Input.KeyListenerState().LastPressed,keyCode);
   xs=Input.KeyListenerState().KeysPressed.c;
   return!List.exists(function(x)
   {
    return x===keyCode;
   },xs)?Var.Set(Input.KeyListenerState().KeysPressed,List.append(xs,List.ofArray([keyCode]))):null;
  }),void $(Global.document).keyup(function(evt)
  {
   var keyCode;
   function p(x)
   {
    return x!==keyCode;
   }
   keyCode=evt.which;
   return Var.Update(Input.KeyListenerState().KeysPressed,function(l)
   {
    return List.filter(p,l);
   });
  })):null;
 };
}());

//# sourceMappingURL=WebSharper.UI.Next.map