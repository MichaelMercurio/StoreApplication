import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public ownerForm: FormGroup;
  public loginDone: boolean;
  public loginError: boolean;
  public errorMessage: string;

  constructor(private router: Router, private http: HttpClient) { }

  ngOnInit() {
    this.ownerForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.maxLength(255), Validators.email]),
      password: new FormControl('', [Validators.required, Validators.maxLength(100)])
    });
    this.loginDone = false;
    this.loginError = false;
    this.errorMessage = "";
  }

  // log the user in
  public login(ownerFormValue) {
    // if the form is valid, try to log in. Otherwise, display an error
    if (this.ownerForm.valid) {
      this.executeLogin(ownerFormValue);
    }
    else {
      if (this.ownerForm.controls.email.errors) {
        this.setError("Invalid email address.");
        return;
      }
      this.setError("Please enter your email address and password.");
    }
  }

  // helper function to display an error message on the screen
  private setError(err: string) {
    this.errorMessage = err;
    this.loginDone = true;
    this.loginError = true;
  }

  // call the login API
  private executeLogin(ownerFormValue) {
    let credentials: string = JSON.stringify(ownerFormValue);
    this.http.post("api/customers/login", credentials, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      // it worked, set local storage values and redirect to products page
      let token = (<any>response).token;
      let id = (<any>response).id;
      localStorage.setItem("jwt", token);
      localStorage.setItem("userId", id);
      this.router.navigate(["/products"]);
    }, err => {
      // something went wrong, display error message on screen
      this.setError(err.error.message);
    });
  }
}
