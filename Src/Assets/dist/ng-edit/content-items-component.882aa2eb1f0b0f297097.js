(window.webpackJsonp=window.webpackJsonp||[]).push([[20],{gur7:function(t,e,n){"use strict";n.r(e);var i=n("D57K"),a=n("5/c3"),o=n("LR82"),r=n("z5yO"),l=n("KLQV"),c=n("o9tz"),s=n("1C3z"),u=n("+raR"),d=n("ZSGP"),p=function(){function t(){this.published="",this.metadata=""}return t.prototype.agInit=function(t){this.params=t},t.prototype.isFilterActive=function(){return""!==this.published||""!==this.metadata},t.prototype.doesFilterPass=function(t){var e,n,i=this.params.valueGetter(t.node);return e=""===this.published||null!=i.published&&i.published.toString()===this.published,n=""===this.metadata||null!=i.metadata&&i.metadata.toString()===this.metadata,e&&n},t.prototype.getModel=function(){if(this.isFilterActive())return{filterType:"pub-meta",published:this.published,metadata:this.metadata}},t.prototype.setModel=function(t){this.published=t?t.published:"",this.metadata=t?t.metadata:""},t.prototype.afterGuiAttached=function(t){},t.prototype.filterChanged=function(){this.params.filterChangedCallback()},t.\u0275fac=function(e){return new(e||t)},t.\u0275cmp=s.Kb({type:t,selectors:[["app-pub-meta-filter"]],decls:18,vars:2,consts:[[1,"title"],[3,"ngModel","ngModelChange"],["value",""],["value","true"],["value","false"]],template:function(t,e){1&t&&(s.Wb(0,"div",0),s.Qc(1,"Published"),s.Vb(),s.Wb(2,"mat-radio-group",1),s.ec("ngModelChange",(function(t){return e.published=t}))("ngModelChange",(function(){return e.filterChanged()})),s.Wb(3,"mat-radio-button",2),s.Qc(4,"All"),s.Vb(),s.Wb(5,"mat-radio-button",3),s.Qc(6,"Published"),s.Vb(),s.Wb(7,"mat-radio-button",4),s.Qc(8,"Not published"),s.Vb(),s.Vb(),s.Wb(9,"div",0),s.Qc(10,"Metadata"),s.Vb(),s.Wb(11,"mat-radio-group",1),s.ec("ngModelChange",(function(t){return e.metadata=t}))("ngModelChange",(function(){return e.filterChanged()})),s.Wb(12,"mat-radio-button",2),s.Qc(13,"All"),s.Vb(),s.Wb(14,"mat-radio-button",3),s.Qc(15,"Is metadata"),s.Vb(),s.Wb(16,"mat-radio-button",4),s.Qc(17,"Is not metadata"),s.Vb(),s.Vb()),2&t&&(s.Bb(2),s.oc("ngModel",e.published),s.Bb(9),s.oc("ngModel",e.metadata))},directives:[u.b,d.l,d.o,u.a],styles:[".title[_ngcontent-%COMP%]{padding:12px 12px 0}.mat-radio-group[_ngcontent-%COMP%]{display:flex;flex-direction:column;justify-content:space-between;overflow:hidden;padding:12px;width:160px;height:104px}"]}),t}(),m=n("8AiQ"),b=n("r4gC"),f=n("Qc/f");function h(t,e){1&t&&(s.Wb(0,"mat-icon",4),s.Qc(1,"visibility"),s.Vb())}function y(t,e){1&t&&(s.Wb(0,"mat-icon",5),s.Qc(1,"visibility_off"),s.Vb())}function g(t,e){if(1&t&&(s.Ub(0),s.Wb(1,"mat-icon",6),s.Qc(2,"local_offer"),s.Vb(),s.Tb()),2&t){var n=s.ic();s.Bb(1),s.oc("matTooltip",n.metadataTooltip)}}var v=function(){function t(){}return t.prototype.agInit=function(t){this.value=t.value;var e=t.data;e.Metadata&&(this.metadataTooltip="Metadata\nType: "+e.Metadata.TargetType+(e.Metadata.KeyNumber?"\nNumber: "+e.Metadata.KeyNumber:"")+(e.Metadata.KeyString?"\nString: "+e.Metadata.KeyString:"")+(e.Metadata.KeyGuid?"\nGuid: "+e.Metadata.KeyGuid:""))},t.prototype.refresh=function(t){return!0},t.\u0275fac=function(e){return new(e||t)},t.\u0275cmp=s.Kb({type:t,selectors:[["app-content-items-status"]],decls:4,vars:3,consts:[[1,"icon-container"],["matTooltip","Published",4,"ngIf"],["matTooltip","Not published",4,"ngIf"],[4,"ngIf"],["matTooltip","Published"],["matTooltip","Not published"],[1,"meta-icon",3,"matTooltip"]],template:function(t,e){1&t&&(s.Wb(0,"div",0),s.Oc(1,h,2,0,"mat-icon",1),s.Oc(2,y,2,0,"mat-icon",2),s.Oc(3,g,3,1,"ng-container",3),s.Vb()),2&t&&(s.Bb(1),s.oc("ngIf",e.value.published),s.Bb(1),s.oc("ngIf",!e.value.published),s.Bb(1),s.oc("ngIf",e.value.metadata))},directives:[m.m,b.a,f.a],styles:[".meta-icon[_ngcontent-%COMP%]{margin-left:8px}"]}),t}(),T=n("OeRG"),C=function(){function t(){}return t.prototype.agInit=function(t){this.params=t,this.item=t.data},t.prototype.refresh=function(t){return!0},t.prototype.clone=function(){this.params.onClone(this.item)},t.prototype.export=function(){this.params.onExport(this.item)},t.prototype.delete=function(){this.params.onDelete(this.item)},t.\u0275fac=function(e){return new(e||t)},t.\u0275cmp=s.Kb({type:t,selectors:[["app-content-items-actions"]],decls:10,vars:0,consts:[[1,"actions-component"],["matRipple","","matTooltip","Copy",1,"like-button","highlight",3,"click"],["matRipple","","matTooltip","Export",1,"like-button","highlight",3,"click"],["matRipple","","matTooltip","Delete",1,"like-button","highlight",3,"click"]],template:function(t,e){1&t&&(s.Wb(0,"div",0),s.Wb(1,"div",1),s.ec("click",(function(){return e.clone()})),s.Wb(2,"mat-icon"),s.Qc(3,"file_copy"),s.Vb(),s.Vb(),s.Wb(4,"div",2),s.ec("click",(function(){return e.export()})),s.Wb(5,"mat-icon"),s.Qc(6,"cloud_download"),s.Vb(),s.Vb(),s.Wb(7,"div",3),s.ec("click",(function(){return e.delete()})),s.Wb(8,"mat-icon"),s.Qc(9,"delete"),s.Vb(),s.Vb(),s.Vb())},directives:[T.q,f.a,b.a],styles:[""]}),t}();function I(t,e){if(1&t&&(s.Wb(0,"span",2),s.Qc(1),s.Vb()),2&t){var n=s.ic();s.Bb(1),s.Rc(n.entities)}}var k=function(){function t(){}return t.prototype.agInit=function(t){this.params=t,Array.isArray(t.value)&&(this.encodedValue=this.htmlEncode(t.value.join(", ")),t.colDef.allowMultiValue&&(this.entities=t.value.length))},t.prototype.refresh=function(t){return!0},t.prototype.htmlEncode=function(t){return t.replace(/&/g,"&amp;").replace(/"/g,"&quot;").replace(/'/g,"&#39;").replace(/</g,"&lt;").replace(/>/g,"&gt;")},t.\u0275fac=function(e){return new(e||t)},t.\u0275cmp=s.Kb({type:t,selectors:[["app-content-items-entity"]],decls:3,vars:3,consts:[[3,"matTooltip"],["class","more-entities",4,"ngIf"],[1,"more-entities"]],template:function(t,e){1&t&&(s.Wb(0,"div",0),s.Oc(1,I,2,1,"span",1),s.Qc(2),s.Vb()),2&t&&(s.oc("matTooltip",e.encodedValue),s.Bb(1),s.oc("ngIf",e.entities),s.Bb(1),s.Sc(" ",e.encodedValue,"\n"))},directives:[f.a,m.m],styles:[".more-entities[_ngcontent-%COMP%]{padding:0 8px;border-radius:10px;border:1px solid rgba(29,39,61,.44);-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none}"]}),t}(),V=n("CeOT"),S=n("0ELX"),W=n("JzAw"),x=n("50eG"),M=n("QjRa"),w=n("uuTa"),O=n("BLjT"),D=n("S36y"),N=n("55Ui"),Q=n("GTfO"),G=n("Xi8o"),R=n("2pW/"),A=n("9HSk"),F=n("G6Ml");n.d(e,"ContentItemsComponent",(function(){return _}));var _=function(){function t(t,e,n,a,r,c,s,u){this.dialogRef=t,this.contentTypesService=e,this.router=n,this.route=a,this.contentItemsService=r,this.entitiesService=c,this.contentExportService=s,this.snackBar=u,this.modules=l.a,this.gridOptions=Object(i.a)(Object(i.a)({},M.a),{frameworkComponents:{pubMetaFilterComponent:p,booleanFilterComponent:V.a,idFieldComponent:W.a,contentItemsStatusComponent:v,contentItemsActionsComponent:C,contentItemsEntityComponent:k}}),this.subscription=new o.a,this.hasChild=!!this.route.snapshot.firstChild,this.contentTypeStaticName=this.route.snapshot.paramMap.get("contentTypeStaticName")}return t.prototype.ngOnInit=function(){var t=this;this.fetchContentType(),this.fetchItems(),this.refreshOnChildClosed(),this.contentItemsService.getColumns(this.contentTypeStaticName).subscribe((function(e){var n;t.columnDefs=t.buildColumnDefs(e),null===(n=t.gridApi)||void 0===n||n.setColumnDefs(t.columnDefs);var a=function(t){var e,n;if(t){var a;"="===t.charAt(t.length-1)&&(t=atob(t));try{a=JSON.parse(t)}catch(p){console.error("Can't parse JSON with filters from url:",t)}if(a){var o={};(a.IsPublished||a.IsMetadata)&&(o.Status=d={filterType:"pub-meta",published:a.IsPublished?a.IsPublished:"",metadata:a.IsMetadata?a.IsMetadata:""});var r=Object.keys(a);try{for(var l=Object(i.i)(r),c=l.next();!c.done;c=l.next()){var s=c.value;if("IsPublished"!==s&&"IsMetadata"!==s){var u=a[s];if("string"==typeof u)o[s]=d={filterType:"text",type:"equals",filter:u};else if("number"==typeof u)o[s]=d={filterType:"number",type:"equals",filter:u,filterTo:null};else if(typeof u==typeof!0){var d={filterType:"boolean",filter:u.toString()};o[s]=d}}}}catch(m){e={error:m}}finally{try{c&&!c.done&&(n=l.return)&&n.call(l)}finally{if(e)throw e.error}}return o}}}(sessionStorage.getItem(S.h));a&&(Object(x.a)("Will try to apply filter:",a),t.gridApi.setFilterModel(a))}))},t.prototype.ngOnDestroy=function(){this.subscription.unsubscribe()},t.prototype.onGridReady=function(t){this.gridApi=t.api,this.columnDefs&&this.gridApi.setColumnDefs(this.columnDefs)},t.prototype.fetchContentType=function(){var t=this;this.contentTypesService.retrieveContentType(this.contentTypeStaticName).subscribe((function(e){t.contentType=e}))},t.prototype.fetchItems=function(){var t=this;this.contentItemsService.getAll(this.contentTypeStaticName).subscribe((function(e){t.items=e}))},t.prototype.editItem=function(t){var e=null==t?void 0:t.data,n={items:[null==e?{ContentTypeName:this.contentTypeStaticName}:{EntityId:e.Id}]},i=Object(w.a)(n);this.router.navigate(["edit/"+i],{relativeTo:this.route})},t.prototype.exportContent=function(){var t=this.gridApi.getFilterModel(),e=Object.keys(t).length>0,n=[];e&&this.gridApi.forEachNodeAfterFilterAndSort((function(t){n.push(t.data.Id)})),this.router.navigate(["export/"+this.contentTypeStaticName+(n.length>0?"/"+n:"")],{relativeTo:this.route})},t.prototype.importItem=function(){this.router.navigate(["import"],{relativeTo:this.route})},t.prototype.addMetadata=function(){var t,e;if(confirm("This is a special operation to add an item which is metadata for another item. If you didn't understand that, this is not for you :). Continue?")){var n=Object.keys(c.a.metadata),a=n.map((function(t){return c.a.metadata[t].type})),o=parseInt(prompt("What kind of assignment do you want?"+n.map((function(t){return"\n"+c.a.metadata[t].type+": "+c.a.metadata[t].target})),c.a.metadata.entity.type.toString()),10);if(!o)return alert("No target type entered. Cancelled");if(!a.includes(o))return alert("Invalid target type. Cancelled");var r=prompt("What key do you want?");if(!r)return alert("No key entered. Cancelled");var l,s=Object.keys(c.a.keyTypes),u=s.map((function(t){return c.a.keyTypes[t]})),d=prompt("What key type do you want?"+s.map((function(t){return"\n"+c.a.keyTypes[t]})),c.a.keyTypes.number);if(!d)return alert("No key type entered. Cancelled");if(!u.includes(d))return alert("Invalid key type. Cancelled");if(d===c.a.keyTypes.number&&!parseInt(r,10))return alert("Key type number and key don't match. Cancelled");try{for(var p=Object(i.i)(n),m=p.next();!m.done;m=p.next()){var b=m.value;o===c.a.metadata[b].type&&(l=c.a.metadata[b].target)}}catch(y){t={error:y}}finally{try{m&&!m.done&&(e=p.return)&&e.call(p)}finally{if(t)throw t.error}}var f={items:[{ContentTypeName:this.contentTypeStaticName,For:Object(i.a)(Object(i.a)(Object(i.a)({Target:l},d===c.a.keyTypes.guid&&{Guid:r}),d===c.a.keyTypes.number&&{Number:parseInt(r,10)}),d===c.a.keyTypes.string&&{String:r})}]},h=Object(w.a)(f);this.router.navigate(["edit/"+h],{relativeTo:this.route})}},t.prototype.debugFilter=function(){console.warn("Current filter:",this.gridApi.getFilterModel()),alert("Check console for filter information")},t.prototype.closeDialog=function(){this.dialogRef.close()},t.prototype.refreshOnChildClosed=function(){var t=this;this.subscription.add(this.router.events.pipe(Object(r.a)((function(t){return t instanceof a.b}))).subscribe((function(e){var n=t.hasChild;t.hasChild=!!t.route.snapshot.firstChild,!t.hasChild&&n&&t.fetchItems()})))},t.prototype.buildColumnDefs=function(t){var e,n,a=[{headerName:"ID",field:"Id",width:70,headerClass:"dense",cellClass:"id-action no-padding no-outline",cellRenderer:"idFieldComponent",sortable:!0,filter:"agTextColumnFilter",valueGetter:this.idValueGetter},{headerName:"Status",field:"Status",width:80,headerClass:"dense",cellClass:"no-outline",filter:"pubMetaFilterComponent",cellRenderer:"contentItemsStatusComponent",valueGetter:this.valueGetterStatus},{headerName:"Item (Entity)",field:"_Title",flex:2,minWidth:250,cellClass:"primary-action highlight",sortable:!0,filter:"agTextColumnFilter",onCellClicked:this.editItem.bind(this)},{cellClass:"secondary-action no-padding",width:120,cellRenderer:"contentItemsActionsComponent",cellRendererParams:{onClone:this.clone.bind(this),onExport:this.export.bind(this),onDelete:this.delete.bind(this)}},{headerName:"Stats",headerTooltip:"Used by others / uses others",field:"_Used",width:70,headerClass:"dense",cellClass:"no-outline",sortable:!0,filter:"agTextColumnFilter",valueGetter:this.valueGetterUsage}];try{for(var o=Object(i.i)(t),r=o.next();!r.done;r=o.next()){var l=r.value,c={headerName:l.StaticName,field:l.StaticName,flex:2,minWidth:250,cellClass:"no-outline",sortable:!0};switch(l.Type){case"Entity":try{c.allowMultiValue=l.Metadata.Entity.AllowMultiValue}catch(s){c.allowMultiValue=!0}c.cellRenderer="contentItemsEntityComponent",c.valueGetter=this.valueGetterEntityField,c.filter="agTextColumnFilter";break;case"DateTime":try{c.useTimePicker=l.Metadata.DateTime.UseTimePicker}catch(s){c.useTimePicker=!1}c.valueGetter=this.valueGetterDateTime,c.filter="agTextColumnFilter";break;case"Boolean":c.valueGetter=this.valueGetterBoolean,c.filter="booleanFilterComponent";break;case"Number":c.filter="agNumberColumnFilter";break;default:c.filter="agTextColumnFilter"}a.push(c)}}catch(u){e={error:u}}finally{try{r&&!r.done&&(n=o.return)&&n.call(o)}finally{if(e)throw e.error}}return a},t.prototype.clone=function(t){var e={items:[{ContentTypeName:this.contentTypeStaticName,DuplicateEntity:t.Id}]},n=Object(w.a)(e);this.router.navigate(["edit/"+n],{relativeTo:this.route})},t.prototype.export=function(t){this.contentExportService.exportEntity(t.Id,this.contentTypeStaticName,!0)},t.prototype.delete=function(t){var e=this;confirm("Delete '"+t._Title+"' ("+t._RepositoryId+")?")&&(this.snackBar.open("Deleting..."),this.entitiesService.delete(this.contentTypeStaticName,t._RepositoryId,!1).subscribe({next:function(){e.snackBar.open("Deleted",null,{duration:2e3}),e.fetchItems()},error:function(n){e.snackBar.dismiss(),confirm(n.error.ExceptionMessage+"\n\nDo you want to force delete '"+t._Title+"' ("+t._RepositoryId+")?")&&(e.snackBar.open("Deleting..."),e.entitiesService.delete(e.contentTypeStaticName,t._RepositoryId,!0).subscribe((function(){e.snackBar.open("Deleted",null,{duration:2e3}),e.fetchItems()})))}}))},t.prototype.idValueGetter=function(t){var e=t.data;return"ID: "+e.Id+"\nRepoID: "+e._RepositoryId+"\nGUID: "+e.Guid},t.prototype.valueGetterStatus=function(t){var e=t.data;return{published:e.IsPublished,metadata:!!e.Metadata}},t.prototype.valueGetterUsage=function(t){var e=t.data;return e._Used+" / "+e._Uses},t.prototype.valueGetterEntityField=function(t){var e=t.data[t.colDef.field];return 0===e.length?null:e.map((function(t){return t.Title}))},t.prototype.valueGetterDateTime=function(t){var e=t.data[t.colDef.field];return e?t.colDef.useTimePicker?e.substring(0,19).replace("T"," "):e.substring(0,10):null},t.prototype.valueGetterBoolean=function(t){var e=t.data[t.colDef.field];return typeof e!=typeof!0?null:e.toString()},t.\u0275fac=function(e){return new(e||t)(s.Qb(O.g),s.Qb(D.a),s.Qb(a.c),s.Qb(a.a),s.Qb(N.a),s.Qb(Q.a),s.Qb(G.a),s.Qb(R.b))},t.\u0275cmp=s.Kb({type:t,selectors:[["app-content-items"]],decls:27,vars:4,consts:[[1,"nav-component-wrapper"],["mat-dialog-title",""],[1,"dialog-title-box"],["mat-icon-button","","matTooltip","Close dialog",3,"click"],[1,"grid-wrapper"],[1,"ag-theme-material",3,"rowData","modules","gridOptions","gridReady"],[1,"actions-box"],["mat-icon-button","","matTooltip","Export",3,"click"],["mat-icon-button","","matTooltip","Import",3,"click"],["mat-icon-button","","matTooltip","Add metadata",3,"click"],["mat-icon-button","","matTooltip","Debug filter",3,"click"],["mat-fab","","mat-elevation-z24","","matTooltip","Add item",1,"grid-fab",3,"click"]],template:function(t,e){1&t&&(s.Wb(0,"div",0),s.Wb(1,"div",1),s.Wb(2,"div",2),s.Wb(3,"div"),s.Qc(4),s.Vb(),s.Wb(5,"button",3),s.ec("click",(function(){return e.closeDialog()})),s.Wb(6,"mat-icon"),s.Qc(7,"close"),s.Vb(),s.Vb(),s.Vb(),s.Vb(),s.Rb(8,"router-outlet"),s.Wb(9,"div",4),s.Wb(10,"ag-grid-angular",5),s.ec("gridReady",(function(t){return e.onGridReady(t)})),s.Vb(),s.Wb(11,"div",6),s.Wb(12,"button",7),s.ec("click",(function(){return e.exportContent()})),s.Wb(13,"mat-icon"),s.Qc(14,"cloud_download"),s.Vb(),s.Vb(),s.Wb(15,"button",8),s.ec("click",(function(){return e.importItem()})),s.Wb(16,"mat-icon"),s.Qc(17,"cloud_upload"),s.Vb(),s.Vb(),s.Wb(18,"button",9),s.ec("click",(function(){return e.addMetadata()})),s.Wb(19,"mat-icon"),s.Qc(20,"local_offer"),s.Vb(),s.Vb(),s.Wb(21,"button",10),s.ec("click",(function(){return e.debugFilter()})),s.Wb(22,"mat-icon"),s.Qc(23,"filter_list"),s.Vb(),s.Vb(),s.Vb(),s.Wb(24,"button",11),s.ec("click",(function(){return e.editItem(null)})),s.Wb(25,"mat-icon"),s.Qc(26,"add"),s.Vb(),s.Vb(),s.Vb(),s.Vb()),2&t&&(s.Bb(4),s.Sc("",null==e.contentType?null:e.contentType.Name," Data"),s.Bb(6),s.oc("rowData",e.items)("modules",e.modules)("gridOptions",e.gridOptions))},directives:[O.h,A.a,f.a,b.a,a.e,F.a],styles:[".actions-box[_ngcontent-%COMP%]{margin-right:66px;margin-left:8px;display:flex}"]}),t}()}}]);
//# sourceMappingURL=https://sources.2sxc.org/11.04.01/ng-edit/content-items-component.882aa2eb1f0b0f297097.js.map