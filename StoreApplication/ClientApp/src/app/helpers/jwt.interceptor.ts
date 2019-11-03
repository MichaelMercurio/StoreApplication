import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor() { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // add jwt token to request if available
    let token = localStorage.getItem("jwt");

    if (token) {
      request = request.clone({
        headers: request.headers.set("Authorization", `Bearer ${token}`)
      });
    }

    return next.handle(request);
  }
}
