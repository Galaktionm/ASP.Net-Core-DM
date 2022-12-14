import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '../Product';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private http:HttpClient) { }

  getProductByName(name:string):Observable<Product[]>{
    var products=this.http.get<Product[]>("https://localhost:7234/api/products/"+name);
    return products;
  }
}


export class ProductRequest {
  
  name:string

  constructor(name:string){
    this.name=name;
  }

}