import {
  HttpClient,
  HttpErrorResponse,
  HttpResponse,
} from '@angular/common/http';
import { Inject, Injectable, InjectionToken, PLATFORM_ID } from '@angular/core';
import { catchError, firstValueFrom, map, of } from 'rxjs';

import {
  UserSignup,
  UserLogin,
  UserOutput,
  InnerUser,
} from '../interfaces/user';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) private platformId: InjectionToken<object>
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }
  private isBrowser!: boolean;

  private _token!: string | null;
  public get token(): string | null {
    if (!this._token && this.isBrowser) {
      this._token = localStorage.getItem('token');
    }
    return this._token;
  }
  public set token(value: string | null) {
    if (this.isBrowser) {
      localStorage.setItem('token', value!);
    }
    this._token = value;
  }
  private _user!: InnerUser | null;
  public get user(): InnerUser | null {
    if ((!this._user || !this._token) && this.isBrowser) {
      this._user = {
        name: localStorage.getItem('name')!,
        role: localStorage.getItem('role')!,
      };
    }
    return this._user;
  }
  public set user(value: InnerUser | null) {
    if (this.isBrowser) {
      localStorage.setItem('name', value!.name);
      localStorage.setItem('role', value!.role);
    }
    this._user = value;
  }

  clearToken() {
    this._token = null;
    this._user = null;
    if (this.isBrowser) {
      localStorage.removeItem('token');
      localStorage.removeItem('name');
      localStorage.removeItem('role');
    }
  }

  signUpAsAdmin(user: UserSignup) {
    return firstValueFrom(
      this.http
        .post('api/User/admin', user, {
          observe: 'response',
        })
        .pipe(
          catchError((err: HttpErrorResponse) =>
            of(
              new HttpResponse({
                ...err,
                url: err.url!,
              })
            )
          )
        )
    );
  }

  signUpAsWorker(user: UserSignup) {
    return firstValueFrom(
      this.http
        .post('api/User', user, {
          observe: 'response',
        })
        .pipe(
          catchError((err: HttpErrorResponse) =>
            of(
              new HttpResponse({
                ...err,
                url: err.url!,
              })
            )
          )
        )
    );
  }

  isAuthenticated(): boolean {
    return !!this.token;
  }

  login(user: UserLogin) {
    return firstValueFrom(
      this.http
        .post('api/User/login', user, {
          observe: 'response',
        })
        .pipe(
          catchError((err: HttpErrorResponse) =>
            of(
              new HttpResponse({
                ...err,
                url: err.url!,
              })
            )
          ),
          map((res: HttpResponse<any>) => {
            const body: UserOutput = res.body;
            this.updateLocalStorage(body);
            return res;
          })
        )
    );
  }
  updateLocalStorage(body: UserOutput) {
    this.token = body.token;
    this.user = {
      name: body.firstName + ' ' + body.lastName,
      role: body.roles[0],
    };
  }
}
