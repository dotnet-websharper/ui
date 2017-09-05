(function()
{
 "use strict";
 var Global,WebSharper,Collections,BalancedTree,Tree,Pair,MapUtil,FSharpMap,Map,FSharpSet,Set,ListEnumerator,List,ResizeArray,LinkedListEnumerator,LinkedList,Grouping,FsComparer,ProjectionComparer,CompoundComparer,ReverseComparer,OrderedEnumerable,Linq,Arrays,Seq,Unchecked,List$1,IntelliFactory,Runtime,Enumerator,Operators,HashSet,Dictionary,Nullable;
 Global=window;
 WebSharper=Global.WebSharper=Global.WebSharper||{};
 Collections=WebSharper.Collections=WebSharper.Collections||{};
 BalancedTree=Collections.BalancedTree=Collections.BalancedTree||{};
 Tree=BalancedTree.Tree=BalancedTree.Tree||{};
 Pair=Collections.Pair=Collections.Pair||{};
 MapUtil=Collections.MapUtil=Collections.MapUtil||{};
 FSharpMap=Collections.FSharpMap=Collections.FSharpMap||{};
 Map=Collections.Map=Collections.Map||{};
 FSharpSet=Collections.FSharpSet=Collections.FSharpSet||{};
 Set=Collections.Set=Collections.Set||{};
 ListEnumerator=Collections.ListEnumerator=Collections.ListEnumerator||{};
 List=Collections.List=Collections.List||{};
 ResizeArray=Collections.ResizeArray=Collections.ResizeArray||{};
 LinkedListEnumerator=Collections.LinkedListEnumerator=Collections.LinkedListEnumerator||{};
 LinkedList=Collections.LinkedList=Collections.LinkedList||{};
 Grouping=WebSharper.Grouping=WebSharper.Grouping||{};
 FsComparer=WebSharper.FsComparer=WebSharper.FsComparer||{};
 ProjectionComparer=WebSharper.ProjectionComparer=WebSharper.ProjectionComparer||{};
 CompoundComparer=WebSharper.CompoundComparer=WebSharper.CompoundComparer||{};
 ReverseComparer=WebSharper.ReverseComparer=WebSharper.ReverseComparer||{};
 OrderedEnumerable=WebSharper.OrderedEnumerable=WebSharper.OrderedEnumerable||{};
 Linq=WebSharper.Linq=WebSharper.Linq||{};
 Arrays=WebSharper&&WebSharper.Arrays;
 Seq=WebSharper&&WebSharper.Seq;
 Unchecked=WebSharper&&WebSharper.Unchecked;
 List$1=WebSharper&&WebSharper.List;
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
 Enumerator=WebSharper&&WebSharper.Enumerator;
 Operators=WebSharper&&WebSharper.Operators;
 HashSet=Collections&&Collections.HashSet;
 Dictionary=Collections&&Collections.Dictionary;
 Nullable=WebSharper&&WebSharper.Nullable;
 Tree.New=function(Node,Left,Right,Height,Count)
 {
  return{
   Node:Node,
   Left:Left,
   Right:Right,
   Height:Height,
   Count:Count
  };
 };
 BalancedTree.TryFind=function(v,t)
 {
  var x;
  x=(BalancedTree.Lookup(v,t))[0];
  return x==null?null:{
   $:1,
   $0:x.Node
  };
 };
 BalancedTree.Contains=function(v,t)
 {
  return!((BalancedTree.Lookup(v,t))[0]==null);
 };
 BalancedTree.Add=function(x,t)
 {
  return BalancedTree.Put(function($1,$2)
  {
   return $2;
  },x,t);
 };
 BalancedTree.Remove=function(k,src)
 {
  var p,t,spine,d;
  p=BalancedTree.Lookup(k,src);
  t=p[0];
  spine=p[1];
  return t==null?src:t.Right==null?BalancedTree.Rebuild(spine,t.Left):t.Left==null?BalancedTree.Rebuild(spine,t.Right):BalancedTree.Rebuild(spine,(d=Arrays.ofSeq(Seq.append(BalancedTree.Enumerate(false,t.Left),BalancedTree.Enumerate(false,t.Right))),BalancedTree.Build(d,0,d.length-1)));
 };
 BalancedTree.Put=function(combine,k,t)
 {
  var p,t$1;
  p=BalancedTree.Lookup(k,t);
  t$1=p[0];
  return t$1==null?BalancedTree.Rebuild(p[1],BalancedTree.Branch(k,null,null)):BalancedTree.Rebuild(p[1],BalancedTree.Branch(combine(t$1.Node,k),t$1.Left,t$1.Right));
 };
 BalancedTree.Rebuild=function(spine,t)
 {
  var t$1,i,$1,m,x,l,m$1,x$1,r,m$2;
  function h(x$2)
  {
   return x$2==null?0:x$2.Height;
  }
  t$1=t;
  for(i=0,$1=Arrays.length(spine)-1;i<=$1;i++){
   t$1=(m=Arrays.get(spine,i),m[0]?(x=m[1],(l=m[2],h(t$1)>h(l)+1?h(t$1.Left)===h(t$1.Right)+1?(m$1=t$1.Left,BalancedTree.Branch(m$1.Node,BalancedTree.Branch(x,l,m$1.Left),BalancedTree.Branch(t$1.Node,m$1.Right,t$1.Right))):BalancedTree.Branch(t$1.Node,BalancedTree.Branch(x,l,t$1.Left),t$1.Right):BalancedTree.Branch(x,l,t$1))):(x$1=m[1],(r=m[2],h(t$1)>h(r)+1?h(t$1.Right)===h(t$1.Left)+1?(m$2=t$1.Right,BalancedTree.Branch(m$2.Node,BalancedTree.Branch(t$1.Node,t$1.Left,m$2.Left),BalancedTree.Branch(x$1,m$2.Right,r))):BalancedTree.Branch(t$1.Node,t$1.Left,BalancedTree.Branch(x$1,t$1.Right,r)):BalancedTree.Branch(x$1,t$1,r))));
  }
  return t$1;
 };
 BalancedTree.Lookup=function(k,t)
 {
  var spine,t$1,loop,m;
  spine=[];
  t$1=t;
  loop=true;
  while(loop)
   if(t$1==null)
    loop=false;
   else
    {
     m=Unchecked.Compare(k,t$1.Node);
     m===0?loop=false:m===1?(spine.unshift([true,t$1.Node,t$1.Left]),t$1=t$1.Right):(spine.unshift([false,t$1.Node,t$1.Right]),t$1=t$1.Left);
    }
  return[t$1,spine];
 };
 BalancedTree.OfSeq=function(data)
 {
  var a;
  a=Arrays.ofSeq(Seq.distinct(data));
  Arrays.sortInPlace(a);
  return BalancedTree.Build(a,0,a.length-1);
 };
 BalancedTree.Build=function(data,min,max)
 {
  var center,left,right;
  return max-min+1<=0?null:(center=(min+max)/2>>0,(left=BalancedTree.Build(data,min,center-1),(right=BalancedTree.Build(data,center+1,max),BalancedTree.Branch(Arrays.get(data,center),left,right))));
 };
 BalancedTree.Enumerate=function(flip,t)
 {
  function gen(t$1,spine)
  {
   var t$2;
   while(true)
    if(t$1==null)
     return spine.$==1?{
      $:1,
      $0:[spine.$0[0],[spine.$0[1],spine.$1]]
     }:null;
    else
     if(flip)
      {
       t$2=t$1;
       t$1=t$2.Right;
       spine=new List$1.T({
        $:1,
        $0:[t$2.Node,t$2.Left],
        $1:spine
       });
      }
     else
      {
       t$2=t$1;
       t$1=t$2.Left;
       spine=new List$1.T({
        $:1,
        $0:[t$2.Node,t$2.Right],
        $1:spine
       });
      }
  }
  return Seq.unfold(function($1)
  {
   return gen($1[0],$1[1]);
  },[t,List$1.T.Empty]);
 };
 BalancedTree.Branch=function(node,left,right)
 {
  var a,b;
  return Tree.New(node,left,right,1+(a=left==null?0:left.Height,(b=right==null?0:right.Height,Unchecked.Compare(a,b)===1?a:b)),1+(left==null?0:left.Count)+(right==null?0:right.Count));
 };
 Pair=Collections.Pair=Runtime.Class({
  Equals:function(other)
  {
   return Unchecked.Equals(this.Key,other.Key);
  },
  GetHashCode:function()
  {
   return Unchecked.Hash(this.Key);
  },
  CompareTo0:function(other)
  {
   return Unchecked.Compare(this.Key,other.Key);
  }
 },null,Pair);
 Pair.New=function(Key,Value)
 {
  return new Pair({
   Key:Key,
   Value:Value
  });
 };
 MapUtil.fromSeq=function(s)
 {
  var a;
  a=Arrays.ofSeq(Seq.delay(function()
  {
   return Seq.collect(function(m)
   {
    return[Pair.New(m[0],m[1])];
   },Seq.distinctBy(function(t)
   {
    return t[0];
   },s));
  }));
  Arrays.sortInPlace(a);
  return BalancedTree.Build(a,0,a.length-1);
 };
 FSharpMap=Collections.FSharpMap=Runtime.Class({
  Equals:function(other)
  {
   return this.get_Count()===other.get_Count()&&Seq.forall2(Unchecked.Equals,this,other);
  },
  GetHashCode:function()
  {
   return Unchecked.Hash(Arrays.ofSeq(this));
  },
  GetEnumerator$1:function()
  {
   return Enumerator.Get(Seq.map(function(kv)
   {
    return{
     K:kv.Key,
     V:kv.Value
    };
   },BalancedTree.Enumerate(false,this.tree)));
  },
  TryFind:function(k)
  {
   var o;
   o=BalancedTree.TryFind(Pair.New(k,void 0),this.tree);
   return o==null?null:{
    $:1,
    $0:o.$0.Value
   };
  },
  Remove:function(k)
  {
   return new FSharpMap.New$1(BalancedTree.Remove(Pair.New(k,void 0),this.tree));
  },
  get_Item:function(k)
  {
   var m;
   m=this.TryFind(k);
   return m==null?Operators.FailWith("The given key was not present in the dictionary."):m.$0;
  },
  get_IsEmpty:function()
  {
   return this.tree==null;
  },
  get_Count:function()
  {
   var tree;
   tree=this.tree;
   return tree==null?0:tree.Count;
  },
  ContainsKey:function(k)
  {
   return BalancedTree.Contains(Pair.New(k,void 0),this.tree);
  },
  Add:function(k,v)
  {
   return new FSharpMap.New$1(BalancedTree.Add(Pair.New(k,v),this.tree));
  },
  get_Tree:function()
  {
   return this.tree;
  },
  GetEnumerator:function()
  {
   return this.GetEnumerator$1();
  },
  GetEnumerator0:function()
  {
   return this.GetEnumerator$1();
  },
  CompareTo0:function(other)
  {
   return Seq.compareWith(Unchecked.Compare,this,other);
  }
 },WebSharper.Obj,FSharpMap);
 FSharpMap.New=Runtime.Ctor(function(s)
 {
  FSharpMap.New$1.call(this,MapUtil.fromSeq(s));
 },FSharpMap);
 FSharpMap.New$1=Runtime.Ctor(function(tree)
 {
  this.tree=tree;
 },FSharpMap);
 Map.Map=function(f,m)
 {
  return new FSharpMap.New$1(BalancedTree.OfSeq(Seq.map(function(kv)
  {
   return Pair.New(kv.Key,f(kv.Key,kv.Value));
  },BalancedTree.Enumerate(false,m.get_Tree()))));
 };
 Map.TryPick=function(f,m)
 {
  return Seq.tryPick(function(kv)
  {
   return f(kv.K,kv.V);
  },m);
 };
 Map.TryFindKey=function(f,m)
 {
  return Seq.tryPick(function(kv)
  {
   return f(kv.K,kv.V)?{
    $:1,
    $0:kv.K
   }:null;
  },m);
 };
 Map.TryFind=function(k,m)
 {
  return m.TryFind(k);
 };
 Map.ToSeq=function(m)
 {
  return Seq.map(function(kv)
  {
   return[kv.Key,kv.Value];
  },BalancedTree.Enumerate(false,m.get_Tree()));
 };
 Map.Pick=function(f,m)
 {
  return Seq.pick(function(kv)
  {
   return f(kv.K,kv.V);
  },m);
 };
 Map.Partition=function(f,m)
 {
  var p,data,data$1;
  p=Arrays.partition(function(kv)
  {
   return f(kv.Key,kv.Value);
  },Arrays.ofSeq(BalancedTree.Enumerate(false,m.get_Tree())));
  return[new FSharpMap.New$1((data=p[0],BalancedTree.Build(data,0,data.length-1))),new FSharpMap.New$1((data$1=p[1],BalancedTree.Build(data$1,0,data$1.length-1)))];
 };
 Map.OfArray=function(a)
 {
  return new FSharpMap.New$1(BalancedTree.OfSeq(Seq.map(function($1)
  {
   return Pair.New($1[0],$1[1]);
  },a)));
 };
 Map.Iterate=function(f,m)
 {
  Seq.iter(function(kv)
  {
   f(kv.K,kv.V);
  },m);
 };
 Map.ForAll=function(f,m)
 {
  return Seq.forall(function(kv)
  {
   return f(kv.K,kv.V);
  },m);
 };
 Map.FoldBack=function(f,m,s)
 {
  return Seq.fold(function(s$1,kv)
  {
   return f(kv.Key,kv.Value,s$1);
  },s,BalancedTree.Enumerate(true,m.get_Tree()));
 };
 Map.Fold=function(f,s,m)
 {
  return Seq.fold(function(s$1,kv)
  {
   return f(s$1,kv.Key,kv.Value);
  },s,BalancedTree.Enumerate(false,m.get_Tree()));
 };
 Map.FindKey=function(f,m)
 {
  return Seq.pick(function(kv)
  {
   return f(kv.K,kv.V)?{
    $:1,
    $0:kv.K
   }:null;
  },m);
 };
 Map.Filter=function(f,m)
 {
  var d;
  return new FSharpMap.New$1((d=Arrays.ofSeq(Seq.filter(function(kv)
  {
   return f(kv.Key,kv.Value);
  },BalancedTree.Enumerate(false,m.get_Tree()))),BalancedTree.Build(d,0,d.length-1)));
 };
 Map.Exists=function(f,m)
 {
  return Seq.exists(function(kv)
  {
   return f(kv.K,kv.V);
  },m);
 };
 FSharpSet=Collections.FSharpSet=Runtime.Class({
  Equals:function(other)
  {
   return this.get_Count()===other.get_Count()&&Seq.forall2(Unchecked.Equals,this,other);
  },
  GetHashCode:function()
  {
   return -1741749453+Unchecked.Hash(Arrays.ofSeq(this));
  },
  GetEnumerator$1:function()
  {
   return Enumerator.Get(BalancedTree.Enumerate(false,this.tree));
  },
  Remove:function(v)
  {
   return new FSharpSet.New$1(BalancedTree.Remove(v,this.tree));
  },
  get_MinimumElement:function()
  {
   return Seq.head(BalancedTree.Enumerate(false,this.tree));
  },
  get_MaximumElement:function()
  {
   return Seq.head(BalancedTree.Enumerate(true,this.tree));
  },
  IsSupersetOf:function(s)
  {
   var $this;
   $this=this;
   return Seq.forall(function(a)
   {
    return $this.Contains(a);
   },s);
  },
  IsSubsetOf:function(s)
  {
   return Seq.forall(function(a)
   {
    return s.Contains(a);
   },this);
  },
  IsProperSupersetOf:function(s)
  {
   return this.IsSupersetOf(s)&&this.get_Count()>s.get_Count();
  },
  IsProperSubsetOf:function(s)
  {
   return this.IsSubsetOf(s)&&this.get_Count()<s.get_Count();
  },
  get_Tree:function()
  {
   return this.tree;
  },
  get_IsEmpty:function()
  {
   return this.tree==null;
  },
  get_Count:function()
  {
   var tree;
   tree=this.tree;
   return tree==null?0:tree.Count;
  },
  Contains:function(v)
  {
   return BalancedTree.Contains(v,this.tree);
  },
  Add:function(x)
  {
   return new FSharpSet.New$1(BalancedTree.Add(x,this.tree));
  },
  sub:function(x)
  {
   return Set.Filter(function(x$1)
   {
    return!x.Contains(x$1);
   },this);
  },
  add:function(x)
  {
   return new FSharpSet.New$1(BalancedTree.OfSeq(Seq.append(this,x)));
  },
  CompareTo0:function(other)
  {
   return Seq.compareWith(Unchecked.Compare,this,other);
  },
  GetEnumerator:function()
  {
   return this.GetEnumerator$1();
  },
  GetEnumerator0:function()
  {
   return this.GetEnumerator$1();
  }
 },WebSharper.Obj,FSharpSet);
 FSharpSet.op_Subtraction=function(x,y)
 {
  return Set.Filter(function(x$1)
  {
   return!y.Contains(x$1);
  },x);
 };
 FSharpSet.op_Addition=function(x,y)
 {
  return new FSharpSet.New$1(BalancedTree.OfSeq(Seq.append(x,y)));
 };
 FSharpSet.New=Runtime.Ctor(function(s)
 {
  FSharpSet.New$1.call(this,BalancedTree.OfSeq(s));
 },FSharpSet);
 FSharpSet.New$1=Runtime.Ctor(function(tree)
 {
  this.tree=tree;
 },FSharpSet);
 Set.Partition=function(f,a)
 {
  var p;
  p=Arrays.partition(f,Arrays.ofSeq(a));
  return[new FSharpSet.New$1(BalancedTree.OfSeq(p[0])),new FSharpSet.New$1(BalancedTree.OfSeq(p[1]))];
 };
 Set.FoldBack=function(f,a,s)
 {
  return Seq.fold(function($1,$2)
  {
   return f($2,$1);
  },s,BalancedTree.Enumerate(true,a.get_Tree()));
 };
 Set.Filter=function(f,s)
 {
  var data;
  return new FSharpSet.New$1((data=Arrays.ofSeq(Seq.filter(f,s)),BalancedTree.Build(data,0,data.length-1)));
 };
 ListEnumerator=Collections.ListEnumerator=Runtime.Class({
  get_Current:function()
  {
   return Arrays.get(this.arr,this.i);
  },
  MoveNext$1:function()
  {
   this.i=this.i+1;
   return this.i<Arrays.length(this.arr);
  },
  Dispose:Global.ignore,
  Current:function()
  {
   return Arrays.get(this.arr,this.i);
  },
  Reset:function()
  {
   Operators.FailWith("IEnumerator.Reset not supported");
  },
  Current0:function()
  {
   return Arrays.get(this.arr,this.i);
  },
  MoveNext:function()
  {
   return this.MoveNext$1();
  }
 },WebSharper.Obj,ListEnumerator);
 ListEnumerator.New=Runtime.Ctor(function(arr)
 {
  this.arr=arr;
  this.i=-1;
 },ListEnumerator);
 List=Collections.List=Runtime.Class({
  ToArray:function()
  {
   return this.arr.slice();
  },
  Reverse:function(index,count)
  {
   Arrays.reverse(this.arr,index,count);
  },
  Reverse$1:function()
  {
   this.arr.reverse();
  },
  RemoveRange:function(index,count)
  {
   ResizeArray.splice(this.arr,index,count,[]);
  },
  RemoveAt:function(x)
  {
   ResizeArray.splice(this.arr,x,1,[]);
  },
  set_Item:function(x,v)
  {
   Arrays.set(this.arr,x,v);
  },
  get_Item:function(x)
  {
   return Arrays.get(this.arr,x);
  },
  InsertRange:function(index,items)
  {
   ResizeArray.splice(this.arr,index,0,Arrays.ofSeq(items));
  },
  Insert:function(index,items)
  {
   ResizeArray.splice(this.arr,index,0,[items]);
  },
  GetRange:function(index,count)
  {
   return new List.New$3(Arrays.sub(this.arr,index,count));
  },
  get_Count:function()
  {
   return Arrays.length(this.arr);
  },
  CopyTo:function(index,target,offset,count)
  {
   Arrays.blit(this.arr,index,target,offset,count);
  },
  CopyTo$1:function(arr,offset)
  {
   this.CopyTo(0,arr,offset,this.get_Count());
  },
  CopyTo$2:function(arr)
  {
   this.CopyTo$1(arr,0);
  },
  Clear:function()
  {
   ResizeArray.splice(this.arr,0,Arrays.length(this.arr),[]);
  },
  AddRange:function(x)
  {
   var $this;
   $this=this;
   Seq.iter(function(a)
   {
    $this.Add(a);
   },x);
  },
  Add:function(x)
  {
   this.arr.push(x);
  },
  GetEnumerator:function()
  {
   return Enumerator.Get(this.arr);
  },
  GetEnumerator0:function()
  {
   return Enumerator.Get0(this.arr);
  }
 },WebSharper.Obj,List);
 List.New=Runtime.Ctor(function(el)
 {
  List.New$3.call(this,Arrays.ofSeq(el));
 },List);
 List.New$1=Runtime.Ctor(function(size)
 {
  List.New$3.call(this,[]);
 },List);
 List.New$2=Runtime.Ctor(function()
 {
  List.New$3.call(this,[]);
 },List);
 List.New$3=Runtime.Ctor(function(arr)
 {
  this.arr=arr;
 },List);
 ResizeArray.splice=function(arr,index,howMany,items)
 {
  return Global.Array.prototype.splice.apply(arr,[index,howMany].concat(items));
 };
 LinkedListEnumerator=Collections.LinkedListEnumerator=Runtime.Class({
  Reset:Global.ignore,
  Dispose:Global.ignore,
  MoveNext:function()
  {
   this.c=this.c.n;
   return!Unchecked.Equals(this.c,null);
  },
  Current0:function()
  {
   return this.c.v;
  },
  Current:function()
  {
   return this.c.v;
  }
 },WebSharper.Obj,LinkedListEnumerator);
 LinkedListEnumerator.New=Runtime.Ctor(function(l)
 {
  this.c=l;
 },LinkedListEnumerator);
 LinkedList=Collections.LinkedList=Runtime.Class({
  RemoveLast:function()
  {
   this.Remove$1(this.p);
  },
  RemoveFirst:function()
  {
   this.Remove$1(this.n);
  },
  Remove:function(value)
  {
   var node;
   node=this.Find(value);
   return Unchecked.Equals(node,null)?false:(this.Remove$1(node),true);
  },
  Remove$1:function(node)
  {
   var before,after;
   before=node.p;
   after=node.n;
   Unchecked.Equals(before,null)?this.n=after:before.n=after;
   Unchecked.Equals(after,null)?this.p=before:after.p=before;
   this.c=this.c-1;
  },
  GetEnumerator$1:function()
  {
   return new LinkedListEnumerator.New(this);
  },
  FindLast:function(value)
  {
   var node,notFound;
   node=this.p;
   notFound=true;
   while(notFound&&!Unchecked.Equals(node,null))
    if(node.v==value)
     notFound=false;
    else
     node=node.p;
   return notFound?null:node;
  },
  Find:function(value)
  {
   var node,notFound;
   node=this.n;
   notFound=true;
   while(notFound&&!Unchecked.Equals(node,null))
    if(node.v==value)
     notFound=false;
    else
     node=node.n;
   return notFound?null:node;
  },
  Contains:function(value)
  {
   var found,node;
   found=false;
   node=this.n;
   while(!Unchecked.Equals(node,null)&&!found)
    if(node.v==value)
     found=true;
    else
     node=node.n;
   return found;
  },
  Clear:function()
  {
   this.c=0;
   this.n=null;
   this.p=null;
  },
  AddLast:function(value)
  {
   var node;
   return this.c===0?(node={
    p:null,
    n:null,
    v:value
   },(this.n=node,this.p=this.n,this.c=1,node)):this.AddAfter(this.p,value);
  },
  AddFirst:function(value)
  {
   var node;
   return this.c===0?(node={
    p:null,
    n:null,
    v:value
   },(this.n=node,this.p=this.n,this.c=1,node)):this.AddBefore(this.n,value);
  },
  AddBefore:function(before,value)
  {
   var after,node;
   after=before.p;
   node={
    p:after,
    n:before,
    v:value
   };
   Unchecked.Equals(before.p,null)?this.n=node:void 0;
   before.p=node;
   !Unchecked.Equals(after,null)?after.n=node:void 0;
   this.c=this.c+1;
   return node;
  },
  AddAfter:function(after,value)
  {
   var before,node;
   before=after.n;
   node={
    p:after,
    n:before,
    v:value
   };
   Unchecked.Equals(after.n,null)?this.p=node:void 0;
   after.n=node;
   !Unchecked.Equals(before,null)?before.p=node:void 0;
   this.c=this.c+1;
   return node;
  },
  GetEnumerator:function()
  {
   return this.GetEnumerator$1();
  },
  GetEnumerator0:function()
  {
   return this.GetEnumerator$1();
  }
 },WebSharper.Obj,LinkedList);
 LinkedList.New=Runtime.Ctor(function()
 {
  LinkedList.New$1.call(this,[]);
 },LinkedList);
 LinkedList.New$1=Runtime.Ctor(function(coll)
 {
  var ie,node;
  this.c=0;
  this.n=null;
  this.p=null;
  ie=Enumerator.Get(coll);
  ie.MoveNext()?(this.n={
   p:null,
   n:null,
   v:ie.Current()
  },this.p=this.n,this.c=1):void 0;
  while(ie.MoveNext())
   {
    node={
     p:this.p,
     n:null,
     v:ie.Current()
    };
    this.p.n=node;
    this.p=node;
    this.c=this.c+1;
   }
 },LinkedList);
 Grouping=WebSharper.Grouping=Runtime.Class({
  System_Linq_IGrouping_2$get_Key:function()
  {
   return this.k;
  },
  GetEnumerator0:function()
  {
   return Enumerator.Get0(this.v);
  },
  GetEnumerator:function()
  {
   return Enumerator.Get(this.v);
  }
 },WebSharper.Obj,Grouping);
 Grouping.New=Runtime.Ctor(function(k,v)
 {
  this.k=k;
  this.v=v;
 },Grouping);
 FsComparer=WebSharper.FsComparer=Runtime.Class({
  Compare:function(x,y)
  {
   return Unchecked.Compare(x,y);
  }
 },WebSharper.Obj,FsComparer);
 FsComparer.New=Runtime.Ctor(function()
 {
 },FsComparer);
 ProjectionComparer=WebSharper.ProjectionComparer=Runtime.Class({
  Compare:function(x,y)
  {
   return this.primary.Compare(this.projection(x),this.projection(y));
  }
 },WebSharper.Obj,ProjectionComparer);
 ProjectionComparer.New=Runtime.Ctor(function(primary,projection)
 {
  this.primary=primary;
  this.projection=projection;
 },ProjectionComparer);
 CompoundComparer=WebSharper.CompoundComparer=Runtime.Class({
  Compare:function(x,y)
  {
   var m;
   m=this.primary.Compare(x,y);
   return m===0?this.secondary.Compare(x,y):m;
  }
 },WebSharper.Obj,CompoundComparer);
 CompoundComparer.New=Runtime.Ctor(function(primary,secondary)
 {
  this.primary=primary;
  this.secondary=secondary;
 },CompoundComparer);
 ReverseComparer=WebSharper.ReverseComparer=Runtime.Class({
  Compare:function(x,y)
  {
   return this.primary.Compare(this.projection(y),this.projection(x));
  }
 },WebSharper.Obj,ReverseComparer);
 ReverseComparer.New=Runtime.Ctor(function(primary,projection)
 {
  this.primary=primary;
  this.projection=projection;
 },ReverseComparer);
 OrderedEnumerable=WebSharper.OrderedEnumerable=Runtime.Class({
  GetEnumerator0:function()
  {
   return Enumerator.Get(this);
  },
  GetEnumerator:function()
  {
   var $this,a;
   $this=this;
   a=Arrays.ofSeq(this.source);
   Arrays.sortInPlaceWith(function($1,$2)
   {
    return $this.primary.Compare($1,$2);
   },a);
   return Enumerator.Get(a);
  },
  System_Linq_IOrderedEnumerable_1$CreateOrderedEnumerable:function(keySelector,secondary,descending)
  {
   return new OrderedEnumerable.New(this.source,new CompoundComparer.New(this.primary,descending?new ReverseComparer.New(secondary,keySelector):new ProjectionComparer.New(secondary,keySelector)));
  }
 },WebSharper.Obj,OrderedEnumerable);
 OrderedEnumerable.New=Runtime.Ctor(function(source,primary)
 {
  this.source=source;
  this.primary=primary;
 },OrderedEnumerable);
 Linq.ElementAtOrDefault=function(_this,index,defaultValue)
 {
  try
  {
   return Seq.nth(index,_this);
  }
  catch(m)
  {
   return defaultValue;
  }
 };
 Linq.FirstOrDefault=function(_this,defaultValue)
 {
  var e;
  e=Enumerator.Get(_this);
  try
  {
   return e.MoveNext()?e.Current():defaultValue;
  }
  finally
  {
   if("Dispose"in e)
    e.Dispose();
  }
 };
 Linq.FirstOrDefault$1=function(_this,predicate,defaultValue)
 {
  var m;
  m=Seq.tryFind(predicate,_this);
  return m==null?defaultValue:m.$0;
 };
 Linq.LastOrDefault=function(_this,predicate,defaultValue)
 {
  var m;
  m=Linq.LastPred(_this,predicate);
  return m==null?defaultValue:m.$0;
 };
 Linq.SingleOrDefault=function(_this,predicate,defaultValue)
 {
  var e,found;
  e=Enumerator.Get(_this);
  try
  {
   found=null;
   while(e.MoveNext())
    if(predicate(e.Current()))
     if(found!=null&&found.$==1)
      Operators.InvalidOp("Sequence contains more than one element");
     else
      found={
       $:1,
       $0:e.Current()
      };
   return found==null?defaultValue:found.$0;
  }
  finally
  {
   if("Dispose"in e)
    e.Dispose();
  }
 };
 Linq.Where=function(_this,predicate)
 {
  return Seq.delay(function()
  {
   return Seq.enumUsing(Enumerator.Get(_this),function(e)
   {
    var i;
    i=0;
    return Seq.enumWhile(function()
    {
     return e.MoveNext();
    },Seq.delay(function()
    {
     return Seq.append(predicate(e.Current(),i)?[e.Current()]:[],Seq.delay(function()
     {
      i=i+1;
      return[];
     }));
    }));
   });
  });
 };
 Linq.Union=function(_this,second,comparer)
 {
  var tbl,e;
  tbl=new HashSet.New(_this,comparer);
  e=Enumerator.Get(second);
  try
  {
   while(e.MoveNext())
    tbl.Add(e.Current());
  }
  finally
  {
   if("Dispose"in e)
    e.Dispose();
  }
  return tbl;
 };
 Linq.ToDictionary=function(_this,keySelector,elementSelector,comparer)
 {
  var d;
  d=new Dictionary.New$3(comparer);
  Seq.iter(function(x)
  {
   d.Add(keySelector(x),elementSelector(x));
  },_this);
  return d;
 };
 Linq.ToDictionary$1=function(_this,keySelector,comparer)
 {
  var d;
  d=new Dictionary.New$3(comparer);
  Seq.iter(function(x)
  {
   d.Add(keySelector(x),x);
  },_this);
  return d;
 };
 Linq.TakeWhile=function(_this,predicate)
 {
  return Seq.delay(function()
  {
   return Seq.enumUsing(Enumerator.Get(_this),function(e)
   {
    return Seq.enumWhile(function()
    {
     return e.MoveNext()&&predicate(e.Current());
    },Seq.delay(function()
    {
     return[e.Current()];
    }));
   });
  });
 };
 Linq.TakeWhile$1=function(_this,predicate)
 {
  return Seq.delay(function()
  {
   return Seq.enumUsing(Enumerator.Get(_this),function(e)
   {
    var i;
    i=0;
    return Seq.enumWhile(function()
    {
     return e.MoveNext()&&predicate(e.Current(),i);
    },Seq.delay(function()
    {
     i=i+1;
     return[e.Current()];
    }));
   });
  });
 };
 Linq.Take=function(_this,count)
 {
  return Seq.delay(function()
  {
   return Seq.enumUsing(Enumerator.Get(_this),function(e)
   {
    var i;
    i=0;
    return Seq.enumWhile(function()
    {
     return i<count&&e.MoveNext();
    },Seq.delay(function()
    {
     i=i+1;
     return[e.Current()];
    }));
   });
  });
 };
 Linq.Sum=function(_this)
 {
  var s;
  s=Seq.choose(function(x)
  {
   return x!=null?{
    $:1,
    $0:Nullable.get(x)
   }:null;
  },_this);
  return Seq.isEmpty(s)?null:Seq.sum(s);
 };
 Linq.SkipWhile=function(_this,predicate)
 {
  return Seq.delay(function()
  {
   return Seq.enumUsing(Enumerator.Get(_this),function(e)
   {
    var predWasTrue;
    predWasTrue=true;
    return Seq.append(Seq.enumWhile(function()
    {
     return predWasTrue&&e.MoveNext();
    },Seq.delay(function()
    {
     return!predicate(e.Current())?(predWasTrue=false,[]):[];
    })),Seq.delay(function()
    {
     return!predWasTrue?Seq.append([e.Current()],Seq.delay(function()
     {
      return Seq.enumWhile(function()
      {
       return e.MoveNext();
      },Seq.delay(function()
      {
       return[e.Current()];
      }));
     })):[];
    }));
   });
  });
 };
 Linq.SkipWhile$1=function(_this,predicate)
 {
  return Seq.delay(function()
  {
   return Seq.enumUsing(Enumerator.Get(_this),function(e)
   {
    var i,predWasTrue;
    i=0;
    predWasTrue=true;
    return Seq.append(Seq.enumWhile(function()
    {
     return predWasTrue&&e.MoveNext();
    },Seq.delay(function()
    {
     return predicate(e.Current(),i)?(i=i+1,[]):(predWasTrue=false,[]);
    })),Seq.delay(function()
    {
     return!predWasTrue?Seq.append([e.Current()],Seq.delay(function()
     {
      return Seq.enumWhile(function()
      {
       return e.MoveNext();
      },Seq.delay(function()
      {
       return[e.Current()];
      }));
     })):[];
    }));
   });
  });
 };
 Linq.Skip=function(_this,count)
 {
  return Seq.delay(function()
  {
   return Seq.enumUsing(Enumerator.Get(_this),function(e)
   {
    var i;
    i=0;
    return Seq.append(Seq.enumWhile(function()
    {
     return i<count&&e.MoveNext();
    },Seq.delay(function()
    {
     i=i+1;
     return[];
    })),Seq.delay(function()
    {
     return Seq.enumWhile(function()
     {
      return e.MoveNext();
     },Seq.delay(function()
     {
      return[e.Current()];
     }));
    }));
   });
  });
 };
 Linq.Single=function(_this,predicate)
 {
  var x;
  function f(state,cur)
  {
   return predicate(cur)?state!=null?Operators.InvalidOp("Sequence contains more than one matching element"):{
    $:1,
    $0:cur
   }:state;
  }
  x=(((Runtime.Curried3(Seq.fold))(f))(null))(_this);
  return x!=null&&x.$==1?x.$0:Operators.InvalidOp("Sequence contains no elements");
 };
 Linq.SequenceEqual=function(_this,second,comparer)
 {
  var e1,$1,e2;
  e1=Enumerator.Get(_this);
  try
  {
   e2=Enumerator.Get(_this);
   try
   {
    $1=function()
    {
     while(true)
      if(e1.MoveNext())
      {
       if(!(e2.MoveNext()&&comparer.CEquals(e1.Current(),e2.Current())))
        return false;
      }
      else
       return!e2.MoveNext();
    }();
   }
   finally
   {
    if("Dispose"in e2)
     e2.Dispose();
   }
   return $1;
  }
  finally
  {
   if("Dispose"in e1)
    e1.Dispose();
  }
 };
 Linq.SelectMany=function(_this,selector,collectionSelector)
 {
  function m(t,cs)
  {
   return Seq.map(function(c)
   {
    return collectionSelector(t,c);
   },cs);
  }
  return Seq.collect(function($1)
  {
   return m($1[0],$1[1]);
  },Seq.mapi(function(i,t)
  {
   return[t,selector(t,i)];
  },_this));
 };
 Linq.SelectMany$1=function(_this,selector,collectionSelector)
 {
  function m(t,cs)
  {
   return Seq.map(function(c)
   {
    return collectionSelector(t,c);
   },cs);
  }
  return Seq.collect(function($1)
  {
   return m($1[0],$1[1]);
  },Seq.map(function(t)
  {
   return[t,selector(t)];
  },_this));
 };
 Linq.SelectMany$2=function(_this,selector)
 {
  return Seq.concat(Seq.mapi(function($1,$2)
  {
   return selector($2,$1);
  },_this));
 };
 Linq.Select=function(_this,selector)
 {
  return Seq.mapi(function($1,$2)
  {
   return selector($2,$1);
  },_this);
 };
 Linq.Reverse=function(_this)
 {
  return Arrays.ofSeq(_this).slice().reverse();
 };
 Linq.Repeat=function(element,count)
 {
  return Seq.init(count,function()
  {
   return element;
  });
 };
 Linq.Range=function(start,count)
 {
  return Seq.init(count,function(y)
  {
   return start+y;
  });
 };
 Linq.OrderByDescending=function(_this,keySelector,comparer)
 {
  return new OrderedEnumerable.New(_this,new ReverseComparer.New(comparer,keySelector));
 };
 Linq.OrderBy=function(_this,keySelector,comparer)
 {
  return new OrderedEnumerable.New(_this,new ProjectionComparer.New(comparer,keySelector));
 };
 Linq.Min=function(_this)
 {
  var s;
  s=Seq.choose(function(x)
  {
   return x!=null?{
    $:1,
    $0:Nullable.get(x)
   }:null;
  },_this);
  return Seq.isEmpty(s)?null:Seq.min(s);
 };
 Linq.Max=function(_this)
 {
  var s;
  s=Seq.choose(function(x)
  {
   return x!=null?{
    $:1,
    $0:Nullable.get(x)
   }:null;
  },_this);
  return Seq.isEmpty(s)?null:Seq.max(s);
 };
 Linq.Last=function(_this,predicate)
 {
  var m;
  m=Linq.LastPred(_this,predicate);
  return m==null?Operators.InvalidOp("Sequence contains no matching element"):m.$0;
 };
 Linq.LastPred=function(_this,predicate)
 {
  function f(acc,elt)
  {
   return predicate(elt)?{
    $:1,
    $0:elt
   }:acc;
  }
  return(((Runtime.Curried3(Seq.fold))(f))(null))(_this);
 };
 Linq.Join=function(outer,inner,outerKeySelector,innerKeySelector,resultSelector,comparer)
 {
  return Seq.delay(function()
  {
   var t,a,e;
   t=new Dictionary.New$3(comparer);
   a=Arrays.ofSeq(Seq.delay(function()
   {
    return Seq.collect(function(o)
    {
     var k,o$1,pair;
     k=outerKeySelector(o);
     return(o$1=null,[t.TryGetValue(k,{
      get:function()
      {
       return o$1;
      },
      set:function(v)
      {
       o$1=v;
      }
     }),o$1])[0]?[]:(pair=[o,new List.New$2()],(t.Add(k,pair),[pair]));
    },outer);
   }));
   e=Enumerator.Get(inner);
   try
   {
    while(e.MoveNext())
     (function()
     {
      var i,m,o;
      i=e.Current();
      m=(o=null,[t.TryGetValue(innerKeySelector(i),{
       get:function()
       {
        return o;
       },
       set:function(v)
       {
        o=v;
       }
      }),o]);
      return m[0]?m[1][1].Add(i):null;
     }());
   }
   finally
   {
    if("Dispose"in e)
     e.Dispose();
   }
   return Arrays.ofSeq(Seq.delay(function()
   {
    return Seq.collect(function(m)
    {
     var o;
     o=m[0];
     return Seq.map(function(i)
     {
      return resultSelector(o,i);
     },m[1]);
    },a);
   }));
  });
 };
 Linq.Intersect=function(_this,second,comparer)
 {
  var t1;
  t1=new HashSet.New(_this,comparer);
  return Seq.delay(function()
  {
   var t2;
   t2=new HashSet.New$1(comparer);
   return Seq.collect(function(x)
   {
    return t1.Contains(x)&&t2.Add(x)?[x]:[];
   },second);
  });
 };
 Linq.GroupJoin=function(outer,inner,outerKeySelector,innerKeySelector,resultSelector,comparer)
 {
  return Seq.delay(function()
  {
   var t,a,e;
   t=new Dictionary.New$3(comparer);
   a=Arrays.ofSeq(Seq.delay(function()
   {
    return Seq.collect(function(o)
    {
     var k,o$1,pair;
     k=outerKeySelector(o);
     return(o$1=null,[t.TryGetValue(k,{
      get:function()
      {
       return o$1;
      },
      set:function(v)
      {
       o$1=v;
      }
     }),o$1])[0]?[]:(pair=[o,new List.New$2()],(t.Add(k,pair),[pair]));
    },outer);
   }));
   e=Enumerator.Get(inner);
   try
   {
    while(e.MoveNext())
     (function()
     {
      var i,m,o;
      i=e.Current();
      m=(o=null,[t.TryGetValue(innerKeySelector(i),{
       get:function()
       {
        return o;
       },
       set:function(v)
       {
        o=v;
       }
      }),o]);
      return m[0]?m[1][1].Add(i):null;
     }());
   }
   finally
   {
    if("Dispose"in e)
     e.Dispose();
   }
   Arrays.iteri(function(i,t$1)
   {
    return Arrays.set(a,i,resultSelector(t$1[0],t$1[1]));
   },a);
   return a;
  });
 };
 Linq.GroupBy=function(_this,keySelector,elementSelector,resultSelector,comparer)
 {
  return Seq.map(function(g)
  {
   return resultSelector(g.System_Linq_IGrouping_2$get_Key(),g);
  },Linq.GroupBy$1(_this,keySelector,elementSelector,comparer));
 };
 Linq.GroupBy$1=function(_this,keySelector,elementSelector,comparer)
 {
  return Seq.delay(function()
  {
   return Arrays.ofSeq(Seq.delay(function()
   {
    var t;
    t=new Dictionary.New$3(comparer);
    return Seq.collect(function(x)
    {
     var k,e,m,o,a;
     k=keySelector(x);
     e=elementSelector(x);
     m=(o=null,[t.TryGetValue(k,{
      get:function()
      {
       return o;
      },
      set:function(v)
      {
       o=v;
      }
     }),o]);
     return m[0]?(m[1].Add(e),[]):(a=new List.New$2(),(a.Add(e),t.set_Item(k,a),[new Grouping.New(k,a)]));
    },_this);
   }));
  });
 };
 Linq.Except=function(_this,second,comparer)
 {
  var tbl,e;
  tbl=new HashSet.New(_this,comparer);
  e=Enumerator.Get(second);
  try
  {
   while(e.MoveNext())
    tbl.Remove(e.Current());
  }
  finally
  {
   if("Dispose"in e)
    e.Dispose();
  }
  return tbl;
 };
 Linq.Distinct=function(_this,comparer)
 {
  return Seq.delay(function()
  {
   return Seq.enumUsing(Enumerator.Get(_this),function(e)
   {
    var tbl;
    tbl=new HashSet.New$1(comparer);
    return Seq.enumWhile(function()
    {
     return e.MoveNext();
    },Seq.delay(function()
    {
     return tbl.Add(e.Current())?[e.Current()]:[];
    }));
   });
  });
 };
 Linq.DefaultIfEmpty=function(_this,defaultValue)
 {
  return Seq.isEmpty(_this)?[defaultValue]:_this;
 };
 Linq.Average=function(_this)
 {
  var x,e,c,c$1;
  x=[];
  e=Enumerator.Get(_this);
  try
  {
   while(e.MoveNext())
    {
     if(c=e.Current(),c!=null)
      {
       x.push((c$1=e.Current(),Nullable.get(c$1)));
      }
    }
   return Arrays.length(x)===0?null:Seq.sum(x)/Arrays.length(x);
  }
  finally
  {
   if("Dispose"in e)
    e.Dispose();
  }
 };
}());

//# sourceMappingURL=WebSharper.Collections.map