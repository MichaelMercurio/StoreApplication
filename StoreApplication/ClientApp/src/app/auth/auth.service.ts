import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(public jwtHelper: JwtHelperService) { }

  public isAuthenticated(): boolean {
    const token: string = localStorage.getItem('jwt');

    // if the token's expired, the user isn't authenticated
    return !this.jwtHelper.isTokenExpired(token);
  }
}
