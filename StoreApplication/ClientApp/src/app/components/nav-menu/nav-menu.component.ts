import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  constructor(private router: Router, private cookieService: CookieService) { }

  ngOnInit() {
  }

  public logOut() {
    // remove local storage and cookie items, redirect to login
    localStorage.removeItem("jwt");
    localStorage.removeItem("userId");
    this.cookieService.delete("UserId");
    this.router.navigate(["/"]);
  }
}
