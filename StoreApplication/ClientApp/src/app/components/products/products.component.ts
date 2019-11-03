import { Component, OnInit } from '@angular/core';
import { Product } from '../../models/Product';
import { Purchase } from '../../models/Purchase';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Sort } from '@angular/material/sort';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
  public products: Product[];
  public purchaseDone: boolean;
  public purchaseSuccess: boolean;
  public errorMessage: string;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    // get list of products
    this.listProducts();

    this.purchaseDone = false;
    this.purchaseSuccess = false;
  }

  // function for sorting table
  public sortData(sort: Sort) {
    // if no sort is active, restore the original list
    if (!sort.active || sort.direction === '') {
      this.listProducts();
      return;
    }

    // otherwise, sort the list by price, toggling between ascending and descending
    this.products = this.products.sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      return (a.price < b.price ? -1 : 1) * (isAsc ? 1 : -1);
    });
  }

  // create a new purchase for this user, save to DB
  public doPurchase(productId: number, userId: number) {
    let tmpPurchase: Purchase = new Purchase();
    tmpPurchase.userId = userId;
    tmpPurchase.productId = productId;

    this.http.post("api/purchases/create", tmpPurchase, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      // success, toggle message on screen
      this.purchaseDone = true;
      this.purchaseSuccess = true;
      }, err => {
      // something went wrong, display error
      this.setError(err.error.message);
    });
  }

  // helper to get the user ID from local storage
  public getUser() {
    return localStorage["userId"];
  }

  // get the products from the DB
  private listProducts() {
    this.http.get("api/products/", {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      // list products on screen
      this.products = new Array();
      this.parseJSON(response);
    }, err => {
      // something went wrong, display error
      this.setError(err.error.message);
    });
  }

  // parse JSON into 
  private parseJSON(arr: Object) {
    for (let i in arr) {
      let tmpProduct = new Product();
      tmpProduct.id = arr[i]["id"];
      tmpProduct.name = arr[i]["name"];
      tmpProduct.price = arr[i]["price"];
      this.products.push(tmpProduct);
    }
  }

  // set error message on the screen
  private setError(err: string) {
    this.errorMessage = err;
    this.purchaseDone = true;
    this.purchaseSuccess = false;
  }
}
