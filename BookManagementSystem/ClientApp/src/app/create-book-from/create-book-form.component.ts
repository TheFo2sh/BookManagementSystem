import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FormBuilder,FormGroup} from '@angular/forms';
@Component({
  selector: 'create-book-form',
  templateUrl: './create-book-form.html'
})
export class CreateBookFormComponenet implements OnInit {
    Categories:Observable<CategoryItem[]>;
    Authors:Observable<AuthorItem[]>;
    BookCreationForm:FormGroup;
    
    constructor (private httpClient:HttpClient ,private formBuilder:FormBuilder ,@Inject('BASE_URL') baseUrl: string){
       this.Categories= httpClient.get<CategoryItem[]>(baseUrl + 'Data/categories');
       this.Authors= httpClient.get<AuthorItem[]>(baseUrl + 'Data/authors');
      
    }
  ngOnInit(): void {
    this.CreateForm();
  }
    CreateForm(){
      this.BookCreationForm=this.formBuilder.group({
        title:'',
        description:'',
        categoryId:0,
        authorId:0
      });
    }
    CategorySelected(event){
      console.log(event.target.value);
      this.BookCreationForm.patchValue({categoryId:event.target.value})
    }
    AuthorSelected(event){
      this.BookCreationForm.patchValue({AuthorSelected:event.target.value})
    }
    onSubmit(){
      console.log(this.BookCreationForm);
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