import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FormBuilder,FormGroup} from '@angular/forms';
import { Router } from '@angular/router';
@Component({
  selector: 'create-book-form',
  templateUrl: './create-book-form.html'
})
export class CreateBookFormComponenet implements OnInit {
    Categories:Observable<CategoryItem[]>;
    Authors:Observable<AuthorItem[]>;
    BookCreationForm:FormGroup;
    
    constructor (private httpClient:HttpClient,
                private formBuilder:FormBuilder,
                private router:Router,
                 @Inject('BASE_URL') private baseUrl: string){
       this.Categories= httpClient.get<CategoryItem[]>(baseUrl + 'Data/categories');
       this.Authors= httpClient.get<AuthorItem[]>(baseUrl + 'Data/authors');
      
    }
  ngOnInit(): void {
    this.CreateForm();
  }
    CreateForm(){
      this.BookCreationForm=this.formBuilder.group({
        Title:'',
        Description:'',
        Category:0,
        Authors:[]
      });
    }
    CategorySelected(event){
      console.log(event.target.value);
      this.BookCreationForm.patchValue({Category:event.target.value})
    }
    AuthorSelected(event){
      this.BookCreationForm.patchValue({AuthorSelected:event.target.value})
    }
    onSubmit(){
      this.httpClient
      .post(this.baseUrl + 'Books',this.BookCreationForm.value)
      .subscribe(()=>this.router.navigateByUrl("/"));
    }
}
interface CategoryItem{
    id:number;
    category:string;
}
interface AuthorItem{
  id:number;
  author:string;
}