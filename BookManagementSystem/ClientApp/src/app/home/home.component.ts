import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ColumnMode, SelectionType } from '@swimlane/ngx-datatable';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  page = { limit: 2,count: 1,offset: 0,orderBy: 'id',orderDir: 'desc'};
  rows:Book[]
  http: HttpClient;
  baseUrl: string;
  singleSelectionType:SelectionType;
  router:Router;
 
  constructor(http: HttpClient,router:Router, @Inject('BASE_URL') baseUrl: string) {
    this.http=http;
    this.baseUrl=baseUrl;
    this.singleSelectionType=SelectionType.single;
    this.router=router;
  }
 
  ngOnInit(): void {
    this.pageCallback({ offset: 0 });
  }

  pageCallback(arg: { offset: number; }) {
    this.page.offset = arg.offset;
    this.LoadData();
  }
  onSelect(arg:{ selected:Book[] }) {
    console.log(arg);
    this.router.navigate(["/counter"],{queryParams:{event:arg.selected[0].id}});
  }
  LoadData() {
    const params = new HttpParams()
      .set('page', `${this.page.offset+1}`)
      .set('pageSize', `${this.page.limit}`);

    this.http.get<Book[]>(this.baseUrl + 'Books',{params}).subscribe(books=>{
      this.rows=books;
      this.page.count=10;
    });
  }
 
  columns = [{ prop: 'id' }, { name: 'title' }, { name: 'description' }, { name: 'category' }, { name: 'authors' }];
}
interface Book{
   id:string ;
   title:string  ;
   description:string  ;
   category:string  ;
   authors:string[]  ;
}
