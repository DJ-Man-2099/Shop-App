import {
  HttpClient,
  HttpErrorResponse,
  HttpResponse,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SimpleHttpClientService {
  constructor(private http: HttpClient) {}

  get<t>(url: string): Observable<HttpResponse<t>> {
    return this.http
      .get<t>(url, {
        observe: 'response',
      })
      .pipe(
        catchError((err: HttpErrorResponse) =>
          of(
            new HttpResponse<t>({
              ...err,
              url: err.url!,
            })
          )
        )
      );
  }
  post<t>(url: string, data: any): Observable<HttpResponse<t>> {
    return this.http
      .post<t>(url, data, {
        observe: 'response',
      })
      .pipe(
        catchError((err: HttpErrorResponse) =>
          of(
            new HttpResponse<t>({
              ...err,
              url: err.url!,
            })
          )
        )
      );
  }
  patch<t>(url: string, data: any): Observable<HttpResponse<t>> {
    return this.http
      .patch<t>(url, data, {
        observe: 'response',
      })
      .pipe(
        catchError((err: HttpErrorResponse) =>
          of(
            new HttpResponse<t>({
              ...err,
              url: err.url!,
            })
          )
        )
      );
  }
  delete<t>(url: string): Observable<HttpResponse<t>> {
    return this.http
      .delete<t>(url, {
        observe: 'response',
      })
      .pipe(
        catchError((err: HttpErrorResponse) =>
          of(
            new HttpResponse<t>({
              ...err,
              url: err.url!,
            })
          )
        )
      );
  }
}
