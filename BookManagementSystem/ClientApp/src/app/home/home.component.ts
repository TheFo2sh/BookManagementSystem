import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  rows:Book[]
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Book[]>(baseUrl + 'Books').subscribe(books=>this.rows=books);
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
