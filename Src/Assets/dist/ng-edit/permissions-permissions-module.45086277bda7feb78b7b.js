(window.webpackJsonp=window.webpackJsonp||[]).push([[38],{SNUn:function(t,n,e){"use strict";e.d(n,"a",(function(){return s}));var i=e("o9tz"),o=e("1C3z"),a=e("ykZ8"),r=e("GTfO"),s=function(){function t(t,n){this.metadataService=t,this.entitiesService=n}return t.prototype.getAll=function(t,n,e){return this.metadataService.getMetadata(t,n,e,i.a.contentTypes.permissions)},t.prototype.delete=function(t){return this.entitiesService.delete(i.a.contentTypes.permissions,t,!1)},t.\u0275fac=function(n){return new(n||t)(o.ac(a.a),o.ac(r.a))},t.\u0275prov=o.Mb({token:t,factory:t.\u0275fac}),t}()},jl54:function(t,n,e){"use strict";e.r(n);var i=e("8AiQ"),o=e("BLjT"),a=e("9HSk"),r=e("r4gC"),s=e("Qc/f"),c=e("OeRG"),l=e("2pW/"),u=e("G6Ml"),d=e("5/c3"),f=e("nXrb"),p=e("D57K"),h={name:"SET_PERMISSIONS_DIALOG",initContext:!0,panelSize:"large",panelClass:null,getComponent:function(){return Object(p.b)(this,void 0,void 0,(function(){return Object(p.e)(this,(function(t){switch(t.label){case 0:return[4,Promise.all([e.e(4),e.e(0),e.e(37)]).then(e.bind(null,"SST1"))];case 1:return[2,t.sent().PermissionsComponent]}}))}))}},g=e("it7M"),b=e("1C3z"),C=[{path:"",component:f.a,data:{dialog:h},children:[{matcher:g.a,loadChildren:function(){return Promise.all([e.e(1),e.e(2),e.e(3),e.e(7),e.e(10),e.e(8),e.e(0),e.e(26)]).then(e.bind(null,"B+J5")).then((function(t){return t.EditModule}))}}]}],m=function(){function t(){}return t.\u0275mod=b.Ob({type:t}),t.\u0275inj=b.Nb({factory:function(n){return new(n||t)},imports:[[d.d.forChild(C)],d.d]}),t}(),v=e("O6Xb"),w=e("Iv+g"),O=e("SNUn"),y=e("ykZ8"),S=e("GTfO");e.d(n,"PermissionsModule",(function(){return j}));var j=function(){function t(){}return t.\u0275mod=b.Ob({type:t}),t.\u0275inj=b.Nb({factory:function(n){return new(n||t)},providers:[w.a,O.a,y.a,S.a],imports:[[i.c,m,v.a,o.f,a.b,r.b,s.b,u.b.withComponents([]),c.r,l.c]]}),t}()},nXrb:function(t,n,e){"use strict";e.d(n,"a",(function(){return u}));var i=e("D57K"),o=e("LR82"),a=e("50eG"),r=e("1C3z"),s=e("BLjT"),c=e("5/c3"),l=e("Iv+g"),u=function(){function t(t,n,e,i,a){if(this.dialog=t,this.viewContainerRef=n,this.router=e,this.route=i,this.context=a,this.subscription=new o.a,this.dialogConfig=this.route.snapshot.data.dialog,!this.dialogConfig)throw new Error("Could not find config for dialog. Did you forget to add DialogConfig to route data?")}return t.prototype.ngOnInit=function(){return Object(i.b)(this,void 0,void 0,(function(){var t,n=this;return Object(i.e)(this,(function(e){switch(e.label){case 0:return Object(a.a)("Open dialog:",this.dialogConfig.name,"Context id:",this.context.id,"Context:",this.context),t=this,[4,this.dialogConfig.getComponent()];case 1:return t.component=e.sent(),this.dialogConfig.initContext&&this.context.init(this.route),this.dialogRef=this.dialog.open(this.component,{backdropClass:"dialog-backdrop",panelClass:Object(i.g)(["dialog-panel","dialog-panel-"+this.dialogConfig.panelSize,this.dialogConfig.showScrollbar?"show-scrollbar":"no-scrollbar"],this.dialogConfig.panelClass?this.dialogConfig.panelClass:[]),viewContainerRef:this.viewContainerRef,autoFocus:!1,closeOnNavigation:!1,position:{top:"0"}}),this.subscription.add(this.dialogRef.afterClosed().subscribe((function(t){if(Object(a.a)("Dialog was closed:",n.dialogConfig.name,"Data:",t),n.route.pathFromRoot.length<=3)try{window.parent.$2sxc.totalPopup.close()}catch(e){}else n.router.navigate(["./"],n.route.snapshot.url.length>0?{relativeTo:n.route.parent,state:t}:{relativeTo:n.route.parent.parent,state:t})}))),[2]}}))}))},t.prototype.ngOnDestroy=function(){this.subscription.unsubscribe(),this.subscription=null,this.dialogConfig=null,this.component=null,this.dialogRef.close(),this.dialogRef=null},t.\u0275fac=function(n){return new(n||t)(r.Qb(s.b),r.Qb(r.O),r.Qb(c.c),r.Qb(c.a),r.Qb(l.a))},t.\u0275cmp=r.Kb({type:t,selectors:[["app-dialog-entry"]],decls:0,vars:0,template:function(t,n){},styles:[""]}),t}()}}]);
//# sourceMappingURL=https://sources.2sxc.org/11.04.01/ng-edit/permissions-permissions-module.45086277bda7feb78b7b.js.map