import { Component, OnInit } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Purchase } from '../../models/Purchase';

@Component({
  selector: 'app-purchases',
  templateUrl: './purchases.component.html',
  styleUrls: ['./purchases.component.css']
})
export class PurchasesComponent implements OnInit {
  public purchases: Purchase[];
  public cancelDone: boolean;
  public cancelSuccess: boolean;
  public errorMessage: string;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.listPurchases();

    this.cancelDone = false;
    this.cancelSuccess = false;
  }

  // call delete API
  public doCancel(purchaseId: number) {
    this.http.delete(`api/purchases/${purchaseId}`, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      // everything's good, refresh screen
      this.cancelDone = true;
      this.cancelSuccess = true;
      this.listPurchases();
      }, err => {
        // something went wrong, display error message
        this.setError(err.error.message);
    });
  }

  // get purchases for user from DB
  private listPurchases() {
    this.http.get(`api/purchases/user/${localStorage['userId']}`, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      // list them
      let list: JSON = (<any>response).list;
      this.purchases = new Array();
      this.parseJSON(list);
      }, err => {
      // something went wrong, display error message
      this.setError(err.error.message);
    });
  }

  // parse JSON into purchase objects and store in the purchases array
  private parseJSON(arr: JSON) {
    for (let i in arr) {
      let tmpPurchase = new Purchase();
      tmpPurchase.id = arr[i]["id"];
      tmpPurchase.userId = arr[i]["userId"];
      tmpPurchase.productId = arr[i]["productId"];
      tmpPurchase.productName = arr[i]["productName"];
      this.purchases.push(tmpPurchase);
    }
  }

  // set error message on the screen
  private setError(err: string) {
    this.errorMessage = err;
    this.cancelDone = true;
    this.cancelSuccess = false;
  }
}
