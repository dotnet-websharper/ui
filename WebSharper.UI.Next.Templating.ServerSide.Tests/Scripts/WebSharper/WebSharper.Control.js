(function()
{
 "use strict";
 var Global,WebSharper,Control,Observer,Message,HotStream,HotStream$1,Observable,Microsoft,FSharp,Control$1,ObservableModule,Event,Event$1,DelegateEvent,DelegateEvent$1,FSharpEvent,FSharpDelegateEvent,EventModule,MailboxProcessor,IntelliFactory,Runtime,Util,List,Seq,Unchecked,Arrays,Collections,List$1,Concurrency,TimeoutException,Operators,LinkedList;
 Global=window;
 WebSharper=Global.WebSharper=Global.WebSharper||{};
 Control=WebSharper.Control=WebSharper.Control||{};
 Observer=Control.Observer=Control.Observer||{};
 Message=Observer.Message=Observer.Message||{};
 HotStream=Control.HotStream=Control.HotStream||{};
 HotStream$1=HotStream.HotStream=HotStream.HotStream||{};
 Observable=Control.Observable=Control.Observable||{};
 Microsoft=Global.Microsoft=Global.Microsoft||{};
 FSharp=Microsoft.FSharp=Microsoft.FSharp||{};
 Control$1=FSharp.Control=FSharp.Control||{};
 ObservableModule=Control$1.ObservableModule=Control$1.ObservableModule||{};
 Event=Control.Event=Control.Event||{};
 Event$1=Event.Event=Event.Event||{};
 DelegateEvent=Control.DelegateEvent=Control.DelegateEvent||{};
 DelegateEvent$1=DelegateEvent.DelegateEvent=DelegateEvent.DelegateEvent||{};
 FSharpEvent=Control.FSharpEvent=Control.FSharpEvent||{};
 FSharpDelegateEvent=Control.FSharpDelegateEvent=Control.FSharpDelegateEvent||{};
 EventModule=Control$1.EventModule=Control$1.EventModule||{};
 MailboxProcessor=Control.MailboxProcessor=Control.MailboxProcessor||{};
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
 Util=WebSharper&&WebSharper.Util;
 List=WebSharper&&WebSharper.List;
 Seq=WebSharper&&WebSharper.Seq;
 Unchecked=WebSharper&&WebSharper.Unchecked;
 Arrays=WebSharper&&WebSharper.Arrays;
 Collections=WebSharper&&WebSharper.Collections;
 List$1=Collections&&Collections.List;
 Concurrency=WebSharper&&WebSharper.Concurrency;
 TimeoutException=WebSharper&&WebSharper.TimeoutException;
 Operators=WebSharper&&WebSharper.Operators;
 LinkedList=Collections&&Collections.LinkedList;
 Message.Completed={
  $:2
 };
 Observer.New=function(f,e,c)
 {
  return{
   OnNext:f,
   OnError:e,
   OnCompleted:function()
   {
    return c();
   }
  };
 };
 Observer.Of=function(f)
 {
  return{
   OnNext:f,
   OnError:function(x)
   {
    throw x;
   },
   OnCompleted:function()
   {
    return null;
   }
  };
 };
 HotStream$1=HotStream.HotStream=Runtime.Class({
  Trigger:function(v)
  {
   this.Latest[0]={
    $:1,
    $0:v
   };
   this.Event.event.Trigger(v);
  },
  Subscribe:function(o)
  {
   this.Latest[0]!=null?o.OnNext(this.Latest[0].$0):void 0;
   return this.Event.event.Subscribe(Util.observer(function(v)
   {
    o.OnNext(v);
   }));
  }
 },null,HotStream$1);
 HotStream$1.New$1=function()
 {
  return HotStream$1.New([null],new FSharpEvent.New());
 };
 HotStream$1.New=function(Latest,Event$2)
 {
  return new HotStream$1({
   Latest:Latest,
   Event:Event$2
  });
 };
 Observable.Sequence=function(ios)
 {
  function sequence(ios$1)
  {
   return ios$1.$==1?Observable.CombineLatest(ios$1.$0,sequence(ios$1.$1),function($1,$2)
   {
    return new List.T({
     $:1,
     $0:$1,
     $1:$2
    });
   }):Observable.Return(List.T.Empty);
  }
  return sequence(List.ofSeq(ios));
 };
 Observable.Aggregate=function(io,seed,fold)
 {
  return{
   Subscribe:function(o1)
   {
    var state;
    state=[seed];
    return io.Subscribe(Observer.New(function(v)
    {
     Observable.Protect(function()
     {
      return fold(state[0],v);
     },function(s)
     {
      state[0]=s;
      o1.OnNext(s);
     },function(a)
     {
      o1.OnError(a);
     });
    },function(a)
    {
     o1.OnError(a);
    },function()
    {
     o1.OnCompleted();
    }));
   }
  };
 };
 Observable.SelectMany=function(io)
 {
  return{
   Subscribe:function(o)
   {
    var disp,d;
    function dispose()
    {
     disp[0]();
     d.Dispose();
    }
    disp=[Global.ignore];
    d=io.Subscribe(Util.observer(function(o1)
    {
     var d$1;
     d$1=o1.Subscribe(Util.observer(function(v)
     {
      o.OnNext(v);
     }));
     disp[0]=function()
     {
      disp[0]();
      d$1.Dispose();
     };
    }));
    return{
     Dispose:function()
     {
      return dispose();
     }
    };
   }
  };
 };
 Observable.Switch=function(io)
 {
  return{
   Subscribe:function(o)
   {
    var index,disp;
    index=[0];
    disp=[null];
    return io.Subscribe(Util.observer(function(o1)
    {
     var currentIndex;
     index[0]++;
     disp[0]!=null?disp[0].$0.Dispose():void 0;
     currentIndex=index[0];
     disp[0]={
      $:1,
      $0:o1.Subscribe(Util.observer(function(v)
      {
       if(currentIndex===index[0])
        o.OnNext(v);
      }))
     };
    }));
   }
  };
 };
 Observable.CombineLatest=function(io1,io2,f)
 {
  return{
   Subscribe:function(o)
   {
    var lv1,lv2,d1,d2;
    function update()
    {
     var $1,$2,v1,v2;
     $1=lv1[0];
     $2=lv2[0];
     $1!=null&&$1.$==1?$2!=null&&$2.$==1?(v1=$1.$0,v2=$2.$0,Observable.Protect(function()
     {
      return f(v1,v2);
     },function(a)
     {
      o.OnNext(a);
     },function(a)
     {
      o.OnError(a);
     })):void 0:void 0;
    }
    function dispose()
    {
     d1.Dispose();
     d2.Dispose();
    }
    lv1=[null];
    lv2=[null];
    d1=io1.Subscribe(Observer.New(function(x)
    {
     lv1[0]={
      $:1,
      $0:x
     };
     update();
    },Global.ignore,Global.ignore));
    d2=io2.Subscribe(Observer.New(function(y)
    {
     lv2[0]={
      $:1,
      $0:y
     };
     update();
    },Global.ignore,Global.ignore));
    return{
     Dispose:function()
     {
      return dispose();
     }
    };
   }
  };
 };
 Observable.Range=function(start,count)
 {
  return{
   Subscribe:function(o)
   {
    var i,$1;
    function dispose()
    {
    }
    for(i=start,$1=start+count;i<=$1;i++)o.OnNext(i);
    return{
     Dispose:function()
     {
      return dispose();
     }
    };
   }
  };
 };
 Observable.Concat=function(io1,io2)
 {
  return{
   Subscribe:function(o)
   {
    var innerDisp,outerDisp;
    function d()
    {
     innerDisp[0]!=null?innerDisp[0].$0.Dispose():void 0;
     outerDisp.Dispose();
    }
    innerDisp=[null];
    outerDisp=io1.Subscribe(Observer.New(function(a)
    {
     o.OnNext(a);
    },Global.ignore,function()
    {
     innerDisp[0]={
      $:1,
      $0:io2.Subscribe(o)
     };
    }));
    return{
     Dispose:function()
     {
      return d();
     }
    };
   }
  };
 };
 Observable.Merge=function(io1,io2)
 {
  return{
   Subscribe:function(o)
   {
    var completed1,completed2,disp1,disp2;
    function dispose()
    {
     disp1.Dispose();
     disp2.Dispose();
    }
    completed1=[false];
    completed2=[false];
    disp1=io1.Subscribe(Observer.New(function(a)
    {
     o.OnNext(a);
    },Global.ignore,function()
    {
     completed1[0]=true;
     completed1[0]&&completed2[0]?o.OnCompleted():void 0;
    }));
    disp2=io2.Subscribe(Observer.New(function(a)
    {
     o.OnNext(a);
    },Global.ignore,function()
    {
     completed2[0]=true;
     completed1[0]&&completed2[0]?o.OnCompleted():void 0;
    }));
    return{
     Dispose:function()
     {
      return dispose();
     }
    };
   }
  };
 };
 Observable.Drop=function(count,io)
 {
  return{
   Subscribe:function(o1)
   {
    var index;
    index=[0];
    return io.Subscribe(Observer.New(function(v)
    {
     index[0]++;
     index[0]>count?o1.OnNext(v):void 0;
    },function(a)
    {
     o1.OnError(a);
    },function()
    {
     o1.OnCompleted();
    }));
   }
  };
 };
 Observable.Choose=function(f,io)
 {
  return{
   Subscribe:function(o1)
   {
    return io.Subscribe(Observer.New(function(v)
    {
     function a(a$1)
     {
      o1.OnNext(a$1);
     }
     Observable.Protect(function()
     {
      return f(v);
     },function(o)
     {
      if(o==null)
       ;
      else
       a(o.$0);
     },function(a$1)
     {
      o1.OnError(a$1);
     });
    },function(a)
    {
     o1.OnError(a);
    },function()
    {
     o1.OnCompleted();
    }));
   }
  };
 };
 Observable.Filter=function(f,io)
 {
  return{
   Subscribe:function(o1)
   {
    return io.Subscribe(Observer.New(function(v)
    {
     function a(a$1)
     {
      o1.OnNext(a$1);
     }
     Observable.Protect(function()
     {
      return f(v)?{
       $:1,
       $0:v
      }:null;
     },function(o)
     {
      if(o==null)
       ;
      else
       a(o.$0);
     },function(a$1)
     {
      o1.OnError(a$1);
     });
    },function(a)
    {
     o1.OnError(a);
    },function()
    {
     o1.OnCompleted();
    }));
   }
  };
 };
 Observable.Map=function(f,io)
 {
  return{
   Subscribe:function(o1)
   {
    return io.Subscribe(Observer.New(function(v)
    {
     Observable.Protect(function()
     {
      return f(v);
     },function(a)
     {
      o1.OnNext(a);
     },function(a)
     {
      o1.OnError(a);
     });
    },function(a)
    {
     o1.OnError(a);
    },function()
    {
     o1.OnCompleted();
    }));
   }
  };
 };
 Observable.Protect=function(f,succeed,fail)
 {
  var m;
  try
  {
   m={
    $:0,
    $0:f()
   };
  }
  catch(e)
  {
   m={
    $:1,
    $0:e
   };
  }
  return m.$==1?fail(m.$0):succeed(m.$0);
 };
 Observable.Never=function()
 {
  return{
   Subscribe:function()
   {
    function dispose()
    {
    }
    return{
     Dispose:function()
     {
      return dispose();
     }
    };
   }
  };
 };
 Observable.Return=function(x)
 {
  return{
   Subscribe:function(o)
   {
    function dispose()
    {
    }
    o.OnNext(x);
    o.OnCompleted();
    return{
     Dispose:function()
     {
      return dispose();
     }
    };
   }
  };
 };
 Observable.Of=function(f)
 {
  return{
   Subscribe:function(o)
   {
    var dispose;
    dispose=f(function(x)
    {
     o.OnNext(x);
    });
    return{
     Dispose:function()
     {
      return dispose();
     }
    };
   }
  };
 };
 ObservableModule.Split=function(f,e)
 {
  return[Observable.Choose(function(x)
  {
   var m;
   m=f(x);
   return m.$==0?{
    $:1,
    $0:m.$0
   }:null;
  },e),Observable.Choose(function(x)
  {
   var m;
   m=f(x);
   return m.$==1?{
    $:1,
    $0:m.$0
   }:null;
  },e)];
 };
 ObservableModule.Scan=function(fold,seed,e)
 {
  return{
   Subscribe:function(o1)
   {
    var state;
    state=[seed];
    return e.Subscribe(Observer.New(function(v)
    {
     Observable.Protect(function()
     {
      return fold(state[0],v);
     },function(s)
     {
      state[0]=s;
      o1.OnNext(s);
     },function(a)
     {
      o1.OnError(a);
     });
    },function(a)
    {
     o1.OnError(a);
    },function()
    {
     o1.OnCompleted();
    }));
   }
  };
 };
 ObservableModule.Partition=function(f,e)
 {
  function g(v)
  {
   return!v;
  }
  return[Observable.Filter(f,e),Observable.Filter(function(x)
  {
   return g(f(x));
  },e)];
 };
 ObservableModule.Pairwise=function(e)
 {
  return{
   Subscribe:function(o1)
   {
    var last;
    last=[null];
    return e.Subscribe(Observer.New(function(v)
    {
     var m;
     m=last[0];
     m!=null&&m.$==1?o1.OnNext([m.$0,v]):void 0;
     last[0]={
      $:1,
      $0:v
     };
    },function(a)
    {
     o1.OnError(a);
    },function()
    {
     o1.OnCompleted();
    }));
   }
  };
 };
 Event$1=Event.Event=Runtime.Class({
  Subscribe$1:function(observer)
  {
   var $this;
   function h(a,x)
   {
    return observer.OnNext(x);
   }
   function dispose()
   {
    $this.RemoveHandler$1(h);
   }
   $this=this;
   this.AddHandler$1(h);
   return{
    Dispose:function()
    {
     return dispose();
    }
   };
  },
  RemoveHandler$1:function(h)
  {
   var o;
   o=Seq.tryFindIndex(function(y)
   {
    return Unchecked.Equals(h,y);
   },this.Handlers);
   o==null?void 0:this.Handlers.RemoveAt(o.$0);
  },
  AddHandler$1:function(h)
  {
   this.Handlers.Add(h);
  },
  Trigger:function(x)
  {
   var a,i,$1;
   a=this.Handlers.ToArray();
   for(i=0,$1=a.length-1;i<=$1;i++)(Arrays.get(a,i))(null,x);
  },
  RemoveHandler:function(x)
  {
   this.RemoveHandler$1(x);
  },
  AddHandler:function(x)
  {
   this.AddHandler$1(x);
  },
  Subscribe:function(observer)
  {
   return this.Subscribe$1(observer);
  },
  Dispose:Global.ignore
 },null,Event$1);
 Event$1.New=function(Handlers)
 {
  return new Event$1({
   Handlers:Handlers
  });
 };
 DelegateEvent$1=DelegateEvent.DelegateEvent=Runtime.Class({
  RemoveHandler$1:function(h)
  {
   var o;
   o=Seq.tryFindIndex(function(y)
   {
    return Unchecked.Equals(h,y);
   },this.Handlers);
   o==null?void 0:this.Handlers.RemoveAt(o.$0);
  },
  AddHandler$1:function(h)
  {
   this.Handlers.Add(h);
  },
  Trigger:function(x)
  {
   var a,i,$1;
   a=this.Handlers.ToArray();
   for(i=0,$1=a.length-1;i<=$1;i++)Arrays.get(a,i).apply(null,x);
  },
  RemoveHandler:function(x)
  {
   this.RemoveHandler$1(x);
  },
  AddHandler:function(x)
  {
   this.AddHandler$1(x);
  },
  Dispose:Global.ignore
 },null,DelegateEvent$1);
 DelegateEvent$1.New=function(Handlers)
 {
  return new DelegateEvent$1({
   Handlers:Handlers
  });
 };
 FSharpEvent=Control.FSharpEvent=Runtime.Class({},WebSharper.Obj,FSharpEvent);
 FSharpEvent.New=Runtime.Ctor(function()
 {
  this.event=Event$1.New(new List$1.New$2());
 },FSharpEvent);
 FSharpDelegateEvent=Control.FSharpDelegateEvent=Runtime.Class({},WebSharper.Obj,FSharpDelegateEvent);
 FSharpDelegateEvent.New=Runtime.Ctor(function()
 {
  this.event=DelegateEvent$1.New(new List$1.New$2());
 },FSharpDelegateEvent);
 EventModule.Split=function(f,e)
 {
  return[EventModule.Choose(function(x)
  {
   var m;
   m=f(x);
   return m.$==0?{
    $:1,
    $0:m.$0
   }:null;
  },e),EventModule.Choose(function(x)
  {
   var m;
   m=f(x);
   return m.$==1?{
    $:1,
    $0:m.$0
   }:null;
  },e)];
 };
 EventModule.Scan=function(fold,seed,e)
 {
  var state;
  state=[seed];
  return EventModule.Map(function(value)
  {
   state[0]=fold(state[0],value);
   return state[0];
  },e);
 };
 EventModule.Partition=function(f,e)
 {
  function g(v)
  {
   return!v;
  }
  return[EventModule.Filter(f,e),EventModule.Filter(function(x)
  {
   return g(f(x));
  },e)];
 };
 EventModule.Pairwise=function(e)
 {
  var buf,ev;
  buf=[null];
  ev=Event$1.New(new List$1.New$2());
  e.Subscribe(Util.observer(function(x)
  {
   var m;
   m=buf[0];
   m!=null&&m.$==1?(buf[0]={
    $:1,
    $0:x
   },ev.Trigger([m.$0,x])):buf[0]={
    $:1,
    $0:x
   };
  }));
  return ev;
 };
 EventModule.Merge=function(e1,e2)
 {
  var r;
  r=Event$1.New(new List$1.New$2());
  e1.Subscribe(Util.observer(function(a)
  {
   r.Trigger(a);
  }));
  e2.Subscribe(Util.observer(function(a)
  {
   r.Trigger(a);
  }));
  return r;
 };
 EventModule.Map=function(f,e)
 {
  var r;
  r=Event$1.New(new List$1.New$2());
  e.Subscribe(Util.observer(function(x)
  {
   r.Trigger(f(x));
  }));
  return r;
 };
 EventModule.Filter=function(ok,e)
 {
  var r;
  r=Event$1.New(new List$1.New$2());
  e.Subscribe(Util.observer(function(x)
  {
   if(ok(x))
    r.Trigger(x);
  }));
  return r;
 };
 EventModule.Choose=function(c,e)
 {
  var r;
  r=new FSharpEvent.New();
  e.Subscribe(Util.observer(function(x)
  {
   var m;
   m=c(x);
   m==null?void 0:r.event.Trigger(m.$0);
  }));
  return r.event;
 };
 MailboxProcessor=Control.MailboxProcessor=Runtime.Class({
  dequeue:function()
  {
   var f;
   f=this.mailbox.n.v;
   this.mailbox.RemoveFirst();
   return f;
  },
  resume:function()
  {
   var m;
   m=this.savedCont;
   m!=null&&m.$==1?(this.savedCont=null,this.startAsync(m.$0)):void 0;
  },
  startAsync:function(a)
  {
   Concurrency.Start(a,this.token);
  },
  Scan:function(scanner,timeout)
  {
   var $this,b;
   $this=this;
   b=null;
   return Concurrency.Delay(function()
   {
    return Concurrency.Bind($this.TryScan(scanner,timeout),function(a)
    {
     var $1,$2;
     if(a!=null&&a.$==1)
      $2=a.$0;
     else
      throw new TimeoutException.New();
     return Concurrency.Return($2);
    });
   });
  },
  TryScan:function(scanner,timeout)
  {
   var $this,timeout$1,d,b;
   $this=this;
   timeout$1=(d=this.get_DefaultTimeout(),timeout==null?d:timeout.$0);
   b=null;
   return Concurrency.Delay(function()
   {
    var m,m$1,found,m$2;
    function a(ok)
    {
     var waiting,pending;
     if(timeout$1<0)
      {
       function scanNext()
       {
        var b$1;
        $this.savedCont={
         $:1,
         $0:(b$1=null,Concurrency.Delay(function()
         {
          var m$3;
          m$3=scanner($this.mailbox.n.v);
          return m$3!=null&&m$3.$==1?($this.mailbox.RemoveFirst(),Concurrency.Bind(m$3.$0,function(a$1)
          {
           ok({
            $:1,
            $0:a$1
           });
           return Concurrency.Zero();
          })):(scanNext(),Concurrency.Zero());
         }))
        };
       }
       scanNext();
      }
     else
      {
       function scanNext$1()
       {
        var b$1;
        $this.savedCont={
         $:1,
         $0:(b$1=null,Concurrency.Delay(function()
         {
          var m$3;
          m$3=scanner($this.mailbox.n.v);
          return m$3!=null&&m$3.$==1?($this.mailbox.RemoveFirst(),Concurrency.Bind(m$3.$0,function(a$1)
          {
           return waiting[0]?(waiting[0]=false,Global.clearTimeout(pending),ok({
            $:1,
            $0:a$1
           }),Concurrency.Zero()):Concurrency.Zero();
          })):(scanNext$1(),Concurrency.Zero());
         }))
        };
       }
       waiting=[true];
       pending=Global.setTimeout(function()
       {
        if(waiting[0])
         {
          waiting[0]=false;
          $this.savedCont=null;
          ok(null);
         }
       },timeout$1);
       scanNext$1();
      }
    }
    m$1=$this.mailbox.n;
    found=null;
    while(!Unchecked.Equals(m$1,null))
     {
      m$2=scanner(m$1.v);
      m$2==null?m$1=m$1.n:($this.mailbox.Remove$1(m$1),m$1=null,found=m$2);
     }
    m=found;
    return m!=null&&m.$==1?Concurrency.Bind(m.$0,function(a$1)
    {
     return Concurrency.Return({
      $:1,
      $0:a$1
     });
    }):Concurrency.FromContinuations(function($1,$2,$3)
    {
     return a.apply(null,[$1,$2,$3]);
    });
   });
  },
  PostAndAsyncReply:function(msgf,timeout)
  {
   var $this,b;
   $this=this;
   b=null;
   return Concurrency.Delay(function()
   {
    return Concurrency.Bind($this.PostAndTryAsyncReply(msgf,timeout),function(a)
    {
     var $1,$2;
     if(a!=null&&a.$==1)
      $2=a.$0;
     else
      throw new TimeoutException.New();
     return Concurrency.Return($2);
    });
   });
  },
  PostAndTryAsyncReply:function(msgf,timeout)
  {
   var $this,timeout$1,d;
   function a(ok)
   {
    var waiting;
    if(timeout$1<0)
     {
      function f(a$1)
      {
       return{
        $:1,
        $0:a$1
       };
      }
      $this.mailbox.AddLast(msgf(function(x)
      {
       return ok(f(x));
      }));
      $this.resume();
     }
    else
     {
      waiting=[true];
      $this.mailbox.AddLast(msgf(function(res)
      {
       if(waiting[0])
        {
         waiting[0]=false;
         ok({
          $:1,
          $0:res
         });
        }
      }));
      $this.resume();
      Global.setTimeout(function()
      {
       if(waiting[0])
        {
         waiting[0]=false;
         ok(null);
        }
      },timeout$1);
     }
   }
   $this=this;
   timeout$1=(d=this.get_DefaultTimeout(),timeout==null?d:timeout.$0);
   return Concurrency.FromContinuations(function($1,$2,$3)
   {
    return a.apply(null,[$1,$2,$3]);
   });
  },
  get_CurrentQueueLength:function()
  {
   return this.mailbox.c;
  },
  Receive:function(timeout)
  {
   var $this,b;
   $this=this;
   b=null;
   return Concurrency.Delay(function()
   {
    return Concurrency.Bind($this.TryReceive(timeout),function(a)
    {
     var $1,$2;
     if(a!=null&&a.$==1)
      $2=a.$0;
     else
      throw new TimeoutException.New();
     return Concurrency.Return($2);
    });
   });
  },
  TryReceive:function(timeout)
  {
   var $this,timeout$1,d;
   function a(ok)
   {
    var b,waiting,pending,b$1;
    if(Unchecked.Equals($this.mailbox.n,null))
    {
     if(timeout$1<0)
      {
       $this.savedCont={
        $:1,
        $0:(b=null,Concurrency.Delay(function()
        {
         ok({
          $:1,
          $0:$this.dequeue()
         });
         return Concurrency.Zero();
        }))
       };
      }
     else
      {
       waiting=[true];
       pending=Global.setTimeout(function()
       {
        if(waiting[0])
         {
          waiting[0]=false;
          $this.savedCont=null;
          ok(null);
         }
       },timeout$1);
       $this.savedCont={
        $:1,
        $0:(b$1=null,Concurrency.Delay(function()
        {
         return waiting[0]?(waiting[0]=false,Global.clearTimeout(pending),ok({
          $:1,
          $0:$this.dequeue()
         }),Concurrency.Zero()):Concurrency.Zero();
        }))
       };
      }
    }
    else
     ok({
      $:1,
      $0:$this.dequeue()
     });
   }
   $this=this;
   timeout$1=(d=this.get_DefaultTimeout(),timeout==null?d:timeout.$0);
   return Concurrency.FromContinuations(function($1,$2,$3)
   {
    return a.apply(null,[$1,$2,$3]);
   });
  },
  Start:function()
  {
   var $this,b;
   $this=this;
   this.started?Operators.FailWith("The MailboxProcessor has already been started."):(this.started=true,$this.startAsync((b=null,Concurrency.Delay(function()
   {
    return Concurrency.TryWith(Concurrency.Delay(function()
    {
     return Concurrency.Bind($this.initial($this),function()
     {
      return Concurrency.Return(null);
     });
    }),function(a)
    {
     $this.errorEvent.event.Trigger(a);
     return Concurrency.Zero();
    });
   }))));
  },
  set_DefaultTimeout:function(v)
  {
   this.DefaultTimeout=v;
  },
  get_DefaultTimeout:function()
  {
   return this.DefaultTimeout;
  },
  remove_Error:function(handler)
  {
   this.errorEvent.event.RemoveHandler(handler);
  },
  add_Error:function(handler)
  {
   this.errorEvent.event.AddHandler(handler);
  },
  get_Error:function()
  {
   return this.errorEvent.event;
  }
 },WebSharper.Obj,MailboxProcessor);
 MailboxProcessor.Start=function(initial,token)
 {
  var mb;
  mb=new MailboxProcessor.New(initial,token);
  mb.Start();
  return mb;
 };
 MailboxProcessor.New=Runtime.Ctor(function(initial,token)
 {
  var $this,m;
  function callback(u)
  {
   return $this.resume();
  }
  $this=this;
  this.initial=initial;
  this.token=token;
  this.started=false;
  this.errorEvent=new FSharpEvent.New();
  this.mailbox=new LinkedList.New();
  this.savedCont=null;
  m=this.token;
  m==null?void 0:Concurrency.Register(m.$0,function()
  {
   callback();
  });
  this.DefaultTimeout=-1;
 },MailboxProcessor);
}());

//# sourceMappingURL=WebSharper.Control.map