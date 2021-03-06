import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ColumnMode, SelectionType } from '@swimlane/ngx-datatable';
import {Router, ActivatedRoute, Params} from '@angular/router';
import { style } from '@angular/animations';

@Component({
  selector: 'book-events-component',
  templateUrl: './book-events.html'
})
export class BookEventsComponent implements OnInit {
  page = { limit: 2,count: 1,offset: 0,orderBy: 'id',orderDir: 'desc'};
  rows:Event[]
  http: HttpClient;
  baseUrl: string;
  singleSelectionType:SelectionType;
  activatedRoute: ActivatedRoute;
  event: string;
  filter={event:null};
  eventsTypes=[
                {id:'TitleChanged',description:'Title Changed'},
                {id:'DescriptionChanged',description:'Description Changed'},
                {id:'CategoryChanged',description:'Category Changed'},
                {id:'AuthorAdded',description:'Author Added'},
                {id:'AuthorRemoved',description:'Author Removed'}];
  constructor(http: HttpClient, activatedRoute: ActivatedRoute, @Inject('BASE_URL') baseUrl: string) {
    this.http=http;
    this.baseUrl=baseUrl;
    this.singleSelectionType=SelectionType.single;
    this.activatedRoute=activatedRoute;
  }
 
  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(param=>{
        this.event=param["event"];
        this.pageCallback({ offset: 0 });

      });
  }
  eventsFilterSelectionChanged(args:{value:{id:string , description:string}}){
    if(args==null) 
      this.filter.event=null;
    else
      this.filter.event=args.value.id;
    
     this.LoadData();

  }
  pageCallback(arg: { offset: number; }) {
    this.page.offset = arg.offset;
    this.LoadData();
  }
  onSelect(arg:{ selected:Event[] }) {
    console.log(arg);
    //alert(arg.selected[0].title);
  }
  LoadData() {
    let params = new HttpParams()
      .set('page', `${this.page.offset+1}`)
      .set('pageSize', `${this.page.limit}`);

      if(this.filter.event!=null){
        params=params.set('Type',this.filter.event)
      }
    this.http.get<Event[]>(this.baseUrl + 'Books/'+this.event+'/events',{params}).subscribe(books=>{
      this.rows=books;
      this.page.count=10;
    });
  }
 
  columns = [ { name: 'event' },
              { prop:'eventDescription', name: 'description' },
              { name: 'createdDate' }];
}

interface Event{
    event: string;
    eventDescription: string;
    createdDate: Date;
}

