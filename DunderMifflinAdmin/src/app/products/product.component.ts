import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Product } from '../Product';
import { ProductService } from './product.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {

  products? : Product[]

  searchForm!: FormControl

  constructor(private service:ProductService){}

  ngOnInit(): void {
    this.searchForm=new FormControl('')
  }

  search(name:string){
    this.service.getProductByName(name).subscribe({
      next: (result)=>{
        this.products=result;
        console.log(result);
      },
      error: (error)=>{
        console.log(error);
      }
  });

}

}
