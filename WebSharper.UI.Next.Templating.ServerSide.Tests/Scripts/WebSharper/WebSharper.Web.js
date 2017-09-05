(function()
{
 "use strict";
 var Global,WebSharper,Json,Provider,Web,Control,FSharpInlineControl,InlineControl,IntelliFactory,Runtime,Collections,Dictionary,FSharpMap,Unchecked,Operators,Arrays,FSharpSet,BalancedTree,List,Enumerator,Map,Seq;
 Global=window;
 WebSharper=Global.WebSharper=Global.WebSharper||{};
 Json=WebSharper.Json=WebSharper.Json||{};
 Provider=Json.Provider=Json.Provider||{};
 Web=WebSharper.Web=WebSharper.Web||{};
 Control=Web.Control=Web.Control||{};
 FSharpInlineControl=Web.FSharpInlineControl=Web.FSharpInlineControl||{};
 InlineControl=Web.InlineControl=Web.InlineControl||{};
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
 Collections=WebSharper&&WebSharper.Collections;
 Dictionary=Collections&&Collections.Dictionary;
 FSharpMap=Collections&&Collections.FSharpMap;
 Unchecked=WebSharper&&WebSharper.Unchecked;
 Operators=WebSharper&&WebSharper.Operators;
 Arrays=WebSharper&&WebSharper.Arrays;
 FSharpSet=Collections&&Collections.FSharpSet;
 BalancedTree=Collections&&Collections.BalancedTree;
 List=WebSharper&&WebSharper.List;
 Enumerator=WebSharper&&WebSharper.Enumerator;
 Map=Collections&&Collections.Map;
 Seq=WebSharper&&WebSharper.Seq;
 Provider.DecodeStringDictionary=Runtime.Curried3(function(decEl,$1,o)
 {
  var d,decEl$1,k;
  d=new Dictionary.New$5();
  decEl$1=decEl();
  for(var k$1 in o)if(function(k$2)
  {
   d.Add(k$2,decEl$1(o[k$2]));
   return false;
  }(k$1))
   break;
  return d;
 });
 Provider.DecodeStringMap=Runtime.Curried3(function(decEl,$1,o)
 {
  var m,decEl$1,k;
  m=[new FSharpMap.New([])];
  decEl$1=decEl();
  for(var k$1 in o)if(function(k$2)
  {
   var v;
   m[0]=(v=decEl$1(o[k$2]),m[0].Add(k$2,v));
   return false;
  }(k$1))
   break;
  return m[0];
 });
 Provider.DecodeArray=function(decEl)
 {
  return Provider.EncodeArray(decEl);
 };
 Provider.DecodeUnion=function(t,discr,cases)
 {
  return function()
  {
   return function(x)
   {
    var tag,tagName,o,r,k;
    function p(name,a$1)
    {
     return name===tagName;
    }
    function a(from,to,dec,kind)
    {
     var r$1;
     if(from===null)
      {
       r$1=(dec(null))(x);
       to?delete r$1[discr]:void 0;
       o.$0=r$1;
      }
     else
      if(Unchecked.Equals(kind,0))
       o[from]=(dec(null))(x[to]);
      else
       if(Unchecked.Equals(kind,1))
        o[from]=x.hasOwnProperty(to)?{
         $:1,
         $0:(dec(null))(x[to])
        }:null;
       else
        Operators.FailWith("Invalid field option kind");
    }
    if(typeof x==="object"&&x!=null)
     {
      o=t===void 0?{}:new t();
      if(Unchecked.Equals(typeof discr,"string"))
       tag=(tagName=x[discr],Arrays.findIndex(function($1)
       {
        return p($1[0],$1[1]);
       },cases));
      else
       {
        r=[void 0];
        for(var k$1 in discr)if(function(k$2)
        {
         return x.hasOwnProperty(k$2)&&(r[0]=discr[k$2],true);
        }(k$1))
         break;
        tag=r[0];
       }
      o.$=tag;
      Arrays.iter(function($1)
      {
       return a($1[0],$1[1],$1[2],$1[3]);
      },(Arrays.get(cases,tag))[1]);
      return o;
     }
    else
     return x;
   };
  };
 };
 Provider.DecodeRecord=function(t,fields)
 {
  return function()
  {
   return function(x)
   {
    var o;
    function a(name,dec,kind)
    {
     if(Unchecked.Equals(kind,0))
     {
      if(x.hasOwnProperty(name))
       o[name]=(dec(null))(x[name]);
      else
       Operators.FailWith("Missing mandatory field: "+name);
     }
     else
      if(Unchecked.Equals(kind,1))
       o[name]=x.hasOwnProperty(name)?{
        $:1,
        $0:(dec(null))(x[name])
       }:null;
      else
       if(Unchecked.Equals(kind,2))
       {
        if(x.hasOwnProperty(name))
         o[name]=(dec(null))(x[name]);
       }
       else
        Operators.FailWith("Invalid field option kind");
    }
    o=t===void 0?{}:new t();
    Arrays.iter(function($1)
    {
     return a($1[0],$1[1],$1[2]);
    },fields);
    return o;
   };
  };
 };
 Provider.DecodeSet=Runtime.Curried3(function(decEl,$1,a)
 {
  return new FSharpSet.New$1(BalancedTree.OfSeq(Arrays.map(decEl(),a)));
 });
 Provider.DecodeList=Runtime.Curried3(function(decEl,$1,a)
 {
  var e;
  e=decEl();
  return List.init(Arrays.length(a),function(i)
  {
   return e(Arrays.get(a,i));
  });
 });
 Provider.DecodeDateTime=Runtime.Curried3(function($1,$2,x)
 {
  return(new Global.Date(x)).getTime();
 });
 Provider.DecodeTuple=function(decs)
 {
  return Provider.EncodeTuple(decs);
 };
 Provider.EncodeStringDictionary=Runtime.Curried3(function(encEl,$1,d)
 {
  var o,e,e$1,a;
  o={};
  e=encEl();
  e$1=Enumerator.Get(d);
  try
  {
   while(e$1.MoveNext())
    {
     a=Operators.KeyValue(e$1.Current());
     o[a[0]]=e(a[1]);
    }
  }
  finally
  {
   if("Dispose"in e$1)
    e$1.Dispose();
  }
  return o;
 });
 Provider.EncodeStringMap=Runtime.Curried3(function(encEl,$1,m)
 {
  var o,e;
  o={};
  e=encEl();
  Map.Iterate(function(k,v)
  {
   o[k]=e(v);
  },m);
  return o;
 });
 Provider.EncodeSet=Runtime.Curried3(function(encEl,$1,s)
 {
  var a,e;
  a=[];
  e=encEl();
  Seq.iter(function(x)
  {
   a.push(e(x));
  },s);
  return a;
 });
 Provider.EncodeArray=Runtime.Curried3(function(encEl,$1,a)
 {
  return Arrays.map(encEl(),a);
 });
 Provider.EncodeUnion=function(a,discr,cases)
 {
  return function()
  {
   return function(x)
   {
    var o,p;
    function a$1(from,to,enc,kind)
    {
     var record,k,m;
     if(from===null)
      {
       record=(enc(null))(x.$0);
       for(var k$1 in record)if(function(f)
       {
        o[f]=record[f];
        return false;
       }(k$1))
        break;
      }
     else
      if(Unchecked.Equals(kind,0))
       o[to]=(enc(null))(x[from]);
      else
       if(Unchecked.Equals(kind,1))
        {
         m=x[from];
         m==null?void 0:o[to]=(enc(null))(m.$0);
        }
       else
        Operators.FailWith("Invalid field option kind");
    }
    return typeof x==="object"&&x!=null?(o={},(p=Arrays.get(cases,x.$),(Unchecked.Equals(typeof discr,"string")?o[discr]=p[0]:void 0,Arrays.iter(function($1)
    {
     return a$1($1[0],$1[1],$1[2],$1[3]);
    },p[1]),o))):x;
   };
  };
 };
 Provider.EncodeRecord=function(a,fields)
 {
  return function()
  {
   return function(x)
   {
    var o;
    function a$1(name,enc,kind)
    {
     var m;
     if(Unchecked.Equals(kind,0))
      o[name]=(enc(null))(x[name]);
     else
      if(Unchecked.Equals(kind,1))
       {
        m=x[name];
        m==null?void 0:o[name]=(enc(null))(m.$0);
       }
      else
       if(Unchecked.Equals(kind,2))
       {
        if(x.hasOwnProperty(name))
         o[name]=(enc(null))(x[name]);
       }
       else
        Operators.FailWith("Invalid field option kind");
    }
    o={};
    Arrays.iter(function($1)
    {
     return a$1($1[0],$1[1],$1[2]);
    },fields);
    return o;
   };
  };
 };
 Provider.EncodeList=Runtime.Curried3(function(encEl,$1,l)
 {
  var a,e;
  a=[];
  e=encEl();
  List.iter(function(x)
  {
   a.push(e(x));
  },l);
  return a;
 });
 Provider.EncodeDateTime=Runtime.Curried3(function($1,$2,x)
 {
  return(new Global.Date(x)).toISOString();
 });
 Provider.EncodeTuple=Runtime.Curried3(function(encs,$1,args)
 {
  return Arrays.map2(function($2,$3)
  {
   return($2(null))($3);
  },encs,args);
 });
 Provider.Id=Runtime.Curried3(function($1,$2,x)
 {
  return x;
 });
 Control=Web.Control=Runtime.Class({
  Body:function()
  {
   return this.get_Body();
  }
 },null,Control);
 FSharpInlineControl=Web.FSharpInlineControl=Runtime.Class({
  get_Body:function()
  {
   return Arrays.fold(function($1,$2)
   {
    return $1[$2];
   },Global,this.funcName).apply(null,this.args);
  }
 },Control,FSharpInlineControl);
 InlineControl=Web.InlineControl=Runtime.Class({
  get_Body:function()
  {
   return Arrays.fold(function($1,$2)
   {
    return $1[$2];
   },Global,this.funcName).apply(null,this.args);
  }
 },Control,InlineControl);
}());

//# sourceMappingURL=WebSharper.Web.map