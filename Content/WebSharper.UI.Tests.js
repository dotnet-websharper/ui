import Runtime from "./WebSharper.Core.JavaScript/Runtime.js"
Runtime.ScriptBasePath="/Scripts/";
import { Iter as Iter_1, GetOptional, CreateFuncWithArgs, DeleteEmptyFields, Create as Create_3, Lazy, Iterate, MarkResizable, SetOptional } from "./WebSharper.Core.JavaScript/Runtime.js"
function isIDisposable(x){
  return"Dispose"in x;
}
function Main(){
  restoreTheme();
  const tr=Doc.Element("button", [Class("btn theme-toggle"), Attr.Create("title", "Switch between the light and dark theme"), Handler("click", () =>() => toggleTheme())], [Doc.TextView(themeLabel())]);
  LoadLocalTemplates("");
  Doc.RunById("theme-slot", tr);
  const _1=greetingDemo();
  LoadLocalTemplates("");
  Doc.RunById("demo-greeting", _1);
  const _2=dataflowDemo();
  LoadLocalTemplates("");
  Doc.RunById("demo-dataflow", _2);
  const _3=listDemo();
  LoadLocalTemplates("");
  Doc.RunById("demo-list", _3);
  const _4=animationDemo();
  LoadLocalTemplates("");
  Doc.RunById("demo-animation", _4);
  const _5=formDemo();
  LoadLocalTemplates("");
  Doc.RunById("demo-form", _5);
  const _6=popoverDemo();
  LoadLocalTemplates("");
  Doc.RunById("demo-popover", _6);
  const _7=templateDemo();
  LoadLocalTemplates("");
  Doc.RunById("demo-template", _7);
  RunTests([VarTest(), ViewTest(), ListModelTest(), SerializerTest(), EltTest(), InputTest(), AttrTest(), DocTest(), LensAndSubmitterTest()]).ReplaceInDom(globalThis.document.querySelector("#main"));
}
function restoreTheme(){
  let saved;
  try {
    saved=globalThis.localStorage.getItem(themeStorageKey());
  }
  catch(m){
    saved=null;
  }
  if(saved=="dark"||saved=="light")theme().Set(saved);
  applyTheme();
}
function toggleTheme(){
  theme().Set(theme().Get()=="dark"?"light":"dark");
  try {
    globalThis.localStorage.setItem(themeStorageKey(), theme().Get());
  }
  catch(m){
  }
  applyTheme();
}
function themeLabel(){
  return _c.themeLabel;
}
function greetingDemo(){
  const name=_c_1.Create_1("WebSharper");
  const trimmed=Map((n) => Trim(n), name.View);
  const greeting=Map((n) => n==""?"Hello, stranger!":"Hello, "+n+"!", trimmed);
  return Doc.Element("div", [Class("demo")], [formField("Your name", Doc.Input([Attr.Create("class", "input"), Attr.Create("placeholder", "Type a name\u2026")], name)), Doc.Element("div", [Class("actions")], [Doc.Element("button", [Class("btn"), Handler("click", () =>() => name.Set("WebSharper"))], [Doc.TextNode("Reset")]), Doc.Element("button", [Class("btn btn-primary"), Handler("click", () =>() => name.Set("Reactive F#"))], [Doc.TextNode("Use an example")])]), Doc.Element("p", [Class("greeting"), DynamicClassPred("is-empty", Map((n) => n=="", trimmed))], [Doc.TextView(greeting)]), Doc.Element("span", [Class("pill")], [Doc.TextView(Map((n) => String(n.length)+" characters", name.View))])]);
}
function dataflowDemo(){
  const first=_c_1.Create_1("Ada");
  const last=_c_1.Create_1("Lovelace");
  const fullName=Map2((_1, _2) => Trim(Trim(_1)+" "+Trim(_2)), first.View, last.View);
  const complete=Map2((_1, _2) => Trim(_1)!=""&&Trim(_2)!="", first.View, last.View);
  const initials=Map2((_1, _2) => {
    const initial=(s) => Trim(s)==""?"?":Substring(Trim(s), 0, 1).toUpperCase();
    return initial(_1)+initial(_2);
  }, first.View, last.View);
  return Doc.Element("div", [Class("demo")], [Doc.Element("div", [Class("form-grid")], [formField("First name", Doc.Input([Attr.Create("class", "input")], first)), formField("Last name", Doc.Input([Attr.Create("class", "input")], last))]), Doc.Element("div", [Class("profile-card"), DynamicClassPred("is-complete", complete)], [Doc.Element("div", [Class("avatar")], [Doc.TextView(initials)]), Doc.Element("div", [Class("profile-text")], [Doc.Element("strong", [], [Doc.TextView(fullName)]), Doc.Element("p", [Class("hint")], [Doc.TextView(Map((ok) => ok?"Both fields are filled in \u2014 ready to submit.":"Fill in both fields to complete the profile.", complete))])])])]);
}
function listDemo(){
  const tasks=Create((t) => t.Name, ofArray([New("Reactive dataflow", _c_1.Create_1(true)), New("Keyed list rendering", _c_1.Create_1(false)), New("Type-safe DOM", _c_1.Create_1(false))]));
  const newTask=_c_1.Create_1("");
  const addTask=() => {
    const name=Trim(newTask.Get());
    if(name!=""&&!tasks.ContainsKey(name)){
      tasks.Append(New(name, _c_1.Create_1(false)));
      newTask.Set("");
    }
  };
  const doneCount=Map((flags) => length(filter_1((x) => x, flags)), Join(MapCached(Sequence, MapSeqCachedBy(tasks.key, (t) => t.Done.View, tasks.u0076ar.View))));
  return Doc.Element("div", [Class("demo")], [Doc.Element("div", [Class("toolbar")], [Doc.Input([Attr.Create("class", "input"), Attr.Create("placeholder", "Add an item\u2026"), Handler("keydown", () =>(ev) => ev.key=="Enter"?addTask():void 0)], newTask), Doc.Element("button", [Class("btn btn-primary"), Handler("click", () =>() => addTask())], [Doc.TextNode("Add")]), Doc.Element("span", [Class("spacer")], []), Doc.Element("span", [Class("pill")], [Doc.TextView(Map((n) => String(n)+" item(s)", tasks.LengthAsView))]), Doc.Element("span", [Class("pill")], [Doc.TextView(Map((n) => String(n)+" done", doneCount))])]), Doc.Element("ul", [Class("task-list")], [Doc.ConvertBy(tasks.key, (t) => Doc.Element("li", [Class("task"), DynamicClassPred("is-done", t.Done.View)], [Doc.CheckBox([], t.Done), Doc.Element("span", [Class("task-name")], [Doc.TextNode(t.Name)]), Doc.Element("button", [Class("icon-btn"), Attr.Create("title", "Remove"), Handler("click", () =>() => tasks.RemoveByKey(t.Name))], [Doc.TextNode("×")])]), tasks.u0076ar.View)])]);
}
function animationDemo(){
  const position=_c_1.Create_1(0);
  const a=get_Double();
  const a_1=Easing.Custom((x) => x);
  const linear=(s) =>(e) => Simple(a, a_1, 360, s, e);
  const a_2=get_Double();
  const cubic=(s) =>(e) => Simple(a_2, Easing.CubicInOut, 360, s, e);
  const transition=Exit((x) =>(cubic(x))(x+18), Enter((x) =>(cubic(x-18))(x), Create_1((_1, _2) =>(linear(_1))(_2))));
  return Doc.Element("div", [Class("demo")], [Doc.Element("div", [Class("stage")], [Doc.Element("div", [Class("slider"), AnimatedStyle("left", transition, position.View, (p) => p.toFixed(1)+"%")], [Doc.TextView(Map((p) => p>0?"On":"Off", position.View))])]), Doc.Element("div", [Class("actions")], [Doc.Element("button", [Class("btn btn-primary"), Handler("click", () =>() => position.Set(position.Get()>0?0:55))], [Doc.TextNode("Toggle position")])]), Doc.Element("p", [Class("hint")], [Doc.TextNode("Driven by Attr.AnimatedStyle with a Trans built from Anim.Simple and Easing.CubicInOut.")])]);
}
function formDemo(){
  const settings=_c_1.Create_1(New_1(New_2("Ada Lovelace", "2026-11-21"), "#4f46e5", 70, true));
  const password=_c_1.Create_1("hunter2");
  const plan=_c_1.Create_1(Pro);
  const files=_c_1.Create_1([]);
  const displayName=_c_1.Lens(settings, (s) => s.Profile.DisplayName, (_1, _2) => New_1(New_2(_2, _1.Profile.Date), _1.Accent, _1.Volume, _1.Subscribe));
  const date=_c_1.Lens(settings, (s) => s.Profile.Date, (_1, _2) => New_1(New_2(_1.Profile.DisplayName, _2), _1.Accent, _1.Volume, _1.Subscribe));
  const accent=_c_1.Lens(settings, (s) => s.Accent, (_1, _2) => New_1(_1.Profile, _2, _1.Volume, _1.Subscribe));
  const volume=_c_1.Lens(settings, (s) => s.Volume, (_1, _2) => New_1(_1.Profile, _1.Accent, _2, _1.Subscribe));
  const subscribe=_c_1.Lens(settings, (s) => s.Subscribe, (_1, _2) => New_1(_1.Profile, _1.Accent, _1.Volume, _2));
  const accentView=accent.View;
  const settingsView=settings.View;
  return Doc.Element("div", [Class("demo")], [Doc.Element("div", [Class("form-grid")], [formField("Display name", Doc.Input([Attr.Create("class", "input")], displayName)), formField("Accent", Doc.ColorInput([Attr.Create("class", "input color-input")], accent)), formField("Password", Doc.PasswordBox([Attr.Create("class", "input")], password)), formField("Sign-up date", Doc.DateInput([Attr.Create("class", "input")], date)), formField("Volume", Doc.Element("div", [Class("range-row")], [Doc.RangeInput([Class("range")], volume), Doc.Element("span", [Class("pill")], [Doc.TextView(Map((v) => String(v)+"%", volume.View))])])), formField("Plan", Doc.Select([Attr.Create("class", "input")], planText, ofArray([Free, Pro, Team]), plan)), formField("Notifications", Doc.Element("label", [Class("check")], [Doc.CheckBox([], subscribe), Doc.TextNode("Email me about updates")])), formField("Attachments", Doc.Element("div", [Class("range-row")], [Doc.FileInput([Class("file-input")], files), Doc.Element("span", [Class("pill")], [Doc.TextView(Map((fs) => String(length_1(fs))+" file(s)", files.View))])]))]), Doc.Element("div", [Class("preview-card"), DynamicStyle("border-color", accentView), DynamicStyle("background", Map((c) => c+"18", accentView))], [Doc.Element("span", [Class("preview-accent"), DynamicStyle("background", accentView)], []), Doc.Element("div", [], [Doc.Element("strong", [], [Doc.TextView(Map((s) => Trim(s.Profile.DisplayName)==""?"Unnamed profile":s.Profile.DisplayName, settingsView))]), Doc.Element("p", [Class("hint")], [Doc.TextView(Map2((_1, _2) => toSafe(planText(_2))+" plan · volume "+String(_1.Volume)+"% · "+toSafe(_1.Subscribe?"notifications on":"notifications off"), settingsView, plan.View))]), Doc.Element("p", [Class("hint")], [Doc.TextView(Map2((_1, _2) => _1.length===0?"No password set.":String(_1.length)+"-character password · date "+toSafe(_2), password.View, date.View))])])])]);
}
function popoverDemo(){
  return Doc.Element("div", [Class("demo")], [Doc.Element("button", [Class("btn btn-primary"), Attr.Create("popovertarget", "demo-popover-panel"), Attr.Create("popovertargetaction", "toggle")], [Doc.TextNode("Toggle popover")]), Doc.Element("div", [Attr.Create("id", "demo-popover-panel"), Attr.Create("popover", "auto"), Class("popover-card")], [Doc.Element("h3", [], [Doc.TextNode("Native popovers")]), Doc.Element("p", [], [Doc.TextNode("A top-layer element toggled declaratively with "), Doc.Element("code", [], [Doc.TextNode("popovertarget")]), Doc.TextNode(".")])]), Doc.Element("p", [Class("hint")], [Doc.TextNode("Click the button to open the panel; click outside it (or press Escape) to dismiss.")])]);
}
function templateDemo(){
  LoadLocalTemplates("local");
  const value=_c_1.Create_1("initial");
  return NamedTemplate("local", Some("testtemplate"), [new Elt("input", Doc.Input([Attr.Create("class", "input")], value)), new Elt("value", Doc.TextView(value.View)), new Elt("item", NamedTemplate("local", Some("item"), [new Text("text", "This item comes from a nested runtime template.")]))]);
}
function VarTest(){
  return _c.VarTest;
}
function ViewTest(){
  return _c.ViewTest;
}
function ListModelTest(){
  return _c.ListModelTest;
}
function SerializerTest(){
  return _c.SerializerTest;
}
function EltTest(){
  return _c.EltTest;
}
function InputTest(){
  return _c.InputTest;
}
function AttrTest(){
  return _c.AttrTest;
}
function DocTest(){
  return _c.DocTest;
}
function LensAndSubmitterTest(){
  return _c.LensAndSubmitterTest;
}
function themeStorageKey(){
  return _c.themeStorageKey;
}
function theme(){
  return _c.theme;
}
function applyTheme(){
  globalThis.document.documentElement.setAttribute("data-theme", theme().Get());
}
function formField(labelText, control){
  return Doc.Element("label", [Class("field")], [Doc.Element("span", [Class("field-label")], [Doc.TextNode(labelText)]), control]);
}
function planText(p){
  return p.$==1?"Pro":p.$==2?"Team":"Free";
}
function mount(doc){
  const host=globalThis.document.createElement("div");
  globalThis.document.body.appendChild(host);
  LoadLocalTemplates("");
  Doc.Run(host, doc);
  return host;
}
function afterUpdate(f){
  const f_1=() => {
    const f_2=() => Return(f());
    return Bind_1(Sleep(50), (_1, _2) =>(f_2(_1))(_2));
  };
  return Delay((_1, _2) =>(f_1(_1))(_2));
}
function trigger(el, eventName){
  el.dispatchEvent(new Event(eventName, {}));
}
function unmount(host){
  host.parentNode.removeChild(host);
}
function isChecked(el){
  return el.checked;
}
function selectedIndices(el){
  const options=el.querySelectorAll("option");
  const l=FSharpList.Cons(void 0, void 0);
  let last=l;
  for(const i of Iter_1(range(0, options.length-1)))if(options[i].selected){
    const c=FSharpList.Cons(i, void 0);
    last.$1=c;
    last=c;
  }
  last.$1=FSharpList.Empty;
  return l.$1;
}
function Class(name){
  return ClassPred(name, true);
}
function ClassPred(name, isSet){
  return Attr.A3((el) => {
    if(isSet)AddClass(el, name);
    else RemoveClass(el, name);
  });
}
function Handler(name, callback){
  return Attr.A3((el) => {
    el.addEventListener(name, (o) =>(callback(el))(o), false);
  });
}
function DynamicClassPred(name, view){
  return Dynamic_1(view, (el) =>(v) => v?AddClass(el, name):RemoveClass(el, name));
}
function AnimatedStyle(name, tr, view, attr_1){
  return Animated(tr, view, (el) =>(v) => {
    const value=attr_1(v);
    return el.style.setProperty(name, value);
  });
}
function DynamicStyle(name, view){
  return Dynamic_1(view, (el) =>(v) => el.style.setProperty(name, v));
}
function Style(name, value){
  return Attr.A3((el) => {
    el.style.setProperty(name, value);
  });
}
function Prop(name, value){
  return Attr.A3((el) => {
    el[name]=value;
  });
}
function DynamicProp(name, view){
  return Dynamic_1(view, (el) =>(v) => {
    el[name]=v;
  });
}
function Dynamic(name, view){
  return Dynamic_1(view, (el) =>(v) => el.setAttribute(name, v));
}
function DynamicBool(name, boolview){
  return Dynamic_1(boolview, (_1) =>(_2) => _2?_1.setAttribute(name, ""):_1.removeAttribute(name));
}
function Value(var_1){
  return ValueWith(StringApply(), var_1);
}
function Checked(var_1){
  return ValueWith(BoolCheckedApply(), var_1);
}
function IntValueUnchecked(var_1){
  return ValueWith(IntApplyUnchecked(), var_1);
}
function FileValue(var_1){
  return ValueWith(FileApplyUnchecked(), var_1);
}
function IntValue(var_1){
  return ValueWith(IntApplyChecked(), var_1);
}
function ValueWith(bind, var_1){
  const p=bind(var_1);
  return AppendTree(Attr.A3(p[0]), DynamicCustom(p[1], p[2]));
}
function OnAfterRender(callback){
  return Attr.A4(callback);
}
function DynamicCustom(set_1, view){
  return Dynamic_1(view, set_1);
}
class Object_1 {
  Equals(obj){
    return this===obj;
  }
  GetHashCode(){
    return -1;
  }
}
class attr extends Object_1 { }
function FailWith(msg){
  throw new Error(msg);
}
function range(min, max_1){
  const count=1+max_1-min;
  return count<=0?[]:init_1(count, (x) => x+min);
}
function KeyValue(kvp){
  return[kvp.K, kvp.V];
}
function RunTests(tests){
  return new RunnerControlBody(() => {
    const e=Get_1(tests);
    try {
      while(e.MoveNext())
        {
          const t=e.Current;
          globalThis.QUnit.module(t.name);
          t.run();
        }
    }
    finally {
      if(typeof e=="object"&&isIDisposable(e))e.Dispose();
    }
  });
}
function GetFieldValues(o){
  const r=[];
  let k;
  for(var k_1 in o)r.push(o[k_1]);
  return r;
}
function Equals(a, b){
  if(a===b)return true;
  else {
    const m=typeof a;
    if(m=="object"){
      if(a===null||a===void 0||b===null||b===void 0||!Equals(typeof b, "object"))return false;
      else if("Equals"in a)return a.Equals(b);
      else if("Equals"in b)return false;
      else if(a instanceof Array&&b instanceof Array)return arrayEquals(a, b);
      else if(a instanceof Date&&b instanceof Date)return dateEquals(a, b);
      else {
        let k;
        let eqR=true;
        for(var k_2 in a)if(((k_3) => {
          eqR=!a.hasOwnProperty(k_3)||b.hasOwnProperty(k_3)&&Equals(a[k_3], b[k_3]);
          return!eqR;
        })(k_2))break;
        if(eqR){
          let k_1;
          for(var k_3 in b)if(((k_4) => {
            eqR=!b.hasOwnProperty(k_4)||a.hasOwnProperty(k_4);
            return!eqR;
          })(k_3))break;
        }
        return eqR;
      }
    }
    else return m=="function"&&("$Func"in a?a.$Func===b.$Func&&a.$Target===b.$Target:"$Invokes"in a&&"$Invokes"in b&&arrayEquals(a.$Invokes, b.$Invokes));
  }
}
function arrayEquals(a, b){
  let eq, i;
  if(length_1(a)===length_1(b)){
    eq=true;
    i=0;
    while(eq&&i<length_1(a))
      {
        !Equals(get(a, i), get(b, i))?eq=false:void 0;
        i=i+1;
      }
    return eq;
  }
  else return false;
}
function dateEquals(a, b){
  return a.getTime()===b.getTime();
}
function Hash(o){
  const m=typeof o;
  return m=="function"?0:m=="boolean"?o?1:0:m=="number"?o:m=="string"?hashString(o):m=="object"?o==null?0:o instanceof Array?hashArray(o):hashObject(o):m=="bigint"?hashString(String(o)):m=="symbol"?hashString(o.description):0;
}
function Compare(a, b){
  if(a===b)return 0;
  else {
    const m=typeof a;
    switch(m=="boolean"?1:m=="number"?1:m=="bigint"?1:m=="string"?1:m=="object"?2:m=="function"?3:m=="symbol"?4:0){
      case 0:
        return typeof b=="undefined"?0:-1;
      case 1:
        return a<b?-1:1;
      case 2:
        if(a===null)return -1;
        else if(b===null)return 1;
        else if("CompareTo"in a)return a.CompareTo(b);
        else if("CompareTo0"in a)return a.CompareTo0(b);
        else if(a instanceof Array&&b instanceof Array)return compareArrays(a, b);
        else if(a instanceof Date&&b instanceof Date)return compareDates(a, b);
        else {
          let k;
          let cmp=0;
          for(var k_2 in a)if(((k_3) =>!a.hasOwnProperty(k_3)?false:!b.hasOwnProperty(k_3)?(cmp=1,true):(cmp=Compare(a[k_3], b[k_3]),cmp!==0))(k_2))break;
          if(cmp===0){
            let k_1;
            for(var k_3 in b)if(((k_4) =>!b.hasOwnProperty(k_4)?false:!a.hasOwnProperty(k_4)&&(cmp=-1,true))(k_3))break;
          }
          return cmp;
        }
        break;
      case 3:
        return FailWith("Cannot compare function values.");
      case 4:
        return FailWith("Cannot compare symbol values.");
    }
  }
}
function hashString(s){
  let hash;
  if(s===null)return 0;
  else {
    hash=5381;
    for(let i=0, _1=s.length-1;i<=_1;i++)hash=hashMix(hash, s[i].charCodeAt());
    return hash;
  }
}
function hashArray(o){
  let h=-34948909;
  for(let i=0, _1=length_1(o)-1;i<=_1;i++)h=hashMix(h, Hash(get(o, i)));
  return h;
}
function hashObject(o){
  if("GetHashCode"in o)return o.GetHashCode();
  else {
    let k;
    const ____=hashMix;
    let h=0;
    for(var k_1 in o)if(((key) => {
      h=____(____(h, hashString(key)), Hash(o[key]));
      return false;
    })(k_1))break;
    return h;
  }
}
function compareArrays(a, b){
  let cmp, i;
  if(length_1(a)<length_1(b))return -1;
  else if(length_1(a)>length_1(b))return 1;
  else {
    cmp=0;
    i=0;
    while(cmp===0&&i<length_1(a))
      {
        cmp=Compare(get(a, i), get(b, i));
        i=i+1;
      }
    return cmp;
  }
}
function compareDates(a, b){
  return Compare(a.getTime(), b.getTime());
}
function hashMix(x, y){
  return(x<<5)+x+y;
}
class Var extends Object_1 { }
function LoadLocalTemplates(baseName){
  !LocalTemplatesLoaded()?(set_LocalTemplatesLoaded(true),LoadNestedTemplates(globalThis.document.body, "")):void 0;
  LoadedTemplates().set_Item(baseName, LoadedTemplateFile(""));
}
function LocalTemplatesLoaded(){
  return _c_2.LocalTemplatesLoaded;
}
function set_LocalTemplatesLoaded(_1){
  _c_2.LocalTemplatesLoaded=_1;
}
function LoadNestedTemplates(root, baseName){
  const loadedTpls=LoadedTemplateFile(baseName);
  const rawTpls=new Dictionary("New_5");
  const wsTemplates=root.querySelectorAll("[ws-template]");
  for(let i=0, _1=wsTemplates.length-1;i<=_1;i++){
    const node=wsTemplates[i];
    const name=node.getAttribute("ws-template").toLowerCase();
    node.removeAttribute("ws-template");
    rawTpls.set_Item(name, FakeRootSingle(node));
  }
  const wsChildrenTemplates=root.querySelectorAll("[ws-children-template]");
  for(let i_1=0, _2=wsChildrenTemplates.length-1;i_1<=_2;i_1++){
    const node_1=wsChildrenTemplates[i_1];
    const name_1=node_1.getAttribute("ws-children-template").toLowerCase();
    node_1.removeAttribute("ws-children-template");
    rawTpls.set_Item(name_1, FakeRoot(node_1));
  }
  const html5TemplateBasedTemplates=root.querySelectorAll("template[id]");
  for(let i_2=0, _3=html5TemplateBasedTemplates.length-1;i_2<=_3;i_2++){
    const node_2=html5TemplateBasedTemplates[i_2];
    rawTpls.set_Item(node_2.getAttribute("id").toLowerCase(), FakeRootFromHTMLTemplate(node_2));
  }
  const html5TemplateBasedTemplates_1=root.querySelectorAll("template[name]");
  for(let i_3=0, _4=html5TemplateBasedTemplates_1.length-1;i_3<=_4;i_3++){
    const node_3=html5TemplateBasedTemplates_1[i_3];
    rawTpls.set_Item(node_3.getAttribute("name").toLowerCase(), FakeRootFromHTMLTemplate(node_3));
  }
  const instantiated=new HashSet("New_3");
  function prepareTemplate(name_2){
    if(!loadedTpls.ContainsKey(name_2)){
      let o=null;
      const m=[rawTpls.TryGetValue(name_2, {get:() => o, set:(v) => {
        o=v;
      }}), o];
      if(m[0]){
        instantiated.SAdd(name_2);
        rawTpls.RemoveKey(name_2);
        PrepareTemplateStrict(baseName, Some(name_2), m[1], Some(prepareTemplate));
      }
      else console.warn(instantiated.Contains(name_2)?"Encountered loop when instantiating "+name_2:"Local template does not exist: "+name_2);
    }
  }
  while(rawTpls.count>0)
    prepareTemplate(head_1(rawTpls.Keys));
}
function LoadedTemplates(){
  return _c_2.LoadedTemplates;
}
function LoadedTemplateFile(name){
  let o=null;
  const m=[LoadedTemplates().TryGetValue(name, {get:() => o, set:(v) => {
    o=v;
  }}), o];
  if(m[0])return m[1];
  else {
    const d=new Dictionary("New_5");
    LoadedTemplates().set_Item(name, d);
    return d;
  }
}
function NamedTemplate(baseName, name, fillWith){
  let o=null;
  const m=[LoadedTemplateFile(baseName).TryGetValue(name==null?"":name.$0, {get:() => o, set:(v) => {
    o=v;
  }}), o];
  return m[0]?ChildrenTemplate(m[1].cloneNode(true), fillWith):(console.warn("Local template doesn't exist", name),Doc.Empty);
}
function FakeRootSingle(el){
  el.removeAttribute("ws-template");
  const m=el.getAttribute("ws-replace");
  if(!(m==null)){
    el.removeAttribute("ws-replace");
    const m_1=el.parentNode;
    if(!Equals(m_1, null)){
      const n=globalThis.document.createElement(el.tagName);
      n.setAttribute("ws-replace", m);
      m_1.replaceChild(n, el);
    }
  }
  const fakeroot=globalThis.document.createElement("div");
  fakeroot.appendChild(el);
  return fakeroot;
}
function FakeRoot(parent){
  const fakeroot=globalThis.document.createElement("div");
  while(parent.hasChildNodes())
    fakeroot.appendChild(parent.firstChild);
  return fakeroot;
}
function FakeRootFromHTMLTemplate(parent){
  const fakeroot=globalThis.document.createElement("div");
  const content=parent.content;
  for(let i=0, _1=content.childNodes.length-1;i<=_1;i++)fakeroot.appendChild(content.childNodes[i].cloneNode(true));
  return fakeroot;
}
function PrepareTemplateStrict(baseName, name, fakeroot, prepareLocalTemplate){
  const processedHTML5Templates=new HashSet("New_3");
  function recF(recI, _1){
    while(true)
      switch(recI){
        case 0:
          if(_1!==null){
            const next=_1.nextSibling;
            if(Equals(_1.nodeType, Node.TEXT_NODE))convertTextNode(_1);
            else Equals(_1.nodeType, Node.ELEMENT_NODE)?convertElement(_1):void 0;
            _1=next;
          }
          else return;
          break;
        case 1:
          const name_2=string(_1.nodeName, Some(3), null).toLowerCase();
          const m=name_2.indexOf(".");
          const p=m===-1?[baseName, name_2]:[string(name_2, null, Some(m-1)), string(name_2, Some(m+1), null)];
          const instName=p[1];
          const instBaseName=p[0];
          if(instBaseName!=""&&!LoadedTemplates().ContainsKey(instBaseName))return failNotLoaded(instName);
          else {
            if(instBaseName==""&&prepareLocalTemplate!=null)prepareLocalTemplate.$0(instName);
            const d=LoadedTemplates().Item(instBaseName);
            if(!d.ContainsKey(instName))return failNotLoaded(instName);
            else {
              const t=d.Item(instName);
              const instance=t.cloneNode(true);
              const usedHoles=new HashSet("New_3");
              const mappings=new Dictionary("New_5");
              const attrs=_1.attributes;
              for(let i=0, _2=attrs.length-1;i<=_2;i++){
                const name_3=attrs.item(i).name.toLowerCase();
                const m_1=attrs.item(i).nodeValue;
                mappings.set_Item(name_3, m_1==""?name_3:m_1.toLowerCase());
                if(!usedHoles.SAdd(name_3))console.warn("Hole mapped twice", name_3);
              }
              for(let i_1=0, _3=_1.childNodes.length-1;i_1<=_3;i_1++){
                const n=_1.childNodes[i_1];
                if(Equals(n.nodeType, Node.ELEMENT_NODE))if(!usedHoles.SAdd(n.nodeName.toLowerCase()))console.warn("Hole filled twice", instName);
              }
              const singleTextFill=_1.childNodes.length===1&&Equals(_1.firstChild.nodeType, Node.TEXT_NODE);
              if(singleTextFill){
                const x=fillTextHole(instance, _1.firstChild.textContent, instName);
                const f=((usedHoles_1) =>(i_2) => usedHoles_1.SAdd(i_2))(usedHoles);
                ((((a) =>(o) => {
                  if(o!=null)a(o.$0);
                })((x_1) => {
                  f(x_1);
                }))(x));
              }
              removeHolesExcept(instance, usedHoles);
              if(!singleTextFill){
                for(let i_2=0, _4=_1.childNodes.length-1;i_2<=_4;i_2++){
                  const n_1=_1.childNodes[i_2];
                  if(Equals(n_1.nodeType, Node.ELEMENT_NODE))if(n_1.hasAttributes())fillInstanceAttrs(instance, n_1);
                  else fillDocHole(instance, n_1);
                }
              }
              mapHoles(instance, mappings);
              fill(instance, _1.parentNode, _1);
              _1.parentNode.removeChild(_1);
              return;
            }
          }
          break;
      }
  }
  function fillDocHole(instance, fillWith){
    const name_2=fillWith.nodeName.toLowerCase();
    const fillHole=(p, n) => {
      if(name_2=="title"&&fillWith.hasChildNodes()){
        const parsed=ParseHTMLIntoFakeRoot(fillWith.textContent);
        fillWith.removeChild(fillWith.firstChild);
        while(parsed.hasChildNodes())
          fillWith.appendChild(parsed.firstChild);
      }
      convertElement(fillWith);
      return fill(fillWith, p, n);
    };
    foreachNotPreserved(instance, "[ws-attr-holes]", (e) => {
      const holeAttrs=SplitChars(e.getAttribute("ws-attr-holes"), [" "], 1);
      for(let i=0, _1=holeAttrs.length-1;i<=_1;i++){
        const attrName=get(holeAttrs, i);
        const this_1=new RegExp("\\${"+name_2+"}", "ig");
        const str=e.getAttribute(attrName);
        const newSubStr=fillWith.textContent;
        e.setAttribute(attrName, str.replace(this_1, newSubStr));
      }
    });
    const m=instance.querySelector("[ws-hole="+name_2+"]");
    if(Equals(m, null)){
      const m_1=instance.querySelector("[ws-replace="+name_2+"]");
      if(Equals(m_1, null)){
        const m_2=instance.querySelector("slot[name="+name_2+"]");
        return instance.tagName.toLowerCase()=="template"?(fillHole(m_2.parentNode, m_2),void m_2.parentNode.removeChild(m_2)):void 0;
      }
      else {
        fillHole(m_1.parentNode, m_1);
        m_1.parentNode.removeChild(m_1);
        return;
      }
    }
    else {
      while(m.hasChildNodes())
        m.removeChild(m.lastChild);
      m.removeAttribute("ws-hole");
      return fillHole(m, null);
    }
  }
  function convertElement(el){
    if(!el.hasAttribute("ws-preserve"))if(StartsWith(el.nodeName.toLowerCase(), "ws-"))convertInstantiation(el);
    else {
      convertAttrs(el);
      convertNodeAndSiblings(el.firstChild);
    }
  }
  function convertNodeAndSiblings(n){
    return recF(0, n);
  }
  function convertInstantiation(el){
    return recF(1, el);
  }
  function convertNestedTemplates(el){
    while(true)
      {
        const m=el.querySelector("[ws-template]");
        if(Equals(m, null)){
          const m_1=el.querySelector("[ws-children-template]");
          if(Equals(m_1, null)){
            const idTemplates=el.querySelectorAll("template[id]");
            for(let i=1, _1=idTemplates.length-1;i<=_1;i++){
              const n=idTemplates[i];
              if(!processedHTML5Templates.Contains(n)){
                PrepareTemplateStrict(baseName, Some(n.getAttribute("id")), n, null);
                processedHTML5Templates.SAdd(n);
              }
            }
            const nameTemplates=el.querySelectorAll("template[name]");
            for(let i_1=1, _2=nameTemplates.length-1;i_1<=_2;i_1++){
              const n_1=nameTemplates[i_1];
              if(!processedHTML5Templates.Contains(n_1)){
                PrepareTemplateStrict(baseName, Some(n_1.getAttribute("name")), n_1, null);
                processedHTML5Templates.SAdd(n_1);
              }
            }
            return;
          }
          else {
            const name_2=m_1.getAttribute("ws-children-template");
            m_1.removeAttribute("ws-children-template");
            PrepareTemplateStrict(baseName, Some(name_2), m_1, null);
            el=el;
          }
        }
        else {
          const name_3=m.getAttribute("ws-template");
          (PrepareSingleTemplate(baseName, Some(name_3), m))(null);
          el=el;
        }
      }
  }
  const name_1=(name==null?"":name.$0).toLowerCase();
  LoadedTemplateFile(baseName).set_Item(name_1, fakeroot);
  if(fakeroot.hasChildNodes()){
    convertNestedTemplates(fakeroot);
    convertNodeAndSiblings(fakeroot.firstChild);
  }
}
function ChildrenTemplate(el, fillWith){
  const p=InlineTemplate(el, append_1(fillWith, GlobalHoles().Values));
  const updates=p[1];
  const docTreeNode=p[0];
  const m=docTreeNode.Els;
  return!Equals(m, null)&&m.length===1&&(get(m, 0)instanceof Node&&(Equals(get(m, 0).nodeType, Node.ELEMENT_NODE)&&true))?Elt_1.TreeNode(docTreeNode, updates):Doc.Mk(TreeDoc(docTreeNode), updates);
}
function foreachNotPreserved(root, selector, f){
  IterSelector(root, selector, (p) => {
    if(p.closest("[ws-preserve]")==null)f(p);
  });
}
function PrepareSingleTemplate(baseName, name, el){
  const root=FakeRootSingle(el);
  return(p) => {
    PrepareTemplateStrict(baseName, name, root, p);
  };
}
function InlineTemplate(el, fillWith){
  let _1;
  const holes=[];
  const updates=[];
  const attrs=[];
  const afterRender=[];
  const fw=new Dictionary("New_5");
  const e=Get_1(fillWith);
  try {
    while(e.MoveNext())
      {
        const x=e.Current;
        fw.set_Item(x.Name, x);
      }
  }
  finally {
    if(typeof e=="object"&&isIDisposable(e))e.Dispose();
  }
  let els=ChildrenArray(el);
  const addAttr=(el_1, attr_1) => {
    const attr_2=Insert(el_1, attr_1);
    updates.push(Updates(attr_2));
    attrs.push([el_1, attr_2]);
    const m=GetOptional(attr_2.OnAfterRender);
    if(m==null)return;
    else {
      const f=m.$0;
      afterRender.push(() => {
        f(el_1);
      });
      return;
    }
  };
  const tryGetAsDoc=(name) => {
    let o=null;
    const m=[fw.TryGetValue(name, {get:() => o, set:(v) => {
      o=v;
    }}), o];
    if(m[0]){
      const th=m[1];
      if(th instanceof Elt)return Some(th.Value);
      else if(th instanceof Text)return Some(Doc.TextNode(th.Value));
      else {
        const o_1=th.ForTextView();
        return o_1==null?null:Some(Doc.TextView(o_1.$0));
      }
    }
    else return null;
  };
  foreachNotPreserved(el, "[ws-hole]", (p) => {
    const name=p.getAttribute("ws-hole");
    p.removeAttribute("ws-hole");
    while(p.hasChildNodes())
      p.removeChild(p.lastChild);
    const m=tryGetAsDoc(name);
    if(m!=null&&m.$==1){
      const doc=m.$0;
      LinkElement(p, doc.docNode);
      holes.push(DocElemNode.New(Empty(p), doc.docNode, null, p, Int(), null));
      updates.push(doc.updates);
    }
  });
  foreachNotPreserved(el, "[ws-replace]", (e_1) => {
    const m=tryGetAsDoc(e_1.getAttribute("ws-replace"));
    if(m!=null&&m.$==1){
      const doc=m.$0;
      const p=e_1.parentNode;
      const after=globalThis.document.createTextNode("");
      p.replaceChild(after, e_1);
      const before=InsertBeforeDelim(after, doc.docNode);
      els=ChildrenArray(el);
      const o=tryFindIndex_1((y) => e_1===y, els);
      if(o==null){ }
      else set(els, o.$0, doc.docNode);
      holes.push(DocElemNode.New(Empty(p), doc.docNode, Some([before, after]), p, Int(), null));
      updates.push(doc.updates);
    }
  });
  let isDefaultSlotProcessed=false;
  foreachNotPreserved(el, "slot", (p) => {
    const name=p.getAttribute("name");
    const name_1=name==""||name==null?"default":name.toLowerCase();
    if(!(isDefaultSlotProcessed&&name_1=="default"||!Equals(el.parentElement, null))){
      while(p.hasChildNodes())
        p.removeChild(p.lastChild);
      if(name_1=="default")isDefaultSlotProcessed=true;
      const m=tryGetAsDoc(name_1);
      if(m!=null&&m.$==1){
        const doc=m.$0;
        LinkElement(p, doc.docNode);
        holes.push(DocElemNode.New(Empty(p), doc.docNode, null, p, Int(), null));
        updates.push(doc.updates);
      }
    }
  });
  foreachNotPreserved(el, "[ws-attr]", (e_1) => {
    const name=e_1.getAttribute("ws-attr");
    e_1.removeAttribute("ws-attr");
    let o=null;
    const m=[fw.TryGetValue(name, {get:() => o, set:(v) => {
      o=v;
    }}), o];
    if(m[0]){
      const th=m[1];
      if(th instanceof Attribute)addAttr(e_1, th.Value);
      else console.warn("Attribute hole filled with non-attribute data", name);
    }
  });
  foreachNotPreserved(el, "[ws-on]", (e_1) => {
    addAttr(e_1, Attr.Concat(choose((x_1) => {
      const a=SplitChars(x_1, [":"], 1);
      let o=null;
      const m=[fw.TryGetValue(get(a, 1), {get:() => o, set:(v) => {
        o=v;
      }}), o];
      if(m[0]){
        const th=m[1];
        return th instanceof Event_1?Some(Handler(get(a, 0), th.Value)):th instanceof EventQ?Some(Attr.HandlerImpl(get(a, 0), th.Value)):(console.warn("Event hole on"+get(a, 0)+" filled with non-event data", get(a, 1)),null);
      }
      else return null;
    }, SplitChars(e_1.getAttribute("ws-on"), [" "], 1))));
    e_1.removeAttribute("ws-on");
  });
  foreachNotPreserved(el, "[ws-onafterrender]", (e_1) => {
    const name=e_1.getAttribute("ws-onafterrender");
    let o=null;
    const m=[fw.TryGetValue(name, {get:() => o, set:(v) => {
      o=v;
    }}), o];
    if(m[0]){
      const th=m[1];
      if(th instanceof AfterRender_1){
        e_1.removeAttribute("ws-onafterrender");
        addAttr(e_1, OnAfterRender(th.Value));
      }
      else if(th instanceof AfterRenderQ){
        e_1.removeAttribute("ws-onafterrender");
        addAttr(e_1, OnAfterRender(th.Value));
      }
      else console.warn("onafterrender hole filled with non-onafterrender data", name);
    }
  });
  foreachNotPreserved(el, "[ws-var]", (e_1) => {
    const name=e_1.getAttribute("ws-var");
    e_1.removeAttribute("ws-var");
    let o=null;
    const m=[fw.TryGetValue(name, {get:() => o, set:(v) => {
      o=v;
    }}), o];
    if(m[0])m[1].AddAttribute((_5) =>(_6) => addAttr(_5, _6), e_1);
  });
  const wsdomHandling=() => {
    foreachNotPreservedwsDOM("[ws-dom]", (e_1) => {
      let toWatch, r;
      let o=null;
      const m=[fw.TryGetValue(e_1.getAttribute("ws-dom").toLowerCase(), {get:() => o, set:(v) => {
        o=v;
      }}), o];
      if(m[0]){
        const th=m[1];
        if(th instanceof VarDomElement){
          const var_1=th.Value;
          e_1.removeAttribute("ws-dom");
          toWatch=e_1;
          const mo=new MutationObserver((_5, mo_1) => {
            iter_2((mr) => {
              mr.removedNodes.forEach(CreateFuncWithArgs((_6) => _6[0]===toWatch&&mr.addedNodes.length!==1?(var_1.SetFinal(null),mo_1.disconnect()):void 0), null);
            }, _5);
          });
          if(e_1.parentElement!==null)mo.observe(e_1.parentElement, (r={},r.childList=true,r));
          var_1.Set(Some(e_1));
          Sink((nel) => {
            if(nel!=null&&nel.$==1){
              const nel_1=nel.$0;
              if(!(toWatch===nel_1)){
                toWatch.replaceWith(nel_1);
                toWatch=nel_1;
              }
            }
            else {
              toWatch.remove();
              mo.disconnect();
            }
          }, var_1.View);
        }
      }
    });
  };
  foreachNotPreserved(el, "[ws-attr-holes]", (e_1) => {
    const re=new RegExp(TextHoleRE(), "g");
    const holeAttrs=SplitChars(e_1.getAttribute("ws-attr-holes"), [" "], 1);
    e_1.removeAttribute("ws-attr-holes");
    for(let i=0, _6=holeAttrs.length-1;i<=_6;i++){
      let _5;
      const attrName=get(holeAttrs, i);
      const s=e_1.getAttribute(attrName);
      let m=null;
      let lastIndex=0;
      const res=[];
      while(m=re.exec(s),m!==null)
        {
          const textBefore=string(s, Some(lastIndex), Some(re.lastIndex-get(m, 0).length-1));
          lastIndex=re.lastIndex;
          res.push([textBefore, get(m, 1)]);
        }
      const finalText=string(s, Some(lastIndex), null);
      re.lastIndex=0;
      const value=foldBack((_7, _8) =>(((t) => {
        const textBefore_1=t[0];
        const holeName=t[1];
        return(t_1) => {
          const textAfter=t_1[0];
          const views=t_1[1];
          let o=null;
          const m_1=[fw.TryGetValue(holeName, {get:() => o, set:(v) => {
            o=v;
          }}), o];
          const holeContent=m_1[0]?m_1[1].AsChoiceView:Choice1Of2("");
          return holeContent.$==1?[textBefore_1, FSharpList.Cons(textAfter==""?holeContent.$0:Map((s_5) => s_5+textAfter, holeContent.$0), views)]:[textBefore_1+holeContent.$0+textAfter, views];
        };
      })(_7))(_8), res, [finalText, FSharpList.Empty]);
      if(value[1].$==1){
        if(value[1].$1.$==1){
          if(value[1].$1.$1.$==1){
            if(value[1].$1.$1.$1.$==0){
              const s_1=value[0];
              _5=Dynamic(attrName, Map3((_7, _8, _9) => s_1+_7+_8+_9, value[1].$0, value[1].$1.$0, value[1].$1.$1.$0));
            }
            else {
              const s_2=value[0];
              _5=Dynamic(attrName, Map((vs) => s_2+concat("", vs), Sequence(value[1])));
            }
          }
          else {
            const s_3=value[0];
            _5=Dynamic(attrName, Map2((_7, _8) => s_3+_7+_8, value[1].$0, value[1].$1.$0));
          }
        }
        else if(value[0]=="")_5=Dynamic(attrName, value[1].$0);
        else {
          const s_4=value[0];
          _5=Dynamic(attrName, Map((v) => s_4+v, value[1].$0));
        }
      }
      else _5=Attr.Create(attrName, value[0]);
      addAttr(e_1, _5);
    }
  });
  const _2=els;
  const _3=afterRender.length==0?Some(() => {
    wsdomHandling();
  }):Some((el_1) => {
    wsdomHandling();
    iter_2((f) => {
      f(el_1);
    }, afterRender);
  });
  const _4=!Equals(els, null)&&els.length===1&&(get(els, 0)instanceof Node&&(get(els, 0)instanceof Element&&(_1=get(els, 0),true)))?Some(_1):null;
  return[DeleteEmptyFields({
    Els:_2, 
    Dirty:true, 
    Holes:holes, 
    Attrs:attrs, 
    Render:_3?_3.$0:void 0, 
    El:_4?_4.$0:void 0
  }, ["Render", "El"]), TreeReduce(Const(), Map2Unit, updates)];
}
function GlobalHoles(){
  return _c_2.GlobalHoles;
}
function TextHoleRE(){
  return _c_2.TextHoleRE;
}
function foreachNotPreservedwsDOM(selector, f){
  IterSelectorDoc(selector, (p) => {
    if(p.closest("[ws-preserve]")==null)f(p);
  });
}
class Doc extends Object_1 {
  docNode;
  updates;
  static RunById(id, tr){
    const m=globalThis.document.getElementById(id);
    if(Equals(m, null))FailWith("invalid id: "+id);
    else Doc.Run(m, tr);
  }
  static Run(parent, doc){
    LinkElement(parent, doc.docNode);
    Doc.RunInPlace(false, parent, doc);
  }
  static Element(name, attr_1, children){
    const a=Attr.Concat(attr_1);
    const c=Doc.Concat(children);
    return Elt_1.New(globalThis.document.createElement(name), a, c);
  }
  static TextView(txt){
    const node=CreateTextNode();
    return Doc.Mk(TextDoc(node), Map((t) => {
      UpdateTextNode(node, t);
    }, txt));
  }
  static Concat(xs){
    return TreeReduce(Doc.Empty, Doc.Append, ofSeqNonCopying(xs));
  }
  static Input(attr_1, var_1){
    return Doc.InputInternal("input", () => append_1(attr_1, [Value(var_1)]));
  }
  static TextNode(v){
    return Doc.Mk(TextNodeDoc(globalThis.document.createTextNode(v)), Const());
  }
  static CheckBox(attrs, chk){
    return Doc.InputInternal("input", () => append_1(attrs, [Attr.Create("type", "checkbox"), Checked(chk)]));
  }
  static ColorInput(attr_1, var_1){
    return Doc.InputInternal("input", () => append_1(attr_1, [Value(var_1), Attr.Create("type", "color")]));
  }
  static PasswordBox(attr_1, var_1){
    return Doc.InputInternal("input", () => append_1(attr_1, [Value(var_1), Attr.Create("type", "password")]));
  }
  static DateInput(attr_1, var_1){
    return Doc.InputInternal("input", () => append_1(attr_1, [Value(var_1), Attr.Create("type", "date")]));
  }
  static RangeInput(attr_1, var_1){
    return Doc.InputInternal("input", () => append_1(attr_1, [IntValueUnchecked(var_1), Attr.Create("type", "range")]));
  }
  static Select(attrs, show, options, current){
    return Doc.SelectImpl(attrs, show, (rOptions) => {
      rOptions[0]=options;
      return Doc.Concat(mapi((_1, _2) => Doc.Element("option", ofArray([Attr.Create("value", String(_1))]), ofArray([Doc.TextNode(show(_2))])), options));
    }, current);
  }
  static FileInput(attr_1, var_1){
    return Doc.InputInternal("input", () => append_1(attr_1, [FileValue(var_1), Attr.Create("type", "file")]));
  }
  static RunInPlace(childrenOnly, parent, doc){
    const st=CreateRunState(parent, doc.docNode);
    Sink(get_UseAnimations()||BatchUpdatesEnabled()?StartProcessor(PerformAnimatedUpdate(childrenOnly, st, doc.docNode)):() => {
      PerformSyncUpdate(childrenOnly, st, doc.docNode);
    }, doc.updates);
  }
  static Mk(node, updates){
    return new Doc(node, updates);
  }
  static BindView(f, view){
    return Doc.EmbedView(Map(f, view));
  }
  static InputArea(attr_1, var_1){
    return Doc.InputInternal("textarea", () => append_1(attr_1, [Value(var_1)]));
  }
  static IntInput(attr_1, var_1){
    return Doc.InputInternal("input", () => append_1(attr_1, [IntValue(var_1), Attr.Create("type", "number")]));
  }
  static CheckBoxGroup(attrs, item, chk){
    const p=(y) => Equals(item, y);
    return Doc.CheckBox(attrs, _c_1.Lens(chk, (l) => exists(p, l), (_1, _2) => _2?exists((y) => Equals(item, y), _1)?_1:FSharpList.Cons(item, _1):filter((y) =>!Equals(item, y), _1)));
  }
  static Append(a, b){
    return Doc.Mk(AppendDoc(a.docNode, b.docNode), Map2Unit(a.updates, b.updates));
  }
  static get Empty(){
    return Doc.Mk(null, Const());
  }
  static Radio(attrs, value, var_1){
    const el=globalThis.document.createElement("input");
    el.addEventListener("click", () => var_1.Set(value), false);
    const valAttr=DynamicProp("checked", Map((x) => Equals(x, value), var_1.View));
    const _____=Attr.Create;
    return Elt_1.New(el, Attr.Concat(append(ofArray([_____("type", "radio"), _____("name", var_1.Id), valAttr]), ofSeq(attrs))), Doc.Empty);
  }
  static SelectMultiple(attrs, show, options, current){
    return Doc.SelectMultipleImpl(attrs, (rOptions) => {
      rOptions[0]=options;
      return Doc.Concat(mapi((_1, _2) => Doc.Element("option", ofArray([Attr.Create("value", String(_1))]), ofArray([Doc.TextNode(show(_2))])), options));
    }, current);
  }
  static EmbedView(view){
    const node=CreateEmbedNode();
    return Doc.Mk(EmbedDoc(node), Map(() => { }, Bind((doc) => {
      UpdateEmbedNode(node, doc.docNode);
      return doc.updates;
    }, view)));
  }
  static Convert(render, view){
    return Doc.Flatten(MapSeqCached(render, view));
  }
  static Button(caption, attrs, action){
    const attrs_1=Attr.Concat(attrs);
    return Elt_1.New(Doc.Clickable("button", action), attrs_1, Doc.TextNode(caption));
  }
  static InputInternal(elemTy, attr_1){
    const el=globalThis.document.createElement(elemTy);
    return Elt_1.New(el, Attr.Concat(attr_1(el)), Doc.Empty);
  }
  static ConvertBy(key, render, view){
    return Doc.Flatten(MapSeqCachedBy(key, render, view));
  }
  static SelectImpl(attrs, show, optionElements, current){
    const options=[FSharpList.Empty];
    const setSelectedItem=(el_1, item) => {
      el_1.selectedIndex=findIndex((y) => Equals(item, y), options[0]);
    };
    const el=globalThis.document.createElement("select");
    const selectedItemAttr=DynamicCustom((_1) =>(_2) => setSelectedItem(_1, _2), current.View);
    el.addEventListener("change", () => current.UpdateMaybe((x) => {
      const i=el.selectedIndex;
      const y=options[0].get_Item(i);
      return Equals(x, y)?null:Some(y);
    }), false);
    return Elt_1.New(el, AppendTree(OnAfterRender((el_1) => {
      setSelectedItem(el_1, current.Get());
    }), AppendTree(selectedItemAttr, Attr.Concat(attrs))), optionElements(options));
  }
  static RunBefore(rdelim, doc){
    const ldelim=globalThis.document.createTextNode("");
    rdelim.parentNode.insertBefore(ldelim, rdelim);
    Doc.RunBetween(ldelim, rdelim, doc);
  }
  static SelectMultipleImpl(attrs, optionElements, current){
    const options=[FSharpList.Empty];
    const setSelectedItem=(el_1, item) => {
      const options_=el_1.options;
      for(let i=0, _1=options_.length-1;i<=_1;i++){
        const option=options_.item(i);
        option.selected=contains(options[0].get_Item(option.index), item);
      }
    };
    const el=globalThis.document.createElement("select");
    const selectedItemAttr=DynamicCustom((_1) =>(_2) => setSelectedItem(_1, _2), current.View);
    el.addEventListener("change", () => current.UpdateMaybe((x) => {
      const selectedOptions=el.selectedOptions;
      const l=FSharpList.Cons(void 0, void 0);
      let last=l;
      for(const i of Iter_1(range(0, selectedOptions.length-1))){
        const option=selectedOptions.item(i);
        const c=FSharpList.Cons(options[0].get_Item(option.index), void 0);
        last.$1=c;
        last=c;
      }
      last.$1=FSharpList.Empty;
      const y=l.$1;
      return Equals(x, y)?null:Some(y);
    }), false);
    return Elt_1.New(el, AppendTree(Attr.Create("multiple", ""), AppendTree(OnAfterRender((el_1) => {
      setSelectedItem(el_1, current.Get());
    }), AppendTree(selectedItemAttr, Attr.Concat(attrs)))), optionElements(options));
  }
  static Flatten(view){
    return Doc.EmbedView(Map((x) => Doc.Concat(x), view));
  }
  static Clickable(elem, action){
    const el=globalThis.document.createElement(elem);
    el.addEventListener("click", (ev) => {
      ev.preventDefault();
      return action();
    }, false);
    return el;
  }
  static RunBetween(ldelim, rdelim, doc){
    LinkPrevElement(rdelim, doc.docNode);
    const st=CreateDelimitedRunState(ldelim, rdelim, doc.docNode);
    Sink(get_UseAnimations()||BatchUpdatesEnabled()?StartProcessor(PerformAnimatedUpdate(false, st, doc.docNode)):() => {
      PerformSyncUpdate(false, st, doc.docNode);
    }, doc.updates);
  }
  ReplaceInDom(elt){
    const rdelim=globalThis.document.createTextNode("");
    elt.parentNode.replaceChild(rdelim, elt);
    Doc.RunBefore(rdelim, this);
  }
  constructor(docNode, updates){
    super();
    this.docNode=docNode;
    this.updates=updates;
  }
}
function NewFromSeq(fields){
  const r={};
  const e=Get_1(fields);
  try {
    while(e.MoveNext())
      {
        const f=e.Current;
        r[f[0]]=f[1];
      }
  }
  finally {
    if(typeof e=="object"&&isIDisposable(e))e.Dispose();
  }
  return r;
}
class Attr {
  static Create(name, value){
    return Attr.A3((el) => {
      el.setAttribute(name, value);
    });
  }
  static Concat(xs){
    const x=ofSeqNonCopying(xs);
    return TreeReduce(EmptyAttr(), (_1, _2) => AppendTree(_1, _2), x);
  }
  static A3(init_4){
    return Create_3(Attr, {$:3, $0:init_4});
  }
  static A1(Item){
    return Create_3(Attr, {$:1, $0:Item});
  }
  static A4(onAfterRender){
    return Create_3(Attr, {$:4, $0:onAfterRender});
  }
  static HandlerImpl(event, q){
    return Attr.A3((el) => {
      el.addEventListener(event, (o) =>(q(el))(o), false);
    });
  }
  static A2(Item1, Item2){
    return Create_3(Attr, {
      $:2, 
      $0:Item1, 
      $1:Item2
    });
  }
  $;
  $0;
  $1;
}
let _c=Lazy((_i) => class $StartupCode_Main {
  static {
    _c=_i(this);
  }
  static themeLabel;
  static theme;
  static themeStorageKey;
  static LensAndSubmitterTest;
  static DocTest;
  static AttrTest;
  static InputTest;
  static EltTest;
  static SerializerTest;
  static ListModelTest;
  static ViewTest;
  static VarTest;
  static {
    let m=0;
    const v=_c_1.Create_1(New_3(0, 0, 0));
    function f(n){
      const w=v.Get();
      v.Set(New_3(w.i+1, n, n-w.n));
      requestAnimationFrame((x) => {
        m=m+1;
        f(x);
      });
    }
    const unchanging=_c_1.Create_1("unchanging");
    requestAnimationFrame(f);
    const _1=Doc.Element("div", [Class("metrics")], [Doc.Element("div", [Class("metric")], [Doc.Element("span", [Class("metric-label")], [Doc.TextNode("Frame")]), Doc.Element("span", [Class("metric-value")], [Doc.TextView(Map((t) => String(t.i), v.View))])]), Doc.Element("div", [Class("metric")], [Doc.Element("span", [Class("metric-label")], [Doc.TextNode("Started")]), Doc.Element("span", [Class("metric-value")], [Doc.TextView(Map((t) => t.n.toFixed(1), v.View))])]), Doc.Element("div", [Class("metric")], [Doc.Element("span", [Class("metric-label")], [Doc.TextNode("Last frame")]), Doc.Element("span", [Class("metric-value")], [Doc.TextView(Map((t) => t.d.toFixed(1)+" ms", v.View))])]), Doc.Element("div", [Style("display", "none")], [Doc.TextView(unchanging.View)])]);
    LoadLocalTemplates("");
    Doc.RunById("stresstest", _1);
    const _2=TestCategory("Var");
    const s={name:_2.name, run:() => {
      Test("Create").Run(Runner.Yield([_c_1.Create_1(1), _c_1.Create_1("a")]).Expect(() => 0));
      Test("CreateWaiting").Run(Runner.Yield(_c_1.CreateWaiting()).EqualMsg((rv_3) => rv_3.Get(), () => null, "initial").For((a) => {
        const count=[0];
        const m_1=Map((x) => {
          count[0]=count[0]+1;
          return x==null?"wrong":x+"!";
        }, a.View);
        a.Set("hi");
        return Runner.Yield([a, count, m_1]);
      }).EqualMsgAsync((t) => GetAsync(t[2]), () =>"hi!", "value set").EqualMsg((t) => t[1][0], () => 1, "function call count"));
      Test("Value").Run(Runner.Yield(_c_1.Create_1(2)).EqualMsg((rv_3) => rv_3.Get(), () => 2, "get Value").EqualMsg((rv_3) => rv_3.Get(), () => 2, "Var.Get").For((a) => {
        a.Set(42);
        return Runner.Yield(a);
      }).EqualMsg((rv_3) => rv_3.Get(), () => 42, "set Value").For((a) => {
        _c_1.Set(a, 35);
        return Runner.Yield(a);
      }).EqualMsg((rv_3) => rv_3.Get(), () => 35, "Var.Set"));
      const _11=Test("Update");
      const rv=_c_1.Create_1(4);
      _c_1.Update(rv, (y) => 16+y);
      _11.Run(Runner.Yield(rv).Equal((rv_3) => rv_3.Get(), () => 20));
      const _12=Test("SetFinal");
      const rv_1=_c_1.Create_1(5);
      _c_1.SetFinal(rv_1, 27);
      _12.Run(Runner.Yield(rv_1).EqualMsg((rv_3) => rv_3.Get(), () => 27, "SetFinal sets").For((a) => {
        a.Set(33);
        return Runner.Yield(a);
      }).EqualMsg((rv_3) => rv_3.Get(), () => 27, "Subsequently setting changes nothing"));
      const _13=Test("View");
      const rv_2=_c_1.Create_1(3);
      _13.Run(Runner.Yield([rv_2, GetAsync(rv_2.View)]).EqualMsgAsync((t) => t[1], () => 3, "initial").For((a) => {
        const rv_3=a[0];
        rv_3.Set(57);
        return Runner.Yield([rv_3, a[1]]);
      }).EqualMsgAsync((t) => t[1], () => 57, "after set"));
    }};
    globalThis.QUnit.module(s.name);
    this.VarTest=s;
    const _3=TestCategory("View");
    const s_1={name:_3.name, run:() => {
      Test("Const").Run(Runner.Yield(GetAsync(Const(12))).EqualAsync((v_2) => v_2, () => 12));
      const _11=Test("ConstAsync");
      const f_1=() => Return(86);
      const a=Delay((_33, _34) =>(f_1(_33))(_34));
      _11.Run(Runner.Yield([a, GetAsync(ConstAsync(a))]).EqualAsync((t) => t[1], () => 86));
      const _12=Test("FromVar");
      const rv=_c_1.Create_1(38);
      _12.Run(Runner.Yield([rv, GetAsync(rv.View)]).EqualMsgAsync((t) => t[1], () => 38, "initial").For((a_1) => {
        const rv_7=a_1[0];
        rv_7.Set(92);
        return Runner.Yield([rv_7, a_1[1]]);
      }).EqualMsgAsync((t) => t[1], () => 92, "after set"));
      const _13=Test("Map");
      const count=[0];
      const rv_1=_c_1.Create_1(7);
      _13.Run(Runner.Yield([count, rv_1, GetAsync(Map((x) => {
        count[0]=count[0]+1;
        return x+15;
      }, rv_1.View))]).EqualMsgAsync((t) => t[2], () => 7+15, "initial").For((a_1) => {
        const rv_7=a_1[1];
        rv_7.Set(23);
        return Runner.Yield([a_1[0], rv_7, a_1[2]]);
      }).EqualMsgAsync((t) => t[2], () => 23+15, "after set").EqualMsg((t) => t[0][0], () => 2, "function call count"));
      const _14=Test("MapCached");
      const count_1=[0];
      const rv_2=_c_1.Create_1(9);
      _14.Run(Runner.Yield([count_1, rv_2, GetAsync(MapCached((x) => {
        count_1[0]=count_1[0]+1;
        return x+21;
      }, rv_2.View))]).EqualMsgAsync((t) => t[2], () => 9+21, "initial").For((a_1) => {
        const rv_7=a_1[1];
        rv_7.Set(66);
        return Runner.Yield([a_1[0], rv_7, a_1[2]]);
      }).EqualMsgAsync((t) => t[2], () => 66+21, "after set").For((a_1) => {
        const rv_7=a_1[1];
        rv_7.Set(66);
        return Runner.Yield([a_1[0], rv_7, a_1[2]]);
      }).EqualMsgAsync((t) => t[2], () => 66+21, "after set to the same value").EqualMsg((t) => t[0][0], () => 2, "function call count"));
      const _15=Test("MapCachedBy");
      const count_2=[0];
      const rv_3=_c_1.Create_1(9);
      _15.Run(Runner.Yield([count_2, rv_3, GetAsync(MapCachedBy((_33, _34) => _33%10===_34%10, (x) => {
        count_2[0]=count_2[0]+1;
        return x+21;
      }, rv_3.View))]).EqualMsgAsync((t) => t[2], () => 9+21, "initial").For((a_1) => {
        const rv_7=a_1[1];
        rv_7.Set(66);
        return Runner.Yield([a_1[0], rv_7, a_1[2]]);
      }).EqualMsgAsync((t) => t[2], () => 66+21, "after set").For((a_1) => {
        const rv_7=a_1[1];
        rv_7.Set(56);
        return Runner.Yield([a_1[0], rv_7, a_1[2]]);
      }).EqualMsgAsync((t) => t[2], () => 66+21, "after set to the same value").EqualMsg((t) => t[0][0], () => 2, "function call count"));
      const _16=Test("MapAsync");
      const count_3=[0];
      const rv_4=_c_1.Create_1(11);
      _16.Run(Runner.Yield([count_3, rv_4, GetAsync(MapAsync((x) => {
        const f_2=() => {
          count_3[0]=count_3[0]+1;
          return Return(x*43);
        };
        return Delay((_33, _34) =>(f_2(_33))(_34));
      }, rv_4.View))]).EqualMsgAsync((t) => t[2], () => 11*43, "initial").For((a_1) => {
        const rv_7=a_1[1];
        rv_7.Set(45);
        return Runner.Yield([a_1[0], rv_7, a_1[2]]);
      }).EqualMsgAsync((t) => t[2], () => 45*43, "after set").EqualMsg((t) => t[0][0], () => 2, "function call count"));
      const _17=Test("Map2");
      const count_4=[0];
      const rv1=_c_1.Create_1(29);
      const rv2=_c_1.Create_1(8);
      _17.Run(Runner.Yield([count_4, rv1, rv2, GetAsync(Map2((_33, _34) => {
        count_4[0]=count_4[0]+1;
        return _33*_34;
      }, rv1.View, rv2.View))]).EqualMsgAsync((t) => t[3], () => 29*8, "initial").For((a_1) => {
        const rv1_8=a_1[1];
        rv1_8.Set(78);
        return Runner.Yield([a_1[0], rv1_8, a_1[2], a_1[3]]);
      }).EqualMsgAsync((t) => t[3], () => 78*8, "after set v1").For((a_1) => {
        const rv2_8=a_1[2];
        rv2_8.Set(30);
        return Runner.Yield([a_1[0], a_1[1], rv2_8, a_1[3]]);
      }).EqualMsgAsync((t) => t[3], () => 78*30, "after set v2").EqualMsg((t) => t[0][0], () => 3, "function call count"));
      const _18=Test("MapAsync2");
      const count_5=[0];
      const rv1_1=_c_1.Create_1(36);
      const rv2_1=_c_1.Create_1(22);
      _18.Run(Runner.Yield([count_5, rv1_1, rv2_1, GetAsync(MapAsync2((_33, _34) => {
        const f_2=() => {
          count_5[0]=count_5[0]+1;
          return Return(_33-_34);
        };
        return Delay((_35, _36) =>(f_2(_35))(_36));
      }, rv1_1.View, rv2_1.View))]).EqualMsgAsync((t) => t[3], () => 36-22, "initial").For((a_1) => {
        const rv1_8=a_1[1];
        rv1_8.Set(82);
        return Runner.Yield([a_1[0], rv1_8, a_1[2], a_1[3]]);
      }).EqualMsgAsync((t) => t[3], () => 82-22, "after set v1").For((a_1) => {
        const rv2_8=a_1[2];
        rv2_8.Set(13);
        return Runner.Yield([a_1[0], a_1[1], rv2_8, a_1[3]]);
      }).EqualMsgAsync((t) => t[3], () => 82-13, "after set v2").EqualMsg((t) => t[0][0], () => 3, "function call count"));
      const _19=Test("Apply");
      const count_6=[0];
      const rv1_2=_c_1.Create_1((x) => {
        count_6[0]=count_6[0]+1;
        return x+5;
      });
      const rv2_2=_c_1.Create_1(57);
      _19.Run(Runner.Yield([count_6, rv1_2, rv2_2, GetAsync(Apply(rv1_2.View, rv2_2.View))]).EqualMsgAsync((t) => t[3], () => 57+5, "initial").For((a_1) => {
        const rv1_8=a_1[1];
        const count_8=a_1[0];
        rv1_8.Set((x) => {
          count_8[0]=count_8[0]+1;
          return x*4;
        });
        return Runner.Yield([count_8, rv1_8, a_1[2], a_1[3]]);
      }).EqualMsgAsync((t) => t[3], () => 57*4, "after set v1").For((a_1) => {
        const rv2_8=a_1[2];
        rv2_8.Set(33);
        return Runner.Yield([a_1[0], a_1[1], rv2_8, a_1[3]]);
      }).EqualMsgAsync((t) => t[3], () => 33*4, "after set v2").EqualMsg((t) => t[0][0], () => 3, "function call count"));
      const _20=Test("Join");
      const count_7=[0];
      const rv1_3=_c_1.Create_1(76);
      const rv2_3=_c_1.Create_1(rv1_3.View);
      _20.Run(Runner.Yield([count_7, rv1_3, rv2_3, GetAsync(Join(Map((x) => {
        count_7[0]=count_7[0]+1;
        return x;
      }, rv2_3.View)))]).EqualMsgAsync((t) => t[3], () => 76, "initial").For((a_1) => {
        const rv1_8=a_1[1];
        rv1_8.Set(44);
        return Runner.Yield([a_1[0], rv1_8, a_1[2], a_1[3]]);
      }).EqualMsgAsync((t) => t[3], () => 44, "after set inner").For((a_1) => {
        const rv2_8=a_1[2];
        const count_8=a_1[0];
        rv2_8.Set(Map((x) => {
          count_8[0]=count_8[0]+1;
          return x;
        }, Const(39)));
        return Runner.Yield([count_8, a_1[1], rv2_8, a_1[3]]);
      }).EqualMsgAsync((t) => t[3], () => 39, "after set outer").EqualMsg((t) => t[0][0], () => 3, "function call count"));
      const _21=Test("Bind");
      const outerCount=[0];
      const innerCount=[0];
      const rv1_4=_c_1.Create_1(93);
      const rv2_4=_c_1.Create_1(27);
      _21.Run(Runner.Yield([outerCount, innerCount, rv1_4, rv2_4, GetAsync(BindInner((x) => {
        outerCount[0]=outerCount[0]+1;
        return Map((y) => {
          innerCount[0]=innerCount[0]+1;
          return x+y;
        }, rv1_4.View);
      }, rv2_4.View))]).EqualMsgAsync((t) => t[4], () => 93+27, "initial").For((a_1) => {
        const rv1_8=a_1[2];
        rv1_8.Set(74);
        return Runner.Yield([a_1[0], a_1[1], rv1_8, a_1[3], a_1[4]]);
      }).EqualMsgAsync((t) => t[4], () => 74+27, "after set inner").For((a_1) => {
        const rv2_8=a_1[3];
        rv2_8.Set(22);
        return Runner.Yield([a_1[0], a_1[1], a_1[2], rv2_8, a_1[4]]);
      }).EqualMsgAsync((t) => t[4], () => 74+22, "after set outer").EqualMsg((t) =>[t[0][0], t[1][0]], () =>[2, 3], "function call count"));
      const _22=Test("Bind with outside Map");
      const outerCount_1=[0];
      const innerCount1=[0];
      const innerCount2=[0];
      const o=_c_1.Create_1(true);
      const i1=_c_1.Create_1(1);
      const i2=_c_1.Create_1(2);
      const m1=Map((x) => {
        innerCount1[0]=innerCount1[0]+1;
        return x*x;
      }, i1.View);
      const m2=Map((x) => {
        innerCount2[0]=innerCount2[0]+1;
        return x*x;
      }, i2.View);
      _22.Run(Runner.Yield([outerCount_1, innerCount1, innerCount2, o, i1, i2, m1, m2, GetAsync(Bind((x) => {
        outerCount_1[0]=outerCount_1[0]+1;
        return x?m1:m2;
      }, o.View))]).EqualMsgAsync((t) => t[8], () => 1, "initial").For((a_1) => {
        const i1_2=a_1[4];
        i1_2.Set(3);
        return Runner.Yield([a_1[0], a_1[1], a_1[2], a_1[3], i1_2, a_1[5], a_1[6], a_1[7], a_1[8]]);
      }).EqualMsgAsync((t) => t[8], () => 9, "after set inner").For((a_1) => {
        const o_2=a_1[3];
        o_2.Set(true);
        return Runner.Yield([a_1[0], a_1[1], a_1[2], o_2, a_1[4], a_1[5], a_1[6], a_1[7], a_1[8]]);
      }).EqualMsgAsync((t) => t[8], () => 9, "after set outer unchanged").For((a_1) => {
        const o_2=a_1[3];
        o_2.Set(false);
        return Runner.Yield([a_1[0], a_1[1], a_1[2], o_2, a_1[4], a_1[5], a_1[6], a_1[7], a_1[8]]);
      }).EqualMsgAsync((t) => t[8], () => 4, "after set outer").EqualMsg((t) =>[t[0][0], t[1][0], t[2][0]], () =>[3, 2, 1], "function call count"));
      const _23=Test("BindInner with outside Map");
      const outerCount_2=[0];
      const innerCount1_1=[0];
      const innerCount2_1=[0];
      const o_1=_c_1.Create_1(true);
      const i1_1=_c_1.Create_1(1);
      const i2_1=_c_1.Create_1(2);
      const m1_1=Map((x) => {
        innerCount1_1[0]=innerCount1_1[0]+1;
        return x*x;
      }, i1_1.View);
      const m2_1=Map((x) => {
        innerCount2_1[0]=innerCount2_1[0]+1;
        return x*x;
      }, i2_1.View);
      _23.Run(Runner.Yield([outerCount_2, innerCount1_1, innerCount2_1, o_1, i1_1, i2_1, m1_1, m2_1, GetAsync(BindInner((x) => {
        outerCount_2[0]=outerCount_2[0]+1;
        return x?m1_1:m2_1;
      }, o_1.View))]).EqualMsgAsync((t) => t[8], () => 1, "initial").For((a_1) => {
        const i1_2=a_1[4];
        i1_2.Set(3);
        return Runner.Yield([a_1[0], a_1[1], a_1[2], a_1[3], i1_2, a_1[5], a_1[6], a_1[7], a_1[8]]);
      }).EqualMsgAsync((t) => t[8], () => 9, "after set inner").For((a_1) => {
        const o_2=a_1[3];
        o_2.Set(true);
        return Runner.Yield([a_1[0], a_1[1], a_1[2], o_2, a_1[4], a_1[5], a_1[6], a_1[7], a_1[8]]);
      }).EqualMsgAsync((t) => t[8], () => 9, "after set outer unchanged").For((a_1) => {
        const o_2=a_1[3];
        o_2.Set(false);
        return Runner.Yield([a_1[0], a_1[1], a_1[2], o_2, a_1[4], a_1[5], a_1[6], a_1[7], a_1[8]]);
      }).EqualMsgAsync((t) => t[8], () => 4, "after set outer").EqualMsg((t) =>[t[0][0], t[1][0], t[2][0]], () =>[3, 3, 1], "function call count"));
      const _24=Test("UpdateWhile");
      const outerCount_3=[0];
      const innerCount_1=[0];
      const rv1_5=_c_1.Create_1(false);
      const v1=Map((x) => {
        outerCount_3[0]=outerCount_3[0]+1;
        return x;
      }, rv1_5.View);
      const rv2_5=_c_1.Create_1(0);
      const v2=Map((x) => {
        innerCount_1[0]=innerCount_1[0]+1;
        return x;
      }, rv2_5.View);
      _24.Run(Runner.Yield([outerCount_3, innerCount_1, rv1_5, v1, rv2_5, v2, GetAsync(UpdateWhile(1, v1, v2))]).EqualMsgAsync((t) => t[6], () => 1, "initial").For((a_1) => {
        const rv2_8=a_1[4];
        rv2_8.Set(27);
        return Runner.Yield([a_1[0], a_1[1], a_1[2], a_1[3], rv2_8, a_1[5], a_1[6]]);
      }).EqualMsgAsync((t) => t[6], () => 1, "changing inner should have no effect").For((a_1) => {
        const rv1_8=a_1[2];
        rv1_8.Set(true);
        return Runner.Yield([a_1[0], a_1[1], rv1_8, a_1[3], a_1[4], a_1[5], a_1[6]]);
      }).EqualMsgAsync((t) => t[6], () => 27, "after set pred true").For((a_1) => {
        const rv2_8=a_1[4];
        rv2_8.Set(22);
        return Runner.Yield([a_1[0], a_1[1], a_1[2], a_1[3], rv2_8, a_1[5], a_1[6]]);
      }).EqualMsgAsync((t) => t[6], () => 22, "after set inner").For((a_1) => {
        const rv1_8=a_1[2];
        rv1_8.Set(false);
        return Runner.Yield([a_1[0], a_1[1], rv1_8, a_1[3], a_1[4], a_1[5], a_1[6]]);
      }).EqualMsgAsync((t) => t[6], () => 22, "after set pred false").For((a_1) => {
        const rv2_8=a_1[4];
        rv2_8.Set(0);
        return Runner.Yield([a_1[0], a_1[1], a_1[2], a_1[3], rv2_8, a_1[5], a_1[6]]);
      }).EqualMsgAsync((t) => t[6], () => 22, "changing inner should have no effect").EqualMsg((t) =>[t[0][0], t[1][0]], () =>[3, 2], "function call count"));
      const _25=Test("SnapShotOn");
      const outerCount_4=[0];
      const innerCount_2=[0];
      const rv1_6=_c_1.Create_1();
      const v1_1=Map(() => {
        outerCount_4[0]=outerCount_4[0]+1;
      }, rv1_6.View);
      const rv2_6=_c_1.Create_1(0);
      const v2_1=Map((x) => {
        innerCount_2[0]=innerCount_2[0]+1;
        return x;
      }, rv2_6.View);
      _25.Run(Runner.Yield([outerCount_4, innerCount_2, rv1_6, v1_1, rv2_6, v2_1, GetAsync(SnapshotOn(1, v1_1, v2_1))]).EqualMsgAsync((t) => t[6], () => 1, "initial").For((a_1) => {
        const rv2_8=a_1[4];
        rv2_8.Set(27);
        return Runner.Yield([a_1[0], a_1[1], a_1[2], a_1[3], rv2_8, a_1[5], a_1[6]]);
      }).EqualMsgAsync((t) => t[6], () => 1, "changing inner should have no effect").For((a_1) => {
        const rv1_8=a_1[2];
        rv1_8.Set(void 0);
        return Runner.Yield([a_1[0], a_1[1], rv1_8, a_1[3], a_1[4], a_1[5], a_1[6]]);
      }).EqualMsgAsync((t) => t[6], () => 27, "after taking snapshot").For((a_1) => {
        const rv2_8=a_1[4];
        rv2_8.Set(22);
        return Runner.Yield([a_1[0], a_1[1], a_1[2], a_1[3], rv2_8, a_1[5], a_1[6]]);
      }).EqualMsgAsync((t) => t[6], () => 27, "changing inner should have no effect").For((a_1) => {
        const rv1_8=a_1[2];
        rv1_8.Set(void 0);
        return Runner.Yield([a_1[0], a_1[1], rv1_8, a_1[3], a_1[4], a_1[5], a_1[6]]);
      }).EqualMsgAsync((t) => t[6], () => 22, "after taking snapshot").EqualMsg((t) =>[t[0][0], t[1][0]], () =>[3, 2], "function call count"));
      const _26=Test("Sequence");
      const seqCount=[0];
      const innerCount_3=[0];
      const rv1_7=_c_1.Create_1(93);
      const rv2_7=_c_1.Create_1(27);
      const v2_2=Map((x) => {
        innerCount_3[0]=innerCount_3[0]+1;
        return x;
      }, rv2_7.View);
      const rvs=Iterate(function*(){
        seqCount[0]=seqCount[0]+1;
        yield rv1_7.View;
        if(seqCount[0]===2)yield v2_2;
      });
      _26.Run(Runner.Yield([seqCount, innerCount_3, rv1_7, rv2_7, v2_2, rvs, GetAsync(Map(ofSeq, Sequence(rvs)))]).EqualMsgAsync((t) => t[6], () => ofArray([93]), "initial").For((a_1) => {
        const rv1_8=a_1[2];
        rv1_8.Set(94);
        return Runner.Yield([a_1[0], a_1[1], rv1_8, a_1[3], a_1[4], a_1[5], a_1[6]]);
      }).EqualMsgAsync((t) => t[6], () => ofArray([94, 27]), "setting an item").For((a_1) => {
        const rv2_8=a_1[3];
        rv2_8.Set(0);
        return Runner.Yield([a_1[0], a_1[1], a_1[2], rv2_8, a_1[4], a_1[5], a_1[6]]);
      }).EqualMsgAsync((t) => t[6], () => ofArray([94]), "setting an item").For((a_1) => {
        const rv2_8=a_1[3];
        rv2_8.Set(1);
        return Runner.Yield([a_1[0], a_1[1], a_1[2], rv2_8, a_1[4], a_1[5], a_1[6]]);
      }).EqualMsgAsync((t) => t[6], () => ofArray([94]), "setting an outside item").EqualMsg((t) =>[t[0][0], t[1][0]], () =>[3, 1], "function call count"));
      const _27=Test("Get");
      const rv_5=_c_1.Create_1(53);
      _27.Run(Runner.Yield([rv_5, FromContinuations((ok) => Get(ok, rv_5.View))]).EqualMsgAsync((t) => t[1], () => 53, "initial").For((a_1) => {
        const rv_7=a_1[0];
        rv_7.Set(84);
        return Runner.Yield([rv_7, a_1[1]]);
      }).EqualMsgAsync((t) => t[1], () => 84, "after set").For((a_1) => {
        const rv_7=a_1[0];
        const v_2=MapAsync((x) => {
          const f_2=() => {
            const f_3=() => Return(x);
            return Bind_1(Sleep(100), (_33, _34) =>(f_3(_33))(_34));
          };
          return Delay((_33, _34) =>(f_2(_33))(_34));
        }, rv_7.View);
        return Runner.Yield([rv_7, a_1[1], v_2, FromContinuations((ok) => Get(ok, v_2))]);
      }).EqualMsgAsync((t) => t[3], () => 84, "async before set").For((a_1) => {
        const rv_7=a_1[0];
        rv_7.Set(12);
        return Runner.Yield([rv_7, a_1[1], a_1[2], a_1[3]]);
      }).EqualMsgAsync((t) => t[3], () => 12, "async after set"));
      const _28=Test("GetAsync");
      const rv_6=_c_1.Create_1(79);
      _28.Run(Runner.Yield([rv_6, GetAsync(rv_6.View)]).EqualMsgAsync((t) => t[1], () => 79, "initial").For((a_1) => {
        const rv_7=a_1[0];
        rv_7.Set(62);
        return Runner.Yield([rv_7, a_1[1]]);
      }).EqualMsgAsync((t) => t[1], () => 62, "after set").For((a_1) => {
        const rv_7=a_1[0];
        const v_2=MapAsync((x) => {
          const f_2=() => {
            const f_3=() => Return(x);
            return Bind_1(Sleep(100), (_33, _34) =>(f_3(_33))(_34));
          };
          return Delay((_33, _34) =>(f_2(_33))(_34));
        }, rv_7.View);
        return Runner.Yield([rv_7, a_1[1], v_2, GetAsync(v_2)]);
      }).EqualMsgAsync((t) => t[3], () => 62, "async before set").For((a_1) => {
        const rv_7=a_1[0];
        rv_7.Set(43);
        return Runner.Yield([rv_7, a_1[1], a_1[2], a_1[3]]);
      }).EqualMsgAsync((t) => t[3], () => 43, "async after set"));
      Test("V").Run(Runner.Yield(Const(12)).EqualMsgAsync(GetAsync, () => 12, "Const").For((a_1) => Runner.Yield([a_1, Map((_33) =>"b"+String(_33), a_1)])).EqualMsgAsync((t) => GetAsync(t[1]), () =>"b12", "Map").For((a_1) => {
        const vconst=a_1[0];
        return Runner.Yield([vconst, a_1[1], Map((_33) => _33+_33, vconst)]);
      }).EqualMsgAsync((t) => GetAsync(t[2]), () => 24, "Map using the same view twice").For((a_1) => {
        const vmap=a_1[1];
        const vconst=a_1[0];
        return Runner.Yield([vconst, vmap, a_1[2], Map2((_33, _34) => String(_33)+"c"+_34, vconst, vmap)]);
      }).EqualMsgAsync((t) => GetAsync(t[3]), () =>"12cb12", "Map2").For((a_1) => {
        const vmap2=a_1[3];
        const vmap=a_1[1];
        const vconst=a_1[0];
        return Runner.Yield([vconst, vmap, a_1[2], vmap2, Apply(Map2((_33, _34) =>(_35) => _33+"d"+_34+"e"+String(_35), vmap2, vmap), vconst)]);
      }).EqualMsgAsync((t) => GetAsync(t[4]), () =>"12cb12db12e12", "Apply"));
      const _29=Test("WithInit");
      const e=new FSharpEvent();
      _29.Run(Runner.Yield([e, WithInit("waiting", ConstAsync(AwaitEvent(e.event, null)))]).EqualMsgAsync((t) => GetAsync(t[1]), () =>"waiting", "Initial value").For((a_1) => {
        const e_2=a_1[0];
        e_2.event.Trigger("done");
        return Runner.Yield([e_2, a_1[1]]);
      }).EqualMsgAsync((t) => GetAsync(t[1]), () =>"done", "Value after waiting"));
      const _30=Test("WithInitOption");
      const e_1=new FSharpEvent();
      _30.Run(Runner.Yield([e_1, WithInitOption(ConstAsync(AwaitEvent(e_1.event, null)))]).EqualMsgAsync((t) => GetAsync(t[1]), () => null, "Initial value").For((a_1) => {
        const e_2=a_1[0];
        e_2.event.Trigger("done");
        return Runner.Yield([e_2, a_1[1]]);
      }).EqualMsgAsync((t) => GetAsync(t[1]), () => Some("done"), "Value after waiting"));
      const _31=Test("Stress test");
      const n=400;
      const sheet=init_2(n, n, () => _c_1.Create_1(Const(0)));
      for(let i_1=1, _33=n-1;i_1<=_33;i_1++){
        const i=i_1;
        get2D(sheet, 0, i_1).Set(Join(get2D(sheet, 0, i_1-1).View));
        get2D(sheet, i_1, 0).Set(Join(get2D(sheet, i_1-1, 0).View));
        for(let j_1=1, _34=n-1;j_1<=_34;j_1++){
          const j=j_1;
          const set_1=(f_2) => {
            get2D(sheet, i, j).Set(Map2((_35, _36) =>(f_2(_35))(_36), Join(get2D(sheet, i-1, j).View), Join(get2D(sheet, i, j-1).View)));
          };
          if(i_1===100&&j_1===100)set_1((a_1) =>(b) => {
            console.log("calculating value at (100, 100)");
            return(a_1+b)%1000000;
          });
          else i_1===n-1&&j_1===n-1?set_1((a_1) =>(b) => {
            console.log("calculating final value");
            return(a_1+b)%1000000;
          }):set_1((a_1) =>(b) =>(a_1+b)%1000000);
        }
      }
      _31.Run(Runner.Yield([n, sheet]).EqualAsync((t) => {
        const n_2=t[0];
        return GetAsync(Join(get2D(t[1], n_2-1, n_2-1).View));
      }, () => 0).For((a_1) => {
        const sheet_1=a_1[1];
        get2D(sheet_1, 0, 0).Set(Const(1));
        return Runner.Yield([a_1[0], sheet_1]);
      }).EqualAsync((t) => {
        const n_2=t[0];
        return GetAsync(Join(get2D(t[1], n_2-1, n_2-1).View));
      }, () => 272000).For((a_1) => {
        globalThis.stressTest=a_1[1];
        return Runner.Zero();
      }));
      const _32=Test("Long chain");
      const n_1=1000;
      const v_1=_c_1.Create_1(0);
      _32.Run(Runner.Yield([n_1, v_1, nth(n_1-1, unfold((a_1) => {
        const b=Map((y) => 1+y, a_1);
        return Some([b, b]);
      }, v_1.View))]).EqualAsync((t) => GetAsync(t[2]), () => 1000).For((a_1) => {
        const v_2=a_1[1];
        v_2.Set(1);
        return Runner.Yield([a_1[0], v_2, a_1[2]]);
      }).EqualAsync((t) => GetAsync(t[2]), () => 1001));
    }};
    globalThis.QUnit.module(s_1.name);
    this.ViewTest=s_1;
    const _4=TestCategory("ListModel");
    const s_2={name:_4.name, run:() => {
      const _11=Test("Wrap");
      const u=Create((t) => t[0], ofArray([[1, "11"], [2, "22"]]));
      const l=u.Wrap((_12) =>[_12[0], _12[1]], (_12) => {
        const k=_12[0];
        return[k, _12[1], k];
      }, (_12, _13) =>(((_14) => {
        const f_1=_14[2];
        return(t) =>[t[0], t[1], f_1];
      })(_12))(_13));
      _11.Run(Runner.Yield([u, l, GetAsync(Map(ofSeq, u.v)), GetAsync(Map(ofSeq, l.v))]).EqualMsgAsync((t) => t[3], () => ofArray([[1, "11", 1], [2, "22", 2]]), "initialization").For((a) => {
        const u_1=a[0];
        u_1.UpdateBy(() => Some([1, "111"]), 1);
        return Runner.Yield([u_1, a[1], a[2], a[3]]);
      }).EqualMsgAsync((t) => t[3], () => ofArray([[1, "111", 1], [2, "22", 2]]), "update underlying item").For((a) => {
        const u_1=a[0];
        u_1.Append([3, "33"]);
        return Runner.Yield([u_1, a[1], a[2], a[3]]);
      }).EqualMsgAsync((t) => t[3], () => ofArray([[1, "111", 1], [2, "22", 2], [3, "33", 3]]), "insert into underlying").For((a) => {
        const u_1=a[0];
        u_1.RemoveByKey(2);
        return Runner.Yield([u_1, a[1], a[2], a[3]]);
      }).EqualMsgAsync((t) => t[3], () => ofArray([[1, "111", 1], [3, "33", 3]]), "remove from underlying").For((a) => {
        const l_1=a[1];
        l_1.UpdateBy(() => Some([1, "1111", 1]), 1);
        return Runner.Yield([a[0], l_1, a[2], a[3]]);
      }).EqualMsgAsync((t) => t[2], () => ofArray([[1, "1111"], [3, "33"]]), "update contextual").For((a) => {
        const l_1=a[1];
        l_1.Append([4, "44", 4]);
        return Runner.Yield([a[0], l_1, a[2], a[3]]);
      }).EqualMsgAsync((t) => t[2], () => ofArray([[1, "1111"], [3, "33"], [4, "44"]]), "insert into contextual").For((a) => {
        const l_1=a[1];
        l_1.RemoveByKey(3);
        return Runner.Yield([a[0], l_1, a[2], a[3]]);
      }).EqualMsgAsync((t) => t[2], () => ofArray([[1, "1111"], [4, "44"]]), "remove from contextual"));
    }};
    globalThis.QUnit.module(s_2.name);
    this.ListModelTest=s_2;
    const _5=TestCategory("Serializer");
    const s_3={name:_5.name, run:() => {
      const _11=Test("Typed");
      const s_9={Encode:(EncodeList(Id()))(), Decode:(DecodeList(Id()))()};
      _11.Run(Runner.Yield([s_9, s_9.Decode(JSON.parse(JSON.stringify(s_9.Encode(ofArray([1, 2, 3])))))]).Equal((t) => nth(2, t[1]), () => 3));
    }};
    globalThis.QUnit.module(s_3.name);
    this.SerializerTest=s_3;
    const _6=TestCategory("Elt");
    const s_4={name:_6.name, run:() => {
      const _11=Test("Children template updates after moving out of its wrapper");
      LoadLocalTemplates("local");
      const value=_c_1.Create_1("first");
      const host=globalThis.document.createElement("div");
      globalThis.document.body.appendChild(host);
      const child=Doc.BindView((value_1) => Doc.Element("div", [], [Doc.TextNode(value_1)]), value.View);
      const _12=NamedTemplate("local", Some("movedfragment"), [new Elt("child", child)]);
      LoadLocalTemplates("");
      Doc.Run(host, _12);
      const f_1=() => {
        const f_2=() => Return(host.textContent);
        return Bind_1(Sleep(50), (_13, _14) =>(f_2(_13))(_14));
      };
      _11.Run(Runner.Yield([value, host, child, Delay((_13, _14) =>(f_1(_13))(_14))]).EqualMsgAsync((t) => t[3], () =>"beforefirstafter", "initial fragment contents").For((a) => {
        const value_1=a[0];
        value_1.Set("second");
        return Runner.Yield([value_1, a[1], a[2], a[3]]);
      }).EqualMsgAsync((t) => t[3], () =>"beforesecondafter", "updated fragment contents and order").For((a) => {
        const host_1=a[1];
        host_1.parentNode.removeChild(host_1);
        return Runner.Zero();
      }));
      Test("Class").Run(Runner.Yield(Doc.Element("div", [Attr.Create("class", "overridden1"), Class("overridden2"), Attr.Create("class", "base1 base2"), Class("extra1"), Class("extra2 extra3")], [])).IsFalseMsg((elt) => elt.HasClass("overridden1"), "overridden1").IsFalseMsg((elt) => elt.HasClass("overridden2"), "overridden2").IsTrueMsg((elt) => elt.HasClass("base1"), "base1").IsTrueMsg((elt) => elt.HasClass("base2"), "base2").IsTrueMsg((elt) => elt.HasClass("extra1"), "extra1").IsTrueMsg((elt) => elt.HasClass("extra2"), "extra2").IsTrueMsg((elt) => elt.HasClass("extra3"), "extra3").EqualMsg((elt) => elt.elt.className, () =>"base1 base2 extra1 extra2 extra3", "full initial").For((a) => {
        AddClass(a.elt, "extra4");
        return Runner.Yield(a);
      }).IsTrueMsg((elt) => elt.HasClass("extra4"), "extra4").EqualMsg((elt) => elt.elt.className, () =>"base1 base2 extra1 extra2 extra3 extra4", "after AddClass").For((a) => {
        AddClass(a.elt, "extra4");
        return Runner.Yield(a);
      }).EqualMsg((elt) => elt.elt.className, () =>"base1 base2 extra1 extra2 extra3 extra4", "after double AddClass").For((a) => {
        RemoveClass(a.elt, "extra1");
        return Runner.Yield(a);
      }).IsFalseMsg((elt) => elt.HasClass("extra1"), "remove extra1").EqualMsg((elt) => elt.elt.className, () =>"base1 base2 extra2 extra3 extra4", "after remove extra1").For((a) => {
        RemoveClass(a.elt, "extra4");
        return Runner.Yield(a);
      }).IsFalseMsg((elt) => elt.HasClass("extra4"), "remove extra4").EqualMsg((elt) => elt.elt.className, () =>"base1 base2 extra2 extra3", "after remove extra4").For((a) => {
        RemoveClass(a.elt, "base1");
        return Runner.Yield(a);
      }).IsFalseMsg((elt) => elt.HasClass("base1"), "remove base1").EqualMsg((elt) => elt.elt.className, () =>"base2 extra2 extra3", "after remove base1").For(() => Runner.Yield(Doc.Element("div", [Attr.Create("class", "cls1 cls2 cls2 cls3 cls3")], []))).EqualMsg((elt) => elt.elt.className, () =>"cls1 cls2 cls2 cls3 cls3", "initial with duplicate").For((a) => {
        RemoveClass(a.elt, "cls2");
        return Runner.Yield(a);
      }).EqualMsg((elt) => elt.elt.className, () =>"cls1 cls3 cls3", "remove duplicate").For((a) => {
        RemoveClass(a.elt, "cls3");
        return Runner.Yield(a);
      }).EqualMsg((elt) => elt.elt.className, () =>"cls1", "remove duplicate in final position"));
    }};
    globalThis.QUnit.module(s_4.name);
    this.EltTest=s_4;
    const _7=TestCategory("Input controls");
    const s_5={name:_7.name, run:() => {
      const _11=Test("Text");
      const rv=_c_1.Create_1("initial");
      const inp=Doc.Input([], rv);
      _11.Run(Runner.Yield([rv, inp, mount(inp)]).EqualMsgAsync((t) => {
        const inp_5=t[1];
        return afterUpdate(() => inp_5.GetValue());
      }, () =>"initial", "initial value rendered").For((a) => {
        const rv_10=a[0];
        rv_10.Set("set from var");
        return Runner.Yield([rv_10, a[1], a[2]]);
      }).EqualMsgAsync((t) => {
        const inp_5=t[1];
        return afterUpdate(() => inp_5.GetValue());
      }, () =>"set from var", "var change updates input").For((a) => {
        const inp_5=a[1];
        inp_5.SetValue("typed");
        trigger(inp_5.elt, "input");
        return Runner.Yield([a[0], inp_5, a[2]]);
      }).EqualMsg((t) => t[0].Get(), () =>"typed", "input event updates var").For((a) => {
        unmount(a[2]);
        return Runner.Zero();
      }));
      const _12=Test("TextArea");
      const rv_1=_c_1.Create_1("initial");
      const inp_1=Doc.InputArea([], rv_1);
      _12.Run(Runner.Yield([rv_1, inp_1, mount(inp_1)]).EqualMsgAsync((t) => {
        const inp_5=t[1];
        return afterUpdate(() => inp_5.GetValue());
      }, () =>"initial", "initial value rendered").For((a) => {
        const inp_5=a[1];
        inp_5.SetValue("line1\nline2");
        trigger(inp_5.elt, "change");
        return Runner.Yield([a[0], inp_5, a[2]]);
      }).EqualMsg((t) => t[0].Get(), () =>"line1\nline2", "change event updates var").For((a) => {
        unmount(a[2]);
        return Runner.Zero();
      }));
      const _13=Test("Int");
      const rv_2=_c_1.Create_1(CheckedInput.Make(42));
      const inp_2=Doc.IntInput([], rv_2);
      _13.Run(Runner.Yield([rv_2, inp_2, mount(inp_2)]).EqualMsgAsync((t) => {
        const inp_5=t[1];
        return afterUpdate(() => inp_5.GetValue());
      }, () =>"42", "initial value rendered").For((a) => {
        const inp_5=a[1];
        inp_5.SetValue("17");
        trigger(inp_5.elt, "input");
        return Runner.Yield([a[0], inp_5, a[2]]);
      }).EqualMsg((t) => t[0].Get(), () => CheckedInput.Valid(17, "17"), "valid number parsed").For((a) => {
        const inp_5=a[1];
        inp_5.SetValue("");
        trigger(inp_5.elt, "input");
        return Runner.Yield([a[0], inp_5, a[2]]);
      }).EqualMsg((t) => t[0].Get(), () => CheckedInput.Blank(""), "blank input").For((a) => {
        const rv_10=a[0];
        rv_10.Set(CheckedInput.Make(5));
        return Runner.Yield([rv_10, a[1], a[2]]);
      }).EqualMsgAsync((t) => {
        const inp_5=t[1];
        return afterUpdate(() => inp_5.GetValue());
      }, () =>"5", "var change updates input").For((a) => {
        unmount(a[2]);
        return Runner.Zero();
      }));
      const _14=Test("Range");
      const rv_3=_c_1.Create_1(10);
      const inp_3=Doc.RangeInput([], rv_3);
      _14.Run(Runner.Yield([rv_3, inp_3, mount(inp_3)]).EqualMsgAsync((t) => {
        const inp_5=t[1];
        return afterUpdate(() => inp_5.GetValue());
      }, () =>"10", "initial value rendered").For((a) => {
        const inp_5=a[1];
        inp_5.SetValue("30");
        trigger(inp_5.elt, "input");
        return Runner.Yield([a[0], inp_5, a[2]]);
      }).EqualMsg((t) => t[0].Get(), () => 30, "input event updates var").For((a) => {
        unmount(a[2]);
        return Runner.Zero();
      }));
      const _15=Test("CheckBox");
      const rv_4=_c_1.Create_1(false);
      const chk=Doc.CheckBox([], rv_4);
      _15.Run(Runner.Yield([rv_4, chk, mount(chk)]).EqualMsgAsync((t) => {
        const chk_1=t[1];
        return afterUpdate(() => isChecked(chk_1.elt));
      }, () => false, "initially unchecked").For((a) => {
        const chk_1=a[1];
        chk_1.elt.click();
        return Runner.Yield([a[0], chk_1, a[2]]);
      }).EqualMsg((t) => t[0].Get(), () => true, "click updates var").For((a) => {
        const rv_10=a[0];
        rv_10.Set(false);
        return Runner.Yield([rv_10, a[1], a[2]]);
      }).EqualMsgAsync((t) => {
        const chk_1=t[1];
        return afterUpdate(() => isChecked(chk_1.elt));
      }, () => false, "var change updates checkbox").For((a) => {
        unmount(a[2]);
        return Runner.Zero();
      }));
      const _16=Test("CheckBoxGroup");
      const rv_5=_c_1.Create_1(ofArray([2]));
      const l=FSharpList.Cons(void 0, void 0);
      let last=l;
      for(const i of Iter_1(range(1, 3))){
        const c=FSharpList.Cons(Doc.CheckBoxGroup([], i, rv_5), void 0);
        last.$1=c;
        last=c;
      }
      last.$1=FSharpList.Empty;
      const boxes=l.$1;
      _16.Run(Runner.Yield([rv_5, boxes, mount(Doc.Concat(map((b) => b, boxes))), () => map((b) => isChecked(b.elt), boxes)]).EqualMsgAsync((t) => afterUpdate(t[3]), () => ofArray([false, true, false]), "initial state").For((a) => {
        const boxes_1=a[1];
        boxes_1.get_Item(0).elt.click();
        return Runner.Yield([a[0], boxes_1, a[2], a[3]]);
      }).EqualMsg((t) => sort(t[0].Get()), () => ofArray([1, 2]), "checking adds to list").For((a) => {
        const boxes_1=a[1];
        boxes_1.get_Item(1).elt.click();
        return Runner.Yield([a[0], boxes_1, a[2], a[3]]);
      }).EqualMsg((t) => t[0].Get(), () => ofArray([1]), "unchecking removes from list").For((a) => {
        const rv_10=a[0];
        rv_10.Set(ofArray([3]));
        return Runner.Yield([rv_10, a[1], a[2], a[3]]);
      }).EqualMsgAsync((t) => afterUpdate(t[3]), () => ofArray([false, false, true]), "var change updates boxes").For((a) => {
        unmount(a[2]);
        return Runner.Zero();
      }));
      const _17=Test("Radio");
      const rv_6=_c_1.Create_1(1);
      const l_1=FSharpList.Cons(void 0, void 0);
      let last_1=l_1;
      for(const i_1 of Iter_1(range(1, 3))){
        const c_1=FSharpList.Cons(Doc.Radio([], i_1, rv_6), void 0);
        last_1.$1=c_1;
        last_1=c_1;
      }
      last_1.$1=FSharpList.Empty;
      const radios=l_1.$1;
      _17.Run(Runner.Yield([rv_6, radios, mount(Doc.Concat(map((r) => r, radios))), () => map((r) => isChecked(r.elt), radios)]).EqualMsgAsync((t) => afterUpdate(t[3]), () => ofArray([true, false, false]), "initial state").For((a) => {
        const radios_1=a[1];
        radios_1.get_Item(1).elt.click();
        return Runner.Yield([a[0], radios_1, a[2], a[3]]);
      }).EqualMsg((t) => t[0].Get(), () => 2, "click updates var").For((a) => {
        const rv_10=a[0];
        rv_10.Set(3);
        return Runner.Yield([rv_10, a[1], a[2], a[3]]);
      }).EqualMsgAsync((t) => afterUpdate(t[3]), () => ofArray([false, false, true]), "var change updates radios").For((a) => {
        unmount(a[2]);
        return Runner.Zero();
      }));
      const _18=Test("Select");
      const rv_7=_c_1.Create_1(B);
      const sel=Doc.Select([], String, ofArray([A, B, C]), rv_7);
      _18.Run(Runner.Yield([rv_7, sel, mount(sel)]).EqualMsgAsync((t) => {
        const sel_1=t[1];
        return afterUpdate(() => selectedIndices(sel_1.elt));
      }, () => ofArray([1]), "initial selection").For((a) => {
        const sel_1=a[1];
        sel_1.elt.selectedIndex=2;
        trigger(sel_1.elt, "change");
        return Runner.Yield([a[0], sel_1, a[2]]);
      }).EqualMsg((t) => t[0].Get(), () => C, "change event updates var").For((a) => {
        const rv_10=a[0];
        rv_10.Set(A);
        return Runner.Yield([rv_10, a[1], a[2]]);
      }).EqualMsgAsync((t) => {
        const sel_1=t[1];
        return afterUpdate(() => selectedIndices(sel_1.elt));
      }, () => ofArray([0]), "var change updates selection").For((a) => {
        unmount(a[2]);
        return Runner.Zero();
      }));
      const _19=Test("SelectMultiple");
      const rv_8=_c_1.Create_1(ofArray([B]));
      const host=mount(Doc.SelectMultiple([], String, ofArray([A, B, C]), rv_8));
      _19.Run(Runner.Yield([rv_8, host, host.querySelector("select")]).EqualMsgAsync((t) => {
        const sel_1=t[2];
        return afterUpdate(() => selectedIndices(sel_1));
      }, () => ofArray([1]), "initial selection").For((a) => {
        const sel_1=a[2];
        (sel_1.querySelectorAll("option"))[0].selected=true;
        trigger(sel_1, "change");
        return Runner.Yield([a[0], a[1], sel_1]);
      }).EqualMsg((t) => t[0].Get(), () => ofArray([A, B]), "change event updates var").For((a) => {
        const rv_10=a[0];
        rv_10.Set(ofArray([C]));
        return Runner.Yield([rv_10, a[1], a[2]]);
      }).EqualMsgAsync((t) => {
        const sel_1=t[2];
        return afterUpdate(() => selectedIndices(sel_1));
      }, () => ofArray([2]), "var change updates selection").For((a) => {
        unmount(a[1]);
        return Runner.Zero();
      }));
      const _20=Test("Text bound to LensAuto");
      const rv_9=_c_1.Create_1(New_4("red", New_5("green", "test")));
      const inp_4=Doc.Input([], _c_1.Lens(rv_9, (v_1) => v_1.y.z, (_21, _22) => New_4(_21.x, New_5(_22, _21.y.t))));
      _20.Run(Runner.Yield([rv_9, inp_4, mount(inp_4)]).EqualMsgAsync((t) => {
        const inp_5=t[1];
        return afterUpdate(() => inp_5.GetValue());
      }, () =>"green", "initial value rendered").For((a) => {
        const inp_5=a[1];
        inp_5.SetValue("blue");
        trigger(inp_5.elt, "input");
        return Runner.Yield([a[0], inp_5, a[2]]);
      }).EqualMsg((t) => t[0].Get(), () => New_4("red", New_5("blue", "test")), "input updates nested field").For((a) => {
        unmount(a[2]);
        return Runner.Zero();
      }));
    }};
    globalThis.QUnit.module(s_5.name);
    this.InputTest=s_5;
    const _8=TestCategory("Reactive attributes");
    const s_6={name:_8.name, run:() => {
      const _11=Test("ClassPred with V");
      const rv=_c_1.Create_1(true);
      const el=Doc.Element("div", [DynamicClassPred("is-green", rv.View)], []);
      _11.Run(Runner.Yield([rv, el, mount(el)]).EqualMsgAsync((t) => {
        const el_5=t[1];
        return afterUpdate(() => el_5.HasClass("is-green"));
      }, () => true, "class set").For((a) => {
        const rv_5=a[0];
        rv_5.Set(false);
        return Runner.Yield([rv_5, a[1], a[2]]);
      }).EqualMsgAsync((t) => {
        const el_5=t[1];
        return afterUpdate(() => el_5.HasClass("is-green"));
      }, () => false, "class removed").For((a) => {
        unmount(a[2]);
        return Runner.Zero();
      }));
      const _12=Test("Style with V");
      const rv_1=_c_1.Create_1("red");
      const el_1=Doc.Element("div", [DynamicStyle("color", rv_1.View)], []);
      _12.Run(Runner.Yield([rv_1, el_1, mount(el_1)]).EqualMsgAsync((t) => {
        const el_5=t[1];
        return afterUpdate(() => el_5.elt.style.color);
      }, () =>"red", "initial style").For((a) => {
        const rv_5=a[0];
        rv_5.Set("green");
        return Runner.Yield([rv_5, a[1], a[2]]);
      }).EqualMsgAsync((t) => {
        const el_5=t[1];
        return afterUpdate(() => el_5.elt.style.color);
      }, () =>"green", "updated style").For((a) => {
        unmount(a[2]);
        return Runner.Zero();
      }));
      const _13=Test("Prop with V");
      const rv_2=_c_1.Create_1(false);
      const el_2=Doc.Element("input", [DynamicProp("disabled", rv_2.View)], []);
      _13.Run(Runner.Yield([rv_2, el_2, mount(el_2)]).EqualMsgAsync((t) => {
        const el_5=t[1];
        return afterUpdate(() => el_5.elt.disabled);
      }, () => false, "initially enabled").For((a) => {
        const rv_5=a[0];
        rv_5.Set(true);
        return Runner.Yield([rv_5, a[1], a[2]]);
      }).EqualMsgAsync((t) => {
        const el_5=t[1];
        return afterUpdate(() => el_5.elt.disabled);
      }, () => true, "disabled after var change").For((a) => {
        unmount(a[2]);
        return Runner.Zero();
      }));
      const _14=Test("Boolean attribute from View");
      const rv_3=_c_1.Create_1(true);
      const el_3=Doc.Element("button", [DynamicBool("disabled", rv_3.View)], [Doc.TextNode("button")]);
      _14.Run(Runner.Yield([rv_3, el_3, mount(el_3)]).EqualMsgAsync((t) => {
        const el_5=t[1];
        return afterUpdate(() => el_5.elt.hasAttribute("disabled"));
      }, () => true, "attribute set").For((a) => {
        const rv_5=a[0];
        rv_5.Set(false);
        return Runner.Yield([rv_5, a[1], a[2]]);
      }).EqualMsgAsync((t) => {
        const el_5=t[1];
        return afterUpdate(() => el_5.elt.hasAttribute("disabled"));
      }, () => false, "attribute removed").For((a) => {
        unmount(a[2]);
        return Runner.Zero();
      }));
      const _15=Test("Dynamic attribute");
      const rv_4=_c_1.Create_1("first");
      const el_4=Doc.Element("div", [Dynamic("title", rv_4.View)], []);
      _15.Run(Runner.Yield([rv_4, el_4, mount(el_4)]).EqualMsgAsync((t) => {
        const el_5=t[1];
        return afterUpdate(() => el_5.elt.getAttribute("title"));
      }, () =>"first", "initial value").For((a) => {
        const rv_5=a[0];
        rv_5.Set("second");
        return Runner.Yield([rv_5, a[1], a[2]]);
      }).EqualMsgAsync((t) => {
        const el_5=t[1];
        return afterUpdate(() => el_5.elt.getAttribute("title"));
      }, () =>"second", "updated value").For((a) => {
        unmount(a[2]);
        return Runner.Zero();
      }));
      const _16=Test("Popover");
      const popup=Doc.Element("div", [Attr.Create("id", "test-popover"), Attr.Create("popover", "manual")], [Doc.TextNode("Popover")]);
      const btn=Doc.Element("button", [Attr.Create("popovertarget", "test-popover"), Attr.Create("popovertargetaction", "toggle")], [Doc.TextNode("Toggle")]);
      _16.Run(Runner.Yield([popup, btn, mount(Doc.Concat([popup, btn]))]).EqualMsg((t) => t[0].elt.getAttribute("popover"), () =>"manual", "popover attribute").EqualMsg((t) => t[1].elt.getAttribute("popovertarget"), () =>"test-popover", "popovertarget attribute").EqualMsg((t) => t[1].elt.getAttribute("popovertargetaction"), () =>"toggle", "popovertargetaction attribute").IsFalseMsg((t) => t[0].elt.matches(":popover-open"), "initially hidden").For((a) => {
        const btn_1=a[1];
        btn_1.elt.click();
        return Runner.Yield([a[0], btn_1, a[2]]);
      }).IsTrueMsg((t) => t[0].elt.matches(":popover-open"), "shown after click").For((a) => {
        const btn_1=a[1];
        btn_1.elt.click();
        return Runner.Yield([a[0], btn_1, a[2]]);
      }).IsFalseMsg((t) => t[0].elt.matches(":popover-open"), "hidden after second click").For((a) => {
        unmount(a[2]);
        return Runner.Zero();
      }));
    }};
    globalThis.QUnit.module(s_6.name);
    this.AttrTest=s_6;
    const _9=TestCategory("Reactive Doc");
    const s_7={name:_9.name, run:() => {
      const _11=Test("textView");
      const rv=_c_1.Create_1("first");
      const el=Doc.Element("div", [], [Doc.TextView(rv.View)]);
      _11.Run(Runner.Yield([rv, el, mount(el)]).EqualMsgAsync((t) => {
        const el_3=t[1];
        return afterUpdate(() => el_3.GetText());
      }, () =>"first", "initial text").For((a) => {
        const rv_3=a[0];
        rv_3.Set("second");
        return Runner.Yield([rv_3, a[1], a[2]]);
      }).EqualMsgAsync((t) => {
        const el_3=t[1];
        return afterUpdate(() => el_3.GetText());
      }, () =>"second", "updated text").For((a) => {
        unmount(a[2]);
        return Runner.Zero();
      }));
      const _12=Test("V notation for a Doc");
      const rv_1=_c_1.Create_1("ab");
      const el_1=Doc.Element("div", [], [Doc.EmbedView(Map((_18) => Doc.Element("ul", [], map_1((c) => Doc.Element("li", [], [Doc.TextNode(c)]), _18)), rv_1.View))]);
      _12.Run(Runner.Yield([rv_1, el_1, mount(el_1), () =>[el_1.elt.querySelectorAll("li").length, el_1.GetText()]]).EqualMsgAsync((t) => afterUpdate(t[3]), () =>[2, "ab"], "initial list").For((a) => {
        const rv_3=a[0];
        rv_3.Set("xyz");
        return Runner.Yield([rv_3, a[1], a[2], a[3]]);
      }).EqualMsgAsync((t) => afterUpdate(t[3]), () =>[3, "xyz"], "updated list").For((a) => {
        unmount(a[2]);
        return Runner.Zero();
      }));
      const _13=Test("BindSeqCached keeps nodes");
      const lm=Create((x) => x, ofArray(["a", "b"]));
      const el_2=Doc.Element("ul", [], [Doc.Convert((x) => Doc.Element("li", [], [Doc.TextNode(x)]), lm.v)]);
      _13.Run(Runner.Yield([lm, el_2, mount(el_2), () =>(el_2.elt.querySelectorAll("li"))[0], [null]]).EqualMsgAsync((t) => {
        const el_3=t[1];
        const firstItem=t[3];
        const first=t[4];
        return afterUpdate(() => {
          first[0]=firstItem();
          return el_3.GetText();
        });
      }, () =>"ab", "initial items").For((a) => {
        const lm_1=a[0];
        lm_1.Append("c");
        return Runner.Yield([lm_1, a[1], a[2], a[3], a[4]]);
      }).EqualMsgAsync((t) => {
        const el_3=t[1];
        return afterUpdate(() => el_3.GetText());
      }, () =>"abc", "after add").EqualMsgAsync((t) => {
        const firstItem=t[3];
        const first=t[4];
        return afterUpdate(() => firstItem()===first[0]);
      }, () => true, "existing node kept").For((a) => {
        const lm_1=a[0];
        lm_1.RemoveByKey("a");
        return Runner.Yield([lm_1, a[1], a[2], a[3], a[4]]);
      }).EqualMsgAsync((t) => {
        const el_3=t[1];
        return afterUpdate(() => el_3.GetText());
      }, () =>"bc", "after remove").For((a) => {
        unmount(a[2]);
        return Runner.Zero();
      }));
      const _14=Test("Button");
      const clicks=[0];
      const btn=Doc.Button("Click me", [], () => {
        clicks[0]=clicks[0]+1;
      });
      _14.Run(Runner.Yield([clicks, btn, mount(btn)]).EqualMsg((t) => t[1].GetText(), () =>"Click me", "caption").For((a) => {
        const btn_1=a[1];
        btn_1.elt.click();
        btn_1.elt.click();
        return Runner.Yield([a[0], btn_1, a[2]]);
      }).EqualMsg((t) => t[0][0], () => 2, "callback called on each click").For((a) => {
        unmount(a[2]);
        return Runner.Zero();
      }));
      const _15=Test("EltUpdater");
      const counter_1=_c_1.Create_1(0);
      const updater=Doc.Element("div", [], []).ToUpdater();
      const host=mount(updater);
      const child=Doc.Element("span", [], [Doc.TextView(Map(String, counter_1.View))]);
      updater.elt.appendChild(child.elt);
      updater.AddUpdated(child);
      counter_1.Set(1);
      _15.Run(Runner.Yield([counter_1, updater, host, child]).EqualMsgAsync((t) => {
        const child_1=t[3];
        return afterUpdate(() => child_1.GetText());
      }, () =>"1", "added child is updated").For((a) => {
        const updater_1=a[1];
        const counter_2=a[0];
        const child_1=a[3];
        updater_1.RemoveUpdated(child_1);
        counter_2.Set(2);
        return Runner.Yield([counter_2, updater_1, a[2], child_1]);
      }).EqualMsgAsync((t) => {
        const child_1=t[3];
        return afterUpdate(() => child_1.GetText());
      }, () =>"1", "removed child is not updated").For((a) => {
        const updater_1=a[1];
        const counter_2=a[0];
        const child_1=a[3];
        updater_1.AddUpdated(child_1);
        counter_2.Set(3);
        return Runner.Yield([counter_2, updater_1, a[2], child_1]);
      }).EqualMsgAsync((t) => {
        const child_1=t[3];
        return afterUpdate(() => child_1.GetText());
      }, () =>"3", "re-added child is updated").For((a) => {
        const updater_1=a[1];
        const counter_2=a[0];
        updater_1.RemoveAllUpdated();
        counter_2.Set(4);
        return Runner.Yield([counter_2, updater_1, a[2], a[3]]);
      }).EqualMsgAsync((t) => {
        const child_1=t[3];
        return afterUpdate(() => child_1.GetText());
      }, () =>"3", "RemoveAllUpdated stops updates").For((a) => {
        unmount(a[2]);
        return Runner.Zero();
      }));
      const _16=Test("Runtime template holes");
      LoadLocalTemplates("local");
      const clicks_1=[0];
      const rv_2=_c_1.Create_1("initial");
      const host_1=globalThis.document.createElement("div");
      globalThis.document.body.appendChild(host_1);
      const _17=NamedTemplate("local", Some("holes"), [new Text("text", "filled text"), new Attribute("attr", Class("filled-attr")), new Event_1("click", () =>() => {
        clicks_1[0]=clicks_1[0]+1;
      }), new VarStr("input", rv_2)]);
      LoadLocalTemplates("");
      Doc.Run(host_1, _17);
      _16.Run(Runner.Yield([clicks_1, rv_2, host_1, (sel) => host_1.querySelector(sel)]).EqualMsgAsync((t) => {
        const q=t[3];
        return afterUpdate(() => q(".text").textContent);
      }, () =>"filled text", "text hole").IsTrueMsg((t) => t[3](".attr").classList.contains("filled-attr"), "attribute hole").For((a) => {
        const q=a[3];
        q("button").click();
        return Runner.Yield([a[0], a[1], a[2], q]);
      }).EqualMsg((t) => t[0][0], () => 1, "event hole").EqualMsg((t) => t[3]("input").value, () =>"initial", "var hole rendered").For((a) => {
        const q=a[3];
        q("input").value="typed";
        trigger(q("input"), "input");
        return Runner.Yield([a[0], a[1], a[2], q]);
      }).EqualMsg((t) => t[1].Get(), () =>"typed", "var hole updates var").For((a) => {
        const rv_3=a[1];
        rv_3.Set("set from var");
        return Runner.Yield([a[0], rv_3, a[2], a[3]]);
      }).EqualMsgAsync((t) => {
        const q=t[3];
        return afterUpdate(() => q("input").value);
      }, () =>"set from var", "var change updates input").For((a) => {
        unmount(a[2]);
        return Runner.Zero();
      }));
    }};
    globalThis.QUnit.module(s_7.name);
    this.DocTest=s_7;
    const _10=TestCategory("Lens and Submitter");
    const s_8={name:_10.name, run:() => {
      const _11=Test("Lens");
      const rv=_c_1.Create_1(New_4("a", New_5("b", "c")));
      _11.Run(Runner.Yield([rv, _c_1.Lens(rv, (r) => r.y.z, (_16, _17) => New_4(_16.x, New_5(_17, _16.y.t)))]).EqualMsg((t) => t[1].Get(), () =>"b", "get through lens").For((a) => {
        const lz=a[1];
        lz.Set("B");
        return Runner.Yield([a[0], lz]);
      }).EqualMsg((t) => t[0].Get(), () => New_4("a", New_5("B", "c")), "set through lens").For((a) => {
        const rv_4=a[0];
        rv_4.Set(New_4("a", New_5("Z", "c")));
        return Runner.Yield([rv_4, a[1]]);
      }).EqualMsgAsync((t) => GetAsync(t[1].View), () =>"Z", "lens view follows var"));
      const _12=Test("LensAuto");
      const rv_1=_c_1.Create_1(New_4("a", New_5("b", "c")));
      _12.Run(Runner.Yield([rv_1, _c_1.Lens(rv_1, (r) => r.y.z, (_16, _17) => New_4(_16.x, New_5(_17, _16.y.t)))]).EqualMsg((t) => t[1].Get(), () =>"b", "get through lens").For((a) => {
        const lz=a[1];
        lz.Set("B");
        return Runner.Yield([a[0], lz]);
      }).EqualMsg((t) => t[0].Get(), () => New_4("a", New_5("B", "c")), "set through lens").For((a) => {
        const rv_4=a[0];
        rv_4.Set(New_4("a", New_5("Z", "c")));
        return Runner.Yield([rv_4, a[1]]);
      }).EqualMsgAsync((t) => GetAsync(t[1].View), () =>"Z", "lens view follows var"));
      const _13=Test("Submitter.Create");
      const rv_2=_c_1.Create_1(1);
      _13.Run(Runner.Yield([rv_2, new Submitter(rv_2.View, 0)]).EqualMsgAsync((t) => GetAsync(t[1].view), () => 0, "initial value").For((a) => {
        const rv_4=a[0];
        rv_4.Set(2);
        return Runner.Yield([rv_4, a[1]]);
      }).EqualMsgAsync((t) => GetAsync(t[1].view), () => 0, "input change alone has no effect").For((a) => {
        const s_9=a[1];
        s_9.Trigger();
        return Runner.Yield([a[0], s_9]);
      }).EqualMsgAsync((t) => GetAsync(t[1].view), () => 2, "trigger takes input value").For((a) => {
        const rv_4=a[0];
        rv_4.Set(3);
        return Runner.Yield([rv_4, a[1]]);
      }).EqualMsgAsync((t) => GetAsync(t[1].view), () => 2, "no effect until next trigger"));
      const _14=Test("Submitter.CreateOption");
      const rv_3=_c_1.Create_1("a");
      _14.Run(Runner.Yield([rv_3, CreateOption(rv_3.View)]).EqualMsgAsync((t) => GetAsync(t[1].view), () => null, "initial value").For((a) => {
        const s_9=a[1];
        s_9.Trigger();
        return Runner.Yield([a[0], s_9]);
      }).EqualMsgAsync((t) => GetAsync(t[1].view), () => Some("a"), "after trigger"));
      const _15=Test("MapSeqCached and Sequence of Var views (ui#67)");
      const inputs=_c_1.Create_1([]);
      _15.Run(Runner.Yield([inputs, Map(length, Join(MapCached(Sequence, MapSeqCached((x) => x.View, inputs.View))))]).EqualMsgAsync((t) => GetAsync(t[1]), () => 0, "empty").For((a) => {
        const inputs_1=a[0];
        inputs_1.Set(FSharpList.Cons(_c_1.Create_1(false), ofSeq(inputs_1.Get())));
        return Runner.Yield([inputs_1, a[1]]);
      }).EqualMsgAsync((t) => GetAsync(t[1]), () => 1, "after first add").For((a) => {
        const inputs_1=a[0];
        inputs_1.Set(FSharpList.Cons(_c_1.Create_1(false), ofSeq(inputs_1.Get())));
        return Runner.Yield([inputs_1, a[1]]);
      }).EqualMsgAsync((t) => GetAsync(t[1]), () => 2, "after second add").For((a) => {
        const inputs_1=a[0];
        inputs_1.Set([]);
        return Runner.Yield([inputs_1, a[1]]);
      }).EqualMsgAsync((t) => GetAsync(t[1]), () => 0, "after removing all"));
    }};
    globalThis.QUnit.module(s_8.name);
    this.LensAndSubmitterTest=s_8;
    this.themeStorageKey="websharper-ui-theme";
    this.theme=_c_1.Create_1("light");
    this.themeLabel=Map((t) => t=="dark"?"\u2600\ufe0f Light mode":"\ud83c\udf19 Dark mode", theme().View);
  }
});
let _c_1=Lazy((_i) => class Var_1 extends Object_1 {
  static {
    _c_1=_i(this);
  }
  static Create_1(v){
    return new ConcreteVar(false, {s:Ready(v, [])}, v);
  }
  static Lens(var_1, get_1, update){
    const id=Id_1();
    const view=Map(get_1, var_1.View);
    return new class extends Var {
      constructor(){
        super();
      }
      Get(){
        return get_1(var_1.Get());
      }
      Set(v){
        return var_1.Update((t) => update(t, v));
      }
      SetFinal(v){
        return this.Set(v);
      }
      Update(f){
        return var_1.Update((t) => update(t, f(get_1(t))));
      }
      UpdateMaybe(f){
        return var_1.UpdateMaybe((t) => {
          const x=f(get_1(t));
          return x==null?null:Some(update(t, x.$0));
        });
      }
      get View(){
        return view;
      }
      get Id(){
        return id;
      }
    }();
  }
  static CreateWaiting(){
    return new ConcreteVar(false, {s:Waiting([], [])}, null);
  }
  static Set(var_1, value){
    var_1.Set(value);
  }
  static Update(var_1, fn){
    _c_1.Set(var_1, fn(var_1.Get()));
  }
  static SetFinal(var_1, value){
    var_1.SetFinal(value);
  }
  static Create(){
    return new ConcreteVar(false, {s:Ready(void 0, [])}, void 0);
  }
  static { }
});
function Map(fn, a){
  return CreateLazy(() => Map_2(fn, a()));
}
function Map2(fn, a, a_1){
  return CreateLazy(() => Map2_1(fn, a(), a_1()));
}
function MapCached(fn, v){
  return MapCachedBy(Equals, fn, v);
}
function Sequence(views){
  return CreateLazy(() => Sequence_1(map_1((a) => a(), views)));
}
function Join(a){
  return CreateLazy(() => Join_1(a()));
}
function GetAsync(v){
  return FromContinuations((ok) => Get(ok, v));
}
function Const(x){
  const o={s:Forever(x)};
  return() => o;
}
function ConstAsync(a){
  const o=CreateForeverAsync(a);
  return() => o;
}
function MapCachedBy(eq, fn, a){
  const vref=[null];
  return CreateLazy(() => MapCachedBy_1(eq, vref, fn, a()));
}
function MapAsync(fn, a){
  return CreateLazy(() => MapAsync_1(fn, a()));
}
function MapAsync2(fn, v1, v2){
  return MapAsync((x) => x, Map2(fn, v1, v2));
}
function Apply(fn, view){
  return Map2((_1, _2) => _1(_2), fn, view);
}
function BindInner(fn, view){
  return JoinInner(Map(fn, view));
}
function Bind(fn, view){
  return Join(Map(fn, view));
}
function UpdateWhile(def, v1, v2){
  let value=def;
  return BindInner((pred) => pred?Map((v) => {
    value=v;
    return v;
  }, v2):Const(value), v1);
}
function SnapshotOn(def, a, a_1){
  const sInit={s:Ready(def, [])};
  return CreateLazy(() => sInit.s==null?SnapshotOn_1(a(), a_1()):(WhenObsolete(a(), sInit),sInit));
}
function Get(f, a){
  let ok=false;
  function obs(){
    WhenRun(a(), (v) => {
      if(!ok){
        ok=true;
        f(v);
      }
    }, () => {
      if(!ok)obs();
    });
  }
  obs();
}
function WithInit(x, a){
  return CreateLazy(() => WithInit_1(x, a()));
}
function WithInitOption(a){
  return CreateLazy(() => WithInitOption_1(a()));
}
function MapSeqCached(conv, view){
  return MapSeqCachedBy((x) => x, conv, view);
}
function CreateLazy(observe){
  const lv={c:null, o:observe};
  return() => {
    let c=lv.c;
    if(c===null){
      c=lv.o();
      lv.c=c;
      const _1=c.s;
      if(_1!=null&&_1.$==0)lv.o=null;
      else WhenObsoleteRun(c, () => {
        lv.c=null;
      });
      return c;
    }
    else return c;
  };
}
function MapSeqCachedBy(key, conv, view){
  let state=new Dictionary("New_5");
  return Map((xs) => {
    const prevState=state;
    const newState=new Dictionary("New_5");
    const result=mapInPlace((x) => {
      const k=key(x);
      const res=prevState.ContainsKey(k)?prevState.Item(k):conv(x);
      newState.set_Item(k, res);
      return res;
    }, ofSeq_1(xs));
    state=newState;
    return result;
  }, view);
}
function JoinInner(a){
  return CreateLazy(() => JoinInner_1(a()));
}
function Sink(act, a){
  function loop(){
    WhenRun(a(), act, () => {
      scheduler().Fork(loop);
    });
  }
  scheduler().Fork(loop);
}
function Map2Unit(a, a_1){
  return CreateLazy(() => Map2Unit_1(a(), a_1()));
}
function Map3(fn, a, a_1, a_2){
  return CreateLazy(() => Map3_1(fn, a(), a_1(), a_2()));
}
function Create(key, init_4){
  return CreateWithStorage(key, InMemory(ofSeq_1(init_4)));
}
function CreateWithStorage(key, storage){
  return new ListModel("New", key, storage);
}
function Wrap(underlying, extract, createItem, updateItem){
  let state=new Dictionary("New_5");
  const init_4=map_2((u) => {
    const t=createItem(u);
    state.set_Item(underlying.key(u), t);
    return t;
  }, underlying.u0076ar.Get());
  return new ListModel("New_3", (a) => underlying.key(extract(a)), _c_1.Lens(underlying.u0076ar, (us) => {
    const newState=new Dictionary("New_5");
    const ts=map_2((u) => {
      const k=underlying.key(u);
      const t=state.ContainsKey(k)?updateItem(state.Item(k), u):createItem(u);
      newState.set_Item(k, t);
      return t;
    }, us);
    state=newState;
    return ts;
  }, (_1, _2) => {
    const newState=new Dictionary("New_5");
    const us=map_2((t) => {
      const u=extract(t);
      newState.set_Item(underlying.key(u), t);
      return u;
    }, _2);
    state=newState;
    return us;
  }), InMemory(init_4));
}
function ofArray(arr){
  let r=FSharpList.Empty;
  for(let i=length_1(arr)-1, _1=0;i>=_1;i--)r=FSharpList.Cons(get(arr, i), r);
  return r;
}
function ofSeq(s){
  if(s instanceof FSharpList)return s;
  else if(s instanceof Array)return ofArray(s);
  else {
    const e=Get_1(s);
    try {
      let r;
      let go=e.MoveNext();
      if(!go)return FSharpList.Empty;
      else {
        const res=Create_3(FSharpList, {$:1});
        r=res;
        while(go)
          {
            r.$0=e.Current;
            if(e.MoveNext()){
              const t=Create_3(FSharpList, {$:1});
              r=(r.$1=t,t);
            }
            else go=false;
          }
        r.$1=FSharpList.Empty;
        return res;
      }
    }
    finally {
      if(typeof e=="object"&&isIDisposable(e))e.Dispose();
    }
  }
}
function map(f, x){
  let r, l, go;
  if(x.$==0)return x;
  else {
    const res=Create_3(FSharpList, {$:1});
    r=res;
    l=x;
    go=true;
    while(go)
      {
        r.$0=f(l.$0);
        l=l.$1;
        if(l.$==0)go=false;
        else {
          const t=Create_3(FSharpList, {$:1});
          r=(r.$1=t,t);
        }
      }
    r.$1=FSharpList.Empty;
    return res;
  }
}
function sort(l){
  const a=ofList(l);
  sortInPlace(a);
  return ofArray(a);
}
function iter(f, l){
  let r=l;
  while(r.$==1)
    {
      f(head(r));
      r=tail(r);
    }
}
function init(s, f){
  return ofArray(init_3(s, f));
}
function mapi(f, x){
  let r, l, i, go;
  if(x.$==0)return x;
  else {
    const res=Create_3(FSharpList, {$:1});
    r=res;
    l=x;
    i=0;
    go=true;
    while(go)
      {
        r.$0=f(i, l.$0);
        l=l.$1;
        if(l.$==0)go=false;
        else {
          const t=Create_3(FSharpList, {$:1});
          r=(r.$1=t,t);
          i=i+1;
        }
      }
    r.$1=FSharpList.Empty;
    return res;
  }
}
function exists(p, x){
  let e=false;
  let l=x;
  while(!e&&l.$==1)
    {
      e=p(l.$0);
      l=l.$1;
    }
  return e;
}
function filter(p, x){
  let res, r, l, go;
  if(x.$==0)return x;
  else {
    res=FSharpList.Empty;
    r=res;
    l=x;
    go=true;
    while(go)
      {
        if(p(l.$0)){
          if(res.$==0){
            res=Create_3(FSharpList, {$:1});
            r=res;
          }
          else {
            const t=Create_3(FSharpList, {$:1});
            r=(r.$1=t,t);
          }
          const v=l.$0;
          r.$=1;
          r.$0=v;
        }
        l=l.$1;
        if(l.$==0)go=false;
      }
    if(!(res.$==0))r.$1=FSharpList.Empty;
    return res;
  }
}
function head(l){
  return l.$==1?l.$0:listEmpty();
}
function tail(l){
  return l.$==1?l.$1:listEmpty();
}
function append(x, y){
  let r, l, go;
  if(x.$==0)return y;
  else if(y.$==0)return x;
  else {
    const res=Create_3(FSharpList, {$:1});
    r=res;
    l=x;
    go=true;
    while(go)
      {
        r.$0=l.$0;
        l=l.$1;
        if(l.$==0)go=false;
        else {
          const t=Create_3(FSharpList, {$:1});
          r=(r.$1=t,t);
        }
      }
    r.$1=y;
    return res;
  }
}
function contains(el, x){
  let c=false;
  let l=x;
  while(!c&&l.$==1)
    {
      c=Equals(el, l.$0);
      l=l.$1;
    }
  return c;
}
function listEmpty(){
  return FailWith("The input list was empty.");
}
function New(Name, Done){
  return{Name:Name, Done:Done};
}
class ListModel extends Object_1 {
  key;
  u0076ar;
  storage;
  v;
  it;
  ContainsKey(key){
    return exists_2((it) => Equals(this.key(it), key), this.u0076ar.Get());
  }
  get LengthAsView(){
    return Map((arr) => length_1(arr), this.u0076ar.View);
  }
  RemoveByKey(key){
    this.u0076ar.Set(this.storage.SRemoveIf((i) => Equals(this.key(i), key), this.u0076ar.Get()));
    this.ObsoleteKey(key);
  }
  Wrap(extract, wrap, update){
    return Wrap(this, extract, wrap, update);
  }
  UpdateBy(fn, key){
    const v=this.u0076ar.Get();
    const m=tryFindIndex_1((it) => Equals(this.key(it), key), v);
    if(m!=null&&m.$==1){
      const index=m.$0;
      const m_1=fn(get(v, index));
      if(m_1!=null&&m_1.$==1){
        this.u0076ar.Set(this.storage.SSetAt(index, m_1.$0, v));
        this.ObsoleteKey(key);
      }
    }
  }
  Append(item){
    const v=this.u0076ar.Get();
    const t=this.key(item);
    const m=tryFindIndex_1((it) => Equals(this.key(it), t), v);
    if(m!=null&&m.$==1)this.u0076ar.Set(this.storage.SSetAt(m.$0, item, v));
    else this.u0076ar.Set(this.storage.SAppend(item, v));
    this.ObsoleteKey(t);
  }
  ObsoleteKey(key){
    let o=null;
    const m=[this.it.TryGetValue(key, {get:() => o, set:(v) => {
      o=v;
    }}), o];
    if(m[0]){
      Obsolete(m[1]);
      this.it.RemoveKey(key);
    }
  }
  GetEnumerator(){
    return Get_1(this.u0076ar.Get());
  }
  GetEnumerator0(){
    return Get0(this.u0076ar.Get());
  }
  constructor(i, _1, _2, _3){
    let key, storage;
    if(i=="New"){
      key=_1;
      storage=_2;
      i="New_3";
      _1=key;
      _2=_c_1.Create_1(ofSeq_1(distinctBy(key, storage.SInit())));
      _3=storage;
    }
    if(i=="New_3"){
      const key_1=_1;
      const var_1=_2;
      const storage_1=_3;
      super();
      this.key=key_1;
      this.u0076ar=var_1;
      this.storage=storage_1;
      this.v=Map((x) => x.slice(), this.u0076ar.View);
      this.it=new Dictionary("New_5");
    }
  }
}
function filter_1(f, s){
  return{GetEnumerator:() => {
    const o=Get_1(s);
    return new T(void 0, null, (e) => {
      let c;
      let loop=o.MoveNext();
      let res=false;
      while(loop)
        {
          c=o.Current;
          f(c)?(e.c=c,res=true,loop=false):!o.MoveNext()?loop=false:void 0;
        }
      return res;
    }, () => {
      o.Dispose();
    });
  }};
}
function length(s){
  let i;
  if(s instanceof Array||Equals(typeof s, "string"))return s.length;
  else {
    i=0;
    const e=Get_1(s);
    try {
      while(e.MoveNext())
        i=i+1;
      return i;
    }
    finally {
      if(typeof e=="object"&&isIDisposable(e))e.Dispose();
    }
  }
}
function unfold(f, s){
  return{GetEnumerator:() => new T(s, null, (e) => {
    const m=f(e.s);
    return m==null?false:(e.c=m.$0[0],e.s=m.$0[1],true);
  }, void 0)};
}
function map_1(f, s){
  return{GetEnumerator:() => {
    const en=Get_1(s);
    return new T(void 0, null, (e) => en.MoveNext()&&(e.c=f(en.Current),true), () => {
      en.Dispose();
    });
  }};
}
function head_1(s){
  const e=Get_1(s);
  try {
    return e.MoveNext()?e.Current:insufficient();
  }
  finally {
    if(typeof e=="object"&&isIDisposable(e))e.Dispose();
  }
}
function nth(index, s){
  if(index<0)FailWith("negative index requested");
  let pos=-1;
  const e=Get_1(s);
  try {
    while(pos<index)
      {
        !e.MoveNext()?insufficient():void 0;
        pos=pos+1;
      }
    return e.Current;
  }
  finally {
    if(typeof e=="object"&&isIDisposable(e))e.Dispose();
  }
}
function init_1(n, f){
  return take(n, initInfinite(f));
}
function append_1(s1, s2){
  return{GetEnumerator:() => {
    const e1=Get_1(s1);
    let first=true;
    return new T(e1, null, (x) => {
      if(x.s.MoveNext()){
        x.c=x.s.Current;
        return true;
      }
      else {
        const x_1=x.s;
        if(!Equals(x_1, null))x_1.Dispose();
        x.s=null;
        return first&&(first=false,x.s=Get_1(s2),x.s.MoveNext()?(x.c=x.s.Current,true):(x.s.Dispose(),x.s=null,false));
      }
    }, (x) => {
      const x_1=x.s;
      if(!Equals(x_1, null))x_1.Dispose();
    });
  }};
}
function take(n, s){
  n<0?nonNegative():void 0;
  return{GetEnumerator:() => {
    let e=Get_1(s);
    return new T(0, null, (o) => {
      o.s=o.s+1;
      if(o.s>n)return false;
      else {
        const en=e;
        return Equals(en, null)?insufficient():en.MoveNext()?(o.c=en.Current,o.s===n?(en.Dispose(),e=null):void 0,true):(en.Dispose(),e=null,insufficient());
      }
    }, () => {
      const x=e;
      if(!Equals(x, null))x.Dispose();
    });
  }};
}
function initInfinite(f){
  return{GetEnumerator:() => new T(0, null, (e) => {
    e.c=f(e.s);
    e.s=e.s+1;
    return true;
  }, void 0)};
}
function distinctBy(f, s){
  return{GetEnumerator:() => {
    const o=Get_1(s);
    const seen=new HashSet("New_3");
    return new T(void 0, null, (e) => {
      let cur, has;
      if(o.MoveNext()){
        cur=o.Current;
        has=seen.SAdd(f(cur));
        while(!has&&o.MoveNext())
          {
            cur=o.Current;
            has=seen.SAdd(f(cur));
          }
        return has&&(e.c=cur,true);
      }
      else return false;
    }, () => {
      o.Dispose();
    });
  }};
}
function fold(f, x, s){
  let r=x;
  const e=Get_1(s);
  try {
    while(e.MoveNext())
      r=f(r, e.Current);
    return r;
  }
  finally {
    if(typeof e=="object"&&isIDisposable(e))e.Dispose();
  }
}
function findIndex(p, s){
  const m=tryFindIndex(p, s);
  return m==null?FailWith("KeyNotFoundException"):m.$0;
}
function tryFindIndex(ok, s){
  const e=Get_1(s);
  try {
    let loop=true;
    let i=0;
    while(loop&&e.MoveNext())
      if(ok(e.Current))loop=false;
      else i=i+1;
    return loop?null:Some(i);
  }
  finally {
    if(typeof e=="object"&&isIDisposable(e))e.Dispose();
  }
}
function iter_1(p, s){
  const e=Get_1(s);
  try {
    while(e.MoveNext())
      p(e.Current);
  }
  finally {
    if(typeof e=="object"&&isIDisposable(e))e.Dispose();
  }
}
function max(s){
  const e=Get_1(s);
  try {
    if(!e.MoveNext())seqEmpty();
    let m=e.Current;
    while(e.MoveNext())
      {
        const x=e.Current;
        if(Compare(x, m)===1)m=x;
      }
    return m;
  }
  finally {
    if(typeof e=="object"&&isIDisposable(e))e.Dispose();
  }
}
function forall(p, s){
  return!exists_1((x) =>!p(x), s);
}
function seqEmpty(){
  return FailWith("The input sequence was empty.");
}
function exists_1(p, s){
  const e=Get_1(s);
  try {
    let r=false;
    while(!r&&e.MoveNext())
      r=p(e.Current);
    return r;
  }
  finally {
    if(typeof e=="object"&&isIDisposable(e))e.Dispose();
  }
}
function Simple(inter, easing, dur, x, y){
  return{Compute:(t) => inter.Interpolate(easing.TransformTime(t/dur), x, y), Duration:dur};
}
function Const_1(v){
  return Const_2(v);
}
function get_UseAnimations(){
  return UseAnimations();
}
function Play(anim){
  const f=() => {
    const x=Run(() => { }, Actions(anim));
    const f_1=() => {
      Finalize(anim);
      return Return(void 0);
    };
    return Bind_1(x, (_1, _2) =>(f_1(_1))(_2));
  };
  return Delay((_1, _2) =>(f(_1))(_2));
}
function Append(a, a_1){
  return Anim(Append_1(a.$0, a_1.$0));
}
function Run(k, anim){
  const dur=anim.Duration;
  if(dur===0)return Zero();
  else {
    const c=(ok) => {
      function loop(start){
        return(now) => {
          const t=now-start;
          anim.Compute(t);
          k();
          return t<=dur?void requestAnimationFrame((t_1) => {
            (loop(start))(t_1);
          }):ok();
        };
      }
      requestAnimationFrame((t) => {
        (loop(t))(t);
      });
    };
    return FromContinuations((_1, _2, _3) => c.apply(null, [_1, _2, _3]));
  }
}
function Anim(Item){
  return{$:0, $0:Item};
}
function Concat(xs){
  return Anim(Concat_1(map_1(List, xs)));
}
function get_Empty(){
  return Anim(Empty_1());
}
function Map_1(f, anim){
  const f_1=anim.Compute;
  return Def(anim.Duration, (x) => f(f_1(x)));
}
function Pack(anim){
  return Anim(Single({$:1, $0:anim}));
}
function WhenDone(f, main){
  return Append(Anim(Single({$:0, $0:f})), main);
}
class Easing extends Object_1 {
  transformTime;
  static get CubicInOut(){
    return CubicInOut();
  }
  static Custom(f){
    return new Easing(f);
  }
  TransformTime(t){
    return this.transformTime(t);
  }
  constructor(transformTime){
    super();
    this.transformTime=transformTime;
  }
}
function get_Double(){
  return DoubleInterpolation.DoubleInterpolation;
}
function Create_1(ch){
  return new Trans("New_1", ch);
}
function Enter(f, tr){
  return tr.Copy(null, Some(f), null, Some(tr.TFlags|2));
}
function Exit(f, tr){
  return tr.Copy(null, null, Some(f), Some(tr.TFlags|4));
}
function CanAnimateEnter(tr){
  const c=tr.TFlags;
  const flag=2;
  return(c&flag)===flag;
}
function CanAnimateExit(tr){
  const c=tr.TFlags;
  const flag=4;
  return(c&flag)===flag;
}
function AnimateExit(tr, x){
  return tr.TExit(x);
}
function AnimateChange(tr, x, y){
  return tr.TChange(x, y);
}
function AnimateEnter(tr, x){
  return tr.TEnter(x);
}
function New_1(Profile, Accent, Volume, Subscribe){
  return{
    Profile:Profile, 
    Accent:Accent, 
    Volume:Volume, 
    Subscribe:Subscribe
  };
}
function New_2(DisplayName, Date_1){
  return{DisplayName:DisplayName, Date:Date_1};
}
const Pro={$:1};
const Free={$:0};
const Team={$:2};
class ReactiveExtensions extends Object_1 { }
function toSafe(s){
  return s==null?"":s;
}
function Some(Value_1){
  return{$:1, $0:Value_1};
}
class TemplateHole extends Object_1 {
  ForTextView(){
    console.warn("Content hole filled with attribute data", this.Name);
    return null;
  }
  AddAttribute(a, a_1){
    console.warn("Var hole filled with non-Var data", this.Name);
  }
  get AsChoiceView(){
    console.warn("Attribute value hole filled with non-text data", this.Name);
    return Choice1Of2("");
  }
}
class Elt extends TemplateHole {
  name;
  fillWith;
  get Value(){
    return this.fillWith;
  }
  get Name(){
    return this.name;
  }
  constructor(name, fillWith){
    super();
    this.name=name;
    this.fillWith=fillWith;
  }
}
class Text extends TemplateHole {
  name;
  fillWith;
  get Value(){
    return this.fillWith;
  }
  get Name(){
    return this.name;
  }
  get AsChoiceView(){
    return Choice1Of2(this.fillWith);
  }
  constructor(name, fillWith){
    super();
    this.name=name;
    this.fillWith=fillWith;
  }
}
class RunnerControlBody extends Object_1 {
  run;
  ReplaceInDom(e){
    if(Equals(globalThis.document.querySelector("#qunit"), null)){
      const fixture=globalThis.document.createElement("div");
      fixture.setAttribute("id", "qunit-fixture");
      const qunit=globalThis.document.createElement("div");
      qunit.setAttribute("id", "qunit");
      const parent=e.parentNode;
      parent.replaceChild(fixture, e);
      parent.insertBefore(qunit, fixture);
    }
    this.run();
  }
  constructor(run){
    super();
    this.run=run;
  }
}
function get2D(arr, n1, n2){
  checkBounds2D(arr, n1, n2);
  return arr[n1][n2];
}
function get(arr, n){
  checkBounds(arr, n);
  return arr[n];
}
function length_1(arr){
  return arr.dims===2?arr.length*arr.length:arr.length;
}
function set2D(arr, n1, n2, x){
  checkBounds2D(arr, n1, n2);
  arr[n1][n2]=x;
}
function checkBounds2D(arr, n1, n2){
  if(n1<0||n2<0||n1>=arr.length||n2>=(arr.length?arr[0].length:0))throw new IndexOutOfRangeException("New");
}
function checkBounds(arr, n){
  if(n<0||n>=arr.length)FailWith("Index was outside the bounds of the array.");
}
function zeroCreate2D(n, m){
  const arr=init_3(n, () => create(m, null));
  arr.dims=2;
  return arr;
}
function set(arr, n, x){
  checkBounds(arr, n);
  arr[n]=x;
}
class Dictionary extends Object_1 {
  equals;
  hash;
  count;
  data;
  set_Item(k, v){
    this.set(k, v);
  }
  ContainsKey(k){
    const d=this.data[this.hash(k)];
    return d==null?false:exists_2((a) => this.equals.apply(null, [(KeyValue(a))[0], k]), d);
  }
  TryGetValue(k, res){
    const d=this.data[this.hash(k)];
    if(d==null)return false;
    else {
      const v=tryPick((a) => {
        const a_1=KeyValue(a);
        return this.equals.apply(null, [a_1[0], k])?Some(a_1[1]):null;
      }, d);
      return v!=null&&v.$==1&&(res.set(v.$0),true);
    }
  }
  RemoveKey(k){
    return this.remove(k);
  }
  get Keys(){
    return new KeyCollection(this);
  }
  set(k, v){
    const h=this.hash(k);
    const d=this.data[h];
    if(d==null){
      this.count=this.count+1;
      this.data[h]=new Array({K:k, V:v});
    }
    else {
      const m=tryFindIndex_1((a) => this.equals.apply(null, [(KeyValue(a))[0], k]), d);
      if(m==null){
        this.count=this.count+1;
        d.push({K:k, V:v});
      }
      else d[m.$0]={K:k, V:v};
    }
  }
  Item(k){
    return this.get(k);
  }
  remove(k){
    const h=this.hash(k);
    const d=this.data[h];
    if(d==null)return false;
    else {
      const r=filter_2((a) =>!this.equals.apply(null, [(KeyValue(a))[0], k]), d);
      return length_1(r)<d.length&&(this.count=this.count-1,this.data[h]=r,true);
    }
  }
  get(k){
    const d=this.data[this.hash(k)];
    return d==null?notPresent():pick((a) => {
      const a_1=KeyValue(a);
      return this.equals.apply(null, [a_1[0], k])?Some(a_1[1]):null;
    }, d);
  }
  get Values(){
    return new ValueCollection(this);
  }
  GetEnumerator(){
    return Get0(concat_1(GetFieldValues(this.data)));
  }
  constructor(i, _1, _2, _3){
    if(i=="New_5"){
      i="New_6";
      _1=[];
      _2=Equals;
      _3=Hash;
    }
    if(i=="New_6"){
      const init_4=_1;
      const equals=_2;
      const hash=_3;
      super();
      this.equals=equals;
      this.hash=hash;
      this.count=0;
      this.data=[];
      const e=Get_1(init_4);
      try {
        while(e.MoveNext())
          {
            const x=e.Current;
            this.set(x.K, x.V);
          }
      }
      finally {
        if(typeof e=="object"&&isIDisposable(e))e.Dispose();
      }
    }
  }
}
function Dynamic_1(view, set_1){
  return Attr.A1(new DynamicAttrNode(view, set_1));
}
function Animated(tr, view, set_1){
  const node=new AnimatedAttrNode(tr, view, set_1);
  let flags=4;
  if(CanAnimateEnter(tr))flags=flags|1;
  if(CanAnimateExit(tr))flags=flags|2;
  const n=Attr.A1(node);
  SetFlags(n, flags);
  return n;
}
function SetFlags(a, f){
  a.flags=f;
}
function Updates(dyn){
  return MapTreeReduce((x) => x.NChanged, Const(), Map2Unit, dyn.DynNodes);
}
function AppendTree(a, b){
  if(a===null)return b;
  else if(b===null)return a;
  else {
    const x=Attr.A2(a, b);
    SetFlags(x, Flags(a)|Flags(b));
    return x;
  }
}
function EmptyAttr(){
  return _c_6.EmptyAttr;
}
function Insert(elem, tree){
  const nodes=[];
  const oar=[];
  function loop(node){
    while(true)
      {
        if(!(node===null)){
          if(node!=null&&node.$==1)return nodes.push(node.$0);
          else if(node!=null&&node.$==2){
            const b=node.$1;
            const a=node.$0;
            loop(a);
            node=b;
          }
          else return node!=null&&node.$==3?node.$0(elem):node!=null&&node.$==4?oar.push(node.$0):void 0;
        }
        else return;
      }
  }
  loop(tree);
  const arr=nodes.slice(0);
  return New_7(elem, Flags(tree), arr, oar.length===0?null:Some((el) => {
    iter_1((f) => {
      f(el);
    }, oar);
  }));
}
function Empty(e){
  return New_7(e, 0, [], null);
}
function HasExitAnim(attr_1){
  const flag=2;
  return(attr_1.DynFlags&flag)===flag;
}
function GetExitAnim(dyn){
  return GetAnim(dyn, (_1, _2) => _1.NGetExitAnim(_2));
}
function HasEnterAnim(attr_1){
  const flag=1;
  return(attr_1.DynFlags&flag)===flag;
}
function GetEnterAnim(dyn){
  return GetAnim(dyn, (_1, _2) => _1.NGetEnterAnim(_2));
}
function HasChangeAnim(attr_1){
  const flag=4;
  return(attr_1.DynFlags&flag)===flag;
}
function GetChangeAnim(dyn){
  return GetAnim(dyn, (_1, _2) => _1.NGetChangeAnim(_2));
}
function Flags(a){
  return a!==null&&a.hasOwnProperty("flags")?a.flags:0;
}
function GetAnim(dyn, f){
  return Concat(map_2((n) => f(n, dyn.DynElem), dyn.DynNodes));
}
function Sync(elem, dyn){
  iter_2((d) => {
    d.NSync(elem);
  }, dyn.DynNodes);
}
function AddClass(element, cl){
  const c=getClass(element);
  if(c=="")setClass(element, cl);
  else!clsRE(cl).test(c)?setClass(element, c+" "+cl):void 0;
}
function RemoveClass(element, cl){
  const this_1=clsRE(cl);
  setClass(element, getClass(element).replace(this_1, (_1, _2, _3) => _2==""||_3==""?"":" "));
}
function getClass(element){
  return element instanceof SVGElement?element.getAttribute("class"):element.className;
}
function setClass(element, value){
  if(element instanceof SVGElement)element.setAttribute("class", value);
  else element.className=value;
}
function clsRE(cls){
  return new RegExp("(\\s+|^)"+cls+"(?:\\s+"+cls+")*(\\s+|$)", "g");
}
function ParseHTMLIntoFakeRoot(elem){
  const root=globalThis.document.createElement("div");
  if(!rhtml().test(elem)){
    root.appendChild(globalThis.document.createTextNode(elem));
    return root;
  }
  else {
    const m=rtagName().exec(elem);
    const tag=Equals(m, null)?"":get(m, 1).toLowerCase();
    const w=(wrapMap())[tag];
    const p=w?w:defaultWrap();
    root.innerHTML=p[1]+elem.replace(rxhtmlTag(), "<$1></$2>")+p[2];
    function unwrap(elt, a){
      while(true)
        {
          if(a===0)return elt;
          else {
            const i=a;
            elt=elt.lastChild;
            a=i-1;
          }
        }
    }
    return unwrap(root, p[0]);
  }
}
function rhtml(){
  return _c_7.rhtml;
}
function wrapMap(){
  return _c_7.wrapMap;
}
function defaultWrap(){
  return _c_7.defaultWrap;
}
function rxhtmlTag(){
  return _c_7.rxhtmlTag;
}
function rtagName(){
  return _c_7.rtagName;
}
function IterSelector(el, selector, f){
  const l=el.querySelectorAll(selector);
  for(let i=0, _1=l.length-1;i<=_1;i++)f(l[i]);
}
function ChildrenArray(element){
  const a=[];
  for(let i=0, _1=element.childNodes.length-1;i<=_1;i++)a.push(element.childNodes[i]);
  return a;
}
function InsertAt(parent, pos, node){
  let _1;
  if(node.parentNode===parent){
    const m=node.nextSibling;
    _1=pos===(Equals(m, null)?null:m);
  }
  else _1=false;
  if(!_1)parent.insertBefore(node, pos);
}
function IterSelectorDoc(selector, f){
  const l=globalThis.document.querySelectorAll(selector);
  for(let i=0, _1=l.length-1;i<=_1;i++)f(l[i]);
}
function RemoveNode(parent, el){
  if(el.parentNode===parent)parent.removeChild(el);
}
class View {
  get V(){
    return FailWith("View<'T>.V can only be called in an argument to a V-enabled function or if 'T = Doc.");
  }
  $;
  $0;
}
function New_3(i, n, d){
  return{
    i:i, 
    n:n, 
    d:d
  };
}
function TestCategory(name){
  return new TestCategoryBuilder(name);
}
function Test(name){
  return new TestBuilder((c) => {
    globalThis.QUnit.test(name, c);
  });
}
class TestCategoryBuilder extends Object_1 {
  name;
  constructor(name){
    super();
    this.name=name;
  }
}
class SubtestBuilder extends Object_1 { }
class TestBuilder extends SubtestBuilder {
  run;
  Run(e){
    this.run((asserter) => {
      try {
        const m=e.Invoke(asserter);
        if(m.$==1){
          const asy=m.$0;
          const __done__=asserter.async();
          const f=() => {
            const f_1=() => {
              const f_2=(a) => {
                asserter.equal(a, null, "Test threw an unexpected asynchronous exception");
                return Return(void 0);
              };
              const f_3=() => {
                const f_4=() => Return(void 0);
                return Bind_1(WithTimeout(1000, asy), (_1, _2) =>(f_4(_1))(_2));
              };
              return TryWith(Delay((_1, _2) =>(f_3(_1))(_2)), (_1, _2) =>(f_2(_1))(_2));
            };
            return TryFinally(Delay((_1, _2) =>(f_1(_1))(_2)), () => {
              __done__();
            });
          };
          Start(Delay((_1, _2) =>(f(_1))(_2)), null);
        }
      }
      catch(e_1){
        asserter.equal(e_1, null, "Test threw an unexpected synchronous exception");
      }
    });
  }
  constructor(run){
    super();
    this.run=run;
  }
}
class FSharpEvent extends Object_1 {
  event;
  constructor(){
    super();
    this.event=Event_2.New(MarkResizable([]));
  }
}
function init_2(n, m, f){
  const array=zeroCreate2D(n, m);
  for(let i=0, _1=n-1;i<=_1;i++)for(let j=0, _2=m-1;j<=_2;j++)set2D(array, i, j, f(i, j));
  return array;
}
function EncodeList(encEl){
  return() =>(l) => {
    const a=[];
    const e=encEl();
    iter((x) => {
      a.push(e(x));
    }, l);
    return a;
  };
}
function Id(){
  return() =>(x) => x;
}
function DecodeList(decEl){
  return() =>(a) => {
    const e=decEl();
    return init(length_1(a), (i) => e(get(a, i)));
  };
}
class CheckedInput {
  static Make(x){
    let c;
    return CheckedInput.Valid(x, (c=x,String(c)));
  }
  static Valid(value, inputText){
    return Create_3(CheckedInput, {
      $:0, 
      $0:value, 
      $1:inputText
    });
  }
  static Blank(inputText){
    return Create_3(CheckedInput, {$:2, $0:inputText});
  }
  get Input(){
    return this.$==1?this.$0:this.$==2?this.$0:this.$1;
  }
  static Invalid(inputText){
    return Create_3(CheckedInput, {$:1, $0:inputText});
  }
  $;
  $0;
  $1;
}
class FSharpList {
  static Cons(Head, Tail){
    return Create_3(FSharpList, {
      $:1, 
      $0:Head, 
      $1:Tail
    });
  }
  static Empty=Create_3(FSharpList, {$:0});
  get_Item(x){
    return nth(x, this);
  }
  GetEnumerator(){
    return new T(this, null, (e) => {
      const m=e.s;
      return m.$==0?false:(e.c=m.$0,e.s=m.$1,true);
    }, void 0);
  }
  $;
  $0;
  $1;
}
const B={$:1};
const A={$:0};
const C={$:2};
function New_4(x, y){
  return{x:x, y:y};
}
function New_5(z, t){
  return{z:z, t:t};
}
class Elt_1 extends Doc {
  docNode_1;
  updates_1;
  elt;
  rvUpdates;
  HasClass(cls){
    return(new RegExp("(\\s|^)"+cls+"(\\s|$)")).test(this.elt.className);
  }
  GetValue(){
    return this.elt.value;
  }
  SetValue(v){
    this.elt.value=v;
  }
  GetText(){
    return this.elt.textContent;
  }
  ToUpdater(){
    let _1;
    const m=this.docNode_1;
    if(m!=null&&m.$==1){
      const _2=Some(this.elt);
      _1=DeleteEmptyFields({
        Els:[this.elt], 
        Dirty:true, 
        Holes:[], 
        Attrs:[[this.elt, m.$0.Attr]], 
        Render:null?null.$0:void 0, 
        El:_2?_2.$0:void 0
      }, ["Render", "El"]);
    }
    else _1=m!=null&&m.$==6?m.$0:FailWith("Invalid docNode in Elt");
    return new EltUpdater(_1, this.updates_1, this.elt, this.rvUpdates, _c_1.Create_1([]));
  }
  static New(el, attr_1, children){
    const node=CreateElemNode(el, attr_1, children.docNode);
    const rvUpdates=Updates_1.Create(children.updates);
    return new Elt_1(ElemDoc(node), Map2Unit(Updates(node.Attr), rvUpdates.v), el, rvUpdates);
  }
  static TreeNode(tree, updates){
    const rvUpdates=Updates_1.Create(updates);
    const x=map_2((_1) => Updates(_1[1]), tree.Attrs);
    return new Elt_1(TreeDoc(tree), Map2Unit(TreeReduce(Const(), Map2Unit, x), rvUpdates.v), get(tree.Els, 0), rvUpdates);
  }
  constructor(docNode, updates, elt, rvUpdates){
    super(docNode, updates);
    this.docNode_1=docNode;
    this.updates_1=updates;
    this.elt=elt;
    this.rvUpdates=rvUpdates;
  }
}
class EltUpdater extends Elt_1 {
  treeNode;
  holeUpdates;
  origHoles;
  AddUpdated(doc){
    const m=doc.docNode;
    if(m!=null&&m.$==1){
      const e=m.$0;
      this.treeNode.Holes=this.treeNode.Holes.concat([e]);
      const hu=this.holeUpdates.Get();
      hu.push([e.ElKey, doc.updates]);
      this.holeUpdates.Set(hu);
    }
    else FailWith("DocUpdater.AddUpdated expects a single element node");
  }
  RemoveUpdated(doc){
    const m=doc.docNode;
    if(m!=null&&m.$==1){
      const k=m.$0.ElKey;
      this.treeNode.Holes=filter_2((h) => h.ElKey!==k, this.treeNode.Holes);
      this.holeUpdates.Set(filter_2((_1) => _1[0]!==k, this.holeUpdates.Get()));
    }
    else FailWith("DocUpdater.RemoveUpdated expects a single element node");
  }
  RemoveAllUpdated(){
    this.treeNode.Holes=this.origHoles;
    this.holeUpdates.Set([]);
  }
  constructor(treeNode, updates, elt, rvUpdates, holeUpdates){
    const x=holeUpdates.View;
    const m=(t) => t[1];
    const f=(a) => map_2(m, a);
    const d=Const();
    const g=(a) => TreeReduce(d, Map2Unit, a);
    super(TreeDoc(treeNode), Map2Unit(updates, BindInner((x_1) => g(f(x_1)), x)), elt, rvUpdates);
    this.treeNode=treeNode;
    this.holeUpdates=holeUpdates;
    this.origHoles=this.treeNode.Holes;
  }
}
class Attribute extends TemplateHole {
  name;
  fillWith;
  get Value(){
    return this.fillWith;
  }
  get Name(){
    return this.name;
  }
  constructor(name, fillWith){
    super();
    this.name=name;
    this.fillWith=fillWith;
  }
}
class Event_1 extends TemplateHole {
  name;
  fillWith;
  get Value(){
    return this.fillWith;
  }
  get Name(){
    return this.name;
  }
  constructor(name, fillWith){
    super();
    this.name=name;
    this.fillWith=fillWith;
  }
}
class VarStr extends TemplateHole {
  name;
  fillWith;
  get Name(){
    return this.name;
  }
  ForTextView(){
    return Some(this.fillWith.View);
  }
  AddAttribute(addAttr, el){
    (addAttr(el))(Value(this.fillWith));
  }
  get AsChoiceView(){
    return Choice2Of2(Map(String, this.fillWith.View));
  }
  constructor(name, fillWith){
    super();
    this.name=name;
    this.fillWith=fillWith;
  }
}
function CreateOption(input){
  return new Submitter(Map(Some, input), null);
}
class Submitter extends Object_1 {
  input;
  u0076ar;
  view;
  Trigger(){
    this.u0076ar.Set(void 0);
  }
  constructor(input, init_4){
    super();
    this.input=input;
    this.u0076ar=_c_1.Create();
    this.view=SnapshotOn(init_4, this.u0076ar.View, this.input);
  }
}
class ConcreteVar extends Var {
  isConst;
  current;
  snap;
  view;
  id;
  get View(){
    return this.view;
  }
  Set(v){
    if(this.isConst)(((_1) => _1("WebSharper.UI: invalid attempt to change value of a Var after calling SetFinal"))((s) => {
      console.log(s);
    }));
    else {
      Obsolete(this.snap);
      this.current=v;
      this.snap={s:Ready(v, [])};
    }
  }
  Get(){
    return this.current;
  }
  Update(f){
    this.Set(f(this.Get()));
  }
  UpdateMaybe(f){
    const m=f(this.Get());
    if(m!=null&&m.$==1)this.Set(m.$0);
  }
  SetFinal(v){
    if(this.isConst)(((_1) => _1("WebSharper.UI: invalid attempt to change value of a Var after calling SetFinal"))((s) => {
      console.log(s);
    }));
    else {
      Obsolete(this.snap);
      this.isConst=true;
      this.current=v;
      this.snap={s:Forever(v)};
    }
  }
  get Id(){
    return"uinref"+String(this.id);
  }
  constructor(isConst, initSnap, initValue){
    super();
    this.isConst=isConst;
    this.current=initValue;
    this.snap=initSnap;
    this.view=() => this.snap;
    this.id=Int();
  }
}
function Map_2(fn, sn){
  const m=sn.s;
  if(m!=null&&m.$==0)return{s:Forever(fn(m.$0))};
  else {
    const res={s:Waiting([], [])};
    When(sn, (a) => {
      MarkDone(res, sn, fn(a));
    }, res);
    return res;
  }
}
function Map2_1(fn, sn1, sn2){
  const _1=sn1.s;
  const _2=sn2.s;
  if(_1!=null&&_1.$==0)return _2!=null&&_2.$==0?{s:Forever(fn(_1.$0, _2.$0))}:Map2Opt1(fn, _1.$0, sn2);
  else if(_2!=null&&_2.$==0)return Map2Opt2(fn, _2.$0, sn1);
  else {
    const res={s:Waiting([], [])};
    const cont=() => {
      const m=res.s;
      if(!(m!=null&&m.$==0||m!=null&&m.$==2)){
        const _3=ValueAndForever(sn1);
        const _4=ValueAndForever(sn2);
        if(_3!=null&&_3.$==1)if(_4!=null&&_4.$==1)if(_3.$0[1]&&_4.$0[1])MarkForever(res, fn(_3.$0[0], _4.$0[0]));
        else MarkReady(res, fn(_3.$0[0], _4.$0[0]));
      }
    };
    When(sn1, cont, res);
    When(sn2, cont, res);
    return res;
  }
}
function Sequence_1(snaps){
  const snaps_1=ofSeq_1(snaps);
  if(snaps_1.length==0)return{s:Forever([])};
  else {
    const res={s:Waiting([], [])};
    let w=length_1(snaps_1)-1;
    const cont=() => {
      if(w===0){
        const vs=map_2((s) => {
          const m=s.s;
          return m!=null&&m.$==0?m.$0:m!=null&&m.$==2?m.$0:FailWith("value not found by View.Sequence");
        }, snaps_1);
        if(forall_2((s) => {
          const _1=s.s;
          return _1!=null&&_1.$==0;
        }, snaps_1))MarkForever(res, vs);
        else MarkReady(res, vs);
      }
      else w=w-1;
    };
    iter_2((s) => {
      When(s, cont, res);
    }, snaps_1);
    return res;
  }
}
function Join_1(snap){
  const res={s:Waiting([], [])};
  When(snap, (x) => {
    const y=x();
    When(y, (v) => {
      let _1;
      const _2=y.s;
      if(_2!=null&&_2.$==0){
        const _3=snap.s;
        _1=_3!=null&&_3.$==0;
      }
      else _1=false;
      if(_1)MarkForever(res, v);
      else MarkReady(res, v);
    }, res);
  }, res);
  return res;
}
function CreateForeverAsync(a){
  const o={s:Waiting([], [])};
  StartTo(a, (v) => {
    MarkForever(o, v);
  });
  return o;
}
function MapCachedBy_1(eq, prev, fn, sn){
  return Map_2((x) => {
    let _1;
    const m=prev[0];
    if(m!=null&&m.$==1&&(eq(x, m.$0[0])&&(_1=[m.$0[0], m.$0[1]],true)))return _1[1];
    else {
      const y=fn(x);
      prev[0]=Some([x, y]);
      return y;
    }
  }, sn);
}
function MapAsync_1(fn, snap){
  const res={s:Waiting([], [])};
  When(snap, (v) => {
    StartTo(fn(v), (v_1) => {
      MarkDone(res, snap, v_1);
    });
  }, res);
  return res;
}
function SnapshotOn_1(sn1, sn2){
  const res={s:Waiting([], [])};
  const cont=() => {
    const m=res.s;
    if(!(m!=null&&m.$==0||m!=null&&m.$==2)){
      const _1=ValueAndForever(sn1);
      const _2=ValueAndForever(sn2);
      if(_1!=null&&_1.$==1)_2!=null&&_2.$==1?_1.$0[1]||_2.$0[1]?MarkForever(res, _2.$0[0]):MarkReady(res, _2.$0[0]):void 0;
    }
  };
  When(sn1, cont, res);
  WhenReady(sn2, cont);
  return res;
}
function WhenObsolete(snap, obs){
  const m=snap.s;
  if(m==null)Obsolete(obs);
  else m!=null&&m.$==2?EnqueueSafe(m.$1, obs):m!=null&&m.$==3?EnqueueSafe(m.$1, obs):void 0;
}
function WhenRun(snap, avail, obs){
  const m=snap.s;
  if(m==null)obs();
  else m!=null&&m.$==2?(m.$1.push(obs),avail(m.$0)):m!=null&&m.$==3?(m.$0.push(avail),m.$1.push(obs)):avail(m.$0);
}
function WithInit_1(x, sn){
  const m=sn.s;
  if(m==null)return sn;
  else if(m!=null&&m.$==2){
    const res={s:Ready(m.$0, [])};
    WhenObsolete(sn, res);
    return res;
  }
  else if(m!=null&&m.$==3){
    const res_1={s:Ready(x, [])};
    When(sn, () => {
      Obsolete(res_1);
    }, res_1);
    return res_1;
  }
  else return sn;
}
function WithInitOption_1(sn){
  const m=sn.s;
  if(m==null)return{s:null};
  else if(m!=null&&m.$==2){
    const res={s:Ready(Some(m.$0), [])};
    WhenObsolete(sn, res);
    return res;
  }
  else if(m!=null&&m.$==3){
    const res_1={s:Ready(null, [])};
    When(sn, () => {
      Obsolete(res_1);
    }, res_1);
    return res_1;
  }
  else return{s:Forever(Some(m.$0))};
}
function WhenObsoleteRun(snap, obs){
  const m=snap.s;
  if(m==null)obs();
  else m!=null&&m.$==2?m.$1.push(obs):m!=null&&m.$==3?m.$1.push(obs):void 0;
}
function When(snap, avail, obs){
  const m=snap.s;
  if(m==null)Obsolete(obs);
  else m!=null&&m.$==2?(EnqueueSafe(m.$1, obs),avail(m.$0)):m!=null&&m.$==3?(m.$0.push(avail),EnqueueSafe(m.$1, obs)):avail(m.$0);
}
function MarkDone(res, sn, v){
  const _1=sn.s;
  if(_1!=null&&_1.$==0)MarkForever(res, v);
  else MarkReady(res, v);
}
function Map2Opt1(fn, x, sn2){
  return Map_2((y) => fn(x, y), sn2);
}
function Map2Opt2(fn, y, sn1){
  return Map_2((x) => fn(x, y), sn1);
}
function ValueAndForever(snap){
  const m=snap.s;
  return m!=null&&m.$==0?Some([m.$0, true]):m!=null&&m.$==2?Some([m.$0, false]):null;
}
function MarkForever(sn, v){
  const m=sn.s;
  if(m!=null&&m.$==3){
    sn.s=Forever(v);
    const qa=m.$0;
    for(let i=0, _1=length_1(qa)-1;i<=_1;i++)(get(qa, i))(v);
  }
}
function MarkReady(sn, v){
  const m=sn.s;
  if(m!=null&&m.$==3){
    sn.s=Ready(v, m.$1);
    const qa=m.$0;
    for(let i=0, _1=length_1(qa)-1;i<=_1;i++)(get(qa, i))(v);
  }
}
function JoinInner_1(snap){
  const res={s:Waiting([], [])};
  When(snap, (x) => {
    const y=x();
    When(y, (v) => {
      let _1;
      const _2=y.s;
      if(_2!=null&&_2.$==0){
        const _3=snap.s;
        _1=_3!=null&&_3.$==0;
      }
      else _1=false;
      if(_1)MarkForever(res, v);
      else MarkReady(res, v);
    }, res);
    WhenObsolete(snap, y);
  }, res);
  return res;
}
function WhenReady(snap, avail){
  let _1;
  const m=snap.s;
  switch(m!=null&&m.$==2?(_1=m.$0,0):m==null?1:m!=null&&m.$==3?2:(_1=m.$0,0)){
    case 0:
      avail(_1);
      break;
    case 1:
      break;
    case 2:
      m.$0.push(avail);
      break;
  }
}
function EnqueueSafe(q, x){
  q.push(x);
  if(q.length%20===0){
    const qcopy=q.slice(0);
    Clear(q);
    for(let i=0, _1=length_1(qcopy)-1;i<=_1;i++){
      const o=get(qcopy, i);
      if(typeof o=="object")(((sn) => {
        if(sn.s)q.push(sn);
      })(o));
      else(((f) => {
        q.push(f);
      })(o));
    }
  }
}
function Map2Unit_1(sn1, sn2){
  const _1=sn1.s;
  const _2=sn2.s;
  if(_1!=null&&_1.$==0)return _2!=null&&_2.$==0?{s:Forever(void 0)}:sn2;
  else if(_2!=null&&_2.$==0)return sn1;
  else {
    const res={s:Waiting([], [])};
    const cont=() => {
      const m=res.s;
      if(!(m!=null&&m.$==0||m!=null&&m.$==2)){
        const _3=ValueAndForever(sn1);
        const _4=ValueAndForever(sn2);
        if(_3!=null&&_3.$==1)if(_4!=null&&_4.$==1)if(_3.$0[1]&&_4.$0[1])MarkForever(res, void 0);
        else MarkReady(res, void 0);
      }
    };
    When(sn1, cont, res);
    When(sn2, cont, res);
    return res;
  }
}
function Copy(sn){
  const m=sn.s;
  if(m==null)return sn;
  else if(m!=null&&m.$==2){
    const res={s:Ready(m.$0, [])};
    WhenObsolete(sn, res);
    return res;
  }
  else if(m!=null&&m.$==3){
    const res_1={s:Waiting([], [])};
    When(sn, (v) => {
      MarkDone(res_1, sn, v);
    }, res_1);
    return res_1;
  }
  else return sn;
}
function Map3_1(fn, sn1, sn2, sn3){
  const _1=sn1.s;
  const _2=sn2.s;
  const _3=sn3.s;
  if(_1!=null&&_1.$==0)return _2!=null&&_2.$==0?_3!=null&&_3.$==0?{s:Forever(fn(_1.$0, _2.$0, _3.$0))}:Map3Opt1(fn, _1.$0, _2.$0, sn3):_3!=null&&_3.$==0?Map3Opt2(fn, _1.$0, _3.$0, sn2):Map3Opt3(fn, _1.$0, sn2, sn3);
  else if(_2!=null&&_2.$==0)return _3!=null&&_3.$==0?Map3Opt4(fn, _2.$0, _3.$0, sn1):Map3Opt5(fn, _2.$0, sn1, sn3);
  else if(_3!=null&&_3.$==0)return Map3Opt6(fn, _3.$0, sn1, sn2);
  else {
    const res={s:Waiting([], [])};
    const cont=() => {
      const m=res.s;
      if(!(m!=null&&m.$==0||m!=null&&m.$==2)){
        const _4=ValueAndForever(sn1);
        const _5=ValueAndForever(sn2);
        const _6=ValueAndForever(sn3);
        if(_4!=null&&_4.$==1)if(_5!=null&&_5.$==1)if(_6!=null&&_6.$==1)if(_4.$0[1]&&_5.$0[1]&&_6.$0[1])MarkForever(res, fn(_4.$0[0], _5.$0[0], _6.$0[0]));
        else MarkReady(res, fn(_4.$0[0], _5.$0[0], _6.$0[0]));
      }
    };
    When(sn1, cont, res);
    When(sn2, cont, res);
    When(sn3, cont, res);
    return res;
  }
}
function Map3Opt1(fn, x, y, sn3){
  return Map_2((z) => fn(x, y, z), sn3);
}
function Map3Opt2(fn, x, z, sn2){
  return Map_2((y) => fn(x, y, z), sn2);
}
function Map3Opt3(fn, x, sn2, sn3){
  return Map2_1((_1, _2) => fn(x, _1, _2), sn2, sn3);
}
function Map3Opt4(fn, y, z, sn1){
  return Map_2((x) => fn(x, y, z), sn1);
}
function Map3Opt5(fn, y, sn1, sn3){
  return Map2_1((_1, _2) => fn(_1, y, _2), sn1, sn3);
}
function Map3Opt6(fn, z, sn1, sn2){
  return Map2_1((_1, _2) => fn(_1, _2, z), sn1, sn2);
}
function Trim(s){
  return s.replace(new RegExp("^\\s+"), "").replace(new RegExp("\\s+$"), "");
}
function Substring(s, ix, ct){
  return s.substr(ix, ct);
}
function concat(separator, strings){
  return ofSeq_1(strings).join(separator);
}
function SplitChars(s, sep, opts){
  return Split(s, new RegExp("["+RegexEscape(sep.join(""))+"]"), opts);
}
function StartsWith(t, s){
  return t.substring(0, s.length)==s;
}
function Split(s, pat, opts){
  return opts===1?filter_2((x) => x!=="", SplitWith(s, pat)):SplitWith(s, pat);
}
function RegexEscape(s){
  return s.replace(new RegExp("[-\\/\\\\^$*+?.()|[\\]{}]", "g"), "\\$&");
}
function SplitWith(str, pat){
  return str.split(pat);
}
function forall_1(f, s){
  return forall(f, protect(s));
}
function protect(s){
  return s==null?"":s;
}
function InMemory(init_4){
  return new ArrayStorage(init_4);
}
function exists_2(f, x){
  let e=false;
  let i=0;
  while(!e&&i<length_1(x))
    if(f(x[i]))e=true;
    else i=i+1;
  return e;
}
function tryFindIndex_1(f, arr){
  let res=null;
  let i=0;
  while(i<arr.length&&res==null)
    {
      f(arr[i])?res=Some(i):void 0;
      i=i+1;
    }
  return res;
}
function ofList(xs){
  const q=[];
  let l=xs;
  while(!(l.$==0))
    {
      q.push(head(l));
      l=tail(l);
    }
  return q;
}
function sortInPlace(arr){
  mapInPlace_1((t) => t[0], mapiInPlace((_1, _2) =>[_2, _1], arr).sort(Compare));
}
function filter_2(f, arr){
  const r=[];
  for(let i=0, _1=arr.length-1;i<=_1;i++)if(f(arr[i]))r.push(arr[i]);
  return r;
}
function ofSeq_1(xs){
  if(xs instanceof Array)return xs.slice();
  else if(xs instanceof FSharpList)return ofList(xs);
  else {
    const q=[];
    const o=Get_1(xs);
    try {
      while(o.MoveNext())
        q.push(o.Current);
      return q;
    }
    finally {
      if(typeof o=="object"&&isIDisposable(o))o.Dispose();
    }
  }
}
function iter_2(f, arr){
  for(let i=0, _1=arr.length-1;i<=_1;i++)f(arr[i]);
}
function map_2(f, arr){
  const r=new Array(arr.length);
  for(let i=0, _1=arr.length-1;i<=_1;i++)r[i]=f(arr[i]);
  return r;
}
function forall_2(f, x){
  let a=true;
  let i=0;
  while(a&&i<length_1(x))
    if(f(x[i]))i=i+1;
    else a=false;
  return a;
}
function tryPick(f, arr){
  let res=null;
  let i=0;
  while(i<arr.length&&res==null)
    {
      const m=f(arr[i]);
      if(m!=null&&m.$==1)res=m;
      i=i+1;
    }
  return res;
}
function init_3(size, f){
  if(size<0)FailWith("Negative size given.");
  const r=new Array(size);
  for(let i=0, _1=size-1;i<=_1;i++)r[i]=f(i);
  return r;
}
function foldBack(f, arr, zero){
  let acc=zero;
  const len=arr.length;
  for(let i=1, _1=len;i<=_1;i++)acc=f(arr[len-i], acc);
  return acc;
}
function pick(f, arr){
  const m=tryPick(f, arr);
  return m==null?FailWith("KeyNotFoundException"):m.$0;
}
function choose(f, arr){
  const q=[];
  for(let i=0, _1=arr.length-1;i<=_1;i++){
    const m=f(arr[i]);
    if(m==null){ }
    else q.push(m.$0);
  }
  return q;
}
function concat_1(xs){
  return Array.prototype.concat.apply([], ofSeq_1(xs));
}
function create(size, value){
  const r=new Array(size);
  for(let i=0, _1=size-1;i<=_1;i++)r[i]=value;
  return r;
}
function Get_1(x){
  return x instanceof Array?ArrayEnumerator(x):Equals(typeof x, "string")?StringEnumerator(x):x.GetEnumerator();
}
function ArrayEnumerator(s){
  return new T(0, null, (e) => {
    const i=e.s;
    return i<length_1(s)&&(e.c=get(s, i),e.s=i+1,true);
  }, void 0);
}
function StringEnumerator(s){
  return new T(0, null, (e) => {
    const i=e.s;
    return i<s.length&&(e.c=s[i],e.s=i+1,true);
  }, void 0);
}
function Get0(x){
  return x instanceof Array?ArrayEnumerator(x):Equals(typeof x, "string")?StringEnumerator(x):"GetEnumerator0"in x?x.GetEnumerator0():x.GetEnumerator();
}
class T extends Object_1 {
  s;
  c;
  n;
  d;
  e;
  MoveNext(){
    const m=this.n(this);
    this.e=m?1:2;
    return m;
  }
  get Current(){
    return this.e===1?this.c:this.e===0?FailWith("Enumeration has not started. Call MoveNext."):FailWith("Enumeration already finished.");
  }
  Dispose(){
    if(this.d)this.d(this);
  }
  constructor(s, c, n, d){
    super();
    this.s=s;
    this.c=c;
    this.n=n;
    this.d=d;
    this.e=0;
  }
}
function CubicInOut(){
  return _c_3.CubicInOut;
}
class DoubleInterpolation {
  static DoubleInterpolation=Create_3(DoubleInterpolation, {$:0});
  Interpolate(t, x, y){
    return x+t*(y-x);
  }
  $;
}
class Trans extends Object_1 {
  change;
  enter;
  exit;
  flags;
  Copy(change, enter, exit, flags){
    const ch=change==null?(_1) =>(_2) => this.TChange(_1, _2):change.$0;
    const d=this.TEnter;
    const d_1=this.TExit;
    const d_2=this.TFlags;
    return new Trans("New_3", (a, a_1) =>(ch(a))(a_1), enter==null?d:enter.$0, exit==null?d_1:exit.$0, flags==null?d_2:flags.$0);
  }
  get TFlags(){
    return this.flags;
  }
  get TExit(){
    return this.exit;
  }
  get TEnter(){
    return this.enter;
  }
  TChange(x, y){
    return this.change(x, y);
  }
  constructor(i, _1, _2, _3, _4){
    let ch;
    if(i=="New_1"){
      ch=_1;
      i="New_3";
      _1=ch;
      _2=Const_1;
      _3=Const_1;
      _4=1;
    }
    if(i=="New_3"){
      const change=_1;
      const enter=_2;
      const exit=_3;
      const flags=_4;
      super();
      this.change=change;
      this.enter=enter;
      this.exit=exit;
      this.flags=flags;
    }
  }
}
function Id_1(){
  set_counter(counter()+1);
  return"uid"+String(counter());
}
function Int(){
  set_counter(counter()+1);
  return counter();
}
function set_counter(_1){
  _c_4.counter=_1;
}
function counter(){
  return _c_4.counter;
}
function TryParse(s, r){
  return TryParse_2(s, -2147483648, 2147483647, r);
}
let _c_2=Lazy((_i) => class $StartupCode_Templates {
  static {
    _c_2=_i(this);
  }
  static RenderedFullDocTemplate;
  static TextHoleRE;
  static GlobalHoles;
  static LocalTemplatesLoaded;
  static LoadedTemplates;
  static {
    this.LoadedTemplates=new Dictionary("New_5");
    this.LocalTemplatesLoaded=false;
    this.GlobalHoles=new Dictionary("New_5");
    this.TextHoleRE="\\${([^}]+)}";
    this.RenderedFullDocTemplate=null;
  }
});
class HashSet extends Object_1 {
  equals;
  hash;
  data;
  count;
  SAdd(item){
    return this.add(item);
  }
  Contains(item){
    const arr=this.data[this.hash(item)];
    return arr==null?false:this.arrContains(item, arr);
  }
  add(item){
    const h=this.hash(item);
    const arr=this.data[h];
    return arr==null?(this.data[h]=[item],this.count=this.count+1,true):this.arrContains(item, arr)?false:(arr.push(item),this.count=this.count+1,true);
  }
  arrContains(item, arr){
    let c=true;
    let i=0;
    const l=arr.length;
    while(c&&i<l)
      if(this.equals.apply(null, [arr[i], item]))c=false;
      else i=i+1;
    return!c;
  }
  GetEnumerator(){
    return Get_1(concat_2(this.data));
  }
  ExceptWith(xs){
    const e=Get_1(xs);
    try {
      while(e.MoveNext())
        this.Remove(e.Current);
    }
    finally {
      if(typeof e=="object"&&isIDisposable(e))e.Dispose();
    }
  }
  get Count(){
    return this.count;
  }
  IntersectWith(xs){
    const other=new HashSet("New_4", xs, this.equals, this.hash);
    const all=concat_2(this.data);
    for(let i=0, _1=all.length-1;i<=_1;i++){
      const item=all[i];
      if(!other.Contains(item))this.Remove(item);
    }
  }
  Remove(item){
    const arr=this.data[this.hash(item)];
    return arr==null?false:this.arrRemove(item, arr)&&(this.count=this.count-1,true);
  }
  CopyTo(arr, index){
    const all=concat_2(this.data);
    for(let i=0, _1=all.length-1;i<=_1;i++)set(arr, i+index, all[i]);
  }
  arrRemove(item, arr){
    let c=true;
    let i=0;
    const l=arr.length;
    while(c&&i<l)
      if(this.equals.apply(null, [arr[i], item])){
        arr.splice(i, 1);
        c=false;
      }
      else i=i+1;
    return!c;
  }
  constructor(i, _1, _2, _3){
    if(i=="New_3"){
      i="New_4";
      _1=[];
      _2=Equals;
      _3=Hash;
    }
    let init_4;
    if(i=="New_2"){
      init_4=_1;
      i="New_4";
      _1=init_4;
      _2=Equals;
      _3=Hash;
    }
    if(i=="New_4"){
      const init_5=_1;
      const equals=_2;
      const hash=_3;
      super();
      this.equals=equals;
      this.hash=hash;
      this.data=[];
      this.count=0;
      const e=Get_1(init_5);
      try {
        while(e.MoveNext())
          this.add(e.Current);
      }
      finally {
        if(typeof e=="object"&&isIDisposable(e))e.Dispose();
      }
    }
  }
}
function TextDoc(Item){
  return{$:4, $0:Item};
}
function TextNodeDoc(Item){
  return{$:5, $0:Item};
}
function AppendDoc(Item1, Item2){
  return{
    $:0, 
    $0:Item1, 
    $1:Item2
  };
}
function EmbedDoc(Item){
  return{$:2, $0:Item};
}
function TreeDoc(Item){
  return{$:6, $0:Item};
}
function ElemDoc(Item){
  return{$:1, $0:Item};
}
class Exception extends Object_1 { }
function LinkElement(el, children){
  InsertDoc(el, children, null);
}
function CreateTextNode(){
  return{
    Text:globalThis.document.createTextNode(""), 
    Dirty:false, 
    Value:""
  };
}
function UpdateTextNode(n, t){
  n.Value=t;
  n.Dirty=true;
}
function InsertDoc(parent, doc, pos){
  while(true)
    {
      if(doc!=null&&doc.$==1){
        const e=doc.$0;
        return InsertNode(parent, e.El, pos);
      }
      else if(doc!=null&&doc.$==2){
        const d=doc.$0;
        d.Dirty=false;
        doc=d.Current;
      }
      else if(doc==null)return pos;
      else if(doc!=null&&doc.$==4){
        const t=doc.$0;
        return InsertNode(parent, t.Text, pos);
      }
      else if(doc!=null&&doc.$==5){
        const t_1=doc.$0;
        return InsertNode(parent, t_1, pos);
      }
      else if(doc!=null&&doc.$==6)return foldBack((_1, _2) =>((((parent_1) =>(el) =>(pos_1) => el==null||el.constructor===Object?InsertDoc(parent_1, el, pos_1):InsertNode(parent_1, el, pos_1))(parent))(_1))(_2), doc.$0.Els, pos);
      else {
        const b=doc.$1;
        const a=doc.$0;
        doc=a;
        pos=InsertDoc(parent, b, pos);
      }
    }
}
function CreateRunState(parent, doc){
  return New_8(get_Empty_1(), CreateElemNode(parent, EmptyAttr(), doc));
}
function PerformAnimatedUpdate(childrenOnly, st, doc){
  if(get_UseAnimations()){
    const f=() => {
      const cur=FindAll(doc);
      const change=ComputeChangeAnim(st, cur);
      const enter=ComputeEnterAnim(st, cur);
      const x=Play(Append(change, ComputeExitAnim(st, cur)));
      const f_1=() => {
        const x_1=SyncElemNodesNextFrame(childrenOnly, st);
        const f_2=() => {
          const x_2=Play(enter);
          const f_3=() => {
            st.PreviousNodes=cur;
            return Return(void 0);
          };
          return Bind_1(x_2, (_1, _2) =>(f_3(_1))(_2));
        };
        return Bind_1(x_1, (_1, _2) =>(f_2(_1))(_2));
      };
      return Bind_1(x, (_1, _2) =>(f_1(_1))(_2));
    };
    return Delay((_1, _2) =>(f(_1))(_2));
  }
  else return SyncElemNodesNextFrame(childrenOnly, st);
}
function PerformSyncUpdate(childrenOnly, st, doc){
  const cur=FindAll(doc);
  SyncElemNode(childrenOnly, st.Top);
  st.PreviousNodes=cur;
}
function CreateEmbedNode(){
  return{Current:null, Dirty:false};
}
function UpdateEmbedNode(node, upd){
  node.Current=upd;
  node.Dirty=true;
}
function InsertNode(parent, node, pos){
  InsertAt(parent, pos, node);
  return node;
}
function CreateElemNode(el, attr_1, children){
  LinkElement(el, children);
  const attr_2=Insert(el, attr_1);
  return DocElemNode.New(attr_2, children, null, el, Int(), GetOptional(attr_2.OnAfterRender));
}
function SyncElemNodesNextFrame(childrenOnly, st){
  if(BatchUpdatesEnabled()){
    const c=(ok) => {
      requestAnimationFrame(() => {
        SyncElemNode(childrenOnly, st.Top);
        ok();
      });
    };
    return FromContinuations((_1, _2, _3) => c.apply(null, [_1, _2, _3]));
  }
  else {
    SyncElemNode(childrenOnly, st.Top);
    return Return(void 0);
  }
}
function ComputeExitAnim(st, cur){
  return Concat(map_2((n) => GetExitAnim(n.Attr), ToArray(Except(cur, Filter((n) => HasExitAnim(n.Attr), st.PreviousNodes)))));
}
function ComputeEnterAnim(st, cur){
  return Concat(map_2((n) => GetEnterAnim(n.Attr), ToArray(Except(st.PreviousNodes, Filter((n) => HasEnterAnim(n.Attr), cur)))));
}
function ComputeChangeAnim(st, cur){
  const f=(n) => HasChangeAnim(n.Attr);
  const relevant=(a) => Filter(f, a);
  return Concat(map_2((n) => GetChangeAnim(n.Attr), ToArray(Intersect(relevant(st.PreviousNodes), relevant(cur)))));
}
function SyncElemNode(childrenOnly, el){
  !childrenOnly?SyncElement(el):void 0;
  Sync_1(el.Children);
  AfterRender(el);
}
function InsertBeforeDelim(afterDelim, doc){
  const p=afterDelim.parentNode;
  const before=globalThis.document.createTextNode("");
  p.insertBefore(before, afterDelim);
  LinkPrevElement(afterDelim, doc);
  return before;
}
function LinkPrevElement(el, children){
  InsertDoc(el.parentNode, children, el);
}
function CreateDelimitedRunState(ldelim, rdelim, doc){
  return New_8(get_Empty_1(), CreateDelimitedElemNode(ldelim, rdelim, EmptyAttr(), doc));
}
function SyncElement(el){
  function hasDirtyChildren(el_1){
    function dirty(doc){
      while(true)
        {
          if(doc!=null&&doc.$==0){
            const b=doc.$1;
            const a=doc.$0;
            if(dirty(a))return true;
            else doc=b;
          }
          else if(doc!=null&&doc.$==2){
            const d=doc.$0;
            if(d.Dirty)return true;
            else doc=d.Current;
          }
          else if(doc!=null&&doc.$==6){
            const t=doc.$0;
            return t.Dirty||exists_2(hasDirtyChildren, t.Holes);
          }
          else return false;
        }
    }
    return dirty(el_1.Children);
  }
  Sync(el.El, el.Attr);
  if(hasDirtyChildren(el))DoSyncElement(el);
}
function Sync_1(doc){
  while(true)
    {
      if(doc!=null&&doc.$==1)return SyncElemNode(false, doc.$0);
      else if(doc!=null&&doc.$==2){
        const n=doc.$0;
        doc=n.Current;
      }
      else if(doc==null)return;
      else if(doc!=null&&doc.$==5)return;
      else if(doc!=null&&doc.$==4){
        const d=doc.$0;
        return d.Dirty?(d.Text.nodeValue=d.Value,d.Dirty=false):void 0;
      }
      else if(doc!=null&&doc.$==6){
        const t=doc.$0;
        iter_2((h) => {
          SyncElemNode(false, h);
        }, t.Holes);
        iter_2((t_1) => {
          Sync(t_1[0], t_1[1]);
        }, t.Attrs);
        return AfterRender(t);
      }
      else {
        const b=doc.$1;
        const a=doc.$0;
        Sync_1(a);
        doc=b;
      }
    }
}
function AfterRender(el){
  const m=GetOptional(el.Render);
  if(m!=null&&m.$==1){
    m.$0(el.El);
    SetOptional(el, "Render", null);
  }
}
function CreateDelimitedElemNode(ldelim, rdelim, attr_1, children){
  const el=ldelim.parentNode;
  LinkPrevElement(rdelim, children);
  const attr_2=Insert(el, attr_1);
  return DocElemNode.New(attr_2, children, Some([ldelim, rdelim]), el, Int(), GetOptional(attr_2.OnAfterRender));
}
function DoSyncElement(el){
  const m=GetOptional(el.Delimiters);
  const parent=m==null?el.El:m.$0[1].parentNode;
  function ins(doc, pos){
    while(true)
      {
        if(doc!=null&&doc.$==1)return doc.$0.El;
        else if(doc!=null&&doc.$==2){
          const d=doc.$0;
          if(d.Dirty){
            d.Dirty=false;
            return InsertDoc(parent, d.Current, pos);
          }
          else doc=d.Current;
        }
        else if(doc==null)return pos;
        else if(doc!=null&&doc.$==4)return doc.$0.Text;
        else if(doc!=null&&doc.$==5)return doc.$0;
        else if(doc!=null&&doc.$==6){
          const t=doc.$0;
          if(t.Dirty)t.Dirty=false;
          return foldBack((_2, _3) => _2==null||_2.constructor===Object?ins(_2, _3):_2, t.Els, pos);
        }
        else {
          const b=doc.$1;
          const a=doc.$0;
          doc=a;
          pos=ins(b, pos);
        }
      }
  }
  Iter((e) => {
    RemoveNode(parent, e);
  }, Except_2(DocChildren(el), Children(parent, GetOptional(el.Delimiters))));
  const _1=el.Children;
  const m_1=GetOptional(el.Delimiters);
  ins(_1, m_1!=null&&m_1.$==1?m_1.$0[1]:null);
}
class Runner {
  Invoke(a){
    return this.$0(a);
  }
  Expect(assertionCount){
    return this.AddTest((_1, _2) => _1.expect(assertionCount(_2)));
  }
  static Yield(x){
    return Runner.Runner(() => Choice1Of2(x));
  }
  EqualMsg(actual, expected, message){
    return this.AddTest((_1, _2) => {
      const actual_1=actual(_2);
      const expected_1=expected(_2);
      return _1.pushResult({
        result:Equals(actual_1, expected_1), 
        actual:actual_1, 
        expected:expected_1, 
        message:message
      });
    });
  }
  EqualMsgAsync(actual, expected, message){
    return this.AddTestAsync((_1, _2) => {
      const f=() => {
        const expected_1=expected(_2);
        const x=actual(_2);
        const f_1=(a) => {
          _1.pushResult({
            result:Equals(a, expected_1), 
            actual:a, 
            expected:expected_1, 
            message:message
          });
          return Return(void 0);
        };
        return Bind_1(x, (_3, _4) =>(f_1(_3))(_4));
      };
      return Delay((_3, _4) =>(f(_3))(_4));
    });
  }
  For(y){
    return Runner.Runner((asserter) => {
      const m=this.Invoke(asserter);
      if(m.$==1){
        const a=m.$0;
        const f=() => {
          const f_1=(a_1) => {
            const m_1=y(a_1).Invoke(asserter);
            return m_1.$==1?m_1.$0:Return(m_1.$0);
          };
          return Bind_1(a, (_1, _2) =>(f_1(_1))(_2));
        };
        return Choice2Of2(Delay((_1, _2) =>(f(_1))(_2)));
      }
      else return y(m.$0).Invoke(asserter);
    });
  }
  Equal(actual, expected){
    return this.AddTest((_1, _2) => {
      const actual_1=actual(_2);
      const expected_1=expected(_2);
      return _1.pushResult({
        result:Equals(actual_1, expected_1), 
        actual:actual_1, 
        expected:expected_1
      });
    });
  }
  EqualAsync(actual, expected){
    return this.AddTestAsync((_1, _2) => {
      const f=() => {
        const expected_1=expected(_2);
        const x=actual(_2);
        const f_1=(a) => {
          _1.pushResult({
            result:Equals(a, expected_1), 
            actual:a, 
            expected:expected_1
          });
          return Return(void 0);
        };
        return Bind_1(x, (_3, _4) =>(f_1(_3))(_4));
      };
      return Delay((_3, _4) =>(f(_3))(_4));
    });
  }
  static Zero(){
    return Runner.Runner(() => Choice1Of2(void 0));
  }
  IsFalseMsg(value, message){
    return this.AddTest((_1, _2) => _1.strictEqual(value(_2), false, message));
  }
  IsTrueMsg(value, message){
    return this.AddTest((_1, _2) => _1.strictEqual(value(_2), true, message));
  }
  AddTest(t){
    return Runner.Runner((asserter) => Map_3((args) => {
      t(asserter, args);
      return args;
    }, this.Invoke(asserter)));
  }
  static Runner(Item){
    return Create_3(Runner, {$:0, $0:Item});
  }
  AddTestAsync(t){
    return Runner.Runner((asserter) => MapAsync_2((args) => {
      const f=() => {
        const x=t(asserter, args);
        const f_1=() => Return(args);
        return Bind_1(x, (_1, _2) =>(f_1(_1))(_2));
      };
      return Delay((_1, _2) =>(f(_1))(_2));
    }, this.Invoke(asserter)));
  }
  $;
  $0;
}
function WithTimeout(_1, a){
  return a;
}
function Map_3(f, x){
  if(x.$==1){
    const args=x.$0;
    const f_1=() => {
      const f_2=(a) => Return(f(a));
      return Bind_1(args, (_1, _2) =>(f_2(_1))(_2));
    };
    return Choice2Of2(Delay((_1, _2) =>(f_1(_1))(_2)));
  }
  else return Choice1Of2(f(x.$0));
}
function MapAsync_2(f, x){
  if(x.$==1){
    const args=x.$0;
    const f_1=() => {
      const f_2=(a) => f(a);
      return Bind_1(args, (_1, _2) =>(f_2(_1))(_2));
    };
    return Choice2Of2(Delay((_1, _2) =>(f_1(_1))(_2)));
  }
  else return Choice2Of2(f(x.$0));
}
function Delay(mk){
  return(c) => {
    try {
      mk(void 0, c);
    }
    catch(e){
      c.k(No(e));
    }
  };
}
function Return(x){
  return(c) => {
    c.k(Ok(x));
  };
}
function FromContinuations(subscribe){
  return(c) => {
    let continued=false;
    const once=(cont) => {
      if(continued)FailWith("A continuation provided by Async.FromContinuations was invoked multiple times");
      else {
        continued=true;
        scheduler().Fork(cont);
      }
    };
    subscribe((a) => {
      once(() => {
        c.k(Ok(a));
      });
    }, (e) => {
      once(() => {
        c.k(No(e));
      });
    }, (e) => {
      once(() => {
        c.k(Cc(e));
      });
    });
  };
}
function Bind_1(r, f){
  return checkCancel((c) => {
    r(New_6((a) => {
      if(a.$==0){
        const x=a.$0;
        scheduler().Fork(() => {
          try {
            f(x, c);
          }
          catch(e){
            c.k(No(e));
          }
        });
      }
      else scheduler().Fork(() => {
        c.k(a);
      });
    }, c.ct));
  });
}
function Sleep(ms){
  return(c) => {
    let creg;
    const pending=setTimeout(() => {
      creg.Dispose();
      scheduler().Fork(() => {
        c.k(Ok(void 0));
      });
    }, ms);
    creg=Register(c.ct, () => {
      clearTimeout(pending);
      scheduler().Fork(() => {
        cancel(c);
      });
    });
  };
}
function AwaitEvent(e, ca){
  return(c) => {
    let sub, creg;
    sub=e.Subscribe(observer((x) => {
      sub.Dispose();
      creg.Dispose();
      scheduler().Fork(() => {
        c.k(Ok(x));
      });
    }));
    creg=Register(c.ct, () => {
      if(ca!=null&&ca.$==1)ca.$0();
      else {
        sub.Dispose();
        scheduler().Fork(() => {
          cancel(c);
        });
      }
    });
  };
}
function TryFinally(run, f){
  return(c) => {
    run(New_6((r) => {
      try {
        f();
        c.k(r);
      }
      catch(e){
        c.k(No(e));
      }
    }, c.ct));
  };
}
function TryWith(r, f){
  return(c) => {
    r(New_6((a) => {
      if(a.$==0)c.k(Ok(a.$0));
      else if(a.$==1){
        try {
          f(a.$0, c);
        }
        catch(e){
          c.k(a);
        }
      }
      else c.k(a);
    }, c.ct));
  };
}
function Start(c, ctOpt){
  const d=(defCTS())[0];
  const ct=ctOpt==null?d:ctOpt.$0;
  scheduler().Fork(() => {
    if(!ct.c)c(New_6((a) => {
      if(a.$==1)UncaughtAsyncError(a.$0);
    }, ct));
  });
}
function checkCancel(r){
  return(c) => {
    if(c.ct.c)cancel(c);
    else r(c);
  };
}
function Register(ct, callback){
  if(ct===noneCT())return{Dispose(){ }};
  else {
    const i=ct.r.push(callback)-1;
    return{Dispose(){
      return set(ct.r, i, () => { });
    }};
  }
}
function cancel(c){
  c.k(Cc(new OperationCanceledException("New", c.ct)));
}
function defCTS(){
  return _c_8.defCTS;
}
function UncaughtAsyncError(e){
  console.log("WebSharper: Uncaught asynchronous exception", e);
}
function scheduler(){
  return _c_8.scheduler;
}
function noneCT(){
  return _c_8.noneCT;
}
function Zero(){
  return _c_8.Zero;
}
function StartWithContinuations(c, s, f, cc, ctOpt){
  const d=(defCTS())[0];
  const ct=ctOpt==null?d:ctOpt.$0;
  if(!ct.c)c(New_6((a) => {
    if(a.$==1)f(a.$0);
    else if(a.$==2)cc(a.$0);
    else s(a.$0);
  }, ct));
}
class Event_2 {
  Handlers;
  Trigger(x){
    const a=this.Handlers.slice();
    for(let i=0, _1=a.length-1;i<=_1;i++)(get(a, i))(null, x);
  }
  Subscribe_1(observer_1){
    const this_1=this;
    const h=(_1, x) => observer_1.OnNext(x);
    this.AddHandler_1(h);
    return{Dispose(){
      return this_1.RemoveHandler_1(h);
    }};
  }
  AddHandler_1(h){
    this.Handlers.push(h);
  }
  RemoveHandler_1(h){
    const o=tryFindIndex((y) => Equals(h, y), this.Handlers);
    if(o==null){ }
    else this.Handlers.splice(o.$0, 1);
  }
  Dispose(){ }
  Subscribe(observer_1){
    return this.Subscribe_1(observer_1);
  }
  static New(Handlers){
    return Create_3(Event_2, {Handlers:Handlers});
  }
}
function ofSeqNonCopying(xs){
  if(xs instanceof Array)return xs;
  else if(xs instanceof FSharpList)return ofList(xs);
  else if(xs===null)return[];
  else {
    const q=[];
    const o=Get_1(xs);
    try {
      while(o.MoveNext())
        q.push(o.Current);
      return q;
    }
    finally {
      if(typeof o=="object"&&isIDisposable(o))o.Dispose();
    }
  }
}
function TreeReduce(defaultValue, reduction, array){
  const l=length_1(array);
  function loop(off){
    return(len) => {
      switch(len<=0?0:len===1?off>=0&&off<l?1:2:2){
        case 0:
          return defaultValue;
        case 1:
          return get(array, off);
        case 2:
          const l2=len/2>>0;
          return reduction((loop(off))(l2), (loop(off+l2))(len-l2));
      }
    };
  }
  return(loop(0))(l);
}
function mapInPlace(f, arr){
  for(let i=0, _1=arr.length-1;i<=_1;i++)arr[i]=f(arr[i]);
  return arr;
}
function MapTreeReduce(mapping, defaultValue, reduction, array){
  const l=length_1(array);
  function loop(off){
    return(len) => {
      switch(len<=0?0:len===1?off>=0&&off<l?1:2:2){
        case 0:
          return defaultValue;
        case 1:
          return mapping(get(array, off));
        case 2:
          const l2=len/2>>0;
          return reduction((loop(off))(l2), (loop(off+l2))(len-l2));
      }
    };
  }
  return(loop(0))(l);
}
class DocElemNode {
  Attr;
  Children;
  Delimiters;
  El;
  ElKey;
  Render;
  Equals(o){
    return this.ElKey===o.ElKey;
  }
  GetHashCode(){
    return this.ElKey;
  }
  static New(Attr_1, Children_1, Delimiters, El, ElKey, Render){
    const _1={
      Attr:Attr_1, 
      Children:Children_1, 
      El:El, 
      ElKey:ElKey
    };
    SetOptional(_1, "Delimiters", Delimiters);
    SetOptional(_1, "Render", Render);
    return Create_3(DocElemNode, _1);
  }
}
function Ready(Item1, Item2){
  return{
    $:2, 
    $0:Item1, 
    $1:Item2
  };
}
function Waiting(Item1, Item2){
  return{
    $:3, 
    $0:Item1, 
    $1:Item2
  };
}
function Forever(Item){
  return{$:0, $0:Item};
}
function Obsolete(sn){
  let _1;
  const m=sn.s;
  if(!(m==null||(m!=null&&m.$==2?(_1=m.$1,false):m!=null&&m.$==3?(_1=m.$1,false):true))){
    sn.s=null;
    for(let i=0, _2=length_1(_1)-1;i<=_2;i++){
      const o=get(_1, i);
      if(typeof o=="object")(((sn_1) => {
        Obsolete(sn_1);
      })(o));
      else o();
    }
  }
}
class DynamicAttrNode extends Object_1 {
  push;
  value;
  dirty;
  updates;
  get NChanged(){
    return this.updates;
  }
  NGetExitAnim(parent){
    return get_Empty();
  }
  NGetEnterAnim(parent){
    return get_Empty();
  }
  NGetChangeAnim(parent){
    return get_Empty();
  }
  NSync(parent){
    if(this.dirty){
      (this.push(parent))(this.value);
      this.dirty=false;
    }
  }
  constructor(view, push){
    super();
    this.push=push;
    this.value=void 0;
    this.dirty=false;
    this.updates=Map((x) => {
      this.value=x;
      this.dirty=true;
    }, view);
  }
}
class ArrayStorage extends Object_1 {
  init;
  SRemoveIf(pred, arr){
    return filter_2((i) =>!pred(i), arr);
  }
  SSetAt(idx, elem, arr){
    set(arr, idx, elem);
    return arr;
  }
  SAppend(i, arr){
    arr.push(i);
    return arr;
  }
  SInit(){
    return this.init;
  }
  constructor(init_4){
    super();
    this.init=init_4;
  }
}
let _c_3=Lazy((_i) => class $StartupCode_Animation {
  static {
    _c_3=_i(this);
  }
  static UseAnimations;
  static CubicInOut;
  static {
    this.CubicInOut=Easing.Custom((t) => {
      const t2=t*t;
      return 3*t2-2*(t2*t);
    });
    this.UseAnimations=true;
  }
});
class AnimatedAttrNode extends Object_1 {
  tr;
  push;
  logical;
  visible;
  dirty;
  updates;
  pushVisible(el, v){
    this.visible=Some(v);
    this.dirty=true;
    (this.push(el))(v);
  }
  sync(p){
    if(this.dirty){
      const x=this.logical;
      if(x==null){ }
      else(this.push(p))(x.$0);
      this.visible=this.logical;
      this.dirty=false;
    }
  }
  get NChanged(){
    return this.updates;
  }
  NGetExitAnim(parent){
    const m=this.visible;
    return WhenDone(() => {
      this.dirty=true;
      this.visible=null;
    }, m!=null&&m.$==1?Pack(Map_1((v) => {
      this.pushVisible(parent, v);
    }, AnimateExit(this.tr, m.$0))):get_Empty());
  }
  NGetEnterAnim(parent){
    let _1, _2;
    const _3=this.visible;
    const _4=this.logical;
    switch(_3==null?_4!=null&&_4.$==1?(_1=_4.$0,1):2:_4!=null&&_4.$==1?this.dirty?(_1=[_4.$0, _3.$0],0):2:2){
      case 0:
        _2=Pack(Map_1((v) => {
          this.pushVisible(parent, v);
        }, AnimateChange(this.tr, _1[1], _1[0])));
        break;
      case 1:
        _2=Pack(Map_1((v) => {
          this.pushVisible(parent, v);
        }, AnimateEnter(this.tr, _1)));
        break;
      case 2:
        _2=get_Empty();
        break;
    }
    return WhenDone(() => {
      this.sync(parent);
    }, _2);
  }
  NGetChangeAnim(parent){
    let _1;
    const _2=this.visible;
    const _3=this.logical;
    return WhenDone(() => {
      this.sync(parent);
    }, _2!=null&&_2.$==1&&(_3!=null&&_3.$==1&&(this.dirty&&(_1=[_3.$0, _2.$0],true)))?Pack(Map_1((v) => {
      this.pushVisible(parent, v);
    }, AnimateChange(this.tr, _1[1], _1[0]))):get_Empty());
  }
  NSync(parent){ }
  constructor(tr, view, push){
    super();
    this.tr=tr;
    this.push=push;
    this.logical=null;
    this.visible=null;
    this.dirty=true;
    this.updates=Map((x) => {
      this.logical=Some(x);
      this.dirty=true;
    }, view);
  }
}
function notPresent(){
  throw new KeyNotFoundException("New");
}
function convertTextNode(n){
  let m=null;
  let li=0;
  const s=n.textContent;
  const strRE=new RegExp(TextHoleRE(), "g");
  while(m=strRE.exec(s),m!==null)
    {
      n.parentNode.insertBefore(globalThis.document.createTextNode(string(s, Some(li), Some(strRE.lastIndex-get(m, 0).length-1))), n);
      li=strRE.lastIndex;
      const hole=globalThis.document.createElement("span");
      hole.setAttribute("ws-replace", get(m, 1).toLowerCase());
      n.parentNode.insertBefore(hole, n);
    }
  strRE.lastIndex=0;
  n.textContent=string(s, Some(li), null);
}
function failNotLoaded(name){
  console.warn("Instantiating non-loaded template", name);
}
function fillTextHole(instance, fillWith, templateName){
  const m=instance.querySelector("[ws-replace]");
  return Equals(m, null)?(console.warn("Filling non-existent text hole", templateName),null):(m.parentNode.replaceChild(globalThis.document.createTextNode(fillWith), m),Some(m.getAttribute("ws-replace")));
}
function removeHolesExcept(instance, dontRemove){
  const run=(attrName) => {
    foreachNotPreserved(instance, "["+attrName+"]", (e) => {
      if(!dontRemove.Contains(e.getAttribute(attrName)))e.removeAttribute(attrName);
    });
  };
  run("ws-attr");
  run("ws-onafterrender");
  run("ws-var");
  foreachNotPreserved(instance, "[ws-hole]", (e) => {
    if(!dontRemove.Contains(e.getAttribute("ws-hole"))){
      e.removeAttribute("ws-hole");
      while(e.hasChildNodes())
        e.removeChild(e.lastChild);
    }
  });
  foreachNotPreserved(instance, "[ws-replace]", (e) => {
    if(!dontRemove.Contains(e.getAttribute("ws-replace")))e.parentNode.removeChild(e);
  });
  foreachNotPreserved(instance, "[ws-on]", (e) => {
    e.setAttribute("ws-on", concat(" ", filter_2((x) => dontRemove.Contains(get(SplitChars(x, [":"], 1), 1)), SplitChars(e.getAttribute("ws-on"), [" "], 1))));
  });
  foreachNotPreserved(instance, "[ws-attr-holes]", (e) => {
    const holeAttrs=SplitChars(e.getAttribute("ws-attr-holes"), [" "], 1);
    for(let i=0, _1=holeAttrs.length-1;i<=_1;i++){
      const attrName=get(holeAttrs, i);
      const this_1=new RegExp(TextHoleRE(), "g");
      e.setAttribute(attrName, e.getAttribute(attrName).replace(this_1, (_2, _3) => dontRemove.Contains(_3)?_2:""));
    }
  });
}
function fillInstanceAttrs(instance, fillWith){
  convertAttrs(fillWith);
  const name=fillWith.nodeName.toLowerCase();
  const m=instance.querySelector("[ws-attr="+name+"]");
  if(Equals(m, null))console.warn("Filling non-existent attr hole", name);
  else {
    m.removeAttribute("ws-attr");
    for(let i=0, _1=fillWith.attributes.length-1;i<=_1;i++){
      const a=fillWith.attributes.item(i);
      if(a.name=="class"&&m.hasAttribute("class"))m.setAttribute("class", m.getAttribute("class")+" "+a.nodeValue);
      else m.setAttribute(a.name, a.nodeValue);
    }
  }
}
function mapHoles(t, mappings){
  const run=(attrName) => {
    foreachNotPreserved(t, "["+attrName+"]", (e) => {
      let o=null;
      const m=[mappings.TryGetValue(e.getAttribute(attrName).toLowerCase(), {get:() => o, set:(v) => {
        o=v;
      }}), o];
      if(m[0])e.setAttribute(attrName, m[1]);
    });
  };
  run("ws-hole");
  run("ws-replace");
  run("ws-attr");
  run("ws-onafterrender");
  run("ws-var");
  foreachNotPreserved(t, "[ws-on]", (e) => {
    e.setAttribute("ws-on", concat(" ", map_2((x) => {
      const a=SplitChars(x, [":"], 1);
      let o=null;
      const m=[mappings.TryGetValue(get(a, 1), {get:() => o, set:(v) => {
        o=v;
      }}), o];
      return m[0]?get(a, 0)+":"+m[1]:x;
    }, SplitChars(e.getAttribute("ws-on"), [" "], 1))));
  });
  foreachNotPreserved(t, "[ws-attr-holes]", (e) => {
    const holeAttrs=SplitChars(e.getAttribute("ws-attr-holes"), [" "], 1);
    for(let i=0, _1=holeAttrs.length-1;i<=_1;i++){
      const attrName=get(holeAttrs, i);
      e.setAttribute(attrName, fold((_2, _3) => {
        const a=KeyValue(_3);
        return _2.replace(new RegExp("\\${"+a[0]+"}", "ig"), "${"+a[1]+"}");
      }, e.getAttribute(attrName), mappings));
    }
  });
}
function fill(fillWith, p, n){
  while(true)
    {
      if(fillWith.hasChildNodes())n=p.insertBefore(fillWith.lastChild, n);
      else return;
    }
}
function convertAttrs(el){
  const attrs=el.attributes;
  const toRemove=[];
  const events=[];
  const holedAttrs=[];
  for(let i=0, _1=attrs.length-1;i<=_1;i++){
    const a=attrs.item(i);
    if(StartsWith(a.nodeName, "ws-on")&&a.nodeName!="ws-onafterrender"&&a.nodeName!="ws-on"){
      toRemove.push(a.nodeName);
      events.push(string(a.nodeName, Some("ws-on".length), null)+":"+a.nodeValue.toLowerCase());
    }
    else if(!StartsWith(a.nodeName, "ws-")&&(new RegExp(TextHoleRE())).test(a.nodeValue)){
      const this_1=new RegExp(TextHoleRE(), "g");
      a.nodeValue=a.nodeValue.replace(this_1, (_2, _3) =>"${"+_3.toLowerCase()+"}");
      holedAttrs.push(a.nodeName);
    }
  }
  if(!(events.length==0))el.setAttribute("ws-on", concat(" ", events));
  if(!(holedAttrs.length==0))el.setAttribute("ws-attr-holes", concat(" ", holedAttrs));
  const lowercaseAttr=(name) => {
    const m=el.getAttribute(name);
    if(m==null){ }
    else el.setAttribute(name, m.toLowerCase());
  };
  lowercaseAttr("ws-hole");
  lowercaseAttr("ws-replace");
  lowercaseAttr("ws-attr");
  lowercaseAttr("ws-onafterrender");
  lowercaseAttr("ws-var");
  iter_2((a_1) => {
    el.removeAttribute(a_1);
  }, toRemove);
}
function string(source, start, finish){
  if(start==null){
    if(finish!=null&&finish.$==1){
      const f=finish.$0;
      return f<0?"":source.slice(0, f+1);
    }
    else return"";
  }
  else if(finish==null)return source.slice(start.$0);
  else {
    const f_1=finish.$0;
    return f_1<0?"":source.slice(start.$0, f_1+1);
  }
}
function insufficient(){
  return FailWith("The input sequence has an insufficient number of elements.");
}
function mapiInPlace(f, arr){
  for(let i=0, _1=arr.length-1;i<=_1;i++)arr[i]=f(i, arr[i]);
  return arr;
}
function mapInPlace_1(f, arr){
  for(let i=0, _1=arr.length-1;i<=_1;i++)arr[i]=f(arr[i]);
}
function nonNegative(){
  return FailWith("The input must be non-negative.");
}
function arrContains(item, arr){
  let c=true;
  let i=0;
  while(c&&i<length_1(arr))
    if(Equals(arr[i], item))c=false;
    else i=i+1;
  return!c;
}
class KeyCollection extends Object_1 {
  d;
  GetEnumerator(){
    return Get_1(map_1((kvp) => kvp.K, this.d));
  }
  constructor(d){
    super();
    this.d=d;
  }
}
function BatchUpdatesEnabled(){
  return _c_5.BatchUpdatesEnabled;
}
function StartProcessor(procAsync){
  let st=0;
  function work(){
    const f=() => {
      const f_1=() => {
        const m=st;
        return Equals(m, 1)?(st=0,Zero()):Equals(m, 2)?(st=1,work()):Zero();
      };
      return Bind_1(procAsync, (_1, _2) =>(f_1(_1))(_2));
    };
    return Delay((_1, _2) =>(f(_1))(_2));
  }
  return() => {
    const m=st;
    if(Equals(m, 0)){
      st=1;
      Start(work(), null);
    }
    else Equals(m, 1)?st=2:void 0;
  };
}
function Choice1Of2(Item){
  return{$:0, $0:Item};
}
function Choice2Of2(Item){
  return{$:1, $0:Item};
}
function New_6(k, ct){
  return{k:k, ct:ct};
}
function No(Item){
  return{$:1, $0:Item};
}
function Ok(Item){
  return{$:0, $0:Item};
}
function Cc(Item){
  return{$:2, $0:Item};
}
function StartTo(comp, k){
  StartWithContinuations(comp, k, (e) => {
    OnError(e);
  }, () => { }, null);
}
function OnError(e){
  return console.log("WebSharper UI: Uncaught asynchronous exception", e);
}
function observer(h){
  return{
    OnCompleted(){ }, 
    OnError(){ }, 
    OnNext(args){
      return h(args);
    }
  };
}
class IndexOutOfRangeException extends Error {
  constructor(i, _1){
    if(i=="New"){
      i="New_1";
      _1="Index was outside the bounds of the array.";
    }
    if(i=="New_1"){
      const message=_1;
      super(message);
    }
  }
}
function New_7(DynElem, DynFlags, DynNodes, OnAfterRender_1){
  const _1={
    DynElem:DynElem, 
    DynFlags:DynFlags, 
    DynNodes:DynNodes
  };
  SetOptional(_1, "OnAfterRender", OnAfterRender_1);
  return _1;
}
function StringApply(){
  return _c_6.StringApply;
}
function BoolCheckedApply(){
  return _c_6.BoolCheckedApply;
}
function IntApplyUnchecked(){
  return _c_6.IntApplyUnchecked;
}
function FileApplyUnchecked(){
  return _c_6.FileApplyUnchecked;
}
function IntApplyChecked(){
  return _c_6.IntApplyChecked;
}
function ApplyValue(get_1, set_1, var_1){
  let expectedValue=null;
  return[(el) => {
    const onChange=() => {
      var_1.UpdateMaybe((v) => {
        let _1;
        expectedValue=get_1(el);
        return expectedValue!=null&&expectedValue.$==1&&(!Equals(expectedValue.$0, v)&&(_1=[expectedValue, expectedValue.$0],true))?_1[0]:null;
      });
    };
    el.addEventListener("change", onChange);
    el.addEventListener("input", onChange);
    el.addEventListener("keypress", onChange);
  }, (x) => {
    const _1=set_1(x);
    return(_2) => _2==null?void 0:_1(_2.$0);
  }, Map((v) => expectedValue!=null&&expectedValue.$==1&&(Equals(expectedValue.$0, v)&&true)?null:Some(v), var_1.View)];
}
function StringSet(){
  return _c_6.StringSet;
}
function StringGet(){
  return _c_6.StringGet;
}
function StringListSet(){
  return _c_6.StringListSet;
}
function StringListGet(){
  return _c_6.StringListGet;
}
function DateTimeSetUnchecked(){
  return _c_6.DateTimeSetUnchecked;
}
function DateTimeGetUnchecked(){
  return _c_6.DateTimeGetUnchecked;
}
function FileApplyValue(get_1, set_1, var_1){
  let expectedValue=null;
  return[(el) => {
    el.addEventListener("change", () => {
      var_1.UpdateMaybe((v) => {
        let _1;
        expectedValue=get_1(el);
        return expectedValue!=null&&expectedValue.$==1&&(expectedValue.$0!==v&&(_1=[expectedValue, expectedValue.$0],true))?_1[0]:null;
      });
    });
  }, (x) => {
    const _1=set_1(x);
    return(_2) => _2==null?void 0:_1(_2.$0);
  }, Map((v) => expectedValue!=null&&expectedValue.$==1&&(Equals(expectedValue.$0, v)&&true)?null:Some(v), var_1.View)];
}
function FileSetUnchecked(){
  return _c_6.FileSetUnchecked;
}
function FileGetUnchecked(){
  return _c_6.FileGetUnchecked;
}
function IntSetUnchecked(){
  return _c_6.IntSetUnchecked;
}
function IntGetUnchecked(){
  return _c_6.IntGetUnchecked;
}
function IntSetChecked(){
  return _c_6.IntSetChecked;
}
function IntGetChecked(){
  return _c_6.IntGetChecked;
}
function FloatSetUnchecked(){
  return _c_6.FloatSetUnchecked;
}
function FloatGetUnchecked(){
  return _c_6.FloatGetUnchecked;
}
function FloatSetChecked(){
  return _c_6.FloatSetChecked;
}
function FloatGetChecked(){
  return _c_6.FloatGetChecked;
}
function Const_2(v){
  return Def(0, () => v);
}
function UseAnimations(){
  return _c_3.UseAnimations;
}
function Def(d, f){
  return{Compute:f, Duration:d};
}
function Actions(a){
  return ConcatActions(choose((a_1) => a_1.$==1?Some(a_1.$0):null, ToArray_1(a.$0)));
}
function Finalize(a){
  iter_2((a_1) => {
    if(a_1.$==0)a_1.$0();
  }, ToArray_1(a.$0));
}
function ConcatActions(xs){
  const xs_1=ofSeqNonCopying(xs);
  const m=length_1(xs_1);
  if(m===0)return Const_2();
  else if(m===1)return get(xs_1, 0);
  else {
    const dur=max(map_1((anim) => anim.Duration, xs_1));
    const xs_2=map_2((x) => Prolong(dur, x), xs_1);
    return Def(dur, (t) => {
      iter_2((anim) => {
        anim.Compute(t);
      }, xs_2);
    });
  }
}
function List(a){
  return a.$0;
}
function Prolong(nextDuration, anim){
  const comp=anim.Compute;
  const dur=anim.Duration;
  const last=Create_2(() => anim.Compute(anim.Duration));
  return{Compute:(t) => t>=dur?last.f():comp(t), Duration:nextDuration};
}
let _c_4=Lazy((_i) => class $StartupCode_Abbrev {
  static {
    _c_4=_i(this);
  }
  static counter;
  static {
    this.counter=0;
  }
});
function New_8(PreviousNodes, Top){
  return{PreviousNodes:PreviousNodes, Top:Top};
}
function get_Empty_1(){
  return NodeSet(new HashSet("New_3"));
}
function FindAll(doc){
  const q=[];
  function recF(recI, _1){
    while(true)
      switch(recI){
        case 0:
          if(_1!=null&&_1.$==0){
            const b=_1.$1;
            const a=_1.$0;
            recF(0, a);
            _1=b;
          }
          else if(_1!=null&&_1.$==1){
            const el=_1.$0;
            _1=el;
            recI=1;
          }
          else if(_1!=null&&_1.$==2){
            const em=_1.$0;
            _1=em.Current;
          }
          else if(_1!=null&&_1.$==6){
            const x=_1.$0.Holes;
            return(((a_1) =>(a_2) => {
              iter_2(a_1, a_2);
            })(loopEN))(x);
          }
          else return;
          break;
        case 1:
          q.push(_1);
          _1=_1.Children;
          recI=0;
          break;
      }
  }
  function loop(node){
    return recF(0, node);
  }
  function loopEN(el){
    return recF(1, el);
  }
  loop(doc);
  return NodeSet(new HashSet("New_2", q));
}
function NodeSet(Item){
  return{$:0, $0:Item};
}
function Filter(f, a){
  return NodeSet(Filter_1(f, a.$0));
}
function Except(a, a_1){
  return NodeSet(Except_1(a.$0, a_1.$0));
}
function ToArray(a){
  return ToArray_2(a.$0);
}
function Intersect(a, a_1){
  return NodeSet(Intersect_1(a.$0, a_1.$0));
}
let _c_5=Lazy((_i) => class Proxy {
  static {
    _c_5=_i(this);
  }
  static BatchUpdatesEnabled;
  static {
    this.BatchUpdatesEnabled=true;
  }
});
class Updates_1 {
  c;
  s;
  v;
  static Create(v){
    let var_1=null;
    var_1=Updates_1.New(v, null, () => {
      let c=var_1.s;
      return c===null?(c=Copy(var_1.c()),var_1.s=c,WhenObsoleteRun(c, () => {
        var_1.s=null;
      }),c):c;
    });
    return var_1;
  }
  static New(Current, Snap, VarView){
    return Create_3(Updates_1, {
      c:Current, 
      s:Snap, 
      v:VarView
    });
  }
}
function New_9(IsCancellationRequested, Registrations){
  return{c:IsCancellationRequested, r:Registrations};
}
class Scheduler extends Object_1 {
  idle;
  robin;
  Fork(action){
    this.robin.push(action);
    this.idle?(this.idle=false,setTimeout(() => {
      this.tick();
    }, 0)):void 0;
  }
  tick(){
    const t=Date.now();
    let loop=true;
    while(loop)
      if(this.robin.length===0){
        this.idle=true;
        loop=false;
      }
      else {
        (this.robin.shift())();
        Date.now()-t>40?(setTimeout(() => {
          this.tick();
        }, 0),loop=false):void 0;
      }
  }
  constructor(){
    super();
    this.idle=true;
    this.robin=[];
  }
}
class OperationCanceledException extends Error {
  ct;
  constructor(i, _1, _2, _3){
    let ct;
    if(i=="New"){
      ct=_1;
      i="New_1";
      _1="The operation was canceled.";
      _2=null;
      _3=ct;
    }
    if(i=="New_1"){
      const message=_1;
      const inner=_2;
      const ct_1=_3;
      super(message);
      this.inner=inner;
      this.ct=ct_1;
    }
  }
}
let _c_6=Lazy((_i) => class Client {
  static {
    _c_6=_i(this);
  }
  static FloatApplyChecked;
  static FloatGetChecked;
  static FloatSetChecked;
  static FloatApplyUnchecked;
  static FloatGetUnchecked;
  static FloatSetUnchecked;
  static IntApplyChecked;
  static IntGetChecked;
  static IntSetChecked;
  static IntApplyUnchecked;
  static IntGetUnchecked;
  static IntSetUnchecked;
  static FileApplyUnchecked;
  static FileGetUnchecked;
  static FileSetUnchecked;
  static DateTimeApplyUnchecked;
  static DateTimeGetUnchecked;
  static DateTimeSetUnchecked;
  static StringListApply;
  static StringListGet;
  static StringListSet;
  static StringApply;
  static StringGet;
  static StringSet;
  static BoolCheckedApply;
  static EmptyAttr;
  static {
    this.EmptyAttr=null;
    this.BoolCheckedApply=(var_1) =>[(el) => {
      el.addEventListener("change", () => var_1.Get()!=el.checked?var_1.Set(el.checked):void 0);
    }, (_1) =>(_2) => _2!=null&&_2.$==1?void(_1.checked=_2.$0):void 0, Map(Some, var_1.View)];
    this.StringSet=(el) =>(s_8) => {
      el.value=s_8;
    };
    this.StringGet=(el) => Some(el.value);
    const g=StringGet();
    const s=StringSet();
    this.StringApply=(v) => ApplyValue(g, s, v);
    this.StringListSet=(el) =>(s_8) => {
      const options_=el.options;
      for(let i=0, _1=options_.length-1;i<=_1;i++){
        const option=options_.item(i);
        option.selected=arrContains(option.value, s_8);
      }
    };
    this.StringListGet=(el) => {
      const selectedOptions=el.selectedOptions;
      const a=[];
      for(const i of Iter_1(range(0, selectedOptions.length-1))){
        const option=selectedOptions.item(i);
        a.push(option.value);
      }
      return Some(a);
    };
    const g_1=StringListGet();
    const s_1=StringListSet();
    this.StringListApply=(v) => ApplyValue(g_1, s_1, v);
    this.DateTimeSetUnchecked=(el) =>(i) => {
      el.value=(new Date(i)).toLocaleString();
    };
    this.DateTimeGetUnchecked=(el) => {
      let o, m;
      const s_8=el.value;
      if(isBlank(s_8))return Some(-8640000000000000);
      else {
        o=0;
        const m_1=TryParse_1(s_8);
        m=[m_1!=null&&m_1.$==1&&(o=m_1.$0,true), o];
        return m[0]?Some(m[1]):null;
      }
    };
    const g_2=DateTimeGetUnchecked();
    const s_2=DateTimeSetUnchecked();
    this.DateTimeApplyUnchecked=(v) => ApplyValue(g_2, s_2, v);
    this.FileSetUnchecked=() =>() => { };
    this.FileGetUnchecked=(el) => {
      const files=el.files;
      const a=[];
      for(const i of Iter_1(range(0, files.length-1)))a.push(files.item(i));
      return Some(a);
    };
    const g_3=FileGetUnchecked();
    const s_3=FileSetUnchecked();
    this.FileApplyUnchecked=(v) => FileApplyValue(g_3, s_3, v);
    this.IntSetUnchecked=(el) =>(i) => {
      el.value=String(i);
    };
    this.IntGetUnchecked=(el) => {
      const s_8=el.value;
      if(isBlank(s_8))return Some(0);
      else {
        const pd=+s_8;
        return pd!==pd>>0?null:Some(pd);
      }
    };
    const g_4=IntGetUnchecked();
    const s_4=IntSetUnchecked();
    this.IntApplyUnchecked=(v) => ApplyValue(g_4, s_4, v);
    this.IntSetChecked=(el) =>(i) => {
      const i_1=i.Input;
      return el.value!=i_1?void(el.value=i_1):void 0;
    };
    this.IntGetChecked=(el) => {
      let _1, o;
      const s_8=el.value;
      if(isBlank(s_8))_1=(el.checkValidity?el.checkValidity():true)?CheckedInput.Blank(s_8):CheckedInput.Invalid(s_8);
      else {
        o=0;
        const m=[TryParse(s_8, {get:() => o, set:(v) => {
          o=v;
        }}), o];
        _1=m[0]?CheckedInput.Valid(m[1], s_8):CheckedInput.Invalid(s_8);
      }
      return Some(_1);
    };
    const g_5=IntGetChecked();
    const s_5=IntSetChecked();
    this.IntApplyChecked=(v) => ApplyValue(g_5, s_5, v);
    this.FloatSetUnchecked=(el) =>(i) => {
      el.value=String(i);
    };
    this.FloatGetUnchecked=(el) => {
      const s_8=el.value;
      if(isBlank(s_8))return Some(0);
      else {
        const pd=+s_8;
        return isNaN(pd)?null:Some(pd);
      }
    };
    const g_6=FloatGetUnchecked();
    const s_6=FloatSetUnchecked();
    this.FloatApplyUnchecked=(v) => ApplyValue(g_6, s_6, v);
    this.FloatSetChecked=(el) =>(i) => {
      const i_1=i.Input;
      return el.value!=i_1?void(el.value=i_1):void 0;
    };
    this.FloatGetChecked=(el) => {
      let _1;
      const s_8=el.value;
      if(isBlank(s_8))_1=(el.checkValidity?el.checkValidity():true)?CheckedInput.Blank(s_8):CheckedInput.Invalid(s_8);
      else {
        const i=+s_8;
        _1=isNaN(i)?CheckedInput.Invalid(s_8):CheckedInput.Valid(i, s_8);
      }
      return Some(_1);
    };
    const g_7=FloatGetChecked();
    const s_7=FloatSetChecked();
    this.FloatApplyChecked=(v) => ApplyValue(g_7, s_7, v);
  }
});
class EventQ extends TemplateHole {
  name;
  key;
  fillWith;
  get Value(){
    return this.fillWith;
  }
  get Name(){
    return this.name;
  }
}
class AfterRender_1 extends TemplateHole {
  name;
  fillWith;
  get Value(){
    return this.fillWith;
  }
  get Name(){
    return this.name;
  }
}
class AfterRenderQ extends TemplateHole {
  name;
  key;
  fillWith;
  get Value(){
    return this.fillWith;
  }
  get Name(){
    return this.name;
  }
}
class VarDomElement extends TemplateHole {
  name;
  fillWith;
  get Value(){
    return this.fillWith;
  }
  get Name(){
    return this.name;
  }
}
class ValueCollection extends Object_1 {
  d;
  GetEnumerator(){
    return Get_1(map_1((kvp) => kvp.V, this.d));
  }
  constructor(d){
    super();
    this.d=d;
  }
}
let _c_7=Lazy((_i) => class $StartupCode_DomUtility {
  static {
    _c_7=_i(this);
  }
  static defaultWrap;
  static wrapMap;
  static rhtml;
  static rtagName;
  static rxhtmlTag;
  static {
    this.rxhtmlTag=new RegExp("<(?!area|br|col|embed|hr|img|input|link|meta|param)(([\\w:]+)[^>]*)\\/>", "gi");
    this.rtagName=new RegExp("<([\\w:]+)");
    this.rhtml=new RegExp("<|&#?\\w+;");
    const table=[1, "<table>", "</table>"];
    this.wrapMap=Object.fromEntries([["option", [1, "<select multiple='multiple'>", "</select>"]], ["legend", [1, "<fieldset>", "</fieldset>"]], ["area", [1, "<map>", "</map>"]], ["param", [1, "<object>", "</object>"]], ["thead", table], ["tbody", table], ["tfoot", table], ["tr", [2, "<table><tbody>", "</tbody></table>"]], ["col", [2, "<table><colgroup>", "</colgoup></table>"]], ["td", [3, "<table><tbody><tr>", "</tr></tbody></table>"]]]);
    this.defaultWrap=[0, "", ""];
  }
});
function Append_1(x, y){
  return x.$==0?y:y.$==0?x:{
    $:2, 
    $0:x, 
    $1:y
  };
}
function ToArray_1(xs){
  const out=[];
  function loop(xs_1){
    while(true)
      {
        if(xs_1.$==1)return out.push(xs_1.$0);
        else if(xs_1.$==2){
          const y=xs_1.$1;
          const x=xs_1.$0;
          loop(x);
          xs_1=y;
        }
        else return xs_1.$==3?iter_2((v) => {
          out.push(v);
        }, xs_1.$0):void 0;
      }
  }
  loop(xs);
  return out.slice(0);
}
function Concat_1(xs){
  const x=ofSeqNonCopying(xs);
  return TreeReduce(Empty_1(), Append_1, x);
}
function Empty_1(){
  return _c_9.Empty;
}
function Single(x){
  return{$:1, $0:x};
}
let _c_8=Lazy((_i) => class $StartupCode_Concurrency {
  static {
    _c_8=_i(this);
  }
  static GetCT;
  static Zero;
  static defCTS;
  static scheduler;
  static noneCT;
  static {
    this.noneCT=New_9(false, []);
    this.scheduler=new Scheduler();
    this.defCTS=[new CancellationTokenSource()];
    this.Zero=Return();
    this.GetCT=(c) => {
      c.k(Ok(c.ct));
    };
  }
});
function Clear(a){
  a.splice(0, length_1(a));
}
function isBlank(s){
  return forall_1(IsWhiteSpace, s);
}
class KeyNotFoundException extends Error {
  constructor(i, _1){
    if(i=="New"){
      i="New_1";
      _1="The given key was not present in the dictionary.";
    }
    if(i=="New_1"){
      const message=_1;
      super(message);
    }
  }
}
function concat_2(o){
  const r=[];
  let k;
  for(var k_1 in o)r.push.apply(r, o[k_1]);
  return r;
}
function Filter_1(ok, set_1){
  return new HashSet("New_2", filter_2(ok, ToArray_2(set_1)));
}
function Except_1(excluded, included){
  const set_1=new HashSet("New_2", ToArray_2(included));
  set_1.ExceptWith(ToArray_2(excluded));
  return set_1;
}
function ToArray_2(set_1){
  const arr=create(set_1.Count, void 0);
  set_1.CopyTo(arr, 0);
  return arr;
}
function Intersect_1(a, b){
  const set_1=new HashSet("New_2", ToArray_2(a));
  set_1.IntersectWith(ToArray_2(b));
  return set_1;
}
class CancellationTokenSource extends Object_1 {
  init;
  c;
  pending;
  r;
  constructor(){
    super();
    this.c=false;
    this.pending=null;
    this.r=[];
    this.init=1;
  }
}
function IsWhiteSpace(c){
  return c.match(new RegExp("\\s"))!==null;
}
function TryParse_1(s){
  const d=Date.parse(s);
  return isNaN(d)?null:Some(d);
}
function TryParse_2(s, min, max_1, r){
  const x=+s;
  const ok=x===x-x%1&&x>=min&&x<=max_1;
  if(ok)r.set(x);
  return ok;
}
function Children(elem, delims){
  let n;
  if(delims!=null&&delims.$==1){
    const a=[];
    n=delims.$0[0].nextSibling;
    while(n!==delims.$0[1])
      {
        a.push(n);
        n=n.nextSibling;
      }
    return DomNodes(a);
  }
  else {
    const _1=elem.childNodes.length;
    const o=elem.childNodes;
    return DomNodes(init_3(_1, (i) => o[i]));
  }
}
function Except_2(a, a_1){
  const excluded=a.$0;
  return DomNodes(filter_2((n) => forall_2((k) =>!(n===k), excluded), a_1.$0));
}
function Iter(f, a){
  iter_2(f, a.$0);
}
function DocChildren(node){
  const q=[];
  function loop(doc){
    while(true)
      {
        if(doc!=null&&doc.$==2){
          const d=doc.$0;
          doc=d.Current;
        }
        else if(doc!=null&&doc.$==1)return q.push(doc.$0.El);
        else if(doc==null)return;
        else if(doc!=null&&doc.$==5)return q.push(doc.$0);
        else if(doc!=null&&doc.$==4)return q.push(doc.$0.Text);
        else if(doc!=null&&doc.$==6){
          const x=doc.$0.Els;
          return(((a_1) =>(a_2) => {
            iter_2(a_1, a_2);
          })((a_1) => {
            if(a_1==null||a_1.constructor===Object)loop(a_1);
            else q.push(a_1);
          }))(x);
        }
        else {
          const b=doc.$1;
          const a=doc.$0;
          loop(a);
          doc=b;
        }
      }
  }
  loop(node.Children);
  return DomNodes(ofSeqNonCopying(q));
}
function DomNodes(Item){
  return{$:0, $0:Item};
}
function Create_2(f){
  return New_10(false, f, forceLazy);
}
function forceLazy(){
  const v=this.v();
  this.c=true;
  this.v=v;
  this.f=cachedLazy;
  return v;
}
function cachedLazy(){
  return this.v;
}
let _c_9=Lazy((_i) => class $StartupCode_AppendList {
  static {
    _c_9=_i(this);
  }
  static Empty;
  static {
    this.Empty={$:0};
  }
});
function New_10(created, evalOrVal, force){
  return{
    c:created, 
    v:evalOrVal, 
    f:force
  };
}
Main();

//# sourceMappingURL=WebSharper.UI.Tests.js.map
