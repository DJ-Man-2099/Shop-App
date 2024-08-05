import { Injectable } from '@angular/core';
import { backendServer } from '../../constants';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class HttpBaseAdderService implements HttpInterceptor {
  private backendServer = backendServer;

  constructor() {}
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const baseReq = req.clone({
      url: `${this.backendServer}/${req.url}`,
    });
    return next.handle(baseReq);
  }
}
