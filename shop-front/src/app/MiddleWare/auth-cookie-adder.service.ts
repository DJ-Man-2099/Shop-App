import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { backendServer } from '../../constants';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../Services/authentication.service';

@Injectable({
  providedIn: 'root',
})
export class AuthCookieAdderService implements HttpInterceptor {
  private backendServer = backendServer;

  constructor(private authService: AuthenticationService) {}
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (!this.authService.token) {
      return next.handle(req);
    }
    const baseReq = req.clone({
      headers: req.headers.append(
        'Authorization',
        `Bearer ${this.authService.token}`
      ),
    });
    return next.handle(baseReq);
  }
}
