(window.webpackJsonp=window.webpackJsonp||[]).push([[41],{"1yen":function(t,e,n){"use strict";n.r(e),n.d(e,"ReplaceContentComponent",(function(){return O}));var o=n("D57K"),i=n("5/c3"),a=n("LR82"),s=n("z5yO"),c=n("uuTa"),r=n("1C3z"),l=n("BLjT"),p=n("PlBB"),u=n("2pW/"),b=n("hOvr"),d=n("LuBX"),h=n("ZSGP"),f=n("8AiQ"),g=n("9HSk"),m=n("Qc/f"),v=n("r4gC"),y=n("OeRG");function C(t,e){if(1&t&&(r.Wb(0,"mat-option",11),r.Qc(1),r.Vb()),2&t){var n=e.$implicit;r.oc("value",n.value),r.Bb(1),r.Rc(n.label)}}var O=function(){function t(t,e,n,o,i){this.dialogRef=t,this.contentGroupService=e,this.router=n,this.route=o,this.snackBar=i,this.hostClass="dialog-component",this.subscription=new a.a,this.hasChild=!!this.route.snapshot.firstChild,this.item={id:null,guid:this.route.snapshot.paramMap.get("guid"),part:this.route.snapshot.paramMap.get("part"),index:parseInt(this.route.snapshot.paramMap.get("index"),10),add:!!this.route.snapshot.queryParamMap.get("add")}}return t.prototype.ngOnInit=function(){this.getConfig(),this.refreshOnChildClosed()},t.prototype.ngOnDestroy=function(){this.subscription.unsubscribe()},t.prototype.save=function(){var t=this;this.snackBar.open("Saving..."),this.contentGroupService.saveItem(this.item).subscribe((function(){t.snackBar.open("Saved",null,{duration:2e3}),t.closeDialog()}))},t.prototype.copySelected=function(){var t={items:[{ContentTypeName:this.contentTypeName,DuplicateEntity:this.item.id}]},e=Object(c.a)(t);this.router.navigate(["edit/"+e],{relativeTo:this.route})},t.prototype.closeDialog=function(){this.dialogRef.close()},t.prototype.getConfig=function(){var t=this;this.contentGroupService.getItems(this.item).subscribe((function(e){var n,i,a=Object.keys(e.Items);t.options=[];try{for(var s=Object(o.i)(a),c=s.next();!c.done;c=s.next()){var r=parseInt(c.value,10);t.options.push({label:e.Items[r]+" ("+r+")",value:r})}}catch(l){n={error:l}}finally{try{c&&!c.done&&(i=s.return)&&i.call(s)}finally{if(n)throw n.error}}t.item.id||t.item.add||(t.item.id=e.SelectedId),t.contentTypeName||(t.contentTypeName=e.ContentTypeName)}))},t.prototype.refreshOnChildClosed=function(){var t=this;this.subscription.add(this.router.events.pipe(Object(s.a)((function(t){return t instanceof i.b}))).subscribe((function(e){var n,o=t.hasChild;if(t.hasChild=!!t.route.snapshot.firstChild,!t.hasChild&&o){t.getConfig();var i=null===(n=t.router.getCurrentNavigation().extras)||void 0===n?void 0:n.state;i&&(t.item.id=i[Object.keys(i)[0]])}})))},t.\u0275fac=function(e){return new(e||t)(r.Qb(l.g),r.Qb(p.a),r.Qb(i.c),r.Qb(i.a),r.Qb(u.b))},t.\u0275cmp=r.Kb({type:t,selectors:[["app-replace-content"]],hostVars:1,hostBindings:function(t,e){2&t&&r.Zb("className",e.hostClass)},decls:20,vars:2,consts:[["mat-dialog-title",""],[1,"dialog-title-box"],[1,"dialog-description"],[1,"options-box"],["appearance","standard","color","accent",1,"options-box__field"],["name","Language",3,"ngModel","ngModelChange"],[3,"value",4,"ngFor","ngForOf"],["mat-icon-button","","matTooltip","Copy",1,"options-box__copy",3,"click"],[1,"dialog-component-actions"],["mat-raised-button","",3,"click"],["mat-raised-button","","color","accent",3,"click"],[3,"value"]],template:function(t,e){1&t&&(r.Wb(0,"div",0),r.Wb(1,"div",1),r.Qc(2,"Replace Content Item"),r.Vb(),r.Vb(),r.Rb(3,"router-outlet"),r.Wb(4,"p",2),r.Qc(5," By replacing a content-item you can make a other content appear in the slot of the original content.\n"),r.Vb(),r.Wb(6,"div",3),r.Wb(7,"mat-form-field",4),r.Wb(8,"mat-label"),r.Qc(9,"Choose item"),r.Vb(),r.Wb(10,"mat-select",5),r.ec("ngModelChange",(function(t){return e.item.id=t})),r.Oc(11,C,2,2,"mat-option",6),r.Vb(),r.Vb(),r.Wb(12,"button",7),r.ec("click",(function(){return e.copySelected()})),r.Wb(13,"mat-icon"),r.Qc(14,"file_copy"),r.Vb(),r.Vb(),r.Vb(),r.Wb(15,"div",8),r.Wb(16,"button",9),r.ec("click",(function(){return e.closeDialog()})),r.Qc(17,"Cancel"),r.Vb(),r.Wb(18,"button",10),r.ec("click",(function(){return e.save()})),r.Qc(19,"Save"),r.Vb(),r.Vb()),2&t&&(r.Bb(10),r.oc("ngModel",e.item.id),r.Bb(1),r.oc("ngForOf",e.options))},directives:[l.h,i.e,b.c,b.g,d.a,h.l,h.o,f.l,g.a,m.a,v.a,y.l],styles:[".options-box[_ngcontent-%COMP%]{display:flex;align-items:flex-end}.options-box__field[_ngcontent-%COMP%]{width:40%}.options-box__copy[_ngcontent-%COMP%]{margin-left:8px}"]}),t}()}}]);
//# sourceMappingURL=https://sources.2sxc.org/11.04.01/ng-edit/replace-content-component.c99bc27f8414a814f26f.js.map