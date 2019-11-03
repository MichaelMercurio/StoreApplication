import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.css']
})
export class CreateComponent implements OnInit {
  public ownerForm: FormGroup;
  public createDone: boolean;
  public createError: boolean;
  public errorMessage: string;

  constructor(private router: Router, private http: HttpClient) { }

  ngOnInit() {
    this.ownerForm = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      email: new FormControl('', [Validators.required, Validators.maxLength(255)]),
      password: new FormControl('', [Validators.required, Validators.maxLength(100)])
    });
    this.createDone = false;
    this.createError = false;
    this.errorMessage = "";
  }

  // call create customer API
  public create(ownerFormValue) {
    // if form is filled in, call API. Otherwise, display error
    if (this.ownerForm.valid) {
      this.executeCreate(ownerFormValue);
    }
    else {
      if (this.ownerForm.controls.name.errors && this.ownerForm.controls.email.errors && this.ownerForm.controls.password.errors) {
        this.setError("Please enter your name, email address and password.");
      }
      else if (this.ownerForm.controls.email.errors) {
        this.setError("Invalid email address.");
      }
      else {
        this.setError("Please enter your password.");
      }
    }
  }

  // helper function to display error messages
  private setError(err: string) {
    this.errorMessage = err;
    this.createDone = true;
    this.createError = true;
  }

  // call create customer API
  private executeCreate(ownerFormValue) {
    let credentials: string = JSON.stringify(ownerFormValue);
    this.http.post("api/customers/create", credentials, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      // it worked, set local storage values and redirect to products
      let token = (<any>response).token;
      let id = (<any>response).id;
      localStorage.setItem("jwt", token);
      localStorage.setItem("userId", id);
      this.router.navigate(["/products"]);
    }, err => {
      // something went wrong, display error message
      this.setError(err.error.message);
    });
  }

}
